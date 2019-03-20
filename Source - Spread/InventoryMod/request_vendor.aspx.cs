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
using InsproDataAccess;
using System.Net.Mail;
using System.Net;

public partial class request_vendor : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegename = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    bool check = false;
    Boolean Cellclick = false;
    string dtaccessdate = DateTime.Now.ToString();
    string dtaccesstime = DateTime.Now.ToLongTimeString();
    private string itemcode;
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    static string itemorbook = "";


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
            CalendarExtender1.EndDate = DateTime.Now;
            caltodate.EndDate = DateTime.Now;
            if (!IsPostBack)
            {
                bindvenbase();
                //rdo_deptwise.Checked = true;
                //rdo_deptwise_Click(sender, e);
                Rad_item.Checked = true;
                rdo_approvedwise.Checked = true;
                //rdo_approvedwise_Click(sender, e);
                rdb_suggested.Checked = true;
                bindrequestcode();
                bindbookrequestcode();
                binddepartment();
                binditem();
                //bindvendorname();
                btn_popgo_Click(sender, e);
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
                txt_reqdate.Attributes.Add("readonly", "readonly");
                txt_reqdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                FpSpread5.Sheets[0].RowCount = 0;
                FpSpread5.Sheets[0].ColumnCount = 0;
                FpSpread5.Visible = false;
                btn_basescreen_Click(sender, e);

                Text_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                Text_Todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                Text_fromdate.Attributes.Add("readonly", "readonly");
                Text_Todate.Attributes.Add("readonly", "readonly");
                txtreqdate.Attributes.Add("readonly", "readonly");
                txtreqdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                binddepartment1();

                if (Rad_item.Checked)
                    itemorbook = "1";
                if (Rad_book.Checked)
                    itemorbook = "2";
            }
        }
        catch (Exception ex) { }
    }

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
                    txt_deptname.Text = "Department(" + cbl_dept.Items.Count + ")";
                }
                binditem();
            }
            else
            {
                txt_deptname.Text = "--Select--";
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
            cbl_item.Items.Clear();
            string deptcode = "";
            int i = 0;
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
            string item = "select distinct itemcode,itemname from IM_ItemDeptMaster dm,IM_ItemMaster im where im.itempk=dm.itemfk and dm.itemdeptfk in ('" + deptcode + "')";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_item.DataSource = ds;
                cbl_item.DataTextField = "itemname";
                cbl_item.DataValueField = "itemcode";
                cbl_item.DataBind();
                if (cbl_item.Items.Count > 0)
                {
                    for (i = 0; i < cbl_item.Items.Count; i++)
                    {
                        cbl_item.Items[i].Selected = true;
                    }
                    txt_itemname.Text = "Item (" + cbl_item.Items.Count + ")";
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

    protected void lnk_btnlogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
    protected void rdo_deptwise_Click(object sender, EventArgs e)
    {
        itemwise_false();
        requestwise_false();
        datewise_true();
        FpSpread1.Visible = false;
        btn_selectitem.Visible = false;
        lblvendor.Visible = false;
        txt_searchvendor.Visible = false;
        btn_request.Visible = false;
        txt_vendorname.Visible = false;
        UpdatePanel2.Visible = false;
        fdrblsmsmail.Visible = false;
        rblsmsmail.Visible = false;
        lbl_Online.Visible = false;
        chkallbook.Visible = false;

    }
    protected void rdo_itemwise_Click(object sender, EventArgs e)
    {
        datewise_false();
        requestwise_false();
        itemwise_true();
        FpSpread1.Visible = false;
        btn_selectitem.Visible = false;
        lblvendor.Visible = false;
        txt_searchvendor.Visible = false;
        btn_request.Visible = false;
        txt_vendorname.Visible = false;
        UpdatePanel2.Visible = false;
        fdrblsmsmail.Visible = false;
        rblsmsmail.Visible = false;
        lbl_Online.Visible = false;
        chkallbook.Visible = false;

    }
    protected void rdo_approvedwise_Click(object sender, EventArgs e)
    {
        itemwise_false();
        datewise_false();
        requestwise_true();
        //txt_searchreq.Visible = true;

        FpSpread1.Visible = false;
        btn_selectitem.Visible = false;
        lblvendor.Visible = false;
        txt_searchvendor.Visible = false;
        btn_request.Visible = false;
        txt_vendorname.Visible = false;
        UpdatePanel2.Visible = false;
        fdrblsmsmail.Visible = false;
        lbl_Online.Visible = false;
        rblsmsmail.Visible = false;
        chkallbook.Visible = false;
    }

    protected void cb_deptname_CheckedChange(object sender, EventArgs e)
    {
        int cout = 0;
        txt_deptname.Text = "--Select--";

        if (cb_dept.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_dept.Items.Count; i++)
            {
                cbl_dept.Items[i].Selected = true;
            }
            txt_deptname.Text = "Department(" + (cbl_dept.Items.Count) + ")";
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
    protected void cbl_deptname_SelectedIndexChange(object sender, EventArgs e)
    {
        int i = 0;
        cb_dept.Checked = false;
        //item();
        int commcount = 0;
        txt_deptname.Text = "--Select--";
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
            txt_deptname.Text = "Department(" + commcount.ToString() + ")";
        }
        binditem();
    }

    protected void cb_item_CheckedChange(object sender, EventArgs e)
    {
        int cout = 0;
        txt_itemname.Text = "--Select--";

        if (cb_item.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_item.Items.Count; i++)
            {
                cbl_item.Items[i].Selected = true;
            }
            txt_itemname.Text = "Item(" + (cbl_item.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_item.Items.Count; i++)
            {
                cbl_item.Items[i].Selected = false;
            }
        }
        //item();

    }
    protected void cbl_item_SelectedIndexChange(object sender, EventArgs e)
    {
        int i = 0;
        cb_item.Checked = false;
        //item();
        int commcount = 0;
        txt_itemname.Text = "--Select--";
        for (i = 0; i < cbl_item.Items.Count; i++)
        {
            if (cbl_item.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_item.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_item.Items.Count)
            {
                cb_item.Checked = true;
            }
            txt_itemname.Text = "Item(" + commcount.ToString() + ")";
        }
    }

    protected void cb_vendorname_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;
        txt_vendorname.Text = "--Select--";

        if (cb_vendorname.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_vendorname.Items.Count; i++)
            {
                cbl_vendorname.Items[i].Selected = true;
            }
            txt_vendorname.Text = "Vendor Name(" + (cbl_vendorname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_vendorname.Items.Count; i++)
            {
                cbl_vendorname.Items[i].Selected = false;
            }
        }
        //item();

    }
    protected void cbl_vendorname_SelectedIndexChange(object sender, EventArgs e)
    {
        int i = 0;
        cb_vendorname.Checked = false;
        int commcount = 0;
        txt_vendorname.Text = "--Select--";
        for (i = 0; i < cbl_vendorname.Items.Count; i++)
        {
            if (cbl_vendorname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_vendorname.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_vendorname.Items.Count)
            {
                cb_vendorname.Checked = true;
            }
            txt_vendorname.Text = "Vendor Name(" + commcount.ToString() + ")";
        }
    }

    //public void bindvendorname()
    //{
    //    try
    //    {
    //        ds.Clear();
    //        cbl_vendorname.Items.Clear();

    //        ds = d2.BindVendorName_inv();
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            cbl_vendorname.DataSource = ds;
    //            cbl_vendorname.DataTextField = "VendorCompName";
    //            cbl_vendorname.DataValueField = "VendorCode";
    //            cbl_vendorname.DataBind();
    //            if (cbl_vendorname.Items.Count > 0)
    //            {
    //                for (int i = 0; i < cbl_vendorname.Items.Count; i++)
    //                {
    //                    cbl_vendorname.Items[i].Selected = true;
    //                }
    //                txt_vendorname.Text = "Vendor Name(" + cbl_vendorname.Items.Count + ")";
    //            }

    //        }
    //    }
    //    catch
    //    {
    //    }
    //}

    protected void btn_go_Click(object sender, EventArgs e)
    {

        try
        {
            lbl_error.Visible = true;
            FpSpread1.Visible = false;
            btn_selectitem.Visible = false;
            reqdiv.Visible = false;
            smsmcontent.Visible = false;
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string firstdate = Convert.ToString(txt_fromdate.Text);
            string seconddate = Convert.ToString(txt_todate.Text);
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            string[] split1 = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);

            string deptcode = "";
            for (int i = 0; i < cbl_dept.Items.Count; i++)
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
            string itemcode = "";

            for (int i = 0; i < cbl_item.Items.Count; i++)
            {
                if (cbl_item.Items[i].Selected == true)
                {
                    if (itemcode == "")
                    {
                        itemcode = "" + cbl_item.Items[i].Value.ToString() + "";

                    }
                    else
                    {
                        itemcode = itemcode + "'" + "," + "'" + cbl_item.Items[i].Value.ToString() + "";
                    }
                }
            }
            string vendorselectquery = "";
            string vendorselectquery1 = "";
            if (txt_searchreq.Text.Trim() != "")
            {
                vendorselectquery = "select distinct r.Requestcode,RequisitionPK,i.itemheadername,i.itemheadercode,i.itemcode,i.itemname,rd.ReqQty -isnull(RejQty,0)as appqty,convert(varchar(10), r.requestdate,103) as requestdate,i.ItemPK  from RQ_Requisition R,RQ_RequisitionDet RD,IM_ItemMaster I  where R.RequisitionPK=RD.RequisitionFK and RD.ItemFK = I.ItemPK and RequestType ='1' and  requestdate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and  rd.deptfk in('" + deptcode + "') and i.itemcode in ('" + itemcode + "')  and r.requestcode='" + txt_searchreq.Text + "' and ISNULL(VendorReq_Type,'0')=0 and RD.ReqAppStatus='1' ";// RequisitionPK not in(select ReqFK from IT_VendorReqDet ) and r.RequestType='1' ";
                //vendorselectquery = "select distinct r.Requestcode,RequisitionPK,i.itemheadername,i.itemheadercode,i.itemcode,i.itemname,rd.appqty,convert(varchar(10), r.requestdate,103) as requestdate,vm.vendorpk,vd.itemfk from RQ_Requisition R,RQ_RequisitionDet RD,IM_ItemMaster I,CO_VendorMaster vm,IM_VendorItemDept vd  where R.RequisitionPK=RD.RequisitionFK and RD.ItemFK = I.ItemPK and RequestType ='1' and r.requestcode='" + txt_searchreq.Text + "' and vm.vendorpk=vd.venitemfk and i.itempk=vd.itemfk";
                vendorselectquery1 = " select distinct i.ItemPK from RQ_Requisition R,RQ_RequisitionDet RD,IM_ItemMaster I where R.RequisitionPK =RD.RequisitionFK and RD.ItemFK = I.ItemPK and RequestType ='1' and  requestdate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and  rd.deptfk in('" + deptcode + "') and i.itemcode in ('" + itemcode + "') and  ISNULL(VendorReq_Type,'0')=0 and RD.ReqAppStatus='1' ";
            }
            else
            {
                vendorselectquery = " select distinct Requestcode,RequisitionPK,i.itemheadername,i.itemheadercode,i.ItemPK ,i.itemcode,i.itemname,rd.ReqQty -isnull(RejQty,0)as appqty,convert(varchar(10), r.requestdate,103) as requestdate from RQ_Requisition R,RQ_RequisitionDet RD,IM_ItemMaster I where R.RequisitionPK =RD.RequisitionFK and RD.ItemFK = I.ItemPK and RequestType ='1' and  requestdate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and  rd.deptfk in('" + deptcode + "') and i.itemcode in ('" + itemcode + "') and  ISNULL(VendorReq_Type,'0')=0 and RD.ReqAppStatus='1' ";

                vendorselectquery1 = " select distinct i.ItemPK from RQ_Requisition R,RQ_RequisitionDet RD,IM_ItemMaster I where R.RequisitionPK =RD.RequisitionFK and RD.ItemFK = I.ItemPK and RequestType ='1' and  requestdate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and  rd.deptfk in('" + deptcode + "') and i.itemcode in ('" + itemcode + "') and  ISNULL(VendorReq_Type,'0')=0 and RD.ReqAppStatus='1' ";


                //RequisitionPK not in(select ReqFK from IT_VendorReqDet ) and r.RequestType='1'";
                // vendorselectquery = "select distinct Requestcode,RequisitionPK,i.itemheadername,i.itemheadercode,i.itemcode,i.itemname,rd.appqty,convert(varchar(10), r.requestdate,103) as requestdate,vm.vendorpk,vd.itemfk from RQ_Requisition R,RQ_RequisitionDet RD,IM_ItemMaster I,CO_VendorMaster vm,IM_VendorItemDept vd  where R.RequisitionPK =RD.RequisitionFK and RD.ItemFK = I.ItemPK and RequestType ='1' and  requestdate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and  rd.deptfk in('" + deptcode + "') and i.itemcode in ('" + itemcode + "') and vm.vendorpk=vd.venitemfk and i.itempk=vd.itemfk";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(vendorselectquery, "Text");
            if (deptcode.Trim() != "" && itemcode.Trim() != "")
            {


                if (txt_searchreq.Text.Trim() != "")
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].DefaultView.RowFilter = "itemheadername='No Header'";
                        DataView dvitem = ds.Tables[0].DefaultView;
                        if (dvitem.Count > 0)
                        {
                            string itemname1 = "";
                            for (int it = 0; it < dvitem.Count; it++)
                            {
                                string itemname = Convert.ToString(dvitem[it]["itemname"]);
                                if (itemname1 == "")
                                    itemname1 = itemname;
                                else
                                    itemname1 = itemname1 + "," + itemname;
                            }
                            if (itemname1 != "")
                            {
                                divPopAlertNEW.Visible = true;
                                divPopAlertContent.Visible = true;
                                lblAlertMsgNEW.Text = itemname1 + "This Items Are Not Having Item Header!..Do You Want To Continue?";
                            }
                        }
                        else
                        {
                            loaditemsdetails();

                        }
                    }
                    else
                    {
                        lbl_error.Visible = true;
                        FpSpread1.Visible = false;
                        //spreaddiv.Visible = false;
                        lbl_error.Text = "No Record Found";
                        btn_selectitem.Visible = false;
                        reqdiv.Visible = false;
                        smsmcontent.Visible = false;
                        //rptprint.Visible = false;
                    }
                }
                else
                {

                    string itempf = d2.GetFunction(vendorselectquery1);
                    if (itempf != "" && itempf != "0")
                    {
                        DataSet dsven = new DataSet();
                        string selectquery = " select distinct Requestcode,RequisitionPK,i.itemheadername,i.itemheadercode,i.ItemPK ,i.itemcode,i.itemname,rd.ReqQty -isnull(RejQty,0)as appqty,convert(varchar(10), r.requestdate,103) as requestdate from RQ_Requisition R,RQ_RequisitionDet RD,IM_ItemMaster I where R.RequisitionPK =RD.RequisitionFK and RD.ItemFK = I.ItemPK and RequestType ='1' and  requestdate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and  rd.deptfk in('" + deptcode + "') and ItemPK not in(" + vendorselectquery1 + ") and  ISNULL(VendorReq_Type,'0')=0 and RD.ReqAppStatus='1' ";
                        dsven.Clear();
                        dsven = d2.select_method_wo_parameter(selectquery, "Text");
                        if (dsven.Tables.Count > 0 && dsven.Tables[0].Rows.Count > 0)
                        {
                            string itemname1 = "";
                            for (int it = 0; it < dsven.Tables[0].Rows.Count; it++)
                            {
                                string itemname = Convert.ToString(dsven.Tables[0].Rows[it]["itemname"]);
                                if (itemname1 == "")
                                    itemname1 = itemname;
                                else
                                    itemname1 = itemname1 + "," + itemname;
                            }
                            if (itemname1 != "")
                            {
                                divPopAlertNEW.Visible = true;
                                divPopAlertContent.Visible = true;
                                lblAlertMsgNEW.Text = itemname1 + "This Items Are Not Having Item Header!..Do You Want To Continue?";
                            }
                        }
                        else
                        {
                            loaditemsdetails();

                        }
                    }
                    else
                    {
                        loaditemsdetails();

                    }

                }



            }
            else
            {
                lbl_error.Visible = true;
                FpSpread1.Visible = false;
                //spreaddiv.Visible = false;
                // rptprint.Visible = false;
                lbl_error.Text = "Please Select All Fields";

            }
        }
        catch
        {
        }
    }


    public void loaditemsdetails()
    {
        try
        {

            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string firstdate = Convert.ToString(txt_fromdate.Text);
            string seconddate = Convert.ToString(txt_todate.Text);
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            string[] split1 = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);

            string deptcode = "";
            for (int i = 0; i < cbl_dept.Items.Count; i++)
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
            string itemcode = "";

            for (int i = 0; i < cbl_item.Items.Count; i++)
            {
                if (cbl_item.Items[i].Selected == true)
                {
                    if (itemcode == "")
                    {
                        itemcode = "" + cbl_item.Items[i].Value.ToString() + "";

                    }
                    else
                    {
                        itemcode = itemcode + "'" + "," + "'" + cbl_item.Items[i].Value.ToString() + "";
                    }
                }
            }
            string vendorselectquery = "";

            if (txt_searchreq.Text.Trim() != "")
            {
                vendorselectquery = "select distinct r.Requestcode,RequisitionPK,i.itemheadername,i.itemheadercode,i.itemcode,i.itemname,rd.ReqQty -isnull(RejQty,0)as appqty,convert(varchar(10), r.requestdate,103) as requestdate,i.ItemPK  from RQ_Requisition R,RQ_RequisitionDet RD,IM_ItemMaster I  where R.RequisitionPK=RD.RequisitionFK and RD.ItemFK = I.ItemPK and RequestType ='1' and r.requestcode='" + txt_searchreq.Text + "' and ISNULL(VendorReq_Type,'0')=0 and RD.ReqAppStatus='1' ";
            }
            else
            {
                vendorselectquery = " select distinct Requestcode,RequisitionPK,i.itemheadername,i.itemheadercode,i.ItemPK ,i.itemcode,i.itemname,rd.ReqQty -isnull(RejQty,0)as appqty,convert(varchar(10), r.requestdate,103) as requestdate from RQ_Requisition R,RQ_RequisitionDet RD,IM_ItemMaster I where R.RequisitionPK =RD.RequisitionFK and RD.ItemFK = I.ItemPK and RequestType ='1' and  requestdate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and  rd.deptfk in('" + deptcode + "') and i.itemcode in ('" + itemcode + "') and  ISNULL(VendorReq_Type,'0')=0 and RD.ReqAppStatus='1' ";

            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(vendorselectquery, "Text");
            if (ds.Tables.Count > 0 & ds.Tables[0].Rows.Count == 0)
            {

                if (txt_searchreq.Text.Trim() != "")
                {
                    vendorselectquery = "select distinct r.Requestcode,RequisitionPK,i.itemheadername,i.itemheadercode,i.itemcode,i.itemname,rd.ReqQty -isnull(RejQty,0)as appqty,convert(varchar(10), r.requestdate,103) as requestdate,i.ItemPK  from RQ_Requisition R,RQ_RequisitionDet RD,IM_ItemMaster I  where R.RequisitionPK=RD.RequisitionFK and RD.ItemFK = I.ItemPK and RequestType ='1' and r.requestcode='" + txt_searchreq.Text + "' and ISNULL(VendorReq_Type,'0')=1 and RD.ReqAppStatus='1' and ItemFK not in( select ItemFK from IT_VendorReqDet where ReqFK=RequisitionPK) ";
                    // vendorselectquery = "select distinct r.Requestcode,RequisitionPK,i.itemheadername,i.itemheadercode,i.itemcode,i.itemname,rd.ReqQty -isnull(RejQty,0)as appqty,convert(varchar(10), r.requestdate,103) as requestdate,i.ItemPK  from RQ_Requisition R,RQ_RequisitionDet RD,IM_ItemMaster I,IT_VendorReqDet rt  where R.RequisitionPK=RD.RequisitionFK and RD.ItemFK = I.ItemPK and RequestType ='1' and r.requestcode='" + txt_searchreq.Text + "' and ISNULL(VendorReq_Type,'0')=1 and RD.ReqAppStatus='1'  and rt.ReqFK=R.RequisitionPK  and rt.ItemFK=i.ItemPK and RD.ItemFK <> rt.ItemFK";


                }
                else
                {
                    vendorselectquery = " select distinct Requestcode,RequisitionPK,i.itemheadername,i.itemheadercode,i.ItemPK ,i.itemcode,i.itemname,rd.ReqQty -isnull(RejQty,0)as appqty,convert(varchar(10), r.requestdate,103) as requestdate from RQ_Requisition R,RQ_RequisitionDet RD,IM_ItemMaster I where R.RequisitionPK =RD.RequisitionFK and RD.ItemFK = I.ItemPK and RequestType ='1' and  requestdate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and  rd.deptfk in('" + deptcode + "') and i.itemcode in ('" + itemcode + "') and  ISNULL(VendorReq_Type,'0')=1 and RD.ReqAppStatus='1' and ItemFK not in( select ItemFK from IT_VendorReqDet where ReqFK=RequisitionPK) ";
                    //vendorselectquery = " select distinct Requestcode,RequisitionPK,i.itemheadername,i.itemheadercode,i.ItemPK ,i.itemcode,i.itemname,rd.ReqQty -isnull(RejQty,0)as appqty,convert(varchar(10), r.requestdate,103) as requestdate from RQ_Requisition R,RQ_RequisitionDet RD,IM_ItemMaster I,IT_VendorReqDet rt where R.RequisitionPK =RD.RequisitionFK and RD.ItemFK = I.ItemPK and RequestType ='1' and  requestdate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and  rd.deptfk in('" + deptcode + "') and i.itemcode in ('" + itemcode + "') and  ISNULL(VendorReq_Type,'0')=1 and RD.ReqAppStatus='1' and rt.ReqFK=R.RequisitionPK  and rt.ItemFK=i.ItemPK and RD.ItemFK <> rt.ItemFK  ";


                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(vendorselectquery, "Text");
            }
            if (deptcode.Trim() != "" && itemcode.Trim() != "")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    FpSpread1.Sheets[0].RowCount = 1;
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    FpSpread1.CommandBar.Visible = false;
                    FpSpread1.Sheets[0].AutoPostBack = false;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread1.Sheets[0].RowHeader.Visible = false;
                    FpSpread1.Sheets[0].ColumnCount = 8;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[0].Locked = true;
                    FpSpread1.Columns[0].Width = 50;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[1].Width = 80;
                    FpSpread1.Columns[1].Visible = false;

                    FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                    chkall.AutoPostBack = true;

                    FpSpread1.Sheets[0].Cells[0, 1].CellType = chkall;
                    FpSpread1.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    chkall.Text = " ";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Request Code";//Approve Date
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[2].Locked = true;
                    FpSpread1.Columns[2].Width = 100;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item Header Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[3].Locked = true;
                    FpSpread1.Columns[3].Width = 200;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Item Code";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[4].Locked = true;
                    FpSpread1.Columns[4].Width = 100;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Item Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[5].Locked = true;
                    FpSpread1.Columns[5].Width = 200;


                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Approved Quantity";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[6].Locked = true;
                    FpSpread1.Columns[6].Width = 100;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Request Date";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[7].Locked = true;
                    FpSpread1.Columns[7].Width = 100;

                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Department";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    //FpSpread1.Columns[5].Width = 200;
                    FarPoint.Web.Spread.CheckBoxCellType chkbox = new FarPoint.Web.Spread.CheckBoxCellType();
                    chkbox.AutoPostBack = false;
                    chkbox.Text = " ";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["RequisitionPK"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        if (rdb_all.Checked == true)
                        {
                            FpSpread1.Columns[1].Visible = false;
                            FpSpread1.Width = 867;
                        }
                        if (rdb_suggested.Checked == true)
                        {
                            FpSpread1.Columns[1].Visible = true;
                            FpSpread1.Width = 947;
                        }

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chkbox;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Requestcode"]);
                        // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["vendorpk"]);

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";


                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["itemheadername"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["itemheadercode"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["itemcode"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["itempk"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["itemname"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["appqty"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["requestdate"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                    }
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    //FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    //FpSpread1.Sheets[0].FrozenRowCount = 0;
                    FpSpread1.SaveChanges();
                    FpSpread1.Visible = true;
                    //spreaddiv.Visible = true;
                    lbl_error.Visible = false;
                    //rptprint.Visible = true;
                    txt_searchreq.Text = "";
                    reqdiv.Visible = true;
                    smsmcontent.Visible = true;
                    lblvendor.Visible = true;
                    txt_searchvendor.Visible = false;
                    btn_request.Visible = true;
                    txt_vendorname.Visible = true;
                    UpdatePanel2.Visible = true;
                    fdrblsmsmail.Visible = true;
                    rblsmsmail.Visible = true;
                    lbl_Online.Visible = true;
                    chkallbook.Visible = false;
                    txt_vendorname.Text = "--Select--";
                    if (rdb_all.Checked == true)
                    {
                        btn_selectitem.Visible = false;
                    }
                    else
                    {
                        btn_selectitem.Visible = true;
                        btn_selectitem.Text = "Selected Item";
                    }
                }
                else
                {
                    lbl_error.Visible = true;
                    FpSpread1.Visible = false;
                    //spreaddiv.Visible = false;
                    lbl_error.Text = "No Record Found";
                    btn_selectitem.Visible = false;
                    reqdiv.Visible = false;
                    smsmcontent.Visible = false;
                    //rptprint.Visible = false;
                }
            }
        }
        catch
        {

        }

    }

    protected void btn_yes_Click(object sender, EventArgs e)
    {
        divPopAlertNEW.Visible = false;
        divPopAlertContent.Visible = false;
        lblAlertMsgNEW.Text = "";
        loaditemsdetails();
    }

    protected void btn_No_Click(object sender, EventArgs e)
    {
        divPopAlertNEW.Visible = false;
        divPopAlertContent.Visible = false;
        lblAlertMsgNEW.Text = "";
        Response.Redirect("Item_master.aspx");
    }
    protected void Fpspread1_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (FpSpread1.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void btn_go1_Click(object sender, EventArgs e)
    {
        try
        {


            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    FpSpread4.Sheets[0].RowCount = 0;
            //    FpSpread4.Sheets[0].ColumnCount = 0;
            //    FpSpread4.CommandBar.Visible = false;
            //    FpSpread4.Sheets[0].AutoPostBack = false;
            //    FpSpread4.Sheets[0].ColumnHeader.RowCount = 1;
            //    FpSpread4.Sheets[0].RowHeader.Visible = false;
            //    FpSpread4.Sheets[0].ColumnCount = 7;

            //    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            //    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            //    darkstyle.ForeColor = Color.White;
            //    FpSpread4.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            //    FpSpread4.Columns[0].Width = 50;

            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Text = " Select";
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            //    FpSpread4.Columns[1].Width = 100;

            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Vendor Name";
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            //    FpSpread4.Columns[2].Width = 200;

            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item Can Supply ";
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            //    FpSpread4.Columns[3].Width = 100;

            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Already Supplied Quantity";
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            //    FpSpread4.Columns[4].Width = 100;

            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Delivered Status";
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            //    FpSpread4.Columns[5].Width = 200;


            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Remarks";
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            //    FpSpread4.Columns[5].Width = 200;

            //    FarPoint.Web.Spread.CheckBoxCellType chkbox = new FarPoint.Web.Spread.CheckBoxCellType();
            //    chkbox.AutoPostBack = false;
            //    chkbox.Text = " ";

            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        FpSpread4.Sheets[0].RowCount++;
            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = chkbox;
            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["VenItemSupplyDur"]);
            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["VenItemIsSupplied"]);
            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["VenItemReference"]);
            //        //FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[i]["MessId"]);

            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
            //        FpSpread4.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
            //    }
            //    FpSpread4.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
            //    FpSpread4.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //    FpSpread4.Sheets[0].FrozenRowCount = 0;
            //    FpSpread4.Visible = true;

            //    lbl_error.Visible = false;
            //    //rptprint.Visible = true;

            //}
            //else
            //{
            //    lbl_error.Visible = true;
            //    FpSpread2.Visible = false;

            //    lbl_error.Text = "No Record Found";
            //    //rptprint.Visible = false;
            //}

            //else
            //{
            //  lbl_error.Visible = true;
            //  FpSpread2.Visible = false;
            //  spreaddiv.Visible = false;
            //  rptprint.Visible = false;
            //  lbl_error.Text = "Please Select All Fields";

            //}

        }
        catch
        {
        }

    }

    protected void FpSpread2_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
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
        catch
        {
        }
    }

    protected void btn_popgo_Click(object sender, EventArgs e)
    {
        string vendorname = "";
        if (txt_popsearchvendor.Text.Trim() != "")
        {
            vendorname = " select vendor_code,vendor_name from vendor_details where vendor_name='" + txt_popsearchvendor.Text + "'";
        }
        else
        {
            vendorname = "select vendor_code,vendor_name from vendor_details ";
        }
        ds.Clear();
        ds = d2.select_method_wo_parameter(vendorname, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            FpSpread3.Sheets[0].RowCount = 0;
            FpSpread3.Sheets[0].ColumnCount = 0;
            FpSpread3.CommandBar.Visible = false;
            FpSpread3.Sheets[0].AutoPostBack = true;
            FpSpread3.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread3.Sheets[0].RowHeader.Visible = false;
            FpSpread3.Sheets[0].ColumnCount = 3;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread3.Columns[0].Width = 50;

            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Vendor Code";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread3.Columns[1].Width = 150;


            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Vendor Name";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread3.Columns[2].Width = 350;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                FpSpread3.Sheets[0].RowCount++;
                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["vendor_code"]);
                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["vendor_name"]);
                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
            }

            FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
            FpSpread3.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);

            FpSpread3.Visible = true;
            vendorsearch_div.Visible = true;
            lbl_error2.Visible = false;

        }
        else
        {
            FpSpread3.Visible = false;
            vendorsearch_div.Visible = false;
            lbl_error2.Visible = true;
            lbl_error2.Text = "No Record Found";
        }


    }

    protected void FpSpread3_CellClick(object sender, EventArgs e)
    {
        try
        {
            Cellclick = true;
        }
        catch (Exception ex)
        {
        }
    }
    protected void FpSpread3_render(object sender, EventArgs e)
    {
        if (Cellclick == true)
        {
            try
            {
                string activerow = "";
                string activecol = "";
                activerow = FpSpread3.ActiveSheetView.ActiveRow.ToString();
                activecol = FpSpread3.ActiveSheetView.ActiveColumn.ToString();
                if (activerow.Trim() != "" && activecol != "0")
                {
                    string vendorcode = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    string vendorname = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);

                    txt_searchvendor.Text = vendorname;

                    pop_vendor.Visible = false;
                }
            }
            catch { }
        }
    }


    protected void txt_popsearchvendor_txt_change(object sender, EventArgs e)
    {
        try
        {
            string q1 = d2.GetFunction("select distinct vendor_code,vendor_name from vendor_details where vendor_name='" + txt_popsearchvendor.Text + "' order by vendor_name");
            if (q1.Trim() != "" && q1.Trim() != "0")
            {
                lbl_error2.Visible = false;

            }
            else
            {
                txt_popsearchvendor.Text = "";
                lbl_error2.Visible = true;
                vendorsearch_div.Visible = false;
                lbl_error2.Text = "Please enter the correct vendor name";

            }
        }
        catch
        { }
    }
    protected void txt_searchvendor_txt_change(object sender, EventArgs e)
    {
        try
        {
            string q1 = d2.GetFunction("select distinct vendor_code,vendor_name from vendor_details where vendor_name='" + txt_searchvendor.Text + "' order by vendor_name");
            if (q1.Trim() != "" && q1.Trim() != "0")
            {

            }
            else
            {
                txt_searchvendor.Text = "";
            }
        }
        catch
        { }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        try
        {
            alertmessage.Visible = false;
        }
        catch { }
        //sug_all.Visible = false;
    }
    //visible functions
    protected void datewise_true()
    {
        lbl_deptname.Visible = true;
        txt_deptname.Visible = true;
        p1.Visible = true;
        lbl_fromdate.Visible = true;
        txt_fromdate.Visible = true;
        lbl_todate.Visible = true;
        txt_todate.Visible = true;

        // lbl_search.Visible = true;
        txt_searchdept.Visible = true;


    }
    protected void datewise_false()
    {
        lbl_deptname.Visible = false;
        txt_deptname.Visible = false;
        p1.Visible = false;
        lbl_fromdate.Visible = false;
        txt_fromdate.Visible = false;
        lbl_todate.Visible = false;
        txt_todate.Visible = false;

        //lbl_search.Visible = false;
        txt_searchdept.Visible = false;
    }
    protected void itemwise_true()
    {
        lbl_itemname.Visible = true;
        txt_itemname.Visible = true;
        p2.Visible = true;


        lbl_deptname.Visible = true;
        txt_deptname.Visible = true;
        p1.Visible = true;
        lbl_fromdate.Visible = true;
        txt_fromdate.Visible = true;
        lbl_todate.Visible = true;
        txt_todate.Visible = true;


        txt_searchitem.Visible = true;
        // ddl_search.Visible = true;
    }
    protected void itemwise_false()
    {
        lbl_itemname.Visible = false;
        txt_itemname.Visible = false;
        p2.Visible = false;


        lbl_deptname.Visible = false;
        txt_deptname.Visible = false;
        p1.Visible = false;
        lbl_fromdate.Visible = false;
        txt_fromdate.Visible = false;
        lbl_todate.Visible = false;
        txt_todate.Visible = false;

        //lbl_search.Visible = false;
        txt_searchitem.Visible = false;
        ddl_search.Visible = false;

    }
    protected void requestwise_true()
    {
        //cb_request.Visible = true;
        //cb_notrequest.Visible = true;

        lbl_itemname.Visible = true;
        txt_itemname.Visible = true;
        p2.Visible = true;


        lbl_deptname.Visible = true;
        txt_deptname.Visible = true;
        p1.Visible = true;


        lbl_fromdate.Visible = true;
        txt_fromdate.Visible = true;
        lbl_todate.Visible = true;
        txt_todate.Visible = true;

        txt_searchreq.Visible = true;

    }
    protected void requestwise_false()
    {
        cb_request.Visible = false;
        cb_notrequest.Visible = false;

        cb_request.Checked = false;
        cb_notrequest.Checked = false;

        lbl_itemname.Visible = false;
        txt_itemname.Visible = false;
        p2.Visible = false;


        lbl_deptname.Visible = false;
        txt_deptname.Visible = false;
        p1.Visible = false;


        lbl_deptname.Visible = false;
        txt_deptname.Visible = false;
        p1.Visible = false;

        lbl_itemname.Visible = false;
        txt_itemname.Visible = false;
        p2.Visible = false;

        txt_searchreq.Visible = false;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname1(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct  vendor_name from Vendor_Details WHERE vendor_name like '" + prefixText + "%' order by vendor_name";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["vendor_name"].ToString());
            }
        }
        return name;
    }

    protected void btn_vendorqmark_Click(object sender, EventArgs e)
    {
        pop_vendor.Visible = true;
    }
    protected void imagebtnpopclose3_Click(object sender, EventArgs e)
    {
        pop_vendor.Visible = false;
    }
    protected void ddl_search_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddl_search.SelectedValue == "0")
        {
            txt_searchitem.Visible = true;
            txt_searchdept.Visible = false;

            txt_searchitem.Text = "";
        }
        else if (ddl_search.SelectedValue == "1")
        {
            txt_searchitem.Visible = false;
            txt_searchdept.Visible = true;
            txt_searchdept.Text = "";
        }

    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> request_search(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "";
        if (itemorbook == "1")
            query = "select distinct Requestcode from RQ_Requisition R,RQ_RequisitionDet RD,IM_ItemMaster I where R.RequisitionPK =RD.RequisitionFK and RequestType ='1' and ISNULL(VendorReq_Type,'0')=0 and RD.ReqAppStatus='1' and Requestcode like '" + prefixText + "%' order by Requestcode";
        if (itemorbook == "2")
            query = "select distinct Requestcode from RQ_Requisition R,RQ_RequisitionDet RD,bookdetails bd,Library lb where R.RequisitionPK =RD.RequisitionFK and RD.Lib_Bookid = bd.BookID and RequestType ='13'   and  ISNULL(VendorReq_Type,'0')=0 and RD.ReqAppStatus='1' and isnull(bd.Lib_Code,'')<>'' and ISNULL(VendorReq_Type,'0')=0 and RD.ReqAppStatus='1' and lb.lib_code=bd.Lib_Code and Requestcode like '" + prefixText + "%' ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["Requestcode"].ToString());
            }
        }
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> searchdepartmentname(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "";
        if (itemorbook == "1")
            query = "SELECT h.Dept_Name FROM HRDept_Master h,department d WHERE h.dept_code=d.dept_code and d.College_Code=h.College_Code and h.dept_name like '" + prefixText + "%' order by h.Dept_Name";
        if (itemorbook == "2")
            query = "select distinct lb.lib_name,bd.Lib_Code from RQ_Requisition R,RQ_RequisitionDet RD,bookdetails bd,Library lb where R.RequisitionPK =RD.RequisitionFK and RD.Lib_Bookid = bd.BookID and RequestType ='13'   and  ISNULL(VendorReq_Type,'0')=0 and RD.ReqAppStatus='1' and isnull(bd.Lib_Code,'')<>'' and lb.lib_code=bd.Lib_Code and lb.lib_name like '" + prefixText + "%' ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["Dept_Name"].ToString());
            }
        }
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> searchitemname(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "";
        if (itemorbook == "1")
            query = "select distinct i.itemname from RQ_Requisition R,RQ_RequisitionDet RD,IM_ItemMaster I  where RD.ItemFK = I.ItemPK and r.RequestType ='1' and RD.ReqAppStatus='1'  and i.itemname like '" + prefixText + "%' order by i.itemname";
        if (itemorbook == "2")
            query = "select distinct bd.Title itemname from RQ_Requisition R,RQ_RequisitionDet RD,bookdetails bd,Library lb where R.RequisitionPK =RD.RequisitionFK and RD.Lib_Bookid = bd.BookID and RequestType ='13'   and  ISNULL(VendorReq_Type,'0')=0 and RD.ReqAppStatus='1' and isnull(bd.Lib_Code,'')<>'' and lb.lib_code=bd.Lib_Code and Requestcode like '" + prefixText + "%' ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["itemname"].ToString());
            }
        }
        return name;
    }


    protected void rdb_suggested_CheckedChanged(object sender, EventArgs e)
    {

        if (rdb_suggested.Checked == true)
        {
            txt_vendorname.Text = "---select---";
            cbl_vendorname.Items.Clear();
            cb_vendorname.Checked = false;
            FpSpread1.Visible = false;
            btn_selectitem.Visible = false;
            lblvendor.Visible = false;
            txt_searchvendor.Visible = false;
            btn_request.Visible = false;
            txt_vendorname.Visible = false;
            UpdatePanel2.Visible = false;
            fdrblsmsmail.Visible = false;
            rblsmsmail.Visible = false;
            lbl_Online.Visible = false;
            chkallbook.Visible = false;

        }

    }
    protected void rdb_allChecked_CheckedChange(object sender, EventArgs e)
    {
        btn_selectitem.Visible = false;
        string Query = "";
        if (rdb_all.Checked == true)
        {
            Query = "select VendorPK,VendorCompName  from CO_VendorMaster where VendorType ='1'";
        }

        ds.Clear();
        cbl_vendorname.Items.Clear();
        ds = d2.select_method_wo_parameter(Query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_vendorname.DataSource = ds;
            cbl_vendorname.DataTextField = "VendorCompName";
            cbl_vendorname.DataValueField = "VendorPK";
            cbl_vendorname.DataBind();
            if (cbl_vendorname.Items.Count > 0)
            {
                for (int i = 0; i < cbl_vendorname.Items.Count; i++)
                {

                    cbl_vendorname.Items[i].Selected = true;
                }
                txt_vendorname.Text = "Vendor Name(" + cbl_vendorname.Items.Count + ")";
            }


        }
        FpSpread1.Visible = false;
        btn_selectitem.Visible = false;
        lblvendor.Visible = false;
        txt_searchvendor.Visible = false;
        btn_request.Visible = false;
        txt_vendorname.Visible = false;
        UpdatePanel2.Visible = false;
        fdrblsmsmail.Visible = false;
        rblsmsmail.Visible = false;
        lbl_Online.Visible = false;
        chkallbook.Visible = false;


    }
    protected void btn_selectitem_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_error.Visible = false;


            if (Radioitem.Checked)
            {
                #region ITEM
                if (FpSpread1.Sheets[0].RowCount > 0)
                {
                    cbl_vendorname.Items.Clear();
                    txt_vendorname.Text = "--Select--";
                    FpSpread1.SaveChanges();
                    string itemcod = "";
                    for (int row = 1; row < FpSpread1.Sheets[0].RowCount; row++)
                    {
                        int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, 1].Value);
                        if (checkval == 1)
                        {
                            string itemcode = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 4].Text);

                            if (itemcod == "")
                            {
                                itemcod = "" + itemcode + "";
                            }
                            else
                            {
                                itemcod = itemcod + "'" + "," + "'" + itemcode + "";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(itemcod))
                    {
                        string venquery = "select distinct vm.VendorCompName,vm.VendorPK from CO_VendorMaster vm,IM_VendorItemDept vd,im_itemmaster i where vm.vendorpk=vd.venitemfk and vd.itemfk=i.itempk and i.itemcode in('" + itemcod + "')";
                        ds.Clear();
                        //cbl_vendorname.Items.Clear();
                        ds = d2.select_method_wo_parameter(venquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            cbl_vendorname.DataSource = ds;
                            cbl_vendorname.DataTextField = "VendorCompName";
                            cbl_vendorname.DataValueField = "VendorPK";
                            cbl_vendorname.DataBind();
                            if (cbl_vendorname.Items.Count > 0)
                            {
                                for (int i = 0; i < cbl_vendorname.Items.Count; i++)
                                {
                                    cbl_vendorname.Items[i].Selected = true;
                                }
                                txt_vendorname.Text = "Vendor Name(" + cbl_vendorname.Items.Count + ")";
                            }

                        }
                    }
                    else
                    {
                        lbl_error.Visible = true;
                        lbl_error.Text = "Please Select Atleast One Item";

                    }


                }
                #endregion
            }
            if (RadioBook.Checked)
            {
                #region BOOK
                if (Showgrid.Rows.Count > 0)
                {
                    cbl_vendorname.Items.Clear();
                    txt_vendorname.Text = "--Select--";

                    string bookid = "";
                    for (int row = 0; row < Showgrid.Rows.Count; row++)
                    {

                        System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)Showgrid.Rows[row].FindControl("selectchk");
                        if (chk.Checked)
                        {
                            Label lbl_bookid = (Label)Showgrid.Rows[row].FindControl("lbl_bookid");
                            string bookidno = lbl_bookid.Text;

                            if (bookid == "")
                            {
                                bookid = "" + bookidno + "";
                            }
                            else
                            {
                                bookid = bookid + "'" + "," + "'" + bookidno + "";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(bookid))
                    {
                        string venquery = "select distinct vm.VendorCompName,vm.VendorPK from CO_VendorMaster vm where vm.LibraryFlag='1'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(venquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            cbl_vendorname.DataSource = ds;
                            cbl_vendorname.DataTextField = "VendorCompName";
                            cbl_vendorname.DataValueField = "VendorPK";
                            cbl_vendorname.DataBind();
                            if (cbl_vendorname.Items.Count > 0)
                            {
                                for (int i = 0; i < cbl_vendorname.Items.Count; i++)
                                {
                                    cbl_vendorname.Items[i].Selected = true;
                                }
                                txt_vendorname.Text = "Vendor Name(" + cbl_vendorname.Items.Count + ")";
                            }

                        }
                    }
                    else
                    {
                        lbl_error.Visible = true;
                        lbl_error.Text = "Please Select Atleast One Book";

                    }

                }
                #endregion

            }
        }
        catch
        {


        }
    }

    protected void btn_Request_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean vensend = false;
            string mobilenoemail = "";
            string sendsms = "";
            Boolean sendsmsmail = false;
            if (rdb_all.Checked == true)
            {
                if (Radioitem.Checked)
                {
                    #region ITEM
                    if (FpSpread1.Sheets[0].Rows.Count > 0)
                    {
                        int reqfkins = 0;
                        int venreqins = 0;
                        sendsmsmail = false;
                        if (cbl_vendorname.Items.Count > 0)
                        {
                            for (int i = 0; i < cbl_vendorname.Items.Count; i++)
                            {
                                //Div1.Visible = true;
                                //bindchkselection();
                                if (cbl_vendorname.Items[i].Selected == true)
                                {
                                    string rc = txt_ref_id.Text;
                                    string[] Split = rc.Split('/');
                                    string venpk1 = Convert.ToString(cbl_vendorname.Items[i].Value);
                                    vensend = false;
                                    string reqcomcode = d2.GetFunction("select top(1) ReqCompCode from IT_VendorReq where isnull(itemorlibrary,'')='' order by VenReqCode desc");
                                    int comparecode = 0;
                                    if (reqcomcode.Trim() != "" && reqcomcode.Trim() != "0")
                                    {
                                        comparecode = Convert.ToInt32(reqcomcode);
                                        comparecode++;
                                    }
                                    else
                                    {
                                        comparecode = 1;
                                    }
                                    for (int row = 1; row < FpSpread1.Sheets[0].Rows.Count; row++)
                                    {
                                        string rqcode = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 0].Tag);
                                        string reqcode = Convert.ToString(txt_ref_id.Text);
                                        string venpk = Convert.ToString(cbl_vendorname.Items[i].Value);
                                        string itemfk = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 4].Tag);
                                        string redate = Convert.ToString(txt_reqdate.Text);
                                        string appqty = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 6].Text);
                                        if (reqcode.Trim() != "")
                                        {
                                            if (appqty.Trim() == "")
                                            {
                                                appqty = "0";
                                            }
                                            string[] split = redate.Split('/');
                                            DateTime dt = new DateTime();
                                            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                                            //string reqcomcode = d2.GetFunction("select top(1) ReqCompCode from IT_VendorReq order by VenReqCode desc");

                                            mobilenoemail = "";
                                            sendsms = "";
                                            sendsmsmail = false;
                                            if (rblsmsmail.SelectedIndex == 0 || rblsmsmail.SelectedIndex == 1)
                                            {
                                                sendsms = "1";
                                                string sms = Convert.ToString(textarea_smsemailcontent.InnerText);
                                                if (sms == "")
                                                {
                                                    lbl_alerterror.Visible = true;
                                                    lbl_alerterror.Text = "Please Enter The Sms Or Mail Content!.";
                                                    alertmessage.Visible = true;
                                                    return;
                                                }
                                            }
                                            else
                                                sendsms = "0";

                                            string venreq = "if exists(select*from IT_VendorReq where VenReqCode='" + reqcode + "' and vendorfk='" + venpk + "' and isnull(itemorlibrary,'')='' and ReqFK='" + rqcode + "') update IT_VendorReq set venreqdate='" + dt.ToString("MM/dd/yyyy") + "',VendorReqDueDate='',ReqFK='" + rqcode + "',VendorFK='" + venpk + "',ReqCompCode='" + comparecode + "',send_smsormail='" + sendsms + "' where  VenReqCode='" + reqcode + "' and vendorfk='" + venpk + "' and isnull(itemorlibrary,'')='' and ReqFK='" + rqcode + "' else INSERT INTO IT_VendorReq(VenReqCode,VenReqDate,VendorReqDueDate,ReqFK,VendorFK,ReqCompCode,PurchaseStatus,send_smsormail)VALUES('" + reqcode + "','" + dt.ToString("MM/dd/yyyy") + "','','" + rqcode + "','" + venpk + "','" + comparecode + "','0','" + sendsms + "')";
                                            venreqins = d2.update_method_wo_parameter(venreq, "Text");

                                            string reqfk = d2.GetFunction("select venreqpk from IT_VendorReq where VenReqCode='" + reqcode + "' and vendorfk='" + venpk + "' and isnull(itemorlibrary,'')='' and ReqFK='" + rqcode + "' ");
                                            string reqdetqry = "if exists(select*from IT_VendorReqDet where itemfk='" + itemfk + "' and venreqpk='" + reqfk + "' and ReqFK='" + rqcode + "' ) update IT_VendorReqDet set ItemFK='" + itemfk + "',ReqQty='" + appqty + "',ReqFK='" + rqcode + "' where venreqpk='" + reqfk + "' and ReqFK='" + rqcode + "' else INSERT INTO IT_VendorReqDet(ItemFK,ReqQty,VenReqPK,ReqFK)values('" + itemfk + "','" + appqty + "','" + reqfk + "','" + rqcode + "')";
                                            reqfkins = d2.update_method_wo_parameter(reqdetqry, "Text");


                                            if (reqfkins != 0 && venreqins != 0)
                                            {
                                                string ins = "update RQ_Requisition set VendorReq_Type='1' where RequisitionPK='" + rqcode + "'";
                                                int requisition = d2.update_method_wo_parameter(ins, "Text");
                                                vensend = true;
                                            }
                                        }
                                        else
                                        {
                                            lbl_alerterror.Visible = true;
                                            lbl_alerterror.Text = "Request Code is Empty.";
                                            alertmessage.Visible = true;
                                        }
                                        bindrequestcode();
                                        //Div1.Visible = true;
                                        //bindchkselection();
                                        //FpSpread4.Visible = true;
                                    }
                                    if (vensend)
                                    {
                                        collegename = d2.GetFunction("select collname from collinfo where college_code='" + collegecode1 + "'");

                                        //string Msg = "Dear Supplier," + "\n" + "We Have Requested Certain Items.Please make Quotation Entry" + "\n" + "By" + collegename;
                                        string Msg = Convert.ToString(textarea_smsemailcontent.InnerText);

                                        if (rblsmsmail.SelectedIndex == 0)
                                        {
                                            mobilenoemail = d2.GetFunction("select isnull(VendorMobileNo,'') from CO_VendorMaster where VendorPK='" + venpk1 + "'");
                                            if (mobilenoemail != "")
                                            {
                                                string user_id = string.Empty;
                                                string ssr = "select * from Track_Value where college_code='" + Convert.ToString(collegecode1) + "'";
                                                ds.Clear();
                                                ds = d2.select_method_wo_parameter(ssr, "Text");
                                                if (ds.Tables[0].Rows.Count > 0)
                                                {
                                                    user_id = Convert.ToString(ds.Tables[0].Rows[0]["SMS_User_ID"]);
                                                }

                                                int sndsms = d2.send_sms(user_id, collegecode1, usercode, mobilenoemail, Msg, "1");
                                                if (sndsms > 0)
                                                    sendsmsmail = true;
                                            }
                                        }
                                        if (rblsmsmail.SelectedIndex == 1)
                                        {
                                            mobilenoemail = d2.GetFunction("select isnull(VendorEmailID,'') from CO_VendorMaster where VendorPK='" + venpk1 + "'");
                                            if (mobilenoemail != "")
                                            {
                                                DataTable dtsendmail = new DataTable();
                                                string send_mail = string.Empty;
                                                string send_pw = string.Empty;
                                                string strquery = "select massemail,masspwd from collinfo where college_code ='" + collegecode1 + "' ";
                                                dtsendmail.Clear();
                                                dtsendmail = dirAcc.selectDataTable(strquery);
                                                {
                                                    send_mail = Convert.ToString(dtsendmail.Rows[0]["massemail"]);
                                                    send_pw = Convert.ToString(dtsendmail.Rows[0]["masspwd"]);

                                                    //send_mail = "palpaporange@gmail.com";
                                                    //send_pw = "palpap1234";
                                                }
                                                if (mobilenoemail != "")
                                                {
                                                    SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                                                    Mail.EnableSsl = true;
                                                    MailMessage mailmsg = new MailMessage();
                                                    MailAddress mfrom = new MailAddress(send_mail);
                                                    mailmsg.From = mfrom;
                                                    mailmsg.To.Add(mobilenoemail);
                                                    mailmsg.Subject = "Item Request For Supplier";
                                                    mailmsg.IsBodyHtml = true;
                                                    mailmsg.Body = Msg;
                                                    Mail.EnableSsl = true;
                                                    Mail.UseDefaultCredentials = false;
                                                    NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                                                    Mail.Credentials = credentials;
                                                    Mail.Send(mailmsg);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (reqfkins != 0 && venreqins != 0)
                        {
                            lbl_alerterror.Visible = true;
                            if (sendsmsmail)
                            {
                                if (rblsmsmail.SelectedIndex == 0)
                                    lbl_alerterror.Text = "Saved And Send SMS Successfully";
                                if (rblsmsmail.SelectedIndex == 1)
                                    lbl_alerterror.Text = "Saved And Send MAIL Successfully";

                            }
                            else
                                lbl_alerterror.Text = "Saved Successfully";


                            alertmessage.Visible = true;
                            btn_go_Click(sender, e);
                            btn_basescreen_Click(sender, e);

                        }
                    }
                    #endregion
                }
                if (RadioBook.Checked)
                {
                    #region BOOK
                    if (Showgrid.Rows.Count > 0)
                    {
                        int reqfkins = 0;
                        int venreqins = 0;
                        if (cbl_vendorname.Items.Count > 0)
                        {
                            for (int i = 0; i < cbl_vendorname.Items.Count; i++)
                            {
                                if (cbl_vendorname.Items[i].Selected == true)
                                {
                                    string rc = txt_reff_id.Text;
                                    string[] Split = rc.Split('/');
                                    string venpk1 = Convert.ToString(cbl_vendorname.Items[i].Value);
                                    vensend = false;
                                    string reqcomcode = d2.GetFunction("select top(1) ReqCompCode from IT_VendorReq where itemorlibrary='1' order by VenReqCode desc");
                                    int comparecode = 0;
                                    if (reqcomcode.Trim() != "" && reqcomcode.Trim() != "0")
                                    {
                                        comparecode = Convert.ToInt32(reqcomcode);
                                        comparecode++;
                                    }
                                    else
                                    {
                                        comparecode = 1;
                                    }
                                    for (int row = 0; row < Showgrid.Rows.Count; row++)
                                    {

                                        Label LabelReqPk = (Label)Showgrid.Rows[row].FindControl("LabelReqPk");
                                        string rqcode = LabelReqPk.Text;
                                        string reqcode = Convert.ToString(txt_reff_id.Text);
                                        string venpk = Convert.ToString(cbl_vendorname.Items[i].Value);
                                        Label lbl_bookid = (Label)Showgrid.Rows[row].FindControl("lbl_bookid");
                                        string bookid = lbl_bookid.Text;
                                        string redate = Convert.ToString(txtreqdate.Text);
                                        Label lbl_Approved = (Label)Showgrid.Rows[row].FindControl("lbl_Approved");
                                        string appqty = lbl_Approved.Text;

                                        if (reqcode.Trim() != "")
                                        {
                                            if (appqty.Trim() == "")
                                            {
                                                appqty = "0";
                                            }
                                            string[] split = redate.Split('/');
                                            DateTime dt = new DateTime();
                                            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);


                                            mobilenoemail = "";
                                            sendsms = "";
                                            if (rblsmsmail.SelectedIndex == 0 || rblsmsmail.SelectedIndex == 1)
                                            {
                                                sendsms = "1";
                                                string sms = Convert.ToString(textarea_smsemailcontent.InnerText);
                                                if (sms == "")
                                                {
                                                    lbl_alerterror.Visible = true;
                                                    lbl_alerterror.Text = "Please Enter The Sms Or Mail Content!.";
                                                    alertmessage.Visible = true;
                                                    return;
                                                }
                                            }
                                            else
                                                sendsms = "0";

                                            string venreq = "if exists(select*from IT_VendorReq where VenReqCode='" + reqcode + "' and vendorfk='" + venpk + "' and itemorlibrary='1' and ReqFK='" + rqcode + "' ) update IT_VendorReq set venreqdate='" + dt.ToString("MM/dd/yyyy") + "',VendorReqDueDate='',ReqFK='" + rqcode + "',VendorFK='" + venpk + "',ReqCompCode='" + comparecode + "',send_smsormail='" + sendsms + "',itemorlibrary='1' where  VenReqCode='" + reqcode + "' and vendorfk='" + venpk + "' and itemorlibrary='1' and ReqFK='" + rqcode + "'  else INSERT INTO IT_VendorReq(VenReqCode,VenReqDate,VendorReqDueDate,ReqFK,VendorFK,ReqCompCode,PurchaseStatus,send_smsormail,itemorlibrary)VALUES('" + reqcode + "','" + dt.ToString("MM/dd/yyyy") + "','','" + rqcode + "','" + venpk + "','" + comparecode + "','0','" + sendsms + "','1')";
                                            venreqins = d2.update_method_wo_parameter(venreq, "Text");

                                            string reqfk = d2.GetFunction("select venreqpk from IT_VendorReq where VenReqCode='" + reqcode + "' and vendorfk='" + venpk + "' and itemorlibrary='1' and ReqFK='" + rqcode + "' ");
                                            string reqdetqry = "if exists(select*from IT_VendorReqDet where itemfk='" + bookid + "' and venreqpk='" + reqfk + "' and ReqFK='" + rqcode + "'  ) update IT_VendorReqDet set ItemFK='" + bookid + "',ReqQty='" + appqty + "',ReqFK='" + rqcode + "' where venreqpk='" + reqfk + "' and ReqFK='" + rqcode + "'  else INSERT INTO IT_VendorReqDet(ItemFK,ReqQty,VenReqPK,ReqFK)values('" + bookid + "','" + appqty + "','" + reqfk + "','" + rqcode + "')";
                                            reqfkins = d2.update_method_wo_parameter(reqdetqry, "Text");
                                            if (reqfkins != 0 && venreqins != 0)
                                            {
                                                string ins = "update RQ_Requisition set VendorReq_Type='1' where RequisitionPK='" + rqcode + "'";
                                                int requisition = d2.update_method_wo_parameter(ins, "Text");

                                            }
                                        }
                                        else
                                        {
                                            lbl_alerterror.Visible = true;
                                            lbl_alerterror.Text = "Request Code is Empty.";
                                            alertmessage.Visible = true;
                                        }
                                        bindbookrequestcode();

                                    }

                                    if (vensend)
                                    {
                                        collegename = d2.GetFunction("select collname from collinfo where college_code='" + collegecode1 + "'");
                                        //string Msg = "Dear Supplier," + "\n" + "We Have Requested Certain Books.Please make Quotation Entry" + "\n" + "By" + collegename;
                                        string Msg = Convert.ToString(textarea_smsemailcontent.InnerText);

                                        if (rblsmsmail.SelectedIndex == 0)
                                        {
                                            mobilenoemail = d2.GetFunction("select isnull(VendorMobileNo,'') from CO_VendorMaster where VendorPK='" + venpk1 + "' and LibraryFlag='1'");
                                            if (mobilenoemail != "")
                                            {
                                                string user_id = string.Empty;
                                                string ssr = "select * from Track_Value where college_code='" + Convert.ToString(collegecode1) + "'";
                                                ds.Clear();
                                                ds = d2.select_method_wo_parameter(ssr, "Text");
                                                if (ds.Tables[0].Rows.Count > 0)
                                                {
                                                    user_id = Convert.ToString(ds.Tables[0].Rows[0]["SMS_User_ID"]);
                                                }

                                                int sndsms = d2.send_sms(user_id, collegecode1, usercode, mobilenoemail, Msg, "1");
                                                if (sndsms > 0)
                                                    sendsmsmail = true;
                                            }
                                        }
                                        if (rblsmsmail.SelectedIndex == 1)
                                        {
                                            mobilenoemail = d2.GetFunction("select isnull(VendorEmailID,'') from CO_VendorMaster where VendorPK='" + venpk1 + "' LibraryFlag='1'");
                                            if (mobilenoemail != "")
                                            {
                                                DataTable dtsendmail = new DataTable();
                                                string send_mail = string.Empty;
                                                string send_pw = string.Empty;
                                                string strquery = "select massemail,masspwd from collinfo where college_code ='" + collegecode1 + "' ";
                                                dtsendmail.Clear();
                                                dtsendmail = dirAcc.selectDataTable(strquery);
                                                {
                                                    send_mail = Convert.ToString(dtsendmail.Rows[0]["massemail"]);
                                                    send_pw = Convert.ToString(dtsendmail.Rows[0]["masspwd"]);
                                                    //send_mail = "palpaporange@gmail.com";
                                                    //send_pw = "palpap1234";
                                                }
                                                if (mobilenoemail != "")
                                                {
                                                    SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                                                    Mail.EnableSsl = true;
                                                    MailMessage mailmsg = new MailMessage();
                                                    MailAddress mfrom = new MailAddress(send_mail);
                                                    mailmsg.From = mfrom;
                                                    mailmsg.To.Add(mobilenoemail);
                                                    mailmsg.Subject = "Book Request For Supplier";
                                                    mailmsg.IsBodyHtml = true;
                                                    mailmsg.Body = Msg;
                                                    Mail.EnableSsl = true;
                                                    Mail.UseDefaultCredentials = false;
                                                    NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                                                    Mail.Credentials = credentials;
                                                    Mail.Send(mailmsg);
                                                    sendsmsmail = true;
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                        if (reqfkins != 0 && venreqins != 0)
                        {
                            lbl_alerterror.Visible = true;
                            if (sendsmsmail)
                            {
                                if (rblsmsmail.SelectedIndex == 0)
                                    lbl_alerterror.Text = "Saved And Send SMS Successfully";
                                if (rblsmsmail.SelectedIndex == 1)
                                    lbl_alerterror.Text = "Saved And Send MAIL Successfully";

                            }
                            else
                                lbl_alerterror.Text = "Saved Successfully";
                            alertmessage.Visible = true;
                            btn_book_request_Go_Click(sender, e);
                            btn_basescreen_Click(sender, e);

                        }
                    }
                    #endregion
                }
            }
            if (rdb_suggested.Checked == true)
            {
                if (Radioitem.Checked)
                {
                    #region ITEM

                    if (FpSpread1.Sheets[0].Rows.Count > 0)
                    {
                        int reqfkins = 0;
                        int venreqins = 0;
                        sendsmsmail = false;
                        if (cbl_vendorname.Items.Count > 0)
                        {
                            for (int i = 0; i < cbl_vendorname.Items.Count; i++)
                            {
                                if (cbl_vendorname.Items[i].Selected == true)
                                {
                                    string venpk1 = Convert.ToString(cbl_vendorname.Items[i].Value);
                                    vensend = false;
                                    string reqcomcode = d2.GetFunction("select top(1) ReqCompCode from IT_VendorReq where isnull(itemorlibrary,'')='' order by VenReqPK desc");
                                    int comparecode = 0;
                                    if (reqcomcode.Trim() != "" && reqcomcode.Trim() != "0")
                                    {
                                        comparecode = Convert.ToInt32(reqcomcode);
                                        comparecode++;
                                    }
                                    else
                                    {
                                        comparecode = 1;
                                    }
                                    for (int row = 1; row < FpSpread1.Sheets[0].RowCount; row++)
                                    {
                                        int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, 1].Value);
                                        if (checkval == 1)
                                        {
                                            //Div1.Visible = true;
                                            //bindchkselection();
                                            string rc = txt_ref_id.Text;
                                            string[] Split = rc.Split('/');

                                            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["RequisitionPK"]);
                                            // string purchaseot = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                                            string rqcode = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 0].Tag);
                                            string reqcode = Convert.ToString(txt_ref_id.Text);
                                            string venpk = Convert.ToString(cbl_vendorname.Items[i].Value);
                                            string itemfk = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 4].Tag);
                                            string redate = Convert.ToString(txt_reqdate.Text);
                                            string appqty = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 6].Text);
                                            if (appqty.Trim() == "")
                                            {
                                                appqty = "0";
                                            }

                                            string[] split = redate.Split('/');
                                            DateTime dt = new DateTime();
                                            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                                            //string reqcomcode = d2.GetFunction("select top(1) ReqCompCode from IT_VendorReq order by VenReqCode desc");

                                            sendsmsmail = false;
                                            mobilenoemail = "";
                                            sendsms = "";
                                            if (rblsmsmail.SelectedIndex == 0 || rblsmsmail.SelectedIndex == 1)
                                            {
                                                sendsms = "1";
                                                string sms = Convert.ToString(textarea_smsemailcontent.InnerText);
                                                if (sms=="")
                                                {
                                                    lbl_alerterror.Visible = true;
                                                    lbl_alerterror.Text = "Please Enter The Sms Or Mail Content!.";
                                                    alertmessage.Visible = true;
                                                    return;
                                                }
                                            }
                                            else
                                                sendsms = "0";
                                            string venreq = "if exists(select*from IT_VendorReq where VenReqCode='" + reqcode + "' and vendorfk='" + venpk + "' and isnull(itemorlibrary,'')='' and ReqFK='" + rqcode + "' ) update IT_VendorReq set venreqdate='" + dt.ToString("MM/dd/yyyy") + "',VendorReqDueDate='',ReqFK='" + rqcode + "',VendorFK='" + venpk + "',ReqCompCode='" + comparecode + "',send_smsormail='" + sendsms + "' where  VenReqCode='" + reqcode + "' and vendorfk='" + venpk + "' and isnull(itemorlibrary,'')='' and ReqFK='" + rqcode + "'  else INSERT INTO IT_VendorReq(VenReqCode,VenReqDate,VendorReqDueDate,ReqFK,VendorFK,ReqCompCode,PurchaseStatus,send_smsormail)VALUES('" + reqcode + "','" + dt.ToString("MM/dd/yyyy") + "','','" + rqcode + "','" + venpk + "','" + comparecode + "','0','" + sendsms + "')";
                                            venreqins = d2.update_method_wo_parameter(venreq, "Text");

                                            string reqfk = d2.GetFunction("select venreqpk from IT_VendorReq where VenReqCode='" + reqcode + "' and vendorfk='" + venpk + "' and  isnull(itemorlibrary,'')='' and ReqFK='" + rqcode + "' ");
                                            string reqdetqry = "if exists(select*from IT_VendorReqDet where itemfk='" + itemfk + "' and venreqpk='" + reqfk + "' and ReqFK='" + rqcode + "'  ) update IT_VendorReqDet set ItemFK='" + itemfk + "',ReqQty='" + appqty + "',ReqFK='" + rqcode + "' where venreqpk='" + reqfk + "' and ReqFK='" + rqcode + "'  else INSERT INTO IT_VendorReqDet(ItemFK,ReqQty,VenReqPK,ReqFK)values('" + itemfk + "','" + appqty + "','" + reqfk + "','" + rqcode + "')";
                                            reqfkins = d2.update_method_wo_parameter(reqdetqry, "Text");


                                            if (reqfkins != 0 && venreqins != 0)
                                            {
                                                string ins = "update RQ_Requisition set VendorReq_Type='1' where RequisitionPK='" + rqcode + "'";
                                                int requisition = d2.update_method_wo_parameter(ins, "Text");
                                                vensend = true;

                                            }
                                        }
                                    }
                                    if (vensend)
                                    {
                                        collegename = d2.GetFunction("select collname from collinfo where college_code='" + collegecode1 + "'");

                                        //string Msg = "Dear Supplier," + "\n" + "We Have Requested Certain Items.Please make Quotation Entry" + "\n" + "By " + collegename;
                                        string Msg = Convert.ToString(textarea_smsemailcontent.InnerText);

                                        if (rblsmsmail.SelectedIndex == 0)
                                        {
                                            mobilenoemail = d2.GetFunction("select isnull(VendorMobileNo,'') from CO_VendorMaster where VendorPK='" + venpk1 + "'");

                                            if (mobilenoemail != "")
                                            {
                                                string user_id = string.Empty;
                                                string ssr = "select * from Track_Value where college_code='" + Convert.ToString(collegecode1) + "'";
                                                ds.Clear();
                                                ds = d2.select_method_wo_parameter(ssr, "Text");
                                                if (ds.Tables[0].Rows.Count > 0)
                                                {
                                                    user_id = Convert.ToString(ds.Tables[0].Rows[0]["SMS_User_ID"]);
                                                }

                                                int sndsms = d2.send_sms(user_id, collegecode1, usercode, mobilenoemail, Msg, "1");
                                                if (sndsms > 0)
                                                    sendsmsmail = true;
                                            }
                                        }
                                        if (rblsmsmail.SelectedIndex == 1)
                                        {
                                            mobilenoemail = d2.GetFunction("select isnull(VendorEmailID,'') from CO_VendorMaster where VendorPK='" + venpk1 + "'");
                                            DataTable dtsendmail = new DataTable();
                                            string send_mail = string.Empty;
                                            string send_pw = string.Empty;
                                            string strquery = "select massemail,masspwd from collinfo where college_code ='" + collegecode1 + "' ";
                                            dtsendmail.Clear();
                                            dtsendmail = dirAcc.selectDataTable(strquery);
                                            {
                                                send_mail = Convert.ToString(dtsendmail.Rows[0]["massemail"]);
                                                send_pw = Convert.ToString(dtsendmail.Rows[0]["masspwd"]);
                                                //send_mail = "palpaporange@gmail.com";
                                                //send_pw = "palpap1234";
                                            }
                                            if (mobilenoemail != "")
                                            {
                                                SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                                                Mail.EnableSsl = true;
                                                MailMessage mailmsg = new MailMessage();
                                                MailAddress mfrom = new MailAddress(send_mail);
                                                mailmsg.From = mfrom;
                                                mailmsg.To.Add(mobilenoemail);
                                                mailmsg.Subject = "Item Request For Supplier";
                                                mailmsg.IsBodyHtml = true;
                                                mailmsg.Body = Msg;
                                                Mail.EnableSsl = true;
                                                Mail.UseDefaultCredentials = false;
                                                NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                                                Mail.Credentials = credentials;
                                                Mail.Send(mailmsg);

                                                sendsmsmail = true;
                                            }

                                        }

                                    }
                                }
                                bindrequestcode();
                            }
                            if (reqfkins != 0 && venreqins != 0)
                            {
                                lbl_alerterror.Visible = true;
                                if (sendsmsmail)
                                {
                                    if (rblsmsmail.SelectedIndex == 0)
                                        lbl_alerterror.Text = "Saved And Send SMS Successfully";
                                    if (rblsmsmail.SelectedIndex == 1)
                                        lbl_alerterror.Text = "Saved And Send MAIL Successfully";

                                }
                                else
                                    lbl_alerterror.Text = "Saved Successfully";
                                alertmessage.Visible = true;
                                btn_go_Click(sender, e);
                                btn_basescreen_Click(sender, e);
                            }
                        }
                    }

                    #endregion

                }

                if (RadioBook.Checked)
                {
                    #region BOOK

                    if (Showgrid.Rows.Count > 0)
                    {
                        int reqfkins = 0;
                        int venreqins = 0;
                        sendsmsmail = false;
                        if (cbl_vendorname.Items.Count > 0)
                        {
                            for (int i = 0; i < cbl_vendorname.Items.Count; i++)
                            {
                                if (cbl_vendorname.Items[i].Selected == true)
                                {
                                    string rc = txt_reff_id.Text;
                                    string[] Split = rc.Split('/');
                                    string venpk1 = Convert.ToString(cbl_vendorname.Items[i].Value);
                                    vensend = false;
                                    string reqcomcode = d2.GetFunction("select top(1) ReqCompCode from IT_VendorReq where itemorlibrary='1' order by VenReqPK desc");
                                    int comparecode = 0;
                                    if (reqcomcode.Trim() != "" && reqcomcode.Trim() != "0")
                                    {
                                        comparecode = Convert.ToInt32(reqcomcode);
                                        comparecode++;
                                    }
                                    else
                                    {
                                        comparecode = 1;
                                    }
                                    for (int row = 0; row < Showgrid.Rows.Count; row++)
                                    {
                                        System.Web.UI.WebControls.CheckBox checkval = (System.Web.UI.WebControls.CheckBox)Showgrid.Rows[row].FindControl("selectchk");

                                        if (checkval.Checked)
                                        {
                                            Label LabelReqPk = (Label)Showgrid.Rows[row].FindControl("LabelReqPk");
                                            string rqcode = LabelReqPk.Text;
                                            string reqcode = Convert.ToString(txt_reff_id.Text);
                                            string venpk = Convert.ToString(cbl_vendorname.Items[i].Value);
                                            Label lbl_bookid = (Label)Showgrid.Rows[row].FindControl("lbl_bookid");
                                            string bookid = lbl_bookid.Text;
                                            string redate = Convert.ToString(txtreqdate.Text);
                                            Label lbl_Approved = (Label)Showgrid.Rows[row].FindControl("lbl_Approved");
                                            string appqty = lbl_Approved.Text;
                                            if (appqty.Trim() == "")
                                            {
                                                appqty = "0";
                                            }

                                            string[] split = redate.Split('/');
                                            DateTime dt = new DateTime();
                                            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);


                                            sendsmsmail = false;
                                            mobilenoemail = "";
                                            sendsms = "";
                                            if (rblsmsmail.SelectedIndex == 0 || rblsmsmail.SelectedIndex == 1)
                                            {
                                                sendsms = "1";
                                                string sms = Convert.ToString(textarea_smsemailcontent.InnerText);
                                                if (sms == "")
                                                {
                                                    lbl_alerterror.Visible = true;
                                                    lbl_alerterror.Text = "Please Enter The Sms Or Mail Content!.";
                                                    alertmessage.Visible = true;
                                                    return;
                                                }
                                            }
                                            else
                                                sendsms = "0";
                                            string venreq = "if exists(select*from IT_VendorReq where VenReqCode='" + reqcode + "' and vendorfk='" + venpk + "' and itemorlibrary='1' and ReqFK='" + rqcode + "' ) update IT_VendorReq set venreqdate='" + dt.ToString("MM/dd/yyyy") + "',VendorReqDueDate='',ReqFK='" + rqcode + "',VendorFK='" + venpk + "',ReqCompCode='" + comparecode + "',send_smsormail='" + sendsms + "' where  VenReqCode='" + reqcode + "' and vendorfk='" + venpk + "' and itemorlibrary='1' and ReqFK='" + rqcode + "' else INSERT INTO IT_VendorReq(VenReqCode,VenReqDate,VendorReqDueDate,ReqFK,VendorFK,ReqCompCode,PurchaseStatus,send_smsormail,itemorlibrary)VALUES('" + reqcode + "','" + dt.ToString("MM/dd/yyyy") + "','','" + rqcode + "','" + venpk + "','" + comparecode + "','0','" + sendsms + "','1')";
                                            venreqins = d2.update_method_wo_parameter(venreq, "Text");

                                            string reqfk = d2.GetFunction("select venreqpk from IT_VendorReq where VenReqCode='" + reqcode + "' and vendorfk='" + venpk + "' and itemorlibrary='1' and ReqFK='" + rqcode + "' ");
                                            string reqdetqry = "if exists(select*from IT_VendorReqDet where itemfk='" + bookid + "' and venreqpk='" + reqfk + "' and ReqFK='" + rqcode + "' ) update IT_VendorReqDet set ItemFK='" + bookid + "',ReqQty='" + appqty + "',ReqFK='" + rqcode + "' where venreqpk='" + reqfk + "' and ReqFK='" + rqcode + "'  else INSERT INTO IT_VendorReqDet(ItemFK,ReqQty,VenReqPK,ReqFK)values('" + bookid + "','" + appqty + "','" + reqfk + "','" + rqcode + "')";
                                            reqfkins = d2.update_method_wo_parameter(reqdetqry, "Text");


                                            if (reqfkins != 0 && venreqins != 0)
                                            {
                                                string ins = "update RQ_Requisition set VendorReq_Type='1' where RequisitionPK='" + rqcode + "'";
                                                int requisition = d2.update_method_wo_parameter(ins, "Text");

                                                vensend = true;
                                            }
                                        }
                                    }
                                    if (vensend)
                                    {
                                        collegename = d2.GetFunction("select collname from collinfo where college_code='" + collegecode1 + "'");

                                        //string Msg = "Dear Supplier," + "\n" + "We Have Requested Certain Books.Please make Quotation Entry" + "\n" + "By" + collegename;
                                        string Msg = Convert.ToString(textarea_smsemailcontent.InnerText);


                                        if (rblsmsmail.SelectedIndex == 0)
                                        {
                                            mobilenoemail = d2.GetFunction("select isnull(VendorMobileNo,'') from CO_VendorMaster where VendorPK='" + venpk1 + "'");
                                            if (mobilenoemail != "")
                                            {
                                                string user_id = string.Empty;
                                                string ssr = "select * from Track_Value where college_code='" + Convert.ToString(collegecode1) + "'";
                                                ds.Clear();
                                                ds = d2.select_method_wo_parameter(ssr, "Text");
                                                if (ds.Tables[0].Rows.Count > 0)
                                                {
                                                    user_id = Convert.ToString(ds.Tables[0].Rows[0]["SMS_User_ID"]);
                                                }

                                                int sndsms = d2.send_sms(user_id, collegecode1, usercode, mobilenoemail, Msg, "1");
                                                if (sndsms > 0)
                                                    sendsmsmail = true;
                                            }
                                        }
                                        if (rblsmsmail.SelectedIndex == 1)
                                        {
                                            mobilenoemail = d2.GetFunction("select isnull(VendorEmailID,'') from CO_VendorMaster where VendorPK='" + venpk1 + "'");
                                            DataTable dtsendmail = new DataTable();
                                            string send_mail = string.Empty;
                                            string send_pw = string.Empty;
                                            string strquery = "select massemail,masspwd from collinfo where college_code ='" + collegecode1 + "' ";
                                            dtsendmail.Clear();
                                            dtsendmail = dirAcc.selectDataTable(strquery);
                                            {
                                                send_mail = Convert.ToString(dtsendmail.Rows[0]["massemail"]);
                                                send_pw = Convert.ToString(dtsendmail.Rows[0]["masspwd"]);
                                                //send_mail = "palpaporange@gmail.com";
                                                //send_pw = "palpap1234";
                                            }
                                            if (mobilenoemail != "")
                                            {
                                                SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                                                Mail.EnableSsl = true;
                                                MailMessage mailmsg = new MailMessage();
                                                MailAddress mfrom = new MailAddress(send_mail);
                                                mailmsg.From = mfrom;
                                                mailmsg.To.Add(mobilenoemail);
                                                mailmsg.Subject = "Book Request For Supplier";
                                                mailmsg.IsBodyHtml = true;
                                                mailmsg.Body = Msg;
                                                Mail.EnableSsl = true;
                                                Mail.UseDefaultCredentials = false;
                                                NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                                                Mail.Credentials = credentials;
                                                Mail.Send(mailmsg);

                                                sendsmsmail = true;
                                            }

                                        }

                                    }

                                }
                                bindrequestcode();
                            }
                            if (reqfkins != 0 && venreqins != 0)
                            {
                                lbl_alerterror.Visible = true;
                                if (sendsmsmail)
                                {
                                    if (rblsmsmail.SelectedIndex == 0)
                                        lbl_alerterror.Text = "Saved And Send SMS Successfully";
                                    if (rblsmsmail.SelectedIndex == 1)
                                        lbl_alerterror.Text = "Saved And Send MAIL Successfully";

                                }
                                else
                                    lbl_alerterror.Text = "Saved Successfully";
                                alertmessage.Visible = true;
                                btn_go_Click(sender, e);
                                btn_basescreen_Click(sender, e);
                            }
                        }
                    }
                    #endregion
                }
            }
            if (Radioitem.Checked)
            {
                bindvenbase();
                Rad_item.Checked = true;
            }
            if (RadioBook.Checked)
            {
                bindvenbase1();
                Rad_book.Checked = true;
            }
        }
        catch
        {
        }
    }

    public void btnDelete_Click(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string actrow = e.SheetView.ActiveRow.ToString();
        string actcol = e.SheetView.ActiveColumn.ToString();
        if (actrow.Trim() != "" && actcol.Trim() != "")
        {
            string VenReqPK = Convert.ToString(FpSpread5.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Tag);
            string itemfk = Convert.ToString(FpSpread5.Sheets[0].Cells[Convert.ToInt32(actrow), 5].Tag);
            // delete IT_VendorReq where VenReqPK=''
            string del = "delete IT_VendorReqDet where VenReqPK='" + VenReqPK + "' and ItemFK='" + itemfk + "'";
            int delete = d2.update_method_wo_parameter(del, "text");
            if (delete != 0)
            {
                alertmessage.Visible = true;
                lbl_alerterror.Visible = true;
                lbl_alerterror.Text = "Deleted Successfully";
                btn_basescreen_Click(sender, e);
            }
        }
    }
    public void bindchkselection()
    {
        try
        {

            int row = 1;
            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["itemfk"]);
            string itemfk = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 4].Tag);
            string chkquery = "SELECT distinct VendorCompName,VenItemSupplyDur,VenItemIsSupplied,VenItemReference FROM CO_VendorMaster V,IM_VendorItemDept D where v.VendorPK = d.VenItemFK and ItemFK ='" + itemfk + "'";

            ds.Clear();
            ds = d2.select_method_wo_parameter(chkquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                FpSpread4.Sheets[0].RowCount = 0;
                FpSpread4.Sheets[0].ColumnCount = 0;
                FpSpread4.CommandBar.Visible = false;
                FpSpread4.Sheets[0].AutoPostBack = false;
                FpSpread4.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread4.Sheets[0].RowHeader.Visible = false;
                FpSpread4.Sheets[0].ColumnCount = 7;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                FpSpread4.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread4.Columns[0].Width = 50;

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Text = " Select";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread4.Columns[1].Width = 80;
                //FpSpread4.Columns[1].Visible = false;

                //FarPoint.Web.Spread.CheckBoxCellType chkall1 = new FarPoint.Web.Spread.CheckBoxCellType();
                //chkall1.AutoPostBack = true;

                //FpSpread4.Sheets[0].Cells[0, 1].CellType = chkall1;
                //FpSpread4.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                //chkall1.Text = " ";

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Vendor Name";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread4.Columns[2].Width = 200;

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item Can Supply ";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpread4.Columns[3].Width = 100;

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Already Supplied Quantity";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                FpSpread4.Columns[4].Width = 100;

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Delivered Status";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                FpSpread4.Columns[5].Width = 200;


                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Remarks";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                FpSpread4.Columns[6].Width = 200;

                FarPoint.Web.Spread.CheckBoxCellType chkbox = new FarPoint.Web.Spread.CheckBoxCellType();
                chkbox.AutoPostBack = false;
                chkbox.Text = " ";

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    FpSpread4.Sheets[0].RowCount++;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 1].CellType = chkbox;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["VenItemSupplyDur"]);
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["VenItemIsSupplied"]);
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 5].Text = "";
                    //FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[i]["MessId"]);

                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["VenItemReference"]);
                    //FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[i]["MessId"]);

                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                }
                FpSpread4.Sheets[0].PageSize = FpSpread4.Sheets[0].RowCount;
                FpSpread4.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread4.Sheets[0].FrozenRowCount = 0;
                FpSpread4.Visible = true;

                lbl_error.Visible = false;
                //rptprint.Visible = true;
            }
            else
            {
                lbl_error.Visible = true;
                FpSpread4.Visible = false;
                lbl_error.Text = "No Record Found";
                //rptprint.Visible = false;
            }
            //else
            //{
            //  lbl_error.Visible = true;
            //  FpSpread2.Visible = false;
            //  spreaddiv.Visible = false;
            //  rptprint.Visible = false;
            //  lbl_error.Text = "Please Select All Fields";
            //}
        }
        catch
        {


        }

    }
    public void bindrequestcode()
    {
        try
        {

            string newitemcode = "";

            //string selectquery = "select Requestcode,RequestType from RQ_Requisition";
            string selectquery = "select VenReqAcr,VenReqStNo,VenReqSize  from IM_CodeSettings order by startdate desc";
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string ordcode = Convert.ToString(ds.Tables[0].Rows[0]["VenReqAcr"]);
                string itemacronym = Convert.ToString(ds.Tables[0].Rows[0]["VenReqAcr"]);
                string itemstarno = Convert.ToString(ds.Tables[0].Rows[0]["VenReqStNo"]);
                string itemsize = Convert.ToString(ds.Tables[0].Rows[0]["VenReqSize"]);
                //selectquery = "select distinct top (1) order_code  from purchase_order where order_code like '" + Convert.ToString(itemacronym) + "%' order by order_code desc";
                //selectquery = "select distinct top (1) VenReqCode  from IT_VendorReq where VenReqCode like '" + Convert.ToString(ordcode) + "[0-9]%' order by VenReqCode desc";
                selectquery = "select distinct top (1) venreqpk,VenReqCode  from IT_VendorReq where isnull(itemorlibrary,'')=''  order by venreqpk desc";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string itemcode = Convert.ToString(ds.Tables[0].Rows[0]["VenReqCode"]);
                    string itemacr = Convert.ToString(itemacronym);
                    int len = itemacr.Length;
                    itemcode = itemcode.Remove(0, len);
                    int len1 = Convert.ToString(itemcode).Length;
                    string newnumber = Convert.ToString((Convert.ToInt32(itemcode) + 1));
                    len = Convert.ToString(newnumber).Length;
                    //len1 = len1 - len;

                    len1 = Convert.ToInt32(itemsize) - len;//3.11.15
                    if (len1 == 2)
                    {
                        newitemcode = "00" + newnumber;
                    }
                    else if (len1 == 1)
                    {
                        newitemcode = "0" + newnumber;
                    }
                    else if (len1 == 4)
                    {
                        newitemcode = "0000" + newnumber;
                    }
                    else if (len1 == 3)
                    {
                        newitemcode = "000" + newnumber;
                    }
                    else
                    {
                        newitemcode = Convert.ToString(newnumber);
                    }
                    if (newitemcode.Trim() != "")
                    {
                        newitemcode = itemacr + "" + newitemcode;
                    }
                }
                else
                {
                    string itemacr = Convert.ToString(itemstarno);
                    int len = itemacr.Length;

                    string items = Convert.ToString(itemsize);
                    int len1 = Convert.ToInt32(items);//items.Length;

                    int size = len1 - len;//Convert.ToInt32(itemacr);

                    if (size == 2)
                    {
                        newitemcode = "00" + itemstarno;
                    }
                    else if (size == 1)
                    {
                        newitemcode = "0" + itemstarno;
                    }
                    else if (size == 4)
                    {
                        newitemcode = "0000" + itemstarno;
                    }
                    else if (size == 3)
                    {
                        newitemcode = "000" + itemstarno;
                    }
                    else
                    {
                        newitemcode = Convert.ToString(itemstarno);
                    }
                    newitemcode = Convert.ToString(itemacronym) + "" + Convert.ToString(newitemcode);

                    // newitemcode = Convert.ToString(itemacronym) + "" + Convert.ToString(itemstarno);
                }
                txt_ref_id.Text = Convert.ToString(newitemcode);
            }
        }
        catch
        {

        }
    }
    //30.12.15
    protected void cb_venname_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;
        txt_venname.Text = "--Select--";

        if (cb_venname.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_venname.Items.Count; i++)
            {
                cbl_venname.Items[i].Selected = true;
            }
            txt_venname.Text = "Vendor Name(" + (cbl_venname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_venname.Items.Count; i++)
            {
                cbl_venname.Items[i].Selected = false;
            }
        }
    }
    protected void cbl_venname_SelectedIndexChange(object sender, EventArgs e)
    {
        int i = 0;
        cb_venname.Checked = false;
        int commcount = 0;
        txt_venname.Text = "--Select--";
        for (i = 0; i < cbl_venname.Items.Count; i++)
        {
            if (cbl_venname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_venname.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_venname.Items.Count)
            {
                cb_venname.Checked = true;
            }
            txt_venname.Text = "Vendor Name(" + commcount.ToString() + ")";
        }
    }
    protected void btn_baseaddnew_Click(object sender, EventArgs e)
    {
        popdiv.Visible = true;
        Printcontrol.Visible = false;

        if (Rad_item.Checked)
        {
            Radioitem.Checked = true;
            Radioitem_Click(sender, e);
        }
        if (Rad_book.Checked)
        {
            RadioBook.Checked = true;
            RadioBook_Click(sender, e);
        }
    }
    protected void btn_basescreen_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            if (txt_venname.Text.Trim() != "--Select--")
            {
                string vencode = "";
                for (int i = 0; i < cbl_venname.Items.Count; i++)
                {
                    if (cbl_venname.Items[i].Selected == true)
                    {
                        if (vencode == "")
                        {
                            vencode = "" + cbl_venname.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            vencode = vencode + "'" + "," + "'" + cbl_venname.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string requestquery = "";
                if (Rad_item.Checked)
                    requestquery = " select distinct vd.ItemFK,r.Requestcode,vr.VenReqCode,vm.VendorCode,vm.VendorCompName,i.ItemName,vr.ReqFK,vr.VenReqPK from IT_VendorReq vr,IT_VendorReqDet vd,RQ_Requisition r,CO_VendorMaster vm,IM_ItemMaster i where vr.VenReqPK=vd.VenReqPK and vr.ReqFK=vd.ReqFK and r.RequisitionPK=vd.ReqFK and r.RequestType ='1' and vm.VendorPK =vr.VendorFK and  i.ItemPK=vd.ItemFK and vm.VendorCode in('" + vencode + "') order by VenReqCode ";
                if (Rad_book.Checked)
                    requestquery = " select distinct vd.ItemFK,r.Requestcode,vr.VenReqCode,vm.VendorCode,vm.VendorCompName,bd.Title,vr.ReqFK,vr.VenReqPK from IT_VendorReq vr,IT_VendorReqDet vd,RQ_Requisition r,CO_VendorMaster vm,bookdetails bd where vr.VenReqPK=vd.VenReqPK and vr.ReqFK=vd.ReqFK and r.RequisitionPK=vd.ReqFK and r.RequestType ='13' and vm.VendorPK =vr.VendorFK and  bd.BookID=vd.ItemFK and vm.VendorCode in('" + vencode + "')  order by VenReqCode";
                ds.Clear();
                ds = d2.select_method_wo_parameter(requestquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    FpSpread5.Sheets[0].RowCount = 0;
                    FpSpread5.Sheets[0].ColumnCount = 0;
                    FpSpread5.CommandBar.Visible = false;
                    FpSpread5.Sheets[0].AutoPostBack = false;
                    FpSpread5.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread5.Sheets[0].RowHeader.Visible = false;
                    FpSpread5.Sheets[0].ColumnCount = 7;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    FpSpread5.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread5.Columns[0].Width = 50;
                    FpSpread5.Columns[0].Locked = true;



                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Request Code";
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread5.Columns[1].Width = 100;
                    FpSpread5.Columns[1].Locked = true;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Vendor Request Code";
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread5.Columns[2].Width = 150;
                    FpSpread5.Columns[2].Locked = true;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Vendor Code";
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread5.Columns[3].Width = 150;
                    FpSpread5.Columns[3].Locked = true;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Vendor Name";
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    FpSpread5.Columns[4].Width = 200;
                    FpSpread5.Columns[4].Locked = true;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Item Name";
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    FpSpread5.Columns[5].Width = 150;
                    FpSpread5.Columns[5].Locked = true;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Select";
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    FpSpread5.Columns[6].Width = 50;
                    // FpSpread5.Columns[6].Locked = true;
                    FarPoint.Web.Spread.ButtonCellType btnType = new FarPoint.Web.Spread.ButtonCellType();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FpSpread5.Sheets[0].RowCount++;
                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["Requestcode"]);
                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["ReqFK"]);
                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["VenReqCode"]);
                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["VenReqPK"]);
                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCode"]);
                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        if (Rad_item.Checked)
                            FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);
                        if (Rad_book.Checked)
                            FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[i]["ItemFK"]);
                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";


                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 6].CellType = btnType;
                        btnType.Text = "Delete";
                        btnType.CssClass = "textbox btn2";
                        btnType.ForeColor = Color.Blue;
                        //FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        //FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        //FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                    }
                    FpSpread5.Sheets[0].PageSize = FpSpread5.Sheets[0].RowCount;
                    FpSpread5.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread5.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread5.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread5.Columns[1].VerticalAlign = VerticalAlign.Middle;

                    FpSpread5.Sheets[0].FrozenRowCount = 0;
                    FpSpread5.Visible = true;
                    spreaddiv1.Visible = true;
                    lbl_error.Visible = false;
                    rptprint.Visible = true;
                    lbl_baseerror.Visible = false;
                }
                else
                {
                    FpSpread5.Visible = false;
                    spreaddiv1.Visible = false;
                    lbl_baseerror.Visible = true;
                    rptprint.Visible = false;
                    lbl_baseerror.Text = "No Record Founds";
                }
            }
            else
            {
                FpSpread5.Visible = false;
                spreaddiv1.Visible = false;
                rptprint.Visible = false;
                lbl_baseerror.Visible = true;
                lbl_baseerror.Text = "Please Select Vendor Name";
            }
        }
        catch { }
    }


    //Added by saranyadevi19.1.2018
    protected void Rad_item_Click(object sender, EventArgs e)
    {
        bindvenbase();
        btn_basescreen_Click(sender, e);

    }
    protected void Rad_book_Click(object sender, EventArgs e)
    {
        bindvenbase1();
        btn_basescreen_Click(sender, e);


    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread5, reportname);
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
            string degreedetails = "Vendor Request";
            string pagename = "request_vendor.aspx";
            Printcontrol.loadspreaddetails(FpSpread5, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }

    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popdiv.Visible = false;
    }
    protected void bindvenbase()
    {
        try
        {
            int i = 0;
            cbl_venname.Items.Clear();
            string q = "select distinct vm.VendorCode,vm.VendorCompName from CO_VendorMaster vm, IT_VendorReq vr where vm.VendorPK=vr.VendorFK and isnull(LibraryFlag,'')=''";
            ds.Clear();
            ds = d2.select_method_wo_parameter(q, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_venname.DataSource = ds;
                cbl_venname.DataTextField = "VendorCompName";
                cbl_venname.DataValueField = "VendorCode";
                cbl_venname.DataBind();
                if (cbl_venname.Items.Count > 0)
                {
                    for (i = 0; i < cbl_venname.Items.Count; i++)
                    {
                        cbl_venname.Items[i].Selected = true;
                    }
                    txt_venname.Text = "Vendor Name(" + cbl_venname.Items.Count + ")";
                }
            }
            else
            {
                txt_venname.Text = "--Select--";
            }
        }
        catch { }
    }

    protected void bindvenbase1()
    {
        try
        {
            int i = 0;
            cbl_venname.Items.Clear();
            string q = "select distinct vm.VendorCode,vm.VendorCompName from CO_VendorMaster vm, IT_VendorReq vr where vm.VendorPK=vr.VendorFK and LibraryFlag='1'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(q, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_venname.DataSource = ds;
                cbl_venname.DataTextField = "VendorCompName";
                cbl_venname.DataValueField = "VendorCode";
                cbl_venname.DataBind();
                if (cbl_venname.Items.Count > 0)
                {
                    for (i = 0; i < cbl_venname.Items.Count; i++)
                    {
                        cbl_venname.Items[i].Selected = true;
                    }
                    txt_venname.Text = "Vendor Name(" + cbl_venname.Items.Count + ")";
                }
            }
            else
            {
                txt_venname.Text = "--Select--";
            }
        }
        catch { }
    }

    protected void Radioitem_Click(object sender, EventArgs e)
    {
        tditem_wise.Visible = true;
        tdbook_wise.Visible = false;
        tbl_ItemRequest.Visible = true;
        Tbl_Book_Request.Visible = false;
        divitemreqid.Visible = true;
        divbookreid.Visible = false;
        lbl_Online.Visible = false;
        rblsmsmail.Visible = false;
        smsmcontent.Visible = false;
        itemorbook = "1";
        rdo_approvedwise_Click(sender, e);
    }

    protected void RadioBook_Click(object sender, EventArgs e)
    {
        tditem_wise.Visible = false;
        tdbook_wise.Visible = true;
        rdo_approvedbookwise.Checked = true;
        tbl_ItemRequest.Visible = false;
        Tbl_Book_Request.Visible = true;
        divitemreqid.Visible = false;
        divbookreid.Visible = true;
        lbl_Online.Visible = false;
        smsmcontent.Visible = false;
        fdrblsmsmail.Visible = false;
        rblsmsmail.Visible = false;
        rdo_approvedbookwise_Click(sender, e);
        itemorbook = "2";
    }

    protected void rdo_bookdeptwise_Click(object sender, EventArgs e)
    {
        Tbl_Book_Request.Visible = true;
        Bookwise_false();
        requestwise_false1();
        datewise_true1();
        FpSpread1.Visible = false;
        btn_selectitem.Visible = false;
        lblvendor.Visible = false;
        txt_searchvendor.Visible = false;
        btn_request.Visible = false;
        txt_vendorname.Visible = false;
        UpdatePanel2.Visible = false;


    }
    protected void rdo_bookwise_Click(object sender, EventArgs e)
    {
        datewise_false1();
        requestwise_false1();
        Bookwise_true();
        FpSpread1.Visible = false;
        btn_selectitem.Visible = false;
        lblvendor.Visible = false;
        txt_searchvendor.Visible = false;
        btn_request.Visible = false;
        txt_vendorname.Visible = false;
        UpdatePanel2.Visible = false;


    }
    protected void rdo_approvedbookwise_Click(object sender, EventArgs e)
    {
        Bookwise_false();
        datewise_false1();
        requestwise_true1();

        FpSpread1.Visible = false;
        btn_selectitem.Visible = false;
        lblvendor.Visible = false;
        txt_searchvendor.Visible = false;
        btn_request.Visible = false;
        txt_vendorname.Visible = false;
        UpdatePanel2.Visible = false;
    }


    protected void binddepartment1()
    {
        try
        {
            ds.Clear();
            cbl_Department.Items.Clear();
            int i = 0;

            string dept = "select distinct lb.lib_name,bd.Lib_Code from RQ_Requisition R,RQ_RequisitionDet RD,bookdetails bd,Library lb where R.RequisitionPK =RD.RequisitionFK and RD.Lib_Bookid = bd.BookID and RequestType ='13'   and  ISNULL(VendorReq_Type,'0')=0 and RD.ReqAppStatus='1' and isnull(bd.Lib_Code,'')<>'' and lb.lib_code=bd.Lib_Code";
            ds = d2.select_method_wo_parameter(dept, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_Department.DataSource = ds;
                cbl_Department.DataTextField = "lib_name";
                cbl_Department.DataValueField = "Lib_Code";
                cbl_Department.DataBind();
                if (cbl_Department.Items.Count > 0)
                {
                    for (i = 0; i < cbl_Department.Items.Count; i++)
                    {
                        cbl_Department.Items[i].Selected = true;
                    }
                    TextDepartment.Text = "Library(" + cbl_Department.Items.Count + ")";
                }
                bindbook();
            }
            else
            {
                TextDepartment.Text = "--Select--";
            }
        }
        catch
        {

        }
    }

    protected void cb_Department_CheckedChange(object sender, EventArgs e)
    {
        int cout = 0;
        TextDepartment.Text = "--Select--";

        if (cb_Department.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_Department.Items.Count; i++)
            {
                cbl_Department.Items[i].Selected = true;
            }
            TextDepartment.Text = "Library(" + (cbl_Department.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_Department.Items.Count; i++)
            {
                cbl_Department.Items[i].Selected = false;
            }
        }
        bindbook();
    }
    protected void cbl_Department_SelectedIndexChange(object sender, EventArgs e)
    {
        int i = 0;
        cb_Department.Checked = false;
        //item();
        int commcount = 0;
        TextDepartment.Text = "--Select--";
        for (i = 0; i < cbl_Department.Items.Count; i++)
        {
            if (cbl_Department.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_Department.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_Department.Items.Count)
            {
                cb_dept.Checked = true;
            }
            TextDepartment.Text = "Library(" + commcount.ToString() + ")";
        }
        bindbook();
    }

    protected void bindbook()
    {
        try
        {
            ds.Clear();
            cbl_Book.Items.Clear();
            string deptcode = "";
            int i = 0;
            for (i = 0; i < cbl_Department.Items.Count; i++)
            {
                if (cbl_Department.Items[i].Selected == true)
                {
                    if (deptcode == "")
                    {
                        deptcode = "" + cbl_Department.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        deptcode = deptcode + "'" + "," + "'" + cbl_Department.Items[i].Value.ToString() + "";
                    }
                }
            }

            string book = "select distinct bd.Title,bd.BookID from RQ_Requisition R,RQ_RequisitionDet RD,bookdetails bd where R.RequisitionPK =RD.RequisitionFK and RD.Lib_Bookid = bd.BookID and RequestType ='13'   and  ISNULL(VendorReq_Type,'0')=0 and RD.ReqAppStatus='1' and bd.Lib_Code in('" + deptcode + "') ";
            //and Dept_Code in('" + deptcode + "')saran
            //and Title='TITLE'
            ds = d2.select_method_wo_parameter(book, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_Book.DataSource = ds;
                cbl_Book.DataTextField = "Title";
                cbl_Book.DataValueField = "BookID";
                cbl_Book.DataBind();
                if (cbl_Book.Items.Count > 0)
                {
                    for (i = 0; i < cbl_Book.Items.Count; i++)
                    {
                        cbl_Book.Items[i].Selected = true;
                    }
                    TextBook.Text = "Book (" + cbl_Book.Items.Count + ")";
                }
            }
            else
            {
                TextBook.Text = "--Select--";
            }
        }
        catch
        {

        }
    }

    protected void cb_Book_CheckedChange(object sender, EventArgs e)
    {
        int cout = 0;
        TextBook.Text = "--Select--";

        if (cb_Book.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_Book.Items.Count; i++)
            {
                cbl_Book.Items[i].Selected = true;
            }
            TextBook.Text = "Book(" + (cbl_Book.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_Book.Items.Count; i++)
            {
                cbl_Book.Items[i].Selected = false;
            }
        }


    }
    protected void cbl_Book_SelectedIndexChange(object sender, EventArgs e)
    {
        int i = 0;
        cb_Book.Checked = false;
        int commcount = 0;
        TextBook.Text = "--Select--";
        for (i = 0; i < cbl_Book.Items.Count; i++)
        {
            if (cbl_Book.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_Book.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_Book.Items.Count)
            {
                cb_Book.Checked = true;
            }
            TextBook.Text = "Book(" + commcount.ToString() + ")";
        }
    }


    protected void btn_book_request_Go_Click(object sender, EventArgs e)
    {

        try
        {
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string firstdate = Convert.ToString(Text_fromdate.Text);
            string seconddate = Convert.ToString(Text_Todate.Text);
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            string[] split1 = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);

            string deptcode = "";
            for (int i = 0; i < cbl_Department.Items.Count; i++)
            {
                if (cbl_Department.Items[i].Selected == true)
                {
                    if (deptcode == "")
                    {
                        deptcode = "" + cbl_Department.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        deptcode = deptcode + "'" + "," + "'" + cbl_Department.Items[i].Value.ToString() + "";
                    }
                }
            }
            string bookid = "";
            for (int i = 0; i < cbl_Book.Items.Count; i++)
            {
                if (cbl_Book.Items[i].Selected == true)
                {
                    if (bookid == "")
                    {
                        bookid = "" + cbl_Book.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        bookid = bookid + "'" + "," + "'" + cbl_Book.Items[i].Value.ToString() + "";
                    }
                }
            }
            string vendorselectquery = "";
            if (txt_searchreq.Text.Trim() != "")
            {
                vendorselectquery = "select distinct r.Requestcode,RequisitionPK,bd.Title,bd.Author,bd.Lib_Code,rd.ReqQty -isnull(RejQty,0)as appqty,convert(varchar(10), r.requestdate,103) as requestdate,bd.BookID   from RQ_Requisition R,RQ_RequisitionDet RD,bookdetails bd  where R.RequisitionPK=RD.RequisitionFK and RD.Lib_Bookid = bd.BookID and RequestType ='13' and r.requestcode='" + txt_searchreq.Text + "' and ISNULL(VendorReq_Type,'0')=0 and RD.ReqAppStatus='1' ";
            }
            else
            {
                vendorselectquery = " select distinct Requestcode,RequisitionPK,bd.Title,bd.Author,bd.Lib_Code,rd.ReqQty -isnull(RejQty,0)as appqty,convert(varchar(10), r.requestdate,103) as requestdate,bd.BookID from RQ_Requisition R,RQ_RequisitionDet RD,bookdetails bd where R.RequisitionPK =RD.RequisitionFK and RD.Lib_Bookid = bd.BookID and RequestType ='13' and  requestdate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and bd.BookID  in ('" + bookid + "') and  ISNULL(VendorReq_Type,'0')=0 and RD.ReqAppStatus='1' ";
                //and  bd.Dept_Code in('" + deptcode + "') saran
            }

            ds.Clear();
            ds = d2.select_method_wo_parameter(vendorselectquery, "Text");
            if (ds.Tables.Count > 0 & ds.Tables[0].Rows.Count == 0)
            {

                if (txt_searchreq.Text.Trim() != "")
                {
                    vendorselectquery = "select distinct r.Requestcode,RequisitionPK,bd.Title,bd.Author,bd.Lib_Code,rd.ReqQty -isnull(RejQty,0)as appqty,convert(varchar(10), r.requestdate,103) as requestdate,bd.BookID   from RQ_Requisition R,RQ_RequisitionDet RD,bookdetails bd  where R.RequisitionPK=RD.RequisitionFK and RD.Lib_Bookid = bd.BookID and RequestType ='13' and r.requestcode='" + txt_searchreq.Text + "' and ISNULL(VendorReq_Type,'0')=1 and RD.ReqAppStatus='1' and ItemFK not in( select ItemFK from IT_VendorReqDet where ReqFK=RequisitionPK) ";
                }
                else
                {
                    vendorselectquery = " select distinct Requestcode,RequisitionPK,bd.Title,bd.Author,bd.Lib_Code,rd.ReqQty -isnull(RejQty,0)as appqty,convert(varchar(10), r.requestdate,103) as requestdate,bd.BookID from RQ_Requisition R,RQ_RequisitionDet RD,bookdetails bd where R.RequisitionPK =RD.RequisitionFK and RD.Lib_Bookid = bd.BookID and RequestType ='13' and  requestdate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and bd.BookID  in ('" + bookid + "') and  ISNULL(VendorReq_Type,'0')=1 and RD.ReqAppStatus='1' and ItemFK not in( select ItemFK from IT_VendorReqDet where ReqFK=RequisitionPK) ";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(vendorselectquery, "Text");
            }
            if (deptcode.Trim() != "" && bookid.Trim() != "")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable data = new DataTable();
                    data.Columns.Add("SNo", typeof(string));
                    data.Columns.Add("RequestCode", typeof(string));
                    data.Columns.Add("Title", typeof(string));
                    data.Columns.Add("Author", typeof(string));
                    data.Columns.Add("Approved", typeof(string));
                    data.Columns.Add("RequestDate", typeof(string));
                    data.Columns.Add("libcode", typeof(string));
                    data.Columns.Add("bookid", typeof(string));
                    data.Columns.Add("ReqPk", typeof(string));
                    DataRow drow;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        drow = data.NewRow();
                        drow["SNo"] = Convert.ToString(i + 1);
                        drow["RequestCode"] = Convert.ToString(ds.Tables[0].Rows[i]["Requestcode"]);
                        drow["Title"] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                        drow["Author"] = Convert.ToString(ds.Tables[0].Rows[i]["Author"]);
                        string appcopy = Convert.ToString(ds.Tables[0].Rows[i]["appqty"]);
                        if (appcopy != "")
                        {
                            if (appcopy.Contains('.'))
                            {
                                string[] spilt = appcopy.Split('.');
                                appcopy = spilt[0].ToString();
                            }
                        }
                        drow["Approved"] = Convert.ToString(appcopy);
                        drow["RequestDate"] = Convert.ToString(ds.Tables[0].Rows[i]["requestdate"]);
                        drow["libcode"] = Convert.ToString(ds.Tables[0].Rows[i]["Lib_Code"]);
                        drow["bookid"] = Convert.ToString(ds.Tables[0].Rows[i]["BookID"]);
                        drow["ReqPk"] = Convert.ToString(ds.Tables[0].Rows[i]["RequisitionPK"]);
                        data.Rows.Add(drow);

                    }
                    if (data.Columns.Count > 0 && data.Rows.Count > 0)
                    {
                        Showgrid.DataSource = data;
                        Showgrid.DataBind();
                        Showgrid.Visible = true;
                        div_book.Visible = true;
                        if (rdb_all.Checked == true)
                            Showgrid.Columns[1].Visible = false;
                        if (rdb_suggested.Checked == true)
                            Showgrid.Columns[1].Visible = true;


                        lbl_error.Visible = false;
                        txt_searchreq.Text = "";
                        reqdiv.Visible = true;
                        smsmcontent.Visible = true;
                        lblvendor.Visible = true;
                        txt_searchvendor.Visible = false;
                        btn_request.Visible = true;
                        txt_vendorname.Visible = true;
                        UpdatePanel2.Visible = true;
                        fdrblsmsmail.Visible = true;
                        rblsmsmail.Visible = true;
                        lbl_Online.Visible = true;
                        chkallbook.Visible = true;
                        txt_vendorname.Text = "--Select--";
                        if (rdb_all.Checked == true)
                        {
                            btn_selectitem.Visible = false;

                        }
                        else
                        {
                            btn_selectitem.Visible = true;
                            btn_selectitem.Text = "Selected Book";
                        }

                    }



                }
                else
                {
                    lbl_error.Visible = true;
                    Showgrid.Visible = false;
                    lbl_error.Text = "No Record Found";
                    btn_selectitem.Visible = false;
                    reqdiv.Visible = false;
                    smsmcontent.Visible = false;
                    chkallbook.Visible = false;
                    div_book.Visible = false;
                }
            }
            else
            {
                lbl_error.Visible = true;
                Showgrid.Visible = false;
                div_book.Visible = false;
                lbl_error.Text = "Please Select All Fields";

            }
        }
        catch
        {
        }
    }


    //visible functions
    protected void datewise_true1()
    {
        Label_book_Department.Visible = true;
        TextDepartment.Visible = true;
        PanelDepartment.Visible = true;
        Label_fromdate.Visible = true;
        Text_fromdate.Visible = true;
        Label_Todate.Visible = true;
        Text_Todate.Visible = true;
        TextBox_dept.Visible = true;


    }
    protected void datewise_false1()
    {
        Label_book_Department.Visible = false;
        TextDepartment.Visible = false;
        PanelDepartment.Visible = false;
        Label_fromdate.Visible = false;
        Text_fromdate.Visible = false;
        Label_Todate.Visible = false;
        Text_Todate.Visible = false;
        TextBox_dept.Visible = false;
    }
    protected void Bookwise_true()
    {
        Label_Book.Visible = true;
        TextBook.Visible = true;
        PanelBook.Visible = true;


        Label_book_Department.Visible = true;
        TextDepartment.Visible = true;
        PanelDepartment.Visible = true;
        Label_fromdate.Visible = true;
        Text_fromdate.Visible = true;
        Label_Todate.Visible = true;
        Text_Todate.Visible = true;
        TextBox_book.Visible = true;

    }
    protected void Bookwise_false()
    {
        Label_Book.Visible = false;
        TextBook.Visible = false;
        PanelBook.Visible = false;
        Label_book_Department.Visible = false;
        TextDepartment.Visible = false;
        PanelDepartment.Visible = false;
        Label_fromdate.Visible = false;
        Text_fromdate.Visible = false;
        Label_Todate.Visible = false;
        Text_Todate.Visible = false;
        TextBox_book.Visible = false;
        ddl_search.Visible = false;

    }
    protected void requestwise_true1()
    {

        Label_Book.Visible = true;
        TextBook.Visible = true;
        PanelBook.Visible = true;
        Label_book_Department.Visible = true;
        TextDepartment.Visible = true;
        PanelDepartment.Visible = true;
        Label_fromdate.Visible = true;
        Text_fromdate.Visible = true;
        Label_Todate.Visible = true;
        Text_Todate.Visible = true;
        TextBox_bookreqid.Visible = true;

    }
    protected void requestwise_false1()
    {
        cb_request.Visible = false;
        cb_notrequest.Visible = false;

        cb_request.Checked = false;
        cb_notrequest.Checked = false;

        Label_Book.Visible = false;
        TextBook.Visible = false;
        PanelBook.Visible = false;


        Label_book_Department.Visible = false;
        TextDepartment.Visible = false;
        PanelDepartment.Visible = false;

        TextBox_bookreqid.Visible = false;
    }

    public void bindbookrequestcode()
    {
        try
        {

            string newitemcode = "";
            string selectquery = "select VenBookReqAcr,VenBookReqStNo,VenBookReqSize  from IM_CodeSettings order by startdate desc";
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string ordcode = Convert.ToString(ds.Tables[0].Rows[0]["VenBookReqAcr"]);
                string itemacronym = Convert.ToString(ds.Tables[0].Rows[0]["VenBookReqAcr"]);
                string itemstarno = Convert.ToString(ds.Tables[0].Rows[0]["VenBookReqStNo"]);
                string itemsize = Convert.ToString(ds.Tables[0].Rows[0]["VenBookReqSize"]);


                selectquery = "select distinct top (1) venreqpk,VenReqCode  from IT_VendorReq where itemorlibrary='1'  order by venreqpk desc";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string itemcode = Convert.ToString(ds.Tables[0].Rows[0]["VenReqCode"]);
                    string itemacr = Convert.ToString(itemacronym);
                    int len = itemacr.Length;
                    itemcode = itemcode.Remove(0, len);
                    int len1 = Convert.ToString(itemcode).Length;
                    string newnumber = Convert.ToString((Convert.ToInt32(itemcode) + 1));
                    len = Convert.ToString(newnumber).Length;
                    //len1 = len1 - len;

                    len1 = Convert.ToInt32(itemsize) - len;//3.11.15
                    if (len1 == 2)
                    {
                        newitemcode = "00" + newnumber;
                    }
                    else if (len1 == 1)
                    {
                        newitemcode = "0" + newnumber;
                    }
                    else if (len1 == 4)
                    {
                        newitemcode = "0000" + newnumber;
                    }
                    else if (len1 == 3)
                    {
                        newitemcode = "000" + newnumber;
                    }
                    else
                    {
                        newitemcode = Convert.ToString(newnumber);
                    }
                    if (newitemcode.Trim() != "")
                    {
                        newitemcode = itemacr + "" + newitemcode;
                    }
                }
                else
                {
                    string itemacr = Convert.ToString(itemstarno);
                    int len = itemacr.Length;

                    string items = Convert.ToString(itemsize);
                    int len1 = Convert.ToInt32(items);//items.Length;

                    int size = len1 - len;//Convert.ToInt32(itemacr);

                    if (size == 2)
                    {
                        newitemcode = "00" + itemstarno;
                    }
                    else if (size == 1)
                    {
                        newitemcode = "0" + itemstarno;
                    }
                    else if (size == 4)
                    {
                        newitemcode = "0000" + itemstarno;
                    }
                    else if (size == 3)
                    {
                        newitemcode = "000" + itemstarno;
                    }
                    else
                    {
                        newitemcode = Convert.ToString(itemstarno);
                    }
                    newitemcode = Convert.ToString(itemacronym) + "" + Convert.ToString(newitemcode);

                    // newitemcode = Convert.ToString(itemacronym) + "" + Convert.ToString(itemstarno);
                }
                txt_reff_id.Text = Convert.ToString(newitemcode);
            }
        }
        catch
        {

        }
    }

    protected void Showgrid_OnRowDataBound(object sender, EventArgs e)
    {
        try
        {


        }
        catch
        {


        }


    }

}