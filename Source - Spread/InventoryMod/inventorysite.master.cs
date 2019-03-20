using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class inventorysite : System.Web.UI.MasterPage
{
    DAccess2 da = new DAccess2();
    static string grouporusercode = string.Empty;
    string sql = string.Empty;
    ArrayList rights = new ArrayList();

    protected void Page_Load(object sender, EventArgs e)
    {
        //string strPreviousPage = "";
        //if (Request.UrlReferrer != null)
        //{
        //    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
        //}
        //if (strPreviousPage == "")
        //{
        //    Session["IsLogin"] = "0";
        //    Response.Redirect("~/Default.aspx");
        //}


        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        string group_code = Convert.ToString(Session["group_code"]);
        if (group_code.Contains(";"))
        {
            string[] group_semi = group_code.Split(';');
            group_code = group_semi[0].ToString();
        }
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            grouporusercode = " group_code=" + group_code + "";
        else
            grouporusercode = " user_code=" + Session["usercode"].ToString().Trim() + "";
        string collegecode = Session["Collegecode"].ToString();

        string collegeName = da.GetFunction("select collname from collinfo where  college_code='" + collegecode + "'");

        if (da.GetFunction("select LinkValue from New_InsSettings where LinkName='UseCommonCollegeCode' and user_code ='" + Session["UserCode"].ToString() + "'") == "1")
        {
            string comCOde = da.GetFunction("select com_name from collinfo where  college_code='" + collegecode + "'").Trim();
            collegeName = (comCOde.Length > 1) ? comCOde : collegeName;
        }
        lblcolname.Text = collegeName;

        //lblcolname.Text = da.GetFunction("select collname from collinfo where  college_code='" + collegecode + "'");
        string color = da.GetFunction("select Farvour_color from user_color where user_code='" + Session["UserCode"].ToString() + "' and college_code='" + collegecode + "'");
        string colornew = "";
        if (color.Trim() == "0")
        {
            colornew = "#06d995";
        }
        else
        {
            colornew = color;
            //prewcolor.Attributes.Add("style", "background-color:" + colornew + ";");
        }
        if (!IsPostBack)
        {
            MainDivIdValue.Attributes.Add("style", "background-color:" + colornew + ";border-bottom: 6px solid lightyellow; box-shadow: 0 0 11px -4px; height: 58px; left: 0; position: fixed; z-index: 2; top: 0; width: 100%;");
            if (Convert.ToString(Session["Staff_Code"]) != "")
            {
                img_stfphoto.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + Session["Staff_Code"];
                imgstdphoto.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + Session["Staff_Code"];
                string stfdescode = "";
                sql = "select desig_code from stafftrans where staff_code='" + Convert.ToString(Session["Staff_Code"]) + "' and latestrec=1";
                stfdescode = da.GetFunction(sql);


                if (stfdescode != "" && stfdescode != null)
                {
                    string stfdesigname = "";
                    sql = "select dm.desig_name from desig_master dm where dm.desig_code='" + stfdescode.ToString() + "' and collegecode=" + Session["collegecode"].ToString();
                    stfdesigname = da.GetFunction(sql);



                    string staffname = "";
                    sql = "select staff_name from staffmaster where staff_code='" + Session["staff_code"] + "'";
                    staffname = da.GetFunction(sql);

                    string deptname = "";
                    sql = "select dt.dept_acronym from Department dt,stafftrans st where dt.Dept_code=st.dept_code and staff_code='" + Session["staff_code"] + "' and latestrec=1";
                    deptname = da.GetFunction(sql);
                    lbslstaffname.Text = Convert.ToString(staffname);
                    lbldesignation.Text = Convert.ToString(stfdesigname);
                    lbldept.Text = Convert.ToString(deptname);

                }
            }
            else
            {


                string staffname = "";
                sql = "select full_name from usermaster where user_code='" + Session["UserCode"] + "'";
                staffname = da.GetFunction(sql);
                lbslstaffname.Text = Convert.ToString(staffname);

            }
        }
        try
        {
            EntryCheck();
            DataSet dsRights = new DataSet();
            DataTable dtOutput = new DataTable();
            DataView dvnew = new DataView();
            string SelQ = string.Empty;
            SelQ = "  select distinct HeaderName from Security_Rights_Details where Rights_Code in(select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Inventory'";
            SelQ = SelQ + " select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Inventory'  order by HeaderPriority, PagePriority asc";
            dsRights = da.select_method_wo_parameter(SelQ, "Text");
            if (dsRights.Tables.Count > 0 && dsRights.Tables[0].Rows.Count > 0 && dsRights.Tables[1].Rows.Count > 0)
            {
                dsRights.Tables[1].DefaultView.RowFilter = " HeaderName='Master'";
                dvnew = dsRights.Tables[1].DefaultView;
                if (dvnew.Count > 0)
                {
                    MasterList.Visible = true;
                    for (int tab1 = 0; tab1 < dvnew.Count; tab1++)
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        tabs1.Controls.Add(li);
                        tabs1.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 15px;");
                        HtmlGenericControl anchor = new HtmlGenericControl("a");
                        anchor.Attributes.Add("target", "_blank");
                        anchor.Attributes.Add("href", Convert.ToString(dvnew[tab1]["PageName"]));
                        anchor.InnerText = Convert.ToString(dvnew[tab1]["ReportName"]);
                        li.Controls.Add(anchor);
                    }
                }
                else
                    MasterList.Visible = false;
                dsRights.Tables[1].DefaultView.RowFilter = " HeaderName='Operation'";
                dvnew = dsRights.Tables[1].DefaultView;
                if (dvnew.Count > 0)
                {
                    OperationList.Visible = true;
                    for (int tab2 = 0; tab2 < dvnew.Count; tab2++)
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        tabs2.Controls.Add(li);
                        tabs2.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 15px;");
                        HtmlGenericControl anchor = new HtmlGenericControl("a");
                        anchor.Attributes.Add("target", "_blank");
                        anchor.Attributes.Add("href", Convert.ToString(dvnew[tab2]["PageName"]));
                        anchor.InnerText = Convert.ToString(dvnew[tab2]["ReportName"]);
                        li.Controls.Add(anchor);
                    }
                }
                else
                    OperationList.Visible = false;
                dsRights.Tables[1].DefaultView.RowFilter = " HeaderName='Report'";
                dvnew = dsRights.Tables[1].DefaultView;
                if (dvnew.Count > 0)
                {
                    ReportList.Visible = true;
                    for (int tab3 = 0; tab3 < dvnew.Count; tab3++)
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        tabs3.Controls.Add(li);
                        tabs3.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 15px;");
                        HtmlGenericControl anchor = new HtmlGenericControl("a");
                        anchor.Attributes.Add("target", "_blank");
                        anchor.Attributes.Add("href", Convert.ToString(dvnew[tab3]["PageName"]));
                        anchor.InnerText = Convert.ToString(dvnew[tab3]["ReportName"]);
                        li.Controls.Add(anchor);
                    }
                }
                else
                    ReportList.Visible = false;
                dsRights.Tables[1].DefaultView.RowFilter = " HeaderName='Chart'";
                dvnew = dsRights.Tables[1].DefaultView;
                if (dvnew.Count > 0)
                {
                    ChartList.Visible = true;
                    for (int tab4 = 0; tab4 < dvnew.Count; tab4++)
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        tabs4.Controls.Add(li);
                        tabs4.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 15px;");
                        HtmlGenericControl anchor = new HtmlGenericControl("a");
                        anchor.Attributes.Add("target", "_blank");
                        anchor.Attributes.Add("href", Convert.ToString(dvnew[tab4]["PageName"]));
                        anchor.InnerText = Convert.ToString(dvnew[tab4]["ReportName"]);
                        li.Controls.Add(anchor);
                    }
                }
                else
                    ChartList.Visible = false;
            }
        }
        catch { }
        LiteralControl ltr = new LiteralControl();
        ltr.Text = "<style type=\"text/css\" rel=\"stylesheet\">" +
                    @"#showmenupages .has-sub ul li:hover a
                                                {
color:lightyellow;
                                                    background-color:" + colornew + @";

                                                }
#showmenupages .has-sub ul li a
        {
border-bottom: 1px dotted " + colornew + @";
}
ul li
{
  border-bottom: 1px dotted " + colornew + @";
            border-right: 1px dotted " + colornew + @";
}
ul li:hover
        {
color:lightyellow;
 background-color:" + colornew + @";
}
a:hover
        {
color:lightyellow;
}
                                                </style>
                                                ";
        this.Page.Header.Controls.Add(ltr);
    }

    private void EntryCheck()
    {
        try
        {
            //master
            rights.Clear();
            rights.Add("Inventory,Master,901,INV001,Code Master,Inv_CodeMaster.aspx,~/helpcontentinvpages/invcodesetting.htm,1,1");
            rights.Add("Inventory,Master,902,INV002,Store Master,Hm_StoreMasterNew.aspx,~/helpcontentinvpages/storemaster.htm,1,2");
            rights.Add("Inventory,Master,903,INV003,Item Master,Item_master.aspx,~/helpcontentinvpages/itemmaster.htm,1,3");
            rights.Add("Inventory,Master,904,INV004,Supplier Master,Supplier_master.aspx,~/helpcontentinvpages/suppliermaster.htm,1,4");
            rights.Add("Inventory,Master,906,INV005,Kit Master,Kit_Master.aspx,~/helpcontentinvpages/kitmaster.htm,1,5");

            //operation
            rights.Add("Inventory,Operation,905,INVO001,Opening Stock,inv_opening_stock.aspx,~/helpcontentinvpages/openingstock.htm,2,5");
            //rights.Add("Inventory,Operation,906,INVO002,Request Hierarchy,HierarchySetting.aspx,~/helpcontentinvpages/.htm,2,6");
            //rights.Add("Inventory,Operation,907,INVO003,Request,Request.aspx,~/helpcontentinvpages/.htm,2,7");
            //rights.Add("Inventory,Operation,908,INVO004,Request Report,request_report.aspx,~/helpcontentinvpages/.htm,2,8");
            // rights.Add("Inventory,Operation,909,INVO005,Approval for Request,request_vendor.aspx,,2,9");
            rights.Add("Inventory,Operation,909,INVO002,Request to Vendor,request_vendor.aspx,~/helpcontentinvpages/requestvendor.htm,2,9");
            rights.Add("Inventory,Operation,910,INVO003,Quotation Entry,vendor_quatation_request.aspx,~/helpcontentinvpages/vendorquotation.htm,2,10");
            rights.Add("Inventory,Operation,911,INVO004,Quotation Comparison,vendor_quotation_compare.aspx,~/helpcontentinvpages/Quotationcompare.htm,2,11");
            rights.Add("Inventory,Operation,912,INVO005,Purchase Order,inv_Purchase.aspx,~/helpcontentinvpages/purchaseorder.htm,2,12");
            rights.Add("Inventory,Operation,913,INVO006,Approval for Purchase Order,inv_purchaseorder_request.aspx,~/helpcontentinvpages/purchaserequestandapprove.htm,2,13");
            rights.Add("Inventory,Operation,914,INVO007,Inward,inv_inward.aspx,~/helpcontentinvpages/inward.htm,2,14");
            //rights.Add("Inventory,Operation,916,INVO012,Return to Vendor,HT_Income.aspx,,2,16");
            //rights.Add("Inventory,Operation,917,INVO013,Issue from Store,HM_Expanses.aspx,,2,17");
            rights.Add("Inventory,Operation,915,INVO008,Transfer,Inv_Transfer.aspx,~/helpcontentinvpages/Transfer.htm,2,15");
            rights.Add("Inventory,Operation,916,INVO009,Item Usage,itemusuage.aspx,~/helpcontentinvpages/itemusage.htm,2,16");
            rights.Add("Inventory,Operation,917,INVO010,Item Missing / Scrap and Breakage,breakage_entry.aspx,~/helpcontentinvpages/breakage.htm,2,17");
            rights.Add("Inventory,Operation,907,INVO011,Student Kit Allotment,Student_Kit_Allotment.aspx,~/helpcontentinvpages/kitallotment.htm,2,18");

            //report
            rights.Add("Inventory,Report,919,INVR001,Department Stock Status Report,Inv_Dept_stockstatus_Report.aspx,~/helpcontentinvpages/deptmentstockstatusreport.htm,3,18");
            rights.Add("Inventory,Report,920,INVR002,Purchase Order setting,purchase_order_setting.aspx,~/helpcontentinvpages/purchaseordersettings.htm,3,19");
            rights.Add("Inventory,Report,921,INVR003,Purchase Status Report,HM_Purchasestatus_Report.aspx,~/helpcontentinvpages/purchasestatusreport.htm,3,20");
            rights.Add("Inventory,Report,922,INVR004,Stock Status Report,HM_Stock_Status_Report.aspx,~/helpcontentpages/Stockstatusreport.htm,3,21");
            rights.Add("Inventory,Report,923,INVR005,Purchase Order print Settings,Investorsposetting.aspx,~/helpcontentpages/purchaseorderprintsettings.htm,3,22");
            rights.Add("Inventory,Report,924,INVR006,Transfer Details,Transfer_details.aspx,~/helpcontentinvpages/Transferdetails.htm,3,23");
            rights.Add("Inventory,Report,925,INVR007,Item Rate Information,Itemrateinformation.aspx,~/helpcontentinvpages/itemrateinformation.htm,3,24");
            rights.Add("Inventory,Report,908,INVR008,Student Kit Report,Student_Kit_Report.aspx,~/helpcontentinvpages/kitreport.htm,3,25");

            for (int i = 0; i < rights.Count; i++)
            {
                string[] index = Convert.ToString(rights[i]).Split(',');
                string q1 = " if exists(select ReportName from  Security_Rights_Details where ModuleName='" + index[0].ToString() + "' and Rights_Code='" + index[2].ToString() + "' ) update Security_Rights_Details set HeaderName='" + index[1].ToString() + "',ReportId='" + index[3].ToString() + "',ReportName='" + index[4].ToString() + "',PageName='" + index[5].ToString() + "',HelpURL='" + index[6].ToString() + "',HeaderPriority='" + index[7].ToString() + "',PagePriority='" + index[8].ToString() + "' where ModuleName='" + index[0].ToString() + "' and Rights_Code='" + index[2].ToString() + "' else insert into Security_Rights_Details (ModuleName,HeaderName,Rights_Code,ReportId,ReportName,PageName,HelpURL,HeaderPriority,PagePriority) values ('" + index[0].ToString() + "','" + index[1].ToString() + "','" + index[2].ToString() + "','" + index[3].ToString() + "','" + index[4].ToString() + "','" + index[5].ToString() + "','" + index[6].ToString() + "','" + index[7].ToString() + "','" + index[8].ToString() + "')";
                int insert = da.update_method_wo_parameter(q1, "text");
            }
        }
        catch { }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }

}
