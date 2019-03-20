using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using AjaxControlToolkit;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Web.UI.DataVisualization.Charting;
using FarPoint.Web.Spread.Design;

public partial class newreport : System.Web.UI.Page
{
    string sql = "";

    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    Hashtable hat = new Hashtable();
    DataSet dsnew = new DataSet();
    DataSet dsrpt1totst = new DataSet();
    ArrayList commarray = new ArrayList();
    ArrayList religarray = new ArrayList();
    Hashtable addrelig = new Hashtable();
    Hashtable addcomm = new Hashtable();
    Hashtable allotrelig = new Hashtable();
    Hashtable allotcomm = new Hashtable();
    Hashtable grantrelig = new Hashtable();
    Hashtable grantcomm = new Hashtable();
    Hashtable grantallotrelig = new Hashtable();
    Hashtable grnatallotcomm = new Hashtable();
    string collegecode = "";
    string usercode = "";
    string columnfield = string.Empty;
    string singleuser = "";
    string group_user = "";
    Boolean cellclick = false;
    string course_id = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("StudentHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/StudentMod/StudentHome.aspx");
                    return;
                }
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            if (!IsPostBack)
            {
                setLabelText();
                loadcollege();
                type();
                batch();
                report();
                rptprint.Visible = false;
                fpspread.Visible = false;
                rdbdepartemntwise.Checked = true;
                rdbdepartemntwise.Enabled = false;
                rdbManagementwise.Enabled = false;
            }
        }
        catch (Exception ex) { }
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




        lbl.Add(lblcollege);



        fields.Add(0);




        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }
    public void bindheaderspread1()
    {

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
        fpspread.Sheets[0].RowCount = 0;
        fpspread.Sheets[0].ColumnCount = 8;
        fpspread.Sheets[0].ColumnHeader.RowCount = 1;
        fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 30;
        fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        fpspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
        fpspread.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Center;
        fpspread.Sheets[0].ColumnHeader.Columns[1].HorizontalAlign = HorizontalAlign.Center;
        fpspread.Sheets[0].ColumnHeader.Columns[2].HorizontalAlign = HorizontalAlign.Center;


        fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";
        fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Dept";
        fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Total Strength";
        fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Applied";
        fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Fee Confirm";
        fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Vacancy / Excess";
        fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Admission Confirm";
        fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Discontinue";
        for (int i = 0; i < 7; i++)
        {
            fpspread.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;

            fpspread.Sheets[0].ColumnHeader.Cells[0, i].Font.Bold = true;
            fpspread.Sheets[0].ColumnHeader.Cells[0, i].Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.Cells[0, i].Font.Name = "Book Antiqua";
            fpspread.Sheets[0].ColumnHeader.Cells[0, i].Font.Bold = true;
            fpspread.Sheets[0].ColumnHeader.Cells[0, i].ForeColor = Color.Black;

        }
        fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 50;
        fpspread.Sheets[0].ColumnHeader.Columns[1].Width = 250;
        fpspread.Sheets[0].ColumnHeader.Columns[2].Width = 150;
        fpspread.Sheets[0].ColumnHeader.Columns[3].Width = 150;
        fpspread.Sheets[0].ColumnHeader.Columns[4].Width = 150;
        fpspread.Sheets[0].ColumnHeader.Columns[5].Width = 150;


        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#10BADC");
        darkstyle.ForeColor = System.Drawing.Color.Black;
        darkstyle.Font.Name = "Book Antiqua";
        darkstyle.Font.Size = FontUnit.Medium;
        darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
        fpspread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
        //fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
        //fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
    }
    public void bindheaderspread2()
    {

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
        fpspread.Sheets[0].RowCount = 0;

        sql = "select * from textvaltable where college_code=" + ddlcollege.SelectedItem.Value + " and  TextCriteria='comm' and textval<>'' and TextCriteria2='comm1'";
        DataSet dscol = new DataSet();
        dscol.Clear();
        dscol = da.select_method_wo_parameter(sql, "Text");
        int colcount = 0;
        colcount = dscol.Tables[0].Rows.Count;

        fpspread.Sheets[0].ColumnCount = (colcount * 2) + 4;
        fpspread.Sheets[0].ColumnHeader.RowCount = 2;

        fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 30;
        fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        fpspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
        fpspread.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Center;
        fpspread.Sheets[0].ColumnHeader.Columns[1].HorizontalAlign = HorizontalAlign.Center;
        fpspread.Sheets[0].ColumnHeader.Columns[2].HorizontalAlign = HorizontalAlign.Center;


        fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";
        fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Dept";
        fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Total Strenght";
        fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Applied";
        fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
        fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
        fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
        int lastrowcount = 0;
        int lastrowcountff = 0;
        for (int i = 0; i < dscol.Tables[0].Rows.Count; i++)
        {
            fpspread.Sheets[0].ColumnHeader.Cells[1, i + 3].Text = dscol.Tables[0].Rows[i]["textval"].ToString();
            fpspread.Sheets[0].ColumnHeader.Cells[1, i + 3].Note = dscol.Tables[0].Rows[i]["TextCode"].ToString();
            lastrowcount = i + 3;

        }
        if (hiddenvalueapplied.Text == "")
        {
            hiddenvalueapplied.Text = "3- " + Convert.ToString(lastrowcount + 1) + " ";
        }
        fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 1, colcount);
        lastrowcount++;
        lastrowcountff = lastrowcount;
        fpspread.Sheets[0].ColumnHeader.Cells[0, lastrowcount].Text = "Admitted";
        for (int i = 0; i < dscol.Tables[0].Rows.Count; i++)
        {

            fpspread.Sheets[0].ColumnHeader.Cells[1, lastrowcount].Text = dscol.Tables[0].Rows[i]["textval"].ToString();
            fpspread.Sheets[0].ColumnHeader.Cells[1, lastrowcount].Note = dscol.Tables[0].Rows[i]["TextCode"].ToString();
            lastrowcount++;

        }
        if (hiddenvalueadmitted.Text == "")
        {
            hiddenvalueadmitted.Text = lastrowcountff + "-" + (lastrowcountff + colcount);
        }

        fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, lastrowcountff, 1, colcount);
        fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Text = "Vacancy";

        // fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnCount - 2, 2, 1);
        fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnCount - 1, 2, 1);

        for (int i = 0; i < fpspread.Sheets[0].ColumnCount; i++)
        {
            fpspread.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;

            fpspread.Sheets[0].ColumnHeader.Cells[0, i].Font.Bold = true;
            fpspread.Sheets[0].ColumnHeader.Cells[0, i].Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.Cells[0, i].Font.Name = "Book Antiqua";
            fpspread.Sheets[0].ColumnHeader.Cells[0, i].Font.Bold = true;
            fpspread.Sheets[0].ColumnHeader.Cells[0, i].ForeColor = Color.Black;

        }
        fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 50;
        fpspread.Sheets[0].ColumnHeader.Columns[1].Width = 250;
        fpspread.Sheets[0].ColumnHeader.Columns[2].Width = 150;
        fpspread.Sheets[0].ColumnHeader.Columns[3].Width = 150;
        fpspread.Sheets[0].ColumnHeader.Columns[4].Width = 150;
        fpspread.Sheets[0].ColumnHeader.Columns[5].Width = 150;
        fpspread.Visible = true;

        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#10BADC");
        darkstyle.ForeColor = System.Drawing.Color.Black;
        darkstyle.Font.Name = "Book Antiqua";
        darkstyle.Font.Size = FontUnit.Medium;
        darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
        fpspread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
        //fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
        //fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
    }

    public void bindheaderspread3()
    {

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
        fpspread.Sheets[0].RowCount = 0;

        sql = "select * from textvaltable where college_code=" + ddlcollege.SelectedItem.Value + " and  TextCriteria='relig' and textval<>'' and TextCriteria2='relig1'";
        DataSet dscol = new DataSet();
        dscol.Clear();
        dscol = da.select_method_wo_parameter(sql, "Text");
        int colcount = 0;
        colcount = dscol.Tables[0].Rows.Count;

        fpspread.Sheets[0].ColumnCount = (colcount * 2) + 4;
        fpspread.Sheets[0].ColumnHeader.RowCount = 2;

        fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 30;
        fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        fpspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
        fpspread.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Center;
        fpspread.Sheets[0].ColumnHeader.Columns[1].HorizontalAlign = HorizontalAlign.Center;
        fpspread.Sheets[0].ColumnHeader.Columns[2].HorizontalAlign = HorizontalAlign.Center;


        fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";
        fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Dept";
        fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Total Strenght";
        fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Applied";
        fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
        fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
        fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
        int lastrowcount = 0;
        int lastrowcountff = 0;
        for (int i = 0; i < dscol.Tables[0].Rows.Count; i++)
        {
            fpspread.Sheets[0].ColumnHeader.Cells[1, i + 3].Text = dscol.Tables[0].Rows[i]["textval"].ToString();
            fpspread.Sheets[0].ColumnHeader.Cells[1, i + 3].Note = dscol.Tables[0].Rows[i]["TextCode"].ToString();
            lastrowcount = i + 3;

        }
        if (hiddenvalueapplied.Text == "")
        {
            hiddenvalueapplied.Text = "3- " + Convert.ToString(lastrowcount + 1) + " ";
        }
        fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 1, colcount);
        lastrowcount++;
        lastrowcountff = lastrowcount;
        fpspread.Sheets[0].ColumnHeader.Cells[0, lastrowcount].Text = "Admitted";
        for (int i = 0; i < dscol.Tables[0].Rows.Count; i++)
        {

            fpspread.Sheets[0].ColumnHeader.Cells[1, lastrowcount].Text = dscol.Tables[0].Rows[i]["textval"].ToString();
            fpspread.Sheets[0].ColumnHeader.Cells[1, lastrowcount].Note = dscol.Tables[0].Rows[i]["TextCode"].ToString();
            lastrowcount++;

        }
        if (hiddenvalueadmitted.Text == "")
        {
            hiddenvalueadmitted.Text = lastrowcountff + "-" + (lastrowcountff + colcount);
        }

        fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, lastrowcountff, 1, colcount);
        fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Text = "Vacancy";

        // fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnCount - 2, 2, 1);
        fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnCount - 1, 2, 1);

        for (int i = 0; i < fpspread.Sheets[0].ColumnCount; i++)
        {
            fpspread.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;

            fpspread.Sheets[0].ColumnHeader.Cells[0, i].Font.Bold = true;
            fpspread.Sheets[0].ColumnHeader.Cells[0, i].Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.Cells[0, i].Font.Name = "Book Antiqua";
            fpspread.Sheets[0].ColumnHeader.Cells[0, i].Font.Bold = true;
            fpspread.Sheets[0].ColumnHeader.Cells[0, i].ForeColor = Color.Black;

        }
        fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 50;
        fpspread.Sheets[0].ColumnHeader.Columns[1].Width = 250;
        fpspread.Sheets[0].ColumnHeader.Columns[2].Width = 150;
        fpspread.Sheets[0].ColumnHeader.Columns[3].Width = 150;
        fpspread.Sheets[0].ColumnHeader.Columns[4].Width = 150;
        fpspread.Sheets[0].ColumnHeader.Columns[5].Width = 150;
        fpspread.Visible = true;

        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#10BADC");
        darkstyle.ForeColor = System.Drawing.Color.Black;
        darkstyle.Font.Name = "Book Antiqua";
        darkstyle.Font.Size = FontUnit.Medium;
        darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
        fpspread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
        //fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
        //fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("default.aspx");
        }
        catch (Exception ex)
        {
        }
    }

    public void loadcollege()
    {
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        if ((group_user.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
        {
            columnfield = " and group_code='" + group_user + "'";
        }
        else
        {
            columnfield = " and user_code='" + Session["usercode"] + "'";
        }
        hat.Clear();
        hat.Add("column_field", columnfield.ToString());
        ds.Dispose();
        ds.Reset();
        ds = da.select_method("bind_college", hat, "sp");
        ddlcollege.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlcollege.DataSource = ds;
            ddlcollege.DataTextField = "collname";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();

        }
    }

    public void type()
    {
        ds.Dispose();
        ds.Reset();
        ds = da.select_method_wo_parameter("select distinct type  from Course where college_code ='" + ddlcollege.SelectedItem.Value + "'", "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddltype.DataSource = ds;
            ddltype.DataTextField = "type";
            ddltype.DataValueField = "type";
            ddltype.DataBind();
        }

    }
    public void batch()
    {


        ds.Dispose();
        ds.Reset();
        ds = da.BindBatch();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlbatch.DataSource = ds;
            ddlbatch.DataTextField = "Batch_year";
            ddlbatch.DataValueField = "Batch_year";
            ddlbatch.DataBind();
            if (ddlbatch.Items.Count > 0)
                ddlbatch.SelectedIndex = ddlbatch.Items.Count - 1;
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        fpspread.Visible = false;
        rptprint.Visible = false;
        type();

    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        fpspread.Visible = false;
        rptprint.Visible = false;

    }
    protected void ddlreport_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlreport.SelectedIndex == 0)
        {
            rdbdepartemntwise.Enabled = false;
            rdbManagementwise.Enabled = false;
        }
        else
        {
            rdbdepartemntwise.Enabled = true;
            rdbManagementwise.Enabled = true;
        }
        fpspread.Visible = false;
        rptprint.Visible = false;


    }
    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        fpspread.Visible = false;
        rptprint.Visible = false;



    }

    protected void btngo_click(object sender, EventArgs e)
    {
        fpspread.Sheets[0].RowCount = 0;
        fpspread.Sheets[0].Columns.Count = 0;
        hiddenvalueadmitted.Text = "";
        hiddenvalueapplied.Text = "";
        rptprint.Visible = false;
        fpspread.Visible = true;
        if (ddlreport.SelectedIndex == 0)
        {
            bindheaderspread1();
            reporttotal();
        }
        if (ddlreport.SelectedIndex == 1)
        {
            categorywisereport();
        }
        if (ddlreport.SelectedIndex == 2)
        {
            Quotawisereport();
        }
        if (ddlreport.SelectedIndex == 3)
        {
            bindheaderspread2();
            reportcommunity();
        }
        if (ddlreport.SelectedIndex == 4)
        {
            bindheaderspread3();
            reportreligion();
        }
        if (ddlreport.SelectedIndex == 5)
        {
            confirmenrollment();
        }

    }

    public void confirmenrollment()
    {
        try
        {
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
            fpspread.Sheets[0].RowCount = 0;
            fpspread.Sheets[0].ColumnCount = 7;
            fpspread.Sheets[0].ColumnHeader.RowCount = 1;
            fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 30;
            fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            fpspread.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].ColumnHeader.Columns[1].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].ColumnHeader.Columns[2].HorizontalAlign = HorizontalAlign.Center;


            fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Dept";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Total Strenght";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Applied";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Admitted";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Confirm Enrollment";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Left";
            for (int i = 0; i < 7; i++)
            {
                fpspread.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;

                fpspread.Sheets[0].ColumnHeader.Cells[0, i].Font.Bold = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, i].Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].ColumnHeader.Cells[0, i].Font.Name = "Book Antiqua";
                fpspread.Sheets[0].ColumnHeader.Cells[0, i].Font.Bold = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, i].ForeColor = Color.Black;

            }
            fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 50;
            fpspread.Sheets[0].ColumnHeader.Columns[1].Width = 250;
            fpspread.Sheets[0].ColumnHeader.Columns[2].Width = 150;
            fpspread.Sheets[0].ColumnHeader.Columns[3].Width = 150;
            fpspread.Sheets[0].ColumnHeader.Columns[4].Width = 150;
            fpspread.Sheets[0].ColumnHeader.Columns[5].Width = 150;
            fpspread.Sheets[0].ColumnHeader.Columns[6].Width = 150;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#10BADC");
            darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
            fpspread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;


            sql = "select distinct Edu_Level  from Course where college_code ='" + ddlcollege.SelectedItem.Value + "' and type ='" + ddltype.SelectedItem.Text + "' order by Edu_Level desc";

            ds.Clear();
            ds = da.select_method_wo_parameter(sql, "Text");
            DataSet dsrpt1 = new DataSet();
            dsrpt1.Clear();
            ArrayList arr_rejectcol = new ArrayList();
            DataSet dsrpt1totst = new DataSet();
            dsrpt1totst.Clear();
            DataView dvcount = new DataView();
            string dummy = "";
            int totalfinalrow = 0;
            arr_rejectcol.Clear();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sql = "select * from Degree d,Department dt,Course c,DeptPrivilages dp where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and d.Degree_Code =dp.degree_code and user_code =" + usercode + " and Edu_Level ='" + ds.Tables[0].Rows[i][0].ToString() + "' and c.type ='" + ddltype.SelectedItem.ToString() + "'";
                dsrpt1 = da.select_method_wo_parameter(sql, "Text");
                if (dummy != ds.Tables[0].Rows[i][0].ToString())
                {
                    fpspread.Sheets[0].RowCount++;
                    totalfinalrow = fpspread.Sheets[0].RowCount;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = ds.Tables[0].Rows[i][0].ToString();
                    //if (!arr_rejectcol.Contains(fpspread.Sheets[0].RowCount - 1))
                    //{
                    //    arr_rejectcol.Add(fpspread.Sheets[0].RowCount - 1);
                    //}
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#65ebca");
                    dummy = ds.Tables[0].Rows[i][0].ToString();

                }

                for (int j = 0; j < dsrpt1.Tables[0].Rows.Count; j++)
                {
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = (j + 1).ToString();
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = dsrpt1.Tables[0].Rows[j]["Dept_Name"].ToString();
                    sql = "select * from applyn where college_code ='" + ddlcollege.SelectedItem.Value + "' and batch_year ='" + ddlbatch.SelectedItem.ToString() + "'   and degree_code='" + dsrpt1.Tables[0].Rows[j]["degree_code"].ToString() + "'";
                    sql = sql + " select * from Registration where college_code ='" + ddlcollege.SelectedItem.Value + "' and batch_year ='" + ddlbatch.SelectedItem.ToString() + "'   and degree_code='" + dsrpt1.Tables[0].Rows[j]["degree_code"].ToString() + "'";
                    dsrpt1totst = da.select_method_wo_parameter(sql, "Text");
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = dsrpt1.Tables[0].Rows[j]["No_Of_seats"].ToString();
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                    dsrpt1totst.Tables[0].DefaultView.RowFilter = "isconfirm  = 1 ";
                    dvcount = dsrpt1totst.Tables[0].DefaultView;
                    int count4 = 0;
                    count4 = dvcount.Count;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(count4);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

                    dsrpt1totst.Tables[0].DefaultView.RowFilter = "admission_status  = 1 ";
                    dvcount = dsrpt1totst.Tables[0].DefaultView;
                    count4 = dvcount.Count;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(count4);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                    //int sr = Convert.ToInt32(dsrpt1.Tables[0].Rows[j]["No_Of_seats"]);
                    //sr = sr - count4;
                    dsrpt1totst.Tables[0].DefaultView.RowFilter = "is_enroll =2";
                    dvcount = dsrpt1totst.Tables[0].DefaultView;
                    count4 = dvcount.Count;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(count4);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;

                    dsrpt1totst.Tables[1].DefaultView.RowFilter = "DelFlag =1";
                    dvcount = dsrpt1totst.Tables[1].DefaultView;
                    count4 = dvcount.Count;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(count4);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;

                }
                fpspread.Sheets[0].RowCount++;
                int counttotal = 0;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Total";
                fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, 2);
                for (int mm = 2; mm < fpspread.Sheets[0].Columns.Count; mm++)
                {
                    for (int m = totalfinalrow; m < fpspread.Sheets[0].RowCount - 1; m++)
                    {
                        counttotal = counttotal + Convert.ToInt32(fpspread.Sheets[0].Cells[m, mm].Text);

                    }
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, mm].Text = Convert.ToString(counttotal);
                    fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#dceaf9");
                    if (!arr_rejectcol.Contains(fpspread.Sheets[0].RowCount - 1))
                    {
                        arr_rejectcol.Add(fpspread.Sheets[0].RowCount - 1);
                    }

                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, mm].HorizontalAlign = HorizontalAlign.Center;
                    counttotal = 0;
                }

                if (fpspread.Sheets[0].RowCount > 0)
                {
                    fpspread.Visible = true;
                    rptprint.Visible = true;

                }

            }
            if (fpspread.Sheets[0].RowCount > 0)
            {
                fpspread.Sheets[0].RowCount++;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
                fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, 2);
                int countgrandtotal = 0;
                for (int j = 2; j < fpspread.Sheets[0].Columns.Count; j++)
                {

                    //int rowss = Convert.ToInt32(arr_rejectcol[j].ToString());
                    for (int i = 0; i < arr_rejectcol.Count; i++)
                    {
                        int rowss = Convert.ToInt32(arr_rejectcol[i].ToString());

                        countgrandtotal = countgrandtotal + Convert.ToInt32(fpspread.Sheets[0].Cells[rowss, j].Text.ToString());

                    }
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, j].Text = Convert.ToString(countgrandtotal);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, j].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#dceaf9");
                    countgrandtotal = 0;
                }
            }
            fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
            fpspread.SaveChanges();
        }
        catch
        {

        }
    }

    public void Quotawisereport()
    {
        try
        {

            fpspread.CommandBar.Visible = false;
            fpspread.Sheets[0].RowHeader.Visible = false;
            fpspread.Sheets[0].AutoPostBack = true;
            double totalstrength = 0;
            int maintotalvalue = 0;
            int admitiontotalvlaue = 0;
            int vacancytotalvalue = 0;

            int mangetotalvalue = 0;
            int admitmanagetotalvalue = 0;
            int vacancymangetotalvalue = 0;

            sql = "select distinct Edu_Level  from Course where college_code ='" + ddlcollege.SelectedItem.Value + "' and type ='" + ddltype.SelectedItem.Text + "' order by Edu_Level desc";
            ds.Clear();
            dsnew = da.select_method_wo_parameter(sql, "Text");
            if (dsnew.Tables[0].Rows.Count > 0)
            {
                string type = Convert.ToString(ddltype.SelectedItem.Text);
                string level = Convert.ToString(dsnew.Tables[0].Rows[0]["Edu_Level"]);
                string concate = Convert.ToString(type) + "-" + Convert.ToString(level);

                fpspread.Sheets[0].ColumnHeader.RowCount = 2;
                fpspread.Sheets[0].ColumnHeader.Columns.Count++;
                fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1, 2, 1);
                fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "S.No";
                fpspread.Sheets[0].ColumnHeader.Columns.Count++;
                fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1, 2, 1);
                fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Department";
                fpspread.Sheets[0].ColumnHeader.Columns.Count++;
                fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Department Allocation";
                fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Strength";
                fpspread.Sheets[0].ColumnHeader.Columns.Count++;
                fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Admitted";
                fpspread.Sheets[0].ColumnHeader.Columns.Count++;
                fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Vacancy";
                fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnHeader.Columns.Count - 3, 1, 3);
                fpspread.Sheets[0].ColumnHeader.Columns.Count++;

                fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Management Allocation";
                fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Strength";
                fpspread.Sheets[0].ColumnHeader.Columns.Count++;
                fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Admitted";
                fpspread.Sheets[0].ColumnHeader.Columns.Count++;
                fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Vacancy";
                fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnHeader.Columns.Count - 3, 1, 3);
                for (int check = 0; check < dsnew.Tables[0].Rows.Count; check++)
                {

                    totalstrength = 0;
                    maintotalvalue = 0;
                    admitiontotalvlaue = 0;
                    vacancytotalvalue = 0;

                    mangetotalvalue = 0;
                    admitmanagetotalvalue = 0;
                    vacancymangetotalvalue = 0;

                    string dummy = "";
                    sql = "select * from Degree d,Department dt,Course c,DeptPrivilages dp where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and d.Degree_Code =dp.degree_code and user_code =" + usercode + " and Edu_Level ='" + dsnew.Tables[0].Rows[check][0].ToString() + "' and c.type ='" + ddltype.SelectedItem.ToString() + "'";
                    sql = sql + " select value  from Master_Settings where settings ='Departmentallocate" + concate + "'";
                    sql = sql + " select * from admitcolumnset where college_code ='" + ddlcollege.SelectedItem.Value + "'";
                    sql = sql + " select value  from Master_Settings where settings ='Managmentallocate" + concate + "'";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(sql, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (dummy != dsnew.Tables[0].Rows[check][0].ToString())
                        {
                            fpspread.Sheets[0].RowCount++;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = dsnew.Tables[0].Rows[check][0].ToString();
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#65ebca");
                            dummy = dsnew.Tables[0].Rows[check][0].ToString();

                        }
                        DataView dv = new DataView();
                        DataTable data = new DataTable();
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            totalstrength = 0;
                            fpspread.Sheets[0].RowCount++;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = (j + 1).ToString();
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[j]["Dept_Name"].ToString();
                            double deptpercentage = 0;
                            string dptvalue = "";
                            string totalseat = Convert.ToString(ds.Tables[0].Rows[j]["No_Of_seats"]);
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                dptvalue = Convert.ToString(ds.Tables[1].Rows[0]["value"]);
                            }
                            if (dptvalue.Trim() != "")
                            {
                                deptpercentage = Convert.ToDouble(totalseat) / Convert.ToDouble(100) * Convert.ToDouble(dptvalue);
                            }
                            totalstrength = totalstrength + Math.Round(deptpercentage);
                            maintotalvalue = maintotalvalue + Convert.ToInt32(totalstrength);
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(Math.Round(deptpercentage));
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                            ds.Tables[2].DefaultView.RowFilter = "setcolumn='" + Convert.ToString(ds.Tables[0].Rows[j]["Degree_Code"]) + "' and (textcriteria ='relig' or textcriteria ='community')";
                            dv = ds.Tables[2].DefaultView;
                            if (dv.Count > 0)
                            {
                                data = dv.ToTable();
                                if (data.Rows.Count > 0)
                                {
                                    string total1 = Convert.ToString(data.Compute("Sum(allot_Confirm)", ""));
                                    if (total1.Trim() != "")
                                    {
                                        double total = Convert.ToDouble(total1);
                                        admitiontotalvlaue = admitiontotalvlaue + Convert.ToInt32(Math.Round(total));
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(total);
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                        total = Convert.ToDouble(totalstrength) - total;
                                        vacancytotalvalue = vacancytotalvalue + Convert.ToInt32(Math.Round(total));
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(total);
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(0);
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                        vacancytotalvalue = vacancytotalvalue + Convert.ToInt32(Math.Round(totalstrength));
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(totalstrength);
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                                    }
                                }
                            }
                            totalstrength = 0;
                            deptpercentage = 0;
                            dptvalue = "";
                            totalseat = Convert.ToString(ds.Tables[0].Rows[j]["No_Of_seats"]);
                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                dptvalue = Convert.ToString(ds.Tables[3].Rows[0]["value"]);
                            }
                            if (dptvalue.Trim() != "")
                            {
                                deptpercentage = Convert.ToDouble(totalseat) / Convert.ToDouble(100) * Convert.ToDouble(dptvalue);
                            }
                            totalstrength = totalstrength + Math.Round(deptpercentage);
                            mangetotalvalue = mangetotalvalue + Convert.ToInt32(Math.Round(totalstrength));
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(Math.Round(deptpercentage));
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;

                            ds.Tables[2].DefaultView.RowFilter = "setcolumn='" + Convert.ToString(ds.Tables[0].Rows[j]["Degree_Code"]) + "' and textcriteria ='Management'";
                            dv = ds.Tables[2].DefaultView;
                            if (dv.Count > 0)
                            {
                                data = dv.ToTable();
                                if (data.Rows.Count > 0)
                                {
                                    string total1 = Convert.ToString(data.Compute("Sum(allot_Confirm)", ""));
                                    if (total1.Trim() != "")
                                    {
                                        double total = Convert.ToDouble(total1);
                                        admitmanagetotalvalue = admitmanagetotalvalue + Convert.ToInt32(Math.Round(total));
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(total);
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                        total = Convert.ToDouble(totalstrength) - total;
                                        vacancymangetotalvalue = vacancymangetotalvalue + Convert.ToInt32(Math.Round(total));
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(total);
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {

                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(0);
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                        vacancymangetotalvalue = vacancymangetotalvalue + Convert.ToInt32(Math.Round(totalstrength));
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(totalstrength);
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                }
                            }
                        }
                    }
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Total";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(maintotalvalue);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(admitiontotalvlaue);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(vacancytotalvalue);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(mangetotalvalue);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(admitmanagetotalvalue);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(vacancymangetotalvalue);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                }
                fpspread.Sheets[0].PageSize = fpspread.Sheets[0].Rows.Count;
                fpspread.Visible = true;
                rptprint.Visible = true;
            }
        }
        catch
        {

        }
    }


    public void categorywisereport()
    {
        try
        {
            fpspread.CommandBar.Visible = false;
            fpspread.Sheets[0].RowHeader.Visible = false;
            fpspread.Sheets[0].AutoPostBack = true;
            double totalstrength = 0;
            double grandtotal = 0;
            sql = "select distinct Edu_Level  from Course where college_code ='" + ddlcollege.SelectedItem.Value + "' and type ='" + ddltype.SelectedItem.Text + "' order by Edu_Level desc";
            ds.Clear();
            dsnew = da.select_method_wo_parameter(sql, "Text");
            if (dsnew.Tables[0].Rows.Count > 0)
            {
                string type = Convert.ToString(ddltype.SelectedItem.Text);
                string level = Convert.ToString(dsnew.Tables[0].Rows[0]["Edu_Level"]);
                string concate = Convert.ToString(type) + "-" + Convert.ToString(level);
                string selectquery = "  select TextVal ,column_name  from admitcolumnset a , TextValTable t  where a.column_name =T.TextCode and T.college_code =a.college_code and  setcolumn ='" + concate + "' and a.TextCriteria ='relig' order by TextVal";
                selectquery = selectquery + "   select TextVal ,column_name  from admitcolumnset a , TextValTable t  where a.column_name =T.TextCode and T.college_code =a.college_code and  setcolumn ='" + concate + "' and a.TextCriteria ='community' order by TextVal";

                ds.Clear();
                ds = da.select_method_wo_parameter(selectquery, "Text");
                fpspread.Sheets[0].ColumnHeader.RowCount = 2;
                fpspread.Sheets[0].ColumnHeader.Columns.Count++;
                fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1, 2, 1);
                fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "S.No";
                fpspread.Sheets[0].ColumnHeader.Columns.Count++;
                fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1, 2, 1);
                fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Department";
                fpspread.Sheets[0].ColumnHeader.Columns.Count++;
                fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1, 2, 1);
                fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Total Strength";

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                    {
                        fpspread.Sheets[0].ColumnHeader.Columns.Count++;
                        fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1].Text = Convert.ToString(ds.Tables[0].Rows[k]["TextVal"]);
                        fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1].Tag = Convert.ToString(ds.Tables[0].Rows[k]["column_name"]);
                        if (!commarray.Contains(Convert.ToString(ds.Tables[0].Rows[k]["column_name"])))
                        {
                            commarray.Add(Convert.ToString(ds.Tables[0].Rows[k]["column_name"]));
                        }
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)
                {

                    for (int k = 0; k < ds.Tables[1].Rows.Count; k++)
                    {
                        fpspread.Sheets[0].ColumnHeader.Columns.Count++;
                        fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1].Text = Convert.ToString(ds.Tables[1].Rows[k]["TextVal"]);
                        fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1].Tag = Convert.ToString(ds.Tables[1].Rows[k]["column_name"]);
                        if (!religarray.Contains(Convert.ToString(ds.Tables[1].Rows[k]["column_name"])))
                        {
                            religarray.Add(Convert.ToString(ds.Tables[1].Rows[k]["column_name"]));
                        }
                    }
                }
                fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnHeader.Columns.Count - (commarray.Count + religarray.Count), 1, commarray.Count + religarray.Count);
                fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnHeader.Columns.Count - (commarray.Count + religarray.Count)].Text = "Applied";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                    {
                        fpspread.Sheets[0].ColumnHeader.Columns.Count++;
                        fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1].Text = Convert.ToString(ds.Tables[0].Rows[k]["TextVal"]);
                        fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1].Tag = Convert.ToString(ds.Tables[0].Rows[k]["column_name"]);
                        //if (!commarray.Contains(Convert.ToString(ds.Tables[0].Rows[k]["column_name"])))
                        //{
                        //    commarray.Add(Convert.ToString(ds.Tables[0].Rows[k]["column_name"]));
                        //}
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)
                {

                    for (int k = 0; k < ds.Tables[1].Rows.Count; k++)
                    {
                        fpspread.Sheets[0].ColumnHeader.Columns.Count++;
                        fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1].Text = Convert.ToString(ds.Tables[1].Rows[k]["TextVal"]);
                        fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnHeader.Columns.Count - 1].Tag = Convert.ToString(ds.Tables[1].Rows[k]["column_name"]);
                        //if (!religarray.Contains(Convert.ToString(ds.Tables[1].Rows[k]["column_name"])))
                        //{
                        //    religarray.Add(Convert.ToString(ds.Tables[1].Rows[k]["column_name"]));
                        //}
                    }
                }
                fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnHeader.Columns.Count - (commarray.Count + religarray.Count), 1, commarray.Count + religarray.Count);
                fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnHeader.Columns.Count - (commarray.Count + religarray.Count)].Text = "Admitted";
                for (int check = 0; check < dsnew.Tables[0].Rows.Count; check++)
                {
                    totalstrength = 0;
                    addrelig.Clear();
                    addcomm.Clear();
                    allotrelig.Clear();
                    allotcomm.Clear();
                    string dummy = "";
                    sql = "select * from Degree d,Department dt,Course c,DeptPrivilages dp where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and d.Degree_Code =dp.degree_code and user_code =" + usercode + " and Edu_Level ='" + dsnew.Tables[0].Rows[check][0].ToString() + "' and c.type ='" + ddltype.SelectedItem.ToString() + "'";
                    sql = sql + " select value  from Master_Settings where settings ='Departmentallocate" + concate + "'";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(sql, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (dummy != dsnew.Tables[0].Rows[check][0].ToString())
                        {
                            fpspread.Sheets[0].RowCount++;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = dsnew.Tables[0].Rows[check][0].ToString();
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#65ebca");
                            dummy = dsnew.Tables[0].Rows[check][0].ToString();

                        }
                        DataView dv = new DataView();
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            fpspread.Sheets[0].RowCount++;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = (j + 1).ToString();
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[j]["Dept_Name"].ToString();
                            double deptpercentage = 0;
                            string dptvalue = "";
                            string totalseat = Convert.ToString(ds.Tables[0].Rows[j]["No_Of_seats"]);
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                dptvalue = Convert.ToString(ds.Tables[1].Rows[0]["value"]);
                            }
                            if (dptvalue.Trim() != "")
                            {
                                deptpercentage = Convert.ToDouble(totalseat) / Convert.ToDouble(100) * Convert.ToDouble(dptvalue);
                            }
                            totalstrength = totalstrength + Math.Round(deptpercentage);
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(Math.Round(deptpercentage));
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            sql = "select * from applyn where college_code ='" + ddlcollege.SelectedItem.Value + "' and batch_year ='" + ddlbatch.SelectedItem.ToString() + "'   and degree_code='" + ds.Tables[0].Rows[j]["degree_code"].ToString() + "' and isconfirm  = 1";
                            sql = sql + " select * from admitcolumnset where setcolumn ='" + Convert.ToString(ds.Tables[0].Rows[j]["degree_code"]) + "'";
                            dsrpt1totst = da.select_method_wo_parameter(sql, "Text");
                            int column = 2;
                            if (dsrpt1totst.Tables[0].Rows.Count > 0)
                            {
                                if (commarray.Count > 0)
                                {
                                    for (int co = 0; co < commarray.Count; co++)
                                    {
                                        column++;
                                        string firstvalue = Convert.ToString(commarray[co]);
                                        dsrpt1totst.Tables[0].DefaultView.RowFilter = "religion=" + firstvalue + "";
                                        dv = dsrpt1totst.Tables[0].DefaultView;
                                        if (dv.Count > 0)
                                        {
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(dv.Count);
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else
                                        {
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(0);
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        if (!addrelig.Contains(firstvalue))
                                        {
                                            addrelig.Add(firstvalue, Convert.ToString(dv.Count));
                                        }
                                        else
                                        {
                                            string getvalue = Convert.ToString(addrelig[firstvalue]);
                                            if (getvalue.Trim() != "")
                                            {
                                                double dtot = Convert.ToDouble(getvalue) + Convert.ToDouble(dv.Count);
                                                addrelig.Remove(firstvalue);
                                                addrelig.Add(firstvalue, Convert.ToString(dtot));
                                            }
                                        }
                                    }
                                }

                                if (religarray.Count > 0)
                                {
                                    for (int co = 0; co < religarray.Count; co++)
                                    {
                                        column++;
                                        string firstvalue = Convert.ToString(religarray[co]);
                                        dsrpt1totst.Tables[0].DefaultView.RowFilter = "community=" + firstvalue + "";
                                        dv = dsrpt1totst.Tables[0].DefaultView;
                                        if (dv.Count > 0)
                                        {
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(dv.Count);
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else
                                        {
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(0);
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                                        }

                                        if (!addcomm.Contains(firstvalue))
                                        {
                                            addcomm.Add(firstvalue, Convert.ToString(dv.Count));
                                        }
                                        else
                                        {
                                            string getvalue = Convert.ToString(addcomm[firstvalue]);
                                            if (getvalue.Trim() != "")
                                            {
                                                double dtot = Convert.ToDouble(getvalue) + Convert.ToDouble(dv.Count);
                                                addcomm.Remove(firstvalue);
                                                addcomm.Add(firstvalue, Convert.ToString(dtot));
                                            }
                                        }
                                    }
                                }
                                if (commarray.Count > 0)
                                {
                                    for (int co = 0; co < commarray.Count; co++)
                                    {
                                        column++;
                                        string admitconfirm = "";
                                        string firstvalue = Convert.ToString(commarray[co]);
                                        dsrpt1totst.Tables[1].DefaultView.RowFilter = "column_name='" + firstvalue + "'";
                                        dv = dsrpt1totst.Tables[1].DefaultView;
                                        if (dv.Count > 0)
                                        {
                                            admitconfirm = Convert.ToString(dv[0]["allot_Confirm"]);
                                            if (admitconfirm.Trim() == "")
                                            {
                                                admitconfirm = "0";
                                            }
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(admitconfirm);
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else
                                        {
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(0);
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        if (!allotrelig.Contains(firstvalue))
                                        {
                                            allotrelig.Add(firstvalue, Convert.ToString(admitconfirm));
                                        }
                                        else
                                        {
                                            string getvalue = Convert.ToString(allotrelig[firstvalue]);
                                            if (getvalue.Trim() != "")
                                            {
                                                double dtot = Convert.ToDouble(getvalue) + Convert.ToDouble(admitconfirm);
                                                allotrelig.Remove(firstvalue);
                                                allotrelig.Add(firstvalue, Convert.ToString(dtot));
                                            }
                                        }
                                    }
                                }

                                if (religarray.Count > 0)
                                {
                                    for (int co = 0; co < religarray.Count; co++)
                                    {
                                        column++;
                                        string admitconfirm = "";
                                        string firstvalue = Convert.ToString(religarray[co]);
                                        dsrpt1totst.Tables[1].DefaultView.RowFilter = "column_name='" + firstvalue + "'";
                                        dv = dsrpt1totst.Tables[1].DefaultView;
                                        if (dv.Count > 0)
                                        {
                                            admitconfirm = Convert.ToString(dv[0]["allot_Confirm"]);
                                            if (admitconfirm.Trim() == "")
                                            {
                                                admitconfirm = "0";
                                            }
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(admitconfirm);
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else
                                        {
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(0);
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        if (!allotcomm.Contains(firstvalue))
                                        {
                                            allotcomm.Add(firstvalue, Convert.ToString(admitconfirm));
                                        }
                                        else
                                        {
                                            string getvalue = Convert.ToString(allotcomm[firstvalue]);
                                            if (getvalue.Trim() != "")
                                            {
                                                double dtot = Convert.ToDouble(getvalue) + Convert.ToDouble(admitconfirm);
                                                allotcomm.Remove(firstvalue);
                                                allotcomm.Add(firstvalue, Convert.ToString(dtot));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        fpspread.Sheets[0].RowCount++;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Total";
                        grandtotal = grandtotal + totalstrength;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(totalstrength);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        int colcount = 2;
                        if (commarray.Count > 0)
                        {
                            for (int co = 0; co < commarray.Count; co++)
                            {
                                colcount++;
                                string regvalue = Convert.ToString(addrelig[commarray[co]]);
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(regvalue);
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                if (!grantrelig.Contains(commarray[co]))
                                {
                                    grantrelig.Add(commarray[co], Convert.ToString(regvalue));
                                }
                                else
                                {
                                    string getvalue = Convert.ToString(grantrelig[commarray[co]]);
                                    if (getvalue.Trim() != "")
                                    {
                                        double dtot = Convert.ToDouble(getvalue) + Convert.ToDouble(regvalue);
                                        grantrelig.Remove(commarray[co]);
                                        grantrelig.Add(commarray[co], Convert.ToString(dtot));
                                    }
                                }
                            }
                        }
                        if (religarray.Count > 0)
                        {
                            for (int co = 0; co < religarray.Count; co++)
                            {
                                colcount++;
                                string regvalue = Convert.ToString(addcomm[religarray[co]]);
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(regvalue);
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Center;

                                if (!grantcomm.Contains(religarray[co]))
                                {
                                    grantcomm.Add(religarray[co], Convert.ToString(regvalue));
                                }
                                else
                                {
                                    string getvalue = Convert.ToString(grantcomm[religarray[co]]);
                                    if (getvalue.Trim() != "")
                                    {
                                        double dtot = Convert.ToDouble(getvalue) + Convert.ToDouble(regvalue);
                                        grantcomm.Remove(religarray[co]);
                                        grantcomm.Add(religarray[co], Convert.ToString(dtot));
                                    }
                                }
                            }
                        }
                        if (commarray.Count > 0)
                        {
                            for (int co = 0; co < commarray.Count; co++)
                            {
                                colcount++;
                                string regvalue = Convert.ToString(allotrelig[commarray[co]]);
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(regvalue);
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Center;

                                if (!grantallotrelig.Contains(commarray[co]))
                                {
                                    grantallotrelig.Add(commarray[co], Convert.ToString(regvalue));
                                }
                                else
                                {
                                    string getvalue = Convert.ToString(grantallotrelig[commarray[co]]);
                                    if (getvalue.Trim() != "")
                                    {
                                        double dtot = Convert.ToDouble(getvalue) + Convert.ToDouble(regvalue);
                                        grantallotrelig.Remove(commarray[co]);
                                        grantallotrelig.Add(commarray[co], Convert.ToString(dtot));
                                    }
                                }
                            }
                        }
                        if (religarray.Count > 0)
                        {
                            for (int co = 0; co < religarray.Count; co++)
                            {
                                colcount++;
                                string regvalue = Convert.ToString(allotcomm[religarray[co]]);
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(regvalue);
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Center;

                                if (!grnatallotcomm.Contains(religarray[co]))
                                {
                                    grnatallotcomm.Add(religarray[co], Convert.ToString(regvalue));
                                }
                                else
                                {
                                    string getvalue = Convert.ToString(grnatallotcomm[religarray[co]]);
                                    if (getvalue.Trim() != "")
                                    {
                                        double dtot = Convert.ToDouble(getvalue) + Convert.ToDouble(regvalue);
                                        grnatallotcomm.Remove(religarray[co]);
                                        grnatallotcomm.Add(religarray[co], Convert.ToString(dtot));
                                    }
                                }
                            }
                        }
                    }
                }
                fpspread.Sheets[0].RowCount++;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(grandtotal);
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                int colllcount = 2;
                if (commarray.Count > 0)
                {
                    for (int co = 0; co < commarray.Count; co++)
                    {
                        colllcount++;
                        string regvalue = Convert.ToString(grantrelig[commarray[co]]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, colllcount].Text = Convert.ToString(regvalue);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, colllcount].HorizontalAlign = HorizontalAlign.Center;
                        // grantrelig.Add(commarray[co], Convert.ToString(regvalue));
                    }
                }
                if (religarray.Count > 0)
                {
                    for (int co = 0; co < religarray.Count; co++)
                    {
                        colllcount++;
                        string regvalue = Convert.ToString(grantcomm[religarray[co]]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, colllcount].Text = Convert.ToString(regvalue);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, colllcount].HorizontalAlign = HorizontalAlign.Center;
                        // grantcomm.Add(religarray[co], Convert.ToString(regvalue));
                    }
                }
                if (commarray.Count > 0)
                {
                    for (int co = 0; co < commarray.Count; co++)
                    {
                        colllcount++;
                        string regvalue = Convert.ToString(grantallotrelig[commarray[co]]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, colllcount].Text = Convert.ToString(regvalue);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, colllcount].HorizontalAlign = HorizontalAlign.Center;
                        //grantallotrelig.Add(commarray[co], Convert.ToString(regvalue));
                    }
                }
                if (religarray.Count > 0)
                {
                    for (int co = 0; co < religarray.Count; co++)
                    {
                        colllcount++;
                        string regvalue = Convert.ToString(grnatallotcomm[religarray[co]]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, colllcount].Text = Convert.ToString(regvalue);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, colllcount].HorizontalAlign = HorizontalAlign.Center;
                        //grnatallotcomm.Add(religarray[co], Convert.ToString(regvalue));
                    }
                }

                fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
                fpspread.Visible = true;
                rptprint.Visible = true;
            }

        }
        catch
        {

        }
    }

    public void reporttotal()
    {
        sql = "select distinct Edu_Level  from Course where college_code ='" + ddlcollege.SelectedItem.Value + "' and type ='" + ddltype.SelectedItem.Text + "' order by Edu_Level desc";

        ds.Clear();
        ds = da.select_method_wo_parameter(sql, "Text");
        DataSet dsrpt1 = new DataSet();
        dsrpt1.Clear();
        ArrayList arr_rejectcol = new ArrayList();
        DataSet dsrpt1totst = new DataSet();
        dsrpt1totst.Clear();
        DataView dvcount = new DataView();
        string dummy = "";
        int totalfinalrow = 0;
        arr_rejectcol.Clear();
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            sql = "select * from Degree d,Department dt,Course c,DeptPrivilages dp where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and d.Degree_Code =dp.degree_code and user_code =" + usercode + " and Edu_Level ='" + ds.Tables[0].Rows[i][0].ToString() + "' and c.type ='" + ddltype.SelectedItem.ToString() + "'  order by c.course_id,d.degree_code asc";
            dsrpt1 = da.select_method_wo_parameter(sql, "Text");
            if (dummy != ds.Tables[0].Rows[i][0].ToString())
            {
                fpspread.Sheets[0].RowCount++;
                totalfinalrow = fpspread.Sheets[0].RowCount;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = ds.Tables[0].Rows[i][0].ToString();
                //if (!arr_rejectcol.Contains(fpspread.Sheets[0].RowCount - 1))
                //{
                //    arr_rejectcol.Add(fpspread.Sheets[0].RowCount - 1);
                //}
                fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#65ebca");
                dummy = ds.Tables[0].Rows[i][0].ToString();

            }

            for (int j = 0; j < dsrpt1.Tables[0].Rows.Count; j++)
            {
                fpspread.Sheets[0].RowCount++;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = (j + 1).ToString();
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = dsrpt1.Tables[0].Rows[j]["Course_Name"].ToString() + "-" + dsrpt1.Tables[0].Rows[j]["Dept_Name"].ToString();
                sql = "select * from applyn where college_code ='" + ddlcollege.SelectedItem.Value + "' and batch_year ='" + ddlbatch.SelectedItem.ToString() + "'   and degree_code='" + dsrpt1.Tables[0].Rows[j]["degree_code"].ToString() + "'";
                sql = sql + "  select * from applyn a,Registration r where a.app_no=r.App_No and CC =0 and DelFlag =0  and Exam_Flag ='OK' and r.degree_code =a.degree_code and admission_status ='1' and r.college_code ='" + ddlcollege.SelectedItem.Value + "' and r.batch_year ='" + ddlbatch.SelectedItem.ToString() + "'   and r.degree_code='" + dsrpt1.Tables[0].Rows[j]["degree_code"].ToString() + "'";
                sql = sql + "  select * from applyn a,Registration r where a.app_no=r.App_No and DelFlag ='1' and r.degree_code =a.degree_code and admission_status ='1' and r.college_code ='" + ddlcollege.SelectedItem.Value + "' and r.batch_year ='" + ddlbatch.SelectedItem.ToString() + "'   and r.degree_code='" + dsrpt1.Tables[0].Rows[j]["degree_code"].ToString() + "'";
                dsrpt1totst = da.select_method_wo_parameter(sql, "Text");
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = dsrpt1.Tables[0].Rows[j]["No_Of_seats"].ToString();
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                dsrpt1totst.Tables[0].DefaultView.RowFilter = "isconfirm  = 1 ";
                dvcount = dsrpt1totst.Tables[0].DefaultView;
                int count4 = 0;
                count4 = dvcount.Count;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(count4);
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

                dsrpt1totst.Tables[0].DefaultView.RowFilter = "admission_status  = 1 ";
                dvcount = dsrpt1totst.Tables[0].DefaultView;
                count4 = dvcount.Count;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(count4);
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;



                dsrpt1totst.Tables[1].DefaultView.RowFilter = "is_Enroll  = 2 ";
                dvcount = dsrpt1totst.Tables[1].DefaultView;
                count4 = dvcount.Count;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(count4);
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;

                int sr = Convert.ToInt32(dsrpt1.Tables[0].Rows[j]["No_Of_seats"]);
                sr = sr - count4;

                //dsrpt1totst.Tables[0].DefaultView.RowFilter = Convert.ToString(ss);
                //dvcount = dsrpt1totst.Tables[0].DefaultView;
                //count4 = dvcount.Count;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(sr);
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;

                //dsrpt1totst.Tables[1].DefaultView.RowFilter = "is_Enroll  = 2 ";
                //dvcount = dsrpt1totst.Tables[1].DefaultView;
                //count4 = dvcount.Count;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dsrpt1totst.Tables[2].Rows.Count);
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;

            }
            fpspread.Sheets[0].RowCount++;
            int counttotal = 0;
            int ExcessTotal = 0;
            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Total";
            fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, 2);
            for (int mm = 2; mm < fpspread.Sheets[0].Columns.Count; mm++)
            {
                ExcessTotal = 0;
                for (int m = totalfinalrow; m < fpspread.Sheets[0].RowCount - 1; m++)
                {
                    if (Convert.ToInt32(fpspread.Sheets[0].Cells[m, mm].Text) >= 0)
                    {
                        counttotal = counttotal + Convert.ToInt32(fpspread.Sheets[0].Cells[m, mm].Text);
                    }
                    else
                    {
                        ExcessTotal += Convert.ToInt32(fpspread.Sheets[0].Cells[m, mm].Text);
                    }

                }
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, mm].Text = Convert.ToString(counttotal);
                if (ExcessTotal < 0)
                {
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, mm].Text = Convert.ToString(counttotal + "/" + ExcessTotal);
                }
                fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#dceaf9");
                if (!arr_rejectcol.Contains(fpspread.Sheets[0].RowCount - 1))
                {
                    arr_rejectcol.Add(fpspread.Sheets[0].RowCount - 1);
                }

                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, mm].HorizontalAlign = HorizontalAlign.Center;
                counttotal = 0;
            }

            if (fpspread.Sheets[0].RowCount > 0)
            {
                fpspread.Visible = true;
                rptprint.Visible = true;

            }

        }
        if (fpspread.Sheets[0].RowCount > 0)
        {
            fpspread.Sheets[0].RowCount++;
            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
            fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, 2);
            int countgrandtotal = 0;
            int ExcessCountTotal = 0;
            for (int j = 2; j < fpspread.Sheets[0].Columns.Count; j++)
            {
                ExcessCountTotal = 0;
                //int rowss = Convert.ToInt32(arr_rejectcol[j].ToString());
                for (int i = 0; i < arr_rejectcol.Count; i++)
                {
                    int rowss = Convert.ToInt32(arr_rejectcol[i].ToString());

                    if (fpspread.Sheets[0].Cells[rowss, j].Text.ToString().Contains("/") == false)
                    {
                        countgrandtotal = countgrandtotal + Convert.ToInt32(fpspread.Sheets[0].Cells[rowss, j].Text.ToString());
                    }
                    else
                    {
                        string[] split = fpspread.Sheets[0].Cells[rowss, j].Text.Split('/');
                        countgrandtotal = countgrandtotal + Convert.ToInt32(split[0]);
                        if (split.Length > 1)
                        {
                            ExcessCountTotal = Convert.ToInt32(split[1]);
                        }
                    }

                }
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, j].Text = Convert.ToString(countgrandtotal);
                if (ExcessCountTotal < 0)
                {
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, j].Text = Convert.ToString(countgrandtotal + "/" + ExcessCountTotal);
                }
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, j].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#dceaf9");
                countgrandtotal = 0;
            }
        }
        fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
        fpspread.SaveChanges();
    }

    public void reportcommunity()
    {
        sql = "select distinct Edu_Level  from Course where college_code ='" + ddlcollege.SelectedItem.Value + "' and type ='" + ddltype.SelectedItem.Text + "' order by Edu_Level desc";

        ds.Clear();
        ds = da.select_method_wo_parameter(sql, "Text");
        DataSet dsrpt1 = new DataSet();
        dsrpt1.Clear();
        ArrayList arr_rejectcol = new ArrayList();
        arr_rejectcol.Clear();
        DataSet dsrpt1totst = new DataSet();
        dsrpt1totst.Clear();
        DataView dvcount = new DataView();
        string dummy = "";
        int totalfinalrow = 0;
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            sql = "select * from Degree d,Department dt,Course c,DeptPrivilages dp where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and d.Degree_Code =dp.degree_code and user_code =" + usercode + " and Edu_Level ='" + ds.Tables[0].Rows[i][0].ToString() + "' and c.type ='" + ddltype.SelectedItem.ToString() + "'";
            dsrpt1 = da.select_method_wo_parameter(sql, "Text");
            if (dummy != ds.Tables[0].Rows[i][0].ToString())
            {
                fpspread.Sheets[0].RowCount++;
                totalfinalrow = fpspread.Sheets[0].RowCount;

                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = ds.Tables[0].Rows[i][0].ToString();
                fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#65ebca");
                dummy = ds.Tables[0].Rows[i][0].ToString();

            }

            for (int j = 0; j < dsrpt1.Tables[0].Rows.Count; j++)
            {
                fpspread.Sheets[0].RowCount++;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = (j + 1).ToString();
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = dsrpt1.Tables[0].Rows[j]["Dept_Name"].ToString();
                sql = "select * from applyn where college_code ='" + ddlcollege.SelectedItem.Value + "' and batch_year ='" + ddlbatch.SelectedItem.ToString() + "'   and degree_code='" + dsrpt1.Tables[0].Rows[j]["degree_code"].ToString() + "'";
                dsrpt1totst = da.select_method_wo_parameter(sql, "Text");
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = dsrpt1.Tables[0].Rows[j]["No_Of_seats"].ToString();
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                int count4 = 0;
                string applycomcols = hiddenvalueapplied.Text;
                string[] splitapplycomcols = applycomcols.Split('-');
                if (splitapplycomcols.GetUpperBound(0) > 0)
                {


                    for (int k = Convert.ToInt32(splitapplycomcols[0]); k < Convert.ToInt32(splitapplycomcols[1]); k++)
                    {
                        string commtextcode = fpspread.Sheets[0].ColumnHeader.Cells[1, k].Note;
                        dsrpt1totst.Tables[0].DefaultView.RowFilter = "isconfirm  = 1 and community=" + commtextcode + "";
                        dvcount = dsrpt1totst.Tables[0].DefaultView;

                        count4 = dvcount.Count;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, k].Text = Convert.ToString(count4);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, k].HorizontalAlign = HorizontalAlign.Center;
                    }
                }

                string admitcomcols = hiddenvalueadmitted.Text;
                string[] splitadmitcomcols = admitcomcols.Split('-');
                int totalvalue = 0;
                if (splitadmitcomcols.GetUpperBound(0) > 0)
                {

                    for (int k = Convert.ToInt32(splitadmitcomcols[0]); k < Convert.ToInt32(splitadmitcomcols[1]); k++)
                    {
                        string commtextcode = fpspread.Sheets[0].ColumnHeader.Cells[1, k].Note;
                        dsrpt1totst.Tables[0].DefaultView.RowFilter = "admission_status  = 1 and community= " + commtextcode + " ";
                        dvcount = dsrpt1totst.Tables[0].DefaultView;
                        count4 = dvcount.Count;
                        totalvalue = totalvalue + dvcount.Count;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, k].Text = Convert.ToString(count4);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, k].HorizontalAlign = HorizontalAlign.Center;
                    }
                }

                int sr = Convert.ToInt32(dsrpt1.Tables[0].Rows[j]["No_Of_seats"]);
                sr = sr - totalvalue;

                //dsrpt1totst.Tables[0].DefaultView.RowFilter = Convert.ToString(ss);
                //dvcount = dsrpt1totst.Tables[0].DefaultView;
                //count4 = dvcount.Count;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, fpspread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(sr);
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, fpspread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            }

            fpspread.Sheets[0].RowCount++;
            int counttotal = 0;
            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Total";
            fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, 2);
            for (int mm = 2; mm < fpspread.Sheets[0].Columns.Count; mm++)
            {
                for (int m = totalfinalrow; m < fpspread.Sheets[0].RowCount - 2; m++)
                {
                    counttotal = counttotal + Convert.ToInt32(fpspread.Sheets[0].Cells[m, mm].Text);
                }
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, mm].Text = Convert.ToString(counttotal);
                fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#dceaf9");
                if (!arr_rejectcol.Contains(fpspread.Sheets[0].RowCount - 1))
                {
                    arr_rejectcol.Add(fpspread.Sheets[0].RowCount - 1);
                }

                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, mm].HorizontalAlign = HorizontalAlign.Center;
                counttotal = 0;
            }

            fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
            fpspread.SaveChanges();

            if (fpspread.Sheets[0].RowCount > 0)
            {
                fpspread.Visible = true;
                rptprint.Visible = true;
            }

        }
        if (fpspread.Sheets[0].RowCount > 0)
        {
            fpspread.Sheets[0].RowCount++;
            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
            fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, 2);
            int countgrandtotal = 0;
            for (int j = 2; j < fpspread.Sheets[0].Columns.Count; j++)
            {

                //int rowss = Convert.ToInt32(arr_rejectcol[j].ToString());
                for (int i = 0; i < arr_rejectcol.Count; i++)
                {
                    int rowss = Convert.ToInt32(arr_rejectcol[i].ToString());

                    countgrandtotal = countgrandtotal + Convert.ToInt32(fpspread.Sheets[0].Cells[rowss, j].Text.ToString());

                }
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, j].Text = Convert.ToString(countgrandtotal);
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, j].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#dceaf9");
                countgrandtotal = 0;
            }
        }
        fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
        fpspread.SaveChanges();
    }

    public void reportreligion()
    {
        sql = "select distinct Edu_Level  from Course where college_code ='" + ddlcollege.SelectedItem.Value + "' and type ='" + ddltype.SelectedItem.Text + "' order by Edu_Level desc";

        ds.Clear();
        ds = da.select_method_wo_parameter(sql, "Text");
        DataSet dsrpt1 = new DataSet();
        ArrayList arr_rejectcol = new ArrayList();
        dsrpt1.Clear();
        DataSet dsrpt1totst = new DataSet();
        dsrpt1totst.Clear();
        DataView dvcount = new DataView();
        string dummy = "";
        int totalfinalrow = 0;
        arr_rejectcol.Clear();
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            sql = "select * from Degree d,Department dt,Course c,DeptPrivilages dp where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and d.Degree_Code =dp.degree_code and user_code =" + usercode + " and Edu_Level ='" + ds.Tables[0].Rows[i][0].ToString() + "' and c.type ='" + ddltype.SelectedItem.ToString() + "'";
            dsrpt1 = da.select_method_wo_parameter(sql, "Text");
            if (dummy != ds.Tables[0].Rows[i][0].ToString())
            {
                fpspread.Sheets[0].RowCount++;
                totalfinalrow = fpspread.Sheets[0].RowCount;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = ds.Tables[0].Rows[i][0].ToString();
                fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#65ebca");
                dummy = ds.Tables[0].Rows[i][0].ToString();

            }
            for (int j = 0; j < dsrpt1.Tables[0].Rows.Count; j++)
            {
                fpspread.Sheets[0].RowCount++;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = (j + 1).ToString();
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = dsrpt1.Tables[0].Rows[j]["Dept_Name"].ToString();
                sql = "select * from applyn where college_code ='" + ddlcollege.SelectedItem.Value + "' and batch_year ='" + ddlbatch.SelectedItem.ToString() + "'   and degree_code='" + dsrpt1.Tables[0].Rows[j]["degree_code"].ToString() + "'";
                dsrpt1totst = da.select_method_wo_parameter(sql, "Text");
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = dsrpt1.Tables[0].Rows[j]["No_Of_seats"].ToString();
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                int count4 = 0;
                int totalcount = 0;
                string applycomcols = hiddenvalueapplied.Text;
                string[] splitapplycomcols = applycomcols.Split('-');
                if (splitapplycomcols.GetUpperBound(0) > 0)
                {


                    for (int k = Convert.ToInt32(splitapplycomcols[0]); k < Convert.ToInt32(splitapplycomcols[1]); k++)
                    {
                        string commtextcode = fpspread.Sheets[0].ColumnHeader.Cells[1, k].Note;
                        dsrpt1totst.Tables[0].DefaultView.RowFilter = "isconfirm  = 1 and religion=" + commtextcode + "";
                        dvcount = dsrpt1totst.Tables[0].DefaultView;

                        count4 = dvcount.Count;

                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, k].Text = Convert.ToString(count4);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, k].HorizontalAlign = HorizontalAlign.Center;
                    }
                }

                string admitcomcols = hiddenvalueadmitted.Text;
                string[] splitadmitcomcols = admitcomcols.Split('-');
                if (splitadmitcomcols.GetUpperBound(0) > 0)
                {

                    for (int k = Convert.ToInt32(splitadmitcomcols[0]); k < Convert.ToInt32(splitadmitcomcols[1]); k++)
                    {
                        string commtextcode = fpspread.Sheets[0].ColumnHeader.Cells[1, k].Note;
                        dsrpt1totst.Tables[0].DefaultView.RowFilter = "admission_status  = 1 and religion= " + commtextcode + " ";
                        dvcount = dsrpt1totst.Tables[0].DefaultView;
                        count4 = dvcount.Count;
                        totalcount = totalcount + dvcount.Count;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, k].Text = Convert.ToString(count4);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, k].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
                int sr = Convert.ToInt32(dsrpt1.Tables[0].Rows[j]["No_Of_seats"]);
                sr = sr - totalcount;

                //dsrpt1totst.Tables[0].DefaultView.RowFilter = Convert.ToString(ss);
                //dvcount = dsrpt1totst.Tables[0].DefaultView;
                //count4 = dvcount.Count;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, fpspread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(sr);
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, fpspread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            }

            fpspread.Sheets[0].RowCount++;
            int counttotal = 0;
            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Total";
            fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, 2);
            for (int mm = 2; mm < fpspread.Sheets[0].Columns.Count; mm++)
            {
                for (int m = totalfinalrow; m < fpspread.Sheets[0].RowCount - 2; m++)
                {
                    counttotal = counttotal + Convert.ToInt32(fpspread.Sheets[0].Cells[m, mm].Text);
                }
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, mm].Text = Convert.ToString(counttotal);
                fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#dceaf9");
                if (!arr_rejectcol.Contains(fpspread.Sheets[0].RowCount - 1))
                {
                    arr_rejectcol.Add(fpspread.Sheets[0].RowCount - 1);
                }

                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, mm].HorizontalAlign = HorizontalAlign.Center;
                counttotal = 0;
            }
            fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
            fpspread.SaveChanges();

            if (fpspread.Sheets[0].RowCount > 0)
            {
                fpspread.Visible = true;
                rptprint.Visible = true;
            }

        }
        if (fpspread.Sheets[0].RowCount > 0)
        {
            fpspread.Sheets[0].RowCount++;
            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
            fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, 2);
            int countgrandtotal = 0;
            for (int j = 2; j < fpspread.Sheets[0].Columns.Count; j++)
            {

                //int rowss = Convert.ToInt32(arr_rejectcol[j].ToString());
                for (int i = 0; i < arr_rejectcol.Count; i++)
                {
                    int rowss = Convert.ToInt32(arr_rejectcol[i].ToString());

                    countgrandtotal = countgrandtotal + Convert.ToInt32(fpspread.Sheets[0].Cells[rowss, j].Text.ToString());

                }
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, j].Text = Convert.ToString(countgrandtotal);
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, j].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#dceaf9");
                countgrandtotal = 0;
            }
        }
        fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
        fpspread.SaveChanges();
    }

    public void report()
    {
        ddlreport.Items.Add("Total Strength");
        ddlreport.Items.Add("Category Wise");
        ddlreport.Items.Add("Quota Wise Report");
        ddlreport.Items.Add("Community");
        ddlreport.Items.Add("Religion");
        ddlreport.Items.Add("Enrollment Confirm Report");
        ddlreport.DataBind();

    }

    protected void btnExcel_Click(object sender, EventArgs e)
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
                    //FpEntry.SaveExcel(appPath + "/Report/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + print + ".xls" + " saved in" + " " + appPath + "/Report" + " successfully')", true);
                    //Aruna on 26feb2013============================
                    string szPath = appPath + "/Report/";
                    string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")

                    fpspread.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Flush();
                    Response.WriteFile(szPath + szFile);
                    //=============================================
                }
                else
                {
                    lblnorec.Text = "Please enter your Report Name";
                    lblnorec.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
        }

    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string Reportname = "";
        if (ddltype.SelectedItem.Text == "DAY")
        {
            Reportname = "Govt Aided (Day)";
        }
        else if (ddltype.SelectedItem.Text == "Evening")
        {
            Reportname = "SFS(Evening)";
        }

        string degreedetails = string.Empty;

        degreedetails = "Admission Report for " + Reportname + " 2017 - 18 " + '@' + "DATE:   " + System.DateTime.Now.ToString("dd/MM/yyyy") + "";
        string pagename = "newreport.aspx";

        Printcontrol.loadspreaddetails(fpspread, pagename, degreedetails);
        Printcontrol.Visible = true;

    }
    //protected void FpSpread2_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{
    //}
    //  protected void fpspread_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{

    //    cellclick = true;


    //}
    //protected void fpspread_SelectedIndexChanged(Object sender, EventArgs e)
    //{
    //    try
    //    {
    //    }
    //    catch
    //    {

    //    }
    //}

}