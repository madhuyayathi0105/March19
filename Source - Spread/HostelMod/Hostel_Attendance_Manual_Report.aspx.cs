using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Configuration;

public partial class Hostel_Attendance_Manual_Report : System.Web.UI.Page
{
    string collegecode1 = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    DataSet dsroom = new DataSet();
    int righ = 0;
    static string floorname = string.Empty;

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
        if (rdbhostel.Checked == true)
        {
            lblhostel.Visible = true;
            ddl_Hostel.Visible = true;
            lblAttendance.Visible = true;
            txt_attandance.Visible = true;
            lblAttendance_to.Visible = true;
            txt_attandance_to.Visible = true;
            Lblsession.Visible = true;
            ddlsession.Visible = true;
            Lblstatus.Visible = true;
            ddl_status.Visible = true;
            btn_go.Visible = true;
        }
        else if (rdbmess.Checked == true)
        {
            lblhostel.Visible = true;
            ddl_Hostel.Visible = true;
            lblAttendance.Visible = true;
            txt_attandance.Visible = true;
            lblAttendance_to.Visible = true;
            txt_attandance_to.Visible = true;
            Lblsession.Visible = true;
            ddlsession.Visible = true;
            Lblstatus.Visible = true;
            ddl_status.Visible = true;
            btn_go.Visible = true;
        }
        else
        {
            lblhostel.Visible = false;
            ddl_Hostel.Visible = false;
            lblAttendance.Visible = false;
            txt_attandance.Visible = false;
            lblAttendance_to.Visible = false;
            txt_attandance_to.Visible = false;
            Lblsession.Visible = false;
            ddlsession.Visible = false;
            Lblstatus.Visible = false;
            ddl_status.Visible = false;
            btn_go.Visible = false;
        }
        if (!IsPostBack)
        {
            string rights = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Attendance' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
            int.TryParse(rights, out righ);
            if (righ == 1)
                rdbhostel.Enabled = true;
            else
                rdbhostel.Enabled = false;
            if (righ == 2)
                rdbmess.Enabled = true;
            else
                rdbmess.Enabled = false;
            if (righ == 3)
            {
                rdbmess.Enabled = true;
                rdbhostel.Enabled = true;
            }
            else
            {
                if (righ == 1)
                    rdbhostel.Enabled = true;
                else
                    rdbhostel.Enabled = false;
                if (righ == 2)
                    rdbmess.Enabled = true;
                else
                    rdbmess.Enabled = false;

            }
            Fpspread6.Visible = false;
            loadsession();
            loadhostel();
            bindhostel();
            load_ddlrollno();
            status();
            bindbuilding();
            bindfloor();
            bindroom();
           
        }
    }
    public void loadsession()
    {
        try
        {
            ds.Clear();
            string deptquery = "select  SessionMasterPK,SessionName  from HM_SessionMaster where MessMasterFK in ('" + ddl_Hostel.SelectedValue + "') order by SessionMasterPK ";


            ds = d2.select_method_wo_parameter(deptquery, "Text");
            //ds = d2.BindSession(itemheader);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsession.DataSource = ds;
                ddlsession.DataTextField = "SessionName";
                ddlsession.DataValueField = "SessionMasterPK";
                ddlsession.DataBind();
            }
        }


        catch
        {
        }
    }
    public void status()
    {
        try
        {
            if (rdbhostel.Checked == true)
            {
                ddlstatus.Items.Clear();
                ddlstatus.Items.Add("Present");
                ddlstatus.Items.Add("Absent");
                ddlstatus.Items.Add("OD");
                ddlstatus.Items.Add("All");
            }
            else
            {
                ddlstatus.Items.Clear();
                ddlstatus.Items.Add("Present");
                ddlstatus.Items.Add("Absent");
                
                ddlstatus.Items.Add("All");
            }
            
        }


        catch
        {
        }
    }

    public void loadhostel()
    {

        try
        {

            ddl_Hostel.Items.Clear();
            ds.Clear();


            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_Hostel.DataSource = ds;
                ddl_Hostel.DataTextField = "MessName";
                ddl_Hostel.DataValueField = "MessMasterPK";
                ddl_Hostel.DataBind();
            }

        }
        catch
        {
        }
    }
    public void bindhostel()
    {
        try
        {
            ds.Clear();
            //string itemname = "select HostelMasterPK,HostelName  from HM_HostelMaster ";// where CollegeCode in ('" + ddl_college.SelectedItem.Value + "') order by HostelMasterPK ";
            //ds = d2.select_method_wo_parameter(itemname, "Text");
            //ddl_Hostel.Items.Clear();
            // ds = d2.BindHostel(ddl_college.SelectedItem.Value);
            string MessmasterFK = string.Empty;
            if (usercode != "" && usercode != "0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Rights' and user_code='" + usercode + "'");
            if (group_user != "" && group_user != "0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Rights' and user_code='" + group_user + "'");
            string itemname = "select HostelMasterPK,HostelName from HM_HostelMaster where  HostelMasterPK in (" + MessmasterFK + ") order by hostelname ";
            ds = d2.select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_Hostel.DataSource = ds;
                ddl_Hostel.DataTextField = "HostelName";
                ddl_Hostel.DataValueField = "HostelMasterPK";
                ddl_Hostel.DataBind();
            }

        }
        catch
        {

        }
    }
    protected void rdbhostel_CheckedChange(object sender, EventArgs e)
    {
        try
        {

            ddl_Hostel.Items.Clear();
            ddlsession.Items.Clear();
            lblhostel.Text = "Hostel Name";
            lblhostel.Visible = true;
            ddl_Hostel.Visible = true;
            lblAttendance.Visible = true;
            txt_attandance.Visible = true;
            Lblsession.Visible = true;
            ddlsession.Visible = true;
            Lblstatus.Visible = true;
            ddl_status.Visible = true;
            btn_go.Visible = true;
            lblAttendance_to.Visible = true;
            txt_attandance_to.Visible = true;
            Label1.Visible = true;
            ddlstatus.Visible = true;
            ddlsession.Items.Add(new System.Web.UI.WebControls.ListItem("Morning", "0"));
            ddlsession.Items.Add(new System.Web.UI.WebControls.ListItem("Evening", "1"));
            bindhostel();
            status();
            Lblroom.Visible = true;
            lbl_floorname.Visible = true;
            txt_floorname.Visible = true;
            Lblroom.Visible = true;
            txt_room.Visible = true;
            Label2.Visible = true;
            drbbuilding.Visible = true;
           
            pflrnm.Visible = true;
            panel_room.Visible = true;
            updatepanel_room.Visible = true;
        }
        catch
        {
        }
    }
    protected void rdbmess_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            lblhostel.Text = "Mess Name";
            lblhostel.Visible = true;
            ddl_Hostel.Visible = true;
            lblAttendance.Visible = true;
            txt_attandance.Visible = true;
            Lblsession.Visible = true;
            ddlsession.Visible = true;
            Lblstatus.Visible = true;
            ddl_status.Visible = true;
            btn_go.Visible = true;
            loadhostel();
            loadsession();
            Fpspread6.Visible = false;
            rptprint1.Visible = false;
            Label1.Visible = true;
            ddlstatus.Visible = true;
            status();
            Lblroom.Visible = false;
            lbl_floorname.Visible = false;
            txt_floorname.Visible = false;
            Lblroom.Visible = false;
            Label2.Visible = false;
            drbbuilding.Visible = false;
        
            txt_room.Visible = false;
            pflrnm.Visible = false;
            panel_room.Visible = false;
            updatepanel_room.Visible = false;
        }
        catch
        {
        }
    }
    protected void rdbstudy_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            lblhostel.Text = "Hostel Name";

        }
        catch
        {
        }
    }
    protected void Go_Click(object sender, EventArgs e)
    {
        try
        {
            if (rdbhostel.Checked == true)
            {
                Fpspread6.Visible = false;
                rptprint1.Visible = false;

                if (txt_attandance.Text != "" && ddlsession.SelectedValue != "")
                {
                    string firstdate1 = Convert.ToString(txt_attandance.Text);
                    string seconddate1 = Convert.ToString(txt_attandance_to.Text);
                    string fdmonth = "";
                    string sdmonth = "";
                    string fdyear = "";
                    string sdyear = "";
                    string month = "";
                    string year = "";
                    string monthyear = "";
                    string fdday = "";
                    string sdday = "";
                    int colcount = 0;
                    int monthvalue = 0;

                    string[] splitt = firstdate1.Split('/');
                    string[] splitt1 = seconddate1.Split('/');
                    fdmonth = Convert.ToString(splitt[1]);
                    sdmonth = Convert.ToString(splitt1[1]);
                    fdyear = Convert.ToString(splitt[2]);
                    sdyear = Convert.ToString(splitt1[2]);
                    fdday = Convert.ToString(splitt[0]);
                    sdday = Convert.ToString(splitt1[0]);
                    string floors = "";
                    string hos = string.Empty;
                    string buildname = string.Empty;


                    if (Convert.ToString(ddl_Hostel.SelectedValue) != "")
                    {
                        hos = "" + ddl_Hostel.SelectedValue + "";

                        string build = d2.GetBuildingCode_inv(hos);
                        char[] delimiterChars = { ',' };
                        string[] build1 = build.Split(delimiterChars);
                        string build2 = "";

                        foreach (string b in build1)
                        {
                            if (build2 == "")
                            {
                                build2 = "" + b + "";
                            }
                            else
                            {
                                build2 = build2 + "'" + "," + "'" + b + "";
                            }
                        }

                        DataSet ds1 = new DataSet();
                        ds1.Clear();
                        string floor = "select code,Building_Name from Building_Master where code in ('" + build2 + "')";
                        ds1 = d2.select_method_wo_parameter(floor, "Text");

                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            string q1 = Convert.ToString(ds1.Tables[0].Rows[0][1]);
                            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                            {
                                string q = Convert.ToString(ds1.Tables[0].Rows[i][1]);
                                if (buildname == "")
                                {
                                    buildname = "" + q + "";
                                }
                                else
                                {
                                    buildname = buildname + "'" + "," + "'" + q + "";
                                }
                            }
                        }
                    }
                    if (cbl_floorname.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_floorname.Items.Count; i++)
                        {
                            if (cbl_floorname.Items[i].Selected == true)
                            {
                                if (floors == "")
                                {
                                    floors = "" + cbl_floorname.Items[i].Text.ToString() + "";
                                }
                                else
                                {
                                    floors = floors + "'" + "," + "'" + cbl_floorname.Items[i].Text.ToString() + "";
                                }
                            }
                        }
                    }
                    string rooms = "";
                    if (cbl_room.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_room.Items.Count; i++)
                        {
                            if (cbl_room.Items[i].Selected == true)
                            {
                                if (rooms == "")
                                {
                                    rooms = "" + cbl_room.Items[i].Text.ToString() + "";
                                }
                                else
                                {
                                    rooms = rooms + "'" + "," + "'" + cbl_room.Items[i].Text.ToString() + "";
                                }
                            }
                        }
                    }





                    string securityrights = string.Empty;
                         string securityrights1 = string.Empty;
                         string room = "select Roll_No,r.Reg_No,Stud_Name,h.id, h.app_no,h.HostelMasterFK,hm.HostelName,bm.Building_Name,fm.Floor_Name,rm.Room_Name,r.Batch_Year,ha.AttnMonth,ha.AttnYear,ha.App_No,[D1],[D2],[D3],[D4],[D5],[D6],[D7],[D8],[D9],[D10],[D11],[D12],[D13],[D14],[D15],[D16],[D17],[D18],[D19],[D20],[D21],[D22],[D23],[D24],[D25],[D26],[D27],[D28],[D29],[D30],[D31],[D1E],[D2E],[D3E],[D4E],[D5E],[D6E],[D7E],[D8E],[D9E],[D10E],[D11E],[D12E],[D13E],[D14E],[D15E],[D16E],[D17E],[D18E],[D19E],[D20E],[D21E],[D22E],[D23E],[D24E],[D25E],[D26E],[D27E],[D28E],[D29E],[D30E],[D31E] from HT_HostelRegistration h,Registration r,HM_HostelMaster hm,HT_Attendance HA,Building_Master bm,Floor_Master fm,Room_Detail rm,Degree d,Department dt,Course c where h.APP_No=r.App_No and h.HostelMasterFK=hm.HostelMasterPK and ha.App_No=h.APP_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id  and ISNULL(h.IsDiscontinued,0)=0 and ISNULL(h.IsSuspend,0) =0 and ISNULL(h.IsVacated,0) =0  and h.HostelMasterFK in('" + ddl_Hostel.SelectedValue + "')   and AttnMonth between '" + fdmonth + "' and '" + sdmonth + "' and AttnYear between '" + fdyear + "' and '" + sdyear + "' and bm.Code=h.BuildingFK and fm.Floorpk=h.FloorFK and rm.Roompk=h.RoomFK and rm.Room_Name in('" + rooms + "') and fm.Floor_Name in('" + floors + "') order by h.APP_No,Roll_No,AttnMonth";
                    dsroom = d2.select_method_wo_parameter(room, "text");

                    if (Convert.ToString(ddl_status.SelectedItem) == "Roll No")
                    {
                        securityrights = "Roll_No";
                        securityrights1 = "Roll No";

                    }
                    if (Convert.ToString(ddl_status.SelectedItem) == "Hostel Id")
                    {
                        securityrights = "id";
                        securityrights1 = "Student Id";
                    }
                    if (Convert.ToString(ddl_status.SelectedItem) == "Reg No")
                    {
                        securityrights = "Reg_No";
                        securityrights1 = "Reg No";
                    }

                    Fpspread6.Visible = false;
                    Fpspread6.Sheets[0].RowCount = 0;
                    Fpspread6.Sheets[0].ColumnCount = 5;
                    Fpspread6.CommandBar.Visible = false;
                    Fpspread6.Sheets[0].AutoPostBack = false;
                    Fpspread6.Sheets[0].ColumnHeader.RowCount = 3;
                    Fpspread6.Sheets[0].RowHeader.Visible = false;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = System.Drawing.Color.White;
                    Fpspread6.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;


                    if (dsroom.Tables[0].Rows.Count > 0)
                    {
                        Fpspread6.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, 4);
                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, 0].BackColor = System.Drawing.Color.White;




                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "S.No";
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Bold = true;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Name = "Book Antiqua";
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Size = FontUnit.Medium;
                        Fpspread6.Columns[colcount].Width = 50;
                        colcount++;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Text = securityrights1;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Bold = true;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Name = "Book Antiqua";
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Size = FontUnit.Medium;
                        Fpspread6.Columns[colcount].Width = 80;
                        colcount++;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "Student Name";
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Bold = true;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Name = "Book Antiqua";
                        Fpspread6.Columns[colcount].Width = 200;
                        colcount++;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "Room No";
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Bold = true;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Name = "Book Antiqua";
                        Fpspread6.Columns[colcount].Width = 100;
                        colcount++;
                        //if (ddlsession.SelectedValue == "0")
                        //{

                        int dd = 0, d = 0;
                        int fdmonth1 = Convert.ToInt32(fdmonth);
                        int sdmonth1 = Convert.ToInt32(sdmonth);
                        int fdday1 = Convert.ToInt32(fdday);
                        int fdday2 = Convert.ToInt32(fdday);
                        int sdday1 = Convert.ToInt32(sdday);
                        int col=4;
                        month = dsroom.Tables[0].Rows[0]["AttnMonth"].ToString();
                        year = dsroom.Tables[0].Rows[0]["AttnYear"].ToString();
                        do
                        {

                        month = Convert.ToString(fdmonth1);
                        year = Convert.ToString(fdyear);
                        switch (month)
                        {
                            //case "1": monthyear = "January " + year;

                            //    break;
                            //case "2": monthyear = "February " + year;
                            //    break;
                            //case "3": monthyear = "March " + year;
                            //    break;
                            //case "4": monthyear = "April " + year;
                            //    break;
                            //case "5": monthyear = "May " + year;
                            //    break;
                            //case "6": monthyear = "June " + year;
                            //    break;
                            //case "7": monthyear = "July " + year;
                            //    break;
                            //case "8": monthyear = "August " + year;
                            //    break;
                            //case "9": monthyear = "September " + year;
                            //    break;
                            //case "10": monthyear = "October " + year;
                            //    break;
                            //case "11": monthyear = "November " + year;
                            //    break;
                            //case "12": monthyear = "December " + year;
                            //    break;


                            case "1": monthyear = "January " + year;
                                monthvalue = 31;
                                break;
                            case "2": monthyear = "February " + year;
                                monthvalue = 28;
                                break;
                            case "3": monthyear = "March " + year;
                                monthvalue = 31;
                                break;
                            case "4": monthyear = "April " + year;
                                monthvalue = 30;
                                break;
                            case "5": monthyear = "May " + year;
                                monthvalue = 31;
                                break;
                            case "6": monthyear = "June " + year;
                                monthvalue = 30;
                                break;
                            case "7": monthyear = "July " + year;
                                monthvalue = 31;
                                break;
                            case "8": monthyear = "August " + year;
                                monthvalue = 31;
                                break;
                            case "9": monthyear = "September " + year;
                                monthvalue = 30;
                                break;
                            case "10": monthyear = "October " + year;
                                monthvalue = 31;
                                break;
                            case "11": monthyear = "November " + year;
                                monthvalue = 30;
                                break;
                            case "12": monthyear = "December " + year;
                                monthvalue = 31;
                                break;
                        }


                              


                                if (fdmonth == sdmonth)
                                {
                                    d = sdday1 - fdday1; ;
                                    d++;
                                    dd += d;
                                    Fpspread6.Sheets[0].ColumnCount += d;
                                }
                                else if (fdday1 != 1)
                                {
                                    d = monthvalue - fdday1; ;
                                    d++;
                                    dd += d;
                                    Fpspread6.Sheets[0].ColumnCount += d;

                                }
                                else
                                {
                                    d = monthvalue;
                                    dd += monthvalue;
                                    Fpspread6.Sheets[0].ColumnCount += monthvalue;

                                }

                                
                                Fpspread6.Sheets[0].ColumnHeaderSpanModel.Add(2, 2, 1, Fpspread6.Sheets[0].ColumnCount);
                                
                                int ii = fdday1;

                                for (int i = colcount; i < Fpspread6.Sheets[0].ColumnCount - 1; i++)
                                {


                                    Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Text = ii.ToString();
                                    Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Bold = true;
                                    Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Name = "Book Antiqua";
                                    Fpspread6.Columns[colcount].Width = 40;
                                    colcount++;
                                    ii++;
                                    fdday1 = 1;
                                    
                                   
                                }
                                fdmonth1++;

                                Fpspread6.Sheets[0].ColumnHeader.Cells[0, col].Text = monthyear;

                                Fpspread6.Sheets[0].ColumnHeader.Cells[0, col].Font.Bold = true;
                                Fpspread6.Sheets[0].ColumnHeader.Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread6.Sheets[0].ColumnHeader.Cells[0, col].Font.Name = "Book Antiqua";
                                Fpspread6.Sheets[0].ColumnHeader.Cells[0, col].Font.Size = FontUnit.Medium;
                                Fpspread6.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, d);

                                col += d;
                               

                        } while (fdmonth1 <= sdmonth1) ;

                        int sno = 0;
                        Fpspread6.Sheets[0].ColumnHeaderSpanModel.Add(2, 0, 1, 2);
                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 0].Text = "Block Name:";
                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 0].Font.Bold = true;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 0].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 0].Font.Name = "Book Antiqua";
                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 0].Font.Size = FontUnit.Medium;

                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 2].Text = dsroom.Tables[0].Rows[0]["Building_Name"].ToString();
                       
                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 2].Font.Bold = true;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 2].Font.Name = "Book Antiqua";
                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 2].Font.Size = FontUnit.Medium;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 2].BackColor = System.Drawing.Color.White;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 2].ForeColor = System.Drawing.Color.Black;


                        for (int r = 0; r < dsroom.Tables[0].Rows.Count; r++)
                        {
                            Boolean chec = false;
                            sno++;
                            int m = fdday2;
                            Fpspread6.Sheets[0].Rows.Count++;
                            Fpspread6.Rows[Fpspread6.Sheets[0].Rows.Count - 1].Height = 40;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].Text = sno.ToString();
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].Locked = true;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].VerticalAlign = VerticalAlign.Middle;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].Font.Name = "Book Antiqua";
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].ForeColor = System.Drawing.Color.Black;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].Font.Bold = true;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Center;



                            Fpspread6.Rows[Fpspread6.Sheets[0].Rows.Count - 1].Height = 40;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].Text = dsroom.Tables[0].Rows[r][securityrights].ToString();
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].Locked = true;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].VerticalAlign = VerticalAlign.Middle;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].Font.Name = "Book Antiqua";
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].ForeColor = System.Drawing.Color.Black;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].Font.Bold = true;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Left;



                            Fpspread6.Rows[Fpspread6.Sheets[0].Rows.Count - 1].Height = 40;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 2].Text = dsroom.Tables[0].Rows[r]["Stud_Name"].ToString();
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 2].Locked = true;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 2].VerticalAlign = VerticalAlign.Middle;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 2].Font.Name = "Book Antiqua";
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 2].ForeColor = System.Drawing.Color.Black;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 2].Font.Bold = true;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 2].HorizontalAlign = HorizontalAlign.Left;


                            Fpspread6.Rows[Fpspread6.Sheets[0].Rows.Count - 1].Height = 40;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 3].Text = dsroom.Tables[0].Rows[r]["Room_Name"].ToString();
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 3].Locked = true;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 3].VerticalAlign = VerticalAlign.Middle;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 3].Font.Name = "Book Antiqua";
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 3].ForeColor = System.Drawing.Color.Black;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 3].Font.Bold = true;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            if (ddlsession.SelectedValue == "0")
                            {

                               
                               // fdday1 = 1;
                                fdmonth1++;
                               
                                int rr = r;
                                if (dsroom.Tables[0].Rows.Count > rr + 1)
                                {
                                    while (dsroom.Tables[0].Rows[rr]["Roll_No"].ToString() == dsroom.Tables[0].Rows[rr + 1]["Roll_No"].ToString())
                                    {

                                       // col += 31;
                                        rr++;
                                        if (dsroom.Tables[0].Rows.Count - 1 == rr)
                                            break;

                                    }

                                } 
                                if (Fpspread6.Sheets[0].ColumnCount<col)
                                    Fpspread6.Sheets[0].ColumnCount = col;
                                int j = 0;
                                int fdmonth2 = Convert.ToInt32(fdmonth);
                                month = Convert.ToString(fdmonth2);
                                year = dsroom.Tables[0].Rows[r]["AttnYear"].ToString();
                                switch (month)
                                {
                                    


                                    case "1": monthyear = "January " + year;
                                        monthvalue = 31;
                                        break;
                                    case "2": monthyear = "February " + year;
                                        monthvalue = 28;
                                        break;
                                    case "3": monthyear = "March " + year;
                                        monthvalue = 31;
                                        break;
                                    case "4": monthyear = "April " + year;
                                        monthvalue = 30;
                                        break;
                                    case "5": monthyear = "May " + year;
                                        monthvalue = 31;
                                        break;
                                    case "6": monthyear = "June " + year;
                                        monthvalue = 30;
                                        break;
                                    case "7": monthyear = "July " + year;
                                        monthvalue = 31;
                                        break;
                                    case "8": monthyear = "August " + year;
                                        monthvalue = 31;
                                        break;
                                    case "9": monthyear = "September " + year;
                                        monthvalue = 30;
                                        break;
                                    case "10": monthyear = "October " + year;
                                        monthvalue = 31;
                                        break;
                                    case "11": monthyear = "November " + year;
                                        monthvalue = 30;
                                        break;
                                    case "12": monthyear = "December " + year;
                                        monthvalue = 31;
                                        break;
                                }
                                int clmonth = monthvalue;
                                for (int colno = 4; colno < col; colno++)
                                {

                                    j = m;
                                   


                                    if (m == clmonth+1)
                                    {
                                        fdmonth2++;
                                       // r++;
                                        m = 1;
                                        j = m;
                                     
                                        //Fpspread6.Sheets[0].ColumnHeaderSpanModel.Add(2, colno, 1, 31);
                                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, colno].BackColor = System.Drawing.Color.White;
                                        month = dsroom.Tables[0].Rows[r]["AttnMonth"].ToString();
                                        year = dsroom.Tables[0].Rows[r]["AttnYear"].ToString();
                                        month =Convert.ToString(fdmonth2);
                                        switch (month)
                                        {
                                            case "1": monthyear = "January " + year;
                                                monthvalue = 31;
                                                break;
                                            case "2": monthyear = "February " + year;
                                                monthvalue = 28;
                                                break;
                                            case "3": monthyear = "March " + year;
                                                monthvalue = 31;
                                                break;
                                            case "4": monthyear = "April " + year;
                                                monthvalue = 30;
                                                break;
                                            case "5": monthyear = "May " + year;
                                                monthvalue = 31;
                                                break;
                                            case "6": monthyear = "June " + year;
                                                monthvalue = 30;
                                                break;
                                            case "7": monthyear = "July " + year;
                                                monthvalue = 31;
                                                break;
                                            case "8": monthyear = "August " + year;
                                                monthvalue = 31;
                                                break;
                                            case "9": monthyear = "September " + year;
                                                monthvalue = 30;
                                                break;
                                            case "10": monthyear = "October " + year;
                                                monthvalue = 31;
                                                break;
                                            case "11": monthyear = "November " + year;
                                                monthvalue = 30;
                                                break;
                                            case "12": monthyear = "December " + year;
                                                monthvalue = 31;
                                                break;
                                                

                                        }
                                        clmonth = monthvalue;
                                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, colno].Text = monthyear;
                                        //Fpspread6.Sheets[0].ColumnHeaderSpanModel.Add(0, colno, 1, 31);
                                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, colno].Font.Bold = true;
                                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, colno].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, colno].Font.Name = "Book Antiqua";
                                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, colno].Font.Size = FontUnit.Medium;
                                        //int colcount1 = colno;
                                        //for (int i = 1; i < 32; i++)
                                        //{


                                        //    Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount1].Text = i.ToString();
                                        //    Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount1].Font.Bold = true;
                                        //    Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount1].HorizontalAlign = HorizontalAlign.Center;
                                        //    Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount1].Font.Name = "Book Antiqua";
                                        //    Fpspread6.Columns[colcount1].Width = 40;
                                        //    colcount1++;
                                        //}

                                    }
                                    m++;

                                    Fpspread6.Rows[Fpspread6.Sheets[0].Rows.Count - 1].Height = 40;
                                    if (Convert.ToString(ddlstatus.SelectedItem) == "All")
                                    {
                                        chec = true;
                                        Fpspread6.Sheets[0].Rows[Fpspread6.Sheets[0].Rows.Count - 1].Visible = true;
                                        if (Convert.ToString(fdmonth2) == dsroom.Tables[0].Rows[r]["AttnMonth"].ToString())
                                        {
                                            if (dsroom.Tables[0].Rows[r]["D" + j.ToString()].ToString() == "" || dsroom.Tables[0].Rows[r]["D" + j.ToString()].ToString() == "0")
                                            {
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "--";
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Black;

                                            }
                                            else if (dsroom.Tables[0].Rows[r]["D" + j.ToString()].ToString() == "1")
                                            {
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "P";
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Green;

                                            }
                                            else if (dsroom.Tables[0].Rows[r]["D" + j.ToString()].ToString() == "2")
                                            {
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "A";
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Red;
                                            }
                                            else if (dsroom.Tables[0].Rows[r]["D" + j.ToString()].ToString() == "3")
                                            {
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "OD";
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Blue;
                                            }
                                        }
                                    }

                                    else   if (Convert.ToString(ddlstatus.SelectedItem) == "Present")
                                    {
                                        if (Convert.ToString(fdmonth2) == dsroom.Tables[0].Rows[r]["AttnMonth"].ToString())
                                        {
                                            if (dsroom.Tables[0].Rows[r]["D" + j.ToString()].ToString() == "" || dsroom.Tables[0].Rows[r]["D" + j.ToString()].ToString() == "0")
                                            {
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "--";
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Black;

                                            }
                                            else if (dsroom.Tables[0].Rows[r]["D" + j.ToString()].ToString() == "1")
                                            {
                                                chec = true;
                                                Fpspread6.Sheets[0].Rows[Fpspread6.Sheets[0].Rows.Count - 1].Visible = true;
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "P";
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Green;

                                            }
                                            
                                            
                                        }
                                    }

                                    else if (Convert.ToString(ddlstatus.SelectedItem) == "Absent")
                                    {
                                        if (Convert.ToString(fdmonth2) == dsroom.Tables[0].Rows[r]["AttnMonth"].ToString())
                                        {
                                            if (dsroom.Tables[0].Rows[r]["D" + j.ToString()].ToString() == "" || dsroom.Tables[0].Rows[r]["D" + j.ToString()].ToString() == "0")
                                            {
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "--";
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Black;

                                            }
                                            else if (dsroom.Tables[0].Rows[r]["D" + j.ToString()].ToString() == "2")
                                            {
                                                chec = true;
                                                Fpspread6.Sheets[0].Rows[Fpspread6.Sheets[0].Rows.Count - 1].Visible = true;
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "A";
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Red;
                                            }


                                        }
                                    }

                                    else if (Convert.ToString(ddlstatus.SelectedItem) == "OD")
                                    {
                                        if (Convert.ToString(fdmonth2) == dsroom.Tables[0].Rows[r]["AttnMonth"].ToString())
                                        {
                                            if (dsroom.Tables[0].Rows[r]["D" + j.ToString()].ToString() == "" || dsroom.Tables[0].Rows[r]["D" + j.ToString()].ToString() == "0")
                                            {
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "--";
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Black;

                                            }
                                            else if (dsroom.Tables[0].Rows[r]["D" + j.ToString()].ToString() == "3")
                                            {
                                                chec = true;
                                                Fpspread6.Sheets[0].Rows[Fpspread6.Sheets[0].Rows.Count - 1].Visible = true;
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "OD";
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Blue;
                                            }


                                        }
                                    }


                                    else
                                    {
                                        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "--";
                                        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Black;
                                    }
                                    if (chec == false)
                                        Fpspread6.Sheets[0].Rows[Fpspread6.Sheets[0].Rows.Count - 1].Visible = false;

                                        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Locked = true;
                                        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].VerticalAlign = VerticalAlign.Middle;
                                        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Name = "Book Antiqua";
                                        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Size = FontUnit.Medium;


                                        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Bold = true;
                                        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].HorizontalAlign = HorizontalAlign.Center;

                                    
                                }

                            }

                            else
                            {
                               
                                
                                // fdday1 = 1;
                                fdmonth1++;
                                int rr = r;
                                if (dsroom.Tables[0].Rows.Count > rr + 1)
                                {
                                    while (dsroom.Tables[0].Rows[rr]["Roll_No"].ToString() == dsroom.Tables[0].Rows[rr + 1]["Roll_No"].ToString())
                                    {

                                       // col += 31;
                                        rr++;
                                        if (dsroom.Tables[0].Rows.Count - 1 == rr)
                                            break;

                                    }

                                }
                                if (Fpspread6.Sheets[0].ColumnCount < col)
                                    Fpspread6.Sheets[0].ColumnCount = col;
                                int j = 0;
                                int fdmonth2 = Convert.ToInt32(fdmonth);
                                month = Convert.ToString(fdmonth2);
                                year = dsroom.Tables[0].Rows[r]["AttnYear"].ToString();
                                switch (month)
                                {



                                    case "1": monthyear = "January " + year;
                                        monthvalue = 31;
                                        break;
                                    case "2": monthyear = "February " + year;
                                        monthvalue = 28;
                                        break;
                                    case "3": monthyear = "March " + year;
                                        monthvalue = 31;
                                        break;
                                    case "4": monthyear = "April " + year;
                                        monthvalue = 30;
                                        break;
                                    case "5": monthyear = "May " + year;
                                        monthvalue = 31;
                                        break;
                                    case "6": monthyear = "June " + year;
                                        monthvalue = 30;
                                        break;
                                    case "7": monthyear = "July " + year;
                                        monthvalue = 31;
                                        break;
                                    case "8": monthyear = "August " + year;
                                        monthvalue = 31;
                                        break;
                                    case "9": monthyear = "September " + year;
                                        monthvalue = 30;
                                        break;
                                    case "10": monthyear = "October " + year;
                                        monthvalue = 31;
                                        break;
                                    case "11": monthyear = "November " + year;
                                        monthvalue = 30;
                                        break;
                                    case "12": monthyear = "December " + year;
                                        monthvalue = 31;
                                        break;
                                }
                                int clmonth = monthvalue;
                                for (int colno = 4; colno < col; colno++)
                                {
                                    j = m;
                                   
                                    if (m == clmonth+1)
                                    {
                                        fdmonth2++;
                                        // r++;
                                        m = 1;
                                        j = m;
                                       
                                        Fpspread6.Sheets[0].ColumnHeaderSpanModel.Add(2, colno, 1, 31);
                                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, colno].BackColor = System.Drawing.Color.White;
                                        month = dsroom.Tables[0].Rows[r]["AttnMonth"].ToString();
                                        year = dsroom.Tables[0].Rows[r]["AttnYear"].ToString();
                                        month = Convert.ToString(fdmonth2);
                                        switch (month)
                                        {


                                            case "1": monthyear = "January " + year;
                                                monthvalue = 31;
                                                break;
                                            case "2": monthyear = "February " + year;
                                                monthvalue = 28;
                                                break;
                                            case "3": monthyear = "March " + year;
                                                monthvalue = 31;
                                                break;
                                            case "4": monthyear = "April " + year;
                                                monthvalue = 30;
                                                break;
                                            case "5": monthyear = "May " + year;
                                                monthvalue = 31;
                                                break;
                                            case "6": monthyear = "June " + year;
                                                monthvalue = 30;
                                                break;
                                            case "7": monthyear = "July " + year;
                                                monthvalue = 31;
                                                break;
                                            case "8": monthyear = "August " + year;
                                                monthvalue = 31;
                                                break;
                                            case "9": monthyear = "September " + year;
                                                monthvalue = 30;
                                                break;
                                            case "10": monthyear = "October " + year;
                                                monthvalue = 31;
                                                break;
                                            case "11": monthyear = "November " + year;
                                                monthvalue = 30;
                                                break;
                                            case "12": monthyear = "December " + year;
                                                monthvalue = 31;
                                                break;

                                        }
                                        clmonth = monthvalue;

                                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, colno].Text = monthyear;
                                        Fpspread6.Sheets[0].ColumnHeaderSpanModel.Add(0, colno, 1, 31);
                                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, colno].Font.Bold = true;
                                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, colno].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, colno].Font.Name = "Book Antiqua";
                                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, colno].Font.Size = FontUnit.Medium;
                                        int colcount1 = colno;
                                        //for (int i = 1; i < 32; i++)
                                        //{


                                        //    Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount1].Text = i.ToString();
                                        //    Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount1].Font.Bold = true;
                                        //    Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount1].HorizontalAlign = HorizontalAlign.Center;
                                        //    Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount1].Font.Name = "Book Antiqua";
                                        //    Fpspread6.Columns[colcount1].Width = 40;
                                        //    colcount1++;
                                        //}
                                    }
                                    m++;
                                    Fpspread6.Rows[Fpspread6.Sheets[0].Rows.Count - 1].Height = 40;
                                    if (Convert.ToString(ddlstatus.SelectedItem) == "All")
                                    {
                                        chec = true;
                                        Fpspread6.Sheets[0].Rows[Fpspread6.Sheets[0].Rows.Count - 1].Visible = true;
                                        if (Convert.ToString(fdmonth2) == dsroom.Tables[0].Rows[r]["AttnMonth"].ToString())
                                        {
                                            if (dsroom.Tables[0].Rows[r]["D" + j.ToString() + "E"].ToString() == "" || dsroom.Tables[0].Rows[r]["D" + j.ToString() + "E"].ToString() == "0")
                                            {
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "--";
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Black;
                                            }
                                            else if (dsroom.Tables[0].Rows[r]["D" + j.ToString() + "E"].ToString() == "1")
                                            {
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "P";
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Green;
                                            }
                                            else if (dsroom.Tables[0].Rows[r]["D" + j.ToString() + "E"].ToString() == "2")
                                            {
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "A";
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Red;
                                            }
                                            else if (dsroom.Tables[0].Rows[r]["D" + j.ToString() + "E"].ToString() == "3")
                                            {
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "OD";
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Black;
                                            }
                                        }
                                    }
                                    else if (Convert.ToString(ddlstatus.SelectedItem) == "Present")
                                    {
                                        if (Convert.ToString(fdmonth2) == dsroom.Tables[0].Rows[r]["AttnMonth"].ToString())
                                        {
                                            if (dsroom.Tables[0].Rows[r]["D" + j.ToString() + "E"].ToString() == "" || dsroom.Tables[0].Rows[r]["D" + j.ToString() + "E"].ToString() == "0")
                                            {
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "--";
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Black;
                                            }
                                            else if (dsroom.Tables[0].Rows[r]["D" + j.ToString() + "E"].ToString() == "1")
                                            {
                                                chec = true;
                                                Fpspread6.Sheets[0].Rows[Fpspread6.Sheets[0].Rows.Count - 1].Visible = true;

                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "P";
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Green;
                                            }
                                        }
                                    }
                                    else if (Convert.ToString(ddlstatus.SelectedItem) == "Absent")
                                    {
                                        if (Convert.ToString(fdmonth2) == dsroom.Tables[0].Rows[r]["AttnMonth"].ToString())
                                        {
                                            if (dsroom.Tables[0].Rows[r]["D" + j.ToString() + "E"].ToString() == "" || dsroom.Tables[0].Rows[r]["D" + j.ToString() + "E"].ToString() == "0")
                                            {
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "--";
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Black;
                                            }
                                            else if (dsroom.Tables[0].Rows[r]["D" + j.ToString() + "E"].ToString() == "2")
                                            {
                                                chec = true;
                                                Fpspread6.Sheets[0].Rows[Fpspread6.Sheets[0].Rows.Count - 1].Visible = true;
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "A";
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Red;
                                            }
                                        }
                                    }
                                    else if (Convert.ToString(ddlstatus.SelectedItem) == "OD")
                                    {
                                        if (Convert.ToString(fdmonth2) == dsroom.Tables[0].Rows[r]["AttnMonth"].ToString())
                                        {
                                            if (dsroom.Tables[0].Rows[r]["D" + j.ToString() + "E"].ToString() == "" || dsroom.Tables[0].Rows[r]["D" + j.ToString() + "E"].ToString() == "0")
                                            {
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "--";
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Black;
                                            }
                                            else if (dsroom.Tables[0].Rows[r]["D" + j.ToString() + "E"].ToString() == "3")
                                            {
                                                chec = true;
                                                Fpspread6.Sheets[0].Rows[Fpspread6.Sheets[0].Rows.Count - 1].Visible = true;
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "OD";
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Black;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "--";
                                        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Black;
                                    }
                                    if (chec == false)
                                        Fpspread6.Sheets[0].Rows[Fpspread6.Sheets[0].Rows.Count - 1].Visible = false;
                                        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Locked = true;
                                        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].VerticalAlign = VerticalAlign.Middle;
                                        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Name = "Book Antiqua";
                                        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Size = FontUnit.Medium;


                                        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Bold = true;
                                        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].HorizontalAlign = HorizontalAlign.Center;

                                  
                                    
                                }





                            }


                        }

                        Fpspread6.SaveChanges();

                        Fpspread6.Sheets[0].PageSize = Fpspread6.Sheets[0].RowCount;
                        Fpspread6.Visible = true;
                        rptprint1.Visible = true;
                    }

                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                        Fpspread6.Visible = false;
                        rptprint1.Visible = false;
                    }

                }
                else
                {
                    alertpopwindow.Visible = true;
                    Fpspread6.Visible = false;
                    rptprint1.Visible = false;
                    lblalerterr.Text = "Please set all feild";
                }

            }
            if (rdbmess.Checked == true)
            {
               
                Fpspread6.Visible = false;
                rptprint1.Visible = false;
                if (txt_attandance.Text != "" && ddlsession.SelectedValue != "")
                {
                    string firstdate1 = Convert.ToString(txt_attandance.Text);
                    string seconddate1 = Convert.ToString(txt_attandance_to.Text);
                    string fdday = "";
                    string sdday = "";
                    string fdmonth = "";
                    string sdmonth = "";
                    string fdyear = "";
                    string sdyear = "";
                    string day = "";
                    string month = "";
                    string year = "";
                    string monthyear = "";
                    int colcount = 0;
                    int monthvalue = 0;
                    string securityrights = string.Empty;
                    string securityrights1 = string.Empty;
                    string[] splitt = firstdate1.Split('/');
                    string[] splitt1 = seconddate1.Split('/');
                    fdday = Convert.ToString(splitt[0]);
                    sdday = Convert.ToString(splitt1[0]);
                    fdmonth = Convert.ToString(splitt[1]);
                    sdmonth = Convert.ToString(splitt1[1]);
                    fdyear = Convert.ToString(splitt[2]);
                    sdyear = Convert.ToString(splitt1[2]);
                    string fdyearmonthday=Convert.ToString(splitt[1])+"/"+Convert.ToString(splitt[0])+"/"+Convert.ToString(splitt[2]);
                    string sdyearmonthday = Convert.ToString(splitt1[1]) + "/" + Convert.ToString(splitt1[0]) + "/" + Convert.ToString(splitt1[2]);




                    string room = "select r.Roll_No,r.Reg_No,r.App_No,r.Stud_Name,mm.MessName,hs.id,r.Stud_Type,hs.HostelRegistrationPK,hs.HostelMasterFK,Dt.Dept_Name,C.Course_Name ,r.Current_Semester,r.Sections,(select b.Building_Name from Building_Master b where Code=hs.BuildingFK) as Building_Name,(select f.Floor_Name from Floor_Master f where f.FloorPK=hs.FloorFK) as Floor_Name,(select r.Room_Name from Room_Detail r where r.Roompk=hs.RoomFK) as Room_Name,h.HostelName as Hostel_Name from HM_HostelMaster h,HT_HostelRegistration hs,Registration r,Degree d,Department dt,Course c,HM_MessMaster mm  where h.HostelMasterPK =hs.HostelMasterFK and hs.APP_No  =r.App_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and ISNULL(IsSuspend,'0')=0  and ISNULL(IsVacated,'0') =0 and ISNULL(IsDiscontinued,'0')=0  and  mm.MessMasterPK ='" + ddl_Hostel.SelectedValue + "' and h.HostelMasterPK=hs.HostelMasterFK  and hs.Messcode = mm.MessMasterPK order by r.roll_no asc";
                    room = room + " select  hm.Roll_No,Session_name,Session_Code,Entry_Date,Hostel_Code from HostelMess_Attendance hm where hostel_code in('" + ddl_Hostel.SelectedValue + "') and Session_Code in('" + ddlsession.SelectedValue + "') and Entry_Date  between '" + Convert.ToString(fdyearmonthday) + "' and  '" + Convert.ToString(sdyearmonthday) + "'";//magesh 21.6.18 remove hs.MessMasterFK=mm.MessMasterPK
                    
                    dsroom = d2.select_method_wo_parameter(room, "text");

                    if (Convert.ToString(ddl_status.SelectedItem) == "Roll No")
                    {
                        securityrights = "Roll_No";
                        securityrights1="Roll No";

                    }
                    if (Convert.ToString(ddl_status.SelectedItem) == "Hostel Id")
                    {
                        securityrights = "id";
                        securityrights1 = "Student Id";
                    }
                    if (Convert.ToString(ddl_status.SelectedItem) == "Reg No")
                    {
                        securityrights = "Reg_No";
                        securityrights1 = "Reg No";
                    }



                    Fpspread6.Visible = false;
                    Fpspread6.Sheets[0].RowCount = 0;
                    Fpspread6.Sheets[0].ColumnCount = 3;
                    Fpspread6.CommandBar.Visible = false;
                    Fpspread6.Sheets[0].AutoPostBack = false;
                    Fpspread6.Sheets[0].ColumnHeader.RowCount = 3;
                    Fpspread6.Sheets[0].RowHeader.Visible = false;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = System.Drawing.Color.White;
                    Fpspread6.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;


                    if (dsroom.Tables[0].Rows.Count > 0)
                    {
                        Fpspread6.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, 3);
                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, 0].BackColor = System.Drawing.Color.White;




                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "S.No";
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Bold = true;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Name = "Book Antiqua";
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Size = FontUnit.Medium;
                        Fpspread6.Columns[colcount].Width = 50;
                        colcount++;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Text = securityrights1;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Bold = true;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Name = "Book Antiqua";
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Size = FontUnit.Medium;
                        Fpspread6.Columns[colcount].Width = 80;
                        colcount++;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "Student Name";
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Bold = true;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Name = "Book Antiqua";
                        Fpspread6.Columns[colcount].Width = 200;
                        colcount++;
                       
                        
                        int dd=0,d=0;
                        int fdmonth1 = Convert.ToInt32(fdmonth);
                        int sdmonth1 = Convert.ToInt32(sdmonth);
                        int fdday1 = Convert.ToInt32(fdday);
                        int sdday1 = Convert.ToInt32(sdday);
                        int col = 3;
                        do
                        {

                        month = Convert.ToString(fdmonth1);
                        year = Convert.ToString(fdyear);
                        
                        switch (month)
                        {
                            case "1": monthyear = "January " + year;
                                monthvalue = 31;
                                break;
                            case "2": monthyear = "February " + year;
                                monthvalue = 28;
                                break;
                            case "3": monthyear = "March " + year;
                                monthvalue = 31;
                                break;
                            case "4": monthyear = "April " + year;
                                monthvalue = 30;
                                break;
                            case "5": monthyear = "May " + year;
                                monthvalue = 31;
                                break;
                            case "6": monthyear = "June " + year;
                                monthvalue = 30;
                                break;
                            case "7": monthyear = "July " + year;
                                monthvalue = 31;
                                break;
                            case "8": monthyear = "August " + year;
                                monthvalue = 31;
                                break;
                            case "9": monthyear = "September " + year;
                                monthvalue = 30;
                                break;
                            case "10": monthyear = "October " + year;
                                monthvalue = 31;
                                break;
                            case "11": monthyear = "November " + year;
                                monthvalue = 30;
                                break;
                            case "12": monthyear = "December " + year;
                                monthvalue = 31;
                                break;

                        }

                        if (fdmonth1 == sdmonth1)
                        {
                            d=sdday1 - fdday1;;
                            d++;
                            dd += d;
                            Fpspread6.Sheets[0].ColumnCount +=d ;
                        }
                        else if(fdday1!=1)
                        {
                             d = monthvalue - fdday1; ;
                             d++;
                            dd += d;
                            Fpspread6.Sheets[0].ColumnCount += d;

                        }
                        else
                        {
                            d = monthvalue;
                            dd += monthvalue;
                             Fpspread6.Sheets[0].ColumnCount += monthvalue;

                        }
                        
                           

                        int ii = fdday1;
                        for (int i = colcount; i < Fpspread6.Sheets[0].ColumnCount; i++)
                        {
                            

                            Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Text = ii.ToString();
                            Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Bold = true;
                            Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Name = "Book Antiqua";
                            Fpspread6.Columns[colcount].Width = 40;
                            ii++;
                            colcount++;
                        }

                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, col].Text = monthyear;
                        Fpspread6.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, d);
                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, col].Font.Bold = true;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, col].Font.Name = "Book Antiqua";
                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, col].Font.Size = FontUnit.Medium;
                        col += d;
                        fdday1 = 1;
                        fdmonth1++;
                       
                        }while(fdmonth1 <= sdmonth1);


                        int cc = Fpspread6.Sheets[0].ColumnCount;
                        Fpspread6.Sheets[0].ColumnCount += 3;
                        Fpspread6.Sheets[0].ColumnHeaderSpanModel.Add(0, cc, 1, 3);
                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, cc].BackColor = System.Drawing.Color.White;


                        
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "Total Days";
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Bold = true;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Name = "Book Antiqua";
                        Fpspread6.Columns[colcount].Width = 80;
                        colcount++;


                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "No of Present";
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Bold = true;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Name = "Book Antiqua";
                        Fpspread6.Columns[colcount].Width = 80;
                        colcount++;



                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "No of Absent";
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Bold = true;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Name = "Book Antiqua";
                        Fpspread6.Columns[colcount].Width = 80;
                        colcount++;



                        int sno = 0;
                        Fpspread6.Sheets[0].ColumnHeaderSpanModel.Add(2, 0, 1, 2);
                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 0].Text = "Mess:";
                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 0].Font.Bold = true;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 0].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 0].Font.Name = "Book Antiqua";
                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 0].Font.Size = FontUnit.Medium;

                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 2].Text = dsroom.Tables[0].Rows[0]["MessName"].ToString();
                        Fpspread6.Sheets[0].ColumnHeaderSpanModel.Add(2, 2, 1, dd+4);
                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 2].Font.Bold = true;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 2].Font.Name = "Book Antiqua";
                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 2].Font.Size = FontUnit.Medium;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 2].BackColor = System.Drawing.Color.White;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[2, 2].ForeColor = System.Drawing.Color.Black;


                        for (int r = 0; r < dsroom.Tables[0].Rows.Count; r++)
                        {
                            sno++;
                            Boolean chk = false;
                            Fpspread6.Sheets[0].Rows.Count++;
                            Fpspread6.Rows[Fpspread6.Sheets[0].Rows.Count - 1].Height = 40;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].Text = sno.ToString();
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].Locked = true;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].VerticalAlign = VerticalAlign.Middle;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].Font.Name = "Book Antiqua";
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].ForeColor = System.Drawing.Color.Black;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].Font.Bold = true;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Center;



                            Fpspread6.Rows[Fpspread6.Sheets[0].Rows.Count - 1].Height = 40;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].Text = dsroom.Tables[0].Rows[r][securityrights].ToString();
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].Locked = true;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].VerticalAlign = VerticalAlign.Middle;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].Font.Name = "Book Antiqua";
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].ForeColor = System.Drawing.Color.Black;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].Font.Bold = true;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Left;



                            Fpspread6.Rows[Fpspread6.Sheets[0].Rows.Count - 1].Height = 40;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 2].Text = dsroom.Tables[0].Rows[r]["Stud_Name"].ToString();
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 2].Locked = true;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 2].VerticalAlign = VerticalAlign.Middle;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 2].Font.Name = "Book Antiqua";
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 2].ForeColor = System.Drawing.Color.Black;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 2].Font.Bold = true;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            string fdyearmonthday1 = fdyearmonthday;
                            int fddayy = 0, fdmon = 0, fdyearr = 0;
                            int noofabsents = 0, noofpresents = 0;
                            int colno = 3,colc=4;
                            int presents=0;
                            string[] spli = fdyearmonthday1.Split('/');
                            fddayy = Convert.ToInt32(spli[1]);
                            fdyearr = Convert.ToInt32(spli[2]);
                            fdmon = Convert.ToInt32(spli[0]);

                            fdyearmonthday1 = Convert.ToString(fdmon) + "/" + Convert.ToString(fddayy) + "/" + Convert.ToString(fdyearr);
                            string roolno = dsroom.Tables[0].Rows[r]["Roll_No"].ToString();
                            for (int j = colno; j < Fpspread6.Sheets[0].ColumnCount-3; j++)
                            {
                                presents=0;
                                for (int i = 0; i < dsroom.Tables[1].Rows.Count; i++)
                                {
                                   
                                    string mdy = Convert.ToString(dsroom.Tables[1].Rows[i]["Entry_Date"]); string f = dsroom.Tables[1].Rows[i]["Roll_No"].ToString();
                                    string[] mdy1 = mdy.Split(' ');
                                    if (roolno == dsroom.Tables[1].Rows[i]["Roll_No"].ToString() && fdyearmonthday1 == mdy1[0] && ddlsession.SelectedItem.ToString() == dsroom.Tables[1].Rows[i]["Session_name"].ToString())
                                    {
                                       presents++;
                                       break;
                                        
                                    }
                                    
                                }

                                 if (Convert.ToString(ddlstatus.SelectedItem) == "All")
                                    {
                                if (presents == 1)
                                {
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "P";
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Green;
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Locked = true;
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].VerticalAlign = VerticalAlign.Middle;
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Name = "Book Antiqua";
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Size = FontUnit.Medium;


                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Bold = true;
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].HorizontalAlign = HorizontalAlign.Center;
                                    noofpresents++;
                                    colno++;
                                }
                                else
                                {
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "A";
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Red;
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Locked = true;
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].VerticalAlign = VerticalAlign.Middle;
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Name = "Book Antiqua";
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Size = FontUnit.Medium;


                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Bold = true;
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].HorizontalAlign = HorizontalAlign.Center;
                                    noofabsents++;
                                    colno++;

                                }
                                }
                                 else if (Convert.ToString(ddlstatus.SelectedItem) == "Present")
                                 {
                                     if (presents == 1)
                                     {
                                         chk = true;

                                         Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "P";
                                         Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Green;
                                         Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Locked = true;
                                         Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].VerticalAlign = VerticalAlign.Middle;
                                         Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Name = "Book Antiqua";
                                         Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Size = FontUnit.Medium;


                                         Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Bold = true;
                                         Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].HorizontalAlign = HorizontalAlign.Center;
                                         noofpresents++;

                                         Fpspread6.Sheets[0].Rows[Fpspread6.Sheets[0].Rows.Count - 1].Visible = true;
                                     }
                                     if (chk == false)
                                     {
                                         Fpspread6.Sheets[0].Rows[Fpspread6.Sheets[0].Rows.Count - 1].Visible = false;
                                         noofabsents++;

                                     }
                                     colno++;
                                 }

                                 else
                                 {
                                     if (presents != 1)
                                     {
                                         Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "A";
                                         Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Red;
                                         Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Locked = true;
                                         Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].VerticalAlign = VerticalAlign.Middle;
                                         Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Name = "Book Antiqua";
                                         Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Size = FontUnit.Medium;


                                         Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Bold = true;
                                         Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].HorizontalAlign = HorizontalAlign.Center;
                                         noofabsents++;
                                        

                                     }
                                     else
                                     {
                                         noofpresents++;
                                         Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = "--";
                                         Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Green;
                                         Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Locked = true;
                                         Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].VerticalAlign = VerticalAlign.Middle;
                                         Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Name = "Book Antiqua";
                                         Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Size = FontUnit.Medium;
                                     }
                                     colno++;
                                 }
                                
                                string[] spli1 = fdyearmonthday1.Split('/');

                                fddayy = Convert.ToInt32(spli1[1]);
                                fddayy++;
                                fdyearr = Convert.ToInt32(spli1[2]);
                                fdmon = Convert.ToInt32(spli1[0]);
                                if(colc<dd+3)
                                {
                                    if (Convert.ToInt32(Fpspread6.Sheets[0].ColumnHeader.Cells[1, colc].Text) == 1)
                                    {
                                        fdmon++;
                                        fddayy = 1;
                                    }

                            }
                                fdyearmonthday1 = Convert.ToString(fdmon) + "/" + Convert.ToString(fddayy) + "/" + Convert.ToString(fdyearr);
                                colc++;
                                
                            }

                            Fpspread6.Rows[Fpspread6.Sheets[0].Rows.Count - 1].Height = 40;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = Convert.ToString(dd);
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Locked = true;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].VerticalAlign = VerticalAlign.Middle;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Name = "Book Antiqua";
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Size = FontUnit.Medium;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Black;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Bold = true;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].HorizontalAlign = HorizontalAlign.Center;
                            colno++;

                            Fpspread6.Rows[Fpspread6.Sheets[0].Rows.Count - 1].Height = 40;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = Convert.ToString(noofpresents);
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Locked = true;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].VerticalAlign = VerticalAlign.Middle;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Name = "Book Antiqua";
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Size = FontUnit.Medium;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Black;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Bold = true;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].HorizontalAlign = HorizontalAlign.Center;
                            colno++;


                            Fpspread6.Rows[Fpspread6.Sheets[0].Rows.Count - 1].Height = 40;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Text = Convert.ToString(noofabsents);
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Locked = true;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].VerticalAlign = VerticalAlign.Middle;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Name = "Book Antiqua";
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Size = FontUnit.Medium;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].ForeColor = System.Drawing.Color.Black;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].Font.Bold = true;
                            Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, colno].HorizontalAlign = HorizontalAlign.Center;
                            colno++;


            

                            
                            


                        }

                        Fpspread6.SaveChanges();

                        Fpspread6.Sheets[0].PageSize = Fpspread6.Sheets[0].RowCount;
                        Fpspread6.Visible = true;
                        rptprint1.Visible = true;
                    }

                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                        Fpspread6.Visible = false;
                        rptprint1.Visible = false;
                    }

                }
                else
                {
                    alertpopwindow.Visible = true;
                    Fpspread6.Visible = false;
                    rptprint1.Visible = false;
                    lblalerterr.Text = "Please set all feild";
                }
            }


        }

        catch
        {
                
        }
    }
    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        lbl_norec1.Visible = false;
        try
        {
            string reportname = txtexcelname1.Text;
            if (reportname.ToString().Trim() != "")
            {

                if (Fpspread6.Visible == true)
                {
                    d2.printexcelreport(Fpspread6, reportname);
                }
                lbl_norec1.Visible = false;
            }
            else
            {
                lbl_norec1.Text = "Please Enter Your Report Name";
                lbl_norec1.Visible = true;
                txtexcelname1.Focus();
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

            string Hostel = "Hostel_Attendance_Manual_Report ";
            string pagename = "Hostel_Attendance_Manual_Report.aspx";


            if (Fpspread6.Visible == true)
            {
                Printcontrol1.loadspreaddetails(Fpspread6, pagename, Hostel);
            }
            Printcontrol1.Visible = true;
            lbl_norec1.Visible = false;
        }

        catch
        {
        }
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    protected void ddl_Hostel_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbhostel.Checked == true)
        {
            bindbuilding();
            bindfloor();
        }
        //hosname = Convert.ToString(ddl_Hostel.SelectedValue);
        //txt_floorname.Text = "Floor Name (" + cbl_floorname.Items.Count + ")";
        //cb_floorname.Checked = true;
        //cb_floorname_CheckedChange(sender, e);
        //if (ddl_Hostel.Text == "select")
        //{
        //    txt_floorname.Text = "--Select--";
        //}
        if (rdbmess.Checked == true)
        {
        }
            //messco = Convert.ToString(ddl_Hostel.SelectedValue);
        //fpspreadvisiblefalse();
        //rptprint.Visible = false;

    }
   
    public void load_ddlrollno()
    {
        try
        {
            System.Web.UI.WebControls.ListItem lst1 = new System.Web.UI.WebControls.ListItem("Roll No", "0");
            System.Web.UI.WebControls.ListItem lst2 = new System.Web.UI.WebControls.ListItem("Reg No", "1");
            //System.Web.UI.WebControls.ListItem lst3 = new System.Web.UI.WebControls.ListItem("Admin No", "2");
            //System.Web.UI.WebControls.ListItem lst4 = new System.Web.UI.WebControls.ListItem("App No", "3");
            //System.Web.UI.WebControls.ListItem lst5 = new System.Web.UI.WebControls.ListItem("Name", "2");
            System.Web.UI.WebControls.ListItem lst51 = new System.Web.UI.WebControls.ListItem("Hostel Id", "2");

            //Roll Number or Reg Number or Admission No or Application Number
            ddl_status.Items.Clear();
            string insqry1 = "select value from Master_Settings where settings='Roll No' and usercode ='" + usercode + "' --and college_code ='" + collegecode1 + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Roll No
                ddl_status.Items.Add(lst1);
            }


            insqry1 = "select value from Master_Settings where settings='Register No' and usercode ='" + usercode + "' --and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                ddl_status.Items.Add(lst2);
            }

            insqry1 = "select value from Master_Settings where settings='Hostel Id' and usercode ='" + usercode + "' --and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                ddl_status.Items.Add(lst51);
            }

            //insqry1 = "select value from Master_Settings where settings='Admission No' and usercode ='" + usercode + "' --and college_code ='" + collegecode1 + "'";
            //save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            //if (save1 == 1)
            //{
            //    //Admission No - Roll Admit
            //    ddlrollno.Items.Add(lst3);
            //}

            //insqry1 = "select value from Master_Settings where settings='Application No' and usercode ='" + usercode + "' --and college_code ='" + collegecode1 + "'";
            //save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            //if (save1 == 1)
            //{
            //    //App Form Number - Application Number
            //    ddlrollno.Items.Add(lst4);

            //}
            if (ddl_status.Items.Count == 0)
            {
                ddl_status.Items.Add(lst1);
            }
            //ddl_status.Items.Add(lst5);
         
            switch (Convert.ToUInt32(ddl_status.SelectedItem.Value))
            {
                case 0:
                   
                    break;
                case 1:
                   
                   
                    break;
                //case 2:
                //    txtno.Attributes.Add("placeholder", "Admin No");
                //    chosedmode = 2;
                //    break;
                //case 3:
                //    txtno.Attributes.Add("placeholder", "App No");
                //    chosedmode = 3;
                //    break;
            }
           
        }
        catch { }
    }

    protected void ddl_status_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Error.Visible = false;
        //FpSpread1.Visible = false;
        //btnprintmaster.Visible = false;

        Lblstatus.Text = ddl_status.SelectedItem.ToString();

    }


    public void bindfloor()
    {
        try
        {
            string hostel = "";

            if (ddl_Hostel.Items.Count > 0)
                hostel = "" + ddl_Hostel.SelectedValue + "";

            string build = d2.GetBuildingCode_inv(hostel);
            char[] delimiterChars = { ',' };
            string[] build1 = build.Split(delimiterChars);
            string build2 = "";

            foreach (string b in build1)
            {
                if (build2 == "")
                {
                    build2 = "" + b + "";
                }
                else
                {
                    build2 = build2 + "'" + "," + "'" + b + "";
                }
            }

            DataSet ds1 = new DataSet(); 
            ds1.Clear();
            string floor = "select code,Building_Name from Building_Master where code in ('" + build2 + "')";
            ds1 = d2.select_method_wo_parameter(floor, "Text");
            string w = "";
            if (ds1.Tables[0].Rows.Count > 0)
            {
                string q1 = Convert.ToString(ds1.Tables[0].Rows[0][1]);
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    string q = Convert.ToString(ds1.Tables[0].Rows[i][1]);
                    if (w == "")
                    {
                        w = "" + q + "";
                    }
                    else
                    {
                        w = w + "'" + "," + "'" + q + "";
                    }
                }
            }
            ds.Clear();
            ds = d2.BindFloor_new(w);
            string MessmasterFK = string.Empty;
            if (usercode != "" && usercode != "0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Floor Rights' and user_code='" + usercode + "'");
            if (group_user != "" && group_user != "0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Floor Rights' and user_code='" + group_user + "'");
            string itemname = "select distinct Floor_Name,FloorPK from Floor_Master where FloorPK in(" + MessmasterFK + ") and Building_Name in(select Building_Name from Building_Master where code in ('" +drbbuilding.SelectedValue + "'))";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");


            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_floorname.DataSource = ds;
                cbl_floorname.DataTextField = "Floor_Name";
                cbl_floorname.DataValueField = "FloorPK";
                cbl_floorname.DataBind();





                if (cbl_floorname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_floorname.Items.Count; i++)
                    {

                        cbl_floorname.Items[i].Selected = true;
                    }

                    txt_floorname.Text = "Floor Name(" + cbl_floorname.Items.Count + ")";
                }
            }
            else
            {

                txt_floorname.Text = "--Select--";
            }
            bindroom();
        }
        catch (Exception ex)
        {
        }
    }
    public void bindroom()
    {
        try
        {
            cbl_room.Items.Clear();
            txt_room.Text = "---Select---";
            cb_room.Checked = false;
            string query = "";
            string floors = "";

            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    if (floors == "")
                    {
                        floors = "" + cbl_floorname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        floors = floors + "'" + "," + "'" + cbl_floorname.Items[i].Value.ToString() + "";
                    }
                }
            }
            floorname = floors;
            query = "select distinct rd.Roompk,rd.Room_Name from Room_Detail rd,Floor_Master hd where rd.Floor_Name=hd.Floor_Name and hd.FloorPK in('" + floors + "') and hd.Building_Name=rd.Building_Name order by Roompk";


            string MessmasterFK = string.Empty;
            if (usercode != "" && usercode != "0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Room Rights' and user_code='" + usercode + "'");
            if (group_user != "" && group_user != "0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Room Rights' and user_code='" + group_user + "'");
            string itemname = "select distinct rd.Roompk,rd.Room_Name from Room_Detail rd,Floor_Master hd where rd.Floor_Name=hd.Floor_Name and hd.FloorPK in('" + floors + "') and hd.Building_Name=rd.Building_Name and Roompk in(" + MessmasterFK + ") order by Roompk";
           // string itemname = "select distinct Room_Name,Roompk from Room_Detail where Roompk in(" + MessmasterFK + ")";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
            // ds.Clear();
            // ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_room.DataSource = ds;
                cbl_room.DataTextField = "Room_Name";
                cbl_room.DataValueField = "Roompk";
                cbl_room.DataBind();

                if (cbl_room.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_room.Items.Count; row++)
                    {
                        cbl_room.Items[row].Selected = true;
                    }
                    txt_room.Text = "Room (" + cbl_room.Items.Count + ")";
                    cb_room.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_room_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_room.Text = "--Select--";
            if (cb_room.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_room.Items.Count; i++)
                {
                    cbl_room.Items[i].Selected = true;
                }
                txt_room.Text = "Room (" + (cbl_room.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_room.Items.Count; i++)
                {
                    cbl_room.Items[i].Selected = false;
                }
                txt_room.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void cbl_room_SelectedIndexChanged(object sender, EventArgs e)
    {
        cb_room.Checked = false;
        int commcount = 0;

        txt_room.Text = "--Select--";

        for (int i = 0; i < cbl_room.Items.Count; i++)
        {
            if (cbl_room.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_room.Items.Count)
            {
                cb_room.Checked = true;
            }
            txt_room.Text = "Room (" + commcount.ToString() + ")";
        }
    }


    protected void cbl_floorname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            cb_floorname.Checked = false;
            txt_floorname.Text = "--Select--";

            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_floorname.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_floorname.Items.Count)
                {
                    cb_floorname.Checked = true;
                }
                txt_floorname.Text = "Floor Name(" + commcount.ToString() + ")";
                bindroom();
            }
        }
        catch { }
    }
    protected void cb_floorname_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            int c = 0;
            if (cb_floorname.Checked == true)
            {
                c++;
                for (int i = 0; i < cbl_floorname.Items.Count; i++)
                {
                    cbl_floorname.Items[i].Selected = true;
                }
                txt_floorname.Text = "Floor Name(" + (cbl_floorname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_floorname.Items.Count; i++)
                {
                    cbl_floorname.Items[i].Selected = false;
                }
                txt_floorname.Text = "--Select--";
            }

        }
        catch { }
    }

    public void bindbuilding()
    {
        try
        {
            string hostel = string.Empty;
            if (ddl_Hostel.Items.Count > 0)
                hostel = "" + ddl_Hostel.SelectedValue + "";
            string MessmasterFK = string.Empty;
            if (usercode != "" && usercode != "0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Building Rights' and user_code='" + usercode + "'");
            if (group_user != "" && group_user != "0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Building Rights' and user_code='" + group_user + "'");
            string itemname = d2.GetFunction("select HostelBuildingFK From  HM_HostelMaster where HostelMasterPK IN ('" + hostel + "') ");
            string itemnames = "select * from  Building_Master where code in(" + MessmasterFK + ") and code in(" + itemname + ")";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemnames, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                drbbuilding.DataSource = ds;
                drbbuilding.DataTextField = "Building_name";
                drbbuilding.DataValueField = "code";
                drbbuilding.DataBind();
            }

        }
        catch
        {
        }
    }
    protected void drbbuilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindfloor();
        }
        catch
        {
        }
    }
}

