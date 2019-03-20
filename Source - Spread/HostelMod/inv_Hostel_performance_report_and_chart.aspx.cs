using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
using System.Configuration;

public partial class inv_Hostel_performance_report_and_chart : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    bool check = false;
    Hashtable hat = new Hashtable();
    Hashtable hatname = new Hashtable();
    Hashtable hatvalue = new Hashtable();
    DataView dv = new DataView();
    DataView dv1 = new DataView();
    Hashtable addindex = new Hashtable();
    DataTable chartdata = new DataTable();
    DataTable checktable = new DataTable();
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
        calfromdate.EndDate = DateTime.Now;
        caltodate.EndDate = DateTime.Now;
        if (rdodaycom.Checked == true)
        {
            txtdaycompar.Visible = true;
            p00.Visible = true;
        }
        else
        {
            txtdaycompar.Visible = false;
            p00.Visible = false;
        }
        if (rdodaycompar1.Checked == true)
        {
            txtcompar1.Visible = true;
            p16.Visible = true;
        }
        else
        {
            //chklstdaycompar.Items.Clear();
            txtcompar1.Visible = false;
            p16.Visible = false;
        }
        if (!IsPostBack)
        {
            rdb_headerwise.Checked = true;
            rdoday.Checked = true;
            txtfromdate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            txtpop2to.Attributes.Add("readonly", "readonly");
            txtpop2from.Attributes.Add("readonly", "readonly");
            txtpop2sessionname.Attributes.Add("readonly", "readonly");
            txtpop2menuname.Attributes.Add("readonly", "readonly");

            txtpop3from.Attributes.Add("readonly", "readonly");
            txtpop3to.Attributes.Add("readonly", "readonly");
            txtpop3session.Attributes.Add("readonly", "readonly");
            txtpop4session.Attributes.Add("readonly", "readonly");
            txtpop4to.Attributes.Add("readonly", "readonly");
            txtpop4menuname.Attributes.Add("readonly", "readonly");
            txtpop4from.Attributes.Add("readonly", "readonly");
            txtpop5from.Attributes.Add("readonly", "readonly");
            txtpop5to.Attributes.Add("readonly", "readonly");
            txtpop6hos.Attributes.Add("readonly", "readonly");
            txtpop6session.Attributes.Add("readOnly", "readonly");

            txt_pop6fromdate.Attributes.Add("readOnly", "readonly");
            txt_pop6todate.Attributes.Add("readOnly", "readonly");


            txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtpop2from.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtpop2to.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtpop3from.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtpop3to.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtpop4from.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtpop4to.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtpop5from.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtpop5to.Text = DateTime.Now.ToString("dd/MM/yyyy");

            txt_pop6fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_pop6todate.Text = DateTime.Now.ToString("dd/MM/yyyy");


            div1itemused.Visible = true;
            div2Itempurchasehty.Visible = false;
            div3menumenuprehty.Visible = false;
            div4costpurhty.Visible = false;
            div5vensuphty.Visible = false;
            div6menuexp.Visible = false;
            rdbquantity.Checked = true;
            rdopop2Qty.Checked = true;

            bindhostelname();
            bindpop2hostelname();
            bindpop3hostelname();
            bindpop4hostelname();
            bindpop5vendorname();
            bindpop6hostelname();
        }

    }

    protected void btn_itemused_Click(object sender, EventArgs e)
    {
        try
        {
            div1itemused.Visible = true;
            div2Itempurchasehty.Visible = false;
            div3menumenuprehty.Visible = false;
            div4costpurhty.Visible = false;
            div5vensuphty.Visible = false;
            div6menuexp.Visible = false;

            rdbquantity.Checked = true;
        }
        catch
        {

        }
    }

    protected void btnitem_purchase_Click(object sender, EventArgs e)
    {
        try
        {
            div1itemused.Visible = false;
            div2Itempurchasehty.Visible = true;
            div3menumenuprehty.Visible = false;
            div4costpurhty.Visible = false;
            div5vensuphty.Visible = false;
            div6menuexp.Visible = false;
            bindpop2session();
        }
        catch
        {

        }
    }
    protected void btnmenu_prepaid_Click(object sender, EventArgs e)
    {
        try
        {
            div1itemused.Visible = false;
            div2Itempurchasehty.Visible = false;
            div3menumenuprehty.Visible = true;
            div4costpurhty.Visible = false;
            div5vensuphty.Visible = false;
            div6menuexp.Visible = false;
        }
        catch { }
    }
    protected void btncost_pur_Click(object sender, EventArgs e)
    {
        try
        {
            div1itemused.Visible = false;
            div2Itempurchasehty.Visible = false;
            div3menumenuprehty.Visible = false;
            div4costpurhty.Visible = true;
            div5vensuphty.Visible = false;
            div6menuexp.Visible = false;
        }
        catch { }
    }
    protected void btnvendor_sup_Click(object sender, EventArgs e)
    {
        try
        {
            div1itemused.Visible = false;
            div2Itempurchasehty.Visible = false;
            div3menumenuprehty.Visible = false;
            div4costpurhty.Visible = false;
            div5vensuphty.Visible = true;
            div6menuexp.Visible = false;
            bindpop5vendorname();
            binditem();
            rdQunatity.Checked = true;
        }
        catch { }
    }
    protected void btnmenuexp_hty_Click(object sender, EventArgs e)
    {
        try
        {
            div1itemused.Visible = false;
            div2Itempurchasehty.Visible = false;
            div3menumenuprehty.Visible = false;
            div4costpurhty.Visible = false;
            div5vensuphty.Visible = false;
            div6menuexp.Visible = true;
            bindpop6hostelname();
            bindpop6sessionname();
            rdopop6day.Checked = true;
            rdbExpenses.Checked = true;
        }
        catch { }
    }
    protected void btnwestage_Click(object sender, EventArgs e)
    {
        try { }
        catch { }
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
    protected void chksession_checkedchange(object sender, EventArgs e)
    {
        if (chksessionname.Checked == true)
        {
            for (int i = 0; i < chklstsession.Items.Count; i++)
            {
                chklstsession.Items[i].Selected = true;
            }
            txtsessionname.Text = "Session Name(" + (chklstsession.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chklstsession.Items.Count; i++)
            {
                chklstsession.Items[i].Selected = false;
            }
            txtsessionname.Text = "--Select--";
        }
        loadmenuname();

    }

    protected void chklstsession_Change(object sender, EventArgs e)
    {
        txtsessionname.Text = "--Select--";
        chksessionname.Checked = false;
        int commcount = 0;
        for (int i = 0; i < chklstsession.Items.Count; i++)
        {
            if (chklstsession.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtsessionname.Text = "Session Name(" + commcount.ToString() + ")";
            if (commcount == chklstsession.Items.Count)
            {
                chksessionname.Checked = true;
            }
        }
        loadmenuname();

    }


    protected void chkmenuname_Change(object sender, EventArgs e)
    {
        if (chkmenuname.Checked == true)
        {
            for (int i = 0; i < chk_lstmenuname.Items.Count; i++)
            {
                chk_lstmenuname.Items[i].Selected = true;
            }
            txtmenuname.Text = "Menu Name(" + (chk_lstmenuname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chk_lstmenuname.Items.Count; i++)
            {
                chk_lstmenuname.Items[i].Selected = false;
            }
            txtmenuname.Text = "--Select--";
        }
    }

    protected void txtfromdate_Change(object sender, EventArgs e)
    {
        try
        {
            loadmenuname();
        }
        catch
        {

        }
    }
    protected void txttodate_Change(object sender, EventArgs e)
    {
        try
        {
            loadmenuname();
        }
        catch
        {

        }
    }
    protected void chk_lstmenuname_Change(object sender, EventArgs e)
    {
        txtmenuname.Text = "--Select--";
        chkmenuname.Checked = false;
        int commcount = 0;
        for (int i = 0; i < chk_lstmenuname.Items.Count; i++)
        {
            if (chk_lstmenuname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtmenuname.Text = "Menu Name(" + commcount.ToString() + ")";
            if (commcount == chk_lstmenuname.Items.Count)
            {
                chkmenuname.Checked = true;
            }
        }
    }
    protected void chkhostelchange(object sender, EventArgs e)
    {
        try
        {
            if (chkhostelname.Checked == true)
            {
                for (int i = 0; i < chklsthostelname.Items.Count; i++)
                {
                    chklsthostelname.Items[i].Selected = true;
                }
                txthostel.Text = "Mess Name(" + (chklsthostelname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklsthostelname.Items.Count; i++)
                {
                    chklsthostelname.Items[i].Selected = false;
                }
                txthostel.Text = "--Select--";
            }
            bindsession();
            //loadmenuname();
        }
        catch
        {

        }
    }
    protected void chklsthostelname_selectindex(object sender, EventArgs e)
    {
        try
        {
            txthostel.Text = "--Select--";
            chkhostelname.Checked = false;
            int commcount = 0;
            for (int i = 0; i < chklsthostelname.Items.Count; i++)
            {
                if (chklsthostelname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txthostel.Text = "Mess Name(" + commcount.ToString() + ")";
                if (commcount == chklsthostelname.Items.Count)
                {
                    chkhostelname.Checked = true;
                }
            }
            bindsession();
            loadmenuname();

        }
        catch
        {

        }
    }
    protected void bindhostelname()
    {
        try
        {
            chklsthostelname.Items.Clear();
            ds.Clear();
            //ds = d2.BindHostel(collegecode1);//Idhris 10/10/2015
            //ds = d2.Bindmess_inv(collegecode1);
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklsthostelname.DataSource = ds;
                //chklsthostelname.DataTextField = "Hostel_Name";
                //chklsthostelname.DataValueField = "Hostel_Code";
                chklsthostelname.DataTextField = "MessName";
                chklsthostelname.DataValueField = "MessMasterPK";
                chklsthostelname.DataBind();
                if (chklsthostelname.Items.Count > 0)
                {
                    for (int row = 0; row < chklsthostelname.Items.Count; row++)
                    {
                        chklsthostelname.Items[row].Selected = true;
                    }
                    txthostel.Text = "Mess Name (" + chklsthostelname.Items.Count + ")";
                }

            }
            else
            {

            }
            bindsession();
            loadmenuname();

        }
        catch
        {

        }
    }
    //protected void ddlhostelname_change(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        bindsession();
    //        loadmenuname();
    //    }
    //    catch
    //    {

    //    }
    //}
    protected void chdaycompar_change(object sender, EventArgs e)
    {
        try
        {
            if (chkdaycompar.Checked == true)
            {
                for (int i = 0; i < chklstdaycompar.Items.Count; i++)
                {
                    chklstdaycompar.Items[i].Selected = true;
                }
                txtdaycompar.Text = "day (" + (chklstdaycompar.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstdaycompar.Items.Count; i++)
                {
                    chklstdaycompar.Items[i].Selected = false;
                }
                txtdaycompar.Text = "--Select--";
            }
        }
        catch
        {

        }
    }
    protected void chkklstdaycompar_selectIndex(object sender, EventArgs e)
    {

        try
        {
            txtdaycompar.Text = "--Select--";
            chkdaycompar.Checked = false;
            int commcount = 0;
            for (int i = 0; i < chklstdaycompar.Items.Count; i++)
            {
                if (chklstdaycompar.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtdaycompar.Text = "Day (" + commcount.ToString() + ")";
                if (commcount == chklstdaycompar.Items.Count)
                {
                    chkdaycompar.Checked = true;
                }
            }
        }
        catch
        {

        }
    }

    protected void ddlpop2hostelname_change(object sender, EventArgs e)
    {
        try
        {
            bindpop2session();
            loadpop2menuname();
        }
        catch
        {

        }
    }
    public void bindsession()
    {
        try
        {
            string hostel = "";
            for (int i = 0; i < chklsthostelname.Items.Count; i++)
            {
                if (chklsthostelname.Items[i].Selected == true)
                {
                    if (hostel == "")
                    {
                        hostel = "" + chklsthostelname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        hostel = hostel + "'" + "," + "'" + chklsthostelname.Items[i].Value.ToString() + "";
                    }
                }
            }


            ds.Clear();
            chklstsession.Items.Clear();
            if (hostel.Trim() != "")
            {
                string selecthostel = "select distinct SessionMasterPK,SessionName from HM_SessionMaster where MessMasterFK in ('" + hostel + "')";
                ds = d2.select_method_wo_parameter(selecthostel, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklstsession.DataSource = ds;
                    chklstsession.DataTextField = "SessionName";
                    chklstsession.DataValueField = "SessionMasterPK";
                    chklstsession.DataBind();
                    if (chklstsession.Items.Count > 0)
                    {
                        for (int i = 0; i < chklstsession.Items.Count; i++)
                        {
                            chklstsession.Items[i].Selected = true;
                        }
                        txtsessionname.Text = "Session Name(" + chklstsession.Items.Count + ")";
                    }
                }
                else
                {
                    txtsessionname.Text = "--Select--";
                }
            }
            else
            {
                txtsessionname.Text = "--Select--";
            }
            loadmenuname();
        }
        catch
        {

        }
    }
    protected void chdaycompar_change1(object sender, EventArgs e)
    {
        try
        {
            if (chkdaycompar1.Checked == true)
            {
                for (int i = 0; i < chklstdaycompar1.Items.Count; i++)
                {
                    chklstdaycompar1.Items[i].Selected = true;
                }
                txtcompar1.Text = "day (" + (chklstdaycompar1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstdaycompar1.Items.Count; i++)
                {
                    chklstdaycompar1.Items[i].Selected = false;
                }
                txtcompar1.Text = "--Select--";
            }
        }
        catch
        {

        }
    }
    protected void chkklstdaycompar_selectIndex1(object sender, EventArgs e)
    {
        try
        {
            txtcompar1.Text = "--Select--";
            chkdaycompar1.Checked = false;
            int commcount = 0;
            for (int i = 0; i < chklstdaycompar1.Items.Count; i++)
            {
                if (chklstdaycompar1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtcompar1.Text = "Day (" + commcount.ToString() + ")";
                if (commcount == chklstdaycompar1.Items.Count)
                {
                    chkdaycompar1.Checked = true;
                }
            }
        }
        catch
        {

        }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            string hostel = "";///new
            for (int i = 0; i < chklsthostelname.Items.Count; i++)
            {
                if (chklsthostelname.Items[i].Selected == true)
                {
                    if (hostel == "")
                    {
                        hostel = "" + chklsthostelname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        hostel = hostel + "'" + "," + "'" + chklsthostelname.Items[i].Value.ToString() + "";
                    }
                }
            }

            if (hostel.Trim() != "")
            {
                string hostelcode = Convert.ToString(hostel);
                string itemheadercode = "";
                for (int i = 0; i < chklstsession.Items.Count; i++)
                {
                    if (chklstsession.Items[i].Selected == true)
                    {
                        if (itemheadercode == "")
                        {
                            itemheadercode = "" + chklstsession.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            itemheadercode = itemheadercode + "'" + "," + "'" + chklstsession.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string itemheadercode1 = "";
                for (int i = 0; i < chk_lstmenuname.Items.Count; i++)
                {
                    if (chk_lstmenuname.Items[i].Selected == true)
                    {
                        if (itemheadercode1 == "")
                        {
                            itemheadercode1 = "" + chk_lstmenuname.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            itemheadercode1 = itemheadercode1 + "'" + "," + "'" + chk_lstmenuname.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string fromdate = Convert.ToString(txtfromdate.Text);
                DateTime dt = new DateTime();
                string[] split = fromdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);//Fromdate-dnew
                string todate = Convert.ToString(txttodate.Text);
                DateTime dt1 = new DateTime();
                string[] split1 = todate.Split('/');
                dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);//Todate
                string days = "";
                DateTime dnew = new DateTime();
                dnew = dt;
                DataTable data = new DataTable();
                DataRow dr;
                while (dnew <= dt1)
                {
                    if (days == "")
                    {
                        days = Convert.ToString(dnew.ToString("dddd"));
                    }
                    else
                    {
                        days = days + "'" + "," + "'" + Convert.ToString(dnew.ToString("dddd"));
                    }
                    dnew = dnew.AddDays(1);
                }
                Chart1.Visible = false;
                if (chk_directconsum.Checked == true)
                {
                    itemheadercode = "0"; itemheadercode1 = "0";
                }
                if (itemheadercode.Trim() != "" && itemheadercode1.Trim() != "")
                {
                    string selectquery = ""; string directconsumquery = ""; string directgroupquery = ""; bool directchk = false;
                    if (chk_directconsum.Checked == true)
                    {
                        directconsumquery = " and MenuMasterFK is null and SessionFK is null";
                        directgroupquery = "";
                        directchk = true;
                    }
                    else
                    {
                        directconsumquery = " and MenuMasterFK  in ('" + itemheadercode1 + "') and SessionFK in ('" + itemheadercode + "') ";
                        directgroupquery = " ,Sessionfk,MenuMasterFK";
                    }
                    if (rdb_itemwise.Checked == true)
                    {

                        selectquery = " select SUM( ConsumptionQty*rpu )Consumption_Value,SUM( ConsumptionQty )Consumption_Qty,i.ItemCode as item_code,(itemname+' ('+itemunit+') ') as item_name,SessionFK,dm.MenuMasterFK, dm.MessMasterFK , DailyConsDate from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and dm.DailyConsumptionMasterPK=dd.DailyConsumptionMasterFK and dm.MessMasterFK in ('" + hostelcode + "') and ForMess<>'2' and i.ItemPK=dd.ItemFK " + directconsumquery + "  group by i.ItemCode,itemname,SessionFK,MenuMasterFK, dm.MessMasterFK,itemunit,DailyConsDate order by i.ItemCode  " + directgroupquery + "";

                        selectquery = selectquery + "   select distinct i.itemcode as item_code,(itemname+' ('+itemunit+') ') as item_name  from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd ,IM_ItemMaster i where dm.DailyConsumptionMasterPK=dd.DailyConsumptionMasterFK and i.ItemPK=dd.ItemFK and i.ItemPK=dd.ItemFK and DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' " + directconsumquery + "  and dm.MessMasterFK in ('" + hostelcode + "') and ForMess<>'2'  order by i.itemcode ";

                        selectquery = selectquery + "  select distinct SessionFK,Total_Present,DailyConsDate from HT_DailyConsumptionMaster  where DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and DailyConsumptionMasterPK  in (select DailyConsumptionMasterFK from HT_DailyConsumptionDetail ) and MessMasterFK in ('" + hostelcode + "') and ForMess<>'2' ";
                    }
                    if (rdb_Subheaderwise.Checked == true)
                    {
                        selectquery = "   select SUM(ConsumptionQty*rpu)Consumption_Value,SUM( ConsumptionQty )Consumption_Qty,i.subheader_code as Item_Code,(select MasterValue from CO_MasterValues where MasterCode =subheader_code) as item_name ,SessionFK,dm.MenuMasterFK,dm.MessMasterFK,DailyConsDate from HT_DailyConsumptionMaster  dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK and i.ItemPK =dd.ItemFK and isnull(subheader_code,'0')<>0  and DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and dm.MessMasterFK in ('" + hostelcode + "') and ForMess<>'2' " + directconsumquery + " group by i.subheader_code,itemname,SessionFK, dm.MenuMasterFK,dm.MessMasterFK ,itemunit,DailyConsDate order by i.subheader_code,MenuMasterFK";

                        selectquery = selectquery + "  select distinct i.subheader_code as Item_Code,(select MasterValue from CO_MasterValues where MasterCode =subheader_code) as item_name  from HT_DailyConsumptionMaster  dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK  =dd.DailyConsumptionMasterFK  and i.ItemPK =dd.ItemFK and isnull(subheader_code,'0')<>0  and DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' " + directconsumquery + " and dm.MessMasterFK in ('" + hostelcode + "') and ForMess<>'2' order by i.subheader_code ";

                        selectquery = selectquery + " select distinct SessionFK,Total_Present,DailyConsDate from HT_DailyConsumptionMaster dm where DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and DailyConsumptionMasterPK  in (select DailyConsumptionMasterFK from HT_DailyConsumptionDetail ) and MessMasterFK in ('" + hostelcode + "') and ForMess<>'2' " + directconsumquery + "";

                    }
                    if (rdb_headerwise.Checked == true)
                    {
                        selectquery = "  select  SUM( ConsumptionQty*RPU )Consumption_Value,SUM( ConsumptionQty )Consumption_Qty,i.ItemHeaderCode as Item_Code,(itemheadername) as item_name,SessionFK,dm.MenuMasterFK ,dm.MessMasterFK,DailyConsDate  from  HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail Dt ,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dt.DailyConsumptionMasterFK and i.ItemPK=dt.ItemFK and DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' " + directconsumquery + " and dm.MessMasterFK in ('" + hostelcode + "') and ForMess<>'2' group by SessionFK,dm.MessMasterFK,DailyConsDate,i.ItemHeaderCode,i.itemheadername,dm.MenumasterFK";

                        selectquery = selectquery + "   select distinct i.itemheadercode as Item_Code,(itemheadername) as item_name  from HT_DailyConsumptionMaster  dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK  =dd.DailyConsumptionMasterFK and i.ItemPK  =dd.ItemFK and  DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' " + directconsumquery + " and dm.MessMasterFK in ('" + hostelcode + "') and ForMess<>'2' order by i.itemheadercode";

                        selectquery = selectquery + "  select distinct SessionFK, Total_Present,DailyConsDate from HT_DailyConsumptionMaster dm where DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and DailyConsumptionMasterPK in (select DailyConsumptionMasterFK  from HT_DailyConsumptionDetail ) and MessMasterFK in ('" + hostelcode + "') and ForMess<>'2' " + directconsumquery + "";

                    }
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                    {
                        btnprintimag.Visible = true;
                        if (rdoday.Checked == true)
                        {
                            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                            {
                                for (int rs = 0; rs < ds.Tables[1].Rows.Count; rs++)
                                {
                                    data.Columns.Add(Convert.ToString(ds.Tables[1].Rows[rs]["item_name"]));
                                    addindex.Add(Convert.ToString(ds.Tables[1].Rows[rs]["item_name"]), rs);
                                }
                                Chart1.Series.Clear();
                                while (dt <= dt1)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = "DailyConsDate='" + dt.ToString("MM/dd/yyyy") + "' ";
                                    dv1 = ds.Tables[0].DefaultView;
                                    DataView dvnew = new DataView();
                                    if (dv1.Count > 0)
                                    {
                                        double newstrenth = 0;
                                        if (ds.Tables[2].Rows.Count > 0)
                                        {
                                            ds.Tables[2].DefaultView.RowFilter = " DailyConsDate='" + dt.ToString("MM/dd/yyyy") + "'";
                                            dvnew = ds.Tables[2].DefaultView;
                                            if (dvnew.Count > 0)
                                            {
                                                DataTable dt_table = new DataTable();
                                                dt_table = dvnew.ToTable();
                                                string strengh = Convert.ToString(dt_table.Compute("Sum(Total_Present)", "")); ;
                                                if (strengh.Trim() != "")
                                                {
                                                    newstrenth = Convert.ToDouble(strengh);
                                                }
                                            }
                                        }
                                        Chart1.Series.Add(dt.ToString("dd/MM/yyyy") + " - " + dt.ToString("dddd") + "(" + newstrenth + ")");
                                        Chart1.Series[0].BorderWidth = 2;
                                        Chart1.RenderType = RenderType.ImageTag;
                                        Chart1.ImageType = ChartImageType.Png;
                                        Chart1.ImageStorageMode = ImageStorageMode.UseImageLocation;
                                        Chart1.ImageLocation = Path.Combine("~/report/", "HostelItemHistory");
                                        if (directchk == false)
                                        {
                                            #region Menu and session wise
                                            for (int i = 0; i < chklstsession.Items.Count; i++)
                                            {
                                                if (chklstsession.Items[i].Selected == true)
                                                {
                                                    if (chk_lstmenuname.Items.Count > 0)
                                                    {
                                                        for (int co = 0; co < chk_lstmenuname.Items.Count; co++)
                                                        {
                                                            if (chk_lstmenuname.Items[co].Selected == true)
                                                            {
                                                                ds.Tables[0].DefaultView.RowFilter = "SessionFK='" + chklstsession.Items[i].Value + "' and  MenuMasterFK='" + chk_lstmenuname.Items[co].Value + "' and  MessMasterFK in ('" + hostelcode + "') and DailyConsDate='" + dt.ToString("MM/dd/yyyy") + "' ";
                                                                dv = ds.Tables[0].DefaultView;
                                                                if (dv.Count > 0)
                                                                {
                                                                    for (int d = 0; d < dv.Count; d++)
                                                                    {
                                                                        if (!hat.Contains(Convert.ToString(dv[d]["Item_Code"])))
                                                                        {
                                                                            hat.Add(Convert.ToString(dv[d]["item_code"]), Convert.ToString(dv[d]["Consumption_Qty"]));
                                                                            hatname.Add(Convert.ToString(dv[d]["item_code"]), Convert.ToString(dv[d]["item_name"]));
                                                                            hatvalue.Add(Convert.ToString(dv[d]["item_code"]), Convert.ToString(dv[d]["Consumption_Value"]));
                                                                        }
                                                                        else
                                                                        {
                                                                            string quantity = Convert.ToString(hat[Convert.ToString(dv[d]["item_code"])]);
                                                                            if (quantity.Trim() != "")
                                                                            {
                                                                                double dq = Convert.ToDouble(quantity);
                                                                                string value = Convert.ToString(dv[d]["Consumption_Qty"]);
                                                                                if (value.Trim() != "")
                                                                                {
                                                                                    dq = dq + Convert.ToDouble(value);
                                                                                }
                                                                                hat.Remove(Convert.ToString(dv[d]["item_code"]));
                                                                                hat.Add(Convert.ToString(dv[d]["item_code"]), Convert.ToString(dq));
                                                                            }
                                                                            string quantity1 = Convert.ToString(hatvalue[Convert.ToString(dv[d]["item_code"])]);
                                                                            if (quantity1.Trim() != "")
                                                                            {
                                                                                double dq = Convert.ToDouble(quantity1);
                                                                                string value = Convert.ToString(dv[d]["Consumption_Value"]);
                                                                                if (value.Trim() != "")
                                                                                {
                                                                                    dq = dq + Convert.ToDouble(value);
                                                                                }
                                                                                hatvalue.Remove(Convert.ToString(dv[d]["item_code"]));
                                                                                hatvalue.Add(Convert.ToString(dv[d]["item_code"]), Convert.ToString(dq));
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Direct Consumption
                                            ds.Tables[0].DefaultView.RowFilter = " MessMasterFK in ('" + hostelcode + "') and DailyConsDate='" + dt.ToString("MM/dd/yyyy") + "' ";
                                            dv = ds.Tables[0].DefaultView;
                                            if (dv.Count > 0)
                                            {
                                                for (int d = 0; d < dv.Count; d++)
                                                {
                                                    if (!hat.Contains(Convert.ToString(dv[d]["Item_Code"])))
                                                    {
                                                        hat.Add(Convert.ToString(dv[d]["item_code"]), Convert.ToString(dv[d]["Consumption_Qty"]));
                                                        hatname.Add(Convert.ToString(dv[d]["item_code"]), Convert.ToString(dv[d]["item_name"]));
                                                        hatvalue.Add(Convert.ToString(dv[d]["item_code"]), Convert.ToString(dv[d]["Consumption_Value"]));
                                                    }
                                                    else
                                                    {
                                                        string quantity = Convert.ToString(hat[Convert.ToString(dv[d]["item_code"])]);
                                                        if (quantity.Trim() != "")
                                                        {
                                                            double dq = Convert.ToDouble(quantity);
                                                            string value = Convert.ToString(dv[d]["Consumption_Qty"]);
                                                            if (value.Trim() != "")
                                                            {
                                                                dq = dq + Convert.ToDouble(value);
                                                            }
                                                            hat.Remove(Convert.ToString(dv[d]["item_code"]));
                                                            hat.Add(Convert.ToString(dv[d]["item_code"]), Convert.ToString(dq));
                                                        }
                                                        string quantity1 = Convert.ToString(hatvalue[Convert.ToString(dv[d]["item_code"])]);
                                                        if (quantity1.Trim() != "")
                                                        {
                                                            double dq = Convert.ToDouble(quantity1);
                                                            string value = Convert.ToString(dv[d]["Consumption_Value"]);
                                                            if (value.Trim() != "")
                                                            {
                                                                dq = dq + Convert.ToDouble(value);
                                                            }
                                                            hatvalue.Remove(Convert.ToString(dv[d]["item_code"]));
                                                            hatvalue.Add(Convert.ToString(dv[d]["item_code"]), Convert.ToString(dq));
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion

                                        }
                                        if (rdbquantity.Checked == true)
                                        {
                                            if (hat.Count > 0)
                                            {
                                                int row = 0;
                                                dr = data.NewRow();
                                                foreach (DictionaryEntry par in hat)
                                                {
                                                    string columnname = Convert.ToString(hatname[Convert.ToString(par.Key)]);
                                                    string datacolumname = Convert.ToString(addindex[Convert.ToString(columnname)]);
                                                    if (datacolumname.Trim() != "")
                                                    {
                                                        dr[Convert.ToInt32(datacolumname)] = Convert.ToString(par.Value);
                                                    }
                                                    else
                                                    {
                                                        dr[row] = "0";
                                                    }
                                                    row++;
                                                }
                                                data.Rows.Add(dr);
                                            }
                                            hat.Clear();
                                            hatname.Clear();
                                            hatvalue.Clear();
                                        }
                                        if (rdbValue.Checked == true)
                                        {
                                            if (hatvalue.Count > 0)
                                            {
                                                int row = 0;
                                                dr = data.NewRow();
                                                foreach (DictionaryEntry par in hatvalue)
                                                {
                                                    string columnname = Convert.ToString(hatname[Convert.ToString(par.Key)]);
                                                    string datacolumname = Convert.ToString(addindex[Convert.ToString(columnname)]);
                                                    if (datacolumname.Trim() != "")
                                                    {
                                                        dr[Convert.ToInt32(datacolumname)] = Convert.ToString(par.Value);
                                                    }
                                                    else
                                                    {
                                                        dr[row] = "0";
                                                    }
                                                    row++;
                                                }
                                                data.Rows.Add(dr);
                                            }
                                            hat.Clear();
                                            hatname.Clear();
                                            hatvalue.Clear();
                                        }
                                    }
                                    dt = dt.AddDays(1);
                                }
                                if (rdbquantity.Checked == true)
                                {
                                    if (data.Rows.Count > 0)
                                    {
                                        for (int chart_i = 0; chart_i < data.Columns.Count; chart_i++)
                                        {
                                            for (int chart_j = 0; chart_j < data.Rows.Count; chart_j++)
                                            {
                                                string subnncode = Convert.ToString(data.Columns[chart_i]);
                                                string m1 = data.Rows[chart_j][chart_i].ToString();
                                                Chart1.Series[chart_j].Points.AddXY(subnncode, m1);
                                                //   Chart1.ChartAreas[0].AxisX.TextOrientation=TextOrientation.                                           
                                                Chart1.Series[chart_j].IsValueShownAsLabel = true;
                                                Chart1.Series[chart_j].IsXValueIndexed = true;
                                            }
                                        }
                                        Chart1.Visible = true;
                                        Chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                        Chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                        lblerror.Visible = false;
                                    }
                                }
                                if (rdbValue.Checked == true)
                                {
                                    if (data.Rows.Count > 0)
                                    {
                                        for (int chart_i = 0; chart_i < data.Columns.Count; chart_i++)
                                        {
                                            for (int chart_j = 0; chart_j < data.Rows.Count; chart_j++)
                                            {
                                                string subnncode = Convert.ToString(data.Columns[chart_i]);
                                                string m1 = data.Rows[chart_j][chart_i].ToString();
                                                Chart1.Series[chart_j].Points.AddXY(subnncode, m1);
                                                Chart1.Series[chart_j].IsValueShownAsLabel = true;
                                                Chart1.Series[chart_j].IsXValueIndexed = true;
                                            }
                                        }
                                        Chart1.Visible = true;
                                        Chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                        Chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                        lblerror.Visible = false;
                                    }
                                }
                            }
                        }
                        if (rdoweek.Checked == true)
                        {
                            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                            {
                                for (int rs = 0; rs < ds.Tables[1].Rows.Count; rs++)
                                {
                                    data.Columns.Add(Convert.ToString(ds.Tables[1].Rows[rs]["item_name"]));
                                    addindex.Add(Convert.ToString(ds.Tables[1].Rows[rs]["item_name"]), rs);
                                }
                                Chart1.Series.Clear();
                                DataTable newdata = new DataTable();
                                int week = 0;
                                while (dt <= dt1)
                                {
                                    week++;
                                    //14.10.15
                                    if (week == 1)
                                    {
                                        Chart1.Series.Add("" + week + "st Week" + " " + dt.ToString("dd/MM") + "-" + dt.AddDays(6).ToString("dd/MM"));
                                    }
                                    else if (week == 2)
                                    {
                                        Chart1.Series.Add("" + week + "nd Week" + " " + dt.ToString("dd/MM") + "-" + dt.AddDays(6).ToString("dd/MM"));
                                    }
                                    else if (week == 3)
                                    {
                                        Chart1.Series.Add("" + week + "rd Week" + " " + dt.ToString("dd/MM") + "-" + dt.AddDays(6).ToString("dd/MM"));
                                    }
                                    else
                                    {
                                        Chart1.Series.Add("" + week + "th Week" + " " + dt.ToString("dd/MM") + "-" + dt.AddDays(6).ToString("dd/MM"));
                                    }

                                    Chart1.Series[0].BorderWidth = 2;
                                    string betweenvalue = "DailyConsDate >= '" + dt.ToString("MM/dd/yyyy") + "' and DailyConsDate <= '" + dt.AddDays(6).ToString("MM/dd/yyyy") + "' ";
                                    ds.Tables[0].DefaultView.RowFilter = "" + betweenvalue + "";
                                    dv1 = ds.Tables[0].DefaultView;
                                    if (dv1.Count > 0)
                                    {
                                        newdata = dv1.ToTable();
                                        dv = new DataView(newdata);
                                        for (int rs = 0; rs < ds.Tables[1].Rows.Count; rs++)
                                        {
                                            dv.RowFilter = "Item_Code='" + Convert.ToString(ds.Tables[1].Rows[rs]["Item_Code"]) + "'";
                                            if (dv.Count > 0)
                                            {
                                                newdata = dv.ToTable();
                                                double total = Convert.ToDouble(newdata.Compute("Sum(Consumption_Qty)", ""));
                                                double totalvalue = Convert.ToDouble(newdata.Compute("Sum(Consumption_Value)", ""));
                                                if (!hat.Contains(ds.Tables[1].Rows[rs]["Item_Code"]))
                                                {
                                                    hat.Add(Convert.ToString(ds.Tables[1].Rows[rs]["Item_Code"]), Convert.ToString(total));
                                                    hatname.Add(Convert.ToString(ds.Tables[1].Rows[rs]["Item_Code"]), Convert.ToString(ds.Tables[1].Rows[rs]["item_name"]));
                                                    hatvalue.Add(Convert.ToString(ds.Tables[1].Rows[rs]["Item_Code"]), Convert.ToString(totalvalue));
                                                }
                                            }
                                        }
                                        if (rdbquantity.Checked == true)
                                        {
                                            if (hat.Count > 0)
                                            {
                                                int row = 0;
                                                dr = data.NewRow();
                                                foreach (DictionaryEntry par in hat)
                                                {
                                                    string columnname = Convert.ToString(hatname[Convert.ToString(par.Key)]);
                                                    string datacolumname = Convert.ToString(addindex[Convert.ToString(columnname)]);
                                                    if (datacolumname.Trim() != "")
                                                    {
                                                        dr[Convert.ToInt32(datacolumname)] = Convert.ToString(par.Value);
                                                    }
                                                    else
                                                    {
                                                        dr[row] = "0";
                                                    }
                                                    row++;
                                                }
                                                data.Rows.Add(dr);
                                            }
                                            hat.Clear();
                                            hatname.Clear();
                                            hatvalue.Clear();
                                        }
                                        if (rdbValue.Checked == true)
                                        {
                                            if (hatvalue.Count > 0)
                                            {
                                                int row = 0;
                                                dr = data.NewRow();
                                                foreach (DictionaryEntry par in hatvalue)
                                                {
                                                    string columnname = Convert.ToString(hatname[Convert.ToString(par.Key)]);
                                                    string datacolumname = Convert.ToString(addindex[Convert.ToString(columnname)]);
                                                    if (datacolumname.Trim() != "")
                                                    {
                                                        dr[Convert.ToInt32(datacolumname)] = Convert.ToString(par.Value);
                                                    }
                                                    else
                                                    {
                                                        dr[row] = "0";
                                                    }
                                                    row++;
                                                }
                                                data.Rows.Add(dr);
                                            }
                                            hat.Clear();
                                            hatname.Clear();
                                            hatvalue.Clear();
                                        }

                                    }
                                    else
                                    {
                                        if (data.Columns.Count > 0)
                                        {
                                            dr = data.NewRow();
                                            for (int row = 0; row < data.Columns.Count; row++)
                                            {
                                                dr[row] = "0";
                                            }
                                            data.Rows.Add(dr);
                                        }
                                    }
                                    dt = dt.AddDays(7);
                                }
                                if (rdbquantity.Checked == true)
                                {

                                    if (data.Rows.Count > 0)
                                    {
                                        for (int chart_i = 0; chart_i < data.Columns.Count; chart_i++)
                                        {
                                            for (int chart_j = 0; chart_j < data.Rows.Count; chart_j++)
                                            {
                                                string subnncode = Convert.ToString(data.Columns[chart_i]);
                                                string m1 = data.Rows[chart_j][chart_i].ToString();
                                                Chart1.Series[chart_j].Points.AddXY(subnncode, m1);
                                                Chart1.Series[chart_j].IsValueShownAsLabel = true;
                                                Chart1.Series[chart_j].IsXValueIndexed = true;
                                            }
                                        }
                                        Chart1.Visible = true;
                                        Chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                        Chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                        lblerror.Visible = false;
                                    }
                                }
                                if (rdbValue.Checked == true)
                                {
                                    if (data.Rows.Count > 0)
                                    {
                                        for (int chart_i = 0; chart_i < data.Columns.Count; chart_i++)
                                        {
                                            for (int chart_j = 0; chart_j < data.Rows.Count; chart_j++)
                                            {
                                                string subnncode = Convert.ToString(data.Columns[chart_i]);
                                                string m1 = data.Rows[chart_j][chart_i].ToString();
                                                Chart1.Series[chart_j].Points.AddXY(subnncode, m1);
                                                Chart1.Series[chart_j].IsValueShownAsLabel = true;
                                                Chart1.Series[chart_j].IsXValueIndexed = true;
                                            }
                                        }
                                        Chart1.Visible = true;
                                        Chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                        Chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                        lblerror.Visible = false;
                                    }
                                }
                            }
                        }
                        if (rdomonth.Checked == true)
                        {
                            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                            {
                                for (int rs = 0; rs < ds.Tables[1].Rows.Count; rs++)
                                {
                                    data.Columns.Add(Convert.ToString(ds.Tables[1].Rows[rs]["item_name"]));
                                    addindex.Add(Convert.ToString(ds.Tables[1].Rows[rs]["item_name"]), rs);
                                }
                                Chart1.Series.Clear();
                                DataTable newdata = new DataTable();
                                int week = 0;
                                while (dt <= dt1)
                                {
                                    week++;
                                    int date = Convert.ToInt32(dt.ToString("dd"));
                                    int noofdays = DateTime.DaysInMonth(Convert.ToInt32(dt.ToString("yyyy")), Convert.ToInt32(dt.ToString("MM")));
                                    int remainday = noofdays - date;
                                    Chart1.Series.Add("" + week + "st Month" + " " + dt.ToString("dd/MM") + "-" + dt.AddDays(remainday).ToString("dd/MM"));
                                    Chart1.Series[0].BorderWidth = 2;
                                    string betweenvalue = "DailyConsDate >= '" + dt.ToString("MM/dd/yyyy") + "' and DailyConsDate <= '" + dt.AddDays(remainday).ToString("MM/dd/yyyy") + "' ";
                                    ds.Tables[0].DefaultView.RowFilter = "" + betweenvalue + "";
                                    dv1 = ds.Tables[0].DefaultView;
                                    if (dv1.Count > 0)
                                    {
                                        newdata = dv1.ToTable();
                                        dv = new DataView(newdata);
                                        for (int rs = 0; rs < ds.Tables[1].Rows.Count; rs++)
                                        {
                                            dv.RowFilter = "Item_Code='" + Convert.ToString(ds.Tables[1].Rows[rs]["Item_Code"]) + "'";
                                            if (dv.Count > 0)
                                            {
                                                newdata = dv.ToTable();
                                                double total = Convert.ToDouble(newdata.Compute("Sum(Consumption_Qty)", ""));
                                                double totalvalue = Convert.ToDouble(newdata.Compute("Sum(Consumption_Value)", ""));
                                                if (!hat.Contains(ds.Tables[1].Rows[rs]["Item_Code"]))
                                                {
                                                    hat.Add(Convert.ToString(ds.Tables[1].Rows[rs]["Item_Code"]), Convert.ToString(total));
                                                    hatname.Add(Convert.ToString(ds.Tables[1].Rows[rs]["Item_Code"]), Convert.ToString(ds.Tables[1].Rows[rs]["item_name"]));
                                                    hatvalue.Add(Convert.ToString(ds.Tables[1].Rows[rs]["Item_Code"]), Convert.ToString(totalvalue));
                                                }
                                            }
                                        }
                                        if (rdbquantity.Checked == true)
                                        {
                                            if (hat.Count > 0)
                                            {
                                                int row = 0;
                                                dr = data.NewRow();
                                                foreach (DictionaryEntry par in hat)
                                                {
                                                    string columnname = Convert.ToString(hatname[Convert.ToString(par.Key)]);
                                                    string datacolumname = Convert.ToString(addindex[Convert.ToString(columnname)]);
                                                    if (datacolumname.Trim() != "")
                                                    {
                                                        dr[Convert.ToInt32(datacolumname)] = Convert.ToString(par.Value);
                                                    }
                                                    else
                                                    {
                                                        dr[row] = "0";
                                                    }
                                                    row++;
                                                }
                                                data.Rows.Add(dr);
                                            }
                                            hat.Clear();
                                            hatname.Clear();
                                            hatvalue.Clear();
                                        }
                                        if (rdbValue.Checked == true)
                                        {
                                            if (hatvalue.Count > 0)
                                            {
                                                int row = 0;
                                                dr = data.NewRow();
                                                foreach (DictionaryEntry par in hatvalue)
                                                {
                                                    string columnname = Convert.ToString(hatname[Convert.ToString(par.Key)]);
                                                    string datacolumname = Convert.ToString(addindex[Convert.ToString(columnname)]);
                                                    if (datacolumname.Trim() != "")
                                                    {
                                                        dr[Convert.ToInt32(datacolumname)] = Convert.ToString(par.Value);
                                                    }
                                                    else
                                                    {
                                                        dr[row] = "0";
                                                    }
                                                    row++;
                                                }
                                                data.Rows.Add(dr);
                                            }
                                            hat.Clear();
                                            hatname.Clear();
                                            hatvalue.Clear();
                                        }

                                    }
                                    else
                                    {
                                        if (data.Columns.Count > 0)
                                        {
                                            dr = data.NewRow();
                                            for (int row = 0; row < data.Columns.Count; row++)
                                            {
                                                dr[row] = "0";
                                            }
                                            data.Rows.Add(dr);
                                        }
                                    }
                                    dt = dt.AddDays(remainday + 1);
                                }
                                if (rdbquantity.Checked == true)
                                {

                                    if (data.Rows.Count > 0)
                                    {
                                        for (int chart_i = 0; chart_i < data.Columns.Count; chart_i++)
                                        {
                                            for (int chart_j = 0; chart_j < data.Rows.Count; chart_j++)
                                            {
                                                string subnncode = Convert.ToString(data.Columns[chart_i]);
                                                string m1 = data.Rows[chart_j][chart_i].ToString();
                                                Chart1.Series[chart_j].Points.AddXY(subnncode, m1);
                                                Chart1.Series[chart_j].IsValueShownAsLabel = true;
                                                Chart1.Series[chart_j].IsXValueIndexed = true;
                                            }
                                        }
                                        Chart1.Visible = true;
                                        Chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                        Chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                        lblerror.Visible = false;
                                    }
                                }
                                if (rdbValue.Checked == true)
                                {
                                    if (data.Rows.Count > 0)
                                    {
                                        for (int chart_i = 0; chart_i < data.Columns.Count; chart_i++)
                                        {
                                            for (int chart_j = 0; chart_j < data.Rows.Count; chart_j++)
                                            {
                                                string subnncode = Convert.ToString(data.Columns[chart_i]);
                                                string m1 = data.Rows[chart_j][chart_i].ToString();
                                                Chart1.Series[chart_j].Points.AddXY(subnncode, m1);
                                                Chart1.Series[chart_j].IsValueShownAsLabel = true;
                                                Chart1.Series[chart_j].IsXValueIndexed = true;
                                            }
                                        }
                                        Chart1.Visible = true;
                                        Chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                        Chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                        lblerror.Visible = false;
                                    }
                                }
                            }
                        }
                        /* day comparsion*/
                        ArrayList daysvalue = new ArrayList();
                        ArrayList datevalue = new ArrayList();
                        if (rdodaycom.Checked == true)
                        {
                            if (chklstdaycompar.Items.Count > 0)
                            {
                                for (int rs = 0; rs < chklstdaycompar.Items.Count; rs++)
                                {
                                    if (chklstdaycompar.Items[rs].Selected == true)
                                    {
                                        daysvalue.Add(chklstdaycompar.Items[rs].Text);
                                    }
                                }
                            }
                            if (daysvalue.Count > 0)
                            {
                                DateTime dn = dt;
                                while (dn <= dt1)
                                {
                                    if (Convert.ToString(daysvalue[0]) == Convert.ToString(dn.ToString("dddd")))
                                    {
                                        datevalue.Add(Convert.ToString(dn.ToString("MM/dd/yyyy")));
                                    }
                                    dn = dn.AddDays(1);
                                }
                                Chart1.Series.Clear();
                                if (datevalue.Count > 0)
                                {
                                    DataTable newdata = new DataTable();
                                    for (int rs = 0; rs < ds.Tables[1].Rows.Count; rs++)
                                    {
                                        data.Columns.Add(Convert.ToString(ds.Tables[1].Rows[rs]["item_name"]));
                                        addindex.Add(Convert.ToString(ds.Tables[1].Rows[rs]["item_name"]), rs);
                                    }
                                    for (int row1 = 0; row1 < datevalue.Count; row1++)
                                    {
                                        string betweenvalue = "DailyConsDate = '" + Convert.ToString(datevalue[row1]) + "'";
                                        ds.Tables[0].DefaultView.RowFilter = "" + betweenvalue + "";
                                        dv1 = ds.Tables[0].DefaultView;
                                        DataView dvnew = new DataView();
                                        if (dv1.Count > 0)
                                        {
                                            double newstrenth = 0;
                                            if (ds.Tables[2].Rows.Count > 0)
                                            {
                                                ds.Tables[2].DefaultView.RowFilter = " DailyConsDate='" + Convert.ToString(datevalue[row1]) + "'";
                                                dvnew = ds.Tables[2].DefaultView;
                                                if (dvnew.Count > 0)
                                                {
                                                    DataTable dt_table = new DataTable();
                                                    dt_table = dvnew.ToTable();
                                                    string strengh = Convert.ToString(dt_table.Compute("Sum(Total_Present)", "")); ;
                                                    if (strengh.Trim() != "")
                                                    {
                                                        newstrenth = Convert.ToDouble(strengh);
                                                    }
                                                }
                                            }

                                            Chart1.Series.Add(Convert.ToString(datevalue[row1] + "-" + Convert.ToDateTime(datevalue[row1]).ToString("dddd") + "(" + newstrenth + ")"));
                                            Chart1.Series[0].BorderWidth = 2;
                                            newdata = dv1.ToTable();
                                            dv = new DataView(newdata);
                                            dr = data.NewRow();
                                            int row = 0;
                                            for (int rs = 0; rs < ds.Tables[1].Rows.Count; rs++)
                                            {
                                                row++;
                                                dv.RowFilter = "Item_Code='" + Convert.ToString(ds.Tables[1].Rows[rs]["Item_Code"]) + "'";
                                                if (dv.Count > 0)
                                                {
                                                    newdata = dv.ToTable();
                                                    double total = Convert.ToDouble(newdata.Compute("Sum(Consumption_Qty)", ""));
                                                    double totalvalue = Convert.ToDouble(newdata.Compute("Sum(Consumption_Value)", ""));
                                                    if (rdbValue.Checked == true)
                                                    {
                                                        dr[row - 1] = Convert.ToString(totalvalue);
                                                    }
                                                    else
                                                    {
                                                        dr[row - 1] = Convert.ToString(total);
                                                    }
                                                }
                                                else
                                                {
                                                    dr[row - 1] = "0";
                                                }
                                            }
                                            data.Rows.Add(dr);
                                        }
                                    }
                                    if (rdbquantity.Checked == true)
                                    {

                                        if (data.Rows.Count > 0)
                                        {
                                            for (int chart_i = 0; chart_i < data.Columns.Count; chart_i++)
                                            {
                                                for (int chart_j = 0; chart_j < data.Rows.Count; chart_j++)
                                                {
                                                    string subnncode = Convert.ToString(data.Columns[chart_i]);
                                                    string m1 = data.Rows[chart_j][chart_i].ToString();
                                                    Chart1.Series[chart_j].Points.AddXY(subnncode, m1);
                                                    Chart1.Series[chart_j].IsValueShownAsLabel = true;
                                                    Chart1.Series[chart_j].IsXValueIndexed = true;
                                                }
                                            }
                                            Chart1.Visible = true;
                                            Chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                            Chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                            lblerror.Visible = false;
                                        }
                                        else
                                        {
                                            lblerror.Visible = true;
                                            lblerror.Text = "No Records Found";
                                            Chart1.Visible = false;
                                        }
                                    }
                                    if (rdbValue.Checked == true)
                                    {
                                        if (data.Rows.Count > 0)
                                        {
                                            for (int chart_i = 0; chart_i < data.Columns.Count; chart_i++)
                                            {
                                                for (int chart_j = 0; chart_j < data.Rows.Count; chart_j++)
                                                {
                                                    string subnncode = Convert.ToString(data.Columns[chart_i]);
                                                    string m1 = data.Rows[chart_j][chart_i].ToString();
                                                    Chart1.Series[chart_j].Points.AddXY(subnncode, m1);
                                                    Chart1.Series[chart_j].IsValueShownAsLabel = true;
                                                    Chart1.Series[chart_j].IsXValueIndexed = true;
                                                }
                                            }
                                            Chart1.Visible = true;
                                            Chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                            Chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                            lblerror.Visible = false;
                                        }
                                        else
                                        {
                                            lblerror.Visible = true;
                                            lblerror.Text = "No Records Found";
                                            Chart1.Visible = false;
                                        }
                                    }
                                }
                                else
                                {
                                    lblerror.Visible = true;
                                    lblerror.Text = "No Records Found";
                                    Chart1.Visible = false;

                                }

                            }
                            else
                            {
                                lblerror.Visible = true;
                                lblerror.Text = "Please Select the Days";
                                Chart1.Visible = false;
                            }

                        }
                        /*end*/
                        // lblerror.Visible = false;
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "No Records Found";
                        Chart1.Visible = false;

                    }
                }
                else
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select All Fields";
                    Chart1.Visible = false;
                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select All Fields";
                Chart1.Visible = false;
            }
        }
        catch
        {

        }
    }

    public void bindpop2session()
    {
        try
        {
            ds.Clear();
            string itemheadercode = "";
            chklst_pop2session.Items.Clear();
            for (int i = 0; i < cblpop2hostel.Items.Count; i++)
            {
                if (cblpop2hostel.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cblpop2hostel.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cblpop2hostel.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemheadercode != "")
            {
                ds.Clear();
                ds = d2.BindSession_inv(itemheadercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklst_pop2session.DataSource = ds;
                    chklst_pop2session.DataTextField = "SessionName";
                    chklst_pop2session.DataValueField = "SessionMasterPK";
                    chklst_pop2session.DataBind();
                    if (chklst_pop2session.Items.Count > 0)
                    {
                        for (int i = 0; i < chklst_pop2session.Items.Count; i++)
                        {
                            chklst_pop2session.Items[i].Selected = true;
                        }
                        txtpop2sessionname.Text = "Session Name(" + chklst_pop2session.Items.Count + ")";
                    }
                }
                else
                {
                    txtpop2sessionname.Text = "--Select--";
                }
            }
            else
            {
                txtpop2sessionname.Text = "--Select--";
            }

        }
        catch
        {

        }
    }
    public void loadmenuname()
    {
        try
        {
            chk_lstmenuname.Items.Clear();
            txtmenuname.Text = "--Select--";
            string itemheadercode = "";
            for (int i = 0; i < chklstsession.Items.Count; i++)
            {
                if (chklstsession.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + chklstsession.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + chklstsession.Items[i].Value.ToString() + "";
                    }
                }
            }

            string hostel = "";
            for (int i = 0; i < chklsthostelname.Items.Count; i++)
            {
                if (chklsthostelname.Items[i].Selected == true)
                {
                    if (hostel == "")
                    {
                        hostel = "" + chklsthostelname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        hostel = hostel + "'" + "," + "'" + chklsthostelname.Items[i].Value.ToString() + "";
                    }
                }
            }
            string hostelcode = "";
            if (hostel.Trim() != "")
            {
                hostelcode = Convert.ToString(hostel);
            }
            string fromdate = Convert.ToString(txtfromdate.Text);
            DateTime dt = new DateTime();
            string[] split = fromdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            DateTime dn = dt;
            string todate = Convert.ToString(txttodate.Text);
            DateTime dt1 = new DateTime();
            string[] split1 = todate.Split('/');
            dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
            string days = "";
            while (dt <= dt1)
            {
                if (days == "")
                {
                    days = Convert.ToString(dt.ToString("dddd"));
                }
                else
                {
                    days = days + "'" + "," + "'" + Convert.ToString(dt.ToString("dddd"));
                }
                dt = dt.AddDays(1);
            }
            if (itemheadercode.Trim() != "" && hostelcode.Trim() != "")
            {
                string menuquery = "";
                menuquery = "select MenuMasterFK,SessionMasterFK from HT_MenuSchedule where SessionMasterFK in('" + itemheadercode + "') and MessMasterFK in('" + hostelcode + "')  and ScheudleItemType='1' and ScheduleType ='1' and MenuScheduleDate between '" + dn.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'";
                menuquery = menuquery + "  select MenuMasterFK,SessionMasterFK from HT_MenuSchedule where SessionMasterFK in('" + itemheadercode + "') and MessMasterFK in('" + hostelcode + "')  and ScheudleItemType='1' and ScheduleType ='2' and MenuScheduleday  in('" + days + "')";
                ds.Clear();
                ds = d2.select_method_wo_parameter(menuquery, "Text");
                menuquery = "";
                string menucode = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        string mcode = Convert.ToString(ds.Tables[0].Rows[row]["MenuMasterFK"]);
                        if (menucode.Contains(mcode) == false)
                        {
                            if (menucode == "")
                            {
                                menucode = mcode;
                            }
                            else
                            {
                                menucode = menucode + "'" + "," + "'" + mcode;
                            }
                        }
                    }
                }
                else
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int row = 0; row < ds.Tables[1].Rows.Count; row++)
                        {
                            string mcode = Convert.ToString(ds.Tables[1].Rows[row]["MenuMasterFK"]);
                            if (menucode.Contains(mcode) == false)
                            {
                                if (menucode == "")
                                {
                                    menucode = mcode;
                                }
                                else
                                {
                                    menucode = menucode + "'" + "," + "'" + mcode;
                                }
                            }
                        }
                    }
                }
                string deptquery = "select distinct MenuMasterPK,MenuName from HM_MenuMaster where CollegeCode ='" + collegecode1 + "' and MenuMasterPK in('" + menucode + "')  order by MenuName";
                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chk_lstmenuname.DataSource = ds;
                    chk_lstmenuname.DataTextField = "MenuName";
                    chk_lstmenuname.DataValueField = "MenuMasterPK";
                    chk_lstmenuname.DataBind();
                    if (chk_lstmenuname.Items.Count > 0)
                    {
                        for (int i = 0; i < chk_lstmenuname.Items.Count; i++)
                        {
                            chk_lstmenuname.Items[i].Selected = true;
                        }
                        txtmenuname.Text = "Menu Name(" + chk_lstmenuname.Items.Count + ")";
                    }
                }
            }
        }
        catch
        {

        }
    }
    protected void itempurchasehty()
    {

    }
    protected void chkpop2session_checkedchange(object sender, EventArgs e)
    {
        if (chkpop2session.Checked == true)
        {
            for (int i = 0; i < chklst_pop2session.Items.Count; i++)
            {
                chklst_pop2session.Items[i].Selected = true;
            }
            txtpop2sessionname.Text = "Session Name(" + (chklst_pop2session.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chklst_pop2session.Items.Count; i++)
            {
                chklst_pop2session.Items[i].Selected = false;
            }
            txtpop2sessionname.Text = "--Select--";
        }
        loadpop2menuname();

    }
    protected void chklst_pop2session_Change(object sender, EventArgs e)
    {
        txtpop2sessionname.Text = "--Select--";
        chkpop2session.Checked = false;
        int commcount = 0;
        for (int i = 0; i < chklst_pop2session.Items.Count; i++)
        {
            if (chklst_pop2session.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtpop2sessionname.Text = "Session Name(" + commcount.ToString() + ")";
            if (commcount == chklst_pop2session.Items.Count)
            {
                chkpop2session.Checked = true;
            }
        }
        loadmenuname();
    }



    protected void chkpop2menuname_Change(object sender, EventArgs e)
    {
        if (chkpop2menuname.Checked == true)
        {
            for (int i = 0; i < chklst_pop2menuname.Items.Count; i++)
            {
                chklst_pop2menuname.Items[i].Selected = true;
            }
            txtpop2menuname.Text = "Menu Name(" + (chklst_pop2menuname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chklst_pop2menuname.Items.Count; i++)
            {
                chklst_pop2menuname.Items[i].Selected = false;
            }
            txtpop2menuname.Text = "--Select--";
        }
    }
    protected void chk_lstpop2menuname_Change(object sender, EventArgs e)
    {
        txtpop2menuname.Text = "--Select--";
        chkmenuname.Checked = false;
        int commcount = 0;
        for (int i = 0; i < chklst_pop2menuname.Items.Count; i++)
        {
            if (chklst_pop2menuname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtpop2sessionname.Text = "Menu Name(" + commcount.ToString() + ")";
            if (commcount == chklst_pop2menuname.Items.Count)
            {
                chkpop2menuname.Checked = true;
            }
        }
    }
    protected void bindpop2hostelname()
    {
        try
        {
            cblpop2hostel.Items.Clear();
            //string q = "select Hostel_code,Hostel_Name  from Hostel_Details order by Hostel_code";
            ds.Clear();
            //ds = d2.select_method_wo_parameter(q, "Text");//Idhris 10/10/2015
            // ds = d2.Bindmess_inv(collegecode1);
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {

                cblpop2hostel.DataSource = ds;
                //cblpop2hostel.DataTextField = "Hostel_Name";
                //cblpop2hostel.DataValueField = "Hostel_Code";
                cblpop2hostel.DataTextField = "MessName";
                cblpop2hostel.DataValueField = "MessMasterPK";
                cblpop2hostel.DataBind();
                if (cblpop2hostel.Items.Count > 0)
                {
                    for (int row = 0; row < cblpop2hostel.Items.Count; row++)
                    {
                        cblpop2hostel.Items[row].Selected = true;
                    }
                    txtpop2hostelname.Text = "Mess Name (" + cblpop2hostel.Items.Count + ")";
                }
                //ddlpopitemname.Items.Insert(ddlpopitemname.Items.Count, "Others");
            }

        }
        catch
        {

        }
    }
    protected void loadpop2menuname()
    {
        try
        {
            chklst_pop2menuname.Items.Clear();
            txtpop2menuname.Text = "--Select--";
            string itemheadercode = "";
            for (int i = 0; i < chklst_pop2session.Items.Count; i++)
            {
                if (chklst_pop2session.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + chklst_pop2session.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + chklst_pop2session.Items[i].Value.ToString() + "";
                    }
                }
            }
            string hostelcode = "";
            if (ddlpop2hosname.SelectedItem.Text != "Select")
            {
                hostelcode = Convert.ToString(ddlpop2hosname.SelectedItem.Value);
            }
            string fromdate = Convert.ToString(txtfromdate.Text);
            DateTime dt = new DateTime();
            string[] split = fromdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

            string todate = Convert.ToString(txttodate.Text);
            DateTime dt1 = new DateTime();
            string[] split1 = todate.Split('/');
            dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
            string days = "";
            while (dt <= dt1)
            {
                if (days == "")
                {
                    days = Convert.ToString(dt.ToString("dddd"));
                }
                else
                {
                    days = days + "'" + "," + "'" + Convert.ToString(dt.ToString("dddd"));
                }
                dt = dt.AddDays(1);
            }

            if (itemheadercode.Trim() != "" && hostelcode.Trim() != "")
            {
                string menuquery = "";
                menuquery = "select MenuMasterFK,SessionMasterFK from HT_MenuSchedule where SessionMasterFK in('" + itemheadercode + "') and MessMasterFK in('" + hostelcode + "')  and ScheudleItemType='1' and ScheduleType ='1' and MenuScheduleDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'";
                menuquery = menuquery + "  select MenuMasterFK,SessionMasterFK from HT_MenuSchedule where SessionMasterFK in('" + itemheadercode + "') and MessMasterFK in('" + hostelcode + "')  and ScheudleItemType='1' and ScheduleType ='2' and MenuScheduleday in('" + days + "')";
                ds.Clear();
                ds = d2.select_method_wo_parameter(menuquery, "Text");
                menuquery = "";
                string menucode = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        string mcode = Convert.ToString(ds.Tables[0].Rows[row]["MenuMasterFK"]);
                        if (menucode.Contains(mcode) == false)
                        {
                            if (menucode == "")
                            {
                                menucode = mcode;
                            }
                            else
                            {
                                menucode = menucode + "'" + "," + "'" + mcode;
                            }
                        }
                    }
                }
                else
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int row = 0; row < ds.Tables[1].Rows.Count; row++)
                        {
                            string mcode = Convert.ToString(ds.Tables[1].Rows[row]["MenuMasterFK"]);
                            if (menucode.Contains(mcode) == false)
                            {
                                if (menucode == "")
                                {
                                    menucode = mcode;
                                }
                                else
                                {
                                    menucode = menucode + "'" + "," + "'" + mcode;
                                }
                            }
                        }
                    }
                }
                string deptquery = "select distinct MenuMasterPK,MenuName from HM_MenuMaster where CollegeCode ='" + collegecode1 + "' and MenuMasterPK in('" + menucode + "')  order by MenuName";
                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklst_pop2menuname.DataSource = ds;
                    chklst_pop2menuname.DataTextField = "MenuName";
                    chklst_pop2menuname.DataValueField = "MenuMasterPK";
                    chklst_pop2menuname.DataBind();
                    if (chklst_pop2menuname.Items.Count > 0)
                    {
                        for (int i = 0; i < chklst_pop2menuname.Items.Count; i++)
                        {
                            chklst_pop2menuname.Items[i].Selected = true;
                        }
                        txtpop2menuname.Text = "Menu Name(" + chklst_pop2menuname.Items.Count + ")";
                    }
                }
            }
        }
        catch
        {

        }
    }
    protected void btnpop2go_Click(object sender, EventArgs e)
    {
        try
        {
            string itemheadercode = "";
            for (int i = 0; i < cblpop2hostel.Items.Count; i++)
            {
                if (cblpop2hostel.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cblpop2hostel.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cblpop2hostel.Items[i].Value.ToString() + "";
                    }
                }
            }
            DataRow dr;

            if (itemheadercode.Trim() != "")
            {
                string fromdate = Convert.ToString(txtpop2from.Text);
                string todate = Convert.ToString(txtpop2to.Text);
                string[] split = fromdate.Split('/');
                DateTime dt = new DateTime();
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = todate.Split('/');
                DateTime dt1 = new DateTime();
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

                string selectquery = " select pi.ItemFK,inwardqty,rpu,CONVERT(varchar(10), OrderDate ,103) as gi_date,(g.inwardqty*pi.RPU) value from IT_PurchaseOrder p ,IT_PurchaseOrderDetail pi,IT_GoodsInward g,IM_ItemMaster i where p.PurchaseOrderPK  =pi.PurchaseOrderFK  and pi.ItemFK  =g.itemfk and i.ItemPK =pi.ItemFK  and p.ApproveStatus ='1' and  p.OrderDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'";
                selectquery = selectquery + "  select distinct pi.ItemFK,i.itemname from IT_PurchaseOrder p ,IT_PurchaseOrderDetail pi,IT_GoodsInward g,IM_ItemMaster i where p.PurchaseOrderPK  =pi.PurchaseOrderFK  and pi.ItemFK =g.itemfk and  i.ItemPK =pi.ItemFK and ApproveStatus ='1' and p.OrderDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'";

                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    DateTime dn = dt;
                    Chart2.Series.Clear();
                    for (int row = 0; row < ds.Tables[1].Rows.Count; row++)
                    {
                        chartdata.Columns.Add(Convert.ToString(ds.Tables[1].Rows[row]["itemname"]));
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        while (dt <= dt1)
                        {
                            dr = chartdata.NewRow();
                            bool ac = false;
                            int c = 0;
                            for (int row = 0; row < ds.Tables[1].Rows.Count; row++)
                            {
                                c++;
                                ds.Tables[0].DefaultView.RowFilter = "gi_date='" + dt.ToString("dd/MM/yyyy") + "' and ItemFK='" + Convert.ToString(ds.Tables[1].Rows[row]["ItemFK"]) + "'";
                                dv = ds.Tables[0].DefaultView;
                                if (dv.Count > 0)
                                {
                                    if (ac == false)
                                    {
                                        Chart2.Series.Add(dn.ToString(Convert.ToString(dt.ToString("dd/MM/yyyy"))));
                                        Chart2.Series[0].BorderWidth = 2;
                                        ac = true;
                                    }
                                    checktable = dv.ToTable();
                                    double total = 0;
                                    if (rdopop2Qty.Checked == true)
                                    {
                                        total = Convert.ToDouble(checktable.Compute("Sum(inwardqty)", ""));
                                    }
                                    if (rdopop2value.Checked == true)
                                    {
                                        total = Convert.ToDouble(checktable.Compute("Sum(value)", ""));
                                    }
                                    dr[c - 1] = Convert.ToString(total);
                                }
                                else
                                {
                                    dr[c - 1] = "0";
                                }
                            }
                            if (ac == true)
                            {
                                chartdata.Rows.Add(dr);
                            }
                            dt = dt.AddDays(1);
                        }
                    }
                    if (chartdata.Rows.Count > 0)
                    {
                        for (int chart_i = 0; chart_i < chartdata.Columns.Count; chart_i++)
                        {
                            for (int chart_j = 0; chart_j < chartdata.Rows.Count; chart_j++)
                            {
                                string subnncode = Convert.ToString(chartdata.Columns[chart_i]);
                                string m1 = chartdata.Rows[chart_j][chart_i].ToString();
                                Chart2.Series[chart_j].Points.AddXY(subnncode, m1);
                                Chart2.Series[chart_j].IsValueShownAsLabel = true;
                                Chart2.Series[chart_j].IsXValueIndexed = true;
                            }
                        }
                        Chart2.Visible = true;
                    }
                    lblpop2error.Visible = false;
                }
                else
                {
                    lblpop2error.Visible = true;
                    Chart2.Visible = false;
                    lblpop2error.Text = "No Record Found";
                }
            }
        }
        catch
        {

        }

    }

    protected void btnpop3go_Click(object sender, EventArgs e)
    {
        lblpop3error.Visible = true;
        lblpop3error.Text = "No Record Found";

    }
    protected void ddlpop3hostelname_change(object sender, EventArgs e)
    {
        try
        {
            bindpop3session();
        }
        catch
        {
        }
    }
    protected void bindpop3session()
    {
        try
        {
            ds.Clear();
            if (ddlpop3hostel.SelectedItem.Value.Trim() != "")
            {
                ds = d2.BindSession_inv(ddlpop3hostel.SelectedItem.Value);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklstpop3session.DataSource = ds;
                    chklstpop3session.DataTextField = "SessionName";
                    chklstpop3session.DataValueField = "SessionMasterPK";
                    chklstpop3session.DataBind();
                    if (chklstpop3session.Items.Count > 0)
                    {
                        for (int i = 0; i < chklstpop3session.Items.Count; i++)
                        {
                            chklstpop3session.Items[i].Selected = true;
                        }
                        txtpop3session.Text = "Session Name(" + chklstpop3session.Items.Count + ")";
                    }
                }
                else
                {
                    txtpop3session.Text = "--Select--";
                }
            }
            else
            {
                txtpop3session.Text = "--Select--";
            }

        }
        catch
        {

        }
    }
    protected void chkpop3session_checkedchange(object sender, EventArgs e)
    {
        if (chkpop3session.Checked == true)
        {
            for (int i = 0; i < chklstpop3session.Items.Count; i++)
            {
                chklstpop3session.Items[i].Selected = true;
            }
            txtpop3session.Text = "Session Name(" + chklstpop3session.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklstpop3session.Items.Count; i++)
            {
                chklstpop3session.Items[i].Selected = false;
            }
            txtpop3session.Text = "--Select--";
        }

    }
    protected void chklst_pop3session_Change(object sender, EventArgs e)
    {
        txtpop3session.Text = "--Select--";
        chkpop3session.Checked = false;
        int ccount = 0;
        for (int i = 0; i < chklstpop3session.Items.Count; i++)
        {
            if (chklstpop3session.Items[i].Selected == true)
            {
                ccount = ccount + 1;
                chkpop3session.Checked = false;
            }
        }
        if (ccount > 0)
        {
            txtpop3session.Text = "Session Name(" + ccount.ToString() + ")";
            if (ccount == chklstpop3session.Items.Count)
            {
                chkpop3session.Checked = true;
            }
        }

    }
    protected void bindpop3hostelname()
    {
        try
        {
            ddlpop3hostel.Items.Clear();
            ds.Clear();
            // ds = d2.Bindmess_inv(collegecode1);
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlpop3hostel.DataSource = ds;
                //ddlpop3hostel.DataTextField = "Hostel_Name";
                //ddlpop3hostel.DataValueField = "Hostel_Code";
                ddlpop3hostel.DataTextField = "MessName";
                ddlpop3hostel.DataValueField = "MessMasterPK";
                ddlpop3hostel.DataBind();
                ddlpop3hostel.Items.Insert(0, "Select");
                //ddlpopitemname.Items.Insert(ddlpopitemname.Items.Count, "Others");
            }
            else
            {
                ddlpop3hostel.Items.Insert(0, "Select");
                //ddlpopitemname.Items.Insert(ddlpopitemname.Items.Count, "Others");
            }
        }
        catch
        {

        }
    }

    /*pop4*/
    protected void ddlpop4hostelname_change(object sender, EventArgs e)
    {
        try
        {
            bindpop4session();
            bindpop4menuname();
        }
        catch
        {

        }


    }
    protected void chkpop4session_checkedchange(object sender, EventArgs e)
    {
        if (chkpop4session.Checked == true)
        {
            for (int i = 0; i < chklstpop4session.Items.Count; i++)
            {
                chklstpop4session.Items[i].Selected = true;
            }
            txtpop4session.Text = "Session Name(" + chklstpop4session.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklstpop4session.Items.Count; i++)
            {
                chklstpop4session.Items[i].Selected = false;
            }
            txtpop4session.Text = "--Select--";
        }
    }
    protected void bindpop4menuname()
    {
        try
        {
            chklstpop4menuname.Items.Clear();
            txtpop4menuname.Text = "--Select--";
            //string itemheadercode = "";
            string itemheadercode = Convert.ToString(ddlpop4hostel.SelectedItem.Value);
            string hostelcode = "";

            for (int i = 0; i < chklstpop4session.Items.Count; i++)
            {
                if (chklstpop4session.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + chklstpop4session.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + chklstpop4session.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (ddlpop4hostel.SelectedItem.Text != "Select")
            {
                hostelcode = Convert.ToString(ddlpop4hostel.SelectedItem.Value);
            }
            string firstdate = Convert.ToString(txtpop4from.Text);
            DateTime dt = new DateTime();
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            if (itemheadercode.Trim() != "" && hostelcode.Trim() != "")
            {
                string menuquery = "";
                menuquery = "select MenuMasterFK,SessionMasterFK from HT_MenuSchedule where SessionMasterFK in('" + itemheadercode + "') and MessMasterFK in('" + hostelcode + "')  and ScheudleItemType='1' and ScheduleType ='1' and MenuScheduleDate between '" + dt.ToString("MM/dd/yyyy") + "'";
                menuquery = menuquery + "  select MenuMasterFK,SessionMasterFK from HT_MenuSchedule where SessionMasterFK in('" + itemheadercode + "') and MessMasterFK in('" + hostelcode + "')  and ScheudleItemType='1' and ScheduleType ='2' and MenuScheduleday ='" + dt.ToString("dddd") + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(menuquery, "Text");
                menuquery = "";
                string menucode = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        string mcode = Convert.ToString(ds.Tables[0].Rows[row]["MenuMasterFK"]);
                        if (menucode.Contains(mcode) == false)
                        {
                            if (menucode == "")
                            {
                                menucode = mcode;
                            }
                            else
                            {
                                menucode = menucode + "'" + "," + "'" + mcode;
                            }
                        }
                    }
                }
                else
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int row = 0; row < ds.Tables[1].Rows.Count; row++)
                        {
                            string mcode = Convert.ToString(ds.Tables[1].Rows[row]["MenuMasterFK"]);
                            if (menucode.Contains(mcode) == false)
                            {
                                if (menucode == "")
                                {
                                    menucode = mcode;
                                }
                                else
                                {
                                    menucode = menucode + "'" + "," + "'" + mcode;
                                }
                            }
                        }
                    }
                }
                string deptquery = "select distinct MenuMasterPK,MenuName from HM_MenuMaster where CollegeCode ='" + collegecode1 + "' and MenuMasterPK in('" + menucode + "')  order by MenuCode";
                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklstpop4menuname.DataSource = ds;
                    chklstpop4menuname.DataTextField = "MenuName";
                    chklstpop4menuname.DataValueField = "MenuMasterPK";
                    chklstpop4menuname.DataBind();
                    if (chklstpop4menuname.Items.Count > 0)
                    {
                        for (int i = 0; i < chklstpop4menuname.Items.Count; i++)
                        {
                            chklstpop4menuname.Items[i].Selected = true;
                        }
                        txtpop4menuname.Text = "Menu Name(" + chklstpop4menuname.Items.Count + ")";
                    }
                }
            }
        }
        catch
        {

        }
    }

    protected void chklst_pop4session_Change(object sender, EventArgs e)
    {
        txtpop4session.Text = "--Select--";
        chkpop4session.Checked = false;
        int ccount = 0;
        for (int i = 0; i < chklstpop4session.Items.Count; i++)
        {
            if (chklstpop4session.Items[i].Selected == true)
            {
                ccount = ccount + 1;
                chkpop4session.Checked = false;
            }
        }
        if (ccount > 0)
        {
            txtpop4session.Text = "Session Name(" + ccount.ToString() + ")";
            if (ccount == chklstpop4session.Items.Count)
            {
                chkpop4session.Checked = true;
            }
        }
        bindpop4menuname();
    }
    protected void chkpop4menuname_Change(object sender, EventArgs e)
    {
        if (chkpop4menuname.Checked == true)
        {
            for (int i = 0; i < chklstpop4menuname.Items.Count; i++)
            {
                chklstpop4menuname.Items[i].Selected = true;
            }
            txtpop4menuname.Text = "Menu Name(" + chklstpop4menuname.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklstpop4menuname.Items.Count; i++)
            {
                chklstpop4menuname.Items[i].Selected = false;
            }
            txtpop4menuname.Text = "--Select--";
        }

    }
    protected void chk_lstpop4menuname_Change(object sender, EventArgs e)
    {
        txtpop4menuname.Text = "--Select--";
        chkpop4menuname.Checked = false;
        int ccount = 0;
        for (int i = 0; i < chklstpop4menuname.Items.Count; i++)
        {
            if (chklstpop4menuname.Items[i].Selected == true)
            {
                ccount = ccount + 1;
                chkpop4menuname.Checked = false;
            }
        }
        if (ccount > 0)
        {
            txtpop4menuname.Text = "Menu Name(" + ccount.ToString() + ")";
            if (ccount == chklstpop4menuname.Items.Count)
            {
                chkpop4menuname.Checked = true;
            }
        }
    }
    protected void btnpop4qty_Click(object sender, EventArgs e)
    {

    }
    protected void btnpop4value_Click(object sender, EventArgs e)
    {

    }
    protected void bindpop4hostelname()
    {
        try
        {
            ddlpop4hostel.Items.Clear();
            ds.Clear();
            // ds = d2.Bindmess_inv(collegecode1);
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlpop4hostel.DataSource = ds;
                ddlpop4hostel.DataTextField = "MessName";
                ddlpop4hostel.DataValueField = "MessMasterPK";
                ddlpop4hostel.DataBind();
                ddlpop4hostel.Items.Insert(0, "Select");
            }
            else
            {
                ddlpop4hostel.Items.Insert(0, "Select");
            }
        }
        catch
        {

        }
    }
    protected void bindpop4session()
    {
        try
        {
            ds.Clear();
            if (ddlpop4hostel.SelectedItem.Value.Trim() != "")
            {
                ds = d2.BindSession_inv(ddlpop4hostel.SelectedItem.Value);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklstpop4session.DataSource = ds;
                    chklstpop4session.DataTextField = "SessionName";
                    chklstpop4session.DataValueField = "SessionMasterPK";
                    chklstpop4session.DataBind();
                    if (chklstpop4session.Items.Count > 0)
                    {
                        for (int i = 0; i < chklstpop4session.Items.Count; i++)
                        {
                            chklstpop4session.Items[i].Selected = true;
                        }
                        txtpop4session.Text = "Session Name(" + chklstpop4session.Items.Count + ")";
                    }
                }
                else
                {
                    txtpop4session.Text = "--Select--";
                }
            }
            else
            {
                txtpop4session.Text = "--Select--";
            }
        }
        catch
        {

        }
    }
    /*pop5*/
    protected void cbvendorname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkpop5vendorname.Checked == true)
            {
                for (int i = 0; i < chklstpop5vendorname.Items.Count; i++)
                {
                    chklstpop5vendorname.Items[i].Selected = true;
                }
                txtpop5vendorname.Text = "Vendor Name(" + (chklstpop5vendorname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstpop5vendorname.Items.Count; i++)
                {
                    chklstpop5vendorname.Items[i].Selected = false;
                }
                txtpop5vendorname.Text = "--Select--";
            }
            binditem();
        }
        catch
        {

        }
    }
    protected void cblvendorname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtpop5vendorname.Text = "--Select--";
            chkpop5vendorname.Checked = false;
            int commcount = 0;
            for (int i = 0; i < chklstpop5vendorname.Items.Count; i++)
            {
                if (chklstpop5vendorname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    chkpop5vendorname.Checked = false;
                }
            }
            if (commcount > 0)
            {
                txtpop5vendorname.Text = "Vendor Name(" + commcount.ToString() + ")";
                if (commcount == chklstpop5vendorname.Items.Count)
                {
                    chkpop5vendorname.Checked = true;
                }
            }
            binditem();
        }
        catch
        {

        }
    }
    public void bindpop5vendorname()
    {
        try
        {
            ds.Clear();
            chklstpop5vendorname.Items.Clear();
            string statequery = "select distinct VendorCode,vendorpk,VendorCompName from CO_VendorMaster order by VendorCompName ";
            ds = d2.select_method_wo_parameter(statequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklstpop5vendorname.DataSource = ds;
                chklstpop5vendorname.DataTextField = "VendorCompName";
                chklstpop5vendorname.DataValueField = "vendorpk";
                chklstpop5vendorname.DataBind();
                if (chklstpop5vendorname.Items.Count > 0)
                {
                    for (int i = 0; i < chklstpop5vendorname.Items.Count; i++)
                    {
                        chklstpop5vendorname.Items[i].Selected = true;
                    }
                    txtpop5vendorname.Text = "Vendor Name(" + chklstpop5vendorname.Items.Count + ")";
                }
            }
        }
        catch
        {

        }
    }
    protected void chkitm1(object sender, EventArgs e)
    {

        int cout = 0;
        txtitm.Text = "---Select---";
        if (chkitm.Checked == true)
        {
            cout++;
            for (int i = 0; i < cblitm.Items.Count; i++)
            {
                cblitm.Items[i].Selected = true;
            }
            txtitm.Text = "Item(" + (cblitm.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblitm.Items.Count; i++)
            {
                cblitm.Items[i].Selected = false;
            }
        }
    }
    protected void cblitm1(object sender, EventArgs e)
    {
        int i = 0;
        chkitm.Checked = false;
        int commcount = 0;
        txtitm.Text = "--Select--";
        for (i = 0; i < cblitm.Items.Count; i++)
        {
            if (cblitm.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                chkitm.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cblitm.Items.Count)
            {
                chkitm.Checked = true;
            }
            txtitm.Text = "Item(" + commcount.ToString() + ")";
        }
    }
    public void binditem()
    {
        //cblitm.Items.Clear();
        string deptquery = "";
        string buildvalue = "";

        for (int i = 0; i < chklstpop5vendorname.Items.Count; i++)
        {
            if (chklstpop5vendorname.Items[i].Selected == true)
            {
                string build = chklstpop5vendorname.Items[i].Value.ToString();
                if (buildvalue == "")
                {
                    buildvalue = build;
                }
                else
                {
                    buildvalue = buildvalue + "'" + "," + "'" + build;
                }
            }
        }
        deptquery = "select distinct it.itempk,it.ItemCode as item_code,it.ItemName as item_name from IM_ItemMaster it,CO_VendorMaster v ,IM_VendorItemDept vi where v.VendorPK =vi.VenItemFK and vi.ItemFK=it.ItemPK and  v.VendorPK in('" + buildvalue + "')";
        ds = d2.select_method_wo_parameter(deptquery, "Text");

        if (ds.Tables[0].Rows.Count > 0)
        {
            cblitm.DataSource = ds;
            cblitm.DataTextField = "item_name";
            cblitm.DataValueField = "itempk";
            cblitm.DataBind();

            if (cblitm.Items.Count > 0)
            {
                for (int i = 0; i < cblitm.Items.Count; i++)
                {

                    cblitm.Items[i].Selected = true;
                }
                txtitm.Text = "Items(" + cblitm.Items.Count + ")";
            }
        }
        else
        {
            txtitm.Text = "--Select--";
        }
    }
    protected void btnpop5go_Click(object sender, EventArgs e)
    {
        try
        {
            string itemheadercode = "";
            DataTable chartable = new DataTable();
            DataTable newchartable = new DataTable();
            for (int i = 0; i < chklstpop5vendorname.Items.Count; i++)
            {
                if (chklstpop5vendorname.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + chklstpop5vendorname.Items[i].Value.ToString() + "";
                        //chartable.Columns.Add(Convert.ToString(chklstpop5vendorname.Items[i].Text));
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + chklstpop5vendorname.Items[i].Value.ToString() + "";
                        //chartable.Columns.Add(Convert.ToString(chklstpop5vendorname.Items[i].Text));
                    }
                }
            }
            DataRow dr;
            string itemheadercode1 = "";
            for (int i = 0; i < cblitm.Items.Count; i++)
            {
                if (cblitm.Items[i].Selected == true)
                {
                    // dr = chartable.NewRow();
                    if (itemheadercode1 == "")
                    {
                        itemheadercode1 = "" + cblitm.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode1 = itemheadercode1 + "'" + "," + "'" + cblitm.Items[i].Value.ToString() + "";
                    }
                    // chartable.Rows.Add(dr);
                }
            }
            DataTable dtnew = new DataTable();
            if (itemheadercode.Trim() != "" && itemheadercode1.Trim() != "")
            {
                string fromdate = Convert.ToString(txtpop5from.Text);
                string todate = Convert.ToString(txtpop5to.Text);
                string[] split = fromdate.Split('/');
                DateTime dt = new DateTime();
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = todate.Split('/');
                DateTime dt1 = new DateTime();
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                string selectquery = " select distinct v.VendorPK,v.vendorcode,V.VendorCompName,i.ItemCode,i.itempk,i.itemname,inwardqty,rpu,(g.inwardqty*pi.rpu)as value from   CO_VendorMaster v,IM_VendorItemDept vi,IT_PurchaseOrder p,IT_PurchaseOrderDetail pi  ,IT_GoodsInward  g,IM_ItemMaster i where v.vendorpk=p.VendorFK and v.VendorPK=vi.VenItemFK  and vi.ItemFK =i.ItemPK  and p.PurchaseOrderPK =pi.PurchaseOrderFK  and vi.VenItemFK  =p.VendorFK and vi.ItemFK  =pi.ItemFK and p.ApproveStatus='1' and i.ForHostelItem='0' and p.OrderDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and v.VendorPK  in('" + itemheadercode + "') and i.ItemPK in ('" + itemheadercode1 + "') and p.PurchaseOrderPK =g.PurchaseOrderFK";

                selectquery = selectquery + "  select distinct v.VendorPK ,V.VendorCompName from CO_VendorMaster v,IM_VendorItemDept vi,IT_PurchaseOrder p,IT_PurchaseOrderDetail  pi  ,IT_GoodsInward  g,IM_ItemMaster i where v.VendorPK =vi.VenItemFK  and v.VendorPK=p.VendorFK and vi.VenItemFK  =p.VendorFK and p.PurchaseOrderPK =pi.PurchaseOrderFK and vi.VenItemFK  =p.VendorFK and vi.ItemFK  =pi.ItemFK and i.ItemPK  =vi.ItemFK and g.itemfk=pi.ItemFK and g.Itemfk  =vi.ItemFK and p.ApproveStatus='1' and i.ForHostelItem='0' and p.OrderDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and v.VendorPK in('" + itemheadercode + "') and i.ItemPK in ('" + itemheadercode1 + "') and p.PurchaseOrderPK =g.PurchaseOrderFK ";
                selectquery = selectquery + "  select distinct i.ItemCode,i.itempk,i.itemname from CO_VendorMaster v,IM_VendorItemDept vi,IT_PurchaseOrder p,IT_PurchaseOrderDetail  pi  ,IT_GoodsInward  g,IM_ItemMaster i where v.VendorPK =vi.VenItemFK and v.VendorPK =p.VendorFK and vi.VenItemFK  =p.VendorFK  and p.PurchaseOrderPK =pi.PurchaseOrderFK  and vi.ItemFK  =pi.ItemFK and i.ItemPK =vi.ItemFK  and i.ItemPK =pi.ItemFK  and g.itemfk=i.ItemPK  and g.Itemfk =vi.ItemFK and p.ApproveStatus ='1' and i.ForHostelItem ='0' and p.OrderDate  between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and v.VendorPK  in('" + itemheadercode + "') and i.ItemPK in ('" + itemheadercode1 + "') and p.PurchaseOrderPK =g.PurchaseOrderFK";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                int ros = 0;
                Chart3.Series.Clear();
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0)
                {
                    for (int rs = 0; rs < ds.Tables[1].Rows.Count; rs++)
                    {
                        chartable.Columns.Add(Convert.ToString(ds.Tables[1].Rows[rs]["VendorCompName"]));
                        addindex.Add(Convert.ToString(ds.Tables[1].Rows[rs]["VendorCompName"]), rs);
                        newchartable.Columns.Add(Convert.ToString(ds.Tables[1].Rows[rs]["VendorCompName"]));

                    }
                    for (int r = 0; r < ds.Tables[2].Rows.Count; r++)
                    {
                        Chart3.Series.Add(Convert.ToString(ds.Tables[2].Rows[r]["ItemCode"]));
                        Chart3.Series[0].BorderWidth = 2;
                    }

                    DataRow drnew;
                    if (cblitm.Items.Count > 0)
                    {
                        for (int co = 0; co < cblitm.Items.Count; co++)
                        {
                            if (cblitm.Items[co].Selected == true)
                            {
                                ros = 0;
                                dr = chartable.NewRow();
                                drnew = newchartable.NewRow();
                                bool newcheck = false;
                                for (int rs = 0; rs < ds.Tables[1].Rows.Count; rs++)
                                {
                                    ros++;
                                    double total = 0;
                                    double totalvalue = 0;
                                    ds.Tables[0].DefaultView.RowFilter = " VendorPK='" + Convert.ToString(ds.Tables[1].Rows[rs]["VendorPK"]) + "' and itempk ='" + Convert.ToString(cblitm.Items[co].Value) + "'";
                                    dv = ds.Tables[0].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        dtnew = dv.ToTable();
                                        total = Convert.ToDouble(dtnew.Compute("Sum(inwardqty)", ""));
                                        totalvalue = Convert.ToDouble(dtnew.Compute("Sum(value )", ""));
                                        dr[ros - 1] = Convert.ToString(total);
                                        drnew[ros - 1] = Convert.ToString(totalvalue);
                                        newcheck = true;
                                    }
                                }
                                if (newcheck == true)
                                {
                                    chartable.Rows.Add(dr);
                                    newchartable.Rows.Add(drnew);
                                }
                            }
                        }
                    }
                    if (rdQunatity.Checked == true)
                    {
                        if (chartable.Rows.Count > 0)
                        {
                            for (int chart_i = 0; chart_i < chartable.Columns.Count; chart_i++)
                            {
                                for (int chart_j = 0; chart_j < chartable.Rows.Count; chart_j++)
                                {
                                    string subnncode = Convert.ToString(chartable.Columns[chart_i]);
                                    string m1 = chartable.Rows[chart_j][chart_i].ToString();
                                    Chart3.Series[chart_j].Points.AddXY(subnncode, m1);
                                    Chart3.Series[chart_j].IsValueShownAsLabel = true;
                                    Chart3.Series[chart_j].IsXValueIndexed = true;
                                }
                            }
                            Chart3.Visible = true;
                        }
                    }
                    if (rdValue.Checked == true)
                    {
                        if (newchartable.Rows.Count > 0)
                        {
                            for (int chart_i = 0; chart_i < newchartable.Columns.Count; chart_i++)
                            {
                                for (int chart_j = 0; chart_j < newchartable.Rows.Count; chart_j++)
                                {
                                    string subnncode = Convert.ToString(newchartable.Columns[chart_i]);
                                    string m1 = newchartable.Rows[chart_j][chart_i].ToString();
                                    Chart3.Series[chart_j].Points.AddXY(subnncode, m1);
                                    Chart3.Series[chart_j].IsValueShownAsLabel = true;
                                    Chart3.Series[chart_j].IsXValueIndexed = true;
                                }
                            }
                            Chart3.Visible = true;
                        }
                    }
                    lblpop5error.Visible = false;
                }
                else
                {
                    lblpop5error.Visible = true;
                    Chart3.Visible = false;
                    lblpop5error.Text = "No Records Found";
                }
            }
            else
            {
                lblpop5error.Visible = true;
                lblpop5error.Text = "Please Select All Field";
                Chart3.Visible = false;
            }
        }
        catch
        {

        }
    }

    protected void cbpop2hostel_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cbpop2hostel.Checked == true)
            {
                for (int i = 0; i < cblpop2hostel.Items.Count; i++)
                {
                    cblpop2hostel.Items[i].Selected = true;
                }
                txtpop2hostelname.Text = "Mess Name(" + cblpop2hostel.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cblpop2hostel.Items.Count; i++)
                {
                    cblpop2hostel.Items[i].Selected = false;
                }
                txtpop2hostelname.Text = "--Select--";
            }
            bindpop2session();
        }
        catch
        {

        }
    }

    protected void cblpop2hostel_Change(object sender, EventArgs e)
    {
        try
        {
            txtpop2hostelname.Text = "--Select--";
            cbpop2hostel.Checked = false;
            int ccount = 0;
            for (int i = 0; i < cblpop2hostel.Items.Count; i++)
            {
                if (cblpop2hostel.Items[i].Selected == true)
                {
                    ccount = ccount + 1;
                    cbpop2hostel.Checked = false;
                }
            }
            if (ccount > 0)
            {
                txtpop2hostelname.Text = "Mess Name(" + ccount.ToString() + ")";
                if (ccount == cblpop2hostel.Items.Count)
                {
                    cbpop2hostel.Checked = true;
                }
                //txtpop3session.Text = "Session Name(" + ccount.ToString() + ")";
            }
            bindpop2session();
        }
        catch
        {

        }
    }

    /*pop6 Menu Expenses History*/
    protected void chkpop6session_checkedchange(object sender, EventArgs e)
    {
        if (chkpop6session.Checked == true)
        {
            for (int i = 0; i < chklstpop6session.Items.Count; i++)
            {
                chklstpop6session.Items[i].Selected = true;
            }
            txtpop6session.Text = "Session Name(" + chklstpop6session.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklstpop6session.Items.Count; i++)
            {
                chklstpop6session.Items[i].Selected = false;
            }
            txtpop6session.Text = "--Select--";
        }

    }
    protected void chklst_pop6session_Change(object sender, EventArgs e)
    {
        txtpop6session.Text = "--Select--";
        chkpop6session.Checked = false;
        int ccount = 0;
        for (int i = 0; i < chklstpop6session.Items.Count; i++)
        {
            if (chklstpop6session.Items[i].Selected == true)
            {
                ccount = ccount + 1;
                chkpop6session.Checked = false;
            }
        }
        if (ccount > 0)
        {
            if (ccount == chklstpop6session.Items.Count)
            {
                chkpop6session.Checked = true;
            }
            txtpop6session.Text = "Session Name(" + ccount.ToString() + ")";
        }
    }
    protected void chkpop6hostel_checkedchange(object sender, EventArgs e)
    {
        if (chkpop6hos.Checked == true)
        {
            for (int i = 0; i < chklstpop6hos.Items.Count; i++)
            {
                chklstpop6hos.Items[i].Selected = true;
            }
            txtpop6hos.Text = "Mess Name(" + chklstpop6hos.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklstpop6hos.Items.Count; i++)
            {
                chklstpop6hos.Items[i].Selected = false;
            }
            txtpop6hos.Text = "--Select--";
        }

    }
    protected void chklst_pop6hostel_Change(object sender, EventArgs e)
    {
        txtpop6hos.Text = "--Select--";
        chkpop6session.Checked = false;
        int ccount = 0;
        for (int i = 0; i < chklstpop6hos.Items.Count; i++)
        {
            if (chklstpop6hos.Items[i].Selected == true)
            {
                ccount = ccount + 1;
                chkpop6hos.Checked = false;
            }
        }
        if (ccount > 0)
        {
            txtpop6hos.Text = "Mess Name(" + ccount.ToString() + ")";
            if (ccount == chklstpop6hos.Items.Count)
            {
                chkpop6hos.Checked = true;
            }
        }
        bindpop6sessionname();

    }
    protected void bindpop6hostelname()
    {
        try
        {
            chklstpop6hos.Items.Clear();
            ds.Clear();
            // ds = d2.Bindmess_inv(collegecode1);
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklstpop6hos.DataSource = ds;
                chklstpop6hos.DataTextField = "MessName";
                chklstpop6hos.DataValueField = "MessMasterPK";
                chklstpop6hos.DataBind();
                if (chklstpop6hos.Items.Count > 0)
                {
                    for (int i = 0; i < chklstpop6hos.Items.Count; i++)
                    {
                        chklstpop6hos.Items[i].Selected = true;
                    }
                    txtpop6hos.Text = "Mess Name(" + chklstpop6hos.Items.Count + ")";
                }
            }
            else
            {

            }
        }
        catch
        {

        }
    }
    protected void bindpop6sessionname()
    {
        string deptquery = "";
        string session = "";

        for (int i = 0; i < chklstpop6hos.Items.Count; i++)
        {
            if (chklstpop6hos.Items[i].Selected == true)
            {
                string build = chklstpop6hos.Items[i].Value.ToString();
                if (session == "")
                {
                    session = build;
                }
                else
                {
                    session = session + "'" + "," + "'" + build;
                }
            }
        }

        deptquery = "select distinct SessionMasterPK,SessionName from HM_SessionMaster where MessMasterFK in('" + session + "')";
        ds = d2.select_method_wo_parameter(deptquery, "Text");

        if (ds.Tables[0].Rows.Count > 0)
        {
            chklstpop6session.DataSource = ds;
            chklstpop6session.DataTextField = "SessionName";
            chklstpop6session.DataValueField = "SessionMasterPK";
            chklstpop6session.DataBind();

            if (chklstpop6session.Items.Count > 0)
            {
                for (int i = 0; i < chklstpop6session.Items.Count; i++)
                {
                    chklstpop6session.Items[i].Selected = true;
                }
                txtpop6session.Text = "Session Name(" + chklstpop6session.Items.Count + ")";
            }
        }
        else
        {
            txtpop6session.Text = "--Select--";
        }
    }
    protected void btngo6_Clcik(object sender, EventArgs e)
    {
        try
        {
            string itemheadercode = "";
            for (int i = 0; i < chklstpop6hos.Items.Count; i++)
            {
                if (chklstpop6hos.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + chklstpop6hos.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + chklstpop6hos.Items[i].Value.ToString() + "";
                    }
                }
            }

            string itemheadercode1 = "";
            for (int i = 0; i < chklstpop6session.Items.Count; i++)
            {
                if (chklstpop6session.Items[i].Selected == true)
                {
                    if (itemheadercode1 == "")
                    {
                        itemheadercode1 = "" + chklstpop6session.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode1 = itemheadercode1 + "'" + "," + "'" + chklstpop6session.Items[i].Value.ToString() + "";
                    }
                }
            }

            string fromdate = Convert.ToString(txt_pop6fromdate.Text);
            string todate = Convert.ToString(txt_pop6todate.Text);
            string[] split = fromdate.Split('/');
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = todate.Split('/');
            DateTime dt1 = new DateTime();
            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            DataRow dr;
            Chart4.Series.Clear();
            if (itemheadercode.Trim() != "" && itemheadercode1.Trim() != "")
            {
                if (rdbExpenses.Checked == true)
                {
                    // string selectquery = " select SUM( ConsumptionQty*RPU )value,m.MenuMasterFK,menuname,DailyConsDate from HT_DailyConsumptionMaster D,HT_DailyConsumptionDetail Dt,IM_ItemMaster i,HM_MenuItemMaster M,HM_MenuItemDetail md,HM_MenuMaster mm where d.DailyConsumptionMasterPK =dt.DailyConsumptionMasterFK and i.ItemPK =dt.ItemFK and m.MenuItemMasterPK =md.MenuItemMasterFK and md.ItemFK =dt.ItemFK and md.ItemFK =i.ItemPK  and DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and SessionFK in ('" + itemheadercode1 + "')  and d.MessMasterFK in ('" + itemheadercode + "') and mm.MenuMasterPK=m.MenuMasterFK  and ForMess<>'2'  and m.MenuMasterFK=d.MenumasterFK group by  ItemHeaderCode ,m.MenuMasterFK,itemheadername,SessionFK,d.MessMasterFK,DailyConsDate,menuname";//and MenuMasterFK  in ('151','152')

                    string selectquery = "  select SUM( ConsumptionQty*RPU )value,d.MenuMasterFK,menuname,DailyConsDate from HT_DailyConsumptionMaster D,HT_DailyConsumptionDetail Dt,IM_ItemMaster i,HM_MenuMaster mm where d.DailyConsumptionMasterPK =dt.DailyConsumptionMasterFK and i.ItemPK =dt.ItemFK and d.MenumasterFK=mm.MenuMasterPK  and DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and SessionFK in ('" + itemheadercode1 + "')  and d.MessMasterFK in ('" + itemheadercode + "') and  ForMess<>'2'  and mm.MenuMasterPK=d.MenumasterFK group by  ItemHeaderCode ,d.MenuMasterFK,itemheadername,SessionFK,d.MessMasterFK,DailyConsDate,menuname ";

                    selectquery = selectquery + "  select distinct MenuMasterPK  ,MenuName from HT_DailyConsumptionMaster  dm ,HT_DailyConsumptionDetail  dd,HM_SessionMaster s,HM_MenuMaster m where  dm.DailyConsumptionMasterPK  =dd.DailyConsumptionMasterFK and s.SessionMasterPK =dm.SessionFK and dm.MessMasterFK =s.MessMasterFK and dm.MessMasterFK  in ('" + itemheadercode + "') and s.SessionMasterPK in ('" + itemheadercode1 + "') and DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'  and ForMess<>'2'  and m.MenuMasterPK=dm.MenumasterFK ";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            btnprintSessionMenu.Visible = true;
                            for (int row = 0; row < ds.Tables[1].Rows.Count; row++)
                            {
                                chartdata.Columns.Add(Convert.ToString(ds.Tables[1].Rows[row]["MenuName"]));
                            }
                            if (rdopop6day.Checked == true)
                            {
                                while (dt <= dt1)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = "DailyConsDate='" + dt.ToString("MM/dd/yyyy") + "'";
                                    dv = ds.Tables[0].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        Chart4.Series.Add(dt.ToString("dd/MM/yyyy") + " - " + dt.ToString("dddd"));
                                        Chart4.Series[0].BorderWidth = 2;
                                        dr = chartdata.NewRow();
                                        for (int row1 = 0; row1 < ds.Tables[1].Rows.Count; row1++)
                                        {
                                            dv.RowFilter = "MenuMasterFK='" + Convert.ToString(ds.Tables[1].Rows[row1]["MenuMasterPK"]) + "' and DailyConsDate='" + dt.ToString("MM/dd/yyyy") + "'";
                                            double todalvalue = 0;
                                            if (dv.Count > 0)
                                            {
                                                DataTable d_new = dv.ToTable();
                                                todalvalue = Convert.ToDouble(d_new.Compute("sum(value)", ""));
                                            }
                                            dr[row1] = Convert.ToString(todalvalue);
                                        }
                                        chartdata.Rows.Add(dr);
                                    }
                                    dt = dt.AddDays(1);
                                }
                                if (chartdata.Rows.Count > 0)
                                {
                                    for (int chart_i = 0; chart_i < chartdata.Columns.Count; chart_i++)
                                    {
                                        for (int chart_j = 0; chart_j < chartdata.Rows.Count; chart_j++)
                                        {
                                            string subnncode = Convert.ToString(chartdata.Columns[chart_i]);
                                            string m1 = chartdata.Rows[chart_j][chart_i].ToString();
                                            Chart4.Series[chart_j].Points.AddXY(subnncode, m1);
                                            Chart4.Series[chart_j].IsValueShownAsLabel = true;
                                            Chart4.Series[chart_j].IsXValueIndexed = true;
                                        }
                                    }
                                    Chart4.Visible = true;
                                }
                            }
                            if (rdopop6week.Checked == true)
                            {
                                int week = 0;
                                while (dt <= dt1)
                                {
                                    string betweenvalue = "DailyConsDate >= '" + dt.ToString("MM/dd/yyyy") + "' and DailyConsDate <= '" + dt.AddDays(6).ToString("MM/dd/yyyy") + "' ";
                                    ds.Tables[0].DefaultView.RowFilter = "" + betweenvalue + "";
                                    dv = ds.Tables[0].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        week++;
                                        Chart4.Series.Add("" + week + "st Week" + " " + dt.ToString("dd/MM") + "-" + dt.AddDays(6).ToString("dd/MM"));
                                        Chart4.Series[0].BorderWidth = 2;
                                        dr = chartdata.NewRow();
                                        for (int row1 = 0; row1 < ds.Tables[1].Rows.Count; row1++)
                                        {
                                            dv.RowFilter = "MenuMasterFK='" + Convert.ToString(ds.Tables[1].Rows[row1]["MenuMasterPK"]) + "' and " + betweenvalue + "";
                                            double todalvalue = 0;
                                            if (dv.Count > 0)
                                            {
                                                DataTable d_new = dv.ToTable();
                                                todalvalue = Convert.ToDouble(d_new.Compute("sum(value)", ""));
                                            }
                                            dr[row1] = Convert.ToString(todalvalue);
                                        }
                                        chartdata.Rows.Add(dr);
                                    }
                                    dt = dt.AddDays(7);
                                }
                                if (chartdata.Rows.Count > 0)
                                {
                                    for (int chart_i = 0; chart_i < chartdata.Columns.Count; chart_i++)
                                    {
                                        for (int chart_j = 0; chart_j < chartdata.Rows.Count; chart_j++)
                                        {
                                            string subnncode = Convert.ToString(chartdata.Columns[chart_i]);
                                            string m1 = chartdata.Rows[chart_j][chart_i].ToString();
                                            Chart4.Series[chart_j].Points.AddXY(subnncode, m1);
                                            Chart4.Series[chart_j].IsValueShownAsLabel = true;
                                            Chart4.Series[chart_j].IsXValueIndexed = true;
                                        }
                                    }
                                    Chart4.Visible = true;
                                }
                            }
                            if (rdopop6month.Checked == true)
                            {
                                int week = 0;
                                while (dt <= dt1)
                                {
                                    int date = Convert.ToInt32(dt.ToString("dd"));
                                    int noofdays = DateTime.DaysInMonth(Convert.ToInt32(dt.ToString("yyyy")), Convert.ToInt32(dt.ToString("MM")));
                                    int remainday = noofdays - date;
                                    string betweenvalue = "DailyConsDate >= '" + dt.ToString("MM/dd/yyyy") + "' and DailyConsDate <= '" + dt.AddDays(remainday).ToString("MM/dd/yyyy") + "' ";
                                    ds.Tables[0].DefaultView.RowFilter = "" + betweenvalue + "";
                                    dv = ds.Tables[0].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        week++;
                                        Chart4.Series.Add("" + week + "st Month" + " " + dt.ToString("dd/MM") + "-" + dt.AddDays(remainday).ToString("dd/MM"));
                                        Chart4.Series[0].BorderWidth = 2;
                                        dr = chartdata.NewRow();
                                        for (int row1 = 0; row1 < ds.Tables[1].Rows.Count; row1++)
                                        {
                                            dv.RowFilter = "MenuMasterFK='" + Convert.ToString(ds.Tables[1].Rows[row1]["MenuMasterPK"]) + "' and " + betweenvalue + "";
                                            double todalvalue = 0;
                                            if (dv.Count > 0)
                                            {
                                                DataTable d_new = dv.ToTable();
                                                todalvalue = Convert.ToDouble(d_new.Compute("sum(value)", ""));
                                            }
                                            dr[row1] = Convert.ToString(todalvalue);
                                        }
                                        chartdata.Rows.Add(dr);
                                    }
                                    dt = dt.AddDays(remainday + 1);
                                }
                                if (chartdata.Rows.Count > 0)
                                {
                                    for (int chart_i = 0; chart_i < chartdata.Columns.Count; chart_i++)
                                    {
                                        for (int chart_j = 0; chart_j < chartdata.Rows.Count; chart_j++)
                                        {
                                            string subnncode = Convert.ToString(chartdata.Columns[chart_i]);
                                            string m1 = chartdata.Rows[chart_j][chart_i].ToString();
                                            Chart4.Series[chart_j].Points.AddXY(subnncode, m1);
                                            Chart4.Series[chart_j].IsValueShownAsLabel = true;
                                            Chart4.Series[chart_j].IsXValueIndexed = true;
                                        }
                                    }

                                    Chart4.Visible = true;
                                }
                            }
                            ArrayList daysvalue = new ArrayList();
                            ArrayList datevalue = new ArrayList();
                            Chart4.RenderType = RenderType.ImageTag;
                            Chart4.ImageType = ChartImageType.Png;
                            Chart4.ImageStorageMode = ImageStorageMode.UseImageLocation;
                            Chart4.ImageLocation = Path.Combine("~/report/", "HostelSessionMenuExpanceStrength");
                            if (rdodaycompar1.Checked == true)
                            {
                                if (chklstdaycompar1.Items.Count > 0)
                                {
                                    for (int rs = 0; rs < chklstdaycompar1.Items.Count; rs++)
                                    {
                                        if (chklstdaycompar1.Items[rs].Selected == true)
                                        {
                                            daysvalue.Add(chklstdaycompar.Items[rs].Text);
                                        }
                                    }
                                }
                                Hashtable Date_Add = new Hashtable();
                                if (daysvalue.Count > 0)
                                {
                                    DateTime dn = dt;
                                    while (dn <= dt1)
                                    {
                                        if (daysvalue.Contains(dn.ToString("dddd")))
                                        {
                                            if (!Date_Add.Contains(Convert.ToString(dn.ToString("dddd"))))
                                            {
                                                Date_Add.Add(Convert.ToString(dn.ToString("dddd")), Convert.ToString(dn.ToString("MM/dd/yyyy")));
                                            }
                                            else
                                            {
                                                string getvalue = Convert.ToString(Date_Add[Convert.ToString(dn.ToString("dddd"))]);
                                                getvalue = getvalue + "," + Convert.ToString(dn.ToString("MM/dd/yyyy"));
                                                Date_Add.Remove(Convert.ToString(dn.ToString("dddd")));
                                                Date_Add.Add(Convert.ToString(dn.ToString("dddd")), Convert.ToString(getvalue));

                                            }
                                        }
                                        dn = dn.AddDays(1);
                                    }
                                    Chart4.Series.Clear();
                                    if (Date_Add.Count > 0)
                                    {
                                        DataTable newdata = new DataTable();
                                        foreach (DictionaryEntry ps in Date_Add)
                                        {
                                            string key = Convert.ToString(ps.Key);
                                            string value = Convert.ToString(ps.Value);
                                            string[] splitn = value.Split(',');
                                            if (splitn.Length > 0)
                                            {
                                                Chart4.Series.Add(key);
                                                Chart4.Series[0].BorderWidth = 2;
                                                dr = chartdata.NewRow();
                                                for (int row = 0; row < ds.Tables[1].Rows.Count; row++)
                                                {
                                                    double todalvalue = 0;
                                                    for (int row1 = 0; row1 <= splitn.GetUpperBound(0); row1++)
                                                    {
                                                        string betweenvalue = "DailyConsDate = '" + Convert.ToString(splitn[row1]) + "'";
                                                        ds.Tables[0].DefaultView.RowFilter = "" + betweenvalue + "";
                                                        dv = ds.Tables[0].DefaultView;
                                                        if (dv.Count > 0)
                                                        {
                                                            newdata = dv.ToTable();
                                                            dv = new DataView(newdata);
                                                            dv.RowFilter = "MenuMasterFK='" + Convert.ToString(ds.Tables[1].Rows[row]["MenuMasterPK"]) + "' and " + betweenvalue + "";

                                                            if (dv.Count > 0)
                                                            {
                                                                DataTable d_new = dv.ToTable();
                                                                todalvalue = todalvalue + Convert.ToDouble(d_new.Compute("sum(value)", ""));
                                                            }
                                                        }
                                                    }
                                                    dr[row] = Convert.ToString(todalvalue);

                                                }
                                                chartdata.Rows.Add(dr);
                                            }
                                        }
                                        if (rdbquantity.Checked == true)
                                        {

                                            if (chartdata.Rows.Count > 0)
                                            {
                                                for (int chart_i = 0; chart_i < chartdata.Columns.Count; chart_i++)
                                                {
                                                    for (int chart_j = 0; chart_j < chartdata.Rows.Count; chart_j++)
                                                    {
                                                        string subnncode = Convert.ToString(chartdata.Columns[chart_i]);
                                                        string m1 = chartdata.Rows[chart_j][chart_i].ToString();
                                                        Chart4.Series[chart_j].Points.AddXY(subnncode, m1);
                                                        Chart4.Series[chart_j].IsValueShownAsLabel = true;
                                                        Chart4.Series[chart_j].IsXValueIndexed = true;
                                                    }
                                                }
                                                Chart4.Visible = true;
                                            }
                                            else
                                            {
                                                Chart4.Visible = false;
                                            }
                                        }
                                        if (rdbValue.Checked == true)
                                        {
                                            if (chartdata.Rows.Count > 0)
                                            {
                                                for (int chart_i = 0; chart_i < chartdata.Columns.Count; chart_i++)
                                                {
                                                    for (int chart_j = 0; chart_j < chartdata.Rows.Count; chart_j++)
                                                    {
                                                        string subnncode = Convert.ToString(chartdata.Columns[chart_i]);
                                                        string m1 = chartdata.Rows[chart_j][chart_i].ToString();
                                                        Chart4.Series[chart_j].Points.AddXY(subnncode, m1);
                                                        Chart4.Series[chart_j].IsValueShownAsLabel = true;
                                                        Chart4.Series[chart_j].IsXValueIndexed = true;
                                                    }
                                                }
                                                Chart4.Visible = true;
                                            }
                                            else
                                            {
                                                Chart4.Visible = false;
                                            }
                                        }
                                    }
                                }
                            }
                            lblpop6error.Visible = false;
                        }
                    }
                    else
                    {
                        Chart4.Visible = false;
                        lblpop6error.Visible = true;
                        lblpop6error.Text = "No Records Found";
                    }
                }
                if (rdbstrength.Checked == true)
                {
                    string selectquery = "select hm.Total_Present, hm.SessionFK,hm.MessMasterFK,hm.DailyConsDate from HT_DailyConsumptionMaster hm where hm.DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'  and hm.MessMasterFK in ('" + itemheadercode + "') and hm.SessionFK in ('" + itemheadercode1 + "')  and ForMess<>'2' ";

                    selectquery = selectquery + " select distinct cm.SessionFK ,SessionName  from HT_DailyConsumptionMaster cm,HM_SessionMaster s where cm.SessionFK  =s. SessionMasterPK and cm.MessMasterFK  =s.MessMasterFK  and DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'  and cm.MessMasterFK  in ('" + itemheadercode + "') and SessionFK in ('" + itemheadercode1 + "')  and ForMess<>'2' ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            for (int row = 0; row < ds.Tables[1].Rows.Count; row++)
                            {
                                chartdata.Columns.Add(Convert.ToString(ds.Tables[1].Rows[row]["SessionName"]));
                            }
                            if (rdopop6day.Checked == true)
                            {
                                while (dt <= dt1)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = "DailyConsDate='" + dt.ToString("MM/dd/yyyy") + "'";
                                    dv = ds.Tables[0].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        Chart4.Series.Add(dt.ToString("dd/MM/yyyy") + " - " + dt.ToString("dddd"));
                                        Chart4.Series[0].BorderWidth = 2;
                                        dr = chartdata.NewRow();
                                        for (int row1 = 0; row1 < ds.Tables[1].Rows.Count; row1++)
                                        {
                                            dv.RowFilter = "SessionFK='" + Convert.ToString(ds.Tables[1].Rows[row1]["SessionFK"]) + "' and DailyConsDate='" + dt.ToString("MM/dd/yyyy") + "'";
                                            double todalvalue = 0;
                                            if (dv.Count > 0)
                                            {
                                                DataTable d_new = dv.ToTable();
                                                todalvalue = Convert.ToDouble(d_new.Compute("sum(Total_Present)", ""));
                                            }
                                            dr[row1] = Convert.ToString(todalvalue);
                                        }
                                        chartdata.Rows.Add(dr);
                                    }
                                    dt = dt.AddDays(1);
                                }
                                if (chartdata.Rows.Count > 0)
                                {
                                    for (int chart_i = 0; chart_i < chartdata.Columns.Count; chart_i++)
                                    {
                                        for (int chart_j = 0; chart_j < chartdata.Rows.Count; chart_j++)
                                        {
                                            string subnncode = Convert.ToString(chartdata.Columns[chart_i]);
                                            string m1 = chartdata.Rows[chart_j][chart_i].ToString();
                                            Chart4.Series[chart_j].Points.AddXY(subnncode, m1);
                                            Chart4.Series[chart_j].IsValueShownAsLabel = true;
                                            Chart4.Series[chart_j].IsXValueIndexed = true;
                                        }
                                    }
                                    Chart4.Visible = true;
                                }
                            }
                            if (rdopop6week.Checked == true)
                            {
                                int week = 0;
                                while (dt <= dt1)
                                {
                                    string betweenvalue = "DailyConsDate >= '" + dt.ToString("MM/dd/yyyy") + "' and DailyConsDate <= '" + dt.AddDays(6).ToString("MM/dd/yyyy") + "' ";
                                    ds.Tables[0].DefaultView.RowFilter = "" + betweenvalue + "";
                                    dv = ds.Tables[0].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        week++;
                                        Chart4.Series.Add("" + week + "st Week" + " " + dt.ToString("dd/MM") + "-" + dt.AddDays(6).ToString("dd/MM"));
                                        Chart4.Series[0].BorderWidth = 2;
                                        dr = chartdata.NewRow();
                                        for (int row1 = 0; row1 < ds.Tables[1].Rows.Count; row1++)
                                        {
                                            dv.RowFilter = "SessionFK='" + Convert.ToString(ds.Tables[1].Rows[row1]["SessionFK"]) + "' and " + betweenvalue + "";
                                            double todalvalue = 0;
                                            if (dv.Count > 0)
                                            {
                                                DataTable d_new = dv.ToTable();
                                                todalvalue = Convert.ToDouble(d_new.Compute("sum(Total_Present)", ""));
                                            }
                                            dr[row1] = Convert.ToString(todalvalue);
                                        }
                                        chartdata.Rows.Add(dr);
                                    }
                                    dt = dt.AddDays(7);
                                }
                                if (chartdata.Rows.Count > 0)
                                {
                                    for (int chart_i = 0; chart_i < chartdata.Columns.Count; chart_i++)
                                    {
                                        for (int chart_j = 0; chart_j < chartdata.Rows.Count; chart_j++)
                                        {
                                            string subnncode = Convert.ToString(chartdata.Columns[chart_i]);
                                            string m1 = chartdata.Rows[chart_j][chart_i].ToString();
                                            Chart4.Series[chart_j].Points.AddXY(subnncode, m1);
                                            Chart4.Series[chart_j].IsValueShownAsLabel = true;
                                            Chart4.Series[chart_j].IsXValueIndexed = true;
                                        }
                                    }
                                    Chart4.Visible = true;
                                }
                            }
                            if (rdopop6month.Checked == true)
                            {
                                int week = 0;
                                while (dt <= dt1)
                                {
                                    int date = Convert.ToInt32(dt.ToString("dd"));
                                    int noofdays = DateTime.DaysInMonth(Convert.ToInt32(dt.ToString("yyyy")), Convert.ToInt32(dt.ToString("MM")));
                                    int remainday = noofdays - date;
                                    string betweenvalue = "DailyConsDate >= '" + dt.ToString("MM/dd/yyyy") + "' and DailyConsDate <= '" + dt.AddDays(remainday).ToString("MM/dd/yyyy") + "' ";
                                    ds.Tables[0].DefaultView.RowFilter = "" + betweenvalue + "";
                                    dv = ds.Tables[0].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        week++;
                                        Chart4.Series.Add("" + week + "st Month" + " " + dt.ToString("dd/MM") + "-" + dt.AddDays(remainday).ToString("dd/MM"));
                                        Chart4.Series[0].BorderWidth = 2;
                                        dr = chartdata.NewRow();
                                        for (int row1 = 0; row1 < ds.Tables[1].Rows.Count; row1++)
                                        {
                                            dv.RowFilter = "SessionFK='" + Convert.ToString(ds.Tables[1].Rows[row1]["SessionFK"]) + "' and " + betweenvalue + "";
                                            double todalvalue = 0;
                                            if (dv.Count > 0)
                                            {
                                                DataTable d_new = dv.ToTable();
                                                todalvalue = Convert.ToDouble(d_new.Compute("sum(Total_Present)", ""));
                                            }
                                            dr[row1] = Convert.ToString(todalvalue);
                                        }
                                        chartdata.Rows.Add(dr);
                                    }
                                    dt = dt.AddDays(remainday + 1);
                                }
                                if (chartdata.Rows.Count > 0)
                                {
                                    for (int chart_i = 0; chart_i < chartdata.Columns.Count; chart_i++)
                                    {
                                        for (int chart_j = 0; chart_j < chartdata.Rows.Count; chart_j++)
                                        {
                                            string subnncode = Convert.ToString(chartdata.Columns[chart_i]);
                                            string m1 = chartdata.Rows[chart_j][chart_i].ToString();
                                            Chart4.Series[chart_j].Points.AddXY(subnncode, m1);
                                            Chart4.Series[chart_j].IsValueShownAsLabel = true;
                                            Chart4.Series[chart_j].IsXValueIndexed = true;
                                        }
                                    }
                                    lblpop6error.Visible = false;
                                    Chart4.Visible = true;
                                }
                            }
                            ArrayList daysvalue = new ArrayList();
                            ArrayList datevalue = new ArrayList();
                            if (rdodaycompar1.Checked == true)
                            {
                                if (chklstdaycompar1.Items.Count > 0)
                                {
                                    for (int rs = 0; rs < chklstdaycompar1.Items.Count; rs++)
                                    {
                                        if (chklstdaycompar1.Items[rs].Selected == true)
                                        {
                                            daysvalue.Add(chklstdaycompar.Items[rs].Text);
                                        }
                                    }
                                }
                                if (daysvalue.Count > 0)
                                {
                                    DateTime dn = dt;
                                    while (dn <= dt1)
                                    {
                                        if (Convert.ToString(daysvalue[0]) == Convert.ToString(dn.ToString("dddd")))
                                        {
                                            datevalue.Add(Convert.ToString(dn.ToString("MM/dd/yyyy")));
                                        }
                                        dn = dn.AddDays(1);
                                    }
                                    Chart4.Series.Clear();
                                    if (datevalue.Count > 0)
                                    {
                                        DataTable newdata = new DataTable();

                                        for (int row1 = 0; row1 < datevalue.Count; row1++)
                                        {
                                            string betweenvalue = "DailyConsDate = '" + Convert.ToString(datevalue[row1]) + "'";
                                            ds.Tables[0].DefaultView.RowFilter = "" + betweenvalue + "";
                                            dv = ds.Tables[0].DefaultView;
                                            if (dv.Count > 0)
                                            {
                                                Chart4.Series.Add(Convert.ToString(datevalue[row1] + "-" + Convert.ToDateTime(datevalue[row1]).ToString("dddd")));
                                                Chart4.Series[0].BorderWidth = 2;
                                                newdata = dv.ToTable();
                                                dv = new DataView(newdata);
                                                dr = chartdata.NewRow();
                                                for (int row = 0; row < ds.Tables[1].Rows.Count; row++)
                                                {
                                                    dv.RowFilter = "SessionFK='" + Convert.ToString(ds.Tables[1].Rows[row]["SessionFK"]) + "' and " + betweenvalue + "";
                                                    double todalvalue = 0;
                                                    if (dv.Count > 0)
                                                    {
                                                        DataTable d_new = dv.ToTable();
                                                        todalvalue = Convert.ToDouble(d_new.Compute("sum(Total_Present)", ""));
                                                    }
                                                    dr[row] = Convert.ToString(todalvalue);
                                                }
                                                chartdata.Rows.Add(dr);
                                            }
                                        }
                                        if (rdbquantity.Checked == true)
                                        {

                                            if (chartdata.Rows.Count > 0)
                                            {
                                                for (int chart_i = 0; chart_i < chartdata.Columns.Count; chart_i++)
                                                {
                                                    for (int chart_j = 0; chart_j < chartdata.Rows.Count; chart_j++)
                                                    {
                                                        string subnncode = Convert.ToString(chartdata.Columns[chart_i]);
                                                        string m1 = chartdata.Rows[chart_j][chart_i].ToString();
                                                        Chart4.Series[chart_j].Points.AddXY(subnncode, m1);
                                                        Chart4.Series[chart_j].IsValueShownAsLabel = true;
                                                        Chart4.Series[chart_j].IsXValueIndexed = true;
                                                    }
                                                }
                                                Chart4.Visible = true;
                                            }
                                            else
                                            {
                                                Chart4.Visible = false;
                                            }
                                        }
                                        if (rdbValue.Checked == true)
                                        {
                                            if (chartdata.Rows.Count > 0)
                                            {
                                                for (int chart_i = 0; chart_i < chartdata.Columns.Count; chart_i++)
                                                {
                                                    for (int chart_j = 0; chart_j < chartdata.Rows.Count; chart_j++)
                                                    {
                                                        string subnncode = Convert.ToString(chartdata.Columns[chart_i]);
                                                        string m1 = chartdata.Rows[chart_j][chart_i].ToString();
                                                        Chart4.Series[chart_j].Points.AddXY(subnncode, m1);
                                                        Chart4.Series[chart_j].IsValueShownAsLabel = true;
                                                        Chart4.Series[chart_j].IsXValueIndexed = true;
                                                    }
                                                }
                                                Chart4.Visible = true;
                                            }
                                            else
                                            {
                                                Chart4.Visible = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Chart4.Visible = false;
                        lblpop6error.Visible = true;
                        lblpop6error.Text = "No Records Found";
                    }
                }
            }
        }
        catch
        {

        }
    }
}
