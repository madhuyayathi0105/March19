using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.Drawing;
using System.Text.RegularExpressions;
using System.IO;

public partial class Default6 : System.Web.UI.Page
{

    #region init 

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
            //if (getselectedpath != null)
            //{
            //    string field1 = dsdm.GetValue(row, 0).ToString();
            //    string field2 = dsdm.GetValue(row, 1).ToString();
            //}
            HyperLink hypType = (HyperLink)c;
            hypType.Text = value.ToString();
            //hypType.NavigateUrl = "http://www.fpoint.com?s1=" + field1 + "s2=" + field2;

            hypType.NavigateUrl = getselectedpath;
            hypType.Target = "_self";
            return hypType;


        }
    }
    SqlConnection CN = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con7 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con5 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlCommand cmd = new SqlCommand();

    public void Connection()
    {
        con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
        con.Open();
    }
    DAccess2 da = new DAccess2();
    DataSet ds;
    DataSet dss;
    DataSet ds2;
    SqlDataAdapter danew;
    DAccess2 dset = new DAccess2();
    DataSet d1 = new DataSet();
    DataSet d2 = new DataSet();
    DataSet d3 = new DataSet();
    Hashtable hastab = new Hashtable();
    Hashtable ht = new Hashtable();
    static Hashtable spr_hash = new Hashtable();
    static Hashtable priority_hash = new Hashtable();
    static int inc = 0, inc1 = 0;
    static int chkflag = 0;
    Boolean cellclick = false;
    int check_addrow = 0;
    int check_FcRow = 0;
    static string path1 = "";
    static string selectedpath = "";
    string branch = "", univ = "", course = "", tvalue = "", sql = "";
    string pcourse = "";
    string pdegree = "";
    string pcol = "";
    string testno = "";
    string testdate = "";
    string testcentre = "";
    string test_detail = "", eval = "";
    string religion = "0", caste = "0";
    string blood = "0", region = "0", FatherQuali = "0", FatherIncome = "0", MotherQuali = "0", MotherIncome = "0", quota = "0";
    string comm = "0", nation = "0", mton = "0", foccu = "0", moccu = "0", statec = "0", statep = "0", mbl = "0", seattype = "0", stateg = "0";
    string medium = "0";
    string vtype = "0", vpur = "0";
    string dealer = "0", state = "0";
    string sex;
    string activity = "0", enquiry = "0", talukp = "0", talukc = "0", talukg = "0";
    string name, phn, email, amount, adres, agent, city, district;
    string refered = "", dir;
    string code = "";
    int row_mark;
    bool Cell;
    int count_mark;
    string passyear;
    string getmark_no, getmark, getsubno, getmin, getmax, result;
    string final_mark = "", mode, sem;
    Boolean Cellclick = false;
    Boolean Cellclick3 = false;
    Boolean Cellclick4 = false;
    static int priority_count = 0;
    static int cbDate = 0;
    string CollegeCode;
    static string[] ss;
    static string p = "";
    static string[] ss1;
    string ss2 = "";
    Boolean flag_true;
    FarPoint.Web.Spread.ComboBoxCellType cf = new FarPoint.Web.Spread.ComboBoxCellType();
    int rowvalue, tempvalue, cvalue;
    string caption = "", fee_code = "", fee_amt = "", header_id = "", semval = "";
    string sqlcmd = "", enqno = ""; string tcode = "";
    ArrayList keyarray = new ArrayList();
    ArrayList valuearray = new ArrayList();
    Hashtable loadhas = new Hashtable();
    DataSet dsload = new DataSet(); static int chk = 0;
    static int chk1 = 0;
    Boolean Cellclick1 = false;
    static string selected_college = "";
    DAccess2 DataAccess = new DAccess2();
    string PageLogOut = "";

    public class MyImg : FarPoint.Web.Spread.ImageCellType
    {

        //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, Boolean upperLevel)
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(70);
            //  img.Height = Unit.Percentage(70);
            return img;


        }
    }
    public class MyImg1 : FarPoint.Web.Spread.ImageCellType
    {

        //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, Boolean upperLevel)
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(70);
            img.Height = Unit.Percentage(70);
            return img;


        }
    }


    #endregion

    protected void lb2_Click(object sender, EventArgs e) //Sankar edit For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }

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
        if (!IsPostBack)
        {
            load_college();
            chk_college.Checked = true;
            chk_college_ChekedChanged(sender, e);
            FileUploadnew.FileName.GetType();
            sprdMainEnquiry.Sheets[0].AutoPostBack = true;
            sprdMainEnquiry.CommandBar.Visible = false;
            sprdMainEnquiry.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            sprdMainEnquiry.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
            sprdMainEnquiry.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            sprdMainEnquiry.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            sprdMainEnquiry.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            sprdMainEnquiry.Sheets[0].DefaultStyle.Font.Bold = false;

            FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
            style1.Font.Size = 12;
            style1.Font.Bold = true;
            style1.HorizontalAlign = HorizontalAlign.Left;
            style1.ForeColor = Color.Black;
            sprdMainEnquiry.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            sprdMainEnquiry.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            sprdMainEnquiry.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            sprdMainEnquiry.Sheets[0].AllowTableCorner = true;

            sprdMainEnquiry.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            sprdMainEnquiry.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
            sprdMainEnquiry.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
            sprdMainEnquiry.Pager.Align = HorizontalAlign.Right;
            sprdMainEnquiry.Pager.Font.Bold = true;
            sprdMainEnquiry.Pager.Font.Name = "Book Antiqua";
            sprdMainEnquiry.Pager.ForeColor = Color.DarkGreen;
            sprdMainEnquiry.Pager.BackColor = Color.Beige;
            sprdMainEnquiry.Pager.BackColor = Color.AliceBlue;
            FarPoint.Web.Spread.TextCellType tb = new FarPoint.Web.Spread.TextCellType();
            sprdMainEnquiry.Sheets[0].ColumnCount = 10;
            sprdMainEnquiry.SheetCorner.Cells[0, 0].Text = "S.No";
            sprdMainEnquiry.Sheets[0].ColumnHeader.Cells[0, 0].Text = "VehicleType";
            sprdMainEnquiry.Sheets[0].ColumnHeader.Cells[0, 1].Text = "VehicleID";
            sprdMainEnquiry.Sheets[0].Columns[1].CellType = tb;
            sprdMainEnquiry.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg.Number";
            sprdMainEnquiry.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg.Date";
            sprdMainEnquiry.Sheets[0].ColumnHeader.Cells[0, 4].Text = "RCNumber";
            sprdMainEnquiry.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Type";
            sprdMainEnquiry.Sheets[0].ColumnHeader.Cells[0, 6].Text = "NumberOfOwners";
            sprdMainEnquiry.Sheets[0].ColumnHeader.Cells[0, 7].Text = "PurchasedOn";
            sprdMainEnquiry.Sheets[0].ColumnHeader.Cells[0, 8].Text = "PlaceReg.";
            sprdMainEnquiry.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Duration";

            sprdMainEnquiry.Sheets[0].Columns[0].Locked = true;
            sprdMainEnquiry.Sheets[0].Columns[1].Locked = true;
            sprdMainEnquiry.Sheets[0].Columns[2].Locked = true;
            sprdMainEnquiry.Sheets[0].Columns[3].Locked = true;
            sprdMainEnquiry.Sheets[0].Columns[4].Locked = true;
            sprdMainEnquiry.Sheets[0].Columns[5].Locked = true;
            sprdMainEnquiry.Sheets[0].Columns[6].Locked = true;
            sprdMainEnquiry.Sheets[0].Columns[7].Locked = true;
            sprdMainEnquiry.Sheets[0].Columns[8].Locked = true;
            sprdMainEnquiry.Sheets[0].Columns[9].Locked = true;
            tbregdate.Attributes.Add("readonly", "readonly");
            tbrenewdate.Attributes.Add("readonly", "readonly");
            tbmanudate.Attributes.Add("readonly", "readonly");
            tbpuron.Attributes.Add("readonly", "readonly");
            sprdMainEnquiry.Visible = true;
            Session["FileUploadnew"] = "";
            Session["imgUpload2"] = "";
            Session["imgUpload3"] = "";
            Session["imgUpload4"] = "";
            Session["imgUpload5"] = "";
            Session["imgUpload6"] = "";
            Session["imgUpload7"] = "";
            Session["imgUpload8"] = "";
            Session["imgUpload9"] = "";
            rbnew.Checked = true;
            rbpruindu.Checked = true;
            tbnoowner.Enabled = false;
            ddldealerdetails.Enabled = false;
            loadinsu();
            loadFC();
            loadspread();
            bindVehicleType();
            bindVehicleID();
            load_detail();
            //Modified by srinath 26/12/2013
            // LoadMainEnquiry_date();
            loadengineno();
            btnMainGo_Click(sender, e);
            Accordion1.SelectedIndex = 0;
        }
    }

    public void bindVehicleType()
    {
        Connection();
        ddlvehicletypeview.Items.Clear();
        ddlvehicletypeview.Items.Insert(0, new ListItem("All", "-1"));
        string sql;
        sql = "select distinct Veh_Type from vehicle_master order by Veh_Type";
        ds = da.select_method_wo_parameter(sql, "txt");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ddlvehicletypeview.Items.Add(ds.Tables[0].Rows[i]["Veh_Type"].ToString());
            }
            ddlvehicletypeview.SelectedIndex = 0;

        }
        con.Close();
    }

    public void bindVehicleID()
    {
        Connection();
        ddltypeview.Items.Clear();
        ddltypeview.Items.Insert(0, new ListItem("All", "-1"));
        string sql;
        sql = "select * from vehicle_master order by len(veh_id), Veh_ID";
        ds = da.select_method_wo_parameter(sql, "txt");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ddltypeview.Items.Add(ds.Tables[0].Rows[i]["Veh_ID"].ToString());
            }
            ddltypeview.SelectedIndex = 0;

        }
        con.Close();
    }

    public void loadinsu()
    {
        //sprdMaininsurance.Sheets[0].AutoPostBack = true;
        sprdMaininsurance.ActiveSheetView.DefaultRowHeight = 20;
        sprdMaininsurance.ActiveSheetView.Rows.Default.Font.Name = "MS Sans Serif";
        sprdMaininsurance.ActiveSheetView.Rows.Default.Font.Size = FontUnit.Small;
        sprdMaininsurance.ActiveSheetView.Rows.Default.Font.Bold = false;
        sprdMaininsurance.ActiveSheetView.Columns.Default.Font.Bold = false;
        sprdMaininsurance.ActiveSheetView.Columns.Default.Font.Name = "MS Sans Serif";
        sprdMaininsurance.ActiveSheetView.Columns.Default.Font.Size = FontUnit.Small;
        sprdMaininsurance.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
        sprdMaininsurance.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "MS Sans Serif";
        sprdMaininsurance.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Small;
        sprdMaininsurance.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        sprdMaininsurance.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        FarPoint.Web.Spread.HyperLinkCellType hypertext = new FarPoint.Web.Spread.HyperLinkCellType();
        //FarPoint.Web.Spread.ButtonCellType btnmodify = new FarPoint.Web.Spread.ButtonCellType();
        FarPoint.Web.Spread.ButtonCellType buttotype = new FarPoint.Web.Spread.ButtonCellType();
        sprdMaininsurance.CommandBar.Visible = false;
        sprdMaininsurance.SheetCorner.DefaultStyle.Font.Bold = true;
        sprdMaininsurance.SheetCorner.DefaultStyle.Font.Name = "MS Sans Serif";
        sprdMaininsurance.SheetCorner.DefaultStyle.Font.Size = FontUnit.Small;
        //tbenqdate.Attributes.Add("readonly", "readonly");
        FarPoint.Web.Spread.LabelCellType btnedit = new FarPoint.Web.Spread.LabelCellType();
        //FarPoint.Web.Spread.TextCellType tt = new FarPoint.Web.Spread.TextCellType();
        sprdMaininsurance.Sheets[0].ColumnCount = 10;
        sprdMaininsurance.Sheets[0].RowCount = 0;
        // sprdMaininsurance.SheetCorner.Columns[0].CellType = tt;
        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        style.Font.Name = "MS Sans Serif";
        style.Font.Size = FontUnit.Small;
        style.Font.Bold = true;
        sprdMaininsurance.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        sprdMaininsurance.Sheets[0].AllowTableCorner = true;
        sprdMaininsurance.Sheets[0].SheetCorner.Cells[0, 0].BackColor = Color.AliceBlue;
        sprdMaininsurance.SheetCorner.Cells[0, 0].Text = "S.No";
        sprdMaininsurance.SheetCorner.Columns[0].HorizontalAlign = HorizontalAlign.Center;
        sprdMaininsurance.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Insurance No";
        sprdMaininsurance.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Center;
        FarPoint.Web.Spread.ButtonCellType btnmodify = new FarPoint.Web.Spread.ButtonCellType();
        buttotype.Text = "Download Certificate";
        btnmodify.Text = "AddCertificate";
        //FarPoint.Web.Spread.ButtonCellType delete = new FarPoint.Web.Spread.ButtonCellType();
        //delete.Text = "Delete";
        sprdMaininsurance.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Insurance Date";
        sprdMaininsurance.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Amt Insured";
        sprdMaininsurance.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Amt Insurance";
        sprdMaininsurance.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Ins Certificate Add";
        sprdMaininsurance.Sheets[0].Columns[4].CellType = btnmodify;
        sprdMaininsurance.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Ins Certificate Copy";
        sprdMaininsurance.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Next Insurance Date";
        sprdMaininsurance.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Provider Name";
        sprdMaininsurance.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Provider contact deatils";

        sprdMaininsurance.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Remarks";

        
       


        FarPoint.Web.Spread.RegExpCellType rgexdate2 = new FarPoint.Web.Spread.RegExpCellType();
        rgexdate2.ValidationExpression = "^\\d{2}-\\d{2}-\\d{4}$";
        rgexdate2.ErrorMessage = "Date (ex, 03-02-1989)";
        sprdMaininsurance.Sheets[0].Columns[1].CellType = rgexdate2;



        FarPoint.Web.Spread.RegExpCellType rgex3 = new FarPoint.Web.Spread.RegExpCellType();
        rgex3.ValidationExpression = "^[0-9]*$";
        rgex3.ErrorMessage = "Enter Valid Insu.Amount";
        sprdMaininsurance.Sheets[0].Columns[2].CellType = rgex3;



        FarPoint.Web.Spread.RegExpCellType rgex4 = new FarPoint.Web.Spread.RegExpCellType();
        rgex4.ValidationExpression = "^[0-9]*$";
        rgex4.ErrorMessage = "Enter Valid Insu.Amount";
        sprdMaininsurance.Sheets[0].Columns[3].CellType = rgex4;




        FarPoint.Web.Spread.RegExpCellType rgexdate = new FarPoint.Web.Spread.RegExpCellType();
        rgexdate.ValidationExpression = "^\\d{2}-\\d{2}-\\d{4}$";
        rgexdate.ErrorMessage = "Date (ex, 03-02-1989)";
        sprdMaininsurance.Sheets[0].Columns[6].CellType = rgexdate;
        //modified by prabha on feb 28 2018
        FarPoint.Web.Spread.TextCellType txttype = new FarPoint.Web.Spread.TextCellType();
        sprdMaininsurance.Sheets[0].Columns[0].CellType = txttype;//rajasekar 2-4





        sprdMaininsurance.Sheets[0].Columns[0].Width = 50;
        sprdMaininsurance.Sheets[0].Columns[1].Width = 100;
        sprdMaininsurance.Sheets[0].Columns[2].Width = 100;
        sprdMaininsurance.Sheets[0].Columns[3].Width = 80;
        sprdMaininsurance.Sheets[0].Columns[4].Width = 80;
        sprdMaininsurance.Sheets[0].Columns[5].Width = 50;
        sprdMaininsurance.Sheets[0].Columns[6].Width = 100;
        sprdMaininsurance.Sheets[0].Columns[7].Width = 100;
        sprdMaininsurance.Sheets[0].Columns[8].Width = 100;
        sprdMaininsurance.Sheets[0].Columns[9].Width = 100;
        //sprdMaininsurance.Sheets[0].Columns[10].Width = 100;

        sprdMaininsurance.Sheets[0].PageSize = 10;
        sprdMaininsurance.Sheets[0].Columns[5].CellType = buttotype;

        sprdMaininsurance.Sheets[0].Columns[5].ForeColor = Color.Black;
        sprdMaininsurance.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
        sprdMaininsurance.Pager.Mode = FarPoint.Web.Spread.PagerMode.NextPrev;
        sprdMaininsurance.Pager.Align = HorizontalAlign.Right;
        sprdMaininsurance.Pager.Font.Bold = true;

        sprdMaininsurance.Pager.ForeColor = Color.DarkGreen;
        sprdMaininsurance.Pager.BackColor = Color.Beige;
        sprdMaininsurance.Pager.BackColor = Color.AliceBlue;
        sprdMaininsurance.Pager.PageCount = 5;

        sprdMaininsurance.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
    }

    public void loadFC()
    {
        //sprdmainFC.Sheets[0].AutoPostBack = true;
        sprdmainFC.ActiveSheetView.DefaultRowHeight = 20;
        sprdmainFC.ActiveSheetView.Rows.Default.Font.Name = "MS Sans Serif";
        sprdmainFC.ActiveSheetView.Rows.Default.Font.Size = FontUnit.Small;
        sprdmainFC.ActiveSheetView.Rows.Default.Font.Bold = false;
        sprdmainFC.ActiveSheetView.Columns.Default.Font.Bold = false;
        sprdmainFC.ActiveSheetView.Columns.Default.Font.Name = "MS Sans Serif";
        sprdmainFC.ActiveSheetView.Columns.Default.Font.Size = FontUnit.Small;
        sprdmainFC.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
        sprdmainFC.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "MS Sans Serif";
        sprdmainFC.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Small;
        sprdmainFC.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        sprdmainFC.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        FarPoint.Web.Spread.HyperLinkCellType hypertext = new FarPoint.Web.Spread.HyperLinkCellType();
        // FarPoint.Web.Spread.ButtonCellType btnmodify = new FarPoint.Web.Spread.ButtonCellType();
        FarPoint.Web.Spread.ButtonCellType buttotype = new FarPoint.Web.Spread.ButtonCellType();
        sprdmainFC.CommandBar.Visible = false;
        sprdmainFC.SheetCorner.DefaultStyle.Font.Bold = true;
        sprdmainFC.SheetCorner.DefaultStyle.Font.Name = "MS Sans Serif";
        sprdmainFC.SheetCorner.DefaultStyle.Font.Size = FontUnit.Small;
        //tbenqdate.Attributes.Add("readonly", "readonly");
        FarPoint.Web.Spread.LabelCellType btnedit = new FarPoint.Web.Spread.LabelCellType();
        sprdmainFC.Sheets[0].ColumnCount = 7;
        sprdmainFC.Sheets[0].RowCount = 0;
        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        style.Font.Name = "MS Sans Serif";
        style.Font.Size = FontUnit.Small;
        style.Font.Bold = true;
        sprdmainFC.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        sprdmainFC.Sheets[0].AllowTableCorner = true;
        sprdmainFC.Sheets[0].SheetCorner.Cells[0, 0].BackColor = Color.AliceBlue;

        sprdmainFC.SheetCorner.Cells[0, 0].Text = "S.No";
        sprdmainFC.SheetCorner.Columns[0].HorizontalAlign = HorizontalAlign.Center;
        sprdmainFC.Sheets[0].ColumnHeader.Cells[0, 0].Text = "FC No";
        sprdmainFC.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Center;
        FarPoint.Web.Spread.ButtonCellType btnmodify1 = new FarPoint.Web.Spread.ButtonCellType();
        btnmodify1.Text = "AddCertificate";
        buttotype.Text = "Download Certificate";
        sprdmainFC.Sheets[0].ColumnHeader.Cells[0, 1].Text = "FC Date";
        sprdmainFC.Sheets[0].ColumnHeader.Cells[0, 2].Text = "FC Amount";
        sprdmainFC.Sheets[0].ColumnHeader.Cells[0, 3].Text = "FC Certificate Add";
        sprdmainFC.Sheets[0].Columns[3].CellType = btnmodify1;
        sprdmainFC.Sheets[0].ColumnHeader.Cells[0, 4].Text = "FC Certificate Copy";
        sprdmainFC.Sheets[0].Columns[4].CellType = buttotype;
        sprdmainFC.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Next FC Date";
       
        sprdmainFC.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Remarks";

        //FarPoint.Web.Spread.RegExpCellType rgex5 = new FarPoint.Web.Spread.RegExpCellType();
        //rgex5.ValidationExpression = "^[0-9]*$";
        //rgex5.ErrorMessage = "Enter Valid FC No";
        //sprdmainFC.Sheets[0].Columns[0].CellType = rgex5;

        FarPoint.Web.Spread.RegExpCellType rgexg = new FarPoint.Web.Spread.RegExpCellType();
        rgexg.ValidationExpression = "^[0-9]*$";
        rgexg.ErrorMessage = "Enter Valid FC Amount";
        sprdmainFC.Sheets[0].Columns[2].CellType = rgexg;

        //FarPoint.Web.Spread.RegExpCellType rgexnew = new FarPoint.Web.Spread.RegExpCellType();
        //rgexnew.ValidationExpression = "^[a-zA-Z]*$";
        //rgexnew.ErrorMessage = "Enter Valid Remarks";
        //sprdmainFC.Sheets[0].Columns[6].CellType = rgexnew;

        FarPoint.Web.Spread.RegExpCellType rgex = new FarPoint.Web.Spread.RegExpCellType();
        rgex.ValidationExpression = "^\\d{2}-\\d{2}-\\d{4}$";
        rgex.ErrorMessage = "Date (ex, 03-02-1989)";
        sprdmainFC.Sheets[0].Columns[1].CellType = rgex;

        FarPoint.Web.Spread.RegExpCellType rgexdate5 = new FarPoint.Web.Spread.RegExpCellType();
        rgexdate5.ValidationExpression = "^\\d{2}-\\d{2}-\\d{4}$";
        rgexdate5.ErrorMessage = "Date (ex, 03-02-1989)";
        sprdmainFC.Sheets[0].Columns[5].CellType = rgexdate5;
        sprdmainFC.Sheets[0].Columns[0].Width = 100;
        sprdmainFC.Sheets[0].Columns[1].Width = 100;
        sprdmainFC.Sheets[0].Columns[2].Width = 100;
        sprdmainFC.Sheets[0].Columns[3].Width = 100;
        sprdmainFC.Sheets[0].Columns[4].Width = 120;
        sprdmainFC.Sheets[0].Columns[5].Width = 100;
        sprdmainFC.Sheets[0].Columns[6].Width = 100;
        sprdmainFC.Sheets[0].PageSize = 10;
        sprdmainFC.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
        sprdmainFC.Pager.Mode = FarPoint.Web.Spread.PagerMode.NextPrev;
        sprdmainFC.Pager.Align = HorizontalAlign.Right;
        sprdmainFC.Pager.Font.Bold = true;

        sprdmainFC.Pager.ForeColor = Color.DarkGreen;
        sprdmainFC.Pager.BackColor = Color.Beige;
        sprdmainFC.Pager.BackColor = Color.AliceBlue;
        sprdmainFC.Pager.PageCount = 5;

        sprdmainFC.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
    }

    public void loadspread()
    {
        FpSpread1.ActiveSheetView.DefaultRowHeight = 20;
        FpSpread1.ActiveSheetView.Rows.Default.Font.Name = "MS Sans Serif";
        FpSpread1.ActiveSheetView.Rows.Default.Font.Size = FontUnit.Small;
        FpSpread1.ActiveSheetView.Rows.Default.Font.Bold = false;
        FpSpread1.ActiveSheetView.Columns.Default.Font.Bold = false;
        FpSpread1.ActiveSheetView.Columns.Default.Font.Name = "MS Sans Serif";
        FpSpread1.ActiveSheetView.Columns.Default.Font.Size = FontUnit.Small;
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "MS Sans Serif";
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Small;
        FpSpread1.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        FpSpread1.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        FarPoint.Web.Spread.HyperLinkCellType hypertext = new FarPoint.Web.Spread.HyperLinkCellType();
        FarPoint.Web.Spread.ButtonCellType btnmodify = new FarPoint.Web.Spread.ButtonCellType();
        FarPoint.Web.Spread.ButtonCellType buttotype = new FarPoint.Web.Spread.ButtonCellType();
        FpSpread1.CommandBar.Visible = false;
        FpSpread1.SheetCorner.DefaultStyle.Font.Bold = true;
        FpSpread1.SheetCorner.DefaultStyle.Font.Name = "MS Sans Serif";
        FpSpread1.SheetCorner.DefaultStyle.Font.Size = FontUnit.Small;
        FarPoint.Web.Spread.LabelCellType btnedit = new FarPoint.Web.Spread.LabelCellType();
        FarPoint.Web.Spread.TextCellType tt = new FarPoint.Web.Spread.TextCellType();
        FpSpread1.Sheets[0].ColumnCount = 4;
        FpSpread1.Sheets[0].RowCount = 0;
        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        style.Font.Name = "MS Sans Serif";
        style.Font.Size = FontUnit.Small;
        style.Font.Bold = true;
        FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        FpSpread1.Sheets[0].AllowTableCorner = true;
        FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].BackColor = Color.AliceBlue;
        FpSpread1.SheetCorner.Cells[0, 0].Text = "S.No";
        FpSpread1.SheetCorner.Columns[0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Vehicle Permit";
        FpSpread1.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Center;
        FarPoint.Web.Spread.ButtonCellType btnmodify1 = new FarPoint.Web.Spread.ButtonCellType();
        FarPoint.Web.Spread.ComboBoxCellType cf = new FarPoint.Web.Spread.ComboBoxCellType();
        cf.ShowButton = true;
        cf.UseValue = true;
        string[] cfp = { "", "Standard", "District" };
        FarPoint.Web.Spread.ComboBoxCellType cf2 = new FarPoint.Web.Spread.ComboBoxCellType(cfp);
        cf2.ShowButton = true;
        btnmodify1.Text = "AddCertificate";
        buttotype.Text = "Download Certificate";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Permit No";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Permit Date";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Permit Type";
        FpSpread1.Sheets[0].Columns[3].CellType = cf2;
        FarPoint.Web.Spread.RegExpCellType rgex3 = new FarPoint.Web.Spread.RegExpCellType();
        rgex3.ValidationExpression = "^\\d{2}-\\d{2}-\\d{4}$";
        rgex3.ErrorMessage = "Date (ex, 03-02-1989)";
        FpSpread1.Sheets[0].Columns[2].CellType = rgex3;
        FpSpread1.Sheets[0].Columns[0].Width = 140;
        FpSpread1.Sheets[0].Columns[1].Width = 140;
        FpSpread1.Sheets[0].Columns[2].Width = 140;
        FpSpread1.Sheets[0].Columns[3].Width = 140;
        FpSpread1.Sheets[0].PageSize = 10;
        FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
        FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.NextPrev;
        FpSpread1.Pager.Align = HorizontalAlign.Right;
        FpSpread1.Pager.Font.Bold = true;
        FpSpread1.Pager.ForeColor = Color.DarkGreen;
        FpSpread1.Pager.BackColor = Color.Beige;
        FpSpread1.Pager.BackColor = Color.AliceBlue;
        FpSpread1.Pager.PageCount = 5;
        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
    }

    public void load_detail()
    {
        ddlvehicletype.Attributes.Add("onfocus", "vehicltype()");
        ddlvehiclepur.Attributes.Add("onfocus", "vehiclepur()");
        ddldealerdetails.Attributes.Add("onfocus", "dealer()");
        ddlstaterto.Attributes.Add("onfocus", "state()");
        //tbvehiid.Attributes.Add("onfocus", "change()");
        //tbpuron.Attributes.Add("onfocus", "changepur()");
        ////ddlvehiclepur.Attributes.Add("onfocus", "changevehipur()");
        //tbtotalpuramount.Attributes.Add("onfocus", "changeamount()");
        //tbinsurance.Attributes.Add("onfocus", "changeinsu()");
        //tbtax.Attributes.Add("onfocus", "changetax()");
        //tbvehiclecast.Attributes.Add("onfocus", "changecast()");
        //tbnoowner.Attributes.Add("onfocus", "changepur1()");
        //tbrcno.Attributes.Add("onfocus", "changepu2r()");
        //tbregdate.Attributes.Add("onfocus", "changepur3()");
        //tbregno.Attributes.Add("onfocus", "changepur4()");
        ////ddlvehicletype.Attributes.Add("onfocus", "changepur5()");
        //tbplacereg.Attributes.Add("onfocus", "changepur6()");
        //tbduration.Attributes.Add("onfocus", "changeduration()");
        //tbseatcapacity.Attributes.Add("onfocus", "changecapacity()");
        //tbmaxallowed.Attributes.Add("onfocus", "changemaxallowed()");
        //tbintial.Attributes.Add("onfocus", "changeinit()");
        //tbrenewdate.Attributes.Add("onfocus", "changerenew()");
        //tbtotaltravel.Attributes.Add("onfocus", "changetotal()");
        //tbstudent.Attributes.Add("onfocus", "changestudent()");
        //tbstaff.Attributes.Add("onfocus", "changestaff()");
        //tbenginno.Attributes.Add("onfocus", "changeenginno()");
        //tbmanudate.Attributes.Add("onfocus", "changemanudate()");
        ////ddldealerdetails.Attributes.Add("onfocus", "changedealer()");
        //tbaddress1.Attributes.Add("onfocus", "changeaddress1()");
        //tbaddress2.Attributes.Add("onfocus", "changeadd2()");
        //tbcityrto.Attributes.Add("onfocus", "changecity()");
        ////ddlstaterto.Attributes.Add("onfocus", "changepstate()");
        //tbpincoderto.Attributes.Add("onfocus", "changepincode()");
        //tbrtocontact.Attributes.Add("onfocus", "changconrtact()");
        //tbcontactnumber.Attributes.Add("onfocus", "changenumber()");


        DataSet dnew1 = new DataSet();
        sqlcmd = "select distinct textcode,textval from textvaltable_new where textcriteria='vtype'";
        dnew1 = dset.select_method_wo_parameter(sqlcmd, "text");
        ddlvehicletype.Items.Clear();
        if (dnew1.Tables[0].Rows.Count > 0)
        {
            ddlvehicletype.DataSource = dnew1.Tables[0];
            ddlvehicletype.DataTextField = "textval";
            ddlvehicletype.DataValueField = "textcode";
            ddlvehicletype.DataBind();
        }
        ddlvehicletype.Items.Insert(0, "");

        DataSet dnew2 = new DataSet();
        sqlcmd = "select distinct textcode,textval from textvaltable_new where textcriteria='vpur'";
        dnew2 = dset.select_method_wo_parameter(sqlcmd, "text");
        ddlvehiclepur.Items.Clear();
        if (dnew2.Tables[0].Rows.Count > 0)
        {
            ddlvehiclepur.DataSource = dnew2.Tables[0];
            ddlvehiclepur.DataTextField = "textval";
            ddlvehiclepur.DataValueField = "textcode";
            ddlvehiclepur.DataBind();

        }
        ddlvehiclepur.Items.Insert(0, "");


        DataSet dnew3 = new DataSet();
        sqlcmd = "select distinct textcode,textval from textvaltable_new where textcriteria='deal'";
        dnew3 = dset.select_method_wo_parameter(sqlcmd, "text");
        ddldealerdetails.Items.Clear();
        if (dnew3.Tables[0].Rows.Count > 0)
        {
            ddldealerdetails.DataSource = dnew3.Tables[0];
            ddldealerdetails.DataTextField = "textval";
            ddldealerdetails.DataValueField = "textcode";
            ddldealerdetails.DataBind();

        }
        ddldealerdetails.Items.Insert(0, "");

        DataSet dnew4 = new DataSet();
        sqlcmd = "select distinct textcode,textval from textvaltable_new where textcriteria='state'";
        dnew4 = dset.select_method_wo_parameter(sqlcmd, "text");
        ddlstaterto.Items.Clear();
        if (dnew4.Tables[0].Rows.Count > 0)
        {
            ddlstaterto.DataSource = dnew4.Tables[0];
            ddlstaterto.DataTextField = "textval";
            ddlstaterto.DataValueField = "textcode";
            ddlstaterto.DataBind();

        }
        ddlstaterto.Items.Insert(0, "");



    }

    protected void tbvehiid_TextChanged(object sender, EventArgs e)
    {



        try
        {

            string streetc = tbvehiid.Text.Trim().ToString();
            //Regex reg = new Regex(@"[a-zA-Z0-9]\w+$");
            Regex reg = new Regex(@"[a-zA-Z0-9]$");//alter by rajasekar 22/05/2018
            Match mat = reg.Match(streetc.ToString());
            if (mat.Success)
            {
                {
                    if (tbvehiid.Text.Trim() != "")
                    {
                        Buttonsave.Enabled = true;
                        sqlcmd = "Select 1 from vehicle_master where Veh_ID='" + tbvehiid.Text.Trim() + "' and Veh_Type='" + ddlvehicletype.SelectedItem.Text.Trim() + "'";
                        d1 = dset.select_method_wo_parameter(sqlcmd, "n");
                        if (d1.Tables[0].Rows.Count > 0)
                        {
                            lblerrvehiid.Visible = false;
                            lblerrordisplay.Visible = false;
                            Buttonsave.Text = "Update";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Vehicle Id Already Exists')", true);

                        }
                        else
                        {
                            lblerrvehiid.Visible = false;
                            lblerrordisplay.Visible = false;
                            Buttonsave.Text = "Save";
                        }
                    }
                    else
                    {
                        Buttonsave.Enabled = true;
                        lblerrvehiid.Visible = false;
                        lblerrordisplay.Visible = false;
                    }
                    //tbvehiid.Text = "";
                    //ddlvehicletype.ClearSelection();


                }
            }
            else
            {
                lblerrvehiid.Visible = true;
                lblerrvehiid.Text = "Only characters and number allowed";
                tbvehiid.Text = "";
            }
        }
        catch
        {

        }



    }
    protected void sprdmainFC_ButtonCommand(object sender, EventArgs e)
    {
        mpedirect1.Show();
        Cellclick4 = true;//added by rajasekar 10/09/2018
    }
    //protected void tbregno_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string streetc = tbregno.Text.Trim().ToString();
    //        Regex reg = new Regex(@"[a-zA-Z0-9]\w+$");
    //        Match mat = reg.Match(streetc.ToString());
    //        if (mat.Success)
    //        {
    //            lblregnoerr.Visible = false;
    //        }
    //        else
    //        {
    //            lblregnoerr.Visible = true;
    //            lblregnoerr.Text = "Only characters and number allowed";
    //            tbregno.Text = "";
    //        }
    //    }
    //    catch
    //    {

    //    }
    //}
    protected void tbrcno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (tbrcno.Text != "")
            {
                lblerrrcno.Visible = false;
            }
            else
            {

            }
        }
        catch
        {

        }
    }
    protected void rbold_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (rbold.Checked == true)
            {
                tbnoowner.Enabled = true;
            }
            else
            {
                tbnoowner.Enabled = false;

            }
        }
        catch
        {

        }
    }
    protected void rbnew_CheckedChanged(object sender, EventArgs e)
    {
        if (rbnew.Checked == true)
        {
            tbnoowner.Enabled = false;
        }
        else
        {
            tbnoowner.Enabled = true;
        }
    }
    protected void ddlvehicletype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlvehicletype.SelectedItem.Text.ToString() != "")
            {
                lblerrordisplay.Visible = false;
            }

        }
        catch
        {

        }
    }
    protected void vehicleadd_Click(object sender, EventArgs e)
    {
        Paneladd.Visible = true;
        Paneladd.Attributes.Add("style", "width:208px; height:94px; top:258px; left:114px; position: absolute;");
        newcaption.InnerHtml = "VehicleType";
    }
    protected void vehicleremove_Click(object sender, EventArgs e)
    {
        lblerrvehicletype.Visible = false;
        if (ddlvehicletype.SelectedItem.Text != "")
        {
            hastab.Clear();

            hastab.Add("tcrit", "vtype");
            hastab.Add("tval", ddlvehicletype.SelectedItem.Text);
            hastab.Add("tcode", ddlvehicletype.SelectedItem.Value.ToString());
            d1 = dset.select_method("enquiry_delete_textcodenew", hastab, "sp");
            if (d1.Tables.Count > 0)
            {
                ddlvehicletype.Items.Clear();
                if (d1.Tables[0].Rows.Count > 0)
                {

                    ddlvehicletype.DataSource = d1;
                    ddlvehicletype.DataTextField = "Textval";
                    ddlvehicletype.DataValueField = "textcode";
                    ddlvehicletype.DataBind();

                }
                ddlvehicletype.Items.Insert(0, "");
            }
        }
        else
        {
            lblerrvehicletype.Text = "Select Vehicle then Delete ";
            lblerrvehicletype.Visible = true;
            //lblerrtrans.Text = "Select Vehicle then Delete ";
            // lblerrtrans.Visible = true;
        }
    }
    protected void vehiclepuradd_Click(object sender, EventArgs e)
    {
        Paneladd.Visible = true;
        Paneladd.Attributes.Add("style", "width:208px; height:94px; top:603px; left:120px; position: absolute;");
        newcaption.InnerHtml = "VehiclePurpose";
    }
    protected void ddlvehiclepur_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlvehiclepur.SelectedItem.Text.ToString() != "")
            {
                lblvehicleerrpur.Visible = false;
            }

        }
        catch
        {

        }
    }
    protected void vehiclepurremove_Click(object sender, EventArgs e)
    {

        if (ddlvehiclepur.SelectedItem.Text != "")
        {
            lblvehicleerrpur.Visible = false;
            hastab.Clear();

            hastab.Add("tcrit", "vpur");
            hastab.Add("tval", ddlvehiclepur.SelectedItem.Text);
            hastab.Add("tcode", ddlvehiclepur.SelectedItem.Value.ToString());
            d1 = dset.select_method("enquiry_delete_textcodenew", hastab, "sp");
            if (d1.Tables.Count > 0)
            {
                ddlvehiclepur.Items.Clear();
                if (d1.Tables[0].Rows.Count > 0)
                {

                    ddlvehiclepur.DataSource = d1;
                    ddlvehiclepur.DataTextField = "Textval";
                    ddlvehiclepur.DataValueField = "textcode";
                    ddlvehiclepur.DataBind();

                }
                ddlvehiclepur.Items.Insert(0, "");
            }
        }
        else
        {
            lblvehicleerrpur.Text = "Select Purpose then Delete ";
            lblvehicleerrpur.Visible = true;
        }
    }
    protected void tbregdate_TextChanged(object sender, EventArgs e)
    {
        lblerror.Visible = false;
        int fd2 = int.Parse((tbregdate.Text.Substring(0, 2).ToString()));
        int fyy2 = int.Parse((tbregdate.Text.Substring(6, 4).ToString()));
        int fm2 = int.Parse((tbregdate.Text.Substring(3, 2).ToString()));
        DateTime ts = Convert.ToDateTime(fm2 + "-" + fd2 + "-" + fyy2);

        if (ts > DateTime.Today)
        {
            tbregdate.Text = "";
            Labelvalidationdate.Visible = true;
            Labelvalidationdate.Text = "Date cannot be greater than today";
            return;
        }
        else
        {
            Labelvalidationdate.Visible = false;
        }

    }
    protected void tbpuron_TextChanged(object sender, EventArgs e)
    {
        lblerror.Visible = false;
        int fd2 = int.Parse((tbpuron.Text.Substring(0, 2).ToString()));
        int fyy2 = int.Parse((tbpuron.Text.Substring(6, 4).ToString()));
        int fm2 = int.Parse((tbpuron.Text.Substring(3, 2).ToString()));
        DateTime ts = Convert.ToDateTime(fm2 + "-" + fd2 + "-" + fyy2);

        if (ts > DateTime.Today)
        {
            tbpuron.Text = "";
            lblerrorpuron.Visible = true;
            lblerrorpuron.Text = "Date cannot be greater than today";
            return;
        }
        else
        {
            lblerrorpuron.Visible = false;
        }
    }
    protected void tbplacereg_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string place = tbplacereg.Text.Trim().ToString();
            Regex reg = new Regex(@"[a-zA-Z]\w+$");
            Match mat = reg.Match(place.ToString());
            if (mat.Success)
            {
                lblerrorplacereg.Visible = false;
            }
            else
            {
                lblerrorplacereg.Visible = true;
                lblerrorplacereg.Text = "Only characters allowed";
                tbplacereg.Text = "";
            }
        }
        catch
        {

        }
    }
    protected void tbpermit_TextChanged(object sender, EventArgs e)
    {

    }
    protected void tbpermitno_TextChanged(object sender, EventArgs e)
    {

    }
    protected void tbpermitdate_TextChanged(object sender, EventArgs e)
    {

    }
    protected void tbinsurancedate_TextChanged(object sender, EventArgs e)
    {

    }
    protected void tbenginno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string streetp = tbenginno.Text.Trim().ToString();
            Regex reg = new Regex(@"[a-zA-Z0-9]\w+$");
            Match mat = reg.Match(streetp.ToString());
            if (mat.Success)
            {
                enqerr.Visible = false;
            }
            else
            {
                enqerr.Visible = true;
                enqerr.Text = "Only characters and number allowed";
                tbenginno.Text = "";
            }
        }
        catch
        {

        }
    }
    protected void rbpruindu_CheckedChanged(object sender, EventArgs e)
    {
        ddldealerdetails.Enabled = false;
    }
    protected void rbdealer_CheckedChanged(object sender, EventArgs e)
    {
        ddldealerdetails.Enabled = true;
    }
    protected void btnadddealer_Click(object sender, EventArgs e)
    {
        Paneladd.Visible = true;
        Paneladd.Attributes.Add("style", "width:208px; height:94px; top:319px; left:744px; position: absolute;");
        newcaption.InnerHtml = "Dealer";
    }
    protected void ddldealerdetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddldealerdetails.SelectedItem.Text.ToString() != "")
            {
                lblerredealer.Visible = false;
            }

        }
        catch
        {

        }
    }
    protected void btnremovedealer_Click(object sender, EventArgs e)
    {
        lblerredealer.Visible = false;
        if (ddldealerdetails.SelectedItem.Text != "")
        {
            hastab.Clear();

            hastab.Add("tcrit", "deal");
            hastab.Add("tval", ddldealerdetails.SelectedItem.Text);
            hastab.Add("tcode", ddldealerdetails.SelectedItem.Value.ToString());
            d1 = dset.select_method("enquiry_delete_textcodenew", hastab, "sp");
            if (d1.Tables.Count > 0)
            {
                ddldealerdetails.Items.Clear();
                if (d1.Tables[0].Rows.Count > 0)
                {

                    ddldealerdetails.DataSource = d1;
                    ddldealerdetails.DataTextField = "Textval";
                    ddldealerdetails.DataValueField = "textcode";
                    ddldealerdetails.DataBind();

                }
                ddldealerdetails.Items.Insert(0, "");
            }
        }
        else
        {
            lblerredealer.Text = "Select Purpose then Delete ";
            lblerredealer.Visible = true;
        }
    }
    protected void tbpincoderto_TextChanged(object sender, EventArgs e)
    {

    }
    //protected void tbrtocontact_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string cityp = tbrtocontact.Text.Trim().ToString();
    //        Regex reg = new Regex(@"[a-zA-Z]\w+$");
    //        Match mat = reg.Match(cityp.ToString());
    //        if (mat.Success)
    //        {
    //            lblrtocontacterror.Visible = false;
    //        }
    //        else
    //        {
    //            lblrtocontacterror.Visible = true;
    //            lblrtocontacterror.Text = "Only characters allowed";
    //            tbrtocontact.Text = "";
    //        }
    //    }
    //    catch
    //    {

    //    }
    //}
    protected void statertoadd_Click(object sender, EventArgs e)
    {
        Paneladd.Visible = true;
        Paneladd.Attributes.Add("style", "width:208px; height:94px; top:521px; left:728px; position: absolute;");
        newcaption.InnerHtml = "State";
    }
    protected void ddlstaterto_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlstaterto.SelectedItem.Text.ToString() != "")
            {
                lblerrorstate.Visible = false;
            }

        }
        catch
        {

        }
    }
    protected void tbcityrto_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string cityp = tbcityrto.Text.Trim().ToString();
            Regex reg = new Regex(@"[a-zA-Z]\w+$");
            Match mat = reg.Match(cityp.ToString());
            if (mat.Success)
            {
                lblcityrto.Visible = false;
            }
            else
            {
                lblcityrto.Visible = true;
                lblcityrto.Text = "Only characters allowed";
                tbcityrto.Text = "";
            }
        }
        catch
        {

        }
    }
    protected void tbaddress2_TextChanged(object sender, EventArgs e)
    {
        //try
        //{
        //    string cityp = tbaddress2.Text.Trim().ToString();
        //    Regex reg = new Regex(@"[a-zA-Z]\w+$");
        //    Match mat = reg.Match(cityp.ToString());
        //    if (mat.Success)
        //    {
        //        lbladdress2error.Visible = false;
        //    }
        //    else
        //    {
        //        lbladdress2error.Visible = true;
        //        lbladdress2error.Text = "Only characters allowed";
        //        tbaddress2.Text = "";
        //    }
        //}
        //catch
        //{

        //}
    }
    protected void tbaddress1_TextChanged(object sender, EventArgs e)
    {
        //try
        //{
        //    string cityp = tbaddress1.Text.Trim().ToString();
        //    Regex reg = new Regex(@"[a-zA-Z]\w+$");
        //    Match mat = reg.Match(cityp.ToString());
        //    if (mat.Success)
        //    {
        //        lbladdress1error.Visible = false;
        //    }
        //    else
        //    {
        //        lbladdress1error.Visible = true;
        //        lbladdress1error.Text = "Only characters allowed";
        //        tbaddress1.Text = "";
        //    }
        //}
        //catch
        //{

        //}
    }
    protected void statertoremove_Click(object sender, EventArgs e)
    {
        lblerrorstate.Visible = false;
        if (ddlstaterto.SelectedItem.Text != "")
        {
            hastab.Clear();

            hastab.Add("tcrit", "state");
            hastab.Add("tval", ddlstaterto.SelectedItem.Text);
            hastab.Add("tcode", ddlstaterto.SelectedItem.Value.ToString());
            d1 = dset.select_method("enquiry_delete_textcodenew", hastab, "sp");
            if (d1.Tables.Count > 0)
            {
                ddlstaterto.Items.Clear();
                if (d1.Tables[0].Rows.Count > 0)
                {

                    ddlstaterto.DataSource = d1;
                    ddlstaterto.DataTextField = "Textval";
                    ddlstaterto.DataValueField = "textcode";
                    ddlstaterto.DataBind();

                }
                ddlstaterto.Items.Insert(0, "");
            }
        }
        else
        {
            lblerrorstate.Text = "Select State then Delete ";
            lblerrorstate.Visible = true;
        }
    }
    protected void UploadCertificate_Click(object sender, EventArgs e)
    {
        if (FileUploadnew.FileName.EndsWith(".jpg") || FileUploadnew.FileName.EndsWith(".gif") || FileUploadnew.FileName.EndsWith(".png"))
        {

            if (FileUploadnew.HasFile)
            {
                string strfilename1;
                strfilename1 = FileUploadnew.FileName;
                strfilename1 = strfilename1.Substring(strfilename1.LastIndexOf("\\") + 1);
                string s = '/'.ToString();
                Session["FileUploadnew"] = FileUploadnew.FileName;
                FileUploadnew.SaveAs(Server.MapPath("image") + "\\" + FileUploadnew.FileName);
                ImageRegCer.ImageUrl = "~/image/" + strfilename1;
                ImageRegCer.Visible = true;
                //mpeparentgardian.Show();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The file you selected is not a valid image file. Please select another file')", true);
        }

        //if (FileUploadnew.FileName.EndsWith(".jpg") || FileUploadnew.FileName.EndsWith(".gif") || FileUploadnew.FileName.EndsWith(".png"))
        //{

        //}
    }
    byte[] ReadFile(string sPath)
    {
        byte[] data = null;
        System.IO.FileInfo fInfo = new System.IO.FileInfo(sPath);
        long numBytes = fInfo.Length;
        System.IO.FileStream fStream = new System.IO.FileStream(sPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

        System.IO.BinaryReader br = new System.IO.BinaryReader(fStream);
        data = br.ReadBytes((int)numBytes);
        fStream.Dispose();
        br.Dispose();
        return data;

    }
    protected void UploadBackPhoto_Click(object sender, EventArgs e)
    {
        if (imgUpload2.FileName.EndsWith(".jpg") || imgUpload2.FileName.EndsWith(".gif") || imgUpload2.FileName.EndsWith(".png"))
        {

            if (imgUpload2.HasFile)
            {
                string strfilename1;
                strfilename1 = imgUpload2.FileName;
                strfilename1 = strfilename1.Substring(strfilename1.LastIndexOf("\\") + 1);
                string s = '/'.ToString();
                Session["imgUpload2"] = imgUpload2.FileName;
                imgUpload2.SaveAs(Server.MapPath("image") + "\\" + imgUpload2.FileName);
                ImgBackPhoto.ImageUrl = "~/image/" + strfilename1;
                ImgBackPhoto.Visible = true;
                // mpeparentgardian.Show();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The file you selected is not a valid image file. Please select another file')", true);
        }
    }
    byte[] ReadFilebackphoto(string sPath)
    {
        byte[] data = null;
        System.IO.FileInfo fInfo = new System.IO.FileInfo(sPath);
        long numBytes = fInfo.Length;
        System.IO.FileStream fStream = new System.IO.FileStream(sPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

        System.IO.BinaryReader br = new System.IO.BinaryReader(fStream);
        data = br.ReadBytes((int)numBytes);
        fStream.Dispose();
        br.Dispose();
        return data;

    }
    protected void UploadFrontPhoto_Click(object sender, EventArgs e)
    {
        if (imgUpload3.FileName.EndsWith(".jpg") || imgUpload3.FileName.EndsWith(".gif") || imgUpload3.FileName.EndsWith(".png"))
        {

            if (imgUpload3.HasFile)
            {
                string strfilename1;
                strfilename1 = imgUpload3.FileName;
                strfilename1 = strfilename1.Substring(strfilename1.LastIndexOf("\\") + 1);
                string s = '/'.ToString();
                Session["imgUpload3"] = imgUpload3.FileName;
                imgUpload3.SaveAs(Server.MapPath("image") + "\\" + imgUpload3.FileName);
                imgFrontPhoto.ImageUrl = "~/image/" + strfilename1;
                imgFrontPhoto.Visible = true;
                // mpeparentgardian.Show();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The file you selected is not a valid image file. Please select another file')", true);
        }
    }
    byte[] ReadFileFrontPhoto(string sPath)
    {
        byte[] data = null;
        System.IO.FileInfo fInfo = new System.IO.FileInfo(sPath);
        long numBytes = fInfo.Length;
        System.IO.FileStream fStream = new System.IO.FileStream(sPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

        System.IO.BinaryReader br = new System.IO.BinaryReader(fStream);
        data = br.ReadBytes((int)numBytes);
        fStream.Dispose();
        br.Dispose();
        return data;

    }
    protected void Uploadleftphoto_Click(object sender, EventArgs e)
    {
        if (imgUpload4.FileName.EndsWith(".jpg") || imgUpload4.FileName.EndsWith(".gif") || imgUpload4.FileName.EndsWith(".png"))
        {

            if (imgUpload4.HasFile)
            {
                string strfilename1;
                strfilename1 = imgUpload4.FileName;
                strfilename1 = strfilename1.Substring(strfilename1.LastIndexOf("\\") + 1);
                string s = '/'.ToString();
                Session["imgUpload4"] = imgUpload4.FileName;
                imgUpload4.SaveAs(Server.MapPath("image") + "\\" + imgUpload4.FileName);
                imgleftphoto.ImageUrl = "~/image/" + strfilename1;
                imgleftphoto.Visible = true;
                //mpeparentgardian.Show();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The file you selected is not a valid image file. Please select another file')", true);
        }
    }
    byte[] ReadFileleft(string sPath)
    {
        byte[] data = null;
        System.IO.FileInfo fInfo = new System.IO.FileInfo(sPath);
        long numBytes = fInfo.Length;
        System.IO.FileStream fStream = new System.IO.FileStream(sPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

        System.IO.BinaryReader br = new System.IO.BinaryReader(fStream);
        data = br.ReadBytes((int)numBytes);
        fStream.Dispose();
        br.Dispose();
        return data;

    }
    protected void UploadrightPhoto_Click(object sender, EventArgs e)
    {
        if (imgUpload5.FileName.EndsWith(".jpg") || imgUpload5.FileName.EndsWith(".gif") || imgUpload5.FileName.EndsWith(".png"))
        {

            if (imgUpload5.HasFile)
            {
                string strfilename1;
                strfilename1 = imgUpload5.FileName;
                strfilename1 = strfilename1.Substring(strfilename1.LastIndexOf("\\") + 1);
                string s = '/'.ToString();
                Session["imgUpload5"] = imgUpload5.FileName;
                imgUpload5.SaveAs(Server.MapPath("image") + "\\" + imgUpload5.FileName);
                imgrightphoto.ImageUrl = "~/image/" + strfilename1;
                imgrightphoto.Visible = true;
                // mpeparentgardian.Show();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The file you selected is not a valid image file. Please select another file')", true);
        }
    }
    byte[] ReadFileright(string sPath)
    {
        byte[] data = null;
        System.IO.FileInfo fInfo = new System.IO.FileInfo(sPath);
        long numBytes = fInfo.Length;
        System.IO.FileStream fStream = new System.IO.FileStream(sPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

        System.IO.BinaryReader br = new System.IO.BinaryReader(fStream);
        data = br.ReadBytes((int)numBytes);
        fStream.Dispose();
        br.Dispose();
        return data;

    }
    protected void Uploadother1Photo_Click(object sender, EventArgs e)
    {
        if (imgUpload6.FileName.EndsWith(".jpg") || imgUpload6.FileName.EndsWith(".gif") || imgUpload6.FileName.EndsWith(".png"))
        {

            if (imgUpload6.HasFile)
            {
                string strfilename1;
                strfilename1 = imgUpload6.FileName;
                strfilename1 = strfilename1.Substring(strfilename1.LastIndexOf("\\") + 1);
                string s = '/'.ToString();
                Session["imgUpload6"] = imgUpload6.FileName;
                imgUpload6.SaveAs(Server.MapPath("image") + "\\" + imgUpload6.FileName);
                imgother1photo.ImageUrl = "~/image/" + strfilename1;
                imgother1photo.Visible = true;
                // mpeparentgardian.Show();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The file you selected is not a valid image file. Please select another file')", true);
        }
    }
    byte[] ReadFileother1(string sPath)
    {
        byte[] data = null;
        System.IO.FileInfo fInfo = new System.IO.FileInfo(sPath);
        long numBytes = fInfo.Length;
        System.IO.FileStream fStream = new System.IO.FileStream(sPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

        System.IO.BinaryReader br = new System.IO.BinaryReader(fStream);
        data = br.ReadBytes((int)numBytes);
        fStream.Dispose();
        br.Dispose();
        return data;

    }
    protected void Uploadother2Photo_Click(object sender, EventArgs e)
    {
        if (imgUpload7.FileName.EndsWith(".jpg") || imgUpload7.FileName.EndsWith(".gif") || imgUpload7.FileName.EndsWith(".png"))
        {

            if (imgUpload7.HasFile)
            {
                string strfilename1;
                strfilename1 = imgUpload7.FileName;
                strfilename1 = strfilename1.Substring(strfilename1.LastIndexOf("\\") + 1);
                string s = '/'.ToString();
                Session["imgUpload7"] = imgUpload7.FileName;
                imgUpload7.SaveAs(Server.MapPath("image") + "\\" + imgUpload7.FileName);
                imgother2photo.ImageUrl = "~/image/" + strfilename1;
                imgother2photo.Visible = true;
                //mpeparentgardian.Show();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The file you selected is not a valid image file. Please select another file')", true);
        }
    }
    byte[] ReadFileother2(string sPath)
    {
        byte[] data = null;
        System.IO.FileInfo fInfo = new System.IO.FileInfo(sPath);
        long numBytes = fInfo.Length;
        System.IO.FileStream fStream = new System.IO.FileStream(sPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

        System.IO.BinaryReader br = new System.IO.BinaryReader(fStream);
        data = br.ReadBytes((int)numBytes);
        fStream.Dispose();
        br.Dispose();
        return data;

    }
    protected void Uploadother3Photo_Click(object sender, EventArgs e)
    {
        if (imgUpload8.FileName.EndsWith(".jpg") || imgUpload8.FileName.EndsWith(".gif") || imgUpload8.FileName.EndsWith(".png"))
        {

            if (imgUpload8.HasFile)
            {
                string strfilename1;
                strfilename1 = imgUpload8.FileName;
                strfilename1 = strfilename1.Substring(strfilename1.LastIndexOf("\\") + 1);
                string s = '/'.ToString();
                Session["imgUpload8"] = imgUpload8.FileName;
                imgUpload8.SaveAs(Server.MapPath("image") + "\\" + imgUpload8.FileName);
                imgother3photo.ImageUrl = "~/image/" + strfilename1;
                imgother3photo.Visible = true;
                //mpeparentgardian.Show();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The file you selected is not a valid image file. Please select another file')", true);
        }
    }
    byte[] ReadFileother3(string sPath)
    {
        byte[] data = null;
        System.IO.FileInfo fInfo = new System.IO.FileInfo(sPath);
        long numBytes = fInfo.Length;
        System.IO.FileStream fStream = new System.IO.FileStream(sPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

        System.IO.BinaryReader br = new System.IO.BinaryReader(fStream);
        data = br.ReadBytes((int)numBytes);
        fStream.Dispose();
        br.Dispose();
        return data;

    }
    protected void Uploadother4Photo_Click(object sender, EventArgs e)
    {
        if (imgUpload9.FileName.EndsWith(".jpg") || imgUpload9.FileName.EndsWith(".gif") || imgUpload9.FileName.EndsWith(".png"))
        {

            if (imgUpload9.HasFile)
            {
                string strfilename1;
                strfilename1 = imgUpload9.FileName;
                strfilename1 = strfilename1.Substring(strfilename1.LastIndexOf("\\") + 1);
                string s = '/'.ToString();
                Session["imgUpload9"] = imgUpload9.FileName;
                imgUpload9.SaveAs(Server.MapPath("image") + "\\" + imgUpload9.FileName);
                imgother4photo.ImageUrl = "~/image/" + strfilename1;
                imgother4photo.Visible = true;
                //mpeparentgardian.Show();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The file you selected is not a valid image file. Please select another file')", true);
        }
    }
    byte[] ReadFileother4(string sPath)
    {
        byte[] data = null;
        System.IO.FileInfo fInfo = new System.IO.FileInfo(sPath);
        long numBytes = fInfo.Length;
        System.IO.FileStream fStream = new System.IO.FileStream(sPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

        System.IO.BinaryReader br = new System.IO.BinaryReader(fStream);
        data = br.ReadBytes((int)numBytes);
        fStream.Dispose();
        br.Dispose();
        return data;

    }
    protected void btnvehiclephoto_Click(object sender, EventArgs e)
    {
        //mpevehiclephoto.Show();
    }
    protected void sprdMaininsurance_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        mpedirect.Show();
        Cellclick3 = true;

    }
    protected void addrowfee(object sender, EventArgs e)
    {
        try
        {

            check_addrow++;
            sprdMaininsurance.SaveChanges();
            //if (ddlfees.Text.Trim() != "")
            sprdMaininsurance.Sheets[0].RowCount = sprdMaininsurance.Sheets[0].RowCount + 1;
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();//rajasekar 22march2018
            sprdMaininsurance.Sheets[0].Cells[sprdMaininsurance.Sheets[0].RowCount - 1, 0].CellType = txt;//rajasekar 22march2018
        }
        catch
        {

        }

    }
    protected void addremoveinsurance(object sender, EventArgs e)
    {
        try
        {
            sprdMaininsurance.SaveChanges();
            string ar = sprdMaininsurance.ActiveSheetView.ActiveRow.ToString();
            string ac = sprdMaininsurance.ActiveSheetView.ActiveColumn.ToString();
            string Insurance_No = sprdMaininsurance.Sheets[0].Cells[Convert.ToInt32(ar), 0].Text;
            string Insurance_Date = sprdMaininsurance.Sheets[0].Cells[Convert.ToInt32(ar), 1].Text;
            string Insurance_Amt = sprdMaininsurance.Sheets[0].Cells[Convert.ToInt32(ar), 2].Text;
            int actrow = Convert.ToInt32(ar);
            int actcol = Convert.ToInt32(ac);
            string updateyes = string.Empty;
            updateyes = "select * from Vehicle_Insurance where Insu_No = '" + Insurance_No + "' and Insurance_Date = '" + Insurance_Date + "' and Amt_Insured = '" + Insurance_Amt + "'";
            SqlCommand cdd = new SqlCommand(updateyes, con);
            SqlDataAdapter cda = new SqlDataAdapter(cdd);
            DataSet cds = new DataSet();
            cda.Fill(cds);
            if (cds.Tables[0].Rows.Count > 0)
            {
                con.Open();
                string deletequery = string.Empty;
                deletequery = "delete from Vehicle_Insurance where Insu_No = '" + Insurance_No + "' and Insurance_Date = '" + Insurance_Date + "' and Amt_Insured = '" + Insurance_Amt + "'";
                SqlCommand cmdsql = new SqlCommand(deletequery, con);
                cmdsql.ExecuteNonQuery();
                con.Close();
            }
            else
            {

                if (actcol >= 0)
                {
                    sprdMaininsurance.Sheets[0].RemoveRows(actrow, 1);
                }

            }
        }
        catch
        {

        }




    }
    protected void addremoveFC(object sender, EventArgs e)
    {
        try
        {
            sprdmainFC.SaveChanges();
            string ar = sprdmainFC.ActiveSheetView.ActiveRow.ToString();
            string ac = sprdmainFC.ActiveSheetView.ActiveColumn.ToString();
            string FC_No = sprdmainFC.Sheets[0].Cells[Convert.ToInt32(ar), 0].Text;
            string FC_Date = sprdmainFC.Sheets[0].Cells[Convert.ToInt32(ar), 1].Text;
            string FC_Amt = sprdmainFC.Sheets[0].Cells[Convert.ToInt32(ar), 2].Text;
            int actrow = Convert.ToInt32(ar);
            int actcol = Convert.ToInt32(ac);
            string updateyes = string.Empty;
            updateyes = "select * from Vehicle_Insurance where FC_No = '" + FC_No + "' and FC_Date = '" + FC_Date + "' and FC_Amount = '" + FC_Amt + "'";
            SqlCommand cdd = new SqlCommand(updateyes, con);
            SqlDataAdapter cda = new SqlDataAdapter(cdd);
            DataSet cds = new DataSet();
            cda.Fill(cds);
            if (cds.Tables[0].Rows.Count > 0)
            {
                con.Open();
                string deletequery = string.Empty;
                deletequery = "delete from Vehicle_Insurance where FC_No = '" + FC_No + "' and FC_Date = '" + FC_Date + "'and FC_Amount = '" + FC_Amt + "'";
                SqlCommand cmdsql = new SqlCommand(deletequery, con);
                cmdsql.ExecuteNonQuery();
                con.Close();
                sprdmainFC.Sheets[0].RemoveRows(actrow, 1);
            }
            else
            {

                if (actcol >= 0)
                {
                    sprdmainFC.Sheets[0].RemoveRows(actrow, 1);
                }

            }
        }
        catch
        {

        }
    }
    protected void addrowFC(object sender, EventArgs e)
    {
        check_FcRow++;
        sprdmainFC.SaveChanges();
        //if (ddlfees.Text.Trim() != "")
        sprdmainFC.Sheets[0].RowCount = sprdmainFC.Sheets[0].RowCount + 1;

    }
    protected void addrowPermit(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges();
        //if (ddlfees.Text.Trim() != "")
        FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
    }
    protected void addremovePermit(object sender, EventArgs e)
    {
        //FpSpread1.SaveChanges();
        ////if (ddlfees.Text.Trim() != "")
        //FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount - 1;
        try
        {
            FpSpread1.SaveChanges();
            string ar = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            string ac = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
            string Permit = FpSpread1.Sheets[0].Cells[Convert.ToInt32(ar), 0].Text;
            string PermitNo = FpSpread1.Sheets[0].Cells[Convert.ToInt32(ar), 1].Text;
            int actrow = Convert.ToInt32(ar);
            int actcol = Convert.ToInt32(ac);
            string updateyes = string.Empty;
            updateyes = "select * from Vehicle_Insurance where Permit = '" + Permit + "' and Permit_No = '" + PermitNo + "'";
            SqlCommand cdd = new SqlCommand(updateyes, con);
            SqlDataAdapter cda = new SqlDataAdapter(cdd);
            DataSet cds = new DataSet();
            cda.Fill(cds);
            if (cds.Tables[0].Rows.Count > 0)
            {
                con.Open();
                string deletequery = string.Empty;
                deletequery = "delete from Vehicle_Insurance where Permit = '" + Permit + "' and Permit_No = '" + PermitNo + "'";
                SqlCommand cmdsql = new SqlCommand(deletequery, con);
                cmdsql.ExecuteNonQuery();
                con.Close();
                FpSpread1.Sheets[0].RemoveRows(actrow, 1);
            }
            else
            {

                if (actcol >= 0)
                {
                    FpSpread1.Sheets[0].RemoveRows(actrow, 1);
                }

            }
        }
        catch
        {

        }


    }
    protected void FpSpread1_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

    }
    protected void FpSpread1prerender(object sender, EventArgs e)
    {



    }
    protected void FpSpread1_click(object sender, EventArgs e)
    {

    }
    protected void Buttonnew_Click(object sender, EventArgs e)
    {
        clear();
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
    protected void Buttonsave_Click(object sender, EventArgs e)
    {
        int fd = 0;
        int fyy = 0;
        int fm = 0;
        string dt = "", dt1 = "";


        if (ddlvehicletype.SelectedItem.Text == "")
        {
            lblerrordisplay.Visible = true;
            lblerrordisplay.Text = "Enter VehicleType";
            return;
        }
        else
        {
            hastab.Add("Veh_Type", ddlvehicletype.SelectedItem.Text.ToString());
        }

        if (tbvehiid.Text.Trim() == "")
        {
            lblerrordisplay.Visible = true;
            lblerrordisplay.Text = "Enter Vehicle Id";
            return;
        }
        else
        {
            hastab.Add("Veh_ID", tbvehiid.Text.Trim());
            lblerrordisplay.Visible = false;
        }
        if (tbregno.Text.Trim() == "")
        {
            //sankar editing
            lblerrordisplay.Visible = true;
            lblerrordisplay.Text = "Enter Registration No";
            return;
        }
        else
        {
            hastab.Add("Reg_No", tbregno.Text.Trim());
            lblerrordisplay.Visible = false;
        }
        if (tbregdate.Text.Trim() == "")
        {
            lblerrordisplay.Visible = true;
            lblerrordisplay.Text = "Enter Registration Date";
            return;
        }
        else
        {
            if (tbregdate.Text.Length == 10)
            {
                fd = int.Parse((tbregdate.Text.Substring(0, 2).ToString()));
                fyy = int.Parse((tbregdate.Text.Substring(6, 4).ToString()));
                fm = int.Parse((tbregdate.Text.Substring(3, 2).ToString()));
                //DateTime dtnew = Convert.ToDateTime(fm + "-" + fd + "-" + fyy);
                //DateTime dt = Convert.ToDateTime(fm + "-" + fd + "-" + fyy);
                dt1 = fyy + "-" + fm + "-" + fd;
                //dt1 = fm + "-" + fd + "-" + fyy;
                hastab.Add("Reg_Date", dt1);
                lblerrordisplay.Visible = false;
            }
        }
        if (tbrcno.Text.Trim() == "")
        {
            lblerrordisplay.Visible = true;
            lblerrordisplay.Text = "Enter RC No";
            return;
        }
        else
        {
            hastab.Add("RC_No", tbrcno.Text.Trim());
            lblerrordisplay.Visible = false;
        }
        if (tbtotaltravel.Text.Trim() == "")
        {
            lblerrordisplay.Visible = true;
            lblerrordisplay.Text = "Enter Total Numbers Of Travellers";
            return;
        }
        else
        {
            hastab.Add("nofTravrs", tbtotaltravel.Text.Trim());
            lblerrordisplay.Visible = false;
        }

        if (tbstudent.Text.Trim() == "")
        {
            lblerrordisplay.Visible = true;
            lblerrordisplay.Text = "Enter Total Numbers Of Student";
            return;
        }
        else
        {
            hastab.Add("nofstudents", tbstudent.Text.Trim());
            lblerrordisplay.Visible = false;
        }
        if (tbstaff.Text.Trim() == "")
        {
            lblerrordisplay.Visible = true;
            lblerrordisplay.Text = "Enter Total Numbers Of Staff";
            return;
        }
        else
        {
            hastab.Add("nofStaffs", tbstaff.Text.Trim());
            lblerrordisplay.Visible = false;
        }

        if (txtkm.Text.Trim() == "")
        {
            lblerrordisplay.Visible = true;
            lblerrordisplay.Text = "Enter Total Mileage";
            return;
        }
        else
        {

            hastab.Add("Mileage", txtkm.Text.Trim());
            lblerrordisplay.Visible = false;
        }
        saveapplication();
    }
    public void saveapplication()
    {
        sprdMaininsurance.SaveChanges(); 
        lbladdview.Text = "Add";
        int fd = 0;

        int fyy = 0;
        int fm = 0;
        string dt = "", dt1 = "", dt2 = "";

        if (rbnew.Checked == true)
            hastab.Add("Type", "0");
        else
            hastab.Add("Type", "1");

        if (tbnoowner.Text.Trim() != "")
            hastab.Add("Numberofowner", tbnoowner.Text.Trim());
        else
            hastab.Add("Numberofowner", "");
        if (tbvehiclecast.Text.Trim() != "")
            hastab.Add("VehicleCast", tbvehiclecast.Text.Trim());
        else
            hastab.Add("VehicleCast", "");
        if (tbtax.Text.Trim() != "")
            hastab.Add("Tax", tbtax.Text.Trim());
        else
            hastab.Add("Tax", "");
        if (tbinsurance.Text.Trim() != "")
            hastab.Add("Vehicle_Ins", tbinsurance.Text.Trim());
        else
            hastab.Add("Vehicle_Ins", "");
        if (tbtotalpuramount.Text.Trim() != "")
            hastab.Add("TotalPur_Amount", tbtotalpuramount.Text.Trim());
        else
            hastab.Add("TotalPur_Amount", "");


        if (ddlvehiclepur.SelectedIndex != 0)
        {
            hastab.Add("Purpose_Vehicle", ddlvehiclepur.SelectedValue.ToString());
        }
        else
        {
            hastab.Add("Purpose_Vehicle", "0");
        }

        if (tbpuron.Text.Trim() != "")
        {
            //hastab.Add("Purchased_On", tbpuron.Text.Trim());
            if (tbpuron.Text.Length == 10)
            {
                fd = int.Parse((tbpuron.Text.Substring(0, 2).ToString()));
                fyy = int.Parse((tbpuron.Text.Substring(6, 4).ToString()));
                fm = int.Parse((tbpuron.Text.Substring(3, 2).ToString()));
                dt2 = fyy + "-" + fm + "-" + fd;
                hastab.Add("Purchased_On", dt2);
            }
        }
        else
        {
            hastab.Add("Purchased_On", "");
        }
        if (tbplacereg.Text.Trim() != "")
            hastab.Add("Place_Reg", tbplacereg.Text.Trim());
        else
            hastab.Add("Place_Reg", "");

        if (tbduration.Text.Trim() != "")
            hastab.Add("Duration", tbduration.Text.Trim());
        else
            hastab.Add("Duration", "");

        if (tbseatcapacity.Text.Trim() != "")
            hastab.Add("TotalNo_Seat", tbseatcapacity.Text.Trim());
        else
            hastab.Add("TotalNo_Seat", "");

        if (tbmaxallowed.Text.Trim() != "")
            hastab.Add("Extra_No", tbmaxallowed.Text.Trim());
        else
            hastab.Add("Extra_No", "");

        if (tbintial.Text.Trim() != "")
            hastab.Add("Intial_Km", tbintial.Text.Trim());
        else
            hastab.Add("Intial_Km", "");

        if (tbrenewdate.Text.Trim() != "")
        {
            if (tbrenewdate.Text.Length == 10)
            {
                fd = int.Parse((tbrenewdate.Text.Substring(0, 2).ToString()));
                fyy = int.Parse((tbrenewdate.Text.Substring(6, 4).ToString()));
                fm = int.Parse((tbrenewdate.Text.Substring(3, 2).ToString()));
                dt1 = fyy + "-" + fm + "-" + fd;
                hastab.Add("Renew_Date", dt1);
            }
        }
        else
        {
            hastab.Add("Renew_Date", "");
        }

        if (tbenginno.Text.Trim() != "")
            hastab.Add("Engine_No", tbenginno.Text.Trim());
        else
            hastab.Add("Engine_No", "");

        if (txt_cheese.Text.Trim() != "")
            hastab.Add("Cheese_No", txt_cheese.Text.Trim());
        else
            hastab.Add("Cheese_No", "");


        if (tbmanudate.Text.Trim() != "")
        {
            if (tbmanudate.Text.Length == 10)
            {
                fd = int.Parse((tbmanudate.Text.Substring(0, 2).ToString()));
                fyy = int.Parse((tbmanudate.Text.Substring(6, 4).ToString()));
                fm = int.Parse((tbmanudate.Text.Substring(3, 2).ToString()));
                dt1 = fyy + "-" + fm + "-" + fd;
                hastab.Add("Manufactura_date", dt1);
            }
        }
        else
        {
            hastab.Add("Manufactura_date", "");
        }

        if (rbpruindu.Checked == true)
            hastab.Add("Purchased_From", "0");
        else
            hastab.Add("Purchased_From", "1");

        if (ddldealerdetails.SelectedIndex != 0)
        {
            hastab.Add("Dealer_Details", ddldealerdetails.SelectedValue.ToString());
        }
        else
        {
            hastab.Add("Dealer_Details", "0");
        }



        if (tbaddress1.Text.Trim() != "")
            hastab.Add("RTOAdd1", tbaddress1.Text.Trim());
        else
            hastab.Add("RTOAdd1", "");

        if (tbaddress2.Text.Trim() != "")
            hastab.Add("RTOAdd2", tbaddress2.Text.Trim());
        else
            hastab.Add("RTOAdd2", "");

        if (tbcityrto.Text.Trim() != "")
            hastab.Add("RTOCity", tbcityrto.Text.Trim());
        else
            hastab.Add("RTOCity", "");

        if (ddlstaterto.SelectedIndex != 0)
        {
            hastab.Add("RTOState", ddlstaterto.SelectedValue.ToString());
        }
        else
        {
            hastab.Add("RTOState", "0");
        }



        if (tbpincoderto.Text.Trim() != "")
            hastab.Add("RTOPin", tbpincoderto.Text.Trim());
        else
            hastab.Add("RTOPin", "");

        if (tbrtocontact.Text.Trim() != "")
            hastab.Add("RTOContPer", tbrtocontact.Text.Trim());
        else
            hastab.Add("RTOContPer", "");


        if (tbcontactnumber.Text.Trim() != "")
            hastab.Add("RTOContPhno", tbcontactnumber.Text.Trim());
        else
            hastab.Add("RTOContPhno", "");



        if (Session["FileUploadnew"] != null && Session["FileUploadnew"] != "")
        {


            byte[] imageData = ReadFile(Server.MapPath("image") + "\\" + Session["FileUploadnew"].ToString());

            string exqry = "select * from Vehicle_Insurance where Veh_ID=@Veh_ID";
            CN.Open();
            SqlCommand cdd = new SqlCommand(exqry, CN);
            cdd.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
            cdd.Parameters.AddWithValue("@ImageData", (object)imageData);
            SqlDataAdapter cda = new SqlDataAdapter(cdd);
            DataSet cds = new DataSet();
            cda.Fill(cds);
            if (cds.Tables[0].Rows.Count > 0)
            {
                string qry1 = "update Vehicle_Insurance set Reg_Photo=@ImageData where Veh_ID=@Veh_ID";
                SqlCommand SqlCom1 = new SqlCommand(qry1, CN);
                SqlCom1.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
                SqlCom1.Parameters.AddWithValue("@ImageData", (object)imageData);

                SqlCom1.ExecuteNonQuery();
            }
            else
            {
                string qry = "INSERT INTO Vehicle_Insurance(Veh_ID,Reg_Photo) VALUES(@Veh_ID, @ImageData) SELECT @@IDENTITY";
                SqlCommand SqlCom = new SqlCommand(qry, CN);
                SqlCom.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
                SqlCom.Parameters.AddWithValue("@ImageData", (object)imageData);

                SqlCom.ExecuteNonQuery();
                CN.Close();
            }





        }



        if (Session["imgUpload2"] != null && Session["imgUpload2"] != "")
        {
            string a = Session["imgUpload2"].ToString();

            byte[] imageData = ReadFile(Server.MapPath("image") + "\\" + Session["imgUpload2"].ToString());

            string exqry = "select * from Vehicle_Insurance where Veh_ID=@Veh_ID";
            CN.Close();
            CN.Open();
            SqlCommand cdd = new SqlCommand(exqry, CN);
            cdd.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
            cdd.Parameters.AddWithValue("@ImageData", (object)imageData);
            SqlDataAdapter cda = new SqlDataAdapter(cdd);
            DataSet cds = new DataSet();
            cda.Fill(cds);
            if (cds.Tables[0].Rows.Count > 0)
            {
                string qry1 = "update Vehicle_Insurance set v_Back=@ImageData where Veh_ID=@Veh_ID";
                SqlCommand SqlCom1 = new SqlCommand(qry1, CN);
                SqlCom1.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
                SqlCom1.Parameters.AddWithValue("@ImageData", (object)imageData);

                SqlCom1.ExecuteNonQuery();
            }
            else
            {
                string qry = "INSERT INTO Vehicle_Insurance(Veh_ID,v_Back) VALUES(@Veh_ID, @ImageData) SELECT @@IDENTITY";
                SqlCommand SqlCom = new SqlCommand(qry, CN);
                SqlCom.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
                SqlCom.Parameters.AddWithValue("@ImageData", (object)imageData);

                SqlCom.ExecuteNonQuery();
                CN.Close();
            }
        }










        if (Session["imgUpload3"] != null && Session["imgUpload3"] != "")
        {


            byte[] imageData = ReadFile(Server.MapPath("image") + "\\" + Session["imgUpload3"].ToString());

            string exqry = "select * from Vehicle_Insurance where Veh_ID=@Veh_ID";
            CN.Close();
            CN.Open();
            SqlCommand cdd = new SqlCommand(exqry, CN);
            cdd.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
            cdd.Parameters.AddWithValue("@ImageData", (object)imageData);
            SqlDataAdapter cda = new SqlDataAdapter(cdd);
            DataSet cds = new DataSet();
            cda.Fill(cds);
            if (cds.Tables[0].Rows.Count > 0)
            {
                string qry1 = "update Vehicle_Insurance set v_Front=@ImageData where Veh_ID=@Veh_ID";
                SqlCommand SqlCom1 = new SqlCommand(qry1, CN);
                SqlCom1.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
                SqlCom1.Parameters.AddWithValue("@ImageData", (object)imageData);

                SqlCom1.ExecuteNonQuery();
            }
            else
            {
                string qry = "INSERT INTO Vehicle_Insurance(Veh_ID,v_Front) VALUES(@Veh_ID, @ImageData) SELECT @@IDENTITY";
                SqlCommand SqlCom = new SqlCommand(qry, CN);
                SqlCom.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
                SqlCom.Parameters.AddWithValue("@ImageData", (object)imageData);

                SqlCom.ExecuteNonQuery();
                CN.Close();
            }
        }



        if (Session["imgUpload4"] != null && Session["imgUpload4"] != "")
        {


            byte[] imageData = ReadFile(Server.MapPath("image") + "\\" + Session["imgUpload4"].ToString());

            string exqry = "select * from Vehicle_Insurance where Veh_ID=@Veh_ID";
            CN.Close();
            CN.Open();
            SqlCommand cdd = new SqlCommand(exqry, CN);
            cdd.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
            cdd.Parameters.AddWithValue("@ImageData", (object)imageData);
            SqlDataAdapter cda = new SqlDataAdapter(cdd);
            DataSet cds = new DataSet();
            cda.Fill(cds);
            if (cds.Tables[0].Rows.Count > 0)
            {
                string qry1 = "update Vehicle_Insurance set v_left=@ImageData where Veh_ID=@Veh_ID";
                SqlCommand SqlCom1 = new SqlCommand(qry1, CN);
                SqlCom1.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
                SqlCom1.Parameters.AddWithValue("@ImageData", (object)imageData);

                SqlCom1.ExecuteNonQuery();
            }
            else
            {
                string qry = "INSERT INTO Vehicle_Insurance(Veh_ID,v_left) VALUES(@Veh_ID, @ImageData) SELECT @@IDENTITY";
                SqlCommand SqlCom = new SqlCommand(qry, CN);
                SqlCom.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
                SqlCom.Parameters.AddWithValue("@ImageData", (object)imageData);

                SqlCom.ExecuteNonQuery();
                CN.Close();
            }
        }


        if (Session["imgUpload5"] != null && Session["imgUpload5"] != "")
        {


            byte[] imageData = ReadFile(Server.MapPath("image") + "\\" + Session["imgUpload5"].ToString());

            string exqry = "select * from Vehicle_Insurance where Veh_ID=@Veh_ID";
            CN.Close();
            CN.Open();
            SqlCommand cdd = new SqlCommand(exqry, CN);
            cdd.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
            cdd.Parameters.AddWithValue("@ImageData", (object)imageData);
            SqlDataAdapter cda = new SqlDataAdapter(cdd);
            DataSet cds = new DataSet();
            cda.Fill(cds);
            if (cds.Tables[0].Rows.Count > 0)
            {
                string qry1 = "update Vehicle_Insurance set v_right=@ImageData where Veh_ID=@Veh_ID";
                SqlCommand SqlCom1 = new SqlCommand(qry1, CN);
                SqlCom1.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
                SqlCom1.Parameters.AddWithValue("@ImageData", (object)imageData);

                SqlCom1.ExecuteNonQuery();
            }
            else
            {
                string qry = "INSERT INTO Vehicle_Insurance(Veh_ID,v_right) VALUES(@Veh_ID, @ImageData) SELECT @@IDENTITY";
                SqlCommand SqlCom = new SqlCommand(qry, CN);
                SqlCom.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
                SqlCom.Parameters.AddWithValue("@ImageData", (object)imageData);

                SqlCom.ExecuteNonQuery();
                CN.Close();
            }
        }


        if (Session["imgUpload6"] != null && Session["imgUpload6"] != "")
        {


            byte[] imageData = ReadFile(Server.MapPath("image") + "\\" + Session["imgUpload6"].ToString());

            string exqry = "select * from Vehicle_Insurance where Veh_ID=@Veh_ID";
            CN.Close();
            CN.Open();
            SqlCommand cdd = new SqlCommand(exqry, CN);
            cdd.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
            cdd.Parameters.AddWithValue("@ImageData", (object)imageData);
            SqlDataAdapter cda = new SqlDataAdapter(cdd);
            DataSet cds = new DataSet();
            cda.Fill(cds);
            if (cds.Tables[0].Rows.Count > 0)
            {
                string qry1 = "update Vehicle_Insurance set v_other1=@ImageData where Veh_ID=@Veh_ID";
                SqlCommand SqlCom1 = new SqlCommand(qry1, CN);
                SqlCom1.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
                SqlCom1.Parameters.AddWithValue("@ImageData", (object)imageData);

                SqlCom1.ExecuteNonQuery();
            }
            else
            {
                string qry = "INSERT INTO Vehicle_Insurance(Veh_ID,v_other1) VALUES(@Veh_ID, @ImageData) SELECT @@IDENTITY";
                SqlCommand SqlCom = new SqlCommand(qry, CN);
                SqlCom.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
                SqlCom.Parameters.AddWithValue("@ImageData", (object)imageData);

                SqlCom.ExecuteNonQuery();
                CN.Close();
            }
        }

        if (Session["imgUpload7"] != null && Session["imgUpload7"] != "")
        {


            byte[] imageData = ReadFile(Server.MapPath("image") + "\\" + Session["imgUpload7"].ToString());

            string exqry = "select * from Vehicle_Insurance where Veh_ID=@Veh_ID";
            CN.Close();
            CN.Open();
            SqlCommand cdd = new SqlCommand(exqry, CN);
            cdd.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
            cdd.Parameters.AddWithValue("@ImageData", (object)imageData);
            SqlDataAdapter cda = new SqlDataAdapter(cdd);
            DataSet cds = new DataSet();
            cda.Fill(cds);
            if (cds.Tables[0].Rows.Count > 0)
            {
                string qry1 = "update Vehicle_Insurance set v_other2=@ImageData where Veh_ID=@Veh_ID";
                SqlCommand SqlCom1 = new SqlCommand(qry1, CN);
                SqlCom1.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
                SqlCom1.Parameters.AddWithValue("@ImageData", (object)imageData);

                SqlCom1.ExecuteNonQuery();
            }
            else
            {
                string qry = "INSERT INTO Vehicle_Insurance(Veh_ID,v_other2) VALUES(@Veh_ID, @ImageData) SELECT @@IDENTITY";
                SqlCommand SqlCom = new SqlCommand(qry, CN);
                SqlCom.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
                SqlCom.Parameters.AddWithValue("@ImageData", (object)imageData);

                SqlCom.ExecuteNonQuery();
                CN.Close();
            }
        }


        if (Session["imgUpload8"] != null && Session["imgUpload8"] != "")
        {


            byte[] imageData = ReadFile(Server.MapPath("image") + "\\" + Session["imgUpload8"].ToString());

            string exqry = "select * from Vehicle_Insurance where Veh_ID=@Veh_ID";
            CN.Close();
            CN.Open();
            SqlCommand cdd = new SqlCommand(exqry, CN);
            cdd.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
            cdd.Parameters.AddWithValue("@ImageData", (object)imageData);
            SqlDataAdapter cda = new SqlDataAdapter(cdd);
            DataSet cds = new DataSet();
            cda.Fill(cds);
            if (cds.Tables[0].Rows.Count > 0)
            {
                string qry1 = "update Vehicle_Insurance set v_other3=@ImageData where Veh_ID=@Veh_ID";
                SqlCommand SqlCom1 = new SqlCommand(qry1, CN);
                SqlCom1.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
                SqlCom1.Parameters.AddWithValue("@ImageData", (object)imageData);

                SqlCom1.ExecuteNonQuery();
            }
            else
            {
                string qry = "INSERT INTO Vehicle_Insurance(Veh_ID,v_other3) VALUES(@Veh_ID, @ImageData) SELECT @@IDENTITY";
                SqlCommand SqlCom = new SqlCommand(qry, CN);
                SqlCom.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
                SqlCom.Parameters.AddWithValue("@ImageData", (object)imageData);

                SqlCom.ExecuteNonQuery();
                CN.Close();
            }
        }

        if (Session["imgUpload9"] != null && Session["imgUpload9"] != "")
        {


            byte[] imageData = ReadFile(Server.MapPath("image") + "\\" + Session["imgUpload9"].ToString());

            string exqry = "select * from Vehicle_Insurance where Veh_ID=@Veh_ID";
            CN.Close();
            CN.Open();
            SqlCommand cdd = new SqlCommand(exqry, CN);
            cdd.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
            cdd.Parameters.AddWithValue("@ImageData", (object)imageData);
            SqlDataAdapter cda = new SqlDataAdapter(cdd);
            DataSet cds = new DataSet();
            cda.Fill(cds);
            if (cds.Tables[0].Rows.Count > 0)
            {
                string qry1 = "update Vehicle_Insurance set v_other4=@ImageData where Veh_ID=@Veh_ID";
                SqlCommand SqlCom1 = new SqlCommand(qry1, CN);
                SqlCom1.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
                SqlCom1.Parameters.AddWithValue("@ImageData", (object)imageData);

                SqlCom1.ExecuteNonQuery();
            }
            else
            {
                string qry = "INSERT INTO Vehicle_Insurance(Veh_ID,v_other4) VALUES(@Veh_ID, @ImageData) SELECT @@IDENTITY";
                SqlCommand SqlCom = new SqlCommand(qry, CN);
                SqlCom.Parameters.AddWithValue("@Veh_ID", (object)tbvehiid.Text.Trim());
                SqlCom.Parameters.AddWithValue("@ImageData", (object)imageData);

                SqlCom.ExecuteNonQuery();
                CN.Close();
            }
        }



        if (FpSpread1.Sheets[0].RowCount > 0)
        {
            FpSpread1.SaveChanges();

            for (int inew = 0; inew < FpSpread1.Sheets[0].RowCount; inew++)
            {

                string Permit = FpSpread1.Sheets[0].Cells[inew, 0].Text.ToString();
                string Permit_No = FpSpread1.Sheets[0].Cells[inew, 1].Text.ToString();
                string Permit_Date = FpSpread1.Sheets[0].Cells[inew, 2].Text.ToString();
                string permit_type = Convert.ToString(FpSpread1.Sheets[0].GetText(inew, 3));

                if (Permit_Date != "")
                {
                    string[] spl_date = Permit_Date.Split('-');
                    Permit_Date = spl_date[1] + "-" + spl_date[0] + "-" + spl_date[2];
                }

                con.Close();
                con.Open();
                string queryUpdate1;
                queryUpdate1 = "select distinct Permit_No,Permit,Permit_Date,Permit_Type from Vehicle_Insurance where Veh_ID = '" + tbvehiid.Text + "' and Permit_No <>'' and Permit_No is not null ";
                SqlCommand cmdperupdate = new SqlCommand(queryUpdate1, con);
                SqlDataAdapter dr1 = new SqlDataAdapter(cmdperupdate);
                DataTable dtnewupdate = new DataTable();
                dr1.Fill(dtnewupdate);

                if (dtnewupdate.Rows.Count > 0)
                {
                    if (Buttonsave.Text == "Update")
                    {

                        string oldpermitno = "";
                        string oldperrmitname = "";
                        string olddatepermit = "";
                        string oldpermitType = "";
                        if (dtnewupdate.Rows.Count > inew)
                        {

                            oldpermitno = dtnewupdate.Rows[inew]["Permit_No"].ToString();
                            oldperrmitname = dtnewupdate.Rows[inew]["Permit"].ToString();
                            olddatepermit = dtnewupdate.Rows[inew]["Permit_Date"].ToString();
                            oldpermitType = dtnewupdate.Rows[inew]["Permit_Type"].ToString();

                            //if (dtnewupdate.Rows[inew]["Permit_No"].ToString() != "" && dtnewupdate.Rows[inew]["Permit_No"].ToString() != null)
                            //{
                            if (oldpermitno != "")
                            {

                                string query;
                                query = "select * from Vehicle_Insurance where Veh_ID = '" + tbvehiid.Text + "' and Permit = '" + oldperrmitname + "' and Permit_No = '" + oldpermitno + "' and Permit_Date = '" + olddatepermit + "'";
                                SqlCommand cmdper = new SqlCommand(query, con);
                                SqlDataAdapter dr = new SqlDataAdapter(cmdper);
                                DataTable dtnew = new DataTable();
                                dr.Fill(dtnew);

                                cmdper.ExecuteNonQuery();
                                con.Close();
                                if (dtnew.Rows.Count > 0)
                                {
                                    con.Close();
                                    con.Open();
                                    string queryupdate;
                                    queryupdate = "Update Vehicle_Insurance set Permit='" + Permit + "',Permit_No='" + Permit_No + "',Permit_Date='" + Permit_Date + "',remainder='0',Permit_Type='" + permit_type + "' where Veh_ID='" + tbvehiid.Text + "' and Permit = '" + oldperrmitname + "' and Permit_No = '" + oldpermitno + "' and Permit_Date = '" + olddatepermit + "'";
                                    SqlCommand cmdupdate = new SqlCommand(queryupdate, con);
                                    cmdupdate.ExecuteNonQuery();
                                    con.Close();
                                }
                            }
                            else
                            {
                                //con.Close();
                                //con.Open();
                                //string queryinsert;
                                //queryinsert = "INSERT INTO Vehicle_Insurance(Veh_ID,Veh_Type,Permit,Permit_No,Permit_Date) VALUES('" + tbvehiid.Text + "','" + ddlvehicletype.SelectedItem.Text.ToString() + "','" + Permit + "','" + Permit_No + "','" + Permit_Date + "')";
                                //SqlCommand cmdinsert = new SqlCommand(queryinsert, con);
                                //cmdinsert.ExecuteNonQuery();
                                //con.Close();
                            }

                        }
                        else
                        {
                            con.Close();
                            con.Open();
                            string queryinsert;
                            queryinsert = "INSERT INTO Vehicle_Insurance(Veh_ID,Veh_Type,Permit,Permit_No,Permit_Date,remainder,Permit_Type) VALUES('" + tbvehiid.Text + "','" + ddlvehicletype.SelectedItem.Text.ToString() + "','" + Permit + "','" + Permit_No + "','" + Permit_Date + "','0','" + permit_type + "')";
                            SqlCommand cmdinsert = new SqlCommand(queryinsert, con);
                            cmdinsert.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    else
                    {
                        con.Close();
                        con.Open();
                        string queryinsert;
                        queryinsert = "INSERT INTO Vehicle_Insurance(Veh_ID,Veh_Type,Permit,Permit_No,Permit_Date,remainder,Permit_Type) VALUES('" + tbvehiid.Text + "','" + ddlvehicletype.SelectedItem.Text.ToString() + "','" + Permit + "','" + Permit_No + "','" + Permit_Date + "','0','" + permit_type + "')";
                        SqlCommand cmdinsert = new SqlCommand(queryinsert, con);
                        cmdinsert.ExecuteNonQuery();
                        con.Close();
                    }


                }
                else
                {
                    con.Close();
                    con.Open();
                    string queryinsert;
                    queryinsert = "INSERT INTO Vehicle_Insurance(Veh_ID,Veh_Type,Permit,Permit_No,Permit_Date,remainder,Permit_Type) VALUES('" + tbvehiid.Text + "','" + ddlvehicletype.SelectedItem.Text.ToString() + "','" + Permit + "','" + Permit_No + "','" + Permit_Date + "','0','" + permit_type + "')";
                    SqlCommand cmdinsert = new SqlCommand(queryinsert, con);
                    cmdinsert.ExecuteNonQuery();
                    con.Close();
                }

            }
        }





        sprdmainFC.SaveChanges();

        if (sprdmainFC.Sheets[0].RowCount > 0)
        {
            for (int i = 0; i < sprdmainFC.Sheets[0].RowCount; i++)
            {
                string FC_No = sprdmainFC.Sheets[0].Cells[i, 0].Text.ToString();
                string FC_Date = sprdmainFC.Sheets[0].Cells[i, 1].Text.ToString();
                string FC_Amount = sprdmainFC.Sheets[0].Cells[i, 2].Text.ToString();
                string FC_FileName = sprdmainFC.Sheets[0].Cells[i, 4].Text.ToString();
                string NextFcdate = sprdmainFC.Sheets[0].Cells[i, 5].Text.ToString();
                string FC_remarks = sprdmainFC.Sheets[0].Cells[i, 6].Text.ToString();

                if (NextFcdate != "")
                {
                    string[] spl_date = NextFcdate.Split('-');
                    NextFcdate = spl_date[1] + "-" + spl_date[0] + "-" + spl_date[2];
                }

                con.Close();
                con.Open();
                int check_file = 0;
                string query1;
                if (FC_FileName != "")
                {
                    query1 = "select * from Vehicle_Insurance where Veh_ID = '" + tbvehiid.Text + "' and FCcertifi_filename = '" + FC_FileName + "'";
                }
                else
                {
                    query1 = "select * from Vehicle_Insurance where Veh_ID = '" + tbvehiid.Text + "' and FC_No <>'' and FC_No is not null ";
                }

                SqlCommand cmdper = new SqlCommand(query1, con);
                SqlDataAdapter dr = new SqlDataAdapter(cmdper);
                DataTable dt_table = new DataTable();
                dr.Fill(dt_table);
                if (FC_FileName == "")
                {
                    if (dt_table.Rows.Count > 0)
                    {
                        if (Buttonsave.Text == "Update")
                        {
                            string oldpermitno = "";
                            string oldperrmitname = "";
                            string olddatepermit = "";
                            if (dt_table.Rows.Count > i)
                            {
                                oldpermitno = dt_table.Rows[i]["FC_No"].ToString();
                                oldperrmitname = dt_table.Rows[i]["FC_Date"].ToString();
                                olddatepermit = dt_table.Rows[i]["FC_Amount"].ToString();
                                if (oldpermitno != "")
                                {
                                    string query;
                                    query = "select * from Vehicle_Insurance where Veh_ID = '" + tbvehiid.Text + "' and FC_No = '" + oldpermitno + "' and FC_Date = '" + oldperrmitname + "' and FC_Amount = '" + olddatepermit + "'";
                                    SqlCommand cmdper1 = new SqlCommand(query, con);
                                    SqlDataAdapter dr1 = new SqlDataAdapter(cmdper1);
                                    DataTable dtnew = new DataTable();
                                    dr.Fill(dtnew);

                                    cmdper.ExecuteNonQuery();
                                    con.Close();
                                    if (dtnew.Rows.Count > 0)
                                    {
                                        con.Open();
                                        string query2;
                                        query2 = "Update Vehicle_Insurance set FC_No = '" + FC_No + "',FC_Date = '" + FC_Date + "',FC_Amount = '" + FC_Amount + "',NextFcdate = '" + NextFcdate + "',FC_remarks = '" + FC_remarks + "',remainder='0' where Veh_ID = '" + tbvehiid.Text + "' and FC_No = '" + oldpermitno + "' and FC_Date = '" + oldperrmitname + "' and FC_Amount = '" + olddatepermit + "'";
                                        SqlCommand cmdper1_new = new SqlCommand(query2, con);
                                        cmdper1_new.ExecuteNonQuery();
                                        con.Close();
                                    }
                                }

                            }
                            else
                            {
                                con.Close();
                                con.Open();
                                string queryinsert;
                                queryinsert = "INSERT INTO Vehicle_Insurance(Veh_ID,Veh_Type,FC_No,FC_Date,FC_Amount,NextFcdate,FC_remarks,remainder) VALUES('" + tbvehiid.Text + "','" + ddlvehicletype.SelectedItem.Text.ToString() + "','" + FC_No + "','" + FC_Date + "','" + FC_Amount + "','" + NextFcdate + "','" + FC_remarks + "','0')";
                                SqlCommand cmdinsert = new SqlCommand(queryinsert, con);
                                cmdinsert.ExecuteNonQuery();
                                con.Close();
                            }

                        }
                        else
                        {
                            con.Close();
                            con.Open();
                            string queryinsert;
                            queryinsert = "INSERT INTO Vehicle_Insurance(Veh_ID,Veh_Type,FC_No,FC_Date,FC_Amount,NextFcdate,FC_remarks,remainder) VALUES('" + tbvehiid.Text + "','" + ddlvehicletype.SelectedItem.Text.ToString() + "','" + FC_No + "','" + FC_Date + "','" + FC_Amount + "','" + NextFcdate + "','" + FC_remarks + "','0')";
                            SqlCommand cmdinsert = new SqlCommand(queryinsert, con);
                            cmdinsert.ExecuteNonQuery();
                            con.Close();
                        }


                    }
                    else
                    {
                        //query1 = "insert into Vehicle_Insurance
                        con.Close();
                        con.Open();
                        string queryinsert;
                        queryinsert = "INSERT INTO Vehicle_Insurance(Veh_ID,Veh_Type,FC_No,FC_Date,FC_Amount,NextFcdate,FC_remarks,remainder) VALUES('" + tbvehiid.Text + "','" + ddlvehicletype.SelectedItem.Text.ToString() + "','" + FC_No + "','" + FC_Date + "','" + FC_Amount + "','" + NextFcdate + "','" + FC_remarks + "','0')";
                        SqlCommand cmdinsert = new SqlCommand(queryinsert, con);
                        cmdinsert.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    con.Close();
                    con.Open();
                    string query2;
                    query2 = "Update Vehicle_Insurance set FC_No = '" + FC_No + "',FC_Date = '" + FC_Date + "',FC_Amount = '" + FC_Amount + "',NextFcdate = '" + NextFcdate + "',FC_remarks = '" + FC_remarks + "',remainder='0' where Veh_ID = '" + tbvehiid.Text + "' and FCcertifi_filename = '" + FC_FileName + "'";
                    SqlCommand cmdper1_new = new SqlCommand(query2, con);
                    cmdper1_new.ExecuteNonQuery();
                    con.Close();
                }
            }
        }






        sprdMaininsurance.SaveChanges();        //modified by raghul on 20/12/2017

        if (sprdMaininsurance.Sheets[0].RowCount > 0)
        {
            for (int i = 0; i < sprdMaininsurance.Sheets[0].RowCount; i++)
            {
                string Insu_No = sprdMaininsurance.Sheets[0].Cells[i, 0].Text.ToString();
                string Insurance_Date = sprdMaininsurance.Sheets[0].Cells[i, 1].Text.ToString();
                string Amt_Insured = sprdMaininsurance.Sheets[0].Cells[i, 2].Text.ToString();
                string Insu_Amount = sprdMaininsurance.Sheets[0].Cells[i, 3].Text.ToString();
                string InsCerificat_Filename = sprdMaininsurance.Sheets[0].Cells[i, 5].Text.ToString();
                string nextins_date = sprdMaininsurance.Sheets[0].Cells[i, 6].Text.ToString();
                string ProviderName = sprdMaininsurance.Sheets[0].Cells[i, 7].Text.ToString(); // //modified by raghul on 20/12/2017
                string ProviderContact = sprdMaininsurance.Sheets[0].Cells[i, 8].Text.ToString(); ;// //modified by raghul on 20/12/2017
                string ins_remarks = sprdMaininsurance.Sheets[0].Cells[i, 9].Text.ToString();

                if (nextins_date != "")
                {
                    string[] spl_date = nextins_date.Split('-');
                    nextins_date = spl_date[1] + "-" + spl_date[0] + "-" + spl_date[2];
                }

                con.Close();
                con.Open();
                string query1;
                if (InsCerificat_Filename != "")
                {
                    query1 = "select * from Vehicle_Insurance where Veh_ID = '" + tbvehiid.Text + "' and InsCerificat_Filename = '" + InsCerificat_Filename + "'";
                }
                else
                {
                    query1 = "select * from Vehicle_Insurance where Veh_ID = '" + tbvehiid.Text + "' and Insu_No <>'' and Insu_No is not null ";
                }
                SqlCommand cmdper = new SqlCommand(query1, con);
                SqlDataAdapter dr = new SqlDataAdapter(cmdper);
                DataTable dt_table = new DataTable();
                dr.Fill(dt_table);
                if (InsCerificat_Filename == "")
                {
                    if (dt_table.Rows.Count > 0)
                    {
                        if (Buttonsave.Text == "Update")
                        {
                            string oldpermitno = "";
                            string oldperrmitname = "";
                            string olddatepermit = "";
                            if (dt_table.Rows.Count > i)
                            {
                                oldpermitno = dt_table.Rows[i]["Insu_No"].ToString();
                                oldperrmitname = dt_table.Rows[i]["Insurance_Date"].ToString();
                                olddatepermit = dt_table.Rows[i]["Amt_Insured"].ToString();
                                if (oldpermitno != "")
                                {
                                    string query;
                                    query = "select * from Vehicle_Insurance where Veh_ID = '" + tbvehiid.Text + "' and Insu_No = '" + oldpermitno + "' and Insurance_Date = '" + oldperrmitname + "' and Amt_Insured = '" + olddatepermit + "'";
                                    SqlCommand cmdper1 = new SqlCommand(query, con);
                                    SqlDataAdapter dr1 = new SqlDataAdapter(cmdper1);
                                    DataTable dtnew = new DataTable();
                                    dr.Fill(dtnew);

                                    cmdper.ExecuteNonQuery();
                                    con.Close();
                                    if (dtnew.Rows.Count > 0)
                                    {
                                        con.Open();
                                        string query2;
                                        query2 = "Update Vehicle_Insurance set Insu_No = '" + Insu_No + "',Insurance_Date = '" + Insurance_Date + "',Amt_Insured = '" + Amt_Insured + "',Insu_Amount = '" + Insu_Amount + "',nextins_date = '" + nextins_date + "',provider_name='"+ProviderName+"',provider_contact_details='"+ProviderContact+"',ins_remarks = '" + ins_remarks + "',remainder='0' where Veh_ID = '" + tbvehiid.Text + "' and Insu_No = '" + oldpermitno + "' and Insurance_Date = '" + oldperrmitname + "' and Amt_Insured = '" + olddatepermit + "'";
                                        SqlCommand cmdperup = new SqlCommand(query2, con);   //modified by raghul on 20/12/2017
                                        cmdperup.ExecuteNonQuery();
                                        con.Close();
                                    }
                                }
                            }
                            else
                            {
                                con.Close();
                                con.Open();
                                string queryinsert;
                                queryinsert = "INSERT INTO Vehicle_Insurance(Veh_ID,Veh_Type,Insu_No,Insurance_Date,Amt_Insured,Insu_Amount,nextins_date,provider_Name,provider_contact_details,ins_remarks,remainder) VALUES('" + tbvehiid.Text + "','" + ddlvehicletype.SelectedItem.Text.ToString() + "','" + Insu_No + "','" + Insurance_Date + "','" + Amt_Insured + "','" + Insu_Amount + "','" + nextins_date + "','"+ProviderName+"','"+ProviderContact+"','" + ins_remarks + "','0')";
                                SqlCommand cmdinsert = new SqlCommand(queryinsert, con);   //modified by raghul on 20/12/2017
                                cmdinsert.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                        else
                        {
                            con.Close();
                            con.Open();
                            string queryinsert;
                            queryinsert = "INSERT INTO Vehicle_Insurance(Veh_ID,Veh_Type,Insu_No,Insurance_Date,Amt_Insured,Insu_Amount,nextins_date,provider_Name,provider_contact_details,ins_remarks,,remainder) VALUES('" + tbvehiid.Text + "','" + ddlvehicletype.SelectedItem.Text.ToString() + "','" + Insu_No + "','" + Insurance_Date + "','" + Amt_Insured + "','" + Insu_Amount + "','" + nextins_date + "','"+ProviderName+"','"+ProviderContact+"','" + ins_remarks + "','0')";
                            SqlCommand cmdinsert = new SqlCommand(queryinsert, con);    //modified by raghul on 20/12/2017
                            cmdinsert.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    else
                    {
                        con.Close();
                        con.Open();
                        string queryinsert;
                        queryinsert = "INSERT INTO Vehicle_Insurance(Veh_ID,Veh_Type,Insu_No,Insurance_Date,Amt_Insured,Insu_Amount,nextins_date,provider_Name,provider_contact_details,ins_remarks,remainder) VALUES('" + tbvehiid.Text + "','" + ddlvehicletype.SelectedItem.Text.ToString() + "','" + Insu_No + "','" + Insurance_Date + "','" + Amt_Insured + "','" + Insu_Amount + "','" + nextins_date + "','" +ProviderName+"','"+ProviderContact+"','"+ ins_remarks + "','0')";
                        SqlCommand cmdinsert = new SqlCommand(queryinsert, con);    //modified by raghul on 20/12/2017
                        cmdinsert.ExecuteNonQuery();
                        con.Close();
                    }
                }
                else
                {
                    con.Close();
                    con.Open();
                    string query2;
                    query2 = "Update Vehicle_Insurance set Insu_No = '" + Insu_No + "',Insurance_Date = '" + Insurance_Date + "',Amt_Insured = '" + Amt_Insured + "',Insu_Amount = '" + Insu_Amount + "',nextins_date = '" + nextins_date + "',provider_name='"+ProviderName+"',provider_contact_details='"+ProviderContact+"',ins_remarks = '" + ins_remarks + "',remainder='0' where Veh_ID = '" + tbvehiid.Text + "' and InsCerificat_Filename = '" + InsCerificat_Filename + "'";
                    SqlCommand cmdper1 = new SqlCommand(query2, con);  //modified by raghul on 20/12/2017
                    cmdper1.ExecuteNonQuery();
                    con.Close();
                }

            }
        }

//added by srinath 24/12/2013

        hastab.Add("gendertype", ddlgender.SelectedValue.ToString());

        if (Buttonsave.Text == "Save")
        {
            hastab.Add("College_code", selected_college);
            int n = dset.insert_method("Vehicle_insert", hastab, "sp");
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
        }
        else if (Buttonsave.Text == "Update")
        {
            hastab.Add("College_code", selected_college);
            int n = dset.insert_method("Vehicle_update", hastab, "sp");
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Updated Successfully')", true);

        }

        clear();
        LoadMainEnquiry_date();
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        mpemsgboxdelete.Hide();
        try
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
            sqlcmd = "delete from vehicle_master where Veh_ID ='" + tbvehiid.Text.Trim() + "'";
            int n = dset.update_method_wo_parameter(sqlcmd, "n");
            LoadMainEnquiry_date();
            Buttondelete.Enabled = false;
            clear();
        }
        catch
        {

        }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mpemsgboxdelete.Hide();
    }
    protected void Buttondelete_Click(object sender, EventArgs e)
    {
        if (tbvehiid.Text.Trim() != "")
            mpemsgboxdelete.Show();
    }
    protected void routebtn_Click(object sender, EventArgs e)
    {

    }
    protected void ddlvehicletypeview_SelectedIndexChanged(object sender, EventArgs e)
    {
        Connection();
        string sqlquery = string.Empty;
        ddltypeview.Items.Clear();
        ddltypeview.Items.Insert(0, new ListItem("All", "-1"));
        if (ddlvehicletypeview.Text == "-1")
        {
            sqlquery = "select * from vehicle_master order by len(veh_id), Veh_ID";
        }
        else
        {
            sqlquery = "select * from vehicle_master  where veh_Type = '" + ddlvehicletypeview.Text.ToString() + "' order by len(veh_id), Veh_ID";
        }
        ds = da.select_method_wo_parameter(sqlquery, "txt");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ddltypeview.Items.Add(ds.Tables[0].Rows[i]["Veh_ID"].ToString());
            }
            ddltypeview.SelectedIndex = 0;
        }
        con.Close();
        loadengineno();
    }
    protected void ddltypeview_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadengineno();
    }
    protected void btnMainGo_Click(object sender, EventArgs e)
    {
        string typeall = "";
        string veh_all = "";

        if (ddlvehicletypeview.Text != "All")
        {
            if (ddlvehicletypeview.Items.Count > 0)
            {
                if (ddlvehicletypeview.Text == "-1")
                {
                    for (int i = 0; i < ddlvehicletypeview.Items.Count; i++)
                    {
                        if (i > 0)
                        {
                            if (typeall == "")
                            {
                                typeall = ddlvehicletypeview.Items[i].Text.ToString();
                            }
                            else
                            {
                                typeall = typeall + "','" + ddlvehicletypeview.Items[i].Text.ToString();
                            }
                        }
                    }

                }
                else
                {
                    typeall = ddlvehicletypeview.Text.ToString();
                }
            }
        }

        if (typeall != "")
        {
            typeall = " and Veh_Type in('" + typeall + "')";
        }

        if (ddltypeview.Text != "All")
        {
            if (ddltypeview.Items.Count > 0)
            {
                if (ddltypeview.Text == "-1")
                {
                    for (int i = 0; i < ddltypeview.Items.Count; i++)
                    {
                        if (i > 0)
                        {
                            if (veh_all == "")
                            {
                                veh_all = ddltypeview.Items[i].Text.ToString();
                            }
                            else
                            {
                                veh_all = veh_all + "','" + ddltypeview.Items[i].Text.ToString();
                            }
                        }
                    }
                }
                else
                {
                    veh_all = ddltypeview.Text.ToString();
                }
            }
        }

        if (veh_all != "")
        {
            veh_all = "and Veh_ID in('" + veh_all + "')";
        }
        string regno = "";
        if (ddlregno.Items.Count > 0)
        {
            if (ddlregno.Text != "All" && ddlregno.Text != "-1")
            {
                if (ddlregno.SelectedItem.ToString() != "-1" && ddlregno.SelectedItem.ToString() != "-1")
                {
                    regno = "'" + ddlregno.SelectedItem.ToString() + "'";
                }
            }
        }
        if (regno != "")
        {
            regno = "and Reg_No in(" + regno + ")";
        }

        sprdMainEnquiry.Sheets[0].AutoPostBack = true;
        sqlcmd = "select * from Vehicle_Master where Veh_ID is not null " + typeall + " " + veh_all + " " + regno + " order by len(veh_id), Veh_ID"; //modifed by prabha on 29jan2018
        //  sqlcmd = "select Reg_Date,case when Purchased_On='1900-01-01 00:00:00.000' then '' else convert(varchar(10),Purchased_On,103)end,Veh_Type,Veh_ID,Reg_No,RC_No,Numberofowner,Place_Reg,Duration from Vehicle_Master where Veh_ID is not null " + typeall + " " + veh_all + " " + regno + " order by len(veh_id), Veh_ID";
        dsload = dset.select_method_wo_parameter(sqlcmd, "Text");
        sprdMainEnquiry.Sheets[0].RowCount = 0;
        try
        {
            if (dsload.Tables[0].Rows.Count > 0)
            {
                int loop = 0;
                for (loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                {
                    string regdate = dsload.Tables[0].Rows[loop]["Reg_Date"].ToString();
                    if (regdate.Trim() != "" && regdate != null)
                    {
                        string[] datereg1 = Convert.ToString(regdate).Split(new char[] { ' ' });
                        string[] spitdate = datereg1[0].Split('/');
                        if (spitdate.GetUpperBound(0) >= 2)
                        {
                            regdate = spitdate[1] + '/' + spitdate[0] + '/' + spitdate[2];
                        }
                    }

                    string purcharon = dsload.Tables[0].Rows[loop]["Purchased_On"].ToString();
                    if (purcharon.Trim() != "" && purcharon != null && purcharon != "1/1/1900 12:00:00 AM")
                    {
                        string[] soiltdate = purcharon.Split(' ');
                        string[] spitdate = soiltdate[0].Split('/');
                        if (spitdate.GetUpperBound(0) >= 2)
                            purcharon = spitdate[1] + '/' + spitdate[0] + '/' + spitdate[2];
                    }
                    else
                        purcharon = "";


                    //DateTime date2 = Convert.ToDateTime(dsload.Tables[0].Rows[loop]["Purchased_On"].ToString());
                    //string[] datepur = Convert.ToString(date2).Split(new char[] { ' ' });
                    lblerrordate.Visible = false;
                    sprdMainEnquiry.Sheets[0].RowCount++;
                    sprdMainEnquiry.Sheets[0].Cells[sprdMainEnquiry.Sheets[0].RowCount - 1, 0].Text = dsload.Tables[0].Rows[loop]["Veh_Type"].ToString();
                    sprdMainEnquiry.Sheets[0].Cells[sprdMainEnquiry.Sheets[0].RowCount - 1, 1].Text = dsload.Tables[0].Rows[loop]["Veh_ID"].ToString();
                    sprdMainEnquiry.Sheets[0].Cells[sprdMainEnquiry.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["Reg_No"].ToString();
                    sprdMainEnquiry.Sheets[0].Cells[sprdMainEnquiry.Sheets[0].RowCount - 1, 3].Text = regdate;
                    sprdMainEnquiry.Sheets[0].Cells[sprdMainEnquiry.Sheets[0].RowCount - 1, 4].Text = dsload.Tables[0].Rows[loop]["RC_No"].ToString();
                    string typeval = Convert.ToString(dsload.Tables[0].Rows[loop]["Type"]);
                    string type = "";
                    if (typeval.Trim() == "0")
                        type = "New";
                    else
                        type = "Old";

                    sprdMainEnquiry.Sheets[0].Cells[sprdMainEnquiry.Sheets[0].RowCount - 1, 5].Text = type;
                    sprdMainEnquiry.Sheets[0].Cells[sprdMainEnquiry.Sheets[0].RowCount - 1, 6].Text = dsload.Tables[0].Rows[loop]["Numberofowner"].ToString();
                    sprdMainEnquiry.Sheets[0].Cells[sprdMainEnquiry.Sheets[0].RowCount - 1, 7].Text = purcharon;
                    sprdMainEnquiry.Sheets[0].Cells[sprdMainEnquiry.Sheets[0].RowCount - 1, 8].Text = dsload.Tables[0].Rows[loop]["Place_Reg"].ToString();
                    sprdMainEnquiry.Sheets[0].Cells[sprdMainEnquiry.Sheets[0].RowCount - 1, 9].Text = dsload.Tables[0].Rows[loop]["Duration"].ToString();

                }
                sprdMainEnquiry.Sheets[0].PageSize = sprdMainEnquiry.Sheets[0].RowCount;
                // sprdMainEnquiry.SaveChanges();
                sprdMainEnquiry.Visible = true;
                lblerrordate.Visible = false;
                ddltypeview.Enabled = true;

            }
            else
            {
                lblerrordate.Visible = true;
                lblerrordate.Text = "No Record(s) Found";
                ddltypeview.Enabled = true;

            }
        }
        catch
        {

        }

    }

    public void loadengineno()
    {
        ddlregno.Items.Clear();
        string typeall = "";
        string veh_all = "";

        if (ddlvehicletypeview.Text != "All")
        {
            if (ddlvehicletypeview.Items.Count > 0)
            {
                if (ddlvehicletypeview.Text == "-1")
                {
                    for (int i = 0; i < ddlvehicletypeview.Items.Count; i++)
                    {
                        if (i > 0)
                        {
                            if (typeall == "")
                            {
                                typeall = ddlvehicletypeview.Items[i].Text.ToString();
                            }
                            else
                            {
                                typeall = typeall + "','" + ddlvehicletypeview.Items[i].Text.ToString();
                            }
                        }
                    }

                }
                else
                {
                    typeall = ddlvehicletypeview.Text.ToString();
                }
            }
        }

        if (typeall != "")
        {
            typeall = " and Veh_Type in('" + typeall + "')";
        }

        if (ddltypeview.Text != "All")
        {
            if (ddltypeview.Items.Count > 0)
            {
                if (ddltypeview.Text == "-1")
                {
                    for (int i = 0; i < ddltypeview.Items.Count; i++)
                    {
                        if (i > 0)
                        {
                            if (veh_all == "")
                            {
                                veh_all = ddltypeview.Items[i].Text.ToString();
                            }
                            else
                            {
                                veh_all = veh_all + "','" + ddltypeview.Items[i].Text.ToString();
                            }
                        }
                    }
                }
                else
                {
                    veh_all = ddltypeview.Text.ToString();
                }
            }
        }

        if (veh_all != "")
        {
            veh_all = "and Veh_ID in('" + veh_all + "')";
        }
        string strquery = "select Reg_No from Vehicle_Master where Veh_ID is not null " + veh_all + " " + typeall + "";
        DataSet dsreg = dset.select_method_wo_parameter(strquery, "Text");
        if (dsreg.Tables[0].Rows.Count > 0)
        {

            ddlregno.DataSource = dsreg;
            ddlregno.DataValueField = "Reg_No";
            ddlregno.DataTextField = "Reg_No";
            ddlregno.DataBind();
            if (dsreg.Tables[0].Rows.Count > 1)
            {
                ddlregno.Items.Insert(0, new ListItem("All", "-1"));
            }
        }
    }

    protected void addnew_Click(object sender, EventArgs e)
    {
        caption = newcaption.InnerHtml;
        Paneladd.Visible = false;
        if (caption == "VehicleType")
        {
            if (tbaddnew.Text != "")
            {
                Paneladd.Visible = false;
                hastab.Clear();

                hastab.Add("tcrit", "vtype");
                hastab.Add("tval", tbaddnew.Text.Trim());
                d1 = dset.select_method("enquiry_add_textcodenew", hastab, "sp");
                if (d1.Tables.Count > 0)
                {
                    ddlvehicletype.Items.Clear();
                    if (d1.Tables[0].Rows.Count > 0)
                    {

                        ddlvehicletype.DataSource = d1;
                        ddlvehicletype.DataTextField = "Textval";
                        ddlvehicletype.DataValueField = "textcode";
                        ddlvehicletype.DataBind();

                    }
                    ddlvehicletype.Items.Insert(0, "");

                }
                else
                {
                    lblerrvehicletype.Text = "Already Exists";
                    lblerrvehicletype.Visible = true;
                }
                ddlvehicletype.SelectedIndex = ddlvehicletype.Items.IndexOf(ddlvehicletype.Items.FindByText(tbaddnew.Text.Trim()));
                tbaddnew.Text = "";
                //ddlvehicletype.SelectedIndex = ddlvehicletype.Items.IndexOf(ddlvehicletype.Items.FindByText(tbaddnew.Text.Trim()));
            }
            else
            {
                lblerrvehicletype.Text = "Please Enter Vehicle";
                lblerrvehicletype.Visible = true;
            }
        }




        else if (caption == "VehiclePurpose")
        {
            if (tbaddnew.Text != "")
            {
                Paneladd.Visible = false;
                hastab.Clear();

                hastab.Add("tcrit", "vpur");
                hastab.Add("tval", tbaddnew.Text.Trim());
                d1 = dset.select_method("enquiry_add_textcodenew", hastab, "sp");
                if (d1.Tables.Count > 0)
                {
                    ddlvehiclepur.Items.Clear();
                    if (d1.Tables[0].Rows.Count > 0)
                    {

                        ddlvehiclepur.DataSource = d1;
                        ddlvehiclepur.DataTextField = "Textval";
                        ddlvehiclepur.DataValueField = "textcode";
                        ddlvehiclepur.DataBind();

                    }
                    ddlvehiclepur.Items.Insert(0, "");

                }
                else
                {
                    lblvehicleerrpur.Text = "Already Exists";
                    lblvehicleerrpur.Visible = true;
                }
                ddlvehiclepur.SelectedIndex = ddlvehiclepur.Items.IndexOf(ddlvehiclepur.Items.FindByText(tbaddnew.Text.Trim()));
                tbaddnew.Text = "";
            }
            else
            {
                lblvehicleerrpur.Text = "Please Enter Purpose";
                lblvehicleerrpur.Visible = true;
            }
        }
        else if (caption == "Dealer")
        {
            if (tbaddnew.Text != "")
            {
                Paneladd.Visible = false;
                hastab.Clear();

                hastab.Add("tcrit", "deal");
                hastab.Add("tval", tbaddnew.Text.Trim());
                d1 = dset.select_method("enquiry_add_textcodenew", hastab, "sp");
                if (d1.Tables.Count > 0)
                {
                    ddldealerdetails.Items.Clear();
                    if (d1.Tables[0].Rows.Count > 0)
                    {


                        ddldealerdetails.DataSource = d1;
                        ddldealerdetails.DataTextField = "Textval";
                        ddldealerdetails.DataValueField = "textcode";
                        ddldealerdetails.DataBind();

                    }
                    ddldealerdetails.Items.Insert(0, "");

                }
                else
                {
                    lblerredealer.Text = "Already Exists";
                    lblerredealer.Visible = true;
                }
                ddldealerdetails.SelectedIndex = ddldealerdetails.Items.IndexOf(ddldealerdetails.Items.FindByText(tbaddnew.Text.Trim()));
                //ddldealerdetails.SelectedIndex = ddldealerdetails.Items.IndexOf(ddldealerdetails.Items.FindByText(tbaddnew.Text.Trim()));
                tbaddnew.Text = "";
            }
            else
            {
                lblerredealer.Text = "Please Enter Dealer";
                lblerredealer.Visible = true;
            }
        }
        else if (caption == "State")
        {
            if (tbaddnew.Text != "")
            {
                Paneladd.Visible = false;
                hastab.Clear();

                hastab.Add("tcrit", "state");
                hastab.Add("tval", tbaddnew.Text.Trim());
                d1 = dset.select_method("enquiry_add_textcodenew", hastab, "sp");
                if (d1.Tables.Count > 0)
                {
                    ddlstaterto.Items.Clear();
                    if (d1.Tables[0].Rows.Count > 0)
                    {

                        ddlstaterto.DataSource = d1;
                        ddlstaterto.DataTextField = "Textval";
                        ddlstaterto.DataValueField = "textcode";
                        ddlstaterto.DataBind();

                    }
                    ddlstaterto.Items.Insert(0, "");

                }
                else
                {
                    lblerrorstate.Text = "Already Exists";
                    lblerrorstate.Visible = true;
                }
                ddlstaterto.SelectedIndex = ddlstaterto.Items.IndexOf(ddlstaterto.Items.FindByText(tbaddnew.Text.Trim()));
                //ddlstaterto.SelectedIndex = ddlstaterto.Items.IndexOf(ddlstaterto.Items.FindByText(tbaddnew.Text.Trim()));
                tbaddnew.Text = "";
            }
            else
            {
                lblerrorstate.Text = "Please Enter State";
                lblerrorstate.Visible = true;
            }
        }


    }
    protected void exitnew_Click(object sender, EventArgs e)
    {
        tbaddnew.Text = "";
        Paneladd.Visible = false;
    }
    protected void sprdMainEnquiry_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        sprdMainEnquiry.Sheets[0].AutoPostBack = true;
        //Cell = true;

        string activerow = sprdMainEnquiry.ActiveSheetView.ActiveRow.ToString();
        string activecol = sprdMainEnquiry.ActiveSheetView.ActiveColumn.ToString();
        Cellclick = true;
        Accordion1.SelectedIndex = 1;

    }
    protected void sprdMainEnquiry_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cellclick == true)
        {
            clear();
            string activerow = "";
            string activecol = "";
            Buttonsave.Text = "Update";
            btnsave.Text = "Update";
            btnsave1.Text = "Update";
            tbvehiid.Enabled = false;
            ddlvehicletype.Enabled = false;

            lbladdview.Text = "Modify";

            activerow = sprdMainEnquiry.ActiveSheetView.ActiveRow.ToString();
            activecol = sprdMainEnquiry.ActiveSheetView.ActiveColumn.ToString();
            

            string Veh_ID = sprdMainEnquiry.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
            string veh_type = sprdMainEnquiry.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Text;

            string selectquery1 = "select * from Vehicle_Insurance where Veh_ID='" + Veh_ID + "' ";
            if (selectquery1 != "")
            {
                SqlDataAdapter daselectquery1 = new SqlDataAdapter(selectquery1, con5);
                DataSet dsselectquery1 = new DataSet();
                daselectquery1.Fill(dsselectquery1);
                con5.Close();
                con5.Open();
                FpSpread1.Sheets[0].RowCount = 0;
                sprdMaininsurance.Sheets[0].RowCount = 0;
                sprdmainFC.Sheets[0].RowCount = 0;

                if (dsselectquery1.Tables[0].Rows.Count > 0)
                {
                    clear();
                    Buttonsave.Enabled = true;
                    Buttondelete.Enabled = true;
                    chk = 1;
                    int k = 0;
                    //Modified by srinath 29/03/2014 ===============Start
                    string permitquery = "select distinct Permit,Permit_No,Permit_Date,Permit_Type from Vehicle_Insurance  where  Veh_ID='" + Veh_ID + "' and Permit is not null and Permit<>''";
                    DataSet dspermit = dset.select_method_wo_parameter(permitquery, "Text");

                    // for (int i2 = 0; i2 < dsselectquery1.Tables[0].Rows.Count; i2++)
                    // {
                    string Permit = "", Permit_No = "", Permit_Date = "", permit_type = "";
                    string Insu_No = "", Insurance_Date = "", Amt_Insured = "", Insu_Amount = "", InsCerificat_Filename = "", nextins_date = "",provider_name="",provider_contact_details="", ins_remarks = "";
                    string FC_No = "", FC_Date = "", FC_Amount = "", FCcertifi_filename = "", NextFcdate = "", FC_remarks = "";
                    for (int i2 = 0; i2 < dspermit.Tables[0].Rows.Count; i2++)
                    {
                        Permit = dspermit.Tables[0].Rows[i2]["Permit"].ToString();
                        Permit_No = dspermit.Tables[0].Rows[i2]["Permit_No"].ToString();
                        Permit_Date = dspermit.Tables[0].Rows[i2]["Permit_Date"].ToString();
                        permit_type = Convert.ToString(dspermit.Tables[0].Rows[i2]["Permit_Type"]);

                        if (Permit_Date != "")
                        {
                            string[] spl_perdate = Permit_Date.Split(' ');
                            string[] spl_date = spl_perdate[0].Split('/');
                            Permit_Date = spl_date[1] + "-" + spl_date[0] + "-" + spl_date[2];
                        }
                        if (Permit != "" && Permit != null)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Permit;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Permit_No;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Permit_Date;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = permit_type;
                            //k = k + 1;
                        }
                    }
                    permitquery = "select * from Vehicle_Insurance  where  Veh_ID='" + Veh_ID + "' and insu_no is not null  and insu_no <>''";
                    dspermit = dset.select_method_wo_parameter(permitquery, "Text");
                    for (int i2 = 0; i2 < dspermit.Tables[0].Rows.Count; i2++)
                    {
                        Insu_No = dspermit.Tables[0].Rows[i2]["Insu_No"].ToString();
                        Insurance_Date = dspermit.Tables[0].Rows[i2]["Insurance_Date"].ToString();
                        Amt_Insured = dspermit.Tables[0].Rows[i2]["Amt_Insured"].ToString();
                        Insu_Amount = dspermit.Tables[0].Rows[i2]["Insu_Amount"].ToString();
                        InsCerificat_Filename = dspermit.Tables[0].Rows[i2]["InsCerificat_Filename"].ToString();
                        provider_name = dspermit.Tables[0].Rows[i2]["provider_name"].ToString();
                        provider_contact_details = dspermit.Tables[0].Rows[i2]["provider_Contact_details"].ToString();

                        //added by prabha on feb 28 2018
                        if (!string.IsNullOrEmpty(dspermit.Tables[0].Rows[i2]["nextins_date"].ToString()))  
                        nextins_date = Convert.ToDateTime(dspermit.Tables[0].Rows[i2]["nextins_date"].ToString()).Date.ToShortDateString();
                        

                        //modified by raghul on 20/12/2017
                        ins_remarks = dspermit.Tables[0].Rows[i2]["ins_remarks"].ToString();

                        if (nextins_date != "")
                        {
                            string[] spl_perdate = nextins_date.Split(' ');
                            string[] spl_date = spl_perdate[0].Split('/');
                            nextins_date = spl_date[1] + "-" + spl_date[0] + "-" + spl_date[2];
                        }
                        if (Insu_No != "" && Insu_No.ToString() != null)
                        {
                            sprdMaininsurance.Sheets[0].RowCount++;
                            sprdMaininsurance.Sheets[0].Cells[sprdMaininsurance.Sheets[0].RowCount - 1, 0].Text = Insu_No;
                            sprdMaininsurance.Sheets[0].Cells[sprdMaininsurance.Sheets[0].RowCount - 1, 1].Text = Insurance_Date;
                            sprdMaininsurance.Sheets[0].Cells[sprdMaininsurance.Sheets[0].RowCount - 1, 2].Text = Amt_Insured;
                            sprdMaininsurance.Sheets[0].Cells[sprdMaininsurance.Sheets[0].RowCount - 1, 3].Text = Insu_Amount;
                            sprdMaininsurance.Sheets[0].Cells[sprdMaininsurance.Sheets[0].RowCount - 1, 5].Text = InsCerificat_Filename;
                            sprdMaininsurance.Sheets[0].Cells[sprdMaininsurance.Sheets[0].RowCount - 1, 6].Text = nextins_date;
                            sprdMaininsurance.Sheets[0].Cells[sprdMaininsurance.Sheets[0].RowCount - 1, 7].Text = provider_name;
                            sprdMaininsurance.Sheets[0].Cells[sprdMaininsurance.Sheets[0].RowCount - 1, 8].Text = provider_contact_details;         //modified by raghul on 20/12/2017
                            sprdMaininsurance.Sheets[0].Cells[sprdMaininsurance.Sheets[0].RowCount - 1, 9].Text = ins_remarks;
                            //sprdMaininsurance.Sheets[0].Cells[i2, 0].Text = dsselectquery1.Tables[0].Rows[i2]["Insu_No"].ToString();
                        }
                    }
                    permitquery = "select * from Vehicle_Insurance  where  Veh_ID='" + Veh_ID + "' and  FC_No is not null and FC_No<>''";
                    dspermit = dset.select_method_wo_parameter(permitquery, "Text");
                    for (int i2 = 0; i2 < dspermit.Tables[0].Rows.Count; i2++)
                    {
                        FC_No = dspermit.Tables[0].Rows[i2]["FC_No"].ToString();
                        FC_Date = dspermit.Tables[0].Rows[i2]["FC_Date"].ToString();
                        FC_Amount = dspermit.Tables[0].Rows[i2]["FC_Amount"].ToString();
                        FCcertifi_filename = dspermit.Tables[0].Rows[i2]["FCcertifi_filename"].ToString();
                        NextFcdate = dspermit.Tables[0].Rows[i2]["NextFcdate"].ToString();
                        FC_remarks = dspermit.Tables[0].Rows[i2]["FC_remarks"].ToString();

                        if (NextFcdate != "")
                        {
                            string[] spl_perdate = NextFcdate.Split(' ');
                            string[] spl_date = spl_perdate[0].Split('/');
                            NextFcdate = spl_date[1] + "-" + spl_date[0] + "-" + spl_date[2];
                        }
                        if (FC_No != "" && FC_No != null)
                        {
                            sprdmainFC.Sheets[0].RowCount++;
                            sprdmainFC.Sheets[0].Cells[sprdmainFC.Sheets[0].RowCount - 1, 0].Text = FC_No;
                            sprdmainFC.Sheets[0].Cells[sprdmainFC.Sheets[0].RowCount - 1, 1].Text = FC_Date;
                            sprdmainFC.Sheets[0].Cells[sprdmainFC.Sheets[0].RowCount - 1, 2].Text = FC_Amount;
                            sprdmainFC.Sheets[0].Cells[sprdmainFC.Sheets[0].RowCount - 1, 4].Text = FCcertifi_filename;
                            sprdmainFC.Sheets[0].Cells[sprdmainFC.Sheets[0].RowCount - 1, 5].Text = NextFcdate;
                            sprdmainFC.Sheets[0].Cells[sprdmainFC.Sheets[0].RowCount - 1, 6].Text = FC_remarks;

                        }

                    }
                    // if (dsselectquery1.Tables[0].Rows[i2]["Permit"].ToString() != "" && dsselectquery1.Tables[0].Rows[i2]["Permit"].ToString() != null)
                    //  if (dsselectquery1.Tables[0].Rows[i2]["Insu_No"].ToString() != "" && dsselectquery1.Tables[0].Rows[i2]["Insu_No"].ToString() != null)
                    // }=================================================================End

                    try
                    {
                        if (dsselectquery1.Tables[0].Rows[0]["Reg_Photo"].ToString() != null && dsselectquery1.Tables[0].Rows[0]["Reg_Photo"].ToString() != "")
                        {
                            ImageRegCer.Visible = true;
                            ImageRegCer.ImageUrl = "Handler/Veh_Reg_Photo.ashx?id=" + Veh_ID;
                        }
                        else
                        {
                            ImageRegCer.Visible = false;
                        }
                        if (dsselectquery1.Tables[0].Rows[0]["v_Front"].ToString() != null && dsselectquery1.Tables[0].Rows[0]["v_Front"].ToString() != "")
                        {
                            imgFrontPhoto.Visible = true;
                            imgFrontPhoto.ImageUrl = "Handler/Veh_Front_Photo.ashx?id=" + Veh_ID;
                        }
                        else
                        {
                            imgFrontPhoto.Visible = false;
                        }
                        if (dsselectquery1.Tables[0].Rows[0]["v_Back"].ToString() != null && dsselectquery1.Tables[0].Rows[0]["v_Back"].ToString() != "")
                        {
                            ImgBackPhoto.Visible = true;
                            ImgBackPhoto.ImageUrl = "Handler/Veh_Back_Photo.ashx?id=" + Veh_ID;
                        }
                        else
                        {
                            ImgBackPhoto.Visible = false;
                        }
                        if (dsselectquery1.Tables[0].Rows[0]["v_left"].ToString() != null && dsselectquery1.Tables[0].Rows[0]["v_left"].ToString() != "")
                        {
                            imgleftphoto.Visible = true;
                            imgleftphoto.ImageUrl = "Handler/Veh_Left_Photo.ashx?id=" + Veh_ID;
                        }
                        else
                        {
                            imgleftphoto.Visible = false;
                        }
                        if (dsselectquery1.Tables[0].Rows[0]["v_right"].ToString() != null && dsselectquery1.Tables[0].Rows[0]["v_right"].ToString() != "")
                        {
                            imgrightphoto.Visible = true;
                            imgrightphoto.ImageUrl = "Handler/Veh_Right_Photo.ashx?id=" + Veh_ID;
                        }
                        else
                        {
                            imgrightphoto.Visible = false;
                        }
                        if (dsselectquery1.Tables[0].Rows[0]["v_other1"].ToString() != null && dsselectquery1.Tables[0].Rows[0]["v_other1"].ToString() != "")
                        {
                            imgother1photo.Visible = true;
                            imgother1photo.ImageUrl = "Handler/Veh_Other1_Photo.ashx?id=" + Veh_ID;
                        }
                        else
                        {
                            imgother1photo.Visible = false;
                        }
                        if (dsselectquery1.Tables[0].Rows[0]["v_other2"].ToString() != null && dsselectquery1.Tables[0].Rows[0]["v_other2"].ToString() != "")
                        {
                            imgother2photo.Visible = true;
                            imgother2photo.ImageUrl = "Handler/Veh_Other2_Photo.ashx?id=" + Veh_ID;
                        }
                        else
                        {
                            imgother2photo.Visible = false;
                        }
                        if (dsselectquery1.Tables[0].Rows[0]["v_other3"].ToString() != null && dsselectquery1.Tables[0].Rows[0]["v_other3"].ToString() != "")
                        {
                            imgother3photo.Visible = true;
                            imgother3photo.ImageUrl = "Handler/Veh_Other3_Photo.ashx?id=" + Veh_ID;
                        }
                        else
                        {
                            imgother3photo.Visible = false;
                        }
                        if (dsselectquery1.Tables[0].Rows[0]["v_other4"].ToString() != null && dsselectquery1.Tables[0].Rows[0]["v_other4"].ToString() != "")
                        {
                            imgother4photo.Visible = true;
                            imgother4photo.ImageUrl = "Handler/Veh_Other4_Photo.ashx?id=" + Veh_ID;
                        }
                        else
                        {
                            imgother4photo.Visible = false;
                        }
                    }
                    catch
                    {

                    }
                }
            }
            else
            {

            }

            string selectquery = "select * from vehicle_master where Veh_ID='" + Veh_ID + "' ";
            if (selectquery != "")
            {
                SqlDataAdapter daselectquery = new SqlDataAdapter(selectquery, con5);
                DataSet dsselectquery = new DataSet();
                daselectquery.Fill(dsselectquery);
                con5.Close();
                con5.Open();

                if (dsselectquery.Tables[0].Rows.Count > 0)
                {
                    //clear();
                    Buttonsave.Enabled = true;
                    Buttondelete.Enabled = true;
                    chk = 1;
                    for (int i1 = 0; i1 < dsselectquery.Tables[0].Rows.Count; i1++)
                    {

                        string Veh_Type = dsselectquery.Tables[0].Rows[i1]["Veh_Type"].ToString();
                        string veh_id = dsselectquery.Tables[0].Rows[i1]["Veh_ID"].ToString();
                        string Reg_No = dsselectquery.Tables[0].Rows[i1]["Reg_No"].ToString();
                        string Reg_Date = dsselectquery.Tables[0].Rows[i1]["Reg_Date"].ToString();
                        string RC_No = dsselectquery.Tables[0].Rows[i1]["RC_No"].ToString();
                        string Numberofowner = dsselectquery.Tables[0].Rows[i1]["Numberofowner"].ToString();
                        string VehicleCast = dsselectquery.Tables[0].Rows[i1]["VehicleCast"].ToString();
                        string Tax = dsselectquery.Tables[0].Rows[i1]["Tax"].ToString();
                        string TotalPur_Amount = dsselectquery.Tables[0].Rows[i1]["TotalPur_Amount"].ToString();
                        string Vehicle_Ins = dsselectquery.Tables[0].Rows[i1]["Vehicle_Ins"].ToString();
                        string Purpose_Vehicle = dsselectquery.Tables[0].Rows[i1]["Purpose_Vehicle"].ToString();
                        string Purchased_On = dsselectquery.Tables[0].Rows[i1]["Purchased_On"].ToString();
                        string Place_Reg = dsselectquery.Tables[0].Rows[i1]["Place_Reg"].ToString();
                        string Permit = dsselectquery.Tables[0].Rows[i1]["Permit"].ToString();
                        string Permit_No = dsselectquery.Tables[0].Rows[i1]["Permit_No"].ToString();
                        string Permit_Date = dsselectquery.Tables[0].Rows[i1]["Permit_Date"].ToString();
                        string Duration = dsselectquery.Tables[0].Rows[i1]["Duration"].ToString();
                        string TotalNo_Seat = dsselectquery.Tables[0].Rows[i1]["TotalNo_Seat"].ToString();
                        string Extra_No = dsselectquery.Tables[0].Rows[i1]["Extra_No"].ToString();
                        string Intial_Km = dsselectquery.Tables[0].Rows[i1]["Intial_Km"].ToString();
                        string Renew_Date = dsselectquery.Tables[0].Rows[i1]["Renew_Date"].ToString();
                        string nofTravrs = dsselectquery.Tables[0].Rows[i1]["nofTravrs"].ToString();
                        string nofStaffs = dsselectquery.Tables[0].Rows[i1]["nofStaffs"].ToString();
                        string nofstudents = dsselectquery.Tables[0].Rows[i1]["nofstudents"].ToString();
                        string Engine_No = dsselectquery.Tables[0].Rows[i1]["Engine_No"].ToString();
                        string Manufactura_date = dsselectquery.Tables[0].Rows[i1]["Manufactura_date"].ToString();
                        string Dealer_Details = dsselectquery.Tables[0].Rows[i1]["Dealer_Details"].ToString();
                        string RTOAdd1 = dsselectquery.Tables[0].Rows[i1]["RTOAdd1"].ToString();
                        string RTOAdd2 = dsselectquery.Tables[0].Rows[i1]["RTOAdd2"].ToString();
                        string RTOCity = dsselectquery.Tables[0].Rows[i1]["RTOCity"].ToString();
                        string RTOState = dsselectquery.Tables[0].Rows[i1]["RTOState"].ToString();
                        string RTOPin = dsselectquery.Tables[0].Rows[i1]["RTOPin"].ToString();
                        string RTOContPer = dsselectquery.Tables[0].Rows[i1]["RTOContPer"].ToString();
                        string RTOContPhno = dsselectquery.Tables[0].Rows[i1]["RTOContPhno"].ToString();
                        string Km_Day = dsselectquery.Tables[0].Rows[i1]["Mileage"].ToString();
                        string cheese_no = dsselectquery.Tables[0].Rows[i1]["Cheese_No"].ToString();
                        string gender = dsselectquery.Tables[0].Rows[i1]["gendertype"].ToString();
                        if (gender.Trim() == "" || gender == null)
                            gender = "0";
                        ddlgender.SelectedValue = gender;
                        //27nov2013==========================================================================
                        chk_college.Checked = false;
                        for (int colcnt = 0; colcnt <= chklst_college.Items.Count - 1; colcnt++)
                        {
                            chklst_college.Items[colcnt].Selected = false;
                            chklst_college_SelectedIndexChanged(sender, e);
                        }

                        if (Convert.ToString(dsselectquery.Tables[0].Rows[i1]["college_code"]) != "")
                        {
                            string[] college = Convert.ToString(dsselectquery.Tables[0].Rows[i1]["college_code"]).Split(new char[] { ',' });
                            if (college.GetUpperBound(0) >= 0)
                            {

                                for (int cnt = 0; cnt <= college.GetUpperBound(0); cnt++)
                                {
                                    for (int colcnt = 0; colcnt <= chklst_college.Items.Count - 1; colcnt++)
                                    {
                                        if (Convert.ToString(college[cnt]) == Convert.ToString(chklst_college.Items[colcnt].Value))
                                        {
                                            chklst_college.Items[colcnt].Selected = true;
                                            chklst_college_SelectedIndexChanged(sender, e);
                                            goto l1;
                                        }
                                    }
                                l1: int a = cnt;
                                }
                            }
                        }
                        //===================================================================================
                        try
                        {
                            //if (Convert.ToString(dsselectquery.Tables[0].Rows[0]["type"].ToString()) == "0")  EXISTING
                            if (Convert.ToString(dsselectquery.Tables[0].Rows[0]["type"].ToString()) == "1")  //MODIFIED by prabha jan 29 2018
                            {

                                rbnew.Checked = false;
                                rbold.Checked = true;
                                tbnoowner.Enabled = false;
                                tbnoowner.Text = "";
                            }
                            else
                            {
                                rbold.Checked = false;
                                rbnew.Checked = true;
                                tbnoowner.Enabled = true;
                            }
                        }
                        catch
                        {

                        }

                        try
                        {
                            if (veh_id != "")
                            {
                                tbvehiid.Text = veh_id;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (Veh_Type != "")
                            {
                                //sankar edit
                                ddlvehicletype.SelectedItem.Text = Veh_Type;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (Reg_No != "")
                            {
                                tbregno.Text = Reg_No;
                            }
                        }
                        catch
                        {

                        }

                        try
                        {
                            if (Reg_Date != "")
                            {

                                DateTime ddd_apply = Convert.ToDateTime(Reg_Date);
                                tbregdate.Text = ddd_apply.ToString("dd-MM-yyyy");
                            }
                            else
                            {

                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (RC_No != "")
                            {
                                tbrcno.Text = RC_No;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (Numberofowner != "")
                            {
                                tbnoowner.Text = Numberofowner;
                            }
                        }
                        catch
                        {

                        }

                        try
                        {
                            if (Km_Day != "")
                            {
                                txtkm.Text = Km_Day;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (VehicleCast != "")
                            {
                                tbvehiclecast.Text = VehicleCast;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (Tax != "")
                            {
                                tbtax.Text = Tax;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (TotalPur_Amount != "")
                            {
                                tbtotalpuramount.Text = TotalPur_Amount;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (Vehicle_Ins != "")
                            {
                                tbinsurance.Text = Vehicle_Ins;
                            }

                        }
                        catch
                        {

                        }
                        try
                        {
                            if (Purpose_Vehicle != "")
                            {
                                ddlvehiclepur.SelectedValue = Purpose_Vehicle;
                            }
                        }
                        catch
                        {

                        }


                        try
                        {
                            if (Purchased_On != "")
                            {
                                //tbpuron.Text = Purchased_On;
                                DateTime ddd_apply1 = Convert.ToDateTime(Purchased_On);
                                tbpuron.Text = ddd_apply1.ToString("dd-MM-yyyy");
                            }
                            else
                            {

                            }

                        }
                        catch
                        {

                        }
                        try
                        {
                            if (Place_Reg != "")
                            {
                                tbplacereg.Text = Place_Reg;
                            }
                        }
                        catch
                        {

                        }

                        try
                        {
                            if (Duration != "")
                            {
                                tbduration.Text = Duration;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (TotalNo_Seat != "")
                            {
                                tbseatcapacity.Text = TotalNo_Seat;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (Extra_No != "")
                            {
                                tbmaxallowed.Text = Extra_No;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (Intial_Km != "")
                            {
                                tbintial.Text = Intial_Km;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (Renew_Date != "")
                            {
                                DateTime ddd_renewdate = Convert.ToDateTime(Renew_Date);
                                tbrenewdate.Text = ddd_renewdate.ToString("dd-MM-yyyy");
                                //tbrenewdate.Text = Renew_Date;

                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (nofTravrs != "")
                            {
                                tbtotaltravel.Text = nofTravrs;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (nofStaffs != "")
                            {
                                tbstaff.Text = nofStaffs;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (nofstudents != "")
                            {
                                tbstudent.Text = nofstudents;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (Engine_No != "")
                            {
                                tbenginno.Text = Engine_No;
                            }
                        }
                        catch
                        {

                        }

                        txt_cheese.Text = cheese_no;

                        try
                        {
                            if (Manufactura_date != "")
                            {
                                DateTime ddd_manudate = Convert.ToDateTime(Manufactura_date);
                                tbmanudate.Text = ddd_manudate.ToString("dd-MM-yyyy");

                                //tbmanudate.Text = Manufactura_date;
                            }
                            else
                            {
                                tbmanudate.Text = "";
                            }
                        }
                        catch
                        {

                        }


                        try
                        {
                            if (Convert.ToInt16(dsselectquery.Tables[0].Rows[0]["Purchased_From"].ToString()) == 0)
                            {

                                rbpruindu.Checked = true;
                                rbdealer.Checked = false;
                                ddldealerdetails.Enabled = false;
                                if (ddldealerdetails.Items.Count > 0)
                                    ddldealerdetails.SelectedIndex = 0;
                            }
                            else if (Convert.ToInt16(dsselectquery.Tables[0].Rows[0]["Purchased_From"].ToString()) == 1)
                            {

                                rbdealer.Checked = true;
                                rbpruindu.Checked = false;
                                ddldealerdetails.Enabled = true;
                            }
                        }
                        catch
                        {
                        }

                        try
                        {
                            if (Dealer_Details != "")
                            {
                                ddldealerdetails.SelectedValue = Dealer_Details;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (RTOAdd1 != "")
                            {
                                tbaddress1.Text = RTOAdd1;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (RTOAdd2 != "")
                            {
                                tbaddress2.Text = RTOAdd2;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (RTOCity != "")
                            {
                                tbcityrto.Text = RTOCity;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (RTOState != "")
                            {
                                ddlstaterto.SelectedValue = RTOState;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (RTOPin != "")
                            {
                                tbpincoderto.Text = RTOPin;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (RTOContPer != "")
                            {
                                tbrtocontact.Text = RTOContPer;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (RTOContPhno != "")
                            {
                                tbcontactnumber.Text = RTOContPhno;
                            }
                        }
                        catch
                        {

                        }

                    }
                }
            }


        }

    }
    protected void btncertifiFC_Click(object sender, EventArgs e)
    {

    }
    protected void LoadMainEnquiry_date()
    {
        sprdMainEnquiry.Sheets[0].RowCount = 0;
        sprdMainEnquiry.SaveChanges();
        DAccess2 da = new DAccess2();
        DataSet ds = new DataSet();


        ht.Clear();
        ht.Add("Veh_Type", ddlvehicletype.SelectedItem.Text.ToString());
        ht.Add("Veh_ID", tbvehiid.Text.Trim());

        ds = da.select_method("vehicle_load", ht, "sp");
        sprdMainEnquiry.Sheets[0].RowCount = ds.Tables[0].Rows.Count;
        sprdMainEnquiry.Sheets[0].PageSize = ds.Tables[0].Rows.Count;

        if (ds.Tables[0].Rows.Count > 0)
        {
            //sprdMainEnquiry.Visible = true;
            lblerrordate.Visible = false;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                FarPoint.Web.Spread.TextCellType tb = new FarPoint.Web.Spread.TextCellType();
                sprdMainEnquiry.Sheets[0].Cells[i, 0].CellType = tb;

                string purcharon = ds.Tables[0].Rows[i]["Purchased_On"].ToString();
                if (purcharon.Trim() != "" && purcharon != null)
                {
                    string[] soiltdate = purcharon.Split(' ');
                    string[] spitdate = soiltdate[0].Split('/');
                    if (spitdate.GetUpperBound(0) >= 2)
                    {
                        purcharon = spitdate[1] + '/' + spitdate[0] + '/' + spitdate[2];
                    }
                }
                sprdMainEnquiry.Sheets[0].Cells[i, 0].Text = ds.Tables[0].Rows[i]["Veh_Type"].ToString();
                sprdMainEnquiry.Sheets[0].Cells[i, 1].Text = ds.Tables[0].Rows[i]["Veh_ID"].ToString();
                sprdMainEnquiry.Sheets[0].Cells[i, 2].Text = ds.Tables[0].Rows[i]["Reg_No"].ToString();
                sprdMainEnquiry.Sheets[0].Cells[i, 3].Text = ds.Tables[0].Rows[i]["Reg_Date"].ToString();
                sprdMainEnquiry.Sheets[0].Cells[i, 4].Text = ds.Tables[0].Rows[i]["RC_No"].ToString();
                sprdMainEnquiry.Sheets[0].Cells[i, 5].Text = ds.Tables[0].Rows[i]["Type"].ToString();
                sprdMainEnquiry.Sheets[0].Cells[i, 6].Text = ds.Tables[0].Rows[i]["Numberofowner"].ToString();
                sprdMainEnquiry.Sheets[0].Cells[i, 7].Text = purcharon;
                sprdMainEnquiry.Sheets[0].Cells[i, 8].Text = ds.Tables[0].Rows[i]["Place_Reg"].ToString();
                sprdMainEnquiry.Sheets[0].Cells[i, 9].Text = ds.Tables[0].Rows[i]["Duration"].ToString();

            }
        }
        else
        {
            sprdMainEnquiry.Visible = false;
            lblerrordate.Text = "No Record(s) Found";
            lblerrordate.Visible = true;
        }


    }
    protected void btncertifiFC1_Click(object sender, EventArgs e)
    {


        if (ddlvehicletype.SelectedItem.Text != "")
        {
            lblinserrorcer.Visible = false;

            string treepath = selectedpath;

            int actrow1 = 0;
            int actcol1 = 0;
            string sch_dt = "";
            string degree_code = "";
            string semester = "";
            string subject_no = "";
            string batchyear = "";
            actrow1 = sprdmainFC.ActiveSheetView.ActiveRow;
            actcol1 = sprdmainFC.ActiveSheetView.ActiveColumn;
            sch_dt = sprdmainFC.Sheets[0].RowHeader.Cells[0, 0].Text;
            if (FileUpload4.FileName.EndsWith(".jpg") || FileUpload4.FileName.EndsWith(".gif") || FileUpload4.FileName.EndsWith(".png") || FileUpload4.FileName.EndsWith(".txt") || FileUpload4.FileName.EndsWith(".doc") || FileUpload4.FileName.EndsWith(".xls") || FileUpload4.FileName.EndsWith(".docx") || FileUpload4.FileName.EndsWith(".txt") || FileUpload4.FileName.EndsWith(".document") || FileUpload4.FileName.EndsWith(".xls") || FileUpload4.FileName.EndsWith(".xlsx") || FileUpload4.FileName.EndsWith(".pdf"))
            {

                String filePath = Server.MapPath(@"~/Doc/" + FileUpload4.FileName);
                FileUpload4.SaveAs(filePath);

                string path = "../Doc/";
                path = path + FileUpload4.FileName;

                string fileName = Path.GetFileName(FileUpload4.PostedFile.FileName);

                string fileExtension = Path.GetExtension(FileUpload4.PostedFile.FileName);
                string documentType = string.Empty;
                switch (fileExtension)
                {

                    case ".pdf":

                        documentType = "application/pdf";
                        // documentType = ".pdf";

                        break;

                    case ".xls":

                        documentType = "application/vnd.ms-excel";
                        // documentType = ".xls";

                        break;

                    case ".xlsx":

                        documentType = "application/vnd.ms-excel";
                        // documentType = "application/vnd.ms-excel";

                        break;

                    case ".doc":

                        documentType = "application/vnd.ms-word";
                        // documentType = ".doc";

                        break;

                    case ".docx":

                        documentType = "application/vnd.ms-word";
                        // documentType = ".doc";

                        break;

                    case ".gif":

                        documentType = "image/gif";
                        //documentType = ".gif";

                        break;

                    case ".png":

                        documentType = "image/png";

                        break;

                    case ".jpg":

                        documentType = "image/jpg";

                        break;

                }

                int fileSize = FileUpload4.PostedFile.ContentLength;
                byte[] documentBinary = new byte[fileSize];
                FileUpload4.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                sprdmainFC.SaveChanges();
                if (sprdmainFC.Sheets[0].RowCount > 0)
                {
                    for (int exp = 0; exp < sprdmainFC.Sheets[0].RowCount; exp++)
                    {
                        if (sprdmainFC.Sheets[0].Cells[exp, 0].Text != "")
                        {
                            if (sprdmainFC.Sheets[0].Cells[exp, 4].Text == "")
                            {
                                lblinserrorcer.Visible = false;

                                string FC_No = sprdmainFC.Sheets[0].Cells[exp, 0].Text.ToString();
                                string FC_Date = sprdmainFC.Sheets[0].Cells[exp, 1].Text.ToString();
                                string FC_Amount = sprdmainFC.Sheets[0].Cells[exp, 2].Text.ToString();
                                string NextFcdate = sprdmainFC.Sheets[0].Cells[exp, 5].Text.ToString();
                                string FC_remarks = sprdmainFC.Sheets[0].Cells[exp, 6].Text.ToString();


                                SqlParameter DocName = new SqlParameter("@DocName2", SqlDbType.VarChar, 50);
                                DocName.Value = fileName.ToString();


                                SqlParameter Type = new SqlParameter("@Type2", SqlDbType.VarChar, 50);
                                Type.Value = documentType.ToString();
                                SqlParameter uploadedDocument = new SqlParameter("@DocData2", SqlDbType.Binary, fileSize);
                                uploadedDocument.Value = documentBinary;
                                hastab.Clear();
                                hastab.Add("Veh_ID", tbvehiid.Text);
                                hastab.Add("Veh_Type", ddlvehicletype.SelectedItem.Text.ToString());
                                //hastab.Add("FC_No", FC_No);
                                //hastab.Add("FC_Date", FC_Date);
                                //hastab.Add("FC_Amount", FC_Amount);
                                hastab.Add("FCcertifi_filename", DocName.Value);
                                hastab.Add("FCcertificate_filedate", uploadedDocument.Value);
                                hastab.Add("FCcertifi_filetype", Type.Value);
                                //hastab.Add("NextFcdate", NextFcdate);
                                //hastab.Add("FC_remarks", FC_remarks);

                                if (chk1 == 0)
                                {
                                    int n = dset.insert_method("FC_Insert", hastab, "sp");
                                }
                                else
                                {
                                    int n = dset.insert_method("FC_Update", hastab, "sp");
                                }


                                retrivespreadfornotes1();
                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Vehicle FC Details Saved Successfully')", true);
                            }
                        }
                    }
                }
            }

            else
            {
                lblinserrorcer.Visible = true;
                lblinserrorcer.Text = "Selected file format is Not allowed";

            }


        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Enter VehicleType')", true);
        }
    }
    protected void btncertifiInsurance1_Click(object sender, EventArgs e)
    {
        if (ddlvehicletype.SelectedItem.Text != "")
        {
            lblerror.Visible = false;
            string treepath = selectedpath;

            int actrow1 = 0;
            int actcol1 = 0;
            string sch_dt = "";
            string degree_code = "";
            string semester = "";
            string subject_no = "";
            string batchyear = "";
            actrow1 = sprdMaininsurance.ActiveSheetView.ActiveRow;
            actcol1 = sprdMaininsurance.ActiveSheetView.ActiveColumn;
            sch_dt = sprdMaininsurance.Sheets[0].RowHeader.Cells[0, 0].Text;
            if (FileUpload3.FileName.EndsWith(".jpg") || FileUpload3.FileName.EndsWith(".gif") || FileUpload3.FileName.EndsWith(".png") || FileUpload3.FileName.EndsWith(".txt") || FileUpload3.FileName.EndsWith(".doc") || FileUpload3.FileName.EndsWith(".xls") || FileUpload3.FileName.EndsWith(".docx") || FileUpload3.FileName.EndsWith(".txt") || FileUpload3.FileName.EndsWith(".document") || FileUpload3.FileName.EndsWith(".xls") || FileUpload3.FileName.EndsWith(".xlsx") || FileUpload3.FileName.EndsWith(".pdf"))
            {


                String filePath = Server.MapPath(@"~/Doc/" + FileUpload3.FileName);
                FileUpload3.SaveAs(filePath);

                string path = "../Doc/";
                path = path + FileUpload3.FileName;



                string fileName = Path.GetFileName(FileUpload3.PostedFile.FileName);

                string fileExtension = Path.GetExtension(FileUpload3.PostedFile.FileName);
                string documentType = string.Empty;
                switch (fileExtension)
                {

                    case ".pdf":

                        documentType = "application/pdf";
                        // documentType = ".pdf";

                        break;

                    case ".xls":

                        documentType = "application/vnd.ms-excel";
                        // documentType = ".xls";

                        break;

                    case ".xlsx":

                        documentType = "application/vnd.ms-excel";
                        // documentType = "application/vnd.ms-excel";

                        break;

                    case ".doc":

                        documentType = "application/vnd.ms-word";
                        // documentType = ".doc";

                        break;

                    case ".docx":

                        documentType = "application/vnd.ms-word";
                        // documentType = ".doc";

                        break;

                    case ".gif":

                        documentType = "image/gif";
                        //documentType = ".gif";

                        break;

                    case ".png":

                        documentType = "image/png";

                        break;

                    case ".jpg":

                        documentType = "image/jpg";

                        break;

                }

                int fileSize = FileUpload3.PostedFile.ContentLength;
                byte[] documentBinary = new byte[fileSize];
                FileUpload3.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                sprdMaininsurance.SaveChanges();
                if (sprdMaininsurance.Sheets[0].RowCount > 0)
                {
                    for (int exp = 0; exp < sprdMaininsurance.Sheets[0].RowCount; exp++)
                    {
                        if (sprdMaininsurance.Sheets[0].Cells[exp, 0].Text != "")
                        {
                            if (sprdMaininsurance.Sheets[0].Cells[exp, 5].Text == "")
                            {
                                lblerror.Visible = false;
                                string Insu_No = sprdMaininsurance.Sheets[0].Cells[exp, 0].Text.ToString();
                                string Insurance_Date = sprdMaininsurance.Sheets[0].Cells[exp, 1].Text.ToString();
                                string Amt_Insured = sprdMaininsurance.Sheets[0].Cells[exp, 2].Text.ToString();
                                string Insu_Amount = sprdMaininsurance.Sheets[0].Cells[exp, 3].Text.ToString();
                                string nextins_date = sprdMaininsurance.Sheets[0].Cells[exp, 6].Text.ToString();
                                string ins_remarks = sprdMaininsurance.Sheets[0].Cells[exp, 7].Text.ToString();

                                SqlParameter DocName = new SqlParameter("@DocName1", SqlDbType.VarChar, 50);
                                DocName.Value = fileName.ToString();
                                //cmd.Parameters.Add(DocName);

                                SqlParameter Type = new SqlParameter("@Type1", SqlDbType.VarChar, 50);
                                Type.Value = documentType.ToString();

                                SqlParameter uploadedDocument = new SqlParameter("@DocData1", SqlDbType.Binary, fileSize);
                                uploadedDocument.Value = documentBinary;

                                hastab.Clear();
                                hastab.Add("Veh_ID", tbvehiid.Text);
                                hastab.Add("Veh_Type", ddlvehicletype.SelectedItem.Text.ToString());
                                //hastab.Add("Insu_No", Insu_No);
                                //hastab.Add("Insurance_Date", Insurance_Date);
                                //hastab.Add("Amt_Insured", Amt_Insured);
                                //hastab.Add("Insu_Amount", Insu_Amount);
                                hastab.Add("InsCerificat_Filename", DocName.Value);
                                hastab.Add("InsCertificat_filedate", uploadedDocument.Value);
                                hastab.Add("InsCertificat_filetype", Type.Value);
                                //hastab.Add("nextins_date", nextins_date);
                                //hastab.Add("ins_remarks", ins_remarks);


                                if (chk1 == 0)
                                {
                                    int n = dset.insert_method("Insurance_Insert", hastab, "sp");
                                }
                                else
                                {
                                    int n = dset.insert_method("Insurance_Update", hastab, "sp");
                                }





                                retrivespreadfornotes();
                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Vehicle Insurance Details Saved Successfully')", true);
                            }
                        }
                    }
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
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Enter VehicleType')", true);
        }

    }
    protected void btncertifiInsurance_Click(object sender, EventArgs e)
    {


    }
    public void retrivespreadfornotes()
    {
        sprdMaininsurance.SaveChanges();
        if (sprdMaininsurance.Sheets[0].RowCount > 0)
        {
            for (int exp = 0; exp < sprdMaininsurance.Sheets[0].RowCount; exp++)
            {
                if (sprdMaininsurance.Sheets[0].Cells[exp, 0].Text != "")
                {
                    if (sprdMaininsurance.Sheets[0].Cells[exp, 5].Text == "")
                    {
                        string path;
                        string fileName = Path.GetFileName(FileUpload3.PostedFile.FileName);
                        path = fileName.ToString();
                        sprdMaininsurance.Sheets[0].Cells[exp, 5].Text = fileName;
                        sprdMaininsurance.Sheets[0].Cells[exp, 5].ForeColor = Color.Black;
                        //sprdMaininsurance.Sheets[0].Cells[exp, 5].BackColor = Color.Blue;
                        sprdMaininsurance.Sheets[0].Cells[exp, 5].Font.Underline = true;


                    }
                }
            }


        }
        else
        {
            sprdMaininsurance.Sheets[0].RowCount = 0;
        }
    }
    protected void retrivespreadfornotes1()
    {
        sprdmainFC.SaveChanges();
        if (sprdmainFC.Sheets[0].RowCount > 0)
        {
            for (int exp = 0; exp < sprdmainFC.Sheets[0].RowCount; exp++)
            {
                if (sprdmainFC.Sheets[0].Cells[exp, 0].Text != "")
                {

                    if (sprdmainFC.Sheets[0].Cells[exp, 4].Text == "")
                    {
                        string path;
                        string fileName = Path.GetFileName(FileUpload4.PostedFile.FileName);
                        path = fileName.ToString();
                        sprdmainFC.Sheets[0].Cells[exp, 4].Text = fileName;
                        sprdmainFC.Sheets[0].Cells[exp, 4].ForeColor = Color.Black;
                        //sprdmainFC.Sheets[0].Cells[exp, 4].BackColor = Color.Blue;
                        sprdmainFC.Sheets[0].Cells[exp, 4].Font.Underline = true;

                    }
                }
            }


        }
        else
        {
            sprdmainFC.Sheets[0].RowCount = 0;
        }
    }
    protected void UploadButtonvehicle_Click(object sender, EventArgs e)
    {

    }
    protected void UploadButtonvback_Click(object sender, EventArgs e)
    {

    }
    protected void UploadButtonsideleft_Click(object sender, EventArgs e)
    {

    }
    protected void UploadButtonvright_Click(object sender, EventArgs e)
    {

    }
    protected void UploadButtonvother1_Click(object sender, EventArgs e)
    {

    }
    protected void UploadButtonother2_Click(object sender, EventArgs e)
    {

    }
    protected void Buttonsavevehiclephoto_Click(object sender, EventArgs e)
    {

    }
    protected void btndeletevehiclephoto_Click(object sender, EventArgs e)
    {

    }
    protected void ButtonExitvphoto_Click(object sender, EventArgs e)
    {

    }
    protected void UploadButtonfsign_Click(object sender, EventArgs e)
    {

    }
    protected void UploadButtonmsign_Click(object sender, EventArgs e)
    {

    }
    protected void UploadButtongsign_Click(object sender, EventArgs e)
    {

    }
    protected void UploadButtonv1sign_Click(object sender, EventArgs e)
    {

    }
    protected void sprdMaininsurance_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

    }
    protected void sprdmainFC_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        //string activerow = sprdmainFC.ActiveSheetView.ActiveRow.ToString();
        //string activecol = sprdmainFC.ActiveSheetView.ActiveColumn.ToString();
        Cellclick4 = true;

        bool x = sprdmainFC.Sheets[0].AutoPostBack;
    }
    protected void sprdmainFCprerender(object sender, EventArgs e)
    {
        if (Cellclick4 == true)
        {
            if (check_FcRow == 0)
            {
                bool x = sprdmainFC.Sheets[0].AutoPostBack;
                sprdmainFC.Enabled = true;
                string activerow = "";
                string activecol = "";
                activerow = sprdmainFC.ActiveSheetView.ActiveRow.ToString();
                activecol = sprdmainFC.ActiveSheetView.ActiveColumn.ToString();
                try
                {
                    if (Convert.ToInt32(activecol) >= 4)
                    {
                        for (int exp = 0; exp < sprdmainFC.Sheets[0].RowCount; exp++)
                        {
                            if (sprdmainFC.Sheets[0].Cells[exp, 0].Text != "")
                            {

                                string fileName = string.Empty;
                                //string fileid = sprdMaininsurance.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Tag + "@" + sprdMaininsurance.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text + "@" + sprdMaininsurance.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Tag + "@" + sprdMaininsurance.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text;
                                //string fileid = sprdMaininsurance.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Tag.ToString();
                                path1 = sprdmainFC.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;

                                SqlCommand cmd = new SqlCommand("SELECT FCcertifi_filename,FCcertificate_filedate,FCcertifi_filetype FROM Vehicle_Insurance WHERE Veh_ID='" + tbvehiid.Text + "' and FCcertifi_filename='" + path1 + "'", con);// and degree_code="++", con);
                                con.Close();
                                con.Open();
                                SqlDataReader dReader = cmd.ExecuteReader();
                                while (dReader.Read())
                                {

                                    //fileName = dReader["FCcertifi_filename"].ToString();
                                    //byte[] documentBinary = (byte[])dReader["FCcertificate_filedate"];


                                    check_FcRow = 0;

                                    Response.Clear();
                                    Response.Buffer = true;
                                    Response.ContentType = dReader["FCcertifi_filetype"].ToString();
                                    Response.AddHeader("content-disposition", "attachment;filename=" + dReader["FCcertifi_filename"].ToString());     // to open file prompt Box open or Save file         
                                    Response.Charset = "";
                                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                    Response.BinaryWrite((byte[])dReader["FCcertificate_filedate"]);
                                    Response.End();
                                    //--------create a folder 20/6/12 PRABHA

                                    //string activeDir = string.Empty;

                                    //activeDir = Server.MapPath("Docs");

                                    //if (!Directory.Exists(Server.MapPath("Docs")))
                                    //{
                                    //    Directory.CreateDirectory(Server.MapPath("docs"));
                                    //}
                                    //else
                                    //{
                                    //    Directory.Delete(Server.MapPath("Docs"), true);
                                    //    Directory.CreateDirectory(Server.MapPath("docs"));
                                    //}
                                    ////  ---------------------------------------
                                    //FileStream fStream = new FileStream(Server.MapPath("Docs") + @"\" + fileName, FileMode.Create);
                                    //fStream.Write(documentBinary, 0, documentBinary.Length);
                                    //fStream.Close();
                                    //fStream.Dispose();


                                    //Response.Redirect(@"Docs\" + fileName);

                                    //myHyperLink mycell = new myHyperLink((FarPoint.Web.Spread.Model.DefaultSheetDataModel)sprdmainFC.Sheets[0].DataModel);
                                    //sprdmainFC.Sheets[0].Cells[Convert.ToInt32(0), 5].CellType = mycell;
                                    //cellclick3 = false;

                                }
                            }
                        }





                    }



                }


                catch
                {

                }
            }
        }
        //}
    }
    protected void sprdMaininsurance_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        //string activerow = sprdMaininsurance.ActiveSheetView.ActiveRow.ToString();
        //string activecol = sprdMaininsurance.ActiveSheetView.ActiveColumn.ToString();
        Cellclick3 = true;

        bool x = sprdMaininsurance.Sheets[0].AutoPostBack;
    }
    protected void sprdMaininsuranceprerender(object sender, EventArgs e)
    {
        bool x = sprdMaininsurance.Sheets[0].AutoPostBack;
        if (Cellclick3 == true)//added by rajasekar 10/09/2018
        {
            if (check_addrow == 0)
            {
                sprdMaininsurance.Enabled = true;
                string activerow = "";
                string activecol = "";
                activerow = sprdMaininsurance.ActiveSheetView.ActiveRow.ToString();
                activecol = sprdMaininsurance.ActiveSheetView.ActiveColumn.ToString();
                try
                {
                    //sprdMaininsurance.Sheets[0].RowCount = 0;
                    if (Convert.ToInt32(activecol) >= 5)
                    {
                        for (int exp = 0; exp < sprdMaininsurance.Sheets[0].RowCount; exp++)
                        {
                            if (sprdMaininsurance.Sheets[0].Cells[exp, 0].Text != "")
                            {
                                string fileName = string.Empty;
                                path1 = sprdMaininsurance.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text;
                                SqlCommand cmd = new SqlCommand("SELECT InsCerificat_Filename,InsCertificat_filedate,InsCertificat_filetype FROM Vehicle_Insurance WHERE Veh_ID='" + tbvehiid.Text + "' and InsCerificat_Filename='" + path1 + "'", con);// and degree_code="++", con);
                                con.Close();
                                con.Open();
                                SqlDataReader dReader = cmd.ExecuteReader();
                                while (dReader.Read())
                                {

                                    //fileName = dReader["InsCerificat_Filename"].ToString();
                                    //byte[] documentBinary = (byte[])dReader["InsCertificat_filedate"];


                                    check_addrow = 0;

                                    Response.Clear();
                                    Response.Buffer = true;
                                    Response.ContentType = dReader["InsCertificat_filetype"].ToString();
                                    Response.AddHeader("content-disposition", "attachment;filename=" + dReader["InsCerificat_Filename"].ToString());     // to open file prompt Box open or Save file         
                                    Response.Charset = "";
                                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                    Response.BinaryWrite((byte[])dReader["InsCertificat_filedate"]);
                                    Response.End();




                                    //--------create a folder 20/6/12 PRABHA

                                    //string activeDir = string.Empty;

                                    //activeDir = Server.MapPath("Docs");

                                    //if (!Directory.Exists(Server.MapPath("Docs")))
                                    //{
                                    //    Directory.CreateDirectory(Server.MapPath("docs"));
                                    //}
                                    //else
                                    //{
                                    //    Directory.Delete(Server.MapPath("Docs"), true);
                                    //    Directory.CreateDirectory(Server.MapPath("docs"));
                                    //}
                                    ////  ---------------------------------------
                                    //FileStream fStream = new FileStream(Server.MapPath("Docs") + @"\" + fileName, FileMode.Create);
                                    //fStream.Write(documentBinary, 0, documentBinary.Length);
                                    //fStream.Close();
                                    //fStream.Dispose();


                                    //Response.Redirect(@"Docs\" + fileName);

                                    //myHyperLink mycell = new myHyperLink((FarPoint.Web.Spread.Model.DefaultSheetDataModel)sprdMaininsurance.Sheets[0].DataModel);
                                    //sprdMaininsurance.Sheets[0].Cells[Convert.ToInt32(0), 5].CellType = mycell;
                                    //cellclick4 = false;
                                }
                            }

                        }

                    }
                }
                catch(Exception ex)
                {

                }
            }
        }
    }
    public void clear()
    {
        txt_cheese.Text = "";
        tbvehiid.Text = "";
        tbregno.Text = "";
        tbregdate.Text = "";
        tbrcno.Text = "";
        txtkm.Text = "";
        rbnew.Checked = true;
        tbnoowner.Text = "";
        tbvehiclecast.Text = "";
        tbtax.Text = "";
        tbinsurance.Text = "";
        tbtotalpuramount.Text = "";
        tbpuron.Text = "";
        tbplacereg.Text = "";
        tbduration.Text = "";
        tbseatcapacity.Text = "";
        tbmaxallowed.Text = "";
        tbintial.Text = "";
        tbrenewdate.Text = "";
        tbtotaltravel.Text = "";
        tbstudent.Text = "";
        tbstaff.Text = "";
        tbenginno.Text = "";
        tbmanudate.Text = "";
        rbpruindu.Checked = true;
        tbaddress1.Text = "";
        tbaddress2.Text = "";
        tbcityrto.Text = "";
        tbpincoderto.Text = "";
        tbcontactnumber.Text = "";
        tbrtocontact.Text = "";
        if (ddlvehicletype.Items.Count > 0)
            ddlvehicletype.SelectedIndex = 0;
        if (ddlvehiclepur.Items.Count > 0)
            ddlvehiclepur.SelectedIndex = 0;
        if (ddldealerdetails.Items.Count > 0)
            ddldealerdetails.SelectedIndex = 0;
        if (ddlstaterto.Items.Count > 0)
            ddlstaterto.SelectedIndex = 0;
        FpSpread1.Sheets[0].RowCount = 0;
        sprdMaininsurance.Sheets[0].RowCount = 0;
        sprdmainFC.Sheets[0].RowCount = 0;
        //ImageRegCer.Visible = false;
        //sprdmainFC.Sheets[0].RowCount = 0;
        //sprdmainFC.SaveChanges();
        //sprdMaininsurance.Sheets[0].RowCount = 0;
        //sprdMaininsurance.SaveChanges();

        ImageRegCer.Visible = false;
        ImgBackPhoto.Visible = false;
        imgFrontPhoto.Visible = false;
        imgleftphoto.Visible = false;
        imgrightphoto.Visible = false;
        imgother1photo.Visible = false;
        imgother2photo.Visible = false;
        imgother3photo.Visible = false;
        imgother4photo.Visible = false;


    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        clear();
        Buttonsave.Text = "Save";
        btnsave.Text = "Save";
        btnsave1.Text = "Save";
        tbvehiid.Enabled = true;
        ddlvehicletype.Enabled = true;
        Buttondelete.Enabled = false;
        lbladdview.Text = "Add";
        ddlvehicletype.SelectedItem.Text = "";
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        if (sprdMainEnquiry.Sheets[0].RowCount > 0)
        {
            Session["column_header_row_count"] = 1;
            string degreedetails = "Vehicle Details";
            string pagename = "Transport_New.aspx";
            Printcontrol.loadspreaddetails(sprdMainEnquiry, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
    }
    protected void chk_college_ChekedChanged(object sender, EventArgs e)
    {

        int count = 0;
        selected_college = "";

        if (chk_college.Checked == true)
        {
            for (int i = 0; i < chklst_college.Items.Count; i++)
            {
                count++;
                chklst_college.Items[i].Selected = true;
                txt_college.Text = "College(" + count.ToString() + ")";
                if (selected_college == "")
                {
                    selected_college = chklst_college.Items[i].Value.ToString();
                }
                else
                {
                    selected_college = selected_college + "," + chklst_college.Items[i].Value.ToString();
                }

            }


        }
        else if (chk_college.Checked == false)
        {
            txt_college.Text = "";
            for (int i = 0; i < chklst_college.Items.Count; i++)
            {

                chklst_college.Items[i].Selected = false;
            }
        }




    }
    protected void chklst_college_SelectedIndexChanged(object sender, EventArgs e)
    {

        selected_college = "";
        int count = 0;

        for (int i = 0; i < chklst_college.Items.Count; i++)
        {
            if (chklst_college.Items[i].Selected == true)
            {
                count++;
                chklst_college.Items[i].Selected = true;
                txt_college.Text = "College(" + count.ToString() + ")";
                if (selected_college == "")
                {
                    selected_college = chklst_college.Items[i].Value.ToString();

                }
                else
                {
                    selected_college = selected_college + "," + chklst_college.Items[i].Value.ToString();

                }
            }
        }


    }
    public void load_college()
    {
        DataSet dss = new DataSet();
        Hashtable hat = new Hashtable();
        string grouporusercode = "";
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
        }
        else
        {
            grouporusercode = " and user_code=" + Session["usercode"].ToString().Trim() + "";
        }
        hat.Add("column_field", grouporusercode);
        dss = DataAccess.select_method("bind_college", hat, "sp");
        if (dss.Tables[0].Rows.Count > 0)
        {

            chklst_college.DataSource = dss;
            chklst_college.DataTextField = "collname";
            chklst_college.DataValueField = "college_code";
            chklst_college.DataBind();
        }
    }
    protected void tbtax_TextChanged(object sender, EventArgs e)
    {
        sumvechilecost();
    }
    protected void tbinsurance_TextChanged(object sender, EventArgs e)
    {
        sumvechilecost();
    }
    protected void tbvehiclecast_TextChanged(object sender, EventArgs e)
    {
        sumvechilecost();
    }
    public void sumvechilecost()
    {
        try
        {
            Double vechilecost = 0, tax = 0, insurancecoset = 0;
            if (tbtax.Text.Trim() != "")
            {
                tax = Convert.ToDouble(tbtax.Text.ToString());
            }
            if (tbinsurance.Text.Trim() != "")
            {
                insurancecoset = Convert.ToDouble(tbinsurance.Text.ToString());
            }
            if (tbvehiclecast.Text.Trim() != "")
            {
                vechilecost = Convert.ToDouble(tbvehiclecast.Text.ToString());
            }
            vechilecost = vechilecost + tax + insurancecoset;
            tbtotalpuramount.Text = vechilecost.ToString();

        }
        catch (Exception ex)
        {
        }
    }
}


   