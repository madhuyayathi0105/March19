using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI.WebControls;
using Gios.Pdf;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Web.UI;
using System.Web.Services;
using System.Text.RegularExpressions;



public partial class fullandfinalsettlement : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    static string collegecode = string.Empty;
    string collegecode1 = string.Empty;
    string usercode = string.Empty;
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    int colheder;
    int colgross;
    int col;
    string sql;
    string sql1 = "";
    string strdept = "";
    string strcategory = "";
    Hashtable hatpre = new Hashtable();
    Hashtable splallow = new Hashtable();
    Hashtable hat = new Hashtable();
    Hashtable ColumnWidth = new Hashtable();
    Hashtable ColumnAdjWid = new Hashtable();
    static Hashtable getcol = new Hashtable();
    DataSet dssmssalary = new DataSet();
    SortedDictionary<string, string> deduct = new SortedDictionary<string, string>();

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
            //    if (!Request.FilePath.Contains("HRM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["HR"].Trim();
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

            if (!Request.FilePath.Contains("HRMenuIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/HRMOD/HRMenuIndex.aspx");
                    return;
                }
            }
            //****************************************************//

            usercode = Convert.ToString(Session["usercode"]);
            if (!IsPostBack)
            {
                bindcollege();
                if (ddlcollege.Items.Count > 0)
                {
                    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                    collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                    binddept();
                    binddesignation();
                    loadstafftype();
                    loadcategory();
                    bindyear();
                }

            }
            if (ddlcollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
        }
        catch (Exception ex) { }
    }

    public void bindcollege()
    {
        try
        {
            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch (Exception e) { }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_code like  '" + prefixText + "%' and college_code='" + collegecode + "' order by staff_code";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_name  from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_name like  '" + prefixText + "%' and college_code='" + collegecode + "' order by staff_name";
        name = ws.Getname(query);
        return name;
    }
    protected void ddlcollege_Change(object sender, EventArgs e)
    {
        collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
        binddept();
        binddesignation();
        loadstafftype();
        loadcategory();
        bindyear();

        ddl_mon.SelectedIndex = 0;



    }
    protected void ddl_mon_Change(object sender, EventArgs e)
    {
        bindyear();
    }
    public void bindyear()
    {
        try
        {
            ddl_year.Items.Clear();
            ds.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter("select distinct year(To_Date) as year from HrPayMonths where College_Code ='" + collegecode1 + "' order by year asc", "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_year.DataSource = ds;
                    ddl_year.DataTextField = "year";
                    ddl_year.DataValueField = "year";
                    ddl_year.DataBind();
                }
            }
        }
        catch { }
    }
    protected void txtstaff_txtchanged(object sender, EventArgs e)
    {
        txtstaffname.Text = "";
    }
    protected void txtname_txtchanged(object sender, EventArgs e)
    {
        txtstaffcode.Text = "";
    }
    protected void cb_dept_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cb_dept, cbl_dept, txt_dept, "Department");
    }
    protected void cbl_dept_selectedchanged(object sender, EventArgs e)
    {
        chklstchange(cb_dept, cbl_dept, txt_dept, "Department");
    }
    protected void cb_desig_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cb_desig, cbl_desig, txt_desig, "Designation");
    }
    protected void cbl_desig_selectedchanged(object sender, EventArgs e)
    {
        chklstchange(cb_desig, cbl_desig, txt_desig, "Designation");
    }
    protected void cb_staffcat_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cb_staffcat, cbl_staffcat, txt_staffcat, "Category");
    }
    protected void cbl_staffcat_selectedchanged(object sender, EventArgs e)
    {
        chklstchange(cb_staffcat, cbl_staffcat, txt_staffcat, "Category");
    }
    protected void cb_stafftyp_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cb_stafftyp, cbl_stafftyp, txt_stafftyp, "StaffType");
    }
    protected void cbl_stafftyp_selectedchanged(object sender, EventArgs e)
    {
        chklstchange(cb_stafftyp, cbl_stafftyp, txt_stafftyp, "StaffType");
    }
    private void chkchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            if (chkchange.Checked == true)
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = true;
                }
                txtchange.Text = label + "(" + Convert.ToString(chklstchange.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = false;
                }
                txtchange.Text = "--Select--";
            }
        }
        catch { }
    }
    private void chklstchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            txtchange.Text = "--Select--";
            chkchange.Checked = false;
            int count = 0;
            for (int i = 0; i < chklstchange.Items.Count; i++)
            {
                if (chklstchange.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txtchange.Text = label + "(" + count + ")";
                if (count == chklstchange.Items.Count)
                {
                    chkchange.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void binddept()
    {
        try
        {
            cbl_dept.Items.Clear();
            ds.Clear();
            string group_user = "";
            string cmd = "";
            string singleuser = Session["single_user"].ToString();
            if (singleuser == "True")
            {
                cmd = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + collegecode1 + "') order by dept_name";
            }
            else
            {
                group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                cmd = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + collegecode1 + "') order by dept_name";
            }
            ds = d2.select_method_wo_parameter(cmd, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds.Tables[0];
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "dept_code";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            cbl_dept.Items[i].Selected = true;
                        }
                        txt_dept.Text = "Department(" + cbl_dept.Items.Count + ")";
                        cb_dept.Checked = true;
                    }
                }
                else
                {
                    txt_dept.Text = "--Select--";
                    cb_dept.Checked = false;
                }
            }
        }
        catch { }
    }
    protected void binddesignation()
    {
        ds.Clear();
        cbl_desig.Items.Clear();
        string statequery = "select desig_code,desig_name from desig_master where collegeCode='" + collegecode1 + "' order by desig_name";
        ds = d2.select_method_wo_parameter(statequery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_desig.DataSource = ds;
            cbl_desig.DataTextField = "desig_name";
            cbl_desig.DataValueField = "desig_code";
            cbl_desig.DataBind();
            cbl_desig.Visible = true;
            if (cbl_desig.Items.Count > 0)
            {
                for (int i = 0; i < cbl_desig.Items.Count; i++)
                {
                    cbl_desig.Items[i].Selected = true;
                }
                txt_desig.Text = "Designation(" + cbl_desig.Items.Count + ")";
                cb_desig.Checked = true;
            }
        }
        else
        {
            txt_desig.Text = "--Select--";
            cb_desig.Checked = false;
        }
    }
    protected void loadstafftype()
    {
        try
        {
            ds.Clear();
            cbl_stafftyp.Items.Clear();
            string item = "select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code = '" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_stafftyp.DataSource = ds;
                cbl_stafftyp.DataTextField = "stftype";
                cbl_stafftyp.DataBind();
                if (cbl_stafftyp.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_stafftyp.Items.Count; i++)
                    {
                        cbl_stafftyp.Items[i].Selected = true;
                    }
                    txt_stafftyp.Text = "StaffType (" + cbl_stafftyp.Items.Count + ")";
                    cb_stafftyp.Checked = true;
                }
            }
            else
            {
                txt_stafftyp.Text = "--Select--";
                cb_stafftyp.Checked = false;
            }
        }
        catch { }
    }
    protected void loadcategory()
    {
        ds.Clear();
        cbl_staffcat.Items.Clear();
        string statequery = "select category_code,category_Name from staffcategorizer where college_code = '" + collegecode1 + "' ";
        ds = d2.select_method_wo_parameter(statequery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_staffcat.DataSource = ds;
            cbl_staffcat.DataTextField = "category_Name";
            cbl_staffcat.DataValueField = "category_code";
            cbl_staffcat.DataBind();
            cbl_staffcat.Visible = true;
            if (cbl_staffcat.Items.Count > 0)
            {
                for (int i = 0; i < cbl_staffcat.Items.Count; i++)
                {
                    cbl_staffcat.Items[i].Selected = true;
                }
                txt_staffcat.Text = "Category(" + cbl_staffcat.Items.Count + ")";
                cb_staffcat.Checked = true;
            }
        }
        else
        {
            txt_staffcat.Text = "--Select--";
            cb_staffcat.Checked = false;
        }
    }
    protected void btngo_click(object sender, EventArgs e)
    {
        try
        {
            sql1 = "select * from incentives_master where college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
            sql1 = sql1 + " ;select distinct CONVERT(VARCHAR(10),from_date,103) as from_date,convert(VARCHAR(10),to_date,103) as to_date from hrpaymonths where paymonthnum='" + ddl_mon.SelectedItem.Value.ToString() + "'and PayYear='" + ddl_year.SelectedItem.Text.ToString() + "' and college_Code=" + collegecode1 + "";
            sql1 = sql1 + " ;select isnull(collname, ' ') as collname,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode from collinfo where college_code='" + collegecode1 + "'";
            sql1 = sql1 + " ;select shortname from leave_category where status<>'pres' and college_code='" + collegecode1 + "'";
            sql1 = sql1 + " ;select shortname from leave_category where status<>'comp' and college_code='" + collegecode1 + "'";
            string getvaluedigits = d2.GetFunction("select value from Master_Settings where settings='Bank Foramte Fixed Digits'");
            DataSet dsset = d2.select_method_wo_parameter(sql1, "Text");

            string fromdate = "";
            string todate = "";
            string[] split = new string[2];
            string[] split1 = new string[2];
            int dedrowcount = 0;
            int allrowcount = 0;
            double convenexp = 0;
            double lunchexp = 0;
            double newconvenexp = 0;
            double newlunchexp = 0;
            string date1 = "";
            string date2 = "";
            string datefrom = "";
            string dateto = "";

            if (dsset.Tables[1].Rows.Count > 0)
            {
                fromdate = dsset.Tables[1].Rows[0]["from_date"].ToString();
                todate = dsset.Tables[1].Rows[0]["to_date"].ToString();
            }
            date1 = fromdate;
            date2 = todate;
            if (date1.Trim() == "" || date2.Trim() == "")
            {

                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"No Records Found\");", true);

            }
            if (date1.Trim() != "")
            {
                split = date1.Split(new Char[] { '/' });
                datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
            }
            if (date2.Trim() != "")
            {
                split1 = date2.Split(new Char[] { '/' });
                dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
            }
            string coll_name = "";
            string coll_address1 = "";
            string coll_address2 = "";
            string coll_address3 = "";
            string pin_code = "";
            if (dsset.Tables[2].Rows.Count > 0)
            {
                coll_name = dsset.Tables[2].Rows[0]["collname"].ToString();
                coll_address1 = dsset.Tables[2].Rows[0]["address1"].ToString();
                coll_address2 = dsset.Tables[2].Rows[0]["address2"].ToString();
                coll_address3 = dsset.Tables[2].Rows[0]["address3"].ToString();
                pin_code = dsset.Tables[2].Rows[0]["pincode"].ToString();
            }

            sql = " SELECT m.*,st.dept_code,m.deductions as deductions,m.allowances as allowances,isnull(m.netsal,0) as netsal,isnull(m.payscale,0) as payscale,ISNULL(m.Actual_Basic,0) as Actual_Basic,ISNULL(m.IncrementAmt,0) as IncrementAmt1,ISNULL(m.DAWithLOP,0) as DALop,m.IncrementTime as IncrementTime,m.Pre_Lop as Pre_Lop,m.Cur_Lop as Cur_Lop,m.stftype as stftype,m.leavedetail as leavedetail,ISNULL(m.basic_alone,0) as basic_alone,ISNULL(m.DAAmt,0) as DA,m.Basic as Basic,ISNULL(m.AGP,0) as AGP,ISNULL(m.bsalary,0) as bsalary,isnull(m.NetAddAct,0) as netaddact,isnull(m.netded,0) as netded,sm.ESI_No as ESI_No,dm.priority,dept_acronym,staff_name,bankaccount,pfnumber,CONVERT(VARCHAR(10),sm.join_date,103) as joindate,ISNULL(m.pay_band,0) as pay_band,ISNULL(m.grade_pay,0) as grade_pay,m.pf as pf,dm.desig_acronym,ISNULL(m.LOP,0) as LOP,len(sm.staff_code),sm.staff_code,dm.print_pri,ISNULL(m.PF_Salary,0) as PF_Salary,ISNULL(m.ESI_Salary,0) as ESI_Salary,hd.dept_name as deptname,dm.desig_name as designame,sm.resign,Convert(varchar(10),sm.appointed_date,103) as appointdate,Convert(varchar(10),sm.retr_date,103) as resigndate,Convert(varchar(10),sa.date_of_birth,103) as dateofbirth,title,convert(decimal ,isnull(m.AdvanceAmt,0)) as AdvanceAmt1,sc.category_name,case when st.PayMode='0' then 'Cash' when st.PayMode='1' then 'Cheque' when st.PayMode='2' then 'Credit'  end as PayMode,case when st.BankAccType='1' then 'Own Account' when st.BankAccType='2' then 'Nominee Account' end as BankAccType,sm.ifsc_code,sm.bank_name,sm.branch_name,sm.pangirnumber,sm.lic_no,sm.adharcardno,sm.loan_no, sm.gpfnumber,sm.UAN_No,hwp.tot_hrs,hwp.amnt_per_hrs ,'' CONVENES_EXP,'' LUNCH_EXP from monthlypay m,desig_master dm,staffmaster sm,hrdept_master hd,staff_appl_master sa,staffcategorizer sc,stafftrans st left join HourWise_PaySettings hwp on st.desig_code=hwp.desig_code and st.dept_code=hwp.dept_code and st.staff_code=hwp.staffcode and isnull(hwp.PayType,0)='1' Where sa.appl_no=sm.appl_no and m.staff_code=st.staff_code and m.staff_code=sm.staff_code and sm.staff_code=st.staff_code and st.latestrec=1 and st.desig_code=dm.desig_code and hd.dept_code=st.dept_code and sc.category_code=st.category_code and sm.college_code=dm.collegecode and sm.college_code=sc.college_code and sm.college_code=m.college_code and ((sm.resign=0 or sm.settled=0) or (sm.resign=1 and sm.relieve_date>='" + dateto + "') or (sm.resign=1 and sm.relieve_date between '" + datefrom + "' and '" + dateto + "')) and relieve_date between '" + datefrom + "' and '" + dateto + "' and m.PayMonth ='" + ddl_mon.SelectedValue.ToString() + "' and m.PayYear ='" + ddl_year.SelectedValue.ToString() + "' and sm.college_code='" + collegecode1 + "'";

            if (txtstaffname.Text.Trim() != "")
            {
                sql = sql + " and sm.staff_name ='" + txtstaffname.Text + "'";
            }
            else if (txtstaffcode.Text.Trim() != "")
            {
                sql = sql + " and sm.staff_code ='" + txtstaffcode.Text + "'";
            }
            else
            {
                if (txt_dept.Text != "---Select---")
                {
                    int itemcount = 0;
                    for (itemcount = 0; itemcount < cbl_dept.Items.Count; itemcount++)
                    {
                        if (cbl_dept.Items[itemcount].Selected == true)
                        {
                            if (strdept == "")
                                strdept = "'" + cbl_dept.Items[itemcount].Value.ToString() + "'";
                            else
                                strdept = strdept + "," + "'" + cbl_dept.Items[itemcount].Value.ToString() + "'";
                        }
                    }
                    if (strdept != "")
                    {
                        sql = sql + " and hd.dept_code in(" + strdept + ")";
                    }
                }
                string strdesig = "";
                for (int idesig = 0; idesig < cbl_desig.Items.Count; idesig++)
                {
                    if (cbl_desig.Items[idesig].Selected == true)
                    {
                        if (strdesig == "")
                        {
                            strdesig = "'" + cbl_desig.Items[idesig].Value.ToString() + "'";
                        }
                        else
                        {
                            strdesig = strdesig + ",'" + cbl_desig.Items[idesig].Value.ToString() + "'";
                        }
                    }
                }
                if (strdesig != "")
                {
                    sql = sql + " and dm.desig_code in(" + strdesig + ")";
                }
            }
            string strtypeval = "";
            for (int idesig = 0; idesig < cbl_stafftyp.Items.Count; idesig++)
            {
                if (cbl_stafftyp.Items[idesig].Selected == true)
                {
                    if (strtypeval == "")
                    {
                        strtypeval = "'" + cbl_stafftyp.Items[idesig].Value.ToString() + "'";
                    }
                    else
                    {
                        strtypeval = strtypeval + ",'" + cbl_stafftyp.Items[idesig].Value.ToString() + "'";
                    }
                }
            }
            if (strtypeval != "")
            {
                sql = sql + " and st.stftype in(" + strtypeval + ")";
            }
            if (txt_staffcat.Text != "---Select---")
            {
                int itemcount1 = 0;
                for (itemcount1 = 0; itemcount1 < cbl_staffcat.Items.Count; itemcount1++)
                {
                    if (cbl_staffcat.Items[itemcount1].Selected == true)
                    {
                        if (strcategory == "")
                            strcategory = "'" + cbl_staffcat.Items[itemcount1].Value.ToString() + "'";
                        else
                            strcategory = strcategory + "," + "'" + cbl_staffcat.Items[itemcount1].Value.ToString() + "'";
                    }
                }
                if (strcategory != "")
                {
                    sql = sql + " and st.category_code in(" + strcategory + ")";
                }
            }
            sql = sql + " select sa.per_mobileno,sa.email,sm.staff_code,allowances,deductions from staff_appl_master sa,staffmaster sm, stafftrans st  where sm.appl_no=sa.appl_no and ((sm.settled=0 and sm.resign=0) and ISNULL(sm.Discontinue,'0')='0') and sm.staff_code=st.staff_code  and st.latestrec=1";
            sql = sql + "  select shortname from leave_category where status ='comp' and college_code =" + collegecode1 + "";
            sql = sql + " select * from incentives_master where college_code='" + collegecode1 + "'";
            string df = fromdate;
            string dt = todate;
            string[] stf = df.Split('/');
            string[] stt = dt.Split('/');
            DateTime dtf = Convert.ToDateTime(stf[1] + '/' + stf[0] + '/' + stf[2]);
            DateTime dtt = Convert.ToDateTime(stt[1] + '/' + stt[0] + '/' + stt[2]);
            string qry = " select * from staff_attnd where mon_year between '" + dtf.ToString("M/yyyy") + "' and '" + dtt.ToString("M/yyyy") + "'";
            qry += " select linkvalue from new_inssettings where linkName='Parttime Staff Convenes Expance' and college_code='" + collegecode1 + "'";
            DataSet dsstaffatt = d2.select_method_wo_parameter(qry, "Text");
            ArrayList addleave = new ArrayList();

            dsset.Reset();
            dsset.Dispose();
            dsset = d2.select_method_wo_parameter(sql, "Text");
            if (dsset.Tables[0].Rows.Count > 0)
            {
                DataTable dtcat = new DataTable();
                DataRow drow;
                dtcat.Columns.Add("stfCode", typeof(string));
                dtcat.Columns.Add("stfName", typeof(string));
                dtcat.Columns.Add("deptName", typeof(string));
                dtcat.Columns.Add("desigName", typeof(string));
                for (int i = 0; i < dsset.Tables[0].Rows.Count; i++)
                {
                    drow = dtcat.NewRow();
                    drow["stfCode"] = Convert.ToString(dsset.Tables[0].Rows[i]["staff_code"]);

                    drow["stfName"] = Convert.ToString(dsset.Tables[0].Rows[i]["staff_name"]);
                    drow["deptName"] = Convert.ToString(dsset.Tables[0].Rows[i]["deptname"]);
                    drow["desigName"] = Convert.ToString(dsset.Tables[0].Rows[i]["designame"]);
                    dtcat.Rows.Add(drow);
                }
                div1.Visible = true;
                grdfinalsettlement.DataSource = dtcat;
                grdfinalsettlement.DataBind();
                grdfinalsettlement.Visible = true;

                for (int l = 0; l < grdfinalsettlement.Rows.Count; l++)
                {
                    foreach (GridViewRow row in grdfinalsettlement.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            grdfinalsettlement.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            grdfinalsettlement.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Left;
                            grdfinalsettlement.Rows[l].Cells[2].HorizontalAlign = HorizontalAlign.Left;
                            grdfinalsettlement.Rows[l].Cells[3].HorizontalAlign = HorizontalAlign.Left;
                            grdfinalsettlement.Rows[l].Cells[4].HorizontalAlign = HorizontalAlign.Left;

                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"No Records Found\");", true);
            }


        }
        catch (Exception ex)
        {

        }

    }
    protected void grdfinalsettlement_RowDataBound(object sende, GridViewRowEventArgs e)
    {
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
    protected void SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            int getrowcountafter = 0;
            int getrowcountsec = 0;
            int colspan = 0;
            string coll_name = string.Empty;
            string qur = "select isnull(collname, ' ') as collname,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode from collinfo where college_code='" + collegecode1 + "'";
            DataSet dsval = new DataSet();
            dsval = d2.select_method_wo_parameter(qur, "text");
            if (dsval.Tables[0].Rows.Count > 0)
            {
                coll_name = Convert.ToString(dsval.Tables[0].Rows[0]["collname"]);
            }
            int getdeductrowcount = 0;


            int col = 0;
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            string str = "select isnull(collname, ' ') as collname,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode from collinfo where college_code='" + collegecode1 + "'";
            str = str + " ;select * from incentives_master where college_code='" + collegecode1 + "'";
            str = str + " ;select distinct CONVERT(VARCHAR(10),from_date,103) as from_date,convert(VARCHAR(10),to_date,103) as to_date from hrpaymonths where paymonthnum='" + ddl_mon.SelectedItem.Value.ToString() + "'and PayYear='" + ddl_year.SelectedItem.Text.ToString() + "' and college_Code=" + collegecode1 + "";
            DataSet ds = d2.select_method_wo_parameter(str, "Text");

            string allowmaster = "";
            string deductmaster = "";
            if (ds.Tables[1].Rows.Count > 0)
            {
                allowmaster = ds.Tables[1].Rows[0]["allowances"].ToString();
                deductmaster = ds.Tables[1].Rows[0]["deductions"].ToString();
            }
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string[] spdet = deductmaster.Split(';');
            for (int d = 0; d <= spdet.GetUpperBound(0); d++)
            {
                string[] spdedet = spdet[d].Split('\\');
                if (spdedet.GetUpperBound(0) >= 1)
                {
                    string val = spdedet[0];
                    string val1 = spdedet[1];
                    if (!dict.ContainsKey(val))
                    {
                        dict.Add(val, val1);
                    }
                }
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                string date3 = Convert.ToString(ds.Tables[2].Rows[0]["from_date"]);
                string datefrom3;
                int monthname1;
                string monyear;
                string monthname2 = "";
                int monthnamenum;
                string yearto = "";
                string monyearto = "";
                string[] split = date3.Split(new Char[] { '/' });
                int totlastaff = 0;
                int left1 = 20;
                int left1a = 135;
                int left2 = 145;
                int left3 = 423;
                int left4 = 570;
                int incrre = 1;
                int rowcount = 0;
                datefrom3 = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                string date4 = Convert.ToString(ds.Tables[2].Rows[0]["to_date"]);
                int year3 = Convert.ToInt16(split[2].ToString());
                string[] split1 = date4.Split(new Char[] { '/' });
                string dateto4 = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                monthname2 = split1[0].ToString();
                monthnamenum = Convert.ToInt32(monthname2.ToString());
                yearto = split1[2].ToString();
                string mnmae = split[1].ToString();
                monthname1 = Convert.ToInt16(mnmae);
                monthname2 = split1[1].ToString();
                monthnamenum = Convert.ToInt16(monthname2);
                string year = split[2].ToString();
                monyear = monthname1.ToString() + "/" + year;
                monyearto = monthnamenum.ToString() + "/" + yearto;
                Boolean getvalflag = false;
                int dayfrm = 0;
                string dayto;
                int daytonum;
                string lopdates = "";
                string[] leavetype = new string[50];
                sql = "select shortname from leave_category where college_code=" + Session["collegecode"] + "";
                DataSet dsleave = d2.select_method_wo_parameter(sql, "Text");
                int lev = 0;
                for (int le = 0; le < dsleave.Tables[0].Rows.Count; le++)
                {
                    lev++;
                    string levatype = dsleave.Tables[0].Rows[le]["shortname"].ToString().Trim().ToLower();
                    leavetype[lev] = levatype;
                }


                int Pay_year = Convert.ToInt32(ddl_year.SelectedItem.Value);
                int pay_month = Convert.ToInt32(ddl_mon.SelectedItem.Value);

                if (pay_month == 1)
                {
                    pay_month = 12;
                    Pay_year = Pay_year - 1;

                }
                else
                {
                    pay_month = pay_month - 1;
                }

                sql = "select m.*,s.staff_name,s.pfnumber,s.ESI_No,s.bankaccount,s.pangirnumber ,IsManualLOP,st.payscale as pay_scalenew , st.allowances as actallowance,h.dept_name as deptname,d.desig_name as designame from monthlypay m,staffmaster s,stafftrans st,hrdept_master h,desig_master d where s.staff_code=m.staff_code and st.dept_code=h.dept_code and st.desig_code=d.desig_code and m.staff_code=st.staff_code and st.latestrec = 1 and m.college_code=s.college_code and m.college_code =h.college_code and m.college_code=d.collegeCode and s.college_code='" + collegecode1 + "' and m.PayYear='" + Convert.ToString(Pay_year) + "' and m.PayMonth='" + Convert.ToString(pay_month) + "'";
                sql = sql + " ; select convert(nvarchar(15),sa.date_of_birth,103) as dob,sm.staff_code,convert(nvarchar(15),sm.retr_date ,103) as retir,sm.staff_name,CONVERT(varchar(10),sm.join_date,103) as join_date,CONVERT(varchar(10),sm.relieve_date,103) as relieve_date,CONVERT(varchar(10),sm.resigdate,103) as resigdate from staff_appl_master sa,staffmaster sm where sa.appl_no=sm.appl_no ";


                string query = "select m.*,s.staff_name,s.pfnumber,s.ESI_No,s.bankaccount,s.pangirnumber ,IsManualLOP,st.payscale as pay_scalenew , st.allowances as actallowance,h.dept_name as deptname,d.desig_name as designame,s.noticepay from monthlypay m,staffmaster s,stafftrans st,hrdept_master h,desig_master d where s.staff_code=m.staff_code and st.dept_code=h.dept_code and st.desig_code=d.desig_code and m.staff_code=st.staff_code and st.latestrec = 1 and m.college_code=s.college_code and m.college_code =h.college_code and m.college_code=d.collegeCode and s.college_code='" + collegecode1 + "' and m.PayYear='" + ddl_year.SelectedValue.ToString() + "' and m.PayMonth='" + ddl_mon.SelectedValue.ToString() + "'";


                DataSet dspay = d2.select_method_wo_parameter(sql, "text");
                DataSet dsstaff = d2.select_method_wo_parameter(query, "text");
                DataTable dtpay = dspay.Tables[0];
                DataTable dtstaffinfo = dspay.Tables[1];
                DataView dvpay = new DataView();
                DataView dvstaffinfo = new DataView();
                DataView dvapp = new DataView();
                DataView currentmonth = new DataView();
                Hashtable deductHash = new Hashtable();
                Hashtable deducthash1 = new Hashtable();
                Hashtable allowhash = new Hashtable();


                DateTime dtDate = new DateTime(2000, pay_month, 1);
                string sMonthName = dtDate.ToString("MMM");

                DateTime dtDates = new DateTime(2000, Convert.ToInt32(ddl_mon.SelectedValue), 1);
                string shortMonthName = dtDates.ToString("MMM");
                string previousyear = Convert.ToString(Pay_year).Substring(Convert.ToString(Pay_year).Length - 2);
                string currentyear = ddl_year.SelectedValue.ToString().Substring(ddl_year.SelectedValue.ToString().Length - 2);
                Label getstaffcode = (Label)grdfinalsettlement.Rows[rowIndex].FindControl("stfcode");
                string staffcode = Convert.ToString(getstaffcode.Text);
                DataTable dtl = new DataTable();
                DataRow dtrow = null;

                string graduity = string.Empty;
                graduity = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='gratuity' and user_code='" + usercode + "' and college_code='" + collegecode1 + "'");

                string grttxt = string.Empty;
                int grtval = 0;
                if (staffcode != "")
                {
                    dtl.Columns.Add("Column1");
                    dtl.Columns.Add("Column2");
                    dtl.Columns.Add("Column3");
                    dtl.Columns.Add("Column4");
                    dtrow = dtl.NewRow();
                    dtrow[col] = "FULL & FINAL SETTLEMENT SHEET";
                    dtl.Rows.Add(dtrow);

                    dtrow = dtl.NewRow();


                    string todaysdate = Convert.ToString(DateTime.Now);

                    if (todaysdate.Contains(' '))
                    {
                        string[] dateToday = todaysdate.Split(' ');
                        string[] split2 = dateToday[0].Split('/');
                        string formatedDate = Convert.ToString(split2[1]) + "-" + Convert.ToString(split2[0]) + "-" + Convert.ToString(split2[2]);
                        dtrow[col] = "RETIREMENT/RESIGNATION/TERMINATION" + " " + " " + " " + " " + " " + " " + "DATE:" + Convert.ToString(formatedDate);

                    }
                    dtl.Rows.Add(dtrow);

                    dtrow = dtl.NewRow();
                    dtrow[col] = "STAFF INFORMATION";
                    dtl.Rows.Add(dtrow);
                    dtpay.DefaultView.RowFilter = " staff_code='" + staffcode + "'";
                    dvpay = dtpay.DefaultView;
                    if (dvpay.Count > 0)
                    {
                        Boolean shortnam = false;
                        string staffname = "";
                        string netsalary = "";
                        string bankno = "";
                        string pfno = "";
                        string esino = "";
                        string panno = "";
                        string Allowances = "";
                        string lopamount = "";
                        string designation = "";
                        string deptname = "";
                        Double totdection = 0;
                        deductHash.Clear();
                        deducthash1.Clear();
                        staffname = dvpay[0]["staff_name"].ToString();
                        bankno = dvpay[0]["bankaccount"].ToString();
                        pfno = dvpay[0]["pfnumber"].ToString();
                        esino = dvpay[0]["ESI_No"].ToString();
                        panno = dvpay[0]["pangirnumber"].ToString();
                        Allowances = dvpay[0]["allowances"].ToString();
                        lopamount = dvpay[0]["Tot_lop"].ToString();
                        designation = dvpay[0]["designame"].ToString();
                        deptname = dvpay[0]["deptname"].ToString();
                        netsalary = Convert.ToString(dvpay[0]["netadd"]);

                        netsalary = String.Format("{0:0.00}", netsalary);

                        if (netsalary.Contains('.'))
                        {
                            string[] splitval = netsalary.Split('.');
                            netsalary = splitval[0];
                        }
                        if (pfno.Trim().ToLower() == "" || pfno == null || pfno.Trim() == "0")
                        {
                            pfno = "-";
                        }
                        string setyear = dvpay[0]["PayYear"].ToString();
                        if (setyear.Trim() == "" || setyear == null)
                        {
                            setyear = yearto;
                        }
                        string noofpresent = "";
                        string presnt = "";
                        string workdays = "";
                        string leavedays = "";
                        string nooflop = "";
                        noofpresent = dvpay[0]["leavedetail"].ToString();
                        string[] presplit = noofpresent.Split(';');
                        if (presplit.Length >= 7)
                        {
                            presnt = presplit[1].ToString();
                            workdays = presplit[0].ToString();
                            leavedays = presplit[2].ToString();
                            nooflop = presplit[6].ToString();
                        }

                        dtstaffinfo.DefaultView.RowFilter = " staff_code='" + staffcode + "'";
                        dvstaffinfo = dtstaffinfo.DefaultView;
                        dsstaff.Tables[0].DefaultView.RowFilter = "staff_code='" + staffcode + "'";

                        currentmonth = dsstaff.Tables[0].DefaultView;

                        string presnts = "";
                        string workdayss = "";
                        string leavedayss = "";
                        string nooflops = "";

                        string noofpresents = currentmonth[0]["leavedetail"].ToString();
                        string[] presplits = noofpresents.Split(';');
                        if (presplits.Length >= 7)
                        {
                            presnts = presplits[1].ToString();
                            workdayss = presplits[0].ToString();
                            leavedayss = presplits[2].ToString();
                            nooflops = presplits[6].ToString();
                        }

                        string joindate = string.Empty;
                        string relvedate = string.Empty;
                        string lastworkingday = string.Empty;
                        string resignationDate = string.Empty;
                        string resigndate = string.Empty;
                        int colspancount = 0;
                        if (dvstaffinfo.Count > 0)
                        {
                            joindate = Convert.ToString(dvstaffinfo[0]["join_date"]);
                            resignationDate = Convert.ToString(dvstaffinfo[0]["retir"]);
                            relvedate = Convert.ToString(dvstaffinfo[0]["relieve_date"]);
                            resigndate = Convert.ToString(dvstaffinfo[0]["resigdate"]);
                        }

                        if (joindate.Contains('/'))
                        {
                            string[] splitjoin = joindate.Split('/');
                            joindate = Convert.ToString(splitjoin[1] + "-" + splitjoin[0] + "-" + splitjoin[2]);

                        }
                        if (relvedate.Contains('/'))
                        {
                            string[] splits = relvedate.Split('/');
                            relvedate = Convert.ToString(splits[1] + "-" + split1[0] + "-" + split1[2]);

                        }
                        if (resigndate.Contains('/'))
                        {
                            string[] resigsplit = resigndate.Split('/');
                            resigndate = Convert.ToString(resigsplit[1] + "-" + resigsplit[0] + "-" + resigsplit[2]);


                        }
                        DateTime join_datetime = Convert.ToDateTime(joindate);

                        DateTime TodayData = DateTime.Now;
                        if (relvedate != "")
                        {
                            TodayData = Convert.ToDateTime(relvedate);
                        }
                        int Years = TodayData.Year - join_datetime.Year;
                        int month = TodayData.Month - join_datetime.Month;

                        dtrow = dtl.NewRow();
                        dtrow[col] = "NAME OF THE STAFF";

                        dtrow[col + 2] = Convert.ToString(staffname);
                        dtl.Rows.Add(dtrow);

                        dtrow = dtl.NewRow();
                        dtrow[col] = "DESIGNATIOM";

                        dtrow[col + 2] = Convert.ToString(designation);
                        dtl.Rows.Add(dtrow);

                        dtrow = dtl.NewRow();
                        dtrow[col] = "DEPARTMENT";

                        dtrow[col + 2] = Convert.ToString(deptname);
                        dtl.Rows.Add(dtrow);

                        dtrow = dtl.NewRow();
                        dtrow[col] = "DATE OF JOINING";

                        dtrow[col + 2] = Convert.ToString(joindate);
                        dtl.Rows.Add(dtrow);

                        dtrow = dtl.NewRow();
                        dtrow[col] = "DATE OF RESIGNATION";
                        dtrow[col + 2] = Convert.ToString(resigndate);

                        dtl.Rows.Add(dtrow);

                        dtrow = dtl.NewRow();
                        dtrow[col] = "DATE OF LAST WORKING DAY";
                        dtrow[col + 2] = Convert.ToString(relvedate);
                        
                        dtl.Rows.Add(dtrow);

                        dtrow = dtl.NewRow();
                        dtrow[col] = "GROSS SALARY P.M";

                        dtrow[col + 2] = "Rs." + Convert.ToString(netsalary);
                        dtl.Rows.Add(dtrow);

                        string applid = d2.GetFunction("select appl_id from staff_appl_master a,staffmaster s where a.appl_no=s.appl_no and staff_code='" + staffcode + "'");
                        string querys = "select * from individual_leave_type where staff_code='" + staffcode + "' and college_code='" + Session["collegecode"] + "'";
                        DataSet ds2 = new DataSet();
                        DataSet ds1 = new DataSet();
                        ds2.Clear();
                        ds2 = d2.select_method_wo_parameter(querys, "Text");
                        double addtot = 0;
                        if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                        {

                            string[] spl_type = ds2.Tables[0].Rows[0]["leavetype"].ToString().Split(new Char[] { '\\' });
                            int col_cnt = 0;
                            for (int i = 0; spl_type.GetUpperBound(0) >= i; i++)
                            {
                                col_cnt++;
                                string[] split_leave = spl_type[i].Split(';');
                                string shortname = d2.GetFunction("select shortname from leave_category where category='" + split_leave[0] + "'");
                                if (shortname == "Earned Leave" || shortname == "EARNED LEAVE" || shortname == "EL")
                                {
                                    shortnam = true;
                                }

                            }
                            int leavetypecount = spl_type.GetUpperBound(0);
                            double tot_leave = 0;
                            string leavefromdate = "";
                            string leavetodate = "";
                            string ishalfdate = "";
                            string halfdaydate = "";
                            int finaldate = 0;
                            string sleave = "";
                           
                            double llcount = 0;

                            for (int i = 0; spl_type.GetUpperBound(0) >= i; i++)
                            {
                                if (spl_type[i].Trim() != "")
                                {

                                    tot_leave = 0;
                                    string[] split_leave = spl_type[i].Split(';');
                                    string leave = split_leave[0];
                                    if (leave == "EARNED LEAVE" || leave == "Earned Leave" || leave == "EL")
                                    {

                                        if (split_leave.Length >= 2)
                                        {
                                            string s = Convert.ToString(split_leave[1]);
                                            if (s == "" || s.Trim() == "-")
                                                addtot = 0;
                                            else
                                                addtot = Convert.ToDouble(s);
                                        }

                                        string leavepk = d2.GetFunction("select LeaveMasterPK from leave_category where category='" + leave + "' and college_code='" + Session["collegecode"] + "'");
                                        string dt_get_leave = "select * from RQ_Requisition r,leave_category l where RequestType=5 and LeaveFrom>='" + dvstaffinfo[0]["join_date"].ToString() + "' and ReqAppNo='" + applid + "' and ReqAppStatus='1' and l.LeaveMasterPK=r.LeaveMasterFK and r.LeaveMasterFK='" + leavepk + "' ";
                                        ds1 = d2.select_method_wo_parameter(dt_get_leave, "Text");
                                        if (ds1.Tables[0].Rows.Count > 0)
                                        {
                                            for (int g = 0; g < ds1.Tables[0].Rows.Count; g++)
                                            {
                                                leavefromdate = Convert.ToString(ds1.Tables[0].Rows[g]["LeaveFrom"]);
                                                leavetodate = Convert.ToString(ds1.Tables[0].Rows[g]["LeaveTo"]);
                                                ishalfdate = Convert.ToString(ds1.Tables[0].Rows[g]["IsHalfDay"]);
                                                if (leavefromdate != "" && leavetodate != "")
                                                {
                                                    string dtT = leavefromdate;
                                                    string[] Split = dtT.Split('/');
                                                    string enddt = leavetodate;
                                                    Split = enddt.Split('/');
                                                    DateTime fromdate = Convert.ToDateTime(dtT);
                                                    DateTime todate = Convert.ToDateTime(enddt);
                                                    TimeSpan days = todate - fromdate;
                                                    string ndate = Convert.ToString(days);
                                                    Split = ndate.Split('.');
                                                    string getdate = Split[0];

                                                    if (fromdate != todate)
                                                    {
                                                        for (; fromdate <= todate; )
                                                        {
                                                            string dayy = fromdate.ToString("dddd");


                                                            string qur1 = "select * from individual_leave_type where  staff_code='" + staffcode + "' and college_code=" + Session["collegecode"] + "";
                                                            ds2 = d2.select_method_wo_parameter(qur1, "Text");
                                                            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                                                            {
                                                                string[] spl_type1 = ds2.Tables[0].Rows[0]["leavetype"].ToString().Split(new Char[] { '\\' });
                                                                for (int f = 0; spl_type.GetUpperBound(0) >= f; f++)
                                                                {
                                                                    string[] split_leave1 = spl_type1[f].Split(';');


                                                                }
                                                            }

                                                            fromdate = fromdate.AddDays(1);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        llcount++;
                                                    }
                                                    if (ishalfdate == "True")
                                                    {
                                                        halfdaydate = Convert.ToString(ds1.Tables[0].Rows[g]["HalfDate"]);
                                                        if (tot_leave == 0)
                                                        {
                                                            
                                                            tot_leave = llcount;
                                                            tot_leave = tot_leave - 0.5;
                                                        }
                                                        else
                                                        {
                                                           
                                                            tot_leave = tot_leave + llcount;
                                                            tot_leave = tot_leave - 0.5;
                                                        }
                                                       
                                                    }
                                                    else
                                                    {
                                                        if (tot_leave == 0)
                                                        {
                                                          
                                                            tot_leave = tot_leave + llcount;
                                                        }
                                                        else
                                                        {
                                                          
                                                            tot_leave = tot_leave + llcount;
                                                        }
                                                       
                                                    }
                                                }
                                            }
                                        }

                                        addtot = addtot - tot_leave;
                                    }
                                }
                            }

                        }

                        dtrow = dtl.NewRow();
                        dtrow[col] = "EARNED LEAVES BALANCE";

                        dtrow[col + 2] = Convert.ToString(addtot)+" "+" "+"Days";
                        dtl.Rows.Add(dtrow);

                        ArrayList grad = new ArrayList();

                       

                        dtrow = dtl.NewRow();
                        dtrow[col] = "GRATUITY ELIGIBILITY";

                        if (graduity != "0")
                        {
                            if (Years > 0)
                            {
                                dtrow[col + 2] = "Yes" + " " + " " + Years + " " + "Yrs";
                            }
                            else
                            {
                                dtrow[col + 2] = "No" + " " + " " + "0" + " " + "Yrs";
                            }
                        }
                        else
                        {
                            dtrow[col + 2] = "No" + " " + " " + "0" + " " + "Yrs";
                        }
                        dtl.Rows.Add(dtrow);

                        dtrow = dtl.NewRow();
                        dtrow[col] = "TOTAL PAYABLE DAYS FOR THE MONTH" + " " + sMonthName.ToUpper() + "" + previousyear;

                        dtrow[col + 2] = Convert.ToString(workdays) + " " + "Days";
                        dtl.Rows.Add(dtrow);



                        string deduction3 = "";
                        deduction3 = dvpay[0]["deductions"].ToString();
                        string[] deduction_arr1;
                        string deductionsplit;

                        deduction_arr1 = deduction3.Split('\\');
                        int exatval = 0;
                        int countval = 0;
                        for (int i = 0; i < deduction_arr1.GetUpperBound(0); i++)
                        {

                            exatval = deduction_arr1.GetUpperBound(0);
                            deductionsplit = deduction_arr1[i];
                            string[] deductionda = deductionsplit.Split(';');
                            if (deductionda.GetUpperBound(0) >= 3)
                            {
                                string da = deductionda[0];
                                string mode = Convert.ToString(deductionda[1]);
                                string daac = "";
                                string da3 = "";


                                if (deductionda[2].Trim() != "")
                                {
                                    string[] spval = deductionda[2].Split('-');
                                    if (spval.Length == 2)
                                    {
                                        if (mode.Trim().ToUpper() == "PERCENT")
                                        {
                                            da3 = Convert.ToString(spval[1]);
                                        }
                                        else
                                        {
                                            da3 = Convert.ToString(spval[0]);
                                        }
                                    }
                                    else
                                    {
                                        da3 = Convert.ToString(deductionda[3]);
                                    }
                                }
                                daac = Convert.ToString(deductionda[3]);
                                double da2 = 0;
                                Double.TryParse(daac, out da2);
                                double ds3 = 0;
                                double.TryParse(da3, out ds3);
                                ds3 = Math.Round(ds3, 2, MidpointRounding.AwayFromZero);
                                string DblAllowActVal = daac;
                                string DblAllowVal = deductionda[3];


                                if (ds3 != 0)
                                {
                                    if (!deductHash.Contains(da))
                                    {
                                        deductHash.Add(da, ds3);

                                    }
                                    countval++;

                                }

                            }
                        }
                        dtrow = dtl.NewRow();
                        dtrow[col] = "SALARY FOR THE MONTH OF" + " " + sMonthName.ToUpper() + "" + previousyear;

                        dtl.Rows.Add(dtrow);
                        dtrow = dtl.NewRow();
                        dtrow[col] = "EARNINGS";
                        dtrow[col + 1] = "AMOUNT";
                        dtrow[col + 2] = "DEDUCTIONS";
                        dtrow[col + 3] = "AMOUNT";
                        dtl.Rows.Add(dtrow);

                        dtrow = dtl.NewRow();
                        dtrow[col] = "CONSOLIDATE PAY";


                        dtrow[col + 1] = "Rs." + Convert.ToString(netsalary);
                        dtl.Rows.Add(dtrow);

                        int getroecount = dtl.Rows.Count;
                        int counts = 1;
                        int calcu = getroecount - counts;

                        double totaldeductPre = 0;
                        foreach (DictionaryEntry item in deductHash)
                        {
                            string key = Convert.ToString(item.Key);

                            string value = Convert.ToString(item.Value);
                            if (calcu < getroecount)
                            {
                                dtl.Rows[calcu][col + 2] = Convert.ToString(key).ToUpper();
                                dtl.Rows[calcu][col + 3] = "Rs." + Convert.ToString(value);
                                totaldeductPre = totaldeductPre + Convert.ToDouble(value);
                                getdeductrowcount++;
                            }
                            else
                            {
                                dtrow = dtl.NewRow();


                                dtrow[col + 2] = Convert.ToString(key).ToUpper();
                                dtrow[col + 3] = "Rs." + Convert.ToString(value);
                                totaldeductPre = totaldeductPre + Convert.ToDouble(value);
                                dtl.Rows.Add(dtrow);
                                getdeductrowcount++;
                            }
                            calcu++;
                        }


                        dtrow = dtl.NewRow();
                        dtrow[col] = "GROSS PAY";
                        dtrow[col + 1] = "Rs." + Convert.ToString(netsalary);
                        dtrow[col + 2] = "TOTAL";
                        dtrow[col + 3] = "Rs." + Convert.ToString(totaldeductPre);
                        dtl.Rows.Add(dtrow);
                        double netprevious = Convert.ToDouble(netsalary) - totaldeductPre;
                        getrowcountafter = dtl.Rows.Count;
                        dtrow = dtl.NewRow();
                        dtrow[col] = "NET SALARY PAYABLE  FOR THE MONTH" + " " + sMonthName.ToUpper() + "" + previousyear;
                        dtrow[col + 3] = "Rs." + Convert.ToString(netprevious);
                        dtl.Rows.Add(dtrow);

                        dtrow = dtl.NewRow();
                        dtrow[col] = "TOTAL PAYABLE DAYS FOR THE MONTH" + " " + shortMonthName.ToUpper() + "" + currentyear;
                        dtrow[col + 3] = Convert.ToString(workdayss) + " " + "Days";
                        dtl.Rows.Add(dtrow);

                        int gratuityval = 0;
                        int getnetsal = 0;
                        int gettotallow = 0;

                        if (graduity != "0")
                        {
                           
                            if (graduity.Contains('+'))
                            {
                                string[] splitval = graduity.Split('+');

                                for (int val = 0; val < splitval.Count(); val++)
                                {
                                    string gratuitytxt = Convert.ToString(splitval[val]);
                                    if (gratuitytxt != "Basic")
                                    {

                                        string allowance3 = "";
                                        allowance3 = dvpay[0]["allowances"].ToString();
                                        string[] allowanmce_arr1;
                                        string alowancesplit;
                                        int exatval1 = 0;

                                        double DblAllowLOP = 0;
                                        double DblAllowLOP1 = 0;
                                        allowanmce_arr1 = allowance3.Split('\\');
                                        for (int i = 0; i < allowanmce_arr1.GetUpperBound(0); i++)
                                        {
                                            exatval1 = allowanmce_arr1.GetUpperBound(0);
                                            alowancesplit = allowanmce_arr1[i];
                                            string[] allowanceda = alowancesplit.Split(';');
                                            if (allowanceda.GetUpperBound(0) >= 3)
                                            {
                                                string da = allowanceda[0];
                                                if (gratuitytxt == da)
                                                {

                                                    string mode = Convert.ToString(allowanceda[1]);
                                                    string daac = "";
                                                    string da3 = "";
                                                    if (allowanceda[2].Trim() != "")
                                                    {
                                                        string[] spval = allowanceda[2].Split('-');
                                                        if (spval.Length == 2)
                                                        {
                                                            if (mode.Trim().ToUpper() == "PERCENT")
                                                            {
                                                                da3 = Convert.ToString(spval[1]);
                                                            }
                                                            else
                                                            {
                                                                da3 = Convert.ToString(spval[0]);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            da3 = Convert.ToString(allowanceda[3]);
                                                        }
                                                    }
                                                    daac = Convert.ToString(allowanceda[3]);
                                                    double da2 = 0;
                                                    Double.TryParse(daac, out da2);
                                                    double ds3 = 0;
                                                    double.TryParse(da3, out ds3);
                                                    ds3 = Math.Round(ds3, 2, MidpointRounding.AwayFromZero);
                                                    string DblAllowActVal = daac;
                                                    string DblAllowVal = allowanceda[3];
                                                    DblAllowLOP = (Convert.ToDouble(DblAllowActVal) - Convert.ToDouble(DblAllowVal));
                                                    DblAllowLOP = Math.Round(DblAllowLOP);
                                                    DblAllowLOP1 = DblAllowLOP1 + DblAllowLOP;
                                                    if (ds3 != 0)
                                                    {
                                                        if (!allowhash.Contains(da))
                                                        {
                                                            allowhash.Add(da, da3);

                                                        }

                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (gratuitytxt == "Basic")
                                    {
                                        double basic_pay3 = 0;
                                        double.TryParse(currentmonth[0]["bsalary"].ToString(), out basic_pay3);
                                        if (!allowhash.Contains("Basic"))
                                        {
                                            allowhash.Add("Basic", basic_pay3);
                                        }


                                    }
                                }

                                foreach (DictionaryEntry item in allowhash)
                                {
                                    string key = Convert.ToString(item.Key);

                                    string value = Convert.ToString(item.Value);

                                    gettotallow = gettotallow + Convert.ToInt32(value);
                                }
                                gratuityval = (Convert.ToInt32(gettotallow) * 15 * Years / 26);
                            }
                            else
                            {
                                if (graduity.Trim() == "Gross Pay")
                                {
                                    gratuityval = (Convert.ToInt32(netsalary) * 15 * Years / 26);
                                }
                            }

                        }


                        string deduction3s = "";
                        double currentnet = 0;
                        double currenttot = 0;
                        deduction3s = currentmonth[0]["deductions"].ToString();//delsiref
                        currentnet = Convert.ToDouble(currentmonth[0]["netadd"]);
                        string[] deduction_arr1s;
                        string deductionsplits;
                        string currentnetsal = Convert.ToString(currentnet);
                        if (currentnetsal.Contains('.'))
                        {
                            string[] splitval = currentnetsal.Split('.');
                            currentnetsal = splitval[0];
                        }



                        deduction_arr1s = deduction3s.Split('\\');
                        int exatvals = 0;
                        int countvals = 0;
                        for (int i = 0; i < deduction_arr1s.GetUpperBound(0); i++)
                        {

                            exatvals = deduction_arr1s.GetUpperBound(0);
                            deductionsplits = deduction_arr1s[i];
                            string[] deductiondas = deductionsplits.Split(';');
                            if (deductiondas.GetUpperBound(0) >= 3)
                            {
                                string da = deductiondas[0];
                                string mode = Convert.ToString(deductiondas[1]);
                                string daac = "";
                                string da3 = "";


                                if (deductiondas[2].Trim() != "")
                                {
                                    string[] spval = deductiondas[2].Split('-');
                                    if (spval.Length == 2)
                                    {
                                        if (mode.Trim().ToUpper() == "PERCENT")
                                        {
                                            da3 = Convert.ToString(spval[1]);
                                        }
                                        else
                                        {
                                            da3 = Convert.ToString(spval[0]);
                                        }
                                    }
                                    else
                                    {
                                        da3 = Convert.ToString(deductiondas[3]);
                                    }
                                }
                                daac = Convert.ToString(deductiondas[3]);
                                double da2 = 0;
                                Double.TryParse(daac, out da2);
                                double ds3 = 0;
                                double.TryParse(da3, out ds3);
                                ds3 = Math.Round(ds3, 2, MidpointRounding.AwayFromZero);
                                string DblAllowActVal = daac;
                                string DblAllowVal = deductiondas[3];


                                if (ds3 != 0)
                                {
                                    if (!deducthash1.Contains(da))
                                    {
                                        deducthash1.Add(da, ds3);

                                    }
                                    countvals++;

                                }

                            }
                        }

                        dtrow = dtl.NewRow();
                        dtrow[col] = "SALARY FOR THE MONTH OF" + " " + shortMonthName.ToUpper() + "" + currentyear;

                        dtl.Rows.Add(dtrow);

                        dtrow = dtl.NewRow();
                        dtrow[col] = "EARNINGS";
                        dtrow[col + 1] = "AMOUNT";
                        dtrow[col + 2] = "DEDUCTIONS";
                        dtrow[col + 3] = "AMOUNT";
                        dtl.Rows.Add(dtrow);

                        dtrow = dtl.NewRow();
                        dtrow[col] = "CONSOLIDATE PAY";
                        dtrow[col + 1] = "Rs." + Convert.ToString(currentnetsal);
                        dtl.Rows.Add(dtrow);

                        int getroecount1 = dtl.Rows.Count;
                        int counts1 = 1;
                        int calcu1 = getroecount1 - counts1;

                        foreach (DictionaryEntry item in deducthash1)
                        {
                            string key = Convert.ToString(item.Key);

                            string value = Convert.ToString(item.Value);
                            if (calcu1 < getroecount1)
                            {
                                dtl.Rows[calcu1][col + 2] = Convert.ToString(key).ToUpper();
                                dtl.Rows[calcu1][col + 3] = "Rs." + Convert.ToString(value);
                                currenttot = currenttot + Convert.ToDouble(value);
                            }
                            else
                            {
                                dtrow = dtl.NewRow();


                                dtrow[col + 2] = Convert.ToString(key).ToUpper();
                                dtrow[col + 3] = "Rs." + Convert.ToString(value);
                                currenttot = currenttot + Convert.ToDouble(value);

                                dtl.Rows.Add(dtrow);
                            }
                            calcu1++;
                        }
                        currentnet = Convert.ToDouble(currentnetsal);

                        Double currentmonthnetpay = Convert.ToDouble(currentnet) - currenttot;
                        dtrow = dtl.NewRow();
                        dtrow[col] = "GROSS PAY";
                        dtrow[col + 1] = "Rs." + Convert.ToString(currentnetsal);
                        dtrow[col + 2] = "TOTAL";
                        dtrow[col + 3] = Convert.ToString("Rs." + currenttot);
                        dtl.Rows.Add(dtrow);
                        getrowcountsec = dtl.Rows.Count;
                        dtrow = dtl.NewRow();
                        dtrow[col] = "NET SALARY PAYABLE FOR THE MONTH" + " " + shortMonthName.ToUpper() + "" + currentyear;
                        dtrow[col + 3] = "Rs." + Convert.ToString(currentmonthnetpay);
                        dtl.Rows.Add(dtrow);


                        dtrow = dtl.NewRow();
                        dtrow[col] = "GRATUITY AMOUNT( will be claimed from LIC of India)";
                        dtrow[col + 3] = "Rs." + " " + Convert.ToString(gratuityval);
                        dtl.Rows.Add(dtrow);

                        double earnedleave = 0;
                        string earnedl = string.Empty;
                        if (workdayss != "")
                        {
                            earnedleave = Convert.ToDouble(netsalary) /Convert.ToDouble( workdayss)*addtot;
                            earnedl = Convert.ToString(earnedleave);
                            if (earnedl.Contains('.'))
                            {
                                string[] splitval = earnedl.Split('.');
                                earnedl = splitval[0];
                            }
                        }
                        dtrow = dtl.NewRow();
                        dtrow[col] = "EARNED LEAVE PAYMENT ( as per College norms)";
                        dtrow[col + 3] = "Rs." + " " + " " + earnedl;
                        dtl.Rows.Add(dtrow);

                        string noticepatamt = Convert.ToString(currentmonth[0]["noticepay"]);
                        double noticeamt = 0;
                        if (noticepatamt.Contains('.'))
                        {
                            string[] splitval = noticepatamt.Split('.');
                            noticepatamt = splitval[0];
                        }
                        if (noticepatamt != "")
                        {
                            noticeamt = Convert.ToDouble(noticepatamt);
                        }
                       

                        DataSet loandt = new DataSet();
                        string qury = "select Staff_Code,LoanType,LoanName,PolicyName,LoanCode,LoanAmount,PolicyAmt,IsInterest from StaffLoanDet where IsActive='1' and IsClose='0' and Staff_Code='" + staffcode + "'";
                        loandt = d2.select_method_wo_parameter(qury, "text");
                        double overallloanamt = 0;
                        if (loandt.Tables[0].Rows.Count > 0)
                        {
                            double emiamt = 0;
                            double loanamt = Convert.ToDouble(loandt.Tables[0].Rows[0]["LoanAmount"]);
                            string loancode = Convert.ToString(loandt.Tables[0].Rows[0]["LoanCode"]);
                            string getsumemi = d2.GetFunction("SELECT ISNULL(SUM(EMIAmt),0) From StaffLoanPayDet WHERE Staff_Code='" + staffcode + "' and LoanCode='" + loancode + "'");
                            Double.TryParse(getsumemi, out emiamt);
                            overallloanamt = loanamt - emiamt;

                        }



                        dtrow = dtl.NewRow();
                        dtrow[col] = "NOTICE PAY (ONE MONTH)";
                        dtrow[col + 3] = "Rs." + " " + noticepatamt;
                        dtl.Rows.Add(dtrow);
                        double totalpayment = Convert.ToDouble(currentmonthnetpay) + Convert.ToDouble(netprevious) + Convert.ToDouble(earnedl) + Convert.ToDouble(gratuityval) - Convert.ToDouble(noticeamt) - Convert.ToDouble(overallloanamt);

                        dtrow = dtl.NewRow();
                        dtrow[col] = "TOTAL AMOUNT";
                        dtrow[col + 3] = "Rs." + Convert.ToString(totalpayment);
                        dtl.Rows.Add(dtrow);

                       

                        dtrow = dtl.NewRow();
                        dtrow[col] = "(-) LOAN FROM COLLEGE";
                        dtrow[col + 3] = "Rs."+Convert.ToString(overallloanamt);
                        dtl.Rows.Add(dtrow);

                        dtrow = dtl.NewRow();
                        dtrow[col] = "TOTAL AMOUNT PAYABLE";
                        dtrow[col + 3] = "Rs." + Convert.ToString(totalpayment);
                        dtl.Rows.Add(dtrow);

                        string word = ConvertNumbertoWords(Convert.ToInt32(totalpayment));
                        dtrow = dtl.NewRow();
                        dtrow[col] = "RUPEES IN WORDS :" + word.ToUpper();
                        dtl.Rows.Add(dtrow);
                        colspan = dtl.Rows.Count;
                        dtrow = dtl.NewRow();
                        dtrow[col] = "No dues Clearance done from all dept:" + " " + " " + "Yes/NO,Prepared By :" + " " + " " + " Checked By:,,,_________________,Signature - MFA";
                        dtrow[col + 2] = ",,BURSAR";
                        dtl.Rows.Add(dtrow);


                        dtrow = dtl.NewRow();
                        dtrow[col] = " "+"------- for Accounts Department only-------,Payment vide Cheque No. :" + " " + "..........................,Date of Payment              :" + " " + ".........................., Name of Bank :" + " " + "..........................,,,Date:____________" + " " + " " + "Accounts Department,(Authorised Signatory)";
                        dtrow[col + 2] = "I hereby agree and confirm having received the above amount as full and final payment from the " + coll_name + " before signing this settlement document. I will prefer no other claim form the College,___________________________________________________,,,(Signature of Staff)";
                        dtl.Rows.Add(dtrow);


                    }

                }



                div2.Visible = true;
                grdgenfinalset.DataSource = dtl;
                grdgenfinalset.DataBind();
                grdgenfinalset.Visible = true;
                grdgenfinalset.HeaderRow.Visible = false;
                rptprint1.Visible = true;

                int rowcounts = grdgenfinalset.Rows.Count;

                for (int i = 0; i < rowcounts; i++)
                {
                    if (i == 0 || i == 1 || i == 2 || i == 13 || i == getrowcountafter + 2)
                    {
                        grdgenfinalset.Rows[i].Cells[0].ColumnSpan = 4;
                        grdgenfinalset.Rows[i].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdgenfinalset.Rows[i].Cells[1].Visible = false;
                        grdgenfinalset.Rows[i].Cells[2].Visible = false;
                        grdgenfinalset.Rows[i].Cells[3].Visible = false;
                    }
                    else if (i == 3 || i == 4 || i == 5 || i == 6 || i == 7 || i == 8 || i == 9 || i == 10 || i == 11 || i == 12)
                    {
                        grdgenfinalset.Rows[i].Cells[0].ColumnSpan = 3;
                        grdgenfinalset.Rows[i].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        grdgenfinalset.Rows[i].Cells[1].Visible = false;
                        grdgenfinalset.Rows[i].Cells[2].HorizontalAlign = HorizontalAlign.Center;
                        grdgenfinalset.Rows[i].Cells[2].ColumnSpan = 2;
                        grdgenfinalset.Rows[i].Cells[3].Visible = false;

                    }
                    else if (i == getrowcountafter || i == getrowcountafter + 1)//delsireff
                    {
                        grdgenfinalset.Rows[i].Cells[0].ColumnSpan = 3;
                        grdgenfinalset.Rows[i].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        grdgenfinalset.Rows[i].Cells[1].Visible = false;
                        grdgenfinalset.Rows[i].Cells[2].Visible = false;
                        grdgenfinalset.Rows[i].Cells[3].HorizontalAlign = HorizontalAlign.Right;

                    }
                    else if (i == getrowcountsec || i == getrowcountsec + 1 || i == getrowcountsec + 2 || i == getrowcountsec + 3 || i == getrowcountsec + 4 || i == getrowcountsec + 5 || i == getrowcountsec + 6)
                    {
                        grdgenfinalset.Rows[i].Cells[0].ColumnSpan = 3;
                        grdgenfinalset.Rows[i].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        grdgenfinalset.Rows[i].Cells[1].Visible = false;
                        grdgenfinalset.Rows[i].Cells[2].Visible = false;
                        grdgenfinalset.Rows[i].Cells[3].HorizontalAlign = HorizontalAlign.Right;
                    }
                    else if (i == getrowcountsec + 7)
                    {
                        grdgenfinalset.Rows[i].Cells[0].ColumnSpan = 4;
                        grdgenfinalset.Rows[i].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdgenfinalset.Rows[i].Cells[1].Visible = false;
                        grdgenfinalset.Rows[i].Cells[2].Visible = false;
                        grdgenfinalset.Rows[i].Cells[3].Visible = false;
                    }
                    else if (i == colspan || i == colspan + 1 || i == colspan + 2 || i == colspan + 3 || i == colspan + 4 || i == colspan + 5 || i == colspan + 6 || i == colspan + 7 || i == colspan + 8 || i == colspan + 9 || i == colspan + 10 || i == colspan + 11 || i == colspan + 12)
                    {
                        grdgenfinalset.Rows[i].Cells[0].ColumnSpan = 3;

                        grdgenfinalset.Rows[i].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        grdgenfinalset.Rows[i].Cells[2].ColumnSpan = 2;
                        grdgenfinalset.Rows[i].Cells[2].HorizontalAlign = HorizontalAlign.Center;
                        grdgenfinalset.Rows[i].Cells[3].Visible = false;

                        grdgenfinalset.Rows[i].Cells[1].Visible = false;

                    }
                    else
                    {
                        grdgenfinalset.Rows[i].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        grdgenfinalset.Rows[i].Cells[1].HorizontalAlign = HorizontalAlign.Right;
                        grdgenfinalset.Rows[i].Cells[2].HorizontalAlign = HorizontalAlign.Left;
                        grdgenfinalset.Rows[i].Cells[3].HorizontalAlign = HorizontalAlign.Right;


                    }
                }
                for (int l = 0; l < grdgenfinalset.Rows.Count; l++)
                {
                    foreach (GridViewRow row in grdgenfinalset.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            grdgenfinalset.Rows[l].Cells[0].Width = 150;
                            grdgenfinalset.Rows[l].Cells[1].Width = 150;
                            grdgenfinalset.Rows[l].Cells[2].Width = 150;
                            grdgenfinalset.Rows[l].Cells[3].Width = 150;
                        }
                    }
                }
            }

            popwindow.Visible = true;




        }
        catch (Exception ex)
        {

        }
    }

    protected void grdgenfinalset_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = e.Row.Cells[0].Text.Replace(",", "</br>").Replace(",", ",</br>");
            e.Row.Cells[2].Text = e.Row.Cells[2].Text.Replace(",", "</br>").Replace(",", ",</br>");
        }
    }
    protected void OnRowCreated_finalsettlement(object sender, GridViewRowEventArgs e)
    {

    }
    protected void SelectedIndexChanged_finalsettlement(Object sender, EventArgs e)
    {


    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }
    private static void AddTableColumn(DataTable resultsTable, StringBuilder ColumnName)
    {
        try
        {
            DataColumn tableCol = new DataColumn(ColumnName.ToString());
            resultsTable.Columns.Add(tableCol);
        }
        catch (System.Data.DuplicateNameException)
        {
            ColumnName.Append(" ");
            AddTableColumn(resultsTable, ColumnName);
        }
    }

    public static string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "Zero";
        if (number < 0)
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        if ((number / 10000000) > 0)
        {
            if (ConvertNumbertoWords(number / 10000000).Trim().ToUpper() == "ONE")
                words += ConvertNumbertoWords(number / 10000000) + " Crore ";
            else
                words += ConvertNumbertoWords(number / 10000000) + " Crores ";
            number %= 10000000;
        }
        if ((number / 100000) > 0)
        {
            if (ConvertNumbertoWords(number / 100000).Trim().ToUpper() == "ONE")
                words += ConvertNumbertoWords(number / 100000) + " Lakh ";
            else
                words += ConvertNumbertoWords(number / 100000) + " Lakhs ";
            number %= 100000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " Thousand ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + "  Hundred ";
            number %= 100;
        }
        if (number > 0)
        {
            if (words != "")
                words += "And ";
            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }
        return words;
    }
    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol1.Visible = false;
            string reportname = txtexcelname1.Text;
            if (reportname.ToString().Trim() != "")
            {
                if (grdgenfinalset.Visible == true)
                {

                    d2.printexcelreportgrid(grdgenfinalset, reportname);
                }

                lbl_norec1.Visible = false;
            }
            else
            {
                lbl_norec1.Text = "Please Enter Your Report Name";
                lbl_norec1.Visible = true;
                txtexcelname1.Focus();
            }
        }
        catch (Exception ex)
        {

        }
    }

    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    { }

}