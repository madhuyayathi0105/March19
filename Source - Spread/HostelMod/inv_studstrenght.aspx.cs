using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;
public partial class inv_studstrenght : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    ReuasableMethods rs = new ReuasableMethods();
    bool check = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        //****************************************************//
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("hostelindex"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/HostelMod/hostelindex.aspx");
                return;
            }
        }
        //****************************************************//
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        lblvalidation1.Text = "";
        lblerror.Visible = false;
        if (!IsPostBack)
        {
            cb_menutype.Checked = true;
            for (int i = 0; i < cbl_menutype.Items.Count; i++)
            {
                cbl_menutype.Items[i].Selected = true;
            }
            cbl_menutype_SelectIndexChange(sender, e);
            rb1.Checked = true;
            bindhostelname();
            loadsession();
            txtfrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtto.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtfrom.Attributes.Add("readonly", "readonly");
            txtto.Attributes.Add("readonly", "readonly");
            txt_prevDate.Text = DateTime.Now.Date.AddDays(-1).ToString("dd/MM/yyyy");
            txt_prevDate.Attributes.Add("readonly", "readonly");
            btngo_click(sender, e);
            bindPurposeCatagory();
        }
        if (cb_prevdate.Checked == true)
        {
            txt_prevDate.Attributes.Add("style", "top: 10px; left: 888px;; position: absolute;display:block;");
        }
        else { txt_prevDate.Attributes.Add("style", "top: 10px; left: 888px; position: absolute;display:none;"); }
    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    public void bindhostelname()
    {
        try
        {
            ddlhostelname.Items.Clear();
            ds.Clear();
            //ds = d2.Bindmess_inv(collegecode1);
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlhostelname.DataSource = ds;
                ddlhostelname.DataTextField = "MessName";
                ddlhostelname.DataValueField = "MessMasterPK";
                ddlhostelname.DataBind();
            }
        }
        catch
        {
        }
    }
    public void loadsession()
    {
        try
        {
            ds.Clear();
            chklstsession.Items.Clear();
            string itemheader = Convert.ToString(ddlhostelname.SelectedItem.Value);
            if (itemheader.Trim() != "")
            {
                string selecthostel = "select distinct SessionMasterPK,SessionName, MessMasterFK  FROM HM_SessionMaster where MessMasterFK in('" + itemheader + "') order by SessionName";
                ds = d2.select_method_wo_parameter(selecthostel, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklstsession.DataSource = ds;
                    chklstsession.DataTextField = "SessionName";
                    chklstsession.DataValueField = "SessionMasterPK";
                    chklstsession.DataBind();
                    if (chklstsession.Items.Count > 0)
                    {
                        for (int i = 0; i < chklstsession.Items.Count; i++)
                        {
                            chklstsession.Items[i].Selected = true;
                        }
                        txtsessionname.Text = "Session Name(" + chklstsession.Items.Count + ")";
                    }
                }
                else
                {
                    txtsessionname.Text = "--Select--";
                }
            }
            else
            {
                txtsessionname.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void ddlhostelname_Change(object sender, EventArgs e)
    {
        loadsession();
    }
    protected void chksession_checkedchange(object sender, EventArgs e)
    {
        if (chksessionname.Checked == true)
        {
            for (int i = 0; i < chklstsession.Items.Count; i++)
            {
                chklstsession.Items[i].Selected = true;
            }
            txtsessionname.Text = "Session Name(" + (chklstsession.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chklstsession.Items.Count; i++)
            {
                chklstsession.Items[i].Selected = false;
            }
            txtsessionname.Text = "--Select--";
        }
    }
    protected void chklstsession_Change(object sender, EventArgs e)
    {
        txtsessionname.Text = "--Select--";
        chksessionname.Checked = false;
        int commcount = 0;
        for (int i = 0; i < chklstsession.Items.Count; i++)
        {
            if (chklstsession.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtsessionname.Text = "Session Name(" + commcount.ToString() + ")";
            if (commcount == chklstsession.Items.Count)
            {
                chksessionname.Checked = true;
            }
        }
    }
    protected void radiobtn1(object sender, EventArgs e)
    {
        if (rb1.Checked == true)
        {
            rb2.Checked = false;
            txtfrom.Enabled = true;
            txtto.Enabled = true;
        }
    }
    protected void radiobtn2(object sender, EventArgs e)
    {
        if (rb2.Checked == true)
        {
            rb1.Checked = false;
            txtfrom.Enabled = false;
            txtto.Enabled = false;
        }
    }
    protected void txtto_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblerror.Visible = false;
            string fromdate = txtfrom.Text;
            string todate = txtto.Text;
            if (fromdate != "" && fromdate != null && todate != "" && todate != null)
            {
                string[] spiltfrom = fromdate.Split('/');
                string[] spitto = todate.Split('/');
                DateTime from = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
                DateTime to = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);
                if (from > to)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Enter To Date Grater Than From Date";
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }
    protected void txtfrom_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblerror.Visible = false;
            string fromdate = txtfrom.Text;
            string todate = txtto.Text;
            if (fromdate != "" && fromdate != null && todate != "" && todate != null)
            {
                string[] spiltfrom = fromdate.Split('/');
                string[] spitto = todate.Split('/');
                DateTime from = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
                DateTime to = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);
                if (from > to)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Enter To Date Greater Than From Date";
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }
    protected void ddlvendor1(object sender, EventArgs e)
    {
        //div1.Visible = false;
        rptprint.Visible = false;
        Fpspread1.Visible = false;
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
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
            string degreedetails = "Student Strength Status Report";
            string pagename = "studstrenght.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void cb_menutype_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_menutype.Checked == true)
            {
                for (int i = 0; i < cbl_menutype.Items.Count; i++)
                {
                    cbl_menutype.Items[i].Selected = true;
                }
                txt_menutype.Text = "Menu Type(" + (cbl_menutype.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_menutype.Items.Count; i++)
                {
                    cbl_menutype.Items[i].Selected = false;
                }
                txt_menutype.Text = "--Select--";
            }
        }
        catch (Exception ex)
        { }
    }
    protected void cbl_menutype_SelectIndexChange(object sender, EventArgs e)
    {
        try
        {
            txt_menutype.Text = "--Select--";
            cb_menutype.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_menutype.Items.Count; i++)
            {
                if (cbl_menutype.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_menutype.Text = "Menu Type(" + commcount.ToString() + ")";
                if (commcount == cbl_menutype.Items.Count)
                {
                    cb_menutype.Checked = true;
                }
            }
        }
        catch (Exception ex)
        { }
    }
    protected void btngo_click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            ArrayList list = new ArrayList();
            //list.Add("Monday");
            //list.Add("Tuesday");
            //list.Add("Wednesday");
            //list.Add("Thursday");
            //list.Add("Friday");
            //list.Add("Saturday");
            //list.Add("Sunday");

            string txtPrevDate = Convert.ToString(txt_prevDate.Text);
            DateTime prevdateDt = new DateTime();
            string[] prevdate = txtPrevDate.Split('/');
            prevdateDt = Convert.ToDateTime(prevdate[1] + "/" + prevdate[0] + "/" + prevdate[2]);
            string prevdate_day = prevdate[0].ToString();
            prevdate_day = prevdate_day.TrimStart('0');
            Boolean PrevDateCheck = false;
            if (cb_prevdate.Checked)
            {
                list.Add(Convert.ToString(prevdateDt.Date.DayOfWeek));
                PrevDateCheck = true;
            }
            list.Add(Convert.ToString(System.DateTime.Now.DayOfWeek));

            ArrayList newarray = new ArrayList();
            string messname = Convert.ToString(ddlhostelname.SelectedItem.Value);
            string MessmasterFK = Convert.ToString(ddlhostelname.SelectedItem.Value);
            string hostelcode = d2.Gethostelcode_inv(messname);
            //string sessionname = rs.GetSelectedItemsValueAsString(chklstsession);
            string SessionFK = rs.GetSelectedItemsValue(chklstsession);
            if (cblTotaltype.Items.Count > 0)
            {
                for (int r = 0; r < cblTotaltype.Items.Count; r++)
                {
                    if (cblTotaltype.Items[r].Text != "T")
                    {
                        newarray.Add(Convert.ToString(cblTotaltype.Items[r].Text));
                    }
                    cblTotaltype.Items[r].Selected = false;
                }
            }
            string firstdate = Convert.ToString(txtfrom.Text);
            string seconddate = Convert.ToString(txtto.Text);
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            DataView dv = new DataView();
            ArrayList tot = new ArrayList();
            ArrayList hour = new ArrayList();
            string Splitmondate = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
            string[] split_date = Splitmondate.Split(new Char[] { '/' });
            DateTime dcheckdate = Convert.ToDateTime(split_date[1] + "/" + split_date[0] + "/" + split_date[2]);
            string str_dayCurent = split_date[0].ToString();
            str_dayCurent = str_dayCurent.TrimStart('0');
            string str_day = str_dayCurent.TrimStart('0');
            string Atmonth = split_date[1].ToString();
            string Atyear = split_date[2].ToString();
            int strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
            string mon_year = Convert.ToString(Atmonth) + "/" + Convert.ToString(Atyear);
            string studentmesstype = "";
            string MenuType = rs.GetSelectedItemsValue(cbl_menutype);
            string purposeCatagory = rs.GetSelectedItemsValueAsString(cbl_purposecatagory);
            int purposeCatagoryCount = GetSelectedItemCount(cbl_purposecatagory);
            if (MenuType.Trim() != "")
                studentmesstype = MenuType;
            if (MenuType.Trim() == "")
            {
                MenuType = "2";
            }
            if (hostelcode.Trim() != "" && SessionFK.Trim() != "" && dt <= dt1)
            {
                /*string selectquery = " select distinct SessionMasterPK ,SessionName  from HM_SessionMaster where MessMasterFK in('" + MessmasterFK + "') and SessionMasterPK in ('" + SessionFK + "') order by SessionMasterPK ";
                selectquery += " select m.MenuSchedulePK,m.MenuScheduleDate,m.SessionMasterFK,mm.MenuName,mm.MenuMasterPK,m.Hostler,m.DayScholor, m.Staffcount, m.Guestcount, m.Change_strength from HT_MenuSchedule m,HM_SessionMaster s,HM_MenuMaster mm ,HM_HostelMaster hm where m.SessionMasterFK =s.SessionMasterPK and m.MessMasterFK in('" + MessmasterFK + "') and ScheudleItemType=1 and mm.MenuMasterPK=m.MenuMasterFK and hm.MessMasterFK=m.MessMasterFK  and mm.MenuType in('" + MenuType + "') and ScheduleType='1'";
                selectquery += "  select m.MenuSchedulePK,m.MenuScheduleday,m.SessionMasterFK,mm.MenuName, mm.MenuMasterPK, m.Hostler, m.DayScholor,m.Staffcount,m.Guestcount,m.Change_strength,m.MessMasterFK from HT_MenuSchedule m,HM_SessionMaster s,HM_MenuMaster mm ,HM_HostelMaster hm where m.SessionMasterFK =s.SessionMasterPK and m.MessMasterFK in('" + MessmasterFK + "') and ScheudleItemType=1 and mm.MenuMasterPK=m.MenuMasterFK and hm.MessMasterFK=m.MessMasterFK and ScheduleType='2'  and mm.MenuType in('" + MenuType + "')";
                selectquery += " select * from HT_HostelRegistration h,Registration r,HM_HostelMaster hm where  ISNULL(IsSuspend,'')=0 and ISNULL(IsVacated,'') =0 and r.App_No =h.APP_No and h.MemType=1 and isnull(h.IsDiscontinued,0)=0 and hm.HostelMasterPK =h.HostelMasterFK and StudMessType in('" + studentmesstype + "')";//r.college_code='" + collegecode1 + "' and
                selectquery += " select s.staff_code , Hostel_Code,Session_code from staffmaster s,DayScholourStaffAdd ds where s.staff_code =ds.Staff_code and resign =0 and settled =0 and ds.Hostel_Code in ('" + MessmasterFK + "') and Session_code in ('" + SessionFK + "')";
                selectquery += "  select  r.Roll_Admit,r.roll_no ,Hostel_Code,Session_code,r.App_No from DayScholourStaffAdd d,Registration r  where d.Roll_No  =r.Roll_No and CC=0 and DelFlag =0 and Exam_Flag<>'DEBAR' and d.Hostel_Code in('" + MessmasterFK + "') and Session_code in ('" + SessionFK + "') ";
                selectquery += " select  EditMenuTotal,Use_Attendance,Att_Hour,Staff_total,Hostel_code,Session_code,Schedule_date,daily_consumption,ISNULL(AllStudentAttendance,0)AllStudentAttendance  from HostelIns_settings where Hostel_code in('" + MessmasterFK + "') and Session_code in('" + SessionFK + "') and Schedule_type ='0' and Schedule_date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' ";
                selectquery += " select  EditMenuTotal,Use_Attendance,Att_Hour,Staff_total,Hostel_code, Session_code,Schedule_Day,daily_consumption,ISNULL(AllStudentAttendance,0)AllStudentAttendance from HostelIns_settings where Hostel_code in('" + MessmasterFK + "') and Session_code in('" + SessionFK + "') and Schedule_type ='1'";
                selectquery += " select MenuscheduleFK,HostelVegCount,HostelNonvegCount, DayscholorVegCount, DayscholorNonvegCount,StaffVegCount, StaffNonvegCount, GuestVegCount,GuestNonvegCount from HT_HostelStudMenuStrength ";
                selectquery += " select MenuscheduleFK,PurposeCode,VegCount,NonVegCount from HT_HostelMenupurposeStrength";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");*/
                //StrengthStatusReport 
                //@MessmasterFk nvarchar(150),@SessionMasterFK nvarchar(max),@MenuType varchar(100),@ScheduleType varchar,@MessType varchar(100),@ScheduleFromDate varchar(100),@ScheduleToDate varchar(100)
                Hashtable strenghtParamHash = new Hashtable();
                strenghtParamHash.Add("MessmasterFk", MessmasterFK);
                strenghtParamHash.Add("SessionMasterFK", SessionFK);
                strenghtParamHash.Add("MenuType", MenuType);
                //strenghtParamHash.Add("ScheduleType", "");
                strenghtParamHash.Add("MessType", studentmesstype);
                strenghtParamHash.Add("ScheduleFromDate", dt.ToString("MM/dd/yyyy"));
                strenghtParamHash.Add("ScheduleToDate", dt1.ToString("MM/dd/yyyy"));
                ds = d2.select_method("StrengthStatusReport", strenghtParamHash, "sp");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fpspread1.Sheets[0].RowCount = 0;
                    Fpspread1.Sheets[0].ColumnCount = 0;
                    Fpspread1.CommandBar.Visible = false;
                    Fpspread1.Sheets[0].AutoPostBack = false;
                    Fpspread1.Sheets[0].ColumnHeader.RowCount = 3;
                    Fpspread1.Sheets[0].RowHeader.Visible = false;
                    Fpspread1.Sheets[0].ColumnCount = 2;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[0].Width = 50;
                    Fpspread1.Columns[0].Locked = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Session / Day";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
                    Fpspread1.Columns[1].Width = 150;
                    Fpspread1.Columns[1].Locked = true;
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        #region Header Bind
                        Fpspread1.Sheets[0].ColumnCount += 1 + newarray.Count;
                        int columncount = 1 + newarray.Count;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - columncount].Text = Convert.ToString(ds.Tables[0].Rows[row]["SessionName"]);
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - columncount].Tag = Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]);
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - columncount].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - columncount].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - columncount].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - columncount].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - columncount].Text = "Menu Name";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - columncount].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - columncount].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - columncount].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - columncount].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Columns[Fpspread1.Sheets[0].ColumnCount - columncount].Width = 150;
                        Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - columncount].Locked = true;
                        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, Fpspread1.Sheets[0].ColumnCount - columncount, 2, 1);
                        int c = 0; int headerspancount = Fpspread1.Sheets[0].ColumnCount - (columncount - 1);
                        for (int s = 0; s < cblTotaltype.Items.Count; s++)
                        {
                            if (cblTotaltype.Items[s].Text != "T")
                            {
                                c++;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Text = Convert.ToString(cblTotaltype.Items[s].Text);
                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Font.Bold = true;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Locked = true;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Text = "Veg";
                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Tag = "0";
                                Fpspread1.Columns[Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Width = 60;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Font.Bold = true;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Locked = true;
                                Fpspread1.Sheets[0].ColumnCount++;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Text = "NonVeg";
                                Fpspread1.Columns[Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Width = 60;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Tag = "1";
                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Font.Bold = true;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Locked = true;
                                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, headerspancount, 1, 2);
                                headerspancount += 2;
                            }
                        }
                        if (!string.IsNullOrEmpty(purposeCatagory))
                        {
                            for (int r = 0; r < cbl_purposecatagory.Items.Count; r++)
                            {
                                if (cbl_purposecatagory.Items[r].Selected == true)
                                {
                                    c++;
                                    Fpspread1.Sheets[0].ColumnCount++;
                                    columncount++;
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Text = Convert.ToString(cbl_purposecatagory.Items[r].Text);
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Tag = Convert.ToString(cbl_purposecatagory.Items[r].Value);
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Font.Bold = true;
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Locked = true;
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Text = "Veg";
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Tag = "0";
                                    Fpspread1.Columns[Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Width = 60;
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Font.Bold = true;
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Locked = true;
                                    Fpspread1.Sheets[0].ColumnCount++;
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Text = "NonVeg";
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Tag = Convert.ToString(cbl_purposecatagory.Items[r].Value);
                                    Fpspread1.Columns[Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Width = 60;
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Tag = "1";
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Font.Bold = true;
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - (columncount - (c))].Locked = true;
                                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, headerspancount, 1, 2);
                                    headerspancount += 2;
                                }
                            }
                        }
                        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, Fpspread1.Sheets[0].ColumnCount - columncount - 4 - purposeCatagoryCount, 1, columncount + 4 + purposeCatagoryCount);
                        //Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, Fpspread1.Sheets[0].ColumnCount - columncount - 4 , 1, columncount + 4 );12.09.17
                        //Fpspread1.Sheets[0].ColumnCount++;
                        //Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - (columncount - (newarray.Count))].Text = "Total Strength";
                        //Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - (columncount - (newarray.Count))].Font.Bold = true;
                        //Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - (columncount - (newarray.Count))].Font.Name = "Book Antiqua";
                        //Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - (columncount - (newarray.Count))].Font.Size = FontUnit.Medium;
                        //Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - (columncount - (newarray.Count))].HorizontalAlign = HorizontalAlign.Center;
                        //Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - (columncount - (1 + newarray.Count))].Locked = true;
                        //Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - (columncount - (1 + newarray.Count))].Visible = false;
                        //FarPoint.Web.Spread.DoubleCellType db = new FarPoint.Web.Spread.DoubleCellType();
                        //db.ErrorMessage = "Enter only Numbers";
                        #endregion
                    }
                    ArrayList adddays = new ArrayList();
                    DataView dcheck = new DataView();
                    DataView dnview = new DataView();
                    ArrayList Presentcount = new ArrayList();
                    ArrayList hostelpresentcount = new ArrayList();
                    ArrayList Presentstaffcount = new ArrayList();
                    ArrayList dummyarray = new ArrayList();
                    ArrayList dummyarraydtwise = new ArrayList();
                    Dictionary<string, string> HostelVegorNvPersentCountDic = new Dictionary<string, string>();
                    int ro = 0;
                    DateTime CalculateDate = new DateTime();
                    if (rb2.Checked == true)//daywise
                    {
                        #region daywise
                        FarPoint.Web.Spread.DoubleCellType db1 = new FarPoint.Web.Spread.DoubleCellType();
                        db1.ErrorMessage = "Enter Only Number";
                        if (list.Count > 0)
                        {
                            for (int jk = 0; jk < list.Count; jk++)
                            {
                                ro++;
                                dummyarray.Clear();
                                Fpspread1.Sheets[0].RowCount++;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ro);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                string ColDate = (PrevDateCheck == true) ? txtPrevDate : Splitmondate;

                                string[] ColDateA = ColDate.Split(new Char[] { '/' });
                                CalculateDate = Convert.ToDateTime(ColDateA[1] + "/" + ColDateA[0] + "/" + ColDateA[2]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ColDate + "-" + list[jk]);
                                if (PrevDateCheck)
                                {
                                    str_day = prevdate_day;
                                    string[] Prev_date = ColDate.Split(new Char[] { '/' });
                                    DateTime PrevDate = Convert.ToDateTime(Prev_date[1] + "/" + Prev_date[0] + "/" + Prev_date[2]);
                                    string PrevDay = Prev_date[0].ToString();
                                    PrevDay = PrevDay.TrimStart('0');
                                    string Prevmonth = Prev_date[1].ToString().TrimStart('0');
                                    string Prevyear = Prev_date[2].ToString();
                                    strdate = (Convert.ToInt32(Prevmonth) + Convert.ToInt32(Prevyear) * 12);
                                    mon_year = Convert.ToString(Prevmonth) + "/" + Convert.ToString(Prevyear);
                                }
                                else
                                {
                                    string[] Current_date = ColDate.Split(new Char[] { '/' });
                                    DateTime CurrentDate = Convert.ToDateTime(Current_date[1] + "/" + Current_date[0] + "/" + Current_date[2]);
                                    string CurrentDay = Current_date[0].ToString();
                                    CurrentDay = CurrentDay.TrimStart('0');
                                    string Currentmonth = Current_date[1].ToString().TrimStart('0');
                                    string Currentyear = Current_date[2].ToString();
                                    strdate = (Convert.ToInt32(Currentmonth) + Convert.ToInt32(Currentyear) * 12);
                                    mon_year = Convert.ToString(Atmonth) + "/" + Convert.ToString(Atyear);
                                    str_day = str_dayCurent;
                                }
                                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(list[jk]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Row.Height = 150;
                                int col = 1;
                                int sum = 0;
                                int purposeCatagoryCnt = 0;//12.09.17
                                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                                {
                                    dummyarray.Clear();
                                    col += 2 + newarray.Count;
                                    ds.Tables[2].DefaultView.RowFilter = "MenuScheduleday ='" + Convert.ToString(list[jk]) + "' and SessionMasterFK ='" + Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]) + "'";
                                    dv = ds.Tables[2].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        sum = 0;
                                        ds.Tables[7].DefaultView.RowFilter = "Hostel_code in ('" + ddlhostelname.SelectedItem.Value + "') and Session_code ='" + Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]) + "' and Schedule_Day='" + Convert.ToString(list[jk]) + "'";
                                        dcheck = ds.Tables[7].DefaultView;
                                        Presentcount.Clear();
                                        hostelpresentcount.Clear();
                                        HostelVegorNvPersentCountDic.Clear();
                                        Presentstaffcount.Clear();
                                        double GuestVeg = 0;
                                        double GuestNonVeg = 0;
                                        tot.Clear();
                                        string Attendancevalue = string.Empty;
                                        if (dcheck.Count > 0)
                                        {
                                            #region Hostel Settings Check
                                            string Editmentotal = Convert.ToString(dcheck[0]["EditMenuTotal"]);
                                            string UseAttendance = Convert.ToString(dcheck[0]["Use_Attendance"]);
                                            string AttendanceHour = Convert.ToString(dcheck[0]["Att_Hour"]);
                                            string StaffAttendance = Convert.ToString(dcheck[0]["Staff_total"]);
                                            string AllstudentAttendance = Convert.ToString(dcheck[0]["AllStudentAttendance"]);
                                            bool AllstudentAttendanceBool = false;
                                            if (Convert.ToString(AllstudentAttendance).ToUpper() == "TRUE" || AllstudentAttendance == "1")
                                                AllstudentAttendanceBool = true;
                                            string daily_consump = Convert.ToString(dcheck[0]["daily_consumption"]);
                                            if (daily_consump.Trim() != "")
                                            {
                                                string[] splitArray = daily_consump.Split(',');
                                                if (splitArray.Length > 0)
                                                {
                                                    for (int row1 = 0; row1 <= splitArray.GetUpperBound(0); row1++)
                                                    {
                                                        dummyarray.Add(Convert.ToString(splitArray[row1]));
                                                    }
                                                }
                                            }
                                            if (Editmentotal.Trim() != "")
                                            {
                                                string[] Menu_Split = Editmentotal.Split(',');
                                                if (Menu_Split.Length > 0)
                                                {
                                                    for (int m = 0; m <= Menu_Split.GetUpperBound(0); m++)
                                                    {
                                                        tot.Add(Menu_Split[m]);
                                                        for (int sh = 0; sh < cblTotaltype.Items.Count; sh++)
                                                        {
                                                            if (Menu_Split[m] == cblTotaltype.Items[sh].Text)
                                                            {
                                                                cblTotaltype.Items[sh].Selected = true;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                            //06.10.17 jpr
                                            if (dummyarray.Contains("D"))
                                            {
                                                #region Dayscholor Attendence
                                                if (UseAttendance.Trim() == "True")
                                                {
                                                    string[] splithour = AttendanceHour.Split(',');
                                                    if (splithour.Length > 0)
                                                    {
                                                        Attendancevalue = "";
                                                        for (int a = 0; a <= splithour.GetUpperBound(0); a++)
                                                        {
                                                            if (Attendancevalue.Trim() == "")
                                                            {
                                                                if (splithour[a].Trim() != "")
                                                                {
                                                                    Attendancevalue = "d" + str_day + "d" + splithour[a];
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (splithour[a].Trim() != "")
                                                                {
                                                                    Attendancevalue += "," + "d" + str_day + "d" + splithour[a];
                                                                }
                                                            }
                                                        }
                                                        DataSet dsv = new DataSet();
                                                        if (Attendancevalue.Trim() != "")
                                                        {
                                                            //if (dcheckdate.ToString("dddd") == Convert.ToString(list[jk]))
                                                            //{
                                                            //ds.Tables[5].DefaultView.RowFilter = "Hostel_Code='" + ddlhostelname.SelectedItem.Value + "' and Session_code='" + Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]) + "'";
                                                            //dnview = ds.Tables[5].DefaultView;
                                                            dnview = ds.Tables[10].DefaultView;
                                                            if (dnview.Count > 0)
                                                            {
                                                                dnview = ds.Tables[10].DefaultView;
                                                                if (dnview.Count > 0)
                                                                {
                                                                    string RollNo = returnDSwithSigleColumnSingleCodeValue(dnview.ToTable(), "roll_no");

                                                                    //for (int rv = 0; rv < dnview.Count; rv++)
                                                                    //{
                                                                    string selectquery_vlaue = "select [" + Attendancevalue + "],roll_no from Attendance where month_year ='" + strdate + "' and roll_no in('" + RollNo + "' )  and [" + Attendancevalue + "] = '1'";//Convert.ToString(dnview[rv]["roll_no"]) like '1%'
                                                                    dsv.Clear();
                                                                    dsv = d2.select_method_wo_parameter(selectquery_vlaue, "Text");
                                                                    if (dsv.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        for (int dsrow = 0; dsrow < dsv.Tables[0].Rows.Count; dsrow++)
                                                                        {
                                                                            string Attendvlaue = Convert.ToString(dsv.Tables[0].Rows[dsrow][0]);
                                                                            if (Attendvlaue.Trim() == "1")
                                                                            {
                                                                                if (!Presentcount.Contains(Convert.ToString(dsv.Tables[0].Rows[dsrow]["roll_no"])))//Convert.ToString(dnview[rv]["roll_no"])))
                                                                                {
                                                                                    Presentcount.Add(Convert.ToString(dsv.Tables[0].Rows[dsrow]["roll_no"]));//Convert.ToString(dnview[rv]["roll_no"]));
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            //}
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    ds.Tables[5].DefaultView.RowFilter = "Hostel_Code='" + ddlhostelname.SelectedItem.Value + "' and Session_code='" + Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]) + "'";
                                                    dnview = ds.Tables[5].DefaultView;
                                                    if (dnview.Count > 0)
                                                    {
                                                        for (int rv = 0; rv < dnview.Count; rv++)
                                                        {
                                                            if (!Presentcount.Contains(Convert.ToString(dnview[rv]["roll_no"])))
                                                            {
                                                                Presentcount.Add(Convert.ToString(dnview[rv]["roll_no"]));
                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                ds.Tables[5].DefaultView.RowFilter = "Hostel_Code='" + ddlhostelname.SelectedItem.Value + "' and Session_code='" + Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]) + "'";
                                                dnview = ds.Tables[5].DefaultView;
                                                if (dnview.Count > 0)
                                                {
                                                    for (int rv = 0; rv < dnview.Count; rv++)
                                                    {
                                                        if (!Presentcount.Contains(Convert.ToString(dnview[rv]["roll_no"])))
                                                        {
                                                            Presentcount.Add(Convert.ToString(dnview[rv]["roll_no"]));
                                                        }
                                                    }
                                                }
                                            }
                                            if (dummyarray.Contains("H"))
                                            {
                                                if (AllstudentAttendanceBool)
                                                {
                                                    #region Dayscholor Attendence
                                                    if (UseAttendance.Trim() == "True")
                                                    {
                                                        string[] splithour = AttendanceHour.Split(',');
                                                        if (splithour.Length > 0)
                                                        {
                                                            Attendancevalue = string.Empty;
                                                            for (int a = 0; a <= splithour.GetUpperBound(0); a++)
                                                            {
                                                                if (Attendancevalue.Trim() == "")
                                                                {
                                                                    if (splithour[a].Trim() != "")
                                                                    {
                                                                        Attendancevalue = "d" + str_day + "d" + splithour[a];
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (splithour[a].Trim() != "")
                                                                    {
                                                                        Attendancevalue += "," + "d" + str_day + "d" + splithour[a];
                                                                    }
                                                                }
                                                            }
                                                            DataSet dsv = new DataSet();
                                                            if (Attendancevalue.Trim() != "")
                                                            {
                                                                //if (dcheckdate.ToString("dddd") == Convert.ToString(list[jk]))
                                                                //{
                                                                //ds.Tables[5].DefaultView.RowFilter = "Hostel_Code='" + ddlhostelname.SelectedItem.Value + "' and Session_code='" + Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]) + "'";
                                                                //dnview = ds.Tables[5].DefaultView;
                                                                dnview = ds.Tables[10].DefaultView;
                                                                if (dnview.Count > 0)
                                                                {
                                                                    string RollNo = returnDSwithSigleColumnSingleCodeValue(dnview.ToTable(), "roll_no");

                                                                    //for (int rv = 0; rv < dnview.Count; rv++)
                                                                    //{
                                                                    string selectquery_vlaue = "select [" + Attendancevalue + "],roll_no from Attendance where month_year ='" + strdate + "' and roll_no in('" + RollNo + "') and [" + Attendancevalue + "] = '1' ";// '" + Convert.ToString(dnview[rv]["roll_no"]) + "'";like '1%'
                                                                    dsv.Clear();
                                                                    dsv = d2.select_method_wo_parameter(selectquery_vlaue, "Text");
                                                                    if (dsv.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        for (int dsrow = 0; dsrow < dsv.Tables[0].Rows.Count; dsrow++) //09.10.17 jpr changes
                                                                        {
                                                                            string Attendvlaue = Convert.ToString(dsv.Tables[0].Rows[dsrow][0]);
                                                                            if (Attendvlaue.Trim() == "1")
                                                                            {
                                                                                if (!Presentcount.Contains(Convert.ToString(dsv.Tables[0].Rows[dsrow]["roll_no"])))// ; dnview[rv]["roll_no"])))
                                                                                {
                                                                                    Presentcount.Add(Convert.ToString(dsv.Tables[0].Rows[dsrow]["roll_no"]));//dnview[rv]["roll_no"]));
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    //}
                                                                }
                                                                //}
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ds.Tables[5].DefaultView.RowFilter = "Hostel_Code='" + ddlhostelname.SelectedItem.Value + "' and Session_code='" + Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]) + "'";
                                                        dnview = ds.Tables[5].DefaultView;
                                                        if (dnview.Count > 0)
                                                        {
                                                            for (int rv = 0; rv < dnview.Count; rv++)
                                                            {
                                                                if (!Presentcount.Contains(Convert.ToString(dnview[rv]["roll_no"])))
                                                                {
                                                                    Presentcount.Add(Convert.ToString(dnview[rv]["roll_no"]));
                                                                }
                                                            }
                                                        }
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region Hosteler attendance
                                                    Attendancevalue = string.Empty;
                                                    if (Attendancevalue.Trim() == "")
                                                        Attendancevalue = "D" + str_day;
                                                    else
                                                        Attendancevalue += "," + "D" + str_day;
                                                    if (Attendancevalue.Trim() != "")
                                                    {
                                                        //if (dcheckdate.ToString("dddd") == Convert.ToString(list[jk]))//30.08.17
                                                        //{
                                                        //for (int rv = 0; rv < ds.Tables[3].Rows.Count; rv++)
                                                        //{
                                                        //06.04.16
                                                        DataView hcount = new DataView();
                                                        DataSet hc = new DataSet();
                                                        ds.Tables[3].DefaultView.RowFilter = "MessMasterFK='" + ddlhostelname.SelectedItem.Value + "'";
                                                        hcount = ds.Tables[3].DefaultView;
                                                        for (int rv = 0; rv < hcount.Count; rv++)
                                                        {
                                                            //string rollno = Convert.ToString(ds.Tables[3].Rows[rv]["App_no"]);
                                                            string rollno = Convert.ToString(hcount[rv]["App_no"]);
                                                            string studentMessType = Convert.ToString(hcount[rv]["StudMessType"]);
                                                            string selectquery_vlaue = "select " + Attendancevalue + " from HT_Attendance where AttnMonth ='" + Convert.ToString(CalculateDate.ToString("MM")) + "' and AttnYear ='" + Convert.ToString(CalculateDate.ToString("yyyy")) + "' and App_no ='" + rollno + "'";
                                                            DataSet dn = new DataSet();
                                                            dn = d2.select_method_wo_parameter(selectquery_vlaue, "Text");
                                                            if (dn.Tables[0].Rows.Count > 0)
                                                            {
                                                                for (int dsrow = 0; dsrow < dn.Tables[0].Columns.Count; dsrow++)
                                                                {
                                                                    string Attendvlaue = Convert.ToString(dn.Tables[0].Rows[0][dsrow]);
                                                                    if (Attendvlaue.Trim() == "1")
                                                                    {
                                                                        if (!hostelpresentcount.Contains(rollno))
                                                                        {
                                                                            hostelpresentcount.Add(rollno);
                                                                            string MessType = string.Empty;
                                                                            int Hper = 0;
                                                                            if (studentMessType == "1")
                                                                                MessType = "nonveg";
                                                                            else
                                                                                MessType = "veg";
                                                                            if (!HostelVegorNvPersentCountDic.ContainsKey(MessType))
                                                                            {
                                                                                Hper += 1;
                                                                                HostelVegorNvPersentCountDic.Add(MessType, Convert.ToString(Hper));
                                                                            }
                                                                            else
                                                                            {
                                                                                int.TryParse(HostelVegorNvPersentCountDic[MessType], out Hper);
                                                                                Hper += 1;
                                                                                HostelVegorNvPersentCountDic.Remove(MessType);
                                                                                HostelVegorNvPersentCountDic.Add(MessType, Convert.ToString(Hper));
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        //}
                                                    }
                                                    //}
                                                    //else
                                                    //{
                                                    //    for (int rv = 0; rv < ds.Tables[3].Rows.Count; rv++)
                                                    //    {
                                                    //        if (!hostelpresentcount.Contains(Convert.ToString(ds.Tables[3].Rows[rv]["Roll_No"])))
                                                    //        {
                                                    //            hostelpresentcount.Add(Convert.ToString(ds.Tables[3].Rows[rv]["Roll_No"]));
                                                    //        }
                                                    //    }
                                                    //}
                                                    #endregion
                                                }
                                            }
                                            if (dummyarray.Contains("S"))
                                            {
                                                #region staff attendancecheck
                                                if (StaffAttendance.Trim() == "True")
                                                {
                                                    DataSet dsv = new DataSet();
                                                    ds.Tables[4].DefaultView.RowFilter = "Hostel_Code='" + ddlhostelname.SelectedItem.Value + "' and Session_code='" + Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]) + "'";
                                                    dnview = ds.Tables[4].DefaultView;
                                                    if (dnview.Count > 0)
                                                    {
                                                        string staff_code = returnDSwithSigleColumnSingleCodeValue(dnview.ToTable(), "staff_code");
                                                        //for (int rv = 0; rv < dnview.Count; rv++)
                                                        //{
                                                        string selectquery_vlaue = "select [" + str_day + "],staff_code from staff_attnd where mon_year ='" + mon_year + "' and staff_code in('" + staff_code + "') and ([" + str_day + "] like 'P-%' or [" + str_day + "] like 'LA-%' or [" + str_day + "] like 'PER-%' or [" + str_day + "] like 'OD-%' or [" + str_day + "] like 'OOD-%')";//[" + str_day + "] like 'P-%'";
                                                        dsv.Clear();
                                                        dsv = d2.select_method_wo_parameter(selectquery_vlaue, "Text");
                                                        if (dsv.Tables[0].Rows.Count > 0)
                                                        {
                                                            for (int dsrow = 0; dsrow < dsv.Tables[0].Rows.Count; dsrow++)
                                                            {
                                                                string Attendvlaue = Convert.ToString(dsv.Tables[0].Rows[dsrow][0]);
                                                                if (Attendvlaue.Trim() != "")
                                                                {
                                                                    string[] splitattendance = Attendvlaue.Split('-');
                                                                    if (splitattendance.Length > 0)
                                                                    {
                                                                        string Attendvalue = Convert.ToString(splitattendance[0]);
                                                                        if (Attendvalue.Trim().ToUpper() == "P")
                                                                        {
                                                                            if (!Presentstaffcount.Contains(Convert.ToString(dsv.Tables[0].Rows[dsrow]["staff_code"])))//dnview[rv]["staff_code"])))
                                                                            {
                                                                                Presentstaffcount.Add(Convert.ToString(dsv.Tables[0].Rows[dsrow]["staff_code"]));//Convert.ToString(dnview[rv]["staff_code"]));
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            //}
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    ds.Tables[4].DefaultView.RowFilter = "Hostel_Code='" + ddlhostelname.SelectedItem.Value + "' and Session_code='" + Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]) + "'";
                                                    dnview = ds.Tables[4].DefaultView;
                                                    if (dnview.Count > 0)
                                                    {
                                                        for (int rv = 0; rv < dnview.Count; rv++)
                                                        {
                                                            if (!Presentstaffcount.Contains(Convert.ToString(dnview[rv]["staff_code"])))
                                                            {
                                                                Presentstaffcount.Add(Convert.ToString(dnview[rv]["staff_code"]));
                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                ds.Tables[4].DefaultView.RowFilter = "Hostel_Code='" + ddlhostelname.SelectedItem.Value + "' and Session_code='" + Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]) + "'";
                                                dnview = ds.Tables[4].DefaultView;
                                                if (dnview.Count > 0)
                                                {
                                                    for (int rv = 0; rv < dnview.Count; rv++)
                                                    {
                                                        if (!Presentstaffcount.Contains(Convert.ToString(dnview[rv]["staff_code"])))
                                                        {
                                                            Presentstaffcount.Add(Convert.ToString(dnview[rv]["staff_code"]));
                                                        }
                                                    }
                                                }
                                            }
                                            if (dummyarray.Contains("G"))
                                            {
                                                Attendancevalue = string.Empty;
                                                if (Attendancevalue.Trim() == "")
                                                    Attendancevalue = "D" + str_day;
                                                DataSet GuestDS = new DataSet();
                                                GuestDS = d2.select_method_wo_parameter("select COUNT(ht.app_no)count,isnull(ht.studmesstype,0)studmesstype from HT_HostelRegistration ht,HM_HostelMaster hm,HT_Attendance a where ht.HostelMasterFK=hm.HostelMasterPK and MemType='3' and hm.MessMasterFK in('" + Convert.ToString(ddlhostelname.SelectedItem.Value) + "') and isnull(issuspend,0)=0 and isnull(isdiscontinued,0)=0 and isnull(isvacated,0)=0 and ht.app_no=a.app_no and [" + Attendancevalue + "]=1 and AttnMonth='" + Convert.ToString(CalculateDate.ToString("MM")) + "' group by ht.studmesstype", "text");
                                                if (GuestDS.Tables[0].Rows.Count > 0)
                                                {
                                                    double.TryParse(Convert.ToString(GuestDS.Tables[0].Compute("Sum(count)", "studmesstype=0")), out GuestVeg);
                                                    double.TryParse(Convert.ToString(GuestDS.Tables[0].Compute("Sum(count)", "studmesstype=1")), out GuestNonVeg);
                                                }
                                            }
                                        }
                                        if (dv.Count > 0)
                                        {
                                            #region Menu Name Binding
                                            string menuname = string.Empty;
                                            string menucode = string.Empty;
                                            string menutype = string.Empty;
                                            for (int k = 0; k < dv.Count; k++)
                                            {
                                                string mname = Convert.ToString(dv[k]["MenuName"]);
                                                string mcode = Convert.ToString(dv[k]["MenuMasterPK"]);
                                                string mtype = Convert.ToString(dv[k]["menutype"]);
                                                if (menuname.Contains(mname) == false)
                                                {
                                                    if (menuname == "")
                                                    {
                                                        menuname = mname;
                                                        menucode = mcode;
                                                        menutype = mtype;
                                                    }
                                                    else
                                                    {
                                                        menuname += "," + mname;
                                                        menucode += "," + mcode;
                                                        menutype += "," + mtype;
                                                    }
                                                }
                                            }
                                            bool VegMenu = false;
                                            bool NonVegMenu = false;
                                            if (menutype.Contains("0") == true)
                                                VegMenu = true;
                                            if (menutype.Contains("1") == true)
                                                NonVegMenu = true;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - 1)].Text = menuname; Convert.ToString(dv[0]["MenuName"]);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - 1)].Tag = menucode;// Convert.ToString(dv[0]["MenuMasterPK"]);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - 1)].HorizontalAlign = HorizontalAlign.Left;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - 1)].Font.Size = FontUnit.Medium;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - 1)].Font.Name = "Book Antiqua";
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - 1)].Locked = true;
                                            #endregion
                                            int ch = 0;
                                            purposeCatagoryCnt = purposeCatagoryCount;
                                            #region cblTotal Type
                                            for (int s = 0; s < cblTotaltype.Items.Count; s++)
                                            {
                                                if (cblTotaltype.Items[s].Text != "T")
                                                {
                                                    ch++;
                                                    string MenuschedulePK = Convert.ToString(dv[0]["MenuschedulePK"]);
                                                    ds.Tables[8].DefaultView.RowFilter = " MenuscheduleFK='" + MenuschedulePK + "' and strengthdate='" + CalculateDate.ToString("MM/dd/yyyy") + "'";
                                                    DataView VegNonvegDv = ds.Tables[8].DefaultView;
                                                    if (tot.Contains(cblTotaltype.Items[s].Text))
                                                    {
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Locked = false;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].BackColor = Color.Gainsboro;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].CellType = db1;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 2))].Locked = false;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 2))].BackColor = Color.Gainsboro;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 2))].CellType = db1;
                                                    }
                                                    else
                                                    {
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Locked = true;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 2))].Locked = true;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].CellType = db1;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 2))].CellType = db1;
                                                    }
                                                    if (cblTotaltype.Items[s].Text == "H")
                                                    {
                                                        //if (ds.Tables[3].Rows.Count > 0)
                                                        //{
                                                        double vegCountH = 0; double NonvegH = 0;
                                                        if (dummyarray.Contains("H"))//changes 28.02.17
                                                        {
                                                            if (Convert.ToString(HostelVegorNvPersentCountDic.Count) != "0")
                                                            {
                                                                //double.TryParse(Convert.ToString(HostelVegorNvPersentCountDic["veg"]), out vegCountH);
                                                                //double.TryParse(Convert.ToString(HostelVegorNvPersentCountDic["nonveg"]), out NonvegH);
                                                                if (HostelVegorNvPersentCountDic.ContainsKey("veg"))
                                                                    double.TryParse(Convert.ToString(HostelVegorNvPersentCountDic["veg"]), out vegCountH);
                                                                if (HostelVegorNvPersentCountDic.ContainsKey("nonveg"))
                                                                    double.TryParse(Convert.ToString(HostelVegorNvPersentCountDic["nonveg"]), out NonvegH);

                                                                double totalH = vegCountH + NonvegH;
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = (Convert.ToString(totalH) == "0") ? "" : Convert.ToString(totalH); //Convert.ToString(HostelVegorNvPersentCountDic["veg"]);
                                                                ch++;
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text =
                                                                    (NonVegMenu == true ? Convert.ToString(NonvegH) : "");
                                                            }
                                                            //barath 01.11.17
                                                            //else
                                                            //{
                                                            //    if (VegNonvegDv.Count > 0)
                                                            //    {
                                                            //        double.TryParse(Convert.ToString(VegNonvegDv[0]["HostelVegCount"]), out vegCountH);
                                                            //        double.TryParse(Convert.ToString(VegNonvegDv[0]["HostelNonvegCount"]), out NonvegH);
                                                            //        double totalH = vegCountH + NonvegH;
                                                            //        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = (Convert.ToString(totalH) == "0") ? "" : Convert.ToString(totalH); //(Convert.ToString(VegNonvegDv[0]["HostelVegCount"]) == "0") ? "" : Convert.ToString(VegNonvegDv[0]["HostelVegCount"]);
                                                            //        ch++;
                                                            //        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = (Convert.ToString(VegNonvegDv[0]["HostelNonvegCount"]) == "0") ? "" : Convert.ToString(VegNonvegDv[0]["HostelNonvegCount"]);
                                                            //    }
                                                            //    else
                                                            //    {
                                                            //        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                            //        ch++;
                                                            //        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                            //    }
                                                            //}
                                                        }
                                                        else
                                                        {
                                                            if (VegNonvegDv.Count > 0)
                                                            {
                                                                double.TryParse(Convert.ToString(VegNonvegDv[0]["HostelVegCount"]), out vegCountH);
                                                                double.TryParse(Convert.ToString(VegNonvegDv[0]["HostelNonvegCount"]), out NonvegH);
                                                                double totalH = vegCountH + NonvegH;
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = (Convert.ToString(totalH) == "0") ? "" : Convert.ToString(totalH); //Convert.ToString(VegNonvegDv[0]["HostelVegCount"]);
                                                                ch++;
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text =
                                                                     (NonVegMenu == true ? Convert.ToString(VegNonvegDv[0]["HostelNonvegCount"]) : "");
                                                            }
                                                            else
                                                            {
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                                ch++;
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                            }
                                                        }
                                                    }
                                                    if (cblTotaltype.Items[s].Text == "S")
                                                    {
                                                        if (dummyarray.Contains("S"))//changes 28.02.17
                                                        {
                                                            if (Presentstaffcount.Count > 0)
                                                            {
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(Presentstaffcount.Count); ch++;
                                                            }
                                                            else
                                                            {
                                                                if (VegNonvegDv.Count > 0)
                                                                {
                                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(VegNonvegDv[0]["StaffVegCount"]);
                                                                    ch++;
                                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text =
                                                                             (NonVegMenu == true ? Convert.ToString(VegNonvegDv[0]["StaffNonvegCount"]) : "");
                                                                }
                                                                else
                                                                {
                                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                                    ch++;
                                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (VegNonvegDv.Count > 0)
                                                            {
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(VegNonvegDv[0]["StaffVegCount"]);
                                                                ch++;
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text =
                                                                (NonVegMenu == true ? Convert.ToString(VegNonvegDv[0]["StaffNonvegCount"]) : "");
                                                            }
                                                            else ch++;
                                                        }
                                                    }
                                                    if (cblTotaltype.Items[s].Text == "D")
                                                    {
                                                        //if (ds.Tables[5].Rows.Count > 0)
                                                        //{
                                                        if (dummyarray.Contains("D"))//changes 28.02.17
                                                        {
                                                            if (Presentcount.Count > 0)
                                                            {
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(Presentcount.Count); ch++;
                                                            }
                                                            else
                                                            {
                                                                if (VegNonvegDv.Count > 0)
                                                                {
                                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(VegNonvegDv[0]["DayscholorVegCount"]);
                                                                    ch++;
                                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = (NonVegMenu == true ? Convert.ToString(VegNonvegDv[0]["DayscholorNonvegCount"]) : "");
                                                                }
                                                                else
                                                                {
                                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                                    ch++;
                                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (VegNonvegDv.Count > 0)
                                                            {
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(VegNonvegDv[0]["DayscholorVegCount"]);
                                                                ch++;
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = (NonVegMenu == true ? Convert.ToString(VegNonvegDv[0]["DayscholorNonvegCount"]) : "");
                                                            }
                                                            else
                                                            {
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                                ch++;
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                            }
                                                        }
                                                    }
                                                    if (cblTotaltype.Items[s].Text == "G")
                                                    {
                                                        if (dummyarray.Contains("G"))//changes 01.11.17
                                                        {
                                                            if (GuestVeg > 0 || GuestNonVeg > 0)
                                                            {
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(GuestVeg); ch++;
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(GuestNonVeg);
                                                            }
                                                            else
                                                            {
                                                                if (VegNonvegDv.Count > 0)
                                                                {
                                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(VegNonvegDv[0]["GuestVegCount"]);
                                                                    ch++;
                                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text =
                                                                        (NonVegMenu == true ? Convert.ToString(VegNonvegDv[0]["GuestNonvegCount"]) : "");
                                                                }
                                                                else
                                                                {
                                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                                    ch++;
                                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (VegNonvegDv.Count > 0)
                                                            {
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(VegNonvegDv[0]["GuestVegCount"]);
                                                                ch++;
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text =
                                                                      (NonVegMenu == true ? Convert.ToString(VegNonvegDv[0]["GuestNonvegCount"]) : ""); //Convert.ToString(VegNonvegDv[0]["GuestNonvegCount"]);
                                                            }
                                                            else
                                                            {
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                                ch++;
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                            }
                                                        }
                                                    }
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch))].HorizontalAlign = HorizontalAlign.Center;
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch))].Font.Size = FontUnit.Medium;
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch))].Font.Name = "Book Antiqua";
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].HorizontalAlign = HorizontalAlign.Center;
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Font.Size = FontUnit.Medium;
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Font.Name = "Book Antiqua";
                                                }
                                            }
                                            #endregion
                                            if (purposeCatagoryCount > 0)//12.09.17
                                            {
                                                #region Purpose Category
                                                for (int s = 0; s < cbl_purposecatagory.Items.Count; s++)
                                                {
                                                    if (cbl_purposecatagory.Items[s].Selected == true)
                                                    {
                                                        ch++;
                                                        string MenuschedulePK = Convert.ToString(dv[0]["MenuschedulePK"]);
                                                        ds.Tables[9].DefaultView.RowFilter = " MenuscheduleFK='" + MenuschedulePK + "'  and PurposeCode='" + Convert.ToString(cbl_purposecatagory.Items[s].Value) + "'";
                                                        DataView PurposeCategoryDV = ds.Tables[9].DefaultView;
                                                        if (PurposeCategoryDV.Count > 0)
                                                        {
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Locked = false;
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].BackColor = Color.Gainsboro;
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = (Convert.ToString(PurposeCategoryDV[0]["VegCount"]) == "0") ? "" : Convert.ToString(PurposeCategoryDV[0]["VegCount"]);
                                                            ch++;
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Locked = false;
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].BackColor = Color.Gainsboro;
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(PurposeCategoryDV[0]["NonVegCount"]);
                                                        }
                                                        else
                                                        {
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Locked = false;
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].BackColor = Color.Gainsboro;
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                            ch++;
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Locked = false;
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].BackColor = Color.Gainsboro;
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";

                                                        }
                                                    }
                                                }
                                                #endregion
                                                purposeCatagoryCnt *= 2;
                                                //ch *= 2;
                                            }
                                            col += 3 + purposeCatagoryCnt;
                                        }
                                    }
                                    else { col += 3 + purposeCatagoryCnt; }
                                }
                                PrevDateCheck = false;
                            }
                            Fpspread1.Visible = true;
                            rptprint.Visible = true;
                            //  div1.Visible = true;
                            errorlable.Visible = false;
                            Fpspread1.Sheets[0].FrozenColumnCount = 2;
                            Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                        }
                        #endregion
                    }
                    if (rb1.Checked == true)//date wise
                    {
                        #region date wise
                        firstdate = Convert.ToString(txtfrom.Text);
                        seconddate = Convert.ToString(txtto.Text);
                        split = firstdate.Split('/');
                        dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                        split = seconddate.Split('/');
                        dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                        while (dt <= dt1)
                        {
                            ro++;
                            Fpspread1.Sheets[0].RowCount++;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ro);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dt.ToString("dd/MM/yyyy")) + " - " + Convert.ToString(dt.ToString("dddd"));
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            int col = 1;
                            int sum = 0;
                            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                            {
                                col += 2 + newarray.Count;
                                Presentcount.Clear();
                                hostelpresentcount.Clear();
                                HostelVegorNvPersentCountDic.Clear();
                                Presentstaffcount.Clear();
                                ds.Tables[1].DefaultView.RowFilter = "MenuScheduleDate ='" + Convert.ToString(dt.ToString("MM/dd/yyyy")) + "' and SessionMasterFK ='" + Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]) + "'";
                                dv = ds.Tables[1].DefaultView;
                                FarPoint.Web.Spread.DoubleCellType db1 = new FarPoint.Web.Spread.DoubleCellType();
                                db1.ErrorMessage = "Enter Only Number";
                                Fpspread1.Columns[7].CellType = db1;
                                //Fpspread1.Columns[7].BackColor = Color.Gainsboro;
                                sum = 0;
                                ds.Tables[6].DefaultView.RowFilter = "Hostel_code in ('" + ddlhostelname.SelectedItem.Value + "') and Session_code ='" + Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]) + "' and Schedule_date='" + Convert.ToString(dt.ToString("MM/dd/yyyy")) + "'";
                                dcheck = ds.Tables[6].DefaultView;
                                tot.Clear();
                                if (dcheck.Count > 0)
                                {
                                    #region Hostel settings
                                    string Editmentotal = Convert.ToString(dcheck[0]["EditMenuTotal"]);
                                    string UseAttendance = Convert.ToString(dcheck[0]["Use_Attendance"]);
                                    string AttendanceHour = Convert.ToString(dcheck[0]["Att_Hour"]);
                                    string StaffAttendance = Convert.ToString(dcheck[0]["Staff_total"]);
                                    Splitmondate = Convert.ToString(dt.ToString("dd/MM/yyyy"));
                                    split_date = Splitmondate.Split(new Char[] { '/' });
                                    str_day = split_date[0].ToString();
                                    str_day = str_day.TrimStart('0');
                                    Atmonth = split_date[1].ToString();
                                    Atyear = split_date[2].ToString();
                                    strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                                    mon_year = Convert.ToString(Atmonth) + "/" + Convert.ToString(Atyear);
                                    string daily_consump = Convert.ToString(dcheck[0]["daily_consumption"]);
                                    dummyarraydtwise.Clear();
                                    if (daily_consump.Trim() != "")
                                    {
                                        string[] splitArray = daily_consump.Split(',');
                                        if (splitArray.Length > 0)
                                        {
                                            for (int row1 = 0; row1 <= splitArray.GetUpperBound(0); row1++)
                                            {
                                                dummyarraydtwise.Add(Convert.ToString(splitArray[row1]));
                                            }
                                        }
                                    }
                                    #endregion
                                    if (dummyarraydtwise.Contains("D"))
                                    {
                                        if (UseAttendance.Trim() == "True")
                                        {
                                            #region Dayscholor Attendance Check
                                            string[] splithour = AttendanceHour.Split(',');
                                            if (splithour.Length > 0)
                                            {
                                                string Attendancevalue = "";
                                                for (int s = 0; s <= splithour.GetUpperBound(0); s++)
                                                {
                                                    if (Attendancevalue.Trim() == "")
                                                    {
                                                        if (splithour[s].Trim() != "")
                                                        {
                                                            Attendancevalue = "d" + str_day + "d" + splithour[s];
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (splithour[s].Trim() != "")
                                                        {
                                                            Attendancevalue = Attendancevalue + "," + "d" + str_day + "d" + splithour[s];
                                                        }
                                                    }
                                                }
                                                DataSet dsv = new DataSet();
                                                if (Attendancevalue.Trim() != "")
                                                {
                                                    ds.Tables[5].DefaultView.RowFilter = "Hostel_Code='" + ddlhostelname.SelectedItem.Value + "' and Session_code='" + Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]) + "'";
                                                    dnview = ds.Tables[5].DefaultView;
                                                    if (dnview.Count > 0)
                                                    {
                                                        for (int rv = 0; rv < dnview.Count; rv++)
                                                        {
                                                            string selectquery_vlaue = "select " + Attendancevalue + " from Attendance where month_year ='" + strdate + "' and roll_no='" + Convert.ToString(dnview[rv]["roll_no"]) + "'";
                                                            dsv.Clear();
                                                            dsv = d2.select_method_wo_parameter(selectquery_vlaue, "Text");
                                                            if (dsv.Tables[0].Rows.Count > 0)
                                                            {
                                                                for (int dsrow = 0; dsrow < dsv.Tables[0].Columns.Count; dsrow++)
                                                                {
                                                                    string Attendvlaue = Convert.ToString(dsv.Tables[0].Rows[0][dsrow]);
                                                                    if (Attendvlaue.Trim() == "1")
                                                                    {
                                                                        if (!Presentcount.Contains(Convert.ToString(dnview[rv]["roll_no"])))
                                                                        {
                                                                            Presentcount.Add(Convert.ToString(dnview[rv]["roll_no"]));
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            ds.Tables[5].DefaultView.RowFilter = "Hostel_Code='" + ddlhostelname.SelectedItem.Value + "' and Session_code='" + Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]) + "'";
                                            dnview = ds.Tables[5].DefaultView;
                                            if (dnview.Count > 0)
                                            {
                                                for (int rv = 0; rv < dnview.Count; rv++)
                                                {
                                                    if (!Presentcount.Contains(Convert.ToString(dnview[rv]["roll_no"])))
                                                    {
                                                        Presentcount.Add(Convert.ToString(dnview[rv]["roll_no"]));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // 28.07.16
                                        ds.Tables[5].DefaultView.RowFilter = "Hostel_Code='" + ddlhostelname.SelectedItem.Value + "' and Session_code='" + Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]) + "'";
                                        dnview = ds.Tables[5].DefaultView;
                                        if (dnview.Count > 0)
                                        {
                                            for (int rv = 0; rv < dnview.Count; rv++)
                                            {
                                                if (!Presentcount.Contains(Convert.ToString(dnview[rv]["roll_no"])))
                                                {
                                                    Presentcount.Add(Convert.ToString(dnview[rv]["roll_no"]));
                                                }
                                            }
                                        }
                                    }
                                    if (dummyarraydtwise.Contains("S"))
                                    {
                                        #region staff attendancecheck
                                        if (StaffAttendance.Trim() == "True")
                                        {
                                            DataSet dsv = new DataSet();
                                            ds.Tables[4].DefaultView.RowFilter = "Hostel_Code='" + ddlhostelname.SelectedItem.Value + "' and Session_code='" + Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]) + "'";
                                            dnview = ds.Tables[4].DefaultView;
                                            if (dnview.Count > 0)
                                            {
                                                #region StaffAttence Check
                                                for (int rv = 0; rv < dnview.Count; rv++)
                                                {
                                                    string selectquery_vlaue = "select [" + str_day + "] from staff_attnd where mon_year ='" + mon_year + "' and staff_code='" + Convert.ToString(dnview[rv]["staff_code"]) + "'";
                                                    dsv.Clear();
                                                    dsv = d2.select_method_wo_parameter(selectquery_vlaue, "Text");
                                                    if (dsv.Tables[0].Rows.Count > 0)
                                                    {
                                                        for (int dsrow = 0; dsrow < dsv.Tables[0].Columns.Count; dsrow++)
                                                        {
                                                            string Attendvlaue = Convert.ToString(dsv.Tables[0].Rows[0][dsrow]);
                                                            if (Attendvlaue.Trim() != "")
                                                            {
                                                                string[] splitattendance = Attendvlaue.Split('-');
                                                                if (splitattendance.Length > 0)
                                                                {
                                                                    string Attendvalue = Convert.ToString(splitattendance[0]);
                                                                    if (Attendvlaue.Trim() == "P")
                                                                    {
                                                                        if (!Presentstaffcount.Contains(Convert.ToString(dnview[rv]["staff_code"])))
                                                                        {
                                                                            Presentstaffcount.Add(Convert.ToString(dnview[rv]["staff_code"]));
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        else
                                        {
                                            ds.Tables[4].DefaultView.RowFilter = "Hostel_Code='" + ddlhostelname.SelectedItem.Value + "' and Session_code='" + Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]) + "'";
                                            dnview = ds.Tables[4].DefaultView;
                                            if (dnview.Count > 0)
                                            {
                                                for (int rv = 0; rv < dnview.Count; rv++)
                                                {
                                                    if (!Presentstaffcount.Contains(Convert.ToString(dnview[rv]["staff_code"])))
                                                    {
                                                        Presentstaffcount.Add(Convert.ToString(dnview[rv]["staff_code"]));
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        ds.Tables[4].DefaultView.RowFilter = "Hostel_Code='" + ddlhostelname.SelectedItem.Value + "' and Session_code='" + Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]) + "'";
                                        dnview = ds.Tables[4].DefaultView;
                                        if (dnview.Count > 0)
                                        {
                                            for (int rv = 0; rv < dnview.Count; rv++)
                                            {
                                                if (!Presentstaffcount.Contains(Convert.ToString(dnview[rv]["staff_code"])))
                                                {
                                                    Presentstaffcount.Add(Convert.ToString(dnview[rv]["staff_code"]));
                                                }
                                            }
                                        }
                                    }
                                    if (dummyarraydtwise.Contains("H"))
                                    {
                                        #region Hosteler attendance
                                        //string[] splithour = AttendanceHour.Split(',');
                                        //if (splithour.Length > 0)
                                        //{
                                        string Attendancevalue = "";
                                        //for (int rs = 0; rs <= splithour.GetUpperBound(0); rs++)
                                        //{
                                        if (Attendancevalue.Trim() == "")
                                        {
                                            //if (splithour[rs].Trim() != "")
                                            //{
                                            Attendancevalue = "D" + str_day;
                                            //}
                                        }
                                        else
                                        {
                                            //if (splithour[rs].Trim() != "")
                                            //{
                                            Attendancevalue = Attendancevalue + "," + "D" + str_day;
                                            //}
                                        }
                                        //}
                                        if (Attendancevalue.Trim() != "")
                                        {
                                            //if (dcheckdate.ToString("dddd") == Convert.ToString(list[jk]))
                                            //{
                                            DataView hcount = new DataView();
                                            DataSet hc = new DataSet();
                                            ds.Tables[3].DefaultView.RowFilter = "MessMasterFK='" + ddlhostelname.SelectedItem.Value + "'";
                                            hcount = ds.Tables[3].DefaultView;
                                            for (int rv = 0; rv < hcount.Count; rv++)
                                            {
                                                //string rollno = Convert.ToString(ds.Tables[3].Rows[rv]["App_no"]);
                                                string rollno = Convert.ToString(hcount[rv]["App_no"]);
                                                string studentMessType = Convert.ToString(hcount[rv]["StudMessType"]);
                                                string selectquery_vlaue = "select " + Attendancevalue + " from HT_Attendance where AttnMonth ='" + Convert.ToString(split_date[1]) + "' and AttnYear ='" + Convert.ToString(split_date[2]) + "' and App_no ='" + rollno + "'";
                                                DataSet dn = new DataSet();
                                                dn = d2.select_method_wo_parameter(selectquery_vlaue, "Text");
                                                if (dn.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int dsrow = 0; dsrow < dn.Tables[0].Columns.Count; dsrow++)
                                                    {
                                                        string Attendvlaue = Convert.ToString(dn.Tables[0].Rows[0][dsrow]);
                                                        if (Attendvlaue.Trim() == "1")
                                                        {
                                                            if (!hostelpresentcount.Contains(rollno))
                                                            {
                                                                hostelpresentcount.Add(rollno);
                                                                string MessType = string.Empty;
                                                                int Hper = 0;
                                                                if (studentMessType == "1")
                                                                    MessType = "nonveg";
                                                                else
                                                                    MessType = "veg";
                                                                if (!HostelVegorNvPersentCountDic.ContainsKey(MessType))
                                                                {
                                                                    Hper += 1;
                                                                    HostelVegorNvPersentCountDic.Add(MessType, Convert.ToString(Hper));
                                                                }
                                                                else
                                                                {
                                                                    int.TryParse(HostelVegorNvPersentCountDic[MessType], out Hper);
                                                                    Hper += 1;
                                                                    HostelVegorNvPersentCountDic.Remove(MessType);
                                                                    HostelVegorNvPersentCountDic.Add(MessType, Convert.ToString(Hper));
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        //}
                                        #endregion
                                    }
                                    if (Editmentotal.Trim() != "")
                                    {
                                        string[] Menu_Split = Editmentotal.Split(',');
                                        if (Menu_Split.Length > 0)
                                        {
                                            for (int s = 0; s <= Menu_Split.GetUpperBound(0); s++)
                                            {
                                                tot.Add(Menu_Split[s]);
                                                for (int sh = 0; sh < cblTotaltype.Items.Count; sh++)
                                                {
                                                    if (Menu_Split[s] == cblTotaltype.Items[sh].Text)
                                                    {
                                                        cblTotaltype.Items[sh].Selected = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (dv.Count > 0)
                                {
                                    #region Menu binding
                                    string menuname = "";
                                    string menucode = "";
                                    for (int k = 0; k < dv.Count; k++)
                                    {
                                        string mname = Convert.ToString(dv[k]["MenuName"]);
                                        string mcode = Convert.ToString(dv[k]["MenuMasterPK"]);
                                        if (menuname.Contains(mname) == false)
                                        {
                                            if (menuname == "")
                                            {
                                                menuname = mname;
                                                menucode = mcode;
                                            }
                                            else
                                            {
                                                menuname = menuname + "," + mname;
                                                menucode = menucode + "," + mcode;
                                            }
                                        }
                                    }
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - 1)].Text = menuname;//Convert.ToString(dv[0]["MenuName"]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - 1)].Tag = menucode;// Convert.ToString(dv[0]["MenuMasterPK"]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - 1)].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - 1)].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - 1)].Font.Name = "Book Antiqua";
                                    #endregion
                                    int ch = 0;
                                    for (int s = 0; s < cblTotaltype.Items.Count; s++)
                                    {
                                        if (cblTotaltype.Items[s].Text != "T")
                                        {
                                            ch++;
                                            string MenuschedulePK = Convert.ToString(dv[0]["MenuschedulePK"]);
                                            ds.Tables[8].DefaultView.RowFilter = " MenuscheduleFK='" + MenuschedulePK + "'";
                                            DataView VegNonvegDv = ds.Tables[8].DefaultView;
                                            if (tot.Contains(cblTotaltype.Items[s].Text))
                                            {
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Locked = false;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].BackColor = Color.Gainsboro;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].CellType = db1;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 2))].Locked = false;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 2))].BackColor = Color.Gainsboro;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 2))].CellType = db1;
                                            }
                                            else
                                            {
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Locked = true;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 2))].Locked = true;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].CellType = db1;
                                            }
                                            if (cblTotaltype.Items[s].Text == "H")
                                            {
                                                //if (ds.Tables[3].Rows.Count > 0)
                                                //{
                                                if (dummyarraydtwise.Contains("H"))//changes 28.02.17
                                                {
                                                    if (Convert.ToString(HostelVegorNvPersentCountDic.Count) != "0")
                                                    {
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(HostelVegorNvPersentCountDic["veg"]);
                                                        ch++;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(HostelVegorNvPersentCountDic["nonveg"]);
                                                    }
                                                    else
                                                    {
                                                        if (VegNonvegDv.Count > 0)
                                                        {
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = (Convert.ToString(VegNonvegDv[0]["HostelVegCount"]) == "0") ? "" : Convert.ToString(VegNonvegDv[0]["HostelVegCount"]);
                                                            ch++;
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = (Convert.ToString(VegNonvegDv[0]["HostelNonvegCount"]) == "0") ? "" : Convert.ToString(VegNonvegDv[0]["HostelNonvegCount"]);
                                                        }
                                                        else
                                                        {
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                            ch++;
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (VegNonvegDv.Count > 0)
                                                    {
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(VegNonvegDv[0]["HostelVegCount"]);
                                                        ch++;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(VegNonvegDv[0]["HostelNonvegCount"]);
                                                    }
                                                    else
                                                    {
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                        ch++;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                    }
                                                }
                                            }
                                            if (cblTotaltype.Items[s].Text == "S")
                                            {
                                                if (dummyarraydtwise.Contains("S"))//barath changes for nec 28.02.17
                                                {
                                                    if (Presentstaffcount.Count > 0)
                                                    {
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(Presentstaffcount.Count);
                                                        ch++;
                                                    }
                                                    else
                                                    {
                                                        if (VegNonvegDv.Count > 0)
                                                        {
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(VegNonvegDv[0]["StaffVegCount"]);
                                                            ch++;
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(VegNonvegDv[0]["StaffNonvegCount"]);
                                                        }
                                                        else
                                                        {
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                            ch++;
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (VegNonvegDv.Count > 0)
                                                    {
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(VegNonvegDv[0]["StaffVegCount"]);
                                                        ch++;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(VegNonvegDv[0]["StaffNonvegCount"]);
                                                    }
                                                    else
                                                        ch++;
                                                }
                                            }
                                            if (cblTotaltype.Items[s].Text == "D")
                                            {
                                                if (ds.Tables[5].Rows.Count > 0)
                                                {
                                                    if (dummyarraydtwise.Contains("D"))//barath changes for nec 28.02.17
                                                    {
                                                        if (Presentcount.Count > 0)
                                                        {
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(Presentcount.Count); ch++;
                                                        }
                                                        else
                                                        {
                                                            if (VegNonvegDv.Count > 0)
                                                            {
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(VegNonvegDv[0]["DayscholorVegCount"]);
                                                                ch++;
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(VegNonvegDv[0]["DayscholorNonvegCount"]);
                                                            }
                                                            else
                                                            {
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                                ch++;
                                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (VegNonvegDv.Count > 0)
                                                        {
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(VegNonvegDv[0]["DayscholorVegCount"]);
                                                            ch++;
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(VegNonvegDv[0]["DayscholorNonvegCount"]);
                                                        }
                                                        else
                                                        {
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                            ch++;
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                        }
                                                    }
                                                }
                                                else { ch++; }
                                                //if (ds.Tables[5].Rows.Count > 0)
                                                //{
                                                //    if (Presentcount.Count > 0)
                                                //    {
                                                //        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(Presentcount.Count);
                                                //    }
                                                //    else
                                                //    {
                                                //        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(dv[0]["DayScholor"]);
                                                //    }
                                                //}
                                                //else
                                                //{
                                                //    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(dv[0]["DayScholor"]);
                                                //}
                                            }
                                            if (cblTotaltype.Items[s].Text == "G")//barath changes for nec 28.02.17
                                            {
                                                if (dummyarraydtwise.Contains("G"))
                                                {
                                                    if (VegNonvegDv.Count > 0)
                                                    {
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(VegNonvegDv[0]["GuestVegCount"]);
                                                        ch++;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(VegNonvegDv[0]["GuestNonvegCount"]);
                                                    }
                                                    else
                                                    {
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                        ch++;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                    }
                                                }
                                                else
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                    ch++;
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                }
                                            }
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].HorizontalAlign = HorizontalAlign.Center;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Font.Size = FontUnit.Medium;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Font.Name = "Book Antiqua";
                                            //sum = sum + Convert.ToInt32(ds.Tables[3].Rows.Count);
                                        }
                                    }
                                    //
                                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(sum);
                                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                }
                                else
                                {
                                    #region without Menuname
                                    //28.07.16
                                    /* int ch = 0;
                                     for (int rs = 0; rs < cblTotaltype.Items.Count; rs++)
                                     {
                                         //if (cblTotaltype.Items[rs].Selected == true)
                                         //{
                                         if (cblTotaltype.Items[rs].Text != "T")
                                         {
                                             ch++;
                                             if (tot.Contains(cblTotaltype.Items[rs].Text))
                                             {
                                                 Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Locked = false;
                                                 Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].BackColor = Color.Gainsboro;
                                                 Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].CellType = db1;
                                             }
                                             else
                                             {
                                                 Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Locked = true;
                                                 Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].CellType = db1;
                                             }
                                             if (cblTotaltype.Items[rs].Text == "H")
                                             {
                                                 DataView dv1 = new DataView();
                                                 ds.Tables[3].DefaultView.RowFilter = "MessMasterFK='" + Convert.ToString(ddlhostelname.SelectedItem.Value) + "'";
                                                 dv1 = ds.Tables[3].DefaultView;
                                                 Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(dv1.Count);
                                                 // Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(ds.Tables[3].Rows.Count);
                                             }
                                             if (cblTotaltype.Items[rs].Text == "S")
                                             {
                                                 if (ds.Tables[4].Rows.Count > 0)
                                                 {
                                                     if (Presentstaffcount.Count > 0)
                                                     {
                                                         Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(Presentstaffcount.Count);
                                                     }
                                                     else
                                                     {
                                                         Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                     }
                                                 }
                                                 else
                                                 {
                                                     Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                 }
                                                 //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(ds.Tables[4].Rows.Count);
                                             }
                                             if (cblTotaltype.Items[rs].Text == "D")
                                             {
                                                 if (ds.Tables[5].Rows.Count > 0)
                                                 {
                                                     if (Presentcount.Count > 0)
                                                     {
                                                         Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(Presentcount.Count);
                                                     }
                                                     else
                                                     {
                                                         Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                     }
                                                 }
                                                 else
                                                 {
                                                     Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                                 }
                                                 // Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = Convert.ToString(ds.Tables[5].Rows.Count);
                                             }
                                             if (cblTotaltype.Items[rs].Text == "G")
                                             {
                                                 Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Text = "";
                                             }
                                             Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].HorizontalAlign = HorizontalAlign.Center;
                                             Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Font.Size = FontUnit.Medium;
                                             Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - (2 + newarray.Count - (ch + 1))].Font.Name = "Book Antiqua";
                                             sum = sum + Convert.ToInt32(ds.Tables[3].Rows.Count);
                                         }
                                         //}
                                     }
                                     Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(sum);
                                     Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                     Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                     Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                      */
                                    #endregion
                                }
                                col += 3;
                            }
                            dt = dt.AddDays(1);
                        }
                        Fpspread1.Visible = true;
                        // div1.Visible = true;
                        errorlable.Visible = false;
                        rptprint.Visible = true;
                        Fpspread1.Sheets[0].FrozenColumnCount = 2;
                        Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                        #endregion
                    }
                }
                else
                {
                    // div1.Visible = false;
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                    errorlable.Visible = true;
                    errorlable.Text = "No Records Found";
                }
            }
            else
            {
                // div1.Visible = false;
                Fpspread1.Visible = false;
                rptprint.Visible = false;
                errorlable.Visible = true;
                errorlable.Text = "Please Select All Fields";
            }
        }
        catch
        {
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            bool saveflage = false;
            if (Fpspread1.Sheets[0].RowCount > 0)
            {
                Fpspread1.SaveChanges();
                string dtaccessdate = DateTime.Now.ToString();
                string dtaccesstime = DateTime.Now.ToLongTimeString();
                ArrayList newarray = new ArrayList();
                if (cblTotaltype.Items.Count > 0)
                {
                    for (int rs = 0; rs < cblTotaltype.Items.Count; rs++)
                    {
                        if (cblTotaltype.Items[rs].Text != "T")
                        {
                            newarray.Add(Convert.ToString(cblTotaltype.Items[rs].Text));
                        }
                    }
                }
                double HostelVegCount = 0;
                double HostelNonvegCount = 0;
                double DayscholorVegCount = 0;
                double DayscholorNonvegCount = 0;
                double StaffVegCount = 0;
                double StaffNonvegCount = 0;
                double GuestVegCount = 0;
                double GuestNonvegCount = 0;
                double hostlerCount = 0;
                double dayscholorCount = 0;
                double staffCount = 0;
                double guestCount = 0;
                int purposeCatagoryCount = GetSelectedItemCount(cbl_purposecatagory);
                //string purposeCatagory = rs.GetSelectedItemsValueAsString(cbl_purposecatagory);
                if (rb1.Checked == true)
                {
                    string purposeCatagory = rs.GetSelectedItemsValueAsString(cbl_purposecatagory);
                    for (int row = 0; row < Fpspread1.Sheets[0].RowCount; row++)
                    {
                        string getdate = Convert.ToString(Fpspread1.Sheets[0].Cells[row, 1].Text);
                        string[] splitdate = getdate.Split('-');
                        splitdate = splitdate[0].Split('/');
                        DateTime dt = new DateTime();
                        if (splitdate.Length > 0)
                        {
                            dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
                        }
                        string getday = dt.ToString("dddd");
                        string hostelname = Convert.ToString(ddlhostelname.SelectedItem.Value);
                        string hostelcode = Convert.ToString(hostelname);
                        for (int col = 2; col < Fpspread1.Sheets[0].Columns.Count; col++)
                        {
                            double totalstrenght = 0;
                            string getmenuname = Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text);
                            string getmenucode = Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Tag);
                            string getsessioncode = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, col].Tag);
                            if (cblTotaltype.Items[0].Selected == true)
                            {
                                col++;
                                double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text), out HostelVegCount);
                                col++;
                                double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text), out HostelNonvegCount);
                            }
                            if (cblTotaltype.Items[1].Selected == true)
                            {
                                col++;
                                double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text), out DayscholorVegCount);
                                col++;
                                double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text), out DayscholorNonvegCount);
                            }
                            if (cblTotaltype.Items[2].Selected == true)
                            {
                                col++;
                                double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text), out StaffVegCount);
                                col++;
                                double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text), out StaffNonvegCount);
                            }
                            if (cblTotaltype.Items[3].Selected == true)
                            {
                                col++;
                                double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text), out GuestVegCount);
                                col++;
                                double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text), out GuestNonvegCount);
                            }
                            totalstrenght = HostelVegCount + HostelNonvegCount + DayscholorVegCount + DayscholorNonvegCount + StaffVegCount + StaffNonvegCount + GuestVegCount + GuestNonvegCount;
                            hostlerCount = HostelVegCount + HostelNonvegCount;
                            dayscholorCount = DayscholorVegCount + DayscholorNonvegCount;
                            staffCount = StaffVegCount + StaffNonvegCount;
                            guestCount = GuestVegCount + GuestNonvegCount;
                            Dictionary<string, string> PurposeCategoryHash = new Dictionary<string, string>();
                            double PurposeVegCount = 0;
                            double PurposeNonVegCount = 0;
                            if (purposeCatagoryCount > 0)
                            {
                                for (int p = 0; p < purposeCatagoryCount * 2; p += 2)
                                {
                                    col++;
                                    string purposeCode = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, col].Tag);
                                    double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text), out PurposeVegCount);
                                    col++;
                                    double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text), out PurposeNonVegCount);
                                    string PurposeValue = Convert.ToString(PurposeVegCount) + "-" + Convert.ToString(PurposeNonVegCount);
                                    if (!PurposeCategoryHash.ContainsKey(purposeCode))
                                        PurposeCategoryHash.Add(purposeCode, PurposeValue);
                                }
                            }
                            if (getmenuname.Trim() != "")
                            {
                                int ins = 0;
                                string[] menucod = getmenucode.Split(',');
                                foreach (string menu in menucod)
                                {
                                    string insertquery = "if exists (select * from HT_MenuSchedule where MenuScheduleDate ='" + dt.ToString("MM/dd/yyyy") + "' and SessionMasterFK ='" + getsessioncode + "' and MessMasterFK ='" + hostelcode + "' and ScheudleItemType='1' and menumasterfk ='" + menu + "') update HT_MenuSchedule set menumasterfk ='" + menu + "',Change_strength='" + totalstrenght + "',Hostler='" + hostlerCount + "',DayScholor='" + dayscholorCount + "',Staffcount='" + staffCount + "',Guestcount='" + guestCount + "',scheduletype='1' where MenuScheduleDate ='" + dt.ToString("MM/dd/yyyy") + "' and SessionMasterFK ='" + getsessioncode + "' and MessMasterFK ='" + hostelcode + "' and ScheudleItemType='1' and menumasterfk ='" + menu + "'";
                                    ins = d2.update_method_wo_parameter(insertquery, "Text");
                                    string menuSchedulePK = d2.GetFunction("select MenuSchedulePK from HT_MenuSchedule where MenuScheduleDate ='" + dt.ToString("MM/dd/yyyy") + "' and SessionMasterFK ='" + getsessioncode + "' and MessMasterFK ='" + hostelcode + "' and ScheudleItemType='1' and menumasterfk ='" + menu + "'");
                                    string StudMenuStrengthQ = " if exists (select * from HT_HostelStudMenuStrength where MenuscheduleFK='" + menuSchedulePK + "' and Strengthdate='" + dt.ToString("MM/dd/yyyy") + "')update HT_HostelStudMenuStrength set HostelVegCount='" + HostelVegCount + "',HostelNonvegCount='" + HostelNonvegCount + "',DayscholorVegCount='" + DayscholorVegCount + "',DayscholorNonvegCount='" + DayscholorNonvegCount + "',StaffVegCount='" + StaffVegCount + "',StaffNonvegCount='" + StaffNonvegCount + "',GuestVegCount='" + GuestVegCount + "',GuestNonvegCount='" + GuestNonvegCount + "' where MenuscheduleFK='" + menuSchedulePK + "' and Strengthdate='" + dt.ToString("MM/dd/yyyy") + "' else insert into HT_HostelStudMenuStrength(MenuscheduleFK,HostelVegCount,HostelNonvegCount,DayscholorVegCount,DayscholorNonvegCount, StaffVegCount,StaffNonvegCount,GuestVegCount, GuestNonvegCount,Strengthdate)values('" + menuSchedulePK + "','" + HostelVegCount + "','" + HostelNonvegCount + "','" + DayscholorVegCount + "','" + DayscholorNonvegCount + "','" + StaffVegCount + "','" + StaffNonvegCount + "','" + GuestVegCount + "','" + GuestNonvegCount + "','" + dt.ToString("MM/dd/yyyy") + "')";
                                    ins = d2.update_method_wo_parameter(StudMenuStrengthQ, "Text");
                                    if (PurposeCategoryHash.Count > 0)
                                    {
                                        foreach (KeyValuePair<string, string> PurposeCategory in PurposeCategoryHash)
                                        {
                                            string VegCount = Convert.ToString(PurposeCategory.Value).Split('-')[0];
                                            string NonVegCount = Convert.ToString(PurposeCategory.Value).Split('-')[1];
                                            string PurposeCode = Convert.ToString(PurposeCategory.Key);
                                            string PurposeCategoryQry = " if exists (select * from HT_HostelMenupurposeStrength where MenuscheduleFK='" + menuSchedulePK + "' and PurposeCode='" + PurposeCode + "')update HT_HostelMenupurposeStrength set VegCount='" + VegCount + "',NonVegCount='" + NonVegCount + "',PurposeCode='" + PurposeCode + "' where MenuscheduleFK='" + menuSchedulePK + "'  and PurposeCode='" + PurposeCode + "' else insert into HT_HostelMenupurposeStrength(MenuscheduleFK,VegCount,NonVegCount,PurposeCode)values('" + menuSchedulePK + "','" + VegCount + "','" + NonVegCount + "','" + PurposeCode + "')";
                                            ins = d2.update_method_wo_parameter(PurposeCategoryQry, "Text");
                                        }
                                    }
                                }
                                if (ins != 0)
                                {
                                    saveflage = true;
                                }
                            }
                        }
                    }
                }
                if (rb2.Checked == true)
                {
                    string purposeCatagory = rs.GetSelectedItemsValueAsString(cbl_purposecatagory);
                    for (int row = 0; row < Fpspread1.Sheets[0].RowCount; row++)
                    {
                        string getday = Convert.ToString(Fpspread1.Sheets[0].Cells[row, 1].Text).Split('-')[1];
                        string hostelname = Convert.ToString(ddlhostelname.SelectedItem.Value);
                        string hostelcode = Convert.ToString(hostelname);

                        string getdate = Convert.ToString(Fpspread1.Sheets[0].Cells[row, 1].Text);
                        string[] splitdate = getdate.Split('-');
                        splitdate = splitdate[0].Split('/');
                        DateTime dt = new DateTime();
                        if (splitdate.Length > 0)
                        {
                            dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
                        }
                        for (int col = 2; col < Fpspread1.Sheets[0].Columns.Count; col++)
                        {
                            double totalstrenght = 0;
                            //string getmenuname = Convert.ToString(Fpspread1.Sheets[0].Cells[row, col - 4].Text);
                            //string getmenucode = Convert.ToString(Fpspread1.Sheets[0].Cells[row, col - 4].Tag);
                            //string changestrength = Convert.ToString(Fpspread1.Sheets[0].Cells[row, col - 3].Text);

                            string getmenuname = Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text);
                            string getmenucode = Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Tag);
                            string getsessioncode = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, col].Tag);
                            if (cblTotaltype.Items[0].Selected == true)
                            {
                                col++;
                                double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text), out HostelVegCount);
                                col++;
                                double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text), out HostelNonvegCount);
                            }
                            if (cblTotaltype.Items[1].Selected == true)
                            {
                                col++;
                                double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text), out DayscholorVegCount);
                                col++;
                                double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text), out DayscholorNonvegCount);
                            }
                            if (cblTotaltype.Items[2].Selected == true)
                            {
                                col++;
                                double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text), out StaffVegCount);
                                col++;
                                double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text), out StaffNonvegCount);
                            }
                            if (cblTotaltype.Items[3].Selected == true)
                            {
                                col++;
                                double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text), out GuestVegCount);
                                col++;
                                double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text), out GuestNonvegCount);
                            }
                            totalstrenght = HostelVegCount + HostelNonvegCount + DayscholorVegCount + DayscholorNonvegCount + StaffVegCount + StaffNonvegCount + GuestVegCount + GuestNonvegCount;

                            hostlerCount = HostelVegCount + HostelNonvegCount;
                            dayscholorCount = DayscholorVegCount + DayscholorNonvegCount;
                            staffCount = StaffVegCount + StaffNonvegCount;
                            guestCount = GuestVegCount + GuestNonvegCount;

                            Dictionary<string, string> PurposeCategoryHash = new Dictionary<string, string>();
                            double PurposeVegCount = 0;
                            double PurposeNonVegCount = 0;
                            if (purposeCatagoryCount > 0)
                            {
                                for (int p = 0; p < purposeCatagoryCount * 2; p += 2)
                                {
                                    col++;
                                    string purposeCode = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, col].Tag);
                                    double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text), out PurposeVegCount);
                                    col++;
                                    double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text), out PurposeNonVegCount);
                                    string PurposeValue = Convert.ToString(PurposeVegCount) + "-" + Convert.ToString(PurposeNonVegCount);
                                    if (!PurposeCategoryHash.ContainsKey(purposeCode))
                                        PurposeCategoryHash.Add(purposeCode, PurposeValue);
                                }
                            }
                            if (getmenuname.Trim() != "")
                            {
                                int ins = 0;
                                string[] menucod = getmenucode.Split(',');
                                foreach (string menu in menucod)
                                {
                                    string insertquery = "if exists (select * from HT_MenuSchedule where MenuScheduleday ='" + getday + "' and SessionMasterFK ='" + getsessioncode + "' and MessMasterFK ='" + hostelcode + "' and ScheudleItemType='1' and menumasterfk ='" + menu + "') update HT_MenuSchedule set menumasterfk ='" + menu + "',Change_strength='" + totalstrenght + "',Hostler='" + hostlerCount + "',DayScholor='" + dayscholorCount + "',Staffcount='" + staffCount + "',Guestcount='" + guestCount + "',scheduletype='2' where MenuScheduleday ='" + getday + "' and SessionMasterFK ='" + getsessioncode + "' and MessMasterFK ='" + hostelcode + "' and ScheudleItemType='1' and menumasterfk ='" + menu + "'";
                                    ins = d2.update_method_wo_parameter(insertquery, "Text");
                                    string menuSchedulePK = d2.GetFunction("select MenuSchedulePK from HT_MenuSchedule where MenuScheduleday ='" + getday + "' and SessionMasterFK ='" + getsessioncode + "' and MessMasterFK ='" + hostelcode + "' and ScheudleItemType='1' and menumasterfk ='" + menu + "'");
                                    string StudMenuStrengthQ = " if exists (select * from HT_HostelStudMenuStrength where MenuscheduleFK='" + menuSchedulePK + "' and Strengthdate='" + dt.ToString("MM/dd/yyyy") + "')update HT_HostelStudMenuStrength set HostelVegCount='" + HostelVegCount + "',HostelNonvegCount='" + HostelNonvegCount + "',DayscholorVegCount='" + DayscholorVegCount + "',DayscholorNonvegCount='" + DayscholorNonvegCount + "',StaffVegCount='" + StaffVegCount + "',StaffNonvegCount='" + StaffNonvegCount + "',GuestVegCount='" + GuestVegCount + "',GuestNonvegCount='" + GuestNonvegCount + "' where MenuscheduleFK='" + menuSchedulePK + "' and Strengthdate='" + dt.ToString("MM/dd/yyyy") + "' else insert into HT_HostelStudMenuStrength(MenuscheduleFK,HostelVegCount,HostelNonvegCount,DayscholorVegCount,DayscholorNonvegCount, StaffVegCount,StaffNonvegCount,GuestVegCount,GuestNonvegCount,Strengthdate)values('" + menuSchedulePK + "','" + HostelVegCount + "','" + HostelNonvegCount + "','" + DayscholorVegCount + "','" + DayscholorNonvegCount + "','" + StaffVegCount + "','" + StaffNonvegCount + "','" + GuestVegCount + "','" + GuestNonvegCount + "','" + dt.ToString("MM/dd/yyyy") + "')";
                                    ins = d2.update_method_wo_parameter(StudMenuStrengthQ, "Text");
                                    if (PurposeCategoryHash.Count > 0)
                                    {
                                        foreach (KeyValuePair<string, string> PurposeCategory in PurposeCategoryHash)
                                        {
                                            string VegCount = Convert.ToString(PurposeCategory.Value).Split('-')[0];
                                            string NonVegCount = Convert.ToString(PurposeCategory.Value).Split('-')[1];
                                            string PurposeCode = Convert.ToString(PurposeCategory.Key);
                                            string PurposeCategoryQry = " if exists (select * from HT_HostelMenupurposeStrength where MenuscheduleFK='" + menuSchedulePK + "' and PurposeCode='" + PurposeCode + "')update HT_HostelMenupurposeStrength set VegCount='" + VegCount + "',NonVegCount='" + NonVegCount + "',PurposeCode='" + PurposeCode + "' where MenuscheduleFK='" + menuSchedulePK + "'  and PurposeCode='" + PurposeCode + "' else insert into HT_HostelMenupurposeStrength(MenuscheduleFK,VegCount,NonVegCount,PurposeCode)values('" + menuSchedulePK + "','" + VegCount + "','" + NonVegCount + "','" + PurposeCode + "')";
                                            ins = d2.update_method_wo_parameter(PurposeCategoryQry, "Text");
                                        }
                                    }
                                }
                                if (ins != 0)
                                {
                                    saveflage = true;
                                }
                            }
                        }
                    }
                }
                if (saveflage == true)
                {
                    btngo_click(sender, e);
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Saved Successfully";
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please Update Menu Schedule First";
                }
            }
        }
        catch
        {
        }
    }
    protected void Cell_Click(object sender, EventArgs e)
    {
        try
        {
            check = true;
        }
        catch
        {
        }
    }
    protected void bindPurposeCatagory()
    {
        try
        {
            cbl_purposecatagory.Items.Clear();
            ds.Clear();
            string sql = "select distinct MasterCode,MasterValue  from CO_MasterValues where MasterCriteria ='Menu Purpose Category' and CollegeCode ='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_purposecatagory.DataSource = ds;
                cbl_purposecatagory.DataTextField = "MasterValue";
                cbl_purposecatagory.DataValueField = "MasterCode";
                cbl_purposecatagory.DataBind();
                //cbl_purposecatagory.Items.Insert(0, new ListItem("Select", "0"));
                lbl_menuPurposeCatagory.Visible = true;
                purposecatagoryTD.Visible = true;
            }
            else
            {
                lbl_menuPurposeCatagory.Visible = false;
                purposecatagoryTD.Visible = false;
            }
        }
        catch
        { }
    }
    protected void cb_purposecatagory_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_purposecatagory.Checked == true)
            {
                for (int i = 0; i < cbl_purposecatagory.Items.Count; i++)
                {
                    cbl_purposecatagory.Items[i].Selected = true;
                }
                txt_purposecatagory.Text = "Category(" + (cbl_purposecatagory.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_purposecatagory.Items.Count; i++)
                {
                    cbl_purposecatagory.Items[i].Selected = false;
                }
                txt_purposecatagory.Text = "--Select--";
            }
        }
        catch (Exception ex)
        { }
    }
    protected void cbl_purposecatagory_SelectIndexChange(object sender, EventArgs e)
    {
        try
        {
            txt_purposecatagory.Text = "--Select--";
            cb_purposecatagory.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_purposecatagory.Items.Count; i++)
            {
                if (cbl_purposecatagory.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_purposecatagory.Text = "Category(" + commcount.ToString() + ")";
                if (commcount == cbl_purposecatagory.Items.Count)
                {
                    cb_purposecatagory.Checked = true;
                }
            }
        }
        catch (Exception ex)
        { }
    }
    public int GetSelectedItemCount(CheckBoxList cblSelected)
    {
        int sbSelected = 0;
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    sbSelected++;
                }
            }
        }
        catch { sbSelected = 0; }
        return sbSelected;
    }
    protected string returnDSwithSigleColumnSingleCodeValue(DataTable Dt, string Col)
    {
        string empty = "";
        if (Dt.Rows.Count > 0)
        {
            if (Dt.Rows.Count > 0)
            {
                foreach (DataRow dr in Dt.Rows)
                {
                    if (empty == "")
                        empty = Convert.ToString(dr[Col]);
                    else
                        empty = empty + "','" + Convert.ToString(dr[Col]);
                }
            }
        }
        return empty;
    }

}