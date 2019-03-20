using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Configuration;

public partial class Inv_CodeMaster : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DateTime dt;
    int row;
    int i;

    string[] split;

    bool fromDropDown = false;
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
            if (!IsPostBack)
            {
                bindcollege();
                txt_frmdate.Attributes.Add("readonly", "readonly");
                BindGridview();
                loadOldSetting();
            }
        }
        catch (Exception ex) { }
    }
    public void loadOldSetting()
    {
        try
        {
            dt = new DateTime();
            string clgcode = "";
            if (ddl_collegename.Items.Count > 0)
            {
                clgcode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }

            string selectPrevDate = "select distinct CONVERT(varchar(10), StartDate,103) as date from IM_CodeSettings where CollegeCode='" + clgcode + "' order by date desc";
            ds1.Clear();
            ds1 = d2.select_method_wo_parameter(selectPrevDate, "Text");
            ddl_PrevDate.Items.Clear();
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ddl_PrevDate.DataSource = ds1;
                ddl_PrevDate.DataTextField = "date";
                ddl_PrevDate.DataBind();
            }

            string selectquery = "select top 1 * from IM_CodeSettings where CollegeCode='" + clgcode + "' order by StartDate desc";

            if (fromDropDown)
            {
                if (ddl_PrevDate.Items.Count > 0)
                {
                    split = ddl_PrevDate.SelectedItem.Text.Split('/');
                    dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

                    selectquery = "select top 1 * from IM_CodeSettings where CollegeCode='" + clgcode + "' and StartDate='" + dt.ToString("MM/dd/yyyy") + "' order by StartDate desc";
                }
            }
            //else
            //{
            //    string selectPrevDate = "select distinct CONVERT(varchar(10), StartDate,103) as date from IM_CodeSettings where CollegeCode='" + clgcode + "' order by date desc";
            //    ds1.Clear();
            //    ds1 = d2.select_method_wo_parameter(selectPrevDate, "Text");
            //    if (ds1.Tables[0].Rows.Count > 0)
            //    {
            //        ddl_PrevDate.DataSource = ds1;
            //        ddl_PrevDate.DataTextField = "date";
            //        ddl_PrevDate.DataBind();
            //    }
            //}
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (i = 0; i < grid_prev.Rows.Count; i++)
                {
                    TextBox txtItemCode = (TextBox)grid_prev.Rows[i].FindControl("txt_acronym1");
                    TextBox txtItemCode2 = (TextBox)grid_prev.Rows[i].FindControl("txt_startno1");
                    TextBox txtItemCode3 = (TextBox)grid_prev.Rows[i].FindControl("txt_size1");

                    switch (i)
                    {
                        case 0:
                            txtItemCode.Text = ds.Tables[0].Rows[0]["ItemHeaderAcr"].ToString();
                            txtItemCode2.Text = ds.Tables[0].Rows[0]["ItemHeaderStNo"].ToString();
                            txtItemCode3.Text = ds.Tables[0].Rows[0]["ItemHeaderSize"].ToString();
                            break;
                        case 1:
                            txtItemCode.Text = ds.Tables[0].Rows[0]["ItemAcr"].ToString();
                            txtItemCode2.Text = ds.Tables[0].Rows[0]["ItemStNo"].ToString();
                            txtItemCode3.Text = ds.Tables[0].Rows[0]["ItemSize"].ToString();

                            string nHeaderChecked = ds.Tables[0].Rows[0]["IncludeHeaderAcr"].ToString();

                            CheckBox nHeaderCheck = (CheckBox)grid_prev.Rows[i].FindControl("cb_y");

                            if (nHeaderChecked.ToUpper() == "TRUE")
                            {
                                nHeaderCheck.Checked = true;
                            }
                            else
                            {
                                nHeaderCheck.Checked = false;
                            }
                            break;
                        case 2:
                            txtItemCode.Text = ds.Tables[0].Rows[0]["CustAcr"].ToString();
                            txtItemCode2.Text = ds.Tables[0].Rows[0]["CustStNo"].ToString();
                            txtItemCode3.Text = ds.Tables[0].Rows[0]["CustSize"].ToString();
                            break;
                        case 3:
                            txtItemCode.Text = ds.Tables[0].Rows[0]["VenAcr"].ToString();
                            txtItemCode2.Text = ds.Tables[0].Rows[0]["VenStNo"].ToString();
                            txtItemCode3.Text = ds.Tables[0].Rows[0]["VenSize"].ToString();
                            break;
                        case 4:
                            txtItemCode.Text = ds.Tables[0].Rows[0]["ReqAcr"].ToString();
                            txtItemCode2.Text = ds.Tables[0].Rows[0]["ReqStNo"].ToString();
                            txtItemCode3.Text = ds.Tables[0].Rows[0]["ReqSize"].ToString();
                            break;
                        case 5:
                            txtItemCode.Text = ds.Tables[0].Rows[0]["QuoAcr"].ToString();
                            txtItemCode2.Text = ds.Tables[0].Rows[0]["QuoStNo"].ToString();
                            txtItemCode3.Text = ds.Tables[0].Rows[0]["QuoSize"].ToString();
                            break;
                        case 6:
                            txtItemCode.Text = ds.Tables[0].Rows[0]["POAcr"].ToString();
                            txtItemCode2.Text = ds.Tables[0].Rows[0]["POStNo"].ToString();
                            txtItemCode3.Text = ds.Tables[0].Rows[0]["POSize"].ToString();
                            break;
                        case 7:
                            txtItemCode.Text = ds.Tables[0].Rows[0]["GIAcr"].ToString();
                            txtItemCode2.Text = ds.Tables[0].Rows[0]["GIStNo"].ToString();
                            txtItemCode3.Text = ds.Tables[0].Rows[0]["GISize"].ToString();
                            break;
                        case 8:
                            txtItemCode.Text = ds.Tables[0].Rows[0]["GRAcr"].ToString();
                            txtItemCode2.Text = ds.Tables[0].Rows[0]["GRStNo"].ToString();
                            txtItemCode3.Text = ds.Tables[0].Rows[0]["GRSize"].ToString();
                            break;
                        case 9:
                            txtItemCode.Text = ds.Tables[0].Rows[0]["AssetAcr"].ToString();
                            txtItemCode2.Text = ds.Tables[0].Rows[0]["AssetStNo"].ToString();
                            txtItemCode3.Text = ds.Tables[0].Rows[0]["AssetSize"].ToString();

                            CheckBox nCbStaff = (CheckBox)grid_prev.Rows[i].FindControl("cb_y");
                            CheckBox nCbItem = (CheckBox)grid_prev.Rows[i].FindControl("cb_y1");

                            nCbStaff.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAssetDeptAcr"].ToString());
                            nCbItem.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAssetItemAcr"].ToString());

                            break;

                        case 10:
                            txtItemCode.Text = ds.Tables[0].Rows[0]["MenuAcr"].ToString();
                            txtItemCode2.Text = ds.Tables[0].Rows[0]["MenuStNo"].ToString();
                            txtItemCode3.Text = ds.Tables[0].Rows[0]["MenuSize"].ToString();
                            break;
                        case 11:
                            txtItemCode.Text = ds.Tables[0].Rows[0]["VenReqAcr"].ToString();
                            txtItemCode2.Text = ds.Tables[0].Rows[0]["VenReqStNo"].ToString();
                            txtItemCode3.Text = ds.Tables[0].Rows[0]["VenReqSize"].ToString();
                            break;
                        case 12:
                            txtItemCode.Text = ds.Tables[0].Rows[0]["VenBookReqAcr"].ToString();
                            txtItemCode2.Text = ds.Tables[0].Rows[0]["VenBookReqStNo"].ToString();
                            txtItemCode3.Text = ds.Tables[0].Rows[0]["VenBookReqSize"].ToString();
                            break;
                        case 13:
                            txtItemCode.Text = ds.Tables[0].Rows[0]["VenQuoNoAcr"].ToString();
                            txtItemCode2.Text = ds.Tables[0].Rows[0]["VenQuoNoStNo"].ToString();
                            txtItemCode3.Text = ds.Tables[0].Rows[0]["VenQuoNoSize"].ToString();
                            break;
                    }
                }
            }
            else
            {
                BindGridview();
            }
        }
        catch { }
    }
    protected void delete_btn_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_delete.Text == "Delete")
            {
                surediv.Visible = true;
                lbl_sure.Text = "Do you want to delete this Record?";

            }
        }
        catch
        {
        }

    }
    public void BindGridview()
    {
        ArrayList addnew = new ArrayList();
        addnew.Add("Item Header Code");
        addnew.Add("Item Code");
        addnew.Add("Customer Code");
        addnew.Add("Vendor Code");
        addnew.Add("Reruisition Code");
        addnew.Add("Quotation Code");
        addnew.Add("Order Code");
        addnew.Add("Inward Code");
        addnew.Add("Return Code");
        addnew.Add("Asset Code");
        addnew.Add("Menu Id");
        addnew.Add("Vendor Request Id");
        addnew.Add("Vendor Book Request Id");
        addnew.Add("Vendor Quotation No");
        ug_grid.Visible = true;
        grid_prev.Visible = true;

        DataTable dt = new DataTable();
        dt.Columns.Add("Dummy");
        dt.Columns.Add("Dummy1");
        dt.Columns.Add("Dummy2");
        dt.Columns.Add("Dummy3");
        dt.Columns.Add("Dummy4");
        dt.Columns.Add("Dummay5");
        DataRow dr;
        for (row = 0; row < addnew.Count; row++)
        {
            dr = dt.NewRow();
            dr[0] = "1";
            dr[1] = Convert.ToString(addnew[row]);
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "";
            dt.Rows.Add(dr);
        }
        if (dt.Rows.Count > 0)
        {
            ug_grid.DataSource = dt;
            ug_grid.DataBind();
            grid_prev.DataSource = dt;
            grid_prev.DataBind();

        }
        txt_frmdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_prvdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }
    public void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            string[] nItemcode = new string[3];
            string[] nItemHeader = new string[3];
            string[] nCustCode = new string[3];
            string[] nVendorCode = new string[3];
            string[] nReruisition = new string[3];
            string[] nQuotation = new string[3];
            string[] nOrder = new string[3];
            string[] nInward = new string[3];
            string[] nReturn = new string[3];
            string[] nAsset = new string[3];
            string[] nmenuid = new string[3];
            string[] nreqid = new string[3];
            string[] nbookreqid = new string[3];
            string[] nQuotationid = new string[3];
            string nAssetChecked = "0,0";
            string nHeaderCheck = "";

            string assdept = "";
            string assitem = "";
            string clgcode = "";
            if (ddl_collegename.Items.Count > 0)
            {
                clgcode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }

            string firstdate = Convert.ToString(txt_frmdate.Text);
            dt = new DateTime();
            split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

            for (int i = 0; i < ug_grid.Rows.Count; i++)
            {
                TextBox txtItemCode = (TextBox)ug_grid.Rows[i].FindControl("txt_acronym");
                TextBox txtItemCode2 = (TextBox)ug_grid.Rows[i].FindControl("txt_startno");
                TextBox txtItemCode3 = (TextBox)ug_grid.Rows[i].FindControl("txt_size");
                switch (i)
                {
                    case 0:
                        nItemHeader[0] = txtItemCode.Text;
                        nItemHeader[1] = txtItemCode2.Text;
                        nItemHeader[2] = txtItemCode3.Text;

                        txtItemCode.Text = "";
                        txtItemCode2.Text = "";
                        txtItemCode3.Text = "";
                        break;
                    case 1:
                        nItemcode[0] = txtItemCode.Text;
                        nItemcode[1] = txtItemCode2.Text;
                        nItemcode[2] = txtItemCode3.Text;

                        txtItemCode.Text = "";
                        txtItemCode2.Text = "";
                        txtItemCode3.Text = "";
                        CheckBox chkItemHeader = (CheckBox)ug_grid.Rows[i].FindControl("cb_x");

                        if (chkItemHeader.Checked)
                        {
                            nHeaderCheck = "1";
                        }
                        else
                        {
                            nHeaderCheck = "0";
                        }
                        chkItemHeader.Checked = false;
                        break;

                    case 2:
                        nCustCode[0] = txtItemCode.Text;
                        nCustCode[1] = txtItemCode2.Text;
                        nCustCode[2] = txtItemCode3.Text;

                        txtItemCode.Text = "";
                        txtItemCode2.Text = "";
                        txtItemCode3.Text = "";
                        break;
                    case 3:
                        nVendorCode[0] = txtItemCode.Text;
                        nVendorCode[1] = txtItemCode2.Text;
                        nVendorCode[2] = txtItemCode3.Text;

                        txtItemCode.Text = "";
                        txtItemCode2.Text = "";
                        txtItemCode3.Text = "";
                        break;
                    case 4:
                        nReruisition[0] = txtItemCode.Text;
                        nReruisition[1] = txtItemCode2.Text;
                        nReruisition[2] = txtItemCode3.Text;

                        txtItemCode.Text = "";
                        txtItemCode2.Text = "";
                        txtItemCode3.Text = "";
                        break;
                    case 5:
                        nQuotation[0] = txtItemCode.Text;
                        nQuotation[1] = txtItemCode2.Text;
                        nQuotation[2] = txtItemCode3.Text;

                        txtItemCode.Text = "";
                        txtItemCode2.Text = "";
                        txtItemCode3.Text = "";
                        break;
                    case 6:
                        nOrder[0] = txtItemCode.Text;
                        nOrder[1] = txtItemCode2.Text;
                        nOrder[2] = txtItemCode3.Text;

                        txtItemCode.Text = "";
                        txtItemCode2.Text = "";
                        txtItemCode3.Text = "";
                        break;
                    case 7:
                        nInward[0] = txtItemCode.Text;
                        nInward[1] = txtItemCode2.Text;
                        nInward[2] = txtItemCode3.Text;

                        txtItemCode.Text = "";
                        txtItemCode2.Text = "";
                        txtItemCode3.Text = "";
                        break;
                    case 8:
                        nReturn[0] = txtItemCode.Text;
                        nReturn[1] = txtItemCode2.Text;
                        nReturn[2] = txtItemCode3.Text;

                        txtItemCode.Text = "";
                        txtItemCode2.Text = "";
                        txtItemCode3.Text = "";
                        break;
                    case 9:
                        nAsset[0] = txtItemCode.Text;
                        nAsset[1] = txtItemCode2.Text;
                        nAsset[2] = txtItemCode3.Text;

                        txtItemCode.Text = "";
                        txtItemCode2.Text = "";
                        txtItemCode3.Text = "";

                        CheckBox nCbStaff = (CheckBox)ug_grid.Rows[i].FindControl("cb_x");
                        CheckBox nCbItem = (CheckBox)ug_grid.Rows[i].FindControl("cb_x1");

                        if (nCbStaff.Checked)
                        {
                            assdept = "1";
                        }
                        else
                        {
                            assdept = "0";
                        }
                        if (nCbItem.Checked)
                        {
                            assitem = "1";
                        }
                        else
                        {
                            assitem = "0";
                        }

                        //if (nCbStaff.Checked && nCbItem.Checked)
                        //{
                        //    nAssetChecked = "1,1";
                        //}
                        //else if (nCbStaff.Checked)
                        //{
                        //    nAssetChecked = "1,0";
                        //}
                        //else if (nCbItem.Checked)
                        //{
                        //    nAssetChecked = "0,1";
                        //}

                        nCbStaff.Checked = false;
                        nCbItem.Checked = false;
                        break;
                    case 10:
                        nmenuid[0] = txtItemCode.Text;
                        nmenuid[1] = txtItemCode2.Text;
                        nmenuid[2] = txtItemCode3.Text;

                        txtItemCode.Text = "";
                        txtItemCode2.Text = "";
                        txtItemCode3.Text = "";
                        break;
                    case 11:
                        nreqid[0] = txtItemCode.Text;
                        nreqid[1] = txtItemCode2.Text;
                        nreqid[2] = txtItemCode3.Text;

                        txtItemCode.Text = "";
                        txtItemCode2.Text = "";
                        txtItemCode3.Text = "";
                        break;
                    case 12:
                        nbookreqid[0] = txtItemCode.Text;
                        nbookreqid[1] = txtItemCode2.Text;
                        nbookreqid[2] = txtItemCode3.Text;

                        txtItemCode.Text = "";
                        txtItemCode2.Text = "";
                        txtItemCode3.Text = "";
                        break;
                    case 13:
                        nQuotationid[0] = txtItemCode.Text;
                        nQuotationid[1] = txtItemCode2.Text;
                        nQuotationid[2] = txtItemCode3.Text;

                        txtItemCode.Text = "";
                        txtItemCode2.Text = "";
                        txtItemCode3.Text = "";
                        break;




                }
            }


            string insertquery = "if exists(select*from IM_CodeSettings where StartDate='" + dt.ToString("MM/dd/yyyy") + "' and collegecode='" + clgcode + "') update IM_CodeSettings set ItemAcr='" + nItemcode[0].ToUpper() + "' ,ItemStNo ='" + nItemcode[1] + "',ItemSize='" + nItemcode[2] + "',VenAcr='" + nVendorCode[0].ToUpper() + "',VenStNo='" + nVendorCode[1] + "',VenSize='" + nVendorCode[2] + "',ReqAcr='" + nReruisition[0].ToUpper() + "', ReqStNo='" + nReruisition[1] + "',ReqSize='" + nReruisition[2] + "',QuoAcr='" + nQuotation[0].ToUpper() + "',QuoStNo='" + nQuotation[1] + "',QuoSize='" + nQuotation[2] + "',POAcr='" + nOrder[0].ToUpper() + "',POStNo='" + nOrder[1] + "',POSize ='" + nOrder[2] + "', GIAcr='" + nInward[0].ToUpper() + "',GIStNo='" + nInward[1] + "',GISize='" + nInward[2] + "',CustAcr='" + nCustCode[0].ToUpper() + "',CustStNo='" + nCustCode[1] + "',CustSize='" + nCustCode[2] + "',StartDate='" + dt.ToString("MM/dd/yyyy") + "',GRAcr='" + nReturn[0].ToUpper() + "',GRStNo='" + nReturn[1] + "', GRSize= '" + nReturn[2] + "',AssetAcr='" + nAsset[0].ToUpper() + "',AssetStNo='" + nAsset[1] + "',AssetSize='" + nAsset[2] + "',ItemHeaderAcr='" + nItemHeader[0].ToUpper() + "',ItemHeaderStNo='" + nItemHeader[1] + "',ItemHeaderSize='" + nItemHeader[2] + "', IncludeHeaderAcr= '" + nHeaderCheck + "',IsAssetDeptAcr='" + assdept + "',IsAssetItemAcr='" + assitem + "',MenuAcr='" + nmenuid[0].ToUpper() + "',MenuStNo='" + nmenuid[1] + "',MenuSize='" + nmenuid[2] + "', VenReqAcr='" + nreqid[0].ToUpper() + "',VenReqStNo='" + nreqid[1] + "',VenReqSize='" + nreqid[2] + "', VenBookReqAcr='" + nbookreqid[0].ToUpper() + "',VenBookReqStNo='" + nbookreqid[1] + "',VenBookReqSize='" + nbookreqid[2] + "',VenQuoNoAcr='" + nQuotationid[0].ToUpper() + "',VenQuoNoStNo='" + nQuotationid[1] + "',VenQuoNoSize='" + nQuotationid[2] + "',collegecode='" + clgcode + "' where StartDate='" + dt.ToString("MM/dd/yyyy") + "' and collegecode='" + clgcode + "' else insert into IM_CodeSettings(ItemAcr,ItemStNo,ItemSize,VenAcr,VenStNo,VenSize,ReqAcr,ReqStNo,ReqSize,QuoAcr,QuoStNo,QuoSize,POAcr,POStNo,POSize,GIAcr,GIStNo,GISize,CustAcr,CustStNo,CustSize,StartDate,GRAcr,GRStNo,GRSize,AssetAcr,AssetStNo,AssetSize,ItemHeaderAcr,ItemHeaderStNo,ItemHeaderSize,IncludeHeaderAcr,MenuAcr,MenuStNo,MenuSize,IsAssetDeptAcr,IsAssetItemAcr,VenReqAcr,VenReqStNo,VenReqSize,collegecode,VenBookReqAcr,VenBookReqStNo,VenBookReqSize,VenQuoNoAcr,VenQuoNoStNo,VenQuoNoSize) values('" + nItemcode[0].ToUpper() + "','" + nItemcode[1] + "','" + nItemcode[2] + "','" + nVendorCode[0].ToUpper() + "','" + nVendorCode[1] + "','" + nVendorCode[2] + "','" + nReruisition[0].ToUpper() + "','" + nReruisition[1] + "','" + nReruisition[2] + "','" + nQuotation[0].ToUpper() + "','" + nQuotation[1] + "','" + nQuotation[2] + "','" + nOrder[0].ToUpper() + "','" + nOrder[1] + "','" + nOrder[2] + "','" + nInward[0].ToUpper() + "','" + nInward[1] + "','" + nInward[2] + "','" + nCustCode[0].ToUpper() + "','" + nCustCode[1] + "','" + nCustCode[2] + "','" + dt.ToString("MM/dd/yyyy") + "','" + nReturn[0].ToUpper() + "','" + nReturn[1] + "','" + nReturn[2] + "','" + nAsset[0].ToUpper() + "','" + nAsset[1] + "','" + nAsset[2] + "','" + nItemHeader[0].ToUpper() + "','" + nItemHeader[1] + "','" + nItemHeader[2] + "','" + nHeaderCheck + "','" + nmenuid[0].ToUpper() + "','" + nmenuid[1] + "','" + nmenuid[2] + "','" + assdept + "','" + assitem + "','" + nreqid[0].ToUpper() + "','" + nreqid[1] + "','" + nreqid[2] + "','" + clgcode + "','" + nbookreqid[0].ToUpper() + "','" + nbookreqid[1] + "','" + nbookreqid[2] + "','" + nQuotationid[0].ToUpper() + "','" + nQuotationid[1] + "','" + nQuotationid[2] + "')";
            int inst = d2.update_method_wo_parameter(insertquery, "Text");
            if (inst != 0)
            {
                loadOldSetting();
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Saved Successfully";

            }

        }
        catch { }

    }
    public void grid_prev_Bound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            (e.Row.Cells[2].FindControl("cb_y") as CheckBox).Visible = false;
            (e.Row.Cells[2].FindControl("cb_y1") as CheckBox).Visible = false;
        }

    }
    public void grid_prev_Bound0(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            (e.Row.Cells[2].FindControl("cb_x") as CheckBox).Visible = false;
            (e.Row.Cells[2].FindControl("cb_x1") as CheckBox).Visible = false;
        }
    }
    public void OnDataBound(object sender, EventArgs e)
    {
        if (grid_prev.Rows.Count > 0)
        {
            for (row = 0; row < grid_prev.Rows.Count; row++)
            {
                if (row == grid_prev.Rows.Count - 3)
                {
                    (grid_prev.Rows[row].FindControl("cb_y") as CheckBox).Visible = true;
                    (grid_prev.Rows[row].FindControl("cb_y1") as CheckBox).Visible = true;
                }
                if (row == 1)
                {
                    (grid_prev.Rows[row].FindControl("cb_y") as CheckBox).Visible = true;
                    (grid_prev.Rows[row].FindControl("cb_y") as CheckBox).Text = "Header";
                }
            }
        }

    }
    public void OnDataBound0(object sender, EventArgs e)
    {
        if (ug_grid.Rows.Count > 0)
        {
            for (row = 0; row < ug_grid.Rows.Count; row++)
            {
                if (row == ug_grid.Rows.Count - 3)
                {
                    (ug_grid.Rows[row].FindControl("cb_x") as CheckBox).Visible = true;
                    (ug_grid.Rows[row].FindControl("cb_x1") as CheckBox).Visible = true;
                }
                if (row == 1)
                {
                    (ug_grid.Rows[row].FindControl("cb_x") as CheckBox).Visible = true;
                    (ug_grid.Rows[row].FindControl("cb_x") as CheckBox).Text = "Header";
                }
            }
        }
    }
    protected void ddl_PrevDate_OnSelectedIndexChange(object sender, EventArgs e)
    {
        fromDropDown = true;
        loadOldSetting();
    }
    protected void ddl_collegeSelectedindexchange(object sender, EventArgs e)
    {
        fromDropDown = true;
        loadOldSetting();
    }
    protected void btn_exit_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Hostel.aspx");
    }
    protected void btn_reset_Click(object sender, EventArgs e)
    {
        clearGridview();
    }
    public void clearGridview()
    {
        ArrayList addnew = new ArrayList();
        addnew.Add("Item Header Code");
        addnew.Add("Item Code");
        addnew.Add("Customer Code");
        addnew.Add("Vendor Code");
        addnew.Add("Reruisition Code");
        addnew.Add("Quotation Code");
        addnew.Add("Order Code");
        addnew.Add("Inward Code");
        addnew.Add("Return Code");
        addnew.Add("Asset Code");
        addnew.Add("Menu Id");
        addnew.Add("Request Id");
        addnew.Add("Book Request Id");
        addnew.Add("Vendor Quotation No");
        ug_grid.Visible = true;

        DataTable dt = new DataTable();
        dt.Columns.Add("Dummy");
        dt.Columns.Add("Dummy1");
        dt.Columns.Add("Dummy2");
        dt.Columns.Add("Dummy3");
        dt.Columns.Add("Dummy4");
        dt.Columns.Add("Dummay5");
        DataRow dr;
        for (row = 0; row < addnew.Count; row++)
        {
            dr = dt.NewRow();
            dr[0] = "1";
            dr[1] = Convert.ToString(addnew[row]);
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "";
            dt.Rows.Add(dr);
        }
        if (dt.Rows.Count > 0)
        {
            ug_grid.DataSource = dt;
            ug_grid.DataBind();
        }
        txt_frmdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_prvdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }
    protected void btn_errclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    public void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }


    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
        //btn_go_Click(sender, e);

    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        imgdiv2.Visible = false;

    }
    public void delete()
    {
        try
        {
            string fromdate = Convert.ToString(txt_frmdate.Text);
            string[] splitdate = fromdate.Split('/');
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            string clgcode = "";
            if (ddl_collegename.Items.Count > 0)
            {
                clgcode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            string q = "delete IM_CodeSettings where StartDate='" + dt.ToString("MM-dd-yyyy") + "' and collegecode='" + clgcode + "'";
            int del = d2.update_method_wo_parameter(q, "Text");
            if (del != 0)
            {
                surediv.Visible = false;
                lbl_alerterr.Visible = true;
                lbl_alerterr.Text = "Deleted successfully";
                imgdiv2.Visible = true;
                loadOldSetting();
                BindGridview();

            }
            else
            {
                surediv.Visible = false;
                lbl_alerterr.Visible = true;
                lbl_alerterr.Text = "Please Select Code Setting Date";
                imgdiv2.Visible = true;
            }
        }
        catch
        {
        }
    }
    public void bindcollege()
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }
        }
        catch
        {
        }
    }
}
