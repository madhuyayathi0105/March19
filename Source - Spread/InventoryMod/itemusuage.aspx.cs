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

public partial class itemusuage : System.Web.UI.Page
{
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    bool check = false;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    static string collegecodestat = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    static Hashtable htRowCount = new Hashtable();
    static Hashtable htRowCountreturn = new Hashtable();

    ReuasableMethods rs = new ReuasableMethods();
    private string RPU;
    string semval = "";
    string sqlcmd = string.Empty;
    string txtcode = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ////****************************************************//
            //string PageLogOut = "";
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("invM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["Inventory"].Trim();
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
            ////****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("inventoryindex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/InventoryMod/inventoryindex.aspx");
                    return;
                }
            }
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            lblvalidation1.Text = "";
            if (!IsPostBack)
            {
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_date.Attributes.Add("readonly", "readonly");
                txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate1.Attributes.Add("readonly", "readonly");
                txt_fromdate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate1.Attributes.Add("readonly", "readonly");
                txt_todate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_issuedate.Attributes.Add("readonly", "readonly");
                txt_issuedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_reDate.Attributes.Add("readonly", "readonly");
                txt_reDate.Text = DateTime.Now.ToString("dd/MM/yyyy");


                cext_fromdate.EndDate = DateTime.Now;
                cext_todate.EndDate = DateTime.Now;

                CalendarExtender1.EndDate = DateTime.Now;
                CalendarExtender2.EndDate = DateTime.Now;
                calfrodate.EndDate = DateTime.Now;
                caltodate.EndDate = DateTime.Now;


                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.Visible = false;
                Fpmain.Sheets[0].RowCount = 0;
                Fpmain.Sheets[0].ColumnCount = 0;
                // btn_go_Click(sender, e);
                binddepartment();
                binditem();
                pcolumnorder.Visible = true;

                bindbatch();
                degree();
                string buildvalue1 = "";
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    if (cbl_degree.Items[i].Selected == true)
                    {
                        string build1 = cbl_degree.Items[i].Value.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                }
                bindbranch(buildvalue1);
                BindSection();
                for (int i = 0; i < cbl_section.Items.Count; i++)
                {
                    cbl_section.Items[i].Selected = true;
                }
                txt_section.Text = "Section(" + cbl_section.Items.Count + ")";
                ddl_searchby_onselectedindexchange(sender, e);

                CheckBox_column1.Checked = true;
                LinkButtonsremove_Click1(sender, e);

                rdb_common.Checked = true;
                rdb_Individual_Checkedchange(sender, e);
                bindstore();
                checkSchoolSetting();
            }
        }
        catch (Exception ex) { }
    }

    #region School_or_College
    private double checkSchoolSetting()
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }
    #endregion
    protected void binddepartment()
    {
        try
        {
            ds.Clear();
            cbl_dept.Items.Clear();
            int i = 0;
            ds = d2.loaddepartment(collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_dept.DataSource = ds;
                cbl_dept.DataTextField = "Dept_Name";
                cbl_dept.DataValueField = "Dept_Code";
                cbl_dept.DataBind();
                if (cbl_dept.Items.Count > 0)
                {
                    for (i = 0; i < cbl_dept.Items.Count; i++)
                    {
                        cbl_dept.Items[i].Selected = true;
                    }
                    txt_dept.Text = "Department(" + cbl_dept.Items.Count + ")";
                }
                binditem();
            }
            else
            {
                txt_dept.Text = "--Select--";
            }
        }
        catch
        {

        }
    }
    protected void binditem()
    {
        try
        {
            ds.Clear();
            cbl_itemname.Items.Clear();
            string deptcode = "";
            string storecode = "";
            int i = 0;
            string item = string.Empty;
            if (rbl_Department.Checked == true)
            {
                for (i = 0; i < cbl_dept.Items.Count; i++)
                {
                    if (cbl_dept.Items[i].Selected == true)
                    {
                        if (deptcode == "")
                        {
                            deptcode = "" + cbl_dept.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            deptcode = deptcode + "'" + "," + "'" + cbl_dept.Items[i].Value.ToString() + "";
                        }
                    }
                }

                //string item = "select distinct itemname,ItemPK from IM_ItemMaster im,IM_ItemDeptMaster dm where im.itempk=dm.itemfk and dm.itemdeptfk in ('" + deptcode + "')";
                item = "select distinct itemname,ItemPK from IM_ItemMaster im,IT_StockDeptDetail dm where im.itempk=dm.itemfk and dm.DeptFK in ('" + deptcode + "')";
            }
            else
            {
                for (i = 0; i < cbl_store.Items.Count; i++)
                {
                    if (cbl_store.Items[i].Selected == true)
                    {
                        if (storecode == "")
                        {
                            storecode = "" + cbl_store.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            storecode = storecode + "'" + "," + "'" + cbl_store.Items[i].Value.ToString() + "";
                        }
                    }
                }

                //string item = "select distinct itemname,ItemPK from IM_ItemMaster im,IM_ItemDeptMaster dm where im.itempk=dm.itemfk and dm.itemdeptfk in ('" + deptcode + "')";
                item = "select distinct itemname,itempk from IM_ItemMaster i,IT_StockDetail im where i.ItemPK=im.ItemFK and im.StoreFK in ('" + storecode + "')";


            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_itemname.DataSource = ds;
                cbl_itemname.DataTextField = "itemname";
                cbl_itemname.DataValueField = "ItemPK";
                cbl_itemname.DataBind();
                if (cbl_itemname.Items.Count > 0)
                {
                    for (i = 0; i < cbl_itemname.Items.Count; i++)
                    {
                        cbl_itemname.Items[i].Selected = true;
                    }
                    txt_itemname.Text = "Item (" + cbl_itemname.Items.Count + ")";
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
    protected void cb_dept_CheckedChange(object sender, EventArgs e)
    {
        int cout = 0;
        txt_dept.Text = "--Select--";

        if (cb_dept.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_dept.Items.Count; i++)
            {
                cbl_dept.Items[i].Selected = true;
            }
            txt_dept.Text = "Department(" + (cbl_dept.Items.Count) + ")";
        }

        else
        {
            for (int i = 0; i < cbl_dept.Items.Count; i++)
            {
                cbl_dept.Items[i].Selected = false;
            }
        }
        binditem();
    }
    protected void cbl_dept_SelectedIndexChange(object sender, EventArgs e)
    {
        int i = 0;
        cb_dept.Checked = false;
        //item();
        int commcount = 0;
        txt_dept.Text = "--Select--";
        for (i = 0; i < cbl_dept.Items.Count; i++)
        {
            if (cbl_dept.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_dept.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_dept.Items.Count)
            {
                cb_dept.Checked = true;
            }
            txt_dept.Text = "Department(" + commcount.ToString() + ")";
        }
        binditem();
    }
    protected void cb_itemname_CheckedChange(object sender, EventArgs e)
    {
        int cout = 0;
        txt_itemname.Text = "--Select--";

        if (cb_itemname.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_itemname.Items.Count; i++)
            {
                cbl_itemname.Items[i].Selected = true;
            }
            txt_itemname.Text = "Item(" + (cbl_itemname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_itemname.Items.Count; i++)
            {
                cbl_itemname.Items[i].Selected = false;
            }
        }
        //item();
    }
    protected void cbl_itemname_SelectedIndexChange(object sender, EventArgs e)
    {
        int i = 0;
        cb_itemname.Checked = false;
        //item();
        lblnorecr.Visible = false;
        int commcount = 0;
        txt_itemname.Text = "--Select--";
        for (i = 0; i < cbl_itemname.Items.Count; i++)
        {
            if (cbl_itemname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_itemname.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_itemname.Items.Count)
            {
                cb_itemname.Checked = true;
            }
            txt_itemname.Text = "Item(" + commcount.ToString() + ")";
        }
    }

    protected void ddl_dept_SelectedIndexChange(object sender, EventArgs e)
    {
        binditem1();
    }
    protected void binddept()
    {

        ds = d2.loaddepartment(collegecode1);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_dept.DataSource = ds;
            ddl_dept.DataTextField = "Dept_Name";
            ddl_dept.DataValueField = "Dept_Code";
            ddl_dept.DataBind();
            binditem1();
        }
    }
    protected void binditem1()
    {
        try
        {
            int i = 0;
            cbl_popitm.Items.Clear();
            string item = string.Empty;
            if (rblissue_Wise.SelectedIndex == 0)
            {
                string deptcode = ddl_dept.SelectedItem.Value;
                item = "select distinct itemname,itempk from IM_ItemMaster im,IT_StockDeptDetail sd where im.ItemPK =sd.ItemFK and sd.DeptFK ='" + deptcode + "'";
            }
            else
            {
                string Store = ddl_Store.SelectedItem.Value;
                item = " select distinct itemname,itempk from IM_ItemMaster i,IT_StockDetail im where i.ItemPK=im.ItemFK and im.StoreFK in ('" + Store + "')";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_popitm.DataSource = ds;
                cbl_popitm.DataTextField = "itemname";
                cbl_popitm.DataValueField = "itempk";
                cbl_popitm.DataBind();
                if (cbl_popitm.Items.Count > 0)
                {
                    for (i = 0; i < cbl_popitm.Items.Count; i++)
                    {
                        cbl_popitm.Items[i].Selected = true;
                    }
                    txt_popitm.Text = "Item (" + cbl_popitm.Items.Count + ")";
                }
            }
            else
            {
                txt_popitm.Text = "--Select--";
            }
        }
        catch { }
    }
    protected void cb_popitm_CheckedChange(object sender, EventArgs e)
    {
        int cout = 0;
        txt_popitm.Text = "--Select--";

        if (cb_popitm.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_popitm.Items.Count; i++)
            {
                cbl_popitm.Items[i].Selected = true;
            }
            txt_popitm.Text = "Item(" + (cbl_popitm.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_popitm.Items.Count; i++)
            {
                cbl_popitm.Items[i].Selected = false;
            }
        }
    }
    protected void cbl_popitm_SelectedIndexChange(object sender, EventArgs e)
    {
        int i = 0;
        cb_popitm.Checked = false;
        int commcount = 0;
        txt_popitm.Text = "--Select--";
        for (i = 0; i < cbl_popitm.Items.Count; i++)
        {
            if (cbl_popitm.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_popitm.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_popitm.Items.Count)
            {
                cb_popitm.Checked = true;
            }
            txt_popitm.Text = "Item(" + commcount.ToString() + ")";
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

    #region Go
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            string deptcode = "";
            int i = 0;
            string selectrpquery = "";
            if (rbl_Department.Checked == true)
            {
                for (i = 0; i < cbl_dept.Items.Count; i++)
                {
                    if (cbl_dept.Items[i].Selected == true)
                    {
                        if (deptcode == "")
                        {
                            deptcode = "" + cbl_dept.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            deptcode = deptcode + "'" + "," + "'" + cbl_dept.Items[i].Value.ToString() + "";
                        }
                    }
                }
            }
            else
            {
                for (i = 0; i < cbl_store.Items.Count; i++)
                {
                    if (cbl_store.Items[i].Selected == true)
                    {
                        if (deptcode == "")
                        {
                            deptcode = "" + cbl_store.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            deptcode = deptcode + "'" + "," + "'" + cbl_store.Items[i].Value.ToString() + "";
                        }
                    }
                }
            }
            string itemcode = "";
            for (i = 0; i < cbl_itemname.Items.Count; i++)
            {
                if (cbl_itemname.Items[i].Selected == true)
                {
                    if (itemcode == "")
                    {
                        itemcode = "" + cbl_itemname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemcode = itemcode + "'" + "," + "'" + cbl_itemname.Items[i].Value.ToString() + "";
                    }
                }
            }

            #region Common
            if (rdb_common.Checked == true)
            {
                string uncheck1 = "";
                Printcontrol.Visible = false;
                if (cbl_dept.Items.Count != 0 && cbl_itemname.Items.Count != 0)
                {

                    bool chk1 = false;
                    bool chk2 = false;

                    string dt = txt_fromdate.Text;
                    string yearend = txt_todate.Text;
                    string[] Split = dt.Split('/');
                    DateTime todate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
                    Split = yearend.Split('/');
                    DateTime newdt = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);

                    //sstring selectrpquery = "select im.ItemHeaderName,d.Dept_Code,im.ItemPK,ItemName,SUM(ConsumptionQty) as ConsumptionQty  ,convert(varchar(10), DailyConsDate,103) as DailyConsDate, im.ItemCode,RPU,d.Dept_Name,(ConsumptionQty * RPU) as consumValue from HT_DailyConsumptionDetail dd,HT_DailyConsumptionMaster hm,IM_ItemMaster im,IT_StockDeptDetail sd,Department d where hm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK and dd.ItemFK =im.ItemPK and sd.ItemFK =im.ItemPK and sd.ItemFK=dd.ItemFK and sd.DeptFK=d.Dept_Code and ForMess ='2' and ItemPK in ('" + itemcode + "') and d.Dept_Code in ('" + deptcode + "') and hm.DailyConsDate between '" + todate.ToString() + "' and '" + newdt.ToString() + "' group by DailyConsDate,ItemPK,ItemName,ForMess,im.ItemCode,RPU,d.Dept_Name,(ConsumptionQty * RPU) ,Dept_Code ,ItemHeaderName";
                    if (rbl_Department.Checked == true)
                    {
                        selectrpquery = " select im.ItemHeaderName,d.Dept_Code,im.ItemPK,ItemName,SUM(ConsumptionQty) as ConsumptionQty  ,convert(varchar(10), DailyConsDate,103) as DailyConsDate, im.ItemCode,RPU,d.Dept_Name,(ConsumptionQty * RPU) as consumValue from HT_DailyConsumptionDetail dd,HT_DailyConsumptionMaster hm,IM_ItemMaster im,Department d where hm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK and dd.ItemFK =im.ItemPK and d.Dept_Code=hm.DeptFK  and ForMess ='2' and ItemPK in ('" + itemcode + "') and d.Dept_Code in ('" + deptcode + "') and hm.DailyConsDate between '" + todate.ToString() + "' and '" + newdt.ToString() + "' group by DailyConsDate,ItemPK,ItemName,ForMess,im.ItemCode,RPU,d.Dept_Name,(ConsumptionQty * RPU) ,Dept_Code ,ItemHeaderName";
                    }
                    else
                    {
                        selectrpquery = "  select im.ItemHeaderName,sm.StorePK,im.ItemPK,ItemName,SUM(ConsumptionQty) as ConsumptionQty  ,convert(varchar(10), DailyConsDate,103) as DailyConsDate, im.ItemCode,RPU,sm.StoreName as Dept_Name,(ConsumptionQty * RPU) as consumValue from HT_DailyConsumptionDetail dd,HT_DailyConsumptionMaster hm,IM_ItemMaster im,IM_StoreMaster sm where hm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK and dd.ItemFK =im.ItemPK and sm.StorePK=hm.DeptFK  and ForMess ='3' and ItemPK in ('" + itemcode + "') and  sm.StorePK in ('" + deptcode + "') and hm.DailyConsDate between '" + todate.ToString() + "' and '" + newdt.ToString() + "' group by DailyConsDate,ItemPK,ItemName,ForMess,im.ItemCode,RPU,StoreName,(ConsumptionQty * RPU) ,sm.StorePK ,ItemHeaderName";
                    }

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectrpquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Fpmain.Sheets[0].RowCount = 0;
                        Fpmain.Sheets[0].ColumnCount = 0;
                        Fpmain.CommandBar.Visible = false;
                        Fpmain.Sheets[0].AutoPostBack = false;
                        Fpmain.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpmain.Sheets[0].RowHeader.Visible = false;
                        Fpmain.Sheets[0].ColumnCount = 9;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        Fpmain.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpmain.Columns[0].Width = 50;
                        Fpmain.Columns[0].Locked = true;

                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpmain.Columns[1].Width = 100;
                        Fpmain.Columns[1].Locked = true;
                        Fpmain.Columns[1].Visible = false;

                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Name";
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpmain.Columns[2].Width = 150;
                        Fpmain.Columns[2].Locked = true;
                        Fpmain.Columns[2].Visible = false;

                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item Header Name";
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpmain.Columns[3].Width = 150;
                        Fpmain.Columns[3].Locked = true;
                        Fpmain.Columns[3].Visible = false;

                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Item Code";
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        Fpmain.Columns[4].Width = 150;
                        Fpmain.Columns[4].Locked = true;
                        Fpmain.Columns[4].Visible = false;

                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Item Name";
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        Fpmain.Columns[5].Width = 150;
                        Fpmain.Columns[5].Locked = true;
                        Fpmain.Columns[5].Visible = false;
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Consumption Qty";
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        Fpmain.Columns[6].Width = 110;
                        Fpmain.Columns[6].Locked = true;
                        Fpmain.Columns[6].Visible = false;
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 7].Text = "RPU";
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                        Fpmain.Columns[7].Width = 150;
                        Fpmain.Columns[7].Locked = true;
                        Fpmain.Columns[7].Visible = false;
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Consumption Value";
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                        Fpmain.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                        Fpmain.Columns[8].Width = 100;
                        Fpmain.Columns[8].Locked = true;
                        Fpmain.Columns[8].Visible = false;
                        //Fpmain.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Pay Method";
                        //Fpmain.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                        //Fpmain.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                        //Fpmain.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                        //Fpmain.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                        //Fpmain.Sheets[0].Columns[9].Width = 150;

                        for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Fpmain.Sheets[0].RowCount++;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["DailyConsDate"]);
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemHeaderName"]);
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[i]["ItemPK"]);
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["ConsumptionQty"]);
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["RPU"]);
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["consumValue"]);
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                            //Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["PayMethodvalue"]);
                            //Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                            //Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                            //Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                            chk1 = true;
                            if (chk1 == true)
                            {
                                uncheck1 = "1";
                            }
                        }
                        if (cblcolumnorder.Items.Count > 0)
                        {
                            for (int k = 0; k < cblcolumnorder.Items.Count; k++)
                            {
                                if (cblcolumnorder.Items[k].Selected == true)
                                {
                                    string headername = Convert.ToString(cblcolumnorder.Items[k].ToString());

                                    if (headername == "Consumption Date")
                                    {
                                        Fpmain.Columns[1].Visible = true;
                                    }
                                    if (headername == "Department Name")
                                    {
                                        Fpmain.Columns[2].Visible = true;
                                    }
                                    else if (headername == "Item Header Name")
                                    {
                                        Fpmain.Columns[3].Visible = true;
                                    }
                                    else if (headername == "Item Name")
                                    {
                                        Fpmain.Columns[4].Visible = true;
                                    }
                                    else if (headername == "Item Code")
                                    {
                                        Fpmain.Columns[5].Visible = true;
                                    }
                                    else if (headername == "ConsumptionQty")
                                    {
                                        Fpmain.Columns[6].Visible = true;
                                    }
                                    else if (headername == "Rpu")
                                    {
                                        Fpmain.Columns[7].Visible = true;
                                    }
                                    else if (headername == "Consumption Value")
                                    {
                                        Fpmain.Columns[8].Visible = true;
                                    }
                                    chk2 = true;
                                }
                            }
                        }
                        if (chk2 == false)
                        {
                            CheckBox_column.Checked = true;
                            LinkButtonsremove_Click(sender, e);
                            for (int k = 0; k < cblcolumnorder.Items.Count; k++)
                            {
                                if (cblcolumnorder.Items[k].Selected == true)
                                {
                                    string headername = Convert.ToString(cblcolumnorder.Items[k].ToString());

                                    if (headername == "Consumption Date")
                                    {
                                        Fpmain.Columns[1].Visible = true;
                                    }
                                    if (headername == "Department Name")
                                    {
                                        Fpmain.Columns[2].Visible = true;
                                    }
                                    else if (headername == "Item Header Name")
                                    {
                                        Fpmain.Columns[3].Visible = true;
                                    }
                                    else if (headername == "Item Name")
                                    {
                                        Fpmain.Columns[4].Visible = true;
                                    }
                                    else if (headername == "Item Code")
                                    {
                                        Fpmain.Columns[5].Visible = true;
                                    }
                                    else if (headername == "ConsumptionQty")
                                    {
                                        Fpmain.Columns[6].Visible = true;
                                    }
                                    else if (headername == "Rpu")
                                    {
                                        Fpmain.Columns[7].Visible = true;
                                    }
                                    else if (headername == "Consumption Value")
                                    {
                                        Fpmain.Columns[8].Visible = true;
                                    }
                                }
                            }
                        }
                        Fpmain.Sheets[0].PageSize = Fpmain.Sheets[0].RowCount;
                        Fpmain.SaveChanges();
                        Fpmain.Height = 280;
                        Fpmain.Width = 950;
                        Fpmain.Visible = true;
                        lblnorecr.Visible = false;
                        rptprint.Visible = true;
                    }
                    else
                    {
                        Fpmain.Visible = false;
                        lblnorecr.Visible = true;
                        lblnorecr.Text = "No Records Founds";
                        rptprint.Visible = false;
                    }
                    if (uncheck1.Trim() != "1" && chk1 == true)
                    {
                        Fpmain.Visible = false;
                        rptprint.Visible = false;
                        lblnorecr.Visible = true;
                        lblnorecr.Text = "Please Select All Fields";
                    }
                }
            }
            #endregion

            #region Individual
            if (rdb_Individual.Checked == true)
            {
                string uncheck1 = ""; bool chk1 = false;
                bool chk2 = false;
                DateTime fdate = new DateTime();
                string fromdate = Convert.ToString(txt_fromdate.Text);
                if (fromdate.Trim() != "")
                {
                    string[] split = fromdate.Split('/');
                    fdate = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                }
                DateTime tdate = new DateTime();
                string todate = Convert.ToString(txt_todate.Text);
                if (todate.Trim() != "")
                {
                    string[] split = todate.Split('/');
                    tdate = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                }
                string q1 = " ";

                if (rbl_Department.Checked == true)
                {
                    if (ddl_type.SelectedItem.Value == "0")
                    {
                        q1 = " select r.Stud_Name,r.Roll_No,r.Reg_No, i.ItemHeaderName,d.Dept_Code,i.ItemPK,ItemName,IssuedQuantity,convert(decimal(10,2), quantity)- isnull(IssuedQuantity,0)quantity,convert(varchar(10), IssueDate,103) as AllotDate, i.ItemCode,s.RPU,dt.Dept_Name, appl_name from Indivitual_student_ItemIssue s,Registration r,Degree d,Department dt,Course c,staff_appl_master sa ,im_itemmaster i where i.ItemPK =s.ItemFK and r.App_No=s.App_no and MemType ='1' and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and sa.appl_id =s.IssuedBy and  ISNULL (Issues,'0')='1' and Quantity = ISNULL (issuedQuantity,0) and IssueDate between '" + fdate.ToString("MM/dd/yyyy") + "' and '" + tdate.ToString("MM/dd/yyyy") + "' and s.DeptFK in('" + deptcode + "') and s.ItemFK in('" + itemcode + "')";
                    }
                    else if (ddl_type.SelectedItem.Value == "1")
                    {
                        q1 = "  select  r.Stud_Name,r.Roll_No,r.Reg_No, i.ItemHeaderName,d.Dept_Code,i.ItemPK,ItemName,IssuedQuantity  ,convert(decimal(10,2), quantity)- isnull(IssuedQuantity,0)quantity,convert(varchar(10), IssueDate,103) as AllotDate, i.ItemCode,s.RPU,dt.Dept_Name, appl_name  from Indivitual_student_ItemIssue s,Registration r,Degree d,Department dt,Course c,staff_appl_master sa ,im_itemmaster i where i.ItemPK =s.ItemFK and r.App_No=s.App_no and MemType ='1' and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and sa.appl_id =s.IssuedBy  and  ISNULL (Issues,'0')='1' and Quantity > ISNULL (issuedQuantity,0) and IssueDate between '" + fdate.ToString("MM/dd/yyyy") + "' and '" + tdate.ToString("MM/dd/yyyy") + "' and s.DeptFK in('" + deptcode + "') and s.ItemFK in('" + itemcode + "')";
                    }
                    else if (ddl_type.SelectedItem.Value == "2")
                    {
                        q1 = " select r.Stud_Name,r.Roll_No,r.Reg_No, i.ItemHeaderName,d.Dept_Code,i.ItemPK,ItemName,IssuedQuantity,convert(decimal(10,2), quantity)- isnull(IssuedQuantity,0)quantity,convert(varchar(10), IssueDate,103) as AllotDate, i.ItemCode,s.RPU,dt.Dept_Name  from Indivitual_student_ItemIssue s,Registration r,Degree d,Department dt,Course c ,im_itemmaster i where i.ItemPK =s.ItemFk and r.App_No=s.App_no and MemType ='1' and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and  ISNULL (Issues,'0')='0' and AllotDate between '" + fdate.ToString("MM/dd/yyyy") + "' and '" + tdate.ToString("MM/dd/yyyy") + "' and s.DeptFK in('" + deptcode + "') and s.ItemFK in('" + itemcode + "') ";
                    }
                }
                else
                {
                    if (ddl_type.SelectedItem.Value == "0")
                    {
                        q1 = " select r.Stud_Name,r.Roll_No,r.Reg_No, i.ItemHeaderName,d.Dept_Code,i.ItemPK,ItemName,IssuedQuantity,convert(decimal(10,2), quantity)- isnull(IssuedQuantity,0)quantity,convert(varchar(10), IssueDate,103) as AllotDate, i.ItemCode,s.RPU,dt.Dept_Name, appl_name from Indivitual_student_ItemIssue s,Registration r,Degree d,Department dt,Course c,staff_appl_master sa ,im_itemmaster i where i.ItemPK =s.ItemFK and r.App_No=s.App_no and MemType ='1' and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and sa.appl_id =s.IssuedBy and  ISNULL (Issues,'0')='1' and Quantity = ISNULL (issuedQuantity,0) and IssueDate between '" + fdate.ToString("MM/dd/yyyy") + "' and '" + tdate.ToString("MM/dd/yyyy") + "' and s.DeptFK in('" + deptcode + "') and s.ItemFK in('" + itemcode + "')";
                    }
                    else if (ddl_type.SelectedItem.Value == "1")
                    {
                        q1 = "  select  r.Stud_Name,r.Roll_No,r.Reg_No, i.ItemHeaderName,d.Dept_Code,i.ItemPK,ItemName,IssuedQuantity  ,convert(decimal(10,2), quantity)- isnull(IssuedQuantity,0)quantity,convert(varchar(10), IssueDate,103) as AllotDate, i.ItemCode,s.RPU,dt.Dept_Name, appl_name  from Indivitual_student_ItemIssue s,Registration r,Degree d,Department dt,Course c,staff_appl_master sa ,im_itemmaster i where i.ItemPK =s.ItemFK and r.App_No=s.App_no and MemType ='1' and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and sa.appl_id =s.IssuedBy  and  ISNULL (Issues,'0')='1' and Quantity > ISNULL (issuedQuantity,0) and IssueDate between '" + fdate.ToString("MM/dd/yyyy") + "' and '" + tdate.ToString("MM/dd/yyyy") + "' and s.DeptFK in('" + deptcode + "') and s.ItemFK in('" + itemcode + "')";
                    }
                    else if (ddl_type.SelectedItem.Value == "2")
                    {
                        q1 = " select r.Stud_Name,r.Roll_No,r.Reg_No, i.ItemHeaderName,d.Dept_Code,i.ItemPK,ItemName,IssuedQuantity,convert(decimal(10,2), quantity)- isnull(IssuedQuantity,0)quantity,convert(varchar(10), IssueDate,103) as AllotDate, i.ItemCode,s.RPU,dt.Dept_Name  from Indivitual_student_ItemIssue s,Registration r,Degree d,Department dt,Course c ,im_itemmaster i where i.ItemPK =s.ItemFk and r.App_No=s.App_no and MemType ='1' and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and  ISNULL (Issues,'0')='0' and AllotDate between '" + fdate.ToString("MM/dd/yyyy") + "' and '" + tdate.ToString("MM/dd/yyyy") + "' and s.DeptFK in('" + deptcode + "') and s.ItemFK in('" + itemcode + "') ";

                        q1 += " select distinct Stu_AppNo,r.Stud_Name,r.Roll_No,r.Reg_No, i.ItemHeaderName,d.Dept_Code,i.ItemPK,ItemName,Qty,convert(varchar(10), Date,103) as AllotDate, i.ItemCode,dt.Dept_Name,tsd.InwardRPU  from Indivitual_student_ItemIssue s,Registration r,Degree d,Department dt,Course c ,im_itemmaster i,IM_StudentKit_Details sd,IT_StockDetail tsd where  r.App_No=sd.Stu_AppNo and MemType ='1' and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and sd.Stu_AppNo<>s.App_no and s.itemfk<>sd.ItemFK and tsd.ItemFK=i.ItemPK and tsd.ItemFK=sd.itemfk  and i.ItemPK=sd.itemfk and i.ItemCode=sd.ItemCode and sd.itemfk=tsd.ItemFK and sd.Date  between '" + fdate.ToString("MM/dd/yyyy") + "' and '" + tdate.ToString("MM/dd/yyyy") + "' and s.DeptFK in('" + deptcode + "') and s.ItemFK in('" + itemcode + "') ";
                    }


                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables.Count > 0)
                {
                    string header = "S.No-50/Allot Date-120/Roll No-150/Reg No-150/Student Name-200/Item Code-150/Item Name-150/Department-200/Quantity-100/Issued Qty-100/Rpu-100";///Consumption Value-100/Sailing Prize-100
                    Fpreadheaderbindmethod(header, Fpmain, "false");
                    for (i = 0; i < Fpmain.Columns.Count; i++)
                    {
                        Fpmain.Columns[i].Visible = false;
                        Fpmain.Columns[i].Locked = true;
                    }
                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Fpmain.Sheets[0].RowCount++;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["AllotDate"]);
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["reg_no"]);
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 3].CellType = txt;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                            if (ddl_type.SelectedItem.Value == "1")
                            {
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["Quantity"]);
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                            }
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[i]["IssuedQuantity"]);
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Right;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(ds.Tables[0].Rows[i]["rpu"]);
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Right;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                            Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";

                            chk1 = true;
                            if (chk1 == true)
                            {
                                uncheck1 = "1";
                            }
                        }
                    }
                    if (ddl_type.SelectedItem.Value == "2")
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                            {
                                Fpmain.Sheets[0].RowCount++;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(j + 1);
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[1].Rows[j]["AllotDate"]);
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[1].Rows[j]["Roll_No"]);
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[1].Rows[j]["reg_no"]);
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 3].CellType = txt;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[1].Rows[j]["Stud_Name"]);
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[1].Rows[j]["ItemCode"]);
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[1].Rows[j]["ItemName"]);
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[1].Rows[j]["Dept_Name"]);
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                if (ddl_type.SelectedItem.Value == "1")
                                {
                                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["Quantity"]);
                                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
                                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                                }
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[1].Rows[j]["Qty"]);
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Right;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(ds.Tables[1].Rows[j]["InwardRPU"]);
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Right;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                                Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";

                                chk1 = true;
                                if (chk1 == true)
                                {
                                    uncheck1 = "1";
                                }
                            }
                        }
                    }
                    if (cblcolumnorder3.Items.Count > 0)
                    {
                        for (int k = 0; k < cblcolumnorder3.Items.Count; k++)
                        {
                            if (cblcolumnorder3.Items[k].Selected == true)
                            {
                                string headername = Convert.ToString(cblcolumnorder3.Items[k].ToString());
                                if (headername == "Allot Date")
                                {
                                    Fpmain.Columns[1].Visible = true;
                                }
                                if (headername == "Roll No")
                                {
                                    Fpmain.Columns[2].Visible = true;
                                }
                                else if (headername == "Reg No")
                                {
                                    Fpmain.Columns[3].Visible = true;
                                }
                                else if (headername == "Student Name")
                                {
                                    Fpmain.Columns[4].Visible = true;
                                }
                                else if (headername == "Item Code")
                                {
                                    Fpmain.Columns[5].Visible = true;
                                }
                                else if (headername == "Item Name")
                                {
                                    Fpmain.Columns[6].Visible = true;
                                }
                                else if (headername == "Department Name")
                                {
                                    Fpmain.Columns[7].Visible = true;
                                }
                                else if (headername == "Issued Qty")
                                {
                                    Fpmain.Columns[9].Visible = true;
                                }
                                else if (headername == "Rpu")
                                {
                                    Fpmain.Columns[10].Visible = true;
                                }
                                Fpmain.Columns[0].Visible = true;
                                chk2 = true;
                            }
                        }
                    }
                    if (chk2 == false)
                    {
                        CheckBox_column3.Checked = true;
                        LinkButtonsremove_Click3(sender, e);
                        for (int k = 0; k < cblcolumnorder3.Items.Count; k++)
                        {
                            string headername = Convert.ToString(cblcolumnorder3.Items[k].ToString());
                            if (headername == "Allot Date")
                            {
                                Fpmain.Columns[1].Visible = true;
                            }
                            if (headername == "Roll No")
                            {
                                Fpmain.Columns[2].Visible = true;
                            }
                            else if (headername == "Student Name")
                            {
                                Fpmain.Columns[4].Visible = true;
                            }
                            else if (headername == "Item Name")
                            {
                                Fpmain.Columns[6].Visible = true;
                            }
                            else if (headername == "Department Name")
                            {
                                Fpmain.Columns[7].Visible = true;
                            }
                            else if (headername == "Issued Qty")
                            {
                                Fpmain.Columns[9].Visible = true;
                            }
                            else if (headername == "Rpu")
                            {
                                Fpmain.Columns[10].Visible = true;
                            }
                            Fpmain.Columns[0].Visible = true;
                        }
                    }
                    if (ddl_type.SelectedItem.Value == "1")
                    {
                        Fpmain.Columns[8].Visible = true;
                    }
                    else
                    {
                        Fpmain.Columns[8].Visible = false;
                    }
                    Fpmain.Sheets[0].PageSize = Fpmain.Sheets[0].RowCount;
                    Fpmain.SaveChanges();
                    Fpmain.Height = 280;
                    Fpmain.Width = 950;
                    Fpmain.Visible = true;
                    lblnorecr.Visible = false;
                    rptprint.Visible = true;
                }
                else
                {
                    Fpmain.Visible = false;
                    lblnorecr.Visible = true;
                    lblnorecr.Text = "No Records Founds";
                    rptprint.Visible = false;
                }
                if (uncheck1.Trim() != "1" && chk1 == true)
                {
                    Fpmain.Visible = false;
                    rptprint.Visible = false;
                    lblnorecr.Visible = true;
                    lblnorecr.Text = "Please Select All Fields";
                }
            }

            #endregion
        }
        catch (Exception ex)
        {
            lblnorecr.Visible = true;
            lblnorecr.Text = ex.Message.ToString();
        }
    }
    #endregion

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpmain, reportname);
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
            string degreedetails = "Item Usage";
            string pagename = "itemusuage.aspx";
            Printcontrol.loadspreaddetails(Fpmain, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }

    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            int itemsave = 0;
            int dailyc = 0;
            int itemsup = 0;
            string dept = ddl_dept.SelectedValue;
            bool chk = false;
            //string itemcode = "";
            //for (int i = 0; i < cbl_popitm.Items.Count; i++)
            //{
            //    if (cbl_popitm.Items[i].Selected == true)
            //    {
            //        if (itemcode == "")
            //        {
            //            itemcode = "" + cbl_popitm.Items[i].Value.ToString() + "";
            //        }
            //        else
            //        {
            //            itemcode = itemcode + "'" + "," + "'" + cbl_popitm.Items[i].Value.ToString() + "";
            //        }
            //    }
            //}
            string dt = txt_date.Text;
            string[] Split = dt.Split('/');
            DateTime date = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
            Fpspread1.SaveChanges();
            if (txt_popitm.Text.Trim() != "--Select--")
            {
                string setrpu = d2.GetFunction("select value from Master_Settings where settings='Consumption Rpu' and usercode='" + usercode + "'");
                if (Fpspread1.Sheets[0].RowCount > 0)
                {
                    for (int row = 0; row < Fpspread1.Sheets[0].RowCount; row++)
                    {
                        //string sdeptRPU = Convert.ToString(Fpspread1.Sheets[0].Cells[row, 7].Text);
                        string itemcode = Convert.ToString(Fpspread1.Sheets[0].Cells[row, 1].Text);
                        string uqty = Convert.ToString(Fpspread1.Sheets[0].Cells[row, 9].Text);
                        string sailingprize = Convert.ToString(Fpspread1.Sheets[0].Cells[row, 8].Text);
                        if (sailingprize.Trim() == "")
                        {
                            sailingprize = "0";
                        }
                        string RPU = "";

                        if (rblissue_Wise.SelectedIndex == 0)
                        {
                            if (setrpu.Trim() == "0")
                            {
                                RPU = d2.GetFunction("select AVG(IssuedRPU) Avg_rpu from IT_StockDeptDetail where ItemFK='" + itemcode + "'");
                            }
                            if (setrpu.Trim() == "1")
                            {
                                RPU = d2.GetFunction("select AVG(Sailing_prize) Avg_rpu from IT_StockDeptDetail where ItemFK='" + itemcode + "'");
                            }
                            if (uqty.Trim() != "")
                            {
                                string savedcqr = "if not exists (select * from HT_DailyConsumptionMaster where DailyConsDate='" + date.ToString() + "' and ForMess='2' and DeptFK='" + Convert.ToString(ddl_dept.SelectedItem.Value) + "') insert into HT_DailyConsumptionMaster (DailyConsDate,ForMess,UserCode,DeptFK)values('" + date.ToString() + "','2','" + usercode + "','" + Convert.ToString(ddl_dept.SelectedItem.Value) + "')";
                                dailyc = d2.update_method_wo_parameter(savedcqr, "Text");

                                string savedc = d2.GetFunction("select DailyConsumptionMasterPK from HT_DailyConsumptionMaster where DailyConsDate='" + date.ToString() + "' and ForMess='2' and DeptFK='" + Convert.ToString(ddl_dept.SelectedItem.Value) + "'");

                                string savequery = "if exists (select * from HT_DailyConsumptionDetail where Itemfk ='" + itemcode + "' and DailyConsumptionMasterFK='" + savedc + "') update HT_DailyConsumptionDetail set ConsumptionQty=ConsumptionQty+'" + uqty + "',RPU='" + RPU + "' where Itemfk ='" + itemcode + "' and DailyConsumptionMasterFK='" + savedc + "' else insert into HT_DailyConsumptionDetail (ItemFK,ConsumptionQty,RPU,DailyConsumptionMasterFK) values ('" + itemcode + "','" + uqty + "','" + RPU + "','" + savedc + "')";
                                itemsave = d2.update_method_wo_parameter(savequery, "Text");
                                string upquery = "update IT_StockDeptDetail set UsedQty =ISNULL (UsedQty,0) +'" + uqty + "' ,BalQty =ISNULL(BalQty,0) -'" + uqty + "',Sailing_prize='" + sailingprize + "' where ItemFK ='" + itemcode + "' and DeptFK ='" + dept + "' ";
                                itemsup = d2.update_method_wo_parameter(upquery, "Text");
                                chk = true;
                            }
                        }
                        else
                        {
                            string Str = Convert.ToString(ddl_Store.SelectedItem.Value);
                            if (setrpu.Trim() == "0")
                            {
                                RPU = d2.GetFunction("select AVG(InwardRPU) Avg_rpu from IT_StockDetail where ItemFK='" + itemcode + "'");
                            }
                            if (setrpu.Trim() == "1")
                            {
                                RPU = d2.GetFunction("select AVG(Sailing_prize) Avg_rpu from IT_StockDetail where ItemFK='" + itemcode + "'");
                            }
                            if (uqty.Trim() != "")
                            {
                                string savedqr = "if not exists (select * from HT_DailyConsumptionMaster where DailyConsDate='" + date.ToString() + "' and ForMess='3' and DeptFK='" + Str + "') insert into HT_DailyConsumptionMaster (DailyConsDate,ForMess,UserCode,DeptFK)values('" + date.ToString() + "','3','" + usercode + "','" + Str + "')";
                                dailyc = d2.update_method_wo_parameter(savedqr, "Text");

                                string saved = d2.GetFunction("select DailyConsumptionMasterPK from HT_DailyConsumptionMaster where DailyConsDate='" + date.ToString() + "' and ForMess='3' and DeptFK='" + Str + "'");

                                string savequry = "if exists (select * from HT_DailyConsumptionDetail where Itemfk ='" + itemcode + "' and DailyConsumptionMasterFK='" + saved + "') update HT_DailyConsumptionDetail set ConsumptionQty=ConsumptionQty+'" + uqty + "',RPU='" + RPU + "' where Itemfk ='" + itemcode + "' and DailyConsumptionMasterFK='" + saved + "' else insert into HT_DailyConsumptionDetail (ItemFK,ConsumptionQty,RPU,DailyConsumptionMasterFK) values ('" + itemcode + "','" + uqty + "','" + RPU + "','" + saved + "')";
                                itemsave = d2.update_method_wo_parameter(savequry, "Text");
                                string upqury = "update IT_StockDetail set UsedQty =ISNULL (UsedQty,0) +'" + uqty + "' ,BalQty =ISNULL(BalQty,0) -'" + uqty + "',Sailing_prize='" + sailingprize + "' where ItemFK ='" + itemcode + "' and StoreFK ='" + Str + "' ";
                                itemsup = d2.update_method_wo_parameter(upqury, "Text");
                                chk = true;
                            }

                        }
                    }
                    if (dailyc != 0 && itemsave != 0)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Saved Successfully";
                        btn_go_Click(sender, e);
                    }
                    if (chk == false)
                    {
                        lbl_errmessage.Visible = true;
                        lbl_errmessage.Text = "Please Enter Used Quantity";
                    }
                }
                else
                {

                }
            }
            else
            {
                lbl_errmessage.Visible = true;
                lbl_errmessage.Text = "Please Select All Fields";
            }
        }
        catch (Exception ex)
        {
            lbl_errmessage.Visible = true;
            lbl_errmessage.Text = ex.ToString();
        }
    }
    protected void btnerrclose1_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
        popwindow.Visible = false;
    }
    protected void LinkButtonsremove_Click(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column.Checked == true)
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = true;
                }
            }
            else
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }

    protected void LinkButtonsremove_Click3(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column3.Checked == true)
            {
                for (int i = 0; i < cblcolumnorder3.Items.Count; i++)
                {
                    cblcolumnorder3.Items[i].Selected = true;
                }
            }
            else
            {
                for (int i = 0; i < cblcolumnorder3.Items.Count; i++)
                {
                    cblcolumnorder3.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }

    protected void btn_addnew_click(object sender, EventArgs e)
    {
        try
        {
            if (rdb_Individual.Checked == true)
            {
                Individual_div.Visible = true;
                txt_fromdate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                btn_go2_Click(sender, e);
            }
            else
            {
                popwindow.Visible = true;
                btn_save.Visible = true;
                btn_exit.Visible = true;
                btn_update.Visible = false;
                btn_delete.Visible = false;
                pcolumnorder.Visible = true;
                binddept();
                binditem1();
                Fpspread1.Visible = false;
                btn_save.Visible = false;
                btn_exit.Visible = false;
            }
        }
        catch (Exception ex)
        {
            imgdiv2.Visible = true;
            lbl_alerterr.Visible = true;
            lbl_alerterr.Text = ex.ToString();
        }
    }
    protected void txt_fromdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_fromdate.Text != "" && txt_todate.Text != "")
            {

                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_todate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;

                if (dt > dt1)
                {
                    imgdiv2.Visible = true;
                    lbl_alerterr.Text = "Enter FromDate less than or equal to the ToDate";
                    txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                }
                else
                {

                }
            }
        }
        catch (Exception ex)
        {
        }
        // PopupMessage("Enter FromDate less than or equal to the ToDate", cv_fromtodt1);
    }
    protected void txt_todate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_fromdate.Text != "" && txt_todate.Text != "")
            {

                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_todate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;
                if (dt > dt1)
                {
                    imgdiv2.Visible = true;
                    lbl_alerterr.Text = "Enter ToDate greater than or equal to the FromDate ";
                    txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                }
                else
                {

                }

            }
        }
        catch (Exception ex)
        {
        }

        // PopupMessage("Enter ToDate greater than or equal to the FromDate", cv_fromtodt2);
    }
    protected void btn_errclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;

    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
        Fpspread1.Visible = false;
        btn_save.Visible = false;
        btn_exit.Visible = false;
    }

    protected void btn_go_add_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            string itemcode = "";
            string selectquery = string.Empty;
            for (int i = 0; i < cbl_popitm.Items.Count; i++)
            {
                if (cbl_popitm.Items[i].Selected == true)
                {
                    if (itemcode == "")
                    {
                        itemcode = "" + cbl_popitm.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemcode = itemcode + "'" + "," + "'" + cbl_popitm.Items[i].Value.ToString() + "";
                    }
                }
            }

            if (rblissue_Wise.SelectedIndex == 0)
            {
                string dpt = ddl_dept.SelectedItem.Value;
                //UsedQty,,UsedValue
                selectquery = "select ItemPK,ItemHeaderCode,ItemHeaderName,ItemCode,ItemName,ISNULL(IssuedQty,0)-ISNULL(UsedQty,0) BalQty,IssuedRPU from IM_ItemMaster im,IT_StockDeptDetail sd where im.ItemPK =sd.ItemFK  and im.ItemPK in ('" + itemcode + "') and sd.DeptFK ='" + dpt + "' ";
            }
            else
            {
                string stor = ddl_Store.SelectedItem.Value;
                //UsedQty,,UsedValue
                selectquery = "select ItemPK,ItemHeaderCode,ItemHeaderName,ItemCode,ItemName,ISNULL(InwardQty,0)-ISNULL(UsedQty,0) BalQty,InwardRPU as IssuedRPU from IM_ItemMaster im,IT_StockDetail sd where im.ItemPK =sd.ItemFK  and im.ItemPK in ('" + itemcode + "') and sd.StoreFK ='" + stor + "' ";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Fpspread1.Visible = true;
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = false;
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.Sheets[0].ColumnCount = 10;
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
                Fpspread1.Sheets[0].Columns[0].Locked = true;

                Fpspread1.Sheets[0].Columns[1].Visible = false;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "ItemPK";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[1].Width = 100;
                Fpspread1.Sheets[0].Columns[1].Locked = true;

                Fpspread1.Sheets[0].Columns[2].Visible = false;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "ItemHeaderCode";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[2].Width = 150;
                Fpspread1.Sheets[0].Columns[2].Locked = true;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "ItemHeaderName";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[3].Width = 130;
                Fpspread1.Sheets[0].Columns[3].Locked = true;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "ItemCode";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[4].Width = 100;
                Fpspread1.Sheets[0].Columns[4].Locked = true;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "ItemName";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[4].Width = 100;
                Fpspread1.Sheets[0].Columns[5].Locked = true;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "BalQty";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[6].Locked = true;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "IssuedRPU";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[7].Width = 100;
                Fpspread1.Sheets[0].Columns[7].Locked = true;

                //FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                //cb.AutoPostBack = true;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Sailing Prize";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "UsedQty";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                FarPoint.Web.Spread.DoubleCellType num = new FarPoint.Web.Spread.DoubleCellType();
                num.ErrorMessage = "Enter Only Numbers";
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["ItemPK"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["ItemHeaderCode"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["ItemHeaderName"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["ItemCode"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["ItemName"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["BalQty"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[row]["IssuedRPU"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].BackColor = Color.Bisque;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].CellType = num;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].BackColor = Color.Bisque;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].CellType = num;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                }
                Fpspread1.Visible = true;
                lbl_errmessage.Visible = false;
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                btn_save.Visible = true;
                btn_exit.Visible = true;
            }
            else
            {

                Fpspread1.Visible = false;
                btn_save.Visible = false;
                btn_exit.Visible = false;
                lbl_errmessage.Visible = true;
                lbl_errmessage.Text = "No Records Founds";
            }
            //}
            //}
        }
        catch (Exception ex)
        {
            lbl_errmessage.Visible = true;
            lbl_errmessage.Text = ex.ToString();
        }
    }

    protected void btn_exit_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
        Fpspread1.Visible = false;
        btn_save.Visible = false;
        btn_exit.Visible = false;
    }
    //17.06.16
    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {
        Individual_div.Visible = false;
    }

    protected void cb_batch_checkedchange(object sender, EventArgs e)
    {
        chkchange(cb_batch, cbl_batch, txt_batch, "Batch");
        degree();
    }

    protected void cbl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklstchange(cb_batch, cbl_batch, txt_batch, "Batch");
        degree();
    }

    protected void cb_degree_checkedchange(object sender, EventArgs e)
    {
        chkchange(cb_degree, cbl_degree, txt_degree, "Degree");
        string buildvalue1 = "";
        for (int i = 0; i < cbl_degree.Items.Count; i++)
        {
            if (cbl_degree.Items[i].Selected == true)
            {
                string build1 = cbl_degree.Items[i].Value.ToString();
                if (buildvalue1 == "")
                {
                    buildvalue1 = build1;
                }
                else
                {
                    buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                }
            }
        }
        bindbranch(buildvalue1);
    }

    protected void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklstchange(cb_degree, cbl_degree, txt_degree, "Degree");
        string buildvalue1 = "";
        for (int i = 0; i < cbl_degree.Items.Count; i++)
        {
            if (cbl_degree.Items[i].Selected == true)
            {
                string build1 = cbl_degree.Items[i].Value.ToString();
                if (buildvalue1 == "")
                {
                    buildvalue1 = build1;
                }
                else
                {
                    buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                }
            }
        }
        bindbranch(buildvalue1);
    }

    protected void cb_branch_checkedchange(object sender, EventArgs e)
    {
        chkchange(cb_branch, cbl_branch, txt_branch, "Branch");
        string buildvalue1 = "";
        for (int i = 0; i < cbl_branch.Items.Count; i++)
        {
            if (cbl_branch.Items[i].Selected == true)
            {
                string build1 = cbl_branch.Items[i].Value.ToString();
                if (buildvalue1 == "")
                {
                    buildvalue1 = build1;
                }
                else
                {
                    buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                }
            }
        }
    }

    protected void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklstchange(cb_branch, cbl_branch, txt_branch, "Branch");
        string buildvalue1 = "";
        for (int i = 0; i < cbl_branch.Items.Count; i++)
        {
            if (cbl_branch.Items[i].Selected == true)
            {
                string build1 = cbl_branch.Items[i].Value.ToString();
                if (buildvalue1 == "")
                {
                    buildvalue1 = build1;
                }
                else
                {
                    buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                }
            }
        }
    }

    protected void cb_section_checkedchange(object sender, EventArgs e)
    {
        chkchange(cb_section, cbl_section, txt_section, "Section");
    }

    protected void cbl_section_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklstchange(cb_section, cbl_section, txt_section, "Section");
    }

    protected void chkchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            if (chkchange.Checked == true)
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = true;
                }
                txtchange.Text = label + "(" + Convert.ToString(chklstchange.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = false;
                }
                txtchange.Text = "--Select--";
            }
        }
        catch { }
    }

    protected void chklstchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            txtchange.Text = "--Select--";
            chkchange.Checked = false;
            int count = 0;
            for (int i = 0; i < chklstchange.Items.Count; i++)
            {
                if (chklstchange.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txtchange.Text = label + "(" + count + ")";
                if (count == chklstchange.Items.Count)
                {
                    chkchange.Checked = true;
                }
            }
        }
        catch { }
    }

    public void bindbatch()
    {
        try
        {
            cbl_batch.Items.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
            }
            int count = 0;
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                cbl_batch.Items[i].Selected = true;
                if (cbl_batch.Items[i].Selected == true)
                {
                    txt_batch.Text = "Batch(" + Convert.ToString(cbl_batch.Items.Count) + ")";
                    cb_batch.Checked = true;
                }
            }
        }
        catch
        {
        }
    }

    public void degree()
    {
        try
        {
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Clear();
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode1);
            hat.Add("user_code", usercode);
            ds.Clear();
            ds = d2.select_method("bind_degree", hat, "sp");
            int count1 = ds.Tables[0].Rows.Count;
            cbl_degree.Items.Clear();
            if (count1 > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
            }
            int count = 0;
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                cbl_degree.Items[i].Selected = true;
                if (cbl_degree.Items[i].Selected == true)
                {
                    txt_degree.Text = "Degree(" + Convert.ToString(cbl_degree.Items.Count) + ")";
                    cb_degree.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindbranch(string branch)
    {
        try
        {
            cbl_branch.Items.Clear();
            string commname = "";
            if (branch != "")
            {
                commname = "select distinct department.Dept_Code,degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";

                ds = d2.select_method(commname, hat, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch.DataSource = ds;
                    cbl_branch.DataTextField = "dept_name";
                    cbl_branch.DataValueField = "Dept_Code";
                    cbl_branch.DataBind();
                }
                int count = 0;
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = true;
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        txt_branch.Text = "Branch(" + Convert.ToString(cbl_branch.Items.Count) + ")";
                    }
                }
            }
            else
            {
                txt_branch.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void BindSection()
    {
        cbl_section.Items.Clear();
        string q1 = "select distinct Sections from Registration where Sections<>'' order by Sections";
        ds.Clear();
        ds = d2.select_method_wo_parameter(q1, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_section.DataSource = ds;
            cbl_section.DataTextField = "Sections";
            cbl_section.DataValueField = "Sections";
            cbl_section.DataBind();
        }
    }

    #region IndividualGo_popup
    protected void btn_go2_Click(object sender, EventArgs e)
    {
        try
        {
            string uncheck1 = "";
            string batchyear = getvalues(cbl_batch);
            string branch = getvalues(cbl_branch);
            string section = getvalues(cbl_section);
            string courseid = getvalues(cbl_degree);
            string kitcode = getvalues(cbl_kitname);
            string stud_name = Convert.ToString(txt_studentname.Text);
            string Rollno = Convert.ToString(txt_roll.Text);
            string regno = Convert.ToString(txt_reg.Text);
            string appno = Convert.ToString(txt_app.Text);
            string qrydate = "";
            string header = "S.No-50/Select-50/Roll No-150/Reg No-150/Student Name-200/Item Code-150/Item Name-150/Department-200/Balance Qty-100/Quantity-100/Rpu-100/Issued Qty-100";
            Fpreadheaderbindmethod(header, FpSpread2, "false");

            DateTime fdate = new DateTime();
            string fromdate = Convert.ToString(txt_fromdate1.Text);
            if (fromdate.Trim() != "")
            {
                string[] split = fromdate.Split('/');
                fdate = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            }
            DateTime tdate = new DateTime();
            string todate = Convert.ToString(txt_todate1.Text);
            if (todate.Trim() != "")
            {
                string[] split = todate.Split('/');
                tdate = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            }

            string q1 = "";
            if (txt_section.Text.Trim() != "--Select--")
            {
                section = " and r.Sections in ('" + section + "',' ')";
            }
            if (rbl_kit.Checked == true && cb_fromto.Checked == true || rbl_retuen.Checked == true && cb_fromto.Checked == true)
            {
                qrydate = "and sd.Date between '" + fdate.ToString("MM/dd/yyyy") + "' and '" + tdate.ToString("MM/dd/yyyy") + "'";
            }

            #region IndividualStudent
            if (rbl_individualstudent.Checked == true)
            {
                if (txt_studentname.Text.Trim() != "")
                {
                    string[] stdname = stud_name.Split('-');
                    string sname = stdname[0];
                    q1 = " select CONVERT(varchar(10), allotdate,103)allotdate,iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName,d.Dept_Name, ledgerfk,iu.itemfk,iu.deptfk,convert(decimal(18,2), quantity)- isnull(IssuedQuantity,0) quantity,rpu,sum(s.BalQty)as BalQty from Indivitual_student_ItemIssue iu,IT_StockDeptDetail s,IM_ItemMaster i,Registration r,Department d where iu.ItemFK=i.ItemPK and r.App_No=iu.App_no and d.Dept_Code=iu.DeptFK and  r.stud_name='" + sname + "' and iu.ItemFK=s.ItemFK and iu.DeptFK=s.DeptFK  and not (Quantity=isnull(IssuedQuantity,0)) and isnull(iu.kit,0)<>'1' group by iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName,d.Dept_Name, ledgerfk,iu.itemfk,iu.deptfk,rpu,iu.allotdate,iu.IssuedQuantity,iu.Quantity";

                    q1 += "select CONVERT(varchar(10), allotdate,103)allotdate,iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName, ledgerfk,iu.itemfk,iu.deptfk,convert(decimal(18,2), quantity)- isnull(IssuedQuantity,0) quantity,rpu,sum(s.BalQty)as BalQty,sm.StoreName as Dept_Name from Indivitual_student_ItemIssue iu,IT_StockDetail s,IM_ItemMaster i,Registration r,IM_StoreMaster sm where iu.ItemFK=i.ItemPK and r.App_No=iu.App_no  and sm.StorePK=s.StoreFK and sm.StorePK=iu.DeptFK and  r.stud_name='" + sname + "' and iu.ItemFK=s.ItemFK and iu.DeptFK=s.StoreFK  and not (Quantity=isnull(IssuedQuantity,0)) and isnull(iu.kit,0)<>'1' group by iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName, ledgerfk,iu.itemfk,iu.deptfk,rpu,iu.allotdate,iu.IssuedQuantity,iu.Quantity,sm.StoreName ";

                }
                else if (txt_roll.Text.Trim() != "")
                {
                    q1 = " select CONVERT(varchar(10), allotdate,103)allotdate,iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName,d.Dept_Name, ledgerfk,iu.itemfk,iu.deptfk,convert(decimal(18,2), quantity)- isnull(IssuedQuantity,0) quantity,rpu,sum(s.BalQty)as BalQty from Indivitual_student_ItemIssue iu,IT_StockDeptDetail s,IM_ItemMaster i,Registration r,Department d where iu.ItemFK=i.ItemPK and r.App_No=iu.App_no and d.Dept_Code=iu.DeptFK and   r.Roll_No='" + Rollno + "' and iu.ItemFK=s.ItemFK and iu.DeptFK=s.DeptFK  and not (Quantity=isnull(IssuedQuantity,0)) and isnull(iu.kit,0)<>'1' group by iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName,d.Dept_Name, ledgerfk,iu.itemfk,iu.deptfk,rpu,iu.allotdate,iu.IssuedQuantity,iu.Quantity";
                    q1 += " select CONVERT(varchar(10), allotdate,103)allotdate,iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName, ledgerfk,iu.itemfk,iu.deptfk,convert(decimal(18,2), quantity)- isnull(IssuedQuantity,0) quantity,rpu,sum(s.BalQty)as BalQty,sm.StoreName as Dept_Name from Indivitual_student_ItemIssue iu,IT_StockDetail s,IM_ItemMaster i,Registration r,IM_StoreMaster sm where iu.ItemFK=i.ItemPK and r.App_No=iu.App_no  and sm.StorePK=s.StoreFK and sm.StorePK=iu.DeptFK and  r.Roll_No='" + Rollno + "' and iu.ItemFK=s.ItemFK and iu.DeptFK=s.StoreFK  and not (Quantity=isnull(IssuedQuantity,0)) and isnull(iu.kit,0)<>'1' group by iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName, ledgerfk,iu.itemfk,iu.deptfk,rpu,iu.allotdate,iu.IssuedQuantity,iu.Quantity,sm.StoreName ";

                }
                else if (txt_reg.Text.Trim() != "")
                {
                    q1 = " select CONVERT(varchar(10), allotdate,103)allotdate,iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName,d.Dept_Name, ledgerfk,iu.itemfk,iu.deptfk,convert(decimal(18,2), quantity)- isnull(IssuedQuantity,0) quantity,rpu,sum(s.BalQty)as BalQty from Indivitual_student_ItemIssue iu,IT_StockDeptDetail s,IM_ItemMaster i,Registration r,Department d where iu.ItemFK=i.ItemPK and r.App_No=iu.App_no and d.Dept_Code=iu.DeptFK and  r.reg_no='" + regno + "' and iu.ItemFK=s.ItemFK and iu.DeptFK=s.DeptFK  and not (Quantity=isnull(IssuedQuantity,0)) and isnull(iu.kit,0)<>'1' group by iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName,d.Dept_Name, ledgerfk,iu.itemfk,iu.deptfk,rpu,iu.allotdate,iu.IssuedQuantity,iu.Quantity";

                    q1 += " select CONVERT(varchar(10), allotdate,103)allotdate,iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName, ledgerfk,iu.itemfk,iu.deptfk,convert(decimal(18,2), quantity)- isnull(IssuedQuantity,0) quantity,rpu,sum(s.BalQty)as BalQty,sm.StoreName as Dept_Name from Indivitual_student_ItemIssue iu,IT_StockDetail s,IM_ItemMaster i,Registration r,IM_StoreMaster sm where iu.ItemFK=i.ItemPK and r.App_No=iu.App_no  and sm.StorePK=s.StoreFK and sm.StorePK=iu.DeptFK and  r.reg_no='" + regno + "' and iu.ItemFK=s.ItemFK and iu.DeptFK=s.StoreFK  and not (Quantity=isnull(IssuedQuantity,0)) and isnull(iu.kit,0)<>'1' group by iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName, ledgerfk,iu.itemfk,iu.deptfk,rpu,iu.allotdate,iu.IssuedQuantity,iu.Quantity,sm.StoreName ";
                }
                else if (txt_app.Text.Trim() != "")
                {
                    string appno1 = d2.GetFunction("select r.App_No from Registration r,applyn a where r.App_No=a.app_no and app_formno='" + Convert.ToString(txt_app.Text.Trim()) + "'");

                    q1 = " select CONVERT(varchar(10), allotdate,103)allotdate,iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName,d.Dept_Name, ledgerfk,iu.itemfk,iu.deptfk,convert(decimal(18,2), quantity)- isnull(IssuedQuantity,0) quantity,rpu,sum(s.BalQty)as BalQty from Indivitual_student_ItemIssue iu,IT_StockDeptDetail s,IM_ItemMaster i,Registration r,Department d where iu.ItemFK=i.ItemPK and r.App_No=iu.App_no and d.Dept_Code=iu.DeptFK and  r.app_no='" + appno1 + "' and iu.ItemFK=s.ItemFK and iu.DeptFK=s.DeptFK  and not (Quantity=isnull(IssuedQuantity,0)) and isnull(iu.kit,0)<>'1' group by iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName,d.Dept_Name, ledgerfk,iu.itemfk,iu.deptfk,rpu,iu.allotdate,iu.IssuedQuantity,iu.Quantity";

                    q1 += "  select CONVERT(varchar(10), allotdate,103)allotdate,iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName, ledgerfk,iu.itemfk,iu.deptfk,convert(decimal(18,2), quantity)- isnull(IssuedQuantity,0) quantity,rpu,sum(s.BalQty)as BalQty,sm.StoreName as Dept_Name from Indivitual_student_ItemIssue iu,IT_StockDetail s,IM_ItemMaster i,Registration r,IM_StoreMaster sm where iu.ItemFK=i.ItemPK and r.App_No=iu.App_no  and sm.StorePK=s.StoreFK and sm.StorePK=iu.DeptFK and r.app_no='" + appno1 + "' and iu.ItemFK=s.ItemFK and iu.DeptFK=s.StoreFK  and not (Quantity=isnull(IssuedQuantity,0)) and isnull(iu.kit,0)<>'1' group by iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName, ledgerfk,iu.itemfk,iu.deptfk,rpu,iu.allotdate,iu.IssuedQuantity,iu.Quantity,sm.StoreName ";

                }
                else
                {
                    //q1 = "select CONVERT(varchar(10), allotdate,103)allotdate,iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName,d.Dept_Name, ledgerfk,itemfk,deptfk,convert(decimal(18,2), quantity)- isnull(IssuedQuantity,0) quantity,rpu from Indivitual_student_ItemIssue iu,IM_ItemMaster i,Registration r,Department d,IT_StockDeptDetail s where iu.ItemFK=i.ItemPK and r.App_No=iu.App_no and d.Dept_Code=iu.DeptFK and iu.ItemFK=s.ItemFK and iu.DeptFK=s.DeptFK  " + section + " and r.Batch_Year in('" + batchyear + "') and d.Dept_Code in('" + branch + "') and AllotDate between '" + fdate.ToString("MM/dd/yyyy") + "' and '" + tdate.ToString("MM/dd/yyyy") + "' and not (Quantity=isnull(IssuedQuantity,0))";

                    q1 = " select CONVERT(varchar(10), allotdate,103)allotdate,iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName,d.Dept_Name, ledgerfk,iu.itemfk,iu.deptfk,convert(decimal(18,2), quantity)- isnull(IssuedQuantity,0) quantity,rpu,sum(s.BalQty)as BalQty from Indivitual_student_ItemIssue iu,IT_StockDeptDetail s,IM_ItemMaster i,Registration r,Department d where iu.ItemFK=i.ItemPK and r.App_No=iu.App_no and d.Dept_Code=iu.DeptFK and r.Batch_Year in('" + batchyear + "') and d.Dept_Code in('" + branch + "') and AllotDate between '" + fdate.ToString("MM/dd/yyyy") + "' and '" + tdate.ToString("MM/dd/yyyy") + "' and iu.ItemFK=s.ItemFK and iu.DeptFK=s.DeptFK  and not (Quantity=isnull(IssuedQuantity,0)) and isnull(iu.kit,0)<>'1' group by iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName,d.Dept_Name, ledgerfk,iu.itemfk,iu.deptfk,rpu,iu.allotdate,iu.IssuedQuantity,iu.Quantity";

                    q1 += "  select CONVERT(varchar(10), allotdate,103)allotdate,iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName, ledgerfk,iu.itemfk,iu.deptfk,convert(decimal(18,2), quantity)- isnull(IssuedQuantity,0) quantity,rpu,sum(s.BalQty)as BalQty,sm.StoreName as Dept_Name from Indivitual_student_ItemIssue iu,IT_StockDetail s,IM_ItemMaster i,Registration r,IM_StoreMaster sm where iu.ItemFK=i.ItemPK and r.App_No=iu.App_no  and sm.StorePK=s.StoreFK and sm.StorePK=iu.DeptFK and  r.Batch_Year in('" + batchyear + "')  and AllotDate between '" + fdate.ToString("MM/dd/yyyy") + "' and '" + tdate.ToString("MM/dd/yyyy") + "' and iu.ItemFK=s.ItemFK and iu.DeptFK=s.StoreFK  and not (Quantity=isnull(IssuedQuantity,0)) and isnull(iu.kit,0)<>'1' group by iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName, ledgerfk,iu.itemfk,iu.deptfk,rpu,iu.allotdate,iu.IssuedQuantity,iu.Quantity,sm.StoreName ";
                    //select CONVERT(varchar(10), allotdate,103)allotdate,iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName,d.Dept_Name, ledgerfk,iu.itemfk,iu.deptfk,convert(decimal(18,2), quantity)- isnull(IssuedQuantity,0) quantity,rpu,sum(s.BalQty)as BalQty from Indivitual_student_ItemIssue iu,IT_StockDetail s,IM_ItemMaster i,Registration r,Department d,IM_StoreMaster sm where iu.ItemFK=i.ItemPK and r.App_No=iu.App_no and sm.StorePK=iu.DeptFK and sm.StorePK=s.StoreFK and r.Batch_Year in('2018','2017','2016','2015','2014') and d.Dept_Code in('45','46','47','48','49','52','50','53','55','58','60','61','63','59','57','62','51','54','52','64','66','59') and AllotDate between '03/29/2018' and '03/29/2018' and iu.ItemFK=s.ItemFK and iu.DeptFK=s.StoreFK  and not (Quantity=isnull(IssuedQuantity,0)) group by iu.app_no,r.Roll_No,r.reg_no,r.Stud_Name,i.ItemCode,i.ItemName,d.Dept_Name, ledgerfk,iu.itemfk,iu.deptfk,rpu,iu.allotdate,iu.IssuedQuantity,iu.Quantity
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                if (txt_batch.Text.Trim() != "--Select--" && txt_degree.Text.Trim() != "--Select--" && txt_branch.Text.Trim() != "")
                {
                    if (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0)
                    {
                        bool chk1 = false;
                        bool chk2 = false;
                        FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                        FarPoint.Web.Spread.DoubleCellType num = new FarPoint.Web.Spread.DoubleCellType();
                        num.ErrorMessage = "Enter Only Numbers";
                        cb.AutoPostBack = false;
                        int sno = 0;
                        for (int i = 0; i < FpSpread2.Columns.Count; i++)
                        {
                            FpSpread2.Columns[i].Visible = false;
                            if (i != 1 && i != 10 && i != 11)
                            {
                                FpSpread2.Columns[i].Locked = true;
                            }
                            else
                            {
                                FpSpread2.Columns[i].Locked = false;
                            }
                        }

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                FpSpread2.Sheets[0].RowCount++;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = cb;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["reg_no"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].CellType = txt;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["ledgerfk"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[i]["itemfk"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(ds.Tables[0].Rows[i]["deptfk"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                                //BalQty

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["BalQty"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";


                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[i]["quantity"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(ds.Tables[0].Rows[i]["rpu"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].BackColor = Color.Bisque;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].Text = "";
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].CellType = num;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].BackColor = Color.Bisque;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";

                                chk1 = true;
                                if (chk1 == true)
                                {
                                    uncheck1 = "1";
                                }
                                sno = i + 1;
                            }
                        }

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                            {
                                FpSpread2.Sheets[0].RowCount++;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno + 1);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = cb;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[1].Rows[j]["Roll_No"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[1].Rows[j]["app_no"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[1].Rows[j]["reg_no"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].CellType = txt;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[1].Rows[j]["Stud_Name"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[1].Rows[j]["ledgerfk"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[1].Rows[j]["ItemCode"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[1].Rows[j]["itemfk"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[1].Rows[j]["ItemName"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[1].Rows[j]["Dept_Name"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(ds.Tables[1].Rows[j]["deptfk"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                                //BalQty

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[1].Rows[j]["BalQty"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";


                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[1].Rows[j]["quantity"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(ds.Tables[1].Rows[j]["rpu"]);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].BackColor = Color.Bisque;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].Text = "";
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].CellType = num;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].BackColor = Color.Bisque;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";

                                chk1 = true;
                                if (chk1 == true)
                                {
                                    uncheck1 = "1";
                                }
                                sno++;
                            }
                        }

                        #region columnorder
                        if (cblcolumnorder1.Items.Count > 0)
                        {
                            for (int k = 0; k < cblcolumnorder1.Items.Count; k++)
                            {
                                if (cblcolumnorder1.Items[k].Selected == true)
                                {
                                    string headername = Convert.ToString(cblcolumnorder1.Items[k].ToString());

                                    if (headername == "Roll No")
                                    {
                                        FpSpread2.Columns[2].Visible = true;
                                    }
                                    else if (headername == "Reg No")
                                    {
                                        FpSpread2.Columns[3].Visible = true;
                                    }
                                    else if (headername == "Student Name")
                                    {
                                        FpSpread2.Columns[4].Visible = true;
                                    }
                                    else if (headername == "Item Code")
                                    {
                                        FpSpread2.Columns[5].Visible = true;
                                    }
                                    else if (headername == "Item Name")
                                    {
                                        FpSpread2.Columns[6].Visible = true;
                                    }
                                    else if (headername == "Department Name")
                                    {
                                        FpSpread2.Columns[7].Visible = true;
                                    }
                                    else if (headername == "Balance Qty")
                                    {
                                        FpSpread2.Columns[8].Visible = true;
                                    }
                                    else if (headername == "Rpu")
                                    {
                                        FpSpread2.Columns[9].Visible = true;
                                    }
                                    chk2 = true;
                                }
                            }
                        }
                        if (chk2 == false)
                        {
                            CheckBox_column1.Checked = true;
                            LinkButtonsremove_Click1(sender, e);
                            for (int k = 0; k < cblcolumnorder1.Items.Count; k++)
                            {
                                //if (cblcolumnorder.Items[k].Selected == true)
                                //{
                                string headername = Convert.ToString(cblcolumnorder1.Items[k].ToString());

                                if (headername == "Roll No")
                                {
                                    FpSpread2.Columns[2].Visible = true;
                                }
                                if (headername == "Reg No")
                                {
                                    FpSpread2.Columns[3].Visible = true;
                                }
                                else if (headername == "Student Name")
                                {
                                    FpSpread2.Columns[4].Visible = true;
                                }
                                else if (headername == "Item Code")
                                {
                                    FpSpread2.Columns[5].Visible = true;
                                }
                                else if (headername == "Item Name")
                                {
                                    FpSpread2.Columns[6].Visible = true;
                                }
                                else if (headername == "Department Name")
                                {
                                    FpSpread2.Columns[7].Visible = true;
                                }
                                else if (headername == "Balance Qty")
                                {
                                    FpSpread2.Columns[8].Visible = true;
                                }
                                else if (headername == "Rpu")
                                {
                                    FpSpread2.Columns[9].Visible = true;
                                }
                                //}
                            }
                        }
                        #endregion

                        FpSpread2.Columns[0].Visible = true;
                        FpSpread2.Columns[1].Visible = true;
                        FpSpread2.Columns[10].Visible = true;
                        FpSpread2.Columns[11].Visible = true;
                        FpSpread2.Visible = true;
                        FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                        FpSpread2.Sheets[0].FrozenColumnCount = 5;
                        saveperson_table.Visible = true;
                        return_row.Visible = false;
                        lbl_erro2.Visible = false;
                    }
                    else
                    {
                        FpSpread2.Visible = false;
                        saveperson_table.Visible = false;
                        lbl_erro2.Visible = true;
                        lbl_erro2.Text = "No Records Found";
                    }
                    if (uncheck1.Trim() != "1")
                    {
                        FpSpread2.Visible = false;
                        saveperson_table.Visible = false;
                        lbl_erro2.Visible = true;
                        lbl_erro2.Text = "No Records Found";
                    }
                }
                else
                {
                    FpSpread2.Visible = false;
                    saveperson_table.Visible = false;
                    lbl_erro2.Visible = true;
                    lbl_erro2.Text = "Please Select All Fields";
                }

                txt_app.Text = "";
                txt_reg.Text = "";
                txt_roll.Text = "";
                txt_studentname.Text = "";
            }
            #endregion


            #region KitStudent

            else if (rbl_kit.Checked == true)
            {
                if (txt_studentname.Text.Trim() != "")
                {
                    string[] stdname1 = stud_name.Split('-');
                    string sdname = stdname1[0];
                    //q1 = "  select distinct sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty from IM_KitMaster km,IM_StudentKit_Details sd,IM_ItemMaster i,Registration r,CO_MasterValues mv where km.CollegeCode=r.college_code and km.CollegeCode=mv.CollegeCode and sd.ItemCode=i.ItemCode and sd.Stu_AppNo=r.App_No and km.ItemCode=sd.ItemCode and km.ItemCode=i.ItemCode    and km.KitCode=mv.MasterCode and sd.KitCode=mv.MasterCode and km.CollegeCode=mv.CollegeCode and   r.stud_name='" + stud_name + "' and mv.MasterCode in('" + kitcode + "') ";
                    q1 = "select distinct sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,sum(se.BalQty)as BalQty,sm.StorePK,i.ItemPK,se.InwardRPU  from IM_KitMaster km,IM_StudentKit_Details sd,IM_ItemMaster i,Registration r,CO_MasterValues mv,IT_StockDetail se where km.CollegeCode=r.college_code and km.CollegeCode=mv.CollegeCode and sd.ItemCode=i.ItemCode and sd.Stu_AppNo=r.App_No and km.ItemCode=sd.ItemCode and km.ItemCode=i.ItemCode    and km.KitCode=mv.MasterCode and sd.KitCode=mv.MasterCode and km.CollegeCode=mv.CollegeCode and se.ItemFK=i.ItemPK and i.StoreFK=sm.StorePK and   r.stud_name='" + sdname + "' and mv.MasterCode in('" + kitcode + "')     group by sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,BalQty,sm.StorePK,i.ItemPK,se.InwardRPU ";

                }
                else if (txt_roll.Text.Trim() != "")
                {
                    //q1 = "select distinct sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty  from registration r,degree de,course c,department dp,IM_KitMaster km,IM_StudentKit_Details sd,IM_ItemMaster i,CO_MasterValues mv  where km.CollegeCode=r.college_code and km.CollegeCode=mv.CollegeCode and sd.ItemCode=i.ItemCode and sd.Stu_AppNo=r.App_No and km.ItemCode=sd.ItemCode and km.ItemCode=i.ItemCode    and km.KitCode=mv.MasterCode and sd.KitCode=mv.MasterCode and km.CollegeCode=mv.CollegeCode and r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar' and r.batch_year in('" + batchyear + "') and  c.Course_Id in('" + courseid + "')  and r.degree_code in('" + branch + "')  and r.Sections in('" + section + "') and mv.MasterCode in('"+kitcode+"')  and  r.Roll_No='" + Rollno + "' group by r.roll_no,r.Stud_Name,r.App_No";
                    q1 = "select distinct sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,sum(se.BalQty)as BalQty,sm.StorePK,i.ItemPK,se.InwardRPU, mv.MasterCode,sd.Date   from IM_KitMaster km,IM_StudentKit_Details sd,IM_ItemMaster i,Registration r,CO_MasterValues mv,IT_StockDetail se,IM_StoreMaster sm where km.CollegeCode=r.college_code and km.CollegeCode=mv.CollegeCode and sd.ItemCode=i.ItemCode and sd.Stu_AppNo=r.App_No and km.ItemCode=sd.ItemCode and km.ItemCode=i.ItemCode    and km.KitCode=mv.MasterCode and sd.KitCode=mv.MasterCode and km.CollegeCode=mv.CollegeCode and se.ItemFK=i.ItemPK  and sm.StorePK=se.StoreFK  and i.StoreFK=sm.StorePK and  r.Roll_No='" + Rollno + "'  and mv.MasterCode in('" + kitcode + "')     group by sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,BalQty,sm.StorePK,i.ItemPK,se.InwardRPU, mv.MasterCode,sd.Date  ";

                }
                else if (txt_reg.Text.Trim() != "")
                {
                    //q1 = "select distinct sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty  from registration r,degree de,course c,department dp,IM_KitMaster km,IM_StudentKit_Details sd,IM_ItemMaster i,CO_MasterValues mv  where km.CollegeCode=r.college_code and km.CollegeCode=mv.CollegeCode and sd.ItemCode=i.ItemCode and sd.Stu_AppNo=r.App_No and km.ItemCode=sd.ItemCode and km.ItemCode=i.ItemCode    and km.KitCode=mv.MasterCode and sd.KitCode=mv.MasterCode and km.CollegeCode=mv.CollegeCode and r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar' and r.batch_year in('" + batchyear + "') and  c.Course_Id in('" + courseid + "')  and r.degree_code in('" + branch + "')  and r.Sections in('" + section + "') and mv.MasterCode in('"+kitcode+"')  and  r.reg_no='" + regno + "' group by r.roll_no,r.Stud_Name,r.App_No";
                    q1 = "select distinct sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,sum(se.BalQty)as BalQty,sm.StorePK,i.ItemPK,se.InwardRPU,mv.MasterCode,sd.Date   from IM_KitMaster km,IM_StudentKit_Details sd,IM_ItemMaster i,Registration r,CO_MasterValues mv,IT_StockDetail se,,IM_StoreMaster sm where km.CollegeCode=r.college_code and km.CollegeCode=mv.CollegeCode and sd.ItemCode=i.ItemCode and sd.Stu_AppNo=r.App_No and km.ItemCode=sd.ItemCode and km.ItemCode=i.ItemCode    and km.KitCode=mv.MasterCode and sd.KitCode=mv.MasterCode and km.CollegeCode=mv.CollegeCode and se.ItemFK=i.ItemPK and sm.StorePK=se.StoreFK and i.StoreFK=sm.StorePK and  r.reg_no='" + regno + "'  and mv.MasterCode in('" + kitcode + "')    group by sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,BalQty,sm.StorePK,i.ItemPK,se.InwardRPU, mv.MasterCode,sd.date ";


                }
                else if (txt_app.Text.Trim() != "")
                {
                    string appno1 = d2.GetFunction("select r.App_No from Registration r,applyn a where r.App_No=a.app_no and app_formno='" + Convert.ToString(txt_app.Text.Trim()) + "'");

                    //q1 = "select distinct sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty  from registration r,degree de,course c,department dp,IM_KitMaster km,IM_StudentKit_Details sd,IM_ItemMaster i,CO_MasterValues mv  where km.CollegeCode=r.college_code and km.CollegeCode=mv.CollegeCode and sd.ItemCode=i.ItemCode and sd.Stu_AppNo=r.App_No and km.ItemCode=sd.ItemCode and km.ItemCode=i.ItemCode    and km.KitCode=mv.MasterCode and sd.KitCode=mv.MasterCode and km.CollegeCode=mv.CollegeCode and r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar' and r.batch_year in('" + batchyear + "') and  c.Course_Id in('" + courseid + "')  and r.degree_code in('" + branch + "')  and r.Sections in('" + section + "') and mv.MasterCode in('"+kitcode+"')  and r.app_no='" + appno1 + "' group by r.roll_no,r.Stud_Name,r.App_No";
                    q1 = "select distinct sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,sum(se.BalQty)as BalQty,sm.StorePK,i.ItemPK,se.InwardRPU,mv.MasterCode,sd.Date  from IM_KitMaster km,IM_StudentKit_Details sd,IM_ItemMaster i,Registration r,CO_MasterValues mv,IT_StockDetail se,IM_StoreMaster sm  where km.CollegeCode=r.college_code and km.CollegeCode=mv.CollegeCode and sd.ItemCode=i.ItemCode and sd.Stu_AppNo=r.App_No and km.ItemCode=sd.ItemCode and km.ItemCode=i.ItemCode    and km.KitCode=mv.MasterCode and sd.KitCode=mv.MasterCode and km.CollegeCode=mv.CollegeCode and se.ItemFK=i.ItemPK and sm.StorePK=se.StoreFK and i.StoreFK=sm.StorePK  and r.app_no='" + appno1 + "'  and mv.MasterCode in('" + kitcode + "')     group by sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,BalQty,sm.StorePK,i.ItemPK,se.InwardRPU, mv.MasterCode,sd.Date ";

                }
                else
                {
                    q1 = "  select distinct sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,sum(se.BalQty)as BalQty,sm.StorePK,i.ItemPK,se.InwardRPU,mv.MasterCode,sd.Date   from registration r,degree de,course c,department dp,IM_KitMaster km,IM_StudentKit_Details sd,IM_ItemMaster i,CO_MasterValues mv,IT_StockDetail se,IM_StoreMaster sm   where km.CollegeCode=r.college_code and km.CollegeCode=mv.CollegeCode and sd.ItemCode=i.ItemCode and sd.Stu_AppNo=r.App_No and km.ItemCode=sd.ItemCode and km.ItemCode=i.ItemCode    and km.KitCode=mv.MasterCode and sd.KitCode=mv.MasterCode and km.CollegeCode=mv.CollegeCode and r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar'  and se.ItemFK=i.ItemPK and sm.StorePK=se.StoreFK and i.StoreFK=sm.StorePK and r.batch_year in('" + batchyear + "') and  c.Course_Id in('" + courseid + "')  and dp.Dept_Code in('" + branch + "')  " + section + " and mv.MasterCode in('" + kitcode + "') " + qrydate + "  group by sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,BalQty,sm.StorePK,i.ItemPK,se.InwardRPU, mv.MasterCode,sd.Date ";

                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                if (txt_batch.Text.Trim() != "--Select--" && txt_degree.Text.Trim() != "--Select--" && txt_branch.Text.Trim() != "")
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataView dv = new DataView();
                        bool chk3 = false;
                        bool fspreadv = false;
                        FpSpread2.Sheets[0].RowCount = 1;
                        FpSpread2.Sheets[0].ColumnCount = 11;
                        FpSpread2.CommandBar.Visible = false;
                        FpSpread2.Sheets[0].AutoPostBack = false;
                        FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread2.Sheets[0].RowHeader.Visible = false;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;



                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Columns[0].Locked = true;
                        FpSpread2.Columns[0].Width = 50;
                        FpSpread2.Columns[0].Visible = false;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Columns[1].Width = 50;
                        FpSpread2.Columns[1].Visible = false;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[2].Locked = true;
                        FpSpread2.Columns[2].Width = 100;
                        FpSpread2.Columns[2].Visible = false;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[3].Locked = true;
                        FpSpread2.Columns[3].Width = 100;
                        FpSpread2.Columns[3].Visible = false;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[4].Locked = true;
                        FpSpread2.Columns[4].Width = 100;
                        FpSpread2.Columns[4].Visible = false;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Kit Name";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[5].Locked = true;
                        FpSpread2.Columns[5].Width = 100;
                        FpSpread2.Columns[5].Visible = false;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Item Code";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[6].Locked = true;
                        FpSpread2.Columns[6].Width = 100;
                        FpSpread2.Columns[6].Visible = false;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Item Name";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[7].Locked = true;
                        FpSpread2.Columns[7].Width = 150;
                        FpSpread2.Columns[7].Visible = false;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Avaiable Qty";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[8].Locked = true;
                        FpSpread2.Columns[8].Width = 80;
                        FpSpread2.Columns[8].Visible = false;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Alloted Qty";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[9].Locked = true;
                        FpSpread2.Columns[9].Width = 50;
                        FpSpread2.Columns[9].Visible = false;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Issued Qty";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[10].Locked = true;
                        FpSpread2.Columns[10].Width = 50;
                        FpSpread2.Columns[10].Visible = false;


                        FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
                        FarPoint.Web.Spread.CheckBoxCellType chk1 = new FarPoint.Web.Spread.CheckBoxCellType();
                        FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                        FarPoint.Web.Spread.DoubleCellType num1 = new FarPoint.Web.Spread.DoubleCellType();
                        num1.ErrorMessage = "Enter Only Numbers";
                        chk1.AutoPostBack = true;
                        chkall.AutoPostBack = false;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = chk1;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        int sno = 0;
                        DataSet dsind = new DataSet();
                        DataSet dspaidamt = new DataSet();
                        Hashtable htkit = new Hashtable();
                        htRowCount.Clear();
                        double amt = 0;
                        string qrypaid = string.Empty;
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            string app_no = Convert.ToString(ds.Tables[0].Rows[row]["Stu_AppNo"]).Trim();
                            string kcode = Convert.ToString(ds.Tables[0].Rows[row]["MasterCode"]).Trim();
                            string allotdate1 = Convert.ToString(ds.Tables[0].Rows[row]["Date"]).Trim();

                            int rowcnt = 0;
                            string paid = "";
                            string headerid = d2.GetFunction("select headerid from inventorykit where collegecode='" + collegecode1 + "' and usercode='" + usercode + "' and kitid='" + kcode + "'");
                            string ledgerid = d2.GetFunction("select ledgerid from inventorykit where collegecode='" + collegecode1 + "' and usercode='" + usercode + "' and kitid='" + kcode + "'");
                            if (headerid != "" && headerid != "0" && ledgerid != "" && ledgerid != "0")
                            {
                                string selsctfeecate = d2.GetFunction("select distinct current_semester from registration r where r.App_No='" + app_no + "' and r.college_code in('" + collegecode1 + "')");

                                if (checkSchoolSetting() == 0)
                                {
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
                                if (semval != "")
                                {
                                    sqlcmd = d2.GetFunction("select distinct textcode from textvaltable where textcriteria ='FEECA' and textval='" + semval + "' and college_code='" + collegecode1 + "'");
                                    if (sqlcmd != "0" && sqlcmd != "")
                                        txtcode = Convert.ToString(sqlcmd);
                                }
                                if (txtcode != "0")
                                    paid = d2.GetFunction("select BalAmount from ft_feeallot where app_no='" + app_no + "' and LedgerFK='" + ledgerid + "' and HeaderFK='" + headerid + "' and FeeCategory='" + txtcode + "' and AllotDate='" + allotdate1 + "'");
                                amt = Convert.ToDouble(paid);
                                if (amt == 0.00)
                                {
                                    if (!htkit.Contains(app_no))
                                    {

                                        htkit.Add(app_no, "");
                                        ds.Tables[0].DefaultView.RowFilter = "Stu_AppNo ='" + app_no + "'";
                                        dv = ds.Tables[0].DefaultView;
                                        if (dv.Count > 0)
                                        {
                                            int rowCnt = 0;
                                            for (int row1 = 0; row1 < dv.Count; row1++)
                                            {
                                                if (row1 == 0)
                                                    rowCnt = FpSpread2.Sheets[0].RowCount;
                                                string rollno = Convert.ToString(dv[row1]["roll_no"]).Trim();
                                                string st_app_no = Convert.ToString(dv[row1]["Stu_AppNo"]).Trim();
                                                string regnum = Convert.ToString(dv[row1]["reg_no"]).Trim();
                                                string stuname = Convert.ToString(dv[row1]["Stud_Name"]).Trim();
                                                string kitname = Convert.ToString(dv[row1]["MasterValue"]).Trim();
                                                string itmcode = Convert.ToString(dv[row1]["ItemCode"]).Trim();
                                                string itmname = Convert.ToString(dv[row1]["ItemName"]).Trim();
                                                string Qty = Convert.ToString(dv[row1]["Qty"]).Trim();
                                                string BalQty = Convert.ToString(dv[row1]["BalQty"]).Trim();
                                                string strpk = Convert.ToString(dv[row1]["StorePK"]).Trim();
                                                string itmpk = Convert.ToString(dv[row1]["ItemPK"]).Trim();
                                                string rpu = Convert.ToString(dv[row1]["InwardRPU"]).Trim();
                                                string kitcod = Convert.ToString(dv[row1]["MasterCode"]).Trim();
                                                string allotdate = Convert.ToString(dv[row1]["date"]).Trim();

                                                string[] avaqty = BalQty.Split('.');
                                                string avaquty = avaqty[0];
                                                string indiv = "select * from Indivitual_student_ItemIssue where App_no='" + app_no + "' and ItemFK='" + itmpk + "' and    (Quantity=isnull(IssuedQuantity,0)) and kit='1' ";
                                                dsind.Clear();
                                                dsind = d2.select_method_wo_parameter(indiv, "TEXT");

                                                if (dsind.Tables[0].Rows.Count == 0)
                                                {
                                                    FpSpread2.Sheets[0].RowCount++;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = chkall;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].CellType = txtCell;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = txtCell;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].CellType = txtCell;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].CellType = txtCell;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].CellType = txtCell;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].CellType = txtCell;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].CellType = num1;

                                                    if (sno == 0)
                                                        sno = 1;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = rollno;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag = st_app_no;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = regnum;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Tag = strpk;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = stuname;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Tag = itmpk;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = kitname;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Tag = kitcod;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = itmcode;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Tag = rpu;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Text = itmname;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Tag = allotdate;

                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = avaquty;
                                                    if (avaquty == "0")
                                                    {
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("Red");
                                                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 9);
                                                    }

                                                    string Allotqty = "";
                                                    string quantiy = d2.GetFunction("select convert(decimal(18,2), quantity)- isnull(IssuedQuantity,0) quantity from Indivitual_student_ItemIssue where App_no='" + app_no + "'  and itemFK='" + itmpk + "'  and  not (Quantity=isnull(IssuedQuantity,0)) and kit='1' ");
                                                    if (quantiy != "" && quantiy != "0")
                                                    {
                                                        string[] quantity = quantiy.Split('.');
                                                        Allotqty = quantity[0];
                                                    }
                                                    else
                                                        Allotqty = Qty;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Text = Allotqty;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Text = Allotqty;



                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Left;


                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].BackColor = Color.Bisque;

                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Locked = true;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Locked = false;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Locked = true;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Locked = true;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Locked = true;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Locked = true;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Locked = true;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Locked = true;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Locked = true;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Locked = true;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Locked = false;
                                                    rowcnt++;
                                                }
                                            }
                                            if (rowcnt > 0)
                                            {
                                                FpSpread2.Sheets[0].SpanModel.Add(rowCnt, 0, rowcnt, 1);
                                                FpSpread2.Sheets[0].SpanModel.Add(rowCnt, 1, rowcnt, 1);
                                                FpSpread2.Sheets[0].SpanModel.Add(rowCnt, 2, rowcnt, 1);
                                                FpSpread2.Sheets[0].SpanModel.Add(rowCnt, 3, rowcnt, 1);
                                                FpSpread2.Sheets[0].SpanModel.Add(rowCnt, 4, rowcnt, 1);
                                                htRowCount.Add(app_no, rowcnt);
                                                sno++;

                                            }

                                        }
                                    }
                                    fspreadv = true;
                                }
                            }
                        }

                        if (!fspreadv)
                        {
                            FpSpread2.Visible = false;
                            saveperson_table.Visible = false;
                        }
                        else
                        {
                            #region columnorder
                            if (cblcolumnorder2.Items.Count > 0)
                            {
                                FpSpread2.Columns[0].Visible = true;
                                FpSpread2.Columns[1].Visible = true;
                                FpSpread2.Columns[10].Visible = true;
                                for (int k1 = 0; k1 < cblcolumnorder2.Items.Count; k1++)
                                {
                                    if (cblcolumnorder2.Items[k1].Selected == true)
                                    {
                                        string headername1 = Convert.ToString(cblcolumnorder2.Items[k1].ToString());

                                        if (headername1 == "Roll No")
                                        {
                                            FpSpread2.Columns[2].Visible = true;

                                        }
                                        else if (headername1 == "Reg No")
                                        {
                                            FpSpread2.Columns[3].Visible = true;

                                        }
                                        else if (headername1 == "Student Name")
                                        {
                                            FpSpread2.Columns[4].Visible = true;

                                        }
                                        else if (headername1 == "Kit Name")
                                        {
                                            FpSpread2.Columns[5].Visible = true;

                                        }
                                        else if (headername1 == "Item Code")
                                        {
                                            FpSpread2.Columns[6].Visible = true;

                                        }
                                        else if (headername1 == "Item Name")
                                        {
                                            FpSpread2.Columns[7].Visible = true;

                                        }
                                        else if (headername1 == "Avaiable Qty")
                                        {
                                            FpSpread2.Columns[8].Visible = true;

                                        }
                                        else if (headername1 == "Alloted Qty")
                                        {
                                            FpSpread2.Columns[9].Visible = true;

                                        }
                                        chk3 = true;
                                    }
                                }
                            }
                            if (chk3 == false)
                            {
                                CheckBox_column1.Checked = true;
                                LinkButtonsremove_Click1(sender, e);
                                FpSpread2.Columns[0].Visible = true;
                                FpSpread2.Columns[1].Visible = true;
                                FpSpread2.Columns[10].Visible = true;
                                for (int k2 = 0; k2 < cblcolumnorder2.Items.Count; k2++)
                                {
                                    //if (cblcolumnorder.Items[k].Selected == true)
                                    //{
                                    string headername2 = Convert.ToString(cblcolumnorder2.Items[k2].ToString());

                                    if (headername2 == "Roll No")
                                    {
                                        FpSpread2.Columns[2].Visible = true;
                                    }
                                    if (headername2 == "Reg No")
                                    {
                                        FpSpread2.Columns[3].Visible = true;
                                    }
                                    else if (headername2 == "Student Name")
                                    {
                                        FpSpread2.Columns[4].Visible = true;
                                    }
                                    else if (headername2 == "Kit Name")
                                    {
                                        FpSpread2.Columns[5].Visible = true;
                                    }
                                    else if (headername2 == "Item Code")
                                    {
                                        FpSpread2.Columns[6].Visible = true;
                                    }
                                    else if (headername2 == "Item Name")
                                    {
                                        FpSpread2.Columns[7].Visible = true;
                                    }
                                    else if (headername2 == "Avaiable Qty")
                                    {
                                        FpSpread2.Columns[8].Visible = true;
                                    }
                                    else if (headername2 == "Alloted Qty")
                                    {
                                        FpSpread2.Columns[9].Visible = true;
                                    }

                                    //}
                                }
                            }
                            #endregion

                            FpSpread2.Visible = true;
                            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                            FpSpread2.Sheets[0].FrozenColumnCount = 5;
                            saveperson_table.Visible = true;
                            return_row.Visible = false;
                            lbl_erro2.Visible = false;
                        }
                    }
                    else
                    {
                        FpSpread2.Visible = false;
                        saveperson_table.Visible = false;
                        lbl_erro2.Visible = true;
                        lbl_erro2.Text = "No Records Found";

                    }
                }
                else
                {
                    FpSpread2.Visible = false;
                    saveperson_table.Visible = false;
                    lbl_erro2.Visible = true;
                    lbl_erro2.Text = "Please Select All Fields";

                }
                txt_app.Text = "";
                txt_reg.Text = "";
                txt_roll.Text = "";
                txt_studentname.Text = "";

            }
            #endregion


            #region Student_Kit_Return
            else
            {

                if (txt_studentname.Text.Trim() != "")
                {
                    string[] stdname1 = stud_name.Split('-');
                    string sdname = stdname1[0];
                    q1 = "select distinct sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,sum(se.BalQty)as BalQty,sm.StorePK,i.ItemPK,se.InwardRPU,mv.MasterCode,sd.Date,convert(varchar(10),r.Batch_Year)+'-'+dp.dept_acronym+'-'++'Sem'+convert(varchar(10),r.Current_Semester)+'-'+r.Sections as degree   from registration r,degree de,course c,department dp,IM_KitMaster km,IM_StudentKit_Details sd,IM_ItemMaster i,CO_MasterValues mv,IT_StockDetail se,IM_StoreMaster sm   where km.CollegeCode=r.college_code and km.CollegeCode=mv.CollegeCode and sd.ItemCode=i.ItemCode and sd.Stu_AppNo=r.App_No and km.ItemCode=sd.ItemCode and km.ItemCode=i.ItemCode    and km.KitCode=mv.MasterCode and sd.KitCode=mv.MasterCode and km.CollegeCode=mv.CollegeCode and r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar'  and se.ItemFK=i.ItemPK and sm.StorePK=se.StoreFK and i.StoreFK=sm.StorePK and isi.DeptFK=sm.StorePK and isi.App_no=sd.Stu_AppNo and isi.ItemFK=sd.itemfk and isi.App_no=r.App_No and isi.Issues='1' and Kit='1' and  r.stud_name='" + sdname + "' group by sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,BalQty,sm.StorePK,i.ItemPK,se.InwardRPU, mv.MasterCode,sd.Date,r.Batch_Year,dp.dept_acronym,r.Current_Semester,r.Sections";

                }
                else if (txt_roll.Text.Trim() != "")
                {

                    q1 = "select distinct sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,sum(se.BalQty)as BalQty,sm.StorePK,i.ItemPK,se.InwardRPU,mv.MasterCode,sd.Date from registration r,degree de,course c,department dp,IM_KitMaster km,IM_StudentKit_Details sd,IM_ItemMaster i,CO_MasterValues mv,IT_StockDetail se,IM_StoreMaster sm,Indivitual_student_ItemIssue isi where km.CollegeCode=r.college_code and km.CollegeCode=mv.CollegeCode and sd.ItemCode=i.ItemCode and sd.Stu_AppNo=r.App_No and km.ItemCode=sd.ItemCode and km.ItemCode=i.ItemCode and km.KitCode=mv.MasterCode and sd.KitCode=mv.MasterCode and km.CollegeCode=mv.CollegeCode and r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar'  and se.ItemFK=i.ItemPK and sm.StorePK=se.StoreFK and i.StoreFK=sm.StorePK and isi.DeptFK=sm.StorePK and isi.App_no=sd.Stu_AppNo and isi.ItemFK=sd.itemfk and isi.App_no=r.App_No and isi.Issues='1' and Kit='1' and r.Roll_No='" + Rollno + "'  group by sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,BalQty,sm.StorePK,i.ItemPK,se.InwardRPU, mv.MasterCode,sd.Date";

                }
                else if (txt_reg.Text.Trim() != "")
                {
                    q1 = "select distinct sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,sum(se.BalQty)as BalQty,sm.StorePK,i.ItemPK,se.InwardRPU,mv.MasterCode,sd.Date from registration r,degree de,course c,department dp,IM_KitMaster km,IM_StudentKit_Details sd,IM_ItemMaster i,CO_MasterValues mv,IT_StockDetail se,IM_StoreMaster sm,Indivitual_student_ItemIssue isi where km.CollegeCode=r.college_code and km.CollegeCode=mv.CollegeCode and sd.ItemCode=i.ItemCode and sd.Stu_AppNo=r.App_No and km.ItemCode=sd.ItemCode and km.ItemCode=i.ItemCode and km.KitCode=mv.MasterCode and sd.KitCode=mv.MasterCode and km.CollegeCode=mv.CollegeCode and r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar'  and se.ItemFK=i.ItemPK and sm.StorePK=se.StoreFK and i.StoreFK=sm.StorePK and isi.DeptFK=sm.StorePK and isi.App_no=sd.Stu_AppNo and isi.ItemFK=sd.itemfk and isi.App_no=r.App_No and isi.Issues='1' and Kit='1' and r.reg_no='" + regno + "'   group by sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,BalQty,sm.StorePK,i.ItemPK,se.InwardRPU, mv.MasterCode,sd.Date";
                }
                else if (txt_app.Text.Trim() != "")
                {
                    string appno1 = d2.GetFunction("select r.App_No from Registration r,applyn a where r.App_No=a.app_no and app_formno='" + Convert.ToString(txt_app.Text.Trim()) + "'");

                    q1 = "  select distinct sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,sum(se.BalQty)as BalQty,sm.StorePK,i.ItemPK,se.InwardRPU,mv.MasterCode,sd.Date from registration r,degree de,course c,department dp,IM_KitMaster km,IM_StudentKit_Details sd,IM_ItemMaster i,CO_MasterValues mv,IT_StockDetail se,IM_StoreMaster sm,Indivitual_student_ItemIssue isi where km.CollegeCode=r.college_code and km.CollegeCode=mv.CollegeCode and sd.ItemCode=i.ItemCode and sd.Stu_AppNo=r.App_No and km.ItemCode=sd.ItemCode and km.ItemCode=i.ItemCode    and km.KitCode=mv.MasterCode and sd.KitCode=mv.MasterCode and km.CollegeCode=mv.CollegeCode and r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar'  and se.ItemFK=i.ItemPK and sm.StorePK=se.StoreFK and i.StoreFK=sm.StorePK and isi.DeptFK=sm.StorePK and isi.App_no=sd.Stu_AppNo and isi.ItemFK=sd.itemfk and isi.App_no=r.App_No and isi.Issues='1' and Kit='1' and and r.app_no='" + appno1 + "'   group by sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,BalQty,sm.StorePK,i.ItemPK,se.InwardRPU, mv.MasterCode,sd.Date";
                }
                else
                {

                    q1 = " select distinct sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,sum(se.BalQty)as BalQty,sm.StorePK,i.ItemPK,se.InwardRPU,mv.MasterCode,sd.Date  from registration r,degree de,course c,department dp,IM_KitMaster km,IM_StudentKit_Details sd,IM_ItemMaster i,CO_MasterValues mv,IT_StockDetail se,IM_StoreMaster sm,Indivitual_student_ItemIssue isi  where km.CollegeCode=r.college_code and km.CollegeCode=mv.CollegeCode and sd.ItemCode=i.ItemCode and sd.Stu_AppNo=r.App_No and km.ItemCode=sd.ItemCode and km.ItemCode=i.ItemCode and km.KitCode=mv.MasterCode and sd.KitCode=mv.MasterCode and km.CollegeCode=mv.CollegeCode and r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar'  and se.ItemFK=i.ItemPK and sm.StorePK=se.StoreFK and i.StoreFK=sm.StorePK and isi.DeptFK=sm.StorePK and isi.App_no=sd.Stu_AppNo and isi.ItemFK=sd.itemfk and isi.App_no=r.App_No and isi.Issues='1' and Kit='1' and r.batch_year in('" + batchyear + "') and  c.Course_Id in('" + courseid + "')  and dp.Dept_Code in('" + branch + "') " + section + " and mv.MasterCode in('" + kitcode + "') " + qrydate + " group by sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,BalQty,sm.StorePK,i.ItemPK,se.InwardRPU, mv.MasterCode,sd.Date";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                if (txt_batch.Text.Trim() != "--Select--" && txt_degree.Text.Trim() != "--Select--" && txt_branch.Text.Trim() != "")
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {

                        DataView dv = new DataView();
                        bool chk3 = false;
                        bool fspreadv = false;
                        FpSpread2.Sheets[0].RowCount = 1;
                        FpSpread2.Sheets[0].ColumnCount = 12;
                        FpSpread2.CommandBar.Visible = false;
                        FpSpread2.Sheets[0].AutoPostBack = false;
                        FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread2.Sheets[0].RowHeader.Visible = false;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;



                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Columns[0].Locked = true;
                        FpSpread2.Columns[0].Width = 50;
                        FpSpread2.Columns[0].Visible = false;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Columns[1].Width = 50;
                        FpSpread2.Columns[1].Visible = false;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[2].Locked = true;
                        FpSpread2.Columns[2].Width = 100;
                        FpSpread2.Columns[2].Visible = false;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[3].Locked = true;
                        FpSpread2.Columns[3].Width = 100;
                        FpSpread2.Columns[3].Visible = false;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[4].Locked = true;
                        FpSpread2.Columns[4].Width = 100;
                        FpSpread2.Columns[4].Visible = false;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Kit Name";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[5].Locked = true;
                        FpSpread2.Columns[5].Width = 100;
                        FpSpread2.Columns[5].Visible = false;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Item Code";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[6].Locked = true;
                        FpSpread2.Columns[6].Width = 100;
                        FpSpread2.Columns[6].Visible = false;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Item Name";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[7].Locked = true;
                        FpSpread2.Columns[7].Width = 150;
                        FpSpread2.Columns[7].Visible = false;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Alloted Qty";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[8].Locked = true;
                        FpSpread2.Columns[8].Width = 80;
                        FpSpread2.Columns[8].Visible = false;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Issue Qty";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[9].Locked = true;
                        FpSpread2.Columns[9].Width = 50;
                        FpSpread2.Columns[9].Visible = false;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Balance Qty";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[10].Locked = true;
                        FpSpread2.Columns[10].Width = 50;
                        FpSpread2.Columns[10].Visible = false;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Return Qty";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[11].Locked = true;
                        FpSpread2.Columns[11].Width = 50;
                        FpSpread2.Columns[11].Visible = true;


                        FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
                        FarPoint.Web.Spread.CheckBoxCellType chk2 = new FarPoint.Web.Spread.CheckBoxCellType();
                        FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                        FarPoint.Web.Spread.DoubleCellType num2 = new FarPoint.Web.Spread.DoubleCellType();
                        num2.ErrorMessage = "Enter Only Numbers";
                        chk2.AutoPostBack = true;
                        chkall.AutoPostBack = false;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = chk2;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        int sno = 0;
                        Hashtable htkit1 = new Hashtable();
                        htRowCountreturn.Clear();
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            string sappno = Convert.ToString(ds.Tables[0].Rows[row]["Stu_AppNo"]).Trim();
                            int rowcnt1 = 0;
                            if (!htkit1.Contains(sappno))
                            {

                                htkit1.Add(sappno, "");
                                ds.Tables[0].DefaultView.RowFilter = "Stu_AppNo ='" + sappno + "'";
                                dv = ds.Tables[0].DefaultView;
                                if (dv.Count > 0)
                                {
                                    int rowCnt1 = 0;
                                    for (int row1 = 0; row1 < dv.Count; row1++)
                                    {
                                        if (row1 == 0)
                                            rowCnt1 = FpSpread2.Sheets[0].RowCount;
                                        string rollno = Convert.ToString(dv[row1]["roll_no"]).Trim();
                                        string st_app_no = Convert.ToString(dv[row1]["Stu_AppNo"]).Trim();
                                        string regnum = Convert.ToString(dv[row1]["reg_no"]).Trim();
                                        string stuname = Convert.ToString(dv[row1]["Stud_Name"]).Trim();
                                        string kitname = Convert.ToString(dv[row1]["MasterValue"]).Trim();
                                        string itmcode = Convert.ToString(dv[row1]["ItemCode"]).Trim();
                                        string itmname = Convert.ToString(dv[row1]["ItemName"]).Trim();
                                        string Qty = Convert.ToString(dv[row1]["Qty"]).Trim();
                                        string BalQty = Convert.ToString(dv[row1]["BalQty"]).Trim();
                                        string strpk = Convert.ToString(dv[row1]["StorePK"]).Trim();
                                        string itmpk = Convert.ToString(dv[row1]["ItemPK"]).Trim();
                                        string rpu = Convert.ToString(dv[row1]["InwardRPU"]).Trim();
                                        string kitcod = Convert.ToString(dv[row1]["MasterCode"]).Trim();
                                        string allotdate = Convert.ToString(dv[row1]["date"]).Trim();
                                        int allot = Convert.ToInt32(Qty);
                                        string balqty = "";
                                        int issue = 0;
                                        int bal = 0;
                                        string issueQty = d2.GetFunction("select IssuedQuantity from Indivitual_student_ItemIssue where App_no='" + st_app_no + "' and ItemFK='" + itmpk + "' and Kit='1'");
                                        if (issueQty != "")
                                        {
                                            string[] issqty = issueQty.Split('.');
                                            issue = Convert.ToInt32(issqty[0]);
                                            bal = allot - issue;
                                            balqty = Convert.ToString(bal);
                                        }

                                        FpSpread2.Sheets[0].RowCount++;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = chkall;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].CellType = txtCell;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = txtCell;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].CellType = txtCell;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].CellType = txtCell;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].CellType = txtCell;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].CellType = txtCell;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].CellType = txtCell;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].CellType = num2;

                                        if (sno == 0)
                                            sno = 1;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = rollno;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag = st_app_no;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = regnum;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Tag = strpk;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = stuname;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Tag = itmpk;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = kitname;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Tag = kitcod;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = itmcode;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Tag = rpu;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Text = itmname;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Tag = allotdate;

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = Qty;

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(issue);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Tag = Convert.ToString(issue);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Text = balqty;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].Text = "";


                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Left;

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].BackColor = Color.Bisque;

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Locked = true;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Locked = false;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Locked = true;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Locked = true;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Locked = true;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Locked = true;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Locked = true;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Locked = true;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Locked = true;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Locked = true;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Locked = true;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].Locked = false;
                                        rowcnt1++;

                                    }
                                    if (rowcnt1 > 0)
                                    {
                                        FpSpread2.Sheets[0].SpanModel.Add(rowCnt1, 0, rowcnt1, 1);
                                        FpSpread2.Sheets[0].SpanModel.Add(rowCnt1, 1, rowcnt1, 1);
                                        FpSpread2.Sheets[0].SpanModel.Add(rowCnt1, 2, rowcnt1, 1);
                                        FpSpread2.Sheets[0].SpanModel.Add(rowCnt1, 3, rowcnt1, 1);
                                        FpSpread2.Sheets[0].SpanModel.Add(rowCnt1, 4, rowcnt1, 1);
                                        htRowCountreturn.Add(sappno, rowcnt1);
                                        sno++;

                                    }

                                }
                            }

                        }
                        #region columnorder
                        if (cblcolumnorder4.Items.Count > 0)
                        {
                            FpSpread2.Columns[0].Visible = true;
                            FpSpread2.Columns[1].Visible = true;
                            FpSpread2.Columns[11].Visible = true;
                            for (int k1 = 0; k1 < cblcolumnorder4.Items.Count; k1++)
                            {
                                if (cblcolumnorder4.Items[k1].Selected == true)
                                {
                                    string headername1 = Convert.ToString(cblcolumnorder4.Items[k1].ToString());

                                    if (headername1 == "Roll No")
                                    {
                                        FpSpread2.Columns[2].Visible = true;

                                    }
                                    else if (headername1 == "Reg No")
                                    {
                                        FpSpread2.Columns[3].Visible = true;

                                    }
                                    else if (headername1 == "Student Name")
                                    {
                                        FpSpread2.Columns[4].Visible = true;

                                    }
                                    else if (headername1 == "Kit Name")
                                    {
                                        FpSpread2.Columns[5].Visible = true;

                                    }
                                    else if (headername1 == "Item Code")
                                    {
                                        FpSpread2.Columns[6].Visible = true;

                                    }
                                    else if (headername1 == "Item Name")
                                    {
                                        FpSpread2.Columns[7].Visible = true;

                                    }
                                    else if (headername1 == "Alloted Qty")
                                    {
                                        FpSpread2.Columns[8].Visible = true;

                                    }
                                    else if (headername1 == "Issue Qty")
                                    {
                                        FpSpread2.Columns[9].Visible = true;

                                    }
                                    else if (headername1 == "Balance Qty")
                                    {
                                        FpSpread2.Columns[10].Visible = true;

                                    }

                                    chk3 = true;
                                }
                            }
                        }
                        if (chk3 == false)
                        {
                            CheckBox_column1.Checked = true;
                            //LinkButtonsremove_Click1(sender, e);
                            FpSpread2.Columns[0].Visible = true;
                            FpSpread2.Columns[1].Visible = true;
                            FpSpread2.Columns[10].Visible = true;
                            for (int k2 = 0; k2 < cblcolumnorder4.Items.Count; k2++)
                            {
                                //if (cblcolumnorder.Items[k].Selected == true)
                                //{
                                string headername2 = Convert.ToString(cblcolumnorder4.Items[k2].ToString());

                                if (headername2 == "Roll No")
                                {
                                    FpSpread2.Columns[2].Visible = true;
                                }
                                if (headername2 == "Reg No")
                                {
                                    FpSpread2.Columns[3].Visible = true;
                                }
                                else if (headername2 == "Student Name")
                                {
                                    FpSpread2.Columns[4].Visible = true;
                                }
                                else if (headername2 == "Kit Name")
                                {
                                    FpSpread2.Columns[5].Visible = true;
                                }
                                else if (headername2 == "Item Code")
                                {
                                    FpSpread2.Columns[6].Visible = true;
                                }
                                else if (headername2 == "Item Name")
                                {
                                    FpSpread2.Columns[7].Visible = true;
                                }

                                else if (headername2 == "Alloted Qty")
                                {
                                    FpSpread2.Columns[8].Visible = true;
                                }
                                else if (headername2 == "Issue Qty")
                                {
                                    FpSpread2.Columns[9].Visible = true;
                                }
                                else if (headername2 == "Balance Qty")
                                {
                                    FpSpread2.Columns[10].Visible = true;
                                }


                            }
                        }
                        #endregion

                        FpSpread2.Visible = true;
                        FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                        FpSpread2.Sheets[0].FrozenColumnCount = 5;
                        saveperson_table.Visible = false;
                        return_row.Visible = true;
                        lbl_erro2.Visible = false;

                    }
                    else
                    {
                        FpSpread2.Visible = false;
                        saveperson_table.Visible = false;
                        lbl_erro2.Visible = true;
                        lbl_erro2.Text = "No Records Found";

                    }
                }
                else
                {
                    FpSpread2.Visible = false;
                    saveperson_table.Visible = false;
                    lbl_erro2.Visible = true;
                    lbl_erro2.Text = "Please Select All Fields";

                }
                txt_app.Text = "";
                txt_reg.Text = "";
                txt_roll.Text = "";
                txt_studentname.Text = "";


            }
            #endregion
        }

        catch (Exception ex)
        {
            lbl_erro2.Visible = true;
            lbl_erro2.Text = ex.ToString();
        }
    }
    #endregion

    protected void LinkButtonsremove_Click1(object sender, EventArgs e)
    {
        try
        {
            if (rbl_individualstudent.Checked == true)
            {
                if (CheckBox_column1.Checked == true)
                {
                    for (int i = 0; i < cblcolumnorder1.Items.Count; i++)
                    {
                        cblcolumnorder1.Items[i].Selected = true;
                    }
                }
                else
                {
                    for (int i = 0; i < cblcolumnorder1.Items.Count; i++)
                    {
                        cblcolumnorder1.Items[i].Selected = false;
                    }
                }
            }
            else if (rbl_kit.Checked == true)
            {

                if (CheckBox_column1.Checked == true)
                {
                    for (int i = 0; i < cblcolumnorder2.Items.Count; i++)
                    {
                        cblcolumnorder2.Items[i].Selected = true;
                    }
                }
                else
                {
                    for (int i = 0; i < cblcolumnorder2.Items.Count; i++)
                    {
                        cblcolumnorder2.Items[i].Selected = false;
                    }
                }


            }
            else
            {

                if (CheckBox_column1.Checked == true)
                {
                    for (int i = 0; i < cblcolumnorder4.Items.Count; i++)
                    {
                        cblcolumnorder4.Items[i].Selected = true;
                    }
                }
                else
                {
                    for (int i = 0; i < cblcolumnorder4.Items.Count; i++)
                    {
                        cblcolumnorder4.Items[i].Selected = false;
                    }
                }


            }
        }
        catch { }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        query = "select distinct r.Stud_Name+'-'+r.Roll_No from Registration r, HT_HostelRegistration h where r.App_No=h.App_No and r.Stud_Name like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetRoll(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        query = "select distinct r.Roll_No from Registration r where  r.roll_no  like '" + prefixText + "%' order by Roll_No";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetReg(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        query = "select distinct r.Reg_No from Registration r where r.Reg_No like '" + prefixText + "%' order by Reg_No";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetApp(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        query = " select a.app_formno from Registration r,applyn a where r.App_No=a.app_no and a.app_formno like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getstaffname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = " select s.staff_name+' - '+dept_name+' - '+desig_name from staffmaster s,staff_appl_master a where s.appl_no=a.appl_no  and resign =0 and settled =0 and staff_name like '" + prefixText + "%' order by s.staff_name";
        name = ws.Getname(query);
        return name;
    }



    protected void ddl_searchby_onselectedindexchange(object sender, EventArgs e)
    {
        if (ddl_searchby.SelectedItem.Value == "0")
        {
            txt_reg.Visible = false;
            txt_app.Visible = false;
            txt_roll.Visible = true;
        }
        else if (ddl_searchby.SelectedItem.Value == "1")
        {
            txt_reg.Visible = true;
            txt_app.Visible = false;
            txt_roll.Visible = false;
        }
        else if (ddl_searchby.SelectedItem.Value == "2")
        {
            txt_reg.Visible = false;
            txt_app.Visible = true;
            txt_roll.Visible = false;
        }
        txt_app.Text = "";
        txt_reg.Text = "";
        txt_roll.Text = "";
        txt_studentname.Text = "";
    }

    public void Fpreadheaderbindmethod(string headername, FarPoint.Web.Spread.FpSpread spreadname, string AutoPostBack)
    {
        try
        {
            string[] header = headername.Split('/');
            int k = 0;
            if (AutoPostBack.Trim().ToUpper() == "TRUE")
            {
                if (header.Length > 0)
                {
                    spreadname.Sheets[0].RowCount = 0;
                    spreadname.Sheets[0].ColumnCount = 0;
                    spreadname.CommandBar.Visible = false;
                    spreadname.Sheets[0].AutoPostBack = true;
                    spreadname.Sheets[0].ColumnHeader.RowCount = 1;
                    spreadname.Sheets[0].RowHeader.Visible = false;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    spreadname.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    foreach (string head in header)
                    {
                        k++;
                        spreadname.Sheets[0].ColumnCount = Convert.ToInt32(header.Length);
                        if (head.Trim().ToUpper() == "S.NO")
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = head;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = 50;
                        }
                        else
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = head;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = 200;
                        }
                    }
                }
            }
            else if (AutoPostBack.Trim().ToUpper() == "FALSE")
            {
                if (header.Length > 0)
                {
                    spreadname.Sheets[0].RowCount = 0;
                    spreadname.Sheets[0].ColumnCount = 0;
                    spreadname.CommandBar.Visible = false;
                    spreadname.Sheets[0].AutoPostBack = false;
                    spreadname.Sheets[0].ColumnHeader.RowCount = 1;
                    spreadname.Sheets[0].RowHeader.Visible = false;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    spreadname.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    foreach (string head in header)
                    {
                        k++;
                        string[] width = head.Split('-');
                        spreadname.Sheets[0].ColumnCount = Convert.ToInt32(header.Length);
                        if (Convert.ToString(width[0]).Trim().ToUpper() == "S.NO")
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = Convert.ToString(width[0]);
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = Convert.ToInt32(width[1]);
                        }
                        else
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = Convert.ToString(width[0]);
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = Convert.ToInt32(width[1]);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            alertpopwindow.Visible = true;
            lblalerterr.Visible = true;
            lblalerterr.Font.Size = FontUnit.Smaller;
            lblalerterr.Text = ex.ToString();
        }
    }

    public string getvalues(CheckBoxList cblname)
    {
        string code = "";
        string value = "";
        try
        {
            for (int i = 0; i < cblname.Items.Count; i++)
            {
                if (cblname.Items[i].Selected == true)
                {
                    code = cblname.Items[i].Value.ToString();
                    if (value == "")
                    {
                        value = code;
                    }
                    else
                    {
                        value = value + "'" + "," + "'" + code;
                    }
                }
            }
        }
        catch { }
        return value;
    }

    #region IssueSave
    public void btn_issuedsave_Click(object sender, EventArgs e)
    {
        try
        {

            #region IndividualStudent
            if (rbl_individualstudent.Checked == true)
            {

                if (FpSpread2.Rows.Count > 0)
                {
                    string setrpu = d2.GetFunction("select value from Master_Settings where settings='Consumption Rpu' and usercode='" + usercode + "'");
                    FpSpread2.SaveChanges(); bool chk = false; bool sail = false; bool greater = false;
                    for (int row = 0; row < FpSpread2.Sheets[0].RowCount; row++)
                    {
                        int checkval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[row, 1].Value);
                        if (checkval == 1)
                        {
                            string app_no = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 2].Tag);
                            string itemFk = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 5].Tag);
                            string DeptFk = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 7].Tag);
                            string ledgerFk = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 4].Tag);
                            string Rpu = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 10].Text);
                            string balqty = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 9].Text);
                            string bqty = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 8].Text);
                            //string Rpu = "";
                            //string sailingprize = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 10].Text);
                            //if (sailingprize.Trim() == "")
                            //{
                            //    sailingprize = "0";
                            //}

                            //if (setrpu.Trim() == "0")
                            //{
                            //    Rpu = d2.GetFunction("select AVG(IssuedRPU) Avg_rpu from IT_StockDeptDetail where ItemFK='" + itemFk + "'");
                            //}
                            //if (setrpu.Trim() == "1")
                            //{
                            //    Rpu = d2.GetFunction("select AVG(Sailing_prize) Avg_rpu from IT_StockDeptDetail where ItemFK='" + itemFk + "'");
                            //}
                            string storeqry = "select StorePK from IM_StoreMaster  sm where sm.StorePK='" + DeptFk + "'";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(storeqry, "text");
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                string storepk = ds.Tables[0].Rows[0]["StorePK"].ToString();

                                if (Rpu.Trim() != "")
                                {
                                    string issuesqty1 = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 11].Text);
                                    string issuedbystaffappl_id1 = Convert.ToString(ViewState["appl_id"]);

                                    string dt1 = txt_issuedate.Text;
                                    DateTime date1 = new DateTime();
                                    if (dt1.Trim() != "")
                                    {
                                        string[] Split1 = dt1.Split('/');
                                        date1 = Convert.ToDateTime(Split1[1] + "/" + Split1[0] + "/" + Split1[2]);
                                    }
                                    if (issuesqty1.Trim() == "")
                                    {
                                        issuesqty1 = "0";
                                    }
                                    if (Convert.ToDouble(balqty) >= Convert.ToDouble(issuesqty1) && Convert.ToDouble(bqty) >= Convert.ToDouble(balqty))
                                    {
                                        if (app_no.Trim() != "" && itemFk.Trim() != "" && storepk.Trim() != "" && ledgerFk.Trim() != "" && issuesqty1.Trim() != "" && issuedbystaffappl_id1.Trim() != "")
                                        {
                                            string savedcqr1 = "if not exists (select * from HT_DailyConsumptionMaster where DailyConsDate='" + date1.ToString("MM/dd/yyyy") + "' and ForMess='3' and DeptFK='" + storepk + "') insert into HT_DailyConsumptionMaster (DailyConsDate,ForMess,UserCode,DeptFK)values('" + date1.ToString("MM/dd/yyyy") + "','3','" + usercode + "','" + storepk + "')";
                                            int dailyc1 = d2.update_method_wo_parameter(savedcqr1, "Text");

                                            string savedc1 = d2.GetFunction("select DailyConsumptionMasterPK from HT_DailyConsumptionMaster where DailyConsDate='" + date1.ToString("MM/dd/yyyy") + "' and ForMess='3' and DeptFK='" + storepk + "'");

                                            string savequery1 = "if exists (select * from HT_DailyConsumptionDetail where Itemfk ='" + itemFk + "' and DailyConsumptionMasterFK='" + savedc1 + "') update HT_DailyConsumptionDetail set ConsumptionQty=isnull(ConsumptionQty,0)+'" + issuesqty1 + "',RPU='" + Rpu + "' where Itemfk ='" + itemFk + "' and DailyConsumptionMasterFK='" + savedc1 + "' else insert into HT_DailyConsumptionDetail (ItemFK,ConsumptionQty,RPU,DailyConsumptionMasterFK) values ('" + itemFk + "','" + issuesqty1 + "','" + Rpu + "','" + savedc1 + "')";
                                            savequery1 = savequery1 + " update IT_StockDetail set UsedQty =ISNULL (UsedQty,0) +'" + issuesqty1 + "' ,BalQty =ISNULL(BalQty,0) -'" + issuesqty1 + "' where ItemFK ='" + itemFk + "' and StoreFK ='" + storepk + "' ";
                                            int itemsup1 = d2.update_method_wo_parameter(savequery1, "Text");
                                            if (itemsup1 != 0)
                                            {
                                                chk = true;
                                            }
                                            string qry1 = "update Indivitual_student_ItemIssue set Issues='1',IssueDate='" + date1.ToString("MM/dd/yyyy") + "',IssuedBy='" + issuedbystaffappl_id1 + "',IssuedQuantity=isnull(IssuedQuantity,0)+'" + issuesqty1 + "' where App_no='" + app_no + "' and ItemFK='" + itemFk + "' and DeptFK='" + storepk + "' and LedgerFK='" + ledgerFk + "'";
                                            int insert1 = d2.update_method_wo_parameter(qry1, "Text");
                                            if (insert1 != 0)
                                            {
                                                chk = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        greater = true;
                                    }
                                }
                                else
                                {
                                    sail = true;
                                }

                            }
                            else
                            {
                                if (Rpu.Trim() != "")
                                {
                                    string issuesqty = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 11].Text);
                                    string issuedbystaffappl_id = Convert.ToString(ViewState["appl_id"]);

                                    string dt = txt_issuedate.Text;
                                    DateTime date = new DateTime();
                                    if (dt.Trim() != "")
                                    {
                                        string[] Split = dt.Split('/');
                                        date = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
                                    }
                                    if (issuesqty.Trim() == "")
                                    {
                                        issuesqty = "0";
                                    }
                                    if (Convert.ToDouble(balqty) >= Convert.ToDouble(issuesqty) && Convert.ToDouble(bqty) >= Convert.ToDouble(balqty))
                                    {
                                        if (app_no.Trim() != "" && itemFk.Trim() != "" && DeptFk.Trim() != "" && ledgerFk.Trim() != "" && issuesqty.Trim() != "" && issuedbystaffappl_id.Trim() != "")
                                        {
                                            string savedcqr = "if not exists (select * from HT_DailyConsumptionMaster where DailyConsDate='" + date.ToString("MM/dd/yyyy") + "' and ForMess='2' and DeptFK='" + DeptFk + "') insert into HT_DailyConsumptionMaster (DailyConsDate,ForMess,UserCode,DeptFK)values('" + date.ToString("MM/dd/yyyy") + "','2','" + usercode + "','" + DeptFk + "')";
                                            int dailyc = d2.update_method_wo_parameter(savedcqr, "Text");

                                            string savedc = d2.GetFunction("select DailyConsumptionMasterPK from HT_DailyConsumptionMaster where DailyConsDate='" + date.ToString("MM/dd/yyyy") + "' and ForMess='2' and DeptFK='" + DeptFk + "'");

                                            string savequery = "if exists (select * from HT_DailyConsumptionDetail where Itemfk ='" + itemFk + "' and DailyConsumptionMasterFK='" + savedc + "') update HT_DailyConsumptionDetail set ConsumptionQty=isnull(ConsumptionQty,0)+'" + issuesqty + "',RPU='" + Rpu + "' where Itemfk ='" + itemFk + "' and DailyConsumptionMasterFK='" + savedc + "' else insert into HT_DailyConsumptionDetail (ItemFK,ConsumptionQty,RPU,DailyConsumptionMasterFK) values ('" + itemFk + "','" + issuesqty + "','" + Rpu + "','" + savedc + "')";
                                            savequery = savequery + " update IT_StockDeptDetail set UsedQty =ISNULL (UsedQty,0) +'" + issuesqty + "' ,BalQty =ISNULL(BalQty,0) -'" + issuesqty + "' where ItemFK ='" + itemFk + "' and DeptFK ='" + DeptFk + "' ";
                                            int itemsup = d2.update_method_wo_parameter(savequery, "Text");
                                            if (itemsup != 0)
                                            {
                                                chk = true;
                                            }
                                            string q1 = "update Indivitual_student_ItemIssue set Issues='1',IssueDate='" + date.ToString("MM/dd/yyyy") + "',IssuedBy='" + issuedbystaffappl_id + "',IssuedQuantity=isnull(IssuedQuantity,0)+'" + issuesqty + "' where App_no='" + app_no + "' and ItemFK='" + itemFk + "' and DeptFK='" + DeptFk + "' and LedgerFK='" + ledgerFk + "'";
                                            int insert = d2.update_method_wo_parameter(q1, "Text");
                                            if (insert != 0)
                                            {
                                                chk = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        greater = true;
                                    }
                                }
                                else
                                {
                                    sail = true;
                                }
                            }

                        }
                    }
                    if (chk == false)
                    {
                        imgdiv2.Visible = true;
                        lbl_alerterr.Visible = true;
                        lbl_alerterr.Text = "Please Select Any One Fields";
                    }
                    if (chk == true)
                    {
                        imgdiv2.Visible = true;
                        lbl_alerterr.Text = "Saved Successfully";
                        lbl_alerterr.Visible = true;
                        btn_go2_Click(sender, e);
                    }
                    if (sail == true)
                    {
                        imgdiv2.Visible = true;
                        lbl_alerterr.Text = "Please Enter Sailing Prize";
                        lbl_alerterr.Visible = true;
                    }
                    if (greater == true)
                    {
                        imgdiv2.Visible = true;
                        lbl_alerterr.Text = "Please Enter Issues Quantity Greater than Balance Qty";
                        lbl_alerterr.Visible = true;
                    }
                }
            }
            #endregion


            #region KitIssue
            else
            {
                bool ins = false; bool greaterbal = false; bool chek = false;
                if (FpSpread2.Rows.Count > 0)
                {
                    string StdAppno = "";
                    FpSpread2.SaveChanges();
                    for (int row = 0; row < FpSpread2.Sheets[0].RowCount-1; row++)
                    {
                        int checkval1 = Convert.ToInt32(FpSpread2.Sheets[0].Cells[row, 1].Value);
                        if (checkval1 == 1)
                        {
                            chek = true;
                            string app_no = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 2].Tag);
                            if (app_no != "")
                            {

                                int rowCnt = 0;
                                if (htRowCount.ContainsKey(app_no))
                                {
                                    if (StdAppno != app_no)
                                    {
                                        int.TryParse(Convert.ToString(htRowCount[app_no]), out rowCnt);

                                        for (int srow = row; srow < row + rowCnt; srow++)
                                        {
                                            string Feecategory = "";
                                            string itempk = Convert.ToString(FpSpread2.Sheets[0].Cells[srow, 4].Tag);
                                            string storepk = Convert.ToString(FpSpread2.Sheets[0].Cells[srow, 3].Tag);
                                            string rpu = Convert.ToString(FpSpread2.Sheets[0].Cells[srow, 6].Tag);
                                            string ktcode = Convert.ToString(FpSpread2.Sheets[0].Cells[srow, 5].Tag);
                                            string balanqty = Convert.ToString(FpSpread2.Sheets[0].Cells[srow, 8].Text);
                                            string Allotqty = Convert.ToString(FpSpread2.Sheets[0].Cells[srow, 9].Text);
                                            string alldate = Convert.ToString(FpSpread2.Sheets[0].Cells[srow, 7].Tag);
                                            string issueqty1 = Convert.ToString(FpSpread2.Sheets[0].Cells[srow, 10].Text);
                                            string issuedbystaffappl_id1 = Convert.ToString(ViewState["appl_id"]);
                                            string ledgerFk = d2.GetFunction("select ledgerid from inventorykit where  kitid='" + ktcode + "'");
                                            string headerid = d2.GetFunction("select headerid from inventorykit where  kitid='" + ktcode + "'");

                                            if (headerid != "" && headerid != "0" && ledgerFk != "" && ledgerFk != "0")
                                            {
                                                string selsctfeecate = d2.GetFunction("select distinct current_semester from registration r where r.App_No='" + app_no + "' and r.college_code in('" + collegecode1 + "')");

                                                if (checkSchoolSetting() == 0)
                                                {
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
                                                if (semval != "")
                                                {
                                                    sqlcmd = d2.GetFunction("select distinct textcode from textvaltable where textcriteria ='FEECA' and textval='" + semval + "' and college_code='" + collegecode1 + "'");
                                                    if (sqlcmd != "0" && sqlcmd != "")
                                                        Feecategory = Convert.ToString(sqlcmd);
                                                }
                                            }
                                            string dt1 = txt_issuedate.Text;
                                            DateTime date1 = new DateTime();
                                            if (dt1.Trim() != "")
                                            {
                                                string[] Split1 = dt1.Split('/');
                                                date1 = Convert.ToDateTime(Split1[1] + "/" + Split1[0] + "/" + Split1[2]);
                                            }
                                            if (issueqty1.Trim() == "")
                                            {
                                                issueqty1 = "0";
                                            }
                                            if (Convert.ToDouble(Allotqty) >= Convert.ToDouble(issueqty1) && Convert.ToDouble(balanqty) >= Convert.ToDouble(Allotqty))
                                            {
                                                //if (app_no.Trim() != "" && itempk.Trim() != "" && storepk.Trim() != "" && ledgerFk.Trim() != "" && issueqty1.Trim() != "" && issuedbystaffappl_id1.Trim() != "")
                                                if (app_no.Trim() != "" && itempk.Trim() != "" && storepk.Trim() != "" && issueqty1.Trim() != "" && issuedbystaffappl_id1.Trim() != "")
                                                {
                                                    string savedcqr1 = "if not exists (select * from HT_DailyConsumptionMaster where DailyConsDate='" + date1.ToString("MM/dd/yyyy") + "' and ForMess='3' and DeptFK='" + storepk + "') insert into HT_DailyConsumptionMaster (DailyConsDate,ForMess,UserCode,DeptFK)values('" + date1.ToString("MM/dd/yyyy") + "','3','" + usercode + "','" + storepk + "')";
                                                    int dailyc1 = d2.update_method_wo_parameter(savedcqr1, "Text");

                                                    string savedc1 = d2.GetFunction("select DailyConsumptionMasterPK from HT_DailyConsumptionMaster where DailyConsDate='" + date1.ToString("MM/dd/yyyy") + "' and ForMess='3' and DeptFK='" + storepk + "'");

                                                    string savequery1 = "if exists (select * from HT_DailyConsumptionDetail where Itemfk ='" + itempk + "' and DailyConsumptionMasterFK='" + savedc1 + "') update HT_DailyConsumptionDetail set ConsumptionQty=isnull(ConsumptionQty,0)+'" + issueqty1 + "',RPU='" + rpu + "' where Itemfk ='" + itempk + "' and DailyConsumptionMasterFK='" + savedc1 + "' else insert into HT_DailyConsumptionDetail (ItemFK,ConsumptionQty,RPU,DailyConsumptionMasterFK) values ('" + itempk + "','" + issueqty1 + "','" + rpu + "','" + savedc1 + "')";
                                                    savequery1 = savequery1 + " update IT_StockDetail set UsedQty =ISNULL (UsedQty,0) +'" + issueqty1 + "' ,BalQty =ISNULL(BalQty,0) -'" + issueqty1 + "' where ItemFK ='" + itempk + "' and StoreFK ='" + storepk + "' ";
                                                    int itemsup1 = d2.update_method_wo_parameter(savequery1, "Text");
                                                    if (itemsup1 != 0)
                                                    {
                                                        ins = true;
                                                    }
                                                    string qry1 = "if exists (select * from Indivitual_student_ItemIssue where  App_no='" + app_no + "' and Itemfk ='" + itempk + "' and kit='1' and MemType='1')update Indivitual_student_ItemIssue set Issues='1',IssueDate='" + date1.ToString("MM/dd/yyyy") + "',IssuedBy='" + issuedbystaffappl_id1 + "',IssuedQuantity=isnull(IssuedQuantity,0)+'" + issueqty1 + "' where  App_no='" + app_no + "' and Itemfk ='" + itempk + "'  and kit='1' and MemType='1' and LedgerFK='" + ledgerFk + "' else insert into Indivitual_student_ItemIssue (AllotDate,MemType,App_no,LedgerFK,ItemFK,DeptFK,Quantity,Rpu,Issues,IssueDate,IssuedBy,IssuedQuantity,feecategory,Kit) values ('" + alldate + "','1','" + app_no + "','" + ledgerFk + "','" + itempk + "','" + storepk + "','" + Allotqty + "','" + rpu + "','1','" + date1.ToString("MM/dd/yyyy") + "','" + issuedbystaffappl_id1 + "','" + issueqty1 + "','" + Feecategory + "','1')";
                                                    int insert1 = d2.update_method_wo_parameter(qry1, "Text");
                                                    if (insert1 != 0)
                                                    {
                                                        ins = true;
                                                    }
                                                }
                                                //else
                                                //{
                                                //    sail = true;
                                                //}
                                            }
                                            else
                                            {
                                                greaterbal = true;
                                            }

                                        }
                                        StdAppno = app_no;
                                    }
                                }
                            }
                        }
                    }

                    if (chek == false)
                    {
                        imgdiv2.Visible = true;
                        lbl_alerterr.Visible = true;
                        lbl_alerterr.Text = "Please Select Any One Fields";
                        btn_go2_Click(sender, e);
                    }
                    if (ins == true)
                    {
                        imgdiv2.Visible = true;
                        lbl_alerterr.Text = "Saved Successfully";
                        lbl_alerterr.Visible = true;
                        btn_go2_Click(sender, e);
                    }
                    //if (sail == true)
                    //{
                    //    imgdiv2.Visible = true;
                    //    lbl_alerterr.Text = "Please Enter Sailing Prize";
                    //    lbl_alerterr.Visible = true;
                    //}
                    if (greaterbal == true)
                    {
                        imgdiv2.Visible = true;
                        lbl_alerterr.Text = "Please Enter Issues Quantity Greater than Avaiable Qty";
                        lbl_alerterr.Visible = true;
                        btn_go2_Click(sender, e);
                    }
                }
            }
            #endregion

            txt_issueperson.Text = "";
        }
        catch (Exception ex)
        {
            
        }
    }
    #endregion

    #region Retrun
    protected void btn_return_Click(object sender, EventArgs e)
    {
        try
        {
            bool ins = false; bool greaterbal = false; bool chek1 = false;
            if (FpSpread2.Rows.Count > 0)
            {
                string StudAppno = "";
                FpSpread2.SaveChanges();
                for (int row = 0; row < FpSpread2.Sheets[0].RowCount-1; row++)
                {
                    int checkval1 = Convert.ToInt32(FpSpread2.Sheets[0].Cells[row, 1].Value);
                    if (checkval1 == 1)
                    {
                        chek1 = true;
                        string app_no = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 2].Tag);
                        if (app_no != "")
                        {
                            int rowCnt = 0;
                            if (htRowCountreturn.ContainsKey(app_no))
                            {
                                if (StudAppno != app_no)
                                {
                                    int.TryParse(Convert.ToString(htRowCountreturn[app_no]), out rowCnt);

                                    for (int srow = row; srow < row + rowCnt; srow++)
                                    {
                                        string itempk = Convert.ToString(FpSpread2.Sheets[0].Cells[srow, 4].Tag);
                                        string storepk = Convert.ToString(FpSpread2.Sheets[0].Cells[srow, 3].Tag);
                                        string rpu = Convert.ToString(FpSpread2.Sheets[0].Cells[srow, 6].Tag);
                                        string ktcode = Convert.ToString(FpSpread2.Sheets[0].Cells[srow, 5].Tag);
                                        string balanqty = Convert.ToString(FpSpread2.Sheets[0].Cells[srow, 8].Text);
                                        string Allotqty = Convert.ToString(FpSpread2.Sheets[0].Cells[srow, 9].Text);
                                        string alldate = Convert.ToString(FpSpread2.Sheets[0].Cells[srow, 7].Tag);
                                        string issueqty1 = Convert.ToString(FpSpread2.Sheets[0].Cells[srow, 9].Tag);
                                        string returnqty = Convert.ToString(FpSpread2.Sheets[0].Cells[srow, 11].Text);

                                        string ledgerFk = d2.GetFunction("select ledgerid from inventorykit where  kitid='" + ktcode + "'");
                                        string dt1 = txt_reDate.Text;
                                        DateTime date1 = new DateTime();
                                        if (dt1.Trim() != "")
                                        {
                                            string[] Split1 = dt1.Split('/');
                                            date1 = Convert.ToDateTime(Split1[1] + "/" + Split1[0] + "/" + Split1[2]);
                                        }
                                        if (returnqty.Trim() == "")
                                        {
                                            returnqty = "0";
                                        }
                                        if (Convert.ToDouble(issueqty1) >= Convert.ToDouble(returnqty) && Convert.ToDouble(Allotqty) >= Convert.ToDouble(returnqty))
                                        {
                                            if (app_no.Trim() != "" && itempk.Trim() != "" && storepk.Trim() != "" && issueqty1.Trim() != "" && returnqty != "")
                                            {
                                                string qry1 = "if exists (select * from Indivitual_student_ItemIssue where  App_no='" + app_no + "' and Itemfk ='" + itempk + "' and kit='1' and MemType='1' and issues='1')update Indivitual_student_ItemIssue set Return_Item='1', Return_Date='" + date1.ToString("MM/dd/yyyy") + "', IssuedQuantity=isnull(IssuedQuantity,0)-'" + returnqty + "' where  App_no='" + app_no + "' and Itemfk ='" + itempk + "'  and kit='1' and MemType='1' and LedgerFK='" + ledgerFk + "'";

                                                //qry1 += "if exists(select * from IM_StudentKit_Details where Stu_AppNo='" + app_no + "' and itemfk='" + itempk + "' and Date='" + alldate + "' and KitCode='" + ktcode + "') update IM_StudentKit_Details set Qty=isnull(Qty,0)+'" + returnqty + "' where Stu_AppNo='" + app_no + "' and itemfk='" + itempk + "' and Date='" + alldate + "' and KitCode='" + ktcode + "'";
                                                int insert1 = d2.update_method_wo_parameter(qry1, "Text");
                                                if (insert1 != 0)
                                                {
                                                    ins = true;
                                                }
                                            }

                                        }
                                        else
                                        {
                                            greaterbal = true;
                                        }
                                    }
                                    StudAppno = app_no;
                                }
                            }
                        }
                    }
                }

                if (chek1 == false)
                {
                    imgdiv2.Visible = true;
                    lbl_alerterr.Visible = true;
                    lbl_alerterr.Text = "Please Select Any One Fields";
                    btn_go2_Click(sender, e);
                }
                if (ins == true)
                {
                    imgdiv2.Visible = true;
                    lbl_alerterr.Text = "Saved Successfully";
                    lbl_alerterr.Visible = true;
                    btn_go2_Click(sender, e);
                }

                if (greaterbal == true)
                {
                    imgdiv2.Visible = true;
                    lbl_alerterr.Text = "Please Enter Return Quantity Greater than Issue Qty";
                    lbl_alerterr.Visible = true;
                    btn_go2_Click(sender, e);
                }
            }

        }
        catch
        {


        }
    }
    #endregion

    protected void txt_issueperson_Text_Changed(object sender, EventArgs e)
    {
        string applid = "";
        string staffname = Convert.ToString(txt_issueperson.Text);
        string[] staffname1 = staffname.Split('-');

        applid = d2.GetFunction("select appl_id from staffmaster s,staff_appl_master a where s.appl_no=a.appl_no and s.staff_name='" + staffname1[0] + "'");
        if (applid.Trim() != "0")
        {
            ViewState["appl_id"] = applid;
        }
        else
        {
            ViewState["appl_id"] = null;
            txt_issueperson.Text = "";
        }
    }

    protected void rdb_common_Checkedchange(object sender, EventArgs e)
    {
        if (rdb_common.Checked == true)
        {
            commoncolumnorder.Visible = true;
            individualcolumnorder.Visible = false;
            ddl_type.Enabled = false;
        }
        else
        {
            individualcolumnorder.Visible = true;
            commoncolumnorder.Visible = false;
            ddl_type.Enabled = true;
        }
        Fpmain.Visible = false;
        rptprint.Visible = false;
    }

    protected void rdb_Individual_Checkedchange(object sender, EventArgs e)
    {
        if (rdb_common.Checked == true)
        {
            commoncolumnorder.Visible = true;
            individualcolumnorder.Visible = false;
            ddl_type.Enabled = false;
        }
        else
        {
            individualcolumnorder.Visible = true;
            commoncolumnorder.Visible = false;
            ddl_type.Enabled = true;
        }
        Fpmain.Visible = false;
        rptprint.Visible = false;
    }


    //Added By SaranyaDevi 29.3.2018
    protected void rbl_Department_Selected(object sender, EventArgs e)
    {
        try
        {
            rbl_Department.Checked = true;
            rbl_Store.Checked = false;
            lbl_dept.Visible = true;
            lbl_store.Visible = false;
            UpdatePanel5.Visible = true;
            UpdatePanel4.Visible = false;
            Fpmain.Visible = false;
            rptprint.Visible = false;
        }
        catch
        {

        }


    }

    protected void rbl_Store_Selected(object sender, EventArgs e)
    {
        try
        {
            rbl_Department.Checked = false;
            rbl_Store.Checked = true;
            lbl_dept.Visible = false;
            lbl_store.Visible = true;
            UpdatePanel5.Visible = false;
            UpdatePanel4.Visible = true;
            Fpmain.Visible = false;
            rptprint.Visible = false;

        }
        catch
        {

        }

    }

    protected void cb_store_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_store.Text = "--Select--";

            if (cb_store.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_store.Items.Count; i++)
                {
                    cbl_store.Items[i].Selected = true;
                }
                txt_store.Text = "Store(" + (cbl_store.Items.Count) + ")";
            }

            else
            {
                for (int i = 0; i < cbl_store.Items.Count; i++)
                {
                    cbl_store.Items[i].Selected = false;
                }
            }
            binditem();
        }
        catch
        {
        }

    }

    protected void cbl_store_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {

            int i = 0;
            cb_store.Checked = false;
            //item();
            int commcount = 0;
            txt_store.Text = "--Select--";
            for (i = 0; i < cbl_store.Items.Count; i++)
            {
                if (cbl_store.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_store.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_store.Items.Count)
                {
                    cb_store.Checked = true;
                }
                txt_store.Text = "Store(" + commcount.ToString() + ")";
            }
            binditem();
        }
        catch
        {

        }

    }

    protected void rblissue_Wise_Selected(object sender, EventArgs e)
    {
        try
        {
            if (rblissue_Wise.SelectedIndex == 0)
            {
                lbl_deptadd.Visible = true;
                lbl_storeadd.Visible = false;
                ddl_dept.Visible = true;
                ddl_Store.Visible = false;
                binddept();
                binditem1();
            }
            else
            {
                lbl_deptadd.Visible = false;
                lbl_storeadd.Visible = true;
                ddl_dept.Visible = false;
                ddl_Store.Visible = true;
                bindstore();
                binditem1();
            }

        }
        catch
        { }

    }

    protected void bindstore()
    {
        ds.Clear();
        string storepk = d2.GetFunction("select value from Master_Settings where settings='Store Rights' and usercode='" + usercode + "'  and value<>''");
        if (storepk.Trim() != "0")
        {
            ds = d2.BindStorebaseonrights_inv(storepk);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_Store.DataSource = ds;
                ddl_Store.DataTextField = "StoreName";
                ddl_Store.DataValueField = "StorePK";
                ddl_Store.DataBind();

                cbl_store.DataSource = ds;
                cbl_store.DataTextField = "StoreName";
                cbl_store.DataValueField = "StorePK";
                cbl_store.DataBind();
                if (cbl_store.Items.Count > 0)
                {
                    for (int j = 0; j < cbl_store.Items.Count; j++)
                    {
                        cbl_store.Items[j].Selected = true;
                    }
                    txt_store.Text = "Store(" + cbl_store.Items.Count + ")";
                }

            }
            else
            {
                txt_store.Text = "--Select--";
            }
        }
    }

    protected void ddl_Store_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            binditem1();
        }
        catch
        {
        }

    }

    //end By SaranyaDevi 30.3.2018


    //Added By SaranyaDevi 30.4.2018

    protected void rbl_individualstudent_Selected(object sender, EventArgs e)
    {
        try
        {
            rbl_individualstudent.Checked = true;
            rbl_retuen.Checked = false;
            rbl_kit.Checked = false;
            lbl_kit.Visible = false;
            kitname.Visible = false;
            lbl_erro2.Visible = false;
            lbl_erro2.Text = "";
            pheaderfilter1.Visible = true;
            individualstudentcolumnorder.Visible = true;
            kitstudentcolumnorder.Visible = false;
            FpSpread2.Visible = false;
            kitstudentcolumnorder.Visible = false;
            kitstudentreturncolumnorder.Visible = false;
            LinkButtonsremove_Click1(sender, e);
            cb_fromto.Visible = false;
            cb_fromto.Checked = false;
            txt_fromdate1.Enabled = true;
            txt_todate1.Enabled = true;
            saveperson_table.Visible = false;
            return_row.Visible = false;
            //saveperson_table.Visible = false;
            //txt_issueperson.Visible = false;
        }
        catch
        {

        }

    }

    protected void rbl_kit_Selected(object sender, EventArgs e)
    {
        try
        {
            rbl_individualstudent.Checked = false;
            rbl_retuen.Checked = false;
            rbl_kit.Checked = true;
            lbl_kit.Visible = true;
            kitname.Visible = true;
            lbl_erro2.Visible = false;
            lbl_erro2.Text = "";
            pheaderfilter1.Visible = true;
            individualstudentcolumnorder.Visible = false;
            kitstudentreturncolumnorder.Visible = false;
            kitstudentcolumnorder.Visible = true;
            FpSpread2.Visible = false;
            saveperson_table.Visible = false;
            return_row.Visible = false;
            loadkit();
            LinkButtonsremove_Click1(sender, e);
            cb_fromto.Visible = true;
            cb_fromto.Checked = false;
            txt_fromdate1.Enabled = false;
            txt_todate1.Enabled = false;
        }
        catch
        {

        }
    }

    protected void rbl_retuen_Selected(object sender, EventArgs e)
    {
        try
        {
            rbl_individualstudent.Checked = false;
            rbl_kit.Checked = false;
            rbl_retuen.Checked = true;
            lbl_kit.Visible = true;
            kitname.Visible = true;
            lbl_erro2.Visible = false;
            lbl_erro2.Text = "";
            pheaderfilter1.Visible = true;
            individualstudentcolumnorder.Visible = false;
            kitstudentcolumnorder.Visible = false;
            kitstudentreturncolumnorder.Visible = true;
            FpSpread2.Visible = false;
            saveperson_table.Visible = false;
            loadkit();
            LinkButtonsremove_Click1(sender, e);
            cb_fromto.Visible = true;
            cb_fromto.Checked = false;
            txt_fromdate1.Enabled = false;
            txt_todate1.Enabled = false;
            return_row.Visible = false;
        }
        catch
        {


        }

    }

    protected void txt_roll_changed(object sender, EventArgs e)
    {
        try
        {
            if (rbl_kit.Checked == true)
            {
                loadkit();

            }
        }
        catch
        {

        }

    }

    protected void txt_reg_changed(object sender, EventArgs e)
    {
        try
        {
            if (rbl_kit.Checked == true)
            {
                loadkit();

            }
        }
        catch
        {

        }

    }

    protected void txt_app_changed(object sender, EventArgs e)
    {
        try
        {
            if (rbl_kit.Checked == true)
            {
                loadkit();

            }
        }
        catch
        {

        }

    }

    protected void txt_studentname_Changed(object sender, EventArgs e)
    {
        try
        {
            if (rbl_kit.Checked == true)
            {
                loadkit();

            }
        }
        catch
        {
        }
    }

    #region Load_Kit_name
    public void loadkit()
    {
        try
        {
            string appno = string.Empty;
            string q1 = "";
            cbl_kitname.Items.Clear();
            if (txt_roll.Text != "")
                appno = d2.GetFunction(" select App_No from Registration where Roll_No='" + txt_roll.Text + "' ");
            else if (txt_reg.Text != "")
                appno = d2.GetFunction(" select App_No from Registration where Reg_No='" + txt_reg.Text + "' ");
            else if (txt_studentname.Text != "")
                appno = d2.GetFunction(" select App_No from Registration where Stud_Name='" + txt_studentname.Text + "' ");
            else
                appno = d2.GetFunction(" select App_No from Registration where app_formno='" + txt_app.Text + "' ");
            if (appno != "" && appno != "0")
            {
                q1 = " select distinct cm.MasterValue,cm.MasterCode from IM_StudentKit_Details sd,IM_KitMaster km,CO_MasterValues cm  where sd.KitCode=km.KitCode and cm.CollegeCode=km.CollegeCode and cm.MasterCode=sd.KitCode and km.KitCode=cm.MasterCode and km.ItemCode=sd.ItemCode and Stu_AppNo='" + appno + "'";
            }
            else
            {
                q1 = " select distinct cm.MasterValue,cm.MasterCode from IM_StudentKit_Details sd,IM_KitMaster km,CO_MasterValues cm  where sd.KitCode=km.KitCode and cm.CollegeCode=km.CollegeCode and cm.MasterCode=sd.KitCode and km.KitCode=cm.MasterCode and km.ItemCode=sd.ItemCode";

            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(q1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_kitname.DataSource = ds;
                cbl_kitname.DataTextField = "MasterValue";
                cbl_kitname.DataValueField = "MasterCode";
                cbl_kitname.DataBind();
                //cbl_section.Items.Insert(0, new ListItem(" ", " "));

            }
            for (int i = 0; i < cbl_kitname.Items.Count; i++)
            {
                cbl_kitname.Items[i].Selected = true;

            }
            txt_kitname.Text = lbl_kitname.Text + "(" + cbl_kitname.Items.Count + ")";
            cb_kitname.Checked = true;
        }
        catch
        {

        }


    }

    protected void cb_kitname_checkedchange(object sender, EventArgs e)
    {
        rs.CallCheckboxChange(cb_kitname, cbl_kitname, txt_kitname, "Kit Name", "--Select--");


    }

    protected void cbl_kitname_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckboxListChange(cb_kitname, cbl_kitname, txt_kitname, "Kit Name", "--Select--");


    }
    #endregion

    protected void Fpspread2_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        //Fpspread2.Visible = true;
        try
        {
            string actrow = FpSpread2.Sheets[0].ActiveRow.ToString();
            string actcol = FpSpread2.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (FpSpread2.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < FpSpread2.Sheets[0].RowCount; i++)
                        {
                            FpSpread2.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < FpSpread2.Sheets[0].RowCount; i++)
                        {
                            FpSpread2.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode, "Individual_StudentFeeStatus"); 
        }
    }

    #region FromToCheck
    protected void cb_fromto_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_fromto.Checked == true)
            {
                txt_fromdate1.Enabled = true;
                txt_todate1.Enabled = true;

            }
            else
            {
                txt_fromdate1.Enabled = false;
                txt_todate1.Enabled = false;

            }
        }
        catch
        {


        }


    }
    #endregion

}