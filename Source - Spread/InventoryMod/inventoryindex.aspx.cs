using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
public partial class inventoryindex : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    ArrayList rights = new ArrayList();
    ArrayList indexcontain = new ArrayList();

    string usercode = "";
    string groupcode = "";
    string collegecode = string.Empty;
    string streamcode = string.Empty;
    string sessstream = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        if (!IsPostBack)
        {
            string finance = d2.GetFunction(" select * from IPatchStatus where UpdatedDate <= ClientUpdatedDate and ModuleName ='Inventory' ");
            if (finance == "0")
            {
                lblalerterr.Text = "Please Update Patch File";
                alertpopwindow.Visible = true;
                return;
            }
            bindinventoryindex();
        }
    }
    protected void bindinventoryindex()
    {
        try
        {
            ////master
            //rights.Clear();
            //rights.Add("Inventory,Master,901,INV001,Code Master,Inv_CodeMaster.aspx,~/helpcontentinvpages/invcodesetting.htm,1,1");
            //rights.Add("Inventory,Master,902,INV002,Store Master,Hm_StoreMasterNew.aspx,~/helpcontentinvpages/storemaster.htm,1,2");
            //rights.Add("Inventory,Master,903,INV003,Item Master,Item_master.aspx,~/helpcontentinvpages/itemmaster.htm,1,3");
            //rights.Add("Inventory,Master,904,INV004,Supplier Master,Supplier_master.aspx,~/helpcontentinvpages/suppliermaster.htm,1,4");

            ////operation
            //rights.Add("Inventory,Operation,905,INVO001,Opening Stock,inv_opening_stock.aspx,~/helpcontentinvpages/openingstock.htm,2,5");
            //rights.Add("Inventory,Operation,906,INVO002,Request Hierarchy,HierarchySetting.aspx,~/helpcontentinvpages/.htm,2,6");
            //rights.Add("Inventory,Operation,907,INVO003,Request,Request.aspx,~/helpcontentinvpages/.htm,2,7");
            //rights.Add("Inventory,Operation,908,INVO004,Request Report,request_report.aspx,~/helpcontentinvpages/.htm,2,8");
            //// rights.Add("Inventory,Operation,909,INVO005,Approval for Request,request_vendor.aspx,,2,9");
            //rights.Add("Inventory,Operation,909,INVO005,Request to Vendor,request_vendor.aspx,~/helpcontentinvpages/requestvendor.htm,2,9");
            //rights.Add("Inventory,Operation,910,INVO006,Quotation Entry,vendor_quatation_request.aspx,~/helpcontentinvpages/vendorquotation.htm,2,10");
            //rights.Add("Inventory,Operation,911,INVO007,Quotation Comparison,vendor_quotation_compare.aspx,~/helpcontentinvpages/Quotationcompare.htm,2,11");
            //rights.Add("Inventory,Operation,912,INVO008,Purchase Order,inv_Purchase.aspx,~/helpcontentinvpages/purchaseorder.htm,2,12");
            //rights.Add("Inventory,Operation,913,INVO009,Approval for Purchase Order,inv_purchaseorder_request.aspx,~/helpcontentinvpages/purchaserequestandapprove.htm,2,13");
            //rights.Add("Inventory,Operation,914,INVO010,Inward,inv_inward.aspx,~/helpcontentinvpages/inward.htm,2,14");
            ////rights.Add("Inventory,Operation,916,INVO012,Return to Vendor,HT_Income.aspx,,2,16");
            ////rights.Add("Inventory,Operation,917,INVO013,Issue from Store,HM_Expanses.aspx,,2,17");
            //rights.Add("Inventory,Operation,915,INVO011,Transfer,Inv_Transfer.aspx,~/helpcontentinvpages/Transfer.htm,2,15");
            //rights.Add("Inventory,Operation,916,INVO012,Item Usage,itemusuage.aspx,~/helpcontentinvpages/itemusage.htm,2,16");
            //rights.Add("Inventory,Operation,917,INVO013,Item Missing / Scrap and Breakage,breakage_entry.aspx,~/helpcontentinvpages/breakage.htm,2,17");

            ////report
            //rights.Add("Inventory,Report,919,INVR001,Department Stock Status Report,Inv_Dept_stockstatus_Report.aspx,~/helpcontentinvpages/deptmentstockstatusreport.htm,3,18");
            //rights.Add("Inventory,Report,920,INVR002,Purchase Order setting,purchase_order_setting.aspx,~/helpcontentinvpages/purchaseordersettings.htm,3,19");
            //rights.Add("Inventory,Report,921,INVR003,Purchase Status Report,HM_Purchasestatus_Report.aspx,~/helpcontentinvpages/purchasestatusreport.htm,3,20");
            //rights.Add("Inventory,Report,922,INVR004,Stock Status Report,HM_Stock_Status_Report.aspx,~/helpcontentpages/Stockstatusreport.htm,3,21");
            //rights.Add("Inventory,Report,923,INVR005,Purchase Order print Settings,Investorsposetting.aspx,~/helpcontentpages/purchaseorderprintsettings.htm,3,22");
            //rights.Add("Inventory,Report,924,INVR006,Transfer Details,Transfer_details.aspx,~/helpcontentinvpages/Transferdetails.htm,3,23");
            //rights.Add("Inventory,Report,925,INVR007,Item Rate Information,Itemrateinformation.aspx,~/helpcontentinvpages/itemrateinformation.htm,3,24");

            //for (int i = 0; i < rights.Count; i++)
            //{
            //    string[] index = Convert.ToString(rights[i]).Split(',');
            //    string q1 = " if exists(select ReportName from  Security_Rights_Details where ModuleName='" + index[0].ToString() + "' and Rights_Code='" + index[2].ToString() + "' ) update Security_Rights_Details set HeaderName='" + index[1].ToString() + "',ReportId='" + index[3].ToString() + "',ReportName='" + index[4].ToString() + "',PageName='" + index[5].ToString() + "',HelpURL='" + index[6].ToString() + "',HeaderPriority='" + index[7].ToString() + "',PagePriority='" + index[8].ToString() + "' where ModuleName='" + index[0].ToString() + "' and Rights_Code='" + index[2].ToString() + "' else insert into Security_Rights_Details (ModuleName,HeaderName,Rights_Code,ReportId,ReportName,PageName,HelpURL,HeaderPriority,PagePriority) values ('" + index[0].ToString() + "','" + index[1].ToString() + "','" + index[2].ToString() + "','" + index[3].ToString() + "','" + index[4].ToString() + "','" + index[5].ToString() + "','" + index[6].ToString() + "','" + index[7].ToString() + "','" + index[8].ToString() + "')";
            //    int insert = d2.update_method_wo_parameter(q1, "text");
            //}
            string grouporusercode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " and user_code=" + Session["usercode"].ToString().Trim() + "";
            }
            string q2 = " select  s.ModuleName,s.HeaderName,s.Rights_Code,s.ReportId,s.ReportName,s.PageName,s.HelpURL,s.PagePriority,s.HeaderPriority  from Security_Rights_Details s,security_user_right r where s.Rights_Code=r.rights_code   " + grouporusercode + " and s.ModuleName='Inventory' order by headerpriority ,pagepriority";//college_code=" + Session["collegecode"] + "

            ds = d2.select_method_wo_parameter(q2, "text");
            DataTable dtt = new DataTable();
            if (ds.Tables[0].Rows.Count > 0)
            {
                dtt.Columns.Add("ModuleName");
                dtt.Columns.Add("HeaderName");
                dtt.Columns.Add("ReportId");
                dtt.Columns.Add("ReportName");
                dtt.Columns.Add("PageName");
                dtt.Columns.Add("HelpURL");

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr;
                    dr = dtt.NewRow();
                    dr[0] = Convert.ToString(ds.Tables[0].Rows[i]["ModuleName"]);
                    dr[1] = Convert.ToString(ds.Tables[0].Rows[i]["HeaderName"]);
                    dr[2] = Convert.ToString(ds.Tables[0].Rows[i]["ReportId"]);
                    dr[3] = Convert.ToString(ds.Tables[0].Rows[i]["ReportName"]);
                    dr[4] = Convert.ToString(ds.Tables[0].Rows[i]["PageName"]);
                    dr[5] = Convert.ToString(ds.Tables[0].Rows[i]["HelpURL"]);
                    dtt.Rows.Add(dr);
                }
                if (dtt.Rows.Count > 0)
                {
                    importgrid.DataSource = dtt;
                    importgrid.DataBind();
                    importgrid.Visible = true;
                }
                for (int ik = 0; ik < importgrid.Rows.Count; ik++)
                {
                    Label sno = (Label)importgrid.Rows[ik].Cells[0].FindControl("lbl_sno");
                    Label headername = (Label)importgrid.Rows[ik].Cells[1].FindControl("lblModul_name");
                    Label reportrid = (Label)importgrid.Rows[ik].Cells[2].FindControl("lbl_rid");
                    LinkButton menu = (LinkButton)importgrid.Rows[ik].Cells[3].FindControl("lbl_menu");
                    Label help = (Label)importgrid.Rows[ik].Cells[4].FindControl("lbl_help");
                    if (headername.Text == "Master")
                    {
                        sno.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                        headername.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                        reportrid.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                        menu.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                        help.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                    }
                    if (headername.Text == "Operation")
                    {
                        sno.ForeColor = Color.Black;
                        headername.ForeColor = Color.Black;
                        reportrid.ForeColor = Color.Black;
                        menu.ForeColor = Color.Black;
                        help.ForeColor = Color.Black;
                    }
                    if (headername.Text == "Report")
                    {
                        sno.ForeColor = Color.Green;
                        headername.ForeColor = Color.Green;
                        reportrid.ForeColor = Color.Green;
                        menu.ForeColor = Color.Green;
                        help.ForeColor = Color.Green;
                    }
                }
            }
        }
        catch { }
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        Response.Redirect("i_patch_master.aspx");
    }
    protected void importgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        ////Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";


    }
    protected void importgrid_span(object sender, EventArgs e)
    {
        try
        {
            for (int i = importgrid.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = importgrid.Rows[i];
                GridViewRow previousRow = importgrid.Rows[i - 1];
                for (int j = 1; j <= 1; j++)
                {
                    Label lnlname = (Label)row.FindControl("lblModul_name");
                    Label lnlname1 = (Label)previousRow.FindControl("lblModul_name");
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
        catch
        {
        }
    }
}