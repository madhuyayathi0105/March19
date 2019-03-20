using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class StaffAttendance : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    Hashtable ht = new Hashtable();
    string[] dtfrom;
    string[] dttodate;
    int noa = 0;
    int nop = 0;
    int noper = 0;
    int nol = 0;
    int enoa = 0;
    int enop = 0;
    int enoper = 0;
    int enol = 0;
    int noo = 0;
    int enoo = 0;
    static string collegecode = "";
    string singleUser = string.Empty;
    string group_user = string.Empty;
    string strdept = "";

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
            //}        //****************************************************//
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
            else
            {
                collegecode = Session["collegecode"].ToString();
                singleUser = Session["single_user"].ToString();
                group_user = Session["group_code"].ToString();
            }
            if (!IsPostBack)
            {
                FpStaffAttendance.CommandBar.Visible = false;
                txtfromdate.Text = (DateTime.Now).ToString("dd/MM/yyyy");
                txttodate.Text = (DateTime.Now).ToString("dd/MM/yyyy");
                FpStaffAttendance.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnprintmaster.Visible = false;
                btnxl.Visible = false;

                //=====Added by saranya on 7/9/2018=====//
                string St_Code = Convert.ToString(Session["Staff_Code"]).Trim();
                if (string.IsNullOrEmpty(St_Code) || St_Code == "0")
                {
                    Lbldept.Visible = true;
                    tdDepartment.Visible = true;
                    LblStcodeName.Visible = true;
                    ddlSt_codeandName.Visible = true;
                    load_dept();
                    load_staffCodename();
                }
                else
                {
                    Lbldept.Visible = false;
                    tdDepartment.Visible = false;
                    LblStcodeName.Visible = false;
                    ddlSt_codeandName.Visible = false;
                }
                //========================================//

                //dtfrom = txtfromdate.Text.Split('/');
                //dttodate = txttodate.Text.Split('/');
                //DateTime strstartdate = Convert.ToDateTime(dtfrom[1] + '/' + dtfrom[0] + '/' + dtfrom[2]);
                //DateTime strenddate = Convert.ToDateTime(dttodate[1] + '/' + dttodate[0] + '/' + dttodate[2]);
            }
            msg.Visible = false;
            msg1.Visible = false;
        }
        catch (Exception ex) { }
    }

    #region Added by Saranya on 7/9/2018 for Department and Staff filter

    void load_dept()
    {
        cbldepttype.Visible = true;
        cbldepttype.Items.Clear();
        ds.Clear();
        string deptquery = "";
        string singleuser = Session["single_user"].ToString();
        if (singleuser == "True")
        {
            deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + collegecode + "') order by dept_name";
        }
        else
        {
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + collegecode + "') order by dept_name";
        }
        if (deptquery != "")
        {
            ds = da.select_method_wo_parameter(deptquery, "Text");
            cbldepttype.DataSource = ds;
            cbldepttype.DataTextField = "dept_name";
            cbldepttype.DataValueField = "dept_code";
            cbldepttype.DataBind();
        }
    }

    protected void chkselect_CheckedChanged(object sender, EventArgs e)
    {
        if (chkselect.Checked == true)
        {
            for (int i = 0; i < cbldepttype.Items.Count; i++)
            {
                cbldepttype.Items[i].Selected = true;
                txtDept.Text = "Department(" + (cbldepttype.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cbldepttype.Items.Count; i++)
            {
                cbldepttype.Items[i].Selected = false;
                txtDept.Text = "---Select---";
            }
        }
        load_staffCodename();
    }

    protected void cbldepttype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int itemcount = 0;
        int SelDept = 0;
        for (itemcount = 0; itemcount < cbldepttype.Items.Count; itemcount++)
        {
            if (cbldepttype.Items[itemcount].Selected == true)
            {
                SelDept++;
            }
        }
        txtDept.Text = "Department(" + (SelDept) + ")";       
        load_staffCodename();
    }

    void load_staffCodename()
    {
        ddlSt_codeandName.Items.Clear();
        ds.Clear();

        string sqlstaffname = "Select distinct staffmaster.Staff_code+' - '+staff_name as staffCodeName from staffmaster,hrdept_master,stafftrans where staffmaster.college_code='" + collegecode + "' ";
        sqlstaffname = sqlstaffname + " and stafftrans.staff_code=staffmaster.staff_code and resign=0 and settled=0 and stafftrans.dept_code=hrdept_master.dept_code";

        strdept = "";
        //if (txtDept.Text != "---Select---")
        //{

        int itemcount = 0;
        for (itemcount = 0; itemcount < cbldepttype.Items.Count; itemcount++)
        {
            if (cbldepttype.Items[itemcount].Selected == true)
            {
                if (strdept == "")
                    strdept = "'" + cbldepttype.Items[itemcount].Value.ToString() + "'";
                else
                    strdept = strdept + "," + "'" + cbldepttype.Items[itemcount].Value.ToString() + "'";
            }
        }
        if (strdept != "")
        {
            strdept = " in (" + strdept + ")";
            sqlstaffname = sqlstaffname + " and hrdept_master.dept_code " + strdept + "";
        }
       
        //}
        ds = da.select_method_wo_parameter(sqlstaffname, "text");
        ddlSt_codeandName.DataSource = ds;
        ddlSt_codeandName.DataTextField = "staffCodeName";
        ddlSt_codeandName.DataValueField = "staffCodeName";
        ddlSt_codeandName.DataBind();
    }

    #endregion

    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            msg.Visible = false;
            FpStaffAttendance.Visible = true;
            FpStaffAttendance.Sheets[0].AutoPostBack = true;
            FpStaffAttendance.Sheets[0].RowCount = 0;
            FpStaffAttendance.Sheets[0].ColumnCount = 0;
            FpStaffAttendance.Sheets[0].ColumnCount = 6;
            FpStaffAttendance.Sheets[0].ColumnHeader.RowCount = 1;
            FpStaffAttendance.RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
            style1.Font.Size = 12;
            style1.Font.Bold = true;
            style1.HorizontalAlign = HorizontalAlign.Center;
            style1.ForeColor = Color.Black;
            style1.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FpStaffAttendance.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            FpStaffAttendance.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            FpStaffAttendance.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            FpStaffAttendance.Sheets[0].AllowTableCorner = true;
            //  FpSpread1.Sheets[0].SheetCorner.Columns[0].Width =100;

            //---------------page number

            //FplibraryInOut.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
            //FplibraryInOut.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
            //FplibraryInOut.Pager.Align = HorizontalAlign.Right;
            //FplibraryInOut.Pager.Font.Bold = true;
            //FplibraryInOut.Pager.Font.Name = "Book Antiqua";
            //FplibraryInOut.Pager.ForeColor = Color.DarkGreen;
            //FplibraryInOut.Pager.BackColor = Color.Beige;
            //FplibraryInOut.Pager.BackColor = Color.AliceBlue;

            FpStaffAttendance.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpStaffAttendance.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
            FpStaffAttendance.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Time In";
            FpStaffAttendance.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Time Out";
            FpStaffAttendance.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Morning";
            FpStaffAttendance.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Evening";

            FpStaffAttendance.Sheets[0].Columns[0].Width = 10;
            FpStaffAttendance.Sheets[0].Columns[1].Width = 80;
            FpStaffAttendance.Sheets[0].Columns[2].Width = 70;
            FpStaffAttendance.Sheets[0].Columns[3].Width = 70;
            FpStaffAttendance.Sheets[0].Columns[4].Width = 10;
            FpStaffAttendance.Sheets[0].Columns[5].Width = 10;

            FpStaffAttendance.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpStaffAttendance.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            FpStaffAttendance.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            FpStaffAttendance.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            FpStaffAttendance.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Right;
            FpStaffAttendance.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Right;

            dtfrom = txtfromdate.Text.Split('/');
            dttodate = txttodate.Text.Split('/');
            DateTime strstartdate = Convert.ToDateTime(dtfrom[1] + '/' + dtfrom[0] + '/' + dtfrom[2]);
            DateTime strenddate = Convert.ToDateTime(dttodate[1] + '/' + dttodate[0] + '/' + dttodate[2]);
            string sql = "";
            string selQry = "";
            if (strenddate <= DateTime.Now && strstartdate <= strenddate)
            {
                while (strstartdate <= strenddate)
                {
                    string staffCode = Convert.ToString(Session["Staff_Code"]).Trim();
                    sql = "select * from bio_attendance where access_date='" + strstartdate + "'";
                    //Modified by Saranya on 7/9/2018
                    if (ddlSt_codeandName.Items.Count > 0)
                    {
                        if (ddlSt_codeandName.SelectedItem.Value.ToString() != "")
                        {
                            string CodeAndName = ddlSt_codeandName.SelectedItem.Value;
                            string[] St_CodeName = CodeAndName.Split('-');

                            sql = sql + " and roll_no='" + St_CodeName[0].ToString() + "'";
                        }
                    }
                    else
                    {
                        sql = sql + " and roll_no='" + staffCode + "'";
                    }
                    //===================================//
                    ds = da.select_method(sql, ht, "Text");

                    FpStaffAttendance.Sheets[0].RowCount++;
                    FpStaffAttendance.Sheets[0].RowCount = FpStaffAttendance.Sheets[0].RowCount++;
                    FpStaffAttendance.Sheets[0].Cells[FpStaffAttendance.Sheets[0].RowCount - 1, 0].Text = FpStaffAttendance.Sheets[0].RowCount.ToString();
                    FpStaffAttendance.Sheets[0].Cells[FpStaffAttendance.Sheets[0].RowCount - 1, 1].Text = String.Format("{0:dd-MM-yyyy}", strstartdate);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string intime = ds.Tables[0].Rows[0]["Time_in"].ToString();
                        string outtime = ds.Tables[0].Rows[0]["Time_Out"].ToString();
                        //Added by Saranyadevi 9.6.2018
                        if (intime != "")
                        {
                            DateTime in_time = Convert.ToDateTime(ds.Tables[0].Rows[0]["Time_in"]);
                            if ("12:00 AM" == in_time.ToString("hh:mm tt"))
                                FpStaffAttendance.Sheets[0].Cells[FpStaffAttendance.Sheets[0].RowCount - 1, 2].Text = "";
                            else
                                FpStaffAttendance.Sheets[0].Cells[FpStaffAttendance.Sheets[0].RowCount - 1, 2].Text = in_time.ToString("hh:mm tt");
                        }
                        else
                        {
                            FpStaffAttendance.Sheets[0].Cells[FpStaffAttendance.Sheets[0].RowCount - 1, 2].Text = "-";
                        }
                        if (outtime != "")
                        {
                            DateTime out_time = Convert.ToDateTime(ds.Tables[0].Rows[0]["Time_Out"]);
                            if ("12:00 AM" == out_time.ToString("hh:mm tt"))
                                FpStaffAttendance.Sheets[0].Cells[FpStaffAttendance.Sheets[0].RowCount - 1, 3].Text = "";
                            else
                                FpStaffAttendance.Sheets[0].Cells[FpStaffAttendance.Sheets[0].RowCount - 1, 3].Text = out_time.ToString("hh:mm tt");
                        }
                        else
                        {
                            FpStaffAttendance.Sheets[0].Cells[FpStaffAttendance.Sheets[0].RowCount - 1, 3].Text = "-";
                        }                      
                    }
                    else
                    {
                        FpStaffAttendance.Sheets[0].Cells[FpStaffAttendance.Sheets[0].RowCount - 1, 2].Text = "-";
                        FpStaffAttendance.Sheets[0].Cells[FpStaffAttendance.Sheets[0].RowCount - 1, 3].Text = "-";                       
                    }
                    
                    string sd = strstartdate.ToString();
                    string[] sda = sd.Split(' ');
                    string dt1 = sda[0].ToString();
                    string[] dta1 = dt1.Split('/');
                    string dat = dta1[1].ToString();
                    string monthyear = dta1[0].ToString().TrimStart('0') + "/" + dta1[2].ToString();

                    selQry = "select [" + dat + "] as dat from staff_attnd where mon_year='" + monthyear + "'";

                    //Modified by Saranya on 7/9/2018
                    if (ddlSt_codeandName.Items.Count > 0)
                    {
                        if (ddlSt_codeandName.SelectedItem.Value.ToString() != "")
                        {
                            string CodeAndName = ddlSt_codeandName.SelectedItem.Value;
                            string[] St_CodeName = CodeAndName.Split('-');

                            selQry = selQry + " and staff_code='" + St_CodeName[0].ToString() + "'";
                        }
                    }
                    else
                    {
                        selQry = selQry + " and staff_code='" + staffCode + "'";
                    }
                    //===================================//

                    ds1 = da.select_method(selQry, ht, "Text");
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        string atn = ds1.Tables[0].Rows[0]["dat"].ToString();
                        if (atn != "")
                        {
                            string[] atns = atn.Split('-');
                            FpStaffAttendance.Sheets[0].Cells[FpStaffAttendance.Sheets[0].RowCount - 1, 4].Text = atns[0].ToString();
                            FpStaffAttendance.Sheets[0].Cells[FpStaffAttendance.Sheets[0].RowCount - 1, 5].Text = atns[1].ToString();
                            if (atns[0].ToString() == "A")
                            {
                                noa = noa + 1;
                            }
                            else if (atns[0].ToString() == "P")
                            {
                                nop = nop + 1;
                            }
                            else if (atns[0].ToString() == "PER")
                            {
                                noper = noper + 1;
                            }
                            else if (atns[0].ToString() == "LA")
                            {
                                nol = nol + 1;
                            }
                            else if (atns[0].ToString() != "H")//delsi 02.05.2018
                            {
                                noo = noo + 1;
                            }
                            if (atns[1].ToString() == "A")
                            {
                                enoa = enoa + 1;
                            }
                            else if (atns[1].ToString() == "P")
                            {
                                enop = enop + 1;
                            }
                            else if (atns[1].ToString() == "PER")
                            {
                                enoper = enoper + 1;
                            }
                            else if (atns[1].ToString() == "LA")
                            {
                                enol = enol + 1;
                            }
                            else if (atns[0].ToString() != "H")//delsi 02.05.2018
                            {
                                enoo = enoo + 1;
                            }
                        }
                        else
                        {
                            FpStaffAttendance.Sheets[0].Cells[FpStaffAttendance.Sheets[0].RowCount - 1, 4].Text = "-";
                            FpStaffAttendance.Sheets[0].Cells[FpStaffAttendance.Sheets[0].RowCount - 1, 5].Text = "-";
                        }
                    }
                    else
                    {
                        FpStaffAttendance.Sheets[0].Cells[FpStaffAttendance.Sheets[0].RowCount - 1, 4].Text = "-";
                        FpStaffAttendance.Sheets[0].Cells[FpStaffAttendance.Sheets[0].RowCount - 1, 5].Text = "-";
                    }
                    strstartdate = strstartdate.AddDays(1);
                    
                }
                if (FpStaffAttendance.Sheets[0].Rows.Count > 0)
                {
                    msg.Visible = false;
                    msg1.Visible = false;
                    FpStaffAttendance.Sheets[0].Visible = true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnprintmaster.Visible = true;
                    btnxl.Visible = true;
                    panel_Total.Visible = true;
                    int p = nop + enop;
                    double ph = (double)p / 2;
                    int a = noa + enoa;
                    double ah = (double)a / 2;
                    int l = nol + enol;
                    int per = noper + enoper;
                    int o = noo + enoo;
                    lbltp.Text = "Total No of Present :" + ph;
                    lblta.Text = "Total No Of Absent :" + ah;
                    lbltl.Text = "Total No Of Late      :" + l;
                    lbltper.Text = "Total No Of Permissions :" + per;
                    lblto.Text = "Total No Of Leave :" + o;
                }
                else
                {
                    panel_Total.Visible = false;
                    msg.Visible = true;
                    msg1.Visible = true;
                    msg1.Text = "No Records Found";
                    FpStaffAttendance.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnprintmaster.Visible = false;
                    btnxl.Visible = false;
                }
            }
            else if ((strstartdate > DateTime.Now && strenddate > DateTime.Now) || (strstartdate > DateTime.Now || strenddate > DateTime.Now))
            {
                msg.Visible = true;
                panel_Total.Visible = false;
                msg.Text = "You cannot View the report for Upcoming Days";
                FpStaffAttendance.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnprintmaster.Visible = false;
                btnxl.Visible = false;
            }
            else
            {
                msg.Visible = true;
                panel_Total.Visible = false;
                msg1.Visible = false;
                msg.Text = "To date Cannot be less than from date";
                FpStaffAttendance.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnprintmaster.Visible = false;
                btnxl.Visible = false;
            }
            FpStaffAttendance.Sheets[0].PageSize = FpStaffAttendance.Sheets[0].Rows.Count;
        }
        catch (Exception ex)
        {            
            da.sendErrorMail(ex, collegecode, "StaffAttendance");
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string date = "";
        date = "@" + "Fromdate : " + txtfromdate.Text.ToString() + " Todate : " + txttodate.Text.ToString() + "";
        string degreedetails = "Staff Attendance Report" + date;
        Printcontrol.loadspreaddetails(FpStaffAttendance, "StaffAttendance.aspx", degreedetails);
        Printcontrol.Visible = true;
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                da.printexcelreport(FpStaffAttendance, reportname);
            }
            else
            {
                msg.Text = "Please Enter Your Report Name";
                msg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            msg.Text = ex.ToString();
        }
    }
}