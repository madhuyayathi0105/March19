using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using Gios.Pdf;
using System.IO;
using System.Net;
using System.Globalization;
using InsproDataAccess;
using System.Configuration;

public partial class ExaminvigilatorReport : System.Web.UI.Page
{
    InsproDirectAccess dir = new InsproDirectAccess();
    Hashtable hat = new Hashtable();
    DAccess2 d2 = new DAccess2();
    bool cellfalsg = false;
    string group_user = string.Empty;
    string singleuser = string.Empty;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string session_var = string.Empty;
    static int commcnt = 0;
    bool flag_true = false;
    DataSet dsdate = new DataSet();
    string exammonth;
    string exmayear;
    bool cellClicked = false;
    int staffCnt = 0;
    ArrayList Alstaff = new ArrayList();
    DataSet ds = new DataSet();
    int actColumn = 0;
    int selectedRow = 0;
    string selectedStaffCount = string.Empty;
    string selecteddate = string.Empty;
    string selectedsession = string.Empty;
    string selectedstaff = string.Empty;
    string staffval = string.Empty;
    string date2 = string.Empty;
    string selectedroom = string.Empty;
    string sel_date = string.Empty;
    string closecommand = string.Empty;
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("CoeHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/CoeMod/CoeHome.aspx");
                    return;
                }
            }
            //****************************************************//rr1.Visible = false;
            if (!Page.IsPostBack)
            {
                singleuser = Session["single_user"].ToString();
                group_user = Session["group_code"].ToString();
                collegecode = Session["collegecode"].ToString();
                usercode = Session["usercode"].ToString();
                lblstaffperstu.Visible = false;
                txtstaffperstu.Visible = false;
                btngenerate.Visible = false;
                btnletter.Visible = false;
                lblattdinationstaff.Visible = false;
                txtaddtionalstafff.Visible = false;
                chkheadimage.Visible = false;
                DropDownList1.Items.Clear();
                DropDownList1.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                DropDownList1.Items.Insert(1, new System.Web.UI.WebControls.ListItem("F.N", "1"));
                DropDownList1.Items.Insert(2, new System.Web.UI.WebControls.ListItem("A.N", "2"));
                DropDownList1.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Both", "3"));
                DateTime now = DateTime.Now;
                txtfromdate.Text = now.Date.ToString("dd/MM/yyyy");
                txttodate.Text = now.Date.ToString("dd/MM/yyyy");
                rbstaff.Checked = true;
                txtfromdate.Enabled = false;
                txttodate.Enabled = false;
                divDeleteStaff.Visible = false;
                year();
                month1();
                type();
                loadstaff();
                clear();
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void loadedate()
    {
        try
        {
            ddledate.Items.Clear();
            DateTime dtf = DateTime.Now;
            DateTime dtt = DateTime.Now;
            string hol = "select * from examholiday where exammonth='" + ddlMonth.SelectedValue.ToString() + "' and examyear='" + ddlYear.SelectedValue.ToString() + "'";
            DataSet ds = d2.select_method_wo_parameter(hol, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                dtf = Convert.ToDateTime(ds.Tables[0].Rows[0]["startdate"].ToString());
                dtt = Convert.ToDateTime(ds.Tables[0].Rows[0]["enddate"].ToString());
            }
            Hashtable hatdate = new Hashtable();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DateTime dthol = Convert.ToDateTime(ds.Tables[0].Rows[i]["holiday_date"].ToString());
                if (!hatdate.Contains(dthol.ToString("MM/dd/yyyy")))
                {
                    hatdate.Add(dthol.ToString("MM/dd/yyyy"), dthol.ToString("MM/dd/yyyy"));
                }
            }
            for (DateTime dt = dtf; dt <= dtt; dt = dt.AddDays(1))
            {
                if (!hatdate.Contains(dt.ToString("MM/dd/yyyy")))
                {
                    ddledate.Items.Insert(0, new System.Web.UI.WebControls.ListItem(dt.ToString("dd/MM/yyyy"), dt.ToString("MM/dd/yyyy")));
                }
            }
            ddlesession.Items.Clear();
            string gethour = "select distinct exam_session,RIGHT(CONVERT(VARCHAR, et.start_time, 100),7)+'@'+RIGHT(CONVERT(VARCHAR, et.start_time, 100),7) as timeval from exmtt e,exmtt_det et where e.exam_code=et.exam_code and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' ";
            DataSet dsedate = d2.select_method_wo_parameter(gethour, "text");
            ddlesession.DataSource = dsedate;
            ddlesession.DataTextField = "exam_session";
            ddlesession.DataValueField = "timeval";
            ddlesession.DataBind();
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    public void type()
    {
        try
        {
            string strtypequery = "select distinct type from course where isnull(type,'')<>''";
            DataSet ds = d2.select_method_wo_parameter(strtypequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddltype.DataSource = ds;
                ddltype.DataTextField = "type";
                ddltype.DataValueField = "type";
                ddltype.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    public void year()
    {
        DataSet ds = d2.Examyear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlYear.DataSource = ds;
            ddlYear.DataTextField = "Exam_year";
            ddlYear.DataValueField = "Exam_year";
            ddlYear.DataBind();
        }
        ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
    }

    protected void month1()
    {
        try
        {
            string year1 = ddlYear.SelectedValue;
            DataSet ds = d2.Exammonth(year1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlMonth.DataSource = ds;
                ddlMonth.DataTextField = "monthName";
                ddlMonth.DataValueField = "Exam_month";
                ddlMonth.DataBind();
            }
            ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void Pdep_CheckedChanged(object sender, EventArgs e)
    {
        lblerr1.Visible = false;
        lblexcelname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        btnPrint.Visible = false;
        AttSpread.Visible = false;
        AttSpread.Visible = false;
        if (cbdepselectall.Checked == true)
        {
            foreach (System.Web.UI.WebControls.ListItem li in Chkdep.Items)
            {
                li.Selected = true;
                tbdep.Text = "Hall No(" + (Chkdep.Items.Count) + ")";
            }
        }
        else
        {
            foreach (System.Web.UI.WebControls.ListItem li in Chkdep.Items)
            {
                li.Selected = false;
                tbdep.Text = "- - Select - -";
            }
        }
    }

    protected void Pdep_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        int commcount = 0;
        string value = string.Empty;
        string code = string.Empty;
        for (int i = 0; i < Chkdep.Items.Count; i++)
        {
            if (Chkdep.Items[i].Selected == true)
            {
                value = Chkdep.Items[i].Text;
                code = Chkdep.Items[i].Value.ToString();
                commcount = commcount + 1;
                tbdep.Text = "Hall No(" + commcount.ToString() + ")";
            }
        }
        cbdepselectall.Checked = false;
        if (commcount == 0)
        {
            tbdep.Text = "- - All - -";
        }
        else
        {
        }
        commcnt = commcount;
    }

    public void Bindhallno()
    {
        try
        {
            string strtypeval = string.Empty;
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                strtypeval = " and c.type='" + ddltype.SelectedItem.ToString() + "'";
            }
            string months = ddlMonth.SelectedValue.ToString();
            string years = ddlYear.SelectedValue.ToString();
            string fdate = txtfromdate.Text.ToString();
            string[] spf = fdate.Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            string tdate = txtfromdate.Text.ToString();
            string[] spt = fdate.Split('/');
            DateTime dtt = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            Chkdep.Items.Clear();
            string sedd = string.Empty;
            if (DropDownList1.SelectedItem.Text == "Both")
            {
                sedd = string.Empty;
            }
            else
            {
                sedd = "and ses_sion='" + DropDownList1.SelectedItem.Text + "'";
            }
            string getdeteails = "SELECT distinct roomno FROM exam_seating e,Registration r,Degree d,course c where e.regno=r.Reg_No and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id " + strtypeval + " and edate between '" + dtf + "' and '" + dtt + "' " + sedd + "";
            DataSet dssem = d2.select_method_wo_parameter(getdeteails, "Text");
            int count5 = 0;
            if (dssem.Tables.Count > 0 && dssem.Tables[0].Rows.Count > 0)
            {
                count5 = dssem.Tables[0].Rows.Count;
                if (count5 > 0)
                {
                    Chkdep.DataSource = dssem;
                    Chkdep.DataTextField = "roomno";
                    Chkdep.DataValueField = "roomno";
                    Chkdep.DataBind();
                }
                else
                {
                    Chkdep.Items.Clear();
                    tbdep.Text = "- - All - -";
                }
            }
            else
            {
                Chkdep.Items.Clear();
                tbdep.Text = "- - All - -";
            }
            if (count5 > 0)
            {
                cbdepselectall.Checked = true;
                for (int i = 0; i < Chkdep.Items.Count; i++)
                {
                    Chkdep.Items[i].Selected = true;
                    tbdep.Text = "Hall No(" + Chkdep.Items.Count + ")";
                }
            }
            // Chkdep.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    public void clear()
    {
        lblerr1.Visible = false;
        lblexcelname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        btnPrint.Visible = false;
        lblDate.Visible = false;
        AttSpread.Visible = false;
        AttSpread.Visible = false;
        lbledate.Visible = false;
        ddledate.Visible = false;
        lblesession.Visible = false;
        ddlesession.Visible = false;
        btnsave.Visible = false;
        btndelete.Visible = false;
        lblstaff.Visible = false;
        ddlstaff.Visible = false;
        btnletter.Visible = false;
        chkheadimage.Visible = false;
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        loadedate();
        loadstaff();
    }

    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        loadedate();
        loadstaff();
        Bindhallno();
    }

    protected void ddlfrmdate_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        Bindhallno();
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        month1();
    }

    protected void ddlexamtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        clear();
        string fdate = txtfromdate.Text.ToString();
        string[] spf = fdate.Split('/');
        DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
        string tdate = txtfromdate.Text.ToString();
        string[] spt = fdate.Split('/');
        DateTime dtt = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
        if (dtt < dtf)
        {
            lblerr1.Visible = true;
            lblerr1.Text = "From Date Must Be Lesser than Todate";
            return;
        }
        Bindhallno();
    }

    protected void txtfromdate_TextChanged(object sender, EventArgs e)
    {
        clear();
        string fdate = txtfromdate.Text.ToString();
        string[] spf = fdate.Split('/');
        DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
        string tdate = txtfromdate.Text.ToString();
        string[] spt = fdate.Split('/');
        DateTime dtt = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
        if (dtt < dtf)
        {
            lblerr1.Visible = true;
            lblerr1.Text = "From Date Must Be Lesser than Todate";
            return;
        }
        Bindhallno();
    }

    protected void reportchange(object sender, EventArgs e)
    {
        clear();
        txtfromdate.Enabled = false;
        txttodate.Enabled = false;
        if (rbdate.Checked == true)
        {
            txtfromdate.Enabled = true;
            txttodate.Enabled = true;
        }
    }

    /* public void Datewisereport()
     {
         try
         {
             clear();
             if (DropDownList1.SelectedValue.ToString() == "" || DropDownList1.SelectedIndex == 0)
             {
                 lblerr1.Visible = true;
                 lblerr1.Text = "Please Select Session";
                 return;
             }
             string fdate = txtfromdate.Text.ToString();
             string[] spf = fdate.Split('/');
             DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
             string tdate = txttodate.Text.ToString();
             string[] spt = tdate.Split('/');
             DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);
             if (dtt < dtf)
             {
                 lblerr1.Visible = true;
                 lblerr1.Text = "From Date Must Be Lesser than Todate";
                 return;
             }
             string departnt = string.Empty;
             int ledgercount = 0;
             for (int f = 0; f < Chkdep.Items.Count; f++)
             {
                 if (Chkdep.Items[f].Selected == true)
                 {
                     ledgercount = ledgercount + 1;
                     if (departnt == "")
                     {
                         departnt = "'" + Chkdep.Items[f].Value.ToString() + "'";
                     }
                     else
                     {
                         departnt = departnt + ",'" + Chkdep.Items[f].Value.ToString() + "'";
                     }
                 }
             }
             if (departnt.Trim() != "")
             {
                 departnt = " and es.roomno in (" + departnt + ")";
             }
             lblDate.Visible = false;
             session_var = DropDownList1.SelectedItem.Text;
             Session["session_var"] = session_var;
             string ff = ddlYear.SelectedValue.ToString();
             AttSpread.Visible = false;
             AttSpread.Sheets[0].RowCount = 0;
             AttSpread.Sheets[0].ColumnCount = 0;
             AttSpread.Sheets[0].ColumnCount = 8;
             AttSpread.Sheets[0].RowHeader.Visible = false;
             AttSpread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
             MyStyle.Font.Size = FontUnit.Medium;
             MyStyle.Font.Name = "Book Antiqua";
             MyStyle.Font.Bold = true;
             MyStyle.HorizontalAlign = HorizontalAlign.Center;
             MyStyle.ForeColor = Color.Black;
             MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
             AttSpread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
             AttSpread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
             AttSpread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
             AttSpread.Sheets[0].DefaultStyle.Font.Bold = false;
             AttSpread.Sheets[0].Columns[0].Width = 50;
             AttSpread.Sheets[0].Columns[1].Width = 100;
             AttSpread.Sheets[0].Columns[2].Width = 100;
             AttSpread.Sheets[0].Columns[3].Width = 100;
             AttSpread.Sheets[0].Columns[4].Width = 100;
             AttSpread.Sheets[0].Columns[5].Width = 50;
             AttSpread.Sheets[0].Columns[6].Width = 50;
             AttSpread.Sheets[0].Columns[7].Width = 400;
             AttSpread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
             AttSpread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
             AttSpread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
             AttSpread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
             AttSpread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
             AttSpread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
             AttSpread.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
             AttSpread.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
             AttSpread.Sheets[0].Columns[5].Visible = true;
             AttSpread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
             AttSpread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
             AttSpread.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
             AttSpread.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
             AttSpread.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
             AttSpread.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
             AttSpread.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
             AttSpread.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
             AttSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
             AttSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Room Name";
             AttSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
             AttSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Session";
             AttSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Room Strength";
             AttSpread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Student Strength";
             AttSpread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Staff Strength";
             AttSpread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Staff Name";
             AttSpread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
             AttSpread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
             AttSpread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
             AttSpread.Sheets[0].AutoPostBack = true;
             AttSpread.CommandBar.Visible = false;
             string collgr = Session["collegecode"].ToString();
             string sess = string.Empty;
             if (DropDownList1.SelectedItem.Text == "Both")
             {
                 sess = string.Empty;
             }
             else
             {
                 sess = "  and et.exam_session='" + DropDownList1.SelectedItem.Text + "'";
             }
             string strtypeval = string.Empty;
             if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
             {
                 if (ddltype.SelectedItem.ToString().Trim().ToLower() == "day")
                 {
                     strtypeval = " and c.type in('Day','MCA')";
                 }
                 else
                 {
                     strtypeval = " and c.type='" + ddltype.SelectedItem.ToString() + "'";
                 }
             }
             string spreadbind1 = "select et.exam_date,et.exam_session,es.roomno,count(es.regno) stustren,Convert(nvarchar(15),et.exam_date,103) as edate  from exmtt e,exmtt_det et,exam_seating es,Degree d,Course c where e.exam_code=et.exam_code and et.exam_date=es.edate and et.exam_session=es.ses_sion and et.subject_no=es.subject_no and e.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' " + departnt + " and et.exam_date between '" + dtf + "' and '" + dtt + "' " + sess + " " + strtypeval + " group by et.exam_date,et.exam_session,es.roomno order by et.exam_date ,et.exam_session desc,es.roomno";
             spreadbind1 = spreadbind1 + " select distinct sm.staff_code,ev.invigilator_code,sm.staff_name,h.dept_name,ev.roomno,ev.edate,ev.ses_sion,ev.month,ev.year,ev.invigilator_name from examinvigilatormaster ev,staffmaster sm,stafftrans st,hrdept_master h  where ev.invigilator_code=sm.staff_code and sm.staff_code=st.staff_code and st.dept_code=h.dept_code and st.latestrec=1; select * from tbl_room_seats";
             DataSet ds2 = d2.select_method_wo_parameter(spreadbind1, "Text");
             string strength = string.Empty;
             string roomno = string.Empty;
             string sesson = string.Empty;
             FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
             DataView dvfilterinvi = new DataView();
             int sno = 0;
             int height = 45;
             if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
             {
                 lbledate.Visible = false;
                 ddledate.Visible = false;
                 lblesession.Visible = false;
                 ddlesession.Visible = false;
                 btnsave.Visible = false;
                 btndelete.Visible = false;
                 lblstaff.Visible = false;
                 ddlstaff.Visible = false;
                 int totroomseat = 0;
                 int totstudentseat = 0;
                 int invilaorcount = 0;
                 Hashtable hatroom = new Hashtable();
                 for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                 {
                     AttSpread.Sheets[0].RowCount = AttSpread.Sheets[0].RowCount + 1;
                     sno++;
                     height = height + AttSpread.Sheets[0].Rows[i].Height;
                     roomno = ds2.Tables[0].Rows[i]["roomno"].ToString();
                     strength = ds2.Tables[0].Rows[i]["stustren"].ToString();
                     sesson = ds2.Tables[0].Rows[i]["exam_session"].ToString();
                     string exdate = ds2.Tables[0].Rows[i]["edate"].ToString();
                     DateTime dtva = Convert.ToDateTime(ds2.Tables[0].Rows[i]["exam_date"].ToString());
                     if (!hatroom.Contains(exdate + '-' + sesson))
                     {
                         if (i > 0)
                         {
                             AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Text = "Total";
                             AttSpread.Sheets[0].Rows[AttSpread.Sheets[0].RowCount - 1].BackColor = Color.LightGray;
                             AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                             AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                             AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                             AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Text = totroomseat.ToString();
                             AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                             AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                             AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Large;
                             AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].Text = totstudentseat.ToString();
                             AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                             AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                             AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Large;
                             AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Text = invilaorcount.ToString();
                             AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                             AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                             AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Large;
                             AttSpread.Sheets[0].SpanModel.Add(AttSpread.Sheets[0].RowCount - 1, 0, 1, 4);
                             AttSpread.Sheets[0].RowCount = AttSpread.Sheets[0].RowCount + 1;
                         }
                         totroomseat = 0;
                         totstudentseat = 0;
                         invilaorcount = 0;
                         hatroom.Add(exdate + '-' + sesson, exdate + '-' + sesson);
                     }
                     AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].CellType = txt;
                     AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 1].CellType = txt;
                     AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].CellType = txt;
                     AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].CellType = txt;
                     AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].CellType = txt;
                     AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].CellType = txt;
                     AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].CellType = txt;
                     AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Text = sno + "";
                     AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].Text = roomno;
                     AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].Note = roomno;
                     AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 1].Text = exdate;
                     AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Note = exdate;
                     AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Text = sesson;
                     AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Note = sesson;
                     AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].Text = strength;
                     totstudentseat = totstudentseat + Convert.ToInt32(strength);
                     string rommsetr = string.Empty;
                     ds2.Tables[2].DefaultView.RowFilter = " hall_no ='" + roomno + "'";
                     DataView dvromm = ds2.Tables[2].DefaultView;
                     if (dvromm.Count > 0)
                     {
                         rommsetr = dvromm[0]["allocted_seats"].ToString();
                     }
                     AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Text = rommsetr;
                     totroomseat = totroomseat + Convert.ToInt32(rommsetr);
                     string staffname = string.Empty;
                     string staffcode = string.Empty;
                     ds2.Tables[1].DefaultView.RowFilter = " roomno ='" + roomno + "' and edate='" + dtva + "'  and ses_sion='" + sesson + "' and month='" + ddlMonth.SelectedValue.ToString() + "' and year='" + ddlYear.SelectedValue.ToString() + "'";
                     dvfilterinvi = ds2.Tables[1].DefaultView;
                     if (dvfilterinvi.Count > 0)
                     {
                         for (int es = 0; es < dvfilterinvi.Count; es++)
                         {
                             if (staffname == "")
                             {
                                 staffname = dvfilterinvi[es]["dept_name"].ToString() + " - " + dvfilterinvi[es]["invigilator_name"].ToString();
                                 staffcode = dvfilterinvi[es]["invigilator_code"].ToString();
                             }
                             else
                             {
                                 staffname = staffname + ", " + dvfilterinvi[es]["dept_name"].ToString() + " - " + dvfilterinvi[es]["invigilator_name"].ToString();
                                 staffcode = staffcode + ", " + dvfilterinvi[es]["invigilator_code"].ToString();
                             }
                         }
                         AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 7].Text = staffname;
                         AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Text = dvfilterinvi.Count.ToString();
                         AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Tag = staffcode;
                         invilaorcount = invilaorcount + dvfilterinvi.Count;
                     }
                     else
                     {
                         AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 7].Text = "-";
                         AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Text = "-";
                         AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Tag = staffcode;
                     }
                     if (i == ds2.Tables[0].Rows.Count - 1)
                     {
                         AttSpread.Sheets[0].RowCount = AttSpread.Sheets[0].RowCount + 1;
                         AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Text = "Total";
                         AttSpread.Sheets[0].Rows[AttSpread.Sheets[0].RowCount - 1].BackColor = Color.LightGray;
                         AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                         AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                         AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                         AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Text = totroomseat.ToString();
                         AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                         AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                         AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Large;
                         AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].Text = totstudentseat.ToString();
                         AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                         AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                         AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Large;
                         AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Text = invilaorcount.ToString();
                         AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                         AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                         AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Large;
                         AttSpread.Sheets[0].SpanModel.Add(AttSpread.Sheets[0].RowCount - 1, 0, 1, 4);
                     }
                 }
                 if (height > 600)
                 {
                     AttSpread.Height = 400;
                 }
                 else if (height > 500)
                 {
                     AttSpread.Height = height - 200;
                 }
                 else if (height > 400)
                 {
                     AttSpread.Height = height - 100;
                 }
                 else
                 {
                     AttSpread.Height = height;
                 }
                 AttSpread.SaveChanges();
                 AttSpread.Visible = true;
                 lblexcelname.Visible = true;
                 txtexcelname.Visible = true;
                 btnExcel.Visible = true;
                 btnPrint.Visible = true;
                 AttSpread.Sheets[0].PageSize = AttSpread.Sheets[0].RowCount;
             }
             else
             {
                 lblerr1.Visible = true;
                 lblerr1.Text = "No Records Found";
             }
             AttSpread.Sheets[0].PageSize = AttSpread.Sheets[0].RowCount;
             AttSpread.Width = 1000;
             Double heighva = 20;
             if (AttSpread.Sheets[0].RowCount > 500)
             {
                 heighva = 1000;
             }
             else
             {
                 heighva = AttSpread.Sheets[0].RowCount * 20 + 40;
             }
             heighva = Math.Round(heighva, 0, MidpointRounding.AwayFromZero);
             heighva = heighva + 20;
             AttSpread.Height = Convert.ToInt32(heighva);
             AttSpread.SaveChanges();
         }
         catch (Exception ex)
         {
             lblerr1.Visible = true;
             lblerr1.Text = ex.ToString();
         }
     }*/

    public void Datewisereport()
    {
        try
        {
            clear();
            if (DropDownList1.SelectedValue.ToString() == "" || DropDownList1.SelectedIndex == 0)
            {
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Session";
                return;
            }
            string fdate = txtfromdate.Text.ToString();
            string[] spf = fdate.Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            string tdate = txttodate.Text.ToString();
            string[] spt = tdate.Split('/');
            DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);
            if (dtt < dtf)
            {
                lblerr1.Visible = true;
                lblerr1.Text = "From Date Must Be Lesser than Todate";
                return;
            }
            string departnt = string.Empty;
            int ledgercount = 0;
            for (int f = 0; f < Chkdep.Items.Count; f++)
            {
                if (Chkdep.Items[f].Selected == true)
                {
                    ledgercount = ledgercount + 1;
                    if (departnt == "")
                    {
                        departnt = "'" + Chkdep.Items[f].Value.ToString() + "'";
                    }
                    else
                    {
                        departnt = departnt + ",'" + Chkdep.Items[f].Value.ToString() + "'";
                    }
                }
            }
            if (departnt.Trim() != "")
            {
                departnt = " and es.roomno in (" + departnt + ")";
            }
            lblDate.Visible = false;
            session_var = DropDownList1.SelectedItem.Text;
            Session["session_var"] = session_var;
            string ff = ddlYear.SelectedValue.ToString();
            AttSpread.Visible = false;
            AttSpread.Sheets[0].RowCount = 0;
            AttSpread.Sheets[0].ColumnCount = 0;
            AttSpread.Sheets[0].ColumnCount = 11;
            AttSpread.Sheets[0].RowHeader.Visible = false;
            AttSpread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.Font.Bold = true;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            AttSpread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            AttSpread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            AttSpread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            AttSpread.Sheets[0].DefaultStyle.Font.Bold = false;
            AttSpread.Sheets[0].Columns[0].Width = 50;
            AttSpread.Sheets[0].Columns[1].Width = 100;
            AttSpread.Sheets[0].Columns[2].Width = 100;
            AttSpread.Sheets[0].Columns[3].Width = 100;
            AttSpread.Sheets[0].Columns[4].Width = 100;
            AttSpread.Sheets[0].Columns[5].Width = 50;
            AttSpread.Sheets[0].Columns[6].Width = 50;
            AttSpread.Sheets[0].Columns[7].Width = 260;
            AttSpread.Sheets[0].Columns[8].Width =  50;
            AttSpread.Sheets[0].Columns[9].Width =  50;
            AttSpread.Sheets[0].Columns[10].Width = 60;

            AttSpread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            AttSpread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            AttSpread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            AttSpread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            AttSpread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            AttSpread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            AttSpread.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            AttSpread.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
            AttSpread.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
            AttSpread.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
            AttSpread.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Center;

            AttSpread.Sheets[0].Columns[8].Visible = true;
            AttSpread.Sheets[0].Columns[9].Visible = true;
            AttSpread.Sheets[0].Columns[5].Visible = true;
            AttSpread.Sheets[0].Columns[10].Visible = true;

            AttSpread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            AttSpread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            AttSpread.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            AttSpread.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            AttSpread.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            AttSpread.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
            AttSpread.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
            AttSpread.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
            AttSpread.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
            AttSpread.Sheets[0].Columns[9].VerticalAlign = VerticalAlign.Middle;
            AttSpread.Sheets[0].Columns[10].VerticalAlign = VerticalAlign.Middle;

            AttSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Room Name";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Session";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Room Strength";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Student Strength";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Staff Strength";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Staff Name";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Add";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Move";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Delete";

            AttSpread.Sheets[0].Columns[0].Locked = true;
            AttSpread.Sheets[0].Columns[1].Locked = true;
            AttSpread.Sheets[0].Columns[2].Locked = true;
            AttSpread.Sheets[0].Columns[3].Locked = true;
            AttSpread.Sheets[0].Columns[4].Locked = true;
            AttSpread.Sheets[0].Columns[5].Locked = true;
            AttSpread.Sheets[0].Columns[6].Locked = true;
            AttSpread.Sheets[0].Columns[7].Locked = true;
            AttSpread.Sheets[0].Columns[8].Locked = false;
            AttSpread.Sheets[0].Columns[9].Locked = false;
            AttSpread.Sheets[0].Columns[10].Locked = false;

            AttSpread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            AttSpread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            AttSpread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            AttSpread.Sheets[0].AutoPostBack = false;
            AttSpread.CommandBar.Visible = false;

            string collgr = Session["collegecode"].ToString();
            string sess = string.Empty;
            if (DropDownList1.SelectedItem.Text == "Both")
            {
                sess = string.Empty;
            }
            else
            {
                sess = "  and et.exam_session='" + DropDownList1.SelectedItem.Text + "'";
            }
            string strtypeval = string.Empty;
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.ToString().Trim().ToLower() == "day")
                {
                    strtypeval = " and c.type in('Day','MCA')";
                }
                else
                {
                    strtypeval = " and c.type='" + ddltype.SelectedItem.ToString() + "'";
                }
            }

            string spreadbind1 = "select et.exam_date,et.exam_session,es.roomno,count(distinct es.regno) stustren,Convert(nvarchar(15),et.exam_date,103) as edate from exmtt e,exmtt_det et,exam_seating es,Degree d,Course c where e.exam_code=et.exam_code and et.exam_date=es.edate and et.exam_session=es.ses_sion and et.subject_no=es.subject_no and e.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' " + departnt + " and et.exam_date between '" + dtf + "' and '" + dtt + "' " + sess + " " + strtypeval + " group by et.exam_date,et.exam_session,es.roomno order by et.exam_date ,et.exam_session desc,es.roomno";
            spreadbind1 = spreadbind1 + " select distinct sm.staff_code,ev.invigilator_code,sm.staff_name,h.dept_name,ev.roomno,ev.edate,ev.ses_sion,ev.month,ev.year,ev.invigilator_name from examinvigilatormaster ev,staffmaster sm,stafftrans st,hrdept_master h  where ev.invigilator_code=sm.staff_code and sm.staff_code=st.staff_code and st.dept_code=h.dept_code and st.latestrec=1; select * from tbl_room_seats";
            DataSet ds2 = d2.select_method_wo_parameter(spreadbind1, "Text");
            string strength = string.Empty;
            string roomno = string.Empty;
            string sesson = string.Empty;


            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.ButtonCellType btn = new FarPoint.Web.Spread.ButtonCellType();
            btn.Text = "Add";
            btn.CommandName = "addstaff";

            FarPoint.Web.Spread.ButtonCellType btn2 = new FarPoint.Web.Spread.ButtonCellType();
            btn2.Text = "Move";
            btn2.CommandName = "movestaff";

            FarPoint.Web.Spread.ButtonCellType btn3 = new FarPoint.Web.Spread.ButtonCellType();
            btn3.Text = "Delete";
            btn3.CommandName = "deletestaff";

            DataView dvfilterinvi = new DataView();
            int sno = 0;
            int height = 45;
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                lbledate.Visible = false;
                ddledate.Visible = false;
                lblesession.Visible = false;
                ddlesession.Visible = false;
                btnsave.Visible = false;
                btndelete.Visible = false;
                lblstaff.Visible = false;
                ddlstaff.Visible = false;
                int totroomseat = 0;
                int totstudentseat = 0;
                int invilaorcount = 0;
                Hashtable hatroom = new Hashtable();
                for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                {
                    AttSpread.Sheets[0].RowCount = AttSpread.Sheets[0].RowCount + 1;
                    sno++;
                    height = height + AttSpread.Sheets[0].Rows[i].Height;
                    roomno = ds2.Tables[0].Rows[i]["roomno"].ToString();
                    strength = ds2.Tables[0].Rows[i]["stustren"].ToString();
                    sesson = ds2.Tables[0].Rows[i]["exam_session"].ToString();
                    string exdate = ds2.Tables[0].Rows[i]["edate"].ToString();
                    DateTime dtva = Convert.ToDateTime(ds2.Tables[0].Rows[i]["exam_date"].ToString());
                    if (!hatroom.Contains(exdate + '-' + sesson))
                    {
                        if (i > 0)
                        {
                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Text = "Total";
                            AttSpread.Sheets[0].Rows[AttSpread.Sheets[0].RowCount - 1].BackColor = Color.LightGray;
                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Text = totroomseat.ToString();
                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Large;
                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].Text = totstudentseat.ToString();
                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Large;
                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Text = invilaorcount.ToString();
                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Large;

                            AttSpread.Sheets[0].SpanModel.Add(AttSpread.Sheets[0].RowCount - 1, 0, 1, 4);
                            AttSpread.Sheets[0].RowCount = AttSpread.Sheets[0].RowCount + 1;
                        }

                        totroomseat = 0;
                        totstudentseat = 0;
                        invilaorcount = 0;
                        hatroom.Add(exdate + '-' + sesson, exdate + '-' + sesson);
                    }
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].CellType = txt;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 1].CellType = txt;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].CellType = txt;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].CellType = txt;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].CellType = txt;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].CellType = txt;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].CellType = txt;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 8].CellType = btn;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 9].CellType = btn2;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 10].CellType = btn3;

                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Text = sno + "";
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].Text = roomno;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].Note = roomno;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 1].Text = exdate;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Note = exdate;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Text = sesson;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Note = sesson;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].Text = strength;

                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 8].Text = "Add";
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 9].Text = "Move";

                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Locked = true;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 1].Locked = true;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Locked = true;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].Locked = true;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Locked = true;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].Locked = true;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Locked = true;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 7].Locked = true;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 8].Locked = false;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 9].Locked = false;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 10].Locked = false;

                    totstudentseat = totstudentseat + Convert.ToInt32(strength);
                    string rommsetr = string.Empty;
                    ds2.Tables[2].DefaultView.RowFilter = " hall_no ='" + roomno + "'";
                    DataView dvromm = ds2.Tables[2].DefaultView;
                    if (dvromm.Count > 0)
                    {
                        rommsetr = dvromm[0]["allocted_seats"].ToString();
                    }
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Text = rommsetr;
                    totroomseat = totroomseat + Convert.ToInt32(rommsetr);
                    string staffname = string.Empty;
                    string staffcode = string.Empty;
                    ds2.Tables[1].DefaultView.RowFilter = " roomno ='" + roomno + "' and edate='" + dtva + "'  and ses_sion='" + sesson + "' and month='" + ddlMonth.SelectedValue.ToString() + "' and year='" + ddlYear.SelectedValue.ToString() + "'";
                    dvfilterinvi = ds2.Tables[1].DefaultView;
                    if (dvfilterinvi.Count > 0)
                    {
                        for (int es = 0; es < dvfilterinvi.Count; es++)
                        {
                            if (staffname == "")
                            {
                                staffname = dvfilterinvi[es]["dept_name"].ToString() + " - " + dvfilterinvi[es]["invigilator_name"].ToString();
                                staffcode = dvfilterinvi[es]["invigilator_code"].ToString();
                            }
                            else
                            {
                                staffname = staffname + "," + dvfilterinvi[es]["dept_name"].ToString() + " - " + dvfilterinvi[es]["invigilator_name"].ToString();
                                staffcode = staffcode + "," + dvfilterinvi[es]["invigilator_code"].ToString();
                            }
                        }
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 7].Text = staffname;
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Text = dvfilterinvi.Count.ToString();
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Tag = staffcode;
                        invilaorcount = invilaorcount + dvfilterinvi.Count;
                    }
                    else
                    {
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 7].Text = "-";
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Text = "-";
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Tag = staffcode;
                    }
                    if (i == ds2.Tables[0].Rows.Count - 1)
                    {
                        AttSpread.Sheets[0].RowCount = AttSpread.Sheets[0].RowCount + 1;
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Text = "Total";
                        AttSpread.Sheets[0].Rows[AttSpread.Sheets[0].RowCount - 1].BackColor = Color.LightGray;
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Text = totroomseat.ToString();
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Large;
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].Text = totstudentseat.ToString();
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Large;
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Text = invilaorcount.ToString();
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Large;

                        AttSpread.Sheets[0].SpanModel.Add(AttSpread.Sheets[0].RowCount - 1, 0, 1, 4);
                    }
                }
                if (height > 600)
                {
                    AttSpread.Height = 400;
                }
                else if (height > 500)
                {
                    AttSpread.Height = height - 200;
                }
                else if (height > 400)
                {
                    AttSpread.Height = height - 100;
                }
                else
                {
                    AttSpread.Height = height;
                }
                AttSpread.SaveChanges();
                AttSpread.Visible = true;
                lblexcelname.Visible = true;
                txtexcelname.Visible = true;
                btnExcel.Visible = true;
                btnPrint.Visible = true;
                AttSpread.Sheets[0].PageSize = AttSpread.Sheets[0].RowCount;
            }
            else
            {
                lblerr1.Visible = true;
                lblerr1.Text = "No Records Found";
            }
            AttSpread.Sheets[0].PageSize = AttSpread.Sheets[0].RowCount;
            AttSpread.Width = 1000;
            Double heighva = 20;
            if (AttSpread.Sheets[0].RowCount > 500)
            {
                heighva = 1000;
            }
            else
            {
                heighva = AttSpread.Sheets[0].RowCount * 20 + 40;
            }
            heighva = Math.Round(heighva, 0, MidpointRounding.AwayFromZero);
            heighva = heighva + 20;
            AttSpread.Height = Convert.ToInt32(heighva);
            AttSpread.SaveChanges();
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    public void staffwiserepot()
    {
        try
        {
            clear();
            lbledate.Visible = false;
            ddledate.Visible = false;
            lblesession.Visible = false;
            ddlesession.Visible = false;
            btnsave.Visible = false;
            btndelete.Visible = false;
            lblstaff.Visible = false;
            ddlstaff.Visible = false;
            lblDate.Visible = false;
            btnSendSMS.Visible = false;
            AttSpread.Visible = false;
            AttSpread.Sheets[0].RowCount = 0;
            AttSpread.Sheets[0].ColumnCount = 7;
            AttSpread.Sheets[0].RowHeader.Visible = false;
            AttSpread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.Font.Bold = true;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            AttSpread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            AttSpread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            AttSpread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            AttSpread.Sheets[0].DefaultStyle.Font.Bold = false;
            AttSpread.Sheets[0].Columns[0].Width = 50;
            AttSpread.Sheets[0].Columns[1].Width = 300;
            AttSpread.Sheets[0].Columns[2].Width = 100;
            AttSpread.Sheets[0].Columns[3].Width = 150;
            AttSpread.Sheets[0].Columns[4].Width = 100;
            AttSpread.Sheets[0].Columns[5].Width = 200;
            AttSpread.Sheets[0].Columns[6].Width = 50;
            AttSpread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            AttSpread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            AttSpread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            AttSpread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            AttSpread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            AttSpread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
            AttSpread.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            AttSpread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            AttSpread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            AttSpread.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            AttSpread.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            AttSpread.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            AttSpread.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
            AttSpread.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Name";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Code";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "No Of Inivilazion";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Date - Session";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "select";
            AttSpread.Sheets[0].Columns[6].Locked = false;
            AttSpread.Sheets[0].AutoPostBack = false;
            AttSpread.CommandBar.Visible = false;
            string collgr = Session["collegecode"].ToString();
            string fdate = txtfromdate.Text.ToString();
            string[] spf = fdate.Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            string tdate = txtfromdate.Text.ToString();
            string[] spt = fdate.Split('/');
            DateTime dtt = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            string sess = string.Empty;
            if (DropDownList1.SelectedItem.Text == "Both")
            {
                sess = string.Empty;
            }
            else
            {
                sess = "  and es.ses_sion='" + DropDownList1.SelectedItem.Text + "'";
            }
            string strtypeval = string.Empty;
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                strtypeval = " and c.type='" + ddltype.SelectedItem.ToString() + "'";
            }
            string spreadbind1 = "select sm.staff_name,sm.staff_code,h.dept_name,Convert(nvarchar(15),sm.join_date,103) as joindas,count(ev.edate) as noofse from examinvigilatormaster ev,staffmaster sm,stafftrans st,hrdept_master h,examstaffmaster c where ev.invigilator_code=sm.staff_code and sm.staff_code=st.staff_code and c.staff_code=ev.invigilator_code and st.dept_code=h.dept_code and st.latestrec=1 " + strtypeval + " and ev.month='" + ddlMonth.SelectedValue.ToString() + "' and ev.year='" + ddlYear.SelectedValue.ToString() + "' group by h.dept_name,sm.staff_name,sm.staff_code,sm.join_date order by  h.dept_name,noofse desc;";
            spreadbind1 = spreadbind1 + " select sm.staff_name,sm.staff_code,h.dept_name,Convert(nvarchar(15),ev.edate,103) edate,ev.ses_sion,ev.roomno,ev.edate from examinvigilatormaster ev,staffmaster sm,stafftrans st,hrdept_master h,examstaffmaster c where ev.invigilator_code=sm.staff_code and sm.staff_code=st.staff_code and c.staff_code=ev.invigilator_code and st.dept_code=h.dept_code  and Inivigition='1' and st.latestrec=1 " + strtypeval + " and ev.month='" + ddlMonth.SelectedValue.ToString() + "' and ev.year='" + ddlYear.SelectedValue.ToString() + "' order by ev.edate,ev.ses_sion desc ";
            DataSet ds2 = d2.select_method_wo_parameter(spreadbind1, "Text");
            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
            chkall.AutoPostBack = true;
            AttSpread.Sheets[0].RowCount = AttSpread.Sheets[0].RowCount + 1;
            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].CellType = chkall;
            AttSpread.Sheets[0].SpanModel.Add(AttSpread.Sheets[0].RowCount - 1, 0, 1, 6);
            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
            DataView dvfilterinvi = new DataView();
            int sno = 0;
            int height = 45;
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                btnletter.Visible = true; chkheadimage.Visible = true;
                for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    AttSpread.Sheets[0].RowCount++;
                    string staffname = ds2.Tables[0].Rows[i]["staff_name"].ToString();
                    string staffcode = ds2.Tables[0].Rows[i]["staff_code"].ToString();
                    string department = ds2.Tables[0].Rows[i]["dept_name"].ToString();
                    string joinda = ds2.Tables[0].Rows[i]["joindas"].ToString();
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 1].Text = staffname.ToString();
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Text = staffcode.ToString();
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].Text = department.ToString();
                    string datetime = string.Empty;
                    ds2.Tables[1].DefaultView.RowFilter = "staff_code='" + staffcode + "'";
                    DataView dvstraff = ds2.Tables[1].DefaultView;
                    for (int se = 0; se < dvstraff.Count; se++)
                    {
                        if (datetime == "")
                        {
                            datetime = dvstraff[se]["edate"].ToString() + " - " + dvstraff[se]["ses_sion"].ToString();
                        }
                        else
                        {
                            datetime = datetime + ", " + dvstraff[se]["edate"].ToString() + " - " + dvstraff[se]["ses_sion"].ToString();
                        }
                    }
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Text = dvstraff.Count.ToString();
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].Text = datetime;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].CellType = chk;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Locked = false;
                    height = height + AttSpread.Sheets[0].Rows[i].Height;
                }
                AttSpread.Sheets[0].Columns[5].Visible = true;
                if (height > 600)
                {
                    AttSpread.Height = 400;
                }
                else if (height > 500)
                {
                    AttSpread.Height = height - 200;
                }
                else if (height > 400)
                {
                    AttSpread.Height = height - 100;
                }
                else
                {
                    AttSpread.Height = height;
                }
                AttSpread.SaveChanges();
                AttSpread.Visible = true;
                lblexcelname.Visible = true;
                txtexcelname.Visible = true;
                btnExcel.Visible = true;
                btnPrint.Visible = true;
                btnSendSMS.Visible = true;
                AttSpread.Sheets[0].PageSize = AttSpread.Sheets[0].RowCount;
            }
            else
            {
                lblerr1.Visible = true;
                lblerr1.Text = "No Records Found";
            }
            AttSpread.Sheets[0].PageSize = AttSpread.Sheets[0].RowCount;
            AttSpread.Width = 1000;
            Double heighva = 20;
            if (AttSpread.Sheets[0].RowCount > 500)
            {
                heighva = 1000;
            }
            else
            {
                heighva = AttSpread.Sheets[0].RowCount * 20 + 40;
            }
            heighva = Math.Round(heighva, 0, MidpointRounding.AwayFromZero);
            heighva = heighva + 20;
            AttSpread.Height = Convert.ToInt32(heighva);
            AttSpread.Width = 1000;
            AttSpread.SaveChanges();
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (ddlMonth.SelectedValue.ToString() == "" || ddlMonth.SelectedIndex == 0)
            {
                lblerr1.Text = "Please Select Month";
                lblerr1.Visible = true;
                return;
            }
            if (ddlYear.SelectedValue.ToString() == "" || ddlYear.SelectedIndex == 0)
            {
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Year";
                return;
            }
            if (rbstaff.Checked == true)
            {
                staffwiserepot();
            }
            if (rbdate.Checked == true)
            {
                Datewisereport();
            }
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        Bindhallno();
    }

    protected void btngenerate_Click(object sender, EventArgs e)
    {
        try
        {
            bool getflag = false;
            exmayear = string.Empty;
            exammonth = string.Empty;
            if (ddlYear.Items.Count > 0)
            {
                exmayear = Convert.ToString(ddlYear.SelectedValue).Trim();
            }
            else
            {
                lblerr1.Visible = true;
                lblerr1.Text = "No Exam Year Found";
                return;
            }
            if (ddlMonth.Items.Count > 0)
            {
                exammonth = Convert.ToString(ddlMonth.SelectedValue).Trim();
            }
            else
            {
                lblerr1.Visible = true;
                lblerr1.Text = "No Exam Month Found";
                return;
            }
            //exmayear = ddlYear.SelectedValue.ToString();
            //exammonth = ddlMonth.SelectedValue.ToString();
            string strtypeval = string.Empty;
            if (exmayear.Trim() == "0" || exmayear.Trim() == "")
            {
                lblerr1.Visible = true;
                lblerr1.Text = "Please Enter The Exam Year";
                return;
            }
            if (exammonth.Trim() == "0" || exammonth.Trim() == "0")
            {
                lblerr1.Visible = true;
                lblerr1.Text = "Please Enter The Exam Month ";
                return;
            }
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.ToString().Trim().ToLower() == "day")
                {
                    strtypeval = " and c.type in('Day','MCA')";
                }
                else
                {
                    strtypeval = " and c.type='" + ddltype.SelectedItem.ToString() + "'";
                }
                //strtypeval = " and c.type='" + ddltype.SelectedItem.ToString() + "'";
            }
            string stupercount = txtstaffperstu.Text.ToString();
            if (stupercount.Trim() == "")
            {
                lblerr1.Visible = true;
                lblerr1.Text = "Please Enter Staff Per Student ";
                return;
            }
            int staffperstaff = 0;// Convert.ToInt32(stupercount);
            int.TryParse(stupercount.Trim(), out staffperstaff);
            if (staffperstaff == 0)
            {
                lblerr1.Visible = true;
                lblerr1.Text = "Please Enter Staff Per Student Value Greater Than ZERO";
                return;
            }
            int noaddstaff = 0;
            string straddstaf = txtaddtionalstafff.Text.ToString();
            if (straddstaf.Trim() != "")
            {
                //noaddstaff = Convert.ToInt32(straddstaf);
                int.TryParse(straddstaf.Trim(), out noaddstaff);
                if (noaddstaff == 0)
                {
                    lblerr1.Visible = true;
                    lblerr1.Text = "Please Enter Addtional Staff Value Greater Than ZERO";
                    return;
                }
            }
            string strdatetwisestrengthcount = "select et.exam_date,et.exam_session,es.roomno,count(es.regno) stustren from exmtt e,exmtt_det et,exam_seating es,Degree d,Course c where e.exam_code=et.exam_code and et.subject_no=es.subject_no and e.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and edate =exam_date and e.Exam_year='" + exmayear + "' and e.Exam_month='" + exammonth + "' " + strtypeval + " group by et.exam_date,et.exam_session,es.roomno order by et.exam_date ,et.exam_session desc,es.roomno";
            DataSet dsexamdaterommcount = d2.select_method_wo_parameter(strdatetwisestrengthcount, "Text");
            string striniviligationstaffquery = "select c.staff_code,sm.staff_name,sm.join_date,sa.experience_info,year(join_date) as y  from examstaffmaster c,staffmaster sm,staff_appl_master sa  where sm.appl_no=sa.appl_no and sm.staff_code=c.staff_code and c.Inivigition='1' " + strtypeval + " order by sm.join_date desc,c.staff_code";
            DataSet dsinivistaff = d2.select_method_wo_parameter(striniviligationstaffquery, "Text");
            string strhallsupsett = "select * from hallsupervision c where c.max_superivison>0  " + strtypeval + " order by expfrom desc";
            DataSet dshallsupset = d2.select_method_wo_parameter(strhallsupsett, "Text");
            string strdayequery = "select min(et.exam_date) as stardate,max(et.exam_date ) endate from exmtt e,exmtt_det et,exam_seating es,Degree d,Course c where e.exam_code=et.exam_code and et.subject_no=es.subject_no and e.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and e.Exam_year='" + exmayear + "' and e.Exam_month='" + exammonth + "' " + strtypeval + "";
            dsdate = d2.select_method_wo_parameter(strdayequery, "Text");
            string delquey = "delete ev from examinvigilatormaster ev,examstaffmaster c where ev.invigilator_code=c.staff_code and ev.month='" + exammonth + "' and ev.year='" + exmayear + "' " + strtypeval + "";
            int del = d2.update_method_wo_parameter(delquey, "text");
            Hashtable hatstaffexp = new Hashtable();
            Hashtable hatstaffinvicount = new Hashtable();
            Hashtable hatstaffinviglesession = new Hashtable();
            Hashtable hatstaffcompletinvi = new Hashtable();
            int alstaff = 0;
            int neestaff = 0;
            string roomno = string.Empty;
            int val = 0;
            string[] ascDesc = new string[2] { "asc", "desc" };
            if (dsdate.Tables.Count > 0 && dsdate.Tables[0].Rows.Count > 0)
            {
                DateTime dtfrom = Convert.ToDateTime(dsdate.Tables[0].Rows[0]["stardate"].ToString());
                DateTime dtendate = Convert.ToDateTime(dsdate.Tables[0].Rows[0]["endate"].ToString());
                int NCount = 0;
                //int ascCount = 0;
                //int descCount = 0;
                int countLoop = 0;
                for (DateTime dt = dtfrom; dt <= dtendate; dt = dt.AddDays(1))
                {
                    string sortBy = " asc ";
                    if (countLoop % 15 == 0 && countLoop != 0)
                    {
                        sortBy = " " + ascDesc[0];
                    }
                    else
                    {
                        sortBy = " " + ascDesc[1];
                    }
                    Hashtable htIsStaAdedtoday = new Hashtable();//added by Idhris 13-02-2017
                    Hashtable htIsStaAdedtodaySession = new Hashtable();//added by Malang 7-04-2017
                    for (int se = 0; se < 2; se++)
                    {
                        Hashtable hatstaffdatesession = new Hashtable();
                        string sesval = "F.N";
                        if (se > 0)
                        {
                            sesval = "A.N";
                        }
                        bool isCheckOnce = false;
                        dsexamdaterommcount.Tables[0].DefaultView.RowFilter = "exam_date='" + dt.ToString("MM/dd/yyyy") + "' and exam_session='" + sesval + "' ";
                        DataView dvexamdateromster = dsexamdaterommcount.Tables[0].DefaultView;
                        if (dvexamdateromster.Count > 0)
                        {
                            if (se == 0)
                            {
                                val++;
                            }
                            for (int i = 0; i < dvexamdateromster.Count; i++)
                            {
                                roomno = dvexamdateromster[i]["roomno"].ToString();
                                int stustrenth = 0;// Convert.ToInt32(dvexamdateromster[i]["stustren"].ToString());
                                int.TryParse(Convert.ToString(dvexamdateromster[i]["stustren"]).Trim(), out stustrenth);
                                if (stustrenth > 0)
                                {

                                    neestaff = stustrenth / staffperstaff;
                                    Double remreg = (stustrenth % staffperstaff);
                                    remreg = Math.Round(remreg, 1, MidpointRounding.AwayFromZero);
                                    //if ((staffperstaff / 2) <= remreg)
                                    //{
                                    //    neestaff++;
                                    //}
                                    #region Idhris

                                    if (remreg > 0.0)
                                    {
                                        neestaff++;
                                    }

                                    #endregion

                                    string staffCompleted = string.Empty;
                                    if (hatstaffcompletinvi.Count > 0)
                                    {
                                        string[] arrLIst = new string[hatstaffcompletinvi.Count];
                                        hatstaffcompletinvi.Keys.CopyTo(arrLIst, 0);
                                        staffCompleted = "'" + string.Join("','", arrLIst) + "'";
                                        if (arrLIst.Length > 0)
                                        {
                                            staffCompleted = "staff_code not in(" + staffCompleted + ")";
                                        }
                                        else
                                        {
                                            staffCompleted = string.Empty;
                                        }
                                    }
                                    alstaff = 0;
                                    dsinivistaff.Tables[0].DefaultView.RowFilter = staffCompleted;
                                    DataView dvstafflsi = dsinivistaff.Tables[0].DefaultView;
                                    //if (val == 1 && se == 0)
                                    //{
                                    //    dvstafflsi.Sort = "join_date asc";
                                    //}
                                    //else if ((val % 2) == 0)
                                    //{
                                    //    dvstafflsi.Sort = "y desc,staff_code";
                                    //}
                                    //else if ((val % 2) == 1)
                                    //{
                                    //    dvstafflsi.Sort = "y desc,staff_code desc";
                                    //}
                                    #region Idhris 26-10-2016
                                    //int halfCount = (dvstafflsi.Count / 2);
                                    //int remCount = (dvstafflsi.Count % 2);
                                    //halfCount += remCount;
                                    //if (halfCount <= NCount)
                                    //{
                                    //    if (se == 0)
                                    //    {
                                    //        ascCount = 0;
                                    //    }
                                    //    else
                                    //    {
                                    //        descCount = 0;
                                    //    }
                                    //}
                                    //Label Next;
                                    while (alstaff < neestaff)
                                    {
                                        if (countLoop % 15 == 0 && countLoop != 0)
                                        {
                                            sortBy = " " + ascDesc[0];
                                        }
                                        else
                                        {
                                            sortBy = " " + ascDesc[1];
                                        }
                                        dvstafflsi.Sort = "join_date " + sortBy;
                                        //if (se == 0)
                                        //{
                                        //    dvstafflsi.Sort = "join_date asc";
                                        //    // NCount = ascCount;
                                        //}
                                        //else
                                        //{
                                        //    dvstafflsi.Sort = "join_date desc";
                                        //    //NCount = descCount;
                                        //}
                                        if (NCount >= dvstafflsi.Count)
                                        {
                                            NCount = 0;
                                        }
                                        bool checkSession = false;
                                        if (htIsStaAdedtoday.Count >= dvstafflsi.Count)
                                        {
                                            if (htIsStaAdedtodaySession.Count >= dvstafflsi.Count)
                                            {
                                                if (isCheckOnce)
                                                {
                                                    break;
                                                }
                                                else
                                                {

                                                }
                                                //checkSession = false;
                                                //break;
                                            }
                                            else
                                            {
                                                checkSession = true;
                                            }
                                        }
                                    #endregion
                                        for (int st = NCount; st < dvstafflsi.Count; st++)
                                        {
                                            if (NCount >= dvstafflsi.Count)
                                            {
                                                st = NCount = 0;
                                            }

                                            //if (halfCount <= NCount)
                                            //{
                                            //    if (se == 0)
                                            //    {
                                            //        ascCount = 0;
                                            //        NCount = ascCount;
                                            //    }
                                            //    else
                                            //    {
                                            //        descCount = 0;
                                            //        NCount = descCount;
                                            //    }
                                            //    st = NCount;
                                            //}
                                            if (htIsStaAdedtoday.Count >= dvstafflsi.Count)
                                            {
                                                if (htIsStaAdedtodaySession.Count >= dvstafflsi.Count)
                                                {
                                                    if (st == dvstafflsi.Count - 1)
                                                    {
                                                        isCheckOnce = true;
                                                    }
                                                }
                                            }
                                            if (alstaff < neestaff)
                                            {
                                                int yearofexp = 0;
                                                string perexp = Convert.ToString(dvstafflsi[st]["experience_info"]);
                                                string da1 = Convert.ToString(dvstafflsi[st]["join_date"]);
                                                string staffcode = Convert.ToString(dvstafflsi[st]["staff_code"]);
                                                string staffname = Convert.ToString(dvstafflsi[st]["staff_name"]);
                                                //Added by Idhris 13-02-2017
                                                if (checkSession)
                                                {
                                                    if (htIsStaAdedtodaySession.Contains(Convert.ToString(dt.ToString("MM/dd/yyyy") + "@" + sesval + "_" + staffcode).Trim().ToLower()))
                                                    {
                                                        continue;
                                                    }
                                                }
                                                else
                                                {
                                                    if (htIsStaAdedtoday.Contains(dt.ToString("MM/dd/yyyy") + "_" + staffcode))
                                                    {
                                                        continue;//continue to next staff if this staff already added for today
                                                    }
                                                }
                                                if (staffcode == "766")
                                                {
                                                }
                                                perexp = string.Empty;
                                                if (!hatstaffdatesession.Contains(staffcode))
                                                {
                                                    if (!hatstaffcompletinvi.Contains(staffcode))
                                                    {
                                                        if (!hatstaffexp.Contains(staffcode))
                                                        {
                                                            int epx = exper(da1, perexp);
                                                            hatstaffexp.Add(staffcode, epx);
                                                            yearofexp = epx;
                                                        }
                                                        else
                                                        {
                                                            yearofexp = Convert.ToInt32(hatstaffexp[staffcode]);
                                                        }
                                                        int maxinvigle = 0;
                                                        string sesvalstaff = string.Empty;
                                                        if (!hatstaffinvicount.Contains(staffcode))
                                                        {
                                                            dshallsupset.Tables[0].DefaultView.RowFilter = string.Empty;
                                                            DataView dvexmval = dshallsupset.Tables[0].DefaultView;
                                                            for (int exp = 0; exp < dvexmval.Count; exp++)
                                                            {
                                                                int expfrom = Convert.ToInt32(dvexmval[exp]["expfrom"].ToString());
                                                                int expto = Convert.ToInt32(dvexmval[exp]["expto"].ToString());
                                                                maxinvigle = Convert.ToInt32(dvexmval[exp]["max_superivison"].ToString());
                                                                sesvalstaff = Convert.ToString(dvexmval[exp]["session"].ToString());
                                                                if (expfrom <= yearofexp && expto >= yearofexp)
                                                                {
                                                                    string setva = maxinvigle + "-" + sesvalstaff;
                                                                    hatstaffinvicount.Add(staffcode, setva);
                                                                    exp = dvexmval.Count;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            string strgetval = Convert.ToString(hatstaffinvicount[staffcode]);
                                                            string[] spv = strgetval.Split('-');
                                                            if (spv.GetUpperBound(0) > 0)
                                                            {
                                                                maxinvigle = Convert.ToInt32(spv[0]);
                                                                sesvalstaff = Convert.ToString(spv[1]);
                                                            }
                                                        }
                                                        string[] spse = sesvalstaff.Split('/');
                                                        for (int ses = 0; ses <= spse.GetUpperBound(0); ses++)
                                                        {
                                                            string getstaffse = spse[ses].ToString().Trim();
                                                            if (getstaffse.Trim().ToLower() == sesval.Trim().ToLower())
                                                            {
                                                                int noofiniviglesess = 0;
                                                                if (hatstaffinviglesession.Contains(staffcode))
                                                                {
                                                                    noofiniviglesess = Convert.ToInt32(hatstaffinviglesession[staffcode]);
                                                                }
                                                                if (maxinvigle >= noofiniviglesess)
                                                                {
                                                                    if (staffcode == "1033")
                                                                    {
                                                                    }
                                                                    string strinsretinvival = "if not exists(select * from examinvigilatormaster where edate='" + dt.ToString("MM/dd/yyyy") + "' and ses_sion='" + sesval + "' and roomno='" + roomno + "' and invigilator_code='" + staffcode + "')";
                                                                    strinsretinvival = strinsretinvival + " insert into examinvigilatormaster(roomno,edate,ses_sion,invigilator_name,invigilator_code,month,year)";
                                                                    strinsretinvival = strinsretinvival + " values('" + roomno + "','" + dt.ToString("MM/dd/yyyy") + "','" + sesval + "','" + staffname + "','" + staffcode + "','" + exammonth + "','" + exmayear + "')";
                                                                    int insexminvigile = d2.update_method_wo_parameter(strinsretinvival, "text");
                                                                    alstaff++;
                                                                    countLoop++;
                                                                    if (!htIsStaAdedtoday.Contains(dt.ToString("MM/dd/yyyy") + "_" + staffcode))
                                                                    {
                                                                        htIsStaAdedtoday.Add(dt.ToString("MM/dd/yyyy") + "_" + staffcode, "1");
                                                                    }
                                                                    if (!htIsStaAdedtodaySession.Contains(Convert.ToString(dt.ToString("MM/dd/yyyy") + "@" + sesval + "_" + staffcode).Trim().ToLower()))
                                                                    {
                                                                        htIsStaAdedtodaySession.Add(Convert.ToString(dt.ToString("MM/dd/yyyy") + "@" + sesval + "_" + staffcode).Trim().ToLower(), "1");
                                                                    }
                                                                    //if (se == 0)
                                                                    //{
                                                                    //    ascCount++;
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    //    descCount++;
                                                                    //}
                                                                    NCount++;
                                                                    getflag = true;
                                                                    noofiniviglesess++;
                                                                    if (!hatstaffinviglesession.Contains(staffcode))
                                                                    {
                                                                        hatstaffinviglesession.Add(staffcode, noofiniviglesess);
                                                                    }
                                                                    else
                                                                    {
                                                                        hatstaffinviglesession[staffcode] = noofiniviglesess;
                                                                    }
                                                                    if (!hatstaffdatesession.Contains(staffcode))
                                                                    {
                                                                        hatstaffdatesession.Add(staffcode, staffcode);
                                                                    }
                                                                    if (noofiniviglesess == maxinvigle)
                                                                    {
                                                                        if (!hatstaffcompletinvi.Contains(staffcode))
                                                                        {
                                                                            hatstaffcompletinvi.Add(staffcode, staffcode);
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (!hatstaffcompletinvi.Contains(staffcode))
                                                                    {
                                                                        hatstaffcompletinvi.Add(staffcode, staffcode);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    //  st = dsinivistaff.Tables[0].Rows.Count;
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        if (alstaff != neestaff)
                                        {
                                            NCount = 0;
                                        }
                                    }
                                }
                            }
                            ///// adding addtional staffdetails
                            if (noaddstaff > 0)
                            {
                                if (dvexamdateromster.Count > 0)
                                {
                                    neestaff = noaddstaff;
                                    alstaff = 0;
                                    dsinivistaff.Tables[0].DefaultView.RowFilter = string.Empty;
                                    DataView dvstafflsi = dsinivistaff.Tables[0].DefaultView;
                                    dvstafflsi.Sort = "join_date asc";
                                    for (int st = 0; st < dsinivistaff.Tables[0].Rows.Count; st++)
                                    {
                                        if (alstaff < neestaff)
                                        {
                                            int yearofexp = 0;
                                            string perexp = Convert.ToString(dsinivistaff.Tables[0].Rows[st]["experience_info"]);
                                            string da1 = Convert.ToString(dsinivistaff.Tables[0].Rows[st]["join_date"]);
                                            string staffcode = Convert.ToString(dsinivistaff.Tables[0].Rows[st]["staff_code"]);
                                            string staffname = Convert.ToString(dsinivistaff.Tables[0].Rows[st]["staff_name"]);
                                            perexp = string.Empty;
                                            if (!hatstaffdatesession.Contains(staffcode))
                                            {
                                                if (!hatstaffcompletinvi.Contains(staffcode))
                                                {
                                                    if (!hatstaffexp.Contains(staffcode))
                                                    {
                                                        int epx = exper(da1, perexp);
                                                        hatstaffexp.Add(staffcode, epx);
                                                        yearofexp = epx;
                                                    }
                                                    else
                                                    {
                                                        yearofexp = Convert.ToInt32(hatstaffexp[staffcode]);
                                                    }
                                                    int maxinvigle = 0;
                                                    string sesvalstaff = string.Empty;
                                                    if (!hatstaffinvicount.Contains(staffcode))
                                                    {
                                                        dshallsupset.Tables[0].DefaultView.RowFilter = string.Empty;
                                                        DataView dvexmval = dshallsupset.Tables[0].DefaultView;
                                                        for (int exp = 0; exp < dvexmval.Count; exp++)
                                                        {
                                                            int expfrom = Convert.ToInt32(dvexmval[exp]["expfrom"].ToString());
                                                            int expto = Convert.ToInt32(dvexmval[exp]["expto"].ToString());
                                                            maxinvigle = Convert.ToInt32(dvexmval[exp]["max_superivison"].ToString());
                                                            sesvalstaff = Convert.ToString(dvexmval[exp]["session"].ToString());
                                                            if (expfrom <= yearofexp && expto >= yearofexp)
                                                            {
                                                                string setva = maxinvigle + "-" + sesval;
                                                                hatstaffinvicount.Add(staffcode, setva);
                                                                exp = dvexmval.Count;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        string strgetval = Convert.ToString(hatstaffinvicount[staffcode]);
                                                        string[] spv = strgetval.Split('-');
                                                        if (spv.GetUpperBound(0) > 0)
                                                        {
                                                            maxinvigle = Convert.ToInt32(spv[0]);
                                                            sesvalstaff = Convert.ToString(spv[1]);
                                                        }
                                                    }
                                                    string[] spse = sesvalstaff.Split('/');
                                                    for (int ses = 0; ses <= spse.GetUpperBound(0); ses++)
                                                    {
                                                        string getstaffse = spse[ses].ToString().Trim();
                                                        if (getstaffse.Trim().ToLower() == sesval.Trim().ToLower())
                                                        {
                                                            int noofiniviglesess = 0;
                                                            if (hatstaffinviglesession.Contains(staffcode))
                                                            {
                                                                noofiniviglesess = Convert.ToInt32(hatstaffinviglesession[staffcode]);
                                                            }
                                                            if (noofiniviglesess > 3)
                                                            {
                                                                if (maxinvigle >= noofiniviglesess)
                                                                {
                                                                    string strinsretinvival = "if not exists(select * from examinvigilatormaster where edate='" + dt.ToString("MM/dd/yyyy") + "' and ses_sion='" + sesval + "' and roomno='" + roomno + "' and invigilator_code='" + staffcode + "')";
                                                                    strinsretinvival = strinsretinvival + " insert into examinvigilatormaster(roomno,edate,ses_sion,invigilator_name,invigilator_code,month,year)";
                                                                    strinsretinvival = strinsretinvival + " values('" + roomno + "','" + dt.ToString("MM/dd/yyyy") + "','" + sesval + "','" + staffname + "','" + staffcode + "','" + exammonth + "','" + exmayear + "')";
                                                                    int insexminvigile = d2.update_method_wo_parameter(strinsretinvival, "text");
                                                                    alstaff++;
                                                                    countLoop++;
                                                                    getflag = true;
                                                                    noofiniviglesess++;
                                                                    if (!hatstaffinviglesession.Contains(staffcode))
                                                                    {
                                                                        hatstaffinviglesession.Add(staffcode, noofiniviglesess);
                                                                    }
                                                                    else
                                                                    {
                                                                        hatstaffinviglesession[staffcode] = noofiniviglesess;
                                                                    }
                                                                    if (!hatstaffdatesession.Contains(staffcode))
                                                                    {
                                                                        hatstaffdatesession.Add(staffcode, staffcode);
                                                                    }
                                                                    if (noofiniviglesess == maxinvigle)
                                                                    {
                                                                        if (!hatstaffcompletinvi.Contains(staffcode))
                                                                        {
                                                                            hatstaffcompletinvi.Add(staffcode, staffcode);
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (!hatstaffcompletinvi.Contains(staffcode))
                                                                    {
                                                                        hatstaffcompletinvi.Add(staffcode, staffcode);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //  st = dsinivistaff.Tables[0].Rows.Count;
                                            }
                                        }
                                        else
                                        {
                                            //st = dsinivistaff.Tables[0].Rows.Count;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (getflag == true)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert(' Saved Successfully')", true);
            }
            else
            {
                lblerr1.Visible = true;
                lblerr1.Text = "Not Generated";
            }
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    public int exper(string da1, string perexp)
    {
        int exp = 0;
        try
        {
            int totalexpyear = 0;
            int totalexpmonth = 0;
            int cureyear = 0;
            int curemonth = 0;
            string joindatestaff = "-";
            if (da1.ToString() != "" && da1 != null)
            {
                DateTime dtexp = Convert.ToDateTime(da1);
                joindatestaff = dtexp.ToString("dd/MM/yyyy");
            }
            if (da1.ToString() != "" && da1 != null)
            {
                DateTime dt = DateTime.Now;
                DateTime dtexp = Convert.ToDateTime(da1);
                int cury = Convert.ToInt32(dt.ToString("yyyy"));
                int jyear = Convert.ToInt32(dtexp.ToString("yyyy"));
                cureyear = cury - jyear;
                int curmon = Convert.ToInt32(dt.ToString("MM"));
                int jmon = Convert.ToInt32(dtexp.ToString("MM"));
                if (curmon < jmon)
                {
                    curemonth = (curmon + 12) - jmon;
                    cureyear--;
                }
                else
                {
                    curemonth = curmon - jmon;
                }
            }
            if (perexp != "")
            {
                int expyear = 0;
                int expmon = 0;
                string[] spit = perexp.Split('\\');
                for (int s = 0; s <= spit.GetUpperBound(0); s++)
                {
                    if (spit[s].Trim().ToString() != "" && spit[s] != "")
                    {
                        string[] sporg = spit[s].Split(';');
                        if (sporg.GetUpperBound(0) > 10)
                        {
                            string yer = sporg[6].ToString();
                            if (yer.ToString().Trim() != "" && yer != null)
                            {
                                expyear = expyear + Convert.ToInt32(yer);
                            }
                            string mon = sporg[7].ToString();
                            if (mon.ToString().Trim() != "" && mon != null)
                            {
                                expmon = expmon + Convert.ToInt32(mon);
                            }
                        }
                    }
                }
                int exy = 0;
                int exaxcm = 0;
                if (expmon.ToString().Trim() != "" && expmon != null)
                {
                    if (expmon > 11)
                    {
                        exy = expmon / 12;
                        exaxcm = expmon % 12;
                    }
                    else
                    {
                        exaxcm = expmon;
                    }
                }
                expyear = expyear + exy;
                totalexpyear = expyear;
                totalexpmonth = exaxcm;
            }
            totalexpyear = cureyear + totalexpyear;
            totalexpmonth = curemonth + totalexpmonth;
            if (totalexpmonth > 11)
            {
                totalexpmonth = totalexpmonth - 12;
                totalexpyear++;
            }
            exp = totalexpyear;
        }
        catch (SqlException ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
        return exp;
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string strexcelname = txtexcelname.Text;
            if (strexcelname != "")
            {
                d2.printexcelreport(AttSpread, strexcelname);
            }
            else
            {
                lblerr1.Text = "Please enter your Report Name";
                lblerr1.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    public void loadstaff()
    {
        try
        {
            string strtypeval = string.Empty;
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                strtypeval = " and c.type='" + ddltype.SelectedItem.ToString() + "'";
            }
            string striniviligationstaffquery = "select c.staff_code,sm.staff_name,sm.staff_name+'-'+c.staff_code as staffval from examstaffmaster c,staffmaster sm,staff_appl_master sa  where sm.appl_no=sa.appl_no and sm.staff_code=c.staff_code and c.Inivigition='1' " + strtypeval + " order by sm.staff_name";
            DataSet dsinivistaff = d2.select_method_wo_parameter(striniviligationstaffquery, "Text");
            if (dsinivistaff.Tables[0].Rows.Count > 0)
            {
                ddlstaff.DataSource = dsinivistaff;
                ddlstaff.DataTextField = "staffval";
                ddlstaff.DataValueField = "staff_code";
                ddlstaff.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void chkautomatic_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        if (chkautomatic.Checked == true)
        {
            lblstaffperstu.Visible = true;
            txtstaffperstu.Visible = true;
            btngenerate.Visible = true;
            lblattdinationstaff.Visible = true;
            txtaddtionalstafff.Visible = true;
            divMoveStaff.Visible = false;
        }
        else
        {
            lblstaffperstu.Visible = false;
            txtstaffperstu.Visible = false;
            btngenerate.Visible = false;
            lblattdinationstaff.Visible = false;
            txtaddtionalstafff.Visible = false;
            divMoveStaff.Visible = false;
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
    }

    protected void btnPrint_Click1(object sender, EventArgs e)
    {
        string mothyer = string.Empty;
        string yersessn = string.Empty;
        string sessn = string.Empty;
        mothyer = ddlMonth.SelectedValue.ToString() + "/" + ddlYear.SelectedValue.ToString();
        sessn = DropDownList1.SelectedItem.ToString();
        Session["column_header_row_count"] = 2;
        string dcommt = " Exam Invigilator Report " + '@' + "Exam Month/Year : " + mothyer + '@' + "Date of Exam/Session : " + yersessn + "";
        Printcontrol.loadspreaddetails(AttSpread, "ExaminvigilatorReport.aspx", dcommt);
        Printcontrol.Visible = true;
    }

    protected void AttSpread_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        AttSpread.SaveChanges();
        selectedRow = AttSpread.Sheets[0].ActiveRow;

        if (e.CommandName == "addstaff")
        {
            if (selectedRow >= 0)
            {
                divAddStaff.Visible = true;
                getstafflist();
            }
        }
        else if (e.CommandName == "movestaff")
        {
            AttSpread.SaveChanges();
            ddlstafffrom.Dispose();
            txtStaff1.Value = "";
            txtfromdate2.Value = "";
            actColumn = AttSpread.Sheets[0].ActiveColumn;
            selectedRow = AttSpread.Sheets[0].ActiveRow;

            selectedStaffCount = AttSpread.Sheets[0].Cells[selectedRow, 6].Text.ToString().Trim();

            selectedroom = AttSpread.Sheets[0].Cells[selectedRow, 3].Text.ToString().Trim();
            sel_date = AttSpread.Sheets[0].Cells[selectedRow, 1].Text.ToString().Trim();

            int.TryParse(selectedStaffCount, out staffCnt);

            selecteddate = AttSpread.Sheets[0].Cells[selectedRow, 1].Text.ToString().Trim();
            date2 = selecteddate;
            //selecteddate = date2.ToString("yyyy/mm/dd");
            DateTime dtSelectedDate = new DateTime();
            DateTime.TryParseExact(selecteddate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtSelectedDate);

            selectedsession = AttSpread.Sheets[0].Cells[selectedRow, 2].Text.ToString().Trim();

            selectedstaff = AttSpread.Sheets[0].Cells[selectedRow, 7].Text.ToString().Trim();

            string selectedStaffCodeList = Convert.ToString(AttSpread.Sheets[0].Cells[selectedRow, 6].Tag).Trim();
            string[] staffar = selectedStaffCodeList.Split(',');
            string[] indiStaff = new string[2];
            string qry = string.Empty;
            staffval = string.Empty; ;


            qry = "select invigilator_name,invigilator_code from examinvigilatormaster where edate='" + dtSelectedDate.ToShortDateString() + "' and ses_sion='" + selectedsession + "' and roomno='" + selectedroom + "'";
            ds = d2.select_method_wo_parameter(qry, "text");
            if (ds.Tables.Count > 0)
            {
                ddlstafffrom.DataSource = ds;
                ddlstafffrom.DataTextField = "invigilator_name";
                ddlstafffrom.DataValueField = "invigilator_code";
                ddlstafffrom.DataBind();
            }

            txtStaff1.Value = selectedStaffCount;
            txtfromdate2.Value = selecteddate;

            divMoveStaff.Visible = true;

            ddlsessionto.Items.Clear();
            ddlsessionto.Items.Add(new ListItem("--", "0"));
            ddlsessionto.Items.Add(new ListItem("F.N", "1"));
            ddlsessionto.Items.Add(new ListItem("A.N", "2"));


            string exam_year = Convert.ToString(ddlYear.SelectedValue);
            string exam_month = Convert.ToString(ddlMonth.SelectedValue);

            string fdate = selecteddate.ToString();

            string[] spf = fdate.Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);

            string tdate = txttodate.Text.ToString();
            DateTime dtt = new DateTime();
            DateTime.TryParseExact(tdate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtt);

            getExamDate();
            btngo_Click(sender, e);
        }
        else if(e.CommandName=="deletestaff")
        {
            AttSpread.SaveChanges();
            ddldeletestaffVal.Items.Clear();
            lblDeleteSessionVal.Text =  string.Empty;
            lblDeleteDateVal.Text =     string.Empty;
            lblDeleteHallVal.Text = string.Empty;

            actColumn = AttSpread.Sheets[0].ActiveColumn;
            selectedRow = AttSpread.Sheets[0].ActiveRow;

            selectedroom = AttSpread.Sheets[0].Cells[selectedRow, 3].Text.ToString().Trim();
            sel_date = AttSpread.Sheets[0].Cells[selectedRow, 1].Text.ToString().Trim();

            selecteddate = AttSpread.Sheets[0].Cells[selectedRow, 1].Text.ToString().Trim();
            date2 = selecteddate;
            //selecteddate = date2.ToString("yyyy/mm/dd");
            DateTime dtSelectedDate = new DateTime();
            DateTime.TryParseExact(selecteddate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtSelectedDate);

            selectedsession = AttSpread.Sheets[0].Cells[selectedRow, 2].Text.ToString().Trim();

            //selectedstaff = AttSpread.Sheets[0].Cells[selectedRow, 7].Text.ToString().Trim();

            string qry = "select invigilator_name,invigilator_code from examinvigilatormaster where edate='" + dtSelectedDate.ToShortDateString() + "' and ses_sion='" + selectedsession + "' and roomno='" + selectedroom + "'";
            ds = d2.select_method_wo_parameter(qry, "text");
            if (ds.Tables.Count > 0)
            {
                ddldeletestaffVal.DataSource = ds.Tables[0];
                ddldeletestaffVal.DataTextField = "invigilator_name";
                ddldeletestaffVal.DataValueField = "invigilator_code";
                ddldeletestaffVal.DataBind();

                if (ddldeletestaffVal.Items.Count > 1)
                {
                    lblDeleteSessionVal.Text = selectedsession;
                    lblDeleteDateVal.Text = dtSelectedDate.ToShortDateString();
                    lblDeleteHallVal.Text = selectedroom;
                    divDeleteStaff.Visible = true;
                }
                else
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "You Cannot Delete the Staff from the Hall which has only One Staff";
                    divDeleteStaff.Visible = false;
                    btngo_Click(sender, e);
                    cellClicked = true;
                }
            }
            else
            {
                ddldeletestaffVal.Items.Add("--Select--");
            }
        }
        else
        {
            try
            {
                string actrow = e.SheetView.ActiveRow.ToString();
                if (flag_true == false && actrow == "0")
                {
                    int s = Convert.ToInt16(AttSpread.Sheets[0].Cells[0, 6].Value);
                    for (int j = 1; j < Convert.ToInt16(AttSpread.Sheets[0].RowCount); j++)
                    {
                        AttSpread.Sheets[0].Cells[j, 6].Value = s;
                    }
                    flag_true = true;
                }
            }
            catch (Exception ex)
            {
                lblerr1.Text = ex.ToString();
                lblerr1.Visible = true;
            }
        }
    }

    //protected void FpSpread2_SelectedIndexChanged(Object sender, EventArgs e)
    //{
    //    try
    //    {
    //        // Cellclick = true;
    //        if (Cellclick == true)
    //        {
    //            string activerow = "";
    //            string activecol = "";
    //            activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
    //            activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
    //            int ar;
    //            int ac;
    //            ar = Convert.ToInt32(activerow.ToString());
    //            ac = Convert.ToInt32(activecol.ToString());
    //            if (ar != -1)
    //            {
    //                txtmessage.Text = FpSpread2.Sheets[0].GetText(ar, 1);
    //            }
    //            Cellclick = false;
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}

    protected void btnletter_Click(object sender, EventArgs e)
    {
        try
        {
            if (!File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg")))
            {
                DataSet dsstuphoto = d2.select_method_wo_parameter("select fileupload from tbl_notification where viewrs='Printmaster' and College_Code='" + Session["collegecode"].ToString() + "'", "Text");
                if (dsstuphoto.Tables[0].Rows.Count > 0)
                {
                    if (dsstuphoto.Tables[0].Rows[0]["fileupload"] != null && dsstuphoto.Tables[0].Rows[0]["fileupload"].ToString().Trim() != "")
                    {
                        byte[] file = (byte[])dsstuphoto.Tables[0].Rows[0]["fileupload"];
                        MemoryStream memoryStream = new MemoryStream();
                        memoryStream.Write(file, 0, file.Length);
                        if (file.Length > 0)
                        {
                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                            System.Drawing.Image thumb = imgx.GetThumbnailImage(2630, 440, null, IntPtr.Zero);
                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg")))
                            {
                                thumb.Save(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                        }
                        memoryStream.Dispose();
                        memoryStream.Close();
                    }
                }
            }
            AttSpread.SaveChanges();
            int selectedcount = 0;
            for (int res = 1; res <= Convert.ToInt32(AttSpread.Sheets[0].RowCount) - 1; res++)
            {
                int isval = Convert.ToInt32(AttSpread.Sheets[0].Cells[res, 6].Value);
                if (isval == 1)
                {
                    selectedcount++;
                }
            }
            if (selectedcount == 0)
            {
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select the Staff and then Proceed";
                return;
            }
            Font Fontbold = new Font("Book Antiqua", 17, FontStyle.Bold);
            Font Fontboldd = new Font("Book Antiqua", 17, FontStyle.Regular);
            Font Fontboldbig = new Font("Book Antiqua", 21, FontStyle.Bold);
            Font Fontbold1 = new Font("Book Antiqua", 12, FontStyle.Bold);
            Font Fontbold2 = new Font("Book Antiqua", 15, FontStyle.Regular);
            Font Fontsmall = new Font("Book Antiqua", 13, FontStyle.Regular);
            Font Fontsmall1 = new Font("Book Antiqua", 15, FontStyle.Regular);
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
            string examyear = ddlYear.SelectedItem.ToString();
            string exammonth = ddlMonth.SelectedValue.ToString();
            bool halfflag = false;
            if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
            {
                string strtypeval = string.Empty;
                if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
                {
                    strtypeval = " and c.type='" + ddltype.SelectedItem.ToString() + "'";
                }
                string spreadbind1 = "select distinct sm.staff_name,sm.staff_code,h.dept_name,d.desig_name from examinvigilatormaster ev,staffmaster sm,stafftrans st,hrdept_master h,examstaffmaster c,desig_master d where ev.invigilator_code=sm.staff_code and st.desig_code=d.desig_code and sm.staff_code=st.staff_code and c.staff_code=ev.invigilator_code and st.dept_code=h.dept_code and st.latestrec=1 " + strtypeval + " and ev.month='" + ddlMonth.SelectedValue.ToString() + "' and ev.year='" + ddlYear.SelectedValue.ToString() + "'";
                spreadbind1 = spreadbind1 + "select sm.staff_name,sm.staff_code,h.dept_name,Convert(nvarchar(15),ev.edate,103) edate,ev.ses_sion,ev.roomno from examinvigilatormaster ev,staffmaster sm,stafftrans st,hrdept_master h,examstaffmaster c where ev.invigilator_code=sm.staff_code and sm.staff_code=st.staff_code and c.staff_code=ev.invigilator_code and st.dept_code=h.dept_code and inivigition='1' and st.latestrec=1 " + strtypeval + " and ev.month='" + ddlMonth.SelectedValue.ToString() + "' and ev.year='" + ddlYear.SelectedValue.ToString() + "' order by sm.staff_code,ev.edate,ev.ses_sion desc";
                DataSet ds2 = d2.select_method_wo_parameter(spreadbind1, "Text");
                string strcolldetails = " select * from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
                DataSet dshall = d2.select_method_wo_parameter(strcolldetails, "Text");
                string collname = string.Empty;
                string address = string.Empty;
                string pincode = string.Empty;
                string university = string.Empty;
                string category = string.Empty;
                string priciplaname = string.Empty;
                string addval = string.Empty;
                if (dshall.Tables[0].Rows.Count > 0)
                {
                    collname = dshall.Tables[0].Rows[0]["collname"].ToString();
                    string ad1 = dshall.Tables[0].Rows[0]["address1"].ToString();
                    string ad2 = dshall.Tables[0].Rows[0]["address2"].ToString();
                    string ad3 = dshall.Tables[0].Rows[0]["address3"].ToString();
                    university = dshall.Tables[0].Rows[0]["university"].ToString();
                    category = dshall.Tables[0].Rows[0]["category"].ToString();
                    pincode = dshall.Tables[0].Rows[0]["pincode"].ToString();
                    priciplaname = dshall.Tables[0].Rows[0]["principal"].ToString();
                    if (ad1 != "" && ad1 != null)
                    {
                        address = ad1;
                    }
                    if (ad2 != "" && ad2 != null)
                    {
                        if (addval != "")
                        {
                            addval = ad2;
                        }
                    }
                    if (pincode != "" && pincode != null)
                    {
                        if (pincode != "")
                        {
                            addval = addval + "- " + pincode;
                        }
                        else
                        {
                            addval = pincode;
                        }
                    }
                }
                DataSet supplymsubds = new DataSet();
                string strsupplymsub = string.Empty;
                for (int res = 1; res <= Convert.ToInt32(AttSpread.Sheets[0].RowCount) - 1; res++)
                {
                    Double coltop = 0;
                    int isval = Convert.ToInt32(AttSpread.Sheets[0].Cells[res, 6].Value);
                    if (isval == 1)
                    {
                        string staffnbame = AttSpread.Sheets[0].Cells[res, 1].Text.ToString();
                        string staffcode = AttSpread.Sheets[0].Cells[res, 2].Text.ToString();
                        string department = AttSpread.Sheets[0].Cells[res, 3].Text.ToString();
                        ds2.Tables[0].DefaultView.RowFilter = "staff_code='" + staffcode + "'";
                        DataView dvstaffde = ds2.Tables[0].DefaultView;
                        // stuexamsubcount = dvstaffde.Count;
                        if (dvstaffde.Count > 0)
                        {
                            PdfTextArea ptc;
                            string designation = dvstaffde[0]["desig_name"].ToString();
                            halfflag = true;
                            mypdfpage = mydocument.NewPage();
                            if (chkheadimage.Checked == true)
                            {
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg")))
                                {
                                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg"));
                                    mypdfpage.Add(LogoImage, 25, 10, 260);
                                }
                                coltop = 90;
                            }
                            else
                            {
                                coltop = coltop + 10;
                                ptc = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, collname + " ( " + category + " )");
                                mypdfpage.Add(ptc);
                                coltop = coltop + 20;
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, address);
                                mypdfpage.Add(ptc);
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                {
                                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage, 30, 10, 500);
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                {
                                    PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                    mypdfpage.Add(leftimage, 740, 10, 500);
                                }
                            }
                            coltop = coltop + 40;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, priciplaname);
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, 680, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date :" + DateTime.Now.ToString("dd-MMM-yy"));
                            mypdfpage.Add(ptc);
                            coltop = coltop + 20;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "PRINCIPAL");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 40;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "To");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 15;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, staffnbame);
                            mypdfpage.Add(ptc);
                            coltop = coltop + 15;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, designation + ". Dept. of " + department);
                            mypdfpage.Add(ptc);
                            coltop = coltop + 15;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, collname);
                            mypdfpage.Add(ptc);
                            //coltop = coltop + 15;
                            //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                            //                                            new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, address);
                            //mypdfpage.Add(ptc);
                            //coltop = coltop + 15;
                            //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                            //                                            new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, addval);
                            //mypdfpage.Add(ptc);
                            coltop = coltop + 30;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Dear Sir/Madam.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 30;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Sub : End of Semester Examinations" + ddlMonth.SelectedItem.Text.ToString() + "-" + ddlYear.SelectedItem.Text.ToString());//NOV-2017
                            mypdfpage.Add(ptc);
                            coltop = coltop + 30;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Kindly invigilate according to the schedule give below :");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 30;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 100, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "--------------------------------------------------------------------------------------------");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 20;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 150, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "S.No ");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 250, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Exam Date");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocument, 350, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Session");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 10;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 100, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "--------------------------------------------------------------------------------------------");
                            mypdfpage.Add(ptc);
                            ds2.Tables[1].DefaultView.RowFilter = "staff_code='" + staffcode + "'";
                            DataView dvstaffdeesd = ds2.Tables[1].DefaultView;
                            int sbbno = 0;
                            for (int es = 0; es < dvstaffdeesd.Count; es++)
                            {
                                sbbno++;
                                string strdate = dvstaffdeesd[es]["edate"].ToString();
                                string sessva = dvstaffdeesd[es]["ses_sion"].ToString();
                                coltop = coltop + 20;
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 150, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, sbbno.ToString());
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 250, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, strdate);
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                          new PdfArea(mydocument, 350, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, sessva);
                                mypdfpage.Add(ptc);
                            }
                            coltop = coltop + 10;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 100, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "--------------------------------------------------------------------------------------------");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 30;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Your's sincerely,");
                            mypdfpage.Add(ptc);
                            MemoryStream memoryStream1 = new MemoryStream();
                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Principalsign.jpeg")))
                            {
                                if (dshall.Tables[0].Rows[0]["principal_sign"] != null && dshall.Tables[0].Rows[0]["principal_sign"].ToString().Trim() != "")
                                {
                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Principalsign.jpeg")))
                                    {
                                        byte[] file = (byte[])dshall.Tables[0].Rows[0]["principal_sign"];
                                        memoryStream1.Write(file, 0, file.Length);
                                        if (file.Length > 0)
                                        {
                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream1, true, true);
                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/Principalsign.jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                        memoryStream1.Dispose();
                                        memoryStream1.Close();
                                    }
                                }
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Principalsign.jpeg")))
                            {
                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Principalsign.jpeg"));
                                mypdfpage.Add(LogoImage, 20, coltop + 30, 500);
                            }
                            coltop = coltop + 80;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "PRINCIPAL");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 20;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "NOTE :");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 10;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 20, coltop, 800, 60), System.Drawing.ContentAlignment.MiddleLeft, "(i)  changes shall not be made without prior approval.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 20;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 20, coltop, 800, 60), System.Drawing.ContentAlignment.MiddleLeft, "(ii)  Please report to the examinations office atleast 20 minutes before the examinations begin.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 20;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 20, coltop, 800, 60), System.Drawing.ContentAlignment.MiddleLeft, "(iii) All Invigilators are expected to return the answer papers to the Examinations Office and sign the register at the ");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 20;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 20, coltop, 800, 60), System.Drawing.ContentAlignment.MiddleLeft, "      end of each session.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 20;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 20, coltop, 800, 60), System.Drawing.ContentAlignment.MiddleLeft, "(iv) Fore-noon : 9.30 am TO 12.30 pm");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 20;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 20, coltop, 800, 60), System.Drawing.ContentAlignment.MiddleLeft, "      after-noon : 1.30 pm TO 04.30 pm");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 20;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 20, coltop, 800, 60), System.Drawing.ContentAlignment.MiddleLeft, "(v) Do not allow candidates without their HALL TICKETS as they may have dues.");
                            mypdfpage.Add(ptc);
                            mypdfpage.SaveToDocument();
                            lblerr1.Visible = false;
                        }
                    }
                }
                if (halfflag == true)
                {
                    lblerr1.Visible = false;
                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        string szPath = appPath + "/Report/";
                        string szFile = "ExamHallTicket.pdf";
                        mydocument.SaveToFile(szPath + szFile);
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/pdf";
                        Response.WriteFile(szPath + szFile);
                    }
                }
                else
                {
                    lblerr1.Text = "Please Select the Student and then Proceed";
                    lblerr1.Visible = true;
                }
            }
            else
            {
                lblerr1.Text = "Please Select Exam Month And Year";
                lblerr1.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerr1.Text = ex.ToString();
            lblerr1.Visible = true;
        }
    }

    protected void btnSendSMS_Click(object sender, EventArgs e)
    {
        try
        {
            AttSpread.SaveChanges();
            bool checkflage = false;
            string month = ddlMonth.SelectedItem.Text.ToString();
            string year = ddlYear.SelectedItem.Text.ToString();
            if (cb_SMS.Checked == true)
            {
                string collegeusercode = string.Empty;
                string sqlcollege = "select SMS_User_ID,college_code from track_value where college_code='" + Session["collegecode"].ToString() + "'";
                DataSet ds = new DataSet();
                ds = d2.select_method_wo_parameter(sqlcollege, "text");
                string user_id = string.Empty;
                string SenderID = string.Empty;
                string Password = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    user_id = ds.Tables[0].Rows[0]["SMS_User_ID"].ToString();
                }
                string getval = d2.GetUserapi(user_id);
                string[] spret = getval.Split('-');
                if (spret.GetUpperBound(0) == 1)
                {
                    SenderID = spret[0].ToString();
                    Password = spret[0].ToString();
                    Session["api"] = user_id;
                    Session["senderid"] = SenderID;
                }
                for (int j = 1; j < AttSpread.Sheets[0].RowCount; j++)
                {
                    int gam = Convert.ToInt32(AttSpread.Sheets[0].Cells[j, 6].Value);
                    if (gam == 1)
                    {
                        string staff_code = Convert.ToString(AttSpread.Sheets[0].Cells[j, 2].Text);
                        string Messange = "Kindly note your invigilation schedule for ESE "+month+" "+year;
                        Messange = Messange + ". " + Convert.ToString(AttSpread.Sheets[0].Cells[j, 5].Text);
                        Messange = Messange + ". Please collect the same from Exam Office.";
                        string MobileNo = d2.GetFunction("select per_mobileno  from staff_appl_master sa,staffmaster s where sa.appl_no =s.appl_no and staff_code ='" + staff_code + "'");
                        //strmobileno = AttSpread.Sheets[0].Cells[j, 16].Text;
                        //MobileNo = "8015867043";
                        if (MobileNo != "Nil" && MobileNo != "" && MobileNo.Trim() != "0")
                        {
                            string mobilenos = MobileNo.ToString();
                            //string strpath1 = "http://unicel.in/SendSMS/sendmsg.php?uname=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobilenos + "&text=" + Messange + " &priority=ndnd&stype=normal";
                            //string isstf = mobilenos;
                            int SMS = d2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), mobilenos, Messange, "1");
                            //smsreport(strpath1, isstf, Messange);
                            checkflage = true;
                        }
                    }
                }
                if (checkflage == true)
                {
                    lblerr1.Text = "SMS Sent Successfully";
                    lblerr1.Visible = true;
                }
                else
                {
                    lblerr1.Text = "Please Select Any one Staff";
                    lblerr1.Visible = true;
                }
            }
        }
        catch
        {
        }
    }

    public void smsreport(string uril, string mobilenos, string message)
    {
        try
        {
            string date = DateTime.Now.ToString("MM/dd/yyyy");
            WebRequest request = WebRequest.Create(uril);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader sr = new StreamReader(data);
            string strvel = sr.ReadToEnd();
            string groupmsgid = string.Empty;
            groupmsgid = strvel.Trim().ToString(); //aruna 02oct2013 strvel;       
            int sms = 0;
            string smsreportinsert = string.Empty;
            string[] split_mobileno = mobilenos.Split(new Char[] { ',' });
            for (int icount = 0; icount <= split_mobileno.GetUpperBound(0); icount++)
            {
                smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date)values( '" + split_mobileno[icount] + "','" + groupmsgid + "','" + message + "','" + Session["collegecode"].ToString() + "','1','" + date + "')";
                sms = d2.update_method_wo_parameter(smsreportinsert, "Text");
            }
        }
        catch (SqlException ex)
        {
        }
    }

    //added by Prabha 28/09/2017
    #region Manual Shipment of Staff

    //public void AttSpread_OnCellClicked(object sender, EventArgs e)
    //{
    //    cellClicked = true;
    //}

    //protected void AttSpread_SelectedIndexChanged(Object sender, EventArgs e)
    //{
    //try
    //{
    //    if (cellClicked == true)
    //    {
    //        AttSpread.SaveChanges();
    //        ddlstafffrom.Dispose();
    //        txtStaff1.Value = "";
    //        txtfromdate2.Value = "";
    //        actColumn = AttSpread.Sheets[0].ActiveColumn;
    //        selectedRow = AttSpread.Sheets[0].ActiveRow;

    //        selectedStaffCount = AttSpread.Sheets[0].Cells[selectedRow, 6].Text.ToString().Trim();

    //        selectedroom = AttSpread.Sheets[0].Cells[selectedRow, 3].Text.ToString().Trim();
    //        sel_date = AttSpread.Sheets[0].Cells[selectedRow, 1].Text.ToString().Trim();

    //        int.TryParse(selectedStaffCount, out staffCnt);

    //        selecteddate = AttSpread.Sheets[0].Cells[selectedRow, 1].Text.ToString().Trim();
    //        date2 = selecteddate;
    //        selectedsession = AttSpread.Sheets[0].Cells[selectedRow, 2].Text.ToString().Trim();

    //        selectedstaff = AttSpread.Sheets[0].Cells[selectedRow, 7].Text.ToString().Trim();

    //        string[] staffar = selectedstaff.Split(',');
    //        string[] indiStaff = new string[2];
    //        string qry = string.Empty;
    //        staffval = string.Empty; ;

    //        for (int i = 0; i < staffar.Length; i++)
    //        {
    //            indiStaff = staffar[i].Split('-');
    //            staffval = staffval + "LTRIM(RTRIM ('" + indiStaff[1] + "')),";
    //        }
    //        qry = "select distinct ei.invigilator_name,ei.invigilator_code from examinvigilatormaster ei, staffmaster sm where sm.staff_code=ei.invigilator_code and ei.invigilator_name IN(" + staffval.Trim() + "'')";
    //        ds = d2.select_method_wo_parameter(qry, "text");
    //        ddlstafffrom.DataSource = ds;
    //        ddlstafffrom.DataTextField = "invigilator_name";
    //        ddlstafffrom.DataValueField = "invigilator_code";
    //        ddlstafffrom.DataBind();

    //        txtStaff1.Value = selectedStaffCount;
    //        txtfromdate2.Value = selecteddate;

    //        divMoveStaff.Visible = true;

    //        //if (ddlsessionto.Items.Count > 0)
    //        //{

    //        //}
    //        //ddlsessionto.Items[0].Text = "--";
    //        //ddlsessionto.Items[1].Text = "A.N";
    //        //ddlsessionto.Items[2].Text = "F.N";
    //        ddlsessionto.Items.Clear();
    //        ddlsessionto.Items.Add(new ListItem("--", "0"));
    //        ddlsessionto.Items.Add(new ListItem("F.N", "1"));
    //        ddlsessionto.Items.Add(new ListItem("A.N", "2"));


    //        string exam_year = Convert.ToString(ddlYear.SelectedValue);
    //        string exam_month = Convert.ToString(ddlMonth.SelectedValue);

    //        string fdate = selecteddate.ToString();

    //        string[] spf = fdate.Split('/');
    //        DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);

    //        string tdate = txttodate.Text.ToString();
    //        DateTime dtt = new DateTime();
    //        DateTime.TryParseExact(tdate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtt);

    //        getExamDate();
    //    }
    //}
    //catch (Exception ex)
    //  {
    //}
    // }

    protected void BtnMovestaff_OnClick(object sender, EventArgs e)
    {
        try
        {
            AttSpread.SaveChanges();
            exammonth = Convert.ToString(ddlMonth.SelectedValue).Trim();
            exmayear = Convert.ToString(ddlYear.SelectedValue).Trim();
            selecteddate = Convert.ToString(ddldateto.SelectedValue).Trim();
            selectedRow = AttSpread.Sheets[0].ActiveRow;
            selectedroom = AttSpread.Sheets[0].Cells[selectedRow, 3].Text.ToString().Trim();
            sel_date = AttSpread.Sheets[0].Cells[selectedRow, 1].Text.ToString().Trim();
            DateTime dtInviDate = new DateTime();
            DateTime.TryParseExact(sel_date, "dd/MM/yyyy", null, DateTimeStyles.None, out dtInviDate);
            if (ddldateto.SelectedItem.ToString().Trim() != "" && ddlstafffrom.SelectedItem.ToString().Trim() != "")
            {
                string strinsretinvival = "if not exists(select * from examinvigilatormaster where edate='" + selecteddate + "' and ses_sion='" + ddlsessionto.SelectedItem.Text + "' and roomno='" + ddlhallto.SelectedItem.Text + "' and invigilator_code='" + ddlstafffrom.SelectedValue.ToString().Trim() + "')";
                strinsretinvival = strinsretinvival + " insert into examinvigilatormaster(roomno,edate,ses_sion,invigilator_name,invigilator_code,month,year)";
                strinsretinvival = strinsretinvival + " values('" + ddlhallto.SelectedItem.Text + "','" + selecteddate + "','" + ddlsessionto.SelectedItem.Text + "','" + ddlstafffrom.SelectedItem.Text + "','" + ddlstafffrom.SelectedValue + "','" + exammonth + "','" + exmayear + "')";

                string delqry = "if exists(select * from examinvigilatormaster where roomno = '" + selectedroom + "'  and edate ='" + dtInviDate.ToString("MM/dd/yyyy") + "' and invigilator_code='" + ddlstafffrom.SelectedValue.ToString().Trim() + "')";
                delqry = delqry + " delete from examinvigilatormaster where roomno = '" + selectedroom + "'  and edate ='" + dtInviDate.ToString("MM/dd/yyyy") + "' and invigilator_code='" + ddlstafffrom.SelectedValue.ToString().Trim() + "'";

                int j = d2.update_method_wo_parameter(delqry, "txt");
                ds = d2.select_method_wo_parameter(strinsretinvival, "text");
                if (j == 1)
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Moved Successfully";
                    lblAlertMsg.ToolTip = "staffmoved";
                }
                else
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Cannot Allot the Staff";
                }
                btngo_Click(sender, e);
                cellClicked = true;
            }
        }
        catch
        {


        }


    }

    protected void BtnMovecncl_OnClick(object sender, EventArgs e)
    {
        divMoveStaff.Visible = false;
        // divAddStaff.Visible = true;
    }

    protected void ddlsessionto_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        getExamHall();
    }

    protected void ddldateto_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddlsessionto.ClearSelection();
        getExamHall();
    }

    private void getExamDate()
    {
        try
        {
            string fdate = txtfromdate.Text.ToString();
            string[] spf = fdate.Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            string tdate = txttodate.Text.ToString();
            string[] spt = tdate.Split('/');
            DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);
            if (dtt < dtf)
            {
                lblerr1.Visible = true;
                lblerr1.Text = "From Date Must Be Lesser than Todate";
                return;
            }
            string strtypeval = string.Empty;
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.ToString().Trim().ToLower() == "day")
                {
                    strtypeval = " and c.type in('Day','MCA')";
                }
                else
                {
                    strtypeval = " and c.type='" + ddltype.SelectedItem.ToString() + "'";
                }
            }
            DataSet dsExamDate = new DataSet();
            string spreadbind1 = "select distinct et.exam_date,Convert(nvarchar(15),et.exam_date,103) as edate  from exmtt e,exmtt_det et,exam_seating es,Degree d,Course c where e.exam_code=et.exam_code and et.exam_date=es.edate and et.exam_session=es.ses_sion and et.subject_no=es.subject_no and e.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and et.exam_date between '" + dtf + "' and '" + dtt + "' " + strtypeval + " order by et.exam_date";
            dsExamDate = d2.select_method_wo_parameter(spreadbind1, "text");
            ddldateto.Items.Clear();
            if (dsExamDate.Tables.Count > 0 && dsExamDate.Tables[0].Rows.Count > 0)
            {
                ddldateto.DataSource = dsExamDate;
                ddldateto.DataTextField = "edate";
                ddldateto.DataValueField = "exam_date";
                ddldateto.DataBind();
            }

        }
        catch
        {
        }
    }

    private void getExamHall()
    {
        try
        {
            ddlhallto.Items.Clear();
            string exam_year = Convert.ToString(ddlYear.SelectedValue);
            string exam_month = Convert.ToString(ddlMonth.SelectedValue);
            string strtype = string.Empty;
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                strtype = " and c.type='" + ddltype.SelectedItem.ToString() + "'";
            }
            //string qryDate = string.Empty;
            //if (ddldateto.Items.Count > 0)
            //{
            //    if (ddldateto.SelectedIndex != 0)
            //        qryDate = " and et.exam_date ='" + ddldateto.SelectedValue.ToString() + "'";
            //}
            string qrySession = string.Empty;
            if (ddlsessionto.Items.Count > 0)
            {
                if (ddlsessionto.SelectedIndex != 0)
                    qrySession = " and es.ses_sion='" + ddlsessionto.SelectedItem.ToString() + "'";
            }
            if (!string.IsNullOrEmpty(qrySession))
            {
                string qry = "select distinct es.roomno from exmtt e,exmtt_det et,exam_seating es,Degree d,Course c where e.exam_code=et.exam_code and et.exam_date=es.edate and et.exam_session=es.ses_sion and et.subject_no=es.subject_no and e.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and e.Exam_year='" + exam_year + "' and e.Exam_month='" + exam_month + "'and et.exam_date ='" + ddldateto.SelectedValue.ToString() + "'  " + strtype + qrySession + " group by et.exam_date,et.exam_session,es.roomno";
                ds = d2.select_method_wo_parameter(qry, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlhallto.DataSource = ds;
                ddlhallto.DataTextField = "roomno";
                ddlhallto.DataValueField = "roomno";
                ddlhallto.DataBind();
            }
        }
        catch { }

    }

    protected void btnAddStaffSave_Click(object sender, EventArgs e)
    {
        selectedRow = AttSpread.Sheets[0].ActiveRow;
        string[] selectedstaff = ddlAddStaffDetails.SelectedItem.Text.Split('-');
        string selectedstaffcode = ddlAddStaffDetails.SelectedValue;
        string staffname = selectedstaff[1];
        selectedroom = AttSpread.Sheets[0].Cells[selectedRow, 3].Text.ToString().Trim();
        selecteddate = AttSpread.Sheets[0].Cells[selectedRow, 1].Text.ToString().Trim();

        string[] slDate = selecteddate.Split('/');
        selecteddate = slDate[2] + '/' + slDate[1] + '/' + slDate[0];

        selectedsession = AttSpread.Sheets[0].Cells[selectedRow, 2].Text.ToString().Trim();
        string strinsretinvival = string.Empty;
        try
        {
            strinsretinvival = "if not exists(select * from examinvigilatormaster where edate='" + selecteddate + "' and ses_sion='" + selectedsession + "' and roomno='" + selectedroom + "' and invigilator_code='" + selectedstaffcode + "')";

            strinsretinvival = strinsretinvival + " insert into examinvigilatormaster(roomno,edate,ses_sion,invigilator_name,invigilator_code,month,year)";
            strinsretinvival = strinsretinvival + " values('" + selectedroom + "','" + selecteddate + "','" + selectedsession + "','" + staffname + "','" + selectedstaffcode + "','" + ddlMonth.SelectedValue + "','" + ddlYear.SelectedValue + "')";
            int i = dir.insertData(strinsretinvival);
            if (i == 1)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Staff Alloted Successfully";
                lblAlertMsg.ToolTip = "staffadded";
                getstafflist();
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Cannot Allot the Staff";
            }
        }
        catch
        {
            divPopAlert.Visible = true;
            lblAlertMsg.Text = "Cannot Allot the Staff";
        }

    }

    protected void getstafflist()
    {
        selectedRow = AttSpread.Sheets[0].ActiveRow;
        selectedroom = AttSpread.Sheets[0].Cells[selectedRow, 3].Text.ToString().Trim();
        selecteddate = AttSpread.Sheets[0].Cells[selectedRow, 1].Text.ToString().Trim();
        selectedsession = AttSpread.Sheets[0].Cells[selectedRow, 2].Text.ToString().Trim();
        string[] date = selecteddate.Split('/');
        string dtt = Convert.ToString(date[2] + "-" + date[1] + "-" + date[0]);
        DataTable dt = new DataTable();
        try
        {
            string sqlstafflist = "select distinct hrm.dept_name+' - '+sfm.staff_name as invigilator_name,sfm.staff_code from examstaffmaster esm,staffmaster sfm,stafftrans st,hrdept_master hrm where esm.staff_code=sfm.staff_code and sfm.staff_code=st.staff_code and st.dept_code=hrm.dept_code and st.latestrec='1'  and isexternal='0' and Inivigition='1' and esm.Type='" + ddltype.SelectedItem.Text + "' and sfm.staff_code not in(select distinct invigilator_code from examinvigilatormaster ev,examstaffmaster esm where esm.staff_code=ev.invigilator_code and esm.Inivigition='1' and esm.Type='" + ddltype.SelectedItem.Text + "' and isexternal='0' and ev.year='" + ddlYear.SelectedItem.Text + "' and ev.month='" + ddlMonth.SelectedValue + "' and ev.edate='" + dtt + "') order by  invigilator_name,sfm.staff_code";
            dt = dir.selectDataTable(sqlstafflist);
            ddlAddStaffDetails.DataSource = dt;
            ddlAddStaffDetails.DataTextField = "invigilator_name";
            ddlAddStaffDetails.DataValueField = "staff_code";
            ddlAddStaffDetails.DataBind();
        }
        catch
        {

        }

    }

    protected void btnAddStaffClose_Click(object sender, EventArgs e)
    {
        divAddStaff.Visible = false;
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            if (lblAlertMsg.ToolTip == "staffadded")
            {
                divAddStaff.Visible = false;
                btngo_Click(sender, e);
            }
            else if (lblAlertMsg.ToolTip == "staffmoved")
            {
                divMoveStaff.Visible = false;
                btngo_Click(sender, e);
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnDeletestaff_OnClick(object sender,EventArgs e)
    {
        string selectedDate = lblDeleteDateVal.Text;
        string selectedHall = lblDeleteHallVal.Text;
        string selectedSession = lblDeleteSessionVal.Text;
        string selectedstaff = ddldeletestaffVal.SelectedItem.ToString();
        string selectedstaffcode = ddldeletestaffVal.SelectedValue;

        DateTime dtsel = Convert.ToDateTime(selectedDate);
        //DateTime.TryParseExact(selectedDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtsel);

        string delqry = "if exists(select * from examinvigilatormaster where roomno = '" + selectedHall + "'  and edate ='" + dtsel.ToString("MM/dd/yyyy") + "' and invigilator_code='" + selectedstaffcode.Trim() + "')";
        delqry = delqry + " delete from examinvigilatormaster where roomno = '" + selectedHall + "'  and edate ='" + dtsel.ToString("MM/dd/yyyy") + "' and invigilator_code='" + selectedstaffcode.Trim() + "'";

        int res = dir.deleteData(delqry);

        if (res > 0)
        {
            divPopAlert.Visible = true;
            lblAlertMsg.Text = "Removed Successfully";
            divDeleteStaff.Visible = false;
            btngo_Click(sender, e);
            cellClicked = true;
        }

    }

    protected void btnDeletestaffCancel_OnClick(object sender, EventArgs e)
    {
        divDeleteStaff.Visible = false;
        lblDeleteHallVal.Text = string.Empty;
        lblDeleteDateVal.Text = string.Empty;
        lblDeleteSessionVal.Text = string.Empty;
        ddldeletestaffVal.Items.Clear();
    }

    #endregion

}