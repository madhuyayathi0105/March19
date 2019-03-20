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
using Gios.Pdf;
using System.IO;
using FarPoint.Web.Spread;


public partial class MonthlyCummulativeSalary : System.Web.UI.Page
{
    Hashtable splallow = new Hashtable();
    Hashtable allow = new Hashtable();
    Hashtable hatt = new Hashtable();
    DAccess2 dac = new DAccess2();
    SortedDictionary<int, double> dicaddtot = new SortedDictionary<int, double>();
    SortedDictionary<int, double> dicaddgrandtot = new SortedDictionary<int, double>();
    static string[] splallw_arry = new string[15];
    static string[] spll_alll_tag_arry = new string[15];
    static string[] allow_arry = new string[15];
    string gssmcat = "";
    string gssmdept = "";
    SqlConnection myconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection myconn1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);

    SqlConnection mysql1 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection mysql2 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection mysql3 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    string user_code, college_code;
    [Serializable()]
    public class MyImg : ImageCellType
    {
        //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, Boolean upperLevel)
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(75);
            return img;
        }
    }
    DataSet ds = new DataSet();
    DataSet dsbind = new DataSet();
    DataSet dset = new DataSet();

    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlDataAdapter da = new SqlDataAdapter();
    static int seatcnt = 0;
    double IntMTotal;
    double IntMTemp;
    double netpaytotal;
    int getval2;
    int col2 = 0;
    static int bloodcnt = 0;
    string[] seatcode = new string[44];
    int[] seatindex = new int[44];
    int[] bloodindex = new int[44];
    string sql1;
    string[] bloodvalue = new string[55];
    string[] bloodcode = new string[55];
    double[] DblAllowTotal = new double[50];
    double[] deductiontotal = new double[50];
    double[] splAllowTotal = new double[50];
    int colheder;
    double basicpaytotal = 0;
    string[] seatvalue = new string[55];
    string sql;
    int col;
    string mname;
    string[] allowanmce_arr1;
    string gstrdept = "";
    string gstrcateogry = "";
    string strdept = "";
    string strcategory = "";
    string strallallowance = "";
    string stralldeduct = "";
    string deduct = "";
    string da3;
    double DblNetAllowTotal = 0;
    double DblNetDedTotal = 0;
    int getval;
    int j = 0;
    static int allowancecnt = 0;
    static int deductioncnt = 0;
    string[] deductioncode = new string[44];
    int[] deductionindex = new int[44];
    string[] allowancecode = new string[44];
    int[] allowanceindex = new int[44];
    string[] allowancevalue = new string[50];
    string[] dedctionvalue = new string[44];
    string codename = "";
    //Added By Srinath 1/4/2013


    string group_user = "";
    DAccess2 d2 = new DAccess2();
    string fin_startdate = "", fin_enddate = "", acct_id = "";

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
            lblnorec.Visible = false;
            if (!IsPostBack)
            {

                fpsalarydemond.Visible = false;
                btnprintmaster.Visible = false;
                btnxl.Visible = false;
                btnsal.Visible = false;
                lblexcel.Visible = false;
                txtxl.Visible = false;
                college_code = Session["collegecode"].ToString();
                con.Close();
                con.Open();
                DataSet dsscol1 = new DataSet();
                string str1 = "select distinct account_info.acct_id from account_info,acctinfo where account_info.acct_id=acctinfo.acct_id and college_code=" + Session["collegecode"].ToString() + " and finyear_start='" + fin_startdate + "' and finyear_end='" + fin_enddate + "'";
                SqlDataAdapter col1 = new SqlDataAdapter(str1, con);
                col1.Fill(dsscol1);
                if (dsscol1.Tables[0].Rows.Count > 0)
                {
                    acct_id = dsscol1.Tables[0].Rows[0][0].ToString();
                }
                string dtchss = DateTime.Today.ToShortDateString();
                string[] dsplitchss = dtchss.Split(new Char[] { '/' });


                DateTime fromdate, todate;
                todate = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                fromdate = Convert.ToDateTime(todate);
                fromdate = fromdate - TimeSpan.FromDays(7);

                string today = System.DateTime.Now.ToString();
                string today1;
                string[] split13 = today.Split(new char[] { ' ' });
                string[] split14 = split13[0].Split(new Char[] { '/' });
                today1 = split14[1].ToString() + "/" + split14[0].ToString() + "/" + split14[2].ToString();

                string today2 = System.DateTime.Now.ToString();
                string today3;
                string[] split15 = today.Split(new char[] { ' ' });
                string[] split16 = split13[0].Split(new Char[] { '/' });
                today3 = split16[1].ToString() + "/" + split16[0].ToString() + "/" + split16[2].ToString();
                load_batchyear();
                load_dept();
                load_category();
                load_allowance();
                if (cblmonthfrom.SelectedItem.ToString() == "All")
                {
                    cbotomonth.Visible = false;
                    lblto.Visible = false;
                }
                else
                {
                    cbotomonth.Visible = true;
                    lblto.Visible = true;
                }
                college_code = Session["collegecode"].ToString();

                user_code = Session["usercode"].ToString();

                pnldemond.Visible = true;
                btngo.Visible = true;
                staff();
            }
        }
        catch (Exception ex) { }
    }
    protected void btnaddnew_Click(object sender, EventArgs e)
    {
        //bindheader();
        //Acctheader_SelectedIndexChanged(sender, e);
        load_batchyear();
        load_dept();
        load_category();
        load_allowance();
        tbseattype.Text = "---Select---";
        tbblood.Text = "---Select---";
        cblmonthfrom.SelectedIndex = 0;
        cbotomonth.Visible = false;
        lblto.Visible = false;
        cblbatchyear.Enabled = true;
        cblmonthfrom.Enabled = true;
        cbotomonth.Enabled = true;
        fpsalarydemond.Visible = false;
        pnldemond.Visible = true;

    }

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {
        System.Web.UI.Control cntUpdateBtnn = fpsalarydemond.FindControl("Update");
        System.Web.UI.Control cntCancelBtnn = fpsalarydemond.FindControl("Cancel");
        System.Web.UI.Control cntCopyBtnn = fpsalarydemond.FindControl("Copy");
        System.Web.UI.Control cntCutBtnn = fpsalarydemond.FindControl("Clear");
        System.Web.UI.Control cntPasteBtnn = fpsalarydemond.FindControl("Paste");
        System.Web.UI.Control cntPageNextBtnn = fpsalarydemond.FindControl("Next");
        System.Web.UI.Control cntPagePreviousBtnn = fpsalarydemond.FindControl("Prev");
        System.Web.UI.Control cntPagePrintBtnn = fpsalarydemond.FindControl("Print");

        if ((cntUpdateBtnn != null))
        {
            TableCell tc = (TableCell)cntUpdateBtnn.Parent;
            TableRow tr = (TableRow)tc.Parent;

            tr.Cells.Remove(tc);

            tc = (TableCell)cntCancelBtnn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntCopyBtnn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntCutBtnn.Parent;
            tr.Cells.Remove(tc);


            tc = (TableCell)cntPasteBtnn.Parent;
            tr.Cells.Remove(tc);

            tr.Cells.Remove(tc);

        }

        base.Render(writer);
    }
    void load_batchyear()
    {
        cblbatchyear.Visible = true;
        ddl_toyear.Visible = true;
        ds.Clear();
        // ListItem lsitem = new ListItem();
        con.Close();
        con.Open();
        SqlCommand cmd = new SqlCommand("select distinct year(fdate) as year from monthlypay order by year desc ", con);
        da.SelectCommand = cmd;
        da.Fill(ds);
        cblbatchyear.DataSource = ds.Tables[0];
        cblbatchyear.DataTextField = "Year";
        cblbatchyear.DataValueField = "year";
        cblbatchyear.DataBind();
        ddl_toyear.DataSource = ds.Tables[0];
        ddl_toyear.DataTextField = "Year";
        ddl_toyear.DataValueField = "year";
        ddl_toyear.DataBind();
        con.Close();
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
            deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"] + "') order by dept_name ";
        }
        else
        {
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"] + "') ";
        }
        if (deptquery != "")
        {
            ds = d2.select_method(deptquery, allow, "Text");
            cbldepttype.DataSource = ds.Tables[0];
            cbldepttype.DataTextField = "dept_name";
            cbldepttype.DataValueField = "dept_code";
            cbldepttype.DataBind();
        }

        for (int i = 0; i < cbldepttype.Items.Count; i++)
        {
            cbldepttype.Items[i].Selected = true;
            tbseattype.Text = "Dept(" + i.ToString() + ")";
            chkselect.Checked = true;
        }

        con.Close();

    }
    void load_category()
    {
        cblcategory.Visible = true;
        cblcategory.Items.Clear();
        ds.Clear();
        con.Open();
        SqlCommand cmd = new SqlCommand("select  distinct category_code,category_name from staffcategorizer where college_code='" + Session["collegecode"] + "' order by category_code", con);
        da.SelectCommand = cmd;
        da.Fill(ds);
        cblcategory.DataSource = ds.Tables[0];
        cblcategory.DataTextField = "category_name";
        cblcategory.DataValueField = "category_code";
        cblcategory.DataBind();
        for (int i = 0; i < cblcategory.Items.Count; i++)
        {
            cblcategory.Items[i].Selected = true;
            tbblood.Text = "Category(" + i.ToString() + ")";
        }
        con.Close();
    }

    void loadmulallowance()
    {
        txtallowance.Text = "--Select--";
        chkallowance.Checked = false;
        cblallowance.Items.Clear();
        cbldeduction.Items.Clear();
        txtdeduction.Text = "--Select--";
        Chkdeduction.Checked = false;
        if (cbSelect.Checked == true)
        {
            Hashtable htab = new Hashtable();
            cblallowance.Items.Clear();
            cbldeduction.Items.Clear();
            ds.Clear();
            con.Open();
            string year = "";
            if (cblbatchyear.SelectedItem.Text.Trim() != "")
            {
                year = cblbatchyear.SelectedItem.Text;
            }
            SqlCommand cmd;
            if (tbseattype.Text.Trim() != "---Select---")
            {
                if (txtstfname.Text.Trim() != "---Select---")
                {
                    string d = ("select * from incentives_master where college_code='" + Session["collegecode"] + "'");
                    cmd = new SqlCommand("select * from incentives_master where college_code='" + Session["collegecode"] + "'", con);

                    SqlDataReader dr;
                    dr = cmd.ExecuteReader();
                    string allowanmce = "";
                    string detection = "";

                    while (dr.Read())
                    {
                        if (dr.HasRows == true)
                        {
                            allowanmce = dr["allowances"].ToString();
                            detection = dr["deductions"].ToString();

                            string[] allowanmce_arr;
                            allowanmce_arr = allowanmce.Split(';');

                            for (int i = 0; i <= allowanmce_arr.GetUpperBound(0); i++)
                            {
                                string all2 = allowanmce_arr[i];
                                string[] splitallo3 = all2.Split('\\');
                                if (splitallo3[0].Trim() != "")
                                {
                                    all2 = splitallo3[0];
                                    if (all2.Trim() != "" && all2.Trim() != "0")
                                    {
                                        cblallowance.Items.Add(all2);
                                        cblallowance.Items[i].Selected = true;
                                        chkallowance.Checked = true;
                                        htab.Add(all2, all2);
                                    }
                                }
                            }

                            string valu1 = "";
                            string code1 = "";
                            string value2 = "";
                            string code2 = "";
                            int deductioncount = 0;
                            int allowancecount = 0;
                            for (int i = 0; i < cblallowance.Items.Count; i++)
                            {
                                if (cblallowance.Items[i].Selected == true)
                                {
                                    valu1 = cblallowance.Items[i].Text;
                                    code1 = cblallowance.Items[i].Value.ToString();
                                    allowancecount = allowancecount + 1;
                                    txtallowance.Text = "Allowance(" + allowancecount.ToString() + ")";
                                }
                            }
                            string[] detection_arr;
                            detection_arr = detection.Split(';');
                            for (int j = 0; j <= detection_arr.GetUpperBound(0); j++)
                            {
                                string all2 = detection_arr[j];
                                string[] splitallo3 = all2.Split('\\');
                                if (splitallo3[0].Trim() != "")
                                {
                                    all2 = splitallo3[0];
                                    if (all2.Trim() != "" && all2.Trim() != "0")
                                    {
                                        cbldeduction.Items.Add(all2);
                                        cbldeduction.Items[j].Selected = true;
                                        Chkdeduction.Checked = true;
                                        htab.Add(all2, all2);
                                    }
                                }
                            }
                            for (int i = 0; i < cbldeduction.Items.Count; i++)
                            {
                                if (cbldeduction.Items[i].Selected == true)
                                {
                                    value2 = cbldeduction.Items[i].Text;
                                    code2 = cbldeduction.Items[i].Value.ToString();
                                    deductioncount = deductioncount + 1;
                                    txtdeduction.Text = "Deduction(" + deductioncount.ToString() + ")";
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            cblallowance.Items.Clear();
            cbldeduction.Items.Clear();
            ds.Clear();
            con.Open();
            SqlCommand cmd;
            cmd = new SqlCommand("Select * from incentives_master where college_code=" + Session["collegecode"] + "", con);
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            string allowanmce = "";
            string detection = "";

            while (dr.Read())
            {
                if (dr.HasRows == true)
                {
                    allowanmce = dr["allowances"].ToString();
                    detection = dr["deductions"].ToString();

                }
            }
            string[] allowanmce_arr;
            allowanmce_arr = allowanmce.Split(';');

            for (int i = 0; i <= allowanmce_arr.GetUpperBound(0); i++)
            {
                string all2 = allowanmce_arr[i];
                string[] splitallo3 = all2.Split('\\');
                if (splitallo3[0].Trim() != "")
                {
                    all2 = splitallo3[0];
                    if (all2.Trim() != "" && all2.Trim() != "0")
                    {
                        cblallowance.Items.Add(all2);
                        cblallowance.Items[i].Selected = true;
                        chkallowance.Checked = true;
                    }
                }
            }

            string valu1 = "";
            string code1 = "";
            string value2 = "";
            string code2 = "";
            int deductioncount = 0;
            int allowancecount = 0;
            for (int i = 0; i < cblallowance.Items.Count; i++)
            {
                if (cblallowance.Items[i].Selected == true)
                {
                    valu1 = cblallowance.Items[i].Text;
                    code1 = cblallowance.Items[i].Value.ToString();
                    allowancecount = allowancecount + 1;
                    txtallowance.Text = "Allowance(" + allowancecount.ToString() + ")";
                }
            }
            string[] detection_arr;
            detection_arr = detection.Split(';');
            for (int j = 0; j <= detection_arr.GetUpperBound(0); j++)
            {
                string all2 = detection_arr[j];
                string[] splitallo3 = all2.Split('\\');
                if (splitallo3[0].Trim() != "")
                {
                    all2 = splitallo3[0];
                    if (all2.Trim() != "" && all2.Trim() != "0")
                    {
                        cbldeduction.Items.Add(all2);
                        cbldeduction.Items[j].Selected = true;
                        Chkdeduction.Checked = true;
                    }
                }
            }
            for (int i = 0; i < cbldeduction.Items.Count; i++)
            {
                if (cbldeduction.Items[i].Selected == true)
                {
                    value2 = cbldeduction.Items[i].Text;
                    code2 = cbldeduction.Items[i].Value.ToString();
                    deductioncount = deductioncount + 1;
                    txtdeduction.Text = "Deduction(" + deductioncount.ToString() + ")";
                }
            }
        }
        con.Close();
    }

    void load_allowance()
    {
        txtallowance.Text = "--Select--";
        chkallowance.Checked = false;
        txtdeduction.Text = "--Select--";
        Chkdeduction.Checked = false;
        if (cbSelect.Checked == true)
        {
            Hashtable htab = new Hashtable();
            cblallowance.Items.Clear();
            cbldeduction.Items.Clear();
            ds.Clear();
            con.Open();
            int seatcount = 0;
            int allcount = 0;
            string year = "";
            if (cblbatchyear.SelectedItem.Text.Trim() != "")
            {
                year = cblbatchyear.SelectedItem.Text;
            }
            string stafcode = "";
            SqlCommand cmd;
            for (int i = 0; i < cbldepttype.Items.Count; i++)
            {
                if (cbldepttype.Items[i].Selected == true)
                {
                    stafcode = ddlstfnam.SelectedItem.Value;
                }
            }
            //if (cblmonthfrom.SelectedItem.Text == "All")
            //{
            string d = ("select allowances,deductions from monthlypay where college_code='" + Session["collegecode"] + "' and staff_code = '" + stafcode + "'");
            cmd = new SqlCommand("select allowances,deductions from monthlypay where college_code='" + Session["collegecode"] + "' and staff_code = '" + stafcode + "'", con);
            //   }

            ////string h = "select allowances,deductions from monthlypay where college_code='" + Session["collegecode"] + "' and staff_code = '" + stafcode + "'  and fdate >= '" + year + "-" + cblmonthfrom.SelectedItem.Value + "-" + "1" + "' and tdate <= '" + year + "-" + cbotomonth.SelectedItem.Value + "-" + "30" + "'";
            //   else
            //   {
            //       string f = "select allowances,deductions from monthlypay where college_code='" + Session["collegecode"] + "' and staff_code = '" + stafcode + "'  and fdate >= '" + year + "-" + cblmonthfrom.SelectedItem.Value + "-" + "1" + "' and tdate <= '" + year + "-" + cbotomonth.SelectedItem.Value + "-" + "30" + "'";
            //       cmd = new SqlCommand("select allowances,deductions from monthlypay where college_code='" + Session["collegecode"] + "' and staff_code = '" + stafcode + "'  and fdate >= '" + year + "-" + cblmonthfrom.SelectedItem.Value + "-" + "1" + "' and tdate <= '" + year + "-" + cbotomonth.SelectedItem.Value + "-" + "30" + "'", con);
            //   }

            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            string allowanmce = "";
            string detection = "";

            while (dr.Read())
            {
                if (dr.HasRows == true)
                {
                    allowanmce = dr["allowances"].ToString();
                    detection = dr["deductions"].ToString();


                    string[] allowanmce_arr;
                    allowanmce_arr = allowanmce.Split('\\');
                    int z = 0;
                    for (int i = 0; i <= allowanmce_arr.GetUpperBound(0); i++)
                    {

                        string all2 = allowanmce_arr[i];
                        string[] splitallo3 = all2.Split(';');
                        int sricheck = splitallo3.GetUpperBound(0);
                        if (splitallo3.GetUpperBound(0) >= 0)
                        {
                            all2 = splitallo3[0];
                            if (all2.Trim() != "" && all2.Trim() != "0")
                            {
                                if (!htab.ContainsKey(all2))
                                {
                                    cblallowance.Items.Add(all2);
                                    chkallowance.Checked = true;
                                    cblallowance.Items[z].Selected = true;
                                    z++;
                                    htab.Add(all2, all2);
                                }
                            }
                        }
                    }
                    string valu1 = "";
                    string code1 = "";
                    string value2 = "";
                    string code2 = "";
                    int deductioncount = 0;
                    int allowancecount = 0;
                    for (int i = 0; i < cblallowance.Items.Count; i++)
                    {
                        if (cblallowance.Items[i].Selected == true)
                        {
                            valu1 = cblallowance.Items[i].Text;
                            code1 = cblallowance.Items[i].Value.ToString();
                            allowancecount = allowancecount + 1;
                            txtallowance.Text = "Allowance(" + allowancecount.ToString() + ")";
                        }
                    }
                    string[] detection_arr;
                    detection_arr = detection.Split('\\');
                    int x = 0;
                    for (int j = 0; j <= detection_arr.GetUpperBound(0); j++)
                    {
                        string all2 = detection_arr[j];
                        string[] splitallo3 = all2.Split(';');
                        if (splitallo3.GetUpperBound(0) >= 0)
                        {
                            all2 = splitallo3[0];
                            if (all2.Trim() != "" && all2.Trim() != "0")
                            {
                                if (!htab.ContainsKey(all2))
                                {
                                    cbldeduction.Items.Add(all2);
                                    cbldeduction.Items[x].Selected = true;
                                    x++;
                                    htab.Add(all2, all2);
                                    Chkdeduction.Checked = true;
                                }
                            }
                        }
                    }
                    for (int i = 0; i < cbldeduction.Items.Count; i++)
                    {
                        if (cbldeduction.Items[i].Selected == true)
                        {
                            value2 = cbldeduction.Items[i].Text;
                            code2 = cbldeduction.Items[i].Value.ToString();
                            deductioncount = deductioncount + 1;
                        }

                    }
                }

            }
            if (allcount == cblallowance.Items.Count)
            {
                txtallowance.Text = "--Select--";
                chkallowance.Checked = true;
            }
            else if (allcount == 0)
            {
                txtallowance.Text = "--Select--";
            }
            else
            {
                txtallowance.Text = "Allowance(" + allcount.ToString() + ")";
                chkallowance.Checked = true;
            }
            if (seatcount == cbldeduction.Items.Count)
            {

                txtdeduction.Text = "--Select--";
                Chkdeduction.Checked = true;
            }
            else if (seatcount == 0)
            {

                txtdeduction.Text = "--Select--";
            }
            else
            {
                txtdeduction.Text = "Deduction(" + seatcount.ToString() + ")";
            }
        }
        else
        {
            cblallowance.Items.Clear();
            cbldeduction.Items.Clear();
            ds.Clear();
            con.Open();
            SqlCommand cmd;
            cmd = new SqlCommand("Select * from incentives_master where college_code=" + Session["collegecode"] + "", con);
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            string allowanmce = "";
            string detection = "";

            while (dr.Read())
            {
                if (dr.HasRows == true)
                {
                    allowanmce = dr["allowances"].ToString();
                    detection = dr["deductions"].ToString();

                }
            }
            string[] allowanmce_arr;
            allowanmce_arr = allowanmce.Split(';');

            for (int i = 0; i <= allowanmce_arr.GetUpperBound(0); i++)
            {
                string all2 = allowanmce_arr[i];
                string[] splitallo3 = all2.Split('\\');
                if (splitallo3[0].Trim() != "")
                {
                    all2 = splitallo3[0];
                    if (all2.Trim() != "" && all2.Trim() != "0")
                    {
                        cblallowance.Items.Add(all2);
                        cblallowance.Items[i].Selected = true;
                        chkallowance.Checked = true;
                    }
                }
            }

            string valu1 = "";
            string code1 = "";
            string value2 = "";
            string code2 = "";
            int deductioncount = 0;
            int allowancecount = 0;
            for (int i = 0; i < cblallowance.Items.Count; i++)
            {
                if (cblallowance.Items[i].Selected == true)
                {
                    valu1 = cblallowance.Items[i].Text;
                    code1 = cblallowance.Items[i].Value.ToString();
                    allowancecount = allowancecount + 1;
                    txtallowance.Text = "Allowance(" + allowancecount.ToString() + ")";
                }
            }
            string[] detection_arr;
            detection_arr = detection.Split(';');
            for (int j = 0; j <= detection_arr.GetUpperBound(0); j++)
            {
                string all2 = detection_arr[j];
                string[] splitallo3 = all2.Split('\\');
                if (splitallo3[0].Trim() != "")
                {
                    all2 = splitallo3[0];
                    if (all2.Trim() != "" && all2.Trim() != "0")
                    {
                        cbldeduction.Items.Add(all2);
                        cbldeduction.Items[j].Selected = true;
                        Chkdeduction.Checked = true;
                    }
                }
                //else
                //{
                //    cbldeduction.Items.Add(detection_arr[j]);
                //    cbldeduction.Items[j].Selected = true;
                //    Chkdeduction.Checked = true;
                //}

            }
            for (int i = 0; i < cbldeduction.Items.Count; i++)
            {
                if (cbldeduction.Items[i].Selected == true)
                {
                    value2 = cbldeduction.Items[i].Text;
                    code2 = cbldeduction.Items[i].Value.ToString();
                    deductioncount = deductioncount + 1;
                    txtdeduction.Text = "Deduction(" + deductioncount.ToString() + ")";
                }
            }
        }
        con.Close();
    }



    public Label seatlabel()
    {
        Label lbc = new Label();
        ViewState["lseatcontrol"] = true;
        return (lbc);
    }
    public Label bloodlabel()
    {
        Label lbc = new Label();
        ViewState["lbloodcontrol"] = true;
        return (lbc);
    }
    protected void cbldepttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        pseattype.Focus();
        cbSelect.Checked = false;
        int seatcount = 0;
        string value = "";
        string code = "";
        load_allowance();
        for (int i = 0; i < cbldepttype.Items.Count; i++)
        {
            if (cbldepttype.Items[i].Selected == true)
            {

                lblstafnam.Visible = false;
                cbSelect.Checked = false;
                ddlstfnam.Visible = false;
                value = cbldepttype.Items[i].Text;
                code = cbldepttype.Items[i].Value.ToString();
                seatcount = seatcount + 1;
                tbseattype.Text = "Department(" + seatcount.ToString() + ")";
            }

        }

        if (seatcount == 0)
            tbseattype.Text = "---Select---";
        else
        {

            Label lbl = seatlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = seatimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(seatimg_Click);
        }
        seatcnt = seatcount;
        staff();
    }
    public void seatimg_Click(object sender, ImageClickEventArgs e)
    {
        seatcnt = seatcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cbldepttype.Items[r].Selected = false;

        tbseattype.Text = "Department(" + seatcnt.ToString() + ")";
        if (tbseattype.Text == "Department(0)")
        {
            tbseattype.Text = "---Select---";
        }
    }
    public ImageButton seatimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }
    protected void tbseattype_TextChanged(object sender, EventArgs e)
    {

    }

    protected void LinkButtonseattype_Click(object sender, EventArgs e)
    {
        cbldepttype.ClearSelection();
        seatcnt = 0;
        tbseattype.Text = "---Select---";
    }

    protected void cblcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        int bloodcount = 0;
        string value = "";
        string code = "";
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
            Label lbl = bloodlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl2-" + code.ToString();
            ImageButton ib = bloodimage();
            ib.ID = "imgbut2_" + code.ToString();
            ib.Click += new ImageClickEventHandler(bloodimg_Click);
        }
        bloodcnt = bloodcount;
    }

    public ImageButton bloodimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["ibloodcontrol"] = true;
        return (imc);
    }
    public void bloodimg_Click(object sender, ImageClickEventArgs e)
    {
        bloodcnt = bloodcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cblcategory.Items[r].Selected = false;
        tbblood.Text = "Blood Group(" + bloodcnt.ToString() + ")";
        if (tbblood.Text == "Blood Group(0)")
        {
            tbblood.Text = "---Select---";
        }
    }



    protected void tbblood_TextChanged(object sender, EventArgs e)
    {

    }

    protected void LinkButtonblood_Click(object sender, EventArgs e)
    {
        cblcategory.ClearSelection();
        bloodcnt = 0;
        tbblood.Text = "---Select---";
    }

    public string getmonth(string mname)
    {
        string month = "";
        if (mname == "1")
        {
            month = "January";
            return month;
        }
        else if (mname == "2")
        {
            month = "February";

        }
        else if (mname == "3")
        {
            month = "March";

        }
        else if (mname == "4")
        {
            month = "April";

        }
        else if (mname == "5")
        {
            month = "May";

        }
        else if (mname == "6")
        {
            month = "June";

        }
        else if (mname == "7")
        {
            month = "July";

        }
        else if (mname == "8")
        {
            month = "August";

        }
        else if (mname == "9")
        {
            month = "September";
        }
        else if (mname == "10")
        {
            month = "October";
        }
        else if (mname == "11")
        {
            month = "November";

        }
        else if (mname == "12")
        {
            month = "December";

        }
        return month;
    }

    protected void cblbatchyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        fpsalarydemond.Visible = false;
        btnprintmaster.Visible = false;
        btnxl.Visible = false;
        lblexcel.Visible = false;
        txtxl.Visible = false;
        btnsal.Visible = false;
    }
    protected void cblmonthfrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        fpsalarydemond.Visible = false;
        btnprintmaster.Visible = false;
        btnxl.Visible = false;
        lblexcel.Visible = false;
        txtxl.Visible = false;
        btnsal.Visible = false;
        if (cblmonthfrom.SelectedItem.ToString() == "All")
        {
            cbotomonth.Visible = false;
            lblto.Visible = false;
        }
        else
        {
            cbotomonth.SelectedValue = cblmonthfrom.SelectedValue;
            cbotomonth.Visible = true;
            lblto.Visible = true;
        }
        if (codename == "Approval")
        {
            cbotomonth.Visible = false;
            lblto.Visible = false;
        }
    }
    protected void chkselect_CheckedChanged(object sender, EventArgs e)
    {

        if (chkselect.Checked == true)
        {
            for (int i = 0; i < cbldepttype.Items.Count; i++)
            {
                cbSelect.Checked = false;
                lblstafnam.Visible = false;
                ddlstfnam.Visible = false;
                staff();
                cbldepttype.Items[i].Selected = true;
                chkselect.Checked = true;
                tbseattype.Text = "Department(" + (cbldepttype.Items.Count) + ")";
                load_allowance();
            }
        }
        else
        {
            for (int i = 0; i < cbldepttype.Items.Count; i++)
            {
                cbSelect.Checked = false;
                lblstafnam.Visible = false;
                ddlstfnam.Visible = false;
                staff();
                cbldepttype.Items[i].Selected = false;
                tbseattype.Text = "---Select---";
            }
        }
    }
    protected void chkcategory_CheckedChanged(object sender, EventArgs e)
    {
        if (chkcategory.Checked == true)
        {
            for (int i = 0; i < cblcategory.Items.Count; i++)
            {
                cblcategory.Items[i].Selected = true;
                tbblood.Text = "Category(" + (cblcategory.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cblcategory.Items.Count; i++)
            {
                cblcategory.Items[i].Selected = false;
                tbblood.Text = "---Select---";
            }
        }

    }
    protected void cblallowance_CheckedChanged(object sender, EventArgs e)
    {
        if (chkallowance.Checked == true)
        {
            for (int i = 0; i < cblallowance.Items.Count; i++)
            {
                cblallowance.Items[i].Selected = true;
            }
            txtallowance.Text = "Allowance(" + (cblallowance.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblallowance.Items.Count; i++)
            {
                cblallowance.Items[i].Selected = false;
                txtallowance.Text = "---Select---";
            }
        }

    }
    protected void Chkdeduction_CheckedChanged(object sender, EventArgs e)
    {
        txtdeduction.Text = "---Select---";
        if (Chkdeduction.Checked == true)
        {
            //if (cbldeduction.Items.Count > 0)
            //{
            for (int i = 0; i < cbldeduction.Items.Count; i++)
            {
                cbldeduction.Items[i].Selected = true;
            }
            txtdeduction.Text = "Deduction(" + (cbldeduction.Items.Count) + ")";
            //}
            //else
            //{
            //    Chkdeduction.Checked = false;
            //}
        }
        else
        {

            for (int i = 0; i < cbldeduction.Items.Count; i++)
            {
                cbldeduction.Items[i].Selected = false;
            }
        }

    }


    protected void cblallowance_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtallowance.Text = "---Select---";
        chkallowance.Checked = false;
        Pallowance.Focus();
        int allowancecount = 0;
        for (int i = 0; i < cblallowance.Items.Count; i++)
        {
            if (cblallowance.Items[i].Selected == true)
            {
                allowancecount = allowancecount + 1;
            }

        }
        if (allowancecount > 0)
        {
            txtallowance.Text = "Allowance(" + allowancecount.ToString() + ")";
            if (allowancecount == cblallowance.Items.Count)
            {
                chkallowance.Checked = true;
            }
        }
    }


    public ImageButton allowanceimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;

        PlaceHolder1.Controls.Add(imc);
        ViewState["iallowancecontrol"] = true;
        return (imc);
    }



    public Label allowancelable()
    {
        Label lbc = new Label();
        PlaceHolder1.Controls.Add(lbc);
        ViewState["lallowancecontrol"] = true;
        return (lbc);
    }
    public void allowanceimage_Click(object sender, ImageClickEventArgs e)
    {
        allowancecnt = allowancecnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cblallowance.Items[r].Selected = false;

        txtallowance.Text = "Allowance(" + allowancecnt.ToString() + ")";
        if (txtallowance.Text == "Allowance(0)")
        {
            txtallowance.Text = "---Select---";
        }
        int p = PlaceHolder1.Controls.IndexOf(b);
        PlaceHolder1.Controls.RemoveAt(p - 1);
        PlaceHolder1.Controls.Remove(b);


    }


    protected void LinkButtonallownce_Click(object sender, EventArgs e)
    {
        cblallowance.ClearSelection();
        PlaceHolder1.Controls.Clear();
        allowancecnt = 0;

        txtallowance.Text = "---Select---";

    }
    protected void cbldeduction_SelectedIndexChanged(object sender, EventArgs e)
    {
        Pdeduction.Focus();
        txtdeduction.Text = "---Select---";
        Chkdeduction.Checked = false;
        int deductioncount = 0;
        for (int i = 0; i < cbldeduction.Items.Count; i++)
        {
            if (cbldeduction.Items[i].Selected == true)
            {
                deductioncount = deductioncount + 1;
            }
        }

        if (deductioncount > 0)
        {
            txtdeduction.Text = "Deduction(" + deductioncount.ToString() + ")";
            if (deductioncount == cbldeduction.Items.Count)
            {
                Chkdeduction.Checked = true;
            }
        }
    }


    public ImageButton deductionimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;

        PlaceHolderded.Controls.Add(imc);
        ViewState["ideductioncontrol"] = true;
        return (imc);
    }
    public Label deductionlable()
    {
        Label lbc = new Label();
        PlaceHolderded.Controls.Add(lbc);
        ViewState["ldeductioncontrol"] = true;
        return (lbc);
    }
    public void deductionimage_Click(object sender, ImageClickEventArgs e)
    {
        deductioncnt = deductioncnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cbldeduction.Items[r].Selected = false;

        txtdeduction.Text = "Deduction(" + deductioncnt.ToString() + ")";
        if (txtdeduction.Text == "Deduction(0)")
        {
            txtdeduction.Text = "---Select---";
        }
        int p = PlaceHolderded.Controls.IndexOf(b);
        PlaceHolderded.Controls.RemoveAt(p - 1);
        PlaceHolderded.Controls.Remove(b);


    }
    protected void LinkButtondeduction_Click(object sender, EventArgs e)
    {
        cbldeduction.ClearSelection();
        PlaceHolderded.Controls.Clear();
        deductioncnt = 0;
        txtdeduction.Text = "---Select---";
    }
    protected void LinkButton3_Click(object sender, EventArgs e)
    {

    }
    protected void btnsetting_Click(object sender, EventArgs e)
    {
        Response.Redirect("categorysetting.aspx");
    }


    public void clear()
    {
        pnldemond.Visible = false;
        fpsalarydemond.Visible = false;

    }


    protected void btndemond_go_Click(object sender, EventArgs e)
    {
        try
        {
            dicaddtot.Clear();
            dicaddgrandtot.Clear();
            Boolean recflag = false;
            Printcontrol.Visible = false;
            fpsalarydemond.Visible = false;
            btnsal.Visible = false;
            btnprintmaster.Visible = false;
            btnxl.Visible = false;
            lblexcel.Visible = false;
            txtxl.Visible = false;
            txtxl.Text = "";

            if (cbotomonth.SelectedItem.Value == "0")
            {
                lblnorec.Text = "Please Select the Valid Month!";
                lblnorec.Visible = true;
                fpsalarydemond.Visible = false;
                lblexcel.Visible = false;
                txtxl.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                btnsal.Visible = false;
                Printcontrol.Visible = false;
                return;
            }
            //else
            //{
            //    if (cblmonthfrom.SelectedItem.Value != "0" && cbotomonth.SelectedItem.Value != "0")
            //    {
            //        if (Convert.ToInt32(cblmonthfrom.SelectedItem.Value) > Convert.ToInt32(cbotomonth.SelectedItem.Value))
            //        {
            //            lblnorec.Text = "Please Select the Valid To Month!";
            //            lblnorec.Visible = true;
            //            fpsalarydemond.Visible = false;
            //            lblexcel.Visible = false;
            //            txtxl.Visible = false;
            //            btnxl.Visible = false;
            //            btnprintmaster.Visible = false;
            //            btnsal.Visible = false;
            //            Printcontrol.Visible = false;
            //            return;
            //        }
            //    }
            //}

            Session["strallow"] = "";
            Session["strdeduct"] = "";
            Session["strcategory"] = "";
            Session["strdept"] = "";
            fpsalarydemond.Sheets[0].ColumnCount = 4;
            fpsalarydemond.Sheets[0].RowCount = 0;
            fpsalarydemond.Sheets[0].PageSize = 11;
            fpsalarydemond.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
            fpsalarydemond.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
            fpsalarydemond.Pager.Align = HorizontalAlign.Right;
            fpsalarydemond.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
            fpsalarydemond.Pager.Font.Bold = true;
            fpsalarydemond.Pager.Font.Name = "Arial";
            fpsalarydemond.Pager.ForeColor = Color.DarkGreen;
            fpsalarydemond.Pager.BackColor = Color.AliceBlue;
            fpsalarydemond.Pager.PageCount = 5;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Bold = true;
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.Border.BorderColorRight = Color.Black;
            fpsalarydemond.Sheets[0].ColumnHeader.DefaultStyle = darkstyle;
            fpsalarydemond.Sheets[0].SheetCorner.DefaultStyle = darkstyle;
            fpsalarydemond.Sheets[0].SetColumnWidth(0, 100);
            fpsalarydemond.ActiveSheetView.Columns[0].Font.Size = FontUnit.Medium;
            fpsalarydemond.ActiveSheetView.Columns[0].Font.Name = "Book Antiqua";
            fpsalarydemond.ActiveSheetView.Columns[1].Font.Size = FontUnit.Medium;
            fpsalarydemond.ActiveSheetView.Columns[1].Font.Name = "Book Antiqua";
            fpsalarydemond.ActiveSheetView.Columns[0].Font.Size = FontUnit.Medium;
            fpsalarydemond.ActiveSheetView.Columns[0].Font.Name = "Book Antiqua";
            fpsalarydemond.ActiveSheetView.Columns[2].Font.Size = FontUnit.Medium;
            fpsalarydemond.ActiveSheetView.Columns[2].Font.Name = "Book Antiqua";
            fpsalarydemond.ActiveSheetView.Columns[3].Font.Size = FontUnit.Medium;
            fpsalarydemond.ActiveSheetView.Columns[3].Font.Name = "Book Antiqua";


            fpsalarydemond.Sheets[0].AutoPostBack = false;
            //////////////////////////////
            fpsalarydemond.Visible = true;
            btnprintmaster.Visible = true;
            btnxl.Visible = true;
            lblexcel.Visible = true;
            txtxl.Visible = true;

            fpsalarydemond.SheetCorner.RowCount = 2;
            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            fpsalarydemond.Sheets[0].Columns[0].CellType = chkcell;
            fpsalarydemond.Sheets[0].Columns[0].Visible = false;


            fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Select";
            if (cbSelect.Checked == true)
            {
                fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
                fpsalarydemond.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            }
            else
            {
                fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Total No Of Staff";
                fpsalarydemond.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.None);
            }
            fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Basic Pay";
            fpsalarydemond.Sheets[0].Columns[3].Locked = true;

            fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Month";

            fpsalarydemond.Sheets[0].Columns[1].Locked = true;

            fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Basic Pay";
            fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            fpsalarydemond.Sheets[0].FrozenColumnCount = 1;
            int colcount1;
            colcount1 = 4;
            string stafcode = "";
            string fdates = cblmonthfrom.SelectedItem.Value;
            string fyear = cblbatchyear.SelectedItem.Text;
            string tdates = cbotomonth.SelectedItem.Value;

            Boolean deptflag = false;
            for (int itemcount = 0; itemcount < cbldepttype.Items.Count; itemcount++)
            {
                if (cbldepttype.Items[itemcount].Selected == true)
                {
                    deptflag = true;
                }
            }
            if (deptflag == false)
            {
                Printcontrol.Visible = false;
                fpsalarydemond.Visible = false;
                btnsal.Visible = false;
                btnprintmaster.Visible = false;
                btnxl.Visible = false;
                lblexcel.Visible = false;
                txtxl.Visible = false;
                txtxl.Text = "";
                lblnorec.Text = "Please Select The Department and then Proceed";
                lblnorec.Visible = true;
                return;
            }
            deptflag = false;
            for (int itemcount1 = 0; itemcount1 < cblcategory.Items.Count; itemcount1++)
            {
                if (cblcategory.Items[itemcount1].Selected == true)
                {
                    deptflag = true;
                }
            }
            if (deptflag == false)
            {
                Printcontrol.Visible = false;
                fpsalarydemond.Visible = false;
                btnsal.Visible = false;
                btnprintmaster.Visible = false;
                btnxl.Visible = false;
                lblexcel.Visible = false;
                txtxl.Visible = false;
                txtxl.Text = "";
                lblnorec.Text = "Please Select The Category and then Proceed";
                lblnorec.Visible = true;
                return;
            }
            if (cbSelect.Checked == true)
            {
                if (ddlstfnam.Items.Count == 0 || cblstfname.Items.Count == 0)
                {
                    Printcontrol.Visible = false;
                    fpsalarydemond.Visible = false;
                    btnsal.Visible = false;
                    btnprintmaster.Visible = false;
                    btnxl.Visible = false;
                    lblexcel.Visible = false;
                    txtxl.Visible = false;
                    txtxl.Text = "";
                    lblnorec.Text = "Please Select The Staff and then Proceed";
                    lblnorec.Visible = true;
                    return;
                }
                else
                {
                    if (ddlstfnam.SelectedItem.ToString().Trim() == "" || txtstfname.Text.Trim() == "---Select---")
                    {
                        Printcontrol.Visible = false;
                        fpsalarydemond.Visible = false;
                        btnsal.Visible = false;
                        btnprintmaster.Visible = false;
                        btnxl.Visible = false;
                        lblexcel.Visible = false;
                        txtxl.Visible = false;
                        txtxl.Text = "";
                        lblnorec.Text = "Please Select The Staff and then Proceed";
                        lblnorec.Visible = true;
                        return;
                    }
                }
            }

            deptflag = false;
            for (int itemcount1 = 0; itemcount1 < cblallowance.Items.Count; itemcount1++)
            {
                if (cblallowance.Items[itemcount1].Selected == true)
                {
                    deptflag = true;
                }
            }
            for (int itemcount1 = 0; itemcount1 < cbldeduction.Items.Count; itemcount1++)
            {
                if (cbldeduction.Items[itemcount1].Selected == true)
                {
                    deptflag = true;
                }
            }
            if (deptflag == false)
            {
                Printcontrol.Visible = false;
                fpsalarydemond.Visible = false;
                btnsal.Visible = false;
                btnprintmaster.Visible = false;
                btnxl.Visible = false;
                lblexcel.Visible = false;
                txtxl.Visible = false;
                txtxl.Text = "";
                lblnorec.Text = "Allowance and Deduction are not Defined.";
                lblnorec.Visible = true;
                return;
            }


            if (cbSelect.Checked == true)
            {
                stafcode = ddlstfnam.SelectedItem.Value;
                btnsal.Visible = true;
                //sql1 = "select  * from monthlypay where college_code=" + Session["collegecode"] + " and staff_code='" + stafcode + "' ";
                sql1 = "select * from incentives_master where college_code=" + Session["collegecode"] + "";
                string stafname = ddlstfnam.SelectedItem.Text;
                for (int i = 0; i < fpsalarydemond.Sheets[0].RowCount; i++)
                {
                    if (fpsalarydemond.Sheets[0].RowCount == 1)
                    {
                        fpsalarydemond.Sheets[0].Cells[i, 2].Text = stafname;
                    }
                    else
                    {
                        fpsalarydemond.Sheets[0].Cells[i, 2].Text = stafname;
                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 2].Text = " ";
                    }
                }
                fpsalarydemond.Sheets[0].Columns[2].Width = 150;
            }
            else
            {
                btnsal.Visible = false;
                sql1 = "select * from incentives_master where college_code=" + Session["collegecode"] + "";
                fpsalarydemond.Sheets[0].Columns[2].Width = 100;
            }
            SqlCommand cmd2 = new SqlCommand(sql1, con1);
            DataSet dsquery = d2.select_method_wo_parameter(sql1, "Text");

            if (dsquery.Tables[0].Rows.Count > 0)
            {
                string allowncweshead;

                string detuctionheader;

                allowncweshead = dsquery.Tables[0].Rows[0]["allowances"].ToString();
                string[] allown2;

                if (cbSelect.Checked == true)
                {
                    allown2 = allowncweshead.Split('\\');
                }
                else
                {
                    allown2 = allowncweshead.Split(';');
                }

                getval = allown2.GetUpperBound(0);
                ArrayList allpayslip = new ArrayList();
                //if (cbSelect.Checked == true)
                //{
                getval = 0;
                for (int r = 0; r < cblallowance.Items.Count; r++)
                {
                    if (cblallowance.Items[r].Selected == true)
                    {
                        allpayslip.Add(cblallowance.Items[r].ToString());
                        getval++;
                    }
                }
                //}
                fpsalarydemond.Sheets[0].ColumnCount = fpsalarydemond.Sheets[0].ColumnCount + getval;
                colcount1 = 4;
                int count = 0;
                count = getval;//allown2.GetUpperBound(0);
                strallallowance = "";
                int spcount = 0;
                int hcount = 3;
                fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, colcount1].Text = "Earnings";
                int checkcount = 0, startcount = 0;
                int setdcol = 0;
                for (int i = 0; i < cblallowance.Items.Count; i++)
                {
                    startcount++;
                    hcount++;
                    if (allpayslip.Contains(cblallowance.Items[i].ToString()))
                    {
                        if (cblallowance.Items[i].Selected == true)
                        {
                            if (checkcount == 0)
                            {
                                checkcount = startcount + 3;
                            }
                            spcount++;
                            if (strallallowance == "")
                            {
                                strallallowance = cblallowance.Items[i].Value.ToString();
                            }
                            else
                            {
                                strallallowance = strallallowance + "," + cblallowance.Items[i].Value.ToString();

                            }

                            fpsalarydemond.Sheets[0].Columns[colcount1 + setdcol].Visible = true;

                            string allo2 = "";
                            if (i <= allown2.GetUpperBound(0))
                            {
                                allo2 = allown2[i];
                                string[] splitallo3;
                                if (cbSelect.Checked == true)
                                {

                                    splitallo3 = allo2.Split(';');
                                }
                                else
                                {
                                    splitallo3 = allo2.Split('\\');
                                }
                                allo2 = splitallo3[0];
                                if (cbSelect.Checked == true)
                                {
                                    allo2 = allpayslip[setdcol].ToString();
                                }

                                //fpsalarydemond.Sheets[0].Columns[colcount1 + setdcol].Locked = true;
                                //fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, colcount1 + setdcol].Text = Convert.ToString(cblallowance.Items[i].Text);
                                //fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, colcount1 + setdcol].HorizontalAlign = HorizontalAlign.Center;
                                //fpsalarydemond.ActiveSheetView.Columns[colcount1 + setdcol].Font.Size = FontUnit.Medium;
                                //fpsalarydemond.ActiveSheetView.Columns[colcount1 + setdcol].Font.Name = "Book Antiqua";
                            }
                            fpsalarydemond.Sheets[0].Columns[colcount1 + setdcol].Locked = true;
                            fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, colcount1 + setdcol].Text = Convert.ToString(cblallowance.Items[i].Text);
                            fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, colcount1 + setdcol].HorizontalAlign = HorizontalAlign.Center;
                            fpsalarydemond.ActiveSheetView.Columns[colcount1 + setdcol].Font.Size = FontUnit.Medium;
                            fpsalarydemond.ActiveSheetView.Columns[colcount1 + setdcol].Font.Name = "Book Antiqua";
                        }
                        else
                        {

                            fpsalarydemond.Sheets[0].Columns[colcount1 + setdcol].Visible = false;
                        }
                        setdcol++;
                    }
                }
                if (setdcol > 0)
                {
                    fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount1, 1, setdcol);
                }
                Session["strallow"] = strallallowance;
                fpsalarydemond.Sheets[0].ColumnCount = fpsalarydemond.Sheets[0].ColumnCount + 1;
                colheder = fpsalarydemond.Sheets[0].ColumnCount;

                fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, colheder - 1].Text = "Earned Salary";

                fpsalarydemond.ActiveSheetView.Columns[colheder - 1].Font.Size = FontUnit.Medium;
                fpsalarydemond.ActiveSheetView.Columns[colheder - 1].Font.Name = "Book Antiqua";

                fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, colheder - 1].HorizontalAlign = HorizontalAlign.Right;

                fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(0, fpsalarydemond.Sheets[0].ColumnCount - 1, 2, 1);
                detuctionheader = dsquery.Tables[0].Rows[0]["deductions"].ToString();
                string[] deduct2;


                if (cbSelect.Checked == true)
                {
                    deduct2 = detuctionheader.Split('\\');
                }
                else
                {
                    deduct2 = detuctionheader.Split(';');
                }
                getval2 = deduct2.GetUpperBound(0);

                col = fpsalarydemond.Sheets[0].ColumnCount;
                col2 = col;
                // col = col + 1;
                ArrayList alldedupayslip = new ArrayList();
                // if (cbSelect.Checked == true)
                {
                    getval2 = 0;
                    for (int r = 0; r < cbldeduction.Items.Count; r++)
                    {
                        if (cbldeduction.Items[r].Selected == true)
                        {
                            alldedupayslip.Add(cbldeduction.Items[r].ToString());
                            getval2++;
                        }
                    }
                }
                fpsalarydemond.Sheets[0].ColumnCount = fpsalarydemond.Sheets[0].ColumnCount + getval2;
                colcount1 = fpsalarydemond.Sheets[0].ColumnCount + 1;
                if (getval2 > 0)
                {
                    fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, col].Text = "Deductions";
                    fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, getval2);
                    stralldeduct = "";
                    setdcol = 0;
                    for (int i = 0; i < cbldeduction.Items.Count; i++)
                    {
                        if (alldedupayslip.Contains(cbldeduction.Items[i].ToString()))
                        {
                            if (cbldeduction.Items[i].Selected == true)
                            {
                                if (stralldeduct == "")
                                {
                                    stralldeduct = cbldeduction.Items[i].Value.ToString();
                                }
                                else
                                {
                                    stralldeduct = stralldeduct + "," + cbldeduction.Items[i].Value.ToString();
                                }
                                if (cbSelect.Checked == true)
                                {
                                    deduct = alldedupayslip[setdcol].ToString();
                                }
                                else
                                {
                                    deduct = cbldeduction.Items[i].Text.ToString();
                                }
                                fpsalarydemond.Sheets[0].Columns[col + setdcol].Visible = true;

                                fpsalarydemond.Sheets[0].Columns[col + setdcol].Locked = true;

                                fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, col + setdcol].Text = Convert.ToString(cbldeduction.Items[i].Text);
                                fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, col + setdcol].HorizontalAlign = HorizontalAlign.Center;

                                fpsalarydemond.ActiveSheetView.Columns[col + setdcol].Font.Size = FontUnit.Medium;
                                fpsalarydemond.ActiveSheetView.Columns[col + setdcol].Font.Name = "Book Antiqua";
                            }
                            else
                            {
                                fpsalarydemond.Sheets[0].Columns[col + setdcol].Visible = false;
                            }
                            setdcol++;
                        }
                    }
                }
                Session["strdeduct"] = stralldeduct;
                fpsalarydemond.Sheets[0].ColumnCount = fpsalarydemond.Sheets[0].ColumnCount + 2;
                colcount1 = fpsalarydemond.Sheets[0].ColumnCount;
                fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, colcount1 - 2].Text = "Total Deduction";
                fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, colcount1 - 2].HorizontalAlign = HorizontalAlign.Center;
                fpsalarydemond.ActiveSheetView.Columns[colcount1 - 2].Font.Size = FontUnit.Medium;
                fpsalarydemond.ActiveSheetView.Columns[colcount1 - 2].Font.Name = "Book Antiqua";
                fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount1 - 2, 2, 1);
                fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, colcount1 - 1].Text = "Net Pay";
                fpsalarydemond.Sheets[0].ColumnHeader.Columns[colcount1 - 1].Locked = true;
                fpsalarydemond.Sheets[0].ColumnHeader.Columns[colcount1 - 2].Locked = true;
                fpsalarydemond.ActiveSheetView.Columns[colcount1 - 1].Font.Size = FontUnit.Medium;
                fpsalarydemond.ActiveSheetView.Columns[colcount1 - 1].Font.Name = "Book Antiqua";
                fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount1 - 1, 2, 1);
            }
            int monthfrom;
            int monthto;
            monthfrom = Convert.ToInt16(cblmonthfrom.SelectedValue.ToString());
            monthto = Convert.ToInt16(cbotomonth.SelectedValue.ToString());


            int firstday = 1;
            int years = Convert.ToInt16(cblbatchyear.SelectedItem.Text);
            int toyear = Convert.ToInt16(ddl_toyear.SelectedItem.Text);
            DateTime fdate = Convert.ToDateTime(monthfrom + "/" + firstday + "/" + years);
            DateTime tdate = Convert.ToDateTime(monthto + "/" + firstday + "/" + toyear);

            if (cbSelect.Checked == true && txtstfname.Text.Trim() != "---Select---")
            {
                for (int st = 0; st < cblstfname.Items.Count; st++)
                {
                    if (cblstfname.Items[st].Selected == true)
                    {
                        dicaddtot.Clear();
                        stafcode = Convert.ToString(cblstfname.Items[st].Value);
                        if (cblmonthfrom.SelectedItem.ToString() != "All")
                        {
                            while (fdate < tdate)
                            {
                                //for (int b = monthfrom; b <= monthto; b++)//delsi
                                //{ 
                                //========Start============Added by Manikandan 20/08/2013=========================
                                string monthsdate = string.Empty;
                                string monthedate = string.Empty;
                                string date_1 = string.Empty;
                                string date_2 = string.Empty;
                                string year = cblbatchyear.SelectedItem.Text;
                                string sqlquery = "select CONVERT(VARCHAR(10),from_date,103) as from_date,convert(VARCHAR(10),to_date,103) as to_date from hrpaymonths where paymonthnum='" + Convert.ToString(fdate.ToString("MM").TrimStart('0')) + "' and PayYear='" + Convert.ToString(fdate.ToString("yyyy")) + "'";
                                con.Close();
                                con.Open();
                                SqlDataAdapter da_hrdate = new SqlDataAdapter(sqlquery, con);
                                DataTable dt_hrdate = new DataTable();
                                da_hrdate.Fill(dt_hrdate);
                                if (dt_hrdate.Rows.Count > 0)
                                {
                                    monthsdate = dt_hrdate.Rows[0]["from_date"].ToString();
                                    monthedate = dt_hrdate.Rows[0]["to_date"].ToString();

                                    string[] split_date = monthsdate.Split(new char[] { '/' });
                                    date_1 = split_date[1] + "/" + split_date[0] + "/" + split_date[2];
                                    string[] split_date_2 = monthedate.Split(new char[] { '/' });
                                    string tday = split_date_2[0];
                                    date_2 = split_date_2[1] + "/" + split_date_2[0] + "/" + split_date_2[2];
                                    Array.Clear(DblAllowTotal, 0, DblAllowTotal.Length);
                                    Array.Clear(deductiontotal, 0, deductiontotal.Length);
                                    sql = "";
                                    //sql = " SELECT monthlypay.*,desig_master.priority,dept_acronym,staff_name,bankaccount,pfnumber,monthlypay.pay_band,monthlypay.grade_pay from monthlypay,desig_master,stafftrans,staffmaster,hrdept_master Where hrdept_master.dept_code=stafftrans.dept_code and stafftrans.staff_code=monthlypay.staff_code and stafftrans.latestrec=1 and desig_master.desig_code=stafftrans.desig_code and month(fdate) ='" + b + "' and  month(tdate) ='" + b + "' and monthlypay.college_code=" + Session["collegecode"] + " and desig_master.collegecode=" + Session["collegecode"] + " and staffmaster.staff_code=monthlypay.staff_code  ";
                                    if (cbSelect.Checked == true)
                                    {
                                        sql = " SELECT monthlypay.*,desig_master.priority,dept_acronym,staff_name,bankaccount,pfnumber,monthlypay.pay_band,monthlypay.grade_pay from monthlypay,desig_master,stafftrans,staffmaster,hrdept_master Where hrdept_master.dept_code=stafftrans.dept_code and stafftrans.staff_code=monthlypay.staff_code and stafftrans.latestrec=1 and desig_master.desig_code=stafftrans.desig_code and (PayMonth = '" + Convert.ToString(fdate.ToString("MM").TrimStart('0')) + "' and PayYear = '" + fdate.ToString("yyyy") + "') and monthlypay.college_code=" + Session["collegecode"] + " and desig_master.collegecode=" + Session["collegecode"] + " and staffmaster.staff_code=monthlypay.staff_code and staffmaster.staff_code = '" + stafcode + "'";//This Query Modified by Manikandan 20/08/2013
                                    }
                                    else
                                    {
                                        sql = " SELECT monthlypay.*,desig_master.priority,dept_acronym,staff_name,bankaccount,pfnumber,monthlypay.pay_band,monthlypay.grade_pay from monthlypay,desig_master,stafftrans,staffmaster,hrdept_master Where hrdept_master.dept_code=stafftrans.dept_code and stafftrans.staff_code=monthlypay.staff_code and stafftrans.latestrec=1 and desig_master.desig_code=stafftrans.desig_code and (PayMonth = '" + Convert.ToString(fdate.ToString("MM").TrimStart('0')) + "' and PayYear = '" + fdate.ToString("yyyy") + "') and monthlypay.college_code=" + Session["collegecode"] + " and desig_master.collegecode=" + Session["collegecode"] + " and staffmaster.staff_code=monthlypay.staff_code";
                                    }
                                    if (tbseattype.Text != "---Select---") //department
                                    {
                                        int itemcount = 0;
                                        strdept = "";
                                        gssmcat = "";

                                        for (itemcount = 0; itemcount < cbldepttype.Items.Count; itemcount++)
                                        {
                                            if (cbldepttype.Items[itemcount].Selected == true)
                                            {
                                                if (strdept == "")
                                                {
                                                    strdept = "'" + cbldepttype.Items[itemcount].Value.ToString() + "'";

                                                    gssmdept = cbldepttype.Items[itemcount].Value.ToString();
                                                }
                                                else
                                                {
                                                    strdept = strdept + "," + "'" + cbldepttype.Items[itemcount].Value.ToString() + "'";
                                                    gssmdept = gssmdept + "," + cbldepttype.Items[itemcount].Value.ToString();
                                                }
                                            }
                                        }
                                        gstrdept = gssmdept;

                                        if (strdept != "")
                                        {
                                            strdept = " in(" + strdept + ")";
                                        }
                                        sql = sql + " and hrdept_master.dept_code " + strdept + "";

                                    }
                                    else
                                    {
                                        gstrdept = "all";
                                    }
                                    Session["strdept"] = gstrdept;
                                    if (tbblood.Text != "---Select---")
                                    {


                                        int itemcount1 = 0;
                                        strcategory = "";
                                        gssmcat = "";
                                        for (itemcount1 = 0; itemcount1 < cblcategory.Items.Count; itemcount1++)
                                        {
                                            if (cblcategory.Items[itemcount1].Selected == true)
                                            {
                                                if (strcategory == "")
                                                {
                                                    strcategory = "'" + cblcategory.Items[itemcount1].Value.ToString() + "'";
                                                    gssmcat = cblcategory.Items[itemcount1].Value.ToString();
                                                    //strcategory = cblcategory.Items[itemcount1].Value.ToString();
                                                }
                                                else
                                                {
                                                    strcategory = strcategory + "," + "'" + cblcategory.Items[itemcount1].Value.ToString() + "'";
                                                    gssmcat = gssmcat + "," + cblcategory.Items[itemcount1].Value.ToString();
                                                    //strcategory = strcategory+","+cblcategory.Items[itemcount1].Value.ToString();
                                                }
                                            }
                                        }

                                        gstrcateogry = gssmcat;
                                        if (strcategory != "")
                                        {
                                            strcategory = " in (" + strcategory + ")";
                                        }
                                        sql = sql + "  and stafftrans.category_code" + strcategory + "";

                                    }
                                    else
                                    {
                                        gstrcateogry = "all";
                                    }
                                    Session["strcategory"] = gstrcateogry;
                                    con.Close();
                                    con.Open();

                                    //Session["allowanddedu"] = strcategory + "@" + strdept + "@" + strallallowance + "@" + stralldeduct;
                                    // string ddd = Session["allowanddedu"].ToString();


                                    int m = 0;

                                    SqlCommand cmd3 = new SqlCommand(sql, con);
                                    SqlDataReader dr30;
                                    int countstaff = 0;

                                    string netadd = "";
                                    double earntotal = 0;
                                    string netded = "";
                                    double totaldeduction = 0;
                                    string netpa = "";
                                    double totalnetpay = 0;
                                    dr30 = cmd3.ExecuteReader();
                                    while (dr30.Read())
                                    {
                                        if (dr30.HasRows == true)
                                        {
                                            recflag = true;
                                            string allowance = "";
                                            string deduction = "";
                                            string basicpay = "";

                                            countstaff = countstaff + 1;

                                            int k = 0;
                                            int p = 4;
                                            int col3 = 0;
                                            col2 = 0;
                                            col3 = col;
                                            col2 = col;
                                            // DblAllowTotal = 0;
                                            basicpay = dr30["bsalary"].ToString();

                                            netadd = dr30["netadd"].ToString();
                                            netded = dr30["netded"].ToString();
                                            netpa = dr30["netsal"].ToString();

                                            Double netpanetpa = Convert.ToDouble(netpa);
                                            netpanetpa = Math.Round(netpanetpa, 0, MidpointRounding.AwayFromZero);
                                            netpa = netpanetpa.ToString();
                                            totalnetpay = Convert.ToDouble(netpa) + totalnetpay;


                                            Double netdepa = Convert.ToDouble(netded);
                                            netdepa = Math.Round(netdepa, 0, MidpointRounding.AwayFromZero);
                                            netded = netdepa.ToString();
                                            totaldeduction = Convert.ToDouble(netded) + totaldeduction;

                                            Double erah = Convert.ToDouble(netadd);
                                            erah = Math.Round(erah, 0, MidpointRounding.AwayFromZero);
                                            netadd = erah.ToString();
                                            earntotal = Convert.ToDouble(netadd) + earntotal;

                                            Double basicl = Convert.ToDouble(basicpay);
                                            basicl = Math.Round(basicl, 0, MidpointRounding.AwayFromZero);
                                            basicpay = basicl.ToString();

                                            basicpaytotal = Convert.ToDouble(basicpay) + basicpaytotal;

                                            allowance = dr30["allowances"].ToString();
                                            deduction = dr30["Deductions"].ToString();

                                            string[] allowance2;
                                            int g = 0;
                                            string alowancesplit;
                                            allowanmce_arr1 = allowance.Split('\\');
                                            if (allowanmce_arr1.GetUpperBound(0) > 0)
                                            {
                                                for (m = 0; m < allowanmce_arr1.GetUpperBound(0); m++)
                                                {

                                                l2: alowancesplit = allowanmce_arr1[m];
                                                    k = 0;
                                                    p = 4;
                                                    if (alowancesplit != "")
                                                    {
                                                        allowance2 = alowancesplit.Split(';');
                                                        string[] splval = allowance2[2].Split('-');
                                                        if (allowance2[1].Trim() == "Amount")  //modified by jeyaprakash on Sep 6th(Allowance not invokes)
                                                        {
                                                            da3 = splval[0];
                                                        }
                                                        else if (allowance2[1].Trim() == "Percent" || allowance2[1].Trim() == "Slab")
                                                        {
                                                            if (splval.Length == 2)
                                                            {
                                                                da3 = splval[1];
                                                            }
                                                        }
                                                        //da3 = allowance2[3];

                                                        if (allowance2.GetUpperBound(0) > 0)
                                                        {
                                                        l3: if (p < fpsalarydemond.Sheets[0].ColumnCount - 1) // changed
                                                            {
                                                                string headval = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, p].Text;
                                                                for (int j = 0; j < allowance2.GetUpperBound(0); j++)
                                                                {
                                                                    Double alle = 0;
                                                                    if (headval == allowance2[j])
                                                                    {
                                                                        Double.TryParse(da3, out alle);
                                                                        alle = Math.Round(alle, 0, MidpointRounding.AwayFromZero);
                                                                        DblAllowTotal[k] = Convert.ToDouble(DblAllowTotal[k]) + Convert.ToDouble(alle);
                                                                        DblNetAllowTotal = alle + Convert.ToDouble(DblNetAllowTotal);

                                                                        m = m + 1;
                                                                        p = p + 1;
                                                                        k = k + 1;
                                                                        goto l2;

                                                                    }
                                                                    else
                                                                    {

                                                                        p = p + 1;
                                                                        k = k + 1;
                                                                        goto l3;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            string[] deduction_arr1;

                                            string[] deduction2;

                                            k = 0;
                                            string deductionsplit;

                                            deduction_arr1 = deduction.Split('\\');

                                            if (deduction_arr1.GetUpperBound(0) > 0)
                                            {
                                                for (m = 0; m < deduction_arr1.GetUpperBound(0); m++)
                                                {
                                                l2: deductionsplit = deduction_arr1[m];
                                                    col3 = col;
                                                    k = 0;
                                                    if (deductionsplit != "")
                                                    {
                                                        deduction2 = deductionsplit.Split(';');
                                                        string[] splval = deduction2[2].Split('-');
                                                        if (deduction2[1].Trim() == "Amount")  //modified by jeyaprakash on Sep 6th(Allowance not invokes)
                                                        {
                                                            da3 = splval[0];
                                                        }
                                                        else if (deduction2[1].Trim() == "Percent" || deduction2[1].Trim() == "Slab")
                                                        {
                                                            if (splval.Length == 2)
                                                            {
                                                                da3 = splval[1];
                                                            }
                                                        }
                                                        //da3 = deduction2[3];
                                                        if (deduction2.GetUpperBound(0) > 0)
                                                        {
                                                        l3: if (col3 < fpsalarydemond.Sheets[0].ColumnCount - 1) //changed fpsalary to fpsalarydemond
                                                            {
                                                                string headval1 = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, col3].Text;
                                                                for (int j = 0; j < deduction2.GetUpperBound(0); j++)
                                                                {
                                                                    Double allov = 0;
                                                                    if (headval1 == deduction2[j])
                                                                    {
                                                                        Double.TryParse(da3, out allov);
                                                                        allov = Math.Round(allov, 0, MidpointRounding.AwayFromZero);
                                                                        deductiontotal[k] = deductiontotal[k] + allov;
                                                                        DblNetDedTotal = Convert.ToDouble(DblNetDedTotal) + allov;

                                                                        m = m + 1;
                                                                        col3 = col3 + 1;
                                                                        k = k + 1;
                                                                        goto l2;

                                                                    }
                                                                    else
                                                                    {

                                                                        col3 = col3 + 1;
                                                                        k = k + 1;
                                                                        goto l3;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                            }

                                                        }
                                                    }
                                                }
                                            }

                                        }
                                        else
                                        {
                                            fpsalarydemond.Visible = false;
                                            btnprintmaster.Visible = false;
                                            btnxl.Visible = false;
                                            lblexcel.Visible = false;
                                            txtxl.Visible = false;
                                            lblnorec.Visible = true;
                                            lblnorec.Text = "No Records Found";
                                        }


                                    }



                                    int rowstr1 = fpsalarydemond.Sheets[0].RowCount++;
                                    mname = Convert.ToString(fdate.ToString("MM").TrimStart('0'));// b.ToString(); delsi 
                                    string month7 = getmonth(mname);
                                    /////////////////////////////////////////////////////////////////////////////////////////
                                    fpsalarydemond.Sheets[0].Cells[rowstr1, 0].Value = 1;
                                    fpsalarydemond.Sheets[0].Cells[rowstr1, 1].Tag = Convert.ToString(fdate.ToString("MM").TrimStart('0'));// b.ToString();

                                    fpsalarydemond.Sheets[0].Cells[rowstr1, 1].Text = month7.ToString();
                                    fpsalarydemond.Sheets[0].Cells[rowstr1, 3].Text = basicpaytotal.ToString();
                                    Double myAmnt = 0;
                                    if (!dicaddtot.ContainsKey(3))
                                        dicaddtot.Add(3, basicpaytotal);
                                    else
                                    {
                                        Double.TryParse(Convert.ToString(dicaddtot[3]), out myAmnt);
                                        myAmnt = myAmnt + basicpaytotal;
                                        dicaddtot.Remove(3);
                                        dicaddtot.Add(3, myAmnt);
                                    }
                                    string stafname = Convert.ToString(cblstfname.Items[st].Text);
                                    fpsalarydemond.Sheets[0].Cells[rowstr1, 2].Text = stafname;
                                    fpsalarydemond.Sheets[0].Cells[rowstr1, 2].Locked = true;
                                    fpsalarydemond.Sheets[0].Cells[rowstr1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    fpsalarydemond.Sheets[0].Cells[rowstr1, 3].HorizontalAlign = HorizontalAlign.Right;
                                    int g1 = 4;
                                    for (int i = 0; i < getval; i++)
                                    {
                                        DblAllowTotal[i] = Math.Round(DblAllowTotal[i], 2);
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, g1].Text = DblAllowTotal[i].ToString();
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, g1].HorizontalAlign = HorizontalAlign.Right;
                                        if (!dicaddtot.ContainsKey(g1))
                                            dicaddtot.Add(g1, DblAllowTotal[i]);
                                        else
                                        {
                                            Double.TryParse(Convert.ToString(dicaddtot[g1]), out myAmnt);
                                            myAmnt = myAmnt + DblAllowTotal[i];
                                            dicaddtot.Remove(g1);
                                            dicaddtot.Add(g1, myAmnt);
                                        }
                                        g1 = g1 + 1;
                                    }
                                    fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].HorizontalAlign = HorizontalAlign.Right;
                                    for (int y = 0; y < getval2; y++)
                                    {
                                        deductiontotal[y] = Math.Round(deductiontotal[y], 2);
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, col2].Text = deductiontotal[y].ToString();
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, col2].HorizontalAlign = HorizontalAlign.Right;
                                        if (!dicaddtot.ContainsKey(col2))
                                            dicaddtot.Add(col2, deductiontotal[y]);
                                        else
                                        {
                                            Double.TryParse(Convert.ToString(dicaddtot[col2]), out myAmnt);
                                            myAmnt = myAmnt + deductiontotal[y];
                                            dicaddtot.Remove(col2);
                                            dicaddtot.Add(col2, myAmnt);
                                        }
                                        col2 = col2 + 1;
                                    }
                                    col2 = 0;
                                    col2 = col;
                                    DblNetDedTotal = Math.Round(DblNetDedTotal, 2);
                                    string tot = Convert.ToString(earntotal);
                                    fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].Text = earntotal.ToString();
                                    if (!dicaddtot.ContainsKey(colheder - 1))
                                        dicaddtot.Add(colheder - 1, earntotal);
                                    else
                                    {
                                        Double.TryParse(Convert.ToString(dicaddtot[colheder - 1]), out myAmnt);
                                        myAmnt = myAmnt + earntotal;
                                        dicaddtot.Remove(colheder - 1);
                                        dicaddtot.Add(colheder - 1, myAmnt);
                                    }
                                    fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].Locked = true;
                                    fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].Text = totaldeduction.ToString();
                                    if (!dicaddtot.ContainsKey(colcount1 - 2))
                                        dicaddtot.Add(colcount1 - 2, totaldeduction);
                                    else
                                    {
                                        Double.TryParse(Convert.ToString(dicaddtot[colcount1 - 2]), out myAmnt);
                                        myAmnt = myAmnt + totaldeduction;
                                        dicaddtot.Remove(colcount1 - 2);
                                        dicaddtot.Add(colcount1 - 2, myAmnt);
                                    }
                                    fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].Locked = true;
                                    fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].HorizontalAlign = HorizontalAlign.Right;
                                    fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].Text = totalnetpay.ToString();
                                    if (!dicaddtot.ContainsKey(colcount1 - 1))
                                        dicaddtot.Add(colcount1 - 1, totalnetpay);
                                    else
                                    {
                                        Double.TryParse(Convert.ToString(dicaddtot[colcount1 - 1]), out myAmnt);
                                        myAmnt = myAmnt + totalnetpay;
                                        dicaddtot.Remove(colcount1 - 1);
                                        dicaddtot.Add(colcount1 - 1, myAmnt);
                                    }
                                    fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].Locked = true;
                                    fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].HorizontalAlign = HorizontalAlign.Right;
                                    basicpaytotal = 0;
                                    DblNetDedTotal = 0;
                                    DblNetAllowTotal = 0;
                                    netpaytotal = 0;
                                }
                                fdate = fdate.AddMonths(1);
                            }
                            fdate = Convert.ToDateTime(monthfrom + "/" + firstday + "/" + years);

                            //if (fpsalarydemond.Sheets[0].RowCount - 1 > 0)
                            //{
                            fpsalarydemond.Sheets[0].RowCount++;

                            fpsalarydemond.Sheets[0].Rows[fpsalarydemond.Sheets[0].RowCount - 1].Font.Bold = true;
                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 0].Locked = true;
                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Text = "Total";
                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                            for (int intColCtr = 2; intColCtr < fpsalarydemond.Sheets[0].ColumnCount; intColCtr++)
                            {
                                //IntMTotal = 0;
                                //for (int IntRowCtr = 0; IntRowCtr < fpsalarydemond.Sheets[0].RowCount - 1; IntRowCtr++)
                                //{
                                //    IntMTemp = 0;
                                //    if (fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text != "-")
                                //    {
                                //        if (fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text != "")
                                //        {
                                //            string testval = fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text;
                                //            Double.TryParse(Convert.ToString(fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text), out IntMTemp);
                                //        }
                                //        else
                                //        {
                                //            IntMTemp = 0;
                                //        }
                                //    }
                                //    else
                                //    {
                                //        IntMTemp = 0;
                                //    }
                                //    IntMTotal = IntMTemp + IntMTotal;
                                //    IntMTotal = Math.Round(IntMTotal, 2);
                                //}
                                Double getNewTot = 0;
                                if (dicaddtot.ContainsKey(intColCtr))
                                {
                                    if (Convert.ToString(dicaddtot[intColCtr]) != "0")
                                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Text = Convert.ToString(dicaddtot[intColCtr]);
                                    else
                                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Text = "-";
                                    if (!dicaddgrandtot.ContainsKey(intColCtr))
                                        dicaddgrandtot.Add(intColCtr, dicaddtot[intColCtr]);
                                    else
                                    {
                                        Double.TryParse(Convert.ToString(dicaddgrandtot[intColCtr]), out getNewTot);
                                        getNewTot = getNewTot + dicaddtot[intColCtr];
                                        dicaddgrandtot.Remove(intColCtr);
                                        dicaddgrandtot.Add(intColCtr, getNewTot);
                                    }
                                }
                                else
                                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Text = "-";
                                //fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Text = IntMTotal > 0 ? IntMTotal + "" : "-";
                                fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Font.Name = "Book Antiqua";
                                fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Font.Size = FontUnit.Medium;
                                fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Font.Bold = true;
                                fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].HorizontalAlign = HorizontalAlign.Right;
                                if (intColCtr == 2)
                                {
                                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                            //}
                        }
                        else if (cblmonthfrom.SelectedItem.ToString() == "All")
                        {
                            //int month5 = 1;

                            //for (month5 = 1; month5 <= 12; month5++)
                            while (fdate < tdate)
                            {
                                Array.Clear(DblAllowTotal, 0, DblAllowTotal.Length);
                                Array.Clear(deductiontotal, 0, deductiontotal.Length);
                                sql = "";
                                if (cbSelect.Checked == true)
                                {
                                    //sql = " SELECT monthlypay.*,desig_master.priority,dept_acronym,staff_name,bankaccount,pfnumber,monthlypay.pay_band,monthlypay.grade_pay from monthlypay,desig_master,stafftrans,staffmaster,hrdept_master Where hrdept_master.dept_code=stafftrans.dept_code and stafftrans.staff_code=monthlypay.staff_code and stafftrans.latestrec=1 and desig_master.desig_code=stafftrans.desig_code and month(fdate) ='" + month5 + "' and month( tdate) ='" + month5 + "' and monthlypay.college_code=" + Session["collegecode"] + " and desig_master.collegecode=" + Session["collegecode"] + " and staffmaster.staff_code=monthlypay.staff_code and staffmaster.staff_code = '" + stafcode + "'";
                                    sql = " SELECT monthlypay.*,desig_master.priority,dept_acronym,staff_name,bankaccount,pfnumber,monthlypay.pay_band,monthlypay.grade_pay from monthlypay,desig_master,stafftrans,staffmaster,hrdept_master Where hrdept_master.dept_code=stafftrans.dept_code and stafftrans.staff_code=monthlypay.staff_code and stafftrans.latestrec=1 and desig_master.desig_code=stafftrans.desig_code and monthlypay.college_code=" + Session["collegecode"] + " and desig_master.collegecode=" + Session["collegecode"] + " and staffmaster.staff_code=monthlypay.staff_code and staffmaster.staff_code = '" + stafcode + "'";
                                }
                                else if (cblmonthfrom.SelectedItem.Text == "All")
                                {
                                    //sql = " SELECT monthlypay.*,desig_master.priority,dept_acronym,staff_name,bankaccount,pfnumber,monthlypay.pay_band,monthlypay.grade_pay from monthlypay,desig_master,stafftrans,staffmaster,hrdept_master Where hrdept_master.dept_code=stafftrans.dept_code and stafftrans.staff_code=monthlypay.staff_code and stafftrans.latestrec=1 and desig_master.desig_code=stafftrans.desig_code and monthlypay.college_code=" + Session["collegecode"] + " and desig_master.collegecode=" + Session["collegecode"] + " and staffmaster.staff_code=monthlypay.staff_code and staffmaster.staff_code = '" + stafcode + "'    ";
                                    sql = " SELECT monthlypay.*,desig_master.priority,dept_acronym,staff_name,bankaccount,pfnumber,monthlypay.pay_band,monthlypay.grade_pay from monthlypay,desig_master,stafftrans,staffmaster,hrdept_master Where hrdept_master.dept_code=stafftrans.dept_code and stafftrans.staff_code=monthlypay.staff_code and stafftrans.latestrec=1 and desig_master.desig_code=stafftrans.desig_code and monthlypay.college_code=" + Session["collegecode"] + " and desig_master.collegecode=" + Session["collegecode"] + " and staffmaster.staff_code=monthlypay.staff_code ";
                                }
                                else
                                {
                                    // sql = " SELECT monthlypay.*,desig_master.priority,dept_acronym,staff_name,bankaccount,pfnumber,monthlypay.pay_band,monthlypay.grade_pay from monthlypay,desig_master,stafftrans,staffmaster,hrdept_master Where hrdept_master.dept_code=stafftrans.dept_code and stafftrans.staff_code=monthlypay.staff_code and stafftrans.latestrec=1 and desig_master.desig_code=stafftrans.desig_code and month(fdate) ='" + month5 + "' and month( tdate) ='" + month5 + "' and monthlypay.college_code=" + Session["collegecode"] + " and desig_master.collegecode=" + Session["collegecode"] + " and staffmaster.staff_code=monthlypay.staff_code  ";
                                    sql = " SELECT monthlypay.*,desig_master.priority,dept_acronym,staff_name,bankaccount,pfnumber,monthlypay.pay_band,monthlypay.grade_pay from monthlypay,desig_master,stafftrans,staffmaster,hrdept_master Where hrdept_master.dept_code=stafftrans.dept_code and stafftrans.staff_code=monthlypay.staff_code and stafftrans.latestrec=1 and desig_master.desig_code=stafftrans.desig_code and monthlypay.college_code=" + Session["collegecode"] + " and desig_master.collegecode=" + Session["collegecode"] + " and staffmaster.staff_code=monthlypay.staff_code  ";
                                }
                                if (cblmonthfrom.SelectedItem.Text != "All")
                                {
                                    sql = sql + " AND (PayMonth = '" + Convert.ToString(fdate.ToString("MM").TrimStart('0')) + "' and PayYear = '" + fdate.ToString("yyyy") + "')";
                                }
                                else
                                {
                                    sql = sql + " and (PayMonth = '" + Convert.ToString(fdate.ToString("MM").TrimStart('0')) + "' and PayYear = '" + fdate.ToString("yyyy") + "')";
                                }
                                if (tbseattype.Text != "---Select---")
                                {
                                    int itemcount = 0;

                                    strdept = "";
                                    gssmcat = "";
                                    for (itemcount = 0; itemcount < cbldepttype.Items.Count; itemcount++)
                                    {
                                        if (cbldepttype.Items[itemcount].Selected == true)
                                        {
                                            if (strdept == "")
                                            {
                                                strdept = "'" + cbldepttype.Items[itemcount].Value.ToString() + "'";
                                                gssmdept = cbldepttype.Items[itemcount].Value.ToString();
                                            }
                                            else
                                            {
                                                strdept = strdept + "," + "'" + cbldepttype.Items[itemcount].Value.ToString() + "'";
                                                gssmdept = gssmdept + "," + cbldepttype.Items[itemcount].Value.ToString();
                                            }
                                        }
                                    }
                                    gstrdept = gssmdept;

                                    if (strdept != "")
                                    {
                                        strdept = " in(" + strdept + ")";
                                    }

                                    sql = sql + " and hrdept_master.dept_code " + strdept + "";

                                }
                                else
                                {
                                    gstrdept = "all";
                                }
                                Session["strdept"] = gstrdept;
                                if (tbblood.Text != "---Select---")
                                {
                                    int itemcount1 = 0;
                                    strcategory = "";
                                    gssmcat = "";
                                    for (itemcount1 = 0; itemcount1 < cblcategory.Items.Count; itemcount1++)
                                    {
                                        if (cblcategory.Items[itemcount1].Selected == true)
                                        {
                                            if (strcategory == "")
                                            {
                                                strcategory = "'" + cblcategory.Items[itemcount1].Value.ToString() + "'";
                                                gssmcat = cblcategory.Items[itemcount1].Value.ToString();
                                            }
                                            else
                                            {
                                                strcategory = strcategory + "," + "'" + cblcategory.Items[itemcount1].Value.ToString() + "'";
                                                gssmcat = gssmcat + "," + cblcategory.Items[itemcount1].Value.ToString();
                                            }
                                        }
                                    }
                                    gstrcateogry = gssmcat;

                                    if (strcategory != "")
                                    {
                                        strcategory = " in (" + strcategory + ")";
                                    }
                                    sql = sql + "  and stafftrans.category_code" + strcategory + "";
                                    strcategory = "";
                                }
                                else
                                {
                                    gstrcateogry = "all";
                                }
                                Session["strcategory"] = gstrcateogry;
                                con.Close();
                                con.Open();
                                int m = 0;
                                int countstaff = 0;
                                SqlCommand cmd3 = new SqlCommand(sql, con);
                                SqlDataReader dr30;
                                string netadd = "";
                                double earntotal = 0;
                                string netded = "";
                                double totaldeduction = 0;
                                string netpa = "";
                                double totalnetpay = 0;

                                // DblAllowTotal[0] 
                                dr30 = cmd3.ExecuteReader();
                                while (dr30.Read())
                                {
                                    if (dr30.HasRows == true)
                                    {
                                        recflag = true;

                                        string allowance = "";
                                        string deduction = "";
                                        string basicpay = "";
                                        countstaff = countstaff + 1;
                                        int k = 0;
                                        int p = 4;
                                        int col3 = 0;
                                        col2 = 0;
                                        col3 = col;
                                        col2 = col;
                                        basicpay = dr30["bsalary"].ToString();
                                        netadd = dr30["netadd"].ToString();
                                        netded = dr30["netded"].ToString();
                                        netpa = dr30["netsal"].ToString();

                                        Double rnetpay = Convert.ToDouble(netpa);
                                        rnetpay = Math.Round(rnetpay, 0, MidpointRounding.AwayFromZero);
                                        netpa = rnetpay.ToString();
                                        totalnetpay = Convert.ToDouble(netpa) + totalnetpay;

                                        Double rnetde = Convert.ToDouble(netded);
                                        rnetde = Math.Round(rnetde, 0, MidpointRounding.AwayFromZero);
                                        netded = rnetde.ToString();
                                        totaldeduction = Convert.ToDouble(netded) + totaldeduction;

                                        Double erah = Convert.ToDouble(netadd);
                                        erah = Math.Round(erah, 0, MidpointRounding.AwayFromZero);
                                        netadd = erah.ToString();
                                        earntotal = Convert.ToDouble(netadd) + earntotal;

                                        Double rbasic = Convert.ToDouble(basicpay);
                                        rbasic = Math.Round(rbasic, 0, MidpointRounding.AwayFromZero);
                                        basicpay = rbasic.ToString();
                                        basicpaytotal = Convert.ToDouble(basicpay) + basicpaytotal;

                                        allowance = dr30["allowances"].ToString();
                                        deduction = dr30["Deductions"].ToString();
                                        // allown2[0]=allown2[0];
                                        // allcount2 = allcount3;

                                        string[] allowance2;
                                        int g = 0;


                                        string alowancesplit;

                                        allowanmce_arr1 = allowance.Split('\\');

                                        if (allowanmce_arr1.GetUpperBound(0) > 0)
                                        {

                                            for (m = 0; m < allowanmce_arr1.GetUpperBound(0); m++)
                                            {

                                            l2: alowancesplit = allowanmce_arr1[m];
                                                k = 0;
                                                p = 4;
                                                if (alowancesplit != "")
                                                {
                                                    allowance2 = alowancesplit.Split(';');
                                                    string[] splval = allowance2[2].Split('-');
                                                    if (allowance2[1].Trim() == "Amount")  //modified by jeyaprakash on Sep 6th(Allowance not invokes)
                                                    {
                                                        da3 = splval[0];
                                                    }
                                                    else if (allowance2[1].Trim() == "Percent" || allowance2[1].Trim() == "Slab")
                                                    {
                                                        if (splval.Length == 2)
                                                        {
                                                            da3 = splval[1];
                                                        }
                                                    }
                                                    //da3 = allowance2[3];

                                                    if (allowance2.GetUpperBound(0) > 0)
                                                    {


                                                    l3: if (p < fpsalarydemond.Sheets[0].ColumnCount - 1)
                                                        {
                                                            string headval = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, p].Text;
                                                            for (int j = 0; j < allowance2.GetUpperBound(0); j++)
                                                            {
                                                                if (headval == allowance2[j])
                                                                {
                                                                    Double allov = Convert.ToDouble(da3);
                                                                    allov = Math.Round(allov, 0, MidpointRounding.AwayFromZero);
                                                                    DblAllowTotal[k] = Convert.ToDouble(DblAllowTotal[k]) + allov;
                                                                    DblNetAllowTotal = allov + Convert.ToDouble(DblNetAllowTotal);

                                                                    m = m + 1;
                                                                    p = p + 1;
                                                                    k = k + 1;
                                                                    goto l2;

                                                                }
                                                                else
                                                                {

                                                                    p = p + 1;
                                                                    k = k + 1;
                                                                    goto l3;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                        }
                                                    }
                                                }
                                            }
                                            string[] deduction_arr1;

                                            string[] deduction2;

                                            k = 0;
                                            string deductionsplit;

                                            deduction_arr1 = deduction.Split('\\');

                                            if (deduction_arr1.GetUpperBound(0) > 0)
                                            {

                                                for (m = 0; m < deduction_arr1.GetUpperBound(0); m++)
                                                {
                                                l2: deductionsplit = deduction_arr1[m];
                                                    col3 = col;
                                                    k = 0;
                                                    if (deductionsplit != "")
                                                    {
                                                        deduction2 = deductionsplit.Split(';');
                                                        string[] splval = deduction2[2].Split('-');
                                                        if (deduction2[1].Trim() == "Amount")  //modified by jeyaprakash on Sep 6th(Allowance not invokes)
                                                        {
                                                            da3 = splval[0];
                                                        }
                                                        else if (deduction2[1].Trim() == "Percent" || deduction2[1].Trim() == "Slab")
                                                        {
                                                            if (splval.Length == 2)
                                                            {
                                                                da3 = splval[1];
                                                            }
                                                        }
                                                        //da3 = deduction2[3];
                                                        if (deduction2.GetUpperBound(0) > 0)
                                                        {
                                                        l3: if (col3 < fpsalarydemond.Sheets[0].ColumnCount - 1)
                                                            {
                                                                string headval1 = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, col3].Text;
                                                                for (int j = 0; j < deduction2.GetUpperBound(0); j++)
                                                                {
                                                                    if (headval1 == deduction2[j])
                                                                    {
                                                                        Double allov = Convert.ToDouble(da3);
                                                                        allov = Math.Round(allov, 0, MidpointRounding.AwayFromZero);
                                                                        deductiontotal[k] = deductiontotal[k] + allov;
                                                                        DblNetDedTotal = Convert.ToDouble(DblNetDedTotal) + allov;

                                                                        m = m + 1;
                                                                        col3 = col3 + 1;
                                                                        k = k + 1;
                                                                        goto l2;

                                                                    }
                                                                    else
                                                                    {

                                                                        col3 = col3 + 1;
                                                                        k = k + 1;
                                                                        goto l3;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                            }

                                                        }
                                                    }
                                                }
                                            }


                                        }
                                    }
                                    else
                                    {
                                        btnprintmaster.Visible = false;
                                        btnxl.Visible = false;
                                        lblexcel.Visible = false;
                                        txtxl.Visible = false;
                                        lblnorec.Text = "No Records Found";
                                        lblnorec.Visible = true;
                                    }


                                }



                                int rowstr1 = fpsalarydemond.Sheets[0].RowCount++;
                                mname = Convert.ToString(fdate.ToString("MM").TrimStart('0'));// month5.ToString();
                                string month7 = getmonth(mname);
                                /////////////////////////////////////////////////////////////////////////////////////////
                                fpsalarydemond.Sheets[0].Cells[rowstr1, 0].Value = 1;
                                fpsalarydemond.Sheets[0].Cells[rowstr1, 1].Tag = Convert.ToString(fdate.ToString("MM").TrimStart('0'));// month5.ToString();

                                fpsalarydemond.Sheets[0].Cells[rowstr1, 1].Text = month7.ToString();
                                string stafname = Convert.ToString(cblstfname.Items[st].Text);
                                fpsalarydemond.Sheets[0].Cells[rowstr1, 2].Text = stafname;
                                fpsalarydemond.Sheets[0].Cells[rowstr1, 2].HorizontalAlign = HorizontalAlign.Left;
                                fpsalarydemond.Sheets[0].Cells[rowstr1, 3].Text = basicpaytotal.ToString();
                                Double basAmnt = 0;
                                if (!dicaddtot.ContainsKey(3))
                                    dicaddtot.Add(3, basicpaytotal);
                                else
                                {
                                    Double.TryParse(Convert.ToString(dicaddtot[3]), out basAmnt);
                                    basAmnt = basAmnt + basicpaytotal;
                                    dicaddtot.Remove(3);
                                    dicaddtot.Add(3, basAmnt);
                                }
                                fpsalarydemond.Sheets[0].Cells[rowstr1, 2].Locked = true;

                                fpsalarydemond.Sheets[0].Cells[rowstr1, 3].HorizontalAlign = HorizontalAlign.Right;
                                fpsalarydemond.Sheets[0].Cells[rowstr1, 3].HorizontalAlign = HorizontalAlign.Right;
                                int g1 = 4;
                                string month;
                                // month = cblmonthfrom.SelectedItem.ToString();
                                for (int i = 0; i < getval; i++)
                                {
                                    DblAllowTotal[i] = Math.Round(DblAllowTotal[i], 2);

                                    fpsalarydemond.Sheets[0].Cells[rowstr1, g1].Text = DblAllowTotal[i].ToString();
                                    fpsalarydemond.Sheets[0].Cells[rowstr1, g1].HorizontalAlign = HorizontalAlign.Right;
                                    if (!dicaddtot.ContainsKey(g1))
                                        dicaddtot.Add(g1, DblAllowTotal[i]);
                                    else
                                    {
                                        Double.TryParse(Convert.ToString(dicaddtot[g1]), out basAmnt);
                                        basAmnt = basAmnt + DblAllowTotal[i];
                                        dicaddtot.Remove(g1);
                                        dicaddtot.Add(g1, basAmnt);
                                    }
                                    g1 = g1 + 1;

                                }
                                // fpsalary.Sheets[0].Cells[rowstr1, colheder - 1].Text = Convert.ToDouble(DblNetAllowTotal + basicpaytotal).ToString();
                                fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].HorizontalAlign = HorizontalAlign.Right;
                                for (int y = 0; y < getval2; y++)
                                {
                                    deductiontotal[y] = Math.Round(deductiontotal[y], 2);
                                    fpsalarydemond.Sheets[0].Cells[rowstr1, col2].Text = deductiontotal[y].ToString();
                                    fpsalarydemond.Sheets[0].Cells[rowstr1, col2].HorizontalAlign = HorizontalAlign.Right;
                                    if (!dicaddtot.ContainsKey(col2))
                                        dicaddtot.Add(col2, deductiontotal[y]);
                                    else
                                    {
                                        Double.TryParse(Convert.ToString(dicaddtot[col2]), out basAmnt);
                                        basAmnt = basAmnt + deductiontotal[y];
                                        dicaddtot.Remove(col2);
                                        dicaddtot.Add(col2, basAmnt);
                                    }
                                    col2 = col2 + 1;
                                }
                                col2 = 0;
                                col2 = col;
                                DblNetDedTotal = Math.Round(DblNetDedTotal, 2);
                                // fpsalary.Sheets[0].Cells[rowstr1, colcount1 - 2].Text = Convert.ToDouble(DblNetDedTotal).ToString();
                                fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].HorizontalAlign = HorizontalAlign.Right;
                                netpaytotal = (basicpaytotal + DblNetAllowTotal) - DblNetDedTotal;
                                netpaytotal = Math.Round(netpaytotal, 0, MidpointRounding.AwayFromZero);
                                //fpsalary.Sheets[0].Cells[rowstr1, colcount1 - 1].Text = Convert.ToDouble(netpaytotal).ToString();

                                earntotal = basicpaytotal + DblNetAllowTotal;
                                fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].Text = earntotal.ToString();
                                if (!dicaddtot.ContainsKey(colheder - 1))
                                    dicaddtot.Add(colheder - 1, earntotal);
                                else
                                {
                                    Double.TryParse(Convert.ToString(dicaddtot[colheder - 1]), out basAmnt);
                                    basAmnt = basAmnt + earntotal;
                                    dicaddtot.Remove(colheder - 1);
                                    dicaddtot.Add(colheder - 1, basAmnt);
                                }
                                totaldeduction = DblNetDedTotal;
                                fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].Locked = true;
                                fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].Text = totaldeduction.ToString();
                                if (!dicaddtot.ContainsKey(colcount1 - 2))
                                    dicaddtot.Add(colcount1 - 2, totaldeduction);
                                else
                                {
                                    Double.TryParse(Convert.ToString(dicaddtot[colcount1 - 2]), out basAmnt);
                                    basAmnt = basAmnt + totaldeduction;
                                    dicaddtot.Remove(colcount1 - 2);
                                    dicaddtot.Add(colcount1 - 2, basAmnt);
                                }
                                fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].Locked = true;

                                fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].Text = netpaytotal.ToString();
                                if (!dicaddtot.ContainsKey(colcount1 - 1))
                                    dicaddtot.Add(colcount1 - 1, netpaytotal);
                                else
                                {
                                    Double.TryParse(Convert.ToString(dicaddtot[colcount1 - 1]), out basAmnt);
                                    basAmnt = basAmnt + netpaytotal;
                                    dicaddtot.Remove(colcount1 - 1);
                                    dicaddtot.Add(colcount1 - 1, basAmnt);
                                }
                                fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].Locked = true;



                                fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].HorizontalAlign = HorizontalAlign.Right;
                                basicpaytotal = 0;
                                DblNetDedTotal = 0;
                                DblNetAllowTotal = 0;
                                netpaytotal = 0;
                                fdate = fdate.AddMonths(1);
                            }

                            if (fpsalarydemond.Sheets[0].RowCount - 1 > 0)
                            {
                                fpsalarydemond.Sheets[0].RowCount++;

                                fpsalarydemond.Sheets[0].Rows[fpsalarydemond.Sheets[0].RowCount - 1].Font.Bold = true;
                                //  fpsalary.Sheets[0].SpanModel.Add(fpsalary.Sheets[0].RowCount - 1, 0, 1, 2);
                                fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 0].Locked = true;
                                fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Text = "Total";
                                for (int intColCtr = 2; intColCtr < fpsalarydemond.Sheets[0].ColumnCount; intColCtr++)
                                {
                                    //IntMTotal = 0;
                                    //for (int IntRowCtr = 0; IntRowCtr < fpsalarydemond.Sheets[0].RowCount - 1; IntRowCtr++)
                                    //{
                                    //    IntMTemp = 0;
                                    //    if (fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text != "-")
                                    //    {
                                    //        if (fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text != "")
                                    //        {
                                    //            IntMTemp = Convert.ToDouble(fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text);
                                    //        }
                                    //        else
                                    //        {
                                    //            IntMTemp = 0;
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        IntMTemp = 0;
                                    //    }
                                    //    IntMTotal = IntMTemp + IntMTotal;
                                    //    IntMTotal = Math.Round(IntMTotal, 2);
                                    //}
                                    Double getNewTot = 0;
                                    if (dicaddtot.ContainsKey(intColCtr))
                                    {
                                        if (Convert.ToString(dicaddtot[intColCtr]) != "0")
                                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Text = Convert.ToString(dicaddtot[intColCtr]);
                                        else
                                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Text = "-";
                                        if (!dicaddgrandtot.ContainsKey(intColCtr))
                                            dicaddgrandtot.Add(intColCtr, dicaddtot[intColCtr]);
                                        else
                                        {
                                            Double.TryParse(Convert.ToString(dicaddgrandtot[intColCtr]), out getNewTot);
                                            getNewTot = getNewTot + dicaddtot[intColCtr];
                                            dicaddgrandtot.Remove(intColCtr);
                                            dicaddgrandtot.Add(intColCtr, getNewTot);
                                        }
                                    }
                                    else
                                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Text = "-";
                                    //fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Text = IntMTotal > 0 ? IntMTotal + "" : "-";
                                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].HorizontalAlign = HorizontalAlign.Right;
                                    if (intColCtr == 2)
                                    {
                                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].HorizontalAlign = HorizontalAlign.Center;
                                    }

                                }
                            }

                            //Double totalRows = 0;
                            //totalRows = Convert.ToInt32(fpsalarydemond.Sheets[0].RowCount);

                            //if (totalRows >= 10)
                            //{
                            //    fpsalarydemond.Sheets[0].PageSize = Convert.ToInt32(totalRows);



                            //}
                            //else if (totalRows == 0)
                            //{

                            //    fpsalarydemond.Height = 300;
                            //}
                            //else
                            //{
                            //    fpsalarydemond.Sheets[0].PageSize = Convert.ToInt32(totalRows);


                            //}
                            //Session["totalPages"] = (int)Math.Ceiling(totalRows / fpsalarydemond.Sheets[0].PageSize);
                        }
                    }
                }
                if (fpsalarydemond.Sheets[0].RowCount - 1 > 0)
                {
                    fpsalarydemond.Sheets[0].RowCount++;

                    fpsalarydemond.Sheets[0].Rows[fpsalarydemond.Sheets[0].RowCount - 1].Font.Bold = true;
                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 0].Locked = true;
                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 2].Text = "Grand Total";
                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                    for (int intColCtr = 3; intColCtr < fpsalarydemond.Sheets[0].ColumnCount; intColCtr++)
                    {
                        if (dicaddgrandtot.ContainsKey(intColCtr))
                        {
                            if (Convert.ToString(dicaddgrandtot[intColCtr]) != "0")
                                fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Text = Convert.ToString(dicaddgrandtot[intColCtr]);
                            else
                                fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Text = "-";
                        }
                        else
                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Text = "-";
                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Font.Name = "Book Antiqua";
                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Font.Size = FontUnit.Medium;
                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Font.Bold = true;
                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].HorizontalAlign = HorizontalAlign.Right;
                        if (intColCtr == 2)
                        {
                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }

                Double totalRows1 = 0;
                totalRows1 = Convert.ToInt32(fpsalarydemond.Sheets[0].RowCount);

                if (totalRows1 >= 10)
                {
                    fpsalarydemond.Sheets[0].PageSize = Convert.ToInt32(totalRows1);


                    fpsalarydemond.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                    fpsalarydemond.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

                }
                else if (totalRows1 == 0)
                {


                }
                else
                {
                    fpsalarydemond.Sheets[0].PageSize = Convert.ToInt32(totalRows1);

                }
                fpsalarydemond.Sheets[0].PageSize = fpsalarydemond.Sheets[0].RowCount;
                Session["totalPages"] = (int)Math.Ceiling(totalRows1 / fpsalarydemond.Sheets[0].PageSize);
            }
            else
            {
                if (cblmonthfrom.SelectedItem.ToString() != "All")
                {

                    //for (int b = monthfrom; b <= monthto; b++)
                    while (fdate < tdate)
                    {
                        //========Start============Added by Manikandan 20/08/2013=========================
                        string monthsdate = string.Empty;
                        string monthedate = string.Empty;
                        string date_1 = string.Empty;
                        string date_2 = string.Empty;
                        string year = cblbatchyear.SelectedItem.Text;
                        string sqlquery = "select CONVERT(VARCHAR(10),from_date,103) as from_date,convert(VARCHAR(10),to_date,103) as to_date from hrpaymonths where paymonthnum='" + Convert.ToString(fdate.ToString("MM").TrimStart('0')) + "' and PayYear='" + Convert.ToString(fdate.ToString("yyyy")) + "'";
                        con.Close();
                        con.Open();
                        SqlDataAdapter da_hrdate = new SqlDataAdapter(sqlquery, con);
                        DataTable dt_hrdate = new DataTable();
                        da_hrdate.Fill(dt_hrdate);
                        if (dt_hrdate.Rows.Count > 0)
                        {
                            monthsdate = dt_hrdate.Rows[0]["from_date"].ToString();
                            monthedate = dt_hrdate.Rows[0]["to_date"].ToString();

                            string[] split_date = monthsdate.Split(new char[] { '/' });
                            date_1 = split_date[1] + "/" + split_date[0] + "/" + split_date[2];
                            string[] split_date_2 = monthedate.Split(new char[] { '/' });
                            string tday = split_date_2[0];
                            date_2 = split_date_2[1] + "/" + split_date_2[0] + "/" + split_date_2[2];
                            //if (tday == "31")   modified by jeyaprakash
                            //{
                            //    date_2 = split_date_2[1] + "/" + 31 + "/" + year;
                            //}
                            //else if (tday == "30")
                            //{
                            //    date_2 = split_date_2[1] + "/" + 30 + "/" + year;
                            //}


                            //else if (tday == "28")
                            //{
                            //    date_2 = split_date_2[1] + "/" + 28 + "/" + year;
                            //}
                            //else
                            //{
                            //    date_2 = split_date_2[1] + "/" + 29 + "/" + year;
                            //}


                            //if (Convert.ToInt32(split_date_2[1]) == 2)  modified by jeyaprakash
                            //{
                            //    if ((Convert.ToInt32(year) % 4) == 0)
                            //    {
                            //        date_2 = split_date_2[1] + "/" + 29 + "/" + year;
                            //    }
                            //    else
                            //    {
                            //        date_2 = split_date_2[1] + "/" + 28 + "/" + year;
                            //    }
                            //}
                            //============================End=================================================
                            Array.Clear(DblAllowTotal, 0, DblAllowTotal.Length);
                            Array.Clear(deductiontotal, 0, deductiontotal.Length);
                            sql = "";
                            //sql = " SELECT monthlypay.*,desig_master.priority,dept_acronym,staff_name,bankaccount,pfnumber,monthlypay.pay_band,monthlypay.grade_pay from monthlypay,desig_master,stafftrans,staffmaster,hrdept_master Where hrdept_master.dept_code=stafftrans.dept_code and stafftrans.staff_code=monthlypay.staff_code and stafftrans.latestrec=1 and desig_master.desig_code=stafftrans.desig_code and month(fdate) ='" + b + "' and  month(tdate) ='" + b + "' and monthlypay.college_code=" + Session["collegecode"] + " and desig_master.collegecode=" + Session["collegecode"] + " and staffmaster.staff_code=monthlypay.staff_code  ";
                            if (cbSelect.Checked == true)
                            {
                                sql = " SELECT monthlypay.*,desig_master.priority,dept_acronym,staff_name,bankaccount,pfnumber,monthlypay.pay_band,monthlypay.grade_pay from monthlypay,desig_master,stafftrans,staffmaster,hrdept_master Where hrdept_master.dept_code=stafftrans.dept_code and stafftrans.staff_code=monthlypay.staff_code and stafftrans.latestrec=1 and desig_master.desig_code=stafftrans.desig_code and (PayMonth = '" + Convert.ToString(fdate.ToString("MM").TrimStart('0')) + "' and PayYear = '" + Convert.ToString(fdate.ToString("yyyy")) + "') and monthlypay.college_code=" + Session["collegecode"] + " and desig_master.collegecode=" + Session["collegecode"] + " and staffmaster.staff_code=monthlypay.staff_code and staffmaster.staff_code = '" + stafcode + "'";//This Query Modified by Manikandan 20/08/2013
                            }
                            else
                            {
                                sql = " SELECT monthlypay.*,desig_master.priority,dept_acronym,staff_name,bankaccount,pfnumber,monthlypay.pay_band,monthlypay.grade_pay from monthlypay,desig_master,stafftrans,staffmaster,hrdept_master Where hrdept_master.dept_code=stafftrans.dept_code and stafftrans.staff_code=monthlypay.staff_code and stafftrans.latestrec=1 and desig_master.desig_code=stafftrans.desig_code and  (PayMonth = '" + Convert.ToString(fdate.ToString("MM").TrimStart('0')) + "' and PayYear = '" + Convert.ToString(fdate.ToString("yyyy")) + "') and monthlypay.college_code=" + Session["collegecode"] + " and desig_master.collegecode=" + Session["collegecode"] + " and staffmaster.staff_code=monthlypay.staff_code";
                            }

                            //( (PayMonth >= '" + frommonth + "' and PayYear = '" + fromyear + "') or (PayMonth <='" + tomonth + "' and PayYear = '" + toyear + "' ))

                            // sql = " SELECT monthlypay.*,desig_master.priority,dept_acronym,staff_name,bankaccount,pfnumber,monthlypay.pay_band,monthlypay.grade_pay from monthlypay,desig_master,stafftrans,staffmaster,hrdept_master Where hrdept_master.dept_code=stafftrans.dept_code and stafftrans.staff_code=monthlypay.staff_code and stafftrans.latestrec=1 and desig_master.desig_code=stafftrans.desig_code and monthlypay.fdate ='" + datefrom + "' and monthlypay.tdate ='" + dateto + "'and monthlypay.college_code=" + Session["collegecode"] + " and desig_master.collegecode=" + Session["collegecode"] + " and staffmaster.staff_code=monthlypay.staff_code  ";//sql = sql + " Where M.Category_Code = C.Category_Code and year(fdate)='" + cblbatchyear.SelectedItem.Value.ToString() + "'";

                            //sql = sql + " AND month(fdate) ='" + b + "' and month( tdate) ='" + b + "'";//Hided by Manikandan 20/08/2013

                            if (tbseattype.Text != "---Select---") //department
                            {
                                int itemcount = 0;
                                strdept = "";
                                gssmcat = "";

                                for (itemcount = 0; itemcount < cbldepttype.Items.Count; itemcount++)
                                {
                                    if (cbldepttype.Items[itemcount].Selected == true)
                                    {
                                        if (strdept == "")
                                        {
                                            strdept = "'" + cbldepttype.Items[itemcount].Value.ToString() + "'";

                                            gssmdept = cbldepttype.Items[itemcount].Value.ToString();
                                        }
                                        else
                                        {
                                            strdept = strdept + "," + "'" + cbldepttype.Items[itemcount].Value.ToString() + "'";
                                            gssmdept = gssmdept + "," + cbldepttype.Items[itemcount].Value.ToString();
                                        }
                                    }
                                }
                                gstrdept = gssmdept;

                                if (strdept != "")
                                {
                                    strdept = " in(" + strdept + ")";
                                }
                                sql = sql + " and hrdept_master.dept_code " + strdept + "";

                            }
                            else
                            {
                                gstrdept = "all";
                            }
                            Session["strdept"] = gstrdept;
                            if (tbblood.Text != "---Select---")
                            {


                                int itemcount1 = 0;
                                strcategory = "";
                                gssmcat = "";
                                for (itemcount1 = 0; itemcount1 < cblcategory.Items.Count; itemcount1++)
                                {
                                    if (cblcategory.Items[itemcount1].Selected == true)
                                    {
                                        if (strcategory == "")
                                        {
                                            strcategory = "'" + cblcategory.Items[itemcount1].Value.ToString() + "'";
                                            gssmcat = cblcategory.Items[itemcount1].Value.ToString();
                                            //strcategory = cblcategory.Items[itemcount1].Value.ToString();
                                        }
                                        else
                                        {
                                            strcategory = strcategory + "," + "'" + cblcategory.Items[itemcount1].Value.ToString() + "'";
                                            gssmcat = gssmcat + "," + cblcategory.Items[itemcount1].Value.ToString();
                                            //strcategory = strcategory+","+cblcategory.Items[itemcount1].Value.ToString();
                                        }
                                    }
                                }

                                gstrcateogry = gssmcat;
                                if (strcategory != "")
                                {
                                    strcategory = " in (" + strcategory + ")";
                                }
                                sql = sql + "  and stafftrans.category_code" + strcategory + "";

                            }
                            else
                            {
                                gstrcateogry = "all";
                            }
                            Session["strcategory"] = gstrcateogry;
                            con.Close();
                            con.Open();

                            //Session["allowanddedu"] = strcategory + "@" + strdept + "@" + strallallowance + "@" + stralldeduct;
                            // string ddd = Session["allowanddedu"].ToString();


                            int m = 0;

                            SqlCommand cmd3 = new SqlCommand(sql, con);
                            SqlDataReader dr30;
                            int countstaff = 0;

                            string netadd = "";
                            double earntotal = 0;
                            string netded = "";
                            double totaldeduction = 0;
                            string netpa = "";
                            double totalnetpay = 0;
                            dr30 = cmd3.ExecuteReader();
                            while (dr30.Read())
                            {
                                if (dr30.HasRows == true)
                                {
                                    recflag = true;
                                    string allowance = "";
                                    string deduction = "";
                                    string basicpay = "";

                                    countstaff = countstaff + 1;

                                    int k = 0;
                                    int p = 4;
                                    int col3 = 0;
                                    col2 = 0;
                                    col3 = col;
                                    col2 = col;
                                    // DblAllowTotal = 0;
                                    basicpay = dr30["bsalary"].ToString();

                                    netadd = dr30["netadd"].ToString();
                                    netded = dr30["netded"].ToString();
                                    netpa = dr30["netsal"].ToString();

                                    Double netpanetpa = Convert.ToDouble(netpa);
                                    netpanetpa = Math.Round(netpanetpa, 0, MidpointRounding.AwayFromZero);
                                    netpa = netpanetpa.ToString();
                                    totalnetpay = Convert.ToDouble(netpa) + totalnetpay;


                                    Double netdepa = Convert.ToDouble(netded);
                                    netdepa = Math.Round(netdepa, 0, MidpointRounding.AwayFromZero);
                                    netded = netdepa.ToString();
                                    totaldeduction = Convert.ToDouble(netded) + totaldeduction;

                                    Double erah = Convert.ToDouble(netadd);
                                    erah = Math.Round(erah, 0, MidpointRounding.AwayFromZero);
                                    netadd = erah.ToString();
                                    earntotal = Convert.ToDouble(netadd) + earntotal;

                                    Double basicl = Convert.ToDouble(basicpay);
                                    basicl = Math.Round(basicl, 0, MidpointRounding.AwayFromZero);
                                    basicpay = basicl.ToString();

                                    basicpaytotal = Convert.ToDouble(basicpay) + basicpaytotal;

                                    allowance = dr30["allowances"].ToString();
                                    deduction = dr30["Deductions"].ToString();

                                    string[] allowance2;
                                    int g = 0;
                                    string alowancesplit;
                                    allowanmce_arr1 = allowance.Split('\\');
                                    if (allowanmce_arr1.GetUpperBound(0) > 0)
                                    {
                                        for (m = 0; m < allowanmce_arr1.GetUpperBound(0); m++)
                                        {

                                        l2: alowancesplit = allowanmce_arr1[m];
                                            k = 0;
                                            p = 4;
                                            if (alowancesplit != "")
                                            {
                                                allowance2 = alowancesplit.Split(';');
                                                string[] splval = allowance2[2].Split('-');
                                                if (allowance2[1].Trim() == "Amount")  //modified by jeyaprakash on Sep 6th(Allowance not invokes)
                                                {
                                                    da3 = splval[0];
                                                }
                                                else if (allowance2[1].Trim() == "Percent" || allowance2[1].Trim() == "Slab")
                                                {
                                                    if (splval.Length == 2)
                                                    {
                                                        da3 = splval[1];
                                                    }
                                                }
                                                //da3 = allowance2[3];

                                                if (allowance2.GetUpperBound(0) > 0)
                                                {
                                                l3: if (p < fpsalarydemond.Sheets[0].ColumnCount - 1) // changed
                                                    {
                                                        string headval = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, p].Text;
                                                        for (int j = 0; j < allowance2.GetUpperBound(0); j++)
                                                        {
                                                            Double alle = 0;
                                                            if (headval == allowance2[j])
                                                            {
                                                                Double.TryParse(da3, out alle);
                                                                alle = Math.Round(alle, 0, MidpointRounding.AwayFromZero);
                                                                DblAllowTotal[k] = Convert.ToDouble(DblAllowTotal[k]) + Convert.ToDouble(alle);
                                                                DblNetAllowTotal = alle + Convert.ToDouble(DblNetAllowTotal);

                                                                m = m + 1;
                                                                p = p + 1;
                                                                k = k + 1;
                                                                goto l2;

                                                            }
                                                            else
                                                            {

                                                                p = p + 1;
                                                                k = k + 1;
                                                                goto l3;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    string[] deduction_arr1;

                                    string[] deduction2;

                                    k = 0;
                                    string deductionsplit;

                                    deduction_arr1 = deduction.Split('\\');

                                    if (deduction_arr1.GetUpperBound(0) > 0)
                                    {
                                        for (m = 0; m < deduction_arr1.GetUpperBound(0); m++)
                                        {
                                        l2: deductionsplit = deduction_arr1[m];
                                            col3 = col;
                                            k = 0;
                                            if (deductionsplit != "")
                                            {
                                                deduction2 = deductionsplit.Split(';');
                                                string[] splval = deduction2[2].Split('-');
                                                if (deduction2[1].Trim() == "Amount")  //modified by jeyaprakash on Sep 6th(Allowance not invokes)
                                                {
                                                    da3 = splval[0];
                                                }
                                                else if (deduction2[1].Trim() == "Percent" || deduction2[1].Trim() == "Slab")
                                                {
                                                    if (splval.Length == 2)
                                                    {
                                                        da3 = splval[1];
                                                    }
                                                }
                                                //da3 = deduction2[3];
                                                if (deduction2.GetUpperBound(0) > 0)
                                                {
                                                l3: if (col3 < fpsalarydemond.Sheets[0].ColumnCount - 1) //changed fpsalary to fpsalarydemond
                                                    {
                                                        string headval1 = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, col3].Text;
                                                        for (int j = 0; j < deduction2.GetUpperBound(0); j++)
                                                        {
                                                            Double allov = 0;
                                                            if (headval1 == deduction2[j])
                                                            {
                                                                Double.TryParse(da3, out allov);
                                                                allov = Math.Round(allov, 0, MidpointRounding.AwayFromZero);
                                                                deductiontotal[k] = deductiontotal[k] + allov;
                                                                DblNetDedTotal = Convert.ToDouble(DblNetDedTotal) + allov;

                                                                m = m + 1;
                                                                col3 = col3 + 1;
                                                                k = k + 1;
                                                                goto l2;

                                                            }
                                                            else
                                                            {

                                                                col3 = col3 + 1;
                                                                k = k + 1;
                                                                goto l3;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                    }

                                                }
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    fpsalarydemond.Visible = false;
                                    btnprintmaster.Visible = false;
                                    btnxl.Visible = false;
                                    lblexcel.Visible = false;
                                    txtxl.Visible = false;
                                    lblnorec.Visible = true;
                                    lblnorec.Text = "No Records Found";
                                }


                            }



                            int rowstr1 = fpsalarydemond.Sheets[0].RowCount++;
                            mname = Convert.ToString(fdate.ToString("MM").TrimStart('0'));// b.ToString();
                            string month7 = getmonth(mname);
                            /////////////////////////////////////////////////////////////////////////////////////////
                            fpsalarydemond.Sheets[0].Cells[rowstr1, 0].Value = 1;
                            fpsalarydemond.Sheets[0].Cells[rowstr1, 1].Tag = Convert.ToString(fdate.ToString("MM").TrimStart('0'));// b.ToString();

                            fpsalarydemond.Sheets[0].Cells[rowstr1, 1].Text = month7.ToString();
                            fpsalarydemond.Sheets[0].Cells[rowstr1, 3].Text = basicpaytotal.ToString();

                            fpsalarydemond.Sheets[0].Cells[rowstr1, 2].Text = countstaff.ToString();
                            fpsalarydemond.Sheets[0].Cells[rowstr1, 2].Locked = true;
                            fpsalarydemond.Sheets[0].Cells[rowstr1, 2].HorizontalAlign = HorizontalAlign.Center;
                            fpsalarydemond.Sheets[0].Cells[rowstr1, 3].HorizontalAlign = HorizontalAlign.Right;
                            int g1 = 4;
                            for (int i = 0; i < getval; i++)
                            {
                                DblAllowTotal[i] = Math.Round(DblAllowTotal[i], 2);

                                fpsalarydemond.Sheets[0].Cells[rowstr1, g1].Text = DblAllowTotal[i].ToString();
                                fpsalarydemond.Sheets[0].Cells[rowstr1, g1].HorizontalAlign = HorizontalAlign.Right;
                                g1 = g1 + 1;

                            }
                            fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].HorizontalAlign = HorizontalAlign.Right;
                            for (int y = 0; y < getval2; y++)
                            {
                                deductiontotal[y] = Math.Round(deductiontotal[y], 2);
                                fpsalarydemond.Sheets[0].Cells[rowstr1, col2].Text = deductiontotal[y].ToString();
                                fpsalarydemond.Sheets[0].Cells[rowstr1, col2].HorizontalAlign = HorizontalAlign.Right;
                                col2 = col2 + 1;
                            }
                            col2 = 0;
                            col2 = col;
                            DblNetDedTotal = Math.Round(DblNetDedTotal, 2);
                            string tot = Convert.ToString(earntotal);
                            fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].Text = earntotal.ToString();
                            fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].Locked = true;
                            fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].Text = totaldeduction.ToString();
                            fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].Locked = true;
                            fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].HorizontalAlign = HorizontalAlign.Right;
                            fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].Text = totalnetpay.ToString();
                            fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].Locked = true;
                            fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].HorizontalAlign = HorizontalAlign.Right;
                            basicpaytotal = 0;
                            DblNetDedTotal = 0;
                            DblNetAllowTotal = 0;
                            netpaytotal = 0;
                        }
                        fdate = fdate.AddMonths(1);
                    }

                    if (fpsalarydemond.Sheets[0].RowCount - 1 > 0)
                    {
                        fpsalarydemond.Sheets[0].RowCount++;

                        fpsalarydemond.Sheets[0].Rows[fpsalarydemond.Sheets[0].RowCount - 1].Font.Bold = true;
                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 0].Locked = true;
                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Text = "Total";
                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        for (int intColCtr = 2; intColCtr < fpsalarydemond.Sheets[0].ColumnCount; intColCtr++)
                        {
                            IntMTotal = 0;
                            for (int IntRowCtr = 0; IntRowCtr < fpsalarydemond.Sheets[0].RowCount - 1; IntRowCtr++)
                            {
                                IntMTemp = 0;
                                if (fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text != "-")
                                {
                                    if (fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text != "")
                                    {
                                        string testval = fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text;
                                        Double.TryParse(Convert.ToString(fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text), out IntMTemp);
                                    }
                                    else
                                    {
                                        IntMTemp = 0;
                                    }
                                }
                                else
                                {
                                    IntMTemp = 0;
                                }
                                IntMTotal = IntMTemp + IntMTotal;
                                IntMTotal = Math.Round(IntMTotal, 2);
                            }
                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Text = IntMTotal > 0 ? IntMTotal + "" : "-";
                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Font.Name = "Book Antiqua";
                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Font.Size = FontUnit.Medium;
                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Font.Bold = true;
                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].HorizontalAlign = HorizontalAlign.Right;
                            if (intColCtr == 2)
                            {
                                fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                    Double totalRows = 0;
                    totalRows = Convert.ToInt32(fpsalarydemond.Sheets[0].RowCount);

                    if (totalRows >= 10)
                    {
                        fpsalarydemond.Sheets[0].PageSize = Convert.ToInt32(totalRows);


                        fpsalarydemond.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                        fpsalarydemond.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

                    }
                    else if (totalRows == 0)
                    {


                    }
                    else
                    {
                        fpsalarydemond.Sheets[0].PageSize = Convert.ToInt32(totalRows);

                    }
                    fpsalarydemond.Sheets[0].PageSize = fpsalarydemond.Sheets[0].RowCount;
                    Session["totalPages"] = (int)Math.Ceiling(totalRows / fpsalarydemond.Sheets[0].PageSize);
                }
                else if (cblmonthfrom.SelectedItem.ToString() == "All")
                {
                    //int month5 = 1;

                    //for (month5 = 1; month5 <= 12; month5++)
                    while (fdate < tdate)
                    {
                        Array.Clear(DblAllowTotal, 0, DblAllowTotal.Length);
                        Array.Clear(deductiontotal, 0, deductiontotal.Length);
                        sql = "";
                        if (cbSelect.Checked == true)
                        {
                            //sql = " SELECT monthlypay.*,desig_master.priority,dept_acronym,staff_name,bankaccount,pfnumber,monthlypay.pay_band,monthlypay.grade_pay from monthlypay,desig_master,stafftrans,staffmaster,hrdept_master Where hrdept_master.dept_code=stafftrans.dept_code and stafftrans.staff_code=monthlypay.staff_code and stafftrans.latestrec=1 and desig_master.desig_code=stafftrans.desig_code and month(fdate) ='" + month5 + "' and month( tdate) ='" + month5 + "' and monthlypay.college_code=" + Session["collegecode"] + " and desig_master.collegecode=" + Session["collegecode"] + " and staffmaster.staff_code=monthlypay.staff_code and staffmaster.staff_code = '" + stafcode + "'";
                            sql = " SELECT monthlypay.*,desig_master.priority,dept_acronym,staff_name,bankaccount,pfnumber,monthlypay.pay_band,monthlypay.grade_pay from monthlypay,desig_master,stafftrans,staffmaster,hrdept_master Where hrdept_master.dept_code=stafftrans.dept_code and stafftrans.staff_code=monthlypay.staff_code and stafftrans.latestrec=1 and desig_master.desig_code=stafftrans.desig_code and monthlypay.college_code=" + Session["collegecode"] + " and desig_master.collegecode=" + Session["collegecode"] + " and staffmaster.staff_code=monthlypay.staff_code and staffmaster.staff_code = '" + stafcode + "'";
                        }
                        else if (cblmonthfrom.SelectedItem.Text == "All")
                        {
                            //sql = " SELECT monthlypay.*,desig_master.priority,dept_acronym,staff_name,bankaccount,pfnumber,monthlypay.pay_band,monthlypay.grade_pay from monthlypay,desig_master,stafftrans,staffmaster,hrdept_master Where hrdept_master.dept_code=stafftrans.dept_code and stafftrans.staff_code=monthlypay.staff_code and stafftrans.latestrec=1 and desig_master.desig_code=stafftrans.desig_code and monthlypay.college_code=" + Session["collegecode"] + " and desig_master.collegecode=" + Session["collegecode"] + " and staffmaster.staff_code=monthlypay.staff_code and staffmaster.staff_code = '" + stafcode + "'    ";
                            sql = " SELECT monthlypay.*,desig_master.priority,dept_acronym,staff_name,bankaccount,pfnumber,monthlypay.pay_band,monthlypay.grade_pay from monthlypay,desig_master,stafftrans,staffmaster,hrdept_master Where hrdept_master.dept_code=stafftrans.dept_code and stafftrans.staff_code=monthlypay.staff_code and stafftrans.latestrec=1 and desig_master.desig_code=stafftrans.desig_code and monthlypay.college_code=" + Session["collegecode"] + " and desig_master.collegecode=" + Session["collegecode"] + " and staffmaster.staff_code=monthlypay.staff_code ";
                        }
                        else
                        {
                            // sql = " SELECT monthlypay.*,desig_master.priority,dept_acronym,staff_name,bankaccount,pfnumber,monthlypay.pay_band,monthlypay.grade_pay from monthlypay,desig_master,stafftrans,staffmaster,hrdept_master Where hrdept_master.dept_code=stafftrans.dept_code and stafftrans.staff_code=monthlypay.staff_code and stafftrans.latestrec=1 and desig_master.desig_code=stafftrans.desig_code and month(fdate) ='" + month5 + "' and month( tdate) ='" + month5 + "' and monthlypay.college_code=" + Session["collegecode"] + " and desig_master.collegecode=" + Session["collegecode"] + " and staffmaster.staff_code=monthlypay.staff_code  ";
                            sql = " SELECT monthlypay.*,desig_master.priority,dept_acronym,staff_name,bankaccount,pfnumber,monthlypay.pay_band,monthlypay.grade_pay from monthlypay,desig_master,stafftrans,staffmaster,hrdept_master Where hrdept_master.dept_code=stafftrans.dept_code and stafftrans.staff_code=monthlypay.staff_code and stafftrans.latestrec=1 and desig_master.desig_code=stafftrans.desig_code and monthlypay.college_code=" + Session["collegecode"] + " and desig_master.collegecode=" + Session["collegecode"] + " and staffmaster.staff_code=monthlypay.staff_code  ";
                        }
                        if (cblmonthfrom.SelectedItem.Text != "All")
                        {
                            sql = sql + " AND (PayMonth = '" + Convert.ToString(fdate.ToString("MM").TrimStart('0')) + "' and PayYear = '" + Convert.ToString(fdate.ToString("yyyy")) + "')";
                        }
                        else
                        {
                            sql = sql + " and  (PayMonth = '" + Convert.ToString(fdate.ToString("MM").TrimStart('0')) + "' and PayYear = '" + Convert.ToString(fdate.ToString("yyyy")) + "')";
                        }
                        if (tbseattype.Text != "---Select---")
                        {
                            int itemcount = 0;

                            strdept = "";
                            gssmcat = "";
                            for (itemcount = 0; itemcount < cbldepttype.Items.Count; itemcount++)
                            {
                                if (cbldepttype.Items[itemcount].Selected == true)
                                {
                                    if (strdept == "")
                                    {
                                        strdept = "'" + cbldepttype.Items[itemcount].Value.ToString() + "'";
                                        gssmdept = cbldepttype.Items[itemcount].Value.ToString();
                                    }
                                    else
                                    {
                                        strdept = strdept + "," + "'" + cbldepttype.Items[itemcount].Value.ToString() + "'";
                                        gssmdept = gssmdept + "," + cbldepttype.Items[itemcount].Value.ToString();
                                    }
                                }
                            }
                            gstrdept = gssmdept;

                            if (strdept != "")
                            {
                                strdept = " in(" + strdept + ")";
                            }

                            sql = sql + " and hrdept_master.dept_code " + strdept + "";

                        }
                        else
                        {
                            gstrdept = "all";
                        }
                        Session["strdept"] = gstrdept;
                        if (tbblood.Text != "---Select---")
                        {
                            int itemcount1 = 0;
                            strcategory = "";
                            gssmcat = "";
                            for (itemcount1 = 0; itemcount1 < cblcategory.Items.Count; itemcount1++)
                            {
                                if (cblcategory.Items[itemcount1].Selected == true)
                                {
                                    if (strcategory == "")
                                    {
                                        strcategory = "'" + cblcategory.Items[itemcount1].Value.ToString() + "'";
                                        gssmcat = cblcategory.Items[itemcount1].Value.ToString();
                                    }
                                    else
                                    {
                                        strcategory = strcategory + "," + "'" + cblcategory.Items[itemcount1].Value.ToString() + "'";
                                        gssmcat = gssmcat + "," + cblcategory.Items[itemcount1].Value.ToString();
                                    }
                                }
                            }
                            gstrcateogry = gssmcat;

                            if (strcategory != "")
                            {
                                strcategory = " in (" + strcategory + ")";
                            }
                            sql = sql + "  and stafftrans.category_code" + strcategory + "";
                            strcategory = "";
                        }
                        else
                        {
                            gstrcateogry = "all";
                        }
                        Session["strcategory"] = gstrcateogry;
                        con.Close();
                        con.Open();
                        int m = 0;
                        int countstaff = 0;
                        SqlCommand cmd3 = new SqlCommand(sql, con);
                        SqlDataReader dr30;
                        string netadd = "";
                        double earntotal = 0;
                        string netded = "";
                        double totaldeduction = 0;
                        string netpa = "";
                        double totalnetpay = 0;

                        // DblAllowTotal[0] 
                        dr30 = cmd3.ExecuteReader();
                        while (dr30.Read())
                        {
                            if (dr30.HasRows == true)
                            {
                                recflag = true;

                                string allowance = "";
                                string deduction = "";
                                string basicpay = "";
                                countstaff = countstaff + 1;
                                int k = 0;
                                int p = 4;
                                int col3 = 0;
                                col2 = 0;
                                col3 = col;
                                col2 = col;
                                basicpay = dr30["bsalary"].ToString();
                                netadd = dr30["netadd"].ToString();
                                netded = dr30["netded"].ToString();
                                netpa = dr30["netsal"].ToString();

                                Double rnetpay = Convert.ToDouble(netpa);
                                rnetpay = Math.Round(rnetpay, 0, MidpointRounding.AwayFromZero);
                                netpa = rnetpay.ToString();
                                totalnetpay = Convert.ToDouble(netpa) + totalnetpay;

                                Double rnetde = Convert.ToDouble(netded);
                                rnetde = Math.Round(rnetde, 0, MidpointRounding.AwayFromZero);
                                netded = rnetde.ToString();
                                totaldeduction = Convert.ToDouble(netded) + totaldeduction;

                                Double erah = Convert.ToDouble(netadd);
                                erah = Math.Round(erah, 0, MidpointRounding.AwayFromZero);
                                netadd = erah.ToString();
                                earntotal = Convert.ToDouble(netadd) + earntotal;

                                Double rbasic = Convert.ToDouble(basicpay);
                                rbasic = Math.Round(rbasic, 0, MidpointRounding.AwayFromZero);
                                basicpay = rbasic.ToString();
                                basicpaytotal = Convert.ToDouble(basicpay) + basicpaytotal;

                                allowance = dr30["allowances"].ToString();
                                deduction = dr30["Deductions"].ToString();
                                // allown2[0]=allown2[0];
                                // allcount2 = allcount3;

                                string[] allowance2;
                                int g = 0;


                                string alowancesplit;

                                allowanmce_arr1 = allowance.Split('\\');

                                if (allowanmce_arr1.GetUpperBound(0) > 0)
                                {

                                    for (m = 0; m < allowanmce_arr1.GetUpperBound(0); m++)
                                    {

                                    l2: alowancesplit = allowanmce_arr1[m];
                                        k = 0;
                                        p = 4;
                                        if (alowancesplit != "")
                                        {
                                            allowance2 = alowancesplit.Split(';');
                                            string[] splval = allowance2[2].Split('-');
                                            if (allowance2[1].Trim() == "Amount")  //modified by jeyaprakash on Sep 6th(Allowance not invokes)
                                            {
                                                da3 = splval[0];
                                            }
                                            else if (allowance2[1].Trim() == "Percent" || allowance2[1].Trim() == "Slab")
                                            {
                                                if (splval.Length == 2)
                                                {
                                                    da3 = splval[1];
                                                }
                                            }
                                            //da3 = allowance2[3];

                                            if (allowance2.GetUpperBound(0) > 0)
                                            {


                                            l3: if (p < fpsalarydemond.Sheets[0].ColumnCount - 1)
                                                {
                                                    string headval = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, p].Text;
                                                    for (int j = 0; j < allowance2.GetUpperBound(0); j++)
                                                    {
                                                        Double allov = 0;
                                                        if (headval == allowance2[j])
                                                        {
                                                            Double.TryParse(da3, out allov);
                                                            allov = Math.Round(allov, 0, MidpointRounding.AwayFromZero);
                                                            DblAllowTotal[k] = Convert.ToDouble(DblAllowTotal[k]) + allov;
                                                            DblNetAllowTotal = allov + Convert.ToDouble(DblNetAllowTotal);

                                                            m = m + 1;
                                                            p = p + 1;
                                                            k = k + 1;
                                                            goto l2;

                                                        }
                                                        else
                                                        {

                                                            p = p + 1;
                                                            k = k + 1;
                                                            goto l3;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                }
                                            }
                                        }
                                    }
                                    string[] deduction_arr1;

                                    string[] deduction2;

                                    k = 0;
                                    string deductionsplit;

                                    deduction_arr1 = deduction.Split('\\');

                                    if (deduction_arr1.GetUpperBound(0) > 0)
                                    {

                                        for (m = 0; m < deduction_arr1.GetUpperBound(0); m++)
                                        {
                                        l2: deductionsplit = deduction_arr1[m];
                                            col3 = col;
                                            k = 0;
                                            if (deductionsplit != "")
                                            {
                                                deduction2 = deductionsplit.Split(';');
                                                string[] splval = deduction2[2].Split('-');
                                                if (deduction2[1].Trim() == "Amount")  //modified by jeyaprakash on Sep 6th(Allowance not invokes)
                                                {
                                                    da3 = splval[0];
                                                }
                                                else if (deduction2[1].Trim() == "Percent" || deduction2[1].Trim() == "Slab")
                                                {
                                                    if (splval.Length == 2)
                                                    {
                                                        da3 = splval[1];
                                                    }
                                                }
                                                //da3 = deduction2[3];
                                                if (deduction2.GetUpperBound(0) > 0)
                                                {
                                                l3: if (col3 < fpsalarydemond.Sheets[0].ColumnCount - 1)
                                                    {
                                                        string headval1 = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, col3].Text;
                                                        for (int j = 0; j < deduction2.GetUpperBound(0); j++)
                                                        {
                                                            Double allov = 0;
                                                            if (headval1 == deduction2[j])
                                                            {
                                                                Double.TryParse(da3, out allov);
                                                                allov = Math.Round(allov, 0, MidpointRounding.AwayFromZero);
                                                                deductiontotal[k] = deductiontotal[k] + allov;
                                                                DblNetDedTotal = Convert.ToDouble(DblNetDedTotal) + allov;

                                                                m = m + 1;
                                                                col3 = col3 + 1;
                                                                k = k + 1;
                                                                goto l2;

                                                            }
                                                            else
                                                            {

                                                                col3 = col3 + 1;
                                                                k = k + 1;
                                                                goto l3;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                    }

                                                }
                                            }
                                        }
                                    }


                                }
                            }
                            else
                            {
                                btnprintmaster.Visible = false;
                                btnxl.Visible = false;
                                lblexcel.Visible = false;
                                txtxl.Visible = false;
                                lblnorec.Text = "No Records Found";
                                lblnorec.Visible = true;
                            }


                        }



                        int rowstr1 = fpsalarydemond.Sheets[0].RowCount++;
                        mname = Convert.ToString(fdate.ToString("MM").TrimStart('0'));// month5.ToString();
                        string month7 = getmonth(mname);
                        /////////////////////////////////////////////////////////////////////////////////////////
                        fpsalarydemond.Sheets[0].Cells[rowstr1, 0].Value = 1;
                        fpsalarydemond.Sheets[0].Cells[rowstr1, 1].Tag = Convert.ToString(fdate.ToString("MM").TrimStart('0'));//.ToString();

                        fpsalarydemond.Sheets[0].Cells[rowstr1, 1].Text = month7.ToString();
                        fpsalarydemond.Sheets[0].Cells[rowstr1, 2].HorizontalAlign = HorizontalAlign.Center;
                        fpsalarydemond.Sheets[0].Cells[rowstr1, 3].Text = basicpaytotal.ToString();
                        fpsalarydemond.Sheets[0].Cells[rowstr1, 2].Text = countstaff.ToString();
                        fpsalarydemond.Sheets[0].Cells[rowstr1, 2].Locked = true;

                        fpsalarydemond.Sheets[0].Cells[rowstr1, 3].HorizontalAlign = HorizontalAlign.Right;
                        fpsalarydemond.Sheets[0].Cells[rowstr1, 3].HorizontalAlign = HorizontalAlign.Right;
                        int g1 = 4;
                        string month;
                        // month = cblmonthfrom.SelectedItem.ToString();
                        for (int i = 0; i < getval; i++)
                        {
                            DblAllowTotal[i] = Math.Round(DblAllowTotal[i], 2);

                            fpsalarydemond.Sheets[0].Cells[rowstr1, g1].Text = DblAllowTotal[i].ToString();
                            fpsalarydemond.Sheets[0].Cells[rowstr1, g1].HorizontalAlign = HorizontalAlign.Right;
                            g1 = g1 + 1;

                        }
                        // fpsalary.Sheets[0].Cells[rowstr1, colheder - 1].Text = Convert.ToDouble(DblNetAllowTotal + basicpaytotal).ToString();
                        fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].HorizontalAlign = HorizontalAlign.Right;
                        for (int y = 0; y < getval2; y++)
                        {
                            deductiontotal[y] = Math.Round(deductiontotal[y], 2);
                            fpsalarydemond.Sheets[0].Cells[rowstr1, col2].Text = deductiontotal[y].ToString();
                            fpsalarydemond.Sheets[0].Cells[rowstr1, col2].HorizontalAlign = HorizontalAlign.Right;
                            col2 = col2 + 1;
                        }
                        col2 = 0;
                        col2 = col;
                        DblNetDedTotal = Math.Round(DblNetDedTotal, 2);
                        // fpsalary.Sheets[0].Cells[rowstr1, colcount1 - 2].Text = Convert.ToDouble(DblNetDedTotal).ToString();
                        fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].HorizontalAlign = HorizontalAlign.Right;
                        netpaytotal = (basicpaytotal + DblNetAllowTotal) - DblNetDedTotal;
                        netpaytotal = Math.Round(netpaytotal, 0, MidpointRounding.AwayFromZero);
                        //fpsalary.Sheets[0].Cells[rowstr1, colcount1 - 1].Text = Convert.ToDouble(netpaytotal).ToString();

                        earntotal = basicpaytotal + DblNetAllowTotal;
                        fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].Text = earntotal.ToString();
                        totaldeduction = DblNetDedTotal;
                        fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].Locked = true;
                        fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].Text = totaldeduction.ToString();
                        fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].Locked = true;

                        fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].Text = netpaytotal.ToString();
                        fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].Locked = true;



                        fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].HorizontalAlign = HorizontalAlign.Right;
                        basicpaytotal = 0;
                        DblNetDedTotal = 0;
                        DblNetAllowTotal = 0;
                        netpaytotal = 0;
                        fdate = fdate.AddMonths(1);
                    }

                    if (fpsalarydemond.Sheets[0].RowCount - 1 > 0)
                    {
                        fpsalarydemond.Sheets[0].RowCount++;

                        fpsalarydemond.Sheets[0].Rows[fpsalarydemond.Sheets[0].RowCount - 1].Font.Bold = true;
                        //  fpsalary.Sheets[0].SpanModel.Add(fpsalary.Sheets[0].RowCount - 1, 0, 1, 2);
                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 0].Locked = true;
                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Text = "Total";
                        for (int intColCtr = 2; intColCtr < fpsalarydemond.Sheets[0].ColumnCount; intColCtr++)
                        {
                            IntMTotal = 0;
                            for (int IntRowCtr = 0; IntRowCtr < fpsalarydemond.Sheets[0].RowCount - 1; IntRowCtr++)
                            {
                                IntMTemp = 0;
                                if (fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text != "-")
                                {
                                    if (fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text != "")
                                    {
                                        IntMTemp = Convert.ToDouble(fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text);
                                    }
                                    else
                                    {
                                        IntMTemp = 0;
                                    }
                                }
                                else
                                {
                                    IntMTemp = 0;
                                }
                                IntMTotal = IntMTemp + IntMTotal;
                                IntMTotal = Math.Round(IntMTotal, 2);
                            }
                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Text = IntMTotal > 0 ? IntMTotal + "" : "-";
                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].HorizontalAlign = HorizontalAlign.Right;
                            if (intColCtr == 2)
                            {
                                fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].HorizontalAlign = HorizontalAlign.Center;
                            }

                        }
                    }


                    Double totalRows = 0;
                    totalRows = Convert.ToInt32(fpsalarydemond.Sheets[0].RowCount);

                    if (totalRows >= 10)
                    {
                        fpsalarydemond.Sheets[0].PageSize = Convert.ToInt32(totalRows);



                    }
                    else if (totalRows == 0)
                    {

                        fpsalarydemond.Height = 300;
                    }
                    else
                    {
                        fpsalarydemond.Sheets[0].PageSize = Convert.ToInt32(totalRows);


                    }
                    Session["totalPages"] = (int)Math.Ceiling(totalRows / fpsalarydemond.Sheets[0].PageSize);
                }
            }
            Double he = 120;
            for (int f = 0; f < fpsalarydemond.Sheets[0].RowCount; f++)
            {
                he = he + fpsalarydemond.Sheets[0].Rows[f].Height;
            }
            if (fpsalarydemond.Sheets[0].RowCount == 0)
            {
                fpsalarydemond.Visible = false;
                btnprintmaster.Visible = false;
                btnxl.Visible = false;
                btnsal.Visible = false;
                lblexcel.Visible = false;
                txtxl.Visible = false;
                lblnorec.Text = "No Records Found";
                lblnorec.Visible = true;
            }
            for (int c = 0; c < fpsalarydemond.Sheets[0].ColumnCount; c++)
            {
                fpsalarydemond.Sheets[0].Columns[c].Locked = true;
            }
            he = Math.Round(he, 0, MidpointRounding.AwayFromZero);
            fpsalarydemond.Height = Convert.ToInt32(he);
            fpsalarydemond.CommandBar.Visible = false;
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }

    protected void cblledger_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    public string GetFunction(string Att_strqueryst)
    {

        string sqlstr;
        sqlstr = Att_strqueryst;
        mysql.Close();
        mysql.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, mysql);
        SqlDataReader drnew;
        SqlCommand cmd = new SqlCommand(sqlstr);
        cmd.Connection = mysql;
        drnew = cmd.ExecuteReader();
        drnew.Read();

        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "";
        }
    }





    protected void btnprintmaster_Click(object sender, EventArgs e)
    {

        Session["column_header_row_count"] = fpsalarydemond.Sheets[0].ColumnHeader.RowCount;

        string degreedetails = string.Empty;

        degreedetails = "Monthly Salary Statement@Year: " + cblbatchyear.SelectedItem.ToString() + "@Month: " + cblmonthfrom.SelectedItem.ToString() + " To " + cbotomonth.SelectedItem.ToString();

        string pagename = "cumulativesalary.aspx";

        Printcontrol.loadspreaddetails(fpsalarydemond, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtxl.Text;

            if (reportname.ToString().Trim() != "")
            {
                lblnorec.Text = "";
                lblnorec.Visible = false;

                d2.printexcelreport(fpsalarydemond, reportname);
            }
            else
            {
                lblnorec.Text = "Please Enter Your Report Name";
                lblnorec.Visible = true;
            }
            txtxl.Text = "";
            reportname = "";
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
            lblnorec.Visible = true;
        }
    }
    //*******************Added by senthil****************************
    protected void cbSelect_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbSelect.Checked == true)
            {
                lblstafnam.Visible = true;
                ddlstfnam.Visible = false;  //**//
                txtstfname.Visible = true;
                pnlstfname.Visible = true;
                staff();
                load_allowance();
                Chkdeduction.Checked = false;
                chkallowance.Checked = false;
            }
            else
            {
                load_allowance();
                lblstafnam.Visible = false;
                ddlstfnam.Visible = false;
                txtstfname.Visible = false;
                pnlstfname.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void staff()
    {
        try
        {
            ddlstfnam.Items.Clear();
            cblstfname.Items.Clear();
            string des = "";

            for (int i = 0; i < cbldepttype.Items.Count; i++)
            {

                if (cbldepttype.Items[i].Selected == true)
                {

                    if (des == "")
                    {
                        des = cbldepttype.Items[i].Value.ToString();
                    }
                    else
                    {
                        des = des + "'" + "," + "'" + cbldepttype.Items[i].Value.ToString();
                    }

                }
            }

            college_code = Session["collegecode"].ToString();

            string year = "";
            year = "Select distinct m.Staff_code,Staff_name from staffmaster m,stafftrans t where ";
            if (cbinculdeRelive.Checked)
            {
                year += " m.staff_code = t.staff_code  and t.latestrec = 1 and dept_code in ('" + des + "') and college_code='" + college_code + "' order by staff_name";
            }
            else
            {
                year += " resign=0 and settled=0 and m.staff_code = t.staff_code  and t.latestrec = 1 and dept_code in ('" + des + "') and college_code='" + college_code + "' order by staff_name";
            }
            ds = dac.select_method_wo_parameter(year, "text");
            {
                ddlstfnam.DataSource = ds;
                ddlstfnam.DataTextField = "Staff_name";
                ddlstfnam.DataValueField = "Staff_code";
                ddlstfnam.DataBind();

                cblstfname.DataSource = ds;
                cblstfname.DataTextField = "Staff_name";
                cblstfname.DataValueField = "Staff_code";
                cblstfname.DataBind();

                for (int my = 0; my < cblstfname.Items.Count; my++)
                {
                    cblstfname.Items[my].Selected = false;
                }
                txtstfname.Text = "---Select---";
                cbstfname.Checked = false;
            }
        }
        catch
        {

        }
    }
    //**************************Added by Senthil********************************
    protected void ddlstfnam_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_allowance();
        Chkdeduction.Checked = false;
        chkallowance.Checked = false;
        //txtdeduction.
        fpsalarydemond.Visible = false;
        btnprintmaster.Visible = false;
        btnxl.Visible = false;
        lblexcel.Visible = false;
        txtxl.Visible = false;
        btnsal.Visible = false;
    }

    protected void cbstfname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txtstfname.Text = "---Select---";
            if (cbstfname.Checked == true)
            {
                for (int ik = 0; ik < cblstfname.Items.Count; ik++)
                {
                    cblstfname.Items[ik].Selected = true;
                }
                txtstfname.Text = "Staff Name(" + Convert.ToString(cblstfname.Items.Count) + ")";
            }
            else
            {
                for (int ik = 0; ik < cblstfname.Items.Count; ik++)
                {
                    cblstfname.Items[ik].Selected = false;
                }
                txtstfname.Text = "---Select---";
            }
            loadmulallowance();
        }
        catch { }
    }

    protected void cblstfname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtstfname.Text = "---Select---";
            cbstfname.Checked = false;
            int checkcount = 0;
            for (int jk = 0; jk < cblstfname.Items.Count; jk++)
            {
                if (cblstfname.Items[jk].Selected == true)
                {
                    checkcount++;
                }
            }
            if (checkcount > 0)
            {
                txtstfname.Text = "Staff Name(" + checkcount + ")";
                if (checkcount == cblstfname.Items.Count)
                    cbstfname.Checked = true;
                else
                    cbstfname.Checked = false;
            }
            loadmulallowance();
        }
        catch { }
    }

    protected void btnsal_Click(object sender, EventArgs e)
    {
        try
        {
            lblnorec.Visible = false;
            Hashtable hsmon = new Hashtable();
            Hashtable hsgetmon = new Hashtable();
            hsmon.Clear();
            hsgetmon.Clear();
            hsmon.Add("January", "Jan");
            hsmon.Add("February", "Feb");
            hsmon.Add("March", "Mar");
            hsmon.Add("April", "Apr");
            hsmon.Add("May", "May");
            hsmon.Add("June", "June");
            hsmon.Add("July", "July");
            hsmon.Add("August", "Aug");
            hsmon.Add("September", "Sep");
            hsmon.Add("October", "Oct");
            hsmon.Add("November", "Nov");
            hsmon.Add("December", "Dec");

            hsgetmon.Add("January", "1");
            hsgetmon.Add("February", "2");
            hsgetmon.Add("March", "3");
            hsgetmon.Add("April", "4");
            hsgetmon.Add("May", "5");
            hsgetmon.Add("June", "6");
            hsgetmon.Add("July", "7");
            hsgetmon.Add("August", "8");
            hsgetmon.Add("September", "9");
            hsgetmon.Add("October", "10");
            hsgetmon.Add("November", "11");
            hsgetmon.Add("December", "12");

            Font Fontco18 = new Font("Book Antiqua", 18, FontStyle.Bold);
            Font Fontco19 = new Font("Book Antiqua", 15, FontStyle.Bold);
            Font Fontco19new = new Font("Book Antiqua", 14, FontStyle.Regular);
            Font Fontco12 = new Font("Book Antiqua", 12, FontStyle.Regular);
            Font Fontco12new = new Font("Book Antiqua", 12, FontStyle.Bold);
            Font Fontco12a = new Font("Book Antiqua", 12, FontStyle.Regular);
            Font Fontco10 = new Font("Book Antiqua", 10, FontStyle.Bold);
            Font Fontco10a = new Font("Book Antiqua", 10, FontStyle.Regular);
            Font Fontco14 = new Font("Book Antiqua", 14, FontStyle.Bold);
            Font Fontco14a = new Font("Book Antiqua", 14, FontStyle.Regular);
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.Letter_8_5x11);
            Gios.Pdf.PdfDocument mydoc1 = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
            Gios.Pdf.PdfPage mypdfpage;

            ArrayList arrtb1row = new ArrayList();
            ArrayList arrtb2row = new ArrayList();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataSet dsAllow = new DataSet();
            DAccess2 alldectsql_da = new DAccess2();
            string allowanmce = "";
            string detection = "";
            string alldectsql = "Select * from incentives_master where college_code=" + Session["collegecode"] + "";
            ds.Clear();
            DataSet ds9 = new DataSet();
            ds = alldectsql_da.select_method_wo_parameter(alldectsql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                allowanmce = ds.Tables[0].Rows[0]["allowances"].ToString();
                detection = ds.Tables[0].Rows[0]["deductions"].ToString();
            }

            string year = cblbatchyear.SelectedItem.Text;
            string fromont = Convert.ToString(hsmon[cblmonthfrom.SelectedItem.Text]);
            string tomont = Convert.ToString(hsmon[cbotomonth.SelectedItem.Text]);
            int rowcount = 0;
            int moncount = 0;
            int newmoncount = 0;
            DateTime dsfrm = new DateTime();
            DateTime dsto = new DateTime();
            DateTime dsmondt = new DateTime();
            int ro = 0;
            int ro1 = 0;
            int ro2 = 0;
            int ro3 = 0;
            int ro4 = 0;
            int ro5 = 0;
            int ro6 = 0;
            int ro7 = 0;
            int ro8 = 0;
            if (cblmonthfrom.SelectedItem.Text.Trim() != "All")
            {
                Int32.TryParse(Convert.ToString(hsgetmon[cbotomonth.SelectedItem.Text]), out moncount);
                Int32.TryParse(Convert.ToString(hsgetmon[cblmonthfrom.SelectedItem.Text]), out newmoncount);
                if (moncount == 0 || newmoncount == 0)
                {
                    lblnorec.Text = "Please Select the Valid Month!";
                    lblnorec.Visible = true;
                    fpsalarydemond.Visible = false;
                    lblexcel.Visible = false;
                    txtxl.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    btnsal.Visible = false;
                    Printcontrol.Visible = false;
                    return;
                }
                else if (moncount < newmoncount)
                {
                    lblnorec.Text = "Please Select the Valid Month!";
                    lblnorec.Visible = true;
                    fpsalarydemond.Visible = false;
                    lblexcel.Visible = false;
                    txtxl.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    btnsal.Visible = false;
                    Printcontrol.Visible = false;
                    return;
                }
                dsfrm = Convert.ToDateTime(Convert.ToString(newmoncount) + "/" + "01" + "/" + Convert.ToString(cblbatchyear.SelectedItem.Text));
                dsto = Convert.ToDateTime(Convert.ToString(moncount) + "/" + "01" + "/" + Convert.ToString(cblbatchyear.SelectedItem.Text));
                //dsmondt = Convert.ToDateTime(dsto - dsfrm);
                moncount = (dsto.Month - dsfrm.Month) + 2;
            }
            else
                moncount = 13;

            if (txtstfname.Text.Trim() != "---Select---")
            {
                for (int my = 0; my < cblstfname.Items.Count; my++)
                {
                    if (cblstfname.Items[my].Selected == true)
                    {
                        string stfname = cblstfname.Items[my].Text;
                        string stfcode = cblstfname.Items[my].Value;
                        string desig = GetFunction("select d.desig_name from stafftrans st,staffmaster sm,desig_master d where st.staff_code=sm.staff_code and st.desig_code=d.desig_code and sm.college_code=d.collegeCode and latestrec='1' and sm.staff_name='" + stfname + "'");
                        //ds9 = alldectsql_da.select_method_wo_parameter(stfdesign, "Text");
                        //string desig = "";
                        //if (ds9.Tables[0].Rows.Count > 0)
                        //{
                        //    desig = ds9.Tables[0].Rows[0]["desig_name"].ToString();
                        //}
                        string allowance = "";
                        string myAllow = "";
                        if (cblmonthfrom.SelectedItem.Text == "All")
                            allowance = "select allowances,deductions from monthlypay where staff_code='" + stfcode + "' and PayMonth between '1' and '12' and PayYear='" + year + "' and College_code=" + Session["collegecode"] + "";
                        else
                            allowance = "select allowances,deductions from monthlypay where staff_code='" + stfcode + "' and PayMonth between '" + cblmonthfrom.SelectedItem.Value + "' and '" + cbotomonth.SelectedItem.Value + "' and PayYear='" + year + "' and College_code=" + Session["collegecode"] + "";

                        dsAllow.Clear();
                        dsAllow = d2.select_method_wo_parameter(allowance, "Text");

                        string value = "";
                        int seatcount = 0;

                        fpsalarydemond.SaveChanges();
                        dt1.Columns.Clear();
                        dt1.Rows.Clear();
                        dt1.Clear();
                        dt2.Columns.Clear();
                        dt2.Rows.Clear();
                        dt2.Clear();
                        DataRow dtdr1 = null;
                        DataRow dtdr2 = null;

                        int tb1rowcount = 3;
                        int tb2rowcount = 0;

                        arrtb1row.Clear();
                        arrtb2row.Clear();
                        arrtb1row.Add(" ");
                        arrtb1row.Add("EARNINGS");
                        arrtb1row.Add("Basic Pay");

                        arrtb2row.Add(" ");
                        arrtb2row.Add("DEDUCTIONS");
                        myAllow = "";
                        if (dsAllow.Tables.Count > 0 && dsAllow.Tables[0].Rows.Count > 0)
                        {
                            for (int l = 0; l < dsAllow.Tables[0].Rows.Count; l++)
                            {
                                myAllow = Convert.ToString(dsAllow.Tables[0].Rows[l]["allowances"]);
                                if (!String.IsNullOrEmpty(myAllow))
                                {
                                    string[] myallspl = myAllow.Split('\\');
                                    if (myallspl.Length > 0)
                                    {
                                        for (int m = 0; m < myallspl.Length; m++)
                                        {
                                            string[] allspl = myallspl[m].Split(';');
                                            if (allspl.Length > 0)
                                            {
                                                if (!String.IsNullOrEmpty(allspl[0]))
                                                {
                                                    if (!arrtb1row.Contains(allspl[0]))
                                                    {
                                                        tb1rowcount++;
                                                        arrtb1row.Add(allspl[0]);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        myAllow = "";
                        if (dsAllow.Tables.Count > 0 && dsAllow.Tables[0].Rows.Count > 0)
                        {
                            for (int l = 0; l < dsAllow.Tables[0].Rows.Count; l++)
                            {
                                myAllow = Convert.ToString(dsAllow.Tables[0].Rows[l]["deductions"]);
                                if (!String.IsNullOrEmpty(myAllow))
                                {
                                    string[] myallspl = myAllow.Split('\\');
                                    if (myallspl.Length > 0)
                                    {
                                        for (int m = 0; m < myallspl.Length; m++)
                                        {
                                            string[] allspl = myallspl[m].Split(';');
                                            if (allspl.Length > 0)
                                            {
                                                if (!String.IsNullOrEmpty(allspl[0]))
                                                {
                                                    if (!arrtb2row.Contains(allspl[0]))
                                                    {
                                                        tb2rowcount++;
                                                        tb1rowcount++;
                                                        arrtb2row.Add(allspl[0]);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //for (int i = 0; i < cblallowance.Items.Count; i++)
                        //{
                        //    if (cblallowance.Items[i].Selected == true)
                        //    {
                        //        tb1rowcount++;
                        //        if (ds.Tables[0].Rows.Count > 0)
                        //        {
                        //            string[] allowanmce_arr;
                        //            allowanmce_arr = allowanmce.Split(';');

                        //            for (int k = 0; k < allowanmce_arr.GetUpperBound(0); k++)
                        //            {
                        //                string all2 = allowanmce_arr[k];
                        //                string[] splitallo3 = all2.Split('\\');
                        //                if (splitallo3.GetUpperBound(0) > 1)
                        //                {
                        //                    all2 = splitallo3[0];
                        //                }
                        //                string selectallo = cblallowance.Items[i].ToString().Trim().ToLower();

                        //                if (selectallo == all2.Trim().ToLower())
                        //                {
                        //                    arrtb1row.Add(splitallo3[0]);
                        //                }
                        //            }
                        //        }
                        //    }
                        //}

                        //for (int i = 0; i < cbldeduction.Items.Count; i++)
                        //{
                        //    if (cbldeduction.Items[i].Selected == true)
                        //    {
                        //        tb2rowcount++;

                        //        tb1rowcount++;
                        //        if (ds.Tables[0].Rows.Count > 0)
                        //        {
                        //            string[] deduction_arr;
                        //            deduction_arr = detection.Split(';');

                        //            for (int k = 0; k < deduction_arr.GetUpperBound(0); k++)
                        //            {
                        //                string all2 = deduction_arr[k];
                        //                string[] splitallo3 = all2.Split('\\');
                        //                if (splitallo3.GetUpperBound(0) >= 0)
                        //                {
                        //                    all2 = splitallo3[0];
                        //                }
                        //                string selectallo = cbldeduction.Items[i].ToString();
                        //                if (selectallo == all2)
                        //                {
                        //                    arrtb2row.Add(splitallo3[0]);
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        arrtb1row.Add(" ");
                        arrtb1row.Add("GROSS");
                        arrtb2row.Add(" ");
                        arrtb2row.Add("TOTAL");

                        dt1.Columns.Add("");
                        dt1.Columns.Add("");
                        for (int i = rowcount; i < rowcount + moncount; i++)
                        {
                            string mont = fpsalarydemond.Sheets[0].Cells[i, 1].Text.ToString();
                            string sub = "";
                            if (mont.Trim().ToUpper() != "TOTAL")
                            {
                                sub = mont.Substring(0, 3);
                            }
                            else
                            {
                                sub = mont;
                            }
                            dt1.Columns.Add(sub);
                        }
                        dtdr1 = dt1.NewRow();
                        dtdr1[0] = "";
                        dtdr1[1] = "";

                        ro = 0;
                        for (int i = rowcount; i < rowcount + moncount; i++)
                        {
                            string mont = fpsalarydemond.Sheets[0].Cells[i, 1].Text.ToString();
                            string sub = "";
                            string sub1 = "";
                            if (mont.Trim().ToUpper() != "TOTAL")
                            {
                                sub = mont.Substring(0, 3);
                                string year1 = cblbatchyear.SelectedItem.Text;
                                //sub1 = year1.Substring(2, 2);
                            }
                            else
                            {
                                sub = mont;
                                sub1 = "";
                            }
                            dtdr1[ro + 2] = sub + sub1;
                            ro++;
                        }

                        dt1.Rows.Add(dtdr1);

                        for (int i = 0; i < arrtb1row.Count; i++)
                        {
                            dtdr1 = dt1.NewRow();
                            dtdr1[0] = arrtb1row[i].ToString();
                            if (i >= 2 && i != arrtb1row.Count - 2)
                            {
                                dtdr1[1] = ":";
                            }
                            dt1.Rows.Add(dtdr1);
                        }
                        //dt1.Rows[2][4] = "";


                        string fp = fpsalarydemond.Sheets[0].RowCount.ToString();
                        string fp1 = fpsalarydemond.Sheets[0].ColumnCount.ToString();
                        for (int i = 3; i < arrtb1row.Count - 2; i++)
                        {
                            for (int k = 0; k < fpsalarydemond.Sheets[0].ColumnCount; k++)
                            {
                                string sdr = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, k].Text.ToString();
                                if (arrtb1row[i].ToString().Trim().ToLower() == fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, k].Text.ToString().ToLower())
                                {
                                    ro1 = 0;
                                    for (int j = rowcount; j < rowcount + moncount; j++)
                                    {
                                        string valueess = fpsalarydemond.Sheets[0].Cells[j, k].Text.ToString();
                                        dt1.Rows[i + 1][ro1 + 2] = valueess;
                                        ro1++;
                                    }
                                }
                            }
                        }
                        ro2 = 0;
                        for (int i = rowcount; i < rowcount + moncount; i++)
                        {
                            string valueess = fpsalarydemond.Sheets[0].Cells[i, 3].Text.ToString();
                            dt1.Rows[3][ro2 + 2] = valueess;
                            ro2++;
                        }

                        for (int i = 0; i < fpsalarydemond.Sheets[0].ColumnCount; i++)
                        {
                            string gross = "Earned Salary";
                            gross = gross.Trim().ToLower();
                            if (gross == fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, i].Text.ToString().ToLower())
                            {
                                ro3 = 0;
                                for (j = rowcount; j < rowcount + moncount; j++)
                                {
                                    dt1.Rows[arrtb1row.Count][ro3 + 2] = fpsalarydemond.Sheets[0].Cells[j, i].Text.ToString();
                                    ro3++;
                                }

                            }

                        }

                        for (int i = 0; i < fpsalarydemond.Sheets[0].ColumnCount; i++)
                        {
                            string valueess = "-----------";
                            ro4 = 0;
                            for (j = rowcount; j < rowcount + moncount; j++)
                            {
                                dt1.Rows[arrtb1row.Count - 1][ro4 + 2] = valueess;
                                ro4++;
                            }
                        }

                        dt2.Columns.Add("");
                        dt2.Columns.Add("");
                        for (int i = rowcount; i < rowcount + moncount; i++)
                        {
                            dt2.Columns.Add(fpsalarydemond.Sheets[0].Cells[i, 1].Text.ToString());

                        }
                        dtdr2 = dt2.NewRow();
                        dtdr2[0] = "";
                        dtdr2[1] = "";
                        dtdr2[2] = "";

                        dt2.Rows.Add(dtdr2);

                        for (int i = 0; i < arrtb2row.Count; i++)
                        {
                            dtdr2 = dt2.NewRow();
                            dtdr2[0] = arrtb2row[i].ToString();
                            if (i >= 2 && i != arrtb2row.Count - 1)
                            {
                                dtdr2[1] = ":";
                            }
                            dt2.Rows.Add(dtdr2);
                        }
                        string fp2 = fpsalarydemond.Sheets[0].RowCount.ToString();
                        string fp3 = fpsalarydemond.Sheets[0].ColumnCount.ToString();
                        for (int i = 2; i < arrtb2row.Count - 1; i++)
                        {

                            for (int k = 0; k < fpsalarydemond.Sheets[0].ColumnCount; k++)
                            {
                                string sdr = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, k].Text.ToString();
                                if (arrtb2row[i].ToString().Trim().ToLower() == fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, k].Text.ToString().ToLower())
                                {
                                    ro5 = 0;
                                    for (int j = rowcount; j < rowcount + moncount; j++)
                                    {
                                        string valueess = fpsalarydemond.Sheets[0].Cells[j, k].Text.ToString();
                                        dt2.Rows[i + 1][ro5 + 2] = valueess;
                                        ro5++;
                                    }
                                }
                            }
                        }

                        for (int i = 0; i < fpsalarydemond.Sheets[0].ColumnCount; i++)
                        {
                            string gross = "Total Deduction";
                            gross = gross.Trim().ToLower();
                            if (gross == fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, i].Text.ToString().ToLower())
                            {
                                ro6 = 0;
                                for (j = rowcount; j < rowcount + moncount; j++)
                                {
                                    dt2.Rows[arrtb2row.Count][ro6 + 2] = fpsalarydemond.Sheets[0].Cells[j, i].Text.ToString();
                                    ro6++;
                                }
                            }
                        }

                        for (int i = 0; i < fpsalarydemond.Sheets[0].ColumnCount; i++)
                        {
                            string valueess = "-----------";
                            ro7 = 0;
                            for (j = rowcount; j < rowcount + moncount; j++)
                            {
                                dt2.Rows[arrtb2row.Count - 1][ro7 + 2] = valueess;
                                ro7++;
                            }
                        }
                        for (int i = 0; i < cbldepttype.Items.Count; i++)
                        {
                            if (cbldepttype.Items[i].Selected == true)
                            {
                                //staff();
                                lblstafnam.Visible = true;
                                ddlstfnam.Visible = false;  //**//
                                value = cbldepttype.Items[i].Text;
                                seatcount = seatcount + 1;
                                tbseattype.Text = "Department(" + seatcount.ToString() + ")";
                            }

                        }

                        value = GetFunction("select h.dept_name from stafftrans st,staffmaster sm,hrdept_master h where st.staff_code=sm.staff_code and st.dept_code=h.dept_code and sm.college_code=h.college_code and latestrec='1' and sm.staff_name='" + stfname + "'");

                        mypdfpage = mydoc.NewPage();
                        PdfTextArea ptsp = new PdfTextArea(Fontco18, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 170, 30, 300, 50), System.Drawing.ContentAlignment.MiddleCenter, "SALARY CERTIFICATE");
                        mypdfpage.Add(ptsp);
                        if (cblmonthfrom.SelectedItem.Text == "All")
                        {
                            //ptsp = new PdfTextArea(Fontco19new, System.Drawing.Color.Black,
                            //                                                        new PdfArea(mydoc, 40, 80, 550, 150), System.Drawing.ContentAlignment.TopLeft, "Following are the pay particulars drawn by " + stfname + "," + "" + desig + ", Department of " + value + " of our College for the period from January" + year + " to December" + year + ":");
                            //mypdfpage.Add(ptsp);

                            ptsp = new PdfTextArea(Fontco12a, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydoc, 40, 80, mydoc.PageWidth, 50), System.Drawing.ContentAlignment.TopLeft, "Following are the pay particulars drawn by " + stfname + "," + "" + desig + ",");
                            mypdfpage.Add(ptsp);

                            double newh = ptsp.PdfArea.Height;
                            int dd = Convert.ToInt16(newh) + 60;
                            ptsp = new PdfTextArea(Fontco12a, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydoc, 40, dd, 550, 50), System.Drawing.ContentAlignment.TopLeft, "Department of " + value + " of our College for the period from Jan " + year + " to Dec " + year + ":");
                            mypdfpage.Add(ptsp);
                        }
                        else
                        {
                            //ptsp = new PdfTextArea(Fontco19new, System.Drawing.Color.Black,
                            //                                                        new PdfArea(mydoc, 40, 80, 550, 150), System.Drawing.ContentAlignment.TopLeft, "Following are the pay particulars drawn by " + stfname + "," + "" + desig + ", Department of " + value + " of our College for the period from " + fromont + " " + year + " to " + tomont + " " + year + ":");
                            //mypdfpage.Add(ptsp);

                            ptsp = new PdfTextArea(Fontco12a, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydoc, 40, 80, 550, 50), System.Drawing.ContentAlignment.TopLeft, "Following are the pay particulars drawn by " + stfname + "," + "" + desig + ",");
                            mypdfpage.Add(ptsp);
                            double newh = ptsp.PdfArea.Height;
                            int dd = Convert.ToInt16(newh) + 60;
                            ptsp = new PdfTextArea(Fontco12a, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydoc, 40, dd, 550, 50), System.Drawing.ContentAlignment.TopLeft, "Department of " + value + " of our College for the period from " + fromont + " " + year + " to " + tomont + " " + year + ":");
                            mypdfpage.Add(ptsp);
                        }


                        Gios.Pdf.PdfTable table1forpage1 = mydoc.NewTable(Fontco12, dt1.Rows.Count, dt1.Columns.Count, 1);

                        table1forpage1.VisibleHeaders = false;
                        table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                        int y = 170;

                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            table1forpage1.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage1.Rows[0].SetFont(Fontco12new);
                            table1forpage1.Rows[2].SetFont(Fontco18);
                            if (i == dt1.Rows.Count - 1)
                            {
                                table1forpage1.Rows[i].SetFont(Fontco12new);
                            }
                            table1forpage1.Rows[2].ToString().ToUpperInvariant();
                            for (int j = 0; j < dt1.Columns.Count; j++)
                            {
                                table1forpage1.Cell(i, j).SetContent(dt1.Rows[i][j].ToString());
                                if (j >= 2)
                                {
                                    table1forpage1.Columns[j].SetContentAlignment(ContentAlignment.MiddleRight);
                                    table1forpage1.Columns[j].SetWidth(10);
                                }
                            }
                        }
                        Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 40, y, 500, 500));

                        mypdfpage.Add(newpdftabpage2);


                        Gios.Pdf.PdfTable table1forpage2 = mydoc.NewTable(Fontco12, dt2.Rows.Count + 3, dt2.Columns.Count, 1);

                        Gios.Pdf.PdfTable table1forpage3 = mydoc.NewTable(Fontco12, dt2.Rows.Count, dt2.Columns.Count, 1);

                        table1forpage1.VisibleHeaders = false;
                        table1forpage1.SetBorders(Color.Black, 1, BorderType.None);


                        for (int i = 0; i < 3; i++)
                        {
                            dtdr2 = dt2.NewRow();
                            dtdr2[0] = "     ";
                            if (i >= 2)
                            {
                                dtdr2[0] = "Net";
                            }
                            dt2.Rows.Add(dtdr2);
                        }
                        for (int i = 0; i < fpsalarydemond.Sheets[0].ColumnCount; i++)
                        {
                            string gross = "Net Pay";
                            gross = gross.Trim().ToLower();
                            //string valueess = fpsalarydemond.Sheets[0].Cells[j, arrtb1row.Count].Text.ToString();
                            if (gross == fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, i].Text.ToString().ToLower())
                            {
                                ro8 = 0;
                                for (j = rowcount; j < rowcount + moncount; j++)
                                {
                                    dt2.Rows[arrtb2row.Count + 3][ro8 + 2] = fpsalarydemond.Sheets[0].Cells[j, i].Text.ToString();
                                    ro8++;
                                }
                            }
                        }
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            table1forpage2.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Rows[2].SetFont(Fontco18);
                            if (i == dt2.Rows.Count - 1 || i == dt2.Rows.Count - 4)
                            {
                                table1forpage2.Rows[i].SetFont(Fontco12new);
                            }

                            for (int j = 0; j < dt2.Columns.Count; j++)
                            {
                                table1forpage2.Cell(i, j).SetContent(dt2.Rows[i][j].ToString());

                                if (j >= 2)
                                {
                                    table1forpage2.Columns[j].SetContentAlignment(ContentAlignment.MiddleRight);
                                    table1forpage2.Columns[j].SetWidth(10);
                                }
                            }
                        }

                        Gios.Pdf.PdfTablePage newpdftabpage3 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 40, y + 180, 500, 500));

                        ptsp = new PdfTextArea(Fontco19, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydoc, 40, 680, 550, 100), System.Drawing.ContentAlignment.MiddleRight, "BURSAR");
                        mypdfpage.Add(ptsp);


                        ptsp = new PdfTextArea(Fontco19, System.Drawing.Color.Black,
                                                            new PdfArea(mydoc, 40, 680, 550, 100), System.Drawing.ContentAlignment.MiddleLeft, DateTime.Now.ToString("dd-MMM-yyyy"));
                        mypdfpage.Add(ptsp);

                        mypdfpage.Add(newpdftabpage3);

                        mypdfpage.SaveToDocument();
                        rowcount = rowcount + moncount;
                    }
                }
            }

            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "Salary Certificate.pdf";


                mydoc.SaveToFile(szPath + szFile);

                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }


        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
            d2.sendErrorMail(ex, Convert.ToString(Session["collegecode"]), "MonthlyCummulativeSalary.aspx");
            lblnorec.Visible = true;
        }
    }
    //protected void txtdeduction(object sender, EventArgs e)
    //{
    //    int seatcount=0;
    //    if (seatcount == cbldeduction.Items.Count)
    //    {
    //        txtdeduction.Text = "Deduction(" + seatcount.ToString() + ")";
    //        Chkdeduction.Checked = true;
    //    }
    //    else if (seatcount == 0)
    //    {
    //        txtdeduction.Text = "--Select--";
    //    }
    //    else
    //    {
    //        txtdeduction.Text = "Deduction(" + seatcount.ToString() + ")";
    //    }
    //}

    //*******************************Ended by Senthil 16.04.2015*********************************

    protected void ddlToYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        fpsalarydemond.Visible = false;
        btnprintmaster.Visible = false;
        btnxl.Visible = false;
        lblexcel.Visible = false;
        txtxl.Visible = false;
        btnsal.Visible = false;
    }
}


//----------------Last Modified By Jeyaprakash on Nov 23rd,2016-------------------------//
//----------------Add Multiple Staff (Change Drop Down to CheckBox List)---------------//
//----------------Add Salary Certificate for the staff whose selected------------------//