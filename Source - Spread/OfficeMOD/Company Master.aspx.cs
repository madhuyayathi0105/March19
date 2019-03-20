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

public partial class Company_Master : System.Web.UI.Page
{
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DAccess2 da = new DAccess2();
    DAccess2 d2 = new DAccess2();
    bool check = false;
    int i = 0;
    private object sender;
    private EventArgs e;
    string PageLogOut = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        //string sess = Convert.ToString(Session["IsLogin"]);
        //PageLogOut = Convert.ToString(Session["PageLogout"]);
        //if (sess == "")
        //{
        //}
        //else
        //{
        //    if (!Request.FilePath.Contains("OffM"))
        //    {
        //        string strPreviousPage = "";
        //        if (Request.UrlReferrer != null)
        //        {
        //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
        //        }
        //        if (strPreviousPage == "")
        //        {
        //            string redrURI = ConfigurationManager.AppSettings["Office"].Trim();
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
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("OfficeHome"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/OfficeMOD/OfficeHome.aspx");
                return;
            }
        }
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        //lblvalidation1.Visible = false;
        if (!IsPostBack)
        {
            bindCompanyname();
        }
    }
    public void bindCompanyname()
    {
        try
        {
            ds.Clear();
            cbl_companyname.Items.Clear();
            string itemname = "select distinct CompanyPK, CompName from CompanyMaster  order by CompanyPK";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_companyname.DataSource = ds;
                cbl_companyname.DataTextField = "CompName";
                cbl_companyname.DataValueField = "CompanyPK";
                cbl_companyname.DataBind();
                if (cbl_companyname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_companyname.Items.Count; i++)
                    {
                        cbl_companyname.Items[i].Selected = true;
                    }
                    txt_companyname.Text = "Company Name(" + cbl_companyname.Items.Count + ")";
                }

            }
        }
        catch
        {
        }
    }

    public void bindstate()
    {
        try
        {
            ds.Clear();
            ddl_State.Items.Clear();

            string state = " select mastercode,mastervalue from CO_MasterValues where mastercriteria='State' and CollegeCode='" + collegecode1 + "' order by MasterValue";
            ds = d2.select_method_wo_parameter(state, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_State.DataSource = ds;
                ddl_State.DataTextField = "mastervalue";
                ddl_State.DataValueField = "mastercode";
                ddl_State.DataBind();
                ddl_State.Items.Insert(0, "Select");
                //ddl_State.Items.Insert(ddl_State.Items.Count, "Others");
            }
            else
            {
                ddl_State.Items.Insert(0, "Select");
                //ddl_State.Items.Insert(ddl_State.Items.Count, "Others");
            }
        }
        catch
        {
        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname1(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();

        string query = "  select distinct  CompName from CompanyMaster WHERE CompName like '" + prefixText + "%' ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["CompName"].ToString());
            }
        }
        return name;
    }
    protected void cb_companyname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_companynamee.Checked == true)
            {
                for (int i = 0; i < cbl_companyname.Items.Count; i++)
                {
                    cbl_companyname.Items[i].Selected = true;
                }
                txt_companyname.Text = "Vendor Name(" + (cbl_companyname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_companyname.Items.Count; i++)
                {
                    cbl_companyname.Items[i].Selected = false;
                }
                txt_companyname.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void cbl_companyname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_companyname.Text = "--Select--";
            cb_companynamee.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_companyname.Items.Count; i++)
            {
                if (cbl_companyname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_companyname.Text = "Supplier Name(" + commcount.ToString() + ")";
                if (commcount == cbl_companyname.Items.Count)
                {
                    cb_companynamee.Checked = true;
                }
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
            int index;
            string colno = "";
            int j = 0;
            Printcontrol.Visible = false;
            Hashtable columnhash = new Hashtable();
            Hashtable hscolumn = new Hashtable();
            Hashtable hscolumnvalue = new Hashtable();
            columnhash.Add("CompName", "Company Name");
            columnhash.Add("CompanyAddress", "Street");
            columnhash.Add("CompanyCity", "City");
            columnhash.Add("CompanyPin", "Pincode");
            columnhash.Add("CompanyPhoneNo", "Phone No");
            columnhash.Add("CompanyFaxNo", "Fax No");
            columnhash.Add("CompanyEmailID", "Mail Id");
            columnhash.Add("CompanyWebsite", "Website");
            columnhash.Add("CompanyDist", "District");
            columnhash.Add("CompanyState", "State");
            columnhash.Add("CompanyMobileNo", "Mobile No");
            columnhash.Add("CompanyPANNo", "PAN");
            columnhash.Add("CompanyStartYear", "Business Start Year");


            if (ItemList.Count == 0)
            {

                ItemList.Add("CompName");
                ItemList.Add("CompanyAddress");
            }

            #region datatable
            DataRow drrow = null;
            DataTable dtTTDisp = new DataTable();
            dtTTDisp.Columns.Add("compk");
            dtTTDisp.Columns.Add("SNo.");
            dtTTDisp.Columns.Add("CompName");
            dtTTDisp.Columns.Add("CompanyAddress");
            dtTTDisp.Columns.Add("CompanyCity");
            dtTTDisp.Columns.Add("CompanyPin");
            dtTTDisp.Columns.Add("CompanyPhoneNo");
            dtTTDisp.Columns.Add("CompanyFaxNo");
            dtTTDisp.Columns.Add("CompanyEmailID");
            dtTTDisp.Columns.Add("CompanyWebsite");
            dtTTDisp.Columns.Add("CompanyDist");
            dtTTDisp.Columns.Add("CompanyState");
            dtTTDisp.Columns.Add("CompanyMobileNo");
            dtTTDisp.Columns.Add("CompanyPANNo");
            dtTTDisp.Columns.Add("CompanyStartYear");

            int y = dtTTDisp.Columns.Count;
            drrow = dtTTDisp.NewRow();
            drrow["compk"] = "CompanyPK";
            drrow["SNo."] = "SNo.";
            drrow["CompName"] = "Company Name";
            drrow["CompanyAddress"] = "Street";
            drrow["CompanyCity"] = "City";
            drrow["CompanyPin"] = "Pincode";
            drrow["CompanyPhoneNo"] = "Phone No";
            drrow["CompanyFaxNo"] = "Fax No";
            drrow["CompanyEmailID"] = "Mail Id";
            drrow["CompanyWebsite"] = "Website";
            drrow["CompanyDist"] = "District";
            drrow["CompanyState"] = "State";
            drrow["CompanyMobileNo"] = "Mobile No";
            drrow["CompanyPANNo"] = "PAN";
            drrow["CompanyStartYear"] = "Business Start Year";

            dtTTDisp.Rows.Add(drrow);

            #endregion

            string itemheadercode = "";
            for (int i = 0; i < cbl_companyname.Items.Count; i++)
            {
                if (cbl_companyname.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_companyname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cbl_companyname.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (txt_companyname.Text.Trim() != "--Select--" || txt_companyname2.Text.Trim() != "")
            {


                string selectqurey = "";

                if (txt_companyname2.Text.Trim() != "")
                {
                    selectqurey = "select distinct vm.CompanyPK,CompName,CompanyAddress,CompanyPin,vm.CompanyPhoneNo,CompanyFaxNo,CompanyEmailID,vm.CompanyMobileNo,CompanyWebsite,CompanyCity,CompanyDist,CompanyState,CompanyStartYear,CompanyPANNo from CompanyMaster vm,IM_CompanyContactMaster bm where CompName='" + Convert.ToString(txt_companyname2.Text) + "' and bm.CompanyFK=vm.CompanyPK";
                }
                else
                {

                    selectqurey = " select distinct vm.CompanyPK,CompName,CompanyAddress,CompanyPin,vm.CompanyPhoneNo,CompanyFaxNo,CompanyEmailID,vm.CompanyMobileNo,CompanyWebsite,CompanyCity,CompanyDist,CompanyState,CompanyStartYear,CompanyPANNo from CompanyMaster vm,IM_CompanyContactMaster bm where CompanyPK in('" + itemheadercode + "') and bm.CompanyFK=vm.CompanyPK order by vm.CompanyPK";

                }
                ds.Clear();
                ds = da.select_method_wo_parameter(selectqurey, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    pcolumnorder.Visible = true;





                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                    for (j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        colno = Convert.ToString(ds.Tables[0].Columns[j]);
                        if (ItemList.Contains(Convert.ToString(colno)))
                        {
                            index = ItemList.IndexOf(Convert.ToString(colno));
                            string loadval = colno;
                            //Convert.ToString(columnhash[colno]);
                            hscolumn.Add(j, loadval);
                            // hscolumnvalue.Add(loadval, printval);
                        }
                    }
                    if (dtTTDisp.Columns.Count > 0)
                    {
                        for (int im = 2; im < dtTTDisp.Columns.Count; im++)
                        {
                            string coluname = dtTTDisp.Columns[im].ToString();
                            if (hscolumn.ContainsValue(dtTTDisp.Columns[im].ToString()))
                            {
                            }
                            else
                            {
                                dtTTDisp.Columns.Remove(dtTTDisp.Columns[im].ToString());




                                im--;


                            }
                        }
                    }
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        drrow = dtTTDisp.NewRow();

                        drrow["SNo."] = Convert.ToString(i + 1);
                        drrow["compk"] = Convert.ToString(ds.Tables[0].Rows[i]["CompanyPK"]);

                        for (j = 0; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (ItemList.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
                            {

                                string col = Convert.ToString(ds.Tables[0].Columns[j].ToString());
                                drrow[col] = ds.Tables[0].Rows[i][j].ToString();

                                string colunms = Convert.ToString(ds.Tables[0].Columns[j].ToString());
                                if (colunms == "CompanyDist")
                                {
                                    string district = d2.GetFunction("select mastervalue from co_mastervalues where mastercriteria='district' and mastercode='" + Convert.ToString(ds.Tables[0].Rows[i][j].ToString()) + "'");
                                    if (district.Trim() == "0")
                                    {
                                        district = "";
                                    }

                                    drrow[colunms] = district;

                                }
                                if (colunms == "CompanyState")
                                {
                                    string state = d2.GetFunction("select mastervalue from co_mastervalues where mastercriteria='State' and mastercode='" + Convert.ToString(ds.Tables[0].Rows[i][j].ToString()) + "'");
                                    if (state.Trim() == "0")
                                    {
                                        state = "";
                                    }
                                    drrow[colunms] = state;

                                }
                            }
                        }

                        dtTTDisp.Rows.Add(drrow);
                    }
                    gview.Visible = true;
                    rptprint.Visible = true;
                    div1.Visible = true;
                    lbl_error.Visible = false;
                    pcolumnorder.Visible = true;
                    pheaderfilter.Visible = true;
                    if (dtTTDisp.Rows.Count > 1)
                    {

                        gview.DataSource = dtTTDisp;

                        gview.DataBind();

                        gview.Visible = true;

                        RowHead(gview);
                    }

                }
                else
                {
                    gview.Visible = false;
                    rptprint.Visible = false;
                    div1.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "No Records Found";
                    pcolumnorder.Visible = false;
                    pheaderfilter.Visible = false;
                }


            }
            else
            {
                gview.Visible = false;
                rptprint.Visible = false;
                div1.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please select all fields";
                pcolumnorder.Visible = false;
                pheaderfilter.Visible = false;
            }

            txt_companyname2.Text = "";
            if (gview.Rows.Count > 1)
            {
                for (int row = 0; row < gview.Rows.Count; row++)
                {

                    gview.Rows[row].Cells[0].Visible = false;
                }

            }

        }




        catch
        {
        }

    }
    protected void RowHead(GridView gview)
    {
        for (int head = 0; head < 1; head++)
        {
            gview.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gview.Rows[head].Font.Bold = true;
            gview.Rows[head].HorizontalAlign = HorizontalAlign.Center;
            gview.Rows[head].Font.Name = "Book Antique";
        }
    }
    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            bindstate();

            btn_congo.Text = "Save";
            btn_save.Visible = true;
            btn_update.Visible = false;
            btn_delete.Visible = false;
            poperrjs.Visible = true;
            Clear();
            txt_startyear.Text = Convert.ToString(System.DateTime.Now.ToString("yyyy"));
        }
        catch
        {
        }

    }
    public void Clear()
    {
        try
        {


            rdb_vendor.Checked = true;
            rdb_customer.Visible = false;
            txt_vendorname1.Text = "";
            txt_street.Text = "";
            txt_city.Text = "";
            txt_pin.Text = "";
            txt_district.Text = "";
            ddl_State.SelectedItem.Text = "Select";
            ddl_district.SelectedIndex = 0;
            //ddlbis.SelectedItem.Text = Convert.ToString(System.DateTime.Now.ToString("yyyy"));
            txt_startyear.Text = "";
            txt_code.Text = "";
            txt_phn.Text = "";
            txtfax.Text = "";
            txt_email.Text = "";
            txt_web.Text = "";
            txt_cst.Text = "";
            txt_tin.Text = "";
            txt_mainmobileno.Text = "";
            txt_pan.Text = "";
            ContactGrid.DataSource = null;
            ContactGrid.DataBind();
            ContactGrid.Visible = false;
            SelectdptGrid.DataSource = null;
            SelectdptGrid.DataBind();
            SelectdptGrid.Visible = false;
        }
        catch
        {
        }
    }
    public void cb_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string si = "";
            int j = 0;
            if (cb_column.Checked == true)
            {
                ItemList.Clear();
                for (i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    ItemList.Add(cblcolumnorder.Items[i].Value.ToString());
                    Itemindex.Add(si);
                }
                lnk_columnorder.Visible = true;
                tborder.Visible = true;
                tborder.Text = "";
                for (i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    tborder.Text = tborder.Text + ItemList[i].ToString();
                    tborder.Text = tborder.Text + "(" + (j).ToString() + ")  ";
                }
            }
            else
            {
                for (i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                    lnk_columnorder.Visible = false;
                    ItemList.Clear();
                    Itemindex.Clear();
                    cblcolumnorder.Items[0].Enabled = false;
                }
                tborder.Text = "";
                tborder.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void lb_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder.ClearSelection();
            cb_column.Checked = false;
            lnk_columnorder.Visible = false;
            ItemList.Clear();
            Itemindex.Clear();
            tborder.Text = "";
            tborder.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    public void cbl_columnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index;
            string value = "";
            string result = "";
            string sindex = "";
            cb_column.Checked = false;
            cblcolumnorder.Items[0].Selected = true;
            cblcolumnorder.Items[0].Enabled = false;
            value = string.Empty;
            result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            sindex = Convert.ToString(index);
            if (cblcolumnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    ItemList.Add(cblcolumnorder.Items[index].Value.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cblcolumnorder.Items[index].Value.ToString());
                Itemindex.Remove(sindex);
            }
            for (i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                if (cblcolumnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblcolumnorder.Items[i].Value.ToString());
                    Itemindex.Remove(sindex);
                }
            }
            lnk_columnorder.Visible = true;
            tborder.Visible = true;
            tborder.Text = "";
            for (i = 0; i < ItemList.Count; i++)
            {
                tborder.Text = tborder.Text + ItemList[i].ToString();

                tborder.Text = tborder.Text + "(" + (i + 1).ToString() + ")  ";

            }
            if (ItemList.Count == 22)
            {
                cb_column.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                tborder.Visible = false;
                lnk_columnorder.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string ss = null;
            string degreedetails = "Company Master";
            string pagename = "Company Master.aspx";
            Printcontrol.loadspreaddetails(gview, pagename, degreedetails, 0, ss);
            ////Printcontrol.loadspreaddetails(attnd_report, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        { //d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx");
        }
    }
    public void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txtexcelname.Text; ;
            if (report.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(gview, report);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
            }
            lblvalidation1.Focus();
        }
        catch (Exception ex)
        {
            lblvalidation1.Text = ex.ToString();
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    protected void ddl_State_Selectindexchange(object sender, EventArgs e)
    {
        binddistrict();
    }

    protected void btncontact_click(object sender, EventArgs e)
    {
        popcon.Visible = true;
        txt_connam.Text = "";
        txt_conpn.Text = "";
        txt_designation.Text = "";
        txt_conmob.Text = "";
        txt_confax.Text = "";
        txt_conmail.Text = "";


    }
    protected void btn_update_Click(object sender, EventArgs e)
    {
        try
        {
            string dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = DateTime.Now.ToLongTimeString();

            string type = "";
            string vendorname = Convert.ToString(txt_vendorname1.Text);
            string street = Convert.ToString(txt_street.Text);
            string city = Convert.ToString(txt_city.Text);
            string VendorPincode = Convert.ToString(txt_pin.Text);
            string dist = Convert.ToString(ddl_district.SelectedItem.Text);

            string district = Convert.ToString(ddl_district.SelectedItem.Value);
            string state = Convert.ToString(ddl_State.SelectedItem.Value);
            string year = Convert.ToString(txt_startyear.Text);
            if (district.Trim() == "Select")
            {
                district = "0";
            }
            if (state.Trim() == "Select")
            {
                state = "0";
            }
            string phoneno = Convert.ToString(txt_phn.Text);
            string faxno = Convert.ToString(txtfax.Text);
            string VendorEmailIDid = Convert.ToString(txt_email.Text);
            string website = Convert.ToString(txt_web.Text);
            string cstno = Convert.ToString(txt_cst.Text);
            string tinno = Convert.ToString(txt_tin.Text);
            string panno = Convert.ToString(txt_pan.Text);
            string moblieno = Convert.ToString(txt_mainmobileno.Text);
            string compk = txt_code.Text;

            string updatevenmaster = "update CompanyMaster set CompName='" + vendorname + "',CompanyAddress='" + street + "',CompanyPin='" + VendorPincode + "',CompanyPhoneNo='" + phoneno + "',CompanyFaxNo='" + faxno + "',CompanyEmailID='" + VendorEmailIDid + "',CompanyWebsite='" + website + "',CompanyCity='" + city + "',CompanyDist='" + district + "',CompanyState='" + state + "',CompanyStartYear='" + year + "',CompanyPANNo='" + panno + "',CompanyMobileNo='" + moblieno + "'  where CompanyPK='" + compk + "'";
            int inst = da.update_method_wo_parameter(updatevenmaster, "Text");
            if (inst != 0)
            {
                if (SelectdptGrid.Rows.Count > 0)
                {



                    for (int row = 0; row < SelectdptGrid.Rows.Count; row++)
                    {
                        string course = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_coursecode") as Label).Text);
                        string degree = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_branchcode") as Label).Text);
                        string deptcode = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_deptcode") as Label).Text);
                        string intquery = "if not exists (select * from IM_CompanyDept where deptcode='" + deptcode + "' and CompanyFK='" + compk + "')  insert into IM_CompanyDept (course,degree,deptcode,CompanyFK) values ('" + course + "','" + degree + "','" + deptcode + "','" + compk + "')";
                        int va = da.update_method_wo_parameter(intquery, "Text");
                    }
                }

                if (ContactGrid.Rows.Count > 0)
                {
                    for (int row = 0; row < ContactGrid.Rows.Count; row++)
                    {
                        string contactname = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_name") as Label).Text);
                        string designame = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_designation") as Label).Text);
                        string contactphone = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_phoneno") as Label).Text);
                        string contactmobile = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_mobileno") as Label).Text);
                        string contactfaxno = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_faxno") as Label).Text);
                        string contactVendorEmailID = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_email") as Label).Text);

                        string vencondetail = "update IM_CompanyContactMaster set ComContactName='" + contactname + "',ComContactDesig='" + designame + "',CompanyPhoneNo='" + contactphone + "',CompanyExtNo='" + contactfaxno + "',CompanyMobileNo='" + contactmobile + "',CompanyEmail='" + contactVendorEmailID + "' where CompanyFK='" + compk + "' and ComContactName='" + contactname + "'";

                        int cont = da.update_method_wo_parameter(vencondetail, "Text");
                    }
                }
                bindCompanyname();
                imgdiv2.Visible = true;
                lbl_alert.Text = "Updated Successfully";
                btn_go_Click(sender, e);
                poperrjs.Visible = false;

            }
        }
        catch
        {
        }
    }
    protected void btn_delete_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_delete.Text == "Delete")
            {
                surediv.Visible = true;
                lbl_sure.Text = "Do you want delete this record?";
            }
        }
        catch
        {
        }
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        imgdiv2.Visible = false;
        poperrjs.Visible = true;
    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
    }
    protected void delete()
    {
        try
        {
            surediv.Visible = false;
            string compk = Convert.ToString(txt_code.Text);


            string nicedeletequery = " delete from CompanyMaster where CompanyPK ='" + compk + "'";

            nicedeletequery = nicedeletequery + " delete from IM_CompanyContactMaster where CompanyFK ='" + compk + "'";

            int del = da.update_method_wo_parameter(nicedeletequery, "Text");
            if (del != 0)
            {
                bindCompanyname();
                imgdiv2.Visible = true;
                lbl_alert.Text = "Deleted Successfully";
                btn_go_Click(sender, e);
                poperrjs.Visible = false;
            }
        }
        catch
        {
        }
    }
    protected void btn_exit_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            string dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = DateTime.Now.ToLongTimeString();

            string type = "";
            if (rdb_vendor.Checked == true)
            {
                type = "0";
            }
            string vendorname = Convert.ToString(txt_vendorname1.Text);
            vendorname = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(vendorname);
            string street = Convert.ToString(txt_street.Text);
            string city = Convert.ToString(txt_city.Text);
            string VendorPincode = Convert.ToString(txt_pin.Text);
            string state = "";
            if (ddl_State.SelectedItem.Text != "Select")
            {
                state = Convert.ToString(ddl_State.SelectedItem.Value);
            }
            else
            {

            }
            string district = "";
            if (ddl_State.SelectedItem.Text != "Select")
            {
                if (ddl_district.SelectedItem.Text != "Select")
                {
                    district = Convert.ToString(ddl_district.SelectedItem.Value);
                }
                else
                {

                }
            }
            if (district.Trim() == "")
            {
                district = "0";
            }

            string year = Convert.ToString(txt_startyear.Text);



            string phoneno = Convert.ToString(txt_phn.Text);
            string faxno = Convert.ToString(txtfax.Text);
            string VendorEmailIDid = Convert.ToString(txt_email.Text);
            string website = Convert.ToString(txt_web.Text);
            string cstno = Convert.ToString(txt_cst.Text);
            string tinno = Convert.ToString(txt_tin.Text);
            string panno = Convert.ToString(txt_pan.Text);
            string moblieno = Convert.ToString(txt_mainmobileno.Text);


            int cont = 0;
            int va = 0;
            int inst = 0;
            if (SelectdptGrid.Rows.Count > 0 && ContactGrid.Rows.Count > 0)
            {
                string insertquery = "insert into CompanyMaster (CompName,CompanyAddress,CompanyPin,CompanyPhoneNo,CompanyFaxNo,CompanyEmailID,CompanyWebsite,CompanyCity,CompanyDist,CompanyState,CompanyStartYear,CompanyPANNo,CompanyMobileNo)values('" + vendorname + "','" + street + "','" + VendorPincode + "','" + phoneno + "','" + faxno + "','" + VendorEmailIDid + "','" + website + "','" + city + "','" + district + "','" + state + "','" + year + "','" + panno + "','" + moblieno + "')";
                inst = da.update_method_wo_parameter(insertquery, "Text");

                string vendoritemfk = d2.GetFunction("select CompanyPK from CompanyMaster where CompName='" + vendorname + "' order by CompanyPK desc");




                for (int row = 0; row < SelectdptGrid.Rows.Count; row++)
                {
                    string course = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_coursecode") as Label).Text);
                    string degree = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_branchcode") as Label).Text);
                    string deptcode = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_deptcode") as Label).Text);
                    string intquery = " insert into IM_CompanyDept (course,degree,deptcode,CompanyFK) values ('" + course + "','" + degree + "','" + deptcode + "','" + vendoritemfk + "')";
                    va = da.update_method_wo_parameter(intquery, "Text");
                }



                for (int row = 0; row < ContactGrid.Rows.Count; row++)
                {
                    string contactname = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_name") as Label).Text);
                    string designame = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_designation") as Label).Text);
                    string contactphone = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_phoneno") as Label).Text);
                    string contactmobile = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_mobileno") as Label).Text);
                    string contactfaxno = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_faxno") as Label).Text);
                    string contactVendorEmailID = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_email") as Label).Text);
                    string vquery = "insert into  IM_CompanyContactMaster (ComContactName,ComContactDesig,CompanyPhoneNo,CompanyMobileNo,CompanyExtNo,CompanyEmail,CompanyFK)";
                    vquery = vquery + "  values ('" + contactname + "','" + designame + "','" + contactphone + "','" + contactmobile + "','" + contactfaxno + "','" + contactVendorEmailID + "','" + vendoritemfk + "')";
                    cont = da.update_method_wo_parameter(vquery, "Text");
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please select Contact details and Department details";
            }


            if (inst != 0 && cont != 0)
            {
                bindCompanyname();
                imgdiv2.Visible = true;
                lbl_alert.Text = "Saved Successfully";
                btn_addnew_Click(sender, e);
                btn_go_Click(sender, e);
                poperrjs.Visible = true;
                Session["contactdata"] = null;


                ContactGrid.DataSource = null;
                ContactGrid.DataBind();
            }
        }
        catch
        {
        }
    }
    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {
        popcon.Visible = false;
    }
    protected void btn_congo_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_congo.Text == "Save")
            {
                DataTable dt = new DataTable();
                DataView dv = new DataView();
                DataRow dr;
                dt.Columns.Add("Name");
                dt.Columns.Add("Designation");
                dt.Columns.Add("Phone");
                dt.Columns.Add("Mobile No");
                dt.Columns.Add("Fax No");
                dt.Columns.Add("Email");
                bool chk = false;
                if (Session["contactdata"] != null)
                {
                    DataTable d1 = new DataTable();
                    d1 = (DataTable)Session["contactdata"];
                    if (d1.Rows.Count > 0)
                    {
                        for (int r = 0; r < d1.Rows.Count; r++)
                        {
                            string name = d1.Rows[0]["Name"].ToString();
                            dr = dt.NewRow();
                            for (int c = 0; c < d1.Columns.Count; c++)
                            {
                                //if (name == Convert.ToString(d1.Rows[r][0]))
                                //{

                                //}
                                //else
                                //{
                                dr[c] = Convert.ToString(d1.Rows[r][c]);
                                //}
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
                dr = dt.NewRow();
                dr[0] = Convert.ToString(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(txt_connam.Text));
                dr[1] = Convert.ToString(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(txt_designation.Text));
                dr[2] = Convert.ToString(txt_conpn.Text);
                dr[3] = Convert.ToString(txt_conmob.Text);
                dr[4] = Convert.ToString(txt_confax.Text);
                dr[5] = Convert.ToString(txt_conmail.Text);

                dt.Rows.Add(dr);
                if (dt.Rows.Count > 0)
                {
                    ContactGrid.DataSource = dt;
                    ContactGrid.DataBind();
                    ContactGrid.Visible = true;
                    Session["contactdata"] = dt;
                    popcon.Visible = false;
                }
                else
                {
                    popcon.Visible = true;
                }
            }
            else
            {
                string name = Convert.ToString(txt_connam.Text);
                string desing = Convert.ToString(txt_designation.Text);
                string ph = Convert.ToString(txt_conpn.Text);
                string mob = Convert.ToString(txt_conmob.Text);
                string fax = Convert.ToString(txt_confax.Text);
                string VendorEmailID = Convert.ToString(txt_conmail.Text);
                int row = Convert.ToInt32(Session["row_new_Value"]);
                (ContactGrid.Rows[row].FindControl("lbl_name") as Label).Text = Convert.ToString(name);
                (ContactGrid.Rows[row].FindControl("lbl_designation") as Label).Text = Convert.ToString(desing);
                (ContactGrid.Rows[row].FindControl("lbl_phoneno") as Label).Text = Convert.ToString(ph);
                (ContactGrid.Rows[row].FindControl("lbl_mobileno") as Label).Text = Convert.ToString(mob);
                (ContactGrid.Rows[row].FindControl("lbl_faxno") as Label).Text = Convert.ToString(fax);
                (ContactGrid.Rows[row].FindControl("lbl_email") as Label).Text = Convert.ToString(VendorEmailID);
                popcon.Visible = false;
                btn_congo.Text = "Save";
            }
        }
        catch
        {
        }
    }
    protected void btn_conexit_Click(object sender, EventArgs e)
    {
        popcon.Visible = false;
    }
    protected void ContactGrid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int row = Convert.ToInt32(e.CommandArgument);
            Session["row_new_Value"] = Convert.ToString(row);
            if (e.CommandName == "instruction")
            {
                string Name = ((ContactGrid.Rows[row].FindControl("lbl_name") as Label).Text);
                string Designation = ((ContactGrid.Rows[row].FindControl("lbl_designation") as Label).Text);
                string phonenumber = ((ContactGrid.Rows[row].FindControl("lbl_phoneno") as Label).Text);
                string Mobileno = ((ContactGrid.Rows[row].FindControl("lbl_mobileno") as Label).Text);
                string fax = ((ContactGrid.Rows[row].FindControl("lbl_faxno") as Label).Text);
                string VendorEmailID = ((ContactGrid.Rows[row].FindControl("lbl_email") as Label).Text);

                txt_connam.Text = Convert.ToString(Name);
                txt_designation.Text = Convert.ToString(Designation);
                txt_conpn.Text = Convert.ToString(phonenumber);
                txt_conmob.Text = Convert.ToString(Mobileno);
                txt_confax.Text = Convert.ToString(fax);
                txt_conmail.Text = Convert.ToString(VendorEmailID);
                btn_congo.Text = "Update";
                popcon.Visible = true;
            }
        }
        catch
        {
        }
    }

    protected void typegrid_OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int ro = 0; ro < e.Row.Cells.Count; ro++)
                {
                    e.Row.Cells[ro].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.ContactGrid, "instruction$" + e.Row.RowIndex);
                }
            }
        }
        catch
        {
        }
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    public void binddistrict()
    {
        try
        {
            ds1.Clear();
            ddl_district.Items.Clear();
            if (ddl_State.SelectedItem.Text.Trim() != "Select")
            {
                string dist = " select  mastercode,mastervalue from CO_MasterValues where mastercriteria='District' and  MasterCriteriaValue2='" + ddl_State.SelectedItem.Text + "' and CollegeCode ='" + collegecode1 + "' order by MasterValue";

                ds1 = d2.select_method_wo_parameter(dist, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    ddl_district.DataSource = ds1;
                    ddl_district.DataTextField = "mastervalue";
                    ddl_district.DataValueField = "mastercode";
                    ddl_district.DataBind();
                    ddl_district.Items.Insert(0, "Select");
                }
                else
                {
                    ddl_district.Items.Insert(0, "Select");
                }
            }
            else
            {
                ddl_district.Items.Insert(0, "Select");
            }
        }
        catch
        {
        }
    }
    protected void gview_onselectedindexchanged(Object sender, EventArgs e)
    {
        try
        {
            Clear();
            bindstate();

            poperrjs.Visible = true;
            btn_save.Visible = false;
            btn_update.Visible = true;
            btn_delete.Visible = true;
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            string activerow = Convert.ToString(rowIndex);
            string activecol = Convert.ToString(selectedCellIndex);
            if (activerow.Trim() != "")
            {
                int row = Convert.ToInt32(activerow);
                int col = Convert.ToInt32(activecol);
                string compk = Convert.ToString(gview.Rows[row].Cells[0].Text);
                txt_code.Text = compk;
                //  string vendorpk = d2.GetFunction("select VendorPK FROM CO_VendorMaster where VendorCode='" + vendorcode + "'");
                string selectquery = "select CompanyPK,CompName,CompanyAddress,CompanyStreet,CompanyCity,CompanyDist,CompanyState,CompanyPin,CompanyPhoneNo,CompanyFaxNo,CompanyEmailID,CompanyWebsite,CompanyStartYear,CompanyPANNo,CompanyMobileNo  from CompanyMaster where  CompanyPK='" + compk + "'";

                selectquery = selectquery + " select vc.ComContactName,vc.ComContactDesig,vc.CompanyPhoneNo,vc.CompanyMobileNo,vc.CompanyExtNo,vc.CompanyEmail from IM_CompanyContactMaster vc where CompanyFK='" + compk + "'";
                selectquery = selectquery + " select course,degree,deptcode from IM_CompanyDept vc where CompanyFK='" + compk + "'";

                ds.Clear();
                ds = da.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txt_vendorname1.Text = Convert.ToString(ds.Tables[0].Rows[0]["CompName"]);
                    txt_street.Text = Convert.ToString(ds.Tables[0].Rows[0]["CompanyAddress"]);
                    txt_city.Text = Convert.ToString(ds.Tables[0].Rows[0]["CompanyCity"]);
                    txt_pin.Text = Convert.ToString(ds.Tables[0].Rows[0]["CompanyPin"]);

                    string statecode = Convert.ToString(ds.Tables[0].Rows[0]["CompanyState"]);
                    if (statecode.Trim() != "")
                    {
                        string State = d2.GetFunction("select mastervalue from CO_MasterValues where mastercode='" + statecode + "' and MasterCriteria='State'");

                        ddl_State.SelectedIndex = ddl_State.Items.IndexOf(ddl_State.Items.FindByText(State));
                        binddistrict();
                    }
                    string district = Convert.ToString(ds.Tables[0].Rows[0]["CompanyDist"]);
                    if (district.Trim() != "")
                    {
                        district = d2.GetFunction("select mastervalue from CO_MasterValues where mastercode='" + district + "' and MasterCriteria='District'");

                        ddl_district.SelectedIndex = ddl_district.Items.IndexOf(ddl_district.Items.FindByText(district));
                    }
                    txt_startyear.Text = Convert.ToString(ds.Tables[0].Rows[0]["CompanyStartYear"]);





                    rdb_vendor.Checked = true;
                    txt_phn.Text = Convert.ToString(ds.Tables[0].Rows[0]["CompanyPhoneNo"]);
                    txtfax.Text = Convert.ToString(ds.Tables[0].Rows[0]["CompanyFaxNo"]);
                    txt_email.Text = Convert.ToString(ds.Tables[0].Rows[0]["CompanyEmailID"]);
                    txt_web.Text = Convert.ToString(ds.Tables[0].Rows[0]["CompanyWebsite"]);

                    txt_mainmobileno.Text = Convert.ToString(ds.Tables[0].Rows[0]["CompanyMobileNo"]);


                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        DataView dv = new DataView();
                        DataRow dr;
                        dt.Columns.Add("course name");
                        dt.Columns.Add("course Code");
                        dt.Columns.Add("degree name");
                        dt.Columns.Add("degree Code");
                        dt.Columns.Add("Dept Name");
                        dt.Columns.Add("Dept Code");
                        DataTable d1 = new DataTable();
                        string supply = "";
                        bool niceflage = false;
                        d1 = ds.Tables[2];
                        if (d1.Rows.Count > 0)
                        {
                            for (int r = 0; r < d1.Rows.Count; r++)
                            {
                                dr = dt.NewRow();
                                niceflage = false;

                                dr["course name"] = d1.Rows[r]["course"];
                                dr["course Code"] = d1.Rows[r]["course"];
                                string getdegree = da.GetFunction("select course_name from course where course_id='" + d1.Rows[r]["degree"] + "'");

                                dr["degree Code"] = d1.Rows[r]["degree"];
                                dr["degree name"] = getdegree;
                                string getdeptname = da.GetFunction("select Dept_Name  from Department where dept_code in(select dept_code from degree where degree_code ='" + d1.Rows[r]["deptcode"] + "')");

                                dr["Dept Code"] = d1.Rows[r]["deptcode"];
                                dr["Dept name"] = getdeptname;




                                dt.Rows.Add(dr);
                            }
                        }
                        if (dt.Rows.Count > 0)
                        {
                            SelectdptGrid.DataSource = dt;
                            SelectdptGrid.DataBind();
                            SelectdptGrid.Visible = true;
                        }
                    }
                    else
                    {
                        SelectdptGrid.Visible = true;
                    }


                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        DataRow dr;
                        dt.Columns.Add("Name");
                        dt.Columns.Add("Designation");
                        dt.Columns.Add("Phone");
                        dt.Columns.Add("Mobile No");
                        dt.Columns.Add("Fax No");
                        dt.Columns.Add("Email");
                        DataTable d1 = new DataTable();
                        d1 = ds.Tables[1];
                        if (d1.Rows.Count > 0)
                        {
                            for (int r = 0; r < d1.Rows.Count; r++)
                            {
                                dr = dt.NewRow();
                                for (int c = 0; c < d1.Columns.Count; c++)
                                {
                                    dr[c] = Convert.ToString(d1.Rows[r][c]);
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                        if (dt.Rows.Count > 0)
                        {
                            ContactGrid.DataSource = dt;
                            ContactGrid.DataBind();
                            ContactGrid.Visible = true;
                        }
                    }
                    else
                    {
                        ContactGrid.Visible = true;
                    }

                }

            }
        }
        catch
        {
        }
    }
    protected void OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }
    protected void btnitm_click(object sender, EventArgs e)
    {



        //cbl_subheader.Visible = false;
        cbldepartment.Items.Clear();
        cbldegree.Items.Clear();
        //binditem();
        cb_course.Checked = false;
        bindedu();
        //Clear();
        popitm.Visible = true;
        for (int row = 0; row < SelectdptGrid.Rows.Count; row++)
        {
            string branch = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_branchcode") as Label).Text);
            string deptcode = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_deptcode") as Label).Text);
            string course = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_coursecode") as Label).Text);
            if (!string.IsNullOrEmpty(course))
                cblcourse.Items.FindByValue(course).Selected = true;
        }
        binddegree();
        for (int row = 0; row < SelectdptGrid.Rows.Count; row++)
        {
            string branch = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_branchcode") as Label).Text);

            if (!string.IsNullOrEmpty(branch))
                cbldegree.Items.FindByValue(branch).Selected = true;
        }
        binddepartment();
        for (int row = 0; row < SelectdptGrid.Rows.Count; row++)
        {
            string deptcode = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_deptcode") as Label).Text);
            if (!string.IsNullOrEmpty(deptcode))
                cbldepartment.Items.FindByValue(deptcode).Selected = true;
        }

    }

    public void binddegree()
    {
        try
        {

            cb_degree.Checked = false;
            string typ = "";
            if (cblcourse.Items.Count > 0)
            {
                for (int i = 0; i < cblcourse.Items.Count; i++)
                {
                    if (cblcourse.Items[i].Selected == true)
                    {
                        if (typ == "")
                        {
                            typ = "" + cblcourse.Items[i].Value + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + cblcourse.Items[i].Value + "";
                        }
                    }

                }
            }
            if (typ != "")
            {
                string deptquery = "select distinct d.Course_Id,c.Course_Name from Degree d,course c ,DeptPrivilages p where p.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + collegecode1 + "' and Edu_Level in('" + typ + "') ";
                ds.Clear();
                ds = da.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbldegree.DataSource = ds;
                    cbldegree.DataTextField = "Course_Name";
                    cbldegree.DataValueField = "Course_Id";
                    cbldegree.DataBind();
                }
            }
            binddepartment();
        }
        catch
        {
        }
    }

    public void binddepartment()
    {
        try
        {

            cb_departemt.Checked = false;
            string typ = "";
            if (cbldegree.Items.Count > 0)
            {
                for (int i = 0; i < cbldegree.Items.Count; i++)
                {
                    if (cbldegree.Items[i].Selected == true)
                    {
                        if (typ == "")
                        {
                            typ = "" + cbldegree.Items[i].Value + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + cbldegree.Items[i].Value + "";
                        }
                    }

                }
            }
            if (typ != "")
            {
                string deptquery = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + typ + "') and  degree.college_code='" + collegecode1 + "'";
                ds.Clear();
                ds = da.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbldepartment.DataSource = ds;
                    cbldepartment.DataTextField = "dept_name";
                    cbldepartment.DataValueField = "degree_code";
                    cbldepartment.DataBind();
                }
            }
        }
        catch
        {
        }
    }
    public void bindedu()
    {
        try
        {
            string deptquery = " select distinct course.Edu_Level from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code='" + collegecode1 + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblcourse.DataSource = ds;
                cblcourse.DataTextField = "Edu_Level";
                cblcourse.DataValueField = "Edu_Level";
                cblcourse.DataBind();
            }
            binddegree();
        }
        catch
        {
        }

    }
    protected void cb_degree_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (cbldegree.Items.Count > 0)
            {
                if (cb_degree.Checked == true)
                {
                    for (int i = 0; i < cbldegree.Items.Count; i++)
                    {
                        cbldegree.Items[i].Selected = true;
                    }
                }
                else
                {
                    for (int i = 0; i < cbldegree.Items.Count; i++)
                    {
                        cbldegree.Items[i].Selected = false;
                    }
                }
            }

            binddepartment();

        }
        catch
        {
        }
    }
    protected void cb_course_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (cblcourse.Items.Count > 0)
            {
                if (cb_course.Checked == true)
                {
                    for (int i = 0; i < cblcourse.Items.Count; i++)
                    {
                        cblcourse.Items[i].Selected = true;
                    }
                }
                else
                {
                    for (int i = 0; i < cblcourse.Items.Count; i++)
                    {
                        cblcourse.Items[i].Selected = false;
                    }
                }
            }

            binddegree();

        }
        catch
        {
        }
    }
    protected void cbdepartment_Change(object sender, EventArgs e)
    {
        try
        {
            if (cbldepartment.Items.Count > 0)
            {
                if (cb_departemt.Checked == true)
                {
                    for (int i = 0; i < cbldepartment.Items.Count; i++)
                    {
                        cbldepartment.Items[i].Selected = true;
                    }
                }
                else
                {
                    for (int i = 0; i < cbldepartment.Items.Count; i++)
                    {
                        cbldepartment.Items[i].Selected = false;
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void btn_save1_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            DataView dv = new DataView();
            DataRow dr;
            dt.Columns.Add("course name");
            dt.Columns.Add("course Code");
            dt.Columns.Add("degree name");
            dt.Columns.Add("degree Code");
            dt.Columns.Add("Dept Name");
            dt.Columns.Add("Dept Code");

            DataTable d1 = new DataTable();

            //if (d1.Rows.Count > 0)
            //{
            //    for (int r = 0; r < d1.Rows.Count; r++)
            //    {
            //        dr = dt.NewRow();
            //        for (int c = 0; c < d1.Columns.Count; c++)
            //        {
            //            dr[c] = Convert.ToString(d1.Rows[r][c]);
            //        }
            //        dt.Rows.Add(dr);
            //    }
            //}

            string val = "";

            bool checkflage = false;
            if (cblcourse.Items.Count > 0)
            {
                for (int i = 0; i < cblcourse.Items.Count; i++)
                {
                    if (cblcourse.Items[i].Selected == true)
                    {
                        string selectquery = "select distinct d.Course_Id,c.Course_Name from Degree d,course c ,DeptPrivilages p where p.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + collegecode1 + "' and Edu_Level in ('" + cblcourse.Items[i].Value + "')";
                        ds.Clear();
                        ds = da.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (cbldegree.Items.Count > 0)
                            {
                                for (int item = 0; item < cbldegree.Items.Count; item++)
                                {
                                    if (cbldegree.Items[item].Selected == true)
                                    {
                                        ds.Tables[0].DefaultView.RowFilter = "Course_Id='" + cbldegree.Items[item].Value + "'";
                                        dv = ds.Tables[0].DefaultView;
                                        if (dv.Count > 0)
                                        {
                                            string selectquerys = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + cbldegree.Items[item].Value + "') and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + collegecode1 + "'";

                                            DataSet dsp = da.select_method_wo_parameter(selectquerys, "Text");
                                            if (dsp.Tables[0].Rows.Count > 0)
                                            {
                                                if (cbldepartment.Items.Count > 0)
                                                {
                                                    for (int dept = 0; dept < cbldepartment.Items.Count; dept++)
                                                    {
                                                        if (cbldepartment.Items[dept].Selected == true)
                                                        {
                                                            dsp.Tables[0].DefaultView.RowFilter = "degree_code='" + cbldepartment.Items[dept].Value + "'";
                                                            dv = dsp.Tables[0].DefaultView;
                                                            if (dv.Count > 0)
                                                            {
                                                                for (int dep = 0; dep < dv.Count; dep++)
                                                                {
                                                                    checkflage = true;
                                                                    dr = dt.NewRow();
                                                                    dr[0] = Convert.ToString(cblcourse.Items[i].Text);
                                                                    dr[1] = Convert.ToString(cblcourse.Items[i].Value);
                                                                    dr[2] = Convert.ToString(cbldegree.Items[item].Text);
                                                                    dr[3] = Convert.ToString(cbldegree.Items[item].Value);
                                                                    dr[4] = Convert.ToString(cbldepartment.Items[dept].Text);
                                                                    dr[5] = Convert.ToString(cbldepartment.Items[dept].Value);

                                                                    dt.Rows.Add(dr);
                                                                }
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                        }




                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                SelectdptGrid.DataSource = dt;
                SelectdptGrid.DataBind();
                SelectdptGrid.Visible = true;
                popitm.Visible = false;
                //Session["itemdata"] = dt;
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Fill All Required Fields";
            }
        }
        catch
        {
        }
    }
    protected void OnDataBound(object sender, EventArgs e)
    {
        try
        {
            for (int i = SelectdptGrid.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = SelectdptGrid.Rows[i];
                GridViewRow previousRow = SelectdptGrid.Rows[i - 1];
                // for (int j = 1; j <= 1; j++)
                // {
                //lbl_itemheader
                Label lnlname = (Label)row.FindControl("lbl_course");
                Label lnlname1 = (Label)previousRow.FindControl("lbl_course");

                if (lnlname.Text == lnlname1.Text)
                {
                    if (previousRow.Cells[1].RowSpan == 0)
                    {
                        if (row.Cells[1].RowSpan == 0)
                        {
                            previousRow.Cells[1].RowSpan += 2;
                        }
                        else
                        {
                            previousRow.Cells[1].RowSpan = row.Cells[1].RowSpan + 1;
                        }
                        row.Cells[1].Visible = false;
                    }
                }
                lnlname = (Label)row.FindControl("lbl_bran");
                lnlname1 = (Label)previousRow.FindControl("lbl_bran");

                //lnname1 = (Label)row.FindControl("lbl_bran");
                //lnname1 = (Label)previousRow.FindControl("lbl_bran");

                if (lnlname.Text == lnlname1.Text)
                {
                    if (previousRow.Cells[2].RowSpan == 0)
                    {
                        if (row.Cells[2].RowSpan == 0)
                        {
                            previousRow.Cells[2].RowSpan += 2;
                        }
                        else
                        {
                            previousRow.Cells[2].RowSpan = row.Cells[2].RowSpan + 1;
                        }
                        row.Cells[2].Visible = false;
                    }
                }
                //lbl_deptname  
                lnlname = (Label)row.FindControl("lbl_deptname");
                lnlname1 = (Label)previousRow.FindControl("lbl_deptname");

                if (lnlname.Text == lnlname1.Text)
                {
                    if (previousRow.Cells[3].RowSpan == 0)
                    {
                        if (row.Cells[3].RowSpan == 0)
                        {
                            previousRow.Cells[3].RowSpan += 2;
                        }
                        else
                        {
                            previousRow.Cells[3].RowSpan = row.Cells[3].RowSpan + 1;
                        }
                        row.Cells[3].Visible = false;
                    }
                }
                //  }
            }
        }
        catch
        {
        }
    }

    protected void imagebtnpopclose3_Click(object sender, EventArgs e)
    {
        popitm.Visible = false;
    }
    protected void btn_exit1_Click(object sender, EventArgs e)
    {
        popitm.Visible = false;
    }

    protected void cblcourse_ChekedChange(object sender, EventArgs e)
    {
        binddegree();
    }
    protected void cbldegree_ChekedChange(object sender, EventArgs e)
    {
        binddepartment();
    }
}