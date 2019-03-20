using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Gios.Pdf;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Configuration;
//done by senthil
public partial class hrsalaryincomepf : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet ds11 = new DataSet();
    DAccess2 da = new DAccess2();
    DAccess2 d2 = new DAccess2();
    ReuasableMethods rs = new ReuasableMethods();
    string college_code = "";
    string dte = "";
    string usercode = string.Empty;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            //string PageLogOut = "";
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("HRM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["HR"].Trim();
            //            Response.Redirect(redrURI, false);
            //            return;
            //        }
            //    }
            //}
            //if (Session["collegecode"] == null)
            //{
            //    string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
            //    Response.Redirect(redrURI, false);
            //    return;
            //}
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("HRMenuIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/HRMOD/HRMenuIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            college_code = Session["collegecode"].ToString();
            usercode = Session["usercode"].ToString();
            if (!IsPostBack)
            {
                //string str = "select PayMonth,PayMonthNum from HrPayMonths where College_Code='" + college_code + "' and selstatus='1' ";
                //ds = da.select_method_wo_parameter(str, "Text");
                //ddlfrommnth.DataSource = ds;
                //ddlfrommnth.DataTextField = "PayMonth";
                //ddlfrommnth.DataValueField = "PayMonthNum";
                //ddlfrommnth.DataBind();
                //ddlfrommnth.Items.Insert(0, "---Select---");
                //ddltomnth.Items.Insert(0, "---Select---");
                //year(dte);
                //    year1(dte);
                load_dept();
                design();
                loadstafftype();
                staff();
                load_batchyear();
                TBDEPT1.Attributes.Add("readonly", "readonly");
                TBDESIGN2.Attributes.Add("readonly", "readonly");
                TBST_NAME3.Attributes.Add("readonly", "readonly");
            }
        }
        catch (Exception ex) { }
    }
    
    public void year(string date)
    {
        //try
        //{
        //    string year = "";
        //    if (date.Trim() != "")
        //    {
        //        year = "select distinct year(To_Date) as year from HrPayMonths where PayMonthNum=" + date + " and  College_Code ='" + college_code + "'   order by year asc";
        //    }
        //    else
        //    {
        //        year = "select distinct year(To_Date) as year from HrPayMonths where College_Code ='" + college_code + "'  order by year asc";
        //    }
        //    ds11 = da.select_method_wo_parameter(year, "text");
        //    if (ds11.Tables[0].Rows.Count > 0)
        //    {
        //        ddlfrmyr.DataSource = ds11;
        //        ddlfrmyr.DataTextField = "year";
        //        ddlfrmyr.DataValueField = "year";
        //        ddlfrmyr.DataBind();
        //        ddltoyr.DataSource = ds11;
        //        ddltoyr.DataTextField = "year";
        //        ddltoyr.DataValueField = "year";
        //        ddltoyr.DataBind();
        //    }
        //}
        //catch (Exception ex)
        //{
        //    lblnorec.Visible = true;
        //    lblnorec.Text = ex.ToString();
        //}
    }
    
    void load_batchyear()
    {
        try
        {
            ddlfrmyr.Visible = true;
            ddltomnth.Visible = true;
            DataSet data = new DataSet();
            data.Clear();
         //   string query = "select distinct year(fdate) as year from monthlypay order by year desc ";

            string query = "select distinct year(hryear_start)as year from hryears order by year desc";

            data = da.select_method_wo_parameter(query, "text");
            if (data.Tables[0].Rows.Count > 0)
            {
                ddlfrmyr.DataSource = data.Tables[0];
                ddlfrmyr.DataTextField = "Year";
                ddlfrmyr.DataValueField = "year";
                ddlfrmyr.DataBind();
                ddltoyr.DataSource = data.Tables[0];
                ddltoyr.DataTextField = "Year";
                ddltoyr.DataValueField = "year";
                ddltoyr.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddlfromyear_selectchange(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            ddltoyr.Items.Clear();
            ddltoyr.Enabled = true;
            ddltomnth.SelectedIndex = 0;
            string year = "select distinct year(From_Date) as year from HrPayMonths where College_Code='" + college_code + "'  order by year asc";
            ds = da.select_method_wo_parameter(year, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string mon = ds.Tables[0].Rows[i]["year"].ToString();
                    if (ddlfrmyr.SelectedItem.Text.ToString() == mon)
                    {
                        for (int j = i; j < ds.Tables[0].Rows.Count; j++)
                        {
                            ddltoyr.Items.Insert(count, new ListItem(ds.Tables[0].Rows[j]["year"].ToString()));
                            count++;
                        }
                    }
                }
            }
            typegrid.Visible = false;
            Buttongen1.Visible = false;
            butgen.Visible = false;
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    public void load_dept()
    {
        try
        {
            string cmd = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"] + "') order by dept_name";
            DataSet ds = new DataSet();
            ds = da.select_method_wo_parameter(cmd, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklistdept.DataSource = ds;
                chklistdept.DataTextField = "dept_name";
                chklistdept.DataValueField = "dept_code";
                chklistdept.DataBind();
            }
            if (chklistdept.Items.Count > 0)
            {
                for (int i = 0; i < chklistdept.Items.Count; i++)
                {
                    chklistdept.Items[i].Selected = true;
                }
                TBDEPT1.Text = "Department(" + chklistdept.Items.Count + ")";
                chkdept.Checked = true;
            }
            else
            {
                TBDEPT1.Text = "--Select--";
                chkdept.Checked = false;
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    public void design()
    {
        try
        {
            int count = 0;
            chklistdesign.Items.Clear();
            string year = "select distinct desig_code,desig_name from desig_master where CollegeCode='" + college_code + "' ";
            ds = da.select_method_wo_parameter(year, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklistdesign.DataSource = ds;
                chklistdesign.DataTextField = "desig_name";
                chklistdesign.DataValueField = "desig_code";
                chklistdesign.DataBind();
            }
            for (int i = 0; i < chklistdesign.Items.Count; i++)
            {
                chklistdesign.Items[i].Selected = true;
                if (chklistdesign.Items[i].Selected == true)
                {
                    count += 1;
                }
                if (chklistdesign.Items.Count == count)
                {
                    chkdesign.Checked = true;
                }
            }
            if (chklistdesign.Items.Count > 0)
            {
                TBDESIGN2.Text = "Designation (" + chklistdesign.Items.Count + ")";
                chkdesign.Checked = true;
            }
            else
            {
                TBDESIGN2.Text = "---Select---";
                chkdesign.Checked = false;
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    public void staff()
    {
        try
        {
            string des = "";
            for (int i = 0; i < chklistdesign.Items.Count; i++)
            {
                if (chklistdesign.Items[i].Selected == true)
                {
                    if (des == "")
                    {
                        des = chklistdesign.Items[i].Value.ToString();
                    }
                    else
                    {
                        des = des + "'" + "," + "'" + chklistdesign.Items[i].Value.ToString();
                    }
                }
            }
            string dept = "";
            for (int i = 0; i < chklistdept.Items.Count; i++)
            {
                if (chklistdept.Items[i].Selected == true)
                {
                    if (dept == "")
                    {
                        dept = chklistdept.Items[i].Value.ToString();
                    }
                    else
                    {
                        dept = dept + "'" + "," + "'" + chklistdept.Items[i].Value.ToString();
                    }
                }
            }
            college_code = Session["collegecode"].ToString();
            chkliststname.Items.Clear();
            string year = "";
            if (dept != "" && des != "")
            {
                if (cb_relived.Checked == false)
                {
                    year = "Select distinct m.Staff_code,Staff_name from staffmaster m,stafftrans t where resign=0 and settled=0 and m.staff_code = t.staff_code and t.desig_code in('" + des + "') and t.latestrec = 1 and dept_code in ('" + dept + "') and college_code='" + college_code + "' order by staff_name ";
                }
                if (cb_relived.Checked == true)//delsi 2807
                {
                    DateTime frm_date = new DateTime();
                    DateTime to_date = new DateTime();
                    string getfromdate = string.Empty;
                    string gettodate = string.Empty;

                    string itsetting = d2.GetFunction("select linkvalue from New_InsSettings where LinkName='IT Calculation Settings' and college_code='" + Convert.ToString(college_code) + "'");
                    if (itsetting.Trim() != "0")
                    {
                        string frommonth = string.Empty;
                        string tomonth = string.Empty;
                        string fromyear = string.Empty;
                        string toyear = string.Empty;
                        string[] linkvalue = itsetting.Split('-');
                        if (linkvalue.Length > 0)
                        {
                            frommonth = linkvalue[0].Split(',')[0];
                            fromyear = linkvalue[0].Split(',')[1];
                            tomonth = linkvalue[1].Split(',')[0];
                            toyear = linkvalue[1].Split(',')[1];
                            getfromdate = frommonth + "/" + "1" + "/" + fromyear;

                            frm_date = Convert.ToDateTime(getfromdate);
                            int mon = Convert.ToInt32(tomonth);
                            int years = Convert.ToInt32(toyear);
                            int daysInmonth = System.DateTime.DaysInMonth(years, mon);
                            string getday = Convert.ToString(daysInmonth);
                            gettodate = tomonth + "/" + getday + "/" + toyear;
                            to_date = Convert.ToDateTime(gettodate);


                        }
                    }
                    else
                    {
                        alertmessage.Visible = true;
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Please Set IT Calculation Settings";
                        return;
                    }

                    year = "Select distinct m.Staff_code,Staff_name from staffmaster m,stafftrans t where  m.staff_code = t.staff_code and t.desig_code in('" + des + "') and t.latestrec = 1 and dept_code in ('" + dept + "') and college_code='" + college_code + "'  and ((resign=0 or settled=0) or (resign=1 and relieve_date>='" + frm_date + "') or (resign=1 and relieve_date between '" + frm_date + "' and '" + to_date + "')) order by staff_name ";
                }

                ds = da.select_method_wo_parameter(year, "text");
                {
                    chkliststname.DataSource = ds;
                    chkliststname.DataTextField = "Staff_name";
                    chkliststname.DataValueField = "Staff_code";
                    chkliststname.DataBind();
                }
                if (chkliststname.Items.Count > 0)
                {
                    for (int i = 0; i < chkliststname.Items.Count; i++)
                    {
                        chkliststname.Items[i].Selected = true;
                    }
                    TBST_NAME3.Text = "StaffName(" + chkliststname.Items.Count + ")";
                    chkstname.Checked = true;
                }
            }
            else
            {
                TBST_NAME3.Text = "--Select--";
                chkstname.Checked = false;
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void ddlfrommnth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if (ddlfrommnth.SelectedIndex == 0)
            //{
            //    ddltomnth.SelectedIndex = 0;
            //}
            //else
            //{
            //    ddltomnth.Items.Clear();
            //    ddltoyr.SelectedIndex = 0;
            //    int count = 0;
            //    string str = "select PayMonth,PayMonthNum from HrPayMonths where College_Code='" + college_code + "'";
            //    ds = da.select_method_wo_parameter(str, "Text");
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //        {
            //            string mon = ds.Tables[0].Rows[i]["PayMonth"].ToString();
            //            if (ddlfrommnth.SelectedItem.Text.ToString() == mon)
            //            {
            //                year(ddlfrommnth.SelectedItem.Value);
            //                for (int j = i; j < ds.Tables[0].Rows.Count; j++)
            //                {
            //                    ddltomnth.Items.Insert(count, new ListItem(ds.Tables[0].Rows[j]["PayMonth"].ToString(), ds.Tables[0].Rows[j]["PayMonthNum"].ToString()));
            //                    count++;
            //                }
            //            }
            //        }
            //    }
            //    ddltomnth.Items.Insert(0, "---Select---");
            //    ddltomnth.Items.Clear();
            //    ddltomnth.Items.Insert(0, "---Select---");
            //    int selvalue = Convert.ToInt32(ddlfrommnth.SelectedIndex.ToString());
            //    int itempos = 1;
            //    for (int i = selvalue; i < ddlfrommnth.Items.Count; i++)
            //    {
            //        ddltomnth.Items.Insert(itempos, ddlfrommnth.Items[i].Text.ToString());
            //        ddltomnth.Items[itempos].Value = ddlfrommnth.Items[i].Value.ToString();
            //        itempos++;
            //    }
            //}
            //typegrid.Visible = false;
            //Buttongen1.Visible = false;
            //butgen.Visible = false;
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void ddlfrmyr_SelectedIndexChanged(object sender, EventArgs e)
    {
        //try
        //{
        //    ddltoyr.Items.Clear();
        //    ddltomnth.SelectedIndex = 0;
        //    string str = "select distinct year(From_Date) as year from HrPayMonths where College_Code='" + college_code + "'  order by year asc";
        //    ds = da.select_method_wo_parameter(str, "Text");
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            var mon = ds.Tables[0].Rows[i]["year"].ToString();
        //            if (ddlfrmyr.SelectedItem.Text.ToString() == mon)
        //            {
        //                for (int j = i; j < ds.Tables[0].Rows.Count; j++)
        //                {
        //                    ddltoyr.Items.Add(ds.Tables[0].Rows[j]["year"].ToString());
        //                }
        //            }
        //        }
        //    }
        //    typegrid.Visible = false;
        //    Buttongen1.Visible = false;
        //    butgen.Visible = false;
        //}
        //catch (Exception ex)
        //{
        //    lblnorec.Visible = true;
        //    lblnorec.Text = ex.ToString();
        //}
    }
    protected void ddltomnth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //    string frm = ddlfrommnth.SelectedItem.Value;
            //    string tom = ddltomnth.SelectedItem.Value;
            //    if (ddltomnth.SelectedItem.Text != "---Select---")
            //    {
            //        if (Convert.ToInt32(frm) > Convert.ToInt32(tom))
            //        {
            //            if (ddltoyr.Items.Count > 2)
            //            {
            //                ddltoyr.SelectedIndex = 1;
            //                ddltoyr.Enabled = false;
            //            }
            //            else
            //            {
            //                ddltoyr.SelectedIndex = 1;
            //                ddltoyr.Enabled = false;
            //            }
            //        }
            //        else if (Convert.ToInt32(frm) == Convert.ToInt32(tom))
            //        {
            //            if (ddlfrmyr.SelectedIndex == 0)
            //            {
            //                ddltoyr.SelectedIndex = 0;
            //                ddltoyr.Enabled = false;
            //            }
            //            else
            //            {
            //                ddltoyr.SelectedIndex = 0;
            //                ddltoyr.Enabled = true;
            //            }
            //        }
            //        else
            //        {
            //            ddltoyr.SelectedIndex = 0;
            //        }
            //    }
            //    else
            //    {
            //        ddltoyr.SelectedIndex = 0;
            //    }
            //    typegrid.Visible = false;
            //    Buttongen1.Visible = false;
            //    butgen.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddltoyr_SelectedIndexChanged(object sender, EventArgs e)
    {
        //try
        //{
        //    typegrid.Visible = false;
        //    Buttongen1.Visible = false;
        //    butgen.Visible = false;
        //}
        //catch (Exception ex)
        //{
        //    lblnorec.Visible = true;
        //    lblnorec.Text = ex.ToString();
        //}
    }
    protected void chkdept_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkdept.Checked == true)
            {
                for (int i = 0; i < chklistdept.Items.Count; i++)
                {
                    chklistdept.Items[i].Selected = true;
                }
                int a = chklistdept.Items.Count;
                TBDEPT1.Text = "Department(" + a + ")";
            }
            else
            {
                for (int i = 0; i < chklistdept.Items.Count; i++)
                {
                    chklistdept.Items[i].Selected = false;
                }
                TBDEPT1.Text = "--Select--";
            }
            design();
            staff();
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void chklistdept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string deptccode = "";
            int a = 0;
            for (int i = 0; i < chklistdept.Items.Count; i++)
            {
                if (chklistdept.Items[i].Selected == true)
                {
                    a++;
                    if (deptccode == "")
                    {
                        deptccode = chklistdept.Items[i].Value.ToString();
                    }
                    else
                    {
                        deptccode = deptccode + "'" + "," + "'" + chklistdept.Items[i].Value.ToString();
                    }
                    TBDEPT1.Text = "Department(" + a + ")";
                }
            }
            if (a == 0)
            {
                TBDEPT1.Text = "--Select--";
            }
            else
            {
                TBDEPT1.Text = "Department(" + a + ")";
            }
            design();
            staff();
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void chkdesign_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkdesign.Checked == true)
            {
                for (int i = 0; i < chklistdesign.Items.Count; i++)
                {
                    chklistdesign.Items[i].Selected = true;
                }
                int a = chklistdesign.Items.Count;
                TBDESIGN2.Text = "Designation(" + a + ")";
            }
            else
            {
                for (int i = 0; i < chklistdesign.Items.Count; i++)
                {
                    chklistdesign.Items[i].Selected = false;
                }
                TBDESIGN2.Text = "--Select--";
            }
            staff();
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void chklistdesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int a = 0;
            for (int i = 0; i < chklistdesign.Items.Count; i++)
            {
                if (chklistdesign.Items[i].Selected == true)
                {
                    a++;
                }
                TBDESIGN2.Text = "Designation(" + a + ")";
            }
            staff();
            if (a == 0)
            {
                TBDESIGN2.Text = "--Select--";
            }
            else
            {
                TBDESIGN2.Text = "Designation(" + a + ")";
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void chkstname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkstname.Checked == true)
            {
                for (int i = 0; i < chkliststname.Items.Count; i++)
                {
                    chkliststname.Items[i].Selected = true;
                }
                int a = chkliststname.Items.Count;
                TBST_NAME3.Text = "StaffName(" + a + ")";
            }
            else
            {
                for (int i = 0; i < chkliststname.Items.Count; i++)
                {
                    chkliststname.Items[i].Selected = false;
                }
                TBST_NAME3.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void chkliststname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int a = 0;
            for (int i = 0; i < chkliststname.Items.Count; i++)
            {
                if (chkliststname.Items[i].Selected == true)
                {
                    a++;
                }
                TBST_NAME3.Text = "StaffName(" + a + ")";
            }
            if (a == 0)
            {
                TBST_NAME3.Text = "--Select--";
            }
            else
            {
                TBST_NAME3.Text = "StaffName(" + a + ")";
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void cbselectall_change(object sender, EventArgs e)
    {
        try
        {
            CheckBox ChkBoxHeader = (CheckBox)typegrid.HeaderRow.FindControl("cbselectall");
            foreach (GridViewRow row in typegrid.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("gridCkBox1");
                if (ChkBoxHeader.Checked == true)
                {
                    ChkBoxRows.Checked = true;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddltype.SelectedItem.Value) != 3)
            {
                #region not format2
                lblnorec.Visible = false;
                //string year1 = "";
                //year1 = "select distinct year(To_Date) as year from HrPayMonths where College_Code ='" + college_code + "'  order by year asc";
                //ds = da.select_method_wo_parameter(year1, "text");
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    typegrid.Visible = true;

                
                Buttongen1.Visible = true;
                butgen.Visible = true;
                string from_month = ddlfrommnth.SelectedItem.Value;
                string to_month = ddltomnth.SelectedItem.Value;
                if (from_month == "0" || to_month == "0")
                {
                    typegrid.Visible = false;
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Enter From Month and To Month\");", true);
                    return;
                
                }
                string deptvalue = "";
                string build = "";
                string builddesign = "";
                string builddes = "";
                string staffvalue = "";
                string staff = "";
                for (int i = 0; i < chklistdept.Items.Count; i++)
                {
                    if (chklistdept.Items[i].Selected == true)
                    {
                        build = chklistdept.Items[i].Value;
                        if (deptvalue == "")
                        {
                            deptvalue = build;
                        }
                        else
                        {
                            deptvalue = deptvalue + "'" + "," + "'" + build;
                        }
                    }
                }
                for (int i = 0; i < chklistdesign.Items.Count; i++)
                {
                    if (chklistdesign.Items[i].Selected == true)
                    {
                        builddes = chklistdesign.Items[i].Value;
                        if (builddesign == "")
                        {
                            builddesign = builddes;
                        }
                        else
                        {
                            builddesign = builddesign + "'" + "," + "'" + builddes;
                        }
                    }
                }
                for (int i = 0; i < chkliststname.Items.Count; i++)
                {
                    if (chkliststname.Items[i].Selected == true)
                    {
                        staff = chkliststname.Items[i].Value;
                        if (staffvalue == "")
                        {
                            staffvalue = staff;
                        }
                        else
                        {
                            staffvalue = staffvalue + "'" + "," + "'" + staff;
                        }
                    }
                }
                string StaffType = rs.GetSelectedItemsValueAsString(cbl_stafftyp);
                DataTable dt = new DataTable();
                DataRow dr = null;
                DataSet ds1 = new DataSet();
                DataSet ds_st = new DataSet();
                ArrayList addarray = new ArrayList();
                int monthfrom;
                int monthto;
                monthfrom = Convert.ToInt16(ddlfrommnth.SelectedValue.ToString());
                monthto = Convert.ToInt16(ddltomnth.SelectedValue.ToString());
                int firstday = 1;
                int years;
                int toyear;
                years = Convert.ToInt16(ddlfrmyr.SelectedItem.Text);
                toyear = Convert.ToInt16(ddltoyr.SelectedItem.Text);
                if (toyear < years)
                {

                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"From Year Should Be Less Than To Year\");", true);
                    return;
                }
                string fromdate = Convert.ToString(monthfrom + "/" + firstday + "/" + years);
                string todate = Convert.ToString(monthto + "/" + firstday + "/" + toyear);
                DateTime fdate = Convert.ToDateTime(fromdate);
                DateTime tdate = Convert.ToDateTime(todate);
                //int f_month = Convert.ToInt32(ddlfrommnth.SelectedItem.Value);
                //int t_month = Convert.ToInt32(ddltomnth.SelectedItem.Value);
                //string frmyr = ddlfrmyr.SelectedValue;
                //string toyr = ddltoyr.SelectedValue;
                //string monthquery = "";
                //if (t_month == 2)
                //{
                //    monthquery = " select From_Date ,To_Date from HrPayMonths where  From_Date >= '" + frmyr + "-" + f_month + "-01' and  To_Date <=  '" + toyr + "-" + t_month + "-28'and College_Code='" + college_code + "' ";
                //}
                //else
                //{
                //    monthquery = " select From_Date ,To_Date from HrPayMonths where  From_Date >= '" + frmyr + "-" + f_month + "-01' and  To_Date <=  '" + toyr + "-" + t_month + "-30'and College_Code='" + college_code + "' ";
                //}
                //ds1.Clear();
                //ds1 = da.select_method_wo_parameter(monthquery, "Text");
                //if (ds1.Tables[0].Rows.Count > 0)
                //{
                //    for (int add = 0; add < ds1.Tables[0].Rows.Count; add++)
                //    {
                //        addarray.Add(Convert.ToString(ds1.Tables[0].Rows[add]["From_Date"]) + "," + Convert.ToString(ds1.Tables[0].Rows[add]["To_Date"]));
                //    }
                //}
                while (fdate < tdate)
                {
                    int lastDay = DateTime.DaysInMonth(years, monthfrom);
                    string last_day = monthfrom + "/" + lastDay + "/" + years;
                    DateTime lday = Convert.ToDateTime(last_day);
                    addarray.Add(Convert.ToString(fdate) + "," + Convert.ToString(lday));
                    fdate = fdate.AddMonths(1);
                }
                dt.Columns.Add("Staff Name", typeof(string));
                dt.Columns.Add("Department", typeof(string));
                dt.Columns.Add("Designation", typeof(string));
                dt.Columns.Add("Actual Salary", typeof(string));
                dt.Columns.Add("staff_code", typeof(string));
                dt.Columns.Add("Dept_code", typeof(string));
                dt.Columns.Add("desig_code", typeof(string));
                string selectquery = "select s.staff_code,s.staff_name,h.dept_code,h.dept_name,d.desig_name,d.desig_code  from staffmaster s,staff_appl_master a,hrdept_master h, stafftrans t,desig_master d where s.appl_no =a.appl_no and h.dept_code=a.dept_code and a.college_code =s.college_code and t.staff_code =s.staff_code and d.desig_code =t.desig_code  and h.dept_code in ('" + deptvalue + "') and d.desig_code in ('" + builddesign + "') and s.staff_code in ('" + staffvalue + "') and latestrec =1 ";
                if (StaffType != "")
                {
                    selectquery = selectquery + " and t.stftype in('" + StaffType + "') order by s.staff_code";//added by delsi1412
                
                }
                else
                {
                   selectquery=selectquery+" order by s.staff_code";
                
                }

              
                ds.Clear();
                ds = da.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        dr = dt.NewRow();
                        dr[0] = Convert.ToString(ds.Tables[0].Rows[row]["staff_name"]);
                        dr[1] = Convert.ToString(ds.Tables[0].Rows[row]["dept_name"]);
                        dr[2] = Convert.ToString(ds.Tables[0].Rows[row]["desig_name"]);
                        dr[4] = Convert.ToString(ds.Tables[0].Rows[row]["staff_code"]);
                        dr[5] = Convert.ToString(ds.Tables[0].Rows[row]["dept_code"]);
                        dr[6] = Convert.ToString(ds.Tables[0].Rows[row]["desig_code"]);
                        if (addarray.Count > 0)
                        {
                            double mainsalary = 0;
                            for (int add = 0; add < addarray.Count; add++)
                            {
                                string datevlaue = Convert.ToString(addarray[add]);
                                if (datevlaue.Trim() != "")
                                {
                                    string[] splitdate = datevlaue.Split(',');
                                    if (splitdate.Length > 0)
                                    {
                                        string fromdate1 = Convert.ToString(splitdate[0]);
                                        string todate1 = Convert.ToString(splitdate[1]);
                                        string[] split1 = fromdate1.Split('/');
                                        string mon_fdate = Convert.ToString(split1[1]);
                                        string year_fdate = Convert.ToString(split1[2]);
                                        //string query1 = "select bsalary from monthlypay where fdate ='" + fromdate1 + "' and tdate ='" + todate1 + "' and staff_code='" + Convert.ToString(ds.Tables[0].Rows[row]["staff_code"]) + "' and College_Code='" + college_code + "'";//delsi
                                        string query1 = "select bsalary from monthlypay where CAST(CONVERT(varchar(20),PayMonth)+'/01/'+CONVERT(varchar(20),PayYear) as Datetime) between CAST(CONVERT(varchar(20),'" + mon_fdate + "')+'/01/'+CONVERT(varchar(20),'" + year_fdate + "') as Datetime) and CAST(CONVERT(varchar(20),'" + mon_fdate + "')+'/01/'+CONVERT(varchar(20),'" + year_fdate + "') as Datetime) and staff_code = '" + Convert.ToString(ds.Tables[0].Rows[row]["staff_code"]) + "' and College_Code='" + college_code + "'";
                                        ds1.Clear();
                                        ds1 = da.select_method_wo_parameter(query1, "Text");
                                        if (ds1.Tables[0].Rows.Count > 0)
                                        {
                                            for (int sal = 0; sal < ds1.Tables[0].Rows.Count; sal++)
                                            {
                                                string subsalary = Convert.ToString(ds1.Tables[0].Rows[sal]["bsalary"]);
                                                if (subsalary.Trim() != "")
                                                {
                                                    mainsalary = mainsalary + Convert.ToDouble(subsalary);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            dr[3] = Convert.ToString(Math.Round(Convert.ToDouble(mainsalary)));
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    typegrid.Visible = true;
                    butgen.Visible = true;
                    Buttongen1.Visible = true;
                    typegrid.DataSource = dt;
                    typegrid.DataBind();
                }
                else
                {
                    typegrid.Visible = false;
                    butgen.Visible = false;
                    Buttongen1.Visible = false;
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"No Records Found\");", true);
                }
                #endregion
            }
            else if (Convert.ToInt32(ddltype.SelectedItem.Value) == 3)
            {
                typegrid.Visible = false;
                butgen.Visible = false;
                Buttongen1.Visible = false;
                ItReportTaxDetailsReport();
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void gridbound(object sender, EventArgs e)
    {
        try
        {
            for (int i = typegrid.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = typegrid.Rows[i];
                GridViewRow previousRow = typegrid.Rows[i - 1];
                for (int j = 4; j <= 5; j++)
                {
                    if (j == 4)
                    {
                        Label lnlname = (Label)row.FindControl("lbldept");
                        Label lnlname1 = (Label)previousRow.FindControl("lbldept");
                        if (lnlname.Text == lnlname1.Text)
                        {
                            if (previousRow.Cells[j].RowSpan == 0)
                            {
                                if (row.Cells[j].RowSpan == 0)
                                {
                                    previousRow.Cells[j].RowSpan += 2;
                                }
                                else
                                {
                                    previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                                }
                                row.Cells[j].Visible = false;
                            }
                        }
                    }
                    if (j == 5)
                    {
                        Label lnlname = (Label)row.FindControl("lbldesig");
                        Label lnlname1 = (Label)previousRow.FindControl("lbldesig");
                        if (lnlname.Text == lnlname1.Text)
                        {
                            if (previousRow.Cells[j].RowSpan == 0)
                            {
                                if (row.Cells[j].RowSpan == 0)
                                {
                                    previousRow.Cells[j].RowSpan += 2;
                                }
                                else
                                {
                                    previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                                }
                                row.Cells[j].Visible = false;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    public void bindpdf()
    {
        try
        {
            DataTable dtproftax = new DataTable();
            dtproftax.Columns.Add("Month");
            dtproftax.Columns.Add("Amount");
            DataRow drproftax;
            Font Fontbold = new Font("Times new roman", 13, FontStyle.Regular);
            Font Fontbold0 = new Font("Times new roman", 10, FontStyle.Regular);
            Font Fontbold1 = new Font("Times new roman", 6, FontStyle.Regular);
            Font Fontbold2 = new Font("Times new roman", 7, FontStyle.Regular);
            Font Fontbold3 = new Font("Times new roman", 12, FontStyle.Regular);
            Font Fontbold4 = new Font("Times new roman", 16, FontStyle.Regular);
            Font Fontbold5 = new Font("Times new roman", 6, FontStyle.Regular);
            Font Fontbold6 = new Font("Times new roman", 13, FontStyle.Regular);
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            double pay = 0;
            double gp = 0;
            double tot = 0;
            int ftot = 0;
            double hrallw = 0;
            double directall = 0;
            double pffinaltt = 0;
            double fspl = 0;
            double proftax = 0;
            double hrr1 = 0;
            double taxpaid = 0;
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            DataSet ds3 = new DataSet();
            DataSet ds4 = new DataSet();
            DataSet ds5 = new DataSet();
            string staffname = "";
            string depart = "";
            string pan_no = "";
            string staff_code = "";
            string dateofbirth = "";
            string dateage = "";
            string Email = "";
            string Phone_num = "";
            string fathername = "";
            string husbandname = "";
            string permanentaddr = "";
            string permanentaddr1 = "";
            string perpincode = "";
            string city = "";
            string pfno = "";
            string surnm = "";
            string collegenew1 = "";
            string address1 = "";
            string address2 = "";
            string acronym = "";
            string amt_wages = "";
            string fr_month = "";
            string to_month = "";
            string desing_name = "";
            int yr1 = 0;
            int yr2 = 0;
            ArrayList addarray = new ArrayList();
            ArrayList column = new ArrayList();
            ArrayList month = new ArrayList();
            Hashtable addmonthnumber = new Hashtable();
            Hashtable addmonthvalue = new Hashtable();
            Hashtable hasadd = new Hashtable();
            ArrayList addressarray = new ArrayList();
            int f_month = Convert.ToInt32(ddlfrommnth.SelectedItem.Value);
            int t_month = Convert.ToInt32(ddltomnth.SelectedItem.Value);
            fr_month = Convert.ToString(ddlfrommnth.SelectedItem.Text);
            to_month = Convert.ToString(ddltomnth.SelectedItem.Text);
            yr1 = Convert.ToInt32(ddlfrmyr.SelectedItem.Value);
            yr2 = Convert.ToInt32(ddltoyr.SelectedItem.Value);
            string monthquery = "";
            if (f_month <= t_month)
            {
                if (yr1 == yr2)
                {
                    monthquery = " select CONVERT(varchar(20), From_Date,101) as From_Date,CONVERT(varchar(20), To_Date,101) as To_Date,(PayMonth+' '+ CONVERT(varchar(10),year(To_date))) as PayMonth from HrPayMonths where PayMonthNum >= " + f_month + " and year(to_date) = " + yr1 + " and PayMonthNum <=" + t_month + " and year(to_date) =  " + yr2 + " and College_Code='" + college_code + "' ";
                }
                else
                {
                    monthquery = " select CONVERT(varchar(20), From_Date,101) as From_Date,CONVERT(varchar(20), To_Date,101) as To_Date,(PayMonth+' '+ CONVERT(varchar(10),year(To_date))) as PayMonth from HrPayMonths where PayMonthNum >= " + f_month + " and year(to_date) = " + yr1 + "  union all select CONVERT(varchar(20), From_Date,101) as From_Date,CONVERT(varchar(20), To_Date,101) as To_Date,(PayMonth+' '+ CONVERT(varchar(10),year(To_date)))as PayMonth from HrPayMonths where PayMonthNum <=" + t_month + " and year(to_date) =  " + yr2 + " and College_Code ='" + college_code + "'";
                }
                ds1.Clear();
                ds1 = da.select_method_wo_parameter(monthquery, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int add = 0; add < ds1.Tables[0].Rows.Count; add++)
                    {
                        addarray.Add(Convert.ToString(ds1.Tables[0].Rows[add]["From_Date"]) + "," + Convert.ToString(ds1.Tables[0].Rows[add]["To_Date"]));
                        month.Add(Convert.ToString(ds1.Tables[0].Rows[add]["PayMonth"]));
                    }
                }
            }
            else
            {
                if (yr1 == yr2)
                {
                    monthquery = " select CONVERT(varchar(20), From_Date,101) as From_Date,CONVERT(varchar(20), To_Date,101) as To_Date,(left(PayMonth,3)+' '+ CONVERT(varchar(10),year(To_date))) as PayMonth from HrPayMonths where PayMonthNum >= " + f_month + " and year(to_date) = " + yr1 + " and PayMonthNum <=" + t_month + " and year(to_date) =  " + yr2 + " and College_Code='" + college_code + "' ";
                }
                else
                {
                    monthquery = " select CONVERT(varchar(20), From_Date,101) as From_Date,CONVERT(varchar(20), To_Date,101) as To_Date,(left(PayMonth,3)+' '+ CONVERT(varchar(10),year(To_date))) as PayMonth from HrPayMonths where PayMonthNum >= " + f_month + " and year(to_date) = " + yr1 + "  union all select CONVERT(varchar(20), From_Date,101) as From_Date,CONVERT(varchar(20), To_Date,101) as To_Date,(left(PayMonth,3)+' '+ CONVERT(varchar(10),year(To_date)))as PayMonth from HrPayMonths where PayMonthNum <=" + t_month + " and year(to_date) =  " + yr2 + " and College_Code ='" + college_code + "'";
                }
                ds1.Clear();
                ds1 = da.select_method_wo_parameter(monthquery, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int add = 0; add < ds1.Tables[0].Rows.Count; add++)
                    {
                        addarray.Add(Convert.ToString(ds1.Tables[0].Rows[add]["From_Date"]) + "," + Convert.ToString(ds1.Tables[0].Rows[add]["To_Date"]));
                        month.Add(Convert.ToString(ds1.Tables[0].Rows[add]["PayMonth"]));
                    }
                }
            }
            string wages = "select FPFMaxAmt from Hr_PaySettings where College_Code=" + college_code + "";
            ds5.Clear();
            ds5 = da.select_method_wo_parameter(wages, "Text");
            if (ds5.Tables[0].Rows.Count > 0)
            {
                for (int count = 0; count < ds5.Tables[0].Rows.Count; count++)
                {
                    amt_wages = Convert.ToString(ds5.Tables[0].Rows[count]["FPFMaxAmt"]);
                }
            }
            string collegetitle = "select isnull(collname,'') as collname,isnull(acr,'') as acr,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code='" + Session["collegecode"] + "'";
            ds2.Clear();
            ds2 = da.select_method_wo_parameter(collegetitle, "Text");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                for (int count = 0; count < ds2.Tables[0].Rows.Count; count++)
                {
                    collegenew1 = Convert.ToString(ds2.Tables[0].Rows[count]["collname"]);
                    address1 = Convert.ToString(ds2.Tables[0].Rows[count]["address1"]);
                    address2 = Convert.ToString(ds2.Tables[0].Rows[count]["address3"]);
                    acronym = Convert.ToString(ds2.Tables[0].Rows[count]["acr"]);
                }
            }
            string selectquery1 = "select family_info,sex,staff_code,pangirnumber,per_address,per_pincode,per_address1,pcity,husband_name,pfnumber,sur_name,convert( varchar(20), date_of_birth,103) as date_of_birth,(CONVERT(int, DATEPART(YEAR,GETDATE()))-(CONVERT(int, DATEPART(YEAR,date_of_birth))))as age  ,email,per_mobileno,father_name from staff_appl_master a,staffmaster m where a.appl_no =m.appl_no and m.College_Code='" + college_code + "' ";
            selectquery1 = selectquery1 + " select staff_code,  deductions  from stafftrans  where   latestrec =1 ";
            ds1.Clear();
            ds1 = da.select_method_wo_parameter(selectquery1, "Text");
            bool flage = false;
            if (typegrid.Rows.Count > 0)
            {
                for (int row = 0; row < typegrid.Rows.Count; row++)
                {
                    dtproftax.Clear();
                    column.Clear();
                    pay = 0;
                    gp = 0;
                    tot = 0;
                    ftot = 0;
                    hrallw = 0;
                    directall = 0;
                    pffinaltt = 0;
                    proftax = 0;
                    fspl = 0;
                    hrr1 = 0;
                    taxpaid = 0;
                    DataView dv1 = new DataView();
                    DataView dv2 = new DataView();
                    if (((typegrid.Rows[row].FindControl("gridCkBox1") as CheckBox).Checked == true))
                    {
                        flage = true;
                        staffname = ((typegrid.Rows[row].FindControl("lblstaff") as Label).Text);
                        depart = ((typegrid.Rows[row].FindControl("lbldept") as Label).Text);
                        staff_code = ((typegrid.Rows[row].FindControl("lblstaff_code") as Label).Text);
                        desing_name = ((typegrid.Rows[row].FindControl("lbldesig") as Label).Text);
                        perpincode = "";
                        city = "";
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            ds1.Tables[0].DefaultView.RowFilter = " staff_code='" + staff_code + "'";
                            dv1 = ds1.Tables[0].DefaultView;
                            if (dv1.Count > 0)
                            {
                                for (int count = 0; count < dv1.Count; count++)
                                {
                                    dateage = Convert.ToString(dv1[count]["age"]);
                                    dateofbirth = Convert.ToString(dv1[count]["date_of_birth"]);
                                    Email = Convert.ToString(dv1[count]["email"]);
                                    Phone_num = Convert.ToString(dv1[count]["per_mobileno"]);
                                    fathername = Convert.ToString(dv1[count]["father_name"]);
                                    pfno = Convert.ToString(dv1[count]["pfnumber"]);
                                    surnm = Convert.ToString(dv1[count]["sur_name"]);
                                    husbandname = Convert.ToString(dv1[count]["husband_name"]);
                                    pan_no = Convert.ToString(dv1[count]["pangirnumber"]);
                                    permanentaddr = Convert.ToString(dv1[count]["per_address"]);
                                    permanentaddr1 = Convert.ToString(dv1[count]["per_address1"]);
                                    perpincode = Convert.ToString(dv1[count]["per_pincode"]);
                                    city = Convert.ToString(dv1[count]["pcity"]);
                                }
                            }
                        }
                        if (ds1.Tables[1].Rows.Count > 0)
                        {
                            ds1.Tables[1].DefaultView.RowFilter = " staff_code='" + staff_code + "'";
                            dv1 = ds1.Tables[1].DefaultView;
                            if (dv1.Count > 0)
                            {
                                for (int i = 0; i < dv1.Count; i++)
                                {
                                    string deducation = Convert.ToString(dv1[i]["deductions"]);
                                    string[] splitfirstarray = deducation.Split('\\');
                                    if (splitfirstarray.Length > 0)
                                    {
                                        for (int arr = 0; arr <= splitfirstarray.GetUpperBound(0); arr++)
                                        {
                                            string secondvalue = Convert.ToString(splitfirstarray[arr]);
                                            if (secondvalue.Trim() != "")
                                            {
                                                string[] splitsecond = secondvalue.Split(';');
                                                if (splitsecond.Length > 0)
                                                {
                                                    string typevlaue = Convert.ToString(splitsecond[0]);
                                                    if (!column.Contains(typevlaue))
                                                    {
                                                        column.Add(typevlaue);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        string fathername_name = "";
                        string husband_name = "";
                        string final_nmae = "";
                        bool check = false;
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            ds1.Tables[0].DefaultView.RowFilter = " staff_code='" + staff_code + "'";
                            dv1 = ds1.Tables[0].DefaultView;
                            if (dv1.Count > 0)
                            {
                                string familyinfo = Convert.ToString(dv1[0]["family_info"]);
                                string gender = Convert.ToString(dv1[0]["sex"]);
                                if (familyinfo.Trim() != "")
                                {
                                    string[] splitfirst = familyinfo.Split('\\');
                                    if (splitfirst.Length > 0)
                                    {
                                        for (int i = 0; i <= splitfirst.GetUpperBound(0); i++)
                                        {
                                            string firstvalue = Convert.ToString(splitfirst[i]);
                                            if (firstvalue.Trim() != "")
                                            {
                                                string[] secondsplit = firstvalue.Split(';');
                                                if (secondsplit.Length > 0)
                                                {
                                                    string name = Convert.ToString(secondsplit[1]);
                                                    if (gender == "Male")
                                                    {
                                                        if (Convert.ToString(secondsplit[4]).ToUpper() == "FATHER")
                                                        {
                                                            fathername_name = Convert.ToString(name);
                                                        }
                                                    }
                                                    if (gender == "Female")
                                                    {
                                                        if (Convert.ToString(secondsplit[4]).ToUpper() == "HUSBAND")
                                                        {
                                                            check = true;
                                                            husband_name = Convert.ToString(name);
                                                        }
                                                        if (Convert.ToString(secondsplit[4]).ToUpper() == "FATHER")
                                                        {
                                                            fathername_name = Convert.ToString(name);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (check == false)
                                        {
                                            final_nmae = fathername_name;
                                        }
                                        else
                                        {
                                            if (fathername_name.Trim() == "")
                                            {
                                                final_nmae = husband_name;
                                            }
                                            else
                                            {
                                                final_nmae = fathername_name;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    final_nmae = "";
                                }
                            }
                        }
                        if (ddltype.SelectedItem.Text == "Income Tax")
                        {
                            int y = 50;
                            Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 20, 10, 450);
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                            {
                                PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                mypdfpage.Add(LogoImage1, 500, 10, 450);
                            }
                            PdfTextArea pdffnamea = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 180, y + 130, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, final_nmae);
                            mypdfpage.Add(pdffnamea);
                            PdfTextArea pdf = new PdfTextArea(Fontbold4, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 20, 400, 30), System.Drawing.ContentAlignment.TopCenter, collegenew1);
                            PdfTextArea pdfaddr1 = new PdfTextArea(Fontbold3, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 35, 400, 30), System.Drawing.ContentAlignment.TopCenter, address1);
                            PdfTextArea pdfaddr2 = new PdfTextArea(Fontbold3, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 45, 400, 30), System.Drawing.ContentAlignment.TopCenter, address2);
                            PdfTextArea pdf1 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 50, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "NAME OF THE STAFF ");
                            PdfTextArea pdf11 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 180, y + 50, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, staffname);
                            PdfTextArea pdf111 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 170, y + 50, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ":");
                            PdfTextArea pdf2 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 70, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "DEPARTMENT   ");
                            PdfTextArea pdf22 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 180, y + 70, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, depart);
                            PdfTextArea pdf222 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 170, y + 70, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ":");
                            PdfTextArea pdf3 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 90, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "POST HELD ");
                            PdfTextArea pdf333 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 170, y + 90, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ":");
                            PdfTextArea pdfdesign = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 180, y + 90, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, desing_name);
                            PdfTextArea pdf4 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 110, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "PANCARD NUMBER  ");
                            PdfTextArea pdf444 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 170, y + 110, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ":");
                            PdfTextArea pdfpan_no = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 180, y + 110, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, pan_no);
                            PdfTextArea pdffname = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 130, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "FATHER/HUSBAND'S NAME");
                            PdfTextArea pdffnamecol = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 170, y + 130, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ":");
                            PdfTextArea pdf5 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 150, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "PHONE NUMBER     ");
                            PdfTextArea pdf55 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 180, y + 150, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Phone_num);
                            PdfTextArea pdf555 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 170, y + 150, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ":");
                            PdfTextArea pdf6 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 170, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "EMAIL-ID  ");
                            PdfTextArea pdf66 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 180, y + 170, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Email);
                            PdfTextArea pdf666 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 170, y + 170, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ":");
                            PdfTextArea pdf7 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 190, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "D.O.B & AGE ");
                            PdfTextArea pdf77 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 180, y + 190, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, dateofbirth);
                            PdfTextArea pdf77nd = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 230, y + 190, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "&");
                            PdfTextArea pdf77age = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 240, y + 190, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, dateage);
                            PdfTextArea pdf777 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 170, y + 190, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ":");
                            PdfTextArea pdf8 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 50, 400, 30), System.Drawing.ContentAlignment.MiddleRight, "RESIDENTIAL :");
                            PdfTextArea pdf88 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 44, y + 70, 400, 30), System.Drawing.ContentAlignment.MiddleRight, "ADDRESS  ");
                            PdfTextArea pdfpmtaddr = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 445, y + 50, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, permanentaddr);
                            PdfTextArea pdfpmtaddr1 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 445, y + 70, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, permanentaddr1);
                            PdfTextArea pdfpmtaddr2 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 445, y + 80, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, city);
                            PdfTextArea pdfpmtaddr3 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 445, y + 90, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, perpincode);
                            mypdfpage.Add(pdfpmtaddr);
                            mypdfpage.Add(pdfpmtaddr1);
                            mypdfpage.Add(pdfpmtaddr2);
                            mypdfpage.Add(pdfpmtaddr3);
                            PdfTextArea pdf9 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y + 230, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "SALARY INCOME RECEIVED DURING " + yr1 + "-" + yr2 + "");
                            PdfTextArea pdf99 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y + 235, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "-------------------------------------------------------------------");
                            PdfTextArea pdfst_sig = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 140, y + 710, 400, 30), System.Drawing.ContentAlignment.BottomRight, "SIGNATURE OF THE STAFF.");
                            Gios.Pdf.PdfTable table = mydoc.NewTable(Fontbold1, 5 + addarray.Count, 10 + column.Count, 2);
                            table = mydoc.NewTable(Fontbold1, 5 + addarray.Count, 10 + column.Count, 2);
                            table.VisibleHeaders = false;
                            int totalrowcount = 5 + addarray.Count;
                            int totalrowcount1 = 4 + addarray.Count;
                            int columncount = 10 + column.Count;
                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table.CellRange(0, 0, 0, 4).SetFont(Fontbold);
                            table.Cell(0, 2).SetContent("Salary Received");
                            table.CellRange(0, 0, 0, 9).SetFont(Fontbold);
                            table.Cell(0, 9).SetContent("Contribution Made To");
                            foreach (PdfCell pr in table.CellRange(0, 2, 0, 2).Cells)
                            {
                                pr.ColSpan = 7;
                            }
                            foreach (PdfCell pr in table.CellRange(0, 9, 0, 9).Cells)
                            {
                                pr.ColSpan = column.Count + 1;
                            }
                            foreach (PdfCell pr in table.CellRange(0, 0, 0, 0).Cells)
                            {
                                pr.RowSpan = 2;
                            }
                            foreach (PdfCell pr in table.CellRange(0, 1, 0, 1).Cells)
                            {
                                pr.RowSpan = 2;
                            }
                            foreach (PdfCell pr in table.CellRange(totalrowcount - 1, 0, totalrowcount - 1, 0).Cells)
                            {
                                pr.ColSpan = 9;
                            }
                            foreach (PdfCell pr in table.CellRange(totalrowcount - 1, 9, totalrowcount - 1, 9).Cells)
                            {
                                pr.ColSpan = column.Count + 1;
                            }
                            table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 0).SetContent("S.No");
                            table.Cell(0, 0).SetFont(Fontbold5);
                            table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 1).SetContent("Month");
                            table.Cell(0, 1).SetFont(Fontbold5);
                            table.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(1, 2).SetContent("PAY");
                            table.Cell(1, 2).SetFont(Fontbold5);
                            table.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(1, 3).SetContent("DP/GP");
                            table.Cell(1, 3).SetFont(Fontbold5);
                            table.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(1, 4).SetContent("DA");
                            table.Cell(1, 4).SetFont(Fontbold5);
                            table.Cell(1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(1, 5).SetContent("SPL.PAY");
                            table.Cell(1, 5).SetFont(Fontbold5);
                            table.Cell(1, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(1, 6).SetContent("TOTAL");
                            table.Cell(1, 6).SetFont(Fontbold5);
                            table.Cell(1, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(1, 7).SetContent("HRA");
                            table.Cell(1, 7).SetFont(Fontbold5);
                            table.Cell(1, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(1, 8).SetContent("TOTAL");
                            table.Cell(1, 8).SetFont(Fontbold5);
                            table.Cell(1, columncount - 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(1, columncount - 1).SetContent("TOTAL");
                            table.Cell(1, columncount - 1).SetFont(Fontbold5);
                            table.Cell(totalrowcount - 3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(totalrowcount - 3, 1).SetContent("Tax Paid");
                            table.Cell(totalrowcount - 3, 1).SetFont(Fontbold5);
                            table.Cell(totalrowcount - 3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table.Cell(totalrowcount - 2, 1).SetContent("Total");
                            table.Cell(totalrowcount - 2, 1).SetFont(Fontbold5);
                            table.Cell(totalrowcount - 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table.Cell(totalrowcount - 1, 0).SetContent("OTHER INCOME(CL ENCASHMENT,PREVIOUS YEAR ARREAR SALARY)Rs.");
                            table.Cell(totalrowcount - 1, 0).SetFont(Fontbold5);
                            //   int ftot1 = Convert.ToInt32(ftot);
                            table.Cell(totalrowcount - 1, 9).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table.Cell(totalrowcount - 1, 9).SetContent("GRAND TOTAL Rs." + ftot + "");
                            table.Cell(totalrowcount - 1, 9).SetFont(Fontbold5);
                            int val = 8;
                            for (int col = 0; col < column.Count; col++)
                            {
                                val++;
                                table.Cell(1, val).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(1, val).SetContent(Convert.ToString(column[col]));
                            }
                            int row3 = 1;
                            for (int row1 = 0; row1 < month.Count; row1++)
                            {
                                row3++;
                                table.Cell(row3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(row3, 0).SetContent(row1 + 1);
                                table.Cell(row3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(row3, 1).SetContent(Convert.ToString(month[row1]));
                            }
                            int row4 = 1;
                            int xy = 500;
                            double lastcount1 = 0;
                            hasadd.Clear();
                            for (int va = 0; va < addarray.Count; va++)
                            {
                                row4++;
                                string date = Convert.ToString(addarray[va]);
                                if (date.Trim() != "")
                                {
                                    string[] splitdate = date.Split(',');
                                    if (splitdate.Length > 0)
                                    {
                                        string firstdate = Convert.ToString(splitdate[0]);
                                        string seconddate = Convert.ToString(splitdate[1]);
                                        string selectquery = "  select bsalary,basic_alone ,grade_pay ,G_Pay ,allowances,deductions  from monthlypay  where latestrec =1 and staff_code='" + staff_code + "' and fdate ='" + firstdate + "' and tdate='" + seconddate + "' and College_Code='" + college_code + "' ";
                                        selectquery = selectquery + "    select SUM(TaxDeposited) from Staff_ITDeposited where Staff_Code='" + staff_code + "' and From_Date ='" + firstdate + "' and To_Date='" + seconddate + "' and College_Code='" + college_code + "'";
                                        ds.Clear();
                                        ds = da.select_method_wo_parameter(selectquery, "Text");
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            string b_salary = Convert.ToString(ds.Tables[0].Rows[0]["bsalary"]);
                                            if (b_salary.Trim() != "")
                                            {
                                                pay = pay + Convert.ToDouble(b_salary);
                                            }
                                            else
                                            {
                                                pay = pay + 0;
                                                b_salary = "0";
                                            }
                                            string gradepay = Convert.ToString(ds.Tables[0].Rows[0]["grade_pay"]);
                                            if (gradepay.Trim() != "")
                                            {
                                                gp = gp + Convert.ToDouble(gradepay);
                                            }
                                            else
                                            {
                                                gp = gp + 0;
                                                gradepay = "0";
                                            }
                                            string allow = Convert.ToString(ds.Tables[0].Rows[0]["allowances"]);
                                            string dedect = Convert.ToString(ds.Tables[0].Rows[0]["deductions"]);
                                            string da1 = "";
                                            string hra = "";
                                            table.Cell(row4, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table.Cell(row4, 2).SetContent(Convert.ToString(Math.Round(Convert.ToDouble(b_salary))));
                                            table.Cell(row4, 3).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table.Cell(row4, 3).SetContent(Convert.ToString(Math.Round(Convert.ToDouble(gradepay))));
                                            double remain = 0;
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                string[] splitfirstarray1 = allow.Split('\\');
                                                if (splitfirstarray1.Length > 0)
                                                {
                                                    for (int arr1 = 0; arr1 <= splitfirstarray1.GetUpperBound(0); arr1++)
                                                    {
                                                        string secondvalue = Convert.ToString(splitfirstarray1[arr1]);
                                                        if (secondvalue.Trim() != "")
                                                        {
                                                            string[] splitsecond = secondvalue.Split(';');
                                                            if (splitsecond.Length > 0)
                                                            {
                                                                string typevalue = Convert.ToString(splitsecond[0]);
                                                                if (typevalue == "DA")
                                                                {
                                                                    table.Cell(row4, 4).SetContentAlignment(ContentAlignment.MiddleRight);
                                                                    da1 = Convert.ToString(splitsecond[3]);
                                                                    if (da1.Trim() != "")
                                                                    {
                                                                        table.Cell(row4, 4).SetContent(Convert.ToString(Math.Round(Convert.ToDouble(splitsecond[3]))));
                                                                    }
                                                                    else
                                                                    {
                                                                        table.Cell(row4, 4).SetContent(0);
                                                                    }
                                                                }
                                                                else if (typevalue == "HRA")
                                                                {
                                                                    table.Cell(row4, 7).SetContentAlignment(ContentAlignment.MiddleRight);
                                                                    hra = Convert.ToString(splitsecond[3]);
                                                                    if (hra.Trim() != "")
                                                                    {
                                                                        table.Cell(row4, 7).SetContent(Convert.ToString(Math.Round(Convert.ToDouble(splitsecond[3]))));
                                                                    }
                                                                    else
                                                                    {
                                                                        table.Cell(row4, 7).SetContent(0);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    string spl = Convert.ToString(splitsecond[3]);
                                                                    if (spl.Trim() != "")
                                                                    {
                                                                        remain = remain + Convert.ToDouble(spl);
                                                                    }
                                                                    else
                                                                    {
                                                                        remain = remain + 0;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            table.Cell(row4, 5).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table.Cell(row4, 5).SetContent(Math.Round(remain));
                                            double total = Convert.ToDouble(b_salary);
                                            double total1 = Convert.ToDouble(gradepay);
                                            double total3 = 0;
                                            if (da1.Trim() != "")
                                            {
                                                total3 = Convert.ToDouble(da1);
                                            }
                                            double total4 = Convert.ToDouble(remain);
                                            directall = directall + Convert.ToDouble(total3);
                                            double Totaladd = total + total1 + total3 + total4;
                                            tot = tot + Convert.ToDouble(Totaladd);
                                            double finalhra = 0;
                                            if (hra.Trim() != "")
                                            {
                                                finalhra = Convert.ToDouble(hra);
                                            }
                                            hrallw = hrallw + Convert.ToDouble(finalhra);
                                            double finaltot = Totaladd + finalhra;
                                            ftot = ftot + Convert.ToInt32(finaltot);
                                            fspl = fspl + Convert.ToDouble(total4);
                                            table.Cell(row4, 6).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table.Cell(row4, 6).SetContent(Convert.ToDouble(Totaladd));
                                            table.Cell(row4, 6).SetContent(Convert.ToString(Math.Round(Convert.ToDouble(Totaladd))));
                                            table.Cell(row4, 8).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table.Cell(row4, 8).SetContent(Convert.ToString(Math.Round(Convert.ToDouble(finaltot))));
                                            table.Cell(totalrowcount - 2, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table.Cell(totalrowcount - 2, 2).SetContent(Math.Round(Convert.ToDouble(pay)));
                                            table.Cell(totalrowcount - 2, 3).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table.Cell(totalrowcount - 2, 3).SetContent(Math.Round(Convert.ToDouble(gp)));
                                            table.Cell(totalrowcount - 2, 4).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table.Cell(totalrowcount - 2, 4).SetContent(Math.Round(Convert.ToDouble(directall)));
                                            table.Cell(totalrowcount - 2, 5).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table.Cell(totalrowcount - 2, 5).SetContent(Math.Round(Convert.ToDouble(fspl)));
                                            table.Cell(totalrowcount - 2, 6).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table.Cell(totalrowcount - 2, 6).SetContent(Math.Round(Convert.ToDouble(tot)));
                                            table.Cell(totalrowcount - 2, 7).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table.Cell(totalrowcount - 2, 7).SetContent(Math.Round(Convert.ToDouble(hrallw)));
                                            table.Cell(totalrowcount - 2, 8).SetContentAlignment(ContentAlignment.MiddleRight);
                                            //int ftot1 = Convert.ToInt32(ftot);
                                            table.Cell(totalrowcount - 1, 9).SetContent("GRAND TOTAL Rs." + ftot + "");
                                            //  table.Cell(totalrowcount - 1, 9).SetContent(Convert.ToString(Math.Round(Convert.ToDouble(ftot))));
                                            int lastcount = 0;
                                            Hashtable hashtablefinaltotal = new Hashtable();
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                bool flage11 = false;
                                                string[] splitfirstarray1 = dedect.Split('\\');
                                                if (splitfirstarray1.Length > 0)
                                                {
                                                    int count1 = 8;
                                                    for (int arr1 = 0; arr1 < splitfirstarray1.GetUpperBound(0); arr1++)
                                                    {
                                                        count1++;
                                                        string secondvalue = Convert.ToString(splitfirstarray1[arr1]);
                                                        if (secondvalue.Trim() != "")
                                                        {
                                                            string[] splitsecond = secondvalue.Split(';');
                                                            if (splitsecond.Length > 0)
                                                            {
                                                                string typevalue = Convert.ToString(splitsecond[0]);
                                                                table.Cell(row4, count1).SetContentAlignment(ContentAlignment.MiddleRight);
                                                                da1 = Convert.ToString(splitsecond[3]);
                                                                string fn_tot = "";
                                                                fn_tot = Convert.ToString(splitsecond[1]);
                                                                if (da1.Trim() != "")
                                                                {
                                                                    table.Cell(row4, count1).SetContent(Convert.ToString(Math.Round(Convert.ToDouble(splitsecond[3]))));
                                                                    lastcount = lastcount + Convert.ToInt32(Math.Round(Convert.ToDouble(splitsecond[3])));
                                                                }
                                                                else
                                                                {
                                                                    table.Cell(row4, count1).SetContent(0);
                                                                }
                                                                if (fn_tot.Trim() != "")
                                                                {
                                                                    table.Cell(row4, columncount - 1).SetContentAlignment(ContentAlignment.MiddleRight);
                                                                    table.Cell(row4, columncount - 1).SetContent(lastcount);
                                                                }
                                                                double finalcount = 0;
                                                                if (!hasadd.Contains(typevalue))
                                                                {
                                                                    hasadd.Add(typevalue, da1);
                                                                }
                                                                else
                                                                {
                                                                    double read = Convert.ToDouble(hasadd[typevalue]);
                                                                    finalcount = read + Convert.ToDouble(da1);
                                                                    hasadd.Remove(typevalue);
                                                                    hasadd.Add(typevalue, finalcount);
                                                                }
                                                                if (typevalue == "PROF TAX")
                                                                {
                                                                    flage11 = true;
                                                                    xy = xy + 100;
                                                                    drproftax = dtproftax.NewRow();
                                                                    drproftax[0] = Convert.ToString(month[va]);
                                                                    drproftax[1] = Convert.ToString(Math.Round(Convert.ToDouble(splitsecond[3])));
                                                                    dtproftax.Rows.Add(drproftax);
                                                                }
                                                                if (typevalue == "HRR")
                                                                {
                                                                    if (Convert.ToString(splitsecond[3]) != "")
                                                                    {
                                                                        hrr1 = hrr1 + Convert.ToDouble(splitsecond[3]);
                                                                    }
                                                                    else
                                                                    {
                                                                        hrr1 = hrr1 + 0;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                lastcount1 = lastcount1 + Convert.ToDouble(lastcount);
                                                if (flage11 == false)
                                                {
                                                    drproftax = dtproftax.NewRow();
                                                    drproftax[0] = Convert.ToString(month[va]);
                                                    drproftax[1] = Convert.ToString(0);
                                                    dtproftax.Rows.Add(drproftax);
                                                }
                                            }
                                        }
                                        if (ds.Tables[1].Rows.Count > 0)
                                        {
                                            string tax = Convert.ToString(ds.Tables[1].Rows[0][0]);
                                            if (tax.Trim() != "")
                                            {
                                                taxpaid = taxpaid + Convert.ToDouble(tax);
                                            }
                                            else
                                            {
                                                taxpaid = taxpaid + 0;
                                            }
                                        }
                                        else
                                        {
                                            taxpaid = taxpaid + 0;
                                        }
                                        int cn = 8;
                                        if (column.Count > 0)
                                        {
                                            for (int i = 0; i < column.Count; i++)
                                            {
                                                cn++;
                                                string value = Convert.ToString(column[i]);
                                                double val_count = Convert.ToDouble(hasadd[value]);
                                                table.Cell(totalrowcount - 2, cn).SetContentAlignment(ContentAlignment.MiddleRight);
                                                table.Cell(totalrowcount - 2, cn).SetContent(Convert.ToString(val_count));
                                            }
                                        }
                                    }
                                }
                            }
                            table.Cell(totalrowcount - 2, columncount - 1).SetContentAlignment(ContentAlignment.MiddleRight);
                            table.Cell(totalrowcount - 2, columncount - 1).SetContent(lastcount1);
                            table.Cell(totalrowcount - 3, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                            table.Cell(totalrowcount - 3, 2).SetContent(Convert.ToString(Math.Round(Convert.ToDouble(taxpaid))));
                            Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 320, 550, 550));
                            mypdfpage.Add(newpdftabpage);
                            Double getheigh = newpdftabpage.Area.Height;
                            xy = Convert.ToInt32(getheigh) + 330;
                            int ypos = xy;
                            if (dtproftax.Rows.Count > 0)
                            {
                                Gios.Pdf.PdfTable table3 = mydoc.NewTable(Fontbold, dtproftax.Rows.Count, 3, 1);
                                table3.SetBorders(Color.Black, 1, BorderType.None);
                                table3.VisibleHeaders = false;
                                for (int j = 0; j < dtproftax.Rows.Count; j++)
                                {
                                    table3.Cell(j, 0).SetContent("PROFESSIONAL TAX - " + dtproftax.Rows[j][0].ToString() + "");
                                    table3.Cell(j, 1).SetContent(":");
                                    table3.Cell(j, 2).SetContent(dtproftax.Rows[j][1].ToString());
                                    table3.Cell(j, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table3.Cell(j, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table3.Cell(j, 0).SetFont(Fontbold);
                                    table3.Columns[0].SetWidth(150);
                                    table3.Columns[1].SetWidth(10);
                                    table3.Columns[2].SetWidth(40);
                                }
                                newpdftabpage = table3.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, xy, 300, 550));
                                mypdfpage.Add(newpdftabpage);
                                getheigh = newpdftabpage.Area.Height;
                                xy = Convert.ToInt32(getheigh) + ypos - 10;
                                PdfTextArea pdfcoldh = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 260, xy, 400, 30), System.Drawing.ContentAlignment.BottomLeft, "_ _ _ _ _ _ _ _ _");
                                xy = xy + 10;
                                PdfTextArea pdftaxtot = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, xy + 3, 400, 30), System.Drawing.ContentAlignment.BottomLeft, " TOTAL PROFESSIONAL TAX");
                                PdfTextArea pdftotcol = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 250, xy + 3, 400, 30), System.Drawing.ContentAlignment.BottomLeft, ":");
                                PdfTextArea pdftotcol1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 90, xy + 6, 400, 30), System.Drawing.ContentAlignment.BottomCenter, Convert.ToString(proftax));
                                xy = xy + 10;
                                PdfTextArea pdfcoldh1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 260, xy, 400, 30), System.Drawing.ContentAlignment.BottomLeft, "_ _ _ _ _ _ _ _ _");
                                if (xy < 600)
                                {
                                    PdfTextArea pdftaxtotrent = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, 700, 400, 30), System.Drawing.ContentAlignment.BottomLeft, " TOTAL HOUSE RENT RECOVERY FOR THE YEAR   " + yr1 + "-" + yr2 + "   :    " + hrr1 + "");
                                    mypdfpage.Add(pdftaxtotrent);
                                }
                                else
                                {
                                    PdfTextArea pdftaxtotrent = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, xy + 20, 400, 30), System.Drawing.ContentAlignment.BottomLeft, " TOTAL HOUSE RENT RECOVERY FOR THE YEAR   " + yr1 + "-" + yr2 + "   :    " + hrr1 + "");
                                    mypdfpage.Add(pdftaxtotrent);
                                }
                                mypdfpage.Add(pdfcoldh);
                                mypdfpage.Add(pdftaxtot);
                                mypdfpage.Add(pdfcoldh1);
                                mypdfpage.Add(pdftotcol);
                                mypdfpage.Add(pdfst_sig);
                                mypdfpage.Add(pdftotcol1);
                            }
                            else
                            {
                                PdfTextArea pdfcoldh = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 260, ypos - 10, 400, 30), System.Drawing.ContentAlignment.BottomLeft, "_ _ _ _ _ _ _ _ _");
                                xy = xy + 10;
                                PdfTextArea pdftaxtot = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, ypos + 5, 400, 30), System.Drawing.ContentAlignment.BottomLeft, " TOTAL PROFESSIONAL TAX");
                                PdfTextArea pdftotcol = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 250, ypos + 5, 400, 30), System.Drawing.ContentAlignment.BottomLeft, ":");
                                PdfTextArea pdftotcol1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 90, ypos + 5, 400, 30), System.Drawing.ContentAlignment.BottomCenter, Convert.ToString(proftax));
                                xy = xy + 10;
                                PdfTextArea pdfcoldh1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 260, ypos + 10, 400, 30), System.Drawing.ContentAlignment.BottomLeft, "_ _ _ _ _ _ _ _ _");
                                PdfTextArea pdftaxtotrent = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, 600, 400, 30), System.Drawing.ContentAlignment.BottomLeft, " TOTAL HOUSE RENT RECOVERY FOR THE YEAR   " + yr1 + "-" + yr2 + "   :    " + hrr1 + "");
                                mypdfpage.Add(pdftaxtotrent);
                                mypdfpage.Add(pdfcoldh);
                                mypdfpage.Add(pdftaxtot);
                                mypdfpage.Add(pdfcoldh1);
                                mypdfpage.Add(pdftotcol);
                                mypdfpage.Add(pdfst_sig);
                                mypdfpage.Add(pdftotcol1);
                            }
                            mypdfpage.Add(pdf);
                            mypdfpage.Add(pdfaddr1);
                            mypdfpage.Add(pdfaddr2);
                            mypdfpage.Add(pdf1);
                            mypdfpage.Add(pdf11);
                            mypdfpage.Add(pdf111);
                            mypdfpage.Add(pdf2);
                            mypdfpage.Add(pdf22);
                            mypdfpage.Add(pdf222);
                            mypdfpage.Add(pdf3);
                            mypdfpage.Add(pdf333);
                            mypdfpage.Add(pdfdesign);
                            mypdfpage.Add(pdfpan_no);
                            mypdfpage.Add(pdf4);
                            mypdfpage.Add(pdf444);
                            mypdfpage.Add(pdffname);
                            mypdfpage.Add(pdffnamecol);
                            mypdfpage.Add(pdf5);
                            mypdfpage.Add(pdf55);
                            mypdfpage.Add(pdf555);
                            mypdfpage.Add(pdf6);
                            mypdfpage.Add(pdf66);
                            mypdfpage.Add(pdf666);
                            mypdfpage.Add(pdf7);
                            mypdfpage.Add(pdf77);
                            mypdfpage.Add(pdf77nd);
                            mypdfpage.Add(pdf77age);
                            mypdfpage.Add(pdf777);
                            mypdfpage.Add(pdf8);
                            mypdfpage.Add(pdf88);
                            mypdfpage.Add(pdf9);
                            mypdfpage.Add(pdf99);
                            mypdfpage.SaveToDocument();
                        }
                        else if (ddltype.SelectedItem.Text == "PF")
                        {
                            column.Clear();
                            int y = 50;
                            Gios.Pdf.PdfPage mypdfpage1 = mydoc.NewPage();
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                mypdfpage1.Add(LogoImage, 20, 10, 450);
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                            {
                                PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                mypdfpage1.Add(LogoImage1, 500, 10, 450);
                            }
                            PdfTextArea pdfclnm = new PdfTextArea(Fontbold4, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 20, 400, 30), System.Drawing.ContentAlignment.TopCenter, collegenew1);
                            PdfTextArea pdfaddr1pg2 = new PdfTextArea(Fontbold3, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 35, 400, 30), System.Drawing.ContentAlignment.TopCenter, address1);
                            PdfTextArea pdfaddr2pg2 = new PdfTextArea(Fontbold3, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 45, 400, 30), System.Drawing.ContentAlignment.TopCenter, address2);
                            Gios.Pdf.PdfTable table1 = mydoc.NewTable(Fontbold1, 4 + addarray.Count, 8, 1);
                            table1 = mydoc.NewTable(Fontbold2, 3 + addarray.Count, 8, 1);
                            table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            int totalrowcount1 = 4 + addarray.Count;
                            foreach (PdfCell pr in table1.CellRange(0, 1, 0, 1).Cells)
                            {
                                pr.ColSpan = 2;
                            }
                            foreach (PdfCell pr in table1.CellRange(0, 3, 0, 3).Cells)
                            {
                                pr.ColSpan = 2;
                            }
                            foreach (PdfCell pr in table1.CellRange(0, 0, 0, 0).Cells)
                            {
                                pr.RowSpan = 2;
                            }
                            foreach (PdfCell pr in table1.CellRange(0, 5, 0, 5).Cells)
                            {
                                pr.RowSpan = 2;
                            }
                            foreach (PdfCell pr in table1.CellRange(0, 6, 0, 6).Cells)
                            {
                                pr.RowSpan = 2;
                            }
                            foreach (PdfCell pr in table1.CellRange(0, 7, 0, 7).Cells)
                            {
                                pr.RowSpan = 2;
                            }
                            int rowt1 = 1;
                            for (int row1 = 0; row1 < month.Count; row1++)
                            {
                                rowt1++;
                                if (row1 == 0 || row1 == month.Count - 1)
                                {
                                    string prev_value = Convert.ToString(month[row1]);
                                    int month_number = Convert.ToInt32(addmonthvalue[prev_value]);
                                    string[] split_value = prev_value.Split(' ');
                                    int yearvalue = Convert.ToInt32(split_value[1]);
                                    month_number = month_number - 1;
                                    if (month_number == 0)
                                    {
                                        month_number = 12;
                                        yearvalue = yearvalue - 1;
                                    }
                                    string key = Convert.ToString(addmonthnumber[Convert.ToString(month_number)]);
                                    table1.Cell(rowt1, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                    table1.Cell(rowt1, 0).SetContent(row1 + 1);
                                    table1.Cell(rowt1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(rowt1, 0).SetContent(Convert.ToString(key + " " + Convert.ToString(yearvalue) + " PAID IN " + month[row1]));
                                }
                                else
                                {
                                    table1.Cell(rowt1, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                    table1.Cell(rowt1, 0).SetContent(row1 + 1);
                                    table1.Cell(rowt1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(rowt1, 0).SetContent(Convert.ToString(month[row1]));
                                }
                            }
                            int rtab1 = 1;
                            double fpf = 0;
                            double pf = 0;
                            double finalsubtot = 0;
                            double fn_wage = 0;
                            for (int tab = 0; tab < addarray.Count; tab++)
                            {
                                rtab1++;
                                string date1 = Convert.ToString(addarray[tab]);
                                if (date1.Trim() != "")
                                {
                                    string[] splitdate1 = date1.Split(',');
                                    if (splitdate1.Length > 0)
                                    {
                                        string firstdate1 = Convert.ToString(splitdate1[0]);
                                        string seconddate1 = Convert.ToString(splitdate1[1]);
                                        string pfquery = " select pf,fpf  from monthlypay  where staff_code='" + staff_code + "' and fdate ='" + firstdate1 + "' and tdate='" + seconddate1 + "' and College_Code='" + college_code + "' ";
                                        pfquery = pfquery + "  select relieve_date,remarks from staffmaster s, stafftrans t  where s.staff_code=t.staff_code and s.staff_code ='" + staff_code + "' and relieve_date between '" + firstdate1 + "' and '" + seconddate1 + "' and college_code ='" + college_code + "' and resign =1 and settled =1 and latestrec =1";
                                        ds3.Clear();
                                        ds3 = da.select_method_wo_parameter(pfquery, "Text");
                                        if (ds3.Tables[0].Rows.Count > 0)
                                        {
                                            string fpfamt = Convert.ToString(ds3.Tables[0].Rows[0]["fpf"]);
                                            if (fpfamt.Trim() != " ")
                                            {
                                                fpf = fpf + Convert.ToDouble(fpfamt);
                                            }
                                            else
                                            {
                                                fpf = fpf + 0;
                                                fpfamt = "0";
                                            }
                                            string pfamt = Convert.ToString(ds3.Tables[0].Rows[0]["pf"]);
                                            if (pfamt.Trim() != "")
                                            {
                                                pf = pf + Convert.ToDouble(pfamt);
                                            }
                                            else
                                            {
                                                pf = pf + 0;
                                                pfamt = "0";
                                            }
                                            table1.Cell(rtab1, 1).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table1.Cell(rtab1, 1).SetContent(Math.Round(Convert.ToDouble(amt_wages)));
                                            table1.Cell(rtab1, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table1.Cell(rtab1, 2).SetContent(Math.Round(Convert.ToDouble(pfamt)));
                                            table1.Cell(rtab1, 4).SetContent(Math.Round(Convert.ToDouble(fpfamt)));
                                            table1.Cell(rtab1, 4).SetContentAlignment(ContentAlignment.MiddleRight);
                                            double amtfp = Convert.ToDouble(fpfamt);
                                            double amtpf = Convert.ToDouble(pfamt);
                                            double amt = amtpf - amtfp;
                                            fn_wage = fn_wage + Convert.ToDouble(amt_wages);
                                            finalsubtot = finalsubtot + Convert.ToDouble(amt);
                                            pffinaltt = pf + fpf + finalsubtot;
                                            table1.Cell(rtab1, 3).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table1.Cell(rtab1, 3).SetContent(Convert.ToString(amt));
                                        }
                                        if (ds3.Tables[1].Rows.Count > 0)
                                        {
                                            string relidate = Convert.ToString(ds3.Tables[0].Rows[0]["relieve_date"]);
                                            string rmks1 = Convert.ToString(ds3.Tables[0].Rows[0]["remarks"]);
                                            table1.Cell(totalrowcount1, 7).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table1.Cell(totalrowcount1, 7).SetContent(Math.Round(Convert.ToDouble(relidate)));
                                            table1.Cell(totalrowcount1, 7).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table1.Cell(totalrowcount1, 7).SetContent(Math.Round(Convert.ToDouble(rmks1)));
                                        }
                                    }
                                }
                            }
                            table1.Cell(totalrowcount1 - 2, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                            table1.Cell(totalrowcount1 - 2, 2).SetContent(Math.Round(Convert.ToDouble(pf)));
                            table1.Cell(totalrowcount1 - 2, 4).SetContentAlignment(ContentAlignment.MiddleRight);
                            table1.Cell(totalrowcount1 - 2, 4).SetContent(Math.Round(Convert.ToDouble(fpf)));
                            table1.Cell(totalrowcount1 - 2, 3).SetContentAlignment(ContentAlignment.MiddleRight);
                            table1.Cell(totalrowcount1 - 2, 3).SetContent(Math.Round(Convert.ToDouble(finalsubtot)));
                            table1.Cell(totalrowcount1 - 2, 1).SetContentAlignment(ContentAlignment.MiddleRight);
                            table1.Cell(totalrowcount1 - 2, 1).SetContent(Math.Round(Convert.ToDouble(fn_wage)));
                            PdfTextArea pdfpara = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 60, 550, 30), System.Drawing.ContentAlignment.TopLeft, "The Employees Provident Fund Scheme,1952 (Paras 35 & 42) and the Employees' Family Pension Scheme, 1995 (Para 19) Form 3A(Revised) Contribution Card for the Currency Period from " + fr_month + "-" + yr1 + "  To " + to_month + "-" + yr2 + " (For Unexempted establishments Only)");
                            PdfTextArea pdf220 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 80, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "1.Account No. :");
                            PdfTextArea pdf220pfno = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 100, y + 80, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, pfno);
                            PdfTextArea pdf221 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 60, y + 80, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "2.Name/Surname :");
                            PdfTextArea pdf221stname = new PdfTextArea(Fontbold2, System.Drawing.Color.Black, new PdfArea(mydoc, 300, y + 82, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, staffname);
                            PdfTextArea pdf221blkltr = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 64, y + 90, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "(In Block Letters)");
                            PdfTextArea pdf221fthrnm = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y + 80, 400, 30), System.Drawing.ContentAlignment.MiddleRight, "3.Father's/              :");
                            PdfTextArea pdf221father = new PdfTextArea(Fontbold2, System.Drawing.Color.Black, new PdfArea(mydoc, 480, y + 82, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, final_nmae.ToUpper());
                            PdfTextArea pdf221husnm = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 76, y + 90, 400, 30), System.Drawing.ContentAlignment.MiddleRight, "Husband's/Name");
                            PdfTextArea pdf222clgnm = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 110, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "4.Name & Address of the   }");
                            PdfTextArea pdf222ftyest = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 45, y + 120, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, " Factory/Establishment     } ");
                            PdfTextArea pdfacro = new PdfTextArea(Fontbold2, System.Drawing.Color.Black, new PdfArea(mydoc, 170, y + 115, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, acronym);
                            PdfTextArea pdf222strate = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 61, y + 110, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "5.Statutory rate      }");
                            PdfTextArea pdf222contri = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 66, y + 120, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "of contribution   }");
                            PdfTextArea pdf222stratepr = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 125, y + 112, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, " 12% ");
                            PdfTextArea pdf222vol = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 93, y + 110, 400, 30), System.Drawing.ContentAlignment.MiddleRight, "6.Voluntary higher    }");
                            PdfTextArea pdf222emprate = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 94, y + 120, 400, 30), System.Drawing.ContentAlignment.MiddleRight, "rate of employer's  }");
                            PdfTextArea pdf222cnif = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 94, y + 130, 400, 30), System.Drawing.ContentAlignment.MiddleRight, "Contribution if any }");
                            PdfTextArea pdfcontrib = new PdfTextArea(Fontbold6, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y + 170, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "CONTRIBUTION");
                            PdfTextArea pdfcontent = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 470, 510, 30), System.Drawing.ContentAlignment.MiddleLeft, "Certified that the total amount of contributions(both shares)indicated in this card i.e Rs." + pffinaltt + " has been already remitted in full in EPF A/c no.1 and Pension Fund A/c no.No.10 ( Vide note below) ");
                            PdfTextArea pdfcontent1 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 510, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "Certified that the difference between the total of the contribution shown under columns 3 & 4a & 4b of the above table and that arrived at on the total wages shown in column 2 at the prescribed rate is solely due to the rounding off the contributions to the nearest rupee under the rules. ");
                            PdfTextArea pdfcontent2 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 560, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Date :");
                            PdfTextArea pdfcontent3 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 170, y + 590, 400, 30), System.Drawing.ContentAlignment.MiddleRight, "Signature of Employer with office seal.");
                            PdfTextArea pdfcontent4 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 620, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "Note : 1) In respect of the Form (3A) sent to the Regional office during the course of the currency period for the purpose of ");
                            PdfTextArea pdfcontent44 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 77, y + 635, 470, 30), System.Drawing.ContentAlignment.MiddleLeft, "final settlement of the accounts of the member who has left service,details of date and reasons for leaving service should be furnished under col. 7(a) & 7(b).");
                            PdfTextArea pdfcontent5 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 660, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "           2) In respect of those who are not members of the Pension Fund the employers share of contribution of the EPF will   ");
                            PdfTextArea pdfcontent55 = new PdfTextArea(Fontbold0, System.Drawing.Color.Black, new PdfArea(mydoc, 77, y + 670, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "be 8-1/3 or 10%  as the case may be & is to be shown under column 4(a).");
                            table1.Columns[0].SetWidth(80);
                            table1.Columns[1].SetWidth(80);
                            table1.Columns[2].SetWidth(80);
                            table1.Columns[3].SetWidth(80);
                            table1.Columns[4].SetWidth(80);
                            table1.Columns[5].SetWidth(90);
                            table1.Columns[6].SetWidth(80);
                            table1.Columns[7].SetWidth(80);
                            table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(0, 1).SetContent("Worker's Share");
                            table1.Cell(0, 1).SetFont(Fontbold);
                            table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(0, 3).SetContent("Employer's Share");
                            table1.Cell(0, 3).SetFont(Fontbold);
                            table1.Cell(0, 0).SetContent("Month");
                            table1.Cell(0, 0).SetFont(Fontbold5);
                            table1.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(1, 1).SetContent("Amount of wages");
                            table1.Cell(1, 1).SetFont(Fontbold5);
                            table1.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(1, 2).SetContent("EPF");
                            table1.Cell(1, 2).SetFont(Fontbold5);
                            table1.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(1, 3).SetContent("EPF Difference between 12% & 8.33%");
                            table1.Cell(1, 3).SetFont(Fontbold5);
                            table1.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(1, 4).SetContent("PENSION FUND Contribution 8.33%");
                            table1.Cell(1, 4).SetFont(Fontbold5);
                            table1.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(0, 5).SetContent("Ref.of Adv");
                            table1.Cell(0, 5).SetFont(Fontbold5);
                            table1.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(0, 6).SetContent("No of days/ Period of Non-Contributing Service(if any)");
                            table1.Cell(0, 6).SetFont(Fontbold5);
                            table1.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(0, 7).SetContent("Remarks");
                            table1.Cell(0, 7).SetFont(Fontbold5);
                            table1.Cell(totalrowcount1 - 2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(totalrowcount1 - 2, 0).SetContent("Total");
                            table1.Cell(totalrowcount1 - 2, 0).SetFont(Fontbold5);
                            mypdfpage1.Add(pdfclnm);
                            mypdfpage1.Add(pdfaddr1pg2);
                            mypdfpage1.Add(pdfaddr2pg2);
                            mypdfpage1.Add(pdf220);
                            mypdfpage1.Add(pdf220pfno);
                            mypdfpage1.Add(pdf221);
                            mypdfpage1.Add(pdf221stname);
                            mypdfpage1.Add(pdf221blkltr);
                            mypdfpage1.Add(pdf221fthrnm);
                            mypdfpage1.Add(pdf221father);
                            mypdfpage1.Add(pdf221husnm);
                            mypdfpage1.Add(pdf222clgnm);
                            mypdfpage1.Add(pdfacro);
                            mypdfpage1.Add(pdf222ftyest);
                            mypdfpage1.Add(pdf222strate);
                            mypdfpage1.Add(pdf222stratepr);
                            mypdfpage1.Add(pdf222contri);
                            mypdfpage1.Add(pdf222vol);
                            mypdfpage1.Add(pdf222emprate);
                            mypdfpage1.Add(pdf222cnif);
                            mypdfpage1.Add(pdfcontrib);
                            mypdfpage1.Add(pdfcontent);
                            mypdfpage1.Add(pdfcontent1);
                            mypdfpage1.Add(pdfcontent2);
                            mypdfpage1.Add(pdfcontent3);
                            mypdfpage1.Add(pdfcontent4);
                            mypdfpage1.Add(pdfcontent44);
                            mypdfpage1.Add(pdfcontent5);
                            mypdfpage1.Add(pdfcontent55);
                            mypdfpage1.Add(pdfpara);
                            Gios.Pdf.PdfTablePage newpdftabpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 30, 260, 550, 550));
                            mypdfpage1.Add(newpdftabpage1);
                            mypdfpage1.SaveToDocument();
                        }
                        string appPath = HttpContext.Current.Server.MapPath("~");
                        if (appPath != "")
                        {
                            string szPath = appPath + "/Report/";
                            string szFile = "IncomeTaxCalculation&PFSettlement" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                            mydoc.SaveToFile(szPath + szFile);
                            Response.ClearHeaders();
                            Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                            Response.ContentType = "application/pdf";
                            Response.WriteFile(szPath + szFile);
                        }
                    }
                }
                if (flage == false)
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Any One Staff\");", true);
                }
            }
        }
        catch (Exception ex)
        {
            lbl.Visible = true;
            lbl.Text = ex.ToString();
        }
    }
    protected void butgen_Click(object sender, EventArgs e)
    {
        try
        {
            bindpdf();
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void butgen1_Click(object sender, EventArgs e)
    {
        try
        {
            Font Fontbold = new Font("Times new roman", 10, FontStyle.Regular);
            Font Fontbold1 = new Font("Times new roman", 6, FontStyle.Regular);
            Font Fontbold2 = new Font("Times new roman", 7, FontStyle.Regular);
            Font Fontbold3 = new Font("Times new roman", 12, FontStyle.Regular);
            Font Fontbold4 = new Font("Times new roman", 16, FontStyle.Regular);
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            bindpdf();
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    public string FindKey(int Value, Hashtable HT)
    {
        string Key = "";
        IDictionaryEnumerator e = HT.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Value.ToString().Equals(Value))
            {
                Key = e.Key.ToString();
            }
        }
        return Key;
    }
    //Barath 28.02.18
    protected void ItReportTaxDetailsReport()//delsiref 2708
    {
        #region Income Tax Format2
        Fpspread2.Visible = false;
        rptprint.Visible = false;
        #region It Calculation Settings
        string frommonth = ""; string fromyear = ""; string tomonth = ""; string toyear = "";
        DateTime frm_date = new DateTime();
        DateTime to_date = new DateTime();
        string getfromdate = string.Empty;
        string gettodate = string.Empty;


        string itsetting = da.GetFunction("select linkvalue from New_InsSettings where LinkName='IT Calculation Settings' and college_code='" + Convert.ToString(Session["collegecode"]) + "'");
        if (itsetting.Trim() != "0")
        {
            string[] linkvalue = itsetting.Split('-');
            if (linkvalue.Length > 0)
            {
                frommonth = linkvalue[0].Split(',')[0];
                fromyear = linkvalue[0].Split(',')[1];
                tomonth = linkvalue[1].Split(',')[0];
                toyear = linkvalue[1].Split(',')[1];

                getfromdate = frommonth + "/" + "1" + "/" + fromyear;

                frm_date = Convert.ToDateTime(getfromdate);
                int mon = Convert.ToInt32(tomonth);
                int year = Convert.ToInt32(toyear);
                int daysInmonth = System.DateTime.DaysInMonth(year, mon);
                string getday = Convert.ToString(daysInmonth);
                gettodate = tomonth + "/" + getday + "/" + toyear;
                to_date = Convert.ToDateTime(gettodate);

            }
        }
        else
        {
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = "Please Set IT Calculation Settings";
            return;
        }
        #endregion

        string DeptCode = rs.GetSelectedItemsValueAsString(chklistdept);
        string DesigCode = rs.GetSelectedItemsValueAsString(chklistdesign);
        string StaffCode = rs.GetSelectedItemsValueAsString(chkliststname);
        string StaffType = rs.GetSelectedItemsValueAsString(cbl_stafftyp);
        if (!string.IsNullOrEmpty(DeptCode) && !string.IsNullOrEmpty(DesigCode) && !string.IsNullOrEmpty(StaffCode))
        {
            int FontSize = 11;
            DataSet STaffDS = new DataSet();
            STaffDS.Clear();
            if (cb_relived.Checked == false)
            {

                 STaffDS = da.select_method_wo_parameter("select sa.appl_id,sa.appl_no, s.staff_code,s.staff_name,d.desig_name,t.desig_code,t.stftype,c.category_Name,h.dept_name,t.dept_code, pangirnumber, upper(sa.sex)sex,sa.father_name from staffmaster s,staff_appl_master sa ,stafftrans t,hrdept_Master h,desig_Master d ,staffcategorizer c where sa.appl_no=s.appl_no and s.staff_code=t.staff_code and t.dept_code=h.dept_code and t.desig_code=d.desig_code and c.category_code=t.category_code and t.latestrec='1' and s.settled=0 and s.resign=0 and  CollegeCode='" + Convert.ToString(Session["collegecode"]) + "' and h.dept_code in ('" + DeptCode + "') and d.desig_code in ('" + DesigCode + "') and s.staff_code in ('" + StaffCode + "')  and t.stftype in('" + StaffType + "') order by isnull(s.PrintPriority,10000)", "text");
            }
            if (cb_relived.Checked == true)//delsi 2807
            {

                STaffDS = da.select_method_wo_parameter("select resign,settled,sa.appl_id,sa.appl_no, s.staff_code,s.staff_name,d.desig_name,t.desig_code,t.stftype,c.category_Name,h.dept_name,t.dept_code, pangirnumber, upper(sa.sex)sex,sa.father_name from staffmaster s,staff_appl_master sa ,stafftrans t,hrdept_Master h,desig_Master d ,staffcategorizer c where sa.appl_no=s.appl_no and s.staff_code=t.staff_code and t.dept_code=h.dept_code and t.desig_code=d.desig_code and c.category_code=t.category_code and t.latestrec='1'  and ((resign=0 or settled=0) or (resign=1 and relieve_date>='" + frm_date + "') or (resign=1 and relieve_date between '" + frm_date + "' and '" + to_date + "')) and  CollegeCode='" + Convert.ToString(Session["collegecode"]) + "' and h.dept_code in ('" + DeptCode + "') and d.desig_code in ('" + DesigCode + "') and s.staff_code in ('" + StaffCode + "')  and t.stftype in('" + StaffType + "') order by isnull(s.PrintPriority,10000)", "text");
            
            }


            if (STaffDS.Tables[0].Rows.Count > 0)
            {
                int i = 0;
                string IncomeSalary = string.Empty;

                string q1 = "S.No/Staff Code/Staff Name/Designation/Pan No/Gross Amount/HRA/House Rent Allowance/Prof Tax/VI A/Taxable Salary Income/Total Tax Payable/Education Cess/Rebate/Less Prepaid Tax/Balance IncomeTax";

                //    rs.Fpreadheaderbindmethod(q1, Fpspread2, "true");
                Fpspread2.Sheets[0].RowCount = 0;
                Fpspread2.Sheets[0].ColumnCount = 0;
                Fpspread2.CommandBar.Visible = false;
                Fpspread2.Sheets[0].AutoPostBack = true;
                Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread2.Sheets[0].RowHeader.Visible = false;
                int columnCount = Fpspread2.Sheets[0].ColumnCount;
                string[] header = q1.Split('/');
                // Fpspread2.Sheets[0].ColumnCount = header.Count();
                Fpspread2.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;
                Fpspread2.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                Fpspread2.Sheets[0].ColumnCount++;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Text = "S.No";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Size = FontUnit.Medium;
                Fpspread2.Columns[columnCount].Width = 50;
                // Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

                columnCount++;
                Fpspread2.Sheets[0].ColumnCount++;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Text = "Staff Code";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Size = FontUnit.Medium;
                Fpspread2.Columns[columnCount].Width = 100;
                // Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                columnCount++;
                Fpspread2.Sheets[0].ColumnCount++;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Text = "Staff Name";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Size = FontUnit.Medium;
                Fpspread2.Columns[columnCount].Width = 200;
                //  Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

                columnCount++;
                Fpspread2.Sheets[0].ColumnCount++;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Text = "Designation";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Size = FontUnit.Medium;
                Fpspread2.Columns[columnCount].Width = 200;
                // Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);

                columnCount++;
                Fpspread2.Sheets[0].ColumnCount++;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Text = "Pan No";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Size = FontUnit.Medium;
                Fpspread2.Columns[columnCount].Width = 200;
                //  Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                columnCount++;
                Fpspread2.Sheets[0].ColumnCount++;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Text = "Gross Amount";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Size = FontUnit.Medium;
                Fpspread2.Columns[columnCount].Width = 200;
                // Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                columnCount++;
                Fpspread2.Sheets[0].ColumnCount++;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Text = "HRA";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Size = FontUnit.Medium;
                Fpspread2.Columns[columnCount].Width = 200;
                //  Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                columnCount++;
                Fpspread2.Sheets[0].ColumnCount++;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Text = "House Rent Allowance";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Size = FontUnit.Medium;
                Fpspread2.Columns[columnCount].Width = 200;
                //  Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
                columnCount++;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Text = "Prof Tax";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Bold = true;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Name = "Book Antiqua";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Size = FontUnit.Medium;
                //Fpspread2.Columns[columnCount].Width = 200;
                //columnCount++;

                // int columncount = Fpspread2.Sheets[0].ColumnCount;

                DataSet headerbindDS = new DataSet();
                DataView dvs = new DataView();
                DataView dvnews = new DataView();
                string ITTypes = string.Empty;
                string ITCommons = string.Empty;
                string ITCommonValues = string.Empty;


                q1 = "select ITGroupPK,GroupName,GroupDesc,MaxLimitAmount from IT_GroupMaster where parentCode='0' and collegeCode='" + college_code + "' order by isnull(Priority,10000) asc";
                q1 = q1 + " select ITGroupPK,GroupName,GroupDesc,ParentCode,ITGroupType,IT_IDFK,ITType,ITAllowDeductName,ITAllowDeductDiscription,ITCommon,ITCommonValue from IT_GroupMaster IT, IT_GroupMapping IM,IT_OtherAllowanceDeducation AD where IT.ITGroupPK=IM.ITGroupFK and AD.IT_ID=IM.IT_IDFK and IT.CollegeCode='" + college_code + "'";
                q1 += " select distinct ITGroupPK,GroupName,GroupDesc,MaxLimitAmount,parentCode,isnull(Priority,10000) from IT_GroupMaster IT,IT_GroupMapping IM where IT.ITGroupPk=IM.ITGroupFK and collegeCode='" + college_code + "' order by isnull(Priority,10000) asc";

                headerbindDS = d2.select_method_wo_parameter(q1, "text");
                for (int k = 0; k < headerbindDS.Tables[0].Rows.Count; k++)
                {
                    headerbindDS.Tables[2].DefaultView.RowFilter = "parentCode='" + Convert.ToString(headerbindDS.Tables[0].Rows[k]["ITGroupPK"]) + "'";
                    dvs = headerbindDS.Tables[2].DefaultView;
                    if (dvs.Count > 0)
                    {
                        for (int intn = 0; intn < dvs.Count; intn++)
                        {
                            headerbindDS.Tables[1].DefaultView.RowFilter = "ITGroupPK='" + Convert.ToString(dvs[intn]["ITGroupPK"]) + "'";
                            dvnews = headerbindDS.Tables[1].DefaultView;
                            if (dvnews.Count > 0)
                            {
                                for (int intCh = 0; intCh < dvnews.Count; intCh++)
                                {
                                    Fpspread2.Sheets[0].ColumnCount++;
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Text = Convert.ToString(dvnews[intCh]["ITAllowDeductDiscription"]);//delsi0503
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Tag = Convert.ToString(dvnews[intCh]["ITAllowDeductName"]);
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Bold = true;
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Name = "Book Antiqua";
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Size = FontUnit.Medium;
                                    Fpspread2.Columns[columnCount].Width = 100;


                                    columnCount++;

                                    ITTypes = Convert.ToString(dvnews[intCh]["ITType"]);

                                    ITCommons = Convert.ToString(dvnews[intCh]["ITCommon"]);
                                    ITCommonValues = Convert.ToString(dvnews[intCh]["ITCommonValue"]);
                                }


                            }
                        }
                    }
                    else
                    {

                        headerbindDS.Tables[1].DefaultView.RowFilter = "ITGroupPK='" + Convert.ToString(headerbindDS.Tables[0].Rows[k]["ITGroupPK"]) + "'";
                        dvnews = headerbindDS.Tables[1].DefaultView;
                        if (dvnews.Count > 0)
                        {
                            for (int intCh = 0; intCh < dvnews.Count; intCh++)
                            {
                                Fpspread2.Sheets[0].ColumnCount++;
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Text = Convert.ToString(dvnews[intCh]["ITAllowDeductDiscription"]);
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Tag = Convert.ToString(dvnews[intCh]["ITAllowDeductName"]);
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Bold = true;
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Size = FontUnit.Medium;
                                Fpspread2.Columns[columnCount].Width = 100;

                                columnCount++;

                            }
                        }

                    }
                }

                // Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 1, 3);

                //columnCount = columnCount + 2;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Text = "VI A";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Bold = true;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Name = "Book Antiqua";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Size = FontUnit.Medium;
                //Fpspread2.Columns[columnCount].Width = 200;


                //  Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, columnCount, 1, 9);


                // columnCount = columnCount + 9;
                Fpspread2.Sheets[0].ColumnCount++;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Text = "Taxable Salary Income";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Size = FontUnit.Medium;
                Fpspread2.Columns[columnCount].Width = 200;


                // Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, columnCount, 2, 1);
                columnCount++;
                Fpspread2.Sheets[0].ColumnCount++;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Text = "Total Tax Payable";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Size = FontUnit.Medium;
                Fpspread2.Columns[columnCount].Width = 200;

                //   Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, columnCount, 2, 1);

                columnCount++;

                Fpspread2.Sheets[0].ColumnCount++;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Text = "Rebate";//Education Cess
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Size = FontUnit.Medium;
                Fpspread2.Columns[columnCount].Width = 200;

                columnCount++;

                Fpspread2.Sheets[0].ColumnCount++;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Text = "Total tax Payable";//Education Cess
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Size = FontUnit.Medium;
                Fpspread2.Columns[columnCount].Width = 200;


                //Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, columnCount, 2, 1);

                columnCount++;
                Fpspread2.Sheets[0].ColumnCount++;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Text = "Education Cess";//Rebate
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Size = FontUnit.Medium;
                Fpspread2.Columns[columnCount].Width = 200;

                //     Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, columnCount, 2, 1);

                columnCount++;
                Fpspread2.Sheets[0].ColumnCount++;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Text = "Final Tax Payable";//delsi2803
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Size = FontUnit.Medium;
                Fpspread2.Columns[columnCount].Width = 200;



                columnCount++;
                Fpspread2.Sheets[0].ColumnCount++;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Text = "Less Prepaid Tax";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Size = FontUnit.Medium;
                Fpspread2.Columns[columnCount].Width = 200;

                columnCount++;
                Fpspread2.Sheets[0].ColumnCount++;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Text = "Reimbursement";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Size = FontUnit.Medium;
                Fpspread2.Columns[columnCount].Width = 200;

                // Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, columnCount, 2, 1);
                columnCount++;
                Fpspread2.Sheets[0].ColumnCount++;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Text = "Balance IncomeTax";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, columnCount].Font.Size = FontUnit.Medium;
                Fpspread2.Columns[columnCount].Width = 200;
                //Fpspread2.Sheets[0].ColumnCount = columnCount;

                //   Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, columnCount, 2, 1);

                foreach (DataRow drRow in STaffDS.Tables[0].Rows)
                {
                    int column = 0;
                    double HouseRentAmount = 0;
                    int TotMonths = 0;
                    double PayLastMonthAllowance = 0;
                    double PayLastMonthDeduction = 0;
                    int DiffenerceMonth = 0;
                    double PayLastMonthSalary = 0;
                    double lastGradePay = 0;
                    int lastpayMonth = 0;
                    int lastpayYear = 0;
                    double reinvestment = 0;
                    Hashtable PayLastMonthAllowanceHash = new Hashtable();
                    Hashtable PayLastMonthDeductionHash = new Hashtable();
                    string staffcode = Convert.ToString(drRow["staff_code"]);
                    string ApplID = Convert.ToString(drRow["appl_id"]);
                    string Gender = Convert.ToString(drRow["sex"]);
                    string resign = string.Empty;
                    string settle = string.Empty;
                    if (cb_relived.Checked == true)
                    {
                        resign = Convert.ToString(drRow["resign"]);
                        settle = Convert.ToString(drRow["settled"]);
                    }

                    IncomeSalary = da.GetFunction(" select sum(netaddact)netaddact from monthlypay where  CAST(CONVERT(varchar(20),PayMonth)+'/01/'+CONVERT(varchar(20),PayYear) as Datetime) between CAST(CONVERT(varchar(20),'" + frommonth + "')+'/01/'+CONVERT(varchar(20),'" + fromyear + "') as Datetime) and CAST(CONVERT(varchar(20),'" + tomonth + "')+'/01/'+CONVERT(varchar(20),'" + toyear + "') as Datetime) and staff_code = '" + staffcode + "'");
                    double.TryParse(da.GetFunction(" select sum(Amount) as TotalAmount from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType='3' and ((ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "')) and staff_ApplID='" + ApplID + "' and CollegeCode='" + Convert.ToString(Session["collegecode"]) + "'"), out HouseRentAmount);

                    double.TryParse(d2.GetFunction(" select sum(Amount) as TotalAmount from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType='5' and ((ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "')) and staff_ApplID='" + ApplID + "' and CollegeCode='" + Convert.ToString(Session["collegecode"]) + "'"), out reinvestment);//delsi 2509
                    string CalculateAllSet = d2.GetFunction("select linkValue from New_InsSettings where LinkName='Form16 Calculate All Month'  and user_code ='" + usercode + "' and college_code ='" + Convert.ToString(Session["collegecode"]) + "'");
                    if (!string.IsNullOrEmpty(CalculateAllSet) && CalculateAllSet.Trim() != "0")
                    {
                        string CalculateMonthDetQuery = "select paymonth,payyear,netaddact,netadd,addd,deddd,convert(varchar(max), allowances)as allowances,convert(varchar(max),deductions)as deductions,bsalary,grade_pay from monthlypay where CAST(CONVERT(varchar(20),PayMonth)+'/01/'+CONVERT(varchar(20),PayYear) as Datetime) between CAST(CONVERT(varchar(20),'" + frommonth + "')+'/01/'+CONVERT(varchar(20),'" + fromyear + "') as Datetime) and CAST(CONVERT(varchar(20),'" + tomonth + "')+'/01/'+CONVERT(varchar(20),'" + toyear + "') as Datetime) and staff_code = '" + staffcode + "' group by payyear,paymonth,netaddact,netadd,addd,deddd,convert(varchar(max), allowances),convert(varchar(max),deductions),bsalary,grade_pay order by year(payyear),year(paymonth) ";
                        CalculateMonthDetQuery += " select Amount,itmonth,ityear from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType='3' and ( (ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "' ))  and CollegeCode='" + Convert.ToString(Session["collegecode"]) + "' and staff_ApplID='" + ApplID + "' group by ityear,itmonth,Amount order by year(ityear),year(itmonth) ";
                        DataSet CalculateMonthDetDS = d2.select_method_wo_parameter(CalculateMonthDetQuery, "text");
                        if (CalculateMonthDetDS.Tables != null && CalculateMonthDetDS.Tables[0].Rows.Count > 0)
                        {
                            #region payProcessLastMonthSalary
                            double lastMonthSalary = 0;
                            double.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["netaddact"]), out lastMonthSalary);
                            int.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["paymonth"]), out lastpayMonth);
                            int.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["payyear"]), out lastpayYear);
                            double.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["addd"]), out PayLastMonthAllowance);
                            double.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["deddd"]), out PayLastMonthDeduction);
                            double.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["grade_pay"]), out lastGradePay);
                            PayLastMonthAllowanceHash = PayProcessAllowanceDet(CalculateMonthDetDS, 0, CalculateMonthDetDS.Tables[0].Rows.Count - 1, ref PayLastMonthSalary);
                            PayLastMonthDeductionHash = PayProcessDeductionDet(CalculateMonthDetDS, 0, CalculateMonthDetDS.Tables[0].Rows.Count - 1, ref PayLastMonthSalary);
                            DateTime FYearDT = new DateTime(Convert.ToInt32(lastpayYear), Convert.ToInt32(lastpayMonth), 28);
                            DateTime TYearDT = new DateTime(Convert.ToInt32(toyear), Convert.ToInt32(tomonth), 28);
                            DiffenerceMonth = (TYearDT.Month - FYearDT.Month) + 12 * (TYearDT.Year - FYearDT.Year);
                            double CurrentSalary = 0;
                            double.TryParse(IncomeSalary, out CurrentSalary);
                            CurrentSalary += lastMonthSalary * DiffenerceMonth;
                            IncomeSalary = Convert.ToString(CurrentSalary);
                            PayLastMonthSalary *= DiffenerceMonth;
                            lastGradePay *= DiffenerceMonth;
                            #endregion
                            #region ItCalculationLastMonthSalary
                            if (CalculateMonthDetDS.Tables[1].Rows.Count > 0)
                            {
                                double lastAllowanceAndDedutionAmt = 0;
                                int lastAllowanceAnddedutionMonth = 0;
                                int lastAllowanceAnddedutionyear = 0;
                                double.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[1].Rows[CalculateMonthDetDS.Tables[1].Rows.Count - 1]["Amount"]), out lastAllowanceAndDedutionAmt);
                                int.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[1].Rows[CalculateMonthDetDS.Tables[1].Rows.Count - 1]["itmonth"]), out lastAllowanceAnddedutionMonth);
                                int.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[1].Rows[CalculateMonthDetDS.Tables[1].Rows.Count - 1]["ityear"]), out lastAllowanceAnddedutionyear);
                                FYearDT = new DateTime(Convert.ToInt32(lastpayYear), Convert.ToInt32(lastpayMonth), 28);
                                TYearDT = new DateTime(Convert.ToInt32(toyear), Convert.ToInt32(tomonth), 28);
                                int HouseRentDiffenerceMonth = (TYearDT.Month - FYearDT.Month) + 12 * (TYearDT.Year - FYearDT.Year);
                            }
                            #endregion
                        }
                    }
                    double ActualBasicAmount = 0;
                    double.TryParse(IncomeSalary, out ActualBasicAmount);
                    double AdditionAllowance = 0;
                    double.TryParse(d2.GetFunction("select sum(a.AllowanceAmt)AllowanceAmt from AdditionalAllowanceAndDeduction a,CO_MasterValues m where m.MasterCode=AllowanceCode and m.MasterCriteria='additionalallowance' and a.CollegeCode=m.CollegeCode and a.CollegeCode='" + Convert.ToString(Session["collegecode"]) + "' and a.staffcode='" + staffcode + "'"), out AdditionAllowance);


                    DataSet chkotherallow = new DataSet();
                    string qur = " select sum(Amount) as TotalAmount,AllowdeductID,ITAllowDeductType,checkotherallow from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType ='4' and ( (ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "' )) and staff_ApplID='" + ApplID + "' and CollegeCode='" + Convert.ToString(Session["collegecode"]) + "'  group by AllowdeductID,ITAllowDeductType,checkotherallow";

                    chkotherallow = d2.select_method_wo_parameter(qur, "text");
                    double totalotherallow = 0;
                    if (chkotherallow.Tables[0].Rows.Count > 0)
                    {
                        for (int val = 0; val < chkotherallow.Tables[0].Rows.Count; val++)
                        {
                            double getval = 0;
                            string DirectValue1 = Convert.ToString(chkotherallow.Tables[0].Rows[val]["TotalAmount"]);
                            double.TryParse(DirectValue1, out getval);
                            totalotherallow = totalotherallow + getval;

                        }

                    }

                    ActualBasicAmount += AdditionAllowance + totalotherallow;

                    Hashtable AllowanceHash = new Hashtable();
                    Hashtable DeductionHash = new Hashtable();
                    Hashtable IncentiveMasterDeductionHash = new Hashtable();
                    double TotalBasicAmount = 0;
                    double GradePayTotal = 0;
                    double CrossSalaryIncome = 0;
                    string professionalTaxSettings = string.Empty;
                    string ptstmonth = string.Empty;
                    string ptendmonth = string.Empty;
                    string ptstyear = string.Empty;
                    string ptendyear = string.Empty;
                    q1 = "select allowances,deductions,bsalary,grade_pay from monthlypay where CAST(CONVERT(varchar(20),PayMonth)+'/01/'+CONVERT(varchar(20),PayYear) as Datetime) between CAST(CONVERT(varchar(20),'" + frommonth + "')+'/01/'+CONVERT(varchar(20),'" + fromyear + "') as Datetime) and CAST(CONVERT(varchar(20),'" + tomonth + "')+'/01/'+CONVERT(varchar(20),'" + toyear + "') as Datetime) and staff_code = '" + staffcode + "'";
                    q1 += " select LinkValue from New_InsSettings where LinkName='Professional Tax Calculation Month' and college_code ='" + Convert.ToString(Session["collegecode"]) + "' and user_code ='" + usercode + "'";
                    q1 += " select deductions from incentives_master  where college_code = '" + Convert.ToString(Session["collegecode"]) + "' ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q1, "text");
                    if (ds.Tables != null)
                    {
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            string st = Convert.ToString(ds.Tables[2].Rows[0]["deductions"]);
                            string[] split = st.Split(';');
                            foreach (var item in split)
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    string staff = item;
                                    string[] split1 = staff.Split('\\');
                                    string description = split1[0];
                                    string apprivation = split1[1];
                                    if (!IncentiveMasterDeductionHash.ContainsKey(description))
                                        IncentiveMasterDeductionHash.Add(apprivation, description);
                                }
                            }
                        }
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            professionalTaxSettings = Convert.ToString(ds.Tables[1].Rows[0]["LinkValue"]);
                            if (!string.IsNullOrEmpty(professionalTaxSettings.Trim()))
                            {
                                string[] det = professionalTaxSettings.Split(';');
                                string monthyear = det[0];
                                string endmonthyear = det[1];
                                ptstmonth = monthyear.Split('-')[0];
                                ptendmonth = endmonthyear.Split('-')[0];
                                ptstyear = monthyear.Split('-')[1];
                                ptendyear = endmonthyear.Split('-')[1];
                            }
                        }
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        #region Allowance Deduction Calculation
                        for (int intds = 0; intds < ds.Tables[0].Rows.Count; intds++)
                        {
                            string AllowanceValue = Convert.ToString(ds.Tables[0].Rows[intds]["allowances"]);
                            string[] SplitFirst = AllowanceValue.Split('\\');
                            if (SplitFirst.Length > 0)
                            {
                                for (int intc = 0; intc < SplitFirst.Length; intc++)
                                {
                                    if (SplitFirst[intc].Trim() != "")
                                    {
                                        string[] SecondSplit = SplitFirst[intc].Split(';');
                                        if (SecondSplit.Length > 0)
                                        {
                                            double AllowTaeknValue = 0;
                                            string Allow = string.Empty;
                                            string takenValue = SecondSplit[2].ToString();
                                            if (takenValue.Trim().Contains('-'))
                                            {
                                                Allow = takenValue.Split('-')[1];
                                                if (Allow.Trim() != "")
                                                {
                                                    double.TryParse(Allow, out AllowTaeknValue);
                                                }
                                            }
                                            else
                                            {
                                                Allow = SecondSplit[3].ToString();
                                                if (Allow.Trim() != "")
                                                {
                                                    double.TryParse(Allow, out AllowTaeknValue);
                                                }
                                            }
                                            if (!AllowanceHash.ContainsKey(SecondSplit[0].Trim()))
                                            {
                                                AllowanceHash.Add(SecondSplit[0].Trim(), AllowTaeknValue);
                                            }
                                            else
                                            {
                                                double GetValue = Convert.ToDouble(AllowanceHash[SecondSplit[0].Trim()]);
                                                GetValue = GetValue + AllowTaeknValue;
                                                AllowanceHash.Remove(SecondSplit[0].Trim());
                                                AllowanceHash.Add(SecondSplit[0].Trim(), GetValue);
                                            }
                                        }
                                    }
                                }
                            }
                            AllowanceValue = Convert.ToString(ds.Tables[0].Rows[intds]["deductions"]);
                            SplitFirst = AllowanceValue.Split('\\');
                            if (SplitFirst.Length > 0)
                            {
                                for (int intc = 0; intc < SplitFirst.Length; intc++)
                                {
                                    if (SplitFirst[intc].Trim() != "")
                                    {
                                        string[] SecondSplit = SplitFirst[intc].Split(';');
                                        if (SecondSplit.Length > 0)
                                        {
                                            double AllowTaeknValue = 0;
                                            string Allow = string.Empty;
                                            string takenValue = SecondSplit[2].ToString();
                                            if (takenValue.Trim().Contains('-'))
                                            {
                                                Allow = takenValue.Split('-')[1];
                                                if (Allow.Trim() != "")
                                                {
                                                    double.TryParse(Allow, out AllowTaeknValue);
                                                }
                                            }
                                            else
                                            {
                                                Allow = SecondSplit[3].ToString();
                                                if (Allow.Trim() != "")
                                                {
                                                    double.TryParse(Allow, out AllowTaeknValue);
                                                }
                                            }
                                            if (!DeductionHash.ContainsKey(SecondSplit[0].Trim()))
                                            {
                                                DeductionHash.Add(SecondSplit[0].Trim(), AllowTaeknValue);
                                            }
                                            else
                                            {
                                                double GetValue = Convert.ToDouble(DeductionHash[SecondSplit[0].Trim()]);
                                                GetValue = GetValue + AllowTaeknValue;
                                                DeductionHash.Remove(SecondSplit[0].Trim());
                                                DeductionHash.Add(SecondSplit[0].Trim(), GetValue);
                                            }
                                        }
                                    }
                                }
                            }
                            TotalBasicAmount += Convert.ToDouble(ds.Tables[0].Rows[intds]["bsalary"]);
                            GradePayTotal += Convert.ToDouble(ds.Tables[0].Rows[intds]["grade_Pay"]);
                        }
                        #endregion
                        #region allmonth Calculation  15.11.17 barath
                        int Diffmonth = 0;
                        if (lastpayYear != 0 && lastpayMonth != 0)
                        {
                            GradePayTotal += lastGradePay;
                            TotalBasicAmount += PayLastMonthSalary;//25.11.17
                            DateTime FCalYearDT = new DateTime(Convert.ToInt32(lastpayYear), Convert.ToInt32(lastpayMonth), 28);
                            DateTime TCalYearDT = new DateTime(Convert.ToInt32(toyear), Convert.ToInt32(tomonth), 28);
                            Diffmonth = (TCalYearDT.Month - FCalYearDT.Month) + 12 * (TCalYearDT.Year - FCalYearDT.Year);
                            DateTime DummyDT = new DateTime();
                            DummyDT = FCalYearDT;
                            DummyDT = DummyDT.AddMonths(1);
                            TCalYearDT = TCalYearDT.AddMonths(1);
                            if (Diffmonth != 0)
                            {
                                double DeductionAmt = 0;
                                double DeductionValue = 0;
                                while (DummyDT < TCalYearDT)
                                {
                                    #region last Month Allowance
                                    foreach (DictionaryEntry dr in PayLastMonthAllowanceHash)
                                    {
                                        DeductionAmt = 0; DeductionValue = 0;
                                        string AllowanceName = Convert.ToString(dr.Key).Trim();
                                        double.TryParse(Convert.ToString(dr.Value), out DeductionAmt);
                                        if (AllowanceHash.ContainsKey(AllowanceName.Trim()))
                                            double.TryParse(Convert.ToString(AllowanceHash[AllowanceName.Trim()]), out DeductionValue);
                                        DeductionValue += DeductionAmt;
                                        AllowanceHash[AllowanceName.Trim()] = DeductionValue;
                                    }
                                    #endregion
                                    foreach (DictionaryEntry dr in PayLastMonthDeductionHash)
                                    {
                                        string DeductionName = Convert.ToString(dr.Key).Trim();
                                        DeductionAmt = 0;
                                        double.TryParse(Convert.ToString(dr.Value), out DeductionAmt);
                                        DeductionValue = 0;
                                        if (!DeductionHash.ContainsKey(DeductionName.Trim()))
                                        {
                                            if (DeductionName.Trim().ToUpper() == "P.TAX" || DeductionName.Trim().ToUpper() == "P TAX" || DeductionName.Trim() == "PROFESSIONAL TAX" || DeductionName.Trim() == "PROFTAX")
                                            {
                                                if (DummyDT.ToString("MM").TrimStart('0') == ptstmonth && DummyDT.ToString("yyyy") == ptstyear || DummyDT.ToString("MM").TrimStart('0') == ptendmonth && DummyDT.ToString("yyyy") == ptendyear)
                                                    DeductionHash.Add(DeductionName.Trim(), DeductionAmt);
                                                else
                                                    DeductionHash.Add(DeductionName.Trim(), DeductionAmt);
                                            }
                                            else if (DeductionName.Trim().ToUpper() == "INC TAX" || DeductionName.Trim().ToUpper() == "I TAX" || DeductionName.Trim().ToUpper() == "INCOME TAX" || DeductionName.Trim().ToUpper() == "ITAX" || DeductionName.Trim().ToUpper() == "TDS")
                                            { }
                                        }
                                        else
                                        {
                                            if (DeductionName.Trim().ToUpper() == "P.TAX" || DeductionName.Trim().ToUpper() == "P TAX" || DeductionName.Trim() == "PROFESSIONAL TAX" || DeductionName.Trim() == "PROFTAX")
                                            {
                                                if (DummyDT.ToString("MM").TrimStart('0') == ptstmonth && DummyDT.ToString("yyyy") == ptstyear || DummyDT.ToString("MM").TrimStart('0') == ptendmonth && DummyDT.ToString("yyyy") == ptendyear)
                                                {
                                                    double.TryParse(Convert.ToString(DeductionHash[DeductionName.Trim()]), out DeductionValue);
                                                    DeductionValue += DeductionAmt;
                                                    DeductionHash[DeductionName.Trim()] = DeductionValue;
                                                }
                                            }
                                            else if (DeductionName.Trim().ToUpper() == "INC TAX" || DeductionName.Trim().ToUpper() == "I TAX" || DeductionName.Trim().ToUpper() == "INCOME TAX" || DeductionName.Trim().ToUpper() == "ITAX" || DeductionName.Trim().ToUpper() == "TDS"||DeductionName.Trim().ToUpper() =="MISC")
                                            { }
                                            else
                                            {
                                                double.TryParse(Convert.ToString(DeductionHash[DeductionName.Trim()]), out DeductionValue);
                                                DeductionValue += DeductionAmt;
                                                DeductionHash[DeductionName.Trim()] = DeductionValue;
                                            }
                                        }
                                    }
                                    DummyDT = DummyDT.AddMonths(1);
                                }
                            }
                        }
                        // Added by for Additional deduction in Additional Allowance poomalar 04.12.17
                        #region Addition Deduction
                        string queryded = "select AllowanceDeductAmt from AdditionalAllowanceAndDeduction a,CO_MasterValues m where m.MasterCode=AllowanceCode and m.MasterCriteria='additionalallowance' and a.CollegeCode=m.CollegeCode and a.CollegeCode='" + Convert.ToString(Session["collegecode"]) + "' and a.staffcode='" + staffcode + "'";
                        DataSet dsded = new DataSet();
                        dsded = d2.select_method_wo_parameter(queryded, "Text");
                        if (dsded.Tables[0].Rows.Count > 0)
                        {
                            for (int ded = 0; ded < dsded.Tables[0].Rows.Count; ded++)
                            {
                                string splded = Convert.ToString(dsded.Tables[0].Rows[ded]["AllowanceDeductAmt"]);
                                string[] spldedname = splded.Split(';'); double dedvalue = 0;
                                if (spldedname.Length > 0)
                                {
                                    for (int spld = 0; spld < spldedname.Length; spld++)
                                    {
                                        if (spldedname[spld].Contains('-'))
                                        {
                                            string dednameadd = spldedname[spld].Split('-')[0]; //splded.Split(';')[spld].Split('-')[0];
                                            string dedvalueadd = spldedname[spld].Split('-')[1];// splded.Split(';')[spld].Split('-')[1];
                                            double.TryParse(dedvalueadd, out dedvalue);
                                            if (!DeductionHash.ContainsKey(dednameadd))
                                                DeductionHash.Add(dednameadd, dedvalue);
                                            else
                                            {
                                                double value = 0;
                                                double.TryParse(Convert.ToString(DeductionHash[dednameadd]), out value);
                                                value += dedvalue;
                                                DeductionHash[dednameadd] = value;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        #endregion
                        #region House Rent Caluculation
                        string Distict = d2.GetFunction("  select MasterValue from staff_appl_master sa,Staffmaster s ,co_mastervalues c where s.appl_no=sa.appl_no and convert(varchar(max), c.MasterCode)=isnull(Pdistrict,0) and staff_code ='" + staffcode + "'");
                        double HouseRent = 0;
                        double TotalHRA = 0;
                        double DAAmount = 0;
                        double PercentHouseRent = 0;
                        double RentPaidAmount = 0;
                        double HalfPercentofActualSalary = 0;
                        string SalaryDeductHouseRentName = d2.GetFunction(" select distinct CommonDuduction from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType='3' and CommonDuduction is not NULL ");//barath 20.01.18
                        if (!string.IsNullOrEmpty(SalaryDeductHouseRentName))
                        {
                            double SalaryDeductHouseRent = 0;
                            if (DeductionHash.ContainsKey(SalaryDeductHouseRentName))
                                double.TryParse(Convert.ToString(DeductionHash[SalaryDeductHouseRentName]), out SalaryDeductHouseRent);
                            HouseRentAmount += SalaryDeductHouseRent;
                        }
                        if (HouseRentAmount != 0 && TotalBasicAmount != 0)
                        {
                            //double.TryParse(HouseRentAmount, out TotalHouseRentAmount);
                            if (AllowanceHash.ContainsKey("HRA"))
                            {
                                TotalHRA = Convert.ToDouble(AllowanceHash["HRA"]);
                            }
                            if (AllowanceHash.ContainsKey("DA"))
                            {
                                DAAmount = Convert.ToDouble(AllowanceHash["DA"]);
                            }
                            PercentHouseRent = (TotalBasicAmount + GradePayTotal + DAAmount) * 10 / 100;
                            if (PercentHouseRent > HouseRentAmount)
                            {
                                RentPaidAmount = PercentHouseRent - HouseRentAmount;
                            }
                            else
                            {
                                RentPaidAmount = HouseRentAmount - PercentHouseRent;
                            }
                            double gross = 0; double.TryParse(IncomeSalary, out gross); // poo 12.12.17
                            if (Distict.Trim().ToLower() == "chennai" || Distict.Trim().ToLower() == "mumbai" || Distict.Trim().ToLower() == "calcutta" || Distict.Trim().ToLower() == "delhi")
                            {
                                //HalfPercentofActualSalary = (ActualBasicAmount) / 100 * 50; // commented by poo 12.12.17
                                HalfPercentofActualSalary = (gross) / 100 * 50; // poo 12.12.17
                            }
                            else
                            {
                                //HalfPercentofActualSalary = (ActualBasicAmount) / 100 * 40; //commented by poo 12.12.17
                                HalfPercentofActualSalary = (gross) / 100 * 40; // poo 12.12.17
                            }
                            if (TotalHRA < RentPaidAmount && TotalHRA < HalfPercentofActualSalary)
                            {
                                HouseRent = TotalHRA;
                            }
                            else if (RentPaidAmount < TotalHRA && RentPaidAmount < HalfPercentofActualSalary)
                            {
                                HouseRent = RentPaidAmount;
                            }
                            else if (HalfPercentofActualSalary < RentPaidAmount && HalfPercentofActualSalary < TotalHRA)
                            {
                                HouseRent = HalfPercentofActualSalary;
                            }
                        }
                           string selHouseRent = d2.GetFunction("select value from Master_Settings where settings='ViewHouseRent' and usercode='" + usercode + "'");
                        double houserentval = 0;
                        if (selHouseRent.Trim() != "" && selHouseRent.Trim() != "0" && selHouseRent.Trim() == "1")
                        {
                            DataSet houserentcheck = new DataSet();
                            string qry = "select ITGroupPK,GroupName,GroupDesc,MaxLimitAmount from IT_GroupMaster where parentCode='0' and collegeCode='" + Convert.ToString(Session["collegecode"]) + "' order by isnull(Priority,10000) asc";
                            qry = qry + " select ITGroupPK,GroupName,GroupDesc,ParentCode,ITGroupType,IT_IDFK,ITType,ITAllowDeductName,ITAllowDeductDiscription,ITCommon,ITCommonValue,IsAgeRange,MaxValue,MinValue from IT_GroupMaster IT, IT_GroupMapping IM,IT_OtherAllowanceDeducation AD where IT.ITGroupPK=IM.ITGroupFK and AD.IT_ID=IM.IT_IDFK and IT.CollegeCode='" + Convert.ToString(Session["collegecode"]) + "'";
                            qry += " select distinct ITGroupPK,GroupName,GroupDesc,MaxLimitAmount,parentCode,isnull(Priority,10000) from IT_GroupMaster IT,IT_GroupMapping IM where IT.ITGroupPk=IM.ITGroupFK and collegeCode='" + Convert.ToString(Session["collegecode"]) + "' order by isnull(Priority,10000) asc";
                            qry += "  select sum(Amount) as TotalAmount,AllowdeductID,ITAllowDeductType,percentage from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType in   (1,2) and ( (ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "' )) and staff_ApplID='" + ApplID + "' and CollegeCode='" + Convert.ToString(Session["collegecode"]) + "'  group by AllowdeductID,ITAllowDeductType,percentage";
                            qry += " select convert(bigint ,round(FromRange,0)) FromRange,convert(bigint ,round (ToRange,0)) ToRange,Amount,mode  from HR_ITCalculationSettings where collegeCode='" + Convert.ToString(Session["collegecode"]) + "' and sex ='" + Gender.Trim() + "'";
                            qry += "  select sum(Amount) as TotalAmount,AllowdeductID,ITAllowDeductType,percentage from IT_Staff_AllowanceDeduction_Details ID,IT_OtherAllowanceDeducation IA where ID.AllowDeductID=IA.IT_ID and ITAllowdeductType in (2) and ( (ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "' )) and staff_ApplID='" + ApplID + "' and IA.CollegeCode='" + Convert.ToString(Session["collegecode"]) + "' and isnull(IsIncomeTax,'0')='1'  group by AllowdeductID,ITAllowDeductType,percentage";
                            qry += " select IT_ID,ITCommon,ITCommonValue,ITType from IT_OtherAllowanceDeducation  where  isnull(IsIncomeTax,'0')='1'  and CollegeCode='" + Convert.ToString(Session["collegecode"]) + "'";
                            houserentcheck.Clear();
                            houserentcheck = d2.select_method_wo_parameter(qry, "text");
                           
                            DataView dvnew1 = new DataView();
                            DataView dvh = new DataView();
                            if (houserentcheck.Tables.Count > 1 && houserentcheck.Tables[0].Rows.Count > 0)
                            {
                               
                                for (int k = 0; k < houserentcheck.Tables[0].Rows.Count; k++)
                                {
                                  
                                    #region subMain
                                    houserentcheck.Tables[1].DefaultView.RowFilter = "ITGroupPK='" + Convert.ToString(houserentcheck.Tables[0].Rows[k]["ITGroupPK"]) + "'";
                                    dvnew1 = houserentcheck.Tables[1].DefaultView;
                                    if (dvnew1.Count > 0)
                                    {
                                        double MaxLimitAmount = 0;
                                        string MaxAmount = Convert.ToString(houserentcheck.Tables[0].Rows[k]["MaxLimitAmount"]);
                                        double.TryParse(MaxAmount, out MaxLimitAmount);
                                        double OverAllTotal = 0;
                                        for (int intCh = 0; intCh < dvnew1.Count; intCh++)
                                        {
                                            string gettext = Convert.ToString(dvnew1[intCh]["ITAllowDeductName"]);
                                            if (gettext == "Less:Deduction For Interest Payable On Housing Loan")
                                            {
                                                string ITTyp = Convert.ToString(dvnew1[intCh]["ITType"]);
                                                string ITCommo = Convert.ToString(dvnew1[intCh]["ITCommon"]);
                                                string ITCommonValu = Convert.ToString(dvnew1[intCh]["ITCommonValue"]);

                                                houserentcheck.Tables[3].DefaultView.RowFilter = "AllowdeductID='" + Convert.ToString(dvnew1[intCh]["IT_IDFK"]) + "'";
                                                dvh = houserentcheck.Tables[3].DefaultView;
                                                if (dvh.Count > 0)
                                                {
                                                    string DirectValue = Convert.ToString(dvh[0]["TotalAmount"]);

                                                    double.TryParse(DirectValue, out houserentval);
                                                }
                                                if (houserentval > 0)
                                                {
                                                    TotalHRA = 0;
                                                    RentPaidAmount = 0;
                                                    HalfPercentofActualSalary = 0;
                                                    HouseRent = 0;
                                                }
                                            }
                                           

                                        }
                                    #endregion

                                    }

                                }

                            }

                        }
                        double GrossSalary = Convert.ToDouble(ActualBasicAmount) - Math.Round(HouseRent);
                        CrossSalaryIncome = GrossSalary;
                        #endregion

                        double Amt = 0;
                        DataView dv = new DataView();
                        DataView dvnew = new DataView();
                        DataView dAllview = new DataView();
                        DataTable dt = new DataTable();
                        Hashtable settingallow = new Hashtable();
                        string ITType = string.Empty;
                        string ITCommon = string.Empty;
                        string ITCommonValue = string.Empty;
                        double LicAmt = 0;
                        double FirstDeductAmt = 0;
                        double SecondDeductAmt = 0;
                        string maxAgeValue = string.Empty;
                        string minAgeValue = string.Empty;
                        string agechecked = string.Empty;
                        double maxAge = 0;
                        double minAge = 0;
                        double maxVal = 0;
                        double minVal = 0;
                        // string age = d2.GetFunction("select DATEDIFF(yyyy,date_of_birth,getdate()) from staff_appl_master where appl_id='" + ApplID + "'");//delsi0803
                        string age = d2.GetFunction("select  DATEDIFF(yy,date_of_birth,getdate())- CASE WHEN  DATEADD(YY,DATEDIFF(YY,date_of_birth,GETDATE()),date_of_birth) >GETDATE()THEN 1 Else 0 END As [Age] from staff_appl_master where appl_id='" + ApplID + "'");//delsi0604
                        if (Convert.ToInt32(age) >= 60)//delsi2403
                        {
                            if (Gender == "Male" || Gender == "MALE")
                            {

                                Gender = "Senior Citizen Male";
                            }
                            if (Gender == "Female" || Gender == "FEMALE")
                            {
                                Gender = "Senior Citizen Female";

                            }
                            if (Gender == "TransGender" || Gender == "TRANSGENDER")
                            {

                                Gender = "Senior Citizen TransGender";
                            }
                        }


                        q1 = "select ITGroupPK,GroupName,GroupDesc,MaxLimitAmount from IT_GroupMaster where parentCode='0' and collegeCode='" + Convert.ToString(Session["collegecode"]) + "' order by isnull(Priority,10000) asc";
                        q1 = q1 + " select ITGroupPK,GroupName,GroupDesc,ParentCode,ITGroupType,IT_IDFK,ITType,ITAllowDeductName,ITAllowDeductDiscription,ITCommon,ITCommonValue,IsAgeRange,MaxValue,MinValue from IT_GroupMaster IT, IT_GroupMapping IM,IT_OtherAllowanceDeducation AD where IT.ITGroupPK=IM.ITGroupFK and AD.IT_ID=IM.IT_IDFK and IT.CollegeCode='" + Convert.ToString(Session["collegecode"]) + "'";
                        q1 += " select distinct ITGroupPK,GroupName,GroupDesc,MaxLimitAmount,parentCode,isnull(Priority,10000) from IT_GroupMaster IT,IT_GroupMapping IM where IT.ITGroupPk=IM.ITGroupFK and collegeCode='" + Convert.ToString(Session["collegecode"]) + "' order by isnull(Priority,10000) asc";
                        q1 += "  select sum(Amount) as TotalAmount,AllowdeductID,ITAllowDeductType,percentage from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType in   (1,2) and ( (ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "' )) and staff_ApplID='" + ApplID + "' and CollegeCode='" + Convert.ToString(Session["collegecode"]) + "'  group by AllowdeductID,ITAllowDeductType,percentage";
                        q1 += " select convert(bigint ,round(FromRange,0)) FromRange,convert(bigint ,round (ToRange,0)) ToRange,Amount,mode  from HR_ITCalculationSettings where collegeCode='" + Convert.ToString(Session["collegecode"]) + "' and sex ='" + Gender.Trim() + "'";
                        q1 += "  select sum(Amount) as TotalAmount,AllowdeductID,ITAllowDeductType from IT_Staff_AllowanceDeduction_Details ID,IT_OtherAllowanceDeducation IA where ID.AllowDeductID=IA.IT_ID and ITAllowdeductType in (2) and ( (ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "' )) and staff_ApplID='" + ApplID + "' and IA.CollegeCode='" + Convert.ToString(Session["collegecode"]) + "' and isnull(IsIncomeTax,'0')='1'  group by AllowdeductID,ITAllowDeductType";
                        q1 += " select IT_ID,ITCommon,ITCommonValue,ITType from IT_OtherAllowanceDeducation  where  isnull(IsIncomeTax,'0')='1'  and CollegeCode='" + Convert.ToString(Session["collegecode"]) + "'";
                        DataSet ds1 = new DataSet();

                        double staff_age = 0;
                        double.TryParse(age, out staff_age);

                        ds1 = d2.select_method_wo_parameter(q1, "text");
                        if (ds1.Tables.Count > 1 && ds1.Tables[0].Rows.Count > 0)
                        {
                            int RowHeight = 20;
                            for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                            {
                                double CommomOverAllTotal = 0;
                                double GrandCommonTotal = 0;
                                ds1.Tables[2].DefaultView.RowFilter = "parentCode='" + Convert.ToString(ds1.Tables[0].Rows[k]["ITGroupPK"]) + "'";
                                dv = ds1.Tables[2].DefaultView;
                                if (dv.Count > 0)
                                {
                                    #region Main
                                    FirstDeductAmt = Convert.ToDouble(CrossSalaryIncome);

                                    int Cs = 0;
                                    string Commonoverall = Convert.ToString(ds1.Tables[0].Rows[k]["MaxLimitAmount"]);
                                    double.TryParse(Commonoverall, out CommomOverAllTotal);
                                    for (int intn = 0; intn < dv.Count; intn++)
                                    {
                                        ds1.Tables[1].DefaultView.RowFilter = "ITGroupPK='" + Convert.ToString(dv[intn]["ITGroupPK"]) + "'";
                                        dvnew = ds1.Tables[1].DefaultView;
                                        if (dvnew.Count > 0)
                                        {
                                            Cs++;
                                            double MaxLimitAmount = 0;
                                            string MaxAmount = Convert.ToString(dv[intn]["MaxLimitAmount"]);
                                            double.TryParse(MaxAmount, out MaxLimitAmount);
                                            double OverAllTotal = 0;
                                            for (int intCh = 0; intCh < dvnew.Count; intCh++)
                                            {
                                                double AllowAndDeductTotal = 0;
                                                double DirectAllowDeductValue = 0;
                                                double Getvalue = 0;
                                                double AdditionalDeduction = 0;
                                                ITType = Convert.ToString(dvnew[intCh]["ITType"]);
                                                ITCommon = Convert.ToString(dvnew[intCh]["ITCommon"]);
                                                ITCommonValue = Convert.ToString(dvnew[intCh]["ITCommonValue"]);
                                                agechecked = Convert.ToString(dvnew[intCh]["IsAgeRange"]);
                                                maxAgeValue = Convert.ToString(dvnew[intCh]["MaxValue"]);//delsi0803
                                                minAgeValue = Convert.ToString(dvnew[intCh]["MinValue"]);
                                                if (ITCommon.Trim() == "1" || ITCommon.Trim() == "True")
                                                {
                                                    if (ITType.Trim() == "1")
                                                    {
                                                        if (ITCommonValue.Trim() != "")
                                                        {
                                                            if (!string.IsNullOrEmpty(ITCommonValue))
                                                            {
                                                                string[] AllowanceName = ITCommonValue.Split(',');
                                                                foreach (var item in AllowanceName)
                                                                {
                                                                    if (!string.IsNullOrEmpty(item))
                                                                    {
                                                                        if (AllowanceHash.ContainsKey(item.Trim()))
                                                                        {
                                                                            double.TryParse(Convert.ToString(AllowanceHash[item.Trim()]), out AdditionalDeduction);
                                                                            AllowAndDeductTotal += AdditionalDeduction;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            //Getvalue = Convert.ToString(AllowanceHash[ITCommonValue.Trim()]);
                                                        }
                                                    }
                                                    else if (ITType.Trim() == "2")
                                                    {
                                                        if (ITCommonValue.Trim() != "")
                                                        {
                                                            //16.12.17 barath
                                                            if (!string.IsNullOrEmpty(ITCommonValue))
                                                            {
                                                                string[] DeductionName = ITCommonValue.Split(',');
                                                                foreach (var item in DeductionName)
                                                                {
                                                                    if (!string.IsNullOrEmpty(item))
                                                                    {
                                                                        if (DeductionHash.ContainsKey(item.Trim()))
                                                                        {
                                                                            double.TryParse(Convert.ToString(DeductionHash[item.Trim()]), out AdditionalDeduction);
                                                                            AllowAndDeductTotal += AdditionalDeduction;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            //Getvalue = Convert.ToString(DeductionHash[ITCommonValue.Trim()]);
                                                        }
                                                    }
                                                    //double.TryParse(Getvalue, out AllowAndDeductTotal);
                                                }
                                                ds1.Tables[3].DefaultView.RowFilter = "AllowdeductID='" + Convert.ToString(dvnew[intCh]["IT_IDFK"]) + "'";
                                                dAllview = ds1.Tables[3].DefaultView;
                                                if (dAllview.Count > 0)
                                                {
                                                    string DirectValue = Convert.ToString(dAllview[0]["TotalAmount"]);
                                                    double.TryParse(DirectValue, out DirectAllowDeductValue);
                                                    if (Convert.ToString(dvnew[intCh]["ITAllowDeductName"]).ToUpper() == "LIC")//jayaram
                                                        LicAmt = DirectAllowDeductValue;
                                                    else if (Convert.ToString(dvnew[intCh]["ITAllowDeductName"]).ToUpper() == "LIFE INSURANCE")
                                                        LicAmt = DirectAllowDeductValue;
                                                    else if (Convert.ToString(dvnew[intCh]["ITAllowDeductName"]).ToUpper() == "LIFE INSURANCE(LIC)")
                                                        LicAmt = DirectAllowDeductValue;
                                                }
                                                else //barath 21.11.17
                                                {
                                                    if (Convert.ToString(dvnew[intCh]["ITAllowDeductName"]).Trim().Contains('/'))
                                                    {
                                                        string[] DeductionName = Convert.ToString(dvnew[intCh]["ITAllowDeductName"]).Trim().Split('/');
                                                        double DedutionAmt = 0;
                                                        foreach (string deduct in DeductionName)
                                                        {
                                                            DedutionAmt = 0;
                                                            string deductShort = Convert.ToString(IncentiveMasterDeductionHash[deduct]);
                                                            if (!string.IsNullOrEmpty(deductShort))
                                                            {
                                                                if (DeductionHash.ContainsKey(deductShort))
                                                                    double.TryParse(Convert.ToString(DeductionHash[deductShort]), out DedutionAmt);
                                                            }
                                                            DirectAllowDeductValue += DedutionAmt;
                                                        }
                                                    }
                                                }
                                                DirectAllowDeductValue += AllowAndDeductTotal;

                                                if (age != "0")
                                                {
                                                    if (agechecked.Trim() == "1" || agechecked.Trim() == "True")//delsi0803
                                                    {
                                                        string[] maxArr = maxAgeValue.Split('-');
                                                        string[] minArr = minAgeValue.Split('-');
                                                        if (maxArr.Length == 2)
                                                        {
                                                            double.TryParse(Convert.ToString(maxArr[0]), out maxAge);
                                                            double.TryParse(Convert.ToString(maxArr[1]), out maxVal);
                                                        }
                                                        if (minArr.Length == 2)
                                                        {
                                                            double.TryParse(Convert.ToString(minArr[0]), out minAge);
                                                            double.TryParse(Convert.ToString(minArr[1]), out minVal);
                                                        }
                                                        //if (maxAge != 0 && maxAge <= staff_age)
                                                        //    if (maxVal != 0 && maxVal > DirectAllowDeductValue)
                                                        //        DirectAllowDeductValue = maxVal;
                                                        //if (minAge != 0 && minAge > staff_age)
                                                        //    if (minVal != 0 && DirectAllowDeductValue > minVal)
                                                        //        DirectAllowDeductValue = minVal;
                                                        if (maxAge != 0 && maxAge < staff_age)
                                                            if (maxVal != 0 && maxVal < DirectAllowDeductValue)//delsi09ref

                                                                DirectAllowDeductValue = maxVal;
                                                        if (minAge != 0 && minAge > staff_age)
                                                            if (minVal != 0 && DirectAllowDeductValue > minVal)
                                                                DirectAllowDeductValue = minVal;
                                                    }
                                                }
                                                OverAllTotal += DirectAllowDeductValue;
                                            }
                                            string MaxWord = string.Empty;
                                            if (MaxLimitAmount != 0)
                                            {
                                                MaxWord = " restricted to Rs." + MaxLimitAmount + "/-";
                                            }
                                            if (MaxLimitAmount != 0 && MaxLimitAmount > OverAllTotal)
                                            {
                                                GrandCommonTotal += OverAllTotal;
                                            }
                                            else if (MaxLimitAmount != 0 && OverAllTotal > MaxLimitAmount)
                                            {
                                                GrandCommonTotal += MaxLimitAmount;
                                            }
                                            else
                                            {
                                                GrandCommonTotal += OverAllTotal;
                                            }
                                        }
                                    }
                                    string WordMax = string.Empty;
                                    if (CommomOverAllTotal != 0)
                                    {
                                        WordMax = " restricted to Rs." + CommomOverAllTotal + "/-";
                                    }
                                    double MainAmount = 0;
                                    if (CommomOverAllTotal != 0 && CommomOverAllTotal > GrandCommonTotal)
                                    {
                                        MainAmount = GrandCommonTotal;
                                    }
                                    else if (CommomOverAllTotal != 0 && GrandCommonTotal > CommomOverAllTotal)
                                    {
                                        MainAmount = CommomOverAllTotal;
                                    }
                                    else
                                    {
                                        MainAmount = GrandCommonTotal;
                                    }
                                    if (ITType.Trim() == "1")
                                    {
                                        GrossSalary = Convert.ToDouble(CrossSalaryIncome) + Math.Round(MainAmount);
                                        CrossSalaryIncome = GrossSalary;
                                    }
                                    else if (ITType.Trim() == "2")
                                    {
                                        GrossSalary = Convert.ToDouble(CrossSalaryIncome) - Math.Round(MainAmount);
                                        CrossSalaryIncome = GrossSalary;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region subMain
                                    ds1.Tables[1].DefaultView.RowFilter = "ITGroupPK='" + Convert.ToString(ds1.Tables[0].Rows[k]["ITGroupPK"]) + "'";
                                    dvnew = ds1.Tables[1].DefaultView;
                                    if (dvnew.Count > 0)
                                    {
                                        double MaxLimitAmount = 0;
                                        string MaxAmount = Convert.ToString(ds1.Tables[0].Rows[k]["MaxLimitAmount"]);
                                        double.TryParse(MaxAmount, out MaxLimitAmount);
                                        double OverAllTotal = 0;
                                        for (int intCh = 0; intCh < dvnew.Count; intCh++)
                                        {
                                            double AllowAndDeductTotal = 0;
                                            double DirectAllowDeductValue = 0;
                                            string Getvalue = string.Empty;
                                            ITType = Convert.ToString(dvnew[intCh]["ITType"]);
                                            ITCommon = Convert.ToString(dvnew[intCh]["ITCommon"]);
                                            ITCommonValue = Convert.ToString(dvnew[intCh]["ITCommonValue"]);
                                            agechecked = Convert.ToString(dvnew[intCh]["IsAgeRange"]);
                                            maxAgeValue = Convert.ToString(dvnew[intCh]["MaxValue"]);//delsi0803
                                            minAgeValue = Convert.ToString(dvnew[intCh]["MinValue"]);
                                            if (ITCommon.Trim() == "1" || ITCommon.Trim() == "True")
                                            {
                                                if (ITType.Trim() == "1")
                                                {
                                                    if (ITCommonValue.Trim() != "")
                                                    {
                                                        Getvalue = Convert.ToString(AllowanceHash[ITCommonValue.Trim()]);
                                                    }
                                                }
                                                else if (ITType.Trim() == "2")
                                                {
                                                    if (ITCommonValue.Trim() != "")
                                                    {
                                                        Getvalue = Convert.ToString(DeductionHash[ITCommonValue.Trim()]);
                                                    }
                                                }
                                                double.TryParse(Getvalue, out AllowAndDeductTotal);
                                            }
                                            ds1.Tables[3].DefaultView.RowFilter = "AllowdeductID='" + Convert.ToString(dvnew[intCh]["IT_IDFK"]) + "'";
                                            dAllview = ds1.Tables[3].DefaultView;
                                            if (dAllview.Count > 0)
                                            {
                                                string percentage_val = Convert.ToString(dAllview[0]["Percentage"]);//delsi2209
                                                string DirectValue = Convert.ToString(dAllview[0]["TotalAmount"]);
                                                if (percentage_val != "" && percentage_val != null)
                                                {
                                                    double calculatepercent = (Convert.ToDouble(DirectValue) / 100) * Convert.ToDouble(percentage_val);
                                                    DirectValue = Convert.ToString(calculatepercent);

                                                }
                                                double.TryParse(DirectValue, out DirectAllowDeductValue);
                                            }
                                            DirectAllowDeductValue += AllowAndDeductTotal;

                                            if (age != "0")
                                            {
                                                if (agechecked.Trim() == "1" || agechecked.Trim() == "True")//delsi0803
                                                {
                                                    string[] maxArr = maxAgeValue.Split('-');
                                                    string[] minArr = minAgeValue.Split('-');
                                                    if (maxArr.Length == 2)
                                                    {
                                                        double.TryParse(Convert.ToString(maxArr[0]), out maxAge);
                                                        double.TryParse(Convert.ToString(maxArr[1]), out maxVal);
                                                    }
                                                    if (minArr.Length == 2)
                                                    {
                                                        double.TryParse(Convert.ToString(minArr[0]), out minAge);
                                                        double.TryParse(Convert.ToString(minArr[1]), out minVal);
                                                    }
                                                    //if (maxAge != 0 && maxAge <= staff_age)
                                                    //    if (maxVal != 0 && maxVal > DirectAllowDeductValue)
                                                    //        DirectAllowDeductValue = maxVal;
                                                    //if (minAge != 0 && minAge > staff_age)
                                                    //    if (minVal != 0 && DirectAllowDeductValue > minVal)
                                                    //        DirectAllowDeductValue = minVal;

                                                    if (maxAge != 0 && maxAge < staff_age)
                                                        if (maxVal != 0 && maxVal < DirectAllowDeductValue)//delsi09ref

                                                            DirectAllowDeductValue = maxVal;
                                                    if (minAge != 0 && minAge > staff_age)
                                                        if (minVal != 0 && DirectAllowDeductValue > minVal)
                                                            DirectAllowDeductValue = minVal;
                                                }
                                            }
                                            OverAllTotal += DirectAllowDeductValue;
                                        }
                                        double MainAmount = 0;
                                        string MaxWord = string.Empty;
                                        if (MaxLimitAmount != 0)
                                        {
                                            MaxWord = " restricted to Rs." + MaxLimitAmount + "/-";
                                        }
                                        if (MaxLimitAmount != 0 && MaxLimitAmount > OverAllTotal)
                                        {
                                            MainAmount = OverAllTotal;
                                        }
                                        else if (MaxLimitAmount != 0 && OverAllTotal > MaxLimitAmount)
                                        {
                                            MainAmount = MaxLimitAmount;
                                        }
                                        else
                                        {
                                            MainAmount = OverAllTotal;
                                        }
                                        if (ITType.Trim() == "1")
                                        {
                                            GrossSalary = Convert.ToDouble(CrossSalaryIncome) + Math.Round(MainAmount);
                                            CrossSalaryIncome = GrossSalary;
                                            //FirstDeductAmt += Math.Round(MainAmount);
                                        }
                                        else if (ITType.Trim() == "2")
                                        {
                                            GrossSalary = Convert.ToDouble(CrossSalaryIncome) - Math.Round(MainAmount);
                                            CrossSalaryIncome = GrossSalary;
                                            //SecondDeductAmt += Math.Round(MainAmount);
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                        double FromRange = 0;
                        double ToRange = 0;
                        double BindAmount = 0;
                        double TotalTaxableAmount = 0;
                        #region RangeCalculation
                        if (ds1.Tables.Count > 3)
                        {
                            for (int intd = 0; intd < ds1.Tables[4].Rows.Count; intd++)
                            {
                                string Bindvalue = "From " + Convert.ToString(ds1.Tables[4].Rows[intd]["FromRange"]) + " To " + Convert.ToString(ds1.Tables[4].Rows[intd]["ToRange"]);
                                double.TryParse(Convert.ToString(ds1.Tables[4].Rows[intd]["FromRange"]), out FromRange);
                                double.TryParse(Convert.ToString(ds1.Tables[4].Rows[intd]["ToRange"]), out ToRange);
                                string Mode = Convert.ToString(ds1.Tables[4].Rows[intd]["mode"]);
                                string CalCAmount = Convert.ToString(ds1.Tables[4].Rows[intd]["Amount"]);
                                if (FromRange < GrossSalary && ToRange < GrossSalary)
                                {
                                    BindAmount = ToRange - FromRange;
                                    BindAmount += 1;
                                }
                                else if (FromRange < GrossSalary && ToRange > GrossSalary)
                                {
                                    BindAmount = GrossSalary - FromRange;
                                    BindAmount += 1;
                                }
                                else
                                {
                                    BindAmount = 0;
                                }
                                double CalCValueAmount = 0;
                                if (Mode.Trim() == "0" || Mode.Trim() == "False")
                                {
                                    double.TryParse(CalCAmount, out  CalCValueAmount);
                                }
                                else if (Mode.Trim() == "1" || Mode.Trim() == "True")
                                {
                                    CalCValueAmount = (BindAmount / 100) * Convert.ToDouble(CalCAmount);
                                }
                                TotalTaxableAmount += CalCValueAmount;
                            }
                        }
                        #endregion
                        double FinalTaxableincome = 0;
                        FinalTaxableincome += TotalTaxableAmount;
                        double TotalSalaryAmount = GrossSalary;
                        i++;
                        Fpspread2.Sheets[0].RowCount++;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        
                        column++;
                        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].CellType = txt;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Text = Convert.ToString(drRow["staff_code"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Tag = Convert.ToString(drRow["appl_no"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Note = Convert.ToString(drRow["appl_id"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                        column++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Text = Convert.ToString(drRow["staff_name"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Tag = Convert.ToString(drRow["dept_code"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                        column++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Text = Convert.ToString(drRow["desig_name"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Tag = Convert.ToString(drRow["desig_code"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                        column++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Text = Convert.ToString(drRow["pangirnumber"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                        column++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Text = Convert.ToString(ActualBasicAmount);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                        column++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Text = Convert.ToString(Math.Round(TotalHRA));
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                        column++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Text = Convert.ToString(Math.Round(HouseRent));
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";

                        for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                        {
                            ds1.Tables[2].DefaultView.RowFilter = "parentCode='" + Convert.ToString(ds1.Tables[0].Rows[k]["ITGroupPK"]) + "'";
                            dv = ds1.Tables[2].DefaultView;
                            if (dv.Count > 0)
                            {
                                for (int intn = 0; intn < dv.Count; intn++)
                                {
                                    ds1.Tables[1].DefaultView.RowFilter = "ITGroupPK='" + Convert.ToString(dv[intn]["ITGroupPK"]) + "'";
                                    dvnew = ds1.Tables[1].DefaultView;
                                    if (dvnew.Count > 0)
                                    {
                                        for (int intCh = 0; intCh < dvnew.Count; intCh++)
                                        {
                                            double AllowAndDeductTotal = 0;
                                            double DirectAllowDeductValue = 0;
                                            double Getvalue = 0;
                                            double AdditionalDeduction = 0;
                                            ITType = Convert.ToString(dvnew[intCh]["ITType"]);
                                            //int it = (int)(dvnew[intCh]["ITType"]);
                                            ITCommon = Convert.ToString(dvnew[intCh]["ITCommon"]);
                                            ITCommonValue = Convert.ToString(dvnew[intCh]["ITCommonValue"]);
                                            agechecked = Convert.ToString(dvnew[intCh]["IsAgeRange"]);
                                            maxAgeValue = Convert.ToString(dvnew[intCh]["MaxValue"]);//delsi0803
                                            minAgeValue = Convert.ToString(dvnew[intCh]["MinValue"]);//delsi0803
                                            if (ITCommon.Trim() == "1" || ITCommon.Trim() == "True")
                                            {
                                                if (ITType.Trim() == "1")
                                                {
                                                    if (ITCommonValue.Trim() != "")
                                                    {

                                                        if (!string.IsNullOrEmpty(ITCommonValue))
                                                        {
                                                            string[] AllowanceName = ITCommonValue.Split(',');
                                                            foreach (var item in AllowanceName)
                                                            {
                                                                if (!string.IsNullOrEmpty(item))
                                                                {
                                                                    if (AllowanceHash.ContainsKey(item.Trim()))
                                                                    {
                                                                        double.TryParse(Convert.ToString(AllowanceHash[item.Trim()]), out AdditionalDeduction);
                                                                        AllowAndDeductTotal += AdditionalDeduction;
                                                                    }
                                                                }
                                                            }
                                                        }

                                                    }
                                                }
                                                else if (ITType.Trim() == "2")
                                                {
                                                    if (ITCommonValue.Trim() != "")
                                                    {

                                                        if (!string.IsNullOrEmpty(ITCommonValue))
                                                        {
                                                            string[] DeductionName = ITCommonValue.Split(',');
                                                            foreach (var item in DeductionName)
                                                            {
                                                                if (!string.IsNullOrEmpty(item))
                                                                {
                                                                    if (DeductionHash.ContainsKey(item.Trim()))
                                                                    {
                                                                        double.TryParse(Convert.ToString(DeductionHash[item.Trim()]), out AdditionalDeduction);
                                                                        AllowAndDeductTotal += AdditionalDeduction;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        //Getvalue = Convert.ToString(DeductionHash[ITCommonValue.Trim()]);
                                                    }
                                                }
                                                //double.TryParse(Getvalue, out AllowAndDeductTotal);
                                            }
                                            ds1.Tables[3].DefaultView.RowFilter = "AllowdeductID='" + Convert.ToString(dvnew[intCh]["IT_IDFK"]) + "'";//delsi0803
                                            dAllview = ds1.Tables[3].DefaultView;
                                            if (dAllview.Count > 0)
                                            {
                                                string DirectValue = Convert.ToString(dAllview[0]["TotalAmount"]);
                                                double.TryParse(DirectValue, out DirectAllowDeductValue);
                                                if (Convert.ToString(dvnew[intCh]["ITAllowDeductName"]).ToUpper() == "LIC")//jayaram
                                                    LicAmt = DirectAllowDeductValue;
                                                else if (Convert.ToString(dvnew[intCh]["ITAllowDeductName"]).ToUpper() == "LIFE INSURANCE")
                                                    LicAmt = DirectAllowDeductValue;
                                                else if (Convert.ToString(dvnew[intCh]["ITAllowDeductName"]).ToUpper() == "LIFE INSURANCE(LIC)")
                                                    LicAmt = DirectAllowDeductValue;
                                            }
                                            else
                                            {
                                                if (Convert.ToString(dvnew[intCh]["ITAllowDeductName"]).Trim().Contains('/'))
                                                {
                                                    string[] DeductionName = Convert.ToString(dvnew[intCh]["ITAllowDeductName"]).Trim().Split('/');
                                                    double DedutionAmt = 0;
                                                    foreach (string deduct in DeductionName)
                                                    {
                                                        DedutionAmt = 0;
                                                        string deductShort = Convert.ToString(IncentiveMasterDeductionHash[deduct]);
                                                        if (!string.IsNullOrEmpty(deductShort))
                                                        {
                                                            if (DeductionHash.ContainsKey(deductShort))
                                                                double.TryParse(Convert.ToString(DeductionHash[deductShort]), out DedutionAmt);
                                                        }
                                                        DirectAllowDeductValue += DedutionAmt;
                                                    }

                                                }
                                            }

                                            if (age != "0")
                                            {
                                                if (agechecked.Trim() == "1" || agechecked.Trim() == "True")//delsi0803
                                                {
                                                    string[] maxArr = maxAgeValue.Split('-');
                                                    string[] minArr = minAgeValue.Split('-');
                                                    if (maxArr.Length == 2)
                                                    {
                                                        double.TryParse(Convert.ToString(maxArr[0]), out maxAge);
                                                        double.TryParse(Convert.ToString(maxArr[1]), out maxVal);
                                                    }
                                                    if (minArr.Length == 2)
                                                    {
                                                        double.TryParse(Convert.ToString(minArr[0]), out minAge);
                                                        double.TryParse(Convert.ToString(minArr[1]), out minVal);
                                                    }
                                                    //if (maxAge != 0 && maxAge <= staff_age)
                                                    //    if (maxVal != 0 && maxVal > DirectAllowDeductValue)
                                                    //        DirectAllowDeductValue = maxVal;
                                                    //if (minAge != 0 && minAge > staff_age)
                                                    //    if (minVal != 0 && DirectAllowDeductValue > minVal)
                                                    //        DirectAllowDeductValue = minVal;

                                                    if (maxAge != 0 && maxAge < staff_age)
                                                        if (maxVal != 0 && maxVal < DirectAllowDeductValue)//delsi09ref

                                                            DirectAllowDeductValue = maxVal;
                                                    if (minAge != 0 && minAge > staff_age)
                                                        if (minVal != 0 && DirectAllowDeductValue > minVal)
                                                            DirectAllowDeductValue = minVal;
                                                }
                                            }
                                            DirectAllowDeductValue += AllowAndDeductTotal;
                                            column++;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Text = Convert.ToString(Math.Round(DirectAllowDeductValue));
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Right;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";

                                        }
                                    }
                                }

                            }
                            else
                            {
                                ds1.Tables[1].DefaultView.RowFilter = "ITGroupPK='" + Convert.ToString(ds1.Tables[0].Rows[k]["ITGroupPK"]) + "'";
                                dvnew = ds1.Tables[1].DefaultView;
                                if (dvnew.Count > 0)
                                {
                                    for (int intCh = 0; intCh < dvnew.Count; intCh++)
                                    {
                                        double DirectAllowDeductValue = 0;
                                        double AllowAndDeductTotal = 0;
                                        string Getvalue = string.Empty;
                                        ITType = Convert.ToString(dvnew[intCh]["ITType"]);
                                        ITCommon = Convert.ToString(dvnew[intCh]["ITCommon"]);
                                        ITCommonValue = Convert.ToString(dvnew[intCh]["ITCommonValue"]);
                                        agechecked = Convert.ToString(dvnew[intCh]["IsAgeRange"]);
                                        maxAgeValue = Convert.ToString(dvnew[intCh]["MaxValue"]);//delsi0803
                                        minAgeValue = Convert.ToString(dvnew[intCh]["MinValue"]);//delsi0803

                                        if (ITCommon.Trim() == "1" || ITCommon.Trim() == "True")
                                        {
                                            if (ITType.Trim() == "1")
                                            {
                                                if (ITCommonValue.Trim() != "")
                                                {
                                                    Getvalue = Convert.ToString(AllowanceHash[ITCommonValue.Trim()]);
                                                }
                                            }
                                            else if (ITType.Trim() == "2")
                                            {
                                                if (ITCommonValue.Trim() != "")
                                                {
                                                    Getvalue = Convert.ToString(DeductionHash[ITCommonValue.Trim()]);
                                                }
                                            }
                                            double.TryParse(Getvalue, out AllowAndDeductTotal);
                                        }
                                        ds1.Tables[3].DefaultView.RowFilter = "AllowdeductID='" + Convert.ToString(dvnew[intCh]["IT_IDFK"]) + "'";//delsi0703
                                        dAllview = ds1.Tables[3].DefaultView;
                                        if (dAllview.Count > 0)
                                        {
                                            string percentage_val = Convert.ToString(dAllview[0]["Percentage"]);//delsi2209
                                            string DirectValue = Convert.ToString(dAllview[0]["TotalAmount"]);
                                            if (percentage_val != "" && percentage_val != null)
                                            {
                                                double calculatepercent = (Convert.ToDouble(DirectValue) / 100) * Convert.ToDouble(percentage_val);
                                                DirectValue = Convert.ToString(calculatepercent);

                                            }
                                            double.TryParse(DirectValue, out DirectAllowDeductValue);
                                        }
                                        DirectAllowDeductValue += AllowAndDeductTotal;
                                        if (age != "0")
                                        {
                                            if (agechecked.Trim() == "1" || agechecked.Trim() == "True")//delsi0803
                                            {
                                                string[] maxArr = maxAgeValue.Split('-');
                                                string[] minArr = minAgeValue.Split('-');
                                                if (maxArr.Length == 2)
                                                {
                                                    double.TryParse(Convert.ToString(maxArr[0]), out maxAge);
                                                    double.TryParse(Convert.ToString(maxArr[1]), out maxVal);
                                                }
                                                if (minArr.Length == 2)
                                                {
                                                    double.TryParse(Convert.ToString(minArr[0]), out minAge);
                                                    double.TryParse(Convert.ToString(minArr[1]), out minVal);
                                                }
                                                //if (maxAge != 0 && maxAge <= staff_age)
                                                //    if (maxVal != 0 && maxVal > DirectAllowDeductValue)
                                                //        DirectAllowDeductValue = maxVal;
                                                //if (minAge != 0 && minAge > staff_age)
                                                //    if (minVal != 0 && DirectAllowDeductValue > minVal)
                                                //        DirectAllowDeductValue = minVal;

                                                if (maxAge != 0 && maxAge < staff_age)
                                                    if (maxVal != 0 && maxVal < DirectAllowDeductValue)//delsi09ref

                                                        DirectAllowDeductValue = maxVal;
                                                if (minAge != 0 && minAge > staff_age)
                                                    if (minVal != 0 && DirectAllowDeductValue > minVal)
                                                        DirectAllowDeductValue = minVal;
                                            }
                                        }


                                        column++;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Text = Convert.ToString(Math.Round(DirectAllowDeductValue));
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Right;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";

                                    }
                                }

                            }
                        }

                        //column++;
                        //SecondDeductAmt = Convert.ToDouble(CrossSalaryIncome);
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Text = Convert.ToString(FirstDeductAmt);
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Right;
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                        //column++;
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Text = Convert.ToString(SecondDeductAmt);
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Right;
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                        column++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Text = Convert.ToString(GrossSalary);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";

                        //barath 11.10.17
                        double RebateAmount = 0;
                        double RebateDeductAmt = 0;
                        double RebateDeductAmount = 0;
                        string rebateAmt = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='RebateDeductAmount' and college_code='" + Convert.ToString(Session["collegecode"]) + "'");
                        string[] Rebate = rebateAmt.Split('-');
                        if (Rebate.Length == 2)
                        {
                            double.TryParse(Convert.ToString(Rebate[0]), out RebateDeductAmt);
                            double.TryParse(Convert.ToString(Rebate[1]), out RebateDeductAmount);
                        }
                        double FinaltaxVal = FinalTaxableincome;

                        column++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Text = Convert.ToString(Math.Round(FinalTaxableincome));
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                        if (TotalSalaryAmount < RebateDeductAmt)
                            RebateAmount = RebateDeductAmount;
                        FinalTaxableincome -= RebateAmount;//delsijustin
                        string geteducess = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Educess' and college_code='" + Convert.ToString(Session["collegecode"]) + "'");
                        int cessval = 0;
                        if (geteducess != "" || geteducess != "0")
                        {

                            cessval = Convert.ToInt32(geteducess);
                        }
                        else
                        {
                            cessval = 3;
                        }
                       
                      //  double TaxAmount = (FinalTaxableincome / 100) * 3;
                        double TaxAmount = (FinalTaxableincome / 100) * cessval;
                        FinalTaxableincome += TaxAmount;
                        column++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Text = Convert.ToString(Math.Round(RebateAmount));// TaxAmount-->cessamount
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";

                        double finalTaxAfterDeduct = FinaltaxVal - RebateAmount;

                        column++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Text = Convert.ToString(Math.Round(finalTaxAfterDeduct));// delsi2803
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";



                        column++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Text = Convert.ToString(Math.Round(TaxAmount));//RebateAmount-->rebateAmount
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";

                        double taxwithcess = finalTaxAfterDeduct + TaxAmount;

                        column++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Text = Convert.ToString(Math.Round(taxwithcess));//delsi2803
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";

                        #region TDS Tax Calculation
                        double TDSAmount = 0;
                        double CheckTds = 0;
                        if (ds1.Tables[5].Rows.Count > 0)
                        {
                            string TdsAmount = Convert.ToString(ds1.Tables[5].Compute("sum(TotalAmount)", ""));
                            double.TryParse(TdsAmount, out CheckTds);
                            TDSAmount += CheckTds;
                        }
                        if (ds1.Tables[6].Rows.Count > 0)
                        {
                            for (int intTds = 0; intTds < ds1.Tables[6].Rows.Count; intTds++)
                            {
                                string Iscommon = Convert.ToString(ds1.Tables[6].Rows[intTds]["ITCommon"]);
                                string iscommonvalue = Convert.ToString(ds1.Tables[6].Rows[intTds]["ITCommonValue"]);
                                if (Iscommon.Trim() == "1" || Iscommon.Trim() == "True")
                                {
                                    if (DeductionHash.ContainsKey(iscommonvalue.Trim()))
                                    {
                                        string GetCommonValue = Convert.ToString(DeductionHash[iscommonvalue.Trim()]);
                                        double.TryParse(GetCommonValue, out CheckTds);
                                        TDSAmount += CheckTds;
                                    }
                                }
                            }
                        }
                        double ProFxTax = TDSAmount;
                        column++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Text = Convert.ToString(Math.Round(ProFxTax));
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                       
                        FinalTaxableincome -= ProFxTax;
                        if (reinvestment != 0 && FinalTaxableincome < 0)//delsi2509
                        {
                            FinalTaxableincome = reinvestment + FinalTaxableincome;
                        }

                        column++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Text = Convert.ToString(Math.Round(reinvestment));
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";

                        column++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Text = Convert.ToString(Math.Round(FinalTaxableincome));
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                        if (cb_relived.Checked == true)//delsi 28/07
                        {

                            if (resign == "1" || resign == "True" && settle == "1" || settle == "True")//delsi
                            {
                                for (int clr = 0; clr <= column; clr++)
                                {
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, clr].BackColor = Color.MistyRose;
                                }


                            }
                        }
                        #endregion
                    }
                }
                Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                Fpspread2.Visible = true;
                rptprint.Visible = true;
            }
        }
        else
        {
            Fpspread2.Visible = false;
            rptprint.Visible = false;
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = "Please Select All Fields";
        }
        #endregion
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertmessage.Visible = false;
    }
    Hashtable PayProcessAllowanceDet(DataSet AllowanceDetDS, int tableNo, int tableRow, ref double TotalBasicAmount)
    {
        Hashtable AllowanceHash = new Hashtable();
        TotalBasicAmount = 0;
        for (int intds = tableRow; intds < AllowanceDetDS.Tables[tableNo].Rows.Count; intds++)
        {
            string AllowanceValue = Convert.ToString(AllowanceDetDS.Tables[tableNo].Rows[intds]["allowances"]);
            string[] SplitFirst = AllowanceValue.Split('\\');
            if (SplitFirst.Length > 0)
            {
                for (int intc = 0; intc < SplitFirst.Length; intc++)
                {
                    if (!string.IsNullOrEmpty(SplitFirst[intc].Trim()))
                    {
                        string[] SecondSplit = SplitFirst[intc].Split(';');
                        if (SecondSplit.Length > 0)
                        {
                            double AllowTaeknValue = 0;
                            string Allow = string.Empty;
                            string takenValue = SecondSplit[2].ToString();
                            if (takenValue.Trim().Contains('-'))
                            {
                                Allow = takenValue.Split('-')[1];
                                if (Allow.Trim() != "")
                                {
                                    double.TryParse(Allow, out AllowTaeknValue);
                                }
                            }
                            else
                            {
                                Allow = SecondSplit[3].ToString();
                                if (Allow.Trim() != "")
                                {
                                    double.TryParse(Allow, out AllowTaeknValue);
                                }
                            }
                            if (!AllowanceHash.ContainsKey(SecondSplit[0].Trim()))
                            {
                                AllowanceHash.Add(SecondSplit[0].Trim(), AllowTaeknValue);
                            }
                            else
                            {
                                double GetValue = Convert.ToDouble(AllowanceHash[SecondSplit[0].Trim()]);
                                GetValue = GetValue + AllowTaeknValue;
                                AllowanceHash.Remove(SecondSplit[0].Trim());
                                AllowanceHash.Add(SecondSplit[0].Trim(), GetValue);
                            }
                        }
                    }
                }
            }
            TotalBasicAmount += Convert.ToDouble(AllowanceDetDS.Tables[tableNo].Rows[intds]["bsalary"]);
        }
        return AllowanceHash;
    }
    /// <summary>
    /// Return monthlypay deduction value in Hashtable
    /// </summary>
    /// <param name="AllowanceDetDS"></param>
    /// <param name="tableNo"></param>
    /// <param name="tableRow"></param>
    /// <param name="TotalBasicAmount"></param>
    /// <returns></returns>
    Hashtable PayProcessDeductionDet(DataSet DeductionDetDS, int tableNo, int tableRow, ref double TotalBasicAmount)
    {
        Hashtable DeductionHash = new Hashtable();
        TotalBasicAmount = 0;
        for (int intds = tableRow; intds < DeductionDetDS.Tables[tableNo].Rows.Count; intds++)
        {
            string deductionValue = Convert.ToString(DeductionDetDS.Tables[tableNo].Rows[intds]["deductions"]);
            string[] SplitFirst = deductionValue.Split('\\');
            if (SplitFirst.Length > 0)
            {
                for (int intc = 0; intc < SplitFirst.Length; intc++)
                {
                    if (!string.IsNullOrEmpty(SplitFirst[intc].Trim()))
                    {
                        string[] SecondSplit = SplitFirst[intc].Split(';');
                        if (SecondSplit.Length > 0)
                        {
                            double AllowTaeknValue = 0;
                            string Allow = string.Empty;
                            string takenValue = SecondSplit[2].ToString();
                            if (takenValue.Trim().Contains('-'))
                            {
                                Allow = takenValue.Split('-')[1];
                                if (Allow.Trim() != "")
                                {
                                    double.TryParse(Allow, out AllowTaeknValue);
                                }
                            }
                            else
                            {
                                Allow = SecondSplit[3].ToString();
                                if (Allow.Trim() != "")
                                {
                                    double.TryParse(Allow, out AllowTaeknValue);
                                }
                            }
                            if (!DeductionHash.ContainsKey(SecondSplit[0].Trim()))
                            {
                                DeductionHash.Add(SecondSplit[0].Trim(), AllowTaeknValue);
                            }
                            else
                            {
                                double GetValue = Convert.ToDouble(DeductionHash[SecondSplit[0].Trim()]);
                                GetValue = GetValue + AllowTaeknValue;
                                DeductionHash.Remove(SecondSplit[0].Trim());
                                DeductionHash.Add(SecondSplit[0].Trim(), GetValue);
                            }
                        }
                    }
                }
            }
            TotalBasicAmount += Convert.ToDouble(DeductionDetDS.Tables[tableNo].Rows[intds]["bsalary"]);
        }
        return DeductionHash;
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread2, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = txtexcelname.Text;
            string pagename = "hrsalaryincomepf.aspx";
            Printcontrol.loadspreaddetails(Fpspread2, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        { }
    }
    protected void cb_stafftyp_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckboxChange(cb_stafftyp, cbl_stafftyp, txt_stafftyp, "--Select--", "StaffType");
    }
    protected void cbl_stafftyp_selectedchanged(object sender, EventArgs e)
    {
        rs.CallCheckboxListChange(cb_stafftyp, cbl_stafftyp, txt_stafftyp, "--Select--", "StaffType");
    }
    protected void loadstafftype()
    {
        try
        {
            ds.Clear();
            cbl_stafftyp.Items.Clear();
            string item = "select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code = '" + Convert.ToString(Session["collegecode"]) + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_stafftyp.DataSource = ds;
                cbl_stafftyp.DataTextField = "stftype";
                cbl_stafftyp.DataBind();
                if (cbl_stafftyp.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_stafftyp.Items.Count; i++)
                    {
                        cbl_stafftyp.Items[i].Selected = true;
                    }
                    txt_stafftyp.Text = "StaffType (" + cbl_stafftyp.Items.Count + ")";
                    cb_stafftyp.Checked = true;
                }
            }
            else
            {
                txt_stafftyp.Text = "--Select--";
                cb_stafftyp.Checked = false;
            }
        }
        catch { }
    }
    protected void cbrelived_checkchange(object sender, EventArgs e)//delsi 2807
    {
        staff();

    }
}
