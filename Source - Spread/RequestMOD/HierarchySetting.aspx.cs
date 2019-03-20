using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Text;
using wc = System.Web.UI.WebControls;
using System.Configuration;

public partial class HierarchySetting : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    Hashtable hat = new Hashtable();
    Hashtable hat1 = new Hashtable();
    Hashtable hat2 = new Hashtable();
    ReuasableMethods rs = new ReuasableMethods();
    DAccess2 d2 = new DAccess2();

    string usercode = "", collegecode = "", singleuser = "", group_user = "";

    DAccess2 dset = new DAccess2();
    DAccess2 da = new DAccess2();

    Boolean flag_true = false;
    Boolean flag_spread1 = false;
    SqlDataAdapter danew;
    DataSet dsload = new DataSet();
    DataSet dss;
    static string hy_order_val = "";
    static string value_con = "";
    string dept_all = string.Empty;
    string design_all = string.Empty;
    string staff_type = string.Empty;
    string staff_type1 = "";
    string dept_all1 = "";
    string design_all1 = "";
    string strbranch = string.Empty;
    string sqlstrbranch = string.Empty;
    string strstaff1 = string.Empty;
    string sqlstrstaff1 = string.Empty;
    string strstaffdept = string.Empty;
    string strstafftype = string.Empty;
    string sqlstrstafftype = string.Empty;
    string sqlstrstaffdept1 = string.Empty;
    static string pri_txt = "";
    static string con_txt = "";
    static string reqapp_pri = "";
    static string ReqAppPriority = "";
    static string gatepass_staffdept = "";
    static string name = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            string PageLogOut = "";
            string sess = Convert.ToString(Session["IsLogin"]);
            PageLogOut = Convert.ToString(Session["PageLogout"]);
            if (sess == "")
            {
            }
            else
            {
                if (!Request.FilePath.Contains("ReqM"))
                {
                    string strPreviousPage = "";
                    if (Request.UrlReferrer != null)
                    {
                        strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                    }
                    if (strPreviousPage == "")
                    {
                        string redrURI = ConfigurationManager.AppSettings["Request"].Trim();
                        Response.Redirect(redrURI, false);
                        return;
                    }
                }
            }
            if (Session["collegecode"] == null)
            {
                string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
                Response.Redirect(redrURI, false);
                return;
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();

            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            FpSpread1.SaveChanges();
            FpSpread2.SaveChanges();
            if (!Page.IsPostBack)
            {
                BindReqName();
                staffinfo();//========================================Define req spread cols
                staffinfo1();
                pageload();
                bindspread1();
                bindspread2();
                ViewState["hro"] = null;
                btn_criteria1.Visible = false;
                tbl_div.Visible = false;
                gatepassrights();
                Session["Priority"] = "";

                ddlcollegestaff_SelectedIndexChanged(sender, e);
            }
        }
        catch (Exception ex) { }
    }
    public void pageload()
    {
        BindReqName();
        BindCollege();
        bindstaffdept1();
        bind_stafType1();
        bindstaffdesg();

        BindCollege1();
        bindstaffdept2();
        bind_stafType();
        bindstaffdeg();
    }
    protected void ddlcollegestaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region written by some others
        gatepassrights();

        if (gatepass_staffdept == "1")
        {
            rdo_gatepass_staff.Visible = true;
            rdo_gatepass_dept.Visible = false;
        }
        else if (gatepass_staffdept == "2")
        {
            rdo_gatepass_staff.Visible = false;
            rdo_gatepass_dept.Visible = true;
            rdo_gatepass_dept.Checked = true;
            rdo_gatepass_staff.Checked = false;
            btnMainGogatepass.Visible = true;
            btnMainGo.Visible = false;
            bindspd_gatepass();
            bindspread_gatepass_dept();
            UpdatePanel1.Visible = false;
            lblstafftype_new.Visible = false;
            UpdatePanel2.Visible = false;
            lblstaff.Visible = false;
            btnMainGogatepass_Click(sender, e);

        }
        else
        {
            rdo_gatepass_staff.Visible = true;
            rdo_gatepass_dept.Visible = true;
        }
        if (ddl_reqname.Items.Count > 0)
        {
            if (ddl_reqname.SelectedItem.Value == "6")
            {

                div_gatepass.Visible = true;
                if (rdo_gatepass_staff.Checked == true)
                {
                    UpdatePanel1.Visible = true;
                    lblstafftype_new.Visible = true;
                    UpdatePanel2.Visible = true;
                    lblstaff.Visible = true;
                }
                else
                {
                    UpdatePanel1.Visible = false;
                    lblstafftype_new.Visible = false;
                    UpdatePanel2.Visible = false;
                    lblstaff.Visible = false;
                    btnMainGogatepass.Visible = true;
                    staffinfo();
                    bindspd_gatepass();
                    btnMainGogatepass_Click(sender, e);

                }

            }
        }
        else
        {
            div_gatepass.Visible = false;
            UpdatePanel1.Visible = true;
            lblstafftype_new.Visible = true;
            UpdatePanel2.Visible = true;
            lblstaff.Visible = true;
            btnMainGo.Visible = true;
            btnMainGogatepass.Visible = false;
            staffinfo();
            bindspread1();
        }
        bindstaffdept1();
        #endregion

        #region leave apply settings -- added by  Idhris 07-11-2016

        if (ddl_reqname.SelectedValue.Trim() == "10")
        {
            rblStaffStudent.Attributes.Add("Style", "display:block");
        }
        else
        {
            rblStaffStudent.Attributes.Add("Style", "display:none");
        }
        if (ddl_reqname.SelectedValue.Trim() == "10" && rblStaffStudent.SelectedIndex == 0)
        {
            divStaffDet.Visible = false;
            divStudSDet.Visible = true;
        }
        else
        {
            divStaffDet.Visible = true;
            divStudSDet.Visible = false;
        }
        #endregion
        if (ddl_reqname.SelectedValue.Trim() == "11")
        {
            rdo_gatepass_staff.Visible = true;
            rdo_gatepass_dept.Visible = false;
            div_gatepass.Visible = true;
        }

        bindstaffdept1();
        bind_stafType1();
        bindstaffdesg();
    }
    protected void chkdeptstaff_CheckedChanged(object sender, EventArgs e)
    {
        if (chkdeptstaff.Checked == true)
        {
            for (int i = 0; i < chldeptstaff.Items.Count; i++)
            {
                chldeptstaff.Items[i].Selected = true;
                txtstaffDepart.Text = "Dept (" + (chldeptstaff.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chldeptstaff.Items.Count; i++)
            {
                chldeptstaff.Items[i].Selected = false;
                txtstaffDepart.Text = "---Select---";
            }
        }
    }
    protected void chldeptstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        int batchcount = 0;
        chkdeptstaff.Checked = false;
        for (int i = 0; i < chldeptstaff.Items.Count; i++)
        {
            if (chldeptstaff.Items[i].Selected == true)
            {
                batchcount = batchcount + 1;
            }
        }
        if (batchcount > 0)
        {
            txtstaffDepart.Text = "Dept (" + batchcount.ToString() + ")";
            if (batchcount == chldeptstaff.Items.Count)
            {
                chkdeptstaff.Checked = true;
            }
        }
        else
        {
            txtstaffDepart.Text = "---Select---";
        }
    }
    protected void chkstafftypenew_CheckedChanged(object sender, EventArgs e)
    {
        if (chkstafftypenew.Checked == true)
        {
            for (int i = 0; i < chlstafftpyenew.Items.Count; i++)
            {
                chlstafftpyenew.Items[i].Selected = true;
                txtstaff_type.Text = "Type (" + (chlstafftpyenew.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chlstafftpyenew.Items.Count; i++)
            {
                chlstafftpyenew.Items[i].Selected = false;
                txtstaff_type.Text = "---Select---";
            }
        }
    }
    protected void chlstafftpyenew_SelectedIndexChanged(object sender, EventArgs e)
    {
        int batchcount = 0;
        chkstafftypenew.Checked = false;
        for (int i = 0; i < chlstafftpyenew.Items.Count; i++)
        {
            if (chlstafftpyenew.Items[i].Selected == true)
            {
                batchcount = batchcount + 1;

            }
        }
        if (batchcount > 0)
        {
            txtstaff_type.Text = "Type (" + batchcount.ToString() + ")";
            if (batchcount == chlstafftpyenew.Items.Count)
            {
                chkstafftypenew.Checked = true;
            }
        }
        else
        {
            txtstaff_type.Text = "---Select---";
        }
    }
    protected void chksatff_CheckedChanged(object sender, EventArgs e)
    {
        if (chksatff.Checked == true)
        {
            for (int i = 0; i < chklststaff.Items.Count; i++)
            {
                chklststaff.Items[i].Selected = true;
                txtstaff.Text = "Desig (" + (chklststaff.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklststaff.Items.Count; i++)
            {
                chklststaff.Items[i].Selected = false;
                txtstaff.Text = "---Select---";
            }
        }
    }
    protected void chklststaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        int batchcount = 0;
        chksatff.Checked = false;
        for (int i = 0; i < chklststaff.Items.Count; i++)
        {
            if (chklststaff.Items[i].Selected == true)
            {
                batchcount = batchcount + 1;
            }
        }
        if (batchcount > 0)
        {
            txtstaff.Text = "Desig (" + batchcount.ToString() + ")";
            if (batchcount == chklststaff.Items.Count)
            {
                chksatff.Checked = true;
            }
        }
        else
            txtstaff.Text = "---Select---";
    }
    protected void btnMainGo_Click(object sender, EventArgs e)
    {
        try //  try catch added by poo 09.12.17
        {

            string inwardval = Convert.ToString(ddl_reqname.SelectedItem.Value); //poo 09.12.17
            if (inwardval.Trim() == "12")
            {
                bindspd_gatepass();
                bindspread_gatepass_dept();
            }
            else
            {
                staffinfo();
                if (txtstaffDepart.Text != "---Select---" || chldeptstaff.Items.Count != null || chklststaff.Items.Count != null)
                {
                    int itemcount = 0;
                    for (itemcount = 0; itemcount < chklststaff.Items.Count; itemcount++)
                    {
                        if (chklststaff.Items[itemcount].Selected == true)
                        {
                            if (strstaff1 == "")
                                strstaff1 = "'" + chklststaff.Items[itemcount].Value.ToString() + "'";
                            else
                                strstaff1 = strstaff1 + "," + "'" + chklststaff.Items[itemcount].Value.ToString() + "'";
                        }
                    }
                    if (strstaff1 != "")
                    {
                        // strstaff1 = strstaff1;
                        strstaff1 = " in(" + strstaff1 + ")";
                        sqlstrstaff1 = "and d.desig_code  " + strstaff1 + "";

                    }
                    else
                        strstaff1 = "";
                }
                if (txtstaffDepart.Text != "---Select---" || chldeptstaff.Items.Count != null)
                {
                    int itemcount = 0;
                    for (itemcount = 0; itemcount < chldeptstaff.Items.Count; itemcount++)
                    {
                        if (chldeptstaff.Items[itemcount].Selected == true)
                        {
                            if (strstaffdept == "")
                                strstaffdept = "'" + chldeptstaff.Items[itemcount].Value.ToString() + "'";
                            else
                                strstaffdept = strstaffdept + "," + "'" + chldeptstaff.Items[itemcount].Value.ToString() + "'";
                        }
                    }
                    if (strstaffdept != "")
                    {
                        strstaffdept = " in(" + strstaffdept + ")";
                        sqlstrstaffdept1 = " and h.dept_code  " + strstaffdept + "";

                    }
                    else
                        strstaffdept = "";
                }
                if (txtstaff_type.Text != "---Select---" || chlstafftpyenew.Items.Count != null)
                {
                    int itemcount = 0;
                    for (itemcount = 0; itemcount < chlstafftpyenew.Items.Count; itemcount++)
                    {
                        if (chlstafftpyenew.Items[itemcount].Selected == true)
                        {
                            if (strstafftype == "")
                                strstafftype = "'" + chlstafftpyenew.Items[itemcount].Value.ToString() + "'";
                            else
                                strstafftype = strstafftype + "," + "'" + chlstafftpyenew.Items[itemcount].Value.ToString() + "'";
                        }
                    }
                    if (strstafftype != "")
                    {
                        strstafftype = " in(" + strstafftype + ")";
                        sqlstrstafftype = " and stftype  " + strstafftype + "";

                    }
                    else
                        strstafftype = "";
                }

                FpSpread2.Sheets[0].RowCount = 0;
                string staffcode1 = "";
                string sqlcmd1 = "";
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                sqlcmd1 = "select distinct s.staff_code,a.appl_id ,s.staff_name,h.dept_name,h.dept_code,d.desig_name from staff_appl_master a ,staffmaster s,hrdept_master h,desig_master d,stafftrans st where a.appl_no =s.appl_no and s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode and s.college_code = d.collegecode " + sqlstrstaff1 + " " + sqlstrstaffdept1 + " " + sqlstrstafftype + " and s.college_code='" + ddlcollegestaff.SelectedValue.ToString() + "' and resign = 0 and settled = 0 and latestrec=1 order by h.dept_name,s.staff_code";
                sqlcmd1 = sqlcmd1 + " select ReqStaffAppNo,RequestType from RQ_RequestHierarchy";
                dsload = dset.select_method_wo_parameter(sqlcmd1, "Text");
                int sno = 1;
                DataView dv = new DataView();
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    for (int loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                    {
                        ++FpSpread2.Sheets[0].RowCount;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Value = 0;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["dept_name"].ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag = dsload.Tables[0].Rows[loop]["dept_code"].ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["desig_name"].ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].CellType = txt;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = dsload.Tables[0].Rows[loop]["staff_code"].ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Tag = dsload.Tables[0].Rows[loop]["appl_id"].ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = dsload.Tables[0].Rows[loop]["staff_name"].ToString();
                        string a = FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text;
                        // staffcode1 = d2.GetFunction("select a.appl_id from staff_appl_master a,staffmaster s where a.appl_no =s.appl_no and staff_code ='" + a + "'");
                        staffcode1 = Convert.ToString(FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Tag);
                        string staff = "";
                        dsload.Tables[1].DefaultView.RowFilter = "ReqStaffAppNo='" + staffcode1 + "' and RequestType='" + ddl_reqname.SelectedItem.Value + "'";
                        dv = dsload.Tables[1].DefaultView;
                        //= d2.GetFunction("select ReqStaffAppNo from RQ_RequestHierarchy where ReqStaffAppNo ='" + staffcode1 + "'");

                        if (dv.Count > 0)
                        {
                            FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#F0A3CC");
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Value = 1;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Locked = true;
                            FpSpread2.Enabled = true;
                            btnreset.Enabled = true;
                            btnview.Enabled = true;
                        }

                        sno++;
                    }
                    FpSpread2.SaveChanges();
                    FpSpread2.Visible = true;
                    lblerrstaff.Visible = false;
                }
                else
                {
                    FpSpread2.Visible = false;
                    lblerrstaff.Visible = true;
                    lblerrstaff.Text = "No Record(s) Found";
                }

                FpSpread2.Sheets[0].PageSize = 12;
                FpSpread2.TitleInfo.Height = 30;
                //if (FpSpread1.Sheets[0].RowCount > 10)
                //{
                //    FpSpread2.Height = 390;
                //}
                //else
                //{
                //    FpSpread2.Height = (FpSpread2.Sheets[0].RowCount * 25) + 140;
                //}
                FpSpread2.Height = 390;

                // applyDetails();

            }
        }

        catch (Exception ex) // poo 09.12.17
        {
            d2.sendErrorMail(ex, ddlcollegestaff.SelectedValue.ToString(), "HierarchySettings");
        }
    }
    protected void FpSpread2_ButtonCommand(object sender, EventArgs e)
    {
        //string activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
        //string activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
        //FpSpread2.SaveChanges();

        //if (activerow != "")
        //{
        //    if (FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Value.ToString() == "1")
        //    {
        //        for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
        //        {
        //            if (i == Convert.ToInt32(activerow))
        //            {
        //                FpSpread2.Sheets[0].Cells[i, 1].Value = "1";
        //            }
        //            else
        //            {
        //                FpSpread2.Sheets[0].Cells[i, 1].Value = "0";
        //            }
        //        }
        //    }
        //}
    }
    protected void FpSpread2_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        flag_spread1 = true;
        // btnview.Enabled = true;
        //  btnreset.Enabled = true;
    }
    protected void FpSpread2_CellClick(object sender, EventArgs e)
    {
    }
    string get_staff_code = "";
    string staff_code_DB = "";
    string HY_Order_DB = "";
    string get_dept_code = "";
    protected void FpSpread2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (flag_spread1 == true)
        {
            FpSpread2.SaveChanges();
            for (int i = 0; i <= Convert.ToInt16(FpSpread2.Sheets[0].RowCount) - 1; i++)
            {
                int isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[i, 1].Value);
                if (ddl_reqname.SelectedItem.Value == "6" && rdo_gatepass_dept.Checked == true)
                {
                    get_dept_code = Convert.ToString(FpSpread2.Sheets[0].GetTag(i, 2));
                }
                else if (ddl_reqname.SelectedItem.Value == "12") // poo 09.12.17
                {
                    get_dept_code = Convert.ToString(FpSpread2.Sheets[0].GetTag(i, 2));
                }
                else if (rdbreqstu.Checked == true)
                {

                }
                else if (ddl_reqname.SelectedItem.Value == "14")
                {

                }
                else
                {
                    //if (string.IsNullOrEmpty(get_dept_code))
                    get_staff_code = FpSpread2.Sheets[0].GetText(i, 4);
                    if (isval == 1)
                    {

                        string sqlquery = "";
                        sqlquery = "select * from ApprovalStaff where Apply_StaffCode = '" + get_staff_code + "'";
                        ds = d2.select_method(sqlquery, hat, "Text");
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            for (int i1 = 0; i1 < ds.Tables[0].Rows.Count; i1++)
                            {
                                staff_code_DB = ds.Tables[0].Rows[i1]["Approval_StaffCode"].ToString();
                                HY_Order_DB = ds.Tables[0].Rows[i1]["HerarchyOrder"].ToString();
                                //for (int sp2 = 0; sp2 < Convert.ToInt16(FpSpread1.Sheets[0].RowCount) - 1; sp2++)
                                //{
                                //    string staff_fp2_code = "";
                                //    staff_fp2_code = FpSpread1.Sheets[0].Cells[sp2, 3].Text.ToString();
                                //    if (staff_code_DB == staff_fp2_code)
                                //    {
                                //        FpSpread1.Sheets[0].Cells[sp2, 5].Value = 1;
                                //        FpSpread1.Sheets[0].Cells[sp2, 5].Locked = true;
                                //        FpSpread1.Sheets[0].Cells[sp2, 6].Text = HY_Order_DB;
                                //        FpSpread1.Sheets[0].Cells[sp2, 6].Locked = true;
                                //        Buttonsave.Text = "Update";
                                //        btnreset.Text = "Delete";
                                //    }
                                //}
                            }
                        }
                    }
                    else
                    {

                    }
                }
            }

        }
        else
        {

        }
    }
    protected void Btn_range_Click(object sender, EventArgs e)
    {
        if (txt_frange.Text == "" || txt_trange.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Enter Both From And To Range.')", true);
            return;
        }

        if (Convert.ToInt32(txt_frange.Text) > Convert.ToInt32(txt_trange.Text))
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('To Range Should Be Greater Than Or Equal To From Range.')", true);
            return;
        }

        for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
        {
            string sl_no = FpSpread2.Sheets[0].Cells[i, 0].Text.ToString();

            if (sl_no != "")
            {
                if (Convert.ToInt32(sl_no) >= Convert.ToInt32(txt_frange.Text) && Convert.ToInt32(sl_no) <= Convert.ToInt32(txt_trange.Text))
                {
                    FpSpread2.Sheets[0].Cells[i, 1].Value = "1";
                    FpSpread2.Sheets[0].Cells[i, 1].Locked = false; // poo

                }
                else
                {
                    FpSpread2.Sheets[0].Cells[i, 1].Value = "0";
                }
            }
        }

        txt_frange.Text = "";
        txt_trange.Text = "";
    }
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {

        bindstaffdept2();
        bind_stafType();
        bindstaffdeg();

    }
    protected void chksatffDept_CheckedChanged(object sender, EventArgs e)
    {
        if (chksatffDept.Checked == true)
        {
            for (int i = 0; i < chklststaffDept.Items.Count; i++)
            {
                chklststaffDept.Items[i].Selected = true;
                txtstaffDept.Text = "Dept (" + (chklststaffDept.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklststaffDept.Items.Count; i++)
            {
                chklststaffDept.Items[i].Selected = false;
                txtstaffDept.Text = "---Select---";
            }
        }
    }
    protected void chklststaffDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        int batchcount = 0;
        chksatffDept.Checked = false;
        for (int i = 0; i < chklststaffDept.Items.Count; i++)
        {
            if (chklststaffDept.Items[i].Selected == true)
            {
                batchcount = batchcount + 1;
            }
        }
        if (batchcount > 0)
        {
            txtstaffDept.Text = "Dept (" + batchcount + ")";
            if (batchcount == chklststaffDept.Items.Count)
            {
                chksatffDept.Checked = true;
            }
        }
        else
        {
            txtstaffDept.Text = "---Select---";
        }
    }
    protected void chksatffType_CheckedChanged(object sender, EventArgs e)
    {
        if (chksatffType.Checked == true)
        {
            for (int i = 0; i < chklststaffType.Items.Count; i++)
            {
                chklststaffType.Items[i].Selected = true;
                txtstaffType.Text = "Type (" + (chklststaffType.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklststaffType.Items.Count; i++)
            {
                chklststaffType.Items[i].Selected = false;
                txtstaffType.Text = "---Select---";
            }
        }
    }
    protected void chklststaffType_SelectedIndexChanged(object sender, EventArgs e)
    {
        int batchcount = 0;
        chksatffType.Checked = false;
        for (int i = 0; i < chklststaffType.Items.Count; i++)
        {
            if (chklststaffType.Items[i].Selected == true)
            {
                batchcount = batchcount + 1;
            }
        }
        if (batchcount > 0)
        {
            txtstaffType.Text = "Type (" + batchcount.ToString() + ")";
            if (batchcount == chklststaffType.Items.Count)
            {
                chksatffType.Checked = true;
            }
        }
        else
        {
            txtstaffType.Text = "---Select---";
        }
    }
    public void chkstaffdeg_CheckedChanged(object sender, EventArgs e)
    {


        if (chkstaffdeg.Checked == true)
        {
            for (int i = 0; i < chlstaffdeg.Items.Count; i++)
            {
                chlstaffdeg.Items[i].Selected = true;
            }
            txtstaffDeg.Text = "Desig(" + (chlstaffdeg.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chlstaffdeg.Items.Count; i++)
            {
                chlstaffdeg.Items[i].Selected = false;
            }
            txtstaffDeg.Text = "--Select--";
        }
    }
    protected void chlstaffdeg_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtstaffDeg.Text = "--Select--";
        chkstaffdeg.Checked = false;
        int commcount = 0;
        for (int i = 0; i < chlstaffdeg.Items.Count; i++)
        {
            if (chlstaffdeg.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtstaffDeg.Text = "Desig(" + commcount.ToString() + ")";
            if (commcount == chlstaffdeg.Items.Count)
            {
                chkstaffdeg.Checked = true;
            }
        }
    }

    protected void btnMainGo1_Click(object sender, EventArgs e)
    {
        staffinfo1();
        //first spread..............
        ViewState["hro"] = null;

        //added by Idhris 21-11-2016
        resetStages();

        if (txtstaffDept.Text != "---Select---" || chklststaffDept.Items.Count != null || chlstaffdeg.Items.Count != null)
        {
            int itemcount = 0;
            for (itemcount = 0; itemcount < chlstaffdeg.Items.Count; itemcount++)
            {
                if (chlstaffdeg.Items[itemcount].Selected == true)
                {
                    if (strstaff1 == "")
                        strstaff1 = "'" + chlstaffdeg.Items[itemcount].Value.ToString() + "'";
                    else
                        strstaff1 = strstaff1 + "," + "'" + chlstaffdeg.Items[itemcount].Value.ToString() + "'";
                }
            }
            if (strstaff1 != "")
            {
                // strstaff1 = strstaff1;
                strstaff1 = " in(" + strstaff1 + ")";
                sqlstrstaff1 = "and d.desig_code  " + strstaff1 + "";

            }
            else
            {
                strstaff1 = "";
            }
        }
        if (txtstaffDept.Text != "---Select---" || chklststaffDept.Items.Count != null)
        {
            int itemcount = 0;


            for (itemcount = 0; itemcount < chklststaffDept.Items.Count; itemcount++)
            {
                if (chklststaffDept.Items[itemcount].Selected == true)
                {
                    if (strstaffdept == "")
                        strstaffdept = "'" + chklststaffDept.Items[itemcount].Value.ToString() + "'";
                    else
                        strstaffdept = strstaffdept + "," + "'" + chklststaffDept.Items[itemcount].Value.ToString() + "'";
                }
            }
            if (strstaffdept != "")
            {
                strstaffdept = " in(" + strstaffdept + ")";
                sqlstrstaffdept1 = " and h.dept_code  " + strstaffdept + "";

            }
            else
            {
                strstaffdept = "";
            }
        }
        if (txtstaff_type.Text != "---Select---" || chklststaffType.Items.Count != null)
        {
            int itemcount = 0;


            for (itemcount = 0; itemcount < chklststaffType.Items.Count; itemcount++)
            {
                if (chklststaffType.Items[itemcount].Selected == true)
                {
                    if (strstafftype == "")
                        strstafftype = "'" + chklststaffType.Items[itemcount].Value.ToString() + "'";
                    else
                        strstafftype = strstafftype + "," + "'" + chklststaffType.Items[itemcount].Value.ToString() + "'";
                }
            }
            if (strstafftype != "")
            {
                strstafftype = " in(" + strstafftype + ")";
                sqlstrstafftype = " and stftype  " + strstafftype + "";

            }
            else
            {
                strstafftype = "";
            }
        }

        FpSpread1.Sheets[0].RowCount = 0;
        string sqlcmd = "";
        string staffcode1 = "";
        DataView dv = new DataView();
        DataSet dsload1 = new DataSet();
        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
        if (!ddlStaffType.Visible)
        {

            sqlcmd = "select distinct s.staff_code,s.staff_name,h.dept_name,h.dept_code,d.desig_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode " + sqlstrstaff1 + " " + sqlstrstaffdept1 + " " + sqlstrstafftype + " and s.college_code='" + ddlcollege.SelectedValue.ToString() + "' and resign = 0 and settled = 0 and latestrec=1 order by h.dept_name,s.staff_code";
            sqlcmd = sqlcmd + " select ReqStaffAppNo,ReqAppStaffAppNo,RequestType from RQ_RequestHierarchy";
            dsload = dset.select_method_wo_parameter(sqlcmd, "Text");
        }
        else
        {
            if (ddlStaffType.SelectedItem.Text == "General")
            {
                sqlcmd = "select distinct s.staff_code,s.staff_name,h.dept_name,h.dept_code,d.desig_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode " + sqlstrstaff1 + " " + sqlstrstaffdept1 + " " + sqlstrstafftype + " and s.college_code='" + ddlcollege.SelectedValue.ToString() + "' and resign = 0 and settled = 0 and latestrec=1 order by h.dept_name,s.staff_code";
                sqlcmd = sqlcmd + " select ReqStaffAppNo,ReqAppStaffAppNo,RequestType from RQ_RequestHierarchy";
                dsload = dset.select_method_wo_parameter(sqlcmd, "Text");
            }
            else
            {
                fpreport.SaveChanges();
                DataSet tempDS = new DataSet();

                for (int i = 0; i < fpreport.Sheets[0].Rows.Count; i++)
                {
                    if (Convert.ToString(fpreport.Sheets[0].Cells[i, 6].Value) == "1")
                    {
                        tempDS.Clear();
                        string tempstr = string.Empty;
                        string ColCode = Convert.ToString(fpreport.Sheets[0].Cells[i, 1].Tag);
                        string batchY = Convert.ToString(fpreport.Sheets[0].Cells[i, 2].Tag);
                        string deg = Convert.ToString(fpreport.Sheets[0].Cells[i, 3].Tag);
                        string sems = Convert.ToString(fpreport.Sheets[0].Cells[i, 4].Tag);
                        string sec = Convert.ToString(fpreport.Sheets[0].Cells[i, 5].Tag);
                        string secdet = string.Empty;
                        if (!string.IsNullOrEmpty(sec))
                            secdet = " and ss.Sections in('" + sec + "')";

                        if (ddlStaffType.SelectedItem.Text == "Staff Selector")
                        {
                            tempstr = "select distinct s.staff_code,s.staff_name,h.dept_name,h.dept_code,d.desig_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st,staff_selector ss,syllabus_master sm,subject su where ss.staff_code=s.staff_code and s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode  and s.college_code='" + ColCode + "' and resign = 0 and settled = 0 and latestrec=1 and su.syll_code=sm.syll_code and su.subject_no=ss.subject_no  and sm.Batch_Year in('" + batchY + "') and sm.degree_code in('" + deg + "') and sm.semester in('" + sems + "') " + secdet + "  order by h.dept_name,s.staff_code";
                            tempstr = tempstr + " select ReqStaffAppNo,ReqAppStaffAppNo,RequestType from RQ_RequestHierarchy";
                        }
                        else if (ddlStaffType.SelectedItem.Text == "Include Staff Selector")
                        {
                            tempstr = "select distinct s.staff_code,s.staff_name,h.dept_name,h.dept_code,d.desig_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode " + sqlstrstaff1 + " " + sqlstrstaffdept1 + " " + sqlstrstafftype + " and s.college_code='" + ddlcollege.SelectedValue.ToString() + "' and resign = 0 and settled = 0 and latestrec=1  and s.staff_code not in( select distinct ss.staff_code from staff_selector ss,syllabus_master sm,subject su where su.syll_code=sm.syll_code and su.subject_no=ss.subject_no  and sm.Batch_Year in('" + batchY + "') and sm.degree_code in('" + deg + "') and sm.semester in('" + sems + "')  " + secdet + ")  union select distinct s.staff_code,s.staff_name,h.dept_name,h.dept_code,d.desig_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st,staff_selector ss,syllabus_master sm,subject su where ss.staff_code=s.staff_code and s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode  and s.college_code='" + ColCode + "' and resign = 0 and settled = 0 and latestrec=1 and su.syll_code=sm.syll_code and su.subject_no=ss.subject_no  and sm.Batch_Year in('" + batchY + "') and sm.degree_code in('" + deg + "') and sm.semester in('" + sems + "') " + secdet + " order by h.dept_name,s.staff_code ";
                            tempstr = tempstr + " select ReqStaffAppNo,ReqAppStaffAppNo,RequestType from RQ_RequestHierarchy";
                        }
                        else
                        {
                            tempstr = "select distinct s.staff_code,s.staff_name,h.dept_name,h.dept_code,d.desig_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode " + sqlstrstaff1 + " " + sqlstrstaffdept1 + " " + sqlstrstafftype + " and s.college_code='" + ddlcollege.SelectedValue.ToString() + "' and resign = 0 and settled = 0 and latestrec=1 order by h.dept_name,s.staff_code";
                            tempstr = tempstr + " select ReqStaffAppNo,ReqAppStaffAppNo,RequestType from RQ_RequestHierarchy";
                        }
                        tempDS = d2.select_method_wo_parameter(tempstr, "text");
                        if (tempDS.Tables.Count > 0 && tempDS.Tables[0].Rows.Count > 0 && dsload.Tables.Count > 0)
                            dsload.Tables[0].Merge(tempDS.Tables[0]);
                        else
                            dsload = d2.select_method_wo_parameter(tempstr, "text");
                    }
                }
            }
        }


        int sno = 1;

        if (dsload.Tables[0].Rows.Count > 0)
        {
            Hashtable hat = new Hashtable();
            for (int loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
            {
                string staffCode = dsload.Tables[0].Rows[loop]["staff_code"].ToString();
                if (!hat.ContainsKey(staffCode))
                {
                    hat.Add(staffCode, staffCode);
                    ++FpSpread1.Sheets[0].RowCount;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dsload.Tables[0].Rows[loop]["dept_name"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["desig_name"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = dsload.Tables[0].Rows[loop]["dept_code"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["staff_code"].ToString();

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = dsload.Tables[0].Rows[loop]["staff_name"].ToString();
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Value = 0;
                    staffcode1 = Convert.ToString(FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text);
                    string code1 = d2.GetFunction("select appl_id from staff_appl_master where appl_no='" + staffcode1 + "'");
                    dsload.Tables[1].DefaultView.RowFilter = "ReqAppStaffAppNo='" + code1 + "' and RequestType='" + ddl_reqname.SelectedItem.Value + "'";
                    dv = dsload.Tables[1].DefaultView;
                    sno++;
                }
            }
            FpSpread1.SaveChanges();
            FpSpread1.Visible = true;
            Label1.Visible = false;
        }
        else
        {
            FpSpread1.Visible = false;
            Label1.Visible = true;
            Label1.Text = "No Record(s) Found";
        }

        FpSpread1.Sheets[0].PageSize = 12;
        FpSpread1.TitleInfo.Height = 30;
        if (FpSpread1.Sheets[0].RowCount > 10)
        {
            FpSpread1.Height = 390;
        }
        else
        {
            FpSpread1.Height = (FpSpread1.Sheets[0].RowCount * 25) + 140;
        }

    }


    protected void FpSpread1_CellClick(object sender, EventArgs e)
    {

    }
    protected void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (flag_true == true)
        {
            FpSpread1.SaveChanges();
            string activrow = "";
            string con = "";
            activrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string activecol = FpSpread1.Sheets[0].ActiveColumn.ToString();

            int actcol = Convert.ToInt16(activecol);
            int hy_order = 0;
            char c1 = 'A';
            if (txt_criteria.Text != "")
            {
                if (Session["Priority"] != null && Session["Priority"] != "")
                {
                    string hy_order1 = Session["Priority"].ToString();
                    if (ViewState["checkvalue"] == null && ViewState["checkvalue"] != "")
                    {
                        ViewState["checkvalue"] = Convert.ToString(hy_order1);
                    }
                    else
                    {
                        string getvalue = Convert.ToString(ViewState["checkvalue"]);
                        if (getvalue != hy_order1)
                        {
                            c1 = 'A';
                            hy_order = 0;
                            ViewState["checkvalue"] = Convert.ToString(hy_order1);
                        }
                        else
                        {
                            hy_order = Convert.ToInt32(ViewState["hro"]);
                        }

                    }
                    if (FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), actcol + 1].BackColor != Color.Coral)
                    {
                        //for (int i = 0; i <= Convert.ToInt16(FpSpread1.Sheets[0].RowCount) - 1; i++)
                        //{
                        //    if (FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), actcol].Locked != true)
                        //    {
                        //        int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, actcol].Value);

                        //        if (isval == 1)
                        //        {

                        hy_order++;
                        if (hy_order == 1)
                        {
                            c1 = 'A';
                        }
                        else
                        {
                            for (int ro = 1; ro < hy_order; ro++)
                            {
                                c1++;
                            }
                        }

                        con = hy_order1 + "-" + c1.ToString();
                        ViewState["hro"] = Convert.ToInt32(hy_order);
                        FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), actcol].Locked = true;

                        //        }
                        //    }
                        //}

                    }
                    FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), actcol + 1].Text = con;
                    string val = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), actcol + 1].Text;
                    if (val != "")
                    {
                        FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), actcol + 1].BackColor = Color.Coral;
                    }
                    string[] ay = con.Split('-');

                    value_con = ay[0];
                    hy_order_val = hy_order.ToString();
                }
                else
                {
                    imgdivalt.Visible = true;
                    panel_erroralert.Visible = true;
                    lbl_erroralert.Text = "Kindly Select The Stages";
                    FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), actcol].Value = 0;
                }
            }
            else
            {
                imgdivalt.Visible = true;
                panel_erroralert.Visible = true;
                lbl_erroralert.Text = "Kindly Fill The Criteria Value";
                FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), actcol].Value = 0;
            }
            for (int reset = 0; reset < Convert.ToInt16(FpSpread1.Sheets[0].RowCount); reset++)
            {
                string val = Convert.ToString(FpSpread1.Sheets[0].Cells[reset, 6].Text);
                string[] split = val.Split('-');
                string stage = split[0];
                if (btn_criteria1.Enabled == true)
                {
                    if (stage == "1")
                    {
                        btn_criteria2.Enabled = true;
                    }
                }
                else if (btn_criteria2.Enabled == true)
                {
                    if (stage == "2")
                    {
                        btn_criteria3.Enabled = true;
                    }
                }
                else if (btn_criteria3.Enabled == true)
                {
                    if (stage == "3")
                    {
                        btn_criteria4.Enabled = true;
                    }
                }
                else if (btn_criteria4.Enabled == true)
                {
                    if (stage == "4")
                    {
                        btn_criteria5.Enabled = true;
                    }
                }
                else if (btn_criteria5.Enabled == true)
                {
                    if (stage == "5")
                    {
                        btn_criteria6.Enabled = true;
                    }
                }
                else if (btn_criteria6.Enabled == true)
                {
                    if (stage == "6")
                    {
                        btn_criteria7.Enabled = true;
                    }
                }
                else if (btn_criteria7.Enabled == true)
                {
                    if (stage == "7")
                    {
                        btn_criteria8.Enabled = true;
                    }
                }
                else if (btn_criteria8.Enabled == true)
                {
                    if (stage == "8")
                    {
                        btn_criteria9.Enabled = true;
                    }
                }
                if (Chktutor.Checked == true)
                {
                    if (stage != "")
                    {
                        int st = 0;
                        int but = 0;
                        int stag = 0;
                        int stgcun = 0;
                        int.TryParse(stage, out stgcun);
                        stag = stgcun + 1;
                        int.TryParse(Txttutor.Text, out but);
                        st = but + 1;
                        if (stag == but)
                        {
                            if (st == 1)
                                btn_criteria1.Enabled = true;
                            if (st == 2)
                                btn_criteria2.Enabled = true;
                            if (st == 3)
                                btn_criteria3.Enabled = true;
                            if (st == 4)
                                btn_criteria4.Enabled = true;
                            if (st == 5)
                                btn_criteria5.Enabled = true;
                            if (st == 6)
                                btn_criteria6.Enabled = true;
                            if (st == 7)
                                btn_criteria7.Enabled = true;
                            if (st == 8)
                                btn_criteria8.Enabled = true;
                            if (st == 9)
                                btn_criteria9.Enabled = true;

                        }
                    }
                }

            }
        }

    }
    protected void FpSpread1_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        //string act;
        //act = e.SheetView.ActiveRow.ToString();
        //int act1 = Convert.ToInt32(act);

        //    if (FpSpread1.Sheets[0].Cells[act1, 5].Value.ToString() == "1")
        //    {
        //        flag_true = false;
        //        FpSpread1.Sheets[0].Cells[act1, 6].Text = "";
        //    }
        //    else
        //    {
        //        flag_true = true;
        //    }


    }
    protected void FpSpread1_ButtonCommand(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges();
        string activerow = FpSpread1.Sheets[0].ActiveRow.ToString();  //FpSpread2.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpread1.Sheets[0].ActiveColumn.ToString();  //FpSpread2.ActiveSheetView.ActiveColumn.ToString();
        string act;
        //act = e.SheetView.ActiveRow.ToString();
        int act1 = Convert.ToInt32(activerow);
        int act2 = Convert.ToInt16(activecol);

        if (FpSpread1.Sheets[0].Cells[act1, act2].Value.ToString() == "1")
        {
            flag_true = true;
            FpSpread1.Sheets[0].Cells[act1, act2 + 1].Text = "";
        }
        else
        {
            flag_true = false;
        }
        FpSpread1.SaveChanges();
        //FpSpread1.SaveChanges();
        //string activerow = FpSpread1.Sheets[0].ActiveRow.ToString();  
        //string activecol = FpSpread1.Sheets[0].ActiveColumn.ToString(); 
        //string act;

        //int act1 = Convert.ToInt32(activerow);
        //for (int i = 5; i < FpSpread1.Sheets[0].ColumnCount; i=i+2)
        //{
        //    if (FpSpread1.Sheets[0].Cells[act1, i].Value.ToString() == "1")
        //    {
        //        flag_true = true;
        //        FpSpread1.Sheets[0].Cells[act1, i+1].Text = "";
        //    }
        //    else
        //    {
        //        flag_true = false;
        //    }
        //}
        //FpSpread1.SaveChanges();
    }

    protected void btnreset_Click(object sender, EventArgs e)
    {
        if (ddl_reqname.SelectedValue == "10" && rblStaffStudent.SelectedIndex == 0)
        {
            resetStud();
        }
        else
        {
            resetStaff();
            if (rdbreqstu.Checked == true)
                btngostu_Click(sender, e);
        }
    }
    private void resetStaff()
    {
        string checkappno = "";
        int vv = 0;
        if (ddl_reqname.SelectedItem.Value == "14")
        {
            for (int reset1 = 0; reset1 < Convert.ToInt16(FpSpread2.Sheets[0].RowCount); reset1++)
            {
                string appstaff_app = Convert.ToString(FpSpread2.Sheets[0].Cells[reset1, 2].Tag);

                FpSpread2.Sheets[0].Cells[reset1, 1].Locked = false;


                if (ddl_reqname.SelectedItem.Value.Trim() == "6")
                {
                    checkappno = d2.GetFunction("select ReqAppNo from RQ_Requisition r,RQ_RequestHierarchy rh where r.RequestType=rh.RequestType and rh.RequestType='" + ddl_reqname.SelectedItem.Value + "' and degreecode='" + appstaff_app + "' and ReqAppStatus='0'");
                }


                if (checkappno != "0")
                {
                    string del_query = "delete from RQ_RequestHierarchy where degreecode='" + appstaff_app + "' and RequestType='" + ddl_reqname.SelectedItem.Value + "'";

                    d2.update_method_wo_parameter(del_query, "Text");
                    btnreset.Enabled = false;
                    btnview.Enabled = false;
                }
                else
                {
                    imgdivalt.Visible = true;
                    panel_erroralert.Visible = true;
                    lbl_erroralert.Text = "You Cannot Delete This Staff";

                }
            }

        }

        else  if (ddl_reqname.SelectedItem.Value == "6" || ddl_reqname.SelectedItem.Value == "12") // ddl_reqname.SelectedItem.Value == "12" added by poo 09.12.17
        {
            if (rdo_gatepass_dept.Checked == false)
            {
                if (rdbreqstu.Checked == false)
                {

                    for (int reset1 = 0; reset1 < Convert.ToInt16(FpSpread2.Sheets[0].RowCount); reset1++)
                    {


                        string appstaff_app = Convert.ToString(FpSpread2.Sheets[0].Cells[reset1, 4].Text);

                        FpSpread2.Sheets[0].Cells[reset1, 1].Locked = false;
                        string stf_codee = d2.GetFunction("select a.appl_id from staff_appl_master a,staffmaster s where a.appl_no =s.appl_no and staff_code ='" + appstaff_app + "'");

                        if (ddl_reqname.SelectedItem.Value.Trim() == "6")
                        {
                            checkappno = d2.GetFunction("select ReqAppNo from RQ_Requisition r,RQ_RequestHierarchy rh where r.RequestType=rh.RequestType and rh.RequestType='" + ddl_reqname.SelectedItem.Value + "' and ReqStaffAppNo='" + stf_codee + "' and ReqAppStatus='0'");
                        }
                        else
                        {
                            checkappno = d2.GetFunction("select ReqAppNo from RQ_Requisition r,RQ_RequestHierarchy rh where r.RequestType=rh.RequestType and rh.RequestType='" + ddl_reqname.SelectedItem.Value + "' and ReqAppNo='" + stf_codee + "' and ReqAppStatus='0'");
                        }

                        if (checkappno == "0")
                        {
                            string del_query = "delete from RQ_RequestHierarchy where ReqStaffAppNo='" + stf_codee + "' and RequestType='" + ddl_reqname.SelectedItem.Value + "'";
                            //string del_query = "update RQ_RequestHierarchy set ReqApproveStage='',ReqAppPriority='' where RequestType='" + ddl_reqname.SelectedItem.Value + "' and ReqStaffAppNo='" + stf_codee + "' ";
                            d2.update_method_wo_parameter(del_query, "Text");
                            btnreset.Enabled = false;
                            btnview.Enabled = false;
                        }
                        else
                        {
                            imgdivalt.Visible = true;
                            panel_erroralert.Visible = true;
                            lbl_erroralert.Text = "You Cannot Delete This Staff";
                            return;
                        }
                    }
                    bindspread1();
                    bindspread2();
                }
                else
                {
                    for (int reset1 = 0; reset1 < Convert.ToInt16(FpSpread2.Sheets[0].RowCount); reset1++)
                    {


                        string appstaff_app = Convert.ToString(FpSpread2.Sheets[0].Cells[reset1, 2].Tag);

                        FpSpread2.Sheets[0].Cells[reset1, 1].Locked = false;


                        if (ddl_reqname.SelectedItem.Value.Trim() == "6")
                        {
                            checkappno = d2.GetFunction("select ReqAppNo from RQ_Requisition r,RQ_RequestHierarchy rh where r.RequestType=rh.RequestType and rh.RequestType='" + ddl_reqname.SelectedItem.Value + "' and degreecode='" + appstaff_app + "' and ReqAppStatus='0'");
                        }


                        if (checkappno != "0")
                        {
                            string del_query = "delete from RQ_RequestHierarchy where degreecode='" + appstaff_app + "' and RequestType='" + ddl_reqname.SelectedItem.Value + "'";
                            //string del_query = "update RQ_RequestHierarchy set ReqApproveStage='',ReqAppPriority='' where RequestType='" + ddl_reqname.SelectedItem.Value + "' and ReqStaffAppNo='" + stf_codee + "' ";
                            d2.update_method_wo_parameter(del_query, "Text");
                            btnreset.Enabled = false;
                            btnview.Enabled = false;
                        }
                        else
                        {
                            imgdivalt.Visible = true;
                            panel_erroralert.Visible = true;
                            lbl_erroralert.Text = "You Cannot Delete This Staff";

                        }
                    }

                }

            }
            else
            {
                for (int reset1 = 0; reset1 < Convert.ToInt16(FpSpread2.Sheets[0].RowCount); reset1++)
                {
                    string appstaff_app = Convert.ToString(Convert.ToString(FpSpread2.Sheets[0].Cells[reset1, 2].Tag));
                    FpSpread2.Sheets[0].Cells[reset1, 1].Locked = false;

                    checkappno = d2.GetFunction("select ReqAppNo from RQ_Requisition r,RQ_RequestHierarchy rh where r.RequestType=rh.RequestType and rh.RequestType='" + ddl_reqname.SelectedItem.Value + "' and ReqStaffAppNo='" + appstaff_app + "' and ReqAppStatus='0'");
                    if (checkappno == "0")
                    {
                        string del_query = "delete from RQ_RequestHierarchy where ReqStaffAppNo='" + appstaff_app + "' and RequestType='" + ddl_reqname.SelectedItem.Value + "'";
                        d2.update_method_wo_parameter(del_query, "Text");
                        btnreset.Enabled = false;
                        btnview.Enabled = false;
                    }
                    else
                    {
                        imgdivalt.Visible = true;
                        panel_erroralert.Visible = true;
                        lbl_erroralert.Text = "You Cannot Delete This Staff";
                        return;
                    }
                }
                bindspread_gatepass_dept();
                bindspread2();

            }
        }
        else
        {
            for (int reset1 = 0; reset1 < Convert.ToInt16(FpSpread2.Sheets[0].RowCount); reset1++)
            {

                string appstaff_app = Convert.ToString(Convert.ToString(FpSpread2.Sheets[0].Cells[reset1, 4].Tag));
                FpSpread2.Sheets[0].Cells[reset1, 1].Locked = false;
                string stf_codee = d2.GetFunction("select a.appl_id from staff_appl_master a,staffmaster s where a.appl_no =s.appl_no and staff_code ='" + appstaff_app + "'");
                if (ddl_reqname.SelectedItem.Value.Trim() == "6")
                {
                    checkappno = d2.GetFunction("select ReqAppNo from RQ_Requisition r,RQ_RequestHierarchy rh where r.RequestType=rh.RequestType and rh.RequestType='" + ddl_reqname.SelectedItem.Value + "' and ReqStaffAppNo='" + stf_codee + "' and ReqAppStatus='0'");
                }
                else
                {
                    checkappno = d2.GetFunction("select ReqAppNo from RQ_Requisition r,RQ_RequestHierarchy rh where r.RequestType=rh.RequestType and rh.RequestType='" + ddl_reqname.SelectedItem.Value + "' and ReqAppNo='" + stf_codee + "' ");//and ReqAppStatus='0'
                }
                if (checkappno == "0" || checkappno != "0")
                {
                    // clearall();
                    string del_query = "delete from RQ_RequestHierarchy where ReqStaffAppNo='" + appstaff_app + "' and RequestType='" + ddl_reqname.SelectedItem.Value + "'";
                    vv = d2.update_method_wo_parameter(del_query, "Text");
                    btnreset.Enabled = false;
                    btnview.Enabled = false;
                }
                else
                {
                    imgdivalt.Visible = true;
                    panel_erroralert.Visible = true;
                    lbl_erroralert.Text = "You Cannot Delete This Staff";
                    return;
                }
            }
        }


        imgdivalt.Visible = true;
        panel_erroralert.Visible = true;
        lbl_erroralert.Text = "Deleted Successfully";
        txt_criteria.Text = "";
        btn_criteria1.Visible = false;
        btn_criteria2.Visible = false;
        btn_criteria3.Visible = false;
        btn_criteria4.Visible = false;
        btn_criteria5.Visible = false;
        btn_criteria6.Visible = false;
        btn_criteria7.Visible = false;
        btn_criteria8.Visible = false;
        btn_criteria9.Visible = false;
        Session["Priority"] = "";
        //#F0F0F0
        btn_criteria1.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria1.ForeColor = Color.Black;
        btn_criteria2.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria2.ForeColor = Color.Black;
        btn_criteria3.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria3.ForeColor = Color.Black;
        btn_criteria4.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria4.ForeColor = Color.Black;
        btn_criteria5.ForeColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria5.ForeColor = Color.Black;
        btn_criteria6.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria6.ForeColor = Color.Black;
        btn_criteria7.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria7.ForeColor = Color.Black;
        btn_criteria8.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria8.ForeColor = Color.Black;
        btn_criteria9.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria9.ForeColor = Color.Black;

        btn_criteria1.Enabled = true;
        btn_criteria2.Enabled = false;
        btn_criteria3.Enabled = false;
        btn_criteria4.Enabled = false;
        btn_criteria5.Enabled = false;
        btn_criteria6.Enabled = false;
        btn_criteria7.Enabled = false;
        btn_criteria8.Enabled = false;
        btn_criteria9.Enabled = false;
    }
    string staff_code_fp1 = "";
    string staff_code_fp2 = "";
    string staff_Name_Fp2 = "";
    string staff_Deg_fp2 = "";
    string staff_dep_fp2 = "";
    string fp2_herarchy_order = "";
    string staff_dept_code = "";

    protected void Buttonsave_Click(object sender, EventArgs e)
    {
        if (ddl_reqname.SelectedValue.Trim() == "10" && rblStaffStudent.SelectedIndex == 0)
        {
            saveForStudent();
        }
        else
        {
            saveForStaff();
        }
    }

    private void saveForStaff()
    {
        FpSpread1.SaveChanges();
        FpSpread2.SaveChanges();
        try
        {
            Int64 RequestType = Convert.ToInt64(ddl_reqname.SelectedItem.Value);
            int CollegeCode = Convert.ToInt16(ddlcollege.SelectedItem.Value);
            Int64 ReqStaffAppNo = 0;
            Int64 ReqAppStaffAppNo = 0;
            string type = "";
            int q = 0;
            int reqstaff = 0;
            int appstaff = 0;
            string cri = Convert.ToString(txt_criteria.Text);
            string activerow = "";
            activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
            string staffReqDeptcode = string.Empty;
            string staffAppDeptcode = string.Empty;
            string valS = FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
            string typ1 = string.Empty;
            if (Cbllbatch.Items.Count > 0)
            {
                for (int i = 0; i < Cbllbatch.Items.Count; i++)
                {
                    if (Cbllbatch.Items[i].Selected == true)
                    {
                        if (typ1 == "")
                        {
                            typ1 = "" + Cbllbatch.Items[i].Value + "";
                        }
                        else
                        {
                            typ1 = typ1 + "'" + "," + "'" + Cbllbatch.Items[i].Value + "";
                        }
                    }

                }
            }
            for (int firstsp = 0; firstsp < Convert.ToInt16(FpSpread2.Sheets[0].RowCount); firstsp++)
            {
                if (FpSpread2.Sheets[0].Cells[firstsp, 1].Locked != true)
                {
                    int isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[firstsp, 1].Value);
                    if (isval == 1)
                    {
                        if (ddl_reqname.SelectedItem.Value == "6" && rdo_gatepass_dept.Checked == true)
                        {
                            staff_code_fp1 = Convert.ToString(FpSpread2.Sheets[0].GetTag(firstsp, 2));
                            type = d2.GetFunction("select RequestType from RQ_RequestHierarchy where ReqStaffAppNo='" + staff_code_fp1 + "'");
                        }
                        else if (ddl_reqname.SelectedItem.Value == "12") // poo 09.12.17
                        {
                            staff_code_fp1 = Convert.ToString(FpSpread2.Sheets[0].GetTag(firstsp, 2));
                            type = d2.GetFunction("select RequestType from RQ_RequestHierarchy where ReqStaffAppNo='" + staff_code_fp1 + "'");
                        }
                        else if (ddl_reqname.SelectedItem.Value == "14")//Added By Saranyadevi22.2.2019
                        {
                            if (rdb_hostelstd.Checked == true)
                            {
                                staff_code_fp1 = Convert.ToString(FpSpread2.Sheets[0].GetTag(firstsp, 2));
                                type = "14";

                            }

                        }
                        else
                        {
                            if (rdbreqstu.Checked == true)
                            {
                                staff_code_fp1 = Convert.ToString(FpSpread2.Sheets[0].GetTag(firstsp, 2));
                                type = "6";
                            }
                            else
                            {
                                staff_code_fp1 = FpSpread2.Sheets[0].GetText(firstsp, 4);
                                ReqStaffAppNo = Convert.ToInt64(da.GetFunction("select appl_id  from staff_appl_master a, staffmaster s where a.appl_no=s.appl_no and staff_code='" + staff_code_fp1 + "'"));
                                type = d2.GetFunction("select RequestType from RQ_RequestHierarchy where ReqStaffAppNo='" + ReqStaffAppNo + "'");
                            }
                        }
                        reqstaff++;
                        string delete_query = string.Empty;

                        if (rdbreqstu.Checked == true)
                        {
                            for (int i = 0; i < Cbllbatch.Items.Count; i++)
                            {
                                if (Cbllbatch.Items[i].Selected == true)
                                {
                                    delete_query = "if exists (select * from RQ_RequestHierarchy where RequestType ='" + RequestType + "' and ReqStaffAppNo='" + ReqStaffAppNo + "'  and DegreeCode='" + staff_code_fp1 + "'  and CollegeCode ='" + ddlcollegestaff.SelectedItem.Value + "') delete from RQ_RequestHierarchy where RequestType ='" + type + "' and ReqStaffAppNo='" + ReqStaffAppNo + "' and CollegeCode ='" + ddlcollegestaff.SelectedItem.Value + "' and DegreeCode='" + staff_code_fp1 + "'  ";
                                    d2.update_method_wo_parameter(delete_query, "Text");
                                }
                            }

                        }
                        else if (ddl_reqname.SelectedItem.Value == "14")//Added By Saranyadevi22.2.2019
                        {
                            if (rdb_hostelstd.Checked == true)
                            {
                                string delete_query1 = "if exists (select * from RQ_RequestHierarchy where RequestType ='" + RequestType + "' and ReqStaffAppNo='" + ReqStaffAppNo + "'  and DegreeCode='" + staff_code_fp1 + "')  delete from RQ_RequestHierarchy where RequestType ='" + type + "' and ReqStaffAppNo='" + ReqStaffAppNo + "' and DegreeCode='" + staff_code_fp1 + "'  ";
                                d2.update_method_wo_parameter(delete_query1, "Text");
                            }
                        }
                        else
                        {
                            delete_query = "if exists (select * from RQ_RequestHierarchy where RequestType ='" + RequestType + "' and ReqStaffAppNo='" + ReqStaffAppNo + "' and CollegeCode ='" + ddlcollegestaff.SelectedItem.Value + "') delete from RQ_RequestHierarchy where RequestType ='" + type + "' and ReqStaffAppNo='" + ReqStaffAppNo + "' and CollegeCode ='" + ddlcollegestaff.SelectedItem.Value + "'";
                            d2.update_method_wo_parameter(delete_query, "Text");
                        }
                        int cunt = 0;
                        for (int secondsp = 0; secondsp < Convert.ToInt16(FpSpread1.Sheets[0].RowCount); secondsp++)
                        {
                            string ReqApproveStage = "";
                            string bind = Convert.ToString(FpSpread1.Sheets[0].Cells[secondsp, 6].Text);
                            if (bind != "")
                            {
                                string[] split = bind.Split('-');
                                ReqApproveStage = split[0];
                                reqapp_pri = split[1];
                                abc1();
                                int colcount = FpSpread1.Sheets[0].ColumnCount;

                                // for (int i = 5; i < colcount; i += 2)
                                //  {
                                //     ReqApproveStage += 1;

                                int isval1 = Convert.ToInt32(FpSpread1.Sheets[0].Cells[secondsp, 5].Value);
                                int Checkval = 0; Convert.ToInt32(FpSpread1.Sheets[0].Cells[secondsp, 5].Value);
                                string CancelRights = "0";
                                if (ddl_reqname.SelectedItem.Value == "5")
                                {
                                    Checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[secondsp, 7].Value);
                                    if (Checkval == 1)
                                    {
                                        CancelRights = "1";
                                    }
                                }
                                string sql = "";
                                if (isval1 == 1)
                                {
                                    appstaff++;
                                    if (ddl_reqname.SelectedItem.Value == "11" && ddl_reqname.SelectedItem.Value == "6")
                                    {
                                        staffReqDeptcode = Convert.ToString(FpSpread2.Sheets[0].GetTag(firstsp, 2));
                                        staffAppDeptcode = Convert.ToString(FpSpread1.Sheets[0].GetTag(secondsp, 2));
                                    }
                                    staff_code_fp2 = FpSpread1.Sheets[0].GetText(secondsp, 3);
                                    ReqAppStaffAppNo = Convert.ToInt64(da.GetFunction("select appl_id  from staff_appl_master a, staffmaster s where a.appl_no=s.appl_no and staff_code='" + staff_code_fp2 + "'"));
                                    // ReqAppPriority = Convert.ToInt16(FpSpread1.Sheets[0].GetText(secondsp, i + 1));
                                    if (ddl_reqname.SelectedItem.Value == "6" && rdo_gatepass_dept.Checked == true)
                                    {
                                        sql = "insert into RQ_RequestHierarchy(RequestType,ReqStaffAppNo,ReqApproveStage,ReqAppStaffAppNo,ReqAppPriority,CollegeCode,ReqApproveStateCount,StaffReqDeptCode,StaffAppDeptCode,ReqCancelRights) values(" + RequestType + "," + staff_code_fp1 + "," + ReqApproveStage + "," + ReqAppStaffAppNo + "," + ReqAppPriority + "," + CollegeCode + ",'" + cri + "','" + staffReqDeptcode + "','" + staffAppDeptcode + "','" + CancelRights + "')";

                                        if (cunt != 1)
                                        {

                                            int but = 0;

                                            int.TryParse(Txttutor.Text, out but);
                                            if (Chktutor.Checked == true)
                                            {
                                                sql = sql + "  insert into RQ_RequestHierarchy(RequestType,ReqStaffAppNo,ReqApproveStage,ReqAppStaffAppNo,ReqAppPriority,CollegeCode,ReqApproveStateCount,StaffReqDeptCode,StaffAppDeptCode,ReqCancelRights,reqtutor) values(" + RequestType + "," + staff_code_fp1 + "," + but + ",'0','1'," + CollegeCode + ",'" + cri + "','" + staffReqDeptcode + "','" + staffAppDeptcode + "','" + CancelRights + "','1')";
                                                cunt = 1;
                                            }
                                        }

                                    }
                                    else if (ddl_reqname.SelectedItem.Value == "12") // poo 09.12.17
                                    {
                                        sql = "insert into RQ_RequestHierarchy(RequestType,ReqStaffAppNo,ReqApproveStage,ReqAppStaffAppNo,ReqAppPriority,CollegeCode,ReqApproveStateCount,StaffReqDeptCode,StaffAppDeptCode,ReqCancelRights) values(" + RequestType + "," + staff_code_fp1 + "," + ReqApproveStage + "," + ReqAppStaffAppNo + "," + ReqAppPriority + "," + CollegeCode + ",'" + cri + "','" + staffReqDeptcode + "','" + staffAppDeptcode + "','" + CancelRights + "')";
                                    }
                                    else
                                    {
                                        if (rdbreqstu.Checked == true)
                                        {

                                            sql = "insert into RQ_RequestHierarchy(RequestType,ReqStaffAppNo,ReqApproveStage,ReqAppStaffAppNo,ReqAppPriority,CollegeCode,ReqApproveStateCount,StaffReqDeptCode,StaffAppDeptCode,ReqCancelRights,degreecode) values(" + RequestType + ",'0'," + ReqApproveStage + "," + ReqAppStaffAppNo + "," + ReqAppPriority + "," + CollegeCode + ",'" + cri + "','" + staffReqDeptcode + "','" + staffAppDeptcode + "','" + CancelRights + "','" + staff_code_fp1 + "')";


                                        }
                                        else
                                            sql = "insert into RQ_RequestHierarchy(RequestType,ReqStaffAppNo,ReqApproveStage,ReqAppStaffAppNo,ReqAppPriority,CollegeCode,ReqApproveStateCount,StaffReqDeptCode,StaffAppDeptCode,ReqCancelRights) values(" + RequestType + "," + ReqStaffAppNo + "," + ReqApproveStage + "," + ReqAppStaffAppNo + "," + ReqAppPriority + "," + CollegeCode + ",'" + cri + "','" + staffReqDeptcode + "','" + staffAppDeptcode + "','" + CancelRights + "')";
                                        if (ddl_reqname.SelectedItem.Value == "6")
                                        {
                                            if (cunt != 1)
                                            {

                                                int but = 0;

                                                int.TryParse(Txttutor.Text, out but);
                                                if (Chktutor.Checked == true)
                                                {
                                                    for (int i = 0; i < Cbllbatch.Items.Count; i++)
                                                    {
                                                        if (Cbllbatch.Items[i].Selected == true)
                                                        {
                                                            sql = sql + " insert into RQ_RequestHierarchy(RequestType,ReqStaffAppNo,ReqApproveStage,ReqAppStaffAppNo,ReqAppPriority,CollegeCode,ReqApproveStateCount,StaffReqDeptCode,StaffAppDeptCode,ReqCancelRights,reqtutor,degreecode,BatchYear) values(" + RequestType + ",'0'," + but + ",'0','1'," + CollegeCode + ",'" + cri + "','" + staffReqDeptcode + "','" + staffAppDeptcode + "','" + CancelRights + "','1','" + staff_code_fp1 + "','" + Cbllbatch.Items[i].Value + "')";
                                                        }
                                                    }
                                                    cunt = 1;
                                                }
                                            }
                                        }
                                        if (ddl_reqname.SelectedItem.Value == "14")//Added By Saranyadevi22.2.2019
                                        {
                                            if (rdb_hostelstd.Checked == true)
                                            {
                                                sql = "insert into RQ_RequestHierarchy(RequestType,ReqStaffAppNo,ReqApproveStage,ReqAppStaffAppNo,ReqAppPriority,CollegeCode,ReqApproveStateCount,StaffReqDeptCode,StaffAppDeptCode,ReqCancelRights,degreecode) values(" + RequestType + ",'0'," + ReqApproveStage + "," + ReqAppStaffAppNo + "," + ReqAppPriority + "," + CollegeCode + ",'" + cri + "','" + staffReqDeptcode + "','" + staffAppDeptcode + "','" + CancelRights + "','" + staff_code_fp1 + "')";
                                            }
                                        }
                                    }
                                    q = da.update_method_wo_parameter(sql, "TEXT");
                                    tbl_div.Visible = false;
                                    imgdivalt.Visible = true;
                                    panel_erroralert.Visible = true;
                                    lbl_erroralert.Text = "Saved Successfully";
                                    btn_criteria1.Visible = false;
                                    btn_criteria2.Visible = false;
                                    btn_criteria3.Visible = false;
                                    btn_criteria4.Visible = false;
                                    btn_criteria5.Visible = false;
                                    btn_criteria6.Visible = false;
                                    btn_criteria7.Visible = false;
                                    btn_criteria8.Visible = false;
                                    btn_criteria1.Enabled = true;
                                    btn_criteria9.Visible = false;
                                    txt_criteria.Text = "";
                                    CLEARCOLOR();

                                }
                                // }
                            }
                        }
                    }
                }
            }
            if (reqstaff == 0)
            {
                Session["Priority"] = "";
                ViewState["checkvalue"] = "";


                CLEARCOLOR();

                imgdivalt.Visible = true;
                panel_erroralert.Visible = true;
                if (ddl_reqname.SelectedItem.Value != "14")
                    lbl_erroralert.Text = "Choose Request Staff";
                else
                    lbl_erroralert.Text = "Choose Request Hostel";
                return;
            }
            if (appstaff == 0)
            {
                Session["Priority"] = "";
                ViewState["checkvalue"] = "";
                imgdivalt.Visible = true;
                panel_erroralert.Visible = true;
                lbl_erroralert.Text = "Choose Approval Staff";
                return;
            }
            tbl_div.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }
    public void CLEARCOLOR()
    {
        btn_criteria1.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria2.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria3.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria4.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria5.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria6.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria7.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria8.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria9.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria1.ForeColor = Color.Black;
        btn_criteria2.BackColor = Color.Black;
        btn_criteria3.BackColor = Color.Black;
        btn_criteria4.BackColor = Color.Black;
        btn_criteria5.BackColor = Color.Black;
        btn_criteria6.BackColor = Color.Black;
        btn_criteria7.BackColor = Color.Black;
        btn_criteria8.BackColor = Color.Black;
        btn_criteria9.BackColor = Color.Black;
    }
    public void clearall()
    {
        try
        {

            if (ddl_reqname.SelectedItem.Text.Trim() == "Student Leave Request")
            {
                for (int reset = 0; reset < Convert.ToInt16(fpreport.Sheets[0].RowCount); reset++)
                {

                    fpreport.Sheets[0].Cells[reset, 8].Value = 0;
                    fpreport.Sheets[0].Cells[reset, 8].Locked = false;

                    fpreport.Sheets[0].Rows[reset].BackColor = ColorTranslator.FromHtml("#F0F0F0");
                }
            }

            for (int reset = 0; reset < Convert.ToInt16(FpSpread1.Sheets[0].RowCount); reset++)
            {

                FpSpread1.Sheets[0].Cells[reset, 5].Value = 0;
                FpSpread1.Sheets[0].Cells[reset, 5].Locked = false;
                FpSpread1.Sheets[0].Cells[reset, 6].Text = "";
                FpSpread1.Sheets[0].Cells[reset, 0].BackColor = ColorTranslator.FromHtml("#F0F0F0");
                FpSpread1.Sheets[0].Cells[reset, 1].BackColor = ColorTranslator.FromHtml("#F0F0F0");
                FpSpread1.Sheets[0].Cells[reset, 2].BackColor = ColorTranslator.FromHtml("#F0F0F0");
                FpSpread1.Sheets[0].Cells[reset, 5].BackColor = ColorTranslator.FromHtml("#F0F0F0");
                FpSpread1.Sheets[0].Cells[reset, 4].BackColor = ColorTranslator.FromHtml("#F0F0F0");
                FpSpread1.Sheets[0].Cells[reset, 6].BackColor = ColorTranslator.FromHtml("#F0F0F0");
                FpSpread1.Sheets[0].Cells[reset, 3].BackColor = ColorTranslator.FromHtml("#F0F0F0");
                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#F0F0F0");

            }
            for (int reset1 = 0; reset1 < Convert.ToInt16(FpSpread2.Sheets[0].RowCount); reset1++)
            {
                //if (BtnNew.Text != "New")
                //{
                if (rdo_gatepass_dept.Checked == false)
                {
                    FpSpread2.Sheets[0].Cells[reset1, 1].Value = 0;
                    FpSpread2.Sheets[0].Cells[reset1, 0].BackColor = ColorTranslator.FromHtml("#F0F0F0");
                    FpSpread2.Sheets[0].Cells[reset1, 1].BackColor = ColorTranslator.FromHtml("#F0F0F0");
                    FpSpread2.Sheets[0].Cells[reset1, 2].BackColor = ColorTranslator.FromHtml("#F0F0F0");
                    FpSpread2.Sheets[0].Cells[reset1, 5].BackColor = ColorTranslator.FromHtml("#F0F0F0");
                    FpSpread2.Sheets[0].Cells[reset1, 4].BackColor = ColorTranslator.FromHtml("#F0F0F0");
                    FpSpread2.Sheets[0].Cells[reset1, 3].BackColor = ColorTranslator.FromHtml("#F0F0F0");
                    FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#F0F0F0");
                }
                else
                {
                    FpSpread2.Sheets[0].Cells[reset1, 1].Value = 0;
                    FpSpread2.Sheets[0].Cells[reset1, 0].BackColor = ColorTranslator.FromHtml("#F0F0F0");
                    FpSpread2.Sheets[0].Cells[reset1, 1].BackColor = ColorTranslator.FromHtml("#F0F0F0");
                    FpSpread2.Sheets[0].Cells[reset1, 2].BackColor = ColorTranslator.FromHtml("#F0F0F0");
                    FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#F0F0F0");
                }
                // }
            }
        }
        catch { }


    }
    protected void BtnNew_Click(object sender, EventArgs e)
    {
        newspread();
        for (int i = 0; i < chklststaffDept.Items.Count; i++)
        {
            chklststaffDept.Items[i].Selected = true;
            //txtstaffDept.Text = "---Select---";
        }
        for (int i = 0; i < chklststaffType.Items.Count; i++)
        {
            chklststaffType.Items[i].Selected = true;
            //txtstaffType.Text = "---Select---";
        }
        for (int i = 0; i < chlstaffdeg.Items.Count; i++)
        {
            chlstaffdeg.Items[i].Selected = true;
            //txtstaffDeg.Text = "---Select---";
        }

        for (int i = 0; i < chldeptstaff.Items.Count; i++)
        {
            chldeptstaff.Items[i].Selected = true;
            //txtstaffDepart.Text = "---Select---";
        }
        for (int i = 0; i < chlstafftpyenew.Items.Count; i++)
        {
            chlstafftpyenew.Items[i].Selected = true;
            // txtstaff_type.Text = "---Select---";
        }
        for (int i = 0; i < chklststaff.Items.Count; i++)
        {
            chklststaff.Items[i].Selected = true;
            // txtstaff.Text = "---Select---";
        }
        clearall();
        Label1.Visible = false;
        btnreset.Enabled = true;
        tbl_div.Visible = false;
        Session["Priority"] = null;
        btn_criteria1.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria1.ForeColor = Color.Black;
        btn_criteria1.Enabled = true;
        btn_criteria2.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria2.ForeColor = Color.Black;
        btn_criteria2.Enabled = false;
        btn_criteria3.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria3.ForeColor = Color.Black;
        btn_criteria3.Enabled = false;
        btn_criteria4.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria4.ForeColor = Color.Black;
        btn_criteria4.Enabled = false;
        btn_criteria5.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria5.ForeColor = Color.Black;
        btn_criteria5.Enabled = false;
        btn_criteria6.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria6.ForeColor = Color.Black;
        btn_criteria6.Enabled = false;
        btn_criteria7.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria7.ForeColor = Color.Black;
        btn_criteria7.Enabled = false;
        btn_criteria8.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria8.ForeColor = Color.Black;
        btn_criteria8.Enabled = false;
        btn_criteria9.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria9.ForeColor = Color.Black;
        btn_criteria9.Enabled = false;
        txt_criteria.Text = "";
        ViewState["checkvalue"] = null;

    }
    protected void btnview_Click(object sender, EventArgs e)
    {
        #region
        //FpSpread2.SaveChanges();
        //for (int i = 0; i <= Convert.ToInt16(FpSpread2.Sheets[0].RowCount) - 1; i++)
        //{
        //    int isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[i, 1].Value);
        //    get_staff_code = FpSpread2.Sheets[0].GetText(i, 4);
        //    if (isval == 1)
        //    {

        //        string sqlquery = "";
        //        sqlquery = "select * from ApprovalStaff where Apply_StaffCode = '" + get_staff_code + "' order by HerarchyOrder";
        //        ds = d2.select_method(sqlquery, hat2, "Text");

        //        string staff_name = FpSpread2.Sheets[0].Cells[i, 5].Text;
        //        //staff_code_DB = dtnew.Rows[i]["Approval_StaffCode"].ToString();
        //        //HY_Order_DB = dtnew.Rows[i]["HerarchyOrder"].ToString();
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            Label1.Visible = false;
        //            for (int i1 = 0; i1 < ds.Tables[0].Rows.Count; i1++)
        //            {
        //                btnview.Enabled = true;
        //                btnreset.Enabled = true;
        //            }
        //        }
        //        else
        //        {
        //            Label1.Visible = true;
        //            Label1.Text = "Not Set" + " The" + " hierarchyOrder" + " For " + staff_name + "";
        //            btnview.Enabled = false;
        //            btnreset.Enabled = false;
        //            return;
        //        }
        //    }
        //    else
        //    {

        //    }
        //}


        //int count = 0;
        //string staff_co = "";
        //for (int i = 0; i <= Convert.ToInt16(FpSpread2.Sheets[0].RowCount) - 1; i++)
        //{

        //    int isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[i, 1].Value);
        //    get_staff_code = FpSpread2.Sheets[0].GetText(i, 4);
        //    if (isval == 1)
        //    {
        //        if (staff_co == "")
        //        {
        //            count++;
        //            staff_co = FpSpread2.Sheets[0].GetText(i, 4).ToString();
        //        }
        //        else
        //        {
        //            count++;
        //            staff_co = staff_co + "','" + FpSpread2.Sheets[0].GetText(i, 4).ToString();

        //            strstaffdept = " in('" + staff_co + "')";
        //            sqlstrstaffdept1 = " Apply_StaffCode  " + strstaffdept + "";

        //        }

        //    }
        //}


        //if (count > 0)
        //{
        //    FpSpread1.Sheets[0].RowCount = 0;

        //    string sqlcmd = "";
        //    if (count == 1)
        //    {
        //        //sqlcmd = "select distinct Approval_Dept,Approval_Deg,Approval_StaffCode,Approval_StaffName,HerarchyOrder from ApprovalStaff where Apply_StaffCode = '" + staff_co + "' order by HerarchyOrder";
        //        sqlcmd = "select distinct h.dept_name,d.desig_name,Approval_StaffCode,s.staff_name,HerarchyOrder from ApprovalStaff a,staffmaster s,stafftrans st,hrdept_master h,desig_master d where s.staff_code=st.staff_code and s.staff_code=a.Approval_StaffCode and st.dept_code=h.dept_code and st.desig_code=d.desig_code and st.latestrec=1 and s.college_code=d.collegeCode and a.Apply_StaffCode =  '" + staff_co + "'  order by HerarchyOrder";
        //    }
        //    else
        //    {
        //        //sqlcmd = "select distinct Approval_Dept,Approval_Deg,Approval_StaffCode,Approval_StaffName,HerarchyOrder from ApprovalStaff where " + sqlstrstaffdept1 + " order by HerarchyOrder";
        //        sqlcmd = "select distinct h.dept_name,d.desig_name,Approval_StaffCode,s.staff_name,HerarchyOrder from ApprovalStaff a,staffmaster s,stafftrans st,hrdept_master h,desig_master d where s.staff_code=st.staff_code and s.staff_code=a.Approval_StaffCode and st.dept_code=h.dept_code and st.desig_code=d.desig_code and st.latestrec=1 and s.college_code=d.collegeCode and " + sqlstrstaffdept1 + "  order by HerarchyOrder";
        //    }
        //    dsload = dset.select_method_wo_parameter(sqlcmd, "Text");
        //    int sno = 1;
        //    if (dsload.Tables[0].Rows.Count > 0)
        //    {
        //        btnreset.Text = "Delete";
        //        btnreset.Enabled = true;

        //        for (int loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
        //        {
        //            ++FpSpread1.Sheets[0].RowCount;
        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dsload.Tables[0].Rows[loop]["dept_name"].ToString();
        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["desig_name"].ToString();
        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["Approval_StaffCode"].ToString();
        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = dsload.Tables[0].Rows[loop]["staff_name"].ToString();
        //            FpSpread1.Sheets[0].Columns[5].Visible = false;
        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = dsload.Tables[0].Rows[loop]["HerarchyOrder"].ToString();
        //            sno++;
        //        }
        //        FpSpread1.SaveChanges();
        //        FpSpread1.Visible = true;
        //        Label1.Visible = false;
        //    }
        //    else
        //    {
        //        FpSpread1.Visible = false;
        //        Label1.Visible = true;
        //        Label1.Text = "No Record(s) Found";
        //    }
        //}
        //else
        //{

        //}

        // btnreset.Enabled = false;
        #endregion

        StringBuilder sbFrom = new StringBuilder();
        StringBuilder sbTo = new StringBuilder();
        StringBuilder sbDeg = new StringBuilder();
        fpreport.SaveChanges();
        if (ddl_reqname.SelectedValue.Trim() == "10" && rblStaffStudent.SelectedIndex == 0)
        {
            for (int firstsp = 0; firstsp < Convert.ToInt16(fpreport.Sheets[0].RowCount); firstsp++)
            {
                int isval = Convert.ToInt32(fpreport.Sheets[0].Cells[firstsp, 8].Value);

                if (isval == 1)
                {
                    int FromDays = Convert.ToInt32(fpreport.Sheets[0].Cells[firstsp, 4].Text);
                    int ToDays = Convert.ToInt32(fpreport.Sheets[0].Cells[firstsp, 5].Text);

                    int degCode = Convert.ToInt32(fpreport.Sheets[0].Cells[firstsp, 6].Tag);

                    sbFrom.Append(FromDays + ",");
                    sbTo.Append(ToDays + ",");
                    sbDeg.Append(degCode + ",");
                }
            }
            if (sbFrom.Length > 0)
            {
                sbFrom.Remove(sbFrom.Length - 1, 1);

            }
            if (sbTo.Length > 0)
            {
                sbTo.Remove(sbTo.Length - 1, 1);
            }
            if (sbDeg.Length > 0)
            {
                sbDeg.Remove(sbDeg.Length - 1, 1);
            }

            string Sql = "select RequestHierarchyPk from RQ_RequestHierarchy where RequestHierarchyPk in (select RequestHierarchyPk from RQ_RequestHierarchy where RequestHierarchyPk in (select RequestHierarchyPk from RQ_RequestHierarchy where ToDays in (" + sbTo + ")) and FromDays in (" + sbFrom.ToString() + ")) and ReqStaffAppNo in (" + sbDeg.ToString() + ")";
            DataSet dsRe = new DataSet();
            dsRe = dset.select_method_wo_parameter(Sql, "Text");
            if (dsRe.Tables.Count > 0 && dsRe.Tables[0].Rows.Count > 0)
            {
                StringBuilder sbReqPks = new StringBuilder();
                for (int i = 0; i < dsRe.Tables[0].Rows.Count; i++)
                {
                    sbReqPks.Append(Convert.ToString(dsRe.Tables[0].Rows[i][0]) + ",");
                }
                if (sbReqPks.Length > 0)
                {
                    sbReqPks.Remove(sbReqPks.Length - 1, 1);
                    viewappl(" and RequestHierarchyPk in (" + sbReqPks.ToString() + ") ");
                }
            }
        }
        else
        {
            viewappl(string.Empty);
        }

    }
    public void viewappl(string RequestHierarchyPks)
    {
        staffinfo1();
        //first spread..............

        if (txtstaffDept.Text != "---Select---" || chklststaffDept.Items.Count != null || chlstaffdeg.Items.Count != null)
        {
            int itemcount = 0;


            for (itemcount = 0; itemcount < chlstaffdeg.Items.Count; itemcount++)
            {
                if (chlstaffdeg.Items[itemcount].Selected == true)
                {
                    if (strstaff1 == "")
                        strstaff1 = "'" + chlstaffdeg.Items[itemcount].Value.ToString() + "'";
                    else
                        strstaff1 = strstaff1 + "," + "'" + chlstaffdeg.Items[itemcount].Value.ToString() + "'";
                }
            }
            if (strstaff1 != "")
            {
                // strstaff1 = strstaff1;
                strstaff1 = " in(" + strstaff1 + ")";
                sqlstrstaff1 = "and d.desig_code  " + strstaff1 + "";

            }
            else
            {
                strstaff1 = "";
            }
        }
        if (txtstaffDept.Text != "---Select---" || chklststaffDept.Items.Count != null)
        {
            int itemcount = 0;


            for (itemcount = 0; itemcount < chklststaffDept.Items.Count; itemcount++)
            {
                if (chklststaffDept.Items[itemcount].Selected == true)
                {
                    if (strstaffdept == "")
                        strstaffdept = "'" + chklststaffDept.Items[itemcount].Value.ToString() + "'";
                    else
                        strstaffdept = strstaffdept + "," + "'" + chklststaffDept.Items[itemcount].Value.ToString() + "'";
                }
            }
            if (strstaffdept != "")
            {
                strstaffdept = " in(" + strstaffdept + ")";
                sqlstrstaffdept1 = " and h.dept_code  " + strstaffdept + "";

            }
            else
            {
                strstaffdept = "";
            }
        }
        if (txtstaff_type.Text != "---Select---" || chklststaffType.Items.Count != null)
        {
            int itemcount = 0;


            for (itemcount = 0; itemcount < chklststaffType.Items.Count; itemcount++)
            {
                if (chklststaffType.Items[itemcount].Selected == true)
                {
                    if (strstafftype == "")
                        strstafftype = "'" + chklststaffType.Items[itemcount].Value.ToString() + "'";
                    else
                        strstafftype = strstafftype + "," + "'" + chklststaffType.Items[itemcount].Value.ToString() + "'";
                }
            }
            if (strstafftype != "")
            {
                strstafftype = " in(" + strstafftype + ")";
                sqlstrstafftype = " and stftype  " + strstafftype + "";

            }
            else
            {
                strstafftype = "";
            }
        }

        FpSpread1.Sheets[0].RowCount = 0;

        string sqlcmd = "";
        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
        sqlcmd = "select distinct s.staff_code,a.appl_id ,s.staff_name,h.dept_name,d.desig_name from staff_appl_master a ,staffmaster s,hrdept_master h,desig_master d,stafftrans st where a.appl_no =s.appl_no and s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode and s.college_code = d.collegecode " + sqlstrstaff1 + " " + sqlstrstaffdept1 + " " + sqlstrstafftype + " and s.college_code='" + ddlcollege.SelectedValue.ToString() + "' and resign = 0 and settled = 0 and latestrec=1  order by h.dept_name,s.staff_code ";
        dsload = dset.select_method_wo_parameter(sqlcmd, "Text");
        int sno = 1;
        string staffcode1 = "";
        DataView dv = new DataView();
        if (dsload.Tables[0].Rows.Count > 0)
        {

            for (int loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
            {
                ++FpSpread1.Sheets[0].RowCount;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dsload.Tables[0].Rows[loop]["dept_name"].ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["desig_name"].ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txt;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["staff_code"].ToString();

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = dsload.Tables[0].Rows[loop]["staff_name"].ToString();
                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Value = 0;
                string a = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text;
                staffcode1 = d2.GetFunction("select a.appl_id from staff_appl_master a,staffmaster s where a.appl_no =s.appl_no and staff_code ='" + a + "'");

                string staff = d2.GetFunction("select ReqAppStaffAppNo from RQ_RequestHierarchy where ReqAppStaffAppNo ='" + staffcode1 + "' and RequestType='" + ddl_reqname.SelectedItem.Value + "' " + RequestHierarchyPks + "");
                if (staffcode1 == staff)
                {
                    string stagetxt = d2.GetFunction("SELECT ReqApproveStage FROM RQ_RequestHierarchy WHERE ReqAppStaffAppNo='" + staff + "' and RequestType='" + ddl_reqname.SelectedItem.Value + "'  " + RequestHierarchyPks + "");
                    pri_txt = d2.GetFunction("select ReqAppPriority FROM RQ_RequestHierarchy WHERE ReqAppStaffAppNo='" + staff + "' and ReqApproveStage='" + stagetxt + "' and RequestType='" + ddl_reqname.SelectedItem.Value + "'  " + RequestHierarchyPks + "");
                    string CancelValue = d2.GetFunction("select ReqCancelRights FROM RQ_RequestHierarchy WHERE ReqAppStaffAppNo='" + staff + "' and ReqApproveStage='" + stagetxt + "' and RequestType='" + ddl_reqname.SelectedItem.Value + "'  " + RequestHierarchyPks + "");
                    abc();
                    string con = stagetxt + "-" + con_txt;
                    if (stagetxt == "1")
                    {
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#F0A3CC");
                    }
                    if (stagetxt == "2")
                    {
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#B7FBAE");
                    }
                    if (stagetxt == "3")
                    {
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#E7D172");
                    }
                    if (stagetxt == "4")
                    {
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#CDD7E0");
                    }
                    if (stagetxt == "5")
                    {
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#EBF3FB");
                    }
                    if (stagetxt == "6")
                    {
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#ECF4FB");
                    }
                    if (stagetxt == "7")
                    {
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#ECF4FB");
                    }
                    if (stagetxt == "8")
                    {
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#ECF4FB");
                    }
                    if (stagetxt == "9")
                    {
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#ECF4FB");
                    }
                    //if (stagetxt == "3")
                    //{
                    //    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#F0A3CC");
                    //}
                    //  FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#F0A3CC");
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Value = 1;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = con;
                    if (ddl_reqname.SelectedValue == "5") // poo
                    {
                        if (CancelValue.Trim() == "True")
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Value = 1; // poo
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Value = 0;
                        }
                    }
                    FpSpread1.Enabled = true;
                    btnreset.Enabled = true;
                    btnview.Enabled = true;
                }
                sno++;
            }
            FpSpread1.SaveChanges();
            FpSpread1.Visible = true;
            Label1.Visible = false;
        }
        else
        {
            FpSpread1.Visible = false;
            Label1.Visible = true;
            Label1.Text = "No Record(s) Found";
        }

        FpSpread1.Sheets[0].PageSize = 12;
        FpSpread1.TitleInfo.Height = 30;
        if (FpSpread1.Sheets[0].RowCount > 10)
        {
            FpSpread1.Height = 390;
        }
        else
        {
            FpSpread1.Height = (FpSpread1.Sheets[0].RowCount * 25) + 140;
        }


    }
    public void abc1()
    {
        if (reqapp_pri == "A")
        {
            ReqAppPriority = "1";
        }
        if (reqapp_pri == "B")
        {
            ReqAppPriority = "2";
        }
        if (reqapp_pri == "C")
        {
            ReqAppPriority = "3";
        }
        if (reqapp_pri == "D")
        {
            ReqAppPriority = "4";
        }
        if (reqapp_pri == "E")
        {
            ReqAppPriority = "5";
        }
        if (reqapp_pri == "F")
        {
            ReqAppPriority = "6";
        }
        if (reqapp_pri == "G")
        {
            ReqAppPriority = "7";
        }
        if (reqapp_pri == "H")
        {
            ReqAppPriority = "8";
        }
        if (reqapp_pri == "I")
        {
            ReqAppPriority = "9";
        }
        if (reqapp_pri == "J")
        {
            ReqAppPriority = "10";
        }
        if (reqapp_pri == "K")
        {
            ReqAppPriority = "11";
        }
        if (reqapp_pri == "L")
        {
            ReqAppPriority = "12";
        }
        if (reqapp_pri == "M")
        {
            ReqAppPriority = "13";
        }
        if (reqapp_pri == "N")
        {
            ReqAppPriority = "14";
        }
        if (reqapp_pri == "O")
        {
            ReqAppPriority = "15";
        }
        if (reqapp_pri == "P")
        {
            ReqAppPriority = "16";
        }
        if (reqapp_pri == "Q")
        {
            ReqAppPriority = "17";
        }
        if (reqapp_pri == "R")
        {
            ReqAppPriority = "18";
        }
        if (reqapp_pri == "S")
        {
            ReqAppPriority = "19";
        }
        if (reqapp_pri == "T")
        {
            ReqAppPriority = "20";
        }
        if (reqapp_pri == "U")
        {
            ReqAppPriority = "21";
        }
        if (reqapp_pri == "V")
        {
            ReqAppPriority = "22";
        }
        if (reqapp_pri == "W")
        {
            ReqAppPriority = "23";
        }
        if (reqapp_pri == "X")
        {
            ReqAppPriority = "24";
        }
        if (reqapp_pri == "Y")
        {
            ReqAppPriority = "25";
        }
        if (reqapp_pri == "Z")
        {
            ReqAppPriority = "26";
        }
    }
    public void abc()
    {
        if (pri_txt == "1")
        {
            con_txt = "A";
        }
        if (pri_txt == "2")
        {
            con_txt = "B";
        }
        if (pri_txt == "3")
        {
            con_txt = "C";
        }
        if (pri_txt == "4")
        {
            con_txt = "D";
        }
        if (pri_txt == "5")
        {
            con_txt = "E";
        }
        if (pri_txt == "6")
        {
            con_txt = "F";
        }
        if (pri_txt == "7")
        {
            con_txt = "G";
        }
        if (pri_txt == "8")
        {
            con_txt = "H";
        }
        if (pri_txt == "9")
        {
            con_txt = "I";
        }
        if (pri_txt == "10")
        {
            con_txt = "J";
        }
        if (pri_txt == "11")
        {
            con_txt = "K";
        }
        if (pri_txt == "12")
        {
            con_txt = "L";
        }
        if (pri_txt == "13")
        {
            con_txt = "M";
        }
        if (pri_txt == "14")
        {
            con_txt = "N";
        }
        if (pri_txt == "15")
        {
            con_txt = "O";
        }
        if (pri_txt == "16")
        {
            con_txt = "P";
        }
        if (pri_txt == "17")
        {
            con_txt = "Q";
        }
        if (pri_txt == "18")
        {
            con_txt = "R";
        }
        if (pri_txt == "19")
        {
            con_txt = "S";
        }
        if (pri_txt == "20")
        {
            con_txt = "T";
        }
        if (pri_txt == "21")
        {
            con_txt = "U";
        }
        if (pri_txt == "22")
        {
            con_txt = "V";
        }
        if (pri_txt == "23")
        {
            con_txt = "W";
        }
        if (pri_txt == "24")
        {
            con_txt = "X";
        }
        if (pri_txt == "25")
        {
            con_txt = "Y";
        }
        if (pri_txt == "26")
        {
            con_txt = "Z";
        }
    }
    public void newspread()
    {
        for (int cnt = 0; cnt < chklststaffType.Items.Count; cnt++)
        {
            if (staff_type1 == "")
            {
                staff_type1 = chklststaffType.Items[cnt].Value;
            }
            else
            {
                staff_type1 = staff_type1 + "','" + chklststaffType.Items[cnt].Value;
            }

        }

        for (int cnt1 = 0; cnt1 < chklststaffDept.Items.Count; cnt1++)
        {
            if (dept_all1 == "")
            {
                dept_all1 = chklststaffDept.Items[cnt1].Value;
            }
            else
            {
                dept_all1 = dept_all1 + "','" + chklststaffDept.Items[cnt1].Value;
            }
        }

        for (int cnt2 = 0; cnt2 < chlstaffdeg.Items.Count; cnt2++)
        {
            if (design_all1 == "")
            {
                design_all1 = chlstaffdeg.Items[cnt2].Value;
            }
            else
            {
                design_all1 = design_all1 + "','" + chlstaffdeg.Items[cnt2].Value;
            }
        }
        FpSpread1.Sheets[0].RowCount = 0;
        string sqlcmd = "";

        sqlcmd = "select distinct s.staff_code,s.staff_name,h.dept_name,d.desig_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode and h.dept_code in ('" + dept_all1 + "') and d.desig_code in ('" + design_all1 + "') and s.college_code='" + ddlcollege.SelectedValue.ToString() + "' and stftype in ('" + staff_type1 + "') and resign = 0 and settled = 0 and latestrec=1";
        dsload = dset.select_method_wo_parameter(sqlcmd, "Text");
        int sno = 1;
        if (dsload.Tables[0].Rows.Count > 0)
        {
            for (int loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
            {
                ++FpSpread1.Sheets[0].RowCount;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dsload.Tables[0].Rows[loop]["dept_name"].ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["desig_name"].ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["staff_code"].ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = dsload.Tables[0].Rows[loop]["staff_name"].ToString();

                sno++;
            }
            FpSpread1.SaveChanges();
            FpSpread1.Visible = true;
            //lblerrmainapp.Visible = false;
        }
        else
        {

        }

        FpSpread1.Sheets[0].PageSize = 12;
        FpSpread1.TitleInfo.Height = 30;
        if (FpSpread1.Sheets[0].RowCount > 10)
        {
            FpSpread1.Height = 390;
        }
        else
        {
            FpSpread1.Height = (FpSpread1.Sheets[0].RowCount * 25) + 140;
        }
    }
    protected void lb2_Click(object sender, EventArgs e) //sankar edit For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    public void applyDetails()
    {
        string applydeatils = "select distinct apply_staffcode from ApprovalStaff";
        DataTable dtappstaff = da.select_method_wop_table(applydeatils, "Text");

        for (int i = 0; i <= Convert.ToInt16(FpSpread2.Sheets[0].RowCount) - 1; i++)
        {
            string staff_colorset = FpSpread2.Sheets[0].Cells[i, 4].Text.ToString();
            dtappstaff.DefaultView.RowFilter = "apply_staffcode='" + staff_colorset + "'";
            DataView dvapplstaff = dtappstaff.DefaultView;
            if (dvapplstaff.Count > 0)
            {
                FpSpread2.Sheets[0].Rows[i].BackColor = Color.LightGreen;
            }
        }

    }
    public void BindReqName()
    {
        try
        {
            string query = "";
            string Master1 = "";
            //string[] reqname = { "Item Request", "Service", "Visitor Appointment", "Complaints", "Leave Request", "GatePass Request", "Event Request", "Payment Request", "Purchase Request" };
            //for (int i = 0; i < 9; i++)
            //{

            //    ddl_reqname.Items.Add(new ListItem(reqname[i], Convert.ToString(i + 1)));

            //}
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {

                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
                //else
                //    Master1 = group;
                query = "select * from Master_Settings where settings ='Request Hierarchy Rights' and group_code ='" + Master1 + "'";
            }
            else
            {
                Master1 = Session["usercode"].ToString();
                query = "select * from Master_Settings where settings ='Request Hierarchy Rights' and usercode ='" + Master1 + "'";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            ddl_reqname.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = Convert.ToString(ds.Tables[0].Rows[i]["value"]);
                    string[] split = val.Split(',');
                    for (int j = 0; j < split.Length; j++)
                    {
                        string v = Convert.ToString(split[j]);
                        requestname(v);
                        ddl_reqname.Items.Add(new ListItem(name, Convert.ToString(v)));
                    }
                }
            }

        }
        catch (Exception ex)
        {
        }
    }

    public void requestname(string val)
    {
        if (val == "1")
        {
            name = "Item Request";
        }
        if (val == "2")
        {
            name = "Service";
        }
        if (val == "3")
        {
            name = "Visitor Appointment";
        }
        if (val == "4")
        {
            name = "Complaints";
        }
        if (val == "5")
        {
            name = "Leave Request";
        }
        if (val == "6")
        {
            name = "GatePass Request";
        }
        if (val == "7")
        {
            name = "Event Request";
        }
        if (val == "8")
        {
            name = "Payment Request";
        }
        if (val == "9")
        {
            name = "Purchase Request";
        }
        if (val == "10")
        {
            name = "Student Leave Request";
        }
        if (val == "11")
        {
            name = "Certificate Request";
        }
        if (val == "12")
        {

            name = "Inward Request";


        }
        if (val == "13")//sd8/19
        {
            name = "Book Request";
        }
        if (val == "14")//sd21/19
        {
            name = "Vacated Request";
        }
    }
    public void BindCollege()
    {
        try
        {
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";

            ds = da.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollegestaff.DataSource = ds;
                ddlcollegestaff.DataTextField = "collname";
                ddlcollegestaff.DataValueField = "college_code";
                ddlcollegestaff.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindstaffdept1()
    {
        try
        {
            string query = "select distinct dept_code,dept_name from   hrdept_master where college_code='" + ddlcollegestaff.SelectedValue.ToString() + "' order by dept_name";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chldeptstaff.DataSource = ds;
                chldeptstaff.DataTextField = "dept_name";
                chldeptstaff.DataValueField = "dept_code";
                chldeptstaff.DataBind();
                chkdeptstaff.Checked = true;
                if (chldeptstaff.Items.Count > 0)
                {
                    for (int i = 0; i < chldeptstaff.Items.Count; i++)
                    {
                        chldeptstaff.Items[i].Selected = true;
                    }
                    txtstaffDepart.Text = "Dept(" + chldeptstaff.Items.Count + ")";
                }

            }
        }

        catch (Exception ex)
        {
        }
    }
    public void bind_stafType1()
    {
        try
        {
            string query = "SELECT DISTINCT StfType FROM StaffTrans T,HrDept_Master D WHERE T.Dept_Code = D.Dept_Code AND T.Latestrec = 1 and d.college_code=" + ddlcollegestaff.SelectedValue.ToString() + "";
            ds = da.select_method_wo_parameter(query, "Text");
            {
                chlstafftpyenew.Items.Clear();
                chlstafftpyenew.DataSource = ds;
                chlstafftpyenew.DataTextField = "StfType";
                chlstafftpyenew.DataValueField = "StfType";
                chlstafftpyenew.DataBind();
                chkstafftypenew.Checked = true;
                if (chlstafftpyenew.Items.Count > 0)
                {
                    for (int i = 0; i < chlstafftpyenew.Items.Count; i++)
                    {
                        chlstafftpyenew.Items[i].Selected = true;
                    }
                    txtstaff_type.Text = "Staff Type(" + chlstafftpyenew.Items.Count + ")";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindstaffdesg()
    {
        try
        {
            string query = "select desig_code,desig_name from desig_master where collegeCode='" + ddlcollegestaff.SelectedValue.ToString() + "' ";
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklststaff.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklststaff.Items.Clear();
                    chklststaff.DataSource = ds;
                    chklststaff.DataTextField = "desig_name";
                    chklststaff.DataValueField = "desig_code";
                    chklststaff.DataBind();
                    chksatff.Checked = true;
                    if (chklststaff.Items.Count > 0)
                    {
                        for (int i = 0; i < chklststaff.Items.Count; i++)
                        {
                            chklststaff.Items[i].Selected = true;
                        }
                        txtstaff.Text = "Desig(" + chklststaff.Items.Count + ")";
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindspread1()
    {
        if (ddl_reqname.Items.Count <= 0)
        {
            alert_div.Visible = true;
            lbl_alertt.Text = "Request Type Should Not Be Empty,Change The Settings After That Proceed";
        }
        //else if (ddl_reqname.SelectedItem.Text == "")
        //{
        //    alert_div.Visible = true;
        //    lbl_alertt.Text = "Request Type Should Not Be Empty,Change The Settings After That Proceed";
        //}
        else
        {
            // if (txtstaffDepart.Text == "---Select---" && txtstaff_type.Text == "---Select---" && txtstaff.Text == "---Select---")
            // {
            //Staff Type......................Bind
            for (int cnt = 0; cnt < chlstafftpyenew.Items.Count; cnt++)
            {
                if (staff_type == "")
                {
                    staff_type = chlstafftpyenew.Items[cnt].Value;
                }
                else
                {
                    staff_type = staff_type + "','" + chlstafftpyenew.Items[cnt].Value;
                }

            }
            //dept...Bind
            for (int cnt1 = 0; cnt1 < chldeptstaff.Items.Count; cnt1++)
            {
                if (dept_all == "")
                {
                    dept_all = chldeptstaff.Items[cnt1].Value;
                }
                else
                {
                    dept_all = dept_all + "','" + chldeptstaff.Items[cnt1].Value;
                }
            }

            for (int cnt2 = 0; cnt2 < chklststaff.Items.Count; cnt2++)
            {
                if (dept_all == "")
                {
                    design_all = chklststaff.Items[cnt2].Value;
                }
                else
                {
                    design_all = design_all + "','" + chklststaff.Items[cnt2].Value;
                }
            }

            string sqlcmd = "";
            string staffcode1 = "";
            FpSpread2.SaveChanges();

            sqlcmd = "select distinct s.staff_code,a.appl_id ,s.staff_name,h.dept_name,d.desig_name from staff_appl_master a ,staffmaster s,hrdept_master h,desig_master d,stafftrans st where a.appl_no =s.appl_no and s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode and h.dept_code in ('" + dept_all + "') and d.desig_code in ('" + design_all + "') and s.college_code='" + ddlcollegestaff.SelectedValue.ToString() + "' and stftype in ('" + staff_type + "') and resign = 0 and settled = 0 and latestrec=1 order by h.dept_name,s.staff_code";
            sqlcmd = sqlcmd + " select ReqStaffAppNo,RequestType from RQ_RequestHierarchy";
            dsload.Clear();
            dsload = dset.select_method_wo_parameter(sqlcmd, "Text");
            int sno = 1;
            DataView dv = new DataView();
            if (dsload.Tables[0].Rows.Count > 0)
            {
                for (int loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                {
                    string sff_cd = Convert.ToString(dsload.Tables[0].Rows[loop]["staff_code"]);


                    FpSpread2.Sheets[0].RowCount++;

                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Value = 0;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["dept_name"].ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["desig_name"].ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = dsload.Tables[0].Rows[loop]["staff_code"].ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Tag = dsload.Tables[0].Rows[loop]["appl_id"].ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = dsload.Tables[0].Rows[loop]["staff_name"].ToString();

                    staffcode1 = Convert.ToString(FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Tag);
                    //zzz
                    if (dsload.Tables[1].Rows.Count > 0)
                    {

                        if (ddl_reqname.Items.Count > 0)
                        {
                            if (ddl_reqname.SelectedItem.Value != "")
                            {

                                if (staffcode1 != "" && ddl_reqname.SelectedItem.Value != "")
                                {
                                    dsload.Tables[1].DefaultView.RowFilter = "ReqStaffAppNo='" + staffcode1 + "' and RequestType='" + ddl_reqname.SelectedItem.Value + "'";
                                    dv = dsload.Tables[1].DefaultView;


                                    if (dv.Count > 0)
                                    {

                                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#F0A3CC");
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Value = 1;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Locked = true;
                                        FpSpread2.Enabled = true;
                                        btnreset.Enabled = true;
                                        btnview.Enabled = true;
                                    }
                                    else
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Value = 0;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Locked = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            alert_div.Visible = true;
                            lbl_alertt.Text = "Request Type Should Not Be Empty,Change The Settings After That Proceed";


                        }
                    }

                    sno++;
                }
                FpSpread2.SaveChanges();
                FpSpread2.Visible = true;

            }
            else
            {

            }

            FpSpread2.Sheets[0].PageSize = 12;
            FpSpread2.TitleInfo.Height = 30;
            if (FpSpread2.Sheets[0].RowCount > 10)
            {
                FpSpread2.Height = 390;
            }
            else
            {
                FpSpread2.Height = (FpSpread2.Sheets[0].RowCount * 25) + 140;
            }
            FpSpread2.Height = 390;
            //  }
        }
    }
    public void staffinfo()
    {
        FpSpread2.Sheets[0].PageSize = 12;
        FpSpread2.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
        FpSpread2.Pager.Mode = FarPoint.Web.Spread.PagerMode.NextPrev;
        FpSpread2.Pager.Align = HorizontalAlign.Right;

        FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
        darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle2.ForeColor = Color.Black;
        darkstyle2.HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
        FpSpread2.Sheets[0].AutoPostBack = false;
        FpSpread2.Pager.PageCount = 5;
        FpSpread2.ActiveSheetView.SheetCorner.DefaultStyle.Font.Bold = false;
        FpSpread2.ActiveSheetView.DefaultRowHeight = 25;
        FpSpread2.ActiveSheetView.Rows.Default.Font.Name = "Book Antiqua";
        FpSpread2.ActiveSheetView.Rows.Default.Font.Size = FontUnit.Small;
        FpSpread2.ActiveSheetView.Rows.Default.Font.Bold = false;
        FpSpread2.ActiveSheetView.Columns.Default.Font.Bold = false;
        FpSpread2.ActiveSheetView.Columns.Default.Font.Size = FontUnit.Small;
        FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
        FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        FpSpread2.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        FpSpread2.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        FpSpread2.Sheets[0].ColumnCount = 6;
        FpSpread2.Sheets[0].RowCount = 0;
        FarPoint.Web.Spread.CheckBoxCellType cbxd = new FarPoint.Web.Spread.CheckBoxCellType();
        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
        cbxd.AutoPostBack = false;
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Department";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Designation";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "StaffCode";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "StaffName";

        FpSpread2.Sheets[0].Columns[1].CellType = cbxd;
        FpSpread2.Sheets[0].Columns[0].CellType = txt;
        FpSpread2.Sheets[0].Columns[2].CellType = txt;
        FpSpread2.Sheets[0].Columns[3].CellType = txt;
        FpSpread2.Sheets[0].Columns[4].CellType = txt;
        FpSpread2.Sheets[0].Columns[5].CellType = txt;


        FpSpread2.Sheets[0].Columns[0].Width = 50;
        //FpSpread2.Sheets[0].Columns[0].Locked = true;
        //FpSpread2.Sheets[0].Columns[1].Locked = true;
        FpSpread2.Sheets[0].Columns[1].Width = 50;
        FpSpread2.Sheets[0].Columns[2].Width = 300;
        FpSpread2.Sheets[0].Columns[3].Width = 180;
        FpSpread2.Sheets[0].Columns[4].Width = 140;
        FpSpread2.Sheets[0].Columns[5].Width = 190;


        FpSpread2.Sheets[0].Columns[0].Locked = true;
        FpSpread2.Sheets[0].Columns[1].Locked = false;
        FpSpread2.Sheets[0].Columns[2].Locked = true;
        FpSpread2.Sheets[0].Columns[3].Locked = true;
        FpSpread2.Sheets[0].Columns[4].Locked = true;
        FpSpread2.Sheets[0].Columns[5].Locked = true;

        FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Width = 909;


        FpSpread2.CommandBar.Visible = false;
        FpSpread2.Sheets[0].RowHeader.Visible = false;
    }
    public void BindCollege1()
    {
        string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
        ds = da.select_method_wo_parameter(Query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlcollege.DataSource = ds;
            ddlcollege.DataTextField = "collname";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();
        }

    }
    public void bindstaffdept2()
    {
        string query = "select distinct dept_code,dept_name from   hrdept_master where college_code='" + ddlcollege.SelectedValue.ToString() + "' order by dept_name";

        ds = da.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            chklststaffDept.Items.Clear();
            chklststaffDept.DataSource = ds.Tables[0];
            chklststaffDept.DataTextField = "dept_name";
            chklststaffDept.DataValueField = "dept_code";
            chklststaffDept.DataBind();
            chksatffDept.Checked = true;
            if (chklststaffDept.Items.Count > 0)
            {
                for (int i = 0; i < chklststaffDept.Items.Count; i++)
                {
                    chklststaffDept.Items[i].Selected = true;
                }
                txtstaffDept.Text = "Dept(" + chklststaffDept.Items.Count + ")";
            }
        }
    }
    public void bind_stafType()
    {

        string Query = "SELECT DISTINCT StfType FROM StaffTrans T,HrDept_Master D WHERE T.Dept_Code = D.Dept_Code AND T.Latestrec = 1 and d.college_code=" + ddlcollege.SelectedValue.ToString() + "";

        ds = da.select_method_wo_parameter(Query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            chklststaffType.Items.Clear();
            chklststaffType.DataSource = ds;
            chklststaffType.DataTextField = "StfType";
            chklststaffType.DataValueField = "StfType";
            chklststaffType.DataBind();
            chksatffType.Checked = true;
            if (chklststaffType.Items.Count > 0)
            {
                for (int i = 0; i < chklststaffType.Items.Count; i++)
                {
                    chklststaffType.Items[i].Selected = true;
                }
                txtstaffType.Text = "Staff Type(" + chklststaffType.Items.Count + ")";
            }

        }
    }
    public void bindstaffdeg()
    {
        string query = "select desig_code,desig_name from desig_master where collegeCode='" + ddlcollege.SelectedValue.ToString() + "' ";
        ds = da.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            {
                chlstaffdeg.Items.Clear();
                chlstaffdeg.DataSource = ds;
                chlstaffdeg.DataTextField = "desig_name";
                chlstaffdeg.DataValueField = "desig_code";
                chlstaffdeg.DataBind();
                chkstaffdeg.Checked = true;
                if (chlstaffdeg.Items.Count > 0)
                {
                    for (int i = 0; i < chlstaffdeg.Items.Count; i++)
                    {
                        chlstaffdeg.Items[i].Selected = true;
                    }
                    txtstaffDeg.Text = "Desig(" + chlstaffdeg.Items.Count + ")";
                }
            }

        }
    }
    public void bindspread2()
    {
        if (ddl_reqname.Items.Count <= 0)
        {
            alert_div.Visible = true;
            lbl_alertt.Text = "Request Type Should Not Be Empty,Change The Settings After That Proceed";

        }
        else
        {
            for (int cnt = 0; cnt < chklststaffType.Items.Count; cnt++)
            {
                if (staff_type1 == "")
                {
                    staff_type1 = chklststaffType.Items[cnt].Value;
                }
                else
                {
                    staff_type1 = staff_type1 + "','" + chklststaffType.Items[cnt].Value;
                }

            }
            //dept...Bind
            for (int cnt1 = 0; cnt1 < chklststaffDept.Items.Count; cnt1++)
            {
                if (dept_all1 == "")
                {
                    dept_all1 = chklststaffDept.Items[cnt1].Value;
                }
                else
                {
                    dept_all1 = dept_all1 + "','" + chklststaffDept.Items[cnt1].Value;
                }
            }

            for (int cnt2 = 0; cnt2 < chlstaffdeg.Items.Count; cnt2++)
            {
                if (design_all1 == "")
                {
                    design_all1 = chlstaffdeg.Items[cnt2].Value;
                }
                else
                {
                    design_all1 = design_all1 + "','" + chlstaffdeg.Items[cnt2].Value;
                }
            }

            string sqlcmd = "";
            string staffcode1 = "";
            sqlcmd = "select distinct s.staff_code,a.appl_id ,s.staff_name,h.dept_name,d.desig_name from staff_appl_master a ,staffmaster s,hrdept_master h,desig_master d,stafftrans st where a.appl_no =s.appl_no and s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode and h.dept_code in ('" + dept_all1 + "') and d.desig_code in ('" + design_all1 + "') and s.college_code='" + ddlcollege.SelectedValue.ToString() + "' and stftype in ('" + staff_type1 + "') and resign = 0 and settled = 0 and latestrec=1 order by h.dept_name,s.staff_code";
            sqlcmd = sqlcmd + " select ReqStaffAppNo,ReqAppStaffAppNo,RequestType from RQ_RequestHierarchy";
            dsload = dset.select_method_wo_parameter(sqlcmd, "Text");
            int sno = 1;
            DataView dv = new DataView();
            if (dsload.Tables[0].Rows.Count > 0)
            {
                for (int loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                {

                    ++FpSpread1.Sheets[0].RowCount;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dsload.Tables[0].Rows[loop]["dept_name"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["desig_name"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["staff_code"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = dsload.Tables[0].Rows[loop]["appl_id"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = dsload.Tables[0].Rows[loop]["staff_name"].ToString();
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Value = 0;
                    staffcode1 = Convert.ToString(FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag);

                    //dsload.Tables[1].DefaultView.RowFilter = "ReqAppStaffAppNo='" + staffcode1 + "' and RequestType='" + ddl_reqname.SelectedItem.Value + "'";
                    //dv = dsload.Tables[1].DefaultView;


                    //if (dv.Count > 0)
                    //{

                    //    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#F0A3CC");
                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Value = 1;
                    //    FpSpread1.Enabled = true;
                    //    btnreset.Enabled = true;
                    //    btnview.Enabled = true;
                    //    string ReqAppStaffAppNo = Convert.ToString(dv[0]["ReqAppStaffAppNo"]);
                    //    string statecount = d2.GetFunction("select ReqApproveStage from RQ_RequestHierarchy where ReqAppStaffAppNo='" + ReqAppStaffAppNo + "'");
                    //    pri_txt = d2.GetFunction("select ReqAppPriority from RQ_RequestHierarchy where ReqAppStaffAppNo='" + ReqAppStaffAppNo + "'");
                    //    abc();
                    //    string con = statecount + "-" + con_txt;
                    //    for (int reset1 = 0; reset1 < Convert.ToInt16(FpSpread1.Sheets[0].RowCount); reset1++)
                    //    {
                    //        // FpSpread1.Sheets[0].Cells[reset1, 6].Text = con;
                    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = con;
                    //    }
                    //}

                    //else
                    //{
                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Value = 0;
                    //}


                    sno++;
                }
                FpSpread1.SaveChanges();
                FpSpread1.Visible = true;
                //lblerrmainapp.Visible = false;
            }
            else
            {
                //lblerrmainapp.Visible = true;
                //lblerrmainapp.Text = "No Record(s) Found";
            }

            FpSpread1.Sheets[0].PageSize = 12;
            FpSpread1.TitleInfo.Height = 30;
            if (FpSpread1.Sheets[0].RowCount > 10)
            {
                FpSpread1.Height = 390;
            }
            else
            {
                FpSpread1.Height = (FpSpread1.Sheets[0].RowCount * 25) + 140;
            }
            //}
        }
    }
    public void staffinfo1()
    {
        FpSpread1.Sheets[0].PageSize = 5;
        FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
        FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.NextPrev;
        FpSpread1.Pager.Align = HorizontalAlign.Right;
        FpSpread1.Pager.Font.Bold = true;
        FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
        darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle2.ForeColor = Color.Black;
        darkstyle2.HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;

        FpSpread1.Pager.PageCount = 5;
        FpSpread1.ActiveSheetView.SheetCorner.DefaultStyle.Font.Bold = false;
        FpSpread1.ActiveSheetView.DefaultRowHeight = 25;
        FpSpread1.ActiveSheetView.Rows.Default.Font.Name = "Book Antiqua";
        FpSpread1.ActiveSheetView.Rows.Default.Font.Size = FontUnit.Small;
        FpSpread1.ActiveSheetView.Rows.Default.Font.Bold = false;
        FpSpread1.ActiveSheetView.Columns.Default.Font.Bold = false;
        FpSpread1.ActiveSheetView.Columns.Default.Font.Size = FontUnit.Small;
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        FpSpread1.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        FpSpread1.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
        FpSpread1.Sheets[0].ColumnCount = 5;
        FpSpread1.Sheets[0].RowCount = 0;
        FarPoint.Web.Spread.CheckBoxCellType cbxd = new FarPoint.Web.Spread.CheckBoxCellType();
        FarPoint.Web.Spread.CheckBoxCellType cbCancel = new FarPoint.Web.Spread.CheckBoxCellType();
        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
        cbxd.AutoPostBack = true;
        cbCancel.AutoPostBack = false;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Department";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Designation";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "StaffCode";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "StaffName";
        tbl_div.Visible = true;

        string criteria = Convert.ToString(txt_criteria.Text);

        if (criteria == "1")
        {
            btn_criteria1.Visible = true;
            btn_criteria2.Visible = false;
            btn_criteria3.Visible = false;
            btn_criteria4.Visible = false;
            btn_criteria5.Visible = false;
            btn_criteria6.Visible = false;
            btn_criteria7.Visible = false;
            btn_criteria8.Visible = false;
            btn_criteria9.Visible = false;
        }
        else if (criteria == "2")
        {
            btn_criteria1.Visible = true;
            btn_criteria2.Visible = true;
            btn_criteria3.Visible = false;
            btn_criteria4.Visible = false;
            btn_criteria5.Visible = false;
            btn_criteria6.Visible = false;
            btn_criteria7.Visible = false;
            btn_criteria8.Visible = false;
            btn_criteria9.Visible = false;
        }
        else if (criteria == "3")
        {
            btn_criteria1.Visible = true;
            btn_criteria2.Visible = true;
            btn_criteria3.Visible = true;
            btn_criteria4.Visible = false;
            btn_criteria5.Visible = false;
            btn_criteria6.Visible = false;
            btn_criteria7.Visible = false;
            btn_criteria8.Visible = false;
            btn_criteria9.Visible = false;
        }
        else if (criteria == "4")
        {
            btn_criteria1.Visible = true;
            btn_criteria2.Visible = true;
            btn_criteria3.Visible = true;
            btn_criteria4.Visible = true;
            btn_criteria5.Visible = false;
            btn_criteria6.Visible = false;
            btn_criteria7.Visible = false;
            btn_criteria8.Visible = false;
            btn_criteria9.Visible = false;
        }
        else if (criteria == "5")
        {
            btn_criteria1.Visible = true;
            btn_criteria2.Visible = true;
            btn_criteria3.Visible = true;
            btn_criteria4.Visible = true;
            btn_criteria5.Visible = true;
            btn_criteria6.Visible = false;
            btn_criteria7.Visible = false;
            btn_criteria8.Visible = false;
            btn_criteria9.Visible = false;
        }
        else if (criteria == "6")
        {
            btn_criteria1.Visible = true;
            btn_criteria2.Visible = true;
            btn_criteria3.Visible = true;
            btn_criteria4.Visible = true;
            btn_criteria5.Visible = true;
            btn_criteria6.Visible = true;
            btn_criteria7.Visible = false;
            btn_criteria8.Visible = false;
            btn_criteria9.Visible = false;
        }
        else if (criteria == "7")
        {
            btn_criteria1.Visible = true;
            btn_criteria2.Visible = true;
            btn_criteria3.Visible = true;
            btn_criteria4.Visible = true;
            btn_criteria5.Visible = true;
            btn_criteria6.Visible = true;
            btn_criteria7.Visible = true;
            btn_criteria8.Visible = false;
            btn_criteria9.Visible = false;
        }
        else if (criteria == "8")
        {
            btn_criteria1.Visible = true;
            btn_criteria2.Visible = true;
            btn_criteria3.Visible = true;
            btn_criteria4.Visible = true;
            btn_criteria5.Visible = true;
            btn_criteria6.Visible = true;
            btn_criteria7.Visible = true;
            btn_criteria8.Visible = true;
            btn_criteria9.Visible = false;
        }
        else if (criteria == "9")
        {
            btn_criteria1.Visible = true;
            btn_criteria2.Visible = true;
            btn_criteria3.Visible = true;
            btn_criteria4.Visible = true;
            btn_criteria5.Visible = true;
            btn_criteria6.Visible = true;
            btn_criteria7.Visible = true;
            btn_criteria8.Visible = true;
            btn_criteria9.Visible = true;
        }
        else if (criteria == "" || criteria == "0")
        {
            tbl_div.Visible = false;
        }
        if (Chktutor.Checked == true)
        {
            if (Txttutor.Text == "1")
            {
                btn_criteria1.Visible = false;
                btn_criteria2.Enabled = true;
            }
            if (Txttutor.Text == "2")
                btn_criteria2.Visible = false;
            if (Txttutor.Text == "3")
                btn_criteria3.Visible = false;
            if (Txttutor.Text == "4")
                btn_criteria4.Visible = false;
            if (Txttutor.Text == "5")
                btn_criteria5.Visible = false;
            if (Txttutor.Text == "6")
                btn_criteria6.Visible = false;
            if (Txttutor.Text == "7")
                btn_criteria7.Visible = false;
            if (Txttutor.Text == "8")
                btn_criteria8.Visible = false;
            if (Txttutor.Text == "9")
                btn_criteria9.Visible = false;
        }
        //for (int i = 0; i < criteria; i++)
        //{
        FpSpread1.Sheets[0].ColumnCount++;

        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Select";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].CellType = cbxd;
        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 50;
        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Locked = false;
        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

        FpSpread1.Sheets[0].ColumnCount++;

        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "HierarchyOrder";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].CellType = txt;
        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 130;
        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Locked = true;
        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

        if (ddl_reqname.SelectedValue == "5")
        {
            FpSpread1.Sheets[0].ColumnCount++;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Cancel Approval";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].CellType = cbCancel;
            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 130;
            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Locked = false;
            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        }


        //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
        //}
        // FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Select";
        //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "HierarchyOrder";

        //  FpSpread1.Sheets[0].Columns[5].CellType = cbxd;
        FpSpread1.Sheets[0].Columns[0].CellType = txt;
        FpSpread1.Sheets[0].Columns[1].CellType = txt;
        FpSpread1.Sheets[0].Columns[2].CellType = txt;
        FpSpread1.Sheets[0].Columns[3].CellType = txt;
        FpSpread1.Sheets[0].Columns[4].CellType = txt;
        // FpSpread1.Sheets[0].Columns[6].CellType = txt;

        FpSpread1.Sheets[0].Columns[0].Width = 50;
        //FpSpread2.Sheets[0].Columns[0].Locked = true;
        //FpSpread2.Sheets[0].Columns[1].Locked = true;
        FpSpread1.Sheets[0].Columns[1].Width = 300;
        FpSpread1.Sheets[0].Columns[2].Width = 180;
        FpSpread1.Sheets[0].Columns[3].Width = 130;
        FpSpread1.Sheets[0].Columns[4].Width = 130;
        // FpSpread1.Sheets[0].Columns[5].Width = 50;
        //  FpSpread1.Sheets[0].Columns[6].Width = 130;

        FpSpread1.Sheets[0].Columns[0].Locked = true;
        FpSpread1.Sheets[0].Columns[1].Locked = true;
        FpSpread1.Sheets[0].Columns[2].Locked = true;
        FpSpread1.Sheets[0].Columns[3].Locked = true;
        FpSpread1.Sheets[0].Columns[4].Locked = true;
        // FpSpread1.Sheets[0].Columns[5].Locked = false;
        //  FpSpread1.Sheets[0].Columns[6].Locked = true;

        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        // FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
        // FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Width = 955;
        //FpSpread1.Sheets[0].AutoPostBack = true;

        FpSpread1.CommandBar.Visible = false;
        FpSpread1.Sheets[0].RowHeader.Visible = false;
    }
    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    public void btnerrclose_Click(object sender, EventArgs e)
    {
        string inwardval = Convert.ToString(ddl_reqname.SelectedItem.Value);

        imgdivalt.Visible = false;
        clearall();
        tbl_div.Visible = false;
        if (inwardval != "14")
        {
            staffinfo();
            staffinfo1();
            bindspread1();
            bindspread2();

            btnMainGo_Click(sender, e); // poo
        }
        else
        {
            bindspd_gatepass();
            bindspread_gatepass_dept();

        }

    }
    public void btn_criteria1_Click(object sender, EventArgs e)
    {
        try
        {
            string p = "1";
            string Priority = "";

            Session["Priority"] = p;
            btn_criteria1.BackColor = Color.Green;
            btn_criteria1.ForeColor = Color.White;
            //  btn_criteria2.Enabled = true;
        }
        catch
        {
        }
    }
    public void btn_criteria2_Click(object sender, EventArgs e)
    {
        try
        {
            string p = "2";
            string Priority = "";
            btn_criteria1.Enabled = false;
            //  btn_criteria3.Enabled = true;
            Session["Priority"] = p;
            btn_criteria1.BackColor = Color.Tomato;
            btn_criteria2.ForeColor = Color.White;
            btn_criteria2.BackColor = Color.Green;
        }
        catch
        {
        }
    }
    public void btn_criteria3_Click(object sender, EventArgs e)
    {
        try
        {
            string p = "3";
            string Priority = "";
            btn_criteria2.Enabled = false;
            btn_criteria1.Enabled = false;
            // btn_criteria4.Enabled = true;
            Session["Priority"] = p;
            btn_criteria1.BackColor = Color.Tomato;
            btn_criteria2.BackColor = Color.Tomato;
            btn_criteria3.ForeColor = Color.White;
            btn_criteria3.BackColor = Color.Green;

        }
        catch
        {
        }
    }
    public void btn_criteria4_Click(object sender, EventArgs e)
    {
        try
        {
            string p = "4";
            string Priority = "";
            btn_criteria1.Enabled = false;
            btn_criteria2.Enabled = false;
            btn_criteria3.Enabled = false;
            //  btn_criteria5.Enabled = true;
            Session["Priority"] = p;
            btn_criteria1.BackColor = Color.Tomato;
            btn_criteria2.BackColor = Color.Tomato;
            btn_criteria3.BackColor = Color.Tomato;
            btn_criteria4.ForeColor = Color.White;
            btn_criteria4.BackColor = Color.Green;
        }
        catch
        {
        }
    }
    public void btn_criteria5_Click(object sender, EventArgs e)
    {
        try
        {
            string p = "5";
            string Priority = "";
            btn_criteria1.Enabled = false;
            btn_criteria2.Enabled = false;
            btn_criteria3.Enabled = false;
            btn_criteria4.Enabled = false;
            //   btn_criteria6.Enabled = true;
            Session["Priority"] = p;
            btn_criteria1.BackColor = Color.Tomato;
            btn_criteria2.BackColor = Color.Tomato;
            btn_criteria3.BackColor = Color.Tomato;
            btn_criteria4.BackColor = Color.Tomato;
            btn_criteria5.ForeColor = Color.White;
            btn_criteria5.BackColor = Color.Green;
        }
        catch
        {
        }
    }
    public void btn_criteria6_Click(object sender, EventArgs e)
    {
        try
        {
            string p = "6";
            string Priority = "";
            btn_criteria1.Enabled = false;
            btn_criteria2.Enabled = false;
            btn_criteria3.Enabled = false;
            btn_criteria4.Enabled = false;
            btn_criteria5.Enabled = false;
            // btn_criteria7.Enabled = true;

            Session["Priority"] = p;
            btn_criteria1.BackColor = Color.Tomato;
            btn_criteria2.BackColor = Color.Tomato;
            btn_criteria3.BackColor = Color.Tomato;
            btn_criteria4.BackColor = Color.Tomato;
            btn_criteria5.BackColor = Color.Tomato;
            btn_criteria6.ForeColor = Color.White;
            btn_criteria6.BackColor = Color.Green;
        }
        catch
        {
        }
    }
    public void btn_criteria7_Click(object sender, EventArgs e)
    {
        try
        {
            string p = "7";
            string Priority = "";
            btn_criteria1.Enabled = false;
            btn_criteria2.Enabled = false;
            btn_criteria3.Enabled = false;
            btn_criteria4.Enabled = false;
            btn_criteria5.Enabled = false;
            btn_criteria6.Enabled = false;
            //  btn_criteria8.Enabled = true;
            Session["Priority"] = p;
            btn_criteria1.BackColor = Color.Tomato;
            btn_criteria2.BackColor = Color.Tomato;
            btn_criteria3.BackColor = Color.Tomato;
            btn_criteria4.BackColor = Color.Tomato;
            btn_criteria5.BackColor = Color.Tomato;
            btn_criteria6.BackColor = Color.Tomato;
            btn_criteria7.ForeColor = Color.White;
            btn_criteria7.BackColor = Color.Green;
        }
        catch
        {
        }
    }
    public void btn_criteria8_Click(object sender, EventArgs e)
    {
        try
        {
            string p = "8";
            string Priority = "";
            btn_criteria1.Enabled = false;
            btn_criteria2.Enabled = false;
            btn_criteria3.Enabled = false;
            btn_criteria4.Enabled = false;
            btn_criteria5.Enabled = false;
            btn_criteria6.Enabled = false;
            btn_criteria7.Enabled = false;
            //   btn_criteria9.Enabled = true;
            Session["Priority"] = p;
            btn_criteria1.BackColor = Color.Tomato;
            btn_criteria2.BackColor = Color.Tomato;
            btn_criteria3.BackColor = Color.Tomato;
            btn_criteria4.BackColor = Color.Tomato;
            btn_criteria5.BackColor = Color.Tomato;
            btn_criteria6.BackColor = Color.Tomato;
            btn_criteria7.BackColor = Color.Tomato;
            btn_criteria8.ForeColor = Color.White;
            btn_criteria8.BackColor = Color.Green;
        }
        catch
        {
        }
    }
    public void btn_criteria9_Click(object sender, EventArgs e)
    {
        try
        {
            string p = "9";
            string Priority = "";
            btn_criteria1.Enabled = false;
            btn_criteria2.Enabled = false;
            btn_criteria3.Enabled = false;
            btn_criteria4.Enabled = false;
            btn_criteria5.Enabled = false;
            btn_criteria6.Enabled = false;
            btn_criteria7.Enabled = false;
            btn_criteria8.Enabled = false;

            Session["Priority"] = p;
            btn_criteria1.BackColor = Color.Tomato;
            btn_criteria2.BackColor = Color.Tomato;
            btn_criteria3.BackColor = Color.Tomato;
            btn_criteria4.BackColor = Color.Tomato;
            btn_criteria5.BackColor = Color.Tomato;
            btn_criteria6.BackColor = Color.Tomato;
            btn_criteria7.BackColor = Color.Tomato;
            btn_criteria8.BackColor = Color.Tomato;
            btn_criteria9.ForeColor = Color.White;
            btn_criteria9.BackColor = Color.Green;
        }
        catch
        {
        }
    }
    public string getapp(string app)
    {
        string appno = d2.GetFunction("select ReqStaffAppNo from RQ_RequestHierarchy");
        return appno;

    }
    public void rdo_gatepass_staff_CheckedChange(object sender, EventArgs e)
    {
        if (rdbreqstu.Checked == true)
        {
            divStaffDept.Visible = true;
            divStaffDets.Visible = false;
        }
        else
        {
            divStaffDept.Visible = false;
            divStaffDets.Visible = true;
        }
        btnMainGogatepass.Visible = false;
        btnMainGo.Visible = true;
        staffinfo();
        bindspread1();
        UpdatePanel1.Visible = true;
        lblstafftype_new.Visible = true;
        UpdatePanel2.Visible = true;
        lblstaff.Visible = true;

    }
    public void rdo_gatepass_dept_CheckedChange(object sender, EventArgs e)
    {
        if (rdbreqstu.Checked == true)
        {
            divStaffDept.Visible = true;
            divStaffDets.Visible = false;
        }
        else
        {
            divStaffDept.Visible = false;
            divStaffDets.Visible = true;
        }
        btnMainGogatepass.Visible = true;
        btnMainGo.Visible = false;
        bindspd_gatepass();
        bindspread_gatepass_dept();
        UpdatePanel1.Visible = false;
        lblstafftype_new.Visible = false;
        UpdatePanel2.Visible = false;
        lblstaff.Visible = false;

    }
    public void bindspd_gatepass()
    {

        string inwardval = Convert.ToString(ddl_reqname.SelectedItem.Value);
        FpSpread2.Sheets[0].PageSize = 12;
        FpSpread2.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
        FpSpread2.Pager.Mode = FarPoint.Web.Spread.PagerMode.NextPrev;
        FpSpread2.Pager.Align = HorizontalAlign.Right;

        FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
        darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle2.ForeColor = Color.Black;
        darkstyle2.HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
        FpSpread2.Sheets[0].AutoPostBack = false;
        FpSpread2.Pager.PageCount = 5;
        FpSpread2.ActiveSheetView.SheetCorner.DefaultStyle.Font.Bold = false;
        FpSpread2.ActiveSheetView.DefaultRowHeight = 25;
        FpSpread2.ActiveSheetView.Rows.Default.Font.Name = "Book Antiqua";
        FpSpread2.ActiveSheetView.Rows.Default.Font.Size = FontUnit.Small;
        FpSpread2.ActiveSheetView.Rows.Default.Font.Bold = false;
        FpSpread2.ActiveSheetView.Columns.Default.Font.Bold = false;
        FpSpread2.ActiveSheetView.Columns.Default.Font.Size = FontUnit.Small;
        FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
        FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        FpSpread2.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        FpSpread2.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        FpSpread2.Sheets[0].ColumnCount = 3;
        FpSpread2.Sheets[0].RowCount = 0;
        FarPoint.Web.Spread.CheckBoxCellType cbxd = new FarPoint.Web.Spread.CheckBoxCellType();
        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
        cbxd.AutoPostBack = false;
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
        if (inwardval == "6")
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Department";
        if (inwardval == "14")
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Hostel";

        FpSpread2.Sheets[0].Columns[1].CellType = cbxd;
        FpSpread2.Sheets[0].Columns[0].CellType = txt;
        FpSpread2.Sheets[0].Columns[2].CellType = txt;

        FpSpread2.Sheets[0].Columns[0].Width = 50;
        //FpSpread2.Sheets[0].Columns[0].Locked = true;
        //FpSpread2.Sheets[0].Columns[1].Locked = true;
        FpSpread2.Sheets[0].Columns[1].Width = 50;
        FpSpread2.Sheets[0].Columns[2].Width = 400;

        FpSpread2.Sheets[0].Columns[0].Locked = true;
        FpSpread2.Sheets[0].Columns[1].Locked = false;
        FpSpread2.Sheets[0].Columns[2].Locked = true;
        FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Width = 509;


        FpSpread2.CommandBar.Visible = false;
        FpSpread2.Sheets[0].RowHeader.Visible = false;
    }
    public void bindspread_gatepass_dept()
    {

        string inwardval = Convert.ToString(ddl_reqname.SelectedItem.Value);
        string typ = string.Empty;
        //dept...Bind
        for (int cnt1 = 0; cnt1 < chldeptstaff.Items.Count; cnt1++)
        {
            if (chldeptstaff.Items[cnt1].Selected == true)
            {
                if (dept_all == "")
                {
                    dept_all = chldeptstaff.Items[cnt1].Value;
                }
                else
                {
                    dept_all = dept_all + "','" + chldeptstaff.Items[cnt1].Value;
                }
            }
        }
        if (cbl_degree.Items.Count > 0)
        {
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (typ == "")
                    {
                        typ = "" + cbl_degree.Items[i].Value + "";
                    }
                    else
                    {
                        typ = typ + "'" + "," + "'" + cbl_degree.Items[i].Value + "";
                    }
                }

            }
        }

        string sqlcmd = "";
        string deptcode1 = "";
        FpSpread2.SaveChanges();

        if (inwardval == "6")
        {
            if (rdbreqstu.Checked == false)
                sqlcmd = "select distinct h.dept_name,h.dept_code from staff_appl_master a ,staffmaster s,hrdept_master h,desig_master d,stafftrans st where a.appl_no =s.appl_no and s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode and h.dept_code in ('" + dept_all + "')  and s.college_code='" + ddlcollegestaff.SelectedValue.ToString() + "' and resign = 0 and settled = 0 and latestrec=1";
            else
            {

                string qurys = string.Empty;

                if (!string.IsNullOrEmpty(group_user) && group_user.Trim() != "0")
                {
                    if (group_user.Contains(';'))
                    {
                        string[] group_semi = group_user.Split(';');
                        group_user = group_semi[0].ToString();
                    }
                    qurys = " and group_code='" + group_user + "'";
                    sqlcmd = "  select distinct degree.degree_code as dept_code,department.dept_name,degree.Acronym from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + typ + "') and degree.college_code=@college_code  and deptprivilages.Degree_code=degree.Degree_code " + qurys + "";
                }
                else if (!string.IsNullOrEmpty(usercode) && usercode.Trim() != "0")
                {
                    qurys = " and user_code='" + usercode + "'";//magesh 11.1.18
                    sqlcmd = "  select distinct degree.degree_code as dept_code,department.dept_name,degree.Acronym from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + typ + "') and degree.college_code='" + ddlclg.SelectedValue + "'  and deptprivilages.Degree_code=degree.Degree_code  " + qurys + "  ";
                }
            }

            sqlcmd = sqlcmd + " select ReqStaffAppNo,RequestType,DegreeCode from RQ_RequestHierarchy";
            dsload = dset.select_method_wo_parameter(sqlcmd, "Text");
        }
        if (inwardval == "14")
        {
            string hostel = rs.getCblSelectedValue(cbl_hostelname);
            if (hostel != "")
            {
                sqlcmd = "select HostelMasterPK,HostelName from HM_HostelMaster where  HostelMasterPK in ('" + hostel + "') order by hostelname";
                sqlcmd = sqlcmd + " select ReqStaffAppNo,RequestType,DegreeCode from RQ_RequestHierarchy";
                dsload = dset.select_method_wo_parameter(sqlcmd, "Text");
            }

        }
        int sno = 1;
        DataView dv = new DataView();
        if (dsload.Tables[0].Rows.Count > 0)
        {
            if (inwardval == "6")
            {

                for (int loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                {

                    FpSpread2.Sheets[0].RowCount++;

                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Value = 0;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["dept_name"].ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag = dsload.Tables[0].Rows[loop]["dept_code"].ToString();

                    deptcode1 = Convert.ToString(FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag);
                    if (rdbreqstu.Checked == true)
                        dsload.Tables[1].DefaultView.RowFilter = "DegreeCode='" + deptcode1 + "' and RequestType='" + ddl_reqname.SelectedItem.Value + "'";
                    else
                        dsload.Tables[1].DefaultView.RowFilter = "ReqStaffAppNo='" + deptcode1 + "' and RequestType='" + ddl_reqname.SelectedItem.Value + "'";
                    dv = dsload.Tables[1].DefaultView;

                    if (dv.Count > 0)
                    {

                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#F0A3CC");
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Value = 1;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Locked = true;
                        FpSpread2.Enabled = true;
                        btnreset.Enabled = true;
                        btnview.Enabled = true;
                    }
                    else
                    {
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Value = 0;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Locked = false;
                    }

                    sno++;
                }


            }
            if (inwardval == "14")
            {

                for (int loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                {

                    FpSpread2.Sheets[0].RowCount++;

                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Value = 0;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["HostelName"].ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag = dsload.Tables[0].Rows[loop]["HostelMasterPK"].ToString();

                    deptcode1 = Convert.ToString(FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag);
                    dsload.Tables[1].DefaultView.RowFilter = "DegreeCode='" + deptcode1 + "' and RequestType='" + ddl_reqname.SelectedItem.Value + "'";
                    dv = dsload.Tables[1].DefaultView;

                    if (dv.Count > 0)
                    {

                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#F0A3CC");
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Value = 1;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Locked = true;
                        FpSpread2.Enabled = true;
                        btnreset.Enabled = true;
                        btnview.Enabled = true;
                    }
                    else
                    {
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Value = 0;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Locked = false;
                    }

                    sno++;
                }
            }
            FpSpread2.SaveChanges();
            FpSpread2.Visible = true;

        }


        FpSpread2.Sheets[0].PageSize = 12;
        FpSpread2.TitleInfo.Height = 30;
        if (FpSpread2.Sheets[0].RowCount > 10)
        {
            FpSpread2.Height = 390;
        }
        else
        {
            FpSpread2.Height = (FpSpread2.Sheets[0].RowCount * 25) + 140;
        }
        FpSpread2.Height = 390;
        //  }
    }
    public void btnMainGogatepass_Click(object sender, EventArgs e)
    {
        bindspd_gatepass();
        bindspread_gatepass_dept();
    }
    public void rdbreqstu_CheckedChange(object sender, EventArgs e)
    {
        if (rdbreqstu.Checked == true)
        {
            divStaffDept.Visible = true;
            divStaffDets.Visible = false;
            loadcolleges();
            bindbatchs();
            binddegrees();
            bindbatchs();
            btngostu_Click(sender, e);
            Chktutor.Visible = true;
            Txttutor.Visible = true;
        }
        else
        {
            divStaffDept.Visible = false;
            divStaffDets.Visible = true;
        }

    }
    public void gatepassrights()
    {
        try
        {
            string query = "";
            string Master1 = "";
            string stud = "";
            string values = "";
            string sms = "";
            string sms1 = "";
            string sms2 = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {

                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
                query = "select * from Master_Settings where settings ='Request Gatepass Rights' and group_code ='" + Master1 + "'";
            }
            else
            {
                Master1 = Session["usercode"].ToString();
                query = "select * from Master_Settings where settings ='Request Gatepass Rights' and usercode ='" + Master1 + "'";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = Convert.ToString(ds.Tables[0].Rows[i]["value"]);
                    if (val == "1")
                    {
                        gatepass_staffdept = "1";
                    }
                    else if (val == "2")
                    {
                        gatepass_staffdept = "2";
                    }
                    else
                    {
                        gatepass_staffdept = "";
                    }

                }
            }

        }
        catch
        {
        }
    }
    public void rdo_req_stud_CheckedChanged(object sender, EventArgs e)
    {
        tbl_stud.Visible = true;
        gatepassrights();
        if (gatepass_staffdept == "1")
        {
            rdo_gatepass_staff.Visible = true;
            rdo_gatepass_dept.Visible = false;
        }
        else if (gatepass_staffdept == "2")
        {
            rdo_gatepass_staff.Visible = false;
            rdo_gatepass_dept.Visible = true;
            rdo_gatepass_dept.Checked = true;
            rdo_gatepass_staff.Checked = false;
            btnMainGogatepass.Visible = true;
            btnMainGo.Visible = false;
            bindspd_gatepass();
            bindspread_gatepass_dept();
            UpdatePanel1.Visible = false;
            lblstafftype_new.Visible = false;
            UpdatePanel2.Visible = false;
            lblstaff.Visible = false;
            btnMainGogatepass_Click(sender, e);

        }
        else
        {
            rdo_gatepass_staff.Visible = true;
            rdo_gatepass_dept.Visible = true;
        }
    }
    public void rdo_staff_req_CheckedChanged(object sender, EventArgs e)
    {
        tbl_stud.Visible = false;
        UpdatePanel1.Visible = true;
        lblstafftype_new.Visible = true;
        UpdatePanel2.Visible = true;
        lblstaff.Visible = true;
        staffinfo();
        staffinfo1();
        bindspread1();
        bindspread2();
    }

    public void btnerrclose1_Click(object sender, EventArgs e)
    {
        alert_div.Visible = false;
    }
    //Added by Idhris -- 07-11-2016

    protected void ddlStaffType_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnMainGo1_Click(sender, e);
    }
    protected void ddl_reqname_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlStaffType.Visible = false;
        rblStaffStudent.SelectedIndex = 0;
        ddlcollegestaff_SelectedIndexChanged(sender, e);
        lblBr.Visible = false;
        Up_dept.Visible = false;
        divStaffDept.Visible = false;
        divStaffDets.Visible = false;

        if (rblStaffStudent.SelectedIndex == 0)
        {
            divStaffDets.Visible = true;
            if (Convert.ToString(ddl_reqname.SelectedItem.Value) == "10")
            {
                loadcollege();
                //BindDepartment();
                BindBatch();
                binddegree();
                bindbranch();
            }
            else if (Convert.ToString(ddl_reqname.SelectedItem.Value) != "10")
            {
                loadcollege();
                BindDepartment();
                btngo_Click(sender, e);
            }


        }
        rdbreqstu.Visible = false;
        div_hostel.Visible = false;
        tbl_stud.Visible = false;
        rdo_req_stud.Visible = false;
        rdo_staff_req.Visible = false;
        rdbreqstu.Visible = false;
        Chktutor.Visible = false;
        Txttutor.Visible = false;
        lbl_hostel.Visible = false;
        Upp2hostel.Visible = false;
        div_gatepass.Visible = false;
        Lbel7.Visible = false;
        UpdatePanel7.Visible = false;
        Label7.Visible = false;
        UpdatePanel8.Visible = false;
        Label6.Visible = false;
        UpdatePanel6.Visible = false;
        Label3.Visible = false;
        ddlclg.Visible = false;
        string inwardval = Convert.ToString(ddl_reqname.SelectedItem.Value);//delsi
        if (inwardval.Trim() == "12")
        {
            lblstafftype_new.Visible = false; // poo 09.12.17
            lblstafftype.Visible = false;
            UpdatePanel1.Visible = false;
            UpdatePanel2.Visible = false;
            lblstaff.Visible = false;
            lbldept.Visible = false;
            bindspd_gatepass(); // poo
            bindspread_gatepass_dept(); //poo

        }
        else // poo 09.12.17
        {
            lblstafftype_new.Visible = true;
            lblstafftype.Visible = true;
            UpdatePanel1.Visible = true;
            UpdatePanel2.Visible = true;
            lblstaff.Visible = true;
            lbldept.Visible = true;
            btnMainGo_Click(sender, e);
        }
        if (inwardval.Trim() == "10")
        {
            ddlStaffType.Visible = true;
        }
        if (inwardval == "14")
        {

            div_hostel.Visible = true;
            hostevat.Visible = true;
            rdb_hostelstd_CheckedChange(sender, e);
        }
        if (inwardval == "6")
        {

            div_gatepass.Visible = true;
            rdbreqstu.Visible = true;
            rdo_req_stud.Visible = true;
            rdo_staff_req.Visible = true;
            tbl_stud.Visible = true;
            Chktutor.Visible = true;
            Txttutor.Visible = true;


        }
        else
        {
            rdbreqstu.Visible = false;
            rdbreqstu.Checked = false;
            btnMainGogatepass.Visible = false;
            btnMainGo.Visible = true;
        }

    }

    protected void rblStaffStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlcollegestaff_SelectedIndexChanged(sender, e);
            if (rblStaffStudent.SelectedIndex == 0)
            {
                loadcollege();
                BindDepartment();
                btngo_Click(sender, e);
            }
        }
        catch { }
    }

    public void loadcollege()
    {
        try
        {
            ddlclgStud.Items.Clear();
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlclgStud.DataSource = ds;
                ddlclgStud.DataTextField = "collname";
                ddlclgStud.DataValueField = "college_code";
                ddlclgStud.DataBind();
            }
        }
        catch
        { }
    }
    public void ddlclgStud_SelectedIndexChanged(object sender, EventArgs e)
    {

        BindDepartment();
    }
    protected void cb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_dept, cbl_dept, txt_dept, lblBr.Text, "--Select--");
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, lblBr.Text, "--Select--");
    }
    private void BindDepartment()
    {
        try
        {
            cbl_dept.Items.Clear();
            string collegeCode = ddlclgStud.Items.Count > 0 ? ddlclgStud.SelectedValue : "13";
            string query = "select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.college_code in (" + collegeCode + ")  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + " --  and degree.course_id in(degree) ";
            DataSet dsBranch = d2.select_method_wo_parameter(query, "Text");
            if (dsBranch.Tables.Count > 0 && dsBranch.Tables[0].Rows.Count > 0)
            {
                cbl_dept.DataSource = dsBranch;
                cbl_dept.DataTextField = "dept_name";
                cbl_dept.DataValueField = "degree_code";
                cbl_dept.DataBind();
                cb_dept.Checked = true;
            }
            else
            {
                cb_dept.Checked = false;
            }

        }
        catch { cb_dept.Checked = false; }
        CallCheckboxChange(cb_dept, cbl_dept, txt_dept, lblBr.Text, "--Select--");
    }
    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("," + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }
    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = "";
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }
    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = "";
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch { }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        lblNoRec.Visible = false;
        ds.Clear();
        //ds = loadDataset();
        //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //{
        loadSpread();
        //}
        //else
        //{
        //    fpreport.Visible = false;
        //    lblNoRec.Visible = true;
        //}
    }

    protected DataSet loadDataset()
    {
        DataSet dsload = new DataSet();
        try
        {
            string clgcode = ddlclgStud.Items.Count > 0 ? ddlclgStud.SelectedValue : "13";
            string degcode = Convert.ToString(getCblSelectedValue(cbl_dept));

            string SelQ = " SELECT SLSettingPK, DegreeCode, IsFinance, HeaderFK, LegerFK, MaxLeave, CollegeCode, FromDay ,ToDay ,Amount FROM AM_Student_Leave_Settings S,AM_Student_Leave_Settings_Det SD WHERE SLSettingPK = SLSettingFK AND CollegeCode ='" + clgcode + "' AND DegreeCode  in (" + degcode + ") ORDER BY DegreeCode ASC";
            dsload.Clear();

            dsload = d2.select_method_wo_parameter(SelQ, "Text");
        }
        catch { }
        return dsload;
    }

    //protected void loadSpread()
    //{
    //    try
    //    {
    //        #region design
    //        fpreport.Sheets[0].RowCount = 0;
    //        fpreport.Sheets[0].ColumnCount = 0;
    //        fpreport.CommandBar.Visible = false;
    //        fpreport.Sheets[0].AutoPostBack = false;
    //        fpreport.Sheets[0].ColumnHeader.RowCount = 1;
    //        fpreport.Sheets[0].RowHeader.Visible = false;
    //        fpreport.Sheets[0].ColumnCount = 9;
    //        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
    //        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
    //        darkstyle.ForeColor = Color.White;
    //        fpreport.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
    //        fpreport.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
    //        fpreport.Sheets[0].Columns[0].Locked = true;
    //        fpreport.Sheets[0].Columns[0].Width = 50;

    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 1].Text = lblclg.Text;
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
    //        fpreport.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
    //        fpreport.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
    //        fpreport.Sheets[0].Columns[1].Visible = false;

    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 2].Text = lblBr.Text;
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
    //        fpreport.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
    //        fpreport.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
    //        //fpreport.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
    //        fpreport.Sheets[0].Columns[2].Width = 450;

    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Maximum Leave";
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
    //        fpreport.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
    //        fpreport.Sheets[0].Columns[3].Visible = false;

    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 4].Text = "From (Days)";
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
    //        fpreport.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
    //        fpreport.Sheets[0].Columns[4].Width = 150;

    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 5].Text = "To (Days)";
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
    //        fpreport.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
    //        fpreport.Sheets[0].Columns[5].Width = 150;

    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Fine Amount";
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
    //        fpreport.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
    //        fpreport.Sheets[0].Columns[6].Visible = false;

    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Finance";
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
    //        fpreport.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
    //        fpreport.Sheets[0].Columns[7].Visible = false;

    //        //FarPoint.Web.Spread.CheckBoxCellType cbSelAll = new FarPoint.Web.Spread.CheckBoxCellType();
    //        //cbSelAll.AutoPostBack = true;
    //        //fpreport.Sheets[0].ColumnHeader.Cells[0, 8].CellType = cbSelAll;
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Select";
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
    //        fpreport.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
    //        fpreport.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;

    //        #endregion

    //        #region value
    //        string sqlcmd = " select ReqStaffAppNo,RequestType,FromDays,ToDays from RQ_RequestHierarchy";
    //        DataSet dsloadN = dset.select_method_wo_parameter(sqlcmd, "Text");
    //        FarPoint.Web.Spread.CheckBoxCellType cbSel = new FarPoint.Web.Spread.CheckBoxCellType();
    //        for (int sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
    //        {
    //            fpreport.Sheets[0].RowCount++;
    //            fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sel + 1);
    //            string collegeCode = Convert.ToString(ds.Tables[0].Rows[sel]["collegecode"]);
    //            string collegeName = ddlclgStud.Items.Count > 0 ? ddlclgStud.Items[ddlclgStud.Items.IndexOf(ddlclgStud.Items.FindByValue(collegeCode))].Text : string.Empty;

    //            string degCode = Convert.ToString(ds.Tables[0].Rows[sel]["DEGREECODE"]).Trim();
    //            string deptName = cbl_dept.Items.Count > 0 ? cbl_dept.Items[cbl_dept.Items.IndexOf(cbl_dept.Items.FindByValue(degCode))].Text : string.Empty;

    //            string SLSettingPK = Convert.ToString(ds.Tables[0].Rows[sel]["SLSettingPK"]).Trim().ToUpper();

    //            string isFin = Convert.ToString(ds.Tables[0].Rows[sel]["IsFinance"]).Trim().ToUpper();
    //            byte isFinVal = 0;
    //            string headerFk = "0";
    //            string ledgerFK = "0";
    //            if (isFin == "1" || isFin == "TRUE")
    //            {
    //                isFin = "Included";
    //                isFinVal = 1;

    //                headerFk = Convert.ToString(ds.Tables[0].Rows[sel]["HeaderFK"]).Trim();
    //                ledgerFK = Convert.ToString(ds.Tables[0].Rows[sel]["LegerFK"]).Trim();
    //            }
    //            else
    //            {
    //                isFin = "Not Included";
    //            }

    //            fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 1].Text = collegeName;
    //            fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 2].Text = deptName;
    //            fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[sel]["MaxLeave"]);
    //            fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[sel]["FromDay"]);
    //            fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[sel]["ToDay"]);
    //            fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[sel]["Amount"]);
    //            fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 7].Text = isFin;

    //            fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 8].CellType = cbSel;

    //            fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 2].Tag = collegeCode;
    //            fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 6].Tag = degCode;
    //            fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 3].Tag = headerFk;
    //            fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 4].Tag = ledgerFK;
    //            fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 5].Tag = SLSettingPK;
    //            fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 7].Tag = isFinVal;

    //            if (dsloadN.Tables.Count > 0)
    //            {
    //                dsloadN.Tables[0].DefaultView.RowFilter = "ReqStaffAppNo='" + degCode + "' and RequestType='" + ddl_reqname.SelectedItem.Value + "' and FromDays='" + Convert.ToString(ds.Tables[0].Rows[sel]["FromDay"]) + "' and ToDays='" + Convert.ToString(ds.Tables[0].Rows[sel]["ToDay"]) + "'";
    //                DataView dv = dsloadN.Tables[0].DefaultView;

    //                if (dv.Count > 0)
    //                {
    //                    fpreport.Sheets[0].Rows[fpreport.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#F0A3CC");
    //                    fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 8].Value = 1;
    //                    fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 8].Locked = true;
    //                }
    //            }
    //        }

    //        fpreport.Sheets[0].PageSize = fpreport.Sheets[0].RowCount;
    //        fpreport.Height = 350;
    //        fpreport.SaveChanges();
    //        divspread.Visible = true;
    //        fpreport.Visible = true;

    //        #endregion

    //    }
    //    catch { }
    //    if (fpreport.Rows.Count > 0)
    //    {
    //        lblNoRec.Visible = false;
    //    }
    //    else
    //    {
    //        lblNoRec.Visible = true;
    //    }
    //}

    protected void loadSpread()
    {


        try
        {
            if (Convert.ToString(ddl_reqname.SelectedItem.Value) == "10")
            {
                #region design
                fpreport.Sheets[0].RowCount = 0;
                fpreport.Sheets[0].ColumnCount = 0;
                fpreport.CommandBar.Visible = false;
                fpreport.Sheets[0].AutoPostBack = false;
                fpreport.Sheets[0].ColumnHeader.RowCount = 1;
                fpreport.Sheets[0].RowHeader.Visible = false;
                fpreport.Sheets[0].ColumnCount = 7;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                fpreport.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                fpreport.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                fpreport.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                fpreport.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                fpreport.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                fpreport.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                fpreport.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                fpreport.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                fpreport.Sheets[0].Columns[0].Locked = true;
                fpreport.Sheets[0].Columns[0].Width = 50;

                fpreport.Sheets[0].ColumnHeader.Cells[0, 1].Text = lblclg.Text;
                fpreport.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                fpreport.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                fpreport.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                fpreport.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                fpreport.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                fpreport.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                fpreport.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                fpreport.Sheets[0].Columns[1].Visible = false;


                fpreport.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Batch";
                fpreport.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
                fpreport.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                fpreport.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                fpreport.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                fpreport.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                fpreport.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                fpreport.Sheets[0].Columns[2].Locked = true;
                fpreport.Sheets[0].Columns[2].Width = 80;


                fpreport.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Branch";
                fpreport.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
                fpreport.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                fpreport.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                fpreport.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                fpreport.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Left;
                fpreport.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                fpreport.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
                fpreport.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                fpreport.Sheets[0].Columns[3].Width = 450;

                fpreport.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Semester";
                fpreport.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
                fpreport.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                fpreport.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                fpreport.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                fpreport.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                fpreport.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                fpreport.Sheets[0].Columns[4].Locked = true;
                fpreport.Sheets[0].Columns[4].Width = 80;


                fpreport.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Sections";
                fpreport.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
                fpreport.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                fpreport.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                fpreport.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                fpreport.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                fpreport.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                fpreport.Sheets[0].Columns[5].Locked = true;
                fpreport.Sheets[0].Columns[5].Width = 80;

                fpreport.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Select";
                fpreport.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
                fpreport.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                fpreport.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                fpreport.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                fpreport.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                fpreport.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;

                #endregion

                #region value
                string sqlcmd = " select ReqStaffAppNo,RequestType,FromDays,ToDays,BatchYear,isnull(Section,'') as Section,Semester from RQ_RequestHierarchy";
                DataSet dsloadN = dset.select_method_wo_parameter(sqlcmd, "Text");
                FarPoint.Web.Spread.CheckBoxCellType cbSel = new FarPoint.Web.Spread.CheckBoxCellType();
                ds.Clear();
                string valBatch = string.Empty;
                string valDegree = string.Empty;
                string valBranch = string.Empty;
                string collegeCode = string.Empty;
                if (ddlclgStud.Items.Count > 0)
                {
                    collegeCode = ddlclgStud.SelectedValue.ToString().Trim();
                }
                else
                {
                    lbl_alertt.Visible = true;
                    lbl_alertt.Text = "No " + lblclg.Text + " Found";
                    alert_div.Visible = true;
                    return;
                }

                if (cblBatch.Items.Count == 0)
                {
                    lbl_alertt.Visible = true;
                    lbl_alertt.Text = "No " + lblBatch.Text + " Found";
                    alert_div.Visible = true;
                    return;
                }
                else
                {
                    valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
                    if (string.IsNullOrEmpty(valBatch))
                    {
                        lbl_alertt.Visible = true;
                        lbl_alertt.Text = "Select Atleast One " + lblBatch.Text + "";
                        alert_div.Visible = true;
                        return;
                    }
                }
                if (cblDegree.Items.Count == 0)
                {
                    lbl_alertt.Visible = true;
                    lbl_alertt.Text = "No " + lblDegree.Text + " Found";
                    alert_div.Visible = true;
                    return;
                }

                else
                {
                    valDegree = rs.GetSelectedItemsValueAsString(cblDegree);
                    if (string.IsNullOrEmpty(valDegree))
                    {
                        lbl_alertt.Visible = true;
                        lbl_alertt.Text = "Select Atleast One " + lblDegree.Text + "";
                        alert_div.Visible = true;
                        return;
                    }
                }
                if (cblBranch.Items.Count == 0)
                {
                    lbl_alertt.Visible = true;
                    lbl_alertt.Text = "No " + lblBranch.Text + " Found";
                    alert_div.Visible = true;
                    return;
                }
                else
                {
                    valBranch = rs.GetSelectedItemsValueAsString(cblBranch);
                    if (string.IsNullOrEmpty(valBranch))
                    {
                        lbl_alertt.Visible = true;
                        lbl_alertt.Text = "Select Atleast One " + lblBranch.Text + "";
                        alert_div.Visible = true;
                        return;
                    }
                }
                //string SelectQ = "select distinct d.Degree_Code,de.Dept_Name,l.collegeCode  from Degree d,Department de,course c,AttMasterSetting a,leaveMaster l  where c.Course_Id=d.Course_Id and d.Dept_Code=de.Dept_Code and  a.CollegeCode=l.collegeCode and a.EntryCode=l.EntryCode and ltrim(rtrim(isnull(Maxval,'')))!='' and  c.Edu_Level=l.eduLevel and c.college_code=l.collegeCode and c.college_code in(13)";

                string SelectQ = "select distinct r.college_code,r.Batch_Year,r.degree_code,r.Current_Semester,(c.Course_Name+'-'+de.Dept_Name) as deptName,r.Sections from Registration r,Degree d,Department de,course c where r.college_code=d.college_code and c.college_code=r.college_code and d.Degree_Code=r.degree_code and d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id and r.Batch_Year in('" + valBatch + "') and r.degree_code in('" + valBranch + "') and r.college_code in('" + collegeCode + "') and CC=0 and DelFlag<>1 order by r.college_code,r.degree_code,r.Batch_Year,r.Current_Semester,(c.Course_Name+'-'+de.Dept_Name),r.Sections";
                ds = d2.select_method_wo_parameter(SelectQ, "text");
                int i = 0;
                for (int sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
                {
                    i++;
                    fpreport.Sheets[0].RowCount++;
                    fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sel + 1);
                    string collegeCode1 = Convert.ToString(ds.Tables[0].Rows[sel]["college_code"]);
                    string collegeName = ddlclgStud.Items.Count > 0 ? ddlclgStud.Items[ddlclgStud.Items.IndexOf(ddlclgStud.Items.FindByValue(collegeCode))].Text : string.Empty;

                    string degCode = Convert.ToString(ds.Tables[0].Rows[sel]["degree_code"]).Trim();
                    //string deptName = cbl_dept.Items.Count > 0 ? cbl_dept.Items[cbl_dept.Items.IndexOf(cbl_dept.Items.FindByValue(degCode))].Text : string.Empty;
                    string deptName = Convert.ToString(ds.Tables[0].Rows[sel]["deptName"]).Trim();
                    string batchC = Convert.ToString(ds.Tables[0].Rows[sel]["Batch_Year"]).Trim();
                    string SemC = Convert.ToString(ds.Tables[0].Rows[sel]["Current_Semester"]).Trim();
                    string secC = Convert.ToString(ds.Tables[0].Rows[sel]["Sections"]).Trim();

                    fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 1].Text = collegeName;
                    fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 2].Text = batchC;
                    fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 3].Text = deptName;
                    fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 4].Text = SemC;
                    fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 5].Text = secC;
                    fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 6].CellType = cbSel;

                    fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 1].Tag = collegeCode;
                    fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 2].Tag = batchC;
                    fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 3].Tag = degCode;
                    fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 4].Tag = SemC;
                    fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 5].Tag = secC;

                    string qrySec = string.Empty;
                    qrySec = "  and isnull(Section,'')='" + secC + "'";
                    if (dsloadN.Tables.Count > 0)
                    {
                        dsloadN.Tables[0].DefaultView.RowFilter = "ReqStaffAppNo='" + degCode + "' and RequestType='" + ddl_reqname.SelectedItem.Value + "' and BatchYear='" + batchC + "' and Semester='" + SemC + "'" + qrySec;//and FromDays='" + Convert.ToString(ds.Tables[0].Rows[sel]["FromDay"]) + "' and ToDays='" + Convert.ToString(ds.Tables[0].Rows[sel]["ToDay"]) + "'
                        DataView dv = dsloadN.Tables[0].DefaultView;
                        if (dv.Count > 0)
                        {
                            fpreport.Sheets[0].Rows[fpreport.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#F0A3CC");
                            fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 6].Value = 1;
                            fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 6].Locked = false;
                        }
                    }
                }

                fpreport.Sheets[0].PageSize = fpreport.Sheets[0].RowCount;
                fpreport.Height = 350;
                fpreport.SaveChanges();
                divspread.Visible = true;
                fpreport.Visible = true;

                #endregion
            }
            else
            {
                try
                {
                    #region design
                    fpreport.Sheets[0].RowCount = 0;
                    fpreport.Sheets[0].ColumnCount = 0;
                    fpreport.CommandBar.Visible = false;
                    fpreport.Sheets[0].AutoPostBack = false;
                    fpreport.Sheets[0].ColumnHeader.RowCount = 1;
                    fpreport.Sheets[0].RowHeader.Visible = false;
                    fpreport.Sheets[0].ColumnCount = 9;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    fpreport.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    fpreport.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpreport.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    fpreport.Sheets[0].Columns[0].Locked = true;
                    fpreport.Sheets[0].Columns[0].Width = 50;

                    fpreport.Sheets[0].ColumnHeader.Cells[0, 1].Text = lblclg.Text;
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    fpreport.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                    fpreport.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    fpreport.Sheets[0].Columns[1].Visible = false;

                    fpreport.Sheets[0].ColumnHeader.Cells[0, 2].Text = lblBr.Text;
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    fpreport.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                    fpreport.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                    //fpreport.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    fpreport.Sheets[0].Columns[2].Width = 450;

                    fpreport.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Maximum Leave";
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    fpreport.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                    fpreport.Sheets[0].Columns[3].Visible = false;

                    fpreport.Sheets[0].ColumnHeader.Cells[0, 4].Text = "From (Days)";
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    fpreport.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                    fpreport.Sheets[0].Columns[4].Width = 150;

                    fpreport.Sheets[0].ColumnHeader.Cells[0, 5].Text = "To (Days)";
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    fpreport.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                    fpreport.Sheets[0].Columns[5].Width = 150;

                    fpreport.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Fine Amount";
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    fpreport.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                    fpreport.Sheets[0].Columns[6].Visible = false;

                    fpreport.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Finance";
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                    fpreport.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
                    fpreport.Sheets[0].Columns[7].Visible = false;

                    //FarPoint.Web.Spread.CheckBoxCellType cbSelAll = new FarPoint.Web.Spread.CheckBoxCellType();
                    //cbSelAll.AutoPostBack = true;
                    //fpreport.Sheets[0].ColumnHeader.Cells[0, 8].CellType = cbSelAll;
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Select";
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                    fpreport.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                    fpreport.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;

                    #endregion

                    #region value
                    string sqlcmd = " select ReqStaffAppNo,RequestType,FromDays,ToDays from RQ_RequestHierarchy";
                    DataSet dsloadN = dset.select_method_wo_parameter(sqlcmd, "Text");
                    FarPoint.Web.Spread.CheckBoxCellType cbSel = new FarPoint.Web.Spread.CheckBoxCellType();
                    for (int sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
                    {
                        fpreport.Sheets[0].RowCount++;
                        fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sel + 1);
                        string collegeCode = Convert.ToString(ds.Tables[0].Rows[sel]["collegecode"]);
                        string collegeName = ddlclgStud.Items.Count > 0 ? ddlclgStud.Items[ddlclgStud.Items.IndexOf(ddlclgStud.Items.FindByValue(collegeCode))].Text : string.Empty;

                        string degCode = Convert.ToString(ds.Tables[0].Rows[sel]["DEGREECODE"]).Trim();
                        string deptName = cbl_dept.Items.Count > 0 ? cbl_dept.Items[cbl_dept.Items.IndexOf(cbl_dept.Items.FindByValue(degCode))].Text : string.Empty;

                        string SLSettingPK = Convert.ToString(ds.Tables[0].Rows[sel]["SLSettingPK"]).Trim().ToUpper();

                        string isFin = Convert.ToString(ds.Tables[0].Rows[sel]["IsFinance"]).Trim().ToUpper();
                        byte isFinVal = 0;
                        string headerFk = "0";
                        string ledgerFK = "0";
                        if (isFin == "1" || isFin == "TRUE")
                        {
                            isFin = "Included";
                            isFinVal = 1;

                            headerFk = Convert.ToString(ds.Tables[0].Rows[sel]["HeaderFK"]).Trim();
                            ledgerFK = Convert.ToString(ds.Tables[0].Rows[sel]["LegerFK"]).Trim();
                        }
                        else
                        {
                            isFin = "Not Included";
                        }

                        fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 1].Text = collegeName;
                        fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 2].Text = deptName;
                        fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[sel]["MaxLeave"]);
                        fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[sel]["FromDay"]);
                        fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[sel]["ToDay"]);
                        fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[sel]["Amount"]);
                        fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 7].Text = isFin;

                        fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 8].CellType = cbSel;

                        fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 2].Tag = collegeCode;
                        fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 6].Tag = degCode;
                        fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 3].Tag = headerFk;
                        fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 4].Tag = ledgerFK;
                        fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 5].Tag = SLSettingPK;
                        fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 7].Tag = isFinVal;

                        if (dsloadN.Tables.Count > 0)
                        {
                            dsloadN.Tables[0].DefaultView.RowFilter = "ReqStaffAppNo='" + degCode + "' and RequestType='" + ddl_reqname.SelectedItem.Value + "' and FromDays='" + Convert.ToString(ds.Tables[0].Rows[sel]["FromDay"]) + "' and ToDays='" + Convert.ToString(ds.Tables[0].Rows[sel]["ToDay"]) + "'";
                            DataView dv = dsloadN.Tables[0].DefaultView;

                            if (dv.Count > 0)
                            {
                                fpreport.Sheets[0].Rows[fpreport.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#F0A3CC");
                                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 8].Value = 1;
                                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 8].Locked = true;
                            }
                        }
                    }

                    fpreport.Sheets[0].PageSize = fpreport.Sheets[0].RowCount;
                    fpreport.Height = 350;
                    fpreport.SaveChanges();
                    divspread.Visible = true;
                    fpreport.Visible = true;

                    #endregion

                }
                catch { }
                if (fpreport.Rows.Count > 0)
                {
                    lblNoRec.Visible = false;
                }
                else
                {
                    lblNoRec.Visible = true;
                }
            }

        }
        catch { }
        if (fpreport.Rows.Count > 0)
        {
            lblNoRec.Visible = false;
        }
        else
        {
            lblNoRec.Visible = true;
        }
    }

    protected void fpreport_OnCellClick(object sender, EventArgs e)
    {
        // cellclick = true;
    }

    protected void fpreport_Selectedindexchanged(object sender, EventArgs e)
    {

        //try
        //{
        //    if (cellclick == true)
        //    {
        //        string actrow = fpreport.ActiveSheetView.ActiveRow.ToString();
        //        string actcol = fpreport.ActiveSheetView.ActiveColumn.ToString();
        //        if (!string.IsNullOrEmpty(actrow))
        //        {
        //            int arow = Convert.ToInt32(actrow);
        //            int acol = Convert.ToInt32(actcol);

        //            string clgcode = Convert.ToString(fpreport.Sheets[0].Cells[arow, 2].Tag);
        //            string degcode = Convert.ToString(fpreport.Sheets[0].Cells[arow, 6].Tag);
        //            string isFin = Convert.ToString(fpreport.Sheets[0].Cells[arow, 7].Tag).Trim();
        //            string headerFK = Convert.ToString(fpreport.Sheets[0].Cells[arow, 3].Tag).Trim();
        //            string ledgerFK = Convert.ToString(fpreport.Sheets[0].Cells[arow, 4].Tag).Trim();

        //            if (!string.IsNullOrEmpty(degcode))
        //            {
        //                string SelQ = " SELECT SLSettingPK, DegreeCode, IsFinance, HeaderFK, LegerFK, MaxLeave, CollegeCode, FromDay ,ToDay ,Amount FROM AM_Student_Leave_Settings S,AM_Student_Leave_Settings_Det SD WHERE SLSettingPK = SLSettingFK AND CollegeCode ='" + clgcode + "' AND DegreeCode ='" + degcode + "'";
        //                SelQ += " select collname,college_code from collinfo";
        //                ds.Clear();
        //                ds = d2.select_method_wo_parameter(SelQ, "Text");
        //                #region Load Department
        //                BindDepartment2();
        //                cb_dept2.Checked = false;

        //                for (int i = 0; i < cbl_dept2.Items.Count; i++)
        //                {
        //                    if (cbl_dept2.Items[i].Value == degcode)
        //                    {
        //                        cbl_dept2.Items[i].Selected = true;
        //                        txtDept2.Text = lblDept2.Text + "(" + cbl_dept2.Items[i].Text + ")";
        //                    }
        //                    else
        //                    {
        //                        cbl_dept2.Items[i].Selected = false;
        //                    }
        //                }
        //                #endregion

        //                clearAddScreen();
        //                btnSaveLeaveSet.Visible = false;
        //                btnUpdateLeaveSet.Visible = true;
        //                btnDeleteLeaveSet.Visible = true;

        //                if (isFin == "1")
        //                {
        //                    chkIncFinance.Checked = true;
        //                    chkIncFinance_CheckedChange(new object(), new EventArgs());

        //                    ddlFinHeader.SelectedIndex = ddlFinHeader.Items.IndexOf(ddlFinHeader.Items.FindByValue(headerFK));
        //                    ddlFinLedger.SelectedIndex = ddlFinLedger.Items.IndexOf(ddlFinLedger.Items.FindByValue(ledgerFK));
        //                }

        //                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //                {
        //                    rbmode.SelectedIndex = 0;
        //                    addddlclg.Items.Clear();
        //                    if (ds.Tables[1].Rows.Count > 0)
        //                    {
        //                        DataView dv = new DataView();
        //                        ds.Tables[1].DefaultView.RowFilter = "college_code='" + clgcode + "'";
        //                        dv = ds.Tables[1].DefaultView;
        //                        if (dv.Count > 0)
        //                            addddlclg.Items.Add(new ListItem(Convert.ToString(dv[0]["collname"]), clgcode));

        //                        txtMaxLeaveSet.Text = Convert.ToString(ds.Tables[0].Rows[0]["MaxLeave"]);

        //                        DataTable dtFineGrid = new DataTable();
        //                        dtFineGrid.Columns.Add("DaysFrom");
        //                        dtFineGrid.Columns.Add("DaysTo");
        //                        dtFineGrid.Columns.Add("Amount");
        //                        for (int cnt = 0; cnt < ds.Tables[0].Rows.Count; cnt++)
        //                        {
        //                            dtFineGrid.Rows.Add(Convert.ToString(ds.Tables[0].Rows[cnt]["FromDay"]), Convert.ToString(ds.Tables[0].Rows[cnt]["ToDay"]), Convert.ToString(ds.Tables[0].Rows[cnt]["Amount"]));
        //                        }
        //                        BindGrid(dtFineGrid);
        //                        divadd.Visible = true;
        //                        tdddl.Visible = true;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        //catch { }
    }

    protected void fpreport_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        //fpreport.SaveChanges();
        //try
        //{
        //    byte check = Convert.ToByte(fpreport.Sheets[0].ColumnHeader.Cells[0, 8].Value);
        //    if (check == 1)
        //    {
        //        for (int ik = 0; ik < fpreport.Sheets[0].Rows.Count; ik++)
        //        {
        //            fpreport.Sheets[0].Cells[ik, 8].Value = 1;
        //        }
        //    }
        //    else
        //    {
        //        for (int ik = 0; ik < fpreport.Sheets[0].Rows.Count; ik++)
        //        {
        //            fpreport.Sheets[0].Cells[ik, 8].Value = 0;
        //        }
        //    }
        //}
        //catch { }
    }


    private void saveForStudent()
    {
        FpSpread1.SaveChanges();
        fpreport.SaveChanges();
        if (Convert.ToInt64(ddl_reqname.SelectedItem.Value) == 10)
        {
            try
            {
                Int64 RequestType = Convert.ToInt64(ddl_reqname.SelectedItem.Value);
                int CollegeCode = Convert.ToInt16(ddlcollege.SelectedItem.Value);
                Int64 ReqStaffAppNo = 0;
                string ReqDegCode = "0";
                string ReqBatch = string.Empty;
                string reqSem = string.Empty;
                string reqSec = string.Empty;
                Int64 ReqAppStaffAppNo = 0;
                string type = "";
                int q = 0;
                int reqstaff = 0;
                int appstaff = 0;

                string cri = Convert.ToString(txt_criteria.Text);
                string activerow = "";
                activerow = fpreport.ActiveSheetView.ActiveRow.ToString();

                string valS = fpreport.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;

                for (int firstsp = 0; firstsp < Convert.ToInt16(fpreport.Sheets[0].RowCount); firstsp++)
                {

                    if (fpreport.Sheets[0].Cells[firstsp, 6].Locked != true)
                    {
                        int isval = Convert.ToInt32(fpreport.Sheets[0].Cells[firstsp, 6].Value);

                        ReqDegCode = Convert.ToString(fpreport.Sheets[0].GetTag(firstsp, 3));
                        ReqBatch = Convert.ToString(fpreport.Sheets[0].GetTag(firstsp, 2));
                        reqSem = Convert.ToString(fpreport.Sheets[0].GetTag(firstsp, 4));
                        reqSec = Convert.ToString(fpreport.Sheets[0].GetTag(firstsp, 5));

                        type = d2.GetFunction("select RequestType from RQ_RequestHierarchy where ReqStaffAppNo='" + ReqDegCode + "'");
                        string Qrystr = string.Empty;
                        string strIns = string.Empty;
                        if (!string.IsNullOrEmpty(reqSec))
                        {
                            Qrystr = " and isnull(Section,'')='" + reqSec + "'";
                        }

                        if (isval == 1)
                        {
                            //int FromDays = Convert.ToInt32(fpreport.Sheets[0].Cells[firstsp, 4].Text);
                            //int ToDays = Convert.ToInt32(fpreport.Sheets[0].Cells[firstsp, 5].Text);
                            int FromDays = 0;
                            int ToDays = 0;
                            reqstaff++;
                            string delete_query = "if exists (select * from RQ_RequestHierarchy where RequestType ='" + RequestType + "' and ReqStaffAppNo='" + ReqDegCode + "' and CollegeCode ='" + ddlclgStud.SelectedItem.Value + "' and FromDays='" + FromDays + "' and ToDays='" + ToDays + "' and BatchYear='" + ReqBatch + "' and Semester='" + reqSem + "'  " + Qrystr + ") delete RQ_RequestHierarchy where RequestType ='" + RequestType + "' and ReqStaffAppNo='" + ReqDegCode + "' and CollegeCode ='" + ddlclgStud.SelectedItem.Value + "'  and FromDays='" + FromDays + "' and ToDays='" + ToDays + "' and BatchYear='" + ReqBatch + "' and Semester='" + reqSem + "'  " + Qrystr + "";
                            d2.update_method_wo_parameter(delete_query, "Text");

                            for (int secondsp = 0; secondsp < Convert.ToInt16(FpSpread1.Sheets[0].RowCount); secondsp++)
                            {
                                string ReqApproveStage = "";
                                string bind = Convert.ToString(FpSpread1.Sheets[0].Cells[secondsp, 6].Text);

                                if (bind != "")
                                {
                                    string[] split = bind.Split('-');
                                    ReqApproveStage = split[0];
                                    reqapp_pri = split[1];
                                    abc1();
                                    int colcount = FpSpread1.Sheets[0].ColumnCount;

                                    // for (int i = 5; i < colcount; i += 2)
                                    //  {
                                    //     ReqApproveStage += 1;

                                    int isval1 = Convert.ToInt32(FpSpread1.Sheets[0].Cells[secondsp, 5].Value);
                                    string sql = "";
                                    if (isval1 == 1)
                                    {
                                        appstaff++;
                                        staff_code_fp2 = FpSpread1.Sheets[0].GetText(secondsp, 3);
                                        ReqAppStaffAppNo = Convert.ToInt64(da.GetFunction("select appl_id  from staff_appl_master a, staffmaster s where a.appl_no=s.appl_no and staff_code='" + staff_code_fp2 + "'"));
                                        // ReqAppPriority = Convert.ToInt16(FpSpread1.Sheets[0].GetText(secondsp, i + 1));

                                        sql = "insert into RQ_RequestHierarchy(RequestType,ReqStaffAppNo,ReqApproveStage,ReqAppStaffAppNo,ReqAppPriority,CollegeCode,ReqApproveStateCount,  FromDays, ToDays,DegreeCode,BatchYear,Semester,Section) values(" + RequestType + "," + ReqDegCode + "," + ReqApproveStage + "," + ReqAppStaffAppNo + "," + ReqAppPriority + "," + CollegeCode + ",'" + cri + "', '" + FromDays + "' ,'" + ToDays + "','" + ReqDegCode + "','" + ReqBatch + "','" + reqSem + "','" + reqSec + "')";

                                        q = da.update_method_wo_parameter(sql, "TEXT");
                                        tbl_div.Visible = false;
                                        imgdivalt.Visible = true;
                                        panel_erroralert.Visible = true;
                                        lbl_erroralert.Text = "Saved Successfully";
                                        btn_criteria1.Visible = false;
                                        btn_criteria2.Visible = false;
                                        btn_criteria3.Visible = false;
                                        btn_criteria4.Visible = false;
                                        btn_criteria5.Visible = false;
                                        btn_criteria6.Visible = false;
                                        btn_criteria7.Visible = false;
                                        btn_criteria8.Visible = false;
                                        btn_criteria1.Enabled = true;
                                        btn_criteria9.Visible = false;
                                        txt_criteria.Text = "";
                                        CLEARCOLOR();

                                    }
                                    // }
                                }
                            }
                        }
                    }
                }
                if (reqstaff == 0)
                {
                    Session["Priority"] = "";
                    ViewState["checkvalue"] = "";


                    CLEARCOLOR();

                    imgdivalt.Visible = true;
                    panel_erroralert.Visible = true;
                    lbl_erroralert.Text = "Choose Leave Settings";
                    return;
                }
                if (appstaff == 0)
                {
                    Session["Priority"] = "";
                    ViewState["checkvalue"] = "";
                    imgdivalt.Visible = true;
                    panel_erroralert.Visible = true;
                    lbl_erroralert.Text = "Choose Approval Staff";
                    return;
                }
                tbl_div.Visible = false;
            }
            catch (Exception ex)
            {
            }
            btngo_Click(new object(), new EventArgs());
        }
        else
        {
            FpSpread1.SaveChanges();
            fpreport.SaveChanges();

            try
            {
                Int64 RequestType = Convert.ToInt64(ddl_reqname.SelectedItem.Value);
                int CollegeCode = Convert.ToInt16(ddlcollege.SelectedItem.Value);
                Int64 ReqStaffAppNo = 0;
                string ReqDegCode = "0";
                Int64 ReqAppStaffAppNo = 0;
                string type = "";
                int q = 0;
                int reqstaff = 0;
                int appstaff = 0;

                string cri = Convert.ToString(txt_criteria.Text);
                string activerow = "";
                activerow = fpreport.ActiveSheetView.ActiveRow.ToString();

                string valS = fpreport.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;

                for (int firstsp = 0; firstsp < Convert.ToInt16(fpreport.Sheets[0].RowCount); firstsp++)
                {

                    if (fpreport.Sheets[0].Cells[firstsp, 8].Locked != true)
                    {
                        int isval = Convert.ToInt32(fpreport.Sheets[0].Cells[firstsp, 8].Value);

                        ReqDegCode = Convert.ToString(fpreport.Sheets[0].GetTag(firstsp, 6));
                        type = d2.GetFunction("select RequestType from RQ_RequestHierarchy where ReqStaffAppNo='" + ReqDegCode + "'");


                        if (isval == 1)
                        {
                            int FromDays = Convert.ToInt32(fpreport.Sheets[0].Cells[firstsp, 4].Text);
                            int ToDays = Convert.ToInt32(fpreport.Sheets[0].Cells[firstsp, 5].Text);

                            reqstaff++;
                            string delete_query = "if exists (select * from RQ_RequestHierarchy where RequestType ='" + RequestType + "' and ReqStaffAppNo='" + ReqDegCode + "' and CollegeCode ='" + ddlclgStud.SelectedItem.Value + "' and FromDays='" + FromDays + "' and ToDays='" + ToDays + "') delete RQ_RequestHierarchy where RequestType ='" + type + "' and ReqStaffAppNo='" + ReqDegCode + "' and CollegeCode ='" + ddlclgStud.SelectedItem.Value + "'  and FromDays='" + FromDays + "' and ToDays='" + ToDays + "'";
                            d2.update_method_wo_parameter(delete_query, "Text");

                            for (int secondsp = 0; secondsp < Convert.ToInt16(FpSpread1.Sheets[0].RowCount); secondsp++)
                            {
                                string ReqApproveStage = "";
                                string bind = Convert.ToString(FpSpread1.Sheets[0].Cells[secondsp, 6].Text);
                                if (bind != "")
                                {
                                    string[] split = bind.Split('-');
                                    ReqApproveStage = split[0];
                                    reqapp_pri = split[1];
                                    abc1();
                                    int colcount = FpSpread1.Sheets[0].ColumnCount;

                                    // for (int i = 5; i < colcount; i += 2)
                                    //  {
                                    //     ReqApproveStage += 1;

                                    int isval1 = Convert.ToInt32(FpSpread1.Sheets[0].Cells[secondsp, 5].Value);
                                    string sql = "";
                                    if (isval1 == 1)
                                    {
                                        appstaff++;
                                        staff_code_fp2 = FpSpread1.Sheets[0].GetText(secondsp, 3);
                                        ReqAppStaffAppNo = Convert.ToInt64(da.GetFunction("select appl_id  from staff_appl_master a, staffmaster s where a.appl_no=s.appl_no and staff_code='" + staff_code_fp2 + "'"));
                                        // ReqAppPriority = Convert.ToInt16(FpSpread1.Sheets[0].GetText(secondsp, i + 1));

                                        sql = "insert into RQ_RequestHierarchy(RequestType,ReqStaffAppNo,ReqApproveStage,ReqAppStaffAppNo,ReqAppPriority,CollegeCode,ReqApproveStateCount,  FromDays, ToDays) values(" + RequestType + "," + ReqDegCode + "," + ReqApproveStage + "," + ReqAppStaffAppNo + "," + ReqAppPriority + "," + CollegeCode + ",'" + cri + "', '" + FromDays + "' ,'" + ToDays + "')";

                                        q = da.update_method_wo_parameter(sql, "TEXT");
                                        tbl_div.Visible = false;
                                        imgdivalt.Visible = true;
                                        panel_erroralert.Visible = true;
                                        lbl_erroralert.Text = "Saved Successfully";
                                        btn_criteria1.Visible = false;
                                        btn_criteria2.Visible = false;
                                        btn_criteria3.Visible = false;
                                        btn_criteria4.Visible = false;
                                        btn_criteria5.Visible = false;
                                        btn_criteria6.Visible = false;
                                        btn_criteria7.Visible = false;
                                        btn_criteria8.Visible = false;
                                        btn_criteria1.Enabled = true;
                                        btn_criteria9.Visible = false;
                                        txt_criteria.Text = "";
                                        CLEARCOLOR();

                                    }
                                    // }
                                }
                            }
                        }
                    }
                }
                if (reqstaff == 0)
                {
                    Session["Priority"] = "";
                    ViewState["checkvalue"] = "";


                    CLEARCOLOR();

                    imgdivalt.Visible = true;
                    panel_erroralert.Visible = true;
                    lbl_erroralert.Text = "Choose Leave Settings";
                    return;
                }
                if (appstaff == 0)
                {
                    Session["Priority"] = "";
                    ViewState["checkvalue"] = "";
                    imgdivalt.Visible = true;
                    panel_erroralert.Visible = true;
                    lbl_erroralert.Text = "Choose Approval Staff";
                    return;
                }
                tbl_div.Visible = false;
            }
            catch (Exception ex)
            {
            }
            btngo_Click(new object(), new EventArgs());
        }

    }

    private void resetStud()
    {
        string checkappno = "";
        int vv = 0;

        for (int reset1 = 0; reset1 < Convert.ToInt16(fpreport.Sheets[0].RowCount); reset1++)
        {

            string appstaff_app = Convert.ToString(Convert.ToString(fpreport.Sheets[0].Cells[reset1, 6].Tag));
            fpreport.Sheets[0].Cells[reset1, 8].Locked = false;
            string stf_codee = appstaff_app;

            int FromDays = Convert.ToInt32(fpreport.Sheets[0].Cells[reset1, 4].Text);
            int ToDays = Convert.ToInt32(fpreport.Sheets[0].Cells[reset1, 5].Text);

            checkappno = d2.GetFunction("select ReqAppNo from RQ_Requisition r,RQ_RequestHierarchy rh where r.RequestType=rh.RequestType and rh.RequestType='" + ddl_reqname.SelectedItem.Value + "' and ReqAppNo='" + stf_codee + "' and ReqAppStatus='0'  and rh.FromDays='" + FromDays + "' and rh.ToDays='" + ToDays + "'");

            if (checkappno == "0")
            {

                string del_query = "delete from RQ_RequestHierarchy where ReqStaffAppNo='" + appstaff_app + "' and RequestType='" + ddl_reqname.SelectedItem.Value + "'  and   FromDays='" + FromDays + "' and ToDays='" + ToDays + "'";
                vv = d2.update_method_wo_parameter(del_query, "Text");
                btnreset.Enabled = false;
                btnview.Enabled = false;
            }
            else
            {
                imgdivalt.Visible = true;
                panel_erroralert.Visible = true;
                lbl_erroralert.Text = "You Cannot Delete This " + lblBr.Text;
                return;
            }
        }

        imgdivalt.Visible = true;
        panel_erroralert.Visible = true;
        lbl_erroralert.Text = "Deleted Successfully";
        txt_criteria.Text = "";
        btn_criteria1.Visible = false;
        btn_criteria2.Visible = false;
        btn_criteria3.Visible = false;
        btn_criteria4.Visible = false;
        btn_criteria5.Visible = false;
        btn_criteria6.Visible = false;
        btn_criteria7.Visible = false;
        btn_criteria8.Visible = false;
        btn_criteria9.Visible = false;
        Session["Priority"] = "";
        //#F0F0F0
        btn_criteria1.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria1.ForeColor = Color.Black;
        btn_criteria2.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria2.ForeColor = Color.Black;
        btn_criteria3.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria3.ForeColor = Color.Black;
        btn_criteria4.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria4.ForeColor = Color.Black;
        btn_criteria5.ForeColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria5.ForeColor = Color.Black;
        btn_criteria6.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria6.ForeColor = Color.Black;
        btn_criteria7.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria7.ForeColor = Color.Black;
        btn_criteria8.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria8.ForeColor = Color.Black;
        btn_criteria9.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria9.ForeColor = Color.Black;

        btn_criteria1.Enabled = true;
        btn_criteria2.Enabled = false;
        btn_criteria3.Enabled = false;
        btn_criteria4.Enabled = false;
        btn_criteria5.Enabled = false;
        btn_criteria6.Enabled = false;
        btn_criteria7.Enabled = false;
        btn_criteria8.Enabled = false;
        btn_criteria9.Enabled = false;
        btngo_Click(new object(), new EventArgs());
    }

    private void resetStages()
    {
        #region modified by Idhris 21-11-2016
        ViewState["checkvalue"] = null;
        Session["Priority"] = null;

        btn_criteria1.Enabled = true;
        btn_criteria1.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria1.ForeColor = Color.Black;
        if (Chktutor.Checked == true)
        {
            if (Txttutor.Text == "1")
            {

                btn_criteria2.Enabled = true;
            }
            else
                btn_criteria2.Enabled = false;
        }
        else
            btn_criteria2.Enabled = false;
        btn_criteria2.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria2.ForeColor = Color.Black;

        btn_criteria3.Enabled = false;
        btn_criteria3.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria3.ForeColor = Color.Black;

        btn_criteria4.Enabled = false;
        btn_criteria4.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria4.ForeColor = Color.Black;

        btn_criteria5.Enabled = false;
        btn_criteria5.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria5.ForeColor = Color.Black;

        btn_criteria6.Enabled = false;
        btn_criteria6.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria6.ForeColor = Color.Black;

        btn_criteria7.Enabled = false;
        btn_criteria7.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria7.ForeColor = Color.Black;

        btn_criteria8.Enabled = false;
        btn_criteria8.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria8.ForeColor = Color.Black;

        btn_criteria9.Enabled = false;
        btn_criteria9.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        btn_criteria9.ForeColor = Color.Black;
        #endregion
    }
    //Last modified by Idhris -- 21-11-2016

    #region modified rajkumar

    #region Common Checkbox and Checkboxlist Event
    private void checkBoxListselectOrDeselect(CheckBoxList cbl, bool selected = true)
    {
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                li.Selected = selected;
            }
        }
        catch
        {
        }
    }


    #endregion

    public void BindBatch()
    {

        try
        {
            string batchquery = string.Empty;
            ds.Clear();
            chkBatch.Checked = false;
            cblBatch.Items.Clear();
            string qryCollege = string.Empty;
            string collegeCode = string.Empty;
            if (ddlclgStud.Items.Count > 0 && ddlclgStud.Visible)
            {
                collegeCode = Convert.ToString(ddlclgStud.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollege = " and r.college_code in(" + collegeCode + ")";
                }
            }
            if (!string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(qryCollege))
            {
                batchquery = "select distinct r.Batch_Year from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=r.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and r.degree_code=dg.Degree_Code  and r.Batch_Year<>'0' and r.Batch_Year<>-1 and r.cc='0' and delflag='0' and exam_flag<>'debar' " + qryCollege + " order by r.Batch_Year desc";
                //ds.Clear();
                ds = da.select_method_wo_parameter(batchquery, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblBatch.DataSource = ds;
                    cblBatch.DataTextField = "Batch_Year";
                    cblBatch.DataValueField = "Batch_Year";
                    cblBatch.DataBind();
                    checkBoxListselectOrDeselect(cblBatch, true);
                    CallCheckboxListChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");
                }
            }
        }
        catch
        {

        }
    }
    public void binddegree()
    {
        try
        {
            ds.Clear();
            txtDegree.Text = "---Select---";
            string batchCode = string.Empty;
            chkDegree.Checked = false;
            cblDegree.Items.Clear();
            //userCode = Session["usercode"].ToString();
            //singleUser = Session["single_user"].ToString();
            //groupUserCode = Session["group_code"].ToString();
            string collegeCode = string.Empty;
            if (ddlclgStud.Items.Count > 0)
                collegeCode = ddlclgStud.SelectedValue.ToString().Trim();

            string columnfield = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            string valBatch = string.Empty;
            if (cblBatch.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch))
            {
                string selDegree = "SELECT DISTINCT c.course_id,c.course_name,c.Priority,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') " + columnfield + " ORDER BY CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = da.select_method_wo_parameter(selDegree, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblDegree.DataSource = ds;
                cblDegree.DataTextField = "course_name";
                cblDegree.DataValueField = "course_id";
                cblDegree.DataBind();
                checkBoxListselectOrDeselect(cblDegree, true);
                CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void bindbranch()
    {
        try
        {
            string degreecode = string.Empty;
            //collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            txtBranch.Text = "---Select---";
            chkBranch.Checked = false;
            cblBranch.Items.Clear();
            ds.Clear();
            string collegeCode = string.Empty;
            if (ddlclgStud.Items.Count > 0)
                collegeCode = ddlclgStud.SelectedValue.ToString().Trim();
            string selBranch = string.Empty;
            string columnfield = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            //string valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            //string valDegree = rs.GetSelectedItemsValueAsString(cblDegree);

            string valBatch = string.Empty;// rs.GetSelectedItemsValueAsString(cblBatch);
            string valDegree = string.Empty;//rs.GetSelectedItemsValueAsString(cblBranch);
            if (cblBatch.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            if (cblDegree.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(cblDegree);

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
            {
                selBranch = "SELECT DISTINCT dg.Degree_Code,dt.Dept_Name,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') AND c.Course_Id in('" + valDegree + "') " + columnfield + " ORDER BY dg.Degree_Code, CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = da.select_method_wo_parameter(selBranch, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblBranch.DataSource = ds;
                cblBranch.DataTextField = "dept_name";
                cblBranch.DataValueField = "degree_code";
                cblBranch.DataBind();
                checkBoxListselectOrDeselect(cblBranch, true);
                CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void chkBatch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");
            binddegree();
            bindbranch();
        }
        catch (Exception ex)
        {
        }
    }

    protected void cblBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");
            binddegree();
            bindbranch();
        }
        catch (Exception ex)
        {
        }
    }

    protected void chkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            bindbranch();
        }
        catch (Exception ex)
        {
        }
    }

    protected void cblDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            bindbranch();
        }
        catch (Exception ex)
        {
        }
    }

    protected void chkBranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
        }
        catch (Exception ex)
        {
        }
    }

    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    #region dept
    public void loadcolleges()
    {
        try
        {
            ddlclg.Items.Clear();
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlclg.DataSource = ds;
                ddlclg.DataTextField = "collname";
                ddlclg.DataValueField = "college_code";
                ddlclg.DataBind();
            }
            binddegrees();
        }
        catch
        { }
    }
    protected void ddlcol_SelectedIndexChanged(object sender, EventArgs e)
    {

        // gatepassrights();

        //  btnMainGos_Click(sender, e);

    }
    public void bindbatchs()
    {
        Cbllbatch.Items.Clear();
        DataSet ds_load = d2.select_method_wo_parameter("bind_batch", "sp");
        int count = ds_load.Tables[0].Rows.Count;
        if (count > 0)
        {
            Cbllbatch.DataSource = ds_load;
            Cbllbatch.DataTextField = "batch_year";
            Cbllbatch.DataValueField = "batch_year";
            Cbllbatch.DataBind();
        }

    }
    public void bindbranchs()
    {
        cbl_branch.Items.Clear();
        hat.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Convert.ToString(ddlclg.SelectedValue);
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }

        string typ = string.Empty;
        if (cbl_degree.Items.Count > 0)
        {
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (typ == "")
                    {
                        typ = "" + cbl_degree.Items[i].Value + "";
                    }
                    else
                    {
                        typ = typ + "'" + "," + "'" + cbl_degree.Items[i].Value + "";
                    }
                }

            }
        }
        if (typ != "")
        {
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("course_id", typ);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);

            DataSet ds_load = d2.select_method("bind_branch", hat, "sp");
            int count2 = ds_load.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                cbl_branch.DataSource = ds_load;
                cbl_branch.DataTextField = "dept_name";
                cbl_branch.DataValueField = "degree_code";
                cbl_branch.DataBind();
            }
        }
    }
    public void binddegrees()
    {
        cbl_degree.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Convert.ToString(ddlclg.SelectedValue);
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        hat.Clear();
        hat.Add("single_user", singleuser.ToString());
        hat.Add("group_code", group_user);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        DataSet ds_load = d2.select_method("bind_degree", hat, "sp");
        int count1 = ds_load.Tables[0].Rows.Count;
        if (count1 > 0)
        {
            cbl_degree.DataSource = ds_load;
            cbl_degree.DataTextField = "course_name";
            cbl_degree.DataValueField = "course_id";
            cbl_degree.DataBind();
        }
    }
    public void ChkBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ChklBatch.Checked == true)
            {
                for (int i = 0; i < Cbllbatch.Items.Count; i++)
                {
                    Cbllbatch.Items[i].Selected = true;
                }
                txtbat.Text = "Batch(" + (Cbllbatch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Cbllbatch.Items.Count; i++)
                {
                    Cbllbatch.Items[i].Selected = false;
                }
                txtbat.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void Cblbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txtbat.Text = "--Select--";
            ChklBatch.Checked = false;
            for (int i = 0; i < Cbllbatch.Items.Count; i++)
            {
                if (Cbllbatch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtbat.Text = "Batch(" + commcount.ToString() + ")";
                if (commcount == Cbllbatch.Items.Count)
                {
                    ChklBatch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }


    public void cb_degree_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_degree.Checked == true)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = true;
                }
                txt_degree.Text = "Degree(" + (cbl_degree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                }
                txt_degree.Text = "--Select--";
            }
            bindbranchs();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_degree.Text = "--Select--";
            cb_degree.Checked = false;
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_degree.Text = "Batch(" + commcount.ToString() + ")";
                if (commcount == cbl_degree.Items.Count)
                {
                    cb_degree.Checked = true;
                }
            }
            bindbranchs();
        }
        catch (Exception ex)
        {
        }
    }

    public void cb_branch_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_branch.Checked == true)
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = true;
                }
                txt_branch.Text = "Branch(" + (cbl_branch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = false;
                }
                txt_branch.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_branch.Text = "--Select--";
            cb_branch.Checked = false;
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_branch.Text = "Branch(" + commcount.ToString() + ")";
                if (commcount == cbl_branch.Items.Count)
                {
                    cb_branch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void btngostu_Click(object sender, EventArgs e)
    {
        try
        {
            bindspd_gatepass();
            bindspread_gatepass_dept();
        }
        catch
        {
        }
    }


    //public void bindspd_gatepassdept()
    //{
    //    FpSpread3.Sheets[0].PageSize = 12;
    //    FpSpread3.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
    //    FpSpread3.Pager.Mode = FarPoint.Web.Spread.PagerMode.NextPrev;
    //    FpSpread3.Pager.Align = HorizontalAlign.Right;

    //    FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
    //    darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
    //    darkstyle2.ForeColor = Color.Black;
    //    darkstyle2.HorizontalAlign = HorizontalAlign.Center;
    //    FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
    //    FpSpread3.Sheets[0].AutoPostBack = false;
    //    FpSpread3.Pager.PageCount = 5;
    //    FpSpread3.ActiveSheetView.SheetCorner.DefaultStyle.Font.Bold = false;
    //    FpSpread3.ActiveSheetView.DefaultRowHeight = 25;
    //    FpSpread3.ActiveSheetView.Rows.Default.Font.Name = "Book Antiqua";
    //    FpSpread3.ActiveSheetView.Rows.Default.Font.Size = FontUnit.Small;
    //    FpSpread3.ActiveSheetView.Rows.Default.Font.Bold = false;
    //    FpSpread3.ActiveSheetView.Columns.Default.Font.Bold = false;
    //    FpSpread3.ActiveSheetView.Columns.Default.Font.Size = FontUnit.Small;
    //    FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
    //    FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
    //    FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
    //    FpSpread3.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
    //    FpSpread3.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
    //    FpSpread3.Sheets[0].ColumnCount = 3;
    //    FpSpread3.Sheets[0].RowCount = 0;
    //    FarPoint.Web.Spread.CheckBoxCellType cbxd = new FarPoint.Web.Spread.CheckBoxCellType();
    //    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
    //    cbxd.AutoPostBack = false;
    //    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
    //    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
    //    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Department";


    //    FpSpread3.Sheets[0].Columns[1].CellType = cbxd;
    //    FpSpread3.Sheets[0].Columns[0].CellType = txt;
    //    FpSpread3.Sheets[0].Columns[2].CellType = txt;

    //    FpSpread3.Sheets[0].Columns[0].Width = 50;
    //    //FpSpread3.Sheets[0].Columns[0].Locked = true;
    //    //FpSpread3.Sheets[0].Columns[1].Locked = true;
    //    FpSpread3.Sheets[0].Columns[1].Width = 50;
    //    FpSpread3.Sheets[0].Columns[2].Width = 400;

    //    FpSpread3.Sheets[0].Columns[0].Locked = true;
    //    FpSpread3.Sheets[0].Columns[1].Locked = false;
    //    FpSpread3.Sheets[0].Columns[2].Locked = true;
    //    FpSpread3.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
    //    FpSpread3.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
    //    FpSpread3.Width = 509;


    //    FpSpread3.CommandBar.Visible = false;
    //    FpSpread3.Sheets[0].RowHeader.Visible = false;
    //}
    //public void bindspread_gatepass_depts()
    //{
    //    //dept...Bind
    //    for (int cnt1 = 0; cnt1 < chldeptstaff.Items.Count; cnt1++)
    //    {
    //        if (chldeptstaff.Items[cnt1].Selected == true)
    //        {
    //            if (dept_all == "")
    //            {
    //                dept_all = chldeptstaff.Items[cnt1].Value;
    //            }
    //            else
    //            {
    //                dept_all = dept_all + "','" + chldeptstaff.Items[cnt1].Value;
    //            }
    //        }
    //    }


    //    string sqlcmd = "";
    //    string deptcode1 = "";
    //    FpSpread3.SaveChanges();
    //    if (rdbreqstu.Checked == false)
    //        sqlcmd = "select distinct h.dept_name,h.dept_code from staff_appl_master a ,staffmaster s,hrdept_master h,desig_master d,stafftrans st where a.appl_no =s.appl_no and s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode and h.dept_code in ('" + dept_all + "')  and s.college_code='" + ddlcollegestaff.SelectedValue.ToString() + "' and resign = 0 and settled = 0 and latestrec=1";
    //    else
    //    {
    //        string typ = string.Empty;
    //        string qurys = string.Empty;
    //        if (cbl_degree.Items.Count > 0)
    //        {
    //            for (int i = 0; i < cbl_degree.Items.Count; i++)
    //            {
    //                if (cbl_degree.Items[i].Selected == true)
    //                {
    //                    if (typ == "")
    //                    {
    //                        typ = "" + cbl_degree.Items[i].Value + "";
    //                    }
    //                    else
    //                    {
    //                        typ = typ + "'" + "," + "'" + cbl_degree.Items[i].Value + "";
    //                    }
    //                }

    //            }
    //        }
    //        if (!string.IsNullOrEmpty(group_user) && group_user.Trim() != "0")
    //        {
    //            if (group_user.Contains(';'))
    //            {
    //                string[] group_semi = group_user.Split(';');
    //                group_user = group_semi[0].ToString();
    //            }
    //            qurys = " and group_code='" + group_user + "'";
    //            sqlcmd = "  select distinct degree.degree_code as dept_code,department.dept_name,degree.Acronym from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + typ + "') and degree.college_code=@college_code  and deptprivilages.Degree_code=degree.Degree_code " + qurys + "";
    //        }
    //        else if (!string.IsNullOrEmpty(usercode) && usercode.Trim() != "0")
    //        {
    //            qurys = " and user_code='" + usercode + "'";//magesh 11.1.18
    //            sqlcmd = "  select distinct degree.degree_code as dept_code,department.dept_name,degree.Acronym from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + typ + "') and degree.college_code='" + ddlclg.SelectedValue + "'  and deptprivilages.Degree_code=degree.Degree_code  " + qurys + "  ";
    //        }
    //    }

    //    sqlcmd = sqlcmd + " select ReqStaffAppNo,RequestType,DegreeCode from RQ_RequestHierarchy";
    //    dsload = dset.select_method_wo_parameter(sqlcmd, "Text");
    //    int sno = 1;
    //    DataView dv = new DataView();
    //    if (dsload.Tables[0].Rows.Count > 0)
    //    {
    //        for (int loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
    //        {

    //            FpSpread3.Sheets[0].RowCount++;

    //            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
    //            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Value = 0;
    //            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["dept_name"].ToString();
    //            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Tag = dsload.Tables[0].Rows[loop]["dept_code"].ToString();

    //            deptcode1 = Convert.ToString(FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Tag);
    //            if (rdbreqstu.Checked == true)
    //                dsload.Tables[1].DefaultView.RowFilter = "DegreeCode='" + deptcode1 + "' and RequestType='" + ddl_reqname.SelectedItem.Value + "'";
    //            else
    //                dsload.Tables[1].DefaultView.RowFilter = "ReqStaffAppNo='" + deptcode1 + "' and RequestType='" + ddl_reqname.SelectedItem.Value + "'";
    //            dv = dsload.Tables[1].DefaultView;

    //            if (dv.Count > 0)
    //            {

    //                FpSpread3.Sheets[0].Rows[FpSpread3.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#F0A3CC");
    //                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Value = 1;
    //                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Locked = true;
    //                FpSpread3.Enabled = true;
    //                btnreset.Enabled = true;
    //                btnview.Enabled = true;
    //            }
    //            else
    //            {
    //                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Value = 0;
    //                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Locked = false;
    //            }

    //            sno++;
    //        }
    //        FpSpread3.SaveChanges();
    //        FpSpread3.Visible = true;

    //    }
    //    else
    //    {

    //    }

    //    FpSpread3.Sheets[0].PageSize = 12;
    //    FpSpread3.TitleInfo.Height = 30;
    //    if (FpSpread3.Sheets[0].RowCount > 10)
    //    {
    //        FpSpread3.Height = 390;
    //    }
    //    else
    //    {
    //        FpSpread3.Height = (FpSpread3.Sheets[0].RowCount * 25) + 140;
    //    }
    //    FpSpread3.Height = 390;
    //    //  }
    //}
    #endregion



    //Added By Saranyadevi22.2.2019

    protected void rdb_hostelstd_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            Lbel7.Visible = false;
            UpdatePanel7.Visible = false;
            Label7.Visible = false;
            UpdatePanel8.Visible = false;
            Label6.Visible = false;
            UpdatePanel6.Visible = false;
            Label3.Visible = false;
            ddlclg.Visible = false;
            if (rdb_hostelstd.Checked == true)
            {
                divStaffDept.Visible = true;
                divStaffDets.Visible = false;
                lbl_hostel.Visible = true;
                Upp2hostel.Visible = true;
                loadcolleges();
                bindhostel();
                btngostu_Click(sender, e);

            }

        }
        catch
        {

        }

    }

    protected void bindhostel()
    {
        try
        {
            cbl_hostelname.Items.Clear();
            string MessmasterFK = string.Empty;
            if (usercode != "" && usercode != "0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Rights' and user_code='" + usercode + "'");
            if (group_user != "" && group_user != "0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Rights' and user_code='" + group_user + "'");
            string itemname = "select HostelMasterPK,HostelName from HM_HostelMaster where  HostelMasterPK in (" + MessmasterFK + ") order by hostelname ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "HostelName";
                cbl_hostelname.DataValueField = "HostelMasterPK";
                cbl_hostelname.DataBind();
                cb_hostelname.Checked = true;
                // mm = cbl_hostelname.SelectedValue;
            }
            else
            {
                cb_hostelname.Checked = false;
                txt_hostelname.Text = "--Select--";
            }
            rs.CallCheckboxChange(cb_hostelname, cbl_hostelname, txt_hostelname, lbl_hostel.Text, "--Select--");
        }
        catch (Exception ex)
        {
        }

    }
    public void cb_hostelname_checkedchange(object sender, EventArgs e)
    {
        try
        {
            rs.CallCheckboxChange(cb_hostelname, cbl_hostelname, txt_hostelname, lbl_hostel.Text, "--Select--");
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckboxListChange(cb_hostelname, cbl_hostelname, txt_hostelname, lbl_hostel.Text, "--Select--");
    }




}