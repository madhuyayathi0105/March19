using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using InsproDataAccess;
using System.Drawing;
using System.Configuration;

public partial class HostelMod_GymCostMaster : System.Web.UI.Page
{
    string usercode = string.Empty;
    static string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string sessstream = string.Empty;

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet dsprint = new DataSet();
    ArrayList colord = new ArrayList();
    DAccess2 queryObject = new DAccess2();
    DAccess2 da = new DAccess2();
    DataView dvhead = new DataView();
    DataSet dscol = new DataSet();
    Hashtable grandtotal = new Hashtable();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    Dictionary<string, string> dicgymList = new Dictionary<string, string>();
    string year = string.Empty;
    string term = string.Empty;
    string month = string.Empty;
    string semester = string.Empty;
    string selQ = string.Empty;
    string qryyearFilter = string.Empty;
    string qrytermFilter = string.Empty;
    string qrymonthFilter = string.Empty;
    string qrysemesterFilter = string.Empty;

    int query = 0;
    string qry = string.Empty;


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
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();

        if (!IsPostBack)
        {
            pattern();
            showreport2.Visible = false;
            btn_save.Visible = false;
            print2.Visible = false;
        }
    }

    #region pattern

    public void pattern()
    {

    }

    protected void ddl_pattern_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            showreport2.Visible = false;
            btn_save.Visible = false;
            print2.Visible = false;
        }
        catch
        {

        }
    }

    #endregion

    #region go
    protected void btnGo_Click(object sender, EventArgs e)
    {
        DataSet Gymcostmaster = new DataSet();
        Gymcostmaster = gymcost();
        if (Gymcostmaster.Tables.Count > 0 && Gymcostmaster.Tables[0].Rows.Count > 0)
        {
            loadspreaddetails(Gymcostmaster);

        }
        else
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = "No Record Found!";
        }
    }
    #endregion

    #region save
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            int query = 0;
            string Gym_Name = string.Empty;
            string Gym_Acroynm = string.Empty;
            string Gym_PK = string.Empty;
            string year = string.Empty;
            string qruy = string.Empty;
            string month = string.Empty;
            string term = string.Empty;
            string semester = string.Empty;

            spreadDet2.SaveChanges();
            int activerow = spreadDet2.ActiveSheetView.ActiveRow;
            int activecol = spreadDet2.ActiveSheetView.ActiveColumn;
            Gym_Name = Convert.ToString(spreadDet2.Sheets[0].Cells[activerow, 2].Text);
            //Gym_Acroynm = Convert.ToString(spreadDet2.Sheets[0].Cells[activerow, 2].Text);
            Gym_PK = Convert.ToString(spreadDet2.Sheets[0].Cells[activerow, 1].Text);
            year = Convert.ToString(spreadDet2.Sheets[0].Cells[activerow, 3].Text);
            month = Convert.ToString(spreadDet2.Sheets[0].Cells[activerow, 4].Text);
            term = Convert.ToString(spreadDet2.Sheets[0].Cells[activerow, 5].Text);
            semester = Convert.ToString(spreadDet2.Sheets[0].Cells[activerow, 6].Text);
            if (ddl_pattern.SelectedIndex == 1 && !string.IsNullOrEmpty(Gym_PK) && !string.IsNullOrEmpty(year))
            {
                qry = "if exists (select * from HM_GymCostMaster where GymFK='" + Gym_PK + "') update HM_GymCostMaster set  GymFK='" + Gym_PK + "',year='" + year + "' where  GymFK='" + Gym_PK + "' else insert into HM_GymCostMaster(GymFK,year) values ('" + Gym_PK + "','" + year + "')";

                query = d2.update_method_wo_parameter(qry, "TEXT");
            }
            else if (ddl_pattern.SelectedIndex == 0 && !string.IsNullOrEmpty(Gym_PK) && !string.IsNullOrEmpty(month))
            {
                qry = "if exists (select * from HM_GymCostMaster where GymFK='" + Gym_PK + "') update HM_GymCostMaster set  GymFK='" + Gym_PK + "',month='" + month + "' where  GymFK='" + Gym_PK + "' else insert into HM_GymCostMaster(GymFK,month) values ('" + Gym_PK + "','" + month + "')";
                query = d2.update_method_wo_parameter(qry, "TEXT");
            }
            else if (ddl_pattern.SelectedIndex == 2 && !string.IsNullOrEmpty(Gym_PK) && !string.IsNullOrEmpty(term))
            {
                qry = "if exists (select * from HM_GymCostMaster where GymFK='" + Gym_PK + "') update HM_GymCostMaster set  GymFK='" + Gym_PK + "',term='" + term + "' where  GymFK='" + Gym_PK + "' else insert into HM_GymCostMaster(GymFK,term) values ('" + Gym_PK + "','" + term + "')";
                query = d2.update_method_wo_parameter(qry, "TEXT");
            }
            else if (ddl_pattern.SelectedIndex == 3 && !string.IsNullOrEmpty(Gym_PK) && !string.IsNullOrEmpty(semester))
            {
                qry = "if exists (select * from HM_GymCostMaster where GymFK='" + Gym_PK + "') update HM_GymCostMaster set  GymFK='" + Gym_PK + "',semester='" + semester + "' where  GymFK='" + Gym_PK + "' else insert into HM_GymCostMaster(GymFK,semester) values ('" + Gym_PK + "','" + semester + "')";
                query = d2.update_method_wo_parameter(qry, "TEXT");
            }
            if (query != 0)
            {
                alertpopwindow.Visible = true;
                pnl2.Visible = true;
                lblalerterr.Text = "Saved Successfully";
            }
        }

        catch { }
    }
    #endregion

    #region fpspread2

    private DataSet gymcost()
    {
        DataSet dsloaddetails = new DataSet();
        string pattern = string.Empty;
        DataTable dtnew = new DataTable();

        string GymFk = string.Empty;
        try
        {
            if (ddl_pattern.Items.Count > 0)
                pattern = Convert.ToString(ddl_pattern.SelectedValue);

            if (!string.IsNullOrEmpty(pattern))
            {

                selQ = "select gm.GymName,GymPk from HM_GymMaster gm ";
                //selQ += "select gc.GymFK,gc.Year,Semester,Month,Term from HM_GymCostMaster gc";
                //selQ = "select gm.GymPK,gm.GymName,gc.Year,gc.Month,gc.Term,gc.Semester from HM_GymCostMaster gc,HM_GymMaster gm where gc.GymFK=gm.GymPK";
                dsloaddetails.Clear();
                dsloaddetails = d2.select_method_wo_parameter(selQ, "Text");

            }
        }
        catch { }
        return dsloaddetails;

    }

    private void loadspreaddetails(DataSet ds)
    {
        try
        {
            string gym_Name = string.Empty;
            string Year = string.Empty;
            string GymPK = string.Empty;
            string Semester = string.Empty;
            string Term = string.Empty;
            string Month = string.Empty;
            DataView dvgym = new DataView();
            DataSet dsgymcost = new DataSet();
            loadspreadHeader(ds);

            FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
            int sno = 0;


            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                spreadDet2.Sheets[0].RowCount++;
                sno++;
                GymPK = Convert.ToString(ds.Tables[0].Rows[row]["GymPK"]).Trim();
                gym_Name = Convert.ToString(ds.Tables[0].Rows[row]["GymName"]).Trim();

                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 1].CellType = txtCell;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 4].CellType = txtCell;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 5].CellType = txtCell;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 6].CellType = txtCell;

                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(GymPK);
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 2].Text = gym_Name;
                if (ddl_pattern.SelectedIndex == 1)
                {
                    if (GymPK != "")
                    {
                        string selQ1 = "select  Year from HM_GymCostMaster gc where GymFK='" + GymPK + "'";
                        dsgymcost.Clear();
                        dsgymcost = d2.select_method_wo_parameter(selQ1, "Text");
                        if (dsgymcost.Tables.Count > 0 && dsgymcost.Tables[0].Rows.Count > 0)
                        {
                            year = Convert.ToString(dsgymcost.Tables[0].Rows[0]["Year"]).Trim();
                            spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 3].Text = year;

                        }
                    }
                }

                if (ddl_pattern.SelectedIndex == 0)
                {
                    if (GymPK != "")
                    {
                        string selQ1 = "select Month from HM_GymCostMaster gc where GymFK='" + GymPK + "'";
                        dsgymcost.Clear();
                        dsgymcost = d2.select_method_wo_parameter(selQ1, "Text");
                        if (dsgymcost.Tables.Count > 0 && dsgymcost.Tables[0].Rows.Count > 0)
                        {
                            Month = Convert.ToString(dsgymcost.Tables[0].Rows[0]["Month"]).Trim();
                            spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 4].Text = Month;

                        }
                    }

                }
                if (ddl_pattern.SelectedIndex == 2)
                {
                    if (GymPK != "")
                    {
                        string selQ1 = "select  Term from HM_GymCostMaster gc where GymFK='" + GymPK + "'";
                        dsgymcost.Clear();
                        dsgymcost = d2.select_method_wo_parameter(selQ1, "Text");
                        if (dsgymcost.Tables.Count > 0 && dsgymcost.Tables[0].Rows.Count > 0)
                        {
                            term = Convert.ToString(dsgymcost.Tables[0].Rows[0]["Term"]).Trim();
                            spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 5].Text = term;

                        }
                    }
                }

                if (ddl_pattern.SelectedIndex == 3)
                {
                    if (GymPK != "")
                    {
                        string selQ1 = "select  Semester from HM_GymCostMaster gc where GymFK='" + GymPK + "'";
                        dsgymcost.Clear();
                        dsgymcost = d2.select_method_wo_parameter(selQ1, "Text");
                        if (dsgymcost.Tables.Count > 0 && dsgymcost.Tables[0].Rows.Count > 0)
                        {
                            semester = Convert.ToString(dsgymcost.Tables[0].Rows[0]["Semester"]).Trim();
                            spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 6].Text = semester;

                        }
                    }
                }




                //spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 2].Text = year;
                //spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 3].Text = semester;
                //spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 4].Text = term;
                //spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 5].Text = Month;


                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;


                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;


                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 0].Locked = true;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 1].Locked = true;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 2].Locked = true;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 3].Locked = false;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 4].Locked = false;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 5].Locked = false;
                spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 6].Locked = false;

            }

            spreadDet2.Sheets[0].Columns[0].Width = 128;
            spreadDet2.Sheets[0].Columns[1].Width = 385;
            spreadDet2.Sheets[0].Columns[2].Width = 265;
            spreadDet2.Sheets[0].Columns[3].Width = 385;
            spreadDet2.Sheets[0].Columns[4].Width = 382;
            spreadDet2.Sheets[0].Columns[5].Width = 382;
            spreadDet2.Sheets[0].Columns[6].Width = 382;
            spreadDet2.Sheets[0].Columns[1].Visible = false;
            spreadDet2.Height = 390;
            spreadDet2.Width = 800;
            spreadDet2.Sheets[0].PageSize = spreadDet2.Sheets[0].RowCount;
            spreadDet2.SaveChanges();
            showreport2.Visible = true;
            btn_save.Visible = true;
            print2.Visible = true;

        }



        catch
        {
        }
    }

    public void loadspreadHeader(DataSet ds)
    {

        try
        {

            spreadDet2.Sheets[0].RowCount = 0;
            spreadDet2.Sheets[0].ColumnCount = 7;
            spreadDet2.CommandBar.Visible = false;
            spreadDet2.Sheets[0].AutoPostBack = false;
            spreadDet2.Sheets[0].ColumnHeader.RowCount = 1;
            spreadDet2.Sheets[0].RowHeader.Visible = false;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            spreadDet2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;


            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "SNo";
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 0].VerticalAlign = VerticalAlign.Bottom;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 0].Locked = true;
            spreadDet2.Sheets[0].Columns[0].Width = 20;

            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "GymPK";
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 1].VerticalAlign = VerticalAlign.Bottom;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 1].Locked = true;

            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Gym Name";
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 2].VerticalAlign = VerticalAlign.Bottom;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 2].Locked = true;


            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Year";
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 3].VerticalAlign = VerticalAlign.Bottom;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 3].Locked = true;


            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Month";
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 4].VerticalAlign = VerticalAlign.Bottom;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 4].Locked = true;


            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Term";
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 5].VerticalAlign = VerticalAlign.Bottom;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 5].Locked = true;


            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Semester";
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 6].VerticalAlign = VerticalAlign.Bottom;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 6].Locked = true;
            //spreadDet2.Sheets[0].Columns[5].Visible = false;

            if (ddl_pattern.SelectedIndex == 1)
            {
                spreadDet2.Columns[0].Visible = true;
                spreadDet2.Columns[1].Visible = false;
                spreadDet2.Columns[2].Visible = true;
                spreadDet2.Columns[3].Visible = true;
                spreadDet2.Columns[4].Visible = false;
                spreadDet2.Columns[5].Visible = false;
                spreadDet2.Columns[6].Visible = false;
            }
            if (ddl_pattern.SelectedIndex == 0)
            {
                spreadDet2.Columns[0].Visible = true;
                spreadDet2.Columns[1].Visible = false;
                spreadDet2.Columns[2].Visible = true;
                spreadDet2.Columns[3].Visible = false;
                spreadDet2.Columns[4].Visible = true;
                spreadDet2.Columns[5].Visible = false;
                spreadDet2.Columns[6].Visible = false;

            }
            if (ddl_pattern.SelectedIndex == 2)
            {
                spreadDet2.Columns[0].Visible = true;
                spreadDet2.Columns[1].Visible = false;
                spreadDet2.Columns[2].Visible = true;
                spreadDet2.Columns[3].Visible = false;
                spreadDet2.Columns[4].Visible = false;
                spreadDet2.Columns[5].Visible = true;
                spreadDet2.Columns[6].Visible = false;

            }
            if (ddl_pattern.SelectedIndex == 3)
            {
                spreadDet2.Columns[0].Visible = true;
                spreadDet2.Columns[1].Visible = false;
                spreadDet2.Columns[2].Visible = true;
                spreadDet2.Columns[3].Visible = false;
                spreadDet2.Columns[4].Visible = false;
                spreadDet2.Columns[5].Visible = false;
                spreadDet2.Columns[6].Visible = true;

            }


        }
        catch (Exception ex) { }
    }

    #region Print
    protected void btnExcel_Click2(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname2.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(spreadDet2, reportname);
                // lblvalidation1.Visible = false;
            }
            else
            {
                //lblvalidation1.Text = "Please Enter Your  Report Name";
                //lblvalidation1.Visible = true;
                txtexcelname2.Focus();
            }
        }
        catch { }

    }

    public void btnprintmaster_Click2(object sender, EventArgs e)
    {
        try
        {
            lblvalidation3.Text = "";
            txtexcelname2.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Allotment Report";
            //+'@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "AllotmentReport.aspx";
            Printcontrolhed2.loadspreaddetails(spreadDet2, pagename, degreedetails);
            Printcontrolhed2.Visible = true;
        }
        catch { }
    }

    protected void getPrintSettings2()
    {
        try
        {

            #region Excel print settings
            string usertype = "";
            if (usercode.Trim() != "")
                usertype = " and usercode='" + usercode + "'";
            else if (group_user.Trim() != "")
                usertype = " and group_code='" + group_user + "'";
            string printset = d2.GetFunction("select value from Master_Settings where settings='Excel and Pdf Print Settings' " + usertype + " ");
            if (printset != "")
            {
                if (printset.Contains("E"))
                {
                    lblrptname2.Visible = true;
                    txtexcelname2.Visible = true;
                    btnExcel2.Visible = true;
                }
                if (printset.Contains("P"))
                {
                    btnprintmasterhed2.Visible = true;
                }
                if (printset == "0")
                {
                    lblrptname2.Visible = true;
                    txtexcelname2.Visible = true;
                    btnExcel2.Visible = true;
                    btnprintmasterhed2.Visible = true;

                }
            }
            #endregion
        }
        catch { }
    }

    #endregion

    #endregion



    #region alertclose
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblalerterr.Text = string.Empty;
            alertpopwindow.Visible = false;
        }
        catch { }
    }


    #endregion

}
