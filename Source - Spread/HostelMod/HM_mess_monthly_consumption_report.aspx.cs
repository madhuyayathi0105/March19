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
using System.Drawing;
public partial class HM_mess_monthly_consumption_report : System.Web.UI.Page
{
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string usercode = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
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
        CalendarExtender1.EndDate = DateTime.Now;
        caltodate.EndDate = DateTime.Now;
        Label1.Text = "";
        if (!IsPostBack)
        {
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_todate.Attributes.Add("readonly", "readonly");
            bindmess();
            bindheadername();
            loadsubheadername();
            binditem();
            loadmenuname();
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Visible = false;
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].ColumnCount = 0;
            FpSpread2.Visible = false;
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
    protected void cb_messname_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_messname.Checked == true)
        {
            for (int i = 0; i < cbl_messname.Items.Count; i++)
            {
                cbl_messname.Items[i].Selected = true;
            }
            txt_messname.Text = "Mess Name(" + (cbl_messname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_messname.Items.Count; i++)
            {
                cbl_messname.Items[i].Selected = false;
            }
            txt_messname.Text = "--Select--";
        }
    }
    protected void cbl_messname_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_messname.Text = "--Select--";
        cb_messname.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_messname.Items.Count; i++)
        {
            if (cbl_messname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txt_messname.Text = "Mess Name(" + commcount.ToString() + ")";
            if (commcount == cbl_messname.Items.Count)
            {
                cb_messname.Checked = true;
            }
        }
    }
    protected void bindmess()
    {
        try
        {
            ds.Clear();
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
                    for (int i = 0; i < cbl_messname.Items.Count; i++)
                    {
                        cbl_messname.Items[i].Selected = true;
                    }
                    txt_messname.Text = "Mess Name(" + cbl_messname.Items.Count + ")";
                }
            }
            else
            {
                txt_messname.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void cb_headername_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_headername.Checked == true)
            {
                for (int i = 0; i < cbl_headername.Items.Count; i++)
                {
                    cbl_headername.Items[i].Selected = true;
                }
                txt_headername.Text = "Header Name(" + (cbl_headername.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_headername.Items.Count; i++)
                {
                    cbl_headername.Items[i].Selected = false;
                }
                txt_headername.Text = "--Select--";
            }
            loadsubheadername();
            binditem();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_headername_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_headername.Text = "--Select--";
            cb_headername.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_headername.Items.Count; i++)
            {
                if (cbl_headername.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_headername.Text = "Header Name(" + commcount.ToString() + ")";
                if (commcount == cbl_headername.Items.Count)
                {
                    cb_headername.Checked = true;
                }
            }
            loadsubheadername();
            binditem();
        }
        catch (Exception ex)
        {
        }
    }
    public void bindheadername()
    {
        try
        {
            cbl_headername.Items.Clear();
            string group_code = Session["group_code"].ToString();
            string columnfield = "";
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
                columnfield = " and usercode='" + Session["usercode"] + "'";
            }
            string maninvalue = "";
            string selectnewquery = d2.GetFunction("select value  from Master_Settings where settings='ItemHeaderRights' " + columnfield + "");
            if (selectnewquery.Trim() != "" && selectnewquery.Trim() != "0")
            {
                string[] splitnew = selectnewquery.Split(',');
                if (splitnew.Length > 0)
                {
                    for (int row = 0; row <= splitnew.GetUpperBound(0); row++)
                    {
                        if (maninvalue == "")
                        {
                            maninvalue = Convert.ToString(splitnew[row]);
                        }
                        else
                        {
                            maninvalue = maninvalue + "'" + "," + "'" + Convert.ToString(splitnew[row]);
                        }
                    }
                }
            }
            ds.Clear();
            ds = d2.BindItemHeaderWithOutRights_inv();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_headername.DataSource = ds;
                cbl_headername.DataTextField = "ItemHeaderName";
                cbl_headername.DataValueField = "ItemHeaderCode";
                cbl_headername.DataBind();
                if (cbl_headername.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_headername.Items.Count; i++)
                    {
                        cbl_headername.Items[i].Selected = true;
                    }
                    txt_headername.Text = "Header Name(" + cbl_headername.Items.Count + ")";
                }
            }
            else
            {
                txt_headername.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void binditem()
    {
        try
        {
            cbl_itemname.Items.Clear();
            string itemheader = "";
            string subheader = "";
            for (int i = 0; i < cbl_headername.Items.Count; i++)
            {
                if (cbl_headername.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_headername.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_headername.Items[i].Value.ToString() + "";
                    }
                }
            }
            for (int i = 0; i < cbl_subheadername.Items.Count; i++)
            {
                if (cbl_subheadername.Items[i].Selected == true)
                {
                    if (subheader == "")
                    {
                        subheader = "" + cbl_subheadername.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        subheader = subheader + "'" + "," + "" + "'" + cbl_subheadername.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemheader.Trim() != "" && subheader.Trim() != "")
            {
                ds.Clear();
                ds = d2.BindItemCodewithsubheader_inv(itemheader, subheader);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_itemname.DataSource = ds;
                    cbl_itemname.DataTextField = "itemname";
                    cbl_itemname.DataValueField = "itemcode";
                    cbl_itemname.DataBind();
                    if (cbl_itemname.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_itemname.Items.Count; i++)
                        {
                            cbl_itemname.Items[i].Selected = true;
                        }
                        txt_itemname.Text = "Item Name(" + cbl_itemname.Items.Count + ")";
                    }
                    if (cbl_itemname.Items.Count > 5)
                    {
                        Panel1.Width = 300;
                        Panel1.Height = 300;
                    }
                }
                else
                {
                    txt_itemname.Text = "--Select--";
                }
            }
            else
            {
                txt_itemname.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void cb_itemname_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_itemname.Checked == true)
            {
                for (int i = 0; i < cbl_itemname.Items.Count; i++)
                {
                    cbl_itemname.Items[i].Selected = true;
                }
                txt_itemname.Text = "Item Name(" + (cbl_itemname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_itemname.Items.Count; i++)
                {
                    cbl_itemname.Items[i].Selected = false;
                }
                txt_itemname.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_itemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_itemname.Text = "--Select--";
            cb_itemname.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_itemname.Items.Count; i++)
            {
                if (cbl_itemname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_itemname.Text = "Item Name(" + commcount.ToString() + ")";
                if (commcount == cbl_itemname.Items.Count)
                {
                    cb_itemname.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();
            string firstdate = Convert.ToString(txt_fromdate.Text);
            string seconddate = Convert.ToString(txt_todate.Text);
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            //string itemheadercode = rs.GetSelectedItemsValueAsString(cbl_messname);
            //string hostelcode = rs.GetSelectedItemsValueAsString(cbl_headername);
            //string menuvalue = rs.GetSelectedItemsValueAsString(cbl_itemname);
            //string menuFk = rs.GetSelectedItemsValueAsString(cbl_menuname);
            string MessMasterFk = rs.GetSelectedItemsValue(cbl_messname);
            string HeaderFk = rs.GetSelectedItemsValue(cbl_headername);
            string ItemFK = rs.GetSelectedItemsValue(cbl_itemname);
            string MenuMasterFk = rs.GetSelectedItemsValue(cbl_menuname);
            string SessionFk = rs.GetSelectedItemsValue(cblsession);
            if (dt <= dt1)
            {
                #region Old query
                //string q = "";
                //if (cb_show.Checked == true)
                //{
                //    q = "select distinct  subheader_code as itemheader_code,(select MasterValue from CO_MasterValues where MasterCode=ISNULL(subheader_code,0))+'('+case when d.menutype =0 then 'Veg' when d.menutype=1 then 'Non Veg' end +')' as itemheader_name ,d.menutype from HT_DailyConsumptionDetail d,HT_DailyConsumptionMaster m,IM_ItemMaster i where d.DailyConsumptionMasterFK =m.DailyConsumptionMasterPK and d.ItemFK=i.ItemPK and m.DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and i.ItemHeaderCode in('" + hostelcode + "') and m.MessMasterFK in('" + itemheadercode + "') and ISNULL(subheader_code,0)<>0 and ForMess <>'2' and m.MenumasterFK in('" + menuFk + "') and menutype is not null order by itemheader_code ";
                //    q += " select subheader_code as itemheader_code,(select MasterValue from CO_MasterValues where MasterCode=ISNULL(subheader_code,0))+'('+case when dt.menutype =0 then 'Veg' when dt.menutype=1 then 'Non Veg' end +')' as itemheader_name,SUM(ConsumptionQty * RPU) as Consumption_Value,DailyConsDate,isnull(PurposeCatagory,'')PurposeCatagory,c.MasterValue,dt.menutype from HT_DailyConsumptionDetail dt,IM_ItemMaster i,HT_DailyConsumptionMaster dm left join CO_MasterValues c on dm.PurposeCatagory=c.MasterCode and c.MasterCriteria='Menu Purpose Category' where dm.DailyConsumptionMasterPK  =dt.DailyConsumptionMasterFK and dt.ItemFK =i.itempk and dm.DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and i.itemheadercode in ('" + hostelcode + "')  and dm.MessMasterFK in ('" + itemheadercode + "') and ForMess <>'2' and dm.MenumasterFK in('" + menuFk + "') and dt.menutype is not null group by subheader_code,DailyConsDate,PurposeCatagory,c.MasterValue,dt.menutype order by DailyConsDate, subheader_code ";
                //    q += " select SUM(Total_Present)as TotatPresent,CONVERT(varchar(10), DailyConsDate,103) as DailyConsDate,isnull(PurposeCatagory,'')PurposeCatagory,sum(VegStrength)VegStrength,sum(NonvegStrength)NonvegStrength  from HT_DailyConsumptionMaster where DailyConsDate between'" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and DailyConsumptionMasterPK  in (select DailyConsumptionMasterFK from HT_DailyConsumptionDetail) and MessMasterFK in ('" + itemheadercode + "') and ForMess <>'2' and MenumasterFK in('" + menuFk + "')  group by DailyConsDate,PurposeCatagory";
                //}
                //else
                //{
                //    q = "  select distinct  subheader_code as itemheader_code,(select MasterValue from CO_MasterValues where MasterCode=ISNULL(subheader_code,0)) as itemheader_name from HT_DailyConsumptionDetail d,HT_DailyConsumptionMaster m,IM_ItemMaster i where d.DailyConsumptionMasterFK =m.DailyConsumptionMasterPK and d.ItemFK=i.ItemPK and m.DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and i.ItemHeaderCode in('" + hostelcode + "') and m.MessMasterFK in('" + itemheadercode + "') and ISNULL(subheader_code,0)<>0 and ForMess <>'2' and m.MenumasterFK in('" + menuFk + "') order by itemheader_code ";
                //    q += "  select subheader_code as itemheader_code,(select MasterValue from CO_MasterValues where MasterCode=ISNULL(subheader_code,0))as itemheader_name,SUM(ConsumptionQty * RPU) as Consumption_Value,DailyConsDate,isnull(PurposeCatagory,'')PurposeCatagory,c.MasterValue from HT_DailyConsumptionDetail dt,IM_ItemMaster i,HT_DailyConsumptionMaster dm left join CO_MasterValues c on dm.PurposeCatagory=c.MasterCode and c.MasterCriteria='Menu Purpose Category' where dm.DailyConsumptionMasterPK  =dt.DailyConsumptionMasterFK and dt.ItemFK =i.itempk and dm.DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and i.itemheadercode in ('" + hostelcode + "')  and dm.MessMasterFK in ('" + itemheadercode + "') and ForMess <>'2' and dm.MenumasterFK in('" + menuFk + "')  group by subheader_code,DailyConsDate,PurposeCatagory,c.MasterValue order by DailyConsDate, subheader_code ";
                //    q += " select SUM(Total_Present)as TotatPresent,CONVERT(varchar(10), DailyConsDate,103) as DailyConsDate,isnull(PurposeCatagory,'')PurposeCatagory,sum(VegStrength)VegStrength,sum(NonvegStrength)NonvegStrength  from HT_DailyConsumptionMaster where DailyConsDate between'" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and DailyConsumptionMasterPK  in (select DailyConsumptionMasterFK from HT_DailyConsumptionDetail) and MessMasterFK in ('" + itemheadercode + "') and ForMess <>'2' and MenumasterFK in('" + menuFk + "')  group by DailyConsDate,PurposeCatagory";
                //}
                #endregion
                int showall = 0;
                if (cb_show.Checked == true)
                    showall = 1;
                Hashtable MessMonthlyConsumptionHash = new Hashtable();
                MessMonthlyConsumptionHash.Add("Showall", showall);
                MessMonthlyConsumptionHash.Add("FromDate", dt.ToString("MM/dd/yyyy"));
                MessMonthlyConsumptionHash.Add("ToDate", dt1.ToString("MM/dd/yyyy"));
                MessMonthlyConsumptionHash.Add("ItemHeader", HeaderFk);
                MessMonthlyConsumptionHash.Add("MessMasterFk", MessMasterFk);
                MessMonthlyConsumptionHash.Add("MenuMasterFk", MenuMasterFk);
                MessMonthlyConsumptionHash.Add("SessionFK", SessionFk);
                ds.Clear();
                ds = d2.select_method("MessMonthlyConsumption", MessMonthlyConsumptionHash, "sp");
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0)
                {
                    FpSpread1.Sheets[0].RowCount = 0;
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    FpSpread1.CommandBar.Visible = false;
                    FpSpread1.Sheets[0].AutoPostBack = true;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread1.Sheets[0].RowHeader.Visible = false;
                    FpSpread1.Sheets[0].ColumnCount = 3;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[0].Width = 50;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[1].Width = 100;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Session"; // poo 18.11.17
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[1].Width = 100;
                    double forgrandtot = 0; double fortotal = 0;
                    Dictionary<int, double> dicgettotal = new Dictionary<int, double>();
                    Dictionary<int,string> dicgrandtotal=new Dictionary<int,string>();
                    
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FpSpread1.Sheets[0].ColumnCount++;
                        string header_name = Convert.ToString(ds.Tables[0].Rows[i]["itemheader_name"]);
                        string header_code = Convert.ToString(ds.Tables[0].Rows[i]["itemheader_code"]);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = header_name;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = header_code;
                        if (cb_show.Checked == true)
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Note = Convert.ToString(ds.Tables[0].Rows[i]["menutype"]);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    }
                    if (cb_show.Checked == true)
                    {
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Veg Value";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Non Veg Value";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    }
                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Consume Value";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    if (cb_show.Checked == true)
                    {
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Veg Count";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Non Veg Count";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    }
                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total Students";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    if (cb_show.Checked == true)
                    {
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Veg per Student";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Non Veg Per Student";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    }
                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Value Per Student";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    int sno = 0;
                    DataView dv = new DataView();
                    ArrayList TypeArr = new ArrayList();
                    TypeArr.Add("0");
                    TypeArr.Add("1");
                    DataTable DistinctDailyconsumptiondate = ds.Tables[2];
                    DataTable DT2count1 = DistinctDailyconsumptiondate.DefaultView.ToTable(true, "DailyConsDate", "PurposeCatagory");
                    DT2count1.DefaultView.Sort = "DailyConsDate,PurposeCatagory";
                    DataTable DT2count = DT2count1.DefaultView.ToTable();
                    for (int i = 0; i < DT2count.Rows.Count; i++)//ds.Tables[2].Rows.Count
                    {
                        FpSpread1.Sheets[0].RowCount++;
                        sno++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        string consumption_date = Convert.ToString(DT2count.Rows[i]["DailyConsDate"]);
                        string[] split2 = consumption_date.Split('/');
                        dt2 = Convert.ToDateTime(split2[1] + "/" + split2[0] + "/" + split2[2]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dt2.ToString("dd/MM/yyyy");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = dt2.ToString("dd/MM/yyyy");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        int col = 2; double totalvalue = 0; double total = 0; double totalval = 0; double consumegrandtot = 0;
                        double veggrand = 0; double nonveggrand = 0; double grandtotstrength = 0; double grandvegper = 0;
                        double grandnonvegper = 0; 
                        if (cb_show.Checked == true)
                        {
                            #region Nec
                            bool first = false;
                            for (int ses = 0; ses < cblsession.Items.Count; ses++)
                            {
                                double totalconsum = 0;
                                col = 2;
                                if (cblsession.Items[ses].Selected == true)
                                {
                                    for (int ik = 0; ik < ds.Tables[0].Rows.Count; ik++)
                                    {
                                        string MenuType = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, ik + 3].Note);
                                        int Headercode = Convert.ToInt32(FpSpread1.Sheets[0].ColumnHeader.Cells[0, ik + 3].Tag);
                                        string HeaderName = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, ik + 3].Text);
                                        //ds.Tables[1].DefaultView.RowFilter = "DailyConsDate='" + dt2.ToString("MM/dd/yyyy") + "' and itemheader_code='" + Convert.ToString(ds.Tables[0].Rows[ik]["itemheader_code"]) + "' and itemheader_name='" + Convert.ToString(ds.Tables[1].Rows[ik]["itemheader_name"]) + "'and PurposeCatagory='" + Convert.ToString(ds.Tables[2].Rows[i]["PurposeCatagory"]) + "' and menutype='" + Convert.ToString(TypeArr[k]) + "'";
                                        ds.Tables[1].DefaultView.RowFilter = "DailyConsDate='" + dt2.ToString("MM/dd/yyyy") + "' and itemheader_code='" + Headercode + "' and itemheader_name='" + HeaderName + "'and PurposeCatagory='" + Convert.ToString(ds.Tables[2].Rows[i]["PurposeCatagory"]) + "' and menutype='" + MenuType + "' and SessionFK='" + Convert.ToString(cblsession.Items[ses].Value) + "'";
                                        dv = ds.Tables[1].DefaultView;
                                        
                                        col++;
                                        if (dv.Count > 0)
                                        {
                                            string PurposeCategory = string.Empty;
                                            if (!string.IsNullOrEmpty(Convert.ToString(dv[0]["MasterValue"])))
                                                PurposeCategory = "(" + Convert.ToString(dv[0]["MasterValue"]) + ")";

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dv[0]["PurposeCatagory"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dt2.ToString("dd/MM/yyyy") + "" + PurposeCategory;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = dt2.ToString("dd/MM/yyyy"); //Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(cblsession.Items[ses].Text);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(cblsession.Items[ses].Value);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dv[0]["Consumption_Value"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            if (dicgrandtotal.ContainsKey(col))
                                                dicgrandtotal[col] += Convert.ToString(dv[0]["Consumption_Value"]);
                                            else
                                                dicgrandtotal.Add(col, Convert.ToString(dv[0]["Consumption_Value"]));

                                            string value = Convert.ToString(dv[0]["Consumption_Value"]);
                                            if (MenuType == "0")
                                            {
                                                if (value.Trim() != "")
                                                    total += Convert.ToDouble(dv[0]["Consumption_Value"]);

                                            }
                                            if (MenuType == "1")
                                            {
                                                if (value.Trim() != "")
                                                {                                                    
                                                    totalval += Convert.ToDouble(dv[0]["Consumption_Value"]);

                                                }
                                            }
                                            if (value.Trim() != "")
                                                totalvalue += Convert.ToDouble(dv[0]["Consumption_Value"]);

                                        }
                                        else
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "-";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                        }
                                        first = true;
                                    }
                                    col++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(total);
                                   
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                    if (dicgrandtotal.ContainsKey(col))
                                        dicgrandtotal[col] += Convert.ToString(total);
                                    else
                                        dicgrandtotal.Add(col, Convert.ToString(total));
                                    col++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(totalval);
                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                    if (dicgrandtotal.ContainsKey(col))
                                        dicgrandtotal[col] += Convert.ToString(totalval);
                                    else
                                        dicgrandtotal.Add(col, Convert.ToString(totalval));
                                    Double Consumetotal = total + totalval;
                                    col++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(Consumetotal); 
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                    if (dicgrandtotal.ContainsKey(col))
                                        dicgrandtotal[col] += Convert.ToString(Consumetotal);
                                    else
                                        dicgrandtotal.Add(col, Convert.ToString(Consumetotal));
                                    consumegrandtot += Consumetotal;
                                    col++;
                                    Double vegind = 0; Double nonvegind = 0;
                                    double.TryParse(Convert.ToString(ds.Tables[1].Compute("Sum(VegStrength)", "DailyConsDate='" + dt2.ToString("MM/dd/yyyy") + "' and itemheader_code='" + Convert.ToString(ds.Tables[1].Rows[i]["itemheader_code"]) + "' and PurposeCatagory='" + Convert.ToString(DT2count.Rows[i]["PurposeCatagory"]) + "' and SessionFK='" + cblsession.Items[ses].Value + "'")), out vegind);
                                    double.TryParse(Convert.ToString(ds.Tables[1].Compute("Sum(NonvegStrength)", "DailyConsDate='" + dt2.ToString("MM/dd/yyyy") + "' and itemheader_code='" + Convert.ToString(ds.Tables[1].Rows[i]["itemheader_code"]) + "' and PurposeCatagory='" + Convert.ToString(DT2count.Rows[i]["PurposeCatagory"]) + "' and SessionFK='" + cblsession.Items[ses].Value + "'")), out nonvegind);
                                    //Double.TryParse(Convert.ToString(ds.Tables[2].Rows[i]["VegStrength"]), out vegind);
                                    
                                    //Double.TryParse(Convert.ToString(ds.Tables[2].Rows[i]["NonvegStrength"]), out nonvegind);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(vegind);                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                    if (dicgrandtotal.ContainsKey(col))
                                        dicgrandtotal[col] += Convert.ToString(vegind);
                                    else
                                        dicgrandtotal.Add(col, Convert.ToString(vegind));
                                    veggrand+=vegind;
                                    col++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(nonvegind);
                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                    if (dicgrandtotal.ContainsKey(col))
                                        dicgrandtotal[col] += Convert.ToString(nonvegind);
                                    else
                                        dicgrandtotal.Add(col, Convert.ToString(nonvegind));
                                    nonveggrand += nonvegind;
                                    col++;
                                    Double totalstrength = vegind+nonvegind;
                                    //Double.TryParse(Convert.ToString(ds.Tables[2].Rows[i]["TotatPresent"]), out totalstrength);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(totalstrength);                                    
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                    if (dicgrandtotal.ContainsKey(col))
                                        dicgrandtotal[col] += Convert.ToString(totalstrength);
                                    else
                                        dicgrandtotal.Add(col, Convert.ToString(totalstrength));
                                    grandtotstrength += totalstrength;
                                    double vegperstud = total / vegind;
                                    double nonvegperstud = totalval / nonvegind;
                                    
                                    col++;
                                    if(total !=0 && vegind !=0)
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(Math.Round(vegperstud, 2));
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                        if (dicgrandtotal.ContainsKey(col))
                                            dicgrandtotal[col] += Convert.ToString(Math.Round(vegperstud, 2));
                                        else
                                            dicgrandtotal.Add(col, Convert.ToString(Math.Round(vegperstud, 2)));
                                        grandvegper += vegperstud;
                                    col++;
                                    if (totalval != 0 && nonvegind != 0)
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(Math.Round(nonvegperstud, 2));
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                        if (dicgrandtotal.ContainsKey(col))
                                            dicgrandtotal[col] += Convert.ToString(Math.Round(nonvegperstud, 2));
                                        else
                                            dicgrandtotal.Add(col, Convert.ToString(Math.Round(nonvegperstud, 2)));
                                        grandnonvegper += nonvegperstud;
                                    }
                                    else 
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "-";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                    }

                                    
                                    sno++;
                                    FpSpread1.Sheets[0].RowCount++;
                                    
                                }
                                col = 2;
                                
                                
                            }
                            for (int ik = 0; ik < ds.Tables[0].Rows.Count; ik++)
                            {
                                col++;
                               
                                
                                
                                string sdfs = "DailyConsDate='" + dt2.ToString("MM/dd/yyyy") + "' and itemheader_code='" + Convert.ToString(ds.Tables[0].Rows[ik]["itemheader_code"]) + "' and MenuType='" + Convert.ToString(ds.Tables[0].Rows[ik]["MenuType"]) + "' ";
                                double.TryParse(Convert.ToString(ds.Tables[1].Compute("Sum(Consumption_Value)", "DailyConsDate='" + dt2.ToString("MM/dd/yyyy") + "' and itemheader_code='" + Convert.ToString(ds.Tables[0].Rows[ik]["itemheader_code"]) + "' and MenuType='" + Convert.ToString(ds.Tables[0].Rows[ik]["MenuType"]) + "' ")), out fortotal);

                                if (fortotal > 0)
                                {
                                    //string PurposeCategory = string.Empty;
                                    //if (!string.IsNullOrEmpty(Convert.ToString(dv[0]["MasterValue"])))
                                    //    PurposeCategory = "(" + Convert.ToString(dv[0]["MasterValue"]) + ")";
                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dv[0]["PurposeCatagory"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dt2.ToString("dd/MM/yyyy");
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = dt2.ToString("dd/MM/yyyy");
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Total";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].ForeColor = Color.BlueViolet;
                                    totalvalue += fortotal;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(fortotal);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].ForeColor = Color.BlueViolet;
                                    if (dicgettotal.ContainsKey(col))
                                        dicgettotal[col] += Math.Round(fortotal, 2);
                                    else
                                        dicgettotal.Add(col, Math.Round(fortotal, 2));
                                    

                                }

                            }
                            col++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(total);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].ForeColor = Color.BlueViolet;
                            if (dicgettotal.ContainsKey(col))
                                dicgettotal[col] += Math.Round(total, 2);
                            else
                                dicgettotal.Add(col,(Math.Round(total, 2)));
                            col++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(totalval);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].ForeColor = Color.BlueViolet;
                            if (dicgettotal.ContainsKey(col))
                                dicgettotal[col] += Math.Round(totalval, 2);
                            else
                                dicgettotal.Add(col, Math.Round(totalval, 2));
                            //col++;
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(totalvalue);
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].ForeColor = Color.BlueViolet;Math.Round(vegperstud, 2)
                            col++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(Math.Round(consumegrandtot,2));
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].ForeColor = Color.BlueViolet;
                            
                            if (dicgettotal.ContainsKey(col))
                                dicgettotal[col] += Math.Round(consumegrandtot, 2);
                            else
                                dicgettotal.Add(col, Math.Round(consumegrandtot, 2));
                            col++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(Math.Round(veggrand,2));
                            
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].ForeColor = Color.BlueViolet;
                            if (dicgettotal.ContainsKey(col))
                                dicgettotal[col] += Math.Round(veggrand, 2);
                            else
                                dicgettotal.Add(col, Math.Round(veggrand, 2));
                            col++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(nonveggrand);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].ForeColor = Color.BlueViolet;
                            if (dicgettotal.ContainsKey(col))
                                dicgettotal[col] += Math.Round(nonveggrand, 2);
                            else
                                dicgettotal.Add(col, Math.Round(nonveggrand, 2));
                            col++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(grandtotstrength);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].ForeColor = Color.BlueViolet;
                            if (dicgettotal.ContainsKey(col))
                                dicgettotal[col] += Math.Round(grandtotstrength, 2);
                            else
                                dicgettotal.Add(col, Math.Round(grandtotstrength, 2));
                            col++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(Math.Round(grandvegper,2));
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].ForeColor = Color.BlueViolet;
                            
                            col++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(Math.Round(grandnonvegper,2));
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].ForeColor = Color.BlueViolet;
                            
                            
                            #endregion
                        }
                        else
                        {
                            #region JPR
                            // sessionwise added by poomalar 18.11.17
                            # region SessionWise
                            int colcount = 2;
                            for (int se = 0; se < cblsession.Items.Count; se++)
                            {
                                double totalconsum = 0;
                                if (cblsession.Items[se].Selected == true)
                                {
                                    for (int sp = 0; sp < ds.Tables[0].Rows.Count; sp++)
                                    {
                                        DataView dv1 = new DataView();
                                        ds.Tables[1].DefaultView.RowFilter = "DailyConsDate='" + dt2.ToString("MM/dd/yyyy") + "' and itemheader_code='" + Convert.ToString(ds.Tables[0].Rows[sp]["itemheader_code"]) + "' and PurposeCatagory='" + Convert.ToString(DT2count.Rows[i]["PurposeCatagory"]) + "' and SessionFK='" + cblsession.Items[se].Value + "'";//Convert.ToString(ds.Tables[2].Rows[i]["PurposeCatagory"])
                                        dv1 = ds.Tables[1].DefaultView;
                                        colcount++;
                                        if (dv1.Count > 0)
                                        {

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dt2.ToString("dd/MM/yyyy");
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = dt2.ToString("dd/MM/yyyy"); //Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(cblsession.Items[se].Text);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(cblsession.Items[se].Value);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(dv1[0]["Consumption_Value"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount].Tag = dt2.ToString("dd/MM/yyyy"); //Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Right;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount].Font.Name = "Book Antiqua";
                                            totalconsum = totalconsum + Convert.ToDouble(dv1[0]["Consumption_Value"]);

                                        }
                                        else
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dt2.ToString("dd/MM/yyyy");
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = dt2.ToString("dd/MM/yyyy"); //Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(cblsession.Items[se].Text);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(cblsession.Items[se].Value);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount].Text = "-";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount].Tag = dt2.ToString("dd/MM/yyyy"); //Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount].Font.Name = "Book Antiqua";
                                        }
                                    }
                                    colcount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(totalconsum);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount].Tag = dt2.ToString("dd/MM/yyyy"); //Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount].Font.Name = "Book Antiqua";
                                    colcount = 2;
                                    DataView dvtotal = new DataView();
                                    ds.Tables[2].DefaultView.RowFilter = "DailyConsDate='" + dt2.ToString("dd/MM/yyyy") + "' and SessionFK='" + cblsession.Items[se].Value + "'";//Convert.ToString(ds.Tables[2].Rows[i]["PurposeCatagory"])
                                    dvtotal = ds.Tables[2].DefaultView;
                                    if (dvtotal.Count > 0)
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(dvtotal[0]["TotatPresent"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Tag = dt2.ToString("dd/MM/yyyy"); //Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = "-";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Tag = dt2.ToString("dd/MM/yyyy"); //Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                                    }
                                }
                                sno++;
                                FpSpread1.Sheets[0].RowCount++;
                            }
                            col = 2;
                            for (int ik = 0; ik < ds.Tables[0].Rows.Count; ik++)
                            {
                               // double fortotal = 0; 
                                ds.Tables[1].DefaultView.RowFilter = "DailyConsDate='" + dt2.ToString("MM/dd/yyyy") + "' and itemheader_code='" + Convert.ToString(ds.Tables[0].Rows[ik]["itemheader_code"]) + "' and PurposeCatagory='" + Convert.ToString(ds.Tables[1].Rows[i]["PurposeCatagory"]) + "'";
                                double.TryParse(Convert.ToString(ds.Tables[1].Compute("Sum(Consumption_Value)", "DailyConsDate='" + dt2.ToString("MM/dd/yyyy") + "' and itemheader_code='" + Convert.ToString(ds.Tables[0].Rows[ik]["itemheader_code"]) + "' and PurposeCatagory='" + Convert.ToString(DT2count.Rows[i]["PurposeCatagory"]) + "'")), out fortotal);
                                dv = ds.Tables[1].DefaultView;
                                col++;
                                if (dv.Count > 0)
                                {
                                    
                                    string PurposeCategory = string.Empty;
                                    if (!string.IsNullOrEmpty(Convert.ToString(dv[0]["MasterValue"])))
                                        PurposeCategory = "(" + Convert.ToString(dv[0]["MasterValue"]) + ")";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dv[0]["PurposeCatagory"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dt2.ToString("dd/MM/yyyy") + "" + PurposeCategory;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = dt2.ToString("dd/MM/yyyy");
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Total";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].ForeColor = Color.BlueViolet;
                                    totalvalue += fortotal;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(fortotal);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].ForeColor = Color.BlueViolet;
                                    if (dicgettotal.ContainsKey(col))
                                        dicgettotal[col] += fortotal;
                                    else
                                        dicgettotal.Add(col, fortotal);
                                   
                                }
                                
                            }
                                col++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(totalvalue);
                                
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].ForeColor = Color.BlueViolet;
                                if (dicgettotal.ContainsKey(col))
                                    dicgettotal[col] += Math.Round(totalvalue, 2);
                                else
                                    dicgettotal.Add(col, Math.Round(totalvalue, 2));
                                double totalPresent = 0; double rp = 0;
                                double.TryParse(Convert.ToString(ds.Tables[2].Compute("Sum(TotatPresent)", "DailyConsDate='" + dt2.ToString("dd/MM/yyyy") + "' and PurposeCatagory='" + Convert.ToString(DT2count.Rows[i]["PurposeCatagory"]) + "'")), out totalPresent);
                                col++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(totalPresent);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].ForeColor = Color.BlueViolet;
                                if (dicgettotal.ContainsKey(col))
                                    dicgettotal[col] += Math.Round(totalPresent, 2);
                                else
                                    dicgettotal.Add(col, Math.Round(totalPresent, 2));
                                



                            #endregion
                            # endregion
                            # region commented
                            /*
                            for (int ik = 0; ik < ds.Tables[0].Rows.Count; ik++)
                            {
                                double fortotal = 0;
                                ds.Tables[1].DefaultView.RowFilter = "DailyConsDate='" + dt2.ToString("MM/dd/yyyy") + "' and itemheader_code='" + Convert.ToString(ds.Tables[0].Rows[ik]["itemheader_code"]) + "' and PurposeCatagory='" + Convert.ToString(ds.Tables[1].Rows[i]["PurposeCatagory"]) + "'";
                                dv = ds.Tables[1].DefaultView;

                                //double.TryParse(Convert.ToString(ds.Tables[1].Compute("Sum(Consumption_Value)", "DailyConsDate='" + dt2.ToString("MM/dd/yyyy") + "' and itemheader_code='" + Convert.ToString(ds.Tables[1].Rows[ik]["itemheader_code"]) + "' and PurposeCatagory='" + Convert.ToString(DT2count.Rows[i]["PurposeCatagory"]) + "'")), out fortotal); // poo
                                col++;
                                if (dv.Count > 0)
                                {

                                    string PurposeCategory = string.Empty;
                                    if (!string.IsNullOrEmpty(Convert.ToString(dv[0]["MasterValue"])))
                                        PurposeCategory = "(" + Convert.ToString(dv[0]["MasterValue"]) + ")";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dv[0]["PurposeCatagory"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dt2.ToString("dd/MM/yyyy") + "" + PurposeCategory;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = dt2.ToString("dd/MM/yyyy"); //Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                                    double value = 0;
                                    double.TryParse(Convert.ToString(fortotal), out value);

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(value);//dv[0]["Consumption_Value"]);   // poo                                 
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                    totalvalue += value;
                                    //string value = Convert.ToString(dv[0]["Consumption_Value"]);
                                    //if (value.Trim() != "")
                                    //{
                                    //  totalvalue = totalvalue + Convert.ToDouble(dv[0]["Consumption_Value"]);
                                    //}
                                }

                                //else
                                //{
                                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "-";
                                //    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                //}
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Total";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = dt2.ToString("dd/MM/yyyy"); //Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            }
                            */
                            
                        }
                        
                        //if (cb_show.Checked == true)
                        //{
                        //    col++;
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(total);
                        //    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                        //    col++;
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(totalval);
                        //    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                        //}
                        
                        //double veg = 0; double nonveg = 0; double tot = 0;
                        //if (cb_show.Checked == true)
                        //{                            
                        //    col++;
                        //    double.TryParse(Convert.ToString(ds.Tables[2].Compute("Sum(VegStrength)", "DailyConsDate='" + dt2.ToString("dd/MM/yyyy") + "' and PurposeCatagory='" + Convert.ToString(DT2count.Rows[i]["PurposeCatagory"]) + "'")), out veg);
                        //    double.TryParse(Convert.ToString(ds.Tables[2].Compute("Sum(NonvegStrength)", "DailyConsDate='" + dt2.ToString("dd/MM/yyyy") + "' and PurposeCatagory='" + Convert.ToString(DT2count.Rows[i]["PurposeCatagory"]) + "'")), out nonveg);
                            
                        //    //if (nonveg == 0)
                        //    //{
                        //    //    tot = veg;
                        //    //}
                        //    //else
                        //    //{
                        //    //    tot = veg - nonveg;
                        //    //}
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(veg);//ds.Tables[2].Rows[i]["vegStrength"]);
                        //    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                        //    col++;
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(nonveg);//ds.Tables[2].Rows[i]["NonvegStrength"]);
                        //    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                        //}
                        //if (cb_show.Checked == false)
                        //{
                        //    double totalPresent = 0;
                        //    double.TryParse(Convert.ToString(ds.Tables[2].Compute("Sum(TotatPresent)", "DailyConsDate='" + dt2.ToString("dd/MM/yyyy") + "' and PurposeCatagory='" + Convert.ToString(DT2count.Rows[i]["PurposeCatagory"]) + "'")), out totalPresent);
                        //    col++;
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(totalPresent);//ds.Tables[2].Rows[i]["TotatPresent"]);
                        //    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].ForeColor = Color.BlueViolet;
                        //}
                        
                        if (cb_show.Checked == true)
                        {
                            #region Nec
                            //for (int st = 0; st < FpSpread1.Sheets[0].RowCount; st++)
                            //{
                            //    double newstrenths = 0;
                            //    Double totalvegvalue = 0;
                            //    double.TryParse(FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 4].Text, out totalvegvalue);
                            //    string strenghs = Convert.ToString(ds.Tables[2].Rows[st]["VegStrength"]);
                            //    string totalvegstrength = Convert.ToString(FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 3].Text); // poo
                            //    if (totalvegstrength.Trim() != "")
                            //    {
                            //        double.TryParse(totalvegstrength, out newstrenths);
                            //        newstrenths = Convert.ToDouble(totalvegstrength);
                            //    }
                            //    double rpus = total / newstrenths;
                            //    double rpus = 0;
                            //    double.TryParse(Convert.ToString(totalvegvalue / newstrenths), out rpus);

                            //    if (Convert.ToString(rpus).ToUpper() == "NAN")
                            //        rpus = 0;

                            //    col++;

                            //    FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(Math.Round(rpus, 2));
                            //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                            //    FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                            //    FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            //    FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            //    double newstrenthsnonveg = 0;
                            //    string nonstrenghs = Convert.ToString(ds.Tables[2].Rows[st]["NonvegStrength"]);
                            //    Double totalnonvegvalue = Convert.ToDouble(FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 6].Text);
                            //    string totalnonvegstrength = Convert.ToString(FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 4].Text); // poo
                            //    if (totalnonvegstrength.Trim() != "")
                            //    {
                            //        double.TryParse(totalnonvegstrength, out newstrenthsnonveg);
                            //        newstrenthsnonveg = Convert.ToDouble(totalnonvegstrength);
                            //    }
                            //    double rps = totalval / newstrenthsnonveg;
                            //    double rps = 0;
                            //    double.TryParse(Convert.ToString(totalvegvalue / newstrenthsnonveg), out rps);
                            //    if (Convert.ToString(rps).ToUpper() == "NAN")
                            //        rps = 0;
                            //    if (Convert.ToString(rps).ToUpper() == "NAN")
                            //    {
                            //        rp = rpus;
                            //    }
                            //    else
                            //    {
                            //        rp = rps + rpus;
                            //    }
                            //    col++;
                            //    FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(Math.Round(rp, 2));
                            //    FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                            //    FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            //    FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            //}
                            #endregion
                        }
                        if (cb_show.Checked == false)
                        {
                            #region Jpr
                            //for (int st = 0; st < FpSpread1.Sheets[0].RowCount; st++)
                            //{
                            //    double newstrenth = 0;
                            //    Double totalconsumevalue = 0;
                            //    //string strengh = Convert.ToString(ds.Tables[2].Rows[st]["TotatPresent"]);
                            //    double.TryParse(FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 3].Text, out totalconsumevalue);
                            //    string totalstrength = Convert.ToString(FpSpread1.Sheets[0].Cells[st, col].Text); // poo
                            //    if (totalstrength.Trim() != "")
                            //    {
                            //        double.TryParse(totalstrength, out newstrenth);
                            //        // newstrenth = Convert.ToDouble(totalstrength);
                            //    }
                            //    //double rpu = totalvalue / newstrenth;
                            //    double rpu = 0;
                            //    double.TryParse(Convert.ToString(totalconsumevalue / newstrenth), out rpu);
                            //    //col++;
                            //    if (totalconsumevalue != 0)
                            //    {
                            //        FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(Math.Round(rpu, 2));
                            //        FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                            //        FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            //        FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            //    }
                            //    else
                            //    {
                            //        FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].Text = "-";
                            //        FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            //        FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            //        FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            //    }
                            //}
                            #endregion
                        }
                        else
                        {
                            col++;
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                        }
                            #endregion

                    }
                    
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "GrandTotal";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].ForeColor = Color.MediumSlateBlue;
                    if (dicgettotal.Count > 0)
                    {
                        for (int dic = 0; dic < FpSpread1.Sheets[0].ColumnCount; dic++)
                        {
                            if (dicgettotal.ContainsKey(dic))
                            {
                                string total = Convert.ToString(dicgettotal[dic]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, dic].Text = total;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, dic].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, dic].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, dic].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, dic].ForeColor = Color.Peru;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, dic].Font.Bold = true;
                            }
                        }
                    }
                    if (cb_show.Checked == false)
                    {
                        for (int st = 0; st < FpSpread1.Sheets[0].RowCount; st++)
                        {

                            double newstrenth = 0;
                            Double totalconsumevalue = 0;
                            //string strengh = Convert.ToString(ds.Tables[2].Rows[st]["TotatPresent"]);
                            double.TryParse(FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 3].Text, out totalconsumevalue);
                            string totalstrength = Convert.ToString(FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 2].Text); // poo
                            if (totalstrength.Trim() != "")
                            {
                                double.TryParse(totalstrength, out newstrenth);
                                // newstrenth = Convert.ToDouble(totalstrength);
                            }
                            //double rpu = totalvalue / newstrenth;
                            double rpu = 0;
                            double.TryParse(Convert.ToString(totalconsumevalue / newstrenth), out rpu);
                            //col++;
                            if (totalconsumevalue != 0)
                            {
                                FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(Math.Round(rpu, 2));
                                FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";

                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].Text = "-";
                                FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            }
                        }
                    }
                    if (cb_show.Checked == true)
                    {
                        for (int st = 0; st < FpSpread1.Sheets[0].RowCount; st++)
                        {
                            
                            double vegstrenth = 0; double nonvegstrenth = 0; Double nonvegvalue = 0;
                            Double vegvalue = 0; double vegper = 0; double nonvegper = 0;
                            //string strengh = Convert.ToString(ds.Tables[2].Rows[st]["TotatPresent"]);
                            double.TryParse(FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 9].Text, out vegvalue);
                            double.TryParse(FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 8].Text, out nonvegvalue);
                            double.TryParse(FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 6].Text, out vegstrenth);
                            double.TryParse(FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 5].Text, out nonvegstrenth);
                            vegper = vegvalue / vegstrenth;
                            nonvegper = nonvegvalue / nonvegstrenth;
                            if (vegvalue != 0 && vegstrenth != 0)
                            {
                                FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 3].Text = Convert.ToString(Math.Round(vegper, 2));
                                FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 3].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 3].Font.Name = "Book Antiqua";
                            }
                            if (nonvegstrenth != 0 && nonvegvalue != 0)
                            {
                                FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(Math.Round(nonvegper, 2));
                                FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[st, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                            }
                        }
                    }
                    // int colu = 2; FpSpread1.Sheets[0].RowCount++;
                    // for (int gt = 0; gt < FpSpread1.Sheets[0].ColumnCount; gt++)
                    //{
                    //    forgrandtot += fortotal;
                    //    colu++;
                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "GrandTotal";
                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gt].Text = Convert.ToString(forgrandtot);
                    //}
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.Visible = true;
                    FpSpread1.SaveChanges();
                    rptprint.Visible = true;
                    div2.Visible = true;
                    lbl_error.Visible = false;
                }
                else
                {
                    FpSpread1.Visible = false;
                    rptprint.Visible = false;
                    div2.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "No Records Found";
                }
            }
        }
        catch { }
    }
    protected void FpSpread1_CellClick(object sender, EventArgs e)
    {
        check = true;
    }
    protected void FpSpread1_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            if (check == true)
            {
                string messfk = rs.GetSelectedItemsValueAsString(cbl_messname);
                FpSpread1.SaveChanges();
                string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
                collegecode = collegecode1;
                if (activerow.Trim() != "")
                {
                    DateTime dt2 = new DateTime();
                    //string consumedate = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                    //string consumedate1 = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), FpSpread1.Sheets[0].ColumnCount - 2].Text);
                    //string Nonveg = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), FpSpread1.Sheets[0].ColumnCount - 3].Text);
                    //string veg = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), FpSpread1.Sheets[0].ColumnCount - 4].Text);
                    string subHeaderCode = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt32(activecol)].Tag);
                    string PurposeCategory = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                    string menuFk = rs.GetSelectedItemsValueAsString(cbl_menuname);
                    string SessionFk = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                    string Sessionfkall = rs.GetSelectedItemsValue(cblsession);
                    //string[] split2 = consumedate.Split('/');
                    //dt2 = Convert.ToDateTime(split2[1] + "/" + split2[0] + "/" + split2[2]);
                    //datelable.Text = "Date :" + Convert.ToString(dt2.ToString("dd/MM/yyyy"));
                    //lblveg.Text = "Veg:" + veg + "";
                    //lblnonveg.Text = "Non_Veg:" + Nonveg + "";
                    //lbltotal.Text = "Total Strength :" + consumedate1 + "";
                    if (cb_show.Checked == true)
                    {
                        string consumedates = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                        string consumedates1 = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), FpSpread1.Sheets[0].ColumnCount - 4].Text);
                        string Nonvegs = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), FpSpread1.Sheets[0].ColumnCount - 5].Text);
                        string vegs = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), FpSpread1.Sheets[0].ColumnCount - 6].Text);
                        string subHeaderCodes = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt32(activecol)].Tag);
                        string PurposeCategorys = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                        string menuFks = rs.GetSelectedItemsValueAsString(cbl_menuname);
                        string[] splits2 = consumedates.Split('/');
                        dt2 = Convert.ToDateTime(splits2[1] + "/" + splits2[0] + "/" + splits2[2]);
                        datelable.Text = "Date :" + Convert.ToString(dt2.ToString("dd/MM/yyyy"));
                        lblveg.Text = "Veg:" + vegs + "";
                        lblnonveg.Text = "Non_Veg:" + Nonvegs + "";
                        lbltotal.Text = "Total Strength :" + consumedates1 + "";
                        lblveg.Visible = true;
                        lblnonveg.Visible = true;
                    }
                    if (cb_show.Checked == false)
                    {
                        string consumedate = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                        string consumedate1 = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), FpSpread1.Sheets[0].ColumnCount - 2].Text);
                        string[] split2 = consumedate.Split('/');
                        dt2 = Convert.ToDateTime(split2[1] + "/" + split2[0] + "/" + split2[2]);
                        datelable.Text = "Date :" + Convert.ToString(dt2.ToString("dd/MM/yyyy"));
                        lbltotal.Text = "Total Strength :" + consumedate1 + "";
                        lblveg.Visible = false;
                        lblnonveg.Visible = false;
                    }
                    string selectquery = "select i.itemcode,i.itemname,DailyConsDate,SUM(ConsumptionQty) as Consumption_Qty,RPU,SUM (ConsumptionQty * RPU) as Consumption_Value from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dt,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dt.DailyConsumptionMasterfK and dt.itemfk =i.ItemPK and dm.DailyConsDate = '" + dt2.ToString("MM/dd/yyyy") + "' and MessMasterFK in('" + messfk + "') and isnull(dm.PurposeCatagory,0)='" + PurposeCategory + "' and dm.MenumasterFK in('" + menuFk + "') "; // poo
                    if (!string.IsNullOrEmpty(subHeaderCode))
                        selectquery += " and i.subheader_code in('" + subHeaderCode + "')";
                    if (!string.IsNullOrEmpty(SessionFk))
                        selectquery += " and SessionFK in ('" + SessionFk + "')";
                    else
                        selectquery += " and SessionFK in (" + Sessionfkall + ")";
                    selectquery += " group by DailyConsDate,RPU,i.itemcode,i.itemname order by i.itemcode";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        surediv.Visible = true;
                        FpSpread2.Sheets[0].RowCount = 0;
                        FpSpread2.Sheets[0].ColumnCount = 0;
                        FpSpread2.CommandBar.Visible = false;
                        FpSpread2.Sheets[0].AutoPostBack = true;
                        FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread2.Sheets[0].RowHeader.Visible = false;
                        FpSpread2.Sheets[0].Columns.Count = 5;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Columns[0].Width = 50;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Item Name";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Columns[1].Width = 150;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Qty";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Columns[2].Width = 100;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Rate/Qty";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Columns[3].Width = 100;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total Value";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Columns[4].Width = 100;
                        int sno = 0;
                        double dtotalvalue = 0;
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            sno++;
                            FpSpread2.Sheets[0].RowCount++;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["itemname"]);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["itemcode"]);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Consumption_Qty"]);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["RPU"]);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["Consumption_Value"]);
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(concatecode);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            dtotalvalue = dtotalvalue + Convert.ToDouble(ds.Tables[0].Rows[row]["Consumption_Value"]);
                        }
                        FpSpread2.Sheets[0].RowCount++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = "Total";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 4);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dtotalvalue);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                        FpSpread2.Visible = true;
                        Div1.Visible = true;
                        div4.Visible = true;
                    }
                    else
                    {
                        FpSpread2.Visible = false;
                        Div1.Visible = false;
                        div4.Visible = false;
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
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
            string degreedetails = "MESS MONTHLY CONSUMPTION REPORT - NOV 2015";
            string pagename = "HM_mess_monthly_consumption_report.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        try
        {
            surediv.Visible = false;
        }
        catch
        {
        }
    }
    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "MESS DAILY CONSUMPTION REPORT - " + System.DateTime.Now.ToString("MMM") + " " + System.DateTime.Now.ToString("yyyy") + " @ " + datelable.Text + " @ " + lbltotal.Text + "";
            string pagename = "HM_mess_monthly_consumption_report.aspx";
            Printmaster1.loadspreaddetails(FpSpread2, pagename, degreedetails);
            Printmaster1.Visible = true;
        }
        catch
        {
        }
    }
    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = TextBox1.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread2, reportname);
                Label1.Visible = false;
            }
            else
            {
                Label1.Text = "Please Enter Your Report Name";
                Label1.Visible = true;
                TextBox1.Focus();
            }
        }
        catch
        {
        }
    }
    protected void cb_subheadername_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_subheadername.Checked == true)
            {
                for (int i = 0; i < cbl_subheadername.Items.Count; i++)
                {
                    cbl_subheadername.Items[i].Selected = true;
                }
                txt_subheadername.Text = "Sub Header Name(" + (cbl_subheadername.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_subheadername.Items.Count; i++)
                {
                    cbl_subheadername.Items[i].Selected = false;
                }
                txt_subheadername.Text = "--Select--";
            }
            binditem();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_subheadername_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_subheadername.Text = "--Select--";
            cb_subheadername.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_subheadername.Items.Count; i++)
            {
                if (cbl_subheadername.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_subheadername.Text = "Sub Header Name(" + commcount.ToString() + ")";
                if (commcount == cbl_subheadername.Items.Count)
                {
                    cb_subheadername.Checked = true;
                }
            }
            binditem();
        }
        catch (Exception ex)
        {
        }
    }
    public void loadsubheadername()
    {
        try
        {
            cbl_subheadername.Items.Clear();
            string itemheader = "";
            for (int i = 0; i < cbl_headername.Items.Count; i++)
            {
                if (cbl_headername.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_headername.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_headername.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemheader.Trim() != "")
            {
                string query = "";
                query = "select distinct MasterValue,MasterCode from CO_MasterValues c,IM_ItemMaster i where MasterCriteria='Subheader' and c.MasterCode=i.subheader_code and CollegeCode='" + collegecode1 + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                // ds.Clear();
                // ds = d2.BindItemCodeAll(itemheader);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_subheadername.DataSource = ds;
                    cbl_subheadername.DataTextField = "MasterValue";
                    cbl_subheadername.DataValueField = "MasterCode";
                    cbl_subheadername.DataBind();
                    if (cbl_subheadername.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_subheadername.Items.Count; i++)
                        {
                            cbl_subheadername.Items[i].Selected = true;
                        }
                        txt_subheadername.Text = "Sub Header Name(" + cbl_subheadername.Items.Count + ")";
                        // cb_subheadername.Checked = true;
                    }
                    if (cbl_subheadername.Items.Count > 5)
                    {
                        Panel3.Width = 300;
                        Panel3.Height = 300;
                    }
                }
                else
                {
                    txt_subheadername.Text = "--Select--";
                    // cb_subheadername.Checked = false;
                }
            }
            else
            {
                txt_subheadername.Text = "--Select--";
                // cb_subheadername.Checked = false;
            }
        }
        catch
        {
        }
    }
    //11.09.17
    protected void cb_menuname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //txt_itemname.Text = "--Select--";
            //cb_itemname.Checked = false;
            if (cb_menuname.Checked == true)
            {
                for (int i = 0; i < cbl_menuname.Items.Count; i++)
                {
                    cbl_menuname.Items[i].Selected = true;
                }
                txt_menuname.Text = "Menu Name(" + (cbl_menuname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_menuname.Items.Count; i++)
                {
                    cbl_menuname.Items[i].Selected = false;
                }
                txt_menuname.Text = "--Select--";
            }
            //loaditem1();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_menuname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //txt_itemname.Text = "--Select--";
            //cb_itemname.Checked = false;
            //cbl_itemname.Items.Clear();
            int commcount = 0;
            for (int i = 0; i < cbl_menuname.Items.Count; i++)
            {
                if (cbl_menuname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_menuname.Text = "Menu Name(" + commcount.ToString() + ")";
                if (commcount == cbl_menuname.Items.Count)
                {
                    cb_menuname.Checked = true;
                }
            }
            else
            {
                for (int i = 0; i < cbl_menuname.Items.Count; i++)
                {
                    cbl_menuname.Items[i].Selected = false;
                }
                cb_menuname.Checked = false;
                txt_menuname.Text = "--Select--";
            }
            // loaditem1();
        }
        catch (Exception ex)
        {
        }
    }
    public void loadmenuname()
    {
        try
        {
            string firstdate = Convert.ToString(txt_fromdate.Text);
            string firstdate1 = Convert.ToString(txt_todate.Text);
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = firstdate1.Split('/');
            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            string MessMasterFk = rs.GetSelectedItemsValueAsString(cbl_messname);
            string SessionFk = rs.GetSelectedItemsValueAsString(cblsession);
            if (!string.IsNullOrEmpty(MessMasterFk) || !string.IsNullOrEmpty(SessionFk))
            {
                string MenuQry = "  select distinct mm.MenuName,dm.MenumasterFK from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,HM_MenuMaster mm where dm.DailyConsumptionMasterPK=dd.DailyConsumptionMasterFK and dm.MenumasterFK=mm.MenuMasterPK and dm.DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and dm.MessMasterFK in('" + MessMasterFk + "') and dm.sessionfk in('" + SessionFk + "')";
                ds.Clear();
                ds.Reset();
                ds = d2.select_method_wo_parameter(MenuQry, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_menuname.DataSource = ds;
                    cbl_menuname.DataTextField = "MenuName";
                    cbl_menuname.DataValueField = "MenumasterFK";
                    cbl_menuname.DataBind();
                    if (cbl_menuname.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_menuname.Items.Count; i++)
                        {
                            cbl_menuname.Items[i].Selected = true;
                            cb_menuname.Checked = true;
                        }
                        txt_menuname.Text = "Menu Name(" + cbl_menuname.Items.Count + ")";
                        lbl_menuname.Text = "Menu Name";
                    }
                }
                else
                {
                    txt_menuname.Text = "--Select--";
                    cbl_menuname.Items.Clear();
                }
            }
            else
            {
                cbl_menuname.Items.Clear();
                txt_menuname.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void cbsessionCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbsession.Checked == true)
            {
                for (int i = 0; i < cblsession.Items.Count; i++)
                {
                    cblsession.Items[i].Selected = true;
                }
                txtsession.Text = "Session Name(" + (cblsession.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cblsession.Items.Count; i++)
                {
                    cblsession.Items[i].Selected = false;
                }
                txtsession.Text = "--Select--";
            }
            loadmenuname();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cblsessionSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtsession.Text = "--Select--";
            cbsession.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cblsession.Items.Count; i++)
            {
                if (cblsession.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtsession.Text = "Session Name(" + commcount.ToString() + ")";
                if (commcount == cblsession.Items.Count)
                {
                    cbsession.Checked = true;
                }
            }
            loadmenuname();
        }
        catch (Exception ex)
        {
        }
    }
    void bindsession()
    {
        string firstdate = Convert.ToString(txt_fromdate.Text);
        string firstdate1 = Convert.ToString(txt_todate.Text);
        DateTime dt = new DateTime();
        DateTime dt1 = new DateTime();
        string[] split = firstdate.Split('/');
        dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
        split = firstdate1.Split('/');
        dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
        string MessMasterFk = rs.GetSelectedItemsValueAsString(cbl_messname);
        string Qry = " select distinct sm.SessionName,dm.SessionFK from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,HM_SessionMaster sm where dm.DailyConsumptionMasterPK=dd.DailyConsumptionMasterFK and dm.SessionFK=sm.SessionMasterPK and dm.DailyConsDate between  '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and dm.MessMasterFK in('" + MessMasterFk + "') ";
        ds.Clear();
        ds.Reset();
        ds = d2.select_method_wo_parameter(Qry, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cblsession.DataSource = ds;
            cblsession.DataTextField = "SessionName";
            cblsession.DataValueField = "SessionFK";
            cblsession.DataBind();
            if (cblsession.Items.Count > 0)
            {
                for (int i = 0; i < cblsession.Items.Count; i++)
                {
                    cblsession.Items[i].Selected = true;
                    cb_menuname.Checked = true;
                }
                txtsession.Text = "Session Name(" + cblsession.Items.Count + ")";
                lblSession.Text = "Session Name";
            }
        }
        else
        {
            txtsession.Text = "--Select--";
            cblsession.Items.Clear();
        }
    }
    protected void txt_fromdateChanged(object sender, EventArgs e)
    {
        cbsession.Checked = true;
        bindsession();
        loadmenuname();
    }
    protected void txt_todateChanged(object sender, EventArgs e)
    {
        cbsession.Checked = true;
        bindsession();
        loadmenuname();
    }
    protected void visiblity(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
    }
}