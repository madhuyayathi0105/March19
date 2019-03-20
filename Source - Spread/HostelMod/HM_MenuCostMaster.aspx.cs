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
using System.Drawing;


public partial class HM_MenuCostMaster : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty; DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
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
            loadmenuname();
            loadpop1menuname();
            cb_menuname.Checked = true;
            cb_pop1menu.Checked = true;
            //txt_searchdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //txt_searchdate.Attributes.Add("readonly", "readonly");
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Attributes.Add("readonly", "readonly");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            date.Attributes.Add("readonly", "readonly");
            txt_pop1date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_pop1date.Attributes.Add("readonly", "readonly");


            Fpspread3.Visible = false;
            div_report.Visible = false;
        }
        btn_go_Click(sender, e);
        lblerror.Visible = false;
        // Fpspread2.Visible = true;
    }
    public void loadmenuname()
    {
        try
        {
            string deptquery = "select  MenuMasterPK,MenuName  from HM_MenuMaster where CollegeCode ='" + collegecode1 + "' order by MenuMasterPK ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            //ds = d2.BindMenuName(collegecode1);
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
                    }
                    txt_menuname.Text = "Menu Name(" + cbl_menuname.Items.Count + ")";
                }
            }
        }
        catch
        {

        }
    }
    //[System.Web.Services.WebMethod]
    //[System.Web.Script.Services.ScriptMethod()]
    //public static List<string> Getname(string prefixText)
    //{
    //    string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    //    using (SqlConnection sqlconn = new SqlConnection(cs))
    //    {
    //        sqlconn.Open();
    //        SqlCommand cmd = new SqlCommand("select distinct MenuName from MenuMaster WHERE MenuName like '" + prefixText + "%' ", sqlconn);
    //        cmd.Parameters.AddWithValue("@store", prefixText);
    //        SqlDataAdapter da = new SqlDataAdapter(cmd);
    //        DataTable dt = new DataTable();
    //        da.Fill(dt);
    //        List<string> name = new List<string>();
    //        for (int i = 0; i < dt.Rows.Count; i++)
    //        {
    //            name.Add(dt.Rows[i]["MenuName"].ToString());
    //        }
    //        return name;
    //    }
    //}

    protected void btnsv_Click(object sender, EventArgs e)
    {
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {

            Printcontrol.Visible = false;
            string mname = "";
            for (int i = 0; i < cbl_menuname.Items.Count; i++)
            {
                if (cbl_menuname.Items[i].Selected == true)
                {
                    if (mname == "")
                    {
                        mname = "" + cbl_menuname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        mname = mname + "'" + "," + "'" + cbl_menuname.Items[i].Value.ToString() + "";
                    }
                }
            }
            string q = "";
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

            if (mname.Trim() != "")
            {

                if (txt_search.Text.Trim() != "")
                {
                    //q = "select Menu_Name,SessionMenu_Code,Qty,Cost,CONVERT(varchar(10),From_Date,103) as From_Date from MenuCost_Master where Menu_Name in('" + txt_search.Text + "') and From_Date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' order by SessionMenu_Code";

                    q = " select mm.MenuMasterPK,mm.MenuName,mc.MenuQty,mc.MenuAmount,mc.Menucost_Date  from HM_MenuCostMaster MC,HM_MenuMaster MM where mm.MenuMasterPK =mc.MenuMasterFK and CollegeCode ='" + collegecode1 + "' and  mm.MenuName in('" + txt_search.Text + "')";
                }
                else
                {
                    q = "select mm.MenuMasterPK,mm.MenuName,mc.MenuQty,mc.MenuAmount,convert(varchar(10),Menucost_Date,103)as Menucost_Date from HM_MenuCostMaster MC,HM_MenuMaster MM where mm.MenuMasterPK =mc.MenuMasterFK and CollegeCode ='" + collegecode1 + "' and mc.MenuMasterFK  in ('" + mname + "') and mc.Menucost_Date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'";

                }

                ds = d2.select_method_wo_parameter(q, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {

                    Fpspread3.Sheets[0].RowCount = 0;
                    Fpspread3.Sheets[0].ColumnCount = 0;
                    Fpspread3.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread3.CommandBar.Visible = false;
                    Fpspread3.Sheets[0].ColumnCount = 5;
                    Fpspread3.Sheets[0].AutoPostBack = true;
                    Fpspread3.Sheets[0].RowHeader.Visible = false;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Columns[0].Width = 50;
                    Fpspread3.Columns[0].Locked = true;

                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Menu Name";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[1].Width = 250;
                    Fpspread3.Columns[1].Locked = true;

                    //Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Menu Name";
                    //Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    //Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    //Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    //Fpspread3.Columns[2].Width = 150;
                    //Fpspread3.Columns[2].Locked = true;

                    FarPoint.Web.Spread.DoubleCellType db = new FarPoint.Web.Spread.DoubleCellType();
                    db.ErrorMessage = "Enter only Numbers";
                    Fpspread3.Columns[2].CellType = db;
                    Fpspread3.Columns[2].BackColor = Color.White;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Quantity";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[2].Width = 100;
                    Fpspread3.Columns[2].Locked = true;

                    FarPoint.Web.Spread.DoubleCellType db1 = new FarPoint.Web.Spread.DoubleCellType();
                    db1.ErrorMessage = "Enter only Numbers";
                    Fpspread3.Columns[3].CellType = db1;
                    Fpspread3.Columns[3].BackColor = Color.White;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Cost";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[3].Width = 100;
                    Fpspread3.Columns[3].Locked = true;

                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Date";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[4].Width = 100;
                    Fpspread3.Columns[4].Locked = true;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Fpspread3.Sheets[0].RowCount++;

                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                        //FarPoint.Web.Spread.CheckBoxCellType chkbox = new FarPoint.Web.Spread.CheckBoxCellType();
                        //chkbox.AutoPostBack = false;
                        //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].CellType = chkbox;
                        //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["MenuName"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["MenuMasterPK"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["MenuQty"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["MenuAmount"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Menucost_Date"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    }
                    Fpspread3.Sheets[0].PageSize = Fpspread3.Sheets[0].RowCount;
                    Fpspread3.Visible = true;
                    lblerror.Visible = false;
                    div_report.Visible = true;
                    div1.Visible = true;
                }
                else
                {
                    lblerror.Visible = true;
                    lblerror.Text = "No Record Found";
                    Fpspread3.Visible = false;
                    div_report.Visible = false;
                    div1.Visible = false;
                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select Menu Name";
                Fpspread3.Visible = false;
                div_report.Visible = false;
                div1.Visible = false;
            }
        }
        catch
        {
        }
    }
    protected void Fpspread1_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = Fpspread3.Sheets[0].ActiveRow.ToString();
            string actcol = Fpspread3.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (Fpspread3.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(Fpspread3.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < Fpspread3.Sheets[0].RowCount; i++)
                        {
                            Fpspread3.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < Fpspread3.Sheets[0].RowCount; i++)
                        {
                            Fpspread3.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch
        {

        }
    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
    protected void cb_menuname_ChekedChange(object sender, EventArgs e)
    {
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
    }
    protected void cbl_menuname_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_menuname.Text = "--Select--";
        cb_menuname.Checked = false;
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
    }

    public void btn_addnew_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = true;
        Fpspread2.Visible = false;
        btn_pop1save.Visible = false;
        btn_pop1exit.Visible = false;
        txt_pop1search.Text = "";
        //btnpop1exit.Visible = false;

    }
    public void clear()
    {
        popwindow1.Visible = true;

        txt_pop1search.Text = "";
        Fpspread2.Visible = false;
        btn_pop1save.Visible = false;
        btn_pop1exit.Visible = false;
    }
    public void viewspd()
    {

        string mname = "";
        for (int i = 0; i < cbl_pop1menu.Items.Count; i++)
        {
            if (cbl_pop1menu.Items[i].Selected == true)
            {
                if (mname == "")
                {
                    mname = "" + cbl_pop1menu.Items[i].Value.ToString() + "";
                }
                else
                {
                    mname = mname + "'" + "," + "'" + cbl_pop1menu.Items[i].Value.ToString() + "";
                }
            }
        }
        string q = "";
        if (txt_pop1search.Text.Trim() != "")
        {
            q = "select MenuName ,MenuMasterPK from HM_MenuMaster where MenuName in('" + txt_pop1search.Text + "')";
        }
        else
        {
            q = "select MenuName ,MenuMasterPK from HM_MenuMaster where MenuMasterPK in('" + mname + "')";
        }

        //if (mname.Trim() != "")
        // {
        // string q = "";
        // q = "select MenuName ,menucode from MenuMaster where MenuCode in('" + mname + "')";
        ds = d2.select_method_wo_parameter(q, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {

            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnCount = 0;
            Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread2.CommandBar.Visible = false;
            Fpspread2.Sheets[0].ColumnCount = 4;
            Fpspread2.Sheets[0].AutoPostBack = false;
            Fpspread2.Sheets[0].RowHeader.Visible = false;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Columns[0].Width = 50;
            Fpspread2.Columns[0].Locked = true;


            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Menu Name";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            Fpspread2.Columns[1].Width = 150;
            Fpspread2.Columns[1].Locked = true;

            FarPoint.Web.Spread.DoubleCellType db = new FarPoint.Web.Spread.DoubleCellType();
            db.ErrorMessage = "Enter only Numbers";
            Fpspread2.Columns[2].CellType = db;
            Fpspread2.Columns[2].BackColor = Color.White;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Quantity";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            Fpspread2.Columns[2].Width = 100;
            Fpspread2.Columns[2].Locked = false;

            FarPoint.Web.Spread.DoubleCellType db1 = new FarPoint.Web.Spread.DoubleCellType();
            db1.ErrorMessage = "Enter only Numbers";
            Fpspread2.Columns[3].CellType = db1;
            Fpspread2.Columns[3].BackColor = Color.White;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Cost";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            Fpspread2.Columns[3].Width = 100;
            Fpspread2.Columns[3].Locked = false;

            //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Change Date";
            //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            //Fpspread2.Columns[5].Width = 150;
            //Fpspread2.Columns[5].Locked = true;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Fpspread2.Sheets[0].RowCount++;

                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                //FarPoint.Web.Spread.CheckBoxCellType chkbox = new FarPoint.Web.Spread.CheckBoxCellType();
                //chkbox.AutoPostBack = false;
                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = chkbox;
                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["menuname"]);
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["MenuMasterPK"]);
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i][""]);
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i][""]);
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;


            }
            Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
            Fpspread2.Visible = true;

            lblerror1.Visible = false;
            btn_pop1save.Visible = true;
            btn_pop1exit.Visible = true;
        }
        else
        {
            lblerror1.Visible = true;
            lblerror1.Text = "No Record Found";
            Fpspread2.Visible = false;
            btn_pop1save.Visible = false;
            btn_pop1exit.Visible = false;

        }
    }
    public void cb_pop1menu_ChekedChange(object sender, EventArgs e)
    {
        if (cb_pop1menu.Checked == true)
        {
            for (int i = 0; i < cbl_pop1menu.Items.Count; i++)
            {
                cbl_pop1menu.Items[i].Selected = true;
            }
            txt_pop1menu.Text = "Menu Name(" + (cbl_pop1menu.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_pop1menu.Items.Count; i++)
            {
                cbl_pop1menu.Items[i].Selected = false;
            }
            txt_pop1menu.Text = "--Select--";
        }
    }
    public void cbl_pop1menu_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_pop1menu.Text = "--Select--";
        cb_pop1menu.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_pop1menu.Items.Count; i++)
        {
            if (cbl_pop1menu.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txt_pop1menu.Text = "Menu Name(" + commcount.ToString() + ")";
            if (commcount == cbl_pop1menu.Items.Count)
            {
                cb_pop1menu.Checked = true;
            }
        }
    }
    public void btn_pop1save_Click(object sender, EventArgs e)
    {
        try
        {
            bool saveflage = false;
            string getdate = Convert.ToString(txt_pop1date.Text);
            string[] splitdate = getdate.Split('-');
            splitdate = splitdate[0].Split('/');
            DateTime dt = new DateTime();

            if (splitdate.Length > 0)
            {
                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            }

            string getday = dt.ToString("MM/dd/yyyy");
            Fpspread2.SaveChanges();
            for (int i = 0; i < Fpspread2.Sheets[0].RowCount; i++)
            {
                string menuname1 = Convert.ToString(Fpspread2.Sheets[0].Cells[i, 1].Text);
                string menupk = Convert.ToString(Fpspread2.Sheets[0].Cells[i, 1].Tag);


                string Qunty = Convert.ToString(Fpspread2.Sheets[0].Cells[i, 2].Text);
                string amount = Convert.ToString(Fpspread2.Sheets[0].Cells[i, 3].Text);


                if (Qunty.Trim() != "" && amount.Trim() != "")
                {
                    //string q = "if exists (select * from MenuCost_Master where SessionMenu_Code ='" + menucode1 + "' and From_Date ='" + dt.ToString("MM/dd/yyyy") + "')update MenuCost_Master set Qty='" + Qunty + "',Cost ='" + amount + "',Menu_Name='" + menuname1 + "' where SessionMenu_Code ='" + menucode1 + "' and From_Date ='" + dt.ToString("MM/dd/yyyy") + "' else insert into MenuCost_Master (SessionMenu_Code,From_Date,Qty,Cost,Menu_Name) values ('" + menucode1 + "','" + dt.ToString("MM/dd/yyyy") + "','" + Qunty + "','" + amount + "','" + menuname1 + "')";

                    string q = "     if exists (select * from HM_MenuCostMaster where MenuMasterFK ='" + menupk + "' and Menucost_Date='" + getday + "')update HM_MenuCostMaster set MenuQty ='" + Qunty + "',MenuAmount ='" + amount + "',Menucost_Date='" + getday + "'  where MenuMasterFK ='" + menupk + "' else insert into HM_MenuCostMaster (MenuAmount,MenuMasterFK,MenuQty,Menucost_Date) values ('" + amount + "','" + menupk + "' ,'" + Qunty + "','" + getday + "')";
                    int ins = d2.update_method_wo_parameter(q, "Text");
                    if (ins != 0)
                    {
                        saveflage = true;
                    }
                }

            }
            if (saveflage == true)
            {
                lblerror1.Visible = false;
                imgdiv2.Visible = true;
                lblalerterr.Text = "Saved Successfully";
                clear();

            }
            else
            {

                lblerror1.Visible = false;
                imgdiv2.Visible = true;
                lblalerterr.Text = "Please Enter The Field";
            }
        }

        catch (Exception ex)
        {

        }
    }
    public void loadpop1menuname()
    {
        try
        {
            string deptquery = "select  MenuMasterPK,MenuName  from HM_MenuMaster where CollegeCode ='" + collegecode1 + "' order by MenuMasterPK ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            // ds = d2.BindMenuName(collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_pop1menu.DataSource = ds;
                cbl_pop1menu.DataTextField = "MenuName";
                cbl_pop1menu.DataValueField = "MenuMasterPK";
                cbl_pop1menu.DataBind();
                if (cbl_pop1menu.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_pop1menu.Items.Count; i++)
                    {
                        cbl_pop1menu.Items[i].Selected = true;
                    }
                    txt_pop1menu.Text = "Menu Name(" + cbl_pop1menu.Items.Count + ")";
                }
            }
        }
        catch
        {
        }
    }
    public void btn_pop1go_Click(object sender, EventArgs e)
    {
        viewspd();

    }
    public void btn_pop1exit_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
    }
    protected void Fpspread2_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = Fpspread2.Sheets[0].ActiveRow.ToString();
            string actcol = Fpspread2.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (Fpspread2.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(Fpspread2.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < Fpspread2.Sheets[0].RowCount; i++)
                        {
                            Fpspread2.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < Fpspread2.Sheets[0].RowCount; i++)
                        {
                            Fpspread2.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch
        {

        }
    }

    public string menucode1 { get; set; }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;

    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct MenuName from HM_MenuMaster WHERE MenuName like '" + prefixText + "%' order by MenuName ";
        //string query = "select distinct Store_Name from StoreMaster WHERE Store_Name like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    protected void imagebtnpop1close_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;

    }
    protected void txtexcelname_TextChanged(object sender, EventArgs e)
    {
        try
        {

            txt_excelname.Visible = true;
            btn_Excel.Visible = true;
            btn_printmaster.Visible = true;
            lbl_reportname.Visible = true;
            lbl_norec.Visible = false;
            //btn_Excel.Focus();
        }
        catch (Exception ex)
        {

        }
    }
    //public void btnExcel_Click(object sender, EventArgs e)
    //{
    //    try
    //    {

    //        string report = txt_excelname.Text;
    //        if (report.ToString().Trim() != "")
    //        {
    //            //  FpSpread1.Sheets[0].Columns[1].Visible = false;
    //            d2.printexcelreport(Fpspread3, report);
    //            lbl_norec.Visible = false;
    //        }
    //        else
    //        {
    //            lbl_norec.Text = "Please Enter Your Report Name";
    //            lbl_norec.Visible = true;
    //        }
    //        btn_Excel.Focus();
    //    }

    //    catch (Exception ex)
    //    {
    //        lbl_norec.Text = ex.ToString();
    //    }

    //}
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txt_excelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread3, reportname);
                lbl_norec.Visible = false;
            }
            else
            {
                lbl_norec.Text = "Please Enter Your Report Name";
                lbl_norec.Visible = true;
                txt_excelname.Focus();
            }
        }
        catch
        {

        }
    }
    public void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Menu Cost Master Report";
            string pagename = "HM_MenuCostMaster.aspx";
            Printcontrol.loadspreaddetails(Fpspread3, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }
    protected void txt_fromdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            datevalidate(txt_fromdate, txt_todate);
        }
        catch (Exception ex)
        {
        }
        // PopupMessage("Enter FromDate less than or equal to the ToDate", cv_fromtodt1);
    }
    protected void txt_todate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            datevalidate(txt_fromdate, txt_todate);
        }
        catch (Exception ex)
        {
        }
        // PopupMessage("Enter ToDate greater than or equal to the FromDate", cv_fromtodt2);
    }
    public void datevalidate(TextBox txt1, TextBox txt2)
    {
        try
        {
            if (txt1.Text != "" && txt2.Text != "")
            {
                //txt_leavedays.Text = "";
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
                    lblalerterr.Text = "Select ToDate greater than or equal to the FromDate ";
                    txt2.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txt1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //txt_leavedays.Text = "";
                    //txt_rebatedays.Text = "";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void Cell_Click1(object sender, EventArgs e)
    {
        try
        {
            check = true;
        }
        catch
        {
        }
    }
    protected void Fpspread3_render(object sender, EventArgs e)
    {
        try
        {
            if (check == true)
            {
                string activerow = "";
                string activecol = "";

                activerow = Fpspread3.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread3.ActiveSheetView.ActiveColumn.ToString();
                if (activerow.Trim() != "" && activecol.Trim() != "")
                {
                    string menuname = Convert.ToString(Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    string qty = Convert.ToString(Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                    string cost = Convert.ToString(Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                    string date1 = Convert.ToString(Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                    string MenuMasterFK = Convert.ToString(Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                    Session["MenuMasterFK"] = Convert.ToString(MenuMasterFK);

                    lbl_spmenu.Text = menuname;
                    txt_qty.Text = qty;
                    txt_cost.Text = cost;
                    date.Text = date1;
                }
                editmenu_div.Visible = true;

            }
        }
        catch { }
    }
    protected void imagebtn_Click(object sender, EventArgs e)
    {

        editmenu_div.Visible = false;

    }
    protected void btn_update_click(object sender, EventArgs e)
    {
        string menuname = Convert.ToString(lbl_spmenu.Text);
        string qty = Convert.ToString(txt_qty.Text);
        string cost = Convert.ToString(txt_cost.Text);
        string date1 = Convert.ToString(date.Text);
        string[] split = date1.Split('/');
        DateTime dt = new DateTime();
        dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

        //string insert = "update MenuCost_Master set Qty='" + qty + "',Cost='" + cost + "',from_date='" + dt.ToString("MM/dd/yyyy") + "' where Menu_Name='" + menuname + "'";

        string insert = "update HM_MenuCostMaster set MenuQty='" + qty + "', MenuAmount='" + cost + "',Menucost_Date='" + dt.ToString("MM/dd/yyyy") + "' where MenuMasterFK='" + Convert.ToString(Session["MenuMasterFK"]) + "' and Menucost_Date='" + dt.ToString("MM/dd/yyyy") + "' ";
        int update = d2.update_method_wo_parameter(insert, "Text");
        if (update != 0)
        {
            lblerror1.Visible = false;
            imgdiv2.Visible = true;
            lblalerterr.Text = "Updated Successfully";
            editmenu_div.Visible = false;

            lbl_spmenu.Text = "";
            txt_qty.Text = "";
            txt_cost.Text = "";
            date.Text = "";

        }
    }
    protected void btn_delete_click(object sendee, EventArgs e)
    {
        if (btn_delete.Text == "Delete")
        {
            surediv.Visible = true;
            lbl_sure.Text = "Do you want to Delete this Record?";

        }

    }
    protected void btn_exit_click(object sendee, EventArgs e)
    {
        editmenu_div.Visible = false;
    }

    //theivamani 6.11.15

    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
        btn_go_Click(sender, e);

    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        imgdiv2.Visible = false;

    }

    public void delete()
    {
        try
        {
            surediv.Visible = false;
            string menuname = Convert.ToString(lbl_spmenu.Text);
            string delete = "delete HM_MenuCostMaster where MenuMasterFK='" + Convert.ToString(Session["MenuMasterFK"]) + "'";
            int update = d2.update_method_wo_parameter(delete, "Text");
            if (update != 0)
            {
                lblerror1.Visible = false;
                imgdiv2.Visible = true;
                lblalerterr.Text = "Deleted Successfully";
                editmenu_div.Visible = false;
            }
        }
        catch
        {

        }
    }
}
