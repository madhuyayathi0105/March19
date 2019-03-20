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

public partial class LeaveMaster_Alter : System.Web.UI.Page
{
    bool cellclick = false;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string autocol = string.Empty;

    string selectQuery = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    DAccess2 queryObject = new DAccess2();
    DAccess2 da = new DAccess2();
    DataTable dtGrid = new DataTable();
    DataTable dtl = new DataTable();
    DataRow dtrow = null;

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["dtGrid"] != null)
            {
                Session.Remove("dtGrid");
            }

        }
        callGridBind();
    }

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
            collegecode1 = Session["collegecode"].ToString();

            if (!IsPostBack)
            {
                if (Session["dtGrid"] != null)
                {
                    Session.Remove("dtGrid");
                }
                hide();
                bindclg();
                bindLeaveReasonMapping();
                if (ddl_college.Items.Count > 0)
                {
                    collegecode1 = Convert.ToString(ddl_college.SelectedItem.Value);
                }
                if (ddl_popclg.Items.Count > 0)
                {
                    collegecode = Convert.ToString(ddl_popclg.SelectedItem.Value);
                    autocol = Convert.ToString(ddl_popclg.SelectedItem.Value);
                }
                btn_go_Click(sender, e);
            }
            if (ddl_college.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_college.SelectedItem.Value);
            }
            if (ddl_popclg.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddl_popclg.SelectedItem.Value);
                autocol = Convert.ToString(ddl_popclg.SelectedItem.Value);
            }
            lbl_validation.Visible = false;
        }
        catch (Exception ex) { }
    }

    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    protected void ddl_college_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        collegecode1 = Convert.ToString(ddl_college.SelectedItem.Value);
        btn_go_Click(sender, e);
    }

    protected void ddl_popclg_Change(object sender, EventArgs e)
    {
        collegecode = Convert.ToString(ddl_popclg.SelectedItem.Value);
        autocol = Convert.ToString(ddl_popclg.SelectedItem.Value);
        string cat_name = Convert.ToString(txt_leavename.Text);
        string acronym = Convert.ToString(txt_shrtfm.Text);

        string catname = d2.GetFunction("select distinct category from leave_category where category='" + cat_name + "' and college_code='" + collegecode + "'");
        if (catname.Trim() != "" && catname.Trim() != "0")
        {
            imgdiv2.Visible = true;
            lbl_alert.Visible = true;
            lbl_alert.Text = "Leave Name Already Exist!";
            txt_leavename.Text = "";
        }
        string acronym1 = d2.GetFunction("select distinct shortname from leave_category where shortname='" + acronym + "' and college_code='" + collegecode + "'");
        if (acronym1.Trim() != "" && acronym1.Trim() != "0")
        {
            imgdiv2.Visible = true;
            lbl_alert.Visible = true;
            lbl_alert.Text = "Leave Acronym Already Exist!";
            txt_shrtfm.Text = "";
        }
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

    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        ddl_popclg.SelectedIndex = ddl_popclg.Items.IndexOf(ddl_popclg.Items.FindByValue(ddl_college.SelectedItem.Value));
        collegecode = Convert.ToString(ddl_popclg.SelectedItem.Value);
        autocol = Convert.ToString(ddl_popclg.SelectedItem.Value);
        ddl_popclg.Enabled = true;
        addnew.Visible = true;
        rb_earn.Checked = false;
        rb_tpres.Checked = false;
        rb_gnrl.Checked = false;
        txt_leavename.Enabled = true;
        txt_shrtfm.Enabled = true;
        txt_leavename.Text = "";
        txt_shrtfm.Text = "";
        btn_save.Visible = true;
        btndel.Visible = false;
        btn_save.Text = "Save";
    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        addnew.Visible = false;
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            hide();

            string selqry = "select category,shortname,status,LeaveMasterPK from leave_category where college_Code='" + collegecode1 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "text");
            grdleave.Visible = false;
            if (ds.Tables[0].Rows.Count > 0)
            {
                dtl.Clear();
                dtrow = dtl.NewRow();
                dtl.Rows.Add(dtrow);

                for (int col = 0; col < 7; col++)
                {
                    dtl.Columns.Add("", typeof(string));

                }

                dtl.Rows[0][0] = "S.No";
                dtl.Rows[0][1] = "Leave Name";
                dtl.Rows[0][2] = "Short Form";
                dtl.Rows[0][3] = "Earn Leave";
                dtl.Rows[0][4] = "Treated As Present";
                dtl.Rows[0][5] = "Treated As LOP";
                dtl.Rows[0][6] = "LeavePk";
                int sno = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    dtrow = dtl.NewRow();
                    dtl.Rows.Add(dtrow);
                    dtl.Rows[dtl.Rows.Count - 1][0] = Convert.ToString(sno);
                    dtl.Rows[dtl.Rows.Count - 1][1] = Convert.ToString(ds.Tables[0].Rows[i]["category"]);
                    dtl.Rows[dtl.Rows.Count - 1][2] = Convert.ToString(ds.Tables[0].Rows[i]["shortname"]);
                    dtl.Rows[dtl.Rows.Count - 1][6] = Convert.ToString(ds.Tables[0].Rows[i]["LeaveMasterPK"]);
                    string statusss = Convert.ToString(ds.Tables[0].Rows[i]["status"]);
                    if (statusss.Trim() == "2")
                    {
                        dtl.Rows[dtl.Rows.Count - 1][3] = 1;

                    }
                    else
                    {
                        dtl.Rows[dtl.Rows.Count - 1][3] = 0;
                    }
                    if (statusss.Trim() == "0")
                    {
                        dtl.Rows[dtl.Rows.Count - 1][4] = 1;
                    }
                    else
                    {
                        dtl.Rows[dtl.Rows.Count - 1][4] = 0;
                    }
                    if (statusss.Trim() == "1")
                    {
                        dtl.Rows[dtl.Rows.Count - 1][5] = 1;
                    }
                    else
                    {
                        dtl.Rows[dtl.Rows.Count - 1][5] = 0;
                    }
                }


                if (dtl.Rows.Count >= 1)
                {
                    Session["dtGrid"] = dtl;
                    callGridBind();
                    grdleave.DataSource = dtl;
                    grdleave.DataBind();
                    grdleave.Visible = true;
                    div1.Visible = true;

                }
                //        FpSpread1.Sheets[0].Cells[i, 3].CellType = chk;
                //        FpSpread1.Sheets[0].Cells[i, 4].CellType = chk;
                //        FpSpread1.Sheets[0].Cells[i, 5].CellType = chk;
                //        FpSpread1.Sheets[0].Cells[i, 6].CellType = chk;
                //        FpSpread1.Sheets[0].Cells[i, 3].Locked = true;
                //        FpSpread1.Sheets[0].Cells[i, 4].Locked = true;
                //        FpSpread1.Sheets[0].Cells[i, 5].Locked = true;
                //        FpSpread1.Sheets[0].Cells[i, 6].Locked = true;

                //        FpSpread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                //        FpSpread1.Sheets[0].Cells[i, 0].Font.Name = "Book Antiqua";
                //        FpSpread1.Sheets[0].Cells[i, 1].Text = ds.Tables[0].Rows[i]["category"].ToString();
                //        FpSpread1.Sheets[0].Cells[i, 1].Font.Name = "Book Antiqua";
                //        FpSpread1.Sheets[0].Cells[i, 2].Text = ds.Tables[0].Rows[i]["shortname"].ToString();
                //        FpSpread1.Sheets[0].Cells[i, 2].Font.Name = "Book Antiqua";
                //        FpSpread1.Sheets[0].Cells[i, 7].Value = ds.Tables[0].Rows[i]["LeaveMasterPK"].ToString();
                //        FpSpread1.Sheets[0].Cells[i, 7].Font.Name = "Book Antiqua";

                //        FpSpread1.Sheets[0].Cells[i, 3].Font.Name = "Book Antiqua";
                //        FpSpread1.Sheets[0].Cells[i, 4].Font.Name = "Book Antiqua";
                //        FpSpread1.Sheets[0].Cells[i, 5].Font.Name = "Book Antiqua";
                //        string statusss = ds.Tables[0].Rows[i]["status"].ToString();
                //        if (statusss.Trim() == "2")
                //        {
                //            FpSpread1.Sheets[0].Cells[i, 3].Value = 1;
                //        }
                //        if (statusss.Trim() == "0")
                //        {
                //            FpSpread1.Sheets[0].Cells[i, 4].Value = 1;
                //        }
                //        if (statusss.Trim() == "1")
                //        {
                //            FpSpread1.Sheets[0].Cells[i, 5].Value = 1;
                //        }

                //    FpSpread1.Sheets[0].ColumnHeader.Columns[0].Width = 60;
                //    FpSpread1.Sheets[0].ColumnHeader.Columns[1].Width = 244;
                //    FpSpread1.Sheets[0].ColumnHeader.Columns[2].Width = 150;
                //    FpSpread1.Sheets[0].ColumnHeader.Columns[3].Width = 102;
                //    FpSpread1.Sheets[0].ColumnHeader.Columns[4].Width = 102;
                //    FpSpread1.Sheets[0].ColumnHeader.Columns[5].Width = 102;
                //    FpSpread1.Sheets[0].ColumnHeader.Columns[6].Width = 102;

                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Leave Name";
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Short Form";
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Earn Leave";
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Treated As Present";
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Treated As LOP";
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Select";
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Id";
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                //    FpSpread1.Sheets[0].Columns[7].Visible = false;
                //    FpSpread1.Sheets[0].Columns[6].Visible = false;
                //    for (int i = 0; i < FpSpread1.Sheets[0].Columns.Count; i++)
                //    {
                //        FpSpread1.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;
                //        FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Name = "Book Antiqua";
                //        FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Bold = true;
                //        FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Size = FontUnit.Medium;
                //        FpSpread1.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;

                //    }
                //    FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                //    FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;

                //    FpSpread1.SaveChanges();
                //    FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count;
                //    FpSpread1.Visible = true;
                //    addnew.Visible = false;
                //    div1.Visible = true;
                //    rptprint.Visible = true;
                //    lbl_error.Visible = false;
            }
            else
            {
                lbl_error.Visible = true;
                lbl_error.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
            //  d2.sendErrorMail(ex, collegecode1, "LeaveMaster_Alter.aspx");
        }
    }

    protected void FpSpread1_OnCellClick(object sender, EventArgs e)
    {
        //string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
        //string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
        //cellclick = true;
        //addnew.Visible = true;
    }

    protected void FpSpread1_Selectedindexchange(object sender, EventArgs e)
    {
        try
        {
            //FpSpread1.SaveChanges();
            //if (cellclick == true)
            //{
            //    rb_earn.Checked = false;
            //    rb_tpres.Checked = false;
            //    rb_gnrl.Checked = false;

            //    ddl_popclg.SelectedIndex = ddl_popclg.Items.IndexOf(ddl_popclg.Items.FindByValue(ddl_college.SelectedItem.Value));
            //    ddl_popclg.Enabled = false;
            //    btn_save.Text = "Update";
            //    btndel.Visible = true;
            //    string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            //    string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
            //    string leavename = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text.ToString();
            //    txt_leavename.Text = leavename;
            //    string shortform = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text.ToString();
            //    txt_shrtfm.Text = shortform;
            //    string lblid = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Text.ToString();
            //    oldlname.Text = lblid;
            //    txt_leavename.Enabled = false;
            //    txt_shrtfm.Enabled = false;

            //    for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
            //    {
            //        if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[Convert.ToSByte(activerow), 3].Value) == 1)
            //        {
            //            rb_earn.Checked = true;
            //            rb_tpres.Checked = false;
            //            rb_gnrl.Checked = false;
            //        }
            //        if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[Convert.ToSByte(activerow), 4].Value) == 1)
            //        {
            //            rb_earn.Checked = false;
            //            rb_tpres.Checked = true;
            //            rb_gnrl.Checked = false;
            //        }
            //        if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[Convert.ToSByte(activerow), 5].Value) == 1)
            //        {
            //            rb_earn.Checked = false;
            //            rb_tpres.Checked = false;
            //            rb_gnrl.Checked = true;
            //        }
            //    }
            //}
        }
        catch { }
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        string collcode = "";
        try
        {
            string category = txt_leavename.Text;
            string shortname = (txt_shrtfm.Text).Trim().ToUpper();
            string leave = "";
            string lblid = Convert.ToString(oldlname.Text.ToString());

            if (rb_earn.Checked == true)
            {
                leave = "2";
            }
            else if (rb_tpres.Checked == true)
            {
                leave = "0";
            }
            else if (rb_gnrl.Checked == true)
            {
                leave = "1";
            }
            if (leave.Trim() != "")
            {
                if (btn_save.Text.Trim().ToLower() == "save")
                {
                    collcode = collegecode;
                    string sql = "insert into leave_category (category,shortname,status,college_code) values ('" + category + "','" + shortname + "','" + leave + "','" + collegecode + "')";
                    ds = d2.select_method_wo_parameter(sql, "Text");
                    imgdiv2.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Saved Successfully";
                }
                else if (btn_save.Text.Trim().ToLower() == "update")
                {
                    collcode = collegecode1;
                    string sql = "update  leave_category set category='" + category + "',shortname='" + shortname + "',status='" + leave + "',college_code='" + collegecode1 + "' where LeaveMasterPK='" + lblid + "'";
                    ds = d2.select_method_wo_parameter(sql, "Text");
                    imgdiv2.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Updated Successfully";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Please Select Any One Leave Type!";
                return;
            }

            addnew.Visible = false;
            btn_go_Click(sender, e);
            div1.Visible = true;
            rptprint.Visible = true;
            //FpSpread1.Visible = true;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collcode, "LeaveMaster_Alter.aspx");
        }
    }

    protected void btn_exit_Click(object sender, EventArgs e)
    {
        addnew.Visible = false;
    }

    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string Leavemaster = "Leavemaster Report";
            string pagename = "LeaveMaster_Alter.aspx";
            //Printcontrol.loadspreaddetails(FpSpread1, pagename, Leavemaster);
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
                //d2.printexcelreport(FpSpread1, reportname);
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

    public void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    public void hide()
    {
        lbl_validation.Visible = false;
        Printcontrol.Visible = false;
        div1.Visible = false;
        rptprint.Visible = false;
    }

    public void btndel_Click(object sender, EventArgs e)//delju
    {
        try
        {


            string category = txt_leavename.Text;
            string shortname = (txt_shrtfm.Text).Trim().ToUpper();

            string lblid = Convert.ToString(oldlname.Text.ToString());
            int savecc = 0;
            string sql = "delete  from leave_category where  LeaveMasterPK ='" + lblid + "' and college_code='" + collegecode + "'";
            int qry = d2.update_method_wo_parameter(sql, "Text");
            if (qry > 0)
            {
                lbl_alert.Text = "Deleted Successfully";
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;
                btn_go_Click(sender, e);
            }
            //int savecc = 0;
            //FpSpread1.SaveChanges();

            //string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            //string sql = "delete  from leave_category where  LeaveMasterPK = '" + FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Text.ToString() + "' and college_code= '" + collegecode + "'";
            //int qry = d2.update_method_wo_parameter(sql, "Text");
            //savecc++;
            //if (savecc > 0)
            //{
            //    lbl_alert.Text = "Deleted Successfully";
            //    lbl_alert.Visible = true;
            //    imgdiv2.Visible = true;
            //    btn_go_Click(sender, e);
            //}
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "LeaveMaster_Alter.aspx");
        }
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
                string querycatname = dd.GetFunction("select distinct shortname,category from leave_category where category='" + cat_name + "' and college_code='" + autocol + "'");
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
                string querycatacr = dd.GetFunction("select distinct shortname,category from leave_category where shortname='" + cat_acr + "' and college_Code='" + autocol + "'");
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
    protected void btn_streamplus_OnClick(object sender, EventArgs e)
    {
        Plusapt.Visible = true;
        btn_plusAdd.Visible = true;
        txt_addstream.Text = "";
    }
    protected void btn_streamminus_OnClick(object sender, EventArgs e)
    {
        string LeaveMapping = Convert.ToString(ddl_leave.SelectedItem);


        string query = "delete from TextValTable where TextVal='" + LeaveMapping + "' and college_code='" + collegecode1 + "'";
        int count = d2.update_method_wo_parameter(query, "Text");
        bindLeaveReasonMapping();

    }
    protected void btn_plusAdd_OnClick(object sender, EventArgs e)
    {
        string stream = txt_addstream.Text;
        string criteria = "LveMp";
        if (stream.Trim() != "")
        {
            string LeaveTypeM = Convert.ToString(ddlleavemapping.SelectedItem.Text);
            string query = "insert into TextValTable(TextVal,TextCriteria,college_code )values ('" + stream + "','" + criteria + "','" + collegecode1 + "')";
            int count = d2.update_method_wo_parameter(query, "Text");

            bindLeaveReasonMapping();
            if (count > 0)
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Saved Successfully\");", true);

            }



        }
    }
    protected void btn_Plusexit_OnClick(object sender, EventArgs e)
    {
        Plusapt.Visible = false;
        btn_plusAdd.Visible = false;
    }
    protected void ddl_leavereason_click(object sender, EventArgs e)
    {
        if (ddlleavemapping.Text != "Select")
        {
            btn_streamplus.Visible = true;
            ddl_leave.Visible = true;
            btn_streamminus.Visible = true;
            btn_saveval.Visible = true;


        }
        else
        {
            btn_streamplus.Visible = false;
            ddl_leave.Visible = false;
            btn_streamminus.Visible = false;
            btn_saveval.Visible = false;

        }

    }
    protected void bindLeaveReasonMapping()
    {
        ds.Clear();
        ddl_leave.Items.Clear();
        string item = "select distinct TextVal,TextCode from TextValTable where TextCriteria='LveMp' ";
        ds = d2.select_method_wo_parameter(item, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_leave.DataSource = ds;
            ddl_leave.DataTextField = "TextVal";
            ddl_leave.DataValueField = "TextCode";
            ddl_leave.DataBind();
            ddl_leave.Items.Insert(0, "Select");
        }
        else
        {
            ddl_leave.Items.Insert(0, "Select");
        }
    }
    protected void btn_Save_OnClick(object sender, EventArgs e)
    {
        string criteria = "ReaMp";
        if (ddlleavemapping.Text != "Select" && ddl_leave.Text != "Select")
        {
            string type = Convert.ToString(ddlleavemapping.SelectedItem.Text);
            string reason = Convert.ToString(ddl_leave.SelectedItem.Text);
            reason = reason + "-" + type;

            string sql = "if exists ( select * from TextValTable where TextVal ='" + reason + "' and TextCriteria ='ReaMp' and TextCriteria2='" + type + "' and college_code ='" + ddl_college.SelectedItem.Value + "') update TextValTable set TextVal ='" + reason + "' where TextCriteria ='ReaMp' and TextCriteria2='" + type + "' and college_code ='" + ddl_college.SelectedItem.Value + "' else insert into TextValTable (TextVal,TextCriteria,college_code,TextCriteria2) values ('" + reason + "','ReaMp','" + ddl_college.SelectedItem.Value + "','" + type + "')";
            int insert = d2.update_method_wo_parameter(sql, "TEXT");
            if (insert != 0)
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Saved Successfully\");", true);
            }

        }
        else
        {
            ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Leave Reason Mapping\");", true);
        }

    }
    protected void grdleave_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        //e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        //e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
        //e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
        //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
        //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
        //e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Font.Name = "Book Antiqua";
            e.Row.Font.Size = FontUnit.Medium;

            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[0].Width = 60;

            e.Row.Cells[1].Width = 245;
            e.Row.Cells[2].Width = 150;
            e.Row.Cells[3].Width = 100;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[4].Width = 100;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[5].Width = 100;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[6].Visible = false;

            if (e.Row.RowIndex == 0)
            {
                e.Row.BackColor = Color.FromArgb(12, 166, 202);
                e.Row.HorizontalAlign = HorizontalAlign.Center;
                e.Row.Font.Bold = true;
            }
            else
            {


                CheckBox check = new CheckBox();
                CheckBox check1 = new CheckBox();
                CheckBox check2 = new CheckBox();

                //check.AutoPostBack = true;
                check.ID = "checkbox_" + e.Row.RowIndex;
                check1.ID = "checkbox_" + e.Row.RowIndex;
                check2.ID = "checkbox_" + e.Row.RowIndex;
                if (dtGrid.Rows.Count > 0)
                {
                    string earnedleave = dtGrid.Rows[e.Row.RowIndex][3].ToString();
                    string treataspre = dtGrid.Rows[e.Row.RowIndex][4].ToString();
                    string treataslop = dtGrid.Rows[e.Row.RowIndex][5].ToString();
                    if (earnedleave == "1")
                    {

                        check.Checked = true;
                        check.Enabled = false;
                    }
                    else
                    {

                        check.Checked = false;
                        check.Enabled = false;
                    }
                    if (treataspre == "1")
                    {
                        check1.Checked = true;
                        check1.Enabled = false;

                    }
                    else
                    {
                        check1.Checked = false;
                        check1.Enabled = false;
                    }
                    if (treataslop == "1")
                    {
                        check2.Checked = true;
                        check2.Enabled = false;

                    }
                    else
                    {

                        check2.Checked = false;
                        check2.Enabled = false;
                    }


                }

                e.Row.Cells[3].Controls.Add(check);
                e.Row.Cells[4].Controls.Add(check1);
                e.Row.Cells[5].Controls.Add(check2);
            }
        }
    }
    protected void grdleave_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }
    protected void grdleave_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);



            rb_earn.Checked = false;
            rb_tpres.Checked = false;
            rb_gnrl.Checked = false;

            ddl_popclg.SelectedIndex = ddl_popclg.Items.IndexOf(ddl_popclg.Items.FindByValue(ddl_college.SelectedItem.Value));
            ddl_popclg.Enabled = false;
            btn_save.Text = "Update";
            btndel.Visible = true;
            string leavename = grdleave.Rows[rowIndex].Cells[1].Text;
            txt_leavename.Text = leavename;
            string shortform = grdleave.Rows[rowIndex].Cells[2].Text;
            txt_shrtfm.Text = shortform;

            string lblid = grdleave.Rows[rowIndex].Cells[6].Text;
            oldlname.Text = lblid;
            txt_leavename.Enabled = false;
            txt_shrtfm.Enabled = false;

            string earnes_lve = grdleave.Rows[rowIndex].Cells[3].Text;
            string treatedas_pres = grdleave.Rows[rowIndex].Cells[4].Text;
            string treatedas_lop = grdleave.Rows[rowIndex].Cells[5].Text;
            if (earnes_lve == "1")
            {
                rb_earn.Checked = true;
                rb_tpres.Checked = false;
                rb_gnrl.Checked = false;

            }
            if (treatedas_pres == "1")
            {
                rb_earn.Checked = false;
                rb_tpres.Checked = true;
                rb_gnrl.Checked = false;
            }
            if (treatedas_lop == "1")
            {
                rb_earn.Checked = false;
                rb_tpres.Checked = false;
                rb_gnrl.Checked = true;
            }
            addnew.Visible = true;
            //if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[Convert.ToSByte(activerow), 3].Value) == 1)
            //{
            //    rb_earn.Checked = true;
            //    rb_tpres.Checked = false;
            //    rb_gnrl.Checked = false;
            //}
            //if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[Convert.ToSByte(activerow), 4].Value) == 1)
            //{
            //    rb_earn.Checked = false;
            //    rb_tpres.Checked = true;
            //    rb_gnrl.Checked = false;
            //}
            //if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[Convert.ToSByte(activerow), 5].Value) == 1)
            //{
            //    rb_earn.Checked = false;
            //    rb_tpres.Checked = false;
            //    rb_gnrl.Checked = true;
            //}

        }
        catch (Exception ex)
        {
        }
    }

    public void callGridBind()
    {

        if (Session["dtGrid"] != null)
        {
            dtGrid = (DataTable)Session["dtGrid"];
            grdleave.DataSource = dtGrid;
            grdleave.DataBind();
            grdleave.HeaderRow.Visible = false;

        }
        else
        {
            grdleave.DataSource = null;
            grdleave.DataBind();
        }


    }
    protected void Leavepriority_click(object sender, EventArgs e)
    {
        try
        {
            popdept.Visible = true;

            string selqry = "select category,shortname,LeaveMasterPK,isnull(LeavePriority,'') as LeavePriority  from leave_category where college_Code='" + collegecode1 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    DataView dv = new DataView();
                    DataSet dskit = new DataSet();
                    DataTable dtleavepri = new DataTable();
                    DataRow drow;

                    dtleavepri.Columns.Add("LeaveName", typeof(string));
                    dtleavepri.Columns.Add("ShortName", typeof(string));
                    dtleavepri.Columns.Add("LeavePk", typeof(string));
                    dtleavepri.Columns.Add("chkval", typeof(bool));
                    dtleavepri.Columns.Add("Priority", typeof(string));
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        drow = dtleavepri.NewRow();
                        drow["LeaveName"] = Convert.ToString(ds.Tables[0].Rows[row]["category"]);
                        drow["ShortName"] = Convert.ToString(ds.Tables[0].Rows[row]["shortname"]);
                        drow["LeavePk"] = Convert.ToString(ds.Tables[0].Rows[row]["LeaveMasterPK"]);

                        if (Convert.ToString(ds.Tables[0].Rows[row]["LeavePriority"]).Trim() !="0")
                        {
                            drow["chkval"] = true;

                        }
                        else
                        {
                            drow["chkval"] = false;
                        }
                        string leavePri = Convert.ToString(ds.Tables[0].Rows[row]["LeavePriority"]);
                        string getpri = string.Empty;
                        if (leavePri != "0")
                        {
                            getpri = leavePri;

                        }
                        else
                        {
                            getpri = "";
                        }
                        drow["Priority"] = getpri;
                        dtleavepri.Rows.Add(drow);
                    }
                    if (dtleavepri.Rows.Count > 0)
                    {
                        grdpri.DataSource = dtleavepri;
                        grdpri.DataBind();
                        grdpri.Visible = true;
                        divdept.Visible = true;
                        DptPriorityDiv.Visible = true;

                    }
                    for (int i = 0; i < grdpri.Rows.Count; i++)
                    {
                        for (int j = 0; j < grdpri.HeaderRow.Cells.Count; j++)
                        {
                            if (j == 0)
                            {
                                grdpri.Rows[i].Cells[j].Width = 50;
                                grdpri.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }

                }
            }

        }
        catch (Exception ex)
        {

        }

    }
    protected void chkpriority_Change(object sender, EventArgs e)
    {


    }
    protected void imgdept_Click(object sender, EventArgs e)
    {
        try
        {
            popdept.Visible = false;


        }
        catch (Exception ex)
        {

        }

    }
    protected void grdpri_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[3].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Visible = false;
                CheckBox chk = e.Row.FindControl("selectchk") as CheckBox;
                if (chk.Text == "True")
                {
                    chk.Enabled = false;
                    chk.Checked = true;
                    chk.Text = "";

                }
                else if (chk.Text == "False")
                {
                    chk.Enabled = true;
                    chk.Checked = false;
                    chk.Text = "";

                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void grdpri_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        grdpri.PageIndex = e.NewPageIndex;

    }
    protected void setpriority_checkedchange(object sender, EventArgs e)
    {
        try
        {

            CheckBox grids = (CheckBox)sender;
            string rowIndxSs = grids.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int rowIndx = Convert.ToInt32(rowIndxSs) - 2;

            string indate = (grdpri.Rows[rowIndx].FindControl("priority") as Label).Text;
            int val = 0;
            if (grids.Checked == true && indate == "")
            {

                foreach (GridViewRow gvrow in grdpri.Rows)
                {

                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);

                    CheckBox chk = (CheckBox)grdpri.Rows[RowCnt].FindControl("selectchk");
                    string prio = (grdpri.Rows[rowIndx].FindControl("priority") as Label).Text;
                    if (chk.Checked == true && chk.Enabled == false)
                    {
                        val++;

                    }


                }
                if (val == 0)
                {

                    val = 1;
                }
                else
                {
                    val = val + 1;

                }
            }
            (grdpri.Rows[rowIndx].FindControl("priority") as Label).Text = Convert.ToString(val);
            (grdpri.Rows[rowIndx].FindControl("selectchk") as CheckBox).Enabled = false;
        }
        catch (Exception ex)
        {

        }
    }
    protected void btnsetdeptpriority_click(object sender, EventArgs e)
    {
        try
        {
            
            int upcount = 0;

            if (grdpri.Rows.Count > 0)
            {
                foreach (GridViewRow gvrow in grdpri.Rows)
                {
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    string updquery = "";
                    bool entry_flag = false;
                    Label leavepkval = (Label)grdpri.Rows[RowCnt].FindControl("LeavePk");
                    Label getpriority = (Label)grdpri.Rows[RowCnt].FindControl("priority");
                    string leavepk = Convert.ToString(leavepkval.Text);
                    string priority = Convert.ToString(getpriority.Text);
                    if (priority.Trim() != "" && priority.Trim() != "0")
                    {
                        entry_flag = true;
                        updquery = "update leave_category set LeavePriority='" + priority + "'";
                    }
                    else if (priority.Trim() == "")
                    {
                        entry_flag = true;
                        updquery = "update leave_category set LeavePriority=Null";
                    }
                   
                    if (updquery.Trim() != "")
                    {
                        updquery = updquery + " where LeaveMasterPK='" + leavepk + "' and college_code=" + collegecode + "";
                        int insQ = d2.update_method_wo_parameter(updquery, "Text");
                        if (insQ > 0)
                        {
                            upcount++;
                        }
                    }
                }
                if (upcount > 0)
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Saved Successfully\");", true);
                
                }

            }
          
        }
        catch (Exception ex)
        {

        }

    }
    protected void btnresetdeptpriority_click(object sender, EventArgs e)
    {
        try
        {
            if (grdpri.Rows.Count > 0)
            {
                int updat = 0;
                foreach (GridViewRow gvrow in grdpri.Rows)
                {
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    Label leavepk = (Label)grdpri.Rows[RowCnt].FindControl("leavepk");
                    string Leavepkval = Convert.ToString(leavepk.Text);

                    int insup = d2.update_method_wo_parameter("update leave_category set LeavePriority = NULL where LeaveMasterPK='" + Leavepkval + "'  and college_code='" + collegecode + "'", "Text");
                    if (insup > 0)
                    {
                        updat++;
                    
                    }

                }
                if (updat > 0)
                { 
                    Leavepriority_click(sender, e);
                
                }
            }
        }
        catch (Exception ex)
        {

        }

    }
    protected void btnexitdept_click(object sender, EventArgs e)
    {

        popdept.Visible = false;

    }

}