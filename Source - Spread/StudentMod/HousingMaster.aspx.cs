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
using InsproDataAccess;
public partial class StudentMod_HousingMaster : System.Web.UI.Page
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
            rdb_genderOnchange(sender, e);

            if (!IsPostBack)
            {
                bindCollege();
                bind_ddlCollege();
                loadHousename();
                loadHousenameMaster();
                bindcollegename();
                loadHousenameLink();
                rdb_gendermale.Checked = true;
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.Visible = false;
                btn_go_Click(sender, e);
            }
            lblvalidation1.Visible = false;
        }
        catch (Exception ex) { }
    }
    public void bindCollege() //to bind college in main page multiple checkbox
    {
        try
        {
            cbl_College.Items.Clear();
            cb_College.Checked = true;
            txtCollege.Text = lblCollege.Text;
            ds.Clear();
            ds = d2.BindCollegebaseonrights(usercode, 1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_College.DataSource = ds;
                cbl_College.DataTextField = "collname";
                cbl_College.DataValueField = "college_code";
                cbl_College.DataBind();
                for (int i = 0; i < cbl_College.Items.Count; i++)
                {
                    cbl_College.Items[i].Selected = true;
                }
                txtCollege.Text = "College(" + cbl_College.Items.Count + ")";
            }
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
    }
    protected void ddl_college_OnIndexChange(object sender, EventArgs e)
    {
        //btn_go_Click(sender, e);
    }
    protected void cb_College_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void cbl_College_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        btn_delete.Visible = false;
        btn_save.Visible = true;
        divexit.Visible = true;
        bind_ddlCollege();
        loadHousename();
        loadHousenameMaster();
        txt_priority.Text = "";
    }
    private void bind_ddlCollege() //to bind college in popup(addnew button) dropdown
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollegebaseonrights(usercode, 1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = ds;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }
        }
        catch
        {
        }
    }
    protected void rdb_genderOnchange(object sender, EventArgs e)
    {
        orderbyTR.Visible = false;
        if (rdb_genderboth.Checked)
            orderbyTR.Visible = true;
    }
    public int getgender()
    {
        int gen = -1;
        if (rdb_genderboth.Checked == true)
        {
            gen = 2;
        }
        else if (rdb_genderfemale.Checked == true)
        {
            gen = 1;
        }
        else if (rdb_gendermale.Checked == true)
        {
            gen = 0;
        }
        Int16 gender = Convert.ToInt16(gen);
        return gender;
    }
    protected void bindcollegename()
    {
        try
        {
            ds.Clear();

            ddlcolhouse.Items.Clear();

            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {


                ddlcolhouse.DataSource = ds;
                ddlcolhouse.DataTextField = "collname";
                ddlcolhouse.DataValueField = "college_code";
                ddlcolhouse.DataBind();


            }
        }
        catch { }
    }
    protected void lb_housepr_click(object sender, EventArgs e)
    {
        pophouse.Visible = true;
        //ddlcolhouse.SelectedIndex = ddlcolhouse.Items.IndexOf(ddlcolhouse.Items.FindByValue(ddl_colhouse.SelectedItem.Value));
        // popcol = Convert.ToString(ddlcolhouse.SelectedItem.Value);

        Fpspreadpophouse.Visible = false;

        
        divhouse.Visible = false;
        btnsethousepriority.Visible = false;
        btnresethousepriority.Visible = false;
        btnexithouse.Visible = false;
    }
    protected void imghouse_Click(object sender, EventArgs e)
    {
        pophouse.Visible = false;
    }
    protected void ddlcolhouse_Change(object sender, EventArgs e)
    {
        popcol = Convert.ToString(ddlcolhouse.SelectedItem.Value);

        Fpspreadpophouse.Visible = false;
        divhouse.Visible = false;
        btnsethousepriority.Visible = false;
        btnresethousepriority.Visible = false;
        btnexithouse.Visible = false;
       

    }
    protected void cb_mainhouse_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cb_mainhouse, cbl_mainhouse, txtmainhouse, "House");
    }
    private void chkchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            if (chkchange.Checked == true)
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = true;
                }
                txtchange.Text = label + "(" + Convert.ToString(chklstchange.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = false;
                }

            }
        }
        catch { }
    }
    protected void cbl_mainhouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklstchange(cb_mainhouse, cbl_mainhouse, txtmainhouse, "House");
    }
    private void chklstchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {

            chkchange.Checked = false;
            int count = 0;
            for (int i = 0; i < chklstchange.Items.Count; i++)
            {
                if (chklstchange.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txtchange.Text = label + "(" + count + ")";
                if (count == chklstchange.Items.Count)
                {
                    chkchange.Checked = true;
                }
            }
        }
        catch { }
    }
   
    protected void btnpophousego_click(object sender, EventArgs e) //for link button
    {
        try
        {
            btnresethousepriority_click(sender, e);
            string housename = rs.GetSelectedItemsValueAsString(cbl_linkhousename);
            string collegecode = rs.GetSelectedItemsValueAsString(cbl_College);
            string housecode = "";
            if (txtlinkhousename.Text.Trim() != "--Select--")
            {
                if (cbl_linkhousename.Items.Count > 0)
                {
                    for (int cbd = 0; cbd < cbl_linkhousename.Items.Count; cbd++)
                    {
                        if (cbl_linkhousename.Items[cbd].Selected == true)
                        {
                            if (housecode.Trim() == "")
                            {
                                housecode = Convert.ToString(cbl_linkhousename.Items[cbd].Value);
                            }
                            else
                            {
                                housecode = housecode + "'" + "," + "'" + Convert.ToString(cbl_linkhousename.Items[cbd].Value);
                            }
                        }
                    }
                }
            }
            bool Newcheckflag = false;
            if (cb_linkhousename.Checked == true)
            {
                Newcheckflag = true;
            }
            string selectquery = "select CollegeCode,HouseName,case when Gender=0 then 'Male' when Gender =1 then 'Female' when Gender =2 then 'Both' end as Gender,HousePriority,HouseAcr,case when OrderBy=0 then 'Male' when OrderBy =1 then 'Female' when OrderBy =2 then 'Both' end as OrderBy,HousePK from HousingDetails where HousePK in('" + housename + "') and CollegeCode in('" + Convert.ToString(ddlcolhouse.SelectedItem.Value) + "')";
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
                    
                    Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 1].Text = "House Name";
                    Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspreadpophouse.Sheets[0].ColumnHeader.Columns[1].Locked = true;
                    Fpspreadpophouse.Columns[1].Width =200;

                    Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 2].Text = "House Acronym";
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

                    Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 4].Text = "HousePriority";
                    Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpspreadpophouse.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspreadpophouse.Sheets[0].ColumnHeader.Columns[4].Locked = true;
                    Fpspreadpophouse.Columns[4].Width =150;
                    
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        Fpspreadpophouse.Sheets[0].RowCount++;
                        Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";


                        Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["HouseName"]);
                        Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["HousePK"]);

                        Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["HouseAcr"]);

                        Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 3].CellType = cbhousepriority;
                        
                            if (Convert.ToString(ds.Tables[0].Rows[row]["HousePriority"]).Trim() != "")
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
                        Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["HousePriority"]);
                        Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspreadpophouse.Sheets[0].Cells[Fpspreadpophouse.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";


                    }
                    Fpspreadpophouse.Visible = true;
                    divhouse.Visible = true;
                    btnsethousepriority.Visible = true;
                    btnresethousepriority.Visible = true;
                    btnexithouse.Visible = true;
                    Fpspreadpophouse.Sheets[0].PageSize = Fpspreadpophouse.Sheets[0].RowCount;
                    
                        //Fpspreadpophouse.Sheets[0].Columns[0].Width = 50;
                        //Fpspreadpophouse.Sheets[0].Columns[0].Locked = true;
                        //Fpspreadpophouse.Sheets[0].Columns[1].Width = 100;
                        //Fpspreadpophouse.Sheets[0].Columns[1].Locked = true;
                        //Fpspreadpophouse.Sheets[0].Columns[2].Width = 190;
                        //Fpspreadpophouse.Sheets[0].Columns[2].Locked = true;
                        //Fpspreadpophouse.Sheets[0].Columns[3].Width = 70;
                        //Fpspreadpophouse.Sheets[0].Columns[4].Width = 95;
                        //Fpspreadpophouse.Sheets[0].Columns[4].Locked = true;
                        //Fpspreadpophouse.Height = 325;
                    


                    if (Newcheckflag == true)
                    {
                        DptPriorityDiv.Visible = true;
                    }
                    else
                    {
                        DptPriorityDiv.Visible = false;
                    }
                }
                else
                {
                    Fpspreadpophouse.Visible = false;
                    divhouse.Visible = false;
                }
            }
            else
            {
                Fpspreadpophouse.Visible = false;
                divhouse.Visible = false;
            }
            Fpspreadpophouse.SaveChanges();
        }
        catch { }
    }
    protected void Cellpophouse_Click(object sender, EventArgs e)
    {
        spreadhouseclick = true;
    }

    protected void Fpspreadhouse_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string value = Convert.ToString(Fpspread1.Sheets[0].Cells[0, 3].Value);
        if (value == "1")
        {
            for (int K = 1; K < Fpspread1.Sheets[0].Rows.Count; K++)
            {
                Fpspread1.Sheets[0].Cells[K, 3].Value = 1;
            }
        }
        else
        {
            for (int K = 1; K < Fpspread1.Sheets[0].Rows.Count; K++)
            {
                Fpspread1.Sheets[0].Cells[K, 3].Value = 0;
            }
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
    protected void btnsethousepriority_click(object sender, EventArgs e)
    {
        try
        {
            
            string college = rs.GetSelectedItemsValueAsString(cbl_College);
            //alertpopwindow.Visible = true;
            pnl2.Visible = true;
            lbl_alert.Visible = true;
            int upcount = 0;
            bool entry_flag = false;
            if (Fpspreadpophouse.Sheets[0].Rows.Count > 0 )
            {
                for (int i = 0; i < Fpspreadpophouse.Sheets[0].Rows.Count; i++)
                {
                    string updquery = "";
                    

                    string housePK = Convert.ToString(Fpspreadpophouse.Sheets[0].Cells[i, 1].Tag);
                    string priority = Convert.ToString(Fpspreadpophouse.Sheets[0].Cells[i, 4].Text.Trim());
                    if (priority.Trim() != "" && priority.Trim() != "0")
                    {
                        entry_flag = true;
                        updquery = "update HousingDetails set HousePriority ='" + priority + "' where housePK='" + housePK + "' and CollegeCode ='" + Convert.ToString(ddlcolhouse.SelectedItem.Value) + "'";
                        int insQ = d2.update_method_wo_parameter(updquery, "Text");
                        if (insQ > 0)
                        {
                            upcount++;
                        }
                    }
                    else
                    {
                        lbl_alert.Text = "Priority Not Assigned";
                    }
                    if (upcount > 0)
                    {
                        lbl_alert.Text = "Priority Assigned";
                    }
                    else
                    {
                        lbl_alert.Text = "Priority Not Assigned";
                    }
                }
            }

            btn_go_Click(sender, e);
        }
        
        catch { }
    }
    protected void btnresethousepriority_click(object sender, EventArgs e)
    {
        try
        {
            
           
            if (Fpspreadpophouse.Sheets[0].Rows.Count > 0)
            {
                for (int i = 0; i < Fpspreadpophouse.Sheets[0].Rows.Count; i++)
                {
                    string housePK = Convert.ToString(Fpspreadpophouse.Sheets[0].Cells[i, 1].Tag);
                    Fpspreadpophouse.Sheets[0].Cells[i, 3].Locked = false;
                    Fpspreadpophouse.Sheets[0].Cells[i, 3].Value = 0;
                    Fpspreadpophouse.Sheets[0].Cells[i, 4].Text = "";
                    string house_Code = Convert.ToString(Fpspreadpophouse.Sheets[0].Cells[i, 1].Tag);

                    int insup = d2.update_method_wo_parameter("update HousingDetails set HousePriority = NULL where HousePK ='" + housePK + "' and CollegeCode ='" + Convert.ToString(ddlcolhouse.SelectedItem.Value) + "'", "Text");
                    

                }
            }
            Fpspreadpophouse.SaveChanges();
            btn_go_Click(sender, e);
        }
        catch { }
    }
    protected void btnexithouse_click(object sender, EventArgs e)
    {
        pophouse.Visible = false;
    }
    protected void btnexit1_Click(object sender, EventArgs e)
    {
        divexit.Visible = false;
    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        divexit.Visible = false;
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void loadHousename() // for bind ddl_housename1 in addnew button
    {
        ddl_housename1.Items.Clear();
        ds.Clear();
        if (!string.IsNullOrEmpty(collegecode1.Trim()))
        {
            string sql = " select mastercode,MasterValue from co_mastervalues where mastercriteria='StudentHousing' and collegecode='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_housename1.DataSource = ds;
                ddl_housename1.DataTextField = "MasterValue";
                ddl_housename1.DataValueField = "mastercode";
                ddl_housename1.DataBind();
                ddl_housename1.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddl_housename1.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
    }
    protected void loadHousenameLink() // for bind checkbox cbl_linkhousename in main or Link button
    {
        cbl_linkhousename.Items.Clear();
        cb_linkhousename.Checked = true;
        ds.Clear();
        if (!string.IsNullOrEmpty(collegecode1.Trim()))
        {
            string query = "select HouseName,HousePK from HousingDetails where collegecode='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(query, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_linkhousename.DataSource = ds;
                cbl_linkhousename.DataTextField = "HouseName";
                cbl_linkhousename.DataValueField = "HousePK";
                cbl_linkhousename.DataBind();
                for (int i = 0; i < cbl_linkhousename.Items.Count; i++)
                {
                    cbl_linkhousename.Items[i].Selected = true;
                }
                txtlinkhousename.Text = "House(" + cbl_linkhousename.Items.Count + ")";
            }
            else
            {

            }
        }
    }
    protected void loadHousenameMaster() //for bind checkbox cbl_mainhouse in main or master page
    {
        cbl_mainhouse.Items.Clear();
        cb_mainhouse.Checked = true;
        ds.Clear();
        if (!string.IsNullOrEmpty(collegecode1.Trim()))
        {
            string query = "select HouseName,HousePK from HousingDetails where collegecode='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(query, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_mainhouse.DataSource = ds;
                cbl_mainhouse.DataTextField = "HouseName";
                cbl_mainhouse.DataValueField = "HousePK";
                cbl_mainhouse.DataBind();
                for (int i = 0; i < cbl_mainhouse.Items.Count; i++)
                {
                    cbl_mainhouse.Items[i].Selected = true;
                }
                txtmainhouse.Text = "House(" + cbl_mainhouse.Items.Count + ")";
            }
            else
            {

            }
        }
    }
    protected void cb_linkhousename_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cb_linkhousename, cbl_linkhousename, txtlinkhousename, "House");
    }
    protected void cbl_linkhousename_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklstchange(cb_linkhousename, cbl_linkhousename, txtlinkhousename, "House"); 
    }
    protected void btn_plus_housename_Click(object sender, EventArgs e)
    {
        txt_housename2.Text = "";
        imgdiv3.Visible = true;
        panel_reason.Visible = true;
    }
    protected void btn_minus_housename_Click(object sender, EventArgs e)
    {
              
        if (ddl_housename1.SelectedIndex == -1)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No Items found";
        }
        else if (ddl_housename1.SelectedIndex == 0)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Select any item";
        }
        else if (ddl_housename1.SelectedIndex != 0 && btn_minus_housename.Text == "-")
        {
            surediv.Visible = true;
            lbl_sure.Text = "Do you want to Delete this Record?";                              
                          
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No items found";
        }
    }
    protected void btn_exit_housename_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = false;
        panel_reason.Visible = false;
    }
    protected void btn_add_housename_Click(object sender, EventArgs e)
    {
        try
        {

            string collegecode1 = ddlCollege.SelectedItem.Value.ToString();
            string texthousename = txt_housename2.Text.ToUpper();
            if (txt_housename2.Text != "")
            {
                string housename = d2.GetFunction("select MasterValue from co_mastervalues where mastercriteria='StudentHousing' and mastervalue='" + Convert.ToString(texthousename) + "' and collegecode='" + collegecode1 + "'");
                if (!string.IsNullOrEmpty(collegecode1.Trim()) && housename == "0")
                {

                    string sql = "  if exists( select MasterValue from co_mastervalues where mastercriteria='StudentHousing' and mastervalue='" + Convert.ToString(texthousename) + "' and collegecode='" + collegecode1 + "') update CO_MasterValues set MasterValue='" + Convert.ToString(texthousename) + "' where mastercriteria='StudentHousing' and collegecode='" + collegecode1 + "' else  insert into co_mastervalues(mastercriteria,MasterValue,collegecode) values('StudentHousing','" + Convert.ToString(texthousename) + "','" + collegecode1 + "')";
                    int insert = d2.update_method_wo_parameter(sql, "Text");
                    if (insert != 0)
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Saved Successfully";
                        txt_housename2.Text = "";
                        imgdiv3.Visible = false;
                        panel_reason.Visible = false;
                    }
                    loadHousename();
                    loadHousenameMaster();


                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "House Name already exists";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Enter the House Name";
            }
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
    }
    protected void btn_exit_Click(object sender, EventArgs e)
    {
        divexit.Visible = false;
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string housename = rs.GetSelectedItemsValueAsString(cbl_mainhouse);
            string collegecode = rs.GetSelectedItemsValueAsString(cbl_College);
            if (!string.IsNullOrEmpty(collegecode.Trim()) && !string.IsNullOrEmpty(housename.Trim()))
            {
                string selectquery = "select CollegeCode,HousePK,HouseName,case when Gender=0 then 'Male' when Gender =1 then 'Female' when Gender =2 then 'Both' end as Gender,GenderPriority,HousePriority,case when OrderBy=1 then 'Male' when OrderBy =2 then 'Female' when OrderBy =3 then 'Both' end as OrderBy from HousingDetails where HousePK in('" + housename + "') and CollegeCode in('" + collegecode + "')";
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
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "House Name";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[1].Width = 100;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Gender";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[2].Width = 100;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "GenderPriority";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[3].Width = 50;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Order By";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[4].Width = 50;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "House Priority";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[5].Width = 50;
                    FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        Fpspread1.Sheets[0].RowCount++;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["HouseName"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["HousePK"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Gender"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["GenderPriority"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].CellType = txtcell;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["OrderBy"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["HousePriority"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
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
                lbl_error.Text = "Please Select All Fields";
            }
        }
        catch
        {
        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        savedetails();
        loadHousenameMaster();
        btn_go_Click(sender, e);
    }
    protected void savedetails()
    {
        try
        {
            orderbyTR.Visible = false;
            string collegecode1 = ddlCollege.SelectedItem.Value.ToString();
            string housename = Convert.ToString(ddl_housename1.SelectedItem.Text);
            string gender = "";
            if (rdb_gendermale.Checked == true)
            {
                gender = "0";
            }
            else if (rdb_genderfemale.Checked == true)
            {
                gender = "1";
            }
            else if (rdb_genderboth.Checked == true)
            {
                gender = "2";
            }

            string priority = Convert.ToString(txt_priority.Text);
            string orderby = "0";
            if (rdb_genderboth.Checked == true)
            {
                if (rdb_orderbymale1.Checked == true)
                {
                    orderby = "1";
                }
                else if (rdb_orderbyfemale1.Checked == true)
                {
                    orderby = "2";
                }
                else if (rdb_orderbyboth1.Checked == true)
                {
                    orderby = "3";
                }
            }
            if (housename != "" && housename != "select")
            {
                string query = "if exists (select * from HousingDetails where  HouseName ='" + housename + "'and CollegeCode='" + collegecode1 + "')update HousingDetails set HouseAcr ='" + txthouseacronym.Text + "',Gender ='" + gender + "',GenderPriority ='" + priority + "',OrderBy ='" + orderby + "' where  HouseName ='" + housename + "' and CollegeCode='" + collegecode1 + "' else insert into HousingDetails(HouseName,HouseAcr,Gender,GenderPriority,OrderBy,CollegeCode,UserCode) values ('" + housename + "','" + txthouseacronym.Text + "','" + gender + "','" + priority + "','" + orderby + "','" + collegecode1 + "','" + usercode + "')";
                int iv = d2.update_method_wo_parameter(query, "Text");
                if (iv != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Saved Successfully";
                    divexit.Visible = false;
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Updated Successfully";
                    divexit.Visible = false;
                }
            }
            
            else 
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Select any item";
            }
            //else if (ddl_housename1.SelectedIndex == -1)
            //{
            //    imgdiv2.Visible = true;
            //    lbl_alert.Text = "No Items found";
            //}
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
                divexit.Visible = true;
                btn_delete.Visible = true;
                btn_save.Visible = false;
                string activerow = "";
                string activecol = "";
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                if (activerow.Trim() != "")
                {
                    string itemname = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    string Gender = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                    string priority = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                    string ledger = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                    ddl_housename1.SelectedItem.Text = Convert.ToString(itemname);
                    txt_priority.Text = Convert.ToString(priority);
                    //ddl_ledger.SelectedItem.Text = Convert.ToString(ledger);
                    if (Gender == "Male")
                    {
                        rdb_gendermale.Checked = true;
                        rdb_genderfemale.Checked = false;
                        rdb_genderboth.Checked = false;
                    }
                    else if (Gender == "Female")
                    {
                        rdb_gendermale.Checked = false;
                        rdb_genderfemale.Checked = true;
                        rdb_genderboth.Checked = false;
                    }
                    else if (Gender == "Both")
                    {
                        rdb_gendermale.Checked = false;
                        rdb_genderfemale.Checked = false;
                        rdb_genderboth.Checked = true;
                    }
                    loadHousename();
                    loadHousenameMaster();
                    rdb_genderOnchange(sender, e);
                    ddl_housename1.SelectedIndex = ddl_housename1.Items.IndexOf(ddl_housename1.Items.FindByText(itemname));
                }
            }
        }
        catch
        {
        }
    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        Delete(sender, e);
        loadHousename();
        loadHousenameMaster();
        
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        imgdiv2.Visible = false;
    }
    protected void Delete(object sender,EventArgs e)
    {
        try
        {
            string collegecode1 = ddlCollege.SelectedItem.Value.ToString();
            string housename = Convert.ToString(ddl_housename1.SelectedItem.Text);
            string query2 = "delete from HousingDetails where HouseName ='" + housename + "'";
            query2 += " delete CO_MasterValues where MasterCriteria='studenthousing' and MasterCode='" + ddl_housename1.SelectedItem.Value + "' and CollegeCode='" + collegecode1 + "'";
            int iv = d2.update_method_wo_parameter(query2, "Text");
            if (iv != 0)
            {
                imgdiv2.Visible = true;
                btn_go_Click(sender, e);
                loadHousenameMaster();
                loadHousename();
                lbl_alert.Text = "Deleted Successfully";
                divexit.Visible = false;
            }
        }
        catch
        {
        }
    }
    protected void btn_delete_Click(object sender, EventArgs e)
    {
        if (btn_delete.Text == "Delete")
        {
            surediv.Visible = true;
            lbl_sure.Text = "Do you want to Delete this Record?";  
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
            string degreedetails = "House Master";
            string pagename = "HousingMaster.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }
}