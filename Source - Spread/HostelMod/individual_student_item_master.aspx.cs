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
public partial class individual_student_item_master : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    bool check = false;
    string led = "";
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

            loaditem();
            loaditem1();
            loaddesc();
            txt_searchby.Visible = true;
            rdb_monthly.Checked = true;
            bindledger();
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread1.Visible = false;
            btn_go_Click(sender, e);
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

    protected void ddl_type_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        btn_delete.Visible = false;
        btn_update.Visible = false;
        btn_save.Visible = true;
        poperrjs.Visible = true;
        bindledger();
        loaditem();
        loaditem1();
        loaddesc();
        txt_cost.Text = "";
        //rdb_monthly.Checked = false;
        //rdb_yearly.Checked = false;
        //rdb_yearly.Checked = false;

    }
    protected void btnexit1_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }


    protected void bindledger()
    {
        try
        {
            ds.Dispose();
            ds.Reset();
            led = "Select Fee_Code,Fee_Type from Fee_Info F,Acctheader H,Acctinfo I WHERE F.Header_ID = H.Header_ID AND H.Acct_ID = I.Acct_ID AND fee = 1 AND College_Code = '" + Session["collegecode"].ToString() + "' and fee_type not in ('Cash','Misc','Income & Expenditure') and fee_type not in  (select bankname from bank_master1) order by fee_type";
            ds = d2.select_method_wo_parameter(led, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_ledger.Items.Clear();
                ddl_ledger.DataSource = ds;
                ddl_ledger.DataTextField = "Fee_Type";
                ddl_ledger.DataValueField = "Fee_Code";
                ddl_ledger.DataBind();
                ddl_ledger.Items.Insert(0, "Select");
                //ddl_rrl.Items.Insert(ddl_rrl.Items.Count,"Others");               
            }
            else
            {
                ddl_ledger.Items.Clear();
                ddl_ledger.Items.Insert(0, "Select");
                ddl_ledger.Items.Insert(ddl_ledger.Items.Count, "Others");
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
    protected void loaditem1()
    {
        ddl_itemname1.Items.Clear();
        ds.Clear();
        string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='Sitem' and college_code ='" + collegecode1 + "'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_itemname1.DataSource = ds;
            ddl_itemname1.DataTextField = "TextVal";
            ddl_itemname1.DataValueField = "TextCode";
            ddl_itemname1.DataBind();
            ddl_itemname1.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            ddl_itemname1.Items.Insert(0, new ListItem("Select", "0"));
        }


    }
    protected void btn_plus_itemname_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = true;
        panel_reason.Visible = true;

    }
    protected void btn_minus_itemname_Click(object sender, EventArgs e)
    {
        if (ddl_itemname1.SelectedIndex == -1)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No Items found";
        }
        else if (ddl_itemname1.SelectedIndex == 0)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Select any item";
        }
        else if (ddl_itemname1.SelectedIndex != 0)
        {
            string sql = "delete from TextValTable where TextCode='" + ddl_itemname1.SelectedItem.Value.ToString() + "' and TextCriteria='Sitem' and college_code='" + collegecode1 + "' ";
            int delete = d2.update_method_wo_parameter(sql, "Text");
            if (delete != 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Deleted Successfully";
                loaditem();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "No items found";
            }
            loaditem1();
        }
        //else if (ddl_reason.SelectedIndex == -1)
        //{
        //    imgdiv3.Visible = true;
        //    lbl_error.Text = "No records found";
        //}
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No items found";
        }

    }
    protected void btn_exit_itemname_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = false;
        panel_reason.Visible = false;
    }
    protected void btn_add_itemname_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_itemname2.Text != "")
            {
                string sql = "if exists ( select * from TextValTable where TextVal ='" + txt_itemname2.Text + "' and TextCriteria ='Sitem' and college_code ='" + collegecode1 + "') update TextValTable set TextVal ='" + txt_itemname2.Text + "' where TextVal ='" + txt_itemname2.Text + "' and TextCriteria ='Sitem' and college_code ='" + collegecode1 + "' else insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + txt_itemname2.Text + "','Sitem','" + collegecode1 + "')";
                int insert = d2.update_method_wo_parameter(sql, "Text");
                if (insert != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Saved Successfully";
                    txt_itemname2.Text = "";
                    imgdiv3.Visible = false;
                    panel_reason.Visible = false;
                    loaditem();
                }
                loaditem1();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Enter the Item";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_exit_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;

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
    public void loaditem()
    {

        try
        {
            cbl_itemname.Items.Clear();

            string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='Sitem' and college_code ='" + collegecode1 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(sql, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_itemname.DataSource = ds;
                cbl_itemname.DataTextField = "TextVal";
                cbl_itemname.DataValueField = "TextCode";
                cbl_itemname.DataBind();
                if (cbl_itemname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_itemname.Items.Count; i++)
                    {
                        cbl_itemname.Items[i].Selected = true;
                    }
                    txt_itemname.Text = "Item Name(" + cbl_itemname.Items.Count + ")";
                    cb_itemname.Checked = true;
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
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='Sitem' ";
        dw = dn.select_method_wo_parameter(sql, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["TextVal"].ToString());
            }
        }
        return name;

    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string itemheadercode = "";
            for (int i = 0; i < cbl_itemname.Items.Count; i++)
            {
                if (cbl_itemname.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_itemname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cbl_itemname.Items[i].Value.ToString() + "";
                    }
                }
            }

            if (itemheadercode.Trim() != "")
            {
                string selectquery = "";
                if (txt_searchby.Text.Trim() != "")
                {
                    selectquery = "select StudItemCode,t.TextVal,case when PayMode=1 then 'Monthly' when PayMode =2 then 'Yearly' when PayMode =3 then 'Semester' end as paymode,StudItemCost,(Select Fee_Type from Fee_Info F,Acctheader H,Acctinfo I WHERE F.Header_ID = H.Header_ID AND H.Acct_ID = I.Acct_ID AND fee = 1 AND Fee_Code =StudItemFeeLed and fee_type not in ('Cash','Misc','Income & Expenditure') and fee_type not in  (select bankname from bank_master1))as StudItemFeeLed,(select TextVal from TextValTable where TextCriteria ='SIdec' and TextCode= ISNULL( StudItemDesc,0))as StudItemDesc  from StudItemMaster s,TextValTable t where s.StudItemCode =t.TextCode and StudItemCode =CONVERT(numeric ,isnull( (select Textcode from TextValTable where TextVal ='" + txt_searchby.Text + "'),0))";
                }
                else
                {
                    selectquery = "select StudItemCode,t.TextVal,case when PayMode=1 then 'Monthly' when PayMode =2 then 'Yearly' when PayMode =3 then 'Semester' end as paymode,StudItemCost,(Select Fee_Type from Fee_Info F,Acctheader H,Acctinfo I WHERE F.Header_ID = H.Header_ID AND H.Acct_ID = I.Acct_ID AND fee = 1 AND Fee_Code =StudItemFeeLed and fee_type not in ('Cash','Misc','Income & Expenditure') and fee_type not in  (select bankname from bank_master1))as StudItemFeeLed,(select TextVal from TextValTable where TextCriteria ='SIdec' and TextCode= ISNULL( StudItemDesc,0))as StudItemDesc  from StudItemMaster s,TextValTable t where s.StudItemCode =t.TextCode and StudItemCode in ('" + itemheadercode + "')";
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
                    Fpspread1.Sheets[0].ColumnCount = 6;
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

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Item Name";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[1].Width = 100;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Mode Of Payment";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[2].Width = 100;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Cost";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[3].Width = 50;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Ledger";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[4].Width = 150;


                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Description";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;


                    FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();

                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        Fpspread1.Sheets[0].RowCount++;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["TextVal"]);
                        //  Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["StudItemCode"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["PayMode"]);
                        //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["rpu"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["StudItemCost"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].CellType = txtcell;
                        // Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Stock_Value"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["StudItemFeeLed"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["StudItemDesc"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";


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
                lbl_error.Text = "Please Select Any one Item Name";
            }
        }
        catch
        {

        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        savedetails();
        btn_go_Click(sender, e);
        ddl_desc.SelectedItem.Text = "Select";
        ddl_itemname1.SelectedItem.Text = "Select";
        txt_cost.Text = "";
        ddl_ledger.SelectedItem.Text = "Select";
        rdb_monthly.Checked = false;
        rdb_yearly.Checked = false;
        rdb_yearly.Checked = false;


    }
    protected void savedetails()
    {
        try
        {
            string pay = "";
            string itemcode = Convert.ToString(ddl_itemname1.SelectedItem.Value);
            if (rdb_monthly.Checked == true)
            {
                pay = "1";
            }
            else if (rdb_yearly.Checked == true)
            {
                pay = "2";
            }
            else if (rdb_sem.Checked == true)
            {
                pay = "3";
            }
            string cost = Convert.ToString(txt_cost.Text);
            string ledger = Convert.ToString(ddl_ledger.SelectedItem.Value);
            string desc = Convert.ToString(ddl_desc.SelectedItem.Value);
            string query = "if exists (select * from StudItemMaster where  StudItemCode ='" + itemcode + "')update StudItemMaster set PayMode ='" + pay + "',StudItemCost ='" + cost + "',StudItemFeeLed ='" + ledger + "',StudItemDesc ='" + desc + "' where  StudItemCode ='" + itemcode + "' else insert into StudItemMaster (StudItemCode,PayMode,StudItemCost,StudItemFeeLed,StudItemDesc) values ('" + itemcode + "','" + pay + "','" + cost + "','" + ledger + "','" + desc + "')";
            //string query = "if exists (select * from StudItemMaster where  StudItemCode ='" + itemcode + "')update StudItemMaster set PayMode ='" + pay + "',StudItemCost ='" + cost + "',StudItemFeeLed ='" + ledger + "' where  StudItemCode ='" + itemcode + "' and PayMode ='" + pay + "' and StudItemCost ='" + cost + "' and StudItemFeeLed ='" + ledger + "' else insert into StudItemMaster (StudItemCode,PayMode,StudItemCost,StudItemFeeLed) values ('" + itemcode + "','" + pay + "','" + cost + "','" + ledger +"')";
            int iv = d2.update_method_wo_parameter(query, "Text");
            if (iv != 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Saved Successfully";
                poperrjs.Visible = false;
            }
        }
        catch (Exception ex)
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

    protected void Fpspread1_render(object sender, EventArgs e)
    {
        try
        {
            if (check == true)
            {

                poperrjs.Visible = true;
                btn_delete.Visible = true;
                btn_update.Visible = true;
                btn_save.Visible = false;
                string activerow = "";
                string activecol = "";
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();

                if (activerow.Trim() != "")
                {

                    string itemname = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    string modeofpayment = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                    string cost = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                    string ledger = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                    string desc = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
                    ddl_itemname1.SelectedItem.Text = Convert.ToString(itemname);
                    txt_cost.Text = Convert.ToString(cost);
                    ddl_ledger.SelectedItem.Text = Convert.ToString(ledger);
                    ddl_desc.SelectedItem.Text = Convert.ToString(desc);
                    if (modeofpayment == "Monthly")
                    {
                        rdb_monthly.Checked = true;
                        rdb_yearly.Checked = false;
                        rdb_sem.Checked = false;
                    }
                    else if (modeofpayment == "Yearly")
                    {
                        rdb_yearly.Checked = true;
                        rdb_monthly.Checked = false;
                        rdb_sem.Checked = false;
                    }
                    else if (modeofpayment == "Semester")
                    {
                        rdb_sem.Checked = true;
                        rdb_yearly.Checked = false;
                        rdb_monthly.Checked = false;
                    }
                    loaditem1();
                    ddl_itemname1.SelectedIndex = ddl_itemname1.Items.IndexOf(ddl_itemname1.Items.FindByText(itemname));
                    bindledger();
                    ddl_ledger.SelectedIndex = ddl_ledger.Items.IndexOf(ddl_ledger.Items.FindByText(ledger));
                    loaddesc();
                    ddl_desc.SelectedIndex = ddl_desc.Items.IndexOf(ddl_desc.Items.FindByText(desc));
                }
            }
        }
        catch
        {

        }

    }
    protected void loaddesc()
    {
        ddl_desc.Items.Clear();
        ds.Clear();
        string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='SIdec' and college_code ='" + collegecode1 + "'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_desc.DataSource = ds;
            ddl_desc.DataTextField = "TextVal";
            ddl_desc.DataValueField = "TextCode";
            ddl_desc.DataBind();
            ddl_desc.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            ddl_desc.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
    protected void btn_plus_desc_Click(object sender, EventArgs e)
    {
        imgdiv4.Visible = true;
        panel_desc.Visible = true;

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
        btn_minus_desc_Click(sender, e);
    }
    protected void btn_minus_desc_Click(object sender, EventArgs e)
    {
        if (btn_minus_desc.Text == "-")
        {
            surediv.Visible = true;
            lbl_sure.Text = "Do you want to Delete this Record?";
        }
    }
    protected void delete()
    {
        surediv.Visible = false;
        if (ddl_desc.SelectedIndex == -1)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No Descriptions found";
        }
        else if (ddl_desc.SelectedIndex == 0)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Select any description";
        }
        else if (ddl_desc.SelectedIndex != 0)
        {
            string sql = "delete from TextValTable where TextCode='" + ddl_desc.SelectedItem.Value.ToString() + "' and TextCriteria='SIdec' and college_code='" + collegecode1 + "' ";
            int delete = d2.update_method_wo_parameter(sql, "Text");
            if (delete != 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Deleted Successfully";

            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "No Descriptions found";
            }
            loaddesc();
        }
        //else if (ddl_reason.SelectedIndex == -1)
        //{
        //    imgdiv3.Visible = true;
        //    lbl_error.Text = "No records found";
        //}
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No Descriptions found";
        }

    }
    protected void btn_add_desc_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_desc.Text != "")
            {
                string sql = "if exists ( select * from TextValTable where TextVal ='" + txt_desc.Text + "' and TextCriteria ='SIdec' and college_code ='" + collegecode1 + "') update TextValTable set TextVal ='" + txt_desc.Text + "' where TextVal ='" + txt_desc.Text + "' and TextCriteria ='SIdec' and college_code ='" + collegecode1 + "' else insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + txt_desc.Text + "','SIdec','" + collegecode1 + "')";
                int insert = d2.update_method_wo_parameter(sql, "Text");
                if (insert != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Saved Successfully";
                    txt_desc.Text = "";
                    imgdiv4.Visible = false;
                    panel_desc.Visible = false;

                }
                loaddesc();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Enter the Description";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_exit_desc_Click(object sender, EventArgs e)
    {
        imgdiv4.Visible = false;
        panel_desc.Visible = false;
    }
    protected void btn_update_Click(object sender, EventArgs e)
    {

        try
        {
            string itemcode = Convert.ToString(ddl_itemname1.SelectedItem.Value);
            string pay = "";
            if (rdb_monthly.Checked == true)
            {
                pay = "1";
            }
            else if (rdb_yearly.Checked == true)
            {
                pay = "2";
            }
            else if (rdb_sem.Checked == true)
            {
                pay = "3";
            }
            string cost = Convert.ToString(txt_cost.Text);
            string ledger = Convert.ToString(ddl_ledger.SelectedItem.Value);
            string desc = Convert.ToString(ddl_desc.SelectedItem.Value);
            string query1 = "if exists (select * from StudItemMaster where  StudItemCode ='" + itemcode + "')update StudItemMaster set PayMode ='" + pay + "',StudItemCost ='" + cost + "',StudItemFeeLed ='" + ledger + "',StudItemDesc ='" + desc + "' where  StudItemCode ='" + itemcode + "' else insert into StudItemMaster (StudItemCode,PayMode,StudItemCost,StudItemFeeLed,StudItemDesc) values ('" + itemcode + "','" + pay + "','" + cost + "','" + ledger + "','" + desc + "')";
            int iv = d2.update_method_wo_parameter(query1, "Text");
            if (iv != 0)
            {
                imgdiv2.Visible = true;
                btn_go_Click(sender, e);
                loaditem();
                loaditem1();
                loaddesc();
                bindledger();
                lbl_alert.Text = "Updated Successfully";
                poperrjs.Visible = false;
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
            string itemcode = Convert.ToString(ddl_itemname1.SelectedItem.Value);
            string pay = "";
            if (rdb_monthly.Checked == true)
            {
                pay = "1";
            }
            else if (rdb_yearly.Checked == true)
            {
                pay = "2";
            }
            else if (rdb_sem.Checked == true)
            {
                pay = "3";
            }
            string cost = Convert.ToString(txt_cost.Text);
            string ledger = Convert.ToString(ddl_ledger.SelectedItem.Value);
            string desc = Convert.ToString(ddl_desc.SelectedItem.Value);
            string query2 = "delete from StudItemMaster where StudItemCode ='" + itemcode + "'";
            int iv = d2.update_method_wo_parameter(query2, "Text");
            if (iv != 0)
            {
                imgdiv2.Visible = true;
                btn_go_Click(sender, e);
                loaditem();
                loaditem1();
                loaddesc();
                bindledger();
                lbl_alert.Text = "Deleted Successfully";
                poperrjs.Visible = false;
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
            string degreedetails = "Individual Student Item Master";
            string pagename = "individual_student_item_master.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }

    }
}


