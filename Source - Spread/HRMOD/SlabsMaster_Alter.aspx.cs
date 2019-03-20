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
using System.IO;
using Gios.Pdf;
using System.Text.RegularExpressions;


public partial class SlabsMaster_Alter : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    bool cellclick = false;
    FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

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
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                rb_all.Checked = true;
                rb_all_add.Checked = true;
                cb.AutoPostBack = false;
                btndel.Visible = false;
                Fpspread1.Visible = false;
                bindclg();
                bindcatg();
                rb_all_CheckedChanged(sender, e);
                rb_all_add_CheckedChanged(sender, e);
            }
        }
        catch (Exception ex) { }
    }

    public void bindclg()
    {
        try
        {
            ddl_clg.Items.Clear();
            ddl_newcol.Items.Clear();
            string selectQuery = "select collname,college_code from collinfo";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_clg.DataSource = ds;
                ddl_clg.DataTextField = "collname";
                ddl_clg.DataValueField = "college_code";
                ddl_clg.DataBind();

                ddl_newcol.DataSource = ds;
                ddl_newcol.DataTextField = "collname";
                ddl_newcol.DataValueField = "college_code";
                ddl_newcol.DataBind();
            }
        }
        catch { }
    }

    protected void txt_salfrm_OnTextChanged(object sender, EventArgs e)
    {
        double frmsal = 0;
        Double.TryParse(Convert.ToString(txt_salfrm.Text), out frmsal);
        double tosal = 0;
        Double.TryParse(Convert.ToString(txt_salto.Text), out tosal);
        if (frmsal > tosal)
        {
            imgdiv2.Visible = true;
            lbl_alert.Visible = true;
            lbl_alert.Text = "Starting salary should be less than end salary";
        }
        else
        {
            imgdiv2.Visible = false;
            lbl_alert.Visible = false;
        }
    }

    protected void txt_salto_OnTextChanged(object sender, EventArgs e)
    {
        double frmsal = 0;
        Double.TryParse(Convert.ToString(txt_salfrm.Text), out frmsal);
        double tosal = 0;
        Double.TryParse(Convert.ToString(txt_salto.Text), out tosal);
        if (tosal < frmsal)
        {
            imgdiv2.Visible = true;
            lbl_alert.Visible = true;
            lbl_alert.Text = "End salary should be greater than starting salary";
        }
        else
        {
            imgdiv2.Visible = false;
            lbl_alert.Visible = false;
        }
    }

    public void bindcatg()
    {
        try
        {
            string collegecode1 = Convert.ToString(ddl_newcol.SelectedItem.Value);
            string selectQuery = "select distinct category_code,category_name from staffcategorizer where college_code ='" + collegecode1 + "' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_catg.DataSource = ds;
                ddl_catg.DataTextField = "category_name";
                ddl_catg.DataValueField = "category_code";
                ddl_catg.DataBind();
                ddl_catg.Items.Insert(0, "Select");
            }
            else
            {
                ddl_catg.Items.Insert(0, "Select");
            }
        }
        catch (Exception ex) { }
    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        addnew.Visible = false;
        rb_all.Checked = false;
        rb_ded.Checked = false;
        rb_grad.Checked = false;
        ddl_clg.SelectedIndex = ddl_clg.Items.IndexOf(ddl_clg.Items.FindByValue(ddl_newcol.SelectedItem.Value));
        if (rb_all_add.Checked == true)
        {
            rb_all.Checked = true;
            rb_all_CheckedChanged(sender, e);
            ddl_slbval.SelectedIndex = ddl_slbvaladd.SelectedIndex;
        }
        if (rb_ded_add.Checked == true)
        {
            rb_ded.Checked = true;
            rb_ded_CheckedChanged(sender, e);
            ddl_slbval.SelectedIndex = ddl_slbvaladd.SelectedIndex;
        }
        if (rb_gradl_add.Checked == true)
        {
            rb_grad.Checked = true;
            rb_grad_CheckedChanged(sender, e);
            ddl_slbval.SelectedIndex = ddl_slbvaladd.SelectedIndex;
        }
        btn_go_Click(sender, e);
    }

    public void btn_go_Click(object sender, EventArgs e)
    {
        string collcode = Convert.ToString(ddl_clg.SelectedItem.Value);
        try
        {
            hide();
            string ddlslbval = ddl_slbval.SelectedItem.Value.ToString();
            Fpspread1.Sheets[0].RowHeader.Visible = false;
            Fpspread1.CommandBar.Visible = false;
            Fpspread1.Sheets[0].AutoPostBack = true;
            Fpspread1.ColumnHeader.RowCount = 1;
            Fpspread1.Sheets[0].Rows.Count = 0;
            Fpspread1.Sheets[0].Columns.Count = 8;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            string selqry = "select distinct salfrom,salto,category_code,ESI_EmpSlabType,ESI_EmpSlabValue,SlabFor,slabtype,slabvalue from pfslabs where SlabFor='" + ddlslbval + "' and college_code='" + collcode + "' order by  category_code";

            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Fpspread1.Sheets[0].Rows.Count = ds.Tables[0].Rows.Count;
                Fpspread1.Sheets[0].Columns.Count = 8;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string catcode = ds.Tables[0].Rows[i]["category_code"].ToString();
                    string catname = d2.GetFunction("SELECT category_name FROM staffcategorizer where college_code ='" + collcode + "' and category_code='" + catcode + "'");
                    Fpspread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                    Fpspread1.Sheets[0].Cells[i, 0].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[i, 1].Text = Convert.ToString(catname);
                    Fpspread1.Sheets[0].Cells[i, 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[i, 1].Tag = Convert.ToString(catcode);

                    Fpspread1.Sheets[0].Cells[i, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["salfrom"].ToString());
                    Fpspread1.Sheets[0].Cells[i, 2].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[i, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["SlabFor"].ToString());

                    Fpspread1.Sheets[0].Cells[i, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["salto"].ToString());
                    Fpspread1.Sheets[0].Cells[i, 3].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[i, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["slabtype"].ToString());
                    Fpspread1.Sheets[0].Cells[i, 4].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[i, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["slabvalue"].ToString());
                    Fpspread1.Sheets[0].Cells[i, 5].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[i, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["ESI_EmpSlabType"].ToString());
                    Fpspread1.Sheets[0].Cells[i, 6].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[i, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["ESI_EmpSlabValue"].ToString());
                    Fpspread1.Sheets[0].Cells[i, 7].Font.Name = "Book Antiqua";
                }

                for (int ii = 0; ii < 8; ii++)
                {
                    Fpspread1.Sheets[0].ColumnHeader.Columns[ii].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeader.Columns[ii].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Columns[ii].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Columns[ii].Font.Size = FontUnit.Medium;

                }
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Category";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Salary From";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Salary To";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Slab Type";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Slab Value";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Emp Slab Type";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Emp Slab Value";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";


                for (int j = 0; j < Fpspread1.Sheets[0].Columns.Count; j++)
                {
                    Fpspread1.Sheets[0].Columns[j].HorizontalAlign = HorizontalAlign.Center;
                }
                Fpspread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                Fpspread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                Fpspread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;

                for (int i = 0; i < Fpspread1.Sheets[0].Columns.Count; i++)
                {
                    Fpspread1.Sheets[0].Columns[i].Locked = true;
                }
                Fpspread1.Sheets[0].Columns[2].Locked = false;
                Fpspread1.Sheets[0].Columns[3].Locked = false;

                Fpspread1.Visible = true;
                rptprint.Visible = true;
                div1.Visible = true;
                lbl_error.Visible = false;
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.SaveChanges();
                addnew.Visible = false;
                show();
            }
            else
            {
                lbl_error.Visible = true;
                lbl_error.Text = "No Records Found";
                hide();
            }
        }
        catch (Exception er)
        {
            d2.sendErrorMail(er, collcode, "SlabsMaster_Alter.aspx");
        }
    }

    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        try
        {
            hide();
            slabdetails();
            txt_salfrm.Text = "";
            txt_salto.Text = "";
            txt_slbvalue.Text = "";
            txt_empslbvalues.Text = "";
            btndel.Visible = false;
            rb_all_add.Checked = false;
            rb_ded_add.Checked = false;
            rb_gradl_add.Checked = false;
            ddl_slbtype.SelectedIndex = 0;
            ddl_empslbtype.SelectedIndex = 0;
            ddl_slbvaladd.Enabled = true;
            txt_salfrm.Enabled = true;
            txt_salto.Enabled = true;
            rb_ded_add.Enabled = true;
            rb_gradl_add.Enabled = true;
            rb_all_add.Enabled = true;
            txtexcelname.Text = "";
            ddl_newcol.SelectedIndex = ddl_newcol.Items.IndexOf(ddl_newcol.Items.FindByValue(ddl_clg.SelectedItem.Value));
            ddl_newcol.Enabled = true;
            bindcatg();
            ddl_catg.SelectedIndex = 0;
            ddl_catg.Enabled = true;

            if (rb_all.Checked == true)
            {
                rb_all_add.Checked = true;
                rb_all_add_CheckedChanged(sender, e);
                ddl_slbvaladd.SelectedIndex = ddl_slbval.SelectedIndex;
            }
            if (rb_ded.Checked == true)
            {
                rb_ded_add.Checked = true;
                rb_ded_add_CheckedChanged(sender, e);
                ddl_slbvaladd.SelectedIndex = ddl_slbval.SelectedIndex;
            }
            if (rb_grad.Checked == true)
            {
                rb_gradl_add.Checked = true;
                rb_gradl_add_CheckedChanged(sender, e);
                ddl_slbvaladd.SelectedIndex = ddl_slbval.SelectedIndex;
            }
        }
        catch { }
    }

    public void slabdetails()
    {
        addnew.Visible = true;
        btnsave.Visible = true;
        btnsave.Text = "save";
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        string collcode = Convert.ToString(ddl_newcol.SelectedItem.Value);
        try
        {
            rb_all.Checked = false;
            rb_ded.Checked = false;
            rb_grad.Checked = false;

            string slabid = "";
            string actrow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            if (Convert.ToInt32(actrow) != -1)
            {
                slabid = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Tag);
            }
            string ddlslbval = Convert.ToString(ddl_slbvaladd.SelectedItem.Text.ToString());
            string ddlcatg = Convert.ToString(ddl_catg.SelectedItem.Value.ToString());
            string txtsalfrm = Convert.ToString(txt_salfrm.Text.ToString());
            string txtsalto = Convert.ToString(txt_salto.Text.ToString());
            string ddlslbtype = Convert.ToString(ddl_slbtype.SelectedItem.Text.ToString());
            string txtslbvalue = Convert.ToString(txt_slbvalue.Text.ToString());
            string ddlempslbtype = Convert.ToString(ddl_empslbtype.SelectedItem.Text.ToString());
            string txtempslbvalue = Convert.ToString(txt_empslbvalues.Text.ToString());
            string ddlslbvaladd = Convert.ToString(ddl_slbvaladd.SelectedItem.Value.ToString());
            string lbloldfrom = Convert.ToString(lbl_oldsalf.Text.ToString());
            string lbloldto = Convert.ToString(lbl_oldsalt.Text.ToString());

            double salfrm = Convert.ToDouble(txtsalfrm);
            double salto = Convert.ToDouble(txtsalto);

            if (ddl_slbvaladd.SelectedItem.Text.Trim() == "Select")
            {
                lbl_alert.Text = "Please Select the Slab Name!";
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;
                return;
            }

            if (ddl_catg.SelectedItem.Text.Trim() == "Select")
            {
                lbl_alert.Text = "Please Select the Category Name!";
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;
                return;
            }

            if (salfrm > salto)
            {
                txt_salfrm.Text = "";
                txt_salto.Text = "";
                txt_salfrm.BorderColor = Color.Red;
                txt_salto.BorderColor = Color.Red;
                lbl_alert.Text = "Please Enter Correct Salary";
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;
                return;
            }
            if (ddlslbtype.ToLower().Trim() == "percent")
            {
                if (Convert.ToDouble(txtslbvalue) > 100)
                {
                    txt_slbvalue.Text = "";
                    txt_slbvalue.BorderColor = Color.Red;
                    lbl_alert.Text = "Please Enter Correct Slab Value Percent";
                    lbl_alert.Visible = true;
                    imgdiv2.Visible = true;
                    return;
                }
            }
            if (ddlempslbtype.ToLower().Trim() == "percent")
            {
                if (Convert.ToDouble(txtempslbvalue) > 100)
                {
                    txt_empslbvalues.Text = "";
                    txt_empslbvalues.BorderColor = Color.Red;
                    lbl_alert.Text = "Please Enter Correct Emp.Slab Value Percent";
                    lbl_alert.Visible = true;
                    imgdiv2.Visible = true;
                    return;
                }
            }
            if (btnsave.Text.ToLower().Trim() == "save")
            {
                string selquery = "select salfrom,salto from pfslabs where college_code='" + collcode + "'  and category_code='" + ddlcatg + "' and SlabFor='" + ddlslbvaladd + "' and salfrom <= '" + txtsalfrm + "' and salto >= '" + txtsalto + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selquery, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Salary Exists!";
                    return;
                }
                else
                {
                    string insqry = "insert into pfslabs(salfrom,salto,slabtype,college_code,category_code, ESI_EmpSlabType,ESI_EmpSlabValue,SlabFor,slabvalue)values('" + txtsalfrm + "','" + txtsalto + "','" + ddlslbtype + "','" + collcode + "','" + ddlcatg + "','" + ddlempslbtype + "','" + txtempslbvalue + "','" + ddlslbvaladd + "','" + txtslbvalue + "')";
                    int updq = d2.update_method_wo_parameter(insqry, "Text");
                    if (updq > 0)
                    {
                        lbl_alert.Text = "Saved  Successfully";
                    }
                }
            }
            else if (btnsave.Text.ToLower().Trim() == "update")
            {
                string updqry = "update pfslabs set slabtype='" + ddlslbtype + "',slabvalue='" + txtslbvalue + "',ESI_EmpSlabType='" + ddlempslbtype + "',ESI_EmpSlabValue='" + txtempslbvalue + "' where college_code='" + collcode + "'  and category_code='" + ddlcatg + "' and SlabFor='" + ddlslbvaladd + "' and salfrom='" + txtsalfrm + "' and salto='" + txtsalto + "'";

                int up = d2.update_method_wo_parameter(updqry, "Text");
                if (up > 0)
                {
                    lbl_alert.Text = "Update Successfully";
                }
                else
                {
                    lbl_alert.Text = "Cannot Update the Category Name!";
                }
            }
            lbl_alert.Visible = true;
            imgdiv2.Visible = true;
            addnew.Visible = false;
            ddl_clg.SelectedIndex = ddl_clg.Items.IndexOf(ddl_clg.Items.FindByValue(ddl_newcol.SelectedItem.Value));
            if (rb_all_add.Checked == true)
            {
                rb_all.Checked = true;
                rb_all_CheckedChanged(sender, e);
                ddl_slbval.SelectedIndex = ddl_slbvaladd.SelectedIndex;
            }
            else if (rb_ded_add.Checked == true)
            {
                rb_ded.Checked = true;
                rb_ded_CheckedChanged(sender, e);
                ddl_slbval.SelectedIndex = ddl_slbvaladd.SelectedIndex;
            }
            else
            {
                rb_grad.Checked = true;
                rb_grad_CheckedChanged(sender, e);
                ddl_slbval.SelectedIndex = ddl_slbvaladd.SelectedIndex;
            }
            btn_go_Click(sender, e);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collcode, "SlabMaster_Alter.aspx");
        }
    }

    protected void Cell_Click(object sender, EventArgs e)
    {
        try
        {
            string activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            string activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();

            cellclick = true;
            btndel.Visible = true;
        }
        catch { }
    }

    protected void Fpspread1_render(object sender, EventArgs e)
    {
        try
        {
            if (cellclick == true)
            {
                string activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                string activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                string slabvaluecell = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag.ToString();

                ddl_newcol.SelectedIndex = ddl_newcol.Items.IndexOf(ddl_newcol.Items.FindByValue(ddl_clg.SelectedItem.Value));
                ddl_newcol.Enabled = false;
                bindcatg();
                rb_all_add.Checked = false;
                rb_ded_add.Checked = false;
                rb_gradl_add.Checked = false;
                ddl_slbvaladd.Enabled = false;
                ddl_catg.Enabled = false;
                txt_salfrm.Enabled = false;
                txt_salto.Enabled = false;

                if (rb_all.Checked == true)
                {
                    rb_ded_add.Enabled = false;
                    rb_gradl_add.Enabled = false;
                    rb_all_add.Enabled = true;
                    rb_all_add.Checked = true;
                    rb_all_add_CheckedChanged(sender, e);
                }
                else if (rb_ded.Checked == true)
                {
                    rb_all_add.Enabled = false;
                    rb_gradl_add.Enabled = false;
                    rb_ded_add.Enabled = true;
                    rb_ded_add.Checked = true;
                    rb_ded_add_CheckedChanged(sender, e);
                }
                else if (rb_grad.Checked == true)
                {
                    rb_all_add.Enabled = false;
                    rb_ded_add.Enabled = false;
                    rb_gradl_add.Enabled = true;
                    rb_gradl_add.Checked = true;
                    rb_gradl_add_CheckedChanged(sender, e);
                }

                ds.Clear();
                string slabvaladd = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag.ToString();
                for (int i = 0; i < ddl_slbvaladd.Items.Count; i++)
                {
                    if (ddl_slbvaladd.Items[i].Value.ToString().ToLower().Trim() == slabvaladd.ToLower().Trim())
                    {
                        ddl_slbvaladd.SelectedIndex = ddl_slbvaladd.Items.IndexOf(ddl_slbvaladd.Items.FindByText(slabvaladd));
                    }
                }

                string catg = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag.ToString();
                for (int i = 0; i < ddl_catg.Items.Count; i++)
                {
                    if (ddl_catg.Items[i].Value.ToString().ToLower().Trim() == catg.ToLower().Trim())
                    {
                        ddl_catg.SelectedIndex = i;
                    }
                }

                string salfrm = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                txt_salfrm.Text = salfrm;
                lbl_oldsalf.Text = salfrm;
                txt_salfrm.Attributes.Add("Style", "border: 1px solid #c4c4c4;text-transform: capitalize;padding: 4px 4px 4px 4px;border-radius: 4px;-moz-border-radius: 4px;-webkit-border-radius: 4px;box-shadow: 0px 0px 8px #d9d9d9;-moz-box-shadow: 0px 0px 8px #d9d9d9;-webkit-box-shadow: 0px 0px 8px #d9d9d9;height: 20px;width: 135px;");

                string salto = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                txt_salto.Text = salto;
                lbl_oldsalt.Text = salto;
                txt_salto.Attributes.Add("Style", "border: 1px solid #c4c4c4;text-transform: capitalize;padding: 4px 4px 4px 4px;border-radius: 4px;-moz-border-radius: 4px;-webkit-border-radius: 4px;box-shadow: 0px 0px 8px #d9d9d9;-moz-box-shadow: 0px 0px 8px #d9d9d9;-webkit-box-shadow: 0px 0px 8px #d9d9d9;height: 20px;width: 135px;");

                string slbtype = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                for (int i = 0; i < ddl_slbtype.Items.Count; i++)
                {
                    if (ddl_slbtype.Items[i].Value.ToString().ToLower().Trim() == slbtype.ToLower().Trim())
                    {
                        ddl_slbtype.SelectedIndex = i;
                    }
                }

                string slbvalue = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text;
                txt_slbvalue.Text = slbvalue;
                txt_slbvalue.Attributes.Add("Style", "border: 1px solid #c4c4c4;text-transform: capitalize;padding: 4px 4px 4px 4px;border-radius: 4px;-moz-border-radius: 4px;-webkit-border-radius: 4px;box-shadow: 0px 0px 8px #d9d9d9;-moz-box-shadow: 0px 0px 8px #d9d9d9;-webkit-box-shadow: 0px 0px 8px #d9d9d9;height: 20px;width: 135px;");

                string empslbtype = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text;
                for (int i = 0; i < ddl_empslbtype.Items.Count; i++)
                {
                    if (ddl_empslbtype.Items[i].Value.ToString().ToLower().Trim() == empslbtype.ToLower().Trim())
                    {
                        ddl_empslbtype.SelectedIndex = i;
                    }
                }
                string empslbvalue = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Text;
                txt_empslbvalues.Text = empslbvalue;
                txt_empslbvalues.Attributes.Add("Style", "border: 1px solid #c4c4c4;text-transform: capitalize;padding: 4px 4px 4px 4px;border-radius: 4px;-moz-border-radius: 4px;-webkit-border-radius: 4px;box-shadow: 0px 0px 8px #d9d9d9;-moz-box-shadow: 0px 0px 8px #d9d9d9;-webkit-box-shadow: 0px 0px 8px #d9d9d9;height: 20px;width: 135px;");

                addnew.Visible = true;
                btnsave.Visible = true;
                btnsave.Text = "Update";
            }
            cellclick = false;
        }
        catch { }
    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        try
        {
            addnew.Visible = false;
            ddl_clg.SelectedIndex = ddl_clg.Items.IndexOf(ddl_clg.Items.FindByValue(ddl_newcol.SelectedItem.Value));
            if (rb_all_add.Checked == true)
            {
                rb_all.Checked = true;
                rb_all_CheckedChanged(sender, e);
                ddl_slbval.SelectedIndex = ddl_slbvaladd.SelectedIndex;
            }
            else if (rb_ded_add.Checked == true)
            {
                rb_ded.Checked = true;
                rb_ded_CheckedChanged(sender, e);
                ddl_slbval.SelectedIndex = ddl_slbvaladd.SelectedIndex;
            }
            else
            {
                rb_grad.Checked = true;
                rb_grad_CheckedChanged(sender, e);
                ddl_slbval.SelectedIndex = ddl_slbvaladd.SelectedIndex;
            }
            btn_go_Click(sender, e);
        }
        catch { }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            lblvalidation1.Visible = false;
            string degreedetails = "SlabsMaster Report";
            string pagename = "SlabsMaster_Alter.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch { }
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
        catch { }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    public void ddl_clg_SelectedChanged(object sender, EventArgs e)
    {
        rb_all.Checked = true;
        rb_ded.Checked = false;
        rb_grad.Checked = false;
        cb.AutoPostBack = false;
        btndel.Visible = false;
        Fpspread1.Visible = false;
        rb_all_CheckedChanged(sender, e);
    }

    protected void ddl_newcol_Change(object sender, EventArgs e)
    {
        rb_all_add.Checked = true;
        rb_ded_add.Checked = false;
        rb_gradl_add.Checked = false;
        cb.AutoPostBack = false;
        btndel.Visible = false;
        bindcatg();
        rb_all_add_CheckedChanged(sender, e);
    }

    public void ddl_val_SelectedIndexChanged(object sender, EventArgs e)
    {
        btn_go_Click(sender, e);
    }

    public void imagebtnpopclose_Click(object sender, EventArgs e)
    {

    }

    public void btndel_Click(object sender, EventArgs e)
    {
        imgdiv1.Visible = true;
        lblalert.Visible = true;
        lblalert.Text = "Do you want to Delete this Record?";
    }

    public void ddl_slbvaladd_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    public void rb_ded_CheckedChanged(object sender, EventArgs e)
    {
        ds.Clear();
        ddl_slbval.Items.Clear();
        string collcode = Convert.ToString(ddl_clg.SelectedItem.Value);
        string item = "select deductions from incentives_master  where college_code = '" + collcode + "' ";
        ds = d2.select_method_wo_parameter(item, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            string st = Convert.ToString(ds.Tables[0].Rows[0]["deductions"]);
            string[] split = st.Split(';');
            for (int row = 0; row < split.Length; row++)
            {
                string staff = split[row];
                string[] split1 = staff.Split('\\');
                string stafftype = split1[0];
                if (stafftype.Trim() != "")
                {
                    ddl_slbval.Items.Add(stafftype);
                }
            }
            ddl_slbval.Items.Insert(0, "Select");
        }
        else
        {
            ddl_slbval.Items.Insert(0, "Select");
        }
        btn_go_Click(sender, e);
    }

    public void rb_all_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            ds.Clear();
            ddl_slbval.Items.Clear();
            string collcode = Convert.ToString(ddl_clg.SelectedItem.Value);
            string item = "select allowances  from incentives_master where college_code = '" + collcode + "'  ";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl_slbval.DataSource = ds;
                string st = Convert.ToString(ds.Tables[0].Rows[0]["allowances"]);
                string[] split = st.Split(';');
                for (int row = 0; row < split.Length; row++)
                {
                    string staff = split[row];
                    string[] split1 = staff.Split('\\');
                    string stafftype = split1[0];
                    if (stafftype.Trim() != "")
                    {
                        ddl_slbval.Items.Add(stafftype);
                    }
                }
                ddl_slbval.Items.Insert(0, "Select");
            }
            else
            {
                ddl_slbval.Items.Insert(0, "Select");
            }
        }
        catch { }
        btn_go_Click(sender, e);
    }

    public void rb_grad_CheckedChanged(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        hide();
        ddl_slbval.Items.Clear();
        ddl_slbval.Items.Add("Grade Pay");
        btn_go_Click(sender, e);
    }

    public void rb_all_add_CheckedChanged(object sender, EventArgs e)
    {
        add_allow();
    }

    public void add_allow()
    {
        try
        {
            ds.Clear();
            ddl_slbvaladd.Items.Clear();
            string collcode = Convert.ToString(ddl_newcol.SelectedItem.Value);
            string item = "select allowances  from incentives_master where college_code = '" + collcode + "'  ";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl_slbvaladd.DataSource = ds;
                string st = Convert.ToString(ds.Tables[0].Rows[0]["allowances"]);
                string[] split = st.Split(';');
                for (int row = 0; row < split.Length; row++)
                {
                    string staff = split[row];
                    string[] split1 = staff.Split('\\');
                    string stafftype = split1[0];
                    if (stafftype.Trim() != "")
                    {
                        ddl_slbvaladd.Items.Add(stafftype);
                    }
                }
                ddl_slbvaladd.Items.Insert(0, "Select");
            }
            else
            {
                ddl_slbvaladd.Items.Insert(0, "Select");
            }
        }
        catch { }
    }

    public void rb_ded_add_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            ds.Clear();
            ddl_slbvaladd.Items.Clear();
            string collcode = Convert.ToString(ddl_newcol.SelectedItem.Value);
            string item = "select deductions from incentives_master  where college_code = '" + collcode + "' ";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                string st = Convert.ToString(ds.Tables[0].Rows[0]["deductions"]);
                string[] split = st.Split(';');
                for (int row = 0; row < split.Length; row++)
                {
                    string staff = split[row];
                    string[] split1 = staff.Split('\\');
                    string stafftype = split1[0];
                    if (stafftype.Trim() != "")
                    {
                        ddl_slbvaladd.Items.Add(stafftype);
                    }
                }
                ddl_slbvaladd.Items.Insert(0, "Select");
            }
            else
            {
                ddl_slbvaladd.Items.Insert(0, "Select");
            }
        }
        catch { }
    }

    public void rb_gradl_add_CheckedChanged(object sender, EventArgs e)
    {
        ddl_slbvaladd.Items.Clear();
        ddl_slbvaladd.Items.Add("Grade Pay");
    }

    public void hide()
    {
        lblvalidation1.Visible = false;
        Printcontrol.Visible = false;
        div1.Visible = false;
        rptprint.Visible = false;
        txtexcelname.Text = "";
    }

    public void show()
    {
        div1.Visible = true;
        rptprint.Visible = true;
    }

    protected void ddl_slbtype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (txt_slbvalue.Text.Trim() != "")
        {
            if (Convert.ToInt32(txt_slbvalue.Text.Trim()) <= 100)
            {
                txt_slbvalue.Text = "";
            }
            else
            {
                txt_slbvalue.Text = "";
            }
        }
    }

    protected void ddl_empslbtype_Change(object sender, EventArgs e)
    {
        if (txt_empslbvalues.Text.Trim() != "")
        {
            if (Convert.ToInt32(txt_empslbvalues.Text.Trim()) <= 100)
            {
                txt_empslbvalues.Text = "";
            }
            else
            {
                txt_empslbvalues.Text = "";
            }
        }
    }

    protected void btnyes_Click(object sender, EventArgs e)
    {
        try
        {
            string slabid = "";
            string collcode = Convert.ToString(ddl_newcol.SelectedItem.Value);
            string actrow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            if (Convert.ToInt32(actrow) != -1)
            {
                slabid = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Tag);
            }
            string ddlslbvaladd = Convert.ToString(ddl_slbvaladd.SelectedItem.Value.ToString());
            string ddlcatg = Convert.ToString(ddl_catg.SelectedItem.Value.ToString());
            string lbloldfrom = Convert.ToString(lbl_oldsalf.Text.ToString());
            string lbloldto = Convert.ToString(lbl_oldsalt.Text.ToString());
            string ddlslbtype = Convert.ToString(ddl_slbtype.SelectedItem.Text.ToString());
            string txtslbvalue = Convert.ToString(txt_slbvalue.Text.ToString());

            string delqry = "delete from pfslabs where  SlabFor='" + ddlslbvaladd + "' and category_code='" + ddlcatg + "' and slabvalue='" + txtslbvalue + "' and slabtype='" + ddlslbtype + "' and salfrom='" + lbloldfrom + "' and salto='" + lbloldto + "' and college_code='" + collcode + "'";
            int upnow = d2.update_method_wo_parameter(delqry, "Text");
            lbl_alert.Text = "Delete Successfully";
            lbl_alert.Visible = true;
            imgdiv2.Visible = true;
            imgdiv1.Visible = false;
            lblalert.Visible = false;
            addnew.Visible = false;

            ddl_clg.SelectedIndex = ddl_clg.Items.IndexOf(ddl_clg.Items.FindByValue(ddl_newcol.SelectedItem.Value));
            if (rb_all_add.Checked == true)
            {
                rb_all.Checked = true;
                rb_all_CheckedChanged(sender, e);
                ddl_slbval.SelectedIndex = ddl_slbvaladd.SelectedIndex;
            }
            else if (rb_ded_add.Checked == true)
            {
                rb_ded.Checked = true;
                rb_ded_CheckedChanged(sender, e);
                ddl_slbval.SelectedIndex = ddl_slbvaladd.SelectedIndex;
            }
            else
            {
                rb_grad.Checked = true;
                rb_grad_CheckedChanged(sender, e);
                ddl_slbval.SelectedIndex = ddl_slbvaladd.SelectedIndex;
            }
            btn_go_Click(sender, e);
        }
        catch { }
    }

    protected void btnno_Click(object sender, EventArgs e)
    {
        imgdiv1.Visible = false;
    }
}