using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Drawing;
using InsproDataAccess;
using System.Configuration;

public partial class HostelMod_GymMaster : System.Web.UI.Page
{
    #region Field Declaration

    SqlCommand cmd = new SqlCommand();
    static Hashtable Has_Stage = new Hashtable();
    ReuasableMethods ru = new ReuasableMethods();
    DataSet dsprint = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable htmnth = new Hashtable();
    bool check = false;
    Hashtable hat = new Hashtable();
    string Gym_PK = string.Empty;
    DataSet d1 = new DataSet();
    Hashtable hastab = new Hashtable();
    Hashtable ht = new Hashtable();
    static Hashtable spr_hash = new Hashtable();
    static Hashtable priority_hash = new Hashtable();
    string usercode = "";
    string singleuser = "", group_user = "";

    string selQ = string.Empty;
    string sql = string.Empty;
    string Gym_Acroynm = string.Empty;
    string Gym_Name = string.Empty;
    string name = string.Empty;
    string acry = string.Empty;
    string nam = string.Empty;
    string acr = string.Empty;
    string sqlu = string.Empty;
    Boolean Cellclick = false;
    int query = 0;
    #endregion

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
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        string Gym_Name = string.Empty;
        string Gym_Acroynm = string.Empty;
        if (!IsPostBack)
        {
            Div1.Visible = false;
        }
    }

    #region gymname

    protected void Gymname_TextChanged(object sender, EventArgs e)
    {

    }

    #endregion

    #region pop
    protected void btnaddnew_Click(object sender, EventArgs e)
    {
        divadd.Visible = true;
        imgAlert.Visible = false;

    }

    protected void imgclose_Click(object sender, EventArgs e)
    {
        divadd.Visible = false;
    }
    #endregion

    #region txtgymacrynm
    protected void Gymacr_TextChanged(object sender, EventArgs e)
    {

    }

    #endregion

    #region Save
    protected void btnSave_Click(object sender, EventArgs e)
    {

        int query = 0;
        try
        {
            if (txtgymname.Text != "" && txtAcry.Text != "")
            {
                name = Convert.ToString(txtgymname.Text);
                acry = Convert.ToString(txtAcry.Text);
                Fpload1.SaveChanges();
                sql = "if exists (select * from HM_GymMaster where GymAcr='" + acry + "' and GymName='" + name + "') update HM_GymMaster set GymAcr='" + acry + "',GymName='" + name + "' where GymAcr='" + acry + "' and GymName='" + name + "' else insert into HM_GymMaster(GymName,GymAcr) values ('" + name + "','" + acry + "')";
                query = d2.update_method_wo_parameter(sql, "TEXT");
                if (query != 0)
                {
                    Div1.Visible = false;
                    imgAlert.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Saved Successfully";
                    //gymmaster();
                   
                }
            }
            else
            {
                Div1.Visible = false;
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Enter The Gym Details!";
            }
        }
        catch
        {
        }
    }
    #endregion

    #region delete and update
    protected void delete()
    {
        try
        {
            int activerow = Fpload1.ActiveSheetView.ActiveRow;
            int activecol = Fpload1.ActiveSheetView.ActiveColumn;
            string sqld = "";
            int query = 0;
            Gym_Name = Convert.ToString(Fpload1.Sheets[0].Cells[activerow, 1].Text);
            Gym_Acroynm = Convert.ToString(Fpload1.Sheets[0].Cells[activerow, 2].Text);
            Gym_PK = Convert.ToString(Fpload1.Sheets[0].Cells[activerow, 3].Text);

            if (txt_name.Text != "" && txt_acry.Text != "")
            {
                nam = Convert.ToString(txt_name.Text);
                acr = Convert.ToString(txt_acry.Text);
                sqld = "delete from HM_GymMaster where GymAcr='" + acr + "' and GymName='" + nam + "'";
                query = d2.update_method_wo_parameter(sqld, "TEXT");
                if (query != 0)
                {
                    Divdelete.Visible = true;
                    Label4.Visible = true;
                    Label4.Text = "Deleted Successfully";
                    Div3.Visible = false;


                }
            }
           
       
        }
        catch (Exception ex) { }
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_delete.Text == "Delete")
            {
                if (txt_name.Text == "" && txt_acry.Text == "")
                {
                   
                    Divdelete.Visible = true;
                    Label4.Visible = true;
                    Label4.Text = "No Record Found";

                }
                else
                {
                    surediv.Visible = true;
                    lbl_sure.Text = "Do you want to delete this record?";
                }
            }
        }
        catch
        {
        }
    }

    protected void btnupdate_Click(object sender, EventArgs e)
    {
       
        try
        {
            int activerow = Fpload1.ActiveSheetView.ActiveRow;
            int activecol = Fpload1.ActiveSheetView.ActiveColumn;
            Gym_Name = Convert.ToString(Fpload1.Sheets[0].Cells[activerow, 1].Text);
            Gym_Acroynm = Convert.ToString(Fpload1.Sheets[0].Cells[activerow, 2].Text);
            Gym_PK = Convert.ToString(Fpload1.Sheets[0].Cells[activerow, 3].Text);

            if (txt_name.Text != "" && txt_acry.Text != "")
            {
                nam = Convert.ToString(txt_name.Text);
                acr = Convert.ToString(txt_acry.Text);

                sqlu = "update HM_GymMaster set GymAcr='" + acr + "', GymName='" + nam + "' where GymPk='" + Gym_PK + "'";
                query = d2.update_method_wo_parameter(sqlu, "TEXT");
                if (query != 0)
                {
                    Div1.Visible = true;
                    Label3.Visible = true;
                    Label3.Text = "Updated Successfully";
                   
                }
            }
            else
            {
                txt_name.Text = "";
                txt_acry.Text = "";
                Div1.Visible = false;
                imgAlert.Visible = true;
                lbl_alert.Text = "No Record Found!";
            }
        }
        catch
        {
        }
    }
    #endregion

    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
        popwindow1.Visible = true;
       
      
    }

    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        
        popwindow1.Visible = true;
    }


   
    #region alertclose
    protected void btnpopsave_Click(object sender, EventArgs e)
    {
        try
        {
            btnSave_Click(sender, e);
        }
        catch (Exception ex) { }
    }

    protected void btn_alertclose_Click(object sender, EventArgs e)
    {

        imgAlert.Visible = false;
        gymmaster();

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
        Div1.Visible = false;
        gymmaster();
        txt_name.Text = "";
        txt_acry.Text = "";
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
        Divdelete.Visible = false;
        gymmaster();
        Div3.Visible = false;
        surediv.Visible = false;
        txt_name.Text = "";
        txt_acry.Text = "";
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            //lblAlertMsg.Text = string.Empty;
            //divPopAlert.Visible = false;
        }
        catch
        {
        }
    }
  
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
    }

    protected void btnpopexit_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
    }
    #endregion

    #region spread
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

    protected void Fpspread_render(object sender, EventArgs e)
    {
        try
        {
            string Gym_PK = string.Empty;
            if (check == true)
            {
                popwindow1.Visible = true;
                btn_save1.Visible = false;
                btn_exit1.Visible = false;
                btn_update.Visible = true;
                btn_delete.Visible = true;


                int activerow = Fpload1.ActiveSheetView.ActiveRow;
                int activecol = Fpload1.ActiveSheetView.ActiveColumn;

                if (activerow > 0)
                {
                    Gym_Name = Convert.ToString(Fpload1.Sheets[0].Cells[activerow, 1].Text);
                    Gym_Acroynm = Convert.ToString(Fpload1.Sheets[0].Cells[activerow, 2].Text);
                    Gym_PK = Convert.ToString(Fpload1.Sheets[0].Cells[activerow, 3].Text);

                    Fpload1.Sheets[0].Cells[activerow, activecol].Tag = Gym_Name;
                    Fpload1.Sheets[0].Cells[activerow, activecol].Note = Gym_Acroynm;
                    Fpload1.Sheets[0].Cells[activerow, activecol].Tag = Gym_PK;


                    lbl_name.Enabled = true;
                    txt_name.Enabled = true;
                    lbl_acry.Enabled = true;
                    txt_acry.Enabled = true;

                    txt_name.Text = Gym_Name;
                    txt_acry.Text = Gym_Acroynm;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
   
    protected void Fpload_OnButtonCommand(object sender, EventArgs e)
    {
        try
        {
            gymmaster();
        }
        catch 
        {
        }
    }

    private void gymmaster()
    {
        string namegym = string.Empty;
        string acrygym = string.Empty;
        DataSet dsgympage = new DataSet();
        DataSet dsgym = new DataSet();
        try
        {
            if (txtgymname.Text != "" && txtAcry.Text != "")
            {

                namegym = Convert.ToString(txtgymname.Text);
                acrygym = Convert.ToString(txtAcry.Text);

                selQ = "select GymAcr,GymName,GymPK from HM_GymMaster";

                dsgym.Clear();
                dsgym = d2.select_method_wo_parameter(selQ, "Text");

                loadspreaddetails(dsgym);
                divadd.Visible = false;
            }
        }
        catch { }

    }

    private void loadspreaddetails(DataSet ds)
    {
        try
        {
            loadspreadHeader(ds);

            FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
            int sno = 0;


            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {

                    Fpload1.Sheets[0].RowCount++;
                    sno++;
                    Gym_Name = Convert.ToString(ds.Tables[0].Rows[row]["GymName"]).Trim();
                    Gym_Acroynm = Convert.ToString(ds.Tables[0].Rows[row]["GymAcr"]).Trim();
                    Gym_PK = Convert.ToString(ds.Tables[0].Rows[row]["GymPK"]).Trim();



                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 1].CellType = txtCell;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 3].CellType = txtCell;



                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);

                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 1].Text = Gym_Name;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 2].Text = Gym_Acroynm;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 3].Text = Gym_PK;




                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;


                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;




                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 0].Locked = true;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 1].Locked = true;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 2].Locked = true;



                }

                Fpload1.Sheets[0].Columns[0].Width = 50;
                Fpload1.Sheets[0].Columns[1].Width = 150;
                Fpload1.Sheets[0].Columns[2].Width = 100;
                Fpload1.Sheets[0].Columns[3].Visible = false;


                Fpload1.Sheets[0].PageSize = Fpload1.Sheets[0].RowCount;
                Fpload1.SaveChanges();
                Fpload1.Visible = true;
                lbprint.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btn_excel.Visible = true;
                btnprintmaster.Visible = true;

            }
        }

        catch
        {
        }
    }

    public void loadspreadHeader(DataSet ds)
    {

        try
        {

            Fpload1.Sheets[0].RowCount = 0;
            Fpload1.Sheets[0].ColumnCount = 4;
            Fpload1.CommandBar.Visible = false;
            Fpload1.Sheets[0].AutoPostBack = true;
            Fpload1.Sheets[0].ColumnHeader.RowCount = 1;
            Fpload1.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            Fpload1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.VerticalAlign = VerticalAlign.Middle;


            Fpload1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "SNo";
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 0].VerticalAlign = VerticalAlign.Bottom;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 0].Locked = true;
            Fpload1.Sheets[0].Columns[0].Width = 20;

            Fpload1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Gym_Name";
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 1].VerticalAlign = VerticalAlign.Bottom;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 1].Locked = true;
            Fpload1.Sheets[0].Columns[1].Width = 10;


            Fpload1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Gym_Acroynm";
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 2].VerticalAlign = VerticalAlign.Bottom;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 2].Locked = true;
            Fpload1.Sheets[0].Columns[2].Width = 50;

            Fpload1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Gym_PK";
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 3].VerticalAlign = VerticalAlign.Bottom;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 3].Locked = true;
          


        }
        catch (Exception ex) {}
    }
    #endregion

    # region print
   

    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;

            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpload1, reportname);
            }
            else
            {
                txtexcelname.Focus();
                //  lblerrmainapp.Text = "Please Enter Your Report Name";
                // lblerrmainapp.Visible = true;lbprint
                lbprint.Text = "Please Enter Your Report Name";
                lbprint.Visible = true;
            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {

        string degreedetails = "Gym Master";

        string pagename = "GymMaster.aspx";
        Session["column_header_row_count"] = Fpload1.ColumnHeader.RowCount;

        Printcontrol.loadspreaddetails(Fpload1, pagename, degreedetails);
        Printcontrol.Visible = true;

    }
    #endregion

}












