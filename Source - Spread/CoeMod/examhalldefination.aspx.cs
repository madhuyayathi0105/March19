using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Configuration;
using System.Drawing;
using System.Data;
public partial class examhalldefination : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    Boolean newroomseats = false;
    Boolean cellclick = false;
    static ArrayList arrybluid = new ArrayList();
    static ArrayList arryfloor = new ArrayList();
    static ArrayList arryhallno = new ArrayList();
    DataSet ds = new DataSet();
    string CollegeCode;
    string building_name = string.Empty;
    string floor_name = string.Empty;
    string hall_name = string.Empty;
    string default_view = string.Empty;
    string arranged_view = string.Empty;
    string mode = string.Empty;
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
    FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.IntegerCellType intgrcel = new FarPoint.Web.Spread.IntegerCellType();
    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("CoeHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/CoeMod/CoeHome.aspx");
                    return;
                }
            }
            //****************************************************//
            cellclick = false;
            CollegeCode = Session["collegecode"].ToString();
            lblroomerror.Visible = false;
            if (!IsPostBack)
            {
                loadtype();
                MyStyle.Font.Size = FontUnit.Medium;
                MyStyle.Font.Name = "Book Antiqua";
                MyStyle.HorizontalAlign = HorizontalAlign.Center;
                MyStyle.Font.Bold = true;
                MyStyle.ForeColor = Color.Black;
                MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                fpspread.Sheets[0].RowCount = 0;
                fpspread.Sheets[0].RowHeader.Visible = false;
                fpspread.CommandBar.Visible = false;
                fpspread.Visible = true;
                bool isGeneral = true;
                if (ddlHallType.Items.Count > 0)
                {
                    int index = ddlHallType.SelectedIndex;
                    switch (index)
                    {
                        case 0:
                            isGeneral = true;
                            break;
                        case 1:
                            isGeneral = false;
                            break;
                        default:
                            isGeneral = true;
                            break;
                    }
                }
                fpspread.Sheets[0].ColumnCount = 8;
                fpspread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                fpspread.Sheets[0].Columns[0].Width = 60;
                fpspread.Sheets[0].Columns[0].Locked = true;
                fpspread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Building Name";
                fpspread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                fpspread.Sheets[0].Columns[1].Width = 120;
                fpspread.Sheets[0].Columns[1].Locked = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Floor Name";
                fpspread.Sheets[0].Columns[2].Width = 120;
                fpspread.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                fpspread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[2].Locked = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Hall No";
                fpspread.Sheets[0].Columns[3].Width = 200;
                fpspread.Sheets[0].AutoPostBack = true;
                fpspread.Sheets[0].Columns[3].Locked = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Actual Students ";
                fpspread.Sheets[0].Columns[4].Width = 300;
                fpspread.Sheets[0].Columns[4].Locked = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Allowed Students";
                fpspread.Sheets[0].Columns[5].Width = 140;
                fpspread.Sheets[0].Columns[5].Locked = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Actual Students (Alternate)";
                fpspread.Sheets[0].Columns[6].Width = 300;
                fpspread.Sheets[0].Columns[6].Locked = true;
                fpspread.Sheets[0].Columns[6].Visible = isGeneral;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Allowed Students (Alternate)";
                fpspread.Sheets[0].Columns[7].Width = 140;
                fpspread.Sheets[0].Columns[7].Locked = isGeneral;
                fpspread.Sheets[0].Columns[7].Visible = false;
                fpspread.Visible = false;
                columnsetting.Visible = false;
                loadfpspread();
            }
        }
        catch(Exception ex)
        {
        }
    }

    protected void ddlbuilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadfloor();
            loadrome();
            selectindexchange();
        }
        catch
        {
        }
    }

    protected void ddlflooring_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadrome();
            selectindexchange();
        }
        catch
        {
        }
    }

    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddltypechange();
            loadfpspread();
        }
        catch
        {
        }
    }

    protected void ddlroom_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            selectindexchange();
        }
        catch
        {
        }
    }

    protected void ddlHallType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            selectindexchange();
        }
        catch
        {
        }
    }

    public void loadtype()
    {
        try
        {
            string collegecode = Session["collegecode"].ToString();
            ddltype.Items.Clear();
            string strmode = string.Empty;
            string strquery = "select distinct type from course where college_code='" + collegecode + "'";
            DataSet dstype = da.select_method_wo_parameter(strquery, "Text");
            if (dstype.Tables[0].Rows.Count > 0)
            {
                ddltype.DataSource = dstype;
                ddltype.DataTextField = "type";
                ddltype.DataBind();
                ddltype.Enabled = true;
                // ddltype.Items.Insert(0, "All");
                //ddltype_SelectedIndexChanged();
                ddltypechange();
            }
            else
            {
                ddltype.Items.Insert(0, "");
                //ddltype.Enabled = false;
            }
            ddltypechange();
        }
        catch
        {
        }
    }

    public void selectindexchange()
    {
        txtroomcolumn.Text = string.Empty;
        txtroomrow.Text = string.Empty;
        loadseat();
        loadfpspread();
    }
    
    public void loadseat()
    {
        try
        {
            bool isGeneral = true;
            if (ddlHallType.Items.Count > 0)
            {
                int index = ddlHallType.SelectedIndex;
                switch (index)
                {
                    case 0:
                        isGeneral = true;
                        break;
                    case 1:
                        isGeneral = false;
                        break;
                    default:
                        isGeneral = true;
                        break;
                }
            }
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.Font.Bold = true;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            int count_actual = 0, count_allot = 0;
            Boolean col_row_alter = false;
            if (ddltype.Enabled == true && ddltype.Items.Count > 0)
            {
                mode = ddltype.SelectedItem.Text.Trim();
            }
            if (ddlbuilding.Items.Count > 0)
            {
                building_name = ddlbuilding.SelectedItem.Text.Trim();
            }
            if (ddlflooring.Items.Count > 0)
            {
                floor_name = ddlflooring.SelectedItem.Text.Trim();
            }
            if (ddlroom.Items.Count > 0)
            {
                hall_name = ddlroom.SelectedItem.Text.Trim();
            }
            intgrcel.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
            intgrcel.ErrorMessage = "Enter valid Seats";
            int ddr = 0, ddc = 0;
            string roomno = ddlroom.Text.ToString().Trim();
            fproomarra.Sheets[0].SheetCorner.ColumnCount = 0;
            fproom.Sheets[0].SheetCorner.ColumnCount = 0;
            string strquery = "select isnull(no_of_columns,'0') as col,isnull(no_of_rows,'0') as row from Room_Detail where Room_Name='" + roomno + "' and College_Code='" + Session["collegecode"].ToString() + "';select * from tbl_room_seats where mode='" + mode + "' and Building_Name='" + building_name + "' and Floor_Name='" + floor_name + "'and Hall_No='" + hall_name + "' ";
            DataSet ds = da.select_method_wo_parameter(strquery, "text");
            string rowtext = txtroomrow.Text.ToString().Trim();
            string coltext = txtroomcolumn.Text.ToString().Trim();
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count == 0)
            {
                if (rowtext.Trim() == "")
                    rowtext = "0";
                if (coltext.Trim() == "")
                    coltext = "0";
                int row11 = 0;
                int col11 = 0;
                int.TryParse(rowtext, out row11);
                int.TryParse(coltext, out col11);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (row11 <= 0)
                    {
                        //row11 = Convert.ToInt32(ds.Tables[0].Rows[0]["row"].ToString());
                        int.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["row"]).Trim(), out row11);
                    }
                    if (col11 <= 0)
                    {
                        //col11 = Convert.ToInt32(ds.Tables[0].Rows[0]["col"].ToString());
                        int.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["col"]).Trim(), out row11);
                    }
                }
                txtroomcolumn.Text = col11.ToString();
                txtroomrow.Text = row11.ToString();
                fproomarra.Sheets[0].RowCount = row11;
                fproomarra.Sheets[0].ColumnCount = col11 + 1;
                fproom.Sheets[0].RowCount = row11;
                fproom.Sheets[0].ColumnCount = col11 + 1;
                fproom.Sheets[0].ColumnHeader.Cells[0, 0].Text = " ";
                fproomarra.Sheets[0].ColumnHeader.Cells[0, 0].Text = " ";
                for (int mm = 0; mm < fproomarra.Sheets[0].RowCount; mm++)
                {
                    for (int nn = 1; nn < fproomarra.Sheets[0].ColumnCount; nn++)
                    {
                        fproomarra.Sheets[0].Cells[mm, nn].CellType = txt;
                        fproom.Sheets[0].Cells[mm, nn].CellType = txt;
                        fproomarra.Sheets[0].Cells[mm, nn].Text = string.Empty;
                        fproom.Sheets[0].Cells[mm, nn].Text = string.Empty;
                    }
                }
                fproom.Visible = true;
                columnsetting.Visible = true;
                fproom.CommandBar.Visible = false;
                fproom.SheetCorner.ColumnCount = 0;
                fproom.Sheets[0].ColumnHeader.RowCount = 1;
                fproom.Sheets[0].AutoPostBack = false;
                fproomarra.Visible = true;
                fproomarra.CommandBar.Visible = false;
                fproomarra.SheetCorner.ColumnCount = 0;
                fproom.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                fproomarra.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                fproomarra.Sheets[0].ColumnHeader.RowCount = 1;
                fproom.Sheets[0].Columns[0].Width = 40;
                fproom.Sheets[0].Columns[0].Locked = true;
                fproom.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                fproom.Sheets[0].ColumnHeader.Cells[0, 0].Text = ".";
                fproomarra.Sheets[0].Columns[0].Width = 40;
                fproomarra.Sheets[0].Columns[0].Locked = true;
                fproomarra.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                fproomarra.Sheets[0].ColumnHeader.Cells[0, 0].Text = ".";
                int c = 0;
                for (c = 1; c < fproomarra.Sheets[0].ColumnCount; c++)
                {
                    fproom.Sheets[0].ColumnHeader.Cells[0, c].Text = "C " + c + "";
                    fproom.Sheets[0].Columns[c].Width = 40;
                    fproom.Sheets[0].Columns[c].CellType = intgrcel;
                    fproom.Sheets[0].Columns[c].HorizontalAlign = HorizontalAlign.Right;
                    fproomarra.Sheets[0].ColumnHeader.Cells[0, c].Text = "C " + c + "";
                    fproomarra.Sheets[0].Columns[c].Width = 80;
                    fproomarra.Sheets[0].Columns[c].CellType = intgrcel;
                    fproomarra.Sheets[0].Columns[c].Locked = false;
                    fproomarra.Sheets[0].Columns[c].HorizontalAlign = HorizontalAlign.Right;
                }
                for (c = 0; c < fproomarra.Sheets[0].RowCount; c++)
                {
                    fproom.Sheets[0].Cells[c, 0].Text = "R " + (c + 1).ToString();
                    fproom.Sheets[0].Cells[c, 0].Locked = true;
                    fproomarra.Sheets[0].Cells[c, 0].Text = "R " + (c + 1).ToString();
                    fproomarra.Sheets[0].Cells[c, 0].Locked = true;
                }
                fproomarra.SaveChanges();
                fproom.SaveChanges();
                fproomarra.SaveChanges();
                fproom.SaveChanges();
                lblvalallot.Text = Convert.ToString(count_allot);
                lblvaldef.Text = Convert.ToString(count_actual);
                fproom.Height = 260;
                // fproom.SaveChanges();
                fproom.Sheets[0].PageSize = fproom.Sheets[0].RowCount;
                fproomarra.Height = 260;
                // fproomarra.SaveChanges();
                fproomarra.Sheets[0].PageSize = fproomarra.Sheets[0].RowCount;
                return;
            }
            if (rowtext.Trim() == "")
                rowtext = "0";
            if (coltext.Trim() == "")
                coltext = "0";
            int row = 0; // Convert.ToInt32(rowtext);
            int col = 0;// Convert.ToInt32(coltext);
            int.TryParse(rowtext, out row);
            int.TryParse(coltext, out col);
            if (newroomseats == true)
            {
                //ddr = Convert.ToInt32(txtroomrow.Text);
                //ddc = Convert.ToInt32(txtroomcolumn.Text);
                int.TryParse(Convert.ToString(txtroomrow.Text).Trim(), out ddr);
                int.TryParse(Convert.ToString(txtroomcolumn.Text).Trim(), out ddc);
                newroomseats = false;
                col_row_alter = true;
            }
            else
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (row <= 0)
                    {
                        //row = Convert.ToInt32(ds.Tables[0].Rows[0]["row"].ToString());
                        int.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["row"]).Trim(), out row);
                    }
                    if (col <= 0)
                    {
                        //col = Convert.ToInt32(ds.Tables[0].Rows[0]["col"].ToString());
                        int.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["col"]).Trim(), out col);
                    }
                }
                if (ds.Tables[1].Rows.Count == 1)
                {
                    int checkrow = 0;// Convert.ToInt32(ds.Tables[1].Rows[0][5]);
                    int checkcol = 0;//Convert.ToInt32(ds.Tables[1].Rows[0][4]);

                    int.TryParse(Convert.ToString(ds.Tables[1].Rows[0]["no_of_rows"]).Trim(), out checkrow);
                    int.TryParse(Convert.ToString(ds.Tables[1].Rows[0]["no_of_columns"]).Trim(), out checkcol);
                    row = checkrow;
                    col = checkcol;
                }
            }
            if (col_row_alter == true)
            {
                //row = Convert.ToInt32(rowtext);
                // = Convert.ToInt32(coltext);
                int.TryParse(rowtext, out row);
                int.TryParse(coltext, out col);
            }
            if (row > 0 && col > 0)
            {
                txtroomcolumn.Text = col.ToString();
                txtroomrow.Text = row.ToString();
                fproom.Visible = true;
                fproom.CommandBar.Visible = false;
                fproom.SheetCorner.ColumnCount = 0;
                fproom.Sheets[0].ColumnHeader.RowCount = 1;
                fproom.Sheets[0].AutoPostBack = false;
                fproom.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                fproomarra.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                fproomarra.Visible = true;
                fproomarra.CommandBar.Visible = false;
                fproomarra.SheetCorner.ColumnCount = 0;
                fproomarra.Sheets[0].ColumnHeader.RowCount = 1;
                fproom.Sheets[0].RowCount = row;
                fproom.Sheets[0].ColumnCount = col + 1;
                fproom.Sheets[0].Columns[0].Width = 40;
                fproom.Sheets[0].Columns[0].Locked = true;
                fproom.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                fproom.Sheets[0].ColumnHeader.Cells[0, 0].Text = ".";
                fproomarra.Sheets[0].RowCount = row;
                fproomarra.Sheets[0].ColumnCount = col + 1;
                fproomarra.Sheets[0].Columns[0].Width = 40;
                fproomarra.Sheets[0].Columns[0].Locked = true;
                fproomarra.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                fproomarra.Sheets[0].ColumnHeader.Cells[0, 0].Text = ".";
                int c = 0;
                for (c = 1; c < col + 1; c++)
                {
                    fproomarra.Sheets[0].Columns[c].CellType = txt;
                    fproom.Sheets[0].Columns[c].CellType = txt;
                    fproom.Sheets[0].ColumnHeader.Cells[0, c].Text = "C " + c + "";
                    fproom.Sheets[0].Columns[c].Width = 40;
                    fproom.Sheets[0].Columns[c].CellType = intgrcel;
                    fproom.Sheets[0].Columns[c].HorizontalAlign = HorizontalAlign.Right;
                    fproomarra.Sheets[0].ColumnHeader.Cells[0, c].Text = "C " + c + "";
                    fproomarra.Sheets[0].Columns[c].Width = 80;
                    fproomarra.Sheets[0].Columns[c].CellType = intgrcel;
                    fproomarra.Sheets[0].Columns[c].Locked = false;
                    fproomarra.Sheets[0].Columns[c].HorizontalAlign = HorizontalAlign.Right;
                }
                for (c = 0; c < row; c++)
                {
                    fproom.Sheets[0].Cells[c, 0].Text = "R " + (c + 1).ToString();
                    fproomarra.Sheets[0].Cells[c, 0].Text = "R " + (c + 1).ToString();
                }
                if (col_row_alter == false)
                {
                    if (ds.Tables[1].Rows.Count == 1)
                    {
                        string hasAlterDefinition = Convert.ToString(ds.Tables[1].Rows[0]["hasAlternate"]).Trim();
                        bool hasAlter = false;
                        if (hasAlterDefinition.Trim().ToLower() == "true" || hasAlterDefinition.Trim().ToLower() == "1")
                        {
                            hasAlter = true;
                        }
                        //bool.TryParse(hasAlterDefinition.Trim(), out hasAlter);
                        default_view = Convert.ToString(ds.Tables[1].Rows[0]["default_view"]).Trim();
                        arranged_view = Convert.ToString(ds.Tables[1].Rows[0]["arranged_view"]).Trim();
                        if (!isGeneral)
                        {
                            if (hasAlter)
                            {
                                string defaultViewNew = Convert.ToString(ds.Tables[1].Rows[0]["defaultViewNew"]).Trim();
                                string arrangedViewNew = Convert.ToString(ds.Tables[1].Rows[0]["arrangedViewNew"]).Trim();
                                if (!string.IsNullOrEmpty(defaultViewNew) && !string.IsNullOrEmpty(arrangedViewNew))
                                {
                                    default_view = defaultViewNew;
                                    arranged_view = arrangedViewNew;
                                }
                            }
                        }
                        string[] splitdefault = default_view.Split(';');
                        string[] splitarrange = arranged_view.Split(';');
                        if (splitarrange.GetUpperBound(0) > 0)
                        {
                            for (int j = 0; j <= splitarrange.GetUpperBound(0); j++)
                            {
                                string splitdata_arge = splitarrange[j].ToString();
                                string[] splitifn = splitdata_arge.Split('-');
                                if (splitifn.GetUpperBound(0) > 0)
                                {
                                    for (int jjj = 1; jjj < fproomarra.Sheets[0].ColumnCount; jjj++)
                                    {
                                        string onesdatas = string.Empty;
                                        int onescount = Convert.ToInt32(splitifn[jjj - 1].ToString());
                                        count_allot = count_allot + onescount;
                                        for (int ones = 0; ones < onescount; ones++)
                                        {
                                            if (onesdatas.Trim() == "")
                                            {
                                                onesdatas = "1";
                                            }
                                            else
                                            {
                                                onesdatas = onesdatas + "1";
                                            }
                                        }
                                        fproomarra.Sheets[0].Cells[j, jjj].Text = onesdatas;
                                    }
                                }
                            }
                        }
                        if (splitdefault.GetUpperBound(0) > 0)
                        {
                            for (int j = 0; j <= splitdefault.GetUpperBound(0); j++)
                            {
                                string splitdata_arge = splitdefault[j].ToString();
                                string[] splitifn = splitdata_arge.Split('-');
                                if (splitifn.GetUpperBound(0) > 0)
                                {
                                    for (int jjj = 1; jjj < fproomarra.Sheets[0].ColumnCount; jjj++)
                                    {
                                        string onesdatas = string.Empty;
                                        int onescount = Convert.ToInt32(splitifn[jjj - 1].ToString());
                                        count_actual = count_actual + onescount;
                                        if (Convert.ToString(onescount) == "0")
                                        {
                                            fproom.Sheets[0].Cells[j, jjj].Text = string.Empty;
                                        }
                                        else
                                        {
                                            fproom.Sheets[0].Cells[j, jjj].Text = Convert.ToString(onescount);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int mm = 0; mm < fproomarra.Sheets[0].RowCount; mm++)
                        {
                            for (int nn = 1; nn < fproomarra.Sheets[0].ColumnCount; nn++)
                            {
                                fproomarra.Sheets[0].Cells[mm, nn].Text = string.Empty;
                                fproom.Sheets[0].Cells[mm, nn].Text = string.Empty;
                            }
                        }
                    }
                }
            }
            else
            {
                fproom.Visible = false;
                fproomarra.Visible = false;
                fpspread.Visible = false;
            }
            fproomarra.SaveChanges();
            fproom.SaveChanges();
            lblvalallot.Text = Convert.ToString(count_allot);
            lblvaldef.Text = Convert.ToString(count_actual);
            fproom.Height = 260;
            // fproom.SaveChanges();
            fproom.Sheets[0].PageSize = fproom.Sheets[0].RowCount;
            fproomarra.Height = 260;
            // fproomarra.SaveChanges();
            fproomarra.Sheets[0].PageSize = fproomarra.Sheets[0].RowCount;
            columnsetting.Visible = true;
        }
        catch (Exception ex)
        {
            lblroomerror.Text = ex.ToString();
            lblroomerror.Visible = true;
        }
    }

    public void loadfpspread()
    {
        try
        {
            DataSet fp_ds = new DataSet();
            //mode = ddltype.SelectedItem.Text;
            string actul_seatscount = string.Empty;
            string allot_seatcount = string.Empty;
            string actualSeatsNew = string.Empty;
            string allotedSeatsNew = string.Empty;
            string strmode = string.Empty;
            bool isGeneral = true;
            if (ddlHallType.Items.Count > 0)
            {
                int index = ddlHallType.SelectedIndex;
                switch (index)
                {
                    case 0:
                        isGeneral = true;
                        break;
                    case 1:
                        isGeneral = false;
                        break;
                    default:
                        isGeneral = true;
                        break;
                }
            }
            if (ddltype.Enabled == true && ddltype.Items.Count > 0)
            {
                mode = ddltype.SelectedItem.Text;
                if (ddltype.SelectedItem.ToString().Trim() != "")
                {
                    strmode = " and mode='" + ddltype.SelectedItem.Text.ToString() + "'";
                }
            }
            string strsql = "select  block,rno,floorid,priority from class_master where coll_code='" + CollegeCode + "' " + strmode + " order by priority";
            fpspread.Sheets[0].RowCount = 0;
            fp_ds.Clear();
            fp_ds = da.select_method_wo_parameter(strsql, "Text");
            DataSet srids = new DataSet();
            fpspread.Sheets[0].Columns[6].Visible = !isGeneral;
            fpspread.Sheets[0].Columns[7].Visible = !isGeneral;
            if (fp_ds.Tables.Count > 0 && fp_ds.Tables[0].Rows.Count > 0)
            {
                int sno = 0;
                for (int i = 0; i < fp_ds.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    building_name = Convert.ToString(fp_ds.Tables[0].Rows[i]["block"]);
                    floor_name = Convert.ToString(fp_ds.Tables[0].Rows[i]["floorid"]);
                    hall_name = Convert.ToString(fp_ds.Tables[0].Rows[i]["rno"]);
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].Cells[i, 0].Text = Convert.ToString(sno);
                    fpspread.Sheets[0].Cells[i, 1].Text = Convert.ToString(fp_ds.Tables[0].Rows[i]["block"]);
                    fpspread.Sheets[0].Cells[i, 2].Text = Convert.ToString(fp_ds.Tables[0].Rows[i]["floorid"]);
                    fpspread.Sheets[0].Cells[i, 3].Text = Convert.ToString(fp_ds.Tables[0].Rows[i]["rno"]);
                    string strstring = "select actual_seats,allocted_seats,actualSeatsNew,allotedSeatsNew from tbl_room_seats where Building_Name='" + building_name + "' and Floor_Name='" + floor_name + "' and Hall_No='" + hall_name + "' and coll_code='" + CollegeCode + "' " + strmode + "";
                    srids.Clear();
                    srids = da.select_method_wo_parameter(strstring, "Text");
                    if (srids.Tables[0].Rows.Count == 1)
                    {
                        actul_seatscount = Convert.ToString(srids.Tables[0].Rows[0]["actual_seats"]).Trim();
                        allot_seatcount = Convert.ToString(srids.Tables[0].Rows[0]["allocted_seats"]).Trim();
                        actualSeatsNew = Convert.ToString(srids.Tables[0].Rows[0]["actualSeatsNew"]).Trim();
                        allotedSeatsNew = Convert.ToString(srids.Tables[0].Rows[0]["allotedSeatsNew"]).Trim();
                        fpspread.Sheets[0].Cells[i, 4].Text = actul_seatscount;
                        fpspread.Sheets[0].Cells[i, 5].Text = allot_seatcount;
                        fpspread.Sheets[0].Cells[i, 6].Text = actualSeatsNew;
                        fpspread.Sheets[0].Cells[i, 7].Text = allotedSeatsNew;
                        fpspread.Sheets[0].Rows[i].BackColor = Color.LightPink;

                    }
                    else
                    {
                        fpspread.Sheets[0].Rows[i].BackColor = Color.LightYellow;
                    }
                    fpspread.Visible = true;
                }
            }
            fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
        }
        catch
        {
        }
    }

    public void ddltypechange()
    {
        try
        {
            arrybluid.Clear();
            arryfloor.Clear();
            arryhallno.Clear();
            DataSet ds = new DataSet();
            string strsql = string.Empty;
            string collegecode = Session["collegecode"].ToString();
            string strmode = string.Empty;
            if (ddltype.Enabled == true && ddltype.Items.Count > 0)
            {
                if (ddltype.SelectedItem.ToString().Trim() != "")
                {
                    strmode = "and c.mode='" + ddltype.SelectedItem.Text.ToString() + "'";
                }
            }
            strsql = "select distinct b.Building_Name as BuildingName,c.priority from Building_Master B,Floor_Master F,Room_Detail R,class_master c  where c.rno= r.room_name " + strmode + "  and R.Floor_Name=F.Floor_Name and  F.Building_Name=B.Building_Name and c.block=b.Building_Name and c.block=b.Building_Name and c.floorid=f.Floor_Name and c.coll_code=b.College_Code and B.College_Code='" + collegecode + "' order by c.priority,BuildingName";
            ds = da.select_method_wo_parameter(strsql, "Text");
            int count = ds.Tables[0].Rows.Count;
            string SNo = string.Empty;
            ddlroom.Items.Clear();
            ddlbuilding.Items.Clear();
            ddlflooring.Items.Clear();
            txtroomcolumn.Text = string.Empty;
            txtroomrow.Text = string.Empty;
            if (count > 0)
            {
                int n = 0;
                ddlroom.Enabled = true;
                ddlbuilding.Enabled = true;
                ddlflooring.Enabled = true;
                txtroomcolumn.Enabled = true;
                txtroomrow.Enabled = true;
                btnroomgo.Enabled = true;
                fproom.Visible = true;
                fproomarra.Visible = true;
                lbldefault.Visible = true;
                lblarrange.Visible = true;
                btnpassval.Visible = true;
                btnsaveseats.Visible = true;
                lbltotaldef.Visible = true;
                lblvaldef.Visible = true;
                lbltotalarge.Visible = true;
                lblvalallot.Visible = true;
                Hashtable hatbuild = new Hashtable();
                for (int i = 0; i < count; i++)
                {
                    if (!hatbuild.ContainsKey(ds.Tables[0].Rows[i]["BuildingName"].ToString()))
                    {
                        hatbuild.Add(ds.Tables[0].Rows[i]["BuildingName"].ToString(), ds.Tables[0].Rows[i]["BuildingName"].ToString());
                        ddlbuilding.Items.Add(ds.Tables[0].Rows[i]["BuildingName"].ToString());
                    }
                }
                loadfloor();
                loadrome();
                loadseat();
            }
            else
            {
                ddlroom.Enabled = false;
                ddlbuilding.Enabled = false;
                ddlflooring.Enabled = false;
                txtroomcolumn.Enabled = false;
                txtroomrow.Enabled = false;
                btnroomgo.Enabled = false;
                fproom.Visible = false;
                fproomarra.Visible = false;
                lbldefault.Visible = false;
                lblarrange.Visible = false;
                btnpassval.Visible = false;
                btnsaveseats.Visible = false;
                lbltotaldef.Visible = false;
                lblvaldef.Visible = false;
                lbltotalarge.Visible = false;
                lblvalallot.Visible = false;
                fpspread.Visible = false;
            }
        }
        catch
        {
        }
    }

    public void loadfloor()
    {
        try
        {
            ddlflooring.Items.Clear();
            ddlflooring.Enabled = false;
            if (ddlbuilding.Enabled == true && ddlbuilding.Items.Count > 0)
            {
                string collegecode = Session["collegecode"].ToString();
                string strmode = string.Empty;
                if (ddltype.Enabled == true && ddltype.Items.Count > 0)
                {
                    if (ddltype.SelectedItem.ToString().Trim() != "")
                    {
                        strmode = "and c.mode='" + ddltype.SelectedItem.Text.ToString() + "'";
                    }
                }
                string strsql = "select distinct  b.Building_Name as BuildingName,F.Floor_Name as FloorNo from Building_Master B,Floor_Master F,Room_Detail R, class_master c where c.rno= r.room_name and R.Floor_Name=F.Floor_Name and c.block=b.Building_Name and c.floorid=f.Floor_Name and  F.Building_Name=B.Building_Name and c.coll_code=b.College_Code and B.College_Code='" + collegecode + "' and F.Building_Name='" + ddlbuilding.SelectedItem.ToString() + "'  order by BuildingName,FloorNo";
                DataSet ds = da.select_method_wo_parameter(strsql, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlflooring.Enabled = true;
                    ddlflooring.DataSource = ds;
                    ddlflooring.DataTextField = "FloorNo";
                    ddlflooring.DataBind();
                }
            }
        }
        catch
        {
        }
    }

    public void loadrome()
    {
        try
        {
            ddlroom.Items.Clear();
            ddlroom.Enabled = false;
            if (ddlbuilding.Enabled == true && ddlbuilding.Items.Count > 0 && ddlflooring.Enabled == true && ddlflooring.Items.Count > 0)
            {
                string collegecode = Session["collegecode"].ToString();
                string strmode = string.Empty;
                if (ddltype.Enabled == true && ddltype.Items.Count > 0)
                {
                    if (ddltype.SelectedItem.ToString().Trim() != "")
                    {
                        strmode = "and c.mode='" + ddltype.SelectedItem.Text.ToString() + "'";
                    }
                }
                string strsql = "select distinct  b.Building_Name as BuildingName,F.Floor_Name as FloorNo,R.Room_Name as HallNo from Building_Master B,Floor_Master F,Room_Detail R ,class_master c where c.rno= r.room_name and R.Floor_Name=F.Floor_Name and  F.Building_Name=B.Building_Name and c.block=b.Building_Name and c.floorid=f.Floor_Name and c.coll_code=b.College_Code and B.College_Code='" + collegecode + "' and F.Building_Name='" + ddlbuilding.SelectedItem.ToString() + "' and f.Floor_Name='" + ddlflooring.SelectedValue.ToString() + "' " + strmode + "  order by BuildingName,FloorNo,HallNo";
                DataSet ds = da.select_method_wo_parameter(strsql, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlroom.Enabled = true;
                    ddlroom.DataSource = ds;
                    ddlroom.DataTextField = "HallNo";
                    ddlroom.DataBind();
                }
            }
        }
        catch
        {
        }
    }

    protected void btnpassval_Click(object sender, EventArgs e)
    {
        try
        {
            fproomarra.SaveChanges();
            fproom.SaveChanges();
            int calcount = 0;
            for (int mm = 0; mm < fproomarra.Sheets[0].RowCount; mm++)
            {
                for (int nn = 1; nn < fproomarra.Sheets[0].ColumnCount; nn++)
                {
                    fproomarra.Sheets[0].Cells[mm, nn].Text = string.Empty;
                }
            }
            for (int mm = 0; mm < fproom.Sheets[0].RowCount; mm++)
            {
                for (int nn = 1; nn < fproom.Sheets[0].ColumnCount; nn++)
                {
                    if (fproom.Sheets[0].Cells[mm, nn].Text.Trim() != "" && fproom.Sheets[0].Cells[mm, nn].Text != null)
                    {
                        string setvalue = string.Empty;
                        int num = 0;
                        int countnum = Convert.ToInt32(fproom.Sheets[0].Cells[mm, nn].Text);
                        calcount = calcount + countnum;
                        for (int ii = 0; ii < countnum; ii++)
                        {
                            if (setvalue.Trim() == "")
                            {
                                setvalue = "1";
                            }
                            else
                            {
                                setvalue = setvalue + "1";
                            }
                        }
                        fproomarra.Sheets[0].Cells[mm, nn].Text = setvalue;
                    }
                    //fproomarra.Sheets[0].Cells[mm, nn].Text =string.Empty;
                }
            }
            lblvalallot.Text = Convert.ToString(calcount);
            lblvaldef.Text = Convert.ToString(calcount);
            fproomarra.SaveChanges();
            fproom.SaveChanges();
        }
        catch
        {
        }
    }

    protected void btnsaveseats_Click(object sender, EventArgs e)
    {
        try
        {
            lblcolerror.Text = string.Empty;
            int issingle = 0;
            int total_cells = 0;
            fproom.SaveChanges();
            fproomarra.SaveChanges();
            int no_of_rows = 0, no_of_columns = 0;
            mode = ddltype.SelectedItem.Text;
            building_name = ddlbuilding.SelectedItem.Text;
            floor_name = ddlflooring.SelectedItem.Text;
            hall_name = ddlroom.SelectedItem.Text;
            no_of_columns = Convert.ToInt32(txtroomcolumn.Text);
            no_of_rows = Convert.ToInt32(txtroomrow.Text);
            int act_seats_count = 0, allot_seats_count = 0;
            string defaultstrings = string.Empty;
            bool isGeneral = true;
          
            if (ddlHallType.Items.Count > 0)
            {
                int index = ddlHallType.SelectedIndex;
                switch (index)
                {
                    case 0:
                        isGeneral = true;
                        break;
                    case 1:
                        isGeneral = false;
                        break;
                    default:
                        isGeneral = true;
                        break;
                }
            }
            for (int mm = 0; mm < fproom.Sheets[0].RowCount; mm++)
            {
                for (int nn = 1; nn < fproom.Sheets[0].ColumnCount; nn++)
                {
                    if (fproom.Sheets[0].Cells[mm, nn].Text.Trim() == "")
                    {
                        defaultstrings = "0";
                    }
                    else
                    {
                        defaultstrings = fproom.Sheets[0].Cells[mm, nn].Text.Trim();
                    }
                    int length = default_view.Length;
                    string vxs = string.Empty;
                    if (length > 0)
                    {
                        vxs = default_view.Substring(default_view.Length - 1);
                    }
                    if (vxs == ";")
                    {
                        default_view = default_view + defaultstrings;
                    }
                    else if (default_view == "")
                    {
                        default_view = defaultstrings;
                    }
                    else
                    {
                        default_view = default_view + "-" + defaultstrings;
                    }
                    if (fproom.Sheets[0].Cells[mm, nn].Text.Trim() != "" && fproom.Sheets[0].Cells[mm, nn].Text != null)
                    {
                        int countnum = Convert.ToInt32(fproom.Sheets[0].Cells[mm, nn].Text);
                        act_seats_count = act_seats_count + countnum;
                    }
                }
                default_view = default_view + ";";
            }
            int onescount = 0;
            string onesstringvalue = string.Empty;
            total_cells = fproomarra.Sheets[0].RowCount * (fproomarra.Sheets[0].ColumnCount - 1);
            for (int mm = 0; mm < fproomarra.Sheets[0].RowCount; mm++)
            {
                for (int nn = 1; nn < fproomarra.Sheets[0].ColumnCount; nn++)
                {
                    string no_of_ones = fproomarra.Sheets[0].Cells[mm, nn].Text.Trim();
                    if (no_of_ones.Trim() == "1")
                    {
                        issingle++;
                    }
                    onescount = no_of_ones.Length;
                    onesstringvalue = Convert.ToString(no_of_ones.Length);
                    int length = arranged_view.Length;
                    string vxs = string.Empty;
                    if (length > 0)
                    {
                        vxs = arranged_view.Substring(arranged_view.Length - 1);
                    }
                    else
                    {
                        onesstringvalue = Convert.ToString(no_of_ones.Length);
                    }
                    if (vxs == ";")
                    {
                        arranged_view = arranged_view + onesstringvalue;
                    }
                    else if (arranged_view == "")
                    {
                        arranged_view = onesstringvalue;
                    }
                    else
                    {
                        arranged_view = arranged_view + "-" + onesstringvalue;
                    }
                    if (fproomarra.Sheets[0].Cells[mm, nn].Text.Trim() != "" && fproomarra.Sheets[0].Cells[mm, nn].Text != null)
                    {
                        int countnum = Convert.ToInt32(no_of_ones.Length);
                        allot_seats_count = allot_seats_count + countnum;
                    }
                }
                arranged_view = arranged_view + ";";
            }

            string maxroomCount = da.GetFunction("select isnull(students_allowed,0) from Room_Detail where Building_Name='" + building_name + "' and Floor_Name='" + floor_name + "' and Room_Name='"+hall_name+"'");
            if (issingle == total_cells)
            {
                issingle = 1;
            }
            else
            {
                issingle = 0;
            }
            if (allot_seats_count <= Convert.ToInt32(maxroomCount))
            {
                //string srisql = "delete from tbl_room_seats where mode='" + mode + "' and Building_Name='" + building_name + "' and Floor_Name='" + floor_name + "'and Hall_No='" + hall_name + "'";
                //int is_saved = da.update_method_wo_parameter(srisql, "Text");

                string qryGeneral = ", default_view='" + default_view + "' , arranged_view='" + arranged_view + "' , actual_seats=" + act_seats_count + " , allocted_seats=" + allot_seats_count;
                if (!isGeneral)
                    qryGeneral = ", defaultViewNew='" + default_view + "' , arrangedViewNew='" + arranged_view + "' , actualSeatsNew=" + act_seats_count + " , allotedSeatsNew=" + allot_seats_count;
                string srisql = "if not exists (select * from tbl_room_seats where mode='" + mode + "' and Building_Name='" + building_name + "' and Floor_Name='" + floor_name + "'and Hall_No='" + hall_name + "') begin insert into tbl_room_seats(mode,Building_Name,Floor_Name,Hall_No,no_of_columns,no_of_rows,default_view,arranged_view,actual_seats,allocted_seats,coll_code,is_single,defaultViewNew,arrangedViewNew,actualSeatsNew,allotedSeatsNew) values ('" + mode + "','" + building_name + "','" + floor_name + "','" + hall_name + "','" + no_of_columns + "','" + no_of_rows + "','" + default_view + "','" + arranged_view + "','" + act_seats_count + "','" + allot_seats_count + "','" + CollegeCode + "', '" + issingle + "','" + default_view + "','" + arranged_view + "','" + act_seats_count + "','" + allot_seats_count + "') end else begin update tbl_room_seats set  no_of_columns=" + no_of_columns + " , no_of_rows=" + no_of_rows + " , coll_code=" + CollegeCode + ",is_single=" + issingle + "" + qryGeneral + " where mode='" + mode + "' and Building_Name='" + building_name + "' and Floor_Name='" + floor_name + "'and Hall_No='" + hall_name + "' end";
                int is_saved = da.update_method_wo_parameter(srisql, "Text");

                if (is_saved == 1)
                {
                    loadfpspread();
                    lblAlertMsg.Text = "Saved Successfull";
                    divPopAlert.Visible = true;
                    lblvalallot.Text = Convert.ToString(allot_seats_count);
                    lblvaldef.Text = Convert.ToString(act_seats_count);
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "Change Student Max Count from Room Details";
                divPopAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblcolerror.Visible = true;
            lblcolerror.Text = ex.ToString();
        }
    }

    protected void btnset_Click(object sender, EventArgs e)
    {
        if (txtselcol.Text.Trim() == "")
        {
            lblcolerror.Text = "Please Enter The Column Position";
            return;
        }
        if (txtvalue.Text.Trim() == "")
        {
            lblcolerror.Text = "Please Enter The Value ";
            return;
        }
        if (txtselcol.Text.Trim() != "" && txtselcol.Text.Trim() != null)
        {
            int setcol = Convert.ToInt32(txtselcol.Text.Trim());
            int columcount = Convert.ToInt32(fproom.Sheets[0].ColumnCount);
            if (columcount > 1 && columcount > setcol && setcol >= 1)
            {
                for (int mm = 0; mm < fproom.Sheets[0].RowCount; mm++)
                {
                    fproom.Sheets[0].Cells[mm, setcol].Text = txtvalue.Text.Trim();
                }
                lblcolerror.Text = string.Empty;
            }
            else
            {
                lblcolerror.Text = "Please Enter Correct Value";
            }
        }
    }

    protected void btnroomgo_Click(object sender, EventArgs e)
    {
        try
        {
            newroomseats = true;
            loadseat();
            loadfpspread();
        }
        catch
        {
        }
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

}