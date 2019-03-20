using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Configuration;

public partial class inv_hostelexpanses_and_strengthreport : System.Web.UI.Page
{
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    DAccess2 da = new DAccess2();
    ReuasableMethods rs = new ReuasableMethods();
    bool check = false;
    string college = "";
    static string cln = "";
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
        hat.Add("single_user", singleuser);
        hat.Add("group_code", group_user);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        if (!IsPostBack)
        {
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread1.Visible = false;

            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Attributes.Add("readonly", "readonly");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            bindhostelname();
            loadsession();
            loadmenuname();
            loaditem1();
            //btn_go_Click(sender, e);
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
    protected void cb_hostelname_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;

        txt_hostelname.Text = "--Select--";
        if (cb_hostelname.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                cbl_hostelname.Items[i].Selected = true;
            }
            txt_hostelname.Text = "Mess Name(" + (cbl_hostelname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                cbl_hostelname.Items[i].Selected = false;
            }
            txt_hostelname.Text = "--Select--";
            cb_sessionname.Checked = false;

        }
        loadsession();

    }
    protected void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            cb_hostelname.Checked = false;
            int commcount = 0;
            //string buildvalue = "";
            //string build = "";
            txt_hostelname.Text = "--Select--";
            for (i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_hostelname.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_hostelname.Items.Count)
                {
                    cb_hostelname.Checked = true;
                }
                txt_hostelname.Text = "Mess Name(" + commcount.ToString() + ")";

            }

            loadsession();
        }
        catch (Exception ex)
        {

        }
    }
    public void bindhostelname()
    {
        try
        {
            cbl_hostelname.Items.Clear();
            ds.Clear();
            //ds = d2.Bindmess_inv(collegecode1);
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "MessName";
                cbl_hostelname.DataValueField = "MessMasterPK";
                cbl_hostelname.DataBind();
                if (cbl_hostelname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                    {
                        cbl_hostelname.Items[i].Selected = true;
                        cb_hostelname.Checked = true;
                    }
                    txt_hostelname.Text = "Mess Name(" + cbl_hostelname.Items.Count + ")";
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
    protected void cb_sessionname_CheckedChanged(object sender, EventArgs e)
    {

        if (cb_sessionname.Checked == true)
        {
            for (int i = 0; i < cbl_sessionname.Items.Count; i++)
            {
                cbl_sessionname.Items[i].Selected = true;
            }
            txt_sessionname.Text = "Session Name(" + (cbl_sessionname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_sessionname.Items.Count; i++)
            {
                cbl_sessionname.Items[i].Selected = false;
            }
            txt_sessionname.Text = "--Select--";
            txt_menuname.Text = "--Select--";
            txt_itemname.Text = "--Select--";
            cb_menuname.Checked = false;
            cb_itemname.Checked = false;
        }
        loadmenuname();
    }

    protected void cbl_sessionname_SelectedIndexChanged(object sender, EventArgs e)
    {

        txt_sessionname.Text = "--Select--";
        cb_sessionname.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_sessionname.Items.Count; i++)
        {
            if (cbl_sessionname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {

            if (commcount == cbl_sessionname.Items.Count)
            {
                cb_sessionname.Checked = true;
            }
            txt_sessionname.Text = "Session Name(" + commcount.ToString() + ")";
        }
        loadmenuname();
    }

    public void loadsession()
    {
        try
        {
            ds.Clear();
            cbl_sessionname.Items.Clear();

            string itemheader = "";
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemheader.Trim() != "")
            {
                //string selecthostel = "select distinct Session_Code,Session_Name  from Session_Master where Hostel_Code in ('" + itemheader + "')";
                //ds = d2.select_method_wo_parameter(selecthostel, "Text");

                ds = d2.BindSession_inv(itemheader);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sessionname.DataSource = ds;
                    cbl_sessionname.DataTextField = "SessionName";
                    cbl_sessionname.DataValueField = "SessionMasterPK";
                    cbl_sessionname.DataBind();
                    if (cbl_sessionname.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_sessionname.Items.Count; i++)
                        {
                            cbl_sessionname.Items[i].Selected = true;
                            cb_sessionname.Checked = true;
                        }
                        txt_sessionname.Text = "Session Name(" + cbl_sessionname.Items.Count + ")";
                    }
                }
                else
                {
                    txt_sessionname.Text = "--Select--";
                }
            }
            else
            {
                txt_sessionname.Text = "--Select--";
            }
        }
        catch
        {

        }
    }
    protected void cb_menuname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_itemname.Text = "--Select--";
            cb_itemname.Checked = false;

            if (cb_menuname.Checked == true)
            {
                for (int i = 0; i < cbl_menuname.Items.Count; i++)
                {
                    cbl_menuname.Items[i].Selected = true;
                }
                txt_menuname.Text = "Menu Name(" + (cbl_menuname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_menuname.Items.Count; i++)
                {
                    cbl_menuname.Items[i].Selected = false;
                }
                txt_menuname.Text = "--Select--";
            }

            loaditem1();
        }
        catch (Exception ex)
        {

        }
    }
    protected void cbl_menuname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            txt_itemname.Text = "--Select--";
            cb_itemname.Checked = false;
            cbl_itemname.Items.Clear();

            int commcount = 0;
            for (int i = 0; i < cbl_menuname.Items.Count; i++)
            {
                if (cbl_menuname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_menuname.Text = "Menu Name(" + commcount.ToString() + ")";
                if (commcount == cbl_menuname.Items.Count)
                {
                    cb_menuname.Checked = true;
                }
            }
            else
            {
                for (int i = 0; i < cbl_menuname.Items.Count; i++)
                {
                    cbl_menuname.Items[i].Selected = false;
                }
                cb_menuname.Checked = false;
                txt_menuname.Text = "--Select--";
            }
            loaditem1();
        }
        catch (Exception ex)
        {

        }
    }
    public void loadmenuname()
    {
        try
        {
            hat.Clear();
            string item = "";
            txt_menuname.Text = "--Select--";
            cbl_menuname.Items.Clear();
            string hostelcode = "";
            for (int i = 0; i < cbl_sessionname.Items.Count; i++)
            {
                if (cbl_sessionname.Items[i].Selected == true)
                {
                    if (item == "")
                    {
                        item = "" + cbl_sessionname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        item = item + "'" + "," + "'" + cbl_sessionname.Items[i].Value.ToString() + "";
                    }
                }
            }
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    if (hostelcode == "")
                    {
                        hostelcode = "" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        hostelcode = hostelcode + "'" + "," + "'" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                }
            }

            if (item.Trim() != "")
            {
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string firstdate1 = Convert.ToString(txt_todate.Text);
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = firstdate1.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                DateTime dnew = dt;
                string days = "";
                while (dnew <= dt1)
                {
                    if (days == "")
                    {
                        days = dnew.ToString("dddd");
                    }
                    else
                    {
                        days = days + "'" + "," + "'" + dnew.ToString("dddd") + "";
                    }
                    dnew = dnew.AddDays(1);
                }
                if (item.Trim() != "" && hostelcode.Trim() != "")
                {
                    string menuquery = "";

                    #region old menu
                    //menuquery = "select Menu_Code,Session_Code from MenuSchedule_DateWise where Session_Code in ('" + item + "') and Hostel_Code in ('" + hostelcode + "') and Schedule_Date  between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and schedule_type='0'";
                    //menuquery = menuquery + " select Menu_Code,Session_Code from MenuSchedule_DayWise where Session_Code in ('" + item + "') and Hostel_Code in ('" + hostelcode + "') and Schedule_Day in ('" + days + "') and schedule_type='0'";
                    //ds.Clear();
                    //ds = d2.select_method_wo_parameter(menuquery, "Text");
                    //menuquery = "";
                    //if (ds.Tables[0].Rows.Count > 0)
                    //{
                    //    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    //    {
                    //        string menucode = Convert.ToString(ds.Tables[1].Rows[row][0]);
                    //        string[] split_new = menucode.Split(',');
                    //        if (split_new.Length > 0)
                    //        {
                    //            for (int low = 0; low <= split_new.GetUpperBound(0); low++)
                    //            {
                    //                if (menuquery.Trim() == "")
                    //                {
                    //                    menuquery = Convert.ToString(split_new[low]);
                    //                }
                    //                else
                    //                {
                    //                    menuquery = menuquery + "'" + "," + "'" + Convert.ToString(split_new[low]);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    if (ds.Tables[1].Rows.Count > 0)
                    //    {
                    //        for (int row = 0; row < ds.Tables[1].Rows.Count; row++)
                    //        {
                    //            string menucode = Convert.ToString(ds.Tables[1].Rows[row][0]);
                    //            string[] split_new = menucode.Split(',');
                    //            if (split_new.Length > 0)
                    //            {
                    //                for (int low = 0; low <= split_new.GetUpperBound(0); low++)
                    //                {
                    //                    if (menuquery.Trim() == "")
                    //                    {
                    //                        menuquery = Convert.ToString(split_new[low]);
                    //                    }
                    //                    else
                    //                    {
                    //                        menuquery = menuquery + "'" + "," + "'" + Convert.ToString(split_new[low]);
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion

                    menuquery = "select MenuMasterFK,SessionMasterFK from HT_MenuSchedule where SessionMasterFK in('" + item + "') and MessMasterFK in('" + hostelcode + "')  and ScheudleItemType='1' and ScheduleType ='1' and MenuScheduleDate ='" + dt.ToString("MM/dd/yyyy") + "'";
                    menuquery = menuquery + "  select MenuMasterFK,SessionMasterFK from HT_MenuSchedule where SessionMasterFK in('" + item + "') and MessMasterFK in('" + hostelcode + "')  and ScheudleItemType='1' and ScheduleType ='2' and MenuScheduleday  in('" + days + "')";//dt.ToString("dddd")
                    ds = d2.select_method_wo_parameter(menuquery, "Text");

                    string menucode = "";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                        {
                            string mcode = Convert.ToString(ds.Tables[0].Rows[k]["MenuMasterFK"]);
                            if (menucode.Contains(mcode) == false)
                            {
                                if (menucode == "")
                                {
                                    menucode = mcode;
                                }
                                else
                                {
                                    menucode = menucode + "'" + "," + "'" + mcode;
                                }
                            }
                        }
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int k = 0; k < ds.Tables[1].Rows.Count; k++)
                        {
                            string mcode = Convert.ToString(ds.Tables[1].Rows[k]["MenuMasterFK"]);
                            if (menucode.Contains(mcode) == false)
                            {
                                if (menucode == "")
                                {
                                    menucode = mcode;
                                }
                                else
                                {
                                    menucode = menucode + "'" + "," + "'" + mcode;
                                }
                            }
                        }
                    }
                    //}
                    string deptquery = "select distinct MenuMasterPK,MenuName,MenuCode  from HM_MenuMaster where CollegeCode ='" + collegecode1 + "' and MenuMasterPK in('" + menucode + "')  order by MenuName ";

                    // string deptquery = "select distinct MenuCode,MenuName  from MenuMaster where College_Code ='" + collegecode1 + "' and MenuCode in('" + menuquery + "')  order by MenuCode ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(deptquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cbl_menuname.DataSource = ds;
                        cbl_menuname.DataTextField = "MenuName";
                        cbl_menuname.DataValueField = "MenuMasterPK";
                        cbl_menuname.DataBind();
                        if (cbl_menuname.Items.Count > 0)
                        {
                            for (int i = 0; i < cbl_menuname.Items.Count; i++)
                            {
                                cbl_menuname.Items[i].Selected = true;
                                cb_menuname.Checked = true;
                            }
                            txt_menuname.Text = "Menu Name(" + cbl_menuname.Items.Count + ")";
                            lbl_menuname.Text = "Menu Name";
                        }
                    }
                    else
                    {
                        txt_menuname.Text = "--Select--";
                    }
                }
            }
            loaditem1();
        }
        catch
        {

        }
    }
    public void cblcolumnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox_column.Checked = false;
            string value = "";
            int index;
            cblcolumnorder.Items[0].Selected = true;
            // cblcolumnorder.Items[0].Enabled = false;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    //if (tborder.Text == "")
                    //{
                    //    ItemList.Add("Company Code");
                    //}
                    //ItemList.Add("Admission No");
                    //ItemList.Add("Name");
                    ItemList.Add(cblcolumnorder.Items[index].Value.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cblcolumnorder.Items[index].Value.ToString());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                //if (i == 0 || i == 1 || i == 2)
                //{
                //    cblcolumnorder.Items[0].Selected = true;
                //    cblcolumnorder.Items[1].Selected = true;
                //    cblcolumnorder.Items[2].Selected = true;
                //}
                if (cblcolumnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblcolumnorder.Items[i].Value.ToString());
                    Itemindex.Remove(sindex);

                }
            }

            lnk_columnorder.Visible = true;
            tborder.Visible = true;
            tborder.Text = "";
            string colname12 = "";
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (colname12 == "")
                {
                    colname12 = ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";

                }
                else
                {
                    colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
                }
                //tborder.Text = tborder.Text + ItemList[i].ToString();

                //tborder.Text = tborder.Text + "(" + (i + 1).ToString() + ")  ";

            }
            tborder.Text = colname12;
            if (ItemList.Count == 14)
            {
                CheckBox_column.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                tborder.Visible = false;
                lnk_columnorder.Visible = false;
            }

            //  Button2.Focus();
        }
        catch (Exception ex)
        {

        }
    }

    public void CheckBox_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column.Checked == true)
            {
                tborder.Text = "";
                ItemList.Clear();
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    ItemList.Add(cblcolumnorder.Items[i].Value.ToString());
                    Itemindex.Add(si);
                }
                lnk_columnorder.Visible = true;
                tborder.Visible = true;
                tborder.Text = "";
                int j = 0;
                string colname12 = "";
                for (int i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    if (colname12 == "")
                    {
                        colname12 = ItemList[i].ToString() + "(" + (j).ToString() + ")";

                    }
                    else
                    {
                        colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (j).ToString() + ")";
                    }
                    // tborder.Text = tborder.Text + ItemList[i].ToString();



                }
                tborder.Text = colname12;

            }
            else
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                    lnk_columnorder.Visible = false;
                    ItemList.Clear();
                    Itemindex.Clear();
                    //cblcolumnorder.Items[0].Selected = true;
                }

                tborder.Text = "";
                tborder.Visible = false;

            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {

        }
    }
    public void LinkButtonsremove_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder.ClearSelection();
            CheckBox_column.Checked = false;
            lnk_columnorder.Visible = false;
            //cblcolumnorder.Items[0].Selected = true;
            ItemList.Clear();
            Itemindex.Clear();
            tborder.Text = "";
            tborder.Visible = false;
            //Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }


    protected void txt_fromdate_TextChanged(object sender, EventArgs e)
    {
        datevalidate(txt_fromdate, txt_todate);
        loadmenuname();
        loaditem1();

    }
    protected void txt_todate_TextChanged(object sender, EventArgs e)
    {
        datevalidate(txt_fromdate, txt_todate);
        loadmenuname();
        loaditem1();
        //txt_menuname.Text = "--Select--";
        //txt_itemname.Text = "--Select--";
        //cb_menuname.Checked = false;
        //cb_itemname.Checked = false;

    }
    public void datevalidate(TextBox txt1, TextBox txt2)
    {
        try
        {
            if (txt1.Text != "" && txt2.Text != "")
            {

                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt1.Text);
                string seconddate = Convert.ToString(txt2.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;
                if (dt > dt1)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Select ToDate greater than or equal to the FromDate ";
                    //(or)

                    //Response.Write("<script>alert('Select ToDate greater than or equal to the FromDate')</script>");
                    txt2.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txt1.Text = DateTime.Now.ToString("dd/MM/yyyy");

                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void cb_itemname_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_itemname.Checked == true)
            {
                for (int i = 0; i < cbl_itemname.Items.Count; i++)
                {
                    cbl_itemname.Items[i].Selected = true;
                }
                txt_itemname.Text = "Item Name(" + (cbl_itemname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_itemname.Items.Count; i++)
                {
                    cbl_itemname.Items[i].Selected = false;
                }

                txt_itemname.Text = "--Select--";
            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void cbl_itemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_itemname.Text = "--Select--";

            int commcount = 0;
            for (int i = 0; i < cbl_itemname.Items.Count; i++)
            {
                if (cbl_itemname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_itemname.Text = "Item Name(" + commcount.ToString() + ")";
                if (commcount == cbl_itemname.Items.Count)
                {
                    cb_itemname.Checked = true;
                }
            }
            else
            {
                cb_itemname.Checked = false;
            }

        }

        catch (Exception ex)
        {

        }
    }
    public void loaditem1()
    {

        try
        {
            cbl_itemname.Items.Clear();
            string menuvalue = "";
            string menuvalue1 = "";

            if (cbl_menuname.Items.Count > 0)
            {
                for (int i = 0; i < cbl_menuname.Items.Count; i++)
                {
                    if (cbl_menuname.Items[i].Selected == true)
                    {
                        if (menuvalue == "")
                        {
                            menuvalue = "" + cbl_menuname.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            menuvalue = menuvalue + "'" + "," + "'" + cbl_menuname.Items[i].Value.ToString() + "";
                        }
                    }
                }
            }

            if (cbl_hostelname.Items.Count > 0)
            {
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cbl_hostelname.Items[i].Selected == true)
                    {
                        if (menuvalue1 == "")
                        {
                            menuvalue1 = "" + cbl_hostelname.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            menuvalue1 = menuvalue1 + "'" + "," + "'" + cbl_hostelname.Items[i].Value.ToString() + "";
                        }
                    }
                }
            }


            if (menuvalue != "" && menuvalue1 != "")
            {
                ds.Clear();

                string selectquery = "select distinct md.ItemFK,i.ItemName from HM_MenuItemDetail md,HM_MenuItemMaster mm,IM_ItemMaster i where md.MenuItemMasterFK=mm.MenuItemMasterPK and i.ItemPK=md.ItemFK and mm.MenuMasterFK in('" + menuvalue + "')";

                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_itemname.DataSource = ds;
                    cbl_itemname.DataTextField = "ItemName";
                    cbl_itemname.DataValueField = "ItemFK";
                    cbl_itemname.DataBind();

                    if (cbl_itemname.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_itemname.Items.Count; row++)
                        {
                            cbl_itemname.Items[row].Selected = true;
                        }
                        txt_itemname.Text = "Item Name(" + cbl_itemname.Items.Count + ")";
                        cb_itemname.Checked = true;
                    }
                    else
                    {
                        for (int i = 0; i < cbl_itemname.Items.Count; i++)
                        {
                            cbl_itemname.Items[i].Selected = false;
                        }
                        cb_itemname.Checked = false;
                        txt_itemname.Text = "--Select--";
                    }
                }
                else
                {
                    for (int i = 0; i < cbl_itemname.Items.Count; i++)
                    {
                        cbl_itemname.Items[i].Selected = false;
                    }
                    cb_itemname.Checked = false;
                    txt_itemname.Text = "--Select--";
                }
            }

        }
        catch
        {
        }
    }
    public void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string colno = "";
            string hostelFk = rs.GetSelectedItemsValueAsString(cbl_hostelname);
            string sessionFk = rs.GetSelectedItemsValueAsString(cbl_sessionname);
            string ItemFk = rs.GetSelectedItemsValueAsString(cbl_itemname);
            string menumasterFk = rs.GetSelectedItemsValueAsString(cbl_menuname);
            if (ItemList.Count == 0)
            {
                ItemList.Add("MessName");
                ItemList.Add("SessionName");
                ItemList.Add("MenuName");
                ItemList.Add("ItemCode");
                ItemList.Add("itemname");
                ItemList.Add("RPU");
                ItemList.Add("Consumption_Date");
                ItemList.Add("qut");
                ItemList.Add("value");
                ItemList.Add("Total_Present");
                //ItemList.Add("VegCount");
                //ItemList.Add("NonVegCount");
            }

            Hashtable columnhash = new Hashtable();
            columnhash.Clear();

            int colinc = 0;
            columnhash.Add("MessName", "Mess Name");
            columnhash.Add("SessionName", "Session Name");
            columnhash.Add("MenuName", "Menu Name");
            columnhash.Add("ItemCode", "Item Code");
            columnhash.Add("itemname", "Item Name");
            columnhash.Add("RPU", "RPU");
            columnhash.Add("Consumption_Date", "Consumption Date");
            columnhash.Add("qut", "Consumption QTY");
            columnhash.Add("value", "Consumption Value");
            columnhash.Add("Total_Present", "Strength");
            columnhash.Add("VegStrength", "VegCount");
            columnhash.Add("NonvegStrength", "NonVegCount");

            string getday = "";
            string gettoday = "";
            string from = "";
            string to = "";
            from = Convert.ToString(txt_fromdate.Text);
            string[] splitdate = from.Split('-');
            splitdate = splitdate[0].Split('/');
            DateTime dt = new DateTime();
            if (splitdate.Length > 0)
            {
                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            }
            getday = dt.ToString("MM/dd/yyyy");

            to = Convert.ToString(txt_todate.Text);
            string[] splitdate1 = to.Split('-');
            splitdate1 = splitdate1[0].Split('/');
            DateTime dt1 = new DateTime();
            if (splitdate1.Length > 0)
            {
                dt1 = Convert.ToDateTime(splitdate1[1] + "/" + splitdate1[0] + "/" + splitdate1[2]);
            }
            gettoday = dt1.ToString("MM/dd/yyyy");
            if (hostelFk != "" && ItemFk != "" && sessionFk.Trim() != "" && menumasterFk.Trim() != "")
            {
                string selectquery = "  select me.MessName,s.SessionName,i.ItemCode,i.itemname,MenuName,RPU,CONVERT(varchar(10), DailyConsDate,103) as Consumption_Date,SUM( ConsumptionQty)qut,SUM( ConsumptionQty*RPU)value,dm.Total_Present,dm.VegStrength,dm.NonvegStrength from HM_MenuMaster Ma ,HT_DailyConsumptionMaster dm ,HT_DailyConsumptionDetail dd,HM_SessionMaster s,IM_ItemMaster i,HM_MessMaster me where dd.DailyConsumptionMasterFK =dm.DailyConsumptionMasterPK and s.SessionMasterPK=dm.SessionFK and i.ItemPK=dd.ItemFK and dm.SessionFK=s.SessionMasterPK and dm.MessMasterFK=me.MessMasterPK and dm.MessMasterFK in ('" + hostelFk + "') and s.SessionMasterPK in ('" + sessionFk + "') and  dd.ItemFK in ('" + ItemFk + "') and DailyConsDate between  '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and dm.MenumasterFK in('" + menumasterFk + "') and dm.MenumasterFK=ma.MenuMasterPK  group by DailyConsDate , dd.ItemFK , s.SessionName,MenuName, i.itemname, me.MessName ,RPU,dm.Total_Present,ItemCode,dm.VegStrength,dm.NonvegStrength order by DailyConsDate ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    pcolumnorder.Visible = true;
                    Fpspread1.Sheets[0].RowHeader.Visible = false;
                    //Fpspread1.Sheets[0].ColumnCount = 11;
                    Fpspread1.CommandBar.Visible = false;
                    Fpspread1.Sheets[0].RowCount = 0;
                    Fpspread1.SheetCorner.ColumnCount = 0;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread1.Sheets[0].RowHeader.Visible = false;
                    Fpspread1.Sheets[0].ColumnCount = ItemList.Count + 1;
                    Fpspread1.Sheets[0].RowCount = ds.Tables[0].Rows.Count;
                    Fpspread1.Sheets[0].AutoPostBack = true;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        colno = Convert.ToString(ds.Tables[0].Columns[j]);
                        if (ItemList.Contains(Convert.ToString(colno)))
                        {
                            int index = ItemList.IndexOf(Convert.ToString(colno));
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Text = Convert.ToString(columnhash[colno]);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Fpspread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                        Fpspread1.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;

                        for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (ItemList.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
                            {
                                int index = ItemList.IndexOf(Convert.ToString(ds.Tables[0].Columns[j].ToString()));
                                Fpspread1.Sheets[0].Columns[index + 1].Width = 150;
                                Fpspread1.Sheets[0].Columns[index + 1].Locked = true;
                                Fpspread1.Sheets[0].Cells[i, index + 1].CellType = txt;
                                Fpspread1.Sheets[0].Cells[i, index + 1].Text = ds.Tables[0].Rows[i][j].ToString();
                                Fpspread1.Sheets[0].Cells[i, index + 1].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[i, index + 1].Font.Size = FontUnit.Medium;
                                if (ds.Tables[0].Columns[j].ToString() == "qut")
                                {
                                    Fpspread1.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Center;

                                }
                                if (ds.Tables[0].Columns[j].ToString() == "value")
                                {
                                    Fpspread1.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Right;

                                }
                                if (ds.Tables[0].Columns[j].ToString() == "Total_Present" || ds.Tables[0].Columns[j].ToString() == "VegStrength" || ds.Tables[0].Columns[j].ToString() == "NonvegStrength")
                                {
                                    Fpspread1.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Right;
                                }
                            }
                        }
                    }
                    rptprint.Visible = true;
                    Fpspread1.Visible = true;
                    div1.Visible = true;
                    lbl_error.Visible = false;
                    pheaderfilter.Visible = true;
                    pcolumnorder.Visible = true;
                    Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpspread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpspread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpspread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpspread1.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                }
                else
                {
                    rptprint.Visible = false;
                    // imgdiv2.Visible = true;
                    //lbl_alert.Text = "No records found";
                    lbl_error.Text = "No records found";
                    pheaderfilter.Visible = false;
                    pcolumnorder.Visible = false;
                    div1.Visible = false;
                    Fpspread1.Visible = false;
                }
            }
            else
            {
                rptprint.Visible = false;
                // imgdiv2.Visible = true;
                //lbl_alert.Text = "No records found";
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select Any One Record";
                pheaderfilter.Visible = false;
                pcolumnorder.Visible = false;
                div1.Visible = false;
                Fpspread1.Visible = false;
            }
        }
        catch (Exception ex)
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
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Hostel Expenses and Strength Report";
            string pagename = "hostelexpanses_and_strengthreport.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }

    }
}

