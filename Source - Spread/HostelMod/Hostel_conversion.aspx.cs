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

public partial class Hostel_conversion : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
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
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("default.aspx", false);
        }
        catch
        {
        }
    }
    protected void btn_conversion_Click(object sender, EventArgs e)
    {
        
        try
        {
            ConvertHostel();
        }
        catch (Exception ex)
        {
            lbl_convert.Font.Bold = false;
            lbl_convert.Visible = true;
            lbl_convert.ForeColor = Color.Red;
            lbl_convert.Text = ex.ToString();
        }



        try
        {
            /* 06.10.2016 Mcc Conversion student so this are commanded
            string except = "";
            lbl_convert.Visible = false;
            int insert = 0;
            int i = 0;
            string q1 = "";
            string q2 = "";
            string q3 = "";
            bool codesetting = false;
            bool messmaster = false;
            bool storemaster = false;
            bool storedept = false;
            bool itemmaster = false;
            bool itemdeptmaster = false;
            bool vendormaster = false;
            bool vendeptmaster = false;
            bool Hostelstaff = false;
            bool guestcontectreg = false;
            bool guestcontectdet = false;
            bool guestregister = false;
            bool studentmentor = false;
            bool sessionmaster = false;
            bool menumaster = false;
            bool hostelmasterattence = false;
            bool menuitemdetails = false;
            bool menuschedule = false;
            bool openningstock = false;
            bool purchaseorder = false;
            bool Dailyconsumption = false;
            bool GoodInward = false;
            bool Income = false;
            bool Expances = false;
            bool Rebate = false;
            bool StudentRebateDetails = false;
            bool Studentadditionaldetails = false;
            bool MenucostMaster = false;
            bool Studenttokendetails = false;
            bool Messbill = false;
            bool deletetable = false;
            bool messdetails = false;
            bool vendorbankdetails = false;
            bool Hostelsetting = false;
            bool cleanitem = false;
            bool vendorcontactdel = false;
            
            
            try
            {
                #region Deletetable


                q1 = "delete IM_VendorBankMaster";
                insert = d2.update_method_wo_parameter(q1, "Text");

                q1 = "delete IM_CodeSettings";
                insert = d2.update_method_wo_parameter(q1, "Text");

                q1 = "delete CO_StudentTutor";
                insert = d2.update_method_wo_parameter(q1, "Text");

                q1 = "delete HT_Attendance";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "delete  HM_MenuItemDetail";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = " delete HM_MenuItemMaster";
                insert = d2.update_method_wo_parameter(q1, "Text");
                //q1 = "delete HT_MenuSchedule";

                //q1 = "delete CleaningitemMaseter_temp   delete CleaningItemDetailMaster_temp";
                //insert = d2.update_method_wo_parameter(q1, "Text");

                q1 = "delete HT_MenuSchedule";
                insert = d2.update_method_wo_parameter(q1, "text");
                q1 = "delete IT_StockDetail";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "delete IT_StockDeptDetail";
                insert = d2.update_method_wo_parameter(q1, "Text");

                q1 = "delete IT_PurchaseOrderDetail";
                q1 = q1 + " delete it_purchaseorder";
                insert = d2.update_method_wo_parameter(q1, "Text");

                q1 = " delete HT_DailyConsumptionDetail";
                q1 = q1 + " delete HT_DailyConsumptionMaster";
                insert = d2.update_method_wo_parameter(q1, "Text");

                q1 = "delete IT_GoodsInward";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "delete HT_HostelIncome";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "delete HT_HostelExpenses";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "delete HM_RebateMaster";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "delete HT_HostelRebateDetail";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "delete HT_StudAdditionalDet";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "delete HM_MenuCostMaster";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "delete HT_StudTokenDetails";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "delete HT_MessBillDetail";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "delete HT_MessBillMaster";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "delete HMessbill_StudDetails";
                insert = d2.update_method_wo_parameter(q1, "Text");

                q1 = "delete IM_StoreMaster";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "delete IM_StoreDeptDet";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "delete IM_ItemMaster";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "delete IM_ItemDeptMaster";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "delete CO_VendorMaster";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "delete IM_VendorItemDept";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = " delete IM_VendorContactMaster";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "delete HT_HostelRegistration";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "delete HM_HostelMaster";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "delete HM_MenuMaster";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "delete HM_SessionMaster";
                insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "delete HM_MessMaster";
                insert = d2.update_method_wo_parameter(q1, "Text");
                #endregion
            }
            catch (Exception ex)
            {
                deletetable = true;
                except = Convert.ToString(ex.Message);
            }
            ConvertHostel();
           
           

            #region Code setting
            try
            {
                //q1 = "delete IM_CodeSettings";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "";
                q1 = "insert into IM_CodeSettings(ItemAcr,ItemStNo,ItemSize,VenAcr,VenStNo,VenSize,ReqAcr,ReqStNo,ReqSize,QuoAcr,QuoStNo,QuoSize,POAcr,POStNo,POSize,GIAcr,GIStNo,GISize,CollegeCode,CustAcr,CustStNo,CustSize,StartDate,GRAcr,GRStNo,GRSize,AssetAcr,AssetStNo,AssetSize,ItemHeaderAcr,ItemHeaderStNo,ItemHeaderSize,IncludeHeaderAcr,MenuAcr,MenuStNo,MenuSize)select Item_Acr,Item_StNo,Item_Size,Vendor_Acr,Vendor_StNo,Vendor_Size,Requisition_Acr,Requisition_StNo,Requisition_Size,Quotation_Acr,Quotation_StNo,Quotation_Size,Order_Acr,Order_StNo,Order_Size,Inward_Acr,inward_StNo,Inward_Size,College_Code,Customer_Acr,Customer_StNo,Customer_Size,From_Date,Return_Acr,Return_StNo,Return_Size,Asset_Acr,Asset_StNo,Asset_Size,itemheader_Acr,itemheader_stNo,itemheader_size,include_header,Menuid_Acr,Menuid_StNo,Menuid_Size from invcode_settings ";
                insert = d2.update_method_wo_parameter(q1, "Text");

            }
            catch (Exception ex)
            {
                codesetting = true;
                except = except + "-" + Convert.ToString(ex.Message);
            }
            #endregion

            #region Mess master

            try
            {
                //q1 = "delete HM_MessMaster";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "insert into HM_MessMaster(MessAcr,MessName,MessStartYear,CollegeCode)select MessAcr,MessName,StartYear,College_Code from MessMaster";
                insert = d2.update_method_wo_parameter(q1, "Text");
            }
            catch (Exception ex)
            {
                messmaster = true;
                except = except + "-" + Convert.ToString(ex.Message);
            }
            #endregion

            #region messDetails
            try
            {
                q1 = "";
                q1 = "select Hostel_code,HostelName,hm.HostelMasterPK,Staff_Code  from Hostel_Details h,HM_HostelMaster hm where h.Hostel_Name=hm.HostelName ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q2 = "";
                        q2 = d2.GetFunction("select MessMasterPK  from messdetail m,MessMaster mm,HM_MessMaster hm where m.MessID=mm.MessID and hm.MessName=mm.MessName and m.hostel_code='" + Convert.ToString(ds.Tables[0].Rows[i]["Hostel_code"]) + "'");
                        string appl_id = "";
                        appl_id = d2.GetFunction("select appl_id from staff_appl_master a,staffmaster s where s.appl_no=a.appl_no and s.staff_code='" + Convert.ToString(ds.Tables[0].Rows[i]["Staff_Code"]) + "' ");
                        if (q2.Trim() != "0")
                        {
                            q1 = "";
                            q1 = "update HM_HostelMaster set MessMasterFK='" + q2 + "',WardenStaff1PK='" + appl_id + "' where HostelMasterPK='" + Convert.ToString(ds.Tables[0].Rows[i]["HostelMasterPK"]) + "'";
                            insert = d2.update_method_wo_parameter(q1, "text");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                messdetails = true;
                except = except + "-" + Convert.ToString(ex.Message);
            }

            #endregion

            #region Store master
            try
            {
                //q1 = "delete IM_StoreMaster";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "";
                q1 = "insert into IM_StoreMaster(StoreAcr,StoreName,StoreStartYear,CollegeCode)select Store_Acr,Store_Name,Start_Year,College_Code from StoreMaster ";
                insert = d2.update_method_wo_parameter(q1, "Text");
            }
            catch (Exception ex)
            {
                storemaster = true;
                except = except + "-" + Convert.ToString(ex.Message);
            }
            #endregion

            #region  Store department details
            try
            {
                //q1 = "delete IM_StoreDeptDet";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "";
                q1 = "select Dept_code,Store_Code from StoreDetails ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q2 = d2.GetFunction("select StorePK from StoreDetails d,StoreMaster s,IM_StoreMaster sm where d.Store_Code=s.Store_Code and s.Store_Name=sm.StoreName and d.Store_Code='" + Convert.ToString(ds.Tables[0].Rows[i]["Store_Code"]) + "'");
                        q1 = "insert into IM_StoreDeptDet(DeptCode,StoreFK)values('" + Convert.ToString(ds.Tables[0].Rows[i]["Dept_code"]) + "','" + q2 + "')";
                        insert = d2.update_method_wo_parameter(q1, "Text");
                        //q1 = "insert into IM_StoreDeptDet(DeptCode,StoreFK)select Dept_code,Store_Code from StoreDetails ";
                    }
                }
            }
            catch (Exception ex)
            {
                storedept = true;
                except = except + "-" + Convert.ToString(ex.Message);
            }
            #endregion

            #region Item master
            try
            {
                //q1 = "delete IM_ItemMaster";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "";
                //q1 = "insert into IM_ItemMaster(ItemHeaderCode,ItemHeaderName,ItemCode,ItemName,ItemTamilName,ItemModel,ItemSize,ItemUnit,ItemSpecification,ItemType,StoreFK,ForHostelItem,subheader_code)select itemheader_code,itemheader_name,item_code,item_name,Tamil_ItemName, model_name,Size_name, item_unit,Special_instru, item_type,Store_Code,Is_Hostel,subheader_code from item_master ";
                //insert = d2.update_method_wo_parameter(q1, "Text");

                q1 = "select itemheader_code,itemheader_name,item_code,item_name,Tamil_ItemName, model_name,Size_name, item_unit,Special_instru, item_type,Store_Code,Is_Hostel,subheader_code from item_master ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q1 = "";
                        q1 = d2.GetFunction("select distinct t.TextVal  from TextValTable t,item_master i where t.TextCode=i.subheader_code and i.subheader_code='" + Convert.ToString(ds.Tables[0].Rows[i]["subheader_code"]) + "'");

                        string clgcode = d2.GetFunction("select college_code from hostel_details ");

                        q2 = "";
                        q2 = "if exists(select*from CO_MasterValues where MasterValue='" + q1 + "' and MasterCriteria='Subheader') update CO_MasterValues set MasterValue='" + q1 + "' where MasterValue='" + q1 + "' and MasterCriteria='Subheader' else insert into CO_MasterValues (MasterValue,CollegeCode,MasterCriteria)values('" + q1 + "','" + clgcode + "','Subheader')";
                        insert = d2.update_method_wo_parameter(q2, "Text");
                        string groupcode = d2.GetFunction("select mastercode from co_mastervalues where MasterValue='" + q1 + "' and MasterCriteria='Subheader'");

                        q2 = "";
                        q2 = "insert into IM_ItemMaster(ItemHeaderCode,ItemHeaderName,ItemCode,ItemName,ItemTamilName,ItemModel,ItemSize,ItemUnit,ItemSpecification,ItemType,StoreFK,ForHostelItem,subheader_code)values('" + Convert.ToString(ds.Tables[0].Rows[i][0]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][1]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][2]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][3]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][4]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][5]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][6]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][7]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][8]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][9]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][10]) + "','" + Convert.ToInt32(ds.Tables[0].Rows[i][11]) + "','" + groupcode + "')";
                        insert = d2.update_method_wo_parameter(q2, "text");
                    }
                }
            }
            catch (Exception ex) { itemmaster = true; except = except + "-" + Convert.ToString(ex.Message); }
            #endregion

            #region item dept details
            try
            {
                //q1 = "delete IM_ItemDeptMaster";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "";
                q1 = "insert into IM_ItemDeptMaster(ItemDeptFK,ItemFK)select dept_code,i.itempk from Item_DeptDetails id,IM_ItemMaster i where id.Item_Code=i.ItemCode";
                insert = d2.update_method_wo_parameter(q1, "Text");
            }
            catch (Exception ex) { itemdeptmaster = true; except = except + "-" + Convert.ToString(ex.Message); }
            #endregion

            #region Vendor Master
            try
            {
                //q1 = "delete CO_VendorMaster";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "";
                //q1 = " insert into CO_VendorMaster (VendorCode,VendorCompName,VendorAddress,VendorCity,VendorPin,VendorPhoneNo,VendorFaxNo,VendorEmailID,VendorWebsite,VendorStartYear,VendorPayType,VendorStatus,VendorBlockFrom,VendorBlockTo,VendorCSTNo,VendorTINNo,VendorPANNo)select vendor_code,vendor_name,vendor_address,Vendor_City,pin,phone_no,fax_no,email,web_name,Vendor_Start_Year,PayType,case when vendor_type='Approved' then 1 when vendor_type='Blocked' then 2 end vendor_type, BlockFrom,BlockTo,cst_no,tin_no,PAN_No from vendor_details ";
                //insert = d2.update_method_wo_parameter(q1, "Text");

                q1 = "select vendor_code,vendor_name,vendor_address,Vendor_City,pin,phone_no,fax_no,email,web_name, Vendor_Start_Year, PayType,case when vendor_type='Approved' then 1 when vendor_type='Blocked' then 2 end vendor_type, BlockFrom,BlockTo,cst_no,tin_no,PAN_No from vendor_details ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q2 = "";
                        string startyear = "";
                        q2 = Convert.ToString(ds.Tables[0].Rows[i]["Vendor_Start_Year"]);
                        if (q2.Trim() != "")
                        {
                            string[] split = q2.Split('/');
                            if (split.Length > 1)
                            {
                                startyear = Convert.ToString(split[1]);
                            }
                            else
                            {
                                startyear = Convert.ToString(split[0]);
                            }
                        }
                        else
                        {
                            startyear = "";
                        }

                        q1 = "";
                        q1 = "insert into CO_VendorMaster (VendorCode,VendorCompName,VendorAddress,VendorCity,VendorPin,VendorPhoneNo,VendorFaxNo,VendorEmailID,VendorWebsite,VendorStartYear,VendorPayType,VendorStatus,VendorBlockFrom,VendorBlockTo,VendorCSTNo,VendorTINNo,VendorPANNo,VendorType) values ('" + Convert.ToString(ds.Tables[0].Rows[i][0]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][1]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][2]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][3]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][4]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][5]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][6]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][7]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][8]) + "','" + startyear + "','" + Convert.ToString(ds.Tables[0].Rows[i][10]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][11]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][12]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][13]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][14]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][15]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][16]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][11]) + "')";

                        insert = d2.update_method_wo_parameter(q1, "Text");
                    }
                }
            }
            catch (Exception ex) { vendormaster = true; except = except + "-" + Convert.ToString(ex.Message); }
            #endregion

            #region vendorcontact details
            try
            {
                q1 = "";
                //q1 = "insert into IM_VendorContactMaster(VendorFK,VenContactName,VendorPhoneNo,VendorMobileNo,VenContactDesig,VendorExtNo,VendorEmail)select (select vm.VendorPK from CO_VendorMaster vm where c.Vendor_Code=vm.VendorCode)as vendorFK, Contact_Name,Contact_PhoneNo,ContactMobileNo,Contact_Desig,Contact_FaxNo,Contact_Email from Vendor_ContactDetails c";

                q1 = "select Vendor_Code,Contact_Name,Contact_PhoneNo,ContactMobileNo,Contact_Desig,Contact_FaxNo,Contact_Email from Vendor_ContactDetails";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q3 = "";
                        q3 = d2.getvenpk(Convert.ToString(ds.Tables[0].Rows[i]["Vendor_Code"]));
                        q2 = "";
                        q2 = "insert into IM_VendorContactMaster(VendorFK,VenContactName,VendorPhoneNo,VendorMobileNo, VenContactDesig,VendorExtNo,VendorEmail)values('" + q3 + "','" + Convert.ToString(ds.Tables[0].Rows[i]["Contact_Name"]) + "','" + Convert.ToString(ds.Tables[0].Rows[i]["Contact_PhoneNo"]) + "','" + Convert.ToString(ds.Tables[0].Rows[i]["ContactMobileNo"]) + "','" + Convert.ToString(ds.Tables[0].Rows[i]["Contact_Desig"]) + "','" + Convert.ToString(ds.Tables[0].Rows[i]["Contact_FaxNo"]) + "','" + Convert.ToString(ds.Tables[0].Rows[i]["Contact_Email"]) + "')";
                        insert = d2.update_method_wo_parameter(q2, "Text");
                    }
                }

                insert = d2.update_method_wo_parameter(q1, "Text");
            }
            catch (Exception ex)
            {
                vendorcontactdel = true; except = except + "-" + Convert.ToString(ex.Message);
            }
            #endregion

            #region Vendor item dept
            try
            {
                //q1 = "delete IM_VendorItemDept";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "";
                q1 = "  insert into  IM_VendorItemDept(VenItemFK,VenItemDeptFK,VenItemSupplyDur,VenItemIsSupplied,VenItemReference,ItemFK)select c.VendorPK,Dept_Code,duration,Already_Supplid,reference,i.itempk from Vendor_ItemDetails v,IM_ItemMaster i,CO_VendorMaster c where i.ItemCode=v.Item_Code and c.VendorCode=v.Vendor_Code ";
                insert = d2.update_method_wo_parameter(q1, "Text");
            }
            catch (Exception ex) { vendeptmaster = true; except = except + "-" + Convert.ToString(ex.Message); }
            #endregion

            #region vendorbankdetails
            try
            {
                q1 = "";
                q1 = "select vendor_code,Banker_Name,Bank_Branch,Bank_AcNo,IFSC_Code,SWIFT_Code from vendor_details ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q3 = "";
                        q3 = Convert.ToString(ds.Tables[0].Rows[i]["Banker_Name"]);
                        //if (q3.Trim() != "")
                        //{
                        q2 = "";
                        q2 = d2.GetFunction("select vendorpk  from CO_VendorMaster where vendorcode='" + Convert.ToString(ds.Tables[0].Rows[i]["vendor_code"]) + "'");

                        q1 = "";
                        q1 = "insert into IM_VendorBankMaster (VenBankName,VenBankBranch,VenBankHolderName,VendorAccName,VendorAccNo, VendorBankIFSCCode,VendorBankSWIFTCode,VendorFK)values ('" + Convert.ToString(ds.Tables[0].Rows[i]["Banker_Name"]) + "','" + Convert.ToString(ds.Tables[0].Rows[i]["Bank_Branch"]) + "','','','" + Convert.ToString(ds.Tables[0].Rows[i]["Bank_AcNo"]) + "','" + Convert.ToString(ds.Tables[0].Rows[i]["IFSC_Code"]) + "','" + Convert.ToString(ds.Tables[0].Rows[i]["SWIFT_Code"]) + "','" + q2 + "')";
                        insert = d2.update_method_wo_parameter(q1, "text");
                        //}
                    }
                }
            }
            catch (Exception ex) { vendorbankdetails = true; except = except + "-" + Convert.ToString(ex.Message); }

            #endregion

            #region Hostel Registeration staff
            try
            {
                q1 = "";
                q1 = " insert into HT_HostelRegistration (MemType,APP_No,HostelAdmDate,BuildingFK,FloorFK,RoomFK,MessType,StudMessType,IsSuspend,IsDiscontinued,DiscontinueDate,IsVacated,VacatedDate,Reason,StudHostelGateStatus,HostelMasterFK,h.CollegeCode)select distinct case when Is_Staff='0' then 1 when Is_Staff='1' then 2 end Is_Staff,sm.appl_id ,Admin_Date,bm.Code,f.Floorpk,rd.Roompk,HostelType,StudMess_Type,Suspension,Relived,Relived_Date,Vacated ,Vacated_Date,Reason,h.Status,HostelMasterPK,h.College_Code from Hostel_StudentDetails h,HM_HostelMaster hm,Hostel_Details hd ,Building_Master bm,Floor_Master f,Room_Detail rd,staffmaster s,staff_appl_master sm where Is_Staff='1' and h.Hostel_Code=hd.Hostel_code and hm.HostelName=hd.Hostel_Name and s.staff_code=h.Roll_No and h.Building_Name=bm.Building_Name and f.Floor_Name=h.Floor_Name and rd.Room_Name=h.Room_Name and s.staff_code=h.Roll_No and f.Floor_Name=rd.Floor_Name and s.appl_no=sm.appl_no ";
                insert = d2.update_method_wo_parameter(q1, "Text");
            }
            catch (Exception ex) { Hostelstaff = true; except = except + "-" + Convert.ToString(ex.Message); }
            #endregion

            #region Guest Registeration
            //try
            //{
            //    q1 = "";
            //    q1 = " insert into CO_VendorMaster VendorAddress,VendorCompName,VendorCity,VendorType,VendorDist,VendorState,VendorCode)select Guest_Address,Guest_Name,Guest_City,'10',c.MasterCode,c.MasterCode ,GuestCode from Hostel_GuestReg h,textvaltable t,CO_MasterValues c where  t.TextVal=c.MasterValue";
            //    insert = d2.update_method_wo_parameter(q1, "Text");
            //}
            //catch (Exception ex) { }
            #endregion

            #region Guest contect registeration
            try
            {
                q1 = "";
                q1 = " insert into CO_VendorMaster(VendorAddress,VendorCompName,VendorCity,VendorType,VendorDist,VendorState,VendorCode)select Guest_Address,Guest_Name,Guest_City,'10',c.MasterCode,(select MasterCode from CO_MasterValues where MasterValue=c.MasterCriteriaValue2)as VendorState,GuestCode from Hostel_GuestReg h,textvaltable t,CO_MasterValues c where t.TextCriteria='dis' and t.TextCode=h.district and t.TextVal=c.MasterValue ";
                insert = d2.update_method_wo_parameter(q1, "Text");
            }
            catch (Exception ex) { guestcontectreg = true; except = except + "-" + Convert.ToString(ex.Message); }
            #endregion

            #region Guest contect details
            try
            {
                q1 = "";
                q1 = " insert into IM_VendorContactMaster(VenContactName,VendorMobileNo,VenContactDesig,VenContactDept,VendorFK)select Guest_Name,MobileNo,(select t.TextVal  from Hostel_GuestReg g,textvaltable t where g.Desig_Code=t.TextCode)as Design ,(select t.TextVal  from Hostel_GuestReg g,textvaltable t where g.department=t.TextCode)as Department,(select VendorPK  from Hostel_GuestReg g,CO_VendorMaster c where c.VendorCode=CONVERT(nvarchar(100), g.GuestCode))as VendorPK from Hostel_GuestReg g,textvaltable t where t.TextCode=desig_code";
                insert = d2.update_method_wo_parameter(q1, "Text");
            }
            catch (Exception ex) { guestcontectdet = true; except = except + "-" + Convert.ToString(ex.Message); }
            #endregion

            #region Guest registeration
            try
            {
                q1 = "";
                q1 = "insert into HT_HostelRegistration(BuildingFK,FloorFK,RoomFK,HostelMasterFK,HostelAdmDate,IsVacated,VacatedDate,GuestVendorType,MemType,APP_No,GuestVendorFK)select bm.Code,f.Floorpk,rd.Roompk,hm.HostelMasterPK,Admission_Date,isvacate,vacate_date,'10','3',(select im.VendorContactPK from IM_VendorContactMaster im,CO_VendorMaster co where co.VendorType='10' and im.VenContactName=g.Guest_Name),(select vendorpk from CO_VendorMaster where vendorcode=CONVERT(nvarchar(100), g.GuestCode))as guestVenfk from Hostel_GuestReg g,Building_Master bm,Floor_Master f,Room_Detail rd,Hostel_Details hd,HM_HostelMaster hm where g.Building_Name=bm.Building_Name and f.Floor_Name=g.Floor_Name and rd.Room_Name=g.Room_Name and hd.Hostel_Name=hm.HostelName ";
                insert = d2.update_method_wo_parameter(q1, "Text");
            }
            catch (Exception ex) { guestregister = true; except = except + "-" + Convert.ToString(ex.Message); }
            #endregion

            #region student Mentor
            try
            {
                //q1 = "delete CO_StudentTutor";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "";
                q1 = "insert into CO_StudentTutor(App_No,StaffMasterFK,TutorFor)select r.App_No,(select appl_id  from staff_appl_master sam,staffmaster sm where sam.appl_no = sm.appl_no and sm.staff_code=s.staff_code) as appl_id,'1' from StudentTutor_Details s,Registration r where r.Roll_No=s.Roll_No ";
                insert = d2.update_method_wo_parameter(q1, "Text");
            }
            catch (Exception ex) { studentmentor = true; except = except + "-" + Convert.ToString(ex.Message); }
            #endregion

            #region Session Master
            try
            {
                //q1 = "delete HM_SessionMaster";
                //insert = d2.update_method_wo_parameter(q1, "Text");

                q1 = "";
                q1 = "select distinct  Session_Name,Start_Time,End_Time,Is_Extension,Extension_Time,Hostel_Code from Session_Master ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q2 = "";
                        //q2 = d2.GetFunction("select distinct MessMasterPK   from MessDetail m,MessMaster d,HM_MessMaster hm where m.MessID =d.MessID and hm.MessName =d.MessName and m.MessID='" + Convert.ToString(ds.Tables[0].Rows[i]["Hostel_Code"]) + "'");

                        q2 = d2.GetFunction("select MessMasterpk from MessMaster m,HM_MessMaster mm where m.MessName=mm.MessName and m.MessID='" + Convert.ToString(ds.Tables[0].Rows[i]["Hostel_Code"]) + "'");
                        if (q2.Trim() != "0")
                        {
                            q1 = "insert into HM_SessionMaster (SessionName,SessionStartTime,SessionCloseTime,IsAllowExtTime,SessionCloseExtTime,MessMasterFK)values ('" + Convert.ToString(ds.Tables[0].Rows[i][0]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][1]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][2]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][3]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][4]) + "','" + q2 + "')";
                            insert = d2.update_method_wo_parameter(q1, "Text");
                        }
                        //q1 = "";
                        //q1 = "insert into HM_SessionMaster (SessionName,SessionStartTime,SessionCloseTime,IsAllowExtTime,SessionCloseExtTime,MessMasterFK)select distinct  Session_Name,Start_Time,End_Time,Is_Extension,Extension_Time,Hostel_Code from Session_Master";
                        //insert = d2.update_method_wo_parameter(q1, "Text");
                    }
                }
            }
            catch (Exception ex) { sessionmaster = true; except = except + "-" + Convert.ToString(ex.Message); }
            #endregion

            #region Menu master
            try
            {
                //q1 = "delete HM_MenuMaster";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "";
                q1 = "insert into HM_MenuMaster (MenuCode,MenuName,MenuType,CollegeCode)select MenuID,MenuName,MenuType,College_Code from MenuMaster ";
                insert = d2.update_method_wo_parameter(q1, "Text");
            }
            catch (Exception ex) { menumaster = true; except = except + "-" + Convert.ToString(ex.Message); }
            #endregion

            #region Hostel master Attendence
            try
            {
                //q1 = "delete HT_Attendance";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "";
                q1 = "insert into HT_Attendance (AttnMonth,AttnYear,App_No,D1,D2,D3,D4,D5,D6,D7,D8,D9,D10,D11,D12,D13,D14,D15,D16,D17,D18,D19,D20,D21,D22,D23,D24,D25,D26,D27,D28,D29,D30,D31)select AttnMonth,AttnYear,r.app_no,d1,d2,d3,d4,d5,d6,d7,d8,d9,d10,d11,d12,d13,d14,d15,d16,d17,d18,d19,d20,d21,d22,d23,d24,d25,d26,d27,d28,d29,d30,d31 from HAttendance h,Registration r where h.Roll_No=r.Roll_No and ISNULL( AttnMonth,0)<>0 and ISNULL(AttnYear,0)<>0";
                insert = d2.update_method_wo_parameter(q1, "Text");
            }
            catch (Exception ex) { hostelmasterattence = true; except = except + "-" + Convert.ToString(ex.Message); }
            #endregion

            #region Hostel setting
            try
            {
                q1 = "select Hostel_code,Session_code,Schedule_type,Schedule_date,Schedule_Day,EditMenuTotal,Use_Attendance, Att_Hour,Staff_total,daily_consumption  from HostelIns_settings";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q2 = "";
                        q2 = d2.GetFunction("select hm.SessionMasterPK from HM_SessionMaster hm,Session_Master s where s.Session_Name=hm.SessionName and s.session_code='" + Convert.ToString(ds.Tables[0].Rows[i]["Session_code"]) + "'");
                        if (q2.Trim() != "0")
                        {
                            q3 = "";
                            q3 = d2.GetFunction("select MessMasterPK  from MessMaster m,HM_MessMaster mm where m.MessName=mm.MessName and MessID='2'");
                            q1 = "update HostelIns_settings set Hostel_code='" + q3 + "',Session_code='" + q2 + "' where Hostel_code='" + Convert.ToString(ds.Tables[0].Rows[i]["Hostel_code"]) + "' and Session_code='" + Convert.ToString(ds.Tables[0].Rows[i]["Session_code"]) + "'";
                            insert = d2.update_method_wo_parameter(q1, "text");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Hostelsetting = true; except = except + "-" + Convert.ToString(ex.Message);
            }

            #endregion

            #region Menu item details
            try
            {
                q1 = "";
                insert = 0;
                //q1 = "delete  HM_MenuItemDetail";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                //q1 = " delete HM_MenuItemMaster";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "";
                q1 = "select Menu_ItemMasterCode,SessionMenu_Code,NoOfPersons,Hostel_Code from Menu_ItemMaster";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string menumasterpk = d2.GetFunction("select MenuMasterPK  from MenuMaster m,HM_MenuMaster hm,Menu_ItemMaster h where h.sessionmenu_code=m.MenuCode and hm.MenuCode=m.MenuID  and h.SessionMenu_Code='" + Convert.ToString(ds.Tables[0].Rows[i]["SessionMenu_Code"]) + "'");

                        if (menumasterpk.Trim() != "" && menumasterpk.Trim() != "0")
                        {
                            string clgccode = d2.GetFunction("select collegecode from HM_MessMaster m,MessMaster mm where m.MessName=mm.MessName and mm.MessID='" + Convert.ToString(ds.Tables[0].Rows[i]["Hostel_Code"]) + "'");

                            // string clgccode = d2.GetFunction("select collegecode from HM_MessMaster m,Menu_ItemMaster h where m.MessMasterPK=h.Hostel_Code and h.Hostel_Code='" + Convert.ToString(ds.Tables[0].Rows[i]["Hostel_Code"]) + "'");

                            q1 = "insert into HM_MenuItemMaster (MenuMasterFK,NoOfPerson,CollegeCode) values ('" + menumasterpk + "','" + Convert.ToString(ds.Tables[0].Rows[i]["NoOfPersons"]) + "','" + clgccode + "')";
                            insert = d2.update_method_wo_parameter(q1, "Text");
                            if (insert != 0)
                            {
                                string menumasterfk = d2.GetFunction("select MenuItemMasterPK  from HM_MenuItemMaster where MenuMasterFK='" + menumasterpk + "'");
                                q1 = "select Item_Code,Needed_Qty from Menu_ItemDetail md where Menu_ItemMasterCode='" + Convert.ToString(ds.Tables[0].Rows[i]["Menu_ItemMasterCode"]) + "'";
                                ds1.Clear();
                                ds1 = d2.select_method_wo_parameter(q1, "Text");
                                if (ds1.Tables[0].Rows.Count > 0)
                                {
                                    for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                                    {
                                        string itempk = d2.getitempk(Convert.ToString(ds1.Tables[0].Rows[k]["Item_Code"]));
                                        if (itempk.Trim() != "0")
                                        {
                                            q1 = "insert into HM_MenuItemDetail (ItemFK ,NeededQty ,MenuItemMasterFK) values ('" + itempk + "','" + Convert.ToString(ds1.Tables[0].Rows[k]["Needed_Qty"]) + "','" + menumasterfk + "')";
                                            insert = d2.update_method_wo_parameter(q1, "text");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                menuitemdetails = true; except = except + "-" + Convert.ToString(ex.Message);
            }
            #endregion

            #region menuschedule
            try
            {
                q1 = "";
                //q1 = "delete HT_MenuSchedule";
                //insert = d2.update_method_wo_parameter(q1, "text");
                //menuschedule datewise
                q1 = "select Schedule_Day,Session_Code,Menu_Code,Hostel_Code,change_strength, hostler, DayScholor,Staffcount,Gustcount,case when schedule_type='0' then '1' when schedule_type='1' then '2' end schedule_type,Schedule_Date from MenuSchedule_DateWise ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        string sessionpk = d2.GetFunction("select hm.SessionMasterPK from HM_SessionMaster hm,Session_Master s where s.Session_Name=hm.SessionName and s.session_code='" + Convert.ToString(ds.Tables[0].Rows[i]["Session_Code"]) + "'");
                        string menucode = Convert.ToString(ds.Tables[0].Rows[i]["Menu_Code"]);
                        string[] split = menucode.Split(',');
                        foreach (string menu in split)
                        {
                            string menumasterFK = "";
                            string scheduletype = Convert.ToString(ds.Tables[0].Rows[i]["schedule_type"]);
                            if (scheduletype.Trim() == "")
                            {
                                scheduletype = "0";
                            }
                            if (Convert.ToInt32(scheduletype) == 1)
                            {
                                menumasterFK = d2.GetFunction("select MenuMasterPK from MenuMaster m,HM_MenuMaster mm where m.MenuID=mm.MenuCode and m.MenuCode='" + menu + "'");
                            }
                            else if (Convert.ToInt32(scheduletype) == 2)
                            {
                                menumasterFK = d2.GetFunction(" select*from IM_ItemMaster where ItemCode='" + menu + "'");
                            }
                            string messmasterfk = d2.GetFunction("select MessMasterPK  from MessMaster m,HM_MessMaster mm where m.MessName=mm.MessName and MessID='" + Convert.ToString(ds.Tables[0].Rows[i]["Hostel_Code"]) + "'");

                            if (messmasterfk.Trim() != "0" && menumasterFK.Trim() != "0" && menumasterFK.Trim() != "" && sessionpk.Trim() != "0")
                            {
                                q1 = "insert into HT_MenuSchedule (MenuScheduleday,SessionMasterFK,MenuMasterFK,MessMasterFK,Change_strength,hostler,DayScholor,staffcount,guestcount,ScheudleItemType,ScheduleType,MenuScheduleDate) values ('" + Convert.ToString(ds.Tables[0].Rows[i][0]) + "','" + sessionpk + "','" + menumasterFK + "','" + messmasterfk + "','" + Convert.ToString(ds.Tables[0].Rows[i][4]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][5]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][6]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][7]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][8]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][9]) + "','1','" + Convert.ToString(ds.Tables[0].Rows[i][10]) + "')";
                                insert = d2.update_method_wo_parameter(q1, "Text");//,'" + Convert.ToString(ds.Tables[0].Rows[i][1]) + "',
                            }

                        }
                    }
                }
                //menuschedule daywise
                q1 = "";
                insert = 0;
                q1 = "select Schedule_Day,Session_Code,Menu_Code,Hostel_Code,change_strength, hostler, DayScholor,Staffcount,Gustcount,case when schedule_type='0' then '1' when schedule_type='1' then '2' end schedule_type from MenuSchedule_DayWise";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string sessionpk = d2.GetFunction("select hm.SessionMasterPK from HM_SessionMaster hm,Session_Master s where s.Session_Name=hm.SessionName and s.session_code='" + Convert.ToString(ds.Tables[0].Rows[i]["Session_Code"]) + "'");
                        string menucode = Convert.ToString(ds.Tables[0].Rows[i]["Menu_Code"]);
                        string[] split = menucode.Split(',');
                        foreach (string menu in split)
                        {
                            string menumasterFK = "";
                            string scheduletype = Convert.ToString(ds.Tables[0].Rows[i]["schedule_type"]);
                            if (scheduletype.Trim() == "")
                            {
                                scheduletype = "0";
                            }
                            if (Convert.ToInt32(scheduletype) == 1)
                            {
                                menumasterFK = d2.GetFunction("select MenuMasterPK from MenuMaster m,HM_MenuMaster mm where m.MenuID=mm.MenuCode and m.MenuCode='" + menu + "'");
                            }
                            else if (Convert.ToInt32(scheduletype) == 2)
                            {
                                menumasterFK = d2.GetFunction(" select*from IM_ItemMaster where ItemCode='" + menu + "'");
                            }
                            string messmasterfk = d2.GetFunction("select MessMasterPK  from MessMaster m,HM_MessMaster mm where m.MessName=mm.MessName and MessID='" + Convert.ToString(ds.Tables[0].Rows[i]["Hostel_Code"]) + "'");

                            if (messmasterfk.Trim() != "0" && menumasterFK.Trim() != "0" && menumasterFK.Trim() != "" && sessionpk.Trim() != "0")
                            {
                                q1 = "insert into HT_MenuSchedule (MenuScheduleday,SessionMasterFK,MenuMasterFK,MessMasterFK,Change_strength,hostler,DayScholor,staffcount,guestcount,ScheudleItemType,ScheduleType) values ('" + Convert.ToString(ds.Tables[0].Rows[i][0]) + "','" + sessionpk + "','" + menumasterFK + "','" + messmasterfk + "','" + Convert.ToString(ds.Tables[0].Rows[i][4]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][5]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][6]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][7]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][8]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][9]) + "','2')";
                                insert = d2.update_method_wo_parameter(q1, "text");
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                menuschedule = true; except = except + "-" + Convert.ToString(ex.Message);
            }
            #endregion

            #region cleanning item
            try
            {
                insert = 0;

                q3 = "";
                try
                {
                    q3 = "Create Table CleaningitemMaseter_temp (Access_Date datetime,Access_Time nvarchar(100),Clean_ItemMasterCode numeric,Session_Code int,NoOfItems int,Hostel_Code numeric, Schedule_Day varchar(50),Schedule_Date datetime,Schedule_type tinyint)  Create Table CleaningItemDetailMaster_temp  (Access_Date datetime,Access_Time nvarchar(100),Clean_ItemDetailMasterCode numeric  NOT NULL,Clean_ItemMasterCode numeric,Item_Code nvarchar(100),Needed_Qty decimal,Hostel_Code numeric)";
                    insert = d2.update_method_wo_parameter(q3, "text");
                }
                catch { }
                insert = 0;
                q1 = "";

                try
                {
                    q1 = "insert into CleaningitemMaseter_temp(Access_Date,Access_Time,Clean_ItemMasterCode,Session_Code, NoOfItems,Hostel_Code,Schedule_Day,Schedule_Date,Schedule_type)(select Access_Date,Access_Time,Clean_ItemMasterCode,Session_Code,NoOfItems,Hostel_Code,Schedule_Day,Schedule_Date,Schedule_type from Cleaning_ItemMaseter)";

                    q1 = q1 + " insert into CleaningItemDetailMaster_temp(Access_Date,Access_Time,Clean_ItemDetailMasterCode,Clean_ItemMasterCode,Item_Code,Needed_Qty,Hostel_Code)select Access_Date,Access_Time,Clean_ItemDetailMasterCode,Clean_ItemMasterCode,Item_Code,Needed_Qty,Hostel_Code from Cleaning_ItemDetailMaster";
                    insert = d2.update_method_wo_parameter(q1, "Text");
                }
                catch { }
                if (insert != 0)
                {
                    q2 = "";
                    q2 = "drop table Cleaning_ItemDetailMaster";
                    q2 = q2 + " drop table Cleaning_ItemMaseter";
                    insert = d2.update_method_wo_parameter(q2, "Text");

                    q3 = " Create Table Cleaning_ItemMaseter (Clean_ItemMasterPK numeric identity(1,1),SessionFK bigint,NoOfItems int,MessMasterFK bigint,Schedule_Day varchar(50),Schedule_Date datetime,Schedule_type tinyint) Create Table Cleaning_ItemDetailMaster (Clean_ItemDetailMasterCode numeric IDENTITY(1,1) NOT NULL,Clean_ItemMasterFK bigint,Itemfk bigint,Needed_Qty decimal)";
                    insert = d2.update_method_wo_parameter(q3, "Text");

                    q1 = "";
                    q1 = "select Clean_ItemMasterCode,Session_Code,NoOfItems,Hostel_Code,Schedule_Day,Schedule_Date,Schedule_type from CleaningitemMaseter_temp";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q1, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string sessionpk = d2.GetFunction("select hm.SessionMasterPK from HM_SessionMaster hm,Session_Master s where s.Session_Name=hm.SessionName and s.session_code='" + Convert.ToString(ds.Tables[0].Rows[i]["Session_Code"]) + "'");

                            string messmasterfk = d2.GetFunction("select MessMasterPK  from MessMaster m,HM_MessMaster mm where m.MessName=mm.MessName and MessID='" + Convert.ToString(ds.Tables[0].Rows[i]["Hostel_Code"]) + "'");

                            q3 = "";
                            q3 = "insert into Cleaning_ItemMaseter(SessionFK,MessMasterFK,NoOfItems,Schedule_Day,Schedule_Date,Schedule_type)values('" + sessionpk + "','" + messmasterfk + "','" + Convert.ToString(ds.Tables[0].Rows[i]["NoOfItems"]) + "','" + Convert.ToString(ds.Tables[0].Rows[i]["Schedule_Day"]) + "','" + Convert.ToString(ds.Tables[0].Rows[i]["Schedule_Date"]) + "','" + Convert.ToString(ds.Tables[0].Rows[i]["Schedule_type"]) + "')";
                            insert = d2.update_method_wo_parameter(q3, "text");
                            string cleanfk = d2.GetFunction("select Clean_ItemMasterPK from Cleaning_ItemMaseter where SessionFK='" + sessionpk + "' and MessMasterFK='" + messmasterfk + "'");

                            q1 = "";
                            q1 = "select Item_Code,Needed_Qty,Hostel_Code from CleaningItemDetailMaster_temp where  Clean_ItemMasterCode='" + Convert.ToString(ds.Tables[0].Rows[i]["Clean_ItemMasterCode"]) + "'";
                            ds1.Clear();
                            ds1 = d2.select_method_wo_parameter(q1, "text");
                            for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                            {
                                q3 = "";
                                q3 = d2.getitempk(Convert.ToString(ds1.Tables[0].Rows[k]["Item_Code"]));
                                q2 = "";
                                q2 = "insert into Cleaning_ItemDetailMaster(Clean_ItemMasterFK,Itemfk,Needed_Qty)values('" + cleanfk + "','" + q3 + "','" + Convert.ToString(ds1.Tables[0].Rows[k]["Needed_Qty"]) + "')";
                                insert = d2.update_method_wo_parameter(q2, "Text");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                cleanitem = true; except = except + "-" + Convert.ToString(ex.Message);
            }
            #endregion

            #region openning Stock
            try
            {
                //q1 = "delete IT_StockDetail";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "";
                q1 = "select item_code,ISNULL(hand_qty,0)hand_qty,ISNULL(Return_Qty,0)Return_Qty,ISNULL(trans_qty,0)trans_qty,ISNULL(rpu,0)rpu,ISNULL(Store_Code,0)Store_Code from stock_master";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string Itempk = d2.GetFunction("select itemPk from IM_ItemMaster where ItemCode='" + Convert.ToString(ds.Tables[0].Rows[i]["item_code"]) + "'");
                        q3 = "";
                        q3 = d2.GetFunction("select StorePK from StoreMaster s,IM_StoreMaster sm where s.Store_Name=sm.StoreName and Store_Code='" + Convert.ToString(ds.Tables[0].Rows[i][5]) + "'");

                        q1 = "insert into IT_StockDetail (ItemFK,InwardQty,ReturnQty,TransferQty,InwardRPU,StoreFK) values('" + Itempk + "','" + Convert.ToString(ds.Tables[0].Rows[i][1]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][2]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][3]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][4]) + "','" + q3 + "')";
                        insert = d2.update_method_wo_parameter(q1, "Text");
                    }
                }
                q1 = "";
                //q1 = "delete IT_StockDeptDetail";
                //insert = d2.update_method_wo_parameter(q1, "Text");

                q1 = " select Item_Code,ISNULL(Dept_Code,0)Dept_Code,ISNULL(AvlQty,0)AvlQty,ISNULL(UsedQty,0)UsedQty,ISNULL(BalQty,0)BalQty,ISNULL(RPU,0)RPU from Stock_Detail";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string Itempk = d2.GetFunction("select itemPk from IM_ItemMaster where ItemCode='" + Convert.ToString(ds.Tables[0].Rows[i]["item_code"]) + "'");
                        string messmasterfk = d2.GetFunction("select MessMasterPK  from MessMaster m,HM_MessMaster mm where m.MessName=mm.MessName and MessID='" + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Code"]) + "'");

                        q1 = "insert into IT_StockDeptDetail(ItemFK,DeptFK,BalQty,UsedQty,IssuedQty,IssuedRPU)values('" + Itempk + "','" + messmasterfk + "','" + Convert.ToString(ds.Tables[0].Rows[i][2]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][3]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][4]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][5]) + "')";
                        insert = d2.update_method_wo_parameter(q1, "Text");
                    }
                }
            }
            catch (Exception ex) { openningstock = true; except = except + "-" + Convert.ToString(ex.Message); }
            #endregion

            #region Purchase Order Insert
            try
            {
                //q1 = "delete IT_PurchaseOrderDetail";
                //q1 = q1 + " delete it_purchaseorder";
                //insert = d2.update_method_wo_parameter(q1, "Text");

                q1 = "";
                q1 = "select order_code,order_date,0,0,due_date,DisType,Disc,tax,Other_Charges,Other_Description,convert(float , (ISNULL(page_no,0)))as pageno,(select vendorpk from CO_VendorMaster c where c.VendorCode=p.vendor_code)as VendorFK,Approval_Status from Purchase_Order p";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q2 = "";
                        q2 = "insert into IT_PurchaseOrder (OrderCode,OrderDate,OrderType,OrderMode,OrderDueDate,IsTotDisPercent,TotDisAmt,TotTaxAmt,TotOtherChgAmt,OrderDescription,PageNo,VendorFK,ApproveStatus) values ('" + Convert.ToString(ds.Tables[0].Rows[i][0]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][1]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][2]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][3]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][4]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][5]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][6]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][7]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][8]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][9]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][10]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][11]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][12]) + "')";
                        insert = d2.update_method_wo_parameter(q2, "Text");

                        if (insert != 0)
                        {
                            string purchaseorderpk = d2.GetFunction("select purchaseorderpk from IT_PurchaseOrder where OrderCode='" + Convert.ToString(ds.Tables[0].Rows[i][0]) + "' ");//and VendorFK='" + Convert.ToString(ds.Tables[0].Rows[i][11]) + "'
                            q3 = "";
                            q3 = "select item_code,app_qty,rpu,convert(bit, DisType),discount,tax,execise_tax,Edu_Cess,HEdu_Cess,other_char,Other_CharDesc,app_qty ,order_code ,ISNULL(goods_in,'0')as inwardstatus from purchaseorder_items po where order_code='" + Convert.ToString(ds.Tables[0].Rows[i][0]) + "'";
                            ds1.Clear();
                            ds1 = d2.select_method_wo_parameter(q3, "Text");
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                                {
                                    string itempk = d2.GetFunction("select ItemPK from IM_ItemMaster where itemcode='" + Convert.ToString(ds1.Tables[0].Rows[k][0]) + "'");

                                    string q4 = "insert into IT_PurchaseOrderDetail (ItemFK,Qty,RPU,IsDiscountPercent,DiscountAmt,TaxPercent,ExeciseTaxPer,EduCessPer,HigherEduCessPer,OtherChargeAmt,OtherChargeDesc,AppQty,PurchaseOrderFK,Inward_Status) values ('" + itempk + "','" + Convert.ToString(ds1.Tables[0].Rows[k][1]) + "','" + Convert.ToString(ds1.Tables[0].Rows[k][2]) + "','" + Convert.ToString(ds1.Tables[0].Rows[k][3]) + "','" + Convert.ToString(ds1.Tables[0].Rows[k][4]) + "','" + Convert.ToString(ds1.Tables[0].Rows[k][5]) + "','" + Convert.ToString(ds1.Tables[0].Rows[k][6]) + "','" + Convert.ToString(ds1.Tables[0].Rows[k][7]) + "','" + Convert.ToString(ds1.Tables[0].Rows[k][8]) + "','" + Convert.ToString(ds1.Tables[0].Rows[k][9]) + "','" + Convert.ToString(ds1.Tables[0].Rows[k][10]) + "','" + Convert.ToString(ds1.Tables[0].Rows[k][11]) + "','" + purchaseorderpk + "','" + Convert.ToString(ds1.Tables[0].Rows[k][13]) + "') ";

                                    insert = d2.update_method_wo_parameter(q4, "Text");

                                }

                            }
                        }
                        q3 = "";
                        q3 = d2.GetFunction("select  case when order_approval='Approved' then '1' when order_approval='Reject' then '2' end order_approval from purchase_order p,purchaseorder_items pi where p.order_code=pi.order_code and p.order_code='" + Convert.ToString(ds.Tables[0].Rows[i][0]) + "'");
                        if (q3.Trim() != "0")
                        {
                            if (q3 == "1")
                            {
                                q1 = "";
                                q1 = "update IT_PurchaseOrder set ApproveStatus='1' where OrderCode='" + Convert.ToString(ds.Tables[0].Rows[i][0]) + "'";
                                insert = d2.update_method_wo_parameter(q1, "text");
                            }
                        }


                    }
                }
            }
            catch (Exception ex) { purchaseorder = true; except = except + "-" + Convert.ToString(ex.Message); }
            #endregion

            #region Daily consumption
            try
            {
                insert = 0;
                //q1 = " delete HT_DailyConsumptionDetail";
                //q1 = q1 + " delete HT_DailyConsumptionMaster";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "";

                q1 = "select Consumption_Date,case when typeofconsume= '0' then '1' when typeofconsume='1' then '2' end typeofconsume,d.Hostel_Code,ss.SessionMasterPK ,d.Hostel_Code,Total_Present,DailyConsumptionMaster_Code from DailyConsumption_Master d,MenuMaster m,Session_Master s,HM_SessionMaster ss where d.SessionMenu_Code=m.MenuCode  and d.Session_Code=s.Session_Code and s.Session_Name=ss.SessionName";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        q1 = "insert into HT_DailyConsumptionMaster(DailyConsDate,ForMess,MessMasterFK,SessionFK,DeptFK,Total_Present)values('" + Convert.ToString(ds.Tables[0].Rows[i][0]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][1]) + "','" + Convert.ToStrin(ds.Tables[0].Rows[i][2]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][3]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][4]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][5]) + "')";
                        insert = d2.update_method_wo_parameter(q1, "Text");
                        if (insert != 0)
                        {
                            string DailyConsumptionMasterPK = d2.GetFunction("select DailyConsumptionMasterPK from  HT_DailyConsumptionMaster where MessMasterFK ='" + Convert.ToString(ds.Tables[0].Rows[i][2]) + "' and DailyConsDate ='" + Convert.ToString(ds.Tables[0].Rows[i][0]) + "' and ForMess='1'");

                            q1 = "";
                            q1 = "  select Item_Code,Consumption_Qty,RPU,isnull(isadjust,'0')isadjust,isnull(isreturn,'0')isreturn from  DailyConsumption_Detail  where DailyConsumptionMaster_Code='" + Convert.ToString(ds.Tables[0].Rows[i][6]) + "'";
                            ds1.Clear();
                            ds1 = d2.select_method_wo_parameter(q1, "Text");
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                                {
                                    string itempk = d2.GetFunction("select ItemPK from IM_ItemMaster where itemcode='" + Convert.ToString(ds1.Tables[0].Rows[k][0]) + "'");

                                    q1 = "";
                                    q1 = "insert into HT_DailyConsumptionDetail(ItemFK,ConsumptionQty,DailyConsumptionMasterFK, isadjust,adjust_qty,rpu) values('" + itempk + "','" + Convert.ToString(ds1.Tables[0].Rows[k][1]) + "','" + DailyConsumptionMasterPK + "','" + Convert.ToInt32(ds1.Tables[0].Rows[k][3]) + "','" + Convert.ToInt32(ds1.Tables[0].Rows[k][4]) + "','" + Convert.ToInt32(ds1.Tables[0].Rows[k][2]) + "')";
                                    insert = d2.update_method_wo_parameter(q1, "Text");
                                }
                            }
                            // insert into HT_DailyConsumptionDetail(ItemFK,ConsumptionQty,DailyConsumptionMasterFK,isadjust,adjust_qty)values(
                        }
                    }
                }
            }
            catch (Exception ex) { Dailyconsumption = true; except = except + "-" + Convert.ToString(ex.Message); }

            #endregion

            #region Good Inward
            try
            {
                //q1 = "";
                //q1 = "delete IT_GoodsInward";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "";
                q1 = "select * from goods_inward ";
                q1 = q1 + " select*from goodsinward_items";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataView dv = new DataView();
                        ds.Tables[1].DefaultView.RowFilter = "gi_code='" + Convert.ToString(ds.Tables[0].Rows[i]["Gi_code"]) + "'";
                        dv = ds.Tables[1].DefaultView;
                        if (dv.Count > 0)
                        {
                            for (int k = 0; k < dv.Count; k++)
                            {
                                string purchasefk = d2.GetFunction("select PurchaseOrderPK  from IT_PurchaseOrder where OrderCode='" + Convert.ToString(ds.Tables[0].Rows[i]["Order_Code"]) + "'");
                                string itempk = d2.GetFunction("select ItemPK from IM_ItemMaster where itemcode='" + Convert.ToString(ds.Tables[1].Rows[k]["item_code"]) + "'");
                                string venfk = d2.GetFunction("select vendorpk from CO_VendorMaster  where vendorcode='" + Convert.ToString(ds.Tables[0].Rows[i]["vendor_code"]) + "'");

                                string staffcode ="" ;                               
                                staffcode = d2.GetFunction("select appl_id  from staffmaster s,staff_appl_master sm where s.appl_no=sm.appl_no and s.staff_code ='"+Convert.ToString(ds.Tables[0].Rows[i]["Inward_staff_code"])+"'");
                                if (staffcode.Trim() == "")
                                {
                                    staffcode = "0";
                                }                                
                                q1 = "";
                                q1 = "insert into IT_GoodsInward (GoodsInwardCode,GoodsInwardDate,OrderQty,InwardQty,PurchaseOrderFK,itemfk,Received_staffcode,VendorFK)values('" + Convert.ToString(dv[k]["gi_code"]) + "','" + Convert.ToString(ds.Tables[0].Rows[i]["gi_date"]) + "','" + Convert.ToString(dv[k]["inward_qty"]) + "','" + Convert.ToString(dv[k]["inward_qty"]) + "','" + purchasefk + "','" + itempk + "','" + staffcode + "','" + venfk + "')";
                                insert = d2.update_method_wo_parameter(q1, "Text");
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { GoodInward = true; except = except + "-" + Convert.ToString(ex.Message); }
            #endregion

            #region Income
            try
            {
                insert = 0;
                //q1 = "";
                //q1 = "delete HT_HostelIncome";
                //insert = d2.update_method_wo_parameter(q1, "Text");

                q1 = "select entry_date,incgroup_code,Inc_Amount,Inc_Description,Hostel_Code from Hostel_Income";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string group = d2.GetFunction("select TextVal from TextValTable where TextCode='" + Convert.ToString(ds.Tables[0].Rows[i]["incgroup_code"]) + "' and  TextCriteria ='HIGrp'");

                        string clgcode = d2.GetFunction("select college_code from hostel_details where Hostel_code='" + Convert.ToString(ds.Tables[0].Rows[i]["Hostel_Code"]) + "'");

                        q1 = "if exists(select*from CO_MasterValues where MasterValue='" + group + "' and MasterCriteria='HostelIncomeGRP') update CO_MasterValues set MasterValue='" + group + "' where MasterValue='" + group + "' and MasterCriteria='HostelIncomeGRP' else insert into CO_MasterValues (MasterValue,CollegeCode,MasterCriteria)values('" + group + "','" + clgcode + "','HostelIncomeGRP')";
                        insert = d2.update_method_wo_parameter(q1, "Text");
                        string groupcode = d2.GetFunction("select mastercode from co_mastervalues where MasterValue='" + group + "' and MasterCriteria='HostelIncomeGRP'");

                        string hostelpk = d2.GetFunction("select HostelMasterPK from  HM_HostelMaster hm,Hostel_Details h,Hostel_Income hi where h.Hostel_Name=hm.HostelName and hi.Hostel_Code=h.Hostel_code and h.hostel_code='" + Convert.ToString(ds.Tables[0].Rows[i]["Hostel_Code"]) + "'");

                        q1 = "INSERT INTO HT_HostelIncome(IncomeDate,IncomeGroup,IncomeAmount,IncomeDesc,HostelMasterFK)values('" + Convert.ToString(ds.Tables[0].Rows[i][0]) + "','" + groupcode + "','" + Convert.ToString(ds.Tables[0].Rows[i][2]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][3]) + "','" + hostelpk + "')";
                        insert = d2.update_method_wo_parameter(q1, "Text");
                    }
                }
            }
            catch (Exception ex) { Income = true; except = except + "-" + Convert.ToString(ex.Message); }
            #endregion

            #region Expances
            try
            {
                //q1 = "";
                //q1 = "delete HT_HostelExpenses";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "";
                q1 = "select * from  Hostel_Expenses";
                q1 = q1 + " select * from Hostel_ExpensesDetail";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataView dv = new DataView();
                        ds.Tables[1].DefaultView.RowFilter = "Exp_Code='" + Convert.ToString(ds.Tables[0].Rows[i]["Expenses_Code"]) + "'";
                        dv = ds.Tables[1].DefaultView;
                        if (dv.Count > 0)
                        {
                            for (int k = 0; k < dv.Count; k++)
                            {
                                q1 = "";
                                string maingroup = d2.GetFunction("select TextVal from TextValTable where TextCriteria ='HEGrp' and textcode='" + Convert.ToString(dv[k]["MainGroup_Desc"]) + "'");

                                string subgroup = d2.GetFunction("select TextVal from TextValTable where TextCriteria ='HESGr' and textcode='" + Convert.ToString(dv[k]["Sub_Group"]) + "'");


                                string hostelpk = d2.GetFunction("select HostelMasterPK from  HM_HostelMaster hm,Hostel_Details h,Hostel_Expenses hi where h.Hostel_Name=hm.HostelName and hi.Hostel_Code=h.Hostel_code and h.hostel_code='" + Convert.ToString(ds.Tables[0].Rows[i]["Hostel_code"]) + "'");

                                string clgcode = d2.GetFunction("select college_code from hostel_details where Hostel_code='" + Convert.ToString(ds.Tables[0].Rows[i]["Hostel_Code"]) + "'");

                                q1 = " if exists ( select * from CO_MasterValues where MasterValue ='" + maingroup + "' and MasterCriteria ='HostelExpGrp' and collegecode ='" + clgcode + "') update CO_MasterValues set MasterValue ='" + maingroup + "' where MasterValue ='" + maingroup + "' and MasterCriteria ='HostelExpGrp' and collegecode ='" + clgcode + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,collegecode) values ('" + maingroup + "','HostelExpGrp','" + clgcode + "')";
                                insert = d2.update_method_wo_parameter(q1, "Text");
                                q1 = "";

                                q1 = "if exists ( select * from CO_MasterValues where MasterValue ='" + subgroup + "' and MasterCriteria ='HostelExpSubGrp' and collegecode ='" + clgcode + "') update CO_MasterValues set MasterValue ='" + subgroup + "' where MasterValue ='" + subgroup + "' and MasterCriteria ='HostelExpSubGrp' and collegecode ='" + clgcode + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,collegecode) values ('" + subgroup + "','HostelExpSubGrp','" + clgcode + "')";
                                insert = d2.update_method_wo_parameter(q1, "Text");
                                q1 = "";

                                string getgroup = d2.GetFunction("select Mastercode from CO_MasterValues where MasterValue ='" + maingroup + "' and MasterCriteria ='HostelExpGrp' and collegecode ='" + clgcode + "'");
                                string getsubgroup = d2.GetFunction("select Mastercode from CO_MasterValues where MasterValue ='" + subgroup + "' and MasterCriteria ='HostelExpSubGrp' and collegecode ='" + clgcode + "'");


                                q1 = "INSERT INTO HT_HostelExpenses(ExpensesType,ExpensesDate,ExpGroup,ExpSubGroup,ExpDesc,ExpAmount,HostelFK)values('" + Convert.ToString(ds.Tables[0].Rows[i]["Expanse_type"]) + "','" + Convert.ToString(ds.Tables[0].Rows[i]["Entry_Date"]) + "','" + getgroup + "','" + getsubgroup + "','" + Convert.ToString(dv[k]["Description"]) + "','" + Convert.ToString(dv[k]["Exp_amount"]) + "','" + hostelpk + "')";
                                insert = d2.update_method_wo_parameter(q1, "Text");
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Expances = true; except = except + "-" + Convert.ToString(ex.Message); }
            #endregion

            #region Rebate
            try
            {
                insert = 0;
                //q1 = "";
                //q1 = "delete HM_RebateMaster";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "";
                q1 = "select Rebate_Type,Actual_Day,Grant_Day,Rebate_Month,isnull(Grant_Amount,0), Hostel_Code from Rebate_Master";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string hostelpk = d2.GetFunction("select HostelMasterPK from  HM_HostelMaster hm,Hostel_Details h,Rebate_Master hi where h.Hostel_Name=hm.HostelName and hi.Hostel_Code=h.Hostel_code and h.hostel_code='" + Convert.ToString(ds.Tables[0].Rows[i]["Hostel_Code"]) + "'");

                        q1 = "insert into HM_RebateMaster(RebateType,RebateActDays,RebateDays,RebateMonth,RebateAmount,HostelFK)values('" + Convert.ToString(ds.Tables[0].Rows[i][0]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][1]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][2]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][3]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][4]) + "','" + hostelpk + "')";
                        insert = d2.update_method_wo_parameter(q1, "Text");

                    }
                }
            }
            catch (Exception ex) { Rebate = true; except = except + "-" + Convert.ToString(ex.Message); }
            #endregion

            #region Student Rebate Details
            try
            {
                insert = 0;
                q1 = "";
                //q1 = "delete HT_HostelRebateDetail";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "";
                q1 = "select Roll_No,Rebate_Type,From_Date,To_Date,Leave_Days,Rebate_Days, Rebate_Date,isnull(Rebate_Amount,0)as Rebate_Amount, Desc_Code,College_Code from StudentRebate_Details";//convert(tinyint, Rebate_Type)
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string app_no = d2.GetFunction("select app_no from Registration where Roll_No='" + Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]) + "'");

                        string desc = d2.GetFunction("select TextVal from TextValTable where TextCriteria ='RDesc' and textcode='" + Convert.ToString(ds.Tables[0].Rows[i]["Desc_Code"]) + "'");

                        string description = "if exists ( select * from CO_MasterValues where MasterValue ='" + desc + "' and MasterCriteria ='RebatestudentDesc' and collegecode ='" + Convert.ToString(ds.Tables[0].Rows[i]["College_Code"]) + "') update CO_MasterValues set MasterValue ='" + desc + "' where MasterValue ='" + desc + "' and MasterCriteria ='RebatestudentDesc' and collegecode ='" + Convert.ToString(ds.Tables[0].Rows[i]["College_Code"]) + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,collegecode) values ('" + desc + "','RebatestudentDesc','" + Convert.ToString(ds.Tables[0].Rows[i]["College_Code"]) + "')";
                        insert = d2.update_method_wo_parameter(description, "Text");

                        string desc_code = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria ='RebatestudentDesc' and  MasterValue='" + desc + "'");
                        q1 = "";
                        q1 = "insert into HT_HostelRebateDetail(MemType,App_No,RebateType,RebateFromDate,RebateToDate,LeaveDays, RebateDays,RebateDate,RebateAmount,RebateDesc)values('1','" + app_no + "','" + Convert.ToInt32(ds.Tables[0].Rows[i][1]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][2]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][3]) + "','" + Convert.ToInt32(ds.Tables[0].Rows[i][4]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][5]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][6]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][7]) + "','" + desc_code + "' )";
                        insert = d2.update_method_wo_parameter(q1, "Text");
                    }
                }
            }
            catch (Exception ex) { StudentRebateDetails = true; except = except + "-" + Convert.ToString(ex.Message); }
            #endregion

            #region Student additional details
            try
            {
                insert = 0;
                q1 = "";
                //q1 = "delete HT_StudAdditionalDet";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "";
                q1 = "select '1' as MemType,(select app_no from Registration r where r.Roll_No=s.Roll_No)App_no ,Entry_Date,Add_Amount,Description,College_Code from StudentAdditional_Details s";//convert(tinyint, Rebate_Type)
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string desc = Convert.ToString(ds.Tables[0].Rows[i]["Description"]);
                        string description = "if exists ( select * from CO_MasterValues where MasterValue ='" + desc + "' and MasterCriteria ='Expense' and collegecode ='" + Convert.ToString(ds.Tables[0].Rows[i]["College_Code"]) + "') update CO_MasterValues set MasterValue ='" + desc + "' where MasterValue ='" + desc + "' and MasterCriteria ='Expense' and collegecode ='" + Convert.ToString(ds.Tables[0].Rows[i]["College_Code"]) + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,collegecode) values ('" + desc + "','Expense','" + Convert.ToString(ds.Tables[0].Rows[i]["College_Code"]) + "')";
                        insert = d2.update_method_wo_parameter(description, "Text");
                        string desc_code = d2.GetFunction("select mastercode from co_mastervalues where mastervalue='" + desc + "' and mastercriteria='Expense'");
                        q1 = "insert into HT_StudAdditionalDet(MemType,App_No,TransDate,AdditionalAmt,AdditionalDesc)values('" + Convert.ToInt32(ds.Tables[0].Rows[i][0]) + "','" + Convert.ToInt32(ds.Tables[0].Rows[i][1]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][2]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][3]) + "','" + desc_code + "')";
                        insert = d2.update_method_wo_parameter(q1, "Text");
                    }
                }
            }
            catch (Exception ex) { Studentadditionaldetails = true; except = except + "-" + Convert.ToString(ex.Message); }
            #endregion

            #region Menu cost Master
            try
            {
                q1 = "";
                //q1 = "delete HM_MenuCostMaster";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "";
                q1 = "select SessionMenu_Code,Qty,Cost,From_Date from MenuCost_Master";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string menucode = d2.GetFunction("select MenuMasterPK  from MenuMaster m,HM_MenuMaster mm,MenuCost_Master c where m.MenuID =mm.MenuCode and c.sessionmenu_code=m.MenuCode and c.sessionmenu_code='" + Convert.ToString(ds.Tables[0].Rows[i]["sessionmenu_code"]) + "'");

                        q1 = "insert into HM_MenuCostMaster (MenuMasterFK,MenuQty,MenuAmount,Menucost_Date) values ('" + menucode + "','" + Convert.ToString(ds.Tables[0].Rows[i][1]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][2]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][3]) + "')";
                        insert = d2.update_method_wo_parameter(q1, "Text");
                    }
                }
            }
            catch (Exception ex) { MenucostMaster = true; except = except + "-" + Convert.ToString(ex.Message); }
            #endregion

            #region Student token details
            try
            {
                //q1 = "";
                //q1 = "delete HT_StudTokenDetails";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                q1 = "";
                q1 = "select TokenDate,'1'as MemType,(select App_No from Registration r where r.Roll_No=s.Roll_No )AS App_No,Session_Code,MenuCode,Hostel_Code,Qty  from StudentToken_Details s";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string SessionPK = d2.GetFunction("select SessionMasterPK from StudentToken_Details s,Session_Master sm,HM_SessionMaster hm where  s.Session_Code=sm.Session_Code and sm.Session_Name=hm.SessionName and s.Session_Code='" + Convert.ToString(ds.Tables[0].Rows[i][3]) + "'");

                        string MenuMasterPK = d2.GetFunction("select MenuMasterPK  from MenuMaster m,HM_MenuMaster mm,StudentToken_Details s,Session_Master sm,HM_SessionMaster hm where m.MenuID=mm.MenuCode and s.Session_Code=sm.Session_Code and sm.Session_Name=hm.SessionName and s.Session_Code='" + Convert.ToString(ds.Tables[0].Rows[i]["Session_Code"]) + "' and m.MenuCode=s.MenuCode and m.MenuCode='" + Convert.ToString(ds.Tables[0].Rows[i]["MenuCode"]) + "'");

                        string Hostelpk = d2.GetFunction(" select HostelMasterPK from  HM_HostelMaster hm,Hostel_Details h,StudentToken_Details s where h.Hostel_Name=hm.HostelName and s.Hostel_Code=h.Hostel_code and h.hostel_code='" + Convert.ToString(ds.Tables[0].Rows[i]["Hostel_Code"]) + "'");

                        q1 = "";
                        q1 = "INSERT INTO HT_StudTokenDetails(TokenDate,MemType,App_No,SessionFK,MenuFK,MessFK,TokenQty)values('" + Convert.ToString(ds.Tables[0].Rows[i][0]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][1]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][2]) + "','" + SessionPK + "','" + MenuMasterPK + "','" + Hostelpk + "','" + Convert.ToString(ds.Tables[0].Rows[i][6]) + "')";
                        insert = d2.update_method_wo_parameter(q1, "Text");
                    }
                }
            }
            catch (Exception ex) { Studenttokendetails = true; except = except + "-" + Convert.ToString(ex.Message); }
            #endregion

            #region Mess bill
            try
            {
                q1 = "";
                //q1 = "delete HT_MessBillDetail";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                //q1 = "delete HT_MessBillMaster";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                //q1 = "delete HMessbill_StudDetails";
                //insert = d2.update_method_wo_parameter(q1, "Text");
                ViewState["guestgrp"] = null;
                string messbillmastercode = "";
                ArrayList messbillmaster = new ArrayList();
                q1 = "select BillMonth,Bill_Year,Hostel_Code,groupcode,Messbillmasterid from MessBill_Master";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string groupcode = Convert.ToString(ds.Tables[0].Rows[i]["groupcode"]);
                        string[] split = groupcode.Split(',');
                        foreach (string group in split)
                        {
                            string messmasterfk = "";
                            ////if (group.Trim() != "")
                            ////{
                            string grouptext = d2.GetFunction("select textval from textvaltable where textcode='" + group + "' and TextCriteria='HEGrp'");
                            string groupid = d2.GetFunction("select mastercode from CO_MasterValues where MasterValue='" + grouptext + "' and MasterCriteria='HostelExpGrp'");
                            string guestgrp = "";
                            if (guestgrp == "")
                            {
                                guestgrp = "" + groupid + "";
                            }
                            else
                            {
                                guestgrp = guestgrp + "," + "" + groupid + "";
                            }
                            ViewState["guestgrp"] = guestgrp;

                            messmasterfk = d2.GetFunction("select MessMasterPK   from MessDetail m,MessMaster d,HM_MessMaster hm where m.MessID =d.MessID and hm.MessName =d.MessName and  m.Hostel_Code in ('" + Convert.ToString(ds.Tables[0].Rows[i]["Hostel_Code"]) + "')");
                            if (messmasterfk.Trim() != "" && messmasterfk.Trim() != "0")
                            {
                                q1 = "Insert into HT_MessBillMaster (MessMonth,MessYear,MessMasterFK,GroupCode) values('" + Convert.ToString(ds.Tables[0].Rows[i][0]) + "','" + Convert.ToString(ds.Tables[0].Rows[i][1]) + "','" + messmasterfk + "','" + guestgrp + "')";
                                insert = d2.update_method_wo_parameter(q1, "Text");
                            }
                            if (insert != 0)
                            {
                                string messbillmasterfk = d2.GetFunction("select MessBillMasterPK from HT_MessBillMaster where MessMonth='" + Convert.ToString(ds.Tables[0].Rows[i][0]) + "' and MessYear='" + Convert.ToString(ds.Tables[0].Rows[i][1]) + "' and MessMasterFK='" + messmasterfk + "'");
                                if (messbillmasterfk.Trim() != "" && messmasterfk.Trim() != "0")
                                {
                                    q1 = " select Roll_No,ISNULL(Fixed_Amount,0)Fixed_Amount,ISNULL(Additional_Amount,0)Additional_Amount,ISNULL(Rebate_Amount,0)Rebate_Amount,Hostel_Code,ISNULL(GroupAmount,0)GroupAmount,Rebete_days,College_Code  from MessBill_Detail  where MessBill_MasterCode='" + Convert.ToString(ds.Tables[0].Rows[i]["Messbillmasterid"]) + "'";
                                    ds1.Clear();
                                    ds1 = d2.select_method_wo_parameter(q1, "Text");
                                    if (ds1.Tables[0].Rows.Count > 0)
                                    {
                                        for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                                        {
                                            string app_no = "";
                                            app_no = d2.GetFunction("select app_no from Registration where Roll_No='" + Convert.ToString(ds1.Tables[0].Rows[k]["Roll_No"]) + "'");
                                            string memtype = "";
                                            if (app_no.Trim() != "0")
                                            {
                                                memtype = "1";
                                            }
                                            else
                                            {
                                                app_no = d2.GetFunction("select staff_code from staffmaster where staff_code='" + Convert.ToString(ds1.Tables[0].Rows[k]["Roll_No"]) + "'");
                                                if (app_no.Trim() != "0")
                                                {
                                                    memtype = "2";
                                                }
                                            }
                                            q1 = "insert into  HT_MessBillDetail (MemType,App_No,MessAmount, MessAdditonalAmt, RebateAmount,MessBillMasterFK,GroupAmount) values('" + memtype + "','" + app_no + "','" + Convert.ToString(ds1.Tables[0].Rows[k]["Fixed_Amount"]) + "','" + Convert.ToString(ds1.Tables[0].Rows[k]["Additional_Amount"]) + "','" + Convert.ToString(ds1.Tables[0].Rows[k]["Rebate_Amount"]) + "','" + messbillmasterfk + "','" + Convert.ToString(ds1.Tables[0].Rows[k]["GroupAmount"]) + "')";
                                            insert = d2.update_method_wo_parameter(q1, "Text");
                                            if (!messbillmaster.Contains(Convert.ToString(ds.Tables[0].Rows[i]["Messbillmasterid"])))
                                            {
                                                if (messbillmastercode == "")
                                                {
                                                    messbillmastercode = "" + Convert.ToString(ds.Tables[0].Rows[i]["Messbillmasterid"]) + "";
                                                }
                                                else
                                                {
                                                    messbillmastercode = messbillmastercode + "," + "" + Convert.ToString(ds.Tables[0].Rows[i]["Messbillmasterid"]) + "";
                                                }
                                                messbillmaster.Add(Convert.ToString(ds.Tables[0].Rows[i]["Messbillmasterid"]));
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
                if (messbillmastercode.Trim() != "")
                {
                    string[] split1 = messbillmastercode.Split(',');
                    foreach (string group in split1)
                    {
                        q1 = "";
                        q1 = " select Roll_No,ISNULL(Fixed_Amount,0)Fixed_Amount,ISNULL(Additional_Amount,0)Additional_Amount,ISNULL(Rebate_Amount,0)Rebate_Amount,Hostel_Code,ISNULL(GroupAmount,0)GroupAmount,Rebete_days,College_Code  from MessBill_Detail  where MessBill_MasterCode='" + Convert.ToString(group) + "'";
                        ds1.Clear();
                        ds1 = d2.select_method_wo_parameter(q1, "Text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            q1 = "select Mess_MonthYear,Mess_Amount,Hostel_Code,College_Code,NvMess_Amount, noofdays, studstrength,mandays,incgroupcode,expgroupcode from MessDividing_Details";
                            ds2.Clear();
                            ds2 = d2.select_method_wo_parameter(q1, "text");
                            if (ds2.Tables[0].Rows.Count > 0)
                            {
                                for (i = 0; i < ds2.Tables[0].Rows.Count; i++)
                                {
                                    string messmasterfk1 = d2.GetFunction("select MessMasterPK from MessDetail m,MessMaster mm,HM_MessMaster hm where mm.MessID=m.MessID and hm.MessName=mm.MessName and m.Hostel_Code in('" + Convert.ToString(ds2.Tables[0].Rows[i]["Hostel_Code"]) + "')");
                                    if (messmasterfk1.Trim() != "" && messmasterfk1.Trim() != "0")
                                    {
                                        string monthandyear = Convert.ToString(ds2.Tables[0].Rows[i]["Mess_MonthYear"]);
                                        string[] mon = monthandyear.Split('-');
                                        mon = monthandyear.Split('/');
                                        q1 = "insert into HMessbill_StudDetails (Hostel_Code,MessBill_Month,MessBill_Year,No_Of_Days,rebate_days,Per_Day_Amount,Extras,guest,rebate_amount,Total,inmatetype,mess_amount,incGroupCode,ExpGroupCode,StudStrength,ManDays,Hreg_code)values('" + messmasterfk1 + "','" + Convert.ToString(mon[0]) + "','" + Convert.ToString(mon[1]) + "','" + Convert.ToString(ds2.Tables[0].Rows[i]["noofdays"]) + "','" + Convert.ToString(ds1.Tables[0].Rows[i]["Rebete_days"]) + "','" + Convert.ToString(ds2.Tables[0].Rows[i]["NvMess_Amount"]) + "','0','0','" + Convert.ToString(ds1.Tables[0].Rows[i]["Rebate_Amount"]) + "','0','0','" + Convert.ToString(ds1.Tables[0].Rows[i]["Fixed_Amount"]) + "','" + Convert.ToString(ds2.Tables[0].Rows[i]["incgroupcode"]) + "','" + Convert.ToString(ds2.Tables[0].Rows[i]["expgroupcode"]) + "','" + Convert.ToString(ds2.Tables[0].Rows[i]["studstrength"]) + "','" + Convert.ToString(ds2.Tables[0].Rows[i]["mandays"]) + "','0')";
                                        insert = d2.update_method_wo_parameter(q1, "Text");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Messbill = true; except = except + "-" + Convert.ToString(ex.Message); }

            #endregion


            if (codesetting == false && messmaster == false && storemaster == false && storedept == false && itemmaster == false && itemdeptmaster == false && vendormaster == false && vendeptmaster == false && Hostelstaff == false && guestcontectreg == false && guestcontectdet == false && guestregister == false && studentmentor == false && sessionmaster == false && menumaster == false && hostelmasterattence == false && menuitemdetails == false && menuschedule == false && openningstock == false && purchaseorder == false && Dailyconsumption == false && GoodInward == false && Income == false && Expances == false && Rebate == false && StudentRebateDetails == false && Studentadditionaldetails == false && MenucostMaster == false && Studenttokendetails == false && Messbill == false && deletetable == false && messdetails == false && vendorbankdetails == false && Hostelsetting == false && cleanitem == false && vendorcontactdel == false)
            {
                lbl_convert.Visible = true;
                lbl_convert.Font.Bold = true;
                lbl_convert.Text = "File Transfer Successfully";
                lbl_convert.ForeColor = Color.Green;
            }
            else
            {
                lbl_convert.Font.Bold = false;
                lbl_convert.Visible = true;
                lbl_convert.ForeColor = Color.Red;
                // lbl_convert.Text = "Some Issues Occurs";
                lbl_convert.Text = except;
            }
            */
        }
        catch (Exception ex)
        {

        }
    }
    public void ConvertHostel()
    {
        try
        {
            string Insert = "Update Hostel_StudentDetails set Roll_No =(select Roll_No from Registration r where r.Roll_Admit =Hostel_StudentDetails.Roll_Admit) from Registration r where r.Roll_Admit =Hostel_StudentDetails.Roll_Admit";
            int NewUpdate = d2.update_method_wo_parameter(Insert, "Text");
            string insertquery = "";
            int InsertQuery = 0;

            //insertquery = insertquery + " Alter table  HT_HostelRegistration drop Column Roll_No";
            //insertquery = insertquery + " Alter table  HT_HostelRegistration drop Column BulidingName";
            //insertquery = insertquery + " Alter table  HT_HostelRegistration drop Column RoomName";
            //insertquery = insertquery + " Alter table  HT_HostelRegistration drop Column FloorName";
            //insertquery = insertquery + " Alter table  HT_HostelRegistration drop Column HostelName";
            insertquery = insertquery + " delete from HT_HostelRegistration";
            insertquery = insertquery + " delete from HM_HostelMaster";
            InsertQuery = d2.update_method_wo_parameter(insertquery, "Text");


            insertquery = "insert into HM_HostelMaster (HostelName,HostelType,HostelBuildingFK)  select Hostel_Name,Gender_Type,Building_Code  from Hostel_Details";


            insertquery = insertquery + " Alter table HT_HostelRegistration Add Roll_No varchar(100)";
            insertquery = insertquery + " alter table HT_HostelRegistration Add BulidingName varchar(100)";
            insertquery = insertquery + " alter table HT_HostelRegistration Add RoomName varchar(100)";
            insertquery = insertquery + " Alter table  HT_HostelRegistration Add FloorName Varchar(100)";
            insertquery = insertquery + " Alter table  HT_HostelRegistration Add HostelName Varchar(100)";
            InsertQuery = d2.update_method_wo_parameter(insertquery, "Text");

            insertquery = "";
            insertquery = "insert into HT_HostelRegistration (MemType,HostelAdmDate,Roll_No,BulidingName,RoomName,FloorName,IsDiscontinued,DiscontinueDate,IsVacated,VacatedDate,StudMessType,HostelName)  select case when Is_Staff='0' then '1' when Is_Staff ='1' then '2' end as MemType,Admin_Date,Roll_No,Building_Name,Room_Name,Floor_Name,Relived,Relived_Date,Vacated,Vacated_Date,StudMess_Type,Hostel_Code from Hostel_StudentDetails";
            InsertQuery = d2.update_method_wo_parameter(insertquery, "Text");



            insertquery = "  update HT_HostelRegistration set APP_No =(select APP_No from Registration r where r.Roll_No =HT_HostelRegistration.Roll_No ) from Registration r where r.Roll_No =HT_HostelRegistration.Roll_No";

            InsertQuery = d2.update_method_wo_parameter(insertquery, "Text");



            insertquery = "   update HT_HostelRegistration set BuildingFK =(select code from Building_Master r where r.Building_Name =HT_HostelRegistration.BulidingName) from Building_Master r where r.Building_Name =HT_HostelRegistration.BulidingName";

            InsertQuery = d2.update_method_wo_parameter(insertquery, "Text");

            insertquery = "  select Roll_No,BulidingName,RoomName,FloorName,App_no,HostelName from HT_HostelRegistration   where ISNULL(APP_No,'') <>''";

            ds.Clear();
            ds = d2.select_method_wo_parameter(insertquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                {
                    string Build = Convert.ToString(ds.Tables[0].Rows[r]["BulidingName"]);
                    string BuildingFk = d2.GetFunction("  select code from Building_Master where Building_Name ='" + Build.Trim() + "'");
                    string RoomFk = d2.GetFunction("select Roompk from Room_Detail where Room_Name ='" + Convert.ToString(ds.Tables[0].Rows[r]["RoomName"]) + "' and Building_Name ='" + Convert.ToString(ds.Tables[0].Rows[r]["BulidingName"]) + "' and Floor_Name ='" + Convert.ToString(ds.Tables[0].Rows[r]["FloorName"]) + "'");
                    string FloorFk = d2.GetFunction(" select Floorpk from Floor_Master where Building_Name ='" + Convert.ToString(ds.Tables[0].Rows[r]["BulidingName"]) + "' and Floor_Name ='" + Convert.ToString(ds.Tables[0].Rows[r]["FloorName"]) + "'");

                    string HostelName = d2.GetFunction("select Hostel_Name  from Hostel_Details where Hostel_code ='" + Convert.ToString(ds.Tables[0].Rows[r]["HostelName"]) + "'");
                    string HostelPk = d2.GetFunction("select HostelMasterPK from HM_HostelMaster where HostelName ='" + HostelName + "'");

                    string Updatequery = "Update HT_HostelRegistration set FloorFK ='" + FloorFk + "',RoomFK ='" + RoomFk + "',BuildingFK ='" + BuildingFk + "',HostelMasterFK='" + HostelPk + "' where APP_No ='" + Convert.ToString(ds.Tables[0].Rows[r]["App_no"]) + "'";

                    int Upd = d2.update_method_wo_parameter(Updatequery, "Text");
                }
            }
            insertquery = "";
            insertquery = insertquery + " Alter table  HT_HostelRegistration drop Column Roll_No";
            insertquery = insertquery + " Alter table  HT_HostelRegistration drop Column BulidingName";
            insertquery = insertquery + " Alter table  HT_HostelRegistration drop Column RoomName";
            insertquery = insertquery + " Alter table  HT_HostelRegistration drop Column FloorName";
            insertquery = insertquery + " Alter table  HT_HostelRegistration drop Column HostelName";
            InsertQuery = d2.update_method_wo_parameter(insertquery, "Text");
            //Response.Write("Converted Successfully");

            lbl_convert.Visible = true;
            lbl_convert.Font.Bold = true;
            lbl_convert.Text = "File Transfer Successfully";
            lbl_convert.ForeColor = Color.Green;
        }
        catch (Exception ex)
        {
            lbl_convert.Visible = true;
            lbl_convert.Font.Bold = true;
            lbl_convert.Text = ex.ToString();
            lbl_convert.ForeColor = Color.Red;
        }
        //*/
    }
}