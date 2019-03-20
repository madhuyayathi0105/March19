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

public partial class HT_MenuSchedule : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    bool check = false;
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
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        lblvalidation1.Text = "";
        calfromdate.EndDate = DateTime.Now;
        caltodate.EndDate = DateTime.Now;
        if (!IsPostBack)
        {
            // loadhostel();
            bindmessname();
            itemheader();
            loadsubheadername();
            itemmaster();
            txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtfromdate.Attributes.Add("readonly", "readonly");
            txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txttodate.Attributes.Add("readonly", "readonly");
            rdodatewise.Checked = true;
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread1.Visible = false;
            btngo_click(sender, e);
            // image1.ImageUrl = "~/Handler/Handler6.ashx";
            rdbMenu.Checked = true;
            Session["dt"] = null;
            Session["activerow"] = null;
            Session["activecoloumn"] = null;
            cb_menutype.Checked = true;
            for (int i = 0; i < cbl_menutype.Items.Count; i++)
            {
                cbl_menutype.Items[i].Selected = true;
            }
            cbl_menutype_SelectIndexChange(sender, e);
        }
        errorlable.Visible = false;
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertmessage.Visible = false;
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
    }
    protected void imagebtnpopclose3_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch
        {

        }
    }
    protected void rdodaywise_CheckedChanged(object sender, EventArgs e)
    {
        lblfromdate.Enabled = false;
        txtfromdate.Enabled = false;
        lbltodate.Enabled = false;
        txttodate.Enabled = false;
        div1.Visible = false;
        rptprint.Visible = false;

    }

    protected void rdodatewise_CheckedChanged(object sender, EventArgs e)
    {
        lblfromdate.Enabled = true;
        txtfromdate.Enabled = true;
        lbltodate.Enabled = true;
        txttodate.Enabled = true;
        div1.Visible = false;
        rptprint.Visible = false;
    }

    protected void btngo_click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            ArrayList list = new ArrayList();
            list.Add("Monday");
            list.Add("Tuesday");
            list.Add("Wednesday");
            list.Add("Thursday");
            list.Add("Friday");
            list.Add("Saturday");
            list.Add("Sunday");

            string itemheadercode = "";
            itemheadercode = ddl_messname.SelectedItem.Value.ToString();

            DataView dv = new DataView();
            DataView dv1 = new DataView();

            if (rdbMenu.Checked == true)
            {
                if (itemheadercode.Trim() != "")
                {
                    int ro = 0;
                    if (rdodaywise.Checked == true)
                    {
                        string firstdate1 = Convert.ToString(txtfromdate.Text);
                        string seconddate1 = Convert.ToString(txttodate.Text);
                        DateTime dtt = new DateTime();
                        DateTime dtt1 = new DateTime();
                        string[] splitt = firstdate1.Split('/');
                        dtt = Convert.ToDateTime(splitt[1] + "/" + splitt[0] + "/" + splitt[2]);
                        string[] split1 = seconddate1.Split('/');
                        dtt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
                        string selectquery = "";
                        selectquery = "select distinct SessionMasterPK ,SessionName  from HM_SessionMaster where MessMasterFK in ('" + itemheadercode + "') order by SessionMasterPK  ";

                        selectquery = selectquery + " select distinct MenuMasterFK, MenuCode,MenuName,menuscheduleday,SessionMasterFK from  HT_MenuSchedule MS,HM_MenuMaster MM where MenuMasterPK=MenuMasterFK and MessMasterFK  in ('" + itemheadercode + "') and ScheduleType='2' and ScheudleItemType='1' ";

                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Fpspread1.Sheets[0].RowCount = 0;
                            Fpspread1.Sheets[0].ColumnCount = 0;
                            Fpspread1.CommandBar.Visible = false;
                            Fpspread1.Sheets[0].AutoPostBack = true;
                            Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                            Fpspread1.Sheets[0].RowHeader.Visible = false;
                            Fpspread1.Sheets[0].ColumnCount = 2;
                            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            darkstyle.ForeColor = Color.White;
                            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[0].Width = 50;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Session / Day";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[1].Width = 200;
                            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                            {
                                Fpspread1.Sheets[0].ColumnCount++;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["SessionName"]);
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]);
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                            }
                            if (list.Count > 0)
                            {
                                for (int jk = 0; jk < list.Count; jk++)
                                {
                                    ro++;
                                    Fpspread1.Sheets[0].RowCount++;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ro);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(list[jk]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                    int col = 1;

                                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                                    {
                                        col++;
                                        if (ds.Tables[1].Rows.Count > 0)
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = " SessionMasterFk='" + Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPk"]) + "' and menuscheduleday='" + Convert.ToString(list[jk]) + "'";
                                            dv1 = ds.Tables[1].DefaultView;
                                            if (dv1.Count > 0)
                                            {
                                                string itemname = "";
                                                string itemcode = "";
                                                for (int i = 0; i < dv1.Count; i++)
                                                {
                                                    string itempk = Convert.ToString(dv1[i]["MenuMasterFK"]);
                                                    string itname = Convert.ToString(dv1[i]["menuname"]);

                                                    if (itemname == "")
                                                    {
                                                        itemname = Convert.ToString(itname);
                                                    }
                                                    else
                                                    {
                                                        itemname = itemname + "," + Convert.ToString(itname);
                                                    }
                                                    if (itemcode == "")
                                                    {
                                                        itemcode = Convert.ToString(itempk);
                                                    }
                                                    else
                                                    {
                                                        itemcode = itemcode + "," + Convert.ToString(itempk);
                                                    }
                                                }
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(itemname);
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Tag = Convert.ToString(itemcode);
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                            }
                                        }
                                    }
                                }
                                Fpspread1.Visible = true;
                                rptprint.Visible = true;
                                div1.Visible = true;
                                errorlable.Visible = false;
                                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                            }
                        }
                        else
                        {
                            Fpspread1.Visible = false;
                            rptprint.Visible = false;
                            div1.Visible = false;
                            errorlable.Visible = true;
                            errorlable.Text = "Please Create Session Name";
                        }
                    }

                    if (rdodatewise.Checked == true)
                    {
                        string firstdate1 = Convert.ToString(txtfromdate.Text);
                        string seconddate1 = Convert.ToString(txttodate.Text);
                        DateTime dtt = new DateTime();
                        DateTime dtt1 = new DateTime();
                        string[] splitt = firstdate1.Split('/');
                        dtt = Convert.ToDateTime(splitt[1] + "/" + splitt[0] + "/" + splitt[2]);
                        string[] split1 = seconddate1.Split('/');
                        dtt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
                        string selectquery = "select distinct SessionMasterPK ,SessionName  from HM_SessionMaster where MessMasterFK in ('" + itemheadercode + "') order by SessionMasterPK  ";
                        selectquery = selectquery + " select MenuMasterFK,MenuName,MenuScheduleDate,SessionMasterFK from  HT_MenuSchedule MS,HM_MenuMaster MM where MenuMasterPK=MenuMasterFK and MessMasterFK  in ('" + itemheadercode + "') and ScheduleType='1' and ScheudleItemType='1' and MenuScheduleDate between '" + dtt.ToString("MM/dd/yyyy") + "' and '" + dtt1.ToString("MM/dd/yyyy") + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Fpspread1.Sheets[0].RowCount = 0;
                            Fpspread1.Sheets[0].ColumnCount = 0;
                            Fpspread1.CommandBar.Visible = false;
                            Fpspread1.Sheets[0].AutoPostBack = true;
                            Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                            Fpspread1.Sheets[0].RowHeader.Visible = false;
                            Fpspread1.Sheets[0].ColumnCount = 2;
                            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            darkstyle.ForeColor = Color.White;
                            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[0].Width = 50;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Session / Day";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[1].Width = 200;
                            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                            {
                                Fpspread1.Sheets[0].ColumnCount++;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["SessionName"]);
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]);
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                            string firstdate = Convert.ToString(txtfromdate.Text);
                            string seconddate = Convert.ToString(txttodate.Text);
                            DateTime dt = new DateTime();
                            DateTime dt1 = new DateTime();
                            string[] split = firstdate.Split('/');
                            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                            split = seconddate.Split('/');
                            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                            while (dt <= dt1)
                            {
                                ro++;
                                Fpspread1.Sheets[0].RowCount++;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ro);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dt.ToString("dd/MM/yyyy")) + " - " + Convert.ToString(dt.ToString("dddd"));
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                int col = 1;

                                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                                {
                                    col++;
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        ds.Tables[1].DefaultView.RowFilter = " SessionMasterFk='" + Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPk"]) + "' and MenuScheduleDate='" + Convert.ToString(dt.ToString("MM/dd/yyyy")) + "'";
                                        dv1 = ds.Tables[1].DefaultView;
                                        if (dv1.Count > 0)
                                        {
                                            string buildvalue = "";
                                            for (int r = 0; r < dv1.Count; r++)
                                            {
                                                if (buildvalue == "")
                                                {
                                                    buildvalue = Convert.ToString(dv1[r]["MenuName"]);
                                                }
                                                else
                                                {
                                                    buildvalue = buildvalue + "," + Convert.ToString(dv1[r]["MenuName"]);
                                                }
                                            }
                                            string mcode = "";
                                            for (int r = 0; r < dv1.Count; r++)
                                            {
                                                if (mcode == "")
                                                {
                                                    mcode = Convert.ToString(dv1[r]["MenuMasterFK"]);
                                                }
                                                else
                                                {
                                                    mcode = mcode + "," + Convert.ToString(dv1[r]["MenuMasterFK"]);
                                                }
                                            }

                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(buildvalue);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Tag = Convert.ToString(mcode);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                        }
                                    }

                                }
                                dt = dt.AddDays(1);
                            }
                            Fpspread1.Visible = true;
                            rptprint.Visible = true;
                            div1.Visible = true;
                            errorlable.Visible = false;
                            Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                        }
                        else
                        {
                            div1.Visible = false;
                            Fpspread1.Visible = false;
                            rptprint.Visible = false;
                            errorlable.Visible = true;
                            //errorlable.Text = "No Records Found";
                            errorlable.Text = "Please Create Session Name";
                        }

                    }
                }
                else
                {
                    div1.Visible = false;
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                    errorlable.Visible = true;
                    errorlable.Text = "Please Select Mess Name";
                }
            }
            if (rdbcleaning.Checked == true)
            {
                gobtn_Click(sender, e);

                if (itemheadercode.Trim() != "")
                {
                    int ro = 0;
                    if (rdodaywise.Checked == true)
                    {
                        string selectquery = "select distinct SessionMasterPK ,SessionName  from HM_SessionMaster where MessMasterFK in ('" + itemheadercode + "') order by SessionMasterPK  ";

                        selectquery = selectquery + "  select itemfk,itemname,Schedule_Day,sessionfk from Cleaning_ItemDetailMaster md,Cleaning_ItemMaseter cm, IM_ItemMaster i,HT_MenuSchedule h where md.clean_itemmasterfk=cm.clean_itemmasterpk and i.itempk=md.itemfk and  cm.messmasterfk in('" + ddl_messname.SelectedItem.Value + "') and h.scheudleitemtype='2' and h.ScheduleType='2' and h.MenuMasterFK=md.itemfk and i.ItemPK =h.MenuMasterFK  and cm.Schedule_type=h.ScheduleType and h.SessionMasterFK=cm.SessionFK and cm.MessMasterFK =h.MessMasterFK and h.MenuScheduleday =cm.Schedule_Day  ";

                        //selectquery = selectquery + " select itemfk,itemname,Schedule_Day,sessionfk from Cleaning_ItemDetailMaster md,Cleaning_ItemMaseter cm, IM_ItemMaster i,HT_MenuSchedule h where md.clean_itemmasterfk=cm.clean_itemmasterpk and i.itempk=md.itemfk and  cm.messmasterfk in('" + ddl_messname.SelectedItem.Value + "') and h.scheudleitemtype='2' and h.ScheduleType='2' and h.MenuMasterFK=md.itemfk  and cm.Schedule_type=h.ScheduleType";

                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Fpspread1.Sheets[0].RowCount = 0;
                            Fpspread1.Sheets[0].ColumnCount = 0;
                            Fpspread1.CommandBar.Visible = false;
                            Fpspread1.Sheets[0].AutoPostBack = true;
                            Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                            Fpspread1.Sheets[0].RowHeader.Visible = false;
                            Fpspread1.Sheets[0].ColumnCount = 2;
                            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            darkstyle.ForeColor = Color.White;
                            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[0].Width = 50;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Session / Day";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[1].Width = 200;
                            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                            {
                                Fpspread1.Sheets[0].ColumnCount++;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["SessionName"]);
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]);
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (list.Count > 0)
                            {
                                for (int jk = 0; jk < list.Count; jk++)
                                {
                                    ro++;
                                    Fpspread1.Sheets[0].RowCount++;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ro);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(list[jk]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                    int col = 1;
                                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                                    {
                                        col++;

                                        if (ds.Tables[1].Rows.Count > 0)
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = "Schedule_Day ='" + Convert.ToString(list[jk]) + "' and Sessionfk ='" + Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]) + "'";

                                            dv = ds.Tables[1].DefaultView;
                                            string itemname = "";
                                            string itemcode = "";
                                            for (int i = 0; i < dv.Count; i++)
                                            {
                                                string itempk = Convert.ToString(dv[i]["itemfk"]);
                                                string itname = Convert.ToString(dv[i]["itemname"]);

                                                if (itemname == "")
                                                {
                                                    itemname = Convert.ToString(itname);
                                                }
                                                else
                                                {
                                                    itemname = itemname + "," + Convert.ToString(itname);
                                                }
                                                if (itemcode == "")
                                                {
                                                    itemcode = Convert.ToString(itempk);
                                                }
                                                else
                                                {
                                                    itemcode = itemcode + "," + Convert.ToString(itempk);
                                                }
                                            }
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Text = itemname;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Tag = itemcode;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                        }

                                    }
                                }
                                Fpspread1.Visible = true;
                                rptprint.Visible = true;
                                div1.Visible = true;
                                errorlable.Visible = false;
                                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                            }

                        }
                        else
                        {
                            div1.Visible = false;
                            Fpspread1.Visible = false;
                            rptprint.Visible = false;
                            errorlable.Visible = true;
                            errorlable.Text = "Please Create Session Name";
                        }
                    }
                    if (rdodatewise.Checked == true)
                    {
                        string firstdate1 = Convert.ToString(txtfromdate.Text);
                        string seconddate1 = Convert.ToString(txttodate.Text);
                        DateTime dtt = new DateTime();
                        DateTime dtt1 = new DateTime();
                        string[] split = firstdate1.Split('/');
                        dtt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                        split = seconddate1.Split('/');
                        dtt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                        string selectquery = "select distinct SessionMasterPK ,SessionName  from HM_SessionMaster where MessMasterFK in ('" + itemheadercode + "') order by SessionMasterPK  ";

                        selectquery = selectquery + "   select itemfk,itemname,Schedule_Day,sessionfk from Cleaning_ItemDetailMaster md,Cleaning_ItemMaseter cm, IM_ItemMaster i,HT_MenuSchedule h where md.clean_itemmasterfk=cm.clean_itemmasterpk and i.itempk=md.itemfk and  cm.messmasterfk in('" + ddl_messname.SelectedItem.Value + "') and h.scheudleitemtype='2' and h.ScheduleType='2' and h.MenuMasterFK=md.itemfk and i.ItemPK =h.MenuMasterFK  and cm.Schedule_type=h.ScheduleType and h.SessionMasterFK=cm.SessionFK and cm.MessMasterFK =h.MessMasterFK and h.MenuScheduleDate =cm.Schedule_Date and h.MenuScheduleDate between '" + dtt.ToString("MM/dd/yyyy") + "' and '" + dtt1.ToString("MM/dd/yyyy") + "'";

                        //selectquery = selectquery + " select itemfk,itemname,Schedule_Date,SessionMasterFK from Cleaning_ItemDetailMaster md,Cleaning_ItemMaseter cm, IM_ItemMaster i,HT_MenuSchedule h where md.clean_itemmasterfk=cm.clean_itemmasterpk and i.itempk=md.itemfk and  cm.messmasterfk in('" + ddl_messname.SelectedItem.Value + "') and h.scheduletype='1' and ScheudleItemType='2' and md.itemfk=h.MenuMasterFK and h.MenuScheduleDate between '" + dtt.ToString("MM/dd/yyyy") + "' and '" + dtt1.ToString("MM/dd/yyyy") + "' and cm.Schedule_type=h.ScheduleType ";

                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Fpspread1.Sheets[0].RowCount = 0;
                            Fpspread1.Sheets[0].ColumnCount = 0;
                            Fpspread1.CommandBar.Visible = false;
                            Fpspread1.Sheets[0].AutoPostBack = true;
                            Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                            Fpspread1.Sheets[0].RowHeader.Visible = false;
                            Fpspread1.Sheets[0].ColumnCount = 2;
                            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            darkstyle.ForeColor = Color.White;
                            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[0].Width = 50;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Session / Day";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[1].Width = 200;
                            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                            {
                                Fpspread1.Sheets[0].ColumnCount++;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["SessionName"]);
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]);
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                            string firstdate = Convert.ToString(txtfromdate.Text);
                            string seconddate = Convert.ToString(txttodate.Text);
                            DateTime dt = new DateTime();
                            DateTime dt1 = new DateTime();
                            string[] split1 = firstdate.Split('/');
                            dt = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
                            split = seconddate.Split('/');
                            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                            FarPoint.Web.Spread.TextCellType txt1 = new FarPoint.Web.Spread.TextCellType();

                            while (dt <= dt1)
                            {
                                ro++;
                                Fpspread1.Sheets[0].RowCount++;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ro);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dt.ToString("dd/MM/yyyy")) + " - " + Convert.ToString(dt.ToString("dddd"));
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                int col = 1;
                                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                                {
                                    col++;
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        ds.Tables[1].DefaultView.RowFilter = "Schedule_Date ='" + Convert.ToString(dt.ToString("MM/dd/yyyy")) + "' and SessionMasterFK ='" + Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]) + "'";
                                        dv = ds.Tables[1].DefaultView;
                                        string itemname = ""; string itemcode = "";
                                        if (dv.Count > 0)
                                        {
                                            for (int i = 0; i < dv.Count; i++)
                                            {
                                                string itempk = Convert.ToString(dv[i]["itemfk"]);
                                                string itname = Convert.ToString(dv[i]["itemname"]);

                                                if (itemname == "")
                                                {
                                                    itemname = Convert.ToString(itname);
                                                }
                                                else
                                                {
                                                    itemname = itemname + "," + Convert.ToString(itname);
                                                }
                                                if (itemcode == "")
                                                {
                                                    itemcode = Convert.ToString(itempk);
                                                }
                                                else
                                                {
                                                    itemcode = itemcode + "," + Convert.ToString(itempk);
                                                }
                                            }
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Text = itemname;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Tag = itemcode;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                        }
                                    }
                                }
                                dt = dt.AddDays(1);
                            }
                            Fpspread1.Visible = true;
                            rptprint.Visible = true;
                            div1.Visible = true;
                            errorlable.Visible = false;
                            Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                        }
                        else
                        {
                            div1.Visible = false;
                            Fpspread1.Visible = false;
                            rptprint.Visible = false;
                            errorlable.Visible = true;
                            errorlable.Text = "Please Create Session Name";
                        }
                    }

                }
                else
                {
                    div1.Visible = false;
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                    errorlable.Visible = true;
                    errorlable.Text = "Please Select Mess Name";
                }
            }

        }
        catch
        {

        }
    }

    protected void btnconexist_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Menu Schedule Report";
            string pagename = "Menu_schedule.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }

    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
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
    protected void Cell_Click(object sender, EventArgs e)
    {
        try
        {
            check = true;
        }
        catch
        {

        }
    }
    /// <summary>
    /// new code 21/08/15
    /// </summary>



    protected void chkitemheader(object sender, EventArgs e)
    {
        int cout = 0;
        txtpop2itemheader.Text = "--Select--";
        if (chk_pop2itemheader.Checked == true)
        {
            cout++;
            for (int i = 0; i < chklst_pop2itemheader.Items.Count; i++)
            {
                chklst_pop2itemheader.Items[i].Selected = true;
            }
            txtpop2itemheader.Text = "Item Header(" + (chklst_pop2itemheader.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chklst_pop2itemheader.Items.Count; i++)
            {
                chklst_pop2itemheader.Items[i].Selected = false;
            }
        }
        loadsubheadername();
        itemmaster();
    }

    protected void chklstitemheader(object sender, EventArgs e)
    {
        int i = 0;
        // chk_pophostelname.Checked = false;
        int commcount = 0;
        txtpop2itemheader.Text = "--Select--";
        for (i = 0; i < chklst_pop2itemheader.Items.Count; i++)
        {
            if (chklst_pop2itemheader.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                chk_pop2itemheader.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == chklst_pop2itemheader.Items.Count)
            {
                chk_pop2itemheader.Checked = true;
            }
            txtpop2itemheader.Text = "Item Header(" + commcount.ToString() + ")";
        }
        loadsubheadername();
        itemmaster();
    }

    protected void chklstitemtyp(object sender, EventArgs e)
    {
        int i = 0;
        chk_pop2itemtyp.Checked = false;
        int commcount = 0;
        txtpop2itemname.Text = "--Select--";
        for (i = 0; i < chklst_pop2itemtyp.Items.Count; i++)
        {
            if (chklst_pop2itemtyp.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                //  chk_pophostelname.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == chklst_pop2itemtyp.Items.Count)
            {
                chk_pop2itemtyp.Checked = true;
            }
            txtpop2itemname.Text = "Item Name(" + commcount.ToString() + ")";
        }
    }

    protected void chkitemtyp(object sender, EventArgs e)
    {
        int cout = 0;
        txtpop2itemname.Text = "---Select---";
        if (chk_pop2itemtyp.Checked == true)
        {
            cout++;
            for (int i = 0; i < chklst_pop2itemtyp.Items.Count; i++)
            {
                chklst_pop2itemtyp.Items[i].Selected = true;
            }
            txtpop2itemname.Text = "Item Name(" + (chklst_pop2itemtyp.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chklst_pop2itemtyp.Items.Count; i++)
            {
                chklst_pop2itemtyp.Items[i].Selected = false;
            }
        }
    }

    public void itemheader()
    {
        try
        {
            chklst_pop2itemheader.Items.Clear();

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
                columnfield = " and usercode='" + Session["usercode"] + "'";
            }
            string maninvalue = "";
            string selectnewquery = d2.GetFunction("select value from Master_Settings where settings='ItemHeaderRights' " + columnfield + "");
            if (selectnewquery.Trim() != "" && selectnewquery.Trim() != "0")
            {
                string[] splitnew = selectnewquery.Split(',');
                if (splitnew.Length > 0)
                {
                    for (int row = 0; row <= splitnew.GetUpperBound(0); row++)
                    {
                        if (maninvalue == "")
                        {
                            maninvalue = Convert.ToString(splitnew[row]);
                        }
                        else
                        {
                            maninvalue = maninvalue + "'" + "," + "'" + Convert.ToString(splitnew[row]);
                        }
                    }
                }
            }
            string headerquery = "";
            if (maninvalue.Trim() != "")
            {
                headerquery = "select distinct ItemHeaderCode ,ItemHeaderName  from IM_ItemMaster where ItemHeaderCode in ('" + maninvalue + "')";
            }
            else
            {
                headerquery = "select distinct ItemHeaderCode ,ItemHeaderName  from IM_ItemMaster";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(headerquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklst_pop2itemheader.DataSource = ds;
                chklst_pop2itemheader.DataTextField = "ItemHeaderName";
                chklst_pop2itemheader.DataValueField = "ItemHeaderCode";
                chklst_pop2itemheader.DataBind();


                if (chklst_pop2itemheader.Items.Count > 0)
                {
                    for (int i = 0; i < chklst_pop2itemheader.Items.Count; i++)
                    {

                        chklst_pop2itemheader.Items[i].Selected = true;
                    }

                    txtpop2itemheader.Text = "Item Header(" + chklst_pop2itemheader.Items.Count + ")";
                }
            }
            else
            {
                txtpop2itemheader.Text = "--Select--";
            }
        }
        catch
        {
        }
    }

    public void itemmaster()
    {
        string itemheadercode = "";
        string subheader = "";
        for (int i = 0; i < chklst_pop2itemheader.Items.Count; i++)
        {
            if (chklst_pop2itemheader.Items[i].Selected == true)
            {
                if (itemheadercode == "")
                {
                    itemheadercode = "" + chklst_pop2itemheader.Items[i].Value.ToString() + "";
                }
                else
                {
                    itemheadercode = itemheadercode + "'" + "," + "'" + chklst_pop2itemheader.Items[i].Value.ToString() + "";
                }
            }
        }
        for (int i = 0; i < cbl_subheadername.Items.Count; i++)
        {
            if (cbl_subheadername.Items[i].Selected == true)
            {
                if (subheader == "")
                {
                    subheader = "" + cbl_subheadername.Items[i].Value.ToString() + "";
                }
                else
                {
                    subheader = subheader + "'" + "," + "" + "'" + cbl_subheadername.Items[i].Value.ToString() + "";
                }
            }
        }

        chklst_pop2itemtyp.Items.Clear();
        if (itemheadercode.Trim() != "")
        {
            string deptquery = "select distinct ItemCode ,ItemName  from IM_ItemMaster  where ItemHeaderCode in ('" + itemheadercode + "') and ForHostelItem ='0' and subheader_code in ('" + subheader + "') order by ItemCode ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklst_pop2itemtyp.DataSource = ds;
                chklst_pop2itemtyp.DataTextField = "ItemName";
                chklst_pop2itemtyp.DataValueField = "ItemCode";
                chklst_pop2itemtyp.DataBind();


                if (chklst_pop2itemtyp.Items.Count > 0)
                {
                    for (int i = 0; i < chklst_pop2itemtyp.Items.Count; i++)
                    {

                        chklst_pop2itemtyp.Items[i].Selected = true;
                    }

                    txtpop2itemname.Text = "Item Name(" + chklst_pop2itemtyp.Items.Count + ")";
                }
            }
            else
            {
                txtpop2itemname.Text = "--Select--";
            }
        }
        else
        {
            txtpop2itemname.Text = "--Select--";
        }
    }

    protected void Fpspread1_render(object sender, EventArgs e)
    {
        try
        {
            if (check == true)
            {
                Session["dt"] = null;
                if (rdbMenu.Checked == true)
                {
                    string activerow = "";
                    string activecol = "";
                    activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                    activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                    collegecode = Session["collegecode"].ToString();
                    Session["activerow"] = Convert.ToString(activerow);
                    Session["activecoloumn"] = Convert.ToString(activecol);
                    if (activecol.Trim() != "0" && activecol.Trim() != "1")
                    {
                        menu();
                        poperrjs.Visible = true;
                    }
                }
                else if (rdbcleaning.Checked == true)
                {

                    string activerow = "";
                    string activecol = "";
                    activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                    activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                    collegecode = Session["collegecode"].ToString();
                    Session["activerow"] = Convert.ToString(activerow);
                    Session["activecoloumn"] = Convert.ToString(activecol);
                    if (activecol.Trim() != "0" && activecol.Trim() != "1")
                    {
                        popwindow1.Visible = true;
                        itemheader();
                        loadsubheadername();
                        itemmaster();
                        gobtn_Click(sender, e);
                        string sessioncode = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt32(activecol)].Tag);
                        Session["Sessioncode"] = Convert.ToString(sessioncode);
                    }

                }
            }
        }
        catch
        {

        }
    }

    protected void gobtn_Click(object sender, EventArgs e)
    {
        try
        {
            string itemheadercode = "";
            for (int i = 0; i < chklst_pop2itemheader.Items.Count; i++)
            {
                if (chklst_pop2itemheader.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + chklst_pop2itemheader.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + chklst_pop2itemheader.Items[i].Value.ToString() + "";
                    }
                }
            }
            string itemheadercode1 = "";
            for (int i = 0; i < chklst_pop2itemtyp.Items.Count; i++)
            {
                if (chklst_pop2itemtyp.Items[i].Selected == true)
                {
                    if (itemheadercode1 == "")
                    {
                        itemheadercode1 = "" + chklst_pop2itemtyp.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode1 = itemheadercode1 + "'" + "," + "'" + chklst_pop2itemtyp.Items[i].Value.ToString() + "";
                    }
                }
            }
            Session["dt"] = null;
            if (itemheadercode.Trim() != "" && itemheadercode1.Trim() != "")
            {
                string selectquery = "";
                if (searchitem.Text.Trim() != "")
                {
                    selectquery = "select distinct  itempk,ItemCode ,ItemName , ItemHeaderCode,ItemHeaderName,ItemUnit from IM_ItemMaster where ItemName='" + searchitem.Text + "' order by ItemCode ";
                }
                else
                {
                    selectquery = "select distinct  itempk,ItemCode ,ItemName , ItemHeaderCode,ItemHeaderName,ItemUnit from IM_ItemMaster where ItemHeaderCode in ('" + itemheadercode + "') and ItemCode in ('" + itemheadercode1 + "') order by ItemCode ";
                }

                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvdatass1.DataSource = ds.Tables[0];
                    gvdatass1.DataBind();
                    string row = Convert.ToString(Session["activerow"]);
                    if (row != "")
                    {
                        string itemcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), Convert.ToInt32(Session["activecoloumn"])].Tag);
                        string[] split = itemcode.Split(',');
                        foreach (string item in split)
                        {
                            foreach (DataListItem gvrow in gvdatass1.Items)
                            {
                                Label lblcode = (Label)gvrow.FindControl("lblitempk");
                                string menucode = lblcode.Text;
                                if (menucode.Trim() == item)
                                {
                                    CheckBox chkSelect = (gvrow.FindControl("CheckBox2") as CheckBox);
                                    chkSelect.Checked = true;
                                }
                            }
                        }
                    }
                    gvdatass1.Visible = true;
                    div2.Visible = true;
                }
                else
                {
                    alertmessage.Visible = true;
                    lbl_alerterror.Visible = true;
                    lbl_alerterror.Text = "Item Name Does Not Exist";
                }
            }
            else
            {
                alertmessage.Visible = true;
                lbl_alerterror.Visible = true;
                lbl_alerterror.Text = "Please Select Any Record";
            }
        }
        catch
        {

        }

    }

    public void menu()
    {
        try
        {
            collegecode = Session["collegecode"].ToString();
            string selecquery = ""; string mess1 = "";
            for (int i = 0; i < cbl_menutype.Items.Count; i++)
            {
                if (cbl_menutype.Items[i].Selected == true)
                {
                    if (mess1 == "")
                    {
                        mess1 = "" + cbl_menutype.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        mess1 = mess1 + "'" + "," + "'" + cbl_menutype.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (mess1.Trim() == "")
            {
                mess1 = "2";
            }
            if (menusearch.Text.Trim() != "")
            {
                selecquery = "select distinct MenuName,MenuCode,MenuMasterPK  from HM_MenuMaster where CollegeCode ='" + collegecode + "' and MenuName='" + menusearch.Text + "' and MenuType in('" + mess1 + "')";
            }
            else
            {
                selecquery = "select distinct MenuName,MenuCode,MenuMasterPK  from HM_MenuMaster where CollegeCode ='" + collegecode + "' and MenuType in('" + mess1 + "')";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selecquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvdatass.DataSource = ds.Tables[0];
                gvdatass.DataBind();
                string row = Convert.ToString(Session["activerow"]);
                if (row != "")
                {
                    string itemcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), Convert.ToInt32(Session["activecoloumn"])].Tag);
                    string[] split = itemcode.Split(',');
                    foreach (string item in split)
                    {
                        foreach (DataListItem gvrow in gvdatass.Items)
                        {
                            Label lblcode = (Label)gvrow.FindControl("lblmenuid");
                            string menucode = lblcode.Text;
                            if (menucode.Trim() == item)
                            {
                                CheckBox chkSelect = (gvrow.FindControl("chkup3") as CheckBox);
                                chkSelect.Checked = true;
                            }
                        }
                    }
                }
                gvdatass.Visible = true;
            }
            else
            {
                gvdatass.Visible = false;
                lbl_alerterror.Visible = true;
                lbl_alerterror.Text = "Select Valid Menu Type";
                alertmessage.Visible = true;
            }
        }
        catch
        {

        }
    }

    protected void menusearch_txtchange(object sender, EventArgs e)
    {
        menu();
    }
    protected void itemsearch_txtchange(object sender, EventArgs e)
    {
        btngo_click(sender, e);
    }
    protected void btnmenusave_click(object sender, EventArgs e)
    {
        try
        {
            string code = "";
            string name = "";
            foreach (DataListItem gvrow in gvdatass.Items)
            {

                CheckBox chkSelect = (gvrow.FindControl("chkup3") as CheckBox);
                if (chkSelect.Checked)
                {
                    Label lblname1 = (Label)gvrow.FindControl("lblMenuname");
                    string menuname = lblname1.Text;
                    Label lblcode = (Label)gvrow.FindControl("lblmenucode");
                    string menucode = lblcode.Text;
                    Label lblmenu = (Label)gvrow.FindControl("lblmenuid");
                    string menufk = lblmenu.Text;

                    if (name == "")
                    {
                        name = menuname;
                        code = menufk;
                    }
                    else
                    {
                        name = name + "," + menuname;
                        code = code + "," + menufk;
                    }
                }
                else
                {

                }
            }
            if (name.Trim() != "")
            {
                string act = Convert.ToString(Session["activerow"]);
                string actcol = Convert.ToString(Session["activecoloumn"]);
                Fpspread1.Sheets[0].Cells[Convert.ToInt32(act), Convert.ToInt32(actcol)].Text = Convert.ToString(name);
                Fpspread1.Sheets[0].Cells[Convert.ToInt32(act), Convert.ToInt32(actcol)].Tag = Convert.ToString(code);
                poperrjs.Visible = false;
            }
            else
            {
                lbl_alerterror.Visible = true;
                lbl_alerterror.Text = "Please Select Any One Items";
                alertmessage.Visible = true;
            }
        }
        catch
        {

        }
    }
    protected void btnmenuexit_click(object sender, EventArgs e)
    {
        try
        {
            poperrjs.Visible = false;
        }
        catch
        {

        }
    }



    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            bool saveflage = false;
            Fpspread1.SaveChanges();
            if (Fpspread1.Sheets[0].RowCount > 0)
            {
                if (rdbMenu.Checked == true)
                {
                    string dtaccessdate = DateTime.Now.ToString();
                    string dtaccesstime = DateTime.Now.ToLongTimeString();
                    if (rdodatewise.Checked == true)
                    {
                        for (int row = 0; row < Fpspread1.Sheets[0].RowCount; row++)
                        {
                            string getdate = Convert.ToString(Fpspread1.Sheets[0].Cells[row, 1].Text);
                            string[] splitdate = getdate.Split('-');
                            splitdate = splitdate[0].Split('/');
                            DateTime dt = new DateTime();
                            if (splitdate.Length > 0)
                            {
                                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
                            }
                            string getday = dt.ToString("dddd");
                            string hostelcode = "";

                            for (int col = 2; col < Fpspread1.Sheets[0].Columns.Count; col++)
                            {
                                // string getmenuname = Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text);
                                string getmenuname = "";
                                if (getmenuname == "")
                                {
                                    getmenuname = "" + Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text) + "";
                                }
                                else
                                {
                                    getmenuname = getmenuname + "'" + "," + "'" + Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text) + "";
                                }
                                string getmenucode = Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Tag);
                                string getsessioncode = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, col].Tag);
                                if (getmenuname.Trim() != "")
                                {
                                    string[] separators = { ",", "'" };
                                    string[] rno = getmenucode.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                    hostelcode = Convert.ToString(ddl_messname.SelectedItem.Value);
                                    //20.02.16
                                    string del = "delete HT_MenuSchedule where SessionMasterFK='" + getsessioncode + "' and ScheudleItemType='1' and ScheduleType='1' and MenuScheduleDate='" + dt.ToString("MM/dd/yyyy") + "' and MessMasterFK='" + hostelcode + "'";
                                    int del1 = d2.update_method_wo_parameter(del, "Text");

                                    for (int ij = 0; ij < rno.Length; ij++)
                                    {
                                        string mcod = Convert.ToString(rno[ij]);
                                        string insertquery = "if exists (select * from HT_MenuSchedule where MenuScheduleDate ='" + dt.ToString("MM/dd/yyyy") + "' and SessionMasterFK ='" + getsessioncode + "' and MessMasterFK ='" + hostelcode + "' and ScheudleItemType='1' and ScheduleType='1' and MenuMasterFK ='" + mcod + "') update HT_MenuSchedule set MenuMasterFK ='" + mcod + "' where MenuScheduleDate ='" + dt.ToString("MM/dd/yyyy") + "' and SessionMasterFK ='" + getsessioncode + "' and MessMasterFK ='" + hostelcode + "' and ScheudleItemType='1' and ScheduleType='1'  and MenuMasterFK ='" + mcod + "' else insert into HT_MenuSchedule (SessionMasterFK,MenuMasterFK,MessMasterFK,MenuScheduleDate,ScheudleItemType,ScheduleType) values ('" + getsessioncode + "','" + mcod + "','" + hostelcode + "','" + dt.ToString("MM/dd/yyyy") + "','1','1')";
                                        int ins = d2.update_method_wo_parameter(insertquery, "Text");

                                        if (ins != 0)
                                        {
                                            saveflage = true;
                                        }

                                    }
                                }
                            }
                        }
                    }
                    if (rdodaywise.Checked == true)
                    {
                        for (int row = 0; row < Fpspread1.Sheets[0].RowCount; row++)
                        {
                            string getday = Convert.ToString(Fpspread1.Sheets[0].Cells[row, 1].Text);
                            getday = getday.ToUpper();

                            //string curyear = DateTime.Now.Year.ToString();
                            //string curDate = "1/1/" + curyear;
                            //string lasDate = "12/31/" + curyear;
                            //DateTime startDate = Convert.ToDateTime(curDate);
                            //DateTime endDate = Convert.ToDateTime(lasDate);

                            //string curDay = startDate.DayOfWeek.ToString();
                            //curDay = curDay.ToUpper();
                            //while (getday != curDay)
                            //{
                            //    startDate = startDate.AddDays(1);
                            //    curDay = startDate.DayOfWeek.ToString();
                            //    curDay = curDay.ToUpper();
                            //}
                            //while (startDate <= endDate)
                            //{
                            //    string dateReqd = startDate.Date.ToString();

                            #region Insertion Process
                            string mess = "";
                            for (int col = 2; col < Fpspread1.Sheets[0].Columns.Count; col++)
                            {
                                string getmenuname = Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text);
                                string getmenucode = Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Tag);
                                string getsessioncode = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, col].Tag);
                                mess = Convert.ToString(ddl_messname.SelectedItem.Value);

                                if (getmenuname.Trim() != "")
                                {
                                    string[] separators = { ",", "'" };
                                    string[] mcode = getmenucode.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                    //20.02.16
                                    string del = "delete HT_MenuSchedule where SessionMasterFK='" + getsessioncode + "' and ScheudleItemType='1' and ScheduleType='2' and MenuScheduleday='" + getday + "' and MessMasterFK='" + mess + "'";
                                    int del1 = d2.update_method_wo_parameter(del, "Text");

                                    for (int ij = 0; ij < mcode.Length; ij++)
                                    {
                                        string mcod = Convert.ToString(mcode[ij]);
                                        string insertquery = "if exists (select * from HT_MenuSchedule where menuscheduleday ='" + getday + "' and SessionMasterFK ='" + getsessioncode + "' and MessMasterFK ='" + mess + "' and ScheudleItemType='1' and ScheduleType='2' and MenuMasterFK ='" + mcod + "') update HT_MenuSchedule set MenuMasterFK ='" + mcod + "' where menuscheduleday ='" + getday + "' and SessionMasterFK ='" + getsessioncode + "' and MessMasterFK ='" + mess + "' and ScheudleItemType='1' and ScheduleType='2' and MenuMasterFK ='" + mcod + "' else insert into HT_MenuSchedule (SessionMasterFK,MenuMasterFK,MessMasterFK,ScheudleItemType,ScheduleType,menuscheduleday) values ('" + getsessioncode + "','" + mcod + "','" + mess + "','1','2','" + getday + "')";
                                        int ins = d2.update_method_wo_parameter(insertquery, "Text");
                                        if (ins != 0)
                                        {
                                            saveflage = true;
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                    if (saveflage == true)
                    {
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Saved Successfully";
                        alertmessage.Visible = true;

                    }
                    else
                    {
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Please Select Any Item";
                        alertmessage.Visible = true;

                    }
                }
                if (rdbcleaning.Checked == true)
                {
                    string dtaccessdate = DateTime.Now.ToString();
                    string dtaccesstime = DateTime.Now.ToLongTimeString();
                    if (rdodatewise.Checked == true)
                    {
                        for (int row = 0; row < Fpspread1.Sheets[0].RowCount; row++)
                        {
                            string getdate = Convert.ToString(Fpspread1.Sheets[0].Cells[row, 1].Text);
                            string[] splitdate = getdate.Split('-');
                            splitdate = splitdate[0].Split('/');
                            DateTime dt = new DateTime();
                            if (splitdate.Length > 0)
                            {
                                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
                            }
                            string getday = dt.ToString("dddd");
                            string hostelcode = "";

                            for (int col = 2; col < Fpspread1.Sheets[0].Columns.Count; col++)
                            {
                                string getmenuname = Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text);
                                string getmenucode = Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Tag);
                                string getsessioncode = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, col].Tag);
                                if (getmenuname.Trim() != "")
                                {
                                    string[] separators = { ",", "'" };
                                    string[] mcode = getmenucode.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                    hostelcode = Convert.ToString(ddl_messname.SelectedItem.Value);

                                    //20.02.16
                                    string del = "delete HT_MenuSchedule where SessionMasterFK='" + getsessioncode + "' and ScheudleItemType='2' and ScheduleType='1' and MenuScheduleDate='" + dt.ToString("MM/dd/yyyy") + "' and MessMasterFK='" + hostelcode + "'";
                                    int del1 = d2.update_method_wo_parameter(del, "Text");

                                    for (int ij = 0; ij < mcode.Length; ij++)
                                    {
                                        string mcod = Convert.ToString(mcode[ij]);
                                        string insertquery = "if exists (select * from HT_MenuSchedule where MenuScheduleDate ='" + dt.ToString("MM/dd/yyyy") + "' and SessionMasterFK ='" + getsessioncode + "' and MessMasterFK ='" + hostelcode + "' and ScheudleItemType='2' and ScheduleType='1' and MenuMasterFK ='" + mcod + "') update HT_MenuSchedule set MenuMasterFK ='" + mcod + "' where MenuScheduleDate ='" + dt.ToString("MM/dd/yyyy") + "' and SessionMasterFK ='" + getsessioncode + "' and MessMasterFK ='" + hostelcode + "' and ScheudleItemType='2' and ScheduleType='1'  and MenuMasterFK ='" + mcod + "' else insert into HT_MenuSchedule (SessionMasterFK,MenuMasterFK,MessMasterFK,MenuScheduleDate,ScheudleItemType,ScheduleType) values ('" + getsessioncode + "','" + mcod + "','" + hostelcode + "','" + dt.ToString("MM/dd/yyyy") + "','2','1')";
                                        int ins = d2.update_method_wo_parameter(insertquery, "Text");
                                        if (ins != 0)
                                        {
                                            saveflage = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (rdodaywise.Checked == true)
                    {
                        for (int row = 0; row < Fpspread1.Sheets[0].RowCount; row++)
                        {
                            string getday = Convert.ToString(Fpspread1.Sheets[0].Cells[row, 1].Text);
                            //string[] splitdate = getday.Split('-');
                            //splitdate = splitdate[0].Split('/');
                            //DateTime dt = new DateTime();
                            //if (splitdate.Length > 0)
                            //{
                            //    dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
                            //}
                            string hostelcode = "";
                            for (int col = 2; col < Fpspread1.Sheets[0].Columns.Count; col++)
                            {
                                string getmenuname = Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Text);
                                string getmenucode = Convert.ToString(Fpspread1.Sheets[0].Cells[row, col].Tag);
                                string getsessioncode = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, col].Tag);
                                hostelcode = Convert.ToString(ddl_messname.SelectedItem.Value);
                                if (getmenuname.Trim() != "")
                                {
                                    string[] separators = { ",", "'" };
                                    string[] mcode = getmenucode.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                    //20.02.16
                                    string del = "delete HT_MenuSchedule where SessionMasterFK='" + getsessioncode + "' and ScheudleItemType='2' and ScheduleType='2' and MenuScheduleday='" + getday + "' and MessMasterFK='" + hostelcode + "'";
                                    int del1 = d2.update_method_wo_parameter(del, "Text");
                                    for (int ij = 0; ij < mcode.Length; ij++)
                                    {
                                        string mcod = Convert.ToString(mcode[ij]);
                                        string insertquery = "if exists (select * from HT_MenuSchedule where MenuScheduleday ='" + getday + "' and SessionMasterFK ='" + getsessioncode + "' and MessMasterFK ='" + hostelcode + "' and ScheudleItemType='2' and ScheduleType='2' and MenuMasterFK ='" + mcod + "') update HT_MenuSchedule set MenuMasterFK ='" + mcod + "' where MenuScheduleday ='" + getday + "' and SessionMasterFK ='" + getsessioncode + "' and MessMasterFK ='" + hostelcode + "' and ScheudleItemType='2' and ScheduleType='2'  and MenuMasterFK ='" + mcod + "' else insert into HT_MenuSchedule (SessionMasterFK,MenuMasterFK,MessMasterFK,MenuScheduleday,ScheudleItemType,ScheduleType) values ('" + getsessioncode + "','" + mcod + "','" + hostelcode + "','" + getday + "','2','2')";

                                        int ins = d2.update_method_wo_parameter(insertquery, "Text");
                                        if (ins != 0)
                                        {
                                            saveflage = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (saveflage == true)
                    {
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Saved Successfully";
                        alertmessage.Visible = true;
                    }
                    else
                    {
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Please Select Any Item";
                        alertmessage.Visible = true;
                    }
                }
            }
        }
        catch
        {

        }
    }

    protected void rdbMenu_Change(object sender, EventArgs e)
    {
        try
        {
            chkall1.Enabled = false;
            Fpspread1.Visible = false;
            rptprint.Visible = false;
            div1.Visible = false;
        }
        catch
        {

        }
    }

    protected void rdbCleaning_Change(object sender, EventArgs e)
    {
        try
        {
            chkall1.Enabled = true;
            Fpspread1.Visible = false;
            rptprint.Visible = false;
            div1.Visible = false;
        }
        catch
        {

        }
    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        alertmessage.Visible = false;
        gobtn_Click(sender, e);
    }

    protected void btnadd_item_Clcik(object sender, EventArgs e)
    {
        try
        {
            if (btnadd_item.Text == "Remove")
            {
                surediv.Visible = true;
                lbl_sure.Text = "Do you want to Remove this Record?";
            }
        }
        catch
        {
        }
    }
    protected void delete()
    {

        try
        {
            surediv.Visible = false;
            DataTable dt = new DataTable();
            DataRow dr;
            bool newcheck = false;
            dt.Columns.Add("ItemCode");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("Measure");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("itempk");
            if (SelectdptGrid.Rows.Count > 0)
            {
                for (int row = 0; row < SelectdptGrid.Rows.Count; row++)
                {
                    if ((SelectdptGrid.Rows[row].FindControl("cbselect") as CheckBox).Checked == false)
                    {
                        dr = dt.NewRow();
                        dr[0] = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lblitemcode") as Label).Text);
                        dr[1] = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lblitemname") as Label).Text);
                        dr[2] = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lblitemmeasure") as Label).Text);
                        dr[3] = Convert.ToString((SelectdptGrid.Rows[row].FindControl("txtquantity") as TextBox).Text);
                        dr[4] = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lblitempk") as Label).Text);

                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        newcheck = true;
                    }

                }
                if (dt.Rows.Count > 0)
                {
                    SelectdptGrid.DataSource = dt;
                    SelectdptGrid.DataBind();
                    SelectdptGrid.Visible = true;
                    Session["dt"] = dt;
                }
                else
                {
                    SelectdptGrid.Visible = false;
                    Session["dt"] = null;
                }
                if (newcheck == true)
                {
                    if (SelectdptGrid.Visible == false)
                    {
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Please select the item";
                        alertmessage.Visible = true;

                        popwindow.Visible = false;
                        popwindow1.Visible = true;
                    }
                    else
                    {
                        if (SelectdptGrid.Visible == true)
                        {
                            btnadd_item.Visible = true;
                        }
                        else
                        {
                            btnadd_item.Visible = false;
                        }
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Removed Successfully";
                        alertmessage.Visible = true;
                    }
                }
                else
                {
                    lbl_alerterror.Visible = true;
                    lbl_alerterror.Text = "Please Select Any one Items";
                    alertmessage.Visible = true;
                    //ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Any one Items\");", true);
                }


            }
        }
        catch
        {

        }
    }
    protected void btnupdate_Click(object sender, EventArgs e)
    {
        try
        {
            string dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = DateTime.Now.ToLongTimeString();

        }
        catch
        {
        }
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {


        }
        catch
        {

        }
    }

    protected void btnpopsave_Clcik(object sender, EventArgs e)
    {
        try
        {
            string dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            string sessioncode = "";
            ArrayList addarray = new ArrayList();
            bool valuecheck = false;
            int itemmenucount = 0;
            string itemvalue = "";
            string itemcodevalue = "";
            string scheduletype = "";
            string scheduledate = "";
            string scheduleday = "";
            if (rdodatewise.Checked == true)
            {
                scheduletype = "1";
            }
            else if (rdodaywise.Checked == true)
            {
                scheduletype = "2";
            }
            string insetquery = "";
            int ins = 0;
            string row = Convert.ToString(Session["activerow"]);
            string col = Convert.ToString(Session["activecoloumn"]);
            string selectquery = "";
            if (Fpspread1.Rows.Count > 0)
            {
                DateTime dt = new DateTime();
                if (rdodatewise.Checked == true)
                {
                    string getdate = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(row), 1].Text);
                    string[] splitdate = getdate.Split('-');
                    splitdate = splitdate[0].Split('/');

                    if (splitdate.Length > 0)
                    {
                        dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
                        scheduleday = "";
                        scheduledate = dt.ToString("MM/dd/yyyy");
                    }

                }
                else if (rdodaywise.Checked == true)
                {
                    scheduleday = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(row), 1].Text);
                    scheduledate = "";
                }
                insetquery = "if exists(select*from Cleaning_ItemMaseter where Schedule_Date='" + scheduledate + "' and Schedule_Day ='" + scheduleday + "' and Schedule_type='" + scheduletype + "' and messmasterfk='" + ddl_messname.SelectedItem.Value + "' and Sessionfk ='" + Convert.ToString(Session["Sessioncode"]) + "') update Cleaning_ItemMaseter set NoOfItems ='" + SelectdptGrid.Rows.Count + "',Schedule_Date='" + scheduledate + "',Schedule_Day='" + scheduleday + "',Schedule_type='" + scheduletype + "' where  Sessionfk ='" + Convert.ToString(Session["Sessioncode"]) + "' and messmasterfk ='" + ddl_messname.SelectedItem.Value + "' and Schedule_Date='" + scheduledate + "' and Schedule_Day ='" + scheduleday + "' and Schedule_type='" + scheduletype + "' else INSERT INTO Cleaning_ItemMaseter (Sessionfk,NoOfItems,messmasterfk, Schedule_Date,Schedule_Day,Schedule_type) values ('" + Convert.ToString(Session["Sessioncode"]) + "','" + SelectdptGrid.Rows.Count + "','" + ddl_messname.SelectedItem.Value + "','" + scheduledate + "','" + scheduleday + "','" + scheduletype + "')";
                ins = d2.update_method_wo_parameter(insetquery, "Text");
            }
            if (ins != 0)
            {
                if (SelectdptGrid.Rows.Count > 0)
                {
                    selectquery = d2.GetFunction("select clean_itemmasterpk from Cleaning_ItemMaseter where Sessionfk ='" + Convert.ToString(Session["Sessioncode"]) + "' and messmasterfk='" + ddl_messname.SelectedItem.Value + "' and Schedule_Day='" + scheduleday + "' and Schedule_Date='" + scheduledate + "' and Schedule_type='" + scheduletype + "'");

                    string delquery = "delete from Cleaning_ItemDetailMaster where Clean_itemMasterFK ='" + selectquery + "'";
                    int del = d2.update_method_wo_parameter(delquery, "Text");
                    for (int i = 0; i < SelectdptGrid.Rows.Count; i++)
                    {
                        string itemname = Convert.ToString((SelectdptGrid.Rows[i].FindControl("lblitemname") as Label).Text);
                        string itemcode = Convert.ToString((SelectdptGrid.Rows[i].FindControl("lblitemcode") as Label).Text);
                        string itempk = Convert.ToString((SelectdptGrid.Rows[i].FindControl("lblitempk") as Label).Text);
                        string quantityvalue = Convert.ToString((SelectdptGrid.Rows[i].FindControl("txtquantity") as TextBox).Text);
                        if (quantityvalue.Trim() != "")
                        {
                            string updatequery = "INSERT INTO Cleaning_ItemDetailMaster(Clean_ItemMasterfk,Itemfk,Needed_Qty) values ('" + selectquery + "','" + itempk + "','" + quantityvalue + "')";
                            int upd = d2.update_method_wo_parameter(updatequery, "Text");
                            if (upd != 0)
                            {
                                valuecheck = true;
                            }
                            if (!addarray.Contains(itempk))
                            {
                                if (itemvalue == "")
                                {
                                    itemvalue = Convert.ToString(itemname);
                                    itemcodevalue = Convert.ToString(itempk);
                                }
                                else
                                {
                                    itemvalue = itemvalue + "," + Convert.ToString(itemname);
                                    itemcodevalue = itemcodevalue + "," + Convert.ToString(itempk);
                                }
                                addarray.Add(itempk);
                            }
                        }
                    }
                }
            }

            if (valuecheck == true)
            {
                popwindow.Visible = false;
                Fpspread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), Convert.ToInt32(Session["activecoloumn"])].Text = Convert.ToString(itemvalue);
                Fpspread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), Convert.ToInt32(Session["activecoloumn"])].Tag = Convert.ToString(itemcodevalue);

            }
            else
            {
                if (SelectdptGrid.Visible == false)
                {

                }
                else
                {
                    lbl_alerterror.Visible = true;
                    lbl_alerterror.Text = "Update Quantity Values";
                    alertmessage.Visible = true;
                }

            }
        }
        catch
        {

        }
    }

    protected void btnpopexit_Click(object sender, EventArgs e)
    {
        gobtn_Click(sender, e);
        popwindow1.Visible = true;
        popwindow.Visible = false;
    }

    protected void SelectdptGrid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int row = Convert.ToInt32(e.CommandArgument);
            Session["rowvalue"] = Convert.ToString(row);
            if (e.CommandName == "instruction")
            {
                string itemcode = ((SelectdptGrid.Rows[row].FindControl("lblitemcode") as Label).Text);
                string itemname = ((SelectdptGrid.Rows[row].FindControl("lblitemname") as Label).Text);
                string qunatity = ((SelectdptGrid.Rows[row].FindControl("lblquantity") as Label).Text);
                //txtpopitem.Text = Convert.ToString(itemname);
                //txtpopqty.Text = Convert.ToString(qunatity);
                btnadd_item.Text = "Update";
                Session["itemnewcode"] = Convert.ToString(itemcode);
            }
        }
        catch
        {

        }
    }

    protected void typegrid_OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.SelectdptGrid, "instruction$" + e.Row.RowIndex);
                e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.SelectdptGrid, "instruction$" + e.Row.RowIndex);
                e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.SelectdptGrid, "instruction$" + e.Row.RowIndex);
                e.Row.Cells[3].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.SelectdptGrid, "instruction$" + e.Row.RowIndex);
            }
        }
        catch
        {

        }

    }

    protected void btnitemsave_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("ItemCode");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("Measure");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("itempk");
            if (Session["dt"] != null)
            {
                DataTable d1 = new DataTable();
                d1 = (DataTable)Session["dt"];
                if (d1.Rows.Count > 0)
                {
                    for (int r = 0; r < d1.Rows.Count; r++)
                    {
                        dr = dt.NewRow();
                        for (int c = 0; c < d1.Columns.Count; c++)
                        {
                            dr[c] = Convert.ToString(d1.Rows[r][c]);
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }

            int count = 0;
            foreach (DataListItem gvrow in gvdatass1.Items)
            {
                CheckBox chkSelect = (gvrow.FindControl("CheckBox2") as CheckBox);
                if (chkSelect.Checked)
                {
                    count++;
                    Label lblname1 = (Label)gvrow.FindControl("lblitemname");
                    string menuname = lblname1.Text;
                    Label lblcode = (Label)gvrow.FindControl("lblitemcode");
                    string menucode = lblcode.Text;
                    Label itemunit = (Label)gvrow.FindControl("lblitemunit");
                    string ite_unit = itemunit.Text;
                    Label itepk = (Label)gvrow.FindControl("lblitempk");
                    string itempk = itepk.Text;
                    dr = dt.NewRow();
                    dr[0] = Convert.ToString(menucode);
                    dr[1] = Convert.ToString(menuname);
                    dr[2] = Convert.ToString(ite_unit);
                    dr[3] = Convert.ToString("");
                    dr[4] = Convert.ToString(itempk);
                    dt.Rows.Add(dr);
                    if (dt.Rows.Count > 0)
                    {
                        SelectdptGrid.DataSource = dt;
                        SelectdptGrid.DataBind();
                        SelectdptGrid.Visible = true;
                        Session["dt"] = dt;
                        popwindow1.Visible = false;
                        popwindow.Visible = true;
                        btnpopsave.Visible = true;
                        btnadd_item.Visible = true;
                    }
                }
            }
            if (count == 0)
            {
                lbl_alerterror.Visible = true;
                lbl_alerterror.Text = "Please Select Any one Item";
                alertmessage.Visible = true;
            }

            //string word = txtquantity.Text.Trim();
            //string[] wordArr = word.Split('.');
            //if (wordArr.Length > 1)
            //{
            //    string afterDot = wordArr[1];
            //    if (afterDot.Length > 2)
            //    {
            //        alert("Only 2 allowed");
            //        txtquantity.Text = wordArr[0] + "." + afterDot.SubString(0, 2);
            //    }
            //}
        }
        catch
        {

        }

    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getmenu(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct MenuName  from HM_MenuMaster where  MenuName like '" + prefixText + "%' order by MenuName";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["MenuName"].ToString());
            }
        }
        return name;
    }



    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getitem(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();

        string query = "select distinct  ItemName from IM_ItemMaster where ItemName like '" + prefixText + "%'  order by ItemName ";

        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["ItemName"].ToString());
            }
        }
        return name;
    }


    protected void txtfromdate_Textchanged(object sender, EventArgs e)
    {
        try
        {
            if (txtfromdate.Text != "" && txttodate.Text != "")
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txtfromdate.Text);
                string seconddate = Convert.ToString(txttodate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;

                if (dt > dt1)
                {
                    alertmessage.Visible = true;
                    lbl_alerterror.Visible = true;
                    lbl_alerterror.Text = "Enter FromDate less than or equal to the ToDate";
                    txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                }
                else
                {

                }
            }
        }
        catch (Exception ex)
        {
        }

    }
    protected void txttodate_Textchanged(object sender, EventArgs e)
    {
        try
        {
            if (txttodate.Text != "" && txtfromdate.Text != "")
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txtfromdate.Text);
                string seconddate = Convert.ToString(txttodate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;

                if (dt > dt1)
                {
                    alertmessage.Visible = true;
                    lbl_alerterror.Visible = true;
                    lbl_alerterror.Text = "Enter ToDate greater than or equal to the FromDate";
                    txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                }
                else
                {

                }
            }
        }
        catch (Exception ex)
        {
        }

    }


    public void bindmessname()
    {
        try
        {


            ds.Clear();
            //string selectQuery = "select MessMasterPK,MessName,MessAcr from HM_MessMaster where CollegeCode=" + collegecode1 + " order by MessMasterPK asc";
            //ds.Clear();
            //ds = d2.select_method_wo_parameter(selectQuery, "Text");
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_messname.DataSource = ds;
                ddl_messname.DataTextField = "MessName";
                ddl_messname.DataValueField = "MessMasterPK";
                ddl_messname.DataBind();


            }
        }
        catch
        {
        }
    }
    protected void ddl_messname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex)
        {
        }
    }



    protected void cb_subheadername_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_subheadername.Checked == true)
            {
                for (int i = 0; i < cbl_subheadername.Items.Count; i++)
                {
                    cbl_subheadername.Items[i].Selected = true;
                }
                txt_subheadername.Text = "Sub Header Name(" + (cbl_subheadername.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_subheadername.Items.Count; i++)
                {
                    cbl_subheadername.Items[i].Selected = false;
                }
                txt_subheadername.Text = "--Select--";
            }
            // loadsubheadername();
            itemmaster();

        }
        catch (Exception ex)
        {

        }
    }
    protected void cbl_subheadername_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_subheadername.Text = "--Select--";
            cb_subheadername.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_subheadername.Items.Count; i++)
            {
                if (cbl_subheadername.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_subheadername.Text = "Sub Header Name(" + commcount.ToString() + ")";
                if (commcount == cbl_subheadername.Items.Count)
                {
                    cb_subheadername.Checked = true;
                }
            }
            itemmaster();
        }
        catch (Exception ex)
        {
        }
    }
    public void loadsubheadername()
    {
        try
        {
            cbl_subheadername.Items.Clear();
            string itemheader = "";
            for (int i = 0; i < chklst_pop2itemheader.Items.Count; i++)
            {
                if (chklst_pop2itemheader.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + chklst_pop2itemheader.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + chklst_pop2itemheader.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemheader.Trim() != "")
            {
                string query = "";
                //  query = "select distinct t.TextCode,t.TextVal  from TextValTable t,item_master i where t.TextCode=i.subheader_code and itemheader_code in ('" + itemheader + "') and college_code in ('" + collegecode1 + "')";
                query = "select distinct t.MasterCode,t.MasterValue  from CO_MasterValues t,IM_ItemMaster i where t.MasterCode=i.subheader_code and ItemHeaderCode in ('" + itemheader + "') and CollegeCode in ('" + collegecode1 + "')";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                // ds.Clear();
                // ds = d2.BindItemCodeAll(itemheader);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_subheadername.DataSource = ds;
                    cbl_subheadername.DataTextField = "MasterValue";
                    cbl_subheadername.DataValueField = "MasterCode";
                    cbl_subheadername.DataBind();
                    if (cbl_subheadername.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_subheadername.Items.Count; i++)
                        {
                            cbl_subheadername.Items[i].Selected = true;
                        }
                        txt_subheadername.Text = "Sub Header Name(" + cbl_subheadername.Items.Count + ")";
                    }
                    if (cbl_subheadername.Items.Count > 5)
                    {
                        Panel5.Width = 300;
                        Panel5.Height = 300;
                    }
                }
                else
                {
                    txt_subheadername.Text = "--Select--";
                }
            }
            else
            {
                txt_subheadername.Text = "--Select--";
            }
        }
        catch
        {
        }
    }

    public int findStatus(string val)
    {
        int Number = 0;
        switch (val)
        {
            case "Monday":
                Number = 1;
                break;
            case "A":
                Number = 1;
                break;
            case "B":
                Number = 1;
                break;
            case "C":
                Number = 1;
                break;
            case "D":
                Number = 1;
                break;
            default:
                Number = 0;
                break;
        }
        return Number;
    }
    protected void cb_menutype_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_menutype.Checked == true)
            {
                for (int i = 0; i < cbl_menutype.Items.Count; i++)
                {
                    cbl_menutype.Items[i].Selected = true;
                }
                txt_menutype.Text = "Menu Type(" + (cbl_menutype.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_menutype.Items.Count; i++)
                {
                    cbl_menutype.Items[i].Selected = false;
                }
                txt_menutype.Text = "--Select--";
            }

        }
        catch (Exception ex)
        { }
        //loadmenuname();
    }
    protected void cbl_menutype_SelectIndexChange(object sender, EventArgs e)
    {
        try
        {
            txt_menutype.Text = "--Select--";
            cb_menutype.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_menutype.Items.Count; i++)
            {
                if (cbl_menutype.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_menutype.Text = "Menu Type(" + commcount.ToString() + ")";
                if (commcount == cbl_menutype.Items.Count)
                {
                    cb_menutype.Checked = true;
                }
            }
        }
        catch (Exception ex)
        { }
        //loadmenuname();
    }
    protected void btn_menutype_Click(object sender, EventArgs e)
    {
        try
        {
            menu();
        }
        catch { }
    }
}