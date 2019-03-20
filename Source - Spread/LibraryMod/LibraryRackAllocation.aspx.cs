using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using InsproDataAccess;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Drawing;
using InsproDataAccess;


public partial class LibraryMod_LibraryRackAllocation : System.Web.UI.Page
{
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DAccess2 d2 = new DAccess2();
    DataTable dtCommon = new DataTable();
    DataSet dsprint = new DataSet();
    DataSet ds = new DataSet();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    FarPoint.Web.Spread.DoubleCellType doubl = new FarPoint.Web.Spread.DoubleCellType();
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string collegecode = string.Empty;
    bool IsRackValid = true;
    bool IsShelfValid = true;
    bool IsPostionValid = true;
    int ACTROW = 0;
    string libraryCode = string.Empty;
    bool CellClick = false;
    DataTable dtRackAllocation = new DataTable();
    DataRow drow;
    DataRow drCurrentRow;
    DataTable dtRackEntry = new DataTable();
    DataTable dtShelfEntry = new DataTable();
    DataTable dtPosEntry = new DataTable();
    bool BlnRackEntryGoClick = false;
    bool BlnRackEntrySaveClick = false;
    bool BlnShelfEntryGoClick = false;
    bool BlnShelfEntrySaveClick = false;
    bool BlnPosEntryGoClick = false;
    bool BlnPosEntrySaveClick = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("LibraryHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/LibraryMod/LibraryHome.aspx");
                    return;
                }
            }
          
                userCollegeCode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
                userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
           
            if (!IsPostBack)
            {
                Bindcollege();
                getLibPrivil();
            }
        }
        catch (Exception ex) { }
    }

    public void Bindcollege()
    {
        try
        {
            dtCommon.Clear();
            ddl_collegename.Enabled = false;
            dsprint.Clear();
            string qryUserCodeOrGroupCode = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["group_code"] != null && Session["single_user"] != null && Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true")
            {
                qryUserCodeOrGroupCode = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["usercode"]).Trim()))
            {
                qryUserCodeOrGroupCode = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (!string.IsNullOrEmpty(qryUserCodeOrGroupCode))
            {
                dicQueryParameter.Clear();
                dicQueryParameter.Add("column_field", Convert.ToString(qryUserCodeOrGroupCode));
                dtCommon = storeAcc.selectDataTable("bind_college", dicQueryParameter);
            }
            if (dtCommon.Rows.Count > 0)
            {
                ddl_collegename.DataSource = dtCommon;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
                ddl_collegename.SelectedIndex = 0;
                ddl_collegename.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "LibraryRackAllocation");
        }
    }

    public void getLibPrivil()
    {
        try
        {
            Hashtable hsLibcode = new Hashtable();
            string libcodecollection = "";
            string coll_Code = Convert.ToString(ddl_collegename.SelectedValue);
            string sql = "";
            string GrpUserVal = "";
            string GrpCode = "";
            string LibCollection = "";
            if (singleUser.ToLower() == "true")
            {
                sql = "SELECT DISTINCT lib_code from lib_privileges where user_code=" + userCode + " and lib_code in (select lib_code from library where college_code=" + coll_Code + ")";
                ds.Clear();
                ds = d2.select_method_wo_parameter(sql, "text");
            }
            else
            {
                string[] groupUser = groupUserCode.Split(';');
                if (groupUser.Length > 0)
                {
                    if (groupUser.Length == 1)
                    {
                        sql = "SELECT DISTINCT lib_code from lib_privileges where group_code=" + groupUser[0] + "";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(sql, "text");
                    }
                    if (groupUser.Length > 1)
                    {
                        for (int i = 0; i < groupUser.Length; i++)
                        {
                            GrpUserVal = groupUser[i];
                            if (!GrpCode.Contains(GrpUserVal))
                            {
                                if (GrpCode == "")
                                    GrpCode = GrpUserVal;
                                else
                                    GrpCode = GrpCode + "','" + GrpUserVal;
                            }
                        }
                        sql = "SELECT DISTINCT lib_code from lib_privileges where group_code in ('" + GrpCode + "')";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(sql, "text");
                    }
                }

            }
            if (ds.Tables[0].Rows.Count == 0)
            {
                libcodecollection = "WHERE lib_code IN (-1)";
                goto aa;
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string codeCollection = Convert.ToString(ds.Tables[0].Rows[i]["lib_code"]);
                    if (!hsLibcode.Contains(codeCollection))
                    {
                        hsLibcode.Add(codeCollection, "LibCode");
                        if (libcodecollection == "")
                            libcodecollection = codeCollection;
                        else
                            libcodecollection = libcodecollection + "','" + codeCollection;
                    }
                }
            }
            //libcodecollection = Left(libcodecollection, Len(libcodecollection) - 1);
            libcodecollection = "WHERE lib_code IN ('" + libcodecollection + "')";
        aa:
            LibCollection = libcodecollection;

            bindLibrary(LibCollection);

        }
        catch (Exception ex)
        {
        }
    }

    protected void bindLibrary(string Libcode)
    {
        try
        {
            ddlLibrary.Items.Clear();
            ddlMaster_Lib.Items.Clear();
            ds.Clear();
            string College = ddl_collegename.SelectedValue.ToString();
            string SelectQ = string.Empty;
            if (!string.IsNullOrEmpty(College))
            {
                dicQueryParameter.Clear();
                string lib_name = "select lib_code,lib_name,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + Libcode + " AND college_code=" + College + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = d2.select_method_wo_parameter(lib_name, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlLibrary.DataSource = ds;
                    ddlLibrary.DataTextField = "lib_name";
                    ddlLibrary.DataValueField = "lib_code";
                    ddlLibrary.DataBind();

                    ddlMaster_Lib.DataSource = ds;
                    ddlMaster_Lib.DataTextField = "lib_name";
                    ddlMaster_Lib.DataValueField = "lib_code";
                    ddlMaster_Lib.DataBind();

                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "LibraryRackAllocation");
        }
    }

    #region Common Checkbox and Checkboxlist Event

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
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
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

    #endregion

    protected void ddl_collegename_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        getLibPrivil();
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            #region query
            string Library = Convert.ToString(ddlLibrary.SelectedValue);
            string selQry = string.Empty;
            string RackNo = txt_RackNo.Text;
            if (Library == "")
            {

                selQry = "select distinct(RM.lib_code),LB.lib_name,RM.rack_no,RM.max_capacity,RM.no_of_copies ,RM.noof_rows,CAST(RIGHT(rm.rack_no, LEN(rm.rack_no) - PATINDEX('%[0-9]%', rm.rack_no)+1) AS INT), LEFT(rm.rack_no, PATINDEX('%[0-9]%', rm.rack_no)-1) FROM rack_master RM,library LB where RM.lib_code = LB.lib_code  and RM.lib_code='" + Library + "' ORDER BY LEFT(rm.rack_no, PATINDEX('%[0-9]%', rm.rack_no)-1)  ,  CAST(RIGHT(rm.rack_no, LEN(rm.rack_no) - PATINDEX('%[0-9]%', rm.rack_no)+1) AS INT)";//,rackrow_master RRM and RRM.lib_code = RM.lib_code
            }
            else
            {
                if (RackNo == "")
                {
                    selQry = "select distinct(RM.lib_code),LB.lib_name,RM.rack_no,RM.max_capacity,RM.no_of_copies ,RM.noof_rows,CAST(RIGHT(rm.rack_no, LEN(rm.rack_no) - PATINDEX('%[0-9]%', rm.rack_no)+1) AS INT), LEFT(rm.rack_no, PATINDEX('%[0-9]%', rm.rack_no)-1) FROM rack_master RM,library LB where RM.lib_code = LB.lib_code  and RM.lib_code='" + Library + "' ORDER BY LEFT(rm.rack_no, PATINDEX('%[0-9]%', rm.rack_no)-1)  ,  CAST(RIGHT(rm.rack_no, LEN(rm.rack_no) - PATINDEX('%[0-9]%', rm.rack_no)+1) AS INT)";
                }
                else
                {
                    selQry = "select distinct(RM.lib_code),LB.lib_name,RM.rack_no,RM.max_capacity,RM.no_of_copies ,RM.noof_rows,CAST(RIGHT(rm.rack_no, LEN(rm.rack_no) - PATINDEX('%[0-9]%', rm.rack_no)+1) AS INT), LEFT(rm.rack_no, PATINDEX('%[0-9]%', rm.rack_no)-1) FROM rack_master RM,library LB where RM.lib_code = LB.lib_code and RM.lib_code='" + Library + "' and rm.rack_no like '" + RackNo + "%' ORDER BY LEFT(rm.rack_no, PATINDEX('%[0-9]%', rm.rack_no)-1)  ,  CAST(RIGHT(rm.rack_no, LEN(rm.rack_no) - PATINDEX('%[0-9]%', rm.rack_no)+1) AS INT)";
                }
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selQry, "Text");
            #endregion

            #region value
            int sno = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                dtRackAllocation.Columns.Add("SNo", typeof(string));
                dtRackAllocation.Columns.Add("Rack Number", typeof(string));
                dtRackAllocation.Columns.Add("Max Capacity", typeof(string));
                dtRackAllocation.Columns.Add("No Of Copies", typeof(string));
                dtRackAllocation.Columns.Add("No Of Shelves", typeof(string));

             

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    drow = dtRackAllocation.NewRow();
                    drow["SNo"] = Convert.ToString(sno);
                    drow["Rack Number"] = Convert.ToString(ds.Tables[0].Rows[i]["rack_no"]);
                    drow["Max Capacity"] = Convert.ToString(ds.Tables[0].Rows[i]["max_capacity"]);
                    drow["No Of Copies"] = Convert.ToString(ds.Tables[0].Rows[i]["no_of_copies"]);
                    drow["No Of Shelves"] = Convert.ToString(ds.Tables[0].Rows[i]["noof_rows"]);
                    dtRackAllocation.Rows.Add(drow);
                    divspread.Visible = true;
                    print.Visible = true;
                    lblvalidation1.Text = "";
                    txtexcelname.Text = "";
                }
                GrdRackMaster.DataSource = dtRackAllocation;
                GrdRackMaster.DataBind();
                GrdRackMaster.Visible = true;

                for (int l = 0; l < GrdRackMaster.Rows.Count; l++)
                {
                    foreach (GridViewRow row in GrdRackMaster.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            GrdRackMaster.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            GrdRackMaster.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                            GrdRackMaster.Rows[l].Cells[2].HorizontalAlign = HorizontalAlign.Center;
                            GrdRackMaster.Rows[l].Cells[3].HorizontalAlign = HorizontalAlign.Center;
                            GrdRackMaster.Rows[l].Cells[3].HorizontalAlign = HorizontalAlign.Center;

                        }
                    }
                }
            }
       
            #endregion
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "LibraryRackAllocation");
        }
    }

  

    protected void GrdRackMaster_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        GrdRackMaster.PageIndex = e.NewPageIndex;
        btngo_Click(sender, e);
    }

    protected void GrdRackMaster_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void GrdRackMaster_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);

            if (Convert.ToString(rowIndex) != "")
            {
                popwindow_RackEntry.Visible = true;
                string var_rackno = "";
                string libcode = Convert.ToString(ddlLibrary.SelectedValue);
                var_rackno = Convert.ToString(GrdRackMaster.Rows[rowIndex].Cells[1].Text);

                dtRackEntry.Columns.Add("GrdRckNo");
                dtRackEntry.Columns.Add("GrdRckMaxCap");
                dtRackEntry.Columns.Add("GrdNoOfShlfinRck");
                dtRackEntry.Columns.Add("AvailableCopy");

                string sql = "select distinct(rack_master.rack_no) as rack_no,rack_master.max_capacity,rack_master.no_of_copies,rack_master.noof_rows from rack_master where  rack_master.lib_code ='" + libcode + "'and rack_master.rack_no = '" + var_rackno + "'order  by rack_master.rack_no";
                ds.Clear();
                ds = d2.select_method_wo_parameter(sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["rack_no"])))
                        txt_ShlfRackNo.Text = Convert.ToString(ds.Tables[0].Rows[0]["rack_no"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["max_capacity"])))
                        txt_ShlfRackMax.Text = Convert.ToString(ds.Tables[0].Rows[0]["max_capacity"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["no_of_copies"])))
                        txt_NORinShlf.Text = Convert.ToString(ds.Tables[0].Rows[0]["no_of_copies"]);

                    string sacr = d2.GetFunction("select sacr from rack_master where lib_code='" + libcode + "' and rack_no='" + Convert.ToString(ds.Tables[0].Rows[0]["rack_no"]) + "'");
                    string sno = d2.GetFunction("select stno from rack_master where lib_code='" + libcode + "' and rack_no='" + Convert.ToString(ds.Tables[0].Rows[0]["rack_no"]) + "'");
                    string racr = d2.GetFunction("select racr from rack_master where lib_code='" + libcode + "' and rack_no='" + Convert.ToString(ds.Tables[0].Rows[0]["rack_no"]) + "'");
                    string rno = d2.GetFunction("select rno from rack_master where lib_code='" + libcode + "' and rack_no='" + Convert.ToString(ds.Tables[0].Rows[0]["rack_no"]) + "'");
                    txt_ShlfAcr.Text = sacr;
                    txt_ShlfStNo.Text = sno;
                    txt_RackAcr.Text = racr;
                    txt_RackstNo.Text = rno;

                    DataRow drow = dtRackEntry.NewRow();
                    drow["GrdRckNo"] = Convert.ToString(ds.Tables[0].Rows[0]["rack_no"]);
                    drow["GrdRckMaxCap"] = Convert.ToString(ds.Tables[0].Rows[0]["max_capacity"]);
                    drow["GrdNoOfShlfinRck"] = Convert.ToString(ds.Tables[0].Rows[0]["noof_rows"]);
                    drow["AvailableCopy"] = "";

                    dtRackEntry.Rows.Add(drow);
                    GrdRackEntry.DataSource = dtRackEntry;
                    GrdRackEntry.DataBind();
                    GrdRackEntry.Visible = true;
                    divSpreadRack.Visible = true;
                    BlnRackEntrySaveClick = true;
                    BtnRackDelete.Visible = true;
                    BtnRackDelete.Enabled = true;
                    BtnRackSave.Visible = false;
                    BtnRackSave.Enabled = false;
                    BtnRackUpdate.Visible = true;
                    BtnRackUpdate.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void GrdRackMaster_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (BlnRackEntrySaveClick == true)
            {
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = true;
                BlnRackEntrySaveClick = false;
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (BlnRackEntrySaveClick == true)
            {
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = true;
                BlnRackEntrySaveClick = false;
            }
        }
    }

    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popwindow_RackEntry.Visible = false;
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    protected void BtnAddRack_Click(object sender, EventArgs e)
    {
        popwindow_RackEntry.Visible = true;
        BtnRackSave.Visible = false;
        BtnRackDelete.Visible = false;
        BtnRackUpdate.Visible = false;
        divSpreadRack.Visible = false;
        txt_TotalRack.Text = "";
        txt_RackAcr.Text = "";
        txt_RackstNo.Text = "";
    }

    #region Rack Entry

    protected void txtRackAcr_OnTextChanged(object sender, EventArgs e)
    {
        txt_RackAcr.Text = txt_RackAcr.Text.ToUpper();
    }

    protected void BtnRackEntryGo_Click(object sender, EventArgs e)
    {
        if (txt_TotalRack.Text == "" || txt_RackAcr.Text == "" || txt_RackstNo.Text == "")
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Fill the rack details and try search";
        }
        else
        {
            AutoGen_RackNo();
            BtnRackSave.Enabled = true;
            BtnRackSave.Visible = true;
            BtnRackDelete.Visible = false;
            BtnRackUpdate.Visible = false;
        }
    }

    protected void AutoGen_RackNo()
    {
        try
        {
            string RackAcr = txt_RackAcr.Text;
            string RackStNo = txt_RackstNo.Text;
            string TotalRackNo = txt_TotalRack.Text;
            int SpRowCnt = GrdRackEntry.Rows.Count;

            double TotalRacks = 0;
            string Rack = "";
            if (TotalRackNo != "" && SpRowCnt == 0)
            {
                SpRowCnt = SpRowCnt + (Convert.ToInt32(TotalRackNo) - SpRowCnt);
                TotalRacks = Convert.ToDouble(RackStNo);
                dtRackEntry.Columns.Add("GrdRckNo");
                dtRackEntry.Columns.Add("GrdRckMaxCap");
                dtRackEntry.Columns.Add("GrdNoOfShlfinRck");
                dtRackEntry.Columns.Add("AvailableCopy");
                for (int i = 0; i < SpRowCnt; i++)
                {
                    if (RackAcr != "" && RackStNo != "")
                    {
                        BlnRackEntryGoClick = true;
                        DataRow drow = dtRackEntry.NewRow();
                        Rack = RackAcr + TotalRacks;
                        drow["GrdRckNo"] = Rack;
                        drow["GrdRckMaxCap"] = "";
                        drow["GrdNoOfShlfinRck"] = "";
                        drow["AvailableCopy"] = "";
                        TotalRacks++;
                        dtRackEntry.Rows.Add(drow);
                    }
                }
                ViewState["CurrentTable"] = dtRackEntry;
                GrdRackEntry.DataSource = dtRackEntry;
                GrdRackEntry.DataBind();
                GrdRackEntry.Visible = true;
                divSpreadRack.Visible = true;
            }
            else if (TotalRackNo != "" && SpRowCnt != 0)
            {
                int SpCnt = GrdRackEntry.Rows.Count;
                TotalRacks = Convert.ToDouble(RackStNo);
                TotalRacks = TotalRacks + SpCnt;
                SpRowCnt = Convert.ToInt32(TotalRackNo) - SpCnt;
                if (ViewState["CurrentTable"] != null)
                {
                    dtRackEntry = (DataTable)ViewState["CurrentTable"];
                    drCurrentRow = null;
                    for (int i = 0; i < SpRowCnt; i++)
                    {
                        if (RackAcr != "" && RackStNo != "")
                        {
                            BlnRackEntryGoClick = true;
                            drCurrentRow = dtRackEntry.NewRow();
                            Rack = RackAcr + TotalRacks;
                            drCurrentRow["GrdRckNo"] = Rack;
                            drCurrentRow["GrdRckMaxCap"] = "";
                            drCurrentRow["GrdNoOfShlfinRck"] = "";
                            drCurrentRow["AvailableCopy"] = "";
                            dtRackEntry.Rows.Add(drCurrentRow);
                            TotalRacks++;
                        }
                    }
                }
                ViewState["CurrentTable"] = dtRackEntry;
                GrdRackEntry.DataSource = dtRackEntry;
                GrdRackEntry.DataBind();
                GrdRackEntry.Visible = true;
                divSpreadRack.Visible = true;
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, userCollegeCode, "LibraryRackAllocation");
        }
    }

    public bool RackValid()
    {
        int SpRowCnt = GrdRackEntry.Rows.Count;
        string Library = Convert.ToString(ddlMaster_Lib.SelectedValue);
        string selQry = string.Empty;
        string RackNO = "";
        string RackCap = "";
        string NoOfShelf = "";
        if (SpRowCnt == 0)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Enter atleast one row";
        }
        foreach (GridViewRow gvrow in GrdRackEntry.Rows)
        {
            int RowCnt = Convert.ToInt32(gvrow.RowIndex);
            TextBox txt_RackNO = (TextBox)GrdRackEntry.Rows[RowCnt].FindControl("txt_GrdRckNo");
            if (txt_RackNO.Text.Trim() != "")
            {
                RackNO = txt_RackNO.Text.Trim();
            }
            TextBox txt_RackCap = (TextBox)GrdRackEntry.Rows[RowCnt].FindControl("txt_GrdRckMaxCap");
            if (txt_RackCap.Text.Trim() != "")
            {
                RackCap = txt_RackCap.Text.Trim();
            }
            TextBox txt_NoOfShelf = (TextBox)GrdRackEntry.Rows[RowCnt].FindControl("txt_GrdNoOfShlfinRck");
            if (txt_NoOfShelf.Text.Trim() != "")
            {
                NoOfShelf = txt_NoOfShelf.Text.Trim();
            }
            if (RackNO == "")
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Rack No. should not be empty";
                IsRackValid = false;
            }
            if (RackCap == "")
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Rack Capacity should not be empty";
                IsRackValid = false;
            }
            if (RackCap == "0")
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Rack Capacity should not be 0";
                IsRackValid = false;
            }
            if (NoOfShelf == "")
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "No. of shelf should not be empty";
                IsRackValid = false;
            }
            if (NoOfShelf == "0")
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "No. of shelf should not be 0";
                IsRackValid = false;
            }
            if (RackCap == "" && NoOfShelf == "")
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Rack Capacity and No. of shelf should not be empty";
            }
            if (RackNO != "" && RackCap != "0" & RackCap != "" && NoOfShelf != "0" && NoOfShelf != "")
            {
                selQry = "SELECT * FROM Rack_Master WHERE Rack_No ='" + RackNO + "' AND Lib_Code ='" + Library + "' ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selQry, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Rack No. " + RackNO + " already exist";
                    IsRackValid = false;
                }
            }
        }
        return IsRackValid;
    }

    protected void BtnRackSave_Click(object sender, EventArgs e)
    {
        try
        {
            string RackNO = "";
            string RackCap = "";
            string NoOfShelf = "";
            if (!RackValid())
            {
                //BtnAddRack_Click(sender, e);
                //txt_TotalRack.Text = "";
                //txt_RackAcr.Text = "";
                //txt_RackstNo.Text = "";
                return;
            }
            else
            {
                ViewState["CurrentTable"] = null;
                dtRackEntry.Columns.Add("GrdRckNo");
                dtRackEntry.Columns.Add("GrdRckMaxCap");
                dtRackEntry.Columns.Add("GrdNoOfShlfinRck");
                dtRackEntry.Columns.Add("AvailableCopy");

                Hashtable htSpreadBind = new Hashtable();
                int SpRowCnt = GrdRackEntry.Rows.Count;
                string Library = Convert.ToString(ddlMaster_Lib.SelectedValue);
                string RackNo = txt_TotalRack.Text;
                string RackAcr = txt_RackAcr.Text;
                string RackStNo = txt_RackstNo.Text;
                string insertQry = "";
                int insert = 0;
                foreach (GridViewRow gvrow in GrdRackEntry.Rows)
                {
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    TextBox txt_RackNO = (TextBox)GrdRackEntry.Rows[RowCnt].FindControl("txt_GrdRckNo");
                    if (txt_RackNO.Text.Trim() != "")
                    {
                        RackNO = txt_RackNO.Text.Trim();
                    }
                    TextBox txt_RackCap = (TextBox)GrdRackEntry.Rows[RowCnt].FindControl("txt_GrdRckMaxCap");
                    if (txt_RackCap.Text.Trim() != "")
                    {
                        RackCap = txt_RackCap.Text.Trim();
                    }
                    TextBox txt_NoOfShelf = (TextBox)GrdRackEntry.Rows[RowCnt].FindControl("txt_GrdNoOfShlfinRck");
                    if (txt_NoOfShelf.Text.Trim() != "")
                    {
                        NoOfShelf = txt_NoOfShelf.Text.Trim();
                    }
                    string Time = DateTime.Now.ToString("hh:mm tt");
                    string Date = DateTime.Now.ToString("MM/dd/yyyy");

                    insertQry = " INSERT INTO Rack_Master(Lib_Code,Rack_No,Max_Capacity,No_Of_Copies,NoOf_Rows,access_date,access_time,RAcr,RNo,sacr,stno) VALUES ('" + Library + "','" + RackNO + "','" + RackCap + "','0','" + NoOfShelf + "','" + Date + "','" + Time + "','" + RackAcr + "','" + RackStNo + "','" + RackAcr + "','" + RackStNo + "')";
                    insert = d2.update_method_wo_parameter(insertQry, "TEXT");
                    DataRow drow = dtRackEntry.NewRow();
                    drow["GrdRckNo"] = RackNO;
                    drow["GrdRckMaxCap"] = RackCap;
                    drow["GrdNoOfShlfinRck"] = NoOfShelf;
                    drow["AvailableCopy"] = "";
                    dtRackEntry.Rows.Add(drow);

                }
                if (insert > 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Rack Information Sucessfully Saved";
                    ViewState["CurrentTable"] = dtRackEntry;
                    GrdRackEntry.DataSource = dtRackEntry;
                    GrdRackEntry.DataBind();
                    GrdRackEntry.Visible = true;
                    divSpreadRack.Visible = true;
                    BtnRackDelete.Visible = true;
                    BtnRackDelete.Enabled = true;
                    BtnRackSave.Visible = false;
                    BtnRackSave.Enabled = false;
                    BtnRackUpdate.Enabled = false;
                    BtnRackUpdate.Visible = false;
                    BlnRackEntrySaveClick = true;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "LibraryRackAllocation");
        }
    }

    protected void BtnRackUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            string selQry = "";
            string insertQry = "";
            int insert = 0;
            string updateQry = "";
            string RackNO = "";
            string RackCap = "";
            string NoOfShelf = "";
            int selCount = 0;
            string Library = Convert.ToString(ddlMaster_Lib.SelectedValue);
            string RackAcr = txt_RackAcr.Text;
            string RackStNo = txt_RackstNo.Text;
            foreach (GridViewRow gvrow in GrdRackEntry.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                if (chk.Checked == true)
                {
                    selCount++;
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    TextBox txt_RackNO = (TextBox)GrdRackEntry.Rows[RowCnt].FindControl("txt_GrdRckNo");
                    if (txt_RackNO.Text.Trim() != "")
                    {
                        RackNO = txt_RackNO.Text.Trim();
                    }
                    TextBox txt_RackCap = (TextBox)GrdRackEntry.Rows[RowCnt].FindControl("txt_GrdRckMaxCap");
                    if (txt_RackCap.Text.Trim() != "")
                    {
                        RackCap = txt_RackCap.Text.Trim();
                    }
                    TextBox txt_NoOfShelf = (TextBox)GrdRackEntry.Rows[RowCnt].FindControl("txt_GrdNoOfShlfinRck");
                    if (txt_NoOfShelf.Text.Trim() != "")
                    {
                        NoOfShelf = txt_NoOfShelf.Text.Trim();
                    }

                    string Time = DateTime.Now.ToString("hh:mm tt");
                    string Date = DateTime.Now.ToString("MM/dd/yyyy");

                    selQry = "SELECT * FROM Rack_Master WHERE Rack_No ='" + RackNO + "' AND Lib_Code ='" + Library + "' ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selQry, "Text");
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        insertQry = "INSERT INTO Rack_Master(Lib_Code,Rack_No,Max_Capacity,No_Of_Copies,NoOf_Rows,Access_Date,Access_Time,RAcr,RNo,SAcr,StNo) VALUES ('" + Library + "','" + RackNO + "','" + RackCap + "','0','" + NoOfShelf + "','" + Date + "','" + Time + "','" + RackAcr + "','" + RackStNo + "','" + RackAcr + "','" + RackStNo + "')";
                        insert = d2.update_method_wo_parameter(insertQry, "TEXT");
                    }
                    else
                    {
                        updateQry = "UPDATE Rack_Master SET Max_Capacity='" + RackCap + "',NoOf_Rows=" + NoOfShelf + ",Access_Date='" + Date + "',Access_Time='" + Time + "',RAcr='" + RackAcr + "',RNo='" + RackStNo + "',SAcr='" + RackAcr + "',StNo='" + RackStNo + "' WHERE Rack_No ='" + RackNO + "' AND Lib_Code ='" + Library + "' ";
                        insert = d2.update_method_wo_parameter(updateQry, "TEXT");
                    }
                }
            }
            if (insert > 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Rack Information Sucessfully Updated";
            }
            if (selCount == 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Select the row to Update";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "LibraryRackAllocation");
        }
    }

    protected void BtnRackDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int selCount = 0;
            foreach (GridViewRow gvrow in GrdRackEntry.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                if (chk.Checked == true)
                {
                    selCount++;
                }
            }
            if (selCount > 0)
            {
                SureDivDeleteRack.Visible = true;
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Select the row to Delete";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "LibraryRackAllocation");
        }
    }

    protected void btn_DeleteRackYes_Click(object sender, EventArgs e)
    {
        try
        {
            string selQry = "";
            string deleteQry = "";
            string RackNO = "";
            int delete = 0;
            int DeleteCount = 0;
            string Library = Convert.ToString(ddlMaster_Lib.SelectedValue);
            string RackAcr = txt_RackAcr.Text;
            string RackStNo = txt_RackstNo.Text;
            foreach (GridViewRow gvrow in GrdRackEntry.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                if (chk.Checked == true)
                {
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    TextBox txt_RackNO = (TextBox)GrdRackEntry.Rows[RowCnt].FindControl("txt_GrdRckNo");
                    if (txt_RackNO.Text.Trim() != "")
                    {
                        RackNO = txt_RackNO.Text.Trim();
                    }

                    selQry = "SELECT * FROM Rack_Master WHERE Rack_No ='" + RackNO + "' AND Lib_Code ='" + Library + "' ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selQry, "Text");

                    deleteQry = "DELETE FROM Rack_Master WHERE Rack_No = '" + RackNO + "' AND Lib_Code = '" + Library + "'";
                    delete = d2.update_method_wo_parameter(deleteQry, "TEXT");
                    DeleteCount++;
                    deleteQry = "DELETE FROM RackRow_Master WHERE Rack_No = '" + RackNO + "' AND Lib_Code = '" + Library + "' ";
                    delete = d2.update_method_wo_parameter(deleteQry, "TEXT");
                    DeleteCount = DeleteCount + delete;
                    deleteQry = "DELETE FROM RowPos_Master WHERE Rack_No = '" + RackNO + "' AND Lib_Code = '" + Library + "'";
                    delete = d2.update_method_wo_parameter(deleteQry, "TEXT");
                    DeleteCount = DeleteCount + delete;
                }
            }
            if (delete > 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Rack has been deleted successfully";
                SureDivDeleteRack.Visible = false;
                selQry = "select rack_no,max_capacity,NoOf_Rows from rack_master where rno='" + RackStNo + "' and racr='" + RackAcr + "' and lib_code='" + Library + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selQry, "Text");
                ViewState["CurrentTable"] = null;
                dtRackEntry.Columns.Add("GrdRckNo");
                dtRackEntry.Columns.Add("GrdRckMaxCap");
                dtRackEntry.Columns.Add("GrdNoOfShlfinRck");
                dtRackEntry.Columns.Add("AvailableCopy");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow drow = dtRackEntry.NewRow();
                    drow["GrdRckNo"] = Convert.ToString(ds.Tables[0].Rows[i]["rack_no"]);
                    drow["GrdRckMaxCap"] = Convert.ToString(ds.Tables[0].Rows[i]["max_capacity"]);
                    drow["GrdNoOfShlfinRck"] = Convert.ToString(ds.Tables[0].Rows[i]["NoOf_Rows"]);
                    drow["AvailableCopy"] = "";
                    dtRackEntry.Rows.Add(drow);
                }
                ViewState["CurrentTable"] = dtRackEntry;
                GrdRackEntry.DataSource = dtRackEntry;
                GrdRackEntry.DataBind();
                GrdRackEntry.Visible = true;
                divSpreadRack.Visible = true;
                BtnRackDelete.Enabled = true;
                BtnRackSave.Enabled = false;
                BtnRackUpdate.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "LibraryRackAllocation");
        }
    }

    protected void btn_DeleteRackNo_Click(object sender, EventArgs e)
    {
        SureDivDeleteRack.Visible = false;
    }

    protected void GrdRackEntry_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (BlnRackEntryGoClick == true)
            {
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
            }
            if (BlnRackEntrySaveClick == true)
            {
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = true;
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (BlnRackEntryGoClick == true)
            {
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
            }
            if (BlnRackEntrySaveClick == true)
            {
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = true;
            }
        }
    }

    #endregion

    protected void btn_AddShelf_click(object sender, EventArgs e)
    {
        int rowindex = ((GridViewRow)((Button)sender).Parent.Parent).RowIndex;
        string RackNO = "";
        string RackCap = "";
        string NoOfShelf = "";
        TextBox txt_RackNO = (TextBox)GrdRackEntry.Rows[rowindex].FindControl("txt_GrdRckNo");
        if (txt_RackNO.Text.Trim() != "")
        {
            RackNO = txt_RackNO.Text.Trim();
        }
        TextBox txt_RackCap = (TextBox)GrdRackEntry.Rows[rowindex].FindControl("txt_GrdRckMaxCap");
        if (txt_RackCap.Text.Trim() != "")
        {
            RackCap = txt_RackCap.Text.Trim();
        }
        TextBox txt_NoOfShelf = (TextBox)GrdRackEntry.Rows[rowindex].FindControl("txt_GrdNoOfShlfinRck");
        if (txt_NoOfShelf.Text.Trim() != "")
        {
            NoOfShelf = txt_NoOfShelf.Text.Trim();
        }
        Session["libCode"] = Convert.ToString(ddlMaster_Lib.SelectedValue);
        DivShelfEntry.Visible = true;
        txt_ShlfRackNo.Text = RackNO;
        txt_ShlfRackNo.Enabled = false;
        txt_ShlfRackMax.Text = RackCap;
        txt_ShlfRackMax.Enabled = false;
        txt_NORinShlf.Text = NoOfShelf;
        txt_NORinShlf.Enabled = false;

        BtnShlfSave.Visible = false;
        BtnShlfDel.Visible = false;
        BtnShlfUpdate.Visible = false;

    }

    #region Shelf Entry

    protected void txt_ShlfAcr_OnTextChanged(object sender, EventArgs e)
    {
        txt_ShlfAcr.Text = txt_ShlfAcr.Text.ToUpper();
    }

    protected void BtnShlfGo_Click(object sender, EventArgs e)
    {
        if (txt_ShlfAcr.Text == "" || txt_ShlfStNo.Text == "")
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Fill the Shelf details and try search";
        }
        else
        {
            AutoGen_Shelf();
            BtnShlfSave.Enabled = true;
            BtnShlfSave.Visible = true;
            BtnShlfDel.Visible = false;
            BtnShlfUpdate.Visible = false;
        }
    }

    protected void imagebtnShlfpopclose_Click(object sender, EventArgs e)
    {
        DivShelfEntry.Visible = false;
    }

    protected void AutoGen_Shelf()
    {
        string ShelfAcr = txt_ShlfAcr.Text;
        string ShelfStNo = txt_ShlfStNo.Text;
        string TotalShelfNo = txt_NORinShlf.Text;
        int SpRowCnt = GrdShelfEntry.Rows.Count;
        double TotalRacks = 0;
        string Rack = "";
        if (TotalShelfNo != "" && SpRowCnt == 0)
        {
            dtShelfEntry.Columns.Add("GrdShlfNo");
            dtShelfEntry.Columns.Add("GrdShlfMaxCap");
            dtShelfEntry.Columns.Add("GrdNoOfPosinShlf");
            dtShelfEntry.Columns.Add("AvailableCopy");
            SpRowCnt = SpRowCnt + (Convert.ToInt32(TotalShelfNo) - SpRowCnt);
            TotalRacks = Convert.ToDouble(ShelfStNo);
            for (int i = 0; i < SpRowCnt; i++)
            {
                if (ShelfAcr != "" && ShelfStNo != "")
                {
                    BlnShelfEntryGoClick = true;
                    Rack = ShelfAcr + TotalRacks;
                    drow = dtShelfEntry.NewRow();
                    drow["GrdShlfNo"] = Rack;
                    drow["GrdShlfMaxCap"] = "";
                    drow["GrdNoOfPosinShlf"] = "";
                    drow["AvailableCopy"] = "";
                    TotalRacks++;
                    dtShelfEntry.Rows.Add(drow);
                }
            }
            GrdShelfEntry.DataSource = dtShelfEntry;
            GrdShelfEntry.DataBind();
            GrdShelfEntry.Visible = true;
            divShelfSpread.Visible = true;
        }
    }

    public bool ShelfValid()
    {
        int SpRowCnt = GrdShelfEntry.Rows.Count;
        string Library = Convert.ToString(Session["libCode"]);
        int IntShelfCap = 0;
        string RackNo = txt_ShlfRackNo.Text;
        string rackMaxCap = txt_ShlfRackMax.Text;
        string selQry = string.Empty;
        string ShelfNO = "";
        string ShelfCap = "";
        string NoOfposition = "";
        if (SpRowCnt == 0)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Enter atleast one row";
        }
        foreach (GridViewRow gvrow in GrdShelfEntry.Rows)
        {
            int RowCnt = Convert.ToInt32(gvrow.RowIndex);
            TextBox txt_ShelfNO = (TextBox)GrdShelfEntry.Rows[RowCnt].FindControl("txt_GrdShlfNo");
            if (txt_ShelfNO.Text.Trim() != "")
            {
                ShelfNO = txt_ShelfNO.Text.Trim();
            }
            TextBox txt_ShelfCap = (TextBox)GrdShelfEntry.Rows[RowCnt].FindControl("txt_GrdShlfMaxCap");
            if (txt_ShelfCap.Text.Trim() != "")
            {
                ShelfCap = txt_ShelfCap.Text.Trim();
            }
            TextBox txt_NoOfposition = (TextBox)GrdShelfEntry.Rows[RowCnt].FindControl("txt_GrdNoOfPosinShlf");
            if (txt_NoOfposition.Text.Trim() != "")
            {
                NoOfposition = txt_NoOfposition.Text.Trim();
            }

            if (ShelfCap != "")
            {
                IntShelfCap = IntShelfCap + Convert.ToInt32(ShelfCap);
            }
            if (ShelfNO == "")
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Shelf No. should not be empty";
                IsShelfValid = false;
            }
            if (ShelfCap == "")
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Shelf Capacity should not be empty";
                IsShelfValid = false;
            }
            if (ShelfCap == "0")
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Shelf Capacity should not be 0";
                IsShelfValid = false;
            }
            if (NoOfposition == "")
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "No. of Position should not be empty";
                IsShelfValid = false;
            }
            if (NoOfposition == "0")
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "No. of Position should not be 0";
                IsShelfValid = false;
            }
            if (ShelfCap == "" && NoOfposition == "")
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Shelf Capacity and No. of Position should not be empty";
            }
            if (ShelfNO != "" && ShelfCap != "0" & ShelfCap != "" && NoOfposition != "0" && NoOfposition != "")
            {
                selQry = "SELECT * FROM RackRow_Master WHERE Rack_No ='" + RackNo + "' AND Row_No ='" + ShelfNO + "' AND Lib_Code ='" + Library + "' ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selQry, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Shelf No." + ShelfNO + " already exist";
                    IsShelfValid = false;
                }
            }
        }
        if (IntShelfCap != Convert.ToInt32(rackMaxCap))
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Shelf total capacity should be " + rackMaxCap + " ";
            IsShelfValid = false;
        }
        return IsShelfValid;
    }

    protected void BtnShlfSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!ShelfValid())
            {
                //txt_TotalRack.Text = "";
                //txt_RackAcr.Text = "";
                //txt_RackstNo.Text = "";
                return;
            }
            else
            {
                Hashtable htSpreadBind = new Hashtable();
                int SpRowCnt = GrdShelfEntry.Rows.Count;
                string Library = Convert.ToString(Session["libCode"]);
                string RackNo = txt_ShlfRackNo.Text;
                string ShelfAcr = txt_ShlfAcr.Text;
                string shelfStNo = txt_ShlfStNo.Text;
                string insertQry = "";
                int insert = 0;
                string ShelfNO = "";
                string ShelfCap = "";
                string NoOfPos = "";
                dtShelfEntry.Columns.Add("GrdShlfNo");
                dtShelfEntry.Columns.Add("GrdShlfMaxCap");
                dtShelfEntry.Columns.Add("GrdNoOfPosinShlf");
                dtShelfEntry.Columns.Add("AvailableCopy");
                string sql = "update rack_master set Acr_Shelf ='" + ShelfAcr + "',St_Shelf ='" + shelfStNo + "' WHERE Lib_Code='" + Library + "' and Rack_No='" + RackNo + "' ";
                foreach (GridViewRow gvrow in GrdShelfEntry.Rows)
                {
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    TextBox txt_ShelfNO = (TextBox)GrdShelfEntry.Rows[RowCnt].FindControl("txt_GrdShlfNo");
                    if (txt_ShelfNO.Text.Trim() != "")
                    {
                        ShelfNO = txt_ShelfNO.Text.Trim();
                    }
                    TextBox txt_ShelfCap = (TextBox)GrdShelfEntry.Rows[RowCnt].FindControl("txt_GrdShlfMaxCap");
                    if (txt_ShelfCap.Text.Trim() != "")
                    {
                        ShelfCap = txt_ShelfCap.Text.Trim();
                    }
                    TextBox txt_NoOfPos = (TextBox)GrdShelfEntry.Rows[RowCnt].FindControl("txt_GrdNoOfPosinShlf");
                    if (txt_NoOfPos.Text.Trim() != "")
                    {
                        NoOfPos = txt_NoOfPos.Text.Trim();
                    }
                    string Time = DateTime.Now.ToString("hh:mm tt");
                    string Date = DateTime.Now.ToString("MM/dd/yyyy");

                    insertQry = " INSERT INTO RackRow_Master (rack_no,row_no,max_capacity,no_of_copies,access_date,access_time,NoOfPos,lib_code) VALUES('" + RackNo + "','" + ShelfNO + "','" + ShelfCap + "','0','" + Date + "','" + Time + "','" + NoOfPos + "','" + Library + "')";
                    insert = d2.update_method_wo_parameter(insertQry, "TEXT");

                    DataRow drow = dtShelfEntry.NewRow();
                    drow["GrdShlfNo"] = ShelfNO;
                    drow["GrdShlfMaxCap"] = ShelfCap;
                    drow["GrdNoOfPosinShlf"] = NoOfPos;
                    drow["AvailableCopy"] = "";
                    dtShelfEntry.Rows.Add(drow);
                }
                if (insert > 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Shelf Information Sucessfully Saved";
                    GrdShelfEntry.DataSource = dtShelfEntry;
                    GrdShelfEntry.DataBind();
                    GrdShelfEntry.Visible = true;
                    divShelfSpread.Visible = true;
                    BtnShlfDel.Visible = true;
                    BtnShlfDel.Enabled = true;
                    BtnShlfSave.Visible = false;
                    BtnShlfSave.Enabled = false;
                    BtnShlfUpdate.Enabled = false;
                    BtnShlfUpdate.Visible = false;
                    BlnShelfEntrySaveClick = true;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "LibraryRackAllocation");
        }
    }

    protected void BtnShlfDel_Click(object sender, EventArgs e)
    {
        try
        {
            int selCount = 0;
            foreach (GridViewRow gvrow in GrdShelfEntry.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                if (chk.Checked == true)
                {
                    selCount++;
                }
            }
            if (selCount > 0)
            {
                SureDivDeleteShelf.Visible = true;
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Select the row to Delete";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "LibraryRackAllocation");
        }
    }

    protected void btn_DeleteShelfYes_Click(object sender, EventArgs e)
    {
        try
        {
            string selQry = "";
            string deleteQry = "";
            int delete = 0;
            string Library = Convert.ToString(Session["libCode"]);
            string RackNo = txt_ShlfRackNo.Text;
            string RackAcr = txt_RackAcr.Text;
            string RackStNo = txt_RackstNo.Text;
            string ShelfNO = "";
            foreach (GridViewRow gvrow in GrdShelfEntry.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                if (chk.Checked == true)
                {
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    TextBox txt_ShelfNO = (TextBox)GrdShelfEntry.Rows[RowCnt].FindControl("txt_GrdShlfNo");
                    if (txt_ShelfNO.Text.Trim() != "")
                    {
                        ShelfNO = txt_ShelfNO.Text.Trim();
                    }
                    selQry = "SELECT * FROM Rack_Allocation WHERE Rack_No ='" + RackNo + "' AND Row_No='" + ShelfNO + "' AND Lib_Code ='" + Library + "' ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selQry, "Text");

                    deleteQry = "DELETE FROM RackRow_Master WHERE Rack_No = '" + RackNo + "' AND Row_No='" + ShelfNO + "' AND Lib_Code = '" + Library + "' ";
                    delete = d2.update_method_wo_parameter(deleteQry, "TEXT");
                    deleteQry = "DELETE FROM RowPos_Master WHERE Rack_No = '" + RackNo + "' AND Row_No='" + ShelfNO + "' AND Lib_Code = '" + Library + "'";
                    delete = d2.update_method_wo_parameter(deleteQry, "TEXT");
                }
            }
            imgdiv2.Visible = true;
            lbl_alert.Text = "Shelf has been deleted successfully";
            SureDivDeleteShelf.Visible = false;

            selQry = "select row_no,max_capacity,NoOfPos from RackRow_Master where rack_no='" + RackNo + "' and lib_code='" + Library + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selQry, "Text");
            ViewState["CurrentTable"] = null;
            dtShelfEntry.Columns.Add("GrdShlfNo");
            dtShelfEntry.Columns.Add("GrdShlfMaxCap");
            dtShelfEntry.Columns.Add("GrdNoOfPosinShlf");
            dtShelfEntry.Columns.Add("AvailableCopy");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow drow = dtShelfEntry.NewRow();
                drow["GrdShlfNo"] = Convert.ToString(ds.Tables[0].Rows[i]["row_no"]);
                drow["GrdShlfMaxCap"] = Convert.ToString(ds.Tables[0].Rows[i]["max_capacity"]);
                drow["GrdNoOfPosinShlf"] = Convert.ToString(ds.Tables[0].Rows[i]["NoOfPos"]);
                drow["AvailableCopy"] = "";
                dtShelfEntry.Rows.Add(drow);
            }
            GrdShelfEntry.DataSource = dtShelfEntry;
            GrdShelfEntry.DataBind();
            GrdShelfEntry.Visible = true;
            divShelfSpread.Visible = true;
            BtnShlfDel.Visible = true;
            BtnShlfDel.Enabled = true;
            BtnShlfSave.Visible = false;
            BtnShlfSave.Enabled = false;
            BtnShlfUpdate.Visible = true;
            BtnShlfUpdate.Enabled = true;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "LibraryRackAllocation");
        }
    }

    protected void btn_DeleteShelfNo_Click(object sender, EventArgs e)
    {
        SureDivDeleteShelf.Visible = false;
    }

    protected void BtnShlfUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            string selQry = "";
            string insertQry = "";
            int insert = 0;
            string updateQry = "";
            string Library = Convert.ToString(Session["libCode"]);
            string RackNo = txt_ShlfRackNo.Text;
            string ShelfAcr = txt_ShlfAcr.Text;
            string shelfStNo = txt_ShlfStNo.Text;
            int selCount = 0;
            string ShelfNO = "";
            string ShelfCap = "";
            string NoOfPos = "";
            foreach (GridViewRow gvrow in GrdShelfEntry.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                if (chk.Checked == true)
                {
                    selCount++;
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    TextBox txt_ShelfNO = (TextBox)GrdShelfEntry.Rows[RowCnt].FindControl("txt_GrdShlfNo");
                    if (txt_ShelfNO.Text.Trim() != "")
                    {
                        ShelfNO = txt_ShelfNO.Text.Trim();
                    }
                    TextBox txt_ShelfCap = (TextBox)GrdShelfEntry.Rows[RowCnt].FindControl("txt_GrdShlfMaxCap");
                    if (txt_ShelfCap.Text.Trim() != "")
                    {
                        ShelfCap = txt_ShelfCap.Text.Trim();
                    }
                    TextBox txt_NoOfPos = (TextBox)GrdShelfEntry.Rows[RowCnt].FindControl("txt_GrdNoOfPosinShlf");
                    if (txt_NoOfPos.Text.Trim() != "")
                    {
                        NoOfPos = txt_NoOfPos.Text.Trim();
                    }

                    string Time = DateTime.Now.ToString("hh:mm tt");
                    string Date = DateTime.Now.ToString("MM/dd/yyyy");

                    selQry = "SELECT * FROM RackRow_Master WHERE Rack_No ='" + RackNo + "' AND  Row_No ='" + ShelfNO + "' AND Lib_Code ='" + Library + "' ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selQry, "Text");
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        insertQry = "INSERT INTO RackRow_Master(rack_no,row_no,max_capacity,no_of_copies,access_date,access_time,NoOfPos,lib_code) VALUES ('" + RackNo + "','" + ShelfNO + "','" + ShelfCap + "','0','" + Date + "','" + Time + "','" + NoOfPos + "','" + Library + "')";
                        insert = d2.update_method_wo_parameter(insertQry, "TEXT");
                    }
                    else
                    {
                        updateQry = "UPDATE RackRow_Master SET Max_Capacity=" + ShelfCap + ",NoOfPos=" + NoOfPos + ",Access_Date='" + Date + "',Access_Time='" + Time + "' WHERE Row_No ='" + ShelfNO + "' AND Lib_Code ='" + Library + "' ";
                        insert = d2.update_method_wo_parameter(updateQry, "TEXT");
                    }
                }
            }
            if (insert > 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Shelf Information Sucessfully Saved";
            }
            if (selCount == 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Select the row to Update";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "LibraryRackAllocation");
        }
    }

    protected void GrdShelfEntry_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (BlnShelfEntryGoClick == true)
            {
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
            }
            if (BlnShelfEntrySaveClick == true)
            {
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = true;
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (BlnShelfEntryGoClick == true)
            {
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
            }
            if (BlnShelfEntrySaveClick == true)
            {
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = true;
            }
        }
    }

    #endregion

    protected void btn_AddPos_click(object sender, EventArgs e)
    {
        int rowindex = ((GridViewRow)((Button)sender).Parent.Parent).RowIndex;
        string ShelfNO = "";
        string ShelfMaxCap = "";
        string NoOfPos = "";
        TextBox txt_ShelfNO = (TextBox)GrdShelfEntry.Rows[rowindex].FindControl("txt_GrdShlfNo");
        if (txt_ShelfNO.Text.Trim() != "")
        {
            ShelfNO = txt_ShelfNO.Text.Trim();
        }
        TextBox txt_ShelfCap = (TextBox)GrdShelfEntry.Rows[rowindex].FindControl("txt_GrdShlfMaxCap");
        if (txt_ShelfCap.Text.Trim() != "")
        {
            ShelfMaxCap = txt_ShelfCap.Text.Trim();
        }
        TextBox txt_NoOfPos = (TextBox)GrdShelfEntry.Rows[rowindex].FindControl("txt_GrdNoOfPosinShlf");
        if (txt_NoOfPos.Text.Trim() != "")
        {
            NoOfPos = txt_NoOfPos.Text.Trim();
        }

        DivPosEntry.Visible = true;
        Txt_PosShlfNo.Text = ShelfNO;
        Txt_PosShlfNo.Enabled = false;
        Txt_PosShlfSMC.Text = ShelfMaxCap;
        Txt_PosShlfSMC.Enabled = false;
        Txt_NOPShlf.Text = NoOfPos;
        Txt_NOPShlf.Enabled = false;

        #region spread Design
        //SpreadPositionEntry.Sheets[0].RowCount = 0;
        //SpreadPositionEntry.Sheets[0].ColumnCount = 5;
        //SpreadPositionEntry.CommandBar.Visible = false;
        //SpreadPositionEntry.Sheets[0].AutoPostBack = false;
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Rows.Count = 1;
        //SpreadPositionEntry.Sheets[0].RowHeader.Visible = false;

        //FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        //darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        //darkstyle.ForeColor = Color.White;
        //SpreadPositionEntry.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        //SpreadPositionEntry.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        //SpreadPositionEntry.Columns[0].Width = 50;
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 0].Locked = true;
        //SpreadPositionEntry.Columns[0].Locked = true;

        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Position Number";
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
        //SpreadPositionEntry.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
        //SpreadPositionEntry.Columns[1].Width = 150;
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 1].Locked = true;
        //SpreadPositionEntry.Columns[1].Locked = true;

        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Max Capacity";
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
        //SpreadPositionEntry.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
        //SpreadPositionEntry.Columns[2].Width = 150;
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 2].Locked = true;

        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Available Copies";
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
        //SpreadPositionEntry.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
        //SpreadPositionEntry.Columns[3].Width = 150;
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 3].Locked = true;
        //SpreadPositionEntry.Columns[3].Locked = true;

        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Select";
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
        //SpreadPositionEntry.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
        //SpreadPositionEntry.Columns[4].Width = 100;
        //SpreadPositionEntry.Sheets[0].ColumnHeader.Cells[0, 4].Locked = true;

        //SpreadPositionEntry.Sheets[0].PageSize = SpreadPositionEntry.Sheets[0].RowCount;
        //SpreadPositionEntry.Width = 600;
        //SpreadPositionEntry.Height = 300;
        //SpreadPositionEntry.SaveChanges();
        //SpreadPositionEntry.Visible = true;
        //divSpreadPos.Visible = true;

        #endregion
    }

    #region Postion Entry

    protected void txt_PosAcr_OnTextChanged(object sender, EventArgs e)
    {
        txt_PosAcr.Text = txt_PosAcr.Text.ToUpper();
    }

    protected void imagebtnPospopclose_Click(object sender, EventArgs e)
    {
        DivPosEntry.Visible = false;
    }

    protected void BtnPosGo_Click(object sender, EventArgs e)
    {
        if (txt_PosAcr.Text == "" || txt_PosStNo.Text == "")
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Fill the Position details and try search";
        }
        else
        {
            AutoGen_Pos();
            BtnPosSave.Enabled = true;
        }
    }

    public bool PositionValid()
    {
        int SpRowCnt = GrdPosEntry.Rows.Count;
        string Library = Convert.ToString(Session["libCode"]);
        int IntPosCap = 0;
        string RackNo = txt_ShlfRackNo.Text;
        string ShelfMaxCap = Txt_PosShlfSMC.Text;
        string selQry = string.Empty;
        string PosNO = "";
        string PosCap = "";
        if (SpRowCnt == 0)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Enter atleast one row";
        }
        foreach (GridViewRow gvrow in GrdPosEntry.Rows)
        {
            int RowCnt = Convert.ToInt32(gvrow.RowIndex);
            TextBox txt_PosNO = (TextBox)GrdPosEntry.Rows[RowCnt].FindControl("txt_GrdPosNo");
            if (txt_PosNO.Text.Trim() != "")
            {
                PosNO = txt_PosNO.Text.Trim();
            }
            TextBox txt_PosCap = (TextBox)GrdPosEntry.Rows[RowCnt].FindControl("txt_GrdPosMaxCap");
            if (txt_PosCap.Text.Trim() != "")
            {
                PosCap = txt_PosCap.Text.Trim();
            }
            if (PosCap != "")
            {
                IntPosCap = IntPosCap + Convert.ToInt32(PosCap);
            }
            if (PosNO == "")
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Postion No. should not be empty";
                IsPostionValid = false;
            }
            if (PosCap == "")
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Postion Capacity should not be empty";
                IsPostionValid = false;
            }
            if (PosCap == "0")
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Postion Capacity should not be 0";
                IsPostionValid = false;
            }
            if (PosNO != "" && PosCap != "0" & PosCap != "")
            {
                selQry = "SELECT * FROM RowPos_Master WHERE Pos_No ='" + PosNO + "' AND Lib_Code ='" + Library + "' ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selQry, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Position No" + PosNO + " already exist";
                    IsPostionValid = false;
                }
            }
        }
        if (IntPosCap != Convert.ToInt32(ShelfMaxCap))
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Position total capacity should be " + ShelfMaxCap + " ";
            IsPostionValid = false;
        }
        return IsPostionValid;
    }

    protected void AutoGen_Pos()
    {
        string PosAcr = txt_PosAcr.Text;
        string PosStNo = txt_PosStNo.Text;
        string PosShelfNo = Txt_NOPShlf.Text;

        int SpRowCnt = GrdPosEntry.Rows.Count;
        double TotalRacks = 0;
        string Rack = "";
        if (PosShelfNo != "")
        {
            dtPosEntry.Columns.Add("GrdPosNo");
            dtPosEntry.Columns.Add("GrdPosMaxCap");
            dtPosEntry.Columns.Add("AvailableCopy");
            SpRowCnt = SpRowCnt + (Convert.ToInt32(PosShelfNo) - SpRowCnt);
            TotalRacks = Convert.ToDouble(PosStNo);
            for (int i = 0; i < SpRowCnt; i++)
            {
                if (PosAcr != "" && PosStNo != "")
                {
                    Rack = PosAcr + TotalRacks;
                    drow = dtPosEntry.NewRow();
                    drow["GrdPosNo"] = Rack;
                    drow["GrdPosMaxCap"] = "";
                    drow["AvailableCopy"] = "";
                    dtPosEntry.Rows.Add(drow);
                    TotalRacks++;
                }
                GrdPosEntry.DataSource = dtPosEntry;
                GrdPosEntry.DataBind();
                GrdPosEntry.Visible = true;
                divSpreadPos.Visible = true;
            }
        }
    }

    protected void BtnPosSave_Click(object sender, EventArgs e)
    {
        try
        {
            string PosNO = "";
            string PosCap = "";
            if (!PositionValid())
            {
                //txt_TotalRack.Text = "";
                //txt_RackAcr.Text = "";
                //txt_RackstNo.Text = "";
                return;
            }
            else
            {
                Hashtable htSpreadBind = new Hashtable();
                int SpRowCnt = GrdPosEntry.Rows.Count;
                string Library = Convert.ToString(Session["libCode"]);
                string ShelfNo = Txt_PosShlfNo.Text;
                string PosAcr = txt_PosAcr.Text;
                string PosStNo = txt_PosStNo.Text;
                string RackNo = txt_ShlfRackNo.Text;
                string insertQry = "";
                string header = "";
                int insert = 0;
                dtPosEntry.Columns.Add("GrdPosNo");
                dtPosEntry.Columns.Add("GrdPosMaxCap");
                dtPosEntry.Columns.Add("AvailableCopy");
                string sql = "update rack_master set Pos_Acr='" + PosAcr + "',Pos_StNo='" + PosStNo + "' where lib_code='" + Library + "' and rack_no='" + RackNo + "'";

                foreach (GridViewRow gvrow in GrdPosEntry.Rows)
                {
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    TextBox txt_PosNO = (TextBox)GrdPosEntry.Rows[RowCnt].FindControl("txt_GrdPosNo");
                    if (txt_PosNO.Text.Trim() != "")
                    {
                        PosNO = txt_PosNO.Text.Trim();
                    }
                    TextBox txt_PosCap = (TextBox)GrdPosEntry.Rows[RowCnt].FindControl("txt_GrdPosMaxCap");
                    if (txt_PosCap.Text.Trim() != "")
                    {
                        PosCap = txt_PosCap.Text.Trim();
                    }
                    string Time = DateTime.Now.ToString("hh:mm tt");
                    string Date = DateTime.Now.ToString("MM/dd/yyyy");
                    header = PosNO + "$" + PosCap;

                    insertQry = " insert into RowPos_Master (Rack_No,Row_No,Pos_No,Max_Capacity,No_Of_Copies,Access_Date,Access_Time,Lib_Code) VALUES('" + RackNo + "','" + ShelfNo + "','" + PosNO + "','" + PosCap + "','0','" + Date + "','" + Time + "','" + Library + "')";
                    insert = d2.update_method_wo_parameter(insertQry, "TEXT");

                    DataRow drow = dtPosEntry.NewRow();
                    drow["GrdPosNo"] = PosNO;
                    drow["GrdPosMaxCap"] = PosCap;
                    drow["AvailableCopy"] = "";
                    dtPosEntry.Rows.Add(drow);
                }
                if (insert > 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Position Information Sucessfully Saved";
                    GrdPosEntry.DataSource = dtPosEntry;
                    GrdPosEntry.DataBind();
                    GrdPosEntry.Visible = true;
                    divSpreadPos.Visible = true;
                    BtnPosSave.Visible = false;
                    BtnPosSave.Enabled = false;
                    BtnPosDel.Visible = true;
                    BtnPosDel.Enabled = true;
                    BtnPosUpdate.Visible = true;
                    BtnPosUpdate.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "LibraryRackAllocation");
        }
    }

    protected void BtnPosUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            string selQry = "";
            string insertQry = "";
            int insert = 0;
            string updateQry = "";
            string Library = Convert.ToString(Session["libCode"]);
            string PosShlfNo = Txt_PosShlfNo.Text;
            string PosAcr = txt_PosAcr.Text;
            string PosStNo = txt_PosStNo.Text;
            string RackNo = txt_ShlfRackNo.Text;
            string PosNO = "";
            string PosCap = "";
            int selCount = 0;
            foreach (GridViewRow gvrow in GrdPosEntry.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                if (chk.Checked == true)
                {
                    selCount++;
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    TextBox txt_PosNO = (TextBox)GrdPosEntry.Rows[RowCnt].FindControl("txt_GrdPosNo");
                    if (txt_PosNO.Text.Trim() != "")
                    {
                        PosNO = txt_PosNO.Text.Trim();
                    }
                    TextBox txt_PosCap = (TextBox)GrdPosEntry.Rows[RowCnt].FindControl("txt_GrdPosMaxCap");
                    if (txt_PosCap.Text.Trim() != "")
                    {
                        PosCap = txt_PosCap.Text.Trim();
                    }

                    string Time = DateTime.Now.ToString("hh:mm tt");
                    string Date = DateTime.Now.ToString("MM/dd/yyyy");

                    selQry = " SELECT * FROM RowPos_Master WHERE Row_No ='" + PosShlfNo + "' AND Pos_No ='" + PosNO + "' AND Lib_Code ='" + Library + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selQry, "Text");
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        insertQry = "INSERT INTO RowPos_Master(Rack_No,Row_No,Pos_No,Max_Capacity,No_Of_Copies,Access_Date,Access_Time,Lib_Code) VALUES ('" + RackNo + "','" + PosShlfNo + "','" + PosNO + "','" + PosCap + "','0','" + Date + "','" + Time + "','" + Library + "')";
                        insert = d2.update_method_wo_parameter(insertQry, "TEXT");
                    }
                    else
                    {
                        updateQry = "UPDATE RowPos_Master SET Max_Capacity=" + PosCap + ",Access_Date='" + Date + "',Access_Time='" + Time + "' WHERE Row_No ='" + PosShlfNo + "' AND Pos_No ='" + PosNO + "' AND Lib_Code ='" + Library + "' ";
                        insert = d2.update_method_wo_parameter(updateQry, "TEXT");
                    }
                }
            }
            if (insert > 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Postion Information Sucessfully Updated";
            }
            if (selCount == 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Select the row to Update";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "LibraryRackAllocation");
        }
    }

    protected void BtnPosDel_Click(object sender, EventArgs e)
    {
        try
        {
            int selCount = 0;
            foreach (GridViewRow gvrow in GrdPosEntry.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                if (chk.Checked == true)
                {
                    selCount++;
                }
            }
            if (selCount > 0)
            {
                SureDivDeletePosition.Visible = true;
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Select the row to Delete";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "LibraryRackAllocation");
        }
    }

    protected void btn_DeletePosYes_Click(object sender, EventArgs e)
    {
        try
        {
            string selQry = "";
            string deleteQry = "";
            int delete = 0;
            string Library = Convert.ToString(Session["libCode"]);
            string PosShlfNo = Txt_PosShlfNo.Text;
            string PosAcr = txt_PosAcr.Text;
            string PosStNo = txt_PosStNo.Text;
            string RackNo = txt_ShlfRackNo.Text;
            string PosNO = "";
            foreach (GridViewRow gvrow in GrdPosEntry.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                if (chk.Checked == true)
                {
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    TextBox txt_PosNO = (TextBox)GrdPosEntry.Rows[RowCnt].FindControl("txt_GrdPosNo");
                    if (txt_PosNO.Text.Trim() != "")
                    {
                        PosNO = txt_PosNO.Text.Trim();
                    }

                    selQry = "SELECT * FROM Rack_Allocation WHERE  Row_No='" + PosNO + "' AND Lib_Code ='" + Library + "' ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selQry, "Text");

                    deleteQry = "DELETE FROM RowPos_Master WHERE Row_No = '" + PosShlfNo + "' AND Pos_No ='" + PosNO + "' AND Lib_Code =  '" + Library + "' ";
                    delete = d2.update_method_wo_parameter(deleteQry, "TEXT");
                }
            }
            imgdiv2.Visible = true;
            lbl_alert.Text = "Position has been deleted successfully";
            SureDivDeletePosition.Visible = false;

            selQry = "select pos_no,max_capacity from RowPos_Master where Row_No='" + PosShlfNo + "' and Lib_Code='" + Library + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selQry, "Text");
            ViewState["CurrentTable"] = null;
            dtPosEntry.Columns.Add("GrdPosNo");
            dtPosEntry.Columns.Add("GrdPosMaxCap");
            dtPosEntry.Columns.Add("AvailableCopy");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow drow = dtShelfEntry.NewRow();
                drow["GrdPosNo"] = Convert.ToString(ds.Tables[0].Rows[i]["pos_no"]);
                drow["GrdPosMaxCap"] = Convert.ToString(ds.Tables[0].Rows[i]["max_capacity"]);
                drow["AvailableCopy"] = "";
                dtPosEntry.Rows.Add(drow);
            }
            GrdPosEntry.DataSource = dtPosEntry;
            GrdPosEntry.DataBind();
            GrdPosEntry.Visible = true;
            divSpreadPos.Visible = true;

            BtnPosDel.Visible = true;
            BtnPosDel.Enabled = true;
            BtnPosSave.Visible = false;
            BtnPosSave.Enabled = false;
            BtnPosUpdate.Visible = true;
            BtnPosUpdate.Enabled = true;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "LibraryRackAllocation");
        }
    }

    protected void btn_DeletePosNo_Click(object sender, EventArgs e)
    {
        SureDivDeletePosition.Visible = false;
    }

    #endregion

    #region Print

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(GrdRackMaster, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your  Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        { }
    }
    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string ss = "";
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Library Card Master " + '@';
            pagename = "Library_Card_Master.aspx";
            Printcontrolhed2.loadspreaddetails(GrdRackMaster, pagename, degreedetails,0, ss, "1");
            Printcontrolhed2.Visible = true;
        }
        catch { }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    #endregion
}