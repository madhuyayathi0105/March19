using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using InsproDataAccess;
using System.Configuration;

public partial class StudentMod_DegreePriority : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    InsproDirectAccess DA = new InsproDirectAccess();
    Hashtable hat = new Hashtable();
    DAccess2 oda = new DAccess2();
    bool check = false;
    string popcol = "";
    bool spreadhouseclick = false;
    bool flaghouse = false;
    ReuasableMethods rs = new ReuasableMethods();
        
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("StudentHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/StudentMod/StudentHome.aspx");
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
                bindCollege();
                binddept();
            }
        }
        catch (Exception ex) { }
    }
    
    protected void cb_degree_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_degree, cb_degree, txtDegree, "Degree");
    }
    
    protected void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_degree, cb_degree, txtDegree, "Degree");
    }
    
    private void bindCollege() //to bind college in popup(addnew button) dropdown
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollegebaseonrights(usercode, 1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcolhouse.DataSource = ds;
                ddlcolhouse.DataTextField = "collname";
                ddlcolhouse.DataValueField = "college_code";
                ddlcolhouse.DataBind();
            }
        }
        catch
        {
        }
    }
    
    public void binddept()
    {
        try
        {
            ds.Clear();
            cbl_degree.Items.Clear();
            string item = "select Degree_Code,dt.Dept_Name from Degree d,Department dt,course c where d.Dept_Code=dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code='" + Convert.ToString(ddlcolhouse.SelectedItem.Value) + "' and c.type in('DAY','Evening')   order by ISNULL(c.Priority,0)";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "Dept_Name";
                cbl_degree.DataValueField = "Degree_Code";
                cbl_degree.DataBind();
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        cbl_degree.Items[i].Selected = true;
                    }
                    txtDegree.Text = "Degree (" + cbl_degree.Items.Count + ")";
                    cb_degree.Checked = true;
                }
            }
            else
            {
                txtDegree.Text = "--Select--";
                cb_degree.Checked = false;
            }
        }
        catch { }
    }
    
    protected void ddlcolhouse_Change(object sender, EventArgs e)
    {
        binddept();
    }
    
    protected void btnsetpriority_click(object sender, EventArgs e)
    {
        if (Fpspreadpophouse.Sheets[0].Rows.Count > 0)
        {
            string collegecode = Convert.ToString(ddlcolhouse.SelectedItem.Value);
            int insup = 0;
            for (int i = 0; i < Fpspreadpophouse.Sheets[0].Rows.Count; i++)
            {
                string Degreecode = Convert.ToString(Fpspreadpophouse.Sheets[0].Cells[i, 1].Tag);
                string priority = Convert.ToString(Fpspreadpophouse.Sheets[0].Cells[i, 4].Text.Trim());
                Fpspreadpophouse.Sheets[0].Cells[i, 3].Locked = false;
                Fpspreadpophouse.Sheets[0].Cells[i, 3].Value = 0;
                insup = d2.update_method_wo_parameter("update Degree set Dept_Priority='" + priority + "' where Degree_Code='" + Degreecode + "' and college_code='" + collegecode + "'", "Text");
            }
            btngo_click(sender, e);
            if (insup != 0)
            {
                alertpop.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Priority Saved Successfully";
            }
        }
    }
    
    protected void btnresetpriority_click(object sender, EventArgs e)
    {
        if (Fpspreadpophouse.Sheets[0].Rows.Count > 0)
        {
            string collegecode = Convert.ToString(ddlcolhouse.SelectedItem.Value);
            int insup = 0;
            for (int i = 0; i < Fpspreadpophouse.Sheets[0].Rows.Count; i++)
            {
                string Degreecode = Convert.ToString(Fpspreadpophouse.Sheets[0].Cells[i, 1].Tag);
                Fpspreadpophouse.Sheets[0].Cells[i, 3].Locked = false;
                Fpspreadpophouse.Sheets[0].Cells[i, 3].Value = 0;
                Fpspreadpophouse.Sheets[0].Cells[i, 4].Text = "";
                insup = d2.update_method_wo_parameter("update Degree set Dept_Priority=NULL where Degree_Code='" + Degreecode + "' and college_code='" + collegecode + "'", "Text");
            }
            Fpspreadpophouse.SaveChanges();
            btngo_click(sender, e);
            if (insup != 0)
            {
                alertpop.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Priority Reset Successfully";
            }
        }
    }
    
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertpop.Visible = false;
    }
    
    protected void btngo_click(object sender, EventArgs e)
    {
        try
        {
            // btnresetpriority_click(sender, e);
            string degreecode = rs.GetSelectedItemsValueAsString(cbl_degree);
            string collegecode = Convert.ToString(ddlcolhouse.SelectedItem.Value);
            if (!string.IsNullOrEmpty(degreecode) && ddlcolhouse.Items.Count > 0)
            {
                string selectquery = "select Degree_Code,isnull(c.type,'')+'-'+c.Course_Name+'-'+dt.Dept_Name as Dept_Name,Dept_Priority,Acronym from Degree d,Department dt,course c where d.Dept_Code=dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code in ('" + collegecode + "') and d.Degree_Code in('" + degreecode + "') order by ISNULL(c.Priority,0)";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Fpspreadpophouse.Sheets[0].RowCount = 0;
                        Fpspreadpophouse.Sheets[0].ColumnCount = 0;
                        Fpspreadpophouse.CommandBar.Visible = false;
                        Fpspreadpophouse.Sheets[0].AutoPostBack = false;
                        Fpspreadpophouse.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspreadpophouse.Sheets[0].RowHeader.Visible = false;
                        Fpspreadpophouse.Sheets[0].ColumnCount = 5;

                        FarPoint.Web.Spread.CheckBoxCellType cbhousepriority = new FarPoint.Web.Spread.CheckBoxCellType();
                        cbhousepriority.AutoPostBack = true;


                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.Font.Name = "Book Antiqua";
                        darkstyle.Font.Size = FontUnit.Medium;
                        darkstyle.Font.Bold = true;
                        darkstyle.Border.BorderSize = 1;
                        darkstyle.HorizontalAlign = HorizontalAlign.Center;
                        darkstyle.VerticalAlign = VerticalAlign.Middle;
                        darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
                        Fpspreadpophouse.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Columns[0].Locked = true;
                        Fpspreadpophouse.Columns[0].Width = 50;

                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Degree";
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Columns[1].Locked = true;
                        Fpspreadpophouse.Columns[1].Width = 400;

                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree Acronym";
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Columns[2].Locked = true;
                        Fpspreadpophouse.Columns[2].Width = 150;

                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Set Priority";
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Columns[3].Locked = false;

                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Degree Priority";
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpophouse.Sheets[0].ColumnHeader.Columns[4].Locked = true;
                        Fpspreadpophouse.Columns[4].Width = 85;

                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            Fpspreadpophouse.Sheets[0].RowCount++;
                            Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                            Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["Dept_Name"]);
                            Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Degree_Code"]);
                            Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Acronym"]);
                            Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 3].CellType = cbhousepriority;
                            if (Convert.ToString(ds.Tables[0].Rows[row]["Dept_Priority"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[row]["Dept_Priority"]).Trim() != "0")
                            {
                                Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 3].Value = 1;
                                Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 3].Locked = true;
                            }
                            else
                            {
                                Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 3].Value = 0;
                                Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 3].Locked = false;
                            }
                            Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 4].Text = (Convert.ToString(ds.Tables[0].Rows[row]["Dept_Priority"]) == "0") ? "" : Convert.ToString(ds.Tables[0].Rows[row]["Dept_Priority"]);
                            Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        }
                        spreadDiv.Visible = true;
                        Fpspreadpophouse.Sheets[0].PageSize = Fpspreadpophouse.Sheets[0].RowCount;
                    }
                    else
                    {
                        spreadDiv.Visible = false;
                    }
                }
                else
                {
                    spreadDiv.Visible = false;
                }
                Fpspreadpophouse.SaveChanges();
            }
            else
            {
                spreadDiv.Visible = false;
                alertpop.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Please Select All Fields";
            }
        }
        catch { }
    }
    
    protected void Cellpophouse_Click(object sender, EventArgs e)
    {
        spreadhouseclick = true;
    }
    
    protected void Fpspreadpophouse_render(object sender, EventArgs e)
    {
        if (flaghouse == true)
        {
            Fpspreadpophouse.SaveChanges();
            string activrow = "";
            activrow = Fpspreadpophouse.Sheets[0].ActiveRow.ToString();
            string activecol = Fpspreadpophouse.Sheets[0].ActiveColumn.ToString();
            int actcol = Convert.ToInt16(activecol);
            int hy_order = 0;
            for (int i = 0; i <= Convert.ToInt16(Fpspreadpophouse.Sheets[0].RowCount) - 1; i++)
            {
                int isval = Convert.ToInt32(Fpspreadpophouse.Sheets[0].Cells[i, actcol].Value);
                if (isval == 1)
                {
                    hy_order++;
                    Fpspreadpophouse.Sheets[0].Cells[Convert.ToInt32(activrow), actcol].Locked = true;
                }
            }
            Fpspreadpophouse.Sheets[0].Cells[Convert.ToInt32(activrow), actcol + 1].Text = hy_order.ToString();
        }
    }
    
    protected void Fpspreadpophouse_buttoncommand(object sender, EventArgs e)
    {
        try
        {
            Fpspreadpophouse.SaveChanges();
            string activerow = Fpspreadpophouse.ActiveSheetView.ActiveRow.ToString();
            string activecol = Fpspreadpophouse.ActiveSheetView.ActiveColumn.ToString();
            if (activecol == "3")
            {
                int act1 = Convert.ToInt32(activerow);
                int act2 = Convert.ToInt16(activecol);
                if (Fpspreadpophouse.Sheets[0].Cells[act1, act2].Value.ToString() == "1")
                {
                    flaghouse = true;
                    Fpspreadpophouse.Sheets[0].Cells[act1, act2 + 1].Text = "";
                }
                else
                {
                    flaghouse = false;
                }
            }
            if (activecol == "5")
            {
                int act1 = Convert.ToInt32(activerow);
                int act2 = Convert.ToInt16(activecol);
                if (Fpspreadpophouse.Sheets[0].Cells[act1, act2].Value.ToString() == "1")
                {
                    flaghouse = true;
                    Fpspreadpophouse.Sheets[0].Cells[act1, act2 + 1].Text = "";
                }
                else
                {
                    flaghouse = false;
                }
            }
            Fpspreadpophouse.SaveChanges();
        }
        catch { }
    }
}