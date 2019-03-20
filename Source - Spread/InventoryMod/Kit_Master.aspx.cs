using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class InventoryMod_Kit_Master : System.Web.UI.Page
{
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string sessstream = string.Empty;
    string selectQuery = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    ReuasableMethods rs = new ReuasableMethods();
    private string usercode;
    bool check = false;
    string collcode = string.Empty;
    string kitcode = string.Empty;
    string itemheadercode = string.Empty;
    string itemsubcode = string.Empty;
    string itname = "";
    string itcode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ////****************************************************//
            //string PageLogOut = "";
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("invM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["Inventory"].Trim();
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
            ////****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("inventoryindex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/InventoryMod/inventoryindex.aspx");
                    return;
                }
            }
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                bindcollege();
                loadheadername();
                loadsubheadername();
                bindKit();
            }
        }
        catch (Exception ex) { }
    }


    #region College
    public void bindcollege()
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }

        }
        catch
        {
        }
    }
    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadheadername();
            loadsubheadername();
            bindKit();
        }
        catch
        {

        }

    }
    #endregion

    #region ItemHeader
    protected void cb_headername_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_headername.Checked == true)
            {
                for (int i = 0; i < cbl_headername.Items.Count; i++)
                {
                    cbl_headername.Items[i].Selected = true;
                }
                txt_headername.Text = "Header Name(" + (cbl_headername.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_headername.Items.Count; i++)
                {
                    cbl_headername.Items[i].Selected = false;
                }
                txt_headername.Text = "--Select--";
            }
            loadsubheadername();
            
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_headername_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_headername.Text = "--Select--";
            cb_headername.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_headername.Items.Count; i++)
            {
                if (cbl_headername.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_headername.Text = "Header Name(" + commcount.ToString() + ")";
                if (commcount == cbl_headername.Items.Count)
                {
                    cb_headername.Checked = true;
                }
            }
            loadsubheadername();
           
        }
        catch (Exception ex)
        {
        }
    }
    public void loadheadername()
    {
        try
        {
            cbl_headername.Items.Clear();
            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and usercode='" + Session["usercode"] + "'";
            }
            string maninvalue = "";
            string selectnewquery = d2.GetFunction("select value  from Master_Settings where settings='ItemHeaderRights' " + columnfield + "");
            if (selectnewquery.Trim() != "" && selectnewquery.Trim() != "0")
            {
                string[] splitnew = selectnewquery.Split(',');
                if (splitnew.Length > 0)
                {
                    for (int row = 0; row <= splitnew.GetUpperBound(0); row++)
                    {
                        if (maninvalue == "")
                        {
                            maninvalue = Convert.ToString(splitnew[row]);
                        }
                        else
                        {
                            maninvalue = maninvalue + "'" + "," + "'" + Convert.ToString(splitnew[row]);
                        }
                    }
                }
            }
            ds.Clear();
            ds = d2.BindItemHeaderWithRights_inv();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_headername.DataSource = ds;
                cbl_headername.DataTextField = "ItemHeaderName";
                cbl_headername.DataValueField = "ItemHeaderCode";
                cbl_headername.DataBind();
              
                if (cbl_headername.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_headername.Items.Count; i++)
                    {
                        cbl_headername.Items[i].Selected = true;
                    }
                    txt_headername.Text = "Header Name(" + cbl_headername.Items.Count + ")";
                }
            }
            else
            {
                
                txt_headername.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    #endregion

    #region ItemSubHeader
    protected void cb_subheadername_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_subheadername.Checked == true)
            {
                for (int i = 0; i < cbl_subheadername.Items.Count; i++)
                {
                    cbl_subheadername.Items[i].Selected = true;
                }
                txt_subheadername.Text = "Sub Header Name(" + (cbl_subheadername.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_subheadername.Items.Count; i++)
                {
                    cbl_subheadername.Items[i].Selected = false;
                }
                txt_subheadername.Text = "--Select--";
            }
           
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_subheadername_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_subheadername.Text = "--Select--";
            cb_subheadername.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_subheadername.Items.Count; i++)
            {
                if (cbl_subheadername.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_subheadername.Text = "Sub Header Name(" + commcount.ToString() + ")";
                if (commcount == cbl_subheadername.Items.Count)
                {
                    cb_subheadername.Checked = true;
                }
            }
           
        }
        catch (Exception ex)
        {
        }
    }
    public void loadsubheadername()
    {
        try
        {
            cbl_subheadername.Items.Clear();
            string itemheader = "";
            if (ddl_collegename.Items.Count > 0)
                collcode = Convert.ToString(ddl_collegename.SelectedValue);
            for (int i = 0; i < cbl_headername.Items.Count; i++)
            {
                if (cbl_headername.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_headername.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_headername.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemheader.Trim() != "")
            {
                string query = "";
                query = "select distinct MasterCode,MasterValue from CO_MasterValues m,IM_ItemMaster i where m.MasterCode=i.subheader_code and itemheadercode in ('" + itemheader + "') and collegecode in ('" + collcode + "')";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_subheadername.DataSource = ds;
                    cbl_subheadername.DataTextField = "MasterValue";
                    cbl_subheadername.DataValueField = "MasterCode";
                    cbl_subheadername.DataBind();
                    if (cbl_subheadername.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_subheadername.Items.Count; i++)
                        {
                            cbl_subheadername.Items[i].Selected = true;
                        }
                        txt_subheadername.Text = "Sub Header Name(" + cbl_subheadername.Items.Count + ")";
                    }
                    if (cbl_subheadername.Items.Count > 5)
                    {
                        Panel2.Width = 300;
                        Panel2.Height = 300;
                    }
                }
                else
                {
                    txt_subheadername.Text = "--Select--";
                }
            }
            else
            {
                txt_subheadername.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    #endregion

    #region Go
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsitem = new DataSet();
            dsitem = GetItemdetails();
            if (dsitem.Tables.Count > 0 && dsitem.Tables[0].Rows.Count > 0)
            {
                loadspread(dsitem);
            }
            else
            {
                alertimg.Visible = true;
                lbl_alert.Text = "No Records Found";

            }
        }
        catch
        {

        }

    }
    #endregion

    #region PlusMinusKit
    protected void btnplus_Click(object sender, EventArgs e)
    {
        try
        {
            plusdiv.Visible = true;
            panel_addgroup.Visible = true;
            lbl_addgroup.Text = "Kit Name";
            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;
        }
        catch
        {

        }
    }

    protected void btnminus_Click(object sender, EventArgs e)
    {
        try
        {

            if (ddl_Kitname.SelectedIndex != 0)
            {
                string sql = "delete from CO_MasterValues where MasterCode='" + ddl_Kitname.SelectedItem.Value.ToString() + "' and MasterCriteria='Kit' and collegecode='" + ddl_collegename.SelectedValue + "'";
                int delete = d2.update_method_wo_parameter(sql, "Text");
                if (delete != 0)
                {
                    alertimg.Visible = true;
                    lbl_alert.Text = "Deleted Successfully";
                }
                else
                {
                    alertimg.Visible = true;
                    lbl_alert.Text = "No Record Selected";
                }
                bindKit();
            }
            else
            {
                alertimg.Visible = true;
                lbl_alert.Text = "No Record Selected";
            }
        }
        catch
        {


        }
    }

    protected void ddl_Kitname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            
            btn_go_Click(sender, e);
        }
        catch 
        {
        }
    
    
    }

    #endregion

    #region Add_And_Delete_Kit

    protected void btn_addgroup_Click(object sender, EventArgs e)
    {
        try
        {
            string group = Convert.ToString(txt_addgroup.Text);
            int insert = 0;
            if (txt_addgroup.Text != "")
            {
                string sqladd = "if exists ( select * from CO_MasterValues where MasterValue='" + group + "' and MasterCriteria='kit' and CollegeCode='" + collegecode + "') Update CO_MasterValues set MasterValue='" + group + "' where MasterValue='" + group + "' and MasterCriteria='kit' and CollegeCode='" + collegecode + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values('" + group + "','kit','" + collegecode + "')";
                insert = d2.update_method_wo_parameter(sqladd, "Text");
                if (insert != 0)
                {
                    alertimg.Visible = true;
                    lbl_alert.Text = "Saved Successfully";
                    bindKit();
                    txt_addgroup.Text = "";
                    plusdiv.Visible = false;
                    panel_addgroup.Visible = false;
                }

            }

            else
            {
                plusdiv.Visible = true;
                lblerror.Visible = true;
                lblerror.Text = "Enter the Kit Name";
            }
        }
        catch
        {

        }
    }
    protected void btn_exitaddgroup_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = false;
        panel_addgroup.Visible = false;
        txt_addgroup.Text = "";
    }

    protected void bindKit()
    {
        try
        {
            ddl_Kitname.Items.Clear();
            ds.Clear();
            string sql = "select distinct MasterCode,MasterValue  from CO_MasterValues where MasterCriteria ='kit' and CollegeCode ='" + ddl_collegename.SelectedValue + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_Kitname.DataSource = ds;
                ddl_Kitname.DataTextField = "MasterValue";
                ddl_Kitname.DataValueField = "MasterCode";
                ddl_Kitname.DataBind();
            }
            ddl_Kitname.Items.Insert(0, new ListItem(" ", "0"));
        }
        catch { }
    }

    #endregion

    #region Fspread
    public DataSet GetItemdetails()
    {

        DataSet dsloaddetails = new DataSet();
        try
        {
            #region get Value
            string selQ = string.Empty;
            if (ddl_collegename.Items.Count > 0)
                collcode = Convert.ToString(ddl_collegename.SelectedValue);
            if (cbl_headername.Items.Count > 0)
                itemheadercode = Convert.ToString(rs.getCblSelectedValue(cbl_headername));
            if (cbl_subheadername.Items.Count > 0)
                itemsubcode = Convert.ToString(rs.getCblSelectedValue(cbl_subheadername));
            if (ddl_Kitname.Items.Count > 0)
                kitcode = Convert.ToString(ddl_Kitname.SelectedValue);
            if (!string.IsNullOrEmpty(collcode))
            {
                selQ = "  select distinct i.ItemPK,i.ItemCode ,i.ItemName +'('+i.ItemUnit+')' as ItemName, sm.StorePK ,sm.StoreName,i.ItemHeaderName,t.MasterValue as ItemSubHeadername from IM_StoreMaster sm,IM_ItemMaster i,CO_MasterValues t  where sm.StorePK=i.StoreFK and t.CollegeCode=sm.CollegeCode and t.MasterCode=i.subheader_code and   sm.CollegeCode='" + collcode + "' and i.ItemHeaderCode  in ('" + itemheadercode + "') and t.MasterCode in('" + itemsubcode + "')  group by ItemPK,ItemCode,ItemName,StorePK,StoreName,i.ItemHeaderName,i.ItemUnit,t.MasterValue order by i.ItemPK ";
                selQ += "select distinct i.ItemPK,i.ItemCode ,i.ItemName +'('+i.ItemUnit+')' as ItemName, sm.StorePK ,sm.StoreName,i.ItemHeaderName,t.MasterValue as ItemSubHeadername from IM_StoreMaster sm,IM_ItemMaster i,CO_MasterValues t,IM_KitMaster km  where sm.StorePK=i.StoreFK and t.CollegeCode=sm.CollegeCode and t.MasterCode=i.subheader_code and t.CollegeCode=km.CollegeCode and i.ItemCode=km.ItemCode and km.KitCode='" + kitcode + "' and  km.CollegeCode='" + collcode + "'  group by ItemPK,i.ItemCode,i.ItemName,StorePK,StoreName,i.ItemHeaderName,i.ItemUnit,t.MasterValue order by i.ItemPK";
            }
            dsloaddetails.Clear();
            dsloaddetails = d2.select_method_wo_parameter(selQ, "Text");
            #endregion
        }
        catch
        {

        }
        return dsloaddetails;
    }

    public void loadspread(DataSet ds)
    {
        try
        {
            DataView dv = new DataView();
            spreadDet1.Sheets[0].RowCount = 0;
            spreadDet1.Sheets[0].ColumnCount = 6;
            spreadDet1.CommandBar.Visible = false;
            spreadDet1.Sheets[0].AutoPostBack = false;
            spreadDet1.Sheets[0].ColumnHeader.RowCount = 1;
            spreadDet1.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;

            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            spreadDet1.Sheets[0].Columns[0].Locked = true;
            spreadDet1.Columns[0].Width = 80;

            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Store Name";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet1.Sheets[0].Columns[1].Locked = true;
            spreadDet1.Columns[1].Width = 150;

            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Header Name";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            spreadDet1.Sheets[0].Columns[2].Locked = true;
            spreadDet1.Columns[2].Width = 200;

            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item SubHeader Name";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            spreadDet1.Sheets[0].Columns[3].Locked = true;
            spreadDet1.Columns[3].Width = 200;


            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Item Name";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            spreadDet1.Columns[4].Width = 200;

            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Select";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            spreadDet1.Columns[5].Width = 50;

            FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
            chkall.AutoPostBack = false;
            int sno = 0;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {

                    spreadDet1.Sheets[0].RowCount++;
                    sno++;
                    string strname = Convert.ToString(ds.Tables[0].Rows[row]["StoreName"]).Trim();
                    string headname = Convert.ToString(ds.Tables[0].Rows[row]["ItemHeaderName"]).Trim();
                    string subheadname = Convert.ToString(ds.Tables[0].Rows[row]["ItemSubHeadername"]).Trim();
                    string itemname = Convert.ToString(ds.Tables[0].Rows[row]["ItemName"]).Trim();
                    string itemcode = Convert.ToString(ds.Tables[0].Rows[row]["ItemCode"]).Trim();


                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].CellType = txtCell;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].CellType = txtCell;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 5].CellType = chkall;

                    ds.Tables[1].DefaultView.RowFilter = "ItemCode ='" + Convert.ToString(ds.Tables[0].Rows[row]["ItemCode"]) + "'";
                    dv = ds.Tables[1].DefaultView;
                    if (dv.Count > 0)
                    {
                        spreadDet1.Sheets[0].Cells[row, 5].Value = 1;
                    }
                    else
                    {
                        spreadDet1.Sheets[0].Cells[row, 5].Value = 0;
                    }
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].Text = strname;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].Text = headname;

                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].Text = subheadname;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].Tag = itemcode;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].Text = itemname;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].Tag = itemname;

                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;

                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;


                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].Locked = true;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].Locked = true;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].Locked = true;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].Locked = true;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].Locked = true;

                    //spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].ForeColor = Color.Blue;
                    //spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].Font.Underline = true;
                    //spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].ForeColor = Color.Blue;
                    //spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].Font.Underline = true;



                    spreadDet1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    spreadDet1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    spreadDet1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                }

                //spreadDet1.Sheets[0].Columns[0].Width = 50;
                //spreadDet1.Sheets[0].Columns[1].Width = 160;
                //spreadDet1.Sheets[0].Columns[2].Width = 100;
                //spreadDet1.Sheets[0].Columns[3].Width = 100;
                //spreadDet1.Sheets[0].Columns[4].Width = 200;
                spreadDet1.Sheets[0].PageSize = spreadDet1.Sheets[0].RowCount;
                spreadDet1.SaveChanges();

                ShowReport.Visible = true;
                spreadDet1.Visible = true;

            }
        }

        catch
        {

        }

    }
    #endregion

    #region Save
    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            int insertkit = 0;
            if (ddl_collegename.Items.Count > 0)
                collcode = Convert.ToString(ddl_collegename.SelectedValue);
            if (ddl_Kitname.Items.Count > 0)
                kitcode = Convert.ToString(ddl_Kitname.SelectedValue);
          
            if (kitcode != "0" && kitcode != " ")
            {
                if (spreadDet1.Sheets[0].RowCount > 0)
                {
                    spreadDet1.SaveChanges();
                    for (int row = 0; row < spreadDet1.Sheets[0].RowCount; row++)
                    {
                        int checkval = Convert.ToInt32(spreadDet1.Sheets[0].Cells[row, 5].Value);
                        if (checkval == 1)
                        {
                            check = true;
                            itcode = Convert.ToString(spreadDet1.Sheets[0].Cells[row, 3].Tag);
                            itname = Convert.ToString(spreadDet1.Sheets[0].Cells[row, 4].Tag);
                            string sqlins = "if exists (select * from IM_KitMaster where ItemCode='" + itcode + "' and KitCode='" + kitcode + "' and CollegeCode='" + collegecode + "') Update IM_KitMaster set ItemName='" + itname + "' where KitCode='" + kitcode + "' and CollegeCode='" + collegecode + "' else insert into IM_KitMaster (ItemName,ItemCode,KitCode,CollegeCode) values('" + itname + "','" + itcode + "','" + kitcode + "','" + collegecode + "')";
                            insertkit = d2.update_method_wo_parameter(sqlins, "Text");

                        }
                    }
                    if (!check)
                    {
                        alertimg.Visible = true;
                        lbl_alert.Text = "Pease Select Any Item";

                    }
                    if (insertkit > 0)
                    {
                        alertimg.Visible = true;
                        lbl_alert.Text = "Saved Successfully";
                        btn_go_Click(sender, e);
                    }
                }
            }
            else
            {
                alertimg.Visible = true;
                lbl_alert.Text = "Please Select Kit Name";

            }

        }
        catch
        {

        }

    }
    #endregion

    #region Delete
    protected void btn_Delete_Click(object sender, EventArgs e)
    {
        try
        {
            surediv.Visible = true;
            lbl_sure.Text = "Do you want to Delete this Record?";

        }
        catch
        {


        }

    }

    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        try
        {
           
            int deletekit = 0;
            if (ddl_collegename.Items.Count > 0)
                collcode = Convert.ToString(ddl_collegename.SelectedValue);
            if (ddl_Kitname.Items.Count > 0)
                kitcode = Convert.ToString(ddl_Kitname.SelectedValue);
            if (kitcode != "0" && kitcode != " ")
            {
                if (spreadDet1.Sheets[0].RowCount > 0)
                {
                    spreadDet1.SaveChanges();
                    for (int row = 0; row < spreadDet1.Sheets[0].RowCount; row++)
                    {
                        int checkval = Convert.ToInt32(spreadDet1.Sheets[0].Cells[row, 5].Value);
                        if (checkval == 1)
                        {
                            check = true;
                            itcode = Convert.ToString(spreadDet1.Sheets[0].Cells[row, 3].Tag);
                            itname = Convert.ToString(spreadDet1.Sheets[0].Cells[row, 4].Tag);
                            string sqldel = "if exists (select * from IM_KitMaster where ItemCode='" + itcode + "' and KitCode='" + kitcode + "' and CollegeCode='" + collcode + "')Delete from  IM_KitMaster where ItemCode='" + itcode + "' and KitCode='" + kitcode + "' and CollegeCode='" + collcode + "'";
                            deletekit = d2.update_method_wo_parameter(sqldel, "Text");

                        }
                    }
                    if (!check)
                    {
                        alertimg.Visible = true;
                        lbl_alert.Text = "Pease Select Any Item";

                    }
                    if (deletekit > 0)
                    {
                        alertimg.Visible = true;
                        lbl_alert.Text = "Deleted Successfully";
                        surediv.Visible = false;
                        btn_go_Click(sender, e);
                    }
                }
            }
            else
            {
                alertimg.Visible = true;
                lbl_alert.Text = "Please Select Kit Name";

            }
        }
        catch { }
    }

    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        try
        {

            surediv.Visible = false;
            alertimg.Visible = false;

        }
        catch
        {
        }

    }
    #endregion

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        try
        {
            alertimg.Visible = false;
        }
        catch
        {

        }
    }
}