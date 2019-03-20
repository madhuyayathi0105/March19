using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Web.Services;
using System.Drawing;
using Gios.Pdf;
using System.IO;
public partial class inv_messbill_cost_sheet : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet ds5 = new DataSet();
    DateTime dt = new DateTime();
    DateTime dt1 = new DateTime();
    Hashtable perheadamt_hash = new Hashtable();
    string dtaccessdate = DateTime.Now.ToString();
    string dtaccesstime = DateTime.Now.ToLongTimeString();
    int i = 0;
    DAccess2 d2 = new DAccess2();
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
        if (!IsPostBack)
        {
            bindmess();
            int year = Convert.ToInt32(System.DateTime.Now.ToString("yyyy"));
            for (int l = 0; l < 15; l++)
            {
                ddl_year.Items.Add(Convert.ToString(year));
                year--;
            }
            //ddl_year.Items.Insert(0, "Select");
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread1.Visible = false;
            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnCount = 0;
            Fpspread2.Visible = false;
            btn_go_Click(sender, e);
        }
    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch
        {
        }
    }
    //mess month
    protected void cb_month_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            int i = 0;
            txt_month.Text = "--Select--";
            if (cb_month.Checked == true)
            {
                cout++;
                for (i = 0; i < cbl_month.Items.Count; i++)
                {
                    cbl_month.Items[i].Selected = true;
                }
                txt_month.Text = "Month(" + cbl_month.Items.Count + ")";
            }
            else
            {
                for (i = 0; i < cbl_month.Items.Count; i++)
                {
                    cbl_month.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_month_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cb_month.Checked = false;
            int commcount = 0;
            txt_month.Text = "--Select--";
            for (i = 0; i < cbl_month.Items.Count; i++)
            {
                if (cbl_month.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_month.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_month.Items.Count)
                {
                    cb_month.Checked = true;
                }
                txt_month.Text = "Month(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    //mess name event
    protected void cb_messname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            int i = 0;
            txt_messname.Text = "--Select--";
            if (cb_messname.Checked == true)
            {
                cout++;
                for (i = 0; i < cbl_messname.Items.Count; i++)
                {
                    cbl_messname.Items[i].Selected = true;
                }
                txt_messname.Text = "Mess Name(" + cbl_messname.Items.Count + ")";
            }
            else
            {
                for (i = 0; i < cbl_messname.Items.Count; i++)
                {
                    cbl_messname.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_messname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cb_messname.Checked = false;
            int commcount = 0;
            txt_messname.Text = "--Select--";
            for (i = 0; i < cbl_messname.Items.Count; i++)
            {
                if (cbl_messname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_messname.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_messname.Items.Count)
                {
                    cb_messname.Checked = true;
                }
                txt_messname.Text = "Mess Name(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    //bind mess
    public void bindmess()
    {
        try
        {
            cbl_messname.Items.Clear();
            //ds = d2.Bindmess_inv(collegecode1);
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_messname.DataSource = ds;
                cbl_messname.DataTextField = "MessName";
                cbl_messname.DataValueField = "MessMasterPK";
                cbl_messname.DataBind();
                if (cbl_messname.Items.Count > 0)
                {
                    for (i = 0; i < cbl_messname.Items.Count; i++)
                    {
                        cbl_messname.Items[i].Selected = true;
                    }
                    txt_messname.Text = "Mess Name(" + cbl_messname.Items.Count + ")";
                    cb_messname.Checked = true;
                }
            }
            else
            {
                txt_messname.Text = "--Select--";
                cb_messname.Checked = false;
            }
        }
        catch
        {
        }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_messname.Text.Trim() != "--Select--" && ddl_year.SelectedItem.Text.Trim() != "Select")//&& txt_month.Text.Trim() != "--Select--"
            {
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = false;
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.Sheets[0].ColumnCount = 5;
                Fpspread1.Width = 670;
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
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Year";
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[1].Width = 100;
                Fpspread1.Columns[1].Locked = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Month";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[2].Width = 100;
                Fpspread1.Columns[2].Locked = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Mess Name";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[3].Width = 200;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Report";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[4].Width = 200;
                FarPoint.Web.Spread.ButtonCellType btnType = new FarPoint.Web.Spread.ButtonCellType();
                int row = 1;
                for (int i = 1; i <= 12; i++)
                {
                    for (int j = 0; j < cbl_messname.Items.Count; j++)
                    {
                        Fpspread1.Sheets[0].RowCount++;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row++);
                        //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(i);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ddl_year.SelectedItem.Text);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = returnMonYear(Convert.ToString(i));
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(i);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(cbl_messname.Items[j].Text);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(cbl_messname.Items[j].Value);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].CellType = btnType;
                        btnType.Text = "View Report";
                        btnType.CssClass = "textbox btn4";
                        btnType.ForeColor = Color.Blue;
                    }
                    Fpspread1.Sheets[0].RowCount++;
                }
                Fpspread1.Visible = true;
                spreaddiv.Visible = true;
                lbl_error.Visible = false;
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpspread1.Columns[1].VerticalAlign = VerticalAlign.Middle;
                Fpspread1.Columns[2].VerticalAlign = VerticalAlign.Middle;
            }
            else
            {
                Fpspread1.Visible = false;
                spreaddiv.Visible = false;
                Fpspread2.Visible = false;
                reportdiv.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select All Fields";
            }
        }
        catch
        { }
    }
    public string returnMonYear(string numeral)
    {
        string monthyear = String.Empty;
        switch (numeral)
        {
            case "1":
                monthyear = "Jan";
                break;
            case "2":
                monthyear = "Feb";
                break;
            case "3":
                monthyear = "Mar";
                break;
            case "4":
                monthyear = "Apr";
                break;
            case "5":
                monthyear = "May";
                break;
            case "6":
                monthyear = "Jun";
                break;
            case "7":
                monthyear = "Jul";
                break;
            case "8":
                monthyear = "Aug";
                break;
            case "9":
                monthyear = "Sep";
                break;
            case "10":
                monthyear = "Oct";
                break;
            case "11":
                monthyear = "Nov";
                break;
            case "12":
                monthyear = "Dec";
                break;
        }
        return monthyear;
    }
    public void btnType_Click(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            Fpspread1.SaveChanges();
            string year = "";
            string month = "";
            string messcode = "";
            string actrow = e.SheetView.ActiveRow.ToString();
            string actcol = e.SheetView.ActiveColumn.ToString();
            string fromdate = "";
            string todate = "";
            double Vegstudentstrength = 0;
            double Nonvegstudentstrength = 0;
            double VegMandays = 0;
            double NonvegMandays = 0;
            double VegPerheadAmt = 0;
            double NonvegPerheadAmt = 0;
            string studentstrength = "";
            string noofdays = "";
            string messamtperhead = "";
            double perheadexpamt = 0;
            double expamt = 0;
            string mmonth = "";
            string myear = "";
            string txtdate = "";
            string txtmonth = "";
            string txtyear = "";
            string txttodate = "";
            string txttomonth = "";
            string txttoyear = "";
            DateTime dbfromdate1 = new DateTime();
            DateTime dbtodate1 = new DateTime();
            string dbtodate = "";
            string dbfromdate = "";
            if (actrow.Trim() != "" && actcol.Trim() != "")
            {
                year = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 1].Text);
                month = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Tag);
                messcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Tag);
                string hostelcode = d2.Gethostelcode_inv(messcode);
                string hostelmessdate = "Hostel Mess Date";
                string dategetquery = " select * from HT_MessBillMaster where  messmonth='" + month + "' and messyear='" + year + "' and messmasterfk='" + messcode + "'";
                dategetquery = dategetquery + " select LinkValue  from InsSettings where LinkName='" + hostelmessdate + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(dategetquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    mmonth = Convert.ToString(ds.Tables[0].Rows[0]["messmonth"]);
                    myear = Convert.ToString(ds.Tables[0].Rows[0]["messyear"]);
                }
                else
                {
                    lbl_error1.Visible = true;
                    lbl_error1.Text = "First Generate Mess Bill Calculation";
                    Fpspread2.Visible = false;
                    reportdiv.Visible = false;
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    string lnkvalue1 = Convert.ToString(ds.Tables[1].Rows[0][0].ToString());
                    string[] lnkvalue = lnkvalue1.Split('-');
                    fromdate = Convert.ToString(lnkvalue[0]);
                    todate = Convert.ToString(lnkvalue[1]);
                }
                if (fromdate.Trim() != "" && todate.Trim() != "")
                {
                    char[] delimiterChars = { '/' };
                    string[] lnkvalue = fromdate.Split(delimiterChars);
                    string[] todate1 = todate.Split(delimiterChars);
                    txtdate = Convert.ToString(lnkvalue[0]);
                    txtmonth = Convert.ToString(lnkvalue[1]);
                    txtyear = Convert.ToString(lnkvalue[2]);
                    txttodate = Convert.ToString(todate1[0]);
                    txttomonth = Convert.ToString(todate1[1]);
                    txttoyear = Convert.ToString(todate1[2]);
                    lbl_error1.Visible = false;
                }
                else
                {
                    lbl_error1.Visible = true;
                    lbl_error1.Text = "First Generate Mess Bill Calculation";
                    Fpspread2.Visible = false;
                    reportdiv.Visible = false;
                }
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    string d = "";
                    d = Convert.ToString("/");
                    dbfromdate = month + d + txtdate + d + txtyear;
                    dbtodate = month + d + txttodate + d + txttoyear;
                    dbfromdate1 = Convert.ToDateTime(dbfromdate);
                    dbtodate1 = Convert.ToDateTime(dbtodate);
                    //pdf using
                    ViewState["month"] = month; ViewState["year"] = year;
                    //string costquery = " SELECT SUM(ConsumptionQty*RPU )as Consumption_Value,subheader_code ,i.itemheadername+'-'+t.MasterValue as headersubheader,i.itemheadername,i.ItemName  FROM HT_DailyConsumptionMaster  m,HT_DailyConsumptionDetail  D,IM_ItemMaster I,CO_MasterValues t where m.DailyConsumptionMasterPK  = d.DailyConsumptionMasterFK AND D.ItemFK  = I.ItemPK  and t.MasterCode =i.subheader_code AND DailyConsDate >=  '" + dbfromdate1.ToString("MM/dd/yyyy") + "'  AND DailyConsDate < '" + dbtodate1.ToString("MM/dd/yyyy") + "' and ForMess <>'2'  group by subheader_code ,t.MasterValue ,i.itemheadername ,i.ItemName order by i.ItemHeaderName,t.MasterValue";
                    string costquery = "  SELECT cast(SUM(ConsumptionQty*RPU ) as decimal(10,2)) as Consumption_Value,subheader_code ,i.itemheadername+'-'+t.MasterValue + (case when d.Menutype=1 then ' (Nonveg)' when d.Menutype=0 then ' (Veg)' end) as headersubheader,i.itemheadername,i.ItemName +' ('+i.ItemUnit+') ' as ItemName,d.Menutype,SUM(ConsumptionQty)ConsumptionQty  FROM HT_DailyConsumptionMaster  m,HT_DailyConsumptionDetail  D,IM_ItemMaster I,CO_MasterValues t where m.DailyConsumptionMasterPK  = d.DailyConsumptionMasterFK AND D.ItemFK  = I.ItemPK  and t.MasterCode =i.subheader_code AND DailyConsDate between  '" + dbfromdate1.ToString("MM/dd/yyyy") + "'  AND '" + dbtodate1.ToString("MM/dd/yyyy") + "' and ForMess <>'2' and d.menutype is not  null and m.deptfk in('" + messcode + "') group by subheader_code ,t.MasterValue ,i.itemheadername ,i.ItemName,d.Menutype,i.ItemUnit order by i.ItemHeaderName,t.MasterValue";
                    //magesh 30.5.18
                    //costquery += " SELECT cast(SUM(ConsumptionQty*RPU) as decimal(10,2)) as Consumption_Value,d.menutype,subheader_code ,t.MasterValue,i.itemheadername+(case when d.Menutype=1 then ' (Nonveg)' when d.Menutype=0 then ' (Veg)' end)itemheadername FROM HT_DailyConsumptionMaster  m,HT_DailyConsumptionDetail  D,IM_ItemMaster I,CO_MasterValues t where m.DailyConsumptionMasterPK  = d.DailyConsumptionMasterFK AND D.ItemFK  = I.ItemPK  and t.MasterCode =i.subheader_code AND DailyConsDate between '" + dbfromdate1.ToString("MM/dd/yyyy") + "' AND '" + dbtodate1.ToString("MM/dd/yyyy") + "' and ForMess <>'2' and m.deptfk in('" + messcode + "') and d.menutype is not  null group by d.menutype,subheader_code ,t.MasterValue ,i.itemheadername";
                    costquery += " SELECT SUM(ConsumptionQty*RPU) as Consumption_Value,d.menutype,subheader_code ,t.MasterValue,i.itemheadername+(case when d.Menutype=1 then ' (Nonveg)' when d.Menutype=0 then ' (Veg)' end)itemheadername FROM HT_DailyConsumptionMaster  m,HT_DailyConsumptionDetail  D,IM_ItemMaster I,CO_MasterValues t where m.DailyConsumptionMasterPK  = d.DailyConsumptionMasterFK AND D.ItemFK  = I.ItemPK  and t.MasterCode =i.subheader_code AND DailyConsDate between '" + dbfromdate1.ToString("MM/dd/yyyy") + "' AND '" + dbtodate1.ToString("MM/dd/yyyy") + "' and ForMess <>'2' and m.deptfk in('" + messcode + "') and d.menutype is not  null group by d.menutype,subheader_code ,t.MasterValue ,i.itemheadername";
                    costquery += " select SUM(IncomeAmount)as Inc_Amount,MasterValue,IncomeDesc from HT_HostelIncome hi,CO_MasterValues co where hi.IncomeGroup=co.MasterCode and MasterCriteria='HostelIncomeGRP' and collegecode='" + collegecode1 + "' and IncomeDate between '" + dbfromdate1.ToString("MM/dd/yyyy") + "' and '" + dbtodate1.ToString("MM/dd/yyyy") + "' and HostelMasterFK in('" + hostelcode + "')  group by MasterValue,IncomeDesc";
                    costquery += " select SUM(ex.ExpAmount)as Exp_Amount, MasterValue,ExpDesc,MasterCode,ExpensesType  from HT_HostelExpenses ex,CO_MasterValues co where ex.ExpGroup=co.MasterCode and MasterCriteria='hostelexpgrp' and ExpensesDate between '" + dbfromdate1.ToString("MM/dd/yyyy") + "' and '" + dbtodate1.ToString("MM/dd/yyyy") + "' and Messname  in('" + messcode + "') group by ExpDesc,MasterValue,ExpDesc,MasterCode,ExpensesType order by ExpensesType";//HostelFK in('" + hostelcode + "')  magesh 4.7.18
                    costquery += " select  ms.No_Of_Days,ms.incGroupCode,ms.mess_amount, ms.MessBill_Year,ms.MessBill_Month,ms.Hostel_Code,ms.mandays,ms.StudStrength,ms.Per_Day_Amount,ms.MessType from HT_MessBillMaster m,HT_MessBillDetail d,HMessbill_StudDetails ms where m.MessBillMasterPK =d.MessBillMasterFK  and ms.ExpGroupCode=m.GroupCode and ms.Hostel_Code in('" + messcode + "') and ms.MessBill_Month in('" + month + "') and ms.MessBill_Year in('" + year + "') and ms.memtype=d.memtype and ms.memtype='1' group by No_Of_Days,incGroupCode,mess_amount, MessBill_Year,MessBill_Month, ms.Hostel_Code,mandays,StudStrength,ms.Per_Day_Amount,ms.MessType";//ms.ExpGroupCode,d.GroupAmount, ExpGroupCode,GroupAmount,
                    costquery += " select SUM(ex.expamount)As exp_amount,mastercode,mastervalue,ExpensesType from HT_HostelExpenses ex,co_mastervalues co where  ex.expgroup=co.mastercode and expensesdate between '" + dbfromdate1.ToString("MM/dd/yyyy") + "' and '" + dbtodate1.ToString("MM/dd/yyyy") + "'  and Messname  in('" + messcode + "') group by mastercode,mastervalue,ExpensesType order by ExpensesType";
                    ds1.Clear();
                    ds1 = d2.select_method_wo_parameter(costquery, "Text");
                    Session["selectedmonthdataset"] = ds1;
                    int i = 0;
                      if(ds1.Tables.Count>0)//  if (ds1.Tables[0].Rows.Count > 0)
                    {
                        
                        Fpspread2.Sheets[0].RowCount = 0;
                        Fpspread2.Sheets[0].ColumnCount = 0;
                        Fpspread2.CommandBar.Visible = false;
                        Fpspread2.Sheets[0].AutoPostBack = false;
                        Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspread2.Sheets[0].RowHeader.Visible = false;
                        Fpspread2.Sheets[0].ColumnCount = 5;
                        Fpspread2.Sheets[0].ColumnHeader.Visible = false;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 5);
                        string clgname = d2.GetFunction("select cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code and cp.college_code='" + collegecode1 + "'");
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = clgname;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Larger;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.Green;
                        Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 0, 1, 5);
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 0, 1, 5);
                        //Fpspread2.Sheets[0].RowCount++;
                        //Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 3);
                        string m = returnMonYear(month);
                        string messhead = "Mess Bill for the Month of  " + m + "-" + year;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = messhead;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.Gray;
                        Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 0, 1, 5);
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 0, 1, 5);
                        Fpspread2.Sheets[0].RowCount++;
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "S.No";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            Fpspread2.Columns[0].Width = 50;
                            Fpspread2.Columns[0].Locked = true;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Item Header - Sub Header";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                            Fpspread2.Columns[1].Width = 200;
                            Fpspread2.Columns[1].Locked = true;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = "Item Name";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                            Fpspread2.Columns[2].Locked = true;
                            Fpspread2.Columns[2].Width = 200;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = "Quantity";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                            Fpspread2.Columns[3].Width = 100;
                            Fpspread2.Columns[3].Locked = true;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = "Amount Rs";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                            Fpspread2.Columns[4].Width = 100;
                            Fpspread2.Columns[4].Locked = true;
                            ////Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 2, 1, 2);
                            i = 0; string headername = ""; double totalconsumption = 0;
                            foreach (DataRow dr in ds1.Tables[0].Rows)
                            {
                                if (Convert.ToString(dr["itemheadername"]) != headername && headername != "")
                                {
                                    Fpspread2.Sheets[0].RowCount++;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = "Total";
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkViolet;
                                    Fpspread2.Columns[2].Width = 200;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(totalconsumption);
                                    totalconsumption = 0;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].ForeColor = Color.DarkViolet;
                                }
                                double totalconsump = 0;
                                double.TryParse(Convert.ToString(dr["Consumption_Value"]), out totalconsump);
                                totalconsumption += totalconsump;
                                headername = Convert.ToString(dr["itemheadername"]);
                                Fpspread2.Sheets[0].RowCount++; i++;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dr["headersubheader"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                Fpspread2.Columns[2].Width = 200;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dr["itemname"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dr["ConsumptionQty"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                //Fpspread2.Columns[4].Width = 200;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dr["Consumption_Value"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            }
                            Fpspread2.Columns[2].Locked = true;
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = "Total";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkViolet;
                            Fpspread2.Columns[2].Width = 200;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(totalconsumption);
                            totalconsumption = 0;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].ForeColor = Color.DarkViolet;
                            //Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 2);
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Particulars of Expenditure";
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                            //Fpspread2.Columns[1].Width = 200;
                            //Fpspread2.Columns[1].Locked = true;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "S.No";
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            //Fpspread2.Columns[0].Width = 50;
                            //Fpspread2.Columns[0].Locked = true;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = "Amount Rs";
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                            //Fpspread2.Columns[3].Width = 200;
                            //Fpspread2.Columns[3].Locked = true;
                            //Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 2);
                            //for (i = 0; i < ds1.Tables[0].Rows.Count; i++)
                            //{
                            //    Fpspread2.Sheets[0].RowCount++;
                            //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Total Consumption Value";
                            //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            //    Fpspread2.Columns[2].Width = 200;
                            //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds1.Tables[0].Rows[i]["consume_total"]);
                            //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                            //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            //}
                            //Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 2);
                        }
                        //dailyconsumtion 
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 3);
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "S.No";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread2.Columns[0].Width = 50;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Item Header Name";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        Fpspread2.Columns[1].Width = 200;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = "Sub Group Name";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        Fpspread2.Columns[2].Width = 100;
                        //Fpspread2.Columns[2].Visible = false;
                        //Fpspread2.Sheets[0].ColumnCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = "Amount Rs";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                        Fpspread2.Columns[3].Width = 100;
                        Fpspread2.Columns[3].Locked = true;
                        for (i = 0; i < ds1.Tables[1].Rows.Count; i++)
                        {
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds1.Tables[1].Rows[i]["itemheadername"]);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds1.Tables[1].Rows[i]["MasterValue"]);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds1.Tables[1].Rows[i]["Consumption_Value"]);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        }
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = "Total Expenditure Amount";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkViolet;
                          //magesh 4.7.18
                        double consumtotal = 0.00;
                        if (ds1.Tables[1].Rows.Count>0)
                         consumtotal = Convert.ToDouble(ds1.Tables[1].Compute("Sum(Consumption_Value)", ""));//65336281
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(consumtotal);
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds1.Tables[5].Rows[0]["Total_Consumption_Value"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].ForeColor = Color.DarkViolet;
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Total Veg Expenditure Amount";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                        Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 2);
                        double vegtotal = 0;
                        double Nonvegtotal = 0;
                        if (ds1.Tables[1].Rows.Count > 0)
                        {
                            double.TryParse(Convert.ToString(ds1.Tables[1].Compute("Sum(Consumption_Value)", "menutype='0'")), out vegtotal);//29.1267
                            double.TryParse(Convert.ToString(ds1.Tables[1].Compute("Sum(Consumption_Value)", "menutype='1'")), out Nonvegtotal);//8.5776
                        }
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(vegtotal);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].ForeColor = Color.Chocolate;
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Total NonVeg Expenditure Amount";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                        Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 2);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(Nonvegtotal);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].ForeColor = Color.Chocolate;
                        //income 
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 3);
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "S.No";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        Fpspread2.Columns[0].Width = 50;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Group Name";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        Fpspread2.Columns[1].Width = 200;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = "Description";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        Fpspread2.Columns[2].Width = 200;
                        //Fpspread2.Sheets[0].ColumnCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = "Amount Rs";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                        for (i = 0; i < ds1.Tables[2].Rows.Count; i++)
                        {
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds1.Tables[2].Rows[i]["MasterValue"]);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds1.Tables[2].Rows[i]["IncomeDesc"]);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds1.Tables[2].Rows[i]["Inc_Amount"]);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        }
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = "Total Deductions Amount";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkViolet;
                        //double totalincome = Convert.ToDouble(ds1.Tables[2].Compute("Sum(Inc_Amount)", ""));
                        double totalincome = 0;
                        string totalinc = Convert.ToString(ds1.Tables[2].Compute("Sum(Inc_Amount)", ""));
                        if (totalinc.Trim() == "")
                        {
                            totalincome = 0;
                        }
                        else
                        {
                            totalincome = Convert.ToDouble(totalinc);
                        }
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(totalincome);
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds1.Tables[6].Rows[0]["Total_Inc_Amount"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].ForeColor = Color.DarkViolet;
                        // Net Expenditure
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 3);
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = "Net Expenditure";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkViolet;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(consumtotal - totalincome);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].ForeColor = Color.DarkViolet;
                        #region expanses
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 3);
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "S.No";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Group Name";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = "Description";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        //Fpspread2.Sheets[0].ColumnCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = "Amount Rs";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                        //double amt = 0;
                        //string text1 = "";
                        //for (i = 0; i < ds1.Tables[3].Rows.Count; i++)
                        //{
                        //string text = Convert.ToString(ds1.Tables[3].Rows[i]["MasterValue"]);
                        //if (text1.Trim() == "")
                        //{
                        //    text1 = text;
                        //}
                        //if (text1 == text)
                        //{
                        //    amt += Convert.ToDouble(ds1.Tables[3].Rows[i]["Exp_Amount"]);
                        //}
                        //else
                        //{
                        //    Fpspread2.Sheets[0].RowCount++;
                        //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = "Total Group Name";
                        //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                        //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(amt);
                        //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                        //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        //    text1 = "";
                        //    amt = 0;
                        //    amt += Convert.ToDouble(ds1.Tables[3].Rows[i]["Exp_Amount"]);
                        //}
                        if (ds1.Tables[3].Rows.Count > 0)
                        {
                            string textcodecomma = string.Empty;
                            if (ds1.Tables[5].Rows.Count > 0)
                            {
                                int row = 0;
                                for (i = 0; i < ds1.Tables[5].Rows.Count; i++)
                                {
                                    string textcode = Convert.ToString(ds1.Tables[5].Rows[i]["MasterCode"]);
                                    DataView dv = new DataView();
                                    ds1.Tables[3].DefaultView.RowFilter = "MasterCode='" + textcode + "'";
                                    dv = ds1.Tables[3].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        for (int r = 0; r < dv.Count; r++)
                                        {
                                            row++;
                                            Fpspread2.Sheets[0].RowCount++;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row);
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv[r]["MasterValue"]);
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[r]["ExpDesc"]);
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[r]["Exp_Amount"]);
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                        }
                                        /*22.12.17 barath
                                        Fpspread2.Sheets[0].RowCount++;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = "Group Total Amount";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkViolet;
                                        DataTable dtcheck = new DataTable();
                                        dtcheck = dv.ToTable();
                                        expamt = Convert.ToDouble(dtcheck.Compute("Sum(Exp_Amount)", ""));
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(expamt);
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].ForeColor = Color.DarkViolet;
                                        Fpspread2.Sheets[0].RowCount++;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = "Per head Amount";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkViolet;
                                        double ExpancesType = 0;
                                        double.TryParse(Convert.ToString(dv[0]["ExpensesType"]), out ExpancesType);
                                        //Vegstudentstrength);
                                        //Nonvegstudentstrength
                                        double Strength = ExpancesType == 1 ? Vegstudentstrength : Nonvegstudentstrength;
                                        double VegperheadExpamt = Convert.ToDouble(expamt / Strength);
                                        if (!perheadamt_hash.ContainsKey(textcode + "-" + ExpancesType))
                                            perheadamt_hash.Add(textcode + "-" + ExpancesType, Convert.ToString(Math.Round(VegperheadExpamt, 2)));
                                        //double NonvegPerheadexpamt = Convert.ToDouble(expamt / Nonvegstudentstrength);
                                        //perheadamt_hash.Add(textcode, Convert.ToString(Math.Round(NonvegPerheadexpamt, 2)));
                                        //perheadexpamt = Convert.ToDouble(expamt / Convert.ToDouble(studentstrength));
                                        //string perdayamt = Convert.ToString(Math.Round(perheadexpamt, 2));
                                        //perheadamt_hash.Add(textcode, perdayamt);
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(Math.Round(VegperheadExpamt, 2));//perheadexpamt
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].ForeColor = Color.DarkViolet;
                                         */
                                    }
                                }
                            }
                        }
                        #endregion
                        if (ds1.Tables[3].Rows.Count > 0)
                        {
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = "Veg Expanses";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkViolet;
                            double vegExpance = 0;
                            double NonvegExpance = 0;
                            double CommonExpance = 0;
                            double.TryParse(Convert.ToString(ds1.Tables[3].Compute("Sum(Exp_Amount)", "Expensestype='1'")), out vegExpance);//29.1267
                            double.TryParse(Convert.ToString(ds1.Tables[3].Compute("Sum(Exp_Amount)", "Expensestype='2'")), out NonvegExpance);//8.5776
                            double.TryParse(Convert.ToString(ds1.Tables[3].Compute("Sum(Exp_Amount)", "Expensestype='0'")), out CommonExpance);//8.5776
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(vegExpance);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].ForeColor = Color.Chocolate;
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = "Non Veg Expanses";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkViolet;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(NonvegExpance);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].ForeColor = Color.Chocolate;
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = "Common Expanses";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkViolet;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(CommonExpance);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].ForeColor = Color.Chocolate;

                        }
                        #region PerdayCalcalation
                        //noof student                        
                        Fpspread2.Sheets[0].RowCount++;
                        //Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 3);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = "Veg";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.Chocolate;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = "Non Veg";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].ForeColor = Color.Chocolate;
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "No Of Days ";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkViolet;
                        if (ds1.Tables[4].Rows.Count > 0)
                        {
                            // MessType
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds1.Tables[4].Compute("Sum(No_Of_Days)", "MessType='0'"));
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds1.Tables[4].Compute("Sum(No_Of_Days)", "MessType='1'"));// Convert.ToString(ds1.Tables[4].Rows[i]["No_Of_Days"]);
                            noofdays = Convert.ToString(ds1.Tables[4].Compute("Sum(No_Of_Days)", "MessType='1'")); //Convert.ToString(ds1.Tables[4].Rows[i]["No_Of_Days"]);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        }
                        //student strength    
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Student Strength ";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkViolet;
                        if (ds1.Tables[4].Rows.Count > 0)
                        {
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds1.Tables[4].Compute("Sum(StudStrength)", "MessType='0'"));
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds1.Tables[4].Compute("Sum(StudStrength)", "MessType='1'"));
                            double.TryParse(Convert.ToString(ds1.Tables[4].Compute("Sum(StudStrength)", "MessType='0'")), out Vegstudentstrength);
                            double.TryParse(Convert.ToString(ds1.Tables[4].Compute("Sum(StudStrength)", "MessType='1'")), out Nonvegstudentstrength);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        }
                          //total days 
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Total Days ";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        DataView dvmesstype = new DataView();
                        ds1.Tables[4].DefaultView.RowFilter = "MessType='0'";
                        dvmesstype = ds1.Tables[4].DefaultView;
                        double stustrength = 0.0;
                        double sumofdays = 0.0;
                        if (dvmesstype.Count > 0)
                        {

                            stustrength = Convert.ToDouble(ds1.Tables[4].Compute("Sum(StudStrength)", "MessType='0'"));
                            sumofdays = Convert.ToDouble(ds1.Tables[4].Compute("Sum(No_Of_Days)", "MessType='0'"));
                        }
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text =Convert.ToString(stustrength * sumofdays);
                           ds1.Tables[4].DefaultView.RowFilter = "MessType='1'";
                        dvmesstype = ds1.Tables[4].DefaultView;
                        stustrength = 0.0;
                         sumofdays = 0.0;
                        if (dvmesstype.Count > 0)
                        {
                            stustrength = Convert.ToDouble(ds1.Tables[4].Compute("Sum(StudStrength)", "MessType='1'"));
                            sumofdays = Convert.ToDouble(ds1.Tables[4].Compute("Sum(No_Of_Days)", "MessType='1'"));
                            
                        }
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(stustrength * sumofdays);
                         Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                         Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                         Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                         Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                         Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                         Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                         //rebeate days
                         if (ds1.Tables[4].Rows.Count > 0)
                         {
                             Fpspread2.Sheets[0].RowCount++;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Total Rebate Days ";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                             double manday = Convert.ToDouble(Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 2, 2].Text);

                               ds1.Tables[4].DefaultView.RowFilter = "MessType='0'";
                        dvmesstype = ds1.Tables[4].DefaultView;
                        double mandayofdays = 0.0;
                        if (dvmesstype.Count > 0)
                        {
                            mandayofdays = Convert.ToDouble(ds1.Tables[4].Compute("Sum(ManDays)", "MessType='0'"));
                        }
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(manday - mandayofdays);
                             manday = Convert.ToDouble(Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 2, 3].Text);
                                 ds1.Tables[4].DefaultView.RowFilter = "MessType='1'";
                        dvmesstype = ds1.Tables[4].DefaultView;
                         mandayofdays = 0.0;
                        if (dvmesstype.Count > 0)
                        {
                            mandayofdays = Convert.ToDouble(ds1.Tables[4].Compute("Sum(ManDays)", "MessType='1'"));
                        }
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(manday - mandayofdays);
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                         }
                        //mandays
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "No Of Manual Days ";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkViolet;
                        if (ds1.Tables[4].Rows.Count > 0)
                        {


                               ds1.Tables[4].DefaultView.RowFilter = "MessType='0'";
                        dvmesstype = ds1.Tables[4].DefaultView;
                        string mandaysnew = string.Empty;
                        if (dvmesstype.Count > 0)
                        {

                             mandaysnew = Convert.ToString(ds1.Tables[4].Compute("Sum(ManDays)", "MessType='0'"));
                           
                        }
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = mandaysnew;
                               ds1.Tables[4].DefaultView.RowFilter = "MessType='1'";
                        dvmesstype = ds1.Tables[4].DefaultView;
                        mandaysnew = string.Empty;
                        if (dvmesstype.Count > 0)
                        {
                            double.TryParse(Convert.ToString(ds1.Tables[4].Compute("Sum(ManDays)", "MessType='1'")), out NonvegMandays);
                             mandaysnew = Convert.ToString(ds1.Tables[4].Compute("Sum(ManDays)", "MessType='1'"));
                           
                        }
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = mandaysnew;
                            
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            // Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].ForeColor = Color.DarkViolet;


                          

                        }
                        //perday amount
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Per day expenditure ";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkViolet;
                        if (ds1.Tables[4].Rows.Count > 0)
                        {
                                 ds1.Tables[4].DefaultView.RowFilter = "MessType='0'";
                        dvmesstype = ds1.Tables[4].DefaultView;
                        if (dvmesstype.Count > 0)
                        {
                            double.TryParse(Convert.ToString(ds1.Tables[4].Compute("Sum(Per_Day_Amount)", "MessType='0'")), out VegPerheadAmt);
                        }//29.1267
                                ds1.Tables[4].DefaultView.RowFilter = "MessType='1'";
                        dvmesstype = ds1.Tables[4].DefaultView;
                        if (dvmesstype.Count > 0)
                        {
                            double.TryParse(Convert.ToString(ds1.Tables[4].Compute("Sum(Per_Day_Amount)", "MessType='1'")), out NonvegPerheadAmt);//8.5776
                        }
                            //VegPerheadAmt = Math.Round(VegPerheadAmt, 2, MidpointRounding.AwayFromZero);
                            //magesh 30.5.18
                           // Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(Math.Round(VegPerheadAmt, 2));
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(VegPerheadAmt); 
                            double NonvegPerdayAmt = 0;//12.02.18
                            double vegPerDayAmt = 0;
                             ds1.Tables[4].DefaultView.RowFilter = "MessType='1'";
                        dvmesstype = ds1.Tables[4].DefaultView;
                        if (dvmesstype.Count > 0)
                        {
                            double.TryParse(Convert.ToString(ds1.Tables[4].Compute("Sum(Per_Day_Amount)", "MessType='1'")), out NonvegPerdayAmt);
                        }
                             ds1.Tables[4].DefaultView.RowFilter = "MessType='0'";
                        dvmesstype = ds1.Tables[4].DefaultView;
                        if (dvmesstype.Count > 0)
                        {
                            double.TryParse(Convert.ToString(ds1.Tables[4].Compute("Sum(Per_Day_Amount)", "MessType='0'")), out vegPerDayAmt);
                        }
                            vegPerDayAmt = NonvegPerdayAmt - vegPerDayAmt;
                            //vegPerDayAmt = Math.Round(vegPerDayAmt, 2, MidpointRounding.AwayFromZero);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(Math.Round(vegPerDayAmt, 5));
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].RowCount++;
                            //magesh 30.5.18
                           // NonvegPerheadAmt = Math.Round(NonvegPerheadAmt, 2, MidpointRounding.AwayFromZero);
                           // Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(Math.Round(NonvegPerheadAmt, 2));
                            //NonvegPerdayAmt = Math.Round(NonvegPerdayAmt, 2, MidpointRounding.AwayFromZero);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(NonvegPerdayAmt);//magesh 30.5.18
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            //04.04.16
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds1.Tables[4].Rows[i]["Mess_Amount"]);
                            //messamtperhead = Convert.ToString(ds1.Tables[4].Rows[i]["Mess_Amount"]);
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds1.Tables[4].Rows[i]["Per_Day_Amount"]);
                            //messamtperhead = Convert.ToString(ds1.Tables[4].Rows[i]["Per_Day_Amount"]);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].ForeColor = Color.DarkViolet;
                        }
                        #endregion
                        #region mess bill
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Mess Bill Calculation";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 3);
                        Fpspread2.Sheets[0].RowCount++;
                        //Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 3);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = "Veg";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.Chocolate;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = "Non Veg";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].ForeColor = Color.Chocolate;
                        //Fpspread2.Sheets[0].RowCount++;
                        //Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 3);
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Mess Bill Amount";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                        double vegperDay = VegPerheadAmt * Convert.ToDouble(noofdays);
                     
                       vegperDay = Math.Round(vegperDay, 2, MidpointRounding.AwayFromZero);
                       //magesh 18.5.18
                       //vegperDay = Math.Ceiling(vegperDay);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(Math.Round(vegperDay, 2));//messamtperhead
                        //magesh 30.5.18
                       // double perDay = NonvegPerdayAmt * Convert.ToDouble(noofdays);
                        double perDay = NonvegPerheadAmt * Convert.ToDouble(noofdays);
                      
                        perDay = Math.Round(perDay, 2, MidpointRounding.AwayFromZero);
                        //magesh 18.5.18
                        //perDay = Math.Ceiling(perDay);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(Math.Round(perDay, 2));//messamtperhead
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkViolet;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].ForeColor = Color.DarkViolet;
                        //Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 2);
                        //expanse
                        if (perheadamt_hash.Count > 0)
                        {
                            foreach (DictionaryEntry parameter in perheadamt_hash)
                            {
                                string GetKey = Convert.ToString(parameter.Key);
                                string groupamt = Convert.ToString(parameter.Value);
                                string[] masterCode = GetKey.Split('-');
                                string GroupCode = string.Empty;
                                if (masterCode.Length == 2)
                                    GroupCode = Convert.ToString(masterCode[0]);
                                DataView dv = new DataView();
                                ds1.Tables[3].DefaultView.RowFilter = "MasterCode='" + GroupCode + "'";
                                dv = ds1.Tables[3].DefaultView;
                                if (dv.Count > 0)
                                {
                                    Fpspread2.Sheets[0].RowCount++;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv[0]["MasterValue"] + "-" + " Amount perhead");// "Group Amount perhead";
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                                    if (Convert.ToString(dv[0]["ExpensesType"]) == "1")
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(groupamt);
                                    else
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = "-";
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkViolet;
                                    //if (Convert.ToString(dv[0]["ExpensesType"]) == "2")
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(groupamt);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].ForeColor = Color.DarkViolet;
                                    //Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 2);
                                }
                            }
                        }
                        //total mess bill
                        double finalamount = 0; double NonvegAmt = 0;
                        /* 22.12.17 barath
                        if (perheadamt_hash.Count > 0)
                        {
                            double NonvegExpanes = 0;
                            foreach (DictionaryEntry parameter in perheadamt_hash)
                            {
                                string groupcode = Convert.ToString(parameter.Key);
                                double groupamt = Convert.ToDouble(parameter.Value);
                                string[] masterCode = groupcode.Split('-');
                                string GroupCode = string.Empty;
                                string ExpanesType = string.Empty;
                                if (masterCode.Length == 2)
                                {
                                    GroupCode = Convert.ToString(masterCode[0]);
                                    ExpanesType = Convert.ToString(masterCode[1]);
                                }
                                if (ExpanesType == "1")
                                    finalamount += groupamt;
                                else if (ExpanesType == "2")
                                    NonvegExpanes += groupamt;
                            }
                           double totalfinalamount = finalamount + (VegPerheadAmt * Convert.ToDouble(noofdays));//messamtperhead
                            NonvegAmt = finalamount + NonvegExpanes + (NonvegPerheadAmt * Convert.ToDouble(noofdays));//messamtperhead
                         */
                        double totalfinalamount = finalamount + (VegPerheadAmt * Convert.ToDouble(noofdays));//messamtperhead
                        NonvegAmt = finalamount + (NonvegPerheadAmt * Convert.ToDouble(noofdays));//messamtperhead
                        double grandtotal;
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 3);
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Total Mess Bill Amount";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkRed;
                    
                       totalfinalamount = Math.Round(totalfinalamount, 2, MidpointRounding.AwayFromZero);
                        //magesh 18.5.18
                      // totalfinalamount = Math.Ceiling(totalfinalamount);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(totalfinalamount);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkRed;
                        
                        NonvegAmt = Math.Round(NonvegAmt, 2, MidpointRounding.AwayFromZero);
                        //magesh 18.5.18
                       // NonvegAmt = Math.Ceiling(NonvegAmt);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(NonvegAmt);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].ForeColor = Color.DarkRed;
                        /*
                        Fpspread2.Sheets[0].RowCount++;
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = " Mess Bill Amount";
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Right;
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkRed;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(VegPerheadAmt * Convert.ToDouble(noofdays));
                        grandtotal = (VegPerheadAmt * Convert.ToDouble(noofdays));
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].ForeColor = Color.DarkRed;
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Grand Amount";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkRed;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(totalfinalamount);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkRed;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(grandtotal + NonvegAmt);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].ForeColor = Color.DarkRed;*/
                        #endregion
                        //}
                        Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                        Fpspread2.Visible = true;
                        reportdiv.Visible = true;
                        lbl_error1.Visible = false;
                        rptprint.Visible = true;
                    }
                    else
                    {
                        lbl_error1.Visible = true;
                        lbl_error1.Text = "No Record Found";
                        Fpspread2.Visible = false;
                        reportdiv.Visible = false;
                    }
                }
                else
                {
                    lbl_error1.Visible = true;
                    lbl_error1.Text = "First Generate Mess Bill Calculation";
                    //Fpspread2.Visible = false;
                    reportdiv.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lbl_error1.Visible = true;
            lbl_error1.Text = ex.ToString();
            reportdiv.Visible = false;
        }
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
            //string degreedetails = "Messbill Cost Sheet";
            //string pagename = "messbill_cost_sheet.aspx";
            //Printcontrol.loadspreaddetails(Fpspread2, pagename, degreedetails);
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4); //InCentimeters(60, 40)
            System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
            System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
            System.Drawing.Font Fontbold16 = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
            System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
            System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
            System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
            System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
            System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
            System.Drawing.Font Fontsmall1bold = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
            Gios.Pdf.PdfPage mypdfpage;
            PdfTextArea collinfo1;
            mypdfpage = mydoc.NewPage();
            Gios.Pdf.PdfPage mypage;
            mypage = mydoc.NewPage();
            Fpspread1.SaveChanges();
            string year = "";
            string month = "";
            string messcode = "";
            string fromdate = "";
            string todate = "";
            string mmonth = "";
            string myear = "";
            string txtdate = "";
            string txtmonth = "";
            string txtyear = "";
            string txttodate = "";
            string txttomonth = "";
            string txttoyear = "";
            DateTime dbfromdate1 = new DateTime();
            DateTime dbtodate1 = new DateTime();
            string dbtodate = "";
            string dbfromdate = "";
            //if (actrow.Trim() != "" && actcol.Trim() != "")
            //{
            //    year = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 1].Text);
            //    month = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Tag);
            //    messcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Tag);
            //    string hostelcode = d2.Gethostelcode_inv(messcode);
            //    string hostelmessdate = "Hostel Mess Date";
            //    string dategetquery = " select * from HT_MessBillMaster where  messmonth='" + month + "' and messyear='" + year + "' and messmasterfk='" + messcode + "'";
            //    dategetquery = dategetquery + " select LinkValue  from InsSettings where LinkName='" + hostelmessdate + "'";
            //    ds.Clear();
            //    ds = d2.select_method_wo_parameter(dategetquery, "Text");
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        mmonth = Convert.ToString(ds.Tables[0].Rows[0]["messmonth"]);
            //        myear = Convert.ToString(ds.Tables[0].Rows[0]["messyear"]);
            //    }
            //    if (ds.Tables[1].Rows.Count > 0)
            //    {
            //        string lnkvalue1 = Convert.ToString(ds.Tables[1].Rows[0][0].ToString());
            //        string[] lnkvalue = lnkvalue1.Split('-');
            //        fromdate = Convert.ToString(lnkvalue[0]);
            //        todate = Convert.ToString(lnkvalue[1]);
            //    }
            //    else 
            //    {
            //        //First Generate Mess Bill Calculation
            //    }
            //    if (fromdate.Trim() != "" && todate.Trim() != "")
            //    {
            //        char[] delimiterChars = { '/' };
            //        string[] lnkvalue = fromdate.Split(delimiterChars);
            //        string[] todate1 = todate.Split(delimiterChars);
            //        txtdate = Convert.ToString(lnkvalue[0]);
            //        txtmonth = Convert.ToString(lnkvalue[1]);
            //        txtyear = Convert.ToString(lnkvalue[2]);
            //        txttodate = Convert.ToString(todate1[0]);
            //        txttomonth = Convert.ToString(todate1[1]);
            //        txttoyear = Convert.ToString(todate1[2]);
            //        lbl_error1.Visible = false;
            //    }
            //    else
            //    {
            //        //First Generate Mess Bill Calculation";
            //    }
            //    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            //    {
            //        string d = "";
            //        d = Convert.ToString("/");
            //        dbfromdate = month + d + txtdate + d + txtyear;
            //        dbtodate = month + d + txttodate + d + txttoyear;
            //        dbfromdate1 = Convert.ToDateTime(dbfromdate);
            //        dbtodate1 = Convert.ToDateTime(dbtodate);
            //    }
            //}
            //string clgname = d2.GetFunction("select cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code and cp.college_code='" + collegecode1 + "'");
            //PdfTextArea ptc = new PdfTextArea(Fontbold16, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 15, 595, 300), System.Drawing.ContentAlignment.TopCenter, clgname);
            //string m = returnMonYear(Convert.ToString(ViewState["month"]));
            //string messhead = "Mess Bill for the Month of  " + m + "-" + Convert.ToString(ViewState["year"]);
            //mypage.Add(ptc);
            //PdfTextArea txtarea1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 40, 595, 50), System.Drawing.ContentAlignment.TopCenter, messhead);
            //mypage.Add(txtarea1);
            #region
            int coltop = 0;
            string Collvalue = "";
            string strquery = "Select * from Collinfo where college_code=" + Session["collegecode"].ToString() + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            string collegedetails = d2.GetFunction("select college_details from tbl_print_master_settings where page_Name='Investorsposetting.aspx'");
            string[] spiltcollegedetails = collegedetails.Split('#');
            for (int i = 0; i <= spiltcollegedetails.GetUpperBound(0); i++)
            {
                coltop = coltop + 15;
                string collinfo = spiltcollegedetails[i].ToString();
                if (collinfo == "College Name")
                {
                    collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0]["collname"].ToString() + "");
                    mypage.Add(collinfo1);
                }
                else if (collinfo == "University")
                {
                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0]["university"].ToString() + "");
                    mypage.Add(collinfo1);
                }
                else if (collinfo == "Affliated By")
                {
                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0]["affliatedby"].ToString() + "");
                    mypage.Add(collinfo1);
                }
                else if (collinfo == "Address")
                {
                    string address1 = ds.Tables[0].Rows[0]["Address1"].ToString();
                    string address2 = ds.Tables[0].Rows[0]["Address2"].ToString();
                    string address3 = ds.Tables[0].Rows[0]["Address3"].ToString();
                    if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                    {
                        Collvalue = address1;
                    }
                    if (address2.Trim() != "" && address2 != null && address2.Length > 1)
                    {
                        if (Collvalue.Trim() != "" && Collvalue != null)
                        {
                            Collvalue = Collvalue + ',' + address2;
                        }
                        else
                        {
                            Collvalue = address2;
                        }
                    }
                    if (address3.Trim() != "" && address3 != null && address3.Length > 1)
                    {
                        if (Collvalue.Trim() != "" && Collvalue != null)
                        {
                            Collvalue = Collvalue + ',' + address3;
                        }
                        else
                        {
                            Collvalue = address3;
                        }
                    }
                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                    mypage.Add(collinfo1);
                }
                else if (collinfo == "City")
                {
                    string address1 = ds.Tables[0].Rows[0]["Address3"].ToString();
                    if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                    {
                        Collvalue = address1;
                    }
                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                    mypage.Add(collinfo1);
                }
                else if (collinfo == "District & State & Pincode")
                {
                    string district = ds.Tables[0].Rows[0]["district"].ToString();
                    string state = ds.Tables[0].Rows[0]["State"].ToString();
                    string pincode = ds.Tables[0].Rows[0]["Pincode"].ToString();
                    if (district.Trim() != "" && district != null && district.Length > 1)
                    {
                        Collvalue = district;
                    }
                    if (state.Trim() != "" && state != null && state.Length > 1)
                    {
                        if (Collvalue.Trim() != "" && Collvalue != null)
                        {
                            Collvalue = Collvalue + ',' + state;
                        }
                        else
                        {
                            Collvalue = state;
                        }
                    }
                    if (pincode.Trim() != "" && pincode != null && pincode.Length > 1)
                    {
                        if (Collvalue.Trim() != "" && Collvalue != null)
                        {
                            Collvalue = Collvalue + '-' + pincode;
                        }
                        else
                        {
                            Collvalue = pincode;
                        }
                    }
                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                    mypage.Add(collinfo1);
                }
                else if (collinfo == "Phone No & Fax")
                {
                    string phone = ds.Tables[0].Rows[0]["Phoneno"].ToString();
                    string fax = ds.Tables[0].Rows[0]["Faxno"].ToString();
                    if (phone.Trim() != "" && phone != null && phone.Length > 1)
                    {
                        Collvalue = "Phone :" + phone;
                    }
                    if (fax.Trim() != "" && fax != null && fax.Length > 1)
                    {
                        if (Collvalue.Trim() != "" && Collvalue != null)
                        {
                            Collvalue = Collvalue + " , Fax : " + fax;
                        }
                        else
                        {
                            Collvalue = "Fax :" + fax;
                        }
                    }
                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                    mypage.Add(collinfo1);
                }
                else if (collinfo == "Email & Web Site")
                {
                    string email = ds.Tables[0].Rows[0]["Email"].ToString();
                    string website = ds.Tables[0].Rows[0]["Website"].ToString();
                    if (email.Trim() != "" && email != null && email.Length > 1)
                    {
                        Collvalue = "Email :" + email;
                    }
                    if (website.Trim() != "" && website != null && website.Length > 1)
                    {
                        if (Collvalue.Trim() != "" && Collvalue != null)
                        {
                            Collvalue = Collvalue + " , Web Site : " + website;
                        }
                        else
                        {
                            Collvalue = "Web Site :" + website;
                        }
                    }
                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                    mypage.Add(collinfo1);
                }
                else if (collinfo == "Left Logo")
                {
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                    {
                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                        mypage.Add(LogoImage, 25, 25, 400);
                    }
                }
                else if (collinfo == "Right Logo")
                {
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                    {
                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                        mypage.Add(LogoImage, 480, 25, 400);
                    }
                }
            }
            #endregion
            string m = returnMonYear(Convert.ToString(ViewState["month"]));
            string messhead = "Mess Bill for the Month of  " + m + "-" + Convert.ToString(ViewState["year"]);
            PdfTextArea txtarea1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, messhead);
            mypage.Add(txtarea1);
            DataSet pdfds = (DataSet)Session["selectedmonthdataset"];
            mypage.SaveToDocument();
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "MessBillCostSheet" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
                Response.End();
            }
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }
}