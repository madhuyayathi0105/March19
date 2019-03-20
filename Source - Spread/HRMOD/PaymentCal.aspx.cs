using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class PaymentCal : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    Hashtable ht = new Hashtable();
    string college = string.Empty;
    int height = 0;
    string usercode = string.Empty;
    FarPoint.Web.Spread.TextCellType cellText = new FarPoint.Web.Spread.TextCellType();

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
            college = Session["collegecode"].ToString();
            usercode = Session["usercode"].ToString();

            if (!IsPostBack)
            {
                FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
                style1.Font.Size = 10;
                style1.Font.Bold = true;
                FpPayment.Sheets[0].AutoPostBack = true;
                FpPayment.CommandBar.Visible = false;
                style1.Font.Size = 13;
                style1.Font.Bold = true;

                style1.Font.Size = FontUnit.Medium;
                FpPayment.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpPayment.Sheets[0].AllowTableCorner = true;
                FpPayment.Sheets[0].RowHeader.Visible = false;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.Font.Size = FontUnit.Medium;
                darkstyle.Font.Name = "Book Antiqua";
                darkstyle.Font.Bold = true;
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                FpPayment.Sheets[0].ColumnHeader.DefaultStyle = darkstyle;
                FpPayment.Sheets[0].RowCount = 0;
                FpPayment.Sheets[0].ColumnCount = 0;
                FpPayment.Sheets[0].Visible = true;
                FpPayment.Visible = true;
                FpPayment.CommandBar.Visible = false;

                //txtfromdate.Text = (DateTime.Now).ToString("MM/yyyy");

                bindcollege();
                bindyear();
                bindmonth();
                msg.Visible = false;
                FpPayment.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnprintmaster.Visible = false;
                btnxl.Visible = false;
                BindDepartment(college);
                BindDesignation(college);
                bindcategory();
                bindstafftype(college);
                lblvalidation.Visible = false;
                loadallowance();
                txtAllowance.Enabled = false;
                rdbtnlst_change(sender, e);
            }
            msg.Visible = false;
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
            ht.Clear();
            ht.Add("column_field", columnfield.ToString());
            ds = da.select_method("bind_college", ht, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
            ddlcollege.Items.Insert(0, "---Select---");
        }
        catch (Exception e) { }
    }

    public void BindDepartment(string scollege)
    {
        try
        {
            string col = scollege;
            if (col == "---Select---")
            {
                col = Session["collegecode"].ToString();
            }
            chklst_Department.Visible = true;
            chklst_Department.Items.Clear();
            ds.Clear();

            string deptquery = "";

            string singleuser = Session["single_user"].ToString();
            if (singleuser == "True")
            {
                deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + col + "') order by dept_name";
            }
            else
            {
                string group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + col + "') order by dept_name";
            }
            if (deptquery != "")
            {
                ds = da.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklst_Department.DataSource = ds;
                    chklst_Department.DataTextField = "dept_name";
                    chklst_Department.DataValueField = "Dept_Code";
                    chklst_Department.DataBind();
                }
            }
            for (int i = 0; i < chklst_Department.Items.Count; i++)
            {
                chklst_Department.Items[i].Selected = true;
            }
            if (chklst_Department.Items.Count > 0)
            {
                panel_Department.Height = 300;
            }
            else
            {
                panel_Department.Height = 100;
            }
        }
        catch (Exception e) { }
    }

    public void bindyear()
    {
        ddlyear.Items.Clear();
        ds.Clear();
        string clgCode = "";
        if (ddlcollege.SelectedItem.Text == "---Select---")
            clgCode = Convert.ToString(Session["collegecode"]);
        else
            clgCode = Convert.ToString(ddlcollege.SelectedItem.Value);
        string query = "";
        query = "select  distinct PayYear as fromdate from HrPayMonths where college_Code='" + clgCode + "' and SelStatus='1' order by fromdate desc";
        ds = da.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string value = ds.Tables[0].Rows[i][0].ToString();
                ddlyear.Items.Add(value.ToString());
            }
        }
    }

    public void bindmonth()
    {
        string query = "";
        ddlmonth.Enabled = true;
        string clgCode = "";
        if (ddlcollege.SelectedItem.Text == "---Select---")
            clgCode = Convert.ToString(Session["collegecode"]);
        else
            clgCode = Convert.ToString(ddlcollege.SelectedItem.Value);
        ds.Clear();
        if (ddlyear.SelectedItem.Text != "")
        {
            query = "select paymonth,PayMonthNum from HrPayMonths where College_Code='" + clgCode + "'and PayYear='" + ddlyear.SelectedItem.Text + "' and SelStatus='1'";

            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlmonth.DataSource = ds;
                    ddlmonth.DataTextField = "paymonth";
                    ddlmonth.DataValueField = "PayMonthNum";
                    ddlmonth.DataBind();
                }
            }
        }
    }

    public void BindDesignation(string coll)
    {
        try
        {
            chklst_Designation.Visible = true;
            chklst_Designation.Items.Clear();
            ds.Clear();
            string college = coll.ToString();
            if (college == "---Select---")
            {
                college = Session["collegecode"].ToString();
            }
            ds = da.loaddesignation(college);
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklst_Designation.DataSource = ds;
                chklst_Designation.DataTextField = "desig_name";
                chklst_Designation.DataValueField = "Desig_Code";
                chklst_Designation.DataBind();
            }

            for (int i = 0; i < chklst_Designation.Items.Count; i++)
            {
                chklst_Designation.Items[i].Selected = true;
            }
            if (chklst_Designation.Items.Count > 5)
            {
                panel_Designation.Height = 300;
            }
            else
            {
                panel_Designation.Height = 100;
            }
        }
        catch (Exception e) { }
    }

    public void bindcategory()
    {
        try
        {
            chklst_Category.Visible = true;
            chklst_Category.Items.Clear();
            ds.Clear();

            ds = da.loadcategory(college);
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklst_Category.DataSource = ds;
                chklst_Category.DataTextField = "category_name";
                chklst_Category.DataValueField = "Category_Code";
                chklst_Category.DataBind();
            }
            for (int i = 0; i < chklst_Category.Items.Count; i++)
            {
                chklst_Category.Items[i].Selected = true;
            }
            if (chklst_Category.Items.Count > 5)
            {
                panel_Category.Height = 250;
            }
            else
            {
                panel_Category.Height = 100;
            }
        }
        catch (Exception e) { }
    }

    public void bindstafftype(string college)
    {
        try
        {
            cblstafftype.Visible = true;
            cblstafftype.Items.Clear();

            string selq = "select distinct stftype from stafftrans st,staffmaster sm where sm.staff_code=st.staff_code and latestrec='1' and college_code='" + college + "' and stftype is not null and stftype<>''";
            ds.Clear();
            //ds = da.loadstafftype(college);
            ds = da.select_method_wo_parameter(selq, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblstafftype.DataSource = ds;
                cblstafftype.DataTextField = "StfType";

                cblstafftype.DataBind();
            }
            for (int i = 0; i < cblstafftype.Items.Count; i++)
            {
                cblstafftype.Items[i].Selected = true;
            }
            if (cblstafftype.Items.Count > 5)
            {
                panelstafftype.Height = 200;
            }
            else
            {
                panelstafftype.Height = 100;
            }
        }
        catch (Exception e) { }
    }

    protected void ddlyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindmonth();
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        string collegecode = ddlcollege.SelectedItem.Value.ToString();
        bindyear();
        bindmonth();
        BindDepartment(collegecode);
        BindDesignation(collegecode);
        loadallowance();
    }

    protected void cbstafftype_CheckedChanged(object sender, EventArgs e)
    {
        if (cbstafftype.Checked == true)
        {
            for (int i = 0; i < cblstafftype.Items.Count; i++)
            {
                cblstafftype.Items[i].Selected = true;
                txt_stafftype.Text = "Staff Type(" + (cblstafftype.Items.Count) + ")";
            }
            // panelstafftype.Focus();
        }
        else
        {
            for (int i = 0; i < cblstafftype.Items.Count; i++)
            {
                cblstafftype.Items[i].Selected = false;
                txt_stafftype.Text = "---Select---";
            }
        }
    }

    protected void cblstafftype_SelectedIndexChanged(object sender, EventArgs e)
    {
        // panelstafftype.Focus();
        int desigcount = 0;
        for (int i = 0; i < cblstafftype.Items.Count; i++)
        {
            if (cblstafftype.Items[i].Selected == true)
            {
                desigcount = desigcount + 1;
                txt_stafftype.Text = "Staff Type(" + desigcount.ToString() + ")";
            }
        }
        if (desigcount == 0)
        {
            txt_stafftype.Text = "---Select---";
            cbstafftype.Checked = false;
        }
    }

    protected void chk_Designation_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_Designation.Checked == true)
        {
            for (int i = 0; i < chklst_Designation.Items.Count; i++)
            {
                chklst_Designation.Items[i].Selected = true;
                txt_designation.Text = "Designation(" + (chklst_Designation.Items.Count) + ")";
            }
            panel_Designation.Focus();
        }
        else
        {
            for (int i = 0; i < chklst_Designation.Items.Count; i++)
            {
                chklst_Designation.Items[i].Selected = false;
                txt_designation.Text = "---Select---";
            }
        }
    }

    protected void chklst_Designation_SelectedIndexChanged(object sender, EventArgs e)
    {
        panel_Designation.Focus();
        int desigcount = 0;
        for (int i = 0; i < chklst_Designation.Items.Count; i++)
        {
            if (chklst_Designation.Items[i].Selected == true)
            {
                desigcount = desigcount + 1;
                txt_designation.Text = "Designation(" + desigcount.ToString() + ")";
            }
        }
        if (desigcount == 0)
        {
            txt_designation.Text = "---Select---";
            chk_Designation.Checked = false;
        }
    }

    protected void chk_Department_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_Department.Checked == true)
        {
            for (int i = 0; i < chklst_Department.Items.Count; i++)
            {
                chklst_Department.Items[i].Selected = true;
                txt_Department.Text = "Department(" + (chklst_Department.Items.Count) + ")";
            }
            panel_Department.Focus();
        }
        else
        {
            for (int i = 0; i < chklst_Department.Items.Count; i++)
            {
                chklst_Department.Items[i].Selected = false;
                txt_Department.Text = "---Select---";
            }
        }
    }

    protected void chklst_Department_SelectedIndexChanged(object sender, EventArgs e)
    {
        panel_Department.Focus();
        int departmentcount = 0;
        for (int i = 0; i < chklst_Department.Items.Count; i++)
        {
            if (chklst_Department.Items[i].Selected == true)
            {
                departmentcount = departmentcount + 1;
                txt_Department.Text = "Department(" + departmentcount.ToString() + ")";
            }
        }

        if (departmentcount == 0)
        {
            txt_Department.Text = "---Select---";
            chk_Department.Checked = false;
        }
    }

    protected void chklst_Category_SelectedIndexChanged(object sender, EventArgs e)
    {
        panel_Category.Focus();
        int category = 0;
        for (int i = 0; i < chklst_Category.Items.Count; i++)
        {
            if (chklst_Category.Items[i].Selected == true)
            {
                category = category + 1;
                txt_Category.Text = "Category (" + category.ToString() + ")";
            }
        }
        if (category == 0)
        {
            txt_Category.Text = "---Select---";
            chk_Category.Checked = false;
        }
    }

    protected void chk_Category_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_Category.Checked == true)
        {
            for (int i = 0; i < chklst_Category.Items.Count; i++)
            {
                chklst_Category.Items[i].Selected = true;
                txt_Category.Text = "Category(" + (chklst_Category.Items.Count) + ")";
            }
            panel_Category.Focus();
        }
        else
        {
            for (int i = 0; i < chklst_Category.Items.Count; i++)
            {
                chklst_Category.Items[i].Selected = false;
                txt_Category.Text = "---Select---";
            }
        }
    }

    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        bindspread();
        txtexcelname.Text = "";
    }

    protected void rdbtnlst_change(object sender, EventArgs e)
    {
        FpPayment.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        lblvalidation1.Visible = false;
        lblvalidation.Visible = false;
        msg.Visible = false;
        msg1.Visible = false;

        if (rdbtnlst.SelectedIndex == 0)
            txtAllowance.Enabled = false;

        else
            txtAllowance.Enabled = true;

        if (rdbtnlst.SelectedIndex == 0)//delsi
        {
            pheaderfilter.Visible = false;
            pheaderfilter1.Visible = true;
            pcolumnorder.Visible = false;
            pcolumnorder1.Visible = true;
        }

        else
        {
            pheaderfilter.Visible = true;
            pheaderfilter1.Visible = false;
            pcolumnorder.Visible = true;
            pcolumnorder1.Visible = false;
        }





    }

    public void bindspread()
    {
        try
        {
            if (ddlcollege.Text != "---Select---")
            {
                if (ddlcollege.Text != "---Select---" || txt_Department.Text != "---Select---" || txt_designation.Text != "---Select---" || txt_stafftype.Text != "---Select---" || txt_Category.Text != "---Select---")
                {
                    msg.Visible = false;
                    string fromdate = "";
                    string todate = "";
                    string bindquery = "";

                    ds.Clear();
                    string datevalue = ddlmonth.SelectedItem.Value;
                    string yearval = Convert.ToString(ddlyear.SelectedItem.Text);
                    string datequery = "";
                    Double ltdamnt = 0;
                    int age = 0;
                    double emp_share_percentage = 0;
                    double employer_share_percentage = 0;
                    double fpf_percentage = 0;
                    string getval = da.GetFunction("select LinkValue from New_InsSettings where LinkName='LTD Settings' and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' and user_code='" + usercode + "'");
                    if (!String.IsNullOrEmpty(getval))
                    {
                        string[] spl = getval.Split('-');
                        if (spl.Length == 1)
                        {
                            Double.TryParse(Convert.ToString(spl[0]), out ltdamnt);
                            age = 0;
                        }
                        else if (spl.Length == 2)
                        {
                            Double.TryParse(Convert.ToString(spl[0]), out ltdamnt);
                            Int32.TryParse(Convert.ToString(spl[1]), out age);
                        }
                    }

                    string emppercent = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Percentage Settings' and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' and user_code='" + usercode + "'");
                    if (!String.IsNullOrEmpty(emppercent) && emppercent != "0")
                    {
                        string[] spl = emppercent.Split('-');
                        Double.TryParse(Convert.ToString(spl[0]), out emp_share_percentage);
                        Double.TryParse(Convert.ToString(spl[1]), out employer_share_percentage);
                        Double.TryParse(Convert.ToString(spl[2]), out fpf_percentage);

                    }
                    if (emppercent == "0")
                    {
                        img_div1.Visible = true;
                        lblsavealert.Visible = true;
                        lblsavealert.Text = "Please set Percentage!";
                        divpercentage.Visible = false;
                        msg.Visible = false;
                        return;
                    }



                    //datequery = "select convert(varchar(20),from_date,111), convert(varchar(20),to_date,111)  from HrPayMonths where college_Code='" + ddlcollege.SelectedItem.Value.ToString() + "' and paymonthNum='" + datevalue.ToString() + "'";
                    //ds = da.select_method_wo_parameter(datequery, "Text");

                    //if (ds.Tables[0].Rows.Count > 0)
                    //{
                    //    fromdate = ds.Tables[0].Rows[0][0].ToString();
                    //    todate = ds.Tables[0].Rows[0][1].ToString();
                    //}
                    //bindquery = "SELECT M.Staff_Code ,Staff_Name, ESI_No, M.BSalary+M.Grade_Pay+DAAmt TotWages,(Cur_LOP+Pre_LOP) LOPDays,M.leavedetail,ISNULL(ESI,0) ESI,ISNULL(ESI_Salary,0) ESI_Salary,(ISNULL(ESI,0)+ISNULL(ESI_Salary,0)) Total_ESI FROM staffcategorizer c, MonthlyPay M,StaffMaster S,StaffTrans T,HrDept_Master D,Desig_Master G WHERE M.Staff_Code = S.Staff_Code AND M.College_Code = S.College_Code AND S.Staff_Code = T.Staff_Code AND S.College_Code = M.College_Code AND T.Dept_Code = D.Dept_Code AND T.Latestrec = 1 AND T.Desig_Code = G.Desig_Code AND M.College_Code = G.CollegeCode  and c.category_code = t.category_code and c.college_code = m.college_code and ISNULL(S.IsESIStaff,0) = 1 and collegecode=" + ddlcollege.SelectedItem.Value.ToString() + " and resign='0' and settled='0' and ISNULL(Discontinue,'0')='0' and M.latestrec='1' and M.PayMonth='" + datevalue + "' and PayYear='" + yearval + "'";
                    if (rdbtnlst.SelectedValue == "ESI")
                    {
                        bindquery = "SELECT M.Staff_Code ,Staff_Name, ESI_No,isnull(UAN_No,'')as UANNumber, M.BSalary, M.BSalary+M.G_PAy+ISNULL(DAWithLOP,0) TotWages,(Cur_LOP+Pre_LOP) LOPDays,M.leavedetail,ISNULL(ESI,0) ESI,ISNULL(ESI_Salary,0) ESI_Salary,(ISNULL(ESI,0)+ISNULL(ESI_Salary,0)) Total_ESI,m.Allowances,d.dept_name FROM staffcategorizer c, MonthlyPay M,StaffMaster S,StaffTrans T,HrDept_Master D,Desig_Master G WHERE M.Staff_Code = S.Staff_Code AND M.College_Code = S.College_Code AND S.Staff_Code = T.Staff_Code AND S.College_Code = M.College_Code AND T.Dept_Code = D.Dept_Code AND T.Latestrec = 1 AND T.Desig_Code = G.Desig_Code AND M.College_Code = G.CollegeCode  and c.category_code = t.category_code and c.college_code = m.college_code and ISNULL(S.IsESIStaff,0) = 1 and collegecode=" + ddlcollege.SelectedItem.Value.ToString() + " and resign='0' and settled='0' and ISNULL(Discontinue,'0')='0' and M.latestrec='1' and M.PayMonth='" + datevalue + "' and PayYear='" + yearval + "'";
                        if (chkincamnt.Checked == false)
                        {
                            // bindquery = bindquery + " and ISNULL(ESI,'0')<>'0'";
                            bindquery = bindquery + " and ISNULL(ESI_Salary,'0')<>'0'";
                        }
                        //else
                        //{
                        //    bindquery = bindquery + "and ISNULL(ESI,'0')!='0'";
                        //}

                    }  //and fdate='" + fromdate + "' and tdate='" + todate + "'
                    if (rdbtnlst.SelectedValue == "PF")//refdelsi1902
                    {
                        // Added by jairam 06-09-2017 Modify Query  Before Modify delsi 1902
                        //bindquery = "  SELECT M.Staff_Code,Staff_Name,PFNumber,M.allowances,(M.BSalary+M.G_PAy) BSalary,ISNULL(DAWithLOP,0) DAAmt ,M.BSalary+M.G_PAy+ISNULL(DAWithLOP,0) TotWages,ISNULL(MPFAmount,0) MPFAmount,PF,PF_Salary,FPF as FPF,CASE WHEN (M.BSalary+M.G_PAy+DAWithLOP) < " + ltdamnt + " THEN (M.BSalary+M.G_PAy+DAWithLOP) ELSE " + ltdamnt + " END LTDWages,(Cur_LOP+Pre_LOP) LOPDays,M.leavedetail,S.appl_no FROM staffcategorizer c, MonthlyPay M,StaffMaster S,StaffTrans T,HrDept_Master D,Desig_Master G WHERE M.Staff_Code = S.Staff_Code AND M.College_Code = S.College_Code  AND S.Staff_Code = T.Staff_Code AND S.College_Code = M.College_Code AND T.Dept_Code = D.Dept_Code AND T.Latestrec = 1 AND T.Desig_Code = G.Desig_Code AND M.College_Code = G.CollegeCode  and c.category_code = t.category_code and ISNULL(S.Is_PF,0)=1 and c.college_code = m.college_code and collegecode=" + ddlcollege.SelectedItem.Value.ToString() + " and resign='0' and settled='0' and ISNULL(Discontinue,'0')='0' and M.latestrec='1' and M.PayMonth='" + datevalue + "' and PayYear='" + yearval + "'";


                        bindquery = "  SELECT M.Staff_Code,Staff_Name,PFNumber,isnull(UAN_No,'')as UANNumber,M.allowances,ISNULL(M.BSalary,0) BSalary, ISNull(M.G_PAy,0) GRADPAY,ISNULL(DAWithLOP,0) DAAmt ,M.BSalary+M.G_PAy+ISNULL(DAWithLOP,0) TotWages,ISNULL(MPFAmount,0) MPFAmount,PF,PF_Salary,FPF as FPF,CASE WHEN (M.BSalary+M.G_PAy+DAWithLOP) < " + ltdamnt + " THEN (M.BSalary+M.G_PAy+DAWithLOP) ELSE " + ltdamnt + " END LTDWages,(Cur_LOP+Pre_LOP) LOPDays,M.leavedetail,S.appl_no FROM staffcategorizer c, MonthlyPay M,StaffMaster S,StaffTrans T,HrDept_Master D,Desig_Master G WHERE M.Staff_Code = S.Staff_Code AND M.College_Code = S.College_Code  AND S.Staff_Code = T.Staff_Code AND S.College_Code = M.College_Code AND T.Dept_Code = D.Dept_Code AND T.Latestrec = 1 AND T.Desig_Code = G.Desig_Code AND M.College_Code = G.CollegeCode  and c.category_code = t.category_code and ISNULL(S.Is_PF,0)=1 and c.college_code = m.college_code and collegecode=" + ddlcollege.SelectedItem.Value.ToString() + " and resign='0' and settled='0' and ISNULL(Discontinue,'0')='0' and M.latestrec='1' and M.PayMonth='" + datevalue + "' and PayYear='" + yearval + "'";
                    } //and fdate='" + fromdate + "' and tdate='" + todate + "'


                    string value = "";
                    string invalue = "";
                    for (int i = 0; i < chklst_Department.Items.Count; i++)
                    {
                        if (chklst_Department.Items[i].Selected == true)
                        {
                            value = chklst_Department.Items[i].Value;
                            if (invalue == "")
                            {
                                invalue = value;
                            }
                            else
                            {
                                invalue = invalue + "'" + "," + "'" + value;
                            }
                        }
                    }
                    if (invalue != "")
                    {
                        bindquery = bindquery + " and t.dept_code in('" + invalue + "')";
                    }

                    string desivalue = "";
                    string desi = "";
                    for (int i = 0; i < chklst_Designation.Items.Count; i++)
                    {
                        if (chklst_Designation.Items[i].Selected == true)
                        {
                            desi = chklst_Designation.Items[i].Value;
                            if (invalue == "")
                            {
                                desivalue = desi;
                            }
                            else
                            {
                                desivalue = desivalue + "'" + "," + "'" + desi;
                            }
                        }
                    }
                    if (desivalue != "")
                    {
                        bindquery = bindquery + " and t.desig_code in('" + desivalue + "')";
                    }

                    string cate = "";
                    string catevalue = "";
                    for (int i = 0; i < chklst_Category.Items.Count; i++)
                    {
                        if (chklst_Category.Items[i].Selected == true)
                        {
                            cate = chklst_Category.Items[i].Value;
                            if (catevalue == "")
                            {
                                catevalue = cate;
                            }
                            else
                            {
                                catevalue = catevalue + "'" + "," + "'" + cate;
                            }
                        }
                    }
                    if (catevalue != "")
                    {
                        bindquery = bindquery + " and t.category_code in('" + catevalue + "')";
                    }

                    string stype = "";
                    string stypevalue = "";
                    for (int i = 0; i < cblstafftype.Items.Count; i++)
                    {
                        if (cblstafftype.Items[i].Selected == true)
                        {
                            stype = cblstafftype.Items[i].Value;
                            if (stypevalue == "")
                            {
                                stypevalue = stype;
                            }
                            else
                            {
                                stypevalue = stypevalue + "'" + "," + "'" + stype;
                            }
                        }
                    }
                    if (stypevalue != "")
                    {
                        bindquery = bindquery + " and t.stftype in('" + stypevalue + "')";
                    }
                    if (bindquery != "")
                    {
                        bindquery = bindquery + " order by D.priority,G.priority,join_date";
                    }
                    ArrayList Allowance = new ArrayList();
                    if (cblallowane.Items.Count > 0)
                    {
                        for (int Cblint = 0; Cblint < cblallowane.Items.Count; Cblint++)
                        {
                            if (cblallowane.Items[Cblint].Selected == true)
                            {
                                Allowance.Add(cblallowane.Items[Cblint].Text);
                            }
                        }
                    }
                    //and m.staff_code ='ADM518'
                    double AbsDays = 0;
                    double.TryParse(Convert.ToString(da.GetFunction("select Abs_Days_Calc from Hr_PaySettings where College_Code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'")), out AbsDays);
                    if (bindquery != "")
                    {
                        string strLeaveDet = string.Empty;
                        if (rdbtnlst.SelectedValue == "ESI")
                        {
                            #region ESI
                            int incre = 0;
                            ht.Clear();
                            ds = da.select_method(bindquery, ht, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                FpPayment.Visible = true;
                                FpPayment.Sheets[0].ColumnHeader.RowCount = 1;
                                FpPayment.Sheets[0].ColumnCount = 13;
                                FpPayment.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                                FpPayment.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
                                FpPayment.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
                                FpPayment.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department Name";
                                FpPayment.Sheets[0].ColumnHeader.Cells[0, 4].Text = "ESI No";
                                FpPayment.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Basic Pay";
                                FpPayment.Sheets[0].ColumnHeader.Cells[0, 6].Text = "SA";
                                FpPayment.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total Wages";
                                FpPayment.Sheets[0].ColumnHeader.Cells[0, 8].Text = "LLP Days";
                                FpPayment.Sheets[0].ColumnHeader.Cells[0, 9].Text = "ESI Employee Share";
                                FpPayment.Sheets[0].ColumnHeader.Cells[0, 10].Text = "ESI Employeer Share";
                                FpPayment.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Total";
                                FpPayment.Sheets[0].ColumnHeader.Cells[0, 12].Text = "UAN Number";//delsi

                                FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                                style2.Font.Size = 14;
                                style2.Font.Name = "Book Antiqua";
                                style2.Font.Bold = true;
                                style2.HorizontalAlign = HorizontalAlign.Center;
                                //style2.ForeColor = Color.Black;
                                //style2.BackColor = Color.AliceBlue;

                                FpPayment.Sheets[0].RowCount = 1;
                                double finalwages = 0;
                                double finallopdays = 0;
                                double final_empshare = 0;
                                double final_employershare = 0;
                                double overalltot = 0;
                                int finalesi = 0;
                                int finalbsalary = 0;
                                int finalesisalary = 0;
                                double final_sa = 0;
                                int finaltotal = 0;
                                double EmpShare = 0;

                                
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    double totwages = 0;
                                    string allowance3 = "";
                                    allowance3 = Convert.ToString(ds.Tables[0].Rows[i]["allowances"]);//delsiref
                                    string[] allowanmce_arr1;
                                    string alowancesplit;
                                    int exatval = 0;
                                    allowanmce_arr1 = allowance3.Split('\\');
                                    double sa = 0;
                                    double tot = 0;
                                    double sa_arr = 0;
                                    double basic_arr = 0;

                                    for (int i_allow = 0; i_allow < allowanmce_arr1.GetUpperBound(0); i_allow++)
                                    {
                                        exatval = allowanmce_arr1.GetUpperBound(0);
                                        alowancesplit = allowanmce_arr1[i_allow];
                                        string[] allowanceda = alowancesplit.Split(';');
                                        if (allowanceda.GetUpperBound(0) >= 3)
                                        {
                                            string das = allowanceda[0];
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

                                            if (ds3 != 0)
                                            {
                                                string saamount = string.Empty;

                                                if (Convert.ToString(allowanceda[0].ToUpper()) == "SA")
                                                {
                                                    saamount = ds3.ToString();
                                                    double.TryParse(saamount, out sa);
                                                    // sa_arr = Math.Round(sa_arr, 0, MidpointRounding.AwayFromZero);
                                                }
                                                //Added by saranya on 5/9/2018
                                                if (Convert.ToString(allowanceda[0].ToUpper()) == "BP ARR")
                                                {
                                                    saamount = ds3.ToString();
                                                    double.TryParse(saamount, out basic_arr);
                                                    //basic_arr = Math.Round(basic_arr, 0, MidpointRounding.AwayFromZero);
                                                }
                                                if (Convert.ToString(allowanceda[0].ToUpper()) == "SA ARR")
                                                {
                                                    saamount = ds3.ToString();
                                                    double.TryParse(saamount, out sa_arr);
                                                    //sa_arr = Math.Round(sa_arr, 0, MidpointRounding.AwayFromZero);
                                                }
                                                ////==================================//
                                            }
                                            //i_allow = allowanmce_arr1.GetUpperBound(0);
                                        }

                                    }
                                    tot = sa + basic_arr + sa_arr;//added by saranya
                                    final_sa = final_sa + sa + basic_arr + sa_arr;//basic_arr + sa_arr added by saranya
                                    final_sa = Math.Round(final_sa, 0, MidpointRounding.AwayFromZero);

                                    strLeaveDet = string.Empty;
                                    EmpShare = 0;
                                    incre++;
                                    FpPayment.Sheets[0].RowCount++;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 0].Text = incre.ToString();
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 0].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 0].Font.Name = "Book Antiqua";

                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 1].Text = ds.Tables[0].Rows[i]["Staff_Code"].ToString();
                                    FpPayment.Sheets[0].Columns[1].Width = 100;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 1].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 1].Font.Name = "Book Antiqua";

                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 2].Text = ds.Tables[0].Rows[i]["Staff_Name"].ToString();
                                    FpPayment.Sheets[0].Columns[2].Width = 300;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 2].HorizontalAlign = HorizontalAlign.Left;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 2].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 2].Font.Name = "Book Antiqua";

                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 3].Text = ds.Tables[0].Rows[i]["dept_name"].ToString();
                                    FpPayment.Sheets[0].Columns[3].Width = 100;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 3].HorizontalAlign = HorizontalAlign.Center;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 3].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 3].Font.Name = "Book Antiqua";

                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 4].Text = ds.Tables[0].Rows[i]["ESI_NO"].ToString();
                                    FpPayment.Sheets[0].Columns[4].Width = 100;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 4].HorizontalAlign = HorizontalAlign.Center;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 4].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 4].Font.Name = "Book Antiqua";

                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 5].Text = ds.Tables[0].Rows[i]["BSalary"].ToString();//delsibasic
                                    FpPayment.Sheets[0].Columns[5].Width = 100;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 5].HorizontalAlign = HorizontalAlign.Center;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 5].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 5].Font.Name = "Book Antiqua";

                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 6].Text = Convert.ToString(sa);//delsibasic
                                    FpPayment.Sheets[0].Columns[6].Width = 100;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 6].HorizontalAlign = HorizontalAlign.Center;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 6].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 6].Font.Name = "Book Antiqua";


                                    Double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["BSalary"]), out EmpShare);
                                    EmpShare = Math.Round(EmpShare, 0, MidpointRounding.AwayFromZero);
                                    totwages = EmpShare + tot;//mosdified by saranya sa as tot
                                    totwages = Math.Round(totwages, 0, MidpointRounding.AwayFromZero);
                                    //string AllowanceString = Convert.ToString(ds.Tables[0].Rows[i]["Allowances"]);
                                    //double AllowanceAmount = 0;
                                    //double GetAmount = 0;
                                    //if (Allowance.Count > 0 && AllowanceString.Trim() != "")
                                    //{
                                    //    string[] FirstSplit = AllowanceString.Split('\\');
                                    //    if (FirstSplit.Length > 0)
                                    //    {
                                    //        for (int FS = 0; FS < FirstSplit.Length; FS++)
                                    //        {
                                    //            if (FirstSplit[FS].Trim() != "")
                                    //            {
                                    //                string[] SecondSplit = FirstSplit[FS].Split(';');
                                    //                if (SecondSplit.Length > 0)
                                    //                {
                                    //                    if (Allowance.Contains(SecondSplit[0].Trim()))
                                    //                    {
                                    //                        string AllowanceValue = SecondSplit[2].Trim();
                                    //                        if (SecondSplit[1].Trim() != "Amount")
                                    //                        {
                                    //                            if (AllowanceValue.Contains('-'))
                                    //                            {
                                    //                                string[] AllowValueSplit = AllowanceValue.Split('-');
                                    //                                if (AllowValueSplit.Length > 1)
                                    //                                {
                                    //                                    double.TryParse(AllowValueSplit[1].Trim(), out GetAmount);
                                    //                                    AllowanceAmount += GetAmount;
                                    //                                }
                                    //                            }
                                    //                            else if (SecondSplit[3].Trim() != "")
                                    //                            {
                                    //                                double.TryParse(SecondSplit[3].Trim(), out GetAmount);
                                    //                                AllowanceAmount += GetAmount;
                                    //                            }
                                    //                        }
                                    //                        else
                                    //                        {
                                    //                            if (AllowanceValue.Contains('-'))
                                    //                            {
                                    //                                string[] AllowValueSplit = AllowanceValue.Split('-');
                                    //                                if (AllowValueSplit.Length > 1)
                                    //                                {
                                    //                                    double.TryParse(AllowValueSplit[0].Trim(), out GetAmount);
                                    //                                    AllowanceAmount += GetAmount;
                                    //                                }
                                    //                            }
                                    //                            else if (SecondSplit[3].Trim() != "")
                                    //                            {
                                    //                                double.TryParse(SecondSplit[3].Trim(), out GetAmount);
                                    //                                AllowanceAmount += GetAmount;
                                    //                            }
                                    //                        }
                                    //                    }
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    //if (AllowanceAmount != 0)
                                    //{
                                    //    EmpShare += AllowanceAmount;
                                    //}
                                    finalwages = finalwages + totwages;
                                    finalwages = Math.Round(finalwages, 0, MidpointRounding.AwayFromZero);

                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 7].Text = Convert.ToString(totwages);
                                    FpPayment.Sheets[0].Columns[7].Width = 100;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 7].HorizontalAlign = HorizontalAlign.Right;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 7].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 7].Font.Name = "Book Antiqua";

                                    strLeaveDet = Convert.ToString(ds.Tables[0].Rows[i]["leavedetail"]);
                                    double LopDays = 0;
                                    double myAbsDays = 0;
                                    if (!String.IsNullOrEmpty(strLeaveDet) && strLeaveDet.Trim() != "0")
                                    {
                                        string[] splLeave = strLeaveDet.Split(';');
                                        if (splLeave.Length >= 7)
                                        {
                                            double.TryParse(splLeave[2], out myAbsDays);
                                            double.TryParse(splLeave[6], out LopDays);
                                            if (AbsDays > 0)
                                            {
                                                myAbsDays *= AbsDays;
                                                LopDays += myAbsDays;
                                            }
                                            else
                                            {
                                                LopDays += myAbsDays;
                                            }
                                        }
                                        if (LopDays > 0)
                                        {
                                            FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 8].Text = Convert.ToString(LopDays);
                                            finallopdays = finallopdays + LopDays;
                                            finallopdays = Math.Round(finallopdays, 0, MidpointRounding.AwayFromZero);
                                        }
                                        else
                                            FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 8].Text = "0";
                                    }
                                    else
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 8].Text = "0";
                                    FpPayment.Sheets[0].Columns[8].Width = 100;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 8].HorizontalAlign = HorizontalAlign.Center;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 8].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 8].Font.Name = "Book Antiqua";
                                    double esi_empshare = 0;
                                    double esi_employershare = 0;
                                    double tot_esishare = 0;

                                    esi_empshare = (1.75 * totwages) / 100;

                                    string empshare = Convert.ToString(esi_empshare);
                                    if (empshare.Contains('.'))
                                    {
                                        string[] split = empshare.Split('.');
                                        if (split[1] != "")
                                        {

                                            string splitval = split[1].Substring(0, 1);
                                            double valcal = 0;
                                            Double.TryParse(splitval, out valcal);
                                            if (valcal < 5)
                                            {
                                                esi_empshare = Math.Round(esi_empshare, 0, MidpointRounding.AwayFromZero);
                                                esi_empshare = esi_empshare + 1;

                                            }
                                        }
                                    }


                                    // Double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["ESI"]), out EmpShare);
                                    esi_empshare = Math.Round(esi_empshare, 0, MidpointRounding.AwayFromZero);
                                    final_empshare = final_empshare + esi_empshare;
                                    final_empshare = Math.Round(final_empshare, 0, MidpointRounding.AwayFromZero);
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 9].Text = Convert.ToString(esi_empshare);
                                    FpPayment.Sheets[0].Columns[9].Width = 100;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 9].HorizontalAlign = HorizontalAlign.Right;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 9].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 9].Font.Name = "Book Antiqua";

                                    esi_employershare = (4.75 * totwages) / 100;

                                    string empershare = Convert.ToString(esi_employershare);
                                    if (empershare.Contains('.'))
                                    {
                                        string[] split = empershare.Split('.');
                                        if (split[1] != "")
                                        {

                                            string splitval = split[1].Substring(0, 1);
                                            double valcal = 0;
                                            Double.TryParse(splitval, out valcal);
                                            if (valcal < 5)
                                            {
                                                esi_employershare = Math.Round(esi_employershare, 0, MidpointRounding.AwayFromZero);
                                                esi_employershare = esi_employershare + 1;

                                            }
                                        }
                                    }


                                    // Double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["ESI_Salary"]), out EmpShare);
                                    esi_employershare = Math.Round(esi_employershare, 0, MidpointRounding.AwayFromZero);
                                    final_employershare = final_employershare + esi_employershare;
                                    final_employershare = Math.Round(final_employershare, 0, MidpointRounding.AwayFromZero);
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 10].Text = Convert.ToString(esi_employershare);
                                    FpPayment.Sheets[0].Columns[10].Width = 100;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 10].HorizontalAlign = HorizontalAlign.Right;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 10].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 10].Font.Name = "Book Antiqua";


                                    tot_esishare = esi_empshare + esi_employershare;
                                    tot_esishare = Math.Round(tot_esishare, 0, MidpointRounding.AwayFromZero);
                                    overalltot = overalltot + tot_esishare;
                                    overalltot = Math.Round(overalltot, 0, MidpointRounding.AwayFromZero);

                                    // Double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["Total_ESI"]), out EmpShare);
                                    // EmpShare = Math.Round(EmpShare, 0, MidpointRounding.AwayFromZero); UANNumber
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 11].Text = Convert.ToString(tot_esishare);
                                    FpPayment.Sheets[0].Columns[11].Width = 100;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 11].HorizontalAlign = HorizontalAlign.Right;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 11].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 11].Font.Name = "Book Antiqua";

                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 12].CellType = cellText;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 12].Text = Convert.ToString(ds.Tables[0].Rows[i]["UANNumber"]);
                                    FpPayment.Sheets[0].Columns[12].Width = 100;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 12].HorizontalAlign = HorizontalAlign.Center;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 12].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 12].Font.Name = "Book Antiqua";

                                    //if (Convert.ToString(ds.Tables[0].Rows[i]["TotWages"].ToString()) != "" && Convert.ToString(ds.Tables[0].Rows[i]["TotWages"].ToString()) != null)
                                    //{
                                    //    double wvalues = Convert.ToDouble(ds.Tables[0].Rows[i]["TotWages"].ToString());
                                    //    int wvalue = Convert.ToInt32(wvalues);
                                    //    if (wvalue != 0)
                                    //    {
                                    //        finalwages = finalwages + wvalue;
                                    //    }
                                    //}
                                    if (Convert.ToString(ds.Tables[0].Rows[i]["BSalary"].ToString()) != "" && Convert.ToString(ds.Tables[0].Rows[i]["BSalary"].ToString()) != null)
                                    {
                                        double bsalary = Convert.ToDouble(ds.Tables[0].Rows[i]["BSalary"].ToString());
                                        if (bsalary != 0)
                                        {
                                            finalbsalary = finalbsalary + Convert.ToInt32(bsalary);
                                        }
                                    }

                                    if (Convert.ToString(ds.Tables[0].Rows[i]["ESI_Salary"].ToString()) != "" && Convert.ToString(ds.Tables[0].Rows[i]["ESI_Salary"].ToString()) != null)
                                    {
                                        double esisalary = Convert.ToDouble(ds.Tables[0].Rows[i]["ESI_Salary"].ToString());
                                        if (esisalary != 0)
                                        {
                                            finalesisalary = finalesisalary + Convert.ToInt32(esisalary);
                                        }
                                    }

                                    if (Convert.ToString(ds.Tables[0].Rows[i]["Total_ESI"].ToString()) != "" && Convert.ToString(ds.Tables[0].Rows[i]["Total_ESI"].ToString()) != null)
                                    {
                                        double totalesi = Convert.ToDouble(ds.Tables[0].Rows[i]["Total_ESI"].ToString());
                                        if (totalesi != 0)
                                        {
                                            finaltotal = finaltotal + Convert.ToInt32(totalesi);
                                        }
                                    }
                                }
                                FpPayment.Sheets[0].RowCount++;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 4].Text = "Total";
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 4].HorizontalAlign = HorizontalAlign.Center;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 4].Font.Bold = true;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 4].Font.Size = FontUnit.Medium;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 4].Font.Name = "Book Antiqua";

                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 5].Text = Convert.ToString(finalbsalary);//basic
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 5].HorizontalAlign = HorizontalAlign.Right;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 5].Font.Bold = true;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 5].Font.Size = FontUnit.Medium;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 5].Font.Name = "Book Antiqua";

                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 6].Text = Convert.ToString(final_sa);//sa
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 6].HorizontalAlign = HorizontalAlign.Right;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 6].Font.Bold = true;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 6].Font.Size = FontUnit.Medium;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 6].Font.Name = "Book Antiqua";

                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 7].Text = Convert.ToString(finalwages);
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 7].HorizontalAlign = HorizontalAlign.Right;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 7].Font.Bold = true;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 7].Font.Size = FontUnit.Medium;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 7].Font.Name = "Book Antiqua";

                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 8].Text = Convert.ToString(finallopdays);
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 8].HorizontalAlign = HorizontalAlign.Center;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 8].Font.Bold = true;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 8].Font.Size = FontUnit.Medium;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 8].Font.Name = "Book Antiqua";

                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 9].Text = Convert.ToString(final_empshare);
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 9].HorizontalAlign = HorizontalAlign.Right;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 9].Font.Bold = true;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 9].Font.Size = FontUnit.Medium;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 9].Font.Name = "Book Antiqua";

                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 10].Text = Convert.ToString(final_employershare);
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 10].HorizontalAlign = HorizontalAlign.Right;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 10].Font.Bold = true;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 10].Font.Size = FontUnit.Medium;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 10].Font.Name = "Book Antiqua";

                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 11].Text = Convert.ToString(overalltot);
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 11].HorizontalAlign = HorizontalAlign.Right;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 11].Font.Bold = true;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 11].Font.Size = FontUnit.Medium;
                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 2, 11].Font.Name = "Book Antiqua";




                                FpPayment.Sheets[0].PageSize = FpPayment.Sheets[0].RowCount - 1;
                                FpPayment.Sheets[0].Columns[1].Visible = false;
                                FpPayment.Sheets[0].Columns[2].Visible = false;
                                FpPayment.Sheets[0].Columns[3].Visible = false;
                                FpPayment.Sheets[0].Columns[4].Visible = false;
                                FpPayment.Sheets[0].Columns[5].Visible = false;
                                FpPayment.Sheets[0].Columns[6].Visible = false;
                                FpPayment.Sheets[0].Columns[7].Visible = false;
                                FpPayment.Sheets[0].Columns[8].Visible = false;
                                FpPayment.Sheets[0].Columns[9].Visible = false;
                                FpPayment.Sheets[0].Columns[10].Visible = false;
                                FpPayment.Sheets[0].Columns[11].Visible = false;
                                FpPayment.Sheets[0].Columns[12].Visible = false;
                                cblcolumnorder.Items[1].Selected = true;
                                cblcolumnorder.Items[2].Selected = true;
                                cblcolumnorder.Items[3].Selected = true;
                                if (cblcolumnorder.Items[0].Selected == true)
                                    FpPayment.Sheets[0].Columns[1].Visible = true;
                                if (cblcolumnorder.Items[1].Selected == true)
                                    FpPayment.Sheets[0].Columns[2].Visible = true;
                                if (cblcolumnorder.Items[2].Selected == true)
                                    FpPayment.Sheets[0].Columns[3].Visible = true;
                                if (cblcolumnorder.Items[3].Selected == true)
                                    FpPayment.Sheets[0].Columns[4].Visible = true;
                                if (cblcolumnorder.Items[4].Selected == true)
                                    FpPayment.Sheets[0].Columns[5].Visible = true;
                                if (cblcolumnorder.Items[5].Selected == true)
                                    FpPayment.Sheets[0].Columns[6].Visible = true;
                                if (cblcolumnorder.Items[6].Selected == true)
                                    FpPayment.Sheets[0].Columns[7].Visible = true;
                                if (cblcolumnorder.Items[7].Selected == true)
                                    FpPayment.Sheets[0].Columns[8].Visible = true;
                                if (cblcolumnorder.Items[8].Selected == true)
                                    FpPayment.Sheets[0].Columns[9].Visible = true;
                                if (cblcolumnorder.Items[9].Selected == true)
                                    FpPayment.Sheets[0].Columns[10].Visible = true;
                                if (cblcolumnorder.Items[10].Selected == true)
                                    FpPayment.Sheets[0].Columns[11].Visible = true;
                                if (cblcolumnorder.Items[11].Selected == true)//delsi
                                    FpPayment.Sheets[0].Columns[12].Visible = true;



                                lblrptname.Visible = true;
                                txtexcelname.Visible = true;
                                btnxl.Visible = true;
                                btnprintmaster.Visible = true;
                                msg.Visible = false;
                                lblvalidation1.Visible = false;
                                FpPayment.Visible = true;
                                FpPayment.Sheets[0].Visible = true;
                                lblvalidation.Visible = false;
                            }
                            else
                            {
                                msg.Visible = true;
                                msg.Text = "No Records Found";
                                lblvalidation.Visible = false;
                                lblrptname.Visible = false;
                                txtexcelname.Visible = false;
                                btnxl.Visible = false;
                                btnprintmaster.Visible = false;
                                FpPayment.Visible = false;
                                FpPayment.Sheets[0].Visible = false;
                                lblvalidation1.Visible = false;
                            }
                            #endregion
                        }
                        else
                        {
                            if (rdbtnlst.SelectedValue == "PF")
                            {
                                #region PF Calculation
                                int esivalue = 0;
                                ht.Clear();
                                ds = da.select_method(bindquery, ht, "Text");

                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    FpPayment.Visible = true;
                                    FpPayment.Sheets[0].ColumnHeader.RowCount = 1;
                                    FpPayment.Sheets[0].ColumnCount = 15;
                                    FpPayment.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                                    FpPayment.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
                                    FpPayment.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
                                    FpPayment.Sheets[0].ColumnHeader.Cells[0, 3].Text = "EPF Number";
                                    FpPayment.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Basic Pay";
                                    FpPayment.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Grade Pay";
                                    FpPayment.Sheets[0].ColumnHeader.Cells[0, 6].Text = "DA";
                                    FpPayment.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total Wages";
                                    FpPayment.Sheets[0].ColumnHeader.Cells[0, 8].Text = "LLP Days";
                                    FpPayment.Sheets[0].ColumnHeader.Cells[0, 9].Text = "MPF Amount";
                                    FpPayment.Sheets[0].ColumnHeader.Cells[0, 10].Text = "EPF Employee Share";
                                    FpPayment.Sheets[0].ColumnHeader.Cells[0, 11].Text = "EPF Employeer Share";
                                    FpPayment.Sheets[0].ColumnHeader.Cells[0, 12].Text = "FPF";
                                    FpPayment.Sheets[0].ColumnHeader.Cells[0, 13].Text = "LTD Wages";
                                    FpPayment.Sheets[0].ColumnHeader.Cells[0, 14].Text = "UAN Number";
                                    FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                                    style2.Font.Size = 13;
                                    style2.Font.Name = "Book Antiqua";
                                    style2.Font.Bold = true;
                                    style2.HorizontalAlign = HorizontalAlign.Center;
                                    string appl_no = "";
                                    int stfage = 0;

                                    FpPayment.Sheets[0].RowCount = 0;

                                    int finaldatt = 0;
                                    int finalbsalary = 0;
                                    int finalgradpay = 0;
                                    int finaltwages = 0;
                                    int finalmpf = 0;
                                    int finalpf = 0;
                                    int finalsalary = 0;
                                    int finalfpf = 0;
                                    double finalemployershare = 0;
                                    double finalempshare = 0;
                                    int finalldwages = 0;
                                    double finallopdays = 0;
                                    double EmpShare = 0;
                                    double BSalary = 0;
                                    double MPFAmount = 0;
                                    double EMPSalary = 0;
                                    double FPFAmount = 0;
                                    double EMPSalaryShare = 0;
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        strLeaveDet = string.Empty;
                                        EmpShare = 0;
                                        appl_no = "";
                                        stfage = 0;
                                        string allowance3 = "";
                                        if (age != 0)
                                        {
                                            appl_no = Convert.ToString(ds.Tables[0].Rows[i]["appl_no"]);
                                            Int32.TryParse(da.GetFunction("select DATEDIFF(YEAR,date_of_birth,GETDATE()),date_of_birth  from staff_appl_master where appl_no='" + appl_no + "'"), out stfage);
                                        }
                                        allowance3 = Convert.ToString(ds.Tables[0].Rows[i]["allowances"]);//delsiref
                                        string[] allowanmce_arr1;
                                        string alowancesplit;
                                        int exatval = 0;
                                        allowanmce_arr1 = allowance3.Split('\\');
                                        double basic_arr = 0;
                                        double gp_arr = 0;
                                        double da_arr = 0;
                                        double tot = 0;
                                        for (int i_allow = 0; i_allow < allowanmce_arr1.GetUpperBound(0); i_allow++)
                                        {
                                            exatval = allowanmce_arr1.GetUpperBound(0);
                                            alowancesplit = allowanmce_arr1[i_allow];
                                            string[] allowanceda = alowancesplit.Split(';');
                                            if (allowanceda.GetUpperBound(0) >= 3)
                                            {
                                                string das = allowanceda[0];
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

                                                if (ds3 != 0)
                                                {
                                                    string alowamount = string.Empty;


                                                    if (Convert.ToString(allowanceda[0].ToUpper()) == "BP ARR")
                                                    {
                                                        alowamount = ds3.ToString();
                                                        double.TryParse(alowamount, out basic_arr);
                                                        basic_arr = Math.Round(basic_arr, 0, MidpointRounding.AwayFromZero);
                                                    }
                                                    if (Convert.ToString(allowanceda[0].ToUpper()) == "GP ARR")
                                                    {
                                                        alowamount = ds3.ToString();
                                                        double.TryParse(alowamount, out gp_arr);
                                                        gp_arr = Math.Round(gp_arr, 0, MidpointRounding.AwayFromZero);
                                                    }
                                                    if (Convert.ToString(allowanceda[0].ToUpper()) == "DA ARR")
                                                    {
                                                        alowamount = ds3.ToString();
                                                        double.TryParse(alowamount, out da_arr);
                                                        da_arr = Math.Round(da_arr, 0, MidpointRounding.AwayFromZero);
                                                    }
                                                    //  alowamount = ds3.ToString();
                                                    // alowamount = String.Format("{0:0.00}", alowamount);
                                                    //  double myValue = 0;
                                                    //  double.TryParse(alowamount, out myValue);
                                                    // myValue = Math.Round(myValue, 0, MidpointRounding.AwayFromZero);

                                                }
                                                //i_allow = allowanmce_arr1.GetUpperBound(0);
                                            }
                                            tot = basic_arr + gp_arr + da_arr;
                                        }
                                        double arr_tot;
                                        esivalue++;
                                        FpPayment.Sheets[0].RowCount++;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 0].Text = esivalue.ToString();
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";


                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["Staff_Code"].ToString();
                                        FpPayment.Sheets[0].Columns[1].Width = 100;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";


                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["Staff_Name"].ToString();
                                        FpPayment.Sheets[0].Columns[2].Width = 300;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";


                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["PFNumber"].ToString();
                                        FpPayment.Sheets[0].Columns[3].Width = 100;

                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                        Double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["BSalary"]), out EmpShare);
                                        EmpShare = Math.Round(EmpShare, 0, MidpointRounding.AwayFromZero);
                                        BSalary = EmpShare; // Added by jairam 06-09-2017
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(EmpShare);
                                        FpPayment.Sheets[0].Columns[4].Width = 100;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                        Double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["GRADPAY"]), out EmpShare);//delsi1902
                                        EmpShare = Math.Round(EmpShare, 0, MidpointRounding.AwayFromZero);

                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(EmpShare);
                                        FpPayment.Sheets[0].Columns[5].Width = 100;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                                        Double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["DAAmt"]), out EmpShare);
                                        EmpShare = Math.Round(EmpShare, 0, MidpointRounding.AwayFromZero);
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(EmpShare);
                                        FpPayment.Sheets[0].Columns[6].Width = 100;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                        Double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["TotWages"]), out EmpShare);
                                        arr_tot = EmpShare + tot;
                                        arr_tot = Math.Round(arr_tot, 0, MidpointRounding.AwayFromZero);
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(arr_tot);
                                        FpPayment.Sheets[0].Columns[7].Width = 100;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                                        strLeaveDet = Convert.ToString(ds.Tables[0].Rows[i]["leavedetail"]);
                                        string LopDaysCheck = Convert.ToString(ds.Tables[0].Rows[i]["LOPDays"]);
                                        double LopDays = 0;
                                        double myAbsDays = 0;
                                        if (!String.IsNullOrEmpty(strLeaveDet) && strLeaveDet.Trim() != "0")
                                        {
                                            string[] splLeave = strLeaveDet.Split(';');
                                            if (splLeave.Length >= 7)
                                            {
                                                double.TryParse(splLeave[2], out myAbsDays);
                                                double.TryParse(splLeave[6], out LopDays);
                                                if (AbsDays > 0)
                                                {
                                                    myAbsDays *= AbsDays;
                                                    LopDays += myAbsDays;
                                                }
                                                else
                                                {
                                                    LopDays += myAbsDays;
                                                }
                                            }
                                            if (LopDays > 0)
                                            {
                                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(LopDays);
                                                finallopdays = finallopdays + LopDays;
                                                finallopdays = Math.Round(finallopdays, 0, MidpointRounding.AwayFromZero);
                                            }
                                            else
                                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 8].Text = "0";
                                        }
                                        else if (LopDaysCheck.Trim() != "" && LopDaysCheck.Trim() != "0")
                                        {
                                            double.TryParse(LopDaysCheck, out myAbsDays);
                                            LopDays += myAbsDays;
                                            if (LopDays > 0)
                                            {
                                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(LopDays);
                                                finallopdays = finallopdays + LopDays;
                                                finallopdays = Math.Round(finallopdays, 0, MidpointRounding.AwayFromZero);
                                            }
                                            else
                                                FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 8].Text = "0";
                                        }
                                        else
                                            FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 8].Text = "0";

                                        FpPayment.Sheets[0].Columns[8].Width = 100;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                                        Double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["MPFAmount"]), out EmpShare);
                                        EmpShare = Math.Round(EmpShare, 0, MidpointRounding.AwayFromZero);
                                        //if (BSalary == 0) // Added by jairam 06-09-2017
                                        //{
                                        //    EmpShare = 0;
                                        //}
                                        MPFAmount = EmpShare;

                                        if (arr_tot > EmpShare)//delsi
                                        {
                                            FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(EmpShare);
                                            FpPayment.Sheets[0].Columns[9].Width = 100;
                                            FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Right;
                                            FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                            FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                                        }
                                        else
                                        {
                                            FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(arr_tot);
                                            FpPayment.Sheets[0].Columns[9].Width = 100;
                                            FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Right;
                                            FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                            FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                                        }
                                        double epfempshare = 0;
                                        epfempshare = (emp_share_percentage * arr_tot) / 100;
                                        //   Double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["PF"]), out EmpShare);
                                        epfempshare = Math.Round(epfempshare, 0, MidpointRounding.AwayFromZero);
                                        //   EMPSalary = EmpShare;
                                        finalempshare = finalempshare + epfempshare;
                                        finalempshare = Math.Round(finalempshare, 0, MidpointRounding.AwayFromZero);
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(epfempshare);//delsi2302
                                        FpPayment.Sheets[0].Columns[9].Width = 100;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Right;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";

                                        if ((age != 0 && stfage <= age) && ltdamnt != 0 && BSalary != 0) // Added by delsi1902
                                        {
                                            Double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["LTDWages"]), out EmpShare);
                                            EmpShare = Math.Round(EmpShare, 0, MidpointRounding.AwayFromZero);

                                        }
                                        else
                                        {
                                            EmpShare = 0;
                                        }
                                        double fpf = 0;
                                        if (EmpShare != 0)
                                        {
                                            fpf = (fpf_percentage * EmpShare) / 100;

                                            // Double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["FPF"]), out EmpShare);
                                            fpf = Math.Round(fpf, 0, MidpointRounding.AwayFromZero);
                                            //FPFAmount = EmpShare;
                                        }

                                        finalfpf = finalfpf + Convert.ToInt32(fpf);
                                        double employershare = 0;
                                        if (fpf != 0)
                                        {
                                            double empmpf = 0;

                                            Double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["MPFAmount"]), out EmpShare);
                                            EmpShare = Math.Round(EmpShare, 0, MidpointRounding.AwayFromZero);
                                            if (arr_tot > EmpShare)//delsi
                                            {
                                                empmpf = EmpShare;

                                            }
                                            else
                                            {
                                                empmpf = arr_tot;
                                            }
                                            //Double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["PF_Salary"]), out EmpShare);
                                            employershare = (((employer_share_percentage * empmpf) / 100) - fpf);
                                        }
                                        else
                                        {
                                            double empmpf = 0;

                                            Double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["MPFAmount"]), out EmpShare);
                                            EmpShare = Math.Round(EmpShare, 0, MidpointRounding.AwayFromZero);
                                            if (arr_tot > EmpShare)//delsi
                                            {
                                                empmpf = EmpShare;

                                            }
                                            else
                                            {
                                                empmpf = arr_tot;
                                            }
                                            employershare = (employer_share_percentage * empmpf) / 100;

                                        }
                                        finalemployershare = finalemployershare + employershare;
                                        finalemployershare = Math.Round(finalemployershare, 0, MidpointRounding.AwayFromZero);
                                        employershare = Math.Round(employershare, 0, MidpointRounding.AwayFromZero);
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(employershare);
                                        FpPayment.Sheets[0].Columns[11].Width = 100;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Right;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";


                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(fpf);
                                        FpPayment.Sheets[0].Columns[12].Width = 100;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Right;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 12].Font.Size = FontUnit.Medium;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 12].Font.Name = "Book Antiqua";

                                        if ((age != 0 && stfage <= age) && ltdamnt != 0 && BSalary != 0) // Added by jairam 06-09-2017
                                        {
                                            Double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["LTDWages"]), out EmpShare);
                                            EmpShare = Math.Round(EmpShare, 0, MidpointRounding.AwayFromZero);
                                            FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 13].Text = Convert.ToString(EmpShare);
                                        }
                                        else
                                        {
                                            FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 13].Text = "0";
                                        }
                                        FpPayment.Sheets[0].Columns[13].Width = 100;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 13].HorizontalAlign = HorizontalAlign.Right;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 13].Font.Size = FontUnit.Medium;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 13].Font.Name = "Book Antiqua";

                                        FpPayment.Sheets[0].Columns[14].Width = 100;//delsi
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 14].CellType = cellText;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 14].Text = Convert.ToString(ds.Tables[0].Rows[i]["UANNumber"]);//delsi
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 14].HorizontalAlign = HorizontalAlign.Center;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 14].Font.Size = FontUnit.Medium;
                                        FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 14].Font.Name = "Book Antiqua";

                                        //delsi
                                        if (Convert.ToString(ds.Tables[0].Rows[i]["GRADPAY"].ToString()) != "" && Convert.ToString(ds.Tables[0].Rows[i]["GRADPAY"].ToString()) != null)
                                        {
                                            double gradepay = Convert.ToDouble(ds.Tables[0].Rows[i]["GRADPAY"].ToString());
                                            if (gradepay != 0)
                                            {
                                                finalgradpay = finalgradpay + Convert.ToInt32(gradepay);
                                            }
                                        }


                                        if (Convert.ToString(ds.Tables[0].Rows[i]["DAAmt"].ToString()) != "" && Convert.ToString(ds.Tables[0].Rows[i]["DAAmt"].ToString()) != null)
                                        {
                                            double daatvalue = Convert.ToDouble(ds.Tables[0].Rows[i]["DAAmt"].ToString());
                                            if (daatvalue != 0)
                                            {
                                                finaldatt = finaldatt + Convert.ToInt32(daatvalue);
                                            }
                                        }
                                        if (Convert.ToString(MPFAmount) != "" && Convert.ToString(MPFAmount) != null)
                                        {
                                            double mpfvalue = Convert.ToDouble(MPFAmount);
                                            if (mpfvalue != 0)
                                            {
                                                finalmpf = finalmpf + Convert.ToInt32(mpfvalue);
                                            }
                                        }
                                        if (Convert.ToString(ds.Tables[0].Rows[i]["TotWages"].ToString()) != "" && Convert.ToString(ds.Tables[0].Rows[i]["TotWages"].ToString()) != null)
                                        {
                                            double totwvalue = Convert.ToDouble(ds.Tables[0].Rows[i]["TotWages"].ToString());
                                            if (totwvalue != 0)
                                            {
                                                finaltwages = finaltwages + Convert.ToInt32(totwvalue);
                                            }
                                        }
                                        if (Convert.ToString(ds.Tables[0].Rows[i]["PF"].ToString()) != "" && Convert.ToString(ds.Tables[0].Rows[i]["PF"].ToString()) != null)
                                        {
                                            double pfvalue = Convert.ToDouble(ds.Tables[0].Rows[i]["PF"].ToString());
                                            if (pfvalue != 0)
                                            {
                                                finalpf = finalpf + Convert.ToInt32(pfvalue);
                                            }
                                        }

                                        //if (Convert.ToString(EMPSalaryShare) != "" && Convert.ToString(EMPSalaryShare) != null)
                                        //{
                                        //    double salaryvalue = Convert.ToDouble(EMPSalaryShare);
                                        //    if (salaryvalue != 0)
                                        //    {
                                        //        finalsalary = finalsalary + Convert.ToInt32(salaryvalue);
                                        //    }
                                        //}

                                        //if (Convert.ToString(ds.Tables[0].Rows[i]["FPF"].ToString()) != "" && Convert.ToString(ds.Tables[0].Rows[i]["FPF"].ToString()) != null)
                                        //{

                                        //    double fpfvalue = Convert.ToDouble(ds.Tables[0].Rows[i]["FPF"].ToString());
                                        //    if (fpfvalue != 0)
                                        //    {
                                        //        finalfpf = finalfpf + Convert.ToInt32(fpfvalue);
                                        //    }
                                        //}

                                        if (Convert.ToString(FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 13].Text) != "" && Convert.ToString(FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 13].Text) != null)
                                        {
                                            double ltdvalue = Convert.ToDouble(FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 13].Text);
                                            if (ltdvalue != 0)
                                            {
                                                finalldwages = finalldwages + Convert.ToInt32(ltdvalue);
                                            }
                                        }
                                    }
                                    FpPayment.Sheets[0].RowCount++;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 3].Text = "Total";
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(finalbsalary);
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(finalgradpay);//delsi
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(finaldatt);
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(finaltwages);
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 7].Font.Bold = true;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(finallopdays);
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 8].Font.Bold = true;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(finalmpf);
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Right;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 9].Font.Bold = true;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(finalempshare);
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Right;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 10].Font.Bold = true;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";

                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(finalemployershare);
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Right;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 11].Font.Bold = true;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";

                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(finalfpf);
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Right;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 12].Font.Bold = true;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 12].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 12].Font.Name = "Book Antiqua";

                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 13].Text = Convert.ToString(finalldwages);
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 13].HorizontalAlign = HorizontalAlign.Right;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 13].Font.Bold = true;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 13].Font.Size = FontUnit.Medium;
                                    FpPayment.Sheets[0].Cells[FpPayment.Sheets[0].RowCount - 1, 13].Font.Name = "Book Antiqua";


                                    FpPayment.SaveChanges();


                                    FpPayment.Sheets[0].Columns[1].Visible = false;
                                    FpPayment.Sheets[0].Columns[2].Visible = false;
                                    FpPayment.Sheets[0].Columns[3].Visible = false;
                                    FpPayment.Sheets[0].Columns[4].Visible = false;
                                    FpPayment.Sheets[0].Columns[5].Visible = false;
                                    FpPayment.Sheets[0].Columns[6].Visible = false;
                                    FpPayment.Sheets[0].Columns[7].Visible = false;
                                    FpPayment.Sheets[0].Columns[8].Visible = false;
                                    FpPayment.Sheets[0].Columns[9].Visible = false;
                                    FpPayment.Sheets[0].Columns[10].Visible = false;
                                    FpPayment.Sheets[0].Columns[11].Visible = false;
                                    FpPayment.Sheets[0].Columns[12].Visible = false;
                                    FpPayment.Sheets[0].Columns[13].Visible = false;
                                    FpPayment.Sheets[0].Columns[14].Visible = false;

                                    cblcolumnorder1.Items[1].Selected = true;
                                    cblcolumnorder1.Items[2].Selected = true;
                                    cblcolumnorder1.Items[9].Selected = true;



                                    if (cblcolumnorder1.Items[0].Selected == true)
                                        FpPayment.Sheets[0].Columns[1].Visible = true;
                                    if (cblcolumnorder1.Items[1].Selected == true)
                                        FpPayment.Sheets[0].Columns[2].Visible = true;
                                    if (cblcolumnorder1.Items[2].Selected == true)
                                        FpPayment.Sheets[0].Columns[3].Visible = true;
                                    if (cblcolumnorder1.Items[3].Selected == true)
                                        FpPayment.Sheets[0].Columns[4].Visible = true;
                                    if (cblcolumnorder1.Items[4].Selected == true)
                                        FpPayment.Sheets[0].Columns[5].Visible = true;
                                    if (cblcolumnorder1.Items[5].Selected == true)
                                        FpPayment.Sheets[0].Columns[6].Visible = true;
                                    if (cblcolumnorder1.Items[6].Selected == true)
                                        FpPayment.Sheets[0].Columns[7].Visible = true;
                                    if (cblcolumnorder1.Items[7].Selected == true)
                                        FpPayment.Sheets[0].Columns[8].Visible = true;

                                    if (cblcolumnorder1.Items[8].Selected == true)
                                        FpPayment.Sheets[0].Columns[9].Visible = true;
                                    if (cblcolumnorder1.Items[9].Selected == true)
                                        FpPayment.Sheets[0].Columns[10].Visible = true;
                                    if (cblcolumnorder1.Items[10].Selected == true)
                                        FpPayment.Sheets[0].Columns[11].Visible = true;
                                    if (cblcolumnorder1.Items[11].Selected == true)
                                        FpPayment.Sheets[0].Columns[12].Visible = true;
                                    if (cblcolumnorder1.Items[12].Selected == true)
                                        FpPayment.Sheets[0].Columns[13].Visible = true;
                                    if (cblcolumnorder1.Items[13].Selected == true)
                                        FpPayment.Sheets[0].Columns[14].Visible = true;



                                    FpPayment.Sheets[0].PageSize = FpPayment.Sheets[0].RowCount;

                                    lblvalidation.Visible = false;
                                    lblrptname.Visible = true;
                                    txtexcelname.Visible = true;
                                    btnxl.Visible = true;
                                    btnprintmaster.Visible = true;
                                    lblvalidation1.Visible = false;
                                    FpPayment.Sheets[0].Visible = true;
                                    FpPayment.Visible = true;
                                }
                                else
                                {
                                    msg.Visible = true;
                                    msg.Text = "No Records Found";
                                    FpPayment.Visible = false;
                                    lblvalidation.Visible = false;
                                    lblrptname.Visible = false;
                                    txtexcelname.Visible = false;
                                    btnxl.Visible = false;
                                    btnprintmaster.Visible = false;
                                    lblvalidation1.Visible = false;
                                }
                                #endregion
                            }
                        }
                    }
                }
                else
                {
                    msg.Text = "Please Select Any One Field";
                    lblvalidation.Visible = false;
                    msg.Visible = true;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    FpPayment.Visible = false;
                    lblvalidation1.Visible = false;
                }
            }
            else
            {
                msg.Text = "Please Select Any One College";
                lblvalidation.Visible = false;
                msg.Visible = true;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                FpPayment.Visible = false;
                lblvalidation1.Visible = false;
            }
        }
        catch (Exception ex)
        {
            // da.sendErrorMail(ex, Convert.ToString(ddlcollege.SelectedItem.Value), "PaymentCal.aspx");
        }
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string print = "";
            string appPath = HttpContext.Current.Server.MapPath("~");
            string strexcelname = "";
            if (appPath != "")
            {
                strexcelname = txtexcelname.Text;
                appPath = appPath.Replace("\\", "/");
                if (strexcelname != "")
                {
                    print = strexcelname;
                    string szPath = appPath + "/Report/";
                    string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")

                    FpPayment.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Flush();
                    Response.WriteFile(szPath + szFile);
                }
                else
                {
                    lblvalidation1.Visible = true;
                    FpPayment.Visible = true;
                    txtexcelname.Focus();
                }
            }
        }
        catch (Exception ex) { }
    }

    protected void btnprintmaster_Clcik(object sender, EventArgs e)
    {
        if (rdbtnlst.Text == "PF")
        {
            lblvalidation1.Visible = false;
            lblvalidation.Visible = false;

            string Month = "";
            Month = "@" + "From Month & Year - " + ddlyear.SelectedItem.Text + " " + "&" + " " + ddlmonth.SelectedItem.Text;
            string pagename = "PaymentCal.aspx";
            string header = "PF Detailed Report" + Month;
            Printcontrol.loadspreaddetails(FpPayment, pagename, header);
            Printcontrol.Visible = true;
        }
        else
        {
            lblvalidation.Visible = false;
            lblvalidation1.Visible = false;
            string Month = "";
            Month = "@" + "From Month & Year - " + ddlyear.SelectedItem.Text + " " + "&" + " " + ddlmonth.SelectedItem.Text;
            string pagename = "PaymentCal.aspx";
            string header = "ESI Detailed Report" + Month;
            Printcontrol.loadspreaddetails(FpPayment, pagename, header);
            Printcontrol.Visible = true;
        }
    }

    protected void ltd_set_click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = true;
        if (Convert.ToString(ddlcollege.SelectedItem.Text).Trim() != "---Select---")
        {
            string selq = "Select LinkValue from New_InsSettings where LinkName='LTD Settings' and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' and user_code='" + usercode + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]).Trim()) && Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]).Trim() != "0")
                {
                    string[] spl = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]).Split('-');
                    if (spl.Length == 1)
                    {
                        txtltdwages.Text = Convert.ToString(spl[0]);
                        txtage.Text = "";
                    }
                    else if (spl.Length == 2)
                    {
                        txtltdwages.Text = Convert.ToString(spl[0]);
                        txtage.Text = Convert.ToString(spl[1]);
                    }
                }
                else
                {
                    txtltdwages.Text = "";
                    txtage.Text = "";
                }
            }
            else
            {
                txtltdwages.Text = "";
                txtage.Text = "";
            }
        }
        else
        {
            alertpopwindow.Visible = false;
            msg.Visible = true;
            msg.Text = "Please Select Any College!";
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            string linkvalue = "";
            Double ltdamnt = 0;
            Double age = 0;
            Double.TryParse(Convert.ToString(txtltdwages.Text), out ltdamnt);
            if (ddlcollege.SelectedItem.Text != "---Select---")
            {
                if (ltdamnt != null)
                {
                    if (txtage.Text.Trim() != "")
                    {
                        Double.TryParse(Convert.ToString(txtage.Text.Trim()), out age);
                        if (age != null)
                            linkvalue = Convert.ToString(ltdamnt) + "-" + Convert.ToString(age);
                        else
                            linkvalue = Convert.ToString(ltdamnt);
                    }
                    else
                    {
                        linkvalue = Convert.ToString(ltdamnt);
                    }
                    string insq = "if exists(Select * from New_InsSettings where LinkName='LTD Settings' and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' and user_code='" + usercode + "') update New_InsSettings set LinkValue='" + Convert.ToString(linkvalue) + "' where LinkName='LTD Settings' and user_code='" + usercode + "' and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code) Values ('LTD Settings','" + Convert.ToString(linkvalue) + "','" + usercode + "','" + Convert.ToString(ddlcollege.SelectedItem.Value) + "')";
                    int inscount = da.update_method_wo_parameter(insq, "Text");
                    if (inscount > 0)
                    {
                        img_div1.Visible = true;
                        lblsavealert.Visible = true;
                        lblsavealert.Text = "Saved Successfully!";
                        alertpopwindow.Visible = false;
                        msg.Visible = false;
                    }
                }
                else
                {
                    msg.Visible = true;
                    msg.Text = "Please Enter the LTD Amount!";
                    alertpopwindow.Visible = false;
                }
            }
            else
            {
                msg.Visible = true;
                msg.Text = "Please Select the College!";
                alertpopwindow.Visible = false;
            }
        }
        catch { }
    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        img_div1.Visible = false;
    }

    protected void cblallowane_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            chklstchange(cballowance, cblallowane, txtAllowance, "Allowance");
        }
        catch
        {
        }
    }
    protected void cballowance_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            chkchange(cballowance, cblallowane, txtAllowance, "Allowance");
        }
        catch
        {

        }
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
                    count = count + 1;
            }
            if (count > 0)
            {
                txtchange.Text = label + "(" + count + ")";
                if (count == chklstchange.Items.Count)
                    chkchange.Checked = true;
            }
        }
        catch { }
    }






    protected void loadallowance()
    {
        try
        {
            ds.Clear();
            cblallowane.Items.Clear();
            if (ddlcollege.SelectedItem.Text != "---Select---")
            {
                string item = "select allowances from incentives_master where college_code='" + ddlcollege.SelectedValue + "'";
                ds = da.select_method_wo_parameter(item, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblallowane.DataSource = ds;
                    string st = Convert.ToString(ds.Tables[0].Rows[0]["allowances"]);
                    string[] split = st.Split(';');
                    for (int row = 0; row < split.Length; row++)
                    {
                        string staff = split[row];
                        string[] split1 = staff.Split('\\');
                        string stafftype = split1[0];
                        if (stafftype.Trim() != "")
                            cblallowane.Items.Add(stafftype);
                    }
                    txtAllowance.Text = "--Select--";
                    cballowance.Checked = false;
                }
            }

        }
        catch { }
    }
    /// <summary>
    /// delsi
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void lb_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder.ClearSelection();
            cb_column.Checked = false;
            lnk_columnorder.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }
    public void lb_Click1(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder1.ClearSelection();
            cb_column1.Checked = false;
            lnk_columnorder1.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    public void cb_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_column.Checked == true)
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                }
                lnk_columnorder.Visible = true;
            }
            else
            {
                cblcolumnorder.ClearSelection();
            }
        }
        catch { }
    }
    public void cb_column_CheckedChanged1(object sender, EventArgs e)
    {
        try
        {
            if (cb_column1.Checked == true)
            {
                for (int i = 0; i < cblcolumnorder1.Items.Count; i++)
                {
                    cblcolumnorder1.Items[i].Selected = true;
                    lnk_columnorder1.Visible = true;
                }
                lnk_columnorder1.Visible = true;
            }
            else
            {
                cblcolumnorder1.ClearSelection();
            }
        }
        catch { }
    }
    protected void btn_exit_Click(object sender, EventArgs e)//delsi26
    {
        divpercentage.Visible = false;
    }

    protected void btnsavePercentage_Click(object sender, EventArgs e)
    {
        try
        {
            string linkvalue = "";
            Double epf_employeeShare = 0;
            Double epf_employeerShare = 0;
            Double fpfPercent = 0;
            Double.TryParse(Convert.ToString(txt_empshare.Text), out epf_employeeShare);
            Double.TryParse(Convert.ToString(txt_employershare.Text), out epf_employeerShare);
            Double.TryParse(Convert.ToString(txt_fpf.Text), out fpfPercent);
            if (ddlcollege.SelectedItem.Text != "---Select---")
            {
                if (epf_employeeShare != null && epf_employeerShare != null && fpfPercent != null)
                {


                    linkvalue = Convert.ToString(epf_employeeShare) + "-" + Convert.ToString(epf_employeerShare) + "-" + Convert.ToString(fpfPercent);


                    string insq = "if exists(Select * from New_InsSettings where LinkName='Percentage Settings' and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' and user_code='" + usercode + "') update New_InsSettings set LinkValue='" + Convert.ToString(linkvalue) + "' where LinkName='Percentage Settings' and user_code='" + usercode + "' and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code) Values ('Percentage Settings','" + Convert.ToString(linkvalue) + "','" + usercode + "','" + Convert.ToString(ddlcollege.SelectedItem.Value) + "')";
                    int inscount = da.update_method_wo_parameter(insq, "Text");
                    if (inscount > 0)
                    {
                        img_div1.Visible = true;
                        lblsavealert.Visible = true;
                        lblsavealert.Text = "Saved Successfully!";
                        divpercentage.Visible = false;
                        msg.Visible = false;
                    }
                }
                else
                {
                    msg.Visible = true;
                    msg.Text = "Please Enter the  Percentage!";
                    divpercentage.Visible = false;
                }
            }
            else
            {
                msg.Visible = true;
                msg.Text = "Please Select the College!";
                alertpopwindow.Visible = false;
            }
        }
        catch { }
    }

    protected void Percentage_set_click(object sender, EventArgs e)
    {
        divpercentage.Visible = true;
        if (Convert.ToString(ddlcollege.SelectedItem.Text).Trim() != "---Select---")
        {
            string selq = "Select LinkValue from New_InsSettings where LinkName='Percentage Settings' and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' and user_code='" + usercode + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]).Trim()) && Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]).Trim() != "0")
                {
                    string[] spl = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]).Split('-');
                    if (spl.Length == 1)
                    {
                        txt_empshare.Text = Convert.ToString(spl[0]);
                        txt_employershare.Text = "";
                        txt_fpf.Text = "";
                    }
                    else if (spl.Length == 2)
                    {
                        txt_empshare.Text = Convert.ToString(spl[0]);
                        txt_employershare.Text = Convert.ToString(spl[1]);
                        txt_fpf.Text = "";
                        //txtltdwages.Text = Convert.ToString(spl[0]);
                        //txtage.Text = Convert.ToString(spl[1]);
                    }
                    else if (spl.Length == 3)
                    {
                        txt_empshare.Text = Convert.ToString(spl[0]);
                        txt_employershare.Text = Convert.ToString(spl[1]);
                        txt_fpf.Text = Convert.ToString(spl[2]);

                    }
                }
                else
                {
                    txt_empshare.Text = "";
                    txt_employershare.Text = "";
                    txt_fpf.Text = "";
                }
            }
            else
            {
                txt_empshare.Text = "";
                txt_employershare.Text = "";
                txt_fpf.Text = "";
            }
        }
        else
        {
            divpercentage.Visible = false;
            msg.Visible = true;
            msg.Text = "Please Select Any College!";
        }
    }


}



//------------Last Modified on Oct 20th,2016--------------------------//
//------------LTD Wages Settings(Include Age) Added By Jeyaprakash on Oct 20th,2016------------//