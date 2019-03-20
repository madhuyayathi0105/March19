using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class LibraryMod_Card_Lock_Unlock : System.Web.UI.Page
{
    #region Field_Declaration
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    Dictionary<string, string> dicStaffList = new Dictionary<string, string>();
    Dictionary<string, string> dicSQLParameter = new Dictionary<string, string>();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    Dictionary<string, string> dictitle = new Dictionary<string, string>();
    DataSet dsprint = new DataSet();
    ArrayList colord = new ArrayList();
    DAccess2 da = new DAccess2();
    DataView dvhead = new DataView();
    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();
    DataTable dtCommon = new DataTable();
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string username1 = string.Empty;
    string groupUserCode = string.Empty;
    string collcode = string.Empty;
    string libcode = string.Empty;
    string libname = string.Empty;
    string activerow = "";
    string activecol = "";
    int selectedcount = 0;
    string strID = "";
    string strStaffID = "";
    string category = "";
    string depart = "";
    DataTable bokcard = new DataTable();
    DataRow drbokcardlock;
    static int searchby = 1;
    static string searchclgcode = string.Empty;
    DataTable dtlock = new DataTable();
    DataRow drlock;
    #endregion

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
            username1 = d2.GetFunction("select User_id from UserMaster where User_Code='" + userCode + "' ");
            if (username1 != "" && username1 != "0")
            {
                Session["username1"] = username1;
            }

            if (!IsPostBack)
            {
                Bindcollege();
                loadCategory();
                Department();
                subyear();
                grdCardLock.Visible = false;
                grdBorrowerDet.Visible = false;
                rptprint.Visible = false;
                btn_Lock.Visible = false;
                fieldborrow.Visible = false;
            }
        }
        catch (Exception ex) { }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getsearch(string prefixText)
    {
        string query = "";
        WebService ws = new WebService();
        List<string> values = new List<string>();
        if (searchby == 1)
        {
            query = "SELECT DISTINCT  TOP  100 Roll_No FROM Registration where Roll_No Like '" + prefixText + "%'  AND college_code='" + searchclgcode + "' order by Roll_No";
        }
        else if (searchby == 2)
        {
            query = "SELECT DISTINCT  TOP  100 staff_code FROM staffmaster where staff_code Like '" + prefixText + "%'  AND college_code='" + searchclgcode + "' order by staff_code";
        }
        if (searchby != 0)
            values = ws.Getname(query);
        return values;
    }

    #region College

    public void Bindcollege()
    {
        try
        {
            ddlCollege.Items.Clear();
            dtCommon.Clear();
            ddlCollege.Enabled = false;
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
                ddlCollege.DataSource = dtCommon;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.SelectedIndex = 0;
                ddlCollege.Enabled = true;


                searchclgcode = Convert.ToString(ddlCollege.SelectedValue);

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Card_Lock_Unlock"); }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdCardLock.Visible = false;
        grdBorrowerDet.Visible = false;
        rptprint.Visible = false;
        btn_Lock.Visible = false;
        fieldborrow.Visible = false;

        searchclgcode = Convert.ToString(ddlCollege.SelectedValue);
    }

    #endregion

    #region Category
    public void loadCategory()
    {
        try
        {
            ddl_Category.Items.Add("Student");
            ddl_Category.Items.Add("Staff");
            ddl_Category.Items.Add("All");
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Card_Lock_Unlock"); }

    }

    protected void ddl_Category_SelectedIndexChanged(object sender, EventArgs e)
    {
        Department();
        if (ddl_Category.SelectedIndex == 0)
        {
            lbl_year.Visible = true;
            ddl_year.Visible = true;
            lbl_rollno.Visible = true;
            lbl_staffcode.Visible = false;

            searchby = 1;
        }
        else if (ddl_Category.SelectedIndex == 1)
        {
            lbl_year.Visible = false;
            ddl_year.Visible = false;
            lbl_rollno.Visible = false;
            lbl_staffcode.Visible = true;

            searchby = 2;
        }
        else
        {
            lbl_year.Visible = false;
            ddl_year.Visible = false;
            lbl_rollno.Visible = true;
            lbl_staffcode.Visible = false;

            searchby = 0;
        }
        grdCardLock.Visible = false;
        grdBorrowerDet.Visible = false;
        rptprint.Visible = false;
        btn_Lock.Visible = false;
        fieldborrow.Visible = false;

    }
    #endregion

    #region Department
    public void Department()
    {
        try
        {

            string College = ddlCollege.SelectedValue.ToString();
            string loaddept = "";
            stdstaff();
            if (!string.IsNullOrEmpty(College))
            {
                if (ddl_Category.SelectedIndex == 1)
                {
                    loaddept = "select DISTINCT isnull(Dept_name,'') Dept_name from tokendetails where is_staff=1  union select department from user_master";
                }
                else
                {
                    loaddept = "select DISTINCT isnull(Dept_name,'') Dept_name from tokendetails,degree,registration where is_staff=0 and degree.degree_code=registration.degree_code and " + strID + "=tokendetails.roll_no  and degree.college_code='" + College + "'";
                }

                ds.Clear();
                ds = da.select_method_wo_parameter(loaddept, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_dept.DataSource = ds;
                    ddl_dept.DataTextField = "Dept_Name";
                    ddl_dept.DataValueField = "Dept_Name";
                    ddl_dept.DataBind();
                    ddl_dept.Items.Insert(0, "All");
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Card_Lock_Unlock"); }

    }

    protected void ddl_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdCardLock.Visible = false;
            grdBorrowerDet.Visible = false;
            rptprint.Visible = false;
            btn_Lock.Visible = false;
            fieldborrow.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Card_Lock_Unlock"); }


    }


    #endregion

    #region Sub.Year
    public void subyear()
    {
        try
        {

            ddl_year.Items.Clear();
            ds = da.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddl_year.DataSource = ds;
                ddl_year.DataTextField = "batch_year";
                ddl_year.DataValueField = "batch_year";
                ddl_year.DataBind();
            }
            int count1 = ds.Tables[1].Rows.Count;
            if (count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                ddl_year.SelectedValue = max_bat.ToString();
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Card_Lock_Unlock"); }

    }

    protected void ddl_year_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdCardLock.Visible = false;
        grdBorrowerDet.Visible = false;
        rptprint.Visible = false;
        btn_Lock.Visible = false;
        fieldborrow.Visible = false;

    }
    #endregion

    #region lockAndUnlockCard
    protected void chkcard_CheckedChanged(object sender, EventArgs e)
    {
        grdCardLock.Visible = false;
        grdBorrowerDet.Visible = false;
        rptprint.Visible = false;
        btn_Lock.Visible = false;
        fieldborrow.Visible = false;
    }

    protected void rblcard_Selected(object sender, EventArgs e)
    {
        grdCardLock.Visible = false;
        grdBorrowerDet.Visible = false;
        rptprint.Visible = false;
        btn_Lock.Visible = false;
        fieldborrow.Visible = false;
    }
    #endregion

    #region Filterstdstaff
    public void stdstaff()
    {
        try
        {
            string linkvalue = d2.GetFunction("select LinkValue from inssettings where linkname ='Library id'");
            if (linkvalue != "")
            {
                if (linkvalue == "0")
                {
                    strID = "registration.roll_no";
                    strStaffID = "staffmaster.staff_code";
                }
                else
                {
                    strID = "registration.lib_id";
                    strStaffID = "staffmaster.lib_id";
                }
            }
            else
            {
                strID = "registration.roll_no";
                strStaffID = "staffmaster.staff_code";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Card_Lock_Unlock"); }


    }
    #endregion

    protected void grdCardLock_onpageindexchanged(object sender, GridViewPageEventArgs e)
    {
        grdCardLock.PageIndex = e.NewPageIndex;
        btngo_Click(sender, e);
    }

    #region Go

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddl_Category.Items.Count > 0)
                category = Convert.ToString(ddl_Category.SelectedValue);
            if (ddl_dept.Items.Count > 0)
                depart = Convert.ToString(ddl_dept.SelectedValue);
            if (category == "" && depart == "")
            {
                if (category == "")
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Select The Category ";
                    return;
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Select The Department ";
                    return;
                }

            }
            ds = getDetails();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                loadspread(ds);
            }
            else
            {

                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Records Found";
                grdCardLock.Visible = false;
                rptprint.Visible = false;
                btn_Lock.Visible = false;
                fieldborrow.Visible = false;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Card_Lock_Unlock"); }
    }

    protected void grdCardLock_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (rblcard.SelectedIndex == 0)
                {
                    if (ddl_Category.SelectedIndex == 0 || ddl_Category.SelectedIndex == 2)
                    {
                        e.Row.Cells[0].Visible = true;
                        e.Row.Cells[1].Visible = true;
                        e.Row.Cells[2].Visible = true;
                        e.Row.Cells[3].Visible = false;
                        e.Row.Cells[4].Visible = true;
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = true;
                        e.Row.Cells[7].Visible = true;
                        e.Row.Cells[8].Visible = true;
                        e.Row.Cells[9].Visible = true;
                        e.Row.Cells[8].Enabled = false;
                    }
                    else if (ddl_Category.SelectedIndex == 1)
                    {
                        e.Row.Cells[0].Visible = true;
                        e.Row.Cells[1].Visible = true;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[3].Visible = true;
                        e.Row.Cells[4].Visible = false;
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = true;
                        e.Row.Cells[7].Visible = true;
                        e.Row.Cells[8].Visible = true;
                        e.Row.Cells[9].Visible = true;
                        e.Row.Cells[8].Enabled = false;
                    }
                }
                if (chkcard.Checked == true)
                {
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = true;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = true;
                    e.Row.Cells[8].Visible = true;
                    e.Row.Cells[9].Visible = false;
                }
                if (rblcard.SelectedIndex == 1)
                {
                    if (ddl_Category.SelectedIndex == 0 || ddl_Category.SelectedIndex == 2)
                    {
                        e.Row.Cells[0].Visible = true;
                        e.Row.Cells[1].Visible = true;
                        e.Row.Cells[2].Visible = true;
                        e.Row.Cells[3].Visible = false;
                        e.Row.Cells[4].Visible = true;
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = true;
                        e.Row.Cells[7].Visible = true;
                        e.Row.Cells[8].Visible = true;
                        e.Row.Cells[9].Visible = false;
                    }
                    else if (ddl_Category.SelectedIndex == 1)
                    {
                        e.Row.Cells[0].Visible = true;
                        e.Row.Cells[1].Visible = true;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[3].Visible = true;
                        e.Row.Cells[4].Visible = true;
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = true;
                        e.Row.Cells[7].Visible = true;
                        e.Row.Cells[8].Visible = true;
                        e.Row.Cells[9].Visible = false;
                    }

                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (rblcard.SelectedIndex == 0)
                {
                    if (ddl_Category.SelectedIndex == 0 || ddl_Category.SelectedIndex == 2)
                    {
                        e.Row.Cells[0].Visible = true;
                        e.Row.Cells[1].Visible = true;
                        e.Row.Cells[2].Visible = true;
                        e.Row.Cells[3].Visible = false;
                        e.Row.Cells[4].Visible = true;
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = true;
                        e.Row.Cells[7].Visible = true;
                        e.Row.Cells[8].Visible = true;
                        e.Row.Cells[9].Visible = true;
                        e.Row.Cells[8].Enabled = false;
                    }
                    else if (ddl_Category.SelectedIndex == 1)
                    {
                        e.Row.Cells[0].Visible = true;
                        e.Row.Cells[1].Visible = true;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[3].Visible = true;
                        e.Row.Cells[4].Visible = false;
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = true;
                        e.Row.Cells[7].Visible = true;
                        e.Row.Cells[8].Visible = true;
                        e.Row.Cells[9].Visible = true;
                        e.Row.Cells[8].Enabled = false;
                    }
                }
                if (chkcard.Checked == true)
                {
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = true;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = true;
                    e.Row.Cells[8].Visible = true;
                    e.Row.Cells[9].Visible = false;
                }
                if (rblcard.SelectedIndex == 1)
                {
                    if (ddl_Category.SelectedIndex == 0 || ddl_Category.SelectedIndex == 2)
                    {
                        e.Row.Cells[0].Visible = true;
                        e.Row.Cells[1].Visible = true;
                        e.Row.Cells[2].Visible = true;
                        e.Row.Cells[3].Visible = false;
                        e.Row.Cells[4].Visible = true;
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = true;
                        e.Row.Cells[7].Visible = true;
                        e.Row.Cells[8].Visible = true;
                        e.Row.Cells[9].Visible = false;
                    }
                    else if (ddl_Category.SelectedIndex == 1)
                    {
                        e.Row.Cells[0].Visible = true;
                        e.Row.Cells[1].Visible = true;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[3].Visible = true;
                        e.Row.Cells[4].Visible = true;
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = true;
                        e.Row.Cells[7].Visible = true;
                        e.Row.Cells[8].Visible = true;
                        e.Row.Cells[9].Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }

    }

    private DataSet getDetails()
    {
        DataSet dsstdstaff = new DataSet();
        try
        {
            #region get Value

            string getrecord = "";
            string qrydept = "";
            string qryroll = "";
            string Batchyear = "";
            if (ddlCollege.Items.Count > 0)
                collcode = Convert.ToString(ddlCollege.SelectedValue);
            if (ddl_Category.Items.Count > 0)
                category = Convert.ToString(ddl_Category.SelectedValue);
            if (ddl_dept.Items.Count > 0)
                depart = Convert.ToString(ddl_dept.SelectedValue);
            if (ddl_year.Items.Count > 0)
                Batchyear = Convert.ToString(ddl_year.SelectedValue);
            stdstaff();
            if (category != "" && depart != "")
            {
                if (depart != "All")
                    qrydept = " and dept_name like '" + depart + "'";
                if (txt_roll_staff.Text != "")
                    qryroll = "and tokendetails.roll_no like '" + txt_roll_staff.Text + "%'";
                if (chkcard.Checked == false)
                {
                    if (ddl_Category.SelectedIndex == 1)
                    {

                        if (rblcard.SelectedIndex == 0)
                        {
                            getrecord = "SELECT distinct tokendetails.roll_no,tokendetails.stud_name,tokendetails.dept_name,count(tokendetails.roll_no) tokendetails,tokendetails.reas_loc,locked_by from tokendetails,staffmaster where is_staff =1 and  " + strStaffID + "=tokendetails.roll_no and resign=0  and is_locked=2 " + qryroll + qrydept + "  group by tokendetails.roll_no, tokendetails.stud_name,tokendetails.dept_name,reas_loc,locked_by order by dept_name,tokendetails.stud_name";
                        }
                        else
                        {
                            getrecord = "SELECT distinct tokendetails.roll_no,tokendetails.stud_name,tokendetails.dept_name,count(tokendetails.roll_no) tokendetails ,tokendetails.reas_loc from tokendetails,staffmaster where is_staff =1 and  " + strStaffID + "=tokendetails.roll_no and resign=0 and is_locked <> 2 " + qryroll + qrydept + " group by tokendetails.roll_no, tokendetails.stud_name,tokendetails.dept_name,tokendetails.reas_loc order by dept_name,tokendetails.stud_name";

                        }
                    }
                    else if (ddl_Category.SelectedIndex == 0)
                    {

                        if (rblcard.SelectedIndex == 0)
                        {
                            getrecord = "SELECT distinct tokendetails.roll_no,tokendetails.stud_name,tokendetails.dept_name,count(tokendetails.roll_no) tokendetails,tokendetails.reas_loc,locked_by from tokendetails,registration where is_staff<>1 and " + strID + "=tokendetails.roll_no and is_locked=2  " + qryroll + qrydept + " and batch_year ='" + Batchyear + "' group by tokendetails.roll_no, tokendetails.stud_name,tokendetails.dept_name,reas_loc,locked_by order by dept_name,tokendetails.stud_name";
                        }
                        else
                        {
                            getrecord = "SELECT distinct tokendetails.roll_no, tokendetails.stud_name,tokendetails.dept_name,count(tokendetails.roll_no)  tokendetails from tokendetails,registration,degree where is_staff<>1 and " + strID + "=tokendetails.roll_no and is_locked in('1','0') " + qryroll + qrydept + " and batch_year ='" + Batchyear + "' and registration.degree_code=degree.degree_code and degree.college_code='" + collcode + "' group by tokendetails.roll_no, tokendetails.stud_name,tokendetails.dept_name order by dept_name,tokendetails.stud_name";
                        }
                    }
                    else
                    {
                        if (rblcard.SelectedIndex == 0)
                        {
                            getrecord = "SELECT distinct tokendetails.roll_no,tokendetails.stud_name,tokendetails.dept_name,count(tokendetails.roll_no) tokendetails,tokendetails.reas_loc,locked_by from tokendetails where is_locked=2  " + qryroll + qrydept + " group by tokendetails.roll_no, tokendetails.stud_name,tokendetails.dept_name,reas_loc,locked_by order by dept_name,tokendetails.stud_name";
                        }
                        else
                        {
                            getrecord = "SELECT distinct tokendetails.roll_no, tokendetails.stud_name,tokendetails.dept_name,count(tokendetails.roll_no) tokendetails,tokendetails.reas_loc from tokendetails,degree,registration where is_locked in('1','0') " + qryroll + qrydept + " and batch_year ='" + Batchyear + "'and degree.degree_code=registration.degree_code and " + strID + "=tokendetails.roll_no  and degree.college_code='" + collcode + "' group by tokendetails.roll_no, tokendetails.stud_name,tokendetails.dept_name,tokendetails.reas_loc order by dept_name,tokendetails.stud_name";

                        }

                    }

                }
                else
                {
                    if (ddl_Category.SelectedIndex == 1)
                    {

                        if (rblcard.SelectedIndex == 0)
                        {
                            getrecord = "SELECT distinct tokendetails.roll_no,tokendetails.stud_name,tokendetails.token_no,tokendetails.dept_name,count(tokendetails.roll_no) tokendetails,tokendetails.reas_loc,locked_by from tokendetails,staffmaster where is_staff =1 and  " + strStaffID + "=tokendetails.roll_no and resign=0  and is_locked=2 " + qryroll + qrydept + " group by tokendetails.roll_no, tokendetails.stud_name,tokendetails.dept_name,reas_loc,locked_by,tokendetails.token_no order by dept_name,tokendetails.stud_name";
                        }
                        else
                        {
                            getrecord = "SELECT distinct tokendetails.roll_no,tokendetails.stud_name,tokendetails.token_no,tokendetails.dept_name,count(tokendetails.roll_no) tokendetails from tokendetails,staffmaster where is_staff =1 and  " + strStaffID + "=tokendetails.roll_no and resign=0 and is_locked <> 2 " + qryroll + qrydept + "  group by tokendetails.roll_no, tokendetails.stud_name,tokendetails.dept_name,tokendetails.token_no order by dept_name,tokendetails.stud_name";

                        }
                    }
                    else if (ddl_Category.SelectedIndex == 0)
                    {

                        if (rblcard.SelectedIndex == 0)
                        {
                            getrecord = "SELECT distinct tokendetails.roll_no,tokendetails.stud_name,tokendetails.token_no,tokendetails.dept_name,count(tokendetails.roll_no) tokendetails,tokendetails.reas_loc,locked_by from tokendetails,registration where is_staff<>1 and " + strID + "=tokendetails.roll_no and is_locked=2 " + qryroll + qrydept + "  and batch_year ='" + Batchyear + "' group by tokendetails.roll_no, tokendetails.stud_name,tokendetails.dept_name,reas_loc,locked_by,tokendetails.token_no order by dept_name,tokendetails.stud_name";
                        }
                        else
                        {
                            getrecord = "SELECT distinct tokendetails.roll_no, tokendetails.stud_name,tokendetails.token_no,tokendetails.dept_name,count(tokendetails.roll_no) tokendetails from tokendetails,registration,degree where is_staff<>1 and " + strID + "=tokendetails.roll_no and is_locked in('1','0') " + qryroll + qrydept + " and batch_year ='" + Batchyear + "' and registration.degree_code=degree.degree_code and degree.college_code='" + collcode + "' group by tokendetails.roll_no, tokendetails.stud_name,tokendetails.dept_name,tokendetails.token_no order by dept_name,tokendetails.stud_name";

                        }

                    }
                    else
                    {
                        if (rblcard.SelectedIndex == 0)
                        {
                            getrecord = "SELECT distinct tokendetails.roll_no,tokendetails.stud_name,tokendetails.token_no,tokendetails.dept_name,count(tokendetails.roll_no) tokendetails,tokendetails.reas_loc,locked_by from tokendetails where is_locked=2  " + qryroll + qrydept + "  group by tokendetails.roll_no, tokendetails.stud_name,tokendetails.dept_name,reas_loc,locked_by,tokendetails.token_no order by dept_name,tokendetails.stud_name";
                        }
                        else
                        {
                            getrecord = "SELECT distinct tokendetails.roll_no, tokendetails.stud_name,tokendetails.token_no,tokendetails.dept_name,count(tokendetails.roll_no) tokendetails,tokendetails.reas_loc from tokendetails where is_locked in('1','0') " + qryroll + qrydept + "  group by tokendetails.roll_no, tokendetails.stud_name,tokendetails.dept_name,tokendetails.token_no order by dept_name,tokendetails.stud_name";

                        }

                    }


                }
                dsstdstaff.Clear();
                dsstdstaff = d2.select_method_wo_parameter(getrecord, "Text");
            }




            #endregion
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Card_Lock_Unlock"); }


        return dsstdstaff;
    }

    public void loadspread(DataSet dsnews)
    {
        try
        {
            if (dsnews.Tables.Count > 0 && dsnews.Tables[0].Rows.Count > 0)
            {
                bokcard.Columns.Add("Roll No");
                bokcard.Columns.Add("Staff Code");
                bokcard.Columns.Add("Name");
                bokcard.Columns.Add("Token No");
                bokcard.Columns.Add("Course");
                bokcard.Columns.Add("No Of Cards");
                bokcard.Columns.Add("Reason");
                bokcard.Columns.Add("Locked By");
                int sno = 0;
                string reason = "";
                string lockby = "";
                string Tokenos = "";
                for (int row = 0; row < dsnews.Tables[0].Rows.Count; row++)
                {
                    sno++;
                    drbokcardlock = bokcard.NewRow();
                    string rollno = Convert.ToString(dsnews.Tables[0].Rows[row]["roll_no"]).Trim();
                    string stdname = Convert.ToString(dsnews.Tables[0].Rows[row]["stud_name"]).Trim();
                    string deptname = Convert.ToString(dsnews.Tables[0].Rows[row]["dept_name"]).Trim();
                    string tokendetails = Convert.ToString(dsnews.Tables[0].Rows[row]["tokendetails"]);
                    if (chkcard.Checked == true)
                    {
                        Tokenos = Convert.ToString(dsnews.Tables[0].Rows[row]["token_no"]);
                    }

                    if (rblcard.SelectedIndex == 0)
                    {
                        reason = Convert.ToString(dsnews.Tables[0].Rows[row]["reas_loc"]);
                        lockby = Convert.ToString(dsnews.Tables[0].Rows[row]["locked_by"]);
                    }
                    drbokcardlock["Roll No"] = rollno;
                    drbokcardlock["Staff Code"] = rollno;
                    drbokcardlock["Name"] = stdname;
                    drbokcardlock["Course"] = deptname;
                    drbokcardlock["Token No"] = Tokenos;
                    drbokcardlock["No Of Cards"] = tokendetails;
                    drbokcardlock["Reason"] = reason;
                    drbokcardlock["Locked By"] = lockby;
                    bokcard.Rows.Add(drbokcardlock);
                }
                grdCardLock.DataSource = bokcard;
                grdCardLock.DataBind();
                grdCardLock.Visible = true;
                rptprint.Visible = true;
                fieldborrow.Visible = true;
                for (int l = 0; l < grdCardLock.Rows.Count; l++)
                {
                    foreach (GridViewRow row in grdCardLock.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            grdCardLock.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            grdCardLock.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                            grdCardLock.Rows[l].Cells[2].HorizontalAlign = HorizontalAlign.Center;
                            grdCardLock.Rows[l].Cells[3].HorizontalAlign = HorizontalAlign.Left;
                            grdCardLock.Rows[l].Cells[4].HorizontalAlign = HorizontalAlign.Left;
                        }
                    }
                }

                if (rblcard.SelectedIndex == 1)
                {
                    btn_Lock.Visible = true;
                    btn_Lock.Text = "Lock";
                }
                else
                {
                    btn_Lock.Text = "Unlock";
                    btn_Lock.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {// d2.sendErrorMail(ex, userCollegeCode, "Card_Lock_Unlock"); 
        }
        {

        }


    }

    #endregion

    #region Fspread2
    protected void grdCardLock_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            grdBorrowerDet.Visible = true;
            string srollno = "";
            DataSet dsborrow = new DataSet();
            DataSet dsgetupdatebook = new DataSet();
            //var grid = (GridView)sender;
            //GridViewRow selectedRow = grid.SelectedRow;
            //int rowIndex = grid.SelectedIndex;
            //int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            //int arow = Convert.ToInt32(activerow);
            //if (Convert.ToString(rowIndex) != "")
            //{

            foreach (GridViewRow row2 in grdCardLock.Rows)
            {
                CheckBox cbsel = (CheckBox)row2.FindControl("chkenbl");
                int RowCnt = Convert.ToInt32(row2.RowIndex);
                if (cbsel.Checked == true)
                {
                    Label roll = (Label)grdCardLock.Rows[RowCnt].FindControl("lbl_rollno");
                    if (roll.Text.Trim() != "")
                    {
                        srollno = roll.Text.Trim();
                    }
                    string borrow = "select roll_no,stud_name,acc_no, title,lib_code from borrow where roll_no='" + srollno + "' and return_flag=0";
                    dsborrow.Clear();
                    dsborrow = d2.select_method_wo_parameter(borrow, "Text");
                    if (dsborrow.Tables[0].Rows.Count > 0)
                    {

                        dtlock.Columns.Add("Roll No", typeof(string));
                        dtlock.Columns.Add("Name", typeof(string));
                        dtlock.Columns.Add("Access No", typeof(string));
                        dtlock.Columns.Add("Title", typeof(string));
                        dtlock.Columns.Add("Library Name", typeof(string));

                        int sno = 0;
                        for (int row = 0; row < dsborrow.Tables[0].Rows.Count; row++)
                        {
                            sno++;
                            drlock = dtlock.NewRow();

                            string rollno = Convert.ToString(dsborrow.Tables[0].Rows[row]["roll_no"]).Trim();
                            string stdname = Convert.ToString(dsborrow.Tables[0].Rows[row]["stud_name"]).Trim();
                            string accno = Convert.ToString(dsborrow.Tables[0].Rows[row]["acc_no"]).Trim();
                            string title = Convert.ToString(dsborrow.Tables[0].Rows[row]["title"]);
                            string library = Convert.ToString(dsborrow.Tables[0].Rows[row]["lib_code"]);
                            string libraryname = d2.GetFunction("select Distinct lib_name from library where lib_code='" + library + "'");

                            drlock["Roll No"] = rollno;
                            drlock["Name"] = stdname;
                            drlock["Access No"] = accno;
                            drlock["Title"] = title;
                            drlock["Library Name"] = libraryname;

                            dtlock.Rows.Add(drlock);

                        }
                        grdBorrowerDet.DataSource = dtlock;
                        grdBorrowerDet.DataBind();
                        grdBorrowerDet.Visible = true;
                        for (int l = 0; l < grdCardLock.Rows.Count; l++)
                        {
                            foreach (GridViewRow row in grdCardLock.Rows)
                            {
                                foreach (TableCell cell in row.Cells)
                                {
                                    grdCardLock.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                                    grdCardLock.Rows[l].Cells[3].HorizontalAlign = HorizontalAlign.Center;
                                    // grdCardLock.Rows[l].Cells[5].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                        }
                        fieldborrow.Visible = true;


                    }
                    else
                    {
                        fieldborrow.Visible = true;
                        grdBorrowerDet.Visible = false;
                    }
                }
            }

            //}

            //else
            //{
            //    fieldborrow.Visible = true;
            //    grdBorrowerDet.Visible = false;
            //}

            //}
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Card_Lock_Unlock"); }
        {

        }

    }
    #endregion

    #region Print

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Card_Lock_Unlock";
            string pagename = "Card_Lock_Unlock.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrolhed2.loadspreaddetails(grdCardLock, pagename, degreedetails, 0, ss, "1");
            Printcontrolhed2.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Card_Lock_Unlock"); }

    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grdCardLock, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Card_Lock_Unlock"); }

    }
    #endregion

    #region Lock
    protected void btn_Lock_Click(object sender, EventArgs e)
    {
        try
        {
            string getrolln = "";
            string getrollno1 = "";
            string reason = "";
            if (grdCardLock.Rows.Count > 0)
            {
                foreach (GridViewRow row in grdCardLock.Rows)
                {
                    CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                    int RowCnt = Convert.ToInt32(row.RowIndex);
                    if (cbsel.Checked == true)
                    {
                        selectedcount++;
                        TextBox reason1 = (TextBox)grdCardLock.Rows[RowCnt].FindControl("lbl_reason");
                        if (reason1.Text.Trim() != "")
                        {
                            reason = reason1.Text.Trim();
                        }
                        if (reason1.Text.Trim() == "")
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Enter Reasons For Locking";
                            return;
                        }
                    }
                }
                if (selectedcount == 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please select atleast one record";
                    return;
                }

            }
            if (grdCardLock.Rows.Count > 0)
            {


                for (int row = 0; row < grdCardLock.Rows.Count; row++)
                {
                    foreach (GridViewRow row1 in grdCardLock.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row1.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row1.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            selectedcount++;
                            Label getroll = (Label)grdCardLock.Rows[RowCnt].FindControl("lbl_rollno");
                            TextBox reason_Val = (TextBox)grdCardLock.Rows[RowCnt].FindControl("lbl_reason");
                            if (reason_Val.Text.Trim() != "")
                            {
                                reason = reason_Val.Text.Trim();
                            }

                            else
                                getrollno1 = getrollno1 + "," + reason;
                        }
                    }
                    if (selectedcount == 0)
                    {
                        if (rblcard.SelectedIndex == 0)
                        {
                            if (ddl_Category.SelectedIndex == 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Please select the roll number of the student whose cards have to be Unlocked";
                                return;
                            }
                            else if (ddl_Category.SelectedIndex == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Please select the Staff Number of the Staff whose cards have to be Unlocked";
                                return;
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Please select the Staff Code / Roll Number whose cards have to be Unlocked";
                                return;
                            }
                        }
                        else
                        {
                            if (ddl_Category.SelectedIndex == 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Please select the roll number of the student whose cards have to be Locked";
                                return;
                            }
                            else if (ddl_Category.SelectedIndex == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Please select the Staff Number of the Staff whose cards have to be Locked";
                                return;
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Please select the Staff Code / Roll Number whose cards have to be Locked";
                                return;
                            }

                        }

                    }
                    else
                    {
                        if (grdBorrowerDet.Rows.Count == 0)
                        {
                            if (btn_Lock.Text.ToUpper() == "UNLOCK")
                            {
                                Divlockunlockrecord.Visible = true;
                                lbl_Divlockunlockrecord.Text = "Do you want to unLock the cards for the roll number:" + getrollno1;
                            }
                            else
                            {
                                Divlockunlockrecord.Visible = true;
                                lbl_Divlockunlockrecord.Text = "Do you want to Lock the cards for the roll number:" + getrollno1;

                            }
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Books have been Issued for this Card,So you cannot Lock this Card";
                        }
                    }

                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Card_Lock_Unlock"); }

    }



    protected void btn_lock_yes__record_Click(object sender, EventArgs e)
    {
        try
        {
            string LockUnlock = "";
            int Lock_Unlock = 0;
            string roll_no = "";
            string username = "";
            string reason = "";
            string tokno = "";
            Divlockunlockrecord.Visible = false;
            grdBorrowerDet.Visible = true;
            if (grdCardLock.Rows.Count > 0)
            {

                if (chkcard.Checked == false)
                {
                    if (btn_Lock.Text.ToUpper() == "UNLOCK")
                    {

                        foreach (GridViewRow row in grdCardLock.Rows)
                        {
                            CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                            int RowCnt = Convert.ToInt32(row.RowIndex);
                            if (cbsel.Checked == true)
                            {
                                Label roll = (Label)grdCardLock.Rows[RowCnt].FindControl("lbl_rollno");
                                if (roll.Text.Trim() != "")
                                {
                                    roll_no = roll.Text.Trim();
                                }
                                Label user = (Label)grdCardLock.Rows[RowCnt].FindControl("lbl_locked");
                                if (user.Text.Trim() != "")
                                {
                                    username = user.Text.Trim();
                                }

                                LockUnlock = "UPDATE tokendetails set is_locked = 0,reas_loc='',locked_by='" + username + "' where roll_no='" + roll_no + "' and is_locked=2";
                                Lock_Unlock = d2.update_method_wo_parameter(LockUnlock, "Text");
                            }
                        }
                    }
                    else
                    {
                        foreach (GridViewRow row in grdCardLock.Rows)
                        {
                            CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                            int RowCnt = Convert.ToInt32(row.RowIndex);
                            if (cbsel.Checked == true)
                            {
                                Label roll = (Label)grdCardLock.Rows[RowCnt].FindControl("lbl_rollno");
                                if (roll.Text.Trim() != "")
                                {
                                    roll_no = roll.Text.Trim();
                                }
                                TextBox reason_Val = (TextBox)grdCardLock.Rows[RowCnt].FindControl("lbl_reason");
                                if (reason_Val.Text.Trim() != "")
                                {
                                    reason = reason_Val.Text.Trim();
                                }
                                LockUnlock = "UPDATE tokendetails set is_locked = 2,reas_loc='" + reason + "',locked_by='" + Convert.ToString(Session["username1"]) + "' where roll_no='" + roll_no + "' and is_locked=0";
                                Lock_Unlock = d2.update_method_wo_parameter(LockUnlock, "Text");

                            }
                        }
                    }
                }
                else
                {
                    if (btn_Lock.Text.ToUpper() == "UNLOCK")
                    {
                        foreach (GridViewRow row in grdCardLock.Rows)
                        {
                            CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                            int RowCnt = Convert.ToInt32(row.RowIndex);
                            if (cbsel.Checked == true)
                            {
                                Label roll = (Label)grdCardLock.Rows[RowCnt].FindControl("lbl_rollno");
                                if (roll.Text.Trim() != "")
                                {
                                    roll_no = roll.Text.Trim();
                                }
                                Label user = (Label)grdCardLock.Rows[RowCnt].FindControl("lbl_locked");
                                if (user.Text.Trim() != "")
                                {
                                    username = user.Text.Trim();
                                }
                                Label tok = (Label)grdCardLock.Rows[RowCnt].FindControl("lbl_token");
                                if (tok.Text.Trim() != "")
                                {
                                    tokno = tok.Text.Trim();
                                }
                                LockUnlock = "UPDATE tokendetails set is_locked = 0,reas_loc='',locked_by='" + username + "' where roll_no='" + roll_no + "' and token_no='" + tokno + "' and is_locked=2";
                                Lock_Unlock = d2.update_method_wo_parameter(LockUnlock, "Text");


                            }
                        }
                    }
                    else
                    {

                        foreach (GridViewRow row in grdCardLock.Rows)
                        {
                            CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                            int RowCnt = Convert.ToInt32(row.RowIndex);
                            if (cbsel.Checked == true)
                            {
                                Label roll = (Label)grdCardLock.Rows[RowCnt].FindControl("lbl_rollno");
                                if (roll.Text.Trim() != "")
                                {
                                    roll_no = roll.Text.Trim();
                                }
                                TextBox reason_Val = (TextBox)grdCardLock.Rows[RowCnt].FindControl("lbl_reason");
                                if (reason_Val.Text.Trim() != "")
                                {
                                    reason = reason_Val.Text.Trim();
                                }
                                Label tok = (Label)grdCardLock.Rows[RowCnt].FindControl("lbl_token");
                                if (tok.Text.Trim() != "")
                                {
                                    tokno = tok.Text.Trim();
                                }
                                LockUnlock = "UPDATE tokendetails set is_locked = 2,reas_loc='" + reason + "',locked_by='" + Convert.ToString(Session["username1"]) + "' where roll_no='" + roll_no + "' and token_no='" + tokno + "' and is_locked=0";
                                Lock_Unlock = d2.update_method_wo_parameter(LockUnlock, "Text");


                            }
                        }
                    }

                }

            }
            if (Lock_Unlock > 0)
            {
                if (rblcard.SelectedIndex == 0)
                {
                    Divalert.Visible = true;
                    lblalertmsg.Text = "Cards have been UnLocked";
                }
                else
                {
                    Divalert.Visible = true;
                    lblalertmsg.Text = "Cards have been Locked";
                }
                btngo_Click(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Card_Lock_Unlock"); }

    }

    protected void btn_lock_no__record_Click(object sender, EventArgs e)
    {

        try
        {
            Divlockunlockrecord.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Card_Lock_Unlock"); }

    }

    #endregion

    #region Close
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }


    protected void btnerrclose1_Click(object sender, EventArgs e)
    {
        Divalert.Visible = false;

    }
    #endregion
}