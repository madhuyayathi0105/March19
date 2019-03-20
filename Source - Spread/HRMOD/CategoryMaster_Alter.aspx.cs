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
using System.Web.Services;
using System.Text.RegularExpressions;


public partial class CategoryMaster_Alter : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string popcol = string.Empty;
    static string autocol = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    string selectQuery = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    DAccess2 queryObject = new DAccess2();
    DAccess2 da = new DAccess2();
    Boolean cellclick = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            //string PageLogOut = "";
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("HRM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["HR"].Trim();
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

            if (!Request.FilePath.Contains("HRMenuIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/HRMOD/HRMenuIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            //collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();

            if (!IsPostBack)
            {
                bindclg();
                if (ddl_college.Items.Count > 0)
                {
                    collegecode1 = Convert.ToString(ddl_college.SelectedItem.Value);
                }
                if (ddl_popclg.Items.Count > 0)
                {
                    popcol = Convert.ToString(ddl_popclg.SelectedItem.Value);
                    autocol = Convert.ToString(ddl_popclg.SelectedItem.Value);
                }
                bindcatg();
                bindgroup();
                loaddesc();
                btn_go_Click(sender, e);
            }
            if (ddl_college.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_college.SelectedItem.Value);
            }
            if (ddl_popclg.Items.Count > 0)
            {
                popcol = Convert.ToString(ddl_popclg.SelectedItem.Value);
                autocol = Convert.ToString(ddl_popclg.SelectedItem.Value);
            }
            lbl_validation.Visible = false;
        }
        catch (Exception ex) { }
    }

    [WebMethod]
    public static string checkCatAcr(string CatAcr)
    {
        string returnValue = "1";
        try
        {
            DAccess2 dd = new DAccess2();
            string cat_acr = CatAcr;
            if (cat_acr.Trim() != "" && cat_acr != null)
            {
                string querycatacr = dd.GetFunction("select distinct CategoryAcr,CategoryMasterPK from HRM_CategoryMaster where CategoryAcr='" + cat_acr + "'");
                if (querycatacr.Trim() == "" || querycatacr == null || querycatacr == "0" || querycatacr == "-1")
                {
                    returnValue = "0";
                }
            }
            else
            {
                returnValue = "2";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }

    [WebMethod]
    public static string checkCatName(string CatName)
    {
        string returnValue = "1";
        try
        {
            DAccess2 dd = new DAccess2();
            string cat_name = CatName;
            if (cat_name.Trim() != "" && cat_name != null)
            {
                string querycatname = dd.GetFunction("select distinct category_code,category_name from staffcategorizer where category_name='" + cat_name + "' and college_code='" + autocol + "'");
                if (querycatname.Trim() == "" || querycatname == null || querycatname == "0" || querycatname == "-1")
                {
                    returnValue = "0";
                }
            }
            else
            {
                returnValue = "2";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }

    public void bindgroup()
    {
        ddl_grp.Items.Clear();
        ds.Tables.Clear();

        string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='HCGrp' and college_code ='" + collegecode1 + "'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_grp.DataSource = ds;
            ddl_grp.DataTextField = "TextVal";
            ddl_grp.DataValueField = "TextCode";
            ddl_grp.DataBind();
            ddl_grp.Items.Insert(0, new ListItem("All", "0"));
        }
        else
        {
            ddl_grp.Items.Insert(0, new ListItem("All", "0"));
        }
    }

    protected void ddl_college_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        collegecode1 = Convert.ToString(ddl_college.SelectedItem.Value);
        bindcatg();
        bindgroup();
        FpSpread1.Visible = false;
        div1.Visible = false;
        rptprint.Visible = false;
    }

    protected void ddl_popclg_Change(object sender, EventArgs e)
    {
        popcol = Convert.ToString(ddl_popclg.SelectedItem.Value);
        autocol = Convert.ToString(ddl_popclg.SelectedItem.Value);
        btn_addnew_Click(sender, e);
    }

    public void bindclg()
    {
        try
        {
            ds.Clear();
            ddl_college.Items.Clear();
            ddl_popclg.Items.Clear();

            selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();

                ddl_popclg.DataSource = ds;
                ddl_popclg.DataTextField = "collname";
                ddl_popclg.DataValueField = "college_code";
                ddl_popclg.DataBind();
            }
        }
        catch (Exception ex) { }
    }

    public void bindcatg()
    {
        ds.Clear();
        ddl_catname.Items.Clear();
        selectQuery = "select category_code,category_name from staffcategorizer where college_code='" + collegecode1 + "'";

        ds = d2.select_method_wo_parameter(selectQuery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_catname.DataSource = ds;
            ddl_catname.DataTextField = "category_name";
            ddl_catname.DataValueField = "category_code";
            ddl_catname.DataBind();
            ddl_catname.Items.Insert(0, new ListItem("All", "0"));
        }
    }

    protected void ddl_catname_OnSelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddl_grp_OnSelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        string category = "";
        //ddl_popclg.SelectedIndex = ddl_popclg.Items.IndexOf(ddl_popclg.Items.FindByValue(ddl_college.SelectedItem.Value));
        popcol = Convert.ToString(ddl_popclg.SelectedItem.Value);
        autocol = Convert.ToString(ddl_popclg.SelectedItem.Value);
        ddl_popclg.Enabled = true;
        string getacr = "select GeneralAcr,StartNo,SerialSize,SettingValues from HRS_CodeSettings where SettingField='4' and CollegeCode='" + popcol + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(getacr, "Text");
        string getexist = d2.GetFunction("select value from Master_Settings where settings='CodeSetting Rights' and value is not null and value<>''");
        category = getcatcode(getexist, ds);

        btndel.Visible = false;
        txt_categname.Text = "";
        txt_catcode.Text = "";
        cb_report.Checked = false;
        txt_catcode.Text = Convert.ToString(category);
        addnew.Visible = true;
        div1.Visible = false;
        rptprint.Visible = false;
        btn_save.Text = "Save";
        txt_catcode.Enabled = false;
        ddl_group.SelectedIndex = 0;
        hide();
    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        addnew.Visible = false;
        btn_go_Click(sender, e);
    }

    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            hide();
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 5;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            string cat = "";
            string grp = "";

            if (ddl_catname.Items.Count > 0)
            {
                if (ddl_catname.SelectedIndex != 0)
                {
                    cat = " and category_code='" + ddl_catname.SelectedItem.Value.ToString() + "'";
                }
            }
            if (ddl_grp.Items.Count > 0)
            {
                if (ddl_grp.SelectedIndex != 0)
                {
                    grp = " and CatGroup='" + ddl_grp.SelectedItem.Value.ToString() + "'";
                }
            }

            string selqry = "select (select textval from textvaltable t where CAST(t.TextCode as varchar)=CatGroup) as grpname,category_code,category_name,CASE WHEN DispReports = 1 THEN 'Yes' ELSE 'No' END DisReports FROM staffcategorizer sc where sc.college_code='" + collegecode1 + "'";
            if (cat.Trim() != "")
            {
                selqry = selqry + cat;
            }
            if (grp.Trim() != "")
            {
                selqry = selqry + grp;
            }
            selqry = selqry + " order by category_code";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                FpSpread1.Sheets[0].Rows.Count = ds.Tables[0].Rows.Count;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    FpSpread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                    FpSpread1.Sheets[0].Cells[i, 0].Font.Name = "Book Antiqua";
                    string GroupCode = Convert.ToString(ds.Tables[0].Rows[i]["grpname"]);

                    FpSpread1.Sheets[0].Cells[i, 1].Text = GroupCode;
                    FpSpread1.Sheets[0].Cells[i, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[i, 2].Text = ds.Tables[0].Rows[i]["category_code"].ToString();
                    FpSpread1.Sheets[0].Cells[i, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[i, 3].Text = ds.Tables[0].Rows[i]["category_name"].ToString();
                    FpSpread1.Sheets[0].Cells[i, 3].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[i, 4].Text = ds.Tables[0].Rows[i]["DisReports"].ToString();
                    FpSpread1.Sheets[0].Cells[i, 4].Font.Name = "Book Antiqua";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Visible = true;
                lbl_erroralert.Text = "No Records Found";
                FpSpread1.Visible = false;
                div1.Visible = false;
                rptprint.Visible = false;
                return;
            }

            for (int i = 0; i < 5; i++)
            {
                FpSpread1.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;
            }

            FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;

            FpSpread1.Sheets[0].ColumnHeader.Columns[0].Width = 52;
            FpSpread1.Sheets[0].ColumnHeader.Columns[1].Width = 133;
            FpSpread1.Sheets[0].ColumnHeader.Columns[2].Width = 116;
            FpSpread1.Sheets[0].ColumnHeader.Columns[3].Width = 220;
            FpSpread1.Sheets[0].ColumnHeader.Columns[4].Width = 169;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Group";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Category Code";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Category Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Display in Reports";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.SaveChanges();
            FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count;
            FpSpread1.Visible = true;
            addnew.Visible = false;
            div1.Visible = true;
            rptprint.Visible = true;
        }
        catch { }
    }

    protected void FpSpread1_OnCellClick(object sender, EventArgs e)
    {
        cellclick = true;
    }

    protected void FpSpread1_Selectedindexchange(object sender, EventArgs e)
    {
        try
        {
            if (cellclick == true)
            {
                btndel.Visible = true;
                addnew.Visible = true;
                string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
                ddl_popclg.SelectedIndex = ddl_popclg.Items.IndexOf(ddl_popclg.Items.FindByValue(ddl_college.SelectedItem.Value));
                ddl_popclg.Enabled = false;
                popcol = Convert.ToString(ddl_popclg.SelectedItem.Value);
                loaddesc();
                string groupcode = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                txt_catcode.Text = groupcode;
                txt_catcode.Enabled = false;
                string catname = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                txt_categname.Text = catname;
                string groupval = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text.ToString();
                for (int i = 0; i < ddl_group.Items.Count; i++)
                {
                    if (ddl_group.Items[i].Text.ToString().Trim() == groupval.Trim())
                    {
                        ddl_group.SelectedIndex = i;
                    }
                }
                if (groupval.Trim() == "")
                {
                    ddl_group.SelectedIndex = 0;
                }

                string value = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text.ToString();


                if (value.Trim() == "Yes")
                {
                    cb_report.Checked = true;
                }
                else
                {
                    cb_report.Checked = false;
                }
                btn_save.Text = "Update";
            }
        }
        catch { }
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            int inscount = 0;
            int upscount = 0;
            string CategoryID = "";
            string actrow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            if (Convert.ToInt32(actrow) != -1)
            {
                CategoryID = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Tag);
            }
            string Catcode = txt_catcode.Text.ToString();
            string CatName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(txt_categname.Text.ToString());
            string Group = "";

            if (ddl_group.SelectedIndex != 0 && ddl_group.SelectedItem.Text != "Select")
            {
                Group = ddl_group.SelectedItem.Value;
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Visible = true;
                lbl_erroralert.Text = "Please Select Group! ";
                return;
            }

            string Displayrpt = "";
            if (cb_report.Checked == true)
            {
                Displayrpt = "1";
            }
            else
            {
                Displayrpt = "0";
            }
            if (btn_save.Text.Trim().ToUpper() == "SAVE")
            {
                string insert = "insert into staffcategorizer (category_code,category_name,college_code,DispReports,CatGroup) values ('" + Catcode.ToUpper() + "','" + CatName + "','" + popcol + "','" + Displayrpt + "','" + Group + "')";

                inscount = d2.update_method_wo_parameter(insert, "Text");
            }
            else if (btn_save.Text.Trim().ToUpper() == "UPDATE")
            {
                string updqry = "update staffcategorizer set category_name='" + CatName + "',category_code='" + Catcode.ToUpper() + "',DispReports='" + Displayrpt + "',CatGroup='" + Group + "' where category_code='" + Catcode + "'  and  college_code='" + popcol + "'";
                upscount = d2.update_method_wo_parameter(updqry, "Text");
            }

            imgdiv2.Visible = true;
            lbl_erroralert.Visible = true;
            if (btn_save.Text.ToUpper().Trim() == "SAVE")
            {
                if (inscount > 0)
                {
                    lbl_erroralert.Text = "Saved Sucessfully";
                }
            }
            else if (btn_save.Text.ToUpper().Trim() == "UPDATE")
            {
                if (upscount > 0)
                {
                    lbl_erroralert.Text = "Updated Sucessfully";
                }
            }

            addnew.Visible = false;
            loaddesc();
            bindcatg();
            bindgroup();

            btn_go_Click(sender, e);
        }
        catch { }
    }

    protected void btn_exit_Click(object sender, EventArgs e)
    {
        addnew.Visible = false;
        btn_go_Click(sender, e);
    }

    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string Categorymaster = "CategoryMaster";
            string pagename = "CategoryMaster_Alter.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, Categorymaster);
            Printcontrol.Visible = true;
        }
        catch { }
    }

    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txt_excelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
                lbl_validation.Visible = false;
            }
            else
            {
                lbl_validation.Text = "Please Enter Your Report Name";
                lbl_validation.Visible = true;
                txt_excelname.Focus();
            }
        }
        catch { }
    }

    protected void btnplus_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = true;
        panel_description.Visible = true;
    }

    protected void btnminus_Click(object sender, EventArgs e)
    {
        if (ddl_group.Items.Count > 0)
        {
            string selsql = "select * from staffcategorizer where CatGroup='" + ddl_group.SelectedItem.Value.ToString() + "' and college_code='" + popcol + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(selsql, "Text");
            if (ds.Tables[0].Rows.Count == 0)
            {
                string sql = "delete from textvaltable where TextCode='" + ddl_group.SelectedItem.Value.ToString() + "' and TextCriteria='HCGrp' and college_code='" + popcol + "' ";
                int delete = d2.update_method_wo_parameter(sql, "TEXT");
                if (delete != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Deleted Sucessfully";
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "No Records Found";
                }
                loaddesc();
                bindgroup();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Group Already Alloted For Category Name";
            }
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_erroralert.Text = "No Records Found";
        }
    }

    public void loaddesc()
    {
        ddl_group.Items.Clear();
        ds.Tables.Clear();

        string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='HCGrp' and college_code ='" + popcol + "'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_group.DataSource = ds;
            ddl_group.DataTextField = "TextVal";
            ddl_group.DataValueField = "TextCode";
            ddl_group.DataBind();
            ddl_group.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            ddl_group.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    protected void btndescpopadd_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_description11.Text != "")
            {
                string textval = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(txt_description11.Text);
                string sql = "if exists ( select * from TextValTable where TextVal ='" + textval + "' and TextCriteria ='HCGrp' and college_code ='" + popcol + "') update TextValTable set TextVal ='" + textval + "' where TextVal ='" + textval + "' and TextCriteria ='HCGrp' and college_code ='" + popcol + "' else insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + textval + "','HCGrp','" + popcol + "')";
                int insert = d2.update_method_wo_parameter(sql, "TEXT");
                if (insert != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Saved Sucessfully";
                    txt_description11.Text = "";
                    imgdiv3.Visible = false;
                    panel_description.Visible = false;
                }
                loaddesc();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Enter the Description";
            }
        }
        catch (Exception ex) { }
    }

    protected void btndescpopexit_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = false;
        panel_description.Visible = false;
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    public void btndel_Click(object sender, EventArgs e)
    {
        imgdiv1.Visible = true;
        lblalert.Visible = true;
        lblalert.Text = "Do you want to Delete this Record?";
    }

    public void hide()
    {
        lbl_validation.Visible = false;
        Printcontrol.Visible = false;
        div1.Visible = false;
        rptprint.Visible = false;
    }

    protected void btnyes_Click(object sender, EventArgs e)
    {
        try
        {
            int savecc = 0;
            string actrow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            string categorcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Text);

            string selsql = "select * from pfslabs where category_code='" + categorcode + "' and college_code='" + popcol + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(selsql, "Text");
            if (ds.Tables[0].Rows.Count == 0)
            {
                string sql = " delete from staffcategorizer  where category_code='" + categorcode + "'  and  college_code='" + popcol + "'";
                int qry = d2.update_method_wo_parameter(sql, "Text");
                savecc++;
                if (savecc > 0)
                {
                    imgdiv1.Visible = false;
                    lblalert.Visible = false;
                    lbl_erroralert.Text = "Deleted Successfully";
                    lbl_erroralert.Visible = true;
                    imgdiv2.Visible = true;
                    btn_go_Click(sender, e);
                }
                addnew.Visible = false;
                txt_catcode.Text = "";
                txt_categname.Text = "";
                cb_report.Checked = false;
                loaddesc();
                bindcatg();
                bindgroup();

                btn_go_Click(sender, e);
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "This Category Already Used in Slabs Master";
            }
        }
        catch { }
    }

    protected void btnno_Click(object sender, EventArgs e)
    {
        imgdiv1.Visible = false;
    }

    private string getcatcode(string setting, DataSet dsacr)
    {
        DataSet dnew = new DataSet();
        string[] aplval = new string[5];
        string[] splval = new string[5];
        string code = "";
        string catacr = "";
        string getdsval = "";
        int startno = 0;
        int size = 0;
        string getval = "";
        try
        {
            if (setting.Trim() != "0" && setting.Trim() != "" && setting.Trim() != null && dsacr.Tables.Count > 0 && dsacr.Tables[0].Rows.Count > 0)
            {
                aplval = setting.Split(',');
                if (aplval.Contains("4"))
                {
                    getdsval = Convert.ToString(dsacr.Tables[0].Rows[0]["SettingValues"]);
                    if (getdsval.Trim() != "")
                    {
                        splval = getdsval.Split(';');
                        if (splval.Length > 0)
                        {
                            for (int ik = 0; ik < splval.Length; ik++)
                            {
                                if (splval[ik] == "1")
                                {
                                    string getcolacr = d2.GetFunction("select Coll_acronym from collinfo where college_code='" + popcol + "'");
                                    if (getcolacr.Trim() != "0" && getcolacr.Trim() != "" && getcolacr.Trim() != null)
                                    {
                                        catacr = catacr + getcolacr;
                                    }
                                }
                                if (splval[ik] == "3")
                                {
                                    catacr = catacr + Convert.ToString(dsacr.Tables[0].Rows[0]["GeneralAcr"]);
                                }
                            }
                        }
                        Int32.TryParse(Convert.ToString(dsacr.Tables[0].Rows[0]["StartNo"]), out startno);
                        Int32.TryParse(Convert.ToString(dsacr.Tables[0].Rows[0]["SerialSize"]), out size);
                        int startlen = Convert.ToString(startno).Trim().Length;
                        int totsize = size - startlen;
                        string selectquery = "select category_code from staffcategorizer where category_code like '" + catacr + "%' and college_code='" + popcol + "' order by LEN(category_code),category_code";
                        dnew = d2.select_method_wo_parameter(selectquery, "Text");
                        if (dnew.Tables[0].Rows.Count > 0)
                        {
                            string concadnew = Convert.ToString(dnew.Tables[0].Rows[dnew.Tables[0].Rows.Count - 1][0]);
                            string concad = "";
                            for (int i = 0; i < catacr.Length; i++)
                            {
                                char a = concadnew[i];
                                concad = concad + a;
                            }
                            string input = concadnew;
                            string[] stringSeparators = new string[] { concad };

                            var result = concadnew.Split(stringSeparators, StringSplitOptions.None);
                            string catcode = result[1];
                            int catcode1 = Convert.ToInt32(catcode);
                            catcode1 = catcode1 + 1;
                            getval = Convert.ToString(catcode1);
                            for (int ik = 0; ik < totsize; ik++)
                            {
                                getval = Convert.ToString("0") + getval;
                            }
                            code = concad + Convert.ToString(getval);
                        }
                        else
                        {
                            for (int ik = 0; ik < totsize; ik++)
                            {
                                getval = getval + Convert.ToString("0");
                            }
                            code = Convert.ToString(catacr) + getval + Convert.ToString(startno);
                        }
                    }
                }
                else
                {
                    string catcod = ("select category_code from staffcategorizer where category_code like 'CAT%' and college_code='" + popcol + "' order by LEN(category_code),category_code");
                    dnew = d2.select_method_wo_parameter(catcod, "Text");
                    if (dnew.Tables[0].Rows.Count > 0)
                    {
                        string concadnew = Convert.ToString(dnew.Tables[0].Rows[dnew.Tables[0].Rows.Count - 1][0]);
                        string concad = "";
                        for (int i = 0; i < 3; i++)
                        {
                            char a = concadnew[i];
                            concad = concad + a;
                        }
                        string input = concadnew;
                        string[] stringSeparators = new string[] { concad };

                        var result = concadnew.Split(stringSeparators, StringSplitOptions.None);
                        string catcode = result[1];
                        int catcode1 = Convert.ToInt32(catcode);
                        catcode1 = catcode1 + 1;
                        code = concad + Convert.ToString(catcode1);
                    }
                }
            }
            else
            {
                string catcod = ("select category_code from staffcategorizer where category_code like 'CAT%' and college_code='" + popcol + "' order by LEN(category_code),category_code");
                dnew = d2.select_method_wo_parameter(catcod, "Text");
                if (dnew.Tables[0].Rows.Count > 0)
                {
                    string concadnew = Convert.ToString(dnew.Tables[0].Rows[dnew.Tables[0].Rows.Count - 1][0]);
                    string concad = "";
                    for (int i = 0; i < 3; i++)
                    {
                        char a = concadnew[i];
                        concad = concad + a;
                    }
                    string input = concadnew;
                    string[] stringSeparators = new string[] { concad };

                    var result = concadnew.Split(stringSeparators, StringSplitOptions.None);
                    string catcode = result[1];
                    int catcode1 = Convert.ToInt32(catcode);
                    catcode1 = catcode1 + 1;
                    code = concad + Convert.ToString(catcode1);
                }
            }
        }
        catch { }
        return code;
    }
}