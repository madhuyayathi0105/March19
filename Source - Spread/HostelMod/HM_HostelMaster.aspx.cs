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
public partial class HM_HostelMaster : System.Web.UI.Page
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
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    Hashtable indexbill = new Hashtable();
    Hashtable hashdays = new Hashtable();
    Hashtable has = new Hashtable();
    bool check = false;
    bool war = false;
    bool war1 = false;
    string buildvalue1 = "";
    string build1 = "";
    string sql = "";
    static string checkvalue = "";
    Boolean Cellclick = false;
    int i = 0;
    int commcount = 0;
    string clgname = "";
    string led = "";
    string messbill = "";
    string gatepass = "";
    string query = "";
    string name = "";
    int rowcount;
    int y;
    string code = "";

    string hostelname = "";
    string warden = "";
    string warden1 = "";
    string gender = "";
    string building = "";
    string phone = "";
    string extension = "";
    string mobile = "";
    string email = "";
    string roomrent = "";
    string roomrentheader = "";
    string hosteladm = "";
    string hosteladmheader = "";
    string studentadm = "";
    string bill = "";
    string mbill = "";
    string pay = "";
    string messfee = "";
    string date = "";
    string messbilldays = "";
    string messbillheader = "";
    string messbillheaderr = "";
    string Gate_per = "";
    string Gatepassunapprove = "";
    string messname = "";

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
        lbl_norec.Visible = false;
        if (!IsPostBack)
        {
            bindclg();
            bindhostel();
            bindrrh();
            //ddl_hosah();
            //rdb_monthly.Checked = true;
            ddl_rrl.Items.Insert(0, "Select");
            ddl_hosteledger.Items.Insert(0, "Select");
            ddl_messdayscholar.Items.Insert(0, "Select");
            ddl_messbill.Items.Insert(0, "Select");

            rdb_male.Checked = true;
            rdb_div.Checked = true;
            rdb_fixed.Checked = true;
            txt_gatepass.Enabled = false;
            txt_messfee.Enabled = false;
            txt_searchby.Visible = true;
            checkvalue = "";
            lbl_norec.Visible = false;

            binddays();
            bindclgpop1();

            //bindmessbiled();
            //bindmessdays();
            ViewState["BuildingCode"] = null;
            ViewState["WardenCode"] = null;
            ViewState["WardenCode1"] = null;
            txt_duedate.Attributes.Add("readonly", "readonly");
            txt_duedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_frmdate.Attributes.Add("readonly", "readonly");
            txt_frmdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;

            fpbuild.Sheets[0].RowCount = 0;
            fpbuild.Sheets[0].ColumnCount = 0;
            fpbuild.SheetCorner.ColumnCount = 0;

            bindcollege();
            binddepartment();
            btn_go_Click(sender, e);
            bindmessmaster();
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
    public void bindmessmaster()
    {
        try
        {
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
           // ddl_messmaster.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                //magesh 20.6.18
                cbl_Mess.DataSource = ds;
                cbl_Mess.DataTextField = "MessName";
                cbl_Mess.DataValueField = "MessMasterPK";
                cbl_Mess.DataBind();
                if (cbl_Mess.Items.Count > 0)
                {
                    for (i = 0; i < cbl_Mess.Items.Count; i++)
                    {
                        //cbl_Mess.Items[i].Selected = true;
                    }
                    txt_Mess.Text = "--Select--";
                }
            }
            else
            {
                txt_Mess.Text = "--Select--";
            }
            //    ddl_messmaster.DataSource = ds;
            //    ddl_messmaster.DataTextField = "MessName";
            //    ddl_messmaster.DataValueField = "MessMasterPK";
            //    ddl_messmaster.DataBind();
            //}
            //ddl_messmaster.Items.Insert(0, "Select");
        }
        catch
        {
            //ddl_messmaster.Items.Clear();
        }
    }
    public void bindhostel()
    {
        try
        {
            ds.Clear();

            string itemname = "select HostelMasterPK,HostelName  from HM_HostelMaster ";// where CollegeCode in ('" + ddl_college.SelectedItem.Value + "') order by HostelMasterPK ";
            ds = d2.select_method_wo_parameter(itemname, "Text");
            cbl_hostelname.Items.Clear();
            // ds = d2.BindHostel(ddl_college.SelectedItem.Value);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "HostelName";
                cbl_hostelname.DataValueField = "HostelMasterPK";
                cbl_hostelname.DataBind();
                if (cbl_hostelname.Items.Count > 0)
                {
                    for (i = 0; i < cbl_hostelname.Items.Count; i++)
                    {
                        cbl_hostelname.Items[i].Selected = true;
                    }
                    txt_hostelname.Text = "Hostel Name(" + cbl_hostelname.Items.Count + ")";
                }
            }
            else
            {
                txt_hostelname.Text = "--Select--";
            }
        }
        catch
        {

        }
    }
    protected void cb_hostelname_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_hostelname.Checked == true)
            {
                for (i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = true;
                }
                txt_hostelname.Text = "Hostel Name(" + (cbl_hostelname.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = false;
                }
                txt_hostelname.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_hostelname.Text = "--Select--";
            cb_hostelname.Checked = false;
            commcount = 0;
            for (i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_hostelname.Text = "Hostel Name(" + commcount.ToString() + ")";
                if (commcount == cbl_hostelname.Items.Count)
                {
                    cb_hostelname.Checked = true;
                }
            }

        }
        catch
        {
        }
    }

    public void bindclg()
    {
        try
        {
            ds.Clear();

            clgname = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(clgname, "Text");
            ddl_college.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddl_college_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindhostel();

            pheaderfilter.Visible = false;
            pcolumnorder.Visible = false;
            FpSpread1.Visible = false;
            fpreaddiv.Visible = false;
            div_report.Visible = false;

        }
        catch (Exception ex)
        {
        }
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            int index;
            Printcontrol.Visible = false;
            FpSpread1.Visible = true;//13.10.15
            //div_report.Visible = true;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;
            //FpSpread1.SheetCorner.ColumnCount = 0;
            ds.Clear();
            DataView dv1 = new DataView();
            Hashtable columnhash = new Hashtable();

            columnhash.Add("HostelName", "Hostel Name");
            columnhash.Add("WardenStaff1PK", "Warden Name1");
            columnhash.Add("WardentStaff2PK", "Warden Name2");
            columnhash.Add("HostelBuildingFK", "Building Name");
            columnhash.Add("PhoneNo", "Phone No");
            columnhash.Add("PhoneExtNo", "Extension No");
            columnhash.Add("MobileNo", "Mobile No");
            columnhash.Add("EmailID", "Email");
            columnhash.Add("RoomRentLedgerFK", "Room Rent Ledger");
            columnhash.Add("HostelAdmFeeAmount", "Hostel Admission Fee");
            columnhash.Add("HostelAdmFeeLedgerFK", "Student Fee Ledger");
            columnhash.Add("NessBukkLedgerFK", "Mess Bill Ledger");
            columnhash.Add("MessBillDSLedgerFK", "Mess Bill Ledger(Dayscholar)");
            //columnhash.Add("Pay_Type", "Pay Type");
            //columnhash.Add("Mess_FixedFeeAmt", "Mess Fee");

            columnhash.Add("MessBillPayDueDays", "Due Days");
            columnhash.Add("MessBillType", "Mess Bill Type");
            columnhash.Add("MessBillMethod", "Fixed Type");
            columnhash.Add("HostelType", "Gender");
            columnhash.Add("IsHostelGatePassPer", "Gate Pass");
            columnhash.Add("HostelGatePassPerCount", "Gate PerCount");
            columnhash.Add("IsAllowUnApproveStud", "Un Approved Students GatePass");

            if (ItemList.Count == 0)
            {
                ItemList.Add("HostelName");
                ItemList.Add("WardenStaff1PK");
                ItemList.Add("HostelBuildingFK");
            }

            if (txt_hostelname.Text != "--Select--")
            {
                sql = "select HostelMasterPK,HostelName,WardenStaff1PK,PhoneExtNo,WardentStaff2PK,HostelBuildingFK,PhoneNo,MobileNo,EmailID ,RoomRentLedgerFK,MessBillType,HostelAdmFeeAmount ,HostelAdmFeeLedgerFK,NessBukkLedgerFK,MessBillDSLedgerFK,MessBillPayDueDays,case when IsHostelGatePassPer =0 then 'No' when IsHostelGatePassPer =1 then 'Yes' end as IsHostelGatePassPer  ,HostelGatePassPerCount,case when IsAllowUnApproveStud=0 then 'No' when IsAllowUnApproveStud=1 then 'Yes' end as IsAllowUnApproveStud   ,case when HostelType = 1 then 'Male'  when HostelType = 2 then 'Female' else 'Both' end HostelType, case when MessBillType = 0 and MessBillMethod = 0  then 'Fixed/Dividend'when MessBillType =0 and MessBillMethod=1 then 'Fixed/Nondividend'when MessBillType =1  and MessBillMethod= 0 then 'Fixed & Additional Purchase/Dividend'when MessBillType =1 and MessBillMethod= 1  then 'Fixed & Additional Purchase/NonDividend'else 'Purchase Only' end MessBillType,MessBillMethod,h.HostelBuildingFK  from HM_HostelMaster h";//CONVERT(varchar(10), Due_Date,103) as Due_Date
                //case when Pay_Type =1  then 'Monthly'when Pay_Type =2 then 'Yearly' when Pay_Type=3 then 'Semester' end Pay_Type

                for (i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cbl_hostelname.Items[i].Selected == true)
                    {
                        build1 = cbl_hostelname.Items[i].Value.ToString();
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

                if (buildvalue1 != "")
                {
                    sql = sql + " Where HostelMasterPK in ('" + buildvalue1 + "')";
                }
                else
                {
                    sql = sql + "";
                }


                sql = sql + "ORDER BY HostelName Asc";
                sql = sql + " select  appl_id,s.staff_code,staff_name ,d.desig_name,h.dept_name  from staffmaster s,stafftrans t,hrdept_master h,desig_master d,staff_appl_master a where  settled =0 and resign =0 and s.staff_code =t.staff_code and t.desig_code =d.desig_code and t.dept_code =h.dept_code and  s.appl_no =a.appl_no and latestrec =1 ";
                sql = sql + " select Building_Name,Code  from Building_Master";
                sql = sql + " select LedgerPK,LedgerName from FM_LedgerMaster where CollegeCode ='" + collegecode1 + "'  and LedgerMode =0";
                ds = d2.select_method_wo_parameter(sql, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    pheaderfilter.Visible = true;
                    pcolumnorder.Visible = true;
                    lbl_error.Visible = false;
                    lbl_errormsg.Visible = false;


                    FpSpread1.Sheets[0].RowCount = 0;
                    // FpSpread1.SheetCorner.ColumnCount = 0;
                    FpSpread1.CommandBar.Visible = false;
                    FpSpread1.Sheets[0].AutoPostBack = true;
                    //FpSpread1.Sheets[0].ColumnCount = 9;
                    //FpSpread1.Sheets[0].ColumnCount = 0;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread1.Sheets[0].RowHeader.Visible = false;
                    FpSpread1.Sheets[0].ColumnCount = ItemList.Count + 1;
                    FpSpread1.Sheets[0].RowCount = ds.Tables[0].Rows.Count;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Columns[0].Width = 50;
                    FpSpread1.Columns[0].Locked = true;

                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        string colno = Convert.ToString(ds.Tables[0].Columns[j]);
                        if (ItemList.Contains(Convert.ToString(colno)))
                        {
                            index = ItemList.IndexOf(Convert.ToString(colno));

                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Text = Convert.ToString(columnhash[colno]);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, index + 1].HorizontalAlign = HorizontalAlign.Center;

                            //if (colno == "Hostel_Name")
                            //{
                            //    FpSpread1.Columns[index + 1].Width = 200;
                            //}
                        }
                    }
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FpSpread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                        FpSpread1.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[i, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["HostelMasterPK"]);

                        FpSpread1.Sheets[0].Columns[0].Width = 50;
                        FpSpread1.Sheets[0].Cells[i, 0].Font.Name = "Book Antiqua";
                        for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                        {
                            //if (ItemList.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
                            //{
                            //    index = ItemList.IndexOf(Convert.ToString(ds.Tables[0].Columns[j].ToString()));
                            //    if (Convert.ToString(ds.Tables[0].Columns[j]) == "Hostel Name")
                            //    {
                            //        FpSpread1.Sheets[0].Columns[index + 1].Width = 200;
                            //        FpSpread1.Sheets[0].Columns[index + 1].Locked = true;
                            //        FpSpread1.Sheets[0].Cells[i, index + 1].Text = ds.Tables[0].Rows[i][j].ToString();
                            //        FpSpread1.Sheets[0].Cells[i, index + 1].Font.Name = "Book Antiqua";
                            //        FpSpread1.Sheets[0].Cells[i, index + 1].Font.Size = FontUnit.Medium;
                            //    }

                            string colname = Convert.ToString(ds.Tables[0].Columns[j]);
                            if (ItemList.Contains(Convert.ToString(colname)))
                            {
                                index = ItemList.IndexOf(Convert.ToString(ds.Tables[0].Columns[j]));
                                FpSpread1.Sheets[0].Columns[index + 1].Width = 100;
                                FpSpread1.Sheets[0].Columns[index + 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[i, index + 1].Font.Size = FontUnit.Medium;
                                if (colname.Trim() != "RoomRentLedgerFK" && colname.Trim() != "MessBillDSLedgerFK" && colname.Trim() != "HostelAdmFeeLedgerFK" && colname.Trim() != "NessBukkLedgerFK")
                                {
                                    if (colname.Trim() == "HostelName")
                                    {
                                        FpSpread1.Sheets[0].Columns[index + 1].Width = 200;
                                        FpSpread1.Sheets[0].Cells[i, index + 1].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[i, index + 1].Font.Size = FontUnit.Medium;
                                    }

                                    if (colname.Trim() == "MobileNo" || colname.Trim() == "PhoneNo" || colname.Trim() == "PhoneExtNo")
                                    {
                                        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                                        FpSpread1.Sheets[0].Cells[i, index + 1].CellType = txt;
                                    }
                                    if (colname.Trim() != "HostelBuildingFK")
                                    {
                                        if (colname.Trim() != "WardenStaff1PK" && colname.Trim() != "WardentStaff2PK")
                                        {
                                            FpSpread1.Sheets[0].Cells[i, index + 1].Text = ds.Tables[0].Rows[i][j].ToString();
                                            FpSpread1.Sheets[0].Cells[i, index + 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[i, index + 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Left;
                                        }
                                        else
                                        {
                                            if (ds.Tables[1].Rows.Count > 0)
                                            {
                                                if (Convert.ToString(ds.Tables[0].Rows[i][j]).Trim() != "")
                                                {
                                                    ds.Tables[1].DefaultView.RowFilter = "appl_id='" + Convert.ToString(ds.Tables[0].Rows[i][j]) + "'";
                                                    dv1 = ds.Tables[1].DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[i, index + 1].Text = Convert.ToString(dv1[0]["staff_name"]);
                                                        FpSpread1.Sheets[0].Cells[i, index + 1].Tag = Convert.ToString(dv1[0]["appl_id"]);
                                                        FpSpread1.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    else
                                    {
                                        if (ds.Tables[2].Rows.Count > 0)
                                        {
                                            if (Convert.ToString(ds.Tables[0].Rows[i][j]).Trim() != "")
                                            {
                                                ds.Tables[2].DefaultView.RowFilter = "code in (" + Convert.ToString(ds.Tables[0].Rows[i][j]) + ")";
                                                dv1 = ds.Tables[2].DefaultView;
                                                if (dv1.Count > 0)
                                                {
                                                    string buildvalue = "";
                                                    for (int r = 0; r < dv1.Count; r++)
                                                    {
                                                        if (buildvalue == "")
                                                        {
                                                            buildvalue = Convert.ToString(dv1[r]["Building_Name"]);
                                                        }
                                                        else
                                                        {
                                                            buildvalue = buildvalue + "," + Convert.ToString(dv1[r]["Building_Name"]);
                                                        }
                                                    }
                                                    FpSpread1.Sheets[0].Cells[i, index + 1].Text = Convert.ToString(buildvalue);
                                                    FpSpread1.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Left;
                                                    //FpSpread1.Sheets[0].Cells[i, index + 1].Tag = Convert.ToString(dv1[0]["Staff_Code"]);
                                                }
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    if (ds.Tables[3].Rows.Count > 0)
                                    {
                                        if (Convert.ToString(ds.Tables[0].Rows[i][j]).Trim() != "")
                                        {
                                            ds.Tables[3].DefaultView.RowFilter = "LedgerPK='" + Convert.ToString(ds.Tables[0].Rows[i][j]) + "'";
                                            dv1 = ds.Tables[3].DefaultView;
                                            if (dv1.Count > 0)
                                            {
                                                FpSpread1.Sheets[0].Cells[i, index + 1].Text = Convert.ToString(dv1[0]["LedgerName"]);
                                                FpSpread1.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Left;
                                            }
                                        }
                                    }
                                }
                                //}
                            }
                        }
                    }
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.Visible = true;
                    fpreaddiv.Visible = true;
                    div_report.Visible = true;
                    lbl_error.Visible = false;
                }
                else
                {
                    pheaderfilter.Visible = false;
                    pcolumnorder.Visible = false;
                    FpSpread1.Visible = false;
                    fpreaddiv.Visible = false;
                    div_report.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "No Records Found";
                }
            }
            else
            {
                pheaderfilter.Visible = false;
                pcolumnorder.Visible = false;
                FpSpread1.Visible = false;
                fpreaddiv.Visible = false;
                div_report.Visible = false;
                FpSpread1.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select Any One Hostel";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        try
        {
            popwindow.Visible = true;
            div5.Visible = true;
            div6.Visible = true;
            bindrrh();
            //ddl_hosah();
            //bindroomrent();
            //bindhostelfee();
            //bindmessbiled();
            // bindmessdays();
            clear();
            binddays();
            btn_save.Text = "Save";
            btn_save.Visible = true;
            btn_update.Visible = false;
            btn_delete.Visible = false;

            rdb_male.Checked = true;
            rdb_both.Checked = false;
            rdb_female.Checked = false;

            rdb_fixed.Checked = true;
            rdb_fixedpur.Checked = false;

            rdb_div.Checked = true;
            rdb_nondiv.Checked = false;

            //rdb_monthly.Checked = true;
            //rdb_yearly.Checked = false;
            //rdb_sem.Checked = false;

            txt_studentledger.Enabled = true;
            txt_gatepass.Enabled = false;
            txt_messfee.Enabled = false;
            cb_gatepass.Checked = false;
            cb_unappgatepass.Checked = false;
            txt_duedate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
            txt_duedate.Enabled = false;

            txt_messfee.Enabled = false;
            rdb_monthly.Enabled = false;
            rdb_yearly.Enabled = false;
            rdb_sem.Enabled = false;
            bindmessmaster();
            //btn_save.Visible = true;
        }
        catch
        {
        }
    }
    protected void clear()
    {
        txt_hostelname1.Text = "";
        txt_warden.Text = "";
        txt_department.Text = "";
        txt_designation.Text = "";
        txt_warden1.Text = "";
        txt_department1.Text = "";
        txt_designation1.Text = "";
        txt_building.Text = "";
        txt_phone.Text = "";
        txt_extension.Text = "";
        txt_mobile.Text = "";
        txt_email.Text = "";
        txt_studentledger.Text = "";
        txt_messfee.Text = "";
        txt_duedate.Text = "";
        txt_gatepass.Text = "";

        ddl_rrl.SelectedIndex = 0;
        ddl_hosteledger.SelectedIndex = 0;
        ddl_messbill.SelectedIndex = 0;
        ddl_messdayscholar.SelectedIndex = 0;

        ddl_rrh.SelectedIndex = 0;
        ddl_hosteladdheader.SelectedIndex = 0;
        ddl_messhed.SelectedIndex = 0;
        ddl_messbillded.SelectedIndex = 0;
    }

    public void cb_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string si = "";
            if (cb_column.Checked == true)
            {
                ItemList.Clear();
                for (i = 0; i < cbl_columnorder.Items.Count; i++)
                {
                    si = Convert.ToString(i);
                    cbl_columnorder.Items[i].Selected = true;
                    lb_columnorder.Visible = true;
                    ItemList.Add(cbl_columnorder.Items[i].Value.ToString());
                    Itemindex.Add(si);
                }
                lb_columnorder.Visible = true;
                txt_border.Visible = true;
                txt_border.Text = "";
                int j = 0;
                for (i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    txt_border.Text = txt_border.Text + ItemList[i].ToString();

                    txt_border.Text = txt_border.Text + "(" + (j).ToString() + ")  ";

                }

            }
            else
            {
                for (i = 0; i < cbl_columnorder.Items.Count; i++)
                {
                    cbl_columnorder.Items[i].Selected = false;
                    lb_columnorder.Visible = false;
                    ItemList.Clear();
                    Itemindex.Clear();
                    cbl_columnorder.Items[0].Enabled = false;
                }

                txt_border.Text = "";
                txt_border.Visible = false;

            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {

        }
    }
    public void lb_Click(object sender, EventArgs e)
    {
        try
        {
            cbl_columnorder.ClearSelection();
            cb_column.Checked = false;
            lb_columnorder.Visible = false;
            //cbl_columnorder.Items[0].Selected = true;
            ItemList.Clear();
            Itemindex.Clear();
            txt_border.Text = "";
            txt_border.Visible = false;
            //Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_columnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cb_column.Checked = false;
            string value = "";
            int index;
            string result = "";
            string sindex = "";
            cbl_columnorder.Items[0].Selected = true;
            cbl_columnorder.Items[0].Enabled = false;
            value = string.Empty;
            result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            sindex = Convert.ToString(index);
            if (cbl_columnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    //if (txt_border.Text == "")
                    //{
                    //    ItemList.Add("Roll No");
                    //}
                    //ItemList.Add("Admission No");
                    //ItemList.Add("Name");
                    ItemList.Add(cbl_columnorder.Items[index].Value.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cbl_columnorder.Items[index].Value.ToString());
                Itemindex.Remove(sindex);
            }
            for (i = 0; i < cbl_columnorder.Items.Count; i++)
            {
                //if (i == 0 || i == 1 || i == 2)
                //{
                //    cbl_columnorder.Items[0].Selected = true;
                //    cbl_columnorder.Items[1].Selected = true;
                //    cbl_columnorder.Items[2].Selected = true;
                //}
                if (cbl_columnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cbl_columnorder.Items[i].Value.ToString());
                    Itemindex.Remove(sindex);

                }
            }

            lb_columnorder.Visible = true;
            txt_border.Visible = true;
            txt_border.Text = "";
            for (i = 0; i < ItemList.Count; i++)
            {
                txt_border.Text = txt_border.Text + ItemList[i].ToString();

                txt_border.Text = txt_border.Text + "(" + (i + 1).ToString() + ")  ";

            }
            if (ItemList.Count == 22)
            {
                cb_column.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                txt_border.Visible = false;
                lb_columnorder.Visible = false;
            }

            //  Button2.Focus();
        }
        catch (Exception ex)
        {

        }
    }

    protected void FpSpread1_CellClick(object sender, EventArgs e)
    {
        try
        {
            Cellclick = true;
        }
        catch (Exception ex)
        {
        }
    }
    protected void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cellclick == true)
        {
            try
            {
                messleg();
                messbillleg();
                ddl_adjfeeleger();
                ddl_hosah();
                bindmessmaster();

                clear();
                btn_save.Visible = true;
                btn_save.Text = "Update";
                btn_update.Visible = false;
                btn_delete.Visible = true;
                //string retroll = "";
                //string bnam = "";
                //string phoneno = "";
                //string mobile = "";
                //string fix = "";
                string pay = "";
                string cl = "";
                string dmess = "";
                string mes = "";
                string ld = "";
                string lld = "";
                string n = "";
                string exphone = "";
                string exphoneno = "";
                string admnfeeno = "";
                string duedate = "";
                string dudat = "";
                string gateallow = "";
                string gateallowapprove = "";
                DataView dv1 = new DataView();
                int activerow = 0;
                activerow = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveRow.ToString());
                for (i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                {
                    if (i == Convert.ToInt32(activerow))
                    {
                        FpSpread1.Sheets[0].Rows[i].BackColor = Color.LightBlue;
                        FpSpread1.Sheets[0].SelectionBackColor = Color.LightBlue;
                        //FpSpread1.Sheets[0].SelectionForeColor = Color.White;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Rows[i].BackColor = Color.White;
                    }
                }
                string hostelcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                ViewState["Hostel_NewCode"] = Convert.ToString(hostelcode);
                if (hostelcode.Trim() != "")
                {
                    sql = "select MessMasterFK,MessMasterFK1, HostelMasterPK,IncludeRebate,HostelMasterPK,HostelName,WardenStaff1PK,PhoneExtNo,WardentStaff2PK,HostelBuildingFK,PhoneNo,MobileNo,EmailID ,RoomRentLedgerFK,MessBillType,HostelAdmFeeAmount ,HostelAdmFeeLedgerFK,NessBukkLedgerFK,MessBillDSLedgerFK,MessBillPayDueDays,case when IsHostelGatePassPer =0 then 'No' when IsHostelGatePassPer =1 then 'Yes' end as IsHostelGatePassPer  ,HostelGatePassPerCount,case when IsAllowUnApproveStud=0 then 'No' when IsAllowUnApproveStud=1 then 'Yes' end as IsAllowUnApproveStud   ,case when HostelType = 1 then 'Male'  when HostelType = 2 then 'Female' else 'Both' end HostelType, case when MessBillType = 0 and MessBillMethod = 0  then 'Fixed/Dividend'when MessBillType =0 and MessBillMethod=1 then 'Fixed/Nondividend'when MessBillType =1  and MessBillMethod= 0 then 'Fixed & Additional Purchase/Dividend'when MessBillType =1 and MessBillMethod= 1  then 'Fixed & Additional Purchase/NonDividend'else 'Purchase Only' end MessBillType,MessBillMethod,h.HostelBuildingFK  from HM_HostelMaster h where HostelMasterPK ='" + hostelcode + "'";
                    sql = sql + " select  appl_id,s.staff_code,staff_name ,d.desig_name,h.dept_name  from staffmaster s,stafftrans t,hrdept_master h,desig_master d,staff_appl_master a where  settled =0 and resign =0 and s.staff_code =t.staff_code and t.desig_code =d.desig_code and t.dept_code =h.dept_code and  s.appl_no =a.appl_no and latestrec =1 ";
                    sql = sql + " select Building_Name,Code  from Building_Master";

                    sql = sql + " select LedgerPK,LedgerName from FM_LedgerMaster where CollegeCode ='" + collegecode1 + "'  and LedgerMode =0";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sql, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txt_hostelname1.Text = ds.Tables[0].Rows[0]["HostelName"].ToString();
                        warden = Convert.ToString(ds.Tables[0].Rows[0]["WardenStaff1PK"]);
                        ViewState["WardenCode"] = Convert.ToString(warden);


                        if (warden != "")
                        {
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                ds.Tables[1].DefaultView.RowFilter = "appl_id='" + warden + "'";
                                dv1 = ds.Tables[1].DefaultView;
                                if (dv1.Count > 0)
                                {
                                    txt_warden.Text = Convert.ToString(dv1[0]["staff_name"]);
                                    txt_department.Text = Convert.ToString(dv1[0]["dept_name"]);
                                    txt_designation.Text = Convert.ToString(dv1[0]["desig_name"]);
                                }
                            }
                            //  txt_warden.Text = Convert.ToString(warden);
                        }
                        warden1 = Convert.ToString(ds.Tables[0].Rows[0]["WardentStaff2PK"]);
                        ViewState["WardenCode1"] = Convert.ToString(warden1);
                        if (warden1 != "")
                        {
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                ds.Tables[1].DefaultView.RowFilter = "appl_id='" + warden1 + "'";
                                dv1 = ds.Tables[1].DefaultView;
                                if (dv1.Count > 0)
                                {
                                    txt_warden1.Text = Convert.ToString(dv1[0]["staff_name"]);
                                    txt_department1.Text = Convert.ToString(dv1[0]["dept_name"]);
                                    txt_designation1.Text = Convert.ToString(dv1[0]["desig_name"]);
                                }
                            }
                            //txt_warden1.Text = Convert.ToString(warden1);
                        }
                        building = Convert.ToString(ds.Tables[0].Rows[0]["HostelBuildingFK"]);
                        ViewState["BuildingCode"] = Convert.ToString(building);
                        if (building != "")
                        {
                            ds.Tables[2].DefaultView.RowFilter = "Code in (" + building + ")";
                            dv1 = ds.Tables[2].DefaultView;
                            if (dv1.Count > 0)
                            {
                                for (int row = 0; row < dv1.Count; row++)
                                {
                                    build1 = Convert.ToString(dv1[row]["Building_Name"]);
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
                            txt_building.Text = Convert.ToString(buildvalue1);
                        }
                        phone = Convert.ToString(ds.Tables[0].Rows[0]["PhoneNo"]);
                        {
                            txt_phone.Text = Convert.ToString(phone);
                        }
                        extension = Convert.ToString(ds.Tables[0].Rows[0]["PhoneExtNo"]);
                        if (extension != "")
                        {
                            txt_extension.Text = Convert.ToString(extension);
                        }
                        mobile = Convert.ToString(ds.Tables[0].Rows[0]["MobileNo"]);
                        if (mobile != "")
                        {
                            txt_mobile.Text = Convert.ToString(mobile);
                        }
                        email = Convert.ToString(ds.Tables[0].Rows[0]["EmailID"]);
                        if (email != "")
                        {
                            txt_email.Text = Convert.ToString(email);
                        }
                        roomrent = Convert.ToString(ds.Tables[0].Rows[0]["RoomRentLedgerFK"]);
                        if (roomrent.Trim() != "")
                        {
                            string headername = d2.GetFunction("select distinct h.HeaderName from FM_LedgerMaster l,FM_HeaderMaster h where l.LedgerPK='" + roomrent + "' and l.HeaderfK=h.HeaderPK");
                            ddl_rrh.SelectedIndex = ddl_rrh.Items.IndexOf(ddl_rrh.Items.FindByText(headername));
                            ddl_hosah();
                            string room = d2.GetFunction("select LedgerName from FM_LedgerMaster l,FM_HeaderMaster h where h.HeaderPK=l.HeaderFK and l.LedgerPK='" + roomrent + "'");
                            ddl_rrl.SelectedIndex = ddl_rrl.Items.IndexOf(ddl_rrl.Items.FindByText(room));



                        }
                        hosteladm = Convert.ToString(ds.Tables[0].Rows[0]["HostelAdmFeeLedgerFK"]);
                        if (hosteladm != "")
                        {
                            string addfeec = d2.GetFunction("select distinct h.HeaderName from FM_LedgerMaster l,FM_HeaderMaster h where l.LedgerPK='" + hosteladm + "' and l.HeaderfK=h.HeaderPK ");
                            ddl_hosteladdheader.SelectedIndex = ddl_hosteladdheader.Items.IndexOf(ddl_rrh.Items.FindByText(addfeec));
                            ddl_adjfeeleger();
                            string hosad = d2.GetFunction("select LedgerName from FM_LedgerMaster l,FM_HeaderMaster h where h.HeaderPK=l.HeaderFK and l.LedgerPK='" + hosteladm + "'");
                            ddl_hosteledger.SelectedIndex = ddl_hosteledger.Items.IndexOf(ddl_hosteledger.Items.FindByText(hosad));

                        }


                        studentadm = Convert.ToString(ds.Tables[0].Rows[0]["HostelAdmFeeAmount"]);
                        if (studentadm != "")
                        {
                            txt_studentledger.Enabled = false;
                            txt_studentledger.Text = Convert.ToString(studentadm);
                        }
                        messbill = Convert.ToString(ds.Tables[0].Rows[0]["NessBukkLedgerFK"]);
                        if (messbill != "")
                        {
                            string messbillhed = d2.GetFunction("select distinct h.HeaderName from FM_LedgerMaster l,FM_HeaderMaster h where l.LedgerPK='" + messbill + "' and l.HeaderfK=h.HeaderPK ");
                            ddl_messhed.SelectedIndex = ddl_messhed.Items.IndexOf(ddl_messhed.Items.FindByText(messbillhed));
                            messleg();
                            string mesbiu = d2.GetFunction("select LedgerName from FM_LedgerMaster l,FM_HeaderMaster h where h.HeaderPK=l.HeaderFK and l.LedgerPK='" + messbill + "'");
                            ddl_messbill.SelectedIndex = ddl_messbill.Items.IndexOf(ddl_messbill.Items.FindByText(mesbiu));


                        }
                        messbilldays = Convert.ToString(ds.Tables[0].Rows[0]["MessBillDSLedgerFK"]);
                        if (messbilldays != "")
                        {
                            string messbillhed = d2.GetFunction("select distinct h.HeaderName from FM_LedgerMaster l,FM_HeaderMaster h where l.LedgerPK='" + messbilldays + "' and l.HeaderfK=h.HeaderPK ");
                            ddl_messbillded.SelectedIndex = ddl_messbillded.Items.IndexOf(ddl_messbillded.Items.FindByText(messbillhed));
                            messbillleg();

                            string bida = d2.GetFunction("select LedgerName from FM_LedgerMaster l,FM_HeaderMaster h where h.HeaderPK=l.HeaderFK and l.LedgerPK='" + messbilldays + "'");
                            ddl_messdayscholar.SelectedIndex = ddl_messdayscholar.Items.IndexOf(ddl_messdayscholar.Items.FindByText(bida));

                        }
                        //messfee = Convert.ToString(ds.Tables[0].Rows[0]["Mess_FixedFeeAmt"]);
                        //if (messfee != "")
                        //{
                        //    //txt_messfee.Enabled = true;it
                        //    txt_messfee.Text = Convert.ToString(messfee);
                        //}
                        date = Convert.ToString(ds.Tables[0].Rows[0]["MessBillPayDueDays"]);
                        if (date != "")
                        {
                            ddl_days.SelectedIndex = ddl_days.Items.IndexOf(ddl_days.Items.FindByText(date));
                            //txt_duedate.Text = Convert.ToString(date);
                        }
                        else
                        {
                            ddl_days.SelectedIndex = ddl_days.Items.IndexOf(ddl_days.Items.FindByText(Convert.ToString(0)));
                        }

                        ddl_messmaster.SelectedIndex = ddl_messmaster.Items.IndexOf(ddl_messmaster.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["MessMasterFK"])));
                        string messms = Convert.ToString(ds.Tables[0].Rows[0]["MessMasterFK1"]);
                         string[] spl = messms.Split('-');
                         if (spl.Length > 0)
                         {
                             int cou = 0;
                             string typ = string.Empty;
                             if (spl.Count() > 0)
                             {
                                 for (int i = 0; i < spl.Count(); i++)
                                 {
                                    
                                         typ =spl[i]; 
                                         if (cbl_Mess.Items.Count > 0)
                                         {
                                             for (int j = 0; j < cbl_Mess.Items.Count; j++)
                                             {
                                                 if (Convert.ToString(cbl_Mess.Items[j].Value) == typ)
                                                 {
                                                     cou++;
                                                     cbl_Mess.Items[j].Selected = true;
                                                     txt_Mess.Text="mess"+cou+"";
                                                 }
                                             }
                                         }
                                     

                                 }

                             }
                         }



                        gatepass = Convert.ToString(ds.Tables[0].Rows[0]["HostelGatePassPerCount"]);
                        if (gatepass != "")
                        {
                            txt_gatepass.Text = Convert.ToString(gatepass);
                        }
                        //  cb_gatepass.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsHostelGatePassPer"]);
                        // cb_unappgatepass.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAllowUnApproveStud"]);

                        string vacate = "";
                        vacate = Convert.ToString(ds.Tables[0].Rows[0]["IsHostelGatePassPer"]);
                        if (vacate != "No")
                        {
                            cb_gatepass.Checked = true;
                            txt_gatepass.Enabled = true;
                        }
                        else
                        {
                            cb_gatepass.Checked = false;
                            txt_gatepass.Enabled = false;
                        }


                        string gatepassallow = "";
                        gatepassallow = Convert.ToString(ds.Tables[0].Rows[0]["IsAllowUnApproveStud"]);
                        if (gatepassallow != "No")
                        {
                            cb_unappgatepass.Checked = true;

                        }
                        else
                        {
                            cb_unappgatepass.Checked = false;

                        }

                        gender = Convert.ToString(ds.Tables[0].Rows[0]["HostelType"]);
                        {
                            if (gender == "Male")
                            {
                                rdb_male.Checked = true;
                                rdb_female.Checked = false;
                                rdb_both.Checked = false;
                            }
                            else if (gender == "Female")
                            {
                                rdb_female.Checked = true;
                                rdb_male.Checked = false;
                                rdb_both.Checked = false;
                            }
                            else if (gender == "Both")
                            {
                                rdb_both.Checked = true;
                                rdb_female.Checked = false;
                                rdb_male.Checked = false;
                            }
                        }
                        bill = Convert.ToString(ds.Tables[0].Rows[0]["MessBillType"]);
                        {
                            if (bill == "Fixed/Dividend")
                            {
                                txt_messfee.Enabled = false;
                                rdb_fixed.Checked = true;
                                rdb_div.Checked = true;

                                ddl_days.Enabled = false;
                                cb_rebate.Enabled = false;


                                rdb_fixedpur.Checked = false;
                                rdb_nondiv.Checked = false;

                            }
                            else if (bill == "Fixed/Nondividend")
                            {
                                rdb_fixed.Checked = true;
                                rdb_nondiv.Checked = true;


                                txt_duedate.Enabled = true;
                                ddl_days.Enabled = true;
                                cb_rebate.Enabled = true;

                                //txt_messfee.Enabled = true;
                                rdb_fixedpur.Checked = false;
                                rdb_div.Checked = false;

                                txt_messfee.Enabled = false;
                                rdb_monthly.Enabled = false;
                                rdb_yearly.Enabled = false;
                                rdb_sem.Enabled = false;

                            }
                            else if (bill == "Fixed & Additional Purchase/Dividend")
                            {
                                rdb_fixedpur.Checked = true;
                                rdb_div.Checked = true;
                            }
                            else if (bill == "Fixed & Additional Purchase/NonDividend")
                            {
                                rdb_fixedpur.Checked = true;
                                rdb_nondiv.Checked = true;
                                //txt_messfee.Enabled = true;
                                txt_duedate.Enabled = true;
                                ddl_days.Enabled = true;
                                cb_rebate.Enabled = true;
                            }
                        }
                        mbill = Convert.ToString(ds.Tables[0].Rows[0]["MessBillMethod"]);
                        {
                            if (mbill == "0")
                            {
                                rdb_div.Checked = true;
                                ddl_days.Enabled = false;
                                cb_rebate.Enabled = false;
                            }
                            else if (mbill == "1")
                            {
                                rdb_nondiv.Checked = true;
                                ddl_days.Enabled = true;
                                cb_rebate.Enabled = true;

                            }
                        }
                        ////pay = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Text;
                        //pay = Convert.ToString(ds.Tables[0].Rows[0]["Pay_Type"]);
                        //{
                        //    if (pay == "Monthly")
                        //    {
                        //        rdb_monthly.Checked = true;
                        //        rdb_yearly.Checked = false;
                        //        rdb_sem.Checked = false;
                        //    }
                        //    else if (pay == "Yearly")
                        //    {
                        //        rdb_yearly.Checked = true;
                        //        rdb_monthly.Checked = false;
                        //        rdb_sem.Checked = false;
                        //    }
                        //    else if (pay == "Semester")
                        //    {
                        //        rdb_sem.Checked = true;
                        //        rdb_monthly.Checked = false;
                        //        rdb_yearly.Checked = false;
                        //    }
                        //}

                        string duedays = Convert.ToString(ds.Tables[0].Rows[0]["MessBillPayDueDays"]);
                        string isrebate = Convert.ToString(ds.Tables[0].Rows[0]["IncludeRebate"]);
                        if (duedays.Trim() != "" && duedays.Trim() != "0")
                        {
                            ddl_days.SelectedIndex = Convert.ToInt32(duedays);
                        }
                        if (isrebate.Trim() == "0" || isrebate.Trim() == "False")
                        {
                            cb_rebate.Checked = false;
                        }
                        else
                        {
                            cb_rebate.Checked = true;
                        }


                        //ddl_college2.Enabled = false;
                        //cl = "select CollegeCode  from HM_HostelMaster where HostelName='" + txt_hostelname1.Text + "'";
                        //ds = d2.select_method_wo_parameter(cl, "text");
                        //if (ds.Tables[0].Rows.Count > 0)
                        //{
                        //    ddl_college2.SelectedIndex = ddl_college2.Items.IndexOf(ddl_college2.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["CollegeCode"])));
                        //}

                        //led = "select Room_FeeCode,DSMess_FeeCode,AdmnFeeCode,Mess_FeeCode from Hostel_Details where Hostel_Name='" + txt_hostelname1.Text + "'";
                        //ds = d2.select_method_wo_parameter(led, "Text");
                        //if (ds.Tables[0].Rows.Count > 0)
                        //{
                        //    dmess = Convert.ToString(ds.Tables[0].Rows[0]["DSMess_FeeCode"]);
                        //    mes = Convert.ToString(ds.Tables[0].Rows[0]["Mess_FeeCode"]);
                        //    ld = "Select Fee_Code,Fee_Type from Fee_Info F,Acctheader H,Acctinfo I WHERE F.Header_ID = H.Header_ID AND H.Acct_ID = I.Acct_ID AND fee = 1  and fee_code='" + dmess + "'  and fee_type not in ('Cash','Misc','Income & Expenditure') and fee_type not in (select bankname from bank_master1) order by fee_type";
                        //    ds = d2.select_method_wo_parameter(ld, "text");
                        //    if (ds.Tables[0].Rows.Count > 0)
                        //    {
                        //        n = ds.Tables[0].Rows[0]["Fee_Type"].ToString();
                        //        ddl_messbill.SelectedItem.Text = n;
                        //    }
                        //    lld = "Select Fee_Code,Fee_Type from Fee_Info F,Acctheader H,Acctinfo I WHERE F.Header_ID = H.Header_ID AND H.Acct_ID = I.Acct_ID AND fee = 1  and fee_code='" + mes + "'  and fee_type not in ('Cash','Misc','Income & Expenditure') and fee_type not in (select bankname from bank_master1) order by fee_type";
                        //    ds = d2.select_method_wo_parameter(lld, "text");
                        //    if (ds.Tables[0].Rows.Count > 0)
                        //    {
                        //        n = ds.Tables[0].Rows[0]["Fee_Type"].ToString();
                        //        ddl_messdayscholar.SelectedItem.Text = n;
                        //    }
                        //    exphone = "select AdmnFee,Extension_No,Gate_PerCount,Due_Date,Mess_FixedFeeAmt from Hostel_Details where Hostel_Name='" + txt_hostelname1.Text + "'";
                        //    ds = d2.select_method_wo_parameter(exphone, "Text");
                        //    if (ds.Tables[0].Rows.Count > 0)
                        //    {
                        //        messbill = ds.Tables[0].Rows[0]["Mess_FixedFeeAmt"].ToString();
                        //        string[] mess = messbill.Split('.');
                        //        messbill = mess[0].ToString();
                        //        txt_messfee.Text = messbill;
                        //        exphoneno = ds.Tables[0].Rows[0]["Extension_No"].ToString();
                        //        txt_extension.Text = exphoneno;
                        //        admnfeeno = ds.Tables[0].Rows[0]["AdmnFee"].ToString();
                        //        //   txtstudenadmission.Text = admnfeeno;
                        //        gatepass = ds.Tables[0].Rows[0]["Gate_PerCount"].ToString();
                        //        if (Convert.ToInt32(gatepass) != 0 || gatepass != "")
                        //        {
                        //            cb_gatepass.Checked = true;
                        //            txt_gatepass.Text = gatepass;
                        //            txt_gatepass.Enabled = true;
                        //        }
                        //        else
                        //        {
                        //            cb_gatepass.Checked = false;
                        //            txt_gatepass.Enabled = false;
                        //        }
                        //        duedate = ds.Tables[0].Rows[0]["Due_Date"].ToString();
                        //        string[] due;
                        //        due = duedate.Split(' ');
                        //        duedate = due[0].ToString();
                        //        string[] ddat = duedate.Split('/');
                        //        dudat = ddat[1].ToString() + "/" + ddat[0].ToString() + "/" + ddat[2].ToString();
                        //        txt_duedate.Text = dudat;
                        //    }
                        //    gateallow = "select case when Gate_AllowUnApprove='true' then 1 when Gate_AllowUnApprove='false' then 0 end Gate_AllowUnApprove from Hostel_Details where Hostel_Name='" + txt_hostelname1.Text + "'";
                        //    ds = d2.select_method_wo_parameter(gateallow, "Text");
                        //    if (ds.Tables[0].Rows.Count > 0)
                        //    {
                        //        gateallowapprove = ds.Tables[0].Rows[0]["Gate_AllowUnApprove"].ToString();
                        //        if (Convert.ToInt32(gateallowapprove) == 0)
                        //        {
                        //            cb_unappgatepass.Checked = false;
                        //        }
                        //        else if (Convert.ToInt32(gateallowapprove) == 1)
                        //        {
                        //            cb_unappgatepass.Checked = true;
                        //        }
                        //    }
                        //    popwindow.Visible = true;
                        //    div5.Visible = true;
                        //    div6.Visible = true;
                        //}
                    }
                }



                popwindow.Visible = true;
                div5.Visible = true;
                div6.Visible = true;
            }
            catch (Exception ex)
            {
            }
        }
    }
    public void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {

            string report = txt_excelname.Text;
            if (report.Trim() != "")
            {
                lbl_norec.Visible = false;
                //  FpSpread1.Sheets[0].Columns[1].Visible = false;
                d2.printexcelreport(FpSpread1, report);

            }
            else
            {
                lbl_norec.Text = "Please Enter Your Report Name";
                lbl_norec.Visible = true;
            }
            btn_Excel.Focus();
        }

        catch (Exception ex)
        {
            lbl_norec.Text = ex.ToString();
        }

    }
    public void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string date = "@" + "Date :" + System.DateTime.Now.ToString("dd/MM/yyy");
            if (cb_hostelname.Checked == true)
            {
                hostelname = "@" + " Hostel : " + cbl_hostelname.SelectedItem.ToString();
            }
            string pagename = "HostelMasterNew.aspx";
            string hosteldetails = "Hostel Master Report" + hostelname + date;
            Printcontrol.loadspreaddetails(FpSpread1, pagename, hosteldetails);
            Printcontrol.Visible = true;

        }
        catch (Exception ex)
        {
        }
    }

    //popupwindow1

    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }
    public void bindclgpop1()
    {
        try
        {
            ds.Clear();

            clgname = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(clgname, "Text");
            ddl_college1.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college1.DataSource = ds;
                ddl_college1.DataTextField = "collname";
                ddl_college1.DataValueField = "college_code";
                ddl_college1.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void rdb_div_CheckedChanged(object sender, EventArgs e)
    {
        if (rdb_div.Checked == true)
        {
            txt_messfee.Enabled = false;
            txt_duedate.Enabled = false;
            ddl_days.Enabled = false;
            cb_rebate.Enabled = false;
        }
        else if (rdb_nondiv.Checked == true)
        {
            // txt_messfee.Enabled = true;
            txt_duedate.Enabled = true;
            ddl_days.Enabled = true;
            cb_rebate.Enabled = true;

            txt_messfee.Enabled = false;
            rdb_monthly.Enabled = false;
            rdb_yearly.Enabled = false;
            rdb_sem.Enabled = false;
        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_name  from staffmaster where resign =0 and settled =0 and staff_name like  '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getstaffcode(string prefixText)
    {

        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code,staff_name from staffmaster where resign =0 and settled =0 and staff_code like  '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }

    protected void btn_warden_Click(object sender, EventArgs e)
    {
        try
        {
            checkvalue = "wardenvalue";
            popupsscode1.Visible = true;
            btn_save1.Visible = false;
            btn_exit2.Visible = false;
            Fpstaff.Visible = false;
            ddl_college2.Enabled = true;
            bindcollege();
            binddepartment();
            lbl_errorsearch.Visible = false;
            txt_searchby.Text = "";
            txt_wardencode.Text = "";
            //btn_go2_Click(sender, e);
        }
        catch
        {
        }
    }
    protected void btn_warden1_Click(object sender, EventArgs e)
    {
        try
        {
            checkvalue = "wardenvalue1";
            popupsscode1.Visible = true;
            btn_save1.Visible = false;
            btn_exit2.Visible = false;
            Fpstaff.Visible = false;
            ddl_college2.Enabled = true;
            bindcollege();
            binddepartment();
            lbl_errorsearch.Visible = false;
            txt_searchby.Text = "";
            txt_wardencode.Text = "";
            //btn_go2_Click(sender, e);
        }
        catch
        {
        }
    }
    protected void btn_building_click(object sender, EventArgs e)
    {
        try
        {
            txt_building1.Text = "";
            lbl_error3.Visible = false;
            popupbuild1.Visible = true;
            fpbuild.Visible = true;
            btn_go3_Click(sender, e);
        }
        catch
        {
        }
    }
    protected void ddl_hosteledger_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddl_hosteledger.SelectedItem.Text == "Select")
            {
                txt_studentledger.Enabled = false;
            }
            else
            {
                txt_studentledger.Enabled = true;


            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void binddays()
    {
        ddl_days.Items.Clear();
        for (int i = 1; i <= 31; i++)
        {
            ddl_days.Items.Add(Convert.ToString(i));

        }
        ddl_days.Items.Insert(0, "Select");
    }

    protected void cb_rebate_CheckedChanged(object sender, EventArgs e)
    {

    }

    protected void cb_gatepass_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_gatepass.Checked == true)
        {
            txt_gatepass.Enabled = true;
        }
        else
        {
            txt_gatepass.Enabled = false;
        }
    }

    protected void savedetails()
    {
        try
        {
            int s;
            string[] splitdate;
            DateTime dt = new DateTime();
            hostelname = Convert.ToString(txt_hostelname1.Text);
            hostelname = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(hostelname);

            if (rdb_male.Checked == true)
            {
                gender = "1";
            }
            else if (rdb_female.Checked == true)
            {
                gender = "2";
            }
            else if (rdb_both.Checked == true)
            {
                gender = "0";
            }
            warden = Convert.ToString(ViewState["WardenCode"]);
            warden1 = Convert.ToString(ViewState["WardenCode1"]);
            building = Convert.ToString(ViewState["BuildingCode"]);

            phone = Convert.ToString(txt_phone.Text);
            extension = Convert.ToString(txt_extension.Text);
            mobile = Convert.ToString(txt_mobile.Text);
            email = Convert.ToString(txt_email.Text);
            roomrent = Convert.ToString(ddl_rrl.SelectedItem.Text);
            if (roomrent.Trim() == "Select")
            {
                roomrent = "0";
            }
            else
            {
                roomrent = Convert.ToString(ddl_rrl.SelectedItem.Value);
            }

            roomrentheader = Convert.ToString(ddl_rrh.SelectedItem.Text);
            if (roomrentheader.Trim() == "Select")
            {
                roomrentheader = "0";
            }
            else
            {
                roomrentheader = Convert.ToString(ddl_rrh.SelectedItem.Value);
            }
            hosteladm = Convert.ToString(ddl_hosteledger.SelectedItem.Text);
            if (hosteladm.Trim() == "Select")
            {
                hosteladm = "0";
                txt_studentledger.Text = "";
                txt_studentledger.Enabled = false;
            }
            else
            {
                hosteladm = Convert.ToString(ddl_hosteledger.SelectedItem.Value);
            }
            studentadm = Convert.ToString(txt_studentledger.Text);

            hosteladmheader = Convert.ToString(ddl_hosteladdheader.SelectedItem.Text);
            if (hosteladmheader.Trim() == "Select")
            {
                hosteladmheader = "0";

            }
            else
            {
                hosteladmheader = Convert.ToString(ddl_hosteladdheader.SelectedItem.Value);
            }
            if (studentadm.Trim() == "")
            {
                studentadm = "0";
            }
            if (rdb_fixed.Checked == true)
            {
                bill = "0";
            }
            else if (rdb_fixedpur.Checked == true)
            {
                bill = "1";
            }
            if (rdb_div.Checked == true)
            {
                txt_messfee.Text = "0";
                txt_messfee.Enabled = false;
                mbill = "0";
            }
            else if (rdb_nondiv.Checked == true)
            {
                //txt_messfee.Enabled = true;
                mbill = "1";
            }
            messbill = Convert.ToString(ddl_messbill.SelectedItem.Text);
            if (messbill.Trim() == "Select")
            {
                messbill = "0";
            }
            else
            {
                messbill = Convert.ToString(ddl_messbill.SelectedItem.Value);
            }

            messbillheaderr = Convert.ToString(ddl_messhed.SelectedItem.Text);
            if (messbillheaderr.Trim() == "Select")
            {
                messbillheaderr = "0";
            }
            else
            {
                messbillheaderr = Convert.ToString(ddl_messhed.SelectedItem.Value);
            }
            if (rdb_monthly.Checked == true)
            {
                pay = "1";
            }
            else if (rdb_yearly.Checked == true)
            {
                pay = "2";
            }
            else if (rdb_sem.Checked == true)
            {
                pay = "3";
            }
            string dudate = "";
            string isrebate = "0";
            if (rdb_nondiv.Checked == true)
            {
                messfee = Convert.ToString(txt_messfee.Text);
                date = Convert.ToString(ddl_days.SelectedItem.Text);
                if (date.Trim() != "Select")
                {
                    dudate = date;
                }
                else
                {
                    dudate = "0";
                }
                if (cb_rebate.Checked == true)
                {
                    isrebate = "1";
                }
                else
                {
                    isrebate = "0";
                }
            }
            else
            {
                dudate = "0";
                messfee = "0";
            }
            messbilldays = Convert.ToString(ddl_messdayscholar.SelectedItem.Text);
            if (messbilldays.Trim() == "Select")
            {
                messbilldays = "0";
            }
            else
            {
                messbilldays = Convert.ToString(ddl_messdayscholar.SelectedItem.Value);
            }

            messbillheader = Convert.ToString(ddl_messbillded.SelectedItem.Text);
            if (messbillheader.Trim() == "Select")
            {
                messbillheader = "0";
            }
            else
            {
                messbillheader = Convert.ToString(ddl_messbillded.SelectedItem.Value);
            }
            gatepass = Convert.ToString(txt_gatepass.Text);

            if (cb_gatepass.Checked == true)
            {
                Gate_per = "1";
            }
            else
            {
                txt_gatepass.Text = "";
                txt_gatepass.Enabled = false;
                Gate_per = "0";
            }
            if (cb_unappgatepass.Checked == true)
            {
                Gatepassunapprove = "1";
            }
            else
            {
                Gatepassunapprove = "0";
            }
            if (messfee.Trim() == "")
            {
                messfee = "0";
            }
         
            string messname = "";
            string singlemessname = "";
            if (cbl_Mess.Items.Count > 0)
            {
                for (int i = 0; i < cbl_Mess.Items.Count; i++)
                {
                    if (cbl_Mess.Items[i].Selected == true)
                    {
                        if (messname == "")
                        {
                            messname = "" + cbl_Mess.Items[i].Value+ "";
                            singlemessname = "" + cbl_Mess.Items[i].Value + "";
                        }
                        else
                        {
                            messname = messname + ""+ "-" + "" + cbl_Mess.Items[i].Value + "";
                        }
                    }
                }
            }
            //messname = Convert.ToString(ddl_messmaster.SelectedItem.Text);
            //if (messname.Trim() == "Select")
            //{
            //    messname = "0";
            //}
            //else
            //{
            //    messname = Convert.ToString(ddl_messmaster.SelectedItem.Value);
            //}

            if (btn_save.Text == "Save")
            {
                query = "INSERT INTO HM_HostelMaster(HostelName,HostelType,WardenStaff1PK,WardentStaff2PK,HostelBuildingFK,PhoneNo,PhoneExtNo,MobileNo,EmailID,RoomRentLedgerFK,HostelAdmFeeLedgerFK,HostelAdmFeeAmount,MessBillType,MessBillMethod,MessBillPayDueDays,MessBillDSLedgerFK,IsHostelGatePassPer ,HostelGatePassPerCount,IsAllowUnApproveStud,IncludeRebate,NessBukkLedgerFK,MessBillDSHeaderFK,RoomRentHeaderFK,HostelAdmFeeHeaderFK,MessBillHeaderFK,MessMasterFK1,messmasterfk)VALUES('" + hostelname + "','" + gender + "','" + warden + "','" + warden1 + "','" + building + "','" + phone + "', '" + extension + "','" + mobile + "','" + email + "','" + roomrent + "','" + hosteladm + "','" + studentadm + "','" + bill + "','" + mbill + "','" + dudate + "','" + messbilldays + "','" + Gate_per + "','" + gatepass + "','" + Gatepassunapprove + "','" + isrebate + "','" + messbill + "','" + messbillheader + "','" + roomrentheader + "','" + hosteladmheader + "','" + messbillheaderr + "','" + messname + "','" + singlemessname + "')";
                //'" + messbill + "','" + pay + "','" + messfee + "',ddl_college1 ,'" + ddl_college1.SelectedItem.Value + "'
                s = d2.update_method_wo_parameter(query, "Text");
                imgdiv2.Visible = true;
                lbl_alerterror.Text = "Saved Successfully";
                btn_addnew_Click(sender, e);
                bindhostel();
                btn_go_Click(sender, e);

            }
            if (btn_save.Text == "Update")
            {
                query = "update HM_HostelMaster set HostelName='" + hostelname + "',HostelType='" + gender + "',WardenStaff1PK='" + warden + "',WardentStaff2PK='" + warden1 + "',HostelBuildingFK='" + building + "',PhoneNo='" + phone + "',PhoneExtNo='" + extension + "',MobileNo='" + mobile + "',EmailID='" + email + "',RoomRentLedgerFK='" + roomrent + "',HostelAdmFeeLedgerFK='" + hosteladm + "',HostelAdmFeeAmount='" + studentadm + "',MessBillType='" + bill + "',MessBillMethod='" + mbill + "',MessBillPayDueDays='" + dudate + "',MessBillDSLedgerFK='" + messbilldays + "',IsHostelGatePassPer='" + Gate_per + "' ,HostelGatePassPerCount='" + gatepass + "',IsAllowUnApproveStud='" + Gatepassunapprove + "',IncludeRebate='" + isrebate + "',NessBukkLedgerFK='" + messbill + "',MessBillDSHeaderFK='" + messbillheader + "',RoomRentHeaderFK='" + roomrentheader + "',HostelAdmFeeHeaderFK='" + hosteladmheader + "',MessBillHeaderFK='" + messbillheaderr + "',MessMasterFK1 ='" + messname + "',messmasterfk ='" + singlemessname+ "' where  HostelMasterPK ='" + Convert.ToString(ViewState["Hostel_NewCode"]) + "'";// CollegeCode='" + ddl_college1.SelectedItem.Value + "'
                s = d2.update_method_wo_parameter(query, "Text");
                popwindow.Visible = false;
                bindhostel();
                btn_go_Click(sender, e);
                imgdiv2.Visible = true;
                lbl_alerterror.Text = "Updated Successfully";
            }
        }
        catch
        {
        }
    }
    protected void but_save_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_warden.Text != "" || txt_warden1.Text != "")
            {
                checkwardanname();
                checkwardanname1();
                if (war == true)
                {
                    savedetails();
                    ViewState["BuildingCode"] = null;
                    ViewState["WardenCode"] = null;
                    ViewState["WardenCode1"] = null;
                }
                else if (war1 == true)
                {
                    savedetails();
                    ViewState["BuildingCode"] = null;
                    ViewState["WardenCode"] = null;
                    ViewState["WardenCode1"] = null;
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alerterror.Visible = true;
                    lbl_alerterror.Text = "Please select correct warden name";
                    txt_warden.Text = "";
                    txt_warden1.Text = "";
                }
            }
            else
            {
                savedetails();
                ViewState["BuildingCode"] = null;
                ViewState["WardenCode"] = null;
                ViewState["WardenCode1"] = null;
            }

        }
        catch { }



    }
    protected void btn_exit_Click(object sender, EventArgs e)
    {
        try
        {
            popwindow.Visible = false;
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
                lbl_sure.Text = "Do you want to Delete this Record?";
            }
        }
        catch
        {
        }
    }
    protected void delete()
    {
        try
        {
            surediv.Visible = false;
            string hostelcode = "";
            string del = "";

            hostelcode = Convert.ToString(ViewState["Hostel_NewCode"]);
            del = "delete from HM_HostelMaster where HostelMasterPK='" + hostelcode + "'";
            y = d2.update_method_wo_parameter(del, "Text");
            if (y != 0)
            {
                ds.Clear();
                bindhostel();
                btn_go_Click(sender, e);
                popwindow.Visible = false;
                imgdiv2.Visible = true;
                lbl_alerterror.Text = "Deleted Successfully";
            }
        }
        catch (Exception ex)
        {
            popwindow.Visible = false;
            imgdiv2.Visible = true;
            lbl_alerterror.Text = "Can't Deleted the Hostel. Because some student is there.";
        }
    }
    protected void btn_update_Click(object sender, EventArgs e)
    {
        try
        {
            savedetails();
        }
        catch (Exception ex)
        {
        }
    }

    //popup window2 select staff code

    protected void ddl_searchby_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_searchby.SelectedValue == "0")
        {
            txt_searchby.Visible = true;
            txt_wardencode.Visible = false;
            txt_wardencode.Text = "";
        }
        else if (ddl_searchby.SelectedValue == "1")
        {
            txt_searchby.Visible = false;
            txt_searchby.Text = "";
            txt_wardencode.Visible = true;
        }
        btn_go2_Click(sender, e);
    }
    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {
        popupsscode1.Visible = false;
    }
    protected void btn_save1_Click(object sender, EventArgs e)
    {
        try
        {
            string dept = "";
            string desg = "";
            string wardencode = "";
            string wardencodee = "";
            string wardencode1 = "";
            string wardencodeee = "";
            string activerow = "";
            string activecol = "";
            if (Fpstaff.Sheets[0].RowCount != 0)
            {
                activerow = Fpstaff.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpstaff.ActiveSheetView.ActiveColumn.ToString();
                if (activerow != Convert.ToString(-1))
                {
                    if (checkvalue == "wardenvalue")
                    {
                        if (txt_searchby.Text == "" || txt_searchby.Text != "")
                        {
                            name = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                            txt_warden.Text = name;
                            dept = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                            txt_department.Text = dept;
                            desg = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                            txt_designation.Text = desg;
                            wardencode = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                            wardencodee = d2.GetFunction("select appl_id  from staffmaster sm,staff_appl_master a where sm.appl_no =a.appl_no and sm.staff_code='" + wardencode + "'");

                        }
                        ViewState["WardenCode"] = Convert.ToString(wardencodee);
                    }
                    if (checkvalue == "wardenvalue1")
                    {
                        if (txt_searchby.Text == "" || txt_searchby.Text != "")
                        {
                            name = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                            txt_warden1.Text = name;
                            dept = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                            txt_department1.Text = dept;
                            desg = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                            txt_designation1.Text = desg;
                            wardencode1 = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                            wardencodeee = d2.GetFunction("select appl_id  from staffmaster sm,staff_appl_master a where sm.appl_no =a.appl_no and sm.staff_code='" + wardencode1 + "'");
                        }
                        ViewState["WardenCode1"] = Convert.ToString(wardencodeee);
                    }
                    popupsscode1.Visible = false;
                }
                else
                {
                    lbl_errorsearch.Visible = true;
                    lbl_errorsearch.Text = "Please Select Any One Staff";
                }
            }
            else
            {
                lbl_errorsearch1.Visible = true;
                lbl_errorsearch1.Text = "No Records Found";
                Fpstaff.Visible = false;
            }
            if (txt_warden.Text == txt_warden1.Text)
            {
                txt_department1.Text = "";
                txt_designation1.Text = "";
                txt_warden1.Text = "";
                imgdiv2.Visible = true;
                lbl_alerterror.Text = "Please Select the Another Staff Name";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_exit2_Click(object sender, EventArgs e)
    {
        try
        {
            popupsscode1.Visible = false;
        }
        catch
        {
        }
    }
    protected void btn_go2_Click(object sender, EventArgs e)
    {
        try
        {
            int rolcount = 0;
            int sno = 0;
            if (txt_searchby.Text != "")
            {
                if (ddl_searchby.SelectedIndex == 0)
                {
                    sql = "select s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and s.Staff_name ='" + Convert.ToString(txt_searchby.Text) + "' order by s.staff_code";
                }
            }
            else if (txt_wardencode.Text.Trim() != "")
            {
                if (ddl_searchby.SelectedIndex == 1)
                {
                    sql = "select s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and s.staff_code ='" + Convert.ToString(txt_wardencode.Text) + "' order by s.staff_code";
                }
            }
            else
            {
                sql = "select s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and h.dept_code in ('" + ddl_department3.SelectedItem.Value + "') order by s.staff_code";
            }
            Fpstaff.Sheets[0].RowCount = 0;
            Fpstaff.SaveChanges();
            Fpstaff.SheetCorner.ColumnCount = 0;

            Fpstaff.CommandBar.Visible = false;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fpstaff.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            Fpstaff.Sheets[0].RowCount = Fpstaff.Sheets[0].RowCount + 1;
            Fpstaff.Sheets[0].SpanModel.Add(Fpstaff.Sheets[0].RowCount - 1, 0, 1, 3);
            Fpstaff.Sheets[0].AutoPostBack = false;
            ds = d2.select_method_wo_parameter(sql, "Text");
            Fpstaff.Sheets[0].RowCount = 0;
            Fpstaff.Sheets[0].ColumnCount = 5;

            if (ds.Tables[0].Rows.Count > 0)
            {

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[0].Locked = true;
                Fpstaff.Columns[0].Width = 80;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[1].Locked = true;
                Fpstaff.Columns[1].Width = 100;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[2].Locked = true;
                Fpstaff.Columns[2].Width = 200;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[3].Locked = true;
                Fpstaff.Columns[3].Width = 250;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Designation";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpstaff.Columns[4].Width = 200;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[4].Locked = true;
                Fpstaff.Width = 846;

                for (rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                {
                    sno++;
                    //Fpstaff.Sheets[0].RowCount++;
                    //name = ds.Tables[0].Rows[rolcount]["staff_name"].ToString();
                    //code = ds.Tables[0].Rows[rolcount]["staff_code"].ToString();

                    Fpstaff.Sheets[0].RowCount = Fpstaff.Sheets[0].RowCount + 1;
                    //Fpstaff.Sheets[0].Rows[Fpstaff.Sheets[0].RowCount - 1].Font.Bold = false;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["staff_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["staff_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                }
                Fpstaff.Visible = true;
                btn_save1.Visible = true;
                btn_exit2.Visible = true;

                lbl_errorsearch.Visible = true;
                lbl_errorsearch.Text = "No Records Found";
                lbl_errorsearch.Text = "No of Staff :" + sno.ToString();
                rowcount = Fpstaff.Sheets[0].RowCount;
                Fpstaff.Height = 370;
                Fpstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
                Fpstaff.SaveChanges();
                err.Visible = false;
            }
            else
            {
                Fpstaff.Visible = false;
                lbl_errorsearch.Visible = false;
                btn_save1.Visible = false;
                btn_exit2.Visible = false;
                err.Visible = true;
                err.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void bindcollege()
    {
        try
        {
            ds.Clear();

            clgname = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(clgname, "Text");
            ddl_college2.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college2.DataSource = ds;
                ddl_college2.DataTextField = "collname";
                ddl_college2.DataValueField = "college_code";
                ddl_college2.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void binddepartment()
    {
        ds.Clear();
        //query = "";
        //query = "select distinct dept_name,dept_code from hrdept_master where college_code='" + ddl_college2.SelectedValue.ToString() + "'";
        ds = d2.loaddepartment(ddl_college2.SelectedValue.ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_department3.DataSource = ds;
            ddl_department3.DataTextField = "Dept_Name";
            ddl_department3.DataValueField = "Dept_Code";
            ddl_department3.DataBind();
            //ddl_department3.Items.Insert(0, "All");
        }
    }

    //popup window3 building name

    protected void imagebtnpopclose3_Click(object sender, EventArgs e)
    {
        popupbuild1.Visible = false;
    }

    // 17.12.15 add
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getbuilding(string prefixText)
    {

        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "SELECT distinct Building_Name FROM Building_Master where  Building_Name like  '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }

    protected void btn_go3_Click(object sender, EventArgs e)
    {
        try
        {
            int sno = 0;
            lbl_error3.Visible = false;
            fpbuild.SaveChanges();
            fpbuild.SheetCorner.ColumnCount = 0;
            fpbuild.Sheets[0].RowCount = 0;
            fpbuild.Sheets[0].ColumnCount = 0;
            fpbuild.Sheets[0].ColumnCount = 4;
            fpbuild.CommandBar.Visible = false;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            fpbuild.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            fpbuild.Sheets[0].AutoPostBack = false;

            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
            cb.AutoPostBack = true;

            fpbuild.Sheets[0].RowCount++;
            fpbuild.Sheets[0].Cells[fpbuild.Sheets[0].RowCount - 1, 1].CellType = cb;//cb select all
            fpbuild.Sheets[0].Cells[fpbuild.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

            sql = "SELECT Code,Building_Name FROM Building_Master where (Building_Name like'" + txt_building1.Text + "%')";
            ds = d2.select_method_wo_parameter(sql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                fpbuild.Visible = true;
                btn_ok.Visible = true;
                btn_exit3.Visible = true;

                fpbuild.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                fpbuild.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                fpbuild.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                fpbuild.Sheets[0].Columns[0].Width = 60;
                fpbuild.Sheets[0].Columns[0].Locked = true;

                fpbuild.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                fpbuild.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                fpbuild.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                fpbuild.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Code";
                fpbuild.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                fpbuild.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                fpbuild.Sheets[0].Columns[2].Locked = true;
                fpbuild.Sheets[0].Columns[2].Width = 60;

                fpbuild.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Building Name";
                fpbuild.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                fpbuild.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                fpbuild.Sheets[0].Columns[3].Locked = true;
                fpbuild.Sheets[0].Columns[3].Width = 130;

                fpbuild.Width = 360;

                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    code = ds.Tables[0].Rows[i]["Code"].ToString();
                    name = ds.Tables[0].Rows[i]["Building_Name"].ToString();

                    fpbuild.Sheets[0].RowCount = fpbuild.Sheets[0].RowCount + 1;
                    fpbuild.Sheets[0].Rows[fpbuild.Sheets[0].RowCount - 1].Font.Bold = false;
                    fpbuild.Sheets[0].Cells[fpbuild.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    fpbuild.Sheets[0].Cells[fpbuild.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                    fpbuild.Sheets[0].Cells[fpbuild.Sheets[0].RowCount - 1, 1].CellType = chkcell1;
                    fpbuild.Sheets[0].Cells[fpbuild.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    fpbuild.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                    chkcell1.AutoPostBack = false;

                    fpbuild.Sheets[0].Cells[fpbuild.Sheets[0].RowCount - 1, 2].Text = code;
                    fpbuild.Sheets[0].Cells[fpbuild.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;

                    fpbuild.Sheets[0].Cells[fpbuild.Sheets[0].RowCount - 1, 3].Text = name;
                    fpbuild.Sheets[0].Cells[fpbuild.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                }
                rowcount = fpbuild.Sheets[0].RowCount;
                fpbuild.Height = 170;
                fpbuild.Sheets[0].PageSize = 15 + (rowcount * 5);
                fpbuild.SaveChanges();
            }
            else
            {
                fpbuild.Visible = false;
                btn_ok.Visible = false;
                btn_exit3.Visible = false;
                lbl_error3.Visible = true;
                lbl_error3.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void fpbuild_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {

            string actrow = fpbuild.Sheets[0].ActiveRow.ToString();
            string actcol = fpbuild.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (fpbuild.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(fpbuild.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < fpbuild.Sheets[0].RowCount; i++)
                        {
                            fpbuild.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < fpbuild.Sheets[0].RowCount; i++)
                        {
                            fpbuild.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }

        }
        catch
        {

        }
    }
    protected void btn_ok_Click(object sender, EventArgs e)
    {
        try
        {
            y = 0;
            int isval = 0;
            string value = "";
            string builcode = "";
            txt_building1.Text = "";
            fpbuild.SaveChanges();
            for (i = 0; i < fpbuild.Rows.Count; i++)
            {
                isval = Convert.ToInt32(fpbuild.Sheets[0].Cells[i, 1].Value);
                if (isval == 1)
                {
                    if (value == "")
                    {
                        value = fpbuild.Sheets[0].Cells[i, 3].Text;
                        builcode = Convert.ToString(fpbuild.Sheets[0].Cells[i, 2].Text);
                    }
                    else
                    {
                        value = value + ',' + fpbuild.Sheets[0].Cells[i, 3].Text;
                        builcode = builcode + "," + Convert.ToString(fpbuild.Sheets[0].Cells[i, 2].Text);
                    }
                    y = 1;
                }
            }
            if (y == 1)
            {
                txt_building.Text = value;
                ViewState["BuildingCode"] = Convert.ToString(builcode);
                popupbuild1.Visible = false;
            }
            else
            {
                lbl_error3.Visible = true;
                lbl_error3.Text = "Please Select Any One Building Name";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_exit3_Click(object sender, EventArgs e)
    {
        try
        {
            popupbuild1.Visible = false;
        }
        catch
        {
        }
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        imgdiv2.Visible = false;
        popwindow.Visible = true;
    }
    //12.10.15
    public EventArgs e { get; set; }
    public object sender { get; set; }


    [WebMethod]
    public static string CheckUserName(string HostelName)
    {
        string returnValue = "1";
        try
        {
            DAccess2 dd = new DAccess2();
            string user_name = HostelName;
            if (user_name.Trim() != "" && user_name != null)
            {
                string query = dd.GetFunction("select distinct HostelName from HM_HostelMaster where HostelName ='" + user_name + "'");
                if (query.Trim() == "" || query == null || query == "0" || query == "-1")
                {
                    returnValue = "0";
                }
            }
            else
            {
                returnValue = "2";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }

    //13.10.15
    protected void checkwardanname()
    {
        string staffname = d2.GetFunction("select staff_name  from staffmaster where resign =0 and settled =0 and staff_name='" + txt_warden.Text + "'");

        if (staffname.Trim() != "0" && staffname.Trim() != "")
        {
            war = true;
        }
        else
        {
            war = false;
        }

    }
    protected void checkwardanname1()
    {
        string staffname = d2.GetFunction("select staff_name  from staffmaster where resign =0 and settled =0 and staff_name='" + txt_warden1.Text + "'");

        if (staffname.Trim() != "0" && staffname.Trim() != "")
        {
            war1 = true;
        }
        else
        {
            war1 = false;
        }
    }
    protected void wardendeg(object sender, EventArgs e)
    {
        string wardencode = "";
        string wardencodee = "";
        string q1 = "select s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code  and s.staff_name ='" + txt_warden.Text + "' order by s.staff_code";
        ds.Clear();
        ds = d2.select_method_wo_parameter(q1, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            txt_department.Text = Convert.ToString(ds.Tables[0].Rows[0]["dept_name"].ToString());
            txt_designation.Text = Convert.ToString(ds.Tables[0].Rows[0]["desig_name"].ToString());

            wardencode = Convert.ToString(ds.Tables[0].Rows[0]["staff_code"].ToString());
            wardencodee = d2.GetFunction("select appl_id  from staffmaster sm,staff_appl_master a where sm.appl_no =a.appl_no and sm.staff_code='" + wardencode + "'");

            ViewState["WardenCode"] = Convert.ToString(wardencodee);
        }
        else
        {
            txt_department.Text = "";
            txt_designation.Text = "";
            txt_warden.Text = "";
        }
    }
    protected void wardendeg1(object sender, EventArgs e)
    {
        string wardencode1 = "";
        string wardencodeee = "";
        string q1 = "select s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code  and s.staff_name ='" + txt_warden1.Text + "' order by s.staff_code";
        ds.Clear();
        ds = d2.select_method_wo_parameter(q1, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            txt_department1.Text = Convert.ToString(ds.Tables[0].Rows[0]["dept_name"].ToString());
            txt_designation1.Text = Convert.ToString(ds.Tables[0].Rows[0]["desig_name"].ToString());

            wardencode1 = Convert.ToString(ds.Tables[0].Rows[0]["staff_code"].ToString());
            wardencodeee = d2.GetFunction("select appl_id  from staffmaster sm,staff_appl_master a where sm.appl_no =a.appl_no and sm.staff_code='" + wardencode1 + "'");

            ViewState["WardenCode1"] = Convert.ToString(wardencodeee);

            // ViewState["WardenCode1"] = Convert.ToString(ds.Tables[0].Rows[0]["staff_code"].ToString());
        }
        else
        {
            txt_department1.Text = "";
            txt_designation1.Text = "";
            txt_warden1.Text = "";
        }
        if (txt_warden.Text == txt_warden1.Text)
        {
            txt_department1.Text = "";
            txt_designation1.Text = "";
            txt_warden1.Text = "";
            imgdiv2.Visible = true;
            lbl_alerterror.Text = "Please Select the Another Staff Name";
        }
    }
    public void bindrrh()
    {
        try
        {
            ds1.Clear();

            clgname = "select HeaderPK,HeaderName from FM_HeaderMaster where CollegeCode ='" + collegecode1 + "'";
            ds1 = d2.select_method_wo_parameter(clgname, "Text");
            ddl_rrh.Items.Clear();
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ddl_rrh.DataSource = ds1;
                ddl_rrh.DataTextField = "HeaderName";
                ddl_rrh.DataValueField = "HeaderPK";
                ddl_rrh.DataBind();
                ddl_rrh.Items.Insert(0, "Select");

                ddl_hosteladdheader.DataSource = ds1;
                ddl_hosteladdheader.DataTextField = "HeaderName";
                ddl_hosteladdheader.DataValueField = "HeaderPK";
                ddl_hosteladdheader.DataBind();
                ddl_hosteladdheader.Items.Insert(0, "Select");

                ddl_messbillded.DataSource = ds1;
                ddl_messbillded.DataTextField = "HeaderName";
                ddl_messbillded.DataValueField = "HeaderPK";
                ddl_messbillded.DataBind();
                ddl_messbillded.Items.Insert(0, "Select");

                ddl_messhed.DataSource = ds1;
                ddl_messhed.DataTextField = "HeaderName";
                ddl_messhed.DataValueField = "HeaderPK";
                ddl_messhed.DataBind();
                ddl_messhed.Items.Insert(0, "Select");
            }
            else
            {
                ddl_rrh.Items.Insert(0, "Select");
                ddl_hosteladdheader.Items.Insert(0, "Select");
                ddl_messbillded.Items.Insert(0, "Select");
                ddl_messhed.Items.Insert(0, "Select");
            }
        }

        catch (Exception ex)
        {
        }
    }
    public void ddl_hosah()
    {
        try
        {
            ds1.Clear();
            clgname = "select LedgerPK,LedgerName from FM_LedgerMaster where CollegeCode ='" + collegecode1 + "' and HeaderFK ='" + ddl_rrh.SelectedItem.Value + "' and LedgerMode =0";
            ds1 = d2.select_method_wo_parameter(clgname, "Text");
            ddl_rrl.Items.Clear();
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ddl_rrl.DataSource = ds1;
                ddl_rrl.DataTextField = "LedgerName";
                ddl_rrl.DataValueField = "LedgerPK";
                ddl_rrl.DataBind();
            }
            else
            {
                ddl_rrl.Items.Insert(0, "Select");
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddl_rrh_Selectedindex_Changed(object sender, EventArgs e)
    {
        if (ddl_rrh.SelectedItem.Text.Trim() != "Select")
        {
            // bindroomrent();  
            ddl_hosah();
            //ddl_rrl.Items.Insert(0, "Select");
        }
        else
        {
            ddl_rrl.Items.Insert(0, "Select");
        }
    }
    protected void ddl_hosteladjexe_Selectedindex_Changed(object sender, EventArgs e)
    {
        if (ddl_hosteladdheader.SelectedItem.Text.Trim() != "Select")
        {
            // bindhostelfee();   
            ddl_adjfeeleger();

        }
        else
        {
            ddl_hosteledger.Items.Insert(0, "Select");
        }
    }
    protected void ddl_messbillhedSelectedindex_Changed(object sender, EventArgs e)
    {
        if (ddl_messbillded.SelectedItem.Text.Trim() != "Select")
        {
            messbillleg();
        }
        else
        {
            ddl_messbillded.Items.Insert(0, "Select");
        }
    }
    protected void ddl_messhedSelectedindex_Changed(object sender, EventArgs e)
    {
        if (ddl_messhed.SelectedItem.Text.Trim() != "Select")
        {
            messleg();
        }
        else
        {
            ddl_messhed.Items.Insert(0, "Select");
        }
    }
    public void ddl_adjfeeleger()
    {
        try
        {
            ds1.Clear();

            clgname = "select LedgerPK,LedgerName from FM_LedgerMaster where CollegeCode ='" + collegecode1 + "' and HeaderFK ='" + ddl_hosteladdheader.SelectedItem.Value + "' and LedgerMode =0";
            ds1 = d2.select_method_wo_parameter(clgname, "Text");
            ddl_hosteledger.Items.Clear();
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ddl_hosteledger.DataSource = ds1;
                ddl_hosteledger.DataTextField = "LedgerName";
                ddl_hosteledger.DataValueField = "LedgerPK";
                ddl_hosteledger.DataBind();

            }
            else
            {
                ddl_hosteledger.Items.Insert(0, "Select");
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void messbillleg()
    {
        try
        {
            ds1.Clear();
            clgname = "select LedgerPK,LedgerName from FM_LedgerMaster where CollegeCode ='" + collegecode1 + "' and HeaderFK ='" + ddl_messbillded.SelectedItem.Value + "' and LedgerMode =0";
            ds1 = d2.select_method_wo_parameter(clgname, "Text");
            ddl_messdayscholar.Items.Clear();
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ddl_messdayscholar.DataSource = ds1;
                ddl_messdayscholar.DataTextField = "LedgerName";
                ddl_messdayscholar.DataValueField = "LedgerPK";
                ddl_messdayscholar.DataBind();

            }
            else
            {
                ddl_messdayscholar.Items.Insert(0, "Select");
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void messleg()
    {
        try
        {
            ds1.Clear();

            clgname = "select LedgerPK,LedgerName from FM_LedgerMaster where CollegeCode ='" + collegecode1 + "' and HeaderFK ='" + ddl_messhed.SelectedItem.Value + "' and LedgerMode =0";
            ds1 = d2.select_method_wo_parameter(clgname, "Text");
            ddl_messbill.Items.Clear();
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ddl_messbill.DataSource = ds1;
                ddl_messbill.DataTextField = "LedgerName";
                ddl_messbill.DataValueField = "LedgerPK";
                ddl_messbill.DataBind();

            }
            else
            {
                ddl_messbill.Items.Insert(0, "Select");
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddl_college2_selectedindexchange(object sender, EventArgs e)
    {
        binddepartment();
    }
    //31.03.17 Hostelserialnumber gen
    protected void lnk_hosteladmissionserialno_click(object sender, EventArgs e)
    {
        serial_nogen_div.Visible = true;
        txt_frmdate_onchange(sender, e);
    }
    protected void btn_save_serial_nogen_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_acronym.Text.Trim() != "" && txt_startno.Text.Trim() != "" && txt_size.Text.Trim() != "")
            {
                query = " if exists (select HostelserialAcr from HM_Codesettings where StartDate='" + Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy")) + "' )update HM_Codesettings set HostelserialAcr='" + txt_acronym.Text.ToUpper() + "',HostelserialStNo='" + txt_startno.Text + "',Hostelserialsize='" + txt_size.Text + "' where StartDate='" + System.DateTime.Now.ToString("MM/dd/yyyy") + "' else insert into HM_Codesettings (StartDate,HostelserialAcr,HostelserialStNo,Hostelserialsize)values('" + Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy")) + "','" + txt_acronym.Text.ToUpper() + "','" + txt_startno.Text + "','" + txt_size.Text + "')";
                int save = d2.update_method_wo_parameter(query, "Text");
                if (save != 0)
                {
                    serial_nogen_div.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alerterror.Text = "Saved Successfully";
                }
            }
        }
        catch (Exception ex)
        {
            imgdiv2.Visible = true;
            lbl_alerterror.Text = ex.ToString();
        }
    }
    protected void imagebtnpopclose4_Click(object sender, EventArgs e)
    {
        serial_nogen_div.Visible = false;
    }
    protected void txt_frmdate_onchange(object sender, EventArgs e)
    {
        try
        {
            string firstdate = Convert.ToString(txt_frmdate.Text);
            DateTime dt = new DateTime();
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

            string hostelset = "select HostelserialAcr,HostelserialStNo,Hostelserialsize from HM_Codesettings where StartDate='" + Convert.ToString(dt.ToString("MM/dd/yyyy")) + "' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(hostelset, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                txt_acronym.Text = Convert.ToString(ds.Tables[0].Rows[0]["HostelserialAcr"]);
                txt_startno.Text = Convert.ToString(ds.Tables[0].Rows[0]["HostelserialStNo"]);
                txt_size.Text = Convert.ToString(ds.Tables[0].Rows[0]["Hostelserialsize"]);
            }
            else
            {
                txt_acronym.Text = "";
                txt_startno.Text = "";
                txt_size.Text = "";
            }
        }
        catch (Exception ex)
        {
            imgdiv2.Visible = true;
            lbl_alerterror.Text = ex.ToString();
        }
    }

    //magesh 20.6.18

    protected void cb_Mess_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_Mess.Text = "--Select--";
            if (cb_Mess.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_Mess.Items.Count; i++)
                {
                    cbl_Mess.Items[i].Selected = true;
                }
                txt_Mess.Text = "Mess (" + (cbl_Mess.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_Mess.Items.Count; i++)
                {
                    cbl_Mess.Items[i].Selected = false;
                }
                txt_Mess.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void cbl_Mess_SelectedIndexChanged(object sender, EventArgs e)
    {
        cb_Mess.Checked = false;
        int commcount = 0;

        txt_Mess.Text = "--Select--";

        for (int i = 0; i < cbl_Mess.Items.Count; i++)
        {
            if (cbl_Mess.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_Mess.Items.Count)
            {
                cb_Mess.Checked = true;
            }
            txt_Mess.Text = "Mess (" + commcount.ToString() + ")";
        }
    }
}