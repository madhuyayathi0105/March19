using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Configuration;
public partial class GatePassEntryExitReportOthers : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string collegecode = string.Empty;
    string usercode = string.Empty;
    static int memCode = 0;
    Boolean Cellclick = false;
    ArrayList colord = new ArrayList();
    static string clgCode = string.Empty;
    static string destin = string.Empty;
    static string deptmar = string.Empty;
    static string staftype = string.Empty;
    static string othss = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            //string PageLogOut = "";
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("ReqM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["Request"].Trim();
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

            if (!Request.FilePath.Contains("RequestHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/RequestMOD/RequestHome.aspx");
                    return;
                }
            }

            //****************************************************//
            collegecode = Convert.ToString(Session["collegecode"]);
            usercode = Convert.ToString(Session["usercode"]);
            if (!IsPostBack)
            {
                bindcollege();
                memtype();
                loadstatus();
                loadColumnOrder();
                loadhour();
                loadmin();
                loadrequesttype();
                dept();
                destination();
                bind_stafType1();
                bind_stafType2();
            }
        }
        catch (Exception ex) { }
    }
    #region college
    public void bindcollege()
    {
        try
        {
            ds.Clear();
            cbl_col.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_col.DataSource = ds;
                cbl_col.DataTextField = "collname";
                cbl_col.DataValueField = "college_code";
                cbl_col.DataBind();
                if (cbl_col.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_col.Items.Count; row++)
                    {
                        cbl_col.Items[row].Selected = true;
                    }
                    cb_col.Checked = true;
                    txt_col.Text = "College(" + cbl_col.Items.Count + ")";
                }
                else
                {
                    cb_col.Checked = false;
                    txt_col.Text = "--Select--";
                }
                clgCode = Convert.ToString(getCblSelectedValue(cbl_col));
            }
            else
            {
                cb_col.Checked = false;
                txt_col.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void cb_col_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_col, cbl_col, txt_col, "college", "--Select--");
        clgCode = Convert.ToString(getCblSelectedValue(cbl_col));
    }
    protected void cbl_col_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_col, cbl_col, txt_col, "college", "--Select--");
        clgCode = Convert.ToString(getCblSelectedValue(cbl_col));
    }
    protected void cb_app_status_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_app_status, cbl_app_status, txt_app_status, "Request", "--Select--");
        clgCode = Convert.ToString(getCblSelectedValue(cbl_col));
    }
    protected void cb_app_status_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_app_status, cbl_app_status, txt_app_status, "Request", "--Select--");
        clgCode = Convert.ToString(getCblSelectedValue(cbl_col));
    }

    #endregion
    #region new
    private void memtype()
    {
        try
        {
            ddlmemtype.Items.Clear();
            ddlmemtype.Items.Add(new ListItem("Staff", "2"));
            ddlmemtype.Items.Add(new ListItem("Parents", "3"));
            ddlmemtype.Items.Add(new ListItem("Visitor", "4"));
            ddlmemtype.Items.Add(new ListItem("Material", "5"));
            ddlmemtype.Items.Add(new ListItem("Vehicle", "6"));
            //auto search binding based on memtype
            if (ddlmemtype.Items.Count > 0)
                getAutoSerach(ddlmemtype.SelectedItem.Text, ddlmemtype.SelectedItem.Value);
        }
        catch { }
    }
    protected void ddlmemtype_Selected(object sender, EventArgs e)
    {
        //auto search binding based on memtype
        if (ddlmemtype.Items.Count > 0)
            getAutoSerach(ddlmemtype.SelectedItem.Text, ddlmemtype.SelectedItem.Value);
        loadColumnOrder();
        if (ddlmemtype.SelectedItem.Value == "4")
        {
           // visi.Style.Add("display", "block");
            visi.Visible = true;
        }
        else
           visi.Visible = false;
           // visi.Style.Add("display", "None");
        loadstatus();
    }
    #endregion
    #region status
    public void loadstatus()
    {
        try
        {
            string gatetype, gatetype1 = "";
            if (ddlmemtype.SelectedItem.Value == "2")
            {
                gatetype = "0";
                gatetype1 = "1";
            }
            else
            {
                gatetype = "1";
                gatetype1 = "0";
            }
            cbl_status.Items.Clear();
            cbl_status.Items.Add(new ListItem("In", gatetype));
            cbl_status.Items.Add(new ListItem("Out", gatetype1));
            for (int i = 0; i < cbl_status.Items.Count; i++)
            {
                cbl_status.Items[i].Selected = true;
            }
            txt_status.Text = "Status(" + cbl_status.Items.Count + ")";
            cb_status.Checked = true;
        }
        catch { }
    }
    protected void loadrequesttype()
    {
        cbl_app_status.Items.Clear();
        cbl_app_status.Items.Add(new ListItem("Un Approved", "0"));
        cbl_app_status.Items.Add(new ListItem("Approved", "1"));
        for (int i = 0; i < cbl_app_status.Items.Count; i++)
        {
            cbl_app_status.Items[i].Selected = true;
        }
        txt_app_status.Text = "Request(" + cbl_app_status.Items.Count + ")";
        cb_app_status.Checked = true;
    }
    protected void cb_status_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_status, cbl_status, txt_status, "Status", "--Select--");
    }
    protected void cbl_status_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_status, cbl_status, txt_status, "Status", "--Select--");
    }
    #endregion
    #region auto search
    protected void getAutoSerach(string txt, string val)
    {
        ddlsearch.Items.Clear();
        ddlsearch.Items.Add(new ListItem(txt, val));
        if (ddlsearch.Items.Count > 0)
            AutoSearch(txt);
    }
    protected void ddlsearch_OnSelected(object sender, EventArgs e)
    {
        if (ddlsearch.Items.Count > 0)
        {
            AutoSearch(ddlsearch.SelectedItem.Text);
        }
    }
    protected void AutoSearch(string txt)
    {
        switch (txt)
        {
            case "Staff":
                txtsearch.Attributes.Add("placeholder", "Staff");
                memCode = 1;
                break;
            case "Parents":
                txtsearch.Attributes.Add("placeholder", "Parents");
                memCode = 2;
                break;
            case "Visitor":
                txtsearch.Attributes.Add("placeholder", "Visitor");
                memCode = 3;
                break;
            case "Material":
                txtsearch.Attributes.Add("placeholder", "Material");
                memCode = 4;
                break;
            case "Vehicle":
                txtsearch.Attributes.Add("placeholder", "Vehicle");
                memCode = 5;
                break;
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetRoll(string prefixText)
    {
        WebService ws = new WebService();
        List<string> roll = new List<string>();
        string SelQ = string.Empty;
        if (memCode == 1)
        {
            SelQ = " select (staff_code+'$'+sm.staff_name) as staff,sm.staff_code from staffmaster sm,staff_appl_master sa where sm.appl_no=sa.appl_no and sm.college_code in('" + clgCode + "') and sm.staff_code like '" + prefixText + "%' order by sm.staff_code asc ";
        }
        else if (memCode == 2)
        {
            SelQ = "";
        }
        else if (memCode == 3)
        {
            SelQ = " select CompanyName+'$'+VisitorName from GateEntryExit where GateMemType='4' and companyname like '" + prefixText + "%'  order by CompanyName  ";
        }
        else if (memCode == 4)
        {
            SelQ = "";
        }
        else if (memCode == 5)
        {
            SelQ = "";
        }
        //  SelQ = "select distinct top(10) Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%'";
        if (SelQ.Trim() != "")
        {
            roll = ws.Getname(SelQ);
        }
        return roll;
    }
    #endregion
    protected void cbdtfrom_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cbdtfrom.Checked)
        {
            txtfrmdt.Enabled = true;
            txttodt.Enabled = true;
            txtfrmdt.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txttodt.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtfrmdt.Attributes.Add("readonly", "readonly");
            txttodt.Attributes.Add("readonly", "readonly");
        }
        else
        {
            txtfrmdt.Enabled = false;
            txttodt.Enabled = false;
        }
    }
    #region time
    protected void cbtime_Changed(object sender, EventArgs e)
    {
        if (cbtime.Checked)
        {
            ddlhourfr.Enabled = true;
            ddlminsfr.Enabled = true;
            ddlsecsfr.Enabled = true;
            ddlhourto.Enabled = true;
            ddlminsto.Enabled = true;
            ddlsecsto.Enabled = true;
        }
        else
        {
            ddlhourfr.Enabled = false;
            ddlminsfr.Enabled = false;
            ddlsecsfr.Enabled = false;
            ddlhourto.Enabled = false;
            ddlminsto.Enabled = false;
            ddlsecsto.Enabled = false;
        }
    }
    public void loadhour()
    {
        ddlhourfr.Items.Clear();
        ddlhourto.Items.Clear();
        for (int i = 1; i <= 12; i++)
        {
            ddlhourfr.Items.Add(Convert.ToString(i));
            ddlhourto.Items.Add(Convert.ToString(i));
            ddlhourfr.SelectedIndex = ddlhourfr.Items.Count - 1;
            ddlhourto.SelectedIndex = ddlhourto.Items.Count - 1;
        }
    }
    public void loadmin()
    {
        ddlminsfr.Items.Clear();
        ddlminsto.Items.Clear();
        for (int i = 0; i <= 59; i++)
        {
            string val = Convert.ToString(i);
            if (val.Length == 1)
            {
                val = "0" + val;
            }
            ddlminsfr.Items.Add(val);
            ddlminsto.Items.Add(val);
        }
    }
    public void timevalue()
    {
        string time = DateTime.Now.ToString("HH:mm:ss");
        string hrr = "";
        string[] ay = time.Split(':');
        string val_hr = ay[0].ToString();
        int hr = Convert.ToInt16(val_hr);
        if (val_hr == "01")
        {
            hrr = "1";
        }
        else if (val_hr == "02")
        {
            hrr = "2";
        }
        else if (val_hr == "03")
        {
            hrr = "3";
        }
        else if (val_hr == "04")
        {
            hrr = "4";
        }
        else if (val_hr == "05")
        {
            hrr = "5";
        }
        else if (val_hr == "06")
        {
            hrr = "6";
        }
        else if (val_hr == "07")
        {
            hrr = "7";
        }
        else if (val_hr == "08")
        {
            hrr = "8";
        }
        else if (val_hr == "09")
        {
            hrr = "9";
        }
        else if (val_hr == "13")
        {
            hrr = "1";
        }
        else if (val_hr == "14")
        {
            hrr = "2";
        }
        else if (val_hr == "15")
        {
            hrr = "3";
        }
        else if (val_hr == "16")
        {
            hrr = "4";
        }
        else if (val_hr == "17")
        {
            hrr = "5";
        }
        else if (val_hr == "18")
        {
            hrr = "6";
        }
        else if (val_hr == "19")
        {
            hrr = "7";
        }
        else if (val_hr == "20")
        {
            hrr = "8";
        }
        else if (val_hr == "21")
        {
            hrr = "9";
        }
        else if (val_hr == "22")
        {
            hrr = "10";
        }
        else if (val_hr == "23")
        {
            hrr = "11";
        }
        else if (val_hr == "24")
        {
            hrr = "12";
        }
        if (val_hr == "10" || val_hr == "11" || val_hr == "12")
        {
            ddlhourfr.Text = val_hr;
            ddlminsfr.Text = ay[1].ToString();
            ddlhourto.Text = val_hr;
            ddlminsto.Text = ay[1].ToString();
        }
        else
        {
            ddlhourfr.Text = hrr;
            ddlminsfr.Text = ay[1].ToString();
            ddlhourto.Text = hrr;
            ddlminsto.Text = ay[1].ToString();
        }
        if (val_hr == "12" || val_hr == "13" || val_hr == "14" || val_hr == "15" || val_hr == "16" || val_hr == "17" || val_hr == "18" || val_hr == "19" || val_hr == "20" || val_hr == "21" || val_hr == "22" || val_hr == "23" || val_hr == "24")
        {
            ddlsecsfr.Text = "PM";
            ddlsecsto.Text = "PM";
        }
        else
        {
            ddlsecsfr.Text = "AM";
            ddlsecsto.Text = "AM";
        }
    }
    #endregion
    protected DataSet loadDetails()
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get Details
            string memType = string.Empty;
            string Status = string.Empty;
            string gateType = string.Empty;
            string getentry = string.Empty;
            string getexit = string.Empty;
            string getOther = string.Empty;
            string request = string.Empty;
            if (cbl_col.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cbl_col));
            if (ddlmemtype.Items.Count > 0)
                memType = Convert.ToString(ddlmemtype.SelectedItem.Value);
            if (cbl_status.Items.Count > 0)
                Status = Convert.ToString(getCblSelectedValue(cbl_status));
            if (cbl_app_status.Items.Count > 0)
                request = Convert.ToString(getCblSelectedValue(cbl_app_status));

            DateTime dtfrm = new DateTime();
            DateTime dtto = new DateTime();
            if (cbdtfrom.Checked == true)
            {
                string frmdt = txtfrmdt.Text;
                string[] splfrm = frmdt.Split('/');
                string newfrm = Convert.ToString(splfrm[1] + "/" + splfrm[0] + "/" + splfrm[2]);
                dtfrm = Convert.ToDateTime(newfrm);
                string todt = txttodt.Text;
                string[] splto = todt.Split('/');
                string newto = Convert.ToString(splto[1] + "/" + splto[0] + "/" + splto[2]);
                dtto = Convert.ToDateTime(newto);
            }
            DateTime getentrydt = new DateTime(); DateTime getexitdt = new DateTime();
            if (cbtime.Checked == true)
            {
                getentry = ddlhourfr.SelectedItem.Text + ":" + ddlminsfr.SelectedItem.Text + " " + ddlsecsfr.SelectedItem.Text;
                getexit = ddlhourto.SelectedItem.Text + ":" + ddlminsto.SelectedItem.Text + " " + ddlsecsto.SelectedItem.Text;
                DateTime.TryParse(getentry, out getentrydt);
                DateTime.TryParse(getexit, out getexitdt);
            }
            #endregion
            #region getfileter query
            if (cbentry.Checked == true && cbexit.Checked == true)
            {
                if (cbdtfrom.Checked == true && cbtime.Checked == true)
                {
                    getOther = getOther + " and g.GatepassEntrydate='" + dtfrm.ToString("MM/dd/yyyy") + "' and g.GatepassExitdate='" + dtto.ToString("MM/dd/yyyy") + "' and CONVERT(nvarchar(100),GatepassEntrytime ,100) >= '" + getentrydt.ToString("hh:mm tt") + "' and CONVERT(nvarchar(100),GatepassExittime ,100) <='" + getexitdt.ToString("hh:mm tt") + "'";
                }
                else if (cbdtfrom.Checked == true)
                {
                    getOther = getOther + " and g.GatepassEntrydate='" + dtfrm.ToString("MM/dd/yyyy") + "' and g.GatepassExitdate='" + dtto.ToString("MM/dd/yyyy") + "'";
                }
                else if (cbtime.Checked == true)
                {
                    getOther = getOther + " and CONVERT(nvarchar(100),GatepassEntrytime ,100) >= '" + getentrydt.ToString("hh:mm tt") + "' and CONVERT(nvarchar(100),GatepassExittime ,100) <='" + getexitdt.ToString("hh:mm tt") + "'";
                }
            }
            else if (cbentry.Checked == true)
            {
                if (cbdtfrom.Checked == true && cbtime.Checked == true)
                {
                    getOther = getOther + " and g.GatepassEntrydate between '" + dtfrm.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "' and CONVERT(nvarchar(100),GatepassEntrytime ,100) between '" + getentrydt.ToString("hh:mm tt") + "' and '" + getexitdt.ToString("hh:mm tt") + "'";
                }
                else if (cbdtfrom.Checked == true)
                {
                    getOther = getOther + " and g.GatepassEntrydate between '" + dtfrm.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "'";
                }
                else if (cbtime.Checked == true)
                {
                    getOther = getOther + " and CONVERT(nvarchar(100),GatepassEntrytime ,100) between '" + getentrydt.ToString("hh:mm tt") + "' and '" + getexitdt.ToString("hh:mm tt") + "'";
                }
            }
            else if (cbexit.Checked == true)
            {
                if (cbdtfrom.Checked == true && cbtime.Checked == true)
                {
                    getOther = getOther + " and g.GatepassExitdate between '" + dtfrm.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "' and CONVERT(nvarchar(100),GatepassExittime ,100) between '" + getentrydt.ToString("hh:mm tt") + "' and '" + getexitdt.ToString("hh:mm tt") + "'";
                }
                else if (cbdtfrom.Checked == true)
                {
                    getOther = getOther + " and g.GatepassExitdate between '" + dtfrm.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "'";
                }
                else if (cbtime.Checked == true)
                {
                    getOther = getOther + " and CONVERT(nvarchar(100),GatepassExittime ,100) between '" + getentrydt.ToString("hh:mm tt") + "' and '" + getexitdt.ToString("hh:mm tt") + "'";
                }
            }
            #endregion
            string SelQ = string.Empty;
            string staffapplId = string.Empty;
            string gatetype = ""; string gatetype1 = "";
            if (ddlmemtype.SelectedItem.Value == "2")
            {
                gatetype = "In";
                gatetype1 = "Out";
                if (txtsearch.Text.Trim() != "")
                {
                    string stafCode = Convert.ToString(txtsearch.Text.Split('$')[0]);
                    staffapplId = Convert.ToString(d2.GetFunction("select appl_id from staffmaster sm,staff_appl_master sa where sm.appl_no=sa.appl_no and sm.college_code in('" + collegecode + "') and sm.staff_code='" + stafCode + "'"));
                }
            }
            else
            {
                gatetype1 = "In";
                gatetype = "Out";
                if (txtsearch.Text.Trim() != "")
                {
                    string stafCode = Convert.ToString(txtsearch.Text.Split('$')[1]);
                    getOther = " and VisitorName='" + stafCode + "'";
                }
            }
            
             string stname = Convert.ToString(getCblSelectedValue(cblstaffname));
             if (cbl_app_status.SelectedValue == "0")
             SelQ = " select distinct case when g.ToMeet='0' then 'Staff' when g.ToMeet='1' then 'Office' when g.ToMeet='2' then 'Others' end Tomeet,g.RelationShip,g.CompanyName,g.VisitorName,g.gatepassno,g.App_No ,convert(varchar,GatepassExitdate,103) as GatepassExitdate,g.GatepassExittime,convert(varchar,GatepassEntrydate,103) as GatepassEntrydate,g.GatepassEntrytime,CASE WHEN gatetype = 1 THEN '" + gatetype1 + "' when gatetype=0  then '" + gatetype + "' END gatetype,CASE WHEN isapproval=1 THEN 'Approved' when isapproval=0 then 'Un Approved' else 'Un Approved' End ReqAppStatus,CASE WHEN islate=0 THEN 'On Time' WHEN islate=1 THEN 'Late Time' End islate,convert(varchar,GatePassDate,103) as 'GatePassDate',g.Purpose,g.GatePassTime,g.ExpectedTime,convert(varchar,ExpectedDate,103) as 'ExpectedDate',g.college_code,requestfk,g.MobileNo,isapproval,tomeet  from  GateEntryExit g,GateEntryExitDet gd  where g.GateMemType='" + memType + "' and g.college_code in('" + collegecode + "') and gd.GateEntryExitID=g.GateEntryExitID and gd.Staff_Code in('" + stname + "') ";
             else if (cbl_app_status.SelectedValue == "1")
                 SelQ = "select distinct case when g.ToMeet='0' then 'Staff' when g.ToMeet='1' then 'Office' when g.ToMeet='2' then 'Others' end Tomeet,g.RelationShip,g.CompanyName,g.VisitorName,g.gatepassno,g.App_No ,convert(varchar,GatepassExitdate,103) as GatepassExitdate,g.GatepassExittime,convert(varchar,GatepassEntrydate,103) as GatepassEntrydate,g.GatepassEntrytime,CASE WHEN gatetype = 1 THEN '" + gatetype1 + "' when gatetype=0  then '" + gatetype + "' END gatetype,CASE WHEN isapproval=1 THEN 'Approved' when isapproval=0 then 'Un Approved' else 'Un Approved' End ReqAppStatus,CASE WHEN islate=0 THEN 'On Time' WHEN islate=1 THEN 'Late Time' End islate,convert(varchar,GatePassDate,103) as 'GatePassDate',g.Purpose,g.GatePassTime,g.ExpectedTime,convert(varchar,ExpectedDate,103) as 'ExpectedDate',g.college_code,requestfk,g.MobileNo,isapproval,tomeet  from  GateEntryExit g,RQ_Requisition R   where g.GateMemType='" + memType + "' and g.college_code in('" + collegecode + "') and (r.MeetStaffAppNo=(select appl_no from staffmaster where staff_code='"+stname+"')) and r.RequisitionPK=g.RequestFk ";
             else
             SelQ = " select distinct case when g.ToMeet='0' then 'Staff' when g.ToMeet='1' then 'Office' when g.ToMeet='2' then 'Others' end Tomeet,g.RelationShip,g.CompanyName,g.VisitorName,g.gatepassno,g.App_No ,convert(varchar,GatepassExitdate,103) as GatepassExitdate,g.GatepassExittime,convert(varchar,GatepassEntrydate,103) as GatepassEntrydate,g.GatepassEntrytime,CASE WHEN gatetype = 1 THEN '" + gatetype1 + "' when gatetype=0  then '" + gatetype + "' END gatetype,CASE WHEN isapproval=1 THEN 'Approved' when isapproval=0 then 'Un Approved' else 'Un Approved' End ReqAppStatus,CASE WHEN islate=0 THEN 'On Time' WHEN islate=1 THEN 'Late Time' End islate,convert(varchar,GatePassDate,103) as 'GatePassDate',g.Purpose,g.GatePassTime,g.ExpectedTime,convert(varchar,ExpectedDate,103) as 'ExpectedDate',g.college_code,requestfk,g.MobileNo,isapproval,tomeet  from  GateEntryExit g,GateEntryExitDet gd  where g.GateMemType='" + memType + "' and g.college_code in('" + collegecode + "') and gd.GateEntryExitID=g.GateEntryExitID and gd.Staff_Code in('" + stname + "') ";
             if (ddlmemtype.SelectedItem.Value == "4")
             {
                 if (othss.ToUpper() == "YES")
                     SelQ += " or ToMeet='2'";
             }
            if (!string.IsNullOrEmpty(staffapplId) && staffapplId.Trim() != "0")
                SelQ += " and g.app_no='" + staffapplId + "'";
            if (!string.IsNullOrEmpty(getOther))
                SelQ += getOther;
            if (Status.Trim() != "")
                SelQ += " and GateType in('" + Status + "')";
            if (request.Trim() != "")
                SelQ += " and isnull(isapproval,0) in('" + request + "')";
            if (ddlmemtype.SelectedItem.Value == "4")
                SelQ += "order by g.gatepassno";
            SelQ += "  select appl_id,sm.staff_name,staff_code,sm.college_code from staffmaster sm,staff_appl_master sa where sm.appl_no=sa.appl_no and sm.college_code in('" + collegecode + "') ";//and sa.appl_id='332'
            

            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelQ, "Text");
        }
        catch { }
        return dsload;
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        ds.Clear();
        ds = loadDetails();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            loadSpreadDetails(ds);
            txtsearch.Text = "";
            spreadDet.Sheets[0].FrozenColumnCount =3;
            
        }
        else
        {
            divspread.Visible = false;
            print.Visible = false;
            txtexcelname.Text = string.Empty;
            lblvalidation1.Text = string.Empty;
            divcolorder.Visible = false;
            lbl_alert.Text = "No Record Found";
            alertDiv.Visible = true;
        }
    }
    protected void loadSpreadDetails(DataSet ds)
    {
        try
        {
            #region design
            loadcolumns();
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 1;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 16;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            spreadDet.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            int check = 0;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[0].Width = 50;
            string headername = ""; string headername1 = ""; string headername3 = "";

            if (ddlmemtype.SelectedItem.Value == "2")
            {
                headername = "Staff Code";
                headername1 = "Staff Name";
            }
            if (ddlmemtype.SelectedItem.Value == "3")
            {
                headername = "Relationship of Student";
                headername1 = "Meet";
            }
            if (ddlmemtype.SelectedItem.Value == "4")
            {
                headername3 = "Gatepass No";
                headername = "Company Name";
                headername1 = "Visitor Name";
            }
            int n = 1;
            if (ddlmemtype.SelectedItem.Value == "4")
            {
                spreadDet.Sheets[0].ColumnCount = 17;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Text = "Select";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[n].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[n].Width = 50;
            }
            n++;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Text = headername;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0,n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].HorizontalAlign = HorizontalAlign.Center;
            //if (!colord.Contains("2"))
            //    spreadDet.Sheets[0].Columns[n].Visible = false;
            //if (colord.Count == 0)
            //    spreadDet.Sheets[0].Columns[n].Visible = true;
            n++;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Text = headername1;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            //if (!colord.Contains("3"))
            //    spreadDet.Sheets[0].Columns[n].Visible = false;
            //if (colord.Count == 0)
            //    spreadDet.Sheets[0].Columns[n].Visible = true;
            if (ddlmemtype.SelectedItem.Value == "4")
            {
                n++;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Text = "Gatepass No";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[n].HorizontalAlign = HorizontalAlign.Center;
                //if (!colord.Contains(n))
                //    spreadDet.Sheets[0].Columns[n].Visible = false;
                //if (colord.Count == 0)
                //    spreadDet.Sheets[0].Columns[n].Visible = true;
            }
            n++;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Text = "Purpose";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].HorizontalAlign = HorizontalAlign.Center;
            //if (!colord.Contains(n))
            //    spreadDet.Sheets[0].Columns[n].Visible = false;
            //if (colord.Count == 0)
            //    spreadDet.Sheets[0].Columns[n].Visible = true;
            n++;
            spreadDet.Sheets[0].ColumnHeader.Cells[0,n].Text = "Approved EntryDate";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].Visible = true;
            //if (!colord.Contains(n))
            //    spreadDet.Sheets[0].Columns[n].Visible = false;
            //if (colord.Count == 0)
            //    spreadDet.Sheets[0].Columns[n].Visible = true;
            n++;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Text = "Approved EntryTime";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].Visible = true;
            //if (!colord.Contains(n))
            //    spreadDet.Sheets[0].Columns[n].Visible = false;
            //if (colord.Count == 0)
            //    spreadDet.Sheets[0].Columns[n].Visible = true;
            n++;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Text = "Approved ExitDate";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].Visible = true;
            //if (!colord.Contains(n))
            //    spreadDet.Sheets[0].Columns[n].Visible = false;
            //if (colord.Count == 0)
            //    spreadDet.Sheets[0].Columns[n].Visible = true;
            n++;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Text = "Approved ExitTime";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].Visible = true;
            //if (!colord.Contains(n))
            //    spreadDet.Sheets[0].Columns[n].Visible = false;
            //if (colord.Count == 0)
            //    spreadDet.Sheets[0].Columns[n].Visible = true;
            //spreadDet.Sheets[0].Columns[n].Visible = false;
            n++;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Text = "Entry Date";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].Visible = true;
            //if (!colord.Contains(n))
            //    spreadDet.Sheets[0].Columns[n].Visible = false;
            //if (colord.Count == 0)
            //    spreadDet.Sheets[0].Columns[n].Visible = true;
            n++;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Text = "Entry Time";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].Visible = true;
            //if (!colord.Contains(n))
            //    spreadDet.Sheets[0].Columns[n].Visible = false;
            //if (colord.Count == 0)
            //    spreadDet.Sheets[0].Columns[n].Visible = true;
            n++;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Text = "Exit Date";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].Visible = true;
            //if (!colord.Contains(n))
            //    spreadDet.Sheets[0].Columns[n].Visible = false;
            //if (colord.Count == 0)
            //    spreadDet.Sheets[0].Columns[n].Visible = true;

            n++;


            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Text = "Exit Time";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].Visible = true;
            //if (!colord.Contains(n))
            //    spreadDet.Sheets[0].Columns[n].Visible = false;
            //if (colord.Count == 0)
            //    spreadDet.Sheets[0].Columns[n].Visible = true;
            n++;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Text = "Entered Time";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].Visible = true;
            //if (!colord.Contains(n))
            //    spreadDet.Sheets[0].Columns[n].Visible = false;
            //if (colord.Count == 0)
            //    spreadDet.Sheets[0].Columns[n].Visible = true;

            n++;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Text = "Status";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].Visible = true;
            //if (!colord.Contains(n))
            //    spreadDet.Sheets[0].Columns[n].Visible = false;
            //if (colord.Count == 0)
            //    spreadDet.Sheets[0].Columns[n].Visible = true;
            n++;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Text = "Approved Status";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[n].Visible = true;
            if (ddlmemtype.SelectedItem.Value == "4")
            {
                spreadDet.Sheets[0].ColumnCount += 6;
                n++;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Text = "Visitor Mobile Number";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[n].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[n].Visible = true;
                n++;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Text = "Meet To Department";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[n].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[n].Visible = true;
                n++;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Text = "Meet To Staff";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[n].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[n].Visible = true;
                n++;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Text = "Staff Code";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[n].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[n].Visible = true;
                n++;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Text = "Staff Communication Mobile Number";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[n].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[n].Visible = true;
                n++;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Text = "Staff Permanent Mobile Number";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, n].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[n].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[n].Visible = true;
            }
            //if (!colord.Contains(n))
            //    spreadDet.Sheets[0].Columns[n].Visible = false;
            //if (colord.Count == 0)
            //    spreadDet.Sheets[0].Columns[n].Visible = true;
            if (ddlmemtype.SelectedItem.Value == "4")
            {
                if (!colord.Contains("1"))
                    spreadDet.Sheets[0].Columns[2].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[2].Visible = true;
                if (!colord.Contains("2"))
                    spreadDet.Sheets[0].Columns[3].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[3].Visible = true;
                if (!colord.Contains("3"))
                    spreadDet.Sheets[0].Columns[4].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[4].Visible = true;

                if (!colord.Contains("4"))
                    spreadDet.Sheets[0].Columns[5].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[5].Visible = true;

                if (!colord.Contains("5"))
                    spreadDet.Sheets[0].Columns[6].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[6].Visible = true;


                if (!colord.Contains("6"))
                    spreadDet.Sheets[0].Columns[7].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[7].Visible = true;

                if (!colord.Contains("7"))
                    spreadDet.Sheets[0].Columns[8].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[8].Visible = true;

                if (!colord.Contains("8"))
                    spreadDet.Sheets[0].Columns[9].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[9].Visible = true;

                if (!colord.Contains("9"))
                    spreadDet.Sheets[0].Columns[10].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[10].Visible = true;

               

                if (!colord.Contains("10"))
                    spreadDet.Sheets[0].Columns[11].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[11].Visible = true;

                if (!colord.Contains("11"))
                    spreadDet.Sheets[0].Columns[12].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[12].Visible = true;

                if (!colord.Contains("12"))
                    spreadDet.Sheets[0].Columns[13].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[13].Visible = true;
                if (!colord.Contains("13"))
                    spreadDet.Sheets[0].Columns[14].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[14].Visible = true;

                if (!colord.Contains("14"))
                    spreadDet.Sheets[0].Columns[15].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[15].Visible = true;

                if (!colord.Contains("15"))
                    spreadDet.Sheets[0].Columns[16].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[16].Visible = true;
                if (!colord.Contains("16"))
                    spreadDet.Sheets[0].Columns[17].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[17].Visible = true;
                if (!colord.Contains("17"))
                    spreadDet.Sheets[0].Columns[18].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[18].Visible = true;
                if (!colord.Contains("18"))
                    spreadDet.Sheets[0].Columns[19].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[19].Visible = true;
                if (!colord.Contains("19"))
                    spreadDet.Sheets[0].Columns[20].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[20].Visible = true;
                if (!colord.Contains("20"))
                    spreadDet.Sheets[0].Columns[21].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[21].Visible = true;
                if (!colord.Contains("21"))
                    spreadDet.Sheets[0].Columns[22].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[22].Visible = true;
              

            }
            else
            {
                if (!colord.Contains("1"))
                    spreadDet.Sheets[0].Columns[1].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[1].Visible = true;
                if (!colord.Contains("2"))
                    spreadDet.Sheets[0].Columns[2].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[2].Visible = true;
                if (!colord.Contains("3"))
                    spreadDet.Sheets[0].Columns[3].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[3].Visible = true;

                if (!colord.Contains("4"))
                    spreadDet.Sheets[0].Columns[4].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[4].Visible = true;

                if (!colord.Contains("5"))
                    spreadDet.Sheets[0].Columns[5].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[5].Visible = true;


                if (!colord.Contains("6"))
                    spreadDet.Sheets[0].Columns[6].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[6].Visible = true;

                if (!colord.Contains("7"))
                    spreadDet.Sheets[0].Columns[7].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[7].Visible = true;

                if (!colord.Contains("8"))
                    spreadDet.Sheets[0].Columns[8].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[8].Visible = true;

                if (!colord.Contains("9"))
                    spreadDet.Sheets[0].Columns[9].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[9].Visible = true;



                if (!colord.Contains("10"))
                    spreadDet.Sheets[0].Columns[10].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[10].Visible = true;

                if (!colord.Contains("11"))
                    spreadDet.Sheets[0].Columns[11].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[11].Visible = true;

                if (!colord.Contains("12"))
                    spreadDet.Sheets[0].Columns[12].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[12].Visible = true;
                if (!colord.Contains("13"))
                    spreadDet.Sheets[0].Columns[13].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[13].Visible = true;

                if (!colord.Contains("14"))
                    spreadDet.Sheets[0].Columns[14].Visible = false;
                if (colord.Count == 0)
                    spreadDet.Sheets[0].Columns[14].Visible = true;
              

              
            }
          
            #endregion
            #region value
            int height = 0;
            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                spreadDet.Sheets[0].RowCount++;
                height += 15;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                string staffName = string.Empty;
                string staffCode = string.Empty;
                string gatepassno = string.Empty;
                n = 0;
                if (ds.Tables[1].Rows.Count > 0)
                {
                    if (ddlmemtype.SelectedItem.Value == "2")
                    {
                        ds.Tables[1].DefaultView.RowFilter = "appl_id='" + Convert.ToString(ds.Tables[0].Rows[row]["app_no"]) + "' and college_code='" + Convert.ToString(ds.Tables[0].Rows[row]["college_code"]) + "'";
                        DataView dvval = ds.Tables[1].DefaultView;
                        if (dvval.Count > 0)
                        {
                            staffName = Convert.ToString(dvval[0]["staff_name"]);
                            staffCode = Convert.ToString(dvval[0]["staff_code"]);
                        }
                    }
                    else if (ddlmemtype.SelectedItem.Value == "3")
                    {
                        staffName = Convert.ToString(ds.Tables[0].Rows[row]["RelationShip"]);
                        staffCode = Convert.ToString(ds.Tables[0].Rows[row]["Tomeet"]);
                    }
                    else
                    {
                        spreadDet.Sheets[0].AutoPostBack = false;
                        n++;
                        FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
                        staffName = Convert.ToString(ds.Tables[0].Rows[row]["CompanyName"]);
                        staffCode = Convert.ToString(ds.Tables[0].Rows[row]["VisitorName"]);
                        gatepassno = Convert.ToString(ds.Tables[0].Rows[row]["gatepassno"]);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n+3].Text = gatepassno;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n].CellType = chk;
                    }
                }
                n++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n].Text = staffName;
                n++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n].Text = staffCode;
                n++;
                if (ddlmemtype.SelectedItem.Value == "4")
                {
                    n = n + 1;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n].Text = Convert.ToString(ds.Tables[0].Rows[row]["purpose"]);
                }
                else
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n].Text = Convert.ToString(ds.Tables[0].Rows[row]["purpose"]);
                n++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n].Text = Convert.ToString(ds.Tables[0].Rows[row]["ExpectedDate"]);
                n++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n].Text = Convert.ToString(ds.Tables[0].Rows[row]["ExpectedTime"]);
                n++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n].Text = Convert.ToString(ds.Tables[0].Rows[row]["GatePassDate"]);
                n++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n].Text = "-";
                n++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n].Text = Convert.ToString(ds.Tables[0].Rows[row]["GatepassEntrydate"]);
                n++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n].Text = Convert.ToString(ds.Tables[0].Rows[row]["GatepassEntrytime"]);
                n++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n].Text = Convert.ToString(ds.Tables[0].Rows[row]["GatepassExitdate"]);
                n++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n].Text = Convert.ToString(ds.Tables[0].Rows[row]["GatepassExittime"]);
                n++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n].Text = Convert.ToString(ds.Tables[0].Rows[row]["islate"]);
                n++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n].Text = Convert.ToString(ds.Tables[0].Rows[row]["gatetype"]);
                n++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n].Text = Convert.ToString(ds.Tables[0].Rows[row]["ReqAppStatus"]);
                string approv = Convert.ToString(ds.Tables[0].Rows[row]["isapproval"]);
                string meet = Convert.ToString(ds.Tables[0].Rows[row]["tomeet"]);
                string reqpk = Convert.ToString(ds.Tables[0].Rows[row]["requestfk"]);
                string dep = string.Empty;
                string depname = string.Empty;
                string stf = string.Empty;
                string stfname = string.Empty;
                string MOb = string.Empty;
                string MObper = string.Empty;
                string Mobli = string.Empty;
                string Mobliper = string.Empty;
                string code = string.Empty;
                string code1 = string.Empty;
                DataSet steaff = new DataSet();
                DataSet steaff1 = new DataSet();
                string stafname = string.Empty;
                string mobile = string.Empty;
                string getID = string.Empty;
                string detsql = string.Empty;
                string staff_names = string.Empty; 
                if (ddlmemtype.SelectedItem.Value == "4")
                {
                    if (approv =="False")
                    {
                         getID = d2.GetFunction("select GateEntryExitID from GateEntryExit where gatepassno='" + gatepassno + "' ");
                         if (meet == "Staff")
                         {
                             detsql = d2.GetFunction(" select Staff_Code from GateEntryExitDet where GateEntryExitID='" + getID + "'");
                             if (detsql != "")
                             {
                                 staff_names = ("select s.staff_name,s.staff_code,sa.com_mobileno,sa.per_mobileno from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_code='" + detsql + "'");
                                 DataSet steafs = d2.select_method_wo_parameter(staff_names, "text");
                                 if (steafs.Tables.Count > 0 && steafs.Tables[0].Rows.Count > 0)
                                 {
                                     stafname = Convert.ToString(steafs.Tables[0].Rows[0]["staff_name"]);
                                     code1 = Convert.ToString(steafs.Tables[0].Rows[0]["staff_code"]);
                                     Mobli = Convert.ToString(steafs.Tables[0].Rows[0]["com_mobileno"]);
                                     Mobliper = Convert.ToString(steafs.Tables[0].Rows[0]["per_mobileno"]);
                                 }

                             }
                         }
                         if (meet == "Office")
                         {
                             detsql = d2.GetFunction(" select Staff_Code from GateEntryExitDet where GateEntryExitID='" + getID + "'");
                             if (detsql != "")
                             {
                                 staff_names = ("select s.staff_name,s.staff_code,sa.com_mobileno,sa.per_mobileno from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_code='" + detsql + "'");
                                 DataSet steafs = d2.select_method_wo_parameter(staff_names, "text");
                                 if (steafs.Tables.Count > 0 && steafs.Tables[0].Rows.Count > 0)
                                 {
                                     stafname = Convert.ToString(steafs.Tables[0].Rows[0]["staff_name"]);
                                     code1 = Convert.ToString(steafs.Tables[0].Rows[0]["staff_code"]);
                                     Mobli = Convert.ToString(steafs.Tables[0].Rows[0]["com_mobileno"]);
                                     Mobliper = Convert.ToString(steafs.Tables[0].Rows[0]["per_mobileno"]);
                                 }

                             }
                         }
                         if (meet == "Others")
                         {
                             detsql = " select OtherName,Relationship,MobileNo from GateEntryExitDet where GateEntryExitID='" + getID + "'";

                             DataSet steafs = d2.select_method_wo_parameter(detsql, "text");
                                 if (steafs.Tables.Count > 0 && steafs.Tables[0].Rows.Count > 0)
                                 {
                                     stafname = Convert.ToString(steafs.Tables[0].Rows[0]["OtherName"]);
                                  
                                     Mobli = Convert.ToString(steafs.Tables[0].Rows[0]["MobileNo"]);
                                 }

                             
                         }
                    }
                    //magesh 13.6.18
                    else
                    {
                      
                        if (reqpk != "")
                        {
                            dep = d2.GetFunction("select MeetDeptCode from  RQ_Requisition where RequisitionPK='" + reqpk + "'");
                            if (dep.Contains(','))
                            {
                                string[] spl = dep.Split(',');



                                if (spl.Length > 0)
                                {
                                    for (int i = 0; i < spl.Length; i++)
                                    {
                                        if (depname == "")
                                            depname = d2.GetFunction("select Dept_Name  from Department where Dept_Code='" + spl[i] + "'");
                                        else
                                            depname = d2.GetFunction("select Dept_Name  from Department where Dept_Code='" + spl[i] + "'") + ',' + depname;


                                    }
                                }
                                else
                                    depname = d2.GetFunction("select Dept_Name  from Department where Dept_Code='" + dep + "'");

                            }

                            if (reqpk != "")
                                stf = d2.GetFunction("select MeetStaffAppNo from  RQ_Requisition where RequisitionPK='" + reqpk + "'");
                            if (stf.Contains(','))
                            {
                                string[] spl = stf.Split(',');
                                if (spl.Length > 0)
                                {
                                    for (int i = 0; i < spl.Length; i++)
                                    {
                                        string sql = "select * from staffmaster where appl_no='" + spl[i] + "'";

                                        steaff = d2.select_method_wo_parameter(sql, "text");

                                        if (steaff.Tables.Count > 0 && steaff.Tables[0].Rows.Count > 0)
                                        {

                                            stfname = Convert.ToString(steaff.Tables[0].Rows[0]["staff_name"]);
                                            code = Convert.ToString(steaff.Tables[0].Rows[0]["staff_code"]);
                                        }
                                        if (stafname == "")
                                        {
                                            stafname = stfname;
                                            code1 = code;

                                        }
                                        else
                                        {
                                            stafname = stafname + ',' + stfname;
                                            code1 = code1 + ',' + code;
                                        }
                                        string sql1 = "select * from staff_appl_master where appl_no='" + spl[i] + "'";
                                        steaff1 = d2.select_method_wo_parameter(sql1, "text");
                                        if (steaff1.Tables.Count > 0 && steaff1.Tables[0].Rows.Count > 0)
                                        {

                                            MOb = Convert.ToString(steaff1.Tables[0].Rows[0]["com_mobileno"]);
                                            MObper = Convert.ToString(steaff1.Tables[0].Rows[0]["per_mobileno"]);

                                        }
                                        if (Mobli == "")
                                        {
                                            Mobli = MOb;


                                        }
                                        else
                                        {
                                            Mobli = Mobli + ',' + MOb;

                                        }
                                        if (Mobliper == "")
                                        {
                                            Mobliper = MObper;
                                        }
                                        else
                                        {
                                            Mobliper = Mobliper + ',' + MObper;

                                        }

                                    }
                                }

                            }
                            else
                            {
                                string sql = "select * from staffmaster where appl_no='" + stf + "'";

                                steaff = d2.select_method_wo_parameter(sql, "text");

                                if (steaff.Tables.Count > 0 && steaff.Tables[0].Rows.Count > 0)
                                {

                                    stfname = Convert.ToString(steaff.Tables[0].Rows[0]["staff_name"]);
                                    code = Convert.ToString(steaff.Tables[0].Rows[0]["staff_code"]);
                                }
                                if (stafname == "")
                                {
                                    stafname = stfname;
                                    code1 = code;

                                }
                                string sql1 = "select * from staff_appl_master where appl_no='" + stf + "'";
                                steaff1 = d2.select_method_wo_parameter(sql1, "text");
                                if (steaff1.Tables.Count > 0 && steaff1.Tables[0].Rows.Count > 0)
                                {

                                    MOb = Convert.ToString(steaff1.Tables[0].Rows[0]["com_mobileno"]);
                                    Mobli = MOb;
                                    MObper = Convert.ToString(steaff1.Tables[0].Rows[0]["per_mobileno"]);
                                    Mobliper = MObper;
                                }
                            }
                        }
                    }
                }
                n++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n].Text = Convert.ToString(ds.Tables[0].Rows[row]["MobileNo"]);
                n++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n].Text = Convert.ToString(depname);
                n++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n].Text = Convert.ToString(stafname);
                n++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n].Text = Convert.ToString(code1);
                n++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n].Text = Convert.ToString(Mobli);
                n++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, n].Text = Convert.ToString(Mobliper);
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1,7].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1,10].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 12].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 12].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 13].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 13].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 13].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 14].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 14].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 14].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 15].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 15].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 15].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 16].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 16].HorizontalAlign = HorizontalAlign.Center;
                if (ddlmemtype.SelectedItem.Value == "4")
                {
                    if (Convert.ToString(ds.Tables[0].Rows[row]["GatepassEntrydate"]) != "")
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1,10].BackColor = Color.LightSeaGreen;
                    if (Convert.ToString(ds.Tables[0].Rows[row]["GatepassEntrytime"]) != "")
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 11].BackColor = Color.LightSeaGreen;
                    if (Convert.ToString(ds.Tables[0].Rows[row]["GatepassExitdate"]) != "")
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 12].BackColor = Color.PaleVioletRed;
                    if (Convert.ToString(ds.Tables[0].Rows[row]["GatepassExittime"]) != "")
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 13].BackColor = Color.PaleVioletRed;

                    if (Convert.ToString(ds.Tables[0].Rows[row]["gatetype"]).ToUpper() == "IN")
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 15].BackColor = Color.LightGreen;
                    else
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 15].BackColor = Color.LightSalmon;
                }
                else
                {
                    if (Convert.ToString(ds.Tables[0].Rows[row]["GatepassEntrydate"]) != "")
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 9].BackColor = Color.LightSeaGreen;
                    if (Convert.ToString(ds.Tables[0].Rows[row]["GatepassEntrytime"]) != "")
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 10].BackColor = Color.LightSeaGreen;
                    if (Convert.ToString(ds.Tables[0].Rows[row]["GatepassExitdate"]) != "")
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 11].BackColor = Color.PaleVioletRed;
                    if (Convert.ToString(ds.Tables[0].Rows[row]["GatepassExittime"]) != "")
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 12].BackColor = Color.PaleVioletRed;

                    if (Convert.ToString(ds.Tables[0].Rows[row]["gatetype"]).ToUpper() == "IN")
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 14].BackColor = Color.LightGreen;
                    else
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 14].BackColor = Color.LightSalmon;
                }
            }
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            spreadDet.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            spreadDet.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            spreadDet.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            spreadDet.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            spreadDet.Height = height;
            spreadDet.SaveChanges();
            spreadDet.Width = 950;
           spreadDet.Height = 500;
            divcolorder.Visible = true;
            divspread.Visible = true;
            print.Visible = true;
            lblvalidation1.Text = string.Empty;
            txtexcelname.Text = string.Empty;
            #endregion
        }
        catch 
        {
        }
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertDiv.Visible = false;
    }
    #region Print
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(spreadDet, reportname);
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
            if (cbdtfrom.Checked == true)
            {
                degreedetails = "Gate Pass Entry Exit Report " + ddlmemtype.SelectedItem.Text + " " + '@' + " Date   : " + txtfrmdt.Text + " To " + txttodt.Text + "";
            }
            else
            {
                degreedetails = "Gate Pass Entry Exit Report " + ddlmemtype.SelectedItem.Text;
            }
            pagename = "GatePassEntryExitReportOthers.aspx";
            Printcontrolhed.loadspreaddetails(spreadDet, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }
    #endregion
    protected void loadColumnOrder()
    {
        try
        {
            cblcolumnorder.Items.Clear();
            string headername, headername1 = ""; string headername3 = "";
            if (ddlmemtype.SelectedItem.Value == "2")
            {
                headername = "Staff Code";
                headername1 = "Staff Name";
            }
            else if (ddlmemtype.SelectedItem.Value == "3")
            {
                headername = "Relationship of Student";
                headername1 = "Meet";
            }
            else
            {
                headername = "Company Name";
                headername1 = "Visitor Name";
                headername3 = "Gatepass No";
                cblcolumnorder.Items.Add(new ListItem(headername, "1"));
                cblcolumnorder.Items.Add(new ListItem(headername1, "2"));
                cblcolumnorder.Items.Add(new ListItem(headername3, "3"));
                cblcolumnorder.Items.Add(new ListItem("Purpose", "4"));
                cblcolumnorder.Items.Add(new ListItem("Approved EntryDate", "5"));
                cblcolumnorder.Items.Add(new ListItem("Approved EntryTime", "6"));
                cblcolumnorder.Items.Add(new ListItem("Approved ExitDate", "8"));
                // cblcolumnorder.Items.Add(new ListItem("Approved ExitTime", "7"));
                cblcolumnorder.Items.Add(new ListItem("Entry Date", "9"));
                cblcolumnorder.Items.Add(new ListItem("Entry Time", "10"));
                cblcolumnorder.Items.Add(new ListItem("Exit Date", "11"));
                cblcolumnorder.Items.Add(new ListItem("Exit Time", "12"));
                cblcolumnorder.Items.Add(new ListItem("Entered Time", "13"));
                cblcolumnorder.Items.Add(new ListItem("Status", "14"));
                cblcolumnorder.Items.Add(new ListItem("Approved Status", "15"));
                cblcolumnorder.Items.Add(new ListItem("Visitor MobileNumber", "16"));
                cblcolumnorder.Items.Add(new ListItem("Meet To Department", "17"));
                cblcolumnorder.Items.Add(new ListItem("Meet To Staff", "18"));
                cblcolumnorder.Items.Add(new ListItem("Staff Code", "19"));
                cblcolumnorder.Items.Add(new ListItem("Staff Permanent MobileNumber", "20"));
                cblcolumnorder.Items.Add(new ListItem("Staff Communication MobileNumber", "21"));
                
            }
            if (ddlmemtype.SelectedItem.Value == "2" || ddlmemtype.SelectedItem.Value == "3")
            {
                cblcolumnorder.Items.Add(new ListItem(headername, "1"));
                cblcolumnorder.Items.Add(new ListItem(headername1, "2"));
                cblcolumnorder.Items.Add(new ListItem("Purpose", "3"));
                cblcolumnorder.Items.Add(new ListItem("Approved EntryDate", "4"));
                cblcolumnorder.Items.Add(new ListItem("Approved EntryTime", "5"));
                cblcolumnorder.Items.Add(new ListItem("Approved ExitDate", "6"));
                // cblcolumnorder.Items.Add(new ListItem("Approved ExitTime", "7"));
                cblcolumnorder.Items.Add(new ListItem("Entry Date", "8"));
                cblcolumnorder.Items.Add(new ListItem("Entry Time", "9"));
                cblcolumnorder.Items.Add(new ListItem("Exit Date", "10"));
                cblcolumnorder.Items.Add(new ListItem("Exit Time", "11"));
                cblcolumnorder.Items.Add(new ListItem("Entered Time", "12"));
                cblcolumnorder.Items.Add(new ListItem("Status", "13"));
                cblcolumnorder.Items.Add(new ListItem("Approved Status", "14"));
            }
            //for (int i = 0; i <= 2; i++)
            //{
            //    cblcolumnorder.Items[i].Selected = true;
            //    cblcolumnorder.Items[i].Enabled = false;
            //}
        }
        catch { }
    }
    public bool columncount()
    {
        bool colorder = false;
        try
        {
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                if (cblcolumnorder.Items[i].Selected == true)
                {
                    colorder = true;
                }
            }
        }
        catch { }
        return colorder;
    }
    public void loadcolumns()
    {
        try
        {
            string linkname = "Gate Pass Entry Exit Report Others";
            string columnvalue = "";
            int clsupdate = 0;
            DataSet dscol = new DataSet();
            string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "'  ";//and college_code in('" + collegecode + "')
            dscol.Clear();
            dscol = d2.select_method_wo_parameter(selcol, "Text");
            if (columncount() == true)
            {
                if (cblcolumnorder.Items.Count > 0)
                {
                    colord.Clear();
                    for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                    {
                        if (cblcolumnorder.Items[i].Selected == true)
                        {
                            colord.Add(Convert.ToString(cblcolumnorder.Items[i].Value));
                            if (columnvalue == "")
                                columnvalue = Convert.ToString(cblcolumnorder.Items[i].Value);
                            else
                                columnvalue = columnvalue + ',' + Convert.ToString(cblcolumnorder.Items[i].Value);
                        }
                    }
                }
            }
            else if (dscol.Tables.Count > 0)
            {
                if (dscol.Tables[0].Rows.Count > 0)
                {
                    colord.Clear();
                    for (int col = 0; col < dscol.Tables[0].Rows.Count; col++)
                    {
                        string value = Convert.ToString(dscol.Tables[0].Rows[col]["LinkValue"]);
                        string[] valuesplit = value.Split(',');
                        if (valuesplit.Length > 0)
                        {
                            for (int k = 0; k < valuesplit.Length; k++)
                            {
                                colord.Add(Convert.ToString(valuesplit[k]));
                                if (columnvalue == "")
                                    columnvalue = Convert.ToString(valuesplit[k]);
                                else
                                    columnvalue = columnvalue + ',' + Convert.ToString(valuesplit[k]);
                            }
                        }
                    }
                }
            }
            else
            {
                colord.Clear();
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = true;
                    colord.Add(Convert.ToString(cblcolumnorder.Items[i].Value));
                    if (columnvalue == "")
                        columnvalue = Convert.ToString(cblcolumnorder.Items[i].Value);
                    else
                        columnvalue = columnvalue + ',' + Convert.ToString(cblcolumnorder.Items[i].Value);
                }
            }
            if (columnvalue != "" && columnvalue != null)
            {
                string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "' and user_code='" + usercode + "' ) update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "' and user_code='" + usercode + "'  else insert into New_InsSettings (LinkName,LinkValue,user_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "')";//and college_code in('" + collegecode + "')
                clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
            }
            if (clsupdate == 1)
            {
                string sel = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "'";//and college_code in('" + collegecode + "')
                DataSet dscolor = new DataSet();
                dscolor.Clear();
                dscolor = d2.select_method_wo_parameter(sel, "Text");
                if (dscolor.Tables.Count > 0)
                {
                    int count = 0;
                    if (dscolor.Tables[0].Rows.Count > 0)
                    {
                        string value = Convert.ToString(dscolor.Tables[0].Rows[0]["LinkValue"]);
                        string[] value1 = value.Split(',');
                        if (value1.Length > 0)
                        {
                            for (int i = 0; i < value1.Length; i++)
                            {
                                string val = value1[i].ToString();
                                for (int k = 0; k < cblcolumnorder.Items.Count; k++)
                                {
                                    if (val == cblcolumnorder.Items[k].Value)
                                        cblcolumnorder.Items[k].Selected = true;
                                    count++;
                                    if (count == cblcolumnorder.Items.Count)
                                        cb_column.Checked = true;
                                    else
                                        cb_column.Checked = false;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch { }
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

    protected void spreadDet_CellClick(object sender, EventArgs e)
    {
        try
        {
            Cellclick = true;
        }
        catch
        {
        }
    }
    protected void spreadDet_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cellclick == true)
            {
                string activerow = spreadDet.ActiveSheetView.ActiveRow.ToString();
                string activecol = spreadDet.ActiveSheetView.ActiveColumn.ToString();
                if (activerow != "-1" && activerow != "")
                {
                    string gateno = Convert.ToString(spreadDet.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                    if (gateno != "")
                    {
                          
        StringBuilder SbHtml = new StringBuilder();


        string clgaddress = string.Empty;
        string pincode = string.Empty;
        string collName = string.Empty;
        string VisitorName = "";
        string CompanyName = "";
        string GatePassDate = "";
        string MobileNo = "";
       
        string intime = string.Empty;
        string outtime = string.Empty;
        string Purpose = string.Empty;
        string add1 = string.Empty;
        string city = string.Empty;
        string state = string.Empty;
        string dis = string.Empty;
        int pin = 0;
        string meet = string.Empty;
        string Deptm = string.Empty;
        string expectedtime = string.Empty;
        string strquery = "select *,district+' - '+pincode as districtpin,collname from collinfo where college_code='" + collegecode + "'";
        ds.Dispose();
        ds.Reset();
        ds = d2.select_method_wo_parameter(strquery, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            pincode = Convert.ToString(ds.Tables[0].Rows[0]["pincode"]).Trim();
            collName = Convert.ToString(ds.Tables[0].Rows[0]["collname"]).Trim();
            clgaddress = Convert.ToString(ds.Tables[0].Rows[0]["address3"]) + " , " + Convert.ToString(ds.Tables[0].Rows[0]["district"]) + ((pin != 0) ? (" - " + pin.ToString()) : " - " + pincode);
        }
        DataSet printds_new = new DataSet();
        string sql2 = "select VisitorName,CompanyName,GatePassDate,MobileNo,GatepassEntrytime,ExpectedTime,Purpose,Add1,City,District,state from GateEntryExit where gatepassno ='" +gateno+ "' and College_Code='" + collegecode + "'";
        printds_new = d2.select_method_wo_parameter(sql2, "Text");
        //printds_new.Reset();
        // printds_new = d2.select_method_wo_parameter(strquery, "Text");
        if (printds_new.Tables.Count > 0 && printds_new.Tables[0].Rows.Count > 0)
        {
            CompanyName = Convert.ToString(printds_new.Tables[0].Rows[0]["CompanyName"]).Trim();
            
            VisitorName = Convert.ToString(printds_new.Tables[0].Rows[0]["VisitorName"]).Trim();
            add1 = Convert.ToString(printds_new.Tables[0].Rows[0]["Add1"]).Trim();
            city = Convert.ToString(printds_new.Tables[0].Rows[0]["City"]).Trim();
            dis = Convert.ToString(printds_new.Tables[0].Rows[0]["District"]).Trim();
            state = Convert.ToString(printds_new.Tables[0].Rows[0]["state"]).Trim();
            meet = Convert.ToString(ViewState["To Meet"]);
            Deptm =Convert.ToString(ViewState["To Meet dept"]);
          
            if (meet != "")
            {
                string[] spli = meet.Split('-');
                if (spli.Length > 0)
                {
                    meet = spli[0];
                    //if (spli.Length >= 3)
                    //    if (spli[2] != "")
                    //        meet = spli[0] + '-' + spli[2];
                }
            }
            if (Deptm != "")
            {
                meet = meet + "-" + Deptm;
            }
            MobileNo = Convert.ToString(printds_new.Tables[0].Rows[0]["MobileNo"]).Trim();
            intime = Convert.ToString(printds_new.Tables[0].Rows[0]["GatepassEntrytime"]).Trim();
            outtime = Convert.ToString(printds_new.Tables[0].Rows[0]["ExpectedTime"]).Trim();
            //expectedtime = ddl_hrs.SelectedItem.Text + ":" + ddl_mins.SelectedItem.Text + "" + ddl_ampm.SelectedItem.Text;
            Purpose = Convert.ToString(printds_new.Tables[0].Rows[0]["Purpose"]).Trim();

        }

        #region I Page
        SbHtml.Append("<html>");
        SbHtml.Append("<body>");
        SbHtml.Append("<div style='height:715px; width: 655px; border:1px solid black; margin:0px; margin-left: 105px;page-break-after: always;'>");

        #region Header

        SbHtml.Append("<div style='width: 910px; border: 0px solid black; margin:0px; margin-left: 5px;'>");
        SbHtml.Append("<font face='IDAutomationHC39M'size='4'>");
        SbHtml.Append("<div style='width: 945px; height: 5px; border: 0px solid black; margin:0px; margin-left: 370px;'>");
        string barcode = "*" + gateno + "*";
        SbHtml.Append("<span style='font-weight:bold;'width: 7px; height:5px; border: 0px solid Red'  >" + barcode + "  </span>");
        SbHtml.Append("</div>");
        SbHtml.Append("</font>");
        SbHtml.Append("<br>");
        SbHtml.Append("<br>");
        SbHtml.Append("<br>");
        SbHtml.Append("<table cellspacing='0' cellpadding='5' border='0px' style='width: 645px; height:30px; font-weight: bold;'>");
        SbHtml.Append("<tr style='text-align:right;'>");
        SbHtml.Append("<td>");

        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr>");
        SbHtml.Append("<td rowspan='3'><img src='" + "../college/Left_Logo.jpeg" + "' style='height:80px; width:80px;'/></td>");
        SbHtml.Append("<td style='text-align:center;'>");
        
        SbHtml.Append("<span> " + collName + "</span>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td rowspan='3'><img src='" + "../college/right_Logo.jpeg" + "' style='height:80px; width:80px;'/></td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr style='text-align:center;'>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span> " + clgaddress + "</span>");
        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr style='text-align:center;'>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span> VISITOR'S SLIP </span>");
        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td colspan='5' style='text-align:right;'>");
        
        
        SbHtml.Append("<span> DATE: " + DateTime.Now.ToString("dd/MM/yyyy") + " </span>");
        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr><td colspan='3'><hr style='height:1px; width:600px;'></td></tr>");
        SbHtml.Append("</table>");
        SbHtml.Append("</div>");

        #endregion

        #region Student Details
       
        SbHtml.Append("<br>");
        SbHtml.Append("<div style='width: 645px; border: 0px solid black; margin:0px; margin-left: 5px;'>");
        SbHtml.Append("<table cellspacing='0' cellpadding='5' border='1px' style='width: 645px; font-weight: bold;'>");
        SbHtml.Append("<tr>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>Gatepass No</span>");


        SbHtml.Append("</td>");
        SbHtml.Append("<td width='400px'>");
        SbHtml.Append("<span>" + gateno + "</span>");

        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>Visitor Name & Address</span>");


        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>" + VisitorName + "<br> " + add1 + " <br>  " + city + "<br> " + dis + " <br>  " + state + " </span>");

        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>To Meet</span>");


        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>" + meet + "</span>");

        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>Time In</span>");


        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>" + intime + "</span>");

        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        //SbHtml.Append("<tr>");
        //SbHtml.Append("<td>");
        //SbHtml.Append("<span>Time Out</span>");


        //SbHtml.Append("</td>");
        //SbHtml.Append("<td>");
        //SbHtml.Append("<span>" + expectedtime + "</span>");

        //SbHtml.Append("</td>");
        //SbHtml.Append("</tr>");
        SbHtml.Append("<tr>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>Purpose</span>");


        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>" + Purpose + "</span>");

        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>Mobile No</span>");


        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>" + MobileNo + "</span>");

        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        //SbHtml.Append("<tr>");
        //SbHtml.Append("<td>");

        //SbHtml.Append("<span>In this Moderation used to any students are need 1 Mark to get Minimum total. It apply and reach minimum total for that student</span>");
        //SbHtml.Append("</td>");
        //SbHtml.Append("</tr>");
        SbHtml.Append("</table>");
        SbHtml.Append("</div>");
        #endregion

        #region FooterDetails

        SbHtml.Append("<br>");
        SbHtml.Append("<br>");
        SbHtml.Append("<br>");
        SbHtml.Append("<br>");
        SbHtml.Append("<br>");
        SbHtml.Append("<br>");
        SbHtml.Append("<div style='width: 645px; border: 0px solid black; margin:0px; margin-left: 5px;'>");
        SbHtml.Append("<table border='0px' cellspacing='0' cellpadding='5' style='width: 645px;'>");
        SbHtml.Append("<tr style='text-align:left;'>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr style='text-align:left;'>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>Security</span>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>Visitor</span>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>Concerned Person</span>");
        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("</table>");
        SbHtml.Append("</div>");
        SbHtml.Append("</div>");
        SbHtml.Append("</body>");
        SbHtml.Append("</html>");

        contentDiv.InnerHtml = SbHtml.ToString();
        contentDiv.Visible = true;
        ScriptManager.RegisterStartupScript(this, GetType(), "btn_erroralert", "PrintDiv();", true);

        #endregion

        #endregion
    }
                    }
                   
                }
            }
        catch
        {

        }
        }
    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
           
        {
            StringBuilder SbHtml = new StringBuilder();
            for (int row = 0; row < spreadDet.Sheets[0].RowCount; row++)
            {
                int selected = 0;
                spreadDet.SaveChanges();
                string gateno1 = Convert.ToString(spreadDet.Sheets[0].Cells[row, 4].Text);
                int.TryParse(Convert.ToString(spreadDet.Sheets[0].Cells[row, 1].Value), out selected);
                if (selected == 1)
                {
                    string gateno = Convert.ToString(spreadDet.Sheets[0].Cells[row, 4].Text);
                    if (gateno != "")
                    {

                       


                        string clgaddress = string.Empty;
                        string pincode = string.Empty;
                        string collName = string.Empty;
                        string VisitorName = "";
                        string CompanyName = "";
                        string GatePassDate = "";
                        string MobileNo = "";

                        string intime = string.Empty;
                        string outtime = string.Empty;
                        string Purpose = string.Empty;
                        string add1 = string.Empty;
                        string city = string.Empty;
                        string state = string.Empty;
                        string dis = string.Empty;
                        int pin = 0;
                        string meet = string.Empty;
                        string Deptm = string.Empty;
                        string expectedtime = string.Empty;
                        if (cbl_col.Items.Count > 0)
                            collegecode = Convert.ToString(getCblSelectedValue(cbl_col));
                       
                        DataSet printds_new = new DataSet();
                        string sql2 = "select VisitorName,CompanyName,GatePassDate,MobileNo,GatepassEntrytime,ExpectedTime,Purpose,Add1,City,District,state,College_Code from GateEntryExit where gatepassno ='" + gateno + "' and College_Code in('" + collegecode + "')";
                        printds_new = d2.select_method_wo_parameter(sql2, "Text");

                        string strquery = "select *,district+' - '+pincode as districtpin,collname from collinfo where college_code='" + Convert.ToString(printds_new.Tables[0].Rows[0]["College_Code"]).Trim()+"'";
                        ds.Dispose();
                        ds.Reset();
                        ds = d2.select_method_wo_parameter(strquery, "Text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            pincode = Convert.ToString(ds.Tables[0].Rows[0]["pincode"]).Trim();
                            collName = Convert.ToString(ds.Tables[0].Rows[0]["collname"]).Trim();
                            clgaddress = Convert.ToString(ds.Tables[0].Rows[0]["address3"]) + " , " + Convert.ToString(ds.Tables[0].Rows[0]["district"]) + ((pin != 0) ? (" - " + pin.ToString()) : " - " + pincode);
                        }
                        //printds_new.Reset();
                        // printds_new = d2.select_method_wo_parameter(strquery, "Text");
                        if (printds_new.Tables.Count > 0 && printds_new.Tables[0].Rows.Count > 0)
                        {
                            CompanyName = Convert.ToString(printds_new.Tables[0].Rows[0]["CompanyName"]).Trim();

                            VisitorName = Convert.ToString(printds_new.Tables[0].Rows[0]["VisitorName"]).Trim();
                            add1 = Convert.ToString(printds_new.Tables[0].Rows[0]["Add1"]).Trim();
                            city = Convert.ToString(printds_new.Tables[0].Rows[0]["City"]).Trim();
                            dis = Convert.ToString(printds_new.Tables[0].Rows[0]["District"]).Trim();
                            state = Convert.ToString(printds_new.Tables[0].Rows[0]["state"]).Trim();
                            meet = Convert.ToString(ViewState["To Meet"]);
                            Deptm = Convert.ToString(ViewState["To Meet dept"]);

                            if (meet != "")
                            {
                                string[] spli = meet.Split('-');
                                if (spli.Length > 0)
                                {
                                    meet = spli[0];
                                    //if (spli.Length >= 3)
                                    //    if (spli[2] != "")
                                    //        meet = spli[0] + '-' + spli[2];
                                }
                            }
                            if (Deptm != "")
                            {
                                meet = meet + "-" + Deptm;
                            }
                            MobileNo = Convert.ToString(printds_new.Tables[0].Rows[0]["MobileNo"]).Trim();
                            intime = Convert.ToString(printds_new.Tables[0].Rows[0]["GatepassEntrytime"]).Trim();
                            outtime = Convert.ToString(printds_new.Tables[0].Rows[0]["ExpectedTime"]).Trim();
                            //expectedtime = ddl_hrs.SelectedItem.Text + ":" + ddl_mins.SelectedItem.Text + "" + ddl_ampm.SelectedItem.Text;
                            Purpose = Convert.ToString(printds_new.Tables[0].Rows[0]["Purpose"]).Trim();

                        }

                        #region I Page
                        SbHtml.Append("<html>");
                        SbHtml.Append("<body>");
                        SbHtml.Append("<div style='height:715px; width: 655px; border:1px solid black; margin:0px; margin-left: 105px;page-break-after: always;'>");

                        #region Header

                        SbHtml.Append("<div style='width: 910px; border: 0px solid black; margin:0px; margin-left: 5px;'>");
                        SbHtml.Append("<font face='IDAutomationHC39M'size='4'>");
                        SbHtml.Append("<div style='width: 945px; height: 5px; border: 0px solid black; margin:0px; margin-left: 370px;'>");
                        string barcode = "*" + gateno + "*";
                        SbHtml.Append("<span style='font-weight:bold;'width: 7px; height:5px; border: 0px solid Red'  >" + barcode + "  </span>");
                        SbHtml.Append("</div>");
                        SbHtml.Append("</font>");
                        SbHtml.Append("<br>");
                        SbHtml.Append("<br>");
                        SbHtml.Append("<br>");
                        SbHtml.Append("<table cellspacing='0' cellpadding='5' border='0px' style='width: 645px; height:30px; font-weight: bold;'>");
                        SbHtml.Append("<tr style='text-align:right;'>");
                        SbHtml.Append("<td>");

                        SbHtml.Append("</td>");
                        SbHtml.Append("</tr>");
                        SbHtml.Append("<tr>");
                        SbHtml.Append("<td rowspan='3'><img src='" + "../college/Left_Logo.jpeg" + "' style='height:80px; width:80px;'/></td>");
                        SbHtml.Append("<td style='text-align:center;'>");

                        SbHtml.Append("<span> " + collName + "</span>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td rowspan='3'><img src='" + "../college/right_Logo.jpeg" + "' style='height:80px; width:80px;'/></td>");
                        SbHtml.Append("</tr>");
                        SbHtml.Append("<tr style='text-align:center;'>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("<span> " + clgaddress + "</span>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("</tr>");
                        SbHtml.Append("<tr style='text-align:center;'>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("<span> VISITOR'S SLIP </span>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("</tr>");
                        SbHtml.Append("<tr>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td colspan='5' style='text-align:right;'>");


                        SbHtml.Append("<span> DATE: " + DateTime.Now.ToString("dd/MM/yyyy") + " </span>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("</tr>");
                        SbHtml.Append("<tr><td colspan='3'><hr style='height:1px; width:600px;'></td></tr>");
                        SbHtml.Append("</table>");
                        SbHtml.Append("</div>");

                        #endregion

                        #region Student Details

                        SbHtml.Append("<br>");
                        SbHtml.Append("<div style='width: 645px; border: 0px solid black; margin:0px; margin-left: 5px;'>");
                        SbHtml.Append("<table cellspacing='0' cellpadding='5' border='1px' style='width: 645px; font-weight: bold;'>");
                        SbHtml.Append("<tr>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("<span>Gatepass No</span>");


                        SbHtml.Append("</td>");
                        SbHtml.Append("<td width='400px'>");
                        SbHtml.Append("<span>" + gateno + "</span>");

                        SbHtml.Append("</td>");
                        SbHtml.Append("</tr>");
                        SbHtml.Append("<tr>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("<span>Visitor Name & Address</span>");


                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("<span>" + VisitorName + "<br> " + add1 + " <br>  " + city + "<br> " + dis + " <br>  " + state + " </span>");

                        SbHtml.Append("</td>");
                        SbHtml.Append("</tr>");
                        SbHtml.Append("<tr>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("<span>To Meet</span>");


                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("<span>" + meet + "</span>");

                        SbHtml.Append("</td>");
                        SbHtml.Append("</tr>");
                        SbHtml.Append("<tr>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("<span>Time In</span>");


                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("<span>" + intime + "</span>");

                        SbHtml.Append("</td>");
                        SbHtml.Append("</tr>");
                        //SbHtml.Append("<tr>");
                        //SbHtml.Append("<td>");
                        //SbHtml.Append("<span>Time Out</span>");


                        //SbHtml.Append("</td>");
                        //SbHtml.Append("<td>");
                        //SbHtml.Append("<span>" + expectedtime + "</span>");

                        //SbHtml.Append("</td>");
                        //SbHtml.Append("</tr>");
                        SbHtml.Append("<tr>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("<span>Purpose</span>");


                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("<span>" + Purpose + "</span>");

                        SbHtml.Append("</td>");
                        SbHtml.Append("</tr>");
                        SbHtml.Append("<tr>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("<span>Mobile No</span>");


                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("<span>" + MobileNo + "</span>");

                        SbHtml.Append("</td>");
                        SbHtml.Append("</tr>");
                        //SbHtml.Append("<tr>");
                        //SbHtml.Append("<td>");

                        //SbHtml.Append("<span>In this Moderation used to any students are need 1 Mark to get Minimum total. It apply and reach minimum total for that student</span>");
                        //SbHtml.Append("</td>");
                        //SbHtml.Append("</tr>");
                        SbHtml.Append("</table>");
                        SbHtml.Append("</div>");
                        #endregion

                        #region FooterDetails

                        SbHtml.Append("<br>");
                        SbHtml.Append("<br>");
                        SbHtml.Append("<br>");
                        SbHtml.Append("<br>");
                        SbHtml.Append("<br>");
                        SbHtml.Append("<br>");
                        SbHtml.Append("<div style='width: 645px; border: 0px solid black; margin:0px; margin-left: 5px;'>");
                        SbHtml.Append("<table border='0px' cellspacing='0' cellpadding='5' style='width: 645px;'>");
                        SbHtml.Append("<tr style='text-align:left;'>");
                        SbHtml.Append("</tr>");
                        SbHtml.Append("<tr style='text-align:left;'>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("<span>Security</span>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("<span>Visitor</span>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("<span>Concerned Person</span>");
                        SbHtml.Append("</td>");
                        SbHtml.Append("</tr>");
                        SbHtml.Append("</table>");
                        SbHtml.Append("</div>");
                        SbHtml.Append("</div>");
                        SbHtml.Append("</body>");
                        SbHtml.Append("</html>");

                        contentDiv.InnerHtml = SbHtml.ToString();
                        contentDiv.Visible = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "btn_erroralert", "PrintDiv();", true);

                        #endregion

                        #endregion
                    }
                }

            }
        }
        catch
        {

        }
    }

    public void destination()
    {
        try
        {
             clgCode = Convert.ToString(getCblSelectedValue(cbl_col));
             string query = "select desig_code,desig_name from desig_master where collegeCode in('" + clgCode + "') ";

             ds = d2.select_method_wo_parameter(query, "Text");
             if (ds.Tables[0].Rows.Count > 0)
             {
                 cbldes.DataSource = ds;
                 cbldes.DataTextField = "desig_name";
                 cbldes.DataValueField = "desig_code";
                 cbldes.DataBind();
                 if (cbldes.Items.Count > 0)
                 {
                     for (int row = 0; row < cbldes.Items.Count; row++)
                     {
                         cbldes.Items[row].Selected = true;
                     }
                     chkdes.Checked = true;
                     TextBox1.Text = "Destination(" + cbldes.Items.Count + ")";
                 }
                 else
                 {
                     chkdes.Checked = false;
                     TextBox1.Text = "--Select--";
                 }
                 //destin = Convert.ToString(getCblSelectedValue(cbldes));
                 bind_stafType2();
             }
        }
        catch
        {
        }
    }

      public void dept()
    {
        try
        {
             clgCode = Convert.ToString(getCblSelectedValue(cbl_col));
             string query = "select distinct dept_code,dept_name from   hrdept_master where college_code in('" + clgCode + "') order by dept_name";
           
             ds = d2.select_method_wo_parameter(query, "Text");
             if (ds.Tables[0].Rows.Count > 0)
             {
                 Cbldept.DataSource = ds;
                 Cbldept.DataTextField = "dept_name";
                 Cbldept.DataValueField = "dept_code";
                
                 Cbldept.DataBind();
                 if (Cbldept.Items.Count > 0)
                 {
                     for (int row = 0; row < Cbldept.Items.Count; row++)
                     {
                         Cbldept.Items[row].Selected = true;
                     }
                     Chkdept.Checked = true;
                     TextBox4.Text = "Department(" + Cbldept.Items.Count + ")";
                 }
                 else
                 {
                     Chkdept.Checked = false;
                     TextBox4.Text = "--Select--";
                 }
            Cbldept.Items.Add( "Others");
                 //deptmar = Convert.ToString(getCblSelectedValue(Cbldept));
                 bind_stafType2();
             }
        }
        catch
        {
        }
    }
      public void bind_stafType1()
      {
          try
          {
              clgCode = Convert.ToString(getCblSelectedValue(cbl_col));
              string query = "SELECT DISTINCT StfType FROM StaffTrans T,HrDept_Master D WHERE T.Dept_Code = D.Dept_Code AND T.Latestrec = 1 and d.college_code in ('" + clgCode + "')";
              ds = d2.select_method_wo_parameter(query, "Text");
              {
                  cblstafftype.Items.Clear();
                  cblstafftype.DataSource = ds;
                  cblstafftype.DataTextField = "StfType";
                  cblstafftype.DataValueField = "StfType";
                  cblstafftype.DataBind();
                  Chkstafftype.Checked = true;
                  if (cblstafftype.Items.Count > 0)
                  {
                      for (int i = 0; i < cblstafftype.Items.Count; i++)
                      {
                          cblstafftype.Items[i].Selected = true;
                      }
                      TextBox3.Text = "Staff Type(" + cblstafftype.Items.Count + ")";
                  }
                  //staftype = Convert.ToString(getCblSelectedValue(cblstafftype));
                  bind_stafType2();
              }
          }
          catch (Exception ex)
          {
          }
      }
      
      public void bind_stafType2()
      {
          try
          {
              deptmar = Convert.ToString(getCblSelectedValue(Cbldept));
           
              for (int i = 0; i < Cbldept.Items.Count; i++)
              {
                  if (Cbldept.Items[i].Selected)
                  {
                      if (Cbldept.Items[i].Text.ToUpper() != "OTHERS")
                      {
                          if (deptmar == "")
                          {
                              deptmar = "" + Cbldept.Items[i].Value.ToString();
                          }
                          else
                          {
                              deptmar += "','" + Cbldept.Items[i].Value.ToString() + "";
                          }
                          othss = "NO";
                      }
                      else
                          othss = "Yes";
                  }
              }

              clgCode = Convert.ToString(getCblSelectedValue(cbl_col));
              staftype = Convert.ToString(getCblSelectedValue(cblstafftype));
              destin = Convert.ToString(getCblSelectedValue(cbldes));
               string staf = " select distinct s.staff_code,a.appl_id ,s.staff_name,h.dept_name,h.dept_code,d.desig_name from staff_appl_master a ,staffmaster s,hrdept_master h,desig_master d,stafftrans st where a.appl_no =s.appl_no and s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode and s.college_code = d.collegecode  and  s.college_code in('" + clgCode
 + "') and resign = 0 and settled = 0 and latestrec=1";
               if (destin != "")
                   staf = staf + "  and d.desig_code in('" + destin + "')";
               if (deptmar!="")
                   staf = staf + "  and h.dept_code in ('" + deptmar + "')";
               if (staftype!="")
                   staf = staf + "  and stftype in('" + staftype + "')";

               staf = staf + "  order by h.dept_name,s.staff_code";
              
               ds = d2.select_method_wo_parameter(staf, "Text");
              {
                  cblstaffname.Items.Clear();
                  cblstaffname.DataSource = ds;
                  cblstaffname.DataTextField = "staff_name";
                  cblstaffname.DataValueField = "staff_code";
                  cblstaffname.DataBind();
                  Chkstafftype.Checked = true;
                  if (cblstaffname.Items.Count > 0)
                  {
                      for (int i = 0; i < cblstaffname.Items.Count; i++)
                      {
                          cblstaffname.Items[i].Selected = true;
                      }
                      TextBox2.Text = "Staff Name(" + cblstaffname.Items.Count + ")";
                  }
                
              }
              string staf1 = string.Empty;
              if (othss == "Yes")
              {
                  staf1 = "select OtherName as staff_name ,Relationship as staff_code from GateEntryExitDet";
              }
              ds = d2.select_method_wo_parameter(staf1, "Text");
              if (ds.Tables.Count > 0)
              {
                  for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                  {
                      if (Convert.ToString(ds.Tables[0].Rows[j]["staff_name"])!="")
                      cblstaffname.Items.Add(Convert.ToString(ds.Tables[0].Rows[j]["staff_name"]));
                     
                  }
              }
              TextBox2.Text = "Staff Name(" + cblstaffname.Items.Count + ")";
          }
          catch
          {
          }
      }


      protected void chkdes_CheckedChanged(object sender, EventArgs e)
      {
          CallCheckboxChange(chkdes, cbldes, TextBox1, "Destination", "--Select--");
          bind_stafType2();
      }
      protected void chkdes1_CheckedChanged(object sender, EventArgs e)
      {
          CallCheckboxChange(Chkdept, Cbldept, TextBox4, "Department", "--Select--");
          bind_stafType2();
      }
      protected void chkdes2_CheckedChanged(object sender, EventArgs e)
      {
          CallCheckboxChange(Chkstafftype, cblstafftype, TextBox3, "Staff Type", "--Select--");
          bind_stafType2();
      }

      protected void cbldes_SelectedIndexChanged(object sender, EventArgs e)
      {
          CallCheckboxListChange(chkdes, cbldes, TextBox1, "Destination", "--Select--");
          bind_stafType2();
      }
      protected void cbldes1_SelectedIndexChanged(object sender, EventArgs e)
      {
          CallCheckboxListChange(Chkdept, Cbldept, TextBox4, "Department", "--Select--");
          bind_stafType2();
      }
      protected void cbldes2_SelectedIndexChanged(object sender, EventArgs e)
      {
          CallCheckboxListChange(Chkstafftype, cblstafftype, TextBox3, "Staff Type", "--Select--");
          bind_stafType2();
      }
      protected void cbldes3_SelectedIndexChanged(object sender, EventArgs e)
      {
          CallCheckboxListChange(chkstaffname, cblstaffname, TextBox2, "Staff Name", "--Select--");
         
      }
      protected void chkdes3_CheckedChanged(object sender, EventArgs e)
      {
          CallCheckboxChange(chkstaffname, cblstaffname, TextBox2, "Staff Name", "--Select--");
      }
}

