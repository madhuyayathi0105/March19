using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Configuration;

public partial class HeaderColumnSettings : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string college_code = String.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    bool Cellclick = false;
    int i = 0;
    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
    FarPoint.Web.Spread.DoubleCellType txtfontsize = new FarPoint.Web.Spread.DoubleCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkisBold = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkSel = new FarPoint.Web.Spread.CheckBoxCellType();
    string PageLogOut = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        //string sess = Convert.ToString(Session["IsLogin"]);
        //PageLogOut = Convert.ToString(Session["PageLogout"]);
        //if (sess == "")
        //{
        //}
        //else
        //{
        //    if (!Request.FilePath.Contains("OffM"))
        //    {
        //        string strPreviousPage = "";
        //        if (Request.UrlReferrer != null)
        //        {
        //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
        //        }
        //        if (strPreviousPage == "")
        //        {
        //            string redrURI = ConfigurationManager.AppSettings["Office"].Trim();
        //            Response.Redirect(redrURI, false);
        //            return;
        //        }
        //    }
        //}
        //if (Session["collegecode"] == null)
        //{
        //    string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
        //    Response.Redirect(redrURI, false);
        //    return;
        //}
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("OfficeHome"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/OfficeMOD/OfficeHome.aspx");
                return;
            }
        }
        usercode = Session["usercode"].ToString();
        college_code = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();

        if (!IsPostBack)
        {
            bindcollege();
            txtMod.Text = "Module Name(" + Convert.ToString(cblMod.Items.Count) + ")";
        }
        lbl_Error.Visible = false;
        lbl_PopError.Visible = false;
        lblvalidation1.Visible = false;
    }

    protected void chkcoll_change(object sender, EventArgs e)
    {
        ChkChange(chkcoll, chklstcoll, txtcoll, "College Name");
    }

    protected void chklstcoll_change(object sender, EventArgs e)
    {
        ChkLstChange(chkcoll, chklstcoll, txtcoll, "College Name");
    }

    protected void cbMod_change(object sender, EventArgs e)
    {
        ChkChange(cbMod, cblMod, txtMod, "Module Name");
    }

    protected void cblMod_change(object sender, EventArgs e)
    {
        ChkLstChange(cbMod, cblMod, txtMod, "Module Name");
    }

    protected void btngo_click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            rptprint.Visible = false;
            lbl_Error.Visible = false;
            string myColVal = String.Empty;
            string myModVal = String.Empty;

            if (String.IsNullOrEmpty(GetSelectedItemsValueAsString(chklstcoll)))
            {
                lbl_Error.Visible = true;
                lbl_Error.Text = "Please Select Any College!";
                return;
            }

            if (String.IsNullOrEmpty(GetSelectedItemsText(cblMod)))
            {
                lbl_Error.Visible = true;
                lbl_Error.Text = "Please Select Any Module!";
                return;
            }
            myColVal = "'" + GetSelectedItemsValueAsString(chklstcoll) + "'";
            myModVal = "'" + GetSelectedItemsText(cblMod) + "'";
            string GetVal = "select (select collname from collinfo where college_code=Col_Hdr_Settings.college_code) as CollName,* from Col_Hdr_Settings where college_code in(" + myColVal + ") and Mod_Name in(" + myModVal + ") order by CollName,Mod_Name";
            ds.Clear();
            ds = d2.select_method_wo_parameter(GetVal, "Text");
            LoadMainHeader(ds);
        }
        catch { }
    }

    private void LoadMainHeader(DataSet mynewDs)
    {
        try
        {
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.RowHeader.Visible = false;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].ColumnCount = 6;

            darkstyle.Font.Bold = true;
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.ForeColor = Color.Black;
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = darkstyle;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "College Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Module Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Header Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Font Size";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Is Bold";
            FpSpread1.Sheets[0].Columns[0].Width = 75;
            FpSpread1.Sheets[0].Columns[1].Width = 150;
            FpSpread1.Sheets[0].Columns[2].Width = 150;
            FpSpread1.Sheets[0].Columns[3].Width = 150;
            FpSpread1.Sheets[0].Columns[4].Width = 100;
            FpSpread1.Sheets[0].Columns[5].Width = 100;

            if (mynewDs.Tables.Count > 0 && mynewDs.Tables[0].Rows.Count > 0)
            {
                for (int ro = 0; ro < mynewDs.Tables[0].Rows.Count; ro++)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ro + 1);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(mynewDs.Tables[0].Rows[ro]["CollName"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(mynewDs.Tables[0].Rows[ro]["college_code"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(mynewDs.Tables[0].Rows[ro]["Mod_Name"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(mynewDs.Tables[0].Rows[ro]["Hdr_Name"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(mynewDs.Tables[0].Rows[ro]["Hdr_Font_Size"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                    if (Convert.ToString(mynewDs.Tables[0].Rows[ro]["Is_Bold"]).Trim().ToLower() == "true")
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "Yes";
                    else
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "No";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                }
                FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.Width = 750;
                FpSpread1.Visible = true;
                rptprint.Visible = true;
            }
            else
            {
                FpSpread1.Visible = false;
                rptprint.Visible = false;
                lbl_Error.Visible = true;
                lbl_Error.Text = "No Records Found!";
            }
        }
        catch { }
    }

    protected void btnaddnew_click(object sender, EventArgs e)
    {
        popper1.Visible = true;
        FpSpread2.Visible = false;
        btnupdate.Visible = false;
        btnsave.Visible = false;
        btnAddRows.Visible = false;
        btnRemRows.Visible = false;
        chkleft_Logo.Visible = false;
        chkright_Logo.Visible = false;
        btnexit.Visible = false;
        btndelete.Visible = false;
        lbl_PopError.Visible = false;
        txtRows.Text = "";
        bindpopcollege();
        for (int ik = 0; ik < cblModPop.Items.Count; ik++)
        {
            cblModPop.Items[ik].Selected = true;
        }
        txtModPop.Text = "Module Name(" + Convert.ToString(cblModPop.Items.Count) + ")";
        cbModPop.Checked = true;
    }

    protected void btngoPop_click(object sender, EventArgs e)
    {
        try
        {
            DataSet myDs = new DataSet();
            myDs.Clear();
            lbl_PopError.Visible = false;

            if (String.IsNullOrEmpty(GetSelectedItemsValueAsString(chklstcollPop)))
            {
                lbl_PopError.Visible = true;
                lbl_PopError.Text = "Please Select Any College!";
                return;
            }

            if (String.IsNullOrEmpty(GetSelectedItemsText(cblModPop)))
            {
                lbl_PopError.Visible = true;
                lbl_PopError.Text = "Please Select Any Module!";
                return;
            }

            if (String.IsNullOrEmpty(txtRows.Text.Trim()) || txtRows.Text.Trim() == "0")
            {
                lbl_PopError.Visible = true;
                lbl_PopError.Text = "Please Enter the No of Rows!";
                return;
            }
            LoadHeader(txtRows.Text.Trim(), myDs);
        }
        catch { }
    }

    private void LoadHeader(string Rows, DataSet newDs)
    {
        FpSpread2.Visible = false;
        btnupdate.Visible = false;
        btnsave.Visible = false;
        btnexit.Visible = false;
        btnAddRows.Visible = false;
        btnRemRows.Visible = false;
        btndelete.Visible = false;
        chkleft_Logo.Visible = false;
        chkright_Logo.Visible = false;
        bool ChkLogo = false;

        FpSpread2.Sheets[0].AutoPostBack = false;
        FpSpread2.RowHeader.Visible = false;
        FpSpread2.CommandBar.Visible = false;
        FpSpread2.Sheets[0].RowCount = 0;
        FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
        FpSpread2.Sheets[0].ColumnCount = 5;

        darkstyle.Font.Bold = true;
        darkstyle.Font.Size = FontUnit.Medium;
        darkstyle.Font.Name = "Book Antiqua";
        darkstyle.HorizontalAlign = HorizontalAlign.Center;
        darkstyle.ForeColor = Color.Black;
        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        FpSpread2.Sheets[0].ColumnHeader.DefaultStyle = darkstyle;
        FarPoint.Web.Spread.TextCellType txtCells = new FarPoint.Web.Spread.TextCellType();

        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Header Name";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Font Size";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Is Bold";
        FpSpread2.Sheets[0].Columns[0].Width = 75;
        FpSpread2.Sheets[0].Columns[1].Width = 75;
        FpSpread2.Sheets[0].Columns[2].Width = 475;
        FpSpread2.Sheets[0].Columns[3].Width = 100;
        FpSpread2.Sheets[0].Columns[4].Width = 75;
        FpSpread2.Columns[0].Locked = true;
        txtfontsize.MaximumValue = 20;
        txtfontsize.ErrorMessage = "Allow only Numerics & Font Size Should be less than or Equal to 20!";
        int myRow = 0;
        Int32.TryParse(Rows, out myRow);
        if (Cellclick == false)
        {
            DataSet myNewDs = new DataSet();
            string AddWithLbl = "";
            string MyLbl = "";

            string selQ = "select distinct Mod_Name,(select Coll_acronymn from collinfo where college_Code=Col_Hdr_Settings.college_code) as CollName,College_Code from Col_Hdr_Settings where college_Code in('" + GetSelectedItemsValueAsString(chklstcollPop) + "') and Mod_Name in('" + GetSelectedItemsText(cblModPop) + "') group by College_Code,Mod_Name order by college_code,Mod_Name";
            myNewDs.Clear();
            myNewDs = d2.select_method_wo_parameter(selQ, "Text");
            if (myNewDs.Tables.Count > 0 && myNewDs.Tables[0].Rows.Count > 0)
            {
                for (int chk = 0; chk < myNewDs.Tables[0].Rows.Count; chk++)
                {
                    if (AddWithLbl.Trim() == "")
                        AddWithLbl = Convert.ToString(myNewDs.Tables[0].Rows[chk]["Mod_Name"]) + "-" + Convert.ToString(myNewDs.Tables[0].Rows[chk]["CollName"]);
                    else
                        AddWithLbl = AddWithLbl + "<br/>" + Convert.ToString(myNewDs.Tables[0].Rows[chk]["Mod_Name"]) + "-" + Convert.ToString(myNewDs.Tables[0].Rows[chk]["CollName"]);
                }
            }
            if (!String.IsNullOrEmpty(AddWithLbl))
                MyLbl = "You have been Already Added the Following <br/>" + AddWithLbl.Replace("-", " for ");
            else
                MyLbl = "";
            if (!String.IsNullOrEmpty(MyLbl))
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = MyLbl;
            }
        }
        if (newDs.Tables.Count > 0 && newDs.Tables[0].Rows.Count > 0)
        {
            for (int ik = 0; ik < newDs.Tables[0].Rows.Count; ik++)
            {
                FpSpread2.Sheets[0].RowCount++;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ik + 1);
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = chkSel;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(newDs.Tables[0].Rows[ik]["Hdr_Name"]);
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].CellType = txtfontsize;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(newDs.Tables[0].Rows[ik]["Hdr_Font_Size"]);
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].CellType = chkisBold;
                if (Convert.ToString(newDs.Tables[0].Rows[ik]["Is_Bold"]).Trim().ToLower() == "true")
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Value = 1;
                else
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Value = 0;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                if (ChkLogo == false)
                {
                    if (Convert.ToString(newDs.Tables[0].Rows[ik]["Is_LeftLogo"]).Trim().ToLower() == "true")
                        chkleft_Logo.Checked = true;
                    else
                        chkleft_Logo.Checked = false;

                    if (Convert.ToString(newDs.Tables[0].Rows[ik]["Is_RightLogo"]).Trim().ToLower() == "true")
                        chkright_Logo.Checked = true;
                    else
                        chkright_Logo.Checked = false;
                    ChkLogo = true;
                }
            }
            FpSpread2.Sheets[0].Columns[1].Visible = true;
            FpSpread2.Width = 825;
            btnupdate.Visible = true;
            btndelete.Visible = true;
            btnexit.Visible = true;
        }
        else
        {
            for (int ik = 0; ik < myRow; ik++)
            {
                FpSpread2.Sheets[0].RowCount++;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ik + 1);
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Value = 0;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = "";
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].CellType = txtfontsize;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].CellType = chkisBold;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Value = 0;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
            }
            chkleft_Logo.Checked = false;
            chkright_Logo.Checked = false;
            FpSpread2.Sheets[0].Columns[1].Visible = false;
            FpSpread2.Width = 745;
            btnsave.Visible = true;
            btnexit.Visible = true;
        }
        FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
        FpSpread2.Visible = true;
        btnAddRows.Visible = true;
        btnRemRows.Visible = true;
        chkleft_Logo.Visible = true;
        chkright_Logo.Visible = true;
    }

    protected void btnAddRows_Click(object sender, EventArgs e)
    {
        try
        {
            if (FpSpread2.Visible == true)
            {
                txtfontsize.MaximumValue = 20;
                txtfontsize.ErrorMessage = "Allow only Numerics & Font Size Should be less than or Equal to 20!";

                int mySlno = FpSpread2.Sheets[0].RowCount++;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(mySlno + 1);
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = chkSel;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Value = 0;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = "";
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].CellType = txtfontsize;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].CellType = chkisBold;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Value = 0;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
            }
        }
        catch { }
    }

    protected void btnRemRows_Click(object sender, EventArgs e)
    {
        try
        {
            if (FpSpread2.Visible == true)
            {
                FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Remove();
                FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
            }
        }
        catch { }
    }

    private bool ValidateSpread()
    {
        bool EntryFlag = true;
        string HdrVal = "";
        string FontVal = "";
        try
        {
            FpSpread2.SaveChanges();
            for (int myRo = 0; myRo < FpSpread2.Rows.Count; myRo++)
            {
                HdrVal = Convert.ToString(FpSpread2.Sheets[0].Cells[myRo, 2].Text);
                FontVal = Convert.ToString(FpSpread2.Sheets[0].Cells[myRo, 3].Text);
                if (String.IsNullOrEmpty(HdrVal) || String.IsNullOrEmpty(FontVal))
                {
                    EntryFlag = false;
                }
            }
        }
        catch { }
        return EntryFlag;
    }

    private bool ValidateSelSpread()
    {
        bool EntryFlag = true;
        string HdrVal = "";
        string FontVal = "";
        try
        {
            FpSpread2.SaveChanges();
            for (int myRo = 0; myRo < FpSpread2.Rows.Count; myRo++)
            {
                byte Val = Convert.ToByte(FpSpread2.Sheets[0].Cells[myRo, 1].Value);
                if (Val == 1)
                {
                    HdrVal = Convert.ToString(FpSpread2.Sheets[0].Cells[myRo, 2].Text);
                    FontVal = Convert.ToString(FpSpread2.Sheets[0].Cells[myRo, 3].Text);
                    if (String.IsNullOrEmpty(HdrVal) || String.IsNullOrEmpty(FontVal))
                    {
                        EntryFlag = false;
                    }
                }
            }
        }
        catch { }
        return EntryFlag;
    }

    private bool SpreadSelChk()
    {
        bool SelChk = false;
        try
        {
            FpSpread2.SaveChanges();
            for (int myRo = 0; myRo < FpSpread2.Rows.Count; myRo++)
            {
                byte Val = Convert.ToByte(FpSpread2.Sheets[0].Cells[myRo, 1].Value);
                if (Val == 1)
                {
                    SelChk = true;
                }
            }
        }
        catch { }
        return SelChk;
    }

    protected void FpSpread1_Click(object sender, EventArgs e)
    {
        Cellclick = true;
    }

    protected void FpSpread1_Render(object sender, EventArgs e)
    {
        try
        {
            if (Cellclick == true)
            {
                string actrow = Convert.ToString(FpSpread1.Sheets[0].ActiveRow);
                string actcol = Convert.ToString(FpSpread1.Sheets[0].ActiveColumn);
                string CollVal = "";
                string CollName = "";
                string ModName = "";
                bindpopcollege();

                if (!String.IsNullOrEmpty(actrow) && !actrow.Contains("-"))
                {
                    CollVal = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 1].Tag);
                    CollName = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 1].Text);
                    ModName = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Text);
                    popper1.Visible = true;
                    for (int co = 0; co < chklstcollPop.Items.Count; co++)
                    {
                        chklstcollPop.Items[co].Selected = false;
                    }
                    for (int co = 0; co < cblModPop.Items.Count; co++)
                    {
                        cblModPop.Items[co].Selected = false;
                    }
                    for (int co = 0; co < chklstcollPop.Items.Count; co++)
                    {
                        if (chklstcollPop.Items[co].Value == CollVal)
                        {
                            chklstcollPop.Items[co].Selected = true;
                        }
                    }
                    for (int co = 0; co < cblModPop.Items.Count; co++)
                    {
                        if (cblModPop.Items[co].Text == ModName)
                        {
                            cblModPop.Items[co].Selected = true;
                        }
                    }
                    txtCollPop.Text = Convert.ToString(CollName);
                    txtModPop.Text = Convert.ToString(ModName);
                    if (chklstcollPop.Items.Count == 1)
                        chkcollPop.Checked = true;
                    else
                        chkcollPop.Checked = false;
                    cbModPop.Checked = false;
                    string GetVal = "select (select collname from collinfo where college_code=Col_Hdr_Settings.college_code) as CollName,* from Col_Hdr_Settings where college_code ='" + CollVal + "' and Mod_Name ='" + ModName + "' order by CollName,Mod_Name";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(GetVal, "Text");
                    txtRows.Text = Convert.ToString(ds.Tables[0].Rows.Count);
                    LoadHeader(Convert.ToString(ds.Tables[0].Rows.Count), ds);
                }
            }
        }
        catch { }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            string HdrVal = "";
            string FontVal = "";
            string isBold = "0";
            string insQ = "";
            int inscount = 0;
            int myInsCount = 0;
            string isLeftlogo = "0";
            string isRightlogo = "0";
            if (chkleft_Logo.Checked == true)
                isLeftlogo = "1";
            if (chkright_Logo.Checked == true)
                isRightlogo = "1";

            if (FpSpread2.Rows.Count > 0)
            {
                if (ValidateSpread() == true)
                {
                    FpSpread2.SaveChanges();
                    for (int myRo = 0; myRo < FpSpread2.Rows.Count; myRo++)
                    {
                        HdrVal = Convert.ToString(FpSpread2.Sheets[0].Cells[myRo, 2].Text);
                        FontVal = Convert.ToString(FpSpread2.Sheets[0].Cells[myRo, 3].Text);
                        isBold = Convert.ToString(FpSpread2.Sheets[0].Cells[myRo, 4].Value);

                        for (int myco = 0; myco < chklstcollPop.Items.Count; myco++)
                        {
                            if (chklstcollPop.Items[myco].Selected == true)
                            {
                                for (int mymo = 0; mymo < cblModPop.Items.Count; mymo++)
                                {
                                    if (cblModPop.Items[mymo].Selected == true)
                                    {
                                        insQ = "if exists (select * from Col_Hdr_Settings where college_code ='" + Convert.ToString(chklstcollPop.Items[myco].Value) + "' and Mod_Name='" + Convert.ToString(cblModPop.Items[mymo].Text) + "' and Hdr_Name='" + HdrVal + "') update Col_Hdr_Settings set Hdr_Font_Size='" + FontVal + "',Is_Bold='" + isBold + "',Is_LeftLogo='" + isLeftlogo + "',Is_RightLogo='" + isRightlogo + "' where college_code ='" + Convert.ToString(chklstcollPop.Items[myco].Value) + "' and Mod_Name='" + Convert.ToString(cblModPop.Items[mymo].Text) + "' and Hdr_Name='" + HdrVal + "' else insert into Col_Hdr_Settings (Mod_Name,Hdr_Name,Hdr_Font_Size,Is_Bold,college_code,Is_LeftLogo,Is_RightLogo) Values ('" + Convert.ToString(cblModPop.Items[mymo].Text) + "','" + HdrVal + "','" + FontVal + "','" + isBold + "','" + Convert.ToString(chklstcollPop.Items[myco].Value) + "','" + isLeftlogo + "','" + isRightlogo + "')";
                                        inscount = d2.update_method_wo_parameter(insQ, "Text");
                                        if (inscount > 0)
                                            myInsCount += 1;
                                    }
                                }
                            }
                        }
                    }
                    if (myInsCount > 0)
                    {
                        alertpopwindow.Visible = true;
                        lbl_PopError.Visible = false;
                        lblalerterr.Text = "Column Header Settings Saved Successfully!";
                    }
                }
                else
                {
                    lbl_PopError.Visible = true;
                    lbl_PopError.Text = "Please Fill All the Values!";
                }
            }
        }
        catch { }
    }

    protected void btnupdate_Click(object sender, EventArgs e)
    {
        try
        {
            string HdrVal = "";
            string FontVal = "";
            string isBold = "0";
            string insQ = "";
            int inscount = 0;
            int myInsCount = 0;
            string collVal = "";
            string ModName = "";
            string isLeftlogo = "0";
            string isRightlogo = "0";
            if (chkleft_Logo.Checked == true)
                isLeftlogo = "1";
            if (chkright_Logo.Checked == true)
                isRightlogo = "1";

            if (FpSpread2.Rows.Count > 0)
            {
                if (SpreadSelChk() == true)
                {
                    if (ValidateSelSpread() == true)
                    {
                        FpSpread2.SaveChanges();
                        for (int myRo = 0; myRo < FpSpread2.Rows.Count; myRo++)
                        {
                            byte MyVal = Convert.ToByte(FpSpread2.Sheets[0].Cells[myRo, 1].Value);
                            if (MyVal == 1)
                            {
                                HdrVal = Convert.ToString(FpSpread2.Sheets[0].Cells[myRo, 2].Text);
                                FontVal = Convert.ToString(FpSpread2.Sheets[0].Cells[myRo, 3].Text);
                                isBold = Convert.ToString(FpSpread2.Sheets[0].Cells[myRo, 4].Value);

                                for (int myco = 0; myco < chklstcollPop.Items.Count; myco++)
                                {
                                    if (chklstcollPop.Items[myco].Selected == true)
                                    {
                                        for (int mymo = 0; mymo < cblModPop.Items.Count; mymo++)
                                        {
                                            if (cblModPop.Items[mymo].Selected == true)
                                            {
                                                insQ = "if exists (select * from Col_Hdr_Settings where college_code ='" + Convert.ToString(chklstcollPop.Items[myco].Value) + "' and Mod_Name='" + Convert.ToString(cblModPop.Items[mymo].Text) + "' and Hdr_Name='" + HdrVal + "') update Col_Hdr_Settings set Hdr_Font_Size='" + FontVal + "',Is_Bold='" + isBold + "',Is_LeftLogo='" + isLeftlogo + "',Is_RightLogo='" + isRightlogo + "' where college_code ='" + Convert.ToString(chklstcollPop.Items[myco].Value) + "' and Mod_Name='" + Convert.ToString(cblModPop.Items[mymo].Text) + "' and Hdr_Name='" + HdrVal + "' else insert into Col_Hdr_Settings (Mod_Name,Hdr_Name,Hdr_Font_Size,Is_Bold,college_code,Is_LeftLogo,Is_RightLogo) Values ('" + Convert.ToString(cblModPop.Items[mymo].Text) + "','" + HdrVal + "','" + FontVal + "','" + isBold + "','" + Convert.ToString(chklstcollPop.Items[myco].Value) + "','" + isLeftlogo + "','" + isRightlogo + "')";
                                                inscount = d2.update_method_wo_parameter(insQ, "Text");
                                                if (inscount > 0)
                                                    myInsCount += 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (myInsCount > 0)
                        {
                            alertpopwindow.Visible = true;
                            lbl_PopError.Visible = false;
                            for (int jp = 0; jp < chklstcollPop.Items.Count; jp++)
                            {
                                if (chklstcollPop.Items[jp].Selected == true)
                                {
                                    collVal = Convert.ToString(chklstcollPop.Items[jp].Value);
                                }
                            }
                            ModName = Convert.ToString(txtModPop.Text);
                            string GetVal = "select (select collname from collinfo where college_code=Col_Hdr_Settings.college_code) as CollName,* from Col_Hdr_Settings where college_code ='" + collVal + "' and Mod_Name ='" + ModName + "' order by CollName,Mod_Name";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(GetVal, "Text");
                            txtRows.Text = Convert.ToString(ds.Tables[0].Rows.Count);
                            LoadHeader(Convert.ToString(ds.Tables[0].Rows.Count), ds);
                            lblalerterr.Text = "Column Header Settings Updated Successfully!";
                        }
                    }
                    else
                    {
                        lbl_PopError.Visible = true;
                        lbl_PopError.Text = "Please Fill All the Value!";
                    }
                }
                else
                {
                    lbl_PopError.Visible = true;
                    lbl_PopError.Text = "Please Select Any Header Name!";
                }
            }
        }
        catch { }
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            try
            {
                string HdrVal = "";
                string FontVal = "";
                string isBold = "0";
                string insQ = "";
                int inscount = 0;
                int myInsCount = 0;
                string collVal = "";
                string ModName = "";

                if (FpSpread2.Rows.Count > 0)
                {
                    if (SpreadSelChk() == true)
                    {
                        if (ValidateSelSpread() == true)
                        {
                            FpSpread2.SaveChanges();
                            for (int myRo = 0; myRo < FpSpread2.Rows.Count; myRo++)
                            {
                                byte MyVal = Convert.ToByte(FpSpread2.Sheets[0].Cells[myRo, 1].Value);
                                if (MyVal == 1)
                                {
                                    HdrVal = Convert.ToString(FpSpread2.Sheets[0].Cells[myRo, 2].Text);
                                    FontVal = Convert.ToString(FpSpread2.Sheets[0].Cells[myRo, 3].Text);
                                    isBold = Convert.ToString(FpSpread2.Sheets[0].Cells[myRo, 4].Value);

                                    for (int myco = 0; myco < chklstcollPop.Items.Count; myco++)
                                    {
                                        if (chklstcollPop.Items[myco].Selected == true)
                                        {
                                            for (int mymo = 0; mymo < cblModPop.Items.Count; mymo++)
                                            {
                                                if (cblModPop.Items[mymo].Selected == true)
                                                {
                                                    insQ = " Delete from Col_Hdr_Settings where college_code ='" + Convert.ToString(chklstcollPop.Items[myco].Value) + "' and Mod_Name='" + Convert.ToString(cblModPop.Items[mymo].Text) + "' and Hdr_Name='" + HdrVal + "'";
                                                    inscount = d2.update_method_wo_parameter(insQ, "Text");
                                                    if (inscount > 0)
                                                        myInsCount += 1;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (myInsCount > 0)
                            {
                                alertpopwindow.Visible = true;
                                lbl_PopError.Visible = false;
                                for (int jp = 0; jp < chklstcollPop.Items.Count; jp++)
                                {
                                    if (chklstcollPop.Items[jp].Selected == true)
                                    {
                                        collVal = Convert.ToString(chklstcollPop.Items[jp].Value);
                                    }
                                }
                                ModName = Convert.ToString(txtModPop.Text);
                                string GetVal = "select (select collname from collinfo where college_code=Col_Hdr_Settings.college_code) as CollName,* from Col_Hdr_Settings where college_code ='" + collVal + "' and Mod_Name ='" + ModName + "' order by CollName,Mod_Name";
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(GetVal, "Text");
                                txtRows.Text = Convert.ToString(ds.Tables[0].Rows.Count);
                                LoadHeader(Convert.ToString(ds.Tables[0].Rows.Count), ds);
                                lblalerterr.Text = "Column Header Settings Deleted Successfully!";
                            }
                        }
                        else
                        {
                            lbl_PopError.Visible = true;
                            lbl_PopError.Text = "Please Fill All the Value!";
                        }
                    }
                    else
                    {
                        lbl_PopError.Visible = true;
                        lbl_PopError.Text = "Please Select Any Header Name!";
                    }
                }
            }
            catch { }
        }
        catch { }
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        popper1.Visible = false;
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch { }
        txtexcelname.Text = "";
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Header Column Settings";
            string pagename = "HeaderColumnSettings.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch { }
    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        popper1.Visible = false;
    }

    protected void chkcollPop_change(object sender, EventArgs e)
    {
        ChkChange(chkcollPop, chklstcollPop, txtCollPop, "College Name");
        HideBtns();
    }

    protected void chklstcollPop_change(object sender, EventArgs e)
    {
        ChkLstChange(chkcollPop, chklstcollPop, txtCollPop, "College Name");
        HideBtns();
    }

    protected void cbModPop_change(object sender, EventArgs e)
    {
        ChkChange(cbModPop, cblModPop, txtModPop, "Module Name");
        HideBtns();
    }

    protected void cblModPop_change(object sender, EventArgs e)
    {
        ChkLstChange(cbModPop, cblModPop, txtModPop, "Module Name");
        HideBtns();
    }

    private void HideBtns()
    {
        FpSpread2.Visible = false;
        btnAddRows.Visible = false;
        btnRemRows.Visible = false;
        chkleft_Logo.Visible = false;
        chkright_Logo.Visible = false;
        btnsave.Visible = false;
        btndelete.Visible = false;
        btnupdate.Visible = false;
        btnexit.Visible = false;
    }

    protected void bindcollege()
    {
        try
        {
            ds.Clear();
            chklstcoll.Items.Clear();
            string collName = "";
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklstcoll.DataSource = ds;
                chklstcoll.DataTextField = "collname";
                chklstcoll.DataValueField = "college_code";
                chklstcoll.DataBind();
                if (chklstcoll.Items.Count > 0)
                {
                    for (i = 0; i < chklstcoll.Items.Count; i++)
                    {
                        chklstcoll.Items[i].Selected = true;
                        if (i == 0)
                            collName = Convert.ToString(chklstcoll.Items[i].Text);
                        else
                            collName = "";
                    }
                    if (chklstcoll.Items.Count == 1)
                        txtcoll.Text = collName;
                    else
                        txtcoll.Text = "College Name(" + chklstcoll.Items.Count + ")";
                    chkcoll.Checked = true;
                }
            }
        }
        catch { }
    }

    protected void bindpopcollege()
    {
        try
        {
            ds.Clear();
            chklstcollPop.Items.Clear();
            string collName = "";
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklstcollPop.DataSource = ds;
                chklstcollPop.DataTextField = "collname";
                chklstcollPop.DataValueField = "college_code";
                chklstcollPop.DataBind();
                if (chklstcollPop.Items.Count > 0)
                {
                    for (i = 0; i < chklstcollPop.Items.Count; i++)
                    {
                        chklstcollPop.Items[i].Selected = true;
                        if (i == 0)
                            collName = Convert.ToString(chklstcollPop.Items[i].Text);
                        else
                            collName = "";
                    }
                    if (chklstcollPop.Items.Count == 1)
                        txtCollPop.Text = collName;
                    else
                        txtCollPop.Text = "College Name(" + chklstcollPop.Items.Count + ")";
                    chkcollPop.Checked = true;
                }
            }
        }
        catch { }
    }

    private void ChkChange(CheckBox cbBox, CheckBoxList cbBoxLst, TextBox txtBox, string Label)
    {
        try
        {
            txtBox.Text = "--Select--";
            string ItmName = "";
            if (cbBox.Checked)
            {
                if (cbBoxLst.Items.Count > 0)
                {
                    for (i = 0; i < cbBoxLst.Items.Count; i++)
                    {
                        cbBoxLst.Items[i].Selected = true;
                        ItmName = Convert.ToString(cbBoxLst.Items[i].Text);
                    }
                    if (cbBoxLst.Items.Count == 1)
                    {
                        txtBox.Text = ItmName;
                        cbBox.Checked = true;
                    }
                    else
                    {
                        txtBox.Text = Label + "(" + cbBoxLst.Items.Count + ")";
                        cbBox.Checked = true;
                    }
                }
                else
                {
                    txtBox.Text = "--Select--";
                    cbBox.Checked = false;
                }
            }
            else
            {
                for (i = 0; i < cbBoxLst.Items.Count; i++)
                {
                    cbBoxLst.Items[i].Selected = false;
                }
                txtBox.Text = "--Select--";
                cbBox.Checked = false;
            }
        }
        catch { }
    }

    private void ChkLstChange(CheckBox cbBox, CheckBoxList cbBoxLst, TextBox txtBox, string Label)
    {
        try
        {
            txtBox.Text = "--Select--";
            cbBox.Checked = false;
            int commcount = 0;
            string ItmName = "";
            if (cbBoxLst.Items.Count > 0)
            {
                for (i = 0; i < cbBoxLst.Items.Count; i++)
                {
                    if (cbBoxLst.Items[i].Selected == true)
                    {
                        commcount += 1;
                        ItmName = Convert.ToString(cbBoxLst.Items[i].Text);
                    }
                }
                if (commcount > 0)
                {
                    if (commcount == 1 && commcount == cbBoxLst.Items.Count)
                    {
                        txtBox.Text = ItmName;
                        cbBox.Checked = true;
                    }
                    else if (commcount == 1 && commcount != cbBoxLst.Items.Count)
                    {
                        txtBox.Text = ItmName;
                        cbBox.Checked = false;
                    }
                    else if (commcount != 1 && commcount == cbBoxLst.Items.Count)
                    {
                        txtBox.Text = Label + "(" + commcount + ")";
                        cbBox.Checked = true;
                    }
                    else if (commcount != 1 && commcount != cbBoxLst.Items.Count)
                    {
                        txtBox.Text = Label + "(" + commcount + ")";
                        cbBox.Checked = false;
                    }
                }
            }
        }
        catch { }
    }

    private string GetSelectedItemsValueAsString(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int j = 0; j < cblSelected.Items.Count; j++)
            {
                if (cblSelected.Items[j].Selected == true)
                {
                    if (sbSelected.Length == 0)
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Value));
                    else
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[j].Value));
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }

    private string GetSelectedItemsText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int j = 0; j < cblSelected.Items.Count; j++)
            {
                if (cblSelected.Items[j].Selected == true)
                {
                    if (sbSelected.Length == 0)
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Text));
                    else
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[j].Text));
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
}