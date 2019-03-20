using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Configuration;

public partial class Inv_Hostel_setting : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    DAccess2 dasri = new DAccess2();
    bool check = false;
    DataSet ds11 = new DataSet();
    private object sender;
    private EventArgs e;

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
        if (!IsPostBack)
        {
            rdodatewise.Checked = true;
            txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtfromdate.Attributes.Add("readonly", "readonly");
            txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txttodate.Attributes.Add("readonly", "readonly");
            txtfromdate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtfromdate1.Attributes.Add("readonly", "readonly");
            txttodate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txttodate1.Attributes.Add("readonly", "readonly");
            rdodatewise1.Checked = true;
            loadhostel();
            loadsession();

            loadhostel1();
            loadsession1();
            Bindhour();

            loadhour();
            loadsecond();
            loadminits();
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread1.Visible = false;
            btngo_Click(sender, e);
            errorlable.Visible = false;
        }
        lblvalidation1.Visible = false;
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

    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        divPopper.Visible = false;
    }

    public void loadhostel()
    {
        try
        {
            ds.Clear();
            chklsthostel.Items.Clear();
            ds.Clear();
            // ds = d2.Bindmess_inv(collegecode1);
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklsthostel.DataSource = ds;
                chklsthostel.DataTextField = "MessName";
                chklsthostel.DataValueField = "MessMasterPK";
                chklsthostel.DataBind();

                if (chklsthostel.Items.Count > 0)
                {
                    for (int i = 0; i < chklsthostel.Items.Count; i++)
                    {
                        chklsthostel.Items[i].Selected = true;
                    }

                    txthostelname.Text = "Mess Name(" + chklsthostel.Items.Count + ")";
                }
            }
            else
            {
                txthostelname.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void chk_hostel_CheckedChanged(object sender, EventArgs e)
    {

        if (chkhostelname.Checked == true)
        {
            for (int i = 0; i < chklsthostel.Items.Count; i++)
            {
                chklsthostel.Items[i].Selected = true;
            }
            txthostelname.Text = "Mess Name(" + (chklsthostel.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chklsthostel.Items.Count; i++)
            {
                chklsthostel.Items[i].Selected = false;
            }
            txthostelname.Text = "--Select--";
        }
        loadsession();
    }
    protected void chklst_hostel_SelectedIndexChanged(object sender, EventArgs e)
    {
        txthostelname.Text = "--Select--";
        chkhostelname.Checked = false;
        int commcount = 0;
        for (int i = 0; i < chklsthostel.Items.Count; i++)
        {
            if (chklsthostel.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txthostelname.Text = "Mess Name(" + commcount.ToString() + ")";
            if (commcount == chklsthostel.Items.Count)
            {
                chkhostelname.Checked = true;
            }
        }
        loadsession();
    }

    public void loadsession()
    {
        try
        {
            ds.Clear();
            chklstsession.Items.Clear();
            string itemheader = "";
            for (int i = 0; i < chklsthostel.Items.Count; i++)
            {
                if (chklsthostel.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + chklsthostel.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + chklsthostel.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemheader.Trim() != "")
            {
                ds = d2.BindSession_inv(itemheader);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklstsession.DataSource = ds;
                    chklstsession.DataTextField = "SessionName";
                    chklstsession.DataValueField = "SessionMasterPK";
                    chklstsession.DataBind();
                    if (chklstsession.Items.Count > 0)
                    {
                        for (int i = 0; i < chklstsession.Items.Count; i++)
                        {
                            chklstsession.Items[i].Selected = true;
                        }
                        txtsessionname.Text = "Session Name(" + chklstsession.Items.Count + ")";
                    }
                }
                else
                {
                    txtsessionname.Text = "--Select--";
                }
            }
            else
            {
                txtsessionname.Text = "--Select--";
            }
        }
        catch
        {

        }
    }
    protected void chksession_checkedchange(object sender, EventArgs e)
    {
        if (chksessionname.Checked == true)
        {
            for (int i = 0; i < chklstsession.Items.Count; i++)
            {
                chklstsession.Items[i].Selected = true;
            }
            txtsessionname.Text = "Session Name(" + (chklstsession.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chklstsession.Items.Count; i++)
            {
                chklstsession.Items[i].Selected = false;
            }
            txtsessionname.Text = "--Select--";
        }
    }
    protected void chklstsession_Change(object sender, EventArgs e)
    {
        txtsessionname.Text = "--Select--";
        chksessionname.Checked = false;
        int commcount = 0;
        for (int i = 0; i < chklstsession.Items.Count; i++)
        {
            if (chklstsession.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtsessionname.Text = "Session Name(" + commcount.ToString() + ")";
            if (commcount == chklstsession.Items.Count)
            {
                chksessionname.Checked = true;
            }
        }
    }

    protected void rdodatewise_CheckedChanged(object sender, EventArgs e)
    {
        lblfromdate.Enabled = true;
        txtfromdate.Enabled = true;
        lbltodate.Enabled = true;
        txttodate.Enabled = true;
        div1.Visible = false;
        rptprint.Visible = false;
        txt_Daywise.Enabled = false;
        txt_Daywise.Text = "--Select--";

    }
    protected void rdodaywise_CheckedChanged(object sender, EventArgs e)
    {
        lblfromdate.Enabled = false;
        txtfromdate.Enabled = false;
        lbltodate.Enabled = false;
        txttodate.Enabled = false;
        div1.Visible = false;
        rptprint.Visible = false;
        txt_Daywise.Enabled = true;
        cbDaywise.Checked = false;
        cbDaywise_change(sender, e);



    }

    protected void txtfrom_TextChanged(object sender, EventArgs e)
    {
        try
        {

            lblerror.Visible = false;
            string fromdate = txtfromdate.Text;
            string todate = txttodate.Text;
            if (fromdate != "" && fromdate != null && todate != "" && todate != null)
            {
                string[] spiltfrom = fromdate.Split('/');
                string[] spitto = todate.Split('/');
                DateTime from = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
                DateTime to = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);
                if (from > to)
                {
                    //lblerror.Visible = true;
                    //lblerror.Text = "Please Enter To Date Greater Than From Date";
                    imgdiv2.Visible = true;

                    lbl_alert.Text = "Enter FromDate less than or equal to the ToDate";
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                    div1.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }
    protected void txtto_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblerror.Visible = false;
            string fromdate = txtfromdate.Text;
            string todate = txttodate.Text;
            if (fromdate != "" && fromdate != null && todate != "" && todate != null)
            {
                string[] spiltfrom = fromdate.Split('/');
                string[] spitto = todate.Split('/');
                DateTime from = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
                DateTime to = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);
                if (from > to)
                {
                    //  lblerror.Visible = true;
                    // lblerror.Text = "Please Enter To Date Grater Than From Date";
                    imgdiv2.Visible = true;

                    lbl_alert.Text = "Enter ToDate greater than or equal to the FromDate";
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                    div1.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void cbDaywise_change(object sender, EventArgs e)
    {
        try
        {
            if (cbDaywise.Checked == true)
            {
                for (int i = 0; i < Cbldaywise.Items.Count; i++)
                {
                    Cbldaywise.Items[i].Selected = true;
                    // errorlable.Visible = false;
                }

                txt_Daywise.Text = "Day(" + (Cbldaywise.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Cbldaywise.Items.Count; i++)
                {
                    Cbldaywise.Items[i].Selected = false;
                }
                txt_Daywise.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void Cbldaywise_selectIndex(object sender, EventArgs e)
    {
        try
        {
            txt_Daywise.Text = "--Select--";
            cbDaywise.Checked = false;
            int commcount = 0;
            for (int i = 0; i < Cbldaywise.Items.Count; i++)
            {
                if (Cbldaywise.Items[i].Selected == true)
                {

                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {

                //  errorlable.Text = "jhghf";

                txt_Daywise.Text = "Day(" + commcount.ToString() + ")";
                if (commcount == Cbldaywise.Items.Count)
                {
                    cbDaywise.Checked = true;
                }
            }
        }
        catch
        {
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            Fpspread1.Visible = true;
            string itemheadercode = "";
            for (int i = 0; i < chklsthostel.Items.Count; i++)
            {
                if (chklsthostel.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + chklsthostel.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + chklsthostel.Items[i].Value.ToString() + "";
                    }
                }
            }
            string itemcode = "";
            for (int i = 0; i < chklstsession.Items.Count; i++)
            {
                if (chklstsession.Items[i].Selected == true)
                {
                    if (itemcode == "")
                    {
                        itemcode = "" + chklstsession.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemcode = itemcode + "'" + "," + "'" + chklstsession.Items[i].Value.ToString() + "";
                    }
                }
            }
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string type = "";
            string days = "";
            if (rdodatewise.Checked == true)
            {
                type = "0";
                string firstdate = Convert.ToString(txtfromdate.Text);
                string secondate = Convert.ToString(txttodate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = secondate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            }

            if (rdodaywise.Checked == true)
            {
                type = "1";
                if (Cbldaywise.Items.Count > 0)
                {
                    for (int rs = 0; rs < Cbldaywise.Items.Count; rs++)
                    {
                        if (Cbldaywise.Items[rs].Selected == true)
                        {
                            if (days == "")
                            {
                                days = Convert.ToString(Cbldaywise.Items[rs].Text);
                            }
                            else
                            {
                                days = days + "'" + "," + "'" + Convert.ToString(Cbldaywise.Items[rs].Text);
                            }
                        }
                    }
                }
            }
            string selectquery = "";
            if (itemcode.Trim() != "" && itemheadercode.Trim() != "" && dt <= dt1)
            {
                if (type == "0")
                {
                    selectquery = "select hs.Hostel_code,m.MessMasterpK,Session_code,m.MessName,s.SessionName,CONVERT(varchar(10),Schedule_date,103) as Schedule_date,EditMenuTotal,Use_Attendance,Att_Hour,Staff_Total,daily_consumption,Mess_attendance_set,case when AllStudentAttendance=1 then 'Yes' when isnull(AllStudentAttendance,0)=0 then 'No' end AllStudentAttendance from HostelIns_settings hs,HM_SessionMaster s,HM_MessMaster M where hs.Hostel_code=s.MessMasterFK and hs.Session_code =s.SessionMasterPK and hs.Hostel_code in('" + itemheadercode + "') and hs.Session_code in('" + itemcode + "') and hs.Schedule_type ='" + type + "' AND m.MessMasterPK=hs.Hostel_code and Schedule_date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' order by hs.Hostel_code,Schedule_Date";

                }
                if (type == "1")
                {
                    selectquery = " select hs.Hostel_code,m.MessMasterpK,Session_code,m.MessName,s.SessionName,Schedule_Day, EditMenuTotal,Use_Attendance,Att_Hour,Staff_Total,daily_consumption,Mess_attendance_set,case when AllStudentAttendance=1 then 'Yes' when isnull(AllStudentAttendance,0)=0 then 'No' end AllStudentAttendance from HostelIns_settings hs,HM_SessionMaster s,HM_MessMaster M where hs.Hostel_code=s.MessMasterFK and hs.Session_code =s.SessionMasterPK and hs.Hostel_code in('" + itemheadercode + "') and hs.Session_code in('" + itemcode + "') and hs.Schedule_type ='" + type + "' AND m.MessMasterPK=hs.Hostel_code and Schedule_Day in ('" + days + "') order by hs.Hostel_code";

                }
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
                    Fpspread1.Sheets[0].ColumnCount = 11;
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

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Mess Name";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Session Name";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    if (rdodatewise.Checked == true)
                    {
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Schedule Date";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    }
                    else if (rdodaywise.Checked == true)
                    {
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Schedule Day";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    }

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Edit Menu";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    //Fpspread1.Sheets[0].Columns[4].Locked = false;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Use Attendance";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Attendance Hour";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Staff Attendance";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Daily Consumption";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Mess Attendance";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "All Student Attendance";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;

                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        Fpspread1.Sheets[0].RowCount++;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(ds.Tables[0].Rows[row]["MessMasterpK"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Session_Code"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["MessName"]);
                        //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Hostel_code"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["SessionName"]);
                        //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Session_Code"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        if (type == "0")
                        {
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Schedule_date"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        }
                        else
                        {
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Schedule_Day"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        }

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["EditMenuTotal"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        string nicevalue = "";
                        if (Convert.ToString(ds.Tables[0].Rows[row]["Use_Attendance"]) == "True")
                        {
                            nicevalue = "Yes";
                        }
                        else
                        {
                            nicevalue = "No";
                        }
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(nicevalue);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["Att_Hour"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        nicevalue = "";
                        if (Convert.ToString(ds.Tables[0].Rows[row]["Staff_Total"]) == "True")
                        {
                            nicevalue = "Yes";
                        }
                        else
                        {
                            nicevalue = "No";
                        }
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(nicevalue);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[row]["daily_consumption"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[row]["Mess_attendance_set"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(ds.Tables[0].Rows[row]["AllStudentAttendance"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";

                    }
                    Fpspread1.Visible = true;
                    rptprint.Visible = true;
                    div1.Visible = true;
                    errorlable.Visible = false;
                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    // Fpspread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                }
                else
                {
                    if (rdodaywise.Checked == true)
                    {
                        //theivamani 6.11.15
                        if (txt_Daywise.Text == "--Select--")
                        {
                            div1.Visible = false;
                            Fpspread1.Visible = false;
                            rptprint.Visible = false;

                            errorlable.Visible = true;
                            errorlable.Text = "Please Select the Day";
                        }
                        else
                        {
                            div1.Visible = false;
                            Fpspread1.Visible = false;
                            rptprint.Visible = false;
                            errorlable.Visible = true;
                            errorlable.Text = "No Records Found";
                        }
                    }
                    else
                    {
                        div1.Visible = false;
                        Fpspread1.Visible = false;
                        rptprint.Visible = false;
                        errorlable.Visible = true;
                        errorlable.Text = "No Records Found";
                    }
                }
            }
            else
            {
                div1.Visible = false;
                Fpspread1.Visible = false;
                rptprint.Visible = false;
                errorlable.Visible = true;
                errorlable.Text = "No Records Found";
            }
        }
        catch
        {

        }

    }
    protected void btnaddnew_Click(object sender, EventArgs e)
    {
        divPopper.Visible = true;
        btnsave.Visible = true;
        btn_update.Visible = false;
        btn_delete.Visible = false;
        loadhostel1();
        loadsession1();
        loadhour();
        txtfromdate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txttodate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtfromdate1.Enabled = true;
        txttodate1.Enabled = true;

        txtdaycompar.Text = "--Select--";
        txtdaycompar.Enabled = false;
        rdodatewise1.Checked = true;
        rdodaywise1.Checked = false;
        cbstudentAttendancehour.Checked = false;
        txt_Attendancehour.Text = "Select";
        cbstaffbiomarric.Checked = false;
        for (int i = 0; i < cbltypeoftotal.Items.Count; i++)
        {
            cbltypeoftotal.Items[i].Selected = false;
        }
        for (int i = 0; i < daily_consumption.Items.Count; i++)
        {
            daily_consumption.Items[i].Selected = false;
        }
        bindaddgroup();
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
    protected void Fpspread1_render(object sender, EventArgs e)
    {
        if (check == true)
        {
            try
            {
                divPopper.Visible = true;
                loadhostel1();
                btnsave.Visible = false;
                btn_update.Visible = true;
                btn_delete.Visible = true;
                DataView dv1 = new DataView();
                int activerow = 0;
                activerow = Convert.ToInt32(Fpspread1.ActiveSheetView.ActiveRow.ToString());
                for (int i = 0; i < Fpspread1.Sheets[0].RowCount; i++)
                {
                    if (i == Convert.ToInt32(activerow))
                    {
                        Fpspread1.Sheets[0].Rows[i].BackColor = Color.LightBlue;
                        Fpspread1.Sheets[0].SelectionBackColor = Color.Orange;
                        Fpspread1.Sheets[0].SelectionForeColor = Color.White;
                    }
                    else
                    {
                        Fpspread1.Sheets[0].Rows[i].BackColor = Color.White;
                    }
                }
                //if (activerow != null)
                //{
                string hostel = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                //string hostelcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                //Session["hostelcode1"] = Convert.ToString(hostelcode);
                //txthostelname1.Text = hostel;

                string hostelcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Note);
                int ch = 0;
                chklsthostel1.ClearSelection();
                if (chklsthostel1.Items.Count > 0)
                {
                    for (int row = 0; row < chklsthostel1.Items.Count; row++)
                    {
                        if (chklsthostel1.Items[row].Value == hostelcode)
                        {
                            ch++;
                            chklsthostel1.Items[row].Selected = true;
                        }
                    }
                    if (ch != 0)
                    {
                        txthostelname1.Text = "Mess Name (" + ch + ")";
                    }
                    else
                    {
                        txthostelname1.Text = "--Select--";
                    }
                }

                string session = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                //string sessioncode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                //Session["sessioncode"] = Convert.ToString(sessioncode);
                //txtsessionname1.Text = session;

                string sessioncode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                int ch1 = 0;
                chklstsession1.ClearSelection();
                if (chklstsession1.Items.Count > 0)
                {
                    for (int row = 0; row < chklstsession1.Items.Count; row++)
                    {
                        if (chklstsession1.Items[row].Value == sessioncode)
                        {
                            ch1++;
                            chklstsession1.Items[row].Selected = true;
                        }
                    }
                    if (ch1 != 0)
                    {
                        txtsessionname1.Text = "Session Name (" + ch1 + ")";
                    }
                    else
                    {
                        txtsessionname1.Text = "--Select--";
                    }
                }

                if (rdodaywise.Checked == true)
                {
                    rdodaywise1.Checked = true;
                    txtdaycompar.Enabled = true;
                    txtfromdate1.Enabled = false;
                    txttodate1.Enabled = false;
                    chklstdaycompar.ClearSelection();
                    string day = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                    if (day != "")
                    {
                        string[] daysplit;
                        if (day != "")
                        {
                            daysplit = day.Split(',');
                            for (int z = 0; z < chklstdaycompar.Items.Count; z++)
                            {
                                for (int y = 0; y < daysplit.Length; y++)
                                {
                                    if (daysplit[y] == chklstdaycompar.Items[z].Text)
                                    {
                                        chklstdaycompar.Items[z].Selected = true;
                                    }
                                    txtdaycompar.Text = "Day(" + daysplit.Length + ")";
                                }
                            }
                        }
                    }
                }
                else
                {
                    txtdaycompar.Text = "--Select--";
                    rdodaywise1.Checked = false;
                    txtdaycompar.Enabled = false;
                    txtfromdate1.Enabled = true;
                    txttodate1.Enabled = true;
                }
                if (rdodatewise.Checked == true)
                {
                    rdodaywise1.Checked = false;
                    txtdaycompar.Enabled = false;
                    txtfromdate1.Enabled = true;
                    rdodatewise1.Checked = true;
                    txttodate1.Enabled = true;
                    string date = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                    if (date != "")
                    {
                        txtfromdate1.Text = txtfromdate.Text;
                        txttodate1.Text = txttodate.Text;
                    }
                }
                cbltypeoftotal.ClearSelection();


                string edit = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                string[] editsplit;
                if (edit != "")
                {
                    editsplit = edit.Split(',');
                    for (int z = 0; z < cbltypeoftotal.Items.Count; z++)
                    {
                        for (int y = 0; y < editsplit.Length; y++)
                        {
                            if (editsplit[y] == cbltypeoftotal.Items[z].Text)
                            {
                                cbltypeoftotal.Items[z].Selected = true;
                            }
                        }
                    }
                }
                //27.10.15
                // ddl_itemheadername.SelectedIndex = ddl_itemheadername.Items.IndexOf(ddl_itemheadername.Items.FindByValue(itemheader));
                daily_consumption.ClearSelection();

                string edit1 = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 8].Text;
                string[] editsplit1;
                if (edit1 != "")
                {
                    editsplit1 = edit1.Split(',');
                    for (int z = 0; z < daily_consumption.Items.Count; z++)
                    {
                        for (int y = 0; y < editsplit1.Length; y++)
                        {
                            if (editsplit1[y] == daily_consumption.Items[z].Text)
                            {
                                daily_consumption.Items[z].Selected = true;
                            }
                        }
                    }
                }

                cbl_messattendance.ClearSelection();
                string messset = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Text;
                string[] mess_set;
                if (messset != "")
                {
                    mess_set = messset.Split(',');
                    for (int z = 0; z < cbl_messattendance.Items.Count; z++)
                    {
                        for (int y = 0; y < mess_set.Length; y++)
                        {
                            if (mess_set[y] == cbl_messattendance.Items[z].Text)
                            {
                                cbl_messattendance.Items[z].Selected = true;
                            }
                        }
                    }
                }
                //string usestd = Convert.ToString(ds.Tables[0].Rows[0]["Use_Attendance"]);
                string usestd = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text;
                string attnhour = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text;
                if (usestd != "No")
                {
                    cbstudentAttendancehour.Checked = true;
                    ddl_hour.Enabled = true;
                    //ddl_hour.SelectedItem.Text = Convert.ToString(attnhour);
                    ddl_hour.SelectedIndex = ddl_hour.Items.IndexOf(ddl_hour.Items.FindByValue(attnhour));
                }
                else
                {
                    cbstudentAttendancehour.Checked = false;
                    ddl_hour.Enabled = false;
                    attnhour = "";
                }

                //string attnhour = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text;
                //if (attnhour != "")
                //{
                //    ddl_hour.SelectedItem.Text = Convert.ToString(attnhour);
                //}

                string usestaff = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Text;
                if (usestaff != "No")
                {
                    cbstaffbiomarric.Checked = true;
                }
                else
                {
                    cbstaffbiomarric.Checked = false;
                }

                string allstudentattendance = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 10].Text;
                if (allstudentattendance.ToUpper() != "NO")
                {
                    cbAllstudentAttendance.Checked = true;
                }
                else
                {
                    cbAllstudentAttendance.Checked = false;
                }

                //}
            }
            catch
            {
            }
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
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {

        try
        {
            //string pagename = "Hostel_Setting.aspx";
            //string student = "Hostel Setting Report";
            //Printcontrol.loadspreaddetails(Fpspread1, pagename, student);
            //Printcontrol.Visible = true;

            string degreedetails = "Hostel Setting Report";
            string pagename = "Hostel_Setting.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        divPopper.Visible = false;
    }
    public void loadhostel1()
    {
        try
        {
            ds.Clear();
            chklsthostel1.Items.Clear();
            ds.Clear();
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklsthostel1.DataSource = ds;
                chklsthostel1.DataTextField = "MessName";
                chklsthostel1.DataValueField = "MessMasterPK";
                chklsthostel1.DataBind();

                if (chklsthostel1.Items.Count > 0)
                {
                    for (int i = 0; i < chklsthostel1.Items.Count; i++)
                    {
                        chklsthostel1.Items[i].Selected = true;
                    }

                    txthostelname1.Text = "Mess Name(" + chklsthostel1.Items.Count + ")";
                }
            }
            else
            {
                txthostelname1.Text = "--Select--";

            }
        }
        catch
        {
        }
    }
    protected void chk_hostel1_CheckedChanged(object sender, EventArgs e)
    {

        if (chkhostelname1.Checked == true)
        {
            for (int i = 0; i < chklsthostel1.Items.Count; i++)
            {
                chklsthostel1.Items[i].Selected = true;
            }
            txthostelname1.Text = "Mess Name(" + (chklsthostel1.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chklsthostel1.Items.Count; i++)
            {
                chklsthostel1.Items[i].Selected = false;
            }
            txthostelname1.Text = "--Select--";
        }
        loadsession1();
    }
    protected void chklst_hostel1_SelectedIndexChanged(object sender, EventArgs e)
    {
        txthostelname1.Text = "--Select--";
        chkhostelname1.Checked = false;
        int commcount = 0;
        for (int i = 0; i < chklsthostel1.Items.Count; i++)
        {
            if (chklsthostel1.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txthostelname1.Text = "Mess Name(" + commcount.ToString() + ")";
            if (commcount == chklsthostel1.Items.Count)
            {
                chkhostelname1.Checked = true;
            }
        }
        loadsession1();
    }

    public void loadsession1()
    {
        try
        {
            ds.Clear();
            chklstsession1.Items.Clear();
            string itemheader = "";
            for (int i = 0; i < chklsthostel1.Items.Count; i++)
            {
                if (chklsthostel1.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + chklsthostel1.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + chklsthostel1.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemheader.Trim() != "")
            {
                //string selecthostel = "select distinct Session_Code,Session_Name  from Session_Master where Hostel_Code in ('" + itemheader + "')";
                ds = d2.BindSession_inv(itemheader);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklstsession1.DataSource = ds;
                    chklstsession1.DataTextField = "SessionName";
                    chklstsession1.DataValueField = "SessionMasterPK";
                    chklstsession1.DataBind();
                    if (chklstsession1.Items.Count > 0)
                    {
                        for (int i = 0; i < chklstsession1.Items.Count; i++)
                        {
                            chklstsession1.Items[i].Selected = true;
                        }
                        txtsessionname1.Text = "Session Name(" + chklstsession1.Items.Count + ")";
                    }
                }
                else
                {
                    txtsessionname1.Text = "--Select--";
                }
            }
            else
            {
                txtsessionname1.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void chksession1_checkedchange(object sender, EventArgs e)
    {
        if (chksessionname1.Checked == true)
        {
            for (int i = 0; i < chklstsession1.Items.Count; i++)
            {
                chklstsession1.Items[i].Selected = true;
            }
            txtsessionname1.Text = "Session Name(" + (chklstsession1.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chklstsession1.Items.Count; i++)
            {
                chklstsession1.Items[i].Selected = false;
            }
            txtsessionname1.Text = "--Select--";
        }

    }
    protected void chklstsession1_Change(object sender, EventArgs e)
    {
        txtsessionname1.Text = "--Select--";
        chksessionname1.Checked = false;
        int commcount = 0;
        for (int i = 0; i < chklstsession1.Items.Count; i++)
        {
            if (chklstsession1.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtsessionname1.Text = "Session Name(" + commcount.ToString() + ")";
            if (commcount == chklstsession1.Items.Count)
            {
                chksessionname1.Checked = true;
            }
        }
    }

    protected void rdodatewise1_CheckedChanged(object sender, EventArgs e)
    {
        lblfromdate1.Enabled = true;
        txtfromdate1.Enabled = true;
        lbltodate1.Enabled = true;
        txttodate1.Enabled = true;
        txtdaycompar.Enabled = false;
    }
    protected void rdodaywise1_CheckedChanged(object sender, EventArgs e)
    {
        lblfromdate1.Enabled = false;
        txtfromdate1.Enabled = false;
        lbltodate1.Enabled = false;
        txttodate1.Enabled = false;
        txtdaycompar.Enabled = true;
        chkdaycompar.Checked = false;

        chdaycompar_change(sender, e);
        chkklstdaycompar_selectIndex(sender, e);
    }
    protected void chdaycompar_change(object sender, EventArgs e)
    {
        try
        {
            if (chkdaycompar.Checked == true)
            {
                for (int i = 0; i < chklstdaycompar.Items.Count; i++)
                {
                    chklstdaycompar.Items[i].Selected = true;
                }
                txtdaycompar.Text = "day (" + (chklstdaycompar.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstdaycompar.Items.Count; i++)
                {
                    chklstdaycompar.Items[i].Selected = false;
                }
                txtdaycompar.Text = "--Select--";
            }
        }
        catch
        {

        }
    }
    protected void chkklstdaycompar_selectIndex(object sender, EventArgs e)
    {
        try
        {
            txtdaycompar.Text = "--Select--";
            chkdaycompar.Checked = false;
            int commcount = 0;
            for (int i = 0; i < chklstdaycompar.Items.Count; i++)
            {
                if (chklstdaycompar.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtdaycompar.Text = "Day (" + commcount.ToString() + ")";
                if (commcount == chklstdaycompar.Items.Count)
                {
                    chkdaycompar.Checked = true;
                }
            }
            if (txtdaycompar.Text == "--Select--")
            {
                for (int i = 0; i < chklstdaycompar.Items.Count; i++)
                {
                    chklstdaycompar.Items[i].Selected = false;
                }
            }
        }
        catch
        {

        }
    }

    protected void btngo1_Click(object sender, EventArgs e)
    {
    }

    protected void cbstudentAttendancehour_Change(object sender, EventArgs e)
    {
        try
        {
            if (cbstudentAttendancehour.Checked == true)
            {
                ddl_hour.Enabled = true;
            }
            else
            {
                ddl_hour.Enabled = false;
            }
        }
        catch
        {

        }
    }
    protected void cbAttendancehour_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbAttendancehour.Checked == true)
            {
                for (int i = 0; i < cblAttendancehour.Items.Count; i++)
                {
                    cblAttendancehour.Items[i].Selected = true;
                }
                txt_Attendancehour.Text = "Hour(" + (cblAttendancehour.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cblAttendancehour.Items.Count; i++)
                {
                    cblAttendancehour.Items[i].Selected = false;
                }
                txt_Attendancehour.Text = "--Select--";
            }
        }
        catch
        {

        }
    }
    protected void cblAttendancehour_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_Attendancehour.Text = "--Select--";
            cbAttendancehour.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cblAttendancehour.Items.Count; i++)
            {
                if (cblAttendancehour.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_Attendancehour.Text = "Hour(" + commcount.ToString() + ")";
                if (commcount == cblAttendancehour.Items.Count)
                {
                    cbAttendancehour.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void btnhostel_Click(object sender, EventArgs e)
    {
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string typeoftotal = "";
            if (cbltypeoftotal.Items.Count > 0)
            {
                for (int i = 0; i < cbltypeoftotal.Items.Count; i++)
                {
                    if (cbltypeoftotal.Items[i].Selected == true)
                    {
                        if (typeoftotal == "")
                        {
                            typeoftotal = Convert.ToString(cbltypeoftotal.Items[i].Value);
                        }
                        else
                        {
                            typeoftotal = typeoftotal + "," + Convert.ToString(cbltypeoftotal.Items[i].Value);
                        }
                    }
                }
            }
            string daily_consum = "";
            if (daily_consumption.Items.Count > 0)
            {
                for (int i = 0; i < daily_consumption.Items.Count; i++)
                {
                    if (daily_consumption.Items[i].Selected == true)
                    {
                        if (daily_consum == "")
                        {
                            daily_consum = Convert.ToString(daily_consumption.Items[i].Text);
                        }
                        else
                        {
                            daily_consum = daily_consum + "," + Convert.ToString(daily_consumption.Items[i].Text);
                        }
                    }
                }
            }

            string mess_attendance = "";
            if (cbl_messattendance.Items.Count > 0)
            {
                for (int i = 0; i < cbl_messattendance.Items.Count; i++)
                {
                    if (cbl_messattendance.Items[i].Selected == true)
                    {
                        if (mess_attendance == "")
                        {
                            mess_attendance = Convert.ToString(cbl_messattendance.Items[i].Text);
                        }
                        else
                        {
                            mess_attendance = mess_attendance + "," + Convert.ToString(cbl_messattendance.Items[i].Text);
                        }
                    }
                }
            }
            string Use_Attendance = "";
            string Attendance_value = "";

            if (cbstudentAttendancehour.Checked == true)
            {
                Use_Attendance = "1";
                if (ddl_hour.SelectedItem.Text != "Select")
                {
                    Attendance_value = ddl_hour.SelectedItem.Text;
                }
                else
                {
                    Attendance_value = "";
                }
            }
            else
            {
                Use_Attendance = "0";
                Attendance_value = " ";
            }


            string staff_Attendance = "";
            if (cbstaffbiomarric.Checked == true)
            {
                staff_Attendance = "1";
            }
            else
            {
                staff_Attendance = "0";
            }

            bool newcheckvalue = false;
            string firstdate = Convert.ToString(txtfromdate1.Text);
            string secondate = Convert.ToString(txttodate1.Text);
            DateTime dn = new DateTime();
            DateTime dt1 = new DateTime();
            string[] split = firstdate.Split('/');
            dn = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = secondate.Split('/');
            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            byte AllStudentAttendance = 0;
            if (cbAllstudentAttendance.Checked)
                AllStudentAttendance = 1;

            if (staff_Attendance.Trim() != "" || Use_Attendance.Trim() != "" || Attendance_value.Trim() != "" || typeoftotal.Trim() != "")
            {
                if (chklsthostel1.Items.Count > 0)
                {
                    for (int row = 0; row < chklsthostel1.Items.Count; row++)
                    {
                        if (chklsthostel1.Items[row].Selected == true)
                        {
                            if (chklstsession1.Items.Count > 0)
                            {
                                for (int row1 = 0; row1 < chklstsession1.Items.Count; row1++)
                                {
                                    if (chklstsession1.Items[row1].Selected == true)
                                    {
                                        if (rdodatewise1.Checked == true)
                                        {
                                            string type = "0";
                                            DateTime dt = dn;
                                            while (dt <= dt1)
                                            {
                                                string insertupdatequery = "if exists (select * from HostelIns_settings where Hostel_code='" + Convert.ToString(chklsthostel1.Items[row].Value) + "' and Session_code='" + Convert.ToString(chklstsession1.Items[row1].Value) + "' and Schedule_type='" + type + "' and Schedule_date='" + dt.ToString("MM/dd/yyyy") + "') update HostelIns_settings set EditMenuTotal='" + typeoftotal + "' ,Use_Attendance='" + Use_Attendance + "',Att_Hour ='" + Attendance_value + "' ,Staff_total ='" + staff_Attendance + "',daily_consumption='" + daily_consum + "',Mess_attendance_set='" + mess_attendance + "',AllStudentAttendance='" + AllStudentAttendance + "' where Hostel_code='" + Convert.ToString(chklsthostel1.Items[row].Value) + "' and Session_code='" + Convert.ToString(chklstsession1.Items[row1].Value) + "' and Schedule_type='" + type + "' and Schedule_date='" + dt.ToString("MM/dd/yyyy") + "' else insert into HostelIns_settings (Hostel_code,Session_code,Schedule_type,Schedule_Day,Schedule_date,EditMenuTotal,Use_Attendance,Att_Hour,Staff_total,daily_consumption,Mess_attendance_set,AllStudentAttendance) values ('" + Convert.ToString(chklsthostel1.Items[row].Value) + "','" + Convert.ToString(chklstsession1.Items[row1].Value) + "','" + type + "','" + dt.ToString("dddd") + "','" + dt.ToString("MM/dd/yyyy") + "','" + typeoftotal + "','" + Use_Attendance + "','" + Attendance_value + "','" + staff_Attendance + "','" + daily_consum + "','" + mess_attendance + "','" + AllStudentAttendance + "')";
                                                int insert = d2.update_method_wo_parameter(insertupdatequery, "Text");
                                                if (insert != 0)
                                                {
                                                    newcheckvalue = true;
                                                }
                                                dt = dt.AddDays(1);
                                            }
                                        }
                                        if (rdodaywise1.Checked == true)
                                        {
                                            string type = "1";
                                            if (chklstdaycompar.Items.Count > 0)
                                            {
                                                for (int r = 0; r < chklstdaycompar.Items.Count; r++)
                                                {
                                                    if (chklstdaycompar.Items[r].Selected == true)
                                                    {
                                                        string insertupdatequery = "if exists (select * from HostelIns_settings where Hostel_code='" + Convert.ToString(chklsthostel1.Items[row].Value) + "' and Session_code='" + Convert.ToString(chklstsession1.Items[row1].Value) + "'and Schedule_type='" + type + "' and Schedule_Day='" + Convert.ToString(chklstdaycompar.Items[r].Text) + "') update HostelIns_settings set EditMenuTotal='" + typeoftotal + "' ,Use_Attendance='" + Use_Attendance + "',Att_Hour ='" + Attendance_value + "' ,Staff_total ='" + staff_Attendance + "',daily_consumption='" + daily_consum + "',Mess_attendance_set='" + mess_attendance + "',AllStudentAttendance='" + AllStudentAttendance + "' where Hostel_code='" + Convert.ToString(chklsthostel1.Items[row].Value) + "' and Session_code='" + Convert.ToString(chklstsession1.Items[row1].Value) + "' and Schedule_type='" + type + "' and Schedule_Day='" + Convert.ToString(chklstdaycompar.Items[r].Text) + "' else insert into HostelIns_settings (Hostel_code,Session_code,Schedule_type,Schedule_Day,EditMenuTotal,Use_Attendance,Att_Hour,Staff_total,daily_consumption,Mess_attendance_set,AllStudentAttendance) values ('" + Convert.ToString(chklsthostel1.Items[row].Value) + "','" + Convert.ToString(chklstsession1.Items[row1].Value) + "','" + type + "','" + Convert.ToString(chklstdaycompar.Items[r].Text) + "','" + typeoftotal + "','" + Use_Attendance + "','" + Attendance_value + "','" + staff_Attendance + "','" + daily_consum + "','" + mess_attendance + "','" + AllStudentAttendance + "')";
                                                        int insert = d2.update_method_wo_parameter(insertupdatequery, "Text");
                                                        if (insert != 0)
                                                        {
                                                            newcheckvalue = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (newcheckvalue == true)
                {
                    divPopper.Visible = false;
                    imgdiv2.Visible = true;
                    btngo_Click(sender, e);
                    lbl_alert.Text = "Saved Successfully";
                    clear();
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please Select the Day";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Select Any one Items";
            }
        }
        catch
        {

        }
    }
    protected void btn_update_Click(object sender, EventArgs e)
    {
        try
        {
            string typeoftotal = "";
            if (cbltypeoftotal.Items.Count > 0)
            {
                for (int i = 0; i < cbltypeoftotal.Items.Count; i++)
                {
                    if (cbltypeoftotal.Items[i].Selected == true)
                    {
                        if (typeoftotal == "")
                        {
                            typeoftotal = Convert.ToString(cbltypeoftotal.Items[i].Text);
                        }
                        else
                        {
                            typeoftotal = typeoftotal + "," + Convert.ToString(cbltypeoftotal.Items[i].Text);
                        }
                    }
                }
            }
            //27.10.15
            string daily_consum = "";
            if (daily_consumption.Items.Count > 0)
            {
                for (int i = 0; i < daily_consumption.Items.Count; i++)
                {
                    if (daily_consumption.Items[i].Selected == true)
                    {
                        if (daily_consum == "")
                        {
                            daily_consum = Convert.ToString(daily_consumption.Items[i].Text);
                        }
                        else
                        {
                            daily_consum = daily_consum + "," + Convert.ToString(daily_consumption.Items[i].Text);
                        }
                    }
                }
            }
            string mess_attendance = "";
            if (cbl_messattendance.Items.Count > 0)
            {
                for (int i = 0; i < cbl_messattendance.Items.Count; i++)
                {
                    if (cbl_messattendance.Items[i].Selected == true)
                    {
                        if (mess_attendance == "")
                        {
                            mess_attendance = Convert.ToString(cbl_messattendance.Items[i].Text);
                        }
                        else
                        {
                            mess_attendance = mess_attendance + "," + Convert.ToString(cbl_messattendance.Items[i].Text);
                        }
                    }
                }
            }
            //
            string Use_Attendance = "";
            string Attendance_value = "";
            //if (ddl_hour.SelectedItem.Text.Trim() != "Select")
            //{
            //    Use_Attendance = "1";
            //    Attendance_value = ddl_hour.SelectedItem.Text;
            //}
            //else
            //{
            //    Use_Attendance = "0";
            //    if (ddl_hour.Enabled == false)
            //    {
            //        Attendance_value = "";
            //    }
            //}

            if (cbstudentAttendancehour.Checked == true)
            {
                Use_Attendance = "1";
                if (ddl_hour.SelectedItem.Text != "Select")
                {
                    Attendance_value = ddl_hour.SelectedItem.Text;
                }
                else
                {
                    Attendance_value = "";
                }
            }
            else
            {
                Use_Attendance = "0";
                Attendance_value = " ";
            }

            string staff_Attendance = "";
            if (cbstaffbiomarric.Checked == true)
            {
                staff_Attendance = "1";
            }
            else
            {
                staff_Attendance = "0";
            }

            bool newcheckvalue = false;
            string firstdate = Convert.ToString(txtfromdate1.Text);
            string secondate = Convert.ToString(txttodate1.Text);
            DateTime dn = new DateTime();
            DateTime dt1 = new DateTime();
            string[] split = firstdate.Split('/');
            dn = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = secondate.Split('/');
            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            byte AllStudentAttendance = 0;
            if (cbAllstudentAttendance.Checked)
                AllStudentAttendance = 1;

            if (staff_Attendance.Trim() != "" || Use_Attendance.Trim() != "" || Attendance_value.Trim() != "" || typeoftotal.Trim() != "")
            {
                if (chklsthostel1.Items.Count > 0)
                {
                    for (int row = 0; row < chklsthostel1.Items.Count; row++)
                    {
                        if (chklsthostel1.Items[row].Selected == true)
                        {
                            if (chklstsession1.Items.Count > 0)
                            {
                                for (int row1 = 0; row1 < chklstsession1.Items.Count; row1++)
                                {
                                    if (chklstsession1.Items[row1].Selected == true)
                                    {
                                        if (rdodatewise1.Checked == true)
                                        {
                                            string type = "0";
                                            DateTime dt = dn;
                                            while (dt <= dt1)
                                            {
                                                string update = "update HostelIns_settings set EditMenuTotal='" + typeoftotal + "' ,Use_Attendance='" + Use_Attendance + "',Att_Hour ='" + Attendance_value + "' ,Staff_total ='" + staff_Attendance + "',daily_consumption='" + daily_consum + "',Mess_attendance_set='" + mess_attendance + "',AllStudentAttendance='" + AllStudentAttendance + "' where Hostel_code='" + Convert.ToString(chklsthostel1.Items[row].Value) + "' and Session_code='" + Convert.ToString(chklstsession1.Items[row1].Value) + "' and Schedule_type='" + type + "' and Schedule_date='" + dn.ToString("MM/dd/yyyy") + "' ";
                                                int insert = d2.update_method_wo_parameter(update, "Text");
                                                if (insert != 0)
                                                {
                                                    newcheckvalue = true;
                                                }
                                                dt = dt.AddDays(1);
                                            }
                                        }
                                        else if (rdodaywise1.Checked == true)
                                        {
                                            string type = "1";
                                            if (chklstdaycompar.Items.Count > 0)
                                            {
                                                for (int r = 0; r < chklstdaycompar.Items.Count; r++)
                                                {
                                                    if (chklstdaycompar.Items[r].Selected == true)
                                                    {
                                                        string update = "update HostelIns_settings set EditMenuTotal='" + typeoftotal + "' ,Use_Attendance='" + Use_Attendance + "',Att_Hour ='" + Attendance_value + "' ,Staff_total ='" + staff_Attendance + "',daily_consumption='" + daily_consum + "',Mess_attendance_set='" + mess_attendance + "',AllStudentAttendance='" + AllStudentAttendance + "' where Hostel_code='" + Convert.ToString(chklsthostel1.Items[row].Value) + "' and Session_code='" + Convert.ToString(chklstsession1.Items[row1].Value) + "' and Schedule_type='" + type + "' and Schedule_Day='" + Convert.ToString(chklstdaycompar.Items[r].Text) + "'";
                                                        int insert = d2.update_method_wo_parameter(update, "Text");
                                                        if (insert != 0)
                                                        {
                                                            newcheckvalue = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (newcheckvalue == true)
                {
                    divPopper.Visible = false;
                    imgdiv2.Visible = true;
                    btngo_Click(sender, e);
                    lbl_alert.Text = "Updated Successfully";
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please Select the Day";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Select Any one Items";
            }
        }
        catch
        {

        }
    }
    public void delete()
    {
        try
        {
            surediv.Visible = false;
            string del = "";

            string firstdate = Convert.ToString(txtfromdate1.Text);
            string secondate = Convert.ToString(txttodate1.Text);

            DateTime dn = new DateTime();
            string[] split = firstdate.Split('/');
            dn = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = secondate.Split('/');

            bool newcheckvalue = false;
            if (chklsthostel1.Items.Count > 0)
            {
                for (int row = 0; row < chklsthostel1.Items.Count; row++)
                {
                    if (chklsthostel1.Items[row].Selected == true)
                    {
                        if (chklstsession1.Items.Count > 0)
                        {
                            for (int row1 = 0; row1 < chklstsession1.Items.Count; row1++)
                            {
                                if (chklstsession1.Items[row1].Selected == true)
                                {
                                    if (rdodatewise1.Checked == true)
                                    {
                                        string type = "0";
                                        del = "delete from HostelIns_settings where Hostel_code='" + Convert.ToString(chklsthostel1.Items[row].Value) + "' and Session_code='" + Convert.ToString(chklstsession1.Items[row1].Value) + "' and Schedule_type='" + type + "' and Schedule_date='" + dn.ToString("MM/dd/yyyy") + "'";
                                        int y = d2.update_method_wo_parameter(del, "Text");
                                        if (y != 0)
                                        {
                                            newcheckvalue = true;
                                        }
                                    }
                                    else if (rdodaywise1.Checked == true)
                                    {
                                        string type = "1";
                                        if (chklstdaycompar.Items.Count > 0)
                                        {
                                            for (int r = 0; r < chklstdaycompar.Items.Count; r++)
                                            {
                                                if (chklstdaycompar.Items[r].Selected == true)
                                                {
                                                    del = "delete from HostelIns_settings where Hostel_code='" + Convert.ToString(chklsthostel1.Items[row].Value) + "' and Session_code='" + Convert.ToString(chklstsession1.Items[row1].Value) + "' and Schedule_type='" + type + "' and Schedule_Day='" + Convert.ToString(chklstdaycompar.Items[r].Text) + "'";
                                                    int y = d2.update_method_wo_parameter(del, "Text");
                                                    if (y != 0)
                                                    {
                                                        newcheckvalue = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (newcheckvalue == true)
            {
                ds.Clear();
                loadhostel();
                btngo_Click(sender, e);
                divPopper.Visible = false;
                imgdiv2.Visible = true;
                //lbl_alerterror.Text = "Deleted Successfully";
                lbl_alert.Text = "Deleted Successfully";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_delete_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_delete.Text == "Delete")
            {
                surediv.Visible = true;
                lbl_sure.Text = "Do you want to delete this record?";
            }
        }
        catch
        {
        }
    }
    protected void btnexit1_Click(object sender, EventArgs e)
    {
        divPopper.Visible = false;
    }

    // both

    public void Bindhour()
    {
        try
        {
            string qeryss = "select max(No_of_hrs_per_day) as No_of_hrs_per_day from PeriodAttndSchedule";
            ds11 = dasri.select_method_wo_parameter(qeryss, "Text");
            if (ds11.Tables[0].Rows.Count > 0)
            {
                int noofhour = Convert.ToInt16(ds11.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString());
                for (int i = 1; i <= noofhour; i++)
                {
                    cblAttendancehour.Items.Add(i.ToString());
                }
                if (cbAttendancehour.Checked == true)
                {
                    for (int i = 0; i < cblAttendancehour.Items.Count; i++)
                    {
                        cblAttendancehour.Items[i].Selected = true;
                        txt_Attendancehour.Text = "Hours(" + (cblAttendancehour.Items.Count) + ")";
                    }
                }
                else
                {
                    for (int i = 0; i < cblAttendancehour.Items.Count; i++)
                    {
                        cblAttendancehour.Items[i].Selected = false;
                        txt_Attendancehour.Text = "---Select---";
                    }
                }
            }
            else
            {
                cblAttendancehour.Items.Clear();
                txt_Attendancehour.Text = "---Select---";
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void loadhour()
    {
        try
        {
            ddl_hour.Items.Clear();
            ds.Clear();
            string sql = "select max(No_of_hrs_per_day) as No_of_hrs_per_day from PeriodAttndSchedule";
            ds = dasri.select_method_wo_parameter(sql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                //ddl_hour.DataSource = ds;
                //ddl_hour.DataTextField = "No_of_hrs_per_day";
                //ddl_hour.DataValueField = "No_of_hrs_per_day";
                //ddl_hour.DataBind();
                //ddl_hour.Items.Insert(0, "Select");
                int noofhour = Convert.ToInt16(ds.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString());
                ddl_hour.Items.Insert(0, "Select");
                for (int i = 1; i <= noofhour; i++)
                {
                    ddl_hour.Items.Add(i.ToString());
                }
            }
            else
            {
                ddl_hour.Items.Insert(0, "Select");
            }

            //for (int i = 1; i <= 12; i++)
            //{
            //    ddlhour.Items.Add(Convert.ToString(i));
            //    ddlendhour.Items.Add(Convert.ToString(i));
            //    ddlexhour.Items.Add(Convert.ToString(i));
            //    ddlhour.SelectedIndex = ddlhour.Items.Count - 1;
            //    ddlendhour.SelectedIndex = ddlendhour.Items.Count - 1;
            //    ddlexhour.SelectedIndex = ddlexhour.Items.Count - 1;
            //}
        }
        catch
        {
        }
    }
    public void loadsecond()
    {
        for (int i = 0; i <= 60; i++)
        {
            string value = Convert.ToString(i);
            if (value.Length == 1)
            {
                value = "0" + "" + value;
            }
            //ddlseconds.Items.Add(Convert.ToString(value));
            //ddlendsecnonds.Items.Add(Convert.ToString(value));
            //ddlexseconds.Items.Add(Convert.ToString(value));
        }
    }
    public void loadminits()
    {
        for (int i = 0; i <= 60; i++)
        {
            string value = Convert.ToString(i);
            if (value.Length == 1)
            {
                value = "0" + "" + value;
            }
            //ddlminits.Items.Add(Convert.ToString(value));
            //ddlendminit.Items.Add(Convert.ToString(value));
            //ddlexminitus.Items.Add(Convert.ToString(value));
        }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
        //surediv.Visible = false;
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        imgdiv2.Visible = false;
        divPopper.Visible = true;
    }
    public void clear()
    {
        divPopper.Visible = true;
        btnsave.Visible = true;
        btn_update.Visible = false;
        btn_delete.Visible = false;
        loadhostel1();
        loadsession1();
        loadhour();
        txtfromdate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txttodate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtdaycompar.Text = "--Select--";
        //poperrjs.Visible = true;
        //  rdodatewise1.Checked = true;
        // rdodaywise1.Checked = false;
        rdodatewise1_CheckedChanged(sender, e);
        rdodaywise1_CheckedChanged(sender, e);
        chkdaycompar.Checked = false;
        cbstudentAttendancehour.Checked = false;
        cbstudentAttendancehour_Change(sender, e);
        txt_Attendancehour.Text = "Select";
        cbstaffbiomarric.Checked = false;

        for (int i = 0; i < cbltypeoftotal.Items.Count; i++)
        {
            cbltypeoftotal.Items[i].Selected = false;
        }
        for (int i = 0; i < daily_consumption.Items.Count; i++)
        {
            daily_consumption.Items[i].Selected = false;
        }
    }
    protected void btnplus_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addgroup.Visible = true;
        lbl_addgroup.Text = "Menu Purpose Category";
        lblerror.Visible = false;

    }
    protected void btnminus_Click(object sender, EventArgs e)
    {
        if (ddl_group.SelectedIndex != 0)
        {
            string sql = "delete from CO_MasterValues where MasterCode='" + ddl_group.SelectedItem.Value.ToString() + "' and MasterCriteria='Menu Purpose Category' and collegecode='" + collegecode1 + "'";
            int delete = d2.update_method_wo_parameter(sql, "Text");
            if (delete != 0)
            {

                imgdiv2.Visible = true;
                lbl_alert.Text = "Deleted Successfully";
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "No Record Selected";
            }
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No Record Selected";
        }
        bindaddgroup();
    }
    protected void btn_addgroup_Click(object sender, EventArgs e)
    {
        try
        {
            string group = Convert.ToString(txt_addgroup.Text);
            group = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(group);
            if (lbl_addgroup.Text == "Menu Purpose Category")
            {
                if (txt_addgroup.Text != "")
                {
                    //magesh 19.3.18
                    //string sql = "if exists ( select * from CO_MasterValues where MasterValue='" + group + "' and MasterCriteria='Menu Purpose Category' and CollegeCode='" + collegecode1 + "') Update CO_MasterValues set MasterValue='" + group + "' where MasterValue='" + group + "' and MasterCriteria='Menu Purpose Category' and CollegeCode='" + collegecode1 + "' else insert into sdf (MasterValue,MasterCriteria,CollegeCode) values('" + group + "','Menu Purpose Category','" + collegecode1 + "')";

                    string sql = "if exists ( select * from CO_MasterValues where MasterValue='" + group + "' and MasterCriteria='Menu Purpose Category' and CollegeCode='" + collegecode1 + "') Update CO_MasterValues set MasterValue='" + group + "' where MasterValue='" + group + "' and MasterCriteria='Menu Purpose Category' and CollegeCode='" + collegecode1 + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values('" + group + "','Menu Purpose Category','" + collegecode1 + "')";
                    int insert = d2.update_method_wo_parameter(sql, "Text");
                    if (insert != 0)
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Saved Successfully";
                        txt_addgroup.Text = "";
                        plusdiv.Visible = false;
                        panel_addgroup.Visible = false;
                    }
                    bindaddgroup();
                    txt_addgroup.Text = "";
                }
                else
                {
                    plusdiv.Visible = true;
                    lblerror.Visible = true;
                    lblerror.Text = "Enter the Catagory";
                }
            }
        }
        catch
        {
        }
    }
    protected void btn_exitaddgroup_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = false;
        panel_addgroup.Visible = false;
        txt_addgroup.Text = "";
    }
    protected void bindaddgroup()
    {
        try
        {
            ddl_group.Items.Clear();
            ds.Clear();
            string sql = "select distinct MasterCode,MasterValue  from CO_MasterValues where MasterCriteria ='Menu Purpose Category' and CollegeCode ='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_group.DataSource = ds;
                ddl_group.DataTextField = "MasterValue";
                ddl_group.DataValueField = "MasterCode";
                ddl_group.DataBind();
                ddl_group.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddl_group.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch
        { }
    }
}