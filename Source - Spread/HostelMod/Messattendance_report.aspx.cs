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
using FarPoint.Web.Spread;
public partial class HostelMod_Messattendance_report : System.Web.UI.Page
{
    SqlConnection myconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection myconn1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlDataAdapter da = new SqlDataAdapter();
    DataSet ds = new DataSet();
    DataSet ds3 = new DataSet();
    DAccess2 d2 = new DAccess2();
    ReuasableMethods rs = new ReuasableMethods();
    DataSet dsbind = new DataSet();
    DataSet dset = new DataSet();
    static int seatcnt = 0;
    Boolean cellclick = false;
    string s = "";
    [Serializable()]
    public class MyImg : ImageCellType
    {
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(75);
            return img;
        }
    }

    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);
    }
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
        if (!IsPostBack)
        {
            //string today = System.DateTime.Now.ToString();
            //string today1;
            //string[] split13 = today.Split(new char[] { ' ' });
            //string[] split14 = split13[0].Split(new Char[] { '/' });
            //today1 = "01" + "/" + split14[0].ToString() + "/" + split14[2].ToString();
            //Txtentryfrom.Text = today1;
            //string today2 = System.DateTime.Now.ToString();
            //string today3;
            //string[] split15 = today.Split(new char[] { ' ' });
            //string[] split16 = split13[0].Split(new Char[] { '/' });
            //today3 = split16[1].ToString() + "/" + split16[0].ToString() + "/" + split16[2].ToString();
            //Txtentryto.Text = today3;
            DateTime stdate;
            string today = System.DateTime.Now.ToString();
            string today1;

            string[] split13 = today.Split(new char[] { ' ' });
            string[] split14 = split13[0].Split(new Char[] { '/' });
            today1 = split14[1].ToString() + "/" + split14[0].ToString() + "/" + split14[2].ToString();

            string today2 = System.DateTime.Now.ToString();
            stdate = System.DateTime.Today.AddDays(-9);
            string today3;
            today3 = stdate.Date.ToShortDateString();
            // today3 =Convert.ToString( day9.AddDays(30));
            string[] split15 = today3.Split(new char[] { ' ' });
            string[] split16 = split15[0].Split(new Char[] { '/' });
            today3 = split16[1].ToString() + "/" + split16[0].ToString() + "/" + split16[2].ToString();
            Txtentryfrom.Text = System.DateTime.Today.AddDays(-9).ToString("dd/MM/yyyy");
            Txtentryto.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            fpmessreport.Visible = false;
            Fpcumulative.Visible = false;
            load_student();
            load_hostelname();
            load_session();
            load_click();
        }
    }
    void load_student()
    {
        cbostudent.Items.Clear();
        ds.Clear();
        //ListItem lsitem = new ListItem();
        con.Open();
        SqlCommand cmd = new SqlCommand("select distinct r.roll_no,r.stud_name from hostel_studentdetails h,registration r where h.roll_admit=r.roll_admit ", con);
        da.SelectCommand = cmd;
        da.Fill(ds);
        cbostudent.DataSource = ds.Tables[0];
        cbostudent.DataTextField = "stud_name";
        cbostudent.DataValueField = "roll_no";
        cbostudent.DataBind();
        cbostudent.Items.Insert(0, "All");
        con.Close();
    }
    void load_session()
    {
        cbosession.Items.Clear();
        ds.Clear();
        string hostel = rs.GetSelectedItemsValueAsString(Cbo_HostelName);
        ds = d2.BindSession_inv(hostel);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbosession.DataSource = ds.Tables[0];
            cbosession.DataTextField = "sessionname";
            cbosession.DataValueField = "SessionMasterPK";
            cbosession.DataBind();
        }
        for (int i = 0; i < cbosession.Items.Count; i++)
        {
            cbosession.Items[i].Selected = true;
        }
        txtsession.Text = "SessionName (" + cbosession.Items.Count + ")";
    }
    void load_hostelname()
    {
        ds.Clear();
        string selectQuery = d2.GetFunction("select value from Master_Settings where settings='Mess Rights'  and usercode='" + Session["usercode"].ToString() + "' and value<>''");
        if (selectQuery.Trim() != "0" && selectQuery.Trim() != "")
        {
            string selectQuery1 = " select HostelName,HostelMasterPK  from HM_HostelMaster where MessMasterFK in(" + selectQuery + ") and MessMasterFK is not null";
            ds = d2.select_method_wo_parameter(selectQuery1, "text");
            Cbo_HostelName.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                Cbo_HostelName.DataSource = ds;
                Cbo_HostelName.DataTextField = "HostelName";
                Cbo_HostelName.DataValueField = "HostelMasterPK";
                Cbo_HostelName.DataBind();
            }
            for (int i = 0; i < Cbo_HostelName.Items.Count; i++)
            {
                Cbo_HostelName.Items[i].Selected = true;
            }
            tbseattype.Text = "HostelName (" + Cbo_HostelName.Items.Count + ")";
        }
    }
    protected void Txtentryfrom_TextChanged(object sender, EventArgs e)
    {
        //  string[] split154 = Txtentryfrom.Text.Split(new Char[] { '/' });
        //  string today1 = split154[1].ToString() + "/" + split154[0].ToString() + "/" + split154[2].ToString();
        //  //Txtentryfrom.Text = today1;
        //  string datefrom6;
        //  DateTime stdate6;
        //  string today10 = "";
        //  datefrom6 = Txtentryfrom.Text;
        //  string datefrom1 = "";
        //  string dateto = "";
        //  string dateto5 = "";
        //  DateTime strdate7;
        //  DateTime dtaefrom2;
        //  dtaefrom2 = Convert.ToDateTime(today1);
        //  DateTime stdate1;
        //  datefrom1 = Txtentryfrom.Text;
        //  //stdate1= dtaefrom2.AddDays(30);
        //  // datefrom1 = stdate1.Date.ToShortDateString();
        //  string[] split14 = datefrom1.Split(new Char[] { '/' });
        //  today10 = split14[1].ToString() + "/" + split14[0].ToString() + "/" + split14[2].ToString();
        //  stdate1 = Convert.ToDateTime(today10);
        //  stdate6 = stdate1.AddDays (9);
        //// stdate6 = stdate1.AddMonths(1);
        // // strdate7 = stdate6.AddDays(-1);
        //  dateto = stdate6.ToString();
        //  string[] split13 = dateto.Split(new char[] { ' ' });
        //  string[] split22 = split13[0].Split(new Char[] { '/' });
        //  dateto5 = split22[1].ToString() + "/" + split22[0].ToString() + "/" + split22[2].ToString();
        //  Txtentryto.Text = dateto5.ToString();
    }
    protected void rdoall_CheckedChanged(object sender, EventArgs e)
    {
        if (rdoall.Checked == true)
        {
            cbostudent.Visible = false;
        }
    }
    protected void rdostudent_CheckedChanged(object sender, EventArgs e)
    {
        if (rdostudent.Checked == true)
        {
            cbostudent.Visible = true;
        }
    }
    protected void Cbo_HostelName_SelectedIndexChanged1(object sender, EventArgs e)
    {
        pseattype.Focus();
        int seatcount = 0;
        string value = "";
        string code = "";


        for (int i = 0; i < Cbo_HostelName.Items.Count; i++)
        {
            if (Cbo_HostelName.Items[i].Selected == true)
            {
                value = Cbo_HostelName.Items[i].Text;
                code = Cbo_HostelName.Items[i].Value.ToString();
                seatcount = seatcount + 1;
                tbseattype.Text = "HostelName(" + seatcount.ToString() + ")";
            }

        }

        if (seatcount == 0)
            tbseattype.Text = "---Select---";
        else
        {

        }
        seatcnt = seatcount;
    }
    protected void chkselect_CheckedChanged(object sender, EventArgs e)
    {
        if (chkselect.Checked == true)
        {
            for (int i = 0; i < Cbo_HostelName.Items.Count; i++)
            {
                Cbo_HostelName.Items[i].Selected = true;
                tbseattype.Text = "HostelName(" + (Cbo_HostelName.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < Cbo_HostelName.Items.Count; i++)
            {
                Cbo_HostelName.Items[i].Selected = false;
                tbseattype.Text = "---Select---";
            }
        }
    }


    protected void Txtentryto_TextChanged(object sender, EventArgs e)
    {

    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        if (cbosession.Items.Count > 0 && Cbo_HostelName.Items.Count > 0)
        {
            if (txtsession.Text.Trim() != "---Select---" && tbseattype.Text.Trim() != "---Select---")
            {
                lblrecord.Text = "     Records Per Page"; lblrecord.ForeColor = Color.Black;
                load_click();
            }
            else
            {
                lblrecord.Visible = true;
                Fpcumulative.Visible = false;
                fpmessreport.Visible = false; Buttontotal.Visible = false;
                if (txtsession.Text.Trim() == "---Select---")
                    lblrecord.Text = "Please Select Session Name ";
                if (tbseattype.Text.Trim() == "---Select---")
                    lblrecord.Text = "Please Select Hostel Name";
                if (txtsession.Text.Trim() == "---Select---" && tbseattype.Text.Trim() == "---Select---")
                    lblrecord.Text = "Please Select SessionName and Hostel Name ";
                DropDownListpage.Visible = false; TextBoxother.Visible = false;
                lblrecord.ForeColor = Color.Red; lblpage.Visible = false; TextBoxpage.Visible = false;
            }
        }
        else
        {
            Fpcumulative.Visible = false; lblrecord.Visible = true; TextBoxpage.Visible = false;
            fpmessreport.Visible = false; Buttontotal.Visible = false; DropDownListpage.Visible = false; TextBoxother.Visible = false;
            if (Cbo_HostelName.Items.Count > 0)
                lblrecord.Text = "Please Create Hostel Name";
            else
                lblrecord.Text = "Please Create Session Name";
            lblpage.Visible = false;
            lblrecord.ForeColor = Color.Red;
        }

    }
    void load_click()
    {
        lblstudent.Visible = false;
        fpmessreport.Visible = true;
        fpmessreport.Sheets[0].ColumnCount = 0;
        fpmessreport.Sheets[0].RowCount = 0;
        Fpcumulative.Visible = true;

        fpmessreport.Sheets[0].PageSize = 11;
        fpmessreport.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
        fpmessreport.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
        fpmessreport.Pager.Align = HorizontalAlign.Right;

        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle.ForeColor = Color.White;
        fpmessreport.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
        fpmessreport.Sheets[0].SheetCorner.DefaultStyle = darkstyle;

        fpmessreport.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
        fpmessreport.Sheets[0].SheetCorner.Cells[0, 0].Font.Name = "Book Antiqua";
        fpmessreport.Sheets[0].SheetCorner.Cells[0, 0].Font.Bold = true;
        fpmessreport.Sheets[0].SheetCorner.Cells[0, 0].Font.Size = FontUnit.Medium;
        fpmessreport.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

        fpmessreport.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        fpmessreport.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        fpmessreport.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
        fpmessreport.Pager.Font.Bold = true;
        fpmessreport.Pager.Font.Name = "Arial";
        //fpmessreport.Pager.ForeColor = Color.DarkGreen;
        //fpmessreport.Pager.BackColor = Color.AliceBlue;
        fpmessreport.Pager.PageCount = 5;
        fpmessreport.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        fpmessreport.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
        fpmessreport.Sheets[0].SheetCorner.RowCount = 2;
        fpmessreport.Sheets[0].ColumnCount = 1;
        fpmessreport.Sheets[0].AutoPostBack = true;
        fpmessreport.Visible = true;

        Buttontotal.Visible = true;
        DropDownListpage.Visible = true;
        TextBoxpage.Visible = true;
        lblrecord.Visible = true;
        lblrecord.Visible = true;
        lblpage.Visible = true;
        lblnorec.Visible = false;
        lblexceedpage.Visible = false;

        fpmessreport.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Date";
        fpmessreport.Sheets[0].Columns[0].Font.Underline = true;
        fpmessreport.Sheets[0].Columns[0].ForeColor = Color.Blue;
        fpmessreport.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
        string datefrom3;
        int monthname1;
        string date3 = Txtentryfrom.Text.ToString();
        string monthname2 = "";
        int monthnamenum;
        string yearto = "";
        string monyear = "";
        string monyearto = "";
        string[] split5 = date3.Split(new Char[] { '/' });
        //int  monyear;
        int dayfrm = 0;
        string dayto;
        int daytonum;
        int year3;

        datefrom3 = split5[1].ToString() + "/" + split5[0].ToString() + "/" + split5[2].ToString();
        year3 = Convert.ToInt16(split5[2]);
        string date4 = Txtentryto.Text.ToString();
        string[] split6 = date4.Split(new Char[] { '/' });
        string dateto4 = split6[1].ToString() + "/" + split6[0].ToString() + "/" + split6[2].ToString();
        monthname2 = split6[0].ToString();
        monthnamenum = Convert.ToInt32(monthname2.ToString());
        yearto = split5[2].ToString();
        int fromday = 0;
        fromday = Convert.ToInt16(split6[0].ToString());
        string mnmae = split5[1].ToString();
        monthname1 = Convert.ToInt16(mnmae);
        monthname2 = split6[1].ToString();
        monthnamenum = Convert.ToInt16(monthname2);
        year3 = Convert.ToInt16(split5[2].ToString());

        fpmessreport.ActiveSheetView.Columns[0].Font.Size = FontUnit.Medium;
        fpmessreport.ActiveSheetView.Columns[0].Font.Name = "Book Antiqua";

        string year = split5[2].ToString();

        monyear = year + "/" + monthname1.ToString();
        monyearto = yearto + "/" + monthnamenum.ToString() + "/";

        string dayfrom;
        int today = 0;
        today = Convert.ToInt16(split6[0].ToString());

        dayfrom = split5[0].ToString();
        dayfrm = Convert.ToInt32(dayfrom);
        dayto = split6[0].ToString();
        daytonum = Convert.ToInt32(dayto);
        int rowtstr = 0;

        string rollno = "";
        int counttotal = 0;
        string total = "";

        string date1 = "";
        string datefrom = "";
        string date2 = "";
        string dateto = "";

        date1 = Txtentryfrom.Text.ToString();
        string[] split = date1.Split(new Char[] { '/' });
        datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
        date2 = Txtentryto.Text.ToString();
        string[] split1 = date2.Split(new Char[] { '/' });
        dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();

        string q1 = ""; string messcode = "";
        string hostelcodenum = rs.GetSelectedItemsValue(Cbo_HostelName);
        q1 = " select MessMasterFK from HM_HostelMaster where MessMasterFK is not null and HostelMasterPK in(" + hostelcodenum + ")";
        ds.Clear();
        ds = d2.select_method_wo_parameter(q1, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            messcode = "";
            for (int sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
            {
                if (messcode.Trim() == "")
                {
                    messcode = Convert.ToString(ds.Tables[0].Rows[sel][0]);
                }
                else
                {
                    messcode += "," + Convert.ToString(ds.Tables[0].Rows[sel][0]);
                }
            }
        }

        if (Convert.ToInt16(split[0].ToString()) <= 31 && Convert.ToInt16(split[1].ToString()) <= 12)
        {
            if (Convert.ToInt16(split1[0].ToString()) <= 31 && Convert.ToInt16(split1[1].ToString()) <= 12)
            {
                long days = -1;
                DateTime dt1 = DateTime.Now.AddDays(-9);
                DateTime dt2 = DateTime.Now;
                try
                {
                    dt1 = Convert.ToDateTime(datefrom.ToString());
                    dt2 = Convert.ToDateTime(dateto.ToString());
                    TimeSpan t = dt2.Subtract(dt1);
                    days = t.Days;
                }

                catch
                {
                    try
                    {
                        dt1 = Convert.ToDateTime(date1);
                        dt2 = Convert.ToDateTime(date2);
                        TimeSpan t = dt2.Subtract(dt1);
                        days = t.Days;
                    }
                    catch
                    {
                    }
                }
                if (days >= 0)
                {
                    lblmistake.Visible = false;
                    Fpcumulative.Sheets[0].PageSize = 11;
                    Fpcumulative.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                    Fpcumulative.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                    Fpcumulative.Pager.Align = HorizontalAlign.Right;

                    Fpcumulative.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    Fpcumulative.Sheets[0].SheetCorner.DefaultStyle = darkstyle;

                    Fpcumulative.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                    Fpcumulative.Sheets[0].SheetCorner.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpcumulative.Sheets[0].SheetCorner.Cells[0, 0].Font.Bold = true;
                    Fpcumulative.Sheets[0].SheetCorner.Cells[0, 0].Font.Size = FontUnit.Medium;

                    Fpcumulative.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                    Fpcumulative.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                    Fpcumulative.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                    Fpcumulative.Pager.Font.Bold = true;
                    Fpcumulative.Pager.Font.Name = "Arial";
                    //Fpcumulative.Pager.ForeColor = Color.DarkGreen;
                    //Fpcumulative.Pager.BackColor = Color.AliceBlue;
                    Fpcumulative.Pager.PageCount = 5;
                    Fpcumulative.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                    Fpcumulative.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;

                    string fromdate = Txtentryfrom.Text;

                    string todate = Txtentryto.Text;
                    string[] spplitfrmdate = fromdate.Split('/');
                    string frmdate3 = spplitfrmdate[1] + "/" + spplitfrmdate[0] + "/" + spplitfrmdate[2];
                    Fpcumulative.Sheets[0].AutoPostBack = true;
                    string[] splittodate = todate.Split('/');
                    string todate4 = splittodate[1] + "/" + splittodate[0] + "/" + splittodate[2];
                    Fpcumulative.Sheets[0].RowCount = 0;
                    Fpcumulative.Sheets[0].ColumnCount = 0;
                    Fpcumulative.Sheets[0].ColumnCount = 1;
                    Fpcumulative.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Type";
                    Fpcumulative.ActiveSheetView.Columns[0].Font.Size = FontUnit.Medium;
                    Fpcumulative.ActiveSheetView.Columns[0].Font.Name = "Book Antiqua";
                    Fpcumulative.Sheets[0].SheetCorner.Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    Fpcumulative.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    string sql4 = "";
                    s = "";

                    sql4 = " select distinct SessionMasterPK,SessionName from HM_SessionMaster where MessMasterFK in(" + messcode + ") order by SessionMasterPK";
                    DataSet ds7 = new DataSet();
                    ds7 = d2.select_method_wo_parameter(sql4, "text");
                    int colcountcu = 1;
                    int countsession = ds7.Tables[0].Rows.Count;
                    if (countsession > 0)
                    {
                        for (int i = 0; i < cbosession.Items.Count; i++)
                        {
                            if (cbosession.Items[i].Selected == true)
                            {
                                Fpcumulative.Sheets[0].ColumnCount = Fpcumulative.Sheets[0].ColumnCount + 1;
                                string session = ds7.Tables[0].Rows[i]["SessionName"].ToString();
                                Fpcumulative.Sheets[0].ColumnHeader.Cells[0, colcountcu].Text = session;
                                Fpcumulative.Sheets[0].ColumnHeader.Cells[0, colcountcu].Tag = ds7.Tables[0].Rows[i]["SessionMasterPK"].ToString();
                                Fpcumulative.Sheets[0].ColumnHeader.Cells[0, colcountcu].HorizontalAlign = HorizontalAlign.Center;
                                Fpcumulative.ActiveSheetView.Columns[colcountcu].Font.Size = FontUnit.Medium;
                                Fpcumulative.ActiveSheetView.Columns[colcountcu].Font.Name = "Book Antiqua";
                                colcountcu = Fpcumulative.Sheets[0].ColumnCount;
                            }
                            else
                            {
                                Fpcumulative.Sheets[0].ColumnCount = Fpcumulative.Sheets[0].ColumnCount + 1;
                                string session = ds7.Tables[0].Rows[i]["SessionName"].ToString();
                                Fpcumulative.Sheets[0].ColumnHeader.Cells[0, colcountcu].Text = session;
                                Fpcumulative.Sheets[0].ColumnHeader.Cells[0, colcountcu].Tag = session;
                                Fpcumulative.Sheets[0].ColumnHeader.Cells[0, colcountcu].HorizontalAlign = HorizontalAlign.Center;
                                Fpcumulative.Sheets[0].Columns[colcountcu].Visible = false;
                                colcountcu = Fpcumulative.Sheets[0].ColumnCount;
                            }
                        }
                    }
                    string hostelcode = ""; string session1 = "";
                    if (tbseattype.Text != "---Select---")
                    {
                        hostelcode = rs.GetSelectedItemsValueAsString(Cbo_HostelName);
                    }
                    if (cbotype.SelectedItem.Text == "Both")
                    {
                        for (int h = 1; h < cbotype.Items.Count; h++)
                        {
                            int rowtsr = Fpcumulative.Sheets[0].RowCount++;
                            if (cbotype.Items[h].Value == "Absent")
                            {
                                for (int col = 1; col <= Fpcumulative.Sheets[0].ColumnCount - 1; col++)
                                {
                                    string totalabsent1 = "";
                                    int totalabsent = 0;
                                    if (monthname1 == monthnamenum)
                                    {
                                        for (int day = dayfrm; day <= daytonum; day++)
                                        {
                                            session1 = Fpcumulative.Sheets[0].ColumnHeader.Cells[0, col].Tag.ToString();
                                            s = "";
                                            s = " SELECT Count(*) tot FROM HT_HostelRegistration hsd,registration r  where  hsd.APP_No =r.App_No and isnull( hsd.IsVacated,0)=0 and isnull(hsd.IsSuspend,0)=0 ";

                                            if (tbseattype.Text != "---Select---")
                                            {
                                                s = s + "  and hsd.HostelMasterFK in('" + hostelcode + "')";
                                            }
                                            s = s + " and  r.roll_no not in";
                                            string date = spplitfrmdate[2] + "/" + spplitfrmdate[1] + "/" + day;
                                            s = s + " (select roll_no from HostelMess_Attendance where Entry_Date ='" + date + "' ";
                                            if (tbseattype.Text != "---Select---")
                                            {
                                                s = s + "  and hostel_code in('" + hostelcode + "')";
                                            }
                                            s = s + " and Session_Code in('" + session1 + "'))";
                                            ds3.Clear();
                                            ds3 = d2.select_method_wo_parameter(s, "text");
                                            int countabsent = ds3.Tables[0].Rows.Count;
                                            if (countabsent > 0)
                                            {
                                                totalabsent1 = ds3.Tables[0].Rows[0]["tot"].ToString();
                                                totalabsent = Convert.ToInt16(totalabsent1) + totalabsent;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        int maxdays = getmaxdays(monthname1, year3);
                                        for (int day = dayfrm; day <= maxdays; day++)
                                        {
                                            session1 = "";
                                            session1 = Fpcumulative.Sheets[0].ColumnHeader.Cells[0, col].Tag.ToString();
                                            s = "";
                                            s = " SELECT Count(*) tot FROM HT_HostelRegistration hsd,registration r  where  hsd.APP_No =r.App_No and isnull( hsd.IsVacated,0)=0 and isnull(hsd.IsSuspend,0)=0 ";
                                            if (tbseattype.Text != "---Select---")
                                            {
                                                s = s + "  and hsd.HostelMasterFK in(" + hostelcode + ")";
                                            }
                                            s = s + " and  r.roll_no not in";
                                            string date = spplitfrmdate[2] + "/" + spplitfrmdate[1] + "/" + day;
                                            s = s + " (select roll_no from HostelMess_Attendance";
                                            s = s + " where Entry_Date ='" + date + "' ";
                                            if (tbseattype.Text != "---Select---")
                                            {
                                                s = s + "  and hostel_code in(" + hostelcode + ")";
                                            }
                                            s = s + " and session_Code='" + session1 + "') ";
                                            ds3.Clear();
                                            ds3 = d2.select_method_wo_parameter(s, "text");
                                            int countabsent = ds3.Tables[0].Rows.Count;
                                            if (countabsent > 0)
                                            {
                                                totalabsent1 = ds3.Tables[0].Rows[0]["tot"].ToString();
                                                totalabsent = Convert.ToInt16(totalabsent1) + totalabsent;
                                            }
                                        }
                                        for (int day9 = 1; day9 <= daytonum; day9++)
                                        {
                                            session1 = "";
                                            session1 = Fpcumulative.Sheets[0].ColumnHeader.Cells[0, col].Tag.ToString();
                                            s = "";
                                            s = " SELECT Count(*) tot FROM HT_HostelRegistration hsd,registration r  where  hsd.APP_No =r.App_No and isnull( hsd.IsVacated,0)=0 and isnull(hsd.IsSuspend,0)=0  ";
                                            string strbuild = "";
                                            if (tbseattype.Text != "---Select---")
                                            {
                                                s = s + "  and hsd.HostelMasterFK in(" + hostelcode + ")";
                                            }
                                            s = s + " and  r.roll_no not in";
                                            string date = splittodate[2] + "/" + splittodate[1] + "/" + day9;
                                            s = s + " (select roll_no from HostelMess_Attendance";
                                            s = s + " where Entry_Date ='" + date + "' ";
                                            if (tbseattype.Text != "---Select---")
                                            {
                                                s = s + "  and hostel_code" + strbuild + "";
                                            }
                                            s = s + " and session_Code='" + session1 + "') ";
                                            ds3.Clear();
                                            ds3 = d2.select_method_wo_parameter(s, "text");
                                            int countabsent = ds3.Tables[0].Rows.Count;
                                            if (countabsent > 0)
                                            {
                                                totalabsent1 = ds3.Tables[0].Rows[0]["tot"].ToString();
                                                totalabsent = Convert.ToInt16(totalabsent1) + totalabsent;
                                            }
                                        }
                                    }
                                    Fpcumulative.Sheets[0].Cells[rowtsr, 0].Text = cbotype.Items[h].Value.ToString();
                                    Fpcumulative.Sheets[0].Cells[rowtsr, col].Text = totalabsent.ToString();
                                    Fpcumulative.Sheets[0].Cells[rowtsr, col].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                            if (cbotype.Items[h].Value == "Present")
                            {
                                for (int col = 1; col <= Fpcumulative.Sheets[0].ColumnCount - 1; col++)
                                {
                                    string totalabsent1 = "";
                                    int totalabsent = 0;
                                    if (monthname1 == monthnamenum)
                                    {
                                        for (int day = dayfrm; day <= daytonum; day++)
                                        {
                                            session1 = "";
                                            session1 = Fpcumulative.Sheets[0].ColumnHeader.Cells[0, col].Tag.ToString();
                                            s = "";
                                            s = "SELECT Count(*) tot FROM HT_HostelRegistration hsd,registration r  where  hsd.APP_No =r.App_No and isnull( hsd.IsVacated,0)=0 and isnull(hsd.IsSuspend,0)=0  ";
                                            if (tbseattype.Text != "---Select---")
                                            {
                                                s = s + "  and hsd.HostelMasterFK in('" + hostelcode + "')";
                                            }
                                            s = s + " and  r.roll_no  in";
                                            string date = spplitfrmdate[2] + "/" + spplitfrmdate[1] + "/" + day;
                                            s = s + " (select roll_no from HostelMess_Attendance where Entry_Date ='" + date + "' ";
                                            if (tbseattype.Text != "---Select---")
                                            {
                                                s = s + "  and hostel_code in('" + hostelcode + "')";
                                            }
                                            s = s + " and session_code='" + session1 + "') ";
                                            ds3.Clear();
                                            ds3 = d2.select_method_wo_parameter(s, "text");
                                            int countabsent = ds3.Tables[0].Rows.Count;
                                            if (countabsent > 0)
                                            {
                                                totalabsent1 = ds3.Tables[0].Rows[0]["tot"].ToString();
                                                totalabsent = Convert.ToInt16(totalabsent1) + totalabsent;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        int maxdays = getmaxdays(monthname1, year3);
                                        for (int day = dayfrm; day <= maxdays; day++)
                                        {

                                            session1 = Fpcumulative.Sheets[0].ColumnHeader.Cells[0, col].Tag.ToString();
                                            s = "";
                                            s = "SELECT Count(*) tot FROM HT_HostelRegistration hsd,registration r  where  hsd.APP_No =r.App_No and isnull( hsd.IsVacated,0)=0 and isnull(hsd.IsSuspend,0)=0  ";

                                            if (tbseattype.Text != "---Select---")
                                            {
                                                s = s + "  and hsd.HostelMasterFK in('" + hostelcode + "')";
                                            }
                                            s = s + " and  r.roll_no  in";
                                            string date = spplitfrmdate[2] + "/" + spplitfrmdate[1] + "/" + day;
                                            s = s + " (select roll_no from HostelMess_Attendance";
                                            s = s + " where Entry_Date ='" + date + "' ";
                                            if (tbseattype.Text != "---Select---")
                                            {
                                                s = s + "  and hostel_code in('" + hostelcode + "')";
                                            }
                                            s = s + " and session_code='" + session1 + "') ";
                                            ds3.Clear();
                                            ds3 = d2.select_method_wo_parameter(s, "text");
                                            int countabsent = ds3.Tables[0].Rows.Count;
                                            if (countabsent > 0)
                                            {
                                                totalabsent1 = ds3.Tables[0].Rows[0]["tot"].ToString();
                                                totalabsent = Convert.ToInt16(totalabsent1) + totalabsent;
                                            }
                                        }
                                        for (int day9 = 1; day9 <= daytonum; day9++)
                                        {
                                            session1 = "";
                                            session1 = Fpcumulative.Sheets[0].ColumnHeader.Cells[0, col].Tag.ToString();
                                            s = "";
                                            s = "SELECT Count(*) tot FROM HT_HostelRegistration hsd,registration r  where  hsd.APP_No =r.App_No and isnull( hsd.IsVacated,0)=0 and isnull(hsd.IsSuspend,0)=0  ";
                                            if (tbseattype.Text != "---Select---")
                                            {
                                                s = s + "  and hsd.HostelMasterFK in('" + hostelcode + "')";
                                            }
                                            s = s + " and  r.roll_no  in";
                                            string date = splittodate[2] + "/" + splittodate[1] + "/" + day9;
                                            s = s + " (select roll_no from HostelMess_Attendance where Entry_Date ='" + date + "' ";
                                            if (tbseattype.Text != "---Select---")
                                            {
                                                s = s + "  and hostel_code in('" + hostelcode + "')";
                                            }
                                            s = s + " and session_code='" + session1 + "') ";
                                            ds3.Clear();
                                            ds3 = d2.select_method_wo_parameter(s, "text");
                                            int countabsent = ds3.Tables[0].Rows.Count;
                                            if (countabsent > 0)
                                            {
                                                totalabsent1 = ds3.Tables[0].Rows[0]["tot"].ToString();
                                                totalabsent = Convert.ToInt16(totalabsent1) + totalabsent;
                                            }
                                        }
                                    }
                                    Fpcumulative.Sheets[0].Cells[rowtsr, 0].Text = cbotype.Items[h].Value.ToString();
                                    Fpcumulative.Sheets[0].Cells[rowtsr, col].Text = totalabsent.ToString();
                                    Fpcumulative.Sheets[0].Cells[rowtsr, col].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                        }
                    }
                    if (cbotype.SelectedItem.Value.ToString() == "Present")
                    {
                        int rowstr = Fpcumulative.Sheets[0].RowCount++;
                        for (int col = 1; col <= Fpcumulative.Sheets[0].ColumnCount - 1; col++)
                        {
                            string totalabsent1 = "";
                            int totalabsent = 0;
                            if (monthname1 == monthnamenum)
                            {
                                for (int day = dayfrm; day <= daytonum; day++)
                                {
                                    session1 = "";
                                    session1 = Fpcumulative.Sheets[0].ColumnHeader.Cells[0, col].Tag.ToString();
                                    s = "";
                                    s = "SELECT Count(*) tot FROM HT_HostelRegistration hsd,registration r  where  hsd.APP_No =r.App_No and isnull( hsd.IsVacated,0)=0 and isnull(hsd.IsSuspend,0)=0  ";
                                    if (tbseattype.Text != "---Select---")
                                    {
                                        s = s + "  and hsd.HostelMasterFK in('" + hostelcode + "')";
                                    }
                                    s = s + " and  r.roll_no  in";
                                    string date = spplitfrmdate[2] + "/" + spplitfrmdate[1] + "/" + day;
                                    s = s + " (select roll_no from HostelMess_Attendance";
                                    s = s + " where Entry_Date ='" + date + "' ";
                                    if (tbseattype.Text != "---Select---")
                                    {
                                        s = s + "  and hostel_code in('" + hostelcode + "')";
                                    }
                                    s = s + " and session_Code='" + session1 + "') ";
                                    ds3.Clear();
                                    ds3 = d2.select_method_wo_parameter(s, "text");
                                    int countabsent = ds3.Tables[0].Rows.Count;
                                    if (countabsent > 0)
                                    {
                                        totalabsent1 = ds3.Tables[0].Rows[0]["tot"].ToString();
                                        totalabsent = Convert.ToInt16(totalabsent1) + totalabsent;
                                    }
                                }
                            }
                            else
                            {
                                int maxdays = getmaxdays(monthname1, year3);
                                for (int day = dayfrm; day <= maxdays; day++)
                                {
                                    session1 = "";
                                    session1 = Fpcumulative.Sheets[0].ColumnHeader.Cells[0, col].Tag.ToString();
                                    s = "";
                                    s = "SELECT Count(*) tot FROM HT_HostelRegistration hsd,registration r  where  hsd.APP_No =r.App_No and isnull( hsd.IsVacated,0)=0 and isnull(hsd.IsSuspend,0)=0  ";
                                    if (tbseattype.Text != "---Select---")
                                    {
                                        s = s + "  and hsd.HostelMasterFK in('" + hostelcode + "')";
                                    }
                                    s = s + " and  r.roll_no  in";
                                    string date = spplitfrmdate[2] + "/" + spplitfrmdate[1] + "/" + day;
                                    s = s + " (select roll_no from HostelMess_Attendance where Entry_Date ='" + date + "' ";
                                    if (tbseattype.Text != "---Select---")
                                    {
                                        s = s + "  and hostel_code in('" + hostelcode + "')";
                                    }
                                    s = s + " and session_Code='" + session1 + "') ";
                                    ds3.Clear();
                                    ds3 = d2.select_method_wo_parameter(s, "text");
                                    int countabsent = ds3.Tables[0].Rows.Count;
                                    if (countabsent > 0)
                                    {
                                        totalabsent1 = ds3.Tables[0].Rows[0]["tot"].ToString();
                                        totalabsent = Convert.ToInt16(totalabsent1) + totalabsent;
                                    }
                                }
                                for (int day9 = 1; day9 <= daytonum; day9++)
                                {
                                    session1 = "";
                                    session1 = Fpcumulative.Sheets[0].ColumnHeader.Cells[0, col].Tag.ToString();

                                    s = "";
                                    s = "SELECT Count(*) tot FROM HT_HostelRegistration hsd,registration r  where  hsd.APP_No =r.App_No and isnull( hsd.IsVacated,0)=0 and isnull(hsd.IsSuspend,0)=0  ";
                                    if (tbseattype.Text != "---Select---")
                                    {
                                        s = s + "  and hsd.HostelMasterFK in('" + hostelcode + "')";
                                    }
                                    s = s + " and  r.roll_no  in";
                                    string date = splittodate[2] + "/" + splittodate[1] + "/" + day9;

                                    s = s + " (select roll_no from HostelMess_Attendance";

                                    s = s + " where Entry_Date ='" + date + "' ";
                                    if (tbseattype.Text != "---Select---")
                                    {
                                        s = s + "  and hostel_code in (" + hostelcode + ")";
                                    }
                                    s = s + " and session_Code='" + session1 + "') ";

                                    ds3.Clear();
                                    ds3 = d2.select_method_wo_parameter(s, "text");
                                    int countabsent = ds3.Tables[0].Rows.Count;
                                    if (countabsent > 0)
                                    {
                                        totalabsent1 = ds3.Tables[0].Rows[0]["tot"].ToString();
                                        totalabsent = Convert.ToInt16(totalabsent1) + totalabsent;
                                    }
                                }
                            }
                            Fpcumulative.Sheets[0].Cells[rowstr, 0].Text = cbotype.SelectedItem.Value.ToString();
                            Fpcumulative.Sheets[0].Cells[rowstr, col].Text = totalabsent.ToString();
                            Fpcumulative.Sheets[0].Cells[rowstr, col].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    if (cbotype.SelectedItem.Value.ToString() == "Absent")
                    {
                        int rowstr = Fpcumulative.Sheets[0].RowCount++;
                        for (int col = 1; col <= Fpcumulative.Sheets[0].ColumnCount - 1; col++)
                        {
                            string totalabsent1 = "";
                            int totalabsent = 0;
                            if (monthname1 == monthnamenum)
                            {
                                for (int day = dayfrm; day <= daytonum; day++)
                                {
                                    session1 = "";
                                    session1 = Fpcumulative.Sheets[0].ColumnHeader.Cells[0, col].Tag.ToString();
                                    s = "";
                                    s = "SELECT Count(*) tot FROM HT_HostelRegistration hsd,registration r  where  hsd.APP_No =r.App_No and isnull( hsd.IsVacated,0)=0 and isnull(hsd.IsSuspend,0)=0  ";
                                    if (tbseattype.Text != "---Select---")
                                    {
                                        s = s + "  and hsd.HostelMasterFK in('" + hostelcode + "')";
                                    }
                                    s = s + " and  r.roll_no not  in";
                                    string date = spplitfrmdate[2] + "/" + spplitfrmdate[1] + "/" + day;

                                    s = s + " (select roll_no from HostelMess_Attendance";

                                    s = s + " where Entry_Date ='" + date + "' ";
                                    if (tbseattype.Text != "---Select---")
                                    {
                                        s = s + "  and hostel_code in('" + hostelcode + "')";
                                    }
                                    s = s + " and session_Code='" + session1 + "') ";
                                    ds3.Clear();
                                    ds3 = d2.select_method_wo_parameter(s, "text");
                                    int countabsent = ds3.Tables[0].Rows.Count;
                                    if (countabsent > 0)
                                    {
                                        totalabsent1 = ds3.Tables[0].Rows[0]["tot"].ToString();
                                        totalabsent = Convert.ToInt16(totalabsent1) + totalabsent;
                                    }
                                }
                            }
                            else
                            {
                                int maxdays = getmaxdays(monthname1, year3);
                                for (int day = dayfrm; day <= maxdays; day++)
                                {
                                    session1 = "";
                                    session1 = Fpcumulative.Sheets[0].ColumnHeader.Cells[0, col].Tag.ToString();
                                    s = "";
                                    s = "SELECT Count(*) tot FROM HT_HostelRegistration hsd,registration r  where  hsd.APP_No =r.App_No and isnull( hsd.IsVacated,0)=0 and isnull(hsd.IsSuspend,0)=0  ";
                                    if (tbseattype.Text != "---Select---")
                                    {
                                        s = s + "  and hsd.HostelMasterFK in('" + hostelcode + "')";
                                    }
                                    s = s + " and  r.roll_no not in";
                                    string date = spplitfrmdate[2] + "/" + spplitfrmdate[1] + "/" + day;
                                    s = s + " (select roll_no from HostelMess_Attendance where Entry_Date ='" + date + "' ";
                                    if (tbseattype.Text != "---Select---")
                                    {
                                        s = s + "  and hostel_code in(" + hostelcode + ")";

                                    }
                                    s = s + " and session_Code='" + session1 + "') ";
                                    ds3.Clear();
                                    ds3 = d2.select_method_wo_parameter(s, "text");
                                    int countabsent = ds3.Tables[0].Rows.Count;
                                    if (countabsent > 0)
                                    {
                                        totalabsent1 = ds3.Tables[0].Rows[0]["tot"].ToString();
                                        totalabsent = Convert.ToInt16(totalabsent1) + totalabsent;
                                    }
                                }
                                for (int day9 = 1; day9 <= daytonum; day9++)
                                {
                                    session1 = "";
                                    session1 = Fpcumulative.Sheets[0].ColumnHeader.Cells[0, col].Tag.ToString();
                                    s = "";
                                    s = "SELECT Count(*) tot FROM HT_HostelRegistration hsd,registration r  where  hsd.APP_No =r.App_No and isnull( hsd.IsVacated,0)=0 and isnull(hsd.IsSuspend,0)=0  ";
                                    if (tbseattype.Text != "---Select---")
                                    {
                                        s = s + "  and hsd.HostelMasterFK in('" + hostelcode + "')";
                                    }
                                    s = s + " and  r.roll_no not in";
                                    string date = splittodate[2] + "/" + splittodate[1] + "/" + day9;
                                    s = s + " (select roll_no from HostelMess_Attendance";
                                    s = s + " where Entry_Date ='" + date + "' ";
                                    if (tbseattype.Text != "---Select---")
                                    {
                                        s = s + "  and hostel_code in(" + hostelcode + ")";
                                    }
                                    s = s + " and session_Code='" + session1 + "') ";

                                    ds3.Clear();
                                    ds3 = d2.select_method_wo_parameter(s, "text");
                                    int countabsent = ds3.Tables[0].Rows.Count;
                                    if (countabsent > 0)
                                    {
                                        totalabsent1 = ds3.Tables[0].Rows[0]["tot"].ToString();
                                        totalabsent = Convert.ToInt16(totalabsent1) + totalabsent;
                                    }
                                }
                            }
                            Fpcumulative.Sheets[0].Cells[rowstr, 0].Text = cbotype.SelectedItem.Value.ToString();
                            Fpcumulative.Sheets[0].Cells[rowstr, col].Text = totalabsent.ToString();
                            Fpcumulative.Sheets[0].Cells[rowstr, col].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    /////////load header//////////////

                    // string s = "";


                    s = "select distinct SessionMasterPK,SessionName from HM_SessionMaster where MessMasterFK in(" + messcode + ") order by SessionMasterPK";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(s, "text");
                    int colcount = 1;
                    int colcount1 = 1;
                    int countds7 = ds.Tables[0].Rows.Count;
                    if (countds7 > 0)
                    {
                        for (int i = 0; i < countds7; i++)
                        {
                            if (cbosession.Items[i].Selected == true)
                            {
                                fpmessreport.Sheets[0].ColumnCount = fpmessreport.Sheets[0].ColumnCount + 5;
                                string session = ds7.Tables[0].Rows[i]["sessionname"].ToString();
                                fpmessreport.Sheets[0].ColumnHeader.Cells[0, colcount].Text = session;
                                fpmessreport.Sheets[0].ColumnHeader.Cells[0, colcount].Tag = Convert.ToString(ds7.Tables[0].Rows[i]["SessionMasterPK"]);
                                fpmessreport.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "Total";
                                if ((rdopercentage.Checked == true) && (cbotype.SelectedItem.Text == "Absent"))
                                {
                                    fpmessreport.Sheets[0].Columns[colcount + 1].Visible = true;

                                    fpmessreport.Sheets[0].Columns[colcount + 1].Font.Underline = true;
                                    fpmessreport.Sheets[0].Columns[colcount + 1].ForeColor = Color.Blue;

                                    fpmessreport.Sheets[0].Columns[colcount + 3].Font.Underline = true;
                                    fpmessreport.Sheets[0].Columns[colcount + 3].ForeColor = Color.Blue;


                                    fpmessreport.Sheets[0].ColumnHeader.Cells[1, colcount + 1].Text = "Absent(%)";
                                    fpmessreport.Sheets[0].ColumnHeader.Cells[1, colcount + 3].Text = "Present(%)";
                                    fpmessreport.Sheets[0].Columns[colcount + 3].Visible = false;
                                    fpmessreport.Sheets[0].Columns[colcount + 4].Visible = false;

                                }
                                else
                                {
                                    fpmessreport.Sheets[0].Columns[colcount + 1].Visible = false;
                                }
                                if ((rdocount.Checked == true) && (cbotype.SelectedItem.Text == "Absent"))
                                {
                                    fpmessreport.Sheets[0].Columns[colcount + 2].Visible = true;
                                    fpmessreport.Sheets[0].Columns[colcount + 4].Visible = false;

                                    fpmessreport.Sheets[0].Columns[colcount + 2].Font.Underline = true;
                                    fpmessreport.Sheets[0].Columns[colcount + 2].ForeColor = Color.Blue;

                                    fpmessreport.Sheets[0].Columns[colcount + 4].Font.Underline = true;
                                    fpmessreport.Sheets[0].Columns[colcount + 4].ForeColor = Color.Blue;



                                    fpmessreport.Sheets[0].ColumnHeader.Cells[1, colcount + 2].Text = "Absent";
                                    fpmessreport.Sheets[0].ColumnHeader.Cells[1, colcount + 4].Text = "Present";
                                    fpmessreport.Sheets[0].Columns[colcount + 4].Visible = false;
                                    fpmessreport.Sheets[0].Columns[colcount + 3].Visible = false;
                                }
                                else
                                {
                                    fpmessreport.Sheets[0].Columns[colcount + 2].Visible = false;
                                    //fpmessreport.Sheets[0].Columns[colcount + 4].Visible = false;
                                    //fpmessreport.Sheets[0].Columns[colcount + 3].Visible = false;

                                }
                                if ((rdocount.Checked == true) && (cbotype.SelectedItem.Text == "Present"))
                                {
                                    fpmessreport.Sheets[0].ColumnHeader.Cells[1, colcount + 4].Text = "Present";
                                    fpmessreport.Sheets[0].Columns[colcount + 4].Font.Underline = true;
                                    fpmessreport.Sheets[0].Columns[colcount + 4].ForeColor = Color.Blue;


                                    fpmessreport.Sheets[0].Columns[colcount + 4].Visible = true;
                                    fpmessreport.Sheets[0].Columns[colcount + 3].Visible = false;

                                }
                                if ((rdopercentage.Checked == true) && (cbotype.SelectedItem.Text == "Present"))
                                {
                                    fpmessreport.Sheets[0].ColumnHeader.Cells[1, colcount + 3].Text = "Present(%)";
                                    fpmessreport.Sheets[0].Columns[colcount + 3].Font.Underline = true;
                                    fpmessreport.Sheets[0].Columns[colcount + 3].ForeColor = Color.Blue;


                                    fpmessreport.Sheets[0].Columns[colcount + 4].Visible = false;
                                    fpmessreport.Sheets[0].Columns[colcount + 3].Visible = true;

                                }
                                if ((cbotype.SelectedItem.Text == "Both") && (rdopercentage.Checked == true))
                                {
                                    fpmessreport.Sheets[0].ColumnHeader.Cells[1, colcount + 1].Text = "Absent(%)";
                                    fpmessreport.Sheets[0].ColumnHeader.Cells[1, colcount + 3].Text = "Present(%)";

                                    fpmessreport.Sheets[0].Columns[colcount + 3].Font.Underline = true;
                                    fpmessreport.Sheets[0].Columns[colcount + 3].ForeColor = Color.Blue;
                                    fpmessreport.Sheets[0].Columns[colcount + 1].Font.Underline = true;
                                    fpmessreport.Sheets[0].Columns[colcount + 1].ForeColor = Color.Blue;
                                    fpmessreport.Sheets[0].Columns[colcount + 4].Visible = false;
                                    fpmessreport.Sheets[0].Columns[colcount + 2].Visible = false;
                                    fpmessreport.Sheets[0].Columns[colcount + 1].Visible = true;

                                }

                                if ((cbotype.SelectedItem.Text == "Both") && (rdocount.Checked == true))
                                {
                                    fpmessreport.Sheets[0].ColumnHeader.Cells[1, colcount + 2].Text = "Absent";
                                    fpmessreport.Sheets[0].ColumnHeader.Cells[1, colcount + 4].Text = "Present";

                                    fpmessreport.Sheets[0].Columns[colcount + 2].Font.Underline = true;
                                    fpmessreport.Sheets[0].Columns[colcount + 2].ForeColor = Color.Blue;
                                    fpmessreport.Sheets[0].Columns[colcount + 4].Font.Underline = true;
                                    fpmessreport.Sheets[0].Columns[colcount + 4].ForeColor = Color.Blue;
                                    fpmessreport.Sheets[0].Columns[colcount + 1].Visible = false;
                                    fpmessreport.Sheets[0].Columns[colcount + 3].Visible = false;
                                    fpmessreport.Sheets[0].Columns[colcount + 1].Visible = false;

                                    fpmessreport.Sheets[0].Columns[colcount + 2].Visible = true;
                                    fpmessreport.Sheets[0].Columns[colcount + 4].Visible = true;


                                }

                                fpmessreport.ActiveSheetView.Columns[colcount].Font.Size = FontUnit.Medium;

                                fpmessreport.ActiveSheetView.Columns[colcount].Font.Name = "Book Antiqua";

                                fpmessreport.ActiveSheetView.Columns[colcount + 1].Font.Size = FontUnit.Medium;

                                fpmessreport.ActiveSheetView.Columns[colcount + 1].Font.Name = "Book Antiqua";
                                fpmessreport.ActiveSheetView.Columns[colcount + 2].Font.Size = FontUnit.Medium;

                                fpmessreport.ActiveSheetView.Columns[colcount + 2].Font.Name = "Book Antiqua";

                                fpmessreport.ActiveSheetView.Columns[colcount + 3].Font.Size = FontUnit.Medium;

                                fpmessreport.ActiveSheetView.Columns[colcount + 3].Font.Name = "Book Antiqua";
                                fpmessreport.ActiveSheetView.Columns[colcount + 4].Font.Size = FontUnit.Medium;

                                fpmessreport.ActiveSheetView.Columns[colcount + 4].Font.Name = "Book Antiqua";

                                fpmessreport.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount, 1, 5);

                                colcount = fpmessreport.Sheets[0].ColumnCount;
                            }
                            else
                            {
                                fpmessreport.Sheets[0].ColumnCount = fpmessreport.Sheets[0].ColumnCount + 5;
                                string session = ds7.Tables[0].Rows[i]["session_name"].ToString();
                                fpmessreport.Sheets[0].ColumnHeader.Cells[0, colcount].Text = session;
                                fpmessreport.Sheets[0].ColumnHeader.Cells[0, colcount].Tag = Convert.ToString(ds7.Tables[0].Rows[i]["SessionMasterPK"]);


                                fpmessreport.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "Total";
                                fpmessreport.Sheets[0].ColumnHeader.Cells[1, colcount + 1].Text = "Absent(%)";
                                fpmessreport.Sheets[0].ColumnHeader.Cells[1, colcount + 2].Text = "Absent";
                                fpmessreport.Sheets[0].ColumnHeader.Cells[1, colcount + 4].Text = "Present";
                                fpmessreport.Sheets[0].ColumnHeader.Cells[1, colcount + 3].Text = "Present(%)";

                                fpmessreport.Sheets[0].Columns[colcount].Visible = false;
                                fpmessreport.Sheets[0].Columns[colcount + 1].Visible = false;
                                fpmessreport.Sheets[0].Columns[colcount + 2].Visible = false;
                                fpmessreport.Sheets[0].Columns[colcount + 3].Visible = false;
                                fpmessreport.Sheets[0].Columns[colcount + 4].Visible = false;

                                colcount = fpmessreport.Sheets[0].ColumnCount;
                            }
                        }
                    }
                    string sql = "";
                    sql = "SELECT Count(*) total FROM HT_HostelRegistration hsd,registration r  where  hsd.APP_No =r.App_No and isnull( hsd.IsVacated,0)=0 and isnull(hsd.IsSuspend,0)=0";

                    if (tbseattype.Text != "---Select---")
                    {
                        sql = sql + " and HostelMasterFK in('" + hostelcode + "')";
                    }
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sql, "text");
                    int count = count = ds.Tables[0].Rows.Count;
                    if (count > 0)
                    {
                        total = ds.Tables[0].Rows[0]["total"].ToString();
                        if (total != "")
                        {
                            counttotal = Convert.ToInt16(total);
                        }
                    }
                    if (monthname1 == monthnamenum)
                    {
                        for (int day3 = dayfrm; dayfrm <= daytonum; dayfrm++)
                        {
                            string date = "";
                            date = monyear + "/" + dayfrm;
                            string[] date5split = date.Split('/');
                            string date6 = date5split[2] + "/" + date5split[1] + "/" + date5split[0];
                            string sql5 = "";
                            session1 = "";
                            string session2 = "";
                            string session3 = "";
                            rowtstr = fpmessreport.Sheets[0].RowCount++;
                            for (int col = 1; col < fpmessreport.Sheets[0].ColumnCount - 1; col = col + 5)
                            {
                                session1 = fpmessreport.Sheets[0].ColumnHeader.Cells[0, col].Tag.ToString();

                                sql5 = "";
                                sql5 = "SELECT Count(*) tot FROM HT_HostelRegistration hsd,registration r  where  hsd.APP_No =r.App_No and isnull( hsd.IsVacated,0)=0 and isnull(hsd.IsSuspend,0)=0";
                                string strdept1 = "";
                                if (tbseattype.Text != "---Select---")
                                {
                                    sql5 = sql5 + " and hsd.HostelMasterFK in('" + hostelcode + "')";
                                }
                                sql5 = sql5 + " and r.roll_no not in";
                                sql5 = sql5 + " (select roll_no from HostelMess_Attendance where Entry_Date = '" + date + "'";
                                if (tbseattype.Text != "---Select---")
                                {
                                    sql5 = sql5 + "  and hostel_code in('" + hostelcode + "')";
                                }
                                sql5 = sql5 + " and session_code='" + session1 + "') ";

                                fpmessreport.Sheets[0].Cells[rowtstr, 0].Text = date6.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, 0].Tag = date.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col].Text = counttotal.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col].HorizontalAlign = HorizontalAlign.Center;
                                DataSet dscount = new DataSet();
                                dscount.Clear();
                                dscount = d2.select_method_wo_parameter(sql5, "text");
                                int breakcount = 0;
                                double breakepercentag = 0;
                                double breakpresentcount = 0;
                                double breakpresentpercentage = 0;
                                int countsess = dscount.Tables[0].Rows.Count;
                                if (countsess > 0)
                                {
                                    string breakefast = dscount.Tables[0].Rows[0]["tot"].ToString();
                                    breakcount = Convert.ToInt32(breakefast);
                                    breakpresentcount = counttotal - breakcount;
                                }
                                if (counttotal == 0)
                                {
                                    breakepercentag = 0;
                                }
                                else
                                {
                                    breakepercentag = (Convert.ToDouble(breakcount) / Convert.ToDouble((counttotal)) * 100);
                                    breakepercentag = Math.Round(breakepercentag, 2);
                                    breakpresentpercentage = (Convert.ToDouble(breakpresentcount) / Convert.ToDouble((counttotal)) * 100);
                                    breakpresentpercentage = Math.Round(breakpresentpercentage, 2);
                                }
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 1].Text = breakepercentag.ToString();

                                fpmessreport.Sheets[0].Cells[rowtstr, col + 1].Tag = session1.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 2].Tag = session1.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 3].Tag = session1.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 4].Tag = session1.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 1].HorizontalAlign = HorizontalAlign.Center;
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 3].Text = breakpresentpercentage.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 3].HorizontalAlign = HorizontalAlign.Center;
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 3].Note = "Present";
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 4].Note = "Present";
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 1].Note = "Absent";
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 2].Note = "Absent";
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 2].Text = breakcount.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 2].HorizontalAlign = HorizontalAlign.Center;
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 4].Text = breakpresentcount.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 4].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                    else
                    {
                        int maxdays = getmaxdays(monthname1, year3);
                        for (int day = dayfrm; day <= maxdays; day++)
                        {
                            string date = "";
                            date = monyear + "/" + day;
                            string[] date5split = date.Split('/');
                            string date6 = date5split[2] + "/" + date5split[1] + "/" + date5split[0];
                            string sql5 = "";
                            session1 = "";
                            rowtstr = fpmessreport.Sheets[0].RowCount++;
                            for (int col = 1; col < fpmessreport.Sheets[0].ColumnCount - 1; col = col + 5)
                            {
                                session1 = fpmessreport.Sheets[0].ColumnHeader.Cells[0, col].Tag.ToString();
                                sql5 = "";
                                sql5 = "  SELECT Count(*) tot FROM HT_HostelRegistration hsd,registration r  where  hsd.APP_No =r.App_No and isnull( hsd.IsVacated,0)=0 and isnull(hsd.IsSuspend,0)=0  ";

                                if (tbseattype.Text != "---Select---")
                                {
                                    sql5 = sql5 + " and Hostel_Code in(" + hostelcode + ")";
                                }
                                sql5 = sql5 + " and r.roll_no not in";
                                sql5 = sql5 + " (select roll_no from HostelMess_Attendance where Entry_Date = '" + date + "'";
                                if (tbseattype.Text != "---Select---")
                                {
                                    sql5 = sql5 + " and hsd.Hostel_Code in(" + hostelcode + ")";
                                }
                                sql5 = sql5 + " and session_code in('" + session1 + "') ";
                                fpmessreport.Sheets[0].Cells[rowtstr, 0].Text = date6.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, 0].Tag = date.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col].Text = counttotal.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col].HorizontalAlign = HorizontalAlign.Center;
                                SqlDataAdapter dacount = new SqlDataAdapter(sql5, con1);
                                DataSet dscount = new DataSet();
                                dscount.Clear();
                                dacount.Fill(dscount);
                                int breakcount = 0; double breakepercentag; double breakpresentcount = 0; double breakpresentpercentage = 0;
                                int countsess = dscount.Tables[0].Rows.Count;
                                if (countsess > 0)
                                {
                                    string breakefast = dscount.Tables[0].Rows[0]["tot"].ToString();
                                    breakcount = Convert.ToInt32(breakefast);
                                    breakpresentcount = counttotal - breakcount;
                                }
                                if (counttotal == 0)
                                {
                                    breakepercentag = 0;
                                }
                                else
                                {
                                    breakepercentag = (Convert.ToDouble(breakcount) / Convert.ToDouble((counttotal)) * 100);
                                    breakepercentag = Math.Round(breakepercentag, 2);
                                    breakpresentpercentage = (Convert.ToDouble(breakpresentcount) / Convert.ToDouble((counttotal)) * 100);
                                    breakpresentpercentage = Math.Round(breakpresentpercentage, 2);
                                }
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 1].Text = breakepercentag.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 1].Tag = session1.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 2].Tag = session1.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 3].Tag = session1.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 4].Tag = session1.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 3].Note = "Present";
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 4].Note = "Present";
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 1].Note = "Absent";
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 2].Note = "Absent";
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 1].HorizontalAlign = HorizontalAlign.Center;
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 3].Text = breakpresentpercentage.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 3].HorizontalAlign = HorizontalAlign.Center;
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 2].Text = breakcount.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 2].HorizontalAlign = HorizontalAlign.Center;
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 4].Text = breakpresentcount.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 4].HorizontalAlign = HorizontalAlign.Center;
                            }

                        }
                        for (int day9 = 1; day9 <= daytonum; day9++)
                        {
                            string date = "";
                            date = monyearto + day9;
                            string[] date5split = date.Split('/');
                            string date6 = date5split[2] + "/" + date5split[1] + "/" + date5split[0];
                            string sql5 = "";
                            session1 = "";
                            rowtstr = fpmessreport.Sheets[0].RowCount++;
                            for (int col = 1; col < fpmessreport.Sheets[0].ColumnCount - 1; col = col + 5)
                            {
                                session1 = fpmessreport.Sheets[0].ColumnHeader.Cells[0, col].Tag.ToString();

                                sql5 = "";
                                sql5 = "  SELECT Count(*) tot FROM HT_HostelRegistration hsd,registration r  where  hsd.APP_No =r.App_No and isnull( hsd.IsVacated,0)=0 and isnull(hsd.IsSuspend,0)=0  ";

                                if (tbseattype.Text != "---Select---")
                                {
                                    sql5 = sql5 + " and hsd.HostelMasterFK in(" + hostelcode + ")";
                                }
                                sql5 = sql5 + " and r.roll_no not in";
                                sql5 = sql5 + " (select roll_no from HostelMess_Attendance where Entry_Date = '" + date + "'";
                                if (tbseattype.Text != "---Select---")
                                {
                                    sql5 = sql5 + " and Hostel_Code in(" + hostelcode + ")";
                                }
                                sql5 = sql5 + " and session_code='" + session1 + "') ";
                                fpmessreport.Sheets[0].Cells[rowtstr, 0].Text = date6.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, 0].Tag = date.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col].Text = counttotal.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col].HorizontalAlign = HorizontalAlign.Center;
                                SqlDataAdapter dacount = new SqlDataAdapter(sql5, con1);
                                DataSet dscount = new DataSet();
                                dscount.Clear();
                                dacount.Fill(dscount);
                                int breakcount = 0;
                                double breakepercentag;
                                double breakpresentcount = 0;
                                double breakpresentpercentage = 0;
                                int countsess = dscount.Tables[0].Rows.Count;
                                if (countsess > 0)
                                {
                                    string breakefast = dscount.Tables[0].Rows[0]["tot"].ToString();
                                    breakcount = Convert.ToInt32(breakefast);
                                    breakpresentcount = counttotal - breakcount;
                                }
                                if (counttotal == 0)
                                {
                                    breakepercentag = 0;
                                }
                                else
                                {
                                    breakepercentag = (Convert.ToDouble(breakcount) / Convert.ToDouble((counttotal)) * 100);
                                    breakepercentag = Math.Round(breakepercentag, 2);
                                    breakpresentpercentage = (Convert.ToDouble(breakpresentcount) / Convert.ToDouble((counttotal)) * 100);
                                    breakpresentpercentage = Math.Round(breakpresentpercentage, 2);
                                }
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 1].Text = breakepercentag.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 1].Tag = session1.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 2].Tag = session1.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 3].Tag = session1.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 4].Tag = session1.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 1].HorizontalAlign = HorizontalAlign.Center;
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 3].Text = breakpresentpercentage.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 3].HorizontalAlign = HorizontalAlign.Center;

                                fpmessreport.Sheets[0].Cells[rowtstr, col + 2].Text = breakcount.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 2].HorizontalAlign = HorizontalAlign.Center;

                                fpmessreport.Sheets[0].Cells[rowtstr, col + 4].Text = breakpresentcount.ToString();
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 4].HorizontalAlign = HorizontalAlign.Center;

                                fpmessreport.Sheets[0].Cells[rowtstr, col + 3].Note = "Present";
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 4].Note = "Present";
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 1].Note = "Absent";
                                fpmessreport.Sheets[0].Cells[rowtstr, col + 2].Note = "Absent";
                            }
                        }
                    }
                    DropDownListpage.Items.Clear();
                    Double totalRows = 0;
                    totalRows = Convert.ToInt32(fpmessreport.Sheets[0].RowCount);
                    DropDownListpage.Items.Clear();
                    if (totalRows >= 10)
                    {
                        fpmessreport.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                        for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                        {
                            DropDownListpage.Items.Add((k + 10).ToString());
                        }
                        DropDownListpage.Items.Add("Others");
                        DropDownListpage.SelectedIndex = DropDownListpage.Items.Count - 2;
                        fpmessreport.Height = 350;
                        fpmessreport.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                        fpmessreport.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

                    }
                    else if (totalRows == 0)
                    {
                        DropDownListpage.Items.Add("0");
                        fpmessreport.Height = 300;
                    }
                    else
                    {
                        fpmessreport.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                        DropDownListpage.Items.Add(fpmessreport.Sheets[0].PageSize.ToString());
                        //sprdHostel.Height = 75 + (50 * Convert.ToInt32(totalRows));
                    }
                    fpmessreport.Height = 30 + (fpmessreport.Sheets[0].RowCount * 30);

                    Session["totalPages"] = (int)Math.Ceiling(totalRows / fpmessreport.Sheets[0].PageSize);
                    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];

                    if (fpmessreport.Sheets[0].RowCount == 0)
                    {
                        fpmessreport.Visible = false;

                        Buttontotal.Visible = false;
                        DropDownListpage.Visible = false;
                        TextBoxpage.Visible = false;
                        lblrecord.Visible = false;
                        lblpage.Visible = false;
                        lblnorec.Visible = true;
                    }
                }
            }
        }
        else
        {
            Fpcumulative.Visible = false;
            fpmessreport.Visible = false;

            Buttontotal.Visible = false;
            DropDownListpage.Visible = false;
            TextBoxpage.Visible = false;
            lblrecord.Visible = false;
            lblrecord.Visible = false;
            lblpage.Visible = false;
            lblnorec.Visible = false;
            FpSpread1.Visible = false;
            lblmistake.Visible = true;
        }
        if (cbosession.Items.Count > 0)
        {
            Fpcumulative.Visible = true;
            fpmessreport.Visible = true;
        }
        else
        {
            Fpcumulative.Visible = false;
            fpmessreport.Visible = false;
            lblrecord.Text = "Please Update Session Information for Hostel";
        }
    }
    public int getmaxdays(int mno, int year)
    {

        int maxdays = 0;
        if ((mno == 2) && (year % 4 == 0))
        {
            maxdays = 29;
            return maxdays;
        }


        else if ((mno == 1) || (mno == 3) || (mno == 5) || (mno == 7) || (mno == 8) || (mno == 10) || (mno == 12))
        {
            maxdays = 31;
            return maxdays;
        }
        else if ((mno == 4) || (mno == 6) || (mno == 9) || (mno == 11))
        {
            maxdays = 30;
            return maxdays;
        }

        else if ((mno == 2) || (year % 4) != 0)
        {
            maxdays = 28;
            return maxdays;
        }
        return maxdays;
    }
    protected void fpmessreport_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        cellclick = true;
        load_popup();
    }
    protected void fpmessreport_SelectedIndexChanged(Object sender, EventArgs e)
    {
        load_popup();
    }
    void load_popup()
    {
        if (cellclick == true)
        {
            string activerow;
            string activecol;
            int ar = 0;
            int ac = 0;
            string getdate;
            string sql6 = "";
            activerow = fpmessreport.ActiveSheetView.ActiveRow.ToString();
            activecol = fpmessreport.ActiveSheetView.ActiveColumn.ToString();
            ar = Convert.ToInt32(activerow.ToString());
            ac = Convert.ToInt32(activecol.ToString());
            string hostelcode = rs.GetSelectedItemsValueAsString(Cbo_HostelName);
            int rowcount = 0;
            if (ar != -1)
            {
                lblstudent.Visible = false;
                string session = Convert.ToString(fpmessreport.Sheets[0].Cells[ar, ac].Tag);
                string type = Convert.ToString(fpmessreport.Sheets[0].Cells[ar, ac].Note);

                FpSpread1.Sheets[0].PageSize = 11;
                FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                FpSpread1.Pager.Align = HorizontalAlign.Right;
                FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].Font.Bold = true;
                FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].Font.Size = FontUnit.Medium;

                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                FpSpread1.Pager.Font.Bold = true;
                FpSpread1.Pager.Font.Name = "Arial";
                FpSpread1.Pager.ForeColor = Color.DarkGreen;
                FpSpread1.Pager.BackColor = Color.AliceBlue;
                FpSpread1.Pager.PageCount = 5;
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                FpSpread1.Sheets[0].SheetCorner.RowCount = 8;
                FpSpread1.Visible = true;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 6;
                FpSpread1.Sheets[0].SetColumnWidth(1, 275);
                FpSpread1.Sheets[0].SetColumnWidth(0, 75);
                FpSpread1.Sheets[0].SetColumnWidth(3, 90);
                //////////set column//////////////////////////////////////////////////////////////////////////////////////
                FpSpread1.Sheets[0].ColumnHeader.Cells[6, 0].Text = "Student Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[6, 1].Text = "Degree";
                FpSpread1.Sheets[0].ColumnHeader.Cells[6, 2].Text = "Semester";
                FpSpread1.Sheets[0].ColumnHeader.Cells[6, 3].Text = "Hostel";
                FpSpread1.Sheets[0].ColumnHeader.Cells[6, 4].Text = "Floor Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[6, 5].Text = "Room No";
                //////////set column//////////////////////////////////////////////////////////////////////////////////////
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
                string date5 = Convert.ToString(fpmessreport.Sheets[0].Cells[ar, 0].Text);
                getdate = Convert.ToString(fpmessreport.Sheets[0].Cells[ar, 0].Tag);
                Lbldate2.Text = getdate;
                /////////////////////////////////////////////////////////////////////////////////
                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 10;
                style.Font.Bold = true;
                FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpSpread1.Sheets[0].AllowTableCorner = true;
                FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].Text = "  ";

                string str = "select isnull(collname, ' ') as collname,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
                con.Close();
                con.Open();
                SqlCommand comm = new SqlCommand(str, con);
                SqlDataReader drr = comm.ExecuteReader();
                drr.Read();
                string coll_name = Convert.ToString(drr["collname"]);
                string coll_address1 = Convert.ToString(drr["address1"]);
                string coll_address2 = Convert.ToString(drr["address2"]);
                string coll_address3 = Convert.ToString(drr["address3"]);
                string pin_code = Convert.ToString(drr["pincode"]);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = coll_name;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorBottom = Color.White;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, 4);

                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Text = coll_address1 + "     " + coll_address2; ;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, 4);
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorBottom = Color.White;

                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, 4);
                FpSpread1.Sheets[0].ColumnHeader.Cells[2, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorBottom = Color.White;

                FpSpread1.Sheets[0].ColumnHeader.Cells[2, 1].Text = coll_address3 + "-" + pin_code + ".";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, 4);
                FpSpread1.Sheets[0].ColumnHeader.Cells[3, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorBottom = Color.White;

                FpSpread1.Sheets[0].ColumnHeader.Cells[3, 1].Text = "Mess Attendance List";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, 4);
                FpSpread1.Sheets[0].ColumnHeader.Cells[3, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorBottom = Color.White;
                FpSpread1.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorBottom = Color.White;

                MyImg mi = new MyImg();
                mi.ImageUrl = "~/images/10BIT001.jpeg";
                mi.ImageUrl = "~/Handler/Handler2.ashx?";
                MyImg mi2 = new MyImg();
                mi2.ImageUrl = "~/college/Left_Logo.jpeg";
                mi2.ImageUrl = "~/Handler/Handler5.ashx?";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].CellType = mi2;
                FpSpread1.Sheets[0].SetColumnWidth(5, 75);
                //FpSpread1.Sheets[0].SetColumnWidth(1, 70);

                FpSpread1.Sheets[0].SheetCornerSpanModel.Add(0, 0, 6, 1);
                FpSpread1.Sheets[0].SheetCornerSpanModel.Add(6, 0, 2, 1);
                FpSpread1.Sheets[0].SheetCorner.Cells[6, 0].Text = "S.No";
                //Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 7, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 6, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 6, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 6, 1);
                FpSpread1.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Rows[1].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Rows[1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Rows[2].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Rows[2].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Rows[3].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Rows[3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Rows[4].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Rows[4].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Rows[5].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Rows[5].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Rows[6].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Rows[6].Font.Size = FontUnit.Medium;

                FpSpread1.Sheets[0].ColumnHeader.Cells[5, 1].Text = "Date:" + date5.ToString();

                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(5, 1, 1, 4);
                //FpSpread1.Sheets[0].ColumnHeader.Cells[4, 2].Text = getdate.ToString();
                //FpSpread1.Sheets[0].ColumnHeader.Cells[4, 3].Text = type.ToString();
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(5, 3, 1, 2);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(5, 4, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(4, 1, 1, 4);

                FpSpread1.Sheets[0].SheetCornerSpanModel.Add(0, 0, 6, 1);
                FpSpread1.Sheets[0].SheetCorner.Cells[6, 0].Text = "S.No";
                FpSpread1.Sheets[0].SheetCorner.Cells[6, 0].BackColor = Color.FromArgb(214, 235, 255);
                FpSpread1.Sheets[0].SheetCorner.Cells[6, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].SheetCorner.Cells[6, 0].Border.BorderColorRight = Color.White;
                FpSpread1.Sheets[0].SheetCornerSpanModel.Add(6, 0, 3, 1);

                FpSpread1.Sheets[0].ColumnHeader.Rows[6].BackColor = Color.FromArgb(214, 235, 255);
                FpSpread1.Sheets[0].ColumnHeader.Rows[7].BackColor = Color.FromArgb(214, 235, 255);
                FpSpread1.Sheets[0].ColumnHeader.Rows[6].BackColor = Color.FromArgb(214, 235, 255);
                FpSpread1.Sheets[0].ColumnHeader.Rows[7].BackColor = Color.FromArgb(214, 235, 255);

                FpSpread1.Sheets[0].ColumnHeader.Rows[6].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Rows[6].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, 0, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, 1, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, 2, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, 3, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, 4, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, 5, 2, 1);

                if (ac == 0)
                {
                    if (cbotype.SelectedItem.Text != "Both")
                    {
                        for (int itemcoun = 0; itemcoun < cbosession.Items.Count; itemcoun++)
                        {
                            string strsession = "";
                            if (cbosession.Items[itemcoun].Selected == true)
                            {
                                strsession = "";
                                if (strsession == "")
                                    strsession = "'" + cbosession.Items[itemcoun].Value.ToString() + "'";
                                else
                                    strsession = strsession + "," + "'" + cbosession.Items[itemcoun].Value.ToString() + "'";

                                if (strsession != "")
                                {
                                    strsession = strsession;
                                }
                                rowcount = FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[rowcount, 0].Text = cbosession.Items[itemcoun].Text;
                                FpSpread1.Sheets[0].Cells[rowcount, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[rowcount, 0].BackColor = Color.SkyBlue;
                                FpSpread1.Sheets[0].SpanModel.Add(rowcount, 0, 1, 6);
                                if (cbotype.SelectedItem.Text == "Absent")
                                {
                                    sql6 = " SELECT r.roll_no,Course_Name+'-'+Dept_Name as Degree,r.stud_name,r.current_semester,h.hostelname,fm.Floor_Name,rd.room_name FROM HT_HostelRegistration hsd,registration r,Degree G, course e,department d ,HM_HostelMaster h ,room_detail rd,Floor_Master fm  where  hsd.APP_No=r.App_No and r.degree_code = G.degree_code  and g.dept_code=d.dept_code and  G.Course_ID = e.Course_ID and h.HostelMasterPK=hsd.HostelMasterFK and rd.Roompk=hsd.RoomFK and fm.Floorpk=hsd.FloorFK and fm.Floor_Name=rd.Floor_Name and isnull(hsd.IsVacated,0)=0 and ISNULL(hsd.IsSuspend,0)=0";
                                    if (tbseattype.Text != "---Select---")
                                    {
                                        sql6 = sql6 + " and hsd.HostelMasterFK in('" + hostelcode + "')";
                                    }

                                    sql6 = sql6 + " and r.roll_no not in";

                                    sql6 = sql6 + " (select roll_no from HostelMess_Attendance where Entry_Date = '" + getdate + "'";
                                    if (tbseattype.Text != "---Select---")
                                    {
                                        sql6 = sql6 = sql6 + "  and hostel_code IN ('" + hostelcode + "')";
                                    }
                                    sql6 = sql6 + " and session_code=" + strsession + ")";
                                    SqlDataAdapter dastudent = new SqlDataAdapter(sql6, myconn);
                                    DataSet dsstudent = new DataSet();
                                    dastudent.Fill(dsstudent);
                                    int rowstr;
                                    int studentcount = dsstudent.Tables[0].Rows.Count;
                                    if (studentcount > 0)
                                    {
                                        for (int stucount = 0; stucount < dsstudent.Tables[0].Rows.Count; stucount++)
                                        {
                                            string studname = "";
                                            string degree = "";
                                            string sem = "";
                                            string hostel_name = "";
                                            string floor_name = "";
                                            string room_no = "";
                                            rowstr = FpSpread1.Sheets[0].RowCount++;
                                            studname = dsstudent.Tables[0].Rows[stucount]["stud_name"].ToString();
                                            degree = dsstudent.Tables[0].Rows[stucount]["degree"].ToString();
                                            sem = dsstudent.Tables[0].Rows[stucount]["current_semester"].ToString();
                                            hostel_name = dsstudent.Tables[0].Rows[stucount]["hostelname"].ToString();
                                            floor_name = dsstudent.Tables[0].Rows[stucount]["floor_name"].ToString();
                                            room_no = dsstudent.Tables[0].Rows[stucount]["room_name"].ToString();
                                            FpSpread1.Sheets[0].Cells[rowstr, 1].Text = degree;
                                            FpSpread1.Sheets[0].Cells[rowstr, 0].Text = studname;
                                            FpSpread1.Sheets[0].Cells[rowstr, 2].Text = sem;
                                            FpSpread1.Sheets[0].Cells[rowstr, 2].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[rowstr, 3].Text = hostel_name;
                                            FpSpread1.Sheets[0].Cells[rowstr, 4].Text = floor_name;
                                            FpSpread1.Sheets[0].Cells[rowstr, 5].Text = room_no;
                                        }
                                    }
                                }

                                if (cbotype.SelectedItem.Text == "Present")
                                {
                                    sql6 = " SELECT r.roll_no,Course_Name+'-'+Dept_Name as Degree,r.stud_name,r.current_semester,h.hostelname,fm.Floor_Name,rd.room_name FROM HT_HostelRegistration hsd,registration r,Degree G, course e,department d ,HM_HostelMaster h ,room_detail rd,Floor_Master fm  where  hsd.APP_No=r.App_No and r.degree_code = G.degree_code  and g.dept_code=d.dept_code and  G.Course_ID = e.Course_ID and h.HostelMasterPK=hsd.HostelMasterFK and rd.Roompk=hsd.RoomFK and fm.Floorpk=hsd.FloorFK and fm.Floor_Name=rd.Floor_Name and isnull(hsd.IsVacated,0)=0 and ISNULL(hsd.IsSuspend,0)=0";
                                    if (tbseattype.Text != "---Select---")
                                    {
                                        sql6 = sql6 + " and hsd.HostelMasterFK in('" + hostelcode + "')";
                                    }
                                    sql6 = sql6 + " and r.roll_no  in";

                                    sql6 = sql6 + " (select roll_no from HostelMess_Attendance where Entry_Date = '" + getdate + "'";
                                    if (tbseattype.Text != "---Select---")
                                    {
                                        sql6 = sql6 = sql6 + "  and hostel_code in('" + hostelcode + "')";
                                    }
                                    sql6 = sql6 + " and session_code=" + strsession + ")";
                                    SqlDataAdapter dastudent = new SqlDataAdapter(sql6, myconn);
                                    DataSet dsstudent = new DataSet();
                                    dsstudent.Clear();
                                    dastudent.Fill(dsstudent);
                                    int rowstr = 0;
                                    int studentcount = dsstudent.Tables[0].Rows.Count;
                                    if (studentcount > 0)
                                    {
                                        for (int stucount = 0; stucount < dsstudent.Tables[0].Rows.Count; stucount++)
                                        {
                                            string studname = "";
                                            string degree = "";
                                            string sem = "";
                                            string hostel_name = "";
                                            string floor_name = "";
                                            string room_no = "";
                                            rowstr = FpSpread1.Sheets[0].RowCount++;
                                            studname = dsstudent.Tables[0].Rows[stucount]["stud_name"].ToString();
                                            degree = dsstudent.Tables[0].Rows[stucount]["degree"].ToString();
                                            sem = dsstudent.Tables[0].Rows[stucount]["current_semester"].ToString();
                                            hostel_name = dsstudent.Tables[0].Rows[stucount]["hostelname"].ToString();
                                            floor_name = dsstudent.Tables[0].Rows[stucount]["floor_name"].ToString();
                                            room_no = dsstudent.Tables[0].Rows[stucount]["room_name"].ToString();
                                            FpSpread1.Sheets[0].Cells[rowstr, 1].Text = degree;
                                            FpSpread1.Sheets[0].Cells[rowstr, 0].Text = studname;

                                            FpSpread1.Sheets[0].Cells[rowstr, 2].Text = sem;
                                            FpSpread1.Sheets[0].Cells[rowstr, 2].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[rowstr, 3].Text = hostel_name;
                                            FpSpread1.Sheets[0].Cells[rowstr, 4].Text = floor_name;
                                            FpSpread1.Sheets[0].Cells[rowstr, 5].Text = room_no;
                                        }
                                    }
                                    if (rowstr == 0)
                                    {
                                        lblstudent.Visible = true;
                                        FpSpread1.Visible = false;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ///////////////////boths/////
                        for (int itemcoun = 0; itemcoun < cbosession.Items.Count; itemcoun++)
                        {
                            string strsession = "";
                            if (cbosession.Items[itemcoun].Selected == true)
                            {
                                strsession = "";
                                if (strsession == "")
                                    strsession = "'" + cbosession.Items[itemcoun].Value.ToString() + "'";
                                else
                                    strsession = strsession + "," + "'" + cbosession.Items[itemcoun].Value.ToString() + "'";
                                if (strsession != "")
                                {
                                    strsession = strsession;
                                }
                                rowcount = FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[rowcount, 0].Text = cbosession.Items[itemcoun].Text;
                                FpSpread1.Sheets[0].Cells[rowcount, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[rowcount, 0].BackColor = Color.SkyBlue;
                                FpSpread1.Sheets[0].SpanModel.Add(rowcount, 0, 1, 6);

                                for (int h = 1; h < cbotype.Items.Count; h++)
                                {
                                    if (cbotype.Items[h].Text == "Absent")
                                    {
                                        sql6 = " SELECT r.roll_no,Course_Name+'-'+Dept_Name as Degree,r.stud_name,r.current_semester,h.hostelname,fm.Floor_Name,rd.room_name FROM HT_HostelRegistration hsd,registration r,Degree G, course e,department d ,HM_HostelMaster h ,room_detail rd,Floor_Master fm  where  hsd.APP_No=r.App_No and r.degree_code = G.degree_code  and g.dept_code=d.dept_code and  G.Course_ID = e.Course_ID and h.HostelMasterPK=hsd.HostelMasterFK and rd.Roompk=hsd.RoomFK and fm.Floorpk=hsd.FloorFK and fm.Floor_Name=rd.Floor_Name and isnull(hsd.IsVacated,0)=0 and ISNULL(hsd.IsSuspend,0)=0";
                                        if (tbseattype.Text != "---Select---")
                                        {
                                            sql6 = sql6 + " and hsd.HostelMasterFK in('" + hostelcode + "')";
                                        }
                                        sql6 = sql6 + " and r.roll_no not in";
                                        sql6 = sql6 + " (select roll_no from HostelMess_Attendance where Entry_Date = '" + getdate + "'";
                                        if (tbseattype.Text != "---Select---")
                                        {
                                            sql6 = sql6 = sql6 + "  and hostel_code in('" + hostelcode + "')";
                                        }
                                        sql6 = sql6 + " and session_code=" + strsession + ")";
                                        DataSet dsstudent = new DataSet();
                                        dsstudent = d2.select_method_wo_parameter(sql6, "Text");
                                        int rowstr;
                                        int studentcount = dsstudent.Tables[0].Rows.Count;
                                        if (studentcount > 0)
                                        {
                                            for (int stucount = 0; stucount < dsstudent.Tables[0].Rows.Count; stucount++)
                                            {
                                                string studname = "";
                                                string degree = "";
                                                string sem = "";
                                                string hostel_name = "";
                                                string floor_name = "";
                                                string room_no = "";
                                                rowstr = FpSpread1.Sheets[0].RowCount++;
                                                studname = dsstudent.Tables[0].Rows[stucount]["stud_name"].ToString();
                                                degree = dsstudent.Tables[0].Rows[stucount]["degree"].ToString();
                                                sem = dsstudent.Tables[0].Rows[stucount]["current_semester"].ToString();
                                                hostel_name = dsstudent.Tables[0].Rows[stucount]["hostelname"].ToString();
                                                floor_name = dsstudent.Tables[0].Rows[stucount]["floor_name"].ToString();
                                                room_no = dsstudent.Tables[0].Rows[stucount]["room_name"].ToString();
                                                FpSpread1.Sheets[0].Cells[rowstr, 1].Text = degree;
                                                FpSpread1.Sheets[0].Cells[rowstr, 0].Text = studname;

                                                FpSpread1.Sheets[0].Cells[rowstr, 2].Text = sem;
                                                FpSpread1.Sheets[0].Cells[rowstr, 2].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[rowstr, 3].Text = hostel_name;
                                                FpSpread1.Sheets[0].Cells[rowstr, 4].Text = floor_name;
                                                FpSpread1.Sheets[0].Cells[rowstr, 5].Text = room_no;
                                            }
                                        }
                                    }
                                    if (cbotype.Items[h].Text == "Present")
                                    {
                                        sql6 = "SELECT r.roll_no,Course_Name+'-'+Dept_Name as Degree,r.stud_name,r.current_semester,h.hostel_name,hsd.floor_name,hsd.room_name FROM Hostel_StudentDetails hsd,registration r,Degree G, course e,department d ,hostel_details h  ";
                                        sql6 = sql6 + " where  hsd.roll_admit=r.roll_admit ";
                                        sql6 = sql6 + " and r.degree_code = G.degree_code  and g.dept_code=d.dept_code and  G.Course_ID = e.Course_ID and h.hostel_code=hsd.hostel_code ";
                                        string strdept1 = "";
                                        if (tbseattype.Text != "---Select---")
                                        {
                                            int itemcount = 0;
                                            for (itemcount = 0; itemcount < Cbo_HostelName.Items.Count; itemcount++)
                                            {
                                                if (Cbo_HostelName.Items[itemcount].Selected == true)
                                                {
                                                    if (strdept1 == "")
                                                        strdept1 = "'" + Cbo_HostelName.Items[itemcount].Value.ToString() + "'";
                                                    else
                                                        strdept1 = strdept1 + "," + "'" + Cbo_HostelName.Items[itemcount].Value.ToString() + "'";
                                                }
                                            }
                                            if (strdept1 != "")
                                            {
                                                strdept1 = " in(" + strdept1 + ")";
                                            }

                                            sql6 = sql6 + " and hsd.Hostel_Code " + strdept1 + "";
                                        }
                                        sql6 = sql6 + " and r.roll_no  in";

                                        sql6 = sql6 + " (select roll_no from HostelMess_Attendance where Entry_Date = '" + getdate + "'";
                                        if (tbseattype.Text != "---Select---")
                                        {
                                            sql6 = sql6 = sql6 + "  and hostel_code" + strdept1 + "";
                                        }
                                        sql6 = sql6 + " and session_name=" + strsession + ")";
                                        SqlDataAdapter dastudent = new SqlDataAdapter(sql6, myconn);
                                        DataSet dsstudent = new DataSet();
                                        dsstudent.Clear();
                                        dastudent.Fill(dsstudent);
                                        int rowstr = 0;
                                        int studentcount = dsstudent.Tables[0].Rows.Count;
                                        if (studentcount > 0)
                                        {
                                            //FpSpread1.Sheets[0].Cells[rowstr, 2].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.DataSource = dsstudent.Tables[0];
                                            FpSpread1.DataBind();

                                            for (int stucount = 0; stucount < dsstudent.Tables[0].Rows.Count; stucount++)
                                            {
                                                string studname = "";
                                                string degree = "";
                                                string sem = "";
                                                string hostel_name = "";
                                                string floor_name = "";
                                                string room_no = "";
                                                rowstr = FpSpread1.Sheets[0].RowCount++;
                                                studname = dsstudent.Tables[0].Rows[stucount]["stud_name"].ToString();
                                                degree = dsstudent.Tables[0].Rows[stucount]["degree"].ToString();
                                                sem = dsstudent.Tables[0].Rows[stucount]["current_semester"].ToString();
                                                hostel_name = dsstudent.Tables[0].Rows[stucount]["hostel_name"].ToString();
                                                floor_name = dsstudent.Tables[0].Rows[stucount]["floor_name"].ToString();
                                                room_no = dsstudent.Tables[0].Rows[stucount]["room_name"].ToString();
                                                FpSpread1.Sheets[0].Cells[rowstr, 1].Text = degree;
                                                FpSpread1.Sheets[0].Cells[rowstr, 0].Text = studname;
                                                FpSpread1.Sheets[0].Cells[rowstr, 0].BackColor = Color.Green;

                                                FpSpread1.Sheets[0].Cells[rowstr, 2].Text = sem;
                                                FpSpread1.Sheets[0].Cells[rowstr, 2].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[rowstr, 3].Text = hostel_name;
                                                FpSpread1.Sheets[0].Cells[rowstr, 4].Text = floor_name;
                                                FpSpread1.Sheets[0].Cells[rowstr, 5].Text = room_no;
                                            }
                                        }
                                        if (rowstr == 0)
                                        {
                                            lblstudent.Visible = true;
                                            FpSpread1.Visible = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    rowcount = FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[rowcount, 0].Text = session.ToString();
                    FpSpread1.Sheets[0].Cells[rowcount, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[rowcount, 0].BackColor = Color.SkyBlue;

                    FpSpread1.Sheets[0].SpanModel.Add(rowcount, 0, 1, 6);
                    if (type == "Absent")
                    {
                        sql6 = " SELECT r.roll_no,Course_Name+'-'+Dept_Name as Degree,r.stud_name,r.current_semester,h.hostelname,fm.Floor_Name,rd.room_name FROM HT_HostelRegistration hsd,registration r,Degree G, course e,department d ,HM_HostelMaster h ,room_detail rd,Floor_Master fm  where  hsd.APP_No=r.App_No and r.degree_code = G.degree_code  and g.dept_code=d.dept_code and  G.Course_ID = e.Course_ID and h.HostelMasterPK=hsd.HostelMasterFK and rd.Roompk=hsd.RoomFK and fm.Floorpk=hsd.FloorFK and fm.Floor_Name=rd.Floor_Name and isnull( hsd.IsVacated,0)=0 and isnull(hsd.IsSuspend,0)=0";
                        if (tbseattype.Text != "---Select---")
                        {
                            sql6 += " and hsd.HostelMasterFK in('" + hostelcode + "')";
                        }
                        sql6 = sql6 + " and r.roll_no not in (select roll_no from HostelMess_Attendance where Entry_Date = '" + getdate + "'";
                        if (tbseattype.Text != "---Select---")
                        {
                            sql6 = sql6 = sql6 + "  and hostel_code in('" + hostelcode + "')";
                        }
                        sql6 = sql6 + " and session_code='" + session + "')";


                        //sql6 = "SELECT r.roll_no,Course_Name+'-'+Dept_Name as Degree,r.stud_name,r.current_semester,h.hostel_name,hsd.floor_name,hsd.room_name FROM Hostel_StudentDetails hsd,registration r,Degree G, course e,department d ,hostel_details h  ";
                        //sql6 = sql6 + " where  hsd.roll_admit=r.roll_admit ";
                        //sql6 = sql6 + " and r.degree_code = G.degree_code  and g.dept_code=d.dept_code and  G.Course_ID = e.Course_ID and h.hostel_code=hsd.hostel_code ";
                        //string strdept1 = "";
                        //if (tbseattype.Text != "---Select---")
                        //{
                        //    int itemcount = 0;
                        //    for (itemcount = 0; itemcount < Cbo_HostelName.Items.Count; itemcount++)
                        //    {
                        //        if (Cbo_HostelName.Items[itemcount].Selected == true)
                        //        {
                        //            if (strdept1 == "")
                        //                strdept1 = "'" + Cbo_HostelName.Items[itemcount].Value.ToString() + "'";
                        //            else
                        //                strdept1 = strdept1 + "," + "'" + Cbo_HostelName.Items[itemcount].Value.ToString() + "'";
                        //        }
                        //    }
                        //    if (strdept1 != "")
                        //    {
                        //        strdept1 = " in(" + strdept1 + ")";
                        //    }
                        //    sql6 = sql6 + " and hsd.Hostel_Code " + strdept1 + "";
                        //}
                        //sql6 = sql6 + " and r.roll_no not in";
                        //sql6 = sql6 + " (select roll_no from HostelMess_Attendance where Entry_Date = '" + getdate + "'";
                        //if (tbseattype.Text != "---Select---")
                        //{
                        //    sql6 = sql6 = sql6 + "  and hostel_code" + strdept1 + "";
                        //}
                        //sql6 = sql6 + " and session_name='" + session + "')";

                        DataSet dsstudent = new DataSet();
                        dsstudent = d2.select_method_wo_parameter(sql6, "text");
                        int rowstr = 0;
                        int studentcount = dsstudent.Tables[0].Rows.Count;
                        if (studentcount > 0)
                        {
                            for (int stucount = 0; stucount < dsstudent.Tables[0].Rows.Count; stucount++)
                            {
                                string studname = "";
                                string degree = "";
                                string sem = "";
                                string hostel_name = "";
                                string floor_name = "";
                                string room_no = "";
                                rowstr = FpSpread1.Sheets[0].RowCount++;
                                studname = dsstudent.Tables[0].Rows[stucount]["stud_name"].ToString();
                                degree = dsstudent.Tables[0].Rows[stucount]["degree"].ToString();
                                sem = dsstudent.Tables[0].Rows[stucount]["current_semester"].ToString();
                                hostel_name = dsstudent.Tables[0].Rows[stucount]["hostelname"].ToString();
                                floor_name = dsstudent.Tables[0].Rows[stucount]["floor_name"].ToString();
                                room_no = dsstudent.Tables[0].Rows[stucount]["room_name"].ToString();
                                FpSpread1.Sheets[0].Cells[rowstr, 1].Text = degree;
                                FpSpread1.Sheets[0].Cells[rowstr, 0].Text = studname;

                                FpSpread1.Sheets[0].Cells[rowstr, 2].Text = sem;
                                FpSpread1.Sheets[0].Cells[rowstr, 2].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[rowstr, 3].Text = hostel_name;
                                FpSpread1.Sheets[0].Cells[rowstr, 4].Text = floor_name;
                                FpSpread1.Sheets[0].Cells[rowstr, 5].Text = room_no;
                            }
                        }
                        if (rowstr == 0)
                        {
                            FpSpread1.Visible = false;
                            lblstudent.Visible = true;
                        }
                    }
                    if (type == "Present")
                    {
                        sql6 = " SELECT r.roll_no,Course_Name+'-'+Dept_Name as Degree,r.stud_name,r.current_semester,h.hostelname,fm.Floor_Name,rd.room_name FROM HT_HostelRegistration hsd,registration r,Degree G, course e,department d ,HM_HostelMaster h ,room_detail rd,Floor_Master fm  where  hsd.APP_No=r.App_No and r.degree_code = G.degree_code  and g.dept_code=d.dept_code and  G.Course_ID = e.Course_ID and h.HostelMasterPK=hsd.HostelMasterFK and rd.Roompk=hsd.RoomFK and fm.Floorpk=hsd.FloorFK and fm.Floor_Name=rd.Floor_Name";
                        if (tbseattype.Text != "---Select---")
                        {
                            sql6 += " and hsd.HostelMasterFK in('" + hostelcode + "')";
                        }
                        sql6 = sql6 + " and r.roll_no  in (select roll_no from HostelMess_Attendance where Entry_Date = '" + getdate + "'";
                        if (tbseattype.Text != "---Select---")
                        {
                            sql6 = sql6 = sql6 + "  and hostel_code in('" + hostelcode + "')";
                        }
                        sql6 = sql6 + " and session_code='" + session + "')";
                        DataSet dsstudent = new DataSet();
                        dsstudent = d2.select_method_wo_parameter(sql6, "text");
                        int rowstr = 0;
                        int studentcount = dsstudent.Tables[0].Rows.Count;
                        if (studentcount > 0)
                        {
                            for (int stucount = 0; stucount < dsstudent.Tables[0].Rows.Count; stucount++)
                            {
                                string studname = "";
                                string degree = "";
                                string sem = "";
                                string hostel_name = "";
                                string floor_name = "";
                                string room_no = "";
                                rowstr = FpSpread1.Sheets[0].RowCount++;
                                studname = dsstudent.Tables[0].Rows[stucount]["stud_name"].ToString();
                                degree = dsstudent.Tables[0].Rows[stucount]["degree"].ToString();
                                sem = dsstudent.Tables[0].Rows[stucount]["current_semester"].ToString();
                                hostel_name = dsstudent.Tables[0].Rows[stucount]["hostelname"].ToString();
                                floor_name = dsstudent.Tables[0].Rows[stucount]["floor_name"].ToString();
                                room_no = dsstudent.Tables[0].Rows[stucount]["room_name"].ToString();
                                FpSpread1.Sheets[0].Cells[rowstr, 1].Text = degree;
                                FpSpread1.Sheets[0].Cells[rowstr, 0].Text = studname;

                                FpSpread1.Sheets[0].Cells[rowstr, 2].Text = sem;
                                FpSpread1.Sheets[0].Cells[rowstr, 2].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[rowstr, 3].Text = hostel_name;
                                FpSpread1.Sheets[0].Cells[rowstr, 4].Text = floor_name;

                                FpSpread1.Sheets[0].Cells[rowstr, 5].Text = room_no;
                            }
                        }
                        if (rowstr == 0)
                        {
                            lblstudent.Visible = true;
                            FpSpread1.Visible = false;
                        }
                    }
                }
                if (FpSpread1.Sheets[0].RowCount == 1)
                {
                    FpSpread1.Visible = false;
                    lblstudent.Visible = true;
                }
                Double totalRows = 0;
                totalRows = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);
                if (totalRows >= 10)
                {
                    FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                    FpSpread1.Height = 350;
                    FpSpread1.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                    FpSpread1.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                }
                else if (totalRows == 0)
                {
                    DropDownListpage.Items.Add("0");
                    FpSpread1.Height = 300;
                }
                else
                {
                    FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                }
                Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread1.Sheets[0].PageSize);
            }
        }
    }
    public string Attmark(string Attstr_mark)
    {

        string Att_mark;
        Att_mark = "";
        if (Attstr_mark == "1")
        {
            Att_mark = "P";

        }

        else if (Attstr_mark == "2")
        {
            Att_mark = "A";

        }
        else if (Attstr_mark == "3")
        {
            Att_mark = "OD";

        }
        else if (Attstr_mark == "4")
        {
            Att_mark = "ML";

        }
        else if (Attstr_mark == "5")
        {
            Att_mark = "SOD";

        }
        else if (Attstr_mark == "10")
        {
            Att_mark = "L";

        }
        else if (Attstr_mark == "11")
        {
            Att_mark = "NSS";

        }
        if (Attstr_mark == "12")
        {
            Att_mark = "HS";

        }
        else if (Attstr_mark == "13")
        {
            Att_mark = "PP";

        }

        else if (Attstr_mark == "14")
        {
            Att_mark = "SYOD";

        }
        else if (Attstr_mark == "15")
        {
            Att_mark = "COD";

        }
        else if (Attstr_mark == "16")
        {
            Att_mark = "OOD";
        }
        else if (Attstr_mark == "17")
        {
            Att_mark = "LA";
        }
        return Att_mark;


    }
    protected void cbosession_SelectedIndexChanged(object sender, EventArgs e)
    {
        psession.Focus();
        // cbldepttype.Focus();
        int seatcount = 0;
        string value = "";
        string code = "";


        for (int i = 0; i < cbosession.Items.Count; i++)
        {
            if (cbosession.Items[i].Selected == true)
            {
                value = cbosession.Items[i].Text;
                code = cbosession.Items[i].Value.ToString();
                seatcount = seatcount + 1;
                txtsession.Text = "Session(" + seatcount.ToString() + ")";
            }

        }

        if (seatcount == 0)
            txtsession.Text = "---Select---";
        else
        {

        }
        seatcnt = seatcount;
    }



    protected void chksession_CheckedChanged(object sender, EventArgs e)
    {
        if (chksession.Checked == true)
        {
            for (int i = 0; i < cbosession.Items.Count; i++)
            {
                cbosession.Items[i].Selected = true;
                txtsession.Text = "Session(" + (cbosession.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cbosession.Items.Count; i++)
            {
                cbosession.Items[i].Selected = false;

                txtsession.Text = "---Select---";

            }
        }
    }


    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {

        TextBoxother.Text = "";

        if (DropDownListpage.Text == "Others")
        {

            TextBoxother.Visible = true;
            TextBoxother.Focus();

        }
        else
        {
            TextBoxother.Visible = false;
            fpmessreport.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            CalculateTotalPages();
        }
        fpmessreport.CurrentPage = 0;
    }
    protected void TextBoxother_TextChanged(object sender, EventArgs e)
    {
        try
        {

            if (TextBoxother.Text != "")
            {
                fpmessreport.Visible = true;

                fpmessreport.Sheets[0].PageSize = Convert.ToInt16(TextBoxother.Text.ToString());

                // FpSpread1.Height = 30 + (38 * Convert.ToInt32(FpSpread1.Sheets[0].RowCount));
                CalculateTotalPages();

            }
        }
        catch
        {
        }
    }
    protected void TextBoxpage_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (TextBoxpage.Text.Trim() != "")
            {
                if (Convert.ToInt64(TextBoxpage.Text) > Convert.ToInt64(Session["totalPages"]))
                {
                    LabelE.Visible = true;
                    LabelE.Text = "Exceed The Page Limit";
                    TextBoxpage.Text = "";
                    lblexceedpage.Visible = false;
                    fpmessreport.Visible = true;
                }
                else if (Convert.ToInt32(TextBoxpage.Text) == 0)
                {
                    LabelE.Visible = true;
                    LabelE.Text = "Search should be greater than zero";
                    TextBoxpage.Text = "";
                }
                else
                {
                    LabelE.Visible = false;
                    fpmessreport.CurrentPage = Convert.ToInt32(TextBoxpage.Text) - 1;
                    fpmessreport.Visible = true;
                    LabelE.Visible = false;
                }
            }
        }
        catch
        {
            lblexceedpage.Visible = true;
            lblexceedpage.Text = "Please Give The Valid Page";

        }

    }


    void CalculateTotalPages()
    {
        Double totalRows = 0;
        totalRows = Convert.ToInt32(fpmessreport.Sheets[0].RowCount);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / fpmessreport.Sheets[0].PageSize);
        Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        //Buttontotal.Visible = true;
    }
}