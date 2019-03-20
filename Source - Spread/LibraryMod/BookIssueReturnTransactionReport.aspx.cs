using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using InsproDataAccess;
using System.Drawing;
using System.Collections;
using System.Configuration;

public partial class LibraryMod_BookIssueReturnTransactionReport : System.Web.UI.Page
{
    string usercollegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string groupusercode = string.Empty;
    string college_code = string.Empty;
    string librcode = string.Empty;
    int dueamount = 0;
    int amounttot = 0;
    int amttot = 0;
    bool flag_true = false;

    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    DataTable dtCommon = new DataTable();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();

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

            usercollegecode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
            usercode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
            singleuser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
            groupusercode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";

            if (!IsPostBack)
            {
                bindclg();
                binddept();
                getLibPrivil();
                bindsem();
                txt_fromdate1.Attributes.Add("readonly", "readonly");
                txt_fromdate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                DateTime FromTime = DateTime.Parse("9:00:00 AM");
                MKB.TimePicker.TimeSelector.AmPmSpec am_pm;
                if (FromTime.ToString("tt") == "AM")
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                }
                else
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                }
                timerselector1.SetTime(FromTime.Hour, FromTime.Minute, FromTime.Second, am_pm);
                DateTime ToTime = DateTime.Parse("5:00:00 PM");
                MKB.TimePicker.TimeSelector.AmPmSpec AM_PM;
                if (ToTime.ToString("tt") == "AM")
                {
                    AM_PM = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                }
                else
                {
                    AM_PM = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                }
                timerselector2.SetTime(ToTime.Hour, ToTime.Minute, ToTime.Second, AM_PM);
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

            dtCommon.Clear();
            ddl_collegename.Enabled = false;
            ds.Clear();
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
            //d2.sendErrorMail(ex, userCollegeCode, "LibraryRackAllocation");
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
            if (singleuser.ToLower() == "true")
            {
                sql = "SELECT DISTINCT lib_code from lib_privileges where user_code=" + usercode + " and lib_code in (select lib_code from library where college_code=" + coll_Code + ")";
                ds.Clear();
                ds = d2.select_method_wo_parameter(sql, "text");
            }
            else
            {
                string[] groupUser = groupusercode.Split(';');
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

    public void bindLibrary(string libcode)
    {
        cbl_library.Items.Clear();
        ds.Clear();
        string collegecode = Convert.ToString(ddl_collegename.SelectedValue);
        string SelectQ = string.Empty;

        string lib_name = "select lib_code,lib_name,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + libcode + " AND college_code=" + collegecode + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
        ds = d2.select_method_wo_parameter(lib_name, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_library.DataSource = ds;
            cbl_library.DataTextField = "lib_name";
            cbl_library.DataValueField = "lib_code";
            cbl_library.DataBind();
            if (cbl_library.Items.Count > 0)
            {
                for (int i = 0; i < cbl_library.Items.Count; i++)
                {
                    cbl_library.Items[i].Selected = true;
                }
                cb_lib.Checked = true;
            }
        }

    }

    public void binddept()
    {
        try
        {

            cbl_dept.Items.Clear();
            string deptquery = string.Empty;
            ds.Clear();
            //if (checkusers.Items[0].Selected || checkusers.Items[0].Selected && checkusers.Items[2].Selected)
            //{
            deptquery = "Select dept_code,(Dept_name) from department order by dept_code";
            ds = d2.select_method_wo_parameter(deptquery, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_dept.DataSource = ds;
                cbl_dept.DataTextField = "Dept_name";
                cbl_dept.DataValueField = "dept_code";
                cbl_dept.DataBind();
                if (cbl_dept.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_dept.Items.Count; i++)
                    {
                        cbl_dept.Items[i].Selected = true;
                    }
                    cb_dept.Checked = true;
                    //}
                }

            }
        }

        catch
        {
        }
    }

    protected void bindsem()
    {
        try
        {
            cbl_sem.Items.Clear();
            string semquery = string.Empty;
            string collegecode = Convert.ToString(ddl_collegename.SelectedValue);
            ds.Clear();
            semquery = "SELECT * FROM TEXTVALTABLE WHERE textcriteria='FEECA' and college_code in ('" + collegecode + "')";
            ds = d2.select_method_wo_parameter(semquery, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_sem.DataSource = ds;
                cbl_sem.DataTextField = "textval";
                cbl_sem.DataValueField = "textcode";
                cbl_sem.DataBind();
                if (cbl_sem.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_sem.Items.Count; i++)
                    {
                        cbl_sem.Items[i].Selected = true;
                    }
                    cb_sem.Checked = true;
                }
            }
        }
        catch { }
    }

    protected void ddl_collegename_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        getLibPrivil();
    }

    protected void cbl_users_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox sampletext = new TextBox();
            CallCheckboxListChange(Cb_user, checkusers, sampletext, "User", "---Select---");
            binddept();
        }
        catch
        {
        }
    }

    protected void cbl_library_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cb_lib, cbl_library, sampleTxt, "Libraryname", "-Select--");

    }

    protected void cbdate1_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cbdate1.Checked == true)
        {
            txt_fromdate1.Enabled = true;
            txt_todate.Enabled = true;
        }
        else
        {
            txt_fromdate1.Enabled = false;
            txt_todate.Enabled = false;
        }
    }

    protected void cbtime_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cbtime1.Checked == true)
        {
            timerselector1.Enabled = true;
            timerselector2.Enabled = true;
        }
        else
        {
            timerselector1.Enabled = false;
            timerselector2.Enabled = false;
        }
    }

    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cb_sem, cbl_sem, sampleTxt, "Semester", "--Select--");

    }

    protected void rbltransactions_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbltransactions.SelectedIndex == 0)
        {
            rbldailyacttrans.Visible = false;
        }
        else if (rbltransactions.SelectedIndex == 1)
        {
            rbldailyacttrans.Visible = true;
        }
        else
        {
            rbldailyacttrans.Visible = false;
        }
    }

    protected void rbldailyacttrans_OnSelectedIndexChanged(object sender, EventArgs e)
    {
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

    private string getCblSelectedTextwithout(CheckBoxList cblSelected)
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
                        selectedText.Append("," + Convert.ToString(cblSelected.Items[sel].Text));
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

    protected void btngo_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (rbltransactions.SelectedIndex == 0)
            {
                BestBookTransaction(sender, e);
            }
            else if (rbltransactions.SelectedIndex == 1)
            {
                if (rbldailyacttrans.SelectedIndex == 0)
                {
                    DailyActivity(sender, e);
                }
                else if (rbldailyacttrans.SelectedIndex == 1)
                {
                    WeeklyActivity(sender, e);
                }
                else if (rbldailyacttrans.SelectedIndex == 2)
                {
                    MonthlyActivity(sender, e);
                }
                else if (rbldailyacttrans.SelectedIndex == 3)
                {
                    YearlyActivity(sender, e);
                }
            }
            else
            {
                OverDueMemebersList(sender, e);
            }
        }
        catch
        {
        }
    }

    //public void //spreadDesign()
    //{
    //    try
    //    {
    //        FpSpread1.Sheets[0].RowCount = 0;
    //        FpSpread1.CommandBar.Visible = false;
    //        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
    //        FpSpread1.Sheets[0].RowHeader.Visible = false;
    //        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
    //        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
    //        darkstyle.ForeColor = Color.Black;
    //        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
    //        darkstyle.Font.Name = "Book Antiqua";
    //        darkstyle.Font.Size = FontUnit.Medium;
    //        darkstyle.HorizontalAlign = HorizontalAlign.Center;
    //        darkstyle.VerticalAlign = VerticalAlign.Middle;
    //        //FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
    //        //FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;

    //        //FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
    //        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chkall;
    //        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center; chk.AutoPostBack = true;
    //        //chkall.AutoPostBack = true;
    //        if (rbltransactions.SelectedIndex == 0)
    //        {

    //            FpSpread1.Sheets[0].ColumnCount = 7;
    //            FpSpread1.Sheets[0].Columns[1].Width = 118;
    //            FpSpread1.Sheets[0].Columns[2].Width = 180;
    //            FpSpread1.Sheets[0].Columns[3].Width = 300;


    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "SNo";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Name";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Course";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total Trans";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Late Returns";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Returns";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
    //        }
    //        else if (rbltransactions.SelectedIndex == 1)
    //        {
    //            FpSpread1.Sheets[0].ColumnCount = 9;
    //            FpSpread1.Sheets[0].Columns[1].Width = 230;
    //            //FpSpread1.Sheets[0].Columns[4].Width = 180;
    //            //FpSpread1.Sheets[0].Columns[5].Width = 100;
    //            //FpSpread1.Sheets[0].Columns[6].Width = 135;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Sno";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Total Issue";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Total Return";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total Renew";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
    //            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Total Trans";
    //            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Total Reservation";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "OPAC Hits";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Gate Entry";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Total Trans";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;


    //        }
    //        else
    //        {
    //            FpSpread1.Sheets[0].ColumnCount = 10;
    //            FpSpread1.Sheets[0].Columns[0].Width = 80;
    //            FpSpread1.Sheets[0].Columns[1].Width = 80;
    //            FpSpread1.Sheets[0].Columns[2].Width = 250;
    //            FpSpread1.Sheets[0].Columns[4].Width = 280;
    //            FpSpread1.Sheets[0].Columns[8].Width = 60;
    //            FpSpread1.Sheets[0].Columns[9].Width = 60;
    //            FpSpread1.Sheets[0].Columns[3].Width = 80;
    //            FpSpread1.Sheets[0].Columns[5].Width = 120;
    //            FpSpread1.Sheets[0].Columns[6].Width = 200;
    //            //FpSpread1.Sheets[0].Columns[4].Width = 180;
    //            //FpSpread1.Sheets[0].Columns[5].Width = 100;
    //            //FpSpread1.Sheets[0].Columns[6].Width = 135;
    //            FpSpread1.Width = 1300;

    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Sno";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Library";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Acc No";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Title";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Token No";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Course";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Due Date";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Over Due Days";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Over Due Amount";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;

    //        }

    //    }
    //    catch
    //    {
    //    }
    //}

    protected void BestBookTransaction(object sender, EventArgs e)
    {
        try
        {
            string dateqry = string.Empty;
            double latereturn = 0;
            string library = getCblSelectedValue(cbl_library);
            string dept = getCblSelectedValue(cbl_dept);

            string sem1 = getCblSelectedText(cbl_sem);
            string[] Semster = sem1.Split(new string[] { "','" }, StringSplitOptions.None);
            string SemVal = string.Empty;
            string sem = "";
            for (int i = 0; i < Semster.Length; i++)
            {
                SemVal = Semster[i];
                string SemCode = SemVal.Split(' ')[0];
                if (!sem.Contains(SemCode))
                {
                    if (sem == "")
                        sem = SemCode;
                    else
                        sem = sem + "','" + SemCode;
                }
            }
            DataTable dtBestBookTransaction = new DataTable();
            DataRow drow;
            dtBestBookTransaction.Columns.Add("SNo", typeof(string));
            dtBestBookTransaction.Columns.Add("Roll No", typeof(string));
            dtBestBookTransaction.Columns.Add("Name", typeof(string));
            dtBestBookTransaction.Columns.Add("Course", typeof(string));
            dtBestBookTransaction.Columns.Add("Total Trans", typeof(string));
            dtBestBookTransaction.Columns.Add("Late Returns", typeof(string));
            dtBestBookTransaction.Columns.Add("Returns", typeof(string));


            drow = dtBestBookTransaction.NewRow();
            drow["SNo"] = "SNo";
            drow["Roll No"] = "Roll No";
            drow["Name"] = "Name";
            drow["Course"] = "Course";
            drow["Total Trans"] = "Total Trans";
            drow["Late Returns"] = "Late Returns";
            drow["Returns"] = "Returns";
            dtBestBookTransaction.Rows.Add(drow);

            string colgcode = Convert.ToString(ddl_collegename.SelectedValue);
            string libraryname = Convert.ToString(cbl_library.SelectedValue).Trim();
            if (cbdate1.Checked == true)
            {
                string fromdate = string.Empty;
                fromdate = txt_fromdate1.Text;
                DateTime dt = new DateTime();
                dt = Convert.ToDateTime(fromdate);
                fromdate = dt.ToString("yyyy/MM/dd");

                string todate = string.Empty;
                todate = txt_todate.Text;
                DateTime dt1 = new DateTime();
                dt1 = Convert.ToDateTime(todate);
                todate = dt1.ToString("yyyy/MM/dd");

                dateqry = " and borrow_date between '" + fromdate + "' and '" + todate + "'";
            }
            else
            {
                dateqry = "";
            }
            string qry = string.Empty;

            #region Student

            if (checkusers.Items[0].Selected == true && checkusers.Items[1].Selected == false)
            {
                qry = "select b.lib_code,lib_name,b.roll_no,b.stud_name,course_name+'-'+dept_name as Course,count(*) tottrans from borrow b,library l,registration r,degree g,course c,department d where l.lib_code = b.lib_code and r.roll_no = b.roll_no and g.degree_code = r.degree_code and c.course_id = g.course_id and d.dept_code = g.dept_code  " + dateqry + " and b.is_staff = 0";
                if (library != "")
                {
                    qry = qry + " and b.lib_code in ('" + library + "')";
                }
                if (dept != "")
                {
                    qry = qry + " and d.dept_code in ('" + dept + "')";
                }
                if (sem != "")
                {
                    qry = qry + " and current_semester in('" + sem + "')";
                }

                qry = qry + " group by b.lib_code,lib_name,b.roll_no,b.stud_name,course_name,dept_name order by lib_name,tottrans desc";
                ds.Clear();
                ds = d2.select_method_wo_parameter(qry, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int sno = 0;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sno++;
                        string studrollno = Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]).Trim();
                        string studname = Convert.ToString(ds.Tables[0].Rows[i]["stud_name"]).Trim();
                        string course = Convert.ToString(ds.Tables[0].Rows[i]["Course"]).Trim();
                        string totaltrans = Convert.ToString(ds.Tables[0].Rows[i]["tottrans"]).Trim();
                        qry = "select b.lib_code,count(*) as TotDays from borrow b inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code where return_date > due_date and  b.is_staff =0 and b.roll_no ='" + studrollno + "' and  g.college_code='" + colgcode + "'";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,b.roll_no";

                        ds1.Clear();
                        ds1 = d2.select_method_wo_parameter(qry, "text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            latereturn = Convert.ToDouble(ds1.Tables[0].Rows[0]["TotDays"]);
                        }
                        string retur = string.Empty;
                        double ret = Convert.ToDouble(totaltrans) - latereturn;
                        retur = Convert.ToString(ret);
                        drow = dtBestBookTransaction.NewRow();
                        drow["SNo"] = Convert.ToString(sno);
                        drow["Roll No"] = Convert.ToString(studrollno);
                        drow["Name"] = Convert.ToString(studname);
                        drow["Course"] = Convert.ToString(course);
                        drow["Total Trans"] = Convert.ToString(totaltrans);
                        drow["Late Returns"] = Convert.ToString(latereturn);
                        drow["Returns"] = Convert.ToString(retur);
                        dtBestBookTransaction.Rows.Add(drow);
                    }
                }
            }

            #endregion

            #region Staff

            else if (checkusers.Items[1].Selected == true && checkusers.Items[0].Selected == false)
            {
                qry = "select b.lib_code,lib_name,b.roll_no,b.stud_name,dept_name as Course,count(*) tottrans from borrow b inner join library l on l.lib_code = b.lib_code inner join staffmaster m on m.staff_code = b.roll_no inner join stafftrans t on t.staff_code = m.staff_code inner join department d on d.dept_name = t.dept_code and d.college_code='" + colgcode + "' and   b.is_staff = 1 and t.latestrec = 1 " + dateqry + "";

                if (library != "")
                {
                    qry = qry + " and b.lib_code in('" + library + "')";
                }
                if (dept != "")
                {
                    qry = qry + " and d.dept_code in('" + dept + "')";
                }
                qry = qry + " group by b.lib_code,lib_name,b.roll_no,b.stud_name,dept_name order by lib_name,tottrans desc";

                ds.Clear();
                ds = d2.select_method_wo_parameter(qry, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int sno = 0;
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        string rollno = Convert.ToString(ds.Tables[0].Rows[j]["roll_no"]).Trim();
                        string name = Convert.ToString(ds.Tables[0].Rows[j]["stud_name"]).Trim();
                        string course = Convert.ToString(ds.Tables[0].Rows[j]["Course"]).Trim();
                        string tottrans = Convert.ToString(ds.Tables[0].Rows[j]["tottrans"]).Trim();
                        qry = "select b.lib_code,count(*) as Totdays from borrow b inner join staffmaster m on m.staff_code = b.roll_no inner join stafftrans t on t.staff_code = m.staff_code inner join department d on d.dept_code = t.dept_code where return_date > due_date and b.roll_no ='" + rollno + "' and d.college_code='" + colgcode + "' and b.is_staff =1 and t.latestrec = 1";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        qry = qry + " group by b.lib_code,b.roll_no";
                        ds1.Clear();
                        ds1 = d2.select_method_wo_parameter(qry, "text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            latereturn = Convert.ToDouble(ds1.Tables[0].Rows[0]["Totdays"]);
                        }
                        string retur = string.Empty;
                        double ret = Convert.ToDouble(tottrans) - latereturn;
                        retur = Convert.ToString(ret);
                        drow = dtBestBookTransaction.NewRow();
                        drow["SNo"] = Convert.ToString(sno);
                        drow["Roll No"] = Convert.ToString(rollno);
                        drow["Name"] = Convert.ToString(name);
                        drow["Course"] = Convert.ToString(course);
                        drow["Total Trans"] = Convert.ToString(tottrans);
                        drow["Late Returns"] = Convert.ToString(latereturn);
                        drow["Returns"] = Convert.ToString(retur);
                        dtBestBookTransaction.Rows.Add(drow);
                    }
                }
            }

            #endregion

            #region All

            if (checkusers.Items[0].Selected == true && checkusers.Items[1].Selected == true)
            {
                qry = "select b.lib_code,lib_name,b.roll_no,b.stud_name,course_name+'-'+dept_name Course,is_staff,count(*) tottrans from borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code and g.college_code='" + colgcode + "'  and b.is_staff = 0 " + dateqry + "";

                if (library != "")
                {
                    qry = qry + " and b.lib_code in('" + library + "')";
                }
                if (dept != "")
                {
                    qry = qry + " and d.dept_code in('" + dept + "')";
                }
                if (sem != "")
                {
                    qry = qry + " and r.current_semester in('" + sem + "')";
                }
                qry = qry + " group by b.lib_code,lib_name,b.roll_no,b.stud_name,course_name,dept_name,is_staff";

                qry = qry + " UNION ALL select  b.lib_code,lib_name,b.roll_no,b.stud_name,dept_name Course,is_staff,count(*) tottrans from borrow b inner join library l on l.lib_code = b.lib_code inner join staffmaster m on m.staff_code = b.roll_no inner join stafftrans t on t.staff_code = m.staff_code inner join department d on cast(d.dept_code as nvarchar) = t.dept_code and d.college_code='" + colgcode + "' and b.is_staff = 1 and t.latestrec = 1 ";

                if (library != "")
                {
                    qry = qry + " and b.lib_code in('" + library + "')";
                }
                if (dept != "")
                {
                    qry = qry + " and d.dept_code in('" + dept + "')";
                }
                qry = qry + " group by b.lib_code,lib_name,b.roll_no,b.stud_name,dept_name,is_staff order by lib_name,tottrans desc";
                ds.Clear();
                ds = d2.select_method_wo_parameter(qry, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                    {
                        string rollno = Convert.ToString(ds.Tables[0].Rows[k]["roll_no"]).Trim();
                        string name = Convert.ToString(ds.Tables[0].Rows[k]["stud_name"]).Trim();
                        string course = Convert.ToString(ds.Tables[0].Rows[k]["Course"]).Trim();
                        string tottrans = Convert.ToString(ds.Tables[0].Rows[k]["tottrans"]).Trim();
                        string isstaff = Convert.ToString(ds.Tables[0].Rows[k]["is_staff"]).Trim();

                        if (isstaff == "0")
                        {
                            qry = "select lib_code,count(*) as totdays from borrow b inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code where return_date > due_date and b.roll_no ='" + rollno + "' and g.college_code='" + colgcode + "' and b.is_staff =0";

                            if (library != "")
                            {
                                qry = qry + " and b.lib_code in('" + library + "')";
                            }
                            if (dept != "")
                            {
                                qry = qry + " and d.dept_code in('" + dept + "')";
                            }
                            if (sem != "")
                            {
                                qry = qry + " and r.current_semester in('" + sem + "')";
                            }

                            qry = qry + " group by b.lib_code,b.roll_no";
                        }
                        else
                        {
                            qry = "select lib_code,count(*) as totdays from borrow b inner join staffmaster m on m.staff_code = b.roll_no inner join stafftrans t on t.staff_code = m.staff_code inner join department d on cast(d.dept_code as nvarchar) = t.dept_code  where return_date > due_date and b.roll_no ='" + rollno + "' and d.college_code='" + colgcode + "' and b.is_staff =1 and t.latestrec = 1";
                            if (library != "")
                            {
                                qry = qry + " and b.lib_code in('" + library + "')";
                            }
                            if (dept != "")
                            {
                                qry = qry + " and d.dept_code in('" + dept + "')";
                            }

                            qry = qry + "group by lib_code,b.roll_no";
                        }
                        ds1.Clear();
                        ds1 = d2.select_method_wo_parameter(qry, "text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            latereturn = Convert.ToDouble(ds1.Tables[0].Rows[0]["totdays"]);
                        }
                        string retur = string.Empty;
                        double ret = Convert.ToDouble(tottrans) - latereturn;
                        retur = Convert.ToString(ret);
                        drow = dtBestBookTransaction.NewRow();
                        drow["Roll No"] = Convert.ToString(rollno);
                        drow["Name"] = Convert.ToString(name);
                        drow["Course"] = Convert.ToString(course);
                        drow["Total Trans"] = Convert.ToString(tottrans);
                        drow["Late Returns"] = Convert.ToString(latereturn);
                        drow["Returns"] = Convert.ToString(retur);
                        dtBestBookTransaction.Rows.Add(drow);
                    }
                }
            }
            #endregion

            if (ds.Tables[0].Rows.Count > 0)
            {
                divtable.Visible = true;
                grdBkIssTransReport.DataSource = dtBestBookTransaction;
                grdBkIssTransReport.DataBind();
                grdBkIssTransReport.Visible = true;
                GrdOverDueMemebersList.Visible = false;
                btn_printmaster.Visible = true;
                btn_Excel.Visible = true;
                lbl_reportname.Visible = true;
                txt_excelname.Visible = true;
                div_report.Visible = true;
                btnPopAlertClose.Visible = false;
                divPopupAlert.Visible = false;
                RowHead(grdBkIssTransReport);
            }
            else
            {
                divtable.Visible = false;
                grdBkIssTransReport.Visible = false;
                btn_printmaster.Visible = false;
                btn_Excel.Visible = false;
                lbl_reportname.Visible = false;
                txt_excelname.Visible = false;
                div_report.Visible = false;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Records Found";
                btnPopAlertClose.Visible = true;
                divPopupAlert.Visible = true;
                GrdOverDueMemebersList.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void DailyActivity(object sender, EventArgs e)
    {
        try
        {
            string dateqry = string.Empty;
            string totissue = string.Empty;
            string totreturn = string.Empty;
            string totrenew = string.Empty;
            string totreservation = string.Empty;
            string opacHits = string.Empty;
            string gateentry = string.Empty;
            string tottrans = string.Empty;
            int opac = 1;
            if (cbdate1.Checked == false)
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Select The Date";
                divPopupAlert.Visible = true;
                btnPopAlertClose.Visible = true;
                return;
            }
            string library = getCblSelectedValue(cbl_library);
            string dept = getCblSelectedValue(cbl_dept);
            string fromdate = string.Empty;
            string sem1 = getCblSelectedText(cbl_sem);
            string[] Semster = sem1.Split(new string[] { "','" }, StringSplitOptions.None);
            string SemVal = string.Empty;
            string sem = "";
            for (int i = 0; i < Semster.Length; i++)
            {
                SemVal = Semster[i];
                string SemCode = SemVal.Split(' ')[0];
                if (!sem.Contains(SemCode))
                {
                    if (sem == "")
                        sem = SemCode;
                    else
                        sem = sem + "','" + SemCode;
                }
            }
            string colgcode = Convert.ToString(ddl_collegename.SelectedValue);
            string libraryname = Convert.ToString(cbl_library.SelectedValue).Trim();
            string qry = string.Empty;
            DateTime dtfrom = new DateTime();
            DateTime dtToDate = new DateTime();
            if (cbdate1.Checked == true)
            {
                fromdate = txt_fromdate1.Text;
                string[] dtdate = fromdate.Split('/');
                if (dtdate.Length == 3)
                    fromdate = dtdate[1].ToString() + "/" + dtdate[0].ToString() + "/" + dtdate[2].ToString();
                dtfrom = Convert.ToDateTime(fromdate);

                //fromdate = dtfrom.ToString("yyyy/MM/dd");
                string todate = string.Empty;
                todate = txt_todate.Text;
                string[] dtdateTo = todate.Split('/');
                if (dtdateTo.Length == 3)
                    todate = dtdateTo[1].ToString() + "/" + dtdateTo[0].ToString() + "/" + dtdateTo[2].ToString();
                dtToDate = Convert.ToDateTime(todate);

            }
            else
            {
                dateqry = "";
            }
            int UserCount = 0;
            string UserCat = "";
            for (int check = 0; check < checkusers.Items.Count; check++)
            {
                if (checkusers.Items[check].Selected == true)
                {
                    UserCount++;
                    if (UserCat == "")
                        UserCat = Convert.ToString(check);
                    else
                        UserCat = UserCat + "," + Convert.ToString(check);
                }
            }
            int tottissue = 0;
            int tottreturn = 0;
            int tottrenew = 0;
            int tottreser = 0;
            int totopachits = 0;
            int totgateentry = 0;
            int totttran = 0;
            DataTable dtDailyActivity = new DataTable();
            DataRow drow;

            dtDailyActivity.Columns.Add("Date", typeof(string));
            dtDailyActivity.Columns.Add("Total Issue", typeof(string));
            dtDailyActivity.Columns.Add("Total Return", typeof(string));
            dtDailyActivity.Columns.Add("Total Renew", typeof(string));
            dtDailyActivity.Columns.Add("Total Reservation", typeof(string));
            dtDailyActivity.Columns.Add("OPAC Hits", typeof(string));
            dtDailyActivity.Columns.Add("Gate Entry", typeof(string));
            dtDailyActivity.Columns.Add("Total Trans", typeof(string));


            drow = dtDailyActivity.NewRow();
            drow["Date"] = "Date";
            drow["Total Issue"] = "Total Issue";
            drow["Total Return"] = "Total Return";
            drow["Total Renew"] = "Total Renew";
            drow["Total Reservation"] = "Total Reservation";
            drow["OPAC Hits"] = "OPAC Hits";
            drow["Gate Entry"] = "Gate Entry";
            drow["Total Trans"] = "Total Trans";

            dtDailyActivity.Rows.Add(drow);

            if (dtfrom <= dtToDate)
            {
                for (; dtfrom <= dtToDate; )
                {

                    #region Student

                    if (checkusers.Items[0].Selected && checkusers.Items[1].Selected == false)
                    {

                        qry = "select b.lib_code,lib_name,borrow_date,count(*) TotIssue From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where 1=1 and g.college_code='" + colgcode + "' and borrow_date ='" + dtfrom + "' and b.is_staff = 0";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,borrow_date order by lib_name,borrow_date";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totissue = Convert.ToString(ds.Tables[0].Rows[0]["TotIssue"]).Trim();
                        }
                        else
                        {
                            totissue = "0";
                        }
                        qry = "select b.lib_code,lib_name,return_date,count(*) TotRet From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where return_flag = 1 and g.college_code='" + colgcode + "' and return_date ='" + dtfrom + "' and b.is_staff =0";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,return_date";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreturn = Convert.ToString(ds.Tables[0].Rows[0]["TotRet"]).Trim();
                        }
                        else
                        {
                            totreturn = "0";
                        }
                        qry = " select b.lib_code,lib_name,borrow_date,count(*) TotRenew From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where return_flag =  0 and isnull(renewflag,0) = 1 and g.college_code='" + colgcode + "' and borrow_date ='" + dtfrom + "' and b.is_staff =0";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,borrow_date";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totrenew = Convert.ToString(ds.Tables[0].Rows[0]["TotRenew"]).Trim();
                        }
                        else
                        {
                            totrenew = "0";
                        }
                        qry = "Select b.lib_code,lib_name,count(*) TotReserv from priority_studstaff b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code where cur_date ='" + dtfrom + "' and g.college_code='" + colgcode + "' and b.roll_no <> ''";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,cur_date";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreservation = Convert.ToString(ds.Tables[0].Rows[0]["TotReserv"]).Trim();
                        }
                        else
                        {
                            totreservation = "0";
                        }
                        if (opac == 1)
                        {
                            qry = " select isnull(sum(lib_count),0) Tot_OPAC from lib_queryhit b where lib_date ='" + dtfrom + "' and is_staff = 0";
                            if (dept != "")
                            {
                                qry = qry + "  and (b.department in('" + cbl_dept.SelectedItem.ToString() + "') or b.department = 'All')";
                            }
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(qry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                opacHits = Convert.ToString(ds.Tables[0].Rows[0]["Tot_OPAC"]);
                            }
                            else
                            {
                                opacHits = "0";
                            }
                        }
                        qry = "SELECT ISNULL(SUM(A.Tot),0) TotGate FROM (SELECT Count(*) Tot FROM LibUsers U WHERE Entry_Date ='" + dtfrom + "' AND UserCat ='Student'";
                        if (library != "")
                        {
                            qry = qry + " and lib_code in('" + library + "')";
                        }
                        // if (dept != "")
                        //{
                        //    qry = qry + " and dept_name in('" + dept + "')";
                        //}
                        if (sem != "")
                        {
                            qry = qry + " and current_semester in('" + sem + "')";
                        }
                        qry = qry + " GROUP BY Roll_No,Lib_Code) A";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            gateentry = Convert.ToString(ds.Tables[0].Rows[0]["TotGate"]).Trim();
                        }
                        else
                        {
                            gateentry = "0";
                        }
                        int tottran = Convert.ToInt32(totissue) + Convert.ToInt32(totreturn) + Convert.ToInt32(totrenew) + Convert.ToInt32(totreservation);
                        if (tottran == null)
                        {
                            tottran = 0;
                        }
                        string frm_date1 = Convert.ToString(dtfrom);
                        string[] dtdate1 = frm_date1.Split('/');
                        if (dtdate1.Length == 3)
                            frm_date1 = dtdate1[1].ToString() + "/" + dtdate1[0].ToString() + "/" + dtdate1[2].ToString();
                        drow = dtDailyActivity.NewRow();


                        drow["Date"] = Convert.ToString(frm_date1.Split(' ')[0]);
                        drow["Total Issue"] = Convert.ToString(totissue);
                        drow["Total Return"] = Convert.ToString(totreturn);
                        drow["Total Renew"] = Convert.ToString(totrenew);
                        drow["Total Reservation"] = Convert.ToString(totreservation);
                        drow["OPAC Hits"] = Convert.ToString(opacHits);
                        drow["Gate Entry"] = Convert.ToString(gateentry);
                        drow["Total Trans"] = Convert.ToString(tottran);
                        dtDailyActivity.Rows.Add(drow);

                        tottissue = tottissue + Convert.ToInt32(totissue);
                        tottreturn = tottreturn + Convert.ToInt32(totreturn);
                        tottrenew = tottrenew + Convert.ToInt32(totrenew);
                        tottreser = tottreser + Convert.ToInt32(totreservation);
                        totopachits = totopachits + Convert.ToInt32(opacHits);
                        totgateentry = totgateentry + Convert.ToInt32(gateentry);
                        totttran = totttran + Convert.ToInt32(tottran);


                        dtfrom = dtfrom.AddDays(1);
                    }




                    #endregion

                    #region Staff
                    if (checkusers.Items[1].Selected && checkusers.Items[0].Selected == false)
                    {
                        qry = "select b.lib_code,lib_name,borrow_date,count(*) TotIssue From borrow b inner join library l on l.lib_code = b.lib_code inner join staffmaster m on m.staff_code = b.roll_no inner join stafftrans t on t.staff_code = m.staff_code inner join department d on d.dept_code = t.dept_code Where 1=1 and m.college_code='" + colgcode + "' and borrow_date ='" + fromdate + "'and b.is_staff = 1 and t.latestrec = 1";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        else if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }

                        qry = qry + " group by b.lib_code,lib_name,borrow_date order by lib_name,borrow_date";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totissue = Convert.ToString(ds.Tables[0].Rows[0]["TotIssue"]).Trim();
                        }
                        else
                        {
                            totissue = "0";
                        }
                        qry = "select b.lib_code,lib_name,return_date,count(*) TotRet From borrow b inner join library l on l.lib_code = b.lib_code inner join staffmaster m on m.staff_code = b.roll_no inner join stafftrans t on t.staff_code = m.staff_code inner join department d on d.dept_code = t.dept_code Where 1=1 AND return_flag = 1 and return_date ='" + fromdate + "' and m.college_code='" + colgcode + "' and b.is_staff =1 and t.latestrec = 1";

                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        else if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,return_date";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreturn = Convert.ToString(ds.Tables[0].Rows[0]["TotRet"]).Trim();
                        }
                        else
                        {
                            totreturn = "0";
                        }
                        qry = " select b.lib_code,lib_name,borrow_date,count(*) TotRenew From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where return_flag =  0 and isnull(renewflag,0) = 1 and g.college_code='" + colgcode + "' and borrow_date ='" + fromdate + "' and b.is_staff =1";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        else if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        else if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,borrow_date";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totrenew = Convert.ToString(ds.Tables[0].Rows[0]["TotRenew"]).Trim();
                        }
                        else
                        {
                            totrenew = "0";
                        }
                        qry = "Select b.lib_code,lib_name,count(*) TotReserv from priority_studstaff b inner join library l on l.lib_code = b.lib_code inner join staffmaster m on m.staff_code = b.roll_no inner join stafftrans t on t.staff_code = m.staff_code inner join department d on d.dept_code = t.dept_code where cur_date ='" + fromdate + "' and m.college_code='" + colgcode + "' and  b.staff_code <> ''";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,cur_date";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreservation = Convert.ToString(ds.Tables[0].Rows[0]["TotReserv"]).Trim();
                        }
                        else
                        {
                            totreservation = "0";
                        }
                        if (opac == 1)
                        {
                            qry = " select sum(lib_count) Tot_OPAC from lib_queryhit b where lib_date  ='" + fromdate + "' and  is_staff = 1";
                            if (dept != "")
                            {
                                qry = qry + "  and (b.department in('" + cbl_dept.SelectedItem.ToString() + "') or b.department = 'All')";
                            }
                            qry = qry + "group by lib_date";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(qry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                opacHits = Convert.ToString(ds.Tables[0].Rows[0]["Tot_OPAC"]);
                            }
                            else
                            {
                                opacHits = "0";
                            }
                        }
                        qry = "SELECT ISNULL(SUM(A.Tot),0) TotGate  FROM (SELECT Count(*) Tot FROM LibUsers WHERE Entry_Date ='" + fromdate + "' AND UserCat ='Staff'";
                        if (library != "")
                        {
                            qry = qry + " and lib_code in('" + library + "')";
                        }
                        qry = qry + " GROUP BY Roll_No) A";
                        //if (dept != "")
                        //{
                        //    qry = qry + " and dept_name in('" + dept + "')";
                        //}
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            gateentry = Convert.ToString(ds.Tables[0].Rows[0]["TotGate"]).Trim();
                        }
                        else
                        {
                            gateentry = "0";
                        }
                        int tottran = Convert.ToInt32(totissue) + Convert.ToInt32(totreturn) + Convert.ToInt32(totrenew) + Convert.ToInt32(totreservation);
                        if (tottran == null)
                        {
                            tottran = 0;
                        }
                        string frm_date1 = Convert.ToString(dtfrom);
                        string[] dtdate1 = frm_date1.Split('/');
                        if (dtdate1.Length == 3)
                            frm_date1 = dtdate1[1].ToString() + "/" + dtdate1[0].ToString() + "/" + dtdate1[2].ToString();
                        drow = dtDailyActivity.NewRow();
                        drow["Date"] = Convert.ToString(frm_date1.Split(' ')[0]);
                        drow["Total Issue"] = Convert.ToString(totissue);
                        drow["Total Return"] = Convert.ToString(totreturn);
                        drow["Total Renew"] = Convert.ToString(totrenew);
                        drow["Total Reservation"] = Convert.ToString(totreservation);
                        drow["OPAC Hits"] = Convert.ToString(opacHits);
                        drow["Gate Entry"] = Convert.ToString(gateentry);
                        drow["Total Trans"] = Convert.ToString(tottran);
                        dtDailyActivity.Rows.Add(drow);
                    }
                    #endregion

                    #region All
                    if (checkusers.Items[0].Selected == true && checkusers.Items[1].Selected == true)
                    {
                        qry = "select b.lib_code,lib_name,borrow_date,count(*) TotIssue From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where 1=1 and g.college_code='" + colgcode + "' and b.is_staff = 0 and  borrow_date ='" + fromdate + "'and b.is_staff = 0 ";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + "and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,borrow_date";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totissue = Convert.ToString(ds.Tables[0].Rows[0]["TotIssue"]).Trim();
                        }
                        else
                        {
                            totissue = "0";
                        }
                        qry = "select b.lib_code,lib_name,borrow_date,count(*) TotIssue From borrow b inner join library l on l.lib_code = b.lib_code inner join staffmaster m on m.staff_code = b.roll_no inner join stafftrans t on t.staff_code = m.staff_code inner join department d on d.dept_code = t.dept_code Where 1=1 and m.college_code='" + colgcode + "' and borrow_date ='" + fromdate + "'and b.is_staff = 1 and t.latestrec = 1";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,borrow_date order by lib_name,borrow_date";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string issue = Convert.ToString(ds.Tables[0].Rows[0]["TotIssue"]).Trim();
                            int tissue = Convert.ToInt32(totissue) + Convert.ToInt32(issue);
                            totissue = Convert.ToString(tissue);
                        }
                        else
                        {
                            totissue = totissue;
                        }
                        qry = "select b.lib_code,lib_name,return_date,count(*) TotRet From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where return_flag = 1  and return_date ='" + fromdate + "' and g.college_code='" + colgcode + "' and b.is_staff =0";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + "and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,return_date";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreturn = Convert.ToString(ds.Tables[0].Rows[0]["TotRet"]).Trim();
                        }
                        else
                        {
                            totreturn = "0";
                        }
                        qry = " select b.lib_code,lib_name,return_date,count(*) TotRet From borrow b inner join library l on l.lib_code = b.lib_code inner join staffmaster m on m.staff_code = b.roll_no inner join stafftrans t on t.staff_code = m.staff_code inner join department d on d.dept_code = t.dept_code Where 1=1 and m.college_code='" + colgcode + "' AND return_flag = 1 and return_date ='" + fromdate + "' and b.is_staff =1 and t.latestrec = 1";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,return_date";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string ret = Convert.ToString(ds.Tables[0].Rows[0]["TotRet"]).Trim();
                            int tret = Convert.ToInt32(totreturn) + Convert.ToInt32(ret);
                            totreturn = Convert.ToString(tret);
                        }
                        else
                        {
                            totreturn = totreturn;
                        }
                        qry = " select b.lib_code,lib_name,borrow_date,count(*) TotRenew From borrow b inner join library l on l.lib_code = b.lib_code  inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where return_flag =  0 and isnull(renewflag,0) = 1  and g.college_code='" + colgcode + " ' and borrow_date ='" + fromdate + "' and (b.is_staff =0 or b.is_staff = 1) ";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,borrow_date";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string trew = Convert.ToString(ds.Tables[0].Rows[0]["TotRenew"]).Trim();
                            int torew = Convert.ToInt32(totrenew) + Convert.ToInt32(trew);
                            totrenew = Convert.ToString(torew);
                        }
                        else
                        {
                            totrenew = "0";
                        }
                        qry = " Select b.lib_code,lib_name,count(*) TotReserv from priority_studstaff b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code where cur_date ='" + fromdate + " ' and g.college_code='" + colgcode + "'  and b.roll_no <> ''";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,cur_date";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreservation = Convert.ToString(ds.Tables[0].Rows[0]["TotReserv"]).Trim();
                        }
                        else
                        {
                            totreservation = "0";
                        }
                        qry = " Select b.lib_code,lib_name,count(*) TotReserv from priority_studstaff b inner join library l on l.lib_code = b.lib_code inner join staffmaster m on m.staff_code = b.roll_no inner join stafftrans t on t.staff_code = m.staff_code inner join department d on d.dept_code = t.dept_code Where 1=1 and cur_date ='" + fromdate + " ' and m.college_code='" + colgcode + "'  and b.staff_code <> '' and t.latestrec = 1";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        // if (sem != "")
                        //{
                        //    qry = qry + " and r.current_semester in('" + sem + "')";
                        //}
                        qry = qry + " group by b.lib_code,lib_name,cur_date";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string tres = Convert.ToString(ds.Tables[0].Rows[0]["TotReserv"]).Trim();
                            int tores = Convert.ToInt32(totreservation) + Convert.ToInt32(tres);
                            totreservation = Convert.ToString(tores);
                        }
                        else
                        {
                            totreservation = "0";
                        }
                        if (opac == 1)
                        {
                            qry = " select sum(lib_count) Tot_OPAC from lib_queryhit b where lib_date ='" + fromdate + "' and is_staff = 0";
                            if (dept != "")
                            {
                                qry = qry + "  and (b.department in('" + cbl_dept.SelectedItem.ToString() + "') or b.department = 'All')";
                            }
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(qry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                opacHits = Convert.ToString(ds.Tables[0].Rows[0]["Tot_OPAC"]);
                            }
                            else
                            {
                                opacHits = "0";
                            }
                        }
                        if (opac == 1)
                        {
                            qry = " select sum(lib_count) Tot_OPAC from lib_queryhit b where lib_date  ='" + fromdate + "' and  is_staff = 1";
                            if (dept != "")
                            {
                                qry = qry + "  and (b.department in('" + cbl_dept.SelectedItem.ToString() + "') or b.department = 'All')";
                            }
                            qry = qry + "group by lib_date";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(qry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string topac = Convert.ToString(ds.Tables[0].Rows[0]["Tot_OPAC"]);
                                int topach = Convert.ToInt32(opacHits) + Convert.ToInt32(topac);
                                opacHits = Convert.ToString(topach);
                            }
                            else
                            {
                                opacHits = opacHits;
                            }
                        }
                        qry = "SELECT ISNULL(SUM(A.Tot),0) TotGate FROM (SELECT Count(*) Tot FROM LibUsers U WHERE Entry_Date ='" + fromdate + "' AND UserCat ='Student'";
                        if (library != "")
                        {
                            qry = qry + " and lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and current_semester in('" + sem + "')";
                        }
                        qry = qry + " GROUP BY Roll_No,Lib_Code) A";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            gateentry = Convert.ToString(ds.Tables[0].Rows[0]["TotGate"]).Trim();
                        }
                        else
                        {
                            gateentry = "0";
                        }
                        qry = "SELECT ISNULL(SUM(A.Tot),0) TotGate  FROM (SELECT Count(*) Tot FROM LibUsers WHERE Entry_Date ='" + fromdate + "' AND UserCat ='Staff'";
                        if (library != "")
                        {
                            qry = qry + " and lib_code in('" + library + "')";
                        }
                        qry = qry + " GROUP BY Roll_No) A";
                        //if (dept != "")
                        //{
                        //    qry = qry + " and dept_name in('" + dept + "')";
                        //}
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string togate = Convert.ToString(ds.Tables[0].Rows[0]["TotGate"]).Trim();
                            int totgatentry = Convert.ToInt32(gateentry) + Convert.ToInt32(togate);
                            gateentry = Convert.ToString(totgatentry);
                        }
                        else
                        {
                            gateentry = gateentry;
                        }
                        int tottran = Convert.ToInt32(totissue) + Convert.ToInt32(totreturn) + Convert.ToInt32(totrenew) + Convert.ToInt32(totreservation);
                        if (tottran == null)
                        {
                            tottran = 0;
                        }
                        string frm_date1 = Convert.ToString(dtfrom);
                        string[] dtdate1 = frm_date1.Split('/');
                        if (dtdate1.Length == 3)
                            frm_date1 = dtdate1[1].ToString() + "/" + dtdate1[0].ToString() + "/" + dtdate1[2].ToString();
                        drow = dtDailyActivity.NewRow();
                        drow["Date"] = Convert.ToString(frm_date1.Split(' ')[0]);
                        drow["Total Issue"] = Convert.ToString(totissue);
                        drow["Total Return"] = Convert.ToString(totreturn);
                        drow["Total Renew"] = Convert.ToString(totrenew);
                        drow["Total Reservation"] = Convert.ToString(totreservation);
                        drow["OPAC Hits"] = Convert.ToString(opacHits);
                        drow["Gate Entry"] = Convert.ToString(gateentry);
                        drow["Total Trans"] = Convert.ToString(tottran);
                        dtDailyActivity.Rows.Add(drow);



                        tottissue = tottissue + Convert.ToInt32(totissue);
                        tottreturn = tottreturn + Convert.ToInt32(totreturn);
                        tottrenew = tottrenew + Convert.ToInt32(totrenew);
                        tottreser = tottreser + Convert.ToInt32(totreservation);
                        totopachits = totopachits + Convert.ToInt32(opacHits);
                        totgateentry = totgateentry + Convert.ToInt32(gateentry);


                        int grtottissue = 0;
                        int grtottreturn = 0;
                        int grtottrenew = 0;
                        int grtottreser = 0;
                        int grtotopachits = 0;
                        int grtotgateentry = 0;
                        int grtotttran = 0;

                        grtottissue = grtottissue + tottissue;
                        grtottreturn = grtottreturn + tottreturn;
                        grtottrenew = grtottrenew + tottrenew;
                        grtottreser = grtottreser + tottreser;
                        grtotopachits = grtotopachits + totopachits;
                        grtotgateentry = grtotgateentry + totgateentry;

                        grtotttran = grtotttran + totttran;

                    }
                    #endregion
                }
                drow = dtDailyActivity.NewRow();

                drow["Date"] = "Total";
                drow["Total Issue"] = Convert.ToString(tottissue);
                drow["Total Return"] = Convert.ToString(tottreturn);
                drow["Total Renew"] = Convert.ToString(tottrenew);
                drow["Total Reservation"] = Convert.ToString(tottreser);
                drow["OPAC Hits"] = Convert.ToString(totopachits);
                drow["Gate Entry"] = Convert.ToString(totgateentry);
                drow["Total Trans"] = Convert.ToString(totttran);
                dtDailyActivity.Rows.Add(drow);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                divtable.Visible = true;
                grdBkIssTransReport.DataSource = dtDailyActivity;
                grdBkIssTransReport.DataBind();
                grdBkIssTransReport.Visible = true;
                GrdOverDueMemebersList.Visible = false;
                btn_printmaster.Visible = true;
                btn_Excel.Visible = true;
                lbl_reportname.Visible = true;
                txt_excelname.Visible = true;
                div_report.Visible = true;
                btnPopAlertClose.Visible = false;
                divPopupAlert.Visible = false;
                RowHead(grdBkIssTransReport);
            }
            else
            {
                divtable.Visible = false;
                grdBkIssTransReport.Visible = false;
                btn_printmaster.Visible = false;
                btn_Excel.Visible = false;
                lbl_reportname.Visible = false;
                txt_excelname.Visible = false;
                div_report.Visible = false;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Records Found";
                btnPopAlertClose.Visible = true;
                divPopupAlert.Visible = true;
                GrdOverDueMemebersList.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void WeeklyActivity(object sender, EventArgs e)
    {
        try
        {
            string fromdate = string.Empty;
            string todate = string.Empty;
            string dateqry = string.Empty;
            string qry = string.Empty;
            string colgcode = string.Empty;
            string library = string.Empty;
            string dept = string.Empty;
            string sem = string.Empty;
            string totissue = string.Empty;
            string totreturn = string.Empty;
            string totrenew = string.Empty;
            string totreservation = string.Empty;
            string opacHits = string.Empty;
            string gateentry = string.Empty;
            if (cbdate1.Checked == false)
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Select The Date";
                divPopupAlert.Visible = true;
                btnPopAlertClose.Visible = true;
                return;
            }
            int opac = 1;
            library = getCblSelectedValue(cbl_library);
            dept = getCblSelectedValue(cbl_dept);
            string sem1 = getCblSelectedText(cbl_sem);
            string[] Semster = sem1.Split(new string[] { "','" }, StringSplitOptions.None);
            string SemVal = string.Empty;
            for (int i = 0; i < Semster.Length; i++)
            {
                SemVal = Semster[i];
                string SemCode = SemVal.Split(' ')[0];
                if (!sem.Contains(SemCode))
                {
                    if (sem == "")
                        sem = SemCode;
                    else
                        sem = sem + "','" + SemCode;
                }
            }
            colgcode = Convert.ToString(ddl_collegename.SelectedValue);
            string libraryname = Convert.ToString(cbl_library.SelectedValue).Trim();

            DataTable dtWeeklyActivity = new DataTable();
            DataRow drow;

            dtWeeklyActivity.Columns.Add("Week", typeof(string));
            dtWeeklyActivity.Columns.Add("Total Issue", typeof(string));
            dtWeeklyActivity.Columns.Add("Total Return", typeof(string));
            dtWeeklyActivity.Columns.Add("Total Renew", typeof(string));
            dtWeeklyActivity.Columns.Add("Total Reservation", typeof(string));
            dtWeeklyActivity.Columns.Add("OPAC Hits", typeof(string));
            dtWeeklyActivity.Columns.Add("Gate Entry", typeof(string));
            dtWeeklyActivity.Columns.Add("Total Trans", typeof(string));


            drow = dtWeeklyActivity.NewRow();
            drow["Week"] = "Week";
            drow["Total Issue"] = "Total Issue";
            drow["Total Return"] = "Total Return";
            drow["Total Renew"] = "Total Renew";
            drow["Total Reservation"] = "Total Reservation";
            drow["OPAC Hits"] = "OPAC Hits";
            drow["Gate Entry"] = "Gate Entry";
            drow["Total Trans"] = "Total Trans";
            dtWeeklyActivity.Rows.Add(drow);
            int tottissue = 0;
            int tottreturn = 0;
            int tottrenew = 0;
            int tottreser = 0;
            int totopachits = 0;
            int totgateentry = 0;
            int totttran = 0;

            int grtottissue = 0;
            int grtottreturn = 0;
            int grtottrenew = 0;
            int grtottreser = 0;
            int grtotopachits = 0;
            int grtotgateentry = 0;
            int grtotttran = 0;

            if (cbdate1.Checked == true)
            {
                fromdate = txt_fromdate1.Text;
                todate = txt_todate.Text;
                string[] frdate = fromdate.Split('/');
                if (frdate.Length == 3)
                    fromdate = frdate[2].ToString() + "/" + frdate[1].ToString() + "/" + frdate[0].ToString();
                string[] tdate = todate.Split('/');
                if (tdate.Length == 3)
                    todate = tdate[2].ToString() + "/" + tdate[1].ToString() + "/" + tdate[0].ToString();
                dateqry = " and entry_date between '" + fromdate + "' and '" + todate + "'";
            }
            else
            {
                dateqry = "";
            }
            qry = "SELECT datename(wk,borrow_Date) montno,year(borrow_date) selyear FROM Borrow where 1=1 and borrow_Date between '" + fromdate + "' and '" + todate + "' GROUP BY datename(wk,borrow_Date ),year(borrow_date)  order by cast(datename(wk,borrow_Date) as numeric) ";
            ds1.Clear();
            ds1 = d2.select_method_wo_parameter(qry, "text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    string montno = Convert.ToString(ds1.Tables[0].Rows[i]["montno"]);
                    string selyr = Convert.ToString(ds1.Tables[0].Rows[i]["selyear"]);

                    #region Student

                    if (checkusers.Items[0].Selected == true && checkusers.Items[1].Selected == false)
                    {
                        qry = "select isnull(count(*),0) as TotIssue From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where 1=1 and g.college_code='" + colgcode + "' AND year(borrow_date) ='" + selyr + "' AND datename(wk,borrow_date) ='" + montno + "' and b.is_staff = 0 ";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(wk,borrow_date) order by lib_name,datename(wk,borrow_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totissue = Convert.ToString(ds.Tables[0].Rows[0]["TotIssue"]).Trim();
                        }
                        else
                        {
                            totissue = "0";
                        }
                        qry = "select isnull(count(*),0) as TotRet From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where return_flag = 1 and g.college_code='" + colgcode + "' AND datename(wk,return_date) ='" + montno + "' AND year(return_date) ='" + selyr + "' and b.is_staff =0";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(wk,return_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreturn = Convert.ToString(ds.Tables[0].Rows[0]["TotRet"]).Trim();
                        }
                        else
                        {
                            totreturn = "0";
                        }
                        qry = " select isnull(count(*),0) as TotIssue From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where 1=1 and g.college_code='" + colgcode + "' AND datename(wk,borrow_date) ='" + montno + "' AND year(borrow_date) ='" + selyr + "' and b.is_staff = 0 and isnull(renewflag,0) = 1";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(wk,borrow_date) order by lib_name,datename(wk,borrow_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totrenew = Convert.ToString(ds.Tables[0].Rows[0]["TotIssue"]).Trim();
                        }
                        else
                        {
                            totrenew = "0";
                        }
                        qry = "Select count(*) TotReserv from priority_studstaff b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code AND datename(wk,cur_date) ='" + montno + "' AND year(cur_date) ='" + selyr + "'and g.college_code='" + colgcode + "'and b.is_staff = 0 ";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(wk,cur_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreservation = Convert.ToString(ds.Tables[0].Rows[0]["TotReserv"]).Trim();
                        }
                        else
                        {
                            totreservation = "0";
                        }
                        if (opac == 1)
                        {
                            qry = " select sum(lib_count) Tot_OPAC from lib_queryhit b where datename(wk,lib_date) ='" + montno + "' and is_staff = 0";
                            //if (dept != "")
                            //{
                            //    qry = qry + "  and ( b.department = 'All')";//b.department in('" + cbl_dept.SelectedItem.ToString() + "') or
                            //}
                            qry = qry + " group by datename(wk,lib_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(qry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                opacHits = Convert.ToString(ds.Tables[0].Rows[0]["Tot_OPAC"]);
                            }
                        }
                        qry = "SELECT ISNULL(SUM(A.Tot),0) TotGate FROM (SELECT Count(*) Tot FROM LibUsers U where datename(wk,Entry_Date) ='" + montno + "' AND year(Entry_Date) ='" + selyr + "' AND UserCat ='Student'";
                        if (library != "")
                        {
                            qry = qry + " and lib_code in('" + library + "')";
                        }
                        //if (dept != "")
                        //{
                        //    qry = qry + " and dept_name in('" + Convert.ToString(getCblSelectedText(cbl_dept)) + "')";
                        //}
                        if (sem != "")
                        {
                            qry = qry + " and current_semester in('" + sem + "')";
                        }
                        qry = qry + " GROUP BY Roll_No,Lib_Code, datename(wk,Entry_Date)) A";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            gateentry = Convert.ToString(ds.Tables[0].Rows[0]["TotGate"]).Trim();
                        }
                        string date1 = DateTime.Now.ToString("dd-MM-yyyy");
                        int tottran = Convert.ToInt32(totissue) + Convert.ToInt32(totreturn) + Convert.ToInt32(totrenew) + Convert.ToInt32(totreservation);

                        drow = dtWeeklyActivity.NewRow();
                        drow["Week"] = Convert.ToString(montno);
                        drow["Total Issue"] = Convert.ToString(totissue);
                        drow["Total Return"] = Convert.ToString(totreturn);
                        drow["Total Renew"] = Convert.ToString(totrenew);
                        drow["Total Reservation"] = Convert.ToString(totreservation);
                        drow["OPAC Hits"] = Convert.ToString(opacHits);
                        drow["Gate Entry"] = Convert.ToString(gateentry);
                        drow["Total Trans"] = Convert.ToString(tottran);
                        dtWeeklyActivity.Rows.Add(drow);

                        tottissue = tottissue + Convert.ToInt32(totissue);
                        tottreturn = tottreturn + Convert.ToInt32(totreturn);
                        tottrenew = tottrenew + Convert.ToInt32(totrenew);
                        tottreser = tottreser + Convert.ToInt32(totreservation);
                        totopachits = totopachits + Convert.ToInt32(opacHits);
                        totgateentry = totgateentry + Convert.ToInt32(gateentry);
                        totttran = totttran + Convert.ToInt32(tottran);


                        grtottissue = grtottissue + tottissue;
                        grtottreturn = grtottreturn + tottreturn;
                        grtottrenew = grtottrenew + tottrenew;
                        grtottreser = grtottreser + tottreser;
                        grtotopachits = grtotopachits + totopachits;
                        grtotgateentry = grtotgateentry + totgateentry;
                        grtotttran = grtotttran + totttran;
                    }

                    #endregion

                    #region Staff

                    if (checkusers.Items[1].Selected == true && checkusers.Items[0].Selected == false)
                    {
                        qry = "select isnull(count(*),0) as TotIssue From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where 1=1 and g.college_code='" + colgcode + "' AND datename(wk,borrow_date) ='" + montno + "' AND year(borrow_date) ='" + selyr + "'and b.is_staff = 1";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(wk,borrow_date) order by lib_name,datename(wk,borrow_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totissue = Convert.ToString(ds.Tables[0].Rows[0]["TotIssue"]).Trim();
                        }
                        else
                        {
                            totissue = "0";
                        }
                        qry = "select  isnull(count(*),0) as TotRet From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where return_flag = 1 and g.college_code='" + colgcode + "' AND datename(wk,return_date) ='" + montno + "' AND year(return_date) ='" + selyr + "' and b.is_staff =1";

                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }

                        qry = qry + " group by b.lib_code,lib_name,datename(wk,return_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreturn = Convert.ToString(ds.Tables[0].Rows[0]["TotRet"]).Trim();
                        }
                        else
                        {
                            totreturn = "0";
                        }
                        qry = " select  isnull(count(*),0) as TotIssue From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where 1=1 and g.college_code='" + colgcode + "' AND datename(wk,borrow_date) ='" + montno + "' AND year(borrow_date) ='" + selyr + "' and b.is_staff = 1 and isnull(renewflag,0) = 1";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(wk,borrow_date) order by lib_name,datename(wk,borrow_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totrenew = Convert.ToString(ds.Tables[0].Rows[0]["TotIssue"]).Trim();
                        }
                        else
                        {
                            totrenew = "0";
                        }
                        qry = "Select isnull(count(*),0) as TotReserv from priority_studstaff b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code AND datename(wk,cur_date) = '" + montno + "' AND year(cur_date) ='" + selyr + "'and g.college_code='" + colgcode + "'and b.is_staff = 1 ";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(wk,cur_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreservation = Convert.ToString(ds.Tables[0].Rows[0]["TotReserv"]).Trim();
                        }
                        else
                        {
                            totreservation = "0";
                        }
                        if (opac == 1)
                        {
                            qry = " select sum(lib_count) Tot_OPAC from lib_queryhit b where datename(wk,lib_date) ='" + montno + "'  and is_staff = 1";
                            //if (dept != "")
                            //{
                            //    qry = qry + "  and (b.department in('" + cbl_dept.SelectedItem.ToString() + "') or b.department = 'All')";
                            //}
                            qry = qry + "group by datename(wk,lib_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(qry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                opacHits = Convert.ToString(ds.Tables[0].Rows[0]["Tot_OPAC"]);
                            }
                            else
                            {
                                opacHits = "0";
                            }
                        }
                        qry = "SELECT ISNULL(SUM(A.Tot),0) TotGate FROM(SELECT Count(*) Tot FROM LibUsers U where datename(wk,Entry_Date) ='" + montno + "' AND year(Entry_Date) ='" + selyr + "' AND UserCat ='Staff'";
                        if (library != "")
                        {
                            qry = qry + " and lib_code in('" + library + "')";
                        }
                        //if (dept != "")
                        //{
                        //    qry = qry + " and dept_name in('" + Convert.ToString(getCblSelectedText(cbl_dept)) + "')";
                        //}
                        qry = qry + " GROUP BY Roll_No,Lib_Code, datename(wk,Entry_Date)) A";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            gateentry = Convert.ToString(ds.Tables[0].Rows[0]["TotGate"]).Trim();
                        }
                        else
                        {
                            gateentry = "0";
                        }
                        int tottran = Convert.ToInt32(totissue) + Convert.ToInt32(totreturn) + Convert.ToInt32(totrenew) + Convert.ToInt32(totreservation);
                        drow = dtWeeklyActivity.NewRow();
                        drow["Week"] = Convert.ToString(montno);
                        drow["Total Issue"] = Convert.ToString(totissue);
                        drow["Total Return"] = Convert.ToString(totreturn);
                        drow["Total Renew"] = Convert.ToString(totrenew);
                        drow["Total Reservation"] = Convert.ToString(totreservation);
                        drow["OPAC Hits"] = Convert.ToString(opacHits);
                        drow["Gate Entry"] = Convert.ToString(gateentry);
                        drow["Total Trans"] = Convert.ToString(tottran);
                        dtWeeklyActivity.Rows.Add(drow);

                        tottissue = tottissue + Convert.ToInt32(totissue);
                        tottreturn = tottreturn + Convert.ToInt32(totreturn);
                        tottrenew = tottrenew + Convert.ToInt32(totrenew);
                        tottreser = tottreser + Convert.ToInt32(totreservation);
                        totopachits = totopachits + Convert.ToInt32(opacHits);
                        totgateentry = totgateentry + Convert.ToInt32(gateentry);
                        totttran = totttran + Convert.ToInt32(tottran);

                        grtottissue = grtottissue + tottissue;
                        grtottreturn = grtottreturn + tottreturn;
                        grtottrenew = grtottrenew + tottrenew;
                        grtottreser = grtottreser + tottreser;
                        grtotopachits = grtotopachits + totopachits;
                        grtotgateentry = grtotgateentry + totgateentry;
                        grtotttran = grtotttran + totttran;
                    }

                    #endregion

                    #region All

                    if (checkusers.Items[0].Selected == true && checkusers.Items[1].Selected == true)
                    {
                        qry = "select count(*) TotIssue From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where 1=1 and g.college_code='" + colgcode + "' AND datename(wk,borrow_date) ='" + montno + "' AND year(borrow_date) ='" + selyr + "' and (b.is_staff = 0 or b.is_staff = 1) ";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + "and r.current_semester in('" + sem + "')";
                        }

                        qry = qry + " group by b.lib_code,lib_name,datename(wk,borrow_date) order by lib_name,datename(wk,borrow_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totissue = Convert.ToString(ds.Tables[0].Rows[0]["TotIssue"]).Trim();
                        }
                        else
                        {
                            totissue = "0";
                        }
                        qry = "select count(*) TotRet From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where return_flag = 1 and g.college_code='" + colgcode + "' AND datename(wk,return_date) ='" + montno + "' AND year(return_date) ='" + selyr + "' and (b.is_staff =0 or b.is_staff = 1) ";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + "and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(wk,return_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreturn = Convert.ToString(ds.Tables[0].Rows[0]["TotRet"]).Trim();
                        }
                        else
                        {
                            totreturn = "0";
                        }
                        qry = " select count(*) TotIssue From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where 1=1 and g.college_code='" + colgcode + "' AND datename(wk,borrow_date) ='" + montno + "' AND year(borrow_date) ='" + selyr + "' and (b.is_staff = 0 or b.is_staff = 1) and isnull(renewflag,0) = 1";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(wk,borrow_date) order by lib_name,datename(wk,borrow_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totrenew = Convert.ToString(ds.Tables[0].Rows[0]["TotIssue"]).Trim();
                        }
                        else
                        {
                            totrenew = "0";
                        }
                        qry = " Select count(*) TotReserv from priority_studstaff b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code AND datename(wk,cur_date) ='" + montno + "' AND year(cur_date) ='" + selyr + "'  and g.college_code='" + colgcode + "' and (b.is_staff = 0 or b.is_staff = 1)";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(wk,cur_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreservation = Convert.ToString(ds.Tables[0].Rows[0]["TotReserv"]).Trim();
                        }
                        else
                        {
                            totreservation = "0";
                        }
                        if (opac == 1)
                        {
                            qry = " select sum(lib_count) Tot_OPAC from lib_queryhit b where datename(wk,lib_date) ='" + montno + "'  and (is_staff = 0 or is_staff = 1)";
                            //if (dept != "")
                            //{
                            //    qry = qry + "  and (b.department in('" + cbl_dept.SelectedItem.ToString() + "') or b.department = 'All')";
                            //}
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(qry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                opacHits = Convert.ToString(ds.Tables[0].Rows[0]["Tot_OPAC"]);
                            }
                        }
                        if (opac == 1)
                        {
                            qry = " select sum(lib_count) Tot_OPAC from lib_queryhit b where lib_date  ='" + fromdate + "' and  is_staff = 1";
                            //if (dept != "")
                            //{
                            //    qry = qry + "  and (b.department in('" + cbl_dept.SelectedItem.ToString() + "') or b.department = 'All')";
                            //}
                            qry = qry + " group by datename(wk,lib_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(qry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string hits = Convert.ToString(ds.Tables[0].Rows[0]["Tot_OPAC"]).Trim();
                                int openh = Convert.ToInt32(opacHits) + Convert.ToInt32(hits);
                                opacHits = Convert.ToString(openh);
                                //opacHits = opacHits + Convert.ToString(ds.Tables[0].Rows[0]["Tot_OPAC"]);
                            }
                            else
                            {
                                opacHits = opacHits;
                            }
                        }
                        qry = "SELECT ISNULL(SUM(A.Tot),0) TotGate FROM (SELECT Count(*) Tot FROM LibUsers U where datename(wk,Entry_Date) ='" + montno + "' AND year(Entry_Date) ='" + selyr + "'  AND (UserCat ='Student' or UserCat = 'Staff') ";
                        if (library != "")
                        {
                            qry = qry + " and lib_code in('" + library + "')";
                        }
                        //if (dept != "")
                        //{
                        //    qry = qry + " and dept_name in('" + Convert.ToString(getCblSelectedText(cbl_dept)) + "')";
                        //}
                        if (sem != "")
                        {
                            qry = qry + " and current_semester in('" + sem + "')";
                        }
                        qry = qry + " GROUP BY Roll_No,Lib_Code, datename(wk,Entry_Date)) A";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            gateentry = Convert.ToString(ds.Tables[0].Rows[0]["TotGate"]).Trim();
                        }
                        int tottran = Convert.ToInt32(totissue) + Convert.ToInt32(totreturn) + Convert.ToInt32(totrenew) + Convert.ToInt32(totreservation);
                        drow = dtWeeklyActivity.NewRow();
                        drow["Week"] = Convert.ToString(montno);
                        drow["Total Issue"] = Convert.ToString(totissue);
                        drow["Total Return"] = Convert.ToString(totreturn);
                        drow["Total Renew"] = Convert.ToString(totrenew);
                        drow["Total Reservation"] = Convert.ToString(totreservation);
                        drow["OPAC Hits"] = Convert.ToString(opacHits);
                        drow["Gate Entry"] = Convert.ToString(gateentry);
                        drow["Total Trans"] = Convert.ToString(tottran);
                        dtWeeklyActivity.Rows.Add(drow);

                        tottissue = tottissue + Convert.ToInt32(totissue);
                        tottreturn = tottreturn + Convert.ToInt32(totreturn);
                        tottrenew = tottrenew + Convert.ToInt32(totrenew);
                        tottreser = tottreser + Convert.ToInt32(totreservation);
                        totopachits = totopachits + Convert.ToInt32(opacHits);
                        totgateentry = totgateentry + Convert.ToInt32(gateentry);
                        totttran = totttran + Convert.ToInt32(tottran);

                        grtottissue = grtottissue + tottissue;
                        grtottreturn = grtottreturn + tottreturn;
                        grtottrenew = grtottrenew + tottrenew;
                        grtottreser = grtottreser + tottreser;
                        grtotopachits = grtotopachits + totopachits;
                        grtotgateentry = grtotgateentry + totgateentry;
                        grtotttran = grtotttran + totttran; ;
                    }

                    #endregion

                }
                drow = dtWeeklyActivity.NewRow();
                drow["Week"] = "Total";
                drow["Total Issue"] = Convert.ToString(tottissue);
                drow["Total Return"] = Convert.ToString(tottreturn);
                drow["Total Renew"] = Convert.ToString(tottrenew);
                drow["Total Reservation"] = Convert.ToString(tottreser);
                drow["OPAC Hits"] = Convert.ToString(totopachits);
                drow["Gate Entry"] = Convert.ToString(totgateentry);
                drow["Total Trans"] = Convert.ToString(totttran);
                dtWeeklyActivity.Rows.Add(drow);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                divtable.Visible = true;
                grdBkIssTransReport.DataSource = dtWeeklyActivity;
                grdBkIssTransReport.DataBind();
                grdBkIssTransReport.Visible = true;
                GrdOverDueMemebersList.Visible = false;
                btn_printmaster.Visible = true;
                btn_Excel.Visible = true;
                lbl_reportname.Visible = true;
                txt_excelname.Visible = true;
                div_report.Visible = true;
                btnPopAlertClose.Visible = false;
                divPopupAlert.Visible = false;
                RowHead(grdBkIssTransReport);
            }
            else
            {
                divtable.Visible = false;
                grdBkIssTransReport.Visible = false;
                btn_printmaster.Visible = false;
                btn_Excel.Visible = false;
                lbl_reportname.Visible = false;
                txt_excelname.Visible = false;
                div_report.Visible = false;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Records Found";
                btnPopAlertClose.Visible = true;
                divPopupAlert.Visible = true;
                GrdOverDueMemebersList.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void MonthlyActivity(object sender, EventArgs e)
    {

        try
        {
            string fromdate = string.Empty;
            string todate = string.Empty;
            string dateqry = string.Empty;
            string qry = string.Empty;
            string colgcode = string.Empty;
            string library = string.Empty;
            string dept = string.Empty;
            string sem = string.Empty;
            string totissue = string.Empty;
            string totreturn = string.Empty;
            string totrenew = string.Empty;
            string totreservation = string.Empty;
            string opacHits = string.Empty;
            string gateentry = string.Empty;
            if (cbdate1.Checked == false)
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Select The Date";
                divPopupAlert.Visible = true;
                btnPopAlertClose.Visible = true;
                return;
            }
            int UserCount = 0;
            int opac = 1;

            library = getCblSelectedValue(cbl_library);
            dept = getCblSelectedValue(cbl_dept);
            string sem1 = getCblSelectedText(cbl_sem);
            string[] Semster = sem1.Split(new string[] { "','" }, StringSplitOptions.None);
            string SemVal = string.Empty;
            for (int i = 0; i < Semster.Length; i++)
            {
                SemVal = Semster[i];
                string SemCode = SemVal.Split(' ')[0];
                if (!sem.Contains(SemCode))
                {
                    if (sem == "")
                        sem = SemCode;
                    else
                        sem = sem + "','" + SemCode;
                }
            }
            colgcode = Convert.ToString(ddl_collegename.SelectedValue);
            string libraryname = Convert.ToString(cbl_library.SelectedValue).Trim();
            string fromdate1 = "";
            string todate1 = "";
            if (cbdate1.Checked == true)
            {
                fromdate = txt_fromdate1.Text;
                string[] frdate = fromdate.Split('/');
                if (frdate.Length == 3)
                    fromdate = frdate[2].ToString() + "/" + frdate[1].ToString() + "/" + frdate[0].ToString();
                DateTime dt = new DateTime();
                dt = Convert.ToDateTime(fromdate);
                fromdate1 = dt.ToString("yyyy");

                todate = txt_todate.Text;
                string[] tdate = todate.Split('/');
                if (tdate.Length == 3)
                    todate = tdate[2].ToString() + "/" + tdate[1].ToString() + "/" + tdate[0].ToString();
                DateTime dt1 = new DateTime();
                dt1 = Convert.ToDateTime(todate);
                todate1 = dt1.ToString("yyyy");
                dateqry = " and entry_date between '" + fromdate + "' and '" + todate + "'";
            }
            else
            {
                dateqry = "";
            }
            DataTable dtMonthlyActivity = new DataTable();
            DataRow drow;

            dtMonthlyActivity.Columns.Add("Month", typeof(string));
            dtMonthlyActivity.Columns.Add("Total Issue", typeof(string));
            dtMonthlyActivity.Columns.Add("Total Return", typeof(string));
            dtMonthlyActivity.Columns.Add("Total Renew", typeof(string));
            dtMonthlyActivity.Columns.Add("Total Reservation", typeof(string));
            dtMonthlyActivity.Columns.Add("OPAC Hits", typeof(string));
            dtMonthlyActivity.Columns.Add("Gate Entry", typeof(string));
            dtMonthlyActivity.Columns.Add("Total Trans", typeof(string));

            drow = dtMonthlyActivity.NewRow();
            drow["Month"] = "Month";
            drow["Total Issue"] = "Total Issue";
            drow["Total Return"] = "Total Return";
            drow["Total Renew"] = "Total Renew";
            drow["Total Reservation"] = "Total Reservation";
            drow["OPAC Hits"] = "OPAC Hits";
            drow["Gate Entry"] = "Gate Entry";
            drow["Total Trans"] = "Total Trans";
            dtMonthlyActivity.Rows.Add(drow);
            int tottissue = 0;
            int tottreturn = 0;
            int tottrenew = 0;
            int tottreser = 0;
            int totopachits = 0;
            int totgateentry = 0;
            int totttran = 0;

            int grtottissue = 0;
            int grtottreturn = 0;
            int grtottrenew = 0;
            int grtottreser = 0;
            int grtotopachits = 0;
            int grtotgateentry = 0;
            int grtotttran = 0;

            qry = "SELECT datename(mm,borrow_Date ) montname, month(borrow_Date) montno FROM Borrow where 1=1  and borrow_Date between '" + fromdate + "' and '" + todate + "'  GROUP BY datename(mm,borrow_Date ),month(borrow_Date) order by month(borrow_Date) ";
            ds1.Clear();
            ds1 = d2.select_method_wo_parameter(qry, "text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                int sno = 0;
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    string montno = Convert.ToString(ds1.Tables[0].Rows[i]["montname"]);
                    string selyr = Convert.ToString(ds1.Tables[0].Rows[i]["montno"]);

                    #region Student

                    if (checkusers.Items[0].Selected == true && checkusers.Items[1].Selected == false)
                    {
                        qry = "select count(*) TotIssue  From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where 1=1 and g.college_code='" + colgcode + "' AND month(borrow_date) ='" + selyr + "' AND year(borrow_date) in ('" + fromdate1 + "','" + todate1 + "') and b.is_staff = 0";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(mm,borrow_date) order by lib_name,datename(mm,borrow_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totissue = Convert.ToString(ds.Tables[0].Rows[0]["TotIssue"]).Trim();
                        }
                        else
                        {
                            totissue = "0";
                        }
                        qry = "select count(*) TotRet  From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where return_flag = 1 and g.college_code='" + colgcode + "' AND month(return_date) ='" + selyr + "'  AND year(return_date) in ('" + fromdate1 + "','" + todate1 + "')  and b.is_staff =0";

                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(mm,return_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreturn = Convert.ToString(ds.Tables[0].Rows[0]["TotRet"]).Trim();
                        }
                        else
                        {
                            totreturn = "0";
                        }
                        qry = " select count(*) TotIssue From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where 1=1 and g.college_code='" + colgcode + "' AND month(borrow_date) ='" + selyr + "' AND year(borrow_date) in ('" + fromdate1 + "','" + todate1 + "') and b.is_staff = 0 and isnull(renewflag,0) = 1";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(mm,borrow_date) order by lib_name,datename(mm,borrow_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totrenew = Convert.ToString(ds.Tables[0].Rows[0]["TotIssue"]).Trim();
                        }
                        else
                        {
                            totrenew = "0";
                        }
                        qry = "Select count(*) TotReserv from priority_studstaff b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code AND month(cur_date) ='" + selyr + "'  AND year(cur_date) in ('" + fromdate1 + "','" + todate1 + "')  and g.college_code='" + colgcode + "' and b.is_staff = 0 ";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(mm,cur_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreservation = Convert.ToString(ds.Tables[0].Rows[0]["TotReserv"]).Trim();
                        }
                        else
                        {
                            totreservation = "0";
                        }
                        if (opac == 1)
                        {
                            qry = " select sum(lib_count) Tot_OPAC from lib_queryhit b where month(lib_date) ='" + selyr + "'   and is_staff = 0";
                            //if (dept != "")
                            //{
                            //    qry = qry + "  and (b.department in('" + cbl_dept.SelectedItem.ToString() + "') or b.department = 'All')";
                            //}
                            qry = qry + " group by datename(mm,lib_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(qry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                opacHits = Convert.ToString(ds.Tables[0].Rows[0]["Tot_OPAC"]);
                            }
                            else
                            {
                                opacHits = "0";
                            }
                        }
                        qry = "SELECT ISNULL(SUM(A.Tot),0) TotGate FROM (SELECT Count(*) Tot FROM LibUsers U where month(Entry_Date) ='" + selyr + "'  AND year(Entry_Date) in ('" + fromdate1 + "','" + todate1 + "')  AND UserCat ='Student'";
                        if (library != "")
                        {
                            qry = qry + " and lib_code in('" + library + "')";
                        }
                        //if (dept != "")
                        //{
                        //    qry = qry + " and dept_name in('" + Convert.ToString(getCblSelectedText(cbl_dept)) + "')";
                        //}
                        if (sem != "")
                        {
                            qry = qry + " and current_semester in('" + sem + "')";
                        }
                        qry = qry + " GROUP BY Roll_No,Lib_Code, datename(mm,Entry_Date)) A";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            gateentry = Convert.ToString(ds.Tables[0].Rows[0]["TotGate"]).Trim();
                        }
                        else
                        {
                            gateentry = "0";
                        }
                        int tottran = Convert.ToInt32(totissue) + Convert.ToInt32(totreturn) + Convert.ToInt32(totrenew) + Convert.ToInt32(totreservation);
                        drow = dtMonthlyActivity.NewRow();
                        drow["Month"] = Convert.ToString(montno);
                        drow["Total Issue"] = Convert.ToString(totissue);
                        drow["Total Return"] = Convert.ToString(totreturn);
                        drow["Total Renew"] = Convert.ToString(totrenew);
                        drow["Total Reservation"] = Convert.ToString(totreservation);
                        drow["OPAC Hits"] = Convert.ToString(opacHits);
                        drow["Gate Entry"] = Convert.ToString(gateentry);
                        drow["Total Trans"] = Convert.ToString(tottran);
                        dtMonthlyActivity.Rows.Add(drow);

                        tottissue = tottissue + Convert.ToInt32(totissue);
                        tottreturn = tottreturn + Convert.ToInt32(totreturn);
                        tottrenew = tottrenew + Convert.ToInt32(totrenew);
                        tottreser = tottreser + Convert.ToInt32(totreservation);
                        totopachits = totopachits + Convert.ToInt32(opacHits);
                        totgateentry = totgateentry + Convert.ToInt32(gateentry);
                        totttran = totttran + Convert.ToInt32(tottran);

                        grtottissue = grtottissue + tottissue;
                        grtottreturn = grtottreturn + tottreturn;
                        grtottrenew = grtottrenew + tottrenew;
                        grtottreser = grtottreser + tottreser;
                        grtotopachits = grtotopachits + totopachits;
                        grtotgateentry = grtotgateentry + totgateentry;
                        grtotttran = grtotttran + totttran;
                    }

                    #endregion

                    #region Staff

                    if (checkusers.Items[1].Selected == true && checkusers.Items[0].Selected == false)
                    {
                        qry = "select count(*) TotIssue  from borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where 1=1 and g.college_code='" + colgcode + "'  AND month(borrow_date) ='" + selyr + "' AND year(borrow_date) in ('" + fromdate1 + "','" + todate1 + "')  and b.is_staff = 1";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }

                        qry = qry + " group by b.lib_code,lib_name,datename(mm,borrow_date) order by lib_name,datename(mm,borrow_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totissue = Convert.ToString(ds.Tables[0].Rows[0]["TotIssue"]).Trim();
                        }
                        else
                        {
                            totissue = "0";
                        }
                        qry = "select count(*) TotRet  From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where return_flag = 1 and g.college_code='" + colgcode + "' AND month(return_date) ='" + selyr + "' AND year(return_date) in ('" + fromdate1 + "','" + todate1 + "')  and b.is_staff =1";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(mm,return_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreturn = Convert.ToString(ds.Tables[0].Rows[0]["TotRet"]).Trim();
                        }
                        else
                        {
                            totreturn = "0";
                        }
                        qry = "select count(*) TotIssue From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where 1=1 and g.college_code='" + colgcode + "'  AND month(borrow_date) ='" + selyr + "'  AND year(borrow_date) in ('" + fromdate1 + "','" + todate1 + "')  and b.is_staff = 1 and isnull(renewflag,0) = 1";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(mm,borrow_date) order by lib_name,datename(mm,borrow_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totrenew = Convert.ToString(ds.Tables[0].Rows[0]["TotIssue"]).Trim();
                        }
                        else
                        {
                            totrenew = "0";
                        }
                        qry = "Select count(*) TotReserv from priority_studstaff b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code AND month(cur_date) ='" + selyr + "' AND year(cur_date) in ('" + fromdate1 + "','" + todate1 + "') and g.college_code='" + colgcode + "' and b.is_staff = 1  ";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(mm,cur_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreservation = Convert.ToString(ds.Tables[0].Rows[0]["TotReserv"]).Trim();
                        }
                        else
                        {
                            totreservation = "0";
                        }
                        if (opac == 1)
                        {
                            qry = " select sum(lib_count) Tot_OPAC from lib_queryhit b where month(lib_date) ='" + selyr + "' AND year(lib_date) in ('" + fromdate1 + "','" + todate1 + "')  and lib_date ='" + fromdate + "' and is_staff = 1";
                            //if (dept != "")
                            //{
                            //    qry = qry + "  and (b.department in('" + cbl_dept.SelectedItem.ToString() + "') or b.department = 'All')";
                            //}
                            qry = qry + " group by datename(mm,lib_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(qry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                opacHits = Convert.ToString(ds.Tables[0].Rows[0]["Tot_OPAC"]);
                            }
                        }
                        qry = "SELECT ISNULL(SUM(A.Tot),0) TotGate FROM (SELECT Count(*) Tot FROM LibUsers U where month(Entry_Date) ='" + selyr + "' AND year(Entry_Date) in ('" + fromdate1 + "','" + todate1 + "')   AND UserCat ='Staff'";
                        if (library != "")
                        {
                            qry = qry + " and lib_code in('" + library + "')";
                        }
                        qry = qry + " GROUP BY Roll_No) A";
                        //if (dept != "")
                        //{
                        //    qry = qry + " and dept_name in('" + Convert.ToString(getCblSelectedText(cbl_dept)) + "')";
                        //}

                        qry = qry + " GROUP BY Roll_No,Lib_Code, datename(mm,Entry_Date)) A";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            gateentry = Convert.ToString(ds.Tables[0].Rows[0]["TotGate"]).Trim();
                        }

                        int tottran = Convert.ToInt32(totissue) + Convert.ToInt32(totreturn) + Convert.ToInt32(totrenew) + Convert.ToInt32(totreservation);

                        drow = dtMonthlyActivity.NewRow();
                        drow["Month"] = Convert.ToString(montno);
                        drow["Total Issue"] = Convert.ToString(totissue);
                        drow["Total Return"] = Convert.ToString(totreturn);
                        drow["Total Renew"] = Convert.ToString(totrenew);
                        drow["Total Reservation"] = Convert.ToString(totreservation);
                        drow["OPAC Hits"] = Convert.ToString(opacHits);
                        drow["Gate Entry"] = Convert.ToString(gateentry);
                        drow["Total Trans"] = Convert.ToString(tottran);
                        dtMonthlyActivity.Rows.Add(drow);

                        tottissue = tottissue + Convert.ToInt32(totissue);
                        tottreturn = tottreturn + Convert.ToInt32(totreturn);
                        tottrenew = tottrenew + Convert.ToInt32(totrenew);
                        tottreser = tottreser + Convert.ToInt32(totreservation);
                        totopachits = totopachits + Convert.ToInt32(opacHits);
                        totgateentry = totgateentry + Convert.ToInt32(gateentry);
                        totttran = totttran + Convert.ToInt32(tottran);

                        grtottissue = grtottissue + tottissue;
                        grtottreturn = grtottreturn + tottreturn;
                        grtottrenew = grtottrenew + tottrenew;
                        grtottreser = grtottreser + tottreser;
                        grtotopachits = grtotopachits + totopachits;
                        grtotgateentry = grtotgateentry + totgateentry;
                        grtotttran = grtotttran + totttran;
                    }

                    #endregion

                    #region All

                    if (checkusers.Items[0].Selected == true && checkusers.Items[1].Selected == true)
                    {
                        qry = " select count(*) TotIssue From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where 1=1 and g.college_code='" + colgcode + "' AND month(borrow_date) ='" + selyr + "' AND year(borrow_date) in ('" + fromdate1 + "','" + todate1 + "') and (b.is_staff = 0 or b.is_staff = 1) ";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + "and r.current_semester in('" + sem + "')";
                        }

                        qry = qry + " group by b.lib_code,lib_name,datename(mm,borrow_date) order by lib_name,datename(mm,borrow_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totissue = Convert.ToString(ds.Tables[0].Rows[0]["TotIssue"]).Trim();
                        }
                        else
                        {
                            totissue = "0";
                        }
                        qry = "select count(*) TotRet  From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where return_flag = 1 and g.college_code='" + colgcode + "'  AND month(return_date) ='" + selyr + "' AND year(return_date) in ('" + fromdate1 + "','" + todate1 + "') and (b.is_staff =0 or b.is_staff = 1)";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + "and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(mm,return_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreturn = Convert.ToString(ds.Tables[0].Rows[0]["TotRet"]).Trim();
                        }
                        else
                        {
                            totreturn = "0";
                        }
                        qry = "select count(*) TotIssue From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where 1=1 and g.college_code='" + colgcode + "'  AND month(borrow_date) ='" + selyr + "' AND year(borrow_date) in ('" + fromdate1 + "','" + todate1 + "')  AND return_flag =  0 and isnull(renewflag,0) = 1 and (b.is_staff = 0 or b.is_staff = 1)";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(mm,borrow_date) order by lib_name,datename(mm,borrow_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totrenew = Convert.ToString(ds.Tables[0].Rows[0]["TotIssue"]).Trim();
                        }
                        else
                        {
                            totrenew = "0";
                        }
                        qry = " Select count(*) TotReserv from priority_studstaff b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code AND month(cur_date) ='" + selyr + "' AND year(cur_date) in ('" + fromdate1 + "','" + todate1 + "') and g.college_code='" + colgcode + "' and (b.is_staff = 0 or b.is_staff = 1)";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(mm,cur_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreservation = Convert.ToString(ds.Tables[0].Rows[0]["TotReserv"]).Trim();
                        }
                        else
                        {
                            totreservation = "0";
                        }
                        if (opac == 1)
                        {
                            qry = " select sum(lib_count) Tot_OPAC from lib_queryhit b where month(lib_date) ='" + selyr + "' and lib_date ='" + fromdate + "' and (is_staff = 0 or is_staff = 1) ";
                            //if (dept != "")
                            //{
                            //    qry = qry + "  and (b.department in('" + cbl_dept.SelectedItem.ToString() + "') or b.department = 'All')";
                            //}
                            qry = qry + " group by datename(mm,lib_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(qry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                opacHits = Convert.ToString(ds.Tables[0].Rows[0]["Tot_OPAC"]);
                            }
                        }
                        qry = "SELECT ISNULL(SUM(A.Tot),0) TotGate FROM (SELECT Count(*) Tot FROM LibUsers U where month(Entry_Date) ='" + selyr + "' AND year(Entry_Date) in ('" + fromdate1 + "','" + todate1 + "') AND (UserCat ='Student' or UserCat = 'Staff')";
                        if (library != "")
                        {
                            qry = qry + " and lib_code in('" + library + "')";
                        }
                        //if (dept != "")
                        //{
                        //    qry = qry + " and dept_name in('" + Convert.ToString(getCblSelectedText(cbl_dept)) + "')";
                        //}
                        if (sem != "")
                        {
                            qry = qry + " and current_semester in('" + sem + "')";
                        }
                        qry = qry + " GROUP BY Roll_No,Lib_Code, datename(mm,Entry_Date)) A";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            gateentry = Convert.ToString(ds.Tables[0].Rows[0]["TotGate"]).Trim();
                        }

                        int tottran = Convert.ToInt32(totissue) + Convert.ToInt32(totreturn) + Convert.ToInt32(totrenew) + Convert.ToInt32(totreservation);
                        drow = dtMonthlyActivity.NewRow();
                        drow["Month"] = Convert.ToString(montno);
                        drow["Total Issue"] = Convert.ToString(totissue);
                        drow["Total Return"] = Convert.ToString(totreturn);
                        drow["Total Renew"] = Convert.ToString(totrenew);
                        drow["Total Reservation"] = Convert.ToString(totreservation);
                        drow["OPAC Hits"] = Convert.ToString(opacHits);
                        drow["Gate Entry"] = Convert.ToString(gateentry);
                        drow["Total Trans"] = Convert.ToString(tottran);
                        dtMonthlyActivity.Rows.Add(drow);

                        tottissue = tottissue + Convert.ToInt32(totissue);
                        tottreturn = tottreturn + Convert.ToInt32(totreturn);
                        tottrenew = tottrenew + Convert.ToInt32(totrenew);
                        tottreser = tottreser + Convert.ToInt32(totreservation);
                        totopachits = totopachits + Convert.ToInt32(opacHits);
                        totgateentry = totgateentry + Convert.ToInt32(gateentry);
                        totttran = totttran + Convert.ToInt32(tottran);

                        grtottissue = grtottissue + tottissue;
                        grtottreturn = grtottreturn + tottreturn;
                        grtottrenew = grtottrenew + tottrenew;
                        grtottreser = grtottreser + tottreser;
                        grtotopachits = grtotopachits + totopachits;
                        grtotgateentry = grtotgateentry + totgateentry;
                        grtotttran = grtotttran + totttran;
                    }

                    #endregion
                }
            }
            drow = dtMonthlyActivity.NewRow();
            drow["Month"] = "Total";
            drow["Total Issue"] = Convert.ToString(tottissue);
            drow["Total Return"] = Convert.ToString(tottreturn);
            drow["Total Renew"] = Convert.ToString(tottrenew);
            drow["Total Reservation"] = Convert.ToString(tottreser);
            drow["OPAC Hits"] = Convert.ToString(totopachits);
            drow["Gate Entry"] = Convert.ToString(totgateentry);
            drow["Total Trans"] = Convert.ToString(totttran);
            dtMonthlyActivity.Rows.Add(drow);

            if (ds.Tables[0].Rows.Count > 0)
            {
                divtable.Visible = true;
                grdBkIssTransReport.DataSource = dtMonthlyActivity;
                grdBkIssTransReport.DataBind();
                grdBkIssTransReport.Visible = true;
                GrdOverDueMemebersList.Visible = false;
                btn_printmaster.Visible = true;
                btn_Excel.Visible = true;
                lbl_reportname.Visible = true;
                txt_excelname.Visible = true;
                div_report.Visible = true;
                btnPopAlertClose.Visible = false;
                divPopupAlert.Visible = false;
                RowHead(grdBkIssTransReport);
            }
            else
            {
                divtable.Visible = false;
                grdBkIssTransReport.Visible = false;
                btn_printmaster.Visible = false;
                btn_Excel.Visible = false;
                lbl_reportname.Visible = false;
                txt_excelname.Visible = false;
                div_report.Visible = false;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Records Found";
                btnPopAlertClose.Visible = true;
                divPopupAlert.Visible = true;
                GrdOverDueMemebersList.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void YearlyActivity(object sender, EventArgs e)
    {

        try
        {
            string fromdate = string.Empty;
            string todate = string.Empty;
            string dateqry = string.Empty;
            string qry = string.Empty;
            string colgcode = string.Empty;
            string library = string.Empty;
            string dept = string.Empty;
            string sem = string.Empty;
            string totissue = string.Empty;
            string totreturn = string.Empty;
            string totrenew = string.Empty;
            string totreservation = string.Empty;
            string opacHits = string.Empty;
            string gateentry = string.Empty;
            if (cbdate1.Checked == false)
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Select The Date";
                divPopupAlert.Visible = true;
                btnPopAlertClose.Visible = true;
                return;
            }
            int opac = 1;
            library = getCblSelectedValue(cbl_library);
            dept = getCblSelectedValue(cbl_dept);
            string sem1 = getCblSelectedText(cbl_sem);
            string[] Semster = sem1.Split(new string[] { "','" }, StringSplitOptions.None);
            string SemVal = string.Empty;

            for (int i = 0; i < Semster.Length; i++)
            {
                SemVal = Semster[i];
                string SemCode = SemVal.Split(' ')[0];
                if (!sem.Contains(SemCode))
                {
                    if (sem == "")
                        sem = SemCode;
                    else
                        sem = sem + "','" + SemCode;
                }
            }
            colgcode = Convert.ToString(ddl_collegename.SelectedValue);
            string libraryname = Convert.ToString(cbl_library.SelectedValue).Trim();
            if (cbdate1.Checked == true)
            {
                fromdate = txt_fromdate1.Text;
                string[] frdate = fromdate.Split('/');
                if (frdate.Length == 3)
                    fromdate = frdate[2].ToString() + "/" + frdate[1].ToString() + "/" + frdate[0].ToString();

                todate = txt_todate.Text;
                string[] tdate = todate.Split('/');
                if (tdate.Length == 3)
                    todate = tdate[2].ToString() + "/" + tdate[1].ToString() + "/" + tdate[0].ToString();
                dateqry = " and entry_date between '" + fromdate + "' and '" + todate + "'";
            }
            else
            {
                dateqry = "";
            }
            DataTable dtYearlyActivity = new DataTable();
            DataRow drow;

            dtYearlyActivity.Columns.Add("Year", typeof(string));
            dtYearlyActivity.Columns.Add("Total Issue", typeof(string));
            dtYearlyActivity.Columns.Add("Total Return", typeof(string));
            dtYearlyActivity.Columns.Add("Total Renew", typeof(string));
            dtYearlyActivity.Columns.Add("Total Reservation", typeof(string));
            dtYearlyActivity.Columns.Add("OPAC Hits", typeof(string));
            dtYearlyActivity.Columns.Add("Gate Entry", typeof(string));
            dtYearlyActivity.Columns.Add("Total Trans", typeof(string));

            drow = dtYearlyActivity.NewRow();
            drow["Year"] = "Year";
            drow["Total Issue"] = "Total Issue";
            drow["Total Return"] = "Total Return";
            drow["Total Renew"] = "Total Renew";
            drow["Total Reservation"] = "Total Reservation";
            drow["OPAC Hits"] = "OPAC Hits";
            drow["Gate Entry"] = "Gate Entry";
            drow["Total Trans"] = "Total Trans";
            dtYearlyActivity.Rows.Add(drow);
            int tottissue = 0;
            int tottreturn = 0;
            int tottrenew = 0;
            int tottreser = 0;
            int totopachits = 0;
            int totgateentry = 0;
            int totttran = 0;

            int grtottissue = 0;
            int grtottreturn = 0;
            int grtottrenew = 0;
            int grtottreser = 0;
            int grtotopachits = 0;
            int grtotgateentry = 0;
            int grtotttran = 0;

            qry = "SELECT datename(yyyy,borrow_Date ) montname, year(borrow_Date) montno FROM Borrow where 1=1  and borrow_Date between '" + fromdate + "' and '" + todate + "'  GROUP BY datename(yyyy,borrow_Date ),year(borrow_Date) order by year(borrow_Date) ";
            ds1.Clear();
            ds1 = d2.select_method_wo_parameter(qry, "text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                int sno = 0;
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    string montno = Convert.ToString(ds1.Tables[0].Rows[i]["montname"]);
                    string selyr = Convert.ToString(ds1.Tables[0].Rows[i]["montno"]);

                    #region Student

                    if (checkusers.Items[0].Selected == true && checkusers.Items[1].Selected == false)
                    {
                        qry = "select count(*) TotIssue  From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where 1=1 and g.college_code='" + colgcode + "' AND year(borrow_date) ='" + selyr + "' and b.is_staff = 0";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(yyyy,borrow_date) order by lib_name,datename(yyyy,borrow_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totissue = Convert.ToString(ds.Tables[0].Rows[0]["TotIssue"]).Trim();
                        }
                        else
                        {
                            totissue = "0";
                        }
                        qry = "select count(*) TotRet  From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where return_flag = 1 and g.college_code='" + colgcode + "' AND year(return_date) ='" + selyr + "' and b.is_staff =0";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(yyyy,return_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreturn = Convert.ToString(ds.Tables[0].Rows[0]["TotRet"]).Trim();
                        }
                        else
                        {
                            totreturn = "0";
                        }
                        qry = "select count(*) TotIssue From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where 1=1 and g.college_code='" + colgcode + "'  AND year(borrow_date) ='" + selyr + "' and b.is_staff = 0 and isnull(renewflag,0) = 1";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(yyyy,borrow_date) order by lib_name,datename(yyyy,borrow_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totrenew = Convert.ToString(ds.Tables[0].Rows[0]["TotIssue"]).Trim();
                        }
                        else
                        {
                            totrenew = "0";
                        }
                        qry = "Select count(*) TotReserv from priority_studstaff b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code AND year(cur_date) ='" + selyr + "'  and g.college_code='" + colgcode + "' and b.is_staff = 0";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(yyyy,cur_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreservation = Convert.ToString(ds.Tables[0].Rows[0]["TotReserv"]).Trim();
                        }
                        else
                        {
                            totreservation = "0";
                        }
                        if (opac == 1)
                        {
                            qry = " select sum(lib_count) Tot_OPAC from lib_queryhit b where year(lib_date) ='" + selyr + "' and is_staff = 0";
                            //if (dept != "")
                            //{
                            //    qry = qry + "  and (b.department in('" + cbl_dept.SelectedItem.ToString() + "') or b.department = 'All')";
                            //}
                            qry = qry + " group by datename(yyyy,lib_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(qry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                opacHits = Convert.ToString(ds.Tables[0].Rows[0]["Tot_OPAC"]);
                            }
                            else
                            {
                                opacHits = "0";
                            }
                        }
                        qry = "SELECT ISNULL(SUM(A.Tot),0) TotGate FROM (SELECT Count(*) Tot FROM LibUsers U where year(Entry_Date) ='" + selyr + "' AND UserCat ='Student'";
                        if (library != "")
                        {
                            qry = qry + " and lib_code in('" + library + "')";
                        }
                        //if (dept != "")
                        //{
                        //    qry = qry + " and  dept_name in('" + Convert.ToString(getCblSelectedText(cbl_dept)) + "')";
                        //}
                        if (sem != "")
                        {
                            qry = qry + " and current_semester in('" + sem + "')";
                        }
                        qry = qry + " GROUP BY Roll_No,Lib_Code, datename(yyyy,Entry_Date)) A";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            gateentry = Convert.ToString(ds.Tables[0].Rows[0]["TotGate"]).Trim();
                        }
                        else
                        {
                            gateentry = "0";
                        }

                        int tottran = Convert.ToInt32(totissue) + Convert.ToInt32(totreturn) + Convert.ToInt32(totrenew) + Convert.ToInt32(totreservation);
                        drow = dtYearlyActivity.NewRow();
                        drow["Year"] = Convert.ToString(montno);
                        drow["Total Issue"] = Convert.ToString(totissue);
                        drow["Total Return"] = Convert.ToString(totreturn);
                        drow["Total Renew"] = Convert.ToString(totrenew);
                        drow["Total Reservation"] = Convert.ToString(totreservation);
                        drow["OPAC Hits"] = Convert.ToString(opacHits);
                        drow["Gate Entry"] = Convert.ToString(gateentry);
                        drow["Total Trans"] = Convert.ToString(tottran);
                        dtYearlyActivity.Rows.Add(drow);

                        tottissue = tottissue + Convert.ToInt32(totissue);
                        tottreturn = tottreturn + Convert.ToInt32(totreturn);
                        tottrenew = tottrenew + Convert.ToInt32(totrenew);
                        tottreser = tottreser + Convert.ToInt32(totreservation);
                        totopachits = totopachits + Convert.ToInt32(opacHits);
                        totgateentry = totgateentry + Convert.ToInt32(gateentry);
                        totttran = totttran + Convert.ToInt32(tottran);

                        grtottissue = grtottissue + tottissue;
                        grtottreturn = grtottreturn + tottreturn;
                        grtottrenew = grtottrenew + tottrenew;
                        grtottreser = grtottreser + tottreser;
                        grtotopachits = grtotopachits + totopachits;
                        grtotgateentry = grtotgateentry + totgateentry;
                        grtotttran = grtotttran + totttran;
                    }

                    #endregion

                    #region Staff
                    if (checkusers.Items[1].Selected == true && checkusers.Items[0].Selected == false)
                    {
                        qry = "select count(*) TotIssue  From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where 1=1 and g.college_code='" + colgcode + "'  AND year(borrow_date) ='" + selyr + "' and b.is_staff = 1";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(yyyy,borrow_date) order by lib_name,datename(yyyy,borrow_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totissue = Convert.ToString(ds.Tables[0].Rows[0]["TotIssue"]).Trim();
                        }
                        else
                        {
                            totissue = "0";
                        }
                        qry = "select count(*) TotRet  From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where return_flag = 1 and g.college_code='" + colgcode + "'  AND year(return_date) ='" + selyr + "' and b.is_staff =1";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(yyyy,return_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreturn = Convert.ToString(ds.Tables[0].Rows[0]["TotRet"]).Trim();
                        }
                        else
                        {
                            totreturn = "0";
                        }
                        qry = "select count(*) TotIssue From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no  inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where 1=1 and g.college_code='" + colgcode + "' AND year(borrow_date) ='" + selyr + "' and b.is_staff = 1 and isnull(renewflag,0) = 1";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(yyyy,borrow_date) order by lib_name,datename(yyyy,borrow_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totrenew = Convert.ToString(ds.Tables[0].Rows[0]["TotIssue"]).Trim();
                        }
                        else
                        {
                            totrenew = "0";
                        }
                        qry = "Select count(*) TotReserv from priority_studstaff b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code AND year(cur_date) ='" + selyr + "' and g.college_code='" + colgcode + "' and b.is_staff = 1  ";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(yyyy,cur_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreservation = Convert.ToString(ds.Tables[0].Rows[0]["TotReserv"]).Trim();
                        }
                        else
                        {
                            totreservation = "0";
                        }
                        if (opac == 1)
                        {
                            qry = " select sum(lib_count) Tot_OPAC from lib_queryhit b where year(lib_date) ='" + selyr + "' and lib_date ='" + fromdate + "' and is_staff = 1";
                            //if (dept != "")
                            //{
                            //    qry = qry + "  and (b.department in('" + cbl_dept.SelectedItem.ToString() + "') or b.department = 'All')";
                            //}
                            qry = qry + " group by datename(yyyy,lib_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(qry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                opacHits = Convert.ToString(ds.Tables[0].Rows[0]["Tot_OPAC"]);
                            }
                            else
                            {
                                opacHits = "0";
                            }
                        }
                        qry = "SELECT ISNULL(SUM(A.Tot),0) TotGate FROM (SELECT Count(*) Tot FROM LibUsers U where year(Entry_Date) ='" + selyr + "' AND UserCat ='Staff'";
                        if (library != "")
                        {
                            qry = qry + " and lib_code in('" + library + "')";
                        }
                        qry = qry + " GROUP BY Roll_No) A";
                        //if (dept != "")
                        //{
                        //    qry = qry + " and  dept_name in('" + Convert.ToString(getCblSelectedText(cbl_dept)) + "')";
                        //}
                        qry = qry + " GROUP BY Roll_No,Lib_Code, datename(yyyy,Entry_Date)) A";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            gateentry = Convert.ToString(ds.Tables[0].Rows[0]["TotGate"]).Trim();
                        }
                        else
                        {
                            gateentry = "0";
                        }

                        int tottran = Convert.ToInt32(totissue) + Convert.ToInt32(totreturn) + Convert.ToInt32(totrenew) + Convert.ToInt32(totreservation);
                        drow = dtYearlyActivity.NewRow();
                        drow["Year"] = Convert.ToString(montno);
                        drow["Total Issue"] = Convert.ToString(totissue);
                        drow["Total Return"] = Convert.ToString(totreturn);
                        drow["Total Renew"] = Convert.ToString(totrenew);
                        drow["Total Reservation"] = Convert.ToString(totreservation);
                        drow["OPAC Hits"] = Convert.ToString(opacHits);
                        drow["Gate Entry"] = Convert.ToString(gateentry);
                        drow["Total Trans"] = Convert.ToString(tottran);
                        dtYearlyActivity.Rows.Add(drow);

                        tottissue = tottissue + Convert.ToInt32(totissue);
                        tottreturn = tottreturn + Convert.ToInt32(totreturn);
                        tottrenew = tottrenew + Convert.ToInt32(totrenew);
                        tottreser = tottreser + Convert.ToInt32(totreservation);
                        totopachits = totopachits + Convert.ToInt32(opacHits);
                        totgateentry = totgateentry + Convert.ToInt32(gateentry);
                        totttran = totttran + Convert.ToInt32(tottran);

                        grtottissue = grtottissue + tottissue;
                        grtottreturn = grtottreturn + tottreturn;
                        grtottrenew = grtottrenew + tottrenew;
                        grtottreser = grtottreser + tottreser;
                        grtotopachits = grtotopachits + totopachits;
                        grtotgateentry = grtotgateentry + totgateentry;
                        grtotttran = grtotttran + totttran;
                    }

                    #endregion

                    #region All

                    if (checkusers.Items[0].Selected == true && checkusers.Items[1].Selected == true)
                    {
                        qry = " select count(*) TotIssue  From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where 1=1 and g.college_code='" + colgcode + "' AND year(borrow_date) ='" + selyr + "' and (b.is_staff = 0 or b.is_staff = 1) ";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + "and r.current_semester in('" + sem + "')";
                        }

                        qry = qry + " group by b.lib_code,lib_name,datename(yyyy,borrow_date) order by lib_name,datename(yyyy,borrow_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totissue = Convert.ToString(ds.Tables[0].Rows[0]["TotIssue"]).Trim();
                        }
                        else
                        {
                            totissue = "0";
                        }
                        qry = "select count(*) TotRet From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where return_flag = 1 and g.college_code='" + colgcode + "' AND year(return_date) ='" + selyr + "' and (b.is_staff =0 or b.is_staff = 1)";

                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + "and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(yyyy,return_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreturn = Convert.ToString(ds.Tables[0].Rows[0]["TotRet"]).Trim();
                        }
                        else
                        {
                            totreturn = "0";
                        }
                        qry = "select count(*) TotIssue From borrow b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code Where 1=1 and g.college_code='" + colgcode + "'  AND year(borrow_date) ='" + selyr + "' and (b.is_staff = 0 or b.is_staff = 1) and isnull(renewflag,0) = 1";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(yyyy,borrow_date) order by lib_name,datename(yyyy,borrow_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totrenew = Convert.ToString(ds.Tables[0].Rows[0]["TotIssue"]).Trim();
                        }
                        else
                        {
                            totrenew = "0";
                        }
                        qry = " Select count(*) TotReserv from priority_studstaff b inner join library l on l.lib_code = b.lib_code inner join registration r on r.roll_no = b.roll_no inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code AND year(cur_date) ='" + selyr + "' and g.college_code='" + colgcode + "' and (b.is_staff = 0 or b.is_staff = 1) ";
                        if (library != "")
                        {
                            qry = qry + " and b.lib_code in('" + library + "')";
                        }
                        if (dept != "")
                        {
                            qry = qry + " and d.dept_code in('" + dept + "')";
                        }
                        if (sem != "")
                        {
                            qry = qry + " and r.current_semester in('" + sem + "')";
                        }
                        qry = qry + " group by b.lib_code,lib_name,datename(yyyy,cur_date)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totreservation = Convert.ToString(ds.Tables[0].Rows[0]["TotReserv"]).Trim();
                        }
                        else
                        {
                            totreservation = "0";
                        }
                        if (opac == 1)
                        {
                            qry = " select sum(lib_count) Tot_OPAC from lib_queryhit b where year(lib_date) ='" + selyr + "'  and lib_date ='" + fromdate + "' and (is_staff = 0 or is_staff = 1) ";
                            //if (dept != "")
                            //{
                            //    qry = qry + "  and (b.department in('" + cbl_dept.SelectedItem.ToString() + "') or b.department = 'All')";
                            //}
                            qry = qry + " group by datename(yyyy,lib_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(qry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                opacHits = Convert.ToString(ds.Tables[0].Rows[0]["Tot_OPAC"]);
                            }
                            else
                            {
                                opacHits = "0";
                            }
                        }
                        qry = "SELECT ISNULL(SUM(A.Tot),0) TotGate FROM (SELECT Count(*) Tot FROM LibUsers U where year(Entry_Date) ='" + selyr + "' AND (UserCat ='Student' or UserCat = 'Staff')";
                        if (library != "")
                        {
                            qry = qry + " and lib_code in('" + library + "')";
                        }
                        //if (dept != "")
                        //{
                        //    qry = qry + " and  dept_name in('" + Convert.ToString(getCblSelectedText(cbl_dept)) + "')";
                        //}
                        if (sem != "")
                        {
                            qry = qry + " and current_semester in('" + sem + "')";
                        }
                        qry = qry + " GROUP BY Roll_No,Lib_Code, datename(yyyy,Entry_Date)) A";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(qry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            gateentry = Convert.ToString(ds.Tables[0].Rows[0]["TotGate"]).Trim();
                        }
                        else
                        {
                            gateentry = "0";
                        }
                        int tottran = Convert.ToInt32(totissue) + Convert.ToInt32(totreturn) + Convert.ToInt32(totrenew) + Convert.ToInt32(totreservation);
                        drow = dtYearlyActivity.NewRow();
                        drow["Year"] = Convert.ToString(montno);
                        drow["Total Issue"] = Convert.ToString(totissue);
                        drow["Total Return"] = Convert.ToString(totreturn);
                        drow["Total Renew"] = Convert.ToString(totrenew);
                        drow["Total Reservation"] = Convert.ToString(totreservation);
                        drow["OPAC Hits"] = Convert.ToString(opacHits);
                        drow["Gate Entry"] = Convert.ToString(gateentry);
                        drow["Total Trans"] = Convert.ToString(tottran);
                        dtYearlyActivity.Rows.Add(drow);

                        tottissue = tottissue + Convert.ToInt32(totissue);
                        tottreturn = tottreturn + Convert.ToInt32(totreturn);
                        tottrenew = tottrenew + Convert.ToInt32(totrenew);
                        tottreser = tottreser + Convert.ToInt32(totreservation);
                        totopachits = totopachits + Convert.ToInt32(opacHits);
                        totgateentry = totgateentry + Convert.ToInt32(gateentry);
                        totttran = totttran + Convert.ToInt32(tottran);

                        grtottissue = grtottissue + tottissue;
                        grtottreturn = grtottreturn + tottreturn;
                        grtottrenew = grtottrenew + tottrenew;
                        grtottreser = grtottreser + tottreser;
                        grtotopachits = grtotopachits + totopachits;
                        grtotgateentry = grtotgateentry + totgateentry;
                        grtotttran = grtotttran + totttran;
                    }

                    #endregion
                }
            }
            drow = dtYearlyActivity.NewRow();
            drow["Year"] = "Total";
            drow["Total Issue"] = Convert.ToString(tottissue);
            drow["Total Return"] = Convert.ToString(tottreturn);
            drow["Total Renew"] = Convert.ToString(tottrenew);
            drow["Total Reservation"] = Convert.ToString(tottreser);
            drow["OPAC Hits"] = Convert.ToString(totopachits);
            drow["Gate Entry"] = Convert.ToString(totgateentry);
            drow["Total Trans"] = Convert.ToString(totttran);
            dtYearlyActivity.Rows.Add(drow);

            if (ds.Tables[0].Rows.Count > 0)
            {
                divtable.Visible = true;
                grdBkIssTransReport.DataSource = dtYearlyActivity;
                grdBkIssTransReport.DataBind();
                grdBkIssTransReport.Visible = true;
                GrdOverDueMemebersList.Visible = false;
                btn_printmaster.Visible = true;
                btn_Excel.Visible = true;
                lbl_reportname.Visible = true;
                txt_excelname.Visible = true;
                div_report.Visible = true;
                btnPopAlertClose.Visible = false;
                divPopupAlert.Visible = false;
                RowHead(grdBkIssTransReport);
            }
            else
            {
                divtable.Visible = false;
                grdBkIssTransReport.Visible = false;
                btn_printmaster.Visible = false;
                btn_Excel.Visible = false;
                lbl_reportname.Visible = false;
                txt_excelname.Visible = false;
                div_report.Visible = false;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Records Found";
                btnPopAlertClose.Visible = true;
                divPopupAlert.Visible = true;
                GrdOverDueMemebersList.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void OverDueMemebersList(object sender, EventArgs e)
    {
        try
        {
            string fromdate = string.Empty;
            string todate = string.Empty;
            string dateqry = string.Empty;
            string qry = string.Empty;
            string colgcode = string.Empty;
            string library = string.Empty;
            string dept = string.Empty;
            string sem = string.Empty;
            string totissue = string.Empty;
            string totreturn = string.Empty;
            string totrenew = string.Empty;
            string totreservation = string.Empty;
            string opacHits = string.Empty;
            string gateentry = string.Empty;
            library = getCblSelectedValue(cbl_library);
            dept = getCblSelectedValue(cbl_dept);
            string sem1 = getCblSelectedText(cbl_sem);
            string[] Semster = sem1.Split(new string[] { "','" }, StringSplitOptions.None);
            string SemVal = string.Empty;

            for (int i = 0; i < Semster.Length; i++)
            {
                SemVal = Semster[i];
                string SemCode = SemVal.Split(' ')[0];
                if (!sem.Contains(SemCode))
                {
                    if (sem == "")
                        sem = SemCode;
                    else
                        sem = sem + "','" + SemCode;
                }
            }
            colgcode = Convert.ToString(ddl_collegename.SelectedValue);
            string libraryname = Convert.ToString(cbl_library.SelectedValue).Trim();
            if (cbdate1.Checked == true)
            {
                fromdate = txt_fromdate1.Text;
                DateTime dt = new DateTime();
                dt = Convert.ToDateTime(fromdate);
                fromdate = dt.ToString("yyyy/MM/dd");
                todate = txt_todate.Text;
                DateTime dt1 = new DateTime();
                dt1 = Convert.ToDateTime(todate);
                todate = dt1.ToString("yyyy/MM/dd");
                dateqry = " and entry_date between '" + fromdate + "' and '" + todate + "'";
            }
            else
            {
                dateqry = "";
            }
            DataTable dtOverDueMemebersList = new DataTable();
            DataRow drow;

            dtOverDueMemebersList.Columns.Add("Library", typeof(string));
            dtOverDueMemebersList.Columns.Add("Acc No", typeof(string));
            dtOverDueMemebersList.Columns.Add("Title", typeof(string));
            dtOverDueMemebersList.Columns.Add("Token No", typeof(string));
            dtOverDueMemebersList.Columns.Add("Course", typeof(string));
            dtOverDueMemebersList.Columns.Add("Due Date", typeof(string));
            dtOverDueMemebersList.Columns.Add("Over Due Days", typeof(string));
            dtOverDueMemebersList.Columns.Add("Over Due Amount", typeof(string));

            drow = dtOverDueMemebersList.NewRow();
            drow["Library"] = "Library";
            drow["Acc No"] = "Acc No";
            drow["Title"] = "Title";
            drow["Token No"] = "Token No";
            drow["Course"] = "Course";
            drow["Due Date"] = "Due Date";
            drow["Over Due Days"] = "Over Due Days";
            drow["Over Due Amount"] = "Over Due Amount";
            dtOverDueMemebersList.Rows.Add(drow);
            #region Student

            if (checkusers.Items[0].Selected == true && checkusers.Items[1].Selected == false)
            {
                qry = "select b.roll_no,r.stud_name,b.lib_code,lib_name,b.Acc_No,k.Title,token_no,course_name+'-'+dept_name course,due_date,isnull(datediff(day,due_date,getdate()),0) overduedays,0 as overdueamt,return_type,c.course_id,d.dept_code from borrow b inner join library l on b.lib_code = l.lib_code inner join registration r on (r.roll_no = b.roll_no or r.lib_id = b.roll_no)  inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code inner join bookdetails k on k.acc_no = b.acc_no and g.college_code= '" + colgcode + "' and b.is_staff = 0 and b.return_flag = 0 and datediff(day,due_date,getdate()) > 0";
                if (library != "")
                {
                    qry = qry + " and b.lib_code in('" + library + "')";
                }
                if (dept != "")
                {
                    qry = qry + " and d.dept_code in('" + dept + "')";
                }
                if (sem != "")
                {
                    qry = qry + " and r.current_semester in('" + sem + "')";
                }
                qry = qry + " order by lib_name,course_name,dept_name,b.roll_no,overduedays";

                ds.Clear();
                ds = d2.select_method_wo_parameter(qry, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int sno = 0;
                    string rolno = string.Empty;
                    string rollno = string.Empty;
                    string dueamt = string.Empty;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sno++;
                        rollno = Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]);
                        string amt = string.Empty;
                        if (rolno != "")
                            if (rolno != "" && rolno != rollno)
                            {
                                drow = dtOverDueMemebersList.NewRow();
                                drow["Over Due Days"] = "Amount To Be Paid";
                                drow["Over Due Amount"] = Convert.ToString(amounttot);
                                dtOverDueMemebersList.Rows.Add(drow);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "Amount To Be Paid";
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Bold = true;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(amounttot);
                                amttot = 0;
                                //FpSpread1.Sheets[0].RowCount++;
                            }

                        string studname = Convert.ToString(ds.Tables[0].Rows[i]["stud_name"]);
                        string accno = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                        string libname = Convert.ToString(ds.Tables[0].Rows[i]["lib_name"]);
                        string title = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                        string duedate = Convert.ToString(ds.Tables[0].Rows[i]["overduedays"]);
                        dueamt = Convert.ToString(ds.Tables[0].Rows[i]["overdueamt"]);
                        string tokenno = Convert.ToString(ds.Tables[0].Rows[i]["token_no"]);
                        string duedate1 = Convert.ToString(ds.Tables[0].Rows[i]["due_date"]);
                        DateTime dt = new DateTime();
                        dt = Convert.ToDateTime(duedate1);
                        duedate1 = dt.ToString("dd-MM-yyyy");
                        string course = Convert.ToString(ds.Tables[0].Rows[i]["course"]);
                        string mattype = Convert.ToString(ds.Tables[0].Rows[i]["return_type"]);
                        ds.Tables[0].DefaultView.RowFilter = " roll_no='" + Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]) + "' ";

                        if (rollno != rolno)
                        {
                            drow = dtOverDueMemebersList.NewRow();
                            drow["Library"] = rollno + studname;
                            dtOverDueMemebersList.Rows.Add(drow);
                            drow = dtOverDueMemebersList.NewRow();
                            drow["Library"] = "Material Type:" + mattype;
                            dtOverDueMemebersList.Rows.Add(drow);

                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = rollno + studname;
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                            //FpSpread1.Sheets[0].RowCount++;
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Material Type:" + mattype;
                        }
                        qry = "Select isnull(fine,0) fine from lib_master where code ='" + Convert.ToString(ds.Tables[0].Rows[i]["course_id"]) + "~" + Convert.ToString(ds.Tables[0].Rows[i]["dept_code"]) + "'  ";
                        ds1.Clear();
                        ds1 = d2.select_method_wo_parameter(qry, "text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                            {
                                string fineamt = Convert.ToString(ds1.Tables[0].Rows[j]["fine"]);
                                dueamount = Convert.ToInt32(duedate) * Convert.ToInt32(fineamt);
                            }
                        }
                        amounttot = Convert.ToInt32(dueamount) + amttot;
                        drow = dtOverDueMemebersList.NewRow();
                        drow["Library"] = libname;
                        drow["Acc No"] = accno;
                        drow["Title"] = title;
                        drow["Token No"] = tokenno;
                        drow["Course"] = course;
                        drow["Due Date"] = duedate1;
                        drow["Over Due Days"] = duedate;
                        drow["Over Due Amount"] = dueamount;
                        //FpSpread1.Sheets[0].RowCount++;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chk;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = accno;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = libname;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = accno;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = title;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = tokenno;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = course;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = duedate1;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = duedate;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dueamount);

                        amttot = amounttot;
                        rolno = rollno;
                        dtOverDueMemebersList.Rows.Add(drow);
                    }
                    drow = dtOverDueMemebersList.NewRow();
                    drow["Over Due Days"] = "Amount To Be Paid";
                    drow["Over Due Amount"] = Convert.ToString(amounttot);
                    dtOverDueMemebersList.Rows.Add(drow);

                    //FpSpread1.Sheets[0].RowCount++;
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "Amount To Be Paid";
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Bold = true;
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(amounttot);
                }
            }

            #endregion

            #region Staff

            if (checkusers.Items[1].Selected == true && checkusers.Items[0].Selected == false)
            {
                qry = "select b.roll_no,m.staff_name as stud_name,b.lib_code,lib_name,b.acc_no,k.title,token_no,dept_name course, due_date,isnull(datediff(day,due_date,getdate()),0) overduedays,0 as overdueamt,return_type from borrow b inner join library l on b.lib_code = l.lib_code inner join staffmaster m on (m.staff_code = b.roll_no or m.lib_id = b.roll_no) inner join stafftrans t on t.staff_code = m.staff_code inner join department d on d.dept_code = t.dept_code inner join bookdetails k on k.acc_no = b.acc_no and d.college_code='" + colgcode + "' and b.is_staff = 1 and b.return_flag = 0 and t.latestrec = 1 " + dateqry + " and datediff(day,due_date,getdate()) > 0";

                if (library != "")
                {
                    qry = qry + " and b.lib_code in('" + library + "')";
                }
                else if (dept != "")
                {
                    qry = qry + " and d.dept_code in('" + dept + "')";
                }

                qry = qry + " order by lib_name,dept_name,b.roll_no,overduedays";

                ds.Clear();
                ds = d2.select_method_wo_parameter(qry, "text");
                int sno = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string rolno = string.Empty;
                    string rollno = string.Empty;
                    string dueamt = string.Empty;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string amt = string.Empty;
                        rollno = Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]);
                        if (rolno != "")
                            if (rolno != "" && rolno != rollno)
                            {
                                drow = dtOverDueMemebersList.NewRow();
                                drow["Over Due Days"] = "Amount To Be Paid";
                                drow["Over Due Amount"] = Convert.ToString(amounttot);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "Amount To Be Paid";
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Bold = true;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(amounttot);
                                amttot = 0;
                                //FpSpread1.Sheets[0].RowCount++;
                            }
                        string studname = Convert.ToString(ds.Tables[0].Rows[i]["stud_name"]);
                        string accno = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                        string libname = Convert.ToString(ds.Tables[0].Rows[i]["lib_name"]);
                        string title = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                        string duedate = Convert.ToString(ds.Tables[0].Rows[i]["overduedays"]);
                        dueamt = Convert.ToString(ds.Tables[0].Rows[i]["overdueamt"]);
                        string tokenno = Convert.ToString(ds.Tables[0].Rows[i]["token_no"]);
                        string duedate1 = Convert.ToString(ds.Tables[0].Rows[i]["due_date"]);
                        DateTime dt = new DateTime();
                        dt = Convert.ToDateTime(duedate1);
                        duedate1 = dt.ToString("dd-MM-yyyy");
                        string course = Convert.ToString(ds.Tables[0].Rows[i]["course"]);
                        string mattype = Convert.ToString(ds.Tables[0].Rows[i]["return_type"]);
                        if (rollno != rolno)
                        {
                            drow = dtOverDueMemebersList.NewRow();
                            drow["Library"] = rollno + studname;
                            drow = dtOverDueMemebersList.NewRow();
                            drow["Library"] = "Material Type:" + mattype;
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = rollno + studname;
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                            //FpSpread1.Sheets[0].RowCount++;
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Material Type:" + mattype;
                        }

                        qry = "Select isnull(fine,0) fine from lib_master where code ='" + rollno + "'";
                        ds1.Clear();
                        ds1 = d2.select_method_wo_parameter(qry, "text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                            {
                                string fineamt = Convert.ToString(ds1.Tables[0].Rows[j]["fine"]);
                                dueamount = Convert.ToInt32(duedate) * Convert.ToInt32(fineamt);

                            }
                        }
                        amounttot = Convert.ToInt32(dueamount) + amttot;
                        drow = dtOverDueMemebersList.NewRow();
                        drow["Library"] = libname;
                        drow["Acc No"] = accno;
                        drow["Title"] = title;
                        drow["Token No"] = tokenno;
                        drow["Course"] = course;
                        drow["Due Date"] = duedate1;
                        drow["Over Due Days"] = duedate;
                        drow["Over Due Amount"] = dueamount;
                        //FpSpread1.Sheets[0].RowCount++;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chk;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = libname;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = accno;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = title;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = tokenno;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = course;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = duedate1;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = duedate;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dueamount);

                        amttot = amounttot;
                        rolno = rollno;
                        dtOverDueMemebersList.Rows.Add(drow);
                    }
                    drow = dtOverDueMemebersList.NewRow();
                    drow["Over Due Days"] = "Amount To Be Paid";
                    drow["Over Due Amount"] = Convert.ToString(amounttot);
                    dtOverDueMemebersList.Rows.Add(drow);
                    //FpSpread1.Sheets[0].RowCount++;
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "Amount To Be Paid";
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Bold = true;
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(amounttot);
                }
            }
            #endregion

            #region All

            if (checkusers.Items[0].Selected == true && checkusers.Items[1].Selected == true)
            {
                qry = "select b.roll_no,r.stud_name,b.lib_code,lib_name,b.acc_no,k.title,token_no,course_name+'-'+dept_name course, due_date,isnull(datediff(day,due_date,getdate()),0) overduedays,0 as overdueamt,return_type,c.course_id,d.dept_code,is_staff from borrow b inner join library l on b.lib_code = l.lib_code inner join registration r on (r.roll_no = b.roll_no or r.lib_id = b.roll_no) inner join degree g on g.degree_code = r.degree_code inner join course c on c.course_id = g.course_id inner join department d on d.dept_code = g.dept_code inner join bookdetails k on k.acc_no = b.acc_no and g.college_code='" + colgcode + "' and b.is_staff = 0 and b.return_flag = 0 " + dateqry + " and datediff(day,due_date,getdate()) > 0";
                if (library != "")
                {
                    qry = qry + " and b.lib_code in('" + library + "')";
                }
                if (dept != "")
                {
                    qry = qry + " and d.dept_code in('" + dept + "')";
                }
                if (sem != "")
                {
                    qry = qry + " and r.current_semester in('" + sem + "')";
                }
                qry = qry + " UNION ALL select b.roll_no,m.staff_name as stud_name,b.lib_code,lib_name,b.acc_no,k.title,token_no,dept_name course, due_date,isnull(datediff(day,due_date,getdate()),0) overduedays, 0 as overdueamt,return_type,0 as course_id,0 as dept_code,is_staff from borrow b inner join library l on b.lib_code = l.lib_code inner join staffmaster m on (m.staff_code = b.roll_no or m.lib_id = b.roll_no)  inner join stafftrans t on t.staff_code = m.staff_code inner join department d on d.dept_code = t.dept_code inner join bookdetails k on k.acc_no = b.acc_no and d.college_code='" + colgcode + "' and b.is_staff = 1 and b.return_flag = 0 and t.latestrec = 1 " + dateqry + " and datediff(day,due_date,getdate()) > 0";
                if (library != "")
                {
                    qry = qry + " and b.lib_code in('" + library + "')";
                }
                if (dept != "")
                {
                    qry = qry + " and d.dept_code in('" + dept + "')";
                }
                qry = qry + " order by lib_name,b.roll_no,overduedays";
                ds.Clear();
                ds = d2.select_method_wo_parameter(qry, "text");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    int sno = 0;
                    string rolno = string.Empty;
                    string rollno = string.Empty;
                    string dueamt = string.Empty;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sno++;
                        string amt = string.Empty;
                        rollno = Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]);
                        if (rolno != "")
                            if (rolno != "" && rolno != rollno)
                            {
                                drow = dtOverDueMemebersList.NewRow();
                                drow["Over Due Days"] = "Amount To Be Paid";
                                drow["Over Due Amount"] = Convert.ToString(amounttot);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "Amount To Be Paid";
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Bold = true;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(amounttot);
                                amttot = 0;
                                //FpSpread1.Sheets[0].RowCount++;
                            }

                        string studname = Convert.ToString(ds.Tables[0].Rows[i]["stud_name"]);
                        string accno = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                        string libname = Convert.ToString(ds.Tables[0].Rows[i]["lib_name"]);
                        string title = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                        string duedate = Convert.ToString(ds.Tables[0].Rows[i]["overduedays"]);
                        dueamt = Convert.ToString(ds.Tables[0].Rows[i]["overdueamt"]);
                        string tokenno = Convert.ToString(ds.Tables[0].Rows[i]["token_no"]);
                        string duedate1 = Convert.ToString(ds.Tables[0].Rows[i]["due_date"]);
                        DateTime dt = new DateTime();
                        dt = Convert.ToDateTime(duedate1);
                        duedate1 = dt.ToString("dd-MM-yyyy");
                        string course = Convert.ToString(ds.Tables[0].Rows[i]["course"]);
                        string mattype = Convert.ToString(ds.Tables[0].Rows[i]["return_type"]);
                        string is_staff = Convert.ToString(ds.Tables[0].Rows[i]["is_staff"]);

                        if (rollno != rolno)
                        {
                            drow = dtOverDueMemebersList.NewRow();
                            drow["Library"] = rollno + studname;
                            drow = dtOverDueMemebersList.NewRow();
                            drow["Library"] = "Material Type:" + mattype;
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = rollno + studname;
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                            //FpSpread1.Sheets[0].RowCount++;
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Material Type:" + mattype;
                        }
                        if (is_staff == "False")
                        {
                            qry = "Select isnull(fine,0) fine from lib_master where code ='" + Convert.ToString(ds.Tables[0].Rows[i]["course_id"]) + "~" + Convert.ToString(ds.Tables[0].Rows[i]["dept_code"]) + "'   ";
                        }
                        else if (is_staff == "True")
                        {
                            qry = "Select isnull(fine,0) fine from lib_master where code ='" + rollno + "'";
                        }
                        ds1.Clear();
                        ds1 = d2.select_method_wo_parameter(qry, "text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                            {
                                string fineamt = Convert.ToString(ds1.Tables[0].Rows[j]["fine"]);
                                dueamount = Convert.ToInt32(duedate) * Convert.ToInt32(fineamt);

                            }
                        }
                        amounttot = Convert.ToInt32(dueamount) + amttot;
                        drow = dtOverDueMemebersList.NewRow();
                        drow["Library"] = libname;
                        drow["Acc No"] = accno;
                        drow["Title"] = title;
                        drow["Token No"] = tokenno;
                        drow["Course"] = course;
                        drow["Due Date"] = duedate1;
                        drow["Over Due Days"] = duedate;
                        drow["Over Due Amount"] = dueamount;
                        //FpSpread1.Sheets[0].RowCount++;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chk;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = libname;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = accno;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = title;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = tokenno;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = course;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = duedate1;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = duedate;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dueamount);

                        amttot = amounttot;
                        rolno = rollno;
                        dtOverDueMemebersList.Rows.Add(drow);
                    }
                    drow = dtOverDueMemebersList.NewRow();
                    drow["Over Due Days"] = "Amount To Be Paid";
                    drow["Over Due Amount"] = Convert.ToString(amounttot);

                    dtOverDueMemebersList.Rows.Add(drow);
                    //FpSpread1.Sheets[0].RowCount++;
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "Amount To Be Paid";
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Bold = true;
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(amounttot);
                }

            }
            #endregion

            if (ds.Tables[0].Rows.Count > 0)
            {
                divtable.Visible = true;
                GrdOverDueMemebersList.DataSource = dtOverDueMemebersList;
                GrdOverDueMemebersList.DataBind();
                GrdOverDueMemebersList.Visible = true;
                grdBkIssTransReport.Visible = false;
                btn_printmaster.Visible = true;
                btn_Excel.Visible = true;
                lbl_reportname.Visible = true;
                txt_excelname.Visible = true;
                div_report.Visible = true;
                btnPopAlertClose.Visible = false;
                divPopupAlert.Visible = false;
                RowHead(grdBkIssTransReport);
            }
            else
            {
                divtable.Visible = false;
                grdBkIssTransReport.Visible = false;
                btn_printmaster.Visible = false;
                btn_Excel.Visible = false;
                lbl_reportname.Visible = false;
                txt_excelname.Visible = false;
                div_report.Visible = false;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Records Found";
                btnPopAlertClose.Visible = true;
                divPopupAlert.Visible = true;
                GrdOverDueMemebersList.Visible = false;
            }

        }
        catch
        {
        }
    }

    protected void RowHead(GridView grdBkIssTransReport)
    {
        for (int head = 0; head < 1; head++)
        {
            grdBkIssTransReport.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            grdBkIssTransReport.Rows[head].Font.Bold = true;
            grdBkIssTransReport.Rows[head].HorizontalAlign = HorizontalAlign.Center;

        }
    }

    protected void grdBkIssTransReport_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdBkIssTransReport.PageIndex = e.NewPageIndex;
        btngo_OnClick(sender, e);
    }

    protected void GrdOverDueMemebersList_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdOverDueMemebersList.PageIndex = e.NewPageIndex;
        btngo_OnClick(sender, e);
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        string report = txt_excelname.Text;
        if (report.ToString().Trim() != "")
        {
            d2.printexcelreportgrid(grdBkIssTransReport, report);
            lbl_norec.Visible = false;
        }
        else
        {
            lbl_norec.Text = "Please Enter Your Report Name";
            lbl_norec.Visible = true;
        }
        btn_Excel.Focus();
    }

    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string attendance = "Transaction Report";
            string pagename = "BookIssueReturnTransactionReport.aspx";
            string ss = null;
            Printcontrol.loadspreaddetails(grdBkIssTransReport, pagename, attendance, 0, ss);
            Printcontrol.Visible = true;
        }
        catch { }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        lblAlertMsg.Text = string.Empty;
        lblAlertMsg.Visible = false;
        divPopupAlert.Visible = false;
        lblAlertMsg.Text = string.Empty;
    }

    protected void txtexcelname_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txt_excelname.Visible = true;
            btn_Excel.Visible = true;
            btn_printmaster.Visible = true;
            lbl_reportname.Visible = true;
            btn_Excel.Focus();
            if (txt_excelname.Text == "")
            {
                lbl_norec.Visible = true;
            }
            else
            {
                lbl_norec.Visible = false;
            }
        }
        catch { }
    }

    //protected void FpSpread1_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{


    //    string actrow = Convert.ToString(e.SheetView.ActiveRow).Trim();
    //    if (flag_true == false && actrow == "0")
    //    {
    //        for (int j = 3; j < Convert.ToInt16(FpSpread1.Sheets[0].RowCount); j++)
    //        {
    //            string actcol = Convert.ToString(e.SheetView.ActiveColumn).Trim();
    //            string seltext = Convert.ToString(e.EditValues[Convert.ToInt16(actcol)]).Trim();
    //            if (seltext != "System.Object")
    //                FpSpread1.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = Convert.ToString(seltext).Trim();

    //        }
    //        flag_true = true;
    //    }

    //}
}

