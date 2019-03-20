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

public partial class HT_Income : System.Web.UI.Page
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

        if (!IsPostBack)
        {
            txt_todaydate.Attributes.Add("readonly", "readonly");
            txt_todaydate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Attributes.Add("readonly", "readonly");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            rdb_datewise.Checked = true;

            loadhostel();
            bindaddgroup();
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread1.Visible = false;
            btn_go_Click(sender, e);
        }
        lbl_error.Visible = false;
    }

    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        popwindow.Visible = true;
        ddl_hostelname.Enabled = true;
        ddl_group.Enabled = true;
        txt_popdesc.Enabled = true;
        txt_todaydate.Enabled = true;
        btn_update.Visible = false;
        btn_plus.Enabled = true;
        btn_minus.Enabled = true;


        txt_popdesc.Text = "";
        txt_amount.Text = "";
        loadhostel();

        bindaddgroup();
        btn_save.Visible = true;
        btn_delete.Visible = false;
        ddl_hostelname.SelectedItem.Value = "0";
        ddl_group.SelectedItem.Value = "0";

    }
    protected void btn_exit_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }
    protected void lnk_btn_logout_Click(object sender, EventArgs e)
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

        if (cb_hostelname.Checked == true)
        {
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                cbl_hostelname.Items[i].Selected = true;
            }
            txt_hostelname.Text = "Hostel Name(" + (cbl_hostelname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                cbl_hostelname.Items[i].Selected = false;
            }
            txt_hostelname.Text = "--Select--";
        }

    }
    protected void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_hostelname.Text = "--Select--";
        cb_hostelname.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_hostelname.Items.Count; i++)
        {
            if (cbl_hostelname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txt_hostelname.Text = "Hostel Name(" + commcount.ToString() + ")";
            if (commcount == cbl_hostelname.Items.Count)
            {
                cb_hostelname.Checked = true;
            }
        }

    }
    public void loadhostel()
    {
        try
        {
            //string bindhostel = "select HostelName,HostelMasterPK from HM_HostelMaster";
            //ds.Clear();
            //ds = d2.select_method_wo_parameter(bindhostel, "TEXT");

            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "HostelName";
                cbl_hostelname.DataValueField = "HostelMasterPK";
                cbl_hostelname.DataBind();


                ddl_hostelname.DataSource = ds;
                ddl_hostelname.DataTextField = "HostelName";
                ddl_hostelname.DataValueField = "HostelMasterPK";
                ddl_hostelname.DataBind();

                ddl_hostelname.Items.Insert(0, "Select");

                if (cbl_hostelname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                    {
                        cbl_hostelname.Items[i].Selected = true;
                    }

                    txt_hostelname.Text = "Hostel Name(" + cbl_hostelname.Items.Count + ")";
                }
            }
            else
            {
                ddl_hostelname.Items.Insert(0, "Select");

                txt_hostelname.Text = "--Select--";

            }
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

            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {

                    ItemList.Add(cblcolumnorder.Items[index].Text.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cblcolumnorder.Items[index].Text.ToString());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {

                if (cblcolumnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblcolumnorder.Items[i].Text.ToString());
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
                    ItemList.Add(cblcolumnorder.Items[i].Text.ToString());
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

                }

                tborder.Text = "";
                tborder.Visible = false;

            }

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

            ItemList.Clear();
            Itemindex.Clear();
            tborder.Text = "";
            tborder.Visible = false;

        }
        catch (Exception ex)
        {
        }
    }




    public void btn_go_Click(object sender, EventArgs e)
    {
        try
        {

            Printcontrol.Visible = false;
            if (txt_hostelname.Text.Trim() != "--Select--" && txt_groupname.Text.Trim() != "--Select--" && txt_description.Text.Trim() != "--Select--")
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_todate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                string[] split1 = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
                string hostel = "";
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cbl_hostelname.Items[i].Selected == true)
                    {
                        if (hostel == "")
                        {
                            hostel = "" + cbl_hostelname.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            hostel = hostel + "'" + "," + "" + "'" + cbl_hostelname.Items[i].Value.ToString() + "";
                        }
                    }
                }

                string group = "";
                for (int i = 0; i < cbl_groupname.Items.Count; i++)
                {
                    if (cbl_groupname.Items[i].Selected == true)
                    {
                        if (group == "")
                        {
                            group = "" + cbl_groupname.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            group = group + "'" + "," + "" + "'" + cbl_groupname.Items[i].Value.ToString() + "";
                        }
                    }
                }

                string gn = d2.GetFunction("");
                string q1 = "";
                if (rdb_datewise.Checked == true)
                {

                    // q1 = "select CONVERT(varchar(10), Entry_Date,103) as Entry_Date, hd.Hostel_Name,hi.IncGroup_Code,(select TextVal from TextValTable where TextCode = ISNULL( hi.IncGroup_Code,0)) as IncGroup_Code_txt,hi.Inc_Amount,hi.Inc_Description from Hostel_Income hi,Hostel_Details hd where  hd.Hostel_code=hi.Hostel_Code and hd.college_code='" + collegecode1 + "' and hi.Entry_Date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and hi.Hostel_Code=hd.Hostel_code and hi.Hostel_Code in('" + hostel + "')and hi.IncGroup_Code in('" + group + "')";
                    q1 = "select CONVERT(varchar(10), IncomeDate,103) as IncomeDate, hd.HostelName,hi.IncomeGroup,(select MasterValue from CO_MasterValues where MasterCode = ISNULL( hi.IncomeGroup,0)) as IncGroup_Code_txt,hi.IncomeAmount,hi.IncomeDesc from HT_HostelIncome hi,HM_HostelMaster hd where  hd.HostelMasterPK=hi.HostelMasterFK and hi.IncomeDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and hi.HostelMasterFK in('" + hostel + "')and hi.IncomeGroup in('" + group + "')";//and hd.collegecode='" + collegecode1 + "' 
                }
                else
                {
                    lbl_error1.Visible = false;
                    q1 = "select CONVERT(varchar(10), IncomeDate,103) as IncomeDate, hd.HostelName,hi.IncomeGroup,(select MasterValue from CO_MasterValues where MasterCode = ISNULL( hi.IncomeGroup,0)) as IncGroup_Code_txt,SUM(hi.IncomeAmount)as IncomeAmount,hi.IncomeDesc from HT_HostelIncome hi,HM_HostelMaster hd where hd.HostelMasterPK=hi.HostelMasterFK  and hi.HostelMasterFK in('" + hostel + "') and hi.IncomeGroup in('" + group + "') group by hi.IncomeGroup,hd.HostelName,hi.IncomeDesc,hi.IncomeDate";//
                }

                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fpspread2.Sheets[0].RowCount = 0;
                    Fpspread2.Sheets[0].ColumnCount = 0;
                    Fpspread2.CommandBar.Visible = false;
                    Fpspread2.Sheets[0].AutoPostBack = true;
                    Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread2.Sheets[0].RowHeader.Visible = false;
                    Fpspread2.Sheets[0].ColumnCount = 6;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread2.Columns[0].Width = 50;

                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Hostel Name";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread2.Columns[1].Width = 200;

                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Group Name";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread2.Columns[2].Width = 100;

                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Description";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread2.Columns[3].Width = 200;

                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Amount";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpspread2.Columns[4].Width = 150;

                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Date";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    Fpspread2.Columns[4].Width = 150;


                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["HostelName"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["IncGroup_Code_txt"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["IncomeGroup"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["IncomeDesc"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["IncomeAmount"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["IncomeDate"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    }
                    Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                    Fpspread2.SaveChanges();
                    Fpspread2.Visible = true;
                    rptprint.Visible = true;
                    div2.Visible = true;
                    lbl_error.Visible = false;
                }
                else
                {
                    Fpspread2.Visible = false;
                    div2.Visible = false;
                    rptprint.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "No Record Found";
                }
            }

            else
            {
                Fpspread2.Visible = false;
                rptprint.Visible = false;
                div2.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select All Field";
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
    protected void Fpspread2_render(object sender, EventArgs e)
    {
        try
        {
            if (check == true)
            {
                loadhostel();
                bindaddgroup();

                popwindow.Visible = true;
                btn_delete.Visible = true;
                btn_update.Visible = true;
                btn_save.Visible = false;
                string activerow = "";
                string activecol = "";
                activerow = Fpspread2.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread2.ActiveSheetView.ActiveColumn.ToString();
                collegecode = Session["collegecode"].ToString();
                if (activerow.Trim() != "")
                {
                    string todaydate = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
                    string hostelname = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    string groupname = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                    string groupnamecode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                    string description = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                    string amount = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);

                    ddl_hostelname.SelectedIndex = ddl_hostelname.Items.IndexOf(ddl_hostelname.Items.FindByText(hostelname));
                    ddl_group.SelectedIndex = ddl_group.Items.IndexOf(ddl_group.Items.FindByText(groupname));

                    //ddl_group.SelectedItem.Text = Convert.ToString(groupname);
                    //ddl_group.SelectedItem.Value = Convert.ToString(groupnamecode);
                    txt_popdesc.Text = Convert.ToString(description);
                    txt_amount.Text = Convert.ToString(amount);
                    txt_todaydate.Text = Convert.ToString(todaydate);

                    ddl_hostelname.Enabled = false;
                    ddl_group.Enabled = false;
                    txt_popdesc.Enabled = false;
                    txt_todaydate.Enabled = false;

                    btn_plus.Enabled = false;
                    btn_minus.Enabled = false;

                }
            }
        }
        catch
        {

        }

    }
    protected void btn_Update_Click(object sender, EventArgs e)
    {
        try
        {
            string hostelname = Convert.ToString(ddl_hostelname.SelectedItem.Text);
            string groupname = Convert.ToString(ddl_group.SelectedItem.Value);
            string description = Convert.ToString(txt_popdesc.Text);
            string amount = Convert.ToString(txt_amount.Text);
            string date = Convert.ToString(txt_todaydate.Text);
            string[] split = date.Split('/');
            DateTime dt = new DateTime();
            if (split.Length >= 2)
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

            if (groupname.Trim() != "")
            {
                string inserquery = "if exists (select * from HT_HostelIncome where IncomeGroup='" + groupname + "' and IncomeDesc='" + description + "' and HostelMasterFK='" + ddl_hostelname.SelectedItem.Value + "' and incomedate='" + date + "') update HT_HostelIncome set IncomeAmount='" + amount + "'  where IncomeGroup='" + groupname + "' and IncomeDesc='" + description + "' and HostelMasterFK='" + ddl_hostelname.SelectedItem.Value + "' and incomedate='" + dt.ToString("MM/dd/yyy") + "'";
                int ins = d2.update_method_wo_parameter(inserquery, "Text");
                if (ins != 0)
                {
                    btn_go_Click(sender, e);
                    popwindow.Visible = false;
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Updated Successfully";
                    lblalerterr.Visible = true;
                }
            }
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
                d2.printexcelreport(Fpspread2, reportname);
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
            string degreedetails = "Hostel Income Report";
            string pagename = "hosincomeexpances.aspx";
            Printcontrol.loadspreaddetails(Fpspread2, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }

    }


    public void loadgroup()
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_group.Items.Insert(0, "Select");

            if (cbl_groupname.Items.Count > 0)
            {
                for (int i = 0; i < cbl_groupname.Items.Count; i++)
                {
                    cbl_groupname.Items[i].Selected = true;
                }
                txt_groupname.Text = "Header Name(" + cbl_groupname.Items.Count + ")";
            }
        }
        else
        {
            ddl_group.Items.Insert(0, "Select");

            txt_groupname.Text = "--Select--";
        }
    }

    public void loadsubgroup()
    {
        if (ds.Tables[0].Rows.Count > 0)
        {

            ddl_subgroup.Items.Insert(0, "Select");

            if (cbl_subgroupname.Items.Count > 0)
            {
                for (int i = 0; i < cbl_subgroupname.Items.Count; i++)
                {
                    cbl_subgroupname.Items[i].Selected = true;
                }
                txt_subgroupname.Text = "Header Name(" + cbl_subgroupname.Items.Count + ")";
            }
        }
        else
        {
            ddl_subgroup.Items.Insert(0, "Select");

            txt_subgroupname.Text = "--Select--";
        }
    }


    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    /*14.09.15*/
    protected void btn_save_Click(object sender, EventArgs e)
    {
        DateTime dt = new DateTime();
        string dtaccessdate = DateTime.Now.ToString();
        string dtaccesstime = DateTime.Now.ToLongTimeString();
        string todaydate = Convert.ToString(txt_todaydate.Text);
        string amount = Convert.ToString(txt_amount.Text);
        string description = Convert.ToString(txt_popdesc.Text);

        description = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(description);
        string[] split = todaydate.Split('/');
        dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
        string group = Convert.ToString(ddl_group.SelectedItem.Value);

        if (txt_popdesc.Text.Trim() != "" && ddl_hostelname.SelectedItem.Value.Trim() != "0" && ddl_group.SelectedItem.Value.Trim() != "0")
        {
            string q = "INSERT INTO HT_HostelIncome(IncomeDate,IncomeGroup,IncomeAmount,IncomeDesc,HostelMasterFK)values('" + dt.ToString("MM/dd/yyyy") + "','" + group + "','" + amount + "','" + description + "','" + ddl_hostelname.SelectedItem.Value + "')";

            ds.Clear();
            int ins = d2.update_method_wo_parameter(q, "Text");
            if (ins != 0)
            {

                alertpopwindow.Visible = true;
                loadhostel();
                bindaddgroup();
                btn_go_Click(sender, e);

                ddl_hostelname.SelectedItem.Value = "0";
                ddl_group.SelectedItem.Value = "0";
                txt_popdesc.Text = "";
                txt_amount.Text = "";
                lblalerterr.Text = "Saved Successfully";
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select All Field";
            }

        }
        else
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = "Please Select All Field";
        }
    }

    protected void binddescription()
    {
        try
        {
            cbl_description.Items.Clear();
            ds.Clear();
            string groupname = "";

            for (int i = 0; i < cbl_groupname.Items.Count; i++)
            {
                if (cbl_groupname.Items[i].Selected == true)
                {
                    if (groupname == "")
                    {
                        groupname = "" + cbl_groupname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        groupname = groupname + "'" + "," + "" + "'" + cbl_groupname.Items[i].Value.ToString() + "";
                    }
                }
            }

            string sql = "select IncomeDesc from HT_HostelIncome where IncomeGroup in('" + groupname + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_description.DataSource = ds;
                cbl_description.DataTextField = "IncomeDesc";
                cbl_description.DataBind();
            }

            if (cbl_description.Items.Count > 0)
            {
                for (int i = 0; i < cbl_description.Items.Count; i++)
                {
                    cbl_description.Items[i].Selected = true;
                }

                txt_description.Text = "Description(" + cbl_description.Items.Count + ")";
            }


        }
        catch
        { }
    }

    protected void bindaddgroup()
    {
        try
        {
            ddl_group.Items.Clear();
            ds.Clear();
            string sql = "select distinct MasterCode,MasterValue  from CO_MasterValues where MasterCriteria ='HostelIncomeGRP' and CollegeCode ='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_group.DataSource = ds;
                ddl_group.DataTextField = "MasterValue";
                ddl_group.DataValueField = "MasterCode";
                ddl_group.DataBind();
                ddl_group.Items.Insert(0, new ListItem("Select", "0"));


                cbl_groupname.DataSource = ds;
                cbl_groupname.DataTextField = "MasterValue";
                cbl_groupname.DataValueField = "MasterCode";
                cbl_groupname.DataBind();

                if (cbl_groupname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_groupname.Items.Count; i++)
                    {
                        cbl_groupname.Items[i].Selected = true;
                    }

                    txt_groupname.Text = "Group Name(" + cbl_groupname.Items.Count + ")";
                }
                binddescription();
                if (cbl_description.Items.Count >= 0)
                {
                    for (int i = 0; i < cbl_description.Items.Count; i++)
                    {
                        cbl_description.Items[i].Selected = true;
                    }

                    txt_description.Text = "Description(" + cbl_description.Items.Count + ")";
                }

            }
            else
            {
                ddl_group.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch
        { }
    }
    protected void btnplus_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addgroup.Visible = true;
        lbl_addgroup.Text = "Add Group";
        lblerror.Visible = false;
    }
    protected void btnminus_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddl_group.SelectedIndex != 0)
            {
                string sql = "delete from CO_MasterValues where MasterCode='" + ddl_group.SelectedItem.Value.ToString() + "' and MasterCriteria='HostelIncomeGRP' and collegecode='" + collegecode1 + "'";
                int delete = d2.update_method_wo_parameter(sql, "Text");
                if (delete != 0)
                {

                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Deleted Successfully";
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Selected";
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Selected";
            }
            bindaddgroup();
        }
        catch { }
    }
    protected void btnplus1_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addgroup.Visible = true;
        lbl_addgroup.Text = "Add Sub Group";
    }
    protected void btnminus1_Click(object sender, EventArgs e)
    {

    }
    protected void btnplus2_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addgroup.Visible = true;
        lbl_addgroup.Text = "Add Description";
    }
    protected void btnminus2_Click(object sender, EventArgs e)
    {

    }
    protected void btn_addgroup_Click(object sender, EventArgs e)
    {
        try
        {
            string group = Convert.ToString(txt_addgroup.Text);
            group = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(group);
            if (lbl_addgroup.Text == "Add Group")
            {
                if (txt_addgroup.Text != "")
                {
                    string sql = "if exists ( select * from CO_MasterValues where MasterValue='" + group + "' and MasterCriteria='HostelIncomeGRP' and CollegeCode='" + collegecode1 + "') Update CO_MasterValues set MasterValue='" + group + "' where MasterValue='" + group + "' and MasterCriteria='HostelIncomeGRP' and CollegeCode='" + collegecode1 + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values('" + group + "','HostelIncomeGRP','" + collegecode1 + "')";
                    int insert = d2.update_method_wo_parameter(sql, "Text");
                    if (insert != 0)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Saved Successfully";
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
                    lblerror.Text = "Enter the Group Name";
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
    protected void cb_groupname_CheckedChange(object sender, EventArgs e)
    {
        if (cb_groupname.Checked == true)
        {
            for (int i = 0; i < cbl_groupname.Items.Count; i++)
            {
                cbl_groupname.Items[i].Selected = true;
            }
            txt_groupname.Text = "Group Name(" + (cbl_groupname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_groupname.Items.Count; i++)
            {
                cbl_groupname.Items[i].Selected = false;
            }
            txt_groupname.Text = "--Select--";
        }

        cb_description_CheckedChange(sender, e);
        cbl_groupname_SelectedIndexChange(sender, e);
        binddescription();

    }
    protected void cbl_groupname_SelectedIndexChange(object sender, EventArgs e)
    {
        txt_groupname.Text = "--Select--";
        cb_groupname.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_groupname.Items.Count; i++)
        {
            if (cbl_groupname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txt_groupname.Text = "Group Name(" + commcount.ToString() + ")";
            if (commcount == cbl_groupname.Items.Count)
            {
                cb_groupname.Checked = true;
            }
        }
        cb_description_CheckedChange(sender, e);
        cbl_description_SelectedIndexChange(sender, e);
        binddescription();

    }
    protected void cb_subgroup_CheckedChange(object sender, EventArgs e)
    {
        if (cb_subgroupname.Checked == true)
        {
            for (int i = 0; i < cbl_subgroupname.Items.Count; i++)
            {
                cbl_subgroupname.Items[i].Selected = true;
            }
            txt_subgroupname.Text = "Sub Group(" + (cbl_subgroupname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_subgroupname.Items.Count; i++)
            {
                cbl_subgroupname.Items[i].Selected = false;
            }
            txt_subgroupname.Text = "--Select--";
        }
    }
    protected void cbl_subgroup_SelectedIndexChange(object sender, EventArgs e)
    {
        txt_subgroupname.Text = "--Select--";
        cb_subgroupname.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_subgroupname.Items.Count; i++)
        {
            if (cbl_subgroupname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txt_subgroupname.Text = "Sub Group(" + commcount.ToString() + ")";
            if (commcount == cbl_subgroupname.Items.Count)
            {
                cb_subgroupname.Checked = true;
            }
        }
    }
    protected void cb_description_CheckedChange(object sender, EventArgs e)
    {
        if (cb_description.Checked == true)
        {
            for (int i = 0; i < cbl_description.Items.Count; i++)
            {
                cbl_description.Items[i].Selected = true;
            }
            txt_description.Text = "Description(" + (cbl_description.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_description.Items.Count; i++)
            {
                cbl_description.Items[i].Selected = false;
            }
            txt_description.Text = "--Select--";

        }
    }
    protected void cbl_description_SelectedIndexChange(object sender, EventArgs e)
    {
        txt_description.Text = "--Select--";
        cb_description.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_description.Items.Count; i++)
        {
            if (cbl_description.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txt_description.Text = "Description(" + commcount.ToString() + ")";
            if (commcount == cbl_description.Items.Count)
            {
                cb_description.Checked = true;
            }
        }
    }

    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
        btn_go_Click(sender, e);
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        alertpopwindow.Visible = false;
        popwindow.Visible = true;
    }

    protected void delete()
    {
        try
        {
            surediv.Visible = false;
            string groupname = Convert.ToString(ddl_group.SelectedItem.Value);
            string description = Convert.ToString(txt_popdesc.Text);
            string date = Convert.ToString(txt_todaydate.Text);
            string[] split = date.Split('/');
            DateTime dt = new DateTime();
            if (split.Length > 2)
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            string delquery = "delete from HT_HostelIncome where IncomeGroup ='" + groupname + "' and IncomeDesc='" + description + "' and HostelMasterFK='" + ddl_hostelname.SelectedItem.Value + "' and incomedate='" + dt.ToString("MM/dd/yyyy") + "'";
            int ins = d2.update_method_wo_parameter(delquery, "Text");
            if (ins != 0)
            {
                popwindow.Visible = false;
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Deleted Successfully";
                lblalerterr.Visible = true;
            }
        }
        catch
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
                lbl_sure.Text = "Do you want to Delete this Record?";
            }
        }
        catch
        {

        }
    }

    protected void txt_fromdate_Textchanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_fromdate.Text != "" && txt_todate.Text != "")
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_todate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;

                if (dt > dt1)
                {
                    alertpopwindow.Visible = true;

                    lblalerterr.Text = "Enter FromDate less than or equal to the ToDate";
                    txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");

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
    protected void txt_todate_Textchanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_todate.Text != "" && txt_fromdate.Text != "")
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_todate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;

                if (dt > dt1)
                {
                    alertpopwindow.Visible = true;

                    lblalerterr.Text = "Enter ToDate greater than or equal to the FromDate";
                    txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");

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

    public void rdb_datewise_CheckedChanged(object sender, EventArgs e)
    {
        txt_fromdate.Enabled = true;
        txt_todate.Enabled = true;

    }
    public void rdb_totalwise_CheckedChanged(object sender, EventArgs e)
    {
        txt_fromdate.Enabled = false;
        txt_todate.Enabled = false;

    }
    protected void txt_amount_textchanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToDouble(txt_amount.Text) > 0)
            {

            }
            else
            {
                txt_amount.Text = "";
            }
        }
        catch
        {
        }
    }
}