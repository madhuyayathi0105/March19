using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class MultipleStage : System.Web.UI.Page
{
    string strquery = "";
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Boolean flag_true = false;
    Hashtable hat = new Hashtable();
    string PageLogOut = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
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
            errmsg.Visible = false;
            FpSpread1.SaveChanges();
            if (!IsPostBack)
            {
                FpSpread1.CommandBar.Visible = false;
                FpSpread1.Sheets[0].SheetName = " ";
                FpSpread1.Sheets[0].SheetCorner.Columns[0].Visible = false;
                FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;

                FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
                style1.Font.Size = 12;
                style1.Font.Bold = true;
                style1.HorizontalAlign = HorizontalAlign.Center;
                style1.ForeColor = System.Drawing.Color.Black;
                FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].AllowTableCorner = true;


                FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                FpSpread1.Pager.Align = HorizontalAlign.Right;
                FpSpread1.Pager.Font.Bold = true;
                FpSpread1.Pager.Font.Name = "Book Antiqua";
                FpSpread1.Pager.ForeColor = System.Drawing.Color.DarkGreen;
                FpSpread1.Pager.BackColor = System.Drawing.Color.Beige;
                FpSpread1.Pager.BackColor = System.Drawing.Color.AliceBlue;
                FpSpread1.Pager.PageCount = 5;
                FpSpread1.Visible = false;


                FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                FpSpread1.Pager.Align = HorizontalAlign.Left;
                FpSpread1.Pager.Font.Bold = true;
                FpSpread1.Pager.Font.Name = "Book Antiqua";
                FpSpread1.Pager.ForeColor = Color.DarkGreen;
                // FpSpread1.Pager.BackColor = Color.Beige;
                // FpSpread1.Pager.BackColor = Color.AliceBlue;
                FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
                FpSpread1.SheetCorner.Columns[0].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;

                loadstage();

            }
        }
        catch
        {
        }
    }

    public void loadstage()
    {
        try
        {
            strquery = "SELECT DISTINCT Stage_ID,S.Stage_Name FROM RouteMaster R,Vehicle_Master V,Stage_Master S WHERE R.Veh_ID = V.Veh_ID AND R.Stage_Name = S.Stage_ID";
            ds.Reset();
            ds.Dispose();

            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlstage.DataSource = ds;
                ddlstage.DataTextField = "Stage_Name";
                ddlstage.DataValueField = "Stage_ID";
                ddlstage.DataBind();
                ddlstage.Items.Add("All");
                ddlstage.SelectedValue = "All";
                bindspread();
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
            }
        }
        catch
        {
        }
    }

    protected void ddlstage_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindspread();
    }
    public void bindspread()
    {
        try
        {
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            strquery = "SELECT DISTINCT Stage_ID,S.Stage_Name,R.Veh_ID,len(r.Veh_ID),(select priority from tbl_vechile_priority p where p.veh_id=r.veh_id and p.stage_id=s.Stage_ID) as periorty FROM RouteMaster R,Vehicle_Master V,Stage_Master S WHERE R.Veh_ID = V.Veh_ID AND R.Stage_Name = S.Stage_ID";
            if (ddlstage.SelectedItem.ToString() != "All")
            {
                strquery = strquery + " and R.Stage_Name = '" + ddlstage.SelectedValue.ToString() + "' ";
            }
            strquery = strquery + " order by S.Stage_Name,len(r.Veh_ID),R.Veh_ID";
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
                btnprintmaster.Visible = true;
                Printcontrol.Visible = false;
                FpSpread1.Visible = true;
                btnsave.Enabled = true;
                btnreset.Enabled = true;

                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread1.Sheets[0].ColumnCount = 5;
                FpSpread1.Sheets[0].RowCount = 0;

                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Rows[0].BackColor = Color.AliceBlue;
                FpSpread1.Sheets[0].ColumnHeader.Rows[0].Height = 25;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Stage";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Vehicle";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Select";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Priority";

                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                FpSpread1.Sheets[0].Columns[0].CellType = txt;
                FpSpread1.Sheets[0].Columns[1].CellType = txt;
                FpSpread1.Sheets[0].Columns[2].CellType = txt;
                FpSpread1.Sheets[0].Columns[4].CellType = txt;
                FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
                chk.AutoPostBack = true;
                FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

                FpSpread1.Sheets[0].Columns[0].Width = 75;
                FpSpread1.Sheets[0].Columns[1].Width = 250;
                FpSpread1.Sheets[0].Columns[2].Width = 150;
                FpSpread1.Sheets[0].Columns[3].Width = 50;
                FpSpread1.Sheets[0].Columns[4].Width = 100;

                int srno = 0;
                string stageid = "";
                string priority = "";

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    srno++;
                    stageid = ds.Tables[0].Rows[i]["Stage_ID"].ToString();
                    priority = ds.Tables[0].Rows[i]["periorty"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["Stage_Name"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = stageid;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["Veh_ID"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = chk;
                    if (priority.Trim() != "0" && priority.Trim() != "" && priority.Trim().ToLower() != null)
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = priority;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "";
                    }
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                }

            }
            else
            {
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                Printcontrol.Visible = false;
                FpSpread1.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "no Records Found";
                btnsave.Enabled = false;
                btnreset.Enabled = false;
            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;

            FpSpread1.SaveChanges();
            int h1 = 100;
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                h1 = h1 + FpSpread1.Sheets[0].Rows[i].Height;
            }

            if (h1 < 500)
            {
                FpSpread1.Height = h1;
            }
            else
            {
                FpSpread1.Height = 500;
            }

            h1 = 20;
            for (int i = 0; i < FpSpread1.Sheets[0].ColumnCount; i++)
            {
                h1 = h1 + FpSpread1.Sheets[0].Columns[i].Width;
            }
            FpSpread1.Width = h1;
        }
        catch
        {
        }
    }

    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
    protected void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            FpSpread1.SaveChanges();
            string activrow = "";
            activrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            if (activrow != "" && activrow != "-1")
            {
                string stage = FpSpread1.Sheets[0].Cells[int.Parse(activrow), 1].Text.ToString();
                int hy_order = 0;
                for (int i = 0; i <= Convert.ToInt16(FpSpread1.Sheets[0].RowCount) - 1; i++)
                {
                    string tempstage = FpSpread1.Sheets[0].Cells[i, 1].Text.ToString();
                    //if (stage == tempstage)
                    //{
                    int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 3].Value);
                    if (isval == 1)
                    {
                        hy_order++;
                        FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), 3].Locked = true;
                    }
                    //  }
                }
                if (hy_order > 0)
                {
                    FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), 4].Text = hy_order.ToString();
                }
            }

        }
        catch
        {
        }
    }

    protected void FpSpread1_CellClick(object sender, EventArgs e)
    {
        flag_true = true;
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            int save = 0;
            string priorityval = "";
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                hat.Clear();
                hat.Add("veh_id", FpSpread1.Sheets[0].Cells[i, 2].Text.ToString());
                hat.Add("stage_id", FpSpread1.Sheets[0].Cells[i, 1].Tag.ToString());
                priorityval = FpSpread1.Sheets[0].Cells[i, 4].Text.ToString();
                if (priorityval == "" || priorityval == null)
                {
                    priorityval = "0";
                }
                hat.Add("priority", priorityval);
                save = d2.insert_method("sp_ins_upd_vechiclepriority", hat, "sp");
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Saved successfully')", true);
        }
        catch
        {
        }
    }
    protected void btnreset_Click(object sender, EventArgs e)
    {
        try
        {
            int save = 0;
            string priorityval = "";
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                hat.Clear();
                hat.Add("veh_id", FpSpread1.Sheets[0].Cells[i, 2].Text.ToString());
                hat.Add("stage_id", FpSpread1.Sheets[0].Cells[i, 1].Tag.ToString());
                hat.Add("priority", priorityval);
                save = d2.insert_method("sp_ins_upd_vechiclepriority", hat, "sp");
            }
            bindspread();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Reseted successfully')", true);
        }
        catch { }
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name";
                errmsg.Visible = true;
            }
        }
        catch
        {
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Printcontrol.loadspreaddetails(FpSpread1, "vechilepriority.aspx", "Vechile Priority Settings");
        Printcontrol.Visible = true;
    }
}