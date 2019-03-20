using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using FarPoint.Web.Spread;
using InsproDataAccess;

public partial class staffleavereport2 : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    Boolean cellflag = false;

    Hashtable totalleave = new Hashtable();
    static DataSet staffdetails_ds = new DataSet();
    string strstaffcode = "";
    string q1 = "";
    string staffdept = "";
    int monthcnt;
    static int seatcnt = 0;
    static int bloodcnt = 0;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string dateto;
    ArrayList arrColHdrNames = new ArrayList();
    ArrayList arrColHdrNames2 = new ArrayList();
    DataTable dtReport = new DataTable();
    DataRow drowGrd = null;
    InsproDirectAccess dir = new InsproDirectAccess();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //if (Session["collegecode"] == null)
            //{
            //    Response.Redirect("~/Default.aspx");
            //}
            lblvalidation1.Visible = false;
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            //if (Session["collegecode"] == null) //Aruna For Back Button
            //{
            //    Response.Redirect("~/Default.aspx");
            //}
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
            strstaffcode = "" + Session["Staff_Code"].ToString();
            staffdetails_ds.Clear();
            staffdetails_ds.Reset();
            if (staffdetails_ds.Tables.Count == 0)
            {
                q1 = "  SELECT M.Staff_Code,Staff_Name,p.Dept_Name,g.Desig_Name,Category_Name, P.dept_acronym,c.category_code,p.dept_code,ap.appl_id  FROM StaffMaster M,StaffTrans T,HrDept_Master P,Desig_Master G,StaffCategorizer C,staff_appl_master ap Where ap.appl_no=m.appl_no and m.staff_code = t.staff_code  AND T.Dept_Code = P.Dept_Code AND M.College_Code = P.College_Code AND T.Desig_Code = G.Desig_Code AND M.College_Code = G.CollegeCode AND T.Category_Code = C.Category_Code AND M.College_Code = C.College_Code AND M.College_Code ='" + collegecode1 + "' AND T.Latestrec = 1 AND ((M.Resign=0 AND M.Settled=0) and (M.Discontinue =0 or M.Discontinue is null)) ORDER BY p.Dept_Name,t.stftype desc,g.print_pri desc,g.priority";
                staffdetails_ds.Clear();
                staffdetails_ds = d2.select_method_wo_parameter(q1, "text");
            }
            if (!IsPostBack)
            {

                lblto.Visible = false;
                Txtentryto.Visible = false;
                lblda.Visible = false;
                Txtentryfrom.Visible = false;
                string fdate = d2.GetFunction("select top 1 convert(nvarchar(15),From_Date,103) as fdate from hrpaymonths where college_code='" + Session["collegecode"].ToString() + "' order by From_Date");
                string tdate = d2.GetFunction("select top 1 convert(nvarchar(15),To_Date,103) as tdate from hrpaymonths where college_code='" + Session["collegecode"].ToString() + "' order by To_Date desc");
                Txtentryfrom.Text = fdate;
                Txtentryto.Text = tdate;
                load_leavetype();
                load_year();
                FpSpreadvisiblefalse();
                if (strstaffcode == "")
                {
                    load_dept();
                    load_category();
                    load_staffname(staffdept);
                    tbseattype.Enabled = true;
                    tbblood.Enabled = true;
                    cbostaffname.Enabled = true;
                }
                else
                {
                    tbseattype.Enabled = false;
                    tbblood.Enabled = false;
                    cbostaffname.Enabled = false;
                    btngo_Click(sender, e);
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    void load_category()
    {
        try
        {
            cblcategory.Visible = true;
            cblcategory.Items.Clear();
            q1 = "select distinct category_code,category_name from staffcategorizer where college_code='" + Session["collegecode"].ToString() + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(q1, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblcategory.DataSource = ds.Tables[0];
                cblcategory.DataTextField = "category_name";
                cblcategory.DataValueField = "category_code";
                cblcategory.DataBind();
            }
            // load_staffname(staffdept);delsi05022019
        }
        catch
        { }
    }
    void load_leavetype()
    {
        try
        {
            //cblleavetype.Visible = true;
            //ds.Clear();
            //q1 = " Select category from leave_category where college_code='" + Session["collegecode"].ToString() + "'";
            //ds = d2.select_method_wo_parameter(q1, "text");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    cblleavetype.DataSource = ds.Tables[0];
            //    cblleavetype.DataTextField = "category";
            //    cblleavetype.DataValueField = "category";
            //    cblleavetype.DataBind();
            //    cblleavetype.Items.Insert(0, "All");
            //    //cblleavetype.Items.Insert(1, "ABSENT");
            //    //cblleavetype.Items.Insert(1, "PERMISSION");
            //    //cblleavetype.Items.Insert(1, "LATE");
            //}
            if (chk_includeprevious.Checked == false)
            {
                ds.Dispose();
                ds.Reset();
                string strleave = "select distinct category,LeaveMasterPK from leave_category where college_code=" + Session["collegecode"].ToString() + "";
                ds = d2.select_method_wo_parameter(strleave, "text");
                chklsleave.DataSource = ds;
                chklsleave.DataTextField = "category";
                chklsleave.DataValueField = "LeaveMasterPK";

                chklsleave.DataBind();

                for (int item = 0; item < chklsleave.Items.Count; item++)
                {
                    chklsleave.Items[item].Selected = true;
                }
                if (chklsleave.Items.Count > 0)
                {
                    txtleave.Text = "Leave (" + chklsleave.Items.Count + ")";
                    chkleave.Checked = true;
                }
                else
                {
                    txtleave.Text = "---Select---";
                }
            }
            if (chk_includeprevious.Checked == true)
            {
                ds.Dispose();
                ds.Reset();
                string strleave = "select distinct category,LeaveMasterPK from leave_category where college_code=" + Session["collegecode"].ToString() + "";
                ds = d2.select_method_wo_parameter(strleave, "text");
                chklsleave.DataSource = ds;
                chklsleave.DataTextField = "category";
                chklsleave.DataValueField = "LeaveMasterPK";

                chklsleave.DataBind();
                int val = chklsleave.Items.Count;

                chklsleave.Items.Insert(val, "A");
                for (int item = 0; item < chklsleave.Items.Count; item++)
                {
                    chklsleave.Items[item].Selected = true;
                }
                if (chklsleave.Items.Count > 0)
                {
                    txtleave.Text = "Leave (" + chklsleave.Items.Count + ")";
                    chkleave.Checked = true;
                }
                else
                {
                    txtleave.Text = "---Select---";
                }

            }
        }
        catch (Exception ex) { }
    }

    void load_staffname(string staffdept)
    {
        string derpatment = "";
        string staff_cat = "";
        cbostaffname.Items.Clear();
        string staffcat_selected = returnwithsinglecodevalue(cblcategory);
        if (staffdept != "")
        {
            derpatment = "and dept_code in ('" + staffdept + "')";
        }
        if (staffdept == "")
        {
            staffdept = returnwithsinglecodevalue(cbldepttype);
        }
        if (staffdept != "")
        {
            derpatment = "and dept_code in ('" + staffdept + "')";
        }
        if (staffcat_selected != "" && staffdept != "")
        {
            staff_cat = "and t.category_code in('" + staffcat_selected + "')";
        }
        ds.Clear();
        q1 = " Select distinct m.Staff_code,Staff_name from staffmaster m,stafftrans t where resign=0 and settled=0 and m.staff_code = t.staff_code " + staff_cat + " and t.latestrec = 1 " + derpatment + " and college_code='" + Session["collegecode"].ToString() + "' order by staff_name ";
        ds = d2.select_method_wo_parameter(q1, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbostaffname.DataSource = ds;
            cbostaffname.DataTextField = "Staff_name";
            cbostaffname.DataValueField = "Staff_code";
            cbostaffname.DataBind();
            cbostaffname.Items.Insert(0, "All");
        }
    }
    public void load_year()
    {
        q1 = " select year(min(From_Date)) as startyear,year(max(To_Date)) as endyear from HrPayMonths where College_Code='" + Session["collegecode"].ToString() + "' and year(From_Date)<>'' and year(From_Date) is not null and Year(To_Date)<>'' and Year(To_Date) is not null";
        ds.Clear();
        ds = d2.select_method_wo_parameter(q1, "text");
        int end_year = 0;
        int startyear = 0;
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            int end_cnt = ds.Tables[0].Rows.Count;
            Int32.TryParse(ds.Tables[0].Rows[end_cnt - 1]["endyear"].ToString(), out end_year);
            Int32.TryParse(ds.Tables[0].Rows[end_cnt - 1]["startyear"].ToString(), out startyear);
            for (int i = startyear; i <= end_year; i++)
            {
                if (startyear <= end_year)
                {
                    ddlyear.Items.Add(Convert.ToString(startyear));
                    startyear++;
                }
            }
        }
        else
        {
            lblnorec.Visible = true;
            lblnorec.Text = "Please Update the HR Year!";
        }
    }
    void load_dept()
    {
        cbldepttype.Visible = true;
        cbldepttype.Items.Clear();
        ds.Clear();
        string deptquery = "";
        string singleuser = Session["single_user"].ToString();
        if (singleuser == "True")
        {
            deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"].ToString() + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"].ToString() + "') order by dept_name";
        }
        else
        {
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"] + "') order by dept_name ";
        }
        if (deptquery != "")
        {
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbldepttype.DataSource = ds.Tables[0];
                cbldepttype.DataTextField = "dept_name";
                cbldepttype.DataValueField = "dept_code";
                cbldepttype.DataBind();
            }
        }
    }
    protected void ddlyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpreadvisiblefalse();
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void chkmonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpreadvisiblefalse();
            pmonth.Focus();
            int monthcount = 0;
            string value = "";
            string code = "";
            for (int i = 0; i < chkmonth.Items.Count; i++)
            {
                if (chkmonth.Items[i].Selected == true)
                {
                    value = chkmonth.Items[i].Text;
                    code = chkmonth.Items[i].Value.ToString();
                    monthcount = monthcount + 1;
                    txtmonth.Text = "Month(" + monthcount.ToString() + ")";
                }
            }
            if (monthcount == 0)
                txtmonth.Text = "---Select---";
            else
            {
            }
            monthcnt = monthcount;
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected string returnwithsinglecodevalue(CheckBoxList cb)
    {
        string empty = "";
        for (int i = 0; i < cb.Items.Count; i++)
        {
            if (cb.Items[i].Selected == true)
            {
                if (empty == "")
                {
                    empty = Convert.ToString(cb.Items[i].Value);
                }
                else
                {
                    empty = empty + "','" + Convert.ToString(cb.Items[i].Value);
                }
            }
        }
        return empty;
    }
    protected void Txtentryto_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpreadvisiblefalse();
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void chkselect_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpreadvisiblefalse();
            CallCheckboxChange(chkselect, cbldepttype, tbseattype, lbldept.Text, "---Select---");
            staffdept = "";
            load_staffname(staffdept);
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void cbldepttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        //CallCheckboxListChange(chkselect, cbldepttype, tbseattype, lbldept.Text);
        try
        {
            pseattype.Focus();
            int seatcount = 0;
            string value = "";
            string code = "";
            FpSpreadvisiblefalse();
            for (int i = 0; i < cbldepttype.Items.Count; i++)
            {
                if (cbldepttype.Items[i].Selected == true)
                {
                    value = cbldepttype.Items[i].Text;
                    code = cbldepttype.Items[i].Value.ToString();
                    seatcount = seatcount + 1;
                    tbseattype.Text = "Department(" + seatcount.ToString() + ")";
                    if ((staffdept == ""))
                    {
                        staffdept = cbldepttype.Items[i].Value.ToString();
                    }
                    else
                    {
                        staffdept = staffdept + "','" + cbldepttype.Items[i].Value.ToString();
                    }
                }
            }
            if (seatcount == 0)
                tbseattype.Text = "---Select---";
            else
            {
            }
            seatcnt = seatcount;
            load_staffname(staffdept);
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void chkcategory_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpreadvisiblefalse();
            CallCheckboxChange(chkcategory, cblcategory, tbblood, "Category", "---Select---");
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void cblcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int bloodcount = 0;
            string value = "";
            string code = "";
            FpSpreadvisiblefalse();
            for (int i = 0; i < cblcategory.Items.Count; i++)
            {
                if (cblcategory.Items[i].Selected == true)
                {
                    value = cblcategory.Items[i].Text;

                    code = cblcategory.Items[i].Value.ToString();
                    bloodcount = bloodcount + 1;
                    tbblood.Text = "Category(" + bloodcount.ToString() + ")";
                }
            }
            if (bloodcount == 0)
            {
                tbblood.Text = "---Select---";
            }
            else
            {
            }
            bloodcnt = bloodcount;
            load_staffname(staffdept);
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    public void FpSpreadvisiblefalse()
    {
        //fpsalary.Visible = false;
        rptprint.Visible = false;
    }
    protected void cblleavetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpreadvisiblefalse();
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void cbostaffname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbostaffname.Items.Count == 0)
            {
                cbostaffname.Items.Insert(0, "---Select---");
            }
            FpSpreadvisiblefalse();
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void rdoyearlywise_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            chkstaffleave.Checked = false;
            chk_includeprevious.Checked = false;
            chk_includeoverall.Visible=false;
            if (rdoyearlywise.Checked == true)
            {
                lblda.Text = "Date";
                Txtentryfrom.Visible = true;
                Txtentryfrom.Enabled = false;
                lblto.Visible = true;
                Txtentryto.Visible = true;
                txtmonth.Visible = false;
                chkmonth.Visible = false;
                FpSpreadvisiblefalse();
                pmonth.Visible = false;
                Label13.Visible = true;
                //cblleavetype.Visible = true;
                lblyear.Visible = false;
                ddlyear.Visible = false;
                chkstaffleave.Visible = true;
                chk_includeprevious.Visible = true;
                chk_includeoverall.Visible = true;
                lblda.Visible = true;
                lblto.Text = "To";
                lblto.Visible = false;
                Txtentryto.Visible = false;
                lblda.Visible = false;
                Txtentryfrom.Visible = false;
                string fdate = d2.GetFunction("select top 1 convert(nvarchar(15),From_Date,103) as fdate from hrpaymonths where college_code='" + Session["collegecode"].ToString() + "' order by From_Date");
                string tdate = d2.GetFunction("select top 1 convert(nvarchar(15),To_Date,103) as tdate from hrpaymonths where college_code='" + Session["collegecode"].ToString() + "' order by To_Date desc");
                Txtentryfrom.Text = fdate;
                Txtentryto.Text = tdate;
            }
            else if (rdomonthlywise.Checked == true)
            {
                lblda.Text = "Month";
                Txtentryfrom.Visible = false;
                Txtentryto.Visible = false;
                chkstaffleave.Visible = true;
                chk_includeprevious.Visible = true;
                chk_includeoverall.Visible = true;
            }
            else
            {
                lblda.Visible = false;
                Txtentryfrom.Visible = false;
                lblto.Visible = true;
                Txtentryto.Visible = true;
                lblto.Text = "Date";
                txtmonth.Visible = false;
                chkmonth.Visible = false;
                pmonth.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst)
    {
        try
        {
            int sel = 0;
            int count = 0;
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = dipst + "(" + count + ")";
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
            else
            {
                txt.Text = "--Select--";
            }
        }
        catch { }
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
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
    protected void rdomonthlywise_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpreadvisiblefalse();
            chkstaffleave.Checked = false;
            chk_includeprevious.Checked = false;
            chk_includeoverall.Visible = false;
            if (rdomonthlywise.Checked == true)
            {
                lblda.Text = "Month";
                Txtentryfrom.Visible = false;
                Txtentryto.Visible = false;
                lblto.Visible = false;
                txtmonth.Visible = true;
                chkmonth.Visible = true;
                pmonth.Visible = true;
                Label13.Visible = true;
                //cblleavetype.Visible = true;
                lblda.Visible = true;
                lblyear.Visible = true;
                ddlyear.Visible = true;
                chkstaffleave.Visible = true;
                chk_includeprevious.Visible = true;
                chk_includeoverall.Visible = true;
            }
            else if (rdoyearlywise.Checked == true)
            {
            }
            else
            {
                lblda.Text = "Date";
                Txtentryfrom.Visible = true;
                lblto.Visible = false;
                Txtentryfrom.Enabled = true;
                txtmonth.Visible = false;
                chkmonth.Visible = false;
                pmonth.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void rdodaywise_CheckedChanged(object sender, EventArgs e)
    {
        FpSpreadvisiblefalse();
        chkstaffleave.Checked = false;
        chk_includeprevious.Checked = false;
        chk_includeoverall.Checked = false;
        if (rdoyearlywise.Checked == true)
        {
            lblda.Text = "Date";
            Txtentryfrom.Visible = true;
            Txtentryfrom.Enabled = true;
            lblto.Visible = true;
            Txtentryto.Visible = true;
            txtmonth.Visible = false;
            chkmonth.Visible = false;
            txtmonth.Visible = false;
            chkmonth.Visible = false;
            pmonth.Visible = false;
            Label13.Visible = true;
            //cblleavetype.Visible = true;
            txtmonth.Visible = false;
        }
        else if (rdomonthlywise.Checked == true)
        {
            lblda.Text = "Month";
            Txtentryfrom.Visible = false;
            Txtentryto.Visible = false;
            Label13.Visible = true;
            //cblleavetype.Visible = true;
        }
        else
        {
            lblda.Visible = false;
            Txtentryfrom.Visible = false;
            lblto.Visible = true;
            Txtentryto.Visible = true;
            lblto.Text = "Date";
            txtmonth.Visible = false;
            chkmonth.Visible = false;
            pmonth.Visible = false;
            Label13.Visible = true;
            // cblleavetype.Visible = true;
            Label13.Visible = true;
            // cblleavetype.Visible = true;
            lblyear.Visible = false;
            ddlyear.Visible = false;
            chkstaffleave.Visible = false;
            chk_includeprevious.Visible = false;
            chk_includeoverall.Visible = false;
        }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            lblnorec.Visible = false;
            load_btnclick();
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
            d2.sendErrorMail(ex, Convert.ToString(Session["collegecode"]), "staffleavereport.aspx");
        }
    }
    protected void load_btnclick()
    {
        try
        {
            if (chk_includeprevious.Checked == false)
            {

                Hashtable leavetypehash = new Hashtable();
                Hashtable totalbind = new Hashtable();

                arrColHdrNames.Add("S.No");
                arrColHdrNames2.Add("S.No");
                dtReport.Columns.Add("S.No");


                arrColHdrNames.Add("Staff Name");
                arrColHdrNames2.Add("Staff Name");
                dtReport.Columns.Add("Staff Name");

                arrColHdrNames.Add("Staff Code");
                arrColHdrNames2.Add("Staff Code");
                dtReport.Columns.Add("Staff Code");

                arrColHdrNames.Add("Dept Acronym");
                arrColHdrNames2.Add("Dept Acronym");
                dtReport.Columns.Add("Dept Acronym");

                arrColHdrNames.Add("Designation");
                arrColHdrNames2.Add("Designation");
                dtReport.Columns.Add("Designation");

                arrColHdrNames.Add("Category Name");
                arrColHdrNames2.Add("Category Name");
                dtReport.Columns.Add("Category Name");

                arrColHdrNames.Add("Appl Id");
                arrColHdrNames2.Add("Appl Id");
                dtReport.Columns.Add("Appl Id");

                arrColHdrNames.Add("Month");
                arrColHdrNames2.Add("Month");
                dtReport.Columns.Add("Month");

                string query = "";
                if (!String.IsNullOrEmpty(strstaffcode))
                {
                    query = "select staff_code,leavetype,category_code,college_code,permission,MaxEarnLeave from individual_leave_type where college_code='" + collegecode1 + "' and staff_code='" + strstaffcode + "'";
                    query += " Select category,shortname,LeaveMasterPK  from leave_category where college_code='" + collegecode1 + "' ";
                    query += " select * from hrpaymonths where college_code='" + collegecode1 + "' and SelStatus='1'";
                }
                else
                {
                    query = "select staff_code,leavetype,category_code,college_code,permission,MaxEarnLeave from individual_leave_type where college_code='" + collegecode1 + "'";
                    query += " Select category,shortname,LeaveMasterPK  from leave_category where college_code='" + collegecode1 + "' ";
                    query += " select * from hrpaymonths where college_code='" + collegecode1 + "' and SelStatus='1'"; //
                }
                ds2.Clear();
                ds2 = d2.select_method_wo_parameter(query, "Text");
                if (ds2.Tables.Count > 0 && ds2.Tables[2].Rows.Count > 0)
                {
                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        int h = 7;
                        for (int i = 0; i < chklsleave.Items.Count; i++)
                        {

                            if (chklsleave.Items[i].Selected == true)
                            {
                                h++;
                                string getLeaveType = Convert.ToString(chklsleave.Items[i].Value);
                                string shortname = d2.GetFunction("Select shortname from leave_category where college_code='" + collegecode1 + "' and LeaveMasterPK='" + getLeaveType + "'");

                                arrColHdrNames.Add("Leave Type");
                                arrColHdrNames2.Add(Convert.ToString(shortname));
                                dtReport.Columns.Add(chklsleave.Items[i].Value);

                                if (!leavetypehash.Contains(Convert.ToString(chklsleave.Items[i].Text)))
                                {

                                    leavetypehash.Add(Convert.ToString(chklsleave.Items[i].Text), h);

                                }
                            }

                        }

                    }
                    DataRow drHdr1 = dtReport.NewRow();
                    DataRow drHdr2 = dtReport.NewRow();
                    for (int grCol = 0; grCol < dtReport.Columns.Count; grCol++)
                    {
                        drHdr1[grCol] = arrColHdrNames[grCol];
                        drHdr2[grCol] = arrColHdrNames2[grCol];
                    }
                    dtReport.Rows.Add(drHdr1);
                    dtReport.Rows.Add(drHdr2);
                    string deptcode = returnwithsinglecodevalue(cbldepttype);
                    string catagorycode = returnwithsinglecodevalue(cblcategory);
                    string leavetype = string.Empty; //Convert.ToString(cblleavetype.SelectedValue);
                    string staffcodemul = Convert.ToString(cbostaffname.SelectedValue);
                    string filter = "";
                    string staffcode = "";
                    string Appl_ID = "";
                    string category_code = "";
                    bool leavedayscheckcount = false;
                    if (staffdetails_ds.Tables[0].Rows.Count > 0)
                    {
                        if (String.IsNullOrEmpty(strstaffcode))
                        {
                            if (deptcode.Trim().ToLower() != "")
                            {
                                filter = " dept_code in ('" + deptcode + "')";
                            }
                            if (catagorycode.Trim().ToLower() != "")
                            {
                                if (filter.Trim() != "")
                                    filter += " and";
                                filter += "  category_code in ('" + catagorycode + "')";
                            }
                            if (staffcodemul.Trim().ToLower() != "all")
                            {
                                if (filter.Trim() != "")
                                    filter += " and";
                                filter += "  Staff_Code in ('" + staffcodemul + "')";
                            }
                        }
                        else
                        {
                            if (filter.Trim() != "")
                                filter += " and";
                            filter += "  Staff_Code='" + strstaffcode + "'";
                        }
                        if (leavetype.Trim().ToLower() != "all")
                        {

                        }
                        bool leavesingle = false;
                        staffdetails_ds.Tables[0].DefaultView.RowFilter = filter;
                        DataView staffdetails_dv = staffdetails_ds.Tables[0].DefaultView;

                        if (staffdetails_dv.Count > 0)
                        {
                            for (int i = 0; i < staffdetails_dv.Count; i++)
                            {
                                Hashtable totalvalue_dic = new Hashtable();
                                drowGrd = dtReport.NewRow();
                                drowGrd[0] = Convert.ToString(i + 1);
                                drowGrd[1] = Convert.ToString(staffdetails_dv[i]["Staff_Name"]);
                                drowGrd[2] = Convert.ToString(staffdetails_dv[i]["Staff_Code"]);
                                drowGrd[3] = Convert.ToString(staffdetails_dv[i]["dept_acronym"]);//Dept_Name

                                drowGrd[4] = Convert.ToString(staffdetails_dv[i]["Desig_Name"]);
                                drowGrd[5] = Convert.ToString(staffdetails_dv[i]["Category_Name"]);
                                drowGrd[6] = Convert.ToString(staffdetails_dv[i]["Appl_ID"]);
                                staffcode = Convert.ToString(staffdetails_dv[i]["Staff_Code"]);
                                Appl_ID = Convert.ToString(staffdetails_dv[i]["Appl_ID"]);
                                category_code = Convert.ToString(staffdetails_dv[i]["category_code"]);


                                //paymonth
                                if (ds2.Tables[2].Rows.Count > 0)
                                {
                                    DataView ds2table2_dv = new DataView();
                                    string ds2tablefilter = "";
                                    if (rdomonthlywise.Checked == true)
                                    {
                                        string selectedmonth = returnwithsinglecodevalue(chkmonth);
                                        if (String.IsNullOrEmpty(selectedmonth.Trim()))
                                        {
                                            lblnorec.Visible = true;
                                            lblnorec.Text = "Please Select Month!";
                                            // fpsalary.Visible = false;
                                            rptprint.Visible = false;
                                            return;
                                        }
                                        ds2tablefilter = " PayMonthNum in('" + selectedmonth + "') and PayYear in('" + ddlyear.SelectedItem.Value.ToString() + "')";
                                    }

                                    if (rdodaywise.Checked == true)
                                    {
                                        string month = ""; string year = "";
                                        string date2 = Txtentryto.Text.ToString();
                                        string[] split1 = date2.Split(new Char[] { '/' });
                                        dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                                        month = split1[1].TrimStart('0').ToString();
                                        year = split1[2].ToString();

                                        ds2tablefilter = " PayMonthNum in('" + month + "') and PayYear in('" + year + "')";

                                    }
                                    ds2.Tables[2].DefaultView.RowFilter = ds2tablefilter;
                                    ds2table2_dv = ds2.Tables[2].DefaultView;
                                    DataTable ds2table2_dt = ds2table2_dv.ToTable();


                                    if (ds2table2_dt.Rows.Count > 0)
                                    {
                                        for (int p = 0; p < ds2table2_dt.Rows.Count; p++)
                                        {
                                            if (p != 0)
                                            {
                                                drowGrd = dtReport.NewRow();
                                            }
                                            drowGrd[0] = Convert.ToString(i + 1);
                                            drowGrd[1] = Convert.ToString(staffdetails_dv[i]["Staff_Name"]);
                                            drowGrd[2] = Convert.ToString(staffdetails_dv[i]["Staff_Code"]);

                                            drowGrd[3] = Convert.ToString(staffdetails_dv[i]["dept_acronym"]);//Dept_Name

                                            drowGrd[4] = Convert.ToString(staffdetails_dv[i]["Desig_Name"]);
                                            drowGrd[5] = Convert.ToString(staffdetails_dv[i]["Category_Name"]);
                                            drowGrd[6] = Convert.ToString(staffdetails_dv[i]["Appl_ID"]);
                                            drowGrd[7] = Convert.ToString(ds2table2_dt.Rows[p]["PayMonth"]) + "-" + Convert.ToString(ds2table2_dt.Rows[p]["Payyear"]);
                                            //fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds2table2_dt.Rows[p]["PayMonth"]) + "-" + Convert.ToString(ds2table2_dt.Rows[p]["Payyear"]);
                                            //fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds2table2_dt.Rows[p]["PayMonthNum"]);
                                            //fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 5].Note = Convert.ToString(ds2table2_dt.Rows[p]["Payyear"]);

                                            double llcount = 0;
                                            totalleave.Clear();
                                            totalbind.Clear();
                                            double addtot = 0;
                                            string actual = "";
                                            double tot_leave = 0;
                                            string leavefromdate = "";
                                            string leavetodate = "";
                                            string ishalfdate = "";
                                            string halfdaydate = "";
                                            int finaldate = 0;
                                            string sleave = "";
                                            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                                            {
                                                ds2.Tables[0].DefaultView.RowFilter = " Staff_Code= '" + staffcode + "' and category_code='" + category_code + "'";
                                                DataView ds2table0_dv = ds2.Tables[0].DefaultView;
                                                if (ds2table0_dv.Count > 0)
                                                {
                                                    string[] spl_type = ds2table0_dv[0]["leavetype"].ToString().Split(new Char[] { '\\' });
                                                    //  for (int k = 0; k < ds2table0_dv.Count; k++)
                                                    //{
                                                    int col = 6;
                                                    for (int l = 0; spl_type.GetUpperBound(0) >= l; l++)
                                                    {
                                                        string leave = "";
                                                        if (spl_type[l].Trim() != "")
                                                        {
                                                            col++;
                                                            tot_leave = 0;
                                                            string[] split_leave = spl_type[l].Split(';');
                                                            leave = split_leave[0];
                                                            if (split_leave.Length >= 2)
                                                            {
                                                                double.TryParse(Convert.ToString(split_leave[1]), out addtot);
                                                            }

                                                            string leavepk = "";
                                                            ds2.Tables[1].DefaultView.RowFilter = " category='" + leave + "'";
                                                            DataView leavepk_dv = ds2.Tables[1].DefaultView;
                                                            if (leavepk_dv.Count > 0)
                                                            {
                                                                leavepk = Convert.ToString(leavepk_dv[0]["LeaveMasterPK"]);
                                                            }//delsi2201
                                                            for (int dtcol = 8; dtcol < dtReport.Columns.Count; dtcol++)
                                                            {
                                                                string getpk = Convert.ToString(dtReport.Columns[dtcol]);
                                                                if (leavepk == getpk)
                                                                {
                                                                    ds2.Tables[2].DefaultView.RowFilter = " PayMonthNum='" + Convert.ToString(ds2table2_dt.Rows[p]["PayMonthNum"]) + "' and PayYear ='" + Convert.ToString(ds2table2_dt.Rows[p]["Payyear"]) + "'";
                                                                    DataView Lfromlto_dv = ds2.Tables[2].DefaultView;
                                                                    if (Lfromlto_dv.Count > 0)
                                                                    {
                                                                        string dt_get_leave = string.Empty;
                                                                        if (rdodaywise.Checked == true)
                                                                        {
                                                                            string month = ""; string year = "";
                                                                            string date2 = Txtentryto.Text.ToString();
                                                                            string[] split1 = date2.Split(new Char[] { '/' });
                                                                            dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                                                                            month = split1[1].TrimStart('0').ToString();
                                                                            year = split1[2].ToString();

                                                                            dt_get_leave = "select * from RQ_Requisition r,leave_category l where RequestType=5 and LeaveFrom='" + Convert.ToString(dateto) + "' and LeaveTo='" + Convert.ToString(dateto) + "' and ReqAppStatus='1' and l.LeaveMasterPK=r.LeaveMasterFK and r.LeaveMasterFK='" + leavepk + "' and ReqAppNo='" + Appl_ID + "'  ";
                                                                        }
                                                                        else
                                                                        {
                                                                            //dt_get_leave = "select * from RQ_Requisition r,leave_category l where RequestType=5 and LeaveFrom>='" + Lfromlto_dv[k]["From_Date"].ToString() + "' and LeaveTo<='" + Lfromlto_dv[k]["To_Date"].ToString() + "' and ReqAppStatus='1' and l.LeaveMasterPK=r.LeaveMasterFK and r.LeaveMasterFK='" + leavepk + "' and ReqAppNo='" + Appl_ID + "'  ";

                                                                            dt_get_leave = "select * from RQ_Requisition r,leave_category l where RequestType=5 and (LeaveFrom>='" + Lfromlto_dv[0]["From_Date"].ToString() + "' and LeaveTo<='" + Lfromlto_dv[0]["To_Date"].ToString() + "' or LeaveFrom between'" + Lfromlto_dv[0]["From_Date"].ToString() + "' and '" + Lfromlto_dv[0]["To_Date"].ToString() + "') and ReqAppStatus='1' and l.LeaveMasterPK=r.LeaveMasterFK and r.LeaveMasterFK='" + leavepk + "' and ReqAppNo='" + Appl_ID + "'  ";//delsi 0501


                                                                        }
                                                                        ds1 = d2.select_method_wo_parameter(dt_get_leave, "Text");
                                                                        if (ds1.Tables[0].Rows.Count > 0)//delsi0314
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
                                                                                    llcount = 0;
                                                                                    if (fromdate != todate)
                                                                                    {
                                                                                        for (; fromdate <= todate; )
                                                                                        {
                                                                                            string dayy = fromdate.ToString("dddd");
                                                                                            leavedayscheckcount = false;
                                                                                            if (dayy == "Sunday")
                                                                                            {
                                                                                                if (split_leave[3] == "0")
                                                                                                    leavedayscheckcount = true;
                                                                                                else
                                                                                                    leavedayscheckcount = false;
                                                                                            }
                                                                                            if (leavedayscheckcount == false)
                                                                                            {
                                                                                                llcount++;
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
                                                                                        sleave = leave + "-" + tot_leave;
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
                                                                                        sleave = leave + "-" + tot_leave;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        if (spl_type[l].Contains(";"))
                                                                        {
                                                                            string sp = split_leave[0].ToString();
                                                                            actual = split_leave[2].ToString();
                                                                            if (actual == "")
                                                                            {
                                                                                actual = "0";
                                                                            }
                                                                            string[] iii = sleave.Split('-');
                                                                            string sp1 = iii[0];
                                                                            if (sp != sp1)
                                                                            {
                                                                                tot_leave = 0;
                                                                            }
                                                                            string tt = Convert.ToString(tot_leave + "/" + actual);
                                                                            if (!totalleave.Contains(Convert.ToString(leave)))
                                                                                totalleave.Add(Convert.ToString(leave), Convert.ToString(tt));
                                                                            else
                                                                            {
                                                                                string getvalue = Convert.ToString(totalleave[Convert.ToString(leave)]);
                                                                                if (getvalue.Trim() != "")
                                                                                {
                                                                                    getvalue = getvalue + "," + tt;
                                                                                    totalleave.Remove(Convert.ToString(leave));
                                                                                    if (getvalue.Trim() != "")
                                                                                        totalleave.Add(Convert.ToString(leave), Convert.ToString(getvalue));
                                                                                }
                                                                            }
                                                                            int colcount = Convert.ToInt32(leavetypehash[leave]);
                                                                            if (colcount != 0)
                                                                            {
                                                                                // poomalar 06.12.17                                                                            
                                                                                #region for table merge
                                                                                string fistcasual = "HalfDay@fh@" + leave + ""; double monthcount = 0; // poo
                                                                                string secondcasual = "HalfDay@sh@" + leave + ""; //poo
                                                                                // string paymonth = Convert.ToString(fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 5].Tag);
                                                                                string dbyear = Convert.ToString(ds2.Tables[2].Rows[p]["payyear"]);
                                                                                if (rdoyearlywise.Checked == true)
                                                                                {

                                                                                    //   string sqlpaymonth = "select sum(no_days) leave,month(fdate) month,year(fdate) year from staff_leave_details where (lt_taken ='" + leave + "' or lt_taken ='" + secondcasual + "' or lt_taken ='" + fistcasual + "') and staff_code='" + staffcode + "' and apply_approve=1 and college_code ='" + Session["collegecode"] + "'  and fdate >=   (SELECT top 1 CONVERT(VARCHAR(2),DATEPART(MONTH, From_Date)) + '/'+ CONVERT(VARCHAR(2),DATEPART(DAY, From_Date)) + '/' + CONVERT(VARCHAR(4),DATEPART(YEAR, From_Date)) from hrpaymonths order by From_Date) and tdate <= (SELECT top 1 CONVERT(VARCHAR(2),DATEPART(MONTH, To_Date)) + '/'+ CONVERT(VARCHAR(2),DATEPART(DAY, To_Date)) + '/' + CONVERT(VARCHAR(4),DATEPART(YEAR, To_Date)) from hrpaymonths order by To_Date DESC) group by month(fdate),year(fdate)";

                                                                                    string sqlpaymonth = "select sum(no_days) leave from staff_leave_details where (lt_taken ='" + leave + "' or lt_taken ='" + secondcasual + "' or lt_taken ='" + fistcasual + "') and staff_code='" + staffcode + "' and apply_approve=1 and college_code ='" + Session["collegecode"] + "' and fdate>='" + Lfromlto_dv[0]["From_Date"].ToString() + "' and tdate<='" + Lfromlto_dv[0]["To_Date"].ToString() + "'  and fdate >=   (SELECT top 1 CONVERT(VARCHAR(2),DATEPART(MONTH, From_Date)) + '/'+ CONVERT(VARCHAR(2),DATEPART(DAY, From_Date)) + '/' + CONVERT(VARCHAR(4),DATEPART(YEAR, From_Date)) from hrpaymonths order by From_Date) and tdate <= (SELECT top 1 CONVERT(VARCHAR(2),DATEPART(MONTH, To_Date)) + '/'+ CONVERT(VARCHAR(2),DATEPART(DAY, To_Date)) + '/' + CONVERT(VARCHAR(4),DATEPART(YEAR, To_Date)) from hrpaymonths order by To_Date DESC)";

                                                                                    DataSet dspaymonth = new DataSet();
                                                                                    dspaymonth = d2.select_method_wo_parameter(sqlpaymonth, "Text");

                                                                                    if (dspaymonth.Tables[0].Rows.Count > 0)
                                                                                    {
                                                                                        //for (int pay = 0; pay < dspaymonth.Tables[0].Rows.Count; pay++)
                                                                                        //{
                                                                                        //    string curmonth = Convert.ToString(dspaymonth.Tables[0].Rows[pay]["month"]);
                                                                                        //    string curyear = Convert.ToString(dspaymonth.Tables[0].Rows[pay]["year"]);
                                                                                        //    if (dbyear == curyear)
                                                                                        //    {
                                                                                        //        if (paymonth == curmonth)
                                                                                        //        {
                                                                                        //            double.TryParse(Convert.ToString(dspaymonth.Tables[0].Rows[pay]["leave"]), out monthcount);
                                                                                        //            tot_leave += monthcount;
                                                                                        //        }
                                                                                        //    }

                                                                                        //}
                                                                                        double.TryParse(Convert.ToString(dspaymonth.Tables[0].Rows[0]["leave"]), out monthcount);
                                                                                        tot_leave += monthcount;//delsi11/05/2018

                                                                                    }
                                                                                }

                                                                                string sql = string.Empty; string leaveold = string.Empty; double oldleave = 0;
                                                                                if (rdomonthlywise.Checked == true)
                                                                                {

                                                                                    string selectedmonth = returnwithsinglecodevalue(chkmonth);
                                                                                    string monthselec = Convert.ToString(ds2table2_dt.Rows[p]["PayMonthNum"]); ;
                                                                                    if (String.IsNullOrEmpty(selectedmonth.Trim()))
                                                                                    {
                                                                                        lblnorec.Visible = true;
                                                                                        lblnorec.Text = "Please Select Month!";
                                                                                        // fpsalary.Visible = false;
                                                                                        rptprint.Visible = false;
                                                                                        return;
                                                                                    }
                                                                                    sql = "select sum(no_days) from staff_leave_details where (lt_taken ='" + leave + "' or lt_taken ='" + secondcasual + "' or lt_taken ='" + fistcasual + "') and staff_code='" + staffcode + "' and apply_approve=1 and college_code ='" + Session["collegecode"] + "'  and fdate >=   (SELECT top 1 CONVERT(VARCHAR(2),DATEPART(MONTH, From_Date)) + '/'+ CONVERT(VARCHAR(2),DATEPART(DAY, From_Date)) + '/' + CONVERT(VARCHAR(4),DATEPART(YEAR, From_Date)) from hrpaymonths order by From_Date) and tdate <= (SELECT top 1 CONVERT(VARCHAR(2),DATEPART(MONTH, To_Date)) + '/'+ CONVERT(VARCHAR(2),DATEPART(DAY, To_Date)) + '/' + CONVERT(VARCHAR(4),DATEPART(YEAR, To_Date)) from hrpaymonths order by To_Date DESC) and month(staff_leave_details.fdate) in('" + monthselec + "') and month  (staff_leave_details.tdate) in('" + monthselec + "') and  year(staff_leave_details.fdate)='" + ddlyear.SelectedItem.Text + "' and year (staff_leave_details.tdate)='" + ddlyear.SelectedItem.Text + "'"; // poo 06.12.17 //paymonth

                                                                                    DataSet dstab = new DataSet();
                                                                                    dstab = d2.select_method_wo_parameter(sql, "Text");
                                                                                    leaveold = d2.GetFunction(sql); double.TryParse(leaveold, out oldleave);
                                                                                    tot_leave += oldleave;


                                                                                }
                                                                                if (rdodaywise.Checked == true)
                                                                                {
                                                                                    string month = ""; string year = "";
                                                                                    string date2 = Txtentryto.Text.ToString();
                                                                                    string[] split1 = date2.Split(new Char[] { '/' });
                                                                                    dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                                                                                    month = split1[1].TrimStart('0').ToString();
                                                                                    year = split1[2].ToString();
                                                                                    string dayquery = "select sum(no_days) from staff_leave_details where (lt_taken ='" + leave + "' or lt_taken ='" + secondcasual + "' or lt_taken ='" + fistcasual + "') and staff_code='" + staffcode + "' and apply_approve=1 and college_code ='" + Session["collegecode"] + "'  and fdate >=   (SELECT top 1 CONVERT(VARCHAR(2),DATEPART(MONTH, From_Date)) + '/'+ CONVERT(VARCHAR(2),DATEPART(DAY, From_Date)) + '/' + CONVERT(VARCHAR(4),DATEPART(YEAR, From_Date)) from hrpaymonths order by From_Date) and tdate <= (SELECT top 1 CONVERT(VARCHAR(2),DATEPART(MONTH, To_Date)) + '/'+ CONVERT(VARCHAR(2),DATEPART(DAY, To_Date)) + '/' + CONVERT(VARCHAR(4),DATEPART(YEAR, To_Date)) from hrpaymonths order by To_Date DESC) and fdate='" + dateto + "' and tdate='" + dateto + "' ";
                                                                                    DataSet dsday = new DataSet();
                                                                                    dsday = d2.select_method_wo_parameter(dayquery, "Text");
                                                                                    leaveold = d2.GetFunction(dayquery); double.TryParse(leaveold, out oldleave);
                                                                                    tot_leave += oldleave;
                                                                                }

                                                                                //DataTable dstab_dt = dstab_dv.ToTable();


                                                                                #endregion
                                                                                drowGrd[dtcol] = Convert.ToString(tot_leave + "/" + actual);

                                                                                //fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(tot_leave + "/" + actual);
                                                                                //fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colcount].Tag = staffcode;
                                                                                //fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colcount].Note = Appl_ID;
                                                                                //fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colcount].Font.Bold = true;
                                                                                //fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colcount].Font.Name = "Book Antiqua";
                                                                                //fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colcount].Font.Size = FontUnit.Medium;
                                                                                //fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                                                                if (totalvalue_dic.Contains(leave))
                                                                                {
                                                                                    string value = totalvalue_dic[leave].ToString();
                                                                                    string[] leavecount = value.Split('/');
                                                                                    totalvalue_dic.Remove(leave);
                                                                                    double leavecnt = 0;//barath 19.06.17
                                                                                    double.TryParse(leavecount[0], out leavecnt);
                                                                                    //leavecnt = leavecnt+oldleave;
                                                                                    double total = (leavecnt + tot_leave);
                                                                                    //total += oldleave;
                                                                                    //int total = Convert.ToInt32(leavecount[0]) + Convert.ToInt32(tot_leave);                  
                                                                                    totalvalue_dic.Add(leave, total + "/" + addtot);
                                                                                }
                                                                                else
                                                                                {
                                                                                    //tot_leave =tot_leave+ oldleave;
                                                                                    totalvalue_dic.Add(leave, Convert.ToString(tot_leave) + "/" + addtot);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                    }
                                                    //}
                                                }
                                            }
                                            dtReport.Rows.Add(drowGrd);
                                        }
                                        drowGrd = dtReport.NewRow();
                                        drowGrd[7] = "Total";

                                        if (totalvalue_dic.Count > 0)
                                        {
                                            foreach (DictionaryEntry item in totalvalue_dic)
                                            {
                                                string leave_key = Convert.ToString(item.Key);
                                                string Value = Convert.ToString(item.Value);
                                                string[] total = Value.Split('/');
                                                int colcount1 = Convert.ToInt32(leavetypehash[leave_key]);
                                                if (total.Length > 1)
                                                {
                                                    double tot = 0;
                                                    double.TryParse(total[0].ToString(), out tot);
                                                    drowGrd[colcount1] = tot + "/" + total[1].ToString();


                                                }
                                            }
                                        }
                                        dtReport.Rows.Add(drowGrd);
                                        drowGrd = dtReport.NewRow();
                                        drowGrd[7] = "Taken";
                                        if (totalvalue_dic.Count > 0)
                                        {
                                            foreach (DictionaryEntry item in totalvalue_dic)
                                            {
                                                string leave_key = Convert.ToString(item.Key);
                                                string Value = Convert.ToString(item.Value);
                                                string[] total = Value.Split('/');
                                                int colcount1 = Convert.ToInt32(leavetypehash[leave_key]);
                                                if (total.Length > 1)
                                                {
                                                    double tot = 0;
                                                    double.TryParse(total[0].ToString(), out tot);
                                                    drowGrd[colcount1] = Convert.ToString(tot);


                                                }
                                            }
                                        }

                                        dtReport.Rows.Add(drowGrd);
                                        drowGrd = dtReport.NewRow();

                                        if (totalvalue_dic.Count > 0)
                                        {
                                            foreach (DictionaryEntry item in totalvalue_dic)
                                            {
                                                string leave_key = Convert.ToString(item.Key);
                                                string Value = Convert.ToString(item.Value);
                                                string[] total = Value.Split('/');
                                                int colcount1 = Convert.ToInt32(leavetypehash[leave_key]);
                                                if (total.Length > 1)
                                                {
                                                    double tot = 0;
                                                    double.TryParse(total[0].ToString(), out tot);
                                                    drowGrd[colcount1] = Convert.ToString(Convert.ToDouble(total[1]) - Convert.ToDouble(tot));


                                                }
                                            }
                                        }

                                        drowGrd[7] = "Available";
                                        dtReport.Rows.Add(drowGrd);

                                        if (totalvalue_dic.Count > 0)
                                        {
                                            foreach (DictionaryEntry item in totalvalue_dic)
                                            {
                                                string leave_key = Convert.ToString(item.Key);
                                                string Value = Convert.ToString(item.Value);
                                                string[] total = Value.Split('/');
                                                int colcount1 = Convert.ToInt32(leavetypehash[leave_key]);
                                                if (total.Length > 1)
                                                {
                                                    double tot = 0;
                                                    double.TryParse(total[0].ToString(), out tot);

                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            rptprint.Visible = false;
                            grdleave.Visible = false;

                        }
                    }
                    //fpsalary.Sheets[0].PageSize = fpsalary.Sheets[0].RowCount;
                    //fpsalary.Visible = true;
                    rptprint.Visible = true;
                    //lblnorec.Visible = false;
                    grdleave.DataSource = dtReport;
                    grdleave.DataBind();
                    grdleave.Visible = true;

                    GridViewRow row = grdleave.Rows[0];
                    GridViewRow previousRow = grdleave.Rows[1];

                    for (int i = 0; i < dtReport.Columns.Count; i++)
                    {
                        if (row.Cells[i].Text == previousRow.Cells[i].Text)
                        {
                            row.Cells[i].RowSpan = 2;
                            previousRow.Cells[i].Visible = false;
                            //row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                            //                       previousRow.Cells[i].RowSpan + 1;
                            //previousRow.Cells[i].Visible = false;
                        }
                    }

                    for (int cell = grdleave.Rows[0].Cells.Count - 1; cell > 0; cell--)
                    {
                        TableCell colum = grdleave.Rows[0].Cells[cell];
                        TableCell previouscol = grdleave.Rows[0].Cells[cell - 1];
                        if (colum.Text == previouscol.Text)
                        {
                            if (previouscol.ColumnSpan == 0)
                            {
                                if (colum.ColumnSpan == 0)
                                {
                                    previouscol.ColumnSpan += 2;
                                }
                                else
                                {
                                    previouscol.ColumnSpan += colum.ColumnSpan + 1;
                                }
                                colum.Visible = false;
                            }
                        }

                    }
                    for (int i = 0; i < grdleave.Rows.Count; i++)
                    {
                        string gettxt = grdleave.Rows[i].Cells[7].Text;
                        if (gettxt == "Total" || gettxt == "Taken" || gettxt == "Available")
                        {
                            grdleave.Rows[i].BackColor = Color.Bisque;
                            grdleave.Rows[i].ForeColor = Color.IndianRed;
                        }
                    }
                    if (dtReport.Rows.Count > 1)
                    {

                        int dtrowcount = dtReport.Rows.Count;
                        int rowspanstart0 = 0;
                        int rowspanstart1 = 0;
                        int rowspanstart2 = 0;
                        int rowspanstart3 = 0;
                        int rowspanstart4 = 0;
                        int rowspanstart5 = 0;
                        for (int i = 0; i < grdleave.Rows.Count; i++)
                        {

                            int rowspancount0 = 0;
                            int rowspancount1 = 0;
                            int rowspancount2 = 0;
                            int rowspancount3 = 0;
                            int rowspancount4 = 0;
                            int rowspancount5 = 0;


                            if (i != dtrowcount)
                            {
                                if (rowspanstart0 == i)
                                {
                                    if (grdleave.Rows[i].Cells[0].Text != "&nbsp;")
                                    {
                                        for (int k = rowspanstart0 + 1; grdleave.Rows[i].Cells[0].Text == grdleave.Rows[k].Cells[0].Text; k++)
                                        {
                                            rowspancount0++;
                                            if (k == dtrowcount - 1)
                                                break;
                                        }


                                    }
                                    rowspanstart0++;
                                }
                                if (rowspanstart1 == i)
                                {
                                    if (grdleave.Rows[i].Cells[0].Text != "&nbsp;")
                                    {
                                        for (int k = rowspanstart1 + 1; grdleave.Rows[i].Cells[1].Text == grdleave.Rows[k].Cells[1].Text; k++)
                                        {
                                            rowspancount1++;
                                            if (k == dtrowcount - 1)
                                                break;
                                        }
                                    }
                                    rowspanstart1++;
                                }

                                if (rowspanstart3 == i)
                                {
                                    if (grdleave.Rows[i].Cells[0].Text != "&nbsp;")
                                    {
                                        for (int k = rowspanstart3 + 1; grdleave.Rows[i].Cells[3].Text == grdleave.Rows[k].Cells[3].Text; k++)
                                        {
                                            rowspancount3++;
                                            if (k == dtrowcount - 1)
                                                break;
                                        }
                                    }
                                    rowspanstart3++;
                                }
                                if (rowspanstart4 == i)
                                {
                                    if (grdleave.Rows[i].Cells[0].Text != "&nbsp;")
                                    {
                                        for (int k = rowspanstart4 + 1; grdleave.Rows[i].Cells[4].Text == grdleave.Rows[k].Cells[4].Text; k++)
                                        {
                                            rowspancount4++;
                                            if (k == dtrowcount - 1)
                                                break;
                                        }
                                    }
                                    rowspanstart4++;
                                }
                                if (rowspanstart5 == i)
                                {
                                    if (grdleave.Rows[i].Cells[0].Text != "&nbsp;")
                                    {
                                        for (int k = rowspanstart5 + 1; grdleave.Rows[i].Cells[5].Text == grdleave.Rows[k].Cells[5].Text; k++)
                                        {
                                            rowspancount5++;
                                            if (k == dtrowcount - 1)
                                                break;
                                        }
                                    }
                                    rowspanstart5++;
                                }


                                if (rowspancount0 != 0)
                                {
                                    rowspanstart0 = rowspanstart0 + rowspancount0;

                                    grdleave.Rows[i].Cells[0].RowSpan = rowspancount0 + 1;
                                    for (int a = i; a < rowspanstart0 - 1; a++)
                                        grdleave.Rows[a + 1].Cells[0].Visible = false;

                                }
                                if (rowspancount1 != 0)
                                {
                                    rowspanstart1 = rowspanstart1 + rowspancount1;

                                    grdleave.Rows[i].Cells[1].RowSpan = rowspancount1 + 1;
                                    for (int a = i; a < rowspanstart1 - 1; a++)
                                        grdleave.Rows[a + 1].Cells[1].Visible = false;

                                }

                                //if (rowspancount2 != 0)
                                //{
                                //    rowspanstart2 = rowspanstart2 + rowspancount2;

                                //    grdUniversalReport.Rows[i].Cells[2].RowSpan = rowspancount2 + 1;
                                //    for (int a = i; a < rowspanstart2 - 1; a++)
                                //        grdUniversalReport.Rows[a + 1].Cells[2].Visible = false;

                                //}
                                if (rowspancount3 != 0)
                                {
                                    rowspanstart3 = rowspanstart3 + rowspancount3;

                                    grdleave.Rows[i].Cells[3].RowSpan = rowspancount3 + 1;
                                    for (int a = i; a < rowspanstart3 - 1; a++)
                                        grdleave.Rows[a + 1].Cells[3].Visible = false;

                                }
                                if (rowspancount4 != 0)
                                {
                                    rowspanstart4 = rowspanstart4 + rowspancount4;

                                    grdleave.Rows[i].Cells[4].RowSpan = rowspancount4 + 1;
                                    for (int a = i; a < rowspanstart4 - 1; a++)
                                        grdleave.Rows[a + 1].Cells[4].Visible = false;

                                }
                                if (rowspancount5 != 0)
                                {
                                    rowspanstart5 = rowspanstart5 + rowspancount5;

                                    grdleave.Rows[i].Cells[5].RowSpan = rowspancount5 + 1;
                                    for (int a = i; a < rowspanstart5 - 1; a++)
                                        grdleave.Rows[a + 1].Cells[5].Visible = false;

                                }
                                for (int j = 0; j < grdleave.HeaderRow.Cells.Count; j++)
                                {
                                    if (i == 0)
                                    {
                                        grdleave.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        if (j == 0)
                                        {
                                            grdleave.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else if (j == 1 || j == 2 || j == 3 || j == 4 || j == 5 || j == 6 || j == 7)
                                        {
                                            grdleave.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;

                                        }
                                        else
                                        {
                                            grdleave.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;

                                        }
                                    }
                                }

                            }

                        }
                    }
                }
            }
            else if (chk_includeprevious.Checked == true)//delsi2301
            {

                Hashtable leavetypehash = new Hashtable();
                Hashtable totalbind = new Hashtable();

                arrColHdrNames.Add("S.No");
                arrColHdrNames2.Add("S.No");
                dtReport.Columns.Add("S.No");


                arrColHdrNames.Add("Staff Name");
                arrColHdrNames2.Add("Staff Name");
                dtReport.Columns.Add("Staff Name");

                arrColHdrNames.Add("Staff Code");
                arrColHdrNames2.Add("Staff Code");
                dtReport.Columns.Add("Staff Code");

                arrColHdrNames.Add("Dept Acronym");
                arrColHdrNames2.Add("Dept Acronym");
                dtReport.Columns.Add("Dept Acronym");

                arrColHdrNames.Add("Designation");
                arrColHdrNames2.Add("Designation");
                dtReport.Columns.Add("Designation");

                arrColHdrNames.Add("Category Name");
                arrColHdrNames2.Add("Category Name");
                dtReport.Columns.Add("Category Name");

                arrColHdrNames.Add("Appl Id");
                arrColHdrNames2.Add("Appl Id");
                dtReport.Columns.Add("Appl Id");

                arrColHdrNames.Add("Month");
                arrColHdrNames2.Add("Month");
                dtReport.Columns.Add("Month");

                string query = "";
                if (!String.IsNullOrEmpty(strstaffcode))
                {
                    query = "select staff_code,leavetype,category_code,college_code,permission,MaxEarnLeave from individual_leave_type where college_code='" + collegecode1 + "' and staff_code='" + strstaffcode + "'";
                    query += " Select category,shortname,LeaveMasterPK  from leave_category where college_code='" + collegecode1 + "' ";
                    query += " select * from hrpaymonths where college_code='" + collegecode1 + "' and SelStatus='1'";
                }
                else
                {
                    query = "select staff_code,leavetype,category_code,college_code,permission,MaxEarnLeave from individual_leave_type where college_code='" + collegecode1 + "'";
                    query += " Select category,shortname,LeaveMasterPK  from leave_category where college_code='" + collegecode1 + "' ";
                    query += " select * from hrpaymonths where college_code='" + collegecode1 + "' and SelStatus='1'"; //
                }
                ds2.Clear();
                ds2 = d2.select_method_wo_parameter(query, "Text");
                if (ds2.Tables.Count > 0 && ds2.Tables[2].Rows.Count > 0)
                {
                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        int h = 7;
                        for (int i = 0; i < chklsleave.Items.Count; i++)
                        {

                            if (chklsleave.Items[i].Selected == true)
                            {
                                if (chklsleave.Items[i].Text != "A")
                                {
                                    string getLeaveType = Convert.ToString(chklsleave.Items[i].Value);
                                    string shortname = d2.GetFunction("Select shortname from leave_category where college_code='" + collegecode1 + "' and LeaveMasterPK='" + getLeaveType + "'");
                                    h++;
                                    arrColHdrNames.Add("Leave Type");
                                    arrColHdrNames2.Add("Previous" + "-" + shortname + " " + "Balance");
                                    dtReport.Columns.Add("Previous" + "-" + chklsleave.Items[i].Value);

                                    h++;
                                    arrColHdrNames.Add("Leave Type");
                                    arrColHdrNames2.Add(Convert.ToString(shortname));
                                    dtReport.Columns.Add(chklsleave.Items[i].Value);

                                    if (!leavetypehash.Contains(Convert.ToString(chklsleave.Items[i].Text)))
                                    {

                                        leavetypehash.Add(Convert.ToString(chklsleave.Items[i].Text), h);

                                    }
                                    h++;
                                    arrColHdrNames.Add("Leave Type");
                                    arrColHdrNames2.Add(Convert.ToString("Balance") + " " + shortname);
                                    dtReport.Columns.Add("Balance" + "-" + chklsleave.Items[i].Value);
                                }
                                else if (chklsleave.Items[i].Text == "A")
                                {
                                    arrColHdrNames.Add("Leave Type");
                                    arrColHdrNames2.Add(Convert.ToString("Absent"));
                                    dtReport.Columns.Add("Absent");

                                }
                            }

                        }

                    }
                    DataRow drHdr1 = dtReport.NewRow();
                    DataRow drHdr2 = dtReport.NewRow();
                    for (int grCol = 0; grCol < dtReport.Columns.Count; grCol++)
                    {
                        drHdr1[grCol] = arrColHdrNames[grCol];
                        drHdr2[grCol] = arrColHdrNames2[grCol];
                    }
                    dtReport.Rows.Add(drHdr1);
                    dtReport.Rows.Add(drHdr2);
                    string deptcode = returnwithsinglecodevalue(cbldepttype);
                    string catagorycode = returnwithsinglecodevalue(cblcategory);
                    string leavetype = string.Empty; //Convert.ToString(cblleavetype.SelectedValue);
                    string staffcodemul = Convert.ToString(cbostaffname.SelectedValue);
                    string filter = "";
                    string staffcode = "";
                    string Appl_ID = "";
                    string category_code = "";
                    bool leavedayscheckcount = false;
                    if (staffdetails_ds.Tables[0].Rows.Count > 0)
                    {
                        if (String.IsNullOrEmpty(strstaffcode))
                        {
                            if (deptcode.Trim().ToLower() != "")
                            {
                                filter = " dept_code in ('" + deptcode + "')";
                            }
                            if (catagorycode.Trim().ToLower() != "")
                            {
                                if (filter.Trim() != "")
                                    filter += " and";
                                filter += "  category_code in ('" + catagorycode + "')";
                            }
                            if (staffcodemul.Trim().ToLower() != "all")
                            {
                                if (filter.Trim() != "")
                                    filter += " and";
                                filter += "  Staff_Code in ('" + staffcodemul + "')";
                            }
                        }
                        else
                        {
                            if (filter.Trim() != "")
                                filter += " and";
                            filter += "  Staff_Code='" + strstaffcode + "'";
                        }
                        if (leavetype.Trim().ToLower() != "all")
                        {

                        }
                        bool leavesingle = false;
                        staffdetails_ds.Tables[0].DefaultView.RowFilter = filter;
                        DataView staffdetails_dv = staffdetails_ds.Tables[0].DefaultView;

                        if (staffdetails_dv.Count > 0)
                        {
                            for (int i = 0; i < staffdetails_dv.Count; i++)
                            {
                                Hashtable totalvalue_dic = new Hashtable();
                                drowGrd = dtReport.NewRow();
                                drowGrd[0] = Convert.ToString(i + 1);
                                drowGrd[1] = Convert.ToString(staffdetails_dv[i]["Staff_Name"]);
                                drowGrd[2] = Convert.ToString(staffdetails_dv[i]["Staff_Code"]);

                                drowGrd[3] = Convert.ToString(staffdetails_dv[i]["dept_acronym"]);//Dept_Name

                                drowGrd[4] = Convert.ToString(staffdetails_dv[i]["Desig_Name"]);
                                drowGrd[5] = Convert.ToString(staffdetails_dv[i]["Category_Name"]);
                                drowGrd[6] = Convert.ToString(staffdetails_dv[i]["Appl_ID"]);
                                staffcode = Convert.ToString(staffdetails_dv[i]["Staff_Code"]);
                                Appl_ID = Convert.ToString(staffdetails_dv[i]["Appl_ID"]);
                                category_code = Convert.ToString(staffdetails_dv[i]["category_code"]);


                                //paymonth
                                if (ds2.Tables[2].Rows.Count > 0)
                                {
                                    DataView ds2table2_dv = new DataView();
                                    string ds2tablefilter = "";
                                    DataView prevdata = new DataView();
                                    string prevdataget = string.Empty;
                                    if (rdomonthlywise.Checked == true)
                                    {
                                        string selectedmonth = returnwithsinglecodevalue(chkmonth);
                                        if (String.IsNullOrEmpty(selectedmonth.Trim()))
                                        {
                                            lblnorec.Visible = true;
                                            lblnorec.Text = "Please Select Month!";
                                            // fpsalary.Visible = false;
                                            rptprint.Visible = false;
                                            return;
                                        }
                                        ds2tablefilter = " PayMonthNum in('" + selectedmonth + "') and PayYear in('" + ddlyear.SelectedItem.Value.ToString() + "')";
                                    }

                                    if (rdodaywise.Checked == true)
                                    {
                                        string month = ""; string year = "";
                                        string date2 = Txtentryto.Text.ToString();
                                        string[] split1 = date2.Split(new Char[] { '/' });
                                        dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                                        month = split1[1].TrimStart('0').ToString();
                                        year = split1[2].ToString();

                                        ds2tablefilter = " PayMonthNum in('" + month + "') and PayYear in('" + year + "')";

                                    }
                                    ds2.Tables[2].DefaultView.RowFilter = ds2tablefilter;
                                    ds2table2_dv = ds2.Tables[2].DefaultView;
                                    DataTable ds2table2_dt = ds2table2_dv.ToTable();


                                    if (ds2table2_dt.Rows.Count > 0)
                                    {
                                        for (int p = 0; p < ds2table2_dt.Rows.Count; p++)
                                        {
                                            if (p != 0)
                                            {
                                                drowGrd = dtReport.NewRow();
                                            }
                                            drowGrd[0] = Convert.ToString(i + 1);
                                            drowGrd[1] = Convert.ToString(staffdetails_dv[i]["Staff_Name"]);
                                            drowGrd[2] = Convert.ToString(staffdetails_dv[i]["Staff_Code"]);

                                            drowGrd[3] = Convert.ToString(staffdetails_dv[i]["dept_acronym"]);//Dept_Name

                                            drowGrd[4] = Convert.ToString(staffdetails_dv[i]["Desig_Name"]);
                                            drowGrd[5] = Convert.ToString(staffdetails_dv[i]["Category_Name"]);
                                            drowGrd[6] = Convert.ToString(staffdetails_dv[i]["Appl_ID"]);
                                            drowGrd[7] = Convert.ToString(ds2table2_dt.Rows[p]["PayMonth"]) + "-" + Convert.ToString(ds2table2_dt.Rows[p]["Payyear"]);
                                            //fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds2table2_dt.Rows[p]["PayMonth"]) + "-" + Convert.ToString(ds2table2_dt.Rows[p]["Payyear"]);
                                            //fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds2table2_dt.Rows[p]["PayMonthNum"]);
                                            //fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 5].Note = Convert.ToString(ds2table2_dt.Rows[p]["Payyear"]);

                                            double llcount = 0;
                                            double prellcount = 0;
                                            totalleave.Clear();
                                            totalbind.Clear();
                                            double addtot = 0;
                                            string actual = "";
                                            double tot_leave = 0;
                                            double pretot_leave = 0;
                                            string leavefromdate = "";
                                            string leavetodate = "";
                                            string preleavefromdate = "";
                                            string preleavetodate = "";
                                            string ishalfdate = "";
                                            string preishalfdate = "";
                                            string halfdaydate = "";
                                            string prehalfdaydate = "";
                                            int finaldate = 0;
                                            string sleave = "";
                                            string presleave = "";
                                            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                                            {
                                                ds2.Tables[0].DefaultView.RowFilter = " Staff_Code= '" + staffcode + "' and category_code='" + category_code + "'";
                                                DataView ds2table0_dv = ds2.Tables[0].DefaultView;
                                                if (ds2table0_dv.Count > 0)
                                                {
                                                    string[] spl_type = ds2table0_dv[0]["leavetype"].ToString().Split(new Char[] { '\\' });
                                                    //  for (int k = 0; k < ds2table0_dv.Count; k++)
                                                    //{
                                                    int col = 6;
                                                    for (int l = 0; spl_type.GetUpperBound(0) >= l; l++)
                                                    {
                                                        string leave = "";
                                                        if (spl_type[l].Trim() != "")
                                                        {
                                                            col++;
                                                            tot_leave = 0;
                                                            string[] split_leave = spl_type[l].Split(';');
                                                            leave = split_leave[0];
                                                            if (split_leave.Length >= 2)
                                                            {
                                                                double.TryParse(Convert.ToString(split_leave[1]), out addtot);
                                                            }

                                                            string leavepk = "";
                                                            ds2.Tables[1].DefaultView.RowFilter = " category='" + leave + "'";
                                                            DataView leavepk_dv = ds2.Tables[1].DefaultView;
                                                            if (leavepk_dv.Count > 0)
                                                            {
                                                                leavepk = Convert.ToString(leavepk_dv[0]["LeaveMasterPK"]);
                                                            }//delsi2201
                                                            for (int dtcol = 8; dtcol < dtReport.Columns.Count; dtcol++)
                                                            {
                                                                string getpk = Convert.ToString(dtReport.Columns[dtcol]);

                                                                if (leavepk == getpk)
                                                                {
                                                                    ds2.Tables[2].DefaultView.RowFilter = " PayMonthNum='" + Convert.ToString(ds2table2_dt.Rows[p]["PayMonthNum"]) + "' and PayYear ='" + Convert.ToString(ds2table2_dt.Rows[p]["Payyear"]) + "'";
                                                                    DataView Lfromlto_dv = ds2.Tables[2].DefaultView;
                                                                    if (Lfromlto_dv.Count > 0)
                                                                    {
                                                                        string dt_get_leave = string.Empty;
                                                                        if (rdodaywise.Checked == true)
                                                                        {
                                                                            string month = ""; string year = "";
                                                                            string date2 = Txtentryto.Text.ToString();
                                                                            string[] split1 = date2.Split(new Char[] { '/' });
                                                                            dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                                                                            month = split1[1].TrimStart('0').ToString();
                                                                            year = split1[2].ToString();

                                                                            dt_get_leave = "select * from RQ_Requisition r,leave_category l where RequestType=5 and LeaveFrom='" + Convert.ToString(dateto) + "' and LeaveTo='" + Convert.ToString(dateto) + "' and ReqAppStatus='1' and l.LeaveMasterPK=r.LeaveMasterFK and r.LeaveMasterFK='" + leavepk + "' and ReqAppNo='" + Appl_ID + "'  ";
                                                                        }
                                                                        else
                                                                        {
                                                                            //dt_get_leave = "select * from RQ_Requisition r,leave_category l where RequestType=5 and LeaveFrom>='" + Lfromlto_dv[k]["From_Date"].ToString() + "' and LeaveTo<='" + Lfromlto_dv[k]["To_Date"].ToString() + "' and ReqAppStatus='1' and l.LeaveMasterPK=r.LeaveMasterFK and r.LeaveMasterFK='" + leavepk + "' and ReqAppNo='" + Appl_ID + "'  ";
                                                                            string getoverallprevious = string.Empty;
                                                                            DataSet prevrec = new DataSet();

                                                                            int currentmonth = Convert.ToInt32(Lfromlto_dv[0]["PayMonthNum"]);
                                                                            DateTime fromdatetime = Convert.ToDateTime(Lfromlto_dv[0]["From_Date"].ToString());
                                                                            fromdatetime = fromdatetime.AddDays(-1);
                                                                            int currentyear = Convert.ToInt32(ddlyear.SelectedItem.Value.ToString());
                                                                            currentmonth--;
                                                                            if (currentmonth == 0)
                                                                            {
                                                                                currentmonth = 12;
                                                                                currentyear--;

                                                                            }
                                                                            //prevdataget = " PayMonthNum in('" + currentmonth + "') and PayYear in('" + currentyear + "')";
                                                                            //ds2.Tables[2].DefaultView.RowFilter = prevdataget;
                                                                            //prevdata = ds2.Tables[2].DefaultView;
                                                                            //if (prevdata.Count > 0)
                                                                            //{
                                                                            getoverallprevious = "select * from RQ_Requisition r,leave_category l where RequestType=5 and (LeaveFrom>='" + ds2.Tables[2].Rows[0]["From_Date"] + "' and LeaveTo<='" + fromdatetime.ToString() + "' or LeaveFrom between'" + ds2.Tables[2].Rows[0]["From_Date"] + "' and '" + fromdatetime.ToString() + "') and ReqAppStatus='1' and l.LeaveMasterPK=r.LeaveMasterFK and r.LeaveMasterFK='" + leavepk + "' and ReqAppNo='" + Appl_ID + "' ";


                                                                            //}
                                                                            prevrec = d2.select_method_wo_parameter(getoverallprevious, "Text");
                                                                            pretot_leave = 0;
                                                                            if (prevrec.Tables[0].Rows.Count > 0)
                                                                            {
                                                                                if (prevrec.Tables[0].Rows.Count > 0)//delsi0314
                                                                                {
                                                                                    for (int prev = 0; prev < prevrec.Tables[0].Rows.Count; prev++)
                                                                                    {
                                                                                        preleavefromdate = Convert.ToString(prevrec.Tables[0].Rows[prev]["LeaveFrom"]);
                                                                                        preleavetodate = Convert.ToString(prevrec.Tables[0].Rows[prev]["LeaveTo"]);
                                                                                        preishalfdate = Convert.ToString(prevrec.Tables[0].Rows[prev]["IsHalfDay"]);
                                                                                        if (preleavefromdate != "" && preleavetodate != "")
                                                                                        {
                                                                                            string dtT = preleavefromdate;
                                                                                            string[] Split = dtT.Split('/');
                                                                                            string enddt = preleavetodate;
                                                                                            Split = enddt.Split('/');
                                                                                            DateTime fromdate = Convert.ToDateTime(dtT);
                                                                                            DateTime todate = Convert.ToDateTime(enddt);
                                                                                            TimeSpan days = todate - fromdate;
                                                                                            string ndate = Convert.ToString(days);
                                                                                            Split = ndate.Split('.');
                                                                                            string getdate = Split[0];
                                                                                            prellcount = 0;
                                                                                            if (fromdate != todate)
                                                                                            {
                                                                                                for (; fromdate <= todate; )
                                                                                                {
                                                                                                    string dayy = fromdate.ToString("dddd");
                                                                                                    leavedayscheckcount = false;
                                                                                                    if (dayy == "Sunday")
                                                                                                    {
                                                                                                        if (split_leave[3] == "0")
                                                                                                            leavedayscheckcount = true;
                                                                                                        else
                                                                                                            leavedayscheckcount = false;
                                                                                                    }
                                                                                                    if (leavedayscheckcount == false)
                                                                                                    {
                                                                                                        prellcount++;
                                                                                                    }
                                                                                                    fromdate = fromdate.AddDays(1);
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                prellcount++;
                                                                                            }
                                                                                            if (preishalfdate == "True")
                                                                                            {
                                                                                                prehalfdaydate = Convert.ToString(prevrec.Tables[0].Rows[prev]["HalfDate"]);
                                                                                                if (pretot_leave == 0)
                                                                                                {
                                                                                                    pretot_leave = prellcount;
                                                                                                    pretot_leave = pretot_leave - 0.5;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    pretot_leave = pretot_leave + prellcount;
                                                                                                    pretot_leave = pretot_leave - 0.5;
                                                                                                }
                                                                                                presleave = leave + "-" + pretot_leave;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                if (pretot_leave == 0)
                                                                                                {
                                                                                                    pretot_leave = pretot_leave + prellcount;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    pretot_leave = pretot_leave + prellcount;
                                                                                                }
                                                                                                presleave = leave + "-" + pretot_leave;
                                                                                            }
                                                                                        }

                                                                                    }
                                                                                }

                                                                            }

                                                                            dt_get_leave = "select * from RQ_Requisition r,leave_category l where RequestType=5 and (LeaveFrom>='" + Lfromlto_dv[0]["From_Date"].ToString() + "' and LeaveTo<='" + Lfromlto_dv[0]["To_Date"].ToString() + "' or LeaveFrom between'" + Lfromlto_dv[0]["From_Date"].ToString() + "' and '" + Lfromlto_dv[0]["To_Date"].ToString() + "') and ReqAppStatus='1' and l.LeaveMasterPK=r.LeaveMasterFK and r.LeaveMasterFK='" + leavepk + "' and ReqAppNo='" + Appl_ID + "'  ";//delsi 0501


                                                                        }
                                                                        ds1 = d2.select_method_wo_parameter(dt_get_leave, "Text");
                                                                        if (ds1.Tables[0].Rows.Count > 0)//delsi0314
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
                                                                                    llcount = 0;
                                                                                    if (fromdate != todate)
                                                                                    {
                                                                                        for (; fromdate <= todate; )
                                                                                        {
                                                                                            string dayy = fromdate.ToString("dddd");
                                                                                            leavedayscheckcount = false;
                                                                                            if (dayy == "Sunday")
                                                                                            {
                                                                                                if (split_leave[3] == "0")
                                                                                                    leavedayscheckcount = true;
                                                                                                else
                                                                                                    leavedayscheckcount = false;
                                                                                            }
                                                                                            if (leavedayscheckcount == false)
                                                                                            {
                                                                                                llcount++;
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
                                                                                        sleave = leave + "-" + tot_leave;
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
                                                                                        sleave = leave + "-" + tot_leave;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        if (spl_type[l].Contains(";"))
                                                                        {
                                                                            string sp = split_leave[0].ToString();
                                                                            actual = split_leave[2].ToString();
                                                                            if (actual == "")
                                                                            {
                                                                                actual = "0";
                                                                            }
                                                                            string[] iii = sleave.Split('-');
                                                                            string sp1 = iii[0];
                                                                            if (sp != sp1)
                                                                            {
                                                                                tot_leave = 0;
                                                                            }
                                                                            string tt = Convert.ToString(tot_leave + "/" + actual);
                                                                            if (!totalleave.Contains(Convert.ToString(leave)))
                                                                                totalleave.Add(Convert.ToString(leave), Convert.ToString(tt));
                                                                            else
                                                                            {
                                                                                string getvalue = Convert.ToString(totalleave[Convert.ToString(leave)]);
                                                                                if (getvalue.Trim() != "")
                                                                                {
                                                                                    getvalue = getvalue + "," + tt;
                                                                                    totalleave.Remove(Convert.ToString(leave));
                                                                                    if (getvalue.Trim() != "")
                                                                                        totalleave.Add(Convert.ToString(leave), Convert.ToString(getvalue));
                                                                                }
                                                                            }
                                                                            int colcount = Convert.ToInt32(leavetypehash[leave]);
                                                                            if (colcount != 0)
                                                                            {
                                                                                // poomalar 06.12.17                                                                            
                                                                                #region for table merge
                                                                                string fistcasual = "HalfDay@fh@" + leave + ""; double monthcount = 0; // poo
                                                                                string secondcasual = "HalfDay@sh@" + leave + ""; //poo
                                                                                // string paymonth = Convert.ToString(fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 5].Tag);
                                                                                string dbyear = Convert.ToString(ds2.Tables[2].Rows[p]["payyear"]);
                                                                                if (rdoyearlywise.Checked == true)
                                                                                {

                                                                                    //   string sqlpaymonth = "select sum(no_days) leave,month(fdate) month,year(fdate) year from staff_leave_details where (lt_taken ='" + leave + "' or lt_taken ='" + secondcasual + "' or lt_taken ='" + fistcasual + "') and staff_code='" + staffcode + "' and apply_approve=1 and college_code ='" + Session["collegecode"] + "'  and fdate >=   (SELECT top 1 CONVERT(VARCHAR(2),DATEPART(MONTH, From_Date)) + '/'+ CONVERT(VARCHAR(2),DATEPART(DAY, From_Date)) + '/' + CONVERT(VARCHAR(4),DATEPART(YEAR, From_Date)) from hrpaymonths order by From_Date) and tdate <= (SELECT top 1 CONVERT(VARCHAR(2),DATEPART(MONTH, To_Date)) + '/'+ CONVERT(VARCHAR(2),DATEPART(DAY, To_Date)) + '/' + CONVERT(VARCHAR(4),DATEPART(YEAR, To_Date)) from hrpaymonths order by To_Date DESC) group by month(fdate),year(fdate)";

                                                                                    string sqlpaymonth = "select sum(no_days) leave from staff_leave_details where (lt_taken ='" + leave + "' or lt_taken ='" + secondcasual + "' or lt_taken ='" + fistcasual + "') and staff_code='" + staffcode + "' and apply_approve=1 and college_code ='" + Session["collegecode"] + "' and fdate>='" + Lfromlto_dv[0]["From_Date"].ToString() + "' and tdate<='" + Lfromlto_dv[0]["To_Date"].ToString() + "'  and fdate >=   (SELECT top 1 CONVERT(VARCHAR(2),DATEPART(MONTH, From_Date)) + '/'+ CONVERT(VARCHAR(2),DATEPART(DAY, From_Date)) + '/' + CONVERT(VARCHAR(4),DATEPART(YEAR, From_Date)) from hrpaymonths order by From_Date) and tdate <= (SELECT top 1 CONVERT(VARCHAR(2),DATEPART(MONTH, To_Date)) + '/'+ CONVERT(VARCHAR(2),DATEPART(DAY, To_Date)) + '/' + CONVERT(VARCHAR(4),DATEPART(YEAR, To_Date)) from hrpaymonths order by To_Date DESC)";

                                                                                    DataSet dspaymonth = new DataSet();
                                                                                    dspaymonth = d2.select_method_wo_parameter(sqlpaymonth, "Text");

                                                                                    if (dspaymonth.Tables[0].Rows.Count > 0)
                                                                                    {
                                                                                        double.TryParse(Convert.ToString(dspaymonth.Tables[0].Rows[0]["leave"]), out monthcount);
                                                                                        tot_leave += monthcount;//delsi11/05/2018

                                                                                    }
                                                                                }

                                                                                string sql = string.Empty; string leaveold = string.Empty; double oldleave = 0;
                                                                                if (rdomonthlywise.Checked == true)
                                                                                {

                                                                                    string selectedmonth = returnwithsinglecodevalue(chkmonth);
                                                                                    string monthselec = Convert.ToString(ds2table2_dt.Rows[p]["PayMonthNum"]); ;
                                                                                    if (String.IsNullOrEmpty(selectedmonth.Trim()))
                                                                                    {
                                                                                        lblnorec.Visible = true;
                                                                                        lblnorec.Text = "Please Select Month!";
                                                                                        // fpsalary.Visible = false;
                                                                                        rptprint.Visible = false;
                                                                                        return;
                                                                                    }
                                                                                    sql = "select sum(no_days) from staff_leave_details where (lt_taken ='" + leave + "' or lt_taken ='" + secondcasual + "' or lt_taken ='" + fistcasual + "') and staff_code='" + staffcode + "' and apply_approve=1 and college_code ='" + Session["collegecode"] + "'  and fdate >=   (SELECT top 1 CONVERT(VARCHAR(2),DATEPART(MONTH, From_Date)) + '/'+ CONVERT(VARCHAR(2),DATEPART(DAY, From_Date)) + '/' + CONVERT(VARCHAR(4),DATEPART(YEAR, From_Date)) from hrpaymonths order by From_Date) and tdate <= (SELECT top 1 CONVERT(VARCHAR(2),DATEPART(MONTH, To_Date)) + '/'+ CONVERT(VARCHAR(2),DATEPART(DAY, To_Date)) + '/' + CONVERT(VARCHAR(4),DATEPART(YEAR, To_Date)) from hrpaymonths order by To_Date DESC) and month(staff_leave_details.fdate) in('" + monthselec + "') and month  (staff_leave_details.tdate) in('" + monthselec + "') and  year(staff_leave_details.fdate)='" + ddlyear.SelectedItem.Text + "' and year (staff_leave_details.tdate)='" + ddlyear.SelectedItem.Text + "'"; // poo 06.12.17 //paymonth

                                                                                    DataSet dstab = new DataSet();
                                                                                    dstab = d2.select_method_wo_parameter(sql, "Text");
                                                                                    leaveold = d2.GetFunction(sql); double.TryParse(leaveold, out oldleave);
                                                                                    tot_leave += oldleave;


                                                                                }
                                                                                if (rdodaywise.Checked == true)
                                                                                {
                                                                                    string month = ""; string year = "";
                                                                                    string date2 = Txtentryto.Text.ToString();
                                                                                    string[] split1 = date2.Split(new Char[] { '/' });
                                                                                    dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                                                                                    month = split1[1].TrimStart('0').ToString();
                                                                                    year = split1[2].ToString();
                                                                                    string dayquery = "select sum(no_days) from staff_leave_details where (lt_taken ='" + leave + "' or lt_taken ='" + secondcasual + "' or lt_taken ='" + fistcasual + "') and staff_code='" + staffcode + "' and apply_approve=1 and college_code ='" + Session["collegecode"] + "'  and fdate >=   (SELECT top 1 CONVERT(VARCHAR(2),DATEPART(MONTH, From_Date)) + '/'+ CONVERT(VARCHAR(2),DATEPART(DAY, From_Date)) + '/' + CONVERT(VARCHAR(4),DATEPART(YEAR, From_Date)) from hrpaymonths order by From_Date) and tdate <= (SELECT top 1 CONVERT(VARCHAR(2),DATEPART(MONTH, To_Date)) + '/'+ CONVERT(VARCHAR(2),DATEPART(DAY, To_Date)) + '/' + CONVERT(VARCHAR(4),DATEPART(YEAR, To_Date)) from hrpaymonths order by To_Date DESC) and fdate='" + dateto + "' and tdate='" + dateto + "' ";
                                                                                    DataSet dsday = new DataSet();
                                                                                    dsday = d2.select_method_wo_parameter(dayquery, "Text");
                                                                                    leaveold = d2.GetFunction(dayquery); double.TryParse(leaveold, out oldleave);
                                                                                    tot_leave += oldleave;
                                                                                }

                                                                                #endregion
                                                                                double preLeaveBalance = addtot - pretot_leave;
                                                                                //drowGrd[dtcol - 1] = Convert.ToString(pretot_leave);
                                                                                if (chk_includeoverall.Checked == true)
                                                                                {
                                                                                    drowGrd[dtcol - 1] = Convert.ToString(preLeaveBalance);
                                                                                }
                                                                                else
                                                                                {
                                                                                    drowGrd[dtcol - 1] = Convert.ToString(preLeaveBalance + "/" + addtot);
                                                                                }
                                                                                if (chk_includeoverall.Checked == true)
                                                                                {
                                                                                    drowGrd[dtcol] = Convert.ToString(tot_leave);
                                                                                }
                                                                                else
                                                                                {
                                                                                    drowGrd[dtcol] = Convert.ToString(tot_leave + "/" + actual);
                                                                                }
                                                                                //double overallcount = pretot_leave + tot_leave;
                                                                                //double balnc = 0;
                                                                                //balnc = addtot - overallcount;
                                                                                double currentbc = preLeaveBalance - tot_leave;
                                                                                //  drowGrd[dtcol + 1] = Convert.ToString(balnc);
                                                                                drowGrd[dtcol + 1] = Convert.ToString(currentbc);
                                                                                if (totalvalue_dic.Contains(leave))
                                                                                {
                                                                                    string value = totalvalue_dic[leave].ToString();
                                                                                    string[] leavecount = value.Split('/');
                                                                                    totalvalue_dic.Remove(leave);
                                                                                    double leavecnt = 0;//barath 19.06.17
                                                                                    double.TryParse(leavecount[0], out leavecnt);
                                                                                    //leavecnt = leavecnt+oldleave;
                                                                                    double total = (leavecnt + tot_leave);
                                                                                    //total += oldleave;
                                                                                    //int total = Convert.ToInt32(leavecount[0]) + Convert.ToInt32(tot_leave);                  
                                                                                    totalvalue_dic.Add(leave, total + "/" + addtot);
                                                                                }
                                                                                else
                                                                                {
                                                                                    //tot_leave =tot_leave+ oldleave;
                                                                                    totalvalue_dic.Add(leave, Convert.ToString(tot_leave) + "/" + addtot);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else if (getpk == "Absent")
                                                                {
                                                                    ds2.Tables[2].DefaultView.RowFilter = " PayMonthNum='" + Convert.ToString(ds2table2_dt.Rows[p]["PayMonthNum"]) + "' and PayYear ='" + Convert.ToString(ds2table2_dt.Rows[p]["Payyear"]) + "'";
                                                                    double mrngcount = 0;
                                                                    double evngcount = 0;
                                                                    double overallcount = 0;
                                                                    DataView Lfromlto_dv = ds2.Tables[2].DefaultView;
                                                                    if (Lfromlto_dv.Count > 0)
                                                                    {
                                                                        DateTime fdatetime = Convert.ToDateTime(Lfromlto_dv[0]["From_Date"].ToString());
                                                                        DateTime tdatetime = Convert.ToDateTime(Lfromlto_dv[0]["To_Date"].ToString());


                                                                        DataTable attendancedt = new DataTable();
                                                                        string fromdatemonyear = fdatetime.Month + "/" + fdatetime.Year;
                                                                        string todatemonyear = tdatetime.Month + "/" + tdatetime.Year;
                                                                        string overallmonthdet = fromdatemonyear + "','" + todatemonyear;
                                                                        string attnquery = "select * from staff_attnd  where mon_year in('" + overallmonthdet + "') and staff_code='" + staffcode + "'";
                                                                        attendancedt = dir.selectDataTable(attnquery);
                                                                        if (attendancedt.Rows.Count > 0)
                                                                        {
                                                                            // dayord = Convert.ToString(dts.Rows[0][array[j]]);

                                                                            DataView attnddv = new DataView();
                                                                            if (fdatetime != tdatetime)
                                                                            {
                                                                                for (; fdatetime <= tdatetime; )
                                                                                {
                                                                                    string getfdate = Convert.ToString(fdatetime.Day);
                                                                                    string getfmonth = Convert.ToString(fdatetime.Month);
                                                                                    string getfyear = Convert.ToString(fdatetime.Year);
                                                                                    string combinemonthyear = getfmonth + "/" + getfyear;
                                                                                    attendancedt.DefaultView.RowFilter = " mon_year='" + combinemonthyear + "'";
                                                                                    attnddv = attendancedt.DefaultView;
                                                                                    string GetAtt = string.Empty;
                                                                                    if (attnddv.Count > 0)
                                                                                    {
                                                                                        GetAtt = Convert.ToString(attnddv[0][getfdate]);

                                                                                    }
                                                                                    //  string qur1 = Convert.ToString(attendancedt.Rows[0][getval.Trim()]);

                                                                                    // string GetAtt = d2.GetFunction("select [" + fdatetime.Day + "] from staff_attnd where mon_year='" + fdatetime.Month + "/" + fdatetime.Year + "' and staff_code='" + staffcode + "'  and ([" + fdatetime.Day + "] like '%" + Convert.ToString("A") + "' or [" + fdatetime.Day + "] like '" + Convert.ToString("A") + "%')");
                                                                                    if (!String.IsNullOrEmpty(GetAtt) && GetAtt.Trim() != "0" && GetAtt.Contains('-'))
                                                                                    {
                                                                                        if (GetAtt.Contains('-'))
                                                                                        {
                                                                                            string[] split_Attendavlue = GetAtt.Split('-');
                                                                                            if (split_Attendavlue.Length > 0)
                                                                                            {
                                                                                                string Morning_value = split_Attendavlue[0].ToString();
                                                                                                string Evening_Value = split_Attendavlue[1].ToString();
                                                                                                if (Morning_value == "A")
                                                                                                {
                                                                                                    mrngcount++;

                                                                                                }
                                                                                                if (Evening_Value == "A")
                                                                                                {
                                                                                                    evngcount++;
                                                                                                }
                                                                                            }
                                                                                        }

                                                                                    }

                                                                                    fdatetime = fdatetime.AddDays(1);
                                                                                }
                                                                                overallcount = (mrngcount + evngcount) / 2;
                                                                                drowGrd[dtcol] = overallcount;
                                                                            }
                                                                        }

                                                                    }

                                                                }
                                                            }
                                                        }

                                                    }
                                                    //}
                                                }
                                            }
                                            dtReport.Rows.Add(drowGrd);
                                        }
                                        //drowGrd = dtReport.NewRow();
                                        //drowGrd[7] = "Total";

                                        //if (totalvalue_dic.Count > 0)
                                        //{
                                        //    foreach (DictionaryEntry item in totalvalue_dic)
                                        //    {
                                        //        string leave_key = Convert.ToString(item.Key);
                                        //        string Value = Convert.ToString(item.Value);
                                        //        string[] total = Value.Split('/');
                                        //        int colcount1 = Convert.ToInt32(leavetypehash[leave_key]);
                                        //        if (total.Length > 1)
                                        //        {
                                        //            double tot = 0;
                                        //            double.TryParse(total[0].ToString(), out tot);
                                        //            drowGrd[colcount1] = tot + "/" + total[1].ToString();


                                        //        }
                                        //    }
                                        //}
                                        //dtReport.Rows.Add(drowGrd);
                                        //drowGrd = dtReport.NewRow();
                                        //drowGrd[7] = "Taken";
                                        //if (totalvalue_dic.Count > 0)
                                        //{
                                        //    foreach (DictionaryEntry item in totalvalue_dic)
                                        //    {
                                        //        string leave_key = Convert.ToString(item.Key);
                                        //        string Value = Convert.ToString(item.Value);
                                        //        string[] total = Value.Split('/');
                                        //        int colcount1 = Convert.ToInt32(leavetypehash[leave_key]);
                                        //        if (total.Length > 1)
                                        //        {
                                        //            double tot = 0;
                                        //            double.TryParse(total[0].ToString(), out tot);
                                        //            drowGrd[colcount1] = Convert.ToString(tot);


                                        //        }
                                        //    }
                                        //}

                                        //dtReport.Rows.Add(drowGrd);
                                        //drowGrd = dtReport.NewRow();

                                        //if (totalvalue_dic.Count > 0)
                                        //{
                                        //    foreach (DictionaryEntry item in totalvalue_dic)
                                        //    {
                                        //        string leave_key = Convert.ToString(item.Key);
                                        //        string Value = Convert.ToString(item.Value);
                                        //        string[] total = Value.Split('/');
                                        //        int colcount1 = Convert.ToInt32(leavetypehash[leave_key]);
                                        //        if (total.Length > 1)
                                        //        {
                                        //            double tot = 0;
                                        //            double.TryParse(total[0].ToString(), out tot);
                                        //            drowGrd[colcount1] = Convert.ToString(Convert.ToDouble(total[1]) - Convert.ToDouble(tot));


                                        //        }
                                        //    }
                                        //}

                                        //drowGrd[7] = "Available";
                                        //dtReport.Rows.Add(drowGrd);

                                        //if (totalvalue_dic.Count > 0)
                                        //{
                                        //    foreach (DictionaryEntry item in totalvalue_dic)
                                        //    {
                                        //        string leave_key = Convert.ToString(item.Key);
                                        //        string Value = Convert.ToString(item.Value);
                                        //        string[] total = Value.Split('/');
                                        //        int colcount1 = Convert.ToInt32(leavetypehash[leave_key]);
                                        //        if (total.Length > 1)
                                        //        {
                                        //            double tot = 0;
                                        //            double.TryParse(total[0].ToString(), out tot);

                                        //        }
                                        //    }
                                        //}
                                    }
                                }
                            }

                        }
                    }
                    rptprint.Visible = true;
                    lblnorec.Visible = false;
                    grdleave.DataSource = dtReport;
                    grdleave.DataBind();
                    grdleave.Visible = true;

                    GridViewRow row = grdleave.Rows[0];
                    GridViewRow previousRow = grdleave.Rows[1];

                    for (int i = 0; i < dtReport.Columns.Count; i++)
                    {
                        if (row.Cells[i].Text == previousRow.Cells[i].Text)
                        {
                            row.Cells[i].RowSpan = 2;
                            previousRow.Cells[i].Visible = false;

                        }

                    }
                    for (int cell = grdleave.Rows[0].Cells.Count - 1; cell > 0; cell--)
                    {
                        TableCell colum = grdleave.Rows[0].Cells[cell];
                        TableCell previouscol = grdleave.Rows[0].Cells[cell - 1];
                        if (colum.Text == previouscol.Text)
                        {
                            if (previouscol.ColumnSpan == 0)
                            {
                                if (colum.ColumnSpan == 0)
                                {
                                    previouscol.ColumnSpan += 2;
                                }
                                else
                                {
                                    previouscol.ColumnSpan += colum.ColumnSpan + 1;
                                }
                                colum.Visible = false;
                            }
                        }

                    }
                    for (int i = 0; i < grdleave.Rows.Count; i++)
                    {


                        string gettxt = grdleave.Rows[i].Cells[7].Text;
                        if (gettxt == "Total" || gettxt == "Taken" || gettxt == "Available")
                        {
                            grdleave.Rows[i].BackColor = Color.Bisque;
                            grdleave.Rows[i].ForeColor = Color.IndianRed;
                        }


                    }
                    if (dtReport.Rows.Count > 1)
                    {

                        int dtrowcount = dtReport.Rows.Count;
                        int rowspanstart0 = 0;
                        int rowspanstart1 = 0;
                        int rowspanstart2 = 0;
                        int rowspanstart3 = 0;
                        int rowspanstart4 = 0;
                        int rowspanstart5 = 0;
                        for (int i = 0; i < grdleave.Rows.Count; i++)
                        {

                            int rowspancount0 = 0;
                            int rowspancount1 = 0;
                            int rowspancount2 = 0;
                            int rowspancount3 = 0;
                            int rowspancount4 = 0;
                            int rowspancount5 = 0;


                            if (i != dtrowcount - 1)
                            {
                                if (rowspanstart0 == i)
                                {

                                    for (int k = rowspanstart0 + 1; grdleave.Rows[i].Cells[0].Text == grdleave.Rows[k].Cells[0].Text; k++)
                                    {
                                        rowspancount0++;
                                        rowspancount3++;
                                        rowspancount4++;
                                        rowspancount5++;
                                        if (k == dtrowcount - 1)
                                            break;
                                    }
                                    rowspanstart0++;
                                }
                                if (rowspanstart1 == i)
                                {

                                    for (int k = rowspanstart1 + 1; grdleave.Rows[i].Cells[1].Text == grdleave.Rows[k].Cells[1].Text; k++)
                                    {
                                        rowspancount1++;
                                        if (k == dtrowcount - 1)
                                            break;
                                    }

                                    rowspanstart1++;
                                }

                                //if (rowspanstart3 == i)
                                //{
                                //    for (int k = rowspanstart3 + 1; grdleave.Rows[i].Cells[3].Text == grdleave.Rows[k].Cells[3].Text; k++)
                                //    {


                                //        rowspancount3++;
                                //        if (k == dtrowcount - 1)
                                //            break;
                                //    }

                                //    rowspanstart3++;
                                //}
                                //if (rowspanstart4 == i)
                                //{

                                //    for (int k = rowspanstart4 + 1; grdleave.Rows[i].Cells[4].Text == grdleave.Rows[k].Cells[4].Text; k++)
                                //    {
                                //        rowspancount4++;
                                //        if (k == dtrowcount - 1)
                                //            break;
                                //    }

                                //    rowspanstart4++;
                                //}
                                //if (rowspanstart5 == i)
                                //{

                                //    for (int k = rowspanstart5 + 1; grdleave.Rows[i].Cells[5].Text == grdleave.Rows[k].Cells[5].Text; k++)
                                //    {
                                //        rowspancount5++;
                                //        if (k == dtrowcount - 1)
                                //            break;
                                //    }

                                //    rowspanstart5++;
                                //}


                                if (rowspancount0 != 0)
                                {
                                    rowspanstart0 = rowspanstart0 + rowspancount0;
                                    rowspanstart3 = rowspanstart3 + rowspancount0;
                                    grdleave.Rows[i].Cells[0].RowSpan = rowspancount0 + 1;
                                    grdleave.Rows[i].Cells[3].RowSpan = rowspancount0 + 1;
                                    grdleave.Rows[i].Cells[4].RowSpan = rowspancount0 + 1;
                                    grdleave.Rows[i].Cells[5].RowSpan = rowspancount0 + 1;
                                    for (int a = i; a < rowspanstart0 - 1; a++)
                                    {
                                        grdleave.Rows[a + 1].Cells[0].Visible = false;
                                        grdleave.Rows[a + 1].Cells[3].Visible = false;
                                        grdleave.Rows[a + 1].Cells[4].Visible = false;
                                        grdleave.Rows[a + 1].Cells[5].Visible = false;
                                    }


                                }
                                if (rowspancount1 != 0)
                                {
                                    rowspanstart1 = rowspanstart1 + rowspancount1;

                                    grdleave.Rows[i].Cells[1].RowSpan = rowspancount1 + 1;
                                    for (int a = i; a < rowspanstart1 - 1; a++)
                                        grdleave.Rows[a + 1].Cells[1].Visible = false;


                                }

                                //if (rowspancount2 != 0)
                                //{
                                //    rowspanstart2 = rowspanstart2 + rowspancount2;

                                //    grdUniversalReport.Rows[i].Cells[2].RowSpan = rowspancount2 + 1;
                                //    for (int a = i; a < rowspanstart2 - 1; a++)
                                //        grdUniversalReport.Rows[a + 1].Cells[2].Visible = false;

                                //}
                                //if (rowspancount3 != 0)
                                //{
                                //    rowspanstart3 = rowspanstart3 + rowspancount3;

                                //    grdleave.Rows[i].Cells[3].RowSpan = rowspancount3 + 1;
                                //    for (int a = i; a < rowspanstart3 - 1; a++)
                                //        grdleave.Rows[a + 1].Cells[3].Visible = false;

                                //}
                                //if (rowspancount4 != 0)
                                //{
                                //    rowspanstart4 = rowspanstart4 + rowspancount4;

                                //    grdleave.Rows[i].Cells[4].RowSpan = rowspancount4 + 1;
                                //    for (int a = i; a < rowspanstart4 - 1; a++)
                                //        grdleave.Rows[a + 1].Cells[4].Visible = false;

                                //}
                                //if (rowspancount5 != 0)
                                //{
                                //    rowspanstart5 = rowspanstart5 + rowspancount5;

                                //    grdleave.Rows[i].Cells[5].RowSpan = rowspancount5 + 1;
                                //    for (int a = i; a < rowspanstart5 - 1; a++)
                                //        grdleave.Rows[a + 1].Cells[5].Visible = false;

                                //}

                                for (int j = 0; j < grdleave.HeaderRow.Cells.Count; j++)
                                {
                                    if (i == 0)
                                    {
                                        grdleave.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                    }

                                    else
                                    {

                                        if (j == 0)
                                        {
                                            grdleave.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else if (j == 1 || j == 2 || j == 3 || j == 4 || j == 5 || j == 6 || j == 7)
                                        {
                                            grdleave.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;

                                        }
                                        else
                                        {
                                            grdleave.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;

                                        }
                                    }
                                }
                            }

                        }
                    }


                }

            }

        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
            d2.sendErrorMail(ex, Convert.ToString(Session["collegecode"]), "staffleavereport.aspx");
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grdleave, reportname);
                //d2.printexcelreport(fpsalary, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {

            Printcontrol.Visible = true;

            string ss = null;
            string page_name = string.Empty;
            string degreedetails = string.Empty;
            string date = "";
            if (rdoyearlywise.Checked == true)
            {
                page_name = "Yearly Wise Staff Cumulative Report";
                date = "";
            }
            else if (rdomonthlywise.Checked == true)
            {
                page_name = "Monthly Wise Staff Cumulative Report";
                date = "";
            }
            if (rdodaywise.Checked == true)
            {
                page_name = "Day Wise Staff Cumulative Report";
                date = "@Date :" + Txtentryfrom.Text.ToString();// +" To :" + Txtentryto.Text.ToString();
            }

            //Session["column_header_row_count"] = fpsalary.Sheets[0].SheetCorner.RowCount;

            degreedetails = page_name;
            string pagename = "staffleavereport.aspx";
            Printcontrol.loadspreaddetails(grdleave, pagename, degreedetails, 0, ss);
            //  Printcontrol.loadspreaddetails(fpsalary, pagename, degreedetails,0,ss);
            Printcontrol.Visible = true;
        }
        catch
        { }
    }

    protected void chkleave_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (chkleave.Checked == true)
            {
                for (int item = 0; item < chklsleave.Items.Count; item++)
                {
                    chklsleave.Items[item].Selected = true;
                }
                txtleave.Text = "Leave (" + chklsleave.Items.Count + ")";
            }
            else
            {
                for (int item = 0; item < chklsleave.Items.Count; item++)
                {
                    chklsleave.Items[item].Selected = false;
                }
                txtleave.Text = "---Select---";
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void chklsleave_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            for (int i = 0; i < chklsleave.Items.Count; i++)
            {
                if (chklsleave.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtleave.Text = "Leave (" + commcount.ToString() + ")";
                }
            }
            if (commcount == 0)
            {
                txtleave.Text = "--Select--";
            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        ViewReport.Visible = false;
    }
    protected void grdleave_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0)
            {
                e.Row.BackColor = Color.FromArgb(12, 166, 202);
                e.Row.HorizontalAlign = HorizontalAlign.Center;
            }
            if (e.Row.RowIndex == 1)
            {
                e.Row.BackColor = Color.FromArgb(12, 166, 202);
                e.Row.HorizontalAlign = HorizontalAlign.Center;
            }
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[6].Visible = false;


        }
    }
    protected void grdleave_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
        }
    }

    protected void grdleave_OnRowCreated(object sender, GridViewRowEventArgs e)
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
    protected void chk_includeprevious_change(object sender, EventArgs e)
    {
        load_leavetype();
    }
    public override void VerifyRenderingInServerForm(Control control)
    { }
    protected void chk_includeoverall_change(object sender, EventArgs e)
    { 
    
    }
}