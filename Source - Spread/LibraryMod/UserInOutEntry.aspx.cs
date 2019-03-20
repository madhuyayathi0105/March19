using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Drawing;
using InsproDataAccess;
using System.Collections;
using System.Configuration;

public partial class LibraryMod_UserInOutEntry : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    DataSet ds = new DataSet();
    DataSet dsload = new DataSet();
    connection cs = new connection();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    int count = 0;
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    DataTable dtCommon = new DataTable();
    DataSet dsprint = new DataSet();
    static int clickCount = 0;
    ArrayList a1 = new ArrayList();
    ArrayList a2 = new ArrayList();
    ArrayList a3 = new ArrayList();
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
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode = Session["collegecode"].ToString();

            if (!IsPostBack)
            {
                Session["clickCount"] = 0;
                Bindcollege();
                binddept();
                bindsem();
                getLibPrivil();
                hitstatus();
                bindBtch();
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
                TimeSelector1.SetTime(FromTime.Hour, FromTime.Minute, FromTime.Second, am_pm);

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
                TimeSelector2.SetTime(ToTime.Hour, ToTime.Minute, ToTime.Second, AM_PM);
                TimeSelector1.Enabled = false;
                TimeSelector2.Enabled = false;
                TimeSelector3.Enabled = false;
                TimeSelector4.Enabled = false;

                DateTime PopFromTime = DateTime.Parse("9:00:00 AM");
                MKB.TimePicker.TimeSelector.AmPmSpec Popam_pm;
                if (FromTime.ToString("tt") == "AM")
                {
                    Popam_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                }
                else
                {
                    Popam_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                }
                TimeSelector3.SetTime(FromTime.Hour, FromTime.Minute, FromTime.Second, Popam_pm);

                DateTime PopToTime = DateTime.Parse("5:00:00 PM");

                MKB.TimePicker.TimeSelector.AmPmSpec PopAM_PM;
                if (ToTime.ToString("tt") == "AM")
                {
                    PopAM_PM = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                }
                else
                {
                    PopAM_PM = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                }
                TimeSelector4.SetTime(ToTime.Hour, ToTime.Minute, ToTime.Second, PopAM_PM);
            }
        }
        catch (Exception ex) { }
    }

    #region college

    public void Bindcollege()
    {
        try
        {
            //ddl_library.Items.Clear();
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
            //d2.sendErrorMail(ex, userCollegeCode, "LibraryRackAllocation");
        }
    }

    protected void ddl_collegename_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        getLibPrivil();
    }

    #endregion

    #region User

    protected void cbl_users_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(Cb_user, checkusers, sampleTxt, "User", "--Select--");
        binddept();
    }

    #endregion

    #region Library

    protected void cbl_library_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cb_lib, cbl_library, sampleTxt, "Libraryname", "-Select--");

    }

    public void bindLibrary(string libCollection)
    {
        cbl_library.Items.Clear();
        ds.Clear();
        string collegecode = Convert.ToString(ddl_collegename.SelectedValue);
        string SelectQ = string.Empty;

        SelectQ = "select *,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + libCollection + " and college_code in('" + collegecode + "') ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
        ds = d2.select_method_wo_parameter(SelectQ, "text");
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

    #endregion

    #region dept

    public void binddept()
    {
        try
        {
            cbl_dept.Items.Clear();
            string deptquery = string.Empty;
            ds.Clear();
            //if (checkusers.Items[2].Selected)
            //{                          
            //    cb_dept.Enabled = false;               
            //    if (cbl_dept.Items.Count > 0)
            //    {
            //        cbl_dept.Items.Clear();
            //        cb_dept.Checked = false;
            //    }
            //}
            if (checkusers.Items[0].Selected || checkusers.Items[0].Selected && checkusers.Items[2].Selected)
            {
                string collegecode = ddl_collegename.SelectedValue;
                deptquery = " SELECT Course_Name + ' - ' + Dept_Name Degree,Degree_Code FROM Degree G,Course C,Department D WHERE G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code AND G.College_Code ='" + collegecode + "' ORDER BY Course_Name,Dept_Name";
                //deptquery = "Select Distinct Dept_name from libusers where usercat='Student' order by dept_name";
                ds = d2.select_method_wo_parameter(deptquery, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds;
                    cbl_dept.DataTextField = "Degree";
                    cbl_dept.DataValueField = "Degree_Code";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            cbl_dept.Items[i].Selected = true;
                        }
                        cb_dept.Checked = true;
                    }
                }

                cb_dept.Enabled = true;
                cbl_dept.Enabled = true;
            }
            if (checkusers.Items[1].Selected || checkusers.Items[1].Selected && checkusers.Items[2].Selected)
            {
                deptquery = "Select Distinct Dept_name from libusers where usercat='Staff' order by dept_name";
                ds = d2.select_method_wo_parameter(deptquery, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds;
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "dept_name";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            cbl_dept.Items[i].Selected = true;
                        }
                        cb_dept.Checked = true;
                    }
                }
                cb_dept.Enabled = true;
                cbl_dept.Enabled = true;
            }
            if (checkusers.Items[0].Selected && checkusers.Items[1].Selected || checkusers.Items[0].Selected && checkusers.Items[1].Selected && checkusers.Items[2].Selected)
            {
                deptquery = "Select Distinct Dept_name from libusers where usercat<>'visitor' order by dept_name";
                ds = d2.select_method_wo_parameter(deptquery, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds;
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "dept_name";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            cbl_dept.Items[i].Selected = true;
                        }
                        cb_dept.Checked = true;
                    }
                }
                cb_dept.Enabled = true;
                cbl_dept.Enabled = true;
            }
        }
        catch { }
    }

    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //TextBox sampleTxt = new TextBox();
        //CallCheckboxListChange(cb_dept, cbl_dept, sampleTxt, "Department", "--Select--");

    }

    #endregion

    #region Semester

    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cb_sem, cbl_sem, sampleTxt, "Semester", "--Select--");

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

    #endregion

    #region batch

    public void bindBtch()
    {
        try
        {
            int i = 0;
            cbl_batch.Items.Clear();
            cb_batch.Checked = false;
            txt_batch.Text = "---Select---";
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
                if (cbl_batch.Items.Count > 0)
                {
                    for (i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[i].Selected = true;
                    }
                    txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                    cb_batch.Checked = true;
                }
            }
        }
        catch { }
    }

    protected void cb_batch_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_batch, cbl_batch, txt_batch, "Batch", "--Select--");
            binddept();
        }
        catch { }
    }

    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_batch, cbl_batch, txt_batch, "Batch", "--Select--");
            binddept();
        }
        catch { }
    }

    #endregion

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

    protected void hitstatus()
    {
        try
        {
            string SelQry = string.Empty;
            string Date = DateTime.Now.ToString("MM/dd/yyy");
            string libraryCode = getCblSelectedValue(cbl_library);
            SelQry = "select count(*) as count from libusers where entry_date='" + Date + "' and usercat = 'Staff' and lib_code=" + libraryCode + "";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelQry, "text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                stafftxt.Text = Convert.ToString(dsload.Tables[0].Rows[0]["count"]);
            }
            else
                stafftxt.Text = "0";
            stafftxt.Enabled = false;

            SelQry = "select count(*) as count from libusers where entry_date='" + Date + "' and usercat = 'Student' and lib_code=" + libraryCode + "";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelQry, "text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                studtxt.Text = Convert.ToString(dsload.Tables[0].Rows[0]["count"]);
            }
            else
                studtxt.Text = "0";
            studtxt.Enabled = false;

            SelQry = "select count(*) as count from libusers where entry_date='" + Date + "' and usercat = 'Visitor' and lib_code=" + libraryCode + "";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelQry, "text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                visitortxt.Text = Convert.ToString(dsload.Tables[0].Rows[0]["count"]);
            }
            else
                visitortxt.Text = "0";
            visitortxt.Enabled = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void reports_changed(object sender, EventArgs e)
    {
        if (reports.Items[0].Selected)
        {
            BestMem.Visible = true;
            MemEntry.Visible = false;
            VisitEntry.Visible = false;
            VisitDet.Visible = false;
            divSpreadReport.Visible = false;
            divSpreadVisitWithTime.Visible = false;
            divVisit_Details.Visible = false;
            Batchwise.Visible = false;
        }
        if (reports.Items[1].Selected)
        {
            MemEntry.Visible = true;
            BestMem.Visible = false;
            VisitEntry.Visible = false;
            VisitDet.Visible = false;
            divSpreadReport.Visible = false;
            divSpreadVisitWithTime.Visible = false;
            divVisit_Details.Visible = false;
            Batchwise.Visible = false;
        }
        if (reports.Items[2].Selected)
        {
            VisitEntry.Visible = true;
            BestMem.Visible = false;
            MemEntry.Visible = false;
            VisitDet.Visible = false;
            divSpreadReport.Visible = false;
            divSpreadVisitWithTime.Visible = false;
            divVisit_Details.Visible = false;
            Batchwise.Visible = false;
        }
        if (reports.Items[3].Selected)
        {
            VisitDet.Visible = true;
            BestMem.Visible = false;
            MemEntry.Visible = false;
            VisitEntry.Visible = false;
            divSpreadReport.Visible = false;
            divSpreadVisitWithTime.Visible = false;
            divVisit_Details.Visible = false;
            Batchwise.Visible = false;
        }
        if (reports.Items[4].Selected)
        {
            Batchwise.Visible = false;
            divSpreadVisitWithTime.Visible = false;
            GrdVisitWithTime.Visible = false;
            divVisit_Details.Visible = false;
            DivVisitDetWithTime.Visible = true;
            BestMem.Visible = false;
            MemEntry.Visible = false;
            VisitEntry.Visible = false;
            VisitDet.Visible = false;
            divSpreadReport.Visible = false;
            divSpreadVisitWithTime.Visible = true;
            LstBoxTime.Items.Clear();
            string Sql = string.Empty;
            string college = Convert.ToString(ddl_collegename.SelectedValue);
            Sql = "SELECT * FROM LibRepTimeSettings where collegecode =" + college + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string FromTime = Convert.ToString(ds.Tables[0].Rows[i]["FromTime"]);
                    string ToTime = Convert.ToString(ds.Tables[0].Rows[i]["ToTime"]);
                    LstBoxTime.Items.Add(FromTime + "-" + ToTime);
                }
            }
        }
        if (reports.Items[5].Selected)
        {
            divSpreadVisitWithTime.Visible = false;
            GrdVisitWithTime.Visible = false;
            divVisit_Details.Visible = false;
            DivVisitDetWithTime.Visible = false;
            BestMem.Visible = false;
            MemEntry.Visible = false;
            VisitEntry.Visible = false;
            VisitDet.Visible = false;
            divSpreadReport.Visible = false;
            divSpreadVisitWithTime.Visible = false;
            Batchwise.Visible = true;
        }
        if (reports.Items[6].Selected)
        {
            VisitEntry.Visible = true;
            BestMem.Visible = false;
            MemEntry.Visible = false;
            VisitDet.Visible = false;
            divSpreadReport.Visible = false;
            divSpreadVisitWithTime.Visible = false;
            divVisit_Details.Visible = false;
            Batchwise.Visible = false;
        }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    protected void MementryDeptwise_OnCheckedChanged(object sender, EventArgs e)
    {
        mementrylist.Visible = true;
        Individualdept.Visible = true;
        Commondept.Visible = true;
        rollstaff.Visible = false;
        txtroll.Visible = false;
        Lblname.Visible = false;
        Nametxt.Visible = false;
        divSpreadReport.Visible = false;
    }

    protected void MementryDayWise_OnCheckedChanged(object sender, EventArgs e)
    {
        mementrylist.Visible = false;
        Individualdept.Visible = false;
        Commondept.Visible = false;
        rollstaff.Visible = false;
        txtroll.Visible = false;
        Lblname.Visible = false;
        Nametxt.Visible = false;
        divSpreadReport.Visible = false;
    }

    protected void imageVisitDetWithTime_Click(object sender, EventArgs e)
    {
        DivVisitDetWithTime.Visible = false;

    }

    protected void cbdate_Changed(object sender, EventArgs e)
    {
        if (cbdate.Checked)
        {
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_todate.Attributes.Add("readonly", "readonly");
            txt_fromdate.Enabled = true;
            txt_todate.Enabled = true;
        }
        else
        {
            txt_fromdate.Enabled = false;
            txt_todate.Enabled = false;
        }
    }

    protected void cbtime_Changed(object sender, EventArgs e)
    {
        if (cbtime.Checked)
        {
            fromtime.Enabled = true;
            totime.Enabled = true;
        }
        else
        {
            fromtime.Enabled = false;
            totime.Enabled = false;
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        if (reports.Items[0].Selected)
        {
            Best_Member(sender, e);
        }
        if (reports.Items[1].Selected)
        {
            if (daywise.Checked)
            {
                Members_DayWise_Entry(sender, e);
            }
            if (DeptWise.Checked)
            {
                if (Commondept.Checked)
                {
                    MembersCommonDept_Entry(sender, e);
                }
                if (Individualdept.Checked)
                {
                    MembersIndDeptWise_Entry(sender, e);
                }
            }
        }
        if (reports.Items[2].Selected)
        {
            Visitor_Entry_Statistics(sender, e);
        }
        if (reports.Items[3].Selected)
        {
            Visit_Details(sender, e);
        }
        if (reports.Items[4].Selected)
        {
            Visit_Details_WithTime(sender, e);
        }
        if (reports.Items[5].Selected)
        {
            BatchWise(sender, e);
        }
        if (reports.Items[6].Selected)
        {
            Visitor_Statistics_Batch_Year(sender, e);
        }
    }

    //protected void grdUserReport_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    //{
    //    grdUserReport.PageIndex = e.NewPageIndex;
    //    btngo_Click(sender, e);
    //}

    //protected void GrdVisitWithTime_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    //{
    //    GrdVisitWithTime.PageIndex = e.NewPageIndex;
    //    btngo_Click(sender, e);
    //}

    //protected void grdVisit_Details_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    //{
    //    grdVisit_Details.PageIndex = e.NewPageIndex;
    //    btngo_Click(sender, e);
    //}

    protected void Best_Member(object sender, EventArgs e)
    {
        try
        {
            int gvHasRows = grdUserReport.Rows.Count;
            if (gvHasRows > 0)
            {
                grdUserReport.Columns.Clear();
                grdUserReport.DataBind();
            }

            divVisit_Details.Visible = false;
            divSpreadVisitWithTime.Visible = false;
            divSpreadReport.Visible = true;
            grdUserReport.Visible = true;
            DataTable dtBest_Member = new DataTable();
            DataRow drow;

            dtBest_Member.Columns.Add("S.No", typeof(string));
            dtBest_Member.Columns.Add("RollNo", typeof(string));
            dtBest_Member.Columns.Add("Name", typeof(string));
            dtBest_Member.Columns.Add("No Of Days", typeof(string));
            dtBest_Member.Columns.Add("No Of Visit", typeof(string));
            dtBest_Member.Columns.Add("Total No. of Hours", typeof(string));


            #region Query

            DataSet record = new DataSet();
            string college = Convert.ToString(ddl_collegename.SelectedValue);
            string library = getCblSelectedValue(cbl_library);
            string dept = getCblSelectedValue(cbl_dept);
            string sem = getCblSelectedText(cbl_sem);
            string RollNo = string.Empty;
            string StrDate = string.Empty;
            string StrTime = string.Empty;
            string sql = string.Empty;
            string selectQry = string.Empty;
            double LngTotDays = 0;
            double LngTotVisit = 0;
            string StrTotHour1 = string.Empty;
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
            string stuOrStaffOrVisitor = string.Empty;
            string usercat = string.Empty;
            int sno = 0;
            string timeTot = "";
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            string[] Semster = sem.Split(new string[] { "','" }, StringSplitOptions.None);
            string SemVal = string.Empty;
            string CurrentSem = "";
            for (int i = 0; i < Semster.Length; i++)
            {
                SemVal = Semster[i];
                string SemCode = SemVal.Split(' ')[0];
                if (!CurrentSem.Contains(SemCode))
                {
                    if (CurrentSem == "")
                        CurrentSem = SemCode;
                    else
                        CurrentSem = CurrentSem + "','" + SemCode;
                }
            }
            DateTime F_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector1.Hour, TimeSelector1.Minute, TimeSelector1.Second, TimeSelector1.AmPm));
            DateTime T_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector2.Hour, TimeSelector2.Minute, TimeSelector2.Second, TimeSelector2.AmPm));

            if (cbdate.Checked)
                StrDate = " and entry_date between '" + fromdate + "' and '" + todate + "'";
            else
                StrDate = "";
            if (cbtime.Checked)
                StrTime = " AND ((CONVERT(datetime,Entry_Time,108) BETWEEN '" + F_time + "' AND '" + T_time + "' ) OR (CONVERT(datetime,Exit_Time,108) BETWEEN '" + F_time + "' AND '" + T_time + "' ))";
            else
                StrTime = "";
            if (UserCount == 1)
            {
                #region Student

                if (checkusers.Items[0].Selected)
                {
                    selectQry = "select usercat,Roll_No as Roll_No,Stud_Name as Stud_Name,Dept_Name from libusers u ,library l where u.lib_code=l.lib_code and l.college_code in('" + college + "') " + StrDate + " " + StrTime + " and u.usercat = 'Student' and roll_no <> ''";
                    if (library != "")
                    {
                        selectQry += " and u.lib_code in('" + library + "')";
                    }
                    if (dept != "")
                    {
                        selectQry += " and u.dept_name in('" + dept + "')";
                    }
                    if (sem != "")
                    {
                        selectQry += " and u.current_semester in('" + CurrentSem + "')";
                    }
                    selectQry += " group by usercat,roll_no,stud_name,dept_name order by usercat,dept_name,roll_no,stud_name";

                    record = d2.select_method_wo_parameter(selectQry, "text");
                    if (record.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < record.Tables[0].Rows.Count; j++)
                        {
                            sno++;
                            string StuRollNo = Convert.ToString(record.Tables[0].Rows[j]["Roll_No"]);
                            usercat = Convert.ToString(record.Tables[0].Rows[j]["usercat"]);
                            selectQry = "select count(TotDays) TotDays from (select count(*) TotDays from libusers u ,library l Where u.lib_code = l.lib_code and u.usercat = 'Student' and u.roll_no ='" + StuRollNo + "' " + StrDate + " " + StrTime + " and l.college_code='" + college + "'";
                            if (library != "")
                            {
                                selectQry += " and u.lib_code in('" + library + "')";
                            }
                            if (dept != "")
                            {
                                selectQry += " and u.dept_name in('" + dept + "')";
                            }
                            selectQry += " group by roll_no,entry_date ) a";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                LngTotDays = Convert.ToDouble(ds.Tables[0].Rows[0]["TotDays"]);
                            }

                            selectQry = "select count(*) TotVisit from libusers u ,library l Where u.lib_code = l.lib_code and u.usercat = 'Student' and u.roll_no ='" + StuRollNo + "' " + StrDate + " " + StrTime + " and l.college_code='" + college + "'";
                            if (library != "")
                            {
                                selectQry += " and u.lib_code in('" + library + "')";
                            }
                            if (dept != "")
                            {
                                selectQry += " and u.dept_name in('" + dept + "')";
                            }
                            selectQry += " group by roll_no";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                LngTotVisit = Convert.ToDouble(ds.Tables[0].Rows[0]["TotVisit"]);
                            }

                            //========================caclulating hours=========================//

                            selectQry = "select convert(char(8),dateadd(second,SUM ( DATEPART(hh,(convert(datetime,convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114),1))) * 3600 + DATEPART(mi, (convert(datetime,convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114),1))) * 60 + DATEPART(ss,(convert(datetime,convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114),1)))),0),108) as Duration from libusers u ,library l where u.lib_code=l.lib_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and exit_time<>'' and u.usercat = 'Student' and roll_no ='" + StuRollNo + "'";
                            if (library != "")
                            {
                                selectQry += " and u.lib_code in('" + library + "')";
                            }
                            if (dept != "")
                            {
                                selectQry += " and u.dept_name in('" + dept + "')";
                            }
                            selectQry += " group by roll_no";
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                StrTotHour1 = "";
                                timeTot = Convert.ToString(ds.Tables[0].Rows[0]["Duration"]);
                            }
                            //selectQry = " select count(*) TotVisit,(select count(TotDays) TotDays from (select count(*) TotDays from libusers u ,library l Where u.lib_code = l.lib_code and u.usercat = 'Student' and u.roll_no in ('" + StuRollNo + "') and l.college_code='" + college + "' group by entry_date) a) TotDays,CASE WHEN exit_time='' THEN '' else convert(char(8),dateadd(second,SUM ( DATEPART(hh,(convert(datetime,convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114),1))) * 3600 + DATEPART(mi, (convert(datetime,convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114),1))) * 60 + DATEPART(ss,(convert(datetime,convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114),1)))),0),108) end as Duration from libusers u ,library l Where u.lib_code = l.lib_code and u.usercat = 'Student' and u.roll_no in('" + StuRollNo + "') " + StrDate + " " + StrTime + " and l.college_code='" + college + "'";
                            //if (library != "")
                            //{
                            //    selectQry += " and u.lib_code in('" + library + "')";
                            //}
                            //if (dept != "")
                            //{
                            //    selectQry += " and u.dept_name in('" + dept + "')";
                            //}
                            //if (sem != "")
                            //{
                            //    selectQry += " and u.current_semester in('" + CurrentSem + "')";
                            //}
                            //selectQry += " group by roll_no,exit_time";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            // LngTotDays = Convert.ToDouble(ds.Tables[0].Rows[0]["TotDays"]);
                            // LngTotVisit = Convert.ToDouble(ds.Tables[0].Rows[0]["TotVisit"]);
                            // StrTotHour1 = "";
                            // timeTot = Convert.ToString(ds.Tables[0].Rows[0]["Duration"]);
                            if (stuOrStaffOrVisitor != usercat)
                            {
                                drow = dtBest_Member.NewRow();
                                drow["RollNo"] = Convert.ToString(usercat.ToUpper());
                                stuOrStaffOrVisitor = usercat;
                            }
                            drow = dtBest_Member.NewRow();
                            drow["S.No"] = sno;
                            drow["RollNo"] = Convert.ToString(StuRollNo);
                            drow["Name"] = Convert.ToString(record.Tables[0].Rows[j]["Stud_Name"]);
                            drow["No Of Days"] = Convert.ToString(LngTotDays);
                            drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                            drow["Total No. of Hours"] = Convert.ToString(timeTot);
                            dtBest_Member.Rows.Add(drow);
                        }
                        grdUserReport.DataSource = dtBest_Member;
                        grdUserReport.DataBind();
                        grdUserReport.Visible = true;
                        divSpreadReport.Visible = true;
                        print.Visible = true;
                        print1.Visible = false;
                        print2.Visible = false;


                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "No records found";
                        grdUserReport.Visible = false;
                        divSpreadReport.Visible = false;
                        print.Visible = false;
                        print1.Visible = false;
                        print2.Visible = false;
                    }
                }
                #endregion

                #region Staff

                if (checkusers.Items[1].Selected)
                {
                    selectQry = "select usercat,Roll_No as Roll_No,Stud_Name as Stud_Name,Dept_Name,Count(*) TotVisit from libusers u ,library l where u.lib_code=l.lib_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Staff' and roll_no <> ''";

                    if (library != "")
                    {
                        selectQry += " and u.lib_code in('" + library + "')";
                    }
                    if (dept != "")
                    {
                        selectQry += " and u.dept_name in('" + dept + "')";
                    }
                    selectQry += " group by usercat,roll_no,stud_name,dept_name order by TotVisit Desc,usercat,dept_name,roll_no,stud_name";
                    record = d2.select_method_wo_parameter(selectQry, "text");
                    if (record.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < record.Tables[0].Rows.Count; j++)
                        {
                            sno++;
                            string StuRollNo = Convert.ToString(record.Tables[0].Rows[j]["Roll_No"]);
                            usercat = Convert.ToString(record.Tables[0].Rows[j]["usercat"]);

                            selectQry = "select count(TotDays) TotDays from (select count(*) TotDays from libusers u ,library l Where u.lib_code = l.lib_code and u.usercat = 'Staff' and u.roll_no ='" + StuRollNo + "' " + StrDate + " " + StrTime + " and l.college_code='" + college + "'";
                            if (library != "")
                            {
                                selectQry += " and u.lib_code in('" + library + "')";
                            }
                            if (dept != "")
                            {
                                selectQry += " and u.dept_name in('" + dept + "')";
                            }
                            selectQry += " group by roll_no,entry_date ) a";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                LngTotDays = Convert.ToDouble(ds.Tables[0].Rows[0]["TotDays"]);
                            }

                            selectQry = "select count(*) TotVisit from libusers u ,library l Where u.lib_code = l.lib_code and u.usercat = 'Staff' and u.roll_no ='" + StuRollNo + "' " + StrDate + " " + StrTime + " and l.college_code='" + college + "'";
                            if (library != "")
                            {
                                selectQry += " and u.lib_code in('" + library + "')";
                            }
                            if (dept != "")
                            {
                                selectQry += " and u.dept_name in('" + dept + "')";
                            }
                            selectQry += " group by roll_no";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                LngTotVisit = Convert.ToDouble(ds.Tables[0].Rows[0]["TotVisit"]);
                            }

                            //========================caclulating hours=========================//

                            selectQry = "select convert(char(8),dateadd(second,SUM ( DATEPART(hh,(convert(datetime,convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114),1))) * 3600 + DATEPART(mi, (convert(datetime,convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114),1))) * 60 + DATEPART(ss,(convert(datetime,convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114),1)))),0),108) as Duration from libusers u ,library l where u.lib_code=l.lib_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and exit_time<>'' and u.usercat = 'Staff' and roll_no ='" + StuRollNo + "'";
                            if (library != "")
                            {
                                selectQry += " and u.lib_code in('" + library + "')";
                            }
                            if (dept != "")
                            {
                                selectQry += " and u.dept_name in('" + dept + "')";
                            }
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                StrTotHour1 = "";
                                timeTot = Convert.ToString(ds.Tables[0].Rows[0]["Duration"]);
                            }
                            if (stuOrStaffOrVisitor != usercat)
                            {
                                drow = dtBest_Member.NewRow();
                                drow["RollNo"] = Convert.ToString(usercat.ToUpper());
                                stuOrStaffOrVisitor = usercat;
                            }
                            drow = dtBest_Member.NewRow();
                            drow["S.No"] = sno;
                            drow["RollNo"] = Convert.ToString(StuRollNo);
                            drow["Name"] = Convert.ToString(record.Tables[0].Rows[j]["Stud_Name"]);
                            drow["No Of Days"] = Convert.ToString(LngTotDays);
                            drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                            drow["Total No. of Hours"] = Convert.ToString(timeTot);
                            dtBest_Member.Rows.Add(drow);
                        }
                        grdUserReport.DataSource = dtBest_Member;
                        grdUserReport.DataBind();
                        grdUserReport.Visible = true;
                        divSpreadReport.Visible = true;
                        print.Visible = true;
                        print1.Visible = false;
                        print2.Visible = false;


                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "No records found";
                        grdUserReport.Visible = false;
                        divSpreadReport.Visible = false;
                        print.Visible = false;
                        print1.Visible = false;
                        print2.Visible = false;
                    }
                }
                #endregion

                #region Visitor

                if (checkusers.Items[2].Selected)
                {
                    selectQry = "select usercat,Roll_No as Roll_No,Stud_Name as Stud_Name,Dept_Name,Count(*) TotVisit from libusers u ,library l where u.lib_code=l.lib_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Visitor' and roll_no <> ''";
                    if (library != "")
                    {
                        selectQry += " and u.lib_code in('" + library + "')";
                    }
                    if (dept != "")
                    {
                        selectQry += " and u.dept_name in('" + dept + "')";
                    }
                    selectQry += " group by usercat,roll_no,stud_name,dept_name order by TotVisit Desc,usercat,dept_name,roll_no,stud_name";
                    record = d2.select_method_wo_parameter(selectQry, "text");
                    if (record.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < record.Tables[0].Rows.Count; j++)
                        {
                            sno++;
                            string StuRollNo = Convert.ToString(record.Tables[0].Rows[j]["Roll_No"]);
                            usercat = Convert.ToString(record.Tables[0].Rows[j]["usercat"]);

                            selectQry = "select count(TotDays) TotDays from (select count(*) TotDays from libusers u ,library l Where u.lib_code = l.lib_code and u.usercat = 'Visitor' and u.roll_no ='" + StuRollNo + "' " + StrDate + " " + StrTime + " and l.college_code='" + college + "'";
                            if (library != "")
                            {
                                selectQry += " and u.lib_code in('" + library + "')";
                            }
                            if (dept != "")
                            {
                                selectQry += " and u.dept_name in('" + dept + "')";
                            }
                            selectQry += " group by roll_no,entry_date ) a";

                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                LngTotDays = Convert.ToDouble(ds.Tables[0].Rows[0]["TotDays"]);
                            }

                            selectQry = "select count(*) TotVisit from libusers u ,library l Where u.lib_code = l.lib_code and u.usercat = 'Visitor' and u.roll_no ='" + StuRollNo + "' " + StrDate + " " + StrTime + " and l.college_code='" + college + "'";
                            if (library != "")
                            {
                                selectQry += " and u.lib_code in('" + library + "')";
                            }
                            if (dept != "")
                            {
                                selectQry += " and u.dept_name in('" + dept + "')";
                            }
                            selectQry += " group by roll_no";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                LngTotVisit = Convert.ToDouble(ds.Tables[0].Rows[0]["TotVisit"]);
                            }

                            //========================caclulating hours=========================//

                            selectQry = "select convert(char(8),dateadd(second,SUM ( DATEPART(hh,(convert(datetime,convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114),1))) * 3600 + DATEPART(mi, (convert(datetime,convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114),1))) * 60 + DATEPART(ss,(convert(datetime,convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114),1)))),0),108) as Duration from libusers u ,library l where u.lib_code=l.lib_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and exit_time<>'' and u.usercat = 'Visitor' and roll_no ='" + StuRollNo + "' ";
                            if (library != "")
                            {
                                selectQry += " and u.lib_code in('" + library + "')";
                            }
                            if (dept != "")
                            {
                                selectQry += " and u.dept_name in('" + dept + "')";
                            }
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                StrTotHour1 = "";
                                timeTot = Convert.ToString(ds.Tables[0].Rows[0]["Duration"]);
                            }
                            if (stuOrStaffOrVisitor != usercat)
                            {
                                drow = dtBest_Member.NewRow();
                                drow["RollNo"] = Convert.ToString(usercat.ToUpper());
                                stuOrStaffOrVisitor = usercat;
                            }
                            drow = dtBest_Member.NewRow();
                            drow["S.No"] = sno;
                            drow["RollNo"] = Convert.ToString(StuRollNo);
                            drow["Name"] = Convert.ToString(record.Tables[0].Rows[j]["Stud_Name"]);
                            drow["No Of Days"] = Convert.ToString(LngTotDays);
                            drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                            drow["Total No. of Hours"] = Convert.ToString(timeTot);
                            dtBest_Member.Rows.Add(drow);
                        }
                        grdUserReport.DataSource = dtBest_Member;
                        grdUserReport.DataBind();
                        grdUserReport.Visible = true;
                        divSpreadReport.Visible = true;
                        print.Visible = true;
                        print1.Visible = false;
                        print2.Visible = false;


                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "No records found";
                        grdUserReport.Visible = false;
                        divSpreadReport.Visible = false;
                        print.Visible = false;
                        print1.Visible = false;
                        print2.Visible = false;
                    }
                }
                #endregion
            }

            if (UserCount > 1)
            {
                #region Student,Staff and visitor

                string[] User = UserCat.Split(',');
                string Memtype = string.Empty;
                string MemberVal = string.Empty;
                string UserCategory = string.Empty;

                for (int i = 0; i < User.Length; i++)
                {
                    Memtype = checkusers.Items[i].Text;
                    if (UserCategory == "")
                        UserCategory = Memtype;
                    else
                        UserCategory = UserCategory + "','" + Memtype;
                }
                CurrentSem = CurrentSem + "','0";
                if (checkusers.Items[0].Selected || checkusers.Items[1].Selected || checkusers.Items[2].Selected)
                {
                    selectQry = "select UserCat,Roll_No as Roll_No,Stud_Name as Stud_Name,Dept_Name,Count(*) TotVisit from libusers u ,library l where u.lib_code=l.lib_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat in ('" + UserCategory + "') and roll_no <> ''";
                    if (library != "")
                    {
                        selectQry += " and u.lib_code in('" + library + "')";
                    }
                    if (dept != "")
                    {
                        selectQry += " and u.dept_name in('" + dept + "')";
                    }
                    if (sem != "")
                    {
                        selectQry += " and u.current_semester in('" + CurrentSem + "')";
                    }
                    selectQry += " group by usercat,roll_no,stud_name,dept_name order by usercat desc,TotVisit Desc,dept_name,roll_no,stud_name";
                    record = d2.select_method_wo_parameter(selectQry, "text");
                    if (record.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < record.Tables[0].Rows.Count; j++)
                        {
                            sno++;
                            string StuRollNo = Convert.ToString(record.Tables[0].Rows[j]["Roll_No"]);
                            usercat = Convert.ToString(record.Tables[0].Rows[j]["usercat"]);

                            selectQry = "select count(TotDays) TotDays from (select count(*) TotDays from libusers u ,library l Where u.lib_code = l.lib_code and u.usercat in ('" + UserCategory + "') and u.roll_no ='" + StuRollNo + "' " + StrDate + " " + StrTime + " and l.college_code='" + college + "'";
                            if (library != "")
                            {
                                selectQry += " and u.lib_code in('" + library + "')";
                            }
                            if (dept != "")
                            {
                                selectQry += " and u.dept_name in('" + dept + "')";
                            }
                            if (sem != "")
                            {
                                selectQry += " and u.current_semester in('" + CurrentSem + "')";
                            }
                            selectQry += " group by roll_no,entry_date ) a";

                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                LngTotDays = Convert.ToDouble(ds.Tables[0].Rows[0]["TotDays"]);
                            }

                            selectQry = "select count(*) TotVisit from libusers u ,library l Where u.lib_code = l.lib_code and u.usercat in ('" + UserCategory + "') and u.roll_no ='" + StuRollNo + "' " + StrDate + " " + StrTime + " and l.college_code='" + college + "'";
                            if (library != "")
                            {
                                selectQry += " and u.lib_code in('" + library + "')";
                            }
                            if (dept != "")
                            {
                                selectQry += " and u.dept_name in('" + dept + "')";
                            }
                            if (sem != "")
                            {
                                selectQry += " and u.current_semester in('" + CurrentSem + "')";
                            }
                            selectQry += " group by roll_no";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                LngTotVisit = Convert.ToDouble(ds.Tables[0].Rows[0]["TotVisit"]);
                            }

                            //========================caclulating hours=========================//

                            selectQry = "select convert(char(8),dateadd(second,SUM ( DATEPART(hh,(convert(datetime,convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114),1))) * 3600 + DATEPART(mi, (convert(datetime,convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114),1))) * 60 + DATEPART(ss,(convert(datetime,convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114),1)))),0),108) as Duration from libusers u ,library l where u.lib_code=l.lib_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and exit_time<>'' and u.usercat in ('" + UserCategory + "') and roll_no ='" + StuRollNo + "'";
                            if (library != "")
                            {
                                selectQry += " and u.lib_code in('" + library + "')";
                            }
                            if (dept != "")
                            {
                                selectQry += " and u.dept_name in('" + dept + "')";
                            }
                            if (sem != "")
                            {
                                selectQry += " and u.current_semester in('" + CurrentSem + "')";
                            }
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                StrTotHour1 = "";
                                timeTot = Convert.ToString(ds.Tables[0].Rows[0]["Duration"]);
                            }
                            if (stuOrStaffOrVisitor != usercat)
                            {
                                MemberVal = usercat.ToUpper();
                                drow = dtBest_Member.NewRow();
                                drow["RollNo"] = Convert.ToString(MemberVal);
                                stuOrStaffOrVisitor = usercat;
                                stuOrStaffOrVisitor = usercat;
                            }
                            drow = dtBest_Member.NewRow();
                            drow["S.No"] = sno;
                            drow["RollNo"] = Convert.ToString(StuRollNo);
                            drow["Name"] = Convert.ToString(record.Tables[0].Rows[j]["Stud_Name"]);
                            drow["No Of Days"] = Convert.ToString(LngTotDays);
                            drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                            drow["Total No. of Hours"] = Convert.ToString(timeTot);
                            dtBest_Member.Rows.Add(drow);
                        }
                        grdUserReport.DataSource = dtBest_Member;
                        grdUserReport.DataBind();
                        grdUserReport.Visible = true;
                        divSpreadReport.Visible = true;
                        print.Visible = true;
                        print1.Visible = false;
                        print2.Visible = false;

                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "No records found";
                        grdUserReport.Visible = false;
                        divSpreadReport.Visible = false;
                        print.Visible = false;
                        print1.Visible = false;
                        print2.Visible = false;
                    }
                }
                #endregion
            }

            #endregion
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "UserInOutEntry");
        }
    }

    protected void Members_DayWise_Entry(object sender, EventArgs e)
    {
        try
        {
            int gvHasRows = grdUserReport.Rows.Count;
            if (gvHasRows > 0)
            {
                grdUserReport.Columns.Clear();
                grdUserReport.DataBind();
            }
            DataTable dtMembers_DayWise = new DataTable();
            DataRow drow;
            dtMembers_DayWise.Columns.Add("S.No", typeof(string));
            dtMembers_DayWise.Columns.Add("Date", typeof(string));
            dtMembers_DayWise.Columns.Add("Department", typeof(string));
            dtMembers_DayWise.Columns.Add("No Of Visit", typeof(string));



            Dictionary<int, string> dicMembers_DayWise_Entry = new Dictionary<int, string>();

            #region Query

            DataSet record = new DataSet();
            string college = Convert.ToString(ddl_collegename.SelectedValue);
            string libraryCode = getCblSelectedValue(cbl_library);
            string dept = getCblSelectedValue(cbl_dept);
            string sem = getCblSelectedText(cbl_sem);

            string StrDate = string.Empty;
            string StrTime = string.Empty;

            string selectQry = string.Empty;
            double LngTotVisit = 0;
            double GrandTotVisit = 0;
            string StrTotHour1 = string.Empty;
            int UserCount = 0;

            string LibraryName = string.Empty;
            string lib_Name = string.Empty;
            string Entry_Date = string.Empty;
            string LibEntry_Dt = string.Empty;
            string usercat = string.Empty;
            int sno = 0;
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            string[] Semster = sem.Split(new string[] { "','" }, StringSplitOptions.None);
            string SemVal = string.Empty;
            string CurrentSem = "";

            int S = 0;
            int grdRow = 0;
            for (int i = 0; i < Semster.Length; i++)
            {
                SemVal = Semster[i];
                string SemCode = SemVal.Split(' ')[0];
                if (!CurrentSem.Contains(SemCode))
                {
                    if (CurrentSem == "")
                        CurrentSem = SemCode;
                    else
                        CurrentSem = CurrentSem + "','" + SemCode;
                }
            }
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

            DateTime F_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector1.Hour, TimeSelector1.Minute, TimeSelector1.Second, TimeSelector1.AmPm));
            DateTime T_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector2.Hour, TimeSelector2.Minute, TimeSelector2.Second, TimeSelector2.AmPm));

            if (cbdate.Checked)
                StrDate = " and entry_date between '" + fromdate + "' and '" + todate + "'";
            else
                StrDate = "";
            if (cbtime.Checked)
                StrTime = " AND ((CONVERT(datetime,Entry_Time,108) BETWEEN '" + F_time + "' AND '" + T_time + "' ) OR (CONVERT(datetime,Exit_Time,108) BETWEEN '" + F_time + "' AND '" + T_time + "' ))";
            else
                StrTime = "";
            if (UserCount == 1)
            {
                #region Student

                if (checkusers.Items[0].Selected)
                {
                    selectQry = "select u.Entry_Date,D.Dept_Name,u.lib_code,lib_name,Count(*) Tot from libusers u ,library l,registration r,degree g,course c,department d where u.lib_code=l.lib_code and r.roll_no = u.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = d.dept_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Student'";
                    if (libraryCode != "")
                    {
                        selectQry += " and u.lib_code in('" + libraryCode + "')";
                    }
                    if (dept != "")
                    {
                        selectQry += " and u.dept_name in('" + dept + "')";
                    }
                    if (sem != "")
                    {
                        selectQry += " and u.current_semester in('" + CurrentSem + "')";
                    }
                    selectQry += " group by u.lib_code,lib_name,entry_date,d.dept_name order by lib_name,entry_date,d.dept_name";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectQry, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            LibraryName = Convert.ToString(ds.Tables[0].Rows[i]["lib_name"]);
                            Entry_Date = Convert.ToString(ds.Tables[0].Rows[i]["entry_date"]);
                            string[] ArrEntryDate = Entry_Date.Split('/');
                            Entry_Date = ArrEntryDate[1].ToString() + "/" + ArrEntryDate[0].ToString() + "/" + ArrEntryDate[2].ToString();

                            if (lib_Name != LibraryName)
                            {
                                if (grdUserReport.Rows.Count == 0)
                                {
                                    drow = dtMembers_DayWise.NewRow();
                                    grdRow = dtMembers_DayWise.Rows.Count;
                                }
                                else
                                {
                                    drow = dtMembers_DayWise.NewRow();
                                    grdRow = dtMembers_DayWise.Rows.Count;
                                    drow["Date"] = "Total";
                                    drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                                    dicMembers_DayWise_Entry.Add(grdRow, "Total");
                                    LngTotVisit = 0;
                                    lib_Name = "";
                                    S = 0;
                                }
                                drow["S.No"] = Convert.ToString(LibraryName.ToUpper());
                                lib_Name = LibraryName;
                                dtMembers_DayWise.Rows.Add(drow);
                                dicMembers_DayWise_Entry.Add(grdRow, "LibName");
                            }
                            if (LibEntry_Dt != Entry_Date)
                            {
                                if (S != 0)
                                {
                                    drow = dtMembers_DayWise.NewRow();
                                    grdRow = dtMembers_DayWise.Rows.Count;
                                    drow["S.No"] = "Total";
                                    drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                                    dicMembers_DayWise_Entry.Add(grdRow, "Total");
                                    LngTotVisit = 0;
                                    dtMembers_DayWise.Rows.Add(drow);
                                }

                                S = S + 1;
                                sno++;
                                drow = dtMembers_DayWise.NewRow();
                                drow["S.No"] = sno;
                                drow["Date"] = Convert.ToString(Entry_Date.Split(' ')[0]);
                                LibEntry_Dt = Entry_Date;
                                drow["Department"] = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                drow["No Of Visit"] = Convert.ToString(ds.Tables[0].Rows[i]["Tot"]);
                                LngTotVisit = LngTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                GrandTotVisit = GrandTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                dtMembers_DayWise.Rows.Add(drow);
                            }
                            else
                            {
                                drow = dtMembers_DayWise.NewRow();
                                sno++;
                                drow["S.No"] = sno;
                                drow["Department"] = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                drow["No Of Visit"] = Convert.ToString(ds.Tables[0].Rows[i]["Tot"]);
                                dtMembers_DayWise.Rows.Add(drow);
                                LngTotVisit = LngTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                GrandTotVisit = GrandTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                LibEntry_Dt = Entry_Date;
                                S = S + 1;
                            }
                        }
                        drow = dtMembers_DayWise.NewRow();
                        drow["S.No"] = "Total";
                        drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                        dtMembers_DayWise.Rows.Add(drow);
                        grdRow = dtMembers_DayWise.Rows.Count;
                        dicMembers_DayWise_Entry.Add(grdRow - 1, "Total");
                    }
                    else
                    {
                    }
                }
                #endregion

                #region Staff

                if (checkusers.Items[1].Selected)
                {
                    selectQry = "select u.Entry_Date,D.Dept_Name,u.lib_code,lib_name,Count(*) Tot from libusers u ,library l,staffmaster m,stafftrans t,department d  where u.lib_code=l.lib_code and m.staff_code = u.roll_no and t.staff_code = m.staff_code and t.dept_code = cast(d.dept_code as nvarchar) and l.college_code='" + college + "' and t.latestrec=1 " + StrDate + " " + StrTime + " and u.usercat = 'Staff'";
                    if (libraryCode != "")
                    {
                        selectQry += " and u.lib_code in('" + libraryCode + "')";
                    }
                    if (dept != "")
                    {
                        selectQry += " and u.dept_name in('" + dept + "')";
                    }
                    selectQry += " group by u.lib_code,lib_name,entry_date,d.dept_name order by lib_name,entry_date,d.dept_name";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectQry, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            LibraryName = Convert.ToString(ds.Tables[0].Rows[i]["lib_name"]);
                            Entry_Date = Convert.ToString(ds.Tables[0].Rows[i]["entry_date"]);
                            string[] ArrEntryDate = Entry_Date.Split('/');
                            Entry_Date = ArrEntryDate[1].ToString() + "/" + ArrEntryDate[0].ToString() + "/" + ArrEntryDate[2].ToString();

                            if (lib_Name != LibraryName)
                            {
                                if (grdUserReport.Rows.Count == 0)
                                {
                                    drow = dtMembers_DayWise.NewRow();
                                }
                                else
                                {
                                    drow = dtMembers_DayWise.NewRow();
                                    drow["S.No"] = "Total";
                                    drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                                    grdRow = dtMembers_DayWise.Rows.Count;
                                    dicMembers_DayWise_Entry.Add(grdRow, "Total");
                                    dtMembers_DayWise.Rows.Add(drow);
                                    LngTotVisit = 0;
                                    lib_Name = "";
                                    S = 0;
                                }
                                grdRow = dtMembers_DayWise.Rows.Count;
                                dicMembers_DayWise_Entry.Add(grdRow, "LibName");
                                drow["S.No"] = Convert.ToString(LibraryName.ToUpper());
                                lib_Name = LibraryName;
                                dtMembers_DayWise.Rows.Add(drow);

                            }
                            if (LibEntry_Dt != Entry_Date)
                            {
                                if (S != 0)
                                {
                                    drow = dtMembers_DayWise.NewRow();
                                    grdRow = dtMembers_DayWise.Rows.Count;
                                    dicMembers_DayWise_Entry.Add(grdRow, "Total");
                                    drow["S.No"] = "Total";
                                    drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                                    dtMembers_DayWise.Rows.Add(drow);
                                    LngTotVisit = 0;

                                }
                                sno++;
                                drow = dtMembers_DayWise.NewRow();
                                drow["S.No"] = sno;
                                drow["Date"] = Convert.ToString(Entry_Date.Split(' ')[0]);
                                LibEntry_Dt = Entry_Date;
                                drow["Department"] = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                drow["No Of Visit"] = Convert.ToString(ds.Tables[0].Rows[i]["Tot"]);
                                dtMembers_DayWise.Rows.Add(drow);
                                S = S + 1;
                                LngTotVisit = LngTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                GrandTotVisit = GrandTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                            }
                            else
                            {
                                drow = dtMembers_DayWise.NewRow();
                                sno++;
                                drow["S.No"] = sno;
                                drow["Department"] = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                drow["No Of Visit"] = Convert.ToString(ds.Tables[0].Rows[i]["Tot"]);
                                dtMembers_DayWise.Rows.Add(drow);
                                LngTotVisit = LngTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                GrandTotVisit = GrandTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                LibEntry_Dt = Entry_Date;
                                S = S + 1;
                            }
                        }
                        drow = dtMembers_DayWise.NewRow();
                        grdRow = dtMembers_DayWise.Rows.Count;
                        dicMembers_DayWise_Entry.Add(grdRow, "Total");
                        drow["S.No"] = "Total";
                        drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                        dtMembers_DayWise.Rows.Add(drow);
                        grdRow = dtMembers_DayWise.Rows.Count;
                        dicMembers_DayWise_Entry.Add(grdRow - 1, "Total");
                    }
                    else
                    {
                    }

                }
                #endregion

                #region Visitors

                if (checkusers.Items[2].Selected)
                {
                    selectQry = "select u.Entry_Date,'' as Dept_Name,u.lib_code,lib_name,Count(*) Tot from libusers u ,library l where u.lib_code=l.lib_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Visitor'";
                    if (libraryCode != "")
                    {
                        selectQry += " and u.lib_code in('" + libraryCode + "')";
                    }
                    if (dept != "")
                    {
                        selectQry += " and u.dept_name in('" + dept + "')";
                    }
                    selectQry += " group by u.lib_code,lib_name,entry_date,d.dept_name order by lib_name,entry_date,d.dept_name";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectQry, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            LibraryName = Convert.ToString(ds.Tables[0].Rows[i]["lib_name"]);
                            Entry_Date = Convert.ToString(ds.Tables[0].Rows[i]["entry_date"]);
                            string[] ArrEntryDate = Entry_Date.Split('/');
                            Entry_Date = ArrEntryDate[1].ToString() + "/" + ArrEntryDate[0].ToString() + "/" + ArrEntryDate[2].ToString();

                            if (lib_Name != LibraryName)
                            {
                                if (grdUserReport.Rows.Count == 0)
                                {
                                    drow = dtMembers_DayWise.NewRow();
                                }
                                else
                                {
                                    drow = dtMembers_DayWise.NewRow();
                                    drow["S.No"] = "Total";
                                    drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                                    dtMembers_DayWise.Rows.Add(drow);
                                    LngTotVisit = 0;
                                    lib_Name = "";
                                    S = 0;
                                }
                                grdRow = dtMembers_DayWise.Rows.Count;
                                dicMembers_DayWise_Entry.Add(grdRow, "LibName");
                                drow["S.No"] = Convert.ToString(LibraryName.ToUpper());
                                lib_Name = LibraryName;
                                dtMembers_DayWise.Rows.Add(drow);

                            }
                            if (LibEntry_Dt != Entry_Date)
                            {
                                if (S != 0)
                                {
                                    drow = dtMembers_DayWise.NewRow();
                                    grdRow = dtMembers_DayWise.Rows.Count;
                                    dicMembers_DayWise_Entry.Add(grdRow, "Total");
                                    drow["S.No"] = "Total";
                                    drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                                    dtMembers_DayWise.Rows.Add(drow);
                                    LngTotVisit = 0;
                                }
                                drow = dtMembers_DayWise.NewRow();
                                sno++;
                                drow["S.No"] = sno;
                                drow["Date"] = Convert.ToString(Entry_Date.Split(' ')[0]);
                                LibEntry_Dt = Entry_Date;
                                drow["Department"] = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                drow["No Of Visit"] = Convert.ToString(ds.Tables[0].Rows[i]["Tot"]);
                                dtMembers_DayWise.Rows.Add(drow);
                                S = S + 1;
                                LngTotVisit = LngTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                GrandTotVisit = GrandTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                            }
                            else
                            {
                                drow = dtMembers_DayWise.NewRow();
                                drow["S.No"] = sno;
                                drow["Department"] = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                drow["No Of Visit"] = Convert.ToString(ds.Tables[0].Rows[i]["Tot"]);
                                dtMembers_DayWise.Rows.Add(drow);
                                LngTotVisit = LngTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                GrandTotVisit = GrandTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                LibEntry_Dt = Entry_Date;
                                S = S + 1;
                            }
                        }
                        drow = dtMembers_DayWise.NewRow();
                        grdRow = dtMembers_DayWise.Rows.Count;
                        dicMembers_DayWise_Entry.Add(grdRow, "Total");
                        drow["S.No"] = "Total";
                        drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                        dtMembers_DayWise.Rows.Add(drow);
                        grdRow = dtMembers_DayWise.Rows.Count;
                        dicMembers_DayWise_Entry.Add(grdRow - 1, "Total");
                    }
                    else
                    {
                    }
                }
                #endregion
            }
            if (UserCount > 1)
            {
                #region Student,Staff,Visitor

                string[] User = UserCat.Split(',');
                string Memtype = string.Empty;
                string MemberVal = string.Empty;
                string UserCategory = string.Empty;

                for (int i = 0; i < User.Length; i++)
                {
                    Memtype = checkusers.Items[i].Text;
                    if (UserCategory == "")
                        UserCategory = Memtype;
                    else
                        UserCategory = UserCategory + "','" + Memtype;
                }
                CurrentSem = CurrentSem + "','0";
                if (checkusers.Items[0].Selected || checkusers.Items[1].Selected || checkusers.Items[2].Selected)
                {
                    selectQry = "select u.Entry_Date,u.Dept_Name,u.lib_code,lib_name,Count(*) Tot from libusers u ,library l where u.lib_code=l.lib_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat in('" + UserCategory + "') ";
                    if (libraryCode != "")
                    {
                        selectQry += " and u.lib_code in('" + libraryCode + "')";
                    }
                    if (dept != "")
                    {
                        selectQry += " and u.dept_name in('" + dept + "')";
                    }
                    if (sem != "")
                    {
                        selectQry += " and u.current_semester in('" + CurrentSem + "')";
                    }
                    selectQry += " group by u.lib_code,lib_name,entry_date,u.dept_name order by lib_name,entry_date,u.dept_name";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectQry, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            LibraryName = Convert.ToString(ds.Tables[0].Rows[i]["lib_name"]);
                            Entry_Date = Convert.ToString(ds.Tables[0].Rows[i]["entry_date"]);
                            string[] ArrEntryDate = Entry_Date.Split('/');
                            Entry_Date = ArrEntryDate[1].ToString() + "/" + ArrEntryDate[0].ToString() + "/" + ArrEntryDate[2].ToString();

                            if (lib_Name != LibraryName)
                            {
                                if (grdUserReport.Rows.Count == 0)
                                {
                                    drow = dtMembers_DayWise.NewRow();
                                }
                                else
                                {
                                    grdRow = dtMembers_DayWise.Rows.Count;
                                    dicMembers_DayWise_Entry.Add(grdRow, "Total");
                                    drow = dtMembers_DayWise.NewRow();
                                    drow["S.No"] = "Total";
                                    drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                                    LngTotVisit = 0;
                                    lib_Name = "";
                                    S = 0;
                                }
                                grdRow = dtMembers_DayWise.Rows.Count;
                                dicMembers_DayWise_Entry.Add(grdRow, "LibName");
                                drow["S.No"] = Convert.ToString(LibraryName.ToUpper());
                                lib_Name = LibraryName;
                                dtMembers_DayWise.Rows.Add(drow);
                            }
                            if (LibEntry_Dt != Entry_Date)
                            {
                                if (S != 0)
                                {
                                    grdRow = dtMembers_DayWise.Rows.Count;
                                    dicMembers_DayWise_Entry.Add(grdRow, "Total");
                                    drow = dtMembers_DayWise.NewRow();
                                    drow["S.No"] = "Total";
                                    drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                                    LngTotVisit = 0;
                                    dtMembers_DayWise.Rows.Add(drow);
                                }
                                S = S + 1;
                                sno++;
                                drow = dtMembers_DayWise.NewRow();
                                drow["S.No"] = sno;
                                drow["Date"] = Convert.ToString(Entry_Date.Split(' ')[0]);
                                LibEntry_Dt = Entry_Date;
                                drow["Department"] = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                drow["No Of Visit"] = Convert.ToString(ds.Tables[0].Rows[i]["Tot"]);
                                LngTotVisit = LngTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                GrandTotVisit = GrandTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                dtMembers_DayWise.Rows.Add(drow);
                            }
                            else
                            {
                                sno++;
                                drow = dtMembers_DayWise.NewRow();
                                drow["S.No"] = sno;
                                drow["Department"] = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                drow["No Of Visit"] = Convert.ToString(ds.Tables[0].Rows[i]["Tot"]);
                                LngTotVisit = LngTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                GrandTotVisit = GrandTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                LibEntry_Dt = Entry_Date;
                                S = S + 1;
                                dtMembers_DayWise.Rows.Add(drow);
                            }
                        }
                        grdRow = dtMembers_DayWise.Rows.Count;
                        dicMembers_DayWise_Entry.Add(grdRow, "Total");
                        drow = dtMembers_DayWise.NewRow();
                        drow["S.No"] = "Total";
                        drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                        dtMembers_DayWise.Rows.Add(drow);
                        grdRow = dtMembers_DayWise.Rows.Count;
                        dicMembers_DayWise_Entry.Add(grdRow - 1, "Total");
                    }
                    else
                    {
                    }
                }
                #endregion
            }
            drow = dtMembers_DayWise.NewRow();
            drow["S.No"] = "Grand Total";
            drow["No Of Visit"] = Convert.ToString(GrandTotVisit);
            dtMembers_DayWise.Rows.Add(drow);
            grdRow = dtMembers_DayWise.Rows.Count;
            dicMembers_DayWise_Entry.Add(grdRow - 1, "GrandTotal");
            grdUserReport.DataSource = dtMembers_DayWise;
            grdUserReport.DataBind();
            grdUserReport.Visible = true;
            divSpreadReport.Visible = true;

            grdUserReport.Rows[0].BackColor = Color.FromArgb(12, 166, 202);
            grdUserReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;
            grdUserReport.Rows[0].Font.Bold = true;
            print.Visible = true;
            print1.Visible = false;
            print2.Visible = false;
            int colCount = grdUserReport.Columns.Count;
            foreach (KeyValuePair<int, string> dr in dicMembers_DayWise_Entry)
            {
                int g = dr.Key;
                string DicValue = dr.Value;
                if (DicValue == "LibName")
                {
                    grdUserReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    grdUserReport.Rows[g].Cells[0].ColumnSpan = 4;
                    for (int a = 1; a < 4; a++)
                        grdUserReport.Rows[g].Cells[a].Visible = false;
                    grdUserReport.Rows[g].BackColor = Color.LightCoral;
                }
                else
                {
                    grdUserReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                    grdUserReport.Rows[g].Cells[0].ColumnSpan = 3;
                    for (int a = 1; a < 3; a++)
                        grdUserReport.Rows[g].Cells[a].Visible = false;
                    if (DicValue == "Total")
                        grdUserReport.Rows[g].BackColor = Color.Green;
                    if (DicValue == "GrandTotal")
                        grdUserReport.Rows[g].BackColor = Color.YellowGreen;
                }
            }
            #endregion
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "UserInOutEntry");
        }
    }

    protected void MembersCommonDept_Entry(object sender, EventArgs e)
    {
        try
        {
            int gvHasRows = grdUserReport.Rows.Count;
            if (gvHasRows > 0)
            {
                grdUserReport.Columns.Clear();
                grdUserReport.DataBind();
            }
            divVisit_Details.Visible = false;
            DataTable dtMembersCommonDept = new DataTable();
            DataRow drow;
            dtMembersCommonDept.Columns.Add("S.No", typeof(string));
            dtMembersCommonDept.Columns.Add("Department", typeof(string));
            dtMembersCommonDept.Columns.Add("Date", typeof(string));
            dtMembersCommonDept.Columns.Add("No Of Visit", typeof(string));


            Dictionary<int, string> dicMembersCommonDept_Entry = new Dictionary<int, string>();

            #region Query

            DataSet record = new DataSet();
            string college = Convert.ToString(ddl_collegename.SelectedValue);
            string libraryCode = getCblSelectedValue(cbl_library);
            string dept = getCblSelectedValue(cbl_dept);
            string sem = getCblSelectedText(cbl_sem);
            string StrDate = string.Empty;
            string StrTime = string.Empty;
            string selectQry = string.Empty;
            double LngTotVisit = 0;
            double GrandTotVisit = 0;
            string StrTotHour1 = string.Empty;
            int UserCount = 0;
            string Entry_Date = string.Empty;
            string LibraryName = string.Empty;
            string lib_Name = string.Empty;
            string DeptName = string.Empty;
            string LibDept_Name = string.Empty;
            string usercat = string.Empty;
            int sno = 0;
            int intSNo = 1;
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            string[] Semster = sem.Split(new string[] { "','" }, StringSplitOptions.None);
            string SemVal = string.Empty;
            string CurrentSem = "";
            int S = 0;
            int grdRow = 0;
            for (int i = 0; i < Semster.Length; i++)
            {
                SemVal = Semster[i];
                string SemCode = SemVal.Split(' ')[0];
                if (!CurrentSem.Contains(SemCode))
                {
                    if (CurrentSem == "")
                        CurrentSem = SemCode;
                    else
                        CurrentSem = CurrentSem + "','" + SemCode;
                }
            }
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
            DateTime F_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector1.Hour, TimeSelector1.Minute, TimeSelector1.Second, TimeSelector1.AmPm));
            DateTime T_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector2.Hour, TimeSelector2.Minute, TimeSelector2.Second, TimeSelector2.AmPm));

            if (cbdate.Checked)
                StrDate = " and entry_date between '" + fromdate + "' and '" + todate + "'";
            else
                StrDate = "";
            if (cbtime.Checked)
                StrTime = " AND ((CONVERT(datetime,Entry_Time,108) BETWEEN '" + F_time + "' AND '" + T_time + "' ) OR (CONVERT(datetime,Exit_Time,108) BETWEEN '" + F_time + "' AND '" + T_time + "' ))";
            else
                StrTime = "";
            if (UserCount == 1)
            {
                #region Student

                if (checkusers.Items[0].Selected)
                {
                    selectQry = "select u.Entry_Date,D.Dept_Name,u.lib_code,lib_name,Count(*) Tot from libusers u ,library l,registration r,degree g,course c,department d where u.lib_code=l.lib_code and r.roll_no = u.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = d.dept_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Student'";
                    if (libraryCode != "")
                    {
                        selectQry += " and u.lib_code in('" + libraryCode + "')";
                    }
                    if (dept != "")
                    {
                        selectQry += " and u.dept_name in('" + dept + "')";
                    }
                    if (sem != "")
                    {
                        selectQry += " and u.current_semester in('" + CurrentSem + "')";
                    }
                    selectQry += " group by u.lib_code,lib_name,d.dept_name,entry_date  order by lib_name,d.dept_name,entry_date";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectQry, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            LibraryName = Convert.ToString(ds.Tables[0].Rows[i]["lib_name"]);
                            DeptName = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);
                            Entry_Date = Convert.ToString(ds.Tables[0].Rows[i]["entry_date"]);
                            string[] ArrEntryDate = Entry_Date.Split('/');
                            Entry_Date = ArrEntryDate[1].ToString() + "/" + ArrEntryDate[0].ToString() + "/" + ArrEntryDate[2].ToString();

                            if (lib_Name != LibraryName)
                            {
                                if (grdUserReport.Rows.Count == 0)
                                {
                                    drow = dtMembersCommonDept.NewRow();
                                }
                                else
                                {
                                    grdRow = dtMembersCommonDept.Rows.Count;
                                    dicMembersCommonDept_Entry.Add(grdRow, "Total");
                                    drow = dtMembersCommonDept.NewRow();
                                    drow["S.No"] = "Total";
                                    drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                                    LngTotVisit = 0;
                                    lib_Name = "";
                                    intSNo = 1;
                                    S = 0;
                                }
                                grdRow = dtMembersCommonDept.Rows.Count;
                                dicMembersCommonDept_Entry.Add(grdRow, "LibName");
                                drow["S.No"] = Convert.ToString(LibraryName.ToUpper());
                                dtMembersCommonDept.Rows.Add(drow);
                                lib_Name = LibraryName;
                                intSNo = 1;
                            }
                            if (LibDept_Name != DeptName)
                            {
                                if (S != 0)
                                {
                                    grdRow = dtMembersCommonDept.Rows.Count;
                                    dicMembersCommonDept_Entry.Add(grdRow, "Total");
                                    drow = dtMembersCommonDept.NewRow();
                                    drow["S.No"] = "Total";
                                    drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                                    LngTotVisit = 0;
                                    intSNo = 1;
                                    dtMembersCommonDept.Rows.Add(drow);
                                }
                                S = S + 1;
                                sno++;
                                drow = dtMembersCommonDept.NewRow();
                                drow["S.No"] = sno;
                                drow["Department"] = Convert.ToString(DeptName);
                                drow["Date"] = Convert.ToString(Entry_Date.Split(' ')[0]);
                                LibDept_Name = DeptName;
                                drow["No Of Visit"] = Convert.ToString(ds.Tables[0].Rows[i]["Tot"]);
                                dtMembersCommonDept.Rows.Add(drow);
                                LngTotVisit = LngTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                GrandTotVisit = GrandTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                intSNo = intSNo + 1;
                            }
                            else
                            {
                                drow = dtMembersCommonDept.NewRow();
                                sno++;
                                drow["S.No"] = sno;
                                drow["Date"] = Convert.ToString(Entry_Date.Split(' ')[0]);
                                drow["No Of Visit"] = Convert.ToString(ds.Tables[0].Rows[i]["Tot"]);
                                LngTotVisit = LngTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                GrandTotVisit = GrandTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                LibDept_Name = DeptName;
                                S = S + 1;
                                intSNo = intSNo + 1;
                                dtMembersCommonDept.Rows.Add(drow);
                            }
                        }
                        grdRow = dtMembersCommonDept.Rows.Count;
                        dicMembersCommonDept_Entry.Add(grdRow, "Total");
                        drow = dtMembersCommonDept.NewRow();
                        drow["S.No"] = "Total";
                        drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                        dtMembersCommonDept.Rows.Add(drow);
                    }
                    else
                    {
                    }
                }
                #endregion

                #region Staff

                if (checkusers.Items[1].Selected)
                {
                    selectQry = "select u.Entry_Date,D.Dept_Name,u.lib_code,lib_name,Count(*) Tot from libusers u ,library l,staffmaster m,stafftrans t,department d  where u.lib_code=l.lib_code and m.staff_code = u.roll_no and t.staff_code = m.staff_code and t.dept_code = cast(d.dept_code as nvarchar) and l.college_code='" + college + "' and t.latestrec=1 " + StrDate + " " + StrTime + " and u.usercat = 'Staff'";
                    if (libraryCode != "")
                    {
                        selectQry += " and u.lib_code in('" + libraryCode + "')";
                    }
                    if (dept != "")
                    {
                        selectQry += " and u.dept_name in('" + dept + "')";
                    }
                    selectQry += " group by u.lib_code,lib_name,d.dept_name,entry_date  order by lib_name,d.dept_name,entry_date";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectQry, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            LibraryName = Convert.ToString(ds.Tables[0].Rows[i]["lib_name"]);
                            DeptName = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);
                            Entry_Date = Convert.ToString(ds.Tables[0].Rows[i]["entry_date"]);
                            string[] ArrEntryDate = Entry_Date.Split('/');
                            Entry_Date = ArrEntryDate[1].ToString() + "/" + ArrEntryDate[0].ToString() + "/" + ArrEntryDate[2].ToString();

                            if (lib_Name != LibraryName)
                            {
                                if (grdUserReport.Rows.Count == 0)
                                {
                                    drow = dtMembersCommonDept.NewRow();
                                }
                                else
                                {
                                    grdRow = dtMembersCommonDept.Rows.Count;
                                    dicMembersCommonDept_Entry.Add(grdRow, "Total");
                                    drow = dtMembersCommonDept.NewRow();
                                    drow["S.No"] = "Total";
                                    drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                                    LngTotVisit = 0;
                                    lib_Name = "";
                                    intSNo = 1;
                                    S = 0;
                                }
                                grdRow = dtMembersCommonDept.Rows.Count;
                                dicMembersCommonDept_Entry.Add(grdRow, "LibName");
                                drow["S.No"] = Convert.ToString(LibraryName.ToUpper());
                                dtMembersCommonDept.Rows.Add(drow);
                                lib_Name = LibraryName;
                                intSNo = 1;
                            }
                            if (LibDept_Name != DeptName)
                            {
                                if (S != 0)
                                {
                                    grdRow = dtMembersCommonDept.Rows.Count;
                                    dicMembersCommonDept_Entry.Add(grdRow, "Total");
                                    drow = dtMembersCommonDept.NewRow();
                                    drow["S.No"] = "Total";
                                    drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                                    LngTotVisit = 0;
                                    intSNo = 1;
                                    dtMembersCommonDept.Rows.Add(drow);
                                }
                                S = S + 1;
                                sno++;
                                drow = dtMembersCommonDept.NewRow();
                                drow["S.No"] = sno;
                                drow["Department"] = Convert.ToString(DeptName);
                                drow["Date"] = Convert.ToString(Entry_Date.Split(' ')[0]);
                                LibDept_Name = DeptName;
                                drow["No Of Visit"] = Convert.ToString(ds.Tables[0].Rows[i]["Tot"]);
                                dtMembersCommonDept.Rows.Add(drow);
                                LngTotVisit = LngTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                GrandTotVisit = GrandTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                intSNo = intSNo + 1;
                            }
                            else
                            {
                                drow = dtMembersCommonDept.NewRow();
                                sno++;
                                drow["S.No"] = sno;
                                drow["Date"] = Convert.ToString(Entry_Date.Split(' ')[0]);
                                drow["No Of Visit"] = Convert.ToString(ds.Tables[0].Rows[i]["Tot"]);
                                LngTotVisit = LngTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                GrandTotVisit = GrandTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                LibDept_Name = DeptName;
                                S = S + 1;
                                intSNo = intSNo + 1;
                                dtMembersCommonDept.Rows.Add(drow);
                            }
                        }
                        grdRow = dtMembersCommonDept.Rows.Count;
                        dicMembersCommonDept_Entry.Add(grdRow, "Total");
                        drow = dtMembersCommonDept.NewRow();
                        drow["S.No"] = "Total";
                        drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                        dtMembersCommonDept.Rows.Add(drow);
                    }
                    else
                    {
                    }

                }
                #endregion

                #region Visitors

                if (checkusers.Items[2].Selected)
                {
                    selectQry = "select u.Entry_Date,'' as Dept_Name,u.lib_code,lib_name,Count(*) Tot from libusers u ,library l where u.lib_code=l.lib_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Visitor'";
                    if (libraryCode != "")
                    {
                        selectQry += " and u.lib_code in('" + libraryCode + "')";
                    }
                    if (dept != "")
                    {
                        selectQry += " and u.dept_name in('" + dept + "')";
                    }
                    selectQry += " group by u.lib_code,lib_name,entry_date order by lib_name,entry_date";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectQry, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            LibraryName = Convert.ToString(ds.Tables[0].Rows[i]["lib_name"]);
                            DeptName = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);
                            Entry_Date = Convert.ToString(ds.Tables[0].Rows[i]["entry_date"]);
                            string[] ArrEntryDate = Entry_Date.Split('/');
                            Entry_Date = ArrEntryDate[1].ToString() + "/" + ArrEntryDate[0].ToString() + "/" + ArrEntryDate[2].ToString();

                            if (lib_Name != LibraryName)
                            {
                                if (grdUserReport.Rows.Count == 0)
                                {
                                    drow = dtMembersCommonDept.NewRow();
                                }
                                else
                                {
                                    grdRow = dtMembersCommonDept.Rows.Count;
                                    dicMembersCommonDept_Entry.Add(grdRow, "Total");
                                    drow = dtMembersCommonDept.NewRow();
                                    drow["S.No"] = "Total";
                                    drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                                    LngTotVisit = 0;
                                    lib_Name = "";
                                    intSNo = 1;
                                    S = 0;
                                }
                                grdRow = dtMembersCommonDept.Rows.Count;
                                dicMembersCommonDept_Entry.Add(grdRow, "LibName");
                                drow["S.No"] = Convert.ToString(LibraryName.ToUpper());
                                dtMembersCommonDept.Rows.Add(drow);
                                lib_Name = LibraryName;
                                intSNo = 1;
                            }
                            if (LibDept_Name != DeptName)
                            {
                                if (S != 0)
                                {
                                    grdRow = dtMembersCommonDept.Rows.Count;
                                    dicMembersCommonDept_Entry.Add(grdRow, "Total");
                                    drow = dtMembersCommonDept.NewRow();
                                    drow["S.No"] = "Total";
                                    drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                                    LngTotVisit = 0;
                                    intSNo = 1;
                                    dtMembersCommonDept.Rows.Add(drow);
                                }
                                S = S + 1;
                                sno++;
                                drow = dtMembersCommonDept.NewRow();
                                drow["S.No"] = sno;
                                drow["Department"] = Convert.ToString(DeptName);
                                drow["Date"] = Convert.ToString(Entry_Date.Split(' ')[0]);
                                LibDept_Name = DeptName;
                                drow["No Of Visit"] = Convert.ToString(ds.Tables[0].Rows[i]["Tot"]);
                                dtMembersCommonDept.Rows.Add(drow);
                                LngTotVisit = LngTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                GrandTotVisit = GrandTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                intSNo = intSNo + 1;
                            }
                            else
                            {
                                drow = dtMembersCommonDept.NewRow();
                                sno++;
                                drow["S.No"] = sno;
                                drow["Date"] = Convert.ToString(Entry_Date.Split(' ')[0]);
                                drow["No Of Visit"] = Convert.ToString(ds.Tables[0].Rows[i]["Tot"]);
                                LngTotVisit = LngTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                GrandTotVisit = GrandTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                LibDept_Name = DeptName;
                                S = S + 1;
                                intSNo = intSNo + 1;
                                dtMembersCommonDept.Rows.Add(drow);
                            }
                        }
                        grdRow = dtMembersCommonDept.Rows.Count;
                        dicMembersCommonDept_Entry.Add(grdRow, "Total");
                        drow = dtMembersCommonDept.NewRow();
                        drow["S.No"] = "Total";
                        drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                        dtMembersCommonDept.Rows.Add(drow);
                    }
                    else
                    {
                    }
                }
                #endregion
            }
            if (UserCount > 1)
            {
                #region Student,Staff,Visitor

                string[] User = UserCat.Split(',');
                string Memtype = string.Empty;
                string MemberVal = string.Empty;
                string UserCategory = string.Empty;

                for (int i = 0; i < User.Length; i++)
                {
                    Memtype = checkusers.Items[i].Text;
                    if (UserCategory == "")
                        UserCategory = Memtype;
                    else
                        UserCategory = UserCategory + "','" + Memtype;
                }
                CurrentSem = CurrentSem + "','0";
                if (checkusers.Items[0].Selected || checkusers.Items[1].Selected || checkusers.Items[2].Selected)
                {
                    selectQry = "select u.Entry_Date,u.Dept_Name,u.lib_code,lib_name,Count(*) Tot from libusers u ,library l where u.lib_code=l.lib_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat in('" + UserCategory + "') ";
                    if (libraryCode != "")
                    {
                        selectQry += " and u.lib_code in('" + libraryCode + "')";
                    }
                    if (dept != "")
                    {
                        selectQry += " and u.dept_name in('" + dept + "')";
                    }
                    if (sem != "")
                    {
                        selectQry += " and u.current_semester in('" + CurrentSem + "')";
                    }
                    selectQry += " group by u.lib_code,lib_name,u.dept_name,entry_date order by lib_name,u.dept_name,entry_date";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectQry, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            LibraryName = Convert.ToString(ds.Tables[0].Rows[i]["lib_name"]);
                            DeptName = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);
                            Entry_Date = Convert.ToString(ds.Tables[0].Rows[i]["entry_date"]);
                            string[] ArrEntryDate = Entry_Date.Split('/');
                            Entry_Date = ArrEntryDate[1].ToString() + "/" + ArrEntryDate[0].ToString() + "/" + ArrEntryDate[2].ToString();

                            if (lib_Name != LibraryName)
                            {
                                if (grdUserReport.Rows.Count == 0)
                                {
                                    drow = dtMembersCommonDept.NewRow();
                                }
                                else
                                {
                                    grdRow = dtMembersCommonDept.Rows.Count;
                                    dicMembersCommonDept_Entry.Add(grdRow - 1, "Total");
                                    drow = dtMembersCommonDept.NewRow();
                                    drow["S.No"] = "Total";
                                    drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                                    LngTotVisit = 0;
                                    lib_Name = "";
                                    intSNo = 1;
                                    S = 0;
                                }
                                grdRow = dtMembersCommonDept.Rows.Count;
                                dicMembersCommonDept_Entry.Add(grdRow - 1, "LibName");
                                drow["S.No"] = Convert.ToString(LibraryName.ToUpper());
                                dtMembersCommonDept.Rows.Add(drow);
                                lib_Name = LibraryName;
                                intSNo = 1;
                            }
                            if (LibDept_Name != DeptName)
                            {
                                if (S != 0)
                                {
                                    grdRow = dtMembersCommonDept.Rows.Count;
                                    dicMembersCommonDept_Entry.Add(grdRow - 1, "Total");
                                    drow = dtMembersCommonDept.NewRow();
                                    drow["S.No"] = "Total";
                                    drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                                    LngTotVisit = 0;
                                    intSNo = 1;
                                    dtMembersCommonDept.Rows.Add(drow);
                                }
                                S = S + 1;
                                sno++;
                                drow = dtMembersCommonDept.NewRow();
                                drow["S.No"] = sno;
                                drow["Department"] = Convert.ToString(DeptName);
                                drow["Date"] = Convert.ToString(Entry_Date.Split(' ')[0]);
                                LibDept_Name = DeptName;
                                drow["No Of Visit"] = Convert.ToString(ds.Tables[0].Rows[i]["Tot"]);
                                dtMembersCommonDept.Rows.Add(drow);
                                LngTotVisit = LngTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                GrandTotVisit = GrandTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                intSNo = intSNo + 1;
                            }
                            else
                            {
                                drow = dtMembersCommonDept.NewRow();
                                drow["S.No"] = sno;
                                drow["Date"] = Convert.ToString(Entry_Date.Split(' ')[0]);
                                drow["No Of Visit"] = Convert.ToString(ds.Tables[0].Rows[i]["Tot"]);
                                LngTotVisit = LngTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                GrandTotVisit = GrandTotVisit + Convert.ToDouble(ds.Tables[0].Rows[i]["Tot"]);
                                LibDept_Name = DeptName;
                                S = S + 1;
                                intSNo = intSNo + 1;
                                dtMembersCommonDept.Rows.Add(drow);
                            }
                        }
                        grdRow = dtMembersCommonDept.Rows.Count;
                        dicMembersCommonDept_Entry.Add(grdRow, "Total");
                        drow = dtMembersCommonDept.NewRow();
                        drow["S.No"] = "Total";
                        drow["No Of Visit"] = Convert.ToString(LngTotVisit);
                        dtMembersCommonDept.Rows.Add(drow);
                    }
                    else
                    {
                    }
                }
                #endregion
            }
            drow = dtMembersCommonDept.NewRow();
            grdRow = dtMembersCommonDept.Rows.Count;
            dicMembersCommonDept_Entry.Add(grdRow, "GrandTotal");
            drow["S.No"] = "Grand Total";
            drow["No Of Visit"] = Convert.ToString(GrandTotVisit);
            dtMembersCommonDept.Rows.Add(drow);


            grdUserReport.DataSource = dtMembersCommonDept;
            grdUserReport.DataBind();
            grdUserReport.Visible = true;
            divSpreadReport.Visible = true;
            grdUserReport.Rows[0].BackColor = Color.FromArgb(12, 166, 202);
            grdUserReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;
            grdUserReport.Rows[0].Font.Bold = true;
            print.Visible = true;
            print1.Visible = false;
            print2.Visible = false;
            foreach (KeyValuePair<int, string> dr in dicMembersCommonDept_Entry)
            {
                int g = dr.Key;
                string DicValue = dr.Value;
                if (DicValue == "LibName")
                {
                    grdUserReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    grdUserReport.Rows[g].Cells[0].ColumnSpan = 4;
                    for (int a = 1; a < 4; a++)
                        grdUserReport.Rows[g].Cells[a].Visible = false;
                    grdUserReport.Rows[g].BackColor = Color.LightCoral;
                }
                else
                {
                    grdUserReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                    grdUserReport.Rows[g].Cells[0].ColumnSpan = 3;
                    for (int a = 1; a < 3; a++)
                        grdUserReport.Rows[g].Cells[a].Visible = false;
                    if (DicValue == "Total")
                        grdUserReport.Rows[g].BackColor = Color.Green;
                    if (DicValue == "GrandTotal")
                        grdUserReport.Rows[g].BackColor = Color.YellowGreen;
                }
            }
            dicMembersCommonDept_Entry.Clear();

            #endregion
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "UserInOutEntry");
        }
    }

    protected void MembersIndDeptWise_Entry(object sender, EventArgs e)
    {
        try
        {
            int gvHasRows = grdUserReport.Rows.Count;
            if (gvHasRows > 0)
            {
                grdUserReport.Columns.Clear();
                grdUserReport.DataBind();
            }
            divVisit_Details.Visible = false;

            string college = Convert.ToString(ddl_collegename.SelectedValue);
            string libraryCode = getCblSelectedValue(cbl_library);
            string dept = getCblSelectedValue(cbl_dept);
            string sem = getCblSelectedText(cbl_sem);
            string UserCat = getCblSelectedText(checkusers);
            string StrDate = string.Empty;
            string StrTime = string.Empty;
            string selectQry = string.Empty;
            int spreadMax = 0;
            string Entry_Date = string.Empty;
            string LibraryName = string.Empty;
            string lib_Name = string.Empty;
            string DeptName = string.Empty;
            string LibDept_Name = string.Empty;
            string usercat = string.Empty;
            int sno = 0;
            int intTotCount = 0;
            int IntTotStud = 0;
            int IntTotStaff = 0;
            int IntTotVisitor = 0;
            int Total = 0;
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            string[] Semster = sem.Split(new string[] { "','" }, StringSplitOptions.None);
            string SemVal = string.Empty;
            string CurrentSem = "";
            int S = 0;
            int gdrow = 0;
            Dictionary<int, string> dicIndDept_Entry = new Dictionary<int, string>();
            for (int i = 0; i < Semster.Length; i++)
            {
                SemVal = Semster[i];
                string SemCode = SemVal.Split(' ')[0];
                if (!CurrentSem.Contains(SemCode))
                {
                    if (CurrentSem == "")
                        CurrentSem = SemCode;
                    else
                        CurrentSem = CurrentSem + "','" + SemCode;
                }
            }
            DateTime F_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector1.Hour, TimeSelector1.Minute, TimeSelector1.Second, TimeSelector1.AmPm));
            DateTime T_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector2.Hour, TimeSelector2.Minute, TimeSelector2.Second, TimeSelector2.AmPm));

            DataTable dtMembersIndDeptWise = new DataTable();
            DataRow drow;
            dtMembersIndDeptWise.Columns.Add("S.No", typeof(string));
            dtMembersIndDeptWise.Columns.Add("Department", typeof(string));


            if (cbdate.Checked)
                StrDate = " and entry_date between '" + fromdate + "' and '" + todate + "'";
            else
                StrDate = "";
            if (cbtime.Checked)
                StrTime = " AND ((CONVERT(datetime,Entry_Time,108) BETWEEN '" + F_time + "' AND '" + T_time + "' ) OR (CONVERT(datetime,Exit_Time,108) BETWEEN '" + F_time + "' AND '" + T_time + "' ))";
            else
                StrTime = "";

            selectQry = "SELECT DISTINCT Dept_Name FROM LibUsers WHERE 1=1 ";
            if (libraryCode != "")
            {
                selectQry += " and lib_code in('" + libraryCode + "')";
            }
            if (dept != "")
            {
                selectQry += " and dept_name in('" + dept + "')";
            }
            if (UserCat != "")
            {
                selectQry += " AND UserCat IN ('" + UserCat + "')";
            }
            selectQry += " ORDER BY Dept_Name ";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(selectQry, "text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsload.Tables[0].Rows.Count; i++)
                {
                    intTotCount = 0;
                    sno++;
                    DeptName = Convert.ToString(dsload.Tables[0].Rows[i]["dept_name"]);
                    drow = dtMembersIndDeptWise.NewRow();
                    drow["S.No"] = sno;
                    drow["Department"] = Convert.ToString(DeptName);

                    //Student
                    if (UserCat == "Student" || UserCat == "Student','Staff" || UserCat == "Student','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Student' AND Dept_Name ='" + DeptName + "' " + StrDate + " " + StrTime + " ";
                        if (libraryCode != "")
                        {
                            selectQry += " and lib_code in('" + libraryCode + "')";
                        }
                        if (sem != "")
                        {
                            selectQry += " and current_semester in('" + CurrentSem + "')";
                        }
                        selectQry += "GROUP BY Dept_Name ";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectQry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            drow["Student"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                            intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            IntTotStud = IntTotStud + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                        }
                        else
                        {
                            drow["Student"] = "0";
                        }
                    }

                    //Staff
                    if (UserCat == "Staff" || UserCat == "Student','Staff" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Staff' AND Dept_Name ='" + DeptName + "' " + StrDate + " " + StrTime + " ";
                        if (libraryCode != "")
                        {
                            selectQry += " and lib_code in('" + libraryCode + "')";
                        }
                        selectQry += "GROUP BY Dept_Name ";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectQry, "text");

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            drow["Staff"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                            intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            IntTotStaff = IntTotStaff + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                        }
                        else
                        {
                            drow["Staff"] = "0";
                        }
                    }
                    //visitors
                    if (UserCat == "Visitor" || UserCat == "Student','Visitor" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Visitor' AND Dept_Name ='" + DeptName + "' " + StrDate + " " + StrTime + " ";
                        if (libraryCode != "")
                        {
                            selectQry += " and lib_code in('" + libraryCode + "')";
                        }
                        selectQry += "GROUP BY Dept_Name ";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectQry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            drow["Visitor"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                            intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            IntTotVisitor = IntTotVisitor + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                        }
                        else
                        {
                            drow["Visitor"] = "0";
                        }
                    }
                    drow["Total"] = Convert.ToString(intTotCount);
                    dtMembersIndDeptWise.Rows.Add(drow);
                }
                gdrow = dtMembersIndDeptWise.Rows.Count;
                dicIndDept_Entry.Add(gdrow, "Total");
                drow = dtMembersIndDeptWise.NewRow();
                drow["S.No"] = "Total";
                if (UserCat == "Student" || UserCat == "Student','Staff" || UserCat == "Student','Visitor" || UserCat == "Student','Staff','Visitor")
                {
                    drow["Student"] = Convert.ToString(IntTotStud);
                }
                if (UserCat == "Staff" || UserCat == "Student','Staff" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                {
                    drow["Staff"] = Convert.ToString(IntTotStaff);
                }
                if (UserCat == "Visitor" || UserCat == "Student','Visitor" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                {
                    drow["Visitor"] = Convert.ToString(IntTotVisitor);
                }
                Total = IntTotStud + IntTotStaff + IntTotVisitor;
                drow["Total"] = Total;
                dtMembersIndDeptWise.Rows.Add(drow);
                grdUserReport.DataSource = dtMembersIndDeptWise;
                grdUserReport.DataBind();
                grdUserReport.Visible = true;
                divSpreadReport.Visible = true;
                grdUserReport.Rows[0].BackColor = Color.FromArgb(12, 166, 202);
                grdUserReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                grdUserReport.Rows[0].Font.Bold = true;
                print.Visible = true;
                print1.Visible = false;
                print2.Visible = false;
                foreach (KeyValuePair<int, string> dr in dicIndDept_Entry)
                {
                    int g = dr.Key;
                    grdUserReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                    grdUserReport.Rows[g].Cells[0].ColumnSpan = 2;
                    for (int a = 1; a < 2; a++)
                        grdUserReport.Rows[g].Cells[a].Visible = false;
                    grdUserReport.Rows[g].BackColor = Color.Green;
                }
                dicIndDept_Entry.Clear();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "No Record found";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "UserInOutEntry");
        }
    }

    protected void Visitor_Entry_Statistics(object sender, EventArgs e)
    {
        try
        {
            int gvHasRows = grdUserReport.Rows.Count;
            if (gvHasRows > 0)
            {
                grdUserReport.Columns.Clear();
                grdUserReport.DataBind();
            }
            divSpreadVisitWithTime.Visible = false;
            divVisit_Details.Visible = false;
            if (cbdate.Checked == false)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Select the Date";
                return;
            }
            string college = Convert.ToString(ddl_collegename.SelectedValue);
            string libraryCode = getCblSelectedValue(cbl_library);
            string dept = getCblSelectedValue(cbl_dept);
            string sem = getCblSelectedText(cbl_sem);
            string UserCat = getCblSelectedText(checkusers);
            string StrDate = string.Empty;
            string StrTime = string.Empty;
            string selectQry = string.Empty;
            string entryDate = string.Empty;
            string Lib_EntryDate = string.Empty;
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[2].ToString() + "/" + frdate[1].ToString() + "/" + frdate[0].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[2].ToString() + "/" + tdate[1].ToString() + "/" + tdate[0].ToString();
            string[] Semster = sem.Split(new string[] { "','" }, StringSplitOptions.None);
            string SemVal = string.Empty;
            string CurrentSem = "";
            int intTotCount = 0;
            int sno = 0;
            int IntTotStud = 0;
            int IntTotStaff = 0;
            int IntTotVisitor = 0;
            int IntTotOpac = 0;
            int Total = 0;
            int gdrow = 0;
            Dictionary<int, string> dicVisitor_Entry_Statistics = new Dictionary<int, string>();

            DataTable dtVisitor_Entry_Statistics = new DataTable();
            DataRow drow;
            dtVisitor_Entry_Statistics.Columns.Add("S.No", typeof(string));
            if (rbdaily.Checked)
            {
                dtVisitor_Entry_Statistics.Columns.Add("Date", typeof(string));
            }
            if (rbweekly.Checked)
            {
                dtVisitor_Entry_Statistics.Columns.Add("Week", typeof(string));
            }
            if (rbmonthly.Checked)
            {
                dtVisitor_Entry_Statistics.Columns.Add("Month", typeof(string));
            }
            if (rbyearly.Checked)
            {
                dtVisitor_Entry_Statistics.Columns.Add("Year", typeof(string));
            }
            if (UserCat == "Student" || UserCat == "Student','Staff" || UserCat == "Student','Visitor" || UserCat == "Student','Staff','Visitor")
            {
                dtVisitor_Entry_Statistics.Columns.Add("Student", typeof(string));
            }
            if (UserCat == "Staff" || UserCat == "Student','Staff" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
            {
                dtVisitor_Entry_Statistics.Columns.Add("Staff", typeof(string));
            }
            if (UserCat == "Visitor" || UserCat == "Student','Visitor" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
            {
                dtVisitor_Entry_Statistics.Columns.Add("Visitor", typeof(string));
            }
            if (rbmonthly.Checked)
            {
                dtVisitor_Entry_Statistics.Columns.Add("Opac", typeof(string));
            }
            dtVisitor_Entry_Statistics.Columns.Add("Total", typeof(string));

            for (int i = 0; i < Semster.Length; i++)
            {
                SemVal = Semster[i];
                string SemCode = SemVal.Split(' ')[0];
                if (!CurrentSem.Contains(SemCode))
                {
                    if (CurrentSem == "")
                        CurrentSem = SemCode;
                    else
                        CurrentSem = CurrentSem + "','" + SemCode;
                }
            }
            DateTime F_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector1.Hour, TimeSelector1.Minute, TimeSelector1.Second, TimeSelector1.AmPm));
            DateTime T_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector2.Hour, TimeSelector2.Minute, TimeSelector2.Second, TimeSelector2.AmPm));

            if (cbdate.Checked)
                StrDate = " and entry_date between '" + fromdate + "' and '" + todate + "'";
            else
                StrDate = "";
            if (cbtime.Checked)
                StrTime = " AND ((CONVERT(datetime,Entry_Time,108) BETWEEN '" + F_time + "' AND '" + T_time + "' ) OR (CONVERT(datetime,Exit_Time,108) BETWEEN '" + F_time + "' AND '" + T_time + "' ))";
            else
                StrTime = "";

            #region Daily

            if (rbdaily.Checked)
            {
                selectQry = "SELECT distinct Entry_Date FROM LibUsers WHERE UserCat in('" + UserCat + "') " + StrDate + " " + StrTime + "";
                if (libraryCode != "")
                {
                    selectQry += " and lib_code in('" + libraryCode + "')";
                }
                if (sem != "")
                {
                    selectQry += " and current_semester in('" + CurrentSem + "','0" + "')";
                }
                selectQry += "GROUP BY Entry_Date ";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selectQry, "text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsload.Tables[0].Rows.Count; i++)
                    {
                        intTotCount = 0;
                        entryDate = Convert.ToString(dsload.Tables[0].Rows[i]["Entry_Date"]);
                        string[] En_date = entryDate.Split('/');
                        if (En_date.Length == 3)
                            Lib_EntryDate = En_date[1].ToString() + "/" + En_date[0].ToString() + "/" + En_date[2].ToString();
                        sno++;
                        drow = dtVisitor_Entry_Statistics.NewRow();
                        drow["S.No"] = sno;
                        drow["Date"] = Convert.ToString(Lib_EntryDate.Split(' ')[0]);
                        //Student
                        if (UserCat == "Student" || UserCat == "Student','Staff" || UserCat == "Student','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Student' AND Entry_Date ='" + entryDate + "' " + StrTime + "";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            if (sem != "")
                            {
                                selectQry += " and current_semester in('" + CurrentSem + "')";
                            }
                            selectQry += "GROUP BY Entry_Date ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Student"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotStud = IntTotStud + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Student"] = "0";
                            }
                        }

                        //Staff
                        if (UserCat == "Staff" || UserCat == "Student','Staff" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Staff' AND Entry_Date ='" + entryDate + "' " + StrTime + "";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }

                            selectQry += "GROUP BY Entry_Date ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Staff"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotStaff = IntTotStaff + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Staff"] = "0";
                            }
                        }
                        //'visitors
                        if (UserCat == "Visitor" || UserCat == "Student','Visitor" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Visitor' AND Entry_Date ='" + entryDate + "' " + StrTime + " ";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            selectQry += "GROUP BY Dept_Name ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Visitor"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotVisitor = IntTotVisitor + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Visitor"] = "0";
                            }
                        }
                        drow["Total"] = Convert.ToString(intTotCount);
                        dtVisitor_Entry_Statistics.Rows.Add(drow);
                    }
                    gdrow = dtVisitor_Entry_Statistics.Rows.Count;
                    dicVisitor_Entry_Statistics.Add(gdrow, "Total");
                    drow = dtVisitor_Entry_Statistics.NewRow();
                    drow["S.No"] = "Total";
                    if (UserCat == "Student" || UserCat == "Student','Staff" || UserCat == "Student','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Student"] = Convert.ToString(IntTotStud);
                    }
                    if (UserCat == "Staff" || UserCat == "Student','Staff" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Staff"] = Convert.ToString(IntTotStaff);
                    }
                    if (UserCat == "Visitor" || UserCat == "Student','Visitor" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Visitor"] = Convert.ToString(IntTotVisitor);
                    }
                    Total = IntTotStud + IntTotStaff + IntTotVisitor;
                    drow["Total"] = Convert.ToString(Total);
                    dtVisitor_Entry_Statistics.Rows.Add(drow);
                    grdUserReport.DataSource = dtVisitor_Entry_Statistics;
                    grdUserReport.DataBind();
                    grdUserReport.Visible = true;
                    divSpreadReport.Visible = true;
                    //grdUserReport.Rows[0].BackColor = Color.FromArgb(12, 166, 202);
                    //grdUserReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                    //grdUserReport.Rows[0].Font.Bold = true;
                    print.Visible = true;
                    print1.Visible = false;
                    print2.Visible = false;
                    foreach (KeyValuePair<int, string> dr in dicVisitor_Entry_Statistics)
                    {
                        int g = dr.Key;
                        grdUserReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        grdUserReport.Rows[g].Cells[0].ColumnSpan = 2;
                        for (int a = 1; a < 2; a++)
                            grdUserReport.Rows[g].Cells[a].Visible = false;
                        grdUserReport.Rows[g].BackColor = Color.Green;
                        grdUserReport.Rows[g].Font.Bold = true;
                    }
                    dicVisitor_Entry_Statistics.Clear();
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "No Record found";
                }
            }
            #endregion

            #region Weekly

            if (rbweekly.Checked)
            {
                string month = string.Empty;
                selectQry = "SELECT datename(wk,entry_date) montno FROM LibUsers where 1=1 " + StrDate + " ";
                selectQry += " GROUP BY datename(wk,entry_date ) order by cast(datename(wk,entry_date) as numeric)";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selectQry, "text");

                if (dsload.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsload.Tables[0].Rows.Count; i++)
                    {
                        intTotCount = 0;
                        month = Convert.ToString(dsload.Tables[0].Rows[i]["montno"]);
                        sno++;
                        drow = dtVisitor_Entry_Statistics.NewRow();
                        drow["S.No"] = sno;
                        drow["Week"] = Convert.ToString(month);
                        //Student
                        if (UserCat == "Student" || UserCat == "Student','Staff" || UserCat == "Student','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Student' AND datename(wk,entry_date) ='" + month + "' " + StrDate + " " + StrTime + "";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            if (sem != "")
                            {
                                selectQry += " and current_semester in('" + CurrentSem + "')";
                            }
                            selectQry += " GROUP BY datename(wk,entry_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Student"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotStud = IntTotStud + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Student"] = "0";
                            }
                        }
                        //Staff
                        if (UserCat == "Staff" || UserCat == "Student','Staff" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Staff' AND datename(wk,entry_date) ='" + month + "' " + StrDate + " " + StrTime + "";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            selectQry += " GROUP BY datename(wk,entry_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Staff"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotStaff = IntTotStaff + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Staff"] = "0";
                            }
                        }
                        //'visitors
                        if (UserCat == "Visitor" || UserCat == "Student','Visitor" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Visitor' AND datename(wk,entry_date) ='" + month + "' " + StrDate + " " + StrTime + " ";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            selectQry += " GROUP BY datename(wk,entry_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Visitor"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotVisitor = IntTotVisitor + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Visitor"] = "0";
                            }
                        }
                        drow["Total"] = Convert.ToString(intTotCount);
                        dtVisitor_Entry_Statistics.Rows.Add(drow);
                    }
                    gdrow = dtVisitor_Entry_Statistics.Rows.Count;
                    dicVisitor_Entry_Statistics.Add(gdrow, "Total");
                    drow = dtVisitor_Entry_Statistics.NewRow();
                    drow["S.No"] = "Total";
                    if (UserCat == "Student" || UserCat == "Student','Staff" || UserCat == "Student','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Student"] = Convert.ToString(IntTotStud);
                    }
                    if (UserCat == "Staff" || UserCat == "Student','Staff" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Staff"] = Convert.ToString(IntTotStaff);
                    }
                    if (UserCat == "Visitor" || UserCat == "Student','Visitor" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Visitor"] = Convert.ToString(IntTotVisitor);
                    }
                    Total = IntTotStud + IntTotStaff + IntTotVisitor;
                    drow["Total"] = Convert.ToString(Total);
                    dtVisitor_Entry_Statistics.Rows.Add(drow);
                    grdUserReport.DataSource = dtVisitor_Entry_Statistics;
                    grdUserReport.DataBind();
                    grdUserReport.Visible = true;
                    divSpreadReport.Visible = true;

                    print.Visible = true;
                    print1.Visible = false;
                    print2.Visible = false;
                    foreach (KeyValuePair<int, string> dr in dicVisitor_Entry_Statistics)
                    {
                        int g = dr.Key;
                        grdUserReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        grdUserReport.Rows[g].Cells[0].ColumnSpan = 2;
                        for (int a = 1; a < 2; a++)
                            grdUserReport.Rows[g].Cells[a].Visible = false;
                        grdUserReport.Rows[g].BackColor = Color.Green;
                        grdUserReport.Rows[g].Font.Bold = true;
                    }
                    dicVisitor_Entry_Statistics.Clear();
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "No Record found";
                }
            }

            #endregion

            #region Monthly

            if (rbmonthly.Checked)
            {
                string monthVal = string.Empty;
                string monthName = string.Empty;
                string monthYear = string.Empty;
                selectQry = "SELECT datename(mm,entry_date) montname,month(entry_date) montno,datename(yyyy,entry_date) Year FROM LibUsers where 1=1 " + StrDate + " ";

                selectQry += " GROUP BY datename(yyyy,entry_date),datename(mm,entry_date ), month(entry_date) order by datename(yyyy,entry_date),month(entry_date) ";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selectQry, "text");

                if (dsload.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsload.Tables[0].Rows.Count; i++)
                    {
                        intTotCount = 0;
                        monthVal = Convert.ToString(dsload.Tables[0].Rows[i]["montno"]);
                        monthName = Convert.ToString(dsload.Tables[0].Rows[i]["montname"]);
                        monthYear = Convert.ToString(dsload.Tables[0].Rows[i]["Year"]);
                        sno++;
                        drow = dtVisitor_Entry_Statistics.NewRow();
                        drow["S.No"] = sno;
                        drow["Month"] = Convert.ToString(monthName);

                        //Student
                        if (UserCat == "Student" || UserCat == "Student','Staff" || UserCat == "Student','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Student' AND month(Entry_Date) ='" + monthVal + "' and year(Entry_Date) ='" + monthYear + "' " + StrDate + " " + StrTime + "";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            if (sem != "")
                            {
                                selectQry += " and current_semester in('" + CurrentSem + "')";
                            }
                            selectQry += " GROUP BY datename(mm,entry_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Student"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotStud = IntTotStud + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Student"] = "0";
                            }
                        }
                        //Staff
                        if (UserCat == "Staff" || UserCat == "Student','Staff" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Staff' AND month(Entry_Date) ='" + monthVal + "' and year(Entry_Date) ='" + monthYear + "' " + StrDate + " " + StrTime + "";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            selectQry += " GROUP BY datename(mm,entry_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Staff"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotStaff = IntTotStaff + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Staff"] = "0";
                            }
                        }
                        //'visitors
                        if (UserCat == "Visitor" || UserCat == "Student','Visitor" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Visitor' AND month(Entry_Date) ='" + monthVal + "' and year(Entry_Date) ='" + monthYear + "' " + StrDate + " " + StrTime + " ";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            selectQry += " GROUP BY datename(mm,entry_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Visitor"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotVisitor = IntTotVisitor + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Visitor"] = "0";
                            }
                        }
                        //OPAC
                        selectQry = "select  * from lib_queryhit WHERE month(lib_date) ='" + monthVal + "' and year(lib_date) ='" + monthYear + "'  and lib_date between '" + fromdate + "' and '" + todate + "' ";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectQry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            double opacVal = 0;
                            double OPacStat = 0;
                            for (int opac = 0; opac < ds.Tables[0].Rows.Count; opac++)
                            {
                                opacVal = Convert.ToDouble(ds.Tables[0].Rows[opac]["lib_count"]);
                                OPacStat = OPacStat + opacVal;
                            }
                            drow["Opac"] = OPacStat;
                            intTotCount = intTotCount + Convert.ToInt32(OPacStat);
                            IntTotOpac = IntTotOpac + Convert.ToInt32(OPacStat);
                        }
                        else
                        {
                            drow["Opac"] = "0";
                        }
                        drow["Total"] = Convert.ToString(intTotCount);
                        dtVisitor_Entry_Statistics.Rows.Add(drow);
                    }
                    drow = dtVisitor_Entry_Statistics.NewRow();
                    drow["S.No"] = "Total";
                    if (UserCat == "Student" || UserCat == "Student','Staff" || UserCat == "Student','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Student"] = Convert.ToString(IntTotStud);
                    }
                    if (UserCat == "Staff" || UserCat == "Student','Staff" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Staff"] = Convert.ToString(IntTotStaff);
                    }
                    if (UserCat == "Visitor" || UserCat == "Student','Visitor" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Visitor"] = Convert.ToString(IntTotVisitor);
                    }
                    drow["Opac"] = IntTotOpac;
                    gdrow = dtVisitor_Entry_Statistics.Rows.Count;
                    dicVisitor_Entry_Statistics.Add(gdrow, "Total");
                    Total = IntTotStud + IntTotStaff + IntTotVisitor + IntTotOpac;
                    drow["Total"] = Convert.ToString(Total);
                    dtVisitor_Entry_Statistics.Rows.Add(drow);
                    grdUserReport.DataSource = dtVisitor_Entry_Statistics;
                    grdUserReport.DataBind();
                    grdUserReport.Visible = true;
                    divSpreadReport.Visible = true;

                    print.Visible = true;
                    print1.Visible = false;
                    print2.Visible = false;
                    foreach (KeyValuePair<int, string> dr in dicVisitor_Entry_Statistics)
                    {
                        int g = dr.Key;
                        grdUserReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        grdUserReport.Rows[g].Cells[0].ColumnSpan = 2;
                        for (int a = 1; a < 2; a++)
                            grdUserReport.Rows[g].Cells[a].Visible = false;
                        grdUserReport.Rows[g].BackColor = Color.Green;
                        grdUserReport.Rows[g].Font.Bold = true;
                    }
                    dicVisitor_Entry_Statistics.Clear();

                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "No Record found";
                }
            }
            #endregion

            #region Yearly

            if (rbyearly.Checked)
            {
                string Year = string.Empty;
                selectQry = "SELECT datename(yyyy,entry_date) Year FROM LibUsers where 1=1 " + StrDate + " ";

                selectQry += "   GROUP BY datename(yyyy,entry_date ) order by datename(yyyy,entry_date)";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selectQry, "text");

                if (dsload.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsload.Tables[0].Rows.Count; i++)
                    {
                        intTotCount = 0;
                        Year = Convert.ToString(dsload.Tables[0].Rows[i]["Year"]);
                        sno++;
                        drow = dtVisitor_Entry_Statistics.NewRow();
                        drow["S.No"] = sno;
                        drow["Year"] = Convert.ToString(Year);
                        //Student
                        if (UserCat == "Student" || UserCat == "Student','Staff" || UserCat == "Student','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Student' AND year(Entry_Date) ='" + Year + "' " + StrDate + " " + StrTime + "";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            if (sem != "")
                            {
                                selectQry += " and current_semester in('" + CurrentSem + "')";
                            }
                            selectQry += " GROUP BY datename(yy,entry_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Student"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotStud = IntTotStud + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Student"] = "0";
                            }
                        }
                        //Staff
                        if (UserCat == "Staff" || UserCat == "Student','Staff" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Staff' AND year(Entry_Date) ='" + Year + "' " + StrDate + " " + StrTime + "";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            selectQry += " GROUP BY datename(yy,entry_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Staff"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotStaff = IntTotStaff + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Staff"] = "0";
                            }
                        }
                        //'visitors
                        if (UserCat == "Visitor" || UserCat == "Student','Visitor" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Visitor' AND year(Entry_Date) ='" + Year + "' " + StrDate + " " + StrTime + " ";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            selectQry += " GROUP BY datename(yy,entry_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Visitor"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotVisitor = IntTotVisitor + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Visitor"] = "0";
                            }
                        }
                        drow["Total"] = Convert.ToString(intTotCount);
                        dtVisitor_Entry_Statistics.Rows.Add(drow);
                    }
                    gdrow = dtVisitor_Entry_Statistics.Rows.Count;
                    dicVisitor_Entry_Statistics.Add(gdrow, "Total");
                    drow = dtVisitor_Entry_Statistics.NewRow();
                    drow["S.No"] = "Total";
                    if (UserCat == "Student" || UserCat == "Student','Staff" || UserCat == "Student','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Student"] = Convert.ToString(IntTotStud);
                    }
                    if (UserCat == "Staff" || UserCat == "Student','Staff" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Staff"] = Convert.ToString(IntTotStaff);
                    }
                    if (UserCat == "Visitor" || UserCat == "Student','Visitor" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Visitor"] = Convert.ToString(IntTotVisitor);
                    }
                    Total = IntTotStud + IntTotStaff + IntTotVisitor;
                    drow["Total"] = Convert.ToString(Total);
                    dtVisitor_Entry_Statistics.Rows.Add(drow);
                    grdUserReport.DataSource = dtVisitor_Entry_Statistics;
                    grdUserReport.DataBind();
                    grdUserReport.Visible = true;
                    divSpreadReport.Visible = true;

                    print.Visible = true;
                    print1.Visible = false;
                    print2.Visible = false;
                    foreach (KeyValuePair<int, string> dr in dicVisitor_Entry_Statistics)
                    {
                        int g = dr.Key;
                        grdUserReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        grdUserReport.Rows[g].Cells[0].ColumnSpan = 2;
                        for (int a = 1; a < 2; a++)
                            grdUserReport.Rows[g].Cells[a].Visible = false;
                        grdUserReport.Rows[g].BackColor = Color.Green;
                        grdUserReport.Rows[g].Font.Bold = true;
                    }
                    dicVisitor_Entry_Statistics.Clear();
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "No Record found";
                }
            }
            #endregion

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "UserInOutEntry");
        }
    }

    protected void Visit_Details(object sender, EventArgs e)
    {
        try
        {
            divSpreadReport.Visible = false;
            divSpreadVisitWithTime.Visible = false;
            DataTable dtVisit_Details = new DataTable();
            DataRow drow;

            dtVisit_Details.Columns.Add("SNo", typeof(string));
            dtVisit_Details.Columns.Add("Type", typeof(string));
            dtVisit_Details.Columns.Add("Roll No", typeof(string));
            dtVisit_Details.Columns.Add("Register No", typeof(string));
            dtVisit_Details.Columns.Add("Name", typeof(string));
            dtVisit_Details.Columns.Add("Department", typeof(string));
            dtVisit_Details.Columns.Add("Entry Date", typeof(string));
            dtVisit_Details.Columns.Add("Entry Time", typeof(string));
            dtVisit_Details.Columns.Add("Exit Time", typeof(string));
            dtVisit_Details.Columns.Add("Duration", typeof(string));

            #region Query

            string college = Convert.ToString(ddl_collegename.SelectedValue);
            string libraryCode = getCblSelectedValue(cbl_library);
            string dept = getCblSelectedText(cbl_dept);
            string sem = getCblSelectedText(cbl_sem);
            string UserCat = getCblSelectedText(checkusers);
            string StrDate = string.Empty;
            string StrTime = string.Empty;
            string selectQry = string.Empty;
            //string selectQry = string.Empty;
            string entryDate = string.Empty;
            string Lib_EntryDate = string.Empty;
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[2].ToString() + "/" + frdate[1].ToString() + "/" + frdate[0].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[2].ToString() + "/" + tdate[1].ToString() + "/" + tdate[0].ToString();
            string[] Semster = sem.Split(new string[] { "','" }, StringSplitOptions.None);
            string SemVal = string.Empty;
            string CurrentSem = "";
            int sno = 0;
            int TotStudCount = 0;
            int TotStaffCount = 0;
            int TotVisitorCount = 0;
            int TotCount = 0;

            for (int i = 0; i < Semster.Length; i++)
            {
                SemVal = Semster[i];
                string SemCode = SemVal.Split(' ')[0];
                if (!CurrentSem.Contains(SemCode))
                {
                    if (CurrentSem == "")
                        CurrentSem = SemCode;
                    else
                        CurrentSem = CurrentSem + "','" + SemCode;
                }
            }
            DateTime F_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector1.Hour, TimeSelector1.Minute, TimeSelector1.Second, TimeSelector1.AmPm));
            string FromTime = Convert.ToString(F_time);
            string FinalFromTime = FromTime.Split(' ')[1] + FromTime.Split(' ')[2];
            DateTime T_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector2.Hour, TimeSelector2.Minute, TimeSelector2.Second, TimeSelector2.AmPm));
            string ToTime = Convert.ToString(T_time);
            string FinalToTime = ToTime.Split(' ')[1] + ToTime.Split(' ')[2];
            if (cbdate.Checked)
                StrDate = " and entry_date between '" + fromdate + "' and '" + todate + "'";
            else
                StrDate = "";
            if (cbtime.Checked)
                StrTime = " AND ((CONVERT(datetime,Entry_Time,108) BETWEEN '" + FinalFromTime + "' AND '" + FinalToTime + "' ) OR (CONVERT(datetime,Exit_Time,108) BETWEEN '" + FinalFromTime + "' AND '" + FinalToTime + "' ))";
            else
                StrTime = "";
            string rollNo = txtroll.Text;
            string Name = Nametxt.Text;

            // student
            if (UserCat == "Student")
            {
                selectQry = "select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number',Reg_No as 'Register No',u.stud_name as 'Name',d.dept_name as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,registration r,degree g,course c,department d where u.lib_code=l.lib_code and (r.roll_no = u.roll_no or r.lib_id = u.roll_no) and r.roll_no = u.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = d.dept_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Student'";

                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                if (sem != "")
                {
                    selectQry += " and u.current_semester in('" + CurrentSem + "','0" + "')";
                }
                if (rollNo != "")
                {
                    selectQry += " and r.roll_no ='" + rollNo + "' ";
                }
                if (Name != "")
                {
                    selectQry += " and r.stud_name like '%" + Name + "%' ";
                }
                //======added for non member=============//
                selectQry += " UNION ALL select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name',n.department as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,user_master n where u.lib_code=l.lib_code and n.user_id = u.roll_no and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Student'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                selectQry += " order by entry_date ,convert(nvarchar(10),entry_time,108) desc";
                //==============================//
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selectQry, "text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    TotStudCount = Convert.ToInt32(dsload.Tables[0].Rows.Count);
                    TotCount = TotCount + Convert.ToInt32(dsload.Tables[0].Rows.Count);
                }
                else
                {
                    TotStudCount = 0;
                }
            }
            //staff
            if (UserCat == "Staff")
            {
                selectQry = "select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Staff Code','' as 'Register No',u.stud_name as 'Name',d.dept_name as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,staffmaster m,stafftrans t,department d  where u.lib_code=l.lib_code and (m.staff_code = u.roll_no or m.lib_id = u.roll_no) and m.staff_code = u.roll_no and t.staff_code = m.staff_code and t.dept_code = cast(d.dept_code as nvarchar) and l.college_code='" + college + "' and t.latestrec = 1 " + StrDate + " " + StrTime + " and u.usercat = 'Staff'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                if (sem != "")
                {
                    selectQry += " and u.current_semester in('" + CurrentSem + "','0" + "')";
                }
                if (rollNo != "")
                {
                    selectQry += " and m.staff_code ='" + rollNo + "' ";
                }
                if (Name != "")
                {
                    selectQry += " and m.staff_name like '%" + Name + "%' ";
                }
                //added for non member
                selectQry += " UNION ALL select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name',n.department as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,user_master n where u.lib_code=l.lib_code and n.user_id = u.roll_no and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Staff'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                selectQry += " order by entry_date ,convert(nvarchar(10),entry_time,108) desc";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selectQry, "text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    TotStaffCount = Convert.ToInt32(dsload.Tables[0].Rows.Count);
                    TotCount = TotCount + Convert.ToInt32(dsload.Tables[0].Rows.Count);
                }
                else
                {
                    TotStaffCount = 0;
                }
            }
            //visitors
            if (UserCat == "Visitor")
            {
                selectQry = "select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name','' as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l where u.lib_code=l.lib_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'visitor'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                selectQry += " order by entry_date ,convert(nvarchar(10),entry_time,108) desc";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selectQry, "text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    TotVisitorCount = Convert.ToInt32(dsload.Tables[0].Rows.Count);
                    TotCount = TotCount + Convert.ToInt32(dsload.Tables[0].Rows.Count);
                }
                else
                {
                    TotVisitorCount = 0;
                }
            }
            //count
            if (UserCat == "Student','Staff','Visitor")
            {
                selectQry = "select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number',Reg_No as 'Register No',u.stud_name as 'Name',d.dept_name as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,registration r,degree g,course c,department d  where u.lib_code=l.lib_code and (r.roll_no = u.roll_no or r.lib_id = u.roll_no) and r.roll_no = u.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = d.dept_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Student'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                if (sem != "")
                {
                    selectQry += " and u.current_semester in('" + CurrentSem + "','0" + "')";
                }
                if (rollNo != "")
                {
                    selectQry += " and r.roll_no ='" + rollNo + "' ";
                }
                if (Name != "")
                {
                    selectQry += " and r.stud_name like '%" + Name + "%' ";
                }
                //added for non member
                selectQry += " UNION ALL select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name',n.department as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,user_master n where u.lib_code=l.lib_code and n.user_id = u.roll_no and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Student'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selectQry, "text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    TotStudCount = Convert.ToInt32(dsload.Tables[0].Rows.Count);
                    TotCount = TotCount + Convert.ToInt32(dsload.Tables[0].Rows.Count);
                }
                else
                {
                    TotStudCount = 0;
                }
                selectQry = "select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name',d.dept_name as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,staffmaster m,stafftrans t,department d where u.lib_code=l.lib_code and (m.staff_code = u.roll_no or m.lib_id = u.roll_no) and m.staff_code = u.roll_no  and t.staff_code = m.staff_code and t.dept_code = cast(d.dept_code as nvarchar) and l.college_code='" + college + "' and t.latestrec = 1 " + StrDate + " " + StrTime + " and u.usercat = 'Staff'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                if (sem != "")
                {
                    selectQry += " and u.current_semester in('" + CurrentSem + "','0" + "')";
                }
                if (rollNo != "")
                {
                    selectQry += " and m.staff_code ='" + rollNo + "' ";
                }
                if (Name != "")
                {
                    selectQry += " and m.staff_name like '%" + Name + "%' ";
                }
                //added for non member
                selectQry += " UNION ALL select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name',n.department as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,user_master n where u.lib_code=l.lib_code and n.user_id = u.roll_no and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Staff'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selectQry, "text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    TotStaffCount = Convert.ToInt32(dsload.Tables[0].Rows.Count);
                    TotCount = TotCount + Convert.ToInt32(dsload.Tables[0].Rows.Count);
                }
                else
                {
                    TotStaffCount = 0;
                }
                selectQry = "select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name','' as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l where u.lib_code=l.lib_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'visitor'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                selectQry += " order by entry_date ,convert(nvarchar(10),entry_time,108) desc";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selectQry, "text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    TotVisitorCount = Convert.ToInt32(dsload.Tables[0].Rows.Count);
                    TotCount = TotCount + Convert.ToInt32(dsload.Tables[0].Rows.Count);
                }
                else
                {
                    TotVisitorCount = 0;
                }

                selectQry = "select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number',Reg_No as 'Register No',u.stud_name as 'Name',d.dept_name as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,registration r,degree g,course c,department d where u.lib_code=l.lib_code and (r.roll_no = u.roll_no or r.lib_id = u.roll_no) and r.roll_no = u.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = d.dept_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Student'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                if (sem != "")
                {
                    selectQry += " and u.current_semester in('" + CurrentSem + "','0" + "')";
                }
                if (rollNo != "")
                {
                    selectQry += " and r.roll_no ='" + rollNo + "' ";
                }
                if (Name != "")
                {
                    selectQry += " and r.stud_name like '%" + Name + "%' ";
                }
                selectQry += "UNION ALL select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name',d.dept_name as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,staffmaster m,stafftrans t,department d where u.lib_code=l.lib_code and (m.staff_code = u.roll_no or m.lib_id = u.roll_no) and m.staff_code = u.roll_no and t.staff_code = m.staff_code and t.dept_code = cast(d.dept_code as nvarchar) and l.college_code='" + college + "' and t.latestrec = 1 " + StrDate + " " + StrTime + " and u.usercat = 'Staff'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                if (sem != "")
                {
                    selectQry += " and u.current_semester in('" + CurrentSem + "','0" + "')";
                }
                if (rollNo != "")
                {
                    selectQry += " and m.staff_code ='" + rollNo + "' ";
                }
                if (Name != "")
                {
                    selectQry += " and m.staff_name like '%" + Name + "%' ";
                }
                //added for non member
                selectQry += " UNION ALL select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name',n.department as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,user_master n where u.lib_code=l.lib_code and n.user_id = u.roll_no and l.college_code='" + college + "' " + StrDate + " " + StrTime + " ";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                selectQry += " order by entry_date ,convert(nvarchar(10),entry_time,108) desc";
            }
            if (UserCat == "Student','Staff")
            {
                //Count
                selectQry = "select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number',Reg_No as 'Register No',u.stud_name as 'Name',d.dept_name as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,registration r,degree g,course c,department d where u.lib_code=l.lib_code and (r.roll_no = u.roll_no or r.lib_id = u.roll_no) and r.roll_no = u.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = d.dept_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Student'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                if (sem != "")
                {
                    selectQry += " and u.current_semester in('" + CurrentSem + "')";
                }
                if (rollNo != "")
                {
                    selectQry += " and r.roll_no ='" + rollNo + "' ";
                }
                if (Name != "")
                {
                    selectQry += " and r.stud_name like '%" + Name + "%' ";
                }

                //added for non member
                selectQry += " UNION ALL select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name', n.department as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,user_master n where u.lib_code=l.lib_code and n.user_id = u.roll_no and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Student'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selectQry, "text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    TotStudCount = Convert.ToInt32(dsload.Tables[0].Rows.Count);
                    TotCount = TotCount + Convert.ToInt32(dsload.Tables[0].Rows.Count);
                }
                else
                {
                    TotStudCount = 0;
                }

                selectQry = "select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name',d.dept_name as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,staffmaster m,stafftrans t,department d  where u.lib_code=l.lib_code and (m.staff_code = u.roll_no or m.lib_id = u.roll_no) and m.staff_code = u.roll_no and t.staff_code = m.staff_code and t.dept_code = cast(d.dept_code as nvarchar) and l.college_code='" + college + "' and t.latestrec = 1 " + StrDate + " " + StrTime + " and u.usercat = 'Staff'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                if (sem != "")
                {
                    selectQry += " and u.current_semester in('" + CurrentSem + "','0" + "')";
                }
                if (rollNo != "")
                {
                    selectQry += " and m.staff_code ='" + rollNo + "' ";
                }
                if (Name != "")
                {
                    selectQry += " and m.staff_name like '%" + Name + "%' ";
                }
                //added for non member
                selectQry += " UNION ALL select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name',n.department as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,user_master n where u.lib_code=l.lib_code and n.user_id = u.roll_no and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Staff' ";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                selectQry += " order by entry_date ,convert(nvarchar(10),entry_time,108) desc";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selectQry, "text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    TotStaffCount = Convert.ToInt32(dsload.Tables[0].Rows.Count);
                    TotCount = TotCount + Convert.ToInt32(dsload.Tables[0].Rows.Count);
                }
                else
                {
                    TotStaffCount = 0;
                }

                selectQry = "select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name','' as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l where u.lib_code=l.lib_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'visitor'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                selectQry += " order by entry_date ,convert(nvarchar(10),entry_time,108) desc";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selectQry, "text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    TotVisitorCount = Convert.ToInt32(dsload.Tables[0].Rows.Count);
                    TotCount = TotCount + Convert.ToInt32(dsload.Tables[0].Rows.Count);
                }
                else
                {
                    TotVisitorCount = 0;
                }
                selectQry = "select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number',Reg_No as 'Register No',u.stud_name as 'Name',d.dept_name as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,registration r,degree g,course c,department d where u.lib_code=l.lib_code and (r.roll_no = u.roll_no or r.roll_no = u.roll_no) and r.roll_no = u.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = d.dept_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Student'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                if (sem != "")
                {
                    selectQry += " and u.current_semester in('" + CurrentSem + "')";
                }
                if (rollNo != "")
                {
                    selectQry += " and r.roll_no ='" + rollNo + "' ";
                }
                if (Name != "")
                {
                    selectQry += " and r.stud_name like '%" + Name + "%' ";
                }
                selectQry += " UNION ALL select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name',d.dept_name as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,staffmaster m,stafftrans t,department d where u.lib_code=l.lib_code and (m.staff_code = u.roll_no or m.lib_id = u.roll_no) and m.staff_code = u.roll_no and t.staff_code = m.staff_code and t.dept_code = cast(d.dept_code as nvarchar) and l.college_code='" + college + "' and t.latestrec = 1 " + StrDate + " " + StrTime + " and u.usercat = 'Staff'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                if (sem != "")
                {
                    selectQry += " and u.current_semester in('" + CurrentSem + "','0" + "')";
                }
                if (rollNo != "")
                {
                    selectQry += " and m.staff_code ='" + rollNo + "' ";
                }
                if (Name != "")
                {
                    selectQry += " and m.staff_name like '%" + Name + "%' ";
                }
                //added for non member
                selectQry += " UNION ALL select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name',n.department as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,user_master n where u.lib_code=l.lib_code and n.user_id = u.roll_no and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and (u.usercat = 'Student' or u.usercat = 'Staff')";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }

                selectQry += " order by entry_date ,convert(nvarchar(10),entry_time,108) desc";
            }
            if (UserCat == "Student','Visitor")
            {
                selectQry = "select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number',Reg_No as 'Register No',u.stud_name as 'Name', d.dept_name as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time', case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,registration r,degree g,course c,department d where u.lib_code=l.lib_code and (r.roll_no = u.roll_no or r.lib_id = u.roll_no) and r.roll_no = u.roll_no and  and r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = d.dept_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Student'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                if (sem != "")
                {
                    selectQry += " and u.current_semester in('" + CurrentSem + "')";
                }
                if (rollNo != "")
                {
                    selectQry += " and r.roll_no ='" + rollNo + "' ";
                }
                if (Name != "")
                {
                    selectQry += " and r.stud_name like '%" + Name + "%' ";
                }

                //added for non member
                selectQry += " UNION ALL select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name',n.department as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,user_master n where u.lib_code=l.lib_code and n.user_id = u.roll_no and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Student'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selectQry, "text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    TotStudCount = Convert.ToInt32(dsload.Tables[0].Rows.Count);
                    TotCount = TotCount + Convert.ToInt32(dsload.Tables[0].Rows.Count);
                }
                else
                {
                    TotStudCount = 0;
                }

                selectQry = "select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name','' as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l where u.lib_code=l.lib_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'visitor'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }

                selectQry += " order by entry_date ,convert(nvarchar(10),entry_time,108) desc";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selectQry, "text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    TotVisitorCount = Convert.ToInt32(dsload.Tables[0].Rows.Count);
                    TotCount = TotCount + Convert.ToInt32(dsload.Tables[0].Rows.Count);
                }
                else
                {
                    TotVisitorCount = 0;
                }
                selectQry = "select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number',Reg_No as 'Register No',u.stud_name as 'Name',d.dept_name as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,registration r,degree g,course c,department d where u.lib_code=l.lib_code and( r.roll_no = u.roll_no or r.lib_id = u.roll_no) and r.roll_no = u.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = d.dept_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Student'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                if (sem != "")
                {
                    selectQry += " and u.current_semester in('" + CurrentSem + "')";
                }
                if (rollNo != "")
                {
                    selectQry += " and r.roll_no ='" + rollNo + "' ";
                }
                if (Name != "")
                {
                    selectQry += " and r.stud_name like '%" + Name + "%' ";
                }

                selectQry += " UNION ALL select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name','' as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l where u.lib_code=l.lib_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'visitor'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                //added for non member
                selectQry += " UNION ALL select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name',n.department as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,user_master n where u.lib_code=l.lib_code and n.user_id = u.roll_no and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Student'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }

                selectQry += " order by entry_date ,convert(nvarchar(10),entry_time,108) desc";
            }
            if (UserCat == "Staff','Visitor")
            {
                selectQry = "select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name', d.dept_name as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,staffmaster m,stafftrans t,department d  where u.lib_code=l.lib_code and (m.staff_code = u.roll_no or m.lib_id = u.roll_no) and m.staff_code = u.roll_no and t.staff_code = m.staff_code and t.dept_code = cast(d.dept_code as nvarchar) and l.college_code='" + college + "' and t.latestrec = 1 " + StrDate + " " + StrTime + " and u.usercat = 'Staff'";

                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                if (rollNo != "")
                {
                    selectQry += " and m.staff_code ='" + rollNo + "' ";
                }
                if (Name != "")
                {
                    selectQry += " and m.staff_name like '%" + Name + "%' ";
                }
                //added for non member
                selectQry += " UNION ALL select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name',n.department as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,user_master n where u.lib_code=l.lib_code and n.user_id = u.roll_no and l.college_code='" + college + "' and u.usercat = 'Staff'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selectQry, "text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    TotStaffCount = Convert.ToInt32(dsload.Tables[0].Rows.Count);
                    TotCount = TotCount + Convert.ToInt32(dsload.Tables[0].Rows.Count);
                }
                else
                {
                    TotStaffCount = 0;
                }
                selectQry = "select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name','' as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l where u.lib_code=l.lib_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'visitor'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                selectQry += " order by entry_date ,convert(nvarchar(10),entry_time,108) desc";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selectQry, "text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    TotVisitorCount = Convert.ToInt32(dsload.Tables[0].Rows.Count);
                    TotCount = TotCount + Convert.ToInt32(dsload.Tables[0].Rows.Count);
                }
                else
                {
                    TotVisitorCount = 0;
                }
                selectQry = "select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name',d.dept_name as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,staffmaster m,stafftrans t,department d where u.lib_code=l.lib_code and (m.staff_code = u.roll_no or m.lib_id = u.roll_no) and m.staff_code = u.roll_no and t.staff_code = m.staff_code and t.dept_code = cast(d.dept_code as nvarchar) and l.college_code='" + college + "' and t.latestrec = 1 " + StrDate + " " + StrTime + " and u.usercat = 'Staff'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                if (sem != "")
                {
                    selectQry += " and u.current_semester in('" + CurrentSem + "','0" + "')";
                }
                if (rollNo != "")
                {
                    selectQry += " and m.staff_code ='" + rollNo + "' ";
                }
                if (Name != "")
                {
                    selectQry += " and m.staff_name like '%" + Name + "%' ";
                }

                //added for non member
                selectQry += " UNION ALL select distinct '' as 'Select',usercat as 'Type',u.roll_no as 'Roll Number','' as 'Register No',u.stud_name as 'Name',n.department as 'Department',entry_date as 'Entry Date',convert(nvarchar(10),entry_time,108) as 'Entry Time',exit_time as 'Exit Time',case when len(exit_time) > 0 then convert(char(8),(cast(exit_time as datetime) - cast(entry_time as datetime)),114) else '' end Duration from libusers u ,library l,user_master n where u.lib_code=l.lib_code and n.user_id = u.roll_no and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Staff'";
                if (libraryCode != "")
                {
                    selectQry += " and u.lib_code in('" + libraryCode + "')";
                }
                if (dept != "")
                {
                    selectQry += " and u.dept_name in('" + dept + "')";
                }
                selectQry += " order by entry_date ,convert(nvarchar(10),convert(nvarchar(10),entry_time,108),108) desc";

            }
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(selectQry, "text");
            #endregion

            #region value

            if (dsload.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsload.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    string EntryDate = Convert.ToString(dsload.Tables[0].Rows[i]["Entry Date"]);
                    string[] En_date = EntryDate.Split('/');
                    if (En_date.Length == 3)
                        Lib_EntryDate = En_date[1].ToString() + "/" + En_date[0].ToString() + "/" + En_date[2].ToString();
                    drow = dtVisit_Details.NewRow();
                    drow["SNo"] = sno;
                    drow["Type"] = Convert.ToString(dsload.Tables[0].Rows[i]["Type"]);
                    drow["Roll No"] = Convert.ToString(dsload.Tables[0].Rows[i]["Roll Number"]);
                    drow["Register No"] = Convert.ToString(dsload.Tables[0].Rows[i]["Register No"]);
                    drow["Name"] = Convert.ToString(dsload.Tables[0].Rows[i]["Name"]);
                    drow["Department"] = Convert.ToString(dsload.Tables[0].Rows[i]["Department"]);
                    drow["Entry Date"] = Convert.ToString(Lib_EntryDate.Split(' ')[0]);
                    drow["Entry Time"] = Convert.ToString(dsload.Tables[0].Rows[i]["Entry Time"]);
                    drow["Exit Time"] = Convert.ToString(dsload.Tables[0].Rows[i]["Exit Time"]);
                    drow["Duration"] = Convert.ToString(dsload.Tables[0].Rows[i]["Duration"]);
                    dtVisit_Details.Rows.Add(drow);
                }
                grdVisit_Details.Visible = true;
                divVisit_Details.Visible = true;
                grdVisit_Details.DataSource = dtVisit_Details;
                grdVisit_Details.DataBind();
                print.Visible = false;
                print1.Visible = false;
                print2.Visible = true;
                btndelete2.Visible = true;


            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "No Record found";
                grdVisit_Details.Visible = false;
                divVisit_Details.Visible = false;
            }
            #endregion
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "UserInOutEntry");
        }
    }

    protected void grdVisit_Details_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //((CheckBox)e.Row.FindControl("allchk")).Attributes.Add("onclick",
            //    "javascript:SelectAll('" +
            //    ((CheckBox)e.Row.FindControl("allchk")).ClientID + "')");
            for (int grCol = 0; grCol < grdVisit_Details.Columns.Count; grCol++)
                e.Row.Cells[grCol].Visible = false;
            //e.Row.Cells[5].Visible = false;

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0)
            {
                //CheckBox cbsel = (CheckBox)e.Row.Cells[5].FindControl("selectchk");
                //cbsel.Visible = false;
                //cbsel.Text = "Select";


            }
            // e.Row.Cells[2].Visible = false;
        }
    }

    protected void Visit_Details_WithTime(object sender, EventArgs e)
    {
        try
        {
            DivVisitDetWithTime.Visible = false;
            divVisit_Details.Visible = false;
            divSpreadVisitWithTime.Visible = true;
            int colCount = 1;
            bool user = false;
            int UserCatCount = checkusers.Items.Count;

            DataTable dtVisit_Details_WithTime = new DataTable();
            DataRow drow;
            dtVisit_Details_WithTime.Columns.Add("SNo", typeof(string));
            dtVisit_Details_WithTime.Columns.Add("Date", typeof(string));
            drow = dtVisit_Details_WithTime.NewRow();



            int i = 0;
            string college = Convert.ToString(ddl_collegename.SelectedValue);
            string Sql = "SELECT distinct * FROM LibRepTimeSettings where collegecode =" + college + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (UserCatCount > 0)
            {
                for (int userVal = 0; userVal < UserCatCount; userVal++)
                {
                    if (checkusers.Items[userVal].Selected == true)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //dtVisit_Details_WithTime.Columns.Add(checkusers.Items[userVal].Text);
                            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                string FromTime = Convert.ToString(ds.Tables[0].Rows[i]["FromTime"]);
                                string ToTime = Convert.ToString(ds.Tables[0].Rows[i]["ToTime"]);
                                dtVisit_Details_WithTime.Columns.Add(FromTime + " To " + ToTime);
                                drow[FromTime + " To " + ToTime] = FromTime + " To " + ToTime;
                            }
                        }
                    }
                }
                dtVisit_Details_WithTime.Rows.Add(drow);
            }

            #region query

            if (cbdate.Checked == false)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Select the Date";
                return;
            }
            string libraryCode = getCblSelectedValue(cbl_library);
            string dept = getCblSelectedValue(cbl_dept);
            string sem = getCblSelectedText(cbl_sem);
            string UserCat = getCblSelectedText(checkusers);
            string StrDate = string.Empty;
            string StrTime = string.Empty;
            string selectQry = string.Empty;
            string entryDate = string.Empty;
            string Lib_EntryDate = string.Empty;
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[2].ToString() + "/" + frdate[1].ToString() + "/" + frdate[0].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[2].ToString() + "/" + tdate[1].ToString() + "/" + tdate[0].ToString();
            string[] Semster = sem.Split(new string[] { "','" }, StringSplitOptions.None);
            string SemVal = string.Empty;
            string CurrentSem = "";

            int sno = 0;
            int Total = 0;
            Hashtable GrandTotHash = new Hashtable();
            int j = 0;
            int k = 0;
            int m = 0;

            for (i = 0; i < Semster.Length; i++)
            {
                SemVal = Semster[i];
                string SemCode = SemVal.Split(' ')[0];
                if (!CurrentSem.Contains(SemCode))
                {
                    if (CurrentSem == "")
                        CurrentSem = SemCode;
                    else
                        CurrentSem = CurrentSem + "','" + SemCode;
                }
            }
            DateTime F_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector1.Hour, TimeSelector1.Minute, TimeSelector1.Second, TimeSelector1.AmPm));
            DateTime T_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector2.Hour, TimeSelector2.Minute, TimeSelector2.Second, TimeSelector2.AmPm));

            if (cbdate.Checked)
                StrDate = " and entry_date between '" + fromdate + "' and '" + todate + "'";
            else
                StrDate = "";

            selectQry = "SELECT distinct Entry_Date FROM LibUsers WHERE UserCat in('" + UserCat + "') " + StrDate + " " + StrTime + "";
            if (libraryCode != "")
            {
                selectQry += " and lib_code in('" + libraryCode + "')";
            }
            if (sem != "")
            {
                selectQry += " and current_semester in('" + CurrentSem + "','0" + "')";
            }
            selectQry += "GROUP BY Entry_Date ";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(selectQry, "text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                for (i = 0; i < dsload.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    entryDate = Convert.ToString(dsload.Tables[0].Rows[i]["Entry_Date"]);
                    string[] En_date = entryDate.Split('/');
                    if (En_date.Length == 3)
                        Lib_EntryDate = En_date[1].ToString() + "/" + En_date[0].ToString() + "/" + En_date[2].ToString();
                    drow = dtVisit_Details_WithTime.NewRow();
                    drow["SNo"] = sno;
                    drow["Date"] = Convert.ToString(Lib_EntryDate.Split(' ')[0]);

                    //Student                   
                    if (UserCat == "Student" || UserCat == "Student','Staff" || UserCat == "Student','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        k = 2;
                        m = 2;
                        //for (j = m; j < (k + (LstBoxTime.Items.Count)); j++)
                        //{
                        for (int dtCol = 2; dtCol < dtVisit_Details_WithTime.Columns.Count; dtCol++)
                        {
                            string strtime = Convert.ToString(dtVisit_Details_WithTime.Columns[dtCol]);//Convert.ToString(HeaderGridRow.Cells[j].Text);
                            string[] Time = strtime.Split(new string[] { "To" }, StringSplitOptions.None);

                            StrTime = " AND (CONVERT(datetime,Entry_Time,108) BETWEEN '" + Time[0] + "' AND '" + Time[1] + "' ) ";
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Student' AND Entry_Date ='" + entryDate + "' " + StrTime + "";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            if (cb_dept.Checked == true)
                            {
                            }
                            else
                            {
                                if (dept != "")
                                {
                                    selectQry += " and dept_name in('" + dept + "')";
                                }
                            }
                            if (sem != "")
                            {
                                selectQry += " and current_semester in('" + CurrentSem + "')";
                            }
                            selectQry += "GROUP BY Entry_Date ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int dsStu = 0; dsStu < ds.Tables[0].Rows.Count; dsStu++)
                                {
                                    drow[strtime] = Convert.ToString(ds.Tables[0].Rows[dsStu]["count"]);
                                    Total = Convert.ToInt32(ds.Tables[0].Rows[dsStu]["count"]);
                                    if (!GrandTotHash.ContainsKey(dtCol))
                                        GrandTotHash.Add(dtCol, Convert.ToString(Total));
                                    else
                                    {
                                        double Count = 0;
                                        double.TryParse(Convert.ToString(GrandTotHash[dtCol]), out Count);
                                        Count += Total;
                                        GrandTotHash.Remove(dtCol);
                                        GrandTotHash.Add(dtCol, Convert.ToString(Count));
                                    }
                                }
                            }
                            else
                            {
                                drow[strtime] = "0";
                            }
                        }
                    }
                    //Staff                    
                    if (UserCat == "Staff" || UserCat == "Student','Staff" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        k = 2 + (LstBoxTime.Items.Count);
                        m = 2 + (LstBoxTime.Items.Count);

                        for (int dtCol = 2; dtCol < dtVisit_Details_WithTime.Columns.Count; dtCol++)
                        {
                            string strtime = Convert.ToString(dtVisit_Details_WithTime.Columns[dtCol]);
                            string[] Time = strtime.Split(new string[] { "To" }, StringSplitOptions.None);
                            StrTime = " AND (CONVERT(datetime,Entry_Time,108) BETWEEN '" + Time[0] + "' AND '" + Time[1] + "' ) ";
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Staff' AND Entry_Date ='" + entryDate + "' " + StrTime + "";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            if (cb_dept.Checked == true)
                            {
                            }
                            else
                            {
                                if (dept != "")
                                {
                                    selectQry += " and dept_name in('" + dept + "')";
                                }
                            }
                            selectQry += "GROUP BY Entry_Date ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int dsStaff = 0; dsStaff < ds.Tables[0].Rows.Count; dsStaff++)
                                {
                                    drow[strtime] = Convert.ToString(ds.Tables[0].Rows[dsStaff]["count"]);
                                    Total = Convert.ToInt32(ds.Tables[0].Rows[dsStaff]["count"]);
                                    if (!GrandTotHash.ContainsKey(dtCol))
                                        GrandTotHash.Add(dtCol, Convert.ToString(Total));
                                    else
                                    {
                                        double Count = 0;
                                        double.TryParse(Convert.ToString(GrandTotHash[dtCol]), out Count);
                                        Count += Total;
                                        GrandTotHash.Remove(dtCol);
                                        GrandTotHash.Add(dtCol, Convert.ToString(Count));
                                    }
                                }
                            }
                            else
                            {
                                drow[strtime] = "0";
                            }
                        }
                    }
                    //Visitor                    
                    if (UserCat == "Visitor" || UserCat == "Student','Visitor" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        k = 2 + (LstBoxTime.Items.Count * 2);
                        m = 2 + (LstBoxTime.Items.Count * 2);
                        for (int dtCol = 2; dtCol < dtVisit_Details_WithTime.Columns.Count; dtCol++)
                        {
                            string strtime = Convert.ToString(dtVisit_Details_WithTime.Columns[dtCol]);
                            string[] Time = strtime.Split(new string[] { "To" }, StringSplitOptions.None);

                            StrTime = " AND (CONVERT(datetime,Entry_Time,108) BETWEEN '" + Time[0] + "' AND '" + Time[1] + "' ) ";

                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Visitor' AND Entry_Date ='" + entryDate + "' " + StrTime + "";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            selectQry += "GROUP BY Entry_Date ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int dsVisit = 0; dsVisit < ds.Tables[0].Rows.Count; dsVisit++)
                                {
                                    drow[strtime] = Convert.ToString(ds.Tables[0].Rows[dsVisit]["count"]);
                                    Total = Convert.ToInt32(ds.Tables[0].Rows[dsVisit]["count"]);
                                    if (!GrandTotHash.ContainsKey(dtCol))
                                        GrandTotHash.Add(dtCol, Convert.ToString(Total));
                                    else
                                    {
                                        double Count = 0;
                                        double.TryParse(Convert.ToString(GrandTotHash[dtCol]), out Count);
                                        Count += Total;
                                        GrandTotHash.Remove(dtCol);
                                        GrandTotHash.Add(dtCol, Convert.ToString(Count));
                                    }
                                }
                            }
                            else
                            {
                                drow[strtime] = "0";
                            }
                        }
                    }
                    dtVisit_Details_WithTime.Rows.Add(drow);
                }
                drow = dtVisit_Details_WithTime.NewRow();
                drow["SNo"] = "Total";
                for (int Count = 2; Count < dtVisit_Details_WithTime.Columns.Count; Count++)
                {
                    int header = Count;
                    if (GrandTotHash.Contains(header))
                    {
                        drow[header] = Convert.ToString(GrandTotHash[header]);
                    }
                }
                dtVisit_Details_WithTime.Rows.Add(drow);
                GrandTotHash.Clear();
                GrdVisitWithTime.DataSource = dtVisit_Details_WithTime;
                GrdVisitWithTime.DataBind();
                GrdVisitWithTime.Visible = true;
                divSpreadVisitWithTime.Visible = true;
                print.Visible = false;
                print1.Visible = true;
                print2.Visible = false;


            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "No Record Found";
                GrdVisitWithTime.Visible = false;
                divSpreadVisitWithTime.Visible = false;
                print.Visible = false;
                print1.Visible = true;
                print2.Visible = false;
            }

            #endregion
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode, "UserInOutEntry");
        }
    }

    #region visit Details With Time Popup

    protected void BtnAddTime_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime F_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector3.Hour, TimeSelector3.Minute, TimeSelector3.Second, TimeSelector3.AmPm));
            DateTime T_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector4.Hour, TimeSelector4.Minute, TimeSelector4.Second, TimeSelector4.AmPm));
            string frmTime = Convert.ToString(F_time);
            string[] FromTime = frmTime.Split(' ');
            string tTime = Convert.ToString(T_time);
            string[] ToTime = tTime.Split(' ');
            LstBoxTime.Items.Add(FromTime[1] + FromTime[2] + "-" + ToTime[1] + ToTime[2]);
            count++;
        }
        catch (Exception ex)
        {
        }
    }

    protected void BtnTimeSave_Click(object sender, EventArgs e)
    {
        try
        {
            string college = Convert.ToString(ddl_collegename.SelectedValue);
            string Sql = string.Empty;
            string insertQry = "";
            int insert = 0;
            int delete = 0;
            Sql = "DELETE FROM LibRepTimeSettings where collegecode in(" + college + ")";
            delete = d2.update_method_wo_parameter(Sql, "TEXT");
            int listCount = LstBoxTime.Items.Count;
            for (int i = 0; i < LstBoxTime.Items.Count; i++)
            {
                string startTime = LstBoxTime.Items[i].Text;
                string[] StrTime = startTime.Split('-');
                if (startTime != "")
                {
                    insertQry = "insert into LibRepTimeSettings(FromTime,ToTime,CollegeCode) values ('" + StrTime[0] + "','" + StrTime[1] + "','" + college + "')";
                    insert = d2.update_method_wo_parameter(insertQry, "TEXT");
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void BtnRemoveTime_Click(object sender, EventArgs e)
    {
        LstBoxTime.Items.Remove(LstBoxTime.SelectedItem);
    }

    #endregion

    protected void BatchWise(object sender, EventArgs e)
    {
        try
        {
            int gvHasRows = grdUserReport.Rows.Count;
            if (gvHasRows > 0)
            {
                grdUserReport.Columns.Clear();
                grdUserReport.DataBind();
            }
            string UserCat = getCblSelectedText(checkusers);
            DataTable dtBatchwise = new DataTable();
            DataRow drow;
            dtBatchwise.Columns.Add("S.No", typeof(string));
            dtBatchwise.Columns.Add("Batch", typeof(string));
            dtBatchwise.Columns.Add("Department", typeof(string));
            dtBatchwise.Columns.Add("Total Visit", typeof(string));
            dtBatchwise.Columns.Add("Issued Count", typeof(string));
            dtBatchwise.Columns.Add("Returned Count", typeof(string));
            dtBatchwise.Columns.Add("Reference Count", typeof(string));

            #region Query

            string college = Convert.ToString(ddl_collegename.SelectedValue);
            string libraryCode = getCblSelectedValue(cbl_library);
            string degreecode = getCblSelectedValue(cbl_dept);
            string dept = getCblSelectedText(cbl_dept);
            string sem = getCblSelectedText(cbl_sem);
            //string batch1 = getCblSelectedText(cbl_batch);

            string StrDate = string.Empty;
            string StrTime = string.Empty;
            string selectQry = string.Empty;
            //string selectQry = string.Empty;
            string entryDate = string.Empty;
            string Lib_EntryDate = string.Empty;
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[2].ToString() + "/" + frdate[1].ToString() + "/" + frdate[0].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[2].ToString() + "/" + tdate[1].ToString() + "/" + tdate[0].ToString();
            string[] Semster = sem.Split(new string[] { "','" }, StringSplitOptions.None);
            string SemVal = string.Empty;
            string CurrentSem = "";
            int sno = 0;
            int TotStudCount = 0;
            int TotStaffCount = 0;
            int TotVisitorCount = 0;
            int TotCount = 0;

            for (int i = 0; i < Semster.Length; i++)
            {
                SemVal = Semster[i];
                string SemCode = SemVal.Split(' ')[0];
                if (!CurrentSem.Contains(SemCode))
                {
                    if (CurrentSem == "")
                        CurrentSem = SemCode;
                    else
                        CurrentSem = CurrentSem + "','" + SemCode;
                }
            }
            DateTime F_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector1.Hour, TimeSelector1.Minute, TimeSelector1.Second, TimeSelector1.AmPm));
            string FromTime = Convert.ToString(F_time);
            string FinalFromTime = FromTime.Split(' ')[1] + FromTime.Split(' ')[2];
            DateTime T_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector2.Hour, TimeSelector2.Minute, TimeSelector2.Second, TimeSelector2.AmPm));
            string ToTime = Convert.ToString(T_time);
            string FinalToTime = ToTime.Split(' ')[1] + ToTime.Split(' ')[2];
            string strborrowdt = "";
            string strReturndt = "";
            if (cbdate.Checked)
            {
                StrDate = " and entry_date between '" + fromdate + "' and '" + todate + "'";
                strborrowdt = " and borrow_date between '" + fromdate + "' and '" + todate + "'";
                strReturndt = " and Return_Date between '" + fromdate + "' and '" + todate + "'";
            }
            else
                StrDate = "";
            if (cbtime.Checked)
            {
                StrTime = " AND ((CONVERT(datetime,Entry_Time,108) BETWEEN '" + FinalFromTime + "' AND '" + FinalToTime + "' ) OR (CONVERT(datetime,Exit_Time,108) BETWEEN '" + FinalFromTime + "' AND '" + FinalToTime + "' ))";
            }
            else
                StrTime = "";

            for (int year = 0; year < cbl_batch.Items.Count; year++)
            {
                if (cbl_batch.Items[year].Selected)
                {
                    string batch1 = Convert.ToString(cbl_batch.Items[year].Value);
                    for (int deg = 0; deg < cbl_dept.Items.Count; deg++)
                    {
                        if (cbl_dept.Items[deg].Selected)
                        {
                            sno++;
                            dept = Convert.ToString(cbl_dept.Items[deg].Text);
                            degreecode = Convert.ToString(cbl_dept.Items[deg].Value);
                            selectQry = "select r.Batch_Year,D.Dept_Name,u.lib_code,lib_name,Count(*) Tot from libusers u ,library l,registration r,degree g,course c,department d where u.lib_code=l.lib_code and r.roll_no = u.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = d.dept_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Student'";
                            if (libraryCode != "")
                            {
                                selectQry += " and u.lib_code in('" + libraryCode + "')";
                            }
                            if (dept != "")
                            {
                                selectQry += " and u.dept_name in('" + dept + "')";
                            }
                            if (sem != "")
                            {
                                selectQry += " and u.current_semester in('" + CurrentSem + "')";
                            }
                            if (batch1 != "")
                            {
                                selectQry = selectQry + " and r.Batch_Year in ('" + batch1 + "')";
                            }
                            selectQry += " group by u.lib_code,lib_name,d.dept_name,r.Batch_Year order by r.Batch_Year,lib_name,d.dept_name";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            drow = dtBatchwise.NewRow();
                            drow["S.No"] = sno;
                            drow["Batch"] = batch1;
                            drow["Department"] = Convert.ToString(cbl_dept.Items[deg].Text);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                double Totalvisit = 0;
                                for (int tv = 0; tv < ds.Tables[0].Rows.Count; tv++)
                                {
                                    double totvis = Convert.ToDouble(ds.Tables[0].Rows[0]["Tot"]);
                                    Totalvisit = Totalvisit + totvis;
                                }
                                drow["Total Visit"] = Convert.ToString(Totalvisit);
                            }
                            else
                                drow["Total Visit"] = "0";

                            selectQry = " select count(*) as 'Books' from borrow,library,department,registration,degree where registration.roll_no=borrow.roll_no  and registration.degree_code=degree.degree_code and degree.dept_code=department.dept_code and borrow.lib_code=library.lib_code and is_staff = 0  " + strborrowdt + " and registration.college_code='" + college + "' ";
                            if (libraryCode != "")
                            {
                                selectQry += " and borrow.lib_code in('" + libraryCode + "')";
                            }
                            if (batch1 != "")
                            {
                                selectQry = selectQry + " and registration.Batch_Year in ('" + batch1 + "')";
                            }
                            if (sem != "")
                            {
                                selectQry += " and registration.current_semester in('" + CurrentSem + "')";
                            }
                            if (dept != "")
                            {
                                selectQry += " and registration.degree_code in('" + degreecode + "')";
                            }
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Issued Count"] = Convert.ToString(ds.Tables[0].Rows[0]["Books"]);
                            }
                            else
                                drow["Issued Count"] = "0";
                            selectQry = "SELECT COUNT(*) as 'ReturnedCount' FROM Borrow B,Registration R,Degree G,Library L WHERE (B.Roll_No = R.Roll_No OR B.Roll_No = R.Lib_ID) and B.Roll_No = R.Roll_No AND B.Lib_Code = L.Lib_Code AND R.Degree_Code = G.Degree_Code AND B.Is_Staff = 0 AND Return_Flag = 1  AND L.College_Code ='" + college + "' " + strReturndt + " ";
                            if (libraryCode != "")
                            {
                                selectQry += " and B.lib_code in('" + libraryCode + "')";
                            }
                            if (dept != "")
                            {
                                selectQry += " and R.degree_code in('" + degreecode + "')";
                            }
                            if (batch1 != "")
                            {
                                selectQry = selectQry + " and R.Batch_Year in ('" + batch1 + "')";
                            }
                            if (sem != "")
                            {
                                selectQry += " and R.current_semester in('" + CurrentSem + "')";
                            }
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                                drow["Returned Count"] = Convert.ToString(ds.Tables[0].Rows[0]["ReturnedCount"]);
                            else
                                drow["Returned Count"] = "0";

                            ///////
                            selectQry = "select r.Batch_Year,D.Dept_Name,u.lib_code,lib_name,Count(*) Tot from libusers u ,library l,registration r,degree g,course c,department d where u.lib_code=l.lib_code and r.roll_no = u.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = d.dept_code and l.college_code='" + college + "' " + StrDate + " " + StrTime + " and u.usercat = 'Student' and u.roll_no not in(select roll_no from borrow)";
                            if (libraryCode != "")
                            {
                                selectQry += " and u.lib_code in('" + libraryCode + "')";
                            }
                            if (dept != "")
                            {
                                selectQry += " and u.dept_name in('" + dept + "')";
                            }
                            if (sem != "")
                            {
                                selectQry += " and u.current_semester in('" + CurrentSem + "')";
                            }
                            if (batch1 != "")
                            {
                                selectQry = selectQry + " and r.Batch_Year in ('" + batch1 + "')";
                            }
                            selectQry += " group by u.lib_code,lib_name,d.dept_name,r.Batch_Year order by r.Batch_Year,lib_name,d.dept_name";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                double Totalvisit = 0;
                                for (int tv = 0; tv < ds.Tables[0].Rows.Count; tv++)
                                {
                                    double totvis = Convert.ToDouble(ds.Tables[0].Rows[0]["Tot"]);
                                    Totalvisit = Totalvisit + totvis;
                                }
                                drow["Reference Count"] = Convert.ToString(Totalvisit);
                            }
                            else
                                drow["Reference Count"] = "0";

                            dtBatchwise.Rows.Add(drow);
                        }
                    }
                }
            }

            grdUserReport.DataSource = dtBatchwise;
            grdUserReport.DataBind();
            grdUserReport.Visible = true;
            divSpreadReport.Visible = true;

            #endregion
        }
        catch (Exception ex)
        {
        }
    }

    protected void Visitor_Statistics_Batch_Year(object sender, EventArgs e)
    {
        try
        {
            int gvHasRows = grdUserReport.Rows.Count;
            if (gvHasRows > 0)
            {
                grdUserReport.Columns.Clear();
                grdUserReport.DataBind();
            }
            divSpreadVisitWithTime.Visible = false;
            divVisit_Details.Visible = false;
            if (cbdate.Checked == false)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Select the Date";
                return;
            }
            string college = Convert.ToString(ddl_collegename.SelectedValue);
            string libraryCode = getCblSelectedValue(cbl_library);
            string dept = getCblSelectedValue(cbl_dept);
            string sem = getCblSelectedText(cbl_sem);
            string UserCat = getCblSelectedText(checkusers);
            string StrDate = string.Empty;
            string StrTime = string.Empty;
            string selectQry = string.Empty;
            string entryDate = string.Empty;
            string Lib_EntryDate = string.Empty;
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[2].ToString() + "/" + frdate[1].ToString() + "/" + frdate[0].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[2].ToString() + "/" + tdate[1].ToString() + "/" + tdate[0].ToString();
            string[] Semster = sem.Split(new string[] { "','" }, StringSplitOptions.None);
            string SemVal = string.Empty;
            string CurrentSem = "";
            int intTotCount = 0;
            int sno = 0;
            int IntTotStud = 0;
            int IntTotStaff = 0;
            int IntTotVisitor = 0;
            int IntTotOpac = 0;
            int Total = 0;
            int gdrow = 0;
           
            Dictionary<int, string> dicVisitor_Entry_Statistics = new Dictionary<int, string>();

            DataTable dtVisitor_Entry_Statistics = new DataTable();
            DataRow drow;
           
            a1.Add("S.No");
            a2.Add("S.No");
            dtVisitor_Entry_Statistics.Columns.Add("S.No", typeof(string));
            if (rbdaily.Checked)
            {
              
                a1.Add("Date");
                a2.Add("Date");
                dtVisitor_Entry_Statistics.Columns.Add("Date", typeof(string));
            }
            if (rbweekly.Checked)
            {
               
                a1.Add("Week");
                a2.Add("Week");
                dtVisitor_Entry_Statistics.Columns.Add("Week", typeof(string));
            }
            if (rbmonthly.Checked)
            {
               
                a1.Add("Month");
                a2.Add("Month");
                dtVisitor_Entry_Statistics.Columns.Add("Month", typeof(string));
            }
            if (rbyearly.Checked)
            {
               
                a1.Add("Year");
                a2.Add("Year");
                dtVisitor_Entry_Statistics.Columns.Add("Year", typeof(string));
            }
            if (UserCat == "Student" || UserCat == "Student','Staff" || UserCat == "Student','Visitor" || UserCat == "Student','Staff','Visitor")
            {
              
                a1.Add("Student");
                a1.Add("Student");
                a1.Add("Student");
                a1.Add("Student");
                a2.Add("Total Visit");
                a2.Add("Issued Count");
                a2.Add("Returned Count");
                a2.Add("Refernce Count");
               
                dtVisitor_Entry_Statistics.Columns.Add("Total Visit", typeof(string));
                dtVisitor_Entry_Statistics.Columns.Add("Issued Count", typeof(string));
                dtVisitor_Entry_Statistics.Columns.Add("Returned Count", typeof(string));
                dtVisitor_Entry_Statistics.Columns.Add("Refernce Count", typeof(string));
            }
            if (UserCat == "Staff" || UserCat == "Student','Staff" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
            {
               
                a1.Add("Staff");
                a1.Add("Staff");
                a1.Add("Staff"); 
                a1.Add("Staff");
                a2.Add("Total Visit");
                a2.Add("Issued Count");
                a2.Add("Returned Count");
                a2.Add("Refernce Count");
               
                dtVisitor_Entry_Statistics.Columns.Add("Total Visit", typeof(string));
                dtVisitor_Entry_Statistics.Columns.Add("Issued Count", typeof(string));
                dtVisitor_Entry_Statistics.Columns.Add("Returned Count", typeof(string));
                dtVisitor_Entry_Statistics.Columns.Add("Refernce Count", typeof(string));
            }
            if (UserCat == "Visitor" || UserCat == "Student','Visitor" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
            {
               
                a1.Add("Visitor");
                a1.Add("Visitor");
                a1.Add("Visitor");
                a1.Add("Visitor");
                a2.Add("Total Visit");
                a2.Add("Issued Count");
                a2.Add("Returned Count");
                a2.Add("Refernce Count");
               
                dtVisitor_Entry_Statistics.Columns.Add("Total Visit", typeof(string));
                dtVisitor_Entry_Statistics.Columns.Add("Issued Count", typeof(string));
                dtVisitor_Entry_Statistics.Columns.Add("Returned Count", typeof(string));
                dtVisitor_Entry_Statistics.Columns.Add("Refernce Count", typeof(string));
            }
          
            dtVisitor_Entry_Statistics.Columns.Add("Total", typeof(string));
            a1.Add("Total");
            a2.Add("Total");

            DataRow drHdr1 = dtVisitor_Entry_Statistics.NewRow();
            DataRow drHdr2 = dtVisitor_Entry_Statistics.NewRow();
            for (int grCol = 0; grCol < dtVisitor_Entry_Statistics.Columns.Count; grCol++)
            {
                drHdr1[grCol] = a1[grCol];
                drHdr2[grCol] = a2[grCol];
               
            }
            dtVisitor_Entry_Statistics.Rows.Add(drHdr1);
            dtVisitor_Entry_Statistics.Rows.Add(drHdr2);
         
            for (int i = 0; i < Semster.Length; i++)
            {
                SemVal = Semster[i];
                string SemCode = SemVal.Split(' ')[0];
                if (!CurrentSem.Contains(SemCode))
                {
                    if (CurrentSem == "")
                        CurrentSem = SemCode;
                    else
                        CurrentSem = CurrentSem + "','" + SemCode;
                }
            }
            DateTime F_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector1.Hour, TimeSelector1.Minute, TimeSelector1.Second, TimeSelector1.AmPm));
            DateTime T_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector2.Hour, TimeSelector2.Minute, TimeSelector2.Second, TimeSelector2.AmPm));

            if (cbdate.Checked)
                StrDate = " and entry_date between '" + fromdate + "'";
            else
                StrDate = "";
            if (cbtime.Checked)
                StrTime = " AND ((CONVERT(datetime,Entry_Time,108) BETWEEN '" + F_time + "' AND '" + T_time + "' ) OR (CONVERT(datetime,Exit_Time,108) BETWEEN '" + F_time + "' AND '" + T_time + "' ))";
            else
                StrTime = "";


            #region Daily

            if (rbdaily.Checked)
            {
                selectQry = "SELECT distinct Entry_Date FROM LibUsers WHERE UserCat in('" + UserCat + "') " + StrDate + " " + StrTime + "";
                if (libraryCode != "")
                {
                    selectQry += " and lib_code in('" + libraryCode + "')";
                }
                if (sem != "")
                {
                    selectQry += " and current_semester in('" + CurrentSem + "','0" + "')";
                }
                selectQry += "GROUP BY Entry_Date ";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selectQry, "text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsload.Tables[0].Rows.Count; i++)
                    {
                        intTotCount = 0;
                        entryDate = Convert.ToString(dsload.Tables[0].Rows[i]["Entry_Date"]);
                        string[] En_date = entryDate.Split('/');
                        if (En_date.Length == 3)
                            Lib_EntryDate = En_date[1].ToString() + "/" + En_date[0].ToString() + "/" + En_date[2].ToString();
                        sno++;
                        drow = dtVisitor_Entry_Statistics.NewRow();
                        drow["S.No"] = sno;
                        drow["Date"] = Convert.ToString(Lib_EntryDate.Split(' ')[0]);
                        //Student
                        if (UserCat == "Student" || UserCat == "Student','Staff" || UserCat == "Student','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "select u.lib_code,lib_name,Count(*) Tot,entry_date from libusers u ,library l,registration r,degree g,course c,department d where UserCat = 'Student' and u.lib_code=l.lib_code and r.roll_no = u.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = d.dept_code and l.college_code='" + college + "'  and " + StrDate + " ";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            if (sem != "")
                            {
                                selectQry += " and current_semester in('" + CurrentSem + "')";
                            }
                            selectQry += "GROUP BY u.lib_code,lib_name,entry_date order by lib_name ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Student"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotStud = IntTotStud + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Student"] = "0";
                            }
                        }

                        //Staff
                        if (UserCat == "Staff" || UserCat == "Student','Staff" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Staff' AND Entry_Date ='" + entryDate + "' " + StrTime + "";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }

                            selectQry += "GROUP BY Entry_Date ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Staff"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotStaff = IntTotStaff + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Staff"] = "0";
                            }
                        }
                        //'visitors
                        if (UserCat == "Visitor" || UserCat == "Student','Visitor" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Visitor' AND Entry_Date ='" + entryDate + "' " + StrTime + " ";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            selectQry += "GROUP BY Dept_Name ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Visitor"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotVisitor = IntTotVisitor + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Visitor"] = "0";
                            }
                        }
                        drow["Total"] = Convert.ToString(intTotCount);
                        dtVisitor_Entry_Statistics.Rows.Add(drow);
                    }
                    gdrow = dtVisitor_Entry_Statistics.Rows.Count;
                    dicVisitor_Entry_Statistics.Add(gdrow, "Total");
                    drow = dtVisitor_Entry_Statistics.NewRow();
                    drow["S.No"] = "Total";
                    if (UserCat == "Student" || UserCat == "Student','Staff" || UserCat == "Student','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Student"] = Convert.ToString(IntTotStud);
                    }
                    if (UserCat == "Staff" || UserCat == "Student','Staff" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Staff"] = Convert.ToString(IntTotStaff);
                    }
                    if (UserCat == "Visitor" || UserCat == "Student','Visitor" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Visitor"] = Convert.ToString(IntTotVisitor);
                    }
                    Total = IntTotStud + IntTotStaff + IntTotVisitor;
                    drow["Total"] = Convert.ToString(Total);
                    dtVisitor_Entry_Statistics.Rows.Add(drow);
                    grdUserReport.DataSource = dtVisitor_Entry_Statistics;
                    grdUserReport.DataBind();
                    grdUserReport.Visible = true;
                    divSpreadReport.Visible = true;
                    //grdUserReport.Rows[0].BackColor = Color.FromArgb(12, 166, 202);
                    //grdUserReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                    //grdUserReport.Rows[0].Font.Bold = true;
                    print.Visible = true;
                    print1.Visible = false;
                    print2.Visible = false;
                    foreach (KeyValuePair<int, string> dr in dicVisitor_Entry_Statistics)
                    {
                        int g = dr.Key;
                        grdUserReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        grdUserReport.Rows[g].Cells[0].ColumnSpan = 2;
                        for (int a = 1; a < 2; a++)
                            grdUserReport.Rows[g].Cells[a].Visible = false;
                        grdUserReport.Rows[g].BackColor = Color.Green;
                        grdUserReport.Rows[g].Font.Bold = true;
                    }
                    dicVisitor_Entry_Statistics.Clear();
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "No Record found";
                }
            }
            #endregion


            #region Weekly

            if (rbweekly.Checked)
            {
                string month = string.Empty;
                selectQry = "SELECT datename(wk,entry_date) montno FROM LibUsers where 1=1 " + StrDate + " ";
                selectQry += " GROUP BY datename(wk,entry_date ) order by cast(datename(wk,entry_date) as numeric)";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selectQry, "text");

                if (dsload.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsload.Tables[0].Rows.Count; i++)
                    {
                        intTotCount = 0;
                        month = Convert.ToString(dsload.Tables[0].Rows[i]["montno"]);
                        sno++;
                        drow = dtVisitor_Entry_Statistics.NewRow();
                        drow["S.No"] = sno;
                        drow["Week"] = Convert.ToString(month);
                        //Student
                        if (UserCat == "Student" || UserCat == "Student','Staff" || UserCat == "Student','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Student' AND datename(wk,entry_date) ='" + month + "' " + StrDate + " " + StrTime + "";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            if (sem != "")
                            {
                                selectQry += " and current_semester in('" + CurrentSem + "')";
                            }
                            selectQry += " GROUP BY datename(wk,entry_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Student"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotStud = IntTotStud + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Student"] = "0";
                            }
                        }
                        //Staff
                        if (UserCat == "Staff" || UserCat == "Student','Staff" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Staff' AND datename(wk,entry_date) ='" + month + "' " + StrDate + " " + StrTime + "";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            selectQry += " GROUP BY datename(wk,entry_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Staff"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotStaff = IntTotStaff + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Staff"] = "0";
                            }
                        }
                        //'visitors
                        if (UserCat == "Visitor" || UserCat == "Student','Visitor" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Visitor' AND datename(wk,entry_date) ='" + month + "' " + StrDate + " " + StrTime + " ";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            selectQry += " GROUP BY datename(wk,entry_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Visitor"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotVisitor = IntTotVisitor + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Visitor"] = "0";
                            }
                        }
                        drow["Total"] = Convert.ToString(intTotCount);
                        dtVisitor_Entry_Statistics.Rows.Add(drow);
                    }
                    gdrow = dtVisitor_Entry_Statistics.Rows.Count;
                    dicVisitor_Entry_Statistics.Add(gdrow, "Total");
                    drow = dtVisitor_Entry_Statistics.NewRow();
                    drow["S.No"] = "Total";
                    if (UserCat == "Student" || UserCat == "Student','Staff" || UserCat == "Student','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Student"] = Convert.ToString(IntTotStud);
                    }
                    if (UserCat == "Staff" || UserCat == "Student','Staff" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Staff"] = Convert.ToString(IntTotStaff);
                    }
                    if (UserCat == "Visitor" || UserCat == "Student','Visitor" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Visitor"] = Convert.ToString(IntTotVisitor);
                    }
                    Total = IntTotStud + IntTotStaff + IntTotVisitor;
                    drow["Total"] = Convert.ToString(Total);
                    dtVisitor_Entry_Statistics.Rows.Add(drow);
                    grdUserReport.DataSource = dtVisitor_Entry_Statistics;
                    grdUserReport.DataBind();
                    grdUserReport.Visible = true;
                    divSpreadReport.Visible = true;

                    print.Visible = true;
                    print1.Visible = false;
                    print2.Visible = false;
                    foreach (KeyValuePair<int, string> dr in dicVisitor_Entry_Statistics)
                    {
                        int g = dr.Key;
                        grdUserReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        grdUserReport.Rows[g].Cells[0].ColumnSpan = 2;
                        for (int a = 1; a < 2; a++)
                            grdUserReport.Rows[g].Cells[a].Visible = false;
                        grdUserReport.Rows[g].BackColor = Color.Green;
                        grdUserReport.Rows[g].Font.Bold = true;
                    }
                    dicVisitor_Entry_Statistics.Clear();
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "No Record found";
                }
            }

            #endregion

            #region Monthly

            if (rbmonthly.Checked)
            {
                string monthVal = string.Empty;
                string monthName = string.Empty;
                string monthYear = string.Empty;
                selectQry = "SELECT datename(mm,entry_date) montname,month(entry_date) montno,datename(yyyy,entry_date) Year FROM LibUsers where 1=1 " + StrDate + " ";

                selectQry += " GROUP BY datename(yyyy,entry_date),datename(mm,entry_date ), month(entry_date) order by datename(yyyy,entry_date),month(entry_date) ";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selectQry, "text");

                if (dsload.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsload.Tables[0].Rows.Count; i++)
                    {
                        intTotCount = 0;
                        monthVal = Convert.ToString(dsload.Tables[0].Rows[i]["montno"]);
                        monthName = Convert.ToString(dsload.Tables[0].Rows[i]["montname"]);
                        monthYear = Convert.ToString(dsload.Tables[0].Rows[i]["Year"]);
                        sno++;
                        drow = dtVisitor_Entry_Statistics.NewRow();
                        drow["S.No"] = sno;
                        drow["Month"] = Convert.ToString(monthName);

                        //Student
                        if (UserCat == "Student" || UserCat == "Student','Staff" || UserCat == "Student','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Student' AND month(Entry_Date) ='" + monthVal + "' and year(Entry_Date) ='" + monthYear + "' " + StrDate + " " + StrTime + "";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            if (sem != "")
                            {
                                selectQry += " and current_semester in('" + CurrentSem + "')";
                            }
                            selectQry += " GROUP BY datename(mm,entry_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Student"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotStud = IntTotStud + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Student"] = "0";
                            }
                        }
                        //Staff
                        if (UserCat == "Staff" || UserCat == "Student','Staff" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Staff' AND month(Entry_Date) ='" + monthVal + "' and year(Entry_Date) ='" + monthYear + "' " + StrDate + " " + StrTime + "";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            selectQry += " GROUP BY datename(mm,entry_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Staff"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotStaff = IntTotStaff + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Staff"] = "0";
                            }
                        }
                        //'visitors
                        if (UserCat == "Visitor" || UserCat == "Student','Visitor" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Visitor' AND month(Entry_Date) ='" + monthVal + "' and year(Entry_Date) ='" + monthYear + "' " + StrDate + " " + StrTime + " ";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            selectQry += " GROUP BY datename(mm,entry_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Visitor"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotVisitor = IntTotVisitor + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Visitor"] = "0";
                            }
                        }
                        //OPAC
                        selectQry = "select  * from lib_queryhit WHERE month(lib_date) ='" + monthVal + "' and year(lib_date) ='" + monthYear + "'  and lib_date between '" + fromdate + "' and '" + todate + "' ";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectQry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            double opacVal = 0;
                            double OPacStat = 0;
                            for (int opac = 0; opac < ds.Tables[0].Rows.Count; opac++)
                            {
                                opacVal = Convert.ToDouble(ds.Tables[0].Rows[opac]["lib_count"]);
                                OPacStat = OPacStat + opacVal;
                            }
                            drow["Opac"] = OPacStat;
                            intTotCount = intTotCount + Convert.ToInt32(OPacStat);
                            IntTotOpac = IntTotOpac + Convert.ToInt32(OPacStat);
                        }
                        else
                        {
                            drow["Opac"] = "0";
                        }
                        drow["Total"] = Convert.ToString(intTotCount);
                        dtVisitor_Entry_Statistics.Rows.Add(drow);
                    }
                    drow = dtVisitor_Entry_Statistics.NewRow();
                    drow["S.No"] = "Total";
                    if (UserCat == "Student" || UserCat == "Student','Staff" || UserCat == "Student','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Student"] = Convert.ToString(IntTotStud);
                    }
                    if (UserCat == "Staff" || UserCat == "Student','Staff" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Staff"] = Convert.ToString(IntTotStaff);
                    }
                    if (UserCat == "Visitor" || UserCat == "Student','Visitor" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Visitor"] = Convert.ToString(IntTotVisitor);
                    }
                    drow["Opac"] = IntTotOpac;
                    gdrow = dtVisitor_Entry_Statistics.Rows.Count;
                    dicVisitor_Entry_Statistics.Add(gdrow, "Total");
                    Total = IntTotStud + IntTotStaff + IntTotVisitor + IntTotOpac;
                    drow["Total"] = Convert.ToString(Total);
                    dtVisitor_Entry_Statistics.Rows.Add(drow);
                    grdUserReport.DataSource = dtVisitor_Entry_Statistics;
                    grdUserReport.DataBind();
                    grdUserReport.Visible = true;
                    divSpreadReport.Visible = true;

                    print.Visible = true;
                    print1.Visible = false;
                    print2.Visible = false;
                    foreach (KeyValuePair<int, string> dr in dicVisitor_Entry_Statistics)
                    {
                        int g = dr.Key;
                        grdUserReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        grdUserReport.Rows[g].Cells[0].ColumnSpan = 2;
                        for (int a = 1; a < 2; a++)
                            grdUserReport.Rows[g].Cells[a].Visible = false;
                        grdUserReport.Rows[g].BackColor = Color.Green;
                        grdUserReport.Rows[g].Font.Bold = true;
                    }
                    dicVisitor_Entry_Statistics.Clear();

                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "No Record found";
                }
            }
            #endregion

            #region Yearly

            if (rbyearly.Checked)
            {
                string Year = string.Empty;
                selectQry = "SELECT datename(yyyy,entry_date) Year FROM LibUsers where 1=1 " + StrDate + " ";

                selectQry += "   GROUP BY datename(yyyy,entry_date ) order by datename(yyyy,entry_date)";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selectQry, "text");

                if (dsload.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsload.Tables[0].Rows.Count; i++)
                    {
                        intTotCount = 0;
                        Year = Convert.ToString(dsload.Tables[0].Rows[i]["Year"]);
                        sno++;
                        drow = dtVisitor_Entry_Statistics.NewRow();
                        drow["S.No"] = sno;
                        drow["Year"] = Convert.ToString(Year);
                        //Student
                        if (UserCat == "Student" || UserCat == "Student','Staff" || UserCat == "Student','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Student' AND year(Entry_Date) ='" + Year + "' " + StrDate + " " + StrTime + "";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            if (sem != "")
                            {
                                selectQry += " and current_semester in('" + CurrentSem + "')";
                            }
                            selectQry += " GROUP BY datename(yy,entry_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Student"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotStud = IntTotStud + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Student"] = "0";
                            }
                        }
                        //Staff
                        if (UserCat == "Staff" || UserCat == "Student','Staff" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Staff' AND year(Entry_Date) ='" + Year + "' " + StrDate + " " + StrTime + "";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            selectQry += " GROUP BY datename(yy,entry_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Staff"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotStaff = IntTotStaff + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Staff"] = "0";
                            }
                        }
                        //'visitors
                        if (UserCat == "Visitor" || UserCat == "Student','Visitor" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                        {
                            selectQry = "SELECT COUNT(*) as count FROM LibUsers WHERE UserCat = 'Visitor' AND year(Entry_Date) ='" + Year + "' " + StrDate + " " + StrTime + " ";
                            if (libraryCode != "")
                            {
                                selectQry += " and lib_code in('" + libraryCode + "')";
                            }
                            selectQry += " GROUP BY datename(yy,entry_date)";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                drow["Visitor"] = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                                intTotCount = intTotCount + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                                IntTotVisitor = IntTotVisitor + Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                            }
                            else
                            {
                                drow["Visitor"] = "0";
                            }
                        }
                        drow["Total"] = Convert.ToString(intTotCount);
                        dtVisitor_Entry_Statistics.Rows.Add(drow);
                    }
                    gdrow = dtVisitor_Entry_Statistics.Rows.Count;
                    dicVisitor_Entry_Statistics.Add(gdrow, "Total");
                    drow = dtVisitor_Entry_Statistics.NewRow();
                    drow["S.No"] = "Total";
                    if (UserCat == "Student" || UserCat == "Student','Staff" || UserCat == "Student','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Student"] = Convert.ToString(IntTotStud);
                    }
                    if (UserCat == "Staff" || UserCat == "Student','Staff" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Staff"] = Convert.ToString(IntTotStaff);
                    }
                    if (UserCat == "Visitor" || UserCat == "Student','Visitor" || UserCat == "Staff','Visitor" || UserCat == "Student','Staff','Visitor")
                    {
                        drow["Visitor"] = Convert.ToString(IntTotVisitor);
                    }
                    Total = IntTotStud + IntTotStaff + IntTotVisitor;
                    drow["Total"] = Convert.ToString(Total);
                    dtVisitor_Entry_Statistics.Rows.Add(drow);
                    grdUserReport.DataSource = dtVisitor_Entry_Statistics;
                    grdUserReport.DataBind();
                    grdUserReport.Visible = true;
                    divSpreadReport.Visible = true;

                    print.Visible = true;
                    print1.Visible = false;
                    print2.Visible = false;
                    foreach (KeyValuePair<int, string> dr in dicVisitor_Entry_Statistics)
                    {
                        int g = dr.Key;
                        grdUserReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        grdUserReport.Rows[g].Cells[0].ColumnSpan = 2;
                        for (int a = 1; a < 2; a++)
                            grdUserReport.Rows[g].Cells[a].Visible = false;
                        grdUserReport.Rows[g].BackColor = Color.Green;
                        grdUserReport.Rows[g].Font.Bold = true;
                    }
                    dicVisitor_Entry_Statistics.Clear();
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "No Record found";
                }
            }
            #endregion

        }
        catch (Exception ex)
        {
           // d2.sendErrorMail(ex, collegecode, "UserInOutEntry");
        }
    }

    protected void btnViewCurrIn_Click(object sender, EventArgs e)
    {
        try
        {
            print.Visible = false;
            divPhoto.Visible = true;
            string selectQry = string.Empty;
            string Date = DateTime.Now.ToString("MM/dd/yyy");
            string rollNo = string.Empty;
            string Name = string.Empty;
            string entryTime = string.Empty;

            ArrayList arRollNo = new ArrayList();
            arRollNo.Add(LblRollNo1);
            arRollNo.Add(LblRollNo2);
            arRollNo.Add(LblRollNo3);
            arRollNo.Add(LblRollNo4);
            arRollNo.Add(LblRollNo5);
            arRollNo.Add(LblRollNo6);
            arRollNo.Add(LblRollNo7);
            arRollNo.Add(LblRollNo8);
            ArrayList arName = new ArrayList();
            arName.Add(LblStudName1);
            arName.Add(LblStudName2);
            arName.Add(LblStudName3);
            arName.Add(LblStudName4);
            arName.Add(LblStudName5);
            arName.Add(LblStudName6);
            arName.Add(LblStudName7);
            arName.Add(LblStudName8);
            ArrayList arImage = new ArrayList();
            arImage.Add(img_stud1);
            arImage.Add(img_stud2);
            arImage.Add(img_stud3);
            arImage.Add(img_stud4);
            arImage.Add(img_stud5);
            arImage.Add(img_stud6);
            arImage.Add(img_stud7);
            arImage.Add(img_stud8);

            selectQry = "SELECT * FROM LibUsers L,Registration R WHERE L.Roll_No = R.Roll_No AND Entry_Date ='" + Date + "' AND Exit_Time = '' ORDER BY Entry_Time ";

            dsload.Clear();
            dsload = d2.select_method_wo_parameter(selectQry, "text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsload.Tables[0].Rows.Count; i++)
                {
                    rollNo = Convert.ToString(dsload.Tables[0].Rows[i]["roll_no"]);
                    entryTime = Convert.ToString(dsload.Tables[0].Rows[i]["entry_time"]);
                    Name = Convert.ToString(dsload.Tables[0].Rows[i]["Stud_Name"]);
                    Lst_PhotoList.Items.Clear();
                    Lst_PhotoList.Items.Add(rollNo + "-" + entryTime + "$" + Name + "$" + "~/Handler/Handler4.ashx?rollno=" + rollNo);
                }
                if (Lst_PhotoList.Items.Count > 0)
                {
                    int ListCount = Lst_PhotoList.Items.Count;
                    if (ListCount > 8)
                        ListCount = 7;
                    for (int j = 0; j <= ListCount; j++)
                    {
                        Label LbRoll = (Label)arRollNo[j];
                        Label LbName = (Label)arName[j];
                        System.Web.UI.WebControls.Image aa = (System.Web.UI.WebControls.Image)arImage[j];
                        string PhotoList = Lst_PhotoList.Items[j].Text;
                        string[] photo = PhotoList.Split('$');
                        LbRoll.Text = photo[0];
                        LbName.Text = photo[1];
                        aa.ImageUrl = photo[2];
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void BtnFirst_Click(object sender, EventArgs e)
    {
        int j = 0;
        string StrRoll = string.Empty;

        ArrayList arRollNo = new ArrayList();
        arRollNo.Add(LblRollNo1);
        arRollNo.Add(LblRollNo2);
        arRollNo.Add(LblRollNo3);
        arRollNo.Add(LblRollNo4);
        arRollNo.Add(LblRollNo5);
        arRollNo.Add(LblRollNo6);
        arRollNo.Add(LblRollNo7);
        arRollNo.Add(LblRollNo8);
        ArrayList arName = new ArrayList();
        arName.Add(LblStudName1);
        arName.Add(LblStudName2);
        arName.Add(LblStudName3);
        arName.Add(LblStudName4);
        arName.Add(LblStudName5);
        arName.Add(LblStudName6);
        arName.Add(LblStudName7);
        arName.Add(LblStudName8);
        ArrayList arImage = new ArrayList();
        arImage.Add(img_stud1);
        arImage.Add(img_stud2);
        arImage.Add(img_stud3);
        arImage.Add(img_stud4);
        arImage.Add(img_stud5);
        arImage.Add(img_stud6);
        arImage.Add(img_stud7);
        arImage.Add(img_stud8);

        for (int i = 0; i <= 7; i++)
        {
            StrRoll = Lst_PhotoList.Items[j].Text;
            string[] Stud = StrRoll.Split('$');
            int StrLength = Stud.Length;
            if (StrLength > 0)
            {
                Label LbRoll = (Label)arRollNo[j];
                Label LbName = (Label)arName[j];
                System.Web.UI.WebControls.Image aa = (System.Web.UI.WebControls.Image)arImage[j];
                string PhotoList = Lst_PhotoList.Items[j].Text;
                string[] photo = PhotoList.Split('$');
                LbRoll.Text = photo[0];
                LbName.Text = photo[1];
                aa.ImageUrl = photo[2];
                //        Lbl_RegNo(i).Caption = StrRoll(0) & "-" & StrRoll(1)
                //        Lbl_Stud_Name(j).Caption = StrRoll(2)
                //        Pic_Photo(j).Picture = LoadPicture(photoAccess(photoGet, Student, Lst_PhotoList.ItemData(j)))
                //         Lbl_RegNo(j).Tag = j
                //         j = j + 1
            }
        }
    }

    protected void BtnPrev_Click(object sender, EventArgs e)
    {
        //        Dim j As Integer
        //Dim StrRoll() As String
        //Dim intLast As Integer
        //intLast = val(Lbl_RegNo(7).Tag) + 1
        //j = val(intLast)
        //For i = 0 To 7
        //    If Lst_PhotoList.listcount - 1 > j And j >= 0 Then
        //        StrRoll = Split(Lst_PhotoList.List(j), "-")
        //        If UBound(StrRoll) >= 1 Then
        //            Lbl_RegNo(i).Caption = StrRoll(0) & "-" & StrRoll(1)
        //            Lbl_Stud_Name(i).Caption = StrRoll(2)
        //            Pic_Photo(i).Picture = LoadPicture(photoAccess(photoGet, Student, Lst_PhotoList.ItemData(j)))
        //             Lbl_RegNo(i).Tag = j
        //             j = j - 1
        //        End If
        //    End If
        //Next i
    }

    protected void BtnNext_Click(object sender, EventArgs e)
    {
        ArrayList arRollNo = new ArrayList();
        arRollNo.Add(LblRollNo1);
        arRollNo.Add(LblRollNo2);
        arRollNo.Add(LblRollNo3);
        arRollNo.Add(LblRollNo4);
        arRollNo.Add(LblRollNo5);
        arRollNo.Add(LblRollNo6);
        arRollNo.Add(LblRollNo7);
        arRollNo.Add(LblRollNo8);
        ArrayList arName = new ArrayList();
        arName.Add(LblStudName1);
        arName.Add(LblStudName2);
        arName.Add(LblStudName3);
        arName.Add(LblStudName4);
        arName.Add(LblStudName5);
        arName.Add(LblStudName6);
        arName.Add(LblStudName7);
        arName.Add(LblStudName8);
        ArrayList arImage = new ArrayList();
        arImage.Add(img_stud1);
        arImage.Add(img_stud2);
        arImage.Add(img_stud3);
        arImage.Add(img_stud4);
        arImage.Add(img_stud5);
        arImage.Add(img_stud6);
        arImage.Add(img_stud7);
        arImage.Add(img_stud8);
        int j = 0;
        string StrRoll = string.Empty;
        clickCount++;
        int listValuesCount = Lst_PhotoList.Items.Count;
        int printedCount = 0;
        if (listValuesCount - (clickCount * 8) > 8)
            printedCount = 8;
        else
            printedCount = listValuesCount - (clickCount * 8);
        int startingIndex = (clickCount * 8);

        for (int i = (startingIndex + 1); i < (startingIndex + printedCount); i++)
        {
            Label LbRoll = (Label)arRollNo[i];
            Label LbName = (Label)arName[i];
            System.Web.UI.WebControls.Image aa = (System.Web.UI.WebControls.Image)arImage[i];
            string PhotoList = Lst_PhotoList.Items[i].Text;
            string[] photo = PhotoList.Split('$');
            LbRoll.Text = photo[0];
            LbName.Text = photo[1];
            aa.ImageUrl = photo[2];
        }
    }

    protected void BtnLast_Click(object sender, EventArgs e)
    {
        //        Dim j As Integer
        //Dim StrRoll() As String

        //j = Lst_PhotoList.listcount - 1
        //For i = 0 To 7
        //    StrRoll = Split(Lst_PhotoList.List(j), "-")
        //    If UBound(StrRoll) >= 1 Then
        //        Lbl_RegNo(i).Caption = StrRoll(0) & "-" & StrRoll(1)
        //        Lbl_Stud_Name(i).Caption = StrRoll(2)
        //        Pic_Photo(i).Picture = LoadPicture(photoAccess(photoGet, Student, Lst_PhotoList.ItemData(j)))
        //         Lbl_RegNo(i).Tag = j
        //         j = j - 1
        //    End If
        //Next i

    }

    #region Print

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grdUserReport, reportname);
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
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "User In/Out Entry " + '@';
            pagename = "UserInOutEntry.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrolhed.loadspreaddetails(grdUserReport, pagename, degreedetails, 0, ss, "1");
            Printcontrolhed.Visible = true;
        }
        catch { }
    }

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(GrdVisitWithTime, reportname);
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
    public void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "User In/Out Entry " + '@';
            pagename = "UserInOutEntry.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrolhed1.loadspreaddetails(GrdVisitWithTime, pagename, degreedetails, 0, ss, "1");

            Printcontrolhed1.Visible = true;
        }
        catch { }
    }

    protected void btnExcel2_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grdVisit_Details, reportname);
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
    public void btnprintmaster2_Click(object sender, EventArgs e)
    {
        try
        {
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "User In/Out Entry " + '@';
            pagename = "UserInOutEntry.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrolhed2.loadspreaddetails(grdVisit_Details, pagename, degreedetails, 0, ss, "1");
            Printcontrolhed2.Visible = true;
        }
        catch { }
    }
    #endregion

    #region Delete

    protected void btndelete2_Click(object sender, EventArgs e)
    {
        try
        {
            int selectedcount = 0;

            foreach (GridViewRow gvrow in grdVisit_Details.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                if (chk.Checked == true)
                {
                    selectedcount++;
                }
            }
            if (selectedcount == 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Select atleast one entry to delete";
                return;
            }
            else
            {
                Diveleterecord.Visible = true;
                lbl_Diveleterecord.Text = "Are you sure to delete the User Entry?";
            }
        }
        catch
        {
        }
    }

    protected void btn_detele_yes__record_Click(object sender, EventArgs e)
    {
        try
        {
            int deletere = 0;
            string roll_no = "";
            string entrytime = "";
            string std_name = "";
            string entry_date = "";
            string deletebook = "";
            foreach (GridViewRow gvrow in grdVisit_Details.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                if (chk.Checked == true)
                {
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    roll_no = Convert.ToString(grdVisit_Details.Rows[RowCnt].Cells[4].Text);
                    std_name = Convert.ToString(grdVisit_Details.Rows[RowCnt].Cells[6].Text);
                    entry_date = Convert.ToString(grdVisit_Details.Rows[RowCnt].Cells[8].Text);
                    entrytime = Convert.ToString(grdVisit_Details.Rows[RowCnt].Cells[9].Text);
                    string[] En_date = entry_date.Split('/');
                    if (En_date.Length == 3)
                        entry_date = En_date[1].ToString() + "/" + En_date[0].ToString() + "/" + En_date[2].ToString();
                    if (checkusers.SelectedIndex == 2)
                    {
                        deletebook = "DELETE from libusers where entry_time='" + entrytime + "' and stud_name='" + std_name + "' and entry_date='" + entry_date + "' and usercat='Visitor'";
                    }
                    else
                    {
                        deletebook = "DELETE from libusers where roll_no='" + roll_no + "' and entry_time='" + entrytime + "' and stud_name='" + std_name + "' and entry_date='" + entry_date + "'";
                    }
                    deletere = d2.update_method_wo_parameter(deletebook, "Text");
                }
            }
            if (deletere > 0)
            {
                Diveleterecord.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Text = "User Entry Deleted Sucessfully";
                btngo_Click(sender, e);
            }
        }
        catch
        {

        }
    }

    protected void btn_detele_no__recordClick(object sender, EventArgs e)
    {
        try
        {
            Diveleterecord.Visible = false;
        }
        catch
        {
        }

    }

    #endregion

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
                string[] groupUser = group_user.Split(';');
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

    protected void grdUserReport_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }


}