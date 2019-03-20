using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Web.Services;
using System.Drawing;

public partial class HM_SessionMaster : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable newhash = new Hashtable();
    Hashtable newhashhour = new Hashtable();
    Hashtable newhashmin = new Hashtable();
    string startingtime = "";
    string endingtime = "";
    string extensiontime = "";
    string isextension = "";


    string exhour, exminits, exseconds, exformat = "";
    //  public static string sessionname = "";
    int session_Code = 0;
    bool check = false;
    DateTime starttime = new DateTime();//"h:mm:ss:tt"
    DateTime endtime = new DateTime();
    DateTime exten = new DateTime();

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
            // rdopopveg.Checked = true;
            cb_Extentionallow1.Checked = false;
            loadhostel();
            loadsession();
            loadhour();
            loadsecond();
            loadminits();
            bindendhrddl();
            bindendminsddl();
            bindendformat();
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread1.Visible = false;
            btn_go_Click(sender, e);

        }
        lblvalidation1.Visible = false;

    }
    //main page   
    public void loadhostel()
    {

        try
        {

            cbl_hostel.Items.Clear();
            //string deptquery = "select  MessMasterPK,MessName  from HM_MessMaster where CollegeCode ='" + collegecode1 + "' order by MessMasterPK ";
            //ds.Clear();
            //ds = d2.select_method_wo_parameter(deptquery, "Text");

            ds.Clear();
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostel.DataSource = ds;
                cbl_hostel.DataTextField = "MessName";
                cbl_hostel.DataValueField = "MessMasterPK";
                cbl_hostel.DataBind();

                if (cbl_hostel.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_hostel.Items.Count; i++)
                    {
                        cbl_hostel.Items[i].Selected = true;
                    }
                    txt_hostelname.Text = "Mess Name(" + cbl_hostel.Items.Count + ")";
                    cb_hostelname.Checked = true;
                }
            }
            else
            {
                txt_hostelname.Text = "--Select--";

            }
        }
        catch
        {
        }
    }
    protected void cb_hostel_CheckedChanged(object sender, EventArgs e)
    {

        if (cb_hostelname.Checked == true)
        {
            for (int i = 0; i < cbl_hostel.Items.Count; i++)
            {
                cbl_hostel.Items[i].Selected = true;
            }
            txt_hostelname.Text = "Mess Name(" + (cbl_hostel.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_hostel.Items.Count; i++)
            {
                cbl_hostel.Items[i].Selected = false;
            }
            txt_hostelname.Text = "--Select--";
        }
        loadsession();
    }
    protected void cbl_hostel_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_hostelname.Text = "--Select--";
        cb_hostelname.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_hostel.Items.Count; i++)
        {
            if (cbl_hostel.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txt_hostelname.Text = "Mess Name(" + commcount.ToString() + ")";
            if (commcount == cbl_hostel.Items.Count)
            {
                cb_hostelname.Checked = true;
            }
        }
        loadsession();
    }
    public void loadsession()
    {
        try
        {
            ds.Clear();
            cbl_session.Items.Clear();

            string itemheader = "";
            for (int i = 0; i < cbl_hostel.Items.Count; i++)
            {
                if (cbl_hostel.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_hostel.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_hostel.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemheader.Trim() != "")
            {
                ds.Clear();

                string deptquery = "select  SessionMasterPK,SessionName  from HM_SessionMaster where MessMasterFK in ('" + itemheader + "') order by SessionMasterPK ";

                ds = d2.select_method_wo_parameter(deptquery, "Text");
                //ds = d2.BindSession(itemheader);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_session.DataSource = ds;
                    cbl_session.DataTextField = "SessionName";
                    cbl_session.DataValueField = "SessionMasterPK";
                    cbl_session.DataBind();
                    if (cbl_session.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_session.Items.Count; i++)
                        {
                            cbl_session.Items[i].Selected = true;
                        }
                        txt_sessionname1.Text = "Session Name(" + cbl_session.Items.Count + ")";
                    }
                }
                else
                {
                    txt_sessionname1.Text = "--Select--";
                }
            }
            else
            {
                txt_sessionname1.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void cb_session_CheckedChange(object sender, EventArgs e)
    {
        if (cb_sessionname.Checked == true)
        {
            for (int i = 0; i < cbl_session.Items.Count; i++)
            {
                cbl_session.Items[i].Selected = true;
            }
            txt_sessionname1.Text = "Session Name(" + (cbl_session.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_session.Items.Count; i++)
            {
                cbl_session.Items[i].Selected = false;
            }
            txt_sessionname1.Text = "--Select--";
        }

    }
    protected void cbl_session_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_sessionname1.Text = "--Select--";
        cb_sessionname.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_session.Items.Count; i++)
        {
            if (cbl_session.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txt_sessionname1.Text = "Session Name(" + commcount.ToString() + ")";
            if (commcount == cbl_session.Items.Count)
            {
                cb_sessionname.Checked = true;
            }
        }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            string itemheadercode = "";
            for (int i = 0; i < cbl_hostel.Items.Count; i++)
            {
                if (cbl_hostel.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_hostel.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cbl_hostel.Items[i].Value.ToString() + "";
                    }
                }
            }
            string itemcode = "";
            for (int i = 0; i < cbl_session.Items.Count; i++)
            {
                if (cbl_session.Items[i].Selected == true)
                {
                    if (itemcode == "")
                    {
                        itemcode = "" + cbl_session.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemcode = itemcode + "'" + "," + "'" + cbl_session.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (txt_hostelname.Text.Trim() != "--Select--" && txt_sessionname1.Text.Trim() != "--Select--")
            {

                if (itemcode.Trim() != "" && itemheadercode.Trim() != "")
                {
                    //string selectquery = "select * from Session_Master s,MessMaster h where s.Hostel_Code =h.Messid and s.Hostel_Code in ('" + itemheadercode + "') and Session_Code in ('" + itemcode + "')";

                    string selectquery = "select s.SessionMasterPK,s.SessionName,s.SessionStartTime,s.SessionCloseTime,s.IsAllowExtTime,s.SessionCloseExtTime,s.MessMasterFK, h.MessName,h.MessMasterPK  from HM_SessionMaster s, HM_MessMaster h where s.MessMasterFK = h.MessMasterPK  and h.MessMasterPK in ('" + itemheadercode + "') and s.SessionMasterPK in ('" + itemcode + "')";


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
                        Fpspread1.Sheets[0].ColumnCount = 7;
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
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Session Name";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Start Time";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "End Time";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Extension Allowed";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Extension Time";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Mess Name";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            Fpspread1.Sheets[0].RowCount++;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["SessionName"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["SessionStartTime"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["SessionCloseTime"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                            string exten = "";
                            if (Convert.ToString(ds.Tables[0].Rows[row]["IsAllowExtTime"]) == "True")
                            {
                                exten = "Yes";
                            }
                            else
                            {
                                exten = "No";
                            }

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(exten);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["SessionCloseExtTime"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["MessName"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(ds.Tables[0].Rows[row]["MessMasterPK"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";


                        }
                        Fpspread1.Visible = true;
                        rptprint.Visible = true;
                        div1.Visible = true;
                        lbl_error.Visible = false;
                        Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    }
                    else
                    {
                        div1.Visible = false;
                        Fpspread1.Visible = false;
                        rptprint.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "No Records Found";
                    }
                }
                else
                {
                    div1.Visible = false;
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "No Records Found";
                }
            }
            else
            {
                div1.Visible = false;
                Fpspread1.Visible = false;
                rptprint.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select All Fields";
            }
        }
        catch
        {

        }

    }
    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        //16.10.15
        //time.Text = DateTime.Now.ToString("h:mm:ss tt");
        txt_hostelname1.Enabled = true;
        time.Visible = false;
        loadhostel1();
        loadhour();
        loadsecond();
        loadminits();
        clear();
        btn_save.Visible = true;
        btn_update.Visible = false;
        btn_delete.Visible = false;
        poperrjs.Visible = true;
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
        try
        {
            if (check == true)
            {
                loadhostel1();
                bindendhrddl();
                bindendminsddl();
                bindendformat();
                btn_save.Visible = false;
                btn_update.Visible = true;
                btn_delete.Visible = true;
                poperrjs.Visible = true;
                string activerow = "";
                string activecol = "";
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                collegecode = Session["collegecode"].ToString();
                if (activerow.Trim() != "")
                {
                    string SessionPK = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                    Session["SessionMasterPK"] = Convert.ToString(SessionPK);
                    string sessionname = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    string hostelname = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text);
                    string hostelcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Tag);
                    string starttime = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                    string endtime = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);

                    //theivamani 13.10.15
                    if (starttime != "")
                    {
                        string[] endtimesplit = starttime.Split(':');
                        if (endtimesplit.Length > 2)
                        {
                            int indhour = Convert.ToInt32(newhashhour[endtimesplit[0]]);
                            int indmin = Convert.ToInt32(newhashmin[endtimesplit[1]]);
                            int indampm = Convert.ToInt32(newhash[endtimesplit[2].Split(' ')[1]]);

                            ddl_hour.SelectedIndex = indhour;
                            ddl_minits.SelectedIndex = indmin;
                            ddl_timeformate.SelectedIndex = indampm;
                        }
                    }

                    if (endtime != "")
                    {
                        string[] endtimesplit = endtime.Split(':');
                        if (endtimesplit.Length > 2)
                        {
                            int indhour = Convert.ToInt32(newhashhour[endtimesplit[0]]);
                            int indmin = Convert.ToInt32(newhashmin[endtimesplit[1]]);
                            int indampm = Convert.ToInt32(newhash[endtimesplit[2].Split(' ')[1]]);

                            ddl_endhour.SelectedIndex = indhour;
                            ddl_endminit.SelectedIndex = indmin;
                            ddl_endformate.SelectedIndex = indampm;

                        }
                    }

                    string extention = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                    string extentiontime = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
                    if (extentiontime != "")
                    {
                        string[] endtimesplit = extentiontime.Split(':');
                        if (endtimesplit.Length > 2)
                        {
                            int indhour = Convert.ToInt32(newhashhour[endtimesplit[0]]);
                            int indmin = Convert.ToInt32(newhashmin[endtimesplit[1]]);
                            int indampm = Convert.ToInt32(newhash[endtimesplit[2].Split(' ')[1]]);

                            ddl_exhour.SelectedIndex = indhour;
                            ddl_exminitus.SelectedIndex = indmin;
                            ddl_exformate.SelectedIndex = indampm;

                        }
                    }
                    if (extention.Trim() == "Yes")
                    {
                        subdiv.Visible = true;
                        cb_Extentionallow1.Checked = true;
                    }
                    else
                    {
                        subdiv.Visible = false;
                        cb_Extentionallow1.Checked = false;
                    }
                    int ch = 0;
                    if (cbl_hostelname1.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_hostelname1.Items.Count; row++)
                        {
                            if (cbl_hostelname1.Items[row].Value == hostelcode)
                            {
                                ch++;
                                cbl_hostelname1.Items[row].Selected = true;
                            }
                            else
                            {
                                cbl_hostelname1.Items[row].Selected = false;
                            }
                        }
                        if (ch != 0)
                        {
                            txt_hostelname1.Text = "Mess Name (" + ch + ")";
                        }
                        else
                        {
                            txt_hostelname1.Text = "--Select--";
                        }
                    }


                    txt_hostelname1.Enabled = false;
                    txt_sessionname.Text = Convert.ToString(sessionname);
                    string[] split = starttime.Split(' ');
                    if (split.Length > 0)
                    {
                        string[] secondsplit = split[0].Split(':');
                        if (secondsplit.Length > 0)
                        {
                            ddl_hour.SelectedItem.Text = Convert.ToString(secondsplit[0]);
                            ddl_minits.SelectedItem.Text = Convert.ToString(secondsplit[1]);
                            ddl_seconds.SelectedItem.Text = Convert.ToString(secondsplit[2]);
                        }
                        ddl_timeformate.SelectedItem.Text = Convert.ToString(split[1]);
                    }
                    split = endtime.Split(' ');
                    if (split.Length > 0)
                    {
                        string[] secondsplit = split[0].Split(':');
                        if (secondsplit.Length > 0)
                        {
                            ddl_endhour.SelectedItem.Text = Convert.ToString(secondsplit[0]);
                            ddl_endminit.SelectedItem.Text = Convert.ToString(secondsplit[1]);
                            ddl_endsecnonds.SelectedItem.Text = Convert.ToString(secondsplit[2]);
                        }
                        ddl_endformate.SelectedItem.Text = Convert.ToString(split[1]);
                    }

                    if (extentiontime.Trim() != "")
                    {
                        split = extentiontime.Split(' ');
                        if (split.Length > 0)
                        {
                            string[] secondsplit = split[0].Split(':');
                            if (secondsplit.Length > 0)
                            {
                                ddl_exhour.SelectedItem.Text = Convert.ToString(secondsplit[0]);
                                ddl_exminitus.SelectedItem.Text = Convert.ToString(secondsplit[1]);
                                ddl_exseconds.SelectedItem.Text = Convert.ToString(secondsplit[2]);
                            }
                            ddl_exformate.SelectedItem.Text = Convert.ToString(split[1]);
                        }
                    }
                }
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
            string degreedetails = "Session Master Report";
            string pagename = "HM_SessionMaster.aspx";
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
    //poperrjs
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    public void loadhostel1()
    {


        try
        {
            cbl_hostelname1.Items.Clear();

            ds.Clear();
            // ds = d2.BindMess(collegecode1);
            //string deptquery = "select  MessMasterPK,MessName  from HM_MessMaster where CollegeCode ='" + collegecode1 + "' order by MessMasterPK ";
            //ds.Clear();
            //ds = d2.select_method_wo_parameter(deptquery, "Text");

            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname1.DataSource = ds;
                cbl_hostelname1.DataTextField = "MessName";
                cbl_hostelname1.DataValueField = "MessMasterPK";
                cbl_hostelname1.DataBind();
                if (cbl_hostelname1.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
                    {
                        cbl_hostelname1.Items[i].Selected = true;
                    }
                    txt_hostelname1.Text = "Mess Name(" + cbl_hostelname1.Items.Count + ")";
                    // cb_hostelname1.Checked = true;
                }
            }
            else
            {

                txt_hostelname1.Text = "--Select--";
            }
        }
        catch { }
    }
    protected void cb_hostelname1_CheckedChange(object sender, EventArgs e)
    {
        if (cb_hostelname1.Checked == true)
        {
            for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
            {
                cbl_hostelname1.Items[i].Selected = true;
            }
            txt_hostelname1.Text = "Mess Name(" + (cbl_hostelname1.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
            {
                cbl_hostelname1.Items[i].Selected = false;
            }
            txt_hostelname1.Text = "--Select--";
        }
    }
    protected void cb_hostelname1_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_hostelname1.Text = "--Select--";
        cb_hostelname1.Checked = false;
        int commcount = 0;

        for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
        {
            if (cbl_hostelname1.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txt_hostelname1.Text = "Mess Name(" + commcount.ToString() + ")";
            if (commcount == cbl_hostelname1.Items.Count)
            {
                cb_hostelname1.Checked = true;
            }
        }
    }
    public void loadhour()
    {
        try
        {
            ddl_hour.Items.Clear();
            ddl_endhour.Items.Clear();
            ddl_exhour.Items.Clear();
            for (int i = 1; i <= 12; i++)
            {
                ddl_hour.Items.Add(Convert.ToString(i));
                ddl_endhour.Items.Add(Convert.ToString(i));
                ddl_exhour.Items.Add(Convert.ToString(i));
                ddl_hour.SelectedIndex = ddl_hour.Items.Count - 1;
                ddl_endhour.SelectedIndex = ddl_endhour.Items.Count - 1;
                ddl_exhour.SelectedIndex = ddl_exhour.Items.Count - 1;
            }
        }
        catch
        {
        }
    }
    public void bindendhrddl()
    {
        ddl_endhour.Items.Clear();
        newhashhour.Clear();

        ddl_endhour.Items.Insert(ddl_endhour.Items.Count, "1");
        newhashhour.Add("1", ddl_endhour.Items.Count - 1);
        ddl_endhour.Items.Insert(ddl_endhour.Items.Count, "2");
        newhashhour.Add("2", ddl_endhour.Items.Count - 1);
        ddl_endhour.Items.Insert(ddl_endhour.Items.Count, "3");
        newhashhour.Add("3", ddl_endhour.Items.Count - 1);
        ddl_endhour.Items.Insert(ddl_endhour.Items.Count, "4");
        newhashhour.Add("4", ddl_endhour.Items.Count - 1);
        ddl_endhour.Items.Insert(ddl_endhour.Items.Count, "5");
        newhashhour.Add("5", ddl_endhour.Items.Count - 1);
        ddl_endhour.Items.Insert(ddl_endhour.Items.Count, "6");
        newhashhour.Add("6", ddl_endhour.Items.Count - 1);
        ddl_endhour.Items.Insert(ddl_endhour.Items.Count, "7");
        newhashhour.Add("7", ddl_endhour.Items.Count - 1);
        ddl_endhour.Items.Insert(ddl_endhour.Items.Count, "8");
        newhashhour.Add("8", ddl_endhour.Items.Count - 1);
        ddl_endhour.Items.Insert(ddl_endhour.Items.Count, "9");
        newhashhour.Add("9", ddl_endhour.Items.Count - 1);
        ddl_endhour.Items.Insert(ddl_endhour.Items.Count, "10");
        newhashhour.Add("10", ddl_endhour.Items.Count - 1);
        ddl_endhour.Items.Insert(ddl_endhour.Items.Count, "11");
        newhashhour.Add("11", ddl_endhour.Items.Count - 1);
        ddl_endhour.Items.Insert(ddl_endhour.Items.Count, "12");
        newhashhour.Add("12", ddl_endhour.Items.Count - 1);
    }
    public void bindendminsddl()
    {
        ddl_endminit.Items.Clear();
        newhashmin.Clear();
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "00");
        newhashmin.Add("00", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "01");
        newhashmin.Add("01", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "02");
        newhashmin.Add("02", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "03");
        newhashmin.Add("03", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "04");
        newhashmin.Add("04", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "05");
        newhashmin.Add("05", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "06");
        newhashmin.Add("06", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "07");
        newhashmin.Add("07", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "08");
        newhashmin.Add("08", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "09");
        newhashmin.Add("09", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "10");
        newhashmin.Add("10", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "11");
        newhashmin.Add("11", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "12");
        newhashmin.Add("12", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "13");
        newhashmin.Add("13", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "14");
        newhashmin.Add("14", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "15");
        newhashmin.Add("15", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "16");
        newhashmin.Add("16", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "17");
        newhashmin.Add("17", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "18");
        newhashmin.Add("18", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "19");
        newhashmin.Add("19", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "20");
        newhashmin.Add("20", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "21");
        newhashmin.Add("21", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "22");
        newhashmin.Add("22", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "23");
        newhashmin.Add("23", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "24");
        newhashmin.Add("24", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "25");
        newhashmin.Add("25", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "26");
        newhashmin.Add("26", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "27");
        newhashmin.Add("27", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "28");
        newhashmin.Add("28", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "29");
        newhashmin.Add("29", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "30");
        newhashmin.Add("30", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "31");
        newhashmin.Add("31", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "32");
        newhashmin.Add("32", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "33");
        newhashmin.Add("33", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "34");
        newhashmin.Add("34", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "35");
        newhashmin.Add("35", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "36");
        newhashmin.Add("36", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "37");
        newhashmin.Add("37", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "38");
        newhashmin.Add("38", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "39");
        newhashmin.Add("39", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "40");
        newhashmin.Add("40", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "41");
        newhashmin.Add("41", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "42");
        newhashmin.Add("42", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "43");
        newhashmin.Add("43", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "44");
        newhashmin.Add("44", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "45");
        newhashmin.Add("45", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "46");
        newhashmin.Add("46", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "47");
        newhashmin.Add("47", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "48");
        newhashmin.Add("48", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "49");
        newhashmin.Add("49", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "50");
        newhashmin.Add("50", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "51");
        newhashmin.Add("51", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "52");
        newhashmin.Add("52", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "53");
        newhashmin.Add("53", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "54");
        newhashmin.Add("54", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "55");
        newhashmin.Add("55", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "56");
        newhashmin.Add("56", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "57");
        newhashmin.Add("57", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "58");
        newhashmin.Add("58", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "59");
        newhashmin.Add("59", ddl_endminit.Items.Count - 1);
        ddl_endminit.Items.Insert(ddl_endminit.Items.Count, "60");
        newhashmin.Add("60", ddl_endminit.Items.Count - 1);
    }
    public void bindendformat()
    {
        ddl_endformate.Items.Clear();
        newhash.Clear();
        ddl_endformate.Items.Insert(ddl_endformate.Items.Count, "AM");
        newhash.Add("AM", ddl_endformate.Items.Count - 1);
        ddl_endformate.Items.Insert(ddl_endformate.Items.Count, "PM");
        newhash.Add("PM", ddl_endformate.Items.Count - 1);

    }
    public void loadsecond()
    {
        ddl_seconds.Items.Clear();
        ddl_endsecnonds.Items.Clear();
        ddl_exseconds.Items.Clear();
        for (int i = 0; i <= 60; i++)
        {
            string value = Convert.ToString(i);
            if (value.Length == 1)
            {
                value = "0" + "" + value;
            }
            ddl_seconds.Items.Add(Convert.ToString(value));
            ddl_endsecnonds.Items.Add(Convert.ToString(value));
            ddl_exseconds.Items.Add(Convert.ToString(value));
        }
    }
    public void loadminits()
    {
        ddl_minits.Items.Clear();
        ddl_endminit.Items.Clear();
        ddl_exminitus.Items.Clear();
        for (int i = 0; i <= 60; i++)
        {
            string value = Convert.ToString(i);
            if (value.Length == 1)
            {
                value = "0" + "" + value;
            }
            ddl_minits.Items.Add(Convert.ToString(value));
            ddl_endminit.Items.Add(Convert.ToString(value));
            ddl_exminitus.Items.Add(Convert.ToString(value));
        }
    }


    protected void cb_Extentionallow1_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_Extentionallow1.Checked == true)
        {
            subdiv.Visible = true;
        }
        else
        {
            subdiv.Visible = false;
        }
    }
    protected void ddl_hour_SelectedIndexChanged(object sender, EventArgs e)
    {
        time.Visible = false;
        timeCheck();
    }
    public void timeCheck()
    {
        time.Visible = false;
        bool timevalue = false;
        int hour1 = Convert.ToInt32(ddl_hour.SelectedItem.Text);
        int hour2 = Convert.ToInt32(ddl_endhour.SelectedItem.Text);
        int min1 = Convert.ToInt32(ddl_minits.SelectedItem.Text);
        int min2 = Convert.ToInt32(ddl_endminit.SelectedItem.Text);
        string format1 = Convert.ToString(ddl_timeformate.SelectedItem.Text);
        string format2 = Convert.ToString(ddl_endformate.SelectedItem.Text);

        int format1value = 0;
        int format2value = 0;
        switch (format1)
        {
            case "AM":
                format1value = 0;
                break;
            case "PM":
                format1value = 1;
                break;
        }

        switch (format2)
        {
            case "AM":
                format2value = 0;
                break;
            case "PM":
                format2value = 1;
                break;
        }

        if (hour1 >= hour2 && format1value == format2value)
        {
            if (min1 <= min2)
            {
                timevalue = false;
            }
            else
            {
                timevalue = true;
            }
        }
        else if (hour1 >= hour2 && format1value < format2value)
        {
            if (min1 <= min2)
            {
                timevalue = false;
            }
            else
            {
                timevalue = true;
            }
        }

        if (timevalue == true)
        {
            clear();
            imgdiv2.Visible = true;
            lbl_alert.Text = "Time InValid";
        }

    }
    public void externtime()
    {
        //timeCheck();
        bool timevalue = false;
        int hour1 = Convert.ToInt32(ddl_endhour.SelectedItem.Text);
        int hour2 = Convert.ToInt32(ddl_exhour.SelectedItem.Text);
        int min1 = Convert.ToInt32(ddl_endminit.SelectedItem.Text);
        int min2 = Convert.ToInt32(ddl_exminitus.SelectedItem.Text);
        string format1 = Convert.ToString(ddl_endformate.SelectedItem.Text);
        string format2 = Convert.ToString(ddl_exformate.SelectedItem.Text);

        int format1value = 0;
        int format2value = 0;
        switch (format1)
        {
            case "AM":
                format1value = 0;
                break;
            case "PM":
                format1value = 1;
                break;
        }

        switch (format2)
        {
            case "AM":
                format2value = 0;
                break;
            case "PM":
                format2value = 1;
                break;
        }

        if (hour1 > hour2 && format1value == format2value)
        {
            timevalue = false;
        }
        else if (hour1 > hour2 && format1value < format2value)
        {
            timevalue = true;
        }
        else if (hour1 == hour2)
        {
            if (min1 > min2)
            {
                timevalue = false;
            }
            else if (min1 == min2)
            {
                if (format1value == format2value)
                {
                    timevalue = false;
                }
                else if (format1value < format2value)
                {
                    timevalue = true;
                }
                else
                {
                    timevalue = false;
                }
            }
        }
        if (timevalue == true)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Time Valid";
            //lbl_hostel1.Text = "Time valid";                     
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Time Valid";
            //lbl_hostel1.Text = "Time invalid";
        }
    }
    protected void ddl_minits_SelectedIndexChanged(object sender, EventArgs e)
    {
        timeCheck();
    }
    protected void ddl_seconds_SelectedIndexChanged(object sender, EventArgs e)
    {
        timeCheck();
    }
    protected void ddl_timeformate_SelectedIndexChanged(object sender, EventArgs e)
    {
        timeCheck();
    }
    //endtime
    protected void ddl_endhour_SelectedIndexChanged(object sender, EventArgs e)
    {
        timeCheck();
    }
    protected void ddl_endminit_SelectedIndexChanged(object sender, EventArgs e)
    {
        timeCheck();
    }
    protected void ddl_endsecnonds_SelectedIndexChanged(object sender, EventArgs e)
    {
        timeCheck();
    }
    protected void ddl_endformate_SelectedIndexChanged(object sender, EventArgs e)
    {
        timeCheck();
    }
    //extension time
    protected void ddl_exhour_SelectedIndexChanged(object sender, EventArgs e)
    {
        timeCheck();
    }
    protected void ddl_exminitus_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void ddl_exseconds_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void ddl_exformate_SelectedIndexChanged(object sender, EventArgs e)
    {
        externtime();
    }
    public void clear()
    {

        ddl_hour.SelectedIndex = ddl_hour.Items.Count - 1;
        ddl_minits.SelectedIndex = 0;
        ddl_timeformate.SelectedIndex = 0;
        ddl_endhour.SelectedIndex = ddl_endhour.Items.Count - 1;
        ddl_endminit.SelectedIndex = 0;
        ddl_endformate.SelectedIndex = 0;
        ddl_exhour.SelectedIndex = ddl_exhour.Items.Count - 1;
        ddl_exminitus.SelectedIndex = 0;
        ddl_exformate.SelectedIndex = 0;
        txt_sessionname.Text = "";
        cb_Extentionallow1.Checked = false;
        subdiv.Visible = false;



    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {

            string sesseioncode = d2.GetFunction("select  top 1 SessionMasterPK from HM_SessionMaster order by  SessionMasterPK desc");
            if (sesseioncode.Trim() != "")
            {
                session_Code = Convert.ToInt32(sesseioncode);
                session_Code++;
            }
            else
            {
                session_Code = 1;
            }
            string sessionname = Convert.ToString(txt_sessionname.Text);
            //theivamani 28.10.15
            sessionname = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(sessionname);
            string hour = Convert.ToString(ddl_hour.SelectedItem.Text);
            string minits = Convert.ToString(ddl_minits.SelectedItem.Text);
            string seconds = Convert.ToString(ddl_seconds.SelectedItem.Text);
            string format = Convert.ToString(ddl_timeformate.SelectedItem.Text);
            startingtime = hour + ":" + minits + ":" + seconds + " " + format;

            string endhour = Convert.ToString(ddl_endhour.SelectedItem.Text);
            string endminits = Convert.ToString(ddl_endminit.SelectedItem.Text);
            string endseconds = Convert.ToString(ddl_endsecnonds.SelectedItem.Text);
            string endformat = Convert.ToString(ddl_endformate.SelectedItem.Text);
            endingtime = endhour + ":" + endminits + ":" + endseconds + " " + endformat;


            if (cb_Extentionallow1.Checked == true)
            {
                isextension = "1";
                string exhour = Convert.ToString(ddl_exhour.SelectedItem.Text);
                string exminits = Convert.ToString(ddl_exminitus.SelectedItem.Text);
                string exseconds = Convert.ToString(ddl_exseconds.SelectedItem.Text);
                string exformat = Convert.ToString(ddl_exformate.SelectedItem.Text);
                extensiontime = exhour + ":" + exminits + ":" + exseconds + " " + exformat;



            }
            else
            {
                isextension = "0";
            }


            //16.10.15

            starttime = Convert.ToDateTime(startingtime);
            endtime = Convert.ToDateTime(endingtime);


            if (starttime < endtime)
            {

                if (cb_Extentionallow1.Checked == true)
                {
                    exten = Convert.ToDateTime(extensiontime);
                    if (starttime < exten && endtime < exten)
                    {
                        session_save();
                    }
                    else
                    {

                        ddl_exhour.SelectedIndex = ddl_exhour.Items.Count - 1;
                        ddl_exminitus.SelectedIndex = 0;
                        ddl_exformate.SelectedIndex = 0;


                        time.Visible = true;

                        time.Text = "End time should be greater that Start time Please select the valid time";
                    }
                }
                else
                {
                    session_save();
                }

            }
            else
            {
                ddl_hour.SelectedIndex = ddl_hour.Items.Count - 1;
                ddl_minits.SelectedIndex = 0;
                ddl_timeformate.SelectedIndex = 0;
                ddl_endhour.SelectedIndex = ddl_endhour.Items.Count - 1;
                ddl_endminit.SelectedIndex = 0;
                ddl_endformate.SelectedIndex = 0;
                ddl_exhour.SelectedIndex = ddl_exhour.Items.Count - 1;
                ddl_exminitus.SelectedIndex = 0;
                ddl_exformate.SelectedIndex = 0;




                time.Visible = true;

                time.Text = "End time should be greater that Start time Please select the valid time";
            }

        }
        catch
        {
        }
    }

    protected void session_save()
    {
        try
        {
            string sessionname = Convert.ToString(txt_sessionname.Text);
            //theivamani 28.10.15
            sessionname = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(sessionname);
            int ins = 0;
            for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
            {
                if (cbl_hostelname1.Items[i].Selected == true)
                {
                    //string insertquery = "insert into Session_Master (Session_Code,Session_TxtCode,Session_Name,Start_Time,End_Time,Is_Extension,Extension_Time,Hostel_Code) values ('" + session_Code + "','0','" + sessionname + "','" + startingtime + "','" + endingtime + "','" + isextension + "','" + extensiontime + "','" + cbl_hostelname1.Items[i].Value + "')";

                    string insertquery = "insert into HM_SessionMaster (SessionAcr,SessionName,SessionStartTime,SessionCloseTime,IsAllowExtTime,SessionCloseExtTime,MessMasterFK) values('','" + sessionname + "','" + startingtime + "','" + endingtime + "','" + isextension + "','" + extensiontime + "','" + cbl_hostelname1.Items[i].Value + "' )";


                    ins = d2.update_method_wo_parameter(insertquery, "Text");
                }
            }
            if (ins != 0)
            {
                loadhostel();
                loadsession();
                imgdiv2.Visible = true;
                lbl_alert.Text = "Saved Successfully";
                btn_addnew_Click(sender, e);
                btn_go_Click(sender, e);
                time.Text = "";

            }
        }
        catch
        {
        }
    }

    protected void btn_exit_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    protected void btn_update_Click(object sender, EventArgs e)
    {
        try
        {
            string session_Code = Convert.ToString(Session["SessionMasterPK"]);
            string sessionname = Convert.ToString(txt_sessionname.Text);
            // string startingtime = "";
            // string endingtime = "";
            string extensiontime = "";
            //string isextension = "";
            string hour = Convert.ToString(ddl_hour.SelectedItem.Text);
            string minits = Convert.ToString(ddl_minits.SelectedItem.Text);
            string seconds = Convert.ToString(ddl_seconds.SelectedItem.Text);
            string format = Convert.ToString(ddl_timeformate.SelectedItem.Text);
            startingtime = hour + ":" + minits + ":" + seconds + " " + format;

            string endhour = Convert.ToString(ddl_endhour.SelectedItem.Text);
            string endminits = Convert.ToString(ddl_endminit.SelectedItem.Text);
            string endseconds = Convert.ToString(ddl_endsecnonds.SelectedItem.Text);
            string endformat = Convert.ToString(ddl_endformate.SelectedItem.Text);
            endingtime = endhour + ":" + endminits + ":" + endseconds + " " + endformat;

            if (cb_Extentionallow1.Checked == true)
            {
                isextension = "1";
                exhour = Convert.ToString(ddl_exhour.SelectedItem.Text);
                exminits = Convert.ToString(ddl_exminitus.SelectedItem.Text);
                exseconds = Convert.ToString(ddl_exseconds.SelectedItem.Text);
                exformat = Convert.ToString(ddl_exformate.SelectedItem.Text);
                extensiontime = exhour + ":" + exminits + ":" + exseconds + " " + exformat;
            }
            else
            {
                isextension = "0";
            }


            starttime = Convert.ToDateTime(startingtime);
            endtime = Convert.ToDateTime(endingtime);


            if (starttime < endtime)
            {

                if (cb_Extentionallow1.Checked == true)
                {
                    exten = Convert.ToDateTime(extensiontime);
                    if (starttime < exten && endtime < exten)
                    {
                        update_btn();
                    }
                    else
                    {
                        ddl_hour.SelectedIndex = ddl_hour.Items.Count - 1;
                        ddl_minits.SelectedIndex = 0;
                        ddl_timeformate.SelectedIndex = 0;
                        ddl_endhour.SelectedIndex = ddl_endhour.Items.Count - 1;
                        ddl_endminit.SelectedIndex = 0;
                        ddl_endformate.SelectedIndex = 0;
                        ddl_exhour.SelectedIndex = ddl_exhour.Items.Count - 1;
                        ddl_exminitus.SelectedIndex = 0;
                        ddl_exformate.SelectedIndex = 0;

                        string a = "Start time";
                        string b = "greater then extention time";
                        string c = "Please select valid time";
                        time.Visible = true;
                        time.Text = a + "  " + starttime.ToString("h:mm:ss:tt") + "  " + b + "  " + exten.ToString("h:mm:ss:tt") + "  " + c;
                    }
                }
                else
                {
                    update_btn();
                }
            }
            else
            {
                ddl_hour.SelectedIndex = ddl_hour.Items.Count - 1;
                ddl_minits.SelectedIndex = 0;
                ddl_timeformate.SelectedIndex = 0;
                ddl_endhour.SelectedIndex = ddl_endhour.Items.Count - 1;
                ddl_endminit.SelectedIndex = 0;
                ddl_endformate.SelectedIndex = 0;
                ddl_exhour.SelectedIndex = ddl_exhour.Items.Count - 1;
                ddl_exminitus.SelectedIndex = 0;
                ddl_exformate.SelectedIndex = 0;


                string a = "Start time";
                string b = "greater then endtime";
                string c = "Please select valid time";

                time.Visible = true;
                time.Text = a + " " + starttime.ToString("h:mm:ss:tt") + "  " + b + "  " + endtime.ToString("h:mm:ss:tt") + "  " + c;
            }

        }
        catch
        {
        }
    }

    protected void update_btn()
    {

        if (cb_Extentionallow1.Checked == true)
        {
            isextension = "1";
            exhour = Convert.ToString(ddl_exhour.SelectedItem.Text);
            exminits = Convert.ToString(ddl_exminitus.SelectedItem.Text);
            exseconds = Convert.ToString(ddl_exseconds.SelectedItem.Text);
            exformat = Convert.ToString(ddl_exformate.SelectedItem.Text);
            extensiontime = exhour + ":" + exminits + ":" + exseconds + " " + exformat;
        }
        else
        {
            isextension = "0";
        }
        string session_Code = Convert.ToString(Session["SessionMasterPK"]);
        string sessionname = Convert.ToString(txt_sessionname.Text);
        string insertquery = "";
        int ins = 0;
        for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
        {
            if (cbl_hostelname1.Items[i].Selected == true)
            {
                //string deltequery = "delete from Session_Master where Session_Code ='" + session_Code + "' and Hostel_Code ='" + cbl_hostelname1.Items[i].Value + "'";
                //int del = d2.update_method_wo_parameter(deltequery, "Text");

                //insertquery = "insert into Session_Master (Session_Code,Session_TxtCode,Session_Name,Start_Time,End_Time,Is_Extension,Extension_Time,Hostel_Code) values ('" + session_Code + "','0','" + sessionname + "','" + startingtime + "','" + endingtime + "','" + isextension + "','" + extensiontime + "','" + cbl_hostelname1.Items[i].Value + "')";


                insertquery = " update HM_SessionMaster set SessionName='" + sessionname + "', SessionStartTime='" + startingtime + "', SessionCloseTime='" + endingtime + "', IsAllowExtTime='" + isextension + "', SessionCloseExtTime='" + extensiontime + "' where SessionMasterPK='" + Convert.ToString(Session["SessionMasterPK"]) + "' ";
                ins = d2.update_method_wo_parameter(insertquery, "Text");
            }
        }
        if (ins != 0)
        {
            loadhostel();
            loadsession();
            poperrjs.Visible = false;
            imgdiv2.Visible = true;
            lbl_alert.Text = "Updated Successfully";
            time.Text = "";
            btn_go_Click(sender, e);
            //ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Updated Sucessfully\");", true);
        }
    }

    protected void btn_delete_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_delete.Text == "Delete")
            {
                surediv.Visible = true;
                lbl_sure.Text = "Do you want to delete this Record?";
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
            string session_Code = Convert.ToString(Session["sessioncode"]);
            int del = 0;
            for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
            {
                if (cbl_hostelname1.Items[i].Selected == true)
                {
                    string deltequery = "delete from HM_SessionMaster where SessionMasterPK='" + Convert.ToString(Session["SessionMasterPK"]) + "'";
                    del = d2.update_method_wo_parameter(deltequery, "Text");
                }
            }
            if (del != 0)
            {
                loadhostel();
                loadsession();
                bindendhrddl();
                bindendminsddl();
                bindendformat();
                poperrjs.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Text = "Deleted Successfully";
                btn_go_Click(sender, e);
                //ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Deleted Sucessfully\");", true);
            }
        }
        catch
        {
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
        poperrjs.Visible = true;
    }
    public object sender { get; set; }
    public EventArgs e { get; set; }
    [WebMethod]
    public static string CheckUserName(string Session_Name)
    {
        string returnValue = "1";
        try
        {
            DAccess2 dd = new DAccess2();
            string user_name = Session_Name;
            //  string itemheader = sessionname;

            if (user_name.Trim() != "" && user_name != null)
            {
                string query = dd.GetFunction("select distinct SessionName from HM_SessionMaster where SessionName ='" + user_name + "'");
                // string query = dd.GetFunction("select distinct Session_Name from Session_Master where Session_Name ='" + user_name + "' and Hostel_Code in ('" + itemheader + "') ");
                if (query.Trim() == "" || query == null || query == "0" || query == "-1")
                {
                    returnValue = "0";
                }

            }
            else
            {
                returnValue = "2";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }
}
