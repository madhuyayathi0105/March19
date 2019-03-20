using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using FarPoint.Web.Spread;
using System.Drawing;
using System.Globalization;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Gios.Pdf;

public partial class Transport_fees : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    Hashtable has = new Hashtable();
    DataTable dt = new DataTable();
    DataTable dt1 = new DataTable();
    ArrayList addr = new ArrayList();
    DataSet dss = new DataSet();
    DataRow dr = null;
    DataView dv = new DataView();

    string sql = "";
    string tot = "";
    int h = 0;
    string PageLogOut = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("TransportIndex"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/TransportMod/TransportIndex.aspx");
                return;
            }
        }
        lblvalidation.Visible = false;
        lblmsg.Visible = false;
        if (!IsPostBack)
        {
            setLabelText();
            clear();
            bindheader();
            bindledger();
            bindbatch();
            binddegree();
            branch();
            FromMonth();
            year();
            yearfrm();
            vehicle();
            ToMonth();
            status();
            txtpaidfrmdate.Attributes.Add("readonly", "readonly");
            txtpaidtodate.Attributes.Add("readonly", "readonly");
            txtpaidfrmdate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            txtpaidtodate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");

            string strdayflag;
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";

            string grouporusercode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }

            if (Session["usercode"] != "")
            {
                string Master1 = "select * from Master_Settings where " + grouporusercode + "";
                DataSet dssett = da.select_method_wo_parameter(Master1, "text");

                for (int s = 0; s < dssett.Tables[0].Rows.Count; s++)
                {
                    if (dssett.Tables[0].Rows[s]["settings"].ToString() == "Roll No" && dssett.Tables[0].Rows[s]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (dssett.Tables[0].Rows[s]["settings"].ToString() == "Register No" && dssett.Tables[0].Rows[s]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                }
            }
        }
    }
    public void FromMonth()
    {
        ddlfrommonth.Items.Clear();
        ddlfrommonth.Items.Insert(0, "--Select--");
        try
        {
            ddlfrommonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
            ddlfrommonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
            ddlfrommonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
            ddlfrommonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
            ddlfrommonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
            ddlfrommonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
            ddlfrommonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
            ddlfrommonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
            ddlfrommonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
            ddlfrommonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
            ddlfrommonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
            ddlfrommonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));

        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    public void ToMonth()
    {
        ddlToMonth.Items.Clear();
        ddlToMonth.Items.Insert(0, "--Select--");
        try
        {
            ddlToMonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
            ddlToMonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
            ddlToMonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
            ddlToMonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
            ddlToMonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
            ddlToMonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
            ddlToMonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
            ddlToMonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
            ddlToMonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
            ddlToMonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
            ddlToMonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
            ddlToMonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    public void year()
    {
        try
        {
            ddltoyear.Items.Clear();
            string yr = "select  distinct  year  from Fee_AllotMonthly";
            ds = da.select_method_wo_parameter(yr, "Text");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddltoyear.DataSource = ds;
                ddltoyear.DataValueField = "year";
                ddltoyear.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    public void yearfrm()
    {
        try
        {
            ddlfrmyear.Items.Clear();
            string yr = "select  distinct  year  from Fee_AllotMonthly";
            ds = da.select_method_wo_parameter(yr, "Text");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlfrmyear.DataSource = ds;
                ddlfrmyear.DataValueField = "year";
                ddlfrmyear.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    public void bindheader()
    {
        try
        {
            ddlHeader.Items.Clear();
            //ds = da.select_method_wo_parameter("select distinct a.header_name,a.header_id from acctheader a inner join Header_privileges h on a.header_id=h.Header_ID ,acctinfo f where  h.Rights=1 and a.acct_id=f.acct_id", "Text");

            ds = da.select_method_wo_parameter("select distinct headername,headerpk from FM_HeaderMaster ", "Text");
            int count = ds.Tables[0].Rows.Count;//modify by rajasekar 19/09/2018
            if (count > 0)
            {
                ddlHeader.DataSource = ds;
                ddlHeader.DataTextField = "headername";
                ddlHeader.DataValueField = "headerpk";
                ddlHeader.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    public void bindledger()
    {
        try
        {
            ButtonGo.Enabled = false;
            ddlLedger.Items.Clear();
            string head = ddlHeader.SelectedValue.ToString();
            if (head != "")
            {
                //ds = da.select_method_wo_parameter("select distinct fee_type,fee_code from fee_info where (fee_type not like 'Cash' and fee_type not like 'Income & Expenditure' and fee_type not like 'Misc' ) and fee_type not in(select distinct bankname from bank_master1) and header_id in ('" + head + "')and fee_code in (select fee_code from ledgerPrivilege where college_code='" + Session["collegecode"] + "' and user_code = " + Session["usercode"] + ")", "Text");

                ds = da.select_method_wo_parameter("select ledgerpk,ledgername from fm_ledgermaster where headerfk in('" + head + "')  and collegecode='" + Session["collegecode"] + "'", "Text");//modify by rajasekar 19/09/2018
            }
            else
            {
                //ds = da.select_method_wo_parameter("select distinct fee_type,fee_code from fee_info where (fee_type not like 'Cash' and fee_type not like 'Income & Expenditure' and fee_type not like 'Misc' ) and fee_type not in(select distinct bankname from bank_master1) and fee_code in (select fee_code from ledgerPrivilege where college_code='" + Session["collegecode"] + "' and user_code = " + Session["usercode"] + ")", "Text");

                ds = da.select_method_wo_parameter("select ledgerpk,ledgername from fm_ledgermaster where  collegecode='" + Session["collegecode"] + "'", "Text");//modify by rajasekar 19/09/2018
            }
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlLedger.DataSource = ds;
                ddlLedger.DataTextField = "ledgername";
                ddlLedger.DataValueField = "ledgerpk";
                ddlLedger.DataBind();
                ButtonGo.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    public void bindbatch()
    {
        try
        {
            ddlBatch.Items.Clear();
            ds = da.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlBatch.DataSource = ds;
                ddlBatch.DataTextField = "batch_year";
                ddlBatch.DataValueField = "batch_year";
                ddlBatch.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    public void binddegree()
    {
        try
        {
            ds.Clear();
            ds = da.BindDegree(Session["single_user"].ToString(), Session["group_code"].ToString(), Session["collegecode"].ToString(), Session["usercode"].ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlDegree.DataSource = ds;
                ddlDegree.DataTextField = "course_name";
                ddlDegree.DataValueField = "course_id";
                ddlDegree.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    public void branch()
    {
        try
        {
            ddlBranch.Items.Clear();
            string deg = ddlDegree.SelectedValue;
            if (deg != " ")
            {
                ds.Clear();
                string s = "select distinct degree.degree_code,department.dept_name,degree.Acronym,degree.Dept_Code  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + deg + "') and degree.college_code='" + Session["collegecode"] + "'and deptprivilages.Degree_code=degree.Degree_code";
                ds = da.select_method_wo_parameter(s, "Text");
            }
            else
            {
                ds.Clear();
                ds = da.select_method_wo_parameter("select distinct degree.degree_code,department.dept_name,degree.Acronym,degree.Dept_Code  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code", "Text");
            }
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlBranch.DataSource = ds;
                ddlBranch.DataTextField = "dept_name";
                ddlBranch.DataValueField = "degree_code";
                ddlBranch.DataBind();
                ddlBranch.Items.Insert(0, "--Select--");
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    public void status()
    {
        try
        {
            ddlstatus.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Paid"));
            ddlstatus.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Yet To Be Paid"));
            ddlstatus.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Both"));
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    protected void ddlHeader_selectchange(object sender, EventArgs e)
    {
        try
        {
            bindledger();
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    public void txtdate()
    {
        try
        {
            string fmonth = ddlfrommonth.SelectedItem.Value;
            string tmonth = ddlToMonth.SelectedItem.Value;
            string fdate = txtpaidfrmdate.Text;
            string tdate = txtpaidtodate.Text;

            string[] query1 = fdate.Split('/');
            string[] query2 = tdate.Split('/');
            string[] query3 = fdate.Split('0');
            string date1 = query1[1].ToString();
            string date2 = query2[1].ToString();
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void ddlLedger_selectchange(object sender, EventArgs e)
    {
        clear();
    }
    protected void ddlBatch_selectchange(object sender, EventArgs e)
    {
        try
        {
            binddegree();
            branch();
            clear();
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    protected void ddlDegree_selectchange(object sender, EventArgs e)
    {
        branch();
        clear();
    }
    protected void ddlBranch_selectchange(object sender, EventArgs e)
    {
        clear();
    }
    protected void ddlfrommonth_selectchange(object sender, EventArgs e)
    {
        clear();
    }
    protected void ddlfrmyear_selectchange(object sender, EventArgs e)
    {
        clear();
    }
    protected void ddlToMonth_selectchange(object sender, EventArgs e)
    {
        clear();
    }
    protected void ddltoyear_selectchange(object sender, EventArgs e)
    {
        clear();
    }
    protected void ddlstatus_selectchange(object sender, EventArgs e)
    {
        try
        {
            clear();
            //if (ddlstatus.SelectedValue == "Paid")
            //{
            //    txtpaidfrmdate.Enabled = true;
            //    txtpaidtodate.Enabled = true;
            //}
            //else if (ddlstatus.SelectedValue == "Yet To Be Paid")
            //{
            //    txtpaidfrmdate.Enabled = false;
            //    txtpaidtodate.Enabled = false;
            //}
            //else if (ddlstatus.SelectedValue == "Both")
            //{
            //    txtpaidfrmdate.Enabled = false;
            //    txtpaidtodate.Enabled = false;
            //}
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    protected void ddlpaidfrmdate_selectchange(object sender, EventArgs e)
    {
        clear();
    }
    protected void ddlpaidtodate_selectchange(object sender, EventArgs e)
    {
        clear();
    }
    protected void lnk_logout_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void ButtonGo_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            Boolean rowflag = false;
            Hashtable addmonth = new Hashtable();
            ArrayList addarray = new ArrayList();
            string RegisterNumber = "";
            int montval = 0;
            int frommonthvalue = 0;
            int tomonthvalue = 0;
            int ze = 0;
            string fromdate = "";
            string todate = "";
            DateTime dtfrom = new DateTime();
            DateTime dtto = new DateTime();
            string datebetween = "";
            //if (ddlstatus.SelectedItem.Text == "Paid")
            //{
                if (txtpaidfrmdate.Text != "" && txtpaidtodate.Text != "")
                {
                    fromdate = Convert.ToString(txtpaidfrmdate.Text);
                    string[] splitform = fromdate.Split('/');
                    dtfrom = Convert.ToDateTime(splitform[1] + "/" + splitform[0] + "/" + splitform[2]);
                    todate = Convert.ToString(txtpaidtodate.Text);
                    string[] splitto = todate.Split('/');
                    dtto = Convert.ToDateTime(splitto[1] + "/" + splitto[0] + "/" + splitto[2]);

                    if (dtfrom > dtto)
                    {
                        lblmsg.Visible = true;
                        lblmsg.Text = "Todate Must Be Greater or Equal To From Date";
                        return;
                    }
                    datebetween = " and cal_date between '" + dtfrom + "' and '" + dtto + "'";
                }
            //}
            if (rdbderee.Checked == true)
            {

                //frommonthvalue = Convert.ToInt32(ddlfrommonth.SelectedItem.Value);
                //tomonthvalue = Convert.ToInt32(ddlToMonth.SelectedItem.Value);
                //if (Convert.ToInt32(frommonthvalue) > Convert.ToInt32(tomonthvalue))
                //{
                //    g2btnexcel.Visible = false;
                //    g2btnprint.Visible = false;
                //    lblmsg.Visible = true;
                //    lblmsg.Text = "Please Select From And To date in Order";
                //    return;
                //}

                int sk3 = 0;
                FpSpread2.Sheets[0].AutoPostBack = true;
                FpSpread2.Sheets[0].RowCount = 0;
                FpSpread2.Sheets[0].ColumnCount = 8;
                FpSpread2.CommandBar.Visible = false;
                FpSpread2.Sheets[0].RowHeader.Visible = false;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";

                if (Session["Rollflag"].ToString() == "1")
                {
                    FpSpread2.Sheets[0].Columns[1].Visible = true;
                }
                else
                {
                    FpSpread2.Sheets[0].Columns[1].Visible = false;
                }

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Register Number";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";

                if (Session["Regflag"].ToString() == "1")
                {
                    FpSpread2.Sheets[0].Columns[2].Visible = true;
                }
                else
                {
                    FpSpread2.Sheets[0].Columns[2].Visible = false;
                }

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";

                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Month";
                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";

                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Demand";
                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Paid";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Balance";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Status";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";


                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Fee_Category";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";


                FpSpread2.Sheets[0].Columns[0].Visible = true;
                FpSpread2.Sheets[0].Columns[1].Visible = true;
                FpSpread2.Sheets[0].Columns[2].Visible = true;
                FpSpread2.Sheets[0].Columns[3].Visible = true;
                FpSpread2.Sheets[0].Columns[4].Visible = true;
                FpSpread2.Sheets[0].Columns[5].Visible = true;
                FpSpread2.Sheets[0].Columns[6].Visible = true;
                FpSpread2.Sheets[0].Columns[7].Visible = true;
                //FpSpread2.Sheets[0].Columns[8].Visible = true;

                FpSpread2.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                FpSpread2.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                FpSpread2.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                FpSpread2.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
                FpSpread2.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
                FpSpread2.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
                FpSpread2.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
                FpSpread2.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
                //FpSpread2.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;

                FpSpread2.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread2.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread2.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread2.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread2.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.None);

                string ledger = ddlLedger.SelectedItem.Value;
                string degcode = ddlBranch.SelectedValue;
                string rollno = "";
                string name = "";
                string appno = "";
                //sql = "select r.Roll_No,r.Reg_No,r.Stud_Name,fee_monallot,flag_status,r.Roll_Admit  from Fee_AllotMonthly m,fee_allot f,Registration r where m.roll_admit = f.roll_admit and m.fee_code = f.fee_code and m.fee_category = f.fee_category and  m.seatcate=f.seatcate and r.Roll_Admit=m.roll_admit and m.fee_code='" + ledger + "' and r.Batch_Year='" + ddlBatch.SelectedItem + "' and r.degree_code='" + degcode + "' and r.college_code='" + Session["collegecode"].ToString() + "'";

                sql = "select r.App_No,r.roll_no,r.reg_no,r.stud_name,r.roll_admit,f.paidamount,f.balamount,f.totalamount,f.FeeCategory  from Registration r,FT_FeeAllot f where f.App_No =r.App_No and  r.Batch_Year='" + ddlBatch.SelectedItem + "' and r.degree_code='" + degcode + "' and r.college_code='" + Session["collegecode"].ToString() + "' and HeaderFK='" + ddlHeader.SelectedValue + "' and LedgerFk='" + ddlLedger.SelectedValue + "' and r.cc=0 and r.delflag=0 and r.exam_flag!='debar'";//modify by rajasekar 19/09/2018
                
                dss = da.select_method_wo_parameter(sql, "text");
                DataSet dss1 = new DataSet();
                int sno = 0;
                string status = "";
                string paidamo = "";
                string balanceamo = "";
                string demand = "";
                for (int n = 0; n < dss.Tables[0].Rows.Count; n++)
                {
                    Boolean srnflag = false;
                    appno = dss.Tables[0].Rows[n]["App_No"].ToString();
                    rollno = dss.Tables[0].Rows[n]["Roll_No"].ToString();
                    name = dss.Tables[0].Rows[n]["stud_name"].ToString();
                    RegisterNumber = dss.Tables[0].Rows[n]["Reg_No"].ToString();
                    //string acname = "select credit,cal_date from dailytransaction where name like '" + rollno + "-" + name + "' and fee_code ='" + ledger + "' " + datebetween + "";
                    string acname = "select transdate,Debit from FT_FinDailyTransaction f where f.App_No ='" + appno + "' and f.headerfk in('" + ddlHeader.SelectedValue + "') and f.ledgerfk in('" + ddlLedger.SelectedValue + "') and f.TransDate between '" + dtfrom + "' and '" + dtto + "' ";//modify by rajasekar 19/09/2018
                    dss1 = da.select_method_wo_parameter(acname, "text");
                    string monthval = "";
                    if (dss1.Tables[0].Rows.Count > 0)
                    {
                        addmonth.Clear();
                        addarray.Clear();
                        if (dss.Tables[0].Rows.Count > 0)
                        {

                            //string feemonallot = dss.Tables[0].Rows[n]["fee_monallot"].ToString();
                            //string[] query1 = feemonallot.Split('/');
                            //frommonthvalue = Convert.ToInt32(ddlfrommonth.SelectedItem.Value);
                            //tomonthvalue = Convert.ToInt32(ddlToMonth.SelectedItem.Value);
                            //if (frommonthvalue <= tomonthvalue)
                            //{
                            //    montval = tomonthvalue - frommonthvalue;
                            //}
                            //else
                            //{
                            //    montval = frommonthvalue - tomonthvalue;
                            //}
                            //montval = montval + 1;
                            //int dectomonth = frommonthvalue - 1;
                            //tot = "select SUM (credit) as credit from dailytransaction where name like '" + rollno + "-" + name + "' and fee_code ='" + ledger + "'  " + datebetween + "";

                            //ds.Clear();
                            //ds = da.select_method_wo_parameter(tot, "Text");
                            //if (ds.Tables[0].Rows.Count > 0)
                            //{

                            //    dv = ds.Tables[0].DefaultView;
                            //    string creditvalue = Convert.ToString(dv[0]["credit"]);
                            //    int rk = 0;
                                //if (creditvalue.Trim() != "")
                                //{
                                //    rk = Convert.ToInt32(creditvalue);
                                    //for (int montname = 0; montname < montval; montname++)
                                    //{
                                    //    string sk = "";
                                    //    int sk1 = 0;
                                    //    int sk2 = 0;
                                    //    dectomonth++;
                                    //    if (dectomonth <= tomonthvalue)
                                    //    {

                                    //        string monthtext = bindmonthname(dectomonth);
                                    //        monthval = monthtext.ToString();
                                    //        foreach (string element in query1)
                                    //        {
                                    //            if (element.Trim() != "")
                                    //            {
                                    //                string[] item = element.Split(';');
                                    //                string months = item[0].ToString();
                                    //                string months1 = item[1].ToString();
                                    //                if (months == Convert.ToString(dectomonth))
                                    //                {
                                    //                    if (!addmonth.Contains(months))
                                    //                    {
                                    //                        addmonth.Add(months, months1);
                                    //                        addarray.Add(months);
                                    //                    }
                                    //                    demand = item[1].ToString();
                                    //                    sk = item[1].ToString();

                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                        //if (dv.Count > 0)
                                        //{

                                        //    if (addarray.Count == montname + 1)
                                        //    {
                                        //        sk = Convert.ToString(addmonth[addarray[montname]]);
                                        //    }
                                        //    else
                                        //    {
                                        //        sk = "0";
                                        //    }
                                        //    if (Convert.ToInt32(rk) >= Convert.ToInt32(Math.Round(Convert.ToDouble(sk))))
                                        //    {
                                        //        sk1 = Convert.ToInt32(rk) - Convert.ToInt32(Math.Round(Convert.ToDouble(sk)));

                                        //        sk2 = sk1;
                                        //        rk = sk2;
                                        //        status = "Paid";
                                        //        balanceamo = "0";
                                        //        paidamo = sk.ToString();
                                        //    }
                                        //    else
                                        //    {
                                        //        paidamo = rk.ToString();
                                        //        sk3 = Convert.ToInt32(Math.Round(Convert.ToDouble(sk))) - rk;
                                        //        rk = 0;
                                        //        status = "Un Paid";
                                        //        balanceamo = sk3.ToString();
                                        //    }
                                        //}

                                            string balanceAmount = dss.Tables[0].Rows[n]["balamount"].ToString();
                                            Double balAmount = 0;
                                            Double.TryParse(balanceAmount, out balAmount);
                                            status = (balAmount > 0) ? "Unpaid" : "Paid";
                                        Boolean setflag = false;
                                        if (ddlstatus.SelectedItem.Text == "Both")
                                        {
                                            setflag = true;
                                        }
                                        else if (ddlstatus.SelectedIndex.ToString() == "0")
                                        {
                                            if (status.Trim().ToLower() == "paid")
                                            {
                                                setflag = true;
                                            }
                                        }
                                        else if (ddlstatus.SelectedIndex.ToString() == "1")
                                        {
                                            if (status.Trim().ToLower() == "unpaid")
                                            {
                                                setflag = true;
                                            }
                                        }

                                        if (setflag == true)
                                        {
                                            rowflag = true;
                                            FpSpread2.Sheets[0].RowCount++;
                                            if (srnflag == false)
                                            {
                                                sno++;
                                                srnflag = true;
                                            }

                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = rollno;
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                            if (RegisterNumber == "")
                                            {
                                                RegisterNumber = "-";
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                            else
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                            }
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = RegisterNumber;
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = name;
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

                                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(monthval);
                                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;


                                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(demand);
                                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;

                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dss.Tables[0].Rows[n]["paidamount"].ToString());
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;

                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dss.Tables[0].Rows[n]["balamount"].ToString());
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;

                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(status);
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;

                                            string feecate1 = dss.Tables[0].Rows[n]["FeeCategory"].ToString();
                                            string feecate = da.GetFunction(" select TextVal from TextValTable where TextCriteria ='feeca' and TextCode='" + feecate1 + "'");
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(feecate);
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                        }
                                    //}
                                }
                           // }
                        //}
                    }
                }
                if (rowflag == true)
                {
                    FpSpread2.Visible = true;
                    //txt_excel.Visible = true;
                    //g2btnexcel.Visible = true;
                    //g2btnprint.Visible = true;
                    lblvalidation.Visible = false;
                    //lblrptname.Visible = true;
                }
                else
                {
                    lblmsg.Visible = true;
                    lblmsg.Text = "No Records Found";
                    //g2btnexcel.Visible = false;
                    //g2btnprint.Visible = false;
                    //txt_excel.Visible = false;
                    lblvalidation.Visible = false;
                    //lblrptname.Visible = false;
                    FpSpread2.Visible = false;
                }
            }
            if (rdbroute.Checked == true)
            {

                

                int sk3 = 0;

                FpSpread2.Sheets[0].AutoPostBack = true;
                FpSpread2.CommandBar.Visible = false;
                FpSpread2.Sheets[0].RowHeader.Visible = false;

                FpSpread2.Sheets[0].RowCount = 0;
                FpSpread2.Sheets[0].ColumnCount = 9;

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Route ID";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Left;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";


                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Register Number";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";


                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";


                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Month";
                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";

                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Demand";
                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Paid";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Balance";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Status";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";


                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Fee_Category";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";

                FpSpread2.Sheets[0].Columns[0].Width = 50;
                FpSpread2.Sheets[0].Columns[1].Width = 100;
                FpSpread2.Sheets[0].Columns[2].Width = 100;
                FpSpread2.Sheets[0].Columns[3].Width = 100;
                FpSpread2.Sheets[0].Columns[4].Width = 200;
                FpSpread2.Sheets[0].Columns[5].Width = 100;
                FpSpread2.Sheets[0].Columns[6].Width = 80;
                FpSpread2.Sheets[0].Columns[7].Width = 80;
                FpSpread2.Sheets[0].Columns[8].Width = 80;
                //FpSpread2.Sheets[0].Columns[9].Width = 100;

                FpSpread2.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread2.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread2.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread2.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread2.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);

                FpSpread2.Sheets[0].Columns[0].Visible = true;
                FpSpread2.Sheets[0].Columns[1].Visible = true;
                FpSpread2.Sheets[0].Columns[2].Visible = true;
                FpSpread2.Sheets[0].Columns[3].Visible = true;
                FpSpread2.Sheets[0].Columns[4].Visible = true;
                FpSpread2.Sheets[0].Columns[5].Visible = true;
                FpSpread2.Sheets[0].Columns[6].Visible = true;
                FpSpread2.Sheets[0].Columns[7].Visible = true;
                FpSpread2.Sheets[0].Columns[8].Visible = true;
                //FpSpread2.Sheets[0].Columns[9].Visible = true;

                FpSpread2.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                FpSpread2.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                FpSpread2.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                FpSpread2.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
                FpSpread2.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
                FpSpread2.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
                FpSpread2.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
                FpSpread2.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
                FpSpread2.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
                //FpSpread2.Sheets[0].Columns[9].VerticalAlign = VerticalAlign.Middle;

                string ledger = ddlLedger.SelectedItem.Value;
                string degcode = ddlBranch.SelectedValue;
                string rollno = "";
                string name = "";
                string appno = "";

                string mainvalue = "";
                if (cbl_routeid.Items.Count > 0)
                {
                    for (int j = 0; j < cbl_routeid.Items.Count; j++)
                    {
                        if (cbl_routeid.Items[j].Selected == true)
                        {
                            string subvalue = cbl_routeid.Items[j].Text;
                            if (mainvalue == "")
                            {
                                mainvalue = subvalue;
                            }
                            else
                            {
                                mainvalue = mainvalue + "'" + "," + "'" + subvalue;
                            }
                        }
                    }
                }

                //sql = "select r.Roll_No,r.Reg_No,r.Stud_Name,fee_monallot,flag_status,r.Roll_Admit,r.Bus_RouteID  from Fee_AllotMonthly m,fee_allot f,Registration r where m.roll_admit = f.roll_admit and m.fee_code = f.fee_code and m.fee_category = f.fee_category and  m.seatcate=f.seatcate and r.Roll_Admit=m.roll_admit and m.fee_code='" + ledger + "'  and r.college_code='" + Session["collegecode"].ToString() + "' and  Bus_RouteID in('" + mainvalue + "')";

                sql = "select r.App_No,r.roll_no,r.reg_no,r.stud_name,r.roll_admit,r.Bus_RouteID,f.paidamount,f.balamount,f.totalamount,f.FeeCategory  from Registration r,FT_FeeAllot f where f.App_No =r.App_No and  r.Bus_RouteID in('" + mainvalue + "')  and r.college_code='" + Session["collegecode"].ToString() + "' and HeaderFK='" + ddlHeader.SelectedValue + "' and LedgerFk='" + ddlLedger.SelectedValue + "' and r.cc=0 and r.delflag=0 and r.exam_flag!='debar'";//modify by rajasekar 19/09/2018
                ;
                string orderby_Setting = da.GetFunction("select value from master_Settings where settings='order_by'");
                if (orderby_Setting == "0")
                {
                    sql = sql + " ORDER BY r.Bus_RouteID,r.Roll_No";
                }
                else if (orderby_Setting == "1")
                {
                    sql = sql + " ORDER BY r.Bus_RouteID,r.Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    sql = sql + " ORDER BY r.Bus_RouteID,r.Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    sql = sql + " ORDER BY r.Bus_RouteID,r.Roll_No,r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    sql = sql + " ORDER BY r.Bus_RouteID,r.Roll_No,r.Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    sql = sql + " ORDER BY r.Bus_RouteID,r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    sql = sql + " ORDER BY r.Bus_RouteID,r.Roll_No,r.Stud_Name";
                }

                dss = da.select_method_wo_parameter(sql, "text");

                DataSet dss1 = new DataSet();
                int sno = 0;

                for (int n = 0; n < dss.Tables[0].Rows.Count; n++)
                {
                    appno = dss.Tables[0].Rows[n]["App_No"].ToString();
                    rollno = dss.Tables[0].Rows[n]["Roll_No"].ToString();
                    name = dss.Tables[0].Rows[n]["stud_name"].ToString();
                    RegisterNumber = dss.Tables[0].Rows[n]["Reg_No"].ToString();
                    //string acname = "select credit,cal_date from dailytransaction where name like '" + rollno + "-" + name + "' and fee_code ='" + ledger + "' " + datebetween + " ";
                    string acname = "select transdate,Debit from FT_FinDailyTransaction f where f.App_No ='" + appno + "' and f.headerfk in('" + ddlHeader.SelectedValue + "') and f.ledgerfk in('" + ddlLedger.SelectedValue + "') and f.TransDate between '" + dtfrom + "' and '" + dtto + "' ";//modify by rajasekar 19/09/2018
                    dss1 = da.select_method_wo_parameter(acname, "text");
                    addmonth.Clear();
                    addarray.Clear();
                    Boolean srnflag = false;
                    if (dss1.Tables[0].Rows.Count > 0)
                    {
                        //string feemonallot = dss.Tables[0].Rows[n]["fee_monallot"].ToString();
                        //string[] query1 = feemonallot.Split('/');
                        //frommonthvalue = Convert.ToInt32(ddlfrommonth.SelectedItem.Value);
                        //tomonthvalue = Convert.ToInt32(ddlToMonth.SelectedItem.Value);
                        //if (frommonthvalue <= tomonthvalue)
                        //{
                        //    montval = tomonthvalue - frommonthvalue;
                        //}
                        //else
                        //{
                        //    montval = frommonthvalue - tomonthvalue;
                        //}
                        //montval = montval + 1;
                        //ViewState["row_cont"] = Convert.ToString(montval);
                        //int dectomonth = frommonthvalue - 1;
                        //tot = "select SUM (credit) as credit from dailytransaction where name like '" + rollno + "-" + name + "' and fee_code ='" + ledger + "' " + datebetween + "";
                        //ds.Clear();
                        //ds = da.select_method_wo_parameter(tot, "Text");
                        //if (ds.Tables[0].Rows.Count > 0)
                        //{
                        //    dv = ds.Tables[0].DefaultView;
                        //    string credit = Convert.ToString(dv[0]["credit"]);
                        //    int rk = 0;
                        //    if (credit.Trim() != "")
                        //    {
                        //        Double getval = Convert.ToDouble(credit);
                        //        getval = Math.Round(getval, 0, MidpointRounding.AwayFromZero);
                        //        rk = Convert.ToInt32(getval);
                        //    }
                        //    else
                        //    {
                        //        rk = 0;
                        //    }
                        //    for (int montname = 0; montname < montval; montname++)
                        //    {
                        //        string sk = "";
                        //        int sk1 = 0;
                        //        int sk2 = 0;
                        //        string month = "";
                        //        string demandamo = "";
                        //        string paidamo = "";
                        //        string balanceamo = "";
                        //        string status = "";

                        //        dectomonth++;
                        //        if (dectomonth <= tomonthvalue)
                        //        {
                        //            string monthtext = bindmonthname(dectomonth);
                        //            month = monthtext.ToString();

                        //            foreach (string element in query1)
                        //            {
                        //                if (element.Trim() != "")
                        //                {
                        //                    string[] item = element.Split(';');
                        //                    string months = item[0].ToString();
                        //                    string months1 = item[1].ToString();
                        //                    if (months == Convert.ToString(dectomonth))
                        //                    {
                        //                        if (!addmonth.Contains(months))
                        //                        {
                        //                            addmonth.Add(months, months1);
                        //                            addarray.Add(months);
                        //                        }
                        //                        demandamo = item[1].ToString();
                        //                        sk = item[1].ToString();

                        //                    }
                        //                }
                        //            }
                        //        }

                        //        if (dv.Count > 0)
                        //        {
                        //            if (RegisterNumber != "")
                        //            {
                        //                if (addarray.Count == montname + 1)
                        //                {
                        //                    sk = Convert.ToString(addmonth[addarray[montname]]);
                        //                }
                        //                else
                        //                {
                        //                    sk = "0";
                        //                }
                        //                if (Convert.ToInt32(rk) >= Convert.ToInt32(Math.Round(Convert.ToDouble(sk))))
                        //                {
                        //                    sk1 = Convert.ToInt32(rk) - Convert.ToInt32(Math.Round(Convert.ToDouble(sk)));

                        //                    sk2 = sk1;
                        //                    rk = sk2;
                        //                    paidamo = sk;
                        //                    balanceamo = "0";
                        //                    status = "Paid";
                        //                }
                        //                else
                        //                {
                        //                    paidamo = rk.ToString();
                        //                    sk3 = Convert.ToInt32(Math.Round(Convert.ToDouble(sk))) - rk;
                        //                    rk = 0;
                        //                    balanceamo = sk3.ToString();
                        //                    status = "Un Paid";

                        //                }

                        //            }

                                    string status = "";
                                    string balanceAmount = dss.Tables[0].Rows[n]["balamount"].ToString();
                                    Double balAmount = 0;
                                    Double.TryParse(balanceAmount, out balAmount);
                                    status = (balAmount > 0) ? "Unpaid" : "Paid";
                                    Boolean setflag = false;
                                    if (ddlstatus.SelectedItem.Text == "Both")
                                    {
                                        setflag = true;
                                    }
                                    else if (ddlstatus.SelectedItem.Text == "Paid")
                                    {
                                        if (status == "Paid")
                                        {
                                            setflag = true;
                                        }
                                    }
                                    if (ddlstatus.SelectedItem.Text == "Yet To Be Paid")
                                    {
                                        if (status == "Unpaid")
                                        {
                                            setflag = true;
                                        }
                                    }
                                    if (setflag == true)
                                    {
                                        rowflag = true;
                                        FpSpread2.Sheets[0].RowCount++;
                                        if (srnflag == false)
                                        {
                                            sno++;
                                            srnflag = true;
                                        }
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = dss.Tables[0].Rows[n]["Bus_RouteID"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = rollno;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = RegisterNumber;
                                        if (RegisterNumber == "")
                                        {
                                            RegisterNumber = "-";
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else
                                        {
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                        }
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = name;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;

                                        //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = month;
                                        //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                        //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                        //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;


                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dss.Tables[0].Rows[n]["paidamount"].ToString());
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dss.Tables[0].Rows[n]["balamount"].ToString());
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Text = status;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;


                                        string feecate1 = dss.Tables[0].Rows[n]["FeeCategory"].ToString();
                                        string feecate = da.GetFunction(" select TextVal from TextValTable where TextCriteria ='feeca' and TextCode='" + feecate1 + "'");
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = feecate;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                    }
                                //}
                            //}
                        //}
                    }
                }
                if (rowflag == true)
                {
                    FpSpread2.Visible = true;
                    //txt_excel.Visible = true;
                    //g2btnexcel.Visible = true;
                    //g2btnprint.Visible = true;
                    lblvalidation.Visible = false;
                    //lblrptname.Visible = true;
                }
                else
                {
                    lblmsg.Visible = true;
                    lblmsg.Text = "No Records Found";
                    //g2btnexcel.Visible = false;
                    //g2btnprint.Visible = false;
                    //txt_excel.Visible = false;
                    lblvalidation.Visible = false;
                    //lblrptname.Visible = false;
                    FpSpread2.Visible = false;
                }
            }
            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    public string bindmonthname(int mon)
    {
        int value = mon;
        string textvalue = "";
        switch (value)
        {
            case 1:
                textvalue = "Jan";
                break;

            case 2:
                textvalue = "Feb";
                break;

            case 3:
                textvalue = "Mar";
                break;

            case 4:
                textvalue = "Apr";
                break;

            case 5:
                textvalue = "May";
                break;

            case 6:
                textvalue = "Jun";
                break;

            case 7:
                textvalue = "Jul";
                break;
            case 8:
                textvalue = "Aug";
                break;

            case 9:
                textvalue = "Sep";
                break;

            case 10:
                textvalue = "Oct";
                break;

            case 11:
                textvalue = "Nov";
                break;

            case 12:
                textvalue = "Dec";
                break;

        }
        return textvalue;
    }
    protected void btnexcel1_OnClick(object sender, EventArgs e)
    {
        try
        {
            string txt_name = Convert.ToString(txt_excel.Text);
            if (txt_name.Trim() != "")
            {

                da.printexcelreport(FpSpread2, txt_name);
                lblvalidation.Visible = false;
            }
            else
            {
                lblvalidation.Visible = true;
                lblvalidation.Text = "Please Enter Your Report Name";
                txt_excel.Focus();
            }

        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    protected void g1btnprint1_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (rdbderee.Checked == true)
            {
                string degreedetails = "Transport Fees Report" + '@' + "Batch: " + ddlBatch.SelectedItem.Text + "" + '@' + "Degree: " + ddlDegree.SelectedItem.Text + "" + '@' + "Branch: " + ddlBranch.SelectedItem.Text + "";
                string pagename = "Transport_fees.aspx";
                Printcontrol.loadspreaddetails(FpSpread2, pagename, degreedetails);
                Printcontrol.Visible = true;
            }
            if (rdbroute.Checked == true)
            {
                string degreedetails = "Transport Fees Report";
                string pagename = "Transport_fees.aspx";
                Printcontrol.loadspreaddetails(FpSpread2, pagename, degreedetails);
                Printcontrol.Visible = true;
            }

        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    protected void txtpaidfrmdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            //if (ddlstatus.SelectedItem.Text == "Yet To Be Paid")
            //{
            //    txtpaidfrmdate.Text = ("");
            //    txtpaidtodate.Text = ("");
            //    txtpaidfrmdate.Enabled = false;
            //    txtpaidtodate.Enabled = false;
            //}
            //else if (ddlstatus.SelectedItem.Text == "Both")
            //{
            //    txtpaidfrmdate.Text = ("");
            //    txtpaidtodate.Text = ("");
            //    txtpaidfrmdate.Enabled = false;
            //    txtpaidtodate.Enabled = false;
            //}
            string datefad = "";
            datefad = txtpaidfrmdate.Text.ToString();
            string[] split4 = datefad.Split(new Char[] { '/' });
            DateTime dt1 = Convert.ToDateTime(split4[1].ToString() + "/" + split4[0].ToString() + "/" + split4[2].ToString());
            if (dt1 > DateTime.Today)
            {
                lblmsg.Text = "You can not mark attendance for the date greater than today";
                lblmsg.Visible = true;

                txtpaidfrmdate.Text = ("");
                txtpaidtodate.Text = ("");
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    protected void txtpaidtodate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (txtpaidfrmdate.Text == "")
            {
                txtpaidtodate.Text = "";
                lblmsg.Visible = true;
                lblmsg.Text = "Enter from date first";
                txtpaidfrmdate.Text = ("");
                txtpaidtodate.Text = ("");
            }
            else
            {
                lblmsg.Visible = false;
                string datefad, dtfromad;
                string datefromad;
                string yr4, m4, d4;
                datefad = txtpaidfrmdate.Text.ToString();
                string[] split4 = datefad.Split(new Char[] { '/' });
                if (split4.Length == 3)
                {
                    datefromad = split4[0].ToString() + "/" + split4[1].ToString() + "/" + split4[2].ToString();
                    yr4 = split4[2].ToString();
                    m4 = split4[1].ToString();
                    d4 = split4[0].ToString();
                    dtfromad = m4 + "/" + d4 + "/" + yr4;

                    string date2ad;
                    string datetoad;
                    string yr5, m5, d5;
                    date2ad = txtpaidtodate.Text.ToString();
                    string[] split5 = date2ad.Split(new Char[] { '/' });
                    if (split5.Length == 3)
                    {
                        datetoad = split5[0].ToString() + "/" + split5[1].ToString() + "/" + split5[2].ToString();
                        yr5 = split5[2].ToString();
                        m5 = split5[1].ToString();
                        d5 = split5[0].ToString();
                        datetoad = m5 + "/" + d5 + "/" + yr5;
                        DateTime dt1 = Convert.ToDateTime(dtfromad);
                        DateTime dt2 = Convert.ToDateTime(datetoad);

                        TimeSpan ts = dt2 - dt1;

                        int days = ts.Days;
                        if (days < 0)
                        {

                            lblmsg.Text = "From Date Should Be Less Than To Date";
                            lblmsg.Visible = true;
                            //gridview1.Visible = false;
                            //g2btnexcel.Visible = false;
                            //g2btnprint.Visible = false;
                            txtpaidfrmdate.Text = ("");
                            txtpaidtodate.Text = ("");
                        }
                        if (dt1 > DateTime.Today)
                        {
                            lblmsg.Text = "You can not mark attendance for the date greater than today";
                            lblmsg.Visible = true;
                            //  gridview1.Visible = false;
                            //g2btnexcel.Visible = false;
                            //g2btnprint.Visible = false;
                            txtpaidfrmdate.Text = ("");
                            txtpaidtodate.Text = ("");
                        }
                        if (dt2 > DateTime.Today)
                        {
                            lblmsg.Text = "You can not mark attendance for the date greater than today";
                            lblmsg.Visible = true;
                            // gridview1.Visible = false;
                            //g2btnexcel.Visible = false;
                            //g2btnprint.Visible = false;
                            txtpaidfrmdate.Text = ("");
                            txtpaidtodate.Text = ("");
                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }


    protected void rdbdegree_Change(object sender, EventArgs e)
    {
        try
        {
            routeidspan.Visible = false;
            txt_routeid.Visible = false;
            panelrouteid.Visible = false;
            txt_vehicleid.Visible = false;
            panelvehicleid.Visible = false;
            vehicleidspan.Visible = false;
            ddlBranch.Enabled = true;
            ddlDegree.Enabled = true;
            ddlBatch.Enabled = true;

            clear();
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void rdbroute_Change(object sender, EventArgs e)
    {
        try
        {
            routeidspan.Visible = true;
            txt_routeid.Visible = true;
            FpSpread2.Visible = false;
            panelrouteid.Visible = true;
            txt_vehicleid.Visible = true;
            panelvehicleid.Visible = true;
            vehicleidspan.Visible = true;
            //g2btnexcel.Visible = false;
            //g2btnprint.Visible = false;
            //txt_excel.Visible = false;
            lblvalidation.Visible = false;
            //lblrptname.Visible = false;
            ddlBranch.Enabled = false;
            ddlDegree.Enabled = false;
            ddlBatch.Enabled = false;

            clear();
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    protected void cb_routeid_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_routeid.Checked == true)
            {

                for (int i = 0; i < cbl_routeid.Items.Count; i++)
                {
                    cbl_routeid.Items[i].Selected = true;
                    txt_routeid.Text = "Route ID(" + (cbl_routeid.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < cbl_routeid.Items.Count; i++)
                {
                    cbl_routeid.Items[i].Selected = false;
                    txt_routeid.Text = "---Select---";
                }

            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    protected void cbl_routeid_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int value = 0;
            for (int i = 0; i < cbl_routeid.Items.Count; i++)
            {
                if (cbl_routeid.Items[i].Selected == true)
                {
                    value = value + 1;

                    txt_routeid.Text = "Route ID(" + value.ToString() + ")";
                }
            }

            if (value == 0)
            {
                txt_routeid.Text = "---Select---";
            }
            cb_routeid.Checked = false;
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    public void bindroute()
    {
        try
        {
            string mainvalue = "";
            if (cbl_vehicleid.Items.Count > 0)
            {
                for (int i = 0; i < cbl_vehicleid.Items.Count; i++)
                {
                    if (cbl_vehicleid.Items[i].Selected == true)
                    {
                        string ve_id = cbl_vehicleid.Items[i].Value;
                        if (mainvalue == "")
                        {
                            mainvalue = "'" + ve_id + "'";
                        }
                        else
                        {
                            mainvalue = mainvalue + ",'" + ve_id + "'";
                        }
                    }

                }
            }
            if (mainvalue != "")
            {
                ds.Clear();
                string sqlquery = "select distinct Route_ID from routemaster where Veh_ID in(" + mainvalue + ") order by Route_ID";
                ds = da.select_method_wo_parameter(sqlquery, "txt");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        cbl_routeid.DataSource = ds;
                        cbl_routeid.DataTextField = "Route_ID";
                        cbl_routeid.DataValueField = "Route_ID";
                        cbl_routeid.DataBind();
                    }
                    if (cbl_routeid.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_routeid.Items.Count; i++)
                        {
                            cbl_routeid.Items[i].Selected = true;
                            txt_routeid.Text = "Route ID(" + cbl_routeid.Items.Count + ")";
                        }
                    }

                }
            }
            else
            {
                cbl_routeid.Items.Clear();
                txt_routeid.Text = "---Select---";

            }

        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void cb_vehicleid_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_vehicleid.Checked == true)
            {

                for (int i = 0; i < cbl_vehicleid.Items.Count; i++)
                {
                    cbl_vehicleid.Items[i].Selected = true;
                    txt_vehicleid.Text = "Vehicle ID(" + (cbl_vehicleid.Items.Count) + ")";
                }

            }
            else
            {
                for (int i = 0; i < cbl_vehicleid.Items.Count; i++)
                {
                    cbl_vehicleid.Items[i].Selected = false;
                    txt_vehicleid.Text = "---Select---";
                }

            }
            bindroute();
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    protected void cbl_vehicleid_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int value = 0;
            for (int i = 0; i < cbl_vehicleid.Items.Count; i++)
            {
                if (cbl_vehicleid.Items[i].Selected == true)
                {
                    value = value + 1;

                    txt_vehicleid.Text = "Vehicle ID(" + value.ToString() + ")";
                }
            }
            bindroute();
            if (value == 0)
            {
                txt_vehicleid.Text = "---Select---";
            }
            cb_vehicleid.Checked = false;
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    public void vehicle()
    {
        try
        {
            ds.Clear();
            ds = da.select_method_wo_parameter("select Veh_ID from vehicle_master order by Veh_ID asc", "txet");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_vehicleid.DataSource = ds;
                cbl_vehicleid.DataValueField = "Veh_ID";
                cbl_vehicleid.DataTextField = "Veh_ID";
                cbl_vehicleid.DataBind();

            }
            if (cbl_vehicleid.Items.Count > 0)
            {
                for (int i = 0; i < cbl_vehicleid.Items.Count; i++)
                {
                    cbl_vehicleid.Items[i].Selected = true;
                    txt_vehicleid.Text = "Vehicle ID(" + cbl_vehicleid.Items.Count + ")";
                }
            }
            bindroute();
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    public void clear()
    {
        //g2btnexcel.Visible = false;
        //g2btnprint.Visible = false;
        //txt_excel.Visible = false;
        lblvalidation.Visible = false;
        //lblrptname.Visible = false;
        FpSpread2.Visible = false;
        //txt_excel.Text = "";
    }

    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();
        lbl.Add(lblclg);
        //lbl.Add(lbl_stream);
        lbl.Add(lbldegree);
        lbl.Add(lblbranch);
        //lbl.Add(lbl_sem);
        fields.Add(0);
        // fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        //fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
    // last modified 22-10-2016 sudhagar

}