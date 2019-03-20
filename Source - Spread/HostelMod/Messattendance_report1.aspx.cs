using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using FarPoint.Web.Spread;
using System.Configuration;

public partial class HostelMod_Messattendance_report1 : System.Web.UI.Page
{
    ReuasableMethods rs = new ReuasableMethods();
    DataSet ds = new DataSet(); DataSet ds1 = new DataSet();
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string college_code = string.Empty;
    Boolean cellclick = false;
    string q1 = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        //****************************************************//
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("hostelindex"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/HostelMod/hostelindex.aspx");
                return;
            }
        }
        //****************************************************//
        if (Session["usercode"] != null)
        {
            usercode = Session["usercode"].ToString();
        }
        college_code = Session["collegecode"].ToString();
        if (!IsPostBack)
        {
            Txtentryfrom.Text = System.DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
            Txtentryto.Text = System.DateTime.Now.ToString("dd/MM/yyyy");

            Txtentryfrom_CalendarExtender.EndDate = System.DateTime.Now;
            Txtentryto_CalendarExtender.EndDate = System.DateTime.Now;
            Bind_messname();
            Bind_session();
        }
    }
    protected void cb_messname_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_messname, cb_messname, txt_messname, lblmessname.Text);
        Bind_session();
    }
    protected void cbl_messname_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_messname, cb_messname, txt_messname, lblmessname.Text);
        Bind_session();
    }
    protected void cb_session_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_session, cb_session, txtsession, "Session Name");
    }
    protected void cbl_session_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_session, cb_session, txtsession, "Session Name");
    }
    void Bind_messname()
    {
        ds.Clear();
        ds = d2.Bindmess_basedonrights(Session["usercode"].ToString(), Convert.ToString(Session["collegecode"]));
        cbl_messname.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_messname.DataSource = ds;
            cbl_messname.DataTextField = "MessName";
            cbl_messname.DataValueField = "MessMasterPK";
            cbl_messname.DataBind();
        }
        for (int i = 0; i < cbl_messname.Items.Count; i++)
        {
            cbl_messname.Items[i].Selected = true;
        }
        txt_messname.Text = "MessName (" + cbl_messname.Items.Count + ")";
    }
    void Bind_session()
    {
        cbl_session.Items.Clear();
        ds.Clear();
        string hostel = rs.GetSelectedItemsValueAsString(cbl_messname);
        ds = d2.BindSession_inv(hostel);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_session.DataSource = ds.Tables[0];
            cbl_session.DataTextField = "sessionname";
            cbl_session.DataValueField = "SessionMasterPK";
            cbl_session.DataBind();
        }
        for (int i = 0; i < cbl_session.Items.Count; i++)
        {
            cbl_session.Items[i].Selected = true;
        }
        txtsession.Text = "SessionName (" + cbl_session.Items.Count + ")";
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            Hashtable totalvalue_dic = new Hashtable(); Printcontrol.Visible = false; Printcontrol1.Visible = false; string percent = "";
            if (cbl_messname.Items.Count > 0 && cbl_session.Items.Count > 0)
            {
                if (txtsession.Text.Trim() != "--Select--" && txt_messname.Text.Trim() != "--Select--")
                {
                    string sessionfk = rs.GetSelectedItemsValueAsString(cbl_session);
                    string messfk = rs.GetSelectedItemsValueAsString(cbl_messname);

                    if (rdopercentage.Checked == true)
                        percent = "(%)";
                    else
                        percent = "";

                    #region Header Name
                    Fpcumulative.Sheets[0].RowCount = 0;
                    Fpcumulative.Sheets[0].ColumnCount = 0;
                    Fpcumulative.CommandBar.Visible = false;
                    Fpcumulative.Sheets[0].AutoPostBack = true;
                    Fpcumulative.Sheets[0].ColumnHeader.RowCount = 2;
                    Fpcumulative.Sheets[0].RowHeader.Visible = false;
                    Fpcumulative.Sheets[0].ColumnCount = 3;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpcumulative.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpcumulative.Columns[0].Width = 50;

                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpcumulative.Columns[1].Width = 100;

                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Session Name";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpcumulative.Columns[2].Width = 200;

                    Fpcumulative.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                    Fpcumulative.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                    Fpcumulative.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

                    DateTime dt = new DateTime();
                    DateTime dt1 = new DateTime();
                    DateTime dt2 = new DateTime();
                    string firstdate = Convert.ToString(Txtentryfrom.Text);
                    string seconddate = Convert.ToString(Txtentryto.Text);
                    string[] split = firstdate.Split('/');
                    dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                    string[] split1 = seconddate.Split('/');
                    dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
                    dt2 = dt;
                    int k = 0;
                    Fpcumulative.Sheets[0].ColumnCount++;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Text = "Hostler";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "Hostler";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    Fpcumulative.Columns[Fpcumulative.Sheets[0].ColumnCount - 1].Width = 100;

                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Text = "Present" + percent + "";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "HP";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    Fpcumulative.Columns[Fpcumulative.Sheets[0].ColumnCount - 1].Width = 100;

                    Fpcumulative.Sheets[0].Columns[Fpcumulative.Sheets[0].ColumnCount - 1].ForeColor = Color.Blue;

                    Fpcumulative.Sheets[0].ColumnCount++;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "Hostler";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Text = "Absent" + percent + "";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "HA";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    Fpcumulative.Columns[Fpcumulative.Sheets[0].ColumnCount - 1].Width = 100;
                    Fpcumulative.Sheets[0].Columns[Fpcumulative.Sheets[0].ColumnCount - 1].ForeColor = Color.Blue;
                    Fpcumulative.Sheets[0].ColumnCount++;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "Hostler";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Text = "Total" + percent + "";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "HT";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    Fpcumulative.Columns[Fpcumulative.Sheets[0].ColumnCount - 1].Width = 100;
                    Fpcumulative.Sheets[0].ColumnHeaderSpanModel.Add(0, Fpcumulative.Sheets[0].ColumnCount - 3, 1, 3);
                    Fpcumulative.Sheets[0].ColumnCount++;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Text = "Day Scholar";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "Day Scholar";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    Fpcumulative.Columns[Fpcumulative.Sheets[0].ColumnCount - 1].Width = 100;
                    Fpcumulative.Sheets[0].Columns[Fpcumulative.Sheets[0].ColumnCount - 1].ForeColor = Color.Blue;


                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Text = "Present" + percent + "";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "DP";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    Fpcumulative.Columns[Fpcumulative.Sheets[0].ColumnCount - 1].Width = 100;
                    Fpcumulative.Sheets[0].Columns[Fpcumulative.Sheets[0].ColumnCount - 1].ForeColor = Color.Blue;
                    Fpcumulative.Sheets[0].ColumnCount++;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Text = "Absent" + percent + "";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "Day Scholar";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "DA";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    Fpcumulative.Columns[Fpcumulative.Sheets[0].ColumnCount - 1].Width = 100;
                    Fpcumulative.Sheets[0].Columns[Fpcumulative.Sheets[0].ColumnCount - 1].ForeColor = Color.Blue;
                    Fpcumulative.Sheets[0].ColumnCount++;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Text = "Total" + percent + "";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "Day Scholar";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "DT";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    Fpcumulative.Columns[Fpcumulative.Sheets[0].ColumnCount - 1].Width = 100;
                    Fpcumulative.Sheets[0].ColumnHeaderSpanModel.Add(0, Fpcumulative.Sheets[0].ColumnCount - 3, 1, 3);


                    Fpcumulative.Sheets[0].ColumnCount++;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Text = "Staff";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "Staff";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    Fpcumulative.Columns[Fpcumulative.Sheets[0].ColumnCount - 1].Width = 100;
                    Fpcumulative.Sheets[0].Columns[Fpcumulative.Sheets[0].ColumnCount - 1].ForeColor = Color.Blue;

                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Text = "Present" + percent + "";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "SP";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "Staff";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    Fpcumulative.Columns[Fpcumulative.Sheets[0].ColumnCount - 1].Width = 100;
                    Fpcumulative.Sheets[0].Columns[Fpcumulative.Sheets[0].ColumnCount - 1].ForeColor = Color.Blue;
                    Fpcumulative.Sheets[0].ColumnCount++;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Text = "Absent" + percent + "";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "SA";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "Staff";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    Fpcumulative.Columns[Fpcumulative.Sheets[0].ColumnCount - 1].Width = 100;
                    Fpcumulative.Sheets[0].Columns[Fpcumulative.Sheets[0].ColumnCount - 1].ForeColor = Color.Blue;
                    Fpcumulative.Sheets[0].ColumnCount++;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Text = "Total" + percent + "";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "ST";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "Staff";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    Fpcumulative.Columns[Fpcumulative.Sheets[0].ColumnCount - 1].Width = 100;
                    Fpcumulative.Sheets[0].ColumnHeaderSpanModel.Add(0, Fpcumulative.Sheets[0].ColumnCount - 3, 1, 3);

                    Fpcumulative.Sheets[0].ColumnCount++;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Text = "GrandTotal";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "OGT";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    Fpcumulative.Columns[Fpcumulative.Sheets[0].ColumnCount - 1].Width = 100;

                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Text = "Grand Present" + percent + "";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "OGT";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "GP";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    Fpcumulative.Columns[Fpcumulative.Sheets[0].ColumnCount - 1].Width = 100; Fpcumulative.Sheets[0].Columns[Fpcumulative.Sheets[0].ColumnCount - 1].ForeColor = Color.Blue;

                    Fpcumulative.Sheets[0].ColumnCount++;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Text = "Grand Absent" + percent + "";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "OGT";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "GA";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    Fpcumulative.Columns[Fpcumulative.Sheets[0].ColumnCount - 1].Width = 100; Fpcumulative.Sheets[0].Columns[Fpcumulative.Sheets[0].ColumnCount - 1].ForeColor = Color.Blue;
                    Fpcumulative.Sheets[0].ColumnCount++;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Text = "Grand Total" + percent + "";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "OGT";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Tag = "GT";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpcumulative.Sheets[0].Columns[Fpcumulative.Sheets[0].ColumnCount - 1].ForeColor = Color.Blue;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[1, Fpcumulative.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    Fpcumulative.Columns[Fpcumulative.Sheets[0].ColumnCount - 1].Width = 100;
                    Fpcumulative.Sheets[0].ColumnHeaderSpanModel.Add(0, Fpcumulative.Sheets[0].ColumnCount - 3, 1, 3);

                    #endregion

                    q1 += " select COUNT(h.App_No)Totalstudent,case when Is_Staff='0' then 'Student' when Is_Staff='1' then 'Staff' end Is_Staff,Session_name,Session_Code,Entry_Date,Hostel_Code from HostelMess_Attendance h,HT_HostelRegistration ht where h.App_No=ht.APP_No and hostel_code in('" + messfk + "') and Session_Code in('" + sessionfk + "') and RoomFK<>'0' group by Is_Staff,Session_name,Session_Code,Entry_Date,Hostel_Code ";

                    q1 += " select COUNT(h.App_No)Totalstudent,case when Is_Staff='0' then 'Student' when Is_Staff='1' then 'Staff' end Is_Staff,Session_name,Session_Code,Entry_Date,Hostel_Code from HostelMess_Attendance h,Registration ht where h.App_No=ht.APP_No and ht.stud_type='Day Scholar' and hostel_code in('" + messfk + "') and Session_Code in('" + sessionfk + "') group by Is_Staff,Session_name,Session_Code,Entry_Date,Hostel_Code ";

                    q1 += " select COUNT(App_No)Totalstudent,case when Is_Staff='0' then 'Student' when Is_Staff='1' then 'Staff' end Is_Staff,Session_name,Session_Code,Entry_Date,Hostel_Code from HostelMess_Attendance h where  hostel_code in('" + messfk + "') and Session_Code in('" + sessionfk + "') and Is_Staff='1' group by Is_Staff,Session_name,Session_Code,Entry_Date,Hostel_Code";

                    q1 += " select COUNT(App_No) Hostelcount,MessMasterFK from HT_HostelRegistration h,HM_HostelMaster hm where h.HostelMasterFK=hm.HostelMasterPK and RoomFK<>'0' group by MessMasterFK";
                    q1 += " SELECT Count(App_No)total FROM registration where Stud_Type='Day Scholar' ";
                    q1 += " select COUNT(appl_id)staffcount from staffmaster s,staff_appl_master sa where s.appl_no=sa.appl_no and settled=0 and resign =0";
                    ds1.Clear();
                    ds1 = d2.select_method_wo_parameter(q1, "text");
                    double grandpersent = 0; double grandabsent = 0; double grandtotal = 0;
                    string value = "";
                    double total = 0; double val = 0; double hper = 0; double totalper = 0;
                    double hostelstudentPresentcount = 0;
                    double hostelstudentTotalcount = 0;
                    double daystudentPresentcount = 0;
                    double daystudenttotalcount = 0;
                    double staffPresentcount = 0;
                    double stafftotalcount = 0;
                    while (dt2 <= dt1)
                    {
                        k++;
                        q1 = "select * from HostelIns_settings where Schedule_date='" + dt2.ToString("MM/dd/yyyy") + "' and Session_code in('" + sessionfk + "') and Hostel_code in('" + messfk + "')";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(q1, "text");
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            q1 = "select * from HostelIns_settings where Schedule_Day='" + dt2.ToString("dddd") + "' and Session_code in('" + sessionfk + "') and Hostel_code in('" + messfk + "')";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(q1, "text");
                        }
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                hper = 0; totalper = 0;
                                Fpcumulative.Sheets[0].RowCount++;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(k);
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dt2.ToString("dd/MM/yyyy"));
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dt2.ToString("MM/dd/yyyy"));
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(cbl_session.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[i]["Session_code"])));
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(Convert.ToString(ds.Tables[0].Rows[i]["Session_code"]));

                                string isstaff = "";
                                hostelstudentPresentcount = 0;
                                hostelstudentTotalcount = 0;
                                daystudentPresentcount = 0;
                                daystudenttotalcount = 0;
                                staffPresentcount = 0;
                                stafftotalcount = 0;

                                #region Settings Based Show column

                                //Fpcumulative.Sheets[0].Columns[4].Visible = false;
                                //Fpcumulative.Sheets[0].Columns[5].Visible = false;
                                //Fpcumulative.Sheets[0].Columns[3].Visible = false;
                                //Fpcumulative.Sheets[0].Columns[6].Visible = false;
                                //Fpcumulative.Sheets[0].Columns[7].Visible = false;
                                //Fpcumulative.Sheets[0].Columns[8].Visible = false;
                                //Fpcumulative.Sheets[0].Columns[9].Visible = false;
                                //Fpcumulative.Sheets[0].Columns[10].Visible = false;
                                //Fpcumulative.Sheets[0].Columns[11].Visible = false;

                                #endregion

                                string mess_settings = Convert.ToString(ds.Tables[0].Rows[i]["Mess_attendance_set"]);
                                if (mess_settings.Contains("H"))
                                {
                                    isstaff = " and Is_Staff='Student'";
                                    double.TryParse(Convert.ToString(ds1.Tables[3].Compute("Sum(Hostelcount)", " MessMasterFK='" + Convert.ToString(ds.Tables[0].Rows[i]["Hostel_code"]) + "'")), out hostelstudentTotalcount);

                                    double.TryParse(Convert.ToString(ds1.Tables[0].Compute("Sum(Totalstudent)", " hostel_code in ('" + Convert.ToString(ds.Tables[0].Rows[i]["Hostel_code"]) + "') and Session_Code in('" + Convert.ToString(ds.Tables[0].Rows[i]["Session_code"]) + "') and Entry_Date ='" + dt2.ToString("MM/dd/yyyy") + "' " + isstaff + "")), out hostelstudentPresentcount);

                                    //Fpcumulative.Sheets[0].Columns[4].Visible = true;
                                    //Fpcumulative.Sheets[0].Columns[5].Visible = true;
                                    //Fpcumulative.Sheets[0].Columns[3].Visible = true;
                                }
                                if (mess_settings.Contains("D"))
                                {
                                    isstaff = " and Is_Staff='Student'";
                                    double.TryParse(Convert.ToString(ds1.Tables[4].Compute("Sum(total)", "")), out daystudenttotalcount);
                                    double.TryParse(Convert.ToString(ds1.Tables[1].Compute("Sum(Totalstudent)", " hostel_code in('" + Convert.ToString(ds.Tables[0].Rows[i]["Hostel_code"]) + "') and Session_Code in('" + Convert.ToString(ds.Tables[0].Rows[i]["Session_code"]) + "') and Entry_Date ='" + dt2.ToString("MM/dd/yyyy") + "' " + isstaff + "")), out daystudentPresentcount);
                                    //Fpcumulative.Sheets[0].Columns[6].Visible = true;
                                    //Fpcumulative.Sheets[0].Columns[7].Visible = true;
                                    //Fpcumulative.Sheets[0].Columns[8].Visible = true;
                                }
                                if (mess_settings.Contains("S"))
                                {
                                    isstaff = " and Is_Staff='Staff'";
                                    double.TryParse(Convert.ToString(ds1.Tables[5].Compute("Sum(staffcount)", "")), out stafftotalcount);
                                    double.TryParse(Convert.ToString(ds1.Tables[2].Compute("Sum(Totalstudent)", " hostel_code in('" + Convert.ToString(ds.Tables[0].Rows[i]["Hostel_code"]) + "') and Session_Code in('" + Convert.ToString(ds.Tables[0].Rows[i]["Session_code"]) + "') and Entry_Date ='" + dt2.ToString("MM/dd/yyyy") + "' " + isstaff + "")), out staffPresentcount);
                                    //Fpcumulative.Sheets[0].Columns[9].Visible = true;
                                    //Fpcumulative.Sheets[0].Columns[10].Visible = true;
                                    //Fpcumulative.Sheets[0].Columns[11].Visible = true;
                                }

                                #region Value binding

                                //Hostler
                                if (Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[1, 3].Tag).Trim() == "HP")
                                {
                                    if (rdopercentage.Checked == true)
                                    {
                                        hper = hostelstudentPresentcount / hostelstudentTotalcount * 100;
                                        if (Convert.ToString(hper) != "NaN")
                                            Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(Math.Round(hper, 2));
                                        else
                                            Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 3].Text = " - ";
                                    }
                                    else
                                    {
                                        Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(hostelstudentPresentcount);
                                    }
                                }
                                if (Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[1, 4].Tag).Trim() == "HA")
                                {
                                    if (rdopercentage.Checked == true)
                                    {
                                        hper = (hostelstudentTotalcount - hostelstudentPresentcount) / hostelstudentTotalcount * 100;
                                        if (Convert.ToString(hper) != "NaN")
                                            Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Math.Round(hper, 2));
                                        else
                                            Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 4].Text = " - ";
                                    }
                                    else
                                    {
                                        Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(hostelstudentTotalcount - hostelstudentPresentcount);
                                    }
                                }
                                if (Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[1, 5].Tag).Trim() == "HT")
                                {
                                    if (rdopercentage.Checked == true)
                                    {
                                        hper = hostelstudentTotalcount / hostelstudentTotalcount * 100;
                                        if (Convert.ToString(hper) != "NaN")
                                            Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(Math.Round(hper, 2));
                                        else
                                            Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 5].Text = " - ";
                                    }
                                    else
                                    {
                                        Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(hostelstudentTotalcount);
                                    }
                                }
                                //Days Scholar
                                if (Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[1, 6].Tag).Trim() == "DP")
                                {
                                    if (rdopercentage.Checked == true)
                                    {
                                        hper = daystudentPresentcount / daystudenttotalcount * 100;
                                        if (Convert.ToString(hper) != "NaN")
                                            Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(Math.Round(hper, 2));
                                        else
                                            Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 6].Text = " - ";

                                    }
                                    else
                                    {
                                        Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(daystudentPresentcount);
                                    }
                                }
                                if (Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[1, 7].Tag).Trim() == "DA")
                                {
                                    if (rdopercentage.Checked == true)
                                    {
                                        hper = (daystudenttotalcount - daystudentPresentcount) / daystudenttotalcount * 100;
                                        if (Convert.ToString(hper) != "NaN")
                                            Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(Math.Round(hper, 2));
                                        else
                                            Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 7].Text = " - ";
                                    }
                                    else
                                    {
                                        Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(daystudenttotalcount - daystudentPresentcount);
                                    }
                                }
                                if (Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[1, 8].Tag).Trim() == "DT")
                                {
                                    if (rdopercentage.Checked == true)
                                    {
                                        hper = daystudenttotalcount / daystudenttotalcount * 100;
                                        if (Convert.ToString(hper) != "NaN")
                                            Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(Math.Round(hper, 2));
                                        else
                                            Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 8].Text = " - ";
                                    }
                                    else
                                    {
                                        Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(daystudenttotalcount);
                                    }
                                }
                                //staff
                                if (Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[1, 9].Tag).Trim() == "SP")
                                {
                                    if (rdopercentage.Checked == true)
                                    {
                                        hper = staffPresentcount / stafftotalcount * 100;
                                        if (Convert.ToString(hper) != "NaN")
                                            Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(Math.Round(hper, 2));
                                        else
                                            Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 9].Text = " - ";
                                    }
                                    else
                                    {
                                        Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(staffPresentcount);
                                    }
                                }
                                if (Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[1, 10].Tag).Trim() == "SA")
                                {
                                    if (rdopercentage.Checked == true)
                                    {
                                        hper = (stafftotalcount - staffPresentcount) / stafftotalcount * 100;
                                        if (Convert.ToString(hper) != "NaN")
                                            Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(Math.Round(hper, 2));
                                        else
                                            Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 10].Text = " - ";
                                    }
                                    else
                                    {
                                        Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(stafftotalcount - staffPresentcount);
                                    }
                                }
                                if (Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[1, 11].Tag).Trim() == "ST")
                                {
                                    if (rdopercentage.Checked == true)
                                    {
                                        hper = stafftotalcount / stafftotalcount * 100;
                                        if (Convert.ToString(hper) != "NaN")
                                            Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(Math.Round(hper, 2));
                                        else
                                            Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 11].Text = " - ";
                                    }
                                    else
                                    {
                                        Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(stafftotalcount);
                                    }
                                }
                                if (rdopercentage.Checked == true)
                                {
                                    totalper = hostelstudentPresentcount + daystudentPresentcount + staffPresentcount / (hostelstudentTotalcount + daystudenttotalcount + stafftotalcount) * 100;
                                    grandpersent = Math.Round(totalper, 2);

                                    totalper = ((hostelstudentTotalcount - hostelstudentPresentcount) + (daystudenttotalcount - daystudentPresentcount) + (stafftotalcount - staffPresentcount)) / (hostelstudentTotalcount + daystudenttotalcount + stafftotalcount) * 100;
                                    grandabsent = Math.Round(totalper, 2);
                                    totalper = (hostelstudentTotalcount + daystudenttotalcount + stafftotalcount) / (hostelstudentTotalcount + daystudenttotalcount + stafftotalcount) * 100;
                                    grandtotal = Math.Round(totalper, 2);
                                }
                                else
                                {
                                    grandpersent = hostelstudentPresentcount + daystudentPresentcount + staffPresentcount;
                                    grandabsent = (hostelstudentTotalcount - hostelstudentPresentcount) + (daystudenttotalcount - daystudentPresentcount) + (stafftotalcount - staffPresentcount);
                                    grandtotal = hostelstudentTotalcount + daystudenttotalcount + stafftotalcount;
                                }
                                string grandpersentS = "";
                                if (Convert.ToString(grandpersent) != "NaN")
                                    grandpersentS = Convert.ToString(grandpersent);
                                else
                                    grandpersentS = " - ";

                                string grandabsentS = "";
                                if (Convert.ToString(grandabsent) != "NaN")
                                    grandabsentS = Convert.ToString(grandabsent);
                                else
                                    grandabsentS = " - ";

                                string grandtotalS = "";
                                if (Convert.ToString(grandtotal) != "NaN")
                                    grandtotalS = Convert.ToString(grandtotal);
                                else
                                    grandtotalS = " - ";

                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(grandpersentS);
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 13].Text = Convert.ToString(grandabsentS);
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 14].Text = Convert.ToString(grandtotalS);
                                val = 0;
                                for (int t = 3; t < Fpcumulative.Sheets[0].ColumnHeader.Columns.Count; t++)
                                {
                                    string tagvalue = Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[1, t].Tag);
                                    string s = Convert.ToString(Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, t].Text);
                                    double.TryParse(Convert.ToString(Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, t].Text), out val);
                                    if (totalvalue_dic.Contains(tagvalue))
                                    {
                                        value = "";
                                        value = totalvalue_dic[tagvalue].ToString();
                                        totalvalue_dic.Remove(tagvalue);
                                        total = 0;
                                        total = Convert.ToDouble(value) + val;
                                        totalvalue_dic.Add(tagvalue, total);
                                    }
                                    else
                                    {
                                        totalvalue_dic.Add(tagvalue, val);
                                    }
                                }
                                #endregion

                                #region alignment

                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 3].Font.Underline = true;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 4].Font.Underline = true;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 6].Font.Underline = true;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 7].Font.Underline = true;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 9].Font.Underline = true;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 10].Font.Underline = true;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Center;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 12].Font.Size = FontUnit.Medium;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 12].Font.Name = "Book Antiqua";
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Center;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 12].Font.Underline = true;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 13].Font.Size = FontUnit.Medium;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 13].Font.Name = "Book Antiqua";
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 13].HorizontalAlign = HorizontalAlign.Center;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 13].Font.Underline = true;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 14].Font.Size = FontUnit.Medium;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 14].Font.Name = "Book Antiqua";
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 14].HorizontalAlign = HorizontalAlign.Center;
                                Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 14].Font.Underline = true;
                                #endregion

                                Fpcumulative.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                Fpcumulative.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

                            }
                        }
                        dt2 = dt2.AddDays(1);
                    }
                    #region Granttotal
                    Fpcumulative.Sheets[0].RowCount++;
                    Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 2].Text = "Grant Total";
                    Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    Fpcumulative.Sheets[0].Rows[Fpcumulative.Sheets[0].RowCount - 1].ForeColor = Color.Brown;
                    Fpcumulative.Sheets[0].Rows[Fpcumulative.Sheets[0].RowCount - 1].BackColor = Color.Bisque;
                    for (int t = 3; t < Fpcumulative.Sheets[0].ColumnHeader.Columns.Count; t++)
                    {
                        string tagvalue = Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[1, t].Tag);
                        if (totalvalue_dic.Count > 0)
                        {
                            value = "";
                            if (totalvalue_dic.Contains(tagvalue))
                            {
                                value = totalvalue_dic[tagvalue].ToString();
                            }
                            else
                            {
                                value = "0";
                            }
                            Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, t].Text = value;
                            Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, t].HorizontalAlign = HorizontalAlign.Center;
                            Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, t].Font.Size = FontUnit.Medium;
                            Fpcumulative.Sheets[0].Cells[Fpcumulative.Sheets[0].RowCount - 1, t].Font.Name = "Book Antiqua";
                        }
                    }
                    #endregion

                    Fpcumulative.SaveChanges();
                    rptprint.Visible = true;
                    Fpcumulative.Visible = true;
                    Fpcumulative.Sheets[0].PageSize = Fpcumulative.Sheets[0].RowCount;
                }
                else
                {
                    if (cbl_messname.Items.Count == 0)
                        lbl_error.Text = "Please Select Mess Name";
                    else
                        lbl_error.Text = "Please Select Session Name";
                    lbl_error.Visible = true;
                    lbl_error.ForeColor = Color.Red;
                }
            }
            else
            {
                if (cbl_messname.Items.Count == 0)
                    lbl_error.Text = "Please Create Mess Name";
                else
                    lbl_error.Text = "Please Create Session Name";
                lbl_error.Visible = true;
                lbl_error.ForeColor = Color.Red;
            }
        }
        catch (Exception ex)
        {
            lbl_error.Text = ex.ToString();
            lbl_error.Visible = true;
            lbl_error.ForeColor = Color.Red;
            d2.sendErrorMail(ex, college_code, "Mess Attendance Report");
        }
    }
    protected void Fpcumulative_SelectedIndexChanged(object sender, EventArgs e)
    {
        show_details();
    }
    protected void Fpcumulative_CellClick(object sender, EventArgs e)
    {
        cellclick = true;
    }
    public void show_details()
    {
        try
        {
            if (cellclick == true)
            {
                Printcontrol1.Visible = false;
                string activerow = Fpcumulative.ActiveSheetView.ActiveRow.ToString();
                string activecol = Fpcumulative.ActiveSheetView.ActiveColumn.ToString();
                string messFk = rs.GetSelectedItemsValueAsString(cbl_messname);
                int actrow = Convert.ToInt32(activerow);
                int actcol = Convert.ToInt32(activecol);
                if (Convert.ToString(activerow) != "-1")
                {
                    FpSpread1.Sheets[0].RowCount = 0;
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    FpSpread1.CommandBar.Visible = false;
                    FpSpread1.Sheets[0].AutoPostBack = true;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread1.Sheets[0].RowHeader.Visible = false;
                    FpSpread1.Sheets[0].ColumnCount = 7;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    //////////set column///////////
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.NO";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Student Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Semester";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Hostel";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Floor Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Room No";
                    FpSpread1.Sheets[0].ColumnHeader.Height = 30;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    //////////set column//////////
                    FpSpread1.ActiveSheetView.Columns[0].Font.Size = FontUnit.Medium;
                    FpSpread1.ActiveSheetView.Columns[0].Font.Name = "Book Antiqua";
                    FpSpread1.ActiveSheetView.Columns[1].Font.Size = FontUnit.Medium;
                    FpSpread1.ActiveSheetView.Columns[1].Font.Name = "Book Antiqua";
                    FpSpread1.ActiveSheetView.Columns[2].Font.Size = FontUnit.Medium;
                    FpSpread1.ActiveSheetView.Columns[2].Font.Name = "Book Antiqua";
                    FpSpread1.ActiveSheetView.Columns[3].Font.Size = FontUnit.Medium;
                    FpSpread1.ActiveSheetView.Columns[3].Font.Name = "Book Antiqua";
                    FpSpread1.ActiveSheetView.Columns[4].Font.Size = FontUnit.Medium;
                    FpSpread1.ActiveSheetView.Columns[4].Font.Name = "Book Antiqua";
                    FpSpread1.ActiveSheetView.Columns[5].Font.Size = FontUnit.Medium;
                    FpSpread1.ActiveSheetView.Columns[5].Font.Name = "Book Antiqua";
                    FpSpread1.ActiveSheetView.Columns[5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.ActiveSheetView.Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.ActiveSheetView.Columns[6].HorizontalAlign = HorizontalAlign.Center;
                    string date5 = Convert.ToString(Fpcumulative.Sheets[0].Cells[actrow, 1].Text);
                    string date = Convert.ToString(Fpcumulative.Sheets[0].Cells[actrow, 1].Tag);
                    string sessionFk = Convert.ToString(Fpcumulative.Sheets[0].Cells[actrow, 2].Tag);
                    string condition = "";
                    if (Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[0, actcol].Tag).Trim() == "Hostler")
                    {
                        if (Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[1, actcol].Tag).Trim() == "HP")
                        {
                            condition = " ";
                        }
                        if (Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[1, actcol].Tag).Trim() == "HA")
                        {
                            condition = " not";
                        }
                        q1 = " SELECT r.roll_no,Course_Name+'-'+Dept_Name as Degree,r.stud_name,r.current_semester,h.hostelname,fm.Floor_Name,rd.room_name FROM HT_HostelRegistration hsd,registration r,Degree G, course e,department d ,HM_HostelMaster h ,room_detail rd,Floor_Master fm  where  hsd.APP_No=r.App_No and r.degree_code = G.degree_code  and g.dept_code=d.dept_code and  G.Course_ID = e.Course_ID and h.HostelMasterPK=hsd.HostelMasterFK and rd.Roompk=hsd.RoomFK and fm.Floorpk=hsd.FloorFK and fm.Floor_Name=rd.Floor_Name and isnull(hsd.IsVacated,0)=0 and ISNULL(hsd.IsSuspend,0)=0 ";
                        if (txt_messname.Text.Trim() != "--Select--")
                        {
                            q1 += " and h.MessMasterFK in('" + messFk + "') ";
                        }
                        q1 += " and r.app_no " + condition + " in (select app_no from HostelMess_Attendance where Entry_Date = '" + date + "'";
                        if (txt_messname.Text.Trim() != "--Select--")
                        {
                            q1 += " and hostel_code IN ('" + messFk + "')";
                        }
                        q1 += " and session_code in('" + sessionFk + "'))";

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Student Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Semester";

                        FpSpread1.Sheets[0].Columns[4].Visible = true;
                        FpSpread1.Sheets[0].Columns[5].Visible = true;
                        FpSpread1.Sheets[0].Columns[6].Visible = true;
                    }
                    if (Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[0, actcol].Tag).Trim() == "Day Scholar")
                    {
                        if (Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[1, actcol].Tag).Trim() == "DP")
                        {
                            condition = " ";
                        }
                        if (Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[1, actcol].Tag).Trim() == "DA")
                        {
                            condition = " not";
                        }
                        q1 = " SELECT r.roll_no,Course_Name+'-'+Dept_Name as Degree,r.stud_name,r.current_semester,'' hostelname,'' Floor_Name,'' room_name FROM registration r,Degree G, course e,department d where  r.degree_code = G.degree_code  and g.dept_code=d.dept_code and  G.Course_ID = e.Course_ID  and r.Stud_Type='Day Scholar' and  r.app_no " + condition + " in (select app_no from HostelMess_Attendance where Entry_Date = '" + date + "' and is_staff='0'";
                        if (txt_messname.Text.Trim() != "--Select--")
                        {
                            q1 += " and hostel_code IN ('" + messFk + "')";
                        }
                        q1 += " and session_code in('" + sessionFk + "'))";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Student Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Semester";
                        FpSpread1.Sheets[0].Columns[6].Visible = false;
                        FpSpread1.Sheets[0].Columns[4].Visible = false;
                        FpSpread1.Sheets[0].Columns[5].Visible = false;
                    }
                    if (Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[0, actcol].Tag).Trim() == "Staff")
                    {
                        if (Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[1, actcol].Tag).Trim() == "SP")
                        {
                            condition = " ";
                        }
                        if (Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[1, actcol].Tag).Trim() == "SA")
                        {
                            condition = " not";
                        }
                        q1 = " select s.staff_name as stud_name,sa.desig_name as degree,sa.staff_type as current_semester, '' hostelname,'' Floor_Name,'' room_name from staffmaster s,staff_appl_master sa where s.appl_no=sa.appl_no and settled=0 and resign =0 and sa.appl_id  " + condition + " in (select app_no from HostelMess_Attendance where  Entry_Date = '" + date + "' and Is_Staff='1' ";

                        if (txt_messname.Text.Trim() != "--Select--")
                        {
                            q1 += " and hostel_code IN ('" + messFk + "')";
                        }
                        q1 += " and session_code in('" + sessionFk + "'))";
                        FpSpread1.Sheets[0].Columns[6].Visible = false;
                        FpSpread1.Sheets[0].Columns[4].Visible = false;
                        FpSpread1.Sheets[0].Columns[5].Visible = false;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Designation";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Type";
                    }
                    if (Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[0, actcol].Tag).Trim() == "OGT")
                    {
                        if (Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[1, actcol].Tag).Trim() == "GP")
                        {
                            condition = " ";
                        }
                        if (Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[1, actcol].Tag).Trim() == "GA" || Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[1, actcol].Tag).Trim() == "GT")
                        {
                            condition = " not";
                        }
                        q1 = "SELECT r.roll_no,Course_Name+'-'+Dept_Name as Degree,r.stud_name,r.current_semester,'' hostelname,'' Floor_Name,'' room_name FROM registration r,HT_HostelRegistration ht,HM_HostelMaster hm,Degree G, course e,department d where r.App_No=ht.APP_No and hm.HostelMasterPK=ht.HostelMasterFK and r.degree_code = G.degree_code  and g.dept_code=d.dept_code and  G.Course_ID = e.Course_ID and RoomFK<>'' and hm.MessMasterFK<>'' and r.app_no " + condition + " in(select app_no from HostelMess_Attendance where Entry_Date = '" + date + "' ";

                        if (txt_messname.Text.Trim() != "--Select--")
                        {
                            q1 += " and hostel_code IN ('" + messFk + "')";
                        }
                        q1 += " and session_code in('" + sessionFk + "'))";

                        q1 += " SELECT r.roll_no,Course_Name+'-'+Dept_Name as Degree,r.stud_name,r.current_semester,'' hostelname,'' Floor_Name,'' room_name FROM registration r, Degree G, course e,department d where  r.degree_code = G.degree_code  and g.dept_code=d.dept_code and  G.Course_ID = e.Course_ID and Stud_Type='Day Scholar'  and r.app_no " + condition + " in(select app_no from HostelMess_Attendance where Entry_Date = '" + date + "' ";
                        if (txt_messname.Text.Trim() != "--Select--")
                        {
                            q1 += " and hostel_code IN ('" + messFk + "')";
                        }
                        q1 += " and session_code in('" + sessionFk + "'))";

                        q1 += " select s.staff_code,s.staff_name,sa.desig_name,sa.staff_type,'' hostelname,'' Floor_Name,'' room_name from staffmaster s,staff_appl_master sa where s.appl_no=sa.appl_no and settled=0 and resign =0 and sa.appl_id " + condition + " in (select app_no from HostelMess_Attendance where Entry_Date = '" + date + "' and Is_Staff='1' ";
                        if (txt_messname.Text.Trim() != "--Select--")
                        {
                            q1 += " and hostel_code IN ('" + messFk + "')";
                        }
                        q1 += " and session_code in('" + sessionFk + "'))";

                        FpSpread1.Sheets[0].Columns[6].Visible = false;
                        FpSpread1.Sheets[0].Columns[4].Visible = false;
                        FpSpread1.Sheets[0].Columns[5].Visible = false;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Details";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Semester";
                    }
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q1, "text");
                    if (ds.Tables.Count > 0)
                    {
                        int rowstr = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            rowstr++;
                            string studname = "";
                            string degree = "";
                            string sem = "";
                            string hostel_name = "";
                            string floor_name = "";
                            string room_no = "";
                            FpSpread1.Sheets[0].RowCount++;
                            studname = dr["stud_name"].ToString();
                            degree = dr["degree"].ToString();
                            sem = dr["current_semester"].ToString();
                            hostel_name = dr["hostelname"].ToString();
                            floor_name = dr["floor_name"].ToString();
                            room_no = dr["room_name"].ToString();
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 0].Text = rowstr.ToString();
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 1].Text = studname;
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 2].Text = degree;
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 3].Text = sem;
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 4].Text = hostel_name;
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 5].Text = floor_name;
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 6].Text = room_no;

                            #region alignment
                            //FpSpread1.Sheets[0].Cells[rowstr - 1, 0].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 0].Font.Size = FontUnit.Medium;
                            //FpSpread1.Sheets[0].Cells[rowstr - 1, 1].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 1].Font.Size = FontUnit.Medium;
                            //FpSpread1.Sheets[0].Cells[rowstr - 1, 2].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 2].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 2].Font.Size = FontUnit.Medium;
                            //FpSpread1.Sheets[0].Cells[rowstr - 1, 3].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 3].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 3].Font.Size = FontUnit.Medium;
                            //FpSpread1.Sheets[0].Cells[rowstr - 1, 4].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 4].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 4].Font.Size = FontUnit.Medium;
                            //FpSpread1.Sheets[0].Cells[rowstr - 1, 5].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 5].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 5].Font.Size = FontUnit.Medium;
                            //FpSpread1.Sheets[0].Cells[rowstr - 1, 6].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 6].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[rowstr - 1, 6].Font.Size = FontUnit.Medium;
                            #endregion
                        }
                        if (Convert.ToString(Fpcumulative.Sheets[0].ColumnHeader.Cells[0, actcol].Tag).Trim() == "OGT")
                        {

                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                rowstr++;
                                string studname = "";
                                string degree = "";
                                string sem = "";
                                string hostel_name = "";
                                string floor_name = "";
                                string room_no = "";
                                FpSpread1.Sheets[0].RowCount++;
                                studname = dr["stud_name"].ToString();
                                degree = dr["degree"].ToString();
                                sem = dr["current_semester"].ToString();
                                hostel_name = dr["hostelname"].ToString();
                                floor_name = dr["floor_name"].ToString();
                                room_no = dr["room_name"].ToString();

                                FpSpread1.Sheets[0].Cells[rowstr - 1, 0].Text = rowstr.ToString();
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 1].Text = studname;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 2].Text = degree;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 3].Text = sem;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 4].Text = hostel_name;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 5].Text = floor_name;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 6].Text = room_no;

                                #region alignment
                                //FpSpread1.Sheets[0].Cells[rowstr - 1, 0].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 0].Font.Size = FontUnit.Medium;
                                //FpSpread1.Sheets[0].Cells[rowstr - 1, 1].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 1].Font.Size = FontUnit.Medium;
                                // FpSpread1.Sheets[0].Cells[rowstr - 1, 2].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 2].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 2].Font.Size = FontUnit.Medium;
                                //FpSpread1.Sheets[0].Cells[rowstr - 1, 3].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 3].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 3].Font.Size = FontUnit.Medium;
                                // FpSpread1.Sheets[0].Cells[rowstr - 1, 4].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 4].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 4].Font.Size = FontUnit.Medium;
                                // FpSpread1.Sheets[0].Cells[rowstr - 1, 5].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 5].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 5].Font.Size = FontUnit.Medium;
                                //FpSpread1.Sheets[0].Cells[rowstr - 1, 6].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 6].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 6].Font.Size = FontUnit.Medium;
                                #endregion
                            }
                            int row = 0;
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                rowstr++;
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 0].Text = "";
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 1].Text = "Staff Name";
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 2].Text = "Designation";
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 3].Text = "Staff Type";
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 0].BackColor = Color.Bisque;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 1].BackColor = Color.Bisque;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 2].BackColor = Color.Bisque;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 3].BackColor = Color.Bisque;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 1].ForeColor = Color.Blue;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 2].ForeColor = Color.Blue;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 3].ForeColor = Color.Blue;

                                #region alignment
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 0].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 1].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 2].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 2].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 2].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 3].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 3].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 3].Font.Size = FontUnit.Medium;
                                #endregion
                            }
                            foreach (DataRow dr in ds.Tables[2].Rows)
                            {
                                rowstr++;
                                string studname = "";
                                string degree = "";
                                string sem = "";
                                string hostel_name = "";
                                string floor_name = "";
                                string room_no = "";
                                FpSpread1.Sheets[0].RowCount++;
                                studname = dr["staff_name"].ToString();
                                degree = dr["desig_name"].ToString();
                                sem = dr["staff_type"].ToString();
                                hostel_name = dr["hostelname"].ToString();
                                floor_name = dr["floor_name"].ToString();
                                room_no = dr["room_name"].ToString();
                                row = rowstr;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 0].Text = (row - 1).ToString();
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 1].Text = studname;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 2].Text = degree;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 3].Text = sem;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 4].Text = hostel_name;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 5].Text = floor_name;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 6].Text = room_no;

                                #region alignment
                                //FpSpread1.Sheets[0].Cells[rowstr - 1, 0].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 0].Font.Size = FontUnit.Medium;
                                //FpSpread1.Sheets[0].Cells[rowstr - 1, 1].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 1].Font.Size = FontUnit.Medium;
                                //FpSpread1.Sheets[0].Cells[rowstr - 1, 2].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 2].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 2].Font.Size = FontUnit.Medium;
                                // FpSpread1.Sheets[0].Cells[rowstr - 1, 3].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 3].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 3].Font.Size = FontUnit.Medium;
                                //FpSpread1.Sheets[0].Cells[rowstr - 1, 4].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 4].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 4].Font.Size = FontUnit.Medium;
                                //FpSpread1.Sheets[0].Cells[rowstr - 1, 5].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 5].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 5].Font.Size = FontUnit.Medium;
                                //FpSpread1.Sheets[0].Cells[rowstr - 1, 6].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 6].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[rowstr - 1, 6].Font.Size = FontUnit.Medium;
                                #endregion
                            }
                        }
                    }
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    if (FpSpread1.Sheets[0].RowCount > 0)
                    {
                        FpSpread1.Visible = true;
                        lbl_error1.Visible = false;
                        rptprint1.Visible = true;
                    }
                    else
                    {
                        rptprint1.Visible = false;
                        FpSpread1.Visible = false;
                        lbl_error1.Visible = true;
                        lbl_error1.Text = " No Records Founds";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, college_code, "Mess Attendance Report");
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpcumulative, reportname);
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
            string degreedetails = "Cumulative Mess Attendance Report";
            string pagename = "Messattendance_report1.aspx";
            Printcontrol.loadspreaddetails(Fpcumulative, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        { }
    }
    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = TextBox1.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                TextBox1.Focus();
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
            string degreedetails = "Cumulative Mess Attendance Report";
            string pagename = "Messattendance_report1.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        { }
    }
}