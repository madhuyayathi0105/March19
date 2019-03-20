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
public partial class HM_Expanses : System.Web.UI.Page
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
    DataTable dt = new DataTable();
    DataTable dt2 = new DataTable();
    DataRow dr;
    static string checknew = "";
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
            ViewState["selecteditems"] = null;
            txt_todaydate.Attributes.Add("readonly", "readonly");
            txt_todaydate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Attributes.Add("readonly", "readonly");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //mess
            txt_messitemname.Attributes.Add("readonly", "readonly");
            txt_messtodate.Attributes.Add("readonly", "readonly");
            txt_messfromdate.Attributes.Add("readonly", "readonly");
            txt_messtodate1.Attributes.Add("readonly", "readonly");
            txt_messtodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_messfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_messtodate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
            rdb_datewise.Checked = true;
            txt_searchby.Visible = true;
            txt_messitem1.Visible = true;
            BindStudentType();
            rdb_commonwise.Checked = true;
            rdb_commonwise_Click(sender, e);
            loadhostel();
            bindaddgroup();
            bindsubgroup();
            binddescription();
            bindmess();
            itemheadername();
            loadsub();
            loaditem();
            Session["dt"] = null;
            //bindgroup();
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread1.Visible = false;
            rdb_pophos.Checked = true;
            btn_go_Click(sender, e);
            bindmessmaster();
        }
        lbl_error.Visible = false;
    }
    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        //rdb_veg.Enabled = true;
        //rdb_veg.Checked = true;
        //rdb_NonVeg.Enabled = true;
        ddlStudType.SelectedIndex = 0;
        Printcontrol.Visible = false;
        popwindow.Visible = true;
        ddl_hostelname.Enabled = true;
        ddl_group.Enabled = true;
        ddl_subgroup.Enabled = true;
        ddl_descrip.Enabled = true;
        txt_todaydate.Enabled = true;
        btn_update.Visible = false;
        btn_plus.Enabled = true;
        btn_plus1.Enabled = true;
        btn_plus2.Enabled = true;
        btn_minus.Enabled = true;
        btn_minus1.Enabled = true;
        btn_minus2.Enabled = true;
        rdb_pophos.Enabled = true;
        rdb_popmess.Enabled = true;
        txt_amount.Text = "";
        loadhostel();
        bindaddgroup();
        bindsubgroup();
        binddescription();
        btn_save.Visible = true;
        btn_delete.Visible = false;
        ddl_hostelname.SelectedItem.Value = "0";
        ddl_group.SelectedItem.Value = "0";
        ddl_descrip.SelectedItem.Value = "0";
        ddl_subgroup.SelectedItem.Value = "0";
        txt_todaydate.Text = DateTime.Now.ToString("dd/MM/yyyy");
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
        bindmessmaster1();
        //loadsession();
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
        bindmessmaster1();
    }
    //magseh 2.7.18
    protected void Chkmess_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkmess.Checked == true)
        {
            for (int i = 0; i < Cblmess.Items.Count; i++)
            {
                Cblmess.Items[i].Selected = true;
            }
            txtmess.Text = "Hostel Name(" + (Cblmess.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < Cblmess.Items.Count; i++)
            {
                Cblmess.Items[i].Selected = false;
            }
            txtmess.Text = "--Select--";
        }
       
        //loadsession();
    }
    protected void Cblmess_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtmess.Text = "--Select--";
        Chkmess.Checked = false;
        int commcount = 0;
        for (int i = 0; i < Cblmess.Items.Count; i++)
        {
            if (Cblmess.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtmess.Text = "Hostel Name(" + commcount.ToString() + ")";
            if (commcount == Cblmess.Items.Count)
            {
                Chkmess.Checked = true;
            }
        }
   
    }
    

    public void loadhostel()
    {
        try
        {
            //magesh 2.7.18
            //ds.Clear();
            //cbl_hostelname.Items.Clear();
            ////string selecthostel = "select HostelMasterPK,HostelName from HM_HostelMaster order by HostelMasterPK";//where 
            ////ds = d2.select_method_wo_parameter(selecthostel, "Text");
            //string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            //ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);  //magesh 2.7.18
            ds.Clear();
            cbl_hostelname.Items.Clear();
            string MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Rights' and user_code='" + usercode + "'");
           // ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            //magesh 21.6.18
            MessmasterFK = "select HostelMasterPK,HostelName from HM_HostelMaster where HostelMasterPK in(" + MessmasterFK + ")  order by hostelname ";
            ds = d2.select_method_wo_parameter(MessmasterFK, "text");
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
                      //  cbl_hostelname.Items[i].Selected = true;
                    }
                    txt_hostelname.Text = "Hostel Name(" + cbl_hostelname.Items.Count + ")";
                }
            }
            else
            {
               // ddl_hostelname.Items.Insert(0, "Select");
                //ddlpophostelname.Items.Insert(ddlpophostelname.Items.Count, "Others");
                txt_hostelname.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void ddl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindmessmaster();
    }
    public void bindmessmaster1()
    {
        try
        {
            string typ1 = string.Empty;
            if (cbl_hostelname.Items.Count > 0)
            {
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cbl_hostelname.Items[i].Selected == true)
                    {
                        if (typ1 == "")
                        {
                            typ1 = "" + cbl_hostelname.Items[i].Value + "";
                        }
                        else
                        {
                            typ1 = typ1 + "'" + "," + "'" + cbl_hostelname.Items[i].Value + "";
                        }
                    }
                   
                }
            }
            string selectQuery = "select MessMasterFK1 from HM_HostelMaster where HostelMasterPK in('" + typ1 + "')";
            //string selectQuery1 =d2.GetFunction("select MessMasterPK from HM_MessMaster where MessMasterPK in(" + selectQuery + ") order by MessMasterPK asc");
            string typ = string.Empty;
             ds = d2.select_method_wo_parameter(selectQuery, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for(int j=0;j<ds.Tables[0].Rows.Count;j++)
                {
                    string jm = Convert.ToString(ds.Tables[0].Rows[j]["MessMasterFK1"]);
                    string[] spl = jm.Split('-');
            if (spl.Length > 0)
            {

                if (spl.Count() > 0)
                {
                    for (int i = 0; i < spl.Count(); i++)
                    {
                        if (typ == "")
                        {
                            typ = "" + spl[i] + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + spl[i] + "";
                        }

                    }

                }
            }
            }
                selectQuery = ("select MessMasterPK,MessName from HM_MessMaster where MessMasterPK in('" + typ + "') order by MessMasterPK asc");
            }

            ds = d2.select_method_wo_parameter(selectQuery, "text");
            // ddl_messmaster.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                //magesh 20.6.18


                Cblmess.DataSource = ds;
                Cblmess.DataTextField = "MessName";
                Cblmess.DataValueField = "MessMasterPK";
                Cblmess.DataBind();
            }
            else
            {
              //  Cblmess.Items.Insert(0, "");
            }
            //    ddl_messmaster.DataSource = ds;
            //    ddl_messmaster.DataTextField = "MessName";
            //    ddl_messmaster.DataValueField = "MessMasterPK";
            //    ddl_messmaster.DataBind();
            //}
            //ddl_messmaster.Items.Insert(0, "Select");
        }
        catch
        {
            //ddl_messmaster.Items.Clear();
        }
    }
    public void bindmessmaster()
    {
        try
        {
            string selectQuery = d2.GetFunction("select MessMasterFK1 from HM_HostelMaster where HostelMasterPK='" + ddl_hostelname.SelectedValue + "'");
            //string selectQuery1 =d2.GetFunction("select MessMasterPK from HM_MessMaster where MessMasterPK in(" + selectQuery + ") order by MessMasterPK asc");
            string[] spl = selectQuery.Split('-');
            if (spl.Length > 0)
            {
                string typ = string.Empty;
                if (spl.Count() > 0)
                {
                    for (int i = 0; i < spl.Count(); i++)
                    {
                        if (typ == "")
                        {
                            typ = "" + spl[i] + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + spl[i] + "";
                        }

                    }

                }
                selectQuery = ("select MessMasterPK,MessName from HM_MessMaster where MessMasterPK in('" + typ + "') order by MessMasterPK asc");
            }

            ds = d2.select_method_wo_parameter(selectQuery, "text");
            // ddl_messmaster.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                //magesh 20.6.18
                ddlmess.DataSource = ds;
                ddlmess.DataTextField = "MessName";
                ddlmess.DataValueField = "MessMasterPK";
                ddlmess.DataBind();
            }
            else
            {
               // ddlmess.Items.Insert(0, "");
            }
            //    ddl_messmaster.DataSource = ds;
            //    ddl_messmaster.DataTextField = "MessName";
            //    ddl_messmaster.DataValueField = "MessMasterPK";
            //    ddl_messmaster.DataBind();
            //}
            //ddl_messmaster.Items.Insert(0, "Select");
        }
        catch
        {
            //ddl_messmaster.Items.Clear();
        }
    }
    //magesh 2.7.18
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
            //theivamani 5.11.15
            string itemheadercode = "";
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                }
            }


             string itemheadercode1 = "";
             for (int i = 0; i < Cblmess.Items.Count; i++)
            {
                if (Cblmess.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode1 = "" + Cblmess.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode1 = itemheadercode1 + "'" + "," + "'" + Cblmess.Items[i].Value.ToString() + "";
                    }
                }
            }
            //ArrayList colnamess = new ArrayList();
            //colnamess.Clear();
            //colnamess.Add("Company Code");
            //int colinc = 0;
            //colnamess.Add("Company Name");
            //colnamess.Add("Person Name");
            //colnamess.Add("Email ID");
            //colnamess.Add("Location");
            //colnamess.Add("Type of company");
            //colnamess.Add("Type of Interview");
            //colnamess.Add("Interview Date");
            //colnamess.Add("From Date");
            //colnamess.Add("To Date");
            //colnamess.Add("Student Required");
            //colnamess.Add("Designation");
            //colnamess.Add("Phone Number");
            //colnamess.Add("Mobile No");
            //if (itemheadercode.Trim() != "")
            //{
            //    Fpspread1.Sheets[0].RowCount = 0;
            //    Fpspread1.Sheets[0].ColumnCount = 0;
            //    Fpspread1.CommandBar.Visible = false;
            //    Fpspread1.Sheets[0].AutoPostBack = true;
            //    Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
            //    Fpspread1.Sheets[0].RowHeader.Visible = false;
            //    Fpspread1.Sheets[0].ColumnCount = Itemindex.Count + 1;
            //    Fpspread1.Sheets[0].RowCount = ds.Tables[0].Rows.Count;
            //    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            //    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            //    darkstyle.ForeColor = Color.White;
            //    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            //    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            //    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            //    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            //    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            //    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            //    Fpspread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            //    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
            //    {
            //        string colno = ds.Tables[0].Columns[j].ToString().Trim();
            //        if (Itemindex.Contains(Convert.ToString(colno)))
            //        {
            //            int insdex = Itemindex.IndexOf(Convert.ToString(colno));
            //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 1].Text = colnamess[Convert.ToInt32(ds.Tables[0].Columns[j].ToString())].ToString();
            //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 1].Font.Bold = true;
            //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 1].Font.Name = "Book Antiqua";
            //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 1].Font.Size = FontUnit.Medium;
            //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 1].HorizontalAlign = HorizontalAlign.Center;
            //        }
            //    }
            //    colinc = 0;
            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
            //        {
            //            if (Itemindex.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
            //            {
            //                int insdex = Itemindex.IndexOf(Convert.ToString(ds.Tables[0].Columns[j].ToString()));
            //                Fpspread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
            //                Fpspread1.Sheets[0].Cells[i, insdex + 1].Text = ds.Tables[0].Rows[i][j].ToString();
            //                Fpspread1.Sheets[0].Cells[i, insdex + 1].Font.Bold = true;
            //                Fpspread1.Sheets[0].Cells[i, insdex + 1].Font.Name = "Book Antiqua";
            //                Fpspread1.Sheets[0].Cells[i, insdex + 1].Font.Size = FontUnit.Medium;
            //            }
            //        }
            //    }
            //    Fpspread1.Visible = true;
            //    div1.Visible = true;
            //    lbl_error.Visible = false;
            //    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
            //    if (Fpspread1.Sheets[0].ColumnCount == 1)
            //    {
            //        Fpspread1.Visible = false;
            //        div1.Visible = false;
            //        lbl_error.Visible = true;
            //        lbl_error.Text = "No Records Found";
            //    }
            //}
            //else
            //{
            //    Fpspread1.Visible = false;
            //    div1.Visible = false;
            //    lbl_error.Visible = true;
            //    lbl_error.Text = "No Records Found";
            //}
            //16.10.15
            Printcontrol.Visible = false;
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
                        group = group + "'" + "," + "'" + cbl_groupname.Items[i].Value.ToString() + "";
                    }
                }
            }
            string subgroup = "";
            for (int i = 0; i < cbl_subgroupname.Items.Count; i++)
            {
                if (cbl_subgroupname.Items[i].Selected == true)
                {
                    if (subgroup == "")
                    {
                        subgroup = "" + cbl_subgroupname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        subgroup = subgroup + "'" + "," + "'" + cbl_subgroupname.Items[i].Value.ToString() + "";
                    }
                }
            }
            string description = "";
            for (int i = 0; i < cbl_description.Items.Count; i++)
            {
                if (cbl_description.Items[i].Selected == true)
                {
                    if (description == "")
                    {
                        description = "" + cbl_description.Items[i].Text.ToString() + "";
                    }
                    else
                    {
                        description = description + "'" + "," + "'" + cbl_description.Items[i].Text.ToString() + "";
                    }
                }
            }
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
                string q1 = "";
                if (rdb_datewise.Checked == true)
                {
                    q1 = "select CONVERT(varchar(10), ExpensesDate,103) as ExpensesDate,M.HostelName,h.ExpSubGroup,ms.MessMasterPK,h.ExpGroup,h.HostelFK, (select MasterValue from CO_MasterValues where MasterCode = ISNULL( ExpGroup,0)) as MainGroup_Desc_txt,(select MasterValue from CO_MasterValues where MasterCode = ISNULL( h.ExpSubGroup,0))as Sub_Group_txt ,h.ExpAmount,h.ExpDesc ,case when ExpensesType='0' then 'Common' else (select StudentTypeName from HostelStudentType where StudentType=ExpensesType) end as ExpensesType,HostelExpensePk,ms.Messname  from HT_HostelExpenses h,HM_HostelMaster M,HM_MessMaster ms where h.HostelFK =m.HostelMasterPK and m.HostelMasterPK in ('" + itemheadercode + "') and h.ExpGroup in ('" + group + "') and h.ExpSubGroup in ('" + subgroup + "') and h.Messname in ('" + itemheadercode1 + "') and h.ExpDesc in ('" + description + "') and h.ExpensesDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ms.MessMasterPK=h.Messname order by h.ExpensesDate  ";
                }
                else
                {
                    lbl_error1.Visible = false;
                    q1 = "select CONVERT(varchar(10), ExpensesDate,103) as ExpensesDate,M.HostelName,h.HostelExpensePk,ms.MessMasterPK,h.ExpSubGroup,h.ExpGroup,(select MasterValue from CO_MasterValues where MasterCode = ISNULL( ExpGroup,0)) as MainGroup_Desc_txt,(select MasterValue from CO_MasterValues where MasterCode = ISNULL( h.ExpSubGroup,0))as Sub_Group_txt ,h.ExpAmount,h.ExpDesc,case when ExpensesType='0' then 'Common' else (select StudentTypeName  from HostelStudentType where StudentType=ExpensesType) end as ExpensesType,HostelFK ,HostelExpensePk,ms.Messname from HT_HostelExpenses h,HM_HostelMaster M,HM_MessMaster ms where h.HostelFK =m.HostelMasterPK and m.HostelMasterPK in ('" + itemheadercode + "') and h.ExpGroup in ('" + group + "') and h.ExpSubGroup in ('" + subgroup + "')  and h.Messname in ('" + itemheadercode1 + "') and h.ExpDesc in ('" + description + "') and ms.MessMasterPK=h.Messname  order by h.ExpensesDate  ";
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
                    Fpspread2.Sheets[0].ColumnCount = 9;
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
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Mess Name";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread2.Columns[2].Width = 200;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Group Name";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread2.Columns[3].Width = 100;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Sub Group Name";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpspread2.Columns[4].Width = 100;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Description";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    Fpspread2.Columns[5].Width = 200;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Amount";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    Fpspread2.Columns[6].Width = 150;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Date";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    Fpspread2.Columns[6].Width = 100;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Expenses Type";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                    Fpspread2.Columns[7].Width = 100;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["ExpensesDate"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["HostelName"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["HostelFK"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Messname"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["MessMasterPK"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["MainGroup_Desc_txt"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["ExpGroup"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Sub_Group_txt"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["ExpSubGroup"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["ExpDesc"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[i]["HostelExpensePk"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["ExpAmount"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["ExpensesDate"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["ExpensesType"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
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
                div2.Visible = false;
                rptprint.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select All Field";
            }
        }
        catch
        {
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
                bindsubgroup();
               
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
                    string todaydate = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                    string hostelname = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    string messname = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                    string messcode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                    string hostelnamecode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                    string groupname = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow),3].Text);
                    string groupnamecode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag);
                    string subgroupname = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                    string subgroupnamecode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag);
                    string description = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
                    //string descriptioncode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text);
                    string amount = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text);
                    string HostelExpensePk = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Tag);
                    Session["HostelExpensePk"] = Convert.ToString(HostelExpensePk);
                    ddl_hostelname.SelectedIndex = ddl_hostelname.Items.IndexOf(ddl_hostelname.Items.FindByValue(hostelnamecode));
                    bindmessmaster();
                    ddlmess.SelectedIndex = ddlmess.Items.IndexOf(ddlmess.Items.FindByValue(messcode));
                    ddl_group.SelectedIndex = ddl_group.Items.IndexOf(ddl_group.Items.FindByValue(groupnamecode));
                    ddl_subgroup.SelectedIndex = ddl_subgroup.Items.IndexOf(ddl_subgroup.Items.FindByValue(subgroupnamecode));//delsi
                    ddl_descrip.SelectedIndex = ddl_descrip.Items.IndexOf(ddl_descrip.Items.FindByText(description));
                    string type = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 8].Text);

                    ddlStudType.SelectedIndex = ddlStudType.Items.IndexOf(ddlStudType.Items.FindByText(type));

                    //if (type == "Veg")
                    //{
                    //    rdb_veg.Checked = true;
                    //    rdb_NonVeg.Checked = false;
                    //    rdb_NonVeg.Enabled = false;
                    //}
                    //if (type == "Non Veg")
                    //{
                    //    rdb_NonVeg.Checked = true;
                    //    rdb_veg.Checked = false;
                    //    rdb_veg.Enabled = false;
                    //}
                    //ddl_hostelname.SelectedItem.Text = Convert.ToString(hostelname);
                    //ddl_group.SelectedItem.Text = Convert.ToString(groupname);
                    //ddl_group.SelectedItem.Value = Convert.ToString(groupnamecode);
                    //ddl_subgroup.SelectedItem.Text = Convert.ToString(subgroupname);
                    //ddl_subgroup.SelectedItem.Value = Convert.ToString(subgroupnamecode);
                    //txt_popdesc.Text = Convert.ToString(description);
                    txt_amount.Text = Convert.ToString(amount);
                    txt_todaydate.Text = Convert.ToString(todaydate);
                    ddl_hostelname.Enabled = false;
                    ddl_group.Enabled = false;
                    ddl_subgroup.Enabled = false;
                    ddl_descrip.Enabled = false;
                    txt_todaydate.Enabled = false;
                    btn_plus.Enabled = false;
                    btn_plus1.Enabled = false;
                    btn_minus.Enabled = false;
                    btn_minus1.Enabled = false;
                    rdb_pophos.Enabled = false;
                    rdb_popmess.Enabled = false;
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
            string hostelnamecode = Convert.ToString(ddl_hostelname.SelectedItem.Value);
            string groupname = Convert.ToString(ddl_group.SelectedItem.Value);
            string subgroup = Convert.ToString(ddl_subgroup.SelectedItem.Value);
            string description = Convert.ToString(ddl_descrip.SelectedItem.Text);
            string amount = Convert.ToString(txt_amount.Text);
            string date = Convert.ToString(txt_todaydate.Text);
            string[] split = date.Split('/');
            DateTime dt = new DateTime();
            if (split.Length > 2)
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            if (groupname.Trim() != "")
            {
                string expansetype = Convert.ToString(ddlStudType.SelectedItem.Value);
                //if (rdb_veg.Checked == true)
                //{
                //    expansetype = "1";
                //}
                //else if (rdb_NonVeg.Checked == true)
                //{
                //    expansetype = "2";
                //}
                string inserquery = " if exists(select * from HT_HostelExpenses  where ExpensesType='" + expansetype + "' and ExpensesDate='" + dt.ToString("MM/dd/yyyy") + "' and ExpGroup='" + groupname + "' and ExpSubGroup='" + subgroup + "' and expdesc='" + description + "' and hostelFk ='" + hostelnamecode + "'  and HostelExpensePk ='" + Convert.ToString(Session["HostelExpensePk"]) + "') update HT_HostelExpenses set ExpAmount='" + amount + "'  where ExpensesType='" + expansetype + "' and ExpensesDate='" + dt.ToString("MM/dd/yyyy") + "' and ExpGroup='" + groupname + "' and ExpSubGroup='" + subgroup + "' and expdesc='" + description + "' and hostelFk ='" + hostelnamecode + "' and HostelExpensePk ='" + Convert.ToString(Session["HostelExpensePk"]) + "' ";
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
            string degreedetails = "Hostel Expenses Report";
            string pagename = "HM_Expanses.aspx";
            Printcontrol.loadspreaddetails(Fpspread2, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }
    protected void btnExcel2_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname2.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread3, reportname);
                lblvalidation2.Visible = false;
            }
            else
            {
                lblvalidation2.Text = "Please Enter Your Report Name";
                lblvalidation2.Visible = true;
                txtexcelname2.Focus();
            }
        }
        catch
        {
        }
    }
    protected void btnprintmaster2_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Hostel Expenses Report";
            string pagename = "HM_Expanses.aspx";
            Printcontrol2.loadspreaddetails(Fpspread3, pagename, degreedetails);
            Printcontrol2.Visible = true;
        }
        catch
        {
        }
    }
    protected void txt_fromdate_Textchanged(object sender, EventArgs e)
    {
        try
        {
            lbl_error1.Visible = false;
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            if (fromdate != "" && fromdate != null && todate != "" && todate != null)
            {
                string[] spiltfrom = fromdate.Split('/');
                string[] spitto = todate.Split('/');
                DateTime from = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
                DateTime to = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);
                if (from > to)
                {
                    lbl_error1.Visible = true;
                    lbl_error1.Text = "Please Enter To Date Greater Than From Date";
                    Fpspread2.Visible = false;
                    div2.Visible = false;
                    btn_save.Visible = false;
                    rptprint.Visible = false;
                    btn_update.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lbl_error1.Visible = true;
            lbl_error1.Text = ex.ToString();
        }
    }
    protected void txt_todate_Textchanged(object sender, EventArgs e)
    {
        try
        {
            lbl_error1.Visible = false;
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            if (fromdate != "" && fromdate != null && todate != "" && todate != null)
            {
                string[] spiltfrom = fromdate.Split('/');
                string[] spitto = todate.Split('/');
                DateTime from = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
                DateTime to = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);
                if (from > to)
                {
                    lbl_error1.Visible = true;
                    lbl_error1.Text = "Please Enter To Date Grater Than From Date";
                    Fpspread2.Visible = false;
                    div2.Visible = false;
                    rptprint.Visible = false;
                    btn_save.Visible = false;
                    btn_update.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = ex.ToString();
        }
    }
    protected void btnplus_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addgroup.Visible = true;
        lbl_addgroup.Text = "Add Group";
        lblerror.Visible = false;
    }
    protected void btn_suredel_Click(object sender, EventArgs e)
    {
        try
        {
            surediv_del.Visible = false;
            if (ddl_group.SelectedIndex != 0)
            {
                //string sql = "delete from TextValTable where TextCode='" + ddl_group.SelectedItem.Value.ToString() + "' and TextCriteria='HEGrp' and college_code='" + collegecode1 + "' ";
                string sql = "delete from CO_MasterValues where MasterCode='" + ddl_group.SelectedItem.Value.ToString() + "' and MasterCriteria='HostelExpGrp' and collegecode='" + collegecode1 + "' ";
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
    protected void btn_delno_Click(object sender, EventArgs e)
    {
        surediv_del.Visible = false;
    }
    protected void btnminus_Click(object sender, EventArgs e)
    {
        surediv_del.Visible = true;
        lbl_suredel.Text = "Do You Want to Delete This Record?";
    }
    protected void btnplus1_Click(object sender, EventArgs e)
    {
        //lblerror.Visible = true;
        plusdiv.Visible = true;
        panel_addgroup.Visible = true;
        lbl_addgroup.Text = "Add Sub Group";
        lblerror.Visible = false;
    }
    protected void btnminus1_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddl_subgroup.SelectedIndex != 0)
            {
                //string sql = "delete from TextValTable where    ='" + ddl_subgroup.SelectedItem.Value.ToString() + "' and TextCriteria='HESGr' and college_code='" + collegecode1 + "' ";
                string sql = " delete from CO_MasterValues where   MasterCode ='" + ddl_subgroup.SelectedItem.Value.ToString() + "' and MasterCriteria='HostelExpSubGrp' and collegecode='" + collegecode1 + "' ";
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
            bindsubgroup();
        }
        catch
        { }
    }
    protected void btnplus2_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addgroup.Visible = true;
        lbl_addgroup.Text = "Add Description";
    }
    protected void btnminus2_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddl_descrip.SelectedIndex != 0)
            {
                //string sql = "delete from TextValTable where    ='" + ddl_subgroup.SelectedItem.Value.ToString() + "' and TextCriteria='HESGr' and college_code='" + collegecode1 + "' ";
                string sql = " delete from CO_MasterValues where   MasterCode ='" + ddl_descrip.SelectedItem.Value.ToString() + "' and MasterCriteria='expdesc' and collegecode='" + collegecode1 + "' ";
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
            binddescription();
        }
        catch
        { }
    }
    protected void btn_addgroup_Click(object sender, EventArgs e)
    {
        try
        {
            //theivamani 7.11.15
            string group = Convert.ToString(txt_addgroup.Text);
            group = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(group);
            string subgroup = Convert.ToString(txt_addgroup.Text);
            subgroup = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(subgroup);
            string Description = Convert.ToString(txt_addgroup.Text);
            subgroup = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Description);
            if (lbl_addgroup.Text == "Add Group")
            {
                if (txt_addgroup.Text != "")
                {
                    // string sql = "if exists ( select * from TextValTable where TextVal ='" + group + "' and TextCriteria ='HEGrp' and college_code ='" + collegecode1 + "') update TextValTable set TextVal ='" + group + "' where TextVal ='" + group + "' and TextCriteria ='HEGrp' and college_code ='" + collegecode1 + "' else insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + group + "','HEGrp','" + collegecode1 + "')";
                    string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + group + "' and MasterCriteria ='HostelExpGrp' and collegecode ='" + collegecode1 + "') update CO_MasterValues set MasterValue ='" + group + "' where MasterValue ='" + group + "' and MasterCriteria ='HostelExpGrp' and collegecode ='" + collegecode1 + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,collegecode) values ('" + group + "','HostelExpGrp','" + collegecode1 + "')";
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
                    //lblalerterr.Text = "Enter the Group Name";
                }
            }
            else if (lbl_addgroup.Text == "Add Sub Group")
            {
                if (txt_addgroup.Text != "")
                {
                    // string sql = "if exists ( select * from TextValTable where TextVal ='" + subgroup + "' and TextCriteria ='HESGr' and college_code ='" + collegecode1 + "') update TextValTable set TextVal ='" + subgroup + "' where TextVal ='" + subgroup + "' and TextCriteria ='HESGr' and college_code ='" + collegecode1 + "' else insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + subgroup + "','HESGr','" + collegecode1 + "')";
                    string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + subgroup + "' and MasterCriteria ='HostelExpSubGrp' and collegecode ='" + collegecode1 + "') update CO_MasterValues set MasterValue ='" + subgroup + "' where MasterValue ='" + subgroup + "' and MasterCriteria ='HostelExpSubGrp' and collegecode ='" + collegecode1 + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,collegecode) values ('" + subgroup + "','HostelExpSubGrp','" + collegecode1 + "')";
                    int insert = d2.update_method_wo_parameter(sql, "Text");
                    if (insert != 0)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Saved Successfully";
                        txt_addgroup.Text = "";
                        plusdiv.Visible = false;
                        panel_addgroup.Visible = false;
                    }
                    bindsubgroup();
                    txt_addgroup.Text = "";
                }
                else
                {
                    plusdiv.Visible = true;
                    lblerror.Visible = true;
                    lblerror.Text = "Enter the Sub Group";
                }
            }
            else if (lbl_addgroup.Text == "Add Description")
            {
                if (txt_addgroup.Text != "")
                {
                    // string sql = "if exists ( select * from TextValTable where TextVal ='" + subgroup + "' and TextCriteria ='HESGr' and college_code ='" + collegecode1 + "') update TextValTable set TextVal ='" + subgroup + "' where TextVal ='" + subgroup + "' and TextCriteria ='HESGr' and college_code ='" + collegecode1 + "' else insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + subgroup + "','HESGr','" + collegecode1 + "')";
                    string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + Description + "' and MasterCriteria ='expdesc' and collegecode ='" + collegecode1 + "') update CO_MasterValues set MasterValue ='" + Description + "' where MasterValue ='" + Description + "' and MasterCriteria ='expdesc' and collegecode ='" + collegecode1 + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,collegecode) values ('" + Description + "','expdesc','" + collegecode1 + "')";
                    int insert = d2.update_method_wo_parameter(sql, "Text");
                    if (insert != 0)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Saved Successfully";
                        txt_addgroup.Text = "";
                        plusdiv.Visible = false;
                        panel_addgroup.Visible = false;
                    }
                    binddescription();
                    txt_addgroup.Text = "";
                }
                else
                {
                    plusdiv.Visible = true;
                    lblerror.Visible = true;
                    lblerror.Text = "Enter the Description";
                }
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
            string sql = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='HostelExpGrp' and collegecode ='" + collegecode1 + "'";
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
            }
            else
            {
                ddl_group.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch
        { }
    }
    protected void bindsubgroup()
    {
        try
        {
            ddl_subgroup.Items.Clear();
            ds.Clear();
            //string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='HESGr' and college_code ='" + collegecode1 + "'";
            string sql = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='HostelExpSubGrp' and collegecode ='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_subgroup.DataSource = ds;
                ddl_subgroup.DataTextField = "MasterValue";
                ddl_subgroup.DataValueField = "MasterCode";
                ddl_subgroup.DataBind();
                ddl_subgroup.Items.Insert(0, new ListItem("Select", "0"));
                cbl_subgroupname.DataSource = ds;
                cbl_subgroupname.DataTextField = "MasterValue";
                cbl_subgroupname.DataValueField = "MasterCode";
                cbl_subgroupname.DataBind();
                if (cbl_subgroupname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_subgroupname.Items.Count; i++)
                    {
                        cbl_subgroupname.Items[i].Selected = true;
                    }
                    txt_subgroupname.Text = "Sub Group(" + cbl_subgroupname.Items.Count + ")";
                }
            }
            else
            {
                ddl_subgroup.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch { }
    }
    protected void binddescription()
    {
        try
        {
            //ds.Clear();
            //string sql = "select ExpDesc from HT_HostelExpenses";
            //ds = d2.select_method_wo_parameter(sql, "TEXT");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    cbl_description.DataSource = ds;
            //    cbl_description.DataTextField = "ExpDesc";
            //    //cbl_description.DataValueField = "TextCode";
            //    cbl_description.DataBind();
            //}
            //if (cbl_description.Items.Count > 0)
            //{
            //    for (int i = 0; i < cbl_description.Items.Count; i++)
            //    {
            //        cbl_description.Items[i].Selected = true;
            //    }
            //    txt_description.Text = "Description(" + cbl_description.Items.Count + ")";
            //}
            //ddl_subgroup.Items.Clear();
            ds.Clear();
            //string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='HESGr' and college_code ='" + collegecode1 + "'";
            string sql = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='expdesc' and collegecode ='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_descrip.DataSource = ds;
                ddl_descrip.DataTextField = "MasterValue";
                ddl_descrip.DataValueField = "MasterCode";
                ddl_descrip.DataBind();
                ddl_descrip.Items.Insert(0, new ListItem("Select", "0"));
                cbl_description.DataSource = ds;
                cbl_description.DataTextField = "MasterValue";
                cbl_description.DataValueField = "MasterCode";
                cbl_description.DataBind();
                if (cbl_description.Items.Count > 0)
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
                ddl_descrip.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch
        { }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        DateTime dt = new DateTime();
        string dtaccessdate = DateTime.Now.ToString();
        string dtaccesstime = DateTime.Now.ToLongTimeString();
        string todaydate = Convert.ToString(txt_todaydate.Text);
        string amount = Convert.ToString(txt_amount.Text);
        string description = Convert.ToString(ddl_descrip.SelectedItem.Value);
        if (description.Trim() == "" && description.Trim() == "0")
        {
            description = "";
        }
        //theivamani 7.11.15
        description = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(description);
        string[] split = todaydate.Split('/');
        dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
        string group = Convert.ToString(ddl_group.SelectedItem.Value);
        string subgroup = Convert.ToString(ddl_subgroup.SelectedItem.Value);
        if (subgroup.Trim() == "" && subgroup.Trim() == "0")
        {
            subgroup = "";
        }
        if (ddl_descrip.SelectedItem.Value.Trim() != "" && ddl_descrip.SelectedItem.Text.Trim() != "" && ddl_hostelname.SelectedItem.Value.Trim() != "0" && ddl_group.SelectedItem.Value.Trim() != "0")//&& ddl_subgroup.SelectedItem.Value.Trim() != "0"
        {
            string expansetype = Convert.ToString(ddlStudType.SelectedItem.Value);
            //if (rdb_veg.Checked == true)
            //{
            //    expansetype = "1";
            //}
            //else if (rdb_NonVeg.Checked == true)
            //{
            //    expansetype = "2";
            //}
            // string q = "INSERT INTO Hostel_Expenses(Access_Date,Access_Time,Entry_Date,Hostel_Code,Expanse_type)values('" + dtaccessdate + "','" + dtaccesstime + "','" + dt.ToString("MM/dd/yyyy") + "','" + ddl_hostelname.SelectedItem.Value + "','" + expansetype + "')";
            string q = "INSERT INTO HT_HostelExpenses(ExpensesType,ExpensesDate,ExpGroup,ExpSubGroup,ExpDesc,ExpAmount,HostelFK,Messname )values('" + expansetype + "','" + dt.ToString("MM/dd/yyyy") + "','" + ddl_group.SelectedItem.Value + "','" + ddl_subgroup.SelectedItem.Value + "','" + ddl_descrip.SelectedItem.Text + "','" + amount + "','" + ddl_hostelname.SelectedItem.Value + "','"+Convert.ToString(ddlmess.SelectedValue)+"') ";
            int ds = d2.update_method_wo_parameter(q, "Text");
            if (ds != 0)
            {
                //popwindow.Visible = false;
                alertpopwindow.Visible = true;
                loadhostel();
                bindaddgroup();
                bindsubgroup();
                binddescription();
                btn_go_Click(sender, e);
                lblalerterr.Text = "Saved Successfully";
                ddl_hostelname.SelectedItem.Value = "0";
                ddl_group.SelectedItem.Value = "0";
                ddl_subgroup.SelectedItem.Value = "0";
                ddl_descrip.SelectedItem.Value = "0";
                txt_amount.Text = "";
                txt_todaydate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Found";
            }
        }
        //}
        //else
        //{
        //    alertpopwindow.Visible = true;
        //    lblalerterr.Text = "Please Select All Field";
        //}
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
        //surediv.Visible = false;
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
            string hostelname = Convert.ToString(ddl_hostelname.SelectedItem.Text);
            string hostelnamecode = Convert.ToString(ddl_hostelname.SelectedItem.Value);
            string groupname = Convert.ToString(ddl_group.SelectedItem.Value);
            string subgroup = Convert.ToString(ddl_subgroup.SelectedItem.Value);
            string description = Convert.ToString(ddl_descrip.SelectedItem.Value);
            string amount = Convert.ToString(txt_amount.Text);
            string date = Convert.ToString(txt_todaydate.Text);
            string[] split = date.Split('/');
            DateTime dt = new DateTime();
            if (split.Length > 2)
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            //  string delquery = "if exists(select * from Hostel_ExpensesDetail hed,Hostel_Expenses he where hed.exp_code=he.Expenses_code and hed.maingroup_desc='" + groupname + "' and hed.sub_group='" + subgroup + "' and hed.exp_code=he.Expenses_code) delete from  Hostel_ExpensesDetail where MainGroup_Desc='" + groupname + "' and Sub_Group='" + subgroup + "' and Description='" + description + "'";
            string delquery = "delete from HT_HostelExpenses where HostelExpensePk ='" + Convert.ToString(Session["HostelExpensePk"]) + "'";
            int ins = d2.update_method_wo_parameter(delquery, "Text");
            if (ins != 0)
            {
                //btn_go_Click(sender, e);
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
                lbl_sure.Text = "Do you want to delete this record?";
            }
            //string hostelname = Convert.ToString(ddl_hostelname.SelectedItem.Text);
            //string hostelnamecode = Convert.ToString(ddl_hostelname.SelectedItem.Value);
            //string groupname = Convert.ToString(ddl_group.SelectedItem.Value);
            //string subgroup = Convert.ToString(ddl_subgroup.SelectedItem.Value);
            //string description = Convert.ToString(txt_popdesc.Text);
            //string amount = Convert.ToString(txt_amount.Text);
            //string delquery = "if exists(select * from Hostel_ExpensesDetail hed,Hostel_Expenses he where hed.exp_code=he.Expenses_code and hed.maingroup_desc='" + groupname + "' and hed.sub_group='" + subgroup + "' and hed.exp_code=he.Expenses_code) delete from  Hostel_ExpensesDetail where MainGroup_Desc='" + groupname + "' and Sub_Group='" + subgroup + "' and Description='" + description + "'";
            //int ins = d2.update_method_wo_parameter(delquery, "Text");
            //if (ins != 0)
            //{
            //    btn_go_Click(sender, e);
            //    popwindow.Visible = false;
            //    alertpopwindow.Visible = true;
            //    lblalerterr.Text = "Deleted Successfully";
            //    lblalerterr.Visible = true;
            //}
        }
        catch
        {
        }
    }
    //theivamani 6.11.15
    public void rdb_datewise_CheckedChanged(object sender, EventArgs e)
    {
        txt_fromdate.Enabled = true;
        txt_todate.Enabled = true;
        // rdb_totalwise.Enabled = false;
    }
    public void rdb_totalwise_CheckedChanged(object sender, EventArgs e)
    {
        txt_fromdate.Enabled = false;
        txt_todate.Enabled = false;
        //rdb_datewise.Enabled = false;
    }
    protected void cb_messname_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_messname.Checked == true)
        {
            if (cbl_messname.Items.Count > 0)
            {
                for (int i = 0; i < cbl_messname.Items.Count; i++)
                {
                    cbl_messname.Items[i].Selected = true;
                }
                txt_messname.Text = "Mess Name(" + (cbl_messname.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cbl_messname.Items.Count; i++)
            {
                cbl_messname.Items[i].Selected = false;
            }
            txt_messname.Text = "--Select--";
        }
    }
    protected void cbl_messname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_messname.Text = "--Select--";
            cb_messname.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_messname.Items.Count; i++)
            {
                if (cbl_messname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_messname.Text = "Mess Name(" + commcount.ToString() + ")";
                if (commcount == cbl_messname.Items.Count)
                {
                    cb_messname.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void bindmess()
    {
        try
        {
            cbl_messname.Items.Clear();
            ds.Clear();
            //ds = d2.BindMess(collegecode1);
            // string itemname = "select MessMasterPK,MessName,MessAcr from HM_MessMaster where CollegeCode=" + collegecode1 + " order by MessMasterPK asc";
            // ds.Clear();
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            //ds = d2.select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_messname.DataSource = ds;
                cbl_messname.DataTextField = "MessName";
                cbl_messname.DataValueField = "MessMasterPK";
                cbl_messname.DataBind();
                ddl_messname.DataSource = ds;
                ddl_messname.DataTextField = "MessName";
                ddl_messname.DataValueField = "MessMasterPK";
                ddl_messname.DataBind();
                if (cbl_messname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_messname.Items.Count; i++)
                    {
                        cbl_messname.Items[i].Selected = true;
                        string messcode = "";
                        if (messcode == "")
                        {
                            messcode = Convert.ToString(cbl_messname.Items[i].Value);
                        }
                        else
                        {
                            messcode = messcode + "'" + "," + "'" + Convert.ToString(cbl_messname.Items[i].Value);
                        }
                    }
                    txt_messname.Text = "Mess Name(" + cbl_messname.Items.Count + ")";
                }
            }
        }
        catch { }
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
            cb_itemname.Checked = false;
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
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_headername_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_headername.Checked == true)
            {
                for (int i = 0; i < cbl_headername.Items.Count; i++)
                {
                    cbl_headername.Items[i].Selected = true;
                }
                txt_headername.Text = "Header Name(" + (cbl_headername.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_headername.Items.Count; i++)
                {
                    cbl_headername.Items[i].Selected = false;
                }
                txt_headername.Text = "--Select--";
            }
            loadsub();
            loaditem();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_headername_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_headername.Text = "--Select--";
            cb_headername.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_headername.Items.Count; i++)
            {
                if (cbl_headername.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_headername.Text = "Header Name(" + commcount.ToString() + ")";
                if (commcount == cbl_headername.Items.Count)
                {
                    cb_headername.Checked = true;
                }
            }
            loadsub();
            loaditem();
        }
        catch (Exception ex)
        {
        }
    }
    protected void itemheadername()
    {
        try
        {
            ds.Clear();
            //ds = d2.BindItemHeaderWithOutRights();
            string headerquery = "select distinct ItemHeaderCode ,ItemHeaderName  from IM_ItemMaster";
            ds.Clear();
            ds = d2.select_method_wo_parameter(headerquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_headername.DataSource = ds;
                cbl_headername.DataTextField = "ItemHeaderName";
                cbl_headername.DataValueField = "ItemHeaderCode";
                cbl_headername.DataBind();
                if (cbl_headername.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_headername.Items.Count; i++)
                    {
                        cbl_headername.Items[i].Selected = true;
                    }
                    txt_headername.Text = "Header Name(" + cbl_headername.Items.Count + ")";
                }
            }
            else
            {
                txt_headername.Text = "--Select--";
                txt_itemname.Text = "--Select--";
            }
            loadsub();
            loaditem();
        }
        catch
        {
        }
    }
    protected void bind_mesheaderlookup()
    {
        try
        {
            ds.Clear();
            //   ds = d2.BindItemHeaderWithOutRights();
            string headerquery = "select distinct ItemHeaderCode ,ItemHeaderName  from IM_ItemMaster";
            ds.Clear();
            ds = d2.select_method_wo_parameter(headerquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_itemheader3.DataSource = ds;
                cbl_itemheader3.DataTextField = "ItemHeaderName";
                cbl_itemheader3.DataValueField = "ItemHeaderCode";
                cbl_itemheader3.DataBind();
                if (cbl_itemheader3.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_itemheader3.Items.Count; i++)
                    {
                        cbl_itemheader3.Items[i].Selected = true;
                    }
                    txt_itemheader3.Text = "Header Name(" + cbl_itemheader3.Items.Count + ")";
                }
            }
            else
            {
                txt_itemheader3.Text = "--Select--";
                txt_itemname3.Text = "--Select--";
            }
            loadsubheadername();
            loadlookupitem();
        }
        catch { }
    }
    public void loadlookupitem()
    {
        try
        {
            // cbl_itemheader3.Items.Clear();
            string messcode = "";
            string subheader = "";
            for (int i = 0; i < cbl_itemheader3.Items.Count; i++)
            {
                if (cbl_itemheader3.Items[i].Selected == true)
                {
                    if (messcode == "")
                    {
                        messcode = "" + cbl_itemheader3.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        messcode = messcode + "'" + "," + "'" + cbl_itemheader3.Items[i].Value.ToString() + "";
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
            if (messcode.Trim() != "" && subheader.Trim() != "")
            {
                ds.Clear();
                //  ds = d2.BindItemCodewithsubheader(messcode, subheader);
                ds = d2.BindItemCodewithsubheaderMaster_inv(messcode, subheader);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklst_pop2itemtyp.DataSource = ds;
                    chklst_pop2itemtyp.DataTextField = "itemname";
                    chklst_pop2itemtyp.DataValueField = "ItemPK";
                    chklst_pop2itemtyp.DataBind();
                    if (cbl_itemname.Items.Count > 0)
                    {
                        for (int i = 0; i < chklst_pop2itemtyp.Items.Count; i++)
                        {
                            chklst_pop2itemtyp.Items[i].Selected = true;
                        }
                        txt_itemname3.Text = "Item Name(" + chklst_pop2itemtyp.Items.Count + ")";
                    }
                    if (chklst_pop2itemtyp.Items.Count > 5)
                    {
                        Panel2.Width = 180;
                        Panel2.Height = 250;
                    }
                }
                else
                {
                    txt_itemname3.Text = "--Select--";
                }
            }
            itemmaster();
        }
        catch
        {
        }
    }
    public void loaditem()
    {
        try
        {
            cbl_itemname.Items.Clear();
            string messcode = "";
            string subheader = "";
            for (int i = 0; i < cbl_headername.Items.Count; i++)
            {
                if (cbl_headername.Items[i].Selected == true)
                {
                    if (messcode == "")
                    {
                        messcode = "" + cbl_headername.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        messcode = messcode + "'" + "," + "'" + cbl_headername.Items[i].Value.ToString() + "";
                    }
                }
            }
            for (int i = 0; i < cbl_sub.Items.Count; i++)
            {
                if (cbl_sub.Items[i].Selected == true)
                {
                    if (subheader == "")
                    {
                        subheader = "" + cbl_sub.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        subheader = subheader + "'" + "," + "" + "'" + cbl_sub.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (messcode.Trim() != "" && subheader.Trim() != "")
            {
                ds.Clear();
                //  ds = d2.BindItemCodewithsubheader(messcode, subheader);
                ds = d2.BindItemCodewithsubheaderMaster_inv(messcode, subheader);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_itemname.DataSource = ds;
                    cbl_itemname.DataTextField = "itemname";
                    cbl_itemname.DataValueField = "ItemPK";
                    cbl_itemname.DataBind();
                    if (cbl_itemname.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_itemname.Items.Count; i++)
                        {
                            cbl_itemname.Items[i].Selected = true;
                        }
                        txt_itemname.Text = "Item Name(" + cbl_itemname.Items.Count + ")";
                    }
                    //if (cbl_itemname.Items.Count > 5)
                    //{
                    //    Panel2.Width = 300;
                    //    Panel2.Height = 300;
                    //}
                }
                else
                {
                    txt_itemname.Text = "--Select--";
                }
            }
            else
            {
                txt_itemname.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void ddl_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_type.SelectedValue == "0")
        {
            txt_searchby.Visible = true;
            txt_searchitemcode.Visible = false;
            txt_searchheadername.Visible = false;
            txt_searchheadername.Text = "";
            txt_searchitemcode.Text = "";
        }
        else if (ddl_type.SelectedValue == "1")
        {
            txt_searchby.Visible = false;
            txt_searchitemcode.Visible = true;
            txt_searchheadername.Visible = false;
            txt_searchby.Text = "";
            txt_searchheadername.Text = "";
        }
        else if (ddl_type.SelectedValue == "2")
        {
            txt_searchby.Visible = false;
            txt_searchitemcode.Visible = false;
            txt_searchheadername.Visible = true;
            txt_searchby.Text = "";
            txt_searchitemcode.Text = "";
        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        //  string query = "select distinct item_name from item_master WHERE item_name like '" + prefixText + "%' ";
        string query = "select distinct ItemName from IM_ItemMaster WHERE ItemName like '" + prefixText + "%'";
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
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getitemcode(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        //    string query = "select distinct item_code from item_master WHERE item_code like '" + prefixText + "%' ";
        string query = "select distinct ItemCode from IM_ItemMaster WHERE ItemCode like '" + prefixText + "%'";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["ItemCode"].ToString());
            }
        }
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getitemheader(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        //   string query = "select distinct itemheader_name from item_master WHERE itemheader_name like '" + prefixText + "%' ";
        string query = "select distinct ItemHeaderName from IM_ItemMaster WHERE ItemHeaderName like '" + prefixText + "%'";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["ItemHeaderName"].ToString());
            }
        }
        return name;
    }
    protected void txt_messfromdate_Textchanged(object sender, EventArgs e)
    {
        try
        {
            lbl_error1.Visible = false;
            string fromdate = txt_messfromdate.Text;
            string todate = txt_messtodate.Text;
            if (fromdate != "" && fromdate != null && todate != "" && todate != null)
            {
                string[] spiltfrom = fromdate.Split('/');
                string[] spitto = todate.Split('/');
                DateTime from = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
                DateTime to = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);
                if (from > to)
                {
                    lbl_error1.Visible = true;
                    lbl_error1.Text = "Please Enter To Date Greater Than From Date";
                    //Fpspread2.Visible = false;
                    //div2.Visible = false;
                    //btn_save.Visible = false;
                    //rptprint.Visible = false;
                    //btn_update.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lbl_error1.Visible = true;
            lbl_error1.Text = ex.ToString();
        }
    }
    protected void txt_messtodate_Textchanged(object sender, EventArgs e)
    {
        try
        {
            lbl_error1.Visible = false;
            string fromdate = txt_messfromdate.Text;
            string todate = txt_messtodate.Text;
            if (fromdate != "" && fromdate != null && todate != "" && todate != null)
            {
                string[] spiltfrom = fromdate.Split('/');
                string[] spitto = todate.Split('/');
                DateTime from = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
                DateTime to = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);
                if (from > to)
                {
                    lbl_error1.Visible = true;
                    lbl_error1.Text = "Please Enter To Date Grater Than From Date";
                    //Fpspread2.Visible = false;
                    //div2.Visible = false;
                    //rptprint.Visible = false;
                    //btn_save.Visible = false;
                    //btn_update.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = ex.ToString();
        }
    }
    protected void btn_messgo_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol2.Visible = false;
            string mess_code = "";
            for (int i = 0; i < cbl_messname.Items.Count; i++)
            {
                if (cbl_messname.Items[i].Selected == true)
                {
                    if (mess_code == "")
                    {
                        mess_code = "" + cbl_messname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        mess_code = mess_code + "'" + "," + "'" + cbl_messname.Items[i].Value.ToString() + "";
                    }
                }
            }
            string itemheadercode = "";
            for (int i = 0; i < cbl_headername.Items.Count; i++)
            {
                if (cbl_headername.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_headername.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cbl_headername.Items[i].Value.ToString() + "";
                    }
                }
            }
            string itemcode = "";
            for (int i = 0; i < cbl_itemname.Items.Count; i++)
            {
                if (cbl_itemname.Items[i].Selected == true)
                {
                    if (itemcode == "")
                    {
                        itemcode = "" + cbl_itemname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemcode = itemcode + "'" + "," + "'" + cbl_itemname.Items[i].Value.ToString() + "";
                    }
                }
            }
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string firstdate = Convert.ToString(txt_messfromdate.Text);
            string seconddate = Convert.ToString(txt_messtodate.Text);
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            string[] split1 = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
            if (txt_messname.Text.Trim() != "--Select--" && txt_headername.Text.Trim() != "--Select--")
            {
                string q = "";
                if (txt_searchby.Text.Trim() != "")
                {
                    q = " select m.MessName,dd.Hostel_Code,dd.Item_Code ,item_name,Hand_Qty,RPU,Consumption_Qty,Consumption_Value,CONVERT(varchar(10),Consumption_Date,103) as Consumption_Date from DailyConsumption_Detail dd,DailyConsumption_Master dm,item_master i,MessMaster m where dd.DailyConsumptionMaster_Code =dm.DailyConsumptionMaster_Code and i.item_code=dd.Item_Code and dd.Hostel_Code=m.MessID and item_name='" + txt_searchby.Text + "'"; // and dd.Hostel_Code in('" + mess_code + "') and i.itemheader_code in ('" + itemheadercode + "') and dd.Item_Code in('" + itemcode + "') and Consumption_Date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' ";
                }
                else if (txt_searchitemcode.Text.Trim() != "")
                {
                    q = " select m.MessName,dd.Hostel_Code,dd.Item_Code ,item_name,Hand_Qty,RPU,Consumption_Qty,Consumption_Value,CONVERT(varchar(10),Consumption_Date,103) as Consumption_Date from DailyConsumption_Detail dd,DailyConsumption_Master dm,item_master i,MessMaster m where dd.DailyConsumptionMaster_Code =dm.DailyConsumptionMaster_Code and i.item_code=dd.Item_Code and dd.Hostel_Code=m.MessID and dd.Item_Code='" + txt_searchitemcode.Text + "'";
                }
                else if (txt_searchheadername.Text.Trim() != "")
                {
                    q = " select m.MessName,dd.Hostel_Code,dd.Item_Code ,item_name,Hand_Qty,RPU,Consumption_Qty,Consumption_Value,CONVERT(varchar(10),Consumption_Date,103) as Consumption_Date from DailyConsumption_Detail dd,DailyConsumption_Master dm,item_master i,MessMaster m where dd.DailyConsumptionMaster_Code =dm.DailyConsumptionMaster_Code and i.item_code=dd.Item_Code and dd.Hostel_Code=m.MessID and i.itemheader_name='" + txt_searchheadername.Text + "'";
                }
                else
                {
                    q = " select m.MessName,dd.Hostel_Code,dd.Item_Code ,item_name,Hand_Qty,RPU,Consumption_Qty,Consumption_Value,CONVERT(varchar(10),Consumption_Date,103) as Consumption_Date from DailyConsumption_Detail dd,DailyConsumption_Master dm,item_master i,MessMaster m where dd.DailyConsumptionMaster_Code =dm.DailyConsumptionMaster_Code and i.item_code=dd.Item_Code and dd.Hostel_Code=m.MessID and dd.Hostel_Code in('" + mess_code + "') and i.itemheader_code in ('" + itemheadercode + "') and dd.Item_Code in('" + itemcode + "') and Consumption_Date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' ";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(q, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fpspread3.Sheets[0].RowCount = 0;
                    Fpspread3.Sheets[0].ColumnCount = 0;
                    Fpspread3.CommandBar.Visible = false;
                    Fpspread3.Sheets[0].AutoPostBack = false;
                    Fpspread3.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread3.Sheets[0].RowHeader.Visible = false;
                    Fpspread3.Sheets[0].ColumnCount = 8;
                    Fpspread3.Width = 910;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[0].Width = 50;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Mess Name";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[1].Width = 200;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Consumption Date";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[2].Width = 100;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item Code";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[3].Width = 100;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Item Name";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[4].Width = 200;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 5].Text = "RPU";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[5].Width = 100;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Consumption Quantity";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[6].Width = 100;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Consumption Value";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[7].Width = 100;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Fpspread3.Sheets[0].RowCount++;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["MessName"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Consumption_Date"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["item_code"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["item_name"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["hostel_code"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["rpu"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["Consumption_Qty"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Hand_Qty"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["Consumption_Value"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                    }
                    Fpspread3.Sheets[0].PageSize = Fpspread3.Sheets[0].RowCount;
                    Fpspread3.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpspread3.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpspread3.Sheets[0].FrozenRowCount = 0;
                    Fpspread3.Visible = true;
                    div5.Visible = true;
                    lbl_error.Visible = false;
                    rptprint2.Visible = true;
                }
                else
                {
                    lbl_error.Visible = true;
                    Fpspread3.Visible = false;
                    div5.Visible = false;
                    lbl_error.Text = "No Record Found";
                    rptprint2.Visible = false;
                }
            }
            else
            {
                lbl_error.Visible = true;
                Fpspread3.Visible = false;
                div5.Visible = false;
                rptprint2.Visible = false;
                lbl_error.Text = "Please Select All Fields";
            }
        }
        catch { }
    }
    protected void btn_messaddnew_Click(object sender, EventArgs e)
    {
        try
        {
            pop_mess.Visible = true;
            Printcontrol2.Visible = false;
            Session["dt"] = null;
            ViewState["selecteditems"] = null;
            selectitemgrid.DataSource = null;
            selectitemgrid.DataBind();
            SelectdptGrid.DataSource = null;
            SelectdptGrid.DataBind();
            txt_messitemname.Text = "";
            ddl_searchitems.SelectedIndex = 0;
            txt_messitem1.Text = "";
            txt_messitemcode1.Text = "";
            txt_messitemheader1.Text = "";
        }
        catch { }
    }
    protected void imagemessbtnpopclose_Click(object sender, EventArgs e)
    {
        pop_mess.Visible = false;
    }
    protected void itemlookup_Click(object sender, EventArgs e)
    {
        bind_mesheaderlookup();
        loadsubheadername();
        loadlookupitem();
        Session["dt"] = null;
        selectitemgrid.DataSource = null;
        selectitemgrid.DataBind();
        ViewState["selecteditems"] = null;
        btn_go3_Click(sender, e);
        pop_messitemlookup.Visible = true;
        txt_messitemname.Text = "";
        ddl_searchitems.SelectedIndex = 0;
        txt_messitem1.Text = "";
        txt_messitemcode1.Text = "";
        txt_messitemheader1.Text = "";
    }
    public void itemmaster()
    {
        string itemheadercode = "";
        string subheader = "";
        for (int i = 0; i < cbl_itemheader3.Items.Count; i++)
        {
            if (cbl_itemheader3.Items[i].Selected == true)
            {
                if (itemheadercode == "")
                {
                    itemheadercode = "" + cbl_itemheader3.Items[i].Value.ToString() + "";
                }
                else
                {
                    itemheadercode = itemheadercode + "'" + "," + "'" + cbl_itemheader3.Items[i].Value.ToString() + "";
                }
            }
        }
        chklst_pop2itemtyp.Items.Clear();
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
        if (itemheadercode.Trim() != "" && subheader.Trim() != "")
        {
            ds.Clear();
            //    ds = d2.BindItemCodewithsubheader(itemheadercode, subheader);
            ds = d2.BindItemCodewithsubheaderMaster_inv(itemheadercode, subheader);
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklst_pop2itemtyp.DataSource = ds;
                chklst_pop2itemtyp.DataTextField = "itemname";
                chklst_pop2itemtyp.DataValueField = "ItemPK";
                chklst_pop2itemtyp.DataBind();
                if (chklst_pop2itemtyp.Items.Count > 0)
                {
                    for (int i = 0; i < chklst_pop2itemtyp.Items.Count; i++)
                    {
                        chklst_pop2itemtyp.Items[i].Selected = true;
                    }
                    txt_itemname3.Text = "Item Name(" + chklst_pop2itemtyp.Items.Count + ")";
                }
            }
            else
            {
                txt_itemname3.Text = "--Select--";
            }
        }
        else
        {
            txt_itemname3.Text = "--Select--";
        }
    }
    protected void cb_itemheader3_CheckedChange(object sender, EventArgs e)
    {
        int cout = 0;
        txt_itemheader3.Text = "--Select--";
        if (cb_itemheader3.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_itemheader3.Items.Count; i++)
            {
                cbl_itemheader3.Items[i].Selected = true;
            }
            txt_itemheader3.Text = "Item Header(" + (cbl_itemheader3.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_itemheader3.Items.Count; i++)
            {
                cbl_itemheader3.Items[i].Selected = false;
            }
        }
        loadsubheadername();
        //  loadlookupitem();
        itemmaster();
    }
    protected void cbl_itemheader_SelectedIndexChange(object sender, EventArgs e)
    {
        int i = 0;
        //cb_hostelname1.Checked = false;
        int commcount = 0;
        txt_itemheader3.Text = "--Select--";
        for (i = 0; i < cbl_itemheader3.Items.Count; i++)
        {
            if (cbl_itemheader3.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_itemheader3.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_itemheader3.Items.Count)
            {
                cb_itemheader3.Checked = true;
            }
            txt_itemheader3.Text = "Item Header(" + commcount.ToString() + ")";
        }
        loadsubheadername();
        // loadlookupitem();
        itemmaster();
    }
    protected void chklstitemtyp(object sender, EventArgs e)
    {
        int i = 0;
        chk_pop2itemtyp.Checked = false;
        int commcount = 0;
        txt_itemname3.Text = "--Select--";
        for (i = 0; i < chklst_pop2itemtyp.Items.Count; i++)
        {
            if (chklst_pop2itemtyp.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                // cb_hostelname1.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == chklst_pop2itemtyp.Items.Count)
            {
                chk_pop2itemtyp.Checked = true;
            }
            txt_itemname3.Text = "Item Name(" + commcount.ToString() + ")";
        }
    }
    protected void chkitemtyp(object sender, EventArgs e)
    {
        int cout = 0;
        txt_itemname3.Text = "--Select--";
        if (chk_pop2itemtyp.Checked == true)
        {
            cout++;
            for (int i = 0; i < chklst_pop2itemtyp.Items.Count; i++)
            {
                chklst_pop2itemtyp.Items[i].Selected = true;
            }
            txt_itemname3.Text = "Item Name(" + (chklst_pop2itemtyp.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chklst_pop2itemtyp.Items.Count; i++)
            {
                chklst_pop2itemtyp.Items[i].Selected = false;
            }
        }
    }
    protected void btn_itemsave4_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("ItemCode");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("Measure");
            dt.Columns.Add("Hand on quantity");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("rpu");
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
            string itemname = "";
            if (selectitemgrid.Rows.Count > 0)
            {
                for (int i = 0; i < selectitemgrid.Rows.Count; i++)
                {
                    string itemcode = Convert.ToString((selectitemgrid.Rows[i].FindControl("itemcodegv") as Label).Text);
                    string checkconsumitem = "select distinct Item_Code,AvlQty,rpu from Stock_Detail where Item_Code='" + itemcode + "' and Dept_Code='" + ddl_messname.SelectedItem.Value + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(checkconsumitem, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dr = dt.NewRow();
                        dr[0] = Convert.ToString((selectitemgrid.Rows[i].FindControl("itemcodegv") as Label).Text);
                        dr[1] = Convert.ToString((selectitemgrid.Rows[i].FindControl("itemnamegv") as Label).Text);
                        dr[2] = Convert.ToString((selectitemgrid.Rows[i].FindControl("lbl_measureitem") as Label).Text);
                        // dr[3] = Convert.ToString((selectitemgrid.Rows[i].FindControl("lbl_avlqty") as Label).Text);
                        dr[3] = Convert.ToString(ds.Tables[0].Rows[0]["AvlQty"].ToString());
                        dr[4] = Convert.ToString("");
                        // dr[5] = Convert.ToString((selectitemgrid.Rows[i].FindControl("lbl_rpu") as Label).Text);
                        dr[5] = Convert.ToString(ds.Tables[0].Rows[0]["rpu"].ToString());
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                SelectdptGrid.DataSource = dt;
                SelectdptGrid.DataBind();
                SelectdptGrid.Visible = true;
                Session["dt"] = dt;
                string itemname1 = Convert.ToString(dt.Rows.Count);
                //if (dt.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        //itemname1 = Convert.ToString(dt.Rows[i]["ItemName"]);
                //        if (itemname1 == "")
                //        {
                //            itemname1 = "" + Convert.ToString(dt.Rows[i]["ItemName"]) + "";
                //        }
                //        else
                //        {
                //            itemname1 = itemname1 + "," + Convert.ToString(dt.Rows[i]["ItemName"]) + "";
                //        }
                //    }
                //}
                txt_messitemname.Text = "Selected Item (" + itemname1 + ")";
                pop_messitemlookup.Visible = false;
                pop_mess.Visible = true;
                btn_messsave.Visible = true;
                btn_messexit.Visible = true;
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please update opening stock";
                lblalerterr.Visible = true;
            }
            if (count == 0)
            {
            }
        }
        catch
        {
        }
    }
    protected void btn_conexit4_Click(object sender, EventArgs e)
    {
        pop_messitemlookup.Visible = false;
    }
    protected void btn_go3_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["selecteditems"] != null)
            {
                DataTable dnew = (DataTable)ViewState["selecteditems"];
                ViewState["sb"] = dnew;
                checknew = "s";
            }
            string itemheadercode = "";
            for (int i = 0; i < cbl_itemheader3.Items.Count; i++)
            {
                if (cbl_itemheader3.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_itemheader3.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cbl_itemheader3.Items[i].Value.ToString() + "";
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
            string selectquery = "";
            if (txt_messitem1.Text.Trim() != "")
            {
                //  selectquery = "select itemheader_name,itemheader_code,item_code,item_name ,model_name,Size_name ,item_unit,description ,special_instru from Item_Master where item_name='" + txt_messitem1.Text + "'  order by item_code";
                selectquery = " select ItemHeaderName,ItemHeaderCode,ItemPK,ItemCode,ItemName ,ItemModel,ItemSize ,ItemUnit,ItemSpecification from IM_ItemMaster where ItemName='" + txt_messitem1.Text + "'  order by ItemCode ";
            }
            else if (txt_messitemcode1.Text.Trim() != "")
            {
                // selectquery = "select itemheader_name,itemheader_code,item_code,item_name ,model_name,Size_name ,item_unit,description ,special_instru from Item_Master where item_code='" + txt_messitemcode1.Text + "' order by item_code";
                selectquery = " select ItemHeaderName,ItemHeaderCode,ItemPK,ItemCode,ItemName ,ItemModel,ItemSize ,ItemUnit,ItemSpecification from IM_ItemMaster where ItemCode='" + txt_messitemcode1.Text + "'  order by ItemCode ";
                //selectquery = " select distinct itemheader_name,itemheader_code,i.item_code,item_name,model_name,Size_name,i.item_unit,description ,special_instru,s.AvlQty,s.rpu from Item_Master i,Stock_Detail s where i.item_code=s.item_code  and i.item_code='" + txt_messitemcode1.Text + "' and s.Dept_Code='" + ddl_messname.SelectedItem.Value + "' order by i.item_code";
            }
            else if (txt_messitemheader1.Text.Trim() != "")
            {
                // selectquery = "select itemheader_name,itemheader_code,item_code,item_name ,model_name,Size_name ,item_unit,description ,special_instru from Item_Master where itemheader_name='" + txt_messitemheader1.Text + "' order by item_code";
                selectquery = " select ItemHeaderName,ItemHeaderCode,ItemPK,ItemCode,ItemName ,ItemModel,ItemSize ,ItemUnit,ItemSpecification from IM_ItemMaster where ItemHeaderName='" + txt_messitemheader1.Text + "'  order by ItemCode ";
            }
            else if (itemheadercode.Trim() != "" && itemheadercode1.Trim() != "")
            {
                //  selectquery = "select distinct  item_code ,item_name , itemheader_code,itemheader_name,item_unit from item_master where itemheader_code in ('" + itemheadercode + "') and item_code in ('" + itemheadercode1 + "') order by item_code ";
                selectquery = " select distinct  ItemCode ,ItemName , ItemHeaderCode,ItemHeaderName,ItemUnit from IM_ItemMaster where ItemHeaderCode in ('" + itemheadercode + "') and ItemPK in ('" + itemheadercode1 + "') order by ItemCode";
            }
            if (txt_itemheader3.Text.Trim() != "--Select--" && txt_itemname3.Text.Trim() != "--Select--")
            {
                if (selectquery.Trim() != "")
                {
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gvdatass.DataSource = ds.Tables[0];
                        gvdatass.DataBind();
                        gvdatass.Visible = true;
                        div4.Visible = true;
                        btn_itemsave4.Visible = true;
                        btn_conexist4.Visible = true;
                        lbl_error3.Visible = false;
                    }
                }
            }
            else
            {
                div4.Visible = false;
                lbl_error3.Visible = true;
                btn_itemsave4.Visible = false;
                btn_conexist4.Visible = false;
                lbl_error3.Text = "Please select all fields";
            }
        }
        catch { }
    }
    protected void ddl_search_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_searchitems.SelectedValue == "0")
        {
            txt_messitem1.Visible = true;
            txt_messitemcode1.Visible = false;
            txt_messitemheader1.Visible = false;
            txt_messitemheader1.Text = "";
            txt_messitemcode1.Text = "";
        }
        else if (ddl_searchitems.SelectedValue == "1")
        {
            txt_messitem1.Visible = false;
            txt_messitemcode1.Visible = true;
            txt_messitemheader1.Visible = false;
            txt_messitem1.Text = "";
            txt_messitemheader1.Text = "";
        }
        else if (ddl_searchitems.SelectedValue == "2")
        {
            txt_messitem1.Visible = false;
            txt_messitemcode1.Visible = false;
            txt_messitemheader1.Visible = true;
            txt_messitem1.Text = "";
            txt_messitemcode1.Text = "";
        }
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        pop_messitemlookup.Visible = false;
    }
    protected void selectedmenuchk(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            bindtable();
            if (checknew == "s")
            {
                if (ViewState["sb"] != null)
                {
                    DataTable dts = (DataTable)ViewState["sb"];
                    DataView dv = new DataView(dts);
                    dt = dv.ToTable();
                    dr = null;
                    // checknew = "";
                }
            }
            else
            {
            }
            foreach (DataListItem gvrow in gvdatass.Items)
            {
                CheckBox chkSelect = (gvrow.FindControl("CheckBox2") as CheckBox);
                if (chkSelect.Checked)
                {
                    count++;
                    dr = dt.NewRow();
                    string itemcode = "";
                    string itemnamegv = "";
                    string itemheadername = "";
                    dr[0] = Convert.ToString(count);
                    Label lbl_itemname = (Label)gvrow.FindControl("lbl_itemname");
                    itemnamegv = lbl_itemname.Text;
                    dr[1] = itemnamegv;
                    Label lbl_itemcode = (Label)gvrow.FindControl("lbl_itemcode");
                    itemcode = lbl_itemcode.Text;
                    dr[2] = itemcode;
                    Label lbl_headername = (Label)gvrow.FindControl("lblitemheadername");
                    itemheadername = lbl_headername.Text;
                    dr[3] = itemheadername;
                    Label lbl_itemheadercode = (Label)gvrow.FindControl("lbl_itemheadercode");
                    string itemheadercode = lbl_itemheadercode.Text;
                    dr[4] = itemheadercode;
                    Label lbl_measureitem = (Label)gvrow.FindControl("lbl_measureitem");
                    string measureitem = lbl_measureitem.Text;
                    //if(measureitem.Trim()!="")
                    //{
                    dr[5] = measureitem;
                    //}
                    //Label lbl_avlqty = (Label)gvrow.FindControl("lbl_avlqty");
                    //string avalqty = lbl_avlqty.Text;
                    //dr[6] = avalqty;
                    //Label lbl_rpu = (Label)gvrow.FindControl("lbl_rpu");
                    //string lbl_rpu1 = lbl_rpu.Text;
                    //dr[7] = lbl_rpu1;
                    if (dt.Rows.Count > 0)
                    {
                        DataView d = new DataView(dt);
                        d.RowFilter = "ItemCode ='" + itemcode + "'";
                        if (d.Count == 0)
                        {
                            dt.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        dt.Rows.Add(dr);
                    }
                    // dt = dt.DefaultView.ToTable(true, "Item Name", "Item Code", "Header Name", "Header code", "Item unit");
                    // ViewState["selecteditems"] = dt;
                    selectitemgrid.DataSource = dt;
                    selectitemgrid.DataBind();
                }
                else
                {
                }
            }
            selectitemgrid.DataSource = dt;
            selectitemgrid.DataBind();
            ViewState["selecteditems"] = dt;
        }
        catch
        { }
    }
    public void bindtable()
    {
        dt.Columns.Add("S.No");
        dt.Columns.Add("Item Name");
        dt.Columns.Add("ItemCode");
        dt.Columns.Add("Header Name");
        dt.Columns.Add("Header code");
        dt.Columns.Add("Item unit");
        dt.Columns.Add("Hand on quantity");
        dt.Columns.Add("rpu");
        dt.TableName = "selecteditems";
    }
    protected void rdb_commonwise_Click(object sender, EventArgs e)
    {
        if (rdb_commonwise.Checked == true)
        {
            commontable.Visible = true;
            messwisetabel.Visible = false;
            div5.Visible = false;
            div2.Visible = false;
            rptprint.Visible = false;
        }
        else
        {
            messwisetabel.Visible = true;
            commontable.Visible = false;
            div5.Visible = false;
            div2.Visible = false;
            rptprint.Visible = false;
        }
    }
    protected void rdb_messwise_Click(object sender, EventArgs e)
    {
        if (rdb_messwise.Checked == true)
        {
            messwisetabel.Visible = true;
            commontable.Visible = false;
            div5.Visible = false;
            div2.Visible = false;
            rptprint.Visible = false;
        }
        else
        {
            commontable.Visible = true;
            messwisetabel.Visible = false;
            div5.Visible = false;
            div2.Visible = false;
            rptprint.Visible = false;
        }
    }
    protected void btn_messsave_Click(object sender, EventArgs e)
    {
        try
        {
            string dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            string consumdate = Convert.ToString(txt_messtodate1.Text);
            DateTime dt = new DateTime();
            string[] split = consumdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            bool save = false;
            bool stock = false;
            bool master = false;
            if (SelectdptGrid.Rows.Count > 0)
            {
                for (int i = 0; i < SelectdptGrid.Rows.Count; i++)
                {
                    string itemcode = Convert.ToString((SelectdptGrid.Rows[i].FindControl("lbl_itemcode") as Label).Text);
                    string usedqty = Convert.ToString((SelectdptGrid.Rows[i].FindControl("txt_quantity") as TextBox).Text);
                    double handonqty = Convert.ToDouble((SelectdptGrid.Rows[i].FindControl("lbl_avlqty") as Label).Text);
                    double rpu = Convert.ToDouble((SelectdptGrid.Rows[i].FindControl("lbl_rpu") as Label).Text);
                    string hostel1 = ddl_messname.SelectedItem.Value;
                    double consumvalue = 0;
                    if (usedqty.Trim() != "")
                    {
                        //string dailymaster = " if exists(select*from DailyConsumption_Master where Hostel_Code='" + hostel1 + "' and Consumption_Date='" + dt.ToString("MM/dd/yyyy") + "' and typeofconsume ='0') update DailyConsumption_Master set Access_Date='" + dtaccessdate + "' ,Access_Time='" + dtaccesstime + "' where Hostel_Code='" + hostel1 + "' and Consumption_Date ='" + dt.ToString("MM/dd/yyyy") + "' else insert into DailyConsumption_Master (Access_Date,Access_Time,Hostel_Code,Consumption_Date,typeofconsume) values('" + dtaccessdate + "' ,'" + dtaccesstime + "','" + hostel1 + "','" + dt.ToString("MM/dd/yyyy") + "','0')";
                        //int update = d2.update_method_wo_parameter(dailymaster, "Text");
                        //if (update != 0)
                        //{
                        //    master = true;
                        //}
                        //string getcode = d2.GetFunction("select DailyConsumptionMaster_Code from DailyConsumption_Master where Consumption_Date='" + dt.ToString("MM/dd/yyyy") + "' and Hostel_Code='" + hostel1 + "'");
                        //int mastercode = 0;
                        //getcode = d2.GetFunction("select distinct top 1 DailyConsumptionMaster_Code from  DailyConsumption_Master  order by DailyConsumptionMaster_Code desc");
                        //if (getcode.Trim() != "" && getcode.Trim() != "0")
                        //{
                        //    mastercode = Convert.ToInt32(getcode) + 1;
                        //}
                        //else
                        //{
                        //    mastercode = 1;
                        //}
                        int mastercode = 0;
                        string getcode = "";
                        string inserquery = "";
                        getcode = d2.GetFunction("select DailyConsumptionMaster_Code from  DailyConsumption_Master where  Hostel_Code='" + hostel1 + "' and Consumption_Date ='" + dt.ToString("MM/dd/yyyy") + "' and typeofconsume='0'");
                        if (getcode.Trim() != "" && getcode.Trim() != "0")
                        {
                            mastercode = Convert.ToInt32(getcode);
                            inserquery = "update DailyConsumption_Master set Access_Date='" + dtaccessdate + "' ,Access_Time='" + dtaccesstime + "' where Hostel_Code='" + hostel1 + "' and Consumption_Date ='" + dt.ToString("MM/dd/yyyy") + "' and DailyConsumptionMaster_Code='" + mastercode + "' and typeofconsume='0'";
                        }
                        else
                        {
                            getcode = d2.GetFunction("select distinct top 1 DailyConsumptionMaster_Code from  DailyConsumption_Master  order by DailyConsumptionMaster_Code desc");
                            if (getcode.Trim() != "" && getcode.Trim() != "0")
                            {
                                mastercode = Convert.ToInt32(getcode) + 1;
                            }
                            else
                            {
                                mastercode = 1;
                            }
                            inserquery = "insert into DailyConsumption_Master(Access_Date,Access_Time,DailyConsumptionMaster_Code,Consumption_Date,Hostel_Code, typeofconsume) values ('" + dtaccessdate + "','" + dtaccesstime + "','" + mastercode + "','" + dt.ToString("MM/dd/yyyy") + "','" + hostel1 + "','0')";
                        }
                        int update = d2.update_method_wo_parameter(inserquery, "Text");
                        if (update != 0)
                        {
                            master = true;
                        }
                        if (Convert.ToString(rpu).Trim() != "" && Convert.ToString(usedqty).Trim() != "")
                        {
                            consumvalue = Convert.ToDouble(rpu) * Convert.ToDouble(usedqty);
                        }
                        // string updatequery = "update DailyConsumption_Detail set Access_Date='" + dtaccessdate + "' ,Access_Time='" + dtaccesstime + "', Consumption_Qty =Consumption_Qty-ISNULL('" + reqqty + "',0),Consumption_Value =Consumption_Value-ISNULL('" + usedvalue + "',0),Hand_Qty =Hand_Qty+ ISNULL('" + reqqty + "',0),Req_Qty =Req_Qty-ISNULL('" + reqqty + "',0),isreturn ='" + isreturnval + "',RPU ='" + rpu1 + "'  where DailyConsumptionMaster_Code ='" + mastercode + "' and Item_Code ='" + itemcode + "' and Hostel_Code ='" + hostel1 + "'";
                        string updatequery = " if exists(select*from DailyConsumption_Detail where Item_Code='" + itemcode + "' and Hostel_Code='" + hostel1 + "' and DailyConsumptionMaster_Code='" + mastercode + "') update DailyConsumption_Detail set Access_Date='" + dtaccessdate + "',Access_Time='" + dtaccesstime + "', Hand_Qty='" + handonqty + "',Item_Code='" + itemcode + "',Req_Qty=Req_Qty+ ISNULL('" + usedqty + "', 0),Consumption_Qty=Consumption_Qty+ ISNULL('" + usedqty + "',0),RPU='" + rpu + "', Consumption_Value=Consumption_Value+ ISNULL('" + consumvalue + "',0) where Item_Code='" + itemcode + "' and Hostel_Code='" + hostel1 + "' and DailyConsumptionMaster_Code='" + mastercode + "'  else insert into DailyConsumption_Detail (Access_Date,Access_Time,Item_Code,Hand_Qty,Req_Qty,Consumption_Qty,DailyConsumptionMaster_Code,Hostel_Code,RPU,Consumption_Value) values ('" + dtaccessdate + "','" + dtaccesstime + "','" + itemcode + "','" + handonqty + "','" + usedqty + "','" + usedqty + "','" + mastercode + "','" + hostel1 + "','" + rpu + "','" + consumvalue + "')";
                        int upd = d2.update_method_wo_parameter(updatequery, "Text");
                        if (upd != 0)
                        {
                            save = true;
                        }
                        string currentavlqty = "";
                        if (Convert.ToString(handonqty).Trim() != "" && Convert.ToString(handonqty).Trim() != "0")
                        {
                            double inval = Convert.ToDouble(handonqty);
                            if (inval >= Convert.ToDouble(usedqty))
                            {
                                inval = inval - Convert.ToDouble(usedqty);
                                currentavlqty = Convert.ToString(inval);
                            }
                            else
                            {
                                currentavlqty = "0";
                            }
                        }
                        else
                        {
                            currentavlqty = "0";
                        }
                        string stockdetails = " if exists (select * from Stock_Detail where item_code ='" + itemcode + "' and Dept_Code ='" + hostel1 + "') update Stock_Detail set Access_Date ='" + dtaccessdate + "',Access_Time ='" + dtaccesstime + "', InvUser_Code ='" + usercode + "' ,College_Code ='" + collegecode1 + "',AvlQty ='" + currentavlqty + "' where item_code ='" + itemcode + "' and Dept_Code ='" + hostel1 + "'";
                        int insertstock = d2.update_method_wo_parameter(stockdetails, "Text");
                        if (insertstock != 0)
                        {
                            stock = true;
                        }
                        // }
                    }
                }
                if (save == true && stock == true && master == true)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Saved Successfully";
                    lblalerterr.Visible = true;
                    pop_mess.Visible = false;
                    btn_messgo_Click(sender, e);
                }
            }
        }
        catch
        { }
    }
    protected void btn_messexit_Click(object sender, EventArgs e)
    {
        pop_mess.Visible = false;
    }
    protected void txt_quantity_textchange(object sender, EventArgs e)
    {
        bool check = false;
        if (SelectdptGrid.Rows.Count > 0)
        {
            for (int i = 0; i < SelectdptGrid.Rows.Count; i++)
            {
                double handonqty = Convert.ToDouble((SelectdptGrid.Rows[i].FindControl("lbl_avlqty") as Label).Text);
                double usedqty = Convert.ToDouble((SelectdptGrid.Rows[i].FindControl("txt_quantity") as TextBox).Text);
                if (handonqty >= usedqty)
                {
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Used quantity greater then hand on quantity";
                    lblalerterr.Visible = true;
                    (SelectdptGrid.Rows[i].FindControl("txt_quantity") as TextBox).Text = "";
                }
            }
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
            //  loadlookupitem();
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
            //loadlookupitem();
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
            for (int i = 0; i < cbl_itemheader3.Items.Count; i++)
            {
                if (cbl_itemheader3.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_itemheader3.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_itemheader3.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemheader.Trim() != "")
            {
                string query = "";
                //   query = "select distinct t.TextCode,t.TextVal  from TextValTable t,item_master i where t.TextCode=i.subheader_code and itemheader_code in ('" + itemheader + "') and college_code in ('" + collegecode1 + "')";
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
    protected void cb_sub_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_sub.Checked == true)
            {
                for (int i = 0; i < cbl_sub.Items.Count; i++)
                {
                    cbl_sub.Items[i].Selected = true;
                }
                txt_sub.Text = "Sub Header Name(" + (cbl_sub.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sub.Items.Count; i++)
                {
                    cbl_sub.Items[i].Selected = false;
                }
                txt_sub.Text = "--Select--";
            }
            // loadsubheadername();
            loaditem();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_sub_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_sub.Text = "--Select--";
            cb_sub.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_sub.Items.Count; i++)
            {
                if (cbl_sub.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_sub.Text = "Sub Header Name(" + commcount.ToString() + ")";
                if (commcount == cbl_sub.Items.Count)
                {
                    cb_sub.Checked = true;
                }
            }
            loaditem();
        }
        catch (Exception ex)
        {
        }
    }
    public void loadsub()
    {
        try
        {
            cbl_sub.Items.Clear();
            string itemheader = "";
            for (int i = 0; i < cbl_headername.Items.Count; i++)
            {
                if (cbl_headername.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_headername.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_headername.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemheader.Trim() != "")
            {
                string query = "";
                //query = "select distinct t.TextCode,t.TextVal  from TextValTable t,item_master i where t.TextCode=i.subheader_code and itemheader_code in ('" + itemheader + "') and college_code in ('" + collegecode1 + "')";
                query = "select distinct t.MasterCode,t.MasterValue  from CO_MasterValues t,IM_ItemMaster i where t.MasterCode=i.subheader_code and ItemHeaderCode in ('" + itemheader + "') and CollegeCode in ('" + collegecode1 + "')";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                // ds.Clear();
                // ds = d2.BindItemCodeAll(itemheader);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sub.DataSource = ds;
                    cbl_sub.DataTextField = "MasterValue";
                    cbl_sub.DataValueField = "MasterCode";
                    cbl_sub.DataBind();
                    if (cbl_sub.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_sub.Items.Count; i++)
                        {
                            cbl_sub.Items[i].Selected = true;
                        }
                        txt_sub.Text = "Sub Header Name(" + cbl_sub.Items.Count + ")";
                    }
                    if (cbl_sub.Items.Count > 5)
                    {
                        Panel3.Width = 300;
                        Panel3.Height = 300;
                    }
                }
                else
                {
                    txt_sub.Text = "--Select--";
                }
            }
            else
            {
                txt_sub.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void txt_todaydate_textchanged(object sender, EventArgs e)
    {
        try
        {
            string fromdate = "";
            string todate = "";
            lbl_error1.Visible = false;
            fromdate = DateTime.Now.ToString("dd/MM/yyyy");
            todate = txt_todaydate.Text;
            if (fromdate != "" && fromdate != null && todate != "" && todate != null)
            {
                string[] spiltfrom = fromdate.Split('/');
                string[] spitto = todate.Split('/');
                DateTime from = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
                DateTime to = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);
                string todate1 = DateTime.Now.ToString("dd/MM/yyyy");
                string[] todate2 = todate1.Split('/');
                DateTime todate3 = Convert.ToDateTime(todate2[1] + '/' + todate2[0] + '/' + todate2[2]);
                //if (from > to)// && to <= todate3
                //{
                //    lbl_error1.Visible = true;
                //    lbl_error1.Text = "Please Enter To Date Grater Than From Date";
                //    FpSpread1.Visible = false;
                //    dat.Visible = false;
                //    btn_save.Visible = false;
                //    btn_update.Visible = false;
                //    btn_reset.Visible = false;
                //    dat.Visible = false;
                //    rptprint.Visible = false;
                //    lbl_holiday.Visible = false;
                //    pheaderfilter.Visible = false;
                //    pcolumnorder.Visible = false;
                //}
                if (to > todate3)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Don't Enter Future Date";
                    lblalerterr.Visible = true;
                    txt_todaydate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = ex.ToString();
        }
    }
    protected void rdb_veg_CheckedChanged(object sender, EventArgs e)
    {
    }
    protected void rdb_Nonveg_CheckedChanged(object sender, EventArgs e)
    {
    }
    protected void BindStudentType()
    {
        try
        {
            ddlStudType.Items.Clear();
            ds.Clear();
            string sql = "select StudentType,StudentTypeName from HostelStudentType where CollegeCode='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlStudType.DataSource = ds;
                ddlStudType.DataTextField = "StudentTypeName";
                ddlStudType.DataValueField = "StudentType";
                ddlStudType.DataBind();
                ddlStudType.Items.Add(new ListItem("Common","0"));
            }
            else
                ddlStudType.Items.Add(new ListItem("Common", "0"));
        }
        catch
        {
        }
    }

}
