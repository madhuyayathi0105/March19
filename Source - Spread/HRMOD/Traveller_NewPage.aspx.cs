using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Drawing;
public partial class Default6 : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlCommand cmd = new SqlCommand();
    static Hashtable Has_Stage = new Hashtable();
    public void Connection()
    {
        con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
        con.Open();
    }
    DataSet dsprint = new DataSet();
    DAccess2 dacces2 = new DAccess2();
    Hashtable htmnth = new Hashtable();
    DataSet ds;
    DataSet dss;
    Hashtable hat = new Hashtable();
    SqlDataAdapter danew;
    DataSet d1 = new DataSet();
    DataSet d2 = new DataSet();
    DataSet d3 = new DataSet();
    DataSet ds2 = new DataSet1();
    Hashtable hastab = new Hashtable();
    Hashtable ht = new Hashtable();
    static Hashtable spr_hash = new Hashtable();
    static Hashtable priority_hash = new Hashtable();
    static int inc = 0, inc1 = 0;
    static int chkflag = 0;
    string usercode = "", singleuser = "", group_user = "";
    string collegecode = "";
    Boolean cellclick = false;
    string branch = "", univ = "", course = "", tvalue = "", sql = "";
    string pcourse = "";
    string pdegree = "";
    string pcol = "";
    string testno = "";
    string testdate = "";
    string testcentre = "";
    int loop = 0;
    string test_detail = "", eval = "";
    string religion = "0", caste = "0";
    string blood = "0", region = "0", FatherQuali = "0", FatherIncome = "0", MotherQuali = "0", MotherIncome = "0", quota = "0";
    string comm = "0", nation = "0", mton = "0", foccu = "0", moccu = "0", statec = "0", statep = "0", mbl = "0", seattype = "0", stateg = "0";
    string medium = "0";
    string sex;
    string activity = "0", enquiry = "0", talukp = "0", talukc = "0", talukg = "0";
    string name, phn, email, amount, adres, agent, city, district;
    string refered = "", dir;
    string code = "";
    int row_mark;
    int count_mark;
    string passyear;
    string getmark_no, getmark, getsubno, getmin, getmax, result;
    string final_mark = "", mode, sem;
    Boolean Cellclick = false;
    static int priority_count = 0;
    static int cbDate = 0;
    string CollegeCode;
    static string[] ss;
    static string p = "";
    static string[] ss1;
    string ss2 = "";
    Boolean flag_true;
    Boolean fpcellclick = false; Boolean fpcolclick = false;
    FarPoint.Web.Spread.ComboBoxCellType cf = new FarPoint.Web.Spread.ComboBoxCellType();
    int rowvalue, tempvalue, cvalue;
    string caption = "", Fee_Code = "", fee_amt = "", semval = "", Cost = "", BatchFee = "", Roll_Adm = "";
    string header_id;
    string PreviousFee = "";
    string allotDate = "";
    string month = "";
    string month_amt = "";
    string sqlcmd = "", enqno = ""; string tcode = "";
    ArrayList keyarray = new ArrayList();
    ArrayList valuearray = new ArrayList();
    Hashtable loadhas = new Hashtable();
    DataSet dsload = new DataSet(); static int chk = 0;
    Boolean Cellclick1 = false;
    static int batchcnt = 0;
    static int degreecnt = 0;
    static int branchcnt = 0;
    string examcodeval = string.Empty;
    string strgrade = string.Empty;
    string strsec = string.Empty;
    string strsection = string.Empty;
    string strsection1 = string.Empty;
    string strsection2 = string.Empty;
    string sturollno = string.Empty;
    string strsubcrd = string.Empty;
    string graders = string.Empty;
    string sqlstr = string.Empty;
    string course_id = string.Empty;
    //string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    //string strbranch = string.Empty;
    string strbranchname = string.Empty;
    string strsem = string.Empty;
    string syllcode = string.Empty;
    string staff_name = string.Empty;
    string strbatchsplit = string.Empty;
    string strbranchsplit = string.Empty;
    string strsecsplit = string.Empty;
    string strecode = string.Empty;
    string strbatch = string.Empty;
    string sqlstrbatch = string.Empty;
    string strdegree = string.Empty;
    string strdegreename = string.Empty;
    string sqlstrdegree = string.Empty;
    string strbranch = string.Empty;
    string sqlstrbranch = string.Empty;
    string strstaff1 = string.Empty;
    string sqlstrstaff1 = string.Empty;
    string strstaffdept = string.Empty;
    string sqlstrstaffdept1 = string.Empty;
    static int studorstaf = 0;
    static double schlSettCode = 0;
    static string clgcode = string.Empty;
    static string studclgcode = string.Empty;
    protected void lb2_Click(object sender, EventArgs e) //sankar edit For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //sankar edit For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        if (Buttonsave.Text == "Update")
        {
            Btn_Delete.Enabled = true;
        }
        else
        {
            // Btn_Delete.Enabled = false;
        }
        lbltravelladd.Text = "Add";
        usercode = Session["usercode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        lblerrdate.Visible = false;
        if (!Page.IsPostBack)
        {
            setLabelText();
            loadcollege();
            loadcollegestud();
            checkSchoolSetting();
            if (schlSettCode == 0)
                lblenqno.Text = "Admission No";
            else
                lblenqno.Text = "Roll No";
            if (rbdirectapply.Checked)
                studorstaf = 0;
            else
                studorstaf = 1;
            string clgcode = "";
            for (int clg = 0; clg < cblclg.Items.Count; clg++)
            {
                if (cblclg.Items[clg].Selected == true)
                {
                    if (clgcode == "")
                        clgcode = cblclg.Items[clg].Value;
                    else
                        clgcode = clgcode + "," + cblclg.Items[clg].Value;
                }
            }
            //   collegecode = Session["collegecode"].ToString();
            collegecode = clgcode;
            Session["studstaffcollegecode"] = null;
            // sprdMainapplication.Sheets[0].AutoPostBack = true;
            fpapplied.Sheets[0].PageSize = 5;
            fpapplied.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
            fpapplied.Pager.Mode = FarPoint.Web.Spread.PagerMode.NextPrev;
            fpapplied.Pager.Align = HorizontalAlign.Right;
            fpapplied.Pager.Font.Bold = true;
            fpapplied.Pager.ForeColor = Color.DarkGreen;
            fpapplied.Pager.BackColor = Color.Beige;
            fpapplied.Pager.BackColor = Color.AliceBlue;
            fpapplied.Pager.PageCount = 5;
            fpapplied.Sheets[0].SheetCorner.RowCount = 2;
            fpapplied.ActiveSheetView.SheetCorner.Cells[0, 0].Text = "S.No";
            fpapplied.ActiveSheetView.SheetCorner.DefaultStyle.Font.Bold = true;
            fpapplied.ActiveSheetView.DefaultRowHeight = 25;
            fpapplied.ActiveSheetView.Rows.Default.Font.Name = "MS Sans Serif";
            fpapplied.ActiveSheetView.Rows.Default.Font.Size = FontUnit.Small;
            fpapplied.ActiveSheetView.Rows.Default.Font.Bold = false;
            fpapplied.ActiveSheetView.Columns.Default.Font.Bold = false;
            fpapplied.ActiveSheetView.Columns.Default.Font.Size = FontUnit.Small;
            fpapplied.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
            fpapplied.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "MS Sans Serif";
            fpapplied.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Small;
            fpapplied.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
            fpapplied.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
            FarPoint.Web.Spread.TextCellType tx = new FarPoint.Web.Spread.TextCellType();
            fpapplied.Sheets[0].ColumnCount = 10;
            fpapplied.Sheets[0].RowCount = 0;
            fpapplied.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Vehicle ID";
            fpapplied.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Route ID";
            fpapplied.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Starting Place";
            fpapplied.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Arrival time";
            fpapplied.Sheets[0].Columns[3].CellType = tx;
            fpapplied.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Departure time";
            fpapplied.Sheets[0].Columns[4].CellType = tx;
            fpapplied.Sheets[0].ColumnHeader.Cells[0, 5].Text = "No.of Stage";
            fpapplied.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Alloted Seats";
            fpapplied.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Student";
            fpapplied.Sheets[0].ColumnHeader.Cells[1, 7].Text = "Staff";
            fpapplied.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Vacancy seats";
            fpapplied.Sheets[0].ColumnHeader.Cells[1, 8].Text = "Student";
            fpapplied.Sheets[0].ColumnHeader.Cells[1, 9].Text = "Staff";
            fpapplied.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            fpapplied.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            fpapplied.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            fpapplied.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            fpapplied.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            fpapplied.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
            fpapplied.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 1, 2);
            fpapplied.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 1, 2);
            fpapplied.Sheets[0].Columns[0].Width = 120;
            fpapplied.Sheets[0].Columns[0].Locked = true;
            fpapplied.Sheets[0].Columns[1].Locked = true;
            fpapplied.Width = 740;
            fpapplied.Sheets[0].AutoPostBack = true;
            fpapplied.CommandBar.Visible = false;
            fpapplied.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            fpapplied.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            fpapplied.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            fpapplied.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            fpapplied.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            fpapplied.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            fpapplied.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
            fpapplied.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
            fpapplied.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
            Fpload.Visible = false;
            rbdirectapply.Checked = true;
            rbsemtype.Checked = true;
            //rbtransfer.Checked = true;
            rbregular.Checked = true;
            rblateral.Checked = false;
            rbregular_CheckedChanged(sender, e);
            //load sem setting
            //  LoadSemesterSetting();
            BindRouteID();
            bindroute();
            bindVehicleID();
            Studentinfo();
            staffinfo();
            bindmonth();
            string group_code = "", columnfield = "";
            //Bind College=====================================================
            group_code = Session["group_code"].ToString();
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            dsprint = dacces2.select_method("bind_college", hat, "sp");
            ddlcolleges.Items.Clear();
            ddlcollegenew.Items.Clear();
            if (dsprint.Tables[0].Rows.Count > 0)
            {
                ddlcolleges.DataSource = dsprint;
                ddlcolleges.DataTextField = "collname";
                ddlcolleges.DataValueField = "college_code";
                ddlcolleges.DataBind();
                //ddlcolleges_SelectedIndexChanged(sender, e);
                ddlcollegenew.DataSource = dsprint;
                ddlcollegenew.DataTextField = "collname";
                ddlcollegenew.DataValueField = "college_code";
                ddlcollegenew.DataBind();
                ddlcollegenew_SelectedIndexChanged(sender, e);
            }
            bindcourse();
            bindBranch();
            bindBatch1();
            bindplace();
            // load_details();
            bindstaff();
            ddlheader.Items.Clear();
            ddlheader.Items.Add("---Select---");
            ddlheader.Items.Add("Roll No");
            ddlheader.Items.Add("Reg No");
            ddlheader.Items.Add("Name");
            ddloperator.Items.Clear();
            ddloperator.Items.Add("---Select---");
            ddloperator.Items.Add("Like");
            ddloperator.Items.Add("Starts With");
            ddloperator.Items.Add("Ends With");
            bindstaffdept();
            bindstaffdept1();
            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            //  LoadMainEnquiry();
            string dselect1 = "";
            #region old farpoint
            //// sprdMainapplication.Sheets[0].AutoPostBack = true;
            //sprdMainapplication.Sheets[0].AutoPostBack = false;
            //sprdMainapplication.CommandBar.Visible = false;
            //sprdMainapplication.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            //sprdMainapplication.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
            //sprdMainapplication.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            //sprdMainapplication.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            //sprdMainapplication.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            //sprdMainapplication.Sheets[0].DefaultStyle.Font.Bold = false;
            //FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
            //style1.Font.Size = 12;
            //style1.Font.Bold = true;
            //style1.HorizontalAlign = HorizontalAlign.Left;
            //style1.ForeColor = Color.Black;
            //sprdMainapplication.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            //sprdMainapplication.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            //sprdMainapplication.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Left;
            //FarPoint.Web.Spread.TextCellType tb = new FarPoint.Web.Spread.TextCellType();
            //sprdMainapplication.Sheets[0].AllowTableCorner = true;
            //sprdMainapplication.Width = 850;
            //sprdMainapplication.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            ////sprdMainapplication.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            //sprdMainapplication.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            //sprdMainapplication.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
            //sprdMainapplication.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
            //sprdMainapplication.Pager.Align = HorizontalAlign.Right;
            //sprdMainapplication.Pager.Font.Bold = true;
            //sprdMainapplication.Pager.Font.Name = "Book Antiqua";
            //sprdMainapplication.Pager.ForeColor = Color.DarkGreen;
            //sprdMainapplication.Pager.BackColor = Color.Beige;
            //sprdMainapplication.Pager.BackColor = Color.AliceBlue;
            //sprdMainapplication.Sheets[0].ColumnCount = 8;
            ////sprdMainapplication.SheetCorner.Cells[0, 0].Text = "S.No";
            //sprdMainapplication.Sheets[0].RowHeader.Visible = false;
            ////sprdMainapplication.ActiveSheetView.SheetCorner = false;
            //sprdMainapplication.SheetCorner.Columns[0].HorizontalAlign = HorizontalAlign.Left;
            ////sprdMainapplication.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Enq No";
            //sprdMainapplication.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Left;
            //sprdMainapplication.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            //sprdMainapplication.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            //sprdMainapplication.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            //sprdMainapplication.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            //sprdMainapplication.Sheets[0].Columns[0].CellType = tb;
            //sprdMainapplication.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Name";
            //sprdMainapplication.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            //sprdMainapplication.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Route";
            //sprdMainapplication.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            //sprdMainapplication.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Vehicle ID";
            //sprdMainapplication.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            //sprdMainapplication.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Seat No";
            //sprdMainapplication.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            //sprdMainapplication.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Start Place";
            //sprdMainapplication.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            //sprdMainapplication.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Department";
            //sprdMainapplication.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            //sprdMainapplication.Sheets[0].Columns[6].Visible = true;
            //sprdMainapplication.Visible = true;
            //sprdMainapplication.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            //sprdMainapplication.Sheets[0].Columns[0].Locked = true;
            //sprdMainapplication.Sheets[0].Columns[1].Locked = true;
            //sprdMainapplication.Sheets[0].Columns[2].Locked = true;
            //sprdMainapplication.Sheets[0].Columns[3].Locked = true;
            //sprdMainapplication.Sheets[0].Columns[4].Locked = true;
            //sprdMainapplication.Sheets[0].Columns[5].Locked = true;
            //sprdMainapplication.Sheets[0].Columns[6].Locked = true;
            //sprdMainapplication.Sheets[0].Columns[7].Locked = true;
            //sprdMainapplication.Sheets[0].Columns[0].Width = 50;
            //sprdMainapplication.Sheets[0].Columns[1].Width = 130;
            //sprdMainapplication.Sheets[0].Columns[2].Width = 80;
            //sprdMainapplication.Sheets[0].Columns[3].Width = 80;
            //sprdMainapplication.Sheets[0].Columns[4].Width = 80;
            //sprdMainapplication.Sheets[0].Columns[6].Width = 80;
            //sprdMainapplication.Visible = false;
            #endregion
            Accordion1.SelectedIndex = 0;
            feeset();
            rbsemtype_Changed(sender, e);
            tbdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            bindmonth();
            year();
            SettingRights();
        }
        if (cblclg.Items.Count > 0)
        {
            string clgcode = "";
            for (int clg = 0; clg < cblclg.Items.Count; clg++)
            {
                if (cblclg.Items[clg].Selected == true)
                {
                    if (clgcode == "")
                        clgcode = cblclg.Items[clg].Value;
                    else
                        clgcode = clgcode + "," + cblclg.Items[clg].Value;
                }
            }
            //   collegecode = Session["collegecode"].ToString();
            collegecode = clgcode;
        }
    }
    public void loadcollegestud()
    {
        try
        {
            ds.Clear();
            ddlclgstud.Items.Clear();
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = dacces2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlclgstud.DataSource = ds;
                ddlclgstud.DataTextField = "collname";
                ddlclgstud.DataValueField = "college_code";
                ddlclgstud.DataBind();
                studclgcode = Convert.ToString(ddlclgstud.SelectedValue);
            }
        }
        catch
        { }
    }
    private void LoadSemesterSetting()
    {
        try
        {
            string settingquery = "";
            settingquery = dacces2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'");
            if (settingquery == "0")
            {
                rbsemtype.Enabled = true;
                rbsemtype.Checked = true;
                rbstutype.Enabled = false;
                rbstutype.Checked = false;
                rbtranfer.Enabled = false;
                rbtranfer.Checked = false;
                rbtermtype.Checked = false;
                rbtermtype.Enabled = false;
            }
            else if (settingquery == "1")
            {
                rbsemtype.Enabled = false;
                rbsemtype.Checked = false;
                rbstutype.Enabled = true;
                rbstutype.Checked = true;
                rbtranfer.Enabled = false;
                rbtranfer.Checked = false;
                rbtermtype.Checked = false;
                rbtermtype.Enabled = false;
            }
            else if (settingquery == "2")
            {
                rbsemtype.Enabled = false;
                rbsemtype.Checked = false;
                rbstutype.Enabled = false;
                rbstutype.Checked = false;
                rbtranfer.Enabled = false;
                rbtranfer.Checked = false;
                rbtermtype.Checked = true;
                rbtermtype.Enabled = true;
            }
        }
        catch { }
    }
    public void feeset()
    {
        try
        {
            //  ds.Clear();
            //check scholl or college setting 
            checkSchoolSetting();
            string strRoll = string.Empty;
            if (schlSettCode != 0)
                strRoll = " and r.roll_no";
            else
                strRoll = " and r.roll_admit";
            string strquery = "";
            string rollid = Convert.ToString(tbenqno.Text);
            if (rollid != "")
            {
                if (ViewState["Clgcode"] != null)
                {
                    collegecode = Convert.ToString(ViewState["Clgcode"]);
                }
                int semandyear = 0;
                string feeSetgCode = dacces2.GetFunction("select value from Master_Settings where settings='TransportFeeAllotmentSettings'  and usercode='" + usercode + "'");
                if (feeSetgCode == "1")
                    semandyear = 1;
                else if (feeSetgCode == "2")
                    semandyear = 2;
                else if (feeSetgCode == "3")
                    semandyear = 3;
                else if (feeSetgCode == "4")
                    semandyear = 4;
                string sem = dacces2.GetFunction("select r.Current_Semester from Registration r where r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar'  " + strRoll + "='" + rollid + "' and college_code=" + collegecode + "");
                if (semandyear == 1)
                {
                    strquery = "SELECT * FROM TEXTVALTABLE WHERE textcriteria='FEECA' and college_code=" + collegecode + " and textval <> 'Hostel' and right(TextVal,4) <>'Year' and textval not like'%-%' and textval not like'%&%' order by textval";
                    if (sem == "1")
                        semval = "1 Semester";
                    if (sem == "2")
                        semval = "2 Semester";
                    if (sem == "3")
                        semval = "3 Semester";
                    if (sem == "4")
                        semval = "4 Semester";
                    if (sem == "5")
                        semval = "5 Semester";
                    if (sem == "6")
                        semval = "6 Semester";
                    if (sem == "7")
                        semval = "7 Semester";
                    if (sem == "8")
                        semval = "8 Semester";
                    if (sem == "9")
                        semval = "9 Semester";
                }
                else if (semandyear == 2)
                {
                    strquery = "SELECT * FROM TEXTVALTABLE WHERE textcriteria='FEECA' and college_code=" + collegecode + " and textval <> 'Hostel'  and right(TextVal,4) ='Year' order by textval";
                    if (sem == "1" || sem == "2")
                        semval = "1 Year";
                    else if (sem == "3" || sem == "4")
                        semval = "2 Year";
                    else if (sem == "5" || sem == "6")
                        semval = "3 Year";
                    else if (sem == "7" || sem == "8")
                        semval = "4 Year";
                }
                else if (semandyear == 3)
                {
                    #region
                    // semval = feecatValue(sem);
                    strquery = dacces2.GetFunction("select linkvalue from New_InsSettings where college_code=" + collegecode + " and LinkName='Fee Yearwise' And user_code = '" + usercode + "'");
                    if (strquery == "1")
                    {
                        strquery = "SELECT * FROM TEXTVALTABLE WHERE textcriteria='FEECA' and college_code=" + collegecode + " and textval <> 'Hostel'  and right(TextVal,4) ='Year' order by textval";
                        if (sem == "1" || sem == "2")
                            semval = "1 Year";
                        else if (sem == "3" || sem == "4")
                            semval = "2 Year";
                        else if (sem == "5" || sem == "6")
                            semval = "3 Year";
                        else if (sem == "7" || sem == "8")
                            semval = "4 Year";
                    }
                    else
                    {
                        strquery = "SELECT * FROM TEXTVALTABLE WHERE textcriteria='FEECA' and college_code=" + collegecode + " and textval <> 'Hostel' and right(TextVal,4) <>'Year' and textval not like'%-%' and textval not like'%&%' order by textval";
                        if (sem == "1")
                            semval = "1 Semester";
                        if (sem == "2")
                            semval = "2 Semester";
                        if (sem == "3")
                            semval = "3 Semester";
                        if (sem == "4")
                            semval = "4 Semester";
                        if (sem == "5")
                            semval = "5 Semester";
                        if (sem == "6")
                            semval = "6 Semester";
                        if (sem == "7")
                            semval = "7 Semester";
                        if (sem == "8")
                            semval = "8 Semester";
                        if (sem == "9")
                            semval = "9 Semester";
                    }
                    #endregion
                }
                else if (semandyear == 4)
                {
                    strquery = "SELECT * FROM TEXTVALTABLE WHERE textcriteria='FEECA' and college_code=" + collegecode + " and textval <> 'Hostel'  and right(TextVal,4) <>'Year' order by textval";
                    if (sem == "1")
                        semval = "Term 1";
                    else if (sem == "2")
                        semval = "Term 2";
                    else if (sem == "3")
                        semval = "Term 3";
                    else if (sem == "4")
                        semval = "Term 4";
                }
                #region old
                #endregion
                //current semester
                string curval = dacces2.GetFunction("select TextCode from textvaltable where TextCriteria = 'FEECA'and textval like '" + semval + "' and college_code='" + collegecode + "' and textval not like '-1%'");
                ds = dacces2.select_method_wo_parameter(strquery, "Text");
                chklsfeeset.Items.Clear();
                fee_cate.Items.Clear();
                int count = 0;
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                    {
                        string feecode = Convert.ToString(ds.Tables[0].Rows[k]["TextCode"]);
                        string feetext = Convert.ToString(ds.Tables[0].Rows[k]["Textval"]);
                        if (Convert.ToInt32(feecode) > Convert.ToInt32(curval))
                        {
                            //cbl
                            chklsfeeset.Items.Add(new ListItem(feetext, feecode));
                            count++;
                            fee_cate.Items.Add(new ListItem(feetext, feecode));
                            for (int i = 0; i < chklsfeeset.Items.Count; i++)
                            {
                                chklsfeeset.Items[i].Selected = true;
                            }
                            chkfeeset.Checked = true;
                            txtfeeset.Text = "Fee Category (" + count + ")";
                            txtfeeset.Enabled = true;
                            btnfeeset.Enabled = true;
                        }
                    }
                }
            }
            else
            {
                txtfeeset.Enabled = false;
            }
        }
        catch { }
    }
    protected void chkfeeset_CheckedChange(object sender, EventArgs e)
    {
        if (chkfeeset.Checked == true)
        {
            for (int i = 0; i < chklsfeeset.Items.Count; i++)
            {
                chklsfeeset.Items[i].Selected = true;
            }
            txtfeeset.Text = "Fee Category (" + chklsfeeset.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklsfeeset.Items.Count; i++)
            {
                chklsfeeset.Items[i].Selected = false;
            }
            txtfeeset.Text = "--Select--";
        }
    }
    protected void chkfeeset_SelectedIndexChanged(object sender, EventArgs e)
    {
        int commcount = 0;
        for (int i = 0; i < chklsfeeset.Items.Count; i++)
        {
            if (chklsfeeset.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                txtfeeset.Text = "Fee Category (" + commcount.ToString() + ")";
            }
        }
        if (commcount == 0)
        {
            txtfeeset.Text = "--Select--";
            chkfeeset.Checked = false;
        }
        else if (commcount == chklsfeeset.Items.Count)
        {
            chkfeeset.Checked = true;
        }
        else
        {
            chkfeeset.Checked = false;
        }
    }
    //public void load_details()
    //{
    //    load_studentlookup();
    //    //RouteLookup();
    //    load_stafflookup();
    //    tbenqno.Attributes.Add("onfocus", "changerollno()");
    //    tbseatno.Attributes.Add("onfocus", "changeseatno()");
    //    tbdate.Attributes.Add("readonly", "readonly");
    //    dss = new DataSet();
    //    con.Open();
    //    ddlcollegestaff.Items.Insert(0, "All");
    //    danew = new SqlDataAdapter("select collname,college_code,acr from collinfo", con);
    //    danew.Fill(dss);
    //    if (dss.Tables[0].Rows.Count > 0)
    //    {
    //        ddlcollegestaff.DataSource = dss;
    //        ddlcollegestaff.DataTextField = "collname";
    //        ddlcollegestaff.DataValueField = "college_code";
    //        ddlcollegestaff.DataBind();
    //    }
    //}
    protected void LoadMainEnquiry()
    {
        string dselect1 = "";
        // sprdMainapplication.Sheets[0].AutoPostBack = true;
        Fpload.Sheets[0].AutoPostBack = false;
        Fpload.CommandBar.Visible = false;
        Fpload.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
        Fpload.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
        Fpload.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
        Fpload.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        Fpload.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        Fpload.Sheets[0].DefaultStyle.Font.Bold = false;
        FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
        style1.Font.Size = 12;
        style1.Font.Bold = true;
        style1.HorizontalAlign = HorizontalAlign.Left;
        style1.ForeColor = Color.Black;
        Fpload.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
        Fpload.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
        Fpload.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Left;
        FarPoint.Web.Spread.TextCellType tb = new FarPoint.Web.Spread.TextCellType();
        Fpload.Sheets[0].AllowTableCorner = true;
        Fpload.Width = 850;
        Fpload.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
        //sprdMainapplication.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
        Fpload.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
        //sprdMainapplication.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
        //sprdMainapplication.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
        //sprdMainapplication.Pager.Align = HorizontalAlign.Right;
        //sprdMainapplication.Pager.Font.Bold = true;
        //sprdMainapplication.Pager.Font.Name = "Book Antiqua";
        //sprdMainapplication.Pager.ForeColor = Color.DarkGreen;
        //sprdMainapplication.Pager.BackColor = Color.Beige;
        //sprdMainapplication.Pager.BackColor = Color.AliceBlue;
        Fpload.Sheets[0].ColumnCount = 7;
        //sprdMainapplication.SheetCorner.Cells[0, 0].Text = "S.No";
        Fpload.Sheets[0].RowHeader.Visible = false;
        //sprdMainapplication.ActiveSheetView.SheetCorner = false;
        Fpload.SheetCorner.Columns[0].HorizontalAlign = HorizontalAlign.Left;
        //sprdMainapplication.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Enq No";
        Fpload.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Left;
        Fpload.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        Fpload.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        Fpload.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
        Fpload.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
        Fpload.Sheets[0].Columns[0].CellType = tb;
        Fpload.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Name";
        Fpload.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
        Fpload.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Route";
        Fpload.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
        Fpload.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Vehicle ID";
        Fpload.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
        Fpload.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Start Place";
        Fpload.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
        Fpload.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Department";
        Fpload.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
        Fpload.Sheets[0].Columns[5].Visible = true;
        Fpload.Visible = true;
        Fpload.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        Fpload.Sheets[0].Columns[0].Locked = true;
        Fpload.Sheets[0].Columns[1].Locked = true;
        Fpload.Sheets[0].Columns[2].Locked = true;
        Fpload.Sheets[0].Columns[3].Locked = true;
        Fpload.Sheets[0].Columns[4].Locked = true;
        Fpload.Sheets[0].Columns[5].Locked = true;
        Fpload.Sheets[0].Columns[6].Locked = true;
        Fpload.Sheets[0].Columns[0].Width = 50;
        Fpload.Sheets[0].Columns[1].Width = 130;
        Fpload.Sheets[0].Columns[2].Width = 80;
        Fpload.Sheets[0].Columns[3].Width = 80;
        Fpload.Sheets[0].Columns[4].Width = 80;
        Fpload.Sheets[0].Columns[5].Width = 80;
        Fpload.Visible = true;
        Fpload.Sheets[0].RowCount = 0;
        if (rbregular.Checked == false && rblateral.Checked == false && rbtransfer.Checked == true)
        {
            if (ddlrouteview.Text == "-1" || ddlvehicletype.Text == "-1" || ddlrouteview.Text == "" || ddlvehicletype.Text == "")
            {
                sqlcmd = "select Roll_No,Stud_Name,Bus_RouteID,VehID,Boarding,de.dept_acronym from Registration r,Degree d,Department de where  r.degree_code=d.Degree_Code and d.Dept_Code=de.dept_code and Bus_RouteID is not null and Bus_RouteID<>'' and VehID is not null and VehID<>'' and Boarding is not null and Boarding<>'' and CC=0 and Exam_Flag <>'debar' and DelFlag=0";
                dsload = dacces2.select_method_wo_parameter(sqlcmd, "Text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    btnprintmaster.Enabled = true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btn_excel.Visible = true;
                    for (loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                    {
                        Fpload.Sheets[0].RowCount++;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 0].Text = Fpload.Sheets[0].RowCount.ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 1].Text = dsload.Tables[0].Rows[loop]["Roll_No"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["Stud_Name"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["Bus_RouteID"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 4].Text = dsload.Tables[0].Rows[loop]["VehID"].ToString();
                        string stage_name = "";
                        if (Has_Stage.Contains(Convert.ToString(dsload.Tables[0].Rows[loop]["Boarding"])) == true)
                        {
                            stage_name = Convert.ToString(GetCorrespondingKey(Convert.ToString(dsload.Tables[0].Rows[loop]["Boarding"]), Has_Stage));
                        }
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 5].Text = stage_name;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(dsload.Tables[0].Rows[loop]["Boarding"]);
                        Fpload.Sheets[0].Rows[loop].BackColor = Color.Lavender;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 6].Text = dsload.Tables[0].Rows[loop]["dept_acronym"].ToString();//Added By Srinath 9/8/2013
                    }
                }
                sqlcmd = "select s.staff_code,s.staff_name,s.Bus_RouteID,s.VehID,s.Boarding,hm.dept_acronym from staffmaster s,stafftrans st,hrdept_master hm where s.staff_code=st.staff_code and st.dept_code=hm.dept_code and Bus_RouteID is not null and Bus_RouteID<>'' and VehID is not null and  VehID<>'' and Boarding is not null and Boarding<>'' and s.college_code=hm.college_code and s.college_code=" + collegecode + " and s.settled <>1 and s.resign <>1 and  st.latestrec<>0 ";
                dsload = dacces2.select_method_wo_parameter(sqlcmd, "Text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    btnprintmaster.Enabled = true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btn_excel.Visible = true;
                    for (int loop1 = 0; loop1 < dsload.Tables[0].Rows.Count; loop1++)
                    {
                        Fpload.Sheets[0].RowCount++;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 0].Text = Fpload.Sheets[0].RowCount.ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 1].Text = dsload.Tables[0].Rows[loop1]["staff_code"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop1]["staff_name"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop1]["Bus_RouteID"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 4].Text = dsload.Tables[0].Rows[loop1]["VehID"].ToString();
                        string stage_name = "";
                        if (Has_Stage.Contains(Convert.ToString(dsload.Tables[0].Rows[loop1]["Boarding"])) == true)
                        {
                            stage_name = Convert.ToString(GetCorrespondingKey(Convert.ToString(dsload.Tables[0].Rows[loop1]["Boarding"]), Has_Stage));
                        }
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 5].Text = stage_name;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(dsload.Tables[0].Rows[loop1]["Boarding"]);
                        Fpload.Sheets[0].Rows[Fpload.Sheets[0].RowCount - 1].BackColor = Color.LavenderBlush;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 6].Text = dsload.Tables[0].Rows[loop1]["dept_acronym"].ToString();
                    }
                    Fpload.SaveChanges();
                    Fpload.Visible = true;
                    lblerrmainapp.Visible = false;
                }
                if (Fpload.Sheets[0].RowCount == 0)
                {
                    lblerrmainapp.Visible = true;
                    lblerrmainapp.Text = "No Record(s) Found";
                    btnprintmaster.Enabled = false;
                    Fpload.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btn_excel.Visible = false;
                }
                Fpload.Sheets[0].PageSize = Fpload.Rows.Count;
            }
        }
    }
    protected void tbseatno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (tbseatno.Text.Trim() != "")
            {
                lblerrdate.Enabled = false;
                string seatno = "";
                seatno = GetFunction("select TotalNo_Seat from vehicle_master where Veh_ID = '" + tbvehno.Text.Trim() + "'  and Route='" + tbroute.Text + "'");
                if (seatno != "")
                {
                    if (Convert.ToInt32(tbseatno.Text) > Convert.ToInt32(seatno))
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Seat Not Available";
                        // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Seat Not Available')", true);
                        tbseatno.Text = "";
                    }
                    else
                    {
                        Buttonsave.Enabled = true;
                        sqlcmd = "Select 1 from Registration where Seat_No='" + tbseatno.Text.Trim() + "' and VehID = '" + tbvehno.Text.Trim() + "'";
                        d1 = dacces2.select_method_wo_parameter(sqlcmd, "n");
                        if (d1.Tables[0].Rows.Count > 0)
                        {
                            //Buttonsave.Text = "Update";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Seat No Already Exists')", true);
                            tbseatno.Text = "";
                        }
                        else
                        {
                            con.Close();
                            con.Open();
                            SqlCommand cmd_get_total = new SqlCommand("select * from vehicle_master where veh_id='" + tbvehno.Text + "'", con);
                            SqlDataAdapter ad_get_total = new SqlDataAdapter(cmd_get_total);
                            DataTable dt_get_total = new DataTable();
                            ad_get_total.Fill(dt_get_total);
                            if (dt_get_total.Rows.Count > 0)
                            {
                                string stu_stf_tot = string.Empty;
                                string SQL = string.Empty;
                                if (rbdirectapply.Checked == true)
                                {
                                    SQL = "select count(*) as count from registration where vehid='" + tbvehno.Text + "'";
                                    stu_stf_tot = dt_get_total.Rows[0]["nofstudents"].ToString();
                                }
                                else if (rbenquiry.Checked == true)
                                {
                                    SQL = "select count(*) as count from staffmaster where vehid='" + tbvehno.Text + "'";
                                    stu_stf_tot = dt_get_total.Rows[0]["nofstaffs"].ToString();
                                }
                                if (stu_stf_tot == "")
                                {
                                    stu_stf_tot = "0";
                                }
                                con.Close();
                                con.Open();
                                SqlCommand cmd_get_count = new SqlCommand(SQL, con);
                                SqlDataAdapter ad_get_count = new SqlDataAdapter(cmd_get_count);
                                DataTable dt_get_count = new DataTable();
                                ad_get_count.Fill(dt_get_count);
                                if (dt_get_count.Rows.Count > 0)
                                {
                                    int tot_count = Convert.ToInt32(dt_get_count.Rows[0]["count"].ToString());
                                    if (tot_count >= Convert.ToInt32(stu_stf_tot))
                                    {
                                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No seats available.')", true);
                                        imgAlert.Visible = true;
                                        lbl_alert.Text = "No seats available.";
                                        tbseatno.Text = "";
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Allot the Total Seat')", true);
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Allot the Total Seat";
                    tbseatno.Text = "";
                }
            }
        }
        catch
        {
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Input String Not Correct')", true);
            imgAlert.Visible = true;
            lbl_alert.Text = "Input String Not Correct";
        }
    }
    public void bindVehicleID()
    {
        Connection();
        ddlvehicletype.Items.Clear();
        ddlvehicletype.Items.Insert(0, new ListItem("All", "-1"));
        string sql;
        sql = "select distinct Veh_ID from vehicle_master order by Veh_ID";
        ds = dacces2.select_method_wo_parameter(sql, "txt");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ddlvehicletype.Items.Add(ds.Tables[0].Rows[i]["Veh_ID"].ToString());
            }
            ddlvehicletype.SelectedIndex = 0;
        }
        con.Close();
    }
    public void BindRouteID()
    {
        Connection();
        ddlrouteview.Items.Clear();
        ddlrouteview.Items.Insert(0, new ListItem("All", "-1"));
        string sql;
        sql = "select distinct Route_ID from routemaster order by Route_ID";
        ds = dacces2.select_method_wo_parameter(sql, "txt");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ddlrouteview.Items.Add(ds.Tables[0].Rows[i]["Route_ID"].ToString());
            }
            ddlrouteview.SelectedIndex = 0;
        }
        con.Close();
    }
    public void bindroute()
    {
        Connection();
        ddlrouteID.Items.Clear();
        ddlrouteID.Items.Insert(0, new ListItem("All", "-1"));
        string sql;
        sql = "select distinct Route_ID from routemaster order by Route_ID";
        ds = dacces2.select_method_wo_parameter(sql, "txt");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ddlrouteID.Items.Add(ds.Tables[0].Rows[i]["Route_ID"].ToString());
            }
            ddlrouteID.SelectedIndex = 0;
        }
        con.Close();
    }
    public void Studentinfo()
    {
        FpSpread1.Sheets[0].PageSize = 5;
        FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
        FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.NextPrev;
        FpSpread1.Pager.Align = HorizontalAlign.Right;
        FpSpread1.Pager.Font.Bold = true;
        FpSpread1.Pager.ForeColor = Color.DarkGreen;
        FpSpread1.Pager.BackColor = Color.Beige;
        FpSpread1.Pager.BackColor = Color.AliceBlue;
        FpSpread1.Pager.PageCount = 5;
        FpSpread1.ActiveSheetView.SheetCorner.Cells[0, 0].Text = "S.No";
        FpSpread1.ActiveSheetView.SheetCorner.DefaultStyle.Font.Bold = true;
        FpSpread1.ActiveSheetView.DefaultRowHeight = 25;
        FpSpread1.ActiveSheetView.Rows.Default.Font.Name = "MS Sans Serif";
        FpSpread1.ActiveSheetView.Rows.Default.Font.Size = FontUnit.Small;
        FpSpread1.ActiveSheetView.Rows.Default.Font.Bold = false;
        FpSpread1.ActiveSheetView.Columns.Default.Font.Bold = false;
        FpSpread1.ActiveSheetView.Columns.Default.Font.Size = FontUnit.Small;
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "MS Sans Serif";
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Small;
        FpSpread1.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        FpSpread1.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        FpSpread1.Sheets[0].ColumnCount = 4;
        FarPoint.Web.Spread.TextCellType tt = new FarPoint.Web.Spread.TextCellType();
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Degree";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
        FpSpread1.Sheets[0].Columns[2].CellType = tt;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "StudentName";
        FpSpread1.Sheets[0].Columns[0].Width = 500;
        FpSpread1.Sheets[0].Columns[0].Locked = true;
        FpSpread1.Sheets[0].Columns[1].Locked = true;
        FpSpread1.Sheets[0].Columns[1].Width = 100;
        FpSpread1.Sheets[0].Columns[2].Width = 100;
        FpSpread1.Sheets[0].Columns[3].Width = 200;
        FpSpread1.Width = 650;
        FpSpread1.Sheets[0].AutoPostBack = true;
        FpSpread1.CommandBar.Visible = false;
    }
    public void staffinfo()
    {
        FpSpread2.Sheets[0].PageSize = 5;
        FpSpread2.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
        FpSpread2.Pager.Mode = FarPoint.Web.Spread.PagerMode.NextPrev;
        FpSpread2.Pager.Align = HorizontalAlign.Right;
        FpSpread2.Pager.Font.Bold = true;
        FpSpread2.Pager.ForeColor = Color.DarkGreen;
        FpSpread2.Pager.BackColor = Color.Beige;
        FpSpread2.Pager.BackColor = Color.AliceBlue;
        FpSpread2.Pager.PageCount = 5;
        FpSpread2.ActiveSheetView.SheetCorner.Cells[0, 0].Text = "S.No";
        FpSpread2.ActiveSheetView.SheetCorner.DefaultStyle.Font.Bold = true;
        FpSpread2.ActiveSheetView.DefaultRowHeight = 25;
        FpSpread2.ActiveSheetView.Rows.Default.Font.Name = "MS Sans Serif";
        FpSpread2.ActiveSheetView.Rows.Default.Font.Size = FontUnit.Small;
        FpSpread2.ActiveSheetView.Rows.Default.Font.Bold = false;
        FpSpread2.ActiveSheetView.Columns.Default.Font.Bold = false;
        FpSpread2.ActiveSheetView.Columns.Default.Font.Size = FontUnit.Small;
        FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
        FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "MS Sans Serif";
        FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Small;
        FpSpread2.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        FpSpread2.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        FarPoint.Web.Spread.TextCellType tt = new FarPoint.Web.Spread.TextCellType();
        FpSpread2.Sheets[0].ColumnCount = 2;
        FpSpread2.Sheets[0].RowCount = 0;
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Staff Code";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Name";
        FpSpread2.Sheets[0].Columns[0].Width = 120;
        FpSpread2.Sheets[0].Columns[0].Locked = true;
        FpSpread2.Sheets[0].Columns[1].Locked = true;
        FpSpread2.Sheets[0].Columns[1].Width = 130;
        FpSpread2.Width = 494;
        FpSpread2.Sheets[0].AutoPostBack = true;
        FpSpread2.CommandBar.Visible = false;
    }
    protected void rbdirectapply_CheckedChanged(object sender, EventArgs e)
    {
        if (rbdirectapply.Checked == true)
        {
            checkSchoolSetting();
            if (schlSettCode == 0)
                lblenqno.Text = "Admission No";
            else
                lblenqno.Text = "Roll No";
            Label1.Text = "Student Name";
            Label2.Text = Label2.Text;
            clear();
            lblfeecat.Visible = false;
            fee_cate.Visible = false;
            lblconcession.Visible = false;
            txtconcession.Visible = false;
            studorstaf = 0;
            enqbtn.Enabled = true;
        }
    }
    protected void ddlserachby_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlrouteID.Enabled = false;
        string sqlquery = string.Empty;
        ddlrouteID.Items.Clear();
        ddlrouteID.Items.Insert(0, new ListItem("All", "-1"));
        if (ddlserachby.Text == "-1")
        {
            sqlquery = "select distinct Route_ID from routemaster";
        }
        else
        {
            sqlquery = "select distinct Route_ID from routemaster where Stage_Name = '" + ddlserachby.Text.ToString() + "'";
        }
        ds = dacces2.select_method_wo_parameter(sqlquery, "txt");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ddlrouteID.Items.Add(ds.Tables[0].Rows[i]["Route_ID"].ToString());
            }
            ddlrouteID.SelectedIndex = 0;
        }
        con.Close();
    }
    public void bindplace()
    {
        Has_Stage.Clear();
        Connection();
        ddlstage.Items.Clear();//Added By SRinath 8/10/2013
        ddlserachby.Items.Clear();
        ddlserachby.Items.Insert(0, new ListItem("All", "-1"));
        ddlstage.Items.Insert(0, new ListItem("All", "-1"));//Added By SRinath 8/10/2013
        string sql;
        sql = "select * from Stage_Master";
        ds = dacces2.select_method_wo_parameter(sql, "txt");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ddlstage.Items.Add(ds.Tables[0].Rows[i]["Stage_Name"].ToString());
                if (Has_Stage.Contains(Convert.ToString(ds.Tables[0].Rows[i]["Stage_id"])) == false)
                {
                    Has_Stage.Add(Convert.ToString(ds.Tables[0].Rows[i]["Stage_id"]), Convert.ToString(ds.Tables[0].Rows[i]["Stage_Name"]));
                }
            }
            ddlserachby.SelectedIndex = 0;
            ddlstage.SelectedIndex = 0;//Added By SRinath 8/10/2013
            ViewState["Stage"] = Has_Stage;
        }
        con.Close();
    }
    protected void rbenquiry_CheckedChanged(object sender, EventArgs e)
    {
        if (rbenquiry.Checked == true)
        {
            lblenqno.Text = "Staff Code";
            Label1.Text = "Staff Name";
            Label2.Text = "Department";
            clear();
            lblfeecat.Visible = true;
            fee_cate.Visible = true;
            lblconcession.Visible = true;
            txtconcession.Visible = true;
            txtconcession.Text = "";
            txtconcession.Enabled = true;
            studorstaf = 1;
            enqbtn.Enabled = false;
        }
    }
    protected void enqbtn_Click(object sender, EventArgs e)
    {
        Session["studstaffcollegecode"] = null;
        if (rbdirectapply.Checked == true)
        {
            Panellookup1.Visible = true;
        }
        else if (rbenquiry.Checked == true)
        {
            dir = "2";
            //if (ddlstaffname.Text.Trim() == "")
            //{
            //    Labelvalidation.Visible = true;
            //    Labelvalidation.Text = "Enter Referred Staff Name"; return;
            //}
            hastab.Add("dir", dir);
            //hastab.Add("refer_stcode", ddlstaffname.SelectedValue.ToString());
            hastab.Add("refer_collegecode", ddlcolleges.SelectedValue.ToString());
            pnllookstaff.Visible = true;
        }
    }
    protected void routebtn_Click(object sender, EventArgs e)
    {
        if (tbborplace.Text != "")
        {
            lblerrdate.Visible = false;
            bindroute();
            RouteLookup();
            fpapplied.Visible = true;
            Panellookup.Visible = true;
        }
        else
        {
            lblerrdate.Visible = true;
            fpapplied.Visible = false;
            Panellookup.Visible = false;
            lblerrdate.Text = "Please Enter the Boarding Place";
            return;
        }
    }
    //roll no textchanged
    protected void tbenqno_TextChanged(object sender, EventArgs e)
    {
        //school or college check setting
        checkSchoolSetting();
        string rollno = Convert.ToString(tbenqno.Text);
        if (rbdirectapply.Checked == true)
        {
            if (rollno != "")
            {
                string strroll = string.Empty;
                if (schlSettCode != 0)
                    strroll = " and r.Roll_no='" + rollno + "'";
                else
                    strroll = " and r.Roll_admit='" + rollno + "'";
                string query = "select a.parent_name,a.stud_name, r.Roll_no,r.Roll_admit,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State,d.college_code  from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  " + strroll + " and r.college_code='" + ddlclgstud.SelectedValue + "'  and ((isnull(Bus_RouteID,'')='' and isnull(Boarding,'')='' and isnull(VehID,'')='') or iscanceledstage='1')";
                //and d.college_code=" + collegecode + "
                ds = dacces2.select_method_wo_parameter(query, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (schlSettCode != 0)
                        tbenqno.Text = Convert.ToString(ds.Tables[0].Rows[0]["Roll_no"]);
                    else
                        tbenqno.Text = Convert.ToString(ds.Tables[0].Rows[0]["Roll_admit"]);
                    tbpname.Text = Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]);
                    tbdept.Text = Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]);
                    string studClgcode = Convert.ToString(ds.Tables[0].Rows[0]["college_code"]);
                    string studphoto = "select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + rollno + "')";
                    ds = dacces2.select_method_wo_parameter(studphoto, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        photo.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + rollno;
                        photo.Visible = true;
                    }
                    else
                    {
                        photo.Visible = false;
                    }
                    ViewState["Clgcode"] = studClgcode;
                    feeset();
                }
                else
                {
                    tbenqno.Text = "";
                    tbpname.Text = "";
                    tbdept.Text = "";
                    tbborplace.Text = "";
                    tbvehno.Text = "";
                    tbroute.Text = "";
                    tbseatno.Text = "";
                    photo.Visible = false;
                }
            }
            else
            {
                tbenqno.Text = "";
                tbpname.Text = "";
                tbdept.Text = "";
                tbborplace.Text = "";
                tbvehno.Text = "";
                tbroute.Text = "";
                tbseatno.Text = "";
                photo.Visible = false;
            }
        }
        else
        {
            getstaffcode(rollno);
        }
    }
    public void getstaffcode(string staff_Code)
    {
        try
        {
            string query = "select s.staff_Code,s.staff_name,dm.desig_name,hr.dept_name,sa.ccity,dm.staffcategory,sa.comm_address,sa.comm_address1,sa.com_mobileno,sa.com_phone,sa.cstate,sa.email,sa.com_pincode,CONVERT(varchar(10), s.join_date,103) as join_date,st.stftype,s.college_code  from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm,stafftrans st where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and st.staff_code =s.staff_code and latestrec =1 and s.staff_Code='" + staff_Code + "' and s.college_code='" + ddlclgstud.SelectedValue + "' and ((isnull(Bus_RouteID,'')='' and isnull(Boarding,'')='' and isnull(VehID,'')='') or iscanceledstage='1')";
            ds = dacces2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string stafcode = "";
                    stafcode = Convert.ToString(ds.Tables[0].Rows[0]["staff_Code"]);
                    tbenqno.Text = stafcode;
                    tbpname.Text = Convert.ToString(ds.Tables[0].Rows[0]["staff_name"]);
                    tbdept.Text = Convert.ToString(ds.Tables[0].Rows[0]["dept_name"]);
                    // TextBox5.Text = ds.Tables[0].Rows[i]["desig_name"].ToString();
                    //txt_stftype.Text = ds.Tables[0].Rows[i]["stftype"].ToString();
                    //txt_stfcat.Text = ds.Tables[0].Rows[i]["staffcategory"].ToString();
                    //txt_stfjn.Text = ds.Tables[0].Rows[i]["join_date"].ToString();
                    string studClgcode = Convert.ToString(ds.Tables[0].Rows[0]["college_code"]);
                    photo.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + stafcode;
                    if (photo.ImageUrl != "")
                        photo.Visible = true;
                    else
                        photo.Visible = false;
                    ViewState["Clgcode"] = studClgcode;
                }
            }
            else
            {
                tbenqno.Text = "";
                tbpname.Text = "";
                tbdept.Text = "";
                tbborplace.Text = "";
                tbvehno.Text = "";
                tbroute.Text = "";
                tbseatno.Text = "";
                photo.Visible = false;
            }
        }
        catch
        {
        }
    }
    protected void tbpname_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string studname = "";
            string deg = "";
            string dept = "";
            string rollno = "";
            string inti = "";
            string name = Convert.ToString(tbpname.Text);
            if (name != "")
            {
                string[] strstudname = name.Split('-');
                if (strstudname.Length == 5)
                {
                    studname = strstudname[0].ToString();
                    inti = strstudname[1].ToString();
                    deg = strstudname[2].ToString();
                    dept = strstudname[3].ToString();
                    rollno = strstudname[4].ToString();
                }
                string query = "select a.parent_name,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State,d.college_code from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and a.stud_name='" + studname + "'";
                ds = dacces2.select_method_wo_parameter(query, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    tbenqno.Text = Convert.ToString(ds.Tables[0].Rows[0]["Roll_no"]);
                    tbpname.Text = Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]);
                    tbdept.Text = Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]);
                    string studClgcode = Convert.ToString(ds.Tables[0].Rows[0]["college_code"]);
                    string studphoto = "select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + rollno + "')";
                    ds = dacces2.select_method_wo_parameter(studphoto, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        photo.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + rollno;
                        photo.Visible = true;
                    }
                    else
                    {
                        photo.Visible = false;
                    }
                    ViewState["Clgcode"] = studClgcode;
                    feeset();
                }
                else
                {
                    tbenqno.Text = "";
                    tbpname.Text = "";
                    tbdept.Text = "";
                    tbborplace.Text = "";
                    tbvehno.Text = "";
                    tbroute.Text = "";
                    tbseatno.Text = "";
                    photo.Visible = false;
                }
            }
            else
            {
                tbenqno.Text = "";
                tbpname.Text = "";
                tbdept.Text = "";
                tbborplace.Text = "";
                tbvehno.Text = "";
                tbroute.Text = "";
                tbseatno.Text = "";
                photo.Visible = false;
            }
        }
        catch { }
    }
    public void RouteLookup()
    {
        if (tbborplace.Text != "")
        {
            string Rou_Stage_Text = string.Empty;
            string Rou_Stage_Value = string.Empty;
            string stage_name_chk = string.Empty;
            lblerrdate.Visible = false;
            fpapplied.Sheets[0].RowCount = 0;
            string stage_master1 = string.Empty;
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            //stage_master1 = "select * from stage_master where stage_name = '" + tbborplace.Text.Trim() + "'";
            //SqlDataAdapter de_ms = new SqlDataAdapter(stage_master1, con);
            //DataSet ds_ms = new DataSet();
            //de_ms.Fill(ds_ms);
            //if (ds_ms.Tables[0].Rows.Count > 0)
            //{
            //    sqlcmd = "select * from routemaster where sess = 'M' and Stage_Name = '" + ds_ms.Tables[0].Rows[0]["Stage_id"].ToString() + "'";
            //}
            //else
            //{
            //    sqlcmd = "select * from routemaster where sess = 'M' and Stage_Name = '" + tbborplace.Text + "'";
            //}    
            //27nov2013=============================================================================================================
            sqlcmd = " (select distinct v.Veh_ID,r.Route_ID,s.Stage_Name,Stage_id,Arr_Time,Dep_Time,Stages,TotalNo_Seat,nofstudents,nofStaffs from vehicle_master v,routemaster r,stage_master s";
            sqlcmd = sqlcmd + " where v.veh_id=r.veh_id and v.route=r.route_id and convert(varchar(50),s.Stage_id)=(r.Stage_Name)";
            sqlcmd = sqlcmd + " and college_code like'%" + ddlclgstud.SelectedValue + "%' and s.stage_name='" + tbborplace.Text + "' and sess='M')";
            sqlcmd = sqlcmd + " UNION ";
            sqlcmd = sqlcmd + " (select distinct v.Veh_ID,r.Route_ID,s.Stage_Name,Stage_id,Arr_Time,Dep_Time,Stages,TotalNo_Seat,nofstudents,nofStaffs from vehicle_master v,routemaster r,stage_master s";
            sqlcmd = sqlcmd + " where v.veh_id=r.veh_id and v.route=r.route_id and convert(varchar(50),s.Stage_id)=(r.Stage_Name)";
            sqlcmd = sqlcmd + " and (college_code is null or college_code='' or college_code not like'%" + ddlclgstud.SelectedValue + "%') and s.stage_name='" + tbborplace.Text + "' and sess='M')";
            //======================================================================================================================
            dsload = dacces2.select_method_wo_parameter(sqlcmd, "Text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                fpapplied.Visible = true;
                lblerrmainapp1.Visible = false;
                for (loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                {
                    ++fpapplied.Sheets[0].RowCount;
                    //Added By srinath 12/12/2014
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 0].CellType = txt;
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 1].CellType = txt;
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 2].CellType = txt;
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 3].CellType = txt;
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 4].CellType = txt;
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 5].CellType = txt;
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 6].CellType = txt;
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 7].CellType = txt;
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 8].CellType = txt;
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 9].CellType = txt;
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 0].Text = dsload.Tables[0].Rows[loop]["Veh_ID"].ToString();
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 1].Text = dsload.Tables[0].Rows[loop]["Route_ID"].ToString();
                    //Boolean e3 = isNumeric(dsload.Tables[0].Rows[loop]["Stage_Name"].ToString(), System.Globalization.NumberStyles.Integer);
                    //if (e3)
                    //{
                    Rou_Stage_Text = Convert.ToString(dsload.Tables[0].Rows[loop]["Stage_Name"]);
                    Rou_Stage_Value = Convert.ToString(dsload.Tables[0].Rows[loop]["Stage_id"]);
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 2].Text = Rou_Stage_Text;
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 2].Tag = Rou_Stage_Value;
                    //}
                    //else
                    //{
                    //    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["Stage_Name"].ToString();
                    //}
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["Arr_Time"].ToString();
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 4].Text = dsload.Tables[0].Rows[loop]["Dep_Time"].ToString();
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 5].Text = dsload.Tables[0].Rows[loop]["Stages"].ToString();
                    try
                    {
                        string totalseat = string.Empty;
                        string totalseat_student = string.Empty;
                        string totalseat_staff = string.Empty;
                        string AllotedSeatStudent = string.Empty;
                        string AllotedSeatStaff = string.Empty;
                        int totalallotedseat = 0;
                        int RemaningSeat = 0;
                        int RemaningSeat_student = 0;
                        int RemaningSeat_staff = 0;
                        totalseat = Convert.ToString(dsload.Tables[0].Rows[loop]["TotalNo_Seat"].ToString() + "'");
                        totalseat_student = Convert.ToString(dsload.Tables[0].Rows[loop]["nofstudents"].ToString() + "'");
                        totalseat_staff = Convert.ToString(dsload.Tables[0].Rows[loop]["nofStaffs"].ToString() + "'");
                        AllotedSeatStudent = GetFunction("select count(*) from registration where VehID = '" + dsload.Tables[0].Rows[loop]["Veh_ID"].ToString() + "' and Bus_RouteID = '" + dsload.Tables[0].Rows[loop]["Route_ID"].ToString() + "'");
                        AllotedSeatStaff = GetFunction("select count(*) from staffmaster where VehID = '" + dsload.Tables[0].Rows[loop]["Veh_ID"].ToString() + "' and Bus_RouteID = '" + dsload.Tables[0].Rows[loop]["Route_ID"].ToString() + "'");
                        totalallotedseat = Convert.ToInt32(AllotedSeatStudent) + Convert.ToInt32(AllotedSeatStaff);
                        fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 6].Text = AllotedSeatStudent;
                        fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 6].Tag = totalseat_student;
                        fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 7].Text = AllotedSeatStaff;
                        fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 7].Tag = totalseat_staff;
                        if (totalseat != "")
                        {
                            RemaningSeat_student = Convert.ToInt32(fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 6].Tag) - Convert.ToInt32(fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 6].Text);
                            RemaningSeat_staff = Convert.ToInt32(fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 7].Tag) - Convert.ToInt32(fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 7].Text);
                            fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(RemaningSeat_student);
                            fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(RemaningSeat_staff);
                            if (RemaningSeat_student == 0)
                            {
                                fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 8].BackColor = Color.LightGreen;
                            }
                            if (RemaningSeat_staff == 0)
                            {
                                fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 9].BackColor = Color.LightGreen;
                            }
                        }
                        else
                        {
                            fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 8].BackColor = Color.LightGreen;
                            fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 9].BackColor = Color.LightGreen;
                        }
                    }
                    catch
                    {
                    }
                }
                fpapplied.Sheets[0].PageSize = fpapplied.Rows.Count;
                fpapplied.SaveChanges();
                fpapplied.Visible = true;
                lbllerrorlook.Visible = false;
            }
            else
            {
                fpapplied.Visible = false;
                lblerrmainapp1.Visible = true;
                lblerrmainapp1.Text = "No Record(s) Found";
            }
        }
        else
        {
            lblerrdate.Visible = true;
            fpapplied.Visible = false;
            lblerrdate.Text = "Please Enter the Boarding Place";
            return;
        }
    }
    #region button save
    protected void Buttonsave_Click(object sender, EventArgs e)
    {
        try
        {
            //last changed 15/07/2016 by sudhagar
            bool saveflag = false;
            int saveupdate = 0;
            Boolean allotflag = false;
            double cost_total = 0;
            int fd = 0;
            int fyy = 0;
            int fm = 0;
            string dt = "", dt1 = "";
            lbltravelladd.Text = "Add";
            string category = string.Empty;
            string mnthamt = "";
            string year = "";
            string mnthcol = "";
            string mnthvalue = "";
            string mnthcol1 = "";
            string mnthvalue1 = "";
            //check school or college setting 
            checkSchoolSetting();
            string strRoll = string.Empty;
            if (ddlclgstud.Items.Count > 0)
                collegecode = Convert.ToString(ddlclgstud.SelectedValue);
            if (rbdirectapply.Checked == true) //student
            {
                // if (ViewState["Clgcode"] != null)
                //  collegecode = Convert.ToString(ViewState["Clgcode"]);
            }
            if (rbenquiry.Checked == true)
            {
                // collegecode = ddlcollegestaff.SelectedValue.ToString();
                //collegecode = getCblSelectedValue(cblclg);
                //  if (ViewState["Clgcode"] != null)
                // collegecode = Convert.ToString(ViewState["Clgcode"]);
            }
            string getactcode = dacces2.GetFunction("select LinkValue from  InsSettings where LinkName='Current Financial Year' and college_code in('" + collegecode + "')");
            double SchoolCollege = checkSchoolSetting();
            string FinayearFk = string.Empty;
            if (SchoolCollege == 0)
            {
                FinayearFk = " and FinyearFk='" + getactcode + "'";
            }
            if (getactcode.Trim() != "" && getactcode.Trim() != "0")
            {
                string type = "";
                if (tbenqno.Text != "")
                {
                    if (schlSettCode != 0)
                        strRoll = "  r.Roll_No = '" + tbenqno.Text + "'";
                    else
                        strRoll = "  r.Roll_Admit = '" + tbenqno.Text + "'";
                    string Regquery = "select r.Roll_Admit,r.App_No,r.Batch_Year from registration r where" + strRoll + " and r.college_code in('" + collegecode + "')";
                    DataTable dsselectquery1 = dacces2.select_method_wop_table(Regquery, "Text");
                    for (int i1 = 0; i1 < dsselectquery1.Rows.Count; i1++)
                    {
                        Roll_Adm = dsselectquery1.Rows[i1]["App_No"].ToString();
                        BatchFee = dsselectquery1.Rows[i1]["Batch_Year"].ToString();
                    }
                    if (tbborplace.Text == "")
                    {
                        lblerrdate.Visible = true;
                        lblerrdate.Text = "Enter Boarding Place";
                        return;
                    }
                    if (tbvehno.Text == "" || tbroute.Text == "")
                    {
                        lblerrdate.Visible = true;
                        lblerrdate.Text = "Select Boarding In Lookup";
                        return;
                    }
                    if (tbdate.Text == "")
                    {
                        lblerrdate.Visible = true;
                        lblerrdate.Text = "Enter Date";
                        return;
                    }
                    if (rbsemtype.Checked == true)
                        type = "Semester";
                    if (rbstutype.Checked == true)
                        type = "Yearly";
                    if (rbtranfer.Checked == true)
                        type = "Monthly";
                    if (rbtermtype.Checked == true)
                        type = "Term";
                    //added by srinath 24/12/2013
                    if (tbenqno.Text != "" && tbvehno.Text != "")
                    {
                        string stugender = "";
                        string Vechilegender = dacces2.GetFunction(" select gendertype from vehicle_master where Veh_ID='" + tbvehno.Text.ToString() + "'");
                        if (rbdirectapply.Checked == true)
                        {
                            if (schlSettCode != 0)
                                stugender = dacces2.GetFunction("select case when a.sex=0 then 'male' else 'female' end as gender from Registration r,applyn a where a.app_no=r.App_No and r.Roll_No='" + tbenqno.Text.ToString() + "' and r.college_code in('" + collegecode + "')");
                            else
                                stugender = dacces2.GetFunction("select case when a.sex=0 then 'male' else 'female' end as gender from Registration r,applyn a where a.app_no=r.App_No and r.roll_admit='" + tbenqno.Text.ToString() + "' and r.college_code in('" + collegecode + "')");
                        }
                        else if (rbenquiry.Checked == true)
                        {
                            stugender = dacces2.GetFunction("select sex from staff_appl_master a,staffmaster sm where a.appl_no=sm.appl_no and staff_code='" + tbenqno.Text.ToString() + "' and sm.college_code in('" + collegecode + "')");
                        }
                        if (Vechilegender.Trim() != "" && Vechilegender != null && Vechilegender.Trim() != "0")
                        {
                            if (Vechilegender == "1")
                                Vechilegender = "male";
                            else
                                Vechilegender = "female";
                            if (Vechilegender.ToLower().Trim() != stugender.ToLower().Trim())
                            {
                                lblerrdate.Visible = true;
                                lblerrdate.Text = "Selected vechicle is allotted only for " + Vechilegender + "";
                                return;
                            }
                        }
                    }
                    ////feecode and cost amount
                    string StgeID = string.Empty;
                    StgeID = GetFunction("select Stage_id from stage_master where Stage_Name = '" + tbborplace.Text + "'");
                    if (StgeID != "0")
                    {
                        //header and ledger
                        string transset = dacces2.GetFunction(" select LinkValue from New_InsSettings where LinkName='TransportLedgerValue'  and user_code='" + usercode + "' and college_code='" + collegecode + "'");
                        if (transset != "")
                        {
                            string[] leng = transset.Split(',');
                            if (leng.Length == 2)
                            {
                                header_id = Convert.ToString(leng[0]);
                                Fee_Code = Convert.ToString(leng[1]);
                            }
                        }
                        //cost amount
                        Cost = dacces2.GetFunction("select CAST(f.cost AS INT) as Cost from FeeInfo f where f.StrtPlace = '" + StgeID + "' and f.payType = '" + type + "' and college_code='" + collegecode + "'");
                    }
                    fd = int.Parse((tbdate.Text.Substring(0, 2).ToString()));
                    fyy = int.Parse((tbdate.Text.Substring(6, 4).ToString()));
                    fm = int.Parse((tbdate.Text.Substring(3, 2).ToString()));
                    dt1 = fm + "-" + fd + "-" + fyy;
                    Hashtable AddMonthyear = new Hashtable();
                    if (rbdirectapply.Checked == true) //student
                    {
                        string selIncl = dacces2.GetFunction("select value from Master_Settings where settings='TransportFeeIncludeAllotmentSettings'  and usercode='" + usercode + "'");
                        if (selIncl == "1")
                        {
                            //with transport fees
                            #region student
                            //fee Category
                            string selsctfeecate = dacces2.GetFunction("select distinct current_semester from registration r where " + strRoll + " and r.college_code in('" + collegecode + "')");
                            //yearwise
                            if (rbstutype.Checked == true)
                            {
                                #region year
                                if (selsctfeecate == "1" || selsctfeecate == "2")
                                    semval = "1 Year";
                                else if (selsctfeecate == "3" || selsctfeecate == "4")
                                    semval = "2 Year";
                                else if (selsctfeecate == "5" || selsctfeecate == "6")
                                    semval = "3 Year";
                                else if (selsctfeecate == "7" || selsctfeecate == "8")
                                    semval = "4 Year";
                                #endregion
                            }
                            //semesterwise
                            else if (rbsemtype.Checked == true)
                            {
                                #region semester
                                if (selsctfeecate == "1")
                                    semval = "1 Semester";
                                if (selsctfeecate == "2")
                                    semval = "2 Semester";
                                if (selsctfeecate == "3")
                                    semval = "3 Semester";
                                if (selsctfeecate == "4")
                                    semval = "4 Semester";
                                if (selsctfeecate == "5")
                                    semval = "5 Semester";
                                if (selsctfeecate == "6")
                                    semval = "6 Semester";
                                if (selsctfeecate == "7")
                                    semval = "7 Semester";
                                if (selsctfeecate == "8")
                                    semval = "8 Semester";
                                if (selsctfeecate == "9")
                                    semval = "9 Semester";
                                #endregion
                            }
                            //monthwise
                            else if (rbtranfer.Checked == true)
                            {
                                #region month
                                //CheckJairam
                                //  string MonthValue = Convert.ToString(Session["MonthValue"]);
                                string MonthValue = dacces2.GetFunction("select Month_Value from FeeInfo f where f.StrtPlace = '" + StgeID + "' and f.payType = '" + type + "' and college_code='" + collegecode + "'");
                                if (MonthValue.Trim() != "" && MonthValue.Trim() != "0")
                                {
                                    string[] SplitMonth = MonthValue.Split(',');
                                    if (SplitMonth.Length > 0)
                                    {
                                        for (int Splen = 0; Splen < SplitMonth.Length; Splen++)
                                        {
                                            if (SplitMonth[Splen].ToString() != "")
                                            {
                                                string[] SecondSpit = SplitMonth[Splen].Split(':');
                                                if (SecondSpit.Length > 1)
                                                {
                                                    if (mnthamt.Trim() != "")
                                                    {
                                                        mnthamt += "," + SecondSpit[0] + ":" + SecondSpit[1] + ":" + SecondSpit[2];
                                                        mnthcol += "," + SecondSpit[0] + ":" + SecondSpit[1] + ":" + SecondSpit[2];
                                                    }
                                                    else
                                                    {
                                                        mnthamt = "," + SecondSpit[0] + ":" + SecondSpit[1] + ":" + SecondSpit[2];
                                                        mnthcol = SecondSpit[0] + ":" + SecondSpit[1] + ":" + SecondSpit[2];
                                                    }
                                                    if (!AddMonthyear.Contains(Convert.ToString(SecondSpit[0] + ":" + SecondSpit[1])))
                                                    {
                                                        AddMonthyear.Add(Convert.ToString(SecondSpit[0] + ":" + SecondSpit[1]), Convert.ToString(SecondSpit[2]));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (ddlmonth.SelectedItem.Text != "Month")
                                        month = Convert.ToString(ddlmonth.SelectedItem.Value);
                                    if (ddlyear.SelectedItem.Text != "Year")
                                        year = Convert.ToString(ddlyear.SelectedItem.Text);
                                    mnthamt = "," + month + ":" + year + ":" + Cost;
                                    mnthcol = month + ":" + year + ":" + Cost;
                                }
                                //  mnthvalue = "'" + mnthamt + "'";
                                //  mnthcol1 = ",FeeAmountMonthly";
                                //  mnthvalue1 = ",'" + mnthamt + "'";
                                semval = feecatValue(selsctfeecate);
                                string[] spl_sem = semval.Split(' ');
                                string curr_sem = spl_sem[0].ToString();
                                if (curr_sem != "")
                                {
                                    //if (Convert.ToInt32(curr_sem) % 2 == 0)
                                    //    category = "Even";
                                    //else
                                    //    category = "Odd";
                                }
                                #endregion
                            }
                            if (rbtermtype.Checked == true)
                            {
                                #region year
                                if (selsctfeecate == "1")
                                    semval = "Term 1";
                                else if (selsctfeecate == "2")
                                    semval = "Term 2";
                                else if (selsctfeecate == "3")
                                    semval = "Term 3";
                                else if (selsctfeecate == "4")
                                    semval = "Term 4";
                                #endregion
                            }
                            //feecatagory
                            sqlcmd = dacces2.GetFunction("select distinct textcode from textvaltable where textcriteria ='FEECA' and textval='" + semval + "' and college_code='" + collegecode + "'");
                            if (sqlcmd != "0")
                                tcode = Convert.ToString(sqlcmd);
                            //setting rights
                            string getlink = dacces2.GetFunction("select LinkValue from inssettings where linkname = 'Transport Link' and college_code ='" + collegecode + "'");
                            if (getlink == "1")
                            {
                                #region  save
                                if (Roll_Adm != "" && Fee_Code != "" && Cost != "" && Cost != "0" && header_id != "" && tcode != "")
                                {
                                    if (rbtranfer.Checked == false)
                                    {
                                        string paidamt = dacces2.GetFunction("");
                                        string querystu1 = " if exists (select * from FT_FeeAllot where App_No ='" + Roll_Adm + "' and LedgerFK='" + Fee_Code + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + tcode + "' " + FinayearFk + " ) update FT_FeeAllot set FeeAmount='" + Cost + "',TotalAmount ='" + Cost + "' ,BalAmount ='" + Cost + "'-isnull(PaidAmount,'0')   where App_No ='" + Roll_Adm + "' and LedgerFK='" + Fee_Code + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + tcode + "' " + FinayearFk + " else insert into FT_FeeAllot (App_No,LedgerFK,HeaderFK,FinYearFK,AllotDate,FeeAmount,FeeCategory,PayStartDate,FineAmount,DeductAmount,TotalAmount,BalAmount,MemType,PayMode,DeductReason,FromGovtAmt)  values ('" + Roll_Adm + "','" + Fee_Code + "','" + header_id + "','" + getactcode + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + Cost + "','" + tcode + "','',0,0,'" + Cost + "','" + Cost + "','1','1',0,0)";
                                        saveupdate = dacces2.update_method_wo_parameter(querystu1, "Text");
                                    }
                                    else
                                    {
                                        #region month
                                        string fnlmnth = "";
                                        int balamt = 0;
                                        string Feemnth = dacces2.GetFunction("select FeeAmountMonthly from FT_FeeAllot where App_No='" + Roll_Adm + "' and FeeCategory ='" + tcode + "' and LedgerFK = '" + Fee_Code + "' " + FinayearFk + "");
                                        if (Feemnth != "" && Feemnth != "0")
                                        {
                                            string[] value = Feemnth.Split(',');
                                            for (int i = 0; i < value.Length; i++)
                                            {
                                                string[] mnthval = value[i].Split(':');
                                                {
                                                    if (mnthval.Length > 0)
                                                    {
                                                        //2;2016;200,3;2016:300
                                                        if (AddMonthyear.ContainsKey(Convert.ToString(mnthval[0] + ":" + mnthval[1]))) //mnthval[0] == month && mnthval[1] == year
                                                        {
                                                            mnthamt = "";
                                                            Cost = Convert.ToString(AddMonthyear[Convert.ToString(mnthval[0] + ":" + mnthval[1])]);
                                                            if (Cost == mnthval[2])
                                                            {
                                                                mnthval[2] = Cost;
                                                                Cost = "0";
                                                            }
                                                            else if (Convert.ToInt32(Cost) > Convert.ToInt32(mnthval[2]))
                                                            {
                                                                balamt = Convert.ToInt32(Cost) - Convert.ToInt32(mnthval[2]);
                                                                Cost = Convert.ToString(balamt);
                                                                mnthval[2] = Cost;
                                                            }
                                                            else if (Convert.ToInt32(Cost) < Convert.ToInt32(mnthval[2]))
                                                            {
                                                                int val = Convert.ToInt32(Cost);
                                                                balamt = Convert.ToInt32(Cost) - Convert.ToInt32(mnthval[2]);
                                                                Cost = Convert.ToString(balamt);
                                                                mnthval[2] = Convert.ToString(val);
                                                            }
                                                            if (fnlmnth == "")
                                                                fnlmnth = mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                                            else
                                                                fnlmnth = fnlmnth + "," + mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                                            AddMonthyear.Remove(Convert.ToString(mnthval[0] + ":" + mnthval[1]));
                                                        }
                                                        else
                                                            if (fnlmnth == "")
                                                            {
                                                                string valuev = mnthval[0].ToString();
                                                                string valuev1 = mnthval[1].ToString();
                                                                string valuev3 = mnthval[2].ToString();
                                                                fnlmnth = mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                                            }
                                                            else
                                                                fnlmnth = fnlmnth + "," + mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                                    }
                                                }
                                            }
                                            if (fnlmnth != "")
                                            {
                                                if (AddMonthyear.Count > 0)
                                                {
                                                    string ConCat = string.Empty;
                                                    foreach (DictionaryEntry Di in AddMonthyear)
                                                    {
                                                        string keyvalue = Convert.ToString(Di.Key);
                                                        string Value = Convert.ToString(Di.Value);
                                                        if (ConCat.Trim() != "")
                                                        {
                                                            ConCat += "," + keyvalue + ":" + Value;
                                                        }
                                                        else
                                                        {
                                                            ConCat = keyvalue + ":" + Value;
                                                        }
                                                    }
                                                    if (ConCat.Trim() != "")
                                                    {
                                                        fnlmnth += "," + ConCat;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                            fnlmnth = mnthcol;
                                        //2:2016:100,3:2016:200
                                        string querystu1 = " if exists (select * from FT_FeeAllot where App_No ='" + Roll_Adm + "' and LedgerFK='" + Fee_Code + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + tcode + "' " + FinayearFk + ") update FT_FeeAllot set FeeAmount='" + Cost + "',TotalAmount ='" + Cost + "' ,BalAmount ='" + Cost + "'-isnull(PaidAmount,'0'), FeeAmountMonthly='" + fnlmnth + "'  where App_No ='" + Roll_Adm + "' and LedgerFK='" + Fee_Code + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + tcode + "' " + FinayearFk + "  else insert into FT_FeeAllot (App_No,LedgerFK,HeaderFK,FinYearFK,AllotDate,FeeAmount,FeeCategory,PayStartDate,FineAmount,DeductAmount,TotalAmount,BalAmount,MemType,PayMode,DeductReason,FromGovtAmt,FeeAmountMonthly)  values ('" + Roll_Adm + "','" + Fee_Code + "','" + header_id + "','" + getactcode + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + Cost + "','" + tcode + "','',0,0,'" + Cost + "','" + Cost + "','1','1',0,0" + mnthvalue1 + ",'" + fnlmnth + "')";
                                        saveupdate = dacces2.update_method_wo_parameter(querystu1, "Text");
                                        string allotpk = dacces2.GetFunction(" select FeeAllotPK from FT_FeeAllot  where App_No ='" + Roll_Adm + "' and LedgerFK='" + Fee_Code + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + tcode + "' " + FinayearFk + "");
                                        if (allotpk != "" && getactcode != "" && fnlmnth.Trim() != "")
                                        {
                                            string[] FistSplit = fnlmnth.Split(',');
                                            if (FistSplit.Length > 0)
                                            {
                                                for (int itFisrt = 0; itFisrt < FistSplit.Length; itFisrt++)
                                                {
                                                    if (FistSplit[itFisrt].Trim() != "")
                                                    {
                                                        string[] SecondSplit = FistSplit[itFisrt].Split(':');
                                                        if (SecondSplit.Length > 1)
                                                        {
                                                            month = Convert.ToString(SecondSplit[0]);
                                                            year = Convert.ToString(SecondSplit[1]);
                                                            Cost = Convert.ToString(SecondSplit[2]);
                                                            if (month.Trim() != "" && year.Trim() != "" && Cost.Trim() != "")
                                                            {
                                                                string InsertQ = "if exists(select * from FT_FeeallotMonthly where FeeAllotPK='" + allotpk + "' and AllotMonth='" + month + "' and AllotYear='" + year + "' and FinYearFK='" + getactcode + "' " + FinayearFk + ")update FT_FeeallotMonthly set AllotAmount='" + Cost + "',BalAmount='" + Cost + "'-isnull(PaidAmount,'0') where FeeAllotPK='" + allotpk + "' and AllotMonth='" + month + "' and AllotYear='" + year + "' and FinYearFK='" + getactcode + "' else insert into FT_FeeallotMonthly (FeeAllotPK,AllotMonth,AllotYear,AllotAmount,FinYearFK,BalAmount) values('" + allotpk + "','" + month + "','" + year + "','" + Cost + "','" + getactcode + "','" + Cost + "')";
                                                                int ins = dacces2.update_method_wo_parameter(InsertQ, "Text");
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    if (saveupdate > 0)
                                        allotflag = true;
                                }
                                else
                                {
                                    imgAlert.Visible = true;
                                    lbl_alert.Text = "There is no Fees available";
                                }
                                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('There is no Fees available')", true);
                            }
                            else
                            {
                                imgAlert.Visible = true;
                                lbl_alert.Text = "There is no Fees available";
                            }
                            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('There is no rights available')", true);
                                #endregion
                            //registration update
                            string strRollAdmit = string.Empty;
                            if (schlSettCode != 0)
                                strRollAdmit = " Roll_No='" + tbenqno.Text + "'";
                            else
                                strRollAdmit = " roll_admit='" + tbenqno.Text + "'";
                            if (allotflag == true)
                            {
                                string seat_no = string.Empty;
                                if (tbseatno.Text == "")
                                    seat_no = "Com";
                                else
                                    seat_no = tbseatno.Text;
                                string querystu;
                                querystu = "update registration set Bus_RouteID='" + tbroute.Text + "',Boarding='" + StgeID + "',VehID='" + tbvehno.Text + "',Seat_No='" + seat_no + "',Trans_PayType='" + type + "',Traveller_Date = '" + dt1 + "',IsCanceledStage='0' where " + strRollAdmit + " and Stud_Name='" + tbpname.Text + "' and college_code in('" + collegecode + "')";
                                saveupdate = dacces2.update_method_wo_parameter(querystu, "text");
                                if (Buttonsave.Text == "Save")
                                // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
                                {
                                    tbenqno.Text = "";
                                    tbpname.Text = "";
                                    tbdept.Text = "";
                                    tbvehno.Text = "";
                                    tbroute.Text = "";
                                    tbseatno.Text = "";
                                    tbborplace.Text = "";
                                    photo.ImageUrl = "";
                                    imgAlert.Visible = true;
                                    lbl_alert.Text = "Saved Successfully";
                                }
                                else
                                {
                                    tbenqno.Text = "";
                                    tbpname.Text = "";
                                    tbdept.Text = "";
                                    tbvehno.Text = "";
                                    tbroute.Text = "";
                                    tbseatno.Text = "";
                                    tbborplace.Text = "";
                                    photo.ImageUrl = "";
                                    imgAlert.Visible = true;
                                    lbl_alert.Text = "Updated Successfully";
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            //without fees only route update the student
                            string strRollAdmit = string.Empty;
                            if (schlSettCode != 0)
                                strRollAdmit = " Roll_No='" + tbenqno.Text + "'";
                            else
                                strRollAdmit = " roll_admit='" + tbenqno.Text + "'";
                            string seat_no = string.Empty;
                            if (tbseatno.Text == "")
                                seat_no = "Com";
                            else
                                seat_no = tbseatno.Text;
                            string querystu;
                            querystu = "update registration set Bus_RouteID='" + tbroute.Text + "',Boarding='" + StgeID + "',VehID='" + tbvehno.Text + "',Seat_No='" + seat_no + "',Trans_PayType='" + type + "',Traveller_Date = '" + dt1 + "',IsCanceledStage='0' where " + strRollAdmit + " and Stud_Name='" + tbpname.Text + "' and college_code in('" + collegecode + "')";
                            saveupdate = dacces2.update_method_wo_parameter(querystu, "text");
                            if (saveupdate > 0)
                            {
                                tbenqno.Text = "";
                                tbpname.Text = "";
                                tbdept.Text = "";
                                tbvehno.Text = "";
                                tbroute.Text = "";
                                tbseatno.Text = "";
                                tbborplace.Text = "";
                                photo.ImageUrl = "";
                                imgAlert.Visible = true;
                                lbl_alert.Text = "Saved Successfully";
                            }
                        }
                    }
                    else if (rbenquiry.Checked == true) //staff
                    {
                        #region staff
                        string placver = Lblplace_Value.Text.ToString();
                        if (placver != "" && placver != null)
                        {
                            if (tbenqno.Text != "" && tbpname.Text != "" && Fee_Code != "")
                            {
                                string querystu1 = "update staffmaster set Bus_RouteID='" + tbroute.Text + "',Boarding='" + Lblplace_Value.Text + "',VehID='" + tbvehno.Text + "',Seat_No='" + tbseatno.Text + "',Traveller_Date = '" + dt1 + "',IsCanceledStage='0' where staff_code='" + tbenqno.Text + "' and staff_name='" + tbpname.Text + "' and college_code in('" + collegecode + "')";
                                int insr = dacces2.update_method_wo_parameter(querystu1, "Text");
                                string rights = dacces2.GetFunction("select LinkValue from inssettings where linkname = 'Transport Link' and college_code ='" + collegecode + "'");
                                tcode = fee_cate.SelectedValue.ToString();
                                if (rights.Trim() == "1")
                                {
                                    //string concesamount = txtconcession.Text;
                                    //Double convalues = 0;
                                    //double.TryParse(Convert.ToString(txtconcession.Text), out convalues);
                                    //if (Convert.ToDouble(Cost) < convalues)
                                    //{
                                    //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Enter Concession amount less than Fee Amount')", true);
                                    //    return;
                                    //}                                
                                    if (Buttonsave.Text == "Save")
                                    {
                                        #region Save
                                        //try
                                        //{
                                        //    string queryUpdate1;                                            
                                        //    queryUpdate1 = "select * from FT_FeeAllot where App_No='" + Roll_Adm + "' and FeeCategory='" + tcode + "' and LedgerFK = '" + Fee_Code + "'";
                                        //    DataTable dtnewupdate = dacces2.select_method_wop_table(queryUpdate1, "text");
                                        //    allotflag = false;
                                        //    if (dtnewupdate.Rows.Count == 0)
                                        //    {
                                        //        //Modified by Srinath 17/7/2014
                                        //        if (convalues > 0)
                                        //        {
                                        //            Double total = Convert.ToDouble(Cost) - convalues;
                                        //            querystu1 = "insert into FT_FeeAllot (App_No,LedgerFK,HeaderFK,FinYearFK,AllotDate,FeeAmount,FeeCategory,PayStartDate,FineAmount,DeductAmount,TotalAmount,BalAmount,MemType,PayMode,DeductReason,FromGovtAmt) values ('" + Roll_Adm + "','" + Fee_Code + "','" + header_id + "','" + getactcode + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + Cost + "','" + tcode + "','',0,'" + convalues + "','" + total + "','" + Cost + "','1','1',0,0)";
                                        //        }
                                        //        else
                                        //        {
                                        //            querystu1 = "insert into FT_FeeAllot (App_No,LedgerFK,HeaderFK,FinYearFK,AllotDate,FeeAmount,FeeCategory,PayStartDate,FineAmount,DeductAmount,TotalAmount,BalAmount,MemType,PayMode,DeductReason,FromGovtAmt) values ('" + Roll_Adm + "','" + Fee_Code + "','" + header_id + "','" + getactcode + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + Cost + "','" + tcode + "','',0,0,'" + Cost + "','" + Cost + "','1','1',0,0)";
                                        //        }
                                        //        int insert = dacces2.update_method_wo_parameter(querystu1, "text");
                                        //        allotflag = true;
                                        //    }
                                        //}
                                        //catch
                                        //{
                                        //}
                                        #endregion
                                    }
                                    else if (Buttonsave.Text == "Update")//Aruna 18Feb2014===============================
                                    {
                                        #region update old
                                        //try
                                        //{
                                        //    double Already_allot_amt = 0;
                                        //    double Already_allot_tot_amt = 0;
                                        //    double Aleady_allot_duduct_amt = 0;
                                        //    double paid_amout = 0;
                                        //    double Already_paid_excess_amount = 0;
                                        //    string queryUpdate1 = "select * from FT_FeeAllot where App_No='" + Roll_Adm + "' and FeeCategory='" + tcode + "' and LedgerFK = '" + Fee_Code + "'";
                                        //    DataTable dtnewupdate = dacces2.select_method_wop_table(queryUpdate1, "Text");
                                        //    allotflag = false;
                                        //    if (dtnewupdate.Rows.Count > 0)
                                        //    {
                                        //        Already_allot_amt = Convert.ToDouble(dtnewupdate.Rows[0]["FeeAmount"]);
                                        //        Already_allot_tot_amt = Convert.ToDouble(dtnewupdate.Rows[0]["TotalAmount"]);
                                        //        Aleady_allot_duduct_amt = Convert.ToDouble(dtnewupdate.Rows[0]["DeductAmount"]);
                                        //        string name = tbenqno.Text + "-" + tbpname.Text;
                                        //        string paidamt = GetFunction("select isnull(sum(credit),0) as paid from FT_FinDailyTransaction where FeeCategory='" + tcode + "' and LedgerFK = '" + Fee_Code + "' and MemName='" + name + "' and debit=0 ");   //and studorothers=0 and vouchertype=1
                                        //        paid_amout = Convert.ToDouble(paidamt);
                                        //        if (paid_amout == 0)
                                        //        {
                                        //            con.Close();
                                        //            con.Open();
                                        //            double allot_amt = Convert.ToDouble(Cost);
                                        //            double allot_tot = 0;
                                        //            Boolean flag_status = false;
                                        //            if (allot_amt >= Aleady_allot_duduct_amt)
                                        //            {
                                        //                allot_tot = allot_amt - Aleady_allot_duduct_amt;
                                        //            }
                                        //            else
                                        //            {
                                        //                allot_tot = 0;
                                        //            }
                                        //            if (paid_amout > 0)
                                        //            {
                                        //                if (allot_tot == paid_amout)
                                        //                {
                                        //                    flag_status = true;
                                        //                }
                                        //                else if (paid_amout >= allot_tot)
                                        //                {
                                        //                    flag_status = true;
                                        //                    Already_paid_excess_amount = paid_amout - allot_tot;
                                        //                }
                                        //                else
                                        //                {
                                        //                    flag_status = false;
                                        //                }
                                        //            }
                                        //            else
                                        //            {
                                        //                flag_status = false;
                                        //            }
                                        //            querystu1 = "update FT_FeeAllot set FeeAmount=" + allot_amt + ",DeductAmount=" + Aleady_allot_duduct_amt + ",TotalAmount=" + allot_tot + " where App_No='" + Roll_Adm + "' and FeeCategory='" + tcode + "' and LedgerFK = '" + Fee_Code + "'";
                                        //            int insert = dacces2.update_method_wo_parameter(querystu1, "text");
                                        //            flag_status = false;
                                        //        }
                                        //    }
                                        //    else if (dtnewupdate.Rows.Count == 0)
                                        //    {
                                        //        //modified by srinath 17/7/2014
                                        //        if (convalues > 0)
                                        //        {
                                        //            Double total = Convert.ToDouble(Cost) - convalues;
                                        //            querystu1 = "insert into FT_FeeAllot (App_No,LedgerFK,HeaderFK,FinyearFK,AllotDate,FeeAmount,FeeCategory,PayStartDate,FineAmount,DeductAmount,TotalAmount,BalAmount,MemType,PayMode,DeductReason,FromGovtAmt) values ('" + Roll_Adm + "','" + Fee_Code + "','" + header_id + "','" + getactcode + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + Cost + "','" + tcode + "','',0,'" + convalues + "','" + total + "','" + Cost + "','1','1',0,0)";
                                        //        }
                                        //        else
                                        //        {
                                        //            querystu1 = "insert into FT_FeeAllot (App_No,LedgerFK,HeaderFK,FinYearFK,AllotDate,FeeAmount,FeeCategory,PayStartDate,FineAmount,DeductAmount,TotalAmount,BalAmount,MemType,PayMode,DeductReason,FromGovtAmt) values ('" + Roll_Adm + "','" + Fee_Code + "','" + header_id + "','" + getactcode + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + Cost + "','" + tcode + "','',0,0,'" + Cost + "','" + Cost + "','1','1',0,0)";
                                        //        }
                                        //        int insert = dacces2.update_method_wo_parameter(querystu1, "text");
                                        //        allotflag = true;
                                        //    }
                                        //}
                                        //catch
                                        //{
                                        //}
                                        #endregion
                                    }
                                }
                            }
                            if (Buttonsave.Text == "Save")
                            {
                                tbenqno.Text = "";
                                tbpname.Text = "";
                                tbdept.Text = "";
                                tbvehno.Text = "";
                                tbroute.Text = "";
                                tbseatno.Text = "";
                                tbborplace.Text = "";
                                photo.ImageUrl = "";
                                imgAlert.Visible = true;
                                lbl_alert.Text = "Saved Successfully";
                            }
                            else
                            {
                                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Updated successfully')", true);
                                imgAlert.Visible = true;
                                lbl_alert.Text = "Updated successfully";
                            }
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Exact vehcile and Route')", true);
                            imgAlert.Visible = true;
                            lbl_alert.Text = "Please Select Exact vehcile and Route";
                        }
                        #endregion
                    }
                    // LoadMainEnquiry();
                    //RouteLookup();
                    //clear();
                }
                else
                {
                    lblerrdate.Visible = true;
                    lblerrdate.Text = "Enter RollNumber";
                    return;
                }
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Finance Year')", true);
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Finance Year";
            }
        }
        catch (Exception ex)
        {
            lblerrdate.Visible = true;
            lblerrdate.Text = ex.ToString();
        }
    }
    #endregion
    protected void Buttondelete_Click(object sender, EventArgs e)
    {
        Buttonsave.Text = "Save";
        // Btn_Delete.Enabled = false;
        lbltravelladd.Text = "Add";
        tbenqno.Text = "";
        tbpname.Text = "";
        tbdept.Text = "";
        tbborplace.Text = "";
        tbvehno.Text = "";
        tbroute.Text = "";
        tbseatno.Text = "";
        photo.Visible = true;
        tbenqno.Enabled = true;
        tbpname.Enabled = true;
        tbdept.Enabled = true;
        tbborplace.Enabled = true;
        tbvehno.Enabled = true;
        tbroute.Enabled = true;
        tbseatno.Enabled = true;
        tbdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtfeeset.Enabled = false;
        btnfeeset.Enabled = false;
        enqbtn.Enabled = true;
        clear();
    }
    protected void fpapplied_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string activerow = fpapplied.ActiveSheetView.ActiveRow.ToString();
        string activecol = fpapplied.ActiveSheetView.ActiveColumn.ToString();
        Cellclick1 = true;
    }
    protected void fpapplied_SelectedIndexChanged(object sender, EventArgs e)
    {
        fpapplied.Sheets[0].AutoPostBack = true;
        fpapplied.SaveChanges();
        if (Cellclick1 == true)
        {
            string activerow = "";
            string activecol = "";
            activerow = fpapplied.ActiveSheetView.ActiveRow.ToString();
            activecol = fpapplied.ActiveSheetView.ActiveColumn.ToString();
            int ar;
            int ac;
            ar = Convert.ToInt32(activerow.ToString());
            ac = Convert.ToInt32(activecol.ToString());
            if (ar != -1)
            {
                Panellookup.Visible = true;
                string Route_ID = "";
                string stagename = "";
                string stage_value = string.Empty;
                string trvel_id = "";
                string totalseat = string.Empty;
                string AllotedSeatStudent = string.Empty;
                string AllotedSeatStaff = string.Empty;
                int totalallotedseat = 0;
                int RemaningSeat = 0;
                Route_ID = fpapplied.Sheets[0].Cells[ar, 1].Text.ToString();
                stagename = fpapplied.Sheets[0].Cells[ar, 2].Text.ToString();
                stage_value = fpapplied.Sheets[0].Cells[ar, 2].Tag.ToString();
                trvel_id = fpapplied.Sheets[0].Cells[ar, 0].Text.ToString();
                tbroute.Text = Route_ID.ToString();
                //tbborplace.Text = stagename.ToString();
                tbvehno.Text = trvel_id.ToString();
                Lblplace_Value.Text = stage_value.ToString();
                //total seat count
                try
                {
                    totalseat = GetFunction("select TotalNo_Seat from vehicle_master where Veh_ID = '" + trvel_id + "'");
                    lbltotalseat.Text = totalseat;
                    AllotedSeatStudent = GetFunction("select count(*) from registration where VehID = '" + trvel_id + "' and Bus_RouteID = '" + Route_ID + "'");
                    AllotedSeatStaff = GetFunction("select count(*) from staffmaster where VehID = '" + trvel_id + "' and Bus_RouteID = '" + Route_ID + "'");
                    totalallotedseat = Convert.ToInt32(AllotedSeatStudent) + Convert.ToInt32(AllotedSeatStaff);
                    lblallotedSeat.Text = totalallotedseat.ToString();
                    if (totalseat != "")
                    {
                        RemaningSeat = Convert.ToInt32(lbltotalseat.Text) - Convert.ToInt32(lblallotedSeat.Text);
                        lblremaingSeat.Text = Convert.ToString(RemaningSeat);
                    }
                    else
                    {
                    }
                }
                catch
                {
                }
            }
            Cellclick1 = false;
            Panellookup.Visible = false;
        }
    }
    protected void btncloselook_Click(object sender, EventArgs e)
    {
        Panellookup.Visible = false;
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //bindtype();
        bindcourse();
        bindBranch();
        // bindSem();
    }
    public void bindBatch1()
    {
        int year2;
        year2 = Convert.ToInt16(DateTime.Today.Year);
        ddlbatch.Items.Clear();
        for (int l = 0; l <= 10; l++)
        {
            ddlbatch.Items.Add(Convert.ToString(year2 - l));
        }
    }
    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindBranch();
    }
    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindcourse();
        bindBranch();
    }
    protected void ddlBranch1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    public void bindcourse()
    {
        string usercode = Session["usercode"].ToString();
        DAccess2 da1 = new DAccess2();
        DataSet ds1 = new DataSet();
        ht.Clear();
        string strisstaff = Session["Staff_Code"].ToString();
        ddlDegree.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        ht.Clear();
        ht.Add("single_user", singleuser);
        ht.Add("group_code", group_user);
        if (strisstaff.ToLower().Trim() == "")
        {
            ht.Add("college_code", ddlcollegenew.SelectedValue.ToString());
        }
        else
        {
            ht.Add("college_code", ddlcollegenew.SelectedValue.ToString());
        }
        ht.Add("user_code", usercode);
        ds1 = da1.select_method("bind_degree", ht, "sp");
        if (ds1.Tables[0].Rows.Count > 0)
        {
            ddlDegree.Enabled = true;
            ddlDegree.Items.Clear();
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                ddlDegree.Items.Insert(i, new ListItem(Convert.ToString(ds1.Tables[0].Rows[i]["course_name"]), Convert.ToString(ds1.Tables[0].Rows[i]["course_id"])));
            }
        }
        else
        {
            ddlDegree.Enabled = false;
        }
        //{
        //    ddlDegree.Items.Clear();
        //    usercode = Session["usercode"].ToString();
        //    collegecode = Session["collegecode"].ToString();
        //    singleuser = Session["single_user"].ToString();
        //    group_user = Session["group_code"].ToString();
        //    if (group_user.Contains(';'))
        //    {
        //        string[] group_semi = group_user.Split(';');
        //        group_user = group_semi[0].ToString();
        //    }
        //    hat.Clear();
        //    hat.Add("single_user", singleuser);
        //    hat.Add("group_code", group_user);
        //    hat.Add("college_code", collegecode);
        //    hat.Add("user_code", usercode);
        //    ds = dacces2.select_method("bind_degree", hat, "sp");
        //    int count1 = ds.Tables[0].Rows.Count;
        //    if (count1 > 0)
        //    {
        //        ddlDegree.DataSource = ds;
        //        ddlDegree.DataTextField = "course_name";
        //        ddlDegree.DataValueField = "course_id";
        //        ddlDegree.DataBind();
        //    }
        //}
    }
    public void bindtype()
    {
    }
    public void bindBranch()
    {
        try
        {
            DAccess2 da1 = new DAccess2();
            DataSet ds1 = new DataSet();
            string strisstaff = Session["Staff_Code"].ToString();
            ddlBranch1.Items.Clear();
            ht.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ht.Add("single_user", singleuser);
            ht.Add("group_code", group_user);
            ht.Add("course_id", ddlDegree.SelectedValue);
            if (strisstaff.ToLower().Trim() == "")
            {
                ht.Add("college_code", ddlcollegenew.SelectedValue.ToString());
            }
            else
            {
                ht.Add("college_code", ddlcollegenew.SelectedValue.ToString());
            }
            ht.Add("user_code", usercode);
            ds1 = da1.select_method("bind_branch", ht, "sp");
            if (ds1.Tables.Count > 0)
            {
                //if (ds1.Tables.Count > 0)
                //{
                //    if (ds1.Tables[0].Rows.Count > 0)
                //    {
                //        ddlBranch1.Enabled = true;
                //        ddlBranch1.Items.Clear();
                //        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                //        {
                //            ddlBranch1.Items.Insert(i, new ListItem(Convert.ToString(ds1.Tables[0].Rows[i]["Acronym"]), Convert.ToString(ds1.Tables[0].Rows[i]["degree_code"])));
                //        }
                //        ddlBranch1.SelectedIndex = 0;
                //    }
                //    else
                //    {
                //        ddlBranch1.Enabled = false;
                //    }
                //}
                ddlBranch1.DataSource = ds1;
                ddlBranch1.DataTextField = "Acronym";
                ddlBranch1.DataValueField = "degree_code";
                ddlBranch1.DataBind();
            }
        }
        catch
        {
        }
        //ddlBranch1.Items.Clear();
        //hat.Clear();
        //usercode = Session["usercode"].ToString();
        //collegecode = Session["collegecode"].ToString();
        //singleuser = Session["single_user"].ToString();
        //group_user = Session["group_code"].ToString();
        //if (group_user.Contains(';'))
        //{
        //    string[] group_semi = group_user.Split(';');
        //    group_user = group_semi[0].ToString();
        //}
        //hat.Add("single_user", singleuser);
        //hat.Add("group_code", group_user);
        //hat.Add("course_id", ddlDegree.SelectedValue);
        //hat.Add("college_code", collegecode);
        //hat.Add("user_code", usercode);
        //ds = dacces2.select_method("bind_branch", hat, "sp");
        //int count2 = ds.Tables[0].Rows.Count;
        //if (count2 > 0)
        //{
        //    ddlBranch1.DataSource = ds;
        //    ddlBranch1.DataTextField = "dept_name";
        //    ddlBranch1.DataValueField = "degree_code";
        //    ddlBranch1.DataBind();
        //}
    }
    protected void FpSpread1_CellClick(object sender, EventArgs e)
    {
        string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
        fpcellclick = true;
    }
    protected void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (fpcellclick == true)
        {
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.SaveChanges();
            if (fpcellclick == true)
            {
                string activerow = "";
                string activecol = "";
                activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
                int ar;
                int ac;
                ar = Convert.ToInt32(activerow.ToString());
                ac = Convert.ToInt32(activecol.ToString());
                if (ar != -1)
                {
                    Panellookup.Visible = false;
                    string RollNo = "";
                    string studname = "";
                    string Dept = "";
                    string appno = "";
                    string clgcode = "";
                    RollNo = FpSpread1.Sheets[0].Cells[ar, 1].Text.ToString();
                    appno = FpSpread1.Sheets[0].Cells[ar, 1].Tag.ToString();
                    studname = FpSpread1.Sheets[0].Cells[ar, 3].Text.ToString();
                    Dept = FpSpread1.Sheets[0].Cells[ar, 0].Text.ToString();
                    clgcode = FpSpread1.Sheets[0].Cells[ar, 3].Note.ToString();
                    tbenqno.Text = RollNo.ToString();
                    tbpname.Text = studname.ToString();
                    tbdept.Text = Dept.ToString();
                    Session["studstaffcollegecode"] = Convert.ToString(ddlcollegenew.SelectedValue);
                    photo.ImageUrl = "Handler/Handler3.ashx?id=" + appno.ToString();
                    photo.Visible = true;
                    ViewState["Clgcode"] = clgcode;
                }
                fpcellclick = false;
                Panellookup1.Visible = false;
            }
        }
    }
    protected void btnlookupgo1_Click(object sender, EventArgs e)
    {
        StudentLookup1();
    }
    public void load_studentlookup()
    {
        FpSpread1.Visible = false;
        FpSpread1.Sheets[0].RowCount = 0;
        if (ddlbatch.Items.Count > 0)
        {
            if (ddlDegree.Items.Count > 0)
            {
                if (ddlBranch1.Items.Count > 0)
                {
                    string Branch_Code;
                    //string Degree;
                    Branch_Code = ddlBranch1.SelectedValue.ToString();  //GetFunction("select Dept_Code from degree where Acronym = '" + ddlBranch1.SelectedItem.Text.ToString() + "'");
                    sqlcmd = "select distinct Roll_No,Stud_Name,degree_code,Reg_No,app_no from  registration where degree_code='" + Branch_Code + "' and batch_year='" + ddlbatch.SelectedItem.Text.ToString() + "'";
                    dsload = dacces2.select_method_wo_parameter(sqlcmd, "Text");
                    if (dsload.Tables[0].Rows.Count > 0)
                    {
                        for (loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                        {
                            ++FpSpread1.Sheets[0].RowCount;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ddlDegree.SelectedItem.Text.ToString() + "-" + ddlBranch1.SelectedItem.Text.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dsload.Tables[0].Rows[loop]["Roll_No"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = dsload.Tables[0].Rows[loop]["app_no"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["Reg_No"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["Stud_Name"].ToString();
                        }
                        //FpSpread1.Sheets[0].PageSize = FpSpread1.Rows.Count;
                        FpSpread1.SaveChanges();
                        FpSpread1.Visible = true;
                        lblerrefp1.Visible = false;
                    }
                    else
                    {
                        lblerrefp1.Visible = true;
                        lblerrefp1.Text = "No Record(s) Found";
                    }
                }
                FpSpread1.Sheets[0].PageSize = 12;
                FpSpread1.TitleInfo.Height = 30;
                if (FpSpread1.Sheets[0].RowCount > 10)
                {
                    FpSpread1.Height = 390;
                }
                else
                {
                    FpSpread1.Height = (FpSpread1.Sheets[0].RowCount * 25) + 140;
                }
            }
        }
    }
    public void StudentLookup1()
    {
        string serach_Crita = "";
        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
        // ddlstatus.Enabled = true;
        FpSpread1.Visible = false;
        FpSpread1.Sheets[0].RowCount = 0;
        if (ddlbatch.Items.Count > 0)
        {
            if (ddlDegree.Items.Count > 0)
            {
                if (ddlBranch1.Items.Count > 0)
                {
                    if (ddlheader.SelectedIndex == 1)
                    {
                        if (ddloperator.SelectedIndex == 1)
                        {
                            serach_Crita = " and Roll_No like '%" + tbvalue.Text.Trim() + "%' ";
                        }
                        else if (ddloperator.SelectedIndex == 2)
                        {
                            serach_Crita = " and Roll_No like '" + tbvalue.Text.Trim() + "%' ";
                        }
                        else if (ddloperator.SelectedIndex == 3)
                        {
                            serach_Crita = " and Roll_No like '%" + tbvalue.Text.Trim() + "' ";
                        }
                    }
                    if (ddlheader.SelectedIndex == 2)
                    {
                        if (ddloperator.SelectedIndex == 1)
                        {
                            serach_Crita = " and Reg_No like '%" + tbvalue.Text.Trim() + "%' ";
                        }
                        else if (ddloperator.SelectedIndex == 2)
                        {
                            serach_Crita = " and Reg_No like '" + tbvalue.Text.Trim() + "%' ";
                        }
                        else if (ddloperator.SelectedIndex == 3)
                        {
                            serach_Crita = " and Reg_No like '%" + tbvalue.Text.Trim() + "' ";
                        }
                    }
                    if (ddlheader.SelectedIndex == 3)
                    {
                        if (ddloperator.SelectedIndex == 1)
                        {
                            serach_Crita = " and Stud_Name like '%" + tbvalue.Text.Trim() + "%' ";
                        }
                        else if (ddloperator.SelectedIndex == 2)
                        {
                            serach_Crita = " and Stud_Name like '" + tbvalue.Text.Trim() + "%' ";
                        }
                        else if (ddloperator.SelectedIndex == 3)
                        {
                            serach_Crita = " and Stud_Name like '%" + tbvalue.Text.Trim() + "' ";
                        }
                    }
                    string Branch_Code;
                    //string Degree;
                    Branch_Code = ddlBranch1.SelectedValue.ToString(); //GetFunction("select degree_Code from degree where acronym = '" + ddlBranch1.SelectedItem.Text.ToString() + "'");
                    sqlcmd = "select distinct Roll_No,Stud_Name,degree_code,Reg_No ,app_no,college_code from  registration where degree_code='" + ddlBranch1.SelectedValue.ToString() + "' and college_code = '" + ddlcollegenew.SelectedValue.ToString() + "' and batch_year='" + ddlbatch.SelectedValue.ToString() + "' " + serach_Crita + " and cc=0 and exam_flag<>'debar' and delflag=0 and ((isnull(Bus_RouteID,'')='' and isnull(Boarding,'')='' and isnull(VehID,'')='') or iscanceledstage='1')";
                    //and (Bus_RouteID is null Or Boarding is null Or VehID is null or Bus_RouteID='' or Boarding='' or VehID='')";
                    dsload = dacces2.select_method_wo_parameter(sqlcmd, "Text");
                    if (dsload.Tables[0].Rows.Count > 0)
                    {
                        for (loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                        {
                            ++FpSpread1.Sheets[0].RowCount;
                            //Added by Srinath 12/12/2014
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].CellType = txt;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txt;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ddlDegree.SelectedItem.Text.ToString() + "-" + ddlBranch1.SelectedItem.Text.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = ddlDegree.SelectedItem.Text.ToString() + "-" + ddlBranch1.SelectedItem.Text.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dsload.Tables[0].Rows[loop]["Roll_No"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = dsload.Tables[0].Rows[loop]["Roll_No"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = dsload.Tables[0].Rows[loop]["app_no"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["Reg_No"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["Stud_Name"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Note = dsload.Tables[0].Rows[loop]["college_code"].ToString();
                        }
                        //FpSpread1.Sheets[0].PageSize = FpSpread1.Rows.Count;
                        FpSpread1.SaveChanges();
                        FpSpread1.Visible = true;
                        lblerrefp1.Visible = false;
                        tbvalue.Text = "";
                        tbvalue.Enabled = true;
                        ddloperator.Enabled = true;
                    }
                    else
                    {
                        lblerrefp1.Visible = true;
                        lblerrefp1.Text = "No Record(s) Found";
                        tbvalue.Text = "";
                        tbvalue.Enabled = false;
                        ddloperator.Enabled = false;
                        ddlheader.ClearSelection();
                        ddloperator.ClearSelection();
                        btnlookupgo1.Enabled = true;
                    }
                }
                FpSpread1.Sheets[0].PageSize = 12;
                FpSpread1.TitleInfo.Height = 30;
                if (FpSpread1.Sheets[0].RowCount > 10)
                {
                    FpSpread1.Height = 390;
                }
                else
                {
                    FpSpread1.Height = (FpSpread1.Sheets[0].RowCount * 25) + 140;
                }
            }
        }
    }
    protected void btncloselook1_Click(object sender, EventArgs e)
    {
        Panellookup1.Visible = false;
    }
    public string GetFunction(string Att_strqueryst)
    {
        string sqlstr;
        sqlstr = Att_strqueryst;
        getsql.Close();
        getsql.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, getsql);
        SqlDataReader drnew;
        SqlCommand cmd = new SqlCommand(sqlstr);
        cmd.Connection = getsql;
        drnew = cmd.ExecuteReader();
        drnew.Read();
        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "";
        }
    }
    protected void ddlrouteview_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sqlquery = string.Empty;
        ddlvehicletype.Items.Clear();
        ddlvehicletype.Items.Insert(0, new ListItem("All", "-1"));
        if (ddlrouteview.Text == "-1")
        {
            sqlquery = "select distinct Veh_ID from vehicle_master";
        }
        else
        {
            sqlquery = "select distinct Veh_ID from vehicle_master where Route = '" + ddlrouteview.SelectedValue.ToString() + "'";
        }
        ds = dacces2.select_method_wo_parameter(sqlquery, "txt");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ddlvehicletype.Items.Add(ds.Tables[0].Rows[i]["Veh_ID"].ToString());
            }
            ddlvehicletype.SelectedIndex = 0;
        }
        con.Close();
        loadvechilestage();
    }
    protected void ddlvehicletype_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadvechilestage();
    }
    public void loadvechilestage()
    {
        string sqlquery = string.Empty;
        string filter = "";
        ddlstage.Items.Clear();
        ddlstage.Items.Insert(0, new ListItem("All", "-1"));
        if (ddlrouteview.Text != "-1")
        {
            filter = " and v.Route='" + ddlrouteview.SelectedValue.ToString() + "'";
        }
        if (ddlvehicletype.Text != "-1")
        {
            filter = filter + ' ' + "and r.Veh_ID='" + ddlvehicletype.Text + "'";
        }
        sqlquery = "select distinct Stage_Name from routemaster r,vehicle_master v where Stage_Name is not null and Stage_Name<>'' and v.Veh_ID=r.Veh_ID " + filter + "";
        ds = dacces2.select_method_wo_parameter(sqlquery, "txt");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Boolean e1 = isNumeric(ds.Tables[0].Rows[i]["Stage_Name"].ToString(), System.Globalization.NumberStyles.Integer);
                if (e1)
                {
                    string Get_Stage = GetFunction("select distinct Stage_Name from stage_master where Stage_id = '" + ds.Tables[0].Rows[i]["Stage_Name"].ToString() + "'");
                    string Get_Stage_id = GetFunction("select distinct Stage_id from stage_master where Stage_id = '" + ds.Tables[0].Rows[i]["Stage_Name"].ToString() + "'");
                    ddlstage.Items.Add(Get_Stage);//Added By SRinath 8/10/2013
                }
                else
                {
                    ddlstage.Items.Add(ds.Tables[0].Rows[i]["Stage_Name"].ToString());
                }
            }
        }
        ddlstage.SelectedIndex = 0;
    }
    //sudhagar added
    #region button Go
    protected void btnMainGo_Click(object sender, EventArgs e)
    {
        try
        {
            #region old farpoint
            //check scholl or college setting
            checkSchoolSetting();
            string sprhead = string.Empty;
            if (schlSettCode != 0)
                sprhead = "Roll_No";
            else
                sprhead = "Admission No";
            // sprdMainapplication.Sheets[0].AutoPostBack = true;
            Fpload.Sheets[0].ColumnCount = 10;
            Fpload.Sheets[0].RowCount = 0;
            Fpload.Sheets[0].AutoPostBack = false;
            Fpload.CommandBar.Visible = false;
            Fpload.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            Fpload.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
            Fpload.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            Fpload.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            Fpload.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            Fpload.Sheets[0].DefaultStyle.Font.Bold = false;
            FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
            style1.Font.Size = 12;
            style1.Font.Bold = true;
            style1.HorizontalAlign = HorizontalAlign.Left;
            style1.ForeColor = Color.Black;
            Fpload.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            Fpload.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            Fpload.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Left;
            FarPoint.Web.Spread.TextCellType tb = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.ButtonCellType btnedit = new FarPoint.Web.Spread.ButtonCellType();
            btnedit.Text = "Edit";
            Fpload.Sheets[0].AllowTableCorner = true;
            cball.AutoPostBack = true;
            cb.AutoPostBack = false;
            Fpload.Width = 850;
            Fpload.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            //sprdMainapplication.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            Fpload.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            Fpload.Sheets[0].RowHeader.Visible = false;
            Fpload.SheetCorner.Columns[0].HorizontalAlign = HorizontalAlign.Left;
            Fpload.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpload.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpload.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            Fpload.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            Fpload.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpload.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            Fpload.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Edit";
            Fpload.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            Fpload.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            Fpload.Sheets[0].ColumnHeader.Cells[0, 3].Text = sprhead;
            Fpload.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            Fpload.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            Fpload.Sheets[0].Columns[0].CellType = tb;
            Fpload.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Name";
            Fpload.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            Fpload.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
            Fpload.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Route";
            Fpload.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            Fpload.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            Fpload.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Vehicle ID";
            Fpload.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            Fpload.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            Fpload.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Seat No";
            Fpload.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            Fpload.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
            Fpload.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Start Place";
            Fpload.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            Fpload.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Left;
            Fpload.Sheets[0].ColumnHeader.Cells[0, 9].Text = Label9.Text;
            Fpload.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
            Fpload.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Left;
            Fpload.Sheets[0].Columns[6].Visible = true;
            Fpload.Visible = true;
            Fpload.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            Fpload.Sheets[0].Columns[0].Locked = true;
            Fpload.Sheets[0].Columns[1].Locked = false;
            Fpload.Sheets[0].Columns[2].Locked = false;
            Fpload.Sheets[0].Columns[3].Locked = true;
            Fpload.Sheets[0].Columns[4].Locked = true;
            Fpload.Sheets[0].Columns[5].Locked = true;
            Fpload.Sheets[0].Columns[6].Locked = true;
            Fpload.Sheets[0].Columns[7].Locked = true;
            Fpload.Sheets[0].Columns[8].Locked = true;
            Fpload.Sheets[0].Columns[9].Locked = true;
            Fpload.Sheets[0].Columns[0].Width = 50;
            Fpload.Sheets[0].Columns[1].Width = 80;
            Fpload.Sheets[0].Columns[2].Width = 50;
            Fpload.Sheets[0].Columns[3].Width = 80;
            Fpload.Sheets[0].Columns[4].Width = 190;
            Fpload.Sheets[0].Columns[5].Width = 50;
            Fpload.Sheets[0].Columns[5].Width = 90;
            Fpload.Sheets[0].Columns[7].Width = 70;
            Fpload.Sheets[0].Columns[8].Width = 112;
            Fpload.Visible = false;
            #endregion
            #region getvalue
            string collegecode = getCblSelectedValue(cblclg);
            string SQL_Query = string.Empty;
            string selected_batch = "";
            for (int i = 0; i < chklstbatch.Items.Count; i++)
            {
                if (chklstbatch.Items[i].Selected == true)
                {
                    chklstbatch.Items[i].Selected = true;
                    if (selected_batch == "")
                    {
                        selected_batch = chklstbatch.Items[i].Value.ToString();
                    }
                    else
                    {
                        selected_batch = selected_batch + "," + chklstbatch.Items[i].Value.ToString();
                    }
                }
            }
            if (selected_batch.ToString() != "")
            {
                selected_batch = " and r.batch_year in(" + selected_batch + ")";
            }
            string selected_courseid = "";
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    if (selected_courseid == "")
                    {
                        selected_courseid = chklstdegree.Items[i].Value.ToString();
                    }
                    else
                    {
                        selected_courseid = selected_courseid + "," + chklstdegree.Items[i].Value.ToString();
                    }
                }
            }
            if (selected_courseid.ToString() != "")
            {
                selected_courseid = " and d.course_id in(" + selected_courseid + ")";
            }
            string selected_depid = "";
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    if (selected_depid == "")
                    {
                        selected_depid = chklstbranch.Items[i].Value.ToString();
                    }
                    else
                    {
                        selected_depid = selected_depid + "," + chklstbranch.Items[i].Value.ToString();
                    }
                }
            }
            if (selected_depid.ToString() != "")
            {
                selected_depid = " and d.dept_code in(" + selected_depid + ")";
            }
            string selected_desig = "";
            for (int i = 0; i < chklststaff.Items.Count; i++)
            {
                if (chklststaff.Items[i].Selected == true)
                {
                    if (selected_desig == "")
                    {
                        selected_desig = "'" + chklststaff.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        selected_desig = selected_desig + "," + "'" + chklststaff.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (selected_desig.ToString() != "")
            {
                selected_desig = " and st.desig_code in(" + selected_desig + ")";
            }
            string selected_dep = "";
            for (int i = 0; i < chklststaffDept.Items.Count; i++)
            {
                if (chklststaffDept.Items[i].Selected == true)
                {
                    if (selected_dep == "")
                    {
                        selected_dep = chklststaffDept.Items[i].Value.ToString();
                    }
                    else
                    {
                        selected_dep = selected_dep + "," + chklststaffDept.Items[i].Value.ToString();
                    }
                }
            }
            if (selected_dep.ToString() != "")
                selected_dep = " and st.dept_code in(" + selected_dep + ")";
            if (ddlrouteview.SelectedItem.ToString() != "All")
                SQL_Query = " and Bus_RouteID='" + ddlrouteview.SelectedItem.ToString() + "'";
            if (ddlvehicletype.SelectedItem.ToString() != "All")
                SQL_Query = SQL_Query + " and VehID='" + ddlvehicletype.SelectedItem.ToString() + "'";
            if (ddlstage.SelectedItem.ToString() != "All")//Added By SRinath 8/10/2013
            {
                string chk_stage_name = string.Empty;
                string stage_id = string.Empty;
                chk_stage_name = "select * from stage_master where stage_name = '" + ddlstage.SelectedItem.ToString() + "'";
                SqlDataAdapter dr_chk = new SqlDataAdapter(chk_stage_name, con);
                DataSet ds_chk = new DataSet();
                dr_chk.Fill(ds_chk);
                if (ds_chk.Tables[0].Rows.Count > 0)
                {
                    stage_id = ds_chk.Tables[0].Rows[0]["Stage_id"].ToString();
                }
                SQL_Query = SQL_Query + " and Boarding='" + stage_id + "'";
            }
            #endregion
            string iscanel = "";
            if (cbcancel.Checked == true)
                iscanel = " and isnull(IsCanceledStage,0)='1'";
            else
                iscanel = " and isnull(IsCanceledStage,0)<>'1'";
            int rowHeight = 0;
            int rowCnt = 0;
            int chk = 0;
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            if (rbregular.Checked == false && rblateral.Checked == false && rbtransfer.Checked == true)
            {
                #region both
                sqlcmd = "select Roll_No,roll_admit,App_no,Stud_Name,Bus_RouteID,VehID,Boarding,de.dept_acronym,Seat_No,r.college_code from Registration r,Degree d,Department de where  r.degree_code=d.Degree_Code and d.Dept_Code=de.dept_code " + iscanel + " and Bus_RouteID is not null and Bus_RouteID<>'' and VehID is not null and VehID<>'' and Boarding is not null and Boarding<>'' " + SQL_Query + " " + selected_batch + " " + selected_courseid + " " + selected_depid + " and r.college_code in('" + collegecode + "')";
                sqlcmd += " order by len(Seat_No),Seat_No";
                dsload = dacces2.select_method_wo_parameter(sqlcmd, "Text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    ++Fpload.Sheets[0].RowCount;
                    Fpload.Sheets[0].Cells[0, 1].CellType = cball;
                    Fpload.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    for (loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                    {
                        ++Fpload.Sheets[0].RowCount;
                        rowHeight += 25;
                        rowCnt++;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 0].Text = rowCnt.ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 1].CellType = cb;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 1].Tag = "-1";
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 2].CellType = btnedit;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 2].Tag = "-1";
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 3].CellType = txtroll;
                        if (schlSettCode != 0)
                            Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["Roll_No"].ToString();
                        else
                            Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["roll_admit"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 3].Tag = dsload.Tables[0].Rows[loop]["App_no"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 4].Text = dsload.Tables[0].Rows[loop]["Stud_Name"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 5].Text = dsload.Tables[0].Rows[loop]["Bus_RouteID"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 6].Text = dsload.Tables[0].Rows[loop]["VehID"].ToString();
                        string stage_name = "";
                        if (ViewState["Stage"] != null)
                        {
                            Hashtable stage = new Hashtable();
                            stage = (Hashtable)ViewState["Stage"];
                            if (stage.Contains(Convert.ToString(dsload.Tables[0].Rows[loop]["Boarding"])) == true)
                            {
                                stage_name = Convert.ToString(GetCorrespondingKey(Convert.ToString(dsload.Tables[0].Rows[loop]["Boarding"]), stage));
                            }
                        }
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dsload.Tables[0].Rows[loop]["Seat_No"]);
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 8].Text = stage_name;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(dsload.Tables[0].Rows[loop]["Boarding"]);
                        Fpload.Sheets[0].Rows[loop].BackColor = Color.Lavender;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 9].Text = dsload.Tables[0].Rows[loop]["dept_acronym"].ToString();//Added By Srinath 9/8/2013
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 9].Tag = dsload.Tables[0].Rows[loop]["college_code"].ToString();
                    }
                    Fpload.SaveChanges();
                    rowHeight += 100;
                    Fpload.Height = rowHeight;
                    Fpload.Visible = true;
                    btnDel.Visible = true;
                    btnCan.Visible = true;
                    Fpload.ShowHeaderSelection = false;
                    lblerrmainapp.Visible = false;
                }
                else
                    chk++;
                // collegecode = ddlcollegestaff.SelectedValue.ToString();
                sqlcmd = "select s.staff_code,s.appl_no,s.staff_name,s.Bus_RouteID,s.VehID,s.Boarding,hm.dept_acronym,s.college_code from staffmaster s,stafftrans st,hrdept_master hm where s.staff_code=st.staff_code and st.dept_code=hm.dept_code and Bus_RouteID is not null and Bus_RouteID<>'' and VehID is not null and  VehID<>'' and Boarding is not null and Boarding<>'' and s.college_code=hm.college_code " + iscanel + " and s.college_code in('" + collegecode + "') and s.settled <>1 and s.resign <>1 and  st.latestrec<>0 " + SQL_Query + " " + selected_dep + " " + selected_desig + "";
                sqlcmd += " order by len(Seat_No),Seat_No";
                dsload = dacces2.select_method_wo_parameter(sqlcmd, "Text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    for (loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                    {
                        ++Fpload.Sheets[0].RowCount;
                        rowHeight += 25;
                        rowCnt++;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 0].Text = rowCnt.ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 1].CellType = cb;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 1].Tag = "-5";
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 2].CellType = btnedit;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 2].Tag = "-5";
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["staff_code"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 3].Tag = dsload.Tables[0].Rows[loop]["appl_no"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 4].Text = dsload.Tables[0].Rows[loop]["staff_name"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 5].Text = dsload.Tables[0].Rows[loop]["Bus_RouteID"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 6].Text = dsload.Tables[0].Rows[loop]["VehID"].ToString();
                        string stage_name = "";
                        if (ViewState["Stage"] != null)
                        {
                            Hashtable stage = new Hashtable();
                            stage = (Hashtable)ViewState["Stage"];
                            if (stage.Contains(Convert.ToString(dsload.Tables[0].Rows[loop]["Boarding"])) == true)
                            {
                                stage_name = Convert.ToString(GetCorrespondingKey(Convert.ToString(dsload.Tables[0].Rows[loop]["Boarding"]), stage));
                            }
                        }
                        // Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dsload.Tables[0].Rows[loop]["Seat_No"]);
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 8].Text = stage_name;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(dsload.Tables[0].Rows[loop]["Boarding"]);
                        Fpload.Sheets[0].Rows[Fpload.Sheets[0].RowCount - 1].BackColor = Color.LavenderBlush;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 9].Text = dsload.Tables[0].Rows[loop]["dept_acronym"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 9].Tag = dsload.Tables[0].Rows[loop]["college_code"].ToString();
                    }
                    Fpload.SaveChanges();
                    rowHeight += 100;
                    Fpload.Height = rowHeight;
                    Fpload.Visible = true;
                    btnDel.Visible = true;
                    btnCan.Visible = true;
                    Fpload.ShowHeaderSelection = false;
                    lblerrmainapp.Visible = false;
                }
                else
                    chk++;
                if (chk == 2)
                {
                    btnDel.Visible = false;
                    btnCan.Visible = false;
                    lblerrmainapp.Visible = true;
                    lblerrmainapp.Text = "No Record(s) Found";
                }
                #endregion
            }
            else if (rbregular.Checked == true && rblateral.Checked == false && rbtransfer.Checked == false)
            {
                #region student
                sqlcmd = "select Roll_No,roll_admit,App_no,Stud_Name,Bus_RouteID,VehID,Boarding,de.dept_acronym,Seat_No,r.college_code from Registration r,Degree d,Department de where  r.degree_code=d.Degree_Code and d.Dept_Code=de.dept_code " + iscanel + " and Bus_RouteID is not null and Bus_RouteID<>'' and VehID is not null and VehID<>'' and Boarding is not null and Boarding<>'' " + SQL_Query + "" + selected_batch + " " + selected_courseid + " " + selected_depid + " and r.college_code in('" + collegecode + "') ";
                sqlcmd += " order by len(Seat_No),Seat_No";
                dsload = dacces2.select_method_wo_parameter(sqlcmd, "Text");
                if (dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count > 0)
                {
                    ++Fpload.Sheets[0].RowCount;
                    Fpload.Sheets[0].Cells[0, 1].CellType = cball;
                    Fpload.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    for (loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                    {
                        ++Fpload.Sheets[0].RowCount;
                        rowHeight += 40;
                        rowCnt++;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 0].Text = rowCnt.ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 1].CellType = cb;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 2].CellType = btnedit;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 3].CellType = txtroll;
                        if (schlSettCode != 0)
                            Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["Roll_No"].ToString();
                        else
                            Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["roll_admit"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 2].Tag = dsload.Tables[0].Rows[loop]["App_no"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 4].Text = dsload.Tables[0].Rows[loop]["Stud_Name"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 5].Text = dsload.Tables[0].Rows[loop]["Bus_RouteID"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 6].Text = dsload.Tables[0].Rows[loop]["VehID"].ToString();
                        string stage_name = "";
                        if (ViewState["Stage"] != null)
                        {
                            Hashtable stage = new Hashtable();
                            stage = (Hashtable)ViewState["Stage"];
                            if (stage.Contains(Convert.ToString(dsload.Tables[0].Rows[loop]["Boarding"])) == true)
                            {
                                stage_name = Convert.ToString(GetCorrespondingKey(Convert.ToString(dsload.Tables[0].Rows[loop]["Boarding"]), stage));
                            }
                        }
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dsload.Tables[0].Rows[loop]["Seat_No"]);
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 8].Text = stage_name;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(dsload.Tables[0].Rows[loop]["Boarding"]);
                        Fpload.Sheets[0].Rows[loop].BackColor = Color.Lavender;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 9].Text = dsload.Tables[0].Rows[loop]["dept_acronym"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 9].Tag = dsload.Tables[0].Rows[loop]["college_code"].ToString();
                    }
                    Fpload.SaveChanges();
                    rowHeight += 100;
                    Fpload.Height = rowHeight;
                    Fpload.Visible = true;
                    btnDel.Visible = true;
                    btnCan.Visible = true;
                    Fpload.ShowHeaderSelection = false;
                    lblerrmainapp.Visible = false;
                }
                else
                {
                    btnDel.Visible = false;
                    btnCan.Visible = false;
                    lblerrmainapp.Visible = true;
                    lblerrmainapp.Text = "No Record(s) Found";
                }
                #endregion
            }
            else
            {
                #region staff
                //Added By SRinath 8/10/2013
                //sqlcmd = "select * from staffmaster where Seat_No is not null and VehID is not null  and Boarding is not null" + SQL_Query + "";
                // collegecode = ddlcollegestaff.SelectedValue.ToString();
                sqlcmd = "select s.staff_code,s.staff_name,s.Bus_RouteID,s.VehID,s.Boarding,hm.dept_acronym,Seat_No,s.appl_no,s.college_code from staffmaster s,stafftrans st,hrdept_master hm where s.staff_code=st.staff_code and st.dept_code=hm.dept_code and Bus_RouteID is not null and Bus_RouteID<>'' and VehID is not null and  VehID<>'' and Boarding is not null and Boarding<>'' and s.college_code=hm.college_code " + iscanel + " and s.college_code in('" + collegecode + "') and s.settled <>1 and s.resign <>1 and  st.latestrec<>0 " + SQL_Query + "" + selected_dep + " " + selected_desig + " ";
                sqlcmd += " order by len(Seat_No),Seat_No";
                dsload = dacces2.select_method_wo_parameter(sqlcmd, "Text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    ++Fpload.Sheets[0].RowCount;
                    Fpload.Sheets[0].Cells[0, 1].CellType = cball;
                    Fpload.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    for (loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                    {
                        ++Fpload.Sheets[0].RowCount;
                        rowHeight += 40;
                        rowCnt++;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 0].Text = rowCnt.ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 1].CellType = cb;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 2].CellType = btnedit;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["staff_code"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 2].Tag = dsload.Tables[0].Rows[loop]["appl_no"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 4].Text = dsload.Tables[0].Rows[loop]["staff_name"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 5].Text = dsload.Tables[0].Rows[loop]["Bus_RouteID"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 6].Text = dsload.Tables[0].Rows[loop]["VehID"].ToString();
                        string stage_name = "";
                        if (ViewState["Stage"] != null)
                        {
                            Hashtable stage = new Hashtable();
                            stage = (Hashtable)ViewState["Stage"];
                            if (stage.Contains(Convert.ToString(dsload.Tables[0].Rows[loop]["Boarding"])) == true)
                            {
                                stage_name = Convert.ToString(GetCorrespondingKey(Convert.ToString(dsload.Tables[0].Rows[loop]["Boarding"]), stage));
                            }
                        }
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dsload.Tables[0].Rows[loop]["Seat_No"]);
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 8].Text = stage_name;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(dsload.Tables[0].Rows[loop]["Boarding"]);
                        Fpload.Sheets[0].Rows[Fpload.Sheets[0].RowCount - 1].BackColor = Color.LavenderBlush;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 9].Text = dsload.Tables[0].Rows[loop]["dept_acronym"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 9].Tag = dsload.Tables[0].Rows[loop]["college_code"].ToString();
                    }
                    Fpload.SaveChanges();
                    rowHeight += 100;
                    Fpload.Height = rowHeight;
                    btnDel.Visible = true;
                    btnCan.Visible = true;
                    Fpload.Visible = true;
                    Fpload.ShowHeaderSelection = false;
                    lblerrmainapp.Visible = false;
                }
                else
                {
                    btnDel.Visible = false;
                    btnCan.Visible = false;
                    lblerrmainapp.Visible = true;
                    lblerrmainapp.Text = "No Record(s) Found";
                }
                #endregion
            }
            lbprint.Text = "";
            lbprint.Visible = false;
            Fpload.Sheets[0].PageSize = Fpload.Rows.Count;
        }
        catch
        { }
    }
    public void bindstudent()
    {
        try
        {
            string SQL_Query = string.Empty;
            if (ddlrouteview.SelectedItem.ToString() != "All")
            {
                SQL_Query = " and Bus_RouteID='" + ddlrouteview.SelectedItem.ToString() + "'";
            }
            if (ddlvehicletype.SelectedItem.ToString() != "All")
            {
                SQL_Query = SQL_Query + " and VehID='" + ddlvehicletype.SelectedItem.ToString() + "'";
            }
            if (ddlstage.SelectedItem.ToString() != "All")//Added By SRinath 8/10/2013
            {
                SQL_Query = SQL_Query + " and Boarding='" + ddlstage.SelectedItem.ToString() + "'";
            }
            //sprdMainapplication.Sheets[0].Columns[5].Visible = true;
            if (txtbatch.Text == "---Select---" && txtdegree.Text == "---Select---" && txtbranch.Text == "---Select---")
            {
                //sqlcmd = "select distinct r.Roll_No,r.Stud_Name,r.App_No,r.Bus_RouteID,r.Boarding,r.VehID,r.degree_code,d.Acronym from  registration r,degree d where r.degree_code = d.Degree_Code and Bus_RouteID is not null  and r.Boarding is not null " + SQL_Query + "";
                sqlcmd = "select distinct r.Roll_No,r.Stud_Name,r.App_No,r.Bus_RouteID,r.Boarding,r.VehID,r.degree_code,d.Acronym from  registration r,degree d where r.degree_code = d.Degree_Code and Bus_RouteID is not null and Bus_RouteID<>'' and VehID is not null and  VehID<>'' and Boarding is not null and Boarding<>'' " + SQL_Query + "";
                dsload = dacces2.select_method_wo_parameter(sqlcmd, "Text");
                Fpload.Sheets[0].RowCount = 0;
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    for (loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                    {
                        ++Fpload.Sheets[0].RowCount;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 0].Text = Fpload.Sheets[0].RowCount.ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 1].Text = dsload.Tables[0].Rows[loop]["Roll_No"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["Stud_Name"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["Bus_RouteID"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 4].Text = dsload.Tables[0].Rows[loop]["VehID"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 5].Text = dsload.Tables[0].Rows[loop]["Boarding"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 6].Text = dsload.Tables[0].Rows[loop]["Acronym"].ToString();
                    }
                    Fpload.SaveChanges();
                    Fpload.Visible = true;
                    lblerrmainapp.Visible = false;
                }
                else
                {
                    lblerrmainapp.Visible = true;
                    lblerrmainapp.Text = "No Record(s) Found";
                }
                Fpload.Sheets[0].PageSize = Fpload.Rows.Count;
            }
            else
            {
                if (txtbatch.Text != "---Select---" || chklstbatch.Items.Count != null)
                {
                    int itemcount = 0;
                    for (itemcount = 0; itemcount < chklstbatch.Items.Count; itemcount++)
                    {
                        if (chklstbatch.Items[itemcount].Selected == true)
                        {
                            if (strbatch == "")
                                strbatch = "'" + chklstbatch.Items[itemcount].Value.ToString() + "'";
                            else
                                strbatch = strbatch + "," + "'" + chklstbatch.Items[itemcount].Value.ToString() + "'";
                        }
                    }
                    if (strbatch != "")
                    {
                        strbatch = " in(" + strbatch + ")";
                        sqlstrbatch = "r.Batch_Year  " + strbatch + "";
                    }
                    else
                    {
                        sqlstrbatch = "";
                    }
                }
                if (txtdegree.Text != "---Select---" || chklstdegree.Items.Count != null)
                {
                    int itemcount = 0;
                    for (itemcount = 0; itemcount < chklstdegree.Items.Count; itemcount++)
                    {
                        if (chklstdegree.Items[itemcount].Selected == true)
                        {
                            if (strdegree == "")
                                strdegree = "'" + chklstdegree.Items[itemcount].Value.ToString() + "'";
                            // strdegreename = "'" +chklstdegree.Items[itemcount].Text.ToString() +"'";
                            else
                                strdegree = strdegree + "," + "'" + chklstdegree.Items[itemcount].Value.ToString() + "'";
                        }
                    }
                    if (strdegree != "")
                    {
                        strdegree = " in(" + strdegree + ")";
                        sqlstrdegree = " and r.degree_code  " + strdegree + "";
                    }
                    else
                    {
                        sqlstrdegree = " ";
                    }
                }
                if (txtbranch.Text != "---Select---" || chklstbranch.Items.Count != null)
                {
                    int itemcount = 0;
                    for (itemcount = 0; itemcount < chklstbranch.Items.Count; itemcount++)
                    {
                        if (chklstbranch.Items[itemcount].Selected == true)
                        {
                            if (strbranch == "")
                                strbranch = "'" + chklstbranch.Items[itemcount].Value.ToString() + "'";
                            else
                                strbranch = strbranch + "," + "'" + chklstbranch.Items[itemcount].Value.ToString() + "'";
                        }
                    }
                    if (strbranch != "")
                    {
                        strbranch = " in(" + strbranch + ")";
                        sqlstrbranch = " and d.Dept_Code  " + strbranch + "";
                    }
                    else
                    {
                        sqlstrbranch = " ";
                    }
                }
                //sqlcmd = "select distinct r.Roll_No,r.Stud_Name,r.degree_code,r.App_No,r.Bus_RouteID,r.Boarding,r.VehID,d.Degree_Code,d.Acronym from  registration r , degree d where r.degree_code = d.Degree_Code and " + sqlstrbatch + " " + sqlstrbranch + " " + SQL_Query + " and vehid<>'' and vehid is not null and r.Boarding is not null and r.Boarding<>''";//Modified By SRinath /8/10/2013
                sqlcmd = "select distinct r.Roll_No,r.Stud_Name,r.degree_code,r.App_No,r.Bus_RouteID,r.Boarding,r.VehID,d.Degree_Code,d.Acronym from  registration r , degree d where r.degree_code = d.Degree_Code and " + sqlstrbatch + " " + sqlstrbranch + " " + SQL_Query + " Bus_RouteID is not null and Bus_RouteID<>'' and VehID is not null and  VehID<>'' and Boarding is not null and Boarding<>''";//Modified By SRinath /8/10/2013
                dsload = dacces2.select_method_wo_parameter(sqlcmd, "Text");
                Fpload.Sheets[0].RowCount = 0;
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    for (loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                    {
                        ++Fpload.Sheets[0].RowCount;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 0].Text = Fpload.Sheets[0].RowCount.ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 1].Text = dsload.Tables[0].Rows[loop]["Roll_No"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["Stud_Name"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["Bus_RouteID"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 4].Text = dsload.Tables[0].Rows[loop]["VehID"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 5].Text = dsload.Tables[0].Rows[loop]["Boarding"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 6].Text = dsload.Tables[0].Rows[loop]["Acronym"].ToString();
                    }
                    Fpload.SaveChanges();
                    Fpload.Visible = true;
                    lblerrmainapp.Visible = false;
                }
                else
                {
                    lblerrmainapp.Visible = true;
                    lblerrmainapp.Text = "No Record(s) Found";
                }
                Fpload.Sheets[0].PageSize = Fpload.Rows.Count;
            }
        }
        catch
        {
        }
    }
    public void bindstaffchanged()
    {
        try
        {
            string SQL_Query = string.Empty;
            if (ddlrouteview.SelectedItem.ToString() != "All")
            {
                SQL_Query = " and Bus_RouteID='" + ddlrouteview.SelectedItem.ToString() + "'";
            }
            if (ddlvehicletype.SelectedItem.ToString() != "All")
            {
                SQL_Query = SQL_Query + " and VehID='" + ddlvehicletype.SelectedItem.ToString() + "'";
            }
            if (ddlstage.SelectedItem.ToString() != "All")//Added By SRinath 8/10/2013
            {
                SQL_Query = SQL_Query + " and Boarding='" + ddlstage.SelectedItem.ToString() + "'";
            }
            if (txtstaff.Text == "---Select---" && txtstaffDept.Text == "---Select---")
            {
                Fpload.Sheets[0].RowCount = 0;
                // sqlcmd = "select * from staffmaster where Bus_RouteID is not null and Bus_RouteID<>'' " + SQL_Query + "";
                sqlcmd = "select * from staffmaster where Seat_No is not null AND   Bus_RouteID is not null and Bus_RouteID<>'' and VehID is not null and  VehID<>'' and Boarding is not null and Boarding<>'' " + SQL_Query + "";
                dsload = dacces2.select_method_wo_parameter(sqlcmd, "Text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    for (loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                    {
                        //chklststaff.DataTextField = "desig_name";
                        ++Fpload.Sheets[0].RowCount;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 0].Text = Fpload.Sheets[0].RowCount.ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 1].Text = dsload.Tables[0].Rows[loop]["staff_code"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["staff_name"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["Bus_RouteID"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 4].Text = dsload.Tables[0].Rows[loop]["VehID"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 5].Text = dsload.Tables[0].Rows[loop]["Boarding"].ToString();
                        //sprdMainapplication.Sheets[0].Cells[sprdMainapplication.Sheets[0].RowCount - 1, 5].Text = dsload.Tables[0].Rows[loop]["desig_name"].ToString();
                        //sprdMainapplication.Sheets[0].Columns[5].Visible = false;//Modified by srinath 8/10/2013
                    }
                    Fpload.SaveChanges();
                    Fpload.Visible = true;
                    lblerrmainapp.Visible = false;
                }
                else
                {
                    lblerrmainapp.Visible = true;
                    lblerrmainapp.Text = "No Record(s) Found";
                }
                Fpload.Sheets[0].PageSize = Fpload.Rows.Count;
            }
            else
            {
                Fpload.Sheets[0].Columns[5].Visible = true;
                if (txtstaff.Text != "---Select---" || chklststaff.Items.Count != null)
                {
                    int itemcount = 0;
                    for (itemcount = 0; itemcount < chklststaff.Items.Count; itemcount++)
                    {
                        if (chklststaff.Items[itemcount].Selected == true)
                        {
                            if (strstaff1 == "")
                                strstaff1 = "'" + chklststaff.Items[itemcount].Value.ToString() + "'";
                            else
                                strstaff1 = strstaff1 + "," + "'" + chklststaff.Items[itemcount].Value.ToString() + "'";
                        }
                    }
                    if (strstaff1 != "")
                    {
                        strstaff1 = " in(" + strstaff1 + ")";
                        sqlstrstaff1 = "desig_master.desig_code  " + strstaff1 + "";
                    }
                    else
                    {
                        sqlstrstaff1 = "";
                    }
                }
                if (txtstaffDept.Text != "---Select---" || chklststaffDept.Items.Count != null)
                {
                    int itemcount = 0;
                    for (itemcount = 0; itemcount < chklststaffDept.Items.Count; itemcount++)
                    {
                        if (chklststaffDept.Items[itemcount].Selected == true)
                        {
                            if (strstaffdept == "")
                                strstaffdept = "'" + chklststaffDept.Items[itemcount].Value.ToString() + "'";
                            else
                                strstaffdept = strstaffdept + "," + "'" + chklststaffDept.Items[itemcount].Value.ToString() + "'";
                        }
                    }
                    if (strstaffdept != "")
                    {
                        strstaffdept = " in(" + strstaffdept + ")";
                        sqlstrstaffdept1 = " and hrdept_master.dept_code  " + strstaffdept + "";
                    }
                    else
                    {
                        sqlstrstaffdept1 = "";
                    }
                }
                //  sqlcmd = ("select distinct category_code,staffmaster.appl_no as sc,staffmaster.staff_name as sn,staffmaster.Bus_RouteID as BisID,staffmaster.VehID as VehID,staffmaster.Boarding as Boarding,hrdept_master.dept_name,desig_master.desig_name from staffmaster,stafftrans,hrdept_master ,desig_master where hrdept_master.college_code=staffmaster.college_code and hrdept_master.dept_code=stafftrans.dept_code and staffmaster.staff_code=stafftrans.staff_code and staffmaster.settled <>1 and staffmaster.resign <>1 and  stafftrans.latestrec<>0 and staffmaster.college_code = '" + ddlcolleges.SelectedValue.ToString() + "' And staffmaster.college_code = hrdept_master.college_code and desig_master.desig_code=stafftrans.desig_code and desig_master.collegecode=hrdept_master.college_code and " + sqlstrstaff1 + " " + sqlstrstaffdept1 + " and staffmaster.Bus_RouteID is not null order by staffmaster.staff_name");
                sqlcmd = ("select distinct category_code,staffmaster.appl_no as sc,staffmaster.staff_name as sn,staffmaster.Bus_RouteID as BisID,staffmaster.VehID as VehID,staffmaster.Boarding as Boarding,hrdept_master.dept_name,desig_master.desig_name from staffmaster,stafftrans,hrdept_master ,desig_master where hrdept_master.college_code=staffmaster.college_code and hrdept_master.dept_code=stafftrans.dept_code and staffmaster.staff_code=stafftrans.staff_code and staffmaster.settled <>1 and staffmaster.resign <>1 and  stafftrans.latestrec<>0 and staffmaster.college_code = '" + ddlcolleges.SelectedValue.ToString() + "' And staffmaster.college_code = hrdept_master.college_code and desig_master.desig_code=stafftrans.desig_code and desig_master.collegecode=hrdept_master.college_code and " + sqlstrstaff1 + " " + sqlstrstaffdept1 + " and Bus_RouteID is not null and Bus_RouteID<>'' and VehID is not null and  VehID<>'' and Boarding is not null and Boarding<>'' AND Seat_No is not null order by staffmaster.staff_name");
                dsload = dacces2.select_method_wo_parameter(sqlcmd, "Text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    for (loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                    {
                        ++Fpload.Sheets[0].RowCount;
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 0].Text = Fpload.Sheets[0].RowCount.ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 1].Text = dsload.Tables[0].Rows[loop]["sc"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["sn"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["BisID"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 4].Text = dsload.Tables[0].Rows[loop]["VehID"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 5].Text = dsload.Tables[0].Rows[loop]["Boarding"].ToString();
                        Fpload.Sheets[0].Cells[Fpload.Sheets[0].RowCount - 1, 6].Text = dsload.Tables[0].Rows[loop]["dept_name"].ToString();
                        //sprdMainapplication.Sheets[0].Cells[sprdMainapplication.Sheets[0].RowCount - 1, 5].Text = dsload.Tables[0].Rows[loop]["desig_name"].ToString();
                        //sprdMainapplication.Sheets[0].Columns[5].Visible = false;//Modified by srinath 8/10/2013
                    }
                    Fpload.SaveChanges();
                    Fpload.Visible = true;
                    lblerrmainapp.Visible = false;
                    Fpload.Sheets[0].PageSize = Fpload.Rows.Count;
                }
                else
                {
                    lblerrmainapp.Visible = true;
                    lblerrmainapp.Text = "No Record(s) Found";
                }
            }
        }
        catch
        {
        }
    }
    protected void Fpload_OnButtonCommand(object sender, EventArgs e)
    {
        try
        {
            Fpload.SaveChanges();
            string actrow = Fpload.Sheets[0].ActiveRow.ToString();
            string actcol = Fpload.Sheets[0].ActiveRow.ToString();
            string value = "";
            string position = "";
            int Arow = 0;
            int Acol = 0;
            if (actrow != "" && actcol != "")
            {
                Arow = Convert.ToInt32(actrow);
                Acol = Convert.ToInt32(Acol);
                if (Arow == 0 && Acol == 1)
                {
                    value = Convert.ToString(Fpload.Sheets[0].Cells[0, 1].Value);
                    if (value == "1")
                    {
                        for (int i = 0; i < Fpload.Sheets[0].Rows.Count; i++)
                        {
                            Fpload.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < Fpload.Sheets[0].Rows.Count; i++)
                        {
                            Fpload.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
                else
                {
                    if (rbregular.Checked == true)
                    {
                        string app_no = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 2].Tag);
                        string rollNo = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 3].Text);
                        string studname = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 4].Text);
                        string route = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 5].Text);
                        string vehiId = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 6].Text);
                        string place = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 8].Text);
                        string clgcode = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 9].Tag);
                        if (rollNo != "" && studname != "" && route != "" && vehiId != "" && place != "")
                            editMethod(rollNo, studname, route, vehiId, place, clgcode);
                    }
                    else if (rblateral.Checked == true)
                    {
                        // string app_no = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 2].Tag);
                        string rollNo = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 3].Text);
                        string staffname = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 4].Text);
                        string route = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 5].Text);
                        string vehiId = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 6].Text);
                        string place = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 8].Text);
                        string clgcode = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 9].Tag);
                        if (rollNo != "" && staffname != "" && route != "" && vehiId != "" && place != "")
                            editMethod(rollNo, staffname, route, vehiId, place, clgcode);
                    }
                    else
                    {
                        string storstaff = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 2].Tag);
                        if (storstaff.Trim() == "-1")
                        {
                            // string app_no = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 2].Tag);
                            string rollNo = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 3].Text);
                            string studname = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 4].Text);
                            string route = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 5].Text);
                            string vehiId = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 6].Text);
                            string place = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 8].Text);
                            string clgcode = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 9].Tag);
                            if (rollNo != "" && studname != "" && route != "" && vehiId != "" && place != "")
                                editMethod(rollNo, studname, route, vehiId, place, clgcode);
                        }
                        else
                        {
                            // string app_no = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 2].Tag);
                            string rollNo = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 3].Text);
                            string staffname = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 4].Text);
                            string route = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 5].Text);
                            string vehiId = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 6].Text);
                            string place = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 8].Text);
                            string clgcode = Convert.ToString(Fpload.Sheets[0].Cells[Arow, 9].Tag);
                            if (rollNo != "" && staffname != "" && route != "" && vehiId != "" && place != "")
                                editMethod(rollNo, staffname, route, vehiId, place, clgcode);
                        }
                    }
                }
            }
        }
        catch { }
    }
    protected void btnDel_Click(object sender, EventArgs e)
    {
        string name = "";
        if (rbregular.Checked == true)
            name = "Student";
        else if (rblateral.Checked == true)
            name = "Staff";
        else
            name = "Student/Staff";
        if (checkSpread())
        {
            btnOkDel.Visible = true;
            buttDelCEl.Visible = true;
            buttDelCEl.Text = "Cancel";
            divDel.Visible = true;
            lblDel.Text = "Do You Want Delete The " + name + "";
        }
        else
        {
            btnOkDel.Visible = false;
            buttDelCEl.Visible = true;
            buttDelCEl.Text = "OK";
            divDel.Visible = true;
            lblDel.Text = "Please Select Any One " + name + "";
        }
    }
    protected void btnOkDel_Click(object sender, EventArgs e)
    {
        try
        {
            Fpload.SaveChanges();
            double val = 0;
            bool save = false;
            //check school or college setting
            checkSchoolSetting();
            for (int sel = 1; sel < Fpload.Sheets[0].Rows.Count; sel++)
            {
                double.TryParse(Convert.ToString(Fpload.Sheets[0].Cells[sel, 1].Value), out val);
                if (val == 1)
                {
                    string strroll = string.Empty;
                    string app_no = "";
                    string rollNo = "";
                    string studname = "";
                    string place = "";
                    string type = "";
                    if (rbregular.Checked == true)
                    {
                        //student
                        app_no = Convert.ToString(Fpload.Sheets[0].Cells[sel, 2].Tag);
                        rollNo = Convert.ToString(Fpload.Sheets[0].Cells[sel, 3].Text);
                        studname = Convert.ToString(Fpload.Sheets[0].Cells[sel, 4].Text);
                        place = Convert.ToString(Fpload.Sheets[0].Cells[sel, 8].Text);
                        string clgcode = Convert.ToString(Fpload.Sheets[0].Cells[sel, 9].Tag);
                        if (schlSettCode != 0)
                            strroll = " Roll_No='" + rollNo + "'";
                        else
                            strroll = " roll_admit='" + rollNo + "'";
                        type = dacces2.GetFunction(" select Trans_PayType from Registration  where " + strroll + " and college_code='" + clgcode + "'");
                        if (app_no != "" && rollNo != "" && studname != "" && place != "" && type != "" && clgcode != "")
                        {
                            ViewState["Clgcode"] = clgcode;
                            delMethod(app_no, rollNo, place, type, studname);
                            btnMainGo_Click(sender, e);
                            //save = true;
                        }
                    }
                    else if (rblateral.Checked == true)
                    {
                        //staff
                        rollNo = Convert.ToString(Fpload.Sheets[0].Cells[sel, 3].Text);
                        studname = Convert.ToString(Fpload.Sheets[0].Cells[sel, 4].Text);
                        string route = Convert.ToString(Fpload.Sheets[0].Cells[sel, 5].Text);
                        string vehiId = Convert.ToString(Fpload.Sheets[0].Cells[sel, 6].Text);
                        place = Convert.ToString(Fpload.Sheets[0].Cells[sel, 8].Text);
                        string clgcode = Convert.ToString(Fpload.Sheets[0].Cells[sel, 9].Tag);
                        if (studname != "" && rollNo != "")
                        {
                            ViewState["Clgcode"] = clgcode;
                            staffDelMethod(studname, rollNo);
                            btnMainGo_Click(sender, e);
                            // save = true;
                        }
                    }
                    else
                    {
                        //both
                        string storstaff = Convert.ToString(Fpload.Sheets[0].Cells[sel, 1].Tag);
                        if (storstaff.Trim() == "-1")
                        {
                            //student
                            app_no = Convert.ToString(Fpload.Sheets[0].Cells[sel, 3].Tag);
                            rollNo = Convert.ToString(Fpload.Sheets[0].Cells[sel, 3].Text);
                            studname = Convert.ToString(Fpload.Sheets[0].Cells[sel, 4].Text);
                            place = Convert.ToString(Fpload.Sheets[0].Cells[sel, 8].Text);
                            string clgcode = Convert.ToString(Fpload.Sheets[0].Cells[sel, 9].Tag);
                            if (schlSettCode != 0)
                                strroll = " Roll_No='" + rollNo + "'";
                            else
                                strroll = " roll_admit='" + rollNo + "'";
                            type = dacces2.GetFunction(" select Trans_PayType from Registration  where " + strroll + "");
                            if (app_no != "" && rollNo != "" && studname != "" && place != "" && type != "")
                            {
                                ViewState["Clgcode"] = clgcode;
                                delMethod(app_no, rollNo, place, type, studname);
                                btnMainGo_Click(sender, e);
                                //save = true;
                            }
                        }
                        else
                        {
                            //staff
                            rollNo = Convert.ToString(Fpload.Sheets[0].Cells[sel, 3].Text);
                            studname = Convert.ToString(Fpload.Sheets[0].Cells[sel, 4].Text);
                            string route = Convert.ToString(Fpload.Sheets[0].Cells[sel, 5].Text);
                            string vehiId = Convert.ToString(Fpload.Sheets[0].Cells[sel, 6].Text);
                            place = Convert.ToString(Fpload.Sheets[0].Cells[sel, 8].Text);
                            string clgcode = Convert.ToString(Fpload.Sheets[0].Cells[sel, 9].Tag);
                            if (studname != "" && rollNo != "")
                            {
                                ViewState["Clgcode"] = clgcode;
                                staffDelMethod(studname, rollNo);
                                btnMainGo_Click(sender, e);
                                // save = true;
                            }
                        }
                    }
                }
            }
            //if (save == true)
            //{
            //    btnMainGo_Click(sender, e);
            //    Div1.Visible = true;
            //    lbldisp.Text = "Deleted Sucessfully";
            //}
            //else
            //{
            //    Div1.Visible = true;
            //    lbldisp.Text = "Please Select Any One Student/Staff";
            //}
        }
        catch { }
    }
    protected void buttDelCEl_Click(object sender, EventArgs e)
    {
        divDel.Visible = false;
    }
    protected void delMethod(string appno, string rollno, string place, string type, string studname)
    {
        try
        {
            bool check = false;
            bool typeval = false;
            bool paidcheck = false;
            bool ftmnth = false;
            bool removebool = false;
            string statgid = "";
            string ledgPK = "";
            string year = "";
            string category = "";
            int semandyear = 0;
            if (ViewState["Clgcode"] != null)
            {
                collegecode = Convert.ToString(ViewState["Clgcode"]);
            }
            //check school or college setting
            checkSchoolSetting();
            string strroll = string.Empty;
            if (schlSettCode != 0)
                strroll = " Roll_No='" + rollno + "'";
            else
                strroll = " roll_admit='" + rollno + "'";
            //setting
            string feeSetgCode = dacces2.GetFunction("select value from Master_Settings where settings='TransportFeeAllotmentSettings'  and usercode='" + usercode + "'");
            if (feeSetgCode == "1")
                semandyear = 1;
            else if (feeSetgCode == "2")
                semandyear = 1;
            else if (feeSetgCode == "3")
                semandyear = 2;
            else if (feeSetgCode == "4")
                semandyear = 1;
            if (feeSetgCode != "0")
            {
                string getactcode = dacces2.GetFunction("select LinkValue from  InsSettings where LinkName='Current Financial Year' and college_code='" + collegecode + "'");
                statgid = GetFunction("select Stage_id from stage_master where Stage_Name = '" + place + "'");
                if (statgid != "0")
                {
                    //header and ledger
                    string transset = dacces2.GetFunction(" select LinkValue from New_InsSettings where LinkName='TransportLedgerValue'  and user_code='" + usercode + "' and college_code='" + collegecode + "'");
                    if (transset != "")
                    {
                        string[] leng = transset.Split(',');
                        if (leng.Length == 2)
                        {
                            header_id = Convert.ToString(leng[0]);
                            ledgPK = Convert.ToString(leng[1]);
                        }
                    }
                    typeval = true;
                    //cost amount
                    Cost = dacces2.GetFunction("select CAST(f.cost AS INT) as Cost from FeeInfo f where f.StrtPlace = '" + statgid + "' and f.payType = '" + type + "' and college_code='" + collegecode + "'");
                }
                string selsctfeecate = dacces2.GetFunction("select distinct current_semester from registration where " + strroll + " and college_code='" + collegecode + "'");
                //yearwise
                if (type == "Yearly")
                {
                    semandyear = 1;
                    if (selsctfeecate == "1" || selsctfeecate == "2")
                        semval = "1 Year";
                    else if (selsctfeecate == "3" || selsctfeecate == "4")
                        semval = "2 Year";
                    else if (selsctfeecate == "5" || selsctfeecate == "6")
                        semval = "3 Year";
                    else if (selsctfeecate == "7" || selsctfeecate == "8")
                        semval = "4 Year";
                }
                //semesterwise
                else if (type == "Semester")
                {
                    semandyear = 1;
                    if (selsctfeecate == "1")
                        semval = "1 Semester";
                    if (selsctfeecate == "2")
                        semval = "2 Semester";
                    if (selsctfeecate == "3")
                        semval = "3 Semester";
                    if (selsctfeecate == "4")
                        semval = "4 Semester";
                    if (selsctfeecate == "5")
                        semval = "5 Semester";
                    if (selsctfeecate == "6")
                        semval = "6 Semester";
                    if (selsctfeecate == "7")
                        semval = "7 Semester";
                    if (selsctfeecate == "8")
                        semval = "8 Semester";
                    if (selsctfeecate == "9")
                        semval = "9 Semester";
                }
                else if (type == "Term")
                {
                    semandyear = 1;
                    if (selsctfeecate == "1")
                        semval = "Term 1";
                    else if (selsctfeecate == "2")
                        semval = "Term 2";
                    else if (selsctfeecate == "3")
                        semval = "Term 3";
                    else if (selsctfeecate == "4")
                        semval = "Term 4";
                }
                else
                {
                    semandyear = 2;
                    if (ddlmonth.SelectedItem.Text != "Month")
                        month = Convert.ToString(ddlmonth.SelectedItem.Value);
                    if (ddlyear.SelectedItem.Text != "Year")
                        year = Convert.ToString(ddlyear.SelectedItem.Text);
                    semval = feecatValue(selsctfeecate);
                    string[] spl_sem = semval.Split(' ');
                    string curr_sem = spl_sem[0].ToString();
                    if (curr_sem != "")
                    {
                        if (Convert.ToInt32(curr_sem) % 2 == 0)
                            category = "Even";
                        else
                            category = "Odd";
                    }
                }
                string feecatg = dacces2.GetFunction("select distinct textcode from textvaltable where textcriteria ='FEECA' and textval='" + semval + "' and college_code='" + collegecode + "'");
                if (place != "" && rollno != "" && feecatg != "" && feecatg != "0")
                {
                    if (semandyear == 1)
                    {
                        if (appno != "0" && header_id != "" && ledgPK != "" && feecatg != "")
                        {
                            double paidamt = 0;
                            double BalAmount = 0;
                            double.TryParse(Convert.ToString(dacces2.GetFunction("select PaidAmount from FT_FeeAllot where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "'")), out paidamt);
                            double.TryParse(Convert.ToString(dacces2.GetFunction("select BalAmount from FT_FeeAllot where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "'")), out BalAmount);
                            if (paidamt == 0 && BalAmount != 0)
                            {
                                string DelQ = "    if exists (select * from FT_FeeAllot where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "' )delete from FT_FeeAllot where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "'";
                                int falt = dacces2.update_method_wo_parameter(DelQ, "Text");
                                string querystu;
                                querystu = "update registration set Bus_RouteID='',Boarding='',VehID='',Seat_No='',Trans_PayType='',Traveller_Date = '' where " + strroll + " and Stud_Name='" + studname + "' and college_code='" + collegecode + "'";
                                dacces2.update_method_wo_parameter(querystu, "Text");
                                check = true;
                                paidcheck = true;
                            }
                        }
                    }
                    else
                    {
                        string fnlmnth = "";
                        string remove = "";
                        string costamt = "";
                        string Feemnth = dacces2.GetFunction("select FeeAmountMonthly from FT_FeeAllot where App_No='" + appno + "' and FeeCategory ='" + feecatg + "' and LedgerFK = '" + ledgPK + "'");
                        if (Feemnth != "" && Feemnth != "0")
                        {
                            string[] value = Feemnth.Split(',');
                            for (int i = 0; i < value.Length; i++)
                            {
                                string[] mnthval = value[i].Split(':');
                                {
                                    if (mnthval.Length > 0)
                                    {
                                        if (mnthval[0] == month && mnthval[1] == year)
                                        {
                                            remove = mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                            string ftPK = dacces2.GetFunction("select FeeAllotPK from FT_FeeAllot where App_No='" + appno + "' and FeeCategory ='" + feecatg + "' and LedgerFK = '" + ledgPK + "'");
                                            removebool = true;
                                            if (ftPK != "0" && ftPK != "")
                                            {
                                                string FTpadiamt = dacces2.GetFunction("select PaidAmount from FT_FeeallotMonthly where AllotMonth='" + mnthval[0] + "' and AllotYear='" + mnthval[1] + "' and FeeAllotPK='" + ftPK + "'");
                                                if (FTpadiamt != "" && FTpadiamt != "0")
                                                {
                                                    ftmnth = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (fnlmnth == "")
                                            {
                                                fnlmnth = mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                            }
                                            else
                                            {
                                                fnlmnth = fnlmnth + "," + mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //2:2016:100,3:2 016:200     
                        if (remove != "" && Cost != "")
                        {
                            if (ftmnth == false)
                            {
                                string querystu1 = "if exists (select * from FT_FeeAllot where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "' ) update FT_FeeAllot set FeeAmount=FeeAmount -'" + Cost + "',TotalAmount =TotalAmount -'" + Cost + "' ,BalAmount =BalAmount -'" + Cost + "', FeeAmountMonthly='" + fnlmnth + "'  where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "'  ";
                                int saveupdate = dacces2.update_method_wo_parameter(querystu1, "Text");
                                string allotpk = dacces2.GetFunction(" select FeeAllotPK from FT_FeeAllot  where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "'");
                                if (allotpk != "" && month != "" && year != "" && getactcode != "")
                                {
                                    string InsertQ = "if exists(select * from FT_FeeallotMonthly where FeeAllotPK='" + allotpk + "' and AllotMonth='" + month + "' and AllotYear='" + year + "' and FinYearFK='" + getactcode + "') delete from FT_FeeallotMonthly where FeeAllotPK='" + allotpk + "' and AllotMonth='" + month + "' and AllotYear='" + year + "' and FinYearFK='" + getactcode + "'";
                                    int ins = dacces2.update_method_wo_parameter(InsertQ, "Text");
                                }
                                string querystu;
                                querystu = "update registration set Bus_RouteID='',Boarding='',VehID='',Seat_No='',Trans_PayType='',Traveller_Date = '' where " + strroll + " and Stud_Name='" + studname + "' and college_code='" + collegecode + "'";
                                dacces2.update_method_wo_parameter(querystu, "Text");
                                check = true;
                                paidcheck = true;
                            }
                        }
                    }
                    divDel.Visible = false;
                    lblDel.Text = "";
                }
                if (semandyear == 1)
                {
                    if (typeval == true)
                    {
                        if (paidcheck == true)
                        {
                            if (check == true)
                            {
                                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted successfully')", true);
                                imgAlert.Visible = true;
                                lbl_alert.Text = " Deleted successfully";
                            }
                            else
                            {
                                imgAlert.Visible = true;
                                lbl_alert.Text = " Please fill the Valid Details";
                                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please fill the Valid Details')", true);
                            }
                        }
                        else
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = " Paid Amount Available So Cant Delete";
                            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Paid Amount Available So Cant Delete')", true);
                        }
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Fees Available ')", true);
                        imgAlert.Visible = true;
                        lbl_alert.Text = " No Fees Available";
                    }
                }
                else
                {
                    if (removebool == true)
                    {
                        if (paidcheck == true)
                        {
                            if (check == true)
                            {
                                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted successfully')", true);
                                imgAlert.Visible = true;
                                lbl_alert.Text = " Deleted successfully";
                            }
                            else
                            {
                                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please fill the Valid Details')", true);
                                imgAlert.Visible = true;
                                lbl_alert.Text = " Please fill the Valid Details";
                            }
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Paid Amount Available So Cant Delete')", true);
                            imgAlert.Visible = true;
                            lbl_alert.Text = " Paid Amount Available So Cant Delete";
                        }
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Fees Available ')", true);
                        imgAlert.Visible = true;
                        lbl_alert.Text = " No Fees Available ";
                    }
                }
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Set the TransportFeeAllotmentSettings ')", true);
                imgAlert.Visible = true;
                lbl_alert.Text = " Please Set the TransportFeeAllotmentSettings  ";
            }
        }
        catch { }
    }
    protected void staffDelMethod(string staffname, string rollNo)
    {
        try
        {
            string querystu1 = "update staffmaster set Bus_RouteID='',Boarding='',VehID='',Seat_No='',Traveller_Date = '' where  staff_name='" + staffname + "' and staff_code='" + rollNo + "' and college_code='" + collegecode + "'";
            int insr = dacces2.update_method_wo_parameter(querystu1, "Text");
            if (insr > 0)
            {
                //  btnMainGo_Click(sender, e);
                divDel.Visible = false;
                lblDel.Text = "";
                Div1.Visible = true;
                lbldisp.Text = "Deleted Sucessfully";
            }
        }
        catch { }
    }
    protected void editMethod(string Rollno, string StudentName, string Route, string Vehicle_ID, string StartPlace, string clgcode)
    {
        try
        {
            SettingRights();
            string totalseat = string.Empty;
            string AllotedSeatStudent = string.Empty;
            string AllotedSeatStaff = string.Empty;
            int totalallotedseat = 0;
            int RemaningSeat = 0;
            rblateral.Checked = false;
            rbregular.Checked = false;
            Buttonsave.Text = "Update";
            Btn_Delete.Enabled = true;
            lbltravelladd.Text = "Modify";
            checkSchoolSetting();
            string strroll = string.Empty;
            if (schlSettCode != 0)
            {
                strroll = " Roll_No='" + Rollno + "'";
                lblenqno.Text = "Roll No";
            }
            else
            {
                strroll = " roll_admit='" + Rollno + "'";
                lblenqno.Text = "Admission No";
            }
            sqlcmd = "Select * from registration where " + strroll + " and Stud_Name='" + StudentName + "' and college_code='" + clgcode + "'";
            d2 = dacces2.select_method_wo_parameter(sqlcmd, "Text");
            if (d2.Tables[0].Rows.Count > 0)
            {
                #region student
                rblateral.Checked = false;
                rbregular.Checked = true;
                //  rbregular_CheckedChanged(sender, e);
                if (d2.Tables[0].Rows[0]["Traveller_Date"].ToString() != "")
                {
                    DateTime date2 = Convert.ToDateTime(d2.Tables[0].Rows[0]["Traveller_Date"].ToString());
                    string[] datereg2 = Convert.ToString(date2).Split(new char[] { ' ' });
                    string[] spli = datereg2[0].Split('/');
                    string firday = spli[1].ToString();
                    if (firday.Length < 2)
                    {
                        firday = "0" + firday;
                    }
                    string senmonth = spli[0].ToString();
                    if (senmonth.Length < 2)
                    {
                        senmonth = "0" + senmonth;
                    }
                    tbdate.Text = firday + "-" + senmonth + "-" + spli[2].ToString();
                }
                else
                {
                    tbdate.Text = d2.Tables[0].Rows[0]["Traveller_Date"].ToString();
                }
                Session["studstaffcollegecode"] = d2.Tables[0].Rows[0]["college_code"].ToString();
                Buttonsave.Enabled = true;
                tbenqno.Text = Rollno.ToString();
                tbpname.Text = StudentName.ToString();
                tbroute.Text = Route.ToString();
                tbborplace.Text = StartPlace.ToString();
                //Hidden By Srinath 26/4/2014
                //Added by subburaj 30.09.2014
                tbvehno.Text = Vehicle_ID.ToString();
                tbroute.Text = Route.ToString();
                // tbvehno.Text = Vehicle_ID.ToString();
                tbseatno.Text = d2.Tables[0].Rows[0]["Seat_No"].ToString();
                hfapplydegree.Value = d2.Tables[0].Rows[0]["degree_code"].ToString();
                hfdegree.Value = d2.Tables[0].Rows[0]["degree_code"].ToString();
                //  photo.ImageUrl = "Handler/Handler4.ashx?id=" + d2.Tables[0].Rows[0]["app_no"].ToString();
                string studphoto = "select photo from stdphoto where app_no in(select app_no from registration where  " + strroll + " )";
                ds = dacces2.select_method_wo_parameter(studphoto, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    photo.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + Rollno;
                    photo.Visible = true;
                }
                else
                {
                    photo.Visible = false;
                }
                //  photo.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + Rollno;
                sqlcmd = "select c.course_name,d.acronym,d.college_code from course c,degree d where degree_code=" + d2.Tables[0].Rows[0]["degree_code"].ToString() + " and c.course_id=d.course_id and d.college_code='" + clgcode + "'";
                d3 = dacces2.select_method_wo_parameter(sqlcmd, "Text");
                if (d3.Tables[0].Rows.Count > 0)
                {
                    tbdept.Text = d3.Tables[0].Rows[0]["course_name"].ToString() + "-" + d3.Tables[0].Rows[0]["acronym"].ToString();
                    ViewState["Clgcode"] = d3.Tables[0].Rows[0]["college_code"].ToString();
                }
                string type_val = d2.Tables[0].Rows[0]["Trans_PayType"].ToString();
                //if (type_val == "Monthly")
                //{
                //    rbtranfer.Checked = true;
                //    rbstutype.Checked = false;
                //    rbsemtype.Checked = false;
                //    rbtermtype.Checked = false;
                //}
                //else if (type_val == "Yearly")
                //{
                //    rbtranfer.Checked = false;
                //    rbstutype.Checked = true;
                //    rbsemtype.Checked = false;
                //    rbtermtype.Checked = false;
                //}
                //else if (type_val == "Term")
                //{
                //    rbtranfer.Checked = false;
                //    rbstutype.Checked = false;
                //    rbsemtype.Checked = false;
                //    rbtermtype.Checked = true;
                //}
                //else
                //{
                //    rbtranfer.Checked = false;
                //    rbstutype.Checked = false;
                //    rbsemtype.Checked = true;
                //    rbtermtype.Checked = false;
                //}
                Session["Bus_RouteID"] = d2.Tables[0].Rows[0]["Bus_RouteID"].ToString();
                Session["Boarding"] = d2.Tables[0].Rows[0]["Boarding"].ToString();
                Session["VehID"] = d2.Tables[0].Rows[0]["VehID"].ToString();
                Session["Seat_No"] = d2.Tables[0].Rows[0]["Seat_No"].ToString();
                lblfeecat.Visible = false;
                fee_cate.Visible = false;
                lblconcession.Visible = false;
                txtconcession.Visible = false;
                Accordion1.SelectedIndex = 1;
                tbenqno.Enabled = false;
                tbpname.Enabled = false;
                tbdept.Enabled = false;
                #endregion
                enqbtn.Enabled = false;
                rbdirectapply.Checked = true;
                rbenquiry.Checked = false;
                rbdirectapply.Enabled = true;
                rbenquiry.Enabled = false;
                feeset();
            }
            else
            {
                #region staff
                //rblateral_CheckedChanged(sender, e);
                rblateral.Checked = true;
                // rblateral_CheckedChanged(sender, e);
                rbregular.Checked = false;
                sqlcmd = "Select staff_code,Seat_No,Traveller_Date,college_code,Boarding from staffmaster where staff_code='" + Rollno + "' and staff_Name='" + StudentName + "' and college_code='" + clgcode + "'";
                d2 = dacces2.select_method_wo_parameter(sqlcmd, "n");
                try
                {
                    if (d2.Tables[0].Rows.Count > 0)
                    {
                        if (d2.Tables[0].Rows[0]["Traveller_Date"].ToString() != "")
                        {
                            DateTime date1 = Convert.ToDateTime(d2.Tables[0].Rows[0]["Traveller_Date"].ToString());
                            string[] datereg1 = Convert.ToString(date1).Split(new char[] { ' ' });
                            //tbdate.Text = datereg1[0].ToString();
                            string[] spli1 = datereg1[0].Split('/');
                            string firday1 = spli1[0].ToString();
                            if (firday1.Length < 2)
                            {
                                firday1 = "0" + firday1;
                            }
                            string senmonth = spli1[1].ToString();
                            if (senmonth.Length < 2)
                            {
                                senmonth = "0" + senmonth;
                            }
                            tbdate.Text = senmonth + "-" + firday1 + "-" + spli1[2].ToString();
                        }
                        else
                        {
                            tbdate.Text = d2.Tables[0].Rows[0]["Traveller_Date"].ToString();
                        }
                        Session["studstaffcollegecode"] = d2.Tables[0].Rows[0]["college_code"].ToString();
                        string staff_code = "";
                        tbenqno.Text = Rollno.ToString();
                        tbpname.Text = StudentName.ToString();
                        tbroute.Text = Route.ToString();
                        tbborplace.Text = StartPlace.ToString();
                        tbvehno.Text = Vehicle_ID.ToString();
                        tbseatno.Text = d2.Tables[0].Rows[0]["Seat_No"].ToString();
                        staff_code = d2.Tables[0].Rows[0]["staff_code"].ToString();
                        photo.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + staff_code;
                        Lblplace_Value.Text = d2.Tables[0].Rows[0]["Boarding"].ToString();
                        if (photo.ImageUrl != "")
                            photo.Visible = true;
                        else
                            photo.Visible = false;
                        sqlcmd = "select dept_name from hrdept_master where dept_code in(select distinct dept_code from stafftrans where staff_code = '" + staff_code + "')";
                        d3 = dacces2.select_method_wo_parameter(sqlcmd, "Text");
                        if (d3.Tables[0].Rows.Count > 0)
                        {
                            tbdept.Text = d3.Tables[0].Rows[0]["dept_name"].ToString();
                        }
                        txtconcession.Enabled = false;
                        txtconcession.Visible = true;
                        string conces = dacces2.GetFunction("select deduct from fee_allot where roll_admit='" + staff_code + "' and deduct>0");
                        ViewState["Clgcode"] = d2.Tables[0].Rows[0]["college_code"].ToString();
                        if (conces != null && conces.Trim() != "" && conces.Trim() != "0")
                        {
                            string[] sp = conces.Split('.');
                            txtconcession.Text = sp[0];
                        }
                    }
                    // tbvehno.Text = "";
                    //  tbroute.Text = "";
                }
                catch
                {
                }
                lblfeecat.Visible = true;
                fee_cate.Visible = true;
                lblconcession.Visible = true;
                txtconcession.Visible = true;
                Accordion1.SelectedIndex = 1;
                enqbtn.Enabled = false;
                tbenqno.Enabled = false;
                tbpname.Enabled = false;
                tbdept.Enabled = false;
                rbdirectapply.Checked = false;
                rbenquiry.Checked = true;
                rbdirectapply.Enabled = false;
                rbenquiry.Enabled = true;
                #endregion
            }
            try
            {
                ddlclgstud.Items.Clear();
                string selq = dacces2.GetFunction("select collname from collinfo where college_code='" + clgcode + "'");
                if (selq != "0")
                {
                    ddlclgstud.Items.Add(new ListItem(selq, clgcode));
                }
                totalseat = GetFunction("select TotalNo_Seat from vehicle_master where Veh_ID = '" + Vehicle_ID + "'");
                lbltotalseat.Text = totalseat;
                AllotedSeatStudent = GetFunction("select count(*) from registration where VehID = '" + Vehicle_ID + "' and Bus_RouteID = '" + Route + "' and college_code='" + clgcode + "'");
                AllotedSeatStaff = GetFunction("select count(*) from staffmaster where VehID = '" + Vehicle_ID + "' and Bus_RouteID = '" + Route + "' and college_code='" + clgcode + "'");
                totalallotedseat = Convert.ToInt32(AllotedSeatStudent) + Convert.ToInt32(AllotedSeatStaff);
                lblallotedSeat.Text = totalallotedseat.ToString();
                if (totalseat != "")
                {
                    RemaningSeat = Convert.ToInt32(lbltotalseat.Text) - Convert.ToInt32(lblallotedSeat.Text);
                    lblremaingSeat.Text = Convert.ToString(RemaningSeat);
                }
                else
                {
                }
            }
            catch
            {
            }
        }
        catch
        { }
    }
    #endregion
    #region button go Cancel
    protected void btnCan_Click(object sender, EventArgs e)
    {
        string name = "";
        if (rbregular.Checked == true)
            name = "Student";
        else if (rblateral.Checked == true)
            name = "Staff";
        else
            name = "Student/Staff";
        if (checkSpread())
        {
            btnOkCan.Visible = true;
            buttCanCEl.Visible = true;
            buttCanCEl.Text = "Cancel";
            divCan.Visible = true;
            lblCan.Text = "Do You Want Cancel The " + name + "";
        }
        else
        {
            btnOkCan.Visible = false;
            buttCanCEl.Visible = true;
            buttCanCEl.Text = "OK";
            divCan.Visible = true;
            lblCan.Text = "Please Select Any One " + name + "";
        }
    }
    protected void btnOkCan_Click(object sender, EventArgs e)
    {
        try
        {
            Fpload.SaveChanges();
            double val = 0;
            bool save = false;
            //check school or college setting
            checkSchoolSetting();
            for (int sel = 1; sel < Fpload.Sheets[0].Rows.Count; sel++)
            {
                double.TryParse(Convert.ToString(Fpload.Sheets[0].Cells[sel, 1].Value), out val);
                if (val == 1)
                {
                    string strroll = string.Empty;
                    string app_no = "";
                    string rollNo = "";
                    string studname = "";
                    string place = "";
                    string type = "";
                    if (rbregular.Checked == true)
                    {
                        //student
                        app_no = Convert.ToString(Fpload.Sheets[0].Cells[sel, 2].Tag);
                        rollNo = Convert.ToString(Fpload.Sheets[0].Cells[sel, 3].Text);
                        studname = Convert.ToString(Fpload.Sheets[0].Cells[sel, 4].Text);
                        place = Convert.ToString(Fpload.Sheets[0].Cells[sel, 8].Text);
                        string clgcode = Convert.ToString(Fpload.Sheets[0].Cells[sel, 9].Tag);
                        if (schlSettCode != 0)
                            strroll = " Roll_No='" + rollNo + "'";
                        else
                            strroll = " roll_admit='" + rollNo + "'";
                        type = dacces2.GetFunction(" select Trans_PayType from Registration  where " + strroll + " and college_code='" + clgcode + "'");
                        if (app_no != "" && rollNo != "" && studname != "" && place != "" && type != "" && clgcode != "")
                        {
                            ViewState["Clgcode"] = clgcode;
                            delMethodStage(app_no, rollNo, place, type, studname);
                            btnMainGo_Click(sender, e);
                            //save = true;
                        }
                    }
                    else if (rblateral.Checked == true)
                    {
                        //staff
                        rollNo = Convert.ToString(Fpload.Sheets[0].Cells[sel, 3].Text);
                        studname = Convert.ToString(Fpload.Sheets[0].Cells[sel, 4].Text);
                        string route = Convert.ToString(Fpload.Sheets[0].Cells[sel, 6].Tag);
                        // string vehiId = Convert.ToString(Fpload.Sheets[0].Cells[sel, 6].Text);
                        // place = Convert.ToString(Fpload.Sheets[0].Cells[sel, 8].Text);
                        string clgcode = Convert.ToString(Fpload.Sheets[0].Cells[sel, 9].Tag);
                        if (studname != "" && rollNo != "")
                        {
                            ViewState["Clgcode"] = clgcode;
                            staffDelMethodStage(studname, rollNo, route);
                            btnMainGo_Click(sender, e);
                            // save = true;
                        }
                    }
                    else
                    {
                        //both
                        string storstaff = Convert.ToString(Fpload.Sheets[0].Cells[sel, 1].Tag);
                        if (storstaff.Trim() == "-1")
                        {
                            //student
                            app_no = Convert.ToString(Fpload.Sheets[0].Cells[sel, 3].Tag);
                            rollNo = Convert.ToString(Fpload.Sheets[0].Cells[sel, 3].Text);
                            studname = Convert.ToString(Fpload.Sheets[0].Cells[sel, 4].Text);
                            place = Convert.ToString(Fpload.Sheets[0].Cells[sel, 8].Text);
                            string clgcode = Convert.ToString(Fpload.Sheets[0].Cells[sel, 9].Tag);
                            if (schlSettCode != 0)
                                strroll = " Roll_No='" + rollNo + "'";
                            else
                                strroll = " roll_admit='" + rollNo + "'";
                            type = dacces2.GetFunction(" select Trans_PayType from Registration  where " + strroll + "");
                            if (app_no != "" && rollNo != "" && studname != "" && place != "" && type != "")
                            {
                                ViewState["Clgcode"] = clgcode;
                                delMethodStage(app_no, rollNo, place, type, studname);
                                btnMainGo_Click(sender, e);
                                //save = true;
                            }
                        }
                        else
                        {
                            //staff
                            rollNo = Convert.ToString(Fpload.Sheets[0].Cells[sel, 3].Text);
                            studname = Convert.ToString(Fpload.Sheets[0].Cells[sel, 4].Text);
                            string route = Convert.ToString(Fpload.Sheets[0].Cells[sel, 6].Tag);
                            // string vehiId = Convert.ToString(Fpload.Sheets[0].Cells[sel, 6].Text);
                            // place = Convert.ToString(Fpload.Sheets[0].Cells[sel, 8].Text);
                            string clgcode = Convert.ToString(Fpload.Sheets[0].Cells[sel, 9].Tag);
                            if (studname != "" && rollNo != "")
                            {
                                ViewState["Clgcode"] = clgcode;
                                staffDelMethodStage(studname, rollNo, route);
                                btnMainGo_Click(sender, e);
                                // save = true;
                            }
                        }
                    }
                }
            }
            //if (save == true)
            //{
            //    btnMainGo_Click(sender, e);
            //    Div1.Visible = true;
            //    lbldisp.Text = "Deleted Sucessfully";
            //}
            //else
            //{
            //    Div1.Visible = true;
            //    lbldisp.Text = "Please Select Any One Student/Staff";
            //}
        }
        catch { }
    }
    protected void buttCanCEl_Click(object sender, EventArgs e)
    {
        divCan.Visible = false;
    }
    protected void delMethodStage(string appno, string rollno, string place, string type, string studname)
    {
        try
        {
            bool check = false;
            bool typeval = false;
            bool paidcheck = false;
            bool ftmnth = false;
            bool removebool = false;
            string statgid = "";
            string ledgPK = "";
            string year = "";
            string category = "";
            int semandyear = 0;
            if (ViewState["Clgcode"] != null)
            {
                collegecode = Convert.ToString(ViewState["Clgcode"]);
            }
            //setting
            string feeSetgCode = dacces2.GetFunction("select value from Master_Settings where settings='TransportFeeAllotmentSettings'  and usercode='" + usercode + "'");
            if (feeSetgCode == "1")
                semandyear = 1;
            else if (feeSetgCode == "2")
                semandyear = 1;
            else if (feeSetgCode == "3")
                semandyear = 2;
            else if (feeSetgCode == "4")
                semandyear = 1;
            //check school or college setting
            checkSchoolSetting();
            if (feeSetgCode != "0")
            {
                string getactcode = dacces2.GetFunction("select LinkValue from  InsSettings where LinkName='Current Financial Year' and college_code='" + collegecode + "'");
                statgid = GetFunction("select Stage_id from stage_master where Stage_Name = '" + place + "'");
                if (statgid != "0")
                {
                    //header and ledger
                    string transset = dacces2.GetFunction(" select LinkValue from New_InsSettings where LinkName='TransportLedgerValue'  and user_code='" + usercode + "' and college_code='" + collegecode + "'");
                    if (transset != "")
                    {
                        string[] leng = transset.Split(',');
                        if (leng.Length == 2)
                        {
                            header_id = Convert.ToString(leng[0]);
                            ledgPK = Convert.ToString(leng[1]);
                        }
                    }
                    typeval = true;
                    //cost amount
                    Cost = dacces2.GetFunction("select CAST(f.cost AS INT) as Cost from FeeInfo f where f.StrtPlace = '" + statgid + "' and f.payType = '" + type + "' and college_code='" + collegecode + "'");
                }
                string strroll = string.Empty;
                if (schlSettCode != 0)
                    strroll = " Roll_No='" + rollno + "'";
                else
                    strroll = " roll_admit='" + rollno + "'";
                string selsctfeecate = dacces2.GetFunction("select distinct current_semester from registration where " + strroll + "  and college_code='" + collegecode + "'");
                //yearwise
                if (type == "Yearly")
                {
                    semandyear = 1;
                    if (selsctfeecate == "1" || selsctfeecate == "2")
                        semval = "1 Year";
                    else if (selsctfeecate == "3" || selsctfeecate == "4")
                        semval = "2 Year";
                    else if (selsctfeecate == "5" || selsctfeecate == "6")
                        semval = "3 Year";
                    else if (selsctfeecate == "7" || selsctfeecate == "8")
                        semval = "4 Year";
                }
                //semesterwise
                else if (type == "Semester")
                {
                    semandyear = 1;
                    if (selsctfeecate == "1")
                        semval = "1 Semester";
                    if (selsctfeecate == "2")
                        semval = "2 Semester";
                    if (selsctfeecate == "3")
                        semval = "3 Semester";
                    if (selsctfeecate == "4")
                        semval = "4 Semester";
                    if (selsctfeecate == "5")
                        semval = "5 Semester";
                    if (selsctfeecate == "6")
                        semval = "6 Semester";
                    if (selsctfeecate == "7")
                        semval = "7 Semester";
                    if (selsctfeecate == "8")
                        semval = "8 Semester";
                    if (selsctfeecate == "9")
                        semval = "9 Semester";
                }
                else if (type == "Term")
                {
                    semandyear = 1;
                    if (selsctfeecate == "1")
                        semval = "Term 1";
                    else if (selsctfeecate == "2")
                        semval = "Term 2";
                    else if (selsctfeecate == "3")
                        semval = "Term 3";
                    else if (selsctfeecate == "4")
                        semval = "Term 4";
                }
                else
                {
                    semandyear = 2;
                    if (ddlmonth.SelectedItem.Text != "Month")
                        month = Convert.ToString(ddlmonth.SelectedItem.Value);
                    if (ddlyear.SelectedItem.Text != "Year")
                        year = Convert.ToString(ddlyear.SelectedItem.Text);
                    semval = feecatValue(selsctfeecate);
                    string[] spl_sem = semval.Split(' ');
                    string curr_sem = spl_sem[0].ToString();
                    if (curr_sem != "")
                    {
                        if (Convert.ToInt32(curr_sem) % 2 == 0)
                            category = "Even";
                        else
                            category = "Odd";
                    }
                }
                string feecatg = dacces2.GetFunction("select distinct textcode from textvaltable where textcriteria ='FEECA' and textval='" + semval + "' and college_code='" + collegecode + "'");
                if (place != "" && rollno != "" && feecatg != "" && feecatg != "0")
                {
                    if (semandyear == 1)
                    {
                        if (appno != "0" && header_id != "" && ledgPK != "" && feecatg != "")
                        {
                            double paidamt = 0;
                            double BalAmount = 0;
                            double.TryParse(Convert.ToString(dacces2.GetFunction("select PaidAmount from FT_FeeAllot where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "'")), out paidamt);
                            double.TryParse(Convert.ToString(dacces2.GetFunction("select BalAmount from FT_FeeAllot where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "'")), out BalAmount);
                            if (paidamt == 0 && BalAmount != 0 || paidamt != 0 && BalAmount == 0)
                            {
                                //string DelQ = "    if exists (select * from FT_FeeAllot where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "' )delete from FT_FeeAllot where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "'";
                                //int falt = dacces2.update_method_wo_parameter(DelQ, "Text");
                                string querystu;
                                //querystu = "update registration set Bus_RouteID='',Boarding='',VehID='',Seat_No='',Trans_PayType='',Traveller_Date = '' where Roll_No='" + rollno + "' and Stud_Name='" + studname + "'";
                                querystu = "update registration set IsCanceledStage='1' where " + strroll + " and Stud_Name='" + studname + "' and Boarding='" + statgid + "'  and college_code='" + collegecode + "'";
                                dacces2.update_method_wo_parameter(querystu, "Text");
                                check = true;
                                paidcheck = true;
                            }
                        }
                    }
                    else
                    {
                        string fnlmnth = "";
                        string remove = "";
                        string costamt = "";
                        string Feemnth = dacces2.GetFunction("select FeeAmountMonthly from FT_FeeAllot where App_No='" + appno + "' and FeeCategory ='" + feecatg + "' and LedgerFK = '" + ledgPK + "'");
                        if (Feemnth != "" && Feemnth != "0")
                        {
                            string[] value = Feemnth.Split(',');
                            for (int i = 0; i < value.Length; i++)
                            {
                                string[] mnthval = value[i].Split(':');
                                {
                                    if (mnthval.Length > 0)
                                    {
                                        if (mnthval[0] == month && mnthval[1] == year)
                                        {
                                            remove = mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                            string ftPK = dacces2.GetFunction("select FeeAllotPK from FT_FeeAllot where App_No='" + appno + "' and FeeCategory ='" + feecatg + "' and LedgerFK = '" + ledgPK + "'");
                                            removebool = true;
                                            if (ftPK != "0" && ftPK != "")
                                            {
                                                string FTpadiamt = dacces2.GetFunction("select PaidAmount from FT_FeeallotMonthly where AllotMonth='" + mnthval[0] + "' and AllotYear='" + mnthval[1] + "' and FeeAllotPK='" + ftPK + "'");
                                                if (FTpadiamt != "" && FTpadiamt != "0")
                                                {
                                                    ftmnth = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (fnlmnth == "")
                                            {
                                                fnlmnth = mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                            }
                                            else
                                            {
                                                fnlmnth = fnlmnth + "," + mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //2:2016:100,3:2 016:200     
                        if (remove != "" && Cost != "")
                        {
                            if (ftmnth == false)
                            {
                                //string querystu1 = "if exists (select * from FT_FeeAllot where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "' ) update FT_FeeAllot set FeeAmount=FeeAmount -'" + Cost + "',TotalAmount =TotalAmount -'" + Cost + "' ,BalAmount =BalAmount -'" + Cost + "', FeeAmountMonthly='" + fnlmnth + "'  where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "'  ";
                                //int saveupdate = dacces2.update_method_wo_parameter(querystu1, "Text");
                                //string allotpk = dacces2.GetFunction(" select FeeAllotPK from FT_FeeAllot  where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "'");
                                //if (allotpk != "" && month != "" && year != "" && getactcode != "")
                                //{
                                //    string InsertQ = "if exists(select * from FT_FeeallotMonthly where FeeAllotPK='" + allotpk + "' and AllotMonth='" + month + "' and AllotYear='" + year + "' and FinYearFK='" + getactcode + "') delete from FT_FeeallotMonthly where FeeAllotPK='" + allotpk + "' and AllotMonth='" + month + "' and AllotYear='" + year + "' and FinYearFK='" + getactcode + "'";
                                //    int ins = dacces2.update_method_wo_parameter(InsertQ, "Text");
                                //}
                                string querystu;
                                // querystu = "update registration set Bus_RouteID='',Boarding='',VehID='',Seat_No='',Trans_PayType='',Traveller_Date = '' where Roll_No='" + rollno + "' and Stud_Name='" + studname + "'";
                                querystu = "update registration set IsCanceledStage='1' where " + strroll + " and Stud_Name='" + studname + "' and Boarding='" + statgid + "'  and college_code='" + collegecode + "'";
                                dacces2.update_method_wo_parameter(querystu, "Text");
                                check = true;
                                paidcheck = true;
                            }
                        }
                    }
                    divCan.Visible = false;
                    lblCan.Text = "";
                }
                if (semandyear == 1)
                {
                    if (typeval == true)
                    {
                        if (paidcheck == true)
                        {
                            if (check == true)
                            {
                                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Canceled successfully')", true);
                                imgAlert.Visible = true;
                                lbl_alert.Text = " Canceled successfully ";
                            }
                            else
                            {
                                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please fill the Valid Details')", true);
                                imgAlert.Visible = true;
                                lbl_alert.Text = " Please fill the Valid Details";
                            }
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Paid Amount Available So Cant Cancel')", true);
                            imgAlert.Visible = true;
                            lbl_alert.Text = " Paid Amount Available So Cant Cancel";
                        }
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Fees Available ')", true);
                        imgAlert.Visible = true;
                        lbl_alert.Text = " No Fees Available";
                    }
                }
                else
                {
                    if (removebool == true)
                    {
                        if (paidcheck == true)
                        {
                            if (check == true)
                            {
                                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Canceled successfully')", true);
                                imgAlert.Visible = true;
                                lbl_alert.Text = " Canceled successfully";
                            }
                            else
                            {
                                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please fill the Valid Details')", true);
                                imgAlert.Visible = true;
                                lbl_alert.Text = "Please fill the Valid Details";
                            }
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Paid Amount Available So Cant Cancel')", true);
                            imgAlert.Visible = true;
                            lbl_alert.Text = "Paid Amount Available So Cant Cancel";
                        }
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Fees Available ')", true);
                        imgAlert.Visible = true;
                        lbl_alert.Text = "No Fees Available";
                    }
                }
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Set the TransportFeeAllotmentSettings ')", true);
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Set the TransportFeeAllotmentSettings ";
            }
        }
        catch { }
    }
    protected void staffDelMethodStage(string staffname, string rollNo, string boarding)
    {
        try
        {
            //  string querystu1 = "update staffmaster set Bus_RouteID='',Boarding='',VehID='',Seat_No='',Traveller_Date = '' where  staff_name='" + staffname + "' and staff_code='" + rollNo + "'";
            string querystu1 = "update staffmaster set IsCanceledStage='1' where  staff_name='" + staffname + "' and staff_code='" + rollNo + "' and Boarding='" + boarding + "'  and college_code='" + collegecode + "'";
            int insr = dacces2.update_method_wo_parameter(querystu1, "Text");
            if (insr > 0)
            {
                //  btnMainGo_Click(sender, e);
                divCan.Visible = false;
                lblCan.Text = "";
                Div1.Visible = true;
                lbldisp.Text = "Canceled Sucessfully";
            }
        }
        catch { }
    }
    #endregion
    protected bool checkSpread()
    {
        bool value = false;
        try
        {
            double val = 0;
            Fpload.SaveChanges();
            for (int sel = 1; sel < Fpload.Sheets[0].Rows.Count; sel++)
            {
                double.TryParse(Convert.ToString(Fpload.Sheets[0].Cells[sel, 1].Value), out val);
                if (val == 1)
                {
                    value = true;
                }
            }
        }
        catch { }
        return value;
    }
    protected void ddlbatch2_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void ddlDegree2_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void ddlBranch2_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void tbdate_TextChanged(object sender, EventArgs e)
    {
        //lblerror.Visible = false;
        int fd2 = int.Parse((tbdate.Text.Substring(0, 2).ToString()));
        int fyy2 = int.Parse((tbdate.Text.Substring(6, 4).ToString()));
        int fm2 = int.Parse((tbdate.Text.Substring(3, 2).ToString()));
        DateTime ts = Convert.ToDateTime(fm2 + "-" + fd2 + "-" + fyy2);
        if (ts > DateTime.Today)
        {
            tbdate.Text = "";
            lblerrdate.Visible = true;
            lblerrdate.Text = "Date cannot be greater than today";
            return;
        }
        else
        {
            lblerrdate.Visible = false;
        }
    }
    public void clear()
    {
        tbenqno.Text = "";
        tbpname.Text = "";
        tbdept.Text = "";
        tbroute.Text = "";
        tbborplace.Text = "";
        Lblplace_Value.Text = "";
        tbvehno.Text = "";
        tbseatno.Text = "";
        tbdate.Text = "";
        lbltotalseat.Text = "0";
        lblallotedSeat.Text = "0";
        lblremaingSeat.Text = "0";
        chklststaffDept.ClearSelection();
        lblerrdate.Visible = false;
        photo.ImageUrl = "";
        photo.Visible = true;
        txtconcession.Text = "";
    }
    protected void sprdMainapplication_SelectedIndexChanged(object sender, EventArgs e)
    {
        // sprdMainapplication.Sheets[0].AutoPostBack = true;
        Fpload.Sheets[0].AutoPostBack = false;
        Fpload.SaveChanges();
        string totalseat = string.Empty;
        string AllotedSeatStudent = string.Empty;
        string AllotedSeatStaff = string.Empty;
        int totalallotedseat = 0;
        int RemaningSeat = 0;
        Session["Bus_RouteID"] = "";
        Session["Boarding"] = "";
        Session["VehID"] = "";
        Session["Seat_No"] = "";
        if (Cellclick == true)
        {
            rblateral.Checked = false;
            rbregular.Checked = false;
            Buttonsave.Text = "Update";
            Btn_Delete.Enabled = true;
            string activerow = "";
            string activecol = "";
            activerow = Fpload.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpload.ActiveSheetView.ActiveColumn.ToString();
            int ar;
            int ac;
            ar = Convert.ToInt32(activerow.ToString());
            ac = Convert.ToInt32(activecol.ToString());
            lbltravelladd.Text = "Modify";
            if (ar != -1)
            {
                string Rollno = "";
                string StudentName = "";
                string Route = "";
                string Vehicle_ID = "";
                string StartPlace = "";
                Rollno = Fpload.Sheets[0].Cells[ar, 1].Text.ToString();
                StudentName = Fpload.Sheets[0].Cells[ar, 2].Text.ToString();
                Route = Fpload.Sheets[0].Cells[ar, 3].Text.ToString();
                Vehicle_ID = Fpload.Sheets[0].Cells[ar, 4].Text.ToString();
                StartPlace = Fpload.Sheets[0].Cells[ar, 6].Text.ToString();
                sqlcmd = "Select * from registration where Roll_No='" + Rollno + "' and Stud_Name='" + StudentName + "'";
                d2 = dacces2.select_method_wo_parameter(sqlcmd, "Text");
                if (d2.Tables[0].Rows.Count > 0)
                {
                    rblateral.Checked = false;
                    rbregular.Checked = true;
                    rbregular_CheckedChanged(sender, e);
                    if (d2.Tables[0].Rows[0]["Traveller_Date"].ToString() != "")
                    {
                        DateTime date2 = Convert.ToDateTime(d2.Tables[0].Rows[0]["Traveller_Date"].ToString());
                        string[] datereg2 = Convert.ToString(date2).Split(new char[] { ' ' });
                        string[] spli = datereg2[0].Split('/');
                        string firday = spli[1].ToString();
                        if (firday.Length < 2)
                        {
                            firday = "0" + firday;
                        }
                        string senmonth = spli[0].ToString();
                        if (senmonth.Length < 2)
                        {
                            senmonth = "0" + senmonth;
                        }
                        tbdate.Text = firday + "-" + senmonth + "-" + spli[2].ToString();
                    }
                    else
                    {
                        tbdate.Text = d2.Tables[0].Rows[0]["Traveller_Date"].ToString();
                    }
                    Session["studstaffcollegecode"] = d2.Tables[0].Rows[0]["college_code"].ToString();
                    Buttonsave.Enabled = true;
                    tbenqno.Text = Rollno.ToString();
                    tbpname.Text = StudentName.ToString();
                    tbroute.Text = Route.ToString();
                    tbborplace.Text = StartPlace.ToString();
                    //Hidden By Srinath 26/4/2014
                    //Added by subburaj 30.09.2014
                    tbvehno.Text = Vehicle_ID.ToString();
                    tbroute.Text = Route.ToString();
                    // tbvehno.Text = Vehicle_ID.ToString();
                    tbseatno.Text = d2.Tables[0].Rows[0]["Seat_No"].ToString();
                    hfapplydegree.Value = d2.Tables[0].Rows[0]["degree_code"].ToString();
                    hfdegree.Value = d2.Tables[0].Rows[0]["degree_code"].ToString();
                    photo.ImageUrl = "Handler/Handler3.ashx?id=" + d2.Tables[0].Rows[0]["app_no"].ToString();
                    sqlcmd = "select c.course_name,d.acronym from course c,degree d where degree_code=" + d2.Tables[0].Rows[0]["degree_code"].ToString() + " and c.course_id=d.course_id";
                    d3 = dacces2.select_method_wo_parameter(sqlcmd, "Text");
                    if (d3.Tables[0].Rows.Count > 0)
                    {
                        tbdept.Text = d3.Tables[0].Rows[0]["course_name"].ToString() + "-" + d3.Tables[0].Rows[0]["acronym"].ToString();
                    }
                    //Type Identify==================================
                    //Start=====================================
                    //string query = "";
                    //query = "Select Trans_PayType from registration where Roll_No='" + Rollno + "' and Stud_Name='" + StudentName + "'";
                    //d2 = dacces2.select_method_wo_parameter(query, "n");
                    //if (d2.Tables[0].Rows.Count > 0)
                    //{
                    string type_val = d2.Tables[0].Rows[0]["Trans_PayType"].ToString();
                    if (type_val == "Monthly")
                    {
                        rbtranfer.Checked = true;
                        rbstutype.Checked = false;
                        rbsemtype.Checked = false;
                    }
                    else if (type_val == "Yearly")
                    {
                        rbtranfer.Checked = false;
                        rbstutype.Checked = true;
                        rbsemtype.Checked = false;
                    }
                    else
                    {
                        rbtranfer.Checked = false;
                        rbstutype.Checked = false;
                        rbsemtype.Checked = true;
                    }
                    Session["Bus_RouteID"] = d2.Tables[0].Rows[0]["Bus_RouteID"].ToString();
                    Session["Boarding"] = d2.Tables[0].Rows[0]["Boarding"].ToString();
                    Session["VehID"] = d2.Tables[0].Rows[0]["VehID"].ToString();
                    Session["Seat_No"] = d2.Tables[0].Rows[0]["Seat_No"].ToString();
                    lblfeecat.Visible = false;
                    fee_cate.Visible = false;
                    lblconcession.Visible = false;
                    txtconcession.Visible = false;
                    //}
                    //End================================
                }
                else
                {
                    //rblateral_CheckedChanged(sender, e);
                    rblateral.Checked = true;
                    rblateral_CheckedChanged(sender, e);
                    rbregular.Checked = false;
                    sqlcmd = "Select staff_code,Seat_No,Traveller_Date,college_code from staffmaster where staff_code='" + Rollno + "' and staff_Name='" + StudentName + "'";
                    d2 = dacces2.select_method_wo_parameter(sqlcmd, "n");
                    try
                    {
                        if (d2.Tables[0].Rows.Count > 0)
                        {
                            if (d2.Tables[0].Rows[0]["Traveller_Date"].ToString() != "")
                            {
                                DateTime date1 = Convert.ToDateTime(d2.Tables[0].Rows[0]["Traveller_Date"].ToString());
                                string[] datereg1 = Convert.ToString(date1).Split(new char[] { ' ' });
                                //tbdate.Text = datereg1[0].ToString();
                                string[] spli1 = datereg1[0].Split('/');
                                string firday1 = spli1[0].ToString();
                                if (firday1.Length < 2)
                                {
                                    firday1 = "0" + firday1;
                                }
                                string senmonth = spli1[1].ToString();
                                if (senmonth.Length < 2)
                                {
                                    senmonth = "0" + senmonth;
                                }
                                tbdate.Text = senmonth + "-" + firday1 + "-" + spli1[2].ToString();
                            }
                            else
                            {
                                tbdate.Text = d2.Tables[0].Rows[0]["Traveller_Date"].ToString();
                            }
                            Session["studstaffcollegecode"] = d2.Tables[0].Rows[0]["college_code"].ToString();
                            string staff_code = "";
                            tbenqno.Text = Rollno.ToString();
                            tbpname.Text = StudentName.ToString();
                            tbroute.Text = Route.ToString();
                            tbborplace.Text = StartPlace.ToString();
                            tbvehno.Text = Vehicle_ID.ToString();
                            tbseatno.Text = d2.Tables[0].Rows[0]["Seat_No"].ToString();
                            staff_code = d2.Tables[0].Rows[0]["staff_code"].ToString();
                            photo.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + staff_code;
                            sqlcmd = "select dept_name from hrdept_master where dept_code in(select distinct dept_code from stafftrans where staff_code = '" + staff_code + "')";
                            d3 = dacces2.select_method_wo_parameter(sqlcmd, "Text");
                            if (d3.Tables[0].Rows.Count > 0)
                            {
                                tbdept.Text = d3.Tables[0].Rows[0]["dept_name"].ToString();
                            }
                            txtconcession.Enabled = false;
                            txtconcession.Visible = true;
                            string conces = dacces2.GetFunction("select deduct from fee_allot where roll_admit='" + staff_code + "' and deduct>0");
                            if (conces != null && conces.Trim() != "" && conces.Trim() != "0")
                            {
                                string[] sp = conces.Split('.');
                                txtconcession.Text = sp[0];
                            }
                        }
                        tbvehno.Text = "";
                        tbroute.Text = "";
                    }
                    catch
                    {
                    }
                    lblfeecat.Visible = true;
                    fee_cate.Visible = true;
                    lblconcession.Visible = true;
                    txtconcession.Visible = true;
                }
                try
                {
                    totalseat = GetFunction("select TotalNo_Seat from vehicle_master where Veh_ID = '" + Vehicle_ID + "'");
                    lbltotalseat.Text = totalseat;
                    AllotedSeatStudent = GetFunction("select count(*) from registration where VehID = '" + Vehicle_ID + "' and Bus_RouteID = '" + Route + "'");
                    AllotedSeatStaff = GetFunction("select count(*) from staffmaster where VehID = '" + Vehicle_ID + "' and Bus_RouteID = '" + Route + "'");
                    totalallotedseat = Convert.ToInt32(AllotedSeatStudent) + Convert.ToInt32(AllotedSeatStaff);
                    lblallotedSeat.Text = totalallotedseat.ToString();
                    if (totalseat != "")
                    {
                        RemaningSeat = Convert.ToInt32(lbltotalseat.Text) - Convert.ToInt32(lblallotedSeat.Text);
                        lblremaingSeat.Text = Convert.ToString(RemaningSeat);
                    }
                    else
                    {
                    }
                }
                catch
                {
                }
            }
            Cellclick = false;
        }
    }
    protected void sprdMainapplication_CellClick(object sender, EventArgs e)
    {
        string activerow = Fpload.ActiveSheetView.ActiveRow.ToString();
        string activecol = Fpload.ActiveSheetView.ActiveColumn.ToString();
        Cellclick = true;
        Accordion1.SelectedIndex = 1;
    }
    protected void btncloselook2_Click(object sender, EventArgs e)
    {
        pnllookstaff.Visible = false;
    }
    protected void ddlcolleges_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindstaffdept();
    }
    public void bindstaffdept()
    {
        //ddldepartment.Items.Clear();
        //ddldepartment.Items.Insert(0, new ListItem("All", "-1"));
        SqlDataAdapter dadept = new SqlDataAdapter("select distinct dept_code,dept_name from   hrdept_master where college_code='" + ddlcolleges.SelectedValue.ToString() + "' order by dept_name", con);
        DataSet dsdept = new DataSet();
        dadept.Fill(dsdept);
        if (dsdept.Tables[0].Rows.Count > 0)
        {
            if (dsdept.Tables[0].Rows.Count > 0)
            {
                ddldepartment.Items.Clear();
                ddldepartment.DataSource = dsdept.Tables[0];
                ddldepartment.DataTextField = "dept_name";
                ddldepartment.DataValueField = "dept_code";
                ddldepartment.DataBind();
            }
        }
    }
    public void bindstaff()
    {
        string collegecode = getCblSelectedValue(cblclg);
        SqlDataAdapter dastaff = new SqlDataAdapter("select desig_code,desig_name from desig_master where collegeCode in('" + collegecode + "') ", con);
        DataSet dsstaff = new DataSet();
        dastaff.Fill(dsstaff);
        if (dsstaff.Tables[0].Rows.Count > 0)
        {
            chklststaff.Items.Clear();
            if (dsstaff.Tables[0].Rows.Count > 0)
            {
                chklststaff.Items.Clear();
                chklststaff.DataSource = dsstaff.Tables[0];
                chklststaff.DataTextField = "desig_name";
                chklststaff.DataValueField = "desig_code";
                chklststaff.DataBind();
            }
        }
    }
    public void bindstaffdept1()
    {
        string collegecode = getCblSelectedValue(cblclg);
        SqlDataAdapter dadept = new SqlDataAdapter("select distinct dept_code,dept_name from   hrdept_master where college_code in('" + collegecode + "') order by dept_name", con);
        DataSet dsdept = new DataSet();
        dadept.Fill(dsdept);
        if (dsdept.Tables[0].Rows.Count > 0)
        {
            chklststaffDept.Items.Clear();
            if (dsdept.Tables[0].Rows.Count > 0)
            {
                chklststaffDept.Items.Clear();
                chklststaffDept.DataSource = dsdept.Tables[0];
                chklststaffDept.DataTextField = "dept_name";
                chklststaffDept.DataValueField = "dept_code";
                chklststaffDept.DataBind();
            }
        }
    }
    protected void ddldepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
        dss = new DataSet();
        con.Open();
        danew = new SqlDataAdapter("select distinct category_code,staffmaster.staff_code as sc,staffmaster.staff_name as sn,hrdept_master.dept_name,desig_master.desig_name from staffmaster,stafftrans,hrdept_master ,desig_master where hrdept_master.college_code=staffmaster.college_code and hrdept_master.dept_code=stafftrans.dept_code and staffmaster.staff_code=stafftrans.staff_code and staffmaster.settled <>1 and staffmaster.resign <>1 and  stafftrans.latestrec<>0 and staffmaster.college_code = '" + ddlcolleges.SelectedValue.ToString() + "' And staffmaster.college_code = hrdept_master.college_code and desig_master.desig_code=stafftrans.desig_code and desig_master.collegecode=hrdept_master.college_code and hrdept_master.dept_name='" + ddldepartment.SelectedItem.Text + "' and (Bus_RouteID is null Or Boarding is null Or VehID is null or Bus_RouteID='' Or Boarding='' Or VehID='') order by staffmaster.staff_name", con);
        danew.Fill(dss);
        FpSpread2.Sheets[0].RowCount = 0;
        if (dss.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
            {
                ++FpSpread2.Sheets[0].RowCount;
                //Added by srinath 12/12/2014
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].CellType = txt;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = txt;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = dss.Tables[0].Rows[i]["sc"].ToString();
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = dss.Tables[0].Rows[i]["sn"].ToString();
            }
            FpSpread2.SaveChanges();
            FpSpread2.Visible = true;
            lblerrstaff.Visible = false;
        }
        else
        {
            lblerrstaff.Visible = true;
            lblerrstaff.Text = "No Record(s) Found";
        }
        FpSpread2.Sheets[0].PageSize = 12;
        FpSpread2.TitleInfo.Height = 30;
        if (FpSpread2.Sheets[0].RowCount > 10)
        {
            FpSpread2.Height = 390;
        }
        else
        {
            FpSpread2.Height = (FpSpread2.Sheets[0].RowCount * 25) + 140;
        }
        con.Close();
    }
    public void load_stafflookup()
    {
        dss = new DataSet();
        con.Open();
        danew = new SqlDataAdapter("select distinct category_code,staffmaster.staff_code as sc,staffmaster.staff_name as sn,hrdept_master.dept_name,desig_master.desig_name from staffmaster,stafftrans,hrdept_master ,desig_master where hrdept_master.college_code=staffmaster.college_code and hrdept_master.dept_code=stafftrans.dept_code and staffmaster.staff_code=stafftrans.staff_code and staffmaster.settled <>1 and staffmaster.resign <>1 and  stafftrans.latestrec<>0 and staffmaster.college_code =  '" + ddlcolleges.SelectedValue.ToString() + "' And staffmaster.college_code = hrdept_master.college_code and desig_master.desig_code=stafftrans.desig_code and desig_master.collegecode=hrdept_master.college_code and hrdept_master.dept_code= '" + ddldepartment.SelectedValue.ToString() + "' order by staffmaster.staff_name", con);
        danew.Fill(dss);
        if (dss.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
            {
                ++FpSpread2.Sheets[0].RowCount;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = dss.Tables[0].Rows[i]["sc"].ToString();
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = dss.Tables[0].Rows[i]["sn"].ToString();
            }
            FpSpread2.SaveChanges();
            FpSpread2.Visible = true;
            lblerrstaff.Visible = false;
        }
        else
        {
            lblerrstaff.Visible = true;
            lblerrstaff.Text = "No Record(s) Found";
        }
        FpSpread2.Sheets[0].PageSize = 12;
        FpSpread2.TitleInfo.Height = 30;
        if (FpSpread2.Sheets[0].RowCount > 10)
        {
            FpSpread2.Height = 390;
        }
        else
        {
            FpSpread2.Height = (FpSpread2.Sheets[0].RowCount * 25) + 140;
        }
        con.Close();
    }
    protected void FpSpread2_CellClick(object sender, EventArgs e)
    {
        string activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
        fpcellclick = true;
    }
    protected void FpSpread2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (fpcellclick == true)
        {
            FpSpread2.Sheets[0].AutoPostBack = true;
            FpSpread2.SaveChanges();
            if (fpcellclick == true)
            {
                string activerow = "";
                string activecol = "";
                activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
                activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
                int ar;
                int ac;
                ar = Convert.ToInt32(activerow.ToString());
                ac = Convert.ToInt32(activecol.ToString());
                if (ar != -1)
                {
                    Panellookup.Visible = false;
                    string StaffName = "";
                    string Staff_Dept = "";
                    string Staff_Code = "";
                    StaffName = FpSpread2.Sheets[0].Cells[ar, 1].Text.ToString();
                    Staff_Dept = ddldepartment.SelectedItem.Text.ToString();
                    Staff_Code = FpSpread2.Sheets[0].Cells[ar, 0].Text.ToString();
                    tbenqno.Text = Staff_Code;
                    tbpname.Text = StaffName.ToString();
                    tbdept.Text = Staff_Dept.ToString();
                    Session["studstaffcollegecode"] = Convert.ToString(ddlcolleges.SelectedValue);
                    photo.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + Staff_Code;
                }
                fpcellclick = false;
                pnllookstaff.Visible = false;
            }
            txtconcession.Enabled = true;
            txtconcession.Text = "";
        }
    }
    protected void btnMainGo1_Click(object sender, EventArgs e)
    {
        if (ddlserachby.Text == "-1" && ddlrouteID.Text == "-1")
        {
            fpapplied.Sheets[0].RowCount = 0;
            sqlcmd = "select * from RouteMaster where Route_ID is not null and sess = 'M'";
            dsload = dacces2.select_method_wo_parameter(sqlcmd, "Text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                for (loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                {
                    ++fpapplied.Sheets[0].RowCount;
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 0].Text = dsload.Tables[0].Rows[loop]["Route_ID"].ToString();
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 1].Text = dsload.Tables[0].Rows[loop]["Stage_Name"].ToString();
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["Veh_ID"].ToString();
                    try
                    {
                        string totalseat = string.Empty;
                        string AllotedSeatStudent = string.Empty;
                        string AllotedSeatStaff = string.Empty;
                        int totalallotedseat = 0;
                        int RemaningSeat = 0;
                        totalseat = GetFunction("select TotalNo_Seat from vehicle_master where Veh_ID = '" + dsload.Tables[0].Rows[loop]["Veh_ID"].ToString() + "'");
                        fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 3].Text = totalseat;
                        AllotedSeatStudent = GetFunction("select count(*) from registration where VehID = '" + dsload.Tables[0].Rows[loop]["Veh_ID"].ToString() + "' and Bus_RouteID = '" + dsload.Tables[0].Rows[loop]["Route_ID"].ToString() + "'");
                        AllotedSeatStaff = GetFunction("select count(*) from staffmaster where VehID = '" + dsload.Tables[0].Rows[loop]["Veh_ID"].ToString() + "' and Bus_RouteID = '" + dsload.Tables[0].Rows[loop]["Route_ID"].ToString() + "'");
                        totalallotedseat = Convert.ToInt32(AllotedSeatStudent) + Convert.ToInt32(AllotedSeatStaff);
                        fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 4].Text = totalallotedseat.ToString();
                        if (totalseat != "")
                        {
                            RemaningSeat = Convert.ToInt32(fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 3].Text) - Convert.ToInt32(fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 4].Text);
                            fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(RemaningSeat);
                            if (RemaningSeat == 0)
                            {
                                fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 5].BackColor = Color.LightGreen;
                            }
                        }
                        else
                        {
                            fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 5].BackColor = Color.LightGreen;
                        }
                    }
                    catch
                    {
                    }
                    int startingpos = 0;
                    string pre_route = string.Empty;
                    int count = 0;
                    int pre_count = 0;
                    for (int i = 0; i < dsload.Tables[0].Rows.Count; i++)
                    {
                        string route_id = "";
                        route_id = dsload.Tables[0].Rows[i]["Route_ID"].ToString();
                        if (i != dsload.Tables[0].Rows.Count - 1)
                        {
                            if (route_id == pre_route || pre_route == "")
                            {
                                count++;
                                pre_route = route_id;
                            }
                            else
                            {
                                startingpos = startingpos + pre_count;
                                fpapplied.Sheets[0].SpanModel.Add(startingpos, 3, count, 1);
                                fpapplied.Sheets[0].SpanModel.Add(startingpos, 4, count, 1);
                                fpapplied.Sheets[0].SpanModel.Add(startingpos, 5, count, 1);
                                pre_count = count;
                                pre_route = route_id;
                                count = 0;
                                count++;
                            }
                        }
                        else
                        {
                            count++;
                            startingpos = startingpos + pre_count;
                            fpapplied.Sheets[0].SpanModel.Add(startingpos, 3, count, 1);
                            fpapplied.Sheets[0].SpanModel.Add(startingpos, 4, count, 1);
                            fpapplied.Sheets[0].SpanModel.Add(startingpos, 5, count, 1);
                        }
                    }
                }
                fpapplied.Sheets[0].PageSize = fpapplied.Rows.Count;
                fpapplied.Visible = true;
                lblerrmainapp1.Visible = false;
            }
            else
            {
                lblerrmainapp1.Visible = true;
                lblerrmainapp1.Text = "No Record(s) Found";
            }
        }
        else
        {
            fpapplied.Sheets[0].RowCount = 0;
            sqlcmd = "select * from RouteMaster where Route_ID in(select Route_ID from RouteMaster where Stage_Name = '" + ddlserachby.SelectedItem.Text.ToString() + "')";
            dsload = dacces2.select_method_wo_parameter(sqlcmd, "Text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                for (loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                {
                    ++fpapplied.Sheets[0].RowCount;
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 0].Text = dsload.Tables[0].Rows[loop]["Route_ID"].ToString();
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 1].Text = dsload.Tables[0].Rows[loop]["Stage_Name"].ToString();
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["Veh_ID"].ToString();
                    try
                    {
                        string totalseat = string.Empty;
                        string AllotedSeatStudent = string.Empty;
                        string AllotedSeatStaff = string.Empty;
                        int totalallotedseat = 0;
                        int RemaningSeat = 0;
                        totalseat = GetFunction("select TotalNo_Seat from vehicle_master where Veh_ID = '" + dsload.Tables[0].Rows[loop]["Veh_ID"].ToString() + "'");
                        fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 3].Text = totalseat;
                        AllotedSeatStudent = GetFunction("select count(*) from registration where VehID = '" + dsload.Tables[0].Rows[loop]["Veh_ID"].ToString() + "' and Bus_RouteID = '" + dsload.Tables[0].Rows[loop]["Route_ID"].ToString() + "'");
                        AllotedSeatStaff = GetFunction("select count(*) from staffmaster where VehID = '" + dsload.Tables[0].Rows[loop]["Veh_ID"].ToString() + "' and Bus_RouteID = '" + dsload.Tables[0].Rows[loop]["Route_ID"].ToString() + "'");
                        totalallotedseat = Convert.ToInt32(AllotedSeatStudent) + Convert.ToInt32(AllotedSeatStaff);
                        fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 4].Text = totalallotedseat.ToString();
                        if (totalseat != "")
                        {
                            RemaningSeat = Convert.ToInt32(fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 3].Text) - Convert.ToInt32(fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 4].Text);
                            fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(RemaningSeat);
                            if (RemaningSeat == 0)
                            {
                                fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 5].BackColor = Color.LightGreen;
                            }
                        }
                        else
                        {
                            fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 5].BackColor = Color.LightGreen;
                        }
                    }
                    catch
                    {
                    }
                    int startingpos = 0;
                    string pre_route = string.Empty;
                    int count = 0;
                    int pre_count = 0;
                    for (int i = 0; i < dsload.Tables[0].Rows.Count; i++)
                    {
                        string route_id = "";
                        route_id = dsload.Tables[0].Rows[i]["Route_ID"].ToString();
                        if (i != dsload.Tables[0].Rows.Count - 1)
                        {
                            if (route_id == pre_route || pre_route == "")
                            {
                                count++;
                                pre_route = route_id;
                            }
                            else
                            {
                                startingpos = startingpos + pre_count;
                                fpapplied.Sheets[0].SpanModel.Add(startingpos, 3, count, 1);
                                fpapplied.Sheets[0].SpanModel.Add(startingpos, 4, count, 1);
                                fpapplied.Sheets[0].SpanModel.Add(startingpos, 5, count, 1);
                                pre_count = count;
                                pre_route = route_id;
                                count = 0;
                                count++;
                            }
                        }
                        else
                        {
                            count++;
                            startingpos = startingpos + pre_count;
                            fpapplied.Sheets[0].SpanModel.Add(startingpos, 3, count, 1);
                            fpapplied.Sheets[0].SpanModel.Add(startingpos, 4, count, 1);
                            fpapplied.Sheets[0].SpanModel.Add(startingpos, 5, count, 1);
                        }
                    }
                }
                fpapplied.Sheets[0].PageSize = fpapplied.Rows.Count;
                fpapplied.Visible = true;
                lblerrmainapp1.Visible = false;
                ddlrouteID.Enabled = true;
            }
            else
            {
                lblerrmainapp1.Visible = true;
                lblerrmainapp1.Text = "No Record(s) Found";
                ddlrouteID.Enabled = true;
            }
        }
    }
    protected void ddlrouteID_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlserachby.Enabled = false;
        if (ddlrouteID.Text != "-1")
        {
            fpapplied.Sheets[0].RowCount = 0;
            sqlcmd = "select * from RouteMaster where Route_ID = '" + ddlrouteID.SelectedItem.Text.ToString() + "' and sess = 'M'";
            dsload = dacces2.select_method_wo_parameter(sqlcmd, "Text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                for (loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                {
                    ++fpapplied.Sheets[0].RowCount;
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 0].Text = dsload.Tables[0].Rows[loop]["Route_ID"].ToString();
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 1].Text = dsload.Tables[0].Rows[loop]["Stage_Name"].ToString();
                    fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["Veh_ID"].ToString();
                    try
                    {
                        string totalseat = string.Empty;
                        string AllotedSeatStudent = string.Empty;
                        string AllotedSeatStaff = string.Empty;
                        int totalallotedseat = 0;
                        int RemaningSeat = 0;
                        totalseat = GetFunction("select TotalNo_Seat from vehicle_master where Veh_ID = '" + dsload.Tables[0].Rows[loop]["Veh_ID"].ToString() + "'");
                        fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 3].Text = totalseat;
                        AllotedSeatStudent = GetFunction("select count(*) from registration where VehID = '" + dsload.Tables[0].Rows[loop]["Veh_ID"].ToString() + "' and Bus_RouteID = '" + dsload.Tables[0].Rows[loop]["Route_ID"].ToString() + "'");
                        AllotedSeatStaff = GetFunction("select count(*) from staffmaster where VehID = '" + dsload.Tables[0].Rows[loop]["Veh_ID"].ToString() + "' and Bus_RouteID = '" + dsload.Tables[0].Rows[loop]["Route_ID"].ToString() + "'");
                        totalallotedseat = Convert.ToInt32(AllotedSeatStudent) + Convert.ToInt32(AllotedSeatStaff);
                        fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 4].Text = totalallotedseat.ToString();
                        if (totalseat != "")
                        {
                            RemaningSeat = Convert.ToInt32(fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 3].Text) - Convert.ToInt32(fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 4].Text);
                            fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(RemaningSeat);
                            if (RemaningSeat == 0)
                            {
                                fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 5].BackColor = Color.LightGreen;
                            }
                        }
                        else
                        {
                            fpapplied.Sheets[0].Cells[fpapplied.Sheets[0].RowCount - 1, 5].BackColor = Color.LightGreen;
                        }
                    }
                    catch
                    {
                    }
                    int startingpos = 0;
                    string pre_route = string.Empty;
                    int count = 0;
                    int pre_count = 0;
                    for (int i = 0; i < dsload.Tables[0].Rows.Count; i++)
                    {
                        string route_id = "";
                        route_id = dsload.Tables[0].Rows[i]["Route_ID"].ToString();
                        if (i != dsload.Tables[0].Rows.Count - 1)
                        {
                            if (route_id == pre_route || pre_route == "")
                            {
                                count++;
                                pre_route = route_id;
                            }
                            else
                            {
                                startingpos = startingpos + pre_count;
                                fpapplied.Sheets[0].SpanModel.Add(startingpos, 3, count, 1);
                                fpapplied.Sheets[0].SpanModel.Add(startingpos, 4, count, 1);
                                fpapplied.Sheets[0].SpanModel.Add(startingpos, 5, count, 1);
                                pre_count = count;
                                pre_route = route_id;
                                count = 0;
                                count++;
                            }
                        }
                        else
                        {
                            count++;
                            startingpos = startingpos + pre_count;
                            fpapplied.Sheets[0].SpanModel.Add(startingpos, 3, count, 1);
                            fpapplied.Sheets[0].SpanModel.Add(startingpos, 4, count, 1);
                            fpapplied.Sheets[0].SpanModel.Add(startingpos, 5, count, 1);
                        }
                    }
                }
                fpapplied.Sheets[0].PageSize = fpapplied.Rows.Count;
                fpapplied.SaveChanges();
                fpapplied.Visible = true;
                lblerrmainapp1.Visible = false;
                ddlserachby.Enabled = true;
            }
            else
            {
                lblerrmainapp1.Visible = true;
                lblerrmainapp1.Text = "No Record(s) Found";
                ddlserachby.Enabled = true;
            }
        }
    }
    protected void ddlbatchstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void ddlstaffDept1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //sprdMainapplication.Sheets[0].RowCount = 0;
        //sqlcmd = ("select distinct category_code,staffmaster.appl_no as sc,staffmaster.staff_name as sn,staffmaster.Bus_RouteID as BisID,staffmaster.VehID as VehID,staffmaster.Boarding as Boarding,hrdept_master.dept_name,desig_master.desig_name from staffmaster,stafftrans,hrdept_master ,desig_master where hrdept_master.college_code=staffmaster.college_code and hrdept_master.dept_code=stafftrans.dept_code and staffmaster.staff_code=stafftrans.staff_code and staffmaster.settled <>1 and staffmaster.resign <>1 and  stafftrans.latestrec<>0 and staffmaster.college_code = '" + ddlcolleges.SelectedValue.ToString() + "' And staffmaster.college_code = hrdept_master.college_code and desig_master.desig_code=stafftrans.desig_code and desig_master.collegecode=hrdept_master.college_code and hrdept_master.dept_name='" + ddlstaffDept1.SelectedItem.Text + "' and desig_master.desig_name = '" + ddlbatchstaff.SelectedItem.Text + "' and staffmaster.Bus_RouteID is not null and staffmaster.Seat_No<>'' order by staffmaster.staff_name");
        //dsload = dacces2.select_method_wo_parameter(sqlcmd, "Text");
        //if (dsload.Tables[0].Rows.Count > 0)
        //{
        //    for (loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
        //    {
        //        ++sprdMainapplication.Sheets[0].RowCount;
        //        sprdMainapplication.Sheets[0].Cells[sprdMainapplication.Sheets[0].RowCount - 1, 0].Text = dsload.Tables[0].Rows[loop]["sc"].ToString();
        //        sprdMainapplication.Sheets[0].Cells[sprdMainapplication.Sheets[0].RowCount - 1, 1].Text = dsload.Tables[0].Rows[loop]["sn"].ToString();
        //        sprdMainapplication.Sheets[0].Cells[sprdMainapplication.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["BisID"].ToString();
        //        sprdMainapplication.Sheets[0].Cells[sprdMainapplication.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["VehID"].ToString();
        //        sprdMainapplication.Sheets[0].Cells[sprdMainapplication.Sheets[0].RowCount - 1, 4].Text = dsload.Tables[0].Rows[loop]["Boarding"].ToString();
        //    }
        //    sprdMainapplication.SaveChanges();
        //    sprdMainapplication.Visible = true;
        //    lblerrmainapp.Visible = false;
        //    sprdMainapplication.Sheets[0].PageSize = sprdMainapplication.Rows.Count;
        //}
        //else
        //{
        //    lblerrmainapp.Visible = true;
        //    lblerrmainapp.Text = "No Record(s) Found";
        //}
    }
    protected void ddlcollegestaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        string collegecode = getCblSelectedValue(cblclg);
        //  collegecode = ddlcollegestaff.SelectedValue.ToString();
        BindBatch();
        BindDegree(singleuser, group_user, collegecode, usercode);
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        bindstaffdept1();
        bindstaff();
    }
    protected void rbregular_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        rbdirectapply.Checked = true;
        checkSchoolSetting();
        if (schlSettCode == 0)
            lblenqno.Text = "Admission No";
        else
            lblenqno.Text = "Roll No";
        Label1.Text = "Student Name";
        Label2.Text = Label2.Text;
        rbenquiry.Checked = false;
        //   ddlcollegestaff.Visible = false;
        lblerrmainapp.Enabled = false;
        txtstaff.Visible = false;
        txtstaffDept.Visible = false;
        pstaff.Visible = false;
        pstaffDept.Visible = false;
        // Label13.Visible = false;
        lblstaff.Visible = false;
        lblstaffDept.Visible = false;
        txtbatch.Visible = true;
        txtdegree.Visible = true;
        txtbranch.Visible = true;
        Label7.Visible = true;
        Label8.Visible = true;
        Label9.Visible = true;
        //Label13.Visible = true;
        pbatch.Visible = true;
        pdegree1.Visible = true;
        pbranch.Visible = true;
        btnDel.Attributes.Add("Style", "top:320px; left:748px; position: absolute;");
        btnCan.Attributes.Add("style", "top:320px; left:807px; position:absolute;");
    }
    protected void rblateral_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        rbenquiry.Checked = true;
        lblenqno.Text = "Staff Code";
        Label1.Text = "Staff Name";
        Label2.Text = "Department";
        rbdirectapply.Checked = false;
        lblerrmainapp.Enabled = false;
        txtbatch.Visible = false;
        txtdegree.Visible = false;
        txtbranch.Visible = false;
        Label7.Visible = false;
        Label8.Visible = false;
        Label9.Visible = false;
        //ddlcollegestaff.Visible = true;
        pbatch.Visible = false;
        pdegree1.Visible = false;
        pbranch.Visible = false;
        txtstaff.Visible = true;
        txtstaffDept.Visible = true;
        pstaff.Visible = true;
        pstaffDept.Visible = true;
        //Label13.Visible = true;
        lblstaff.Visible = true;
        lblstaffDept.Visible = true;
        //Label11.Visible = true;
        //Label12.Visible = true;
        btnDel.Attributes.Add("Style", "top:320px; left:748px; position: absolute;");
        btnCan.Attributes.Add("Style", "top:320px; left:807px; position: absolute;");
    }
    protected void rbtransfer_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        //ddlcollegestaff.Visible = true;
        txtbatch.Visible = true;
        txtdegree.Visible = true;
        txtbranch.Visible = true;
        Label7.Visible = true;
        Label8.Visible = true;
        Label9.Visible = true;
        // Label13.Visible = true;
        pbatch.Visible = true;
        pdegree1.Visible = true;
        pbranch.Visible = true;
        txtstaff.Visible = true;
        txtstaffDept.Visible = true;
        pstaff.Visible = true;
        pstaffDept.Visible = true;
        //Label13.Visible = false;
        lblstaff.Visible = true;
        lblstaffDept.Visible = true;
        btnDel.Attributes.Add("Style", "top:342px; left:748px; position: absolute;");
        btnCan.Attributes.Add("Style", "top:342px; left:807px; position: absolute;");
    }
    protected void ddlheader_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlheader.SelectedItem.Text != "---Select---")
        {
            ddloperator.Enabled = true;
            btnlookupgo1.Enabled = true;
        }
        else
        {
            ddloperator.Enabled = false;
            tbvalue.Enabled = false;
        }
    }
    protected void ddloperator_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddloperator.SelectedItem.Text != "---Select---")
        {
            tbvalue.Enabled = true;
            btnlookupgo1.Enabled = true;
        }
        else
        {
            tbvalue.Enabled = false;
            btnlookupgo1.Enabled = false;
        }
    }
    protected void tbvalue_TextChanged(object sender, EventArgs e)
    {
        StudentLookup1();
    }
    protected void chkbatch_CheckedChanged(object sender, EventArgs e)
    {
        if (chkbatch.Checked == true)
        {
            for (int i = 0; i < chklstbatch.Items.Count; i++)
            {
                chklstbatch.Items[i].Selected = true;
                txtbatch.Text = "Batch(" + (chklstbatch.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstbatch.Items.Count; i++)
            {
                chklstbatch.Items[i].Selected = false;
                txtbatch.Text = "---Select---";
            }
        }
        //bindstaff(); //Added By srinath 12/1/13
    }
    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        pbatch.Focus();
        int batchcount = 0;
        string value = "";
        string code = "";
        for (int i = 0; i < chklstbatch.Items.Count; i++)
        {
            if (chklstbatch.Items[i].Selected == true)
            {
                value = chklstbatch.Items[i].Text;
                code = chklstbatch.Items[i].Value.ToString();
                batchcount = batchcount + 1;
                txtbatch.Text = "Batch(" + batchcount.ToString() + ")";
            }
        }
        if (batchcount == 0)
            txtbatch.Text = "---Select---";
        else
        {
            Label lbl = batchlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = batchimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(batchimg_Click);
        }
        batchcnt = batchcount;
        //BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
    }
    public void BindBatch()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();
            ds2 = dacces2.BindBatch();
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstbatch.DataSource = ds2;
                chklstbatch.DataTextField = "Batch_year";
                chklstbatch.DataValueField = "Batch_year";
                chklstbatch.DataBind();
                chklstbatch.SelectedIndex = 0;
                txtbatch.Text = "Batch(" + 1 + ")";
                //  chklstbatch.SelectedIndex = chklstbatch.Items.Count - 1;
                //for (int i = 0; i < chklstbatch.Items.Count; i++)
                //{
                //    chklstbatch.Items[i].Selected = true;
                //    if (chklstbatch.Items[i].Selected == true)
                //    {
                //        count += 1;
                //    }
                //    if (chklstbatch.Items.Count == count)
                //    {
                //        chkbatch.Checked = true;
                //    }
                //}
                //chklstbatch.Items[].Selected = true;
            }
        }
        catch (Exception ex)
        {
            lblMainError.Text = ex.ToString();
        }
    }
    protected void LinkButtonbatch_Click(object sender, EventArgs e)
    {
        chklstbatch.ClearSelection();
        batchcnt = 0;
        txtbatch.Text = "---Select---";
    }
    public void batchimg_Click(object sender, ImageClickEventArgs e)
    {
        batchcnt = batchcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstbatch.Items[r].Selected = false;
        txtbatch.Text = "Batch(" + batchcnt.ToString() + ")";
        if (txtbatch.Text == "Batch(0)")
        {
            txtbatch.Text = "---Select---";
        }
    }
    public Label batchlabel()
    {
        Label lbc = new Label();
        ViewState["lseatcontrol"] = true;
        return (lbc);
    }
    public ImageButton batchimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }
    //------Load Function for the Degree Details-----
    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            if (cblclg.Items.Count > 0)
            {
                string clgcode = "";
                for (int clg = 0; clg < cblclg.Items.Count; clg++)
                {
                    if (cblclg.Items[clg].Selected == true)
                    {
                        if (clgcode == "")
                            clgcode = cblclg.Items[clg].Value;
                        else
                            clgcode = clgcode + "," + cblclg.Items[clg].Value;
                    }
                }
                //   collegecode = Session["collegecode"].ToString();
                collegecode = clgcode;
            }
            chklstdegree.Items.Clear();
            txtdegree.Text = "--Select--";
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            string selqry = string.Empty;
            ds2.Clear();
            if (!string.IsNullOrEmpty(collegecode))
            {
                selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code in(" + collegecode + ")";
                ds2 = dacces2.select_method_wo_parameter(selqry, "Text");
            }
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                chklstdegree.DataSource = ds2;
                chklstdegree.DataTextField = "course_name";
                chklstdegree.DataValueField = "course_id";
                chklstdegree.DataBind();
                chklstdegree.Items[0].Selected = true;
                txtdegree.Text = Label8.Text + "(" + 1 + ")";
            }
        }
        catch (Exception ex)
        {
            //lblMainError.Text = ex.ToString();
        }
    }
    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        if (chkdegree.Checked == true)
        {
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                chklstdegree.Items[i].Selected = true;
                txtdegree.Text = Label8.Text + "(" + (chklstdegree.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                chklstdegree.Items[i].Selected = false;
                txtdegree.Text = "---Select---";
            }
        }
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
    }
    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        //pdegree.Focus();
        pdegree1.Focus();
        int degreecount = 0;
        string value = "";
        string code = "";
        for (int i = 0; i < chklstdegree.Items.Count; i++)
        {
            if (chklstdegree.Items[i].Selected == true)
            {
                value = chklstdegree.Items[i].Text;
                code = chklstdegree.Items[i].Value.ToString();
                degreecount = degreecount + 1;
                txtdegree.Text = Label8.Text + "(" + degreecount.ToString() + ")";
            }
        }
        if (degreecount == 0)
            txtdegree.Text = "---Select---";
        else
        {
            Label lbl = degreelabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = degreeimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(degreeimg_Click);
        }
        degreecnt = degreecount;
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
    }
    protected void LinkButtondegree_Click(object sender, EventArgs e)
    {
        chklstdegree.ClearSelection();
        degreecnt = 0;
        txtdegree.Text = "---Select---";
    }
    public void degreeimg_Click(object sender, ImageClickEventArgs e)
    {
        degreecnt = degreecnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstdegree.Items[r].Selected = false;
        txtdegree.Text = Label8.Text + "(" + degreecnt.ToString() + ")";
        if (txtdegree.Text == Label8.Text + "(0)")
        {
            txtdegree.Text = "---Select---";
        }
    }
    public Label degreelabel()
    {
        Label lbc = new Label();
        ViewState["lseatcontrol"] = true;
        return (lbc);
    }
    public ImageButton degreeimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }
    //------Load Function for the Branch Details-----
    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = chklstdegree.Items[i].Value.ToString();
                    }
                    else
                    {
                        course_id = course_id + "'" + "," + "'" + chklstdegree.Items[i].Value.ToString();
                    }
                }
            }
            if (cblclg.Items.Count > 0)
            {
                string clgcode = "";
                for (int clg = 0; clg < cblclg.Items.Count; clg++)
                {
                    if (cblclg.Items[clg].Selected == true)
                    {
                        if (clgcode == "")
                            clgcode = cblclg.Items[clg].Value;
                        else
                            clgcode = clgcode + "," + cblclg.Items[clg].Value;
                    }
                }
                //   collegecode = Session["collegecode"].ToString();
                collegecode = clgcode;
            }
            //course_id = chklstdegree.SelectedValue.ToString();
            chklstbranch.Items.Clear();
            txtbranch.Text = "--Select--";
            //if (group_user.Contains(';'))
            //{
            //    string[] group_semi = group_user.Split(';');
            //    group_user = group_semi[0].ToString();
            //}
            //ds2.Dispose();
            //ds2.Reset();
            //ds2 = dacces2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            // string sel = " select * from Department dt,Degree d where d.Dept_Code=dt.Dept_Code and d.Degree_Code in('" + course_id + "') and  d.college_code in(" + collegecode + ")";
            if (!string.IsNullOrEmpty(collegecode))
            {
                string sel = "  select dt.Dept_Name,dt.dept_code from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.Course_Id in('" + course_id + "') and d.college_code in(" + collegecode + ")";
                ds2 = dacces2.select_method_wo_parameter(sel, "Text");
            }
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                chklstbranch.DataSource = ds2;
                chklstbranch.DataTextField = "dept_name";
                chklstbranch.DataValueField = "dept_code";
                chklstbranch.DataBind();
                chklstbranch.Items[0].Selected = true;
                txtbranch.Text = Label9.Text + "(" + 1 + ")";
            }
        }
        catch (Exception ex)
        {
            //lblMainError.Text = "Please Select the Degree";
        }
    }
    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        if (chkbranch.Checked == true)
        {
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                chklstbranch.Items[i].Selected = true;
                txtbranch.Text = Label9.Text + "(" + (chklstbranch.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                chklstbranch.Items[i].Selected = false;
                txtbranch.Text = "---Select---";
            }
        }
    }
    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        pbranch.Focus();
        int branchcount = 0;
        string value = "";
        string code = "";
        for (int i = 0; i < chklstbranch.Items.Count; i++)
        {
            if (chklstbranch.Items[i].Selected == true)
            {
                value = chklstbranch.Items[i].Text;
                code = chklstbranch.Items[i].Value.ToString();
                branchcount = branchcount + 1;
                txtbranch.Text = Label9.Text + "(" + branchcount.ToString() + ")";
            }
        }
        if (branchcount == 0)
            txtbranch.Text = "---Select---";
        else
        {
            Label lbl = branchlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = branchimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(branchimg_Click);
        }
        branchcnt = branchcount;
    }
    protected void LinkButtonbranch_Click(object sender, EventArgs e)
    {
        chklstbranch.ClearSelection();
        branchcnt = 0;
        txtbranch.Text = "---Select---";
    }
    public void branchimg_Click(object sender, ImageClickEventArgs e)
    {
        branchcnt = branchcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstbranch.Items[r].Selected = false;
        txtdegree.Text = Label9.Text + "(" + branchcnt.ToString() + ")";
        if (txtdegree.Text == Label9.Text + "(0)")
        {
            txtdegree.Text = "---Select---";
        }
    }
    public Label branchlabel()
    {
        Label lbc = new Label();
        ViewState["lseatcontrol"] = true;
        return (lbc);
    }
    public ImageButton branchimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }
    protected void chksatff_CheckedChanged(object sender, EventArgs e)
    {
        if (chksatff.Checked == true)
        {
            for (int i = 0; i < chklststaff.Items.Count; i++)
            {
                chklststaff.Items[i].Selected = true;
                txtstaff.Text = "Staff(" + (chklststaff.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklststaff.Items.Count; i++)
            {
                chklststaff.Items[i].Selected = false;
                txtstaff.Text = "---Select---";
            }
        }
    }
    protected void chklststaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        int batchcount = 0;
        string value = "";
        string code = "";
        for (int i = 0; i < chklststaff.Items.Count; i++)
        {
            if (chklststaff.Items[i].Selected == true)
            {
                value = chklststaff.Items[i].Text;
                code = chklststaff.Items[i].Value.ToString();
                batchcount = batchcount + 1;
                txtstaff.Text = "Staff(" + batchcount.ToString() + ")";
            }
        }
    }
    protected void chksatffDept_CheckedChanged(object sender, EventArgs e)
    {
        if (chksatffDept.Checked == true)
        {
            for (int i = 0; i < chklststaffDept.Items.Count; i++)
            {
                chklststaffDept.Items[i].Selected = true;
                txtstaffDept.Text = "Staff(" + (chklststaffDept.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklststaffDept.Items.Count; i++)
            {
                chklststaffDept.Items[i].Selected = false;
                txtstaffDept.Text = "---Select---";
            }
        }
    }
    protected void chklststaffDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        int batchcount = 0;
        string value = "";
        string code = "";
        for (int i = 0; i < chklststaffDept.Items.Count; i++)
        {
            if (chklststaffDept.Items[i].Selected == true)
            {
                value = chklststaffDept.Items[i].Text;
                code = chklststaffDept.Items[i].Value.ToString();
                batchcount = batchcount + 1;
                txtstaffDept.Text = "Staff(" + batchcount.ToString() + ")";
            }
        }
    }
    #region button delete
    protected void Btn_Delete_Click(object sender, EventArgs e)
    {
        if (rbdirectapply.Checked == true)
        {
            //check school or college setting 
            checkSchoolSetting();
            bool check = false;
            bool typeval = false;
            bool paidcheck = false;
            bool ftmnth = false;
            bool removebool = false;
            string statgid = "";
            string type = "";
            string appno = "";
            if (rbsemtype.Checked == true)
                type = "Semester";
            if (rbstutype.Checked == true)
                type = "Yearly";
            if (rbtranfer.Checked == true)
                type = "Monthly";
            if (rbtermtype.Checked == true)
                type = "Term";
            string place = Convert.ToString(tbborplace.Text);
            string rollno = Convert.ToString(tbenqno.Text);
            string studname = Convert.ToString(tbpname.Text);
            string ledgPK = "";
            string year = "";
            string category = "";
            if (ViewState["Clgcode"] != null)
            {
                // collegecode = Convert.ToString(ViewState["Clgcode"]);
                if (ddlclgstud.Items.Count > 0)
                    collegecode = Convert.ToString(ddlclgstud.SelectedValue);
            }
            int semandyear = 0;
            ///
            string getactcode = dacces2.GetFunction("select LinkValue from  InsSettings where LinkName='Current Financial Year' and college_code='" + collegecode + "'");
            string strRoll = string.Empty;
            if (schlSettCode != 0)
                strRoll = " Roll_No='" + rollno + "'";
            else
                strRoll = " roll_admit='" + rollno + "'";
            appno = dacces2.GetFunction("select app_no from Registration where " + strRoll + " and college_code='" + collegecode + "'");
            statgid = GetFunction("select Stage_id from stage_master where Stage_Name = '" + place + "'");
            if (statgid != "0")
            {
                //header and ledger
                string transset = dacces2.GetFunction(" select LinkValue from New_InsSettings where LinkName='TransportLedgerValue'  and user_code='" + usercode + "' and college_code='" + collegecode + "'");
                if (transset != "")
                {
                    string[] leng = transset.Split(',');
                    if (leng.Length == 2)
                    {
                        header_id = Convert.ToString(leng[0]);
                        // Fee_Code = Convert.ToString(leng[1]);
                        ledgPK = Convert.ToString(leng[1]);
                    }
                }
                typeval = true;
                //cost amount
                Cost = dacces2.GetFunction("select CAST(f.cost AS INT) as Cost from FeeInfo f where f.StrtPlace = '" + statgid + "' and f.payType = '" + type + "' and college_code='" + collegecode + "'");
            }
            string selsctfeecate = dacces2.GetFunction("select distinct current_semester from registration where " + strRoll + " and college_code='" + collegecode + "'");
            //yearwise
            if (rbstutype.Checked == true)
            {
                #region year
                semandyear = 1;
                if (selsctfeecate == "1" || selsctfeecate == "2")
                    semval = "1 Year";
                else if (selsctfeecate == "3" || selsctfeecate == "4")
                    semval = "2 Year";
                else if (selsctfeecate == "5" || selsctfeecate == "6")
                    semval = "3 Year";
                else if (selsctfeecate == "7" || selsctfeecate == "8")
                    semval = "4 Year";
                #endregion
            }
            //semesterwise
            else if (rbsemtype.Checked == true)
            {
                #region semester
                semandyear = 1;
                if (selsctfeecate == "1")
                    semval = "1 Semester";
                if (selsctfeecate == "2")
                    semval = "2 Semester";
                if (selsctfeecate == "3")
                    semval = "3 Semester";
                if (selsctfeecate == "4")
                    semval = "4 Semester";
                if (selsctfeecate == "5")
                    semval = "5 Semester";
                if (selsctfeecate == "6")
                    semval = "6 Semester";
                if (selsctfeecate == "7")
                    semval = "7 Semester";
                if (selsctfeecate == "8")
                    semval = "8 Semester";
                if (selsctfeecate == "9")
                    semval = "9 Semester";
                #endregion
            }
            else if (rbtranfer.Checked == true)
            {
                #region month
                semandyear = 2;
                if (ddlmonth.SelectedItem.Text != "Month")
                    month = Convert.ToString(ddlmonth.SelectedItem.Value);
                if (ddlyear.SelectedItem.Text != "Year")
                    year = Convert.ToString(ddlyear.SelectedItem.Text);
                semval = feecatValue(selsctfeecate);
                string[] spl_sem = semval.Split(' ');
                string curr_sem = spl_sem[0].ToString();
                if (curr_sem != "")
                {
                    if (Convert.ToInt32(curr_sem) % 2 == 0)
                        category = "Even";
                    else
                        category = "Odd";
                }
                #endregion
            }
            else if (rbtermtype.Checked == true)
            {
                #region Term
                semandyear = 1;
                if (selsctfeecate == "1")
                    semval = "Term 1";
                else if (selsctfeecate == "2")
                    semval = "Term 2";
                else if (selsctfeecate == "3")
                    semval = "Term 3";
                else if (selsctfeecate == "4")
                    semval = "Term 4";
                #endregion
            }
            string feecatg = dacces2.GetFunction("select distinct textcode from textvaltable where textcriteria ='FEECA' and textval='" + semval + "' and college_code='" + collegecode + "'");
            if (place != "" && rollno != "" && feecatg != "" && feecatg != "0")
            {
                if (semandyear == 1)
                {
                    if (appno != "0" && header_id != "" && ledgPK != "" && feecatg != "")
                    {
                        double paidamt = 0;
                        double BalAmount = 0;
                        double.TryParse(Convert.ToString(dacces2.GetFunction("select PaidAmount from FT_FeeAllot where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "'")), out paidamt);
                        double.TryParse(Convert.ToString(dacces2.GetFunction("select BalAmount from FT_FeeAllot where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "'")), out BalAmount);
                        if (paidamt == 0 && BalAmount != 0)
                        {
                            string DelQ = "    if exists (select * from FT_FeeAllot where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "' )delete from FT_FeeAllot where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "'";
                            int falt = dacces2.update_method_wo_parameter(DelQ, "Text");
                            string querystu;
                            querystu = "update registration set Bus_RouteID='',Boarding='',VehID='',Seat_No='',Trans_PayType='',Traveller_Date = '' where " + strRoll + " and Stud_Name='" + studname + "' and college_code='" + collegecode + "'";
                            dacces2.update_method_wo_parameter(querystu, "Text");
                            check = true;
                            paidcheck = true;
                        }
                    }
                }
                else
                {
                    #region month
                    string fnlmnth = "";
                    string remove = "";
                    string costamt = "";
                    string Feemnth = dacces2.GetFunction("select FeeAmountMonthly from FT_FeeAllot where App_No='" + appno + "' and FeeCategory ='" + feecatg + "' and LedgerFK = '" + ledgPK + "'");
                    if (Feemnth != "" && Feemnth != "0")
                    {
                        string[] value = Feemnth.Split(',');
                        for (int i = 0; i < value.Length; i++)
                        {
                            string[] mnthval = value[i].Split(':');
                            {
                                if (mnthval.Length > 0)
                                {
                                    if (mnthval[0] == month && mnthval[1] == year)
                                    {
                                        remove = mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                        string ftPK = dacces2.GetFunction("select FeeAllotPK from FT_FeeAllot where App_No='" + appno + "' and FeeCategory ='" + feecatg + "' and LedgerFK = '" + ledgPK + "'");
                                        removebool = true;
                                        if (ftPK != "0" && ftPK != "")
                                        {
                                            string FTpadiamt = dacces2.GetFunction("select PaidAmount from FT_FeeallotMonthly where AllotMonth='" + mnthval[0] + "' and AllotYear='" + mnthval[1] + "' and FeeAllotPK='" + ftPK + "'");
                                            if (FTpadiamt != "" && FTpadiamt != "0")
                                            {
                                                ftmnth = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (fnlmnth == "")
                                        {
                                            fnlmnth = mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                        }
                                        else
                                        {
                                            fnlmnth = fnlmnth + "," + mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //2:2016:100,3:2 016:200     
                    if (remove != "" && Cost != "")
                    {
                        if (ftmnth == false)
                        {
                            string querystu1 = "if exists (select * from FT_FeeAllot where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "' ) update FT_FeeAllot set FeeAmount=FeeAmount -'" + Cost + "',TotalAmount =TotalAmount -'" + Cost + "' ,BalAmount =BalAmount -'" + Cost + "', FeeAmountMonthly='" + fnlmnth + "'  where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "'  ";
                            int saveupdate = dacces2.update_method_wo_parameter(querystu1, "Text");
                            string allotpk = dacces2.GetFunction(" select FeeAllotPK from FT_FeeAllot  where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "'");
                            if (allotpk != "" && month != "" && year != "" && getactcode != "")
                            {
                                string InsertQ = "if exists(select * from FT_FeeallotMonthly where FeeAllotPK='" + allotpk + "' and AllotMonth='" + month + "' and AllotYear='" + year + "' and FinYearFK='" + getactcode + "') delete from FT_FeeallotMonthly where FeeAllotPK='" + allotpk + "' and AllotMonth='" + month + "' and AllotYear='" + year + "' and FinYearFK='" + getactcode + "'";
                                int ins = dacces2.update_method_wo_parameter(InsertQ, "Text");
                            }
                            string querystu;
                            querystu = "update registration set Bus_RouteID='',Boarding='',VehID='',Seat_No='',Trans_PayType='',Traveller_Date = '' where " + strRoll + " and Stud_Name='" + studname + "' and college_code='" + collegecode + "'";
                            dacces2.update_method_wo_parameter(querystu, "Text");
                            check = true;
                            paidcheck = true;
                        }
                    }
                    #endregion
                }
            }
            if (semandyear == 1)
            {
                if (typeval == true)
                {
                    if (paidcheck == true)
                    {
                        if (check == true)
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = "Deleted successfully";
                            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted successfully')", true);
                        }
                        else
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = "Please fill the Valid Details";
                            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please fill the Valid Details')", true);
                        }
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Paid Amount Available So Cant Delete";
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Paid Amount Available So Cant Delete')", true);
                    }
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "No Fees Available";
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Fees Available ')", true);
                }
            }
            else
            {
                if (removebool == true)
                {
                    if (paidcheck == true)
                    {
                        if (check == true)
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = "Deleted successfully";
                            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted successfully')", true);
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please fill the Valid Details')", true);
                            imgAlert.Visible = true;
                            lbl_alert.Text = "Please fill the Valid Details";
                        }
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Paid Amount Available So Cant Delete')", true);
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Paid Amount Available So Cant Delete";
                    }
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Fees Available ')", true);
                    imgAlert.Visible = true;
                    lbl_alert.Text = "No Fees Available ";
                }
            }
        }
        else if (rbenquiry.Checked == true)
        {
            con.Close();
            con.Open();
            string querystu1;
            querystu1 = "update staffmaster set Bus_RouteID='',Boarding='',VehID='',Seat_No='',Traveller_Date = '' where staff_code='" + tbenqno.Text + "' and staff_name='" + tbpname.Text + "' and college_code='" + collegecode + "'";
            SqlCommand cmdtype = new SqlCommand(querystu1, con);
            cmdtype.ExecuteNonQuery();
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted successfully')", true);
            imgAlert.Visible = true;
            lbl_alert.Text = "Deleted successfully ";
        }
        btnMainGo_Click(sender, e);
        Buttondelete_Click(sender, e);
    }
    #endregion
    #region region Cancel
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (rbdirectapply.Checked == true)
        {
            //school or college check setting 
            checkSchoolSetting();
            bool check = false;
            bool typeval = false;
            bool paidcheck = false;
            bool ftmnth = false;
            bool removebool = false;
            string statgid = "";
            string type = "";
            string appno = "";
            if (rbsemtype.Checked == true)
                type = "Semester";
            else if (rbstutype.Checked == true)
                type = "Yearly";
            else if (rbtranfer.Checked == true)
                type = "Monthly";
            else if (rbtermtype.Checked == true)
                type = "Term";
            string place = Convert.ToString(tbborplace.Text);
            string rollno = Convert.ToString(tbenqno.Text);
            string studname = Convert.ToString(tbpname.Text);
            string ledgPK = "";
            string year = "";
            string category = "";
            if (ddlclgstud.Items.Count > 0)
                collegecode = Convert.ToString(ddlclgstud.SelectedValue);
            //if (ViewState["Clgcode"] != null)
            // collegecode = Convert.ToString(ViewState["Clgcode"]);
            int semandyear = 0;
            ///
            string getactcode = dacces2.GetFunction("select LinkValue from  InsSettings where LinkName='Current Financial Year' and college_code='" + collegecode + "'");
            string strRoll = string.Empty;
            if (schlSettCode != 0)
                strRoll = " Roll_No='" + rollno + "'";
            else
                strRoll = " roll_admit='" + rollno + "'";
            appno = dacces2.GetFunction("select app_no from Registration where " + strRoll + " and college_code='" + collegecode + "'");
            statgid = GetFunction("select Stage_id from stage_master where Stage_Name = '" + place + "'");
            if (statgid != "0")
            {
                //header and ledger
                string transset = dacces2.GetFunction(" select LinkValue from New_InsSettings where LinkName='TransportLedgerValue'  and user_code='" + usercode + "' and college_code='" + collegecode + "'");
                if (transset != "")
                {
                    string[] leng = transset.Split(',');
                    if (leng.Length == 2)
                    {
                        header_id = Convert.ToString(leng[0]);
                        // Fee_Code = Convert.ToString(leng[1]);
                        ledgPK = Convert.ToString(leng[1]);
                    }
                }
                typeval = true;
                //cost amount
                Cost = dacces2.GetFunction("select CAST(f.cost AS INT) as Cost from FeeInfo f where f.StrtPlace = '" + statgid + "' and f.payType = '" + type + "' and college_code='" + collegecode + "'");
            }
            string selsctfeecate = dacces2.GetFunction("select distinct current_semester from registration where " + strRoll + " and college_code='" + collegecode + "'");
            //yearwise
            if (rbstutype.Checked == true)
            {
                #region year
                semandyear = 1;
                if (selsctfeecate == "1" || selsctfeecate == "2")
                    semval = "1 Year";
                else if (selsctfeecate == "3" || selsctfeecate == "4")
                    semval = "2 Year";
                else if (selsctfeecate == "5" || selsctfeecate == "6")
                    semval = "3 Year";
                else if (selsctfeecate == "7" || selsctfeecate == "8")
                    semval = "4 Year";
                #endregion
            }
            //semesterwise
            else if (rbsemtype.Checked == true)
            {
                #region semester
                semandyear = 1;
                if (selsctfeecate == "1")
                    semval = "1 Semester";
                if (selsctfeecate == "2")
                    semval = "2 Semester";
                if (selsctfeecate == "3")
                    semval = "3 Semester";
                if (selsctfeecate == "4")
                    semval = "4 Semester";
                if (selsctfeecate == "5")
                    semval = "5 Semester";
                if (selsctfeecate == "6")
                    semval = "6 Semester";
                if (selsctfeecate == "7")
                    semval = "7 Semester";
                if (selsctfeecate == "8")
                    semval = "8 Semester";
                if (selsctfeecate == "9")
                    semval = "9 Semester";
                #endregion
            }
            else if (rbtranfer.Checked == true)
            {
                #region month
                semandyear = 2;
                if (ddlmonth.SelectedItem.Text != "Month")
                    month = Convert.ToString(ddlmonth.SelectedItem.Value);
                if (ddlyear.SelectedItem.Text != "Year")
                    year = Convert.ToString(ddlyear.SelectedItem.Text);
                semval = feecatValue(selsctfeecate);
                string[] spl_sem = semval.Split(' ');
                string curr_sem = spl_sem[0].ToString();
                if (curr_sem != "")
                {
                    if (Convert.ToInt32(curr_sem) % 2 == 0)
                        category = "Even";
                    else
                        category = "Odd";
                }
                #endregion
            }
            else if (rbtermtype.Checked == true)
            {
                #region year
                semandyear = 1;
                if (selsctfeecate == "1")
                    semval = "Term 1";
                else if (selsctfeecate == "2")
                    semval = "Term 2";
                else if (selsctfeecate == "3")
                    semval = "Term 3";
                else if (selsctfeecate == "4")
                    semval = "Term 4";
                #endregion
            }
            string feecatg = dacces2.GetFunction("select distinct textcode from textvaltable where textcriteria ='FEECA' and textval='" + semval + "' and college_code='" + collegecode + "'");
            if (place != "" && rollno != "" && feecatg != "" && feecatg != "0")
            {
                if (semandyear == 1)
                {
                    if (appno != "0" && header_id != "" && ledgPK != "" && feecatg != "")
                    {
                        double paidamt = 0;
                        double BalAmount = 0;
                        double.TryParse(Convert.ToString(dacces2.GetFunction("select PaidAmount from FT_FeeAllot where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "'")), out paidamt);
                        double.TryParse(Convert.ToString(dacces2.GetFunction("select BalAmount from FT_FeeAllot where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "'")), out BalAmount);
                        if (paidamt == 0 && BalAmount != 0 || paidamt != 0 && BalAmount == 0)
                        {
                            //string DelQ = "    if exists (select * from FT_FeeAllot where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "' )delete from FT_FeeAllot where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "'";
                            // int falt = dacces2.update_method_wo_parameter(DelQ, "Text");
                            string querystu;
                            // querystu = "update registration set Bus_RouteID='',Boarding='',VehID='',Seat_No='',Trans_PayType='',Traveller_Date = '' where Roll_No='" + rollno + "' and Stud_Name='" + studname + "'";statgid
                            querystu = "update registration set IsCanceledStage='1' where " + strRoll + " and Stud_Name='" + studname + "' and Boarding='" + statgid + "' and college_code='" + collegecode + "'";
                            dacces2.update_method_wo_parameter(querystu, "Text");
                            check = true;
                            paidcheck = true;
                        }
                    }
                }
                else
                {
                    string fnlmnth = "";
                    string remove = "";
                    string costamt = "";
                    string Feemnth = dacces2.GetFunction("select FeeAmountMonthly from FT_FeeAllot where App_No='" + appno + "' and FeeCategory ='" + feecatg + "' and LedgerFK = '" + ledgPK + "'");
                    if (Feemnth != "" && Feemnth != "0")
                    {
                        string[] value = Feemnth.Split(',');
                        for (int i = 0; i < value.Length; i++)
                        {
                            string[] mnthval = value[i].Split(':');
                            {
                                if (mnthval.Length > 0)
                                {
                                    if (mnthval[0] == month && mnthval[1] == year)
                                    {
                                        remove = mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                        string ftPK = dacces2.GetFunction("select FeeAllotPK from FT_FeeAllot where App_No='" + appno + "' and FeeCategory ='" + feecatg + "' and LedgerFK = '" + ledgPK + "'");
                                        removebool = true;
                                        if (ftPK != "0" && ftPK != "")
                                        {
                                            string FTpadiamt = dacces2.GetFunction("select PaidAmount from FT_FeeallotMonthly where AllotMonth='" + mnthval[0] + "' and AllotYear='" + mnthval[1] + "' and FeeAllotPK='" + ftPK + "'");
                                            if (FTpadiamt != "" && FTpadiamt != "0")
                                            {
                                                ftmnth = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (fnlmnth == "")
                                        {
                                            fnlmnth = mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                        }
                                        else
                                        {
                                            fnlmnth = fnlmnth + "," + mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //2:2016:100,3:2 016:200     
                    if (remove != "" && Cost != "")
                    {
                        if (ftmnth == false)
                        {
                            //string querystu1 = "if exists (select * from FT_FeeAllot where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "' ) update FT_FeeAllot set FeeAmount=FeeAmount -'" + Cost + "',TotalAmount =TotalAmount -'" + Cost + "' ,BalAmount =BalAmount -'" + Cost + "', FeeAmountMonthly='" + fnlmnth + "'  where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "'  ";
                            // int saveupdate = dacces2.update_method_wo_parameter(querystu1, "Text");
                            string allotpk = "";
                            // string allotpk = dacces2.GetFunction(" select FeeAllotPK from FT_FeeAllot  where App_No ='" + appno + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecatg + "'");
                            if (allotpk != "" && month != "" && year != "" && getactcode != "")
                            {
                                // string InsertQ = "if exists(select * from FT_FeeallotMonthly where FeeAllotPK='" + allotpk + "' and AllotMonth='" + month + "' and AllotYear='" + year + "' and FinYearFK='" + getactcode + "') delete from FT_FeeallotMonthly where FeeAllotPK='" + allotpk + "' and AllotMonth='" + month + "' and AllotYear='" + year + "' and FinYearFK='" + getactcode + "'";
                                //  int ins = dacces2.update_method_wo_parameter(InsertQ, "Text");
                            }
                            string querystu;
                            querystu = "update registration set IsCanceledStage='1' where " + strRoll + " and Stud_Name='" + studname + "' and Boarding='" + statgid + "' and college_code='" + collegecode + "'";
                            dacces2.update_method_wo_parameter(querystu, "Text");
                            check = true;
                            paidcheck = true;
                        }
                    }
                }
            }
            if (semandyear == 1)
            {
                if (typeval == true)
                {
                    if (paidcheck == true)
                    {
                        if (check == true)
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = "Canceled successfully";
                        }
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Canceled successfully')", true);
                        else
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = "Please fill the Valid Details";
                        }
                        // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please fill the Valid Details')", true);
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Paid Amount Available So Cant Cancel";
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Paid Amount Available So Cant Cancel')", true);
                    }
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "No Fees Available ";
                }
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Fees Available ')", true);
            }
            else
            {
                if (removebool == true)
                {
                    if (paidcheck == true)
                    {
                        if (check == true)
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = "Canceled successfully";
                        }
                        // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Canceled successfully')", true);
                        else
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = "Please fill the Valid Details";
                        }
                        // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please fill the Valid Details')", true);
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Paid Amount Available So Cant Cancel')", true);
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Paid Amount Available So Cant Cancel";
                    }
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "No Fees Available";
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Fees Available ')", true);
                }
            }
        }
        else if (rbenquiry.Checked == true)
        {
            con.Close();
            con.Open();
            string querystu1;
            //querystu1 = "update staffmaster set Bus_RouteID='',Boarding='',VehID='',Seat_No='',Traveller_Date = '' where staff_code='" + tbenqno.Text + "' and staff_name='" + tbpname.Text + "'";
            querystu1 = "update staffmaster set IsCanceledStage='1', Bus_RouteID='',Boarding='',VehID='',Seat_No='',Traveller_Date = '' where staff_code='" + tbenqno.Text + "' and staff_name='" + tbpname.Text + "' and college_code='" + collegecode + "'";
            SqlCommand cmdtype = new SqlCommand(querystu1, con);
            cmdtype.ExecuteNonQuery();
            // and Boarding='" + statgid + "'
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Canceled successfully')", true);
            imgAlert.Visible = true;
            lbl_alert.Text = "Canceled successfully";
        }
        btnMainGo_Click(sender, e);
        Buttondelete_Click(sender, e);
    }
    #endregion
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = "Traveller Allotment Report";
        string pagename = "Traveller_NewPage.aspx";
        Session["column_header_row_count"] = Fpload.ColumnHeader.RowCount;
        Printcontrol.loadspreaddetails(Fpload, pagename, degreedetails);
        Printcontrol.Visible = true;
    }
    public void loadstage()
    {
        ds2.Dispose();
        ds2.Reset();
        ds2 = dacces2.Bindplace();
        int count = 0;
        if (ds2.Tables[0].Rows.Count > 0)
        {
            ddlstage.DataSource = ds2;
            ddlstage.DataTextField = "Stage_Name";
            ddlstage.DataValueField = "Stage_Name";
            ddlstage.DataBind();
        }
    }
    protected void ddlstage_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    //[System.Web.Script.Services.ScriptMethod()]
    //[System.Web.Services.WebMethod]
    //public static List<string> GetListofCountries(string prefixText)
    //{
    //    string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    //    using (SqlConnection sqlconn = new SqlConnection(cs))
    //    {
    //        sqlconn.Open();
    //        SqlCommand cmd = new SqlCommand("select Stage_id,Stage_Name,Address,District from stage_master where Stage_Name like '" + prefixText + "%' ", sqlconn);
    //        cmd.Parameters.AddWithValue("@Stage_Name", prefixText);
    //        SqlDataAdapter da = new SqlDataAdapter(cmd);
    //        DataTable dt = new DataTable();
    //        da.Fill(dt);
    //        List<string> CountryNames = new List<string>();
    //        for (int i = 0; i < dt.Rows.Count; i++)
    //        {
    //            //CountryNames.Add(dt.Rows[i]["stud_name"].ToString() + "|" + dt.Rows[i]["roll_no"].ToString() + "|" + dt.Rows[i]["reg_no"].ToString() + "\n\n");
    //            CountryNames.Add(dt.Rows[i]["Stage_Name"].ToString());
    //        }
    //        return CountryNames;
    //    }
    //}
    protected void tbborplace_TextChanged(object sender, EventArgs e)
    {
    }
    public bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
    {
        Double result;
        return Double.TryParse(val, NumberStyle,
            System.Globalization.CultureInfo.CurrentCulture, out result);
    }
    protected void ddlcollegenew_SelectedIndexChanged(object sender, EventArgs e)
    {
        loaddetails();
    }
    public void loaddetails()
    {
        bindBatch1();
        bindcourse();
        if (ddlDegree.Items.Count > 0)
        {
            bindBranch();
        }
    }
    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value;
            }
        }
        return null;
    }
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                dacces2.printexcelreport(Fpload, reportname);
            }
            else
            {
                txtexcelname.Focus();
                //  lblerrmainapp.Text = "Please Enter Your Report Name";
                // lblerrmainapp.Visible = true;lbprint
                lbprint.Text = "Please Enter Your Report Name";
                lbprint.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerrmainapp.Text = ex.ToString();
        }
    }
    #region all update
    protected void btnfeeset_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean allotflag = false;
            Boolean saveflag = false;
            string year = "";
            string mnthamt = "";
            string mnthcol = "";
            string mnthvalue = "";
            string mnthcol1 = "";
            string mnthvalue1 = "";
            int balamt = 0;
            hat.Clear();
            if (ddlclgstud.Items.Count > 0)
                collegecode = Convert.ToString(ddlclgstud.SelectedValue);
            //if (ViewState["Clgcode"] != null)
            //{
            //    collegecode = Convert.ToString(ViewState["Clgcode"]);
            //}
            //  collegecode = Session["collegecode"].ToString();
            string type = "", category = "";
            string getactcode = dacces2.getCurrentFinanceYear(usercode, collegecode);
            // string getactcode = dacces2.GetFunction("select LinkValue from InsSettings where LinkName like 'Current%' college_code='" + collegecode + "'");
            if (getactcode.Trim() != "" && getactcode.Trim() != "0")
            {
                //check scholl or college setting 
                checkSchoolSetting();
                string strtype = dacces2.GetFunction("select LinkValue from New_InsSettings where college_code='" + collegecode + "' and LinkName='Fee Yearwise' And user_code = '" + usercode + "'");
                if (strtype == "1")
                    type = "Yearly";
                else if (strtype == "2")
                    type = "Term";
                else
                    type = "Semester";
                if (rbsemtype.Checked == true)
                    type = "Semester";
                if (rbstutype.Checked == true)
                    type = "Yearly";
                if (rbtranfer.Checked == true)
                    type = "Monthly";
                if (rbtermtype.Checked == true)
                    type = "Term";
                string currsem = "";
                string feesem = "";
                string textcode = "";
                for (int i = 0; i < chklsfeeset.Items.Count; i++)
                {
                    if (chklsfeeset.Items[i].Selected == true)
                    {
                        if (feesem == "")
                        {
                            feesem = chklsfeeset.Items[i].Text;
                            textcode = chklsfeeset.Items[i].Value.ToString();
                            hat.Add(feesem, textcode);
                            if (strtype == "1")
                            {
                                string[] sp = chklsfeeset.Items[i].Text.Split(' ');
                                if (sp[0] == "1")
                                    currsem = "1,2";
                                else if (sp[0] == "2")
                                    currsem = "3,4";
                                else if (sp[0] == "3")
                                    currsem = "5,6";
                                else if (sp[0] == "4")
                                    currsem = "7,8";
                            }
                            else if (strtype == "2")
                            {
                                string[] sp = chklsfeeset.Items[i].Text.Split(' ');
                                currsem = sp[1];
                            }
                            else
                            {
                                string[] sp = chklsfeeset.Items[i].Text.Split(' ');
                                currsem = sp[0];
                            }
                        }
                        else
                        {
                            feesem = feesem + ',' + chklsfeeset.Items[i].Text;
                            textcode = textcode + ',' + chklsfeeset.Items[i].Value.ToString();
                            hat.Add(chklsfeeset.Items[i].Text.ToString(), chklsfeeset.Items[i].Value.ToString());
                            if (strtype == "1")
                            {
                                string[] sp = chklsfeeset.Items[i].Text.Split(' ');
                                if (sp[0] == "1")
                                    currsem = currsem + ',' + "1,2";
                                else if (sp[0] == "2")
                                    currsem = currsem + ',' + "3,4";
                                else if (sp[0] == "3")
                                    currsem = currsem + ',' + "5,6";
                                else if (sp[0] == "4")
                                    currsem = currsem + ',' + "7,8";
                            }
                            else if (strtype == "2")
                            {
                                string[] sp = chklsfeeset.Items[i].Text.Split(' ');
                                currsem = currsem + ',' + sp[1];
                            }
                            else
                            {
                                string[] sp = chklsfeeset.Items[i].Text.Split(' ');
                                currsem = currsem + ',' + sp[0];
                            }
                        }
                    }
                }
                string rollid = Convert.ToString(tbenqno.Text);
                string strroll = string.Empty;
                if (schlSettCode != 0)
                    strroll = "and roll_no='" + rollid + "'";
                else
                    strroll = "and roll_admit='" + rollid + "'";
                string strrool = "select r.roll_no,r.App_No,r.roll_admit,r.batch_year,r.Current_Semester,VehID,Bus_RouteID,Boarding from Registration r where r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar'  and VehID<>'' and r.Bus_RouteID<>'' and Boarding<>'' " + strroll + " and college_code=" + collegecode + "";
                ds = dacces2.select_method_wo_parameter(strrool, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Fee_Code = "";
                        Double cost_total = 0;
                        category = "";
                        string insertquery = "";
                        int insert = 0;
                        string rollno = ds.Tables[0].Rows[i]["roll_no"].ToString();
                        string rolladmit = ds.Tables[0].Rows[i]["App_No"].ToString();
                        string batch = ds.Tables[0].Rows[i]["batch_year"].ToString();
                        string sem = ds.Tables[0].Rows[i]["Current_Semester"].ToString();
                        string vechile = ds.Tables[0].Rows[i]["VehID"].ToString();
                        string route = ds.Tables[0].Rows[i]["Bus_RouteID"].ToString();
                        // string boadring = ds.Tables[0].Rows[i]["Boarding"].ToString();
                        string boadring = GetFunction("select Stage_id from stage_master where Stage_Name = '" + tbborplace.Text + "'");
                        //if (boadring.Trim() != "")
                        //{
                        //    string selectquery = "select f.cost,f.Fee_Code from FeeInfo f,FM_HeaderMaster h,FM_LedgerMaster i where f.Fee_Code = i.LedgerPK and h.HeaderPK = i.HeaderFK and f.StrtPlace = '" + boadring + "' and f.payType='" + type + "'-- and f.category='" + category + "' ";   // and f.StrtPlace = '" + boadring + "' and f.payType = '" + type + "' and f.category='" + category + "' 
                        //    DataSet dsselectquery = dacces2.select_method_wo_parameter(selectquery, "Text");
                        //    for (int i1 = 0; i1 < dsselectquery.Tables[0].Rows.Count; i1++)
                        //    {
                        //        Fee_Code = dsselectquery.Tables[0].Rows[i1]["Fee_Code"].ToString();
                        //        Cost = dsselectquery.Tables[0].Rows[i1]["Cost"].ToString();
                        //        header_id = dacces2.GetFunction("select distinct HeaderFK from FM_LedgerMaster where LedgerPK = '" + Fee_Code + "'");
                        //    }
                        //}
                        if (boadring != "0")
                        {
                            //header and ledger
                            string transset = dacces2.GetFunction(" select LinkValue from New_InsSettings where LinkName='TransportLedgerValue'  and user_code='" + usercode + "' and college_code='" + collegecode + "'");
                            if (transset != "")
                            {
                                string[] leng = transset.Split(',');
                                if (leng.Length == 2)
                                {
                                    header_id = Convert.ToString(leng[0]);
                                    Fee_Code = Convert.ToString(leng[1]);
                                }
                            }
                            //cost amount
                            Cost = dacces2.GetFunction("select CAST(f.cost AS INT) as Cost from FeeInfo f where f.StrtPlace = '" + boadring + "' and f.payType = '" + type + "' and college_code='" + collegecode + "'");
                        }
                        if (rbsemtype.Checked == true)
                        {
                            if (sem == "1")
                                semval = "1 Semester";
                            if (sem == "2")
                                semval = "2 Semester";
                            if (sem == "3")
                                semval = "3 Semester";
                            if (sem == "4")
                                semval = "4 Semester";
                            if (sem == "5")
                                semval = "5 Semester";
                            if (sem == "6")
                                semval = "6 Semester";
                            if (sem == "7")
                                semval = "7 Semester";
                            if (sem == "8")
                                semval = "8 Semester";
                            if (sem == "9")
                                semval = "9 Semester";
                        }
                        else if (rbstutype.Checked == true)
                        {
                            if (sem == "1" || sem == "2")
                                semval = "1 Year";
                            else if (sem == "3" || sem == "4")
                                semval = "2 Year";
                            else if (sem == "5" || sem == "6")
                                semval = "3 Year";
                            else if (sem == "7" || sem == "8")
                                semval = "4 Year";
                        }
                        else if (rbtranfer.Checked == true)
                        {
                            if (ddlmonth.SelectedItem.Text != "Month")
                                month = Convert.ToString(ddlmonth.SelectedItem.Value);
                            if (ddlyear.SelectedItem.Text != "Year")
                                year = Convert.ToString(ddlyear.SelectedItem.Text);
                            // if (month != "" && year != "")
                            // {
                            mnthamt = "," + month + ":" + year + ":" + Cost;
                            mnthcol = month + ":" + year + ":" + Cost;
                            // }
                            // else
                            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Month and Year')", true);
                            // return;
                            semval = feecatValue(sem);
                            string[] spl_sem = semval.Split(' ');
                            string curr_sem = spl_sem[0].ToString();
                            if (curr_sem != "")
                            {
                                if (Convert.ToInt32(curr_sem) % 2 == 0)
                                    category = "Even";
                                else
                                    category = "Odd";
                            }
                        }
                        else if (rbtermtype.Checked == true)
                        {
                            if (sem == "1")
                                semval = "Term 1";
                            else if (sem == "2")
                                semval = "Term 2";
                            else if (sem == "3")
                                semval = "Term 3";
                            else if (sem == "4")
                                semval = "Term 4";
                        }
                        if (hat.Contains(semval))
                        {
                            tcode = GetCorrespondingKey(semval, hat).ToString();
                        }
                        if (Fee_Code != "")
                        {
                            string rights = dacces2.GetFunction("select LinkValue from InsSettings where linkname = 'Transport Link' and college_code ='" + collegecode + "'");
                            if (rights == "1")
                            {
                                for (int row = 0; row < chklsfeeset.Items.Count; row++)
                                {
                                    if (chklsfeeset.Items[row].Selected == true)
                                    {
                                        string feecat = chklsfeeset.Items[row].Value.ToString();
                                        string queryUpdate1 = "select * from FT_FeeAllot where App_No='" + rolladmit + "' and FeeCategory='" + feecat + "' and LedgerFK = '" + Fee_Code + "'";
                                        DataSet dtnewupdate = dacces2.select_method_wo_parameter(queryUpdate1, "Text");
                                        allotflag = false;
                                        if (dtnewupdate.Tables[0].Rows.Count == 0)
                                        {
                                            if (rbtranfer.Checked == false)
                                            {
                                                insertquery = " if exists (select * from FT_FeeAllot where App_No ='" + rolladmit + "' and LedgerFK='" + Fee_Code + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecat + "' ) update FT_FeeAllot set FeeAmount='" + Cost + "',TotalAmount ='" + Cost + "' ,BalAmount ='" + Cost + "'-isnull(Paidamount,'0') where App_No ='" + rolladmit + "' and LedgerFK='" + Fee_Code + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecat + "' else insert into FT_FeeAllot (App_No,LedgerFK,HeaderFK,FinYearFK,AllotDate,FeeAmount,FeeCategory,PayStartDate,FineAmount,DeductAmount,TotalAmount,BalAmount,MemType,PayMode,DeductReason,FromGovtAmt)  values ('" + rolladmit + "','" + Fee_Code + "','" + header_id + "','" + getactcode + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + Cost + "','" + feecat + "','',0,0,'" + Cost + "','" + Cost + "','1','1',0,0)";
                                                insert = dacces2.update_method_wo_parameter(insertquery, "Text");
                                            }
                                            else
                                            {
                                                string fnlmnth = "";
                                                string Feemnth = dacces2.GetFunction("select FeeAmountMonthly from FT_FeeAllot where App_No='" + Roll_Adm + "' and FeeCategory ='" + tcode + "' and LedgerFK = '" + Fee_Code + "'");
                                                if (Feemnth != "" && Feemnth != "0")
                                                {
                                                    string[] value = Feemnth.Split(',');
                                                    for (int j = 0; j < value.Length; j++)
                                                    {
                                                        string[] mnthval = value[j].Split(';');
                                                        {
                                                            if (mnthval.Length > 0)
                                                            {
                                                                if (mnthval[0] == month && mnthval[1] == year)
                                                                {
                                                                    mnthamt = "";
                                                                    if (Cost == mnthval[2])
                                                                    {
                                                                        mnthval[2] = Cost;
                                                                        Cost = "0";
                                                                    }
                                                                    else if (Convert.ToInt32(Cost) > Convert.ToInt32(mnthval[2]))
                                                                    {
                                                                        balamt = Convert.ToInt32(Cost) - Convert.ToInt32(mnthval[2]);
                                                                        Cost = Convert.ToString(balamt);
                                                                        mnthval[2] = Cost;
                                                                    }
                                                                    else if (Convert.ToInt32(Cost) < Convert.ToInt32(mnthval[2]))
                                                                    {
                                                                        int val = Convert.ToInt32(Cost);
                                                                        balamt = Convert.ToInt32(Cost) - Convert.ToInt32(mnthval[2]);
                                                                        Cost = Convert.ToString(balamt);
                                                                        mnthval[2] = Convert.ToString(val);
                                                                    }
                                                                    if (fnlmnth == "")
                                                                        fnlmnth = mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                                                    else
                                                                        fnlmnth = fnlmnth + "," + mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                                                }
                                                                else
                                                                    if (fnlmnth == "")
                                                                        fnlmnth = mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                                                    else
                                                                        fnlmnth = fnlmnth + "," + mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                                            }
                                                        }
                                                    }
                                                    if (fnlmnth != "")
                                                    {
                                                        fnlmnth = fnlmnth + mnthamt;
                                                    }
                                                }
                                                else
                                                    fnlmnth = mnthcol;
                                                //allot
                                                insertquery = " if exists (select * from FT_FeeAllot where App_No ='" + rolladmit + "' and LedgerFK='" + Fee_Code + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecat + "' ) update FT_FeeAllot set FeeAmount='" + Cost + "',TotalAmount ='" + Cost + "' ,BalAmount ='" + Cost + "'-isnull(PaidAmount,'0'),FeeAmountMonthly='" + fnlmnth + "' where App_No ='" + rolladmit + "' and LedgerFK='" + Fee_Code + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecat + "' else insert into FT_FeeAllot (App_No,LedgerFK,HeaderFK,FinYearFK,AllotDate,FeeAmount,FeeCategory,PayStartDate,FineAmount,DeductAmount,TotalAmount,BalAmount,MemType,PayMode,DeductReason,FromGovtAmt,FeeAmountMonthly)  values ('" + rolladmit + "','" + Fee_Code + "','" + header_id + "','" + getactcode + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + Cost + "','" + feecat + "','',0,0,'" + Cost + "','" + Cost + "','1','1',0,0" + mnthvalue1 + ",'" + fnlmnth + "')";
                                                insert = dacces2.update_method_wo_parameter(insertquery, "Text");
                                                string allotpk = dacces2.GetFunction(" select FeeAllotPK from FT_FeeAllot  where App_No ='" + rolladmit + "' and LedgerFK='" + Fee_Code + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + feecat + "'");
                                                if (allotpk != "" && month != "" && year != "" && getactcode != "")
                                                {
                                                    string InsertQ = "if exists(select * from FT_FeeallotMonthly where FeeAllotPK='" + allotpk + "' and AllotMonth='" + month + "' and AllotYear='" + year + "' and FinYearFK='" + getactcode + "')update FT_FeeallotMonthly set AllotAmount='" + Cost + "',BalAmount='" + Cost + "'-isnull(PaidAmount,'0') where FeeAllotPK='" + allotpk + "' and AllotMonth='" + month + "' and AllotYear='" + year + "' and FinYearFK='" + getactcode + "' else insert into FT_FeeallotMonthly (FeeAllotPK,AllotMonth,AllotYear,AllotAmount,FinYearFK,BalAmount) values('" + allotpk + "','" + month + "','" + year + "','" + Cost + "','" + getactcode + "','" + Cost + "')";
                                                    int ins = dacces2.update_method_wo_parameter(InsertQ, "Text");
                                                }
                                            }
                                            //registraiton upate
                                            string querystu = "update registration set IsCanceledStage='0' where Stud_Name='" + tbpname.Text + "' " + strroll + " and Boarding='" + boadring + "' and college_code='" + collegecode + "'";
                                            dacces2.update_method_wo_parameter(querystu, "Text");
                                            allotflag = true;
                                            saveflag = true;
                                        }
                                        else
                                        {
                                            tbenqno.Text = "";
                                            tbpname.Text = "";
                                            tbdept.Text = "";
                                            tbborplace.Text = "";
                                            tbvehno.Text = "";
                                            tbroute.Text = "";
                                            tbseatno.Text = "";
                                            photo.Visible = false;
                                            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Already Updated')", true);
                                            imgAlert.Visible = true;
                                            lbl_alert.Text = "Already Updated ";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Set Rights ')", true);
                                imgAlert.Visible = true;
                                lbl_alert.Text = "Please Set Rights ";
                                return;
                            }
                        }
                    }
                }
                if (saveflag == true)
                {
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Saved successfully";
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Allot Fees')", true);
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Please Allot Fees";
                }
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Finance Year";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Finance Year')", true);
            }
        }
        catch (Exception ex)
        {
        }
    }
    #endregion
    #region tansport fee stud auto search
    //boarding place
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetListofCountries(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select Stage_Name from stage_master where Stage_Name like '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }
    //[System.Web.Script.Services.ScriptMethod()]
    //[System.Web.Services.WebMethod]
    //public static List<string> GetListofCountries(string prefixText)
    //{
    //    string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    //    using (SqlConnection sqlconn = new SqlConnection(cs))
    //    {
    //        sqlconn.Open();
    //        SqlCommand cmd = new SqlCommand("select Stage_id,Stage_Name,Address,District from stage_master where Stage_Name like '" + prefixText + "%' ", sqlconn);
    //        cmd.Parameters.AddWithValue("@Stage_Name", prefixText);
    //        SqlDataAdapter da = new SqlDataAdapter(cmd);
    //        DataTable dt = new DataTable();
    //        da.Fill(dt);
    //        List<string> CountryNames = new List<string>();
    //        for (int i = 0; i < dt.Rows.Count; i++)
    //        {
    //            //CountryNames.Add(dt.Rows[i]["stud_name"].ToString() + "|" + dt.Rows[i]["roll_no"].ToString() + "|" + dt.Rows[i]["reg_no"].ToString() + "\n\n");
    //            CountryNames.Add(dt.Rows[i]["Stage_Name"].ToString());
    //        }
    //        return CountryNames;
    //    }
    //}
    //rollno
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        string query = "";
        WebService ws = new WebService();
        List<string> name = new List<string>();
        if (studorstaf == 0 && studclgcode != "")
        {
            if (schlSettCode != 0)
            {
                query = "select top (10)Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR'  and Roll_No like '" + prefixText + "%'  and ((isnull(Bus_RouteID,'')='' and isnull(Boarding,'')='' and isnull(VehID,'')='') or iscanceledstage='1') and college_code='" + studclgcode + "' ";
            }
            else
            {
                //velamal schol admission no 
                query = "select top (10)Roll_admit from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR'  and Roll_admit like '" + prefixText + "%'  and ((isnull(Bus_RouteID,'')='' and isnull(Boarding,'')='' and isnull(VehID,'')='') or iscanceledstage='1') and college_code='" + studclgcode + "'";
            }
        }
        else
        {
            query = "select distinct top (50) s.staff_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_code like '" + prefixText + "%' and ((isnull(Bus_RouteID,'')='' and isnull(Boarding,'')='' and isnull(VehID,'')='') or iscanceledstage='1') and s.college_code='" + studclgcode + "'";
        }
        name = ws.Getname(query);
        return name;
    }
    // stud name
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select top (10) a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+r.Roll_No,r.Roll_No from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%'";
        // studhash = ws.Getnamevalue(query);
        name = ws.Getname(query);
        return name;
    }
    #endregion
    #region SemAndYear Setting
    protected string feecatValue(string value)
    {
        string semval = "";
        string type = "";
        try
        {
            string strtype = dacces2.GetFunction("select LinkValue from New_InsSettings where college_code='" + Session["collegecode"].ToString() + "' and LinkName='Fee Yearwise' And user_code = '" + usercode + "'");
            if (strtype == "1")
                type = "Yearly";
            else if (strtype == "0")
                type = "Semester";
            else
                type = "Term";
            if (type == "Yearly")
            {
                if (value == "1" || value == "2")
                    semval = "1 Year";
                else if (value == "3" || value == "4")
                    semval = "2 Year";
                else if (value == "5" || value == "6")
                    semval = "3 Year";
                else if (value == "7" || value == "8")
                    semval = "4 Year";
            }
            else if (type == "Semester")
            {
                if (value == "1")
                    semval = "1 Semester";
                else if (value == "2")
                    semval = "2 Semester";
                else if (value == "3")
                    semval = "3 Semester";
                else if (value == "4")
                    semval = "4 Semester";
                else if (value == "5")
                    semval = "5 Semester";
                else if (value == "6")
                    semval = "6 Semester";
                else if (value == "7")
                    semval = "7 Semester";
                else if (value == "8")
                    semval = "8 Semester";
                else if (value == "9")
                    semval = "9 Semester";
            }
            else
            {
                if (value == "1")
                    semval = "Term 1";
                else if (value == "2")
                    semval = "Term 2";
                else if (value == "3")
                    semval = "Term 3";
                else if (value == "4")
                    semval = "Term 4";
            }
        }
        catch { }
        return semval;
    }
    protected void year()
    {
        try
        {
            string year = System.DateTime.Now.ToString("yyyy");
            int a1 = 0;
            for (int y = Convert.ToInt32(year); y >= 2005; y--)
            {
                a1++;
                // ddlyear.Items.Insert(a1, Convert.ToString(y));
                ddlyear.Items.Add(new ListItem(Convert.ToString(y), Convert.ToString(y)));
            }
        }
        catch { }
    }
    protected void rbsemtype_Changed(object sender, EventArgs e)
    {
        ddlmonth.Enabled = false;
        ddlyear.Enabled = false;
        feeset();
    }
    protected void rbstutype_Changed(object sender, EventArgs e)
    {
        ddlmonth.Enabled = false;
        ddlyear.Enabled = false;
        feeset();
    }
    protected void rbtermtype_Changed(object sender, EventArgs e)
    {
        ddlmonth.Enabled = false;
        ddlyear.Enabled = false;
        feeset();
    }
    protected void rbtranfer_Changed(object sender, EventArgs e)
    {
        ddlmonth.Enabled = true;
        ddlyear.Enabled = true;
        // ddlmonth.Items.Insert(0, "Month");
        //  ddlyear.Items.Insert(0, "Year");
        year();
        feeset();
    }
    #endregion
    protected void btn_alertclose_Click(object sender, EventArgs e)
    {
        imgAlert.Visible = false;
    }
    protected void btndelclose_Click(object sender, EventArgs e)
    {
        Div1.Visible = false;
    }
    #region college
    public void loadcollege()
    {
        try
        {
            string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + usercode + " and cp.college_code=cf.college_code";
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblclg.DataSource = ds;
                cblclg.DataTextField = "collname";
                cblclg.DataValueField = "college_code";
                cblclg.DataBind();
                if (cblclg.Items.Count > 0)
                {
                    for (int i = 0; i < cblclg.Items.Count; i++)
                    {
                        cblclg.Items[i].Selected = true;
                    }
                    txtclg.Text = Label16.Text + "(" + cblclg.Items.Count + ")";
                    cbclg.Checked = true;
                }
            }
        }
        catch
        { }
    }
    protected void cbclg_CheckedChanged(object sender, EventArgs e)
    {
        if (cblclg.Items.Count > 0)
        {
            string clgcode = "";
            for (int clg = 0; clg < cblclg.Items.Count; clg++)
            {
                if (cblclg.Items[clg].Selected == true)
                {
                    if (clgcode == "")
                        clgcode = cblclg.Items[clg].Value;
                    else
                        clgcode = clgcode + "," + cblclg.Items[clg].Value;
                }
            }
            //   collegecode = Session["collegecode"].ToString();
            collegecode = clgcode;
        }
        CallCheckboxChange(cbclg, cblclg, txtclg, Label16.Text, "--Select--");
        BindBatch();
        BindDegree(singleuser, group_user, collegecode, usercode);
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
    }
    protected void cblclg_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cblclg.Items.Count > 0)
        {
            string clgcode = "";
            for (int clg = 0; clg < cblclg.Items.Count; clg++)
            {
                if (cblclg.Items[clg].Selected == true)
                {
                    if (clgcode == "")
                        clgcode = cblclg.Items[clg].Value;
                    else
                        clgcode = clgcode + "," + cblclg.Items[clg].Value;
                }
            }
            //   collegecode = Session["collegecode"].ToString();
            collegecode = clgcode;
        }
        CallCheckboxListChange(cbclg, cblclg, txtclg, Label16.Text, "--Select--");
        BindBatch();
        BindDegree(singleuser, group_user, collegecode, usercode);
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
    }
    #region Common Checkbox and Checkboxlist Event
    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }
    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = "";
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }
    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = "";
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch { }
    }
    #endregion
    #endregion
    protected void SettingRights()
    {
        try
        {
            string feeSetgCode = dacces2.GetFunction("select value from Master_Settings where settings='TransportFeeAllotmentSettings'  and usercode='" + usercode + "'");
            string[] splitval = feeSetgCode.Split('-');
            if (splitval[0] == "1")
            {
                rbsemtype.Checked = true;
                rbsemtype.Enabled = true;
                rbstutype.Enabled = false;
                rbstutype.Checked = false;
                rbtranfer.Enabled = false;
                rbtranfer.Checked = false;
                rbtermtype.Enabled = false;
                rbtermtype.Checked = false;
            }
            else if (splitval[0] == "2")
            {
                rbstutype.Checked = true;
                rbstutype.Enabled = true;
                rbsemtype.Enabled = false;
                rbsemtype.Checked = false;
                rbtranfer.Enabled = false;
                rbtranfer.Checked = false;
                rbtermtype.Enabled = false;
                rbtermtype.Checked = false;
            }
            else if (splitval[0] == "3")
            {
                ddlmonth.Items.Clear();
                Hashtable ht = new Hashtable();
                rbtranfer.Checked = true;
                rbtranfer.Enabled = true;
                rbstutype.Enabled = false;
                rbstutype.Checked = false;
                rbsemtype.Enabled = false;
                rbsemtype.Checked = false;
                rbtermtype.Enabled = false;
                rbtermtype.Checked = false;
                if (splitval[1].Contains(";") == true)
                {
                    string[] year1 = splitval[1].Split(';');
                    if (year1.Length > 1)
                    {
                        ddlyear.Items.Clear();
                        // ddlyear.SelectedIndex = ddlyear.Items.IndexOf(ddlyear.Items.FindByValue(Convert.ToString(year1[1])));
                        ddlyear.Items.Add(year1[1]);
                        ddlyear.Enabled = true;
                    }
                    if (year1[0].Contains(",") == true)
                    {
                        string[] year2 = year1[0].Split(',');
                        if (year2.Length > 0)
                        {
                            for (int row = 0; row < year2.Length; row++)
                            {
                                ht.Add(row, year2[row]);
                            }
                        }
                    }
                }
                if (ht.Count > 0)
                {
                    Hashtable htmon = new Hashtable();
                    htmon = (Hashtable)ViewState["mnth"];
                    for (int cbl = 0; cbl < ht.Count; cbl++)
                    {
                        string val = Convert.ToString(ht[cbl]);
                        string mnth = Convert.ToString(htmon[val]);
                        ddlmonth.Items.Add(new ListItem(mnth, val));
                        ddlmonth.Enabled = true;
                    }
                }
            }
            else if (splitval[0] == "4")
            {
                rbstutype.Checked = false;
                rbstutype.Enabled = false;
                rbsemtype.Enabled = false;
                rbsemtype.Checked = false;
                rbtranfer.Enabled = false;
                rbtranfer.Checked = false;
                rbtermtype.Checked = true;
                rbtermtype.Enabled = true;
            }
        }
        catch { }
    }
    public void bindmonth()
    {
        htmnth.Clear();
        ddlmonth.Items.Clear();
        string type = "";
        string[] transmonth = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
        for (int i = 0; i < 12; i++)
        {
            ddlmonth.Items.Add(new System.Web.UI.WebControls.ListItem(transmonth[i], Convert.ToString(i + 1)));
            htmnth.Add(Convert.ToString(i + 1), Convert.ToString(transmonth[i]));
        }
        ViewState["mnth"] = htmnth;
    }
    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();
        lbl.Add(Label16);
        //lbl.Add(lbl_stream);
        lbl.Add(Label8);
        lbl.Add(Label9);
        // lbl.Add(lbl_sem);
        fields.Add(0);
        // fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        //fields.Add(4);
        lbl.Add(Label2);
        fields.Add(2);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
    //added by sudhagar 28.01
    private double checkSchoolSetting()
    {
        double getVal = 0;
        try
        {
            double.TryParse(Convert.ToString(dacces2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
            schlSettCode = getVal;
        }
        catch { }
        return getVal;
    }
    protected void ddlclgstud_Selected(object sender, EventArgs e)
    {
        if (ddlclgstud.Items.Count > 0)
        {
            studclgcode = Convert.ToString(ddlclgstud.SelectedValue);
        }
    }
    // last modified 28.01.2017 sudhagar
    //added by jairam 01-05-2017
    protected void FpSpread3_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            int a1 = (FpSpread3.Sheets[0].RowCount) - 2;
            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Formula = "SUM(D1:D" + a1 + ")";
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnexi_Click(object sender, EventArgs e)
    {
        pnlupdate.Visible = false;
    }
    protected void btnok_Click(object sender, EventArgs e)
    {
        try
        {
            string monthwise = "";
            FpSpread3.SaveChanges();
            FpSpread1.SaveChanges();
            string actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            int col = Convert.ToInt32(actcol);
            int colindex = col + 1;
            double GetTotalAmount = 0;
            double.TryParse(lblTotalAmount.Text, out GetTotalAmount);
            double TotalAmount = 0;
            for (int i = 0; i < FpSpread3.Sheets[0].Rows.Count; i++)
            {
                if (FpSpread3.Sheets[0].Cells[i, 3].Text.Trim() != "")
                {
                    string Amount = string.Empty;
                    Amount = Convert.ToString(FpSpread3.Sheets[0].Cells[i, 3].Text);
                    if (Amount.Trim() != "")
                    {
                        TotalAmount += Convert.ToDouble(Amount);
                    }
                    if (monthwise == "")
                    {
                        monthwise = "" + FpSpread3.Sheets[0].Cells[i, 1].Tag + ":" + FpSpread3.Sheets[0].Cells[i, 2].Text + ":" + FpSpread3.Sheets[0].Cells[i, 3].Text + "";
                    }
                    else
                    {
                        monthwise = monthwise + "," + FpSpread3.Sheets[0].Cells[i, 1].Tag + ":" + FpSpread3.Sheets[0].Cells[i, 2].Text + ":" + FpSpread3.Sheets[0].Cells[i, 3].Text + "";
                    }
                }
            }
            if (GetTotalAmount != TotalAmount)
            {
                lblErrorMsg.Text = "Monthly Allot distribution does not match";
                lblErrorMsg.Visible = true;
            }
            else
            {
                if (monthwise.Trim() != "")
                {
                    Session["MonthValue"] = monthwise.ToString();
                }
                pnlupdate.Visible = false;
                lblErrorMsg.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void monthwise()
    {
        try
        {
            #region Montwise Retrieve
            string[] prevYear = new string[13];
            string[] prevAmt = new string[13];
            #endregion
            string StgeID = string.Empty;
            StgeID = GetFunction("select Stage_id from stage_master where Stage_Name = '" + tbborplace.Text + "'");
            string type = string.Empty;
            if (rbsemtype.Checked == true)
                type = "Semester";
            if (rbstutype.Checked == true)
                type = "Yearly";
            if (rbtranfer.Checked == true)
                type = "Monthly";
            if (rbtermtype.Checked == true)
                type = "Term";
            Cost = dacces2.GetFunction("select CAST(f.cost AS INT) as Cost from FeeInfo f where f.StrtPlace = '" + StgeID + "' and f.payType = '" + type + "' and college_code='" + ddlclgstud.SelectedValue + "'");
            lblTotalAmount.Text = Cost.ToString();
            FpSpread3.Sheets[0].RowCount = 0;
            FpSpread3.Sheets[0].ColumnCount = 0;
            FpSpread3.SaveChanges();
            FpSpread3.Sheets[0].RowHeader.Visible = false;
            FpSpread3.CommandBar.Visible = false;
            FpSpread3.Sheets[0].AutoPostBack = false;
            FpSpread3.Sheets[0].RowCount = 0;
            FpSpread3.Sheets[0].ColumnCount = 4;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Column.Width = 50;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Month";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Column.Width = 100;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            ArrayList array = new ArrayList();
            for (int s = ddlyear.Items.Count; s > 0; s--)
            {
                array.Add(ddlyear.Items[s - 1]);
            }
            string[] droparray = new string[array.Count];
            for (int yea = 0; yea < array.Count; yea++)
            {
                droparray[yea] = array[yea].ToString();
            }
            FarPoint.Web.Spread.ComboBoxCellType cbYear = new FarPoint.Web.Spread.ComboBoxCellType(droparray);
            cbYear.UseValue = true;
            cbYear.ShowButton = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Year";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Column.Width = 80;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Amount";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Column.Width = 80;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FarPoint.Web.Spread.DoubleCellType intgrcell = new FarPoint.Web.Spread.DoubleCellType();
            intgrcell.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
            //  intgrcell.MaximumValue = Convert.ToInt32(100);
            intgrcell.MinimumValue = 0;
            intgrcell.ErrorMessage = "Enter valid Number";
            FpSpread3.Sheets[0].Columns[2].CellType = intgrcell;
            FpSpread3.Sheets[0].Columns[2].Font.Bold = false;
            FpSpread3.Sheets[0].Columns[2].Font.Name = "Book Antiqua";
            for (int i = 0; i < ddlmonth.Items.Count; i++)
            {
                FpSpread3.Sheets[0].Rows.Count++;
                FpSpread3.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                FpSpread3.Sheets[0].Cells[i, 1].Text = Convert.ToString(ddlmonth.Items[i].Text);
                FpSpread3.Sheets[0].Cells[i, 1].Tag = Convert.ToString(ddlmonth.Items[i].Value);
                FpSpread3.Sheets[0].Cells[i, 2].CellType = cbYear;
            }
            FpSpread3.Height = 350;
            FpSpread3.SaveChanges();
            FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].Rows.Count;
        }
        catch (Exception ex)
        {
        }
    }
    protected void lnkMultipleMonth_Clik(object sender, EventArgs e)
    {
        try
        {
            if (rbtranfer.Checked == true && tbborplace.Text != "")
            {
                lblErrorMsg.Visible = false;
                pnlupdate.Visible = true;
                monthwise();
            }
            else
            {
                pnlupdate.Visible = false;
            }
        }
        catch
        {
        }
    }
}
