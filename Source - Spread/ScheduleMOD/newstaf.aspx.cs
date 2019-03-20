using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class newstaf : System.Web.UI.Page
{
    string batchsetting = string.Empty;

    [Serializable()]
    public class myHyperLink : FarPoint.Web.Spread.HyperLinkCellType
    {
        FarPoint.Web.Spread.Model.DefaultSheetDataModel dsdm;
        public myHyperLink()
        {
        }

        public myHyperLink(FarPoint.Web.Spread.Model.DefaultSheetDataModel mydatamodel)
        {
            dsdm = mydatamodel;
        }

        public override Control PaintCell(string id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, bool upperLevel)
        {
            Control c = base.PaintCell(id, parent, style, margin, value, upperLevel);
            string[] idarray = id.Split(new char[] { ',' });
            int row = Convert.ToInt32(idarray[0]);
            string getselectedpath = path1;
            HyperLink hypType = (HyperLink)c;
            hypType.Text = value.ToString();
            //hypType.NavigateUrl = "http://www.fpoint.com?s1=" + field1 + "s2=" + field2;
            hypType.NavigateUrl = getselectedpath;
            hypType.Target = "_self";
            return hypType;
        }
    }

    SqlConnection ssql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    FarPoint.Web.Spread.HyperLinkCellType hypertext = new FarPoint.Web.Spread.HyperLinkCellType();
    SqlCommand cmd = new SqlCommand();

    bool Cellclick;
    bool cellclick2;
    bool cellclick3;
    static string path1 = string.Empty;
    string staff_code = string.Empty;
    string loadunitssubj_no = string.Empty;
    bool flag_true = false;
    bool cellclick1 = false;
    static int flag1 = 0;
    string strdayflag;
    string regularflag = string.Empty;
    string genderflag = string.Empty;
    string str = string.Empty;
    string degree_code = string.Empty;
    string semester = string.Empty;
    bool chk = false;
    string strday = string.Empty;
    TreeNode cnode;
    TreeNode ynode;
    string subject_no = string.Empty;
    string sections = string.Empty;
    string hr = string.Empty;
    static string sel_date1 = string.Empty;
    static string sel_date = string.Empty;
    static string getcelltag = string.Empty;
    string getcolheader = string.Empty;
    string getdate = string.Empty;
    string getdate_new = string.Empty;
    string strsec = string.Empty;
    string Att_strqueryst = string.Empty;
    string subj_count_in_onehr = string.Empty;
    static string selectedpath = string.Empty;
    static string storepath = string.Empty;
    bool check_record = false;
    int len = 0, len1 = 0, len2 = 0, len3 = 0, len4 = 0;
    static int ar;
    static int ac;
    string subj_type = string.Empty;
    string MsgText = string.Empty;
    string RecepientNo = string.Empty;
    string AttDate = string.Empty;
    string AttHour = string.Empty;
    string tmp_camprevar = string.Empty;
    string cur_camprevar = string.Empty;
    DAccess2 da = new DAccess2();
    DataSet ds_iscount = new DataSet();
    DataSet ds_attndmaster = new DataSet();
    Hashtable present_calcflag = new Hashtable();
    Hashtable absent_calcflag = new Hashtable();
    Hashtable hat = new Hashtable();
    static Hashtable ht_sch = new Hashtable();
    static Hashtable ht_sdate = new Hashtable();
    static Hashtable ht_bell = new Hashtable();
    static Hashtable ht_period = new Hashtable();
    int Att_mark_column = 0, Att_mark_row = 0, absent_count = 0, present_count = 0;
    int count_master = 0;
    static string grouporusercode = string.Empty;
    static bool hr_lock = false;
    string noofdays = string.Empty;
    string start_datesem = string.Empty;
    string start_dayorder = string.Empty;
    string degree_var = string.Empty;
    string tmp_datevalue = string.Empty;
    string strsction = string.Empty;
    string Day_Order = string.Empty;
    string Day_Var = string.Empty;
    bool singlesubject = false;
    string singlesubjectno = string.Empty;
    static int inicolcount = 0;
    string SenderID = string.Empty;
    string Password = string.Empty;
    string user_id = string.Empty;
    bool slipfalg = false;
    string strquerytext = string.Empty;
    DataSet ds = new DataSet();
    Hashtable hatroll = new Hashtable();
    string strinvalidroll = string.Empty;
    bool dailyentryflag = false;
    bool attendanceentryflag = false;
    Hashtable has_attnd_masterset = new Hashtable();
    Hashtable has_attnd_masterset_notconsider = new Hashtable();
    Hashtable hatabsentvalues = new Hashtable();
    Hashtable has_total_attnd_hour = new Hashtable();
    Hashtable ht_sphr = new Hashtable();
    Hashtable has_hs = new Hashtable();
    Hashtable has_load_rollno = new Hashtable();
    DateTime dumm_from_date;
    Dictionary<string, DateTime[]> dicFeeOfRollStudents = new Dictionary<string, DateTime[]>();
    Dictionary<string, byte> dicFeeOnRollStudents = new Dictionary<string, byte>();

    public bool daycheck(DateTime seldate)
    {
        string collegecode = Session["collegecode"].ToString();
        bool daycheck = false;
        DateTime curdate;//, prevdate;
        long total, k, s;
        string[] ddate;
        //DateTime[] ddate = new DateTime[500];
        //curdate == DateTime.Today.ToString() ;
        string c_date = DateTime.Today.ToString();
        DateTime todate_day = Convert.ToDateTime(DateTime.Today.ToString());
        curdate = DateTime.Today;
        if (seldate.ToString() == c_date)
        {
            daycheck = true;
            return daycheck;
        }
        else
        {
            //Modified by srinath 12/8/2013
            string lockdayvalue = "select lockdays,lflag from collinfo where college_code=" + collegecode + "";
            DataSet ds = new DataSet();
            ds = da.select_method(lockdayvalue, hat, "Text");
            // da.Fill(ds);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i][1].ToString() == "True")
                    {
                        if (ds.Tables[0].Rows[i][0].ToString() != null && int.Parse(ds.Tables[0].Rows[i][0].ToString()) >= 0)
                        {
                            total = int.Parse(ds.Tables[0].Rows[i][0].ToString());
                            total = total + 1;
                            //Modified by srinath 12/8/2013
                            String strholidasquery = "select holiday_date from holidaystudents where degree_code=" + Session["deg_code"].ToString() + "  and semester=" + Session["semester"].ToString() + "";
                            DataSet ds1 = new DataSet();
                            ds1 = da.select_method(strholidasquery, hat, "Text");
                            //if (ds1.Tables[0].Rows.Count <= 0)
                            if (ds1.Tables[0].Rows.Count <= 0)
                            {
                                for (int i1 = 1; i1 < total; i1++)
                                {
                                    string temp_date = todate_day.AddDays(-i1).ToString();
                                    string temp2 = todate_day.AddDays(i1).ToString();
                                    if (temp_date == seldate.ToString())
                                    {
                                        daycheck = true;
                                        return daycheck;
                                    }
                                    if (temp2 == seldate.ToString())
                                    {
                                        daycheck = true;
                                        return daycheck;
                                    }
                                }
                            }
                            else
                            {
                                k = 0;
                                ddate = new string[ds1.Tables[0].Rows.Count];
                                for (int i1 = 1; i1 < ds1.Tables[0].Rows.Count; i1++)
                                {
                                    ddate[k] = ds1.Tables[0].Rows[i1][0].ToString();
                                    k++;
                                }
                                i = 0;
                                while (i <= total - 1)
                                {
                                    string temp_date = curdate.AddDays(-i).ToString();
                                    for (s = 0; s < k - 1; s++)
                                    {
                                        if (temp_date == ddate[s].ToString())
                                        {
                                            total = total + 1;
                                            goto lab;
                                        }
                                    }
                                lab:
                                    i = i + 1;
                                    if (temp_date == seldate.ToString())
                                    {
                                        daycheck = true;
                                        return daycheck;
                                    }
                                }
                            }
                        }
                        else
                        {
                            daycheck = true;
                        }
                    }
                    else
                    {
                        daycheck = true;
                    }
                }
            }
        }
        return daycheck;
    }

    public bool DayLockForUser(DateTime seldate)
    {
        string collegecode = Session["collegecode"].ToString();
        bool daycheck = false;
        DateTime curdate;//, prevdate;
        long total, k, s;
        string[] ddate = new string[500];
        string c_date = DateTime.Today.ToString();
        DateTime todate_day = Convert.ToDateTime(DateTime.Today.ToString());
        curdate = DateTime.Today;
        if (seldate.ToString() == c_date)
        {
            daycheck = true;
            return daycheck;
        }
        else
        {
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim().Split(';')[0] + "";
            }
            else
            {
                grouporusercode = " and usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            string lockdayvalue = "select value from Master_Settings where settings='Attendance Lock Days' " + grouporusercode + "";
            DataSet ds = new DataSet();
            ds = da.select_method(lockdayvalue, hat, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i][0].ToString() != null && ds.Tables[0].Rows[i][0].ToString() != "")
                    {
                        total = int.Parse(ds.Tables[0].Rows[i][0].ToString());
                        total = total + 1;
                        String strholidasquery = "select holiday_date from holidaystudents where degree_code='" + Convert.ToString(Session["deg_code"]).Trim() + "'  and semester='" + Convert.ToString(Session["semester"]).Trim() + "'";
                        DataSet ds1 = new DataSet();
                        ds1 = da.select_method(strholidasquery, hat, "Text");
                        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count <= 0)
                        {
                            for (int i1 = 1; i1 < total; i1++)
                            {
                                string temp_date = todate_day.AddDays(-i1).ToString();
                                string temp2 = todate_day.AddDays(i1).ToString();
                                if (temp_date == seldate.ToString())
                                {
                                    daycheck = true;
                                    return daycheck;
                                }
                                if (temp2 == seldate.ToString())
                                {
                                    daycheck = true;
                                    return daycheck;
                                }
                            }
                        }
                        else
                        {
                            k = 0;
                            for (int i1 = 1; i1 < ds1.Tables[0].Rows.Count; i1++)
                            {
                                ddate[k] = ds1.Tables[0].Rows[i1][0].ToString();
                                k++;
                            }
                            i = 0;
                            while (i <= total - 1)
                            {
                                string temp_date = curdate.AddDays(-i).ToString();
                                for (s = 0; s < k - 1; s++)
                                {
                                    if (temp_date == ddate[s].ToString())
                                    {
                                        total = total + 1;
                                        goto lab;
                                    }
                                }
                            lab:
                                i = i + 1;
                                if (temp_date == seldate.ToString())
                                {
                                    daycheck = true;
                                    return daycheck;
                                }
                            }
                        }
                    }
                    else
                    {
                        daycheck = true;
                    }
                }
            }
        }
        return daycheck;
    }

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {
        Control cntUpdateBtn = FpSpread3.FindControl("Update");
        Control cntCancelBtn = FpSpread3.FindControl("Cancel");
        Control cntCopyBtn = FpSpread3.FindControl("Copy");
        Control cntCutBtn = FpSpread3.FindControl("Clear");
        Control cntPasteBtn = FpSpread3.FindControl("Paste");
        Control cntNextBtn = FpSpread3.FindControl("Next");
        Control cntPreviousBtn = FpSpread3.FindControl("Previous");
        Control cntPagePrintBtn = FpSpread3.FindControl("Print");
        Control cntUpdateBtn1 = spreadatt_qtnadd.FindControl("Update");
        Control cntCancelBtn1 = spreadatt_qtnadd.FindControl("Cancel");
        Control cntCopyBtn1 = spreadatt_qtnadd.FindControl("Copy");
        Control cntCutBtn1 = spreadatt_qtnadd.FindControl("Clear");
        Control cntPasteBtn1 = spreadatt_qtnadd.FindControl("Paste");
        Control cntPagePrintBtn1 = spreadatt_qtnadd.FindControl("Print");
        if ((cntUpdateBtn != null))
        {
            TableCell tc = (TableCell)cntUpdateBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;
            tr.Cells.Remove(tc);
            tc = (TableCell)cntCancelBtn.Parent;
            tr.Cells.Remove(tc);
            //tc = (TableCell)cntCancelBtn1.Parent;
            //tr.Cells.Remove(tc);
            tc = (TableCell)cntCopyBtn.Parent;
            tr.Cells.Remove(tc);
            tc = (TableCell)cntCutBtn.Parent;
            tr.Cells.Remove(tc);
            tc = (TableCell)cntPasteBtn.Parent;
            tr.Cells.Remove(tc);
            tc = (TableCell)cntNextBtn.Parent;
            tr.Cells.Remove(tc);
            //tc = (TableCell)cntPreviousBtn.Parent;
            //tr.Cells.Remove(tc);
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("ScheduleHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/ScheduleMOD/ScheduleHome.aspx");
                    return;
                }
            }
            FpSpread3.Sheets[0].AutoPostBack = true;
            // Buttonsavelesson.Enabled = false;
            pnl_sliplist.Visible = false;
            btnsliplist.Enabled = false;
            staff_code = (string)Session["Staff_Code"];
            FpSpread1.Sheets[0].Columns.Default.Width = 300;
            btnsliplist.Visible = false;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            chkis_studavailable.Checked = false;
            if (!IsPostBack)
            {
                tbfdate.Attributes.Add("readonly", "readonly");
                tbtodate.Attributes.Add("readonly", "readonly");
                FpSpread3.CommandBar.Visible = false;
                btnaddquestion.Enabled = true;
                btnqtnupdate.Enabled = false;
                string dateFormat = DateTime.Now.GetDateTimeFormats('d')[0];
                if (staff_code == "" || staff_code == null)
                {
                    // Response.Write("You Are not a Valid Staff");
                    //return;//Hided by Manikandan 14/08/2013
                }
                //Start==============added by Manikandan 14/08/2013================
                tbfdate.Text = DateTime.Now.AddDays(0).ToString("d-MM-yyyy");
                // =datefrom.ToString();
                tbtodate.Text = DateTime.Now.AddDays(0).ToString("d-MM-yyyy");

                //added by rajasekar 26/10/2018
                DateTime baseDate = DateTime.Today;

                var today = baseDate;
                var yesterday = baseDate.AddDays(-1);
                var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                var monday = thisWeekStart.AddDays(1);
                tbfdate.Text = monday.ToString("d-MM-yyyy");
                tbtodate.Text = thisWeekEnd.ToString("d-MM-yyyy");
                //================================//

                string staffOrAdmin = Convert.ToString(Session["StafforAdmin"]).Trim();

                Session["curr_year"] = DateTime.Now.ToString("yyyy");
                if ((staffOrAdmin.ToLower().Trim() == "staff"))
                {
                    //scheduleorattnd = 1;                
                    loadstafspread();
                    Label1.Text = "Individual Staff Report";
                    scodelbl.Visible = false;
                    scodetxt.Visible = false;
                    snamelbl.Visible = false;
                    snamelbl1.Visible = false;
                    lblstaffname.Visible = false;
                    ddlstaffname.Visible = false;
                }
                else if ((staffOrAdmin.ToLower().Trim() == "admin"))
                {
                    //scheduleorattnd = 1;
                    bindstaff();
                    loadstafspread();
                    scodelbl.Visible = true;
                    scodetxt.Visible = true;
                    snamelbl.Visible = true;
                    snamelbl1.Visible = false;
                    FpSpread1.Visible = false;
                    lblstaffname.Visible = true;
                    ddlstaffname.Visible = true;
                    Label1.Text = "Individual Staff Report";
                    clearfield();
                }
                else
                {
                    if (string.IsNullOrEmpty(Convert.ToString(Session["Staff_Code"]).Trim()))
                    {
                        bindstaff();
                        loadstafspread();
                        scodelbl.Visible = true;
                        scodetxt.Visible = true;
                        snamelbl.Visible = true;
                        snamelbl1.Visible = false;
                        FpSpread1.Visible = false;
                        lblstaffname.Visible = true;
                        ddlstaffname.Visible = true;
                        Label1.Text = "Individual Staff Report";
                        clearfield();
                    }
                    else
                    {
                        //Label1.Text = "Individual Staff Report";
                        scodelbl.Visible = false;
                        scodetxt.Visible = false;
                        snamelbl.Visible = false;
                        snamelbl1.Visible = false;
                        lblstaffname.Visible = false;
                        ddlstaffname.Visible = false;
                    }
                }
                //===============================End===============================
                FpSpread1.Sheets[0].Columns.Default.Font.Name = "Book Antiqua";
                // FpSpread1.Sheets[0].Columns.Default.Font.Bold=true;
                FpSpread1.Sheets[0].Columns.Default.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Rows.Default.Font.Name = "Book Antiqua";
                // FpSpread1.Sheets[0].Rows.Default.Font.Bold = true;
                FpSpread1.Sheets[0].Rows.Default.Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].Columns.Default.Font.Name = "Book Antiqua";
                // FpSpread2.Sheets[0].Columns.Default.Font.Bold = true;
                FpSpread2.Sheets[0].Columns.Default.Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].Rows.Default.Font.Name = "Book Antiqua";
                // FpSpread2.Sheets[0].Rows.Default.Font.Bold = true;
                FpSpread2.Sheets[0].Rows.Default.Font.Size = FontUnit.Medium;
                Color c = FpSpread1.ColumnHeader.DefaultStyle.BackColor;
                FpSpread2.ActiveSheetView.SheetCorner.DefaultStyle.BackColor = Color.LightCyan;
                FpSpread1.ActiveSheetView.SheetCorner.DefaultStyle.BackColor = Color.LightCyan;
                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = FontUnit.Medium;
                style.Font.Bold = true;
                style.Font.Name = "Book Antiqua";
                style.HorizontalAlign = HorizontalAlign.Center;
                style.ForeColor = Color.Black;
                style.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                FpSpread2.Sheets[0].ColumnHeader.DefaultStyle = style;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = style;
                FpSpread3.Sheets[0].ColumnHeader.DefaultStyle = style;
                fpattendanceentry.Sheets[0].ColumnHeader.DefaultStyle = style;
                spreadatt_qtnadd.Sheets[0].ColumnHeader.DefaultStyle = style;
                sprdnoofchoices.Sheets[0].ColumnHeader.DefaultStyle = style;
                sprdviewdata.Sheets[0].ColumnHeader.DefaultStyle = style;
                spread_sliplist.Sheets[0].ColumnHeader.DefaultStyle = style;
                FpSpread1.Sheets[0].RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].RowHeader.DefaultStyle.Font.Bold = true;
                FpSpread1.Sheets[0].RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread2.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                FpSpread2.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpSpread1.Sheets[0].AllowTableCorner = true;
                FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpSpread2.Sheets[0].AllowTableCorner = true;
                FpSpread2.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.SheetCorner.Rows.Default.Font.Size = FontUnit.Medium;
                FpSpread1.SheetCorner.Rows.Default.Font.Name = "Book Antiqua";
                FpSpread1.SheetCorner.Rows.Default.Font.Bold = true;
                FpSpread2.SheetCorner.Rows.Default.Font.Size = FontUnit.Medium;
                FpSpread2.SheetCorner.Rows.Default.Font.Name = "Book Antiqua";
                FpSpread2.SheetCorner.Rows.Default.Font.Bold = true;
                FpSpread2.Sheets[0].SheetCorner.RowCount = 2;
                FpSpread2.ActiveSheetView.SheetCornerSpanModel.Add(0, 0, 2, 1);
                FpSpread2.ActiveSheetView.SheetCorner.Cells[0, 0].Text = "S.No";
                FpSpread2.Sheets[0].Columns.Default.HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread2.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread2.Sheets[0].SheetCorner.DefaultStyle.BackColor = FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.BackColor;
                FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].Text = "Date";
                tvyet.Attributes.Add("onclick", "postBackByObject()");
                tvcomplete.Attributes.Add("onclick", "postBackByObject()");
                ddlmark.Items.Insert(0, new ListItem("--Select--", "-1"));
                ddlmark.Items.Add("P");
                ddlmark.Items.Add("A");
                ddlmark.Items.Add("OD");
                ddlmark.Items.Add("SOD");
                ddlmark.Items.Add("ML");
                ddlmark.Items.Add("NSS");
                ddlmark.Items.Add("L");
                ddlmarkothers.Items.Insert(0, new ListItem("--Select--", "-1"));
                ddlmarkothers.Items.Add("P");
                ddlmarkothers.Items.Add("A");
                ddlmarkothers.Items.Add("OD");
                ddlmarkothers.Items.Add("SOD");
                ddlmarkothers.Items.Add("ML");
                ddlmarkothers.Items.Add("NSS");
                ddlmarkothers.Items.Add("L");
                Session["curr_year"] = DateTime.Now.ToString("yyyy");
                Session["Rollflag"] = "0";//26.01.17
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                Session["Sex"] = "0";
                Session["flag"] = "-1";
                FpSpread2.SaveChanges();
                fpattendanceentry.CommandBar.Visible = false;
                fpattendanceentry.Height = 100;
                fpattendanceentry.Width = 600;
                fpattendanceentry.SheetCorner.ColumnCount = 0;
                fpattendanceentry.SheetCorner.RowCount = 0;
                fpattendanceentry.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
                fpattendanceentry.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
                string Master = "select * from Master_Settings where " + grouporusercode + "";
                DataSet ds = da.select_method(Master, hat, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "sex" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Sex"] = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "General attend" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            option.SelectedValue = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Absentees" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            option.SelectedValue = "2";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "RollNo" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            RadioButtonList1.SelectedValue = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "RegisterNo" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            RadioButtonList1.SelectedValue = "2";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Admission No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            RadioButtonList1.SelectedValue = "3";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "General" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["flag"] = 0;
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "As Per Lesson" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["flag"] = 1;
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Male" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            genderflag = " and (applyn.sex='0'";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Female" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            if (genderflag != "" && genderflag != "\0")
                            {
                                genderflag = genderflag + " or applyn.sex='1'";
                            }
                            else
                            {
                                genderflag = " and (applyn.sex='1'";
                            }
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Days Scholor" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            strdayflag = " and (registration.Stud_Type='Day Scholar'";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Hostel" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            if (strdayflag != null && strdayflag != "\0")
                            {
                                strdayflag = strdayflag + " or registration.Stud_Type='Hostler'";
                            }
                            else
                            {
                                strdayflag = " and (registration.Stud_Type='Hostler'";
                            }
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Regular" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            regularflag = "and ((registration.mode=1)";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Lateral" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            if (regularflag != "")
                            {
                                regularflag = regularflag + " or (registration.mode=3)";
                            }
                            else
                            {
                                regularflag = regularflag + " and ((registration.mode=3)";
                            }
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Transfer" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            if (regularflag != "")
                            {
                                regularflag = regularflag + " or (registration.mode=2)";
                            }
                            else
                            {
                                regularflag = regularflag + " and ((registration.mode=2)";
                            }
                        }
                    }
                }
                if (strdayflag != null && strdayflag != "")
                {
                    strdayflag = strdayflag + ")";
                }
                Session["strvar"] = strdayflag;
                if (regularflag != "")
                {
                    regularflag = regularflag + ")";
                }
                if (genderflag != "")
                {
                    genderflag = genderflag + ")";
                }
                Session["strvar"] = Session["strvar"] + regularflag + genderflag;
                Session["StaffSelector"] = "0";
                Session["Copy Attendance"] = "0";
                string rightscopy = da.GetFunction("select value from Master_Settings where settings='Copy Attendance'  and " + grouporusercode + "");
                if (rightscopy == "1")
                {
                    Session["Copy Attendance"] = "1";
                }
                //********************************************
                if (Session["StafforAdmin"] == "")//Added by Manikandan 17/08/2013
                {
                    loadstafspread();//=========================================================function for load spread
                }
                FpSpread1.CommandBar.Visible = false;
                //question addition
                sprdnoofchoices.Sheets[0].RowCount = 0;
                sprdnoofchoices.CommandBar.Visible = false;
                sprdnoofchoices.Sheets[0].SheetCorner.RowCount = 1;
                sprdnoofchoices.Sheets[0].RowHeader.Visible = false;
                sprdnoofchoices.Sheets[0].ColumnHeader.Visible = false;
                sprdnoofchoices.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                sprdnoofchoices.Sheets[0].ColumnHeader.DefaultStyle = style;
                sprdnoofchoices.Sheets[0].ColumnCount = 3;
                sprdnoofchoices.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                sprdnoofchoices.Sheets[0].DefaultStyle.Font.Bold = false;
                FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                sprdnoofchoices.Sheets[0].Columns[1].CellType = chkcell;
                chkcell.AutoPostBack = true;
                sprdviewdata.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                sprdviewdata.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
                sprdviewdata.Sheets[0].RowCount = 0;
                sprdviewdata.CommandBar.Visible = true;
                sprdviewdata.Sheets[0].AutoPostBack = true;
                sprdviewdata.Sheets[0].SheetCorner.RowCount = 4;
                sprdviewdata.Sheets[0].RowHeader.Visible = false;
                sprdviewdata.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                sprdviewdata.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                sprdviewdata.Sheets[0].DefaultStyle.Font.Bold = false;
                sprdviewdata.Sheets[0].ColumnCount = 4;
                sprdviewdata.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, 4);
                sprdviewdata.Sheets[0].ColumnHeaderSpanModel.Add(1, 0, 1, 2);
                sprdviewdata.Sheets[0].ColumnHeaderSpanModel.Add(2, 0, 1, 2);
                sprdviewdata.Sheets[0].ColumnHeaderSpanModel.Add(1, 2, 1, 2);
                sprdviewdata.Sheets[0].ColumnHeaderSpanModel.Add(2, 2, 1, 2);
                sprdviewdata.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Questions Addition Report";
                sprdviewdata.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                sprdviewdata.Sheets[0].ColumnHeader.Rows[0].Border.BorderColorBottom = Color.White;
                sprdviewdata.Sheets[0].ColumnHeader.Rows[3].BackColor = Color.AliceBlue;
                sprdviewdata.Sheets[0].ColumnHeader.Rows[3].HorizontalAlign = HorizontalAlign.Center;
                sprdviewdata.Sheets[0].ColumnHeader.Cells[3, 0].Text = "S.No";
                sprdviewdata.Sheets[0].ColumnHeader.Cells[3, 1].Text = "Questions";
                sprdviewdata.Sheets[0].ColumnHeader.Cells[3, 2].Text = "Answers";
                sprdviewdata.Sheets[0].ColumnHeader.Cells[3, 3].Text = "Correct Answer";
                sprdviewdata.Sheets[0].Columns[0].Locked = true;
                sprdviewdata.Sheets[0].Columns[1].Locked = true;
                sprdviewdata.Sheets[0].Columns[2].Locked = true;
                sprdviewdata.Sheets[0].Columns[3].Locked = true;
                sprdviewdata.Sheets[0].Columns[0].Width = 80;
                sprdviewdata.Sheets[0].Columns[1].Width = 300;
                sprdviewdata.Sheets[0].Columns[2].Width = 200;
                sprdviewdata.Sheets[0].Columns[3].Width = 100;
                sprdviewdata.Sheets[0].Columns[0].Visible = true;
                sprdviewdata.Sheets[0].Columns[1].Visible = true;
                sprdviewdata.Sheets[0].Columns[2].Visible = true;
                sprdviewdata.Sheets[0].Columns[3].Visible = true;
                sprdviewdata.Sheets[0].BackColor = Color.White;
                sprdviewdata.Width = 680;
                loadreason();
                ddlreason.Attributes.Add("onfocus", "reason()");
                pnotesuploadadd.Visible = false;
            }
            FpSpread2.SaveChanges();
        }
        catch (Exception ex)
        {
        }
    }

    protected void Buttongo_Click(object sender, EventArgs e)
    {
        //lblmanysubject.Visible = true ;//--------14/6/12 PRABHA
        //ddlselectmanysub.Visible = true ;//--------14/6/12 PRABHA
        FpSpread2.Visible = false;
        Buttonsave.Enabled = true;
        Buttonupdate.Enabled = false;
        loadstafspread();
        FpSpread1.Sheets[0].Columns.Default.Width = 300;
        lbl_alert.Visible = false;
        pHeaderatendence.Visible = false;
        pBodyatendence.Visible = false;
        pHeaderlesson.Visible = false;
        pBodylesson.Visible = false;
        headerpanelnotes.Visible = false;
        pBodynotes.Visible = false;
        headerADDQuestion.Visible = false;
        pBodyaddquestion.Visible = false;
        headerquestionaddition.Visible = false;
        pBodyquestionaddition.Visible = false;
        btnsliplist.Visible = false;
    }

    public void loadstafspread()
    {
        try
        {
            Labelstaf.Visible = false;
            string sql_s = string.Empty;
            string Strsql = string.Empty;
            string SqlBatchYear = string.Empty;
            string SqlPrefinal1 = string.Empty;
            string SqlPrefinal2 = string.Empty;
            string SqlPrefinal3 = string.Empty;
            string SqlPrefinal4 = string.Empty;
            DataSet dsgetvalue = new DataSet();
            string getquery = string.Empty;
            string SqlFinal = string.Empty;
            string SqlFinal1 = string.Empty;
            string sql1 = string.Empty;
            string tmp_varstr = string.Empty;
            string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            lbl_alert.Visible = false;
            lblmanysubject.Visible = false;
            ddlselectmanysub.Visible = false;
            DataSet dsalterperiod = new DataSet();
            Hashtable hatsublab = new Hashtable();
            string check_lab = string.Empty;
            DataSet dsstuatt = new DataSet();
            Hashtable hatvalue = new Hashtable();
            string sectionsvalue = string.Empty;
            string sectionvar = string.Empty;
            string date1;
            string date2;
            string datefrom;
            string dateto;
            string sqlstr = string.Empty;
            int noofhrs = 0;
            sprdnoofchoices.Sheets[0].RowCount = 0;
            sprdviewdata.Sheets[0].RowCount = 0;
            //=============================================================================================================
            string vari = string.Empty;
            ht_sch.Clear();
            // string sql_stringvar = "select distinct p.degree_code,p.semester,p.SchOrder,nodays from periodattndschedule p,registration r where r.degree_code=p.degree_code  and r.current_semester=p.semester";
            // ds_attndmaster = da.select_method(sql_stringvar, hat, "Text");
            hat.Clear();
            hat.Add("college_code", Session["collegecode"].ToString());
            string sql_stringvar = "sp_select_details_staff";
            ds_attndmaster.Dispose();
            ds_attndmaster.Reset();
            ds_attndmaster = da.select_method(sql_stringvar, hat, "sp");
            if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
            {
                for (int pcont = 0; pcont < ds_attndmaster.Tables[0].Rows.Count; pcont++)
                {
                    degree_var = Convert.ToString(ds_attndmaster.Tables[0].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[0].Rows[pcont]["semester"]);
                    if (!ht_sch.Contains(Convert.ToString(degree_var)))
                    {
                        vari = ds_attndmaster.Tables[0].Rows[pcont]["SchOrder"] + "," + ds_attndmaster.Tables[0].Rows[pcont]["nodays"];
                        ht_sch.Add(degree_var, Convert.ToString(vari));
                    }
                }
            }
            ht_sdate.Clear();
            //sql_stringvar = "select distinct s.batch_year,s.degree_code,s.semester,CONVERT(VARCHAR(10),s.start_date,23)as sdate,starting_dayorder from seminfo s,registration r where r.degree_code=s.degree_code  and r.current_semester=s.semester and r.batch_year=s.batch_year";
            //ds_attndmaster = da.select_method(sql_stringvar, hat, "Text");
            if (ds_attndmaster.Tables.Count > 1 && ds_attndmaster.Tables[1].Rows.Count > 0)
            {
                for (int pcont = 0; pcont < ds_attndmaster.Tables[1].Rows.Count; pcont++)
                {
                    degree_var = Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["batch_year"]) + "-" + Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["semester"]);
                    if (!ht_sdate.Contains(Convert.ToString(degree_var)))
                    {
                        vari = ds_attndmaster.Tables[1].Rows[pcont]["sdate"] + "," + ds_attndmaster.Tables[1].Rows[pcont]["starting_dayorder"];
                        ht_sdate.Add(degree_var, Convert.ToString(vari));
                    }
                }
            }
            ht_bell.Clear();
            //sql_stringvar = "select distinct b.batch_year, b.degree_code,b.semester,b.period1,LTRIM(RIGHT(CONVERT(VARCHAR(20), b.start_time, 100), 7))as start_time ,LTRIM(RIGHT(CONVERT(VARCHAR(20), b.end_time, 100), 7))as end_time  from BellSchedule b,degree d where  b.degree_code=d.degree_code and b.batch_year is not null and d.college_code=" + Session["collegecode"].ToString() + " order by b.batch_year, b.degree_code,b.semester,b.period1";
            //ds_attndmaster = da.select_method(sql_stringvar, hat, "Text");
            if (ds_attndmaster.Tables.Count > 2 && ds_attndmaster.Tables[2].Rows.Count > 0)
            {
                for (int pcont = 0; pcont < ds_attndmaster.Tables[2].Rows.Count; pcont++)
                {
                    degree_var = Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["batch_year"]) + "-" + Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["semester"]) + "-" + Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["period1"]);
                    if (!ht_bell.Contains(Convert.ToString(degree_var)))
                    {
                        vari = ds_attndmaster.Tables[2].Rows[pcont]["start_time"] + "," + ds_attndmaster.Tables[2].Rows[pcont]["end_time"];
                        ht_bell.Add(degree_var, Convert.ToString(vari));
                    }
                }
            }
            ht_period.Clear();
            //sql_stringvar = "select * from attendance_hrlock where college_code=" + Session["collegecode"].ToString() + " order by lock_hr";
            //ds_attndmaster = da.select_method(sql_stringvar, hat, "Text");
            if (ds_attndmaster.Tables.Count > 3 && ds_attndmaster.Tables[3].Rows.Count > 0)
            {
                for (int pcont = 0; pcont < ds_attndmaster.Tables[3].Rows.Count; pcont++)
                {
                    degree_var = Convert.ToString(ds_attndmaster.Tables[3].Rows[pcont]["lock_hr"]);
                    if (!ht_period.Contains(Convert.ToString(degree_var)))
                    {
                        vari = ds_attndmaster.Tables[3].Rows[pcont]["markatt_from"] + "," + ds_attndmaster.Tables[3].Rows[pcont]["markatt_to"];
                        ht_period.Add(degree_var, Convert.ToString(vari));
                    }
                }
            }
            hr_lock = false;
            if (ds_attndmaster.Tables.Count > 4 && ds_attndmaster.Tables[4].Rows.Count > 0)
            {
                string locktrue = ds_attndmaster.Tables[4].Rows[0]["hrlock"].ToString();
                if (locktrue == "1")
                {
                    hr_lock = true;
                }
            }
            //sql_stringvar = "select isnull(hrlock,0) as hrlock from collinfo where college_code=" + Session["collegecode"].ToString() + " ";
            //string locktrue = da.GetFunction(sql_stringvar);
            //if (locktrue == "1")
            //{
            //    hr_lock = true;
            //}
            string degreename = string.Empty;
            //string strdegrename = "select distinct case c.Course_Name when '-1' then ' ' else c.Course_Name end as course ,de.dept_acronym,d.Degree_Code from Degree d,course c,Department de ,Registration r where de.Dept_Code=d.Dept_Code and c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code";
            //DataSet dsdegreename = da.select_method(strdegrename, hat, "Text");
            Hashtable hatdegreename = new Hashtable();
            if (ds_attndmaster.Tables.Count > 5 && ds_attndmaster.Tables[5].Rows.Count > 0)
            {
                for (int i = 0; i < ds_attndmaster.Tables[5].Rows.Count; i++)
                {
                    if (!hatdegreename.Contains(ds_attndmaster.Tables[5].Rows[i]["Degree_Code"].ToString()))
                    {
                        hatdegreename.Add(ds_attndmaster.Tables[5].Rows[i]["Degree_Code"].ToString(), ds_attndmaster.Tables[5].Rows[i]["course"].ToString() + '-' + ds_attndmaster.Tables[5].Rows[i]["dept_acronym"].ToString());
                    }
                }
            }
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Columns.Default.Font.Name = "Book Antiqua";
            FpSpread1.Columns.Default.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].AutoPostBack = true;
            date1 = tbfdate.Text.ToString();
            string[] split = date1.Split(new Char[] { '-' });
            datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
            date2 = tbtodate.Text.ToString();
            string[] split1 = date2.Split(new Char[] { '-' });
            dateto = split1[1].ToString() + "-" + split1[0].ToString() + "-" + split1[2].ToString();
            string ddf = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
            string ddt = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
            if (Convert.ToInt16(split[0].ToString()) <= 31 && Convert.ToInt16(split[1].ToString()) <= 12 && Convert.ToInt16(split[2].ToString()) <= Convert.ToInt16(Session["curr_year"]))
            {
                if (Convert.ToInt16(split1[0].ToString()) <= 31 && Convert.ToInt16(split1[1].ToString()) <= 12 && Convert.ToInt16(split1[2].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                {
                    long days = -1;
                    DateTime dt1 = DateTime.Now.AddDays(-6);
                    DateTime dt2 = DateTime.Now;
                    try
                    {
                        dt1 = Convert.ToDateTime(ddf);
                        dt2 = Convert.ToDateTime(ddt);
                        TimeSpan t = dt2.Subtract(dt1);
                        days = t.Days;
                    }
                    catch
                    {
                        try
                        {
                            dt1 = Convert.ToDateTime(date1);
                            dt2 = Convert.ToDateTime(date2);
                            TimeSpan t = dt2.Subtract(dt1);
                            days = t.Days;
                        }
                        catch
                        {
                            Labelstaf.Text = ddf + ddt;
                        }
                    }
                    if (days < 0)
                    {
                        Labelstaf.Visible = true;
                        FpSpread1.Visible = false;
                        FpSpread2.Visible = false;
                        pHeaderatendence.Visible = false;
                        pHeaderlesson.Visible = false;
                        headerpanelnotes.Visible = false;
                        headerADDQuestion.Visible = false;
                        headerquestionaddition.Visible = false;
                        pBodyatendence.Visible = false;
                        pBodylesson.Visible = false;
                        headerpanelnotes.Visible = false;
                        pBodynotes.Visible = false;
                        pBodyquestionaddition.Visible = false;
                        return;
                    }
                    if (days >= 0)
                    {
                        //load_attnd_spread();
                        Labelstaf.Visible = false;
                        FpSpread1.Visible = true;
                        FpSpread2.Visible = false;
                        pHeaderatendence.Visible = false;
                        pHeaderlesson.Visible = false;
                        headerpanelnotes.Visible = false;
                        pBodyatendence.Visible = false;
                        pBodylesson.Visible = false;
                        pBodynotes.Visible = false;
                        headerADDQuestion.Visible = false;
                        headerquestionaddition.Visible = false;
                        pBodyquestionaddition.Visible = false;
                        string[] differdays = new string[days];
                        //sqlstr = da.GetFunction("select max(No_of_hrs_per_day) from PeriodAttndSchedule");
                        noofhrs = 0;
                        if (ds_attndmaster.Tables.Count > 6 && ds_attndmaster.Tables[6].Rows.Count > 0)
                        {
                            if (ds_attndmaster.Tables[6].Rows[0]["noofhours"].ToString().Trim() != "" && ds_attndmaster.Tables[6].Rows[0]["noofhours"].ToString().Trim() != null && ds_attndmaster.Tables[6].Rows[0]["noofhours"].ToString().Trim() != "0")
                            {
                                noofhrs = Convert.ToInt32(ds_attndmaster.Tables[6].Rows[0]["noofhours"].ToString());
                            }
                        }
                        if (noofhrs != 0)
                        {
                            for (int i = 1; i <= noofhrs; i++)
                            {
                                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Period " + Convert.ToString(i);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = i.ToString();
                                //=======Start=========Added by Manikandan 14/08/2013===================
                                if ((Session["StafforAdmin"] == "Staff") || (Session["StafforAdmin"] == "Admin"))
                                //if (scheduleorattnd == 1)
                                {
                                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Locked = true;
                                }
                                //======================End=============================================
                            }
                            //aruna 13dec2012==============================================================================
                            for (int row_inc = 0; row_inc <= days; row_inc++)
                            {
                                FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                                FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = dt2.AddDays(-row_inc).ToString("d-MM-yyyy");
                            }
                            sql1 = string.Empty;
                            Strsql = string.Empty;
                            SqlFinal = string.Empty;
                            //Start===========Added by Manikandan 14/08/2013===============
                            string stafcode = string.Empty;
                            if ((Session["StafforAdmin"] == "Staff") || (Session["StafforAdmin"] == "Admin"))
                            {
                                ck_append.Visible = false;
                                btnsliplist.Visible = false;
                            }
                            else
                            {
                                ck_append.Visible = true;
                                btnsliplist.Visible = true;
                                //scheduleorattnd = 2;
                            }
                            if (Session["StafforAdmin"] == "Admin")
                            {
                                string stafnamecode = scodetxt.SelectedItem.ToString();
                                //string[] splitcode = stafnamecode.Split(new char[] { '-' });
                                // stafcode = splitcode[1];
                                stafcode = scodetxt.SelectedValue.ToString();
                            }
                            else
                            {
                                stafcode = Session["Staff_Code"].ToString();
                            }
                            //============================End==============================
                            for (int day_lp = 0; day_lp < 7; day_lp++)
                            {
                                strday = Days[day_lp].ToString();
                                sql1 = sql1 + "(";
                                tmp_varstr = string.Empty;
                                for (int i_loop = 1; i_loop <= noofhrs; i_loop++)
                                {
                                    Strsql = Strsql + strday + Convert.ToString(i_loop) + ",";
                                    if (tmp_varstr == "")
                                    {
                                        tmp_varstr = tmp_varstr + strday + Convert.ToString(i_loop) + " like '%" + stafcode + "%'";//Modified by Manikandan 14/08/2013 from above comment line
                                    }
                                    else
                                    {
                                        tmp_varstr = tmp_varstr + " or " + strday + Convert.ToString(i_loop) + " like '%" + stafcode + "%'";//Modified by Manikandan 14/08/2013 from above comment line
                                    }
                                }
                                if (day_lp != 6)
                                    tmp_varstr = tmp_varstr + ") or ";
                                else
                                    tmp_varstr = tmp_varstr + ")";
                                sql1 = sql1 + tmp_varstr.ToString();
                            }
                            SqlPrefinal1 = string.Empty;
                            SqlPrefinal2 = string.Empty;
                            SqlPrefinal3 = string.Empty;
                            SqlPrefinal4 = string.Empty;
                            sql_s = "select semester_schedule.degree_code,(select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=semester_schedule.degree_code and semester=semester_schedule.semester), ";
                            sql_s = sql_s + Strsql + "";
                            SqlBatchYear = "(select distinct(registration.batch_year) from registration,semester_schedule where registration.degree_code=semester_schedule.degree_code and registration.cc=0 and delflag=0 and registration.exam_flag<>'DEBAR' AND registration.current_Semester = semester_schedule.semester)";
                            SqlPrefinal1 = sql_s + " semester,sections,batch_year,FromDate from semester_schedule where lastrec=1 and batch_year in " + SqlBatchYear + " and " + sql1 + " and semester=1 and degree_code in (select degree_code from degree where Exam_System= 'Semester' and First_Year_Nonsemester<>0)";
                            SqlPrefinal2 = sql_s + " semester,sections,batch_year,FromDate from semester_schedule where lastrec=1 and batch_year in " + SqlBatchYear + " and " + sql1 + " and degree_code in (select degree_code from degree where Exam_System='Semester' and First_Year_Nonsemester=0)";
                            SqlPrefinal3 = sql_s + " semester,sections,batch_year,FromDate from semester_schedule where lastrec=1 and  batch_year in " + SqlBatchYear + " and " + sql1 + " and degree_code in (select degree_code from degree where Exam_System='Non Semester') ";
                            SqlPrefinal4 = sql_s + " semester,sections,batch_year,FromDate from semester_schedule where lastrec=1 and  batch_year in " + SqlBatchYear + " and " + sql1 + " and semester<>1 and semester<>-1 and degree_code in (select degree_code from degree where Exam_System= 'Semester' and First_Year_Nonsemester<>0)";
                            SqlFinal = "(" + SqlPrefinal1 + ") union all (" + SqlPrefinal4 + ") union all (" + SqlPrefinal2 + ") union all (" + SqlPrefinal3 + ")";
                            SqlFinal = SqlFinal + " order by batch_year,degree_code,semester,sections,FromDate";
                            //Start Added By Aruna on 13feb2013=====================================================================
                            SqlFinal = string.Empty;
                            //  SqlFinal = " select distinct  r.degree_code,r.batch_year,s.semester,r.sections,s.fromdate,";
                            SqlFinal = " select distinct  r.degree_code,r.batch_year,s.semester,r.sections ,";
                            //  SqlFinal = SqlFinal + Strsql;
                            SqlFinal = SqlFinal + " (select distinct  (c.course_name+'-'+ dt.dept_acronym) from degree d,course c,department dt where d.course_id=c.course_id and d.dept_code=dt.dept_code and degree_code=s.degree_code) as degree";
                            SqlFinal = SqlFinal + ", (select distinct si.end_date from seminfo si where si.degree_code=s.degree_code and si.batch_year=s.batch_year and si.semester=s.semester) as end_date";
                            SqlFinal = SqlFinal + " from semester_schedule s,registration r where s.semester=r.current_semester and s.batch_year=r.batch_year and s.degree_code=r.degree_code and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' and s.sections=r.sections and ";
                            SqlFinal = SqlFinal + "(" + sql1 + ")";
                            //SqlFinal = SqlFinal + " and FromDate in (select top 1 FromDate from semester_schedule where degree_code =r.degree_code  and semester = s.semester  and batch_year = r.batch_year and FromDate <='" + datefrom + "'  order by FromDate Desc)";
                            //  SqlFinal = SqlFinal + " order by r.degree_code,r.batch_year,s.semester,r.sections,FromDate";
                            SqlFinal = SqlFinal + " order by r.degree_code,r.batch_year,s.semester,r.sections";
                            //End==================================================================================================
                            //STart Srinath 15/4/2014==================
                            //STart subburaj 19/8/2014==================
                            SqlFinal = " select distinct r.Batch_Year,r.degree_code,sy.semester,r.Sections,si.end_date from staff_selector ss,Registration r,";
                            SqlFinal = SqlFinal + " subject s,sub_sem sm,syllabus_master sy,seminfo si where sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code";
                            SqlFinal = SqlFinal + " and sy.semester=r.Current_Semester and sy.syll_code=sm.syll_code and sm.subType_no=s.subType_no ";
                            SqlFinal = SqlFinal + " and s.subject_no=ss.subject_no and isnull(r.sections,'')=isnull(ss.sections,'') and ss.batch_year=r.Batch_Year";
                            SqlFinal = SqlFinal + " and si.Batch_Year=r.Batch_Year and si.degree_code=r.degree_code and si.semester=r.Current_Semester and ";
                            SqlFinal = SqlFinal + " si.Batch_Year=sy.Batch_Year and sy.degree_code=r.degree_code and si.semester=sy.Semester and r.CC=0 and r.Exam_Flag<>'debar'";
                            SqlFinal = SqlFinal + " and r.DelFlag=0 and ss.staff_code='" + stafcode + "'";
                            //==========================End====================
                            DataView dvalternaet = new DataView();
                            DataView dvsemster = new DataView();
                            DataView dvholiday = new DataView();
                            DataView dvdaily = new DataView();
                            DataView dvsubject = new DataView();
                            DataView dvsublab = new DataView();
                            // remove collegecode by srinath // 02-09-2014
                            string getalldetails = "select * from Alternate_Schedule where FromDate between '" + ddf + "' and '" + ddt + "' ; ";
                            getalldetails = getalldetails + "select * from Semester_Schedule order by FromDate desc; ";
                            getalldetails = getalldetails + "Select * from holidaystudents where holiday_date between '" + ddf + "' and '" + ddt + "'  ";//01.03.17 barath and isnull(Not_include_dayorder,0)<>'1'
                            getalldetails = getalldetails + "select * from dailyentdet de,dailystaffentry ds where de.lp_code=ds.lp_code and ds.sch_date between '" + ddf + "' and '" + ddt + "'  ; ";
                            getalldetails = getalldetails + " select distinct s.subject_no,s.subject_name,s.subject_code,sy.Batch_Year,sy.degree_code,sy.semester,sm.Lab from syllabus_master sy,sub_sem sm,subject s,staff_selector ss where sy.syll_code=sm.syll_code and sy.syll_code=s.syll_code and sm.syll_code=s.syll_code and sm.subType_no=s.subType_no and ss.subject_no=s.subject_no and ss.batch_year=sy.Batch_Year and ss.staff_code='" + stafcode + "' order by sy.Batch_Year,sy.degree_code,sy.semester ;";
                            getalldetails = getalldetails + " select distinct Current_Semester,Batch_Year,degree_code from Registration where cc=0 and delflag=0 and exam_flag<>'debar'; ";
                            getalldetails = getalldetails + " select no_of_hrs_I_half_day as mor,no_of_hrs_I_half_day as eve,degree_code,semester from periodattndschedule";
                            getalldetails = getalldetails + " select * from tbl_consider_day_order";
                            getalldetails = getalldetails + " select distinct r.Batch_Year,r.degree_code,r.Current_Semester,r.Sections,s.TTName,l.Day_Value,l.Hour_Value,l.Timetablename,l.auto_switch,COUNT(distinct l.Stu_Batch) as no_of_batch  from LabAlloc l,Registration r,Semester_Schedule s where r.Batch_Year=s.batch_year and r.degree_code=s.degree_code and r.Current_Semester=s.semester and r.Sections=s.Sections and r.Batch_Year=l.Batch_Year and r.degree_code=l.degree_code and r.Current_Semester=l.Semester and r.Sections=s.Sections and s.Batch_Year=l.Batch_Year and s.degree_code=l.degree_code and s.Semester=l.Semester and s.Sections=s.Sections and l.Timetablename=s.TTName and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and s.FromDate<='" + ddt + "' and l.auto_switch<>'' group by r.Batch_Year,r.degree_code,r.Current_Semester,r.Sections,s.TTName,l.Day_Value,l.Hour_Value,l.Timetablename,l.auto_switch order by r.Batch_Year,r.degree_code,r.Current_Semester,r.Sections";
                            DataSet dsall = da.select_method_wo_parameter(getalldetails, "Text");
                            //**************added By Srinath 29Jan2015
                            string strstaffselector = string.Empty;
                            Hashtable hatholiday = new Hashtable();
                            DataSet dsperiod = da.select_method(SqlFinal, hat, "Text");
                            if (dsperiod.Tables.Count > 0 && dsperiod.Tables[0].Rows.Count > 0)
                            {
                                for (int pre = 0; pre < dsperiod.Tables[0].Rows.Count; pre++)
                                {
                                    cur_camprevar = Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]);
                                    string getdate = string.Empty;
                                    if (Convert.ToString(tmp_camprevar.Trim()) != Convert.ToString(cur_camprevar.Trim()))
                                    {
                                        strsction = string.Empty;
                                        if ((Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "") && (Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "-1"))
                                        {
                                            strsction = " and sections='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                        }
                                        DataSet dsgetsub = da.select_method_wo_parameter("select distinct s.subject_no,s.subject_name,s.subject_code,sy.Batch_Year,sy.degree_code,sy.semester,sm.Lab from syllabus_master sy,sub_sem sm,subject s where sy.syll_code=sm.syll_code and sy.syll_code=s.syll_code and sm.syll_code=s.syll_code and sm.subType_no=s.subType_no and sy.degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and sy.Batch_Year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and sy.semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' order by sy.Batch_Year,sy.degree_code,sy.semester ", "Text");
                                        DataView dtcurlab = new DataView();
                                        if (dsgetsub.Tables.Count > 0)
                                        {
                                            dsgetsub.Tables[0].DefaultView.RowFilter = " degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"] + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                            dtcurlab = dsgetsub.Tables[0].DefaultView;
                                        }
                                        Hashtable hatcurlab = new Hashtable();
                                        for (int cula = 0; cula < dtcurlab.Count; cula++)
                                        {
                                            string lasubno = dtcurlab[cula]["subject_no"].ToString();
                                            string labhour = dtcurlab[cula]["lab"].ToString();
                                            if (labhour.Trim() == "1" || labhour.Trim().ToLower() == "true")
                                            {
                                                if (!hatcurlab.Contains(lasubno))
                                                {
                                                    hatcurlab.Add(lasubno, lasubno);
                                                }
                                            }
                                        }
                                        string strsubstucount = " select count(distinct r.Roll_No) as stucount,r.Batch_Year,r.degree_code,r.Current_Semester,r.Sections,s.subject_no,r.adm_date from registration r,subjectchooser s where  r.roll_no=s.roll_no and  r.current_semester=s.semester";
                                        strsubstucount = strsubstucount + " and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"] + "' and  degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "'  and current_semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'  and cc=0 and delflag=0 and exam_flag<>'debar' " + strsction + " " + strstaffselector + "  group by r.Batch_Year,r.degree_code,r.Current_Semester,r.Sections,s.subject_no,r.adm_date";
                                        DataSet dssubstucount = da.select_method_wo_parameter(strsubstucount, "Text");
                                        DataView dvsubstucount = new DataView();
                                        hatholiday.Clear();
                                        DataView duholiday = new DataView();
                                        if (dsall.Tables.Count > 2)
                                        {
                                            dsall.Tables[2].DefaultView.RowFilter = " degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " ";
                                            duholiday = dsall.Tables[2].DefaultView;
                                        }
                                        for (int i = 0; i < duholiday.Count; i++)
                                        {
                                            if (!hatholiday.Contains(duholiday[i]["holiday_date"].ToString()))
                                            {
                                                hatholiday.Add(duholiday[i]["holiday_date"].ToString(), duholiday[i]["holiday_desc"].ToString());
                                            }
                                        }
                                        int frshlf = 0;
                                        int schlf = 0;
                                        DataView dvperiod = new DataView();
                                        if (dsall.Tables.Count > 6)
                                        {
                                            dsall.Tables[6].DefaultView.RowFilter = " degree_code ='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and  semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                            dvperiod = dsall.Tables[6].DefaultView;
                                        }
                                        if (dvperiod.Count > 0)
                                        {
                                            string morhr = dvperiod[0]["mor"].ToString();
                                            string evehr = dvperiod[0]["mor"].ToString();
                                            if (morhr != null && morhr.Trim() != "")
                                            {
                                                frshlf = Convert.ToInt32(morhr);
                                            }
                                            if (evehr != null && evehr.Trim() != "")
                                            {
                                                schlf = Convert.ToInt32(evehr);
                                            }
                                        }
                                        string getcurrent_sem = string.Empty;
                                        DataView dvcurrsem = new DataView();
                                        if (dsall.Tables.Count > 5)
                                        {
                                            dsall.Tables[5].DefaultView.RowFilter = "degree_code ='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "'  and batch_year = '" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "'";
                                            dvcurrsem = dsall.Tables[5].DefaultView;
                                        }
                                        if (dvcurrsem.Count > 0)
                                        {
                                            getcurrent_sem = dvcurrsem[0]["current_semester"].ToString();
                                        }
                                        if (Convert.ToString(getcurrent_sem) == Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]))
                                        {
                                            string semenddate = dsperiod.Tables[0].Rows[pre]["end_date"].ToString();
                                            string altersetion = string.Empty;
                                            if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() != "-1" && dsperiod.Tables[0].Rows[pre]["sections"].ToString() != null && dsperiod.Tables[0].Rows[pre]["sections"].ToString().Trim() != "")
                                            {
                                                altersetion = "and Sections='" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "'";
                                            }
                                            //===============================Start==============================================================
                                            Hashtable hatdc = new Hashtable();
                                            Dictionary<DateTime, byte> dicAlternateDayOrder = new Dictionary<DateTime, byte>();
                                            DataView dvdayorderchanged = new DataView();
                                            if (dsall.Tables.Count > 7)
                                            {
                                                dsall.Tables[7].DefaultView.RowFilter = "degree_code ='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "'  and batch_year = '" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "  ";
                                                dvdayorderchanged = dsall.Tables[7].DefaultView;
                                            }
                                            for (int dc = 0; dc < dvdayorderchanged.Count; dc++)
                                            {
                                                DateTime dtdcf = Convert.ToDateTime(dvdayorderchanged[dc]["from_date"].ToString());
                                                DateTime dtdct = Convert.ToDateTime(dvdayorderchanged[dc]["to_date"].ToString());
                                                string alternateDayOrder = Convert.ToString(dvdayorderchanged[dc]["DayOrder"]).Trim();
                                                byte alternateDay = 0;
                                                byte.TryParse(alternateDayOrder, out alternateDay);
                                                for (DateTime dtc = dtdcf; dtc <= dtdct; dtc = dtc.AddDays(1))
                                                {
                                                    if (!hatdc.Contains(dtc))
                                                    {
                                                        hatdc.Add(dtc, dtc);
                                                    }
                                                    if (!dicAlternateDayOrder.ContainsKey(dtc))
                                                    {
                                                        dicAlternateDayOrder.Add(dtc, alternateDay);
                                                    }
                                                }
                                            }
                                            //=================================End==============================================================
                                            Session["StaffSelector"] = "0";
                                            strstaffselector = string.Empty;   //" + Session["collegecode"].ToString() + "
                                            string minimumabsentsms = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Session["collegecode"].ToString() + "'");
                                            string[] splitminimumabsentsms = minimumabsentsms.Split('-');
                                            if (splitminimumabsentsms.Length == 2)
                                            {
                                                int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
                                                if (splitminimumabsentsms[0].ToString() == "1")
                                                {
                                                    if (Convert.ToInt32(dsperiod.Tables[0].Rows[pre]["batch_year"].ToString()) >= batchyearsetting)
                                                    {
                                                        Session["StaffSelector"] = "1";
                                                    }
                                                }
                                            }
                                            if (Session["StaffSelector"].ToString() == "1")
                                            {
                                                strstaffselector = " and s.staffcode='" + Session["Staff_Code"].ToString() + "'";
                                            }
                                            for (int row_inc = 0; row_inc <= days; row_inc++) //Date Loop
                                            {
                                                if (hatdegreename.Contains(dsperiod.Tables[0].Rows[pre]["degree_code"].ToString()))
                                                {
                                                    degreename = GetCorrespondingKey(dsperiod.Tables[0].Rows[pre]["degree_code"].ToString(), hatdegreename).ToString();
                                                }
                                                DateTime cur_day = new DateTime();
                                                cur_day = dt2.AddDays(-row_inc);
                                                if (!hatdc.Contains(cur_day) || (dicAlternateDayOrder.ContainsKey(cur_day) && dicAlternateDayOrder[cur_day] != 0))
                                                {
                                                    tmp_datevalue = Convert.ToString(cur_day);
                                                    degree_var = Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]);
                                                    string SchOrder = string.Empty;
                                                    string day_from = cur_day.ToString("yyyy-MM-dd");
                                                    DateTime schfromdate = cur_day;
                                                    if (dsall.Tables.Count > 1)
                                                    {
                                                        dsall.Tables[1].DefaultView.RowFilter = "batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and FromDate<='" + cur_day.ToString() + "'";
                                                        dvsemster = dsall.Tables[1].DefaultView;
                                                    }
                                                    if (dvsemster.Count > 0)
                                                    {
                                                        getdate = dvsemster[0]["FromDate"].ToString();
                                                    }
                                                    else
                                                    {
                                                        getdate = string.Empty;
                                                    }
                                                    if (Convert.ToString(getdate) != "" && Convert.ToString(getdate).Trim() != "0" && Convert.ToString(getdate).Trim() != null)
                                                    {
                                                        DateTime getsche = Convert.ToDateTime(getdate);
                                                        if (Convert.ToDateTime(schfromdate) == Convert.ToDateTime(getsche) || Convert.ToDateTime(schfromdate) != Convert.ToDateTime(getsche))
                                                        {
                                                            if (ht_sch.Contains(Convert.ToString(degree_var)))
                                                            {
                                                                string contvar = Convert.ToString(GetCorrespondingKey(Convert.ToString(degree_var), ht_sch));
                                                                string[] sp_rd_semi = contvar.Split(',');
                                                                if (sp_rd_semi.GetUpperBound(0) >= 1)
                                                                {
                                                                    SchOrder = sp_rd_semi[0].ToString();
                                                                    noofdays = sp_rd_semi[1].ToString();
                                                                }
                                                            }
                                                            Dictionary<string, string> dicautoswitch = new Dictionary<string, string>();
                                                            DataView dvautoswitch = new DataView();
                                                            if (dsall.Tables.Count > 8)
                                                            {
                                                                dsall.Tables[8].DefaultView.RowFilter = " batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and Current_Semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and TTName='" + dvsemster[0]["ttname"].ToString() + "'";
                                                                dvautoswitch = dsall.Tables[8].DefaultView;
                                                            }
                                                            for (int au = 0; au < dvautoswitch.Count; au++)
                                                            {
                                                                string autoswi = dvautoswitch[au]["Day_Value"].ToString() + dvautoswitch[au]["Hour_Value"].ToString();
                                                                if (!dicautoswitch.ContainsKey(autoswi))
                                                                {
                                                                    dicautoswitch.Add(autoswi, dvautoswitch[au]["auto_switch"].ToString() + '-' + dvautoswitch[au]["no_of_batch"].ToString());
                                                                }
                                                            }
                                                            degree_var = Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]);
                                                            if (ht_sdate.Contains(Convert.ToString(degree_var)))
                                                            {
                                                                string contvar = Convert.ToString(GetCorrespondingKey(Convert.ToString(degree_var), ht_sdate));
                                                                string[] sp_rd_semi = contvar.Split(',');
                                                                if (sp_rd_semi.GetUpperBound(0) >= 1)
                                                                {
                                                                    start_datesem = sp_rd_semi[0].ToString();
                                                                    start_dayorder = sp_rd_semi[1].ToString();
                                                                }
                                                            }
                                                            if (noofdays.ToString().Trim() == "")
                                                            {
                                                                goto lb1;
                                                            }
                                                            Day_Order = string.Empty;
                                                            if (SchOrder == "1")
                                                            {
                                                                strday = cur_day.ToString("ddd"); //Week Dayorder
                                                                Day_Order = "0-" + Convert.ToString(strday);
                                                                FpSpread1.Sheets[0].RowHeader.Cells[row_inc, 0].Text = cur_day.ToString("d-MM-yyyy") + " (" + cur_day.DayOfWeek.ToString() + ")";
                                                            }
                                                            else
                                                            {
                                                                string[] sps = dt2.ToString().Split('/');
                                                                string curdate = sps[0] + '/' + sps[1] + '/' + sps[2];
                                                                strday = da.findday(cur_day.ToString(), dsperiod.Tables[0].Rows[pre]["degree_code"].ToString(), dsperiod.Tables[0].Rows[pre]["semester"].ToString(), dsperiod.Tables[0].Rows[pre]["batch_year"].ToString(), start_datesem.ToString(), noofdays.ToString(), start_dayorder);
                                                                if (dicAlternateDayOrder.ContainsKey(cur_day))
                                                                {
                                                                    strday = findDayName(dicAlternateDayOrder[cur_day]);
                                                                    Day_Order = Convert.ToString(dicAlternateDayOrder[cur_day]).Trim();
                                                                }
                                                                else
                                                                {
                                                                    if (strday.Trim().ToLower() == "mon")
                                                                        Day_Order = "1";
                                                                    else if (strday.Trim().ToLower() == "tue")
                                                                        Day_Order = "2";
                                                                    else if (strday.Trim().ToLower() == "wed")
                                                                        Day_Order = "3";
                                                                    else if (strday.Trim().ToLower() == "thu")
                                                                        Day_Order = "4";
                                                                    else if (strday.Trim().ToLower() == "fri")
                                                                        Day_Order = "5";
                                                                    else if (strday.Trim().ToLower() == "sat")
                                                                        Day_Order = "6";
                                                                    else if (strday.Trim().ToLower() == "sun")
                                                                        Day_Order = "7";
                                                                }
                                                                Day_Order = Day_Order + "-" + Convert.ToString(strday);
                                                            }
                                                            if (strday.ToString().Trim() == "")
                                                            {
                                                                goto lb1;
                                                            }
                                                            //==check holiday
                                                            string reasonsun = string.Empty;
                                                            if (hatholiday.Contains(cur_day.ToString()))
                                                            {
                                                                reasonsun = GetCorrespondingKey(cur_day.ToString(), hatholiday).ToString();
                                                                if (reasonsun.Trim().ToLower() == "sunday")
                                                                {
                                                                    FpSpread1.Sheets[0].SpanModel.Add((row_inc), 0, 1, (FpSpread1.Sheets[0].ColumnCount));
                                                                    FpSpread1.Sheets[0].Cells[(row_inc), 0].Text = "Sunday Holiday";
                                                                    FpSpread1.Sheets[0].Cells[(row_inc), 0].Tag = "Selected day is Holiday- Reason-" + reasonsun;
                                                                    FpSpread1.Sheets[0].Cells[(row_inc), 0].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[(row_inc), 0].ForeColor = Color.Red;
                                                                    FpSpread1.Sheets[0].Rows[(row_inc)].Locked = true;
                                                                }
                                                            }
                                                            if (!hatholiday.Contains(cur_day.ToString()) || reasonsun.Trim().ToLower() != "sunday")
                                                            {
                                                                string str_day = strday;
                                                                string Atmonth = cur_day.Month.ToString();
                                                                string Atyear = cur_day.Year.ToString();
                                                                long strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                                                                sql1 = string.Empty;
                                                                Strsql = string.Empty;
                                                                for (int i_loop = 1; i_loop <= noofhrs; i_loop++)
                                                                {
                                                                    Strsql = Strsql + strday + Convert.ToString(i_loop) + ",";
                                                                    if (sql1 == "")
                                                                    {
                                                                        sql1 = sql1 + strday + Convert.ToString(i_loop) + " like '%" + stafcode + "%'";//Modified by Manikandan 14/08/2013 from above comment line
                                                                    }
                                                                    else
                                                                    {
                                                                        sql1 = sql1 + " or " + strday + Convert.ToString(i_loop) + " like '%" + stafcode + "%'";//Modified by Manikandan 14/08/2013 from above comment line
                                                                    }
                                                                }
                                                                string day_aten = cur_day.Day.ToString();
                                                                Boolean check_hour = false;
                                                                string strsectionvar = string.Empty;
                                                                string labsection = string.Empty;
                                                                if (Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "" && Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "-1")
                                                                {
                                                                    strsectionvar = " and sections='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                                                    labsection = " and sections='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                                                }
                                                                sql1 = " and (" + sql1 + ")";
                                                                if (dsall.Tables.Count > 0)
                                                                {
                                                                    dsall.Tables[0].DefaultView.RowFilter = "degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and fromdate='" + day_from + "'";
                                                                    dvalternaet = dsall.Tables[0].DefaultView;
                                                                }
                                                                string text_temp = string.Empty;
                                                                int temp = 0;
                                                                text_temp = string.Empty;
                                                                string getcolumnfield = string.Empty;
                                                                string getcolumnfield_alter = string.Empty;
                                                                Boolean moringleav = false;
                                                                Boolean evenleave = false;
                                                                if (dsall.Tables.Count > 2)
                                                                {
                                                                    dsall.Tables[2].DefaultView.RowFilter = "holiday_date='" + cur_day.ToString("MM/dd/yyyy") + "' and degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                                                    dvholiday = dsall.Tables[2].DefaultView;
                                                                }
                                                                if (dvholiday.Count > 0)
                                                                {
                                                                    if (!hatholiday.Contains(cur_day.ToString()))
                                                                    {
                                                                        hatholiday.Add(cur_day.ToString(), dvholiday[0]["holiday_desc"].ToString());
                                                                    }
                                                                    if (dvholiday[0]["morning"].ToString() == "1" || dvholiday[0]["morning"].ToString().Trim().ToLower() == "true")
                                                                    {
                                                                        moringleav = true;
                                                                    }
                                                                    if (dvholiday[0]["evening"].ToString() == "1" || dvholiday[0]["evening"].ToString().Trim().ToLower() == "true")
                                                                    {
                                                                        evenleave = true;
                                                                    }
                                                                    if (dvholiday[0]["halforfull"].ToString() == "0" || dvholiday[0]["halforfull"].ToString().Trim().ToLower() == "false")
                                                                    {
                                                                        evenleave = true;
                                                                        moringleav = true;
                                                                    }
                                                                }
                                                                for (temp = 1; temp <= noofhrs; temp++)
                                                                {
                                                                    try
                                                                    {
                                                                        if (dicautoswitch.ContainsKey(strday + temp))
                                                                        {
                                                                            Boolean altflag = false;
                                                                            if (dvalternaet.Count > 0)
                                                                            {
                                                                                string getva = dvalternaet[0]["" + strday.Trim() + temp + ""].ToString();
                                                                                if (getva.Trim() != "" && getva != null)
                                                                                {
                                                                                    altflag = true;
                                                                                }
                                                                            }
                                                                            if (altflag == false)
                                                                            {
                                                                                string[] autobatch = dicautoswitch[strday + temp].Split('-');
                                                                                if (autobatch.GetUpperBound(0) == 1)
                                                                                {
                                                                                    int batch = Convert.ToInt32(autobatch[1]);
                                                                                    DateTime dts = Convert.ToDateTime(getdate);
                                                                                    DateTime dtnow = cur_day;
                                                                                    TimeSpan ts = dtnow - dts;
                                                                                    int dif_days = ts.Days;
                                                                                    int weekcoun = dif_days / 7;
                                                                                    string[] spsubva = autobatch[0].Split(',');
                                                                                    int counsubj = spsubva.GetUpperBound(0) + 1;
                                                                                    int order = weekcoun % counsubj;
                                                                                    string rsec = string.Empty;
                                                                                    if (altersetion.Trim() != "" && altersetion != null)
                                                                                    {
                                                                                        rsec = dsperiod.Tables[0].Rows[pre]["sections"].ToString();
                                                                                    }
                                                                                    string getstaffcode = string.Empty;
                                                                                    string setalte = string.Empty;
                                                                                    DataSet dsstaff = new DataSet();
                                                                                    if (batch >= 1)
                                                                                    {
                                                                                        for (int b = 0; b < batch; b++)
                                                                                        {
                                                                                            int val = order + b;
                                                                                            int su = val % counsubj;
                                                                                            string subno = spsubva[su].ToString();
                                                                                            string batchset = "B" + (b + 1).ToString();
                                                                                            string getstaffquery = "select distinct staff_code from staff_selector where subject_no='" + subno + "' and Sections='" + rsec + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' ";
                                                                                            dsstaff.Reset();
                                                                                            dsstaff = da.select_method_wo_parameter(getstaffquery, "Text");
                                                                                            getstaffcode = subno;
                                                                                            if (dsstaff.Tables.Count > 0 && dsstaff.Tables[0].Rows.Count > 0)
                                                                                            {
                                                                                                for (int sh = 0; sh < dsstaff.Tables[0].Rows.Count; sh++)
                                                                                                {
                                                                                                    getstaffcode = getstaffcode + '-' + dsstaff.Tables[0].Rows[sh]["staff_code"].ToString();
                                                                                                }
                                                                                            }
                                                                                            getstaffcode = getstaffcode + "-L";
                                                                                            if (setalte == "")
                                                                                            {
                                                                                                setalte = getstaffcode;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                setalte = setalte + ";" + getstaffcode;
                                                                                            }
                                                                                            string strquery = "delete from subjectChooser_New where subject_no='" + subno + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' and fromdate='" + cur_day.ToString() + "' and roll_no in( select roll_no from Registration where  batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and cc=0 and delflag=0 and exam_flag<>'debar' )";
                                                                                            int insert = da.update_method_wo_parameter(strquery, "Text");
                                                                                            strquery = "delete from laballoc_new where  batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and day_value='" + strday + "' and hour_value='" + temp + "' and fdate='" + cur_day.ToString() + "' and subject_no='" + subno + "'";
                                                                                            insert = da.update_method_wo_parameter(strquery, "Text");
                                                                                            strquery = "insert into subjectChooser_New (semester,roll_no,subject_no,subtype_no,Batch,fromdate,todate) ";
                                                                                            strquery = strquery + "(select s.semester,s.roll_no,s.subject_no,s.subtype_no,s.Batch,'" + cur_day.ToString() + "' as fromdate ,'" + cur_day.ToString() + "' as todate from Registration r ,subjectChooser s where r.Roll_No=s.roll_no and r.Current_Semester=s.semester and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'Debar' and s.subject_no='" + subno + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and r.degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and s.semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and s.Batch='" + batchset + "')";
                                                                                            insert = da.update_method_wo_parameter(strquery, "Text");
                                                                                            strquery = "insert into laballoc_new (Batch_Year,Degree_Code,Semester,Sections,Subject_No,Stu_Batch,Day_Value,Hour_Value,fdate,tdate) ";
                                                                                            strquery = strquery + "values('" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "','" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "','" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "','" + rsec + "','" + subno + "','" + batchset + "','" + strday + "','" + temp + "','" + cur_day.ToString() + "','" + cur_day.ToString() + "')";
                                                                                            insert = da.update_method_wo_parameter(strquery, "Text");
                                                                                        }
                                                                                        string strquery1 = "if exists(select * from Alternate_Schedule where batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and FromDate= '" + cur_day.ToString() + "')";
                                                                                        strquery1 = strquery1 + " Update Alternate_Schedule set " + strday + temp + "='" + setalte + "' where batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and FromDate= '" + cur_day.ToString() + "'";
                                                                                        strquery1 = strquery1 + " ELse insert into Alternate_Schedule(batch_year,degree_code,semester,Sections,FromDate," + strday + temp + ") values('" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "','" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "','" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "','" + rsec + "','" + cur_day.ToString() + "','" + setalte + "')";
                                                                                        int insert1 = da.update_method_wo_parameter(strquery1, "Text");
                                                                                    }
                                                                                    getstaffcode = "select * from Alternate_Schedule where batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and FromDate= '" + cur_day.ToString() + "'";
                                                                                    dsstaff.Reset();
                                                                                    dsstaff = da.select_method_wo_parameter(getstaffcode, "Text");
                                                                                    dvalternaet = dsstaff.Tables[0].DefaultView;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    catch
                                                                    {
                                                                    }
                                                                    string sp_rd = string.Empty;
                                                                    Boolean altfalg = false;
                                                                    if (dvalternaet.Count > 0)
                                                                    {
                                                                        sp_rd = dvalternaet[0]["" + strday.Trim() + temp + ""].ToString();
                                                                        if (hatdegreename.Contains(dvalternaet[0]["degree_code"].ToString()))
                                                                        {
                                                                            degreename = GetCorrespondingKey(dvalternaet[0]["degree_code"].ToString(), hatdegreename).ToString();
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        sp_rd = string.Empty;
                                                                    }
                                                                    if (sp_rd.Trim() != "" && sp_rd.Trim() != "0" && sp_rd != null)
                                                                    {
                                                                        altfalg = true;
                                                                        string[] sp_rd_split = sp_rd.Split(';');
                                                                        for (int index = 0; index <= sp_rd_split.GetUpperBound(0); index++)
                                                                        {
                                                                            string[] sp2 = sp_rd_split[index].Split(new Char[] { '-' });
                                                                            if (sp2.GetUpperBound(0) >= 1)
                                                                            {
                                                                                int upperbound = sp2.GetUpperBound(0);
                                                                                for (int multi_staff = 1; multi_staff < sp2.GetUpperBound(0); multi_staff++)
                                                                                {
                                                                                    if (sp2[multi_staff] == stafcode)
                                                                                    {
                                                                                        //==============================theroy batch=======================================
                                                                                        Boolean checklabhr = false;
                                                                                        for (int sr = 0; sr <= sp_rd_split.GetUpperBound(0); sr++)
                                                                                        {
                                                                                            string[] getlasub = sp_rd_split[sr].ToString().Split('-');
                                                                                            if (getlasub.GetUpperBound(0) > 1)
                                                                                            {
                                                                                                string srllab = getlasub[0].ToString();
                                                                                                if (hatcurlab.Contains(srllab))
                                                                                                {
                                                                                                    checklabhr = true;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        //======================================================================
                                                                                        string sect = dsperiod.Tables[0].Rows[pre]["sections"].ToString();
                                                                                        if (sect != "-1" && sect != null && sect.Trim() != "")
                                                                                        {
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            sect = string.Empty;
                                                                                        }
                                                                                        if (semenddate.Trim() != "" && semenddate.Trim() != null && semenddate.Trim() != "0")
                                                                                        {
                                                                                            if (cur_day <= (Convert.ToDateTime(semenddate)))
                                                                                            {
                                                                                                check_hour = true;
                                                                                                double Num;
                                                                                                bool isNum = double.TryParse(sp2[0].ToString(), out Num);
                                                                                                if (isNum)
                                                                                                {
                                                                                                    if (checklabhr == false)
                                                                                                    {
                                                                                                        dsall.Tables[4].DefaultView.RowFilter = " subject_no=" + sp2[0] + "";
                                                                                                        dvsubject = dsall.Tables[4].DefaultView;
                                                                                                        if (dvsubject.Count > 0)
                                                                                                        {
                                                                                                            text_temp = dvsubject[0]["subject_name"].ToString() + "-S";
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        dsall.Tables[4].DefaultView.RowFilter = " subject_no=" + sp2[0] + "";
                                                                                                        dvsubject = dsall.Tables[4].DefaultView;
                                                                                                        if (dvsubject.Count > 0)
                                                                                                        {
                                                                                                            text_temp = dvsubject[0]["subject_name"].ToString() + "-L";
                                                                                                        }
                                                                                                    }
                                                                                                    string Schedule_string = string.Empty;
                                                                                                    if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() == "-1" || dsperiod.Tables[0].Rows[pre]["sections"].ToString() == "" || dsperiod.Tables[0].Rows[pre]["sections"].ToString() == null)
                                                                                                    {
                                                                                                        if (checklabhr == false)
                                                                                                        {
                                                                                                            Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-S-0"; //+ sp_rd_semi.GetUpperBound(0);
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-L-0"; //+ sp_rd_semi.GetUpperBound(0);
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        if (checklabhr == false)
                                                                                                        {
                                                                                                            Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-S-0";// +sp_rd_semi.GetUpperBound(0);
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-L-0";// +sp_rd_semi.GetUpperBound(0);
                                                                                                        }
                                                                                                    }
                                                                                                    bool allowleave = false;
                                                                                                    if (hatholiday.Contains(cur_day.ToString()))
                                                                                                    {
                                                                                                        if (moringleav == true)
                                                                                                        {
                                                                                                            if (frshlf >= temp)
                                                                                                            {
                                                                                                                allowleave = true;
                                                                                                            }
                                                                                                        }
                                                                                                        if (evenleave == true)
                                                                                                        {
                                                                                                            if (temp > frshlf)
                                                                                                            {
                                                                                                                allowleave = true;
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                    if (allowleave == true)
                                                                                                    {
                                                                                                        if (hatholiday.Contains(cur_day.ToString()))
                                                                                                        {
                                                                                                            string holidayreason = GetCorrespondingKey(cur_day.ToString(), hatholiday).ToString();
                                                                                                            if (FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Text.Trim() == "")
                                                                                                            {
                                                                                                                FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Text = "Selected day is Holiday- Reason-" + holidayreason + "-" + text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Tag = "Selected day is Holiday- Reason-" + holidayreason + "-" + Schedule_string.ToString() + "-alter";
                                                                                                                if (Convert.ToString(Day_Order).Trim() != "")
                                                                                                                {
                                                                                                                    FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Note = Convert.ToString(Day_Order);
                                                                                                                }
                                                                                                                FpSpread1.Sheets[0].Cells[(row_inc), temp - 1].ForeColor = Color.Blue;
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Text = FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Text + " * " + "Selected day is Holiday- Reason-" + holidayreason + "-" + text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Tag = FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Tag + " * " + "Selected day is Holiday- Reason-" + holidayreason + "-" + Schedule_string.ToString() + "-alter";
                                                                                                                if (Convert.ToString(Day_Order).Trim() != "")
                                                                                                                {
                                                                                                                    FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Note = Convert.ToString(Day_Order);
                                                                                                                }
                                                                                                                FpSpread1.Sheets[0].Cells[(row_inc), temp - 1].ForeColor = Color.Blue;
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        if (FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Text.Trim() == "")
                                                                                                        {
                                                                                                            FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Text = text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                            FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Tag = Schedule_string.ToString() + "-alter";
                                                                                                            if (Convert.ToString(Day_Order).Trim() != "")
                                                                                                            {
                                                                                                                FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Note = Convert.ToString(Day_Order);
                                                                                                            }
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            string tmpvar = string.Empty;
                                                                                                            string istemp = string.Empty;
                                                                                                            istemp = FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Text.ToString();
                                                                                                            tmpvar = text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                            if (Convert.ToString(istemp) != Convert.ToString(tmpvar))
                                                                                                            {
                                                                                                                FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Text = FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Text + " * " + text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + da.GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "") + "-" + da.GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "") + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Tag = FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Tag + " * " + Schedule_string.ToString() + "-alter";
                                                                                                                if (Convert.ToString(Day_Order).Trim() != "")
                                                                                                                {
                                                                                                                    FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Note = Convert.ToString(Day_Order);
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                        if (FpSpread1.Sheets[0].Cells[row_inc, temp - 1].ForeColor != Color.Blue)
                                                                                                        {
                                                                                                            string daystring = dt2.AddDays(-row_inc).ToString("dd");
                                                                                                            string daystring1 = dt2.AddDays(-row_inc).ToString("ddd");
                                                                                                            string Att_dcolumn = "d" + Convert.ToInt16(daystring) + "d" + temp;
                                                                                                            check_lab = string.Empty;
                                                                                                            hatvalue.Clear();
                                                                                                            if (checklabhr == false)
                                                                                                            {
                                                                                                                check_lab = "0";
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                check_lab = "1";
                                                                                                            }
                                                                                                            //if (!hatsublab.Contains(sp2[0]))
                                                                                                            //{
                                                                                                            //    check_lab = da.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + sp2[0] + "'");
                                                                                                            //    hatsublab.Add(sp2[0], check_lab);
                                                                                                            //}
                                                                                                            //else
                                                                                                            //{
                                                                                                            //    check_lab = GetCorrespondingKey(sp2[0], hatsublab).ToString();
                                                                                                            //}
                                                                                                            sectionvar = string.Empty;
                                                                                                            sectionsvalue = string.Empty;
                                                                                                            if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() != "-1" && dsperiod.Tables[0].Rows[pre]["sections"].ToString() != "" && dsperiod.Tables[0].Rows[pre]["sections"].ToString() != null)
                                                                                                            {
                                                                                                                sectionvar = " and sections='" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "'";
                                                                                                                sectionsvalue = dsperiod.Tables[0].Rows[pre]["sections"].ToString();
                                                                                                            }
                                                                                                            if (check_lab == "1" || check_lab.Trim().ToLower() == "true")
                                                                                                            {
                                                                                                                hatvalue.Clear();
                                                                                                                hatvalue.Add("batch_year", dsperiod.Tables[0].Rows[pre]["batch_year"].ToString());
                                                                                                                hatvalue.Add("degree_code", dsperiod.Tables[0].Rows[pre]["degree_code"].ToString());
                                                                                                                hatvalue.Add("sem", dsperiod.Tables[0].Rows[pre]["semester"].ToString());
                                                                                                                hatvalue.Add("sections", sectionsvalue);
                                                                                                                hatvalue.Add("month_year", strdate);
                                                                                                                hatvalue.Add("date", cur_day);
                                                                                                                hatvalue.Add("subject_no", sp2[0]);
                                                                                                                hatvalue.Add("day", strday);
                                                                                                                hatvalue.Add("hour", temp);
                                                                                                                dsstuatt.Reset();
                                                                                                                dsstuatt.Dispose();
                                                                                                                dsstuatt = da.select_method("sp_stu_atten_month_check_lab_alter", hatvalue, "sp");
                                                                                                                if (dsstuatt.Tables.Count > 0 && dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                                {
                                                                                                                    Att_strqueryst = dsstuatt.Tables[0].Rows[0]["stucount"].ToString();
                                                                                                                    if (int.Parse(Att_strqueryst) > 0)
                                                                                                                    {
                                                                                                                        hatvalue.Clear();
                                                                                                                        hatvalue.Add("columnname", Att_dcolumn);
                                                                                                                        hatvalue.Add("batch_year", dsperiod.Tables[0].Rows[pre]["batch_year"].ToString());
                                                                                                                        hatvalue.Add("degree_code", dsperiod.Tables[0].Rows[pre]["degree_code"].ToString());
                                                                                                                        hatvalue.Add("sem", dsperiod.Tables[0].Rows[pre]["semester"].ToString());
                                                                                                                        hatvalue.Add("sections", sectionsvalue);
                                                                                                                        hatvalue.Add("month_year", strdate);
                                                                                                                        hatvalue.Add("date", cur_day);
                                                                                                                        hatvalue.Add("subject_no", sp2[0]);
                                                                                                                        hatvalue.Add("day", strday);
                                                                                                                        hatvalue.Add("hour", temp);
                                                                                                                        dsstuatt.Reset();
                                                                                                                        dsstuatt.Dispose();
                                                                                                                        //  dsstuatt = da.select_method("sp_stu_atten_day_check_lab_alter", hatvalue, "sp");
                                                                                                                        string strgetatt = "select  count(distinct r.Roll_No) as stucount from registration r,attendance a,subjectchooser_new s where degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and current_semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and cc=0 and delflag=0 and  exam_flag<>'debar' and month_year=" + strdate + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and";
                                                                                                                        strgetatt = strgetatt + " r.current_semester=s.semester and subject_no='" + sp2[0].ToString() + "' " + sectionvar + " and(" + Att_dcolumn + " is not null and " + Att_dcolumn + "<>'0' and " + Att_dcolumn + "<>'') and fromdate='" + cur_day + "' and batch    in(select stu_batch from laballoc_new where subject_no='" + sp2[0].ToString() + "'  and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "'  and hour_value='" + temp + "'  and    degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' ";
                                                                                                                        strgetatt = strgetatt + " and day_value='" + strday + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + sectionvar + " and fdate='" + cur_day + "') and adm_date<='" + cur_day + "'";
                                                                                                                        dsstuatt = da.select_method_wo_parameter(strgetatt, "Text");
                                                                                                                        if (dsstuatt.Tables.Count > 0 && dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                                        {
                                                                                                                            if (Att_strqueryst == dsstuatt.Tables[0].Rows[0]["stucount"].ToString())
                                                                                                                            {
                                                                                                                                Att_strqueryst = "0";
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                Att_strqueryst = "1";
                                                                                                                            }
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            Att_strqueryst = "1";
                                                                                                                        }
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        Att_strqueryst = "1";
                                                                                                                    }
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    Att_strqueryst = "1";
                                                                                                                }
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                hatvalue.Clear();
                                                                                                                hatvalue.Add("batch_year", dsperiod.Tables[0].Rows[pre]["batch_year"].ToString());
                                                                                                                hatvalue.Add("degree_code", dsperiod.Tables[0].Rows[pre]["degree_code"].ToString());
                                                                                                                hatvalue.Add("sem", dsperiod.Tables[0].Rows[pre]["semester"].ToString());
                                                                                                                hatvalue.Add("sections", sectionsvalue);
                                                                                                                hatvalue.Add("month_year", strdate);
                                                                                                                hatvalue.Add("date", cur_day);
                                                                                                                hatvalue.Add("subject_no", sp2[0]);
                                                                                                                dsstuatt.Reset();
                                                                                                                dsstuatt.Dispose();
                                                                                                                // dsstuatt = da.select_method("sp_stu_atten_month_check", hatvalue, "sp");
                                                                                                                dssubstucount.Tables[0].DefaultView.RowFilter = "batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and  degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "'  and current_semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' and subject_no='" + sp2[0] + "' " + strsction + " and adm_date<='" + cur_day.ToString("MM/dd/yyyy").ToString() + "' ";//added admissiondare
                                                                                                                dvsubstucount = dssubstucount.Tables[0].DefaultView;
                                                                                                                if (dvsubstucount.Count > 0)
                                                                                                                {
                                                                                                                    int stustradm = 0;
                                                                                                                    for (int stuadmcou = 0; stuadmcou < dvsubstucount.Count; stuadmcou++)
                                                                                                                    {
                                                                                                                        stustradm = stustradm + Convert.ToInt32(dvsubstucount[stuadmcou]["stucount"]);
                                                                                                                    }
                                                                                                                    Att_strqueryst = stustradm.ToString();
                                                                                                                    // Att_strqueryst = dvsubstucount[0]["stucount"].ToString();
                                                                                                                    if (int.Parse(Att_strqueryst) > 0)
                                                                                                                    {
                                                                                                                        hatvalue.Clear();
                                                                                                                        hatvalue.Add("columnname ", Att_dcolumn);
                                                                                                                        hatvalue.Add("batch_year", dsperiod.Tables[0].Rows[pre]["batch_year"].ToString());
                                                                                                                        hatvalue.Add("degree_code", dsperiod.Tables[0].Rows[pre]["degree_code"].ToString());
                                                                                                                        hatvalue.Add("sem", dsperiod.Tables[0].Rows[pre]["semester"].ToString());
                                                                                                                        hatvalue.Add("sections", sectionsvalue);
                                                                                                                        hatvalue.Add("month_year", strdate);
                                                                                                                        hatvalue.Add("date", cur_day);
                                                                                                                        hatvalue.Add("subject_no", sp2[0]);
                                                                                                                        dsstuatt.Reset();
                                                                                                                        dsstuatt.Dispose();
                                                                                                                        //dsstuatt = da.select_method("sp_stu_atten_day_check", hatvalue, "sp");
                                                                                                                        string strgetatt = "select count(registration.roll_no) as stucount  from registration,attendance,subjectchooser s where degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and current_semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and  registration.roll_no=s.roll_no ";
                                                                                                                        strgetatt = strgetatt + " and registration.current_semester=s.semester and subject_no='" + sp2[0] + "' " + sectionvar + "";
                                                                                                                        strgetatt = strgetatt + " and (" + Att_dcolumn + " is not null and " + Att_dcolumn + "<>'0' and " + Att_dcolumn + "<>'') and adm_date<='" + cur_day + "' " + strstaffselector + " ";
                                                                                                                        dsstuatt = da.select_method_wo_parameter(strgetatt, "Text");
                                                                                                                        if (dsstuatt.Tables.Count > 0 && dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                                        {
                                                                                                                            if (Att_strqueryst == dsstuatt.Tables[0].Rows[0]["stucount"].ToString())
                                                                                                                            {
                                                                                                                                Att_strqueryst = "0";
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                Att_strqueryst = "1";
                                                                                                                            }
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            Att_strqueryst = "1";
                                                                                                                        }
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        Att_strqueryst = "1";
                                                                                                                    }
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    Att_strqueryst = "1";
                                                                                                                }
                                                                                                            }
                                                                                                            if (int.Parse(Att_strqueryst) > 0)
                                                                                                            {
                                                                                                                attendanceentryflag = false;
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                attendanceentryflag = true;
                                                                                                            }
                                                                                                            FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Font.Underline = true;
                                                                                                            // }
                                                                                                            if (dsall.Tables.Count > 3)
                                                                                                            {
                                                                                                                dsall.Tables[3].DefaultView.RowFilter = "batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sectionvar + " and subject_no='" + sp2[0] + "' and  staff_code='" + stafcode + "' and sch_date='" + cur_day + "' and hr=" + temp + "";
                                                                                                                dvdaily = dsall.Tables[3].DefaultView;
                                                                                                            }
                                                                                                            //strquerytext = "select de.lp_code from dailyentdet de,dailystaffentry ds where de.lp_code=ds.lp_code  and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sectionvar + " and subject_no='" + sp2[0] + "' and  staff_code='" + stafcode + "' and sch_date='" + cur_day + "' and hr=" + temp + "";
                                                                                                            //dsstuatt.Reset();
                                                                                                            //dsstuatt.Dispose();
                                                                                                            //dsstuatt = da.select_method(strquerytext, hatvalue, "Text");
                                                                                                            if (dvdaily.Count > 0)
                                                                                                            {
                                                                                                                dailyentryflag = true;
                                                                                                            }
                                                                                                            if (dailyentryflag == false && attendanceentryflag == false)
                                                                                                            {
                                                                                                                FpSpread1.Sheets[0].Cells[row_inc, temp - 1].ForeColor = Color.Blue;
                                                                                                            }
                                                                                                            else if (dailyentryflag == true && attendanceentryflag == false)
                                                                                                            {
                                                                                                                if (FpSpread1.Sheets[0].Cells[row_inc, temp - 1].ForeColor == Color.DarkOrchid)
                                                                                                                {
                                                                                                                    FpSpread1.Sheets[0].Cells[row_inc, temp - 1].ForeColor = Color.Blue;
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    FpSpread1.Sheets[0].Cells[row_inc, temp - 1].ForeColor = Color.DarkTurquoise;
                                                                                                                }
                                                                                                            }
                                                                                                            else if (dailyentryflag == false && attendanceentryflag == true)
                                                                                                            {
                                                                                                                if (FpSpread1.Sheets[0].Cells[row_inc, temp - 1].ForeColor == Color.DarkTurquoise)
                                                                                                                {
                                                                                                                    FpSpread1.Sheets[0].Cells[row_inc, temp - 1].ForeColor = Color.Blue;
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    FpSpread1.Sheets[0].Cells[row_inc, temp - 1].ForeColor = Color.DarkOrchid;
                                                                                                                }
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                Color getcolor = FpSpread1.Sheets[0].Cells[row_inc, temp - 1].ForeColor;
                                                                                                                if (getcolor != Color.Blue && getcolor != Color.DarkOrchid && getcolor != Color.DarkTurquoise)
                                                                                                                {
                                                                                                                    FpSpread1.Sheets[0].Cells[row_inc, temp - 1].ForeColor = Color.ForestGreen;
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                        dailyentryflag = false;
                                                                                                        attendanceentryflag = false;
                                                                                                        FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Font.Bold = true;
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    if (altfalg == false)
                                                                    {
                                                                        getcolumnfield = Convert.ToString(strday + temp);
                                                                        attendanceentryflag = false;
                                                                        dailyentryflag = false;
                                                                        // if (dsperiod.Tables[0].Rows[pre][getcolumnfield].ToString() != "" && dsperiod.Tables[0].Rows[pre][getcolumnfield].ToString() != null && dsperiod.Tables[0].Rows[pre][getcolumnfield].ToString() != "\0")
                                                                        if (dvsemster.Count > 0)
                                                                        {
                                                                            if (dvsemster[0][getcolumnfield].ToString() != "" && dvsemster[0][getcolumnfield].ToString() != null && dvsemster[0][getcolumnfield].ToString() != "\0")
                                                                            {
                                                                                string timetable = string.Empty;
                                                                                string name = dvsemster[0]["ttname"].ToString();
                                                                                if (name != null && name.Trim() != "")
                                                                                {
                                                                                    timetable = name;
                                                                                }
                                                                                sp_rd = dvsemster[0][getcolumnfield].ToString();
                                                                                string[] sp_rd_semi = sp_rd.Split(';');
                                                                                for (int semi = 0; semi <= sp_rd_semi.GetUpperBound(0); semi++)
                                                                                {
                                                                                    string[] sp2 = sp_rd_semi[semi].Split(new Char[] { '-' });
                                                                                    if (sp2.GetUpperBound(0) >= 1)
                                                                                    {
                                                                                        int upperbound = sp2.GetUpperBound(0);
                                                                                        for (int multi_staff = 1; multi_staff < sp2.GetUpperBound(0); multi_staff++)
                                                                                        {
                                                                                            if (sp2[multi_staff] == stafcode)
                                                                                            {
                                                                                                //==============================theroy batch=======================================
                                                                                                bool checklabhr = false;
                                                                                                for (int sr = 0; sr <= sp_rd_semi.GetUpperBound(0); sr++)
                                                                                                {
                                                                                                    string[] getlasub = sp_rd_semi[sr].ToString().Split('-');
                                                                                                    if (getlasub.GetUpperBound(0) > 1)
                                                                                                    {
                                                                                                        string srllab = getlasub[0].ToString();
                                                                                                        if (hatcurlab.Contains(srllab))
                                                                                                        {
                                                                                                            checklabhr = true;
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                //======================================================================
                                                                                                string sect = dsperiod.Tables[0].Rows[pre]["sections"].ToString();
                                                                                                if (sect == "-1" || sect == null || sect.Trim() == "")
                                                                                                {
                                                                                                    sect = string.Empty;
                                                                                                }
                                                                                                if (semenddate.Trim() != "" && semenddate.Trim() != null && semenddate.Trim() != "0")
                                                                                                {
                                                                                                    if (cur_day <= (Convert.ToDateTime(semenddate)))
                                                                                                    {
                                                                                                        check_hour = true;
                                                                                                        double Num;
                                                                                                        bool isNum = double.TryParse(sp2[0].ToString(), out Num);
                                                                                                        if (isNum)
                                                                                                        {
                                                                                                            // text_temp = da.GetFunction("select subject_name from subject where subject_no=" + sp2[0] + "") + "-" + sp2[upperbound];
                                                                                                            if (checklabhr == false)
                                                                                                            {
                                                                                                                dsall.Tables[4].DefaultView.RowFilter = " subject_no=" + sp2[0] + "";
                                                                                                                dvsubject = dsall.Tables[4].DefaultView;
                                                                                                                if (dvsubject.Count > 0)
                                                                                                                {
                                                                                                                    text_temp = dvsubject[0]["subject_name"].ToString() + "-S";
                                                                                                                }
                                                                                                                //text_temp = da.GetFunction("select subject_name from subject where subject_no=" + sp2[0] + "") + "-S";
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                dsall.Tables[4].DefaultView.RowFilter = " subject_no=" + sp2[0] + "";
                                                                                                                dvsubject = dsall.Tables[4].DefaultView;
                                                                                                                if (dvsubject.Count > 0)
                                                                                                                {
                                                                                                                    text_temp = dvsubject[0]["subject_name"].ToString() + "-S";
                                                                                                                }
                                                                                                                //text_temp = da.GetFunction("select subject_name from subject where subject_no=" + sp2[0] + "") + "-L";
                                                                                                            }
                                                                                                            string Schedule_string = string.Empty;
                                                                                                            if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() == "-1" || dsperiod.Tables[0].Rows[pre]["sections"].ToString() == "" || dsperiod.Tables[0].Rows[pre]["sections"].ToString() == null)
                                                                                                            {
                                                                                                                if (checklabhr == false)
                                                                                                                {
                                                                                                                    Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-S-" + sp_rd_semi.GetUpperBound(0);
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-L-" + sp_rd_semi.GetUpperBound(0);
                                                                                                                }
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                if (checklabhr == false)
                                                                                                                {
                                                                                                                    Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-S-" + sp_rd_semi.GetUpperBound(0);
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-L-" + sp_rd_semi.GetUpperBound(0);
                                                                                                                }
                                                                                                            }
                                                                                                            bool allowleave = false;
                                                                                                            if (hatholiday.Contains(cur_day.ToString()))
                                                                                                            {
                                                                                                                if (moringleav == true)
                                                                                                                {
                                                                                                                    if (frshlf >= temp)
                                                                                                                    {
                                                                                                                        allowleave = true;
                                                                                                                    }
                                                                                                                }
                                                                                                                if (evenleave == true)
                                                                                                                {
                                                                                                                    if (temp > frshlf)
                                                                                                                    {
                                                                                                                        allowleave = true;
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                            if (allowleave == true)
                                                                                                            {
                                                                                                                if (hatholiday.Contains(cur_day.ToString()))
                                                                                                                {
                                                                                                                    string holidayreason = GetCorrespondingKey(cur_day.ToString(), hatholiday).ToString();
                                                                                                                    if (FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Text.Trim() == "")
                                                                                                                    {
                                                                                                                        FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Text = "Selected day is Holiday- Reason-" + holidayreason + "-" + text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                        FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Tag = "Selected day is Holiday- Reason-" + holidayreason + "-" + Schedule_string.ToString() + "-sem";
                                                                                                                        if (Convert.ToString(Day_Order).Trim() != "")
                                                                                                                        {
                                                                                                                            FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Note = Convert.ToString(Day_Order);
                                                                                                                        }
                                                                                                                        FpSpread1.Sheets[0].Cells[(row_inc), temp - 1].ForeColor = Color.Blue;
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Text = FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Text + '*' + "Selected day is Holiday- Reason-" + holidayreason + "-" + text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                        FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Tag = FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Tag.ToString() + '*' + "Selected day is Holiday- Reason-" + holidayreason + "-" + Schedule_string.ToString() + "-sem";
                                                                                                                        if (Convert.ToString(Day_Order).Trim() != "")
                                                                                                                        {
                                                                                                                            FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Note = Convert.ToString(Day_Order);
                                                                                                                        }
                                                                                                                        FpSpread1.Sheets[0].Cells[(row_inc), temp - 1].ForeColor = Color.Blue;
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                if (FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Text.Trim() == "")
                                                                                                                {
                                                                                                                    FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Text = text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                    FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Tag = Schedule_string.ToString() + "-sem";
                                                                                                                    if (Convert.ToString(Day_Order).Trim() != "")
                                                                                                                    {
                                                                                                                        FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Note = Convert.ToString(Day_Order);
                                                                                                                    }
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    if (FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Text.Trim() != "")
                                                                                                                    {
                                                                                                                        string tmpvar = string.Empty;
                                                                                                                        string istemp = string.Empty;
                                                                                                                        istemp = FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Text.ToString();
                                                                                                                        tmpvar = text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                        if (Convert.ToString(istemp) != Convert.ToString(tmpvar))
                                                                                                                        {
                                                                                                                            FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Text = FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Text + " * " + text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                            FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Tag = FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Tag + " * " + Schedule_string.ToString() + "-sem";
                                                                                                                            if (Convert.ToString(Day_Order).Trim() != "")
                                                                                                                            {
                                                                                                                                FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Note = Convert.ToString(Day_Order);
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Text = text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                        FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Tag = Schedule_string.ToString() + "-sem";
                                                                                                                        if (Convert.ToString(Day_Order).Trim() != "")
                                                                                                                        {
                                                                                                                            FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Note = Convert.ToString(Day_Order);
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                                //----------------set color
                                                                                                                if (FpSpread1.Sheets[0].Cells[row_inc, temp - 1].ForeColor != Color.Blue)
                                                                                                                {
                                                                                                                    string daystring = dt2.AddDays(-row_inc).ToString("dd");
                                                                                                                    string daystring1 = dt2.AddDays(-row_inc).ToString("ddd");
                                                                                                                    string Att_dcolumn = "d" + Convert.ToInt16(daystring) + "d" + temp;
                                                                                                                    check_lab = string.Empty;
                                                                                                                    hatvalue.Clear();
                                                                                                                    if (checklabhr == false)
                                                                                                                    {
                                                                                                                        check_lab = "0";
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        check_lab = "1";
                                                                                                                    }
                                                                                                                    sectionvar = string.Empty;
                                                                                                                    sectionsvalue = string.Empty;
                                                                                                                    if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() != "-1" && dsperiod.Tables[0].Rows[pre]["sections"].ToString() != "" && dsperiod.Tables[0].Rows[pre]["sections"].ToString() != null)
                                                                                                                    {
                                                                                                                        sectionvar = " and sections='" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "'";
                                                                                                                        sectionsvalue = dsperiod.Tables[0].Rows[pre]["sections"].ToString();
                                                                                                                    }
                                                                                                                    if (check_lab == "1" || check_lab.Trim().ToLower() == "true")
                                                                                                                    {
                                                                                                                        hatvalue.Clear();
                                                                                                                        //hatvalue.Add("batch_year", dsperiod.Tables[0].Rows[pre]["batch_year"].ToString());
                                                                                                                        //hatvalue.Add("degree_code", dsperiod.Tables[0].Rows[pre]["degree_code"].ToString());
                                                                                                                        //hatvalue.Add("sem", dsperiod.Tables[0].Rows[pre]["semester"].ToString());
                                                                                                                        //hatvalue.Add("sections", sectionsvalue);
                                                                                                                        //hatvalue.Add("month_year", strdate);
                                                                                                                        //hatvalue.Add("date", cur_day);
                                                                                                                        //hatvalue.Add("subject_no", sp2[0]);
                                                                                                                        //hatvalue.Add("day", strday);
                                                                                                                        //hatvalue.Add("hour", temp);
                                                                                                                        //hatvalue.Add("ttmane", timetable);
                                                                                                                        //hatvalue.Add("staff_code", staff_code);
                                                                                                                        string strstt = "select count(distinct r.Roll_No) as stucount from registration r,subjectchooser s where degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and ";
                                                                                                                        strstt = strstt + " current_semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar' and r.roll_no=s.roll_no ";
                                                                                                                        strstt = strstt + " and r.current_semester=s.semester and subject_no='" + sp2[0] + "' " + sectionvar + " and batch in(select stu_batch from ";
                                                                                                                        strstt = strstt + " laballoc where subject_no='" + sp2[0] + "'  and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "'  and hour_value='" + temp + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' ";
                                                                                                                        strstt = strstt + " and day_value='" + strday + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + sectionvar + " and Timetablename='" + timetable + "') and adm_date<='" + cur_day + "'  " + strstaffselector + "";
                                                                                                                        dsstuatt.Dispose();
                                                                                                                        dsstuatt.Reset();
                                                                                                                        // dsstuatt = da.select_method("sp_stu_atten_month_check_lab", hatvalue, "sp");
                                                                                                                        dsstuatt = da.select_method_wo_parameter(strstt, "text");
                                                                                                                        if (dsstuatt.Tables.Count > 0 && dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                                        {
                                                                                                                            Att_strqueryst = dsstuatt.Tables[0].Rows[0]["stucount"].ToString();
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            Att_strqueryst = "1";
                                                                                                                        }
                                                                                                                        if (dsstuatt.Tables.Count > 0 && dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                                        {
                                                                                                                            Att_strqueryst = dsstuatt.Tables[0].Rows[0]["stucount"].ToString();
                                                                                                                            if (int.Parse(Att_strqueryst) > 0)
                                                                                                                            {
                                                                                                                                hatvalue.Clear();
                                                                                                                                hatvalue.Add("columnname", Att_dcolumn);
                                                                                                                                hatvalue.Add("batch_year", dsperiod.Tables[0].Rows[pre]["batch_year"].ToString());
                                                                                                                                hatvalue.Add("degree_code", dsperiod.Tables[0].Rows[pre]["degree_code"].ToString());
                                                                                                                                hatvalue.Add("sem", dsperiod.Tables[0].Rows[pre]["semester"].ToString());
                                                                                                                                hatvalue.Add("sections", sectionsvalue);
                                                                                                                                hatvalue.Add("month_year", strdate);
                                                                                                                                hatvalue.Add("date", cur_day);
                                                                                                                                hatvalue.Add("subject_no", sp2[0]);
                                                                                                                                hatvalue.Add("day", strday);
                                                                                                                                hatvalue.Add("hour", temp);
                                                                                                                                hatvalue.Add("ttmane", timetable);
                                                                                                                                dsstuatt.Reset();
                                                                                                                                dsstuatt.Dispose();
                                                                                                                                // dsstuatt = da.select_method("sp_stu_atten_day_check_lab", hatvalue, "sp");
                                                                                                                                string strgetatt = "select count(distinct r.Roll_No) as stucount from registration r,attendance a,subjectchooser s where degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and current_semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year=" + strdate + "";
                                                                                                                                strgetatt = strgetatt + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and r.current_semester=s.semester and subject_no='" + sp2[0].ToString() + "' " + sectionvar + " and(" + Att_dcolumn + " is not null and " + Att_dcolumn + "<>'0' and " + Att_dcolumn + "<>'') and batch in(select stu_batch from laballoc ";
                                                                                                                                strgetatt = strgetatt + " where subject_no='" + sp2[0].ToString() + "' and Timetablename='" + timetable + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "'  and hour_value='" + temp + "'  and    degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and day_value='" + strday + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + sectionvar + ") and adm_date<='" + cur_day + "'  " + strstaffselector + "";
                                                                                                                                dsstuatt = da.select_method_wo_parameter(strgetatt, "Text");
                                                                                                                                if (dsstuatt.Tables.Count > 0 && dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                                                {
                                                                                                                                    if (Att_strqueryst == dsstuatt.Tables[0].Rows[0]["stucount"].ToString())
                                                                                                                                    {
                                                                                                                                        Att_strqueryst = "0";
                                                                                                                                    }
                                                                                                                                    else
                                                                                                                                    {
                                                                                                                                        Att_strqueryst = "1";
                                                                                                                                    }
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    Att_strqueryst = "1";
                                                                                                                                }
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                Att_strqueryst = "1";
                                                                                                                            }
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            Att_strqueryst = "1";
                                                                                                                        }
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        hatvalue.Clear();
                                                                                                                        hatvalue.Add("batch_year", dsperiod.Tables[0].Rows[pre]["batch_year"].ToString());
                                                                                                                        hatvalue.Add("degree_code", dsperiod.Tables[0].Rows[pre]["degree_code"].ToString());
                                                                                                                        hatvalue.Add("sem", dsperiod.Tables[0].Rows[pre]["semester"].ToString());
                                                                                                                        hatvalue.Add("sections", sectionsvalue);
                                                                                                                        hatvalue.Add("month_year", strdate);
                                                                                                                        hatvalue.Add("date", cur_day);
                                                                                                                        hatvalue.Add("subject_no", sp2[0]);
                                                                                                                        dsstuatt.Reset();
                                                                                                                        dsstuatt.Dispose();
                                                                                                                        dssubstucount.Tables[0].DefaultView.RowFilter = "batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and  degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "'  and current_semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' and subject_no='" + sp2[0] + "' " + strsction + " and adm_date<='" + cur_day.ToString("MM/dd/yyyy").ToString() + "' ";
                                                                                                                        dvsubstucount = dssubstucount.Tables[0].DefaultView;
                                                                                                                        if (dvsubstucount.Count > 0)
                                                                                                                        {
                                                                                                                            int stustradm = 0;
                                                                                                                            for (int stuadmcou = 0; stuadmcou < dvsubstucount.Count; stuadmcou++)
                                                                                                                            {
                                                                                                                                stustradm = stustradm + Convert.ToInt32(dvsubstucount[stuadmcou]["stucount"]);
                                                                                                                            }
                                                                                                                            Att_strqueryst = stustradm.ToString();
                                                                                                                            if (int.Parse(Att_strqueryst) > 0)
                                                                                                                            {
                                                                                                                                hatvalue.Clear();
                                                                                                                                hatvalue.Add("columnname ", Att_dcolumn);
                                                                                                                                hatvalue.Add("batch_year", dsperiod.Tables[0].Rows[pre]["batch_year"].ToString());
                                                                                                                                hatvalue.Add("degree_code", dsperiod.Tables[0].Rows[pre]["degree_code"].ToString());
                                                                                                                                hatvalue.Add("sem", dsperiod.Tables[0].Rows[pre]["semester"].ToString());
                                                                                                                                hatvalue.Add("sections", sectionsvalue);
                                                                                                                                hatvalue.Add("month_year", strdate);
                                                                                                                                hatvalue.Add("date", cur_day);
                                                                                                                                hatvalue.Add("subject_no", sp2[0]);
                                                                                                                                dsstuatt.Reset();
                                                                                                                                dsstuatt.Dispose();
                                                                                                                                string strgetatt = "select count( registration.roll_no) as stucount  from registration,attendance,subjectchooser s where degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and current_semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and  registration.roll_no=s.roll_no ";
                                                                                                                                strgetatt = strgetatt + " and registration.current_semester=s.semester and subject_no='" + sp2[0] + "' " + sectionvar + "";
                                                                                                                                strgetatt = strgetatt + " and (" + Att_dcolumn + " is not null and " + Att_dcolumn + "<>'0' and " + Att_dcolumn + "<>'') and adm_date<='" + cur_day + "'  " + strstaffselector + "";
                                                                                                                                dsstuatt = da.select_method_wo_parameter(strgetatt, "Text");
                                                                                                                                if (dsstuatt.Tables.Count > 0 && dsstuatt.Tables[0].Rows.Count > 0)
                                                                                                                                {
                                                                                                                                    if (Att_strqueryst == dsstuatt.Tables[0].Rows[0]["stucount"].ToString())
                                                                                                                                    {
                                                                                                                                        Att_strqueryst = "0";
                                                                                                                                    }
                                                                                                                                    else
                                                                                                                                    {
                                                                                                                                        Att_strqueryst = "1";
                                                                                                                                    }
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    Att_strqueryst = "1";
                                                                                                                                }
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                Att_strqueryst = "1";
                                                                                                                            }
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            Att_strqueryst = "1";
                                                                                                                        }
                                                                                                                    }
                                                                                                                    if (int.Parse(Att_strqueryst) > 0)
                                                                                                                    {
                                                                                                                        attendanceentryflag = false;
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        attendanceentryflag = true;
                                                                                                                    }
                                                                                                                    FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Font.Underline = true;
                                                                                                                    dsall.Tables[3].DefaultView.RowFilter = "batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sectionvar + " and subject_no='" + sp2[0] + "' and  staff_code='" + stafcode + "' and sch_date='" + cur_day + "' and hr=" + temp + "";
                                                                                                                    dvdaily = dsall.Tables[3].DefaultView;
                                                                                                                    if (dvdaily.Count > 0)
                                                                                                                    {
                                                                                                                        dailyentryflag = true;
                                                                                                                    }
                                                                                                                    if (dailyentryflag == false && attendanceentryflag == false)
                                                                                                                    {
                                                                                                                        FpSpread1.Sheets[0].Cells[row_inc, temp - 1].ForeColor = Color.Blue;
                                                                                                                    }
                                                                                                                    else if (dailyentryflag == true && attendanceentryflag == false)
                                                                                                                    {
                                                                                                                        if (FpSpread1.Sheets[0].Cells[row_inc, temp - 1].ForeColor == Color.DarkOrchid)
                                                                                                                        {
                                                                                                                            FpSpread1.Sheets[0].Cells[row_inc, temp - 1].ForeColor = Color.Blue;
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            FpSpread1.Sheets[0].Cells[row_inc, temp - 1].ForeColor = Color.DarkTurquoise;
                                                                                                                        }
                                                                                                                    }
                                                                                                                    else if (dailyentryflag == false && attendanceentryflag == true)
                                                                                                                    {
                                                                                                                        if (FpSpread1.Sheets[0].Cells[row_inc, temp - 1].ForeColor == Color.DarkTurquoise)
                                                                                                                        {
                                                                                                                            FpSpread1.Sheets[0].Cells[row_inc, temp - 1].ForeColor = Color.Blue;
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            FpSpread1.Sheets[0].Cells[row_inc, temp - 1].ForeColor = Color.DarkOrchid;
                                                                                                                        }
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        Color getcolor = FpSpread1.Sheets[0].Cells[row_inc, temp - 1].ForeColor;
                                                                                                                        if (getcolor != Color.Blue && getcolor != Color.DarkOrchid && getcolor != Color.DarkTurquoise)
                                                                                                                        {
                                                                                                                            FpSpread1.Sheets[0].Cells[row_inc, temp - 1].ForeColor = Color.ForestGreen;
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                                dailyentryflag = false;
                                                                                                                attendanceentryflag = false;
                                                                                                                FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Font.Bold = true;
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }//Added By Srinath Day Order Change 4Sep2014
                                        }
                                    }
                                //lb1: tmp_camprevar = Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["FromDate"]);
                                lb1: tmp_camprevar = Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]);
                                }
                            }
                        }
                    }
                    FpSpread1.Sheets[0].AutoPostBack = true;
                    FpSpread1.SaveChanges();
                    FpSpread1.Visible = true;
                    Labelstaf.Visible = false;
                    FpSpread1.Sheets[0].Columns.Default.Width = 300;
                }
            }
            if (FpSpread1.Sheets[0].RowCount > 0)
                FpSpread1.Sheets[0].PageSize = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);
            else
            {
                FpSpread1.Visible = false;//Added by manikandan 24/8/2013
            }
            FpSpread1.SaveChanges();
        }
        catch (Exception ex)
        {
            Labelstaf.Visible = true;
            Labelstaf.Text = ex.ToString();
        }
    }

    protected void tbfdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Labelstaf.Visible = false;
            string[] spiltfrom = tbfdate.Text.ToString().Split(new Char[] { '-' });
            string[] spilto = tbtodate.Text.ToString().Split('-');
            DateTime dtto = Convert.ToDateTime(spilto[1].ToString() + '-' + spilto[0].ToString() + '-' + spilto[2].ToString());
            DateTime dtfrom = Convert.ToDateTime(spiltfrom[1].ToString() + '-' + spiltfrom[0].ToString() + '-' + spiltfrom[2].ToString());
            if (dtfrom > DateTime.Today)
            {
                if (Session["StafforAdmin"] == "")
                {
                    Labelstaf.Visible = true;
                    Labelstaf.Text = "You can not mark attendance for the date greater than today";
                    tbfdate.Text = DateTime.Today.ToString("d-MM-yyyy");
                }
            }
            if (dtfrom > dtto)
            {
                Labelstaf.Visible = true;
                Labelstaf.Text = "To Date Must be Greater than From Date";
                tbfdate.Text = tbtodate.Text;
            }
        }
        catch
        {
            Labelstaf.Visible = true;
            Labelstaf.Text = "Please Enter Valid From Date";
        }
    }

    protected void tbtodate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Labelstaf.Visible = false;
            string[] spiltfrom = tbfdate.Text.ToString().Split(new Char[] { '-' });
            string[] spilto = tbtodate.Text.ToString().Split('-');
            DateTime dtto = Convert.ToDateTime(spilto[1].ToString() + '-' + spilto[0].ToString() + '-' + spilto[2].ToString());
            DateTime dtfrom = Convert.ToDateTime(spiltfrom[1].ToString() + '-' + spiltfrom[0].ToString() + '-' + spiltfrom[2].ToString());
            if (dtto > DateTime.Today)
            {
                if (Session["StafforAdmin"] == "")
                {
                    Labelstaf.Visible = true;
                    Labelstaf.Text = "You can not mark attendance for the date greater than today";
                    tbtodate.Text = DateTime.Today.ToString("d-MM-yyyy");
                }
            }
            if (dtfrom > dtto)
            {
                Labelstaf.Visible = true;
                Labelstaf.Text = "To Date Must be Greater than From Date";
                tbfdate.Text = tbtodate.Text;
            }
        }
        catch
        {
            Labelstaf.Visible = true;
            Labelstaf.Text = "Please Enter Valid To Date";
        }
    }

    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void option_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void spreadatt_qtnadd_cellclick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        cellclick2 = true;
    }

    protected void FpSpread1_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        cellclick1 = false;
        //colhead = true;
    }

    public string Attvalues(string Att_str1)
    {
        string Attvalue;
        Attvalue = string.Empty;
        if (Att_str1 == "P")
        {
            Attvalue = "1";
        }
        else if (Att_str1 == "A")
        {
            Attvalue = "2";
        }
        else if (Att_str1 == "OD")
        {
            Attvalue = "3";
        }
        else if (Att_str1 == "ML")
        {
            Attvalue = "4";
        }
        else if (Att_str1 == "SOD")
        {
            Attvalue = "5";
        }
        else if (Att_str1 == "NSS")
        {
            Attvalue = "6";
        }
        else if (Att_str1 == "H")
        {
            Attvalue = "7";
        }
        else if (Att_str1 == "NJ")
        {
            Attvalue = "8";
        }
        else if (Att_str1 == "S")
        {
            Attvalue = "9";
        }
        else if (Att_str1 == "L")
        {
            Attvalue = "10";
        }
        else if (Att_str1 == "NCC")
        {
            Attvalue = "11";
        }
        else if (Att_str1 == "HS")
        {
            Attvalue = "12";
        }
        else if (Att_str1 == "PP")
        {
            Attvalue = "13";
        }
        else if (Att_str1 == "SYOD")
        {
            Attvalue = "14";
        }
        else if (Att_str1 == "COD")
        {
            Attvalue = "15";
        }
        else if (Att_str1 == "OOD")
        {
            Attvalue = "16";
        }
        else if (Att_str1 == "LA")
        {
            Attvalue = "17";
        }
        else
        {
            Attvalue = string.Empty;
        }
        return Attvalue;
    }

    public string Attmark(string Attstr_mark)
    {
        string Att_mark = string.Empty;
        if (Attstr_mark == "1")
        {
            Att_mark = "P";
        }
        else if (Attstr_mark == "2")
        {
            Att_mark = "A";
        }
        else if (Attstr_mark == "3")
        {
            Att_mark = "OD";
        }
        else if (Attstr_mark == "4")
        {
            Att_mark = "ML";
        }
        else if (Attstr_mark == "5")
        {
            Att_mark = "SOD";
        }
        else if (Attstr_mark == "6")
        {
            Att_mark = "NSS";
        }
        else if (Attstr_mark == "7")
        {
            Att_mark = "H";
        }
        else if (Attstr_mark == "8")
        {
            Att_mark = "NJ";
        }
        else if (Attstr_mark == "9")
        {
            Att_mark = "S";
        }
        else if (Attstr_mark == "10")
        {
            Att_mark = "L";
        }
        else if (Attstr_mark == "11")
        {
            Att_mark = "NCC";
        }
        else if (Attstr_mark == "12")
        {
            Att_mark = "HS";
        }
        else if (Attstr_mark == "13")
        {
            Att_mark = "PP";
        }
        else if (Attstr_mark == "14")
        {
            Att_mark = "SYOD";
        }
        else if (Attstr_mark == "15")
        {
            Att_mark = "COD";
        }
        else if (Attstr_mark == "16")
        {
            Att_mark = "OOD";
        }
        else if (Attstr_mark == "17")
        {
            Att_mark = "LA";
        }
        else
        {
            Att_mark = string.Empty;
        }
        //return Convert.ToInt32(Att_mark);
        return Att_mark;
    }

    protected void FpSpread1_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            // old_spread2_select();
            bool checkleave = false;
            if (cellclick1 == true)
            {
                Panelyet.Width = 460;
                Panelcomplete.Width = 460;
                if (Session["flag"].ToString().Trim() == "1")
                {
                    chkalterlession.Visible = true;
                }
                else
                {
                    chkalterlession.Visible = false;
                }
                chkalterlession.Checked = false;
                Plessionalter.Visible = false;
                ddlclassnotes.Items.Clear();//Added by srinath 22/3/2015
                Labelstaf.Visible = false;
                lbl_alert.Visible = false;
                FpSpread1.SaveChanges();
                Buttonupdate.Enabled = false;
                Buttonsavelesson.Enabled = false;
                txtquestion1.Text = string.Empty;//Added by Srinath 21/8/2013
                btnupdatequetion.Enabled = false;
                btndeleteatndqtn.Enabled = false;
                btnaddquestion.Enabled = true;//Added By Srinath 21/8/2013
                FpSpread3.Sheets[0].RowCount = 0;
                singlesubject = false;
                lbldayorder.Visible = false;
                ddlselectmanysub.Visible = false;
                lblmanysubject.Visible = false;
                string sub_name = string.Empty;
                selectedpath = string.Empty;
                Buttonsave.Enabled = true;
                //  pattendanceentry.Visible = false;
                clearfield();
                dicFeeOfRollStudents = new Dictionary<string, DateTime[]>();
                dicFeeOnRollStudents = new Dictionary<string, byte>();
                GetFeeOfRollStudent(ref dicFeeOfRollStudents, ref dicFeeOnRollStudents);
                if (staff_code.ToString() != "")
                {
                    //LabelE.Visible = false;
                    btnaddquestion.Enabled = true;
                    btnqtnupdate.Enabled = false;
                    ar = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveRow.ToString());
                    ac = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveColumn.ToString());
                    string getdayorder = Convert.ToString(FpSpread1.Sheets[0].Cells[ar, ac].Note);
                    if (Convert.ToString(getdayorder).Trim() != "")
                    {
                        string[] dayorderval = getdayorder.Split(new Char[] { '-' });
                        if (Convert.ToString(dayorderval[0]).Trim() != "0")
                        {
                            lbldayorder.Visible = true;
                            lbldayorder.Text = "Day Order " + Convert.ToString(dayorderval[0]).Trim();
                        }
                        Day_Var = Convert.ToString(dayorderval[1]).Trim();
                    }
                    if (FpSpread1.Sheets[0].Cells[ar, ac].Locked == false)
                    {
                        if (ar != -1)
                        {
                            //Modified by srinath 29/8/2013 ==Start
                            string spread_text = FpSpread1.Sheets[0].Cells[ar, ac].Text;
                            string text_val = FpSpread1.Sheets[0].Cells[ar, ac].Text;
                            if (spread_text != "" && spread_text != "Sunday Holiday")
                            {
                                getcelltag = FpSpread1.Sheets[0].GetTag(ar, ac).ToString();
                                string avoidholiday = string.Empty;
                                string avoidholidaytext = string.Empty;
                                string[] spiltgetceltag = getcelltag.Split('*');
                                string[] spilttext = text_val.Split('*');
                                for (int k = 0; k <= spiltgetceltag.GetUpperBound(0); k++)
                                {
                                    string[] spitvalue = spiltgetceltag[k].Split('-');  //" + Session["collegecode"].ToString() + "
                                    string minimumabsentsms = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Session["collegecode"].ToString() + "'");
                                    string[] splitminimumabsentsms = minimumabsentsms.Split('-');
                                    if (splitminimumabsentsms.Length == 2)
                                    {
                                        int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
                                        int checkbatchyear = 0;
                                        if (spitvalue.Length == 7)
                                        {
                                            checkbatchyear = Convert.ToInt32(spitvalue[3].ToString());
                                        }
                                        if (spitvalue.Length == 8)
                                        {
                                            checkbatchyear = Convert.ToInt32(spitvalue[4].ToString());
                                        }
                                        if (splitminimumabsentsms[0].ToString() == "1" && checkbatchyear >= batchyearsetting)
                                        // if (splitminimumabsentsms[0].ToString() == "1")
                                        {
                                            Session["StaffSelector"] = "1";
                                        }
                                        else
                                        {
                                            Session["StaffSelector"] = "0";
                                        }
                                    }
                                    else
                                    {
                                        Session["StaffSelector"] = "0";
                                    }
                                    if (spitvalue[0].ToLower().Trim() == "selected day is holiday")
                                    {
                                    }
                                    else
                                    {
                                        if (avoidholiday == "")
                                        {
                                            avoidholiday = spiltgetceltag[k].ToString();
                                            avoidholidaytext = spilttext[k].ToString();
                                        }
                                        else
                                        {
                                            avoidholiday = avoidholiday + '*' + spiltgetceltag[k].ToString();
                                            avoidholidaytext = avoidholidaytext + '*' + spilttext[k].ToString();
                                        }
                                    }
                                }
                                getcelltag = avoidholiday;
                                text_val = avoidholidaytext;
                                //========end
                                hr = FpSpread1.Sheets[0].ColumnHeader.Cells[0, ac].Tag.ToString();
                                string[] spitdate = FpSpread1.Sheets[0].RowHeader.Cells[ar, 0].Text.Split(' ');
                                sel_date1 = spitdate[0].ToString();
                                getcolheader = FpSpread1.Sheets[0].ColumnHeader.Cells[0, ac].Text;
                                string[] sel_date_split = sel_date1.Split(new Char[] { '-' });
                                getdate_new = sel_date_split[0] + "-" + sel_date_split[1] + "-" + sel_date_split[2];
                                if (sel_date_split[0].Length == 1)
                                {
                                    sel_date_split[0] = "0" + sel_date_split[0];
                                }
                                if (sel_date_split[1].Length == 1)
                                {
                                    sel_date_split[1] = "0" + sel_date_split[1];
                                }
                                sel_date1 = sel_date_split[0] + "-" + sel_date_split[1] + "-" + sel_date_split[2];
                                sel_date = sel_date_split[1] + "-" + sel_date_split[0] + "-" + sel_date_split[2];
                                getdate = sel_date_split[2] + "-" + sel_date_split[1] + "-" + sel_date_split[0];
                                string[] subject1 = spread_text.Split(new char[] { '2' });
                                sub_name = subject1[0].ToString();
                                string[] treepath1 = sub_name.Split(new char[] { '-' });
                                if (treepath1.GetUpperBound(0) == 1)
                                {
                                    sub_name = treepath1[0].ToString();
                                }
                                else if (treepath1.GetUpperBound(0) > 1)
                                {
                                    sub_name = treepath1[0] + "-" + treepath1[1];
                                }
                                selectedpath = sub_name + " " + "/";
                                storepath = selectedpath;
                                DateTime tem = Convert.ToDateTime(sel_date);
                                strday = tem.ToString("ddd");
                                // string text_val = FpSpread1.Sheets[0].Cells[ar, ac].Text;
                                string[] splittext = text_val.Split(new char[] { '*' });
                                string[] splitvalue = getcelltag.Split('*');
                                string split_val = string.Empty;
                                ddlselectmanysub.Items.Clear();
                                for (int splitvalue_star = 0; splitvalue_star <= splittext.GetUpperBound(0); splitvalue_star++)
                                {
                                    int max_val = ddlselectmanysub.Items.Count;
                                    split_val = splittext[splitvalue_star].ToString();
                                    ddlselectmanysub.Items.Add(split_val);
                                    ddlselectmanysub.Items[max_val].Value = splitvalue[splitvalue_star].ToString();
                                    ddlclassnotes.Items.Add(split_val);//Added by srinath 22/3/2015
                                    ddlclassnotes.Items[max_val].Value = splitvalue[splitvalue_star].ToString();//Added by srinath 22/3/2015
                                }
                                ddlselectmanysub.Items.Insert(0, " ");
                                if (ddlselectmanysub.Items.Count >= 3)
                                {
                                    ddlselectmanysub.Visible = true;
                                    lblmanysubject.Visible = true;
                                    ddlselectmanysub.SelectedIndex = 0;
                                    ddlclassnotes.Items.Insert(0, "All");//Added by srinath 22/3/2015
                                }
                                else
                                {
                                    if (ddlselectmanysub.Items.Count == 2)
                                    {
                                        ddlselectmanysub.SelectedIndex = 1;
                                    }
                                    ddlselectmanysub.Visible = false;
                                    lblmanysubject.Visible = false;
                                }
                                splitvalue = ddlselectmanysub.SelectedValue.ToString().Split('-');
                                if (splitvalue.GetUpperBound(0) > 0)
                                {
                                    if (splitvalue.GetUpperBound(0) == 7)
                                    {
                                        string degree_code = splitvalue[0].ToString();
                                        string semester = splitvalue[1].ToString();
                                        string subject_no = splitvalue[2].ToString();
                                        string batch_year = splitvalue[4].ToString();
                                        string secval = string.Empty;
                                        if (splitvalue.GetUpperBound(0) == 7)
                                        {
                                            secval = splitvalue[3];
                                            batch_year = splitvalue[4];
                                        }
                                        else
                                        {
                                            batch_year = splitvalue[3];
                                        }
                                        loadunitssubj_no = subject_no;
                                        bool hrlock = Hour_lock(degree_code, batch_year, semester, getcolheader, secval);
                                        if (hrlock == true)
                                        {
                                            Buttonsave.Visible = false;
                                            Buttonupdate.Visible = false;
                                            pHeaderatendence.Visible = false;
                                            pHeaderlesson.Visible = false;
                                            headerpanelnotes.Visible = false;
                                            pBodyatendence.Visible = false;
                                            pBodylesson.Visible = false;
                                            pBodynotes.Visible = false;
                                            pBodyquestionaddition.Visible = false;
                                            headerquestionaddition.Visible = false;
                                            headerADDQuestion.Visible = false;
                                            lbl_alert.Visible = true;
                                            //Added by srinath 7/9/2013
                                            //  lbl_alert.Text = "You cannot edit this day/Hour attendance due to security reasons.Contact Inspro Plus Administrator";
                                            //Added by srinath 25/8/2016 JPR
                                            lbl_alert.Text = " You cannot edit this day/Hour attendance due to security reasons. Get permission from PRINCIPAL to update the attendance";
                                            FpSpread2.Visible = false;
                                            Buttondeselect.Visible = false;
                                            Buttonselectall.Visible = false;
                                            lblmanysubject.Visible = false;
                                            ddlselectmanysub.Visible = false;
                                            return;
                                        }
                                        sprdnoofchoices.Sheets[0].RowCount = 0;
                                        sprdviewdata.Sheets[0].RowCount = 0;
                                        retrievespreadattendancequestion(batch_year, degree_code, semester, subject_no, sel_date1);
                                        retrivespreadfornotes(batch_year, degree_code, semester, subject_no, sel_date1);
                                        sprdretrivedate();
                                        btnSave.Visible = true;
                                    }
                                    else
                                    {
                                        string degree_code = splitvalue[0].ToString();
                                        string semester = splitvalue[1].ToString();
                                        string subject_no = splitvalue[2].ToString();
                                        loadunitssubj_no = subject_no;
                                        string batch_year = splitvalue[3].ToString();
                                        string secval = string.Empty;
                                        if (splitvalue.GetUpperBound(0) == 7)
                                        {
                                            secval = splitvalue[3];
                                            batch_year = splitvalue[4];
                                        }
                                        else
                                        {
                                            batch_year = splitvalue[3];
                                        }
                                        loadunitssubj_no = subject_no;
                                        Boolean hrlock = Hour_lock(degree_code, batch_year, semester, getcolheader, secval);
                                        if (hrlock == true)
                                        {
                                            Buttonsave.Visible = false;
                                            Buttonupdate.Visible = false;
                                            pHeaderatendence.Visible = false;
                                            pHeaderlesson.Visible = false;
                                            headerpanelnotes.Visible = false;
                                            pBodyatendence.Visible = false;
                                            pBodylesson.Visible = false;
                                            pBodynotes.Visible = false;
                                            pBodyquestionaddition.Visible = false;
                                            headerquestionaddition.Visible = false;
                                            headerADDQuestion.Visible = false;
                                            lbl_alert.Visible = true;
                                            //Added by srinath 25/8/2016 JPR
                                            lbl_alert.Text = " You cannot edit this day/Hour attendance due to security reasons. Get permission from PRINCIPAL to update the attendance";
                                            //Added by srinath 7/9/2013
                                            //  lbl_alert.Text = "You cannot edit this day/Hour attendance due to security reasons.Contact Inspro Plus Administrator";
                                            FpSpread2.Visible = false;
                                            Buttondeselect.Visible = false;
                                            Buttonselectall.Visible = false;
                                            lblmanysubject.Visible = false;
                                            ddlselectmanysub.Visible = false;
                                            return;
                                        }
                                        retrievespreadattendancequestion(batch_year, degree_code, semester, subject_no, sel_date1);
                                        retrivespreadfornotes(batch_year, degree_code, semester, subject_no, sel_date1);
                                        loadunitssubj_no = subject_no;
                                        sprdretrivedate();
                                        btnSave.Visible = true;
                                    }
                                }
                                else
                                {
                                    spreadatt_qtnadd.Visible = false;
                                    FpSpread3.Visible = false;
                                }
                                if (getcelltag != "")
                                {
                                    {
                                        if (ck_append.Checked == false)
                                        {
                                            load_attnd_spread();
                                            mark_attendance();
                                            checkleave = false;
                                        }
                                        else
                                        {
                                            if (FpSpread2.Sheets[0].ColumnCount != 8)
                                            {
                                                mark_attendance2();
                                                checkleave = false;
                                            }
                                            else
                                            {
                                                // load_attnd_spread();
                                                mark_attendance();
                                                checkleave = false;
                                            }
                                        }
                                    }
                                    rbgraphics.Checked = true;
                                    loadgraphics();
                                    if (slipfalg == true)
                                    {
                                        btnsliplist.Enabled = true;
                                    }
                                    else
                                    {
                                        btnsliplist.Enabled = false;
                                    }
                                    btnSave.Visible = true;
                                }
                                else
                                {
                                    checkleave = true;
                                    FpSpread2.Visible = false;
                                    pHeaderlesson.Visible = false;
                                    headerpanelnotes.Visible = false;
                                    headerADDQuestion.Visible = false;
                                    headerquestionaddition.Visible = false;
                                    pBodylesson.Visible = false;
                                    pHeaderatendence.Visible = false;
                                    pBodyatendence.Visible = false;
                                    pBodynotes.Visible = false;
                                    pBodyquestionaddition.Visible = false;
                                }
                            }
                            else
                            {
                                if (ck_append.Checked == false && FpSpread2.Visible == false)
                                {
                                    Buttonsave.Visible = false;
                                    Buttonupdate.Visible = false;
                                    pHeaderatendence.Visible = false;
                                    pHeaderlesson.Visible = false;
                                    headerpanelnotes.Visible = false;
                                    pBodyatendence.Visible = false;
                                    pBodylesson.Visible = false;
                                    pBodynotes.Visible = false;
                                    pBodyquestionaddition.Visible = false;
                                    headerquestionaddition.Visible = false;
                                    headerADDQuestion.Visible = false;
                                    pBodyaddquestion.Visible = false;
                                    if (cellclick1 == true)
                                    {
                                        lbl_alert.Visible = true;
                                        lbl_alert.Text = "Selected Day is Sunday Holiday ";
                                    }
                                }
                            }
                        }
                        else
                        {
                            Labelstaf.Visible = true;
                            Labelstaf.Text = "Select The Subject";
                            FpSpread2.Visible = false;
                            pHeaderlesson.Visible = false;
                            headerpanelnotes.Visible = false;
                            headerADDQuestion.Visible = false;
                            headerquestionaddition.Visible = false;
                            pBodylesson.Visible = false;
                            pHeaderatendence.Visible = false;
                            pBodyatendence.Visible = false;
                            pBodynotes.Visible = false;
                            pBodyquestionaddition.Visible = false;
                        }
                        load_presen_absent_count();
                        if (ddlselectmanysub.Visible == false)
                        {
                            divatt.Style.Value = "margin-left: 450px";
                        }
                        else
                        {
                            divatt.Style.Value = "margin-left: 150px";
                        }
                    }
                    //else
                    //{
                    //    Buttonsave.Visible = false;
                    //    Buttonupdate.Visible = false;
                    //    pHeaderatendence.Visible = false;
                    //    pHeaderlesson.Visible = false;
                    //    headerpanelnotes.Visible = false;
                    //    pBodyatendence.Visible = false;
                    //    pBodylesson.Visible = false;
                    //    pBodynotes.Visible = false;
                    //    pBodyquestionaddition.Visible = false;
                    //    headerquestionaddition.Visible = false;
                    //    headerADDQuestion.Visible = false;
                    //    pBodyaddquestion.Visible = false;
                    //    lbl_alert.Visible = true;
                    //    lbl_alert.Text = "Selected Hour is Holiday";
                    //}
                }
                if ((Session["StafforAdmin"] == "Staff") || (Session["StafforAdmin"] == "Admin"))
                {
                    lbl_alert.Visible = false;
                }
                //  clearfield();
                if (FpSpread1.Visible == true)
                {
                    if (FpSpread2.Sheets[0].RowCount > 1)
                    {
                    }
                    else
                    {
                        Buttonsave.Visible = false;
                        Buttonupdate.Visible = false;
                        pHeaderatendence.Visible = false;
                        pHeaderlesson.Visible = false;
                        headerpanelnotes.Visible = false;
                        pBodyatendence.Visible = false;
                        pBodylesson.Visible = false;
                        pBodynotes.Visible = false;
                        pBodyquestionaddition.Visible = false;
                        headerquestionaddition.Visible = false;
                        headerADDQuestion.Visible = false;
                        pBodyaddquestion.Visible = false;
                    }
                    if (ar >= 0 && ac >= 0)
                    {
                        string text_val1 = FpSpread1.Sheets[0].Cells[ar, ac].Text.Trim();
                        if (text_val1.Trim() == "")
                        {
                            if (ck_append.Checked == false)
                            {
                                Buttonsave.Visible = false;
                                Buttonupdate.Visible = false;
                                pHeaderatendence.Visible = false;
                                pHeaderlesson.Visible = false;
                                headerpanelnotes.Visible = false;
                                pBodyatendence.Visible = false;
                                pBodylesson.Visible = false;
                                pBodynotes.Visible = false;
                                pBodyquestionaddition.Visible = false;
                                headerquestionaddition.Visible = false;
                                headerADDQuestion.Visible = false;
                                pBodyaddquestion.Visible = false;
                                if (cellclick1 == true)
                                {
                                    //Modified by subburaj 19/8/2014******//
                                    lbl_alert.Visible = true;
                                    lbl_alert.Text = "Select Hour is Empty";
                                    //***********End*********//
                                }
                                FpSpread2.Sheets[0].RowCount = 0;
                                FpSpread2.Visible = false;
                            }
                            else if (ck_append.Checked == true && FpSpread2.Visible == false)
                            {
                                Buttonsave.Visible = false;
                                Buttonupdate.Visible = false;
                                pHeaderatendence.Visible = false;
                                pHeaderlesson.Visible = false;
                                headerpanelnotes.Visible = false;
                                pBodyatendence.Visible = false;
                                pBodylesson.Visible = false;
                                pBodynotes.Visible = false;
                                pBodyquestionaddition.Visible = false;
                                headerquestionaddition.Visible = false;
                                headerADDQuestion.Visible = false;
                                pBodyaddquestion.Visible = false;
                                if (cellclick1 == true)
                                {
                                    //Modified by subburaj 19/8/2014******//
                                    lbl_alert.Visible = true;
                                    lbl_alert.Text = "Select Hour is Empty";
                                    //***********End*********//
                                }
                                FpSpread2.Sheets[0].RowCount = 0;
                                FpSpread2.Visible = false;
                            }
                            else
                            {
                                if (cellclick1 == true)
                                {
                                    //Modified by subburaj 19/8/2014******//
                                    lbl_alert.Visible = true;
                                    lbl_alert.Text = "Select Hour is Empty";
                                    //***********End*********//
                                }
                            }
                        }
                        else
                        {
                            if (checkleave == true)
                            {
                                Buttonsave.Visible = false;
                                Buttonupdate.Visible = false;
                                pHeaderatendence.Visible = false;
                                pHeaderlesson.Visible = false;
                                headerpanelnotes.Visible = false;
                                pBodyatendence.Visible = false;
                                pBodylesson.Visible = false;
                                pBodynotes.Visible = false;
                                pBodyquestionaddition.Visible = false;
                                headerquestionaddition.Visible = false;
                                headerADDQuestion.Visible = false;
                                pBodyaddquestion.Visible = false;
                                lbl_alert.Visible = true;
                                lbl_alert.Text = "Selected Hour is Holiday";
                            }
                        }
                    }
                }
                if (Session["Copy Attendance"].ToString() == "1") { check_attendance.Visible = true; }
                else { check_attendance.Visible = false; }
                pnotesuploadadd.Visible = false;//Added By Srinath
                loadunits(loadunitssubj_no);
            }
            FpSpread2.CommandBar.Visible = false;
            FpSpread2.Sheets[0].AutoPostBack = false;
            FpSpread2.SaveChanges();
        }
        catch (Exception ex)
        {
            Labelstaf.Visible = true;
            Labelstaf.Text = ex.ToString();
        }
    }

    public void loadunits(string subject_no)
    {
        if (subject_no.Trim() != "")
        {
            string sqlunitsquery = "select * from sub_unit_details where subject_no='" + subject_no + "' and parent_code='0'";
            DataSet unitsds = new DataSet();
            unitsds.Clear();
            unitsds = da.select_method_wo_parameter(sqlunitsquery, "Text");
            if (unitsds.Tables.Count > 0 && unitsds.Tables[0].Rows.Count > 0)
            {
                ddlunits.DataTextField = "unit_name";
                ddlunits.DataValueField = "topic_no";
                ddlunits.DataSource = unitsds;
                ddlunits.DataBind();
                //ddlunitsobj.DataTextField = "unit_name";
                //ddlunitsobj.DataValueField = "topic_no";
                //ddlunitsobj.DataSource = unitsds;
                //ddlunitsobj.DataBind();
                ddlunits.Enabled = true;
                //ddlunitsobj.Enabled = true;
            }
            else
            {
                ddlunits.Enabled = false;
                //ddlunitsobj.Enabled = false;
            }
        }
    }

    public void load_attnd_spread()
    {
        FpSpread2.Sheets[0].ColumnCount = 0;
        FpSpread2.Sheets[0].RowCount = 0;
        FpSpread2.Sheets[0].ColumnHeader.RowCount = 2;
        FpSpread2.ActiveSheetView.SheetCornerSpanModel.Add(0, 0, 2, 1);
        FpSpread2.ActiveSheetView.SheetCorner.Cells[0, 0].Text = "S.No";
        FpSpread2.Sheets[0].ColumnCount = 7;
        FpSpread2.Sheets[0].SheetCornerSpanModel.Add(0, 0, 2, 1);
        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
        FpSpread2.Sheets[0].DefaultColumnWidth = 0;
        FpSpread2.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Select";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
        FpSpread2.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
        FpSpread2.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Student Type";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Con ( Attnd )";
        FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
        FpSpread2.Sheets[0].Columns[1].CellType = textcel_type;
        FpSpread2.Sheets[0].Columns[2].CellType = textcel_type;
        FpSpread2.Sheets[0].Columns[1].Locked = true;
        FpSpread2.Sheets[0].Columns[2].Locked = true;
        FpSpread2.Sheets[0].Columns[3].Locked = true;
        FpSpread2.Sheets[0].Columns[4].Locked = true;
        FpSpread2.Sheets[0].Columns[5].Locked = true;
        FpSpread2.Sheets[0].Columns[6].Locked = true;
        FarPoint.Web.Spread.ComboBoxCellType objintcell = new FarPoint.Web.Spread.ComboBoxCellType();
        //   strcomo = new string[] { "Select for All ", " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD" };
        //strcomo = new string[] { " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD" };//-------21/6/12 PRABHA
        //---------------------------------load rights
        string[] strcomo = new string[20];
        string[] attnd_rights1 = new string[100];
        int i = 0;
        string odrights = da.GetFunction("select rights from  OD_Master_Setting where " + grouporusercode + "");
        if (odrights.Trim() != null && odrights.Trim() != "" && odrights.Trim() != "0")
        {
            string od_rights = string.Empty;
            od_rights = odrights;
            string[] split_od_rights = od_rights.Split(',');
            strcomo = new string[split_od_rights.GetUpperBound(0) + 2];
            strcomo[i++] = string.Empty;
            for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
            {
                strcomo[i++] = split_od_rights[od_temp].ToString();
            }
        }
        else
        {
            strcomo[0] = string.Empty;
            strcomo[1] = "P";
            strcomo[2] = "A";
            strcomo[3] = "OD";
            strcomo[4] = "SOD";
            strcomo[5] = "ML";
            strcomo[6] = "NSS";
            strcomo[7] = "L";
            strcomo[8] = "NCC";
            strcomo[9] = "HS";
            strcomo[10] = "PP";
            strcomo[11] = "SYOD";
            strcomo[12] = "COD";
            strcomo[13] = "OOD";
            strcomo[14] = "LA";
        }
        //---------------------------
        objintcell = new FarPoint.Web.Spread.ComboBoxCellType(strcomo);
        objintcell.ShowButton = true;
        objintcell.AutoPostBack = true;
        objintcell.UseValue = true;
        FpSpread2.ActiveSheetView.Columns[0].CellType = objintcell;
        FpSpread2.SaveChanges();
        FpSpread2.Sheets[0].Columns[0].BackColor = Color.MistyRose;

        FpSpread2.Sheets[0].Columns[0].Visible = true;
        FpSpread2.Sheets[0].Columns[0].Width = 50;
        FpSpread2.Sheets[0].Columns[1].Visible = false;
        FpSpread2.Sheets[0].Columns[2].Visible = false;
        FpSpread2.Sheets[0].Columns[3].Visible = true;
        FpSpread2.Sheets[0].Columns[2].Width = 0;
        FpSpread2.Sheets[0].Columns[2].Width = 0;
        FpSpread2.Sheets[0].Columns[6].Width = 100;
        FpSpread2.Sheets[0].RowCount = 1;
        FpSpread2.Sheets[0].Cells[0, 0].CellType = textcel_type;
        FpSpread2.Sheets[0].RowHeader.Cells[0, 0].Text = " ";
    }

    public void mark_attendance()
    {
        try
        {
            dicFeeOfRollStudents = new Dictionary<string, DateTime[]>();
            dicFeeOnRollStudents = new Dictionary<string, byte>();
            GetFeeOfRollStudent(ref dicFeeOfRollStudents, ref dicFeeOnRollStudents);
            Boolean attavailable = false;
            chkis_studavailable.Checked = false;
            string get_alter_or_sem = string.Empty;
            Boolean tag_flag = false;
            Hashtable hatstudegree = new Hashtable();
            string[] split_tag_val;
            if (singlesubject == true)
            {
                split_tag_val = Convert.ToString(singlesubjectno).Split('*');
                FpSpread2.Sheets[0].RowCount = 1;
                FpSpread2.Sheets[0].ColumnCount = inicolcount;
            }
            else
            {
                split_tag_val = getcelltag.Split('*');
                FpSpread2.Sheets[0].RowCount = 1;
                if (cellclick1 == false)
                {
                    FpSpread2.Sheets[0].ColumnCount = inicolcount;
                }
                else
                {
                    inicolcount = Convert.ToInt16(FpSpread2.Sheets[0].ColumnCount);
                }
            }
            for (int tag_for = 0; tag_for <= split_tag_val.GetUpperBound(0); tag_for++)
            {
                str = split_tag_val[tag_for].ToString();
                string tempdegree = split_tag_val[tag_for].ToString();
                if (str != "")
                {
                    string[] sp1 = str.Split(new Char[] { '-' });
                    if (sp1[0].ToString().Trim() != "Selected day is Holiday" && sp1[0].ToString().Trim() != "")
                    {
                        string byear = string.Empty;
                        degree_code = sp1[0];
                        semester = sp1[1];
                        subject_no = sp1[2];
                        string batch_year = sp1[4].ToString();
                        //==============================================================================================
                        //  string check_lab = da.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subject_no + "'");
                        if (sp1.GetUpperBound(0) == 7)
                        {
                            sections = sp1[3];
                            byear = sp1[4];
                            subj_type = sp1[5];
                            subj_count_in_onehr = sp1[6];
                            get_alter_or_sem = sp1[7];
                        }
                        else
                        {
                            sections = string.Empty;
                            byear = sp1[3];
                            subj_type = sp1[4];
                            subj_count_in_onehr = sp1[5];
                            get_alter_or_sem = sp1[6];
                        }
                        //if (check_lab == "1" || check_lab == "True" || check_lab == "TRUE" || check_lab == "true")
                        //{
                        //    subj_type = "L";
                        //}
                        //else
                        //{
                        //    subj_type = "S";
                        //}
                        Session["deg_code"] = degree_code;
                        Session["semester"] = semester;
                        Session["sub_no"] = subject_no;
                        Session["sections"] = sections;
                        Session["batch_year"] = byear;
                        bool hrlock = Hour_lock(degree_code, batch_year, semester, getcolheader, sections);  //aruna 23july2013
                        if (hrlock == true)
                        {
                            Buttonsave.Visible = false;
                            Buttonupdate.Visible = false;
                            pHeaderatendence.Visible = false;
                            pHeaderlesson.Visible = false;
                            headerpanelnotes.Visible = false;
                            pBodyatendence.Visible = false;
                            pBodylesson.Visible = false;
                            pBodynotes.Visible = false;
                            pBodyquestionaddition.Visible = false;
                            headerquestionaddition.Visible = false;
                            headerADDQuestion.Visible = false;
                            lbl_alert.Visible = true;
                            //lbl_alert.Text = "You cannot edit this day/Hour attendance due to security reasons.Contact Inspro Plus Administrator";
                            //Added by srinath 25/8/2016 JPR
                            lbl_alert.Text = " You cannot edit this day/Hour attendance due to security reasons. Get permission from PRINCIPAL to update the attendance";
                            FpSpread2.Visible = false;
                            Buttondeselect.Visible = false;
                            Buttonselectall.Visible = false;
                            lblmanysubject.Visible = false;
                            ddlselectmanysub.Visible = false;
                            return;
                        }
                        chk = daycheck(Convert.ToDateTime(sel_date));
                        bool userDayLock = DayLockForUser(Convert.ToDateTime(sel_date));
                        if (!userDayLock)
                        {
                            Buttonsave.Visible = false;
                            Buttonupdate.Visible = false;
                            pHeaderatendence.Visible = false;
                            pHeaderlesson.Visible = false;
                            headerpanelnotes.Visible = false;
                            pBodyatendence.Visible = false;
                            pBodylesson.Visible = false;
                            pBodynotes.Visible = false;
                            pBodyquestionaddition.Visible = false;
                            lbl_alert.Visible = true;
                            //lbl_alert.Text = "You cannot edit this day/Hour attendance due to security reasons.Contact Inspro Plus Administrator";
                            //Added by srinath 25/8/2016 JPR
                            lbl_alert.Text = " You cannot edit this day attendance due to security reasons. Get permission from PRINCIPAL to update the attendance";
                            FpSpread2.Visible = false;
                            Buttondeselect.Visible = false;
                            Buttonselectall.Visible = false;
                            lblmanysubject.Visible = false;
                            ddlselectmanysub.Visible = false;
                            headerquestionaddition.Visible = false;
                            headerADDQuestion.Visible = false;
                            return;
                        }
                        else if (chk == false)
                        {
                            Buttonsave.Visible = false;
                            Buttonupdate.Visible = false;
                            pHeaderatendence.Visible = false;
                            pHeaderlesson.Visible = false;
                            headerpanelnotes.Visible = false;
                            pBodyatendence.Visible = false;
                            pBodylesson.Visible = false;
                            pBodynotes.Visible = false;
                            pBodyquestionaddition.Visible = false;
                            lbl_alert.Visible = true;
                            //lbl_alert.Text = "You cannot edit this day/Hour attendance due to security reasons.Contact Inspro Plus Administrator";
                            //Added by srinath 25/8/2016 JPR
                            lbl_alert.Text = " You cannot edit this day/Hour attendance due to security reasons. Get permission from PRINCIPAL to update the attendance";
                            FpSpread2.Visible = false;
                            Buttondeselect.Visible = false;
                            Buttonselectall.Visible = false;
                            lblmanysubject.Visible = false;
                            ddlselectmanysub.Visible = false;
                            headerquestionaddition.Visible = false;
                            headerADDQuestion.Visible = false;
                        }
                        else
                        {
                            getcolheader = FpSpread1.Sheets[0].ColumnHeader.Cells[0, ac].Text;
                            string temp_date = FpSpread1.Sheets[0].RowHeader.Cells[ar, 0].Text;
                            string[] spitdate = temp_date.Split(' ');
                            string[] date_split = spitdate[0].Split('-');
                            getdate = date_split[2] + "-" + date_split[1] + "-" + date_split[0];
                            hr = FpSpread1.Sheets[0].ColumnHeader.Cells[0, ac].Tag.ToString();
                            if (sections.ToString() != "" && sections.ToString() != "-1" && sections != null)
                            {
                                strsec = " and sections='" + sections.ToString() + "' ";
                            }
                            else
                            {
                                strsec = string.Empty;
                            }
                            //-------------------------------serial number check
                            Session["str_section"] = strsec;
                            Session["hr"] = ac + 1;
                            if (Session["Rollflag"].ToString() == "1")
                            {
                                FpSpread2.Sheets[0].ColumnHeader.Columns[1].Visible = true;
                                FpSpread2.Sheets[0].Columns[1].Width = 100;
                            }
                            if (Session["Regflag"].ToString() == "1")
                            {
                                FpSpread2.Sheets[0].ColumnHeader.Columns[2].Visible = true;
                                FpSpread2.Sheets[0].Columns[2].Width = 100;
                            }
                            if (Session["Studflag"].ToString() == "1")
                            {
                                FpSpread2.Sheets[0].ColumnHeader.Columns[5].Visible = true;
                                FpSpread2.Sheets[0].Columns[5].Width = 100;
                            }
                            FpSpread2.Sheets[0].Columns[4].Width = 220;
                            FpSpread1.Sheets[0].FrozenRowCount = 1;
                            //**************added By Srinath 29Jan2015
                            Session["StaffSelector"] = "0";   //" + Session["collegecode"].ToString() + "
                            string minimumabsentsms = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Session["collegecode"].ToString() + "'");
                            string[] splitminimumabsentsms = minimumabsentsms.Split('-');
                            if (splitminimumabsentsms.Length == 2)
                            {
                                int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
                                if (splitminimumabsentsms[0].ToString() == "1")
                                {
                                    if (Convert.ToInt32(byear) >= batchyearsetting)
                                    {
                                        Session["StaffSelector"] = "1";
                                    }
                                }
                            }
                            string strstaffselector = string.Empty;
                            if (Session["StaffSelector"].ToString() == "1")
                            {
                                strstaffselector = " and subjectchooser.staffcode like '%" + Session["Staff_Code"].ToString() + "%'";
                            }
                            string batchvalue = da.GetFunction("select Stud_batch from staff_selector where subject_no='" + subject_no + "' and staff_code='" + Session["Staff_Code"].ToString() + "' " + strsec + "");
                            if (batchvalue.Trim() != "" && batchvalue.Trim() != "0" && batchvalue.Trim() != "-1" && batchvalue != null)
                            {
                                batchvalue = " and batch='" + batchvalue + "'";
                            }
                            else
                            {
                                batchvalue = string.Empty;
                            }
                            Hashtable hatfeeroll = new Hashtable();
                            string strfeeofrollquery = "select r.Roll_No from stucon s,Registration r where s.roll_no=r.Roll_No and r.Current_Semester=s.semester and s.ack_fee_of_roll=1  and r.Batch_Year='" + byear + "' and r.degree_code=" + degree_code + " and r.Current_Semester='" + semester + "' and r.cc=0 and r.delflag=0 and r.Exam_Flag<>'debar' " + strsec + " ";
                            DataSet dsfeerol = da.select_method_wo_parameter(strfeeofrollquery, "text");
                            if (dsfeerol.Tables.Count > 0 && dsfeerol.Tables[0].Rows.Count > 0)
                            {
                                for (int fs = 0; fs < dsfeerol.Tables[0].Rows.Count; fs++)
                                {
                                    string feeofrolls = dsfeerol.Tables[0].Rows[fs]["Roll_No"].ToString().Trim().ToLower();
                                    if (!hatfeeroll.Contains(feeofrolls))
                                    {
                                        hatfeeroll.Add(feeofrolls, feeofrolls);
                                    }
                                }
                            }
                            string strorder = filterfunction();
                            string strstudentquery = string.Empty;
                            string timetable = string.Empty;
                            string gettimetable = da.GetFunction("Select top 1 ttname from semester_schedule where batch_year=" + byear + "  and degree_code=" + degree_code + " and semester='" + semester + "'  " + strsec + " and FromDate<='" + sel_date + "' order by FromDate Desc");                            //DataSet dsttname = da.select_method_wo_parameter(gettimetable, "text");
                            if (gettimetable.Trim() != null && gettimetable.Trim() != "" && gettimetable.Trim() != "0")
                            {
                                timetable = " and Timetablename='" + gettimetable + "'";
                            }
                            if (subj_type == "L")
                            {
                                if (get_alter_or_sem.Trim() == "alter")
                                {
                                    //strstudentquery = "select distinct registration.roll_no,registration.reg_no,registration.roll_admit ,registration.stud_name,registration.stud_type,registration.serialno from subjectchooser_New,sub_sem,subject,registration where fromdate='" + getdate + "' and  todate='" + getdate + "' and batch in(select   distinct stu_batch from laballoc_new where subject_no='" + subject_no + "'  and batch_year=" + byear + "  and hour_value=" + hr + "   " + strsec + "  and degree_code=" + degree_code + " and fdate='" + getdate + "' and  tdate='" + getdate + "' and day_value='" + Day_Var + "' )  and sub_sem.lab=1 and subjectchooser_New.subtype_no=sub_sem.subtype_no and  semester =  " + semester.ToString() + " and subjectchooser_New.subject_no=subject.subject_no and  registration.roll_no=subjectchooser_New.roll_no and  registration.current_semester=subjectchooser_New.semester and subjectchooser_New.subject_no='" + subject_no + "'   and adm_date<='" + sel_date + "'  and SubjectChooser_new.Semester=registration.current_semester " + strsec + " and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' " + strorder + "";
                                    strstudentquery = "select distinct registration.roll_no,registration.app_no,registration.reg_no,registration.roll_admit ,registration.stud_name,registration.stud_type,registration.serialno,registration.degree_code,Registration.college_code from subjectchooser_New,sub_sem,subject,registration where fromdate='" + getdate + "' and  todate='" + getdate + "' and batch in(select distinct stu_batch from laballoc_new where subject_no='" + subject_no + "'  and batch_year=" + byear + "  and hour_value='" + hr + "'   " + strsec + "  and degree_code=" + degree_code + " and fdate='" + getdate + "' and  tdate='" + getdate + "' and day_value='" + Day_Var + "' ) and subjectchooser_New.subtype_no=sub_sem.subtype_no and  semester =  " + semester.ToString() + " and subjectchooser_New.subject_no=subject.subject_no and  registration.roll_no=subjectchooser_New.roll_no and  registration.current_semester=subjectchooser_New.semester and subjectchooser_New.subject_no='" + subject_no + "'   and adm_date<='" + sel_date + "'  and SubjectChooser_new.Semester=registration.current_semester " + strsec + " and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' " + strorder + "";
                                }
                                else
                                {
                                    //strstudentquery = "select distinct registration.roll_no,registration.reg_no,registration.roll_admit ,registration.stud_name,registration.stud_type,registration.serialno from subjectchooser,sub_sem,subject,registration where batch in(select   stu_batch from laballoc where subject_no='" + subject_no + "'  and batch_year=" + byear + "  and hour_value=" + hr + "   " + strsec + "  and degree_code=" + degree_code + " and day_value='" + Day_Var + "' " + timetable + " )  and sub_sem.lab=1 and subjectchooser.subtype_no=sub_sem.subtype_no and  semester =  " + semester.ToString() + " and subjectchooser.subject_no=subject.subject_no and  registration.roll_no=subjectchooser.roll_no and  registration.current_semester=subjectchooser.semester and subjectchooser.subject_no='" + subject_no + "'  and  registration.roll_no= subjectchooser.roll_no and registration.batch_year=" + byear + "  and registration.degree_code=" + degree_code + " " + strsec + "     and adm_date<='" + sel_date + "' and SubjectChooser.Semester=registration.current_semester and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' " + strorder + "";
                                    strstudentquery = "select distinct registration.roll_no,registration.app_no,registration.reg_no,registration.roll_admit ,registration.stud_name,registration.stud_type,registration.serialno,registration.degree_code,Registration.college_code from subjectchooser,sub_sem,subject,registration where batch in(select stu_batch from laballoc where subject_no='" + subject_no + "'  and batch_year=" + byear + "  and hour_value=" + hr + "   " + strsec + "  and degree_code=" + degree_code + " and day_value='" + Day_Var + "' " + timetable + " ) and subjectchooser.subtype_no=sub_sem.subtype_no and  semester =  " + semester.ToString() + " and subjectchooser.subject_no=subject.subject_no and  registration.roll_no=subjectchooser.roll_no and  registration.current_semester=subjectchooser.semester and subjectchooser.subject_no='" + subject_no + "'  and  registration.roll_no= subjectchooser.roll_no and registration.batch_year=" + byear + "  and registration.degree_code=" + degree_code + " " + strsec + "     and adm_date<='" + sel_date + "' and SubjectChooser.Semester=registration.current_semester and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR'  " + strstaffselector + " " + batchvalue + " " + strorder + "";
                                }
                            }
                            else
                            {
                                strstudentquery = "Select distinct registration.roll_no,registration.app_no,registration.reg_no,registration.roll_admit ,registration.stud_name,registration.stud_type,registration.serialno,registration.degree_code,Registration.college_code from registration,SubjectChooser,applyn where registration.roll_no = subjectchooser.roll_no and registration.Degree_Code =" + degree_code + " and Semester = '" + semester + "' and Subject_No = '" + subject_no.ToString() + "' and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' and Semester = '" + semester + "' " + strsec + Session["strvar"].ToString() + "and registration.app_no=applyn.app_no" + "    and adm_date<='" + sel_date + "'  and SubjectChooser.Semester=registration.current_semester " + strstaffselector + " " + batchvalue + " " + strorder + "";
                            }
                            strstudentquery = strstudentquery + " ;select Distinct  Textval from textvaltable where textcriteria ='Attrs'";
                            strstudentquery = strstudentquery + " ; select rights from  OD_Master_Setting where " + grouporusercode + "";
                            strstudentquery = strstudentquery + " ; select c.course_name,de.dept_acronym,d.degree_code from degree d,department de,course c where d.dept_code=de.dept_code and c.course_id=d.course_id";
                            DataSet dsstudentquery = da.select_method_wo_parameter(strstudentquery, "Text");
                            if (dsstudentquery.Tables.Count > 0 && dsstudentquery.Tables[0].Rows.Count > 0)
                            {
                                attavailable = true;
                                chkis_studavailable.Checked = true;
                                for (int i = 0; i < dsstudentquery.Tables[0].Rows.Count; i++)
                                {
                                    if (tag_for == 0)
                                    {
                                        tag_flag = true;
                                    }
                                    check_record = true;
                                    if (!hatstudegree.Contains(Convert.ToString(dsstudentquery.Tables[0].Rows[i]["roll_no"]).Trim().ToLower()))
                                    {
                                        hatstudegree.Add(Convert.ToString(dsstudentquery.Tables[0].Rows[i]["roll_no"]).Trim().ToLower(), byear + "," + degree_code + "," + semester + "," + sections);
                                    }
                                    //   if (!hatfeeroll.Contains(dsstudentquery.Tables[0].Rows[i]["roll_no"].ToString().Trim().ToLower()))
                                    // {
                                    FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 1;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = dsstudentquery.Tables[0].Rows[i]["roll_no"].ToString();
                                    //barath 26.01.17
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Tag = dsstudentquery.Tables[0].Rows[i]["college_code"].ToString();
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Note = dsstudentquery.Tables[0].Rows[i]["app_no"].ToString();
                                    FpSpread2.Sheets[0].RowHeader.Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = (FpSpread2.Sheets[0].RowCount - 1).ToString();
                                    int l = FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text.Length;
                                    if (len < l)
                                        len = l;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = dsstudentquery.Tables[0].Rows[i]["reg_no"].ToString();
                                    int l1 = FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text.Length;
                                    if (len1 < l1)
                                        len1 = l1;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = dsstudentquery.Tables[0].Rows[i]["roll_admit"].ToString();
                                    int l2 = FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text.Length;
                                    if (len2 < l2)
                                        len2 = l2;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = dsstudentquery.Tables[0].Rows[i]["stud_name"].ToString();
                                    int l3 = FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text.Length;
                                    if (len3 < 3)
                                        len3 = l3;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = dsstudentquery.Tables[0].Rows[i]["stud_type"].ToString();
                                    int l4 = FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text.Length;
                                    if (len4 < l4)
                                        len4 = l4;
                                    if (dsstudentquery.Tables[0].Rows[i]["stud_type"].ToString() == "Hostler")
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].BackColor = Color.LightYellow;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].BackColor = Color.LightYellow;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].BackColor = Color.LightYellow;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].BackColor = Color.LightYellow;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].BackColor = Color.LightYellow;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].BackColor = Color.LightYellow;
                                    }
                                    else
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].BackColor = Color.MediumSeaGreen;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].BackColor = Color.MediumSeaGreen;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].BackColor = Color.MediumSeaGreen;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].BackColor = Color.MediumSeaGreen;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].BackColor = Color.MediumSeaGreen;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].BackColor = Color.MediumSeaGreen;
                                    }
                                    //    }
                                    //if (hatfeeroll.Contains(dsstudentquery.Tables[0].Rows[i]["roll_no"].ToString().Trim().ToLower()))
                                    //{
                                    //    FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Locked = true;
                                    //    FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].BackColor = Color.Red;
                                    //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].BackColor = Color.Red;
                                    //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].BackColor = Color.Red;
                                    //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].BackColor = Color.Red;
                                    //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].BackColor = Color.Red;
                                    //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].BackColor = Color.Red;
                                    //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].BackColor = Color.Red;
                                    //}
                                    string rollNo = string.Empty;
                                    bool checkedFeeOfRoll = false;
                                    rollNo = Convert.ToString(dsstudentquery.Tables[0].Rows[i]["roll_no"]).Trim().ToLower();
                                    if (dicFeeOfRollStudents.ContainsKey(rollNo.Trim()) && dicFeeOnRollStudents.ContainsKey(rollNo.Trim()))
                                    {
                                        DateTime[] dtFeeOfRoll = dicFeeOfRollStudents[rollNo.Trim()];
                                        DateTime dtSelDate = new DateTime();
                                        dtSelDate = Convert.ToDateTime(getdate);
                                        string dtadntdate = da.GetFunction("select adm_date from registration where Roll_No ='" + rollNo + "'");
                                        DateTime dtadm = Convert.ToDateTime(dtadntdate);
                                        if (dtadm <= dtSelDate)
                                        {
                                            if (dtSelDate >= dtFeeOfRoll[0])
                                            {
                                                DateTime dtDefaultDate = new DateTime(1900, 1, 1);//SqlServer Default Date
                                                if (dicFeeOnRollStudents[rollNo.Trim()] == 0 && dtSelDate < dtFeeOfRoll[1])
                                                {
                                                    checkedFeeOfRoll = true;
                                                }
                                                else if (dicFeeOnRollStudents[rollNo.Trim()] == 1)
                                                {
                                                    checkedFeeOfRoll = true;
                                                }
                                                else if (dicFeeOnRollStudents[rollNo.Trim()] == 0 && dtFeeOfRoll[1] == dtDefaultDate)
                                                {
                                                    checkedFeeOfRoll = true;
                                                }
                                                //else if (dicFeeOnRollStudents[rollNo.Trim()] == 0 && dtSelDate >= dtFeeOfRoll[1])
                                                //{
                                                //    checkedFeeOfRoll = false;
                                                //}
                                                else
                                                {
                                                    checkedFeeOfRoll = false;
                                                }
                                            }
                                            else
                                            {
                                                checkedFeeOfRoll = false;
                                            }
                                        }
                                        else
                                        {
                                            checkedFeeOfRoll = false;
                                        }
                                    }
                                    if (checkedFeeOfRoll)
                                    {
                                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Locked = true;
                                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].BackColor = Color.Red;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].BackColor = Color.Red;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].BackColor = Color.Red;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].BackColor = Color.Red;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].BackColor = Color.Red;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].BackColor = Color.Red;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].BackColor = Color.Red;
                                    }
                                }
                            }
                            // DataSet dsval = da.select_method_wo_parameter(getvalquery, "Text");
                            string odrights = string.Empty;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Tag = getdate;
                            if (check_record == true)
                            {
                                if (sp1.GetUpperBound(0) == 7)
                                {
                                    sections = sp1[3];
                                    byear = sp1[4];
                                    subj_type = sp1[5];
                                    subj_count_in_onehr = sp1[6];
                                    get_alter_or_sem = sp1[7];
                                }
                                else
                                {
                                    sections = string.Empty;
                                    byear = sp1[3];
                                    subj_type = sp1[4];
                                    subj_count_in_onehr = sp1[5];
                                    get_alter_or_sem = sp1[6];
                                }
                                if (sections.ToString() != "" && sections.ToString() != "-1" && sections != null)
                                {
                                    strsec = " and sections='" + sections.ToString() + "' ";
                                }
                                else
                                {
                                    strsec = string.Empty;
                                }
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, 1].Tag = getcelltag.ToString();
                                if (tag_for == 0 || FpSpread2.Sheets[0].ColumnCount <= 7)
                                {
                                    FpSpread2.Sheets[0].SpanModel.Add(0, 0, 1, 6);
                                    FpSpread2.Sheets[0].Cells[0, 0].Locked = true;
                                    FpSpread2.Sheets[0].ColumnCount = FpSpread2.Sheets[0].ColumnCount + 1;
                                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = 50;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = sel_date1;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = getcolheader;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Tag = hr;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Tag = getdate;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Note = FpSpread1.Sheets[0].Cells[ar, ac].Tag.ToString();
                                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                                    FpSpread2.Sheets[0].AutoPostBack = false;
                                    FarPoint.Web.Spread.ComboBoxCellType objintcell = new FarPoint.Web.Spread.ComboBoxCellType();
                                    FarPoint.Web.Spread.ComboBoxCellType objcom = new FarPoint.Web.Spread.ComboBoxCellType();
                                    string[] strcomo = new string[20];
                                    string[] strcomo1 = new string[20];
                                    string[] attnd_rights1 = new string[100];
                                    int i = 0, j = 0;
                                    //string odrights = da.GetFunction("select rights from  OD_Master_Setting where " + grouporusercode + "");
                                    odrights = dsstudentquery.Tables[2].Rows[0][0].ToString();
                                    if (odrights.Trim() != null && odrights.Trim() != "" && odrights.Trim() != "0")
                                    {
                                        string od_rights = string.Empty;
                                        od_rights = odrights;
                                        string[] split_od_rights = od_rights.Split(',');
                                        strcomo = new string[split_od_rights.GetUpperBound(0) + 2];
                                        strcomo1 = new string[split_od_rights.GetUpperBound(0) + 3];
                                        strcomo1[j++] = "Select for All";
                                        strcomo1[j++] = string.Empty;
                                        strcomo[i++] = string.Empty;
                                        for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                                        {
                                            strcomo[i++] = split_od_rights[od_temp].ToString();
                                            strcomo1[j++] = split_od_rights[od_temp].ToString();
                                        }
                                    }
                                    else
                                    {
                                        strcomo[0] = string.Empty;
                                        strcomo[1] = "P";
                                        strcomo[2] = "A";
                                        strcomo[3] = "OD";
                                        strcomo[4] = "SOD";
                                        strcomo[5] = "ML";
                                        strcomo[6] = "NSS";
                                        strcomo[7] = "L";
                                        strcomo[8] = "NCC";
                                        strcomo[9] = "HS";
                                        strcomo[10] = "PP";
                                        strcomo[11] = "SYOD";
                                        strcomo[12] = "COD";
                                        strcomo[13] = "OOD";
                                        strcomo[14] = "LA";
                                        strcomo1[0] = "Select for All";
                                        strcomo1[1] = string.Empty;
                                        strcomo1[2] = "P";
                                        strcomo1[3] = "A";
                                        strcomo1[4] = "OD";
                                        strcomo1[5] = "SOD";
                                        strcomo1[6] = "ML";
                                        strcomo1[7] = "NSS";
                                        strcomo1[8] = "L";
                                        strcomo1[9] = "NCC";
                                        strcomo1[10] = "HS";
                                        strcomo1[11] = "PP";
                                        strcomo1[12] = "SYOD";
                                        strcomo1[13] = "COD";
                                        strcomo1[14] = "OOD";
                                        strcomo1[15] = "LA";
                                    }

                                    objintcell = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1);
                                    objintcell.ShowButton = true;
                                    objintcell.AutoPostBack = true;
                                    objintcell.UseValue = true;
                                    FpSpread2.ActiveSheetView.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].CellType = objintcell;
                                    FpSpread2.SaveChanges();
                                    objcom = new FarPoint.Web.Spread.ComboBoxCellType(strcomo);
                                    objcom.AutoPostBack = true;
                                    objcom.UseValue = true;
                                    objcom.ShowButton = false;
                                    FpSpread2.SaveChanges();
                                    FpSpread2.ActiveSheetView.Columns[FpSpread2.Sheets[0].ColumnCount - 1].CellType = objcom;
                                    FpSpread2.SaveChanges();
                                    FpSpread2.Sheets[0].ColumnCount = FpSpread2.Sheets[0].ColumnCount + 1;
                                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - 2, 1, 2);
                                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = 60;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString("Reason");
                                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                                }
                                if (tag_for == 1 && tag_flag == false)
                                {
                                    lbl_alert.Visible = true;
                                    lbl_alert.Text = "Please Allocate Batch For The Selected Class Students";
                                    FpSpread2.Sheets[0].SpanModel.Add(0, 1, 1, 4);
                                    if (FpSpread2.Sheets[0].ColumnCount == 6)
                                    {
                                        FpSpread2.Sheets[0].ColumnCount = FpSpread2.Sheets[0].ColumnCount + 1;
                                    }
                                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = 50;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = sel_date1;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = getcolheader;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Tag = hr;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Tag = getdate;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Note = FpSpread1.Sheets[0].Cells[ar, ac].Tag.ToString();
                                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                                    FpSpread2.Sheets[0].AutoPostBack = false;
                                    FarPoint.Web.Spread.ComboBoxCellType objintcell = new FarPoint.Web.Spread.ComboBoxCellType();
                                    FarPoint.Web.Spread.ComboBoxCellType objcom = new FarPoint.Web.Spread.ComboBoxCellType();
                                    string[] strcomo = new string[20];
                                    string[] strcomo1 = new string[20];
                                    string[] attnd_rights1 = new string[100];
                                    int i = 0, j = 0;
                                    //string odrights = da.GetFunction("select rights from  OD_Master_Setting where " + grouporusercode + "");
                                    if (odrights.Trim() != null && odrights.Trim() != "" && odrights.Trim() != "0")
                                    {
                                        string[] split_od_rights = odrights.Split(',');
                                        strcomo = new string[split_od_rights.GetUpperBound(0) + 2];
                                        strcomo1 = new string[split_od_rights.GetUpperBound(0) + 3];
                                        strcomo1[j++] = "Select for All";
                                        strcomo1[j++] = string.Empty;
                                        strcomo[i++] = string.Empty;
                                        for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                                        {
                                            strcomo[i++] = split_od_rights[od_temp].ToString();
                                            strcomo1[j++] = split_od_rights[od_temp].ToString();
                                        }
                                    }
                                    else
                                    {
                                        strcomo[0] = string.Empty;
                                        strcomo[1] = "P";
                                        strcomo[2] = "A";
                                        strcomo[3] = "OD";
                                        strcomo[4] = "SOD";
                                        strcomo[5] = "ML";
                                        strcomo[6] = "NSS";
                                        strcomo[7] = "L";
                                        strcomo[8] = "NCC";
                                        strcomo[9] = "HS";
                                        strcomo[10] = "PP";
                                        strcomo[11] = "SYOD";
                                        strcomo[12] = "COD";
                                        strcomo[13] = "OOD";
                                        strcomo[14] = "LA";
                                        strcomo1[0] = "Select for All";
                                        strcomo1[1] = string.Empty;
                                        strcomo1[2] = "P";
                                        strcomo1[3] = "A";
                                        strcomo1[4] = "OD";
                                        strcomo1[5] = "SOD";
                                        strcomo1[6] = "ML";
                                        strcomo1[7] = "NSS";
                                        strcomo1[8] = "L";
                                        strcomo1[9] = "NCC";
                                        strcomo1[10] = "HS";
                                        strcomo1[11] = "PP";
                                        strcomo1[12] = "SYOD";
                                        strcomo1[13] = "COD";
                                        strcomo1[14] = "LA";
                                    }
                                    //---------------------------
                                    objintcell = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1);
                                    objintcell.ShowButton = true;
                                    objintcell.AutoPostBack = true;
                                    objintcell.UseValue = true;
                                    FpSpread2.ActiveSheetView.Cells[0, 6].CellType = objintcell;
                                    FpSpread2.SaveChanges();
                                    objcom = new FarPoint.Web.Spread.ComboBoxCellType(strcomo);
                                    objcom.AutoPostBack = true;
                                    objcom.UseValue = true;
                                    objcom.ShowButton = false;
                                    FpSpread2.SaveChanges();
                                    FpSpread2.ActiveSheetView.Columns[FpSpread2.Sheets[0].ColumnCount - 1].CellType = objcom;
                                    FpSpread2.SaveChanges();
                                    if (FpSpread2.Sheets[0].ColumnCount == 7)
                                    {
                                        FpSpread2.Sheets[0].ColumnCount = FpSpread2.Sheets[0].ColumnCount + 1;
                                    }
                                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - 2, 1, 2);
                                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = 50;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString("Reason");
                                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                                }
                                ArrayList al_reason = new ArrayList();
                                if (dsstudentquery.Tables.Count > 1 && dsstudentquery.Tables[1].Rows.Count > 0)
                                {
                                    for (int i = 0; i < dsstudentquery.Tables[1].Rows.Count; i++)
                                    {
                                        al_reason.Add(dsstudentquery.Tables[1].Rows[i]["Textval"].ToString());
                                    }
                                }
                                string[] reason = new string[al_reason.Count + 2];
                                string[] reason1 = new string[al_reason.Count + 1];
                                reason[0] = "Select for All";
                                reason[1] = string.Empty;
                                reason1[0] = string.Empty;
                                for (int r = 1; r <= al_reason.Count; r++)
                                {
                                    reason[r + 1] = al_reason[r - 1].ToString();
                                    reason1[r] = al_reason[r - 1].ToString();
                                }
                                FarPoint.Web.Spread.ComboBoxCellType objintcell10 = new FarPoint.Web.Spread.ComboBoxCellType(reason);
                                objintcell10.ShowButton = true;
                                objintcell10.AutoPostBack = true;
                                objintcell10.UseValue = true;
                                FpSpread2.ActiveSheetView.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].CellType = objintcell10;
                                FpSpread2.SaveChanges();
                                FarPoint.Web.Spread.ComboBoxCellType objintcol = new FarPoint.Web.Spread.ComboBoxCellType();
                                objintcol = new FarPoint.Web.Spread.ComboBoxCellType(reason1);
                                FpSpread2.SaveChanges();
                                objintcol.UseValue = true;
                                objintcol.ShowButton = false; ;
                                objintcol.AutoPostBack = true;
                                FpSpread2.ActiveSheetView.Columns[FpSpread2.Sheets[0].ColumnCount - 1].CellType = objintcol;
                                FpSpread2.SaveChanges();
                                int Att_mark_row;
                                string str_Date;
                                string str_day;
                                string Atmonth;
                                string Atyear;
                                long strdate;
                                string Att_str_hour;
                                string rollno_Att = string.Empty;
                                string Att_dcolumn = string.Empty;
                                string Att_Markvalue;
                                string Att_Mark1;
                                int temp = 0;
                                string Sliptempdegree = string.Empty;
                                string slipdegreedetails = string.Empty;
                                string getdate56 = FpSpread1.Sheets[0].RowHeader.Cells[ar, 0].Text;
                                string[] spitdate56 = getdate56.Split(' ');
                                str_Date = spitdate56[0];
                                Att_str_hour = Convert.ToString(ac + 1);
                                string[] split = str_Date.Split(new Char[] { '-' });
                                str_day = split[0].ToString();
                                Atmonth = split[1].ToString();
                                Atyear = split[2].ToString();
                                strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                                Att_dcolumn = "d" + str_day + "d" + Att_str_hour;
                                int splitdate = Convert.ToInt32(split[0]);
                                int splitmonth = Convert.ToInt32(split[1]);
                                int splityear = Convert.ToInt32(split[2]);
                                string concat_susdate = splitmonth + "/" + splitdate + "/" + splityear;
                                //Increase Speed for load Attendance Modified by srinath 23/9/2013
                                string attendancequery = "Select a." + Att_dcolumn + " as Attendance, registration.roll_no from registration,Attendance a  where  a.roll_no=registration.Roll_No and registration.RollNo_Flag<>0 and registration.cc=0 and registration.delflag=0 and registration.exam_flag <>'DEBAR'";
                                attendancequery = attendancequery + " and registration.Batch_year='" + byear + "' and registration.Degree_Code = " + degree_code + " and registration.current_semester = '" + semester + "'  " + strsec + " and adm_date<='" + concat_susdate + "'  and  a.month_year=" + strdate + "";
                                //and Registration.college_code=a.Att_CollegeCode
                                attendancequery = attendancequery + " ;Select a." + Att_dcolumn + " as Reason, registration.roll_no from registration,Attendance_withreason a  where a.roll_no=registration.Roll_No and registration.RollNo_Flag<>0 and registration.cc=0 and registration.delflag=0 and registration.exam_flag <>'DEBAR' and a." + Att_dcolumn + "<>'' and a." + Att_dcolumn + " is not null";
                                attendancequery = attendancequery + " and registration.Batch_year='" + byear + "' and registration.Degree_Code = " + degree_code + " and registration.current_semester = '" + semester + "' " + strsec + " and adm_date<='" + concat_susdate + "'  and  a.month_year=" + strdate + " ";
                                attendancequery = attendancequery + " ;select  convert(varchar(15),dateadd(day,tot_days-1,ack_date),1) as action_days,ack_date,tot_days,s.roll_no from stucon s,Registration where Registration.Roll_No=s.roll_no and registration.Batch_year='" + byear + "' and Registration.Degree_Code = " + degree_code + " and Registration.current_semester = '" + semester + "'  and ack_susp=1 and tot_days>0 and Registration.CC=0 and Registration.DelFlag=0 and Registration.Exam_Flag<>'Debar' and ack_date<='" + concat_susdate + "'";
                                DataSet dsattendance = da.select_method_wo_parameter(attendancequery, "Text");
                                hatroll.Clear();
                                if (dsattendance.Tables.Count > 0 && dsattendance.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < dsattendance.Tables[0].Rows.Count; i++)
                                    {
                                        string rollNo = string.Empty;
                                        bool checkedFeeOfRoll = false;
                                        rollNo = Convert.ToString(dsattendance.Tables[0].Rows[i]["Roll_no"]).Trim().ToLower();
                                        if (dicFeeOfRollStudents.ContainsKey(rollNo.Trim()) && dicFeeOnRollStudents.ContainsKey(rollNo.Trim()))
                                        {
                                            DateTime[] dtFeeOfRoll = dicFeeOfRollStudents[rollNo.Trim()];
                                            DateTime dtSelDate = new DateTime();
                                            dtSelDate = Convert.ToDateTime(concat_susdate);
                                            string dtadntdate = da.GetFunction("select adm_date from registration where Roll_No ='" + rollNo + "'");
                                            DateTime dtadm = Convert.ToDateTime(dtadntdate);
                                            if (dtadm <= dtSelDate)
                                            {
                                                if (dtSelDate >= dtFeeOfRoll[0])
                                                {
                                                    DateTime dtDefaultDate = new DateTime(1900, 1, 1);//SqlServer Default Date
                                                    if (dicFeeOnRollStudents[rollNo.Trim()] == 0 && dtSelDate < dtFeeOfRoll[1])
                                                    {
                                                        checkedFeeOfRoll = true;
                                                    }
                                                    else if (dicFeeOnRollStudents[rollNo.Trim()] == 1)
                                                    {
                                                        checkedFeeOfRoll = true;
                                                    }
                                                    else if (dicFeeOnRollStudents[rollNo.Trim()] == 0 && dtFeeOfRoll[1] == dtDefaultDate)
                                                    {
                                                        checkedFeeOfRoll = true;
                                                    }
                                                    //else if (dicFeeOnRollStudents[rollNo.Trim()] == 0 && dtSelDate >= dtFeeOfRoll[1])
                                                    //{
                                                    //    checkedFeeOfRoll = false;
                                                    //}
                                                    else
                                                    {
                                                        checkedFeeOfRoll = false;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                checkedFeeOfRoll = false;
                                            }
                                        }
                                        if (dsattendance.Tables[0].Rows[i]["attendance"].ToString().Trim() != null && dsattendance.Tables[0].Rows[i]["attendance"].ToString().Trim() != "")
                                        {
                                            if (!checkedFeeOfRoll)
                                            {
                                                if (!hatroll.Contains(Convert.ToString(dsattendance.Tables[0].Rows[i]["Roll_no"]).Trim().ToLower()))
                                                {
                                                    hatroll.Add(Convert.ToString(dsattendance.Tables[0].Rows[i]["Roll_no"]).Trim().ToLower(), Convert.ToString(dsattendance.Tables[0].Rows[i]["Attendance"]).Trim());
                                                }
                                            }
                                            else
                                            {
                                                //if (!hatroll.Contains(Convert.ToString(dsattendance.Tables[0].Rows[i]["Roll_no"]).Trim()))
                                                //{
                                                //    hatroll.Add(Convert.ToString(dsattendance.Tables[0].Rows[i]["Roll_no"]).Trim(), "2");
                                                //}
                                            }
                                        }
                                        //else if(checkedFeeOfRoll)
                                        //{
                                        //    if (!hatroll.Contains(Convert.ToString(dsattendance.Tables[0].Rows[i]["Roll_no"]).Trim()))
                                        //    {
                                        //        hatroll.Add(Convert.ToString(dsattendance.Tables[0].Rows[i]["Roll_no"]).Trim(), "2");
                                        //    }
                                        //}
                                    }
                                }
                                Hashtable hatreason = new Hashtable();
                                if (dsattendance.Tables.Count > 1 && dsattendance.Tables[1].Rows.Count > 0)
                                {
                                    for (int i = 0; i < dsattendance.Tables[1].Rows.Count; i++)
                                    {
                                        if (dsattendance.Tables[1].Rows[i]["Reason"].ToString().Trim() != null && dsattendance.Tables[1].Rows[i]["Reason"].ToString().Trim() != "")
                                        {
                                            if (!hatreason.Contains(Convert.ToString(dsattendance.Tables[1].Rows[i]["Roll_no"]).Trim().ToLower()))
                                            {
                                                hatreason.Add(Convert.ToString(dsattendance.Tables[1].Rows[i]["Roll_no"]).Trim().ToLower(), dsattendance.Tables[1].Rows[i]["Reason"].ToString());
                                            }
                                        }
                                    }
                                }
                                Hashtable hatsuspend = new Hashtable();
                                if (dsattendance.Tables.Count > 2 && dsattendance.Tables[2].Rows.Count > 0)
                                {
                                    for (int s = 0; s < dsattendance.Tables[2].Rows.Count; s++)
                                    {
                                        if (!hatsuspend.Contains(Convert.ToString(dsattendance.Tables[2].Rows[s]["Roll_no"]).Trim().ToLower()))
                                        {
                                            hatsuspend.Add(Convert.ToString(dsattendance.Tables[2].Rows[s]["Roll_no"]).Trim().ToLower(), dsattendance.Tables[2].Rows[s]["action_days"].ToString() + '^' + dsattendance.Tables[2].Rows[s]["ack_date"].ToString() + '^' + dsattendance.Tables[2].Rows[s]["tot_days"].ToString());
                                        }
                                    }
                                }
                                for (Att_mark_row = 1; Att_mark_row < FpSpread2.Sheets[0].RowCount; Att_mark_row++)
                                {
                                    rollno_Att = FpSpread2.Sheets[0].Cells[Att_mark_row, 1].Text.ToString();
                                    FpSpread2.Sheets[0].Cells[Att_mark_row, 7].Locked = false;
                                    string rollNo = string.Empty;
                                    bool checkedFeeOfRoll = false;
                                    rollNo = Convert.ToString(rollno_Att).Trim().ToLower();
                                    if (dicFeeOfRollStudents.ContainsKey(rollNo.Trim()) && dicFeeOnRollStudents.ContainsKey(rollNo.Trim()))
                                    {
                                        DateTime[] dtFeeOfRoll = dicFeeOfRollStudents[rollNo.Trim()];
                                        DateTime dtSelDate = new DateTime();
                                        dtSelDate = Convert.ToDateTime(concat_susdate);
                                        string dtadntdate = da.GetFunction("select adm_date from registration where Roll_No ='" + rollNo + "'");
                                        DateTime dtadm = Convert.ToDateTime(dtadntdate);
                                        if (dtadm <= dtSelDate)
                                        {
                                            if (dtSelDate >= dtFeeOfRoll[0])
                                            {
                                                DateTime dtDefaultDate = new DateTime(1900, 1, 1);//SqlServer Default Date
                                                if (dicFeeOnRollStudents[rollNo.Trim()] == 0 && dtSelDate < dtFeeOfRoll[1])
                                                {
                                                    checkedFeeOfRoll = true;
                                                }
                                                else if (dicFeeOnRollStudents[rollNo.Trim()] == 1)
                                                {
                                                    checkedFeeOfRoll = true;
                                                }
                                                else if (dicFeeOnRollStudents[rollNo.Trim()] == 0 && dtFeeOfRoll[1] == dtDefaultDate)
                                                {
                                                    checkedFeeOfRoll = true;
                                                }
                                                //else if (dicFeeOnRollStudents[rollNo.Trim()] == 0 && dtSelDate >= dtFeeOfRoll[1])
                                                //{
                                                //    checkedFeeOfRoll = false;
                                                //}
                                                else
                                                {
                                                    checkedFeeOfRoll = false;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            checkedFeeOfRoll = false;
                                        }
                                    }
                                    //if (hatfeeroll.Contains(rollno_Att.Trim().ToLower()))
                                    //{
                                    //    FarPoint.Web.Spread.TextCellType tc = new FarPoint.Web.Spread.TextCellType();
                                    //    FpSpread2.Sheets[0].Cells[Att_mark_row, 7].CellType = tc;
                                    //    FpSpread2.Sheets[0].Cells[Att_mark_row, 7].Text = "A";
                                    //    FpSpread2.Sheets[0].Cells[Att_mark_row, 7].Locked = true;
                                    //    FpSpread2.Sheets[0].Cells[Att_mark_row, 7].Tag = "2";
                                    //}
                                    if (checkedFeeOfRoll)
                                    {
                                        FarPoint.Web.Spread.TextCellType tc = new FarPoint.Web.Spread.TextCellType();
                                        FpSpread2.Sheets[0].Cells[Att_mark_row, 7].CellType = tc;
                                        FpSpread2.Sheets[0].Cells[Att_mark_row, 7].Text = "A";
                                        FpSpread2.Sheets[0].Cells[Att_mark_row, 7].Locked = true;
                                        FpSpread2.Sheets[0].Cells[Att_mark_row, 7].Tag = "2";
                                    }
                                    else
                                    {
                                        if (hatsuspend.Contains(rollno_Att.Trim().ToLower()))
                                        {
                                            DateTime dt_curr = Convert.ToDateTime(concat_susdate.ToString());
                                            string values = GetCorrespondingKey(rollno_Att.Trim().ToLower(), hatsuspend).ToString();
                                            if (values.Trim() != null && values.Trim() != "")
                                            {
                                                string[] strspiltvalues = values.Split('^');
                                                if (strspiltvalues.GetUpperBound(0) > 1)
                                                {
                                                    string actiondate = strspiltvalues[0].ToString();
                                                    string ackdate = strspiltvalues[1].ToString();
                                                    long totalactdays = Convert.ToInt32(strspiltvalues[2].ToString());
                                                    DateTime dt_act = Convert.ToDateTime(actiondate);
                                                    DateTime dt_curr1 = Convert.ToDateTime(ackdate);
                                                    TimeSpan t_con = dt_act.Subtract(dt_curr);
                                                    long daycon = t_con.Days;
                                                    DateTime dt_act1 = Convert.ToDateTime(concat_susdate.ToString());
                                                    TimeSpan t_con1 = dt_act1.Subtract(dt_curr1);
                                                    long daycon1 = t_con1.Days;
                                                    if ((Convert.ToInt32(daycon + daycon1) == totalactdays - 1) && (daycon > 0))
                                                    {
                                                        FarPoint.Web.Spread.TextCellType tc = new FarPoint.Web.Spread.TextCellType();
                                                        FpSpread2.Sheets[0].Cells[Att_mark_row, 7].CellType = tc;
                                                        FpSpread2.Sheets[0].Cells[Att_mark_row, 7].Text = "S";
                                                        FpSpread2.Sheets[0].Cells[Att_mark_row, 7].Locked = true;
                                                        FpSpread2.Sheets[0].Cells[Att_mark_row, 7].Tag = "9";
                                                    }
                                                    else
                                                    {
                                                        if (hatroll.Contains(rollno_Att.Trim().ToLower()))
                                                        {
                                                            Att_Markvalue = GetCorrespondingKey(rollno_Att.Trim().ToLower(), hatroll).ToString();
                                                            if (Att_Markvalue.Trim() != null && Att_Markvalue.Trim() != "" && Att_Markvalue.Trim() != "0")
                                                            {
                                                                Att_Mark1 = Attmark(Att_Markvalue);
                                                                FpSpread2.Sheets[0].Cells[Att_mark_row, 7].Note = Att_Markvalue.ToString();
                                                                FpSpread2.Sheets[0].SetText(Att_mark_row, 7, Att_Mark1.ToString());
                                                                if (hatreason.Contains(rollno_Att.Trim().ToLower()))
                                                                {
                                                                    Att_Markvalue = GetCorrespondingKey(rollno_Att.Trim().ToLower(), hatreason).ToString();
                                                                    FpSpread2.Sheets[0].SetText(Att_mark_row, 8, Att_Markvalue.ToString());
                                                                }
                                                                FpSpread2.Sheets[0].AutoPostBack = false;
                                                                if (Att_Mark1 != "")
                                                                {
                                                                    temp = temp + 1;
                                                                    Buttonupdate.Enabled = true;
                                                                    Buttonsave.Enabled = false;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Att_Markvalue = string.Empty;
                                            if (hatroll.Contains(rollno_Att.Trim().ToLower()))
                                            {
                                                Att_Markvalue = GetCorrespondingKey(rollno_Att.Trim().ToLower(), hatroll).ToString();
                                                if (Att_Markvalue.Trim() != null && Att_Markvalue.Trim() != "" && Att_Markvalue.Trim() != "0")
                                                {
                                                    Att_Mark1 = Attmark(Att_Markvalue);
                                                    FpSpread2.Sheets[0].Cells[Att_mark_row, 7].Note = Att_Markvalue.ToString();
                                                    FpSpread2.Sheets[0].SetText(Att_mark_row, 7, Att_Mark1.ToString());
                                                    FpSpread2.Sheets[0].AutoPostBack = false;
                                                }
                                                else
                                                {
                                                    slipfalg = true;
                                                    Sliptempdegree = tempdegree;
                                                    if (hatstudegree.Contains(rollno_Att.Trim().ToLower()))
                                                    {
                                                        string degree = hatstudegree[rollno_Att.Trim().ToLower()].ToString();
                                                        string[] spde = degree.Split(',');
                                                        if (spde.GetUpperBound(0) >= 3)
                                                        {
                                                            dsstudentquery.Tables[3].DefaultView.RowFilter = " degree_code='" + spde[1].ToString() + "'";
                                                            DataView dvdegree = dsstudentquery.Tables[3].DefaultView;
                                                            if (dvdegree.Count > 0)
                                                            {
                                                                slipdegreedetails = spde[0] + "-" + dvdegree[0]["course_name"].ToString() + "-" + dvdegree[0]["dept_acronym"].ToString() + "-" + spde[2] + "";
                                                                if (spde[3].ToString().Trim() != "" && spde[3].ToString().Trim() != null && spde[3].ToString().Trim() != "-1")
                                                                {
                                                                    slipdegreedetails = slipdegreedetails + "-" + spde[3].ToString();
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (hatreason.Contains(rollno_Att.Trim().ToLower()))
                                            {
                                                Att_Markvalue = GetCorrespondingKey(rollno_Att.Trim().ToLower(), hatreason).ToString();
                                                FpSpread2.Sheets[0].SetText(Att_mark_row, 8, Att_Markvalue.ToString());
                                            }
                                            if (Att_Markvalue.Trim() != null && Att_Markvalue.Trim() != "" && Att_Markvalue.Trim() != "0")
                                            {
                                                temp = temp + 1;
                                                Buttonupdate.Enabled = true;
                                                Buttonsave.Enabled = false;
                                            }
                                            else
                                            {
                                                slipfalg = true;
                                                Sliptempdegree = tempdegree;
                                                if (hatstudegree.Contains(rollno_Att))
                                                {
                                                    string degree = hatstudegree[rollno_Att].ToString();
                                                    string[] spde = degree.Split(',');
                                                    if (spde.GetUpperBound(0) >= 3)
                                                    {
                                                        dsstudentquery.Tables[3].DefaultView.RowFilter = " degree_code='" + spde[1].ToString() + "'";
                                                        DataView dvdegree = dsstudentquery.Tables[3].DefaultView;
                                                        if (dvdegree.Count > 0)
                                                        {
                                                            slipdegreedetails = spde[0] + "-" + dvdegree[0]["Course_Name"].ToString() + "-" + dvdegree[0]["dept_acronym"].ToString() + "-" + spde[2] + "";
                                                            if (spde[3].ToString().Trim() != "" && spde[3].ToString().Trim() != null && spde[3].ToString().Trim() != "-1")
                                                            {
                                                                slipdegreedetails = slipdegreedetails + "-" + spde[3].ToString();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    FpSpread2.Sheets[0].Cells[Att_mark_row, 5].Tag = slipdegreedetails;
                                }
                                if (temp > 0)
                                {
                                    Buttonsave.Visible = false;
                                    Buttonupdate.Visible = true;
                                }
                                Labelstaf.Visible = false;
                                FpSpread2.SaveChanges();
                                FpSpread2.Visible = true;
                                Buttonselectall.Visible = true;
                                Buttondeselect.Visible = true;
                                FpSpread2.Sheets[0].RowHeader.Width = 45;
                                FpSpread2.Visible = true;
                                Panelcomplete.Visible = true;
                                Panelyet.Visible = true;
                                filltree();
                                FpSpread2.Visible = true;
                                FpSpread1.Visible = true;
                                pHeaderlesson.Visible = true;
                                headerpanelnotes.Visible = true;
                                pBodylesson.Visible = true;
                                pHeaderatendence.Visible = true;
                                pBodyatendence.Visible = true;
                                pHeaderatendence.Visible = true;
                                pHeaderlesson.Visible = true;
                                pBodynotes.Visible = true;
                                pBodyquestionaddition.Visible = true;
                                headerADDQuestion.Visible = true;
                                headerquestionaddition.Visible = true;
                                pBodyatendence.Visible = true;
                                pBodylesson.Visible = true;
                                lbl_alert.Visible = false;
                                Buttonsave.Visible = true;
                                Buttonupdate.Visible = true;
                                pHeaderatendence.Visible = true;
                                pHeaderlesson.Visible = true;
                                Buttonsave.Visible = true;
                            }
                            else
                            {
                                if (ddlselectmanysub.Visible = true && ddlselectmanysub.Items.Count > 0)
                                {
                                    if (get_alter_or_sem.Trim() == "alter" || subj_type == "L")
                                    {
                                        lbl_alert.Visible = true;
                                        lbl_alert.Text = "Please Allocate Batch For The Selected Class Students";
                                    }
                                    else
                                    {
                                        lbl_alert.Visible = true;
                                        lbl_alert.Text = "No Students Found";
                                    }
                                }
                                else
                                {
                                    lbl_alert.Visible = true;
                                    lbl_alert.Text = "Please Allocate Batch For The Selected Class Students";
                                    pBodyatendence.Visible = false;
                                    pHeaderatendence.Visible = false;
                                }
                            }
                        }
                    }
                }
            }
            if (attavailable == true)
            {
                load_attendance();
            }
        }
        catch (Exception ex)
        {
            Labelstaf.Visible = true;
            Labelstaf.Text = ex.ToString();
        }
    }

    public void mark_attendance2()
    {
        dicFeeOfRollStudents = new Dictionary<string, DateTime[]>();
        dicFeeOnRollStudents = new Dictionary<string, byte>();
        GetFeeOfRollStudent(ref dicFeeOfRollStudents, ref dicFeeOnRollStudents);
        Boolean hr_bool = false;
        Boolean flag_sameclass = false;
        if (FpSpread2.Visible == true)
        {
            if (chkis_studavailable.Checked == true || chkis_studavailable.Checked == false)
            {
                DateTime dtt1 = Convert.ToDateTime(getdate);
                DateTime dtt2 = Convert.ToDateTime(FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Tag.ToString());
                //if (getdate_new == FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Tag.ToString())
                // if (getdate == FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Tag.ToString())
                if (dtt1.ToString("MM/dd/yyyy") == dtt2.ToString("MM/dd/yyyy"))
                {
                    flag_sameclass = true;
                    string previoustag = FpSpread2.Sheets[0].ColumnHeader.Cells[1, 1].Tag.ToString();
                    if (getcelltag.Trim().ToLower() != previoustag.Trim().ToLower())
                    {
                        flag_sameclass = false;
                    }
                    //if (getcelltag == FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Note.ToString())
                    if (flag_sameclass == true)
                    //================================================================================                 
                    {
                        for (int check_hr = 7; check_hr <= FpSpread2.Sheets[0].ColumnCount - 1; check_hr = check_hr + 2)
                        {
                            if (hr == FpSpread2.Sheets[0].ColumnHeader.Cells[1, check_hr].Tag.ToString())
                            {
                                hr_bool = true;
                            }
                            else
                            {
                            }
                        }
                        if (hr_bool == false)
                        {
                            chk = daycheck(Convert.ToDateTime(sel_date));
                            if (chk == false)
                            {
                                //FpSpread1.Visible = false;
                                Buttonsave.Visible = false;
                                Buttonupdate.Visible = false;
                                pHeaderatendence.Visible = false;
                                pHeaderlesson.Visible = false;
                                pBodyatendence.Visible = false;
                                pBodylesson.Visible = false;
                                pBodynotes.Visible = false;
                                pBodyquestionaddition.Visible = false;
                                lbl_alert.Visible = true;
                                //lblmanysubject.Visible = false;//--------14/6/12 PRABHA
                                //ddlselectmanysub.Visible = false;//--------14/6/12 PRABHA
                                //Added by srinath 25/8/2016 JPR
                                lbl_alert.Text = " You cannot edit this day/Hour attendance due to security reasons. Get permission from PRINCIPAL to update the attendance";
                            }
                            else
                            {
                                string temp_date = FpSpread1.Sheets[0].RowHeader.Cells[ar, 0].Text;
                                string[] spitdate = temp_date.Split(' ');
                                sel_date = spitdate[0].ToString();
                                getcolheader = FpSpread1.Sheets[0].ColumnHeader.Cells[0, ac].Text;
                                //if (tag_for == 0)
                                {
                                    FpSpread2.Sheets[0].SpanModel.Add(0, 1, 1, 4);
                                    FpSpread2.Sheets[0].ColumnCount = FpSpread2.Sheets[0].ColumnCount + 1;
                                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = 50;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = sel_date;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = getcolheader;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Tag = hr;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Tag = getdate;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Note = FpSpread1.Sheets[0].Cells[ar, ac].Tag.ToString();
                                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                                    FpSpread2.Sheets[0].AutoPostBack = false;
                                    //string[] strcomo;
                                    //string[] strcomo1;
                                    FarPoint.Web.Spread.ComboBoxCellType objintcell = new FarPoint.Web.Spread.ComboBoxCellType();
                                    FarPoint.Web.Spread.ComboBoxCellType objcom = new FarPoint.Web.Spread.ComboBoxCellType();
                                    //strcomo1 = new string[] { "Select for All ", " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD" };//----21/6/12 PRABHA
                                    //strcomo = new string[] { " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD" };
                                    //---------------------------------load rights
                                    string[] strcomo = new string[20];
                                    string[] strcomo1 = new string[20];
                                    string[] attnd_rights1 = new string[100];
                                    int i = 0, j = 0;
                                    string odrights = da.GetFunction("select rights from  OD_Master_Setting where " + grouporusercode + "");
                                    if (odrights.Trim() != null && odrights.Trim() != "" && odrights.Trim() != "0")
                                    {
                                        string[] split_od_rights = odrights.Split(',');
                                        strcomo = new string[split_od_rights.GetUpperBound(0) + 2];
                                        strcomo1 = new string[split_od_rights.GetUpperBound(0) + 3];
                                        strcomo1[j++] = "Select for All";
                                        strcomo1[j++] = string.Empty;
                                        strcomo[i++] = string.Empty;
                                        for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                                        {
                                            strcomo[i++] = split_od_rights[od_temp].ToString();
                                            strcomo1[j++] = split_od_rights[od_temp].ToString();
                                        }
                                    }
                                    else
                                    {
                                        // "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD" 
                                        strcomo[0] = string.Empty;
                                        strcomo[1] = "P";
                                        strcomo[2] = "A";
                                        strcomo[3] = "OD";
                                        strcomo[4] = "SOD";
                                        strcomo[5] = "ML";
                                        strcomo[6] = "NSS";
                                        strcomo[7] = "L";
                                        strcomo[8] = "NCC";
                                        strcomo[9] = "HS";
                                        strcomo[10] = "PP";
                                        strcomo[11] = "SYOD";
                                        strcomo[12] = "COD";
                                        strcomo[13] = "OOD";
                                        strcomo[14] = "LA";
                                        // "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD" 
                                        strcomo1[0] = "Select for All";
                                        strcomo1[1] = string.Empty;
                                        strcomo1[2] = "P";
                                        strcomo1[3] = "A";
                                        strcomo1[4] = "OD";
                                        strcomo1[5] = "SOD";
                                        strcomo1[6] = "ML";
                                        strcomo1[7] = "NSS";
                                        strcomo1[8] = "L";
                                        strcomo1[9] = "NCC";
                                        strcomo1[10] = "HS";
                                        strcomo1[11] = "PP";
                                        strcomo1[12] = "SYOD";
                                        strcomo1[13] = "COD";
                                        strcomo1[14] = "OOD";
                                        strcomo1[15] = "LA";
                                    }
                                    //---------------------------
                                    objintcell = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1);
                                    objintcell.ShowButton = true;
                                    objintcell.AutoPostBack = true;
                                    objintcell.UseValue = true;
                                    FpSpread2.ActiveSheetView.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].CellType = objintcell;
                                    FpSpread2.SaveChanges();
                                    objcom = new FarPoint.Web.Spread.ComboBoxCellType(strcomo);
                                    objcom.AutoPostBack = true;
                                    objcom.UseValue = true;
                                    objcom.ShowButton = false;
                                    FpSpread2.SaveChanges();
                                    FpSpread2.ActiveSheetView.Columns[FpSpread2.Sheets[0].ColumnCount - 1].CellType = objcom;
                                    FpSpread2.SaveChanges();
                                    FpSpread2.Sheets[0].ColumnCount = FpSpread2.Sheets[0].ColumnCount + 1;
                                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - 2, 1, 2);
                                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = 60;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString("Reason");
                                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                                }
                                string querytext;
                                ArrayList al_reason = new ArrayList();
                                querytext = "select Distinct  Textval from textvaltable where textcriteria ='Attrs'";
                                DataSet dstextval = da.select_method(querytext, hat, "Text");
                                if (dstextval.Tables.Count > 0 && dstextval.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < dstextval.Tables[0].Rows.Count; i++)
                                    {
                                        al_reason.Add(dstextval.Tables[0].Rows[i]["Textval"].ToString());
                                    }
                                }
                                string[] reason = new string[al_reason.Count + 2];
                                string[] reason1 = new string[al_reason.Count + 1];
                                reason[0] = "Select for All";
                                reason[1] = string.Empty;
                                reason1[0] = string.Empty;
                                for (int r = 1; r <= al_reason.Count; r++)
                                {
                                    reason[r + 1] = al_reason[r - 1].ToString();
                                    reason1[r] = al_reason[r - 1].ToString();
                                }
                                FarPoint.Web.Spread.ComboBoxCellType objintcell10 = new FarPoint.Web.Spread.ComboBoxCellType(reason);
                                objintcell10.ShowButton = true;
                                objintcell10.AutoPostBack = true;
                                objintcell10.UseValue = true;
                                FpSpread2.ActiveSheetView.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].CellType = objintcell10;
                                FpSpread2.SaveChanges();
                                FarPoint.Web.Spread.ComboBoxCellType objintcol = new FarPoint.Web.Spread.ComboBoxCellType();
                                objintcol = new FarPoint.Web.Spread.ComboBoxCellType(reason1);
                                objintcol.ShowButton = false;
                                objintcol.AutoPostBack = true;
                                objintcol.UseValue = true;
                                FpSpread2.ActiveSheetView.Columns[FpSpread2.Sheets[0].ColumnCount - 1].CellType = objintcol;
                                FpSpread2.SaveChanges();
                                int Att_mark_row;
                                string str_Date;
                                string str_day;
                                string Atmonth;
                                string Atyear;
                                long strdate;
                                string Att_str_hour;
                                string rollno_Att = string.Empty;
                                string Att_dcolumn = string.Empty;
                                string Att_strqueryst = string.Empty;
                                string Att_Markvalue;
                                string Att_Mark1;
                                int temp = 0;
                                string[] split_tag_val;
                                //Increase Speed for load Attendance Modified by srinath 23/9/2013
                                if (singlesubject == true)
                                {
                                    split_tag_val = Convert.ToString(singlesubjectno).Split('*');
                                    FpSpread2.Sheets[0].RowCount = 1;
                                    FpSpread2.Sheets[0].ColumnCount = inicolcount;
                                }
                                else
                                {
                                    split_tag_val = getcelltag.Split('*');
                                    if (cellclick1 == false)
                                    {
                                        FpSpread2.Sheets[0].ColumnCount = inicolcount;
                                    }
                                    else
                                    {
                                        inicolcount = Convert.ToInt16(FpSpread2.Sheets[0].ColumnCount);
                                    }
                                }
                                for (int att_mark_col = 7 + 2; att_mark_col <= FpSpread2.Sheets[0].ColumnCount - 2; att_mark_col = att_mark_col + 2)
                                {
                                    string get_alter_or_sem = string.Empty;
                                    Boolean tag_flag = false;
                                    string byear = string.Empty;
                                    string strorder = filterfunction();
                                    for (int tag_for = 0; tag_for <= split_tag_val.GetUpperBound(0); tag_for++)
                                    {
                                        string tempdegree =
                                       str = split_tag_val[tag_for].ToString();
                                        if (str != "")
                                        {
                                            string[] sp1 = str.Split(new Char[] { '-' });
                                            if (sp1[0].ToString().Trim() != "Selected day is Holiday" && sp1[0].ToString().Trim() != "")
                                            {
                                                if (sp1.GetUpperBound(0) == 7)
                                                {
                                                    sections = sp1[3];
                                                    byear = sp1[4];
                                                    subj_count_in_onehr = sp1[6];
                                                    get_alter_or_sem = sp1[7];
                                                    degree_code = sp1[0];
                                                    subject_no = sp1[2];
                                                    semester = sp1[1];
                                                }
                                                else
                                                {
                                                    degree_code = sp1[0];
                                                    sections = string.Empty;
                                                    byear = sp1[3];
                                                    subj_count_in_onehr = sp1[5];
                                                    get_alter_or_sem = sp1[6];
                                                    subject_no = sp1[2];
                                                    semester = sp1[1];
                                                }
                                                bool hrlock = Hour_lock(degree_code, byear, semester, getcolheader, sections);  //aruna 23july2013
                                                if (hrlock == true)
                                                {
                                                    Buttonsave.Visible = false;
                                                    Buttonupdate.Visible = false;
                                                    pHeaderatendence.Visible = false;
                                                    pHeaderlesson.Visible = false;
                                                    headerpanelnotes.Visible = false;
                                                    pBodyatendence.Visible = false;
                                                    pBodylesson.Visible = false;
                                                    pBodynotes.Visible = false;
                                                    pBodyquestionaddition.Visible = false;
                                                    headerquestionaddition.Visible = false;
                                                    headerADDQuestion.Visible = false;
                                                    lbl_alert.Visible = true;
                                                    lbl_alert.Text = "You cannot edit this day/Hour attendance due to security reasons.Contact Inspro Plus Administrator";
                                                    FpSpread2.Visible = false;
                                                    Buttondeselect.Visible = false;
                                                    Buttonselectall.Visible = false;
                                                    lblmanysubject.Visible = false;
                                                    ddlselectmanysub.Visible = false;
                                                    return;
                                                }
                                                string check_lab = da.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subject_no + "'");
                                                if (check_lab == "1" || check_lab == "True" || check_lab == "TRUE" || check_lab == "true")
                                                {
                                                    subj_type = "L";
                                                }
                                                else
                                                {
                                                    subj_type = "S";
                                                }
                                                Att_str_hour = FpSpread2.Sheets[0].ColumnHeader.Cells[1, att_mark_col].Tag.ToString();
                                                string getdateact = FpSpread1.Sheets[0].RowHeader.Cells[ar, 0].Text;
                                                string[] spgetdateact = getdateact.Split(' ');
                                                str_Date = spgetdateact[0].ToString();
                                                string[] split = str_Date.Split(new Char[] { '-' });
                                                str_day = split[0].ToString();
                                                Atmonth = split[1].ToString();
                                                Atyear = split[2].ToString();
                                                strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                                                Att_dcolumn = "d" + str_day + "d" + Att_str_hour;
                                                string concat_susdate = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                                                Session["StaffSelector"] = "0";    //" + Session["collegecode"].ToString() + "
                                                string minimumabsentsms = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Session["collegecode"].ToString() + "'");
                                                string[] splitminimumabsentsms = minimumabsentsms.Split('-');
                                                if (splitminimumabsentsms.Length == 2)
                                                {
                                                    int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
                                                    if (splitminimumabsentsms[0].ToString() == "1")
                                                    {
                                                        if (Convert.ToInt32(byear) >= batchyearsetting)
                                                        {
                                                            Session["StaffSelector"] = "1";
                                                        }
                                                    }
                                                }
                                                string strstaffselector = string.Empty;
                                                if (Session["StaffSelector"].ToString() == "1")
                                                {
                                                    strstaffselector = " and subjectchooser.staffcode like '%" + Session["Staff_Code"].ToString() + "%'";
                                                }
                                                string attendancequery = "Select a." + Att_dcolumn + " as Attendance, registration.roll_no,registration.reg_no ,registration.stud_name,registration.serialno from registration,SubjectChooser s,applyn,Attendance a  where registration.roll_no = s.roll_no and  s.Semester=registration.current_semester and registration.app_no=applyn.app_no and s.roll_no=a.roll_no and a.roll_no=registration.Roll_No and registration.RollNo_Flag<>0 and registration.cc=0 and registration.delflag=0 and registration.exam_flag <>'DEBAR'";
                                                attendancequery = attendancequery + " and registration.Batch_year='" + byear + "' and registration.Degree_Code = " + degree_code + " and registration.current_semester = '" + semester + "' and s.Subject_No = '" + subject_no.ToString() + "'and s.Semester = '" + semester + "'  " + strsec + " " + Session["strvar"].ToString() + "and adm_date<='" + concat_susdate + "'  and  a.month_year=" + strdate + " " + strorder + "";
                                                DataSet dsattendance = da.select_method(attendancequery, hat, "Text");
                                                Hashtable hatroll = new Hashtable();
                                                if (dsattendance.Tables.Count > 0 && dsattendance.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int i = 0; i < dsattendance.Tables[0].Rows.Count; i++)
                                                    {
                                                        if (dsattendance.Tables[0].Rows[i]["attendance"].ToString().Trim() != null && dsattendance.Tables[0].Rows[i]["attendance"].ToString().Trim() != "")
                                                        {
                                                            if (!hatroll.Contains(dsattendance.Tables[0].Rows[i]["Roll_no"].ToString().Trim().ToLower()))
                                                            {
                                                                hatroll.Add(dsattendance.Tables[0].Rows[i]["Roll_no"].ToString().Trim().ToLower(), dsattendance.Tables[0].Rows[i]["Attendance"].ToString());
                                                            }
                                                        }
                                                    }
                                                }
                                                Hashtable hatreason = new Hashtable();
                                                string attendacereason = "Select a." + Att_dcolumn + " as Reason, registration.roll_no,registration.reg_no ,registration.stud_name,registration.serialno from registration,SubjectChooser s,applyn,Attendance_withreason a  where registration.roll_no = s.roll_no and  s.Semester=registration.current_semester and registration.app_no=applyn.app_no and s.roll_no=a.roll_no and a.roll_no=registration.Roll_No and registration.RollNo_Flag<>0 and registration.cc=0 and registration.delflag=0 and registration.exam_flag <>'DEBAR' and a." + Att_dcolumn + "<>'' and a." + Att_dcolumn + " is not null";
                                                attendacereason = attendacereason + " and registration.Batch_year='" + byear + "' and registration.Degree_Code = " + degree_code + " and registration.current_semester = '" + semester + "' and s.Subject_No = '" + subject_no.ToString() + "'and s.Semester = '" + semester + "'  " + strsec + " " + Session["strvar"].ToString() + "and adm_date<='" + concat_susdate + "'  and  a.month_year=" + strdate + " " + strorder + "";
                                                DataSet daattreason = da.select_method(attendacereason, hat, "Text");
                                                if (daattreason.Tables.Count > 0 && daattreason.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int i = 0; i < daattreason.Tables[0].Rows.Count; i++)
                                                    {
                                                        if (daattreason.Tables[0].Rows[i]["Reason"].ToString().Trim() != null && daattreason.Tables[0].Rows[i]["Reason"].ToString().Trim() != "")
                                                        {
                                                            if (!hatreason.Contains(daattreason.Tables[0].Rows[i]["Roll_no"].ToString().Trim().ToLower()))
                                                            {
                                                                hatreason.Add(daattreason.Tables[0].Rows[i]["Roll_no"].ToString().Trim().ToLower(), daattreason.Tables[0].Rows[i]["Reason"].ToString());
                                                            }
                                                        }
                                                    }
                                                }
                                                string strsuspend = "select  convert(varchar(15),dateadd(day,tot_days-1,ack_date),1) as action_days,ack_date,tot_days,s.roll_no from stucon s,Registration where Registration.Roll_No=s.roll_no and registration.Batch_year='" + byear + "' and Registration.Degree_Code = " + degree_code + " and Registration.current_semester = '" + semester + "'  and ack_susp=1 and tot_days>0 and Registration.CC=0 and Registration.DelFlag=0 and Registration.Exam_Flag<>'Debar' and ack_date<='" + concat_susdate + "'";
                                                DataSet dssuspend = da.select_method(strsuspend, hat, "Text");
                                                Hashtable hatsuspend = new Hashtable();
                                                if (dssuspend.Tables.Count > 0 && dssuspend.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int s = 0; s < dssuspend.Tables[0].Rows.Count; s++)
                                                    {
                                                        if (!hatsuspend.Contains(dssuspend.Tables[0].Rows[s]["Roll_no"].ToString().Trim().ToLower()))
                                                        {
                                                            hatsuspend.Add(dssuspend.Tables[0].Rows[s]["Roll_no"].ToString().Trim().ToLower(), dssuspend.Tables[0].Rows[s]["action_days"].ToString() + '^' + dssuspend.Tables[0].Rows[s]["ack_date"].ToString() + '^' + dssuspend.Tables[0].Rows[s]["tot_days"].ToString());
                                                        }
                                                    }
                                                }
                                                //for (int att_mark_col = 6 + 2; att_mark_col <= FpSpread2.Sheets[0].ColumnCount - 2; att_mark_col = att_mark_col + 2)
                                                //{
                                                //    Att_str_hour = FpSpread2.Sheets[0].ColumnHeader.Cells[1, att_mark_col].Tag.ToString();
                                                for (Att_mark_row = 1; Att_mark_row < FpSpread2.Sheets[0].RowCount - 2; Att_mark_row++)
                                                {
                                                    FpSpread2.SaveChanges();
                                                    {
                                                        rollno_Att = FpSpread2.Sheets[0].Cells[Att_mark_row, 1].Text.ToString();
                                                        if ((FpSpread2.Sheets[0].Rows[Att_mark_row].BackColor == Color.Red)) // 30-12-2016 (Jayaraman)
                                                        {
                                                            FarPoint.Web.Spread.TextCellType tc = new FarPoint.Web.Spread.TextCellType();
                                                            FpSpread2.Sheets[0].Cells[Att_mark_row, att_mark_col].CellType = tc;
                                                            FpSpread2.Sheets[0].Cells[Att_mark_row, att_mark_col].Text = "A";
                                                            FpSpread2.Sheets[0].Cells[Att_mark_row, att_mark_col].Locked = true;
                                                            FpSpread2.Sheets[0].Cells[Att_mark_row, att_mark_col].Tag = "2";
                                                        }
                                                        else
                                                        {
                                                            if (hatsuspend.Contains(rollno_Att.Trim().ToLower()))
                                                            {
                                                                DateTime dt_curr = Convert.ToDateTime(concat_susdate.ToString());
                                                                string values = GetCorrespondingKey(rollno_Att.Trim().ToLower(), hatsuspend).ToString();
                                                                if (values.Trim() != null && values.Trim() != "")
                                                                {
                                                                    string[] strspiltvalues = values.Split('^');
                                                                    if (strspiltvalues.GetUpperBound(0) > 1)
                                                                    {
                                                                        string actiondate = strspiltvalues[0].ToString();
                                                                        string ackdate = strspiltvalues[1].ToString();
                                                                        long totalactdays = Convert.ToInt32(strspiltvalues[2].ToString());
                                                                        DateTime dt_act = Convert.ToDateTime(actiondate);
                                                                        DateTime dt_curr1 = Convert.ToDateTime(ackdate);
                                                                        // DateTime dt_act = Convert.ToDateTime(ds_suspend.Tables[0].Rows[0]["action_days"].ToString());
                                                                        TimeSpan t_con = dt_act.Subtract(dt_curr);
                                                                        long daycon = t_con.Days;
                                                                        // DateTime dt_curr1 = Convert.ToDateTime(ds_suspend.Tables[0].Rows[0]["ack_date"].ToString());
                                                                        DateTime dt_act1 = Convert.ToDateTime(concat_susdate.ToString());
                                                                        TimeSpan t_con1 = dt_act1.Subtract(dt_curr1);
                                                                        long daycon1 = t_con1.Days;
                                                                        //DateTime dt_curr = Convert.ToDateTime(concat_susdate.ToString());
                                                                        //DateTime dt_act = Convert.ToDateTime(ds_suspend.Tables[0].Rows[0]["action_days"].ToString());
                                                                        //TimeSpan t_con = dt_act.Subtract(dt_curr);
                                                                        //long daycon = t_con.Days;
                                                                        // DateTime dt_curr1 = Convert.ToDateTime(ds_suspend.Tables[0].Rows[0]["ack_date"].ToString());
                                                                        // DateTime dt_act1 = Convert.ToDateTime(concat_susdate.ToString());
                                                                        // TimeSpan t_con1 = dt_act1.Subtract(dt_curr1);
                                                                        //  long daycon1 = t_con1.Days;
                                                                        //long totalactdays = Convert.ToInt32(ds_suspend.Tables[0].Rows[0]["tot_days"]);
                                                                        if ((Convert.ToInt32(daycon + daycon1) == totalactdays - 1) && (daycon > 0))
                                                                        {
                                                                            FarPoint.Web.Spread.TextCellType tc = new FarPoint.Web.Spread.TextCellType();
                                                                            FpSpread2.Sheets[0].Cells[Att_mark_row, att_mark_col].CellType = tc;
                                                                            FpSpread2.Sheets[0].Cells[Att_mark_row, att_mark_col].Text = "S";
                                                                            FpSpread2.Sheets[0].Cells[Att_mark_row, att_mark_col].Locked = true;
                                                                            FpSpread2.Sheets[0].Cells[Att_mark_row, att_mark_col].Tag = "9";
                                                                        }
                                                                        else
                                                                        {
                                                                            //Att_strqueryst = "select " + Att_dcolumn + " from Attendance where Roll_no='" + rollno_Att.ToString() + "' and month_year=" + strdate.ToString() + "";
                                                                            if (hatroll.Contains(rollno_Att.Trim().ToLower()))
                                                                            {
                                                                                Att_Markvalue = GetCorrespondingKey(rollno_Att.Trim().ToLower(), hatroll).ToString();
                                                                                Att_Markvalue = da.GetFunction(Att_strqueryst);
                                                                                Att_Mark1 = Attmark(Att_Markvalue);
                                                                                FpSpread2.Sheets[0].Cells[Att_mark_row, att_mark_col].Note = Att_Markvalue.ToString();
                                                                                FpSpread2.Sheets[0].SetText(Att_mark_row, att_mark_col, Att_Mark1.ToString());
                                                                                FpSpread2.Sheets[0].AutoPostBack = false;
                                                                                if (Att_Mark1 != "")
                                                                                {
                                                                                    temp = temp + 1;
                                                                                }
                                                                            }
                                                                            if (hatreason.Contains(rollno_Att.Trim().ToLower()))
                                                                            {
                                                                                Att_Markvalue = GetCorrespondingKey(rollno_Att.Trim().ToLower(), hatreason).ToString();
                                                                                FpSpread2.Sheets[0].SetText(Att_mark_row, att_mark_col + 1, Att_Markvalue.ToString());
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            //End ===========================
                                                            else //if the student does not have suspension this part will work
                                                            {
                                                                // Att_strqueryst = "select " + Att_dcolumn + " from Attendance where Roll_no='" + rollno_Att.ToString() + "' and month_year=" + strdate.ToString() + "";
                                                                // Att_Markvalue = da.GetFunction(Att_strqueryst);
                                                                if (hatroll.Contains(rollno_Att.Trim().ToLower()))
                                                                {
                                                                    Att_Markvalue = GetCorrespondingKey(rollno_Att.Trim().ToLower(), hatroll).ToString();
                                                                    Att_Mark1 = Attmark(Att_Markvalue);
                                                                    //   FpSpread2.Sheets[0].SetValue(Att_mark_row, att_mark_col, Att_Markvalue.ToString());
                                                                    FpSpread2.Sheets[0].Cells[Att_mark_row, att_mark_col].Note = Att_Markvalue.ToString();
                                                                    FpSpread2.Sheets[0].SetText(Att_mark_row, att_mark_col, Att_Mark1.ToString());
                                                                    FpSpread2.Sheets[0].AutoPostBack = false;
                                                                    if (Att_Mark1 != "")
                                                                    {
                                                                        temp = temp + 1;
                                                                    }
                                                                }
                                                                if (hatreason.Contains(rollno_Att.Trim().ToLower()))
                                                                {
                                                                    Att_Markvalue = GetCorrespondingKey(rollno_Att.Trim().ToLower(), hatreason).ToString();
                                                                    FpSpread2.Sheets[0].SetText(Att_mark_row, att_mark_col + 1, Att_Markvalue.ToString());
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (temp > 0)
                                            {
                                                Buttonsave.Visible = false;
                                                Buttonupdate.Visible = true;
                                            }
                                            Labelstaf.Visible = false;
                                            FpSpread2.SaveChanges();
                                            FpSpread2.Visible = true;
                                            Buttonselectall.Visible = true;
                                            Buttondeselect.Visible = true;
                                            FpSpread2.Sheets[0].RowHeader.Width = 45;
                                            FpSpread2.Visible = true;
                                            Panelcomplete.Visible = true;
                                            Panelyet.Visible = true;
                                            filltree();
                                            FpSpread2.Visible = true;
                                            FpSpread1.Visible = true;
                                            headerpanelnotes.Visible = true;
                                            pHeaderlesson.Visible = true;
                                            pBodylesson.Visible = true;
                                            pBodynotes.Visible = true;
                                            pBodyquestionaddition.Visible = true;
                                            pHeaderatendence.Visible = true;
                                            pBodyatendence.Visible = true;
                                            pHeaderatendence.Visible = true;
                                            pHeaderlesson.Visible = true;
                                            pBodyatendence.Visible = true;
                                            pBodylesson.Visible = true;
                                            lbl_alert.Visible = false;
                                            Buttonsave.Visible = true;
                                            Buttonupdate.Visible = true;
                                            pHeaderatendence.Visible = true;
                                            pHeaderlesson.Visible = true;
                                            Buttonsave.Visible = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            lbl_alert.Visible = true;
                            lbl_alert.Text = "Append Can't be Allow For The Same Hour";
                        }
                    }
                    else
                    {
                        lbl_alert.Visible = true;
                        lbl_alert.Text = "Append Can't be Allow Different (Type Of) Subjects";
                    }
                }
                else
                {
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Append Can't be Allow For Different Date";
                }
            }
            else
            {
                lbl_alert.Visible = true;
                lbl_alert.Text = "No Students Found";
            }
        }
        else
        {
            load_attnd_spread();
            mark_attendance();
        }
    }

    public void filltree()
    {
        try
        {
            tvcomplete.Nodes.Clear();
            tvyet.Nodes.Clear();
            string sqlstr = string.Empty;
            int actrow1;
            int actcol1;
            string getcelltag;
            // string str =string.Empty;
            string degree_code = string.Empty;
            string semester = string.Empty;
            string subject_no = string.Empty;
            string sections = string.Empty;
            string strsec = string.Empty;
            string topics = string.Empty;
            int hr = 0;
            string sch_date = string.Empty;
            string[] sp_date;
            string sch_dt = string.Empty;
            ArrayList topics_com = new ArrayList();
            ArrayList topics_yet = new ArrayList();
            ArrayList topics_today = new ArrayList();
            string topics_plan = string.Empty;
            string topics_Entry = string.Empty;
            string topics_Entryall = string.Empty;
            actrow1 = FpSpread1.ActiveSheetView.ActiveRow;
            actcol1 = FpSpread1.ActiveSheetView.ActiveColumn;
            string sub_name = ddlselectmanysub.SelectedItem.ToString();
            sch_dt = FpSpread1.Sheets[0].RowHeader.Cells[actrow1, 0].Text;
            string[] spdatesp = FpSpread1.Sheets[0].RowHeader.Cells[actrow1, 0].Text.Split(' '); ;
            sch_dt = spdatesp[0].ToString();
            DataSet dsipcode = new DataSet();
            DataSet dstopic = new DataSet();
            string stripcode = string.Empty;
            string strtopic = string.Empty;
            if (sch_dt != "")
            {
                sp_date = sch_dt.Split(new Char[] { '-' });
                sch_date = sp_date[2].ToString() + "-" + sp_date[1].ToString() + "-" + sp_date[0].ToString();
                Session["sch_date"] = sch_date;
            }
            if (sub_name.Trim() == "")
                return;
            else
            {
                Buttonsavelesson.Visible = true;
                Labellvalid.Visible = false;
                Panelcomplete.Visible = true;
                Panelyet.Visible = true;
                string flag = (string)Session["flag"].ToString();
                tvcomplete.Nodes.Clear();
                tvyet.Nodes.Clear();
                Label2.Text = "Lesson Plan Topics";
                getcelltag = FpSpread1.Sheets[0].GetTag(actrow1, actcol1).ToString();
                string[] spitvalue = getcelltag.Split('*');
                getcelltag = string.Empty;
                for (int l = 0; l <= spitvalue.GetUpperBound(0); l++)
                {
                    string[] spitvalue1 = spitvalue[l].Split('-');
                    if (spitvalue1[0].ToLower().Trim() != "selected day is holiday")
                    {
                        if (getcelltag == "")
                        {
                            getcelltag = spitvalue[l].ToString();
                        }
                        else
                        {
                            getcelltag = getcelltag + spitvalue[l].ToString();
                        }
                    }
                }
                if (ddlselectmanysub.Items.Count > 0)
                {
                    string getddlsub = ddlselectmanysub.SelectedValue.ToString();
                    if (getddlsub.Trim() != "")
                    {
                        string[] sp1 = getddlsub.Split(new Char[] { '-' });
                        if (sp1.GetUpperBound(0) >= 7)
                        {
                            degree_code = sp1[0];
                            semester = sp1[1];
                            subject_no = sp1[2];
                            sections = sp1[3];
                            hr = actcol1 + 1;
                        }
                        else
                        {
                            degree_code = sp1[0];
                            semester = sp1[1];
                            subject_no = sp1[2];
                            sections = string.Empty;
                            hr = actcol1 + 1;
                        }
                    }
                }
                else
                {
                    if (getcelltag != "")
                    {
                        string[] sp1 = getcelltag.Split(new Char[] { '-' });
                        if (sp1.GetUpperBound(0) >= 7)
                        {
                            degree_code = sp1[0];
                            semester = sp1[1];
                            subject_no = sp1[2];
                            sections = sp1[3];
                            hr = actcol1 + 1;
                        }
                        else
                        {
                            degree_code = sp1[0];
                            semester = sp1[1];
                            subject_no = sp1[2];
                            sections = string.Empty;
                            hr = actcol1 + 1;
                        }
                    }
                }
                cnode = new TreeNode(sub_name, subject_no);
                ynode = new TreeNode(sub_name, subject_no);
                if (sections.ToString() != "" && sections.ToString() != "-1")
                {
                    strsec = " and sections='" + sections.ToString() + "' ";
                }
                else
                {
                    strsec = string.Empty;
                }
                //Start Topics Completed Datewise and Hourwise========================================================================
                //stripcode = "select lp_code from dailyStaffEntry where degree_code=" + degree_code + " and semester= " + semester + strsec + " and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + sch_date + "'";
                //dsipcode = da.select_method(stripcode, hat, "Text");
                //if (dsipcode.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i < dsipcode.Tables[0].Rows.Count; i++)
                //    {
                //        Session["lp_code"] = dsipcode.Tables[0].Rows[i]["lp_code"].ToString();
                //        strtopic = "select distinct topics from dailyEntdet where lp_code=" + dsipcode.Tables[0].Rows[i]["lp_code"].ToString() + " and subject_no = " + subject_no + " and staff_code='" + staff_code + "'  and hr=" + hr + "";
                //        dstopic.Dispose();
                //        dstopic.Reset();
                //        dstopic = da.select_method(strtopic, hat, "Text");
                //        if (dstopic.Tables[0].Rows.Count > 0)
                //        {
                //            topics = dstopic.Tables[0].Rows[0]["topics"].ToString();
                //            if (topics.Contains("/"))
                //            {
                //                string[] split = topics.Split(new Char[] { '/' });
                //                int ln = split.Length;
                //                for (int t = 0; t < ln; t++)
                //                {
                //                    topics_com.Add(split[t].ToString());
                //                    if (topics_Entry.ToString().Trim() == "")
                //                    {
                //                        topics_Entry = split[t].ToString();
                //                    }
                //                    else
                //                    {
                //                        topics_Entry = topics_Entry + "," + split[t].ToString();
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                topics_com.Add(topics.ToString());
                //                if (topics_Entry.ToString().Trim() == "")
                //                {
                //                    topics_Entry = topics.ToString();
                //                }
                //                else
                //                {
                //                    topics_Entry = topics_Entry + "," + topics.ToString();
                //                }
                //            }
                //        }
                //        // read_top.Close();
                //    }
                //}
                topics_Entry = string.Empty;
                stripcode = "select topics from dailyStaffEntry d,dailyEntdet de where d.lp_code=de.lp_code and degree_code='" + degree_code + "' and semester= '" + semester + "' " + strsec + " and batch_year='" + Session["batch_year"].ToString() + "' and sch_date<='" + sch_date + "' order by sch_date";
                dsipcode = da.select_method(stripcode, hat, "Text");
                if (dsipcode.Tables.Count > 0 && dsipcode.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsipcode.Tables[0].Rows.Count; i++)
                    {
                        topics = dsipcode.Tables[0].Rows[i]["topics"].ToString();
                        if (topics.Trim() != "")
                        {
                            string[] sptopic = topics.Split('/');
                            for (int stp = 0; stp <= sptopic.GetUpperBound(0); stp++)
                            {
                                if (sptopic[stp].ToString().Trim() != "")
                                {
                                    if (topics_Entry.ToString().Trim() == "")
                                    {
                                        topics_Entry = sptopic[stp].ToString();
                                    }
                                    else
                                    {
                                        topics_Entry = topics_Entry + "," + sptopic[stp].ToString();
                                    }
                                }
                            }
                        }
                    }
                }
                this.tvcomplete.Nodes.Clear();
                HierarchyTrees hierarchyTrees1 = new HierarchyTrees();
                HierarchyTrees.HTree objHTree1 = null;
                sqlstr = string.Empty;
                if (topics_Entry.ToString().Trim() != "")
                {
                    //con.Close();
                    //con.Open();
                    sqlstr = "select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + subject_no + "' ";
                    sqlstr = sqlstr + " and (topic_no  in(select parent_code from sub_unit_details where topic_no in(" + topics_Entry + "))";
                    sqlstr = sqlstr + " or topic_no  in( select parent_code from sub_unit_details where topic_no in(select parent_code from sub_unit_details where topic_no in(" + topics_Entry + ")))";
                    sqlstr = sqlstr + " or topic_no in(" + topics_Entry + ")) order by parent_code,topic_no";
                    dstopic.Dispose();
                    dstopic.Reset();
                    dstopic = da.select_method(sqlstr, hat, "Text");
                    this.tvcomplete.Nodes.Clear();
                    hierarchyTrees1.Clear();
                    if (dstopic.Tables.Count > 0 && dstopic.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dstopic.Tables[0].Rows.Count; i++)
                        {
                            objHTree1 = new HierarchyTrees.HTree();
                            objHTree1.topic_no = int.Parse(dstopic.Tables[0].Rows[i]["Topic_no"].ToString());
                            objHTree1.parent_code = int.Parse(dstopic.Tables[0].Rows[i]["parent_code"].ToString());
                            objHTree1.unit_name = dstopic.Tables[0].Rows[i]["unit_name"].ToString();
                            hierarchyTrees1.Add(objHTree1);
                        }
                    }
                    //using (SqlCommand command = new SqlCommand(sqlstr, con))
                    //{
                    //    this.tvcomplete.Nodes.Clear();
                    //    hierarchyTrees1.Clear();
                    //    SqlDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    //    while (reader.Read())
                    //    {
                    //        objHTree1 = new HierarchyTrees.HTree();
                    //        objHTree1.topic_no = int.Parse(reader["Topic_no"].ToString());
                    //        objHTree1.parent_code = int.Parse(reader["parent_code"].ToString());
                    //        objHTree1.unit_name = reader["unit_name"].ToString();
                    //        hierarchyTrees1.Add(objHTree1);
                    //    }
                    //}
                    foreach (HierarchyTrees.HTree hTree in hierarchyTrees1)
                    {
                        HierarchyTrees.HTree parentNode = hierarchyTrees1.Find(delegate(HierarchyTrees.HTree emp) { return emp.topic_no == hTree.parent_code; });
                        if (parentNode != null)
                        {
                            foreach (TreeNode tn in tvcomplete.Nodes)
                            {
                                if (tn.Value == parentNode.topic_no.ToString())
                                {
                                    tn.ChildNodes.Add(new TreeNode(hTree.unit_name.ToString(), hTree.topic_no.ToString()));
                                }
                                if (tn.ChildNodes.Count > 0)
                                {
                                    foreach (TreeNode ctn in tn.ChildNodes)
                                    {
                                        RecursiveChild(ctn, parentNode.topic_no.ToString(), hTree);
                                    }
                                }
                            }
                        }
                        else
                        {
                            tvcomplete.Nodes.Add(new TreeNode(hTree.unit_name, hTree.topic_no.ToString()));
                        }
                        //  tvcomplete.ExpandAll();
                    }
                }
                //End =================================================================================================================================
                //Start Topics Completed ===================================================================================================
                //mysql.Close();
                //mysql.Open();
                topics_Entryall = string.Empty;
                string striplcode = "select lp_code from dailyStaffEntry where degree_code=" + degree_code + " and semester= " + semester + strsec + " and batch_year=" + Session["batch_year"].ToString() + " ";
                dsipcode.Dispose();
                dsipcode.Reset();
                dsipcode = da.select_method(striplcode, hat, "Text");
                if (dsipcode.Tables.Count > 0 && dsipcode.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsipcode.Tables[0].Rows.Count; i++)
                    {
                        //cmd.CommandText = "select lp_code from dailyStaffEntry where degree_code=" + degree_code + " and semester= " + semester + strsec + " and batch_year=" + Session["batch_year"].ToString() + " ";
                        //cmd.Connection = mysql;
                        //SqlDataReader read_lp1 = cmd.ExecuteReader();
                        //if (read_lp1.HasRows)
                        //{
                        //while (read_lp1.Read())
                        //{
                        //mysql1.Close();
                        //mysql1.Open();
                        Session["lp_code"] = dsipcode.Tables[0].Rows[i]["lp_code"].ToString();
                        strtopic = "select distinct topics from dailyEntdet where lp_code=" + dsipcode.Tables[0].Rows[i]["lp_code"].ToString() + " and subject_no = " + subject_no + " and staff_code='" + staff_code + "' ";
                        dstopic.Dispose();
                        dstopic.Reset();
                        dstopic = da.select_method(strtopic, hat, "Text");
                        if (dstopic.Tables.Count > 0 && dstopic.Tables[0].Rows.Count > 0)
                        {
                            //cmd.CommandText = "select distinct topics from dailyEntdet where lp_code=" + dsiplcode.Tables[0].Rows[i]["lp_code"].ToString() + " and subject_no = " + subject_no + " and staff_code='" + staff_code + "' ";
                            //cmd.Connection = mysql1;
                            //SqlDataReader read_top = cmd.ExecuteReader();
                            //if (read_top.Read())
                            //{
                            topics = dstopic.Tables[0].Rows[0]["topics"].ToString();
                            if (topics.Contains("/"))
                            {
                                string[] split = topics.Split(new Char[] { '/' });
                                int ln = split.Length;
                                for (int t = 0; t < ln; t++)
                                {
                                    topics_com.Add(split[t].ToString());
                                    if (topics_Entryall.ToString().Trim() == "")
                                    {
                                        topics_Entryall = split[t].ToString();
                                    }
                                    else
                                    {
                                        topics_Entryall = topics_Entryall + "," + split[t].ToString();
                                    }
                                }
                            }
                            else
                            {
                                topics_com.Add(topics.ToString());
                                if (topics_Entryall.ToString().Trim() == "")
                                {
                                    topics_Entryall = topics.ToString();
                                }
                                else
                                {
                                    topics_Entryall = topics_Entryall + "," + topics.ToString();
                                }
                            }
                        }
                        //read_top.Close();
                    }
                    if (topics_Entryall.ToString().Trim() != "")
                    {
                        topics_Entryall = " and topic_no not in(" + topics_Entryall + ")";
                    }
                }
                //==================================================================================================================================
                //Start Lesson Plan Topics Datewise and Hourwise===============================================================================================================
                //mysql.Close();
                //mysql.Open();
                topics_plan = string.Empty;
                stripcode = "select lp_code from lesson_plan where degree_code=" + degree_code + " and semester= " + semester + strsec + " and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + sch_date + "'";
                dsipcode.Dispose();
                dsipcode.Reset();
                dsipcode = da.select_method(stripcode, hat, "Text");
                if (dsipcode.Tables.Count > 0 && dsipcode.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsipcode.Tables[0].Rows.Count; i++)
                    {
                        //cmd.CommandText = "select lp_code from lesson_plan where degree_code=" + degree_code + " and semester= " + semester + strsec + " and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + sch_date + "'";
                        //cmd.Connection = mysql;
                        //SqlDataReader read_lp_cur = cmd.ExecuteReader();
                        //if (read_lp_cur.HasRows)
                        //{
                        //    while (read_lp_cur.Read())
                        //    {
                        //mysql1.Close();
                        //mysql1.Open();
                        //cmd.CommandText = "select distinct topics from lessonplantopics where lp_code=" + dsipcode.Tables[0].Rows[i]["lp_code"].ToString() + " and subject_no = " + subject_no + " and staff_code='" + staff_code + "' and hr=" + hr;
                        //cmd.Connection = mysql1;
                        //SqlDataReader read_top = cmd.ExecuteReader();
                        //if (read_top.Read())
                        //{
                        strtopic = "select distinct topics from lessonplantopics where lp_code=" + dsipcode.Tables[0].Rows[i]["lp_code"].ToString() + " and subject_no = " + subject_no + " and staff_code='" + staff_code + "' and hr=" + hr;
                        dstopic.Dispose();
                        dstopic.Reset();
                        dstopic = da.select_method(strtopic, hat, "Text");
                        if (dstopic.Tables.Count > 0 && dstopic.Tables[0].Rows.Count > 0)
                        {
                            topics = dstopic.Tables[0].Rows[0]["topics"].ToString();
                            if (topics.Contains("/"))
                            {
                                string[] split = topics.Split(new Char[] { '/' });
                                int ln = split.Length;
                                for (int t = 0; t < ln; t++)
                                {
                                    topics_today.Add(split[t].ToString());
                                    if (topics_plan.ToString().Trim() == "")
                                    {
                                        topics_plan = split[t].ToString();
                                    }
                                    else
                                    {
                                        topics_plan = topics_plan + "," + split[t].ToString();
                                    }
                                }
                            }
                            else
                            {
                                topics_today.Add(topics.ToString());
                                if (topics_plan.ToString().Trim() == "")
                                {
                                    topics_plan = topics.ToString();
                                }
                                else
                                {
                                    topics_plan = topics_plan + "," + topics.ToString();
                                }
                            }
                        }
                        //read_top.Close();
                    }
                }
                //End ========================================================================================================================================================
                //Yet To Be Complete=============================================================================================================================================================
                this.tvyet.Nodes.Clear();
                HierarchyTrees hierarchyTrees = new HierarchyTrees();
                HierarchyTrees.HTree objHTree = null;
                //con.Close();
                //con.Open();
                sqlstr = string.Empty;
                if (flag == "1") //As Per Lesson Plan
                {
                    if (topics_plan.ToString().Trim() != "")
                    {
                        sqlstr = "select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + subject_no + "' ";
                        sqlstr = sqlstr + " and (topic_no  in(select parent_code from sub_unit_details where topic_no in(" + topics_plan + "))";
                        sqlstr = sqlstr + " or topic_no  in( select parent_code from sub_unit_details where topic_no in(select parent_code from sub_unit_details where topic_no in(" + topics_plan + ")))";
                        sqlstr = sqlstr + " or topic_no in(" + topics_plan + ")) order by parent_code,topic_no";
                    }
                }
                else //General
                {
                    // order added by Srinath   //01-09-2014
                    // sqlstr = "select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + subject_no + "' "+ topics_Entryall +""; //Exclude Daily Entry Topic
                    sqlstr = "select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + subject_no + "' order by parent_code,topic_no "; //Include Daily Entry Topic
                }
                if (sqlstr.ToString().Trim() != "")
                {
                    dstopic.Dispose();
                    dstopic.Reset();
                    dstopic = da.select_method(sqlstr, hat, "Text");
                    this.tvyet.Nodes.Clear();
                    hierarchyTrees.Clear();
                    if (dstopic.Tables.Count > 0 && dstopic.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dstopic.Tables[0].Rows.Count; i++)
                        {
                            objHTree = new HierarchyTrees.HTree();
                            objHTree.topic_no = int.Parse(dstopic.Tables[0].Rows[i]["Topic_no"].ToString());
                            objHTree.parent_code = int.Parse(dstopic.Tables[0].Rows[i]["parent_code"].ToString());
                            objHTree.unit_name = dstopic.Tables[0].Rows[i]["unit_name"].ToString();
                            hierarchyTrees.Add(objHTree);
                        }
                    }
                    //using (SqlCommand command = new SqlCommand(sqlstr, con))
                    //{
                    //    this.tvyet.Nodes.Clear();
                    //    hierarchyTrees.Clear();
                    //    SqlDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    //    while (reader.Read())
                    //    {
                    //        objHTree = new HierarchyTrees.HTree();
                    //        objHTree.topic_no = int.Parse(reader["Topic_no"].ToString());
                    //        objHTree.parent_code = int.Parse(reader["parent_code"].ToString());
                    //        objHTree.unit_name = reader["unit_name"].ToString();
                    //        hierarchyTrees.Add(objHTree);
                    //    }
                    //}
                    foreach (HierarchyTrees.HTree hTree in hierarchyTrees)
                    {
                        HierarchyTrees.HTree parentNode = hierarchyTrees.Find(delegate(HierarchyTrees.HTree emp) { return emp.topic_no == hTree.parent_code; });
                        if (parentNode != null)
                        {
                            foreach (TreeNode tn in tvyet.Nodes)
                            {
                                if (tn.Value == parentNode.topic_no.ToString())
                                {
                                    tn.ChildNodes.Add(new TreeNode(hTree.unit_name.ToString(), hTree.topic_no.ToString()));
                                }
                                if (tn.ChildNodes.Count > 0)
                                {
                                    foreach (TreeNode ctn in tn.ChildNodes)
                                    {
                                        RecursiveChild(ctn, parentNode.topic_no.ToString(), hTree);
                                    }
                                }
                            }
                        }
                        else
                        {
                            tvyet.Nodes.Add(new TreeNode(hTree.unit_name, hTree.topic_no.ToString()));
                        }
                        // tvyet.ExpandAll();
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void FpSpread2_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        if (Buttonsave.Enabled == false)
        {
            Buttonupdate.Enabled = true;
        }
        string actrow = FpSpread2.Sheets[0].ActiveRow.ToString();  //e.SheetView.ActiveRow.ToString();
        string actcol = FpSpread2.Sheets[0].ActiveColumn.ToString();  //e.SheetView.ActiveColumn.ToString();
        string last = e.CommandArgument.ToString();
        if (actrow == "0")
        {
            if (last == "0")
            {
                flag_true = false;
            }
            else
            {
                flag_true = true;
            }
        }
        if (actcol == "0")
        {
            if (actrow == last)
            {
                flag_true = false;
            }
            else
            {
                flag_true = true;
            }
        }
        if (flag_true == false)
        {
            actrow = e.SheetView.ActiveRow.ToString();
            actcol = e.SheetView.ActiveColumn.ToString();
            if (flag_true == false && actrow == "0")
            {
                for (int j = 1; j < Convert.ToInt16(FpSpread2.Sheets[0].RowCount - 2); j++)
                {
                    string seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                    if (seltext != "Select for All")
                    {
                        if (seltext != "System.Object")
                        {
                            //string get_text = FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text;
                            //if (get_text != "S")
                            //if ((FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text == "" || FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text == " " || FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text == null) && (FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text != "S")) 
                            if ((FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text != "OD") && (FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text != "S") && (FpSpread2.Sheets[0].Rows[j].BackColor != Color.Red))
                            {
                                string attmark = Attvalues(seltext);//Added by srinath 21/8/2013
                                FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = seltext.ToString();
                                FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Note = attmark.ToString();//Added by srinath 21/8/2013
                            }
                        }
                    }
                }
                flag_true = true;
                // FpSpread2.Focus();
            }
            if (flag_true == false && actcol == "0")
            {
                int colcnt;
                int i;
                string strvalue;
                int r = (int)e.CommandArgument;
                colcnt = e.EditValues.Count - 1;
                for (i = 0; i <= colcnt; i++)
                {
                    if (i >= 6)
                    {
                        if (!object.ReferenceEquals(e.EditValues[i], FarPoint.Web.Spread.FpSpread.Unchanged))
                        {
                            strvalue = e.EditValues[i].ToString();
                            FpSpread2.Sheets[0].Cells[r, i].Value = strvalue;
                        }
                    }
                }
                for (int j = 7; j <= Convert.ToInt16(FpSpread2.Sheets[0].ColumnCount - 2); j = j + 2)
                {
                    actcol = e.SheetView.ActiveColumn.ToString();
                    string seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                    string value = e.EditValues[1].ToString();
                    e.Handled = true;
                    seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                    if (seltext != "Select for All")
                    {
                        if (seltext != "System.Object")
                        {
                            if ((FpSpread2.Sheets[0].Cells[Convert.ToInt16(actrow), j].Text != "OD") && (FpSpread2.Sheets[0].Cells[Convert.ToInt16(actrow), j].Text != "S") && (FpSpread2.Sheets[0].Rows[Convert.ToInt16(actrow)].BackColor != Color.Red))
                            {
                                string attmark = Attvalues(seltext);//Added by srinath 21/8/2013
                                FpSpread2.Sheets[0].Cells[Convert.ToInt16(actrow), j].Text = seltext.ToString();
                                FpSpread2.Sheets[0].Cells[Convert.ToInt16(actrow), j].Note = attmark.ToString();//Added by srinath 21/8/2013
                            }
                        }
                    }
                }
                flag_true = true;
                //  FpSpread2.Focus();
            }
            //---------------get calcflag
            present_calcflag.Clear();
            absent_calcflag.Clear();
            hat.Clear();
            hat.Add("colege_code", Session["collegecode"].ToString());
            ds_attndmaster = da.select_method("ATT_MASTER_SETTING", hat, "sp");
            if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
            {
                count_master = (ds_attndmaster.Tables[0].Rows.Count);
                if (count_master > 0)
                {
                    for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
                    {
                        if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
                        {
                            present_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                        }
                        if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                        {
                            absent_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                        }
                    }
                }
            }
            if (FpSpread2.Sheets[0].RowCount > 2)
            {
                for (Att_mark_column = 7; Att_mark_column < FpSpread2.Sheets[0].ColumnCount; Att_mark_column++)
                {
                    absent_count = 0;
                    present_count = 0;
                    for (Att_mark_row = 1; Att_mark_row < FpSpread2.Sheets[0].RowCount - 2; Att_mark_row++)
                    {
                        if (FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text.ToString() != "")
                        {
                            if (present_calcflag.ContainsKey(FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Note.ToString()))
                            {
                                present_count++;
                            }
                            if (absent_calcflag.ContainsKey(FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Note.ToString()))
                            {
                                absent_count++;
                            }
                        }
                    }
                    FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 2), Att_mark_column].Text = present_count.ToString();
                    FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 1), Att_mark_column].Text = absent_count.ToString();
                    Att_mark_column++;
                }
            }
        }
        //----------------------------
    }

    protected void Buttonsave_Click(object sender, EventArgs e)
    {
        try
        {
            divConfirmBox.Visible = true;
            string msgValue = string.Empty;

            for (int col = 7; col <= FpSpread2.Sheets[0].ColumnCount - 2; col += 2)
            {
                DateTime dtAttDate = new DateTime();
                string hour = FpSpread2.Sheets[0].ColumnHeader.Cells[1, col].Tag.ToString();
                string attendanceDate = FpSpread2.Sheets[0].ColumnHeader.Cells[0, col].Text;
                Dictionary<DateTime, Dictionary<string, int>> dicStudentAttendance = new Dictionary<DateTime, Dictionary<string, int>>();
                if (DateTime.TryParseExact(attendanceDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtAttDate))
                {
                    for (int row = 1; row < FpSpread2.Sheets[0].RowCount - 2; row++)
                    {
                        Dictionary<string, int> dicAttCount = new Dictionary<string, int>();
                        string attendanceMark = Convert.ToString(FpSpread2.Sheets[0].Cells[row, col].Text).Trim().ToLower();
                        if (!dicStudentAttendance.ContainsKey(dtAttDate))
                        {
                            if (!dicAttCount.ContainsKey(attendanceMark))
                            {

                                dicAttCount.Add(attendanceMark, 1);
                            }
                            else
                            {
                                dicAttCount[attendanceMark] += 1;
                            }
                            dicStudentAttendance.Add(dtAttDate, dicAttCount);
                        }
                        else
                        {
                            dicAttCount = dicStudentAttendance[dtAttDate];
                            if (!dicAttCount.ContainsKey(attendanceMark))
                            {

                                dicAttCount.Add(attendanceMark, 1);
                            }
                            else
                            {
                                dicAttCount[attendanceMark] += 1;
                            }
                            dicStudentAttendance[dtAttDate] = dicAttCount;
                        }
                    }
                    if (dicStudentAttendance.ContainsKey(dtAttDate))
                    {
                        string msgAttCount = string.Empty;
                        Dictionary<string, int> dicStudAttCount = dicStudentAttendance[dtAttDate];
                        foreach (KeyValuePair<string, int> keyItem in dicStudAttCount)
                        {
                            msgAttCount += ((!string.IsNullOrEmpty(keyItem.Key.ToUpper().Trim())) ? keyItem.Key.ToUpper().Trim() : "Unmarked") + "\t:\t" + keyItem.Value + "\t\t";
                        }
                        msgValue += "Attendance Date : " + dtAttDate.ToString("dd/MM/yyyy") + " Hour : " + hour + "\t\t" + msgAttCount;
                    }
                }
            }
            lblConfirmMsg.Text = "Do You Want To Save Attendance ? " + ((!string.IsNullOrEmpty(msgValue)) ? msgValue : "");
            divConfirmBox.Visible = true;
        }
        catch (Exception ex)
        {
            Labelstaf.Visible = true;
            Labelstaf.Text = ex.ToString();
        }
    }

    protected void Buttonupdate_Click(object sender, EventArgs e)
    {
        Buttonsave_Click(sender, e);
    }

    protected void Buttonexit_Click(object sender, EventArgs e)
    {
    }

    protected void ddlmarkothers_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void btngoindividual_Click(object sender, EventArgs e)
    {
    }

    protected void ddlmark_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void txtregno_TextChanged(object sender, EventArgs e)
    {
    }

    protected void txtrunning_TextChanged(object sender, EventArgs e)
    {
    }

    //protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    LabelE.Visible = false;
    //    TextBoxother.Text =string.Empty;
    //    if (DropDownListpage.Text == "Others")
    //    {
    //        TextBoxother.Visible = true;
    //        TextBoxother.Focus();
    //    }
    //    else
    //    {
    //        TextBoxother.Visible = false;
    //        FpSpread2.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
    //        CalculateTotalPages();
    //    }
    //}
    //void CalculateTotalPages()
    //{
    //    Double totalRows = 0;
    //    totalRows = Convert.ToInt32(FpSpread2.Sheets[0].RowCount);
    //    Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread2.Sheets[0].PageSize);
    //    Buttontotal.Text = "Records:" + totalRows + " Pages:" + Session["totalPages"];
    //    Buttontotal.Visible = true;
    //}
    //protected void TextBoxpage_TextChanged(object sender, EventArgs e)
    //{
    //    LabelE.Visible = false;
    //    try
    //    {
    //        if (FpSpread2.Sheets[0].RowCount > 0)
    //        {
    //            if (TextBoxpage.Text.Trim() != "")
    //            {
    //                if (Convert.ToInt32(TextBoxpage.Text) > Convert.ToInt16(Session["totalPages"]))
    //                {
    //                    LabelE.Visible = true;
    //                    LabelE.Text = "Exceed The Page Limit";
    //                    TextBoxpage.Text =string.Empty;
    //                    FpSpread2.Visible = true;
    //                }
    //                else if ((Convert.ToInt32(TextBoxpage.Text) == 0))
    //                {
    //                    LabelE.Text = "Page search should be more than 0";
    //                    LabelE.Visible = true;
    //                    TextBoxpage.Text =string.Empty;
    //                    FpSpread2.Visible = true;
    //                }
    //                else
    //                {
    //                    LabelE.Visible = false;
    //                    FpSpread2.CurrentPage = Convert.ToInt32(TextBoxpage.Text) - 1;
    //                    FpSpread2.Visible = true;
    //                }
    //            }
    //        }
    //    }
    //    catch
    //    {
    //        LabelE.Text = "Exceed The Page Limit";
    //        TextBoxpage.Text =string.Empty;
    //        LabelE.Visible = true;
    //    }
    //}
    //protected void TextBoxother_TextChanged(object sender, EventArgs e)
    //{
    //    LabelE.Visible = false;
    //    try
    //    {
    //        if (TextBoxother.Text != string.Empty)
    //        {
    //            if (FpSpread2.Sheets[0].RowCount >= Convert.ToInt16(TextBoxother.Text.ToString()) && Convert.ToInt16(TextBoxother.Text.ToString()) != 0)
    //            {
    //                LabelE.Visible = false;
    //                FpSpread2.Sheets[0].PageSize = Convert.ToInt16(TextBoxother.Text.ToString());
    //                CalculateTotalPages();
    //            }
    //            else
    //            {
    //                LabelE.Visible = true;
    //                LabelE.Text = "Please Enter valid Record count";
    //                TextBoxother.Text =string.Empty;
    //            }
    //        }
    //    }
    //    catch
    //    {
    //        LabelE.Visible = true;
    //        LabelE.Text = "Please Enter valid Record count";
    //        TextBoxother.Text =string.Empty;
    //    }
    //}

    protected void Buttonselectall_Click(object sender, EventArgs e)
    {
        try
        {
            //Added by srinath 24/8/2013
            String rollnovalue = string.Empty;
            Boolean savefalg = false;
            if (FpSpread2.Sheets[0].RowCount > 1)
                for (int temp_col = 7; temp_col <= FpSpread2.Sheets[0].ColumnCount - 1; temp_col = temp_col + 2)
                {
                    for (int row = 0; row < FpSpread2.Sheets[0].RowCount; row++)
                    {
                        if (FpSpread2.Sheets[0].Cells[row, temp_col].Text != "S" && FpSpread2.Sheets[0].Cells[row, temp_col].Locked != true && FpSpread2.Sheets[0].Cells[row, temp_col].Text.ToLower() != "od" && (FpSpread2.Sheets[0].Rows[row].BackColor != Color.Red))// condn added on 09.08.12 mythli
                        {
                            FpSpread2.Sheets[0].Cells[row, temp_col].Text = "P";
                            savefalg = true;
                            rollnovalue = FpSpread2.Sheets[0].Cells[1, 1].Text;
                        }
                    }
                }
            //---------------get calcflag
            present_calcflag.Clear();
            absent_calcflag.Clear();
            hat.Clear();
            hat.Add("colege_code", Session["collegecode"].ToString());
            ds_attndmaster = da.select_method("ATT_MASTER_SETTING", hat, "sp");
            if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
            {
                count_master = (ds_attndmaster.Tables[0].Rows.Count);
                if (count_master > 0)
                {
                    for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
                    {
                        if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
                        {
                            present_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                        }
                        if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                        {
                            absent_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                        }
                    }
                }
            }
            for (Att_mark_column = 7; Att_mark_column < FpSpread2.Sheets[0].ColumnCount; Att_mark_column++)
            {
                absent_count = 0;
                present_count = 0;
                for (Att_mark_row = 1; Att_mark_row < FpSpread2.Sheets[0].RowCount - 2; Att_mark_row++)
                {
                    if (FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text.ToString() != "")
                    {
                        if (present_calcflag.ContainsKey(FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Value.ToString()))
                        {
                            present_count++;
                        }
                        if (absent_calcflag.ContainsKey(FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Value.ToString()))
                        {
                            absent_count++;
                        }
                    }
                }
                FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 2), Att_mark_column].Text = present_count.ToString();
                FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 1), Att_mark_column].Text = absent_count.ToString();
                Att_mark_column++;
            }
            FpSpread2.SaveChanges();
            //----------------------------
            //Added by srinath 24/8/2013
            if (savefalg == true)
            {
                string entrycode = Session["Entry_Code"].ToString();
                string formname = "Student Attendance Entry";
                string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                string doa = DateTime.Now.ToString("MM/dd/yyy");
                string details = string.Empty;
                DataSet dsroll = da.select_method("Select Batch_year,Degree_code,Current_semester,Sections from registration where roll_no='" + rollnovalue + "'", hat, "Text");
                if (dsroll.Tables.Count > 0 && dsroll.Tables[0].Rows.Count > 0)
                {
                    details = "" + dsroll.Tables[0].Rows[0]["Degree_code"].ToString() + ": Sem - " + dsroll.Tables[0].Rows[0]["Current_semester"].ToString() + ": Batch Year - " + dsroll.Tables[0].Rows[0]["Batch_year"].ToString();
                    if (dsroll.Tables[0].Rows[0]["Sections"].ToString() != "" && dsroll.Tables[0].Rows[0]["Sections"].ToString() != "0" && dsroll.Tables[0].Rows[0]["Sections"].ToString() != null)
                    {
                        details = details + ": Sections - " + dsroll.Tables[0].Rows[0]["Sections"].ToString();
                    }
                }
                string modules = "0";
                string act_diff = " ";
                string ctsname = "Change Attendance Information";
                string strlogdetails = "insert into UserLog (Entry_Code,Form_Name,UsrAction,TOA,DOA,Details,Module,Act_Diff,ctrNam) values ('" + entrycode + "','" + formname + "','7','" + toa + "','" + doa + "','" + details + "','" + modules + "','" + act_diff + "','" + ctsname + "')";
                int a = da.update_method_wo_parameter(strlogdetails, "Text");
            }
            //    Buttonselectall.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    protected void Buttondeselect_Click(object sender, EventArgs e)
    {
        try
        {
            String rollnovalue = string.Empty;
            Boolean savefalg = false;
            if (FpSpread2.Sheets[0].RowCount > 1)
                for (int temp_col = 7; temp_col <= FpSpread2.Sheets[0].ColumnCount - 1; temp_col++)
                {
                    for (int row = 0; row < FpSpread2.Sheets[0].RowCount; row++)
                    {
                        if (FpSpread2.Sheets[0].Cells[row, temp_col].Text != "S" && FpSpread2.Sheets[0].Cells[row, temp_col].Locked != true && FpSpread2.Sheets[0].Cells[row, temp_col].Text.ToLower() != "od" && (FpSpread2.Sheets[0].Rows[row].BackColor != Color.Red))// condn added on 09.08.12 mythli
                        {
                            FpSpread2.Sheets[0].Cells[row, temp_col].Text = string.Empty;
                            savefalg = true;
                            rollnovalue = FpSpread2.Sheets[0].Cells[1, 1].Text;
                        }
                        FpSpread2.SaveChanges();
                    }
                }
            //---------------get calcflag
            present_calcflag.Clear();
            absent_calcflag.Clear();
            hat.Clear();
            hat.Add("colege_code", Session["collegecode"].ToString());
            ds_attndmaster = da.select_method("ATT_MASTER_SETTING", hat, "sp");
            if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
            {
                count_master = (ds_attndmaster.Tables[0].Rows.Count);
                if (count_master > 0)
                {
                    for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
                    {
                        if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
                        {
                            present_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                        }
                        if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                        {
                            absent_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                        }
                    }
                }
            }
            for (Att_mark_column = 7; Att_mark_column < FpSpread2.Sheets[0].ColumnCount; Att_mark_column++)
            {
                absent_count = 0;
                present_count = 0;
                for (Att_mark_row = 1; Att_mark_row < FpSpread2.Sheets[0].RowCount - 2; Att_mark_row++)
                {
                    if (FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text.ToString() != "")
                    {
                        if (present_calcflag.ContainsKey(FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Value.ToString()))
                        {
                            present_count++;
                        }
                        if (absent_calcflag.ContainsKey(FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Value.ToString()))
                        {
                            absent_count++;
                        }
                    }
                }
                FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 2), Att_mark_column].Text = present_count.ToString();
                FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 1), Att_mark_column].Text = absent_count.ToString();
                Att_mark_column++;
            }
            //----------------------------
            //Added by srinath 24/8/2013
            if (savefalg == true)
            {
                string entrycode = Session["Entry_Code"].ToString();
                string formname = "Student Attendance Entry";
                string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                string doa = DateTime.Now.ToString("MM/dd/yyy");
                string details = string.Empty;
                DataSet dsroll = da.select_method("Select Batch_year,Degree_code,Current_semester,Sections from registration where roll_no='" + rollnovalue + "'", hat, "Text");
                if (dsroll.Tables.Count > 0 && dsroll.Tables[0].Rows.Count > 0)
                {
                    details = "" + dsroll.Tables[0].Rows[0]["Degree_code"].ToString() + ": Sem - " + dsroll.Tables[0].Rows[0]["Current_semester"].ToString() + ": Batch Year - " + dsroll.Tables[0].Rows[0]["Batch_year"].ToString();
                    if (dsroll.Tables[0].Rows[0]["Sections"].ToString() != "" && dsroll.Tables[0].Rows[0]["Sections"].ToString() != "0" && dsroll.Tables[0].Rows[0]["Sections"].ToString() != null)
                    {
                        details = details + ": Sections - " + dsroll.Tables[0].Rows[0]["Sections"].ToString();
                    }
                }
                string modules = "0";
                string act_diff = " ";
                string ctsname = "Change Attendance Information";
                string strlogdetails = "insert into UserLog (Entry_Code,Form_Name,UsrAction,TOA,DOA,Details,Module,Act_Diff,ctrNam) values ('" + entrycode + "','" + formname + "','8','" + toa + "','" + doa + "','" + details + "','" + modules + "','" + act_diff + "','" + ctsname + "')";
                int a = da.update_method_wo_parameter(strlogdetails, "Text");
            }
            //Buttonselectall.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    protected void OnTreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
    {
        Buttonsavelesson.Enabled = true;
        bool t = e.Node.Checked;
        if (ddlselectmanysub.Items.Count > 0)
        {
            string valsp = ddlselectmanysub.SelectedValue.ToString();
            string[] sp1 = valsp.Split(new Char[] { '-' });
            if (sp1.GetUpperBound(0) > 2)
            {
                string subcode = sp1[2].ToString();
                string subname = da.GetFunction("Select Subject_Name from subject where subject_no='" + subcode + "'");
                storepath = subname + " " + "/";
            }
            selectedpath = storepath;
        }
    }

    protected void Buttonsavelesson_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkalterlession.Checked == true)
            {
                savelessionalter();
            }
            else
            {
                string topics = string.Empty;
                string sch_order = string.Empty;
                foreach (TreeNode node in tvyet.CheckedNodes)
                {
                    if (topics == "")
                    {
                        topics = topics + node.Value;
                        selectedpath = selectedpath + node.ValuePath;
                    }
                    else
                    {
                        topics = topics + "/" + node.Value;
                        selectedpath = selectedpath + "=" + node.ValuePath;
                    }
                }
                if (topics == "")
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('There is nothing to save')", true);
                    return;
                }
                string order_day = string.Empty;
                string query = "select schorder from PeriodAttndSchedule where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString();
                string order = da.GetFunction(query);
                if (order == "")
                    return;
                string curday = Session["sch_date"].ToString();
                DateTime day_lesson = Convert.ToDateTime(curday);
                if (order != "0")
                    order_day = day_lesson.ToString("ddd");
                else
                {
                    order_day = find_day_order();
                    if (order_day == "")
                        return;
                }
                if (order_day == "mon")
                    sch_order = "1";
                else if (order_day == "tue")
                    sch_order = "2";
                else if (order_day == "wed")
                    sch_order = "3";
                else if (order_day == "thu")
                    sch_order = "4";
                else if (order_day == "fri")
                    sch_order = "5";
                else if (order_day == "sat")
                    sch_order = "6";
                else if (order_day == "sun")
                    sch_order = "7";
                if (order_day == "Mon")
                    sch_order = "1";
                else if (order_day == "Tue")
                    sch_order = "2";
                else if (order_day == "Wed")
                    sch_order = "3";
                else if (order_day == "Thu")
                    sch_order = "4";
                else if (order_day == "Fri")
                    sch_order = "5";
                else if (order_day == "Sat")
                    sch_order = "6";
                else if (order_day == "Sun")
                    sch_order = "7";
                string subj_no = (string)Session["sub_no"].ToString();
                string hour_hr = (string)Session["hr"].ToString();
                int a = 0;
                string strdailtquery = "select * from dailyStaffEntry where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + Session["str_section"].ToString() + "  and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + Session["sch_date"].ToString() + "'";
                DataSet dsdaily = da.select_method(strdailtquery, hat, "Text");
                if (dsdaily.Tables.Count > 0 && dsdaily.Tables[0].Rows.Count > 0)
                {
                    strdailtquery = "update dailyStaffEntry set sch_order=" + sch_order + " where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + Session["str_section"].ToString() + "  and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + Session["sch_date"].ToString() + "'";
                    a = da.update_method_wo_parameter(strdailtquery, "Text");
                }
                else
                {
                    string sec = (string)Session["sections"].ToString();
                    if (sec != "")
                        strdailtquery = "insert into dailyStaffEntry (degree_code,semester,batch_year,sch_date,sch_order,sections) values(" + Session["deg_code"].ToString() + "," + Session["semester"].ToString() + "," + Session["batch_year"].ToString() + ",'" + Session["sch_date"].ToString() + "'," + sch_order + ",'" + Session["sections"].ToString() + "')";
                    else
                        strdailtquery = "insert into dailyStaffEntry (degree_code,semester,batch_year,sch_date,sch_order) values(" + Session["deg_code"].ToString() + "," + Session["semester"].ToString() + "," + Session["batch_year"].ToString() + ",'" + Session["sch_date"].ToString() + "'," + sch_order + ")";
                    a = da.update_method_wo_parameter(strdailtquery, "Text");
                }
                string lp_query = "select lp_code from dailyStaffEntry where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + Session["str_section"].ToString() + "  and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + Session["sch_date"].ToString() + "' and sch_order=" + sch_order;
                string lp_code = da.GetFunction(lp_query);
                if (lp_code != "")
                {
                    strdailtquery = "select * from dailyEntdet where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "";
                    dsdaily.Dispose();
                    dsdaily.Reset();
                    dsdaily = da.select_method(strdailtquery, hat, "Text");
                    if (dsdaily.Tables.Count > 0 && dsdaily.Tables[0].Rows.Count > 0)
                    {
                        string strgettopic = da.GetFunction("select topics from dailyEntdet where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "");
                        if (strgettopic != "" && strgettopic != null && strgettopic != "0")
                        {
                            topics = topics + "/" + strgettopic;
                        }
                        //cmd.CommandText = "update dailyEntdet set topics='" + topics + "' where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "";                  
                        strdailtquery = "update dailyEntdet set topics='" + topics + "' where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "";
                        a = da.insert_method(strdailtquery, hat, "Text");
                    }
                    else
                    {
                        //cmd.CommandText = "insert into dailyEntdet (lp_code,subject_no,topics,hr,staff_code) values(" + lp_code + "," + subj_no + ",'" + topics + "'," + hour_hr + ",'" + staff_code + "')";
                        strdailtquery = "insert into dailyEntdet (lp_code,subject_no,topics,hr,staff_code) values(" + lp_code + "," + subj_no + ",'" + topics + "'," + hour_hr + ",'" + staff_code + "')";
                        a = da.update_method_wo_parameter(strdailtquery, "Text");
                    }
                }
                ar = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveRow.ToString());
                ac = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveColumn.ToString());
                if (ar != -1)
                {
                    string spread_text = FpSpread1.Sheets[0].Cells[ar, ac].Text;
                    //  getcelltag = FpSpread1.Sheets[0].GetTag(ar, ac).ToString();
                    getcelltag = ddlselectmanysub.SelectedValue.ToString();
                    string getdate = FpSpread1.Sheets[0].Cells[ar, 0].Text;
                    string[] splitvalue = getcelltag.Split(new char[] { '-' });
                    if (splitvalue.GetUpperBound(0) > 0)
                    {
                        string degree_code = splitvalue[0].ToString();
                        string semester = splitvalue[1].ToString();
                        string subject_no = splitvalue[2].ToString();
                        string batch_year = string.Empty;
                        if (splitvalue.GetUpperBound(0) == 7)
                        {
                            batch_year = splitvalue[4].ToString();
                        }
                        else
                        {
                            batch_year = splitvalue[3].ToString();
                        }
                        filltree();
                    }
                    ////Set Color
                    //**************added By Srinath 29Jan2015
                    string strstaffselector = string.Empty;
                    string checkalter = string.Empty;
                    for (int att_col = 6; att_col <= FpSpread2.Sheets[0].ColumnCount - 2; att_col = att_col + 2)
                    {
                        if (ar != -1)
                        {
                            string spread_text1 = FpSpread1.Sheets[0].Cells[ar, ac].Text;
                            string text_val = FpSpread1.Sheets[0].Cells[ar, ac].Text;
                            if (spread_text1 != "" && spread_text1 != "Sunday Holiday")
                            {
                                getcelltag = FpSpread1.Sheets[0].GetTag(ar, ac).ToString();
                                string avoidholiday = string.Empty;
                                string avoidholidaytext = string.Empty;
                                string[] spiltgetceltag = getcelltag.Split('*');
                                string[] spilttext = text_val.Split('*');
                                for (int k = 0; k <= spiltgetceltag.GetUpperBound(0); k++)
                                {
                                    string[] spitvalue = spiltgetceltag[k].Split('-');
                                    if (spitvalue[0].ToLower().Trim() == "selected day is holiday")
                                    {
                                    }
                                    else
                                    {
                                        if (avoidholiday == "")
                                        {
                                            avoidholiday = spiltgetceltag[k].ToString();
                                            avoidholidaytext = spilttext[k].ToString();
                                        }
                                        else
                                        {
                                            avoidholiday = avoidholiday + '*' + spiltgetceltag[k].ToString();
                                            avoidholidaytext = avoidholidaytext + '*' + spilttext[k].ToString();
                                        }
                                    }
                                }
                                getcelltag = avoidholiday;
                                text_val = avoidholidaytext;
                            }
                        }
                        hr = FpSpread1.Sheets[0].ColumnHeader.Cells[0, ac].Tag.ToString();
                        string temp_date = FpSpread1.Sheets[0].RowHeader.Cells[ar, 0].Text;
                        string[] spitdate = temp_date.Split(' ');
                        string str_Date = spitdate[0].ToString();
                        //string str_Date = FpSpread2.Sheets[0].ColumnHeader.Cells[0, att_col].Text;
                        string[] split = str_Date.Split(new Char[] { '-' });
                        string str_day = (Convert.ToInt16(split[0].ToString())).ToString();
                        string Atmonth = (Convert.ToInt16(split[1].ToString())).ToString();
                        string Atyear = split[2].ToString();
                        int strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                        string str_hour = Convert.ToString(hr);
                        string dcolumn = "d" + str_day + "d" + str_hour;
                        string batch = "", section = "", strquery = string.Empty;
                        DataSet dsattendance = new DataSet();
                        DateTime date = Convert.ToDateTime(split[1].ToString() + '-' + split[0].ToString() + '-' + split[2].ToString());
                        string day = date.ToString("ddd");
                        string[] spilttext2 = getcelltag.Split('*');
                        for (int j = 0; j <= spilttext2.GetUpperBound(0); j++)
                        {
                            Boolean colorflag = false;
                            if (j > 0)
                            {
                                if (FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor == Color.Blue)
                                {
                                    colorflag = true;
                                }
                                else
                                {
                                    colorflag = false;
                                }
                            }
                            if (colorflag == false)
                            {
                                dailyentryflag = false;
                                attendanceentryflag = false;
                                string[] split_tag_val = spilttext2[j].Split('-');
                                string check_lab = string.Empty;
                                if (split_tag_val.GetUpperBound(0) >= 7)
                                {
                                    batch = split_tag_val[4].ToString();
                                    degree_code = split_tag_val[0].ToString();
                                    semester = split_tag_val[1].ToString();
                                    subject_no = split_tag_val[2].ToString();
                                    //section = "and Registration.Sections='" + split_tag_val[3].ToString() + "'";
                                    section = split_tag_val[3].ToString();
                                    checkalter = split_tag_val[7].ToString();
                                    check_lab = split_tag_val[5].ToString();
                                }
                                else
                                {
                                    batch = split_tag_val[3].ToString();
                                    degree_code = split_tag_val[0].ToString();
                                    semester = split_tag_val[1].ToString();
                                    subject_no = split_tag_val[2].ToString();
                                    section = string.Empty;
                                    checkalter = split_tag_val[6].ToString();
                                    check_lab = split_tag_val[4].ToString();
                                }
                                Session["StaffSelector"] = "0";
                                strstaffselector = string.Empty;    //" + Session["collegecode"].ToString() + "
                                string staffbatchyear = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Session["collegecode"].ToString() + "'");
                                string[] splitminimumabsentsms = staffbatchyear.Split('-');
                                if (splitminimumabsentsms.Length == 2)
                                {
                                    int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
                                    if (splitminimumabsentsms[0].ToString() == "1")
                                    {
                                        if (Convert.ToInt32(batch) >= batchyearsetting)
                                        {
                                            Session["StaffSelector"] = "1";
                                        }
                                    }
                                }
                                if (Session["StaffSelector"].ToString() == "1")
                                {
                                    strstaffselector = " and s.staffcode like '%" + Session["Staff_Code"].ToString() + "%'";
                                }
                                string labsection = string.Empty;
                                if (section.Trim() == "" && section != null && section.Trim() != "-1")
                                {
                                    labsection = " and sections='" + section + "'";
                                }
                                //  string check_lab = da.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subject_no + "'");
                                if (check_lab == "L" || check_lab.Trim().ToLower() == "l")
                                {
                                    strquery = "  select p.schOrder,p.nodays,Convert(nvarchar(15),s.start_date,23) as start,s.starting_dayorder from PeriodAttndSchedule p, seminfo s where s.degree_code=p.degree_code and s.semester=p.semester and s.degree_code=" + degree_code + " and s.semester=" + semester + " AND s.batch_year=" + batch + "";
                                    Day_Order = "0";
                                    dsattendance = da.select_method(strquery, hat, "Text");
                                    if (dsattendance.Tables.Count > 0 && dsattendance.Tables[0].Rows.Count > 0)
                                    {
                                        Day_Order = dsattendance.Tables[0].Rows[0]["schOrder"].ToString();
                                        noofdays = dsattendance.Tables[0].Rows[0]["nodays"].ToString();
                                        start_datesem = dsattendance.Tables[0].Rows[0]["start"].ToString();
                                        start_dayorder = dsattendance.Tables[0].Rows[0]["starting_dayorder"].ToString();
                                    }
                                    //Week / Day order
                                    if (Day_Order == "1")
                                    {
                                        day = date.ToString("ddd");
                                    }
                                    else
                                    {
                                        day = da.findday(date.ToString(), degree_code, semester, batch, start_datesem.ToString(), noofdays.ToString(), start_dayorder);//Modifeied By Srianth add comman Daccess 5/9/2014
                                    }
                                    if (checkalter.ToLower().Trim() == "alter")
                                    {
                                        hat.Clear();
                                        hat.Add("batch_year", batch);
                                        hat.Add("degree_code", degree_code);
                                        hat.Add("sem", semester);
                                        hat.Add("sections", section);
                                        hat.Add("month_year", strdate);
                                        hat.Add("date", date);
                                        hat.Add("subject_no", subject_no);
                                        hat.Add("day", day);
                                        hat.Add("hour", str_hour);
                                        ds.Reset();
                                        ds.Dispose();
                                        ds = da.select_method("sp_stu_atten_month_check_lab_alter", hat, "sp");
                                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        {
                                            Att_strqueryst = ds.Tables[0].Rows[0]["stucount"].ToString();
                                            if (int.Parse(Att_strqueryst) > 0)
                                            {
                                                hat.Clear();
                                                hat.Add("columnname", dcolumn);
                                                hat.Add("batch_year", batch);
                                                hat.Add("degree_code", degree_code);
                                                hat.Add("sem", semester);
                                                hat.Add("sections", section);
                                                hat.Add("month_year", strdate);
                                                hat.Add("date", date);
                                                hat.Add("subject_no", subject_no);
                                                hat.Add("day", day);
                                                hat.Add("hour", str_hour);
                                                ds.Reset();
                                                ds.Dispose();
                                                // ds = da.select_method("sp_stu_atten_day_check_lab_alter", hat, "sp");
                                                string strgetatt = "select count(distinct r.Roll_No) as stucount from registration r,attendance a,subjectchooser_new s where degree_code='" + degree_code + "' and current_semester='" + semester + "' and batch_year='" + batch + "' and cc=0 and delflag=0 and  exam_flag<>'debar' and month_year=" + strdate + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and";
                                                strgetatt = strgetatt + " r.current_semester=s.semester and subject_no='" + subject_no + "' " + labsection + " and(" + dcolumn + " is not null and " + dcolumn + "<>'0' and " + dcolumn + "<>'') and fromdate='" + date + "' and batch    in(select stu_batch from laballoc_new where subject_no='" + subject_no + "'  and batch_year='" + batch + "'  and hour_value='" + str_hour + "'  and    degree_code='" + degree_code + "' ";
                                                strgetatt = strgetatt + " and day_value='" + day + "' and semester='" + semester + "' " + labsection + " and fdate='" + date + "') and adm_date<='" + date + "' " + strstaffselector + "";
                                                ds = da.select_method_wo_parameter(strgetatt, "Text");
                                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                {
                                                    if (Att_strqueryst == ds.Tables[0].Rows[0]["stucount"].ToString())
                                                    {
                                                        Att_strqueryst = "0";
                                                    }
                                                    else
                                                    {
                                                        Att_strqueryst = "1";
                                                    }
                                                }
                                                else
                                                {
                                                    Att_strqueryst = "1";
                                                }
                                            }
                                            else
                                            {
                                                Att_strqueryst = "1";
                                            }
                                        }
                                        else
                                        {
                                            Att_strqueryst = "1";
                                        }
                                    }
                                    else
                                    {
                                        string timetable = da.GetFunction("select top 1 TTName,  FromDate from  Semester_Schedule where degree_code='" + degree_code + "' and semester='" + semester + "' and batch_year='" + batch + "' " + labsection + " and FromDate<='" + date.ToString("MM/dd/yyyy") + "'  order by FromDate Desc");
                                        hat.Clear();
                                        //hat.Add("batch_year", batch);
                                        //hat.Add("degree_code", degree_code);
                                        //hat.Add("sem", semester);
                                        //hat.Add("sections", section);
                                        //hat.Add("month_year", strdate);
                                        //hat.Add("date", date);
                                        //hat.Add("subject_no", subject_no);
                                        //hat.Add("day", day);
                                        //hat.Add("hour", str_hour);
                                        //hat.Add("ttmane", timetable);
                                        //hat.Add("staff_code", staff_code);
                                        string strstt = "select count(distinct r.Roll_No) as stucount from registration r,subjectchooser s where degree_code='" + degree_code + "' and ";
                                        strstt = strstt + " current_semester='" + semester + "' and batch_year='" + batch.ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar' and r.roll_no=s.roll_no ";
                                        strstt = strstt + " and r.current_semester=s.semester and subject_no='" + subject_no + "' " + labsection + " and batch in(select stu_batch from ";
                                        strstt = strstt + " laballoc where subject_no='" + subject_no + "'  and batch_year='" + batch.ToString() + "'  and hour_value='" + str_hour + "' and degree_code='" + degree_code + "' ";
                                        strstt = strstt + " and day_value='" + day + "' and semester='" + semester + "' " + labsection + " and Timetablename='" + timetable + "') and adm_date<='" + date.ToString("MM/dd/yyyy") + "'  " + strstaffselector + "";
                                        ds.Reset();
                                        ds.Dispose();
                                        // ds = da.select_method("sp_stu_atten_month_check_lab", hat, "sp");
                                        ds = da.select_method_wo_parameter(strstt, "Text");
                                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        {
                                            Att_strqueryst = ds.Tables[0].Rows[0]["stucount"].ToString();
                                            if (int.Parse(Att_strqueryst) > 0)
                                            {
                                                hat.Clear();
                                                hat.Add("columnname", dcolumn);
                                                hat.Add("batch_year", batch);
                                                hat.Add("degree_code", degree_code);
                                                hat.Add("sem", semester);
                                                hat.Add("sections", section);
                                                hat.Add("month_year", strdate);
                                                hat.Add("date", date);
                                                hat.Add("subject_no", subject_no);
                                                hat.Add("day", day);
                                                hat.Add("hour", str_hour);
                                                hat.Add("ttmane", timetable);
                                                ds.Reset();
                                                ds.Dispose();
                                                //  ds = da.select_method("sp_stu_atten_day_check_lab", hat, "sp");
                                                string strgetatt = "select count( r.Roll_No) as stucount from registration r,attendance a,subjectchooser s where degree_code='" + degree_code + "' and current_semester='" + semester + "' and batch_year='" + batch + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year=" + strdate + "";
                                                strgetatt = strgetatt + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and r.current_semester=s.semester and subject_no='" + subject_no + "' " + labsection + " and(" + dcolumn + " is not null and " + dcolumn + "<>'0' and " + dcolumn + "<>'') and batch in(select stu_batch from laballoc ";
                                                strgetatt = strgetatt + " where subject_no='" + subject_no + "' and Timetablename='" + timetable + "' and batch_year='" + batch + "'  and hour_value='" + str_hour + "'  and    degree_code='" + degree_code + "' and day_value='" + day + "' and semester='" + semester + "' " + labsection + ") and adm_date<='" + date + "' " + strstaffselector + "";
                                                ds = da.select_method_wo_parameter(strgetatt, "Text");
                                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                {
                                                    if (Att_strqueryst == ds.Tables[0].Rows[0]["stucount"].ToString())
                                                    {
                                                        Att_strqueryst = "0";
                                                    }
                                                    else
                                                    {
                                                        Att_strqueryst = "1";
                                                    }
                                                }
                                                else
                                                {
                                                    Att_strqueryst = "1";
                                                }
                                            }
                                            else
                                            {
                                                Att_strqueryst = "1";
                                            }
                                        }
                                        else
                                        {
                                            Att_strqueryst = "1";
                                        }
                                    }
                                }
                                else
                                {
                                    //hat.Clear();
                                    //hat.Add("batch_year", batch);
                                    //hat.Add("degree_code", degree_code);
                                    //hat.Add("sem", semester);
                                    //hat.Add("sections", section);
                                    //hat.Add("month_year", strdate);
                                    //hat.Add("date", date);
                                    //hat.Add("subject_no", subject_no);
                                    //hat.Add("staff_code", staff_code);
                                    ds.Reset();
                                    ds.Dispose();
                                    //ds = da.select_method("sp_stu_atten_month_check", hat, "sp");
                                    string strgetatt1 = "select count(distinct r.Roll_No) as stucount from registration r,subjectchooser s where  r.roll_no=s.roll_no and ";
                                    strgetatt1 = strgetatt1 + " r.current_semester=s.semester and batch_year='" + batch + "' and degree_code='" + degree_code + "'  and current_semester='" + semester + "' " + labsection + " ";
                                    strgetatt1 = strgetatt1 + "  and subject_no='" + subject_no + "'  and adm_date<='" + date + "' and cc=0 and delflag=0 and exam_flag<>'debar'  " + strstaffselector + "";
                                    ds = da.select_method_wo_parameter(strgetatt1, "Text");
                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                    {
                                        Att_strqueryst = ds.Tables[0].Rows[0]["stucount"].ToString();
                                        if (int.Parse(Att_strqueryst) > 0)
                                        {
                                            hat.Clear();
                                            hat.Add("columnname", dcolumn);
                                            hat.Add("batch_year", batch);
                                            hat.Add("degree_code", degree_code);
                                            hat.Add("sem", semester);
                                            hat.Add("sections", section);
                                            hat.Add("month_year", strdate);
                                            hat.Add("date", date);
                                            hat.Add("subject_no", subject_no);
                                            ds.Reset();
                                            ds.Dispose();
                                            //ds = da.select_method("sp_stu_atten_day_check", hat, "sp");
                                            string strgetatt = "select count(registration.roll_no) as stucount  from registration,attendance,subjectchooser s where degree_code='" + degree_code + "' and current_semester='" + semester + "' and batch_year='" + batch + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and  registration.roll_no=s.roll_no ";
                                            strgetatt = strgetatt + " and registration.current_semester=s.semester and subject_no='" + subject_no + "' " + labsection + "";
                                            strgetatt = strgetatt + " and (" + dcolumn + " is not null and " + dcolumn + "<>'0' and " + dcolumn + "<>'') and adm_date<='" + date + "' " + strstaffselector + " ";
                                            ds = da.select_method_wo_parameter(strgetatt, "Text");
                                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                            {
                                                if (Att_strqueryst == ds.Tables[0].Rows[0]["stucount"].ToString())
                                                {
                                                    Att_strqueryst = "0";
                                                }
                                                else
                                                {
                                                    Att_strqueryst = "1";
                                                }
                                            }
                                            else
                                            {
                                                Att_strqueryst = "1";
                                            }
                                        }
                                        else
                                        {
                                            Att_strqueryst = "1";
                                        }
                                    }
                                    else
                                    {
                                        Att_strqueryst = "1";
                                    }
                                }
                                if (int.Parse(Att_strqueryst) > 0)
                                {
                                    //FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.Blue;
                                    attendanceentryflag = false;
                                }
                                else
                                {
                                    attendanceentryflag = true;
                                    //FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.ForestGreen;
                                }
                                if (section.Trim() == "" || section == null || section.Trim() == "-1")
                                {
                                    section = string.Empty;
                                }
                                else
                                {
                                    section = " and Sections='" + section + "'";
                                }
                                strquerytext = "select de.lp_code from dailyentdet de,dailystaffentry ds where de.lp_code=ds.lp_code  and batch_year=" + batch + " and degree_code='" + degree_code + "' and semester=" + semester + " " + section + " and subject_no='" + subject_no + "' and  staff_code='" + staff_code + "' and sch_date='" + date + "' and hr=" + hr + "";
                                ds.Reset();
                                ds.Dispose();
                                ds = da.select_method(strquerytext, hat, "Text");
                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    dailyentryflag = true;
                                }
                                if (dailyentryflag == false && attendanceentryflag == false)
                                {
                                    FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.Blue;
                                    j = spilttext2.GetUpperBound(0) + 1;
                                }
                                else if (dailyentryflag == true && attendanceentryflag == false)
                                {
                                    if (FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor == Color.DarkOrchid)
                                    {
                                        FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.Blue;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.DarkTurquoise;
                                    }
                                }
                                else if (dailyentryflag == false && attendanceentryflag == true)
                                {
                                    if (FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor == Color.DarkTurquoise)
                                    {
                                        FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.Blue;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.DarkOrchid;
                                    }
                                }
                                else
                                {
                                    if (j == 0)
                                    {
                                        FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.ForestGreen;
                                    }
                                    else
                                    {
                                        if (FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor == Color.ForestGreen)
                                        {
                                            FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.ForestGreen;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void Buttonexitlesson_Click(object sender, EventArgs e)
    {
        // Response.Redirect("Default2.aspx");
    }

    protected void btndailyentrydelete_Click(object sender, EventArgs e)
    {
        try
        {
            string topics = string.Empty;
            string sch_order = string.Empty;
            Hashtable hatdelnode = new Hashtable();
            foreach (TreeNode node in tvcomplete.CheckedNodes)
            {
                if (topics == "")
                {
                    topics = topics + node.Value;
                    hatdelnode.Add(node.Value, node.Value);
                    selectedpath = selectedpath + node.ValuePath;
                }
                else
                {
                    topics = topics + "/" + node.Value;
                    hatdelnode.Add(node.Value, node.Value);
                    selectedpath = selectedpath + "=" + node.ValuePath;
                }
            }
            if (topics == "")
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('There is nothing to Delete')", true);
                return;
            }
            string order_day = string.Empty;
            string query = "select schorder from PeriodAttndSchedule where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString();
            string order = da.GetFunction(query);
            if (order == "")
                return;
            string curday = Session["sch_date"].ToString();
            DateTime day_lesson = Convert.ToDateTime(curday);
            if (order != "0")
                order_day = day_lesson.ToString("ddd");
            else
            {
                order_day = find_day_order();
                if (order_day == "")
                    return;
            }
            if (order_day == "mon")
                sch_order = "1";
            else if (order_day == "tue")
                sch_order = "2";
            else if (order_day == "wed")
                sch_order = "3";
            else if (order_day == "thu")
                sch_order = "4";
            else if (order_day == "fri")
                sch_order = "5";
            else if (order_day == "sat")
                sch_order = "6";
            else if (order_day == "sun")
                sch_order = "7";
            if (order_day == "Mon")
                sch_order = "1";
            else if (order_day == "Tue")
                sch_order = "2";
            else if (order_day == "Wed")
                sch_order = "3";
            else if (order_day == "Thu")
                sch_order = "4";
            else if (order_day == "Fri")
                sch_order = "5";
            else if (order_day == "Sat")
                sch_order = "6";
            else if (order_day == "Sun")
                sch_order = "7";
            string subj_no = (string)Session["sub_no"].ToString();
            string hour_hr = (string)Session["hr"].ToString();
            string lp_code = string.Empty;
            int a = 0;
            string updatenode = string.Empty;
            string strdailtquery = "select * from dailyStaffEntry where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + Session["str_section"].ToString() + "  and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + Session["sch_date"].ToString() + "'";
            DataSet dsdaily = da.select_method(strdailtquery, hat, "Text");
            if (dsdaily.Tables.Count > 0 && dsdaily.Tables[0].Rows.Count > 0)
            {
                string lp_query = "select lp_code from dailyStaffEntry where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + Session["str_section"].ToString() + "  and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + Session["sch_date"].ToString() + "' and sch_order=" + sch_order;
                lp_code = da.GetFunction(lp_query);
                if (lp_code != "")
                {
                    strdailtquery = da.GetFunction("select topics from dailyEntdet where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "");
                    string[] strb = strdailtquery.Split('/');
                    for (int st = 0; st <= strb.GetUpperBound(0); st++)
                    {
                        string getva = strb[st].ToString();
                        if (!hatdelnode.Contains(getva))
                        {
                            if (updatenode == "")
                            {
                                updatenode = getva;
                            }
                            else
                            {
                                updatenode = updatenode + '/' + getva;
                            }
                        }
                    }
                }
            }
            if (updatenode.Trim() != "")//Update Here
            {
                string struopdatye = "update  dailyEntdet set topics='" + updatenode + "' where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "";
                int upqu = da.update_method_wo_parameter(struopdatye, "text");
            }
            else//Delete Here
            {
                string struopdatye = "delete dailyEntdet where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "";
                int upqu = da.update_method_wo_parameter(struopdatye, "text");
                struopdatye = "delete dailyStaffEntry where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + Session["str_section"].ToString() + "  and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + Session["sch_date"].ToString() + "' and sch_order=" + sch_order;
                upqu = da.update_method_wo_parameter(struopdatye, "text");
            }
            ar = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveRow.ToString());
            ac = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveColumn.ToString());
            if (ar != -1)
            {
                string spread_text = FpSpread1.Sheets[0].Cells[ar, ac].Text;
                getcelltag = ddlselectmanysub.SelectedValue.ToString();
                string getdate = FpSpread1.Sheets[0].Cells[ar, 0].Text;
                string[] splitvalue = getcelltag.Split(new char[] { '-' });
                if (splitvalue.GetUpperBound(0) > 0)
                {
                    string degree_code = splitvalue[0].ToString();
                    string semester = splitvalue[1].ToString();
                    string subject_no = splitvalue[2].ToString();
                    string batch_year = string.Empty;
                    if (splitvalue.GetUpperBound(0) == 7)
                    {
                        batch_year = splitvalue[4].ToString();
                    }
                    else
                    {
                        batch_year = splitvalue[3].ToString();
                    }
                    filltree();
                }
            }
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted successfully')", true);
        }
        catch (Exception ex)
        {
        }
    }

    public string find_day_order()
    {
        int holiday = 0;
        string query = "select CONVERT(VARCHAR(10),start_date,23) from seminfo where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + "  and batch_year=" + Session["batch_year"].ToString();
        string sdate = da.GetFunction(query);
        string quer = "select nodays from PeriodAttndSchedule where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString();
        string no_days = da.GetFunction(quer);
        if (sdate != "")
        {
            string curday = Session["sch_date"].ToString();
            string[] sp_date = sdate.Split(new Char[] { '-' });
            string start_date = sp_date[1].ToString() + "-" + sp_date[2].ToString() + "-" + sp_date[0].ToString();
            DateTime dt1 = Convert.ToDateTime(start_date);
            DateTime dt2 = Convert.ToDateTime(curday);
            TimeSpan ts = dt2 - dt1;
            string query1 = "select count(*) from holidaystudents  where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + " and holiday_date between'" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "' and halforfull='0'";
            string holday = da.GetFunction(query1);
            if (holday != "")
                holiday = Convert.ToInt32(holday);
            int dif_days = ts.Days;
            int nodays = Convert.ToInt32(no_days);
            int order = (dif_days - holiday) % nodays;
            order = order + 1;
            string findday = string.Empty;
            if (order == 1)
                findday = "mon";
            else if (order == 2) findday = "tue";
            else if (order == 3) findday = "wed";
            else if (order == 4) findday = "thu";
            else if (order == 5) findday = "fri";
            else if (order == 6) findday = "sat";
            else if (order == 7) findday = "sun";
            return findday;
        }
        else
            return "";
    }

    //protected void tvcomplete_SelectedNodeChanged1(object sender, EventArgs e)
    //{
    //   // string s =string.Empty;
    //}
    //protected void tvyet_SelectedNodeChanged(object sender, EventArgs e)
    //{
    //}
    //protected void ck_append_CheckedChanged(object sender, EventArgs e)
    //{
    //}
    //protected void tvyet_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
    //{
    //}
    //protected void tvyet_TreeNodeCollapsed(object sender, TreeNodeEventArgs e)
    //{
    //}
    //protected void tvyet_TreeNodePopulate(object sender, TreeNodeEventArgs e)
    //{
    //}

    protected void tvyet_SelectedNodeChanged1(object sender, EventArgs e)
    {
        Buttonsavelesson.Enabled = true;
        //bool t = e.Node.Checked;
    }

    //protected void tvyet_Unload(object sender, EventArgs e)
    //{
    //}

    protected void OnTreeNodecompleteCheckChanged(object sender, TreeNodeEventArgs e)
    {
        Buttonsavelesson.Enabled = true;
        bool t = e.Node.Checked;
    }

    public void SendingSms(string rollno, string appno, string regno, string admno, string date, string hour, string degree, int total, int absent)
    {
        try
        {
            string Gender = string.Empty;
            string Hour = hour;
            string hour_check = string.Empty;
            //UserEmailID =string.Empty;
            MsgText = string.Empty;
            RecepientNo = string.Empty;
            int check = 0;
            string coursename = string.Empty;
            string collegename = string.Empty;
            string[] split = date.Split(new Char[] { '-' });
            string datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
            date = datefrom;
            if (Convert.ToInt16(hour) == 1)
            {
                Hour = hour + "st ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) == 2)
            {
                Hour = hour + "nd ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) == 3)
            {
                Hour = hour + "rd ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) > 3)
            {
                Hour = hour + "th ";
                hour_check = hour;
            }
            string collquery = "Select collname,Coll_acronymn from collinfo where college_code=" + Session["collegecode"].ToString() + "";
            DataSet datacol = new DataSet();
            datacol.Clear();
            datacol = da.select_method_wo_parameter(collquery, "Text");
            if (datacol.Tables.Count > 0 && datacol.Tables[0].Rows.Count > 0)
            {
                collegename = datacol.Tables[0].Rows[0]["Coll_acronymn"].ToString();
            }
            //string degreequery = "select distinct Course_Name,Dept_Name from Department dep, Degree deg, course c where dep.Dept_Code=deg.Dept_Code and c.Course_Id=deg.Course_Id and deg.college_code =" + Session["collegecode"].ToString() + " and Degree_Code=" + degree + "";
            string degreequery = "select distinct Course_Name,Dept_Name,r.degree_code from Department dep, Degree deg, course c,Registration r where dep.Dept_Code=deg.Dept_Code and c.Course_Id=deg.Course_Id and r.degree_code=deg.Degree_Code and r.Roll_No='" + rollno + "'";
            DataSet dscode = new DataSet(); string degreecode = string.Empty;
            dscode = da.select_method_wo_parameter(degreequery, "Text");
            if (dscode.Tables.Count > 0 && dscode.Tables[0].Rows.Count > 0)
            {
                string course = dscode.Tables[0].Rows[0]["Course_Name"].ToString();
                string deptname = dscode.Tables[0].Rows[0]["Dept_Name"].ToString();
                degreecode = dscode.Tables[0].Rows[0]["degree_code"].ToString();
                coursename = course + "-" + deptname;
            }
            string str1 = string.Empty;
            string group_code = Session["group_code"].ToString();
            if (group_code.Contains(";"))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where  college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + group_code + "'";
                str1 = str1 + "  select template from master_Settings where settings='SmsAttendanceTepmlate' and group_code='" + group_code + "'and value='1'";
            }
            else
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + Session["usercode"].ToString() + "'";
                str1 = str1 + "  select template from master_Settings where settings='SmsAttendanceTepmlate' and usercode='" + Session["usercode"].ToString() + "'and value='1'";
            }
            Boolean flage = false;
            DataSet ds1;
            ds1 = da.select_method_wo_parameter(str1, "txt");
            string hodphone = da.GetFunction("  select d.PhoneNo from Department d,Degree de,staffmaster s,staff_appl_master sa where d.Dept_Code=de.Dept_Code and s.appl_no=sa.appl_no and d.Head_Of_Dept=s.staff_code and resign='0' and settled='0' and de.Dept_Code ='" + degreecode + "'");
            DataSet dsSMSSendDetails = new DataSet();
            bool hourWiseAbsent = false;//Convert.ToDateTime(date).ToString("dd/MM/yyyy")
            dsSMSSendDetails = da.select_method_wo_parameter("select * from smsdeliverytrackmaster where Convert(varchar(20),date,103)='" + DateTime.Now.ToString("dd/MM/yyyy") + "' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "' and smsFor='absentees'", "text");
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                for (int jj = 0; jj < ds1.Tables[0].Rows.Count; jj++)
                {
                    if (ds1.Tables[0].Rows[jj]["TextName"].ToString() == "Attendance Sms for Absent" && ds1.Tables[0].Rows[jj]["Taxtval"].ToString() == "1")
                    {
                        flage = true;
                    }
                    else if (ds1.Tables[0].Rows[jj]["TextName"].ToString() == "Hour" && Convert.ToInt32(ds1.Tables[0].Rows[jj]["Taxtval"]) == 1)
                    {
                        hourWiseAbsent = true;
                    }
                    else if (ds1.Tables[0].Rows[jj]["TextName"].ToString() == "Hour" && Convert.ToInt32(ds1.Tables[0].Rows[jj]["Taxtval"]) == 0)
                    {
                        hourWiseAbsent = false;
                    }
                }
                if (flage == true)
                {
                    for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                    {
                        if (ds1.Tables[0].Rows[k]["TextName"].ToString() == "Period" && ds1.Tables[0].Rows[k]["Taxtval"].ToString() != "")
                        {
                            string splihours = ds1.Tables[0].Rows[k]["Taxtval"].ToString();
                            string[] fin_split = splihours.Split(',');
                            int count = fin_split.Length;
                            for (int i = 0; i < count; i++)
                            {
                                string final_Hours = fin_split[i];
                                if (hour_check == final_Hours)
                                {
                                    check = check + 1;
                                }
                            }
                        }
                    }
                }
            }
            if (check > 0)
            {
                check = 0;
                string ssr = "select * from Track_Value where college_code='" + Session["collegecode"].ToString() + "'";
                DataSet dstrack;
                dstrack = da.select_method_wo_parameter(ssr, "txt");
                if (dstrack.Tables.Count > 0 && dstrack.Tables[0].Rows.Count > 0)
                {
                    user_id = Convert.ToString(dstrack.Tables[0].Rows[0]["SMS_User_ID"]).Trim();
                    string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName,r.app_no from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                    DataSet dsMobile;
                    dsMobile = da.select_method_wo_parameter(Phone, "txt");
                    if (ds1.Tables.Count > 1 && ds1.Tables[1].Rows.Count > 0) //************************ added by jairam****************************** 10-10-2014
                    {
                        DateTime dt = Convert.ToDateTime(date);
                        string templatevlaue = Convert.ToString(ds1.Tables[1].Rows[0]["template"]);
                        if (templatevlaue.Trim() != "")
                        {
                            string[] splittemplate = templatevlaue.Split('$');
                            if (splittemplate.Length > 0)
                            {
                                for (int j = 0; j <= splittemplate.GetUpperBound(0); j++)
                                {
                                    if (splittemplate[j].ToString() != "")
                                    {
                                        if (splittemplate[j].ToString() == "College Name")
                                        {
                                            MsgText = MsgText + " " + collegename;
                                        }
                                        else if (splittemplate[j].ToString() == "Student Name")
                                        {
                                            MsgText = MsgText + " " + dsMobile.Tables[0].Rows[0]["StudName"].ToString();
                                        }
                                        else if (splittemplate[j].ToString() == "Degree")
                                        {
                                            MsgText = MsgText + " " + coursename;
                                        }
                                        else if (splittemplate[j].ToString() == "Section")
                                        {
                                            if (sections != "")
                                            {
                                                MsgText = MsgText + " " + "" + sections + " Section";
                                            }
                                        }
                                        else if (splittemplate[j].ToString() == "Thank You")
                                        {
                                            MsgText = MsgText + " " + splittemplate[j].ToString();
                                        }
                                        else if (splittemplate[j].ToString() == "Absent")
                                        {
                                            MsgText = MsgText + " " + Hour + " hour Absent";
                                        }
                                        //22/09/16
                                        else if (splittemplate[j].ToString() == "Date")
                                        {
                                            MsgText = MsgText + " Date: " + dt.ToString("dd/MM/yyyy") + "";
                                        }
                                        else if (splittemplate[j].ToString() == "HOD")
                                        {
                                            if (hodphone.Trim() != "")
                                            {
                                                MsgText = MsgText + " - " + hodphone;
                                            }
                                            else
                                            {
                                                MsgText = MsgText + " ";
                                            }
                                        }
                                        else if (splittemplate[j].ToString() == "Roll No")
                                        {
                                            MsgText = MsgText + " " + rollno;
                                        }
                                        else if (splittemplate[j].ToString() == "Register No")
                                        {
                                            MsgText = MsgText + " " + regno;
                                        }
                                        else if (splittemplate[j].ToString() == "Application No")
                                        {
                                            MsgText = MsgText + " " + appno;
                                        }
                                        else if (splittemplate[j].ToString() == "Admission No")
                                        {
                                            MsgText = MsgText + " " + admno;
                                        }
                                        else
                                        {
                                            if (MsgText == "")
                                            {
                                                MsgText = splittemplate[j].ToString();
                                            }
                                            else
                                            {
                                                MsgText = MsgText + " " + splittemplate[j].ToString();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MsgText = "Dear Parent, Good Morning. This Message from" + " " + collegename + ". Your ward " + dsMobile.Tables[0].Rows[0]["StudName"].ToString() + " of " + coursename + " is found absent  " + Hour + " hour. Conducted Hours:" + total + " Absent Hours:" + absent + ". Thank you";
                    }
                    if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                    {
                        string studentAppNo = Convert.ToString(dsMobile.Tables[0].Rows[0]["app_no"]).Trim();
                        for (int jj1 = 0; jj1 < ds1.Tables[0].Rows.Count; jj1++)
                        {
                            bool checkHourAbsentees = false;
                            bool isSentAbsentees = true;
                            if (hourWiseAbsent)
                            {
                                checkHourAbsentees = true;
                            }
                            DataView dvSendSMSDetails = new DataView();
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {

                                if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                                {
                                    RecepientNo = Convert.ToString(dsMobile.Tables[0].Rows[0]["FatherMobile"]).Trim();
                                    string getval = da.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[1].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    //Modified By Srinath
                                    string strpath = string.Empty;
                                    dvSendSMSDetails = new DataView();
                                    isSentAbsentees = true;
                                    if (dsSMSSendDetails.Tables.Count > 0 && dsSMSSendDetails.Tables[0].Rows.Count > 0)
                                    {
                                        dsSMSSendDetails.Tables[0].DefaultView.RowFilter = "mobilenos='" + RecepientNo + "' and studentAppNo='" + studentAppNo + "'";
                                        dvSendSMSDetails = dsSMSSendDetails.Tables[0].DefaultView;
                                    }
                                    if (checkHourAbsentees && dvSendSMSDetails.Count > 0)
                                    {
                                        isSentAbsentees = false;
                                    }
                                    //if (SenderID != "eSNCET" && Password != "yahoo10")
                                    //{
                                    //strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                    //}
                                    //else
                                    //{
                                    //    strpath = "http://49.50.69.90/api/smsapi.aspx?username=" + user_id + "&password=" + Password + "&to=" + RecepientNo + "&from=" + SenderID + "&message=" + MsgText;
                                    //}
                                    //string isst = "0";
                                    //smsreport(strpath, isst, dt);
                                    int nofosmssend = 0;
                                    if (isSentAbsentees)
                                        nofosmssend = da.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0", "absentees", studentAppNo);
                                }
                            }
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                    string getval = da.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[1].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    //Modified By SRinath /2/2014
                                    string strpath = string.Empty;
                                    dvSendSMSDetails = new DataView();
                                    isSentAbsentees = true;
                                    if (dsSMSSendDetails.Tables.Count > 0 && dsSMSSendDetails.Tables[0].Rows.Count > 0)
                                    {
                                        dsSMSSendDetails.Tables[0].DefaultView.RowFilter = "mobilenos='" + RecepientNo + "' and studentAppNo='" + studentAppNo + "'";
                                        dvSendSMSDetails = dsSMSSendDetails.Tables[0].DefaultView;
                                    }
                                    if (checkHourAbsentees && dvSendSMSDetails.Count > 0)
                                    {
                                        isSentAbsentees = false;
                                    }

                                    //if (SenderID != "eSNCET" && Password != "yahoo10")
                                    //{
                                    //strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                    //}
                                    //else
                                    //{
                                    //    strpath = "http://49.50.69.90/api/smsapi.aspx?username=" + user_id + "&password=" + Password + "&to=" + RecepientNo + "&from=" + SenderID + "&message=" + MsgText;
                                    //}
                                    //string isst = "0";
                                    //smsreport(strpath, isst, dt);
                                    int nofosmssend = 0;
                                    if (isSentAbsentees)
                                        nofosmssend = da.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0", "absentees", studentAppNo);
                                }
                            }
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                    string getval = da.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[1].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    //Modified By Srinatrh 8/2/2014
                                    string strpath = string.Empty;
                                    dvSendSMSDetails = new DataView();
                                    isSentAbsentees = true;
                                    if (dsSMSSendDetails.Tables.Count > 0 && dsSMSSendDetails.Tables[0].Rows.Count > 0)
                                    {
                                        dsSMSSendDetails.Tables[0].DefaultView.RowFilter = "mobilenos='" + RecepientNo + "' and studentAppNo='" + studentAppNo + "'";
                                        dvSendSMSDetails = dsSMSSendDetails.Tables[0].DefaultView;
                                    }
                                    if (checkHourAbsentees && dvSendSMSDetails.Count > 0)
                                    {
                                        isSentAbsentees = false;
                                    }
                                    //if (SenderID != "eSNCET" && Password != "yahoo10")
                                    //{
                                    //strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                    //}
                                    //else
                                    //{
                                    //    strpath = "http://49.50.69.90/api/smsapi.aspx?username=" + user_id + "&password=" + Password + "&to=" + RecepientNo + "&from=" + SenderID + "&message=" + MsgText;
                                    //}
                                    //string isst = "0";
                                    //smsreport(strpath, isst, dt);
                                    int nofosmssend = 0;
                                    if (isSentAbsentees)
                                        nofosmssend = da.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0", "absentees", studentAppNo);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnnotesdelete_Click(object sender, EventArgs e)
    {
        try
        {
            string actrow1 = string.Empty;
            int actcol1 = 0;
            actrow1 = Convert.ToString(FpSpread3.ActiveSheetView.ActiveRow);
            actcol1 = FpSpread3.ActiveSheetView.ActiveColumn;
            if (Convert.ToInt32(actrow1) >= 0 && actrow1 != "")
            {
                string date = FpSpread3.Sheets[0].Cells[Convert.ToInt32(actrow1), 1].Text;
                string path = FpSpread3.Sheets[0].Cells[Convert.ToInt32(actrow1), 4].Text;
                string batvhyear = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(actrow1), 0].Tag);
                string degreecode = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(actrow1), 1].Tag);
                string semester = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(actrow1), 2].Tag);
                string subno = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(actrow1), 3].Tag);
                //modified by srinath 26/8/2013
                //string deletenotes = "delete from attendance_document_save where batch_year=" + batvhyear + " and degree_code=" + degreecode + " and semester=" + semester + " and convert(varchar(20),date,105)='" + date + "' and path='" + path + "'";
                string deletenotes = "delete from notestbl where batch=" + batvhyear + " and degree_code=" + degreecode + " and sem=" + semester + " and convert(varchar(20),date,105)='" + date + "' and filename='" + path + "'";
                int a = da.update_method_wo_parameter(deletenotes, "Text");
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert(' Deleted Successfully')", true);
                retrivespreadfornotes(batvhyear, degreecode, semester, subno, date);
                btndeleteatndqtn.Enabled = false;
                btnSave.Enabled = true;
                btnnotesdelete.Enabled = false;
            }
            fileupload.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnaddnotes_Click(object sender, EventArgs e)
    {
        try
        {
            if (!fileupload.HasFile)
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The File And Then Proceed";
                return;
            }
            string strval = string.Empty;
            if (ddlclassnotes.SelectedItem.ToString().Trim().ToLower() == "all")
            {
                for (int dlt = 0; dlt < ddlclassnotes.Items.Count; dlt++)
                {
                    string setval = ddlclassnotes.Items[dlt].Value.ToString();
                    string[] spset = setval.Split('-');
                    if (spset.GetUpperBound(0) >= 5)
                    {
                        if (strval == "")
                        {
                            strval = ddlclassnotes.Items[dlt].Value.ToString();
                        }
                        else
                        {
                            strval = strval + '*' + ddlclassnotes.Items[dlt].Value.ToString();
                        }
                    }
                }
            }
            else
            {
                strval = ddlclassnotes.SelectedValue.ToString();
            }
            string[] sptree = strval.Split('*');
            Boolean savnotsflag = false;
            int actrow1 = FpSpread1.ActiveSheetView.ActiveRow;
            int actcol1 = FpSpread1.ActiveSheetView.ActiveColumn;
            string sch_dt = string.Empty;
            string[] spdatesp = FpSpread1.Sheets[0].RowHeader.Cells[actrow1, 0].Text.Split(' ');
            sch_dt = spdatesp[0].ToString();
            string degree_code = string.Empty;
            string semester = string.Empty;
            string subject_no = string.Empty;
            string batchyear = string.Empty;
            for (int trs = 0; trs <= sptree.GetUpperBound(0); trs++)
            {
                //int actrow1 = 0;
                //int actcol1 = 0;
                //string sub_name = FpSpread1.Sheets[0].Cells[actrow1, actcol1].Text;
                //string[] subj_name_split = sub_name.Split('-');
                //string subj_name = subj_name_split[0].ToString();
                //  string subcode = Convert.ToString(FpSpread1.Sheets[0].Cells[actrow1, actcol1].Tag);
                // string subcode = ddlclassnotes.SelectedValue.ToString();
                degree_code = string.Empty;
                semester = string.Empty;
                subject_no = string.Empty;
                batchyear = string.Empty;
                string subcode = sptree[trs].ToString();
                string subj_name = string.Empty;
                string treepath = string.Empty;
                if (ddlclassnotes.Items.Count > 0)
                {
                    string valsp = sptree[trs].ToString();
                    string[] sp1 = valsp.Split(new Char[] { '-' });
                    if (sp1.GetUpperBound(0) > 2)
                    {
                        string subcode1 = sp1[2].ToString();
                        subj_name = da.GetFunction("Select Subject_Name from subject where subject_no='" + subcode1 + "'");
                        treepath = subj_name + " " + "/";
                    }
                }
                lblerror.Visible = false;
                if (fileupload.HasFile)
                {
                    if (fileupload.FileName.EndsWith(".jpg") || fileupload.FileName.EndsWith(".gif") || fileupload.FileName.EndsWith(".png") || fileupload.FileName.EndsWith(".txt") || fileupload.FileName.EndsWith(".doc") || fileupload.FileName.EndsWith(".xls") || fileupload.FileName.EndsWith(".docx") || fileupload.FileName.EndsWith(".txt") || fileupload.FileName.EndsWith(".document") || fileupload.FileName.EndsWith(".xls") || fileupload.FileName.EndsWith(".xlsx") || fileupload.FileName.EndsWith(".pdf") || fileupload.FileName.EndsWith(".ppt") || fileupload.FileName.EndsWith(".pptx"))
                    {
                        string fileName = Path.GetFileName(fileupload.PostedFile.FileName);
                        string fileExtension = Path.GetExtension(fileupload.PostedFile.FileName);
                        string documentType = string.Empty;
                        switch (fileExtension)
                        {
                            case ".pdf":
                                documentType = "application/pdf";
                                break;
                            case ".xls":
                                documentType = "application/vnd.ms-excel";
                                break;
                            case ".xlsx":
                                documentType = "application/vnd.ms-excel";
                                break;
                            case ".doc":
                                documentType = "application/vnd.ms-word";
                                break;
                            case ".docx":
                                documentType = "application/vnd.ms-word";
                                break;
                            case ".gif":
                                documentType = "image/gif";
                                break;
                            case ".png":
                                documentType = "image/png";
                                break;
                            case ".jpg":
                                documentType = "image/jpg";
                                break;
                            case ".ppt":
                                documentType = "application/vnd.ms-ppt";
                                break;
                            case ".pptx":
                                documentType = "application/vnd.ms-pptx";
                                break;
                            case ".txt":
                                documentType = "application/txt";
                                break;
                        }
                        int fileSize = fileupload.PostedFile.ContentLength;
                        //Create array and read the file into it
                        byte[] documentBinary = new byte[fileSize];
                        fileupload.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                        if (subcode.ToString().Trim() != "")
                        {
                            string[] sp1 = subcode.Split(new Char[] { '-' });
                            degree_code = sp1[0];
                            semester = sp1[1];
                            subject_no = sp1[2];
                            if (sp1.GetUpperBound(0) == 7)
                            {
                                batchyear = sp1[4];
                            }
                            else
                            {
                                batchyear = sp1[3];
                            }
                            string[] spdate = sch_dt.Split(new Char[] { '-' });
                            if (spdate[0].Length == 1)
                            {
                                spdate[0] = "0" + spdate[0];
                            }
                            if (spdate[1].Length == 1)
                            {
                                spdate[1] = "0" + spdate[1];
                            }
                            sch_dt = spdate[0] + "-" + spdate[1] + "-" + spdate[2];
                            string date1 = spdate[1] + "-" + spdate[0] + "-" + spdate[2];
                            sch_dt = spdate[0] + "-" + spdate[1] + "-" + spdate[2];
                            SqlCommand cmdnotes = new SqlCommand();
                            string fileid = batchyear + "@" + degree_code + "@" + semester + "@" + subject_no;
                            cmdnotes.CommandText = "INSERT INTO notestbl(filename,filetype,filedata,date,subject_no,subject_name,degree_code,sem,batch,treeview,fileid)" + " VALUES (@DocName,@Type,@DocData,@date,@subject_no,@subject_name,@degree_code,@semester,@batch,@treepath,@fileid)";
                            cmdnotes.CommandType = CommandType.Text;
                            cmdnotes.Connection = ssql;
                            SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 50);
                            DocName.Value = fileName.ToString();
                            cmdnotes.Parameters.Add(DocName);
                            SqlParameter Type = new SqlParameter("@Type", SqlDbType.VarChar, 50);
                            Type.Value = documentType.ToString();
                            cmdnotes.Parameters.Add(Type);
                            SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                            uploadedDocument.Value = documentBinary;
                            cmdnotes.Parameters.Add(uploadedDocument);
                            SqlParameter uploadedDate = new SqlParameter("@date", SqlDbType.DateTime, 100);
                            uploadedDate.Value = date1;
                            cmdnotes.Parameters.Add(uploadedDate);
                            SqlParameter uploadedsubject_no = new SqlParameter("@subject_no", SqlDbType.VarChar, 50);
                            uploadedsubject_no.Value = subject_no;
                            cmdnotes.Parameters.Add(uploadedsubject_no);
                            SqlParameter uploadedsubject_name = new SqlParameter("@subject_name", SqlDbType.VarChar, 500);
                            uploadedsubject_name.Value = subj_name;
                            cmdnotes.Parameters.Add(uploadedsubject_name);
                            SqlParameter uploaded_deg_code = new SqlParameter("@degree_code", SqlDbType.VarChar, 50);
                            uploaded_deg_code.Value = degree_code;
                            cmdnotes.Parameters.Add(uploaded_deg_code);
                            SqlParameter uploaded_sem = new SqlParameter("@semester", SqlDbType.Int, 50);
                            uploaded_sem.Value = semester;
                            cmdnotes.Parameters.Add(uploaded_sem);
                            SqlParameter uploaded_batch_yr = new SqlParameter("@batch", SqlDbType.Int, 50);
                            uploaded_batch_yr.Value = batchyear;
                            cmdnotes.Parameters.Add(uploaded_batch_yr);
                            SqlParameter uploaded_treepath = new SqlParameter("@treepath", SqlDbType.VarChar, 500);
                            uploaded_treepath.Value = treepath;
                            cmdnotes.Parameters.Add(uploaded_treepath);
                            SqlParameter uploaded_id = new SqlParameter("@fileid", SqlDbType.VarChar, 500);
                            uploaded_id.Value = fileid;
                            cmdnotes.Parameters.Add(uploaded_id);
                            //string insertquery = " INSERT INTO notestbl(filename,filetype,filedata,date,subject_no,subject_name,degree_code,sem,batch,treeview,fileid)";
                            //insertquery = insertquery + " values ('" + fileName + "','" + documentType + "'," + documentBinary + ",'" + date1 + "','" + subject_no + "','" + subj_name + "','" + degree_code + "','" + semester + "','" + batchyear + "','" + treepath + "','" + fileid + "')";
                            ssql.Close();
                            ssql.Open();
                            int result = cmdnotes.ExecuteNonQuery();
                            savnotsflag = true;
                        }
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Selected file format is Not allowed";
                    }
                }
                else
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Selected file format is Not allowed";
                }
            }
            if (savnotsflag == true)
            {
                retrivespreadfornotes(batchyear, degree_code, semester, subject_no, sch_dt);
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert(' Saved Successfully')", true);
            }
            //else
            //{
            //    lblerror.Visible = true;
            //    lblerror.Text = "Select the Topic And Proceed";
            //}
            fileupload.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    protected void fpspread3_click(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        cellclick3 = true;
    }

    protected void fpspread3prerender(Object sender, EventArgs e)
    {
        try
        {
            bool x = FpSpread3.Sheets[0].AutoPostBack;
            if (cellclick3 == true)
            {
                btnSave.Enabled = false;
                btnnotesdelete.Enabled = true;
                string activerow = string.Empty;
                string activecol = string.Empty;
                activerow = FpSpread3.ActiveSheetView.ActiveRow.ToString();
                activecol = FpSpread3.ActiveSheetView.ActiveColumn.ToString();
                if (Convert.ToInt32(activecol) == 4)
                {
                    string fileName = string.Empty;
                    //  string fileid = FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Tag + "@" + FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text + "@" + FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Tag + "@" + FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text;
                    string fileid = FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag.ToString();
                    path1 = FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                    //---------------------------------------19/6/12 PRABHA
                    //SqlCommand cmd = new SqlCommand("SELECT filename,filedata,filetype FROM notestbl WHERE fileid='" + fileid + "' and filename='" + path1 + "'", con);// and degree_code="++", con);
                    string strquer = "SELECT filename,filedata,filetype FROM notestbl WHERE fileid='" + fileid + "' and filename='" + path1 + "'";
                    DataSet dsquery = da.select_method(strquer, hat, "Text");
                    if (dsquery.Tables.Count > 0 && dsquery.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                        {
                            //Response.ContentType = dReader["filetype"].ToString();
                            //Response.AddHeader("Content-Disposition", "attachment;filename=\"" + dReader["filename"] + "\"");
                            //Response.BinaryWrite((byte[])dReader["filedata"]);
                            Response.ContentType = dsquery.Tables[0].Rows[i]["filetype"].ToString();
                            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + dsquery.Tables[0].Rows[i]["filename"] + "\"");
                            Response.BinaryWrite((byte[])dsquery.Tables[0].Rows[i]["filedata"]);
                            Response.End();
                            cellclick3 = false;
                        }
                    }
                }
                fileupload.Focus();
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void retrivespreadfornotes(string batchyear, string degree_code, string semester, string subject_no, string datenew)
    {
        try
        {
            if (ddlselectmanysub.Items.Count > 0)
            {
                string valsp = ddlselectmanysub.SelectedValue.ToString();
                string[] sp1 = valsp.Split(new Char[] { '-' });
                if (sp1.GetUpperBound(0) > 2)
                {
                    string subcode = sp1[2].ToString();
                    string subname = da.GetFunction("Select Subject_Name from subject where subject_no='" + subcode + "'");
                    selectedpath = subname + " " + "/";
                }
            }
            FpSpread3.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
            MyStyle.Font.Bold = true;
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FpSpread3.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            FpSpread3.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].DefaultStyle.Font.Bold = false;
            FpSpread3.Sheets[0].RowHeader.Visible = false;
            FpSpread3.Sheets[0].RowCount = 0;
            FpSpread3.Sheets[0].ColumnCount = 5;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Topic";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Path";
            FpSpread3.Sheets[0].Columns[4].CellType = hypertext;
            FpSpread3.Sheets[0].Columns[4].ForeColor = Color.Black;
            FpSpread3.Sheets[0].Columns[0].Width = 60;
            FpSpread3.Sheets[0].Columns[1].Width = 80;
            FpSpread3.Sheets[0].Columns[2].Width = 120;
            FpSpread3.Sheets[0].Columns[3].Width = 80;
            FpSpread3.Sheets[0].Columns[4].Width = 200;
            FarPoint.Web.Spread.TextCellType txt1 = new FarPoint.Web.Spread.TextCellType();
            FpSpread3.Sheets[0].Columns[1].CellType = txt1;
            // string getdate = "select date,path,subject_name,subject.subject_no,treeview from attendance_document_save,subject where subject.subject_no=attendance_document_save.subject_no and batch_year=" + batchyear + " and degree_code=" + degree_code + " and semester=" + semester + " and treeview like'" + selectedpath + "%' and attendance_document_save.subject_no=" + subject_no + " and convert(varchar(20),attendance_document_save.date,105)='" + datenew + "'";
            string getdate = "select filename,date,subject_name,subject_no,treeview from notestbl where subject_no=" + subject_no + " and batch=" + batchyear + " and degree_code=" + degree_code + " and sem=" + semester + " and subject_no=" + subject_no + " and treeview like'" + selectedpath + "%' ";//and date='"+datetime+"'";//and convert(varchar(20),attendance_document_save.date,105)='" + datenew + "'";
            //Modified by srinath 12/9/2013
            //  SqlDataAdapter dagetdate = new SqlDataAdapter(getdate, ssql);
            DataSet dsgetdate = new DataSet();
            dsgetdate = da.select_method(getdate, hat, "Text");
            if (dsgetdate.Tables.Count > 0 && dsgetdate.Tables[0].Rows.Count > 0)
            {
                FpSpread3.Visible = true;
                string date = string.Empty;
                string subject = string.Empty;
                string path = string.Empty;
                string subjectno = string.Empty;
                string treepath = string.Empty;
                int sno = 0;
                for (int i = 0; i < dsgetdate.Tables[0].Rows.Count; i++)
                {
                    string selecttopic = string.Empty;
                    string date1 = string.Empty;
                    string[] treepath1 = new string[10];
                    string getpathname = string.Empty;
                    string topic = string.Empty;
                    int maxpath = 0;
                    sno++;
                    date = dsgetdate.Tables[0].Rows[i]["date"].ToString();
                    string[] spdate = date.Split(new Char[] { '/' });
                    string[] spyear = spdate[2].Split(new char[] { ' ' });
                    if (spdate[0].Length == 1)
                    {
                        spdate[0] = "0" + spdate[0];
                    }
                    if (spdate[1].Length == 1)
                    {
                        spdate[1] = "0" + spdate[1];
                    }
                    date1 = spdate[1] + "-" + spdate[0] + "-" + spyear[0];
                    subject = dsgetdate.Tables[0].Rows[i]["subject_name"].ToString();
                    subjectno = dsgetdate.Tables[0].Rows[i]["subject_no"].ToString();
                    path = dsgetdate.Tables[0].Rows[i]["filename"].ToString();
                    treepath = dsgetdate.Tables[0].Rows[i]["treeview"].ToString();
                    string[] treepath2 = treepath.Split(new char[] { '=' });
                    if (treepath2.GetUpperBound(0) > 0)
                    {
                        for (int i1 = 0; i1 <= treepath2.GetUpperBound(0); i1++)
                        {
                            treepath1 = treepath2[i1].Split(new char[] { '/' });
                            maxpath = treepath1.GetUpperBound(0);
                            if (treepath1.GetUpperBound(0) > 1)
                            {
                                topic = treepath1[maxpath];
                                getpathname = "select * from sub_unit_details where subject_no = " + subjectno + " and topic_no=" + topic + " order by topic_no";
                            }
                            else
                            {
                                topic = treepath1[0];
                                getpathname = "select * from sub_unit_details where subject_no = " + subjectno + "  order by topic_no";
                            }
                            lblerrorquestionadd_att.Visible = false;
                            //Modified by srinath 12/9/2013
                            DataSet dsgetpathname = new DataSet();
                            dsgetpathname = da.select_method(getpathname, hat, "Text");
                            if (dsgetpathname.Tables.Count > 0 && dsgetpathname.Tables[0].Rows.Count > 0)
                            {
                                if (selecttopic == "")
                                {
                                    selecttopic = dsgetpathname.Tables[0].Rows[0]["unit_name"].ToString();
                                }
                                else
                                {
                                    selecttopic = selecttopic + "," + dsgetpathname.Tables[0].Rows[0]["unit_name"].ToString();
                                }
                            }
                            if (selecttopic == "")
                            {
                                selecttopic = treepath1[0];
                            }
                        }
                    }
                    else
                    {
                        treepath1 = treepath.Split(new char[] { '/' });
                        maxpath = treepath1.GetUpperBound(0);
                        if (treepath1.GetUpperBound(0) > 1)
                        {
                            topic = treepath1[maxpath];
                            getpathname = "select * from sub_unit_details where subject_no = " + subjectno + " and topic_no=" + topic + " order by topic_no";
                        }
                        else
                        {
                            topic = treepath1[0];
                            getpathname = "select * from sub_unit_details where subject_no = " + subjectno + "  order by topic_no";
                        }
                        lblerrorquestionadd_att.Visible = false;
                        //Modified by srinath 12/9/2013
                        DataSet dsgetpathname = new DataSet();
                        dsgetpathname = da.select_method(getpathname, hat, "Text");
                        if (dsgetpathname.Tables.Count > 0 && dsgetpathname.Tables[0].Rows.Count > 0)
                        {
                            if (selecttopic == "")
                            {
                                selecttopic = dsgetpathname.Tables[0].Rows[0]["unit_name"].ToString();
                            }
                            else
                            {
                                selecttopic = selecttopic + "," + dsgetpathname.Tables[0].Rows[0]["unit_name"].ToString();
                            }
                        }
                        if (selecttopic == "")
                        {
                            selecttopic = treepath1[0];
                        }
                    }
                    if (treepath != "" && selecttopic != "")
                    {
                        FpSpread3.Sheets[0].RowCount = FpSpread3.Sheets[0].RowCount + 1;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Text = date1;
                        //FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Tag = spdate[0] + "-" + spdate[1] + "-" + spyear[0];
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Text = subject;
                        // FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Tag = subjectno;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Text = selecttopic;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Text = path;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Tag = batchyear;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Tag = degree_code;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Tag = semester;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Tag = subjectno;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].ForeColor = Color.Black;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].BackColor = Color.Blue;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Font.Underline = true;
                        //batchyear + "@" + degree_code + "@" + semester + "@" + subject_no;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Tag = batchyear + "@" + degree_code + "@" + semester + "@" + subject_no;
                    }
                }
            }
            else
            {
                FpSpread3.Sheets[0].RowCount = 0;
                FpSpread3.Visible = false;
            }
            FpSpread3.Height = FpSpread3.Sheets[0].RowCount * 50;
            FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
            FpSpread3.SaveChanges();
        }
        catch (Exception ex)
        {
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        //Response.Write("<script>window.open('" + openpath + "')</script>");
        //Response.Redirect(openpath);
        //Response.RedirectToRoute(openpath);
    }

    protected void HyperLink1_PreRender(object sender, EventArgs e)
    {
        //HyperLink1.NavigateUrl = openpath;
    }

    protected void openfile(object sender, EventArgs e)
    {
        //hpl.GetRouteUrl(openpath);
    }

    protected void btnaddquestion_Click(object sender, EventArgs e)
    {
        try
        {
            string unitselected = string.Empty;
            string treepath = selectedpath;
            if (treepath != "")
            {
                if (txtquestion1.Text != "")
                {
                    if (ddlgivemarks.Text != "")
                    {
                        if (ddlunits.Items.Count > 0)
                        {
                            if (ddlunits.SelectedItem.Text.Trim() != "")
                            {
                                unitselected = ddlunits.SelectedItem.Value.ToString();
                            }
                        }
                        int actrow1 = 0;
                        int actcol1 = 0;
                        string sch_dt = string.Empty;
                        string degree_code = string.Empty;
                        string semester = string.Empty;
                        string subject_no = string.Empty;
                        string batchyear = string.Empty;
                        actrow1 = FpSpread1.ActiveSheetView.ActiveRow;
                        actcol1 = FpSpread1.ActiveSheetView.ActiveColumn;
                        string sub_name = ddlselectmanysub.SelectedItem.ToString();
                        string subcode = ddlselectmanysub.SelectedValue.ToString();
                        string[] spdatesp = FpSpread1.Sheets[0].RowHeader.Cells[actrow1, 0].Text.Split(' '); ;
                        sch_dt = spdatesp[0].ToString();
                        // sch_dt = FpSpread1.Sheets[0].RowHeader.Cells[actrow1, 0].Text;
                        if (subcode != "")
                        {
                            string[] sp1 = subcode.Split(new Char[] { '-' });
                            if (sp1.GetUpperBound(0) == 7)
                            {
                                batchyear = sp1[4];
                            }
                            else
                            {
                                batchyear = sp1[3];
                            }
                            degree_code = sp1[0];
                            semester = sp1[1];
                            subject_no = sp1[2];
                            string[] spdate = sch_dt.Split(new Char[] { '-' });
                            if (spdate[0].Length == 1)
                            {
                                spdate[0] = "0" + spdate[0];
                            }
                            if (spdate[1].Length == 1)
                            {
                                spdate[1] = "0" + spdate[1];
                            }
                            sch_dt = spdate[0] + "-" + spdate[1] + "-" + spdate[2];
                            string date1 = spdate[1] + "-" + spdate[0] + "-" + spdate[2];
                            string question = txtquestion1.Text;
                            question = Regex.Replace(question, "'", ".");
                            string insertquestions = "insert into attendance_question_addition (batch_year,degree_code,semester,subject_no,date,question,marks,treeviewpath,subj_unit) values(" + batchyear + "," + degree_code + "," + semester + "," + subject_no + ",'" + date1 + "','" + question + "','" + ddlgivemarks.SelectedItem.Text + "','" + treepath + "','" + unitselected + "')";
                            int a = da.insert_method(insertquestions, hat, "Text");
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert(' Added Successfully')", true);
                            retrievespreadattendancequestion(batchyear, degree_code, semester, subject_no, sch_dt);
                            txtquestion1.Text = string.Empty;
                            ddlgivemarks.SelectedValue = "0";
                        }
                    }
                    else
                    {
                        lblerrorquestionadd_att.Visible = true;
                        lblerrorquestionadd_att.Text = "Enter the Marks";
                    }
                }
                else
                {
                    lblerrorquestionadd_att.Visible = true;
                    lblerrorquestionadd_att.Text = "Enter the Question";
                }
            }
            else
            {
                lblerrorquestionadd_att.Visible = true;
                lblerrorquestionadd_att.Text = "Select the Topic And Proceed";
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void retrievespreadattendancequestion(string batchyear, string degree_code, string semester, string subject_no, string datenotes)
    {
        try
        {
            if (ddlselectmanysub.Items.Count > 0)
            {
                string valsp = ddlselectmanysub.SelectedValue.ToString();
                string[] sp1 = valsp.Split(new Char[] { '-' });
                if (sp1.GetUpperBound(0) > 2)
                {
                    string subcode = sp1[2].ToString();
                    string subname = da.GetFunction("Select Subject_Name from subject where subject_no='" + subcode + "'");
                    selectedpath = subname + " " + "/";
                }
            }
            string getdate = "select date,question,marks,subject_name,subject.subject_no,qtn,treeviewpath from attendance_question_addition,subject where subject.subject_no=attendance_question_addition.subject_no and batch_year=" + batchyear + " and degree_code=" + degree_code + " and semester=" + semester + "and attendance_question_addition.subject_no=" + subject_no + " and treeviewpath like '" + selectedpath + "%' and convert(varchar(20),attendance_question_addition.date,105)='" + datenotes + "' order by qtn asc";
            // string getdate = "select date,question,marks,subject_name,subject.subject_no,qtn,treeviewpath,s.unit_name from attendance_question_addition,subject,sub_unit_details s  where subject.subject_no=attendance_question_addition.subject_no and s.subject_no=attendance_question_addition.subject_no and batch_year=" + batchyear + " and degree_code=" + degree_code + " and semester=" + semester + "and attendance_question_addition.subject_no=" + subject_no + " and treeviewpath like '" + selectedpath + "%' and convert(varchar(20),attendance_question_addition.date,105)='" + datenotes + "' order by qtn asc";
            DataSet dsgetdate = new DataSet();
            dsgetdate = da.select_method(getdate, hat, "Text");
            if (dsgetdate.Tables.Count > 0 && dsgetdate.Tables[0].Rows.Count > 0)
            {
                spreadatt_qtnadd.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
                MyStyle.Font.Bold = true;
                MyStyle.Font.Name = "Book Antiqua";
                MyStyle.Font.Size = FontUnit.Medium;
                MyStyle.HorizontalAlign = HorizontalAlign.Center;
                MyStyle.ForeColor = Color.Black;
                MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                spreadatt_qtnadd.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                spreadatt_qtnadd.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                spreadatt_qtnadd.Sheets[0].DefaultStyle.Font.Bold = false;
                spreadatt_qtnadd.Sheets[0].RowHeader.Visible = false;
                spreadatt_qtnadd.Sheets[0].AutoPostBack = true;
                spreadatt_qtnadd.Sheets[0].RowCount = 0;
                spreadatt_qtnadd.Sheets[0].ColumnCount = 6;
                spreadatt_qtnadd.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                spreadatt_qtnadd.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
                spreadatt_qtnadd.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject";
                spreadatt_qtnadd.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Units";
                spreadatt_qtnadd.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Question";
                spreadatt_qtnadd.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Marks";
                spreadatt_qtnadd.Sheets[0].Columns[0].Width = 60;
                spreadatt_qtnadd.Sheets[0].Columns[1].Width = 80;
                spreadatt_qtnadd.Sheets[0].Columns[2].Width = 120;
                spreadatt_qtnadd.Sheets[0].Columns[3].Width = 80;
                spreadatt_qtnadd.Sheets[0].Columns[4].Width = 200;
                spreadatt_qtnadd.Sheets[0].Columns[5].Width = 60;
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                spreadatt_qtnadd.Sheets[0].Columns[1].CellType = txt;
                btnupdatequetion.Enabled = false;
                btnaddquestion.Enabled = true;
                btndeleteatndqtn.Enabled = false;//Added by Srinath 21/8/2013
                spreadatt_qtnadd.Visible = true;
                string date = string.Empty;
                string subject = string.Empty;
                //string path =string.Empty;
                string question = string.Empty;
                string qtn_no = string.Empty;
                string mark = string.Empty;
                string treepath = string.Empty;
                string subjectno = string.Empty;
                int sno = 0;
                if (dsgetdate.Tables.Count > 0 && dsgetdate.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsgetdate.Tables[0].Rows.Count; i++)
                    {
                        sno++;
                        DateTime sa = Convert.ToDateTime(dsgetdate.Tables[0].Rows[i]["date"].ToString());
                        date = dsgetdate.Tables[0].Rows[i]["date"].ToString();
                        string[] spdate = date.Split(new Char[] { '/' });
                        if (spdate[0].Length == 1)
                        {
                            spdate[0] = "0" + spdate[0];
                        }
                        if (spdate[1].Length == 1)
                        {
                            spdate[1] = "0" + spdate[1];
                        }
                        string[] spyear = spdate[2].Split(new char[] { ' ' });
                        string date1 = spdate[1] + "-" + spdate[0] + "-" + spyear[0];
                        subject = dsgetdate.Tables[0].Rows[i]["subject_name"].ToString();
                        question = dsgetdate.Tables[0].Rows[i]["question"].ToString();
                        mark = dsgetdate.Tables[0].Rows[i]["marks"].ToString();
                        qtn_no = dsgetdate.Tables[0].Rows[i]["qtn"].ToString();
                        treepath = dsgetdate.Tables[0].Rows[i]["treeviewpath"].ToString();
                        subjectno = dsgetdate.Tables[0].Rows[i]["subject_no"].ToString();
                        string selecttopic = string.Empty;
                        string[] treepath2 = treepath.Split(new char[] { '=' });
                        if (treepath2.GetUpperBound(0) > 0)
                        {
                            for (int i1 = 0; i1 <= treepath2.GetUpperBound(0); i1++)
                            {
                                string[] treepath1 = treepath2[i1].Split(new char[] { '/' });
                                int maxpath = treepath1.GetUpperBound(0);
                                string topic = string.Empty;
                                string getpathname = string.Empty;
                                if (treepath1.GetUpperBound(0) > 1)
                                {
                                    topic = treepath1[maxpath];
                                    getpathname = "select * from sub_unit_details where subject_no = " + subjectno + " and topic_no=" + topic + " order by topic_no";
                                }
                                else
                                {
                                    topic = treepath1[0];
                                    getpathname = "select * from sub_unit_details where subject_no = " + subjectno + "  order by topic_no";
                                }
                                lblerrorquestionadd_att.Visible = false;
                                //Modified by srinath 12/9/2013
                                //  SqlDataAdapter dagetpathname = new SqlDataAdapter(getpathname, con2);
                                DataSet dsgetpathname = new DataSet();
                                dsgetpathname = da.select_method(getpathname, hat, "Text");
                                if (dsgetpathname.Tables.Count > 0 && dsgetpathname.Tables[0].Rows.Count > 0)
                                {
                                    if (selecttopic == "")
                                    {
                                        selecttopic = dsgetpathname.Tables[0].Rows[0]["unit_name"].ToString();
                                    }
                                    else
                                    {
                                        selecttopic = selecttopic + "," + dsgetpathname.Tables[0].Rows[0]["unit_name"].ToString();
                                    }
                                }
                                if (selecttopic == "")
                                {
                                    selecttopic = treepath1[0];
                                }
                            }
                        }
                        else
                        {
                            string[] treepath1 = treepath.Split(new char[] { '/' });
                            int maxpath = treepath1.GetUpperBound(0);
                            string topic = string.Empty;
                            string getpathname = string.Empty;
                            if (treepath1.GetUpperBound(0) > 1)
                            {
                                topic = treepath1[maxpath];
                                getpathname = "select * from sub_unit_details where subject_no = " + subjectno + " and topic_no=" + topic + " order by topic_no";
                            }
                            else
                            {
                                topic = treepath1[0];
                                getpathname = "select * from sub_unit_details where subject_no = " + subjectno + "  order by topic_no";
                            }
                            lblerrorquestionadd_att.Visible = false;
                            DataSet dsgetpathname = new DataSet();
                            dsgetpathname = da.select_method(getpathname, hat, "Text");
                            if (dsgetpathname.Tables.Count > 0 && dsgetpathname.Tables[0].Rows.Count > 0)
                            {
                                if (selecttopic == "")
                                {
                                    selecttopic = dsgetpathname.Tables[0].Rows[0]["unit_name"].ToString();
                                }
                                else
                                {
                                    selecttopic = selecttopic + "," + dsgetpathname.Tables[0].Rows[0]["unit_name"].ToString();
                                }
                            }
                            if (selecttopic == "")
                            {
                                selecttopic = treepath1[0];
                            }
                        }
                        if (treepath != "" && selecttopic != "")
                        {
                            string strunitsno = da.GetFunction("select subj_unit from attendance_question_addition where qtn='" + qtn_no + "' and date='" + date + "' and subject_no='" + subjectno + "' and degree_code='" + degree_code + "' and batch_year='" + batchyear + "' and semester='" + semester + "'");
                            string strunits = da.GetFunction("select unit_name from sub_unit_details where topic_no='" + strunitsno + "'");
                            spreadatt_qtnadd.Sheets[0].RowCount = spreadatt_qtnadd.Sheets[0].RowCount + 1;
                            spreadatt_qtnadd.Sheets[0].Cells[spreadatt_qtnadd.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                            spreadatt_qtnadd.Sheets[0].Cells[spreadatt_qtnadd.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            spreadatt_qtnadd.Sheets[0].Cells[spreadatt_qtnadd.Sheets[0].RowCount - 1, 1].Text = date1;
                            spreadatt_qtnadd.Sheets[0].Cells[spreadatt_qtnadd.Sheets[0].RowCount - 1, 1].Tag = spdate[0] + "-" + spdate[1] + "-" + spyear[0];
                            spreadatt_qtnadd.Sheets[0].Cells[spreadatt_qtnadd.Sheets[0].RowCount - 1, 2].Text = subject;
                            spreadatt_qtnadd.Sheets[0].Cells[spreadatt_qtnadd.Sheets[0].RowCount - 1, 2].Tag = subjectno;
                            spreadatt_qtnadd.Sheets[0].Cells[spreadatt_qtnadd.Sheets[0].RowCount - 1, 3].Text = strunits;
                            spreadatt_qtnadd.Sheets[0].Cells[spreadatt_qtnadd.Sheets[0].RowCount - 1, 3].Note = strunitsno;
                            spreadatt_qtnadd.Sheets[0].Cells[spreadatt_qtnadd.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            spreadatt_qtnadd.Sheets[0].Cells[spreadatt_qtnadd.Sheets[0].RowCount - 1, 4].Text = question;
                            spreadatt_qtnadd.Sheets[0].Cells[spreadatt_qtnadd.Sheets[0].RowCount - 1, 4].Tag = qtn_no;
                            spreadatt_qtnadd.Sheets[0].Cells[spreadatt_qtnadd.Sheets[0].RowCount - 1, 5].Text = mark;
                            spreadatt_qtnadd.Sheets[0].Cells[spreadatt_qtnadd.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            spreadatt_qtnadd.Sheets[0].Cells[spreadatt_qtnadd.Sheets[0].RowCount - 1, 0].Tag = batchyear;
                            spreadatt_qtnadd.Sheets[0].Cells[spreadatt_qtnadd.Sheets[0].RowCount - 1, 1].Tag = degree_code;
                            spreadatt_qtnadd.Sheets[0].Cells[spreadatt_qtnadd.Sheets[0].RowCount - 1, 2].Tag = semester;
                            spreadatt_qtnadd.Sheets[0].Cells[spreadatt_qtnadd.Sheets[0].RowCount - 1, 3].Tag = subject_no;
                        }
                        else
                        {
                            lblerrorquestionadd_att.Visible = true;
                            lblerrorquestionadd_att.Text = "Select the Topic and Proceed";
                        }
                    }
                }
                spreadatt_qtnadd.Height = spreadatt_qtnadd.Sheets[0].RowCount * 50;
                spreadatt_qtnadd.Sheets[0].PageSize = spreadatt_qtnadd.Sheets[0].RowCount * 50;
                spreadatt_qtnadd.SaveChanges();
            }
            else
            {
                spreadatt_qtnadd.Sheets[0].RowCount = 0;
                spreadatt_qtnadd.Visible = false;
                spreadatt_qtnadd.Height = spreadatt_qtnadd.Sheets[0].RowCount * 50;
                spreadatt_qtnadd.Sheets[0].PageSize = spreadatt_qtnadd.Sheets[0].RowCount * 50;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btndeleteatndqtn_Click(object sender, EventArgs e)
    {
        try
        {
            string activerow = string.Empty;
            string activecol = string.Empty;
            activerow = spreadatt_qtnadd.ActiveSheetView.ActiveRow.ToString();
            activecol = spreadatt_qtnadd.ActiveSheetView.ActiveColumn.ToString();
            if (Convert.ToInt32(activerow) >= 0 && activerow != "")
            {
                string qtn = spreadatt_qtnadd.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                string Mark = spreadatt_qtnadd.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text;
                string qtn_no = Convert.ToString(spreadatt_qtnadd.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag);
                string batchyear = Convert.ToString(spreadatt_qtnadd.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                string degreecode = Convert.ToString(spreadatt_qtnadd.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                string semester = Convert.ToString(spreadatt_qtnadd.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                string subject_no = Convert.ToString(spreadatt_qtnadd.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag);
                string date2 = spreadatt_qtnadd.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                string deletequery = "delete from attendance_question_addition where qtn=" + qtn_no + "";
                //Modified by srinath 12/9/2013
                int insert = da.update_method_wo_parameter(deletequery, "Text");
                //SqlCommand cmdupdate = new SqlCommand(deletequery, con1);
                //con1.Close();
                //con1.Open();
                //cmdupdate.ExecuteNonQuery();
                cellclick2 = false;
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
                retrievespreadattendancequestion(batchyear, degreecode, semester, subject_no, date2);
                btnqtnupdate.Enabled = false;
                btnaddquestion.Enabled = true;
                btndeleteatndqtn.Enabled = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnupdatequetion_Click(object sender, EventArgs e)
    {
        try
        {
            string unitselected = string.Empty;
            if (txtquestion1.Text != "")
            {
                if (ddlgivemarks.SelectedItem.Text != "")
                {
                    lblerrorquestionadd_att.Visible = false;
                    string activerow = string.Empty;
                    string activecol = string.Empty;
                    activerow = spreadatt_qtnadd.ActiveSheetView.ActiveRow.ToString();
                    activecol = spreadatt_qtnadd.ActiveSheetView.ActiveColumn.ToString();
                    if (Convert.ToInt32(activerow) >= 0 && activerow != "")
                    {
                        if (ddlunits.Items.Count > 0)
                        {
                            if (ddlunits.SelectedItem.Text.Trim() != "")
                            {
                                unitselected = ddlunits.SelectedItem.Value.ToString();
                            }
                        }
                        string qtn = spreadatt_qtnadd.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                        string Mark = spreadatt_qtnadd.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text;
                        string qtn_no = Convert.ToString(spreadatt_qtnadd.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag);
                        string batchyear = Convert.ToString(spreadatt_qtnadd.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                        string degreecode = Convert.ToString(spreadatt_qtnadd.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                        string semester = Convert.ToString(spreadatt_qtnadd.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                        string subject_no = Convert.ToString(spreadatt_qtnadd.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag);
                        string date2 = spreadatt_qtnadd.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                        string updatequery = "update attendance_question_addition set question='" + txtquestion1.Text + "', marks=" + ddlgivemarks.SelectedItem.Text + ", subj_unit='" + unitselected + "' where qtn=" + qtn_no + "";
                        //Modified by SRinath 12/9/2013
                        int insert = da.update_method_wo_parameter(updatequery, "Text");
                        //SqlCommand cmdupdate = new SqlCommand(updatequery, con1);
                        //con1.Close();
                        //con1.Open();
                        //cmdupdate.ExecuteNonQuery();
                        cellclick2 = false;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Updated Successfully')", true);
                        retrievespreadattendancequestion(batchyear, degreecode, semester, subject_no, date2);
                        txtquestion1.Text = string.Empty;
                        ddlgivemarks.SelectedValue = "0";
                        btnqtnupdate.Enabled = false;
                        btnaddquestion.Enabled = true;
                        btndeleteatndqtn.Enabled = false;
                    }
                }
                else
                {
                    lblerrorquestionadd_att.Visible = true;
                    lblerrorquestionadd_att.Text = "Enter the mark";
                }
            }
            else
            {
                lblerrorquestionadd_att.Visible = true;
                lblerrorquestionadd_att.Text = "Enter the Question";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void spreadatt_qtnadd_SelectedChange(Object sender, EventArgs e)
    {
        if (cellclick2 == true)
        {
            spreadatt_qtnadd.SaveChanges();
            string activerow = string.Empty;
            string activecol = string.Empty;
            activerow = spreadatt_qtnadd.ActiveSheetView.ActiveRow.ToString();
            activecol = spreadatt_qtnadd.ActiveSheetView.ActiveColumn.ToString();
            string qtn = spreadatt_qtnadd.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
            string Mark = spreadatt_qtnadd.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text;
            string strunits = spreadatt_qtnadd.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Note.ToString();
            for (int un = 0; un < ddlunits.Items.Count; un++)
            {
                if (ddlunits.Items[un].Value == strunits)
                {
                    ddlunits.SelectedIndex = un;
                }
            }
            string qtn_no = Convert.ToString(spreadatt_qtnadd.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag);
            txtquestion1.Text = qtn;
            txtquestion1.ToolTip = qtn_no;
            ddlgivemarks.SelectedValue = Convert.ToString(Convert.ToInt32(Mark));
            btnaddquestion.Enabled = false;
            btnqtnupdate.Enabled = true;
            btndeleteatndqtn.Enabled = true;
            btnupdatequetion.Enabled = true;//Added By Srinath 21/8/2013
        }
    }

    protected void sprdnoofchoices_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = e.SheetView.ActiveRow.ToString();
            if (flag_true == false)
            {
                for (int j = 0; j < Convert.ToInt16(sprdnoofchoices.Sheets[0].RowCount); j++)
                {
                    if (Convert.ToInt32(actrow) != j)
                    {
                        string actcol = e.SheetView.ActiveColumn.ToString();
                        string seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                        if (seltext != "System.Object")
                            sprdnoofchoices.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Value = 0;
                    }
                }
                flag_true = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlnoofanswers_SelectedIndexchanged(object sender, EventArgs e)
    {
        try
        {
            txtqtnname.Text = string.Empty;
            sprdnoofchoices.Sheets[0].RowCount = 0;
            sprdnoofchoices.SaveChanges();
            sprdnoofchoices.Sheets[0].Columns[0].Width = 60;
            sprdnoofchoices.Sheets[0].Columns[1].Width = 80;
            sprdnoofchoices.Sheets[0].Columns[2].Width = 265;//Modified By Srinath 21/8/2013 280
            sprdnoofchoices.Sheets[0].Columns[0].Locked = true;
            sprdnoofchoices.SaveChanges();
            sprdnoofchoices.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            if (ddlnoofanswers.SelectedValue.ToString() != "A")
            {
                int noofchc = Convert.ToInt32(ddlnoofanswers.SelectedItem.Text);
                for (int i = 1; i <= noofchc; i++)
                {
                    string sno = ddlnoofanswers.Items[i - 1].Value;
                    sprdnoofchoices.Sheets[0].RowCount = sprdnoofchoices.Sheets[0].RowCount + 1;
                    sprdnoofchoices.Sheets[0].Cells[sprdnoofchoices.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    sprdnoofchoices.Sheets[0].Cells[sprdnoofchoices.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnqtnsave_Click(object sender, EventArgs e)
    {
        try
        {
            string treepath = selectedpath;
            if (ddlselectmanysub.Items.Count > 0)
            {
                string valsp = ddlselectmanysub.SelectedValue.ToString();
                string[] sp1 = valsp.Split(new Char[] { '-' });
                if (sp1.GetUpperBound(0) > 2)
                {
                    string subcode = sp1[2].ToString();
                    string subname = da.GetFunction("Select Subject_Name from subject where subject_no='" + subcode + "'");
                    treepath = subname + " " + "/";
                }
            }
            if (treepath != "")
            {
                lblnorec.Visible = false;
                int ansflag = 0;
                int ansfillflag = 0;
                int qtnflag = 0;
                sprdnoofchoices.SaveChanges();
                //if (TreeView1.SelectedNode != null)
                //{
                int actrow1 = 0;
                int actcol1 = 0;
                string sch_dt = string.Empty;
                string degree_code = string.Empty;
                string semester = string.Empty;
                string subject_no = string.Empty;
                string batchyear = string.Empty;
                actrow1 = FpSpread1.ActiveSheetView.ActiveRow;
                actcol1 = FpSpread1.ActiveSheetView.ActiveColumn;
                string sub_name = FpSpread1.Sheets[0].Cells[actrow1, actcol1].Text;
                string subcode = Convert.ToString(FpSpread1.Sheets[0].Cells[actrow1, actcol1].Tag);
                string[] spdatesp = FpSpread1.Sheets[0].RowHeader.Cells[actrow1, 0].Text.Split(' '); ;
                sch_dt = spdatesp[0].ToString();
                //sch_dt = FpSpread1.Sheets[0].RowHeader.Cells[actrow1, 0].Text;
                if (subcode != "")
                {
                    if (txtqtnname.Text != "")
                    {
                        string nofans = string.Empty;
                        for (int row = 0; row < sprdnoofchoices.Sheets[0].RowCount; row++)
                        {
                            string answers = sprdnoofchoices.Sheets[0].Cells[row, 2].Text;
                            if (nofans == "")
                            {
                                nofans = answers;
                            }
                            else
                            {
                                nofans = nofans + "?" + answers;
                            }
                            nofans = Regex.Replace(nofans, "'", ".");
                            if (answers == "")
                            {
                                ansfillflag = 1;
                                lblnorec.Visible = true;
                                lblnorec.Text = "Fill all the Answers and Proceed";
                            }
                        }
                        string corretans = string.Empty;
                        for (int res = 0; res <= Convert.ToInt32(sprdnoofchoices.Sheets[0].RowCount) - 1; res++)
                        {
                            int isval = 0;
                            string s = sprdnoofchoices.Sheets[0].Cells[res, 1].Text;
                            isval = Convert.ToInt32(sprdnoofchoices.Sheets[0].Cells[res, 1].Value);
                            if (isval == 1 && ansflag == 0)
                            {
                                ansflag = 1;
                                corretans = sprdnoofchoices.Sheets[0].Cells[res, 2].Text;
                                corretans = Regex.Replace(corretans, "'", ".");
                                sprdnoofchoices.Sheets[0].Cells[res, 1].Value = false;
                            }
                            else if (ansflag == 1 && isval == 1)
                            {
                                sprdnoofchoices.Sheets[0].Cells[res, 1].Value = false;
                                ansflag = 2;
                                lblnorec.Visible = true;
                                lblnorec.Text = "Select Any one Answer as correct Answer";
                            }
                        }
                        string tough = string.Empty;
                        if (radiotough1.Checked == true)
                        {
                            tough = "1";
                        }
                        else if (radiotough2.Checked == true)
                        {
                            tough = "2";
                        }
                        else if (radiotough3.Checked == true)
                        {
                            tough = "3";
                        }
                        else if (radiotough4.Checked == true)
                        {
                            tough = "4";
                        }
                        string[] sp1 = subcode.Split(new Char[] { '-' });
                        if (sp1.GetUpperBound(0) > 0)
                        {
                            degree_code = sp1[0];
                            semester = sp1[1];
                            subject_no = sp1[2];
                            if (sp1.GetUpperBound(0) == 7)
                            {
                                batchyear = sp1[4];
                            }
                            else
                            {
                                batchyear = sp1[3];
                            }
                        }
                        string[] spdate = sch_dt.Split(new Char[] { '-' });
                        if (spdate[0].Length == 1)
                        {
                            spdate[0] = "0" + spdate[0];
                        }
                        if (spdate[1].Length == 1)
                        {
                            spdate[1] = "0" + spdate[1];
                        }
                        string date1 = spdate[1] + "-" + spdate[0] + "-" + spdate[2];
                        sch_dt = spdate[0] + "-" + spdate[1] + "-" + spdate[2];
                        string[] subject1 = sub_name.Split(new char[] { '2' });
                        sub_name = subject1[0].ToString();
                        string path = treepath;
                        string qtnname = txtqtnname.Text;
                        qtnname = Regex.Replace(qtnname, "'", "''");
                        string checkquestion = "select question from questionaddition where batch_year=" + batchyear + " and degree_code=" + degree_code + " and semester=" + semester + " and treepath='" + path + "' and collegecode=" + Session["collegecode"].ToString() + "  and question='" + qtnname + "'";
                        DataSet dscheckquestion = new DataSet();
                        dscheckquestion = da.select_method(checkquestion, hat, "Text");
                        if (dscheckquestion.Tables.Count > 0 && dscheckquestion.Tables[0].Rows.Count > 0)
                        {
                            qtnflag = 1;
                        }
                        if (ansflag == 1 && ansfillflag == 0 && qtnflag == 0)
                        {
                            //Technical English (12)/38218/38219
                            //sno++;
                            string insertquery = string.Empty;
                            if (RadioSubject.Checked == true)
                            {
                                insertquery = "insert into questionaddition (batch_year,degree_code,semester,subject_no,treepath,question,choices,correct_ans,toughness,collegecode) values(" + batchyear + "," + degree_code + "," + semester + "," + subject_no + ",'" + path + "','" + qtnname + "','" + nofans + "','" + corretans + "','" + tough + "'," + Session["collegecode"].ToString() + ")";
                            }
                            else if (RadioGeneral.Checked == true)
                            {
                                insertquery = "insert into questionaddition (treepath,question,choices,correct_ans,toughness,collegecode) values('General','" + qtnname + "','" + nofans + "','" + corretans + "','" + tough + "'," + Session["collegecode"].ToString() + ")";
                            }
                            //Modified by srinath12/9/2013
                            int insert = da.update_method_wo_parameter(insertquery, "Text");
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                            ddlnoofanswers.SelectedIndex = 0;
                            txtqtnname.Text = string.Empty;
                            sprdnoofchoices.Sheets[0].RowCount = 0;
                            radiotough1.Checked = true;
                            sprdretrivedate();
                            //bindtree();
                        }
                        else if (qtnflag == 1)
                        {
                            lblnorec.Visible = true;
                            lblnorec.Text = "Question already exist";
                        }
                        else if (ansfillflag != 0)
                        {
                            lblnorec.Visible = true;
                            lblnorec.Text = "Fill all the Answers and Proceed";
                        }
                        else if (ansflag != 1)
                        {
                            lblnorec.Visible = true;
                            lblnorec.Text = "Check any one as correct answer";
                            if (sprdnoofchoices.Sheets[0].RowCount < 1)
                            {
                                lblnorec.Visible = true;
                                lblnorec.Text = "Select No of Choices";
                            }
                        }
                    }
                    else
                    {
                        lblnorec.Visible = true;
                        lblnorec.Text = "Enter the question";
                    }
                }
            }
            else
            {
                lblnorec.Visible = true;
                lblnorec.Text = "Select the Topic And Proceed";
            }
        }
        catch
        {
        }
        //}
        //else
        //{
        //    lblnorec.Visible = true;
        //    lblnorec.Text = "Select Subject and test";
        //}
        //sprdretrivedate();
    }

    protected void RadioSubject_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            ddlnoofanswers.SelectedIndex = 0;
            txtqtnname.Text = string.Empty;
            sprdnoofchoices.Sheets[0].RowCount = 0;
            radiotough1.Checked = true;
            btnSave.Enabled = true;
            btnqtnupdate.Enabled = false;
            sprdretrivedate();
        }
        catch (Exception ex)
        {
        }
    }

    protected void RadioGeneral_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            ddlnoofanswers.SelectedIndex = 0;
            txtquestion1.Text = string.Empty;
            sprdnoofchoices.Sheets[0].RowCount = 0;
            radiotough1.Checked = true;
            btnSave.Enabled = true;
            btnqtnupdate.Enabled = false;
            sprdretrivedate();
        }
        catch (Exception ex)
        {
        }
    }

    public void sprdretrivedate()
    {
        try
        {
            if (ddlselectmanysub.Items.Count > 0)
            {
                string valsp = ddlselectmanysub.SelectedValue.ToString();
                string[] sp1 = valsp.Split(new Char[] { '-' });
                if (sp1.GetUpperBound(0) > 2)
                {
                    string subcode = sp1[2].ToString();
                    string subname = da.GetFunction("Select Subject_Name from subject where subject_no='" + subcode + "'");
                    selectedpath = subname + " " + "/";
                }
            }
            string treepath = selectedpath;
            if (treepath != "")
            {
                sprdviewdata.Visible = false;
                string path = string.Empty;
                int actrow1 = 0;
                int actcol1 = 0;
                string sch_dt = string.Empty;
                string degree_code = string.Empty;
                string semester = string.Empty;
                string subject_no = string.Empty;
                string batchyear = string.Empty;
                actrow1 = FpSpread1.ActiveSheetView.ActiveRow;
                actcol1 = FpSpread1.ActiveSheetView.ActiveColumn;
                string sub_name = FpSpread1.Sheets[0].Cells[actrow1, actcol1].Text;
                //    string subcode = Convert.ToString(FpSpread1.Sheets[0].Cells[actrow1, actcol1].Tag);
                string subcode = ddlselectmanysub.SelectedValue.ToString();
                string[] spdatesp = FpSpread1.Sheets[0].RowHeader.Cells[actrow1, 0].Text.Split(' '); ;
                sch_dt = spdatesp[0].ToString();
                // sch_dt = FpSpread1.Sheets[0].RowHeader.Cells[actrow1, 0].Text;
                string[] subject1 = sub_name.Split(new char[] { '2' });
                sub_name = subject1[0].ToString();
                path = treepath + "%";
                if (subcode != "")
                {
                    string[] sp1 = subcode.Split(new Char[] { '-' });
                    //if (sp1.GetUpperBound(0) >0)
                    //{
                    //    degree_code = sp1[0];
                    //    semester = sp1[1];
                    //    subject_no = sp1[2];
                    //    batchyear = sp1[3];
                    //}
                    //====================PRABHA 8/6/12
                    if (sp1.GetUpperBound(0) == 7)
                    {
                        degree_code = sp1[0];
                        semester = sp1[1];
                        subject_no = sp1[2];
                        batchyear = sp1[4];
                    }
                    else
                    {
                        degree_code = sp1[0];
                        semester = sp1[1];
                        subject_no = sp1[2];
                        batchyear = sp1[3];
                    }
                    //========================
                    string[] spdate = sch_dt.Split(new Char[] { '-' });
                    if (spdate[0].Length == 1)
                    {
                        spdate[0] = "0" + spdate[0];
                    }
                    if (spdate[1].Length == 1)
                    {
                        spdate[1] = "0" + spdate[1];
                    }
                    string date1 = spdate[1] + "-" + spdate[0] + "-" + spdate[2];
                    sch_dt = spdate[0] + "-" + spdate[1] + "-" + spdate[2];
                    sprdviewdata.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Question Addition Report";
                    string getdegreecode = "select dept_name,course_name from course,department,degree where degree.dept_code=department.dept_code and degree.course_id=course.course_id and degree.degree_code=" + degree_code + " and degree.college_code=" + Session["collegecode"].ToString() + "";
                    //Modified by srinath 12/9/2013
                    // SqlDataAdapter dagetdegreecode = new SqlDataAdapter(getdegreecode, con1);
                    DataSet dsgetdegreecode = new DataSet();
                    dsgetdegreecode = da.select_method(getdegreecode, hat, "Text");
                    // dagetdegreecode.Fill(dsgetdegreecode);
                    string deptname = string.Empty;
                    string coursename = string.Empty;
                    if (dsgetdegreecode.Tables.Count > 0 && dsgetdegreecode.Tables[0].Rows.Count > 0)
                    {
                        deptname = dsgetdegreecode.Tables[0].Rows[0]["dept_name"].ToString();
                        coursename = dsgetdegreecode.Tables[0].Rows[0]["course_name"].ToString();
                    }
                    if (RadioSubject.Checked == true)
                    {
                        //if (ddlsubject.SelectedItem.Text != "")
                        sprdviewdata.Sheets[0].ColumnHeader.Cells[2, 2].Text = "Subject" + " " + ":" + " " + sub_name;
                        sprdviewdata.Sheets[0].ColumnHeader.Cells[1, 0].Text = "Batch" + " " + ":" + " " + batchyear;
                        sprdviewdata.Sheets[0].ColumnHeader.Cells[1, 2].Text = "Semester" + " " + ":" + " " + semester;
                        sprdviewdata.Sheets[0].ColumnHeader.Cells[2, 0].Text = "Degree" + " " + ":" + " " + deptname + " " + "-" + " " + coursename;
                    }
                    if (RadioGeneral.Checked == true)
                    {
                        sprdviewdata.Sheets[0].ColumnHeader.Cells[2, 2].Text = " ";
                        sprdviewdata.Sheets[0].ColumnHeader.Cells[1, 0].Text = " ";
                        sprdviewdata.Sheets[0].ColumnHeader.Cells[1, 2].Text = " ";
                        sprdviewdata.Sheets[0].ColumnHeader.Cells[2, 0].Text = " ";
                    }
                    sprdviewdata.Sheets[0].ColumnHeader.Cells[1, 0].Margin.Left = 10;
                    sprdviewdata.Sheets[0].ColumnHeader.Cells[2, 0].Margin.Left = 10;
                    sprdviewdata.Sheets[0].ColumnHeader.Cells[1, 2].Margin.Left = 20;
                    sprdviewdata.Sheets[0].ColumnHeader.Cells[2, 2].Margin.Left = 20;
                    sprdviewdata.Sheets[0].ColumnHeader.Rows[1].Border.BorderColorBottom = Color.White;
                    sprdviewdata.Sheets[0].ColumnHeader.Rows[1].Border.BorderColorRight = Color.White;
                    sprdviewdata.Sheets[0].ColumnHeader.Rows[2].Border.BorderColorRight = Color.White;
                    sprdviewdata.Sheets[0].ColumnHeader.Rows[2].Font.Size = FontUnit.Small;
                    sprdviewdata.Sheets[0].ColumnHeader.Rows[2].Font.Bold = true;
                    sprdviewdata.Sheets[0].ColumnHeader.Rows[1].Font.Size = FontUnit.Small;
                    sprdviewdata.Sheets[0].ColumnHeader.Rows[1].Font.Bold = true;
                    sprdviewdata.Sheets[0].ColumnHeader.Rows[2].Border.BorderColorBottom = Color.Black;
                    sprdviewdata.Sheets[0].ColumnHeader.Rows[3].Border.BorderColorBottom = Color.Black;
                    sprdviewdata.Sheets[0].ColumnHeader.Rows[3].Border.BorderColorRight = Color.Black;
                    sprdviewdata.Sheets[0].RowCount = 0;
                    int querytype = 3;
                    if (RadioGeneral.Checked == true)
                    {
                        querytype = 0;
                        //selectquery = "select question_no,question,choices,correct_ans from questionaddition where  treepath='General'";
                    }
                    if (RadioSubject.Checked == true)
                    {
                        if (path != "")
                        {
                            querytype = 1;
                            //selectquery = "select question_no,question,choices,correct_ans from questionaddition where batch_year=" + byear + " and degree_code=" + degree_code + " and semester=" + ddlsem.SelectedItem.Text + " and subject_no=" + ddlsubject.SelectedValue.ToString() + "and treepath like '" + path + "%'";
                        }
                        else
                        {
                            querytype = 2;
                            //selectquery= "select question_no,question,choices,correct_ans from questionaddition where batch_year=" + byear + " and degree_code=" + degree_code + " and semester=" + ddlsem.SelectedItem.Text + " and subject_no=" + ddlsubject.SelectedValue.ToString() + " ";
                        }
                    }
                    //Modified by srinath 10/9/2013
                    //SqlCommand cmdquestionrpt = new SqlCommand("questionadditonretrive", con);
                    //cmdquestionrpt.CommandType = CommandType.StoredProcedure;
                    //cmdquestionrpt.Parameters.AddWithValue("@batch_year", Convert.ToInt32(batchyear));
                    //cmdquestionrpt.Parameters.AddWithValue("@degreecode", Convert.ToInt32(degree_code));
                    //cmdquestionrpt.Parameters.AddWithValue("@semester", Convert.ToInt32(semester));
                    //cmdquestionrpt.Parameters.AddWithValue("@subjectno", Convert.ToInt32(subject_no));
                    //cmdquestionrpt.Parameters.AddWithValue("@querytype", querytype);
                    //cmdquestionrpt.Parameters.AddWithValue("@collegecode", Convert.ToInt32(Session["collegecode"].ToString()));
                    //cmdquestionrpt.Parameters.AddWithValue("@path", path);
                    //SqlDataAdapter daselectquery = new SqlDataAdapter(cmdquestionrpt);
                    hat.Clear();
                    DataSet dsselectquery = new DataSet();
                    //con2.Close();
                    //con2.Open();
                    hat.Add("batch_year", Convert.ToInt32(batchyear));
                    hat.Add("degreecode", Convert.ToInt32(degree_code));
                    hat.Add("semester", Convert.ToInt32(semester));
                    hat.Add("subjectno", Convert.ToInt32(subject_no));
                    hat.Add("querytype", querytype);
                    hat.Add("collegecode", Convert.ToInt32(Session["collegecode"].ToString()));
                    hat.Add("path", path.ToString());
                    dsselectquery = da.select_method("questionadditonretrive", hat, "sp");
                    //daselectquery.Fill(dsselectquery);
                    if (dsselectquery.Tables.Count > 0 && dsselectquery.Tables[0].Rows.Count > 0)
                    {
                        sprdviewdata.Visible = true;
                        int sno = 0;
                        sprdviewdata.Sheets[0].RowCount = 0;
                        for (int row = 0; row < dsselectquery.Tables[0].Rows.Count; row++)
                        {
                            sno++;
                            string qtnno = dsselectquery.Tables[0].Rows[row]["question_no"].ToString();
                            string question = dsselectquery.Tables[0].Rows[row]["question"].ToString();
                            string choices = dsselectquery.Tables[0].Rows[row]["choices"].ToString();
                            string correctans = dsselectquery.Tables[0].Rows[row]["correct_ans"].ToString();
                            string[] choices1 = choices.Split(new char[] { '?' });
                            int choices2 = choices1.GetUpperBound(0) + 1;
                            int sno1 = 0;
                            for (int i = 0; i < choices2; i++)
                            {
                                sno1++;
                                choices = choices1[i].ToString();
                                sprdviewdata.Sheets[0].RowCount = sprdviewdata.Sheets[0].RowCount + 1;
                                sprdviewdata.Sheets[0].Cells[sprdviewdata.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                sprdviewdata.Sheets[0].Cells[sprdviewdata.Sheets[0].RowCount - 1, 0].Note = qtnno;
                                sprdviewdata.Sheets[0].Cells[sprdviewdata.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                sprdviewdata.Sheets[0].Cells[sprdviewdata.Sheets[0].RowCount - 1, 1].Text = question;
                                sprdviewdata.Sheets[0].Cells[sprdviewdata.Sheets[0].RowCount - 1, 1].Note = path;
                                sprdviewdata.Sheets[0].Cells[sprdviewdata.Sheets[0].RowCount - 1, 2].Text = sno1 + "." + " " + choices;
                                sprdviewdata.Sheets[0].Cells[sprdviewdata.Sheets[0].RowCount - 1, 3].Text = correctans;
                            }
                            sprdviewdata.SaveChanges();
                            sprdviewdata.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            sprdviewdata.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            sprdviewdata.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        }
                    }
                    else
                    {
                        sprdviewdata.Visible = false;
                        sprdviewdata.Sheets[0].RowCount = 0;
                    }
                    int rowcount = sprdviewdata.Sheets[0].RowCount;
                    sprdviewdata.Height = rowcount * 25;
                    sprdviewdata.Sheets[0].PageSize = rowcount * 25;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void sprdviewdata_cellclick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        // sprdnoofchoices.Sheets[0].RowCount = 0;
        Cellclick = true;
        // flag1 = 0;
    }

    protected void sprdviewdata_call(Object sender, EventArgs e)
    {
        try
        {
            if (Cellclick == true)
            {
                sprdnoofchoices.SaveChanges();
                sprdnoofchoices.Sheets[0].RowCount = 0;
                Session["path"] = string.Empty;
                Session["qtn_no"] = string.Empty;
                btnqtnsave.Enabled = false;
                btnqtnupdate.Enabled = true;
                //btnqtndelete.Enabled = true;
                string activerow = string.Empty;
                string activecol = string.Empty;
                activerow = sprdviewdata.ActiveSheetView.ActiveRow.ToString();
                activecol = sprdviewdata.ActiveSheetView.ActiveColumn.ToString();
                string qtn_no = sprdviewdata.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Note;
                string path = sprdviewdata.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Note;
                Session["path"] = path;
                Session["qtn_no"] = qtn_no;
                string selectforupdate = "select question,choices,correct_ans,toughness from questionaddition where collegecode=" + Session["collegecode"].ToString() + " and question_no='" + qtn_no + "'";
                //Modified by srinath 12/9/2013
                // SqlDataAdapter daselectforupdate = new SqlDataAdapter(selectforupdate, con2);
                DataSet dsselectforupdate = new DataSet();
                dsselectforupdate = da.select_method(selectforupdate, hat, "Text");
                // con2.Close();
                //con2.Open();
                // daselectforupdate.Fill(dsselectforupdate);
                string question = string.Empty;
                string choices = string.Empty;
                string correct_ans = string.Empty;
                string tough = string.Empty;
                if (dsselectforupdate.Tables.Count > 0 && dsselectforupdate.Tables[0].Rows.Count > 0)
                {
                    question = dsselectforupdate.Tables[0].Rows[0]["question"].ToString();
                    choices = dsselectforupdate.Tables[0].Rows[0]["choices"].ToString();
                    correct_ans = dsselectforupdate.Tables[0].Rows[0]["correct_ans"].ToString();
                    tough = dsselectforupdate.Tables[0].Rows[0]["toughness"].ToString();
                    if (tough == "1")
                    {
                        radiotough1.Checked = true;
                        radiotough2.Checked = false;
                        radiotough3.Checked = false;
                        radiotough4.Checked = false;
                    }
                    if (tough == "2")
                    {
                        radiotough1.Checked = false;
                        radiotough2.Checked = true;
                        radiotough3.Checked = false;
                        radiotough4.Checked = false;
                    }
                    if (tough == "3")
                    {
                        radiotough1.Checked = false;
                        radiotough2.Checked = false;
                        radiotough3.Checked = true;
                        radiotough4.Checked = false;
                    }
                    if (tough == "4")
                    {
                        radiotough1.Checked = false;
                        radiotough2.Checked = false;
                        radiotough3.Checked = false;
                        radiotough4.Checked = true;
                    }
                    txtqtnname.Text = question;
                    string[] ch1 = choices.Split(new Char[] { '?' });
                    if (ch1.GetUpperBound(0) > 0)
                    {
                        int flag = 1;
                        int totch = ch1.GetUpperBound(0);
                        ddlnoofanswers.SelectedIndex = totch;
                        for (int i = 0; i < totch + 1; i++)
                        {
                            sprdnoofchoices.Sheets[0].RowCount = sprdnoofchoices.Sheets[0].RowCount + 1;
                            sprdnoofchoices.Sheets[0].Cells[sprdnoofchoices.Sheets[0].RowCount - 1, 0].Text = ddlnoofanswers.Items[i].Value;
                            sprdnoofchoices.Sheets[0].Cells[sprdnoofchoices.Sheets[0].RowCount - 1, 2].Text = ch1[i].ToString();
                            if (ch1[i].ToString() == correct_ans && flag == 1)
                            {
                                flag = 0;
                                sprdnoofchoices.Sheets[0].Cells[sprdnoofchoices.Sheets[0].RowCount - 1, 1].Value = true;
                            }
                            sprdnoofchoices.Sheets[0].Cells[sprdnoofchoices.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            sprdnoofchoices.Sheets[0].Cells[sprdnoofchoices.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        }
                        Cellclick = false;
                        //if (flag1 == 1)
                        //{
                        //    flag1 = 1;
                        //    sprdviewdata_call(sender, e);
                        //}
                    }
                }
            }
            sprdnoofchoices.Sheets[0].PageSize = sprdnoofchoices.Sheets[0].RowCount;
            sprdnoofchoices.SaveChanges();
        }
        catch
        {
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        //modify jairam  **************** 04-09-2014 *********//
        ddlnoofanswers.SelectedIndex = 0;
        txtqtnname.Text = string.Empty;
        sprdnoofchoices.Sheets[0].RowCount = 0;
        btnqtnsave.Enabled = true;
        btnqtnupdate.Enabled = false;
        btndeleteatndqtn.Enabled = false;
        Cellclick = false;
        Session["qtn_no"] = string.Empty;
    }

    protected void btnqtnupdate_Click(object sender, EventArgs e)
    {
        try
        {
            lblnorec.Visible = false;
            sprdnoofchoices.SaveChanges();
            int ansflag = 0;
            int ansfillflag = 0;
            int qtnflag = 0;
            int actrow1 = 0;
            int actcol1 = 0;
            string sch_dt = string.Empty;
            string degree_code = string.Empty;
            string semester = string.Empty;
            string subject_no = string.Empty;
            string batchyear = string.Empty;
            actrow1 = FpSpread1.ActiveSheetView.ActiveRow;
            actcol1 = FpSpread1.ActiveSheetView.ActiveColumn;
            string sub_name = FpSpread1.Sheets[0].Cells[actrow1, actcol1].Text;
            string subcode = Convert.ToString(FpSpread1.Sheets[0].Cells[actrow1, actcol1].Tag);
            sch_dt = FpSpread1.Sheets[0].RowHeader.Cells[actrow1, 0].Text;
            string[] spdatesp = FpSpread1.Sheets[0].RowHeader.Cells[actrow1, 0].Text.Split(' '); ;
            sch_dt = spdatesp[0].ToString();
            if (subcode != "")
            {
                string[] sp1 = subcode.Split(new Char[] { '-' });
                if (sp1.GetUpperBound(0) > 0)
                {
                    degree_code = sp1[0];
                    semester = sp1[1];
                    subject_no = sp1[2];
                    batchyear = sp1[3];
                }
                string[] spdate = sch_dt.Split(new Char[] { '-' });
                if (spdate[0].Length == 1)
                {
                    spdate[0] = "0" + spdate[0];
                }
                if (spdate[1].Length == 1)
                {
                    spdate[1] = "0" + spdate[1];
                }
                string date1 = spdate[1] + "-" + spdate[0] + "-" + spdate[2];
                sch_dt = spdate[0] + "-" + spdate[1] + "-" + spdate[2];
                if (txtqtnname.Text != "")
                {
                    string corretans = string.Empty;
                    for (int res = 0; res <= Convert.ToInt32(sprdnoofchoices.Sheets[0].RowCount) - 1; res++)
                    {
                        int isval = 0;
                        string s = sprdnoofchoices.Sheets[0].Cells[res, 1].Text;
                        isval = Convert.ToInt32(sprdnoofchoices.Sheets[0].Cells[res, 1].Value);
                        if (isval == 1 && ansflag == 0)
                        {
                            ansflag = 1;
                            corretans = sprdnoofchoices.Sheets[0].Cells[res, 2].Text;
                            sprdnoofchoices.Sheets[0].Cells[res, 1].Value = false;
                        }
                        else if (ansflag == 1 && isval == 1)
                        {
                            ansflag = 2;
                            lblnorec.Visible = true;
                            lblnorec.Text = "Select Any one Answer as correct Answer";
                            sprdnoofchoices.Sheets[0].Cells[res, 1].Value = false;
                        }
                    }
                    string nofans = string.Empty;
                    for (int row = 0; row < sprdnoofchoices.Sheets[0].RowCount; row++)
                    {
                        string answers = sprdnoofchoices.Sheets[0].Cells[row, 2].Text;
                        if (nofans == "")
                        {
                            nofans = answers;
                        }
                        else
                        {
                            nofans = nofans + "?" + answers;
                        }
                        nofans = Regex.Replace(nofans, "'", ".");
                        if (answers == "")
                        {
                            ansfillflag = 1;
                            lblnorec.Visible = true;
                            lblnorec.Text = "Fill all the Answers and Proceed";
                        }
                    }
                    string tough = string.Empty;
                    if (radiotough1.Checked == true)
                    {
                        tough = "1";
                    }
                    else if (radiotough2.Checked == true)
                    {
                        tough = "2";
                    }
                    else if (radiotough3.Checked == true)
                    {
                        tough = "3";
                    }
                    else if (radiotough4.Checked == true)
                    {
                        tough = "4";
                    }
                    //string criteria = TreeView1.SelectedNode.Text;
                    //string parentcode = TreeView1.SelectedNode.Value;
                    string path = Session["path"].ToString();
                    string qtnname = txtqtnname.Text;
                    qtnname = Regex.Replace(qtnname, "'", "''");
                    string checkquestion = "select question from questionaddition where batch_year=" + batchyear + "and degree_code=" + degree_code + " and semester=" + semester + "and treepath='" + path + "' and collegecode=" + Session["collegecode"].ToString() + "  and question='" + qtnname + "'";
                    //Modified by srinath 12/9/2013
                    // SqlDataAdapter dacheckquestion = new SqlDataAdapter(checkquestion, con1);
                    DataSet dscheckquestion = new DataSet();
                    dscheckquestion = da.select_method(checkquestion, hat, "Text");
                    // con1.Close();
                    //  con1.Open();
                    //dacheckquestion.Fill(dscheckquestion);
                    if (dscheckquestion.Tables.Count > 0 && dscheckquestion.Tables[0].Rows.Count > 0)
                    {
                        qtnflag = 1;
                    }
                    if (ansflag == 1 && ansfillflag == 0 && qtnflag == 0)
                    {
                        if (Session["qtn_no"].ToString() != "")
                        {
                            string updatequery = "update questionaddition set question='" + qtnname + "',choices='" + nofans + "',correct_ans='" + corretans + "',toughness='" + tough + "',collegecode=" + Session["collegecode"].ToString() + " where collegecode=" + Session["collegecode"].ToString() + " and question_no=" + Session["qtn_no"].ToString() + "";
                            //Modified by srinath 12/8/2013
                            //  SqlCommand updatequerycmd = new SqlCommand(updatequery, con3);
                            //con3.Close();
                            //con3.Open();
                            //updatequerycmd.ExecuteNonQuery();
                            int insert = da.update_method_wo_parameter(updatequery, "Text");
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Updated Successfully')", true);
                            sprdretrivedate();
                            ddlnoofanswers.SelectedIndex = 0;
                            txtqtnname.Text = string.Empty;
                            sprdnoofchoices.Sheets[0].RowCount = 0;
                            radiotough1.Checked = true;
                            Session["qtn_no"] = string.Empty;
                            btnqtnupdate.Enabled = false;
                            //btnqtndelete.Enabled = false;
                            btnqtnsave.Enabled = true;
                            Cellclick = false;
                        }
                    }
                    else if (qtnflag == 1)
                    {
                        lblnorec.Visible = true;
                        lblnorec.Text = "Question already exist";
                    }
                    else if (ansfillflag != 0)
                    {
                        lblnorec.Visible = true;
                        lblnorec.Text = "Fill all the Answers and Proceed";
                    }
                    else if (ansflag != 1)
                    {
                        lblnorec.Visible = true;
                        lblnorec.Text = "Check any one as correct answer";
                        if (sprdnoofchoices.Sheets[0].RowCount < 1)
                        {
                            lblnorec.Visible = true;
                            lblnorec.Text = "Select No of Choices";
                        }
                    }
                }
                else
                {
                    lblnorec.Visible = true;
                    lblnorec.Text = "Enter the question";
                }
            }
        }
        catch
        {
        }
    }

    protected void btnqtndelete_Click(object sender, EventArgs e)
    {
        try
        {
            lblnorec.Visible = false;
            if (Session["qtn_no"].ToString() != "")
            {
                string deletemodquery = "delete from questionaddition where collegecode=" + Session["collegecode"].ToString() + " and question_no=" + Session["qtn_no"].ToString() + "";
                //Modified by srinath 12/8/2013
                //  SqlCommand deletemodquerycmd = new SqlCommand(deletemodquery, con1);
                //con1.Close();
                //con1.Open();
                // deletemodquerycmd.ExecuteNonQuery();
                int insert = da.update_method_wo_parameter(deletemodquery, "Text");
                Session["qtn_no"] = string.Empty;
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
                ddlnoofanswers.SelectedIndex = 0;
                txtqtnname.Text = string.Empty;
                sprdnoofchoices.Sheets[0].RowCount = 0;
                radiotough1.Checked = true;
                btnqtnupdate.Enabled = false;
                btnqtndelete.Enabled = false;
                btnqtnsave.Enabled = true;
                sprdretrivedate();
                txtquestion1.Text = string.Empty;
                Cellclick = false;
            }
        }
        catch
        {
        }
    }

    protected void btnsliplist_Click(object sender, EventArgs e)
    {
        try
        {
            if (FpSpread2.Sheets[0].RowCount > 1)
            {
                Boolean check_unmark = false;
                pnl_sliplist.Visible = false;
                spread_sliplist.SheetCorner.Columns[0].Visible = false;
                spread_sliplist.Sheets[0].RowCount = 0;
                spread_sliplist.Sheets[0].ColumnCount = 0;
                spread_sliplist.Sheets[0].ColumnCount = 5;
                spread_sliplist.Sheets[0].RowHeader.Visible = false;
                spread_sliplist.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                spread_sliplist.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
                spread_sliplist.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Hour";
                spread_sliplist.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Name";
                spread_sliplist.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Degree";
                spread_sliplist.Sheets[0].ColumnHeader.Columns[0].Width = 50;
                spread_sliplist.Sheets[0].ColumnHeader.Columns[1].Width = 100;
                spread_sliplist.Sheets[0].ColumnHeader.Columns[2].Width = 50;
                spread_sliplist.Sheets[0].ColumnHeader.Columns[3].Width = 200;
                spread_sliplist.Sheets[0].ColumnHeader.Columns[4].Width = 300;
                //int ac = 0, ar = 0;
                string active_tag = string.Empty;
                int rowcnt = 0;
                for (int colcnt = 6; colcnt < FpSpread2.Sheets[0].ColumnCount; colcnt++)
                {
                    for (rowcnt = 1; rowcnt < FpSpread2.Sheets[0].RowCount - 2; rowcnt++)
                    {
                        if (FpSpread2.Sheets[0].Cells[rowcnt, colcnt].Text == "" || FpSpread2.Sheets[0].Cells[rowcnt, colcnt].Text == " ")
                        {
                            active_tag = FpSpread2.Sheets[0].Cells[rowcnt, 5].Tag.ToString();
                            string tag_val = string.Empty;
                            tag_val = active_tag + "-" + FpSpread2.Sheets[0].ColumnHeader.Cells[1, colcnt].Tag.ToString();
                            for (int row = 0; row < spread_sliplist.Sheets[0].RowCount; row++)
                            {
                                if (tag_val == spread_sliplist.Sheets[0].Cells[(row), 0].Tag.ToString())
                                {
                                    check_unmark = true;
                                }
                            }
                            if (check_unmark == false)
                            {
                                //  if (spread_sliplist.Sheets[0].RowCount == 0)
                                {
                                    spread_sliplist.Sheets[0].RowCount++;
                                }
                                spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 0].Text = spread_sliplist.Sheets[0].RowCount.ToString();
                                spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 0].Tag = tag_val;
                                spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 1].Text = FpSpread2.Sheets[0].ColumnHeader.Cells[0, colcnt].Text;
                                spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 2].Text = FpSpread2.Sheets[0].ColumnHeader.Cells[1, colcnt].Tag.ToString();
                                spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 3].Text = da.GetFunction("select staff_name from staffmaster where staff_code='" + Session["Staff_Code"].ToString() + "'");
                                spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 4].Text = active_tag;
                                spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 0].HorizontalAlign = HorizontalAlign.Center;
                                spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 1].HorizontalAlign = HorizontalAlign.Center;
                                spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 2].HorizontalAlign = HorizontalAlign.Center;
                            }
                            check_unmark = false;
                        }
                    }
                    colcnt++;
                }
                pnl_sliplist.Height = spread_sliplist.Sheets[0].RowCount * 200;
                // if (check_unmark == true)
                {
                    pnl_sliplist.Visible = true;
                }
                Buttonupdate.Enabled = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void exit_sliplist_Click(object sender, EventArgs e)
    {
        pnl_sliplist.Visible = false;
        btnsliplist.Enabled = true;
    }

    protected void ddlselectmanysub_SelectedIndexChanged(object sender, EventArgs e)//----13/6/12 PRABHA
    {
        try
        {
            storepath = string.Empty;
            selectedpath = string.Empty;
            FpSpread3.Sheets[0].RowCount = 0;
            string[] splitvalue = ddlselectmanysub.SelectedValue.ToString().Split(new char[] { '-' });
            //Added by srinath 30/8/2013
            string getdayorder = Convert.ToString(FpSpread1.Sheets[0].Cells[ar, ac].Note);
            if (Convert.ToString(getdayorder).Trim() != "")
            {
                string[] dayorderval = getdayorder.Split(new Char[] { '-' });
                if (Convert.ToString(dayorderval[0]).Trim() != "0")
                {
                    lbldayorder.Visible = true;
                    lbldayorder.Text = "Day Order " + Convert.ToString(dayorderval[0]).Trim();
                }
                Day_Var = Convert.ToString(dayorderval[1]).Trim();
            }
            if (splitvalue.GetUpperBound(0) > 0)
            {
                filltree();
                if (splitvalue.GetUpperBound(0) == 7)
                {
                    string degree_code = splitvalue[0].ToString();
                    string semester = splitvalue[1].ToString();
                    string subject_no = splitvalue[2].ToString();
                    string batch_year = splitvalue[4].ToString();
                    retrievespreadattendancequestion(batch_year, degree_code, semester, subject_no, sel_date1);
                    retrivespreadfornotes(batch_year, degree_code, semester, subject_no, sel_date1);
                    sprdretrivedate();
                }
                else
                {
                    string degree_code = splitvalue[0].ToString();
                    string semester = splitvalue[1].ToString();
                    string subject_no = splitvalue[2].ToString();
                    string batch_year = splitvalue[3].ToString();
                    retrievespreadattendancequestion(batch_year, degree_code, semester, subject_no, sel_date1);
                    retrivespreadfornotes(batch_year, degree_code, semester, subject_no, sel_date1);
                    sprdretrivedate();
                }
                string ongetdate = FpSpread1.Sheets[0].RowHeader.Cells[ar, 0].Text;
                string[] spongetdate = ongetdate.Split(' ');
                sel_date1 = spongetdate[0].ToString();
                getcolheader = FpSpread1.Sheets[0].ColumnHeader.Cells[0, ac].Text;
                string[] sel_date_split = sel_date1.Split(new Char[] { '-' });
                getdate_new = sel_date_split[2] + "-" + sel_date_split[1] + "-" + sel_date_split[0];
                if (sel_date_split[0].Length == 1)
                {
                    sel_date_split[0] = "0" + sel_date_split[0];
                }
                if (sel_date_split[1].Length == 1)
                {
                    sel_date_split[1] = "0" + sel_date_split[1];
                }
                sel_date1 = sel_date_split[0] + "-" + sel_date_split[1] + "-" + sel_date_split[2];
                sel_date = sel_date_split[1] + "-" + sel_date_split[0] + "-" + sel_date_split[2];
                getdate = sel_date_split[2] + "-" + sel_date_split[1] + "-" + sel_date_split[0];
                //added  by aruna
                if (ddlselectmanysub.Items.Count >= 3)
                {
                    if (ddlselectmanysub.SelectedItem.Text.ToString().Trim() != "")
                    {
                        singlesubject = true;
                        singlesubjectno = Convert.ToString(ddlselectmanysub.SelectedValue);
                        load_attnd_spread();//Added by srinath 13/8/2013
                        mark_attendance();
                        load_presen_absent_count();
                    }
                    else
                    {
                        singlesubject = false;
                        load_attnd_spread();//Added by srinath 13/8/2013
                        mark_attendance();
                        load_presen_absent_count();
                    }
                }
            }
            else //added  by aruna
            {
                if (ddlselectmanysub.Items.Count >= 3)
                {
                    singlesubject = false;
                    load_attnd_spread();//Added by srinath 13/8/2013
                    mark_attendance();
                    load_presen_absent_count();
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public string findday(string curday, string deg_code, string semester, string batch_year, string sdate, string no_days, string stastdayorder)
    {
        int holiday = 0;
        if (no_days == "")
            return "";
        if (sdate != "")
        {
            string[] sp_date = sdate.Split(new Char[] { '-' });
            string start_date = sp_date[1].ToString() + "-" + sp_date[2].ToString() + "-" + sp_date[0].ToString();
            DateTime dt1 = Convert.ToDateTime(start_date);
            DateTime dt2 = Convert.ToDateTime(curday);
            TimeSpan ts = dt2 - dt1;
            string query1 = "select count(*)as count from holidaystudents  where degree_code=" + deg_code.ToString() + " and semester=" + semester.ToString() + " and holiday_date between'" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "' and isnull(Not_include_dayorder,0)<>'1'";//01.03.17 barath";";
            string holday = da.GetFunction(query1);
            if (holday != "")
                holiday = Convert.ToInt32(holday);
            int dif_days = ts.Days;
            //Added by Srinath 10/9/2013
            string leave = da.GetFunction(" select Holiday_desc from holidaystudents  where degree_code=" + deg_code.ToString() + "  and semester=" + semester.ToString() + " and  holiday_date='" + dt2.ToString("yyyy-MM-dd") + "' ");
            if (leave != null && leave != "0")
            {
                dif_days = dif_days + 1;
            }
            //=================Added by srinath 4/9/2014==============================================================================
            int dayorderchangedate = 0;
            try
            {
                string strdayorder = "select * from tbl_consider_day_order where Degree_code='" + deg_code.ToString() + "' and Batch_year='" + batch_year + "' and Semester='" + semester + "' and ((From_Date between '" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "') or (To_Date between '" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "'))";
                DataSet dsdayorderchange = da.select_method_wo_parameter(strdayorder, "Text");
                if (dsdayorderchange.Tables.Count > 0 && dsdayorderchange.Tables[0].Rows.Count > 0)
                {
                    for (int doc = 0; doc < dsdayorderchange.Tables[0].Rows.Count; doc++)
                    {
                        DateTime dtdcf = Convert.ToDateTime(dsdayorderchange.Tables[0].Rows[doc]["From_Date"].ToString());
                        DateTime dtdct = Convert.ToDateTime(dsdayorderchange.Tables[0].Rows[doc]["To_Date"].ToString());
                        for (DateTime dtdcst = dtdcf; dtdcst <= dtdct; dtdcst = dtdcst.AddDays(1))
                        {
                            if (dtdcst <= dt2)
                            {
                                dayorderchangedate = dayorderchangedate + 1;
                            }
                        }
                    }
                }
                holiday = holiday + dayorderchangedate;
            }
            catch
            {
            }
            //=================End==================================================================================================
            int nodays = Convert.ToInt32(no_days);
            int order = (dif_days - holiday) % nodays;
            order = order + 1;
            if (stastdayorder.ToString().Trim() != "")
            {
                if ((stastdayorder.ToString().Trim() != "1") && (stastdayorder.ToString().Trim() != "0"))
                {
                    order = order + (Convert.ToInt16(stastdayorder) - 1);
                    if (order == (nodays + 1))
                        order = 1;
                    else if (order > nodays)
                        order = order % nodays;
                }
            }
            //-----------------------------------------------------------
            if (order.ToString() == "0")
            {
                order = Convert.ToInt32(no_days);
            }
            string findday = string.Empty;
            if (order == 1)
                findday = "mon";
            else if (order == 2) findday = "tue";
            else if (order == 3) findday = "wed";
            else if (order == 4) findday = "thu";
            else if (order == 5) findday = "fri";
            else if (order == 6) findday = "sat";
            else if (order == 7) findday = "sun";
            if (order >= 1)
            {
                Day_Order = Convert.ToString(order) + "-" + Convert.ToString(findday);
            }
            else
            {
                Day_Order = string.Empty;
            }
            return findday;
        }
        else
            return "";
    }

    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value.ToString();
            }
        }
        return null;
    }

    public void load_presen_absent_count()
    {
        try
        {
            if (Convert.ToInt32(FpSpread2.Sheets[0].RowCount) > 1)
            {
                if (FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 2), 0].Text != "No Of Student(s) Present:")
                {
                    FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 2;
                    FpSpread2.Sheets[0].FrozenRowCount = 1;
                    FpSpread2.Sheets[0].SpanModel.Add((FpSpread2.Sheets[0].RowCount - 2), 0, 1, 7);
                    FpSpread2.Sheets[0].SpanModel.Add((FpSpread2.Sheets[0].RowCount - 1), 0, 1, 7);
                    FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                    FpSpread2.Sheets[0].Rows[(FpSpread2.Sheets[0].RowCount - 2)].CellType = textcel_type;
                    FpSpread2.Sheets[0].Rows[(FpSpread2.Sheets[0].RowCount - 1)].CellType = textcel_type;
                    FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 2), 0].Text = "No Of Student(s) Present:";
                    FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 1), 0].Text = "No Of Student(s) Absent:";
                    FpSpread2.Sheets[0].RowHeader.Cells[(FpSpread2.Sheets[0].RowCount - 2), 0].Text = " ";
                    FpSpread2.Sheets[0].RowHeader.Cells[(FpSpread2.Sheets[0].RowCount - 1), 0].Text = " ";
                }
                present_calcflag.Clear();
                absent_calcflag.Clear();
                hat.Clear();
                hat.Add("colege_code", Session["collegecode"].ToString());
                ds_attndmaster = da.select_method("ATT_MASTER_SETTING", hat, "sp");
                if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
                {
                    count_master = (ds_attndmaster.Tables[0].Rows.Count);
                    if (count_master > 0)
                    {
                        for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
                        {
                            if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
                            {
                                present_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                            }
                            if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                            {
                                absent_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                            }
                        }
                    }
                }
                for (Att_mark_column = 7; Att_mark_column < FpSpread2.Sheets[0].ColumnCount; Att_mark_column++)
                {
                    absent_count = 0;
                    present_count = 0;
                    for (Att_mark_row = 1; Att_mark_row < FpSpread2.Sheets[0].RowCount - 2; Att_mark_row++)
                    {
                        if (FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text.ToString() != "")
                        {
                            if (present_calcflag.ContainsValue(FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Note.ToString()))
                            {
                                present_count++;
                            }
                            if (absent_calcflag.ContainsValue(FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Note.ToString()))
                            {
                                absent_count++;
                            }
                        }
                    }
                    FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 2), Att_mark_column].Text = present_count.ToString();
                    FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 1), Att_mark_column].Text = absent_count.ToString();
                    Att_mark_column++;
                }
                FpSpread2.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                FpSpread2.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                string totalRows = FpSpread2.Sheets[0].RowCount.ToString();
                //FpSpread2.Height = 500;
                FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                FpSpread2.Columns.Default.Font.Name = "Book Antiqua";
                FpSpread2.Columns.Default.Font.Size = FontUnit.Medium;
                int widt = 0;
                for (int col = 0; col < FpSpread2.Sheets[0].ColumnCount; col++)
                    widt = widt + FpSpread2.Sheets[0].Columns[col].Width;
                widt = widt + FpSpread2.Sheets[0].RowHeader.Width + 18;
                FpSpread2.Height = (FpSpread2.Sheets[0].RowCount + 10) * 35 + 75;
                if (widt > 900)
                    FpSpread2.Width = 900;
                else
                    FpSpread2.Width = widt;
                FpSpread2.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                FpSpread2.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                FpSpread2.SaveChanges();
            }
        }
        catch (Exception ex)
        {
        }
    }

    //Modified By Srinath 8/2/2014
    //public void GetUserapi(string user_id)
    //{
    //    try
    //    {
    //        if (user_id == "DEANSEC")
    //        {
    //            SenderID = "DEANSE";
    //            Password = "DEANSEC";
    //        }
    //        else if (user_id == "PROFCL")
    //        {
    //            SenderID = "PROFCL";
    //            Password = "PROFCL";
    //        }
    //        else if (user_id == "SASTHA")
    //        {
    //            SenderID = "SASTHA";
    //            Password = "SASTHA";
    //        }
    //        else if (user_id == "SSMCE")
    //        {
    //            SenderID = "SSMCE";
    //            Password = "SSMCE";
    //        }
    //        else if (user_id == "NECARE")
    //        {
    //            SenderID = "NECARE";
    //            Password = "NECARE";
    //        }
    //        else if (user_id == "SVCTCG")
    //        {
    //            SenderID = "SVCTCG";
    //            Password = "SVCTCG";
    //        }
    //        else if (user_id == "AGNICT")
    //        {
    //            SenderID = "AGNICT";
    //            Password = "AGNICT";
    //        }
    //        else if (user_id == "NANDHA")
    //        {
    //            SenderID = "NANDHA";
    //            Password = "NANDHA";
    //        }
    //        else if (user_id == "DHIRA")
    //        {
    //            SenderID = "DHIRAJ";
    //            Password = "DHIRA";
    //        }
    //        else if (user_id == "ANGEL123")
    //        {
    //            SenderID = "ANGELS";
    //            Password = "ANGEL123";
    //        }
    //        else if (user_id == "BALAJI12")
    //        {
    //            SenderID = "BALAJI";
    //            Password = "BALAJI12";
    //        }
    //        else if (user_id == "AKSHYA123")
    //        {
    //            SenderID = "AKSHYA";
    //            Password = "AKSHYA";
    //        }
    //        else if (user_id == "PPGITS")
    //        {
    //            SenderID = "PPGITS";
    //            Password = "PPGITS";
    //        }
    //        else if (user_id == "PETENG")
    //        {
    //            SenderID = "PETENG";
    //            Password = "PETENG";
    //        }
    //        else if (user_id == "JJCET")
    //        {
    //            SenderID = "JJCET";
    //            Password = "JJCET";
    //        }
    //        else if (user_id == "PSVCET")
    //        {
    //            SenderID = "PSVCET";
    //            Password = "PSVCET";
    //        }
    //        else if (user_id == "AMSECE")
    //        {
    //            SenderID = "AMSECE";
    //            Password = "AMSECE";
    //        }
    //        else if (user_id == "GKMCET")
    //        {
    //            SenderID = "GKMCET";
    //            Password = "GKMCET";
    //        }
    //        else if (user_id == "SLAECT")
    //        {
    //            SenderID = "SLAECT";
    //            Password = "SLAECT";
    //        }
    //        else if (user_id == "DCTSCE")
    //        {
    //            SenderID = "DCTSCE";
    //            Password = "DCTSCE";
    //        }
    //        else if (user_id == "DCTSCE")
    //        {
    //            SenderID = "DCTSCE";
    //            Password = "DCTSCE";
    //        }
    //        else if (user_id == "DCTSEC")
    //        {
    //            SenderID = "DCTSEC";
    //            Password = "DCTSEC";
    //        }
    //        else if (user_id == "DCTSBS")
    //        {
    //            SenderID = "DCTSBS";
    //            Password = "DCTSBS";
    //        }
    //        else if (user_id == "SCTSCE")
    //        {
    //            SenderID = "SCTSCE";
    //            Password = "SCTSCE";
    //        }
    //        else if (user_id == "SCTSEC")
    //        {
    //            SenderID = "SCTSEC";
    //            Password = "SCTSEC";
    //        }
    //        else if (user_id == "SCTSBS")
    //        {
    //            SenderID = "SCTSBS";
    //            Password = "SCTSBS";
    //        }
    //        else if (user_id == "ESECED")
    //        {
    //            SenderID = "ESECED";
    //            Password = "ESECED";
    //        }
    //        else if (user_id == "IJAYAM")
    //        {
    //            SenderID = "IJAYAM";
    //            Password = "IJAYAM";
    //        }
    //        else if (user_id == "MPNMJS")
    //        {
    //            SenderID = "MPNMJS";
    //            Password = "MPNMJS";
    //        }
    //        else if (user_id == "EASACG")
    //        {
    //            SenderID = "EASACG";
    //            Password = "EASACG";
    //        }
    //        else if (user_id == "KTVRKP")
    //        {
    //            SenderID = "KTVRKP";
    //            Password = "KTVRKP";
    //        }
    //        else if (user_id == "SVSCBE")
    //        {
    //            SenderID = "SVSCBE";
    //            Password = "SVSCBE";
    //        }
    //        else if (user_id == "AIHTCH")
    //        {
    //            SenderID = "AIHTCH";
    //            Password = "AIHTCH";
    //        }
    //        else if (user_id == "NSNCET")
    //        {
    //            SenderID = "NSNCET";
    //            Password = "NSNCET";
    //        }
    //        else if (user_id == "SVICET")
    //        {
    //            SenderID = "SVICET";
    //            Password = "SVICET";
    //        }
    //        else if (user_id == "SSCENG")
    //        {
    //            SenderID = "SSCENG";
    //            Password = "SSCENG";
    //        }
    //        else if (user_id == "ECESMS")
    //        {
    //            SenderID = "ECESMS";
    //            Password = "ECESMS";
    //        }
    //        else if (user_id == "NGPTEC")
    //        {
    //            SenderID = "NGPTEC";
    //            Password = "NGPTEC";
    //        }
    //        else if (user_id == "NGPTEC")
    //        {
    //            SenderID = "NGPTEC";
    //            Password = "NGPTEC";
    //        }
    //        else if (user_id == "KSRIET")
    //        {
    //            SenderID = "KSRIET";
    //            Password = "KSRIET";
    //        }
    //        else if (user_id == "VCWSMS")
    //        {
    //            SenderID = "VCWSMS";
    //            Password = "VCWSMS";
    //        }
    //        else if (user_id == "PMCTEC")
    //        {
    //            SenderID = "PMCTEC";
    //            Password = "PMCTEC";
    //        }
    //        else if (user_id == "SRECCG")
    //        {
    //            SenderID = "SRECCG";
    //            Password = "SRECCG";
    //        }
    //        else if (user_id == "SCHCLG")
    //        {
    //            SenderID = "SCHCLG";
    //            Password = "SCHCLG";
    //        }
    //        else if (user_id == "TSMJCT")
    //        {
    //            SenderID = "TSMJCT";
    //            Password = "TSMJCT";
    //        }
    //        else if (user_id == "SRECTD")
    //        {
    //            SenderID = "SRECTD";
    //            Password = "SRECTD";
    //        }
    //        else if (user_id == "EICTPC")
    //        {
    //            SenderID = "EICTPC";
    //            Password = "EICTPC";
    //        }
    //        else if (user_id == "SHACLG")
    //        {
    //            SenderID = "SHACLG";
    //            Password = "SHACLG";
    //        }
    //        else if (user_id == "ARASUU")
    //        {
    //            SenderID = "ARASUU";
    //            Password = "ARASUU";
    //        }
    //        else if (user_id == "TECAAA")
    //        {
    //            SenderID = "TECAAA";
    //            Password = "TECAAA";
    //        }
    //        else if (user_id == "AAACET")
    //        {
    //            SenderID = "AAACET";
    //            Password = "AAACET";
    //        }
    //        else if (user_id == "SVISTE")
    //        {
    //            SenderID = "SVISTE";
    //            Password = "SVISTE";
    //        }
    //        else if (user_id == "AALIME")
    //        {
    //            SenderID = "AALIME";
    //            Password = "AALIME";
    //        }
    //        else if (user_id == "VRSCET")
    //        {
    //            SenderID = "VRSCET";
    //            Password = "VRSCET";
    //        }
    //        Session["api"] = user_id;
    //        Session["senderid"] = SenderID;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //public void GetUserapi(string user_id)
    //{
    //    try
    //    {
    //        if (user_id == "AAACET")
    //        {
    //            SenderID = "AAACET";
    //            Password = "AAACET";
    //        }
    //        else if (user_id == "AALIME")
    //        {
    //            SenderID = "AALIME";
    //            Password = "AALIME";
    //        }
    //        else if (user_id == "SVschl")
    //        {
    //            SenderID = "SVschl";
    //            Password = "SVschl";
    //        }
    //        else if (user_id == "ACETVM")
    //        {
    //            SenderID = "ACETVM";
    //            Password = "ACETVM";
    //        }
    //        else if (user_id == "AGNICT")
    //        {
    //            SenderID = "AGNICT";
    //            Password = "AGNICT";
    //        }
    //        else if (user_id == "AMSPTC")
    //        {
    //            SenderID = "AMSPTC";
    //            Password = "AMSPTC";
    //        }
    //        else if (user_id == "ANGE")
    //        {
    //            SenderID = "ANGE";
    //            Password = "ANGE";
    //        }
    //        else if (user_id == "ARASUU")
    //        {
    //            SenderID = "ARASUU";
    //            Password = "ARASUU";
    //        }
    //        else if (user_id == "DAVINC")
    //        {
    //            SenderID = "DAVINC";
    //            Password = "DAVINC";
    //        }
    //        else if (user_id == "EASACG")
    //        {
    //            SenderID = "EASACG";
    //            Password = "EASACG";
    //        }
    //        else if (user_id == "ECESMS")
    //        {
    //            SenderID = "ECESMS";
    //            Password = "ECESMS";
    //        }
    //        else if (user_id == "ESECED")
    //        {
    //            SenderID = "ESECED";
    //            Password = "ESECED";
    //        }
    //        else if (user_id == "ESENGG")
    //        {
    //            SenderID = "ESENGG";
    //            Password = "ESENGG";
    //        }
    //        else if (user_id == "ESEPTC")
    //        {
    //            SenderID = "ESEPTC";
    //            Password = "ESEPTC";
    //        }
    //        else if (user_id == "ESMSCH")
    //        {
    //            SenderID = "ESMSCH";
    //            Password = "ESMSCH";
    //        }
    //        else if (user_id == "GKMCET")
    //        {
    //            SenderID = "GKMCET";
    //            Password = "GKMCET";
    //        }
    //        else if (user_id == "IJAYAM")
    //        {
    //            SenderID = "IJAYAM";
    //            Password = "IJAYAM";
    //        }
    //        else if (user_id == "JJAAMC")
    //        {
    //            SenderID = "JJAAMC";
    //            Password = "JJAAMC";
    //        }
    //        else if (user_id == "KINGSE")
    //        {
    //            SenderID = "KINGSE";
    //            Password = "KINGSE";
    //        }
    //        else if (user_id == "KNMHSS")
    //        {
    //            SenderID = "KNMHSS";
    //            Password = "KNMHSS";
    //        }
    //        else if (user_id == "KSRIET")
    //        {
    //            SenderID = "KSRIET";
    //            Password = "KSRIET";
    //        }
    //        else if (user_id == "KTVRKP")
    //        {
    //            SenderID = "KTVRKP";
    //            Password = "KTVRKP";
    //        }
    //        else if (user_id == "MPNMJS")
    //        {
    //            SenderID = "MPNMJS";
    //            Password = "MPNMJS";
    //        }
    //        else if (user_id == "NANDHA")
    //        {
    //            SenderID = "NANDHA";
    //            Password = "NANDHA";
    //        }
    //        else if (user_id == "NECARE")
    //        {
    //            SenderID = "NECARE";
    //            Password = "NECARE";
    //        }
    //        else if (user_id == "NSNCET")
    //        {
    //            SenderID = "NSNCET";
    //            Password = "NSNCET";
    //        }
    //        else if (user_id == "PETENG")
    //        {
    //            SenderID = "PETENG";
    //            Password = "PETENG";
    //        }
    //        else if (user_id == "PMCTEC")
    //        {
    //            SenderID = "PMCTEC";
    //            Password = "PMCTEC";
    //        }
    //        else if (user_id == "PPGITS")
    //        {
    //            SenderID = "PPGITS";
    //            Password = "PPGITS";
    //        }
    //        else if (user_id == "PROFCL")
    //        {
    //            SenderID = "PROFCL";
    //            Password = "PROFCL";
    //        }
    //        else if (user_id == "PSVCET")
    //        {
    //            SenderID = "PSVCET";
    //            Password = "PSVCET";
    //        }
    //        else if (user_id == "SASTH")
    //        {
    //            SenderID = "SASTH";
    //            Password = "SASTH";
    //        }
    //        else if (user_id == "SCTSBS")
    //        {
    //            SenderID = "SCTSBS";
    //            Password = "SCTSBS";
    //        }
    //        else if (user_id == "SCTSCE")
    //        {
    //            SenderID = "SCTSCE";
    //            Password = "SCTSCE";
    //        }
    //        else if (user_id == "SCTSEC")
    //        {
    //            SenderID = "SCTSEC";
    //            Password = "SCTSEC";
    //        }
    //        else if (user_id == "SKCETC")
    //        {
    //            SenderID = "SKCETC";
    //            Password = "SKCETC";
    //        }
    //        else if (user_id == "SRECCG")
    //        {
    //            SenderID = "SRECCG";
    //            Password = "SRECCG";
    //        }
    //        else if (user_id == "SLAECT")
    //        {
    //            SenderID = "SLAECT";
    //            Password = "SLAECT";
    //        }
    //        else if (user_id == "SSCENG")
    //        {
    //            SenderID = "SSCENG";
    //            Password = "SSCENG";
    //        }
    //        else if (user_id == "SSMCEE")
    //        {
    //            SenderID = "SSMCEE";
    //            Password = "SSMCEE";
    //        }
    //        else if (user_id == "SVICET")
    //        {
    //            SenderID = "SVICET";
    //            Password = "SVICET";
    //        }
    //        else if (user_id == "SVCTCG")
    //        {
    //            SenderID = "SVCTCG";
    //            Password = "SVCTCG";
    //        }
    //        else if (user_id == "SVSCBE")
    //        {
    //            SenderID = "SVSCBE";
    //            Password = "SVSCBE";
    //        }
    //        else if (user_id == "TECENG")
    //        {
    //            SenderID = "TECENG";
    //            Password = "TECENG";
    //        }
    //        else if (user_id == "TJENGG")
    //        {
    //            SenderID = "TJENGG";
    //            Password = "TJENGG";
    //        }
    //        else if (user_id == "TSMJCT")
    //        {
    //            SenderID = "TSMJCT";
    //            Password = "TSMJCT";
    //        }
    //        else if (user_id == "VCWSMS")
    //        {
    //            SenderID = "VCWSMS";
    //            Password = "VCWSMS";
    //        }
    //        else if (user_id == "VRSCET")
    //        {
    //            SenderID = "VRSCET";
    //            Password = "VRSCET";
    //        }
    //        else if (user_id == "AUDIIT")
    //        {
    //            SenderID = "AUDIIT";
    //            Password = "AUDIIT";
    //        }
    //        else if (user_id == "SAENGG")
    //        {
    //            SenderID = "SAENGG";
    //            Password = "SAENGG";
    //        }
    //        else if (user_id == "STANE")
    //        {
    //            SenderID = "STANES";
    //            Password = "STANES";
    //        }
    //        else if (user_id == "MBCBSE")
    //        {
    //            SenderID = "MBCBSE";
    //            Password = "MBCBSE";
    //        }
    //        else if (user_id == "HIETPT")
    //        {
    //            SenderID = "HIETPT";
    //            Password = "HIETPT";
    //        }
    //        else if (user_id == "SVPITM")
    //        {
    //            SenderID = "SVPITM";
    //            Password = "SVPITM";
    //        }
    //        else if (user_id == "AUDCET")
    //        {
    //            SenderID = "AUDCET";
    //            Password = "AUDCET";
    //        }
    //        else if (user_id == "AUDWOM")
    //        {
    //            SenderID = "AUDWOM";
    //            Password = "AUDWOM";
    //        }
    //        else if (user_id == "AUDIPG")
    //        {
    //            SenderID = "AUDIPG";
    //            Password = "AUDIPG";
    //        }
    //        else if (user_id == "MCCDAY")
    //        {
    //            SenderID = "MCCDAY";
    //            Password = "MCCDAY";
    //        }
    //        else if (user_id == "MCCSFS")
    //        {
    //            SenderID = "MCCSFS";
    //            Password = "MCCSFS";
    //        }
    //        else if (user_id == "JMHRSS")
    //        {
    //            SenderID = "JMHRSS";
    //            Password = "JMHRSS";
    //        }
    //        else if (user_id == "JHSSCB")
    //        {
    //            SenderID = "JHSSCB";
    //            Password = "JHSSCB";
    //        } 
    //        Session["api"] = user_id;
    //        Session["senderid"] = SenderID;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    public void smsreport(string uril, string isstaff, DateTime dt)
    {
        try
        {
            string date = dt.ToString("MM/dd/yyyy") + ' ' + DateTime.Now.ToString("hh:mm:ss");
            WebRequest request = WebRequest.Create(uril);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader sr = new StreamReader(data);
            string strvel = sr.ReadToEnd();
            string groupmsgid = string.Empty;
            groupmsgid = strvel;
            int sms = 0;
            string smsreportinsert = string.Empty;
            string[] split_mobileno = RecepientNo.Split(new Char[] { ',' });
            for (int icount = 0; icount <= split_mobileno.GetUpperBound(0); icount++)
            {
                smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date,sender_id)values( '" + split_mobileno[icount] + "','" + groupmsgid + "','" + MsgText + "','" + Session["collegecode"].ToString() + "','" + isstaff + "','" + date + "','" + Session["UserCode"].ToString() + "')";// Added by jairam 21-11-2014
                sms = da.insert_method(smsreportinsert, hat, "Text");
            }
        }
        catch (Exception ex)
        {
        }
    }

    public Boolean Hour_lock(string degree_code, string batch_year, string semester, string prd, string secval)
    {
        string degree_var = string.Empty;
        string starttime = string.Empty;
        string endtime = string.Empty;
        string startperiod = string.Empty;
        string endperiod = string.Empty;
        string actualtime = string.Empty;
        string period = string.Empty;
        string[] sp = prd.Split(' ');
        DateTime current_time;
        DateTime start_time;
        DateTime end_time;
        Boolean lock_flag = false;
        if (sp.GetUpperBound(0) >= 1)
        {
            period = Convert.ToString(sp[1]);
        }
        hr_lock = false;
        string getlock = string.Empty;
        if (secval.Trim() != "")
        {
            getlock = da.GetFunction("select lockstatus from attendance_hrlock where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and semester='" + semester + "' and section='" + secval + "' and locktype=2");
        }
        else
        {
            getlock = da.GetFunction("select lockstatus from attendance_hrlock where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and semester='" + semester + "' and locktype=2 ");
        }
        if (getlock.Trim().ToLower() == "true" || getlock.Trim() == "1")
        {
            hr_lock = true;
        }
        if (hr_lock == true)
        {
            if (ht_period.Count > 0)
            {
                if (ht_period.Contains(Convert.ToString(period)))
                {
                    string contvar = Convert.ToString(GetCorrespondingKey(Convert.ToString(period), ht_period));
                    string[] sp_rd_semi = contvar.Split(',');
                    if (sp_rd_semi.GetUpperBound(0) >= 1) //Get Mark attendance Hrs for lock
                    {
                        startperiod = Convert.ToString(sp_rd_semi[0]);
                        endperiod = Convert.ToString(sp_rd_semi[1]);
                        if (ht_bell.Count > 0)
                        {
                            degree_var = Convert.ToString(batch_year) + "-" + Convert.ToString(degree_code) + "-" + Convert.ToString(semester) + "-" + Convert.ToString(startperiod);
                            if (ht_bell.Contains(Convert.ToString(degree_var))) //Get period start time for lock
                            {
                                string contvar1 = Convert.ToString(GetCorrespondingKey(Convert.ToString(degree_var), ht_bell));
                                string[] sp_rd_semi1 = contvar1.Split(',');
                                if (sp_rd_semi1.GetUpperBound(0) >= 1)
                                {
                                    starttime = Convert.ToString(sp_rd_semi1[0]);
                                }
                            }
                            degree_var = Convert.ToString(batch_year) + "-" + Convert.ToString(degree_code) + "-" + Convert.ToString(semester) + "-" + Convert.ToString(endperiod);
                            if (ht_bell.Contains(Convert.ToString(degree_var))) //Get period end time for lock
                            {
                                string contvar1 = Convert.ToString(GetCorrespondingKey(Convert.ToString(degree_var), ht_bell));
                                string[] sp_rd_semi1 = contvar1.Split(',');
                                if (sp_rd_semi1.GetUpperBound(0) >= 1)
                                {
                                    endtime = Convert.ToString(sp_rd_semi1[1]);
                                }
                            }
                            string sql_stringvar = "SELECT LTRIM(RIGHT(CONVERT(VARCHAR(20), GETDATE(), 100), 7))as time";
                            ds_attndmaster = da.select_method(sql_stringvar, hat, "Text");
                            if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
                            {
                                actualtime = Convert.ToString(ds_attndmaster.Tables[0].Rows[0]["time"]);
                            }
                            if (starttime.ToString().Trim() != "" && endtime.ToString().Trim() != "" && actualtime.ToString().Trim() != "")
                            {
                                current_time = Convert.ToDateTime(actualtime);
                                start_time = Convert.ToDateTime(starttime);
                                end_time = Convert.ToDateTime(endtime);
                                if (current_time >= start_time && current_time <= end_time)
                                {
                                    lock_flag = false;
                                }
                                else
                                {
                                    lock_flag = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        return lock_flag;
    }

    private void PopulateTreeview_General(string subno)
    {
        try
        {
            this.tvyet.Nodes.Clear();
            HierarchyTrees hierarchyTrees = new HierarchyTrees();
            HierarchyTrees.HTree objHTree = null;
            strquerytext = "select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + subno + "'";
            ds.Reset();
            ds.Dispose();
            ds = da.select_method(strquerytext, hat, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objHTree = new HierarchyTrees.HTree();
                    objHTree.topic_no = int.Parse(ds.Tables[0].Rows[i]["Topic_no"].ToString());
                    objHTree.parent_code = int.Parse(ds.Tables[0].Rows[i]["parent_code"].ToString());
                    objHTree.unit_name = ds.Tables[0].Rows[i]["unit_name"].ToString();
                    hierarchyTrees.Add(objHTree);
                }
            }
            foreach (HierarchyTrees.HTree hTree in hierarchyTrees)
            {
                HierarchyTrees.HTree parentNode = hierarchyTrees.Find(delegate(HierarchyTrees.HTree emp) { return emp.topic_no == hTree.parent_code; });
                if (parentNode != null)
                {
                    foreach (TreeNode tn in tvyet.Nodes)
                    {
                        if (tn.Value == parentNode.topic_no.ToString())
                        {
                            tn.ChildNodes.Add(new TreeNode(hTree.unit_name.ToString(), hTree.topic_no.ToString()));
                        }
                        if (tn.ChildNodes.Count > 0)
                        {
                            foreach (TreeNode ctn in tn.ChildNodes)
                            {
                                RecursiveChild(ctn, parentNode.topic_no.ToString(), hTree);
                            }
                        }
                    }
                }
                else
                {
                    tvyet.Nodes.Add(new TreeNode(hTree.unit_name, hTree.topic_no.ToString()));
                }
                tvyet.ExpandAll();
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void RecursiveChild(TreeNode tn, string searchValue, HierarchyTrees.HTree hTree)
    {
        if (tn.Value == searchValue)
        {
            tn.ChildNodes.Add(new TreeNode(hTree.unit_name.ToString(), hTree.topic_no.ToString()));
        }
        if (tn.ChildNodes.Count > 0)
        {
            foreach (TreeNode ctn in tn.ChildNodes)
            {
                RecursiveChild(ctn, searchValue, hTree);
            }
        }
    }

    public class HierarchyTrees : List<HierarchyTrees.HTree>
    {
        public class HTree
        {
            private int m_topic_no;
            private int m_parent_code;
            private string m_unit_name;
            public int topic_no
            {
                get { return m_topic_no; }
                set { m_topic_no = value; }
            }
            public int parent_code
            {
                get { return m_parent_code; }
                set { m_parent_code = value; }
            }
            public string unit_name
            {
                get { return m_unit_name; }
                set { m_unit_name = value; }
            }
        }
    }

    //Start===================Added by Manikandan 16/08/2013=========================
    protected void scodetxt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            strquerytext = da.GetFunction("select staff_name from staffmaster where staffmaster.staff_code='" + scodetxt.SelectedValue.ToString() + "'");
            if (strquerytext.Trim() != "" && strquerytext != null && strquerytext.Trim() != "0")
            {
                snamelbl.Text = strquerytext + "!!!!!";
                snamelbl.ForeColor = Color.Green;
                ddlstaffname.SelectedValue = scodetxt.SelectedValue.ToString();

                Buttongo_Click(sender, e);//added by rajasekar 26/10/2018
            }
            else
            {
                snamelbl.Text = "No Staff Available in this Code";
                snamelbl.ForeColor = Color.Red;
            }
            if (Session["Staff_Code_val"] != "")
            {
                snamelbl.Visible = true;
                snamelbl1.Visible = true;
            }
            string name_code = string.Empty;
            name_code = scodetxt.SelectedValue.ToString();
            Session["Staff_Code_val"] = name_code.ToString();
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlstaffname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            strquerytext = da.GetFunction("select staff_name from staffmaster where staffmaster.staff_code='" + ddlstaffname.SelectedValue.ToString() + "'");
            if (strquerytext.Trim() != "" && strquerytext != null && strquerytext.Trim() != "0")
            {
                snamelbl.Text = strquerytext + "!!!!!";
                snamelbl.ForeColor = Color.Green;
                scodetxt.SelectedValue = ddlstaffname.SelectedValue.ToString();
            }
            else
            {
                snamelbl.Text = "No Staff Available in this Code";
                snamelbl.ForeColor = Color.Red;
            }
            if (Session["Staff_Code_val"] != "")
            {
                snamelbl.Visible = true;
                snamelbl1.Visible = true;
            }
            string name_code = string.Empty;
            name_code = scodetxt.SelectedValue.ToString();
            Session["Staff_Code_val"] = name_code.ToString();
        }
        catch (Exception ex)
        {
        }
    }

    public void bindstaff()
    {
        try
        {
            scodetxt.Items.Clear();
            string staff_name = string.Empty;
            string staff_code = string.Empty;
            strquerytext = "select distinct staff_name,m.staff_code from staffmaster m,stafftrans t,hrdept_master h,desig_master d,staff_selector st where m.resign<>1 and m.settled<>1 and m.staff_code = t.staff_code and t.dept_code = h.dept_code and t.desig_code = d.desig_code and latestrec = 1 and st.staff_code=m.staff_code and m.college_code = " + Session["collegecode"] + " order by staff_name";
            ds.Dispose();
            ds.Reset();
            ds = da.select_method(strquerytext, hat, "Text");
            //Hided by gowtham////////////////
            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    staff_name = ds.Tables[0].Rows[i]["staff_name"].ToString();
            //    staff_code = ds.Tables[0].Rows[i]["staff_code"].ToString();
            //    ListItem acclist = new ListItem();
            //    acclist.Value = (staff_code.ToString());
            //    acclist.Text = (staff_name.ToString()) + "-" + (staff_code.ToString());
            //    scodetxt.Items.Add(staff_code);
            //    ddlstaffname.Items.Add(staff_name);
            //}
            /////////////////////////////
            //Added by gowtham ------------------------
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    scodetxt.DataSource = ds;
                    scodetxt.DataTextField = "staff_code";
                    scodetxt.DataValueField = "staff_code";
                    scodetxt.DataBind();
                    ddlstaffname.DataSource = ds;
                    ddlstaffname.DataTextField = "staff_name";
                    ddlstaffname.DataValueField = "staff_code";
                    ddlstaffname.DataBind();
                }
            }
            //------------------End--------------------
        }
        catch (Exception ex)
        {
        }
    }

    protected void LinkButton3_Click(object sender, EventArgs e)
    {
        try
        {
            if ((Session["StafforAdmin"] == "Staff") || (Session["StafforAdmin"] == "Admin"))
            {
                //clearfield();
                Session["StafforAdmin"] = string.Empty;
                Session["clearschedulesession"] = "clear";
                Response.Redirect("adminschedulegrid.aspx");
            }
            else
            {
                Response.Redirect("Default_login.aspx");
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void clearfield()
    {
        try
        {
            if ((Session["StafforAdmin"] == "Staff") || (Session["StafforAdmin"] == "Admin"))
            {
                pHeaderatendence.Visible = false;
                pBodyatendence.Visible = false;
                pHeaderlesson.Visible = false;
                pBodylesson.Visible = false;
                headerpanelnotes.Visible = false;
                pBodynotes.Visible = false;
                headerADDQuestion.Visible = false;
                pBodyaddquestion.Visible = false;
                headerquestionaddition.Visible = false;
                pBodyquestionaddition.Visible = false;
                ck_append.Visible = false;
                btnsliplist.Visible = false;
            }
            else
            {
                pHeaderatendence.Visible = true;
                pBodyatendence.Visible = true;
                pHeaderlesson.Visible = true;
                pBodylesson.Visible = true;
                headerpanelnotes.Visible = true;
                pBodynotes.Visible = true;
                headerADDQuestion.Visible = true;
                pBodyaddquestion.Visible = true;
                headerquestionaddition.Visible = true;
                pBodyquestionaddition.Visible = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    public string filterfunction()
    {
        string strorder = "ORDER BY Registration.Roll_No";
        string serialno = da.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
        if (serialno == "1")
        {
            strorder = "order by registration.serialno";
        }
        else
        {
            string orderby_Setting = da.GetFunction("select value from master_Settings where settings='order_by'");
            if (orderby_Setting == "0")
            {
                strorder = "ORDER BY Registration.Roll_No";
            }
            else if (orderby_Setting == "1")
            {
                strorder = "ORDER BY Registration.Reg_No";
            }
            else if (orderby_Setting == "2")
            {
                strorder = "ORDER BY Registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = "ORDER BY Registration.Roll_No,Registration.Reg_No,Registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1")
            {
                strorder = "ORDER BY Registration.Roll_No,Registration.Reg_No";
            }
            else if (orderby_Setting == "1,2")
            {
                strorder = "ORDER BY Registration.Reg_No,Registration.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                strorder = "ORDER BY Registration.Roll_No,Registration.Stud_Name";
            }
        }
        return strorder;
    }

    protected void OnTreeNodecompleteCheckChanged(object sender, EventArgs e)
    {
    }

    protected void tvcomplete_SelectedNodeChanged1(object sender, EventArgs e)
    {
    }

    public void appentiesentry()
    {
        try
        {
            FpSpread2.Visible = false;
            Buttonselectall.Visible = false;
            Buttondeselect.Visible = false;
            Buttonsave.Visible = false;
            Buttonupdate.Visible = false;
            lblmanysubject.Visible = false;
            ddlselectmanysub.Visible = false;
            lblatdate.Visible = true;
            lblcurdate.Visible = true;
            lblhour.Visible = true;
            lblhrvalue.Visible = true;
            lblattend.Visible = true;
            ddlattend.Visible = true;
            btnaddrow.Visible = true;
            fpattendanceentry.Visible = true;
            lblreststudent.Visible = true;
            ddlreststudent.Visible = true;
            lblerrmsg.Visible = true;
            btnaddattendance.Visible = true;
            fieldat.Visible = true;
            lblerrmsg.Visible = false;
            fpattendanceentry.Sheets[0].RowCount = 0;
            string hour = string.Empty;
            int atar = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveRow.ToString());
            int atac = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveColumn.ToString());
            if (atar > -1 && atar > -1)
            {
                lblcurdate.Text = FpSpread1.Sheets[0].RowHeader.Cells[atar, 0].Text.ToString();
                for (int att_col = 7; att_col <= FpSpread2.Sheets[0].ColumnCount - 2; att_col = att_col + 2)
                {
                    if (hour == "")
                    {
                        hour = FpSpread2.Sheets[0].ColumnHeader.Cells[1, att_col].Tag.ToString();
                    }
                    else
                    {
                        hour = hour + " , " + FpSpread2.Sheets[0].ColumnHeader.Cells[1, att_col].Tag.ToString();
                    }
                }
                lblhrvalue.Text = hour.ToString();
                ddlattend.Items.Clear();
                ddlreststudent.Items.Clear();
                string odrights = da.GetFunction("select rights from OD_Master_Setting where " + grouporusercode + "");
                if (odrights.Trim() != null && odrights.Trim() != "" && odrights.Trim() != "0")
                {
                    string od_rights = string.Empty;
                    od_rights = odrights;
                    string[] split_od_rights = od_rights.Split(',');
                    ddlattend.Items.Add(" ");
                    ddlreststudent.Items.Add(" ");
                    for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                    {
                        string value = split_od_rights[od_temp];
                        ddlattend.Items.Add("" + value + " ");
                        ddlreststudent.Items.Add("" + value + " ");
                    }
                }
                else
                {
                    ddlreststudent.Items.Add(" ");
                    ddlreststudent.Items.Add("P");
                    ddlreststudent.Items.Add("A");
                    ddlreststudent.Items.Add("OD ");
                    ddlreststudent.Items.Add("SOD");
                    ddlreststudent.Items.Add("ML");
                    ddlreststudent.Items.Add("NSS");
                    ddlreststudent.Items.Add("L");
                    ddlreststudent.Items.Add("NCC");
                    ddlreststudent.Items.Add("HS");
                    ddlreststudent.Items.Add("PP");
                    ddlreststudent.Items.Add("SYOD");
                    ddlreststudent.Items.Add("COD");
                    ddlreststudent.Items.Add("OOD");
                    ddlreststudent.Items.Add("LA");

                    ddlattend.Items.Add(" ");
                    ddlattend.Items.Add("P");
                    ddlattend.Items.Add("A");
                    ddlattend.Items.Add("OD ");
                    ddlattend.Items.Add("SOD");
                    ddlattend.Items.Add("ML");
                    ddlattend.Items.Add("NSS");
                    ddlattend.Items.Add("L");
                    ddlattend.Items.Add("NCC");
                    ddlattend.Items.Add("HS");
                    ddlattend.Items.Add("PP");
                    ddlattend.Items.Add("SYOD");
                    ddlattend.Items.Add("COD");
                    ddlattend.Items.Add("OOD");
                    ddlattend.Items.Add("LA");
                }
                fpattendanceentry.Sheets[0].ColumnHeader.RowCount = 1;
                fpattendanceentry.Sheets[0].ColumnCount = 2;
                FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();
                fpattendanceentry.Sheets[0].Columns[0].CellType = txtcell;
                fpattendanceentry.Sheets[0].Columns[1].CellType = txtcell;
                fpattendanceentry.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Roll Prefix";
                fpattendanceentry.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No of the Student";
                fpattendanceentry.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                fpattendanceentry.Sheets[0].Columns[0].Width = 150;
                fpattendanceentry.Sheets[0].Columns[1].Width = 451;
                fpattendanceentry.Sheets[0].ColumnHeader.Rows[0].Height = 45;
                fpattendanceentry.Height = 250;
                fpattendanceentry.Enabled = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void fpattendanceentry_OnUpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
    }

    protected void btnaddattendance_Click(object sender, EventArgs e)
    {
        try
        {
            rbgraphics.Checked = false;
            rbappenses.Checked = true;
            lblerrmsg.Visible = false;
            fpattendanceentry.SaveChanges();
            string studroll = string.Empty;
            string rollprefix = string.Empty;
            string setattandance = ddlattend.SelectedItem.ToString().Trim();
            string setrestattendance = ddlreststudent.SelectedItem.ToString().Trim();
            Hashtable hatrestroll = new Hashtable();
            Hashtable hatinvalidroll = new Hashtable();
            Boolean entryfalag = false;
            hatinvalidroll.Clear();
            hatrestroll.Clear();
            fpattendanceentry.SaveChanges();
            //string strinvalidroll =string.Empty;
            if (setattandance.Trim() != "" && setattandance.Trim() != null && setattandance.Trim() != "-1")
            {
                for (int j = 0; j < FpSpread2.Sheets[0].RowCount; j++)
                {
                    studroll = FpSpread2.Sheets[0].Cells[j, 1].Text.ToString().ToLower().Trim();
                    if (!hatroll.Contains(studroll.Trim().ToLower()))
                    {
                        hatroll.Add(studroll.Trim().ToLower(), j);
                    }
                }
                if (fpattendanceentry.Sheets[0].RowCount > 0)
                {
                    for (int i = 0; i < fpattendanceentry.Sheets[0].RowCount; i++)
                    {
                        rollprefix = fpattendanceentry.Sheets[0].Cells[i, 0].Text.ToString().Trim().ToLower();
                        string prefixrollno = fpattendanceentry.Sheets[0].Cells[i, 1].Text.ToString().Trim().ToLower();
                        if (rollprefix.Trim() != null && rollprefix.Trim() != "" && prefixrollno != null && prefixrollno.Trim() != "")
                        {
                            string[] prerollno = prefixrollno.Split(',');
                            for (int j = 0; j <= prerollno.GetUpperBound(0); j++)
                            {
                                for (int col = 7; col < FpSpread2.Sheets[0].ColumnCount; col = col + 2)
                                {
                                    studroll = rollprefix + prerollno[j].ToString().Trim().ToLower();
                                    if (hatroll.Contains(studroll.Trim().ToLower()))
                                    {
                                        entryfalag = true;
                                        string rowvalue = GetCorrespondingKey(studroll.Trim().ToLower(), hatroll).ToString();
                                        if (rowvalue != "Entered" && FpSpread2.Sheets[0].Cells[Convert.ToInt32(rowvalue), col].Locked == false && FpSpread2.Sheets[0].Cells[Convert.ToInt32(rowvalue), col].Text != "S" && FpSpread2.Sheets[0].Cells[Convert.ToInt32(rowvalue), col].Text.ToLower().Trim() != "od")
                                        {
                                            int row = Convert.ToInt32(rowvalue);
                                            FpSpread2.Sheets[0].Cells[row, col].Text = setattandance;
                                            if (col == FpSpread2.Sheets[0].ColumnCount - 2)
                                            {
                                                hatroll[studroll] = "Entered";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!hatinvalidroll.Contains(studroll.ToLower()))
                                        {
                                            hatinvalidroll.Add(studroll, studroll);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (setrestattendance.Trim() != "" && setrestattendance.Trim() != null && setrestattendance.Trim() != "-1")
                    {
                        for (int j = 0; j < FpSpread2.Sheets[0].RowCount; j++)
                        {
                            studroll = FpSpread2.Sheets[0].Cells[j, 1].Text.ToString().ToLower().Trim();
                            if (hatroll.Contains(studroll.Trim().ToLower()))
                            {
                                for (int col = 7; col < FpSpread2.Sheets[0].ColumnCount; col = col + 2)
                                {
                                    string restroll = GetCorrespondingKey(studroll.Trim().ToLower(), hatroll).ToString();
                                    if (restroll != "Entered" && FpSpread2.Sheets[0].Cells[Convert.ToInt32(restroll), col].Text != "S" && FpSpread2.Sheets[0].Cells[Convert.ToInt32(restroll), col].Locked == false && FpSpread2.Sheets[0].Cells[Convert.ToInt32(restroll), col].Text.ToLower().Trim() != "od")
                                    {
                                        FpSpread2.Sheets[0].Cells[Convert.ToInt32(restroll), col].Text = setrestattendance;
                                    }
                                }
                            }
                        }
                    }
                    if (hatinvalidroll.Count > 0)
                    {
                        foreach (DictionaryEntry parameter1 in hatinvalidroll)
                        {
                            if (strinvalidroll == "")
                            {
                                strinvalidroll = (parameter1.Key).ToString();
                            }
                            else
                            {
                                strinvalidroll = strinvalidroll + " , " + (parameter1.Key).ToString();
                            }
                        }
                    }
                    if (entryfalag == true)
                    {
                        //if (strinvalidroll != "")
                        //{
                        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Following Roll Nos Are Invalid:" + strinvalidroll + "')", true);
                        //}
                        Buttonsave_Click(sender, e);
                        appentiesentry();
                    }
                    else
                    {
                        lblerrmsg.Visible = true;
                        lblerrmsg.Text = "No Student Match";
                    }
                }
                else
                {
                    lblerrmsg.Visible = true;
                    lblerrmsg.Text = "Please Add Row";
                }
            }
            else
            {
                lblerrmsg.Visible = true;
                lblerrmsg.Text = "Please Enter Selected Students Attendance";
            }
            FpSpread2.SaveChanges();
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnaddrow_Click(object sender, EventArgs e)
    {
        fpattendanceentry.Sheets[0].RowCount++;
    }

    protected void rbgraphics_checkchange(object sender, EventArgs e)
    {
        loadgraphics();
    }

    protected void rbappenses_checkchange(object sender, EventArgs e)
    {
        appentiesentry();
    }

    public void loadgraphics()
    {
        try
        {
            rbgraphics.Checked = true;
            rbappenses.Checked = false;
            FpSpread2.Visible = true;
            Buttonselectall.Visible = true;
            Buttondeselect.Visible = true;
            Buttonsave.Visible = true;
            Buttonupdate.Visible = true;
            int acr = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveRow.ToString());
            int acc = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveColumn.ToString());
            if (acr > -1 && acc > -1)
            {
                string degreeclass = FpSpread1.Sheets[0].Cells[acr, acc].Text.ToString();
                string[] splitclass = degreeclass.Split('*');
                if (splitclass.GetUpperBound(0) > 0)
                {
                    lblmanysubject.Visible = true;
                    ddlselectmanysub.Visible = true;
                }
            }
            lblatdate.Visible = false;
            lblcurdate.Visible = false;
            lblhour.Visible = false;
            lblhrvalue.Visible = false;
            lblattend.Visible = false;
            ddlattend.Visible = false;
            btnaddrow.Visible = false;
            fpattendanceentry.Visible = false;
            lblreststudent.Visible = false;
            ddlreststudent.Visible = false;
            lblerrmsg.Visible = false;
            btnaddattendance.Visible = false;
            fieldat.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    public void loadreason()
    {
        ddlreason.Items.Clear();
        string collegecode = Session["collegecode"].ToString();
        string query = "select TextCode,Textval from textvaltable where TextCriteria='Attrs' and college_code=" + collegecode + "";
        ds.Dispose(); ds.Reset();
        ds = da.select_method_wo_parameter(query, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlreason.DataSource = ds;
            ddlreason.DataTextField = "Textval";
            ddlreason.DataValueField = "TextCode";
            ddlreason.DataBind();
        }
    }

    public void btnremovereason_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlreason.Items.Count > 0)
            {
                string collegecode = Session["collegecode"].ToString();
                string reason = ddlreason.SelectedItem.ToString();
                if (reason.Trim().ToLower() != "all" && reason.Trim() != "")
                {
                    string strquery = "delete textvaltable where TextVal='" + reason + "' and TextCriteria='Attrs' and college_code='" + collegecode + "'";
                    int a = da.update_method_wo_parameter(strquery, "Text");
                    loadreason();
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void btnaddreason_Click(object sender, EventArgs e)
    {
        panel1.Visible = true;
    }

    public void btnreasonnew_Click(object sender, EventArgs e)
    {
        panel1.Visible = true;
        string collegecode = Session["collegecode"].ToString();
        string reason = txtreason.Text.ToString();
        if (reason.Trim() != "")
        {
            string strquery = "insert into textvaltable (TextVal,TextCriteria,college_code) values('" + reason + "','Attrs','" + collegecode + "')";
            int a = da.update_method_wo_parameter(strquery, "Text");
            txtreason.Text = string.Empty;
            loadreason();
        }
    }

    public void btnreasonexit_Click(object sender, EventArgs e)
    {
        panel1.Visible = false;
    }

    public void sendvoicecall(string rollno, string date, string hour, string batch, string degree)
    {
        try
        {
            string Gender = string.Empty;
            string Hour = hour;
            string hour_check = string.Empty;
            //UserEmailID =string.Empty;
            string roll = rollno;
            string batchyear = batch;
            string coursename = string.Empty;
            string collegename = string.Empty;
            string collaccronymn = string.Empty;
            string voicelanguage = string.Empty;
            string MsgText = string.Empty;
            string RecepientNo = string.Empty;
            int check = 0;
            string[] split = date.Split(new Char[] { '-' });
            string datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
            date = datefrom;
            if (Convert.ToInt16(hour) == 1)
            {
                Hour = hour + "st ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) == 2)
            {
                Hour = hour + "nd ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) == 3)
            {
                Hour = hour + "rd ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) > 3)
            {
                Hour = hour + "th ";
                hour_check = hour;
            }
            string collquery = "Select collname from collinfo where college_code=" + Session["collegecode"].ToString() + "";
            DataSet datacol = new DataSet();
            datacol.Clear();
            datacol = da.select_method_wo_parameter(collquery, "Text");
            if (datacol.Tables.Count > 0 && datacol.Tables[0].Rows.Count > 0)
            {
                collegename = datacol.Tables[0].Rows[0]["collname"].ToString();
            }
            string degreequery = "select distinct Course_Name,Dept_Name from Department dep, Degree deg, course c where dep.Dept_Code=deg.Dept_Code and c.Course_Id=deg.Course_Id and deg.college_code =" + Session["collegecode"].ToString() + " and Degree_Code=" + degree + "";
            DataSet dscode = new DataSet();
            dscode = da.select_method_wo_parameter(degreequery, "Text");
            if (dscode.Tables.Count > 0 && dscode.Tables[0].Rows.Count > 0)
            {
                string course = dscode.Tables[0].Rows[0]["Course_Name"].ToString();
                string deptname = dscode.Tables[0].Rows[0]["Dept_Name"].ToString();
                coursename = course + "-" + deptname;
            }
            string str1 = string.Empty;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where  college_code='" + Session["collegecode"].ToString() + "'";
            }
            else
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where college_code='" + Session["collegecode"].ToString() + "'";
            }
            Boolean flage = false;
            DataSet ds1;
            ds1 = da.select_method_wo_parameter(str1, "txt");
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                for (int jj = 0; jj < ds1.Tables[0].Rows.Count; jj++)
                {
                    if (ds1.Tables[0].Rows[jj]["TextName"].ToString() == "Voice Call for Absent" && ds1.Tables[0].Rows[jj]["Taxtval"].ToString() == "1")
                    {
                        flage = true;
                    }
                }
                if (flage == true)
                {
                    for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                    {
                        if (ds1.Tables[0].Rows[k]["TextName"].ToString() == "Period" && ds1.Tables[0].Rows[k]["Taxtval"].ToString() != "")
                        {
                            string splihours = ds1.Tables[0].Rows[k]["Taxtval"].ToString();
                            string[] fin_split = splihours.Split(',');
                            int count = fin_split.Length;
                            for (int i = 0; i < count; i++)
                            {
                                string final_Hours = fin_split[i];
                                if (hour_check == final_Hours)
                                {
                                    check = check + 1;
                                }
                            }
                        }
                    }
                }
            }
            if (check > 0)
            {
                check = 0;
                string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName,VoiceLang from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                DataSet dsMobile;
                dsMobile = da.select_method_wo_parameter(Phone, "txt");
                string str = string.Empty;
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    str = "select distinct textname,taxtval from Attendance_Settings where  college_code='" + Session["collegecode"].ToString() + "'";
                }
                else
                {
                    str = "select distinct textname,taxtval from Attendance_Settings where  college_code='" + Session["collegecode"].ToString() + "'";
                }
                string voicelang = Convert.ToString(dsMobile.Tables[0].Rows[0]["VoiceLang"]);
                if (voicelang != "")
                {
                    string langquery = string.Empty;
                    langquery = "select TextVal from textvaltable where TextCode  ='" + voicelang + "' and TextCriteria='PLang' and college_code=" + Session["collegecode"] + "";
                    DataSet datalang = new DataSet();
                    datalang = da.select_method_wo_parameter(langquery, "Text");
                    if (datalang.Tables[0].Rows.Count > 0)
                    {
                        voicelanguage = datalang.Tables[0].Rows[0]["TextVal"].ToString();
                    }
                }
                // voicelanguage = "English";
                DataSet ds;
                ds = da.select_method_wo_parameter(str, "txt");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && dsMobile.Tables.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                {
                    //    if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                    //    {
                    //        Gender = "Your Son ";
                    //    }
                    //    else
                    //    {
                    //        Gender = "Your daughter";
                    //    }
                    //    string studentname = dsMobile.Tables[0].Rows[0]["stud_name"].ToString();
                    //    string[] splitname = studentname.Split('.');
                    //    string finalstudentname = splitname[0].ToString();
                    //    if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\dear parents.wav") == true)
                    //    {
                    //        if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\good morning.wav") == true)
                    //        {
                    //            if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\this call from.wav") == true)
                    //            {
                    //                if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\javagar school.wav") == true)
                    //                {
                    //                    if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\" + Gender + ".wav") == true)
                    //                    {
                    //                        if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\" + finalstudentname + ".wav") == true)
                    //                        {
                    //                            if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\" + coursename.ToString() + ".wav") == true)
                    //                            {
                    //                                if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\is absent today.wav") == true)
                    //                                {
                    //                                    if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\at 7th.wav") == true)
                    //                                    {
                    //                                        if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\thank you.wav") == true)
                    //                                        {
                    //                                            string[] files = new string[10] { "C:\\Documents and Settings\\Admin\\Desktop\\cutter\\dear parents.wav", "C:\\Documents and Settings\\Admin\\Desktop\\cutter\\good morning.wav", "C:\\Documents and Settings\\Admin\\Desktop\\cutter\\this call from.wav", "C:\\Documents and Settings\\Admin\\Desktop\\cutter\\javagar school.wav",
                    //                                          "C:\\Documents and Settings\\Admin\\Desktop\\cutter\\" + Gender + ".wav" ,"C:\\Documents and Settings\\Admin\\Desktop\\cutter\\" + studentname + ".wav","C:\\Documents and Settings\\Admin\\Desktop\\cutter\\" + coursename.ToString() + ".wav","C:\\Documents and Settings\\Admin\\Desktop\\cutter\\is absent today.wav","C:\\Documents and Settings\\Admin\\Desktop\\cutter\\at 7th.wav","C:\\Documents and Settings\\Admin\\Desktop\\cutter\\thank you.wav"};
                    //                                            // WaveIO wa = new WaveIO();
                    //                                            Concatenate(Server.MapPath("~/UploadFiles/chinnamaili.wav"), files);
                    //                                            filepath = Server.MapPath("~/UploadFiles/chinnamaili.wav");
                    //                                            insertmethod(filepath);
                    //                                        }
                    //                                    }
                    //                                }
                    //                            }
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //    FileInfo fileinfo = new FileInfo(filepath);
                    //    string filename = fileinfo.Name;
                    string gender = string.Empty;
                    if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                    {
                        gender = "MALE";
                    }
                    else
                    {
                        gender = "FEMALE";
                    }
                    string orginalname = string.Empty;
                    string student_name = Convert.ToString(dsMobile.Tables[0].Rows[0]["StudName"]);
                    if (student_name.Contains(".") == true)
                    {
                        string[] splitname = student_name.Split('.');
                        for (int i = 0; i <= splitname.GetUpperBound(0); i++)
                        {
                            string lengthname = splitname[i].ToString();
                            if (lengthname.Trim().Length > 2)
                            {
                                orginalname = splitname[i].ToString();
                            }
                        }
                    }
                    else
                    {
                        string[] split2ndname = student_name.Split(' ');
                        if (split2ndname.Length > 0)
                        {
                            for (int k = 0; k <= split2ndname.GetUpperBound(0); k++)
                            {
                                string firstname = split2ndname[k].ToString();
                                if (firstname.Trim().Length > 2)
                                {
                                    if (orginalname == "")
                                    {
                                        orginalname = firstname.ToString();
                                    }
                                    else
                                    {
                                        orginalname = orginalname + " " + firstname.ToString();
                                    }
                                }
                            }
                        }
                    }
                    DateTime dt = Convert.ToDateTime(date);
                    for (int jj1 = 0; jj1 < ds.Tables[0].Rows.Count; jj1++)
                    {
                        if (ds.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                        {
                            if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                            {
                                //  DateTime dt = Convert.ToDateTime(date);
                                MsgText = "ABSETN AT ";
                                RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                //Modified By Srinath
                                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                // string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + RecepientNo + "&message=" + MsgText + "&sender=" + SenderID;
                                biz.lbinfotech.www.Data h = new biz.lbinfotech.www.Data();
                                string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILYHOUR", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString() + "");
                                string isst = "0";
                                //smsreport(strpath, isst, dt);
                            }
                        }
                        if (ds.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                        {
                            if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                            {
                                // DateTime dt = Convert.ToDateTime(date);
                                MsgText = " ABSETN AT ";
                                RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                //Modified By SRinath /2/2014
                                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                //  string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + RecepientNo + "&message=" + MsgText + "&sender=" + SenderID;
                                biz.lbinfotech.www.Data h = new biz.lbinfotech.www.Data();
                                string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILYHOUR", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString() + "");
                                string isst = "0";
                                //smsreport(strpath, isst, dt);
                            }
                        }
                        if (ds.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                        {
                            if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                            {
                                MsgText = " ABSENT AT";
                                RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                //Modified By Srinatrh 8/2/2014
                                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                //string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + RecepientNo + "&message=" + MsgText + "&sender=" + SenderID;
                                biz.lbinfotech.www.Data h = new biz.lbinfotech.www.Data();
                                string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILYHOUR", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString() + "");
                                string isst = "0";
                                //smsreport(strpath, isst, dt);
                            }
                        }
                    }
                    //}
                    //}
                }
            }
        }
        catch
        {
        }
    }

    public void btnclosenotes_Click(object sender, EventArgs e)
    {
        pnotesuploadadd.Visible = false;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            pnotesuploadadd.Visible = true;
        }
        catch
        {
        }
    }

    public void loadalternode()
    {
        tvalterlession.Nodes.Clear();
        int hr = 0;
        string batch_year = string.Empty;
        string secval = string.Empty;
        string sch_dt = string.Empty;
        string dateval = string.Empty;
        int actrow1 = FpSpread1.ActiveSheetView.ActiveRow;
        int actcol1 = FpSpread1.ActiveSheetView.ActiveColumn;
        sch_dt = FpSpread1.Sheets[0].RowHeader.Cells[actrow1, 0].Text;
        string[] spdatesp = FpSpread1.Sheets[0].RowHeader.Cells[actrow1, 0].Text.Split(' '); ;
        sch_dt = spdatesp[0].ToString();
        if (sch_dt != "")
        {
            string[] sp_date = sch_dt.Split(new Char[] { '-' });
            dateval = sp_date[2].ToString() + "-" + sp_date[1].ToString() + "-" + sp_date[0].ToString();
        }
        Plessionalter.Visible = true;
        if (ddlselectmanysub.Items.Count > 0)
        {
            string getddlsub = ddlselectmanysub.SelectedValue.ToString();
            if (getddlsub.Trim() != "")
            {
                string[] sp1 = getddlsub.Split(new Char[] { '-' });
                if (sp1.GetUpperBound(0) >= 7)
                {
                    degree_code = sp1[0];
                    semester = sp1[1];
                    subject_no = sp1[2];
                    sections = sp1[3];
                    batch_year = sp1[4];
                    secval = " and Sections='" + sections + "'";
                }
                else
                {
                    degree_code = sp1[0];
                    semester = sp1[1];
                    subject_no = sp1[2];
                    sections = string.Empty;
                    batch_year = sp1[4];
                }
            }
        }
        else
        {
            if (getcelltag != "")
            {
                string[] sp1 = getcelltag.Split(new Char[] { '-' });
                if (sp1.GetUpperBound(0) >= 7)
                {
                    degree_code = sp1[0];
                    semester = sp1[1];
                    subject_no = sp1[2];
                    sections = sp1[3];
                    batch_year = sp1[4];
                    secval = " and Sections='" + sections + "'";
                }
                else
                {
                    degree_code = sp1[0];
                    semester = sp1[1];
                    subject_no = sp1[2];
                    sections = string.Empty;
                    batch_year = sp1[4];
                }
            }
        }
        string topics_Entry = string.Empty;
        string strquerylession = "select * from lesson_plan p,lessonplantopics l where l.lp_code=p.lp_code and p.degree_code='" + degree_code + "' and p.Batch_Year='" + batch_year + "' and p.semester='" + semester + "' and sch_date<='" + dateval + "' and subject_no='" + subject_no + "'  order by sch_date,hr";
        DataSet dsalter = da.select_method_wo_parameter(strquerylession, "Text");
        if (dsalter.Tables.Count > 0 && dsalter.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dsalter.Tables[0].Rows.Count; i++)
            {
                string strlession = dsalter.Tables[0].Rows[i]["topics"].ToString();
                string[] spless = strlession.Split('/');
                for (int s = 0; s <= spless.GetUpperBound(0); s++)
                {
                    string lessionval = spless[s].ToString();
                    if (lessionval.Trim() != "" && lessionval != null)
                    {
                        if (topics_Entry == "")
                        {
                            topics_Entry = lessionval;
                        }
                        else
                        {
                            topics_Entry = topics_Entry + "," + lessionval;
                        }
                    }
                }
            }
        }
        HierarchyTrees hierarchyTrees1 = new HierarchyTrees();
        HierarchyTrees.HTree objHTree1 = null;
        hierarchyTrees1.Clear();
        tvalterlession.Nodes.Clear();
        string sqlstr = "select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + subject_no + "' ";
        sqlstr = sqlstr + " and (topic_no  in(select parent_code from sub_unit_details where topic_no in(" + topics_Entry + "))";
        sqlstr = sqlstr + " or topic_no  in( select parent_code from sub_unit_details where topic_no in(select parent_code from sub_unit_details where topic_no in(" + topics_Entry + ")))";
        sqlstr = sqlstr + " or topic_no in(" + topics_Entry + ")) order by parent_code,topic_no";
        DataSet dstopic = da.select_method(sqlstr, hat, "Text");
        if (dstopic.Tables.Count > 0 && dstopic.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dstopic.Tables[0].Rows.Count; i++)
            {
                objHTree1 = new HierarchyTrees.HTree();
                objHTree1.topic_no = int.Parse(dstopic.Tables[0].Rows[i]["Topic_no"].ToString());
                objHTree1.parent_code = int.Parse(dstopic.Tables[0].Rows[i]["parent_code"].ToString());
                objHTree1.unit_name = dstopic.Tables[0].Rows[i]["unit_name"].ToString();
                hierarchyTrees1.Add(objHTree1);
            }
        }
        foreach (HierarchyTrees.HTree hTree in hierarchyTrees1)
        {
            HierarchyTrees.HTree parentNode = hierarchyTrees1.Find(delegate(HierarchyTrees.HTree emp) { return emp.topic_no == hTree.parent_code; });
            if (parentNode != null)
            {
                foreach (TreeNode tn in tvalterlession.Nodes)
                {
                    if (tn.Value == parentNode.topic_no.ToString())
                    {
                        tn.ChildNodes.Add(new TreeNode(hTree.unit_name.ToString(), hTree.topic_no.ToString()));
                    }
                    if (tn.ChildNodes.Count > 0)
                    {
                        foreach (TreeNode ctn in tn.ChildNodes)
                        {
                            RecursiveChild(ctn, parentNode.topic_no.ToString(), hTree);
                        }
                    }
                }
            }
            else
            {
                tvalterlession.Nodes.Add(new TreeNode(hTree.unit_name, hTree.topic_no.ToString()));
            }
            tvalterlession.ExpandAll();
        }
    }

    protected void chkalterlession_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkalterlession.Checked == true)
            {
                Panelyet.Width = 312;
                Panelcomplete.Width = 312;
                Plessionalter.Width = 312;
                loadalternode();
                Plessionalter.Visible = true;
            }
            else
            {
                Panelyet.Width = 460;
                Panelcomplete.Width = 460;
                Plessionalter.Visible = false;
            }
        }
        catch
        {
        }
    }

    public void savelessionalter()
    {
        try
        {
            string topics = string.Empty;
            string sch_order = string.Empty;
            foreach (TreeNode node in tvalterlession.CheckedNodes)
            {
                if (topics == "")
                {
                    topics = topics + node.Value;
                    selectedpath = selectedpath + node.ValuePath;
                }
                else
                {
                    topics = topics + "/" + node.Value;
                    selectedpath = selectedpath + "=" + node.ValuePath;
                }
            }
            if (topics == "")
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('There is nothing to save')", true);
                return;
            }
            string order_day = string.Empty;
            string query = "select schorder from PeriodAttndSchedule where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString();
            string order = da.GetFunction(query);
            if (order == "")
                return;
            string curday = Session["sch_date"].ToString();
            DateTime day_lesson = Convert.ToDateTime(curday);
            if (order != "0")
                order_day = day_lesson.ToString("ddd");
            else
            {
                order_day = find_day_order();
                if (order_day == "")
                    return;
            }
            if (order_day == "mon")
                sch_order = "1";
            else if (order_day == "tue")
                sch_order = "2";
            else if (order_day == "wed")
                sch_order = "3";
            else if (order_day == "thu")
                sch_order = "4";
            else if (order_day == "fri")
                sch_order = "5";
            else if (order_day == "sat")
                sch_order = "6";
            else if (order_day == "sun")
                sch_order = "7";
            if (order_day == "Mon")
                sch_order = "1";
            else if (order_day == "Tue")
                sch_order = "2";
            else if (order_day == "Wed")
                sch_order = "3";
            else if (order_day == "Thu")
                sch_order = "4";
            else if (order_day == "Fri")
                sch_order = "5";
            else if (order_day == "Sat")
                sch_order = "6";
            else if (order_day == "Sun")
                sch_order = "7";
            string subj_no = (string)Session["sub_no"].ToString();
            string hour_hr = (string)Session["hr"].ToString();
            int a = 0;
            string strdailtquery = "select * from dailyStaffEntry where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + Session["str_section"].ToString() + "  and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + Session["sch_date"].ToString() + "'";
            DataSet dsdaily = da.select_method(strdailtquery, hat, "Text");
            if (dsdaily.Tables.Count > 0 && dsdaily.Tables[0].Rows.Count > 0)
            {
                strdailtquery = "update dailyStaffEntry set sch_order=" + sch_order + " where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + Session["str_section"].ToString() + "  and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + Session["sch_date"].ToString() + "'";
                a = da.update_method_wo_parameter(strdailtquery, "Text");
            }
            else
            {
                string sec = (string)Session["sections"].ToString();
                if (sec != "")
                    strdailtquery = "insert into dailyStaffEntry (degree_code,semester,batch_year,sch_date,sch_order,sections) values(" + Session["deg_code"].ToString() + "," + Session["semester"].ToString() + "," + Session["batch_year"].ToString() + ",'" + Session["sch_date"].ToString() + "'," + sch_order + ",'" + Session["sections"].ToString() + "')";
                else
                    strdailtquery = "insert into dailyStaffEntry (degree_code,semester,batch_year,sch_date,sch_order) values(" + Session["deg_code"].ToString() + "," + Session["semester"].ToString() + "," + Session["batch_year"].ToString() + ",'" + Session["sch_date"].ToString() + "'," + sch_order + ")";
                a = da.update_method_wo_parameter(strdailtquery, "Text");
            }
            string lp_query = "select lp_code from dailyStaffEntry where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + Session["str_section"].ToString() + "  and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + Session["sch_date"].ToString() + "' and sch_order=" + sch_order;
            string lp_code = da.GetFunction(lp_query);
            if (lp_code != "")
            {
                strdailtquery = "select * from dailyEntdet where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "";
                dsdaily.Dispose();
                dsdaily.Reset();
                dsdaily = da.select_method(strdailtquery, hat, "Text");
                if (dsdaily.Tables.Count > 0 && dsdaily.Tables[0].Rows.Count > 0)
                {
                    string strgettopic = da.GetFunction("select topics from dailyEntdet where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "");
                    if (strgettopic != "" && strgettopic != null && strgettopic != "0")
                    {
                        topics = topics + "/" + strgettopic;
                    }
                    //cmd.CommandText = "update dailyEntdet set topics='" + topics + "' where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "";                  
                    strdailtquery = "update dailyEntdet set topics='" + topics + "' where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "";
                    a = da.insert_method(strdailtquery, hat, "Text");
                }
                else
                {
                    //cmd.CommandText = "insert into dailyEntdet (lp_code,subject_no,topics,hr,staff_code) values(" + lp_code + "," + subj_no + ",'" + topics + "'," + hour_hr + ",'" + staff_code + "')";
                    strdailtquery = "insert into dailyEntdet (lp_code,subject_no,topics,hr,staff_code) values(" + lp_code + "," + subj_no + ",'" + topics + "'," + hour_hr + ",'" + staff_code + "')";
                    a = da.update_method_wo_parameter(strdailtquery, "Text");
                }
            }
            ar = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveRow.ToString());
            ac = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveColumn.ToString());
            if (ar != -1)
            {
                string spread_text = FpSpread1.Sheets[0].Cells[ar, ac].Text;
                //  getcelltag = FpSpread1.Sheets[0].GetTag(ar, ac).ToString();
                getcelltag = ddlselectmanysub.SelectedValue.ToString();
                string getdate = FpSpread1.Sheets[0].Cells[ar, 0].Text;
                string[] splitvalue = getcelltag.Split(new char[] { '-' });
                if (splitvalue.GetUpperBound(0) > 0)
                {
                    string degree_code = splitvalue[0].ToString();
                    string semester = splitvalue[1].ToString();
                    string subject_no = splitvalue[2].ToString();
                    string batch_year = string.Empty;
                    if (splitvalue.GetUpperBound(0) == 7)
                    {
                        batch_year = splitvalue[4].ToString();
                    }
                    else
                    {
                        batch_year = splitvalue[3].ToString();
                    }
                }
                filltree();
                loadalternode();
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
            }
        }
        catch
        {
        }
    }

    public void load_attendance()
    {
        bool splhr_flag = false;
        string[] split_holiday_status = new string[1000];
        string sections = string.Empty;
        Hashtable hatonduty = new Hashtable();
        DataSet dsonduty = new DataSet();
        Hashtable hatodtot = new Hashtable();
        DataSet dsmark = new DataSet();
        DataView dvmark = new DataView();
        Hashtable has = new Hashtable();
        DateTime temp_date, dt2;
        Hashtable hat_holy = new Hashtable();
        Hashtable temp_has_subj_code = new Hashtable();
        int split_holiday_status_1 = 0, split_holiday_status_2 = 0;
        bool holiflag = false;
        try
        {
            has.Clear();
            has.Add("colege_code", Session["collegecode"].ToString());
            ds_attndmaster = da.select_method("ATT_MASTER_SETTING", has, "sp");
            if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
            {
                count_master = (ds_attndmaster.Tables[0].Rows.Count);
            }
            if (count_master > 0)
            {
                for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
                {
                    if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
                    {
                        if (!has_attnd_masterset.ContainsKey(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString()))
                        {
                            has_attnd_masterset.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString());
                        }
                    }
                    if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "2")
                    {
                        if (!has_attnd_masterset_notconsider.ContainsKey(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString()))
                        {
                            has_attnd_masterset_notconsider.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString());
                        }
                    }
                    if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                    {
                        if (!hatabsentvalues.ContainsKey(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString()))
                        {
                            hatabsentvalues.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString());
                        }
                    }
                }
            }
            string get_alter_or_sem = string.Empty;
            string[] split_tag_val = getcelltag.Split('*');
            for (int tag_for = 0; tag_for <= split_tag_val.GetUpperBound(0); tag_for++)
            {
                str = split_tag_val[tag_for].ToString();
                string tempdegree = split_tag_val[tag_for].ToString();
                if (str != "")
                {
                    string[] sp1 = str.Split(new Char[] { '-' });
                    if (sp1[0].ToString().Trim() != "Selected day is Holiday" && sp1[0].ToString().Trim() != "")
                    {
                        string byear = string.Empty;
                        degree_code = sp1[0];
                        semester = sp1[1];
                        string subject_no = sp1[2].Trim();
                        string batch_year = sp1[4].ToString();
                        string subj_type = string.Empty;
                        if (sp1.GetUpperBound(0) == 7)
                        {
                            sections = sp1[3];
                            byear = sp1[4];
                            subj_type = sp1[5];
                            subj_count_in_onehr = sp1[6];
                            get_alter_or_sem = sp1[7];
                        }
                        else
                        {
                            sections = string.Empty;
                            byear = sp1[3];
                            subj_type = sp1[4];
                            subj_count_in_onehr = sp1[5];
                            get_alter_or_sem = sp1[6];
                        }
                        count_master = 0;
                        string splhrsec = string.Empty;
                        string rstrsec = string.Empty;
                        if (sections.Trim() == "" || sections.Trim() == "-1")
                        {
                            strsec = string.Empty;
                            rstrsec = string.Empty;
                            splhrsec = string.Empty;
                        }
                        else
                        {
                            strsec = " and sections='" + sections + "'";
                            rstrsec = " and r.sections='" + sections + "'";
                            splhrsec = "and sections='" + sections + "'";
                        }
                        DataSet ds_student = da.select_method_wo_parameter(" select distinct r.roll_no as 'ROLL NO', r.stud_name as 'STUD NAME', r.reg_no as 'REG NO',r.Adm_Date,p.No_of_hrs_per_day as 'PER DAY',schorder as 'order',start_date,no_of_hrs_I_half_day,no_of_hrs_II_half_day,len(r.roll_no),convert(varchar(15),adm_date,103) as adm_date,r.serialno FROM registration r, Department d ,PeriodAttndSchedule p  ,seminfo s WHERE r.degree_code=p.degree_code and r.Batch_Year=" + byear + "  and  s.batch_Year=" + byear + "  and r.degree_code= " + degree_code + " and s.degree_code= " + degree_code + " and  s.semester=" + semester + " and p.semester=" + semester + "  and (r.CC = 0)  AND (r.DelFlag = 0)  AND (r.Exam_Flag <> 'debar') AND (r.Current_Semester IS NOT NULL)  " + strsec + " ", "Text");
                        int stud_count = ds_student.Tables[0].Rows.Count;
                        int no_of_hrs = int.Parse(ds_student.Tables[0].Rows[0]["PER DAY"].ToString());
                        int mng_hrs = int.Parse(ds_student.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString());
                        int evng_hrs = int.Parse(ds_student.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString());
                        string order = ds_student.Tables[0].Rows[0]["order"].ToString();
                        string sem_start_date = ds_student.Tables[0].Rows[0]["start_date"].ToString();
                        string temp_date1 = FpSpread1.Sheets[0].RowHeader.Cells[ar, 0].Text;
                        string[] spitdate = temp_date1.Split(' ');
                        string[] date_split = spitdate[0].Split('-');
                        getdate = date_split[0] + "-" + date_split[1] + "-" + date_split[2];
                        string datefrom = sem_start_date;
                        DateTime dt1 = Convert.ToDateTime(sem_start_date);
                        string date2 = getdate;
                        string[] split1 = date2.Split(new Char[] { '-' });
                        string dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                        dt2 = Convert.ToDateTime(dateto.ToString());
                        DateTime spfromdate = dt1;
                        DateTime sptodate = dt2;
                        string dummy_date = string.Empty;
                        string month_year = string.Empty;
                        string strDay = string.Empty;
                        string full_hour = string.Empty;
                        string single_hour = string.Empty;
                        string temp_hr_field = string.Empty;
                        string date_temp_field = string.Empty;
                        ht_sphr.Clear();
                        string hrdetno = string.Empty;
                        string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + degree_code + " and batch_year=" + byear + " and semester=" + semester + " " + splhrsec + " and date between '" + spfromdate.ToString() + "' and '" + sptodate.ToString() + "'";
                        DataSet ds_sphr = da.select_method(getsphr, hat, "Text");
                        if (ds_sphr.Tables.Count > 0 && ds_sphr.Tables[0].Rows.Count > 0)
                        {
                            for (int sphr = 0; sphr < ds_sphr.Tables[0].Rows.Count; sphr++)
                            {
                                if (ht_sphr.Contains(Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"])))
                                {
                                    hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"]), ht_sphr));
                                    hrdetno = hrdetno + "," + Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["hrdet_no"]);
                                    ht_sphr[Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"])] = hrdetno;
                                }
                                else
                                {
                                    ht_sphr.Add(Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"]), Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["hrdet_no"]));
                                }
                            }
                        }
                        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                        {
                            grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                        }
                        else
                        {
                            grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                        }
                        string spl_hr_rights = da.GetFunction("select rights from  special_hr_rights where " + grouporusercode + "");
                        if (spl_hr_rights.Trim().ToLower() == "true" || spl_hr_rights.Trim().ToLower() == "1")
                        {
                            splhr_flag = true;
                        }
                        int present_count = 0;
                        temp_date = dt1;
                        string stralldetaisquery = "select distinct r.roll_no,s.subject_no,s.batch,r.adm_date from registration r,subjectchooser s where s.roll_no=r.roll_no and r.batch_year='" + byear + "' and r.degree_code='" + degree_code + "' and s.subject_no='" + subject_no + "' " + rstrsec + "";
                        stralldetaisquery = stralldetaisquery + " ;select distinct r.roll_no,s.subject_no,s.batch,r.adm_date from registration r,subjectchooser_new s where s.roll_no=r.roll_no and r.batch_year='" + byear + "' and r.degree_code='" + degree_code + "' and s.subject_no='" + subject_no + "' " + rstrsec + "";
                        stralldetaisquery = stralldetaisquery + " ;select distinct day_value,hour_value,stu_batch,subject_no,timetablename from laballoc where batch_year='" + byear + "' and degree_code='" + degree_code + "' and subject_no='" + subject_no + "' " + strsec + "";
                        stralldetaisquery = stralldetaisquery + " ;select distinct day_value,hour_value,stu_batch,subject_no,fdate from laballoc_new where batch_year='" + byear + "' and degree_code='" + degree_code + "' and subject_no='" + subject_no + "' " + strsec + "";
                        stralldetaisquery = stralldetaisquery + " ;select a.* from attendance a,registration r where a.roll_no=r.roll_no and r.batch_year='" + byear + "' and r.degree_code='" + degree_code + "' " + rstrsec + "";
                        stralldetaisquery = stralldetaisquery + " ;select a.* from attendance_withreason a,registration r where a.roll_no=r.roll_no and r.batch_year='" + byear + "' and r.degree_code='" + degree_code + "' " + rstrsec + "";
                        stralldetaisquery = stralldetaisquery + " ;select * from Semester_Schedule where batch_year='" + byear + "' and degree_code='" + degree_code + "' and semester='" + semester + "'  " + strsec + " order by FromDate desc";
                        stralldetaisquery = stralldetaisquery + " ;select * from Alternate_Schedule where batch_year='" + byear + "' and degree_code='" + degree_code + "' and semester='" + semester + "'  " + strsec + "  order by FromDate desc";
                        DataSet dsalldetails = da.select_method_wo_parameter(stralldetaisquery, "Text");

                        string getdeteails = "select convert(nvarchar(15),s.start_date,101) as start_date,nodays,s.starting_dayorder from seminfo s,PeriodAttndSchedule p where s.degree_code=p.degree_code and p.semester=s.semester and s.semester='" + semester + "' and s.batch_year='" + byear + "'  and s.degree_code='" + degree_code + "'";
                        getdeteails = getdeteails + " ; select * from tbl_consider_day_order where semester='" + semester + "' and batch_year='" + byear + "'  and degree_code='" + degree_code + "'";
                        getdeteails = getdeteails + " ; select subject_type,LAB From sub_sem where subtype_no=(select subtype_no from subject where  subject_no='" + subject_no + "')";
                        DataSet dssem = da.select_method_wo_parameter(getdeteails, "Text");
                        string semstartdate = string.Empty;
                        string noofdays = string.Empty;
                        string startday = string.Empty;
                        if (dssem.Tables.Count > 0 && dssem.Tables[0].Rows.Count > 0)
                        {
                            semstartdate = dssem.Tables[0].Rows[0]["start_date"].ToString();
                            noofdays = dssem.Tables[0].Rows[0]["nodays"].ToString();
                            startday = dssem.Tables[0].Rows[0]["starting_dayorder"].ToString();
                        }
                        Hashtable hatdc = new Hashtable();
                        try
                        {
                            if (dssem.Tables.Count > 1 && dssem.Tables[1].Rows.Count > 0)
                            {
                                for (int dc = 0; dc < dssem.Tables[1].Rows.Count; dc++)
                                {
                                    DateTime dtdcf = Convert.ToDateTime(dssem.Tables[1].Rows[dc]["from_date"].ToString());
                                    DateTime dtdct = Convert.ToDateTime(dssem.Tables[1].Rows[dc]["to_date"].ToString());
                                    for (DateTime dtc = dtdcf; dtc <= dtdct; dtc = dtc.AddDays(1))
                                    {
                                        if (!hatdc.Contains(dtc))
                                        {
                                            hatdc.Add(dtc, dtc);
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                        has.Clear();
                        has.Add("from_date", dt1);
                        has.Add("to_date", dt2);
                        has.Add("degree_code", degree_code);
                        has.Add("sem", semester);
                        has.Add("coll_code", Session["collegecode"].ToString());
                        int iscount = 0;
                        string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + dt1.ToString() + "' and '" + dt2.ToString() + "' and degree_code=" + degree_code + " and semester=" + semester + "";
                        DataSet dsholiday = da.select_method_wo_parameter(sqlstr_holiday, "Text");
                        if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count > 0)
                        {
                            iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
                        }
                        has.Add("iscount", iscount);
                        DataSet ds_holi = da.select_method("HOLIDATE_DETAILS_FINE", has, "sp");
                        string halforfull = "", mng = "", evng = "", holiday_sched_details = string.Empty;
                        if (ds_holi.Tables.Count > 0 && ds_holi.Tables[0].Rows.Count > 0)
                        {
                            for (int holi = 0; holi < ds_holi.Tables[0].Rows.Count; holi++)
                            {
                                if (ds_holi.Tables[0].Rows[holi]["halforfull"].ToString() == "False")
                                {
                                    halforfull = "0";
                                }
                                else
                                {
                                    halforfull = "1";
                                }
                                if (ds_holi.Tables[0].Rows[holi]["morning"].ToString() == "False")
                                {
                                    mng = "0";
                                }
                                else
                                {
                                    mng = "1";
                                }
                                if (ds_holi.Tables[0].Rows[holi]["evening"].ToString() == "False")
                                {
                                    evng = "0";
                                }
                                else
                                {
                                    evng = "1";
                                }
                                holiday_sched_details = halforfull + "*" + mng + "*" + evng;
                                if (!hat_holy.ContainsKey(Convert.ToDateTime(ds_holi.Tables[0].Rows[holi]["HOLI_DATE"].ToString())))
                                {
                                    hat_holy.Add(Convert.ToDateTime(ds_holi.Tables[0].Rows[holi]["HOLI_DATE"].ToString()), holiday_sched_details);
                                }
                            }
                        }
                        subj_type = dssem.Tables[2].Rows[0]["LAB"].ToString();
                        while (temp_date <= dt2)
                        {
                            Boolean check_alter = false;
                            if (!hatdc.Contains(temp_date))
                            {
                                if (splhr_flag == true)
                                {
                                    if (ht_sphr.Contains(Convert.ToString(temp_date)))
                                    {
                                        getspecial_hr(Convert.ToString(temp_date.ToString("MM/dd/yyyy")), subject_no.Trim(), dsalldetails);
                                    }
                                }
                                if (!hat_holy.ContainsKey(temp_date))
                                {
                                    if (!hat_holy.ContainsKey(temp_date))
                                    {
                                        hat_holy.Add(temp_date, "3*0*0");
                                    }
                                }
                                string value_holi_status = GetCorrespondingKey(temp_date, hat_holy).ToString();
                                split_holiday_status = value_holi_status.Split('*');
                                if (split_holiday_status[0].ToString() == "3")//=========ful day working day
                                {
                                    split_holiday_status_1 = 1;
                                    split_holiday_status_2 = no_of_hrs;
                                }
                                else if (split_holiday_status[0].ToString() == "1")//=============half day working day
                                {
                                    if (split_holiday_status[1].ToString() == "1")//==============mng holiday//evng working day
                                    {
                                        split_holiday_status_1 = mng_hrs + 1;
                                        split_holiday_status_2 = no_of_hrs;
                                    }
                                    if (split_holiday_status[2].ToString() == "1")//==============evng holiday//mng working day
                                    {
                                        split_holiday_status_1 = 1;
                                        split_holiday_status_2 = mng_hrs;
                                    }
                                }
                                else if (split_holiday_status[0].ToString() == "0")//=================fulday holiday
                                {
                                    split_holiday_status_1 = 0;
                                    split_holiday_status_2 = 0;
                                }
                                if (split_holiday_status_1 == 0 && split_holiday_status_2 == 0)
                                {
                                }
                                else
                                {
                                    holiflag = true;
                                    DataView dvaltersech = new DataView();
                                    DataView dvsemsech = new DataView();
                                    if (dsalldetails.Tables.Count > 7 && dsalldetails.Tables[7].Rows.Count > 0)
                                    {
                                        dsalldetails.Tables[7].DefaultView.RowFilter = "degree_code = " + degree_code + " and semester = " + semester + " and batch_year = " + byear + " and FromDate ='" + temp_date + "' " + strsec + "";
                                        dvaltersech = dsalldetails.Tables[7].DefaultView;
                                    }
                                    if (dsalldetails.Tables.Count > 6 && dsalldetails.Tables[6].Rows.Count > 0)
                                    {
                                        dsalldetails.Tables[6].DefaultView.RowFilter = "degree_code = " + degree_code + " and semester = " + semester + " and batch_year = " + byear + " and FromDate <='" + temp_date + "' " + strsec + "";
                                        dvsemsech = dsalldetails.Tables[6].DefaultView;
                                    }
                                    if (dvsemsech.Count > 0)
                                    {
                                        if (no_of_hrs > 0)
                                        {
                                            dummy_date = temp_date.ToString();
                                            string[] dummy_date_split = dummy_date.Split(' ');
                                            string[] final_date_string = dummy_date_split[0].Split('/');
                                            dummy_date = final_date_string[1].ToString() + "/" + final_date_string[0].ToString() + "/" + final_date_string[2].ToString();
                                            month_year = ((Convert.ToInt16(final_date_string[2].ToString()) * 12) + (Convert.ToInt16(final_date_string[0].ToString()))).ToString();
                                            if (order != "0")
                                            {
                                                strDay = temp_date.ToString("ddd");
                                            }
                                            else
                                            {
                                                string[] sp = dummy_date.Split('/');
                                                string curdate = sp[1] + '/' + sp[0] + '/' + sp[2];
                                                strDay = da.findday(curdate, degree_code, semester, byear, semstartdate, noofdays, startday);
                                            }

                                            for (int temp_hr = split_holiday_status_1; temp_hr <= split_holiday_status_2; temp_hr++)
                                            {
                                                check_alter = false;
                                                present_count = 0;
                                                temp_hr_field = strDay + temp_hr;
                                                date_temp_field = "d" + final_date_string[1].ToString() + "d" + temp_hr;
                                                if (dvaltersech.Count > 0)
                                                {
                                                    for (int hasrow = 0; hasrow < dvaltersech.Count; hasrow++)
                                                    {
                                                        full_hour = dvaltersech[hasrow][temp_hr_field].ToString();
                                                        if (full_hour.Trim() != "")
                                                        {
                                                            check_alter = true;
                                                            temp_has_subj_code.Clear();
                                                            string[] split_full_hour = full_hour.Split(';');
                                                            for (int semi_colon = 0; semi_colon <= split_full_hour.GetUpperBound(0); semi_colon++)
                                                            {
                                                                single_hour = split_full_hour[semi_colon].ToString();
                                                                string[] split_single_hour = single_hour.Split('-');
                                                                if (split_single_hour.GetUpperBound(0) >= 1)
                                                                {
                                                                    string subjectno = Convert.ToString(split_single_hour[0]).Trim();
                                                                    staff_code = Convert.ToString(Session["Staff_Code"]);
                                                                    if (subject_no.Trim() == subjectno.Trim())
                                                                    {
                                                                        if (!temp_has_subj_code.ContainsKey(subject_no.Trim()))
                                                                        {
                                                                            temp_has_subj_code.Add(subject_no.Trim(), subject_no.Trim());
                                                                            Hashtable has_stud_list = new Hashtable();
                                                                            subj_type = dssem.Tables[2].Rows[0]["LAB"].ToString();
                                                                            if (subj_type != "1" && subj_type.Trim().ToLower() != "true")
                                                                            {
                                                                                DataView dvlabhr = new DataView();
                                                                                if (dsalldetails.Tables.Count > 0 && dsalldetails.Tables[0].Rows.Count > 0)
                                                                                {
                                                                                    dsalldetails.Tables[0].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + subject_no + "'";
                                                                                    dvlabhr = dsalldetails.Tables[0].DefaultView;
                                                                                }
                                                                                for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                                                                                {
                                                                                    string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                                                                                    DataView dvattva = new DataView();
                                                                                    if (dsalldetails.Tables.Count > 4 && dsalldetails.Tables[4].Rows.Count > 0)
                                                                                    {
                                                                                        dsalldetails.Tables[4].DefaultView.RowFilter = " month_year='" + month_year.ToString() + "' and roll_no='" + rollno + "'";
                                                                                        dvattva = dsalldetails.Tables[4].DefaultView;
                                                                                    }
                                                                                    bool checkedFeeOfRoll = false;
                                                                                    if (dicFeeOfRollStudents.ContainsKey(rollno.Trim().ToLower()) && dicFeeOnRollStudents.ContainsKey(rollno.Trim().ToLower()))
                                                                                    {
                                                                                        DateTime[] dtFeeOfRoll = dicFeeOfRollStudents[rollno.Trim().ToLower()];
                                                                                        if (temp_date >= dtFeeOfRoll[0])
                                                                                        {
                                                                                            bool hasRollOff = false;
                                                                                            DateTime dtDefaultDate = new DateTime(1900, 1, 1);//SqlServer Default Date
                                                                                            if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 0 && temp_date < dtFeeOfRoll[1])
                                                                                            {
                                                                                                hasRollOff = true;
                                                                                                checkedFeeOfRoll = true;
                                                                                            }
                                                                                            else if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 1)
                                                                                            {
                                                                                                hasRollOff = true;
                                                                                                checkedFeeOfRoll = true;
                                                                                            }
                                                                                            else if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 0 && dtFeeOfRoll[1] == dtDefaultDate)
                                                                                            {
                                                                                                hasRollOff = true;
                                                                                                checkedFeeOfRoll = true;
                                                                                            }
                                                                                            //else if (dicFeeOnRollStudents[rollNo.Trim()] == 0 && dtSelDate >= dtFeeOfRoll[1])
                                                                                            //{
                                                                                            //    hasRollOff = false;
                                                                                            //    checkedFeeOfRoll = false;
                                                                                            //}
                                                                                            else
                                                                                            {
                                                                                                hasRollOff = false;
                                                                                                checkedFeeOfRoll = false;
                                                                                            }
                                                                                            if (hasRollOff)
                                                                                            {
                                                                                                //if (has_attnd_masterset.ContainsKey("2"))
                                                                                                //{
                                                                                                //    if (has_load_rollno.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                                //    {
                                                                                                //        present_count = Convert.ToInt16(has_load_rollno[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                                //        present_count++;
                                                                                                //        has_load_rollno[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                                //    }
                                                                                                //    else
                                                                                                //    {
                                                                                                //        has_load_rollno.Add(rollno + '-' + subjectno, 1);
                                                                                                //    }
                                                                                                //}
                                                                                                if (has_total_attnd_hour.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                                {
                                                                                                    present_count = Convert.ToInt16(has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                                    present_count++;
                                                                                                    has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    has_total_attnd_hour.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    if (dvattva.Count > 0)
                                                                                    {
                                                                                        string attval = Convert.ToString(dvattva[0][date_temp_field]).Trim();
                                                                                        if (!string.IsNullOrEmpty(attval) && attval.Trim() != "12" && attval.Trim() != "8" && attval.Trim() != "0")
                                                                                        {
                                                                                            if (has_attnd_masterset.ContainsKey(attval.Trim()))
                                                                                            {
                                                                                                if (has_load_rollno.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                                {
                                                                                                    present_count = Convert.ToInt16(has_load_rollno[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                                    present_count++;
                                                                                                    has_load_rollno[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    has_load_rollno.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
                                                                                                }
                                                                                            }
                                                                                            if (has_total_attnd_hour.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                            {
                                                                                                present_count = Convert.ToInt16(has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                                present_count++;
                                                                                                has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                has_total_attnd_hour.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                dsalldetails.Tables[3].DefaultView.RowFilter = "hour_value=" + temp_hr + "  and day_value='" + strDay + "' and subject_no='" + subject_no + "' and fdate='" + temp_date.ToString("MM/dd/yyyy").ToString() + "'";
                                                                                DataView dvlabbatch = dsalldetails.Tables[3].DefaultView;
                                                                                for (int lb = 0; lb < dvlabbatch.Count; lb++)
                                                                                {
                                                                                    string batch = dvlabbatch[lb]["stu_batch"].ToString();
                                                                                    if (batch != null && batch.Trim() != "")
                                                                                    {
                                                                                        DataView dvlabhr = new DataView();
                                                                                        if (dsalldetails.Tables.Count > 1 && dsalldetails.Tables[1].Rows.Count > 0)
                                                                                        {
                                                                                            dsalldetails.Tables[1].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + subject_no + "' and batch='" + batch + "' ";
                                                                                            dvlabhr = dsalldetails.Tables[1].DefaultView;
                                                                                        }
                                                                                        for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                                                                                        {
                                                                                            string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                                                                                            DataView dvattva = new DataView();
                                                                                            if (dsalldetails.Tables.Count > 4 && dsalldetails.Tables[4].Rows.Count > 0)
                                                                                            {
                                                                                                dsalldetails.Tables[4].DefaultView.RowFilter = " month_year='" + month_year.ToString() + "' and roll_no='" + rollno + "'";
                                                                                                dvattva = dsalldetails.Tables[4].DefaultView;
                                                                                            }
                                                                                            bool checkedFeeOfRoll = false;
                                                                                            if (dicFeeOfRollStudents.ContainsKey(rollno.Trim().ToLower()) && dicFeeOnRollStudents.ContainsKey(rollno.Trim().ToLower()))
                                                                                            {
                                                                                                DateTime[] dtFeeOfRoll = dicFeeOfRollStudents[rollno.Trim().ToLower()];
                                                                                                if (temp_date >= dtFeeOfRoll[0])
                                                                                                {
                                                                                                    bool hasRollOff = false;
                                                                                                    DateTime dtDefaultDate = new DateTime(1900, 1, 1);//SqlServer Default Date
                                                                                                    if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 0 && temp_date < dtFeeOfRoll[1])
                                                                                                    {
                                                                                                        hasRollOff = true;
                                                                                                        checkedFeeOfRoll = true;
                                                                                                    }
                                                                                                    else if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 1)
                                                                                                    {
                                                                                                        hasRollOff = true;
                                                                                                        checkedFeeOfRoll = true;
                                                                                                    }
                                                                                                    else if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 0 && dtFeeOfRoll[1] == dtDefaultDate)
                                                                                                    {
                                                                                                        hasRollOff = true;
                                                                                                        checkedFeeOfRoll = true;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        hasRollOff = false;
                                                                                                        checkedFeeOfRoll = false;
                                                                                                    }
                                                                                                    if (hasRollOff)
                                                                                                    {
                                                                                                        //if (has_attnd_masterset.ContainsKey("2"))
                                                                                                        //{
                                                                                                        //    if (has_load_rollno.Contains(rollno + '-' + subjectno))
                                                                                                        //    {
                                                                                                        //        present_count = Convert.ToInt16(has_load_rollno[rollno + '-' + subjectno]);
                                                                                                        //        present_count++;
                                                                                                        //        has_load_rollno[rollno + '-' + subjectno] = present_count;
                                                                                                        //    }
                                                                                                        //    else
                                                                                                        //    {
                                                                                                        //        has_load_rollno.Add(rollno + '-' + subjectno, 1);
                                                                                                        //    }
                                                                                                        //}
                                                                                                        if (has_total_attnd_hour.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                                        {
                                                                                                            present_count = Convert.ToInt16(has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                                            present_count++;
                                                                                                            has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            has_total_attnd_hour.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            if (dvattva.Count > 0 && !checkedFeeOfRoll)
                                                                                            {
                                                                                                string attval = dvattva[0][date_temp_field].ToString();
                                                                                                //if (attval != "12" && attval != "8" && attval.Trim() != "")
                                                                                                if (!string.IsNullOrEmpty(attval) && attval.Trim() != "12" && attval.Trim() != "8" && attval.Trim() != "0")
                                                                                                {
                                                                                                    if (has_attnd_masterset.ContainsKey(attval))
                                                                                                    {
                                                                                                        if (has_load_rollno.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                                        {
                                                                                                            present_count = Convert.ToInt16(has_load_rollno[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                                            present_count++;
                                                                                                            has_load_rollno[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            has_load_rollno.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
                                                                                                        }
                                                                                                    }
                                                                                                    if (has_total_attnd_hour.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                                    {
                                                                                                        present_count = Convert.ToInt16(has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                                        present_count++;
                                                                                                        has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        has_total_attnd_hour.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                present_count = 0;
                                                if (check_alter == false)
                                                {
                                                    full_hour = dvsemsech[0][temp_hr_field].ToString();
                                                    if (full_hour.Trim() != "")
                                                    {
                                                        temp_has_subj_code.Clear();
                                                        string[] split_full_hour_sem = full_hour.Split(';');
                                                        for (int semi_colon = 0; semi_colon <= split_full_hour_sem.GetUpperBound(0); semi_colon++)
                                                        {
                                                            single_hour = split_full_hour_sem[semi_colon].ToString();
                                                            string[] split_single_hour = single_hour.Split('-');
                                                            if (split_single_hour.GetUpperBound(0) >= 1)
                                                            {
                                                                string subjectno = split_single_hour[0].ToString().Trim();
                                                                //if (subject_no == subjectno)
                                                                //{
                                                                staff_code = Convert.ToString(Session["Staff_Code"]);
                                                                if (subject_no.Trim() == subjectno.Trim())
                                                                {
                                                                    if (!temp_has_subj_code.ContainsKey(subject_no.Trim()))
                                                                    {
                                                                        temp_has_subj_code.Add(subject_no.Trim(), subject_no.Trim());
                                                                        Hashtable has_stud_list = new Hashtable();
                                                                        subj_type = dssem.Tables[2].Rows[0]["LAB"].ToString();
                                                                        if (subj_type.Trim() != "1" && subj_type != "True" && subj_type != "TRUE" && subj_type.Trim().ToLower() != "true")
                                                                        {
                                                                            dsalldetails.Tables[0].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + subject_no + "'";
                                                                            DataView dvlabhr = dsalldetails.Tables[0].DefaultView;
                                                                            for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                                                                            {
                                                                                string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                                                                                dsalldetails.Tables[4].DefaultView.RowFilter = " month_year='" + month_year.ToString() + "' and roll_no='" + rollno + "'";
                                                                                DataView dvattva = dsalldetails.Tables[4].DefaultView;
                                                                                bool checkedFeeOfRoll = false;
                                                                                if (dicFeeOfRollStudents.ContainsKey(rollno.Trim().ToLower()) && dicFeeOnRollStudents.ContainsKey(rollno.Trim().ToLower()))
                                                                                {
                                                                                    DateTime[] dtFeeOfRoll = dicFeeOfRollStudents[rollno.Trim().ToLower()];
                                                                                    if (temp_date >= dtFeeOfRoll[0])
                                                                                    {
                                                                                        bool hasRollOff = false;
                                                                                        DateTime dtDefaultDate = new DateTime(1900, 1, 1);//SqlServer Default Date
                                                                                        if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 0 && temp_date < dtFeeOfRoll[1])
                                                                                        {
                                                                                            hasRollOff = true;
                                                                                            checkedFeeOfRoll = true;
                                                                                        }
                                                                                        else if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 1)
                                                                                        {
                                                                                            hasRollOff = true;
                                                                                            checkedFeeOfRoll = true;
                                                                                        }
                                                                                        else if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 0 && dtFeeOfRoll[1] == dtDefaultDate)
                                                                                        {
                                                                                            hasRollOff = true;
                                                                                            checkedFeeOfRoll = true;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            hasRollOff = false;
                                                                                            checkedFeeOfRoll = false;
                                                                                        }
                                                                                        if (hasRollOff)
                                                                                        {
                                                                                            //if (has_attnd_masterset.ContainsKey("2"))
                                                                                            //{
                                                                                            //    if (has_load_rollno.Contains(rollno + '-' + subjectno))
                                                                                            //    {
                                                                                            //        present_count = Convert.ToInt16(has_load_rollno[rollno + '-' + subjectno]);
                                                                                            //        present_count++;
                                                                                            //        has_load_rollno[rollno + '-' + subjectno] = present_count;
                                                                                            //    }
                                                                                            //    else
                                                                                            //    {
                                                                                            //        has_load_rollno.Add(rollno + '-' + subjectno, 1);
                                                                                            //    }
                                                                                            //}
                                                                                            if (has_total_attnd_hour.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                            {
                                                                                                present_count = Convert.ToInt16(has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                                present_count++;
                                                                                                has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                has_total_attnd_hour.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                                if (dvattva.Count > 0 && !checkedFeeOfRoll)
                                                                                {
                                                                                    string attval = dvattva[0][date_temp_field].ToString();
                                                                                    //if (attval != "12" && attval != "8" && attval.Trim() != "")
                                                                                    if (!string.IsNullOrEmpty(attval) && attval.Trim() != "12" && attval.Trim() != "8" && attval.Trim() != "0")
                                                                                    {
                                                                                        if (has_attnd_masterset.ContainsKey(attval))
                                                                                        {
                                                                                            if (has_load_rollno.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                            {
                                                                                                present_count = Convert.ToInt16(has_load_rollno[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                                present_count++;
                                                                                                has_load_rollno[rollno + '-' + subjectno] = present_count;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                has_load_rollno.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
                                                                                            }
                                                                                        }
                                                                                        if (has_total_attnd_hour.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                        {
                                                                                            present_count = Convert.ToInt16(has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                            present_count++;
                                                                                            has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            has_total_attnd_hour.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            dsalldetails.Tables[2].DefaultView.RowFilter = "hour_value=" + temp_hr + " and subject_no='" + subject_no + "'  and day_value='" + strDay + "' and timetablename='" + dvsemsech[0]["ttname"].ToString() + "'";
                                                                            DataView dvlabbatch = dsalldetails.Tables[2].DefaultView;
                                                                            for (int lb = 0; lb < dvlabbatch.Count; lb++)
                                                                            {
                                                                                string batch = dvlabbatch[lb]["stu_batch"].ToString();
                                                                                if (batch != null && batch.Trim() != "")
                                                                                {
                                                                                    dsalldetails.Tables[0].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + subject_no + "' and batch='" + batch + "' ";
                                                                                    DataView dvlabhr = dsalldetails.Tables[0].DefaultView;
                                                                                    for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                                                                                    {
                                                                                        string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                                                                                        dsalldetails.Tables[4].DefaultView.RowFilter = " month_year='" + month_year.ToString() + "' and roll_no='" + rollno + "'";
                                                                                        DataView dvattva = dsalldetails.Tables[4].DefaultView;
                                                                                        bool checkedFeeOfRoll = false;
                                                                                        if (dicFeeOfRollStudents.ContainsKey(rollno.Trim().ToLower()) && dicFeeOnRollStudents.ContainsKey(rollno.Trim().ToLower()))
                                                                                        {
                                                                                            DateTime[] dtFeeOfRoll = dicFeeOfRollStudents[rollno.Trim().ToLower()];
                                                                                            if (temp_date >= dtFeeOfRoll[0])
                                                                                            {
                                                                                                bool hasRollOff = false;
                                                                                                DateTime dtDefaultDate = new DateTime(1900, 1, 1);//SqlServer Default Date
                                                                                                if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 0 && temp_date < dtFeeOfRoll[1])
                                                                                                {
                                                                                                    hasRollOff = true;
                                                                                                    checkedFeeOfRoll = true;
                                                                                                }
                                                                                                else if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 1)
                                                                                                {
                                                                                                    hasRollOff = true;
                                                                                                    checkedFeeOfRoll = true;
                                                                                                }
                                                                                                else if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 0 && dtFeeOfRoll[1] == dtDefaultDate)
                                                                                                {
                                                                                                    hasRollOff = true;
                                                                                                    checkedFeeOfRoll = true;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    hasRollOff = false;
                                                                                                    checkedFeeOfRoll = false;
                                                                                                }
                                                                                                if (hasRollOff)
                                                                                                {
                                                                                                    //if (has_attnd_masterset.ContainsKey("2"))
                                                                                                    //{
                                                                                                    //    if (has_load_rollno.Contains(rollno + '-' + subjectno))
                                                                                                    //    {
                                                                                                    //        present_count = Convert.ToInt16(has_load_rollno[rollno + '-' + subjectno]);
                                                                                                    //        present_count++;
                                                                                                    //        has_load_rollno[rollno + '-' + subjectno] = present_count;
                                                                                                    //    }
                                                                                                    //    else
                                                                                                    //    {
                                                                                                    //        has_load_rollno.Add(rollno + '-' + subjectno, 1);
                                                                                                    //    }
                                                                                                    //}
                                                                                                    if (has_total_attnd_hour.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                                    {
                                                                                                        present_count = Convert.ToInt16(has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                                        present_count++;
                                                                                                        has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        has_total_attnd_hour.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        if (dvattva.Count > 0 && !checkedFeeOfRoll)
                                                                                        {
                                                                                            string attval = dvattva[0][date_temp_field].ToString();
                                                                                            //if (attval != "12" && attval != "8" && attval.Trim() != "")
                                                                                            if (!string.IsNullOrEmpty(attval) && attval.Trim() != "12" && attval.Trim() != "8" && attval.Trim() != "0")
                                                                                            {
                                                                                                if (has_attnd_masterset.ContainsKey(attval))
                                                                                                {
                                                                                                    if (has_load_rollno.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                                    {
                                                                                                        present_count = Convert.ToInt16(has_load_rollno[rollno + '-' + subjectno]);
                                                                                                        present_count++;
                                                                                                        has_load_rollno[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        has_load_rollno.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
                                                                                                    }
                                                                                                }
                                                                                                if (has_total_attnd_hour.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                                {
                                                                                                    present_count = Convert.ToInt16(has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                                    present_count++;
                                                                                                    has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    has_total_attnd_hour.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            check_alter = false;
                                        }
                                    }
                                }
                            }
                            temp_date = temp_date.AddDays(1);
                        }
                        for (int r = 1; r < FpSpread2.Sheets[0].RowCount; r++)
                        {
                            string roll = FpSpread2.Sheets[0].Cells[r, 1].Text.ToString().Trim().ToLower();
                            double attnd_hr = 0, tot_hr = 0;
                            if (has_total_attnd_hour.Contains(roll.Trim() + '-' + subject_no.Trim()))
                            {
                                tot_hr = Convert.ToDouble(has_total_attnd_hour[roll.Trim() + '-' + subject_no.Trim()]);
                                if (has_load_rollno.Contains(roll.Trim() + '-' + subject_no.Trim()))
                                {
                                    attnd_hr = Convert.ToDouble(has_load_rollno[roll.Trim() + '-' + subject_no.Trim()]);
                                }
                                FpSpread2.Sheets[0].Cells[r, 6].Text = tot_hr.ToString() + " ( " + attnd_hr.ToString() + " )";
                                FpSpread2.Sheets[0].Cells[r, 6].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Labelstaf.Visible = true;
            Labelstaf.Text = ex.ToString();
        }
    }

    public void getspecial_hr(string temp_date, string subject_no, DataSet dsalldetails)
    {
        try
        {
            string hrdetno = string.Empty;
            if (ht_sphr.Contains(Convert.ToString(temp_date)))
            {
                hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(temp_date), ht_sphr));
            }
            if (hrdetno != "")
            {
                Hashtable hatsphco = new Hashtable();
                string splhr_query_master = "select spa.roll_no,spa.attendance,spa.hrdet_no from  specialhr_attendance spa,specialhr_details spd where spa.hrdet_no=spd.hrdet_no and spd.subject_no='" + subject_no + "' and spd.hrdet_no in(" + hrdetno + ") order by spa.hrdet_no";
                DataSet dsval = da.select_method_wo_parameter(splhr_query_master, "Text");
                DataView dvsphratt = new DataView();
                dsalldetails.Tables[0].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString() + "' and subject_no='" + subject_no + "'";
                DataView dvlabhr = dsalldetails.Tables[0].DefaultView;
                for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                {
                    string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                    dsval.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "'";
                    dvsphratt = dsval.Tables[0].DefaultView;
                    for (int spo = 0; spo < dvsphratt.Count; spo++)
                    {
                        string attval = dvsphratt[spo][1].ToString();
                        string value = Attmark(attval);
                        int columno = 0;
                        if (hatsphco.Contains(dvsphratt[spo][2].ToString()))
                        {
                            columno = Convert.ToInt32(hatsphco[dvsphratt[spo][2].ToString()]);
                        }
                        if ((dvsphratt[spo][1].ToString()) != "8")
                        {
                            if (Attmark(dvsphratt[spo][1].ToString()) != "HS")
                            {
                                bool checkedFeeOfRoll = false;
                                if (dicFeeOfRollStudents.ContainsKey(rollno.Trim().ToLower()) && dicFeeOnRollStudents.ContainsKey(rollno.Trim().ToLower()))
                                {
                                    DateTime[] dtFeeOfRoll = dicFeeOfRollStudents[rollno.Trim().ToLower()];
                                    DateTime dtTempDate = new DateTime();
                                    DateTime.TryParseExact(temp_date, "MM/dd/yyyy", null, DateTimeStyles.None, out dtTempDate);

                                    if (dtTempDate >= dtFeeOfRoll[0])
                                    {
                                        bool hasRollOff = false;
                                        DateTime dtDefaultDate = new DateTime(1900, 1, 1);//SqlServer Default Date
                                        if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 0 && dtTempDate < dtFeeOfRoll[1])
                                        {
                                            hasRollOff = true;
                                            checkedFeeOfRoll = true;
                                        }
                                        else if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 1)
                                        {
                                            hasRollOff = true;
                                            checkedFeeOfRoll = true;
                                        }
                                        else if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 0 && dtFeeOfRoll[1] == dtDefaultDate)
                                        {
                                            hasRollOff = true;
                                            checkedFeeOfRoll = true;
                                        }
                                        else
                                        {
                                            hasRollOff = false;
                                            checkedFeeOfRoll = false;
                                        }
                                        if (hasRollOff)
                                        {
                                            //if (has_attnd_masterset.ContainsKey("2"))
                                            //{
                                            //    if (has_load_rollno.Contains(rollno + '-' + subjectno))
                                            //    {
                                            //        present_count = Convert.ToInt16(has_load_rollno[rollno + '-' + subjectno]);
                                            //        present_count++;
                                            //        has_load_rollno[rollno + '-' + subjectno] = present_count;
                                            //    }
                                            //    else
                                            //    {
                                            //        has_load_rollno.Add(rollno + '-' + subjectno, 1);
                                            //    }
                                            //}
                                            if (has_total_attnd_hour.Contains(rollno.Trim() + '-' + subject_no.Trim()))
                                            {
                                                present_count = Convert.ToInt16(has_total_attnd_hour[rollno.Trim() + '-' + subject_no.Trim()]);
                                                present_count++;
                                                has_total_attnd_hour[rollno.Trim() + '-' + subject_no.Trim()] = present_count;
                                            }
                                            else
                                            {
                                                has_total_attnd_hour.Add(rollno.Trim() + '-' + subject_no.Trim(), 1);
                                            }
                                        }
                                    }
                                }
                                if (!checkedFeeOfRoll)
                                {
                                    if (has_attnd_masterset.ContainsKey(attval))
                                    {
                                        if (has_load_rollno.Contains(rollno.Trim() + '-' + subject_no.Trim()))
                                        {
                                            present_count = Convert.ToInt16(has_load_rollno[rollno.Trim() + '-' + subject_no.Trim()]);
                                            present_count++;
                                            has_load_rollno[rollno.Trim() + '-' + subject_no.Trim()] = present_count;
                                        }
                                        else
                                        {
                                            has_load_rollno.Add(rollno.Trim() + '-' + subject_no.Trim(), 1);
                                        }
                                    }
                                    if (has_total_attnd_hour.Contains(rollno.Trim() + '-' + subject_no.Trim()))
                                    {
                                        present_count = Convert.ToInt16(has_total_attnd_hour[rollno.Trim() + '-' + subject_no.Trim()]);
                                        present_count++;
                                        has_total_attnd_hour[rollno.Trim() + '-' + subject_no.Trim()] = present_count;
                                    }
                                    else
                                    {
                                        has_total_attnd_hour.Add(rollno.Trim() + '-' + subject_no.Trim(), 1);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    //Added by Idhris 29-12-2016
    #region allstudentattendancereport new table

    protected void attendanceMark(string appno, int mnthYear, string attDay, int noMaxHrsDay, int noFstHrsDay, int noSndHrsDay, int noMinFstHrsDay, int noMinSndHrsDay, string DateVal, string collegecode)
    {
        try
        {
            DataSet dsload = new DataSet();
            Dictionary<int, int> AttValueMrng = new Dictionary<int, int>();
            Dictionary<int, int> AttvalueEve = new Dictionary<int, int>();
            double attVal = 0;
            int MPCnt = 0;
            int EPCnt = 0;
            int MnullCnt = 0;
            int EnullCnt = 0;
            string SelQ = " select " + attDay + ",A.ROLL_NO,r.app_no from attendance a,registration r where r.roll_no =a.roll_no  and r.college_code=Att_CollegeCode and  r.college_code='" + collegecode + "' AND month_year='" + mnthYear + "' and Att_App_no='" + appno + "' ";
            //d1d1,d1d2,d1d3,d1d4,d1d5,d1d6,d1d7,d1d8,d1d9,d1d10,d2d1,d2d2,d2d3,d2d4,d2d5,d2d6,d2d7,d2d8,d2d9,d2d10,d3d1,d3d2,d3d3,d3d4,d3d5,d3d6,d3d7,d3d8,d3d9,d3d10,d4d1,d4d2,d4d3,d4d4,d4d5,d4d6,d4d7,d4d8,d4d9,d4d10,d5d1,d5d2,d5d3,d5d4,d5d5,d5d6,d5d7,d5d8,d5d9,d5d10,d6d1,d6d2,d6d3,d6d4,d6d5,d6d6,d6d7,d6d8,d6d9,d6d10,d7d1,d7d2,d7d3,d7d4,d7d5,d7d6,d7d7,d7d8,d7d9,d7d10,d8d1,d8d2,d8d3,d8d4,d8d5,d8d6,d8d7,d8d8,d8d9,d8d10,d9d1,d9d2,d9d3,d9d4,d9d5,d9d6,d9d7,d9d8,d9d9,d9d10,d10d1,d10d2,d10d3,d10d4,d10d5,d10d6,d10d7,d10d8,d10d9,d10d10,d11d1,d11d2,d11d3,d11d4,d11d5,d11d6,d11d7,d11d8,d11d9,d11d10,d12d1,d12d2,d12d3,d12d4,d12d5,d12d6,d12d7,d12d8,d12d9,d12d10,d13d1,d13d2,d13d3,d13d4,d13d5,d13d6,d13d7,d13d8,d13d9,d13d10,d14d1,d14d2,d14d3,d14d4,d14d5,d14d6,d14d7,d14d8,d14d9,d14d10,d15d1,d15d2,d15d3,d15d4,d15d5,d15d6,d15d7,d15d8,d15d9,d15d10,d16d1,d16d2,d16d3,d16d4,d16d5,d16d6,d16d7,d16d8,d16d9,d16d10,d17d1,d17d2,d17d3,d17d4,d17d5,d17d6,d17d7,d17d8,d17d9,d17d10,d18d1,d18d2,d18d3,d18d4,d18d5,d18d6,d18d7,d18d8,d18d9,d18d10,d19d1,d19d2,d19d3,d19d4,d19d5,d19d6,d19d7,d19d8,d19d9,d19d10,d20d1,d20d2,d20d3,d20d4,d20d5,d20d6,d20d7,d20d8,d20d9,d20d10,d21d1,d21d2,d21d3,d21d4,d21d5,d21d6,d21d7,d21d8,d21d9,d21d10,d22d1,d22d2,d22d3,d22d4,d22d5,d22d6,d22d7,d22d8,d22d9,d22d10,d23d1,d23d2,d23d3,d23d4,d23d5,d23d6,d23d7,d23d8,d23d9,d23d10,d24d1,d24d2,d24d3,d24d4,d24d5,d24d6,d24d7,d24d8,d24d9,d24d10,d25d1,d25d2,d25d3,d25d4,d25d5,d25d6,d25d7,d25d8,d25d9,d25d10,d26d1,d26d2,d26d3,d26d4,d26d5,d26d6,d26d7,d26d8,d26d9,d26d10,d27d1,d27d2,d27d3,d27d4,d27d5,d27d6,d27d7,d27d8,d27d9,d27d10,d28d1,d28d2,d28d3,d28d4,d28d5,d28d6,d28d7,d28d8,d28d9,d28d10,d29d1,d29d2,d29d3,d29d4,d29d5,d29d6,d29d7,d29d8,d29d9,d29d10,d30d1,d30d2,d30d3,d30d4,d30d5,d30d6,d30d7,d30d8,d30d9,d30d10,d31d1,d31d2,d31d3,d31d4,d31d5,d31d6,d31d7,d31d8,d31d9,d31d10
            dsload.Clear();
            dsload = da.select_method_wo_parameter(SelQ, "Text");
            if (dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count > 0)
            {
                for (int sel = 0; sel < noMaxHrsDay; sel++)
                {
                    if (sel < noFstHrsDay)
                    {
                        double.TryParse(Convert.ToString(dsload.Tables[0].Rows[0][sel]), out attVal);
                        if (attVal != 0 || attVal != 0.0)
                        {
                            if (attVal == 1)
                                MPCnt++;
                            else
                            {
                                //  MOCnt = attVal;
                                if (!AttValueMrng.ContainsKey(Convert.ToInt32(attVal)))
                                    AttValueMrng.Add(Convert.ToInt32(attVal), 1);
                                else
                                {
                                    int Cnt = 0;
                                    int.TryParse(Convert.ToString(AttValueMrng[Convert.ToInt32(attVal)]), out Cnt);
                                    Cnt += 1;
                                    AttValueMrng.Remove(Convert.ToInt32(attVal));
                                    AttValueMrng.Add(Convert.ToInt32(attVal), Cnt);
                                }
                            }
                        }
                        else
                            MnullCnt++;
                    }
                    else if (sel >= noSndHrsDay)
                    {
                        double.TryParse(Convert.ToString(dsload.Tables[0].Rows[0][sel]), out attVal);
                        if (attVal != 0 || attVal != 0.0)
                        {
                            if (attVal == 1)
                                EPCnt++;
                            else
                            {
                                // EOCnt = attVal;
                                if (!AttvalueEve.ContainsKey(Convert.ToInt32(attVal)))
                                    AttvalueEve.Add(Convert.ToInt32(attVal), 1);
                                else
                                {
                                    int Cnt = 0;
                                    int.TryParse(Convert.ToString(AttvalueEve[Convert.ToInt32(attVal)]), out Cnt);
                                    Cnt += 1;
                                    AttvalueEve.Remove(Convert.ToInt32(attVal));
                                    AttvalueEve.Add(Convert.ToInt32(attVal), Cnt);
                                }
                            }
                        }
                        else
                            EnullCnt++;
                    }
                }
                int matt = attendanceSet(MPCnt, MnullCnt, noMinFstHrsDay, AttValueMrng);
                int eatt = attendanceSet(EPCnt, EnullCnt, noMinSndHrsDay, AttvalueEve);
                if (matt != null && eatt != null)
                {
                    string InsQ = " if exists (select AppNo from AllStudentAttendanceReport where dateofattendance='" + DateVal + "' and appno='" + dsload.Tables[0].Rows[0]["app_no"] + "')update AllStudentAttendanceReport set mleavecode='" + matt + "',eleavecode='" + eatt + "' where  dateofattendance='" + DateVal + "' and appno='" + dsload.Tables[0].Rows[0]["app_no"] + "' else insert into AllStudentAttendanceReport(AppNo, DateofAttendance,MLeaveCode,ELeaveCode) values('" + dsload.Tables[0].Rows[0]["app_no"] + "','" + DateVal + "','" + matt + "','" + eatt + "')";
                    int save = da.update_method_wo_parameter(InsQ, "Text");
                }
            }
        }
        catch { }
    }

    protected int attendanceSet(int attCnt, int nullCnt, int hrCntCheck, Dictionary<int, int> val)
    {
        int attVal = 0;
        try
        {
            //if (attCnt >= hrCntCheck)
            //    attVal = Convert.ToInt32(leave);
            //else if (nullCnt > 0)
            //    attVal = 0;
            //else
            //    attVal = Convert.ToInt32(leave);
            if (attCnt >= hrCntCheck)
                attVal = 1;
            else if (nullCnt > 0)
                attVal = 0;
            else
            {
                val = val.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                foreach (KeyValuePair<int, int> txt in val)
                {
                    attVal = Convert.ToInt32(txt.Key);
                    break;
                }
            }
        }
        catch { }
        return attVal;
    }

    #endregion

    /// <summary>
    /// It is Used to Get Day Name For DayOrder
    /// Developed By Malang Raja T
    /// </summary>
    /// <param name="dayOrder"></param>
    /// <returns></returns>
    private string findDayName(byte dayOrder)
    {
        string dayName = string.Empty;
        switch (dayOrder)
        {
            case 0:
                dayName = string.Empty;
                break;
            case 1:
                dayName = "mon";
                break;
            case 2:
                dayName = "tue";
                break;
            case 3:
                dayName = "wed";
                break;
            case 4:
                dayName = "thu";
                break;
            case 5:
                dayName = "fri";
                break;
            case 6:
                dayName = "sat";
                break;
            case 7:
                dayName = "sun";
                break;
            default:
                break;
        }
        return dayName;
    }

    /// <summary>
    /// This Is Used to Get All Fee of Students 
    /// Developed By Malang Raja T
    /// </summary>
    /// <param name="dicFeeOfRoll">referenced type Dictionary To Hold Fee of Roll Student</param>
    /// <param name="dtFromDate">From Date Format dd-MM-yyyy</param>
    /// <param name="dtToDate">To Date Format dd-MM-yyyy</param>
    private void GetFeeOfRollStudent(ref Dictionary<string, DateTime[]> dicFeeOffRollStudents, ref Dictionary<string, byte> dicFeeOnRoll, string fromDate = null, string toDate = null)
    {
        try
        {
            DataSet dsFeeOfRollDate = new DataSet();
            DateTime dtFromDate = new DateTime();
            DateTime dtToDate = new DateTime();
            bool isFromSuccess = false;
            bool isToSuccess = false;
            if (!string.IsNullOrEmpty(fromDate))
            {
                isFromSuccess = DateTime.TryParseExact(fromDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtFromDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                isToSuccess = DateTime.TryParseExact(toDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtToDate);
            }
            string qryFeeOfRollDate = string.Empty;
            if (isFromSuccess && isToSuccess)
            {
                qryFeeOfRollDate = " and curr_date between '" + dtFromDate.ToString("mm/dd/yyyy") + "' and '" + dtToDate.ToString("mm/dd/yyyy") + "'";
            }
            else if (isFromSuccess)
            {
                qryFeeOfRollDate = " and curr_date='" + dtFromDate.ToString("mm/dd/yyyy") + "'";
            }
            else if (isToSuccess)
            {
                qryFeeOfRollDate = " and curr_date='" + dtToDate.ToString("mm/dd/yyyy") + "'";
            }
            else
            {
                qryFeeOfRollDate = string.Empty;
            }
            //qry = "select roll_no, Convert(varchar(50),curr_date,103) as curr_date,infr_type,Convert(varchar(50),CAST(ack_date as DateTime),103) as ack_date,Convert(varchar(50),feeOnRollDate,103) as feeOnRollDate,ack_diss,ack_fine,ack_remarks,ack_susp,ack_warn,tot_days,fine_amo,semester,ack_fee_of_roll,Remark,Convert(varchar(50),suspendFromDate,103) as suspendFromDate,Convert(varchar(50),suspendToDate,103) as suspendToDate from stucon where (ack_fee_of_roll=1 or feeOnRollDate is not null) " + qryFeeOfRollDate;
            string qry = "select roll_no,Convert(varchar(50),curr_date,103) as curr_date,Convert(varchar(50),CAST(ack_date as DateTime),103) as ack_date,Convert(varchar(50),feeOnRollDate,103) as feeOnRollDate,semester,ack_fee_of_roll from stucon where (ack_fee_of_roll=1 or feeOnRollDate is not null) " + qryFeeOfRollDate;
            dsFeeOfRollDate = da.select_method_wo_parameter(qry, "text");
            if (dsFeeOfRollDate.Tables.Count > 0 && dsFeeOfRollDate.Tables[0].Rows.Count > 0)
            {
                dicFeeOffRollStudents.Clear();
                foreach (DataRow drFeeOfRoll in dsFeeOfRollDate.Tables[0].Rows)
                {
                    string rollNo = Convert.ToString(drFeeOfRoll["roll_no"]).Trim().ToLower();
                    string feeOffRollDate = Convert.ToString(drFeeOfRoll["curr_date"]).Trim();
                    string feeOffRollDate1 = Convert.ToString(drFeeOfRoll["ack_date"]).Trim();
                    string feeOnRollDate = Convert.ToString(drFeeOfRoll["feeOnRollDate"]).Trim();
                    string isFeeOfRoll = Convert.ToString(drFeeOfRoll["ack_fee_of_roll"]).Trim();
                    byte FeeOnRoll = 0;
                    byte.TryParse(isFeeOfRoll.Trim(), out FeeOnRoll);
                    DateTime dtFeeOffRollDate = new DateTime();
                    DateTime dtFeeOnRollDate = new DateTime();
                    bool isFeeOff = DateTime.TryParseExact(feeOffRollDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtFeeOffRollDate);
                    bool isFeeOn = DateTime.TryParseExact(feeOnRollDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtFeeOnRollDate);
                    DateTime[] dtFeeRoll = new DateTime[2];
                    dtFeeRoll[0] = dtFeeOffRollDate;
                    dtFeeRoll[1] = dtFeeOnRollDate;
                    if (!isFeeOn)
                    {
                        //dtFeeOnRollDate = ;
                    }
                    if (!dicFeeOffRollStudents.ContainsKey(rollNo.Trim().ToLower()))
                    {
                        dicFeeOffRollStudents.Add(rollNo.Trim().ToLower(), dtFeeRoll);
                    }
                    if (!dicFeeOnRoll.ContainsKey(rollNo.Trim().ToLower().ToLower()))
                    {
                        dicFeeOnRoll.Add(rollNo.Trim().ToLower(), FeeOnRoll);
                    }
                    //string rollNo = Convert.ToString(drFeeOfRoll[""]).Trim();
                }
            }
        }
        catch
        {
        }
    }

    private bool CheckSchoolOrCollege(string collegeCode)
    {
        bool isSchoolOrCollege = false;
        try
        {
            if (!string.IsNullOrEmpty(collegeCode))
            {
                //qry = "select ISNULL(InstType,'0') as InstType,case when ISNULL(InstType,'0')='0' then 'College' when ISNULL(InstType,'0')='1' then 'School' end as CollegeOrSchool from collinfo where college_code='" + collegeCode + "'";
                string qry = "select ISNULL(InstType,'0') as InstType from collinfo where college_code='" + collegeCode + "'";
                string insType = da.GetFunction(qry);
                if (!string.IsNullOrEmpty(insType) && insType.Trim() != "0")
                {
                    isSchoolOrCollege = true;
                }
                else
                {
                    isSchoolOrCollege = false;
                }

            }
            return isSchoolOrCollege;
        }
        catch
        {
            return false;
        }
    }

    protected void btnYes_Click(object sender, EventArgs e)
    {
        try
        {
            Labelstaf.Visible = false;
            Boolean nullflag = false;
            Boolean savefalg = true;//Added by Srinath 23/8/2013
            int savevalue = 0;
            int insert = 0;
            string getroll = string.Empty;
            string batch = string.Empty;
            string section = string.Empty;
            string attendancequery = string.Empty;
            string val = string.Empty;
            string strorder = filterfunction();
            DataSet dsattendance = new DataSet();
            Hashtable hatattendance = new Hashtable();
            Hashtable hatreason = new Hashtable();
            Hashtable hatattvalue = new Hashtable();
            ArrayList notarray = new ArrayList();
            DataSet data1 = new DataSet();
            WebService web = new WebService();
            dailyentryflag = false;
            attendanceentryflag = false;
            bool isSchoolAttendance = false;
            staff_code = (string)Session["Staff_Code"];
            if (tbfdate.Text != "" && tbtodate.Text != "")
            {
                string reason_var = string.Empty;
                string str_rollno;
                string AppNo = string.Empty;
                string regno = string.Empty;
                string admno = string.Empty;
                int actcol;
                string str_Date = string.Empty;
                string str_day = string.Empty;
                string Atmonth = string.Empty;
                string Atyear = string.Empty;
                long strdate;
                string str_hour = string.Empty;
                string strquery = string.Empty;
                string Att_mark1 = string.Empty;
                string Att_value1 = string.Empty;
                string dcolumn = string.Empty;
                string strt_date = string.Empty;
                int startsem_date = 0;
                int total_conduct_hour = 0;
                int absent_hour = 0;
                actcol = FpSpread1.Sheets[0].ActiveColumn;
                ar = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveRow.ToString());
                ac = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveColumn.ToString());
                string[] split_tag_val;
                if (FpSpread1.Sheets[0].Cells[ar, ac].Locked == false)
                {
                    if (ar != -1)
                    {
                        string spread_text = FpSpread1.Sheets[0].Cells[ar, ac].Text;
                        string text_val = FpSpread1.Sheets[0].Cells[ar, ac].Text;
                        if (spread_text != "" && spread_text != "Sunday Holiday")
                        {
                            getcelltag = FpSpread1.Sheets[0].GetTag(ar, ac).ToString();
                            string avoidholiday = string.Empty;
                            string avoidholidaytext = string.Empty;
                            string[] spiltgetceltag = getcelltag.Split('*');
                            string[] spilttext = text_val.Split('*');
                            for (int k = 0; k <= spiltgetceltag.GetUpperBound(0); k++)
                            {
                                string[] spitvalue = spiltgetceltag[k].Split('-');
                                if (spitvalue[0].ToLower().Trim() == "selected day is holiday")
                                {
                                }
                                else
                                {
                                    if (avoidholiday == "")
                                    {
                                        avoidholiday = spiltgetceltag[k].ToString();
                                        avoidholidaytext = spilttext[k].ToString();
                                    }
                                    else
                                    {
                                        avoidholiday = avoidholiday + '*' + spiltgetceltag[k].ToString();
                                        avoidholidaytext = avoidholidaytext + '*' + spilttext[k].ToString();
                                    }
                                }
                            }
                            getcelltag = avoidholiday;
                            text_val = avoidholidaytext;
                        }
                    }
                }
                string strstaffselector = string.Empty;
                attendancequery = "select distinct LeaveCode,CalcFlag from AttMasterSetting";
                dsattendance.Dispose();
                dsattendance.Reset();
                dsattendance = da.select_method(attendancequery, hat, "Text");
                present_calcflag.Clear();
                absent_calcflag.Clear();
                hatattendance.Clear();
                string minimumabsentsms = da.GetFunction("select value from Master_Settings where settings='Minimum_days_Absent_Sms'");
                if (dsattendance.Tables.Count > 0 && dsattendance.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsattendance.Tables[0].Rows.Count; i++)
                    {
                        if (!hatattendance.Contains(dsattendance.Tables[0].Rows[i]["LeaveCode"].ToString()))
                        {
                            hatattendance.Add(dsattendance.Tables[0].Rows[i]["LeaveCode"].ToString(), dsattendance.Tables[0].Rows[i]["CalcFlag"].ToString());
                        }
                        //for Present Absent Count=
                        if (dsattendance.Tables[0].Rows[i]["calcflag"].ToString() == "0")
                        {
                            present_calcflag.Add(dsattendance.Tables[0].Rows[i]["leavecode"].ToString(), dsattendance.Tables[0].Rows[i]["leavecode"].ToString());
                        }
                        if (dsattendance.Tables[0].Rows[i]["calcflag"].ToString() == "1")
                        {
                            absent_calcflag.Add(dsattendance.Tables[0].Rows[i]["leavecode"].ToString(), dsattendance.Tables[0].Rows[i]["leavecode"].ToString());
                        }
                    }
                }
                if (check_attendance.Checked == true)  // ********************** Added by jairam 04-10-2014 ********************************
                {
                    for (int att_col = 7; att_col <= FpSpread2.Sheets[0].ColumnCount - 2; att_col = att_col + 2)
                    {
                        hatreason.Clear();
                        hatattvalue.Clear();
                        hr = FpSpread2.Sheets[0].ColumnHeader.Cells[1, att_col].Tag.ToString();
                        str_Date = FpSpread2.Sheets[0].ColumnHeader.Cells[0, att_col].Text;
                        string[] split = str_Date.Split(new Char[] { '-' });
                        str_day = (Convert.ToInt16(split[0].ToString())).ToString();
                        Atmonth = (Convert.ToInt16(split[1].ToString())).ToString();
                        Atyear = split[2].ToString();
                        strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                        str_hour = Convert.ToString(hr);
                        dcolumn = "d" + str_day + "d" + str_hour;
                        string[] spilttext = getcelltag.Split('*');
                        for (int j = 0; j <= spilttext.GetUpperBound(0); j++)
                        {
                            split_tag_val = spilttext[j].Split('-');
                            if (split_tag_val.GetUpperBound(0) >= 7)
                            {
                                batch = split_tag_val[4].ToString();
                                degree_code = split_tag_val[0].ToString();
                                semester = split_tag_val[1].ToString();
                                subject_no = split_tag_val[2].ToString();
                                section = "and r.Sections='" + split_tag_val[3].ToString() + "'";
                            }
                            else
                            {
                                batch = split_tag_val[3].ToString();
                                degree_code = split_tag_val[0].ToString();
                                semester = split_tag_val[1].ToString();
                                subject_no = split_tag_val[2].ToString();
                                section = string.Empty;
                            }
                        }
                        FpSpread2.SaveChanges();
                        int hour_day = 0;
                        string number_hour_query = string.Empty;
                        number_hour_query = " select * from periodattndschedule where degree_code=" + degree_code + " and semester=" + semester + "";
                        ds.Clear();
                        ds = da.select_method_wo_parameter(number_hour_query, "Text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            hour_day = Convert.ToInt32(ds.Tables[0].Rows[0]["No_of_hrs_per_day"]);
                        }
                        string hourwise = string.Empty;
                        // string daywise =string.Empty;
                        string hourwisedata = string.Empty;
                        DataSet ds1 = new DataSet();
                        string settingquery = string.Empty;
                        settingquery = "select TextName,Taxtval from Attendance_Settings where  College_Code ='" + Session["collegecode"].ToString() + "'and user_id='" + Session["usercode"].ToString() + "'";
                        // settingquery = settingquery + " ;select * from attendance";
                        ds1.Clear();
                        ds1 = da.select_method_wo_parameter(settingquery, "Text");
                        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            DataView dv_demand_data = new DataView();
                            ds1.Tables[0].DefaultView.RowFilter = "TextName in ('Hour')";
                            dv_demand_data = ds1.Tables[0].DefaultView;
                            if (dv_demand_data.Count > 0)
                            {
                                if (dv_demand_data[0]["TextName"].ToString() == "Hour" && Convert.ToInt32(dv_demand_data[0]["Taxtval"]) == 1)
                                {
                                    hourwise = "1";
                                    hourwisedata = "Hour";
                                }
                                else if (dv_demand_data[0]["TextName"].ToString() == "Hour" && Convert.ToInt32(dv_demand_data[0]["Taxtval"]) == 0)
                                {
                                    hourwise = "0";
                                }
                            }
                        }
                        for (int Att_row = 1; Att_row < FpSpread2.Sheets[0].RowCount - 2; Att_row++)
                        {
                            if (FpSpread2.Sheets[0].Cells[Att_row, 1].Text.ToString().Trim() != "" && FpSpread2.Sheets[0].Cells[Att_row, 1].Text.ToString() != null)
                            {
                                str_rollno = FpSpread2.Sheets[0].Cells[Att_row, 1].Text.ToString();
                                AppNo = FpSpread2.Sheets[0].Cells[Att_row, 1].Note.ToString();
                                regno = FpSpread2.Sheets[0].Cells[Att_row, 2].Text.ToString();
                                admno = FpSpread2.Sheets[0].Cells[Att_row, 3].Text.ToString();
                                reason_var = Convert.ToString(FpSpread2.GetEditValue(Att_row, 7));
                                if (reason_var == "System.Object")
                                {
                                    reason_var = FpSpread2.Sheets[0].Cells[Att_row, att_col + 1].Text.ToString();
                                }
                                if (reason_var == null)
                                {
                                    reason_var = "Null";
                                }
                                Att_mark1 = Convert.ToString(FpSpread2.GetEditValue(Att_row, att_col).ToString());
                                FpSpread2.SaveChanges();
                                if (Att_mark1 == "System.Object")
                                {
                                    Att_mark1 = FpSpread2.Sheets[0].Cells[Att_row, att_col].Text.ToString();
                                }
                                if (Att_mark1 != "")
                                {
                                    nullflag = true;
                                }
                                Att_value1 = Attvalues(Att_mark1);
                                for (int lk = 1; lk <= hour_day; lk++)
                                {
                                    dcolumn = "d" + str_day + "d" + lk;
                                    string hour_value = Convert.ToString(lk);
                                    //if (reason_var.Trim() != "")
                                    //{
                                    //val =string.Empty;
                                    //if (hatreason.Contains(str_rollno))
                                    //{
                                    //    val = GetCorrespondingKey(str_rollno, hatreason).ToString();
                                    //}
                                    //if (val != reason_var)
                                    //{
                                    hat.Clear();
                                    hat.Add("AtWr_App_no", AppNo);
                                    hat.Add("AttWr_CollegeCode", Convert.ToString(Session["collegecode"]));
                                    hat.Add("columnname", dcolumn);
                                    hat.Add("roll_no", str_rollno);
                                    hat.Add("month_year", strdate);
                                    hat.Add("values", reason_var);
                                    strquery = "sp_ins_upd_student_attendance_reason";
                                    insert = da.insert_method(strquery, hat, "sp");
                                    //}
                                    // }
                                    //Insert Attendance  Value
                                    //if (Att_value1.Trim() != "")
                                    //{
                                    //val =string.Empty;
                                    //if (hatattvalue.Contains(str_rollno))
                                    //{
                                    //    val = GetCorrespondingKey(str_rollno, hatattvalue).ToString();
                                    //}
                                    //if (val != Att_value1)
                                    //{
                                    hat.Clear();
                                    hat.Add("columnname", dcolumn);
                                    hat.Add("roll_no", str_rollno);
                                    hat.Add("month_year", strdate);
                                    hat.Add("values", Att_value1);
                                    strquery = "sp_ins_upd_student_attendance";
                                    insert = da.insert_method(strquery, hat, "sp");
                                    //}
                                    //}
                                    if (hourwise == "1")
                                    {
                                        if (absent_calcflag.Contains(Att_value1) == true)
                                        {
                                            string value_return = web.coundected_hour(strdate, startsem_date, str_rollno, absent_calcflag, notarray);
                                            if (value_return == "Empty")
                                            {
                                                total_conduct_hour = 1;
                                                absent_hour = 1;
                                            }
                                            else
                                            {
                                                string[] splitvalue = value_return.Split('-');
                                                if (splitvalue.Length > 0)
                                                {
                                                    if (splitvalue[0].ToString() != "")
                                                    {
                                                        total_conduct_hour = Convert.ToInt32(splitvalue[0]);
                                                        total_conduct_hour++;
                                                    }
                                                    else
                                                    {
                                                        total_conduct_hour++;
                                                    }
                                                    if (splitvalue[1].ToString() != "")
                                                    {
                                                        absent_hour = Convert.ToInt32(splitvalue[1]);
                                                        absent_hour++;
                                                    }
                                                    else
                                                    {
                                                        absent_hour++;
                                                    }
                                                }
                                            }
                                            SendingSms(str_rollno.ToString(), AppNo, regno, admno, AttDate, hour_value, degree_code, total_conduct_hour, absent_hour);
                                            sendvoicecall(str_rollno.ToString(), AttDate, hour_value, batch, degree_code);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }//****************************************** End ***********************
                else
                {
                    #region Added by Idhris 29-12-2016
                    string savehoursqlstrq;
                    int totalhor;
                    string noofhours_save = string.Empty;
                    string no_firsthalf = string.Empty;
                    string no_secondhalf = string.Empty;
                    string no_minpresent_firsthalf = string.Empty;
                    string no_minpresent_secondhalf = string.Empty;
                    string min_per_day = string.Empty;
                    bool daysGot = false;
                    string sb_aattddaayy = string.Empty;
                    #endregion
                    for (int att_col = 7; att_col <= FpSpread2.Sheets[0].ColumnCount - 2; att_col = att_col + 2)
                    {
                        hatreason.Clear();
                        hatattvalue.Clear();
                        hr = FpSpread2.Sheets[0].ColumnHeader.Cells[1, att_col].Tag.ToString();
                        str_Date = FpSpread2.Sheets[0].ColumnHeader.Cells[0, att_col].Text;
                        string[] split = str_Date.Split(new Char[] { '-' });
                        str_day = (Convert.ToInt16(split[0].ToString())).ToString();
                        Atmonth = (Convert.ToInt16(split[1].ToString())).ToString();
                        Atyear = split[2].ToString();
                        strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                        str_hour = Convert.ToString(hr);
                        dcolumn = "d" + str_day + "d" + str_hour;
                        string[] spilttext = getcelltag.Split('*');
                        for (int j = 0; j <= spilttext.GetUpperBound(0); j++)
                        {
                            split_tag_val = spilttext[j].Split('-');
                            if (split_tag_val.GetUpperBound(0) >= 7)
                            {
                                batch = split_tag_val[4].ToString();
                                degree_code = split_tag_val[0].ToString();
                                semester = split_tag_val[1].ToString();
                                subject_no = split_tag_val[2].ToString();
                                section = "and r.Sections='" + split_tag_val[3].ToString() + "'";
                            }
                            else
                            {
                                batch = split_tag_val[3].ToString();
                                degree_code = split_tag_val[0].ToString();
                                semester = split_tag_val[1].ToString();
                                subject_no = split_tag_val[2].ToString();
                                section = string.Empty;
                            }
                            #region Added by Idhris 29-12-2016
                            if (!daysGot)
                            {
                                //daysGot = true;
                                savehoursqlstrq = "select No_of_hrs_per_day,no_of_hrs_I_half_day,no_of_hrs_II_half_day,min_pres_II_half_day ,min_pres_I_half_day,min_hrs_per_day  from PeriodAttndSchedule where degree_code=" + degree_code + " and semester=" + semester + "";
                                DataSet dsDa = da.select_method_wo_parameter(savehoursqlstrq, "Text");
                                if (dsDa.Tables.Count > 0 && dsDa.Tables[0].Rows.Count > 0)
                                {
                                    noofhours_save = dsDa.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
                                    no_firsthalf = dsDa.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString();
                                    no_secondhalf = dsDa.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString();
                                    no_minpresent_firsthalf = dsDa.Tables[0].Rows[0]["min_pres_I_half_day"].ToString();
                                    no_minpresent_secondhalf = dsDa.Tables[0].Rows[0]["min_pres_II_half_day"].ToString();
                                    min_per_day = dsDa.Tables[0].Rows[0]["min_hrs_per_day"].ToString();
                                }
                                sb_aattddaayy = "d" + str_day + "d1," + "d" + str_day + "d2," + "d" + str_day + "d3," + "d" + str_day + "d4," + "d" + str_day + "d5," + "d" + str_day + "d6," + "d" + str_day + "d7," + "d" + str_day + "d8," + "d" + str_day + "d9," + "d" + str_day + "d10";
                            }
                            #endregion
                            Session["StaffSelector"] = "0";
                            strstaffselector = string.Empty;
                            string staffbatchyear = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Session["collegecode"].ToString() + "'");
                            string[] splitminimumabsentsms = staffbatchyear.Split('-');
                            if (splitminimumabsentsms.Length == 2)
                            {
                                int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
                                if (splitminimumabsentsms[0].ToString() == "1")
                                {
                                    if (Convert.ToInt32(batch) >= batchyearsetting)
                                    {
                                        Session["StaffSelector"] = "1";
                                    }
                                }
                            }
                            if (Session["StaffSelector"].ToString() == "1")
                            {
                                strstaffselector = " and s.staffcode like '%" + Session["Staff_Code"].ToString() + "%'";
                            }
                            string startdatequery = string.Empty;
                            startdatequery = "select leavecode from AttMasterSetting where calcflag='2' and collegecode=" + Session["collegecode"].ToString() + "";
                            startdatequery = startdatequery + " select convert(varchar(10),start_date,103) as start_date from seminfo where  degree_code='" + degree_code + "' and semester='" + semester + "' and batch_year='" + batch + "'";
                            startdatequery = startdatequery + " select convert(varchar(10),holiday_date,103)as holiday_date ,halforfull,morning,evening from holidayStudents where degree_code ='" + degree_code + "' and semester ='" + semester + "'";
                            data1.Clear();
                            data1 = da.select_method_wo_parameter(startdatequery, "Text");
                            if (data1.Tables.Count > 0 && data1.Tables[0].Rows.Count > 0)
                            {
                                for (int val1 = 0; val1 < data1.Tables[0].Rows.Count; val1++)
                                {
                                    notarray.Add(data1.Tables[0].Rows[val1]["leavecode"].ToString());
                                }
                            }
                            if (data1.Tables.Count > 1 && data1.Tables[1].Rows.Count > 0)
                            {
                                strt_date = data1.Tables[1].Rows[0]["start_date"].ToString();
                                string[] split1 = strt_date.Split(new Char[] { '/' });
                                string str_day1 = split1[0].ToString();
                                string Atmonth1 = split1[1].ToString();
                                string Atyear1 = split1[2].ToString();
                                startsem_date = (Convert.ToInt32(Atmonth1) + Convert.ToInt32(Atyear1) * 12);
                            }
                            //Check Attendance With Reason
                            strquery = "select r.roll_no," + dcolumn + " from attendance_withreason a,Registration r where a.roll_no=r.Roll_No and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'Debar' and r.Batch_Year=" + batch + " ";
                            strquery = strquery + " and r.degree_code=" + degree_code + " and r.Current_Semester=" + semester + " " + section + " and month_year='" + strdate + "' and (" + dcolumn + "<>'' and " + dcolumn + " is not null)";
                            dsattendance.Reset();
                            dsattendance.Dispose();
                            dsattendance = da.select_method(strquery, hat, "Text");
                            if (dsattendance.Tables.Count > 0 && dsattendance.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsattendance.Tables[0].Rows.Count; i++)
                                {
                                    str_rollno = dsattendance.Tables[0].Rows[i]["Roll_no"].ToString().Trim().ToLower();
                                    reason_var = dsattendance.Tables[0].Rows[i]["" + dcolumn + ""].ToString();
                                    if (!hatreason.Contains(str_rollno))
                                    {
                                        hatreason.Add(str_rollno, reason_var);
                                    }
                                }
                            }
                            //Check Attendance
                            strquery = "select r.roll_no," + dcolumn + " from attendance a,Registration r,subjectChooser s where r.Roll_No=s.roll_no and a.roll_no=r.Roll_No and s.roll_no=a.roll_no and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'Debar'  and r.Batch_Year=" + batch + " and r.degree_code=" + degree_code + " ";
                            strquery = strquery + "and r.Current_Semester=" + semester + " " + section + " and month_year='" + strdate + "' and s.subject_no=" + subject_no + " " + strstaffselector + " and (" + dcolumn + "<>''and " + dcolumn + "<>'0' and " + dcolumn + " is not null )";//barath 26.01.17
                            //and a.Att_CollegeCode=r.college_code 
                            dsattendance.Reset();
                            dsattendance.Dispose();
                            dsattendance = da.select_method(strquery, hat, "Text");
                            if (dsattendance.Tables.Count > 0 && dsattendance.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsattendance.Tables[0].Rows.Count; i++)
                                {
                                    str_rollno = dsattendance.Tables[0].Rows[i]["Roll_no"].ToString().Trim().ToLower();
                                    reason_var = dsattendance.Tables[0].Rows[i]["" + dcolumn + ""].ToString();
                                    if (!hatattvalue.Contains(str_rollno.Trim().ToLower()))
                                    {
                                        hatattvalue.Add(str_rollno.Trim().ToLower(), reason_var);
                                    }
                                }
                            }
                        }
                        for (int Att_row = 1; Att_row < FpSpread2.Sheets[0].RowCount - 2; Att_row++)
                        {
                            if (FpSpread2.Sheets[0].Cells[Att_row, 1].Text.ToString().Trim() != "" && FpSpread2.Sheets[0].Cells[Att_row, 1].Text.ToString() != null)
                            {
                                str_rollno = FpSpread2.Sheets[0].Cells[Att_row, 1].Text.ToString();
                                AppNo = FpSpread2.Sheets[0].Cells[Att_row, 1].Note.ToString();
                                regno = FpSpread2.Sheets[0].Cells[Att_row, 2].Text.ToString();
                                admno = FpSpread2.Sheets[0].Cells[Att_row, 3].Text.ToString();
                                //barath
                                string clgcode = FpSpread2.Sheets[0].Cells[Att_row, 1].Tag.ToString();
                                reason_var = Convert.ToString(FpSpread2.GetEditValue(Att_row, 7));
                                if (reason_var == "System.Object")
                                {
                                    reason_var = FpSpread2.Sheets[0].Cells[Att_row, att_col + 1].Text.ToString();
                                }
                                if (reason_var == null)
                                {
                                    reason_var = "Null";
                                }
                                Att_mark1 = Convert.ToString(FpSpread2.GetEditValue(Att_row, att_col).ToString());
                                FpSpread2.SaveChanges();
                                if (Att_mark1 == "System.Object")
                                {
                                    Att_mark1 = FpSpread2.Sheets[0].Cells[Att_row, att_col].Text.ToString();
                                }
                                if (Att_mark1 != "")
                                {
                                    nullflag = true;
                                }
                                Att_value1 = Attvalues(Att_mark1);
                                {
                                    //Insert Attendance With Reason Value
                                    if (reason_var.Trim() != "" || hatreason.Contains(str_rollno.Trim().ToLower()))
                                    {
                                        val = string.Empty;
                                        if (hatreason.Contains(str_rollno.Trim().ToLower()))
                                        {
                                            val = GetCorrespondingKey(str_rollno.Trim().ToLower(), hatreason).ToString();
                                        }
                                        if (val != reason_var)
                                        {
                                            hat.Clear();
                                            hat.Add("AttWr_CollegeCode", clgcode);// Session["collegecode"].ToString());
                                            hat.Add("AtWr_App_no", AppNo);
                                            hat.Add("columnname", dcolumn);
                                            hat.Add("roll_no", str_rollno);
                                            hat.Add("month_year", strdate);
                                            hat.Add("values", reason_var);
                                            strquery = "sp_ins_upd_student_attendance_reason";
                                            insert = da.insert_method(strquery, hat, "sp");
                                        }
                                    }
                                    //Insert Attendance  Value
                                    if (Att_value1 != "" || hatattvalue.Contains(str_rollno.Trim().ToLower()))
                                    {
                                        val = string.Empty;
                                        if (hatattvalue.Contains(str_rollno.Trim().ToLower()))
                                        {
                                            val = GetCorrespondingKey(str_rollno.Trim().ToLower(), hatattvalue).ToString();
                                        }
                                        if (val != Att_value1)
                                        {
                                            hat.Clear();
                                            hat.Add("Att_App_no", AppNo);
                                            hat.Add("Att_CollegeCode", clgcode);// Session["collegecode"].ToString());
                                            hat.Add("columnname", dcolumn);
                                            hat.Add("roll_no", str_rollno);
                                            hat.Add("month_year", strdate);
                                            hat.Add("values", Att_value1);
                                            strquery = "sp_ins_upd_student_attendance";
                                            insert = da.insert_method(strquery, hat, "sp");
                                        }
                                    }
                                    isSchoolAttendance = false;
                                    isSchoolAttendance = CheckSchoolOrCollege(clgcode);
                                    if (isSchoolAttendance)
                                    {
                                        attendanceMark(AppNo, (int)strdate, sb_aattddaayy, Convert.ToInt32(noofhours_save), Convert.ToInt32(no_firsthalf), Convert.ToInt32(no_secondhalf), Convert.ToInt32(no_minpresent_firsthalf), Convert.ToInt32(no_minpresent_secondhalf), (split[1] + "/" + split[0] + "/" + split[2]), clgcode);// Session["collegecode"].ToString());
                                    }
                                    savefalg = true;
                                    getroll = str_rollno.ToString();
                                    if (hatattendance.Contains(Att_value1.ToString()))
                                    {
                                        string att = GetCorrespondingKey(Att_value1.ToString(), hatattendance).ToString();
                                        if (att.Trim() == "1")
                                        {
                                            if (minimumabsentsms.Trim() != "" && minimumabsentsms != null && minimumabsentsms.Trim() != "0")
                                            {
                                                Boolean abshrs = true;
                                                string[] curedate = FpSpread2.Sheets[0].ColumnHeader.Cells[0, att_col].Text.Split('-');
                                                DateTime dtstart = Convert.ToDateTime(curedate[1] + '/' + curedate[0] + '/' + curedate[2]);
                                                string strgetval = "select r.degree_code,r.Current_Semester,No_of_hrs_per_day,p.min_pres_I_half_day,no_of_hrs_II_half_day,p.min_pres_I_half_day,p.min_pres_II_half_day,s.start_date from seminfo s,PeriodAttndSchedule p,Registration r where p.degree_code=s.degree_code and r.degree_code=p.degree_code and p.semester=r.Current_Semester and p.semester=s.semester and r.Batch_Year=s.batch_year  and r.Roll_No='" + str_rollno + "'";
                                                DataSet dsgetval = da.select_method_wo_parameter(strgetval, "Text");
                                                if (dsgetval.Tables.Count > 0 && dsgetval.Tables[0].Rows.Count > 0)
                                                {
                                                    string degree_code = dsgetval.Tables[0].Rows[0]["degree_code"].ToString();
                                                    string semester = dsgetval.Tables[0].Rows[0]["Current_Semester"].ToString();
                                                    string noofhrs = dsgetval.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
                                                    string fhrs = dsgetval.Tables[0].Rows[0]["min_pres_I_half_day"].ToString();
                                                    string shrs = dsgetval.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString();
                                                    string minfhpre = dsgetval.Tables[0].Rows[0]["min_pres_I_half_day"].ToString();
                                                    string minshpre = dsgetval.Tables[0].Rows[0]["min_pres_II_half_day"].ToString();
                                                    DateTime st = Convert.ToDateTime(dsgetval.Tables[0].Rows[0]["start_date"].ToString());
                                                    string holidayquery = "select CONVERT(nvarchar(15),holiday_date,101) as holidate,halforfull,morning,evening,holiday_desc from holidayStudents where degree_code='" + degree_code + "' and semester='" + semester + "' and holiday_date between '" + st.ToString() + "' and '" + dtstart.ToString() + "'";
                                                    DataSet dsholiday = da.select_method_wo_parameter(holidayquery, "Text");
                                                    Dictionary<DateTime, string> dtholid = new Dictionary<DateTime, string>();
                                                    if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count > 0)
                                                    {
                                                        for (int hd = 0; hd < dsholiday.Tables[0].Rows.Count; hd++)
                                                        {
                                                            DateTime holday = Convert.ToDateTime(dsholiday.Tables[0].Rows[0]["holidate"].ToString());
                                                            string halorfulvalue = dsholiday.Tables[0].Rows[0]["halforfull"].ToString() + '/' + dsholiday.Tables[0].Rows[0]["morning"].ToString() + '/' + dsholiday.Tables[0].Rows[0]["evening"].ToString();
                                                            if (!dtholid.ContainsKey(holday))
                                                            {
                                                                dtholid.Add(holday, halorfulvalue);
                                                            }
                                                        }
                                                    }
                                                    int endday = int.Parse(minimumabsentsms);
                                                    int totpre = 0, totabs = 0, totcount = 0, tothrs = 0;
                                                    string periodval = string.Empty;
                                                    for (int std = 0; std < endday; std++)
                                                    {
                                                        periodval = string.Empty;
                                                        DateTime dtcheda = dtstart.AddDays(-std);
                                                        if (!dtholid.ContainsKey(dtcheda))
                                                        {
                                                            string[] spdate = dtcheda.ToString().Split(' ');
                                                            string str_Date1 = spdate[0].ToString();
                                                            string[] split1 = str_Date1.Split(new Char[] { '/' });
                                                            string str_day1 = (Convert.ToInt16(split1[1].ToString())).ToString();
                                                            string Atmonth1 = (Convert.ToInt16(split1[0].ToString())).ToString();
                                                            string Atyear1 = split1[2].ToString();
                                                            int getmotnyear = (Convert.ToInt32(Atmonth1) + Convert.ToInt32(Atyear1) * 12);
                                                            for (int abhr = 1; abhr <= int.Parse(noofhrs); abhr++)
                                                            {
                                                                if (periodval == "")
                                                                {
                                                                    periodval = "d" + str_day1 + "d" + abhr + "";
                                                                }
                                                                else
                                                                {
                                                                    periodval = periodval + ',' + "d" + str_day1 + "d" + abhr + "";
                                                                }
                                                            }
                                                            tothrs = 0;
                                                            strgetval = "select " + periodval + " from attendance where roll_no='" + str_rollno + "' and month_year='" + getmotnyear + "'";
                                                            dsgetval = da.select_method_wo_parameter(strgetval, "Text");
                                                            if (dsgetval.Tables.Count > 0 && dsgetval.Tables[0].Rows.Count > 0)
                                                            {
                                                                int valhrs = 1;
                                                                for (valhrs = 1; valhrs < int.Parse(fhrs) + 1; valhrs++)
                                                                {
                                                                    string atthrval = dsgetval.Tables[0].Rows[0][valhrs - 1].ToString();
                                                                    if (atthrval.Trim() != "" && atthrval != null)
                                                                    {
                                                                        if (hatattendance.Contains(atthrval))
                                                                        {
                                                                            tothrs++;
                                                                            string absorpre = GetCorrespondingKey(atthrval.ToString(), hatattendance).ToString();
                                                                            if (absorpre == "0")
                                                                            {
                                                                                totpre++;
                                                                            }
                                                                            if (absorpre == "1")
                                                                            {
                                                                                totabs++;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (tothrs >= int.Parse(minfhpre) && totpre < int.Parse(minfhpre) && totabs > 0)
                                                                {
                                                                    abshrs = false;
                                                                    valhrs--;
                                                                }
                                                                else
                                                                {
                                                                    abshrs = true;
                                                                    std = endday + 20;
                                                                    valhrs = int.Parse(noofhrs) * 10;
                                                                }
                                                                totpre = 0; totabs = 0; tothrs = 0;
                                                                if (abshrs == false)
                                                                {
                                                                    for (valhrs = valhrs + 1; valhrs <= int.Parse(noofhrs); valhrs++)
                                                                    {
                                                                        string atthrval = dsgetval.Tables[0].Rows[0][valhrs - 1].ToString();
                                                                        if (atthrval.Trim() != "" && atthrval != null)
                                                                        {
                                                                            if (hatattendance.Contains(atthrval))
                                                                            {
                                                                                tothrs++;
                                                                                string absorpre = GetCorrespondingKey(atthrval.ToString(), hatattendance).ToString();
                                                                                if (absorpre == "0")
                                                                                {
                                                                                    totpre++;
                                                                                }
                                                                                if (absorpre == "1")
                                                                                {
                                                                                    totabs++;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    if (tothrs >= int.Parse(minshpre) && totpre < int.Parse(minshpre) && totabs > 0)
                                                                    {
                                                                        abshrs = false;
                                                                    }
                                                                    else
                                                                    {
                                                                        abshrs = true;
                                                                        std = endday + 20;
                                                                        valhrs = int.Parse(noofhrs) * 10;
                                                                    }
                                                                    totpre = 0; totabs = 0; tothrs = 0;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            totpre = 0; totabs = 0; tothrs = 0;
                                                            string[] get = dtholid[dtcheda].Split('/');
                                                            string fulldau = get[0].ToString();
                                                            string morin = get[1].ToString();
                                                            string eneveing = get[2].ToString();
                                                            if (fulldau == "1")
                                                            {
                                                                endday++;
                                                            }
                                                            else
                                                            {
                                                                string[] spdate = dtcheda.ToString().Split(' ');
                                                                string str_Date1 = spdate[0].ToString();
                                                                string[] split1 = str_Date1.Split(new Char[] { '/' });
                                                                string str_day1 = (Convert.ToInt16(split1[1].ToString())).ToString();
                                                                string Atmonth1 = (Convert.ToInt16(split1[0].ToString())).ToString();
                                                                string Atyear1 = split1[2].ToString();
                                                                int getmotnyear = (Convert.ToInt32(Atmonth1) + Convert.ToInt32(Atyear1) * 12);
                                                                int finalhrs = 0;
                                                                int minprehours = 0;
                                                                int abhr1 = 1;
                                                                int minprehrs = 0;
                                                                if (morin == "1" || morin.Trim().ToLower() == "true")
                                                                {
                                                                    finalhrs = int.Parse(fhrs);
                                                                    abhr1 = 1;
                                                                    minprehrs = int.Parse(minfhpre);
                                                                }
                                                                if (morin == "1" || morin.Trim().ToLower() == "true")
                                                                {
                                                                    finalhrs = int.Parse(noofhrs);
                                                                    abhr1 = int.Parse(fhrs) + 1;
                                                                    minprehrs = int.Parse(minshpre);
                                                                }
                                                                for (int abhr = abhr1; abhr <= finalhrs; abhr++)
                                                                {
                                                                    if (periodval == "")
                                                                    {
                                                                        periodval = "d" + str_day1 + "d" + abhr + "";
                                                                    }
                                                                    else
                                                                    {
                                                                        periodval = periodval + ',' + "d" + str_day1 + "d" + abhr + "";
                                                                    }
                                                                }
                                                                strgetval = "select " + periodval + " from attendance where roll_no='" + str_rollno + "' and month_year='" + getmotnyear + "'";
                                                                dsgetval = da.select_method_wo_parameter(strgetval, "Text");
                                                                if (dsgetval.Tables.Count > 0 && dsgetval.Tables[0].Rows.Count > 0)
                                                                {
                                                                    for (int valhrs = abhr1; valhrs < int.Parse(fhrs) + 1; valhrs++)
                                                                    {
                                                                        string atthrval = dsgetval.Tables[0].Rows[0][valhrs - 1].ToString();
                                                                        if (atthrval.Trim() != "" && atthrval != null)
                                                                        {
                                                                            if (hatattendance.Contains(atthrval))
                                                                            {
                                                                                tothrs++;
                                                                                string absorpre = GetCorrespondingKey(atthrval.ToString(), hatattendance).ToString();
                                                                                if (absorpre == "0")
                                                                                {
                                                                                    totpre++;
                                                                                }
                                                                                if (absorpre == "1")
                                                                                {
                                                                                    totabs++;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    if (tothrs >= int.Parse(minshpre) && totpre < int.Parse(minshpre) && totabs > 0)
                                                                    {
                                                                        abshrs = false;
                                                                    }
                                                                    else
                                                                    {
                                                                        abshrs = true;
                                                                        std = endday + 20;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    abshrs = true;
                                                                    std = endday + 20;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (abshrs == false)
                                                    {
                                                        AttDate = str_Date.Trim();
                                                        AttHour = str_hour;
                                                        if (AttHour != "")
                                                        {
                                                            string value_return = web.coundected_hour(strdate, startsem_date, str_rollno, absent_calcflag, notarray);
                                                            if (value_return == "Empty")
                                                            {
                                                                total_conduct_hour = 1;
                                                                absent_hour = 1;
                                                            }
                                                            else
                                                            {
                                                                string[] splitvalue = value_return.Split('-');
                                                                if (splitvalue.Length > 0)
                                                                {
                                                                    if (splitvalue[0].ToString() != "")
                                                                    {
                                                                        total_conduct_hour = Convert.ToInt32(splitvalue[0]);
                                                                        total_conduct_hour++;
                                                                    }
                                                                    else
                                                                    {
                                                                        total_conduct_hour++;
                                                                    }
                                                                    if (splitvalue[1].ToString() != "")
                                                                    {
                                                                        absent_hour = Convert.ToInt32(splitvalue[1]);
                                                                        absent_hour++;
                                                                    }
                                                                    else
                                                                    {
                                                                        absent_hour++;
                                                                    }
                                                                }
                                                            }
                                                            SendingSms(str_rollno.ToString(), AppNo, regno, admno, AttDate, AttHour, degree_code, total_conduct_hour, absent_hour);
                                                            sendvoicecall(str_rollno.ToString(), AttDate, AttHour, batch, degree_code);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    AttDate = str_Date.Trim();
                                                    AttHour = str_hour;
                                                    if (AttHour != "")
                                                    {
                                                        string value_return = web.coundected_hour(strdate, startsem_date, str_rollno, absent_calcflag, notarray);
                                                        if (value_return == "Empty")
                                                        {
                                                            total_conduct_hour = 1;
                                                            absent_hour = 1;
                                                        }
                                                        else
                                                        {
                                                            string[] splitvalue = value_return.Split('-');
                                                            if (splitvalue.Length > 0)
                                                            {
                                                                if (splitvalue[0].ToString() != "")
                                                                {
                                                                    total_conduct_hour = Convert.ToInt32(splitvalue[0]);
                                                                    total_conduct_hour++;
                                                                }
                                                                else
                                                                {
                                                                    total_conduct_hour++;
                                                                }
                                                                if (splitvalue[1].ToString() != "")
                                                                {
                                                                    absent_hour = Convert.ToInt32(splitvalue[1]);
                                                                    absent_hour++;
                                                                }
                                                                else
                                                                {
                                                                    absent_hour++;
                                                                }
                                                            }
                                                        }
                                                        SendingSms(str_rollno.ToString(), AppNo, regno, admno, AttDate, AttHour, degree_code, total_conduct_hour, absent_hour);
                                                        sendvoicecall(str_rollno.ToString(), AttDate, AttHour, batch, degree_code);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                AttDate = str_Date.Trim();
                                                AttHour = str_hour;
                                                if (AttHour != "")
                                                {
                                                    string value_return = web.coundected_hour(strdate, startsem_date, str_rollno, absent_calcflag, notarray);
                                                    if (value_return == "Empty")
                                                    {
                                                        total_conduct_hour = 1;
                                                        absent_hour = 1;
                                                    }
                                                    else
                                                    {
                                                        string[] splitvalue = value_return.Split('-');
                                                        if (splitvalue.Length > 0)
                                                        {
                                                            if (splitvalue[0].ToString() != "")
                                                            {
                                                                total_conduct_hour = Convert.ToInt32(splitvalue[0]);
                                                                total_conduct_hour++;
                                                            }
                                                            else
                                                            {
                                                                total_conduct_hour++;
                                                            }
                                                            if (splitvalue[1].ToString() != "")
                                                            {
                                                                absent_hour = Convert.ToInt32(splitvalue[1]);
                                                                absent_hour++;
                                                            }
                                                            else
                                                            {
                                                                absent_hour++;
                                                            }
                                                        }
                                                    }
                                                    SendingSms(str_rollno.ToString(), AppNo, regno, admno, AttDate, AttHour, degree_code, total_conduct_hour, absent_hour);
                                                    sendvoicecall(str_rollno.ToString(), AttDate, AttHour, batch, degree_code);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        hr = (int.Parse(hr) + 1).ToString();
                    }
                }
                ////Set Color
                string checkalter = string.Empty;
                for (int att_col = 7; att_col <= FpSpread2.Sheets[0].ColumnCount - 2; att_col = att_col + 2)
                {
                    hr = FpSpread2.Sheets[0].ColumnHeader.Cells[1, att_col].Tag.ToString();
                    str_Date = FpSpread2.Sheets[0].ColumnHeader.Cells[0, att_col].Text;
                    string[] split = str_Date.Split(new Char[] { '-' });
                    str_day = (Convert.ToInt16(split[0].ToString())).ToString();
                    Atmonth = (Convert.ToInt16(split[1].ToString())).ToString();
                    Atyear = split[2].ToString();
                    strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                    str_hour = Convert.ToString(hr);
                    dcolumn = "d" + str_day + "d" + str_hour;
                    DateTime date = Convert.ToDateTime(split[1].ToString() + '-' + split[0].ToString() + '-' + split[2].ToString());
                    string day = date.ToString("ddd");
                    string[] spilttext = getcelltag.Split('*');
                    for (int j = 0; j <= spilttext.GetUpperBound(0); j++)
                    {
                        Boolean colorflag = false;
                        if (j > 0)
                        {
                            if (FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor == Color.Blue)
                            {
                                colorflag = true;
                            }
                            else
                            {
                                colorflag = false;
                            }
                        }
                        if (colorflag == false)
                        {
                            string check_lab = string.Empty;
                            dailyentryflag = false;
                            attendanceentryflag = false;
                            split_tag_val = spilttext[j].Split('-');
                            if (split_tag_val.GetUpperBound(0) >= 7)
                            {
                                batch = split_tag_val[4].ToString();
                                degree_code = split_tag_val[0].ToString();
                                semester = split_tag_val[1].ToString();
                                subject_no = split_tag_val[2].ToString();
                                //section = "and Registration.Sections='" + split_tag_val[3].ToString() + "'";
                                section = split_tag_val[3].ToString();
                                checkalter = split_tag_val[7].ToString();
                                check_lab = split_tag_val[5].ToString();
                            }
                            else
                            {
                                batch = split_tag_val[3].ToString();
                                degree_code = split_tag_val[0].ToString();
                                semester = split_tag_val[1].ToString();
                                subject_no = split_tag_val[2].ToString();
                                section = string.Empty;
                                checkalter = split_tag_val[6].ToString();
                                check_lab = split_tag_val[4].ToString();
                            }
                            //string check_lab = da.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subject_no + "'");
                            string sectionvar = string.Empty;
                            if (section.Trim() != "" && section != null && section != "-1")
                            {
                                sectionvar = " and sections='" + section + "'";
                            }
                            if (check_lab == "L" || check_lab.Trim().ToLower() == "l")
                            {
                                strquery = "  select p.schOrder,p.nodays,Convert(nvarchar(15),s.start_date,23) as start,s.starting_dayorder from PeriodAttndSchedule p, seminfo s where s.degree_code=p.degree_code and s.semester=p.semester and s.degree_code=" + degree_code + " and s.semester=" + semester + " AND s.batch_year=" + batch + "";
                                Day_Order = "0";
                                dsattendance = da.select_method(strquery, hat, "Text");
                                if (dsattendance.Tables.Count > 0 && dsattendance.Tables[0].Rows.Count > 0)
                                {
                                    Day_Order = dsattendance.Tables[0].Rows[0]["schOrder"].ToString();
                                    noofdays = dsattendance.Tables[0].Rows[0]["nodays"].ToString();
                                    start_datesem = dsattendance.Tables[0].Rows[0]["start"].ToString();
                                    start_dayorder = dsattendance.Tables[0].Rows[0]["starting_dayorder"].ToString();
                                }
                                //Week / Day order
                                if (Day_Order == "1")
                                {
                                    day = date.ToString("ddd");
                                }
                                else
                                {
                                    day = da.findday(date.ToString(), degree_code, semester, batch, start_datesem.ToString(), noofdays.ToString(), start_dayorder);//Modifeied By Srianth add comman Daccess 5/9/2014
                                }
                                if (checkalter.ToLower().Trim() == "alter")
                                {
                                    hat.Clear();
                                    hat.Add("batch_year", batch);
                                    hat.Add("degree_code", degree_code);
                                    hat.Add("sem", semester);
                                    hat.Add("sections", section);
                                    hat.Add("month_year", strdate);
                                    hat.Add("date", date);
                                    hat.Add("subject_no", subject_no);
                                    hat.Add("day", day);
                                    hat.Add("hour", str_hour);
                                    ds.Reset();
                                    ds.Dispose();
                                    ds = da.select_method("sp_stu_atten_month_check_lab_alter", hat, "sp");
                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                    {
                                        Att_strqueryst = ds.Tables[0].Rows[0]["stucount"].ToString();
                                        if (int.Parse(Att_strqueryst) > 0)
                                        {
                                            hat.Clear();
                                            //hat.Add("columnname", dcolumn);
                                            //hat.Add("batch_year", batch);
                                            //hat.Add("degree_code", degree_code);
                                            //hat.Add("sem", semester);
                                            //hat.Add("sections", section);
                                            //hat.Add("month_year", strdate);
                                            //hat.Add("date", date);
                                            //hat.Add("subject_no", subject_no);
                                            //hat.Add("day", day);
                                            //hat.Add("hour", str_hour);
                                            ds.Reset();
                                            ds.Dispose();
                                            string strgetatt = "select count(distinct r.Roll_No) as stucount from registration r,attendance a,subjectchooser_new s where degree_code='" + degree_code + "' and current_semester='" + semester + "' and batch_year='" + batch + "' and cc=0 and delflag=0 and  exam_flag<>'debar' and month_year=" + strdate + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and";
                                            strgetatt = strgetatt + " r.current_semester=s.semester and subject_no='" + subject_no + "' " + sectionvar + " and(" + dcolumn + " is not null and " + dcolumn + "<>'0' and " + dcolumn + "<>'') and fromdate='" + date + "' and batch    in(select stu_batch from laballoc_new where subject_no='" + subject_no + "'  and batch_year='" + batch + "'  and hour_value='" + str_hour + "'  and    degree_code='" + degree_code + "' ";
                                            strgetatt = strgetatt + " and day_value='" + day + "' and semester='" + semester + "' " + sectionvar + " and fdate='" + date + "') and adm_date<='" + date + "'";
                                            ds = da.select_method_wo_parameter(strgetatt, "Text");
                                            //ds = da.select_method("sp_stu_atten_day_check_lab_alter", hat, "sp");
                                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                            {
                                                if (Att_strqueryst == ds.Tables[0].Rows[0]["stucount"].ToString())
                                                {
                                                    Att_strqueryst = "0";
                                                }
                                                else
                                                {
                                                    Att_strqueryst = "1";
                                                }
                                            }
                                            else
                                            {
                                                Att_strqueryst = "1";
                                            }
                                        }
                                        else
                                        {
                                            Att_strqueryst = "1";
                                        }
                                    }
                                    else
                                    {
                                        Att_strqueryst = "1";
                                    }
                                }
                                else
                                {
                                    string timetable = da.GetFunction("select top 1 TTName,  FromDate from  Semester_Schedule where degree_code='" + degree_code + "' and semester='" + semester + "' and batch_year='" + batch + "' " + sectionvar + " and FromDate<='" + date.ToString("MM/dd/yyyy") + "' order by FromDate Desc");
                                    hat.Clear();
                                    //hat.Add("batch_year", batch);
                                    //hat.Add("degree_code", degree_code);
                                    //hat.Add("sem", semester);
                                    //hat.Add("sections", section);
                                    //hat.Add("month_year", strdate);
                                    //hat.Add("date", date);
                                    //hat.Add("subject_no", subject_no);
                                    //hat.Add("day", day);
                                    //hat.Add("hour", str_hour);
                                    //hat.Add("ttmane", timetable);
                                    //hat.Add("staff_code", staff_code);
                                    string strstt = "select count(distinct r.Roll_No) as stucount from registration r,subjectchooser s where degree_code='" + degree_code + "' and ";
                                    strstt = strstt + " current_semester='" + semester + "' and batch_year='" + batch.ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar' and r.roll_no=s.roll_no ";
                                    strstt = strstt + " and r.current_semester=s.semester and subject_no='" + subject_no + "'  " + sectionvar + "  and batch in(select stu_batch from ";
                                    strstt = strstt + " laballoc where subject_no='" + subject_no + "'  and batch_year='" + batch.ToString() + "'  and hour_value='" + str_hour + "' and degree_code='" + degree_code + "' ";
                                    strstt = strstt + " and day_value='" + day + "' and semester='" + semester + "'  " + sectionvar + " and Timetablename='" + timetable + "') and adm_date<='" + date.ToString("MM/dd/yyyy") + "'  " + strstaffselector + "";
                                    ds.Reset();
                                    ds.Dispose();
                                    //ds = da.select_method("sp_stu_atten_month_check_lab", hat, "sp");
                                    ds = da.select_method_wo_parameter(strstt, "Text");
                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                    {
                                        Att_strqueryst = ds.Tables[0].Rows[0]["stucount"].ToString();
                                        if (int.Parse(Att_strqueryst) > 0)
                                        {
                                            hat.Clear();
                                            //hat.Add("columnname", dcolumn);
                                            //hat.Add("batch_year", batch);
                                            //hat.Add("degree_code", degree_code);
                                            //hat.Add("sem", semester);
                                            //hat.Add("sections", section);
                                            //hat.Add("month_year", strdate);
                                            //hat.Add("date", date);
                                            //hat.Add("subject_no", subject_no);
                                            //hat.Add("day", day);
                                            //hat.Add("hour", str_hour);
                                            //hat.Add("ttmane", timetable);
                                            ds.Reset();
                                            ds.Dispose();
                                            //ds = da.select_method("sp_stu_atten_day_check_lab", hat, "sp");
                                            string strgetatt = "select count( r.Roll_No) as stucount from registration r,attendance a,subjectchooser s where degree_code='" + degree_code + "' and current_semester='" + semester + "' and batch_year='" + batch + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year=" + strdate + "";
                                            strgetatt = strgetatt + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and r.current_semester=s.semester and subject_no='" + subject_no + "' " + sectionvar + " and(" + dcolumn + " is not null and " + dcolumn + "<>'0' and " + dcolumn + "<>'') and batch in(select stu_batch from laballoc ";
                                            strgetatt = strgetatt + " where subject_no='" + subject_no + "' and Timetablename='" + timetable + "' and batch_year='" + batch + "'  and hour_value='" + str_hour + "'  and    degree_code='" + degree_code + "' and day_value='" + day + "' and semester='" + semester + "' " + sectionvar + ") and adm_date<='" + date + "' " + strstaffselector + "";
                                            ds = da.select_method_wo_parameter(strgetatt, "Text");
                                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                            {
                                                if (Att_strqueryst == ds.Tables[0].Rows[0]["stucount"].ToString())
                                                {
                                                    Att_strqueryst = "0";
                                                }
                                                else
                                                {
                                                    Att_strqueryst = "1";
                                                }
                                            }
                                            else
                                            {
                                                Att_strqueryst = "1";
                                            }
                                        }
                                        else
                                        {
                                            Att_strqueryst = "1";
                                        }
                                    }
                                    else
                                    {
                                        Att_strqueryst = "1";
                                    }
                                }
                            }
                            else
                            {
                                hat.Clear();
                                //hat.Add("batch_year", batch);
                                //hat.Add("degree_code", degree_code);
                                //hat.Add("sem", semester);
                                //hat.Add("sections", section);
                                //hat.Add("month_year", strdate);
                                //hat.Add("date", date);
                                //hat.Add("subject_no", subject_no);
                                //hat.Add("staff_code", staff_code);
                                //ds.Reset();
                                //ds.Dispose();
                                //ds = da.select_method("sp_stu_atten_month_check", hat, "sp");
                                string strgetatt1 = "select count(distinct r.Roll_No) as stucount from registration r,subjectchooser s where  r.roll_no=s.roll_no and ";
                                strgetatt1 = strgetatt1 + " r.current_semester=s.semester and batch_year='" + batch + "' and degree_code='" + degree_code + "'  and current_semester='" + semester + "' " + sectionvar + " ";
                                strgetatt1 = strgetatt1 + "  and subject_no='" + subject_no + "'  and adm_date<='" + date + "' and cc=0 and delflag=0 and exam_flag<>'debar'  " + strstaffselector + "";
                                ds = da.select_method_wo_parameter(strgetatt1, "Text");
                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    Att_strqueryst = ds.Tables[0].Rows[0]["stucount"].ToString();
                                    if (int.Parse(Att_strqueryst) > 0)
                                    {
                                        hat.Clear();
                                        hat.Add("columnname", dcolumn);
                                        hat.Add("batch_year", batch);
                                        hat.Add("degree_code", degree_code);
                                        hat.Add("sem", semester);
                                        hat.Add("sections", section);
                                        hat.Add("month_year", strdate);
                                        hat.Add("date", date);
                                        hat.Add("subject_no", subject_no);
                                        ds.Reset();
                                        ds.Dispose();
                                        string strgetatt = "select count(registration.roll_no) as stucount  from registration,attendance,subjectchooser s where degree_code='" + degree_code + "' and current_semester='" + semester + "' and batch_year='" + batch + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and  registration.roll_no=s.roll_no ";
                                        strgetatt = strgetatt + " and registration.current_semester=s.semester and subject_no='" + subject_no + "' " + sectionvar + "";
                                        strgetatt = strgetatt + " and (" + dcolumn + " is not null and " + dcolumn + "<>'0' and " + dcolumn + "<>'') and adm_date<='" + date + "'  " + strstaffselector + "";
                                        ds = da.select_method_wo_parameter(strgetatt, "Text");
                                        //    ds = da.select_method("sp_stu_atten_day_check", hat, "sp");
                                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        {
                                            if (Att_strqueryst == ds.Tables[0].Rows[0]["stucount"].ToString())
                                            {
                                                Att_strqueryst = "0";
                                            }
                                            else
                                            {
                                                Att_strqueryst = "1";
                                            }
                                        }
                                        else
                                        {
                                            Att_strqueryst = "1";
                                        }
                                    }
                                    else
                                    {
                                        Att_strqueryst = "1";
                                    }
                                }
                                else
                                {
                                    Att_strqueryst = "1";
                                }
                            }
                            if (int.Parse(Att_strqueryst) > 0)
                            {
                                //FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.Blue;
                                attendanceentryflag = false;
                            }
                            else
                            {
                                attendanceentryflag = true;
                                //FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.ForestGreen;
                            }
                            if (section.Trim() == "" || section == null || section == "-1")
                            {
                                section = string.Empty;
                            }
                            else
                            {
                                section = " and Sections='" + section + "'";
                            }
                            strquerytext = "select de.lp_code from dailyentdet de,dailystaffentry ds where de.lp_code=ds.lp_code  and batch_year=" + batch + " and degree_code='" + degree_code + "' and semester=" + semester + " " + section + " and subject_no='" + subject_no + "' and  staff_code='" + staff_code + "' and sch_date='" + date + "' and hr=" + hr + "";
                            ds.Reset();
                            ds.Dispose();
                            ds = da.select_method(strquerytext, hat, "Text");
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                dailyentryflag = true;
                            }
                            if (dailyentryflag == false && attendanceentryflag == false)
                            {
                                FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.Blue;
                                j = spilttext.GetUpperBound(0) + 1;
                            }
                            else if (dailyentryflag == true && attendanceentryflag == false)
                            {
                                if (FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor == Color.DarkOrchid)
                                {
                                    FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.Blue;
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.DarkTurquoise;
                                }
                            }
                            else if (dailyentryflag == false && attendanceentryflag == true)
                            {
                                if (FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor == Color.DarkTurquoise)
                                {
                                    FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.Blue;
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.DarkOrchid;
                                }
                            }
                            else
                            {
                                if (j == 0)
                                {
                                    FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.ForestGreen;
                                }
                                else
                                {
                                    if (FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor == Color.ForestGreen)
                                    {
                                        FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.ForestGreen;
                                    }
                                }
                            }
                        }
                    }
                }
                if (Buttonsave.Enabled == true)
                {
                    savevalue = 1;
                }
                else
                {
                    savevalue = 2;
                }
                if (nullflag == true)
                {
                    lbl_alert.Visible = false;
                    Buttonupdate.Enabled = true;
                    Buttonsave.Enabled = false;
                }
                else
                {
                    Buttonupdate.Enabled = false;
                    Buttonsave.Enabled = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Mark Attendance For Save ";
                }
                if (savefalg == true)
                {
                    load_attendance();
                    string entrycode = string.Empty;
                    if (Session["Entry_Code"] != null)
                    {
                        entrycode = Session["Entry_Code"].ToString();
                    }
                    hat.Clear();
                    string formname = "Student Attendance";
                    string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                    string doa = DateTime.Now.ToString("MM/dd/yyy");
                    string details = string.Empty;
                    if (getroll != "")
                    {
                        DataSet dsroll = da.select_method("Select Batch_year,Degree_code,Current_semester,Sections from registration where roll_no='" + getroll + "'", hat, "Text");
                        if (dsroll.Tables.Count > 0 && dsroll.Tables[0].Rows.Count > 0)
                        {
                            details = "" + dsroll.Tables[0].Rows[0]["Degree_code"].ToString() + ": Sem - " + dsroll.Tables[0].Rows[0]["Current_semester"].ToString() + ": Batch Year - " + dsroll.Tables[0].Rows[0]["Batch_year"].ToString();
                            if (dsroll.Tables[0].Rows[0]["Sections"].ToString() != "" && dsroll.Tables[0].Rows[0]["Sections"].ToString() != "0" && dsroll.Tables[0].Rows[0]["Sections"].ToString() != null)
                            {
                                details = details + ": Sections - " + dsroll.Tables[0].Rows[0]["Sections"].ToString();
                            }
                        }
                    }
                    string modules = "0";
                    string act_diff = " ";
                    string ctsname = "Update the Attendance Entry Details";
                    if (savevalue == 1)
                    {
                        ctsname = "Save the Attendance Entry Details";
                    }
                    string strlogdetails = "insert into UserLog (Entry_Code,Form_Name,UsrAction,TOA,DOA,Details,Module,Act_Diff,ctrNam) values ('" + entrycode + "','" + formname + "','" + savevalue + "','" + toa + "','" + doa + "','" + details + "','" + modules + "','" + act_diff + "','" + ctsname + "')";
                    int a = da.update_method_wo_parameter(strlogdetails, "Text");
                    //Present Absent Count Details
                    for (Att_mark_column = 7; Att_mark_column < FpSpread2.Sheets[0].ColumnCount; Att_mark_column++)
                    {
                        absent_count = 0;
                        present_count = 0;
                        for (Att_mark_row = 1; Att_mark_row < FpSpread2.Sheets[0].RowCount - 2; Att_mark_row++)
                        {
                            if (FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text.ToString() != "")
                            {
                                string attmaktext = FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text.ToString();
                                string attval = Attvalues(attmaktext);
                                FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Note = attval;
                                if (present_calcflag.ContainsKey(FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Note.ToString()))
                                {
                                    present_count++;
                                }
                                if (absent_calcflag.ContainsKey(FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Note.ToString()))
                                {
                                    absent_count++;
                                }
                            }
                        }
                        FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 2), Att_mark_column].Text = present_count.ToString();
                        FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 1), Att_mark_column].Text = absent_count.ToString();
                        Att_mark_column++;
                    }
                    if (strinvalidroll == "")
                    {
                        Buttonsavelesson.Enabled = true;
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
                        lblAlertMsg.Text = "Saved successfully";
                        divPopAlert.Visible = true;
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully ......But Following Roll No Are Invalid : " + strinvalidroll + "')", true);
                        lblAlertMsg.Text = "Saved successfully ......But Following Roll No Are Invalid : " + strinvalidroll + "";
                        divPopAlert.Visible = true;
                    }
                }
            }
            divConfirmBox.Visible = false;
        }
        catch
        {
        }
    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
        try
        {
            divConfirmBox.Visible = false;
        }
        catch
        {
        }
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

}