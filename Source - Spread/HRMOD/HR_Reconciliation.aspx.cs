using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Text;
using Gios.Pdf;
using System.Collections;
public partial class HR_Reconciliation : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet ds5 = new DataSet();
    DataSet ds6 = new DataSet();
    DataSet ds7 = new DataSet();
    DataSet ds11 = new DataSet();
    Hashtable hast = new Hashtable();
    DAccess2 d2 = new DAccess2();
    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "";
    string college_code = "";
    string d = "";
    Boolean cellclick = false;
    ReuasableMethods rs = new ReuasableMethods();
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
            //    Rbtformat1.Checked = true;
            //    fpspread.Visible = false;
            //    return;
            //}
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
                Rbtformat1.Checked = true;
                fpspread.Visible = false;
                return;
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
            college_code = Session["collegecode"].ToString();
            if (!IsPostBack)
            {
                Rbtformat1.Checked = true;
                Rbtformat2.Checked = false;
                btngo.Visible = false;
                Button1.Visible = true;
                ddlfrom.Enabled = true;
                ddlfromyear.Enabled = true;
                ddlto.Enabled = true;
                ddltoyear.Enabled = true;
                ddlcollege.Enabled = false;
                txtdept.Enabled = false;
                txtdesign.Enabled = false;
                ddlmonth.Enabled = false;
                ddlyear.Enabled = false;
                txt_Category.Enabled = false;
                bindcollege();
                binddepartment();
                binddesign();
                bindcategory();
                bindstaffcategory();
                bindMonthandYear();
                clear();
                fpspread.Visible = false;
                ddlfrom.Items.Clear();
                string str = "select PayMonth,PayMonthNum from HrPayMonths where College_Code='" + college_code + "' and SelStatus='1'";
                ds = da.select_method_wo_parameter(str, "Text");
                ddlfrom.DataSource = ds;
                ddlfrom.DataTextField = "PayMonth";
                ddlfrom.DataValueField = "PayMonthNum";
                ddlfrom.DataBind();
                ddlfrom.Items.Insert(0, "---Select---");
                ddlto.Items.Insert(0, "---Select---");
                year(d);
                year1(d);
                steam();
                stafftype();
            }
            lblnorec.Text = "";
            lblnorec.Visible = false;
        }
        catch (Exception ex) { }
    }
    protected void ddlfrom_selectchange(object sender, EventArgs e)
    {
        try
        {
            gridview1.Visible = false;
            btngen.Visible = false;
            ddlto.Items.Clear();
            string str = "select PayMonth,PayMonthNum,From_Date from HrPayMonths where College_Code='" + college_code + "' and SelStatus='1'";
            ds = da.select_method_wo_parameter(str, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string mon = ds.Tables[0].Rows[i]["PayMonth"].ToString();
                    if (ddlfrom.SelectedItem.Text.ToString() == mon)
                    {
                        string date = Convert.ToString(ddlfrom.SelectedItem.Value);
                        for (int j = i; j < ds.Tables[0].Rows.Count; j++)
                        {
                            ddlto.Items.Insert(count, new ListItem(ds.Tables[0].Rows[j]["PayMonth"].ToString(), ds.Tables[0].Rows[j]["PayMonthNum"].ToString()));
                            count++;
                        }
                        year(date);
                    }
                }
                ddlto.Items.Insert(0, "---Select---");
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    public void year(string date)
    {
        try
        {
            ds11.Clear();
            string year = "";
            if (date.Trim() == "")
            {
                year = "select distinct year(From_Date) as year from HrPayMonths  where College_Code='" + college_code + "' and SelStatus='1' order by year asc";
            }
            else
            {
                year = "select distinct year(From_Date) as year from HrPayMonths  where College_Code='" + college_code + "' and PayMonthNum =" + date + " and SelStatus='1' order by year asc";
            }
            ds11 = da.select_method_wo_parameter(year, "text");
            if (ds11.Tables[0].Rows.Count > 0)
            {
                ddlfromyear.DataSource = ds11;
                ddlfromyear.DataTextField = "year";
                ddlfromyear.DataValueField = "year";
                ddlfromyear.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    public void year1(string date)
    {
        try
        {
            ds11.Clear();
            string year = "";
            if (date.Trim() == "")
            {
                year = "select distinct year(From_Date) as year from HrPayMonths  where College_Code='" + college_code + "' and SelStatus='1' order by year asc";
            }
            else
            {
                year = "select distinct year(From_Date) as year from HrPayMonths  where College_Code='" + college_code + "' and PayMonthNum =" + date + " and SelStatus='1' order by year asc";
            }
            ds11 = da.select_method_wo_parameter(year, "text");
            if (ds11.Tables[0].Rows.Count > 0)
            {
                ddltoyear.DataSource = ds11;
                ddltoyear.DataTextField = "year";
                ddltoyear.DataValueField = "year";
                ddltoyear.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    public void steam()
    {
        string year = "select distinct ISNULL(stream,'') as Stream from staffmaster where College_Code='" + college_code + "' ";
        ds11 = da.select_method_wo_parameter(year, "text");
        if (ds11.Tables[0].Rows.Count > 0)
        {
            ddlsteam.DataSource = ds11;
            ddlsteam.DataTextField = "Stream";
            ddlsteam.DataValueField = "Stream";
            ddlsteam.DataBind();
            ddlsteam.Items.Add("All");
            ddlsteam.SelectedIndex = ddlsteam.Items.Count - 1;
        }
    }
    public void stafftype()
    {
        try
        {
            string year = "select distinct stftype from stafftrans";
            ds11 = da.select_method_wo_parameter(year, "text");
            if (ds11.Tables[0].Rows.Count > 0)
            {
                ddlstftype.DataSource = ds11;
                ddlstftype.DataTextField = "stftype";
                ddlstftype.DataValueField = "stftype";
                ddlstftype.DataBind();
                ddlstftype.Items.Add("All");
                ddlstftype.SelectedIndex = ddlstftype.Items.Count - 1;
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void ddlfromyear_selectchange(object sender, EventArgs e)
    {
        try
        {
            gridview1.Visible = false;
            btngen.Visible = false;
            ddltoyear.Items.Clear();
            string str = "select distinct year(To_Date) as year from HrPayMonths  where College_Code='" + college_code + "' and SelStatus='1' order by year asc";
            ds = da.select_method_wo_parameter(str, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var mon = ds.Tables[0].Rows[i]["year"].ToString();
                    if (ddlfromyear.SelectedItem.Text.ToString() == mon)
                    {
                        for (int j = i; j < ds.Tables[0].Rows.Count; j++)
                        {
                            ddltoyear.Items.Add(ds.Tables[0].Rows[j]["year"].ToString());
                        }
                        ddltoyear.Items.Insert(0, "Select");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void ddlto_selectchange(object sender, EventArgs e)
    {
        try
        {
            year1(ddlto.SelectedItem.Value);
            gridview1.Visible = false;
            btngen.Visible = false;
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void ddlsteam_selectchange(object sender, EventArgs e)
    {
        try
        {
            gridview1.Visible = false;
            btngen.Visible = false;
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void ddlstftype_selectchange(object sender, EventArgs e)
    {
        try
        {
            gridview1.Visible = false;
            btngen.Visible = false;
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            btngen.Visible = true;
            if (Rbtformat1.Checked == true)
            {
                fpspread.Visible = false;
                Label1.Visible = false;
                gridview1.Visible = true;
            }
            else if (Rbtformat2.Checked == true)
            {
                fpspread.Visible = true;
                gridview1.Visible = false;
            }

            string staffcategory = string.Empty;


            for (int i = 0; i < chklst_f1_category.Items.Count; i++)
            {
                if (chklst_f1_category.Items[i].Selected)
                {
                    if (staffcategory == "")
                    {
                        staffcategory = "" + chklst_f1_category.Items[i].Value.ToString();
                    }
                    else
                    {
                        staffcategory += "','" + chklst_f1_category.Items[i].Value.ToString() + "";
                    }
                }
            }

            DataTable dt = new DataTable();
            int count = 0;
            ArrayList addyear = new ArrayList();
            ArrayList addvalue = new ArrayList();
            ArrayList monthadd = new ArrayList();
            ArrayList monthnumber = new ArrayList();
            dt.Columns.Add("Months", typeof(string));
            dt.Columns.Add("Overall Salary", typeof(string));
            dt.Columns.Add("Add", typeof(string));
            dt.Columns.Add("Less", typeof(string));
            dt.Columns.Add("Number", typeof(string));
            dt.Columns.Add("monthnum", typeof(string));
            int from_month = Convert.ToInt32(ddlfrom.SelectedItem.Value);
            int to_month = Convert.ToInt32(ddlto.SelectedItem.Value);
            int year = Convert.ToInt32(ddlfromyear.SelectedItem.Text);
            int yearto = Convert.ToInt32(ddltoyear.SelectedItem.Text);
            string query2 = "";
            string strqurey = "select Convert(nvarchar(15),From_Date,101) as From_Date,Convert(nvarchar(15),To_Date,101) as To_Date,PayMonthNum from HrPayMonths where College_Code='" + college_code + "' and SelStatus='1'";
            DataSet dspa = da.select_method_wo_parameter(strqurey, "text");
            if (from_month < to_month)
            {
                if (year <= yearto)
                {
                    //query2 = "select PayMonth,CONVERT(varchar(20), From_Date,101)as From_Date ,CONVERT(varchar(20), To_Date,101)as To_Date,PayMonthNum  from HrPayMonths where PayMonthNum between " + from_month + " and " + to_month + " and College_Code='" + college_code + "' and SelStatus='1'";    --  Old Query
                    query2 = "select PayMonth,CONVERT(varchar(20), From_Date,101)as From_Date ,CONVERT(varchar(20), To_Date,101)as To_Date,PayMonthNum  from HrPayMonths where (PayMonthNum >= " + from_month + " and PayYear between '" + year + "' and '" + yearto + "') and (PayMonthNum <=" + to_month + " and PayYear between '" + year + "' and '" + yearto + "') and College_Code='" + college_code + "' and SelStatus='1'";                    // -- New Query
                }
                else
                {
                    lblnorec.Visible = true;
                    lblnorec.Text = "Please Select Correct Month & Year!";
                    btngen.Visible = false;
                    gridview1.Visible = false;
                    return;
                }
            }
            else if (from_month == to_month)
            {
                if (year <= yearto)
                {
                    //query2 = "select PayMonth,CONVERT(varchar(20), From_Date,101)as From_Date ,CONVERT(varchar(20), To_Date,101)as To_Date,PayMonthNum  from HrPayMonths where PayMonthNum between " + from_month + " and " + to_month + " and College_Code='" + college_code + "' and SelStatus='1'";    -- Old Query
                    query2 = "select PayMonth,CONVERT(varchar(20), From_Date,101)as From_Date ,CONVERT(varchar(20), To_Date,101)as To_Date,PayMonthNum  from HrPayMonths where (PayMonthNum >=" + from_month + " and PayYear between '" + year + "' and '" + yearto + "') and (PayMonthNum <=" + to_month + " and PayYear between '" + year + "' and '" + yearto + "') and College_Code='" + college_code + "' and SelStatus='1'";                   //--New Query
                }
                else
                {
                    lblnorec.Visible = true;
                    lblnorec.Text = "Please Select Correct Month & Year!";
                    btngen.Visible = false;
                    gridview1.Visible = false;
                    return;
                }
            }
            else
            {
                if (year != yearto)
                {
                    //query2 = "   select PayMonth,CONVERT(varchar(20), From_Date,101)as From_Date ,CONVERT(varchar(20), To_Date,101)as To_Date,PayMonthNum  from HrPayMonths where PayMonthNum between '" + from_month + "' and 12 or  PayMonthNum between 1 and '" + to_month + "' and College_Code='" + college_code + "' and PayYear between '" + year + "' and '" + yearto + "' and SelStatus='1'";                                         //   --- old query
                    query2 = " select PayMonth,CONVERT(varchar(20), From_Date,101)as From_Date ,CONVERT(varchar(20), To_Date,101)as To_Date,PayMonthNum  from HrPayMonths where (PayMonthNum >='" + from_month + "' and PayYear between '" + year + "' and '" + yearto + "') or (PayMonthNum <='" + to_month + "' and PayYear between '" + year + "' and '" + yearto + "') and College_Code='" + college_code + "' and SelStatus='1'";    //   --- New Query
                }
                else
                {
                    lblnorec.Visible = true;
                    lblnorec.Text = "Please Select Correct Month & Year!";
                    btngen.Visible = false;
                    gridview1.Visible = false;
                    return;
                }
            }
            ds.Clear();
            ds = da.select_method_wo_parameter(query2, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    monthadd.Add(Convert.ToString(ds.Tables[0].Rows[row]["PayMonth"]));
                    monthnumber.Add(Convert.ToString(ds.Tables[0].Rows[row]["PayMonthNum"]));
                    addyear.Add(Convert.ToString(ds.Tables[0].Rows[row]["From_Date"]) + "," + Convert.ToString(ds.Tables[0].Rows[row]["To_Date"]));
                }
            }
            string strtypequery = "";
            if (ddlstftype.SelectedItem.Text != "All")
            {
                strtypequery = " and t.stftype='" + ddlstftype.SelectedItem.Text + "'";
            }
            string strstreamquery = "";
            if (ddlsteam.SelectedItem.Text != "All")
            {
                strstreamquery = " and s.Stream='" + ddlsteam.SelectedItem.Text + "'";
            }
            //    string category = "";

            string paymonth = "";
            string paymonthnumber = "";
            string monthdate = "";
            DataSet ds1 = new DataSet();
            if (addyear.Count > 0)
            {
                for (int i = 0; i < addyear.Count; i++)
                {
                    Double Netsalary = 0;
                    Double totAddtion = 0;
                    Double totdeduction = 0;
                    Double prevmonsal = 0;
                    count++;
                    paymonth = Convert.ToString(monthadd[i]);
                    paymonthnumber = Convert.ToString(monthnumber[i]);
                    string date = Convert.ToString(addyear[i]);
                    if (date.Trim() != "")
                    {
                        string[] date_split = date.Split(',');
                        if (date_split.Length > 0)
                        {
                            ds1.Clear();
                            string f1_date = Convert.ToString(date_split[0]);
                            string t1_date = Convert.ToString(date_split[1]);
                            monthdate = f1_date + "," + t1_date;
                            string getmonthnumber = (paymonthnumber);
                            int bal = Convert.ToInt32(getmonthnumber);
                            if (getmonthnumber == "1")
                            {
                                bal = 12;
                            }
                            else
                            {
                                bal--;
                            }
                            dspa.Tables[0].DefaultView.RowFilter = "PayMonthNum='" + bal + "'";
                            DataView dvquery = dspa.Tables[0].DefaultView;
                            if (dvquery.Count > 0)
                            {
                                DateTime dtcurr = Convert.ToDateTime(f1_date.ToString());
                                DateTime dtpre = Convert.ToDateTime(dvquery[0]["From_Date"].ToString());
                                DateTime dtpret = Convert.ToDateTime(dvquery[0]["to_Date"].ToString());
                                if (dtpre > dtcurr)
                                {
                                    dtpre = dtpre.AddYears(-1);
                                    dtpret = dtpret.AddYears(-1);
                                }
                                Double newstaffsal = 0;
                                Double reivedsatffsal = 0;
                                //string strval = " select isnull(sum(m.NetAddAct),'0') as monsal, m.fdate from monthlypay m,stafftrans t,staffmaster s where m.staff_code=t.staff_code  and t.staff_code = s.staff_code  " + strtypequery + " " + strstreamquery + " and fdate= '" + f1_date.ToString() + "' and t.latestrec = 1 and s.College_Code='" + college_code + "' and (s.resign=0 and s.settled=0 or (s.resign=1 and settled=1 and relieve_date >='" + t1_date.ToString() + "')) group by m.fdate";
                                //strval = strval + " select isnull(SUM(netaddact),'0') as relived from staffmaster s,monthlypay m,stafftrans t  where s.staff_code =m.staff_code and resign = 1  and settled = 1 and s.relieve_date between '" + f1_date.ToString() + "' and '" + t1_date.ToString() + "' and m.staff_code=s.staff_code  and t.staff_code =m.staff_code and s.staff_code =m.staff_code and s.College_Code='" + college_code + "' and m.college_code=s.college_code  and m.fdate ='" + dtpre.ToString("MM/dd/yyyy") + "' and m.tdate='" + dtpret.ToString("MM/dd/yyyy") + "' and t.latestrec =1  " + strtypequery + " " + strstreamquery + "";
                                //strval = strval + " select isnull(SUM(netaddact),'0') as newsatff from staffmaster s,monthlypay m,stafftrans t where s.staff_code =m.staff_code and t.staff_code =s.staff_code and t.staff_code =m.staff_code and s.join_date between '" + f1_date.ToString() + "'and '" + t1_date.ToString() + "' and m.fdate='" + f1_date.ToString() + "' and m.tdate='" + t1_date.ToString() + "' and s.College_Code='" + college_code + "' and t.latestrec =1 " + strtypequery + " " + strstreamquery + " ";
                                //strval = strval + " select s.staff_code,s.staff_name,NetAddAct,isnull((select NetAddAct from monthlypay m where m.staff_code=m1.staff_code and m.fdate='" + dtpre.ToString("MM/dd/yyyy") + "') -m1.NetAddAct,'0') as diffnet from monthlypay m1,staffmaster s,stafftrans t where s.staff_code=m1.staff_code and t.staff_code=m1.staff_code and t.staff_code=s.staff_code and  m1.fdate='" + f1_date.ToString() + "' and isnull((select NetAddAct from monthlypay m where m.staff_code=m1.staff_code and m.fdate='" + dtpre.ToString("MM/dd/yyyy") + "') -m1.NetAddAct,'0') <>'0' and t.latestrec=1 and s.resign=0 and s.settled=0 " + strtypequery + " " + strstreamquery + " group by  s.staff_code,s.staff_name,m1.staff_code,NetAddAct order by diffnet";
                                string strval = " select isnull(sum(m.netadd),'0') as currmonsal, m.fdate from monthlypay m,stafftrans t,staffmaster s where m.staff_code=t.staff_code  and t.staff_code = s.staff_code    and fdate= '" + f1_date.ToString() + "' and t.latestrec = 1 and t.category_code in('" + staffcategory + "') and s.College_Code='" + college_code + "' and (((s.resign=0 and s.settled=0) and (s.Discontinue=0 or s.Discontinue is null)) or (((s.resign=1 and settled=1) or (s.Discontinue =1)) and relieve_date >='" + f1_date.ToString() + "'))  group by m.fdate";//changed t1_date to f1_date for relieve_date
                                //strval = strval + " select isnull(sum(m.netadd),'0') as prevmonsal, m.fdate from monthlypay m,stafftrans t,staffmaster s where m.staff_code=t.staff_code  and t.staff_code = s.staff_code    and fdate= '" + dtpre.ToString("MM/dd/yyyy") + "' and t.latestrec = 1 and s.College_Code='" + college_code + "' and (s.resign=0 and s.settled=0 or (s.resign=1 and settled=1 and relieve_date >='" + dtpret.ToString("MM/dd/yyyy") + "'))  group by m.fdate";
                                //  delsi  //  strval = strval + " select s.staff_code,s.staff_name,isnull((select isnull(netadd,0) from monthlypay m where m.staff_code = s.staff_code and fdate = '" + f1_date.ToString() + "'),'0') as prevmon,isnull((select isnull(netadd,0) from monthlypay p where p.staff_code = s.staff_code and fdate = '" + f1_date.ToString() + "'),'0') as currmon,(isnull((select isnull(netadd,0) from monthlypay m where m.staff_code = s.staff_code and fdate = '" + dtpre.ToString("MM/dd/yyyy") + "'),'0')-isnull((select isnull(netadd,0) from monthlypay p where p.staff_code = s.staff_code and fdate = '" + f1_date.ToString() + "'),'0')) as diff from staffmaster s where ((resign = 0  and settled = 0) and (Discontinue =0 or Discontinue is null)) or (((resign = 1 and settled = 1) or (Discontinue =1)) and relieve_date > '" + dtpret.ToString("MM/dd/yyyy") + "')";
                                strval = strval + " select distinct s.staff_code,s.staff_name,s.college_code,isnull((select isnull(netadd,0) from monthlypay m where m.staff_code = s.staff_code and fdate = '" + dtpre.ToString() + "'),'0') as prevmon,isnull((select isnull(netadd,0) from monthlypay p where p.staff_code = s.staff_code and fdate = '" + f1_date.ToString() + "'),'0') as currmon,(isnull((select isnull(netadd,0) from monthlypay m where m.staff_code = s.staff_code and fdate = '" + dtpre.ToString("MM/dd/yyyy") + "'),'0')-isnull((select isnull(netadd,0) from monthlypay p where p.staff_code = s.staff_code and fdate = '" + f1_date.ToString() + "'),'0')) as diff from staffmaster s,stafftrans t where s.staff_code=t.staff_code and t.latestrec=1 and t.category_code in('" + staffcategory + "') and ((resign = 0  and settled = 0) and (Discontinue =0 or Discontinue is null)) or (((resign = 1 and settled = 1) or (Discontinue =1)) and relieve_date > '" + dtpre.ToString("MM/dd/yyyy") + "' and s.college_code='" + college_code + "')";//changed prevmon column value date from f1_date to dtpre date changed dtpret to  dtpre

                                DataSet dsshortclaim = da.select_method_wo_parameter(strval, "text");
                                DataView dvcollege = new DataView(); // poo
                                dsshortclaim.Tables[1].DefaultView.RowFilter = " college_code='" + college_code + "'"; // poo 
                                dvcollege = dsshortclaim.Tables[1].DefaultView; // poo 
                                Double exstaffaddtions = 0;
                                Double exstaffless = 0;
                                if (dsshortclaim.Tables[0].Rows.Count > 0)
                                {
                                    Netsalary = Convert.ToDouble(dsshortclaim.Tables[0].Rows[0]["currmonsal"].ToString());
                                }
                                if (dsshortclaim.Tables[1].Rows.Count > 0)
                                {
                                    for (int d = 0; d < dvcollege.Count; d++) // poo 
                                    {
                                        prevmonsal = Convert.ToDouble(dvcollege[d]["diff"].ToString()); // poo 
                                        if (prevmonsal > 0)
                                        {
                                            exstaffless = exstaffless + prevmonsal;
                                        }
                                        else
                                        {
                                            prevmonsal = Math.Round(prevmonsal, 0, MidpointRounding.AwayFromZero);
                                            string strva = prevmonsal.ToString();
                                            strva = strva.Replace('-', '0');
                                            prevmonsal = Convert.ToDouble(strva);
                                            exstaffaddtions = exstaffaddtions + Convert.ToDouble(strva);
                                        }
                                    }
                                }
                                //if (Netsalary > prevmonsal)
                                //{
                                //    totAddtion = Netsalary - prevmonsal;
                                //}
                                //if (prevmonsal > Netsalary)
                                //{
                                //    totdeduction = prevmonsal - Netsalary;
                                //}
                                //if (dsshortclaim.Tables[1].Rows.Count > 0)
                                //{
                                //    reivedsatffsal = Convert.ToDouble(dsshortclaim.Tables[1].Rows[0]["relived"].ToString());
                                //}
                                //if (dsshortclaim.Tables[2].Rows.Count > 0)
                                //{
                                //    newstaffsal = Convert.ToDouble(dsshortclaim.Tables[2].Rows[0]["newsatff"].ToString());
                                //}
                                //Double exstaffaddtions = 0;
                                //Double exstaffless = 0;
                                //for (int d = 0; d < dsshortclaim.Tables[3].Rows.Count; d++)
                                //{
                                //    Double getval = Convert.ToDouble(dsshortclaim.Tables[3].Rows[d]["diffnet"].ToString());
                                //    if (getval > 0)
                                //    {
                                //        exstaffless = exstaffless + getval;
                                //    }
                                //    else
                                //    {
                                //        getval = Math.Round(getval, 0, MidpointRounding.AwayFromZero);
                                //        string strva = getval.ToString();
                                //        strva = strva.Replace('-', '0');
                                //        getval = Convert.ToDouble(strva);
                                //        exstaffaddtions = exstaffaddtions + Convert.ToDouble(strva);
                                //    }
                                //}
                                //totAddtion = newstaffsal + exstaffaddtions;
                                totAddtion = exstaffaddtions;
                                totAddtion = Math.Round(totAddtion, 0, MidpointRounding.AwayFromZero);
                                //totdeduction = reivedsatffsal + exstaffless;
                                totdeduction = exstaffless;
                                totdeduction = Math.Round(totdeduction, 0, MidpointRounding.AwayFromZero);
                                if (totdeduction > totAddtion)
                                {
                                    totdeduction = totdeduction - totAddtion;
                                    totAddtion = 0;
                                }
                                else
                                {
                                    totAddtion = totAddtion - totdeduction;
                                    totdeduction = 0;
                                }
                            }
                        }
                        dt.Rows.Add(paymonth, Math.Round(Netsalary), Math.Round(totAddtion), Math.Round(totdeduction), monthdate, paymonthnumber);
                    }
                }
            }
            gridview1.DataSource = dt;
            gridview1.DataBind();
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, college_code, "HR_Reconciliation.aspx");
        }
    }
    //protected void Button1_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        btngen.Visible = true;
    //        if (Rbtformat1.Checked == true)
    //        {
    //            fpspread.Visible = false;
    //            Label1.Visible = false;
    //            gridview1.Visible = true;
    //        }
    //        else if (Rbtformat2.Checked == true)
    //        {
    //            fpspread.Visible = true;
    //            gridview1.Visible = false;
    //        }
    //        DataTable dt = new DataTable();
    //        int count = 0;
    //        ArrayList addyear = new ArrayList();
    //        ArrayList addvalue = new ArrayList();
    //        ArrayList monthadd = new ArrayList();
    //        ArrayList monthnumber = new ArrayList();
    //        dt.Columns.Add("Months", typeof(string));
    //        dt.Columns.Add("Overall Salary", typeof(string));
    //        dt.Columns.Add("Add", typeof(string));
    //        dt.Columns.Add("Less", typeof(string));
    //        dt.Columns.Add("Number", typeof(string));
    //        dt.Columns.Add("monthnum", typeof(string));
    //        int from_month = Convert.ToInt32(ddlfrom.SelectedItem.Value);
    //        int to_month = Convert.ToInt32(ddlto.SelectedItem.Value);
    //        int year = Convert.ToInt32(ddlfromyear.SelectedItem.Text);
    //        int yearto = Convert.ToInt32(ddltoyear.SelectedItem.Text);
    //        string query2 = "";
    //        Double allow = 0;
    //        Double allow1 = 0;
    //        string strqurey = "select Convert(nvarchar(15),From_Date,101) as From_Date,Convert(nvarchar(15),To_Date,101) as To_Date,PayMonthNum from HrPayMonths where College_Code='" + college_code + "' ";
    //        DataSet dspa = da.select_method_wo_parameter(strqurey, "text");
    //        if (from_month < to_month)
    //        {
    //            query2 = "select PayMonth,CONVERT(varchar(20), From_Date,101)as From_Date ,CONVERT(varchar(20), To_Date,101)as To_Date,PayMonthNum  from HrPayMonths where PayMonthNum between " + from_month + " and " + to_month + " and College_Code='" + college_code + "' ";
    //        }
    //        else if (from_month == to_month)
    //        {
    //            query2 = "select PayMonth,CONVERT(varchar(20), From_Date,101)as From_Date ,CONVERT(varchar(20), To_Date,101)as To_Date,PayMonthNum  from HrPayMonths where PayMonthNum between " + from_month + " and " + to_month + " and College_Code='" + college_code + "' ";
    //        }
    //        else
    //        {
    //            query2 = " select PayMonth,CONVERT(varchar(20), From_Date,101)as From_Date ,CONVERT(varchar(20), To_Date,101)as To_Date,PayMonthNum  from HrPayMonths where PayMonthNum between " + from_month + " and 12 or  PayMonthNum between 1 and " + to_month + " and College_Code='" + college_code + "'";
    //        }
    //        //int fm1 = Convert.ToInt32(from_month);
    //        //int fm = 0 + from_month;
    //        //fd1 = Convert.ToString(fd / fm / year);
    //        //DateTime dt1 = Convert.ToDateTime(fd1);
    //        //int tm1 = Convert.ToInt32(to_month);
    //        //int tm = 0 + to_month;
    //        //fd2 = Convert.ToString(td / tm / yearto);
    //        //DateTime dt2 = Convert.ToDateTime(fd2);
    //        //if (from_month == to_month && year < yearto)
    //        //{
    //        //    query2 = "select PayMonth,From_Date,To_Date,PayMonthNum  from HrPayMonths where  From_Date >='"+dt1+"' and To_Date<='" + dt2 +"' and College_Code='" + college_code + "'";
    //        //}
    //        ds.Clear();
    //        ds = da.select_method_wo_parameter(query2, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
    //            {
    //                monthadd.Add(Convert.ToString(ds.Tables[0].Rows[row]["PayMonth"]));
    //                monthnumber.Add(Convert.ToString(ds.Tables[0].Rows[row]["PayMonthNum"]));
    //                addyear.Add(Convert.ToString(ds.Tables[0].Rows[row]["From_Date"]) + "," + Convert.ToString(ds.Tables[0].Rows[row]["To_Date"]));
    //            }
    //        }
    //        string strtypequery = "";
    //        if (ddlstftype.SelectedItem.Text != "All")
    //        {
    //            strtypequery = " and t.stftype='" + ddlstftype.SelectedItem.Text + "'";
    //        }
    //        string strstreamquery = "";
    //        if (ddlsteam.SelectedItem.Text != "All")
    //        {
    //            strstreamquery = " and s.Stream='" + ddlsteam.SelectedItem.Text + "'";
    //        }
    //        string paymonth = "";
    //        string paymonthnumber = "";
    //        string monthdate = "";
    //        DataSet ds1 = new DataSet();
    //        if (addyear.Count > 0)
    //        {
    //            for (int i = 0; i < addyear.Count; i++)
    //            {
    //                count++;
    //                paymonth = Convert.ToString(monthadd[i]);
    //                paymonthnumber = Convert.ToString(monthnumber[i]);
    //                Double preamnt = 0;
    //                double preamntbasic = 0;
    //                double preamntnewcount = 0;
    //                double preamntrelived = 0;
    //                string newcount = "0";
    //                double addgrid = 0;
    //                string basic = "";
    //                string gettoded = "";
    //                string date = Convert.ToString(addyear[i]);
    //                if (date.Trim() != "")
    //                {
    //                    string[] date_split = date.Split(',');
    //                    if (date_split.Length > 0)
    //                    {
    //                        ds1.Clear();
    //                        string f1_date = Convert.ToString(date_split[0]);
    //                        string t1_date = Convert.ToString(date_split[1]);
    //                        monthdate = f1_date + "," + t1_date;
    //                        string premon = "select isnull(SUM(netaddact),'0') as net from monthlypay m,staffmaster s ,stafftrans t where m.fdate='" + f1_date + "' and m.tdate='" + t1_date + "' and m.staff_code=s.staff_code and t.staff_code =m.staff_code and s.staff_code =m.staff_code and s.College_Code='" + college_code + "' and m.college_code=s.College_Code and t.latestrec =1 and s.resign = 0 and settled = 0  " + strtypequery + " " + strstreamquery + "";
    //                        premon = premon + " select isnull(SUM(BasicInc),'0') as basic from monthlypay m,stafftrans t,staffmaster s  where  m.staff_code =t.staff_code and s.staff_code=m.staff_code and s.staff_code=t.staff_code and increment_date between '" + f1_date + "' and '" + t1_date + "' and m.fdate ='" + f1_date + "' and m.tdate ='" + t1_date + "' and parea ='Increment' and m.College_Code='" + college_code + "' " + strtypequery + " " + strstreamquery + "";
    //                        premon = premon + " select  distinct AllowIncDet  from monthlypay m,stafftrans t,staffmaster s  where  m.staff_code =t.staff_code and s.staff_code=m.staff_code and s.staff_code=t.staff_code and increment_date between '" + f1_date + "' and '" + t1_date + "' and m.fdate ='" + f1_date + "' and m.tdate ='" + t1_date + "' and  parea ='Increment' and m.College_Code='" + college_code + "' and isnull(AllowIncDet,'') <> '' " + strtypequery + "" + strstreamquery + "";
    //                        premon = premon + " select isnull(SUM(netaddact),'0') as newcount from staffmaster s,monthlypay m,stafftrans t  where s.staff_code =m.staff_code and s.join_date between '" + f1_date + "'and '" + t1_date + "' and m.staff_code=s.staff_code and t.staff_code =m.staff_code and s.staff_code =m.staff_code and m.college_code=s.college_code and m.fdate ='" + f1_date + "'and m.tdate='" + t1_date + "' and s.College_Code='" + college_code + "' and t.latestrec =1 and s.resign = 0 and settled = 0 " + strtypequery + " " + strstreamquery + "";
    //                        string getmonthnumber = (paymonthnumber);
    //                        int bal = Convert.ToInt32(getmonthnumber);
    //                        if (getmonthnumber == "1")
    //                        {
    //                            bal = 12;
    //                        }
    //                        else
    //                        {
    //                            bal--;
    //                        }
    //                        string getdiff = "";
    //                        string getleft = "";
    //                        Double taddtions = 0;
    //                        dspa.Tables[0].DefaultView.RowFilter = "PayMonthNum='" + bal + "'";
    //                        DataView dvquery = dspa.Tables[0].DefaultView;
    //                        if (dvquery.Count > 0)
    //                        {
    //                            DateTime dtcurr = Convert.ToDateTime(f1_date.ToString());
    //                            DateTime dtpre = Convert.ToDateTime(dvquery[0]["From_Date"].ToString());
    //                            DateTime dtpret = Convert.ToDateTime(dvquery[0]["to_Date"].ToString());
    //                            if (dtpre > dtcurr)
    //                            {
    //                                dtpre = dtpre.AddYears(-1);
    //                                dtpret = dtpret.AddYears(-1);
    //                            }
    //                            string strval = "select isnull(sum(m.NetAddAct),'0') as diff, m.fdate from monthlypay m,stafftrans t,staffmaster s where m.staff_code=t.staff_code  and t.staff_code = s.staff_code  " + strtypequery + " " + strstreamquery + " and fdate= '" + dtpre.ToString("MM/dd/yyyy") + "' and t.latestrec = 1 and s.resign = 0 and settled = 0 and s.College_Code='" + college_code + "' group by m.fdate";
    //                            strval = strval + " select isnull(sum(m.NetAddAct),'0') as diff, m.fdate from monthlypay m,stafftrans t,staffmaster s where m.staff_code=t.staff_code  and t.staff_code = s.staff_code  " + strtypequery + " " + strstreamquery + " and fdate= '" + f1_date.ToString() + "' and t.latestrec = 1 and s.resign = 0 and settled = 0 and s.College_Code='" + college_code + "' group by m.fdate";
    //                            strval = strval + " select isnull(SUM(netaddact),'0') as relived from staffmaster s,monthlypay m,stafftrans t  where s.staff_code =m.staff_code and resign = 1  and settled = 1 and s.relieve_date between '" + f1_date.ToString() + "' and '" + t1_date.ToString() + "' and m.staff_code=s.staff_code  and t.staff_code =m.staff_code and s.staff_code =m.staff_code and s.College_Code='" + college_code + "' and m.college_code=s.college_code  and m.fdate ='" + dtpre.ToString("MM/dd/yyyy") + "' and m.tdate='" + dtpret.ToString("MM/dd/yyyy") + "' and t.latestrec =1  " + strtypequery + " " + strstreamquery + "";
    //                            DataSet dsshortclaim = da.select_method_wo_parameter(strval, "text");
    //                            Double permondiff = 0, curmondiffv = 0, slaft = 0, tdeductions = 0;
    //                            if (dsshortclaim.Tables[0].Rows.Count > 0)
    //                            {
    //                                permondiff = Convert.ToDouble(dsshortclaim.Tables[0].Rows[0]["diff"].ToString());
    //                            }
    //                            if (dsshortclaim.Tables[1].Rows.Count > 0)
    //                            {
    //                                curmondiffv = Convert.ToDouble(dsshortclaim.Tables[1].Rows[0]["diff"].ToString());
    //                            }
    //                            if (dsshortclaim.Tables[2].Rows.Count > 0)
    //                            {
    //                                slaft = Convert.ToDouble(dsshortclaim.Tables[2].Rows[0]["relived"].ToString());
    //                            }
    //                            if (permondiff > curmondiffv)
    //                            {
    //                                tdeductions = permondiff - curmondiffv;
    //                                taddtions = 0;
    //                            }
    //                            else
    //                            {
    //                                tdeductions = 0;
    //                                taddtions = curmondiffv - permondiff;
    //                            }
    //                            tdeductions = Math.Round(tdeductions, 0, MidpointRounding.AwayFromZero);
    //                            getdiff = tdeductions.ToString();
    //                            slaft = Math.Round(slaft, 0, MidpointRounding.AwayFromZero);
    //                            getleft = slaft.ToString();
    //                            slaft = slaft + tdeductions;
    //                            gettoded = slaft.ToString();
    //                        }
    //                        premon = premon + " select isnull(SUM(netaddact),'0') as relived from staffmaster s,monthlypay m,stafftrans t  where s.staff_code =m.staff_code and resign = 1 and settled = 1 and s.relieve_date between '" + f1_date + "' and '" + t1_date + "' and m.staff_code=s.staff_code and t.staff_code =m.staff_code and s.staff_code =m.staff_code and s.College_Code='" + college_code + "' and m.college_code=s.college_code and m.fdate ='" + f1_date + "'and m.tdate='" + t1_date + "' and t.latestrec =1 " + strtypequery + " " + strstreamquery + "";
    //                        ds1 = da.select_method_wo_parameter(premon, "Text");
    //                        if (ds1.Tables[0].Rows.Count > 0)
    //                        {
    //                            string amount = Convert.ToString(ds1.Tables[0].Rows[0]["net"]);
    //                            if (amount.Trim() != "")
    //                            {
    //                                preamnt = Convert.ToDouble(amount);
    //                            }
    //                            else
    //                            {
    //                                preamnt = 0;
    //                            }
    //                        }
    //                        else
    //                        {
    //                            preamnt = 0;
    //                        }
    //                        if (ds1.Tables[1].Rows.Count > 0)
    //                        {
    //                            basic = Convert.ToString(ds1.Tables[1].Rows[0]["basic"]);
    //                            if (basic.Trim() != "")
    //                            {
    //                                preamntbasic = Convert.ToDouble(basic);
    //                            }
    //                            else
    //                            {
    //                                preamntbasic = 0;
    //                            }
    //                        }
    //                        else
    //                        {
    //                            preamntbasic = 0;
    //                        }
    //                        if (ds1.Tables[3].Rows.Count > 0)
    //                        {
    //                            newcount = Convert.ToString(ds1.Tables[3].Rows[0]["newcount"]);
    //                            if (newcount.Trim() != "")
    //                            {
    //                                preamntnewcount = Convert.ToDouble(newcount);
    //                            }
    //                            else
    //                            {
    //                                preamntnewcount = 0;
    //                            }
    //                        }
    //                        if (ds1.Tables[4].Rows.Count > 0)
    //                        {
    //                            string relived = Convert.ToString(ds1.Tables[4].Rows[0]["relived"]);
    //                            if (relived.Trim() != "")
    //                            {
    //                                preamntrelived = Convert.ToDouble(relived);
    //                            }
    //                            else
    //                            {
    //                                preamntrelived = 0;
    //                            }
    //                        }
    //                        else
    //                        {
    //                            preamntrelived = 0;
    //                        }
    //                        preamntrelived =Convert.ToDouble(gettoded);
    //                        if (ds1.Tables[2].Rows.Count > 0)
    //                        {
    //                            for (int al = 0; al < ds1.Tables[2].Rows.Count; al++)
    //                            {
    //                                string allowance = Convert.ToString(ds1.Tables[2].Rows[0]["AllowIncDet"]);
    //                                if (allowance.Trim() != "")
    //                                {
    //                                    string[] splitallowance = allowance.Split('\\');
    //                                    if (splitallowance.Length > 0)
    //                                    {
    //                                        for (int sp = 0; sp <= splitallowance.GetUpperBound(0); sp++)
    //                                        {
    //                                            string secondsplit = Convert.ToString(splitallowance[sp]);
    //                                            if (secondsplit.Trim() != "")
    //                                            {
    //                                                string[] splitscond = secondsplit.Split(';');
    //                                                if (splitscond.Length > 0)
    //                                                {
    //                                                    string allowvalue = Convert.ToString(splitscond[1]);
    //                                                    if (allowvalue.Trim() != "")
    //                                                    {
    //                                                        allow = allow + Convert.ToInt32(allowvalue);
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        allow = allow + 0;
    //                                                    }
    //                                                }
    //                                            }
    //                                        }
    //                                    }
    //                                    allow1 = Convert.ToDouble(allow);
    //                                }
    //                                else
    //                                {
    //                                    allow1 = allow + 0;
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            allow1 = 0;
    //                        }
    //                        addgrid = preamntnewcount + preamntbasic + allow1;
    //                    }
    //                }
    //                addvalue.Add(preamnt);
    //                addvalue.Add(preamntbasic);
    //                addvalue.Add(preamntnewcount);
    //                addvalue.Add(addgrid);
    //                dt.Rows.Add(paymonth, Math.Round(preamnt), Math.Round(addgrid), Math.Round(preamntrelived), monthdate, paymonthnumber);
    //            }
    //            gridview1.DataSource = dt;
    //            gridview1.DataBind();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblnorec.Visible = true;
    //        lblnorec.Text = ex.ToString();
    //    }
    //}
    public void bindbutn(Gios.Pdf.PdfDocument mydoc, Font Fontbold, Font Fontsmall)
    {
        try
        {

            string staffcategory1 = string.Empty;


            for (int i = 0; i < chklst_f1_category.Items.Count; i++)
            {
                if (chklst_f1_category.Items[i].Selected)
                {
                    if (staffcategory1 == "")
                    {
                        staffcategory1 = "" + chklst_f1_category.Items[i].Value.ToString();
                    }
                    else
                    {
                        staffcategory1 += "','" + chklst_f1_category.Items[i].Value.ToString() + "";
                    }
                }
            }




            lblnorec.Visible = false;
            string overallsalay = "";
            string monthtext = "";
            string monthtext1 = "";
            string previous = "";
            string overallsalay1 = "";
            string date = "";
            int year1 = 0;
            int year_value_bind = 0;
            string monthnumber = "";
            bool flag = false;
            string strtypequery = "";
            if (ddlstftype.SelectedItem.Text != "All")
            {
                strtypequery = " and t.stftype='" + ddlstftype.SelectedItem.Text + "'";
            }
            string strstreamquery = "";
            if (ddlsteam.SelectedItem.Text != "All")
            {
                strstreamquery = " and s.Stream='" + ddlsteam.SelectedItem.Text + "'";
            }
            string strqurey = "select Convert(nvarchar(15),From_Date,101) as From_Date,Convert(nvarchar(15),To_Date,101) as To_Date,PayMonthNum from HrPayMonths where College_Code='" + college_code + "' and SelStatus='1'";
            DataSet dspa = da.select_method_wo_parameter(strqurey, "text");
            string year = ddlfromyear.SelectedItem.Text;
            if (gridview1.Rows.Count > 0)
            {
                for (int row = 0; row < gridview1.Rows.Count; row++)
                {
                    if (((gridview1.Rows[row].FindControl("cbSelect") as CheckBox).Checked == true))
                    {
                        flag = true;
                        date = ((gridview1.Rows[row].FindControl("lblmonthnumber") as Label).Text);
                        overallsalay = ((gridview1.Rows[row].FindControl("lbloverall") as Label).Text);
                        monthtext = ((gridview1.Rows[row].FindControl("lblmonth") as Label).Text);
                        monthtext1 = monthtext.Substring(0, 3);
                        if (row != 0)
                        {
                            previous = ((gridview1.Rows[row - 1].FindControl("lblmonth") as Label).Text);
                            overallsalay1 = ((gridview1.Rows[row - 1].FindControl("lbloverall") as Label).Text);
                            string[] spt = overallsalay1.Split('.');
                            overallsalay1 = spt[0].ToString();
                        }
                        else
                        {
                            monthnumber = ((gridview1.Rows[row].FindControl("lblmonthnum") as Label).Text);
                            int prev_number = Convert.ToInt32(monthnumber) - 1;
                            int prev_year = Convert.ToInt32(ddlfromyear.SelectedItem.Text);
                            if (prev_number == 0)
                            {
                                prev_number = 12;
                                prev_year = prev_year - 1;
                            }
                            string row7 = "select distinct fdate,tdate  from monthlypay where PayMonth ='" + prev_number + "' and PayYear ='" + prev_year + "' and College_Code='" + college_code + "'";
                            ds.Clear();
                            ds = da.select_method_wo_parameter(row7, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string from_prev_date = Convert.ToString(ds.Tables[0].Rows[0]["fdate"]);
                                string to_prev_date = Convert.ToString(ds.Tables[0].Rows[0]["tdate"]);
                                string row8 = "select SUM(netaddact) as net from monthlypay m,staffmaster s ,stafftrans t where m.staff_code=s.staff_code and t.staff_code =m.staff_code and s.staff_code =m.staff_code and m.college_code=s.College_Code and t.latestrec =1 and ((s.resign = 0 and settled = 0) and (s.Discontinue=0 or s.Discontinue is null)) and m.fdate='" + from_prev_date + "' and m.tdate='" + to_prev_date + "' and s.College_Code='" + college_code + "'" + strstreamquery + " " + strtypequery + "";
                                ds.Clear();
                                ds = da.select_method_wo_parameter(row8, "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    overallsalay1 = Convert.ToString(ds.Tables[0].Rows[0]["net"]);
                                    string[] spt = overallsalay1.Split('.');
                                    overallsalay1 = spt[0].ToString();
                                }
                                string row9 = "select PayMonth  from HrPayMonths where PayMonthNum ='" + prev_number + "' and College_Code='" + college_code + "' and SelStatus='1'";
                                ds.Clear();
                                ds = da.select_method_wo_parameter(row9, "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    previous = Convert.ToString(ds.Tables[0].Rows[0]["PayMonth"]);
                                }
                            }
                            else
                            {
                                overallsalay1 = "0";
                                string row9 = "select PayMonth  from HrPayMonths where PayMonthNum ='" + prev_number + "' and College_Code='" + college_code + "' and SelStatus='1'";
                                ds.Clear();
                                ds = da.select_method_wo_parameter(row9, "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    previous = Convert.ToString(ds.Tables[0].Rows[0]["PayMonth"]);
                                }
                            }
                        }
                        string collegenew1 = "";
                        string address1 = "";
                        string address2 = "";
                        string district = "";
                        string pincode = "";
                        string collegetitle = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,district ,pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
                        ds7.Clear();
                        ds7 = da.select_method_wo_parameter(collegetitle, "Text");
                        if (ds7.Tables[0].Rows.Count > 0)
                        {
                            for (int count = 0; count < ds7.Tables[0].Rows.Count; count++)
                            {
                                collegenew1 = Convert.ToString(ds7.Tables[0].Rows[count]["collname"]);
                                address1 = Convert.ToString(ds7.Tables[0].Rows[count]["address1"]);
                                address2 = Convert.ToString(ds7.Tables[0].Rows[count]["address3"]);
                                district = Convert.ToString(ds7.Tables[0].Rows[count]["district"]);
                                pincode = Convert.ToString(ds7.Tables[0].Rows[count]["pincode"]);
                            }
                        }
                        Double permondiff = 0; Double curmondiffv = 0, totdiff = 0, relivestaffsal = 0, newstaffsal = 0, tdeductions = 0, totaddtion = 0, exstaffaddtions = 0, exstaffless = 0, newlyadded = 0, newlyleft = 0, newjoinerexjoindate = 0, relivedlastmonth = 0;
                        if (date.Trim() != "")
                        {
                            string[] m = date.Split(',');
                            if (m.Length > 0)
                            {
                                ds.Clear();
                                string l = m[0];
                                string o = m[1];
                                DateTime dt1 = new DateTime();
                                dt1 = Convert.ToDateTime(o);
                                year_value_bind = Convert.ToInt32(Convert.ToString(dt1.ToString("yyyy")));
                                int month_value = Convert.ToInt32(Convert.ToString(dt1.ToString("MM")));
                                if (month_value == 1)
                                {
                                    year1 = year_value_bind - 1;
                                }
                                else
                                {
                                    year1 = year_value_bind;
                                }
                                string getmonthnumber = ((gridview1.Rows[row].FindControl("lblmonthnum") as Label).Text);
                                int bal = Convert.ToInt32(getmonthnumber);
                                if (getmonthnumber == "1")
                                {
                                    bal = 12;
                                }
                                else
                                {
                                    bal--;
                                }
                                string getdiff = "";
                                dspa.Tables[0].DefaultView.RowFilter = "PayMonthNum='" + bal + "'";
                                DataView dvquery = dspa.Tables[0].DefaultView;
                                string strstaffdetails = "";
                                string strrelivestaffdetails = "";
                                DataSet dsgetstaffnettdetails = new DataSet();
                                if (dvquery.Count > 0)
                                {
                                    DateTime dtcurr = Convert.ToDateTime(l.ToString());
                                    DateTime dtpre = Convert.ToDateTime(dvquery[0]["From_Date"].ToString());
                                    DateTime dtpret = Convert.ToDateTime(dvquery[0]["to_Date"].ToString());
                                    if (dtpre > dtcurr)
                                    {
                                        dtpre = dtpre.AddYears(-1);
                                        dtpret = dtpret.AddYears(-1);
                                    }
                                    //string strval = "select isnull(sum(m.NetAddAct),'0') as previousmonsal, m.fdate from monthlypay m,stafftrans t,staffmaster s where m.staff_code=t.staff_code and t.staff_code = s.staff_code  " + strtypequery + " " + strstreamquery + " and fdate= '" + dtpre.ToString("MM/dd/yyyy") + "' and t.latestrec = 1 and s.College_Code='" + college_code + "' and (s.resign=0 and s.settled=0 or (s.resign=1 and settled=1 and relieve_date >='" + dtpret.ToString("MM/dd/yyyy") + "')) group by m.fdate";
                                    // string strval = "select ISNULL(SUM(netadd),'0') as previousmonsal from monthlypay m,staffmaster s where m.staff_code=s.staff_code and fdate='" + dtpre.ToString("MM/dd/yyyy") + "' and ((resign=0 and settled=0) and (Discontinue=0 or Discontinue is null)) or (((resign=1 and settled=1) or (Discontinue=1)) and relieve_date>'" + dtpret.ToString("MM/dd/yyyy") + "')"; delsi 3007

                                    string strval = " select isnull(sum(netadd),'0') as previousmonsal from monthlypay m,staffmaster s,stafftrans t where m.staff_code=s.staff_code and t.staff_code = s.staff_code  and t.latestrec = 1 and t.category_code in('" + staffcategory1 + "')  and fdate= '" + dtpre.ToString("MM/dd/yyyy") + "' and (((resign=0 and settled=0) and (Discontinue=0 or Discontinue is null)) or (((resign=1 and settled=1) or (Discontinue =1))  and relieve_date >='" + dtpre.ToString("MM/dd/yyyy") + "' ))";



                                    strval = strval + " select isnull(sum(netadd),'0') as currmonsal from monthlypay m,staffmaster s ,stafftrans t where m.staff_code=s.staff_code and t.staff_code = s.staff_code  and t.latestrec = 1 and t.category_code in('" + staffcategory1 + "') and fdate= '" + l.ToString() + "' and (((resign=0 and settled=0) and (Discontinue=0 or Discontinue is null)) or (((resign=1 and settled=1) or (Discontinue =1))  and relieve_date >='" + l.ToString() + "' )) ";// relieved_date changed o date to l date
                                    // strval = strval + " select ISNULL(SUM(netadd),'0') as currmonsal from monthlypay m,staffmaster s where m.staff_code=s.staff_code and fdate='" + l.ToString() + "' and ((resign=0 and settled=0) and (Discontinue=0 or Discontinue is null)) or (((resign=1 and settled=1) or (Discontinue=1)) and relieve_date>'" + o.ToString() + "')";//delsi 3007
                                    //strval = strval + " select isnull(sum(m.netadd),'0') as currmonsal, m.fdate from monthlypay m,stafftrans t,staffmaster s where m.staff_code=t.staff_code  and t.staff_code = s.staff_code  " + strtypequery + " " + strstreamquery + " and fdate= '" + l.ToString() + "' and t.latestrec = 1 and s.College_Code='" + college_code + "' and (s.resign=0 and s.settled=0 or (s.resign=1 and settled=1 and relieve_date >='" + o.ToString() + "'))  group by m.fdate";
                                    strval = strval + " select isnull(SUM(netadd),'0') as relived from staffmaster s,monthlypay m,stafftrans t  where s.staff_code =m.staff_code and ((resign = 1  and settled = 1) or (Discontinue=1)) and s.relieve_date between '" + l.ToString() + "' and '" + o.ToString() + "' and m.staff_code=s.staff_code  and t.staff_code =m.staff_code and s.staff_code =m.staff_code and s.College_Code='" + college_code + "' and m.college_code=s.college_code  and m.fdate ='" + dtpre.ToString("MM/dd/yyyy") + "' and m.tdate='" + dtpret.ToString("MM/dd/yyyy") + "' and t.latestrec =1  " + strtypequery + " " + strstreamquery + "";
                                    strval = strval + " select isnull(SUM(netadd),'0') as newsatff from staffmaster s,monthlypay m,stafftrans t where s.staff_code =m.staff_code and t.staff_code =s.staff_code and t.staff_code =m.staff_code and s.join_date between '" + l.ToString() + "'and '" + o.ToString() + "' and m.fdate='" + l.ToString() + "' and m.tdate='" + o.ToString() + "' and s.College_Code='" + college_code + "' and t.latestrec =1 " + strtypequery + " " + strstreamquery + " ";
                                    DataSet dsshortclaim = da.select_method_wo_parameter(strval, "text");
                                    //strstaffdetails = "select s.staff_code,s.staff_name,NetAddAct,isnull((select NetAddAct from monthlypay m where m.staff_code=m1.staff_code and m.fdate='" + dtpre.ToString("MM/dd/yyyy") + "') -m1.NetAddAct,'0') as diffnet from monthlypay m1,staffmaster s,stafftrans t where s.staff_code=m1.staff_code and t.staff_code=m1.staff_code and t.staff_code=s.staff_code and  m1.fdate='" + l.ToString() + "' and isnull((select NetAddAct from monthlypay m where m.staff_code=m1.staff_code and m.fdate='" + dtpre.ToString("MM/dd/yyyy") + "') -m1.NetAddAct,'0') <>'0' and t.latestrec=1 and s.resign=0 and s.settled=0 " + strtypequery + " " + strstreamquery + " group by  s.staff_code,s.staff_name,m1.staff_code,NetAddAct order by diffnet";
                                    //delsi  strstaffdetails = " select s.staff_code,s.staff_name,isnull((select isnull(netadd,0) from monthlypay m where m.staff_code = s.staff_code and fdate = '" + dtpre.ToString("MM/dd/yyyy") + "'),'0') as prevmon,isnull((select isnull(netadd,0) from monthlypay p where p.staff_code = s.staff_code and fdate = '" + l.ToString() + "'),'0') as currmon,ISNULL((select isnull(netadd,0) from monthlypay m where m.staff_code = s.staff_code and fdate = '" + dtpre.ToString("MM/dd/yyyy") + "'),'0')-isnull((select isnull(netadd,0) from monthlypay p where p.staff_code = s.staff_code and fdate = '" + l.ToString() + "'),'0') as different from staffmaster s where ((resign = 0  and settled = 0) and (Discontinue=0 or Discontinue is null)) or (((resign = 1 and settled = 1) or (Discontinue=1)) and relieve_date > '" + o.ToString() + "') " + strstreamquery + "";   //" + strtypequery + "
                                    strstaffdetails = " select distinct s.staff_code,s.staff_name,s.college_code,isnull((select isnull(netadd,0) from monthlypay m where m.staff_code = s.staff_code and fdate = '" + dtpre.ToString("MM/dd/yyyy") + "'),'0') as prevmon,isnull((select isnull(netadd,0) from monthlypay p where p.staff_code = s.staff_code and fdate = '" + l.ToString() + "'),'0') as currmon,ISNULL((select isnull(netadd,0) from monthlypay m where m.staff_code = s.staff_code and fdate = '" + dtpre.ToString("MM/dd/yyyy") + "' and m.category_code in('" + staffcategory1 + "')),'0')-isnull((select isnull(netadd,0) from monthlypay p where p.staff_code = s.staff_code and fdate = '" + l.ToString() + "' and p.category_code in('" + staffcategory1 + "')),'0') as different from staffmaster s,stafftrans t where s.staff_code=t.staff_code and t.latestrec=1 and t.category_code in('" + staffcategory1 + "') and ((resign = 0  and settled = 0) and (Discontinue=0 or Discontinue is null)) or (((resign = 1 and settled = 1) or (Discontinue=1)) and relieve_date > '" + dtpre.ToString() + "') and s.college_code='" + college_code + "' " + strstreamquery + "";   //" + strtypequery + "
                                    strrelivestaffdetails = " select s.staff_code,s.staff_name,isnull(netadd,'0') as relived from staffmaster s,monthlypay m,stafftrans t  where s.staff_code =m.staff_code and ((resign = 1  and settled = 1) or (Discontinue=1)) and s.relieve_date between '" + l.ToString() + "' and '" + o.ToString() + "' and m.staff_code=s.staff_code  and t.staff_code =m.staff_code and s.staff_code =m.staff_code and s.College_Code='" + college_code + "' and m.college_code=s.college_code  and m.fdate ='" + dtpre.ToString("MM/dd/yyyy") + "' and m.tdate='" + dtpret.ToString("MM/dd/yyyy") + "' and t.latestrec =1  " + strtypequery + " " + strstreamquery + "";
                                    dsgetstaffnettdetails = da.select_method_wo_parameter(strstaffdetails, "Text");

                                    DataView dvstaffdetailsFilter = new DataView();//delsi0704
                                    dsgetstaffnettdetails.Tables[0].DefaultView.RowFilter = "College_Code='" + college_code + "'";
                                    dvstaffdetailsFilter = dsgetstaffnettdetails.Tables[0].DefaultView;
                                    if (dsshortclaim.Tables[0].Rows.Count > 0)
                                    {
                                        permondiff = Convert.ToDouble(dsshortclaim.Tables[0].Rows[0]["previousmonsal"].ToString());
                                        permondiff = Math.Round(permondiff, 0, MidpointRounding.AwayFromZero);
                                    }
                                    if (dsshortclaim.Tables[1].Rows.Count > 0)
                                    {
                                        curmondiffv = Convert.ToDouble(dsshortclaim.Tables[1].Rows[0]["currmonsal"].ToString());
                                        curmondiffv = Math.Round(curmondiffv, 0, MidpointRounding.AwayFromZero);
                                    }
                                    if (permondiff > curmondiffv)
                                    {
                                        totdiff = permondiff - curmondiffv;
                                        totdiff = Math.Round(totdiff, 0, MidpointRounding.AwayFromZero);
                                    }
                                    else
                                    {
                                        totdiff = curmondiffv - permondiff;
                                        totdiff = Math.Round(totdiff, 0, MidpointRounding.AwayFromZero);
                                    }
                                    if (dsshortclaim.Tables[2].Rows.Count > 0)
                                    {
                                        relivestaffsal = Convert.ToDouble(dsshortclaim.Tables[2].Rows[0]["relived"].ToString());
                                    }
                                    if (dsshortclaim.Tables[3].Rows.Count > 0)
                                    {
                                        newstaffsal = Convert.ToDouble(dsshortclaim.Tables[3].Rows[0]["newsatff"].ToString());
                                    }
                                    // for (int d = 0; d < dsgetstaffnettdetails.Tables[0].Rows.Count; d++)
                                    for (int d = 0; d < dvstaffdetailsFilter.Count; d++)//delsi0704
                                    {
                                        // Double getval = Convert.ToDouble(dsgetstaffnettdetails.Tables[0].Rows[d]["different"].ToString());
                                        Double getval = Convert.ToDouble(dvstaffdetailsFilter[d]["different"].ToString());//delsi0704
                                        Double prevmon = Convert.ToDouble(dvstaffdetailsFilter[d]["prevmon"].ToString());
                                        Double currmon = Convert.ToDouble(dvstaffdetailsFilter[d]["currmon"].ToString());
                                        if (prevmon != 0 && currmon != 0)
                                        {
                                            if (getval > 0)
                                            {
                                                exstaffless = exstaffless + getval;
                                            }
                                            else
                                            {
                                                getval = Math.Round(getval, 0, MidpointRounding.AwayFromZero);
                                                string strva = getval.ToString();
                                                strva = strva.Replace('-', '0');
                                                exstaffaddtions = exstaffaddtions + Convert.ToDouble(strva);
                                            }
                                        }
                                        else if (prevmon == 0 && currmon != 0)
                                        {
                                            getval = Math.Round(getval, 0, MidpointRounding.AwayFromZero);
                                            string strva = getval.ToString();
                                            strva = strva.Replace('-', '0');
                                            newjoinerexjoindate = newjoinerexjoindate + Convert.ToDouble(strva);

                                        }
                                        else if (prevmon != 0 && currmon == 0)
                                        {
                                            getval = Math.Round(getval, 0, MidpointRounding.AwayFromZero);
                                            relivedlastmonth = getval;

                                        }
                                        else
                                        {

                                            if (getval > 0)
                                            {
                                                newlyleft = newlyleft + getval;
                                            }
                                            else
                                            {
                                                getval = Math.Round(getval, 0, MidpointRounding.AwayFromZero);
                                                string strva = getval.ToString();
                                                strva = strva.Replace('-', '0');
                                                newlyadded = newlyadded + Convert.ToDouble(strva);
                                            }
                                        }
                                    }
                                    // exstaffaddtions = exstaffaddtions + newlyadded; delsi2109
                                    exstaffaddtions = Math.Round(exstaffaddtions, 0, MidpointRounding.AwayFromZero);
                                    // totaddtion = newstaffsal + exstaffaddtions;
                                    totaddtion = newjoinerexjoindate + exstaffaddtions;
                                    totaddtion = Math.Round(totaddtion, 0, MidpointRounding.AwayFromZero);
                                    // exstaffless = exstaffless + newlyleft;

                                    exstaffless = Math.Round(exstaffless, 0, MidpointRounding.AwayFromZero);
                                    //  tdeductions = relivestaffsal + exstaffless;
                                    tdeductions = relivedlastmonth + exstaffless;
                                    tdeductions = Math.Round(tdeductions, 0, MidpointRounding.AwayFromZero);
                                    newlyadded = Math.Round(newlyadded, 0, MidpointRounding.AwayFromZero);
                                    newlyleft = Math.Round(newlyleft, 0, MidpointRounding.AwayFromZero);
                                    newjoinerexjoindate = Math.Round(newjoinerexjoindate, 0, MidpointRounding.AwayFromZero);
                                    relivedlastmonth = Math.Round(relivedlastmonth, 0, MidpointRounding.AwayFromZero);
                                }
                                int y = 60;
                                Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
                                Gios.Pdf.PdfPage mypdfpage1;
                                Gios.Pdf.PdfPage mypdfpage2;
                                int coltop = 0;
                                for (int i = 0; i < 5; i++)
                                {
                                    coltop = coltop + 20;
                                    if (coltop > 720)
                                    {
                                        coltop = 20;
                                    }
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                {
                                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage, 20, 10, 450);
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                {
                                    PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage1, 500, 10, 450);
                                }
                                PdfTextArea pdf = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 80, 15, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, collegenew1);
                                PdfTextArea pdf01 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 80, 30, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, address2 + "," + district + " " + pincode);
                                PdfTextArea pdf02 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 80, 60, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "RECONCILIATION");
                                PdfTextArea pdf1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 40, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, "RECONCILIATION STATEMENT FOR THE MONTH OF ");

                                PdfTextArea pdfline = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 60, y + 45, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, "        __________________________________________________________");
                                mypdfpage.Add(pdfline);

                                y = y + 10;
                                PdfTextArea pdf2 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, y + 70, 400, 30), System.Drawing.ContentAlignment.TopLeft, "SALARY CLAIMED IN ");
                                PdfTextArea pdf22 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 330, y + 60, 200, 30), System.Drawing.ContentAlignment.MiddleRight, curmondiffv.ToString());
                                PdfTextArea pdf3 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, y + 90, 400, 30), System.Drawing.ContentAlignment.TopLeft, "SALARY CLAIMED IN ");
                                PdfTextArea pdf23 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 330, y + 80, 200, 30), System.Drawing.ContentAlignment.MiddleRight, permondiff.ToString());
                                PdfTextArea pdf005 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 20, y + 110, 200, 30), System.Drawing.ContentAlignment.TopLeft, "TOTAL");
                                PdfTextArea pdf0001 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 470, y + 100, 200, 30), System.Drawing.ContentAlignment.TopLeft, "-----------------");
                                PdfTextArea pdf24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 330, y + 107, 200, 30), System.Drawing.ContentAlignment.MiddleRight, totdiff.ToString());
                                PdfTextArea pdf0000 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 470, y + 120, 200, 30), System.Drawing.ContentAlignment.TopLeft, "-----------------");
                                PdfTextArea pdf4 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 20, y + 160, 500, 30), System.Drawing.ContentAlignment.TopLeft, "A            ADDITION ");
                                PdfTextArea pdf5 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 70, y + 180, 500, 30), System.Drawing.ContentAlignment.TopLeft, "INCREASE IN SALARY");
                                PdfTextArea pdf007 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 330, y + 170, 200, 30), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(Math.Round(exstaffaddtions)));
                                PdfTextArea pdf6 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 70, y + 200, 500, 30), System.Drawing.ContentAlignment.TopLeft, "NEWLY ADDED IN ");
                                PdfTextArea pdf25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 470, y + 210, 200, 30), System.Drawing.ContentAlignment.TopLeft, "-----------------");
                                PdfTextArea pdf29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 330, y + 220, 200, 30), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(Math.Round(totaddtion)));  //totaddtion
                                PdfTextArea pdf26 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 330, y + 190, 200, 30), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(Math.Round(newjoinerexjoindate)));   //newstaffsal
                                PdfTextArea pdf7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 20, y + 260, 500, 30), System.Drawing.ContentAlignment.TopLeft, "B            LESS ");
                                //  PdfTextArea pdf8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 70, y + 280, 500, 30), System.Drawing.ContentAlignment.TopLeft, "SHORT CLAIMS");
                                PdfTextArea pdf8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 70, y + 280, 500, 30), System.Drawing.ContentAlignment.TopLeft, "DECREASE IN SALARY");
                                PdfTextArea pdf28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 330, y + 270, 200, 30), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(exstaffless));   //exstaffless
                                PdfTextArea pdf9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 70, y + 300, 500, 30), System.Drawing.ContentAlignment.TopLeft, "LEFT");
                                PdfTextArea pdf19 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 330, y + 290, 200, 30), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(Math.Round(relivedlastmonth)));  //exstaffless
                                // PdfTextArea pdf003 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 330, y + 315, 200, 30), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(Math.Round(relivestaffsal)));   //relivestaffsal
                                PdfTextArea pdf002 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 470, y + 335, 200, 30), System.Drawing.ContentAlignment.TopLeft, "-----------------");
                                PdfTextArea pdf004 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 330, y + 340, 200, 30), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(Math.Round(tdeductions)));
                                PdfTextArea pdf006 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 470, y + 355, 200, 30), System.Drawing.ContentAlignment.TopLeft, "-----------------");
                                PdfTextArea pdf050 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 70, y + 375, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Total (A-B)");
                                PdfTextArea pdf051 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 330, y + 375, 200, 30), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(totdiff));
                                string gettext = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='printRecons' and college_code='" + Convert.ToString(college_code) + "' and user_Code='" + Convert.ToString(Session["usercode"]) + "'");//delsi 1010
                                PdfTextArea pdf028;
                                if (gettext != "0")
                                {
                                    pdf028 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 500, y + 450, 200, 30), System.Drawing.ContentAlignment.TopLeft, gettext);

                                }
                                else
                                {
                                    pdf028 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 500, y + 450, 200, 30), System.Drawing.ContentAlignment.TopLeft, "Principal");
                                }
                                PdfTextArea pdf029 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 20, y + 450, 200, 30), System.Drawing.ContentAlignment.TopLeft, DateTime.Now.ToString("dd/MM/yyyy"));
                                PdfTextArea pdf91 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 450, y + 30, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "" + monthtext.ToString().ToUpper() + "-" + "" + year_value_bind + "");
                                PdfTextArea pdf92 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 125, y + 70, 500, 30), System.Drawing.ContentAlignment.TopLeft, "" + monthtext.ToString().ToUpper() + "-" + "" + year_value_bind + "");
                                PdfTextArea pdf333 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 125, y + 90, 400, 30), System.Drawing.ContentAlignment.TopLeft, "" + previous.ToString().ToUpper() + "-" + "" + year1 + "");
                                PdfTextArea pdf90 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 160, y + 200, 500, 30), System.Drawing.ContentAlignment.TopLeft, "" + monthtext1.ToString().ToUpper() + "-" + "" + year_value_bind + " PAY BILL");
                                mypdfpage.Add(pdf91);
                                mypdfpage.Add(pdf92);
                                mypdfpage.Add(pdf333);
                                mypdfpage.Add(pdf90);
                                mypdfpage.Add(pdf);
                                mypdfpage.Add(pdf0000);
                                mypdfpage.Add(pdf0001);
                                mypdfpage.Add(pdf050);
                                mypdfpage.Add(pdf051);
                                mypdfpage.Add(pdf028);
                                mypdfpage.Add(pdf029);
                                mypdfpage.Add(pdf01);
                                mypdfpage.Add(pdf02);
                                mypdfpage.Add(pdf002);
                                //mypdfpage.Add(pdf003);delsi2109
                                mypdfpage.Add(pdf004);
                                mypdfpage.Add(pdf005);
                                mypdfpage.Add(pdf006);
                                mypdfpage.Add(pdf007);
                                mypdfpage.Add(pdf1);
                                mypdfpage.Add(pdf2);
                                mypdfpage.Add(pdf3);
                                mypdfpage.Add(pdf22);
                                mypdfpage.Add(pdf23);
                                mypdfpage.Add(pdf24);
                                mypdfpage.Add(pdf4);
                                mypdfpage.Add(pdf5);
                                mypdfpage.Add(pdf6);
                                mypdfpage.Add(pdf7);
                                mypdfpage.Add(pdf8);
                                mypdfpage.Add(pdf9);
                                mypdfpage.Add(pdf19);
                                mypdfpage.Add(pdf25);
                                mypdfpage.Add(pdf26);
                                mypdfpage.Add(pdf28);
                                mypdfpage.Add(pdf29);
                                mypdfpage.SaveToDocument();
                                mypdfpage1 = mydoc.NewPage();
                                mypdfpage1.Add(pdf);
                                mypdfpage1.Add(pdf01);
                                coltop = 100;
                                PdfTextArea pdf30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, "NET INCREASE/DECREASE FOR THE MONTH OF " + monthtext.ToString().ToUpper() + "-" + "" + year_value_bind + "");
                                mypdfpage1.Add(pdf30);
                                coltop = coltop + 40;
                                PdfTextArea pdf31 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 20, coltop, 500, 30), System.Drawing.ContentAlignment.TopLeft, "INCREASE IN SALARY ");
                                mypdfpage1.Add(pdf31);
                                coltop = coltop + 20;
                                PdfTextArea pdf32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 500, 30), System.Drawing.ContentAlignment.TopLeft, "EMP No        NAME ");
                                PdfTextArea pdf39 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 500, 30), System.Drawing.ContentAlignment.TopRight, "AMOUNT");
                                coltop = coltop + 20;
                                PdfTextArea pdf33 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 500, 30), System.Drawing.ContentAlignment.TopLeft, "A:INCREASE ");
                                mypdfpage1.Add(pdf32);
                                mypdfpage1.Add(pdf33);
                                mypdfpage1.Add(pdf39);
                                coltop = coltop + 20;
                                double add = 0;
                                DataView grossaddition = new DataView();
                                if (exstaffaddtions > 0)
                                {
                                    int srno = 0;
                                    //string getval = "different < 0"; // commented by poomalar 22.12.17
                                    string getval = "different < 0 and college_code='" + college_code + "' and prevmon<>0 and currmon<>'0' "; // poo 22.12.17 included prevmon and currmon
                                    dsgetstaffnettdetails.Tables[0].DefaultView.RowFilter = getval;
                                    DataView dvaddtion = dsgetstaffnettdetails.Tables[0].DefaultView;

                                    for (int an = 0; an < dvaddtion.Count; an++)
                                    {
                                        Double prevsal = Convert.ToDouble(dvaddtion[an]["prevmon"]);
                                        Double currmon = Convert.ToDouble(dvaddtion[an]["currmon"]);
                                        if (prevsal != 0 || currmon != 0)
                                        {
                                            coltop = coltop + 20;
                                            if (coltop > 700)
                                            {
                                                mypdfpage1.SaveToDocument();
                                                coltop = 40;
                                                mypdfpage1 = mydoc.NewPage();
                                            }
                                            srno++;
                                            string staff = dvaddtion[an]["staff_name"].ToString();
                                            string staffcode = dvaddtion[an]["staff_code"].ToString();
                                            double diff = Convert.ToDouble(dvaddtion[an]["different"].ToString());
                                            diff = Math.Round(diff, 0, MidpointRounding.AwayFromZero);
                                            string strva = diff.ToString();
                                            strva = strva.Replace('-', '0');
                                            diff = Convert.ToDouble(strva);
                                            add = add + Convert.ToDouble(diff);
                                            PdfTextArea pdf415 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 10, coltop - 10, 500, 30), System.Drawing.ContentAlignment.TopLeft, srno.ToString());
                                            mypdfpage1.Add(pdf415);
                                            PdfTextArea pdf45 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop - 10, 500, 30), System.Drawing.ContentAlignment.TopLeft, staffcode + "        " + staff.ToString());
                                            mypdfpage1.Add(pdf45);
                                            PdfTextArea pdf451 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop - 10, 500, 30), System.Drawing.ContentAlignment.TopRight, diff.ToString());
                                            mypdfpage1.Add(pdf451);
                                        }
                                    }
                                }
                                else
                                {
                                    coltop = coltop + 20;
                                    PdfTextArea pdf451 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop - 10, 500, 30), System.Drawing.ContentAlignment.TopRight, "0");
                                    mypdfpage1.Add(pdf451);
                                }
                                coltop = coltop + 30;
                                PdfTextArea pdfaddtotal = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 300, 30), System.Drawing.ContentAlignment.TopRight, "Total");
                                mypdfpage1.Add(pdfaddtotal);
                                pdfaddtotal = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 500, 30), System.Drawing.ContentAlignment.TopRight, add.ToString());
                                mypdfpage1.Add(pdfaddtotal);
                                coltop = coltop + 20;
                                PdfTextArea pdf331 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 500, 30), System.Drawing.ContentAlignment.TopLeft, "B:ADDED ");
                                mypdfpage1.Add(pdf331);
                                coltop = coltop + 20;
                                Double add1 = 0;
                                string staffaddde = "select s.staff_code,s.staff_name,isnull(m.NetAdd,'0') NetAddAct from staffmaster s,monthlypay m,stafftrans t where s.staff_code =m.staff_code and t.staff_code =s.staff_code and t.staff_code =m.staff_code and s.join_date between '" + l.ToString() + "'and '" + o.ToString() + "' and m.fdate='" + l.ToString() + "' and m.tdate='" + o.ToString() + "' and s.College_Code='" + college_code + "' and t.latestrec =1 " + strtypequery + " " + strstreamquery + "";
                                DataSet dsnewstaff = da.select_method_wo_parameter(staffaddde, "text");

                                string getval1 = "different < 0 and college_code='" + college_code + "' and prevmon=0 and currmon<>'0' "; // poo 22.12.17 included prevmon and currmon
                                dsgetstaffnettdetails.Tables[0].DefaultView.RowFilter = getval1;
                                DataView dvaddtioninsalary = dsgetstaffnettdetails.Tables[0].DefaultView;
                                if (dvaddtioninsalary.Count > 0)
                                {
                                    int srno = 0;
                                    for (int an = 0; an < dvaddtioninsalary.Count; an++)
                                    {
                                        srno++;
                                        coltop = coltop + 20;
                                        if (coltop > 700)
                                        {
                                            mypdfpage1.SaveToDocument();
                                            coltop = 40;
                                            mypdfpage1 = mydoc.NewPage();
                                        }

                                        string staff = dvaddtioninsalary[an]["staff_name"].ToString();
                                        string staffcode = dvaddtioninsalary[an]["staff_code"].ToString();
                                        double diff = Convert.ToDouble(dvaddtioninsalary[an]["different"].ToString());
                                        diff = Math.Round(diff, 0, MidpointRounding.AwayFromZero);
                                        string strva = diff.ToString();
                                        strva = strva.Replace('-', '0');
                                        diff = Convert.ToDouble(strva);
                                        add1 = add1 + Convert.ToDouble(diff);

                                        PdfTextArea pdf415 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 10, coltop, 500, 30), System.Drawing.ContentAlignment.TopLeft, srno.ToString());
                                        mypdfpage1.Add(pdf415);
                                        PdfTextArea pdf45 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 500, 30), System.Drawing.ContentAlignment.TopLeft, staffcode + "        " + staff.ToString());
                                        mypdfpage1.Add(pdf45);
                                        PdfTextArea pdf451 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 500, 30), System.Drawing.ContentAlignment.TopRight, diff.ToString());
                                        mypdfpage1.Add(pdf451);


                                    }
                                }

                                //if (dsnewstaff.Tables[0].Rows.Count > 0)
                                //{
                                //    int srno = 0;
                                //    for (int an = 0; an < dsnewstaff.Tables[0].Rows.Count; an++)
                                //    {
                                //        coltop = coltop + 20;
                                //        if (coltop > 700)
                                //        {
                                //            mypdfpage1.SaveToDocument();
                                //            coltop = 40;
                                //            mypdfpage1 = mydoc.NewPage();
                                //        }
                                //        srno++;
                                //        string staff = dsnewstaff.Tables[0].Rows[an]["staff_name"].ToString();
                                //        string staffcode = dsnewstaff.Tables[0].Rows[an]["staff_code"].ToString();
                                //        Double diff = Convert.ToDouble(dsnewstaff.Tables[0].Rows[an]["NetAddAct"].ToString());
                                //        diff = Math.Round(diff, 0, MidpointRounding.AwayFromZero);
                                //        add1 = add1 + Convert.ToDouble(diff);
                                //        PdfTextArea pdf415 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 10, coltop, 500, 30), System.Drawing.ContentAlignment.TopLeft, srno.ToString());
                                //        mypdfpage1.Add(pdf415);
                                //        PdfTextArea pdf45 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 500, 30), System.Drawing.ContentAlignment.TopLeft, staffcode + "        " + staff.ToString());
                                //        mypdfpage1.Add(pdf45);
                                //        PdfTextArea pdf451 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 500, 30), System.Drawing.ContentAlignment.TopRight, diff.ToString());
                                //        mypdfpage1.Add(pdf451);
                                //    }
                                //}
                                else
                                {
                                    coltop = coltop + 20;
                                    PdfTextArea pdf451 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 500, 30), System.Drawing.ContentAlignment.TopRight, "0");
                                    mypdfpage1.Add(pdf451);
                                    coltop = coltop + 20;
                                }
                                coltop = coltop + 20;
                                PdfTextArea pdfaddtotal1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 300, 30), System.Drawing.ContentAlignment.TopRight, "Total");
                                mypdfpage1.Add(pdfaddtotal1);
                                pdfaddtotal = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 500, 30), System.Drawing.ContentAlignment.TopRight, add1.ToString());
                                mypdfpage1.Add(pdfaddtotal);
                                coltop = coltop + 30;
                                PdfTextArea pdfaddtotal1t = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 300, 30), System.Drawing.ContentAlignment.TopRight, "Total (A+B)");
                                mypdfpage1.Add(pdfaddtotal1t);
                                add1 = add1 + add;
                                PdfTextArea pdf451t = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 500, 30), System.Drawing.ContentAlignment.TopRight, add1.ToString());
                                mypdfpage1.Add(pdf451t);
                                coltop = coltop + 40;
                                PdfTextArea pdflesschar = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 500, 30), System.Drawing.ContentAlignment.TopLeft, "LESS ");
                                mypdfpage1.Add(pdflesschar);
                                coltop = coltop + 40;
                                PdfTextArea pdfshotclam = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 500, 30), System.Drawing.ContentAlignment.TopLeft, "A: DECREASE IN SALARY ");
                                mypdfpage1.Add(pdfshotclam);
                                double dedeuction = 0;
                                string getval2 = "different > 0 and college_code='" + college_code + "' and prevmon<>'0' and currmon<>'0' "; // poo 22.12.17 included prevmon and currmon
                                dsgetstaffnettdetails.Tables[0].DefaultView.RowFilter = getval2;
                                DataView dvdeduction = dsgetstaffnettdetails.Tables[0].DefaultView;
                                if (dvdeduction.Count > 0)
                                {
                                    int srno = 0;
                                    for (int an = 0; an < dvdeduction.Count; an++)
                                    {
                                        coltop = coltop + 20;
                                        if (coltop > 700)
                                        {
                                            mypdfpage1.SaveToDocument();
                                            coltop = 40;
                                            mypdfpage1 = mydoc.NewPage();
                                        }
                                        srno++;
                                        string staff = dvdeduction[an]["staff_name"].ToString();
                                        string staffcode = dvdeduction[an]["staff_code"].ToString();
                                        Double basic = Convert.ToDouble(dvdeduction[an]["different"].ToString());
                                        basic = Math.Round(basic, 0, MidpointRounding.AwayFromZero);
                                        dedeuction = dedeuction + Convert.ToDouble(basic);
                                        PdfTextArea pdf415 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 10, coltop, 500, 30), System.Drawing.ContentAlignment.TopLeft, srno.ToString());
                                        mypdfpage1.Add(pdf415);
                                        PdfTextArea pdf45 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 500, 30), System.Drawing.ContentAlignment.TopLeft, staffcode + "        " + staff.ToString());
                                        mypdfpage1.Add(pdf45);
                                        PdfTextArea pdf451 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 500, 30), System.Drawing.ContentAlignment.TopRight, basic.ToString());
                                        mypdfpage1.Add(pdf451);
                                    }
                                }

                                //if (newlyleft > 0)
                                //{
                                //    int srno = 0;
                                //    //string getval = "different > 0"; // poo
                                //    string getval = "different > 0 and college_code='" + college_code + "'"; // poo
                                //    dsgetstaffnettdetails.Tables[0].DefaultView.RowFilter = getval;
                                //    DataView dvaddtion = dsgetstaffnettdetails.Tables[0].DefaultView;
                                //    for (int an = 0; an < dvaddtion.Count; an++)
                                //    {
                                //        coltop = coltop + 20;
                                //        if (coltop > 700)
                                //        {
                                //            mypdfpage1.SaveToDocument();
                                //            coltop = 40;
                                //            mypdfpage1 = mydoc.NewPage();
                                //        }
                                //        srno++;
                                //        string staff = dvaddtion[an]["staff_name"].ToString();
                                //        string staffcode = dvaddtion[an]["staff_code"].ToString();
                                //        Double basic = Convert.ToDouble(dvaddtion[an]["different"].ToString());
                                //        basic = Math.Round(basic, 0, MidpointRounding.AwayFromZero);
                                //        dedeuction = dedeuction + Convert.ToDouble(basic);
                                //        PdfTextArea pdf415 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 10, coltop, 500, 30), System.Drawing.ContentAlignment.TopLeft, srno.ToString());
                                //        mypdfpage1.Add(pdf415);
                                //        PdfTextArea pdf45 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 500, 30), System.Drawing.ContentAlignment.TopLeft, staffcode + "        " + staff.ToString());
                                //        mypdfpage1.Add(pdf45);
                                //        PdfTextArea pdf451 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 500, 30), System.Drawing.ContentAlignment.TopRight, basic.ToString());
                                //        mypdfpage1.Add(pdf451);
                                //    }
                                //}
                                coltop = coltop + 30;
                                PdfTextArea pdfshctotal = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop - 10, 300, 30), System.Drawing.ContentAlignment.TopRight, "Total");
                                mypdfpage1.Add(pdfshctotal);
                                pdfshctotal = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop - 10, 500, 30), System.Drawing.ContentAlignment.TopRight, dedeuction.ToString());
                                mypdfpage1.Add(pdfshctotal);
                                coltop = coltop + 30;
                                PdfTextArea pdfletchar = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, 500, 30), System.Drawing.ContentAlignment.TopLeft, "B: LEFT ");
                                mypdfpage1.Add(pdfletchar);
                                coltop = coltop + 10;
                                double dedeuctionleft = 0;
                                DataSet dsrelive = da.select_method_wo_parameter(strrelivestaffdetails, "Text");

                                string getval3 = "different > 0 and college_code='" + college_code + "' and prevmon<>'0' and currmon=0 "; // poo 22.12.17 included prevmon and currmon
                                dsgetstaffnettdetails.Tables[0].DefaultView.RowFilter = getval3;
                                DataView dvdeduction1 = dsgetstaffnettdetails.Tables[0].DefaultView;
                                if (dvdeduction1.Count > 0)
                                {
                                    int srno = 0;
                                    for (int an = 0; an < dvdeduction1.Count; an++)
                                    {
                                        coltop = coltop + 20;
                                        if (coltop > 700)
                                        {
                                            mypdfpage1.SaveToDocument();
                                            coltop = 40;
                                            mypdfpage1 = mydoc.NewPage();
                                        }
                                        srno++;


                                        string staff = dvdeduction1[an]["staff_name"].ToString();
                                        string staffcode = dvdeduction1[an]["staff_code"].ToString();
                                        Double diff = Convert.ToDouble(dvdeduction1[an]["different"].ToString());
                                        diff = Math.Round(diff, 0, MidpointRounding.AwayFromZero);
                                        dedeuctionleft = dedeuctionleft + Convert.ToDouble(diff);
                                        PdfTextArea pdf415 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 10, coltop - 10, 500, 30), System.Drawing.ContentAlignment.TopLeft, srno.ToString());
                                        mypdfpage1.Add(pdf415);
                                        PdfTextArea pdf45 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop - 10, 500, 30), System.Drawing.ContentAlignment.TopLeft, staffcode + "        " + staff.ToString());
                                        mypdfpage1.Add(pdf45);
                                        PdfTextArea pdf451 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop - 10, 500, 30), System.Drawing.ContentAlignment.TopRight, diff.ToString());
                                        mypdfpage1.Add(pdf451);
                                    }
                                }


                                //if (dsrelive.Tables[0].Rows.Count > 0)
                                //{
                                //    int srno = 0;
                                //    for (int an = 0; an < dsrelive.Tables[0].Rows.Count; an++)
                                //    {
                                //        coltop = coltop + 20;
                                //        if (coltop > 700)
                                //        {
                                //            mypdfpage1.SaveToDocument();
                                //            coltop = 40;
                                //            mypdfpage1 = mydoc.NewPage();
                                //        }
                                //        srno++;
                                //        string staff = dsrelive.Tables[0].Rows[an]["staff_name"].ToString();
                                //        string staffcode = dsrelive.Tables[0].Rows[an]["staff_code"].ToString();
                                //        Double diff = Convert.ToDouble(dsrelive.Tables[0].Rows[an]["relived"].ToString());
                                //        diff = Math.Round(diff, 0, MidpointRounding.AwayFromZero);
                                //        dedeuctionleft = dedeuctionleft + Convert.ToDouble(diff);
                                //        PdfTextArea pdf415 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 10, coltop - 10, 500, 30), System.Drawing.ContentAlignment.TopLeft, srno.ToString());
                                //        mypdfpage1.Add(pdf415);
                                //        PdfTextArea pdf45 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop - 10, 500, 30), System.Drawing.ContentAlignment.TopLeft, staffcode + "        " + staff.ToString());
                                //        mypdfpage1.Add(pdf45);
                                //        PdfTextArea pdf451 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop - 10, 500, 30), System.Drawing.ContentAlignment.TopRight, diff.ToString());
                                //        mypdfpage1.Add(pdf451);
                                //    }
                                //}
                                else
                                {
                                    coltop = coltop + 20;
                                    PdfTextArea pdf451 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop - 10, 500, 30), System.Drawing.ContentAlignment.TopRight, "0");
                                    mypdfpage1.Add(pdf451);
                                    coltop = coltop + 20;
                                }
                                coltop = coltop + 30;
                                PdfTextArea pdfleftotal = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop - 10, 300, 30), System.Drawing.ContentAlignment.TopRight, "Total");
                                mypdfpage1.Add(pdfleftotal);
                                pdfleftotal = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop - 10, 500, 30), System.Drawing.ContentAlignment.TopRight, dedeuctionleft.ToString());
                                mypdfpage1.Add(pdfleftotal);
                                coltop = coltop + 30;
                                dedeuctionleft = dedeuctionleft + dedeuction;
                                PdfTextArea pdfdedtotal = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop - 10, 300, 30), System.Drawing.ContentAlignment.TopRight, "Total (A+B)");
                                mypdfpage1.Add(pdfdedtotal);
                                pdfdedtotal = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop - 10, 500, 30), System.Drawing.ContentAlignment.TopRight, dedeuctionleft.ToString());
                                mypdfpage1.Add(pdfdedtotal);


                                PdfTextArea pdfnet = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, -80, coltop + 20, 500, 30), System.Drawing.ContentAlignment.TopRight, "NET INCREASE/DECREASE");//delsis25
                                mypdfpage1.Add(pdfnet);
                                PdfTextArea pdfdi = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop + 20, 500, 30), System.Drawing.ContentAlignment.TopRight, Convert.ToString(totdiff));
                                mypdfpage1.Add(pdfdi);

                                //int amount = 0;
                                //yx = y + 140;
                                //int zy = 40;
                                //double add = 0;
                                //string basic = "";
                                //if (ds4.Tables[0].Rows.Count > 0)
                                //{
                                //    for (int s = 0; s < ds4.Tables[0].Rows.Count; s++)
                                //    {
                                //        yx = yx + 20;
                                //        if (yx > 700)
                                //        {
                                //            mypdfpage1.SaveToDocument();
                                //            yx = 40;
                                //            mypdfpage1 = mydoc.NewPage();
                                //        }
                                //        string staff = ds4.Tables[0].Rows[s]["staff_name"].ToString();
                                //        string staffcode = ds4.Tables[0].Rows[s]["staff_code"].ToString();
                                //        basic = ds4.Tables[0].Rows[s]["BasicInc"].ToString();
                                //        add = add + Convert.ToDouble(basic);
                                //        PdfTextArea pdf45 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx - 10, 500, 30), System.Drawing.ContentAlignment.TopLeft, staffcode + "        " + staff.ToString());
                                //        mypdfpage1.Add(pdf45);
                                //        PdfTextArea pdf451 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx - 10, 500, 30), System.Drawing.ContentAlignment.TopRight, basic.ToString());
                                //        mypdfpage1.Add(pdf451);
                                //    }
                                //    //if (Convert.ToString(basic) == 0)
                                //    //{
                                //    //}
                                //}
                                //else
                                //{
                                //    yx = yx + 20;
                                //    if (yx > 700)
                                //    {
                                //        mypdfpage1.SaveToDocument();
                                //        yx = 40;
                                //        mypdfpage1 = mydoc.NewPage();
                                //    }
                                //    PdfTextArea pdf45 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx, 500, 30), System.Drawing.ContentAlignment.TopRight, Convert.ToString(amount));
                                //    mypdfpage1.Add(pdf45);
                                //}
                                //PdfTextArea pdf37 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 10, 500, 30), System.Drawing.ContentAlignment.TopRight, "---------");
                                //mypdfpage1.Add(pdf37);
                                //PdfTextArea pdf34 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 20, 500, 30), System.Drawing.ContentAlignment.TopLeft, "TOTAL");
                                //PdfTextArea pdf36 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 30, 500, 30), System.Drawing.ContentAlignment.TopRight, "---------");
                                //PdfTextArea pdf341 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 20, 500, 30), System.Drawing.ContentAlignment.TopRight, Convert.ToString(add));
                                //PdfTextArea pdf35 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 50, 500, 30), System.Drawing.ContentAlignment.TopLeft, "B:ADDED IN " + monthtext.ToString().ToUpper() + " " + year_value_bind + " PAY BILL");
                                //mypdfpage1.Add(pdf34);
                                //mypdfpage1.Add(pdf341);
                                //mypdfpage1.Add(pdf35);
                                //mypdfpage1.Add(pdf36);
                                //double sk = 0;
                                //if (ds5.Tables[0].Rows.Count > 0)
                                //{
                                //    for (int sn = 0; sn < ds5.Tables[0].Rows.Count; sn++)
                                //    {
                                //        if (yx > 550)
                                //        {
                                //            mypdfpage1.SaveToDocument();
                                //            yx = 40;
                                //            mypdfpage1 = mydoc.NewPage();
                                //        }
                                //        yx = yx + 20;
                                //        string staff1 = ds5.Tables[0].Rows[sn]["staff_name"].ToString();
                                //        string staffcode = ds5.Tables[0].Rows[sn]["staff_code"].ToString();
                                //        string add1 = ds5.Tables[0].Rows[sn]["NetAddAct"].ToString();
                                //        sk = sk + Convert.ToDouble(add1);
                                //        PdfTextArea pdf46 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 50, 500, 30), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(staffcode + "        " + staff1.ToString()));
                                //        mypdfpage1.Add(pdf46);
                                //        PdfTextArea pdf452 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 50, 500, 30), System.Drawing.ContentAlignment.TopRight, Convert.ToString(Math.Round(Convert.ToDouble(add1))));
                                //        mypdfpage1.Add(pdf452);
                                //    }
                                //}
                                //else
                                //{
                                //    if (yx > 550)
                                //    {
                                //        mypdfpage1.SaveToDocument();
                                //        yx = 40;
                                //        mypdfpage1 = mydoc.NewPage();
                                //    }
                                //    yx = yx + 20;
                                //    PdfTextArea pdf451 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 40, 500, 30), System.Drawing.ContentAlignment.TopRight, Convert.ToString(Math.Round(Convert.ToDouble(amount))));
                                //    mypdfpage1.Add(pdf451);
                                //}
                                //double sk1 = 0;
                                //if (sk > add)
                                //{
                                //    sk1 = sk + add;
                                //}
                                //else
                                //{
                                //    sk1 = add + sk;
                                //}
                                //int sc = 0;
                                //yx = yx + 20;
                                //PdfTextArea pdf42 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 90, 500, 30), System.Drawing.ContentAlignment.TopRight, Convert.ToString(Math.Round(Convert.ToDouble(sk1))));
                                //PdfTextArea pdf41 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 60, 500, 30), System.Drawing.ContentAlignment.TopRight, Convert.ToString(Math.Round(Convert.ToDouble(sk))));
                                //PdfTextArea pdf38 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 60, 500, 30), System.Drawing.ContentAlignment.TopLeft, "TOTAL");
                                //PdfTextArea pdf43 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 90, 500, 30), System.Drawing.ContentAlignment.TopLeft, "TOTAL(A+B)");
                                //PdfTextArea pdf392 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 100, 500, 30), System.Drawing.ContentAlignment.TopRight, "--------");
                                //PdfTextArea pdf391 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 50, 500, 30), System.Drawing.ContentAlignment.TopRight, "--------");
                                //PdfTextArea pdf40 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 70, 500, 30), System.Drawing.ContentAlignment.TopRight, "--------");
                                //mypdfpage1.Add(pdf392);
                                //mypdfpage1.Add(pdf38);
                                //mypdfpage1.Add(pdf391);
                                //mypdfpage1.Add(pdf40);
                                //mypdfpage1.Add(pdf41);
                                //mypdfpage1.Add(pdf42);
                                //mypdfpage1.Add(pdf43);
                                //PdfTextArea pdf44 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 160, 500, 30), System.Drawing.ContentAlignment.TopLeft, "LESS");
                                //PdfTextArea pdf50 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 180, 500, 30), System.Drawing.ContentAlignment.TopLeft, "A:SHORT CLAIMS");
                                //PdfTextArea pdf51 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 180, 500, 30), System.Drawing.ContentAlignment.TopRight, Convert.ToString(Math.Round(Convert.ToDouble(sc))));
                                //PdfTextArea pdf52 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 210, 500, 30), System.Drawing.ContentAlignment.TopLeft, "TOTAL");
                                //PdfTextArea pdf53 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 200, 500, 30), System.Drawing.ContentAlignment.TopRight, "--------");
                                //PdfTextArea pdf54 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 215, 500, 30), System.Drawing.ContentAlignment.TopRight, "--------");
                                //PdfTextArea pdf55 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 210, 500, 30), System.Drawing.ContentAlignment.TopRight, Convert.ToString(Math.Round(Convert.ToDouble(sc))));
                                //PdfTextArea pdf56 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 230, 500, 30), System.Drawing.ContentAlignment.TopLeft, "B:LEFT");
                                //mypdfpage1.Add(pdf44);
                                //mypdfpage1.Add(pdf50);
                                //mypdfpage1.Add(pdf51);
                                //mypdfpage1.Add(pdf52);
                                //mypdfpage1.Add(pdf53);
                                //mypdfpage1.Add(pdf54);
                                //mypdfpage1.Add(pdf55);
                                //mypdfpage1.Add(pdf56);
                                //double sk2 = 0;
                                //if (ds6.Tables[0].Rows.Count > 0)
                                //{
                                //    for (int sn1 = 0; sn1 < ds6.Tables[0].Rows.Count; sn1++)
                                //    {
                                //        if (yx >= 550)
                                //        {
                                //            mypdfpage1.SaveToDocument();
                                //            yx = -160;
                                //            mypdfpage1 = mydoc.NewPage();
                                //        }
                                //        yx = yx + 20;
                                //        string staff2 = ds6.Tables[0].Rows[sn1]["staff_name"].ToString();
                                //        string staffcode = ds6.Tables[0].Rows[sn1]["staff_code"].ToString();
                                //        string add2 = ds6.Tables[0].Rows[sn1]["netaddact"].ToString();
                                //        PdfTextArea pdf47 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 220, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(staffcode + "        " + staff2.ToString()));
                                //        mypdfpage1.Add(pdf47);
                                //        PdfTextArea pdf453 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 230, 500, 30), System.Drawing.ContentAlignment.TopRight, Convert.ToString(Math.Round(Convert.ToDouble(add2))));
                                //        mypdfpage1.Add(pdf453);
                                //        sk2 = sk2 + Convert.ToDouble(add2);
                                //    }
                                //}
                                //else
                                //{
                                //    int am = 0;
                                //    if (yx > 550)
                                //    {
                                //        mypdfpage1.SaveToDocument();
                                //        yx = -180;
                                //        mypdfpage1 = mydoc.NewPage();
                                //    }
                                //    yx = yx + 20;
                                //    PdfTextArea pdf450 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 240, 500, 30), System.Drawing.ContentAlignment.TopRight, Convert.ToString(Math.Round(Convert.ToDouble(am))));
                                //    mypdfpage1.Add(pdf450);
                                //}
                                //double sk3 = 0;
                                //if (sk1 > sk2)
                                //{
                                //    sk3 = Convert.ToDouble(sk1) - Convert.ToDouble(sk2);
                                //}
                                //else
                                //{
                                //    sk3 = Convert.ToDouble(sk2) - Convert.ToDouble(sk1);
                                //}
                                //PdfTextArea pdf57 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 260, 500, 30), System.Drawing.ContentAlignment.TopLeft, "TOTAL");
                                //PdfTextArea pdf60 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 250, 500, 30), System.Drawing.ContentAlignment.TopRight, "--------");
                                //PdfTextArea pdf61 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 270, 500, 30), System.Drawing.ContentAlignment.TopRight, "--------");
                                //PdfTextArea pdf62 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 280, 500, 30), System.Drawing.ContentAlignment.TopRight, "--------");
                                //PdfTextArea pdf58 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 290, 500, 30), System.Drawing.ContentAlignment.TopLeft, "TOTAL(A+B)");
                                //PdfTextArea pdf71 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 290, 500, 30), System.Drawing.ContentAlignment.TopRight, Convert.ToString(Math.Round(Convert.ToDouble(sk2))));
                                //PdfTextArea pdf63 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 300, 500, 30), System.Drawing.ContentAlignment.TopRight, "--------");
                                //PdfTextArea pdf64 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 310, 500, 30), System.Drawing.ContentAlignment.TopRight, "--------");
                                //PdfTextArea pdf59 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 320, 500, 30), System.Drawing.ContentAlignment.TopLeft, "NET INCREASE/DECREASE");
                                //PdfTextArea pdf65 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 330, 500, 30), System.Drawing.ContentAlignment.TopRight, "--------");
                                //PdfTextArea pdf70 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 260, 500, 30), System.Drawing.ContentAlignment.TopRight, Convert.ToString(Math.Round(Convert.ToDouble(sk2))));
                                //PdfTextArea pdf72 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, yx + 320, 500, 30), System.Drawing.ContentAlignment.TopRight, Convert.ToString(Math.Round(Convert.ToDouble(sk3))));
                                //mypdfpage1.Add(pdf57);
                                //mypdfpage1.Add(pdf58);
                                //mypdfpage1.Add(pdf59);
                                //mypdfpage1.Add(pdf60);
                                //mypdfpage1.Add(pdf61);
                                //mypdfpage1.Add(pdf62);
                                //mypdfpage1.Add(pdf63);
                                //mypdfpage1.Add(pdf64);
                                //mypdfpage1.Add(pdf65);
                                //mypdfpage1.Add(pdf70);
                                //mypdfpage1.Add(pdf71);
                                //mypdfpage1.Add(pdf72);
                                mypdfpage1.SaveToDocument();
                                if (dsgetstaffnettdetails.Tables[0].Rows.Count > 0)
                                {
                                    string getgrossval = " college_code='" + college_code + "' ";//and prevmon<>0 and currmon<>'0'
                                    dsgetstaffnettdetails.Tables[0].DefaultView.RowFilter = getgrossval;
                                    grossaddition = dsgetstaffnettdetails.Tables[0].DefaultView;
                                    if (grossaddition.Count > 0)
                                    {
                                        mypdfpage2 = mydoc.NewPage();
                                        mypdfpage2.Add(pdf);
                                        mypdfpage2.Add(pdf01);
                                        coltop = 100;
                                        PdfTextArea pdfg1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, 60, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "GROSS DIFFERENCE FOR THE MONTH OF");
                                        PdfTextArea pdfg2 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 370, 60, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "" + monthtext.ToString().ToUpper() + "-" + "" + year_value_bind + "");

                                        previous = previous.Substring(0, 3);
                                        PdfTextArea pdfg20 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 360, coltop, 500, 30), System.Drawing.ContentAlignment.TopLeft, "" + previous.ToString().ToUpper() + "-" + "" + year1 + "");

                                        monthtext = monthtext.Substring(0, 3);
                                        PdfTextArea pdfg21 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 440, coltop, 500, 30), System.Drawing.ContentAlignment.TopLeft, "" + monthtext.ToString().ToUpper() + "-" + "" + year_value_bind + "");

                                        PdfTextArea pdfg10 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 10, coltop + 20, 500, 30), System.Drawing.ContentAlignment.TopLeft, "S.NO");

                                        PdfTextArea pdfg11 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 50, coltop + 20, 500, 30), System.Drawing.ContentAlignment.TopLeft, "STAFF CODE");


                                        PdfTextArea pdfg12 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 150, coltop + 20, 500, 30), System.Drawing.ContentAlignment.TopLeft, "STAFF NAME");


                                        PdfTextArea pdfg13 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 360, coltop + 20, 500, 30), System.Drawing.ContentAlignment.TopLeft, "GROSS");



                                        PdfTextArea pdfg14 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 440, coltop + 20, 500, 30), System.Drawing.ContentAlignment.TopLeft, "GROSS");


                                        PdfTextArea pdfg15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 500, coltop + 20, 500, 30), System.Drawing.ContentAlignment.TopLeft, "DIFFERENCE");

                                        mypdfpage2.Add(pdfg1);
                                        mypdfpage2.Add(pdfg2);//delsi2510
                                        mypdfpage2.Add(pdfg10);
                                        mypdfpage2.Add(pdfg11);
                                        mypdfpage2.Add(pdfg12);
                                        mypdfpage2.Add(pdfg13);
                                        mypdfpage2.Add(pdfg14);
                                        mypdfpage2.Add(pdfg15);
                                        mypdfpage2.Add(pdfg20);
                                        mypdfpage2.Add(pdfg21);
                                        coltop = coltop + 40;
                                        int srno1 = 0;
                                        double adddiff = 0;
                                        double addcur = 0;
                                        double addpre = 0;
                                        for (int an = 0; an < grossaddition.Count; an++)
                                        {
                                            Double prevsal = Convert.ToDouble(grossaddition[an]["prevmon"]);
                                            Double currmon = Convert.ToDouble(grossaddition[an]["currmon"]);
                                            prevsal = Math.Round(prevsal, 0, MidpointRounding.AwayFromZero);
                                            currmon = Math.Round(currmon, 0, MidpointRounding.AwayFromZero);
                                            addcur = addcur + currmon;
                                            addpre = addpre + prevsal;
                                            if (prevsal != 0 || currmon != 0)
                                            {
                                                coltop = coltop + 20;
                                                if (coltop > 700)
                                                {
                                                    mypdfpage2.SaveToDocument();
                                                    coltop = 40;
                                                    mypdfpage2 = mydoc.NewPage();
                                                }
                                                srno1++;
                                                string staff = grossaddition[an]["staff_name"].ToString();
                                                string staffcode = grossaddition[an]["staff_code"].ToString();

                                                double diff = Convert.ToDouble(grossaddition[an]["different"].ToString());
                                                diff = Math.Round(diff, 0, MidpointRounding.AwayFromZero);
                                                string strva = diff.ToString();
                                                strva = strva.Replace('-', '0');
                                                diff = Convert.ToDouble(strva);
                                                if (prevsal != 0 && currmon == 0)
                                                {
                                                    diff = 0;
                                                }
                                                adddiff = adddiff + Convert.ToDouble(diff);
                                                PdfTextArea pdfg4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 10, coltop - 10, 500, 30), System.Drawing.ContentAlignment.TopLeft, srno1.ToString());
                                                mypdfpage2.Add(pdfg4);
                                                PdfTextArea pdfg5 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 50, coltop - 10, 500, 30), System.Drawing.ContentAlignment.TopLeft, staffcode);
                                                mypdfpage2.Add(pdfg5);

                                                PdfTextArea pdf17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 150, coltop - 10, 500, 30), System.Drawing.ContentAlignment.TopLeft, staff.ToString());
                                                mypdfpage2.Add(pdf17);

                                                PdfTextArea pdfg8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, -110, coltop - 10, 500, 30), System.Drawing.ContentAlignment.TopRight, prevsal.ToString());
                                                mypdfpage2.Add(pdfg8);

                                                PdfTextArea pdfg7 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, -30, coltop - 10, 500, 30), System.Drawing.ContentAlignment.TopRight, currmon.ToString());
                                                mypdfpage2.Add(pdfg7);

                                                PdfTextArea pdfg6 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop - 10, 500, 30), System.Drawing.ContentAlignment.TopRight, diff.ToString());
                                                mypdfpage2.Add(pdfg6);
                                            }
                                        }

                                        coltop = coltop + 30;
                                        PdfTextArea pdfgtot = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, -60, coltop - 10, 300, 30), System.Drawing.ContentAlignment.TopRight, "TOTAL");
                                        mypdfpage2.Add(pdfgtot);

                                        PdfTextArea pdfgpretot = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 100, coltop - 10, 300, 30), System.Drawing.ContentAlignment.TopRight, addpre.ToString());

                                        mypdfpage2.Add(pdfgpretot);

                                        PdfTextArea pdfgcurtot = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 190, coltop - 10, 300, 30), System.Drawing.ContentAlignment.TopRight, addcur.ToString());

                                        mypdfpage2.Add(pdfgcurtot);


                                        PdfTextArea pdfgdifftot = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 250, coltop - 10, 300, 30), System.Drawing.ContentAlignment.TopRight, adddiff.ToString());

                                        mypdfpage2.Add(pdfgdifftot);


                                        mypdfpage2.SaveToDocument();
                                    }
                                }


                                string appPath = HttpContext.Current.Server.MapPath("~");
                                if (appPath != "")
                                {
                                    string szPath = appPath + "/Report/";
                                    string szFile = "HRReconciliation" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                                    mydoc.SaveToFile(szPath + szFile);
                                    Response.ClearHeaders();
                                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                    Response.ContentType = "application/pdf";
                                    Response.WriteFile(szPath + szFile);
                                }
                            }
                        }
                    }
                }
                if (flag == false)
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Any one Record\");", true);
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void btngen_Click(object sender, EventArgs e)
    {
        try
        {
            Font Fontbold = new Font("Book Antiqua", 12, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antiqua", 10, FontStyle.Regular);
            Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            bindbutn(mydoc, Fontbold, Fontsmall);
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
            da.sendErrorMail(ex, Convert.ToString(college_code), "HR_Reconciliation.aspx");
        }
    }
    protected void cbselectall_change(object sender, EventArgs e)
    {
        try
        {
            CheckBox ChkBoxHeader = (CheckBox)gridview1.HeaderRow.FindControl("cbselectall");
            foreach (GridViewRow row in gridview1.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("cbSelect");
                if (ChkBoxHeader.Checked == true)
                {
                    ChkBoxRows.Checked = true;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void Rbtformat2_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            btngo.Visible = true;
            Button1.Visible = false;
            ddlfrom.Enabled = false;
            ddlfromyear.Enabled = false;
            ddlto.Enabled = false;
            ddltoyear.Enabled = false;
            ddlsteam.Enabled = false;
            ddlstftype.Enabled = false;
            //Label1.Visible = false;
            ddlcollege.Enabled = true;
            txtdept.Enabled = true;
            txtdesign.Enabled = true;
            ddlmonth.Enabled = true;
            ddlyear.Enabled = true;
            txt_Category.Enabled = true;
            bindcollege();
            binddepartment();
            binddesign();
            bindMonthandYear();
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void Rbtformat1_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            btngo.Visible = false;
            Button1.Visible = true;
            ddlfrom.Enabled = true;
            ddlfromyear.Enabled = true;
            ddlto.Enabled = true;
            ddltoyear.Enabled = true;
            ddlsteam.Enabled = true;
            ddlstftype.Enabled = true;
            ddlcollege.Enabled = false;
            txtdept.Enabled = false;
            txtdesign.Enabled = false;
            ddlmonth.Enabled = false;
            ddlyear.Enabled = false;
            txt_Category.Enabled = false;
            Label1.Visible = false;
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void bindcollege()
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
            hast.Clear();
            hast.Add("column_field", columnfield.ToString());
            ds = da.select_method("bind_college", hast, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void btngo_OnClick(object sender, EventArgs e)
    {
        try
        {
            clear();
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            Printcontrol.Visible = false;
            int serial = 0;
            string staffname = "";
            string staffdesign = "";
            string staffdept = "";
            string staffcode = "";
            Label1.Visible = false;
            string salary = "";
            string presalary = "";
            int addition = 0;
            int deletion = 0;
            int totpresalary = 0;
            int totaddition = 0;
            int totdeletion = 0;
            int totsalary = 0;
            double roundsal = 0;
            double roundpresal = 0;
            fpspread.Sheets[0].RowHeader.Visible = false;
            fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.ForeColor = Color.Black;
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            fpspread.Sheets[0].DefaultStyle.Font.Bold = false;
            fpspread.Sheets[0].AutoPostBack = true;
            fpspread.CommandBar.Visible = false;
            fpspread.Sheets[0].ColumnCount = 9;
            fpspread.Sheets[0].ColumnHeader.RowCount = 1;
            fpspread.Sheets[0].RowCount = 0;
            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = Color.White;
            style2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            fpspread.Sheets[0].AutoPostBack = true;
            fpspread.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            fpspread.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            //  fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antique";
            fpspread.Sheets[0].Columns[0].Width = 30;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Name";
            // fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antique";
            fpspread.Sheets[0].Columns[1].Width = 180;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Department ";
            //  fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antique";
            fpspread.Sheets[0].Columns[2].Width = 130;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Designation";
            // fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antique";
            fpspread.Sheets[0].Columns[3].Width = 150;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Staff Category";//21.10.17
            // fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antique";
            fpspread.Sheets[0].Columns[4].Width = 150;
            //fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "";
            //fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Addition";
            // fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antique";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Deletion";
            // fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antique";
            //----------------month and year column add
            string month = ddlmonth.SelectedItem.Text;
            int monthvalue = Convert.ToInt32(ddlmonth.SelectedItem.Value);
            int yearprev = Convert.ToInt32(ddlyear.SelectedValue.ToString());
            string premonth = ddlmonth.SelectedItem.Text;
            fpspread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            fpspread.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            fpspread.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
            fpspread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
            int setpermonth = 0;
            int setperyear = 0;
            if (monthvalue == 1)
            {
                setpermonth = 12;
                setperyear = yearprev - 1;
            }
            else
            {
                setpermonth = monthvalue - 1;
                setperyear = yearprev;
            }
            if (monthvalue >= 2)
            {
                monthvalue = monthvalue - 2;
            }
            else
            {
                monthvalue = 11;
                yearprev = yearprev - 1;
            }
            string monthprev = ddlmonth.Items[monthvalue].Text.ToString();
            string year = ddlyear.SelectedValue.ToString();
            fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = monthprev + "-" + Convert.ToString(yearprev.ToString());
            fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Text = month + "-" + year.ToString();
            //---------------------------
            string strdept = "";
            for (int d = 0; d < chklsdept.Items.Count; d++)
            {
                if (chklsdept.Items[d].Selected == true)
                {
                    if (strdept == "")
                    {
                        strdept = chklsdept.Items[d].Value.ToString();
                    }
                    else
                    {
                        strdept = strdept + ',' + chklsdept.Items[d].Value.ToString();
                    }
                }
            }
            string strquerydept = "";
            if (strdept != "")
            {
                strquerydept = " and t.dept_code in(" + strdept + ")";
            }
            else
            {
                lblnorec.Visible = true;
                lblnorec.Text = "Please Select The Department And Then Proceed";
                return;
            }
            string strdesig = "";
            for (int d = 0; d < chklsdesign.Items.Count; d++)
            {
                if (chklsdesign.Items[d].Selected == true)
                {
                    if (strdesig == "")
                    {
                        strdesig = "'" + chklsdesign.Items[d].Value.ToString() + "'";
                    }
                    else
                    {
                        strdesig = strdesig + ",'" + chklsdesign.Items[d].Value.ToString() + "'";
                    }
                }
            }
            string strquerydesign = "";
            if (strdesig != "")
            {
                strquerydesign = " and t.desig_code in(" + strdesig + ")";
            }
            else
            {
                lblnorec.Visible = true;
                lblnorec.Text = "Please Select The Designation And Then Proceed";
                return;
            }
            string catagory = rs.GetSelectedItemsValueAsString(chklst_Category);
            if (!string.IsNullOrEmpty(catagory.Trim()))
                catagory = " and t.category_code in('" + catagory + "')";
            string SQL = "";
            SQL = SQL + " SELECT distinct m.Staff_Code,Staff_Name,d.Dept_Name,g.Desig_Name,p.netadd,m.staff_code,p.PayMonth,p.PayYear,t.dept_code,t.desig_code,c.category_name  FROM staffmaster M,stafftrans T,HrDept_Master D,desig_master G,monthlypay p ,staffcategorizer c WHERE c.category_code=t.category_code  and p.staff_code = t.staff_code and  p.staff_code = m.staff_code and  M.staff_code = t.staff_code  and t.desig_code= g.desig_code and t.dept_code=d.dept_code and m.college_code = g.collegeCode and t.latestrec = 1 and p.PayMonth='" + ddlmonth.SelectedValue.ToString() + "' and p.PayYear='" + ddlyear.SelectedItem.ToString() + "' " + strquerydesign + " " + strquerydept + " " + catagory + " and m.college_code ='" + ddlcollege.SelectedValue.ToString() + "'  order by t.dept_code,t.desig_code,m.Staff_Code";
            ds = da.select_method_wo_parameter(SQL, "Text");
            string SQl1 = " select m.netadd,m.staff_code,m.PayMonth,m.PayYear from monthlypay m,stafftrans t where m.staff_code=t.staff_code and t.latestrec=1 and m.PayMonth='" + setpermonth + "' and m.PayYear='" + setperyear + "' " + strquerydesign + " " + strquerydept + "" + catagory + "";
            ds1 = da.select_method_wo_parameter(SQl1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    fpspread.Sheets[0].RowCount++;
                    serial++;
                    staffname = Convert.ToString(ds.Tables[0].Rows[i]["Staff_Name"]);
                    staffdesign = Convert.ToString(ds.Tables[0].Rows[i]["Desig_Name"]);
                    staffdept = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                    staffcode = Convert.ToString(ds.Tables[0].Rows[i]["Staff_Code"]);
                    salary = Convert.ToString(ds.Tables[0].Rows[i]["netadd"]);
                    string categoryName = Convert.ToString(ds.Tables[0].Rows[i]["category_name"]);
                    roundsal = 0;
                    addition = 0;
                    deletion = 0;   //Added By Jeyaprakash
                    if (salary.Trim() != "" && salary.Trim() != "NULL")
                    {
                        roundsal = Math.Round(Convert.ToDouble(salary), 0, MidpointRounding.AwayFromZero);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Text = roundsal.ToString();
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                        // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antique";
                    }
                    else
                    {
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Text = "0";
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                    }
                    roundpresal = 0;
                    ds1.Tables[0].DefaultView.RowFilter = "staff_code='" + staffcode + "'";
                    DataView dvprevstaffsal = ds1.Tables[0].DefaultView;
                    if (dvprevstaffsal.Count > 0)
                    {
                        presalary = Convert.ToString(dvprevstaffsal[0]["netadd"]);
                        if (presalary.Trim() != "" && presalary.Trim() != "NUll")
                        {
                            roundpresal = Math.Round(Convert.ToDouble(presalary), 0, MidpointRounding.AwayFromZero);
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Text = roundpresal.ToString();
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                            //fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antique";
                        }
                        else
                        {
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Text = "0";
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                        }
                    }
                    if (Convert.ToInt32(roundsal) > Convert.ToInt32(roundpresal))
                    {
                        addition = Convert.ToInt32(roundsal) - Convert.ToInt32(roundpresal);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(addition);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                    }
                    else if (Convert.ToInt32(roundsal) < Convert.ToInt32(roundpresal))
                    {
                        deletion = Convert.ToInt32(roundpresal) - Convert.ToInt32(roundsal);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(deletion);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
                    }
                    else
                    {
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Text = "-";
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 8].Text = "-";
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
                    }
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = serial.ToString();
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = staffname.ToString();
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antique";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Text = staffdesign.ToString();
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antique";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = staffdept.ToString();
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;

                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(categoryName);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antique";
                    if (totpresalary == 0)
                    {
                        totpresalary = Convert.ToInt32(roundpresal);
                    }
                    else
                    {
                        totpresalary = Convert.ToInt32(roundpresal) + totpresalary;
                    }
                    if (totsalary == 0)
                    {
                        totsalary = Convert.ToInt32(roundsal);
                    }
                    else
                    {
                        totsalary = Convert.ToInt32(roundsal) + totsalary;
                    }
                    if (totaddition == 0)
                    {
                        totaddition = Convert.ToInt32(addition);
                    }
                    else
                    {
                        totaddition = Convert.ToInt32(addition) + totaddition;
                    }
                    if (totdeletion == 0)
                    {
                        totdeletion = Convert.ToInt32(deletion);
                    }
                    else
                    {
                        totdeletion = Convert.ToInt32(deletion) + totdeletion;  //Modified By Jeyaprakash
                    }
                }
                fpspread.Sheets[0].RowCount++;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].ForeColor = System.Drawing.Color.Brown;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antique";
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Text = "Total";
                //fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, 4);
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Text = totpresalary.ToString();
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].ForeColor = System.Drawing.Color.Brown;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antique";
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Text = totsalary.ToString();
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].ForeColor = System.Drawing.Color.Brown;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antique";
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Text = totaddition.ToString();
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].ForeColor = System.Drawing.Color.Brown;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Font.Bold = true;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antique";
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 8].Text = totdeletion.ToString();
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 8].ForeColor = System.Drawing.Color.Brown;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 8].Font.Bold = true;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antique";
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                fpspread.Visible = true;
                Print.Visible = true;
                Excel.Visible = true;
                lblreptname.Visible = true;
                txtreptname.Visible = true;
            }
            else
            {
                lblnorec.Visible = true;
                lblnorec.Text = "No Records Found";
                fpspread.Visible = false;
                Print.Visible = false;
                Excel.Visible = false;
                lblreptname.Visible = false;
                txtreptname.Visible = false;
            }
            fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
            lblnorec.Visible = true;
            da.sendErrorMail(ex, Convert.ToString(ddlcollege.SelectedValue), "HR_Reconciliation.aspx");
        }
    }
    protected void binddepartment()
    {
        try
        {
            txtdept.Text = "--Select--";
            chkdept.Checked = false;
            chklsdept.Items.Clear();
            string strquery = "";
            collegecode = ddlcollege.SelectedValue.ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (singleuser == "True")
            {
                strquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + collegecode + "') order by dept_name";
            }
            else
            {
                group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                strquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + collegecode + "') order by dept_name";
            }
            ds.Clear();
            ds = da.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklsdept.DataSource = ds;
                chklsdept.DataTextField = "dept_name";
                chklsdept.DataValueField = "dept_code";
                chklsdept.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void binddesign()
    {
        try
        {
            chklsdesign.Items.Clear();
            txtdesign.Text = "--Select--";
            chkdesign.Checked = false;
            collegecode = ddlcollege.SelectedValue.ToString();
            string strdesigquery = "select distinct desig_name,desig_code from desig_master where  collegeCode=" + collegecode + "";
            ds.Reset();
            ds.Dispose();
            ds = da.select_method_wo_parameter(strdesigquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklsdesign.DataSource = ds;
                chklsdesign.DataTextField = "desig_name";
                chklsdesign.DataValueField = "desig_code";
                chklsdesign.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    public void bindMonthandYear()
    {
        try
        {
            ddlmonth.Items.Clear();
            ddlmonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("January", "1"));
            ddlmonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("February", "2"));
            ddlmonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("March", "3"));
            ddlmonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("April", "4"));
            ddlmonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("May", "5"));
            ddlmonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("June", "6"));
            ddlmonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("July", "7"));
            ddlmonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("August", "8"));
            ddlmonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("September", "9"));
            ddlmonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("October", "10"));
            ddlmonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("November", "11"));
            ddlmonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("December", "12"));
            int year;
            year = Convert.ToInt16(DateTime.Today.Year);
            ddlyear.Items.Clear();
            for (int l = 0; l <= 7; l++)
            {
                ddlyear.Items.Add(Convert.ToString(year - l));
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void ddlyear_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }
    protected void ddlcollege_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        try
        {
            binddesign();
            binddepartment();
            bindcategory();
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void ddldegree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void ddldept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void ddlmonth_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void fpspread_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        fpspread.Visible = true;
    }
    protected void fpspread_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (cellclick == true)
            {
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    protected void Excel_OnClick(object sender, EventArgs e)
    {
        try
        {
            string report = txtreptname.Text;
            if (report.ToString().Trim() != "")
            {
                da.printexcelreport(fpspread, report);
                lblnorec.Visible = false;
            }
            else
            {
                Label1.Text = "Please Enter Your Report Name";
                Label1.Visible = true;
            }
            Print.Focus();
        }
        catch (Exception ex)
        {
            Label1.Visible = true;
            Label1.Text = ex.ToString();
        }
    }
    protected void Print_OnClick(object sender, EventArgs e)
    {
        try
        {
            fpspread.Visible = true;
            string degreedetails = "HR Reconciliation";
            string pagename = "Pay_Bill_Reconceliation.aspx";
            Printcontrol.loadspreaddetails(fpspread, pagename, degreedetails);
            Printcontrol.Visible = true;
            lblnorec.Visible = false;
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    public void clear()
    {
        Print.Visible = false;
        Excel.Visible = false;
        Label1.Visible = false;
        txtreptname.Visible = false;
        lblreptname.Visible = false;
        lblnorec.Visible = false;
        fpspread.Visible = false;
        btngen.Visible = false;
        gridview1.Visible = false;
        lblnorec.Visible = false;
    }
    protected void chkdept_ChekedChange(object sender, EventArgs e)
    {
        if (chkdept.Checked == true)
        {
            if (chklsdept.Items.Count > 0)
            {
                for (int i = 0; i < chklsdept.Items.Count; i++)
                {
                    chklsdept.Items[i].Selected = true;
                }
                txtdept.Text = "Dept(" + chklsdept.Items.Count + ")";
            }
            else
            {
                txtdept.Text = "--Select--";
                chkdept.Checked = false;
            }
        }
        else
        {
            for (int i = 0; i < chklsdept.Items.Count; i++)
            {
                chklsdept.Items[i].Selected = false;
            }
            txtdept.Text = "--Select--";
        }
    }
    protected void chklsdept_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtdept.Text = "--Select--";
        chkdept.Checked = false;
        int commcount = 0;
        for (int i = 0; i < chklsdept.Items.Count; i++)
        {
            if (chklsdept.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtdept.Text = "Dept(" + commcount.ToString() + ")";
            if (commcount == chklsdept.Items.Count)
            {
                chkdept.Checked = true;
            }
        }
    }
    protected void chkdesign_ChekedChange(object sender, EventArgs e)
    {
        if (chkdesign.Checked == true)
        {
            for (int i = 0; i < chklsdesign.Items.Count; i++)
            {
                chklsdesign.Items[i].Selected = true;
                txtdesign.Text = "Design(" + chklsdesign.Items.Count + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklsdesign.Items.Count; i++)
            {
                chklsdesign.Items[i].Selected = false;
            }
            txtdesign.Text = "--Select--";
        }
    }
    protected void chklsdesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtdesign.Text = "--Select--";
        chkdesign.Checked = false;
        int commcount = 0;
        for (int i = 0; i < chklsdesign.Items.Count; i++)
        {
            if (chklsdesign.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtdesign.Text = "Design(" + commcount.ToString() + ")";
            if (commcount == chklsdesign.Items.Count)
            {
                chkdesign.Checked = true;
            }
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


    protected void chklst_f1_Category_SelectedIndexChanged(object sender, EventArgs e)
    {

        int category = 0;
        for (int i = 0; i < chklst_f1_category.Items.Count; i++)
        {
            if (chklst_f1_category.Items[i].Selected == true)
            {
                category = category + 1;
                txt_f1_category.Text = "Category (" + category.ToString() + ")";
            }
        }
        if (category == 0)
        {
            txt_f1_category.Text = "---Select---";
            Cb_f1_category.Checked = false;
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

    protected void cb_f1_Category_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_f1_category.Checked == true)
        {
            for (int i = 0; i < chklst_f1_category.Items.Count; i++)
            {
                chklst_f1_category.Items[i].Selected = true;
                txt_f1_category.Text = "Category(" + (chklst_f1_category.Items.Count) + ")";
            }

        }
        else
        {
            for (int i = 0; i < chklst_f1_category.Items.Count; i++)
            {
                chklst_f1_category.Items[i].Selected = false;
                txt_f1_category.Text = "---Select---";
            }
        }
    }




    public void bindcategory()
    {
        try
        {
            chklst_f1_category.Visible = true;
            chklst_f1_category.Items.Clear();
            ds.Clear();
            string query1 = "select distinct category_code,category_name from staffcategorizer where college_code='" + college_code + "'";
            ds = d2.select_method_wo_parameter(query1, "text");
            //ds = da.loadcategory(Convert.ToString(ddlcollege.SelectedItem.Value));
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklst_f1_category.DataSource = ds;
                chklst_f1_category.DataTextField = "category_name";
                chklst_f1_category.DataValueField = "category_code";
                chklst_f1_category.DataBind();
            }
            for (int i = 0; i < chklst_f1_category.Items.Count; i++)
            {
                chklst_f1_category.Items[i].Selected = true;
            }
            if (chklst_f1_category.Items.Count > 5)
            {
                pnl_f1_category.Height = 250;
            }
            else
            {
                pnl_f1_category.Height = 100;
            }
        }
        catch (Exception e) { }
    }




    public void bindstaffcategory()
    {
        try
        {
            chklst_Category.Visible = true;
            chklst_Category.Items.Clear();
            ds.Clear();
            ds = da.loadcategory(Convert.ToString(ddlcollege.SelectedItem.Value));
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
    protected void lnk_btn_print_click(object sender, EventArgs e)
    {
        try
        {
            printpopup.Visible = true;


            string footerdetails = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='printRecons' and college_code='" + Convert.ToString(college_code) + "' and user_Code='" + Convert.ToString(Session["usercode"]) + "'");

            if (footerdetails.Trim() != "")
            {
                if (footerdetails.Trim().ToString() != "0")
                {
                    txt_print.Text = Convert.ToString(footerdetails);
                }
                else
                {
                    txt_print.Text = "";
                }
            }
            else
            {
                txt_print.Text = "";
            }


        }
        catch (Exception ex)
        {

        }
    }
    protected void btnsavePrint_Click(object sender, EventArgs e)
    {
        try
        {
            error.Visible = false;
            string GetName = Convert.ToString(txt_print.Text);

            string insquer = "if exists(select * from New_InsSettings where LinkName='printRecons' and user_code='" + Convert.ToString(Session["usercode"]) + "' and college_code='" + Convert.ToString(college_code) + "') update New_InsSettings set LinkValue='" + GetName + "' where LinkName='printRecons' and user_code='" + Convert.ToString(Session["usercode"]) + "' and college_code='" + Convert.ToString(college_code) + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('printRecons','" + GetName + "','" + Convert.ToString(Session["usercode"]) + "','" + Convert.ToString(college_code) + "')";

            int inscount = d2.update_method_wo_parameter(insquer, "Text");
            if (inscount > 0)
            {
                error.Visible = true;

            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void btnexitPrint_Click(object sender, EventArgs e)
    {
        try
        {
            printpopup.Visible = false;
            error.Visible = false;
        }
        catch (Exception ex)
        {

        }


    }
}