using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.Drawing;
using System.Text.RegularExpressions;
using System.IO;

public partial class StageMaster : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    DAccess2 da = new DAccess2();
    DAccess2 dset = new DAccess2();
    Hashtable hastab = new Hashtable();
    DataSet d1 = new DataSet();
    string PageLogOut = "";

    protected void lb2_Click(object sender, EventArgs e) //Sankar For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("TransportIndex"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/TransportMod/TransportIndex.aspx");
                return;
            }
        }
        if (!IsPostBack)
        {
            btnsave.Enabled = false;
            Buttondelete.Enabled = false;
            Printcontrol.Visible = false;
            loadstage();
            GetRecord();
            loadDetails();
            Stage_Name_Filter();
            load_filter();
            ddlcertificate.Attributes.Add("onfocus", "fStage()");

        }
        FpSpreadstage.Sheets[0].Columns[4].Visible = true;

    }

    public void loadDetails()
    {
        DataSet dnew = new DataSet();
        string sqlcmd = string.Empty;
        //sqlcmd = "select distinct textcode,textval from textvaltable where textcriteria  in ('Sdis','dis') ";   //modified by prabha  textcriteria='Sdis'
        sqlcmd = "select * from District_Stage ";//Modified by rajasekar on 11/05/2018
        dnew = dset.select_method_wo_parameter(sqlcmd, "text");
        if (dnew.Tables[0].Rows.Count > 0)
        {
            ddlcertificate.Items.Clear();

            ddlcertificate.DataSource = dnew.Tables[0];
            //ddlcertificate.DataTextField = "textval";
            //ddlcertificate.DataValueField = "textcode";
            ddlcertificate.DataTextField = "District_Name";//Modified by rajasekar on 11/05/2018
            ddlcertificate.DataValueField = "District_Code";//Modified by rajasekar on 11/05/2018
            ddlcertificate.DataBind();
        }

        ddlcertificate.Items.Insert(0, "");
    }

    public void load_filter()
    {
        DataSet dnew = new DataSet();
        string sqlcmd = string.Empty;
        //sqlcmd = "select distinct textcode,textval from textvaltable where textcriteria   in ('Sdis','dis') ";   //modified by prabha  textcriteria='Sdis'
        sqlcmd = "select * from District_Stage ";//Modified by rajasekar on 11/05/2018
        dnew = dset.select_method_wo_parameter(sqlcmd, "text");
        if (dnew.Tables[0].Rows.Count > 0)
        {
            ddlvehicletypeview.Items.Clear();

            ddlvehicletypeview.DataSource = dnew.Tables[0];
            //ddlvehicletypeview.DataTextField = "textval";
            //ddlvehicletypeview.DataValueField = "textcode";
            ddlvehicletypeview.DataTextField = "District_Name";//Modified by rajasekar on 11/05/2018
            ddlvehicletypeview.DataValueField = "District_Code";//Modified by rajasekar on 11/05/2018
            ddlvehicletypeview.DataBind();
        }

        ddlvehicletypeview.Items.Insert(0, new ListItem("All", "-1"));



    }

    public void Stage_Name_Filter()
    {
        DataSet dnew1 = new DataSet();
        ddltypeview.Items.Clear();
        ddltypeview.Items.Insert(0, new ListItem("All", "-1"));
        string sql;
        sql = "select distinct Stage_Name from stage_master order by Stage_Name";
        dnew1 = da.select_method_wo_parameter(sql, "txt");
        if (dnew1.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dnew1.Tables[0].Rows.Count; i++)
            {
                ddltypeview.Items.Add(dnew1.Tables[0].Rows[i]["Stage_Name"].ToString());
            }
            ddltypeview.SelectedIndex = 0;

        }
        con.Close();
    }

    public void GetRecord()
    {
        int temp = -1;
        int temptemp = 0;
        string getdate = "";
        getdate = "select * from Stage_Master";
        SqlDataAdapter dr = new SqlDataAdapter(getdate, con);
        DataSet dt = new DataSet();
        dr.Fill(dt);
        if (dt.Tables[0].Rows.Count > 0)
        {
            Btnprint.Enabled = true;
            Buttondelete.Enabled = false;
            FpSpreadstage.Sheets[0].RowCount = dt.Tables[0].Rows.Count;

            string sqlcmd = string.Empty;
            sqlcmd = "select * from District_Stage";//Modified by rajasekar on 11/05/2018
            //sqlcmd = "select distinct textcode,textval from textvaltable where textcriteria in ('Sdis','dis')";  //modified by prabha on 21dec2017
            DataSet dnew = new DataSet();
            dnew = dset.select_method_wo_parameter(sqlcmd, "text");
            ddltypeview.Items.Clear();
            ddltypeview.Items.Insert(0, new ListItem("All", "-1"));
            for (int i2 = 0; i2 < dt.Tables[0].Rows.Count; i2++)
            {
                temp++;
                temptemp = temp + 1;
                FpSpreadstage.Sheets[0].Cells[i2, 0].Text = Convert.ToInt32(temptemp).ToString();
                FpSpreadstage.Sheets[0].Cells[i2, 2].Text = dt.Tables[0].Rows[i2]["Stage_Name"].ToString();
                FpSpreadstage.Sheets[0].Cells[i2, 2].Tag = dt.Tables[0].Rows[i2]["Stage_id"].ToString();
                FpSpreadstage.Sheets[0].Cells[i2, 3].Text = dt.Tables[0].Rows[i2]["Address"].ToString();

                FarPoint.Web.Spread.ComboBoxCellType cmbcol = new FarPoint.Web.Spread.ComboBoxCellType();
                cmbcol.ShowButton = true;
                cmbcol.UseValue = true;
                cmbcol.DataSource = dnew;
                //cmbcol.DataTextField = "textval";
                //cmbcol.DataValueField = "textcode";
                cmbcol.DataTextField = "District_Name";//Modified by rajasekar on 11/05/2018
                cmbcol.DataValueField = "District_Code";//Modified by rajasekar on 11/05/2018
                FpSpreadstage.Sheets[0].Columns[1].CellType = cmbcol;

                FpSpreadstage.Sheets[0].Cells[i2, 1].Text = Convert.ToString(dt.Tables[0].Rows[i2]["District"]);
                FpSpreadstage.Sheets[0].Cells[i2, 4].Value = 0;
                ddltypeview.Items.Add(dt.Tables[0].Rows[i2]["Stage_Name"].ToString());
            }
            //30Nov2013========================================================
            ddlcertificate.Items.Clear();
            ddlcertificate.DataSource = dnew;
            //ddlcertificate.DataTextField = "textval";
            //ddlcertificate.DataValueField = "textcode";
            ddlcertificate.DataTextField = "District_Name";//Modified by rajasekar on 11/05/2018
            ddlcertificate.DataValueField = "District_Code";//Modified by rajasekar on 11/05/2018
            ddlcertificate.DataBind();
            ddlcertificate.Items.Insert(0, "");

            ddlvehicletypeview.Items.Clear();
            ddlvehicletypeview.DataSource = dnew.Tables[0];
            //ddlvehicletypeview.DataTextField = "textval";
            //ddlvehicletypeview.DataValueField = "textcode";
            ddlvehicletypeview.DataTextField = "District_Name";//Modified by rajasekar on 11/05/2018
            ddlvehicletypeview.DataValueField = "District_Code";//Modified by rajasekar on 11/05/2018
            ddlvehicletypeview.DataBind();
            ddlvehicletypeview.Items.Insert(0, new ListItem("All", "-1"));
            //=================================================================
            FpSpreadstage.Sheets[0].PageSize = FpSpreadstage.Sheets[0].RowCount;

        }
        FpSpreadstage.SaveChanges();
    }

    protected void FpSpreadstage_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        //string ar = e.SheetView.ActiveRow.ToString();
        //string ac = e.SheetView.ActiveColumn.ToString();
        //int actrow = Convert.ToInt32(ar);
        //int actcol = Convert.ToInt32(ac);
        //string stageidd = Convert.ToString(FpSpreadstage.Sheets[0].Cells[actrow, 0].Tag);
        //if (stageidd != "")
        //{
        //    if (actcol == 2)
        //    {
        //        con.Close();
        //        con.Open();
        //        string deletequery = "";
        //        deletequery = "delete from Stage_Master where Stage_id = '" + stageidd + "'";
        //        SqlCommand del = new SqlCommand(deletequery, con);
        //        del.ExecuteNonQuery();
        //        con.Close();
        //        FpSpreadstage.Sheets[0].RemoveRows(actrow, 1);
        //    }
        //}
        //else
        //{
        //    FpSpreadstage.Sheets[0].RemoveRows(actrow, 1);
        //}


    }

    protected void FpSpreadstage_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        for (int i = 0; i < Convert.ToInt16(FpSpreadstage.Sheets[0].RowCount); i++)
        {

            string actrow1;
            actrow1 = e.SheetView.ActiveRow.ToString();
            int actrow = Convert.ToInt32(actrow1);
            string actrow11 = e.SheetView.ActiveRow.ToString();
            string actcol = e.SheetView.ActiveColumn.ToString();
            if (actrow.ToString() != "-1" && actrow11 != "-1" && actcol != "-1")
            {
                //if (actrow1 == "0")
                if (FpSpreadstage.Sheets[0].Cells[actrow, 4].Value != null)
                {
                    //{
                    if (i == 0)
                    {
                        Buttondelete.Enabled = true;
                        if (FpSpreadstage.Sheets[0].Cells[actrow, 4].Value.ToString() == "0")
                        {
                            for (int j = 0; j < Convert.ToInt16(FpSpreadstage.Sheets[0].RowCount); j++)
                            {
                                if (j == 0)
                                {
                                    FpSpreadstage.Sheets[0].Cells[actrow, 4].Value = 1;
                                }
                                //}

                            }
                        }
                        else
                        {
                            for (int j = 0; j < Convert.ToInt16(FpSpreadstage.Sheets[0].RowCount); j++)
                            {
                                if (j == 0)
                                {
                                    FpSpreadstage.Sheets[0].Cells[actrow, 4].Value = 0;
                                }

                            }
                        }
                    }
                }
                //}
            }
        }

    }

    public void loadstage()
    {
        FarPoint.Web.Spread.TextCellType tb = new FarPoint.Web.Spread.TextCellType();
        //FpSpreadstage.Sheets[0].AutoPostBack = true;
        //FpSpreadstage.ActiveSheetView.SheetCorner.Cells[0, 0].Text = "S.No";
        //FpSpreadstage.ActiveSheetView.SheetCorner.DefaultStyle.Font.Bold = true;
        FpSpreadstage.ActiveSheetView.DefaultRowHeight = 25;
        FpSpreadstage.ActiveSheetView.Rows.Default.Font.Name = "MS Sans Serif";
        FpSpreadstage.ActiveSheetView.Rows.Default.Font.Size = FontUnit.Small;
        FpSpreadstage.ActiveSheetView.Rows.Default.Font.Bold = false;
        FpSpreadstage.ActiveSheetView.Columns.Default.Font.Bold = false;
        FpSpreadstage.Sheets[0].RowHeader.Visible = false;
        FpSpreadstage.ActiveSheetView.Columns.Default.Font.Name = "MS Sans Serif";
        FpSpreadstage.ActiveSheetView.Columns.Default.Font.Size = FontUnit.Small;
        FpSpreadstage.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
        FpSpreadstage.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "MS Sans Serif";
        FpSpreadstage.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Small;
        FpSpreadstage.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
        FpSpreadstage.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
        FarPoint.Web.Spread.ButtonCellType btnmodify = new FarPoint.Web.Spread.ButtonCellType();
        FarPoint.Web.Spread.CheckBoxCellType cbxd = new FarPoint.Web.Spread.CheckBoxCellType();
        FarPoint.Web.Spread.ComboBoxCellType cf = new FarPoint.Web.Spread.ComboBoxCellType();
        cf.AutoPostBack = true;
        cbxd.AutoPostBack = true;
        btnmodify.Text = "Remove";
        FpSpreadstage.Sheets[0].RowCount = 0;
        FpSpreadstage.Sheets[0].ColumnCount = 5;
        FpSpreadstage.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpreadstage.Sheets[0].ColumnHeader.Cells[0, 1].Text = "District";
        FpSpreadstage.Sheets[0].Columns[1].CellType = cf;
        FpSpreadstage.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Stage Name";
        FpSpreadstage.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Address";
        FpSpreadstage.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Select";
        FpSpreadstage.Sheets[0].Columns[4].CellType = cbxd;
        //FpSpreadstage.Sheets[0].Columns[2].CellType = btnmodify;
        FpSpreadstage.Sheets[0].Columns[3].Visible = true;
        FpSpreadstage.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        FpSpreadstage.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
        //FpSpreadstage.Sheets[0].Columns[2].Font.Bold = true;
        FpSpreadstage.Sheets[0].Columns[0].Width = 40;
        FpSpreadstage.Sheets[0].Columns[1].Width = 200;
        FpSpreadstage.Sheets[0].Columns[2].Width = 350;
        FpSpreadstage.Sheets[0].Columns[3].Width = 340;
        FpSpreadstage.Sheets[0].Columns[4].Width = 50;
        FpSpreadstage.Sheets[0].PageSize = 100;
        FpSpreadstage.Width = 1000;
        FpSpreadstage.Height = 391;
        FpSpreadstage.SaveChanges();
        FpSpreadstage.CommandBar.Visible = false;

    }

    protected void addremoveStage(object sender, EventArgs e)
    {

    }

    protected void addrowStage(object sender, EventArgs e)
    {
        if (ddlcertificate.Items.Count > 1)
        {
            lblerror.Visible = false;
            FpSpreadstage.Sheets[0].RowCount = FpSpreadstage.Sheets[0].RowCount + 1;
            string sqlcmd = string.Empty;
            //sqlcmd = "select distinct textcode,textval from textvaltable where textcriteria   in ('Sdis','dis') ";   //modified by prabha  textcriteria='Sdis'
            sqlcmd = "select * from District_Stage";//Modified by rajasekar on 11/05/2018
            DataSet dnew = new DataSet();
            dnew = dset.select_method_wo_parameter(sqlcmd, "text");
            FarPoint.Web.Spread.ComboBoxCellType cmbcol = new FarPoint.Web.Spread.ComboBoxCellType();
            cmbcol.ShowButton = true;
            cmbcol.UseValue = true;
            cmbcol.DataSource = dnew;
            //cmbcol.DataTextField = "textval";
            //cmbcol.DataValueField = "textcode";
            cmbcol.DataTextField = "District_Name";//Modified by rajasekar on 11/05/2018
            cmbcol.DataValueField = "District_Code";//Modified by rajasekar on 11/05/2018
            FpSpreadstage.Sheets[0].Columns[1].CellType = cmbcol;
            FpSpreadstage.Sheets[0].Cells[0, 4].Value = 0;
            Buttondelete.Enabled = false;
            btnsave.Enabled = true;
            FpSpreadstage.Sheets[0].PageSize = FpSpreadstage.Sheets[0].RowCount;
            FpSpreadstage.SaveChanges();
        }
        else
        {
            lblerror.Visible = false;
            lblerror.Text = "Enter the District Name";
        }
    }

    protected void ButtonsaveRoute_Click(object sender, EventArgs e)
    {
        lblerror.Visible = false;
        FpSpreadstage.SaveChanges();
        ArrayList a1 = new ArrayList();
        ArrayList a2 = new ArrayList();
        Hashtable hatdis = new Hashtable();
        string colcode = "";
        string degreecode = "";
        if (FpSpreadstage.Sheets[0].RowCount > 0)
        {
            for (int i = 0; i < FpSpreadstage.Rows.Count; i++)
            {

                colcode = FpSpreadstage.Sheets[0].Cells[i, 2].Text.ToString();
                degreecode = FpSpreadstage.Sheets[0].Cells[i, 1].Text.ToString();

                if (!hatdis.Contains(colcode))
                {
                    hatdis.Add(colcode, colcode + ',' + degreecode);
                }
                //modified by srinath 16/2/2014
                if (i != 0)
                {
                    if (a1.Contains(colcode))
                    {
                        if (hatdis.Contains(colcode))
                        {
                            string disval = hatdis[colcode].ToString();
                            if (disval == colcode + ',' + degreecode)
                            {
                                a2.Add(colcode);
                                //srinath 19mar2013===============================
                                lblerror.Visible = true;
                                lblerror.Text = "  '" + degreecode + '-' + colcode + "'  Stage is Already Exist";
                                FpSpreadstage.Visible = true;
                                return;
                                //================================================
                            }
                        }
                    }
                }
                a1.Add(colcode);
            }
            int count = a2.ToArray().GroupBy(q => q).Count(q => q.Count() > 1);
            if (a2.Count > 0)
            {
                lblerror.Visible = true;
                lblerror.Text = "Enter different Stage Name and District";
                FpSpreadstage.Visible = true;
                return;
            }
            else
            {
                lblerror.Visible = false;
            }
        }
        else
        {
            lblerror.Visible = true;
            lblerror.Text = "Enter the Stage Details";
            return;
        }
        if (FpSpreadstage.Sheets[0].RowCount > 0)
        {
            //First Delete And Insert and Update Query=====================================
            //con.Close();
            //con.Open();
            //string deletequery = "";
            //deletequery = "delete from Stage_Master";
            //SqlCommand del = new SqlCommand(deletequery, con);
            //del.ExecuteNonQuery();
            //con.Close();
            //===================================================================================
            FpSpreadstage.SaveChanges();
            for (int inew = 0; inew < FpSpreadstage.Sheets[0].RowCount; inew++)
            {
                string stagename = Convert.ToString(FpSpreadstage.Sheets[0].Cells[inew, 2].Text);
                stagename = stagename.Replace("'", "''");
                string stage_id = Convert.ToString(FpSpreadstage.Sheets[0].Cells[inew, 2].Tag);
                string Address = Convert.ToString(FpSpreadstage.Sheets[0].Cells[inew, 3].Text);
                string District = Convert.ToString(FpSpreadstage.GetEditValue(inew, 1));
                if (District == "System.Object") District = FpSpreadstage.Sheets[0].GetValue(inew, 1).ToString();
                if (stagename != "")
                {
                    //Update chehk========================
                    if (stage_id != "")
                    {
                        string chk_stage_id = string.Empty;
                        chk_stage_id = "select * from stage_master where Stage_id = '" + stage_id + "'";
                        DataSet cht_id = new DataSet();
                        cht_id = da.select_method_wo_parameter(chk_stage_id, "text");
                        if (cht_id.Tables[0].Rows.Count > 0)
                        {
                            string old_stageName = cht_id.Tables[0].Rows[0]["Stage_Name"].ToString();
                            old_stageName = old_stageName.Replace("'", "''");
                            string old_address = cht_id.Tables[0].Rows[0]["Address"].ToString();
                            string old_Dist = cht_id.Tables[0].Rows[0]["District"].ToString();
                            if (old_stageName == stagename && old_address == Address && old_Dist == District)
                            {
                                con.Close();
                                con.Open();
                                string updatequery = string.Empty;
                                updatequery = "update Stage_Master set Stage_Name = '" + stagename + "',Address = '" + Address + "',District = '" + District + "' where Stage_Name = '" + old_stageName + "' and Address = '" + old_address + "' and District = '" + old_Dist + "'";
                                SqlCommand cmd_new = new SqlCommand(updatequery, con);
                                cmd_new.ExecuteNonQuery();
                                con.Close();
                            }
                            else
                            {
                                con.Close();
                                con.Open();
                                string updatequery = string.Empty;
                                updatequery = "update Stage_Master set Stage_Name = '" + stagename + "',Address = '" + Address + "',District = '" + District + "' where Stage_Name = '" + old_stageName + "' and Address = '" + old_address + "' and District = '" + old_Dist + "'";
                                SqlCommand cmd_new = new SqlCommand(updatequery, con);
                                cmd_new.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                        else
                        {
                            con.Close();
                            con.Open();
                            string sqlquery = "";
                            sqlquery = "insert into Stage_Master(Stage_Name,Address,District) values('" + stagename + "','" + Address + "','" + District + "')";
                            SqlCommand cmd = new SqlCommand(sqlquery, con);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    else
                    {
                        con.Close();
                        con.Open();
                        string sqlquery = "";
                        sqlquery = "insert into Stage_Master(Stage_Name,Address,District) values('" + stagename + "','" + Address + "','" + District + "')";
                        SqlCommand cmd = new SqlCommand(sqlquery, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                }
            }
            //clear();
            GetRecord();
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
        }
        else
        {
            lblerror.Visible = true;
            lblerror.Text = "Enter the Stage Details";
        }


    }

    protected void Buttondelete_Click(object sender, EventArgs e)
    {
        try
        {
            for (int i = 0; i < FpSpreadstage.Sheets[0].RowCount; i++)
            {
                if (FpSpreadstage.Sheets[0].Cells[i, 4].Value != null)
                {
                    if (FpSpreadstage.Sheets[0].Cells[i, 4].Value.ToString() == "1")
                    {
                        string stagename = string.Empty;
                        stagename = Convert.ToString(FpSpreadstage.Sheets[0].Cells[i, 2].Text);
                        stagename = stagename.Replace("'", "''");
                        string Address = Convert.ToString(FpSpreadstage.Sheets[0].Cells[i, 3].Text);
                        string District = Convert.ToString(FpSpreadstage.Sheets[0].GetValue(i, 1));
                        con.Close();
                        con.Open();
                        string deletequery = string.Empty;
                        //deletequery = "delete from Stage_Master where Stage_Name = '" + stagename + "' and Address = '" + Address + "' and District = '" + District + "'";
                        deletequery = "delete from Stage_Master where Stage_Name = '" + stagename + "'  and District = '" + District + "'and (ISNULL(Address,null) is null or ISNULL(Address,'')='"+Address+"')";//rajasekar
                        SqlCommand cmddel = new SqlCommand(deletequery, con);
                        cmddel.ExecuteNonQuery();
                        con.Close();

                    }
                }
            }
            GetRecord();
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted successfully')", true);
        }
        catch
        {

        }
    }

    public void clear()
    {
        FpSpreadstage.Rows.Count = 0;
    }

    protected void ButtonPrint_Click(object sender, EventArgs e)
    {
        FpSpreadstage.Sheets[0].Columns[4].Visible = false;

        Session["column_header_row_count"] = 1;

        string degreedetails = "Stage Master Report";
        string pagename = "StageMaster.aspx";

        Printcontrol.loadspreaddetails(FpSpreadstage, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    protected void ceradd_Click(object sender, EventArgs e)
    {
        Panelceradd.Visible = true;
    }

    protected void cerremove_Click(object sender, EventArgs e)
    {
        Labelvalidation.Visible = false;
        if (ddlcertificate.SelectedItem.Text != "")
        {
            hastab.Clear();

            hastab.Add("tcrit", "Sdis");
            hastab.Add("tval", ddlcertificate.SelectedItem.Text);
            hastab.Add("tcode", ddlcertificate.SelectedItem.Value.ToString());
            hastab.Add("collegecode", "0");
            d1 = dset.select_method("enquiry_delete_textcode", hastab, "sp");
            if (d1.Tables.Count > 0)
            {
                if (d1.Tables[0].Rows.Count > 0)
                {
                    ddlcertificate.Items.Clear();
                    ddlcertificate.DataSource = d1;
                    ddlcertificate.DataTextField = "Textval";
                    ddlcertificate.DataValueField = "textcode";
                    ddlcertificate.DataBind();
                    ddlcertificate.Items.Insert(0, "");
                }
            }
        }
    }

    protected void exitcernew_Click(object sender, EventArgs e)
    {
        Panelceradd.Visible = false;
    }

    protected void ddlvehicletypeview_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        string sqlquery = string.Empty;
        ddltypeview.Items.Clear();
        ddltypeview.Items.Insert(0, new ListItem("All", "-1"));
        if (ddlvehicletypeview.Text == "-1")
        {
            sqlquery = "select * from stage_master";
        }
        else
        {
            sqlquery = "select * from stage_master where District = '" + ddlvehicletypeview.SelectedValue.ToString() + "'";
        }
        ds = da.select_method_wo_parameter(sqlquery, "txt");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ddltypeview.Items.Add(ds.Tables[0].Rows[i]["Stage_Name"].ToString());
            }
            ddltypeview.SelectedIndex = 0;
        }
        con.Close();
    }

    protected void btnMainGo_Click(object sender, EventArgs e)
    {
        FpSpreadstage.Sheets[0].RowCount = 0;
        string typeall = string.Empty;
        string veh_all = string.Empty;
        if (ddlvehicletypeview.Text == "-1")
        {
            for (int i = 0; i < ddlvehicletypeview.Items.Count; i++)
            {
                if (i > 0)
                {
                    if (typeall == "")
                    {
                        typeall = ddlvehicletypeview.Items[i].Value.ToString();
                    }
                    else
                    {
                        typeall = typeall + "','" + ddlvehicletypeview.Items[i].Value.ToString();
                    }
                }
            }

        }
        else
        {
            typeall = ddlvehicletypeview.Text.ToString();
        }
        if (ddltypeview.Text == "-1")
        {
            for (int i = 0; i < ddltypeview.Items.Count; i++)
            {
                if (i > 0)
                {
                    if (veh_all == "")
                    {
                        //modified by srinath 17/2/2014
                        string getstagename = ddltypeview.Items[i].Text.ToString();
                        getstagename = getstagename.Replace("'", "''");
                        veh_all = getstagename;


                    }
                    else
                    {  //modified by srinath 17/2/2014
                        string getstagename = ddltypeview.Items[i].Text.ToString();
                        getstagename = getstagename.Replace("'", "''");
                        veh_all = veh_all + "','" + getstagename;
                    }
                }
            }
        }
        else
        {
            veh_all = ddltypeview.Text.ToString();
        }
        int temp = -1;
        int temptemp = 0;
        string getdate = "";
        getdate = "select * from Stage_Master where District in('" + typeall + "') and Stage_Name in('" + veh_all + "')";
        SqlDataAdapter dr = new SqlDataAdapter(getdate, con);
        DataSet dt = new DataSet();
        dr.Fill(dt);
        if (dt.Tables[0].Rows.Count > 0)
        {
            Btnprint.Enabled = true;
            Buttondelete.Enabled = false;
            FpSpreadstage.Sheets[0].RowCount = dt.Tables[0].Rows.Count;

            string sqlcmd = string.Empty;
            //select distinct textcode,textval from textvaltable where textcriteria in ('Sdis','dis') modified by prabha
            sqlcmd = "select distinct textcode,textval from textvaltable where textcriteria in ('Sdis','dis')";
            DataSet dnew = new DataSet();
            dnew = dset.select_method_wo_parameter(sqlcmd, "text");
            for (int i2 = 0; i2 < dt.Tables[0].Rows.Count; i2++)
            {
                temp++;
                temptemp = temp + 1;
                FpSpreadstage.Sheets[0].Cells[i2, 0].Text = Convert.ToInt32(temptemp).ToString();
                FpSpreadstage.Sheets[0].Cells[i2, 2].Text = dt.Tables[0].Rows[i2]["Stage_Name"].ToString();
                FpSpreadstage.Sheets[0].Cells[i2, 2].Tag = dt.Tables[0].Rows[i2]["Stage_id"].ToString();
                FpSpreadstage.Sheets[0].Cells[i2, 3].Text = dt.Tables[0].Rows[i2]["Address"].ToString();
                FarPoint.Web.Spread.ComboBoxCellType cmbcol = new FarPoint.Web.Spread.ComboBoxCellType();
                cmbcol.ShowButton = true;
                cmbcol.UseValue = true;
                cmbcol.DataSource = dnew;
                cmbcol.DataTextField = "textval";
                cmbcol.DataValueField = "textcode";
                FpSpreadstage.Sheets[0].Columns[1].CellType = cmbcol;
                FpSpreadstage.Sheets[0].Cells[i2, 1].Text = Convert.ToString(dt.Tables[0].Rows[i2]["District"]);
                FpSpreadstage.Sheets[0].Cells[i2, 4].Value = 0;
            }


            FpSpreadstage.Sheets[0].PageSize = FpSpreadstage.Sheets[0].RowCount;
            btnsave.Enabled = true;
            Buttondelete.Enabled = true;
        }
        FpSpreadstage.SaveChanges();

    }

    protected void addcernew_Click(object sender, EventArgs e)
    {
        Labelvalidation.Visible = false;
        Panelceradd.Visible = false;
        if (tbaddcer.Text != "")
        {
            hastab.Clear();
            hastab.Add("dis_val", tbaddcer.Text.Trim());
            d1 = dset.select_method("add_district", hastab, "sp");
            if (d1.Tables.Count > 0)
            {
                if (d1.Tables[0].Rows.Count > 0)
                {
                    ddlcertificate.Items.Clear();
                    ddlcertificate.DataSource = d1;
                    ddlcertificate.DataTextField = "District_Name";
                    ddlcertificate.DataValueField = "District_Code";
                    ddlcertificate.DataBind();
                    ddlcertificate.Items.Insert(0, "");
                }
            }
            tbaddcer.Text = "";

        }
    }

}