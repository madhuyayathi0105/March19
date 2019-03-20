using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using InsproDataAccess;
using System.Drawing;
using System.Configuration;

public partial class LibraryMod_backvolume : System.Web.UI.Page
{
    string collegecode = string.Empty;
    string clgcode = string.Empty;
    string usercollegecode = string.Empty;
    string singleuser = string.Empty;
    string groupuser = string.Empty;
    string usercode = string.Empty;
    string month = string.Empty;
    string year = string.Empty;
    int tot;
    bool cellflag = false;
    string monthname = string.Empty;
    static int searchby = 0;
    static string searchlibcode = string.Empty;
    Hashtable ht = new Hashtable();
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("AttendanceHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/AttendanceMOD/AttendanceHome.aspx");
                    return;
                }
            }

            usercollegecode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
            usercode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
            singleuser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
            groupuser = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";

            if (!IsPostBack)
            {
                bindclg();
                getLibPrivil();
                txt_fromdate1.Attributes.Add("readonly", "readonly");
                txt_fromdate1.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                txt_todate1.Attributes.Add("readonly", "readonly");
                txt_todate1.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            //base.OnPreRender(e);
        }
        catch (Exception ex) { }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getsearchby(string prefixText)
    {
        string query = "";
        WebService ws = new WebService();
        List<string> values = new List<string>();


        if (searchby == 1)
        {

            if (searchlibcode != "All")
                query = "SELECT DISTINCT  TOP  100 access_code FROM back_volume where access_code Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by access_code";
            else
                query = "SELECT DISTINCT  TOP  100 access_code FROM back_volume where access_code Like '" + prefixText + "%' order by access_code ";
        }
        else if (searchby == 2)
        {
            if (searchlibcode != "All")
                query = "SELECT DISTINCT  TOP  100 title FROM back_volume where title Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by title";
            else
                query = "SELECT DISTINCT  TOP  100 title FROM back_volume where title Like '" + prefixText + "%' order by title";
        }
        else if (searchby == 5)
        {
            if (searchlibcode != "All")
                query = "SELECT DISTINCT  TOP  100 journal_year FROM back_volume where journal_year Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by journal_year";
            else
                query = "SELECT DISTINCT  TOP  100 journal_year FROM back_volume where journal_year Like '" + prefixText + "%' order by journal_year";
        }
        else if (searchby == 7)
        {
            if (searchlibcode != "All")
                query = "SELECT DISTINCT  TOP  100 contents FROM back_volume where contents Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by contents";
            else
                query = "SELECT DISTINCT  TOP  100 contents FROM back_volume where contents Like '" + prefixText + "%' order by contents";
        }
        values = ws.Getname(query);
        return values;
    }

    #region bindHeaders

    public void bindclg()
    {

        try
        {
            ddlclg.Items.Clear();
            string columnfield = string.Empty;
            string group_user = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
            if (group_user.Contains(";"))
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
            ht.Clear();
            ht.Add("column_field", Convert.ToString(columnfield));
            DataSet dsprint = da.select_method("bind_college", ht, "sp");
            ddlclg.Items.Clear();
            if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
            {
                ddlclg.DataSource = dsprint;
                ddlclg.DataTextField = "collname";
                ddlclg.DataValueField = "college_code";
                ddlclg.DataBind();
                ddlclg.SelectedIndex = 0;

            }
        }


        catch
        {
        }
    }

    public void bindlibrary(string LibCode)
    {
        try
        {
            ddllibrary.Items.Clear();
            ds.Clear();
            if (ddlclg.Items.Count > 0)
            {
                foreach (ListItem li in ddlclg.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(clgcode))
                        {
                            clgcode = "'" + li.Value + "'";
                        }
                        else
                        {
                            clgcode = ",'" + li.Value + "'";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(clgcode))
            {
                string lib = "select lib_name,lib_code,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + LibCode + " and college_code in(" + clgcode + ") ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = da.select_method_wo_parameter(lib, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddllibrary.DataSource = ds;
                    ddllibrary.DataValueField = "lib_code";
                    ddllibrary.DataTextField = "lib_name";
                    ddllibrary.DataBind();

                    searchlibcode = Convert.ToString(ddllibrary.SelectedValue);
                }
            }
        }
        catch
        {
        }
    }

    public void binddept()
    {
        try
        {
            ddldept.Items.Clear();
            ds.Clear();
            if (ddlclg.Items.Count > 0)
            {
                foreach (ListItem li in ddlclg.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(clgcode))
                        {
                            clgcode = "'" + li.Value + "'";
                        }
                        else
                        {
                            clgcode = ",'" + li.Value + "'";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(clgcode))
            {
                string dept = "select distinct department from journal_master where lib_code='" + ddllibrary.SelectedValue.ToString() + "'";
                ds = da.select_method_wo_parameter(dept, "text");
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
                {
                    ddldept.DataSource = ds;
                    ddldept.DataTextField = "department";
                    ddldept.DataValueField = "department";
                    ddldept.DataBind();
                }
            }
        }
        catch
        {
        }
    }

    #endregion

    protected void ddlcollege_selectedindexchange(object sender, EventArgs e)
    {
        getLibPrivil();
        ddllibrary_SelectedIndexChanged(sender, e);
    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        searchlibcode = Convert.ToString(ddllibrary.SelectedValue);
    }

    protected void ddlsearch_selectedindexchange(object sender, EventArgs e)
    {
        if (ddlsearch.SelectedIndex == 1 || ddlsearch.SelectedIndex == 2 || ddlsearch.SelectedIndex == 5 || ddlsearch.SelectedIndex == 7)
        {
            txtusernam.Visible = true;
            ddlstatus1.Visible = false;
            ddldept.Visible = false;
            txt_fromdate1.Visible = false;
            txt_todate1.Visible = false;
            lblfrom.Visible = false;
            lbl_todate.Visible = false;

            if (ddlsearch.SelectedIndex == 1)
                searchby = 1;

            else if (ddlsearch.SelectedIndex == 2)
                searchby = 2;

            else if (ddlsearch.SelectedIndex == 5)
                searchby = 5;

            else if (ddlsearch.SelectedIndex == 7)
                searchby = 7;

        }
        else if (ddlsearch.SelectedIndex == 3)
        {
            txt_fromdate1.Visible = true;
            txt_todate1.Visible = true;
            txtusernam.Visible = false;
            ddldept.Visible = false;
            ddlstatus1.Visible = false;
            lblfrom.Visible = true;
            lbl_todate.Visible = true;
        }
        else if (ddlsearch.SelectedIndex == 4)
        {

            txt_fromdate1.Visible = false;
            txt_todate1.Visible = false;
            txtusernam.Visible = false;
            ddldept.Visible = true;
            ddlstatus1.Visible = false;
            lblfrom.Visible = false;
            lbl_todate.Visible = false;
            binddept();

        }
        else if (ddlsearch.SelectedIndex == 6)
        {
            txt_fromdate1.Visible = false;
            txt_todate1.Visible = false;
            txtusernam.Visible = false;
            ddldept.Visible = false;
            ddlstatus1.Visible = true;
            lblfrom.Visible = false;
            lbl_todate.Visible = false;
        }
        else
        {
            txt_fromdate1.Visible = false;
            txt_todate1.Visible = false;
            txtusernam.Visible = false;
            ddldept.Visible = false;
            ddlstatus1.Visible = false;
            lblfrom.Visible = false;
            lbl_todate.Visible = false;

        }

    }

    protected void ddlstatus1_selectedindexchange(object sender, EventArgs e)
    {
    }

    protected void ddldept_OnselectedindexchangeD(object sender, EventArgs e)
    {
    }

    #region ButtonEvents

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            string qry = string.Empty;
            string fromdate = string.Empty;
            string todate = string.Empty;
            if (ddlsearch.SelectedIndex == 0)
            {
                qry = "select back_volume.access_code,back_volume.title,back_volume.access_date,back_volume.journal_year,frommonthpub+'-'+tomonthpub frommonth,back_volume.issue_flag,back_volume.volumeno,back_volume.issueno,Dept_Name as dept,MissMOnthYear,contents from back_volume where back_volume.lib_code='" + Convert.ToString(ddllibrary.SelectedValue).Trim() + "'  and issue_flag<>'Return' order by len(access_code),access_code ";
            }
            else if (ddlsearch.SelectedIndex == 1)
            {
                qry = "select back_volume.access_code,back_volume.title,back_volume.access_date,back_volume.journal_year,frommonthpub+'-'+tomonthpub frommonth,back_volume.issue_flag,back_volume.volumeno,back_volume.issueno,Dept_Name as dept,MissMOnthYear,contents  from back_volume where back_volume.access_code like '" + txtusernam.Text + "%' and back_volume.lib_code='" + Convert.ToString(ddllibrary.SelectedValue).Trim() + "' and issue_flag<>'Return'  order by len(access_code),access_code";
            }
            else if (ddlsearch.SelectedIndex == 2)
            {
                qry = "select back_volume.access_code,back_volume.title,back_volume.access_date,back_volume.journal_year,frommonthpub+'-'+tomonthpub frommonth,back_volume.issue_flag,back_volume.volumeno,back_volume.issueno,Dept_Name as dept,MissMOnthYear,contents  from back_volume where back_volume.title like '" + txtusernam.Text + "%' and back_volume.lib_code='" + Convert.ToString(ddllibrary.SelectedValue).Trim() + "' and issue_flag<>'Return'  order by len(access_code),access_code";
            }

            else if (ddlsearch.SelectedIndex == 5)
            {
                qry = "select back_volume.access_code,back_volume.title,back_volume.access_date,back_volume.journal_year,frommonthpub+'-'+tomonthpub frommonth,back_volume.issue_flag,back_volume.volumeno,back_volume.issueno,Dept_Name as dept,MissMOnthYear,contents  from back_volume where back_volume.journal_year like '" + txtusernam.Text + "%' and back_volume.lib_code='" + Convert.ToString(ddllibrary.SelectedValue).Trim() + "' and issue_flag<>'Return'  order by len(access_code),access_code";
            }
            else if (ddlsearch.SelectedIndex == 6)
            {
                qry = "select back_volume.access_code,back_volume.title,back_volume.access_date,back_volume.journal_year,frommonthpub+'-'+tomonthpub frommonth,back_volume.issue_flag,back_volume.volumeno,back_volume.issueno,Dept_Name as dept,MissMOnthYear,contents  from back_volume where back_volume.issue_flag like '" + Convert.ToString(ddlstatus1.SelectedItem.Text) + "%' and back_volume.lib_code='" + Convert.ToString(ddllibrary.SelectedValue).Trim() + "'  order by len(access_code),access_code";
            }
            else if (ddlsearch.SelectedIndex == 3)
            {
                fromdate = txt_fromdate1.Text;
                todate = txt_todate1.Text;
                DateTime dt = new DateTime();
                dt = Convert.ToDateTime(fromdate);
                fromdate = dt.ToString("yyyy-MM-dd");

                DateTime dt1 = new DateTime();
                dt1 = Convert.ToDateTime(todate);
                todate = dt1.ToString("yyyy-MM-dd");

                qry = "select back_volume.access_code,back_volume.title,back_volume.access_date,back_volume.journal_year,frommonthpub+'-'+tomonthpub frommonth,back_volume.issue_flag,back_volume.volumeno,back_volume.issueno,Dept_Name as dept,MissMOnthYear,contents  from back_volume where back_volume.access_date between '" + fromdate + "' and '" + todate + "' and back_volume.lib_code='" + Convert.ToString(ddllibrary.SelectedValue).Trim() + "' and issue_flag<>'Return'  order by len(access_code),access_code ";
            }
            else if (ddlsearch.SelectedIndex == 4)
            {
                qry = "select back_volume.access_code,back_volume.title,back_volume.access_date,back_volume.journal_year,frommonthpub+'-'+tomonthpub frommonth,back_volume.issue_flag,back_volume.volumeno,back_volume.issueno,department as dept,MissMOnthYear,contents  From back_volume, journal_master WHERE journal_master.journal_name = back_volume.periodicalname  and journal_master.department='" + Convert.ToString(ddldept.SelectedItem.Text) + "' and back_volume.lib_code=journal_master.lib_code and back_volume.lib_code='" + Convert.ToString(ddllibrary.SelectedValue).Trim() + "' and issue_flag<>'Return'  order by len(access_code),access_code ";
            }
            else
            {
                qry = "select back_volume.access_code,back_volume.title,back_volume.access_date,back_volume.journal_year,frommonthpub+'-'+tomonthpub frommonth,back_volume.issue_flag,back_volume.volumeno,back_volume.issueno,Dept_Name as dept,MissMOnthYear,contents  from back_volume where contents like '%" + txtusernam.Text + "%' and back_volume.lib_code='" + Convert.ToString(ddllibrary.SelectedValue).Trim() + "' and issue_flag<>'Return'  order by len(access_code),access_code";
            }

            ds.Clear();
            ds = da.select_method_wo_parameter(qry, "text");
            DataTable dtBackVol = new DataTable();
            DataRow drow;
            int sno = 0;
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                dtBackVol.Columns.Add("SNo", typeof(string));
                dtBackVol.Columns.Add("Access No", typeof(string));
                dtBackVol.Columns.Add("Title", typeof(string));
                dtBackVol.Columns.Add("Acc Date", typeof(string));
                dtBackVol.Columns.Add("Year", typeof(string));
                dtBackVol.Columns.Add("From-To Month", typeof(string));
                dtBackVol.Columns.Add("Status", typeof(string));
                dtBackVol.Columns.Add("Department", typeof(string));
                dtBackVol.Columns.Add("Volume No", typeof(string));
                dtBackVol.Columns.Add("Missing Month/Year", typeof(string));
                dtBackVol.Columns.Add("Contents", typeof(string));


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    string accno = Convert.ToString(ds.Tables[0].Rows[i]["access_code"]).Trim();
                    string title = Convert.ToString(ds.Tables[0].Rows[i]["title"]).Trim();
                    string accdate = Convert.ToString(ds.Tables[0].Rows[i]["access_date"]).Trim();
                    DateTime dt = new DateTime();
                    dt = Convert.ToDateTime(accdate);
                    accdate = dt.ToString("dd/MM/yyyy");
                    string year = Convert.ToString(ds.Tables[0].Rows[i]["journal_year"]).Trim();
                    string fromtomonth = Convert.ToString(ds.Tables[0].Rows[i]["frommonth"]).Trim();
                    string status = Convert.ToString(ds.Tables[0].Rows[i]["issue_flag"]).Trim();
                    string volno = Convert.ToString(ds.Tables[0].Rows[i]["volumeno"]).Trim();
                    string dept = Convert.ToString(ds.Tables[0].Rows[i]["dept"]).Trim();
                    string missingmonyr = Convert.ToString(ds.Tables[0].Rows[i]["MissMOnthYear"]).Trim();
                    string content = Convert.ToString(ds.Tables[0].Rows[i]["contents"]).Trim();

                    drow = dtBackVol.NewRow();
                    drow["SNo"] = Convert.ToString(sno);
                    drow["Access No"] = accno;
                    drow["Title"] = title;
                    drow["Acc Date"] = accdate;
                    drow["Year"] = year;
                    drow["From-To Month"] = fromtomonth;
                    drow["Status"] = status;
                    drow["Department"] = dept;
                    drow["Volume No"] = volno;
                    drow["Missing Month/Year"] = missingmonyr;
                    drow["Contents"] = content;
                    dtBackVol.Rows.Add(drow);
                }
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                grdBackVol.DataSource = dtBackVol;
                grdBackVol.DataBind();

                grdBackVol.Visible = true;
                divtable.Visible = true;
                int totalrows = grdBackVol.Rows.Count;
                btn_printmaster.Visible = true;
                btn_Excel.Visible = true;
                txt_excelname.Visible = true;
                btnPopAlertClose.Visible = false;
                divPopupAlert.Visible = false;
                divAlertContent.Visible = false;
                lbl_reportname.Visible = true;
                btnyes.Visible = false;
                btnNo.Visible = false;
                div4.Visible = true;
                txtnoofbooks.Text = Convert.ToString(totalrows);
            }
            else
            {
                grdBackVol.Visible = false;
                divtable.Visible = false;
                btnPopAlertClose.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Records Found";
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                btnyes.Visible = false;
                btnNo.Visible = false;
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, usercollegecode, "InwardEntry");
        }
    }

    protected void btnadd_Click(object sender, EventArgs e)
    {
        try
        {
            divPopAlertbackvolume.Visible = true;
            divPopAlertback.Visible = true;
            periodicalname();
            txt_accessdate2.Attributes.Add("readonly", "readonly");
            txt_accessdate2.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            accessno();
            Btnsave.Visible = true;
            Btnclose.Visible = true;
            btnupdate.Visible = false;
            btndelete.Visible = false;
        }
        catch
        {

        }
    }

    protected void btnclose_Click(object sender, EventArgs e)
    {
        try
        {
            divPopAlertbackvolume.Visible = false;
            divPopAlertback.Visible = false;
            cleartxt();
        }
        catch
        {
        }
    }

    protected void btn_ex_Click(object sender, EventArgs e)
    {
        div1.Visible = false;
        div2.Visible = false;
        div3.Visible = false;
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        divPopupAlert.Visible = true;
        divPopupAlert.Visible = true;
        lblAlertMsg.Visible = true;
        lblAlertMsg.Text = "Are You Sure To Delete?";
        btnyes.Visible = true;
        btnNo.Visible = true;
        btnPopAlertClose.Visible = false;
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            string insertqry = string.Empty;
            string insertqry1 = string.Empty;
            int insert1, insert2;
            string currdate = string.Empty;
            string currtime = string.Empty;
            currdate = DateTime.Now.ToString("yyyy-MM-dd");
            currtime = DateTime.Now.ToString("hh:mm:ss tt");

            if (chkjour.Checked == true)
            {
                if (txtjournal.Text != "" && txtnewjour.Text != "")
                {
                    string qry = "SELECT * FROM Journal_Master WHERE (Journal_Name ='" + txtnewjour.Text + "' OR Journal_Code ='" + txtjournal.Text + "')";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(qry, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblAlertMsg.Visible = true;
                        lblAlertMsg.Text = "Journal already exist";
                        btnPopAlertClose.Visible = true;
                        divAlertContent.Visible = true;
                        divPopupAlert.Visible = true;
                        btnyes.Visible = false;
                        btnNo.Visible = false;
                        return;
                    }
                    else
                    {
                        string period = Convert.ToString(ddlperidical.SelectedItem.Text);
                        period = txtnewjour.Text;
                        ddlperidical.SelectedItem.Text = txtnewjour.Text;
                        insertqry = "INSERT INTO Journal_Master(Access_Date,Access_Time,Journal_Code,Journal_Name,Lib_Code) values('" + currdate + "','" + currtime + "','" + txtjournal.Text + "','" + txtnewjour.Text + "','" + Convert.ToString(ddllibrary.SelectedValue) + "' )";
                        insert1 = da.update_method_wo_parameter(insertqry, "text");

                    }
                }
                else
                {
                    lblAlertMsg.Text = "Journal Code & Journal Name should not be empty";
                }
            }
            if (Txtfromyear.Text == "")
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Enter The Year";
                btnPopAlertClose.Visible = true;
                divAlertContent.Visible = true;
                divPopupAlert.Visible = true;
                btnyes.Visible = false;
                btnNo.Visible = false;
                return;
            }
            string year1 = string.Empty;
            string toyear = string.Empty;
            if (Txttoyear.Text == "")
            {
                toyear = "0";
                year1 = Txtfromyear.Text;
            }
            else
            {

                if (Convert.ToInt32(Txttoyear.Text) > 1)
                {
                    year1 = Txtfromyear.Text + "-" + Txttoyear.Text;
                    toyear = Txttoyear.Text;
                }
            }

            string accdate = string.Empty;
            accdate = txt_accessdate2.Text;
            DateTime dt2 = new DateTime();

            dt2 = Convert.ToDateTime(accdate);
            accdate = dt2.ToString("yyyy-MM-dd");
            addmissingyrmonth();
            string missingmonyr = month + "-" + year;



            if (Convert.ToInt32(toyear) > 1)
            {
                insertqry1 = "insert into back_volume (access_code,access_time,title,journal_year,access_date,lib_code,issue_flag,back_access_date,publisher,remarks,issueno,volumeno,monthpub,periodicalname,MissMonthYear,billno,billdate,contents,frommonthpub,tomonthpub,journal_Code) values('" + Txtaccno.Text + "', '" + currtime + "','" + Txttile.Text + "','" + year1 + "','" + accdate + "','" + Convert.ToString(ddllibrary.SelectedValue) + "','" + ddlstatus.SelectedItem.ToString() + "','" + currdate + "','','" + txtremark.Text + "','" + txtissueno.Text + "','" + Txtvolumeno.Text + "','" + Txtyear.Text + "','" + Convert.ToString(ddlperidical.SelectedItem.Text) + "','" + missingmonyr + "','','" + accdate + "','','" + Txtfromyear.Text + "','" + Txttoyear.Text + "','" + txtjournal.Text + "')";
            }
            else
            {
                insertqry1 = "insert into back_volume (access_code,access_time,title,journal_year,access_date,lib_code,issue_flag,back_access_date,publisher,remarks,issueno,volumeno,frommonthpub,tomonthpub,periodicalname,MissMonthYear,billno,billdate,contents,journal_Code) values('" + Txtaccno.Text + "', '" + currtime + "','" + Txttile.Text + "','" + year1 + "','" + accdate + "','" + Convert.ToString(ddllibrary.SelectedValue) + "','" + ddlstatus.SelectedItem.ToString() + "','" + currdate + "','','" + txtremark.Text + "','" + txtissueno.Text + "','" + Txtvolumeno.Text + "','" + ddlmonthpublication.SelectedValue.ToString() + "','" + ddlto.SelectedValue.ToString() + "','" + Convert.ToString(ddlperidical.SelectedItem.Text) + "','" + missingmonyr + "','','" + accdate + "','','" + txtjournal.Text + "')";
            }
            insert2 = da.update_method_wo_parameter(insertqry1, "text");
            if (insert2 == 0)
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Records Not Saved";
                divPopAlertback.Visible = false;
                divPopAlertbackvolume.Visible = false;
                btnPopAlertClose.Visible = true;
                btnyes.Visible = false;
                btnNo.Visible = false;
                cleartxt();
            }
            else
            {
                divPopAlertbackvolume.Visible = false;
                divPopAlertback.Visible = false;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Records Saved Successfully";
                btnPopAlertClose.Visible = true;
                divPopupAlert.Visible = true;
                btnyes.Visible = false;
                btnNo.Visible = false;
                cleartxt();

            }


        }
        catch
        {
        }
    }

    protected void btnupdate_Click(object sender, EventArgs e)
    {
        try
        {
            string insertqry1 = string.Empty;
            int insert1;
            string year1 = string.Empty;
            if (txtjournal.Text == "")
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Enter The Journal Code";
                btnPopAlertClose.Visible = true;
                divAlertContent.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            if (Txtaccno.Text == "")
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Enter The Access Code";
                btnPopAlertClose.Visible = true;
                divAlertContent.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            if (Txtfromyear.Text == "")
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Enter The Year";
                btnPopAlertClose.Visible = true;
                divAlertContent.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            if (txtmonth.Text == "")
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Enter The Month Of Publication";
                btnPopAlertClose.Visible = true;
                divAlertContent.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            if (Convert.ToInt32(Txttoyear.Text) > 1)
            {
                year1 = Txtfromyear.Text + "-" + Txttoyear.Text;
            }
            else
            {
                year1 = Txtfromyear.Text;
            }
            addmissingyrmonth();
            string missingmonyr = month + "-" + year;
            string accdate = string.Empty;
            accdate = txt_accessdate2.Text;
            DateTime dt2 = new DateTime();
            dt2 = Convert.ToDateTime(accdate);
            accdate = dt2.ToString("yyyy-MM-dd");
            if (Convert.ToInt32(Txttoyear.Text) > 1)
            {
                insertqry1 = "update back_volume set title='" + Txttile.Text + "',journal_year='" + year1 + "',access_date='" + accdate + "',lib_code='" + Convert.ToString(ddllibrary.SelectedValue) + "',issue_flag ='" + ddlstatus.SelectedItem.Text.ToString() + "',publisher='',remarks='" + txtremark.Text + "',issueno='" + txtissueno.Text + "',volumeno='" + Txtvolumeno.Text + "',monthpub='" + Txtyear.Text + "',frommonthpub='" + ddlmonthpublication.SelectedValue.ToString() + "',tomonthpub='" + ddlto.SelectedValue.ToString() + "',missmonthyear='" + missingmonyr + "',billno='',billdate='" + accdate + "',contents='' where access_code='" + Txtaccno.Text + "' and lib_code='" + Convert.ToString(ddllibrary.SelectedValue) + "' ";
            }

            else
            {
                insertqry1 = "update back_volume set title='" + Txttile.Text + "',journal_year='" + year1 + "',access_date='" + accdate + "',lib_code='" + Convert.ToString(ddllibrary.SelectedValue) + "',issue_flag ='" + ddlstatus.SelectedItem.Text.ToString() + "',publisher='',remarks='" + txtremark.Text + "',issueno='" + txtissueno.Text + "',volumeno='" + Txtvolumeno.Text + "',frommonthpub='" + ddlmonthpublication.SelectedValue.ToString() + "',tomonthpub='" + ddlto.SelectedValue.ToString() + "',monthpub='' ,missmonthyear='" + missingmonyr + "',billno='',billdate='" + accdate + "',contents=''  where access_code='" + Txtaccno.Text + "' and lib_code='" + Convert.ToString(ddllibrary.SelectedValue) + "' ";
            }

            insert1 = da.update_method_wo_parameter(insertqry1, "text");
            if (insert1 == 0)
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Records Not Upadted";
                divPopAlertback.Visible = false;
                divPopAlertbackvolume.Visible = false;
                btnPopAlertClose.Visible = true;
                btnyes.Visible = false;
                btnNo.Visible = false;
            }
            else
            {
                divPopAlertbackvolume.Visible = false;
                divPopAlertback.Visible = false;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Records Updated Successfully";
                btnPopAlertClose.Visible = true;
                divPopupAlert.Visible = true;
                btnyes.Visible = false;
                btnNo.Visible = false;
                string activerow = string.Empty;
                string activecol = string.Empty;
                //if (activerow.Trim() != "-1")
                //{
                //    activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                //    activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
                //}
                //Txtaccno.Text = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text.ToString();
                //Txttile.Text = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text.ToString();
            }
        }
        catch
        {
        }
    }

    protected void btnyes_Click(object sender, EventArgs e)
    {
        try
        {

            int delete1;
            string deleteqry = string.Empty;
            deleteqry = "delete from back_volume where access_code='" + Txtaccno.Text + "' and lib_code='" + Convert.ToString(ddllibrary.SelectedValue) + "'";
            delete1 = da.update_method_wo_parameter(deleteqry, "text");
            if (delete1 == 0)
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Records Not Deleted";
                divPopAlertback.Visible = false;
                divPopAlertbackvolume.Visible = false;
                btnPopAlertClose.Visible = true;
                btnyes.Visible = false;
                btnNo.Visible = false;
            }
            else
            {
                divPopAlertbackvolume.Visible = false;
                divPopAlertback.Visible = false;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Records Deleted Successfully";
                btnPopAlertClose.Visible = true;
                divPopupAlert.Visible = true;
                btnyes.Visible = false;
                btnNo.Visible = false;
            }

        }
        catch
        {
        }
    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
        divPopupAlert.Visible = false;
        divPopupAlert.Visible = false;
        lblAlertMsg.Visible = false;
        btnyes.Visible = false;
        btnNo.Visible = false;
    }

    #endregion

    public void periodicalname()
    {
        try
        {
            string lib = Convert.ToString(ddllibrary.SelectedValue).Trim();
            string periodicals = "select distinct journal_name from journal_master where lib_code='" + lib + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(periodicals, "text");
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                ddlperidical.DataSource = ds;
                ddlperidical.DataTextField = "journal_name";
                ddlperidical.DataValueField = "journal_name";
                ddlperidical.DataBind();
                ddlperidical.Items.Insert(0, "");
            }
        }
        catch
        {
        }
    }

    protected void chkjour_OncheckedChanged(object sender, EventArgs e)
    {
        if (chkjour.Checked == true)
        {
            txtnewjour.Visible = true;
            txtjournal.Enabled = true;
        }
        else
        {
            txtnewjour.Visible = false;
            txtjournal.Enabled = true;
        }
    }

    protected void ddlperidical_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string qry = string.Empty;
            string journalcode = string.Empty;
            qry = "select publisher,journal_code from journal_master where journal_name='" + ddlperidical.SelectedItem.ToString() + "' AND Lib_Code ='" + Convert.ToString(ddllibrary.SelectedValue).Trim() + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(qry, "text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                journalcode = Convert.ToString(ds.Tables[0].Rows[0]["journal_code"]).Trim();


            }
            txtjournal.Text = journalcode;
            Txttile.Text = Convert.ToString(ddlperidical.SelectedItem.Text);
        }
        catch
        {
        }
    }

    protected void ddlmonthpublication_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlto_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void chkmonth_CheckedChanged(object sender, EventArgs e)
    {
        txtmonth.Text = "--Select--";

        if (chkmonth.Checked == true)
        {

            for (int i = 0; i < cblmonth.Items.Count; i++)
            {
                cblmonth.Items[i].Selected = true;
            }
            txtmonth.Text = "Month(" + (cblmonth.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblmonth.Items.Count; i++)
            {
                cblmonth.Items[i].Selected = false;
            }
        }

    }

    protected void chkmonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = 0;
        chkmonth.Checked = false;
        int commcount = 0;
        // Txtfromyear.Text = Txtyear.Text;
        Txtyear.Text = Txtfromyear.Text;
        txtmonth.Text = "--Select--";
        for (i = 0; i < cblmonth.Items.Count; i++)
        {
            if (cblmonth.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cblmonth.Items.Count)
            {
                chkmonth.Checked = true;
            }
            txtmonth.Text = "Month(" + commcount.ToString() + ")";
        }
    }

    protected void ddlstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        lblAlertMsg.Text = string.Empty;
        lblAlertMsg.Visible = false;
        divPopupAlert.Visible = false;
        lblAlertMsg.Text = string.Empty;
    }

    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string vol = "Back Volume";
            string pagename = "backvolume.aspx";
            string ss = "";
            Printcontrolhed2.loadspreaddetails(grdBackVol, pagename, vol, 0, ss, "1");
            Printcontrolhed2.Visible = true;
        }
        catch { }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {
                da.printexcelreportgrid(grdBackVol, report);
                lbl_norec.Visible = false;
            }
            else
            {
                lbl_norec.Text = "Please Enter Your Report Name";
                lbl_norec.Visible = true;
            }
            btn_Excel.Focus();
        }
        catch
        {

        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

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

    protected void lnkIssues_Click(object sender, EventArgs e)
    {
        string qry = string.Empty;
        //qry = "SELECT distinct(journal.access_code),journal.journal_code,journal.title,isnull(journal.dept_name,'') dept_name,isnull(journal.volume_no,'') volume_no,journal.issue_no,journal.received_date,journal.issue_date,isnull(journal.noofcopies,'') noofcopies,isnull(journal.remarks,'') remarks,isnull(journal.attachement,'') attachement,journal.issue_flag,journal.receive_date,isnull(journal.issn,'') issn,journal.contents,isnull(journal.supplier,'') supplier,isnull(address,'') address,isnull(invoice_no,'') invoice_no,isnull(pay_type,'') pay_type,expiry_date FROM journal INNER JOIN journal_master ON (journal.journal_code = journal_master.journal_code) AND (journal.lib_code = journal_master.lib_code) where journal.access_code = '" + Txtaccno.Text + "' and journal.lib_code  = '" + Convert.ToString(ddllibrary.SelectedValue).Trim() + "'";
        qry = "SELECT Journal_Code as JournalCode,Access_Code as AccessCode,Title,Dept_name as Department,Issue_No as IssueNo,Received_Date as ReceivedDate FROM Journal WHERE Journal_Code ='" + txtjournal.Text + "' ";
        ds.Clear();
        ds = da.select_method_wo_parameter(qry, "text");

        if (ds.Tables[0].Rows.Count > 0)
        {
            divissues.Visible = true;
            divissue.Visible = true;
            div1issue.Visible = true;
            GrdIssues.DataSource = ds.Tables[0];
            GrdIssues.DataBind();
            GrdIssues.Visible = true;

        }

        else
        {
            lblAlertMsg.Visible = true;
            lblAlertMsg.Text = "No Records Found";
            btnPopAlertClose.Visible = true;
            divPopupAlert.Visible = true;
            divAlertContent.Visible = true;
            divissue.Visible = false;
            divissues.Visible = false;
            div1issue.Visible = false;
            btnyes.Visible = false;
            btnNo.Visible = false;
            GrdIssues.Visible = false;
        }
    }

    #region MissingMonthclick

    protected void btnAddyear_OnClick(object sender, EventArgs e)
    {
        try
        {
            addmissingyrmonth();
        }
        catch
        {
        }
    }

    public void addmissingyrmonth()
    {
        for (int i = 0; i < cblmonth.Items.Count; i++)
        {
            if (cblmonth.Items[i].Selected == true)
            {
                if (month == "")
                {
                    month = Convert.ToString(cblmonth.Items[i].Value);
                }
                else
                {
                    month += "," + Convert.ToString(cblmonth.Items[i].Value);
                }
            }
        }
        DataTable dtMissingMnt = new DataTable();
        DataRow drow;
        dtMissingMnt.Columns.Add("Missing Year", typeof(string));
        dtMissingMnt.Columns.Add("Missing Month", typeof(string));

        int rowIndex = 0;
        year = Txtyear.Text;
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                {
                    drCurrentRow = dtCurrentTable.NewRow();
                    string yearval = Convert.ToString(dtCurrentTable.Rows[i]["Missing Year"]);
                    string monthval = Convert.ToString(dtCurrentTable.Rows[i]["Missing Month"]);
                    if (!string.IsNullOrEmpty(yearval))
                    {
                        dtCurrentTable.Rows[i]["Missing Year"] = yearval;
                    }
                    else if (!string.IsNullOrEmpty(year))
                    {
                        dtCurrentTable.Rows[i]["Missing Year"] = year;
                    }
                    if (!string.IsNullOrEmpty(monthval))
                    {
                        dtCurrentTable.Rows[i]["Missing Month"] = monthval;
                    }
                    else if (!string.IsNullOrEmpty(month))
                    {
                        dtCurrentTable.Rows[i]["Missing Month"] = month;
                    }
                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable"] = dtCurrentTable;
                GrdView.DataSource = dtCurrentTable;
                GrdView.DataBind();
                GrdView.Visible = true;
            }
        }
        else
        {
            drow = dtMissingMnt.NewRow();
            drow["Missing Year"] = year;
            drow["Missing Month"] = month;
            dtMissingMnt.Rows.Add(drow);

            ViewState["CurrentTable"] = dtMissingMnt;
            GrdView.DataSource = dtMissingMnt;
            GrdView.DataBind();
            GrdView.Visible = true;
        }
        SetPreviousData();
        Txtyear.Text = "";
    }

    public void SetPreviousData()
    {
        try
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    DataTable dt = (DataTable)ViewState["CurrentTable"];
                    Hashtable hashlist = new Hashtable();
                    if (dt.Rows.Count > 0)
                    {
                        hashlist.Add(0, "Missing Year");
                        hashlist.Add(1, "Missing Month");

                        DataRow dr;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string MisYr = dt.Rows[i][0].ToString();
                            string Mismonth = dt.Rows[i][1].ToString();
                            string val_file = Convert.ToString(hashlist[i]);
                            rowIndex++;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnview_OnClick(object sender, EventArgs e)
    {
        try
        {
            DataTable dtMissingMnt = new DataTable();
            DataRow drow;
            dtMissingMnt.Columns.Add("Missing Year", typeof(string));
            dtMissingMnt.Columns.Add("Missing Month", typeof(string));
            drow = dtMissingMnt.NewRow();
            GrdView.DataSource = dtMissingMnt;
            GrdView.DataBind();
            GrdView.Visible = true;
            div1.Visible = true;
            div2.Visible = true;
            div3.Visible = true;
        }
        catch
        {
        }
    }

    #endregion

    public void accessno()
    {
        try
        {
            string codeno = string.Empty;
            string codeno1 = string.Empty;
            string TotLen = string.Empty;
            int intpos = 0;
            DataSet dsBack = new DataSet();
            string sql = "";
            string AccessCode = "";
            string libcode = Convert.ToString(ddllibrary.SelectedValue);
            sql = "SELECT ISNULL(BackVolumeAutoNo,0) BackVolume_AutoNo,ISNULL(BV_Acr,'') BV_Acr,ISNULL(BV_StNo,1) BV_StNo FROM Library Where Lib_Code ='" + libcode + "'";
            ds = da.select_method_wo_parameter(sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string backVol = Convert.ToString(ds.Tables[0].Rows[0]["BackVolume_AutoNo"]);
                if (backVol.ToLower() == "true")
                {
                    Txtaccno.Enabled = false;
                    sql = "SELECT * FROM Back_Volume WHERE Lib_Code ='" + libcode + "' ORDER BY LEN(Access_Code),Access_Code";

                    dsBack.Clear();
                    dsBack = da.select_method_wo_parameter(sql, "text");
                    if (dsBack.Tables[0].Rows.Count > 0)
                    {
                        codeno = Convert.ToString(dsBack.Tables[0].Rows[dsBack.Tables[0].Rows.Count - 1]["Access_Code"]);
                        string str = "";
                        for (int k = 0; k < codeno.Length; k++)
                        {
                            string a = Convert.ToString(codeno.ElementAt<char>(k));
                            if (a.All(char.IsNumber))
                            {
                                str = str + a;
                            }
                        }
                        int jj = Convert.ToInt32(str) + 1;

                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["BV_Acr"]) + jj;
                        Txtaccno.Text = codeno1;
                        Txtaccno.Enabled = false;
                    }
                    else
                    {
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["BV_Acr"]) + Convert.ToString(ds.Tables[0].Rows[0]["BV_StNo"]);
                        Txtaccno.Text = codeno1;
                        Txtaccno.Enabled = false;
                    }
                }
                else
                {
                    Txtaccno.Text = "";
                    Txtaccno.Enabled = true;
                }
            }
            else
            {
                Txtaccno.Text = "";
                Txtaccno.Enabled = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void cleartxt()
    {
        txtjournal.Text = string.Empty;
        Txttile.Text = string.Empty;
        Txtaccno.Text = string.Empty;
        Txtfromyear.Text = string.Empty;
        Txttoyear.Text = string.Empty;
        txtmonth.Text = string.Empty;
        Txtyear.Text = string.Empty;
        Txtvolumeno.Text = string.Empty;
        txtissueno.Text = string.Empty;
        txtremark.Text = string.Empty;

    }

    public void getLibPrivil()
    {
        try
        {
            string libcodecollection = "";
            string coll_Code = Convert.ToString(ddlclg.SelectedValue);
            string sql = "";
            string GrpUserVal = "";
            string GrpCode = "";
            string LibCollection = "";
            Hashtable hsLibcode = new Hashtable();
            if (singleuser.ToLower() == "true")
            {
                sql = "SELECT DISTINCT lib_code from lib_privileges where user_code=" + usercode + " and lib_code in (select lib_code from library where college_code=" + coll_Code + ")";
                ds.Clear();
                ds = da.select_method_wo_parameter(sql, "text");
            }
            else
            {
                string[] groupUser = groupuser.Split(';');
                if (groupUser.Length > 0)
                {
                    if (groupUser.Length == 1)
                    {
                        sql = "SELECT DISTINCT lib_code from lib_privileges where group_code=" + groupUser[0] + "";
                        ds.Clear();
                        ds = da.select_method_wo_parameter(sql, "text");
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
                        ds = da.select_method_wo_parameter(sql, "text");
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
            bindlibrary(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }

    protected void grdBackVol_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        grdBackVol.PageIndex = e.NewPageIndex;
        btngo_Click(sender, e);
    }

    protected void grdBackVol_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
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

    protected void grdBackVol_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            string accno1 = string.Empty;
            string fromyear = string.Empty;
            string toyear = string.Empty;
            string frtoyr = string.Empty;
            string frmonth = string.Empty;
            string tomont = string.Empty;
            string month = string.Empty;
            string sql = "";
            string libCode = Convert.ToString(ddllibrary.SelectedValue);

            if (Convert.ToString(rowIndex) != "")
            {
                chkjour.Visible = false;
                btnupdate.Visible = true;
                btndelete.Visible = true;
                Btnsave.Visible = false;
                Btnclose.Visible = true;
                divPopAlertbackvolume.Visible = true;
                divPopAlertback.Visible = true;
                divPopupAlert.Visible = false;
                divPopAlertbackvolume.Visible = true;
                divPopAlertback.Visible = true;
                Txtaccno.Text = grdBackVol.Rows[rowIndex].Cells[1].Text;
                string accNo = grdBackVol.Rows[rowIndex].Cells[1].Text;
                sql = "select access_date,journal_year,title,publisher,remarks,issueno,volumeno,monthpub,issue_flag,periodicalname,frommonthpub,tomonthpub,missmonthyear,billno,billdate,contents,journal_code,periodicalname from back_volume where lib_code='" + libCode + "' and access_code='" + accNo + "'";
                DataSet dsBack = da.select_method_wo_parameter(sql, "Text");
                if (dsBack.Tables[0].Rows.Count > 0)
                {
                    Txttile.Text = grdBackVol.Rows[rowIndex].Cells[2].Text;
                    txt_accessdate2.Text = grdBackVol.Rows[rowIndex].Cells[3].Text;
                    frtoyr = grdBackVol.Rows[rowIndex].Cells[4].Text;
                    string[] years = frtoyr.Split('-');
                    if (years.Length > 1)
                    {
                        Txtfromyear.Text = years[0];
                        Txttoyear.Text = years[1];
                    }
                    else
                    {
                        Txtfromyear.Text = frtoyr;
                    }
                    if (!string.IsNullOrEmpty(Txtfromyear.Text))
                    {
                        Txtyear.Text = Txtfromyear.Text;
                    }
                    txtjournal.Text = Convert.ToString(dsBack.Tables[0].Rows[0]["journal_code"]);
                    txtremark.Text = Convert.ToString(dsBack.Tables[0].Rows[0]["remarks"]);
                    string status = grdBackVol.Rows[rowIndex].Cells[6].Text;
                    Txtvolumeno.Text = Convert.ToString(dsBack.Tables[0].Rows[0]["volumeno"]);
                    txtissueno.Text = Convert.ToString(dsBack.Tables[0].Rows[0]["issueno"]);
                    string periodicalname = Convert.ToString(dsBack.Tables[0].Rows[0]["periodicalname"]);
                    if (!string.IsNullOrEmpty(periodicalname))
                    {
                        ddlperidical.ClearSelection();
                        ddlperidical.Items.FindByText(periodicalname).Selected = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void hide_click(Object sender, EventArgs e)
    {
        divissue.Visible = false;
        divissues.Visible = false;
        div1issue.Visible = false;
        GrdIssues.Visible = false;
    }

    protected void Txtaccno_OnTextChanged(Object sender, EventArgs e)
    {
        try
        {
            string accNo = Txtaccno.Text;
            string libCode = Convert.ToString(ddllibrary.SelectedValue);
            string fromyear = string.Empty;
            string toyear = string.Empty;
            string frtoyr = string.Empty;
            string frmonth = string.Empty;
            string tomont = string.Empty;
            string month = string.Empty;
            string sql = "select access_date,journal_year,title,publisher,remarks,issueno,volumeno,monthpub,issue_flag,periodicalname,frommonthpub,tomonthpub,missmonthyear,billno,billdate,contents,journal_code,periodicalname from back_volume where lib_code='" + libCode + "' and access_code='" + accNo + "'";
            DataSet dsBack = da.select_method_wo_parameter(sql, "Text");
            if (dsBack.Tables[0].Rows.Count > 0)
            {
                Txttile.Text = Convert.ToString(dsBack.Tables[0].Rows[0]["title"]);
                string date = Convert.ToString(dsBack.Tables[0].Rows[0]["access_date"]);
                string[] dat = date.Split('/');
                if (dat.Length == 3)
                    date = dat[1] + '/' + dat[0] + '/' + dat[2];
                txt_accessdate2.Text = date.Split(' ')[0];
                frtoyr = Convert.ToString(dsBack.Tables[0].Rows[0]["journal_year"]);
                string[] years = frtoyr.Split('-');
                if (years.Length > 1)
                {
                    Txtfromyear.Text = years[0];
                    Txttoyear.Text = years[1];
                }
                else
                {
                    Txtfromyear.Text = frtoyr;
                }
                if (!string.IsNullOrEmpty(Txtfromyear.Text))
                {
                    Txtyear.Text = Txtfromyear.Text;
                }
                txtjournal.Text = Convert.ToString(dsBack.Tables[0].Rows[0]["journal_code"]);
                txtremark.Text = Convert.ToString(dsBack.Tables[0].Rows[0]["remarks"]);
                string status = Convert.ToString(dsBack.Tables[0].Rows[0]["issue_flag"]);
                Txtvolumeno.Text = Convert.ToString(dsBack.Tables[0].Rows[0]["volumeno"]);
                txtissueno.Text = Convert.ToString(dsBack.Tables[0].Rows[0]["issueno"]);
                string periodicalname = Convert.ToString(dsBack.Tables[0].Rows[0]["periodicalname"]);
                if (!string.IsNullOrEmpty(periodicalname))
                {
                    ddlperidical.ClearSelection();
                    ddlperidical.Items.FindByText(periodicalname).Selected = true;
                }

            }
        }
        catch (Exception ex) { }
    }

}

