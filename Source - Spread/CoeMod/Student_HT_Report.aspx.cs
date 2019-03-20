using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class Student_HT_Report : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();

    DataView dv = new DataView();
    DataView dv1 = new DataView();
    DataSet newds = new DataSet();
    DataView dv2 = new DataView();
    DataView dv3 = new DataView();

    int count = 0;
    string course_id = string.Empty;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    string strbatch = string.Empty;
    string strbranch = string.Empty;
    static string grouporusercode = "";

    ArrayList alv = new ArrayList();
    Hashtable hashmark = new Hashtable();

    bool check = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblerror.Visible = false;
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
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            string collegecode1 = Session["collegecode"].ToString();
            if (!IsPostBack)
            {
                loadcollege();
                collegecode = ddlcollege.SelectedValue.ToString();
                BindBatch();
                BindDegree(singleuser, group_user, collegecode, usercode);
                if (txtdegree.Enabled == true)
                {
                    txtdegree.Enabled = true;
                    txtbranch.Enabled = true;
                    BindBranch(singleuser, group_user, course_id, collegecode, usercode);
                    BindSectransport(strbatch, strbranch);
                }
                else
                {
                    txtdegree.Enabled = false;
                    txtbranch.Enabled = false;
                }

                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";

                string grouporusercode = "";

                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                string Master = "select * from Master_Settings where " + grouporusercode + "";
                DataSet ds = d2.select_method(Master, hat, "Text");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Studflag"] = "1";
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void loadcollege()
    {
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
            columnfield = " and user_code='" + Session["usercode"] + "'";
        }
        hat.Clear();
        hat.Add("column_field", columnfield.ToString());
        ds = da.select_method("bind_college", hat, "sp");
        ddlcollege.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlcollege.Enabled = true;
            ddlcollege.DataSource = ds;
            ddlcollege.DataTextField = "collname";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();
        }
    }
    public void BindBatch()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();
            string strsql = "select distinct batch_year from applyn where batch_year<>'-1' and batch_year<>'' order by batch_year asc";
            //ds2 = d2.BindBatch();
            ds2 = d2.select_method_wo_parameter(strsql, "Text");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklsbatch.DataSource = ds2;
                chklsbatch.DataTextField = "Batch_year";
                chklsbatch.DataValueField = "Batch_year";
                chklsbatch.DataBind();
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    if (chklsbatch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklsbatch.Items.Count == count)
                    {
                        chkbatch.Checked = true;
                    }
                }
            }


        }
        catch (Exception ex)
        {
            //imgAlert.Visible = true;
            //lbl_alert.Text = ex.ToString();
        }
    }

    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            count = 0;

            chklstdegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstdegree.DataSource = ds2;
                chklstdegree.DataTextField = "course_name";
                chklstdegree.DataValueField = "course_id";
                chklstdegree.DataBind();
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    if (chklstdegree.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstdegree.Items.Count == count)
                    {
                        chkdegree.Checked = true;
                    }
                }
                txtdegree.Enabled = true;
            }
            else
            {
                txtdegree.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            //imgAlert.Visible = true;
            //lbl_alert.Text = ex.ToString();
        }

    }

    public void BindBranch(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            count = 0;

            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                }
            }
            chklstbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstbranch.DataSource = ds2;
                chklstbranch.DataTextField = "dept_name";
                chklstbranch.DataValueField = "degree_code";
                chklstbranch.DataBind();
                chklstbranch.Items[0].Selected = true;
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                    if (chklstbranch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstbranch.Items.Count == count)
                    {
                        chkbranch.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //imgAlert.Visible = true;
            //lbl_alert.Text = ex.ToString();
        }

    }

    public void BindSectransport(string strbatch, string strbranch)
    {
        try
        {
            count = 0;

            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    if (strbatch == "")
                    {
                        strbatch = "'" + chklsbatch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbatch = strbatch + "," + "'" + chklsbatch.Items[i].Value.ToString() + "'";
                    }
                }
            }

            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    if (strbranch == "")
                    {
                        strbranch = "'" + chklstbranch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbranch = strbranch + "," + "'" + chklstbranch.Items[i].Value.ToString() + "'";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //imgAlert.Visible = true;
            //lbl_alert.Text = ex.ToString();
        }
    }

    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            count = 0;

            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                }
            }
            chklstbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstbranch.DataSource = ds2;
                chklstbranch.DataTextField = "dept_name";
                chklstbranch.DataValueField = "degree_code";
                chklstbranch.DataBind();
                chklstbranch.Items[0].Selected = true;
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                    if (chklstbranch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstbranch.Items.Count == count)
                    {
                        chkbranch.Checked = true;
                    }
                }
            }
        }

        catch (Exception ex)
        {
            //imgAlert.Visible = true;
            //lbl_alert.Text = ex.ToString();
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranch(singleuser, group_user, course_id, collegecode, usercode);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    # region Batch CheckChange -Events
    protected void chkbatch_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (chkbatch.Checked == true)
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                    txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = false;
                    txtbatch.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            imgAlert.Visible = true;
            lbl_alert.Text = ex.ToString();
        }
    }
    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            string clg = "";
            int commcount = 0;
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtbatch.Text = "Batch(" + commcount.ToString() + ")";
                    if (clg == "")
                    {
                        clg = chklsbatch.Items[i].Value.ToString();
                    }
                    else
                    {
                        clg = clg + "','" + chklsbatch.Items[i].Value;
                    }
                }
            }

            if (commcount == 0)
            {
                txtbatch.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            imgAlert.Visible = true;
            lbl_alert.Text = ex.ToString();
        }

    }
    #endregion

    # region Degree CheckChange -Events
    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkdegree.Checked == true)
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                    txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = false;
                    txtdegree.Text = "---Select---";
                }
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
        catch (Exception ex)
        {
            imgAlert.Visible = true;
            lbl_alert.Text = ex.ToString();
        }
    }
    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string clg = "";
            int commcount = 0;
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtdegree.Text = "Degree(" + commcount.ToString() + ")";
                    if (clg == "")
                    {
                        clg = chklstdegree.Items[i].Value.ToString();
                    }
                    else
                    {
                        clg = clg + "','" + chklstdegree.Items[i].Value;
                    }
                }
            }
            if (commcount == 0)
            {
                txtbatch.Text = "--Select--";
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            chklstbranch_SelectedIndexChanged(sender, e);
        }
        catch (Exception ex)
        {
            imgAlert.Visible = true;
            lbl_alert.Text = ex.ToString();
        }
    }
    #endregion

    # region Branch CheckChange -Events
    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkbranch.Checked == true)
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                    txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = false;
                    txtbranch.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            imgAlert.Visible = true;
            lbl_alert.Text = ex.ToString();
        }
    }
    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string clg = "";
            int commcount = 0;
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtbranch.Text = "Branch(" + commcount.ToString() + ")";
                    if (clg == "")
                    {
                        clg = chklstbranch.Items[i].Value.ToString();
                    }
                    else
                    {
                        clg = clg + "','" + chklstbranch.Items[i].Value;
                    }
                }
            }

            if (commcount == 0)
            {
                txtbatch.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            imgAlert.Visible = true;
            lbl_alert.Text = ex.ToString();
        }
    }
    #endregion

    //protected void FpSpread1_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{
    //    try
    //    {
    //        string ctrlname = Page.Request.Params["__EVENTTARGET"];
    //        if (ctrlname != null && ctrlname != String.Empty)
    //        {
    //            string[] spiltspreadname = ctrlname.Split('$');
    //            if (spiltspreadname.GetUpperBound(0) > 1)
    //            {
    //                string getrowxol = spiltspreadname[3].ToString().Trim();
    //                string[] spr = getrowxol.Split(',');
    //                if (spr.GetUpperBound(0) == 1)
    //                {
    //                    int arow = Convert.ToInt32(spr[0]);
    //                    int acol = Convert.ToInt32(spr[1]);
    //                    if (arow == 0 && acol > 4)
    //                    {
    //                        string setval = e.EditValues[acol].ToString();
    //                        int setvalcel = 0;
    //                        if (setval.Trim().ToLower() == "true" || setval.Trim() == "1")
    //                        {
    //                            setvalcel = 1;
    //                        }
    //                        for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
    //                        {
    //                            FpSpread1.Sheets[0].Cells[r, acol].Value = setvalcel;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblerror.Visible = true;
    //        lblerror.Text = ex.ToString();
    //    }
    //}
    protected void FpSpread2_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        FpSpread2.SaveChanges();
        string value = Convert.ToString(FpSpread2.Sheets[0].Cells[0, 5].Value);
        if (value == "1")
        {
            for (int K = 1; K < FpSpread2.Sheets[0].Rows.Count; K++)
            {
                FpSpread2.Sheets[0].Cells[K, 5].Value = 1;
            }
        }
        else if (value == "0")
        {
            for (int K = 1; K < FpSpread2.Sheets[0].Rows.Count; K++)
            {
                FpSpread2.Sheets[0].Cells[K, 5].Value = 0;
            }
        }
    }

    protected void Buttongo_Click(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        FpSpread1.Sheets[0].AutoPostBack = true;
        FpSpread1.CommandBar.Visible = false;
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = 11;
        FpSpread1.Width = 900;


        FarPoint.Web.Spread.NamedStyle fontblue = new FarPoint.Web.Spread.NamedStyle("blue");
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
        FpSpread1.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
        FpSpread1.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        FpSpread1.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;

        FpSpread1.ActiveSheetView.DefaultStyle.Font.Name = "Book Antiqua";
        FpSpread1.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].SheetCorner.Columns[0].Visible = false;

        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        style.Font.Size = 12;
        style.Font.Bold = true;
        style.HorizontalAlign = HorizontalAlign.Center;
        style.ForeColor = Color.Black;
        FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].AllowTableCorner = true;

        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#0080ff");
        darkstyle.Font.Name = "Book Antiqua";
        darkstyle.Font.Size = 11;
        darkstyle.Font.Bold = true;
        darkstyle.Border.BorderSize = 1;
        darkstyle.HorizontalAlign = HorizontalAlign.Center;
        darkstyle.VerticalAlign = VerticalAlign.Middle;
        darkstyle.Border.BorderColor = System.Drawing.Color.Black;
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

        FpSpread1.Sheets[0].Columns[0].Locked = true;
        FpSpread1.Sheets[0].Columns[1].Locked = true;
        FpSpread1.Sheets[0].Columns[2].Locked = true;
        FpSpread1.Sheets[0].Columns[3].Locked = true;
        FpSpread1.Sheets[0].Columns[4].Locked = true;
        FpSpread1.Sheets[0].Columns[5].Locked = true;
        FpSpread1.Sheets[0].Columns[6].Locked = true;
        FpSpread1.Sheets[0].Columns[7].Locked = true;
        FpSpread1.Sheets[0].Columns[8].Locked = true;
        FpSpread1.Sheets[0].Columns[9].Locked = true;
        FpSpread1.Sheets[0].Columns[10].Locked = true;

        FpSpread1.Sheets[0].RowHeader.Visible = false;
        FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].SheetCorner.RowCount = 1;


        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Degree Details";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Total Students";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Total Selected";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Not Selected";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Print Challan";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Not Print Challan";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Paid";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Not Paid";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Downloaded Hall Ticket";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Not Downloaded Hall Ticket";

        LoadSpread();

    }

    protected void LoadSpread()
    {
        if (txtbatch.Text != "---Select---" && txtdegree.Text != "---Select---" && txtbranch.Text != "---Select---")
        {
            string batchyear = "";
            if (txtbatch.Text != "--Select--")
            {
                for (int j = 0; j < chklsbatch.Items.Count; j++)
                {
                    if (chklsbatch.Items[j].Selected == true)
                    {
                        if (batchyear == "")
                            batchyear = "'" + chklsbatch.Items[j].Value.ToString() + "'";
                        else
                            batchyear = batchyear + "," + "'" + chklsbatch.Items[j].Value.ToString() + "'";
                    }
                }
            }
            string degree = "";
            if (txtdegree.Text != "--Select--")
            {
                for (int s = 0; s < chklstdegree.Items.Count; s++)
                {
                    if (chklstdegree.Items[s].Selected == true)
                    {
                        if (degree == "")
                            degree = "'" + chklstdegree.Items[s].Value.ToString() + "'";
                        else
                            degree = degree + "," + "'" + chklstdegree.Items[s].Value.ToString() + "'";
                    }
                }
            }

            string branch = "";
            if (txtbranch.Text != "--Select--")
            {
                for (int k = 0; k < chklstbranch.Items.Count; k++)
                {
                    if (chklstbranch.Items[k].Selected == true)
                    {
                        if (branch == "")
                            branch = "'" + chklstbranch.Items[k].Value.ToString() + "'";
                        else
                            branch = branch + "," + "'" + chklstbranch.Items[k].Value.ToString() + "'";
                    }
                }
            }

            string getexamvalue = d2.GetFunction("select value from master_settings where settings='Exam year and month Valuation'");
            if (getexamvalue.Trim() != null && getexamvalue.Trim() != "" && getexamvalue.Trim() != "")
            {
                string[] spe = getexamvalue.Split(',');
                if (spe.GetUpperBound(0) == 1)
                {
                    Session["exam_month"] = spe[1].ToString();
                    Session["exam_year"] = spe[0].ToString();
                }
            }
            else
            {
                lblnorec.Text = "Exam Schedule is Not Alloted ";
                lblnorec.Visible = true;
                FpSpread1.Visible = false;
                return;
            }

            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[1].Width = 200;
            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;

            string selectquery = " select r.Batch_Year,c.Course_Name,de.dept_acronym,de.Dept_Code,d.Degree_Code,r.Current_Semester,r.Sections,r.Reg_No,r.Roll_No,r.Stud_Name from Registration r,Degree d,Course c,Department de where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.Batch_Year in(" + batchyear + ") and r.degree_code in(" + branch + ") order by r.Batch_Year,c.Course_Name,de.Dept_Code,d.Degree_Code,r.Current_Semester,r.Sections,r.Reg_No";
            selectquery = selectquery + " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status  from Exam_Details ed,exam_application ea where ed.exam_code=ea.exam_code  and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + batchyear + ") and ed.degree_code in(" + branch + ") and app_status in(1,2,3)";
            selectquery = selectquery + " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status  from Exam_Details ed,exam_application ea where ed.exam_code=ea.exam_code  and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + batchyear + ") and ed.degree_code in(" + branch + ") and app_status in(2,3)";
            selectquery = selectquery + " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status  from Exam_Details ed,exam_application ea where ed.exam_code=ea.exam_code  and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + batchyear + ") and ed.degree_code in(" + branch + ") and app_status in(3) and is_confirm=1";
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int sno = 1;
                Session["batchyear"] = batchyear;
                Session["branch"] = branch;

                string getquery = "  select c.Course_Name,d.Degree_Code,c.Course_Id from Course c,Degree d where c.Course_Id=d.Course_Id and d.degree_code in (" + branch + ")";
                ds3 = d2.select_method_wo_parameter(getquery, "Text");
                sno = 1;
                for (int j = 0; j < chklsbatch.Items.Count; j++)
                {
                    if (chklsbatch.Items[j].Selected == true)
                    {
                        string year = chklsbatch.Items[j].Value.ToString();
                        for (int cr = 0; cr < ds3.Tables[0].Rows.Count; cr++)
                        {
                            ds.Tables[0].DefaultView.RowFilter = "degree_code='" + Convert.ToString(ds3.Tables[0].Rows[cr]["degree_code"]) + "' and Batch_Year='" + year + "'";
                            dv = ds.Tables[0].DefaultView;
                            newds.Tables.Clear();
                            newds.Tables.Add(dv.ToTable());
                            int totstd = dv.Count;
                            ds.Tables[1].DefaultView.RowFilter = "degree_code='" + Convert.ToString(ds3.Tables[0].Rows[cr]["degree_code"]) + "' and Batch_Year='" + year + "'";
                            dv1 = ds.Tables[1].DefaultView;
                            int selstd = dv1.Count;
                            ds.Tables[2].DefaultView.RowFilter = "degree_code='" + Convert.ToString(ds3.Tables[0].Rows[cr]["degree_code"]) + "' and Batch_Year='" + year + "'";
                            dv2 = ds.Tables[2].DefaultView;
                            int print = dv2.Count;
                            ds.Tables[3].DefaultView.RowFilter = "degree_code='" + Convert.ToString(ds3.Tables[0].Rows[cr]["degree_code"]) + "' and Batch_Year='" + year + "'";
                            dv3 = ds.Tables[3].DefaultView;
                            int downloaded = dv3.Count;

                            string cname = ds3.Tables[0].Rows[cr]["Course_Name"].ToString();
                            if (dv.Count > 0)
                            {
                                string checkcourse = "";
                                string coursedetails = "";
                                coursedetails = "";
                                if (checkcourse != cname)
                                {
                                    checkcourse = cname;
                                    //string batch = newds.Tables[0].Rows[0]["Batch_Year"].ToString();
                                    string batch = year;
                                    string dept = newds.Tables[0].Rows[0]["dept_acronym"].ToString();
                                    string section = newds.Tables[0].Rows[0]["Sections"].ToString();
                                    if (section != "")
                                    {
                                        coursedetails = batch + " " + checkcourse + " " + dept + " " + " " + section;
                                    }
                                    else
                                    {
                                        coursedetails = batch + " " + checkcourse + " " + dept;
                                    }
                                }

                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = coursedetails;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = coursedetails;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(totstd);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = "Total Students";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(selstd);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = "Selected";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(totstd - selstd);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = "Not Selected";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(print);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Tag = "Print Challan";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(selstd - print);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Tag = "Not Print Challan";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(print);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Tag = "Paid";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(selstd - print);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Tag = "Not Paid";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(downloaded);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Tag = "Downloaded HT";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(print - downloaded);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Tag = "Not Downloaded HT";

                            }
                        }
                    }
                }

                int totalstudents = ds.Tables[0].Rows.Count;
                int selected = ds.Tables[1].Rows.Count;
                int notselected = totalstudents - selected;
                int printchallan = ds.Tables[2].Rows.Count;
                int notprintchallan = selected - printchallan;
                int paid = ds.Tables[2].Rows.Count;
                int notpaid = selected - paid;
                int downloadht = ds.Tables[3].Rows.Count;
                int notdownloadht = printchallan - downloadht;

                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Total";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(totalstudents);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = "Total Students";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(selected);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = "Selected";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(notselected);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = "Not Selected";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(printchallan);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Tag = "Print Challan";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(notprintchallan);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Tag = "Not Print Challan";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(paid);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Tag = "Paid";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(notpaid);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Tag = "Not Paid";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(downloadht);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Tag = "Downloaded HT";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(notdownloadht);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Tag = "Not Downloaded HT";
            }

            FpSpread1.Visible = true;
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();
        }
        else
        {
            FpSpread1.Visible = false;
            exceldiv.Visible = false;
            lblnorec.Visible = true;
            lblnorec.Text = "Please Select The Required Fields And Then Proceed !!!";
        }
    }

    protected void Cell1_Click(object sender, EventArgs e)
    {
        try
        {
            check = true;
        }
        catch
        {

        }

    }

    protected void FpSpread1_OnPreRender(object sender, EventArgs e)
    {
        try
        {
            if (check == true)
            {
                string activerow = "";
                string activecol = "";
                activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
                string selectedtext = "";
                string det = "";
                if (activerow.Trim() != "")
                {
                    selectedtext = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Tag);
                    det = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                }
                string batchyr = "";
                string cname = "";
                string dptacr = "";
                string sec = "";

                FpSpread2.CommandBar.Visible = false;
                FpSpread2.Sheets[0].AutoPostBack = false;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
                darkstyle.Font.Name = "Book Antiqua";
                darkstyle.Font.Size = FontUnit.Medium;
                darkstyle.Font.Bold = true;
                darkstyle.HorizontalAlign = HorizontalAlign.Center;
                darkstyle.VerticalAlign = VerticalAlign.Middle;
                FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                FpSpread2.RowHeader.Visible = false;

                FarPoint.Web.Spread.StyleInfo darkstyle1 = new FarPoint.Web.Spread.StyleInfo();
                darkstyle1.Font.Name = "Book Antiqua";
                darkstyle1.Font.Size = FontUnit.Medium;

                FpSpread2.Sheets[0].DefaultStyle = darkstyle1;
                FpSpread2.Sheets[0].RowCount = 0;
                FpSpread2.Sheets[0].ColumnCount = 6;
                FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread2.Sheets[0].ColumnHeader.Columns[0].Width = 50;
                FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Mobile No";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Select";

                FpSpread2.Sheets[0].Columns[1].Width = 100;
                FpSpread2.Sheets[0].Columns[2].Width = 100;
                FpSpread2.Sheets[0].Columns[3].Width = 200;
                FpSpread2.Sheets[0].Columns[4].Width = 130;
                FpSpread2.Sheets[0].Columns[5].Width = 75;

                if (det.Trim() != "" && det.Trim() != "Total")
                {
                    string[] dsplit = det.Split(' ');
                    batchyr = dsplit[0];
                    cname = dsplit[1];
                    dptacr = dsplit[2];

                    if (selectedtext == "Total Students")
                    {
                        //string selectquery = " select  c.Course_Name,de.Dept_Name,r.Reg_No,r.Roll_No,r.Stud_Name from Registration r,Degree d,Course c,Department de where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.Batch_Year in(" + Session["batchyear"].ToString() + ")  and r.degree_code in(" + Session["branch"].ToString() + ") order by r.Batch_Year,c.Course_Name,de.Dept_Code,d.Degree_Code,r.Current_Semester,r.Sections,r.Reg_No";
                        string selectquery = "select r.Batch_Year,d.Degree_Code,c.Course_Name,de.dept_acronym,de.Dept_Name,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name from applyn a, Registration r,Degree d,Course c,Department de where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.Batch_Year in(" + Session["batchyear"].ToString() + ")  and r.degree_code in(" + Session["branch"].ToString() + ") order by r.Batch_Year,c.Course_Name,de.Dept_Code,d.Degree_Code,r.Current_Semester,r.Sections,r.Reg_No";

                        //selectquery = selectquery + " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status  from Exam_Details ed,exam_application ea where ed.exam_code=ea.exam_code  and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + Session["batchyear"].ToString() + ")  and ed.degree_code in(" + Session["branch"].ToString() + ") and app_status in(1,2,3)";
                        //selectquery = selectquery + " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status  from Exam_Details ed,exam_application ea where ed.exam_code=ea.exam_code  and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + Session["batchyear"].ToString() + ")  and ed.degree_code in(" + Session["branch"].ToString() + ") and app_status in(2,3)";
                        //selectquery = selectquery + " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status  from Exam_Details ed,exam_application ea where ed.exam_code=ea.exam_code  and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + Session["batchyear"].ToString() + ")  and ed.degree_code in(" + Session["branch"].ToString() + ") and app_status in(3) and is_confirm=1";
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string dgrname = "";
                            string dptname = "";
                            int sno = 1;
                            string getquery = "  select c.Course_Name,d.Degree_Code,c.Course_Id from Course c,Degree d where c.Course_Id=d.Course_Id and d.degree_code in (" + Session["branch"].ToString() + ")";
                            ds3 = d2.select_method_wo_parameter(getquery, "Text");

                            //for (int j = 0; j < chklsbatch.Items.Count; j++)
                            //{
                            //    if (chklsbatch.Items[j].Selected == true)
                            //    {
                            //        string year = chklsbatch.Items[j].Value.ToString();
                            //for (int cr = 0; cr < ds3.Tables[0].Rows.Count; cr++)
                            //{
                            ds.Tables[0].DefaultView.RowFilter = "dept_acronym='" + dptacr + "' and Batch_Year='" + batchyr + "' and Course_Name ='" + cname + "'";

                            dv = ds.Tables[0].DefaultView;
                            newds.Tables.Clear();
                            newds.Tables.Add(dv.ToTable());

                            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                            chkall.AutoPostBack = true;
                            FarPoint.Web.Spread.CheckBoxCellType chkall1 = new FarPoint.Web.Spread.CheckBoxCellType();
                            chkall1.AutoPostBack = false;
                            FpSpread2.Sheets[0].RowCount++;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;
                            for (int sk = 0; sk < newds.Tables[0].Rows.Count; sk++)
                            {
                                FpSpread2.Sheets[0].RowCount++;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = newds.Tables[0].Rows[sk]["Roll_No"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = newds.Tables[0].Rows[sk]["Reg_No"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = newds.Tables[0].Rows[sk]["Stud_Name"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = newds.Tables[0].Rows[sk]["Student_Mobile"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall1;

                            }
                            //for (int sk = 0; sk < ds.Tables[0].Rows.Count; sk++)
                            //{
                            //    FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                            //    chkall.AutoPostBack = false;
                            //    if (dgrname != ds.Tables[0].Rows[sk]["Course_Name"].ToString() || dptname != ds.Tables[0].Rows[sk]["Dept_Name"].ToString())
                            //    {
                            //        FpSpread2.Sheets[0].RowCount++;
                            //        dgrname = ds.Tables[0].Rows[sk]["Course_Name"].ToString();
                            //        dptname = ds.Tables[0].Rows[sk]["Dept_Name"].ToString();
                            //        if (dgrname.Trim() != "" && dptname.Trim() != "")
                            //        {
                            //            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);
                            //            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = dgrname;
                            //            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = dptname;
                            //            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].CellType = chkall;
                            //            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            //        }
                            //    }
                            //}
                            //}
                            // }
                            //}

                        }
                    }
                    else if (selectedtext == "Selected")
                    {
                        int sno = 1;
                        string dptcode = d2.GetFunction("select Dept_Code from Department where dept_acronym='" + dptacr + "'");
                        if (dptcode.Trim() != "")
                        {
                            string degreecd = d2.GetFunction("select Degree_Code from Degree where Dept_Code='" + dptcode + "'");
                            if (degreecd.Trim() != "")
                            {
                                string selectquery = " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name  from Exam_Details ed,exam_application ea,Registration r,applyn a where ea.exam_code=ed.exam_code and ed.degree_code=r.degree_code and ed.Batch_Year=r.Batch_Year and r.app_no=a.app_no and r.roll_no=ea.roll_no and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + batchyr + ")  and ed.degree_code in(" + degreecd + ") and app_status in(1,2,3)";
                                ds = d2.select_method_wo_parameter(selectquery, "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                                    chkall.AutoPostBack = false;
                                    FpSpread2.Sheets[0].RowCount++;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;
                                    for (int sk = 0; sk < ds.Tables[0].Rows.Count; sk++)
                                    {
                                        FpSpread2.Sheets[0].RowCount++;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[sk]["Roll_No"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[sk]["Reg_No"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[sk]["Stud_Name"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[sk]["Student_Mobile"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;
                                    }
                                }
                                else
                                {
                                    imgAlert.Visible = true;
                                    lbl_alert.Text = "No Records Found";
                                    FpSpread2.Visible = false;
                                    return;
                                }
                            }
                        }
                    }
                    else if (selectedtext == "Not Selected")
                    {
                        int sno = 1;
                        string dptcode = d2.GetFunction("select Dept_Code from Department where dept_acronym='" + dptacr + "'");
                        if (dptcode.Trim() != "")
                        {
                            string degreecd = d2.GetFunction("select Degree_Code from Degree where Dept_Code='" + dptcode + "'");
                            if (degreecd.Trim() != "")
                            {
                                string rno = "";
                                string getquery = " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name  from Exam_Details ed,exam_application ea,Registration r,applyn a where ea.exam_code=ed.exam_code and ed.degree_code=r.degree_code and ed.Batch_Year=r.Batch_Year and r.app_no=a.app_no and r.roll_no=ea.roll_no and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + batchyr + ")  and ed.degree_code in(" + degreecd + ") and app_status in(1,2,3)";
                                ds1 = d2.select_method_wo_parameter(getquery, "Text");
                                if (ds1.Tables[0].Rows.Count > 0)
                                {
                                    for (int rn = 0; rn < ds1.Tables[0].Rows.Count; rn++)
                                    {
                                        if (rno == "")
                                        {
                                            rno = "'" + ds1.Tables[0].Rows[rn]["Roll_No"].ToString() + "'";
                                        }
                                        else
                                        {
                                            rno = rno + "," + "'" + ds1.Tables[0].Rows[rn]["Roll_No"].ToString() + "'";
                                        }
                                    }
                                }
                                string selectquery = "";
                                if (rno.Trim() != "")
                                {
                                    selectquery = " select r.Batch_Year,d.Degree_Code,c.Course_Name,de.dept_acronym,de.Dept_Name,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name from applyn a, Registration r,Degree d,Course c,Department de where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.Batch_Year in(" + batchyr + ")  and r.degree_code in(" + degreecd + ") and r.roll_no not in(" + rno + ") order by r.Batch_Year,c.Course_Name,de.Dept_Code,d.Degree_Code,r.Current_Semester,r.Sections,r.Reg_No";
                                }
                                else
                                {
                                    selectquery = " select r.Batch_Year,d.Degree_Code,c.Course_Name,de.dept_acronym,de.Dept_Name,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name from applyn a, Registration r,Degree d,Course c,Department de where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.Batch_Year in(" + batchyr + ")  and r.degree_code in(" + degreecd + ")  order by r.Batch_Year,c.Course_Name,de.Dept_Code,d.Degree_Code,r.Current_Semester,r.Sections,r.Reg_No";
                                }
                                ds = d2.select_method_wo_parameter(selectquery, "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                                    chkall.AutoPostBack = false;
                                    FpSpread2.Sheets[0].RowCount++;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;
                                    for (int sk = 0; sk < ds.Tables[0].Rows.Count; sk++)
                                    {
                                        FpSpread2.Sheets[0].RowCount++;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[sk]["Roll_No"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[sk]["Reg_No"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[sk]["Stud_Name"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[sk]["Student_Mobile"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;

                                    }
                                }
                                else
                                {
                                    imgAlert.Visible = true;
                                    lbl_alert.Text = "No Records Found";
                                    FpSpread2.Visible = false;
                                    return;
                                }
                            }
                        }
                    }
                    else if (selectedtext == "Print Challan" || selectedtext == "Paid")
                    {
                        int sno = 1;
                        string dptcode = d2.GetFunction("select Dept_Code from Department where dept_acronym='" + dptacr + "'");
                        if (dptcode.Trim() != "")
                        {
                            string degreecd = d2.GetFunction("select Degree_Code from Degree where Dept_Code='" + dptcode + "'");
                            if (degreecd.Trim() != "")
                            {
                                string selectquery = " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name  from Exam_Details ed,exam_application ea,Registration r,applyn a where ea.exam_code=ed.exam_code and ed.degree_code=r.degree_code and ed.Batch_Year=r.Batch_Year and r.app_no=a.app_no and r.roll_no=ea.roll_no and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + batchyr + ")  and ed.degree_code in(" + degreecd + ") and app_status in(2,3)";
                                ds = d2.select_method_wo_parameter(selectquery, "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                                    chkall.AutoPostBack = false;
                                    FpSpread2.Sheets[0].RowCount++;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;
                                    for (int sk = 0; sk < ds.Tables[0].Rows.Count; sk++)
                                    {
                                        FpSpread2.Sheets[0].RowCount++;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[sk]["Roll_No"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[sk]["Reg_No"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[sk]["Stud_Name"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[sk]["Student_Mobile"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;

                                    }
                                }
                                else
                                {
                                    imgAlert.Visible = true;
                                    lbl_alert.Text = "No Records Found";
                                    FpSpread2.Visible = false;
                                    return;
                                }
                            }
                        }
                    }
                    else if (selectedtext == "Not Print Challan" || selectedtext == "Not Paid")
                    {
                        int sno = 1;
                        string dptcode = d2.GetFunction("select Dept_Code from Department where dept_acronym='" + dptacr + "'");
                        if (dptcode.Trim() != "")
                        {
                            string degreecd = d2.GetFunction("select Degree_Code from Degree where Dept_Code='" + dptcode + "'");
                            if (degreecd.Trim() != "")
                            {
                                string rno = "";
                                string getquery = " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name  from Exam_Details ed,exam_application ea,Registration r,applyn a where ea.exam_code=ed.exam_code and ed.degree_code=r.degree_code and ed.Batch_Year=r.Batch_Year and r.app_no=a.app_no and r.roll_no=ea.roll_no and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + batchyr + ")  and ed.degree_code in(" + degreecd + ") and app_status in(2,3)";
                                ds1 = d2.select_method_wo_parameter(getquery, "Text");
                                if (ds1.Tables[0].Rows.Count > 0)
                                {
                                    for (int rn = 0; rn < ds1.Tables[0].Rows.Count; rn++)
                                    {
                                        if (rno == "")
                                        {
                                            rno = "'" + ds1.Tables[0].Rows[rn]["Roll_No"].ToString() + "'";
                                        }
                                        else
                                        {
                                            rno = rno + "," + "'" + ds1.Tables[0].Rows[rn]["Roll_No"].ToString() + "'";
                                        }
                                    }
                                }
                                string selectquery = "";
                                if (rno.Trim() != "")
                                {
                                    selectquery = " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name  from Exam_Details ed,exam_application ea,Registration r,applyn a where ea.exam_code=ed.exam_code and ed.degree_code=r.degree_code and ed.Batch_Year=r.Batch_Year and r.app_no=a.app_no and r.roll_no=ea.roll_no and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + batchyr + ")  and ed.degree_code in(" + degreecd + ") and app_status in(1,2,3) and r.roll_no not in(" + rno + ")";
                                }
                                else
                                {
                                    selectquery = " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name  from Exam_Details ed,exam_application ea,Registration r,applyn a where ea.exam_code=ed.exam_code and ed.degree_code=r.degree_code and ed.Batch_Year=r.Batch_Year and r.app_no=a.app_no and r.roll_no=ea.roll_no and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + batchyr + ")  and ed.degree_code in(" + degreecd + ") and app_status in(1,2,3) ";
                                }
                                ds = d2.select_method_wo_parameter(selectquery, "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                                    chkall.AutoPostBack = false;
                                    FpSpread2.Sheets[0].RowCount++;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;
                                    for (int sk = 0; sk < ds.Tables[0].Rows.Count; sk++)
                                    {
                                        FpSpread2.Sheets[0].RowCount++;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[sk]["Roll_No"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[sk]["Reg_No"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[sk]["Stud_Name"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[sk]["Student_Mobile"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;

                                    }
                                }
                                else
                                {
                                    imgAlert.Visible = true;
                                    lbl_alert.Text = "No Records Found";
                                    FpSpread2.Visible = false;
                                    return;
                                }
                            }
                        }
                    }
                    //else if (selectedtext == "Paid")
                    //{

                    //}
                    //else if (selectedtext == "Not Paid")
                    //{

                    //}
                    else if (selectedtext == "Downloaded HT")
                    {
                        int sno = 1;
                        string dptcode = d2.GetFunction("select Dept_Code from Department where dept_acronym='" + dptacr + "'");
                        if (dptcode.Trim() != "")
                        {
                            string degreecd = d2.GetFunction("select Degree_Code from Degree where Dept_Code='" + dptcode + "'");
                            if (degreecd.Trim() != "")
                            {
                                string selectquery = " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name  from Exam_Details ed,exam_application ea,Registration r,applyn a where ea.exam_code=ed.exam_code and ed.degree_code=r.degree_code and ed.Batch_Year=r.Batch_Year and r.app_no=a.app_no and r.roll_no=ea.roll_no and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + batchyr + ")  and ed.degree_code in(" + degreecd + ") and app_status in(3) and is_confirm=1";
                                ds = d2.select_method_wo_parameter(selectquery, "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                                    chkall.AutoPostBack = false;
                                    FpSpread2.Sheets[0].RowCount++;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;
                                    for (int sk = 0; sk < ds.Tables[0].Rows.Count; sk++)
                                    {
                                        FpSpread2.Sheets[0].RowCount++;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[sk]["Roll_No"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[sk]["Reg_No"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[sk]["Stud_Name"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[sk]["Student_Mobile"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;

                                    }
                                }
                                else
                                {
                                    imgAlert.Visible = true;
                                    lbl_alert.Text = "No Records Found";
                                    FpSpread2.Visible = false;
                                    return;
                                }
                            }
                        }
                    }
                    else if (selectedtext == "Not Downloaded HT")
                    {
                        int sno = 1;
                        string dptcode = d2.GetFunction("select Dept_Code from Department where dept_acronym='" + dptacr + "'");
                        if (dptcode.Trim() != "")
                        {
                            string degreecd = d2.GetFunction("select Degree_Code from Degree where Dept_Code='" + dptcode + "'");
                            string selectquery = "";
                            if (degreecd.Trim() != "")
                            {
                                string rno = "";
                                string getquery = " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name  from Exam_Details ed,exam_application ea,Registration r,applyn a where ea.exam_code=ed.exam_code and ed.degree_code=r.degree_code and ed.Batch_Year=r.Batch_Year and r.app_no=a.app_no and r.roll_no=ea.roll_no and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + batchyr + ")  and ed.degree_code in(" + degreecd + ") and app_status in(3) and is_confirm=1";
                                ds1 = d2.select_method_wo_parameter(getquery, "Text");
                                if (ds1.Tables[0].Rows.Count > 0)
                                {
                                    for (int rn = 0; rn < ds1.Tables[0].Rows.Count; rn++)
                                    {
                                        if (rno == "")
                                        {
                                            rno = "'" + ds1.Tables[0].Rows[rn]["Roll_No"].ToString() + "'";
                                        }
                                        else
                                        {
                                            rno = rno + "," + "'" + ds1.Tables[0].Rows[rn]["Roll_No"].ToString() + "'";
                                        }
                                    }
                                    if (rno.Trim() != "")
                                    {
                                        selectquery = " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name  from Exam_Details ed,exam_application ea,Registration r,applyn a where ea.exam_code=ed.exam_code and ed.degree_code=r.degree_code and ed.Batch_Year=r.Batch_Year and r.app_no=a.app_no and r.roll_no=ea.roll_no and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + batchyr + ")  and ed.degree_code in(" + degreecd + ") and app_status in(2,3) and r.roll_no not in(" + rno + ") ";
                                    }
                                    else
                                    {
                                        selectquery = " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name  from Exam_Details ed,exam_application ea,Registration r,applyn a where ea.exam_code=ed.exam_code and ed.degree_code=r.degree_code and ed.Batch_Year=r.Batch_Year and r.app_no=a.app_no and r.roll_no=ea.roll_no and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + batchyr + ")  and ed.degree_code in(" + degreecd + ") ";
                                    }
                                }
                                else
                                {
                                    selectquery = " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name  from Exam_Details ed,exam_application ea,Registration r,applyn a where ea.exam_code=ed.exam_code and ed.degree_code=r.degree_code and ed.Batch_Year=r.Batch_Year and r.app_no=a.app_no and r.roll_no=ea.roll_no and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + batchyr + ")  and ed.degree_code in(" + degreecd + ") and app_status in(2,3)";
                                }
                                ds = d2.select_method_wo_parameter(selectquery, "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {

                                    FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                                    chkall.AutoPostBack = false;
                                    FpSpread2.Sheets[0].RowCount++;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;
                                    for (int sk = 0; sk < ds.Tables[0].Rows.Count; sk++)
                                    {
                                        FpSpread2.Sheets[0].RowCount++;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[sk]["Roll_No"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[sk]["Reg_No"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[sk]["Stud_Name"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[sk]["Student_Mobile"].ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;

                                    }
                                }
                                else
                                {
                                    imgAlert.Visible = true;
                                    lbl_alert.Text = "No Records Found";
                                    FpSpread2.Visible = false;
                                    return;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (selectedtext == "Total Students")
                    {
                        string selectquery = "select r.Batch_Year,d.Degree_Code,c.Course_Name,de.dept_acronym,de.Dept_Name,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name from applyn a, Registration r,Degree d,Course c,Department de where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.Batch_Year in(" + Session["batchyear"].ToString() + ")  and r.degree_code in(" + Session["branch"].ToString() + ") order by r.Batch_Year,c.Course_Name,de.Dept_Code,d.Degree_Code,r.Current_Semester,r.Sections,r.Reg_No";
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string dgrname = "";
                            string dptname = "";
                            int sno = 1;
                            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                            FpSpread2.Sheets[0].RowCount++;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;
                            chkall.AutoPostBack = false;
                            for (int sk = 0; sk < ds.Tables[0].Rows.Count; sk++)
                            {
                                FpSpread2.Sheets[0].RowCount++;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[sk]["Roll_No"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[sk]["Reg_No"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[sk]["Stud_Name"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[sk]["Student_Mobile"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;

                            }
                        }
                    }
                    else if (selectedtext == "Selected")
                    {
                        int sno = 1;
                        string selectquery = " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name  from Exam_Details ed,exam_application ea,Registration r,applyn a where ea.exam_code=ed.exam_code and ed.degree_code=r.degree_code and ed.Batch_Year=r.Batch_Year and r.app_no=a.app_no and r.roll_no=ea.roll_no and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + Session["batchyear"].ToString() + ")  and ed.degree_code in(" + Session["branch"].ToString() + ") and app_status in(1,2,3)";
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                            chkall.AutoPostBack = false;
                            FpSpread2.Sheets[0].RowCount++;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;
                            string chkrno = "";
                            for (int sk = 0; sk < ds.Tables[0].Rows.Count; sk++)
                            {
                                FpSpread2.Sheets[0].RowCount++;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[sk]["Roll_No"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[sk]["Reg_No"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[sk]["Stud_Name"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[sk]["Student_Mobile"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;
                                if (chkrno == "")
                                {
                                    chkrno = ds.Tables[0].Rows[sk]["Roll_No"].ToString();
                                }
                                else
                                {
                                    chkrno = chkrno + "," + ds.Tables[0].Rows[sk]["Roll_No"].ToString();
                                }

                            }
                        }
                        else
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = "No Records Found";
                            FpSpread2.Visible = false;
                            return;
                        }
                    }
                    else if (selectedtext == "Not Selected")
                    {
                        int sno = 1;
                        string rno = "";
                        string getquery = " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name  from Exam_Details ed,exam_application ea,Registration r,applyn a where ea.exam_code=ed.exam_code and ed.degree_code=r.degree_code and ed.Batch_Year=r.Batch_Year and r.app_no=a.app_no and r.roll_no=ea.roll_no and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + Session["batchyear"].ToString() + ")  and ed.degree_code in(" + Session["branch"].ToString() + ") and app_status in(1,2,3)";
                        ds1 = d2.select_method_wo_parameter(getquery, "Text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            for (int rn = 0; rn < ds1.Tables[0].Rows.Count; rn++)
                            {
                                if (rno == "")
                                {
                                    rno = "'" + ds1.Tables[0].Rows[rn]["Roll_No"].ToString() + "'";
                                }
                                else
                                {
                                    rno = rno + "," + "'" + ds1.Tables[0].Rows[rn]["Roll_No"].ToString() + "'";
                                }
                            }
                        }

                        string selectquery = "";
                        if (rno.Trim() != "")
                        {
                            selectquery = " select r.Batch_Year,d.Degree_Code,c.Course_Name,de.dept_acronym,de.Dept_Name,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name from applyn a, Registration r,Degree d,Course c,Department de where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.Batch_Year in(" + Session["batchyear"].ToString() + ")  and r.degree_code in(" + Session["branch"].ToString() + ") and r.roll_no not in(" + rno + ") order by r.Batch_Year,c.Course_Name,de.Dept_Code,d.Degree_Code,r.Current_Semester,r.Sections,r.Reg_No";
                        }
                        else
                        {
                            selectquery = " select r.Batch_Year,d.Degree_Code,c.Course_Name,de.dept_acronym,de.Dept_Name,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name from applyn a, Registration r,Degree d,Course c,Department de where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.Batch_Year in(" + Session["batchyear"].ToString() + ")  and r.degree_code in(" + Session["branch"].ToString() + ")  order by r.Batch_Year,c.Course_Name,de.Dept_Code,d.Degree_Code,r.Current_Semester,r.Sections,r.Reg_No";
                        }
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                            chkall.AutoPostBack = false;
                            FpSpread2.Sheets[0].RowCount++;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;
                            for (int sk = 0; sk < ds.Tables[0].Rows.Count; sk++)
                            {
                                FpSpread2.Sheets[0].RowCount++;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[sk]["Roll_No"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[sk]["Reg_No"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[sk]["Stud_Name"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[sk]["Student_Mobile"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;

                            }
                        }
                        else
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = "No Records Found";
                            FpSpread2.Visible = false;
                            return;
                        }
                    }
                    else if (selectedtext == "Print Challan" || selectedtext == "Paid")
                    {
                        int sno = 1;

                        string selectquery = " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name  from Exam_Details ed,exam_application ea,Registration r,applyn a where ea.exam_code=ed.exam_code and ed.degree_code=r.degree_code and ed.Batch_Year=r.Batch_Year and r.app_no=a.app_no and r.roll_no=ea.roll_no and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + Session["batchyear"].ToString() + ")  and ed.degree_code in(" + Session["branch"].ToString() + ") and app_status in(2,3)";
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                            chkall.AutoPostBack = false;
                            FpSpread2.Sheets[0].RowCount++;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;
                            for (int sk = 0; sk < ds.Tables[0].Rows.Count; sk++)
                            {
                                FpSpread2.Sheets[0].RowCount++;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[sk]["Roll_No"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[sk]["Reg_No"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[sk]["Stud_Name"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[sk]["Student_Mobile"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;

                            }
                        }
                        else
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = "No Records Found";
                            FpSpread2.Visible = false;
                            return;
                        }
                    }
                    else if (selectedtext == "Not Print Challan" || selectedtext == "Not Paid")
                    {
                        int sno = 1;

                        string rno = "";
                        string getquery = " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name  from Exam_Details ed,exam_application ea,Registration r,applyn a where ea.exam_code=ed.exam_code and ed.degree_code=r.degree_code and ed.Batch_Year=r.Batch_Year and r.app_no=a.app_no and r.roll_no=ea.roll_no and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + Session["batchyear"].ToString() + ")  and ed.degree_code in(" + Session["branch"].ToString() + ") and app_status in(2,3)";
                        ds1 = d2.select_method_wo_parameter(getquery, "Text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            for (int rn = 0; rn < ds1.Tables[0].Rows.Count; rn++)
                            {
                                if (rno == "")
                                {
                                    rno = "'" + ds1.Tables[0].Rows[rn]["Roll_No"].ToString() + "'";
                                }
                                else
                                {
                                    rno = rno + "," + "'" + ds1.Tables[0].Rows[rn]["Roll_No"].ToString() + "'";
                                }
                            }
                        }
                        string selectquery = "";
                        if (rno.Trim() != "")
                        {
                            selectquery = " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name  from Exam_Details ed,exam_application ea,Registration r,applyn a where ea.exam_code=ed.exam_code and ed.degree_code=r.degree_code and ed.Batch_Year=r.Batch_Year and r.app_no=a.app_no and r.roll_no=ea.roll_no and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + Session["batchyear"].ToString() + ")  and ed.degree_code in(" + Session["branch"].ToString() + ") and app_status in(1,2,3) and r.roll_no not in(" + rno + ")";
                        }
                        else
                        {
                            selectquery = " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name  from Exam_Details ed,exam_application ea,Registration r,applyn a where ea.exam_code=ed.exam_code and ed.degree_code=r.degree_code and ed.Batch_Year=r.Batch_Year and r.app_no=a.app_no and r.roll_no=ea.roll_no and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + Session["batchyear"].ToString() + ")  and ed.degree_code in(" + Session["branch"].ToString() + ") and app_status in(1,2,3) ";
                        }
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                            chkall.AutoPostBack = false;
                            FpSpread2.Sheets[0].RowCount++;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;
                            for (int sk = 0; sk < ds.Tables[0].Rows.Count; sk++)
                            {
                                FpSpread2.Sheets[0].RowCount++;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[sk]["Roll_No"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[sk]["Reg_No"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[sk]["Stud_Name"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[sk]["Student_Mobile"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;

                            }
                        }
                        else
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = "No Records Found";
                            FpSpread2.Visible = false;
                            return;
                        }

                    }
                    //else if (selectedtext == "Paid")
                    //{

                    //}
                    //else if (selectedtext == "Not Paid")
                    //{

                    //}
                    else if (selectedtext == "Downloaded HT")
                    {
                        int sno = 1;

                        string selectquery = " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name  from Exam_Details ed,exam_application ea,Registration r,applyn a where ea.exam_code=ed.exam_code and ed.degree_code=r.degree_code and ed.Batch_Year=r.Batch_Year and r.app_no=a.app_no and r.roll_no=ea.roll_no and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + Session["batchyear"].ToString() + ")  and ed.degree_code in(" + Session["branch"].ToString() + ") and app_status in(3) and is_confirm=1";
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                            chkall.AutoPostBack = false;
                            FpSpread2.Sheets[0].RowCount++;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;
                            for (int sk = 0; sk < ds.Tables[0].Rows.Count; sk++)
                            {
                                FpSpread2.Sheets[0].RowCount++;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[sk]["Roll_No"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[sk]["Reg_No"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[sk]["Stud_Name"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[sk]["Student_Mobile"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;

                            }
                        }
                        else
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = "No Records Found";
                            FpSpread2.Visible = false;
                            return;
                        }

                    }
                    else if (selectedtext == "Not Downloaded HT")
                    {
                        int sno = 1;
                        string rno = "";
                        string selectquery = "";
                        string getquery = " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name  from Exam_Details ed,exam_application ea,Registration r,applyn a where ea.exam_code=ed.exam_code and ed.degree_code=r.degree_code and ed.Batch_Year=r.Batch_Year and r.app_no=a.app_no and r.roll_no=ea.roll_no and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + Session["batchyear"].ToString() + ")  and ed.degree_code in(" + Session["branch"].ToString() + ") and app_status in(3) and is_confirm=1";
                        ds1 = d2.select_method_wo_parameter(getquery, "Text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            for (int rn = 0; rn < ds1.Tables[0].Rows.Count; rn++)
                            {
                                if (rno == "")
                                {
                                    rno = "'" + ds1.Tables[0].Rows[rn]["Roll_No"].ToString() + "'";
                                }
                                else
                                {
                                    rno = rno + "," + "'" + ds1.Tables[0].Rows[rn]["Roll_No"].ToString() + "'";
                                }
                            }
                            if (rno.Trim() != "")
                            {
                                selectquery = " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name  from Exam_Details ed,exam_application ea,Registration r,applyn a where ea.exam_code=ed.exam_code and ed.degree_code=r.degree_code and ed.Batch_Year=r.Batch_Year and r.app_no=a.app_no and r.roll_no=ea.roll_no and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + Session["batchyear"].ToString() + ")  and ed.degree_code in(" + Session["branch"].ToString() + ") and app_status in(2,3) and r.roll_no not in(" + rno + ") ";
                            }
                            else
                            {
                                selectquery = " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name  from Exam_Details ed,exam_application ea,Registration r,applyn a where ea.exam_code=ed.exam_code and ed.degree_code=r.degree_code and ed.Batch_Year=r.Batch_Year and r.app_no=a.app_no and r.roll_no=ea.roll_no and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + Session["batchyear"].ToString() + ")  and ed.degree_code in(" + Session["branch"].ToString() + ") ";
                            }
                        }
                        else
                        {
                            selectquery = " select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,ea.is_confirm,ea.app_status,Student_Mobile,r.Reg_No,r.Roll_No,r.Stud_Name  from Exam_Details ed,exam_application ea,Registration r,applyn a where ea.exam_code=ed.exam_code and ed.degree_code=r.degree_code and ed.Batch_Year=r.Batch_Year and r.app_no=a.app_no and r.roll_no=ea.roll_no and ed.Exam_Month='" + Session["exam_month"].ToString() + "' and ed.Exam_year='" + Session["exam_year"].ToString() + "' and ed.Batch_Year in(" + Session["batchyear"].ToString() + ")  and ed.degree_code in(" + Session["branch"].ToString() + ") and app_status in(2,3)";
                        }
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                            chkall.AutoPostBack = false;
                            FpSpread2.Sheets[0].RowCount++;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;
                            for (int sk = 0; sk < ds.Tables[0].Rows.Count; sk++)
                            {
                                FpSpread2.Sheets[0].RowCount++;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[sk]["Roll_No"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[sk]["Reg_No"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[sk]["Stud_Name"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[sk]["Student_Mobile"].ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkall;

                            }
                        }
                        else
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = "No Records Found";
                            FpSpread2.Visible = false;
                            return;
                        }
                    }
                }
                div_Add.Visible = true;
                FpSpread2.Visible = true;
                FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                FpSpread2.SaveChanges();
                txtmsg.Visible = true;
                btn_sms.Visible = true;
            }

        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public bool checkedcOK()
    {
        bool Ok = false;
        FpSpread2.SaveChanges();
        for (int i = 0; i < FpSpread2.Sheets[0].Rows.Count; i++)
        {
            byte check = Convert.ToByte(FpSpread2.Sheets[0].Cells[i, 5].Value);
            if (check == 1)
            {
                Ok = true;
            }
        }
        return Ok;
    }

    protected void btn_sms_Click(object sender, EventArgs e)
    {
        try
        {
            if (checkedcOK())
            {
                string sname = "";
                string strsmstext = txtmsg.Text.ToString();
                if (strsmstext.Trim() != "")
                {
                    for (int i = 0; i < FpSpread2.Sheets[0].Rows.Count; i++)
                    {
                        int check = Convert.ToInt32(FpSpread2.Sheets[0].Cells[i, 5].Value);
                        if (check == 1)
                        {
                            string roll = FpSpread2.Sheets[0].Cells[i, 2].Text.ToString();
                            string reg = FpSpread2.Sheets[0].Cells[i, 2].Text.ToString();
                            string name = FpSpread2.Sheets[0].Cells[i, 3].Text.ToString();
                            string mobileno = FpSpread2.Sheets[0].Cells[i, 4].Text.ToString();


                            string strbval = strsmstext;
                            strbval = strbval.ToUpper().Replace("$ROLLNO$", roll);
                            strbval = strbval.ToUpper().Replace("$REGNO$", reg);
                            strbval = strbval.ToUpper().Replace("$NAME$", name);
                            string strsenderquery = "select SMS_User_ID,college_code from Track_Value where college_code = '" + collegecode + "'";
                            ds1 = da.select_method_wo_parameter(strsenderquery, "Text");
                            string user_id = "";
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                user_id = Convert.ToString(ds1.Tables[0].Rows[0]["SMS_User_ID"]);
                            }
                            if (mobileno.Trim() != "")
                            {
                                int sms = da.send_sms(user_id, collegecode, usercode, mobileno, strbval, "0");
                            }
                            else
                            {
                                //lblerror.Visible = true;
                                //lblerror.Text = "There is no mobile no for this student :" + name;
                            }
                        }
                    }
                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Staff And Then Proceed";
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void ImageButton6_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            div_Add.Visible = false;

        }
        catch
        {

        }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("~/Default.aspx");
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    protected void btn_alertclose_Click(object sender, EventArgs e)
    {
        imgAlert.Visible = false;
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = "";
        string pagename = "Student_HT_Report.aspx";
        Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
        Printcontrol.Visible = true;
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
                lblerror.Text = "Please Enter Your Report Name";
                lblerror.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
}