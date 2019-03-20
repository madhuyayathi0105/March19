using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Drawing;
public partial class StaffAttendanceReport : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    DataSet ds = new DataSet();

    SqlDataAdapter da = new SqlDataAdapter();
    DAccess2 daccess = new DAccess2();
    Hashtable hashtab = new Hashtable();
    Hashtable hat = new Hashtable();
    string group_user = string.Empty;
    string strdept = string.Empty;
    string strcategory = string.Empty;
    static int seatcnt = 0;
    DataSet dsHolidaystaff = new DataSet();
    DataSet dsHolidayPresent = new DataSet();
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    static Hashtable htcolumn = new Hashtable();

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
            if (!IsPostBack)
            {
                try
                {
                    Fp_StaffAttendance.Sheets[0].AutoPostBack = false;
                    Fp_StaffAttendance.CommandBar.Visible = false;
                    FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    style2.Font.Size = 10;
                    style2.Font.Bold = true;
                    Fp_StaffAttendance.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style2);
                    Fp_StaffAttendance.Sheets[0].AllowTableCorner = true;
                    Fp_StaffAttendance.Sheets[0].RowHeader.Visible = false;
                    Fp_StaffAttendance.Sheets[0].SheetName = " ";
                    darkstyle.Font.Size = FontUnit.Medium;
                    darkstyle.Font.Name = "Book Antiqua";
                    darkstyle.Font.Bold = true;
                    darkstyle.HorizontalAlign = HorizontalAlign.Center;
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    Fp_StaffAttendance.Sheets[0].ColumnHeader.DefaultStyle = darkstyle;
                    Fp_StaffAttendance.Sheets[0].DefaultColumnWidth = 50;
                    Fp_StaffAttendance.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                    Fp_StaffAttendance.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                    Fp_StaffAttendance.Sheets[0].DefaultStyle.Font.Bold = false;
                    Fp_StaffAttendance.SheetCorner.Cells[0, 0].Font.Bold = true;
                    Fp_StaffAttendance.Sheets[0].ColumnCount = 0;
                    Fp_StaffAttendance.Visible = false;
                    btnprintmaster.Visible = false;
                    lblxl.Visible = false;
                    txtxl.Visible = false;
                    btnxl.Visible = false;
                    txttodate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                    txtfromdate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                    txttodate.Attributes.Add("ReadOnly", "ReadOnly");
                    txtfromdate.Attributes.Add("ReadOnly", "ReadOnly");
                    load_dept();
                    load_category();
                    bind_designation();
                    load_staffname();

                    //==Added by saranya on 29/8/2018==//
                    ItemList.Clear();
                    pheaderfilter.Visible = true;
                    pbodyfilter.Visible = true;
                    //cblsearch.Items[0].Selected = true;
                    //cblsearch.Items[1].Selected = true;
                    //===============================//
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        catch (Exception ex) { }
    }

    void load_dept()
    {
        chklst_dept.Items.Clear();
        ds.Clear();
        // ListItem lsitem = new ListItem();
        //Modified BY Srinath 1/4/2013
        // con.Open();
        //SqlCommand cmd = new SqlCommand("select distinct dept_code,dept_name from hrdept_master ", con);
        //da.SelectCommand = cmd;
        // da.Fill(ds);
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
            deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"].ToString() + "') order by dept_name";
        }
        if (deptquery != "")
        {
            ds = daccess.select_method(deptquery, hashtab, "Text");
            chklst_dept.DataSource = ds;
            chklst_dept.DataTextField = "dept_name";
            chklst_dept.DataValueField = "dept_code";
            chklst_dept.DataBind();
            // con.Close();
        }
    }

    void load_category()
    {
        //cblcategory.Visible = true;
        chklst_category.Items.Clear();
        ds.Clear();
        // ListItem lsitem = new ListItem();
        con.Open();
        SqlCommand cmd = new SqlCommand("select distinct category_code,category_name from staffcategorizer where college_Code='" + Convert.ToString(Session["collegecode"]) + "' ", con);
        da.SelectCommand = cmd;
        da.Fill(ds);
        chklst_category.DataSource = ds.Tables[0];
        chklst_category.DataTextField = "category_name";
        chklst_category.DataValueField = "category_code";
        chklst_category.DataBind();
        for (int i = 0; i < chklst_category.Items.Count; i++)
        {
            chklst_category.Items[i].Selected = true;
        }
        con.Close();
    }

    void load_staffname()
    {
        ddl_staff.Items.Clear();
        ds.Clear();
        // ListItem lsitem = new ListItem();
        con.Close();
        con.Open();
        string sqlstaffname = "Select distinct staffmaster.Staff_code,staff_name from staffmaster,hrdept_master,stafftrans where staffmaster.college_code='" + Session["collegecode"].ToString() + "' ";
        sqlstaffname = sqlstaffname + " and stafftrans.staff_code=staffmaster.staff_code  and resign=0 and settled=0 and stafftrans.dept_code=hrdept_master.dept_code  and stafftrans.latestrec=1 ";
        strdept = "";
        if (txt_dept.Text != "---Select---")
        {
            int itemcount = 0;
            for (itemcount = 0; itemcount < chklst_dept.Items.Count; itemcount++)
            {
                if (chklst_dept.Items[itemcount].Selected == true)
                {
                    if (strdept == "")
                        strdept = "'" + chklst_dept.Items[itemcount].Value.ToString() + "'";
                    else
                        strdept = strdept + "," + "'" + chklst_dept.Items[itemcount].Value.ToString() + "'";
                }
            }
            if (strdept != "")
            {
                strdept = " in(" + strdept + ")";
            }
            sqlstaffname = sqlstaffname + " and hrdept_master.dept_code " + strdept + "";
        }
        if (txt_category.Text != "--Select--")
        {
            int itemcount1 = 0;
            for (itemcount1 = 0; itemcount1 < chklst_category.Items.Count; itemcount1++)
            {
                if (chklst_category.Items[itemcount1].Selected == true)
                {
                    if (strcategory == "")
                        strcategory = "'" + chklst_category.Items[itemcount1].Value.ToString() + "'";
                    else
                        strcategory = strcategory + "," + "'" + chklst_category.Items[itemcount1].Value.ToString() + "'";
                }
            }
            if (strcategory != "")
            {
                strcategory = " and stafftrans.category_code in (" + strcategory + ")";
            }
            sqlstaffname = sqlstaffname + strcategory + "";
        }
        SqlCommand cmdstaff = new SqlCommand(sqlstaffname, con);
        da.SelectCommand = cmdstaff;
        da.Fill(ds);
        ddl_staff.DataSource = ds.Tables[0];
        ddl_staff.DataTextField = "Staff_name";
        ddl_staff.DataValueField = "Staff_code";
        ddl_staff.DataBind();
        ddl_staff.Items.Insert(0, "All");
        con.Close();
    }

    public void bind_designation()
    {
        con.Open();
        SqlCommand cmd_desig = new SqlCommand("select desig_code,desig_name from desig_master where collegecode='" + Session["collegecode"].ToString() + "'", con);
        SqlDataAdapter da_desig = new SqlDataAdapter(cmd_desig);
        DataTable dt_desig = new DataTable();
        da_desig.Fill(dt_desig);
        con.Close();
        if (dt_desig.Rows.Count > 0)
        {
            chklst_desig.Items.Clear();
            chklst_desig.DataSource = dt_desig;
            chklst_desig.DataValueField = "desig_code";
            chklst_desig.DataTextField = "desig_name";
            chklst_desig.DataBind();
        }
    }

    protected void chk_dept_CheckedChanged(object sender, EventArgs e)
    {
        int count = 0;
        if (chk_dept.Checked == true)
        {
            for (int i = 0; i < chklst_dept.Items.Count; i++)
            {
                count++;
                chklst_dept.Items[i].Selected = true;
                txt_dept.Text = "Department(" + count.ToString() + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklst_dept.Items.Count; i++)
            {
                chklst_dept.Items[i].Selected = false;
            }
            txt_dept.Text = "---Select---";
        }
        load_staffname();
    }

    protected void chklst_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        pdept.Focus();
        int deptcount = 0;
        string value = "";
        string code = "";
        for (int i = 0; i < chklst_dept.Items.Count; i++)
        {
            if (chklst_dept.Items[i].Selected == true)
            {
                value = chklst_dept.Items[i].Text;
                code = chklst_dept.Items[i].Value.ToString();
                deptcount = deptcount + 1;
                txt_dept.Text = "Department(" + deptcount.ToString() + ")";
            }
        }
        if (deptcount == 0)
            txt_dept.Text = "---Select---";
        else
        {
        }
        seatcnt = deptcount;
        load_staffname();
    }

    protected void chk_desig_CheckedChanged(object sender, EventArgs e)
    {
        int count = 0;
        if (chk_desig.Checked == true)
        {
            for (int i = 0; i < chklst_desig.Items.Count; i++)
            {
                count++;
                chklst_desig.Items[i].Selected = true;
                txt_desig.Text = "Designation(" + count.ToString() + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklst_desig.Items.Count; i++)
            {
                chklst_desig.Items[i].Selected = false;
            }
            txt_desig.Text = "---Select---";
        }
    }

    protected void chklst_desig_SelectedIndexChanged(object sender, EventArgs e)
    {
        pdesig.Focus();
        int desigcount = 0;
        string value = "";
        string code = "";
        for (int i = 0; i < chklst_desig.Items.Count; i++)
        {
            if (chklst_desig.Items[i].Selected == true)
            {
                value = chklst_desig.Items[i].Text;
                code = chklst_desig.Items[i].Value.ToString();
                desigcount = desigcount + 1;
                txt_desig.Text = "Designation(" + desigcount.ToString() + ")";
            }
        }
        if (desigcount == 0)
            txt_desig.Text = "---Select---";
        else
        {
        }
    }

    protected void chk_category_CheckedChanged(object sender, EventArgs e)
    {
        int count = 0;
        if (chk_category.Checked == true)
        {
            for (int i = 0; i < chklst_category.Items.Count; i++)
            {
                count++;
                chklst_category.Items[i].Selected = true;
                txt_category.Text = "Category(" + count.ToString() + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklst_category.Items.Count; i++)
            {
                chklst_category.Items[i].Selected = false;
            }
            txt_category.Text = "---Select---";
        }
        load_staffname();
    }

    protected void chklst_category_SelectedIndexChanged(object sender, EventArgs e)
    {
        pcategory.Focus();
        int categorycount = 0;
        string value = "";
        string code = "";
        for (int i = 0; i < chklst_category.Items.Count; i++)
        {
            if (chklst_category.Items[i].Selected == true)
            {
                value = chklst_category.Items[i].Text;
                code = chklst_category.Items[i].Value.ToString();
                categorycount = categorycount + 1;
                txt_category.Text = "Category(" + categorycount.ToString() + ")";
            }
        }
        if (categorycount == 0)
            txt_category.Text = "---Select---";
        else
        {
        }
        load_staffname();
    }

    private double lopval(double limitval)
    {
        double newretval = 0.0;
        double retval = 0.0;
        double frmval = 0.0;
        double toval = 0.0;
        try
        {
            string selq = daccess.GetFunction("select PerLOPDet from Hr_PaySettings where College_Code='" + Convert.ToString(Session["collegecode"]) + "'");
            if (selq.Trim() != "" && selq.Trim() != "0")
            {
                string[] spl = selq.Split('\\');
                if (spl.Length > 0)
                {
                    for (int ik = 0; ik < spl.Length; ik++)
                    {
                        string[] spl1 = spl[ik].Split(';');
                        if (spl1.Length >= 3)
                        {
                            Double.TryParse(Convert.ToString(spl1[0]), out frmval);
                            Double.TryParse(Convert.ToString(spl1[1]), out toval);
                            if (frmval <= limitval || toval <= limitval)   // && limitval <= toval
                            {
                                Double.TryParse(Convert.ToString(spl1[2]), out newretval);
                                retval = retval + newretval;
                            }
                        }
                    }
                }
            }
        }
        catch { }
        return retval;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            double percantageval = 0;
            double overallpercentage = 0;
            double totalrecord = 0;
            int coldayscount = 0;
            string deptcode = string.Empty;
            string desigcode = string.Empty;
            string categorycode = string.Empty;
            string staffcode = string.Empty;
            string str_deparment = string.Empty;
            string str_staffquery = string.Empty;
            string str_deptquery = string.Empty;
            string str_desigquery = string.Empty;
            string str_categoryquery = string.Empty;
            string[] splitfromdate = txtfromdate.Text.Split(new char[] { '/' });
            string[] splittodate = txttodate.Text.Split(new char[] { '/' });
            string fromdate = splitfromdate[1] + "/" + splitfromdate[0] + "/" + splitfromdate[2];
            string todate = splittodate[1] + "/" + splittodate[0] + "/" + splittodate[2];
            DateTime dtfrom = Convert.ToDateTime(fromdate.ToString());
            DateTime dtto = Convert.ToDateTime(todate.ToString());
            DateTime dtnow = DateTime.Now;
            DateTime myDtfrm = new DateTime();
            DateTime myDtTo = new DateTime();
            TimeSpan tscal = new TimeSpan();
            lastset.Visible = true;

            //===Added by saranya on 29/8/2018===//
            string coltext1 = "";
            int indexVal = 1;
            if (cblsearch.Visible == true)
            {
                if (ItemList.Count == 0)
                {
                    ItemList.Insert(0, "Staff Code");
                    ItemList.Insert(1, "Staff Name");
                }
            }
            htcolumn.Clear();
            htcolumn.Add("0", "Staff Code");
            htcolumn.Add("1", "Staff Name");
            htcolumn.Add("2", "Department Name");
            htcolumn.Add("3", "Department Acronym");
            htcolumn.Add("4", "Designation Name");
            htcolumn.Add("5", "Designation Acronym");
            htcolumn.Add("6", "No.Of Working Days");
            htcolumn.Add("7", "Holiday");
            htcolumn.Add("8", "Total No.Of Days for this Month");
            htcolumn.Add("9", "Total Present Days");
            htcolumn.Add("10", "Total Absent Days");

            int count = 0;
            for (int i = 0; i < cblsearch.Items.Count; i++)
            {
                if (cblsearch.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            //==================================//
            if (dtfrom > dtto)
            {
                lblerror.Text = "From Date Can't Be Greater Than To Date";
                Fp_StaffAttendance.Visible = false;
                lastset.Visible = false;
                //pheaderfilter.Visible = false;
                return;
            }
            if (dtnow < dtto)
            {
                lblerror.Text = "Please Enter Valid To Date";
                Fp_StaffAttendance.Visible = false;
                lastset.Visible = false;
                //pheaderfilter.Visible = false;
                return;
            }
            lblerror.Text = "";
            int start_day = Convert.ToInt32(dtfrom.Day);
            int end_day = Convert.ToInt32(dtto.Day);
            //Format1 02-11-2016
            if (rdbformat1.Checked == true)
            {
                if (dtfrom == dtto)
                {
                    coldayscount = 1;
                }
                else
                {
                    tscal = dtto - dtfrom;
                    Int32.TryParse(Convert.ToString(Convert.ToString(tscal).Split('.')[0]), out coldayscount);
                    coldayscount = coldayscount + 1;
                }
                myDtfrm = dtfrom;
                myDtTo = dtto;
            }
            TimeSpan t = dtto.Subtract(dtfrom);
            int day_count = (Convert.ToInt32(t.TotalDays) + 1);
            for (int i = 0; i < chklst_dept.Items.Count; i++)
            {
                if (chklst_dept.Items[i].Selected == true)
                {
                    if (deptcode == string.Empty)
                    {
                        deptcode = chklst_dept.Items[i].Value;
                    }
                    else
                    {
                        deptcode = deptcode + "','" + chklst_dept.Items[i].Value;
                    }
                }
            }
            for (int i = 0; i < chklst_desig.Items.Count; i++)
            {
                if (chklst_desig.Items[i].Selected == true)
                {
                    if (desigcode == string.Empty)
                    {
                        desigcode = chklst_desig.Items[i].Value;
                    }
                    else
                    {
                        desigcode = desigcode + "','" + chklst_desig.Items[i].Value;
                    }
                }
            }
            for (int i = 0; i < chklst_category.Items.Count; i++)
            {
                if (chklst_category.Items[i].Selected == true)
                {
                    if (categorycode == string.Empty)
                    {
                        categorycode = chklst_category.Items[i].Value;
                    }
                    else
                    {
                        categorycode = categorycode + "','" + chklst_category.Items[i].Value;
                    }
                }
            }
            if (ddl_staff.SelectedItem.ToString() == "All")
            {
                for (int i = 1; i < ddl_staff.Items.Count; i++)
                {
                    if (staffcode == string.Empty)
                    {
                        staffcode = ddl_staff.Items[i].Value;
                    }
                    else
                    {
                        staffcode = staffcode + "','" + ddl_staff.Items[i].Value;
                    }
                }
            }
            else
            {
                staffcode = ddl_staff.SelectedItem.Value;
            }
            if (!string.IsNullOrEmpty(staffcode))
            {
                str_staffquery = " AND M.Staff_Code in('" + staffcode + "') ";
                //str_staffquery = "M.Staff_Code in('" + staffcode + "')";
            }
            else
            {
                str_staffquery = "";
            }
            if (!string.IsNullOrEmpty(deptcode))
            {
                str_deptquery = " AND D.Dept_code in('" + deptcode + "') ";
            }
            else
            {
                str_deptquery = "";
            }
            if (!string.IsNullOrEmpty(desigcode))
            {
                str_desigquery = " AND G.desig_code in('" + desigcode + "') ";
            }
            else
            {
                str_desigquery = "";
            }
            con.Open();
            //SqlCommand cmd_linkvalue = new SqlCommand("select LinkValue from inssettings where linkname = 'Staff Holiday By Staff Type' AND College_Code ='" + Session["collegecode"].ToString() + "'", con);
            string str_linkvaue = daccess.GetFunction("select value from Master_Settings where settings='HR_PanelSettings' and usercode='" + Convert.ToString(Session["usercode"]) + "'");
            if (!string.IsNullOrEmpty(str_linkvaue) && str_linkvaue.Trim().Contains('3'))
                str_linkvaue = "1";
            else
                str_linkvaue = "0";
            SqlDataAdapter da_leavetype = new SqlDataAdapter("select * from leave_category where shortname not in('PER','LA') and college_code='" + Session["collegecode"].ToString() + "'", con);
            DataTable dt_leavetype = new DataTable();
            da_leavetype.Fill(dt_leavetype);
            DataView dv_salarydaycount = new DataView();
            dt_leavetype.DefaultView.RowFilter = "status in('','comp')";
            dv_salarydaycount = dt_leavetype.DefaultView;
            DataTable dt_staffdetails = new DataTable();
            if (ddl_staff.Items.Count > 1)
            {
                //SqlDataAdapter da_staffdetails = new SqlDataAdapter("SELECT M.Staff_Code,Staff_Name,Desig_Acronym,Desig_Name,Dept_Acronym,Dept_Name,category_code,stftype FROM StaffMaster M,StaffTrans T,Desig_Master G,HrDept_Master D WHERE M.Staff_Code = T.Staff_Code AND T.Desig_Code = G.Desig_Code AND M.College_Code = G.CollegeCode AND T.Dept_Code = D.Dept_Code AND M.College_Code = D.College_Code " + str_staffquery + str_deptquery + str_desigquery + " AND T.Latestrec = 1 AND M.College_Code ='" + Session["collegecode"].ToString() + "' order by G.print_pri desc,D.dept_name,T.stftype ,G.priority", con);//delsi1104 changing in orderby added dept_code1606

                SqlDataAdapter da_staffdetails = new SqlDataAdapter("SELECT M.Staff_Code,T.dept_code,Staff_Name,Desig_Acronym,Desig_Name,Dept_Acronym,Dept_Name,category_code,stftype FROM StaffMaster M,StaffTrans T,Desig_Master G,HrDept_Master D WHERE M.Staff_Code = T.Staff_Code AND T.Desig_Code = G.Desig_Code AND M.College_Code = G.CollegeCode AND T.Dept_Code = D.Dept_Code AND M.College_Code = D.College_Code " + str_staffquery + str_deptquery + str_desigquery + " AND T.Latestrec = 1 AND M.College_Code ='" + Session["collegecode"].ToString() + "' order by D.dept_name asc,T.stftype ,G.priority", con);//delsi1104

                da_staffdetails.Fill(dt_staffdetails);
            }
            con.Close();
            Fp_StaffAttendance.Sheets[0].ColumnHeader.RowCount = 0;
            Fp_StaffAttendance.Sheets[0].ColumnCount = 0;
            Fp_StaffAttendance.Sheets[0].RowCount = 0;
            int zerocolidx = 0;
            int shortcount = 0;
            //Format1 02-11-2016
            if (rdbformat1.Checked == true)
            {
                Fp_StaffAttendance.Sheets[0].ColumnHeader.RowCount = 3;
                Fp_StaffAttendance.Sheets[0].ColumnCount = 11 + (coldayscount * 2);   //Modified by srinath3/04/2014 9 to 11
            }
            else
            {
                Fp_StaffAttendance.Sheets[0].ColumnHeader.RowCount = 1;

            }
            //Format1 02-11-2016
            if (rdbformat1.Checked == true)
            {
                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Designation";
                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Department";
                Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
                Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
                Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
                Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);
                Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 3, 1);
                Fp_StaffAttendance.Columns[0].Width = 50;
                Fp_StaffAttendance.Columns[1].Width = 100;
                Fp_StaffAttendance.Columns[2].Width = 175;
                if (rdbtn_desig_acronym.SelectedIndex == 0)
                    Fp_StaffAttendance.Columns[3].Width = 175;
                else
                    Fp_StaffAttendance.Columns[3].Width = 100;
                if (rdbtn_dept_acronym.SelectedIndex == 0)
                    Fp_StaffAttendance.Columns[4].Width = 175;
                else
                    Fp_StaffAttendance.Columns[4].Width = 100;
                Fp_StaffAttendance.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

                int colidx = Fp_StaffAttendance.Sheets[0].ColumnCount - ((coldayscount * 2) + 7);
                while (myDtfrm <= myDtTo)
                {
                    colidx++;
                    Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[2, colidx].Text = "M";
                    colidx++;
                    Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[2, colidx].Text = "E";
                    Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[1, colidx - 1].Text = Convert.ToString(myDtfrm.Day);
                    Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(1, colidx - 1, 1, 2);
                    zerocolidx++;
                    myDtfrm = myDtfrm.AddDays(1);
                }
                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, colidx - ((zerocolidx * 2) - 1)].Text = "Attendance for the month of";
                Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, colidx - ((zerocolidx * 2) - 1), 1, (zerocolidx * 2));
                colidx += 2;
                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, colidx - 1].Text = "No.of Working Days";
                Fp_StaffAttendance.Columns[colidx - 1].Width = 75;
                Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, colidx - 1, 3, 1);
                colidx++;
                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, colidx - 1].Text = "Holiday";
                Fp_StaffAttendance.Columns[colidx - 1].Width = 75;
                Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, colidx - 1, 3, 1);
                colidx++;
                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, colidx - 1].Text = "Total Days";
                Fp_StaffAttendance.Columns[colidx - 1].Width = 75;
                Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, colidx - 1, 3, 1);
                colidx++;
                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, colidx - 1].Text = "P";
                Fp_StaffAttendance.Columns[colidx - 1].Width = 50;
                Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, colidx - 1, 3, 1);
                colidx++;
                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, colidx - 1].Text = "LA";//Added by srinath3/04/2014 9 to 11
                Fp_StaffAttendance.Columns[colidx - 1].Width = 50;
                Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, colidx - 1, 3, 1);
                colidx++;
                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, colidx - 1].Text = "PER";//Added by srinath3/04/2014 9 to 11
                Fp_StaffAttendance.Columns[colidx - 1].Width = 50;
                Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, colidx - 1, 3, 1);
            }
            else
            {
                //Added by saranya on 30August2018//                
                Fp_StaffAttendance.Sheets[0].ColumnCount = ItemList.Count + 1;
                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);

                int insdex = 0;
                foreach (string key in htcolumn.Keys)
                {
                    coltext1 = htcolumn[key].ToString();
                    if (ItemList.Contains(Convert.ToString(coltext1)))
                    {
                        if (coltext1 != "Total Present Days" && coltext1 != "Total Absent Days")
                        {
                            //Fp_StaffAttendance.Sheets[0].ColumnCount++;
                            insdex = ItemList.IndexOf(Convert.ToString(coltext1));
                            //FpSpread1.Columns[insdex].Locked = true;
                            Fp_StaffAttendance.Columns[insdex + 1].Width = 150;
                            Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, insdex + 1].Text = Convert.ToString(coltext1);
                            Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, insdex + 1].Font.Bold = true;
                            Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, insdex + 1].Font.Name = "Book Antiqua";
                            Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, insdex + 1].Font.Size = FontUnit.Medium;
                            Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, insdex + 1].HorizontalAlign = HorizontalAlign.Center;
                            Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, insdex + 1, 2, 1);
                            indexVal++;
                        }
                    }
                }

                //Commented By Saranya

                //Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, 5].Text = "No.of Working Days";
                //Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Holiday";
                //Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total Days";
                //Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, 8].Text = "P";
                //Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, 9].Text = "LA";//Added by srinath3/04/2014 9 to 11
                //Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, 10].Text = "PER";//Added by srinath3/04/2014 9 to 11

                Fp_StaffAttendance.Sheets[0].ColumnCount = Fp_StaffAttendance.Sheets[0].ColumnCount + 3;
                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Text = "P";
                Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, indexVal, 3, 1);
                Fp_StaffAttendance.Sheets[0].Columns[indexVal].HorizontalAlign = HorizontalAlign.Center;
                Fp_StaffAttendance.Columns[indexVal].Width = 50;
                indexVal++;
                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Text = "LA";
                Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, indexVal, 3, 1);
                Fp_StaffAttendance.Sheets[0].Columns[indexVal].HorizontalAlign = HorizontalAlign.Center;
                Fp_StaffAttendance.Columns[indexVal].Width = 50;
                indexVal++;
                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Text = "PER";
                Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, indexVal, 3, 1);
                Fp_StaffAttendance.Sheets[0].Columns[indexVal].HorizontalAlign = HorizontalAlign.Center;
                Fp_StaffAttendance.Columns[indexVal].Width = 50;

                //=================================================//

            }
            if (rdbformat1.Checked == true)
            {
                for (int col_inc = 0; col_inc < dt_leavetype.Rows.Count; col_inc++)
                {
                    Fp_StaffAttendance.Sheets[0].ColumnCount++;
                    shortcount++;
                    Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, Fp_StaffAttendance.Sheets[0].ColumnCount - 1].Text = dt_leavetype.Rows[col_inc]["shortname"].ToString();
                    Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, Fp_StaffAttendance.Sheets[0].ColumnCount - 1].Tag = dt_leavetype.Rows[col_inc]["shortname"].ToString();
                    Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, Fp_StaffAttendance.Sheets[0].ColumnCount - 1, 3, 1);
                }
            }
            else//Added by saranya on 30/08/2018
            {
                DataSet dsLeaveMaster = new DataSet();
                string selQry = "";

                #region For Treated as Present

                selQry = "select * from leave_category where shortname not in('PER','LA') and college_code='" + Session["collegecode"].ToString() + "' and status in('0','2','pres','earn')";
                dsLeaveMaster.Clear();
                dsLeaveMaster = daccess.select_method_wo_parameter(selQry, "Text");
                if (dsLeaveMaster.Tables[0].Rows.Count > 0)
                {
                    Fp_StaffAttendance.Sheets[0].ColumnCount = Fp_StaffAttendance.Sheets[0].ColumnCount + dsLeaveMaster.Tables[0].Rows.Count;
                    for (int LeaveCnt = 0; LeaveCnt < dsLeaveMaster.Tables[0].Rows.Count; LeaveCnt++)
                    {
                        indexVal++;
                        shortcount++;
                        Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Text = dsLeaveMaster.Tables[0].Rows[LeaveCnt]["shortname"].ToString();
                        Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Tag = dsLeaveMaster.Tables[0].Rows[LeaveCnt]["shortname"].ToString();
                        Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, indexVal, 3, 1);
                    }
                }
                foreach (string key in htcolumn.Keys)
                {
                    coltext1 = htcolumn[key].ToString();
                    if (ItemList.Contains(Convert.ToString(coltext1)))
                    {
                        if (coltext1 == "Total Present Days")
                        {
                            indexVal++;
                            Fp_StaffAttendance.Columns[indexVal].Width = 100;
                            Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Text = Convert.ToString(coltext1);
                            Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Tag = indexVal.ToString();
                            Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Font.Bold = true;
                            Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Font.Name = "Book Antiqua";
                            Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Font.Size = FontUnit.Medium;
                            Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].HorizontalAlign = HorizontalAlign.Center;
                            Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, indexVal, 2, 1);
                        }
                    }
                }

                #endregion

                #region For Treated as LOP

                selQry = "select * from leave_category where shortname not in('PER','LA') and college_code='" + Session["collegecode"].ToString() + "' and status in('1','comp')";
                dsLeaveMaster.Clear();
                dsLeaveMaster = daccess.select_method_wo_parameter(selQry, "Text");
                if (dsLeaveMaster.Tables[0].Rows.Count > 0)
                {
                    Fp_StaffAttendance.Sheets[0].ColumnCount = Fp_StaffAttendance.Sheets[0].ColumnCount + dsLeaveMaster.Tables[0].Rows.Count;
                    for (int LeaveCnt = 0; LeaveCnt < dsLeaveMaster.Tables[0].Rows.Count; LeaveCnt++)
                    {
                        indexVal++;
                        shortcount++;
                        Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Text = dsLeaveMaster.Tables[0].Rows[LeaveCnt]["shortname"].ToString();
                        Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Tag = dsLeaveMaster.Tables[0].Rows[LeaveCnt]["shortname"].ToString();
                        Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, indexVal, 3, 1);
                    }
                }

                #endregion
            }

            if (chklalop.Checked == true)
            {
                if (rdbformat2.Checked == true)
                {
                    //Fp_StaffAttendance.Sheets[0].ColumnCount += 4;
                    //Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, Fp_StaffAttendance.Sheets[0].ColumnCount - 4].Text = "A";
                    //Fp_StaffAttendance.Columns[Fp_StaffAttendance.Sheets[0].ColumnCount - 4].Width = 50;
                    //Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, Fp_StaffAttendance.Sheets[0].ColumnCount - 4, 3, 1);
                    //Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, Fp_StaffAttendance.Sheets[0].ColumnCount - 3].Text = "LA as LOP";
                    //Fp_StaffAttendance.Columns[Fp_StaffAttendance.Sheets[0].ColumnCount - 3].Width = 75;
                    //Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, Fp_StaffAttendance.Sheets[0].ColumnCount - 3, 3, 1);
                    //Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, Fp_StaffAttendance.Sheets[0].ColumnCount - 2].Text = "No of Days Eligible for Pay";
                    //Fp_StaffAttendance.Columns[Fp_StaffAttendance.Sheets[0].ColumnCount - 2].Width = 75;
                    //Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, Fp_StaffAttendance.Sheets[0].ColumnCount - 2, 3, 1);

                    //Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, Fp_StaffAttendance.Sheets[0].ColumnCount - 1].Text = "Eligible to Pay Percentage";
                    //Fp_StaffAttendance.Columns[Fp_StaffAttendance.Sheets[0].ColumnCount - 1].Width = 75;
                    //Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, Fp_StaffAttendance.Sheets[0].ColumnCount - 1, 3, 1);

                    //Modified by saranya

                    Fp_StaffAttendance.Sheets[0].ColumnCount = Fp_StaffAttendance.Sheets[0].ColumnCount + 4;
                    indexVal++;
                    Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Text = "A";
                    Fp_StaffAttendance.Columns[indexVal].Width = 50;
                    Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, indexVal, 3, 1);
                    indexVal++;
                    Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Text = "LA as LOP";
                    Fp_StaffAttendance.Columns[indexVal].Width = 75;
                    Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, indexVal, 3, 1);
                    //Total AbsentDays
                    foreach (string key in htcolumn.Keys)
                    {
                        coltext1 = htcolumn[key].ToString();
                        if (ItemList.Contains(Convert.ToString(coltext1)))
                        {
                            if (coltext1 == "Total Absent Days")
                            {
                                indexVal++;
                                Fp_StaffAttendance.Columns[indexVal].Width = 100;
                                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Text = Convert.ToString(coltext1);
                                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Tag = indexVal.ToString();
                                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Font.Bold = true;
                                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Font.Name = "Book Antiqua";
                                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Font.Size = FontUnit.Medium;
                                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].HorizontalAlign = HorizontalAlign.Center;
                                Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, indexVal, 2, 1);
                            }
                        }
                    }

                    indexVal++;
                    Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Text = "No of Days Eligible for Pay";
                    Fp_StaffAttendance.Columns[indexVal].Width = 75;
                    Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, indexVal, 3, 1);
                    indexVal++;
                    Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Text = "Eligible to Pay Percentage";
                    Fp_StaffAttendance.Columns[indexVal].Width = 75;
                    Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, indexVal, 3, 1);

                }
                else
                {

                    Fp_StaffAttendance.Sheets[0].ColumnCount += 3;
                    Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, Fp_StaffAttendance.Sheets[0].ColumnCount - 3].Text = "A";
                    Fp_StaffAttendance.Columns[Fp_StaffAttendance.Sheets[0].ColumnCount - 3].Width = 50;
                    Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, Fp_StaffAttendance.Sheets[0].ColumnCount - 3, 3, 1);
                    Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, Fp_StaffAttendance.Sheets[0].ColumnCount - 2].Text = "LA as LOP";
                    Fp_StaffAttendance.Columns[Fp_StaffAttendance.Sheets[0].ColumnCount - 2].Width = 75;
                    Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, Fp_StaffAttendance.Sheets[0].ColumnCount - 2, 3, 1);
                    Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, Fp_StaffAttendance.Sheets[0].ColumnCount - 1].Text = "No of Days Eligible for Pay";
                    Fp_StaffAttendance.Columns[Fp_StaffAttendance.Sheets[0].ColumnCount - 1].Width = 75;
                    Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, Fp_StaffAttendance.Sheets[0].ColumnCount - 1, 3, 1);
                }
            }
            else
            {
                if (rdbformat2.Checked == true)
                {
                    //Commented by Saranya on 30/8/2018

                    //Fp_StaffAttendance.Sheets[0].ColumnCount += 3;
                    //Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, Fp_StaffAttendance.Sheets[0].ColumnCount - 3].Text = "A";
                    //Fp_StaffAttendance.Columns[Fp_StaffAttendance.Sheets[0].ColumnCount - 3].Width = 50;
                    //Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, Fp_StaffAttendance.Sheets[0].ColumnCount - 3, 3, 1);
                    //Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, Fp_StaffAttendance.Sheets[0].ColumnCount - 2].Text = "No of Days Eligible for Pay";
                    //Fp_StaffAttendance.Columns[Fp_StaffAttendance.Sheets[0].ColumnCount - 2].Width = 75;
                    //Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, Fp_StaffAttendance.Sheets[0].ColumnCount - 2, 3, 1);
                    ////delsi1203
                    //Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, Fp_StaffAttendance.Sheets[0].ColumnCount - 1].Text = "Eligible to Pay Percentage";
                    //Fp_StaffAttendance.Columns[Fp_StaffAttendance.Sheets[0].ColumnCount - 1].Width = 75;
                    //Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, Fp_StaffAttendance.Sheets[0].ColumnCount - 1, 3, 1);

                    //Modified By Saranya on 30/08/2018
                    indexVal++;
                    Fp_StaffAttendance.Sheets[0].ColumnCount = Fp_StaffAttendance.Sheets[0].ColumnCount + 3;
                    Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Text = "A";
                    Fp_StaffAttendance.Columns[indexVal].Width = 50;
                    Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, indexVal, 3, 1);

                    //Total AbsentDays
                    foreach (string key in htcolumn.Keys)
                    {
                        coltext1 = htcolumn[key].ToString();
                        if (ItemList.Contains(Convert.ToString(coltext1)))
                        {
                            if (coltext1 == "Total Absent Days")
                            {
                                indexVal++;
                                Fp_StaffAttendance.Columns[indexVal].Width = 100;
                                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Text = Convert.ToString(coltext1);
                                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Tag = indexVal.ToString();
                                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Font.Bold = true;
                                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Font.Name = "Book Antiqua";
                                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Font.Size = FontUnit.Medium;
                                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].HorizontalAlign = HorizontalAlign.Center;
                                Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, indexVal, 2, 1);
                            }
                        }
                    }
                    indexVal++;
                    Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Text = "No of Days Eligible for Pay";
                    Fp_StaffAttendance.Columns[indexVal].Width = 100;
                    Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, indexVal, 3, 1);
                    indexVal++;
                    Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Text = "Eligible to Pay Percentage";
                    Fp_StaffAttendance.Columns[indexVal].Width = 100;
                    Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, indexVal, 3, 1);
                }
                else
                {

                    Fp_StaffAttendance.Sheets[0].ColumnCount += 2;
                    Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, Fp_StaffAttendance.Sheets[0].ColumnCount - 2].Text = "A";
                    Fp_StaffAttendance.Columns[Fp_StaffAttendance.Sheets[0].ColumnCount - 2].Width = 50;
                    Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, Fp_StaffAttendance.Sheets[0].ColumnCount - 2, 3, 1);
                    Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, Fp_StaffAttendance.Sheets[0].ColumnCount - 1].Text = "No of Days Eligible for Pay";
                    Fp_StaffAttendance.Columns[Fp_StaffAttendance.Sheets[0].ColumnCount - 1].Width = 75;
                    Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, Fp_StaffAttendance.Sheets[0].ColumnCount - 1, 3, 1);

                }
            }
            if (rdbformat2.Checked == true)
            {
                Fp_StaffAttendance.Sheets[0].ColumnCount = Fp_StaffAttendance.Sheets[0].ColumnCount + 1;
                indexVal++;
                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, indexVal].Text = "Signature";
                Fp_StaffAttendance.Sheets[0].Columns[indexVal].Width = 150;
                Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, indexVal, 3, 1);
            }
            else
            {
                Fp_StaffAttendance.Sheets[0].ColumnCount++;
                Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, Fp_StaffAttendance.Sheets[0].ColumnCount - 1].Text = "Signature";
                Fp_StaffAttendance.Sheets[0].Columns[Fp_StaffAttendance.Sheets[0].ColumnCount - 1].Width = 150;
                Fp_StaffAttendance.Sheets[0].ColumnHeaderSpanModel.Add(0, Fp_StaffAttendance.Sheets[0].ColumnCount - 1, 3, 1);
            }
            int sno = 0;
            con.Open();
            double deentry = 0;
            double countwise = 0;
            for (int i = 0; i < dt_staffdetails.Rows.Count; i++)
            {
                hat.Clear();
                hat.Add("P", 0);
                hat.Add("A", 0);
                hat.Add("PER", 0);//Added by srinath3/04/2014 9 to 11
                hat.Add("LA", 0);//Added by srinath3/04/2014 9 to 11
                for (int col_inc = 0; col_inc < dt_leavetype.Rows.Count; col_inc++)
                {
                    if (!hat.Contains(dt_leavetype.Rows[col_inc]["shortname"].ToString()))//barath 13.06.17
                        hat.Add(dt_leavetype.Rows[col_inc]["shortname"].ToString(), 0);
                }
                //for (int sprd_col = 7; sprd_col < Fp_StaffAttendance.Sheets[0].ColumnCount; sprd_col++)
                //{
                double no_of_workingdays = 0;
                double totalworkingdays = 0;
                string holiday_count = string.Empty;
                string cur_categorycode = dt_staffdetails.Rows[i]["category_code"].ToString();
                string deptCode = Convert.ToString(dt_staffdetails.Rows[i]["dept_code"]);
                string cur_stafftype = dt_staffdetails.Rows[i]["stftype"].ToString();
                string cur_staffcode = dt_staffdetails.Rows[i]["staff_code"].ToString();
                if (str_deparment == string.Empty || str_deparment != dt_staffdetails.Rows[i]["Dept_Name"].ToString())
                {
                    if (rdbformat1.Checked == true)
                    {
                        Fp_StaffAttendance.Sheets[0].RowCount++;
                        if (rdbtn_dept_acronym.Items[0].Selected == true)
                        {
                            Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, 0].Text = dt_staffdetails.Rows[i]["Dept_Name"].ToString();
                        }
                        else
                        {
                            Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, 0].Text = dt_staffdetails.Rows[i]["Dept_Acronym"].ToString();
                        }
                        Fp_StaffAttendance.Sheets[0].SpanModel.Add(Fp_StaffAttendance.Sheets[0].RowCount - 1, 0, 1, Fp_StaffAttendance.Sheets[0].ColumnCount);
                        Fp_StaffAttendance.Sheets[0].Rows[Fp_StaffAttendance.Sheets[0].RowCount - 1].BackColor = Color.MediumAquamarine;
                    }
                    if (rdbformat2.Checked == true)
                    {
                        Fp_StaffAttendance.Sheets[0].RowCount++;
                        if (cblsearch.Items[2].Selected == true)
                        {
                            Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, 0].Text = dt_staffdetails.Rows[i]["Dept_Name"].ToString();
                        }
                        if (cblsearch.Items[3].Selected == true)
                        {
                            Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, 0].Text = dt_staffdetails.Rows[i]["Dept_Acronym"].ToString();
                        }
                        Fp_StaffAttendance.Sheets[0].SpanModel.Add(Fp_StaffAttendance.Sheets[0].RowCount - 1, 0, 1, Fp_StaffAttendance.Sheets[0].ColumnCount);
                        Fp_StaffAttendance.Sheets[0].Rows[Fp_StaffAttendance.Sheets[0].RowCount - 1].BackColor = Color.MediumAquamarine;
                    }
                }
                sno++;
                totalrecord = sno;
                Fp_StaffAttendance.Sheets[0].RowCount++;
                bool flag = false; // added by jairam 07-11-2014
                //for (DateTime dt = Convert.ToDateTime(fromdate); dt.Month <= dtto.Month; dt = dt.AddMonths(1))
                //{
                DateTime dt = Convert.ToDateTime(fromdate);
                holiday_count = string.Empty;
                start_day = Convert.ToInt32(dt.ToString("dd"));
                if (str_linkvaue == "0")
                {
                    SqlCommand cmd_staffholiday = new SqlCommand("SELECT COUNT(*) FROM HolidayStaff WHERE Category_Code = '" + cur_categorycode + "' AND Holiday_date BETWEEN '" + fromdate + "' and '" + todate + "' and dept_code='" + deptCode + "' and college_code =" + Session["collegecode"].ToString() + "", con); // added by jairam 07-11-2014
                    holiday_count = Convert.ToString(cmd_staffholiday.ExecuteScalar());
                    if (string.IsNullOrEmpty(holiday_count))
                    {
                        holiday_count = "0";
                    }
                }
                else
                {
                    SqlCommand cmd_staffholiday = new SqlCommand("SELECT distinct COUNT(category_code) FROM HolidayStaff WHERE stftype = '" + cur_stafftype + "' AND Holiday_date BETWEEN '" + fromdate + "' and '" + todate + "'  and dept_code='" + deptCode + "' and college_code =" + Session["collegecode"].ToString() + " group by category_code", con); // added by jairam 07-11-2014
                    holiday_count = Convert.ToString(cmd_staffholiday.ExecuteScalar());
                    if (string.IsNullOrEmpty(holiday_count))
                    {
                        holiday_count = "0";
                    }
                }
                no_of_workingdays = (Convert.ToInt32(day_count) - Convert.ToInt32(holiday_count));
                totalworkingdays = (Convert.ToInt32(day_count));//+ Convert.ToInt32(holiday_count));                
                Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, 0].Text = sno.ToString();

                if (rdbformat1.Checked == true)
                {
                    Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, 1].Text = dt_staffdetails.Rows[i]["staff_code"].ToString();
                    Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, 2].Text = dt_staffdetails.Rows[i]["staff_name"].ToString();
                    if (rdbtn_desig_acronym.Items[0].Selected == true)
                    {
                        Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, 3].Text = dt_staffdetails.Rows[i]["Desig_Name"].ToString();
                    }
                    else
                    {
                        Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, 3].Text = dt_staffdetails.Rows[i]["Desig_Acronym"].ToString();
                    }
                    if (rdbtn_dept_acronym.Items[0].Selected == true)
                    {
                        Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, 4].Text = dt_staffdetails.Rows[i]["Dept_Name"].ToString();
                    }
                    else
                    {
                        Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, 4].Text = dt_staffdetails.Rows[i]["Dept_Acronym"].ToString();
                    }
                }

                //Format2 02-11-2016
                if (rdbformat2.Checked == true)
                {
                    string sql = "SELECT * FROM HolidayStaff WHERE Category_Code = '" + cur_categorycode + "' AND Holiday_date BETWEEN '" + fromdate + "' and '" + todate + "' and dept_code='" + deptCode + "' and college_code =" + Session["collegecode"].ToString() + "";

                    double TotalHolidayCount = Convert.ToDouble(holiday_count);
                    double TotalNoWorkingDays = Convert.ToDouble(no_of_workingdays);
                    dsHolidaystaff = daccess.select_method_wo_parameter(sql, "Text");
                    if (dsHolidaystaff.Tables[0].Rows.Count > 0)
                    {
                        for (int holiday = 0; holiday < dsHolidaystaff.Tables[0].Rows.Count; holiday++)
                        {
                            string holidayDate = Convert.ToString(dsHolidaystaff.Tables[0].Rows[holiday]["Holiday_date"]);
                            string[] DtSplit = holidayDate.Split('/');
                            holidayDate = DtSplit[1];

                            //For Date
                            holidayDate = holidayDate.StartsWith("0") ? holidayDate.Substring(1) : holidayDate;                           

                            //For Month
                            string holidayMnt = DtSplit[0];
                            holidayMnt = holidayMnt.StartsWith("0") ? holidayMnt.Substring(1) : holidayMnt;                           
                           
                            string Year = DtSplit[2];
                            string MonthYear = holidayMnt + "/" + Year.Split(' ')[0];
                            holidayDate = "[" + holidayDate + "]";

                            string Attendance = daccess.GetFunction(" select " + holidayDate + " from staff_attnd where mon_year in('" + MonthYear + "') and staff_code='" + cur_staffcode + "' ");
                            string[] attnValue = Attendance.Split('-');

                            if (attnValue[0] == "P" && attnValue[1] == "P" || attnValue[0] == "P" && attnValue[1] == "PER" || attnValue[0] == "PER" && attnValue[1] == "P")
                            {
                                holiday_count = Convert.ToString(TotalHolidayCount - 1);
                                no_of_workingdays = TotalNoWorkingDays + 1;
                            }
                            if (attnValue[0] == "P" && attnValue[1] == "A" || attnValue[0] == "A" && attnValue[1] == "P")
                            {
                                holiday_count = Convert.ToString(TotalHolidayCount - 0.5);
                                no_of_workingdays = TotalNoWorkingDays + 0.5;
                            }
                        }
                    }
                    int insdex = 0;
                    foreach (string key in htcolumn.Keys)
                    {
                        coltext1 = htcolumn[key].ToString();
                        insdex = ItemList.IndexOf(Convert.ToString(coltext1));
                        if (ItemList.Contains(Convert.ToString(coltext1)))
                        {
                            if (coltext1.Trim() == "Staff Code")
                            {
                                Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, insdex + 1].Text = dt_staffdetails.Rows[i]["staff_code"].ToString();
                            }
                            if (coltext1.Trim() == "Staff Name")
                            {
                                Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, insdex + 1].Text = dt_staffdetails.Rows[i]["staff_name"].ToString();
                            }

                            if (coltext1.Trim() == "Department Name")
                            {
                                Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, insdex + 1].Text = dt_staffdetails.Rows[i]["Dept_Name"].ToString();
                                Fp_StaffAttendance.Sheets[0].SetColumnMerge(insdex + 1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                Fp_StaffAttendance.Sheets[0].Columns[insdex + 1].HorizontalAlign = HorizontalAlign.Left;
                                Fp_StaffAttendance.Sheets[0].Columns[insdex + 1].VerticalAlign = VerticalAlign.Middle;
                            }
                            if (coltext1.Trim() == "Department Acronym")
                            {
                                Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, insdex + 1].Text = dt_staffdetails.Rows[i]["Dept_Acronym"].ToString();
                                Fp_StaffAttendance.Sheets[0].SetColumnMerge(insdex + 1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                Fp_StaffAttendance.Sheets[0].Columns[insdex + 1].HorizontalAlign = HorizontalAlign.Left;
                                Fp_StaffAttendance.Sheets[0].Columns[insdex + 1].VerticalAlign = VerticalAlign.Middle;
                            }

                            if (coltext1.Trim() == "Designation Name")
                            {
                                Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, insdex + 1].Text = dt_staffdetails.Rows[i]["Desig_Name"].ToString();
                                Fp_StaffAttendance.Sheets[0].SetColumnMerge(insdex + 1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                Fp_StaffAttendance.Sheets[0].Columns[insdex + 1].HorizontalAlign = HorizontalAlign.Left;
                                Fp_StaffAttendance.Sheets[0].Columns[insdex + 1].VerticalAlign = VerticalAlign.Middle;
                            }
                            if (coltext1.Trim() == "Designation Acronym")
                            {
                                Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, insdex + 1].Text = dt_staffdetails.Rows[i]["Desig_Acronym"].ToString();
                                Fp_StaffAttendance.Sheets[0].SetColumnMerge(insdex + 1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                Fp_StaffAttendance.Sheets[0].Columns[insdex + 1].HorizontalAlign = HorizontalAlign.Left;
                                Fp_StaffAttendance.Sheets[0].Columns[insdex + 1].VerticalAlign = VerticalAlign.Middle;
                            }
                            if (coltext1.Trim() == "No.Of Working Days")
                            {
                                Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, insdex + 1].Text = no_of_workingdays.ToString();
                                Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, insdex + 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (coltext1.Trim() == "Holiday")
                            {
                                Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, insdex + 1].Text = holiday_count.ToString();
                                Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, insdex + 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (coltext1.Trim() == "Total No.Of Days for this Month")
                            {
                                Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, insdex + 1].Text = Convert.ToString(Convert.ToDouble(no_of_workingdays) + Convert.ToDouble(holiday_count));
                                Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, insdex + 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }

                    //Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, 5].Text = no_of_workingdays.ToString();
                    //Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, 6].Text = holiday_count.ToString();
                    //Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(Convert.ToDouble(no_of_workingdays) + Convert.ToDouble(holiday_count));
                }
                int mycolidx = Fp_StaffAttendance.Sheets[0].ColumnCount - ((coldayscount * 2) + 9 + shortcount);
                DateTime ddttfrom = Convert.ToDateTime(fromdate);
                DateTime ddttto = Convert.ToDateTime(todate);
                string monyear = Convert.ToString(ddttfrom.Month) + "/" + Convert.ToString(ddttfrom.Year);
                string monyear1 = Convert.ToString(ddttto.Month) + "/" + Convert.ToString(ddttto.Year); string monthyear = string.Empty; int co = 0;
                int Diffmonth = (ddttto.Month - ddttfrom.Month) + 12 * (ddttto.Year - ddttfrom.Year);// poo
                //DateTime from = ddttfrom;
                DateTime from = Convert.ToDateTime(Convert.ToString(ddttfrom.Month) + "/01/" + Convert.ToString(ddttfrom.Year));
                // poo              
                if (ddttfrom <= ddttto) // poo
                {
                    //for (int ij = 0; ij < Diffmonth; ij++)
                    while (ddttto >= from)
                    {
                        co++;
                        if (string.IsNullOrEmpty(monthyear))
                            monthyear = Convert.ToString(from.Month) + "/" + Convert.ToString(from.Year);//monyear + "','
                        else
                            monthyear = monthyear + "','" + Convert.ToString(from.Month) + "/" + Convert.ToString(from.Year); //
                        from = from.AddMonths(1);
                    }
                }
                //SqlDataAdapter da_staff_attnd = new SqlDataAdapter("select * from staff_attnd where mon_year in('" + monyear + "','" + monyear1 + "') and staff_code='" + cur_staffcode + "'", con);
                string query = "select * from staff_attnd where mon_year in('" + monthyear + "') and staff_code='" + cur_staffcode + "'";
                SqlDataAdapter da_staff_attnd = new SqlDataAdapter(query, con);
                DataSet dt_staff_attnd = new DataSet();
                da_staff_attnd.Fill(dt_staff_attnd);
                DataView dvnew = new DataView();
                while (ddttto >= ddttfrom) //****************** Start ************* Added by jairam  10-12-2014 *********
                {
                    //if (ddttto >= ddttfrom)
                    //{
                    if (flag == true) // added by jairam 07-11-2014
                    {
                        start_day = 1;
                    }
                    //if (ddttfrom.Month == ddttto.Month)
                    //{
                    //if (dt_staff_attnd.Rows.Count > 0)
                    //{
                    //for (int day = 3 + start_day; day < dtto.Day + 4; day++)
                    //{
                    if (dt_staff_attnd.Tables.Count > 0 && dt_staff_attnd.Tables[0].Rows.Count > 0)
                    {
                        dt_staff_attnd.Tables[0].DefaultView.RowFilter = " mon_year='" + Convert.ToString(ddttfrom.Month + "/" + ddttfrom.Year) + "'";
                        dvnew = dt_staff_attnd.Tables[0].DefaultView;
                        string att_type = "";
                        if (dvnew.Count > 0)
                        {
                            att_type = Convert.ToString(dvnew[0]["" + ddttfrom.Day + ""]);
                        }
                        string[] split_att_type = att_type.Split(new char[] { '-' });
                        if (split_att_type.Length > 1)//delsis1302 modified mycolidx into  mycolidx -1
                        {
                            //Format1
                            if (rdbformat1.Checked == true)
                            {
                                if (split_att_type.Length == 2)
                                {
                                    mycolidx++;
                                    if (split_att_type[0] != " ")
                                        Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, mycolidx - 1].Text = Convert.ToString(split_att_type[0]);
                                    else
                                        Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, mycolidx - 1].Text = Convert.ToString("-");
                                    mycolidx++;
                                    if (split_att_type[1] != " ")
                                        Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, mycolidx - 1].Text = Convert.ToString(split_att_type[1]);
                                    else
                                        Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, mycolidx - 1].Text = Convert.ToString("-");
                                }
                            }
                            for (int j = 0; j <= split_att_type.GetUpperBound(0); j++)
                            {
                                if (!string.IsNullOrEmpty(split_att_type[j]))
                                {
                                    //Modifeied  by srinath3/04/2014 
                                    if (hat.Contains(split_att_type[j]))
                                    {
                                        deentry = Convert.ToDouble(hat[split_att_type[j]]);
                                        deentry = deentry + 0.5;
                                        countwise = Convert.ToDouble(hat[split_att_type[j]]);
                                        countwise = countwise + 0.5;//1 change 0.5 delsi 07.11.2017
                                        if (rbdaywise.Checked == true)
                                        {
                                            hat[split_att_type[j]] = deentry;
                                        }
                                        else if (rbcountwise.Checked == true)
                                        {
                                            if (split_att_type[j].Trim() == "P")//|| split_att_type[j].Trim()=="CL"
                                            {
                                                hat[split_att_type[j]] = deentry;
                                            }
                                            else
                                            {
                                                hat[split_att_type[j]] = countwise;
                                            }
                                        }
                                    }
                                    //if (split_att_type[j] != "PER")
                                    //{
                                    //    deentry = Convert.ToDouble(hat[split_att_type[j]]);
                                    //    deentry = deentry + 0.5;
                                    //    hat[split_att_type[j]] = deentry;
                                    //}
                                    //else if (split_att_type[j] == "PER")
                                    //{
                                    //    deentry = Convert.ToDouble(hat["P"]);
                                    //    deentry = deentry + 0.5;
                                    //    hat["P"] = deentry;
                                    //}
                                }
                            }
                        }
                        else
                        {
                            mycolidx += 2;
                        }
                    }
                    else
                    {
                        if (rdbformat1.Checked == true)
                        {
                            mycolidx++;
                            Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, mycolidx].Text = Convert.ToString("-");
                            mycolidx++;
                            Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, mycolidx].Text = Convert.ToString("-");
                        }
                    }
                    //}
                    //}
                    //else
                    //{
                    //    if (rdbformat1.Checked == true)
                    //    {
                    //        mycolidx++;
                    //        Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, mycolidx].Text = Convert.ToString("-");
                    //        mycolidx++;
                    //        Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, mycolidx].Text = Convert.ToString("-");
                    //    }
                    //}
                    //}
                    //else
                    //{
                    //if (dt_staff_attnd.Rows.Count > 0)
                    //{
                    //    for (int day = 3 + start_day; day < dt_staff_attnd.Columns.Count; day++)
                    //    {
                    //        flag = true;
                    //        string att_type = dt_staff_attnd.Rows[0][day].ToString();
                    //        string[] split_att_type = att_type.Split(new char[] { '-' });
                    //        if (split_att_type.Length > 1)
                    //        {
                    //            if (rdbformat1.Checked == true)
                    //            {
                    //                if (split_att_type.Length == 2)
                    //                {
                    //                    mycolidx++;
                    //                    if (split_att_type[0] != " ")
                    //                        Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, mycolidx].Text = Convert.ToString(split_att_type[0]);
                    //                    else
                    //                        Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, mycolidx].Text = Convert.ToString("-");
                    //                    mycolidx++;
                    //                    if (split_att_type[1] != " ")
                    //                        Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, mycolidx].Text = Convert.ToString(split_att_type[1]);
                    //                    else
                    //                        Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, mycolidx].Text = Convert.ToString("-");
                    //                }
                    //            }
                    //            for (int j = 0; j <= split_att_type.GetUpperBound(0); j++)
                    //            {
                    //                if (!string.IsNullOrEmpty(split_att_type[j]))
                    //                {
                    //                    //Modifeied  by srinath3/04/2014 
                    //                    if (hat.Contains(split_att_type[j]))
                    //                    {
                    //                        deentry = Convert.ToDouble(hat[split_att_type[j]]);
                    //                        deentry = deentry + 0.5;
                    //                        countwise = Convert.ToDouble(hat[split_att_type[j]]);
                    //                        countwise = countwise + 1;
                    //                        if (rbdaywise.Checked == true)
                    //                        {
                    //                            hat[split_att_type[j]] = deentry;
                    //                        }
                    //                        else if (rbcountwise.Checked == true)
                    //                        {
                    //                            if (split_att_type[j].Trim() == "P")
                    //                            {
                    //                                hat[split_att_type[j]] = deentry;
                    //                            }
                    //                            else
                    //                            {
                    //                                hat[split_att_type[j]] = countwise;
                    //                            }
                    //                        }
                    //                    }
                    //                    //if (split_att_type[j] != "PER")
                    //                    //{
                    //                    //    deentry = Convert.ToDouble(hat[split_att_type[j]]);
                    //                    //    deentry = deentry + 0.5;
                    //                    //    hat[split_att_type[j]] = deentry;
                    //                    //}
                    //                    //else if (split_att_type[j] == "PER")
                    //                    //{
                    //                    //    deentry = Convert.ToDouble(hat["P"]);
                    //                    //    deentry = deentry + 0.5;
                    //                    //    hat["P"] = deentry;
                    //                    //}
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    if (rdbformat1.Checked == true)
                    //    {
                    //        mycolidx++;
                    //        Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, mycolidx].Text = Convert.ToString("-");
                    //        mycolidx++;
                    //        Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, mycolidx].Text = Convert.ToString("-");
                    //    }
                    //}
                    //}
                    //}
                    //else
                    //{
                    //    goto mc;
                    //}
                    //ddttfrom = Convert.ToDateTime(splittodate[1] + "/" + Convert.ToString(ddttfrom.Day) + "/" + splittodate[2]);
                    ddttfrom = ddttfrom.AddDays(1);
                }                         //******************************* End *********************************
                //mc:
                //=========================bind attendance value in spread==============================
                //Format1 02-11-2016
                if (rdbformat1.Checked == true)
                {
                    Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, 5 + (zerocolidx * 2)].Text = no_of_workingdays.ToString();
                    Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, 6 + (zerocolidx * 2)].Text = holiday_count.ToString();
                    Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, 7 + (zerocolidx * 2)].Text = Convert.ToString(Convert.ToDouble(no_of_workingdays) + Convert.ToDouble(holiday_count));
                    for (int hat_value = 8 + (zerocolidx * 2); hat_value < Fp_StaffAttendance.Sheets[0].ColumnCount - 1; hat_value++)
                    {
                        string leavetype = Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, hat_value].Text.Trim();
                        double leavecount = Convert.ToDouble(hat[leavetype]);
                        Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, hat_value].Text = leavecount.ToString();
                        Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, hat_value].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
                else
                {
                    int CheckCount = 0;
                    int ColOrderCnt = ItemList.Count + 1;//Added by saranya on 30/8/2018
                    foreach (string key in htcolumn.Keys)
                    {
                        coltext1 = htcolumn[key].ToString();
                        //insdex = ItemList.IndexOf(Convert.ToString(coltext1));
                        if (ItemList.Contains(Convert.ToString(coltext1)))
                        {
                            if (coltext1.Trim() == "Total Present Days" || coltext1.Trim() == "Total Absent Days")
                            {
                                CheckCount++;
                            }
                        }
                    }
                    if (CheckCount > 0)
                    {
                        ColOrderCnt = (ItemList.Count + 1) - CheckCount;
                    }
                    for (int hat_value = ColOrderCnt; hat_value < Fp_StaffAttendance.Sheets[0].ColumnCount - 1; hat_value++)
                    {
                        string leavetype = Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, hat_value].Text.Trim();
                        double leavecount = Convert.ToDouble(hat[leavetype]);
                        Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, hat_value].Text = leavecount.ToString();
                        Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, hat_value].HorizontalAlign = HorizontalAlign.Center;

                    }
                }
                double salaryday = 0;
                for (int dv = 0; dv < dv_salarydaycount.Count; dv++)
                {
                    salaryday = salaryday + Convert.ToDouble(hat[dv_salarydaycount[dv]["shortname"].ToString()]);
                }
                Double lopdays = 0;
                if (chklalop.Checked == true)
                {
                    double late = 0;
                    Double.TryParse(Convert.ToString(hat["LA"]), out late);
                    if (rbdaywise.Checked == true)
                    {
                        late = late * 2;
                    }
                    lopdays = lopval(late);
                    Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, Fp_StaffAttendance.Sheets[0].ColumnCount - 2].Text = lopdays.ToString();
                    Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, Fp_StaffAttendance.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                }
                double totalPresent = 0;
                double totalAbsent = 0;

                if (salaryday != 0)
                {
                    salaryday = salaryday + Convert.ToDouble(hat["A"]);
                    totalAbsent = salaryday;//Added by saranya for total absentdays
                }
                else
                {
                    salaryday = salaryday + Convert.ToDouble(hat["A"]) + Convert.ToDouble(hat["LOP"]);//delsi added + Convert.ToDouble(hat["LOP"])
                    totalAbsent = salaryday;//Added by saranya for total absentdays
                }

                if (chklalop.Checked == true)
                {
                    if (rbcountwise.Checked == true)
                    {
                        salaryday = (Convert.ToDouble(no_of_workingdays) + Convert.ToDouble(holiday_count)) - (salaryday / 2) - lopdays;
                        totalPresent = salaryday;//Added by saranya for total Presentdays
                    }
                    else if (rbdaywise.Checked == true)
                    {
                        salaryday = (Convert.ToDouble(no_of_workingdays) + Convert.ToDouble(holiday_count)) - salaryday - lopdays;
                        totalPresent = salaryday;//Added by saranya for total Presentdays
                    }
                }
                else
                {
                    if (rbcountwise.Checked == true)
                    {
                        salaryday = (Convert.ToDouble(no_of_workingdays) + Convert.ToDouble(holiday_count)) - (salaryday / 2);
                        totalPresent = salaryday;//Added by saranya for total Presentdays
                    }
                    else if (rbdaywise.Checked == true)
                    {
                        salaryday = (Convert.ToDouble(no_of_workingdays) + Convert.ToDouble(holiday_count)) - salaryday;
                        totalPresent = salaryday;//Added by saranya for total Presentdays

                    }
                }
                if (rdbformat2.Checked == true)
                {
                    //========Added by saranya on 30/8/2018=============//

                    for (int hat_value = 0; hat_value < Fp_StaffAttendance.Sheets[0].ColumnCount - 1; hat_value++)
                    {
                        string leavetype = Fp_StaffAttendance.Sheets[0].ColumnHeader.Cells[0, hat_value].Text.Trim();
                        if (leavetype == "Total Present Days")
                        {
                            Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, hat_value].Text = totalPresent.ToString();
                            Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, hat_value].HorizontalAlign = HorizontalAlign.Center;
                        }
                        if (leavetype == "Total Absent Days")
                        {
                            Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, hat_value].Text = totalAbsent.ToString();
                            Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, hat_value].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, Fp_StaffAttendance.Sheets[0].ColumnCount - 3].Text = salaryday.ToString();
                    Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, Fp_StaffAttendance.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                    //=======================================================//
                    //  percantageval = (salaryday / no_of_workingdays) * 100;

                    percantageval = (salaryday / totalworkingdays) * 100;
                    percantageval = Math.Round(percantageval, 0, MidpointRounding.AwayFromZero);
                    overallpercentage = overallpercentage + percantageval;
                    Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, Fp_StaffAttendance.Sheets[0].ColumnCount - 2].Text = Convert.ToString(percantageval);
                    Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, Fp_StaffAttendance.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                }
                else
                {
                    Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, Fp_StaffAttendance.Sheets[0].ColumnCount - 2].Text = salaryday.ToString();
                    Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, Fp_StaffAttendance.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                }
                //======================================End=============================================
                //}
                str_deparment = dt_staffdetails.Rows[i]["Dept_Name"].ToString();
            }
            if (rdbformat2.Checked == true)
            {
                double totalpercentage = (overallpercentage / totalrecord);//delsi0312
                Fp_StaffAttendance.Sheets[0].RowCount++;
                Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, Fp_StaffAttendance.Sheets[0].ColumnCount - 3].Text = "Grand Total";//delsi0312
                totalpercentage = Math.Round(totalpercentage, 0, MidpointRounding.AwayFromZero);
                Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, Fp_StaffAttendance.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, Fp_StaffAttendance.Sheets[0].ColumnCount - 2].Text = Convert.ToString(totalpercentage);
                Fp_StaffAttendance.Sheets[0].Cells[Fp_StaffAttendance.Sheets[0].RowCount - 1, Fp_StaffAttendance.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                Fp_StaffAttendance.Sheets[0].SpanModel.Add(Fp_StaffAttendance.Sheets[0].RowCount - 1, 0, 1, Fp_StaffAttendance.Sheets[0].ColumnCount - 3);

            }
            con.Close();
            if (Fp_StaffAttendance.Sheets[0].RowCount > 0)
            {
                lblerrormsg.Visible = false;
                Fp_StaffAttendance.Visible = true;
                if (rdbformat1.Checked == true)
                {
                    Fp_StaffAttendance.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fp_StaffAttendance.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                    Fp_StaffAttendance.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
                    Fp_StaffAttendance.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fp_StaffAttendance.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                    Fp_StaffAttendance.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
                }
                Fp_StaffAttendance.Sheets[0].FrozenColumnCount = 5;
                Fp_StaffAttendance.Sheets[0].PageSize = Fp_StaffAttendance.Sheets[0].RowCount;
                //Fp_StaffAttendance.Width = 900;
                Fp_StaffAttendance.Height = 1000;
                btnprintmaster.Visible = true;
                lblxl.Visible = true;
                txtxl.Visible = true;
                btnxl.Visible = true;
            }
            else
            {
                Fp_StaffAttendance.Visible = false;
                lblerrormsg.Text = "No records are found";
                lblerrormsg.Visible = true;
                btnprintmaster.Visible = false;
                lblxl.Visible = false;
                txtxl.Visible = false;
                btnxl.Visible = false;
            }
        }
        catch (TimeoutException ex)
        {
            lblerrormsg.Text = "ERROR: Time out please try again by clicking go button";
            lblerrormsg.Visible = true;
        }
        catch (RowNotInTableException ex)
        {
            lblerrormsg.Text = "ERROR: Invalid attempt to data when no rows in table";
            lblerrormsg.Visible = true;
        }
        catch (InvalidOperationException ex)
        {
            lblerrormsg.Text = "ERROR: Invalid Operation Occured";
            lblerrormsg.Visible = true;
        }
        catch (ArithmeticException ex)
        {
            lblerrormsg.Text = "ERROR: Cannot Divided by Zero";
            lblerrormsg.Visible = true;
        }
        catch (ArgumentOutOfRangeException ex)
        {
            lblerrormsg.Text = "ERROR: Argument out of range";
            lblerrormsg.Visible = true;
        }
        catch (FormatException ex)
        {
            lblerrormsg.Text = "ERROR: Invalid datatype in a parameter ";
            lblerrormsg.Visible = true;
        }
        catch (IndexOutOfRangeException ex)
        {
            lblerrormsg.Text = "ERROR: Index out of range";
            lblerrormsg.Visible = true;
        }
        catch (InsufficientMemoryException ex)
        {
            lblerrormsg.Text = "ERROR: Insufficient Memory";
            lblerrormsg.Visible = true;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Session["column_header_row_count"] = 1;
        string deg_details = string.Empty;
        string date_pdf = string.Empty;
        string header = string.Empty;
        string degreedetails = string.Empty;
        string pagename = "StaffAttendanceReport.aspx";
        degreedetails = "Staff Attendance Report" + '@' + "Date :" + txtfromdate.Text.ToString() + " to " + txttodate.Text.ToString();
        Printcontrol.loadspreaddetails(Fp_StaffAttendance, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtxl.Text;
            if (reportname.ToString().Trim() != "")
            {
                daccess.printexcelreport(Fp_StaffAttendance, reportname);
            }
            else
            {
                lblerrormsg.Text = "Please Enter Your Report Name";
                lblerrormsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    #region Added by saranya on 29/08/2018 for Format2

    protected void rbdformat1_OnCheckedChanged(object sender, EventArgs e)
    {
        pheaderfilter.Visible = true;
        pbodyfilter.Visible = true;
        PFormat2HeaderFilter.Visible = false;
        PFormat2BodyFilter.Visible = false;
    }

    protected void rbdformat2_OnCheckedChanged(object sender, EventArgs e)
    {
        pheaderfilter.Visible = false;
        pbodyfilter.Visible = false;
        PFormat2HeaderFilter.Visible = true;
        PFormat2BodyFilter.Visible = true;
    }

    #region Column_Order

    protected void cblcolumnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string value = "";
            int index;
            // ItemList.Clear();
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblsearch.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    ItemList.Add(cblsearch.Items[index].Value.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cblsearch.Items[index].Value.ToString());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cblsearch.Items.Count; i++)
            {
                if (cblsearch.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblsearch.Items[i].Value.ToString());
                    Itemindex.Remove(sindex);
                }
            }
            tborder.Visible = true;
            tborder.Text = "";
            string colname12 = "";
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (colname12 == "")
                {
                    colname12 = ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
                }
                else
                {
                    colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
                }
            }
            tborder.Text = colname12;
            if (ItemList.Count == 0)
            {
                tborder.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }

    }

    #endregion

    #endregion

}

//--------------Last Modified By Jeyaprakash on Nov 2nd,2016---------------//
//--------------Staff Attendance for the individual Day Added--------------//