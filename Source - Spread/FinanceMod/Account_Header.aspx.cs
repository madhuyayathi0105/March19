using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;

public partial class Account_Header : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string sesscolcode = string.Empty;
    bool spreadnewclick = false;
    bool spreadclick1 = false;
    bool spreadclick2 = false;
    bool spreadclick3 = false;
    bool spreadclick4 = false;
    static string collegestat = "";
    static string collegestat0 = "";
    Boolean flag_true = false;
    bool check = false;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblvalidation1.Visible = false;
            sesscolcode = Session["collegecode"].ToString();
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("FinanceIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/FinanceMod/FinanceIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                setLabelText();
                bindcollege();
                bindloadcol();
                if (ddlclgname.Items.Count > 0)
                {
                    collegestat = Convert.ToString(ddlclgname.SelectedItem.Value);
                }
                if (ddlmainacc.Items.Count > 0)
                {
                    collegestat0 = Convert.ToString(ddlmainacc.SelectedItem.Value);
                }
                bindacctname();
                cbacctname.Checked = true;
                btngo_Click(sender, e);
            }
            if (ddlclgname.Items.Count > 0)
            {
                collegestat = Convert.ToString(ddlclgname.SelectedItem.Value);
            }
            if (ddlmainacc.Items.Count > 0)
            {
                collegestat0 = Convert.ToString(ddlmainacc.SelectedItem.Value);
            }
        }
        catch (Exception ex) { }
    }

    [WebMethod]
    public static string CheckAccHeader(string AccHeader)
    {
        string returnValue = "1";
        try
        {
            DAccess2 dd = new DAccess2();
            string acr_hdr = AccHeader;
            if (acr_hdr.Trim() != "" && acr_hdr != null)
            {
                string queryacc = dd.GetFunction("select distinct HeaderName from FM_HeaderMaster where HeaderName='" + acr_hdr + "' AND COLLEGECODE=" + collegestat0 + "");
                if (queryacc.Trim() == "" || queryacc == null || queryacc == "0" || queryacc == "-1")
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
    public static string CheckAccAcr(string AccAcr)
    {
        string returnValue = "1";
        try
        {
            DAccess2 dd = new DAccess2();
            string acr_acc = AccAcr;
            if (acr_acc.Trim() != "" && acr_acc != null)
            {
                string queryacc = dd.GetFunction("select distinct HeaderAcr from FM_HeaderMaster where HeaderAcr='" + acr_acc + "' AND COLLEGECODE=" + collegestat0 + "");
                if (queryacc.Trim() == "" || queryacc == null || queryacc == "0" || queryacc == "-1")
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

    protected void ddlclgname_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindacctname();
            btngo_Click(sender, e);
        }
        catch { }
    }
    //protected void lb2_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Session.Abandon();
    //        Session.Clear();
    //        Session.RemoveAll();
    //        System.Web.Security.FormsAuthentication.SignOut();
    //        Response.Redirect("Account_Header.aspx", false);
    //    }
    //    catch
    //    {
    //    }
    //}
    protected void bindacctname()
    {
        try
        {
            ds.Clear();
            cblacctname.Items.Clear();
            string header = "";
            string statequery = "select distinct HeaderName,HeaderPK from FM_HeaderMaster where CollegeCode='" + collegestat + "' order by HeaderPK ";
            ds = d2.select_method_wo_parameter(statequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblacctname.DataSource = ds;
                cblacctname.DataTextField = "HeaderName";
                cblacctname.DataValueField = "HeaderPK";
                cblacctname.DataBind();
                if (cblacctname.Items.Count > 0)
                {
                    for (int i = 0; i < cblacctname.Items.Count; i++)
                    {
                        cblacctname.Items[i].Selected = true;
                        header = Convert.ToString(cblacctname.Items[i].Text);
                    }
                    if (cblacctname.Items.Count == 1)
                    {
                        txt_acctname.Text = "Header(" + header + ")";
                    }
                    else
                    {
                        txt_acctname.Text = "Header(" + cblacctname.Items.Count + ")";
                    }
                }
            }

        }
        catch
        {

        }
    }

    protected void ddlsearch1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlsearch1.SelectedIndex == 0)
        {
            txtsearch1.Visible = true;
            Label1.Text = "Search By Name";
            txtsearch1c.Visible = false;
        }
        else
        {
            txtsearch1c.Visible = true;
            Label1.Text = "Search By Code";
            txtsearch1.Visible = false;
        }
    }

    protected void ddlsearch2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlsearch2.SelectedIndex == 0)
        {
            txtsearch2.Visible = true;
            Label2.Text = "Search By Name";
            txtsearch2c.Visible = false;
        }
        else
        {
            txtsearch2c.Visible = true;
            Label2.Text = "Search By Code";
            txtsearch2.Visible = false;
        }
    }

    protected void ddlsearch3_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlsearch3.SelectedIndex == 0)
        {
            txtsearch3.Visible = true;
            Label3.Text = "Search By Name";
            txtsearch3c.Visible = false;
        }
        else
        {
            txtsearch3c.Visible = true;
            Label3.Text = "Search By Code";
            txtsearch3.Visible = false;
        }
    }

    protected void ddlsearch4_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlsearch4.SelectedIndex == 0)
        {
            txtsearch4.Visible = true;
            Label4.Text = "Search By Name";
            txtsearch4c.Visible = false;
        }
        else
        {
            txtsearch4c.Visible = true;
            Label4.Text = "Search By Code";
            txtsearch4.Visible = false;
        }
    }

    protected void cbacctname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string header = "";
            if (cbacctname.Checked == true)
            {
                for (int i = 0; i < cblacctname.Items.Count; i++)
                {
                    cblacctname.Items[i].Selected = true;
                    header = Convert.ToString(cblacctname.Items[i].Text);
                }
                if (cblacctname.Items.Count == 1)
                {
                    txt_acctname.Text = "" + header + "";
                }
                else
                {
                    txt_acctname.Text = "Header(" + (cblacctname.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < cblacctname.Items.Count; i++)
                {
                    cblacctname.Items[i].Selected = false;
                }
                txt_acctname.Text = "--Select--";
            }
        }
        catch
        {

        }
    }
    protected void cblacctname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_acctname.Text = "--Select--";
            cbacctname.Checked = false;
            string header = "";
            int commcount = 0;
            for (int i = 0; i < cblacctname.Items.Count; i++)
            {
                if (cblacctname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    header = Convert.ToString(cblacctname.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                if (commcount == cblacctname.Items.Count)
                {
                    cbacctname.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_acctname.Text = "" + header + "";
                }
                else
                {
                    txt_acctname.Text = "Header(" + commcount.ToString() + ")";
                }
            }

        }
        catch
        {

        }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        if (!cbhdpriority.Checked)
        {
            loadMainDetails();
        }
        else
        {
            cbhdpriority_Changed(sender, e);
        }
    }
    protected void loadMainDetails()
    {
        try
        {
            if (sesscolcode != null)
            {
                string headercode = "";
                int count = 0;
                for (int i = 0; i < cblacctname.Items.Count; i++)
                {
                    if (cblacctname.Items[i].Selected == true)
                    {
                        if (headercode == "")
                        {
                            headercode = "" + cblacctname.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            headercode = headercode + "'" + "," + "'" + cblacctname.Items[i].Value.ToString() + "";
                        }
                    }
                    if (txtsearch.Text.Trim() == cblacctname.Items[i].Text.ToString())
                    {
                        count = count + 1;
                    }

                }
                if (headercode.Trim() != "")
                {
                    string selq = "";
                    if (txtsearch.Text.Trim() != "")
                    {
                        selq = "select HeaderAcr,HeaderName,HeaderPK,Purpose,CollegeCode,PayInchargeStaff1,PayInchargeStaff2,PayInchargeStaff3,RcptInchargeStaff1,hd_priority from FM_HeaderMaster where HeaderName='" + Convert.ToString(txtsearch.Text) + "' and HeaderPK in('" + headercode + "') and CollegeCode='" + collegestat + "' order by len(isnull(hd_priority,10000)),hd_priority asc";
                    }
                    else
                    {
                        selq = "select HeaderAcr,HeaderName,HeaderPK,Purpose,CollegeCode,PayInchargeStaff1,PayInchargeStaff2,PayInchargeStaff3,RcptInchargeStaff1,hd_priority from FM_HeaderMaster where HeaderPK in('" + headercode + "') and CollegeCode='" + collegestat + "' order by len(isnull(hd_priority,10000)),hd_priority asc";
                    }
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selq, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Fpspreadnew.Sheets[0].RowCount = 0;
                        Fpspreadnew.Sheets[0].ColumnCount = 0;
                        Fpspreadnew.CommandBar.Visible = false;
                        Fpspreadnew.Sheets[0].AutoPostBack = true;
                        Fpspreadnew.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspreadnew.Sheets[0].RowHeader.Visible = false;
                        Fpspreadnew.Sheets[0].ColumnCount = 5;

                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.Black;
                        Fpspreadnew.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadnew.Columns[0].Width = 30;

                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Header Name";
                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadnew.Columns[1].Width = 50;

                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Acronym";
                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadnew.Columns[2].Width = 50;

                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Payment Incharge";
                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Purpose";
                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        Fpspreadnew.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;


                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            Fpspreadnew.Sheets[0].RowCount++;
                            Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                            Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["HeaderName"]);
                            Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["CollegeCode"]);
                            Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                            Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["HeaderAcr"]);
                            Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["HeaderPK"]);
                            Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                            Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[row]["PayInchargeStaff1"]);
                            string desig_code = Convert.ToString(Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 3].Tag);
                            string selquid = "select desig_name from desig_master where desig_code='" + desig_code + "' and collegeCode='" + collegestat + "'";
                            string desig_name = d2.GetFunction(selquid);

                            if (desig_name.Trim() != "" && desig_name.Trim() != "0")
                            {
                                Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(desig_name);
                            }
                            else
                            {
                                Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 3].Text = "";
                            }
                            Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                            Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["Purpose"]);
                            Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        }
                        Fpspreadnew.Visible = true;
                        div1.Visible = true;
                        txtsearch.Text = "";
                        lblerror.Visible = false;
                        rportprint.Visible = true;
                        Fpspreadnew.Sheets[0].PageSize = Fpspreadnew.Sheets[0].RowCount;
                    }
                    else
                    {
                        if (txtsearch.Text.Trim() != "")
                        {
                            string selquery = "select HeaderName from FM_HeaderMaster where HeaderName in('" + Convert.ToString(txtsearch.Text) + "') and HeaderPK in('" + headercode + "') and CollegeCode='" + collegestat + "'";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selquery, "Text");
                            if (ds.Tables[0].Rows.Count == 0 && count == 0)
                            {
                                Fpspreadnew.Visible = false;
                                div1.Visible = false;
                                txtsearch.Text = "";
                                lblerror.Visible = true;
                                rportprint.Visible = false;
                                lblerror.Text = "Invalid Search";
                            }
                            else
                            {
                                Fpspreadnew.Visible = false;
                                div1.Visible = false;
                                txtsearch.Text = "";
                                lblerror.Visible = true;
                                rportprint.Visible = false;
                                lblerror.Text = "Select Proper Header Name";
                            }
                        }
                    }
                }
                else
                {
                    Fpspreadnew.Visible = false;
                    div1.Visible = false;
                    txtsearch.Text = "";
                    lblerror.Visible = true;
                    rportprint.Visible = false;
                    lblerror.Text = "No Records Found!";
                }
            }
        }
        catch
        {
        }
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        clear();
        lblmainerr.Visible = false;
        poppernew.Visible = true;
        btnsave.Visible = true;
        btnupdate.Visible = false;
        btndelete.Visible = false;
        cbincmis.Checked = false;
    }
    protected void Cellcont_Click(object sender, EventArgs e)
    {
        try
        {
            spreadnewclick = true;
        }
        catch
        {
        }
    }
    protected void Fpspreadnew_render(object sender, EventArgs e)
    {
        try
        {
            if (spreadnewclick == true)
            {
                //bindaccname();
                bindacctname();
                poppernew.Visible = true;
                lblmainerr.Visible = false;
                btnsave.Visible = false;
                btnupdate.Visible = true;
                btndelete.Visible = true;
                cbincmis.Checked = false;
                string currentrow = "";
                string openbal = "";
                string currentcol = "";
                currentrow = Fpspreadnew.ActiveSheetView.ActiveRow.ToString();
                currentcol = Fpspreadnew.ActiveSheetView.ActiveColumn.ToString();
                string clgcode = Convert.ToString(Fpspreadnew.Sheets[0].Cells[Convert.ToInt32(currentrow), 1].Tag);
                if (currentrow.Trim() != "")
                {
                    //string acc_id = Convert.ToString(Fpspreadnew.Sheets[0].Cells[Convert.ToInt32(currentrow), 1].Tag);
                    string header_id = Convert.ToString(Fpspreadnew.Sheets[0].Cells[Convert.ToInt32(currentrow), 2].Tag);
                    string inch = Convert.ToString(Fpspreadnew.Sheets[0].Cells[Convert.ToInt32(currentrow), 3].Text);
                    //string amount = Convert.ToString(Fpspreadnew.Sheets[0].Cells[Convert.ToInt32(currentrow),4].Note);
                    //txt_openbal.Text = Convert.ToString(amount);
                    string selectaccload = "select HeaderAcr,HeaderName,Purpose,CollegeCode,PayInchargeStaff1,PayInchargeStaff2,PayInchargeStaff3,RcptInchargeStaff1,hd_Miscellaneous,openbal from FM_HeaderMaster where HeaderPK='" + header_id + "' and CollegeCode='" + clgcode + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectaccload, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //ddlmainacc.SelectedValue = acc_id;
                        txthdracr.Text = ds.Tables[0].Rows[0]["HeaderAcr"].ToString();
                        txtacchdr.Text = ds.Tables[0].Rows[0]["HeaderName"].ToString();
                        txt_openbal.Text = ds.Tables[0].Rows[0]["openbal"].ToString();
                        string pay1 = ds.Tables[0].Rows[0]["PayInchargeStaff1"].ToString();
                        string selq1 = "select convert(varchar(10),desig_code)+'-'+convert(varchar(10),desig_name) as staffname from desig_master where desig_code='" + pay1 + "' and collegeCode='" + clgcode + "'";
                        string designame1 = d2.GetFunction(selq1);
                        if (designame1.Trim() != "" && designame1.Trim() != "0")
                        {
                            txtpay1.Text = designame1;
                        }
                        else
                        {
                            txtpay1.Text = "";
                        }

                        string pay2 = ds.Tables[0].Rows[0]["PayInchargeStaff2"].ToString();
                        string selq2 = "select convert(varchar(10),desig_code)+'-'+convert(varchar(10),desig_name) as staffname from desig_master where desig_code='" + pay2 + "' and collegeCode='" + clgcode + "'";
                        string designame2 = d2.GetFunction(selq2);
                        if (designame2.Trim() != "" && designame2.Trim() != "0")
                        {
                            txtpay2.Text = designame2;
                        }
                        else
                        {
                            txtpay2.Text = "";
                        }

                        string pay3 = ds.Tables[0].Rows[0]["PayInchargeStaff3"].ToString();
                        string selq3 = "select convert(varchar(10),desig_code)+'-'+convert(varchar(10),desig_name) as staffname from desig_master where desig_code='" + pay3 + "' and collegeCode='" + clgcode + "'";
                        string designame3 = d2.GetFunction(selq3);
                        if (designame3.Trim() != "" && designame3.Trim() != "0")
                        {
                            txtpay3.Text = designame2;
                        }
                        else
                        {
                            txtpay3.Text = "";
                        }

                        string pay4 = ds.Tables[0].Rows[0]["RcptInchargeStaff1"].ToString();
                        string selq4 = "select convert(varchar(10),desig_code)+'-'+convert(varchar(10),desig_name) as staffname from desig_master where desig_code='" + pay4 + "' and collegeCode='" + clgcode + "'";
                        string designame4 = d2.GetFunction(selq4);
                        if (designame4.Trim() != "" && designame4.Trim() != "0")
                        {
                            txtrec.Text = designame4;
                        }
                        else
                        {
                            txtrec.Text = "";
                        }
                        txtpur.Text = ds.Tables[0].Rows[0]["Purpose"].ToString();
                        ddlmainacc.SelectedIndex = ddlmainacc.Items.IndexOf(ddlmainacc.Items.FindByValue(clgcode));
                        int miscel = 0;
                        int.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["hd_Miscellaneous"]), out miscel);
                        if (miscel == 1)
                            cbincmis.Checked = true;
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspreadnew, reportname);
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
            string degreedetails = "Account Header Report";
            string pagename = "Account Header.aspx";
            Printcontrol.loadspreaddetails(Fpspreadnew, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        poppernew.Visible = false;
    }
    protected void ddlmainacc_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnupdate_Click(object sender, EventArgs e)
    {
        string actrow = Fpspreadnew.ActiveSheetView.ActiveRow.ToString();
        string actcol = Fpspreadnew.ActiveSheetView.ActiveColumn.ToString();
        string clgid = Convert.ToString(Fpspreadnew.Sheets[0].Cells[Convert.ToInt32(actrow), 1].Tag);
        string acr = txthdracr.Text.ToString();
        string headcode = Convert.ToString(Fpspreadnew.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Tag);
        double openbal;
        //string newaccid = ddlmainacc.SelectedValue.ToString();
        //string accname = ddlmainacc.SelectedItem.Text.ToString();
        if (string.IsNullOrEmpty(txt_openbal.Text))
        {
            openbal = 0; // entry is null
        }
        else
        {
            openbal = Convert.ToDouble(txt_openbal.Text);
        }
        string accheader = txtacchdr.Text.ToString();
        string[] split = new string[2];
        string inc1 = "";
        string inc2 = "";
        string inc3 = "";
        string inc4 = "";
        string pay1 = txtpay1.Text.ToString();
        if (pay1.Trim() != "")
        {
            split = pay1.Split('-');
            inc1 = split[0];
        }
        else
        {
            inc1 = "";
        }
        string pay2 = txtpay2.Text.ToString();
        if (pay2.Trim() != "")
        {
            split = pay2.Split('-');
            inc2 = split[0];
        }
        else
        {
            inc2 = "";
        }

        string pay3 = txtpay3.Text.ToString();
        if (pay3.Trim() != "")
        {
            split = pay3.Split('-');
            inc3 = split[0];
        }
        else
        {
            inc3 = "";
        }
        string receive = txtrec.Text.ToString();
        if (receive.Trim() != "")
        {
            split = receive.Split('-');
            inc4 = split[0];
        }
        else
        {
            inc4 = "";
        }
        string purpose = txtpur.Text.ToString();
        //txt_openbal.Text = openbal;
        accheader = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(accheader);
        int miscell = 0;
        if (cbincmis.Checked)
            miscell = 1;
        string selquery = "select HeaderName from FM_HeaderMaster where HeaderName='" + accheader + "' and HeaderPK not in('" + headcode + "') and CollegeCode='" + collegestat0 + "'";
        selquery = selquery + " select HeaderAcr from FM_HeaderMaster where HeaderAcr='" + acr + "' and HeaderPK not in('" + headcode + "') and CollegeCode='" + collegestat0 + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(selquery, "Text");
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Header Name already Exist!";
            }
            else if (ds.Tables[1].Rows.Count > 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Header Acronym already Exist!";
            }
            else
            {
                string updatequery = "Update FM_HeaderMaster set HeaderName='" + accheader + "',HeaderAcr='" + acr.ToUpper() + "',PayInchargeStaff1='" + inc1 + "',PayInchargeStaff2='" + inc2 + "',PayInchargeStaff3='" + inc3 + "',RcptInchargeStaff1='" + inc4 + "',Purpose='" + purpose + "',CollegeCode='" + collegestat0 + "',hd_Miscellaneous='" + miscell + "',openbal='" + openbal + "' where HeaderPK='" + headcode + "' and CollegeCode='" + clgid + "'";
                int count = d2.update_method_wo_parameter(updatequery, "Text");
                if (count > 0)
                {
                    bindacctname();
                    btngo_Click(sender, e);
                    poppernew.Visible = false;
                    cbincmis.Checked = false;
                    imgdiv2.Visible = true;
                    lbl_alert.Visible = true;

                    lbl_alert.Text = "Updated Successfully";
                }
                else
                {

                }
            }
        }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        imgdiv1.Visible = true;
        lblalert.Visible = true;
        lblalert.Text = "Do you want to delete this record?";
    }

    protected void btnyes_Click(object sender, EventArgs e)
    {
        try
        {
            string exstrow = Fpspreadnew.ActiveSheetView.ActiveRow.ToString();
            string exstcol = Fpspreadnew.ActiveSheetView.ActiveColumn.ToString();
            //string accid = ddlmainacc.SelectedValue;
            string headcode = Convert.ToString(Fpspreadnew.Sheets[0].Cells[Convert.ToInt32(exstrow), 2].Tag);

            string selquery = "";
            selquery = "select * from FM_LedgerMaster where HeaderFK='" + headcode + "'";
            selquery = selquery + " select * from FM_FinCodeSettings where HeaderFK='" + headcode + "'";
            selquery = selquery + " Select * from FT_FeeAllot where HeaderFk='" + headcode + "'";
            selquery = selquery + " Select * from FT_FeeAllotDegree where HeaderFK='" + headcode + "'";
            selquery = selquery + " select * from FT_FinDailyTransaction where HeaderFK='" + headcode + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selquery, "Text");
            if (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0 || ds.Tables[2].Rows.Count > 0 || ds.Tables[3].Rows.Count > 0 || ds.Tables[4].Rows.Count > 0)
            {
                imgdiv1.Visible = false;
                lblalert.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "You can't delete this record";
            }
            else
            {
                string delquery = "delete from FM_HeaderMaster where HeaderPK='" + headcode + "'";
                int delcount = d2.update_method_wo_parameter(delquery, "Text");
                if (delcount > 0)
                {
                    bindacctname();
                    btngo_Click(sender, e);
                    poppernew.Visible = false;
                    imgdiv1.Visible = false;
                    imgdiv2.Visible = true;
                    lblalert.Visible = false;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Deleted Successfully";
                }
            }
        }
        catch
        {
        }
    }
    protected void btnno_Click(object sender, EventArgs e)
    {
        imgdiv1.Visible = false;
        lblalert.Visible = false;
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            //string accname = ddlmainacc.SelectedItem.Text;
            string acronym = txthdracr.Text.ToString();
            string headername = txtacchdr.Text.ToString();
            string pay1 = "";
            string pay2 = "";
            string pay3 = "";
            string pay4 = "";
            string purpose = txtpur.Text.ToString();
            double openbal;
            try
            {
                pay1 = txtpay1.Text.Split('-')[0];
                pay2 = txtpay2.Text.Split('-')[0];
                pay3 = txtpay3.Text.Split('-')[0];
                pay4 = txtrec.Text.Split('-')[0];
            }
            catch { }
            //string acctid = ddlmainacc.SelectedValue;
            if (string.IsNullOrEmpty(txt_openbal.Text))
            {
                openbal = 0; // entry is null
            }
            else
            {
                openbal = Convert.ToDouble(txt_openbal.Text);
            }
            headername = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(headername);
            //if (ddlmainacc.SelectedItem.Text == "Select")
            //{
            //    lblmainerr.Visible = true;
            //}
            int miscell = 0;
            if (cbincmis.Checked)
                miscell = 1;
            string chk = "select HeaderName from FM_HeaderMaster where HeaderName='" + headername + "' and collegecode='" + collegestat0 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(chk, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Header Name already exist!";
            }
            else
            {
                string insertacc = "Insert into FM_HeaderMaster(HeaderAcr,HeaderName,Purpose,CollegeCode,PayInchargeStaff1,PayInchargeStaff2,PayInchargeStaff3,RcptInchargeStaff1,hd_Miscellaneous,openbal)";
                insertacc = insertacc + "values('" + acronym.Trim().ToUpper() + "','" + headername.Trim() + "','" + purpose.Trim() + "','" + collegestat0 + "','" + pay1.Trim() + "','" + pay2.Trim() + "','" + pay3.Trim() + "','" + pay4.Trim() + "','" + miscell + "','" + openbal + "')";
                int accins = d2.update_method_wo_parameter(insertacc, "Text");
                bindacctname();
                btngo_Click(sender, e);
                clear();
                poppernew.Visible = true;
                cbincmis.Checked = false;
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Saved Successfully";
            }
        }
        catch
        {
        }
    }
    protected void btnexit_Click(object sender, EventArgs e)
    {
        poppernew.Visible = false;
    }
    protected void btnpoppay1_Click(object sender, EventArgs e)
    {
        try
        {
            popper1.Visible = true;
            ddlsearch1.SelectedIndex = 0;
            Label1.Text = "Search By Name";
            txtsearch1.Visible = true;
            txtsearch1c.Visible = false;
            txtsearch1.Text = "";
            if (sesscolcode != null)
            {
                string selq = "";
                selq = "select desig_code,desig_name  from desig_master where collegeCode ='" + collegestat0 + "' order by priority";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fpspreadpay1.Sheets[0].RowCount = 0;
                    Fpspreadpay1.Sheets[0].ColumnCount = 0;
                    Fpspreadpay1.CommandBar.Visible = false;
                    Fpspreadpay1.Sheets[0].AutoPostBack = true;
                    Fpspreadpay1.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspreadpay1.Sheets[0].RowHeader.Visible = false;
                    Fpspreadpay1.Sheets[0].ColumnCount = 3;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspreadpay1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    FarPoint.Web.Spread.TextCellType txttype = new FarPoint.Web.Spread.TextCellType();

                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Designation Code";
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Designation Name";
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        Fpspreadpay1.Sheets[0].RowCount++;
                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspreadpay1.Columns[0].Width = 50;

                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 1].CellType = txttype;
                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["desig_code"]);
                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["desig_name"]);
                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    }
                    Fpspreadpay1.Visible = true;
                    div2.Visible = true;
                    lblerr1.Visible = false;
                    Fpspreadpay1.Sheets[0].PageSize = Fpspreadpay1.Sheets[0].RowCount;
                }
            }
        }
        catch
        {
        }
    }
    protected void btnpoppay2_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtpay1.Text.Trim() != "")
            {
                popper2.Visible = true;
                ddlsearch2.SelectedIndex = 0;
                Label2.Text = "Search By Name";
                txtsearch2.Visible = true;
                txtsearch2c.Visible = false;
                txtsearch2.Text = "";
                if (sesscolcode != null)
                {
                    string selq = "";
                    selq = "select desig_code,desig_name  from desig_master where collegeCode ='" + collegestat0 + "' order by priority";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selq, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Fpspreadpay2.Sheets[0].RowCount = 0;
                        Fpspreadpay2.Sheets[0].ColumnCount = 0;
                        Fpspreadpay2.CommandBar.Visible = false;
                        Fpspreadpay2.Sheets[0].AutoPostBack = true;
                        Fpspreadpay2.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspreadpay2.Sheets[0].RowHeader.Visible = false;
                        Fpspreadpay2.Sheets[0].ColumnCount = 3;

                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        Fpspreadpay2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FarPoint.Web.Spread.TextCellType txttype = new FarPoint.Web.Spread.TextCellType();

                        Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                        Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Designation Code";
                        Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                        Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Designation Name";
                        Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            Fpspreadpay2.Sheets[0].RowCount++;
                            Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                            Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fpspreadpay2.Columns[0].Width = 50;

                            Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 1].CellType = txttype;
                            Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["desig_code"]);
                            Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                            Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["desig_name"]);
                            Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        }
                        Fpspreadpay2.Visible = true;
                        div5.Visible = true;
                        lblerr2.Visible = false;
                        Fpspreadpay2.Sheets[0].PageSize = Fpspreadpay2.Sheets[0].RowCount;
                    }
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Please select Payment Incharge1!";
            }
        }
        catch
        {
        }
    }
    protected void btnpoppay3_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtpay2.Text.Trim() != "")
            {
                popper3.Visible = true;
                ddlsearch3.SelectedIndex = 0;
                Label3.Text = "Search By Name";
                txtsearch3.Visible = true;
                txtsearch3c.Visible = false;
                txtsearch3.Text = "";
                if (sesscolcode != null)
                {
                    string selq = "";
                    selq = "select desig_code,desig_name  from desig_master where collegeCode ='" + collegestat0 + "' order by priority";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selq, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Fpspreadpay3.Sheets[0].RowCount = 0;
                        Fpspreadpay3.Sheets[0].ColumnCount = 0;
                        Fpspreadpay3.CommandBar.Visible = false;
                        Fpspreadpay3.Sheets[0].AutoPostBack = true;
                        Fpspreadpay3.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspreadpay3.Sheets[0].RowHeader.Visible = false;
                        Fpspreadpay3.Sheets[0].ColumnCount = 3;

                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        Fpspreadpay3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FarPoint.Web.Spread.TextCellType tctype = new FarPoint.Web.Spread.TextCellType();

                        Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                        Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Designation Code";
                        Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                        Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Designation Name";
                        Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            Fpspreadpay3.Sheets[0].RowCount++;
                            Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                            Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fpspreadpay3.Columns[0].Width = 50;

                            Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 1].CellType = tctype;
                            Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["desig_code"]);
                            Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                            Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["desig_name"]);
                            Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        }
                        Fpspreadpay3.Visible = true;
                        div7.Visible = true;
                        lblerr3.Visible = false;
                        Fpspreadpay3.Sheets[0].PageSize = Fpspreadpay3.Sheets[0].RowCount;
                    }
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Please select Payment Incharge2!";
            }
        }
        catch
        {
        }
    }
    protected void btnrec_Click(object sender, EventArgs e)
    {
        try
        {
            //if (txtpay3.Text.Trim() != "")
            //{
            popper4.Visible = true;
            ddlsearch4.SelectedIndex = 0;
            Label4.Text = "Search By Name";
            txtsearch4.Visible = true;
            txtsearch4c.Visible = false;
            txtsearch4.Text = "";
            if (sesscolcode != null)
            {
                string selq = "";
                selq = "select desig_code,desig_name  from desig_master where collegeCode ='" + collegestat0 + "' order by priority";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fpspreadpay4.Sheets[0].RowCount = 0;
                    Fpspreadpay4.Sheets[0].ColumnCount = 0;
                    Fpspreadpay4.CommandBar.Visible = false;
                    Fpspreadpay4.Sheets[0].AutoPostBack = true;
                    Fpspreadpay4.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspreadpay4.Sheets[0].RowHeader.Visible = false;
                    Fpspreadpay4.Sheets[0].ColumnCount = 3;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspreadpay4.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    FarPoint.Web.Spread.TextCellType txttype = new FarPoint.Web.Spread.TextCellType();

                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Designation Code";
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Designation Name";
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        Fpspreadpay4.Sheets[0].RowCount++;
                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspreadpay4.Columns[0].Width = 50;

                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 1].CellType = txttype;
                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["desig_code"]);
                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["desig_name"]);
                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    }
                    Fpspreadpay4.Visible = true;
                    div9.Visible = true;
                    lblerr4.Visible = false;
                    Fpspreadpay4.Sheets[0].PageSize = Fpspreadpay4.Sheets[0].RowCount;
                }
            }
            //}
            //else
            //{
            //    imgdiv2.Visible = true;
            //    lbl_alert.Visible = true;
            //    lbl_alert.Text = "Please select Payment Incharge3!";
            //}
        }
        catch
        {
        }
    }
    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {
        popper1.Visible = false;
    }
    protected void Cellpay1_Click(object sender, EventArgs e)
    {
        try
        {
            spreadclick1 = true;
        }
        catch
        {

        }

    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct desig_name from desig_master WHERE desig_name like '" + prefixText + "%' and collegeCode=" + collegestat + " ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["desig_name"].ToString());
            }
        }
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetdesCode(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct desig_code from desig_master WHERE desig_code like '" + prefixText + "%' and collegeCode=" + collegestat + " ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["desig_code"].ToString());
            }
        }
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getheader(string prefixText)
    {
        DAccess2 da = new DAccess2();
        DataSet das = new DataSet();
        List<string> lstheader = new List<string>();
        string getheader = "select distinct HeaderName from FM_HeaderMaster where HeaderName like '" + prefixText + "%'  AND COLLEGECODE=" + collegestat
 + "";
        das = da.select_method_wo_parameter(getheader, "Text");
        if (das.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < das.Tables[0].Rows.Count; i++)
            {
                lstheader.Add(das.Tables[0].Rows[i]["HeaderName"].ToString());
            }
        }
        return lstheader;
    }

    protected void Fpspreadpay1_render(object sender, EventArgs e)
    {
        try
        {
            if (spreadclick1 == true)
            {
                poppernew.Visible = true;
                string activerow = "";
                string activecol = "";
                activerow = Fpspreadpay1.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspreadpay1.ActiveSheetView.ActiveColumn.ToString();

                string designame = Convert.ToString(Fpspreadpay1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text) + "-" + Convert.ToString(Fpspreadpay1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);

                if (activerow.Trim() != "")
                {
                    txtpay1.Text = designame;
                    popper1.Visible = false;
                }
            }
        }
        catch
        {
        }
    }
    protected void btnok1_Click(object sender, EventArgs e)
    {

    }
    protected void btncancel1_Click(object sender, EventArgs e)
    {
        popper1.Visible = false;
    }
    protected void imagebtnpopclose5_Click(object sender, ImageClickEventArgs e)
    {
        popper4.Visible = false;
    }
    protected void imagebtnpopclose4_Click(object sender, ImageClickEventArgs e)
    {
        popper3.Visible = false;
    }
    protected void imagebtnpopclose3_Click(object sender, EventArgs e)
    {
        popper2.Visible = false;
    }
    protected void Cellpay2_Click(object sender, EventArgs e)
    {
        try
        {
            spreadclick2 = true;
        }
        catch
        {
        }
    }
    protected void Fpspreadpay2_render(object sender, EventArgs e)
    {
        try
        {
            if (spreadclick2 == true)
            {
                poppernew.Visible = true;
                string activerow = "";
                string activecol = "";
                activerow = Fpspreadpay2.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspreadpay2.ActiveSheetView.ActiveColumn.ToString();

                string designame = Convert.ToString(Fpspreadpay2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text) + "-" + Convert.ToString(Fpspreadpay2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);

                if (activerow.Trim() != "")
                {
                    txtpay2.Text = designame;
                    popper2.Visible = false;
                }
            }
        }
        catch
        {
        }
    }
    protected void btnok2_Click(object sender, EventArgs e)
    {

    }
    protected void btncancel2_Click(object sender, EventArgs e)
    {
        popper2.Visible = false;
    }
    protected void Cellpay3_Click(object sender, EventArgs e)
    {
        try
        {
            spreadclick3 = true;
        }
        catch
        {
        }
    }
    protected void Fpspreadpay3_render(object sender, EventArgs e)
    {
        try
        {
            if (spreadclick3 == true)
            {
                poppernew.Visible = true;
                string activerow = "";
                string activecol = "";
                activerow = Fpspreadpay3.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspreadpay3.ActiveSheetView.ActiveColumn.ToString();

                string designame = Convert.ToString(Fpspreadpay3.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text) + "-" + Convert.ToString(Fpspreadpay3.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);

                if (activerow.Trim() != "")
                {
                    txtpay3.Text = designame;
                    popper3.Visible = false;
                }
            }
        }
        catch
        {
        }
    }
    protected void btnok3_Click(object sender, EventArgs e)
    {

    }
    protected void btncancel3_Click(object sender, EventArgs e)
    {
        popper3.Visible = false;
    }
    protected void Cellpay4_Click(object sender, EventArgs e)
    {
        try
        {
            spreadclick4 = true;
        }
        catch
        {
        }
    }
    protected void Fpspreadpay4_render(object sender, EventArgs e)
    {
        try
        {
            if (spreadclick4 == true)
            {
                poppernew.Visible = true;
                string activerow = "";
                string activecol = "";
                activerow = Fpspreadpay4.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspreadpay4.ActiveSheetView.ActiveColumn.ToString();

                string designame = Convert.ToString(Fpspreadpay4.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text) + "-" + Convert.ToString(Fpspreadpay4.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);

                if (activerow.Trim() != "")
                {
                    txtrec.Text = designame;
                    popper4.Visible = false;
                }
            }
        }
        catch
        {
        }
    }
    protected void btnok4_Click(object sender, EventArgs e)
    {

    }
    protected void btncancel4_Click(object sender, EventArgs e)
    {
        popper4.Visible = false;
    }
    protected void btngo1_Click(object sender, EventArgs e)
    {
        try
        {
            if (sesscolcode != null)
            {
                string selq = "";
                if (txtsearch1.Text.Trim() != "")
                {
                    selq = "select desig_code,desig_name from desig_master where desig_name='" + Convert.ToString(txtsearch1.Text) + "' and collegeCode='" + collegestat0 + "'";
                }
                else if (txtsearch1c.Text.Trim() != "")
                {
                    selq = "select desig_code,desig_name from desig_master where desig_code='" + Convert.ToString(txtsearch1c.Text) + "' and collegeCode='" + collegestat0 + "'";
                }
                else
                {
                    selq = "select desig_code,desig_name  from desig_master where collegeCode ='" + collegestat0 + "' order by priority";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fpspreadpay1.Sheets[0].RowCount = 0;
                    Fpspreadpay1.Sheets[0].ColumnCount = 0;
                    Fpspreadpay1.CommandBar.Visible = false;
                    Fpspreadpay1.Sheets[0].AutoPostBack = true;
                    Fpspreadpay1.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspreadpay1.Sheets[0].RowHeader.Visible = false;
                    Fpspreadpay1.Sheets[0].ColumnCount = 3;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspreadpay1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    FarPoint.Web.Spread.TextCellType txttype = new FarPoint.Web.Spread.TextCellType();

                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Designation Code";
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Designation Name";
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        Fpspreadpay1.Sheets[0].RowCount++;
                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 1].CellType = txttype;
                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["desig_code"]);
                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["desig_name"]);
                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    }
                    Fpspreadpay1.Visible = true;
                    div2.Visible = true;
                    lblerr1.Visible = false;
                    Fpspreadpay1.Sheets[0].PageSize = Fpspreadpay1.Sheets[0].RowCount;
                    txtsearch1.Text = "";
                    txtsearch1c.Text = "";
                }
            }
        }
        catch
        {
        }
    }
    protected void btngo2_Click(object sender, EventArgs e)
    {
        try
        {
            if (sesscolcode != null)
            {
                string selq = "";
                if (txtsearch2.Text.Trim() != "")
                {
                    selq = "select desig_code,desig_name from desig_master where desig_name='" + Convert.ToString(txtsearch2.Text) + "' and collegeCode='" + collegestat0 + "'";
                }
                else if (txtsearch2c.Text.Trim() != "")
                {
                    selq = "select desig_code,desig_name from desig_master where desig_code='" + Convert.ToString(txtsearch2c.Text) + "' and collegeCode='" + collegestat0 + "'";
                }
                else
                {
                    selq = "select desig_code,desig_name  from desig_master where collegeCode ='" + collegestat0 + "' order by priority";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fpspreadpay2.Sheets[0].RowCount = 0;
                    Fpspreadpay2.Sheets[0].ColumnCount = 0;
                    Fpspreadpay2.CommandBar.Visible = false;
                    Fpspreadpay2.Sheets[0].AutoPostBack = true;
                    Fpspreadpay2.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspreadpay2.Sheets[0].RowHeader.Visible = false;
                    Fpspreadpay2.Sheets[0].ColumnCount = 3;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspreadpay2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    FarPoint.Web.Spread.TextCellType txttype = new FarPoint.Web.Spread.TextCellType();

                    Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Designation Code";
                    Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                    Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Designation Name";
                    Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspreadpay2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        Fpspreadpay2.Sheets[0].RowCount++;
                        Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 1].CellType = txttype;
                        Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["desig_code"]);
                        Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["desig_name"]);
                        Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspreadpay2.Sheets[0].Cells[Fpspreadpay2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    }
                    Fpspreadpay2.Visible = true;
                    div5.Visible = true;
                    lblerr2.Visible = false;
                    Fpspreadpay2.Sheets[0].PageSize = Fpspreadpay2.Sheets[0].RowCount;
                    txtsearch2.Text = "";
                    txtsearch2c.Text = "";
                }
            }
        }
        catch
        {
        }
    }
    protected void btngo3_Click(object sender, EventArgs e)
    {
        try
        {
            if (sesscolcode != null)
            {
                string selq = "";
                if (txtsearch3.Text.Trim() != "")
                {
                    selq = "select desig_code,desig_name from desig_master where desig_name='" + Convert.ToString(txtsearch3.Text) + "' and collegeCode='" + collegestat0 + "'";
                }
                else if (txtsearch3c.Text.Trim() != "")
                {
                    selq = "select desig_code,desig_name from desig_master where desig_code='" + Convert.ToString(txtsearch3c.Text) + "' and collegeCode='" + collegestat0 + "'";
                }
                else
                {
                    selq = "select desig_code,desig_name  from desig_master where collegeCode ='" + collegestat0 + "' order by priority";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fpspreadpay3.Sheets[0].RowCount = 0;
                    Fpspreadpay3.Sheets[0].ColumnCount = 0;
                    Fpspreadpay3.CommandBar.Visible = false;
                    Fpspreadpay3.Sheets[0].AutoPostBack = true;
                    Fpspreadpay3.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspreadpay3.Sheets[0].RowHeader.Visible = false;
                    Fpspreadpay3.Sheets[0].ColumnCount = 3;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspreadpay3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    FarPoint.Web.Spread.TextCellType txttype = new FarPoint.Web.Spread.TextCellType();

                    Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Designation Code";
                    Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                    Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Designation Name";
                    Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspreadpay3.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        Fpspreadpay3.Sheets[0].RowCount++;
                        Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 1].CellType = txttype;
                        Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["desig_code"]);
                        Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["desig_name"]);
                        Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspreadpay3.Sheets[0].Cells[Fpspreadpay3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    }
                    Fpspreadpay3.Visible = true;
                    div7.Visible = true;
                    lblerr3.Visible = false;
                    Fpspreadpay3.Sheets[0].PageSize = Fpspreadpay3.Sheets[0].RowCount;
                    txtsearch3.Text = "";
                    txtsearch3c.Text = "";
                }
            }
        }
        catch
        {
        }
    }
    protected void btngo4_Click(object sender, EventArgs e)
    {
        try
        {
            if (sesscolcode != null)
            {
                string selq = "";
                if (txtsearch4.Text.Trim() != "")
                {
                    selq = "select desig_code,desig_name from desig_master where desig_name='" + Convert.ToString(txtsearch4.Text) + "' and collegeCode='" + collegestat0 + "'";
                }
                else if (txtsearch4c.Text.Trim() != "")
                {
                    selq = "select desig_code,desig_name from desig_master where desig_code='" + Convert.ToString(txtsearch4c.Text) + "' and collegeCode='" + collegestat0 + "'";
                }
                else
                {
                    selq = "select desig_code,desig_name  from desig_master where collegeCode ='" + collegestat0 + "' order by priority";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fpspreadpay4.Sheets[0].RowCount = 0;
                    Fpspreadpay4.Sheets[0].ColumnCount = 0;
                    Fpspreadpay4.CommandBar.Visible = false;
                    Fpspreadpay4.Sheets[0].AutoPostBack = true;
                    Fpspreadpay4.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspreadpay4.Sheets[0].RowHeader.Visible = false;
                    Fpspreadpay4.Sheets[0].ColumnCount = 3;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspreadpay4.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    FarPoint.Web.Spread.TextCellType txttype = new FarPoint.Web.Spread.TextCellType();

                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Designation Code";
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Designation Name";
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspreadpay4.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        Fpspreadpay4.Sheets[0].RowCount++;
                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 1].CellType = txttype;
                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["desig_code"]);
                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["desig_name"]);
                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspreadpay4.Sheets[0].Cells[Fpspreadpay4.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    }
                    Fpspreadpay4.Visible = true;
                    div9.Visible = true;
                    lblerr4.Visible = false;
                    Fpspreadpay4.Sheets[0].PageSize = Fpspreadpay4.Sheets[0].RowCount;
                    txtsearch4.Text = "";
                    txtsearch4c.Text = "";
                }
            }
        }
        catch
        {
        }
    }
    protected void clear()
    {
        //ddlmainacc.SelectedIndex = 0;
        txthdracr.Text = "";
        txtacchdr.Text = "";
        txtpay1.Text = "";
        txtpay2.Text = "";
        txtpay3.Text = "";
        txtrec.Text = "";
        txtpur.Text = "";
        txt_openbal.Text = "";
    }
    //protected void bindaccname()
    //{
    //    try
    //    {
    //        ds.Clear();
    //        ddlmainacc.Items.Clear();
    //        string slctquery = "select  A.acct_id,acct_name from acctinfo A,account_info AC WHERE A.acct_id = AC.acct_id order by A.acct_id";
    //        ds = d2.select_method_wo_parameter(slctquery, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ddlmainacc.DataSource = ds;
    //            ddlmainacc.DataTextField = "acct_name";
    //            ddlmainacc.DataValueField = "acct_id";
    //            ddlmainacc.DataBind();
    //            ddlmainacc.Items.Insert(0, "Select");
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}

    protected void bindcollege()
    {
        try
        {
            string strUser = d2.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
            ds.Clear();
            ddlclgname.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlclgname.DataSource = ds;
                ddlclgname.DataTextField = "collname";
                ddlclgname.DataValueField = "college_code";
                ddlclgname.DataBind();
            }
        }
        catch
        {
        }
    }

    protected void bindloadcol()
    {
        try
        {
            string strUser = d2.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
            ds.Clear();
            ddlmainacc.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlmainacc.DataSource = ds;
                ddlmainacc.DataTextField = "collname";
                ddlmainacc.DataValueField = "college_code";
                ddlmainacc.DataBind();
            }
        }
        catch
        {

        }
    }

    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();

        lbl.Add(lblcol);
        fields.Add(0);
        lbl.Add(lblMainacc);
        fields.Add(0);

        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    protected DataSet loadDetails()
    {
        DataSet dsload = new DataSet();
        try
        {
            string SelectQ = string.Empty;
            string hdFK = Convert.ToString(getCblSelectedValue(cblacctname));
            if (!string.IsNullOrEmpty(hdFK))
            {
                SelectQ = "select HeaderAcr,HeaderName,HeaderPK,Purpose,CollegeCode,PayInchargeStaff1,PayInchargeStaff2,PayInchargeStaff3,RcptInchargeStaff1,hd_priority from FM_HeaderMaster where HeaderPK in('" + hdFK + "') and CollegeCode='" + ddlclgname.SelectedValue + "'";
            }
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelectQ, "Text");
        }
        catch { }
        return dsload;
    }

    protected void loadSpread(DataSet ds)
    {
        try
        {
            #region design
            spreadPriority.Sheets[0].RowCount = 0;
            spreadPriority.Sheets[0].ColumnCount = 0;
            spreadPriority.Sheets[0].ColumnHeader.RowCount = 1;
            spreadPriority.CommandBar.Visible = false;
            spreadPriority.Sheets[0].AutoPostBack = false;

            spreadPriority.Sheets[0].RowHeader.Visible = false;
            spreadPriority.Sheets[0].ColumnCount = 5;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            spreadPriority.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            spreadPriority.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            spreadPriority.Sheets[0].Columns[0].Width = 50;

            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Header Name";
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadPriority.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            spreadPriority.Sheets[0].Columns[1].Width = 350;

            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Acronym";
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            spreadPriority.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            spreadPriority.Sheets[0].Columns[2].Width = 150;

            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Set Priority";
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            spreadPriority.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;

            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Priority";
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            spreadPriority.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            spreadPriority.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
            cb.AutoPostBack = true;
            #endregion

            #region value
            int height = 0;
            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                spreadPriority.Sheets[0].RowCount++;
                height += 35;
                spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[row]["CollegeCode"]);
                spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["HeaderName"]);
                spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["headerpk"]);
                spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["HeaderAcr"]);
                //Fpspreadnew.Sheets[0].Cells[Fpspreadnew.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Degree_Code"]);
                spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 3].CellType = cb;
                spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                double deptPr = 0;
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["hd_priority"]), out deptPr);
                if (deptPr != 0)
                {
                    spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 3].Locked = true;
                    spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 3].Value = 1;
                }
                else
                    spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 3].Locked = false;
                spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(deptPr);
                spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                spreadPriority.Sheets[0].Cells[spreadPriority.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
            }
            spreadPriority.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            spreadPriority.Visible = true;
            rportprint.Visible = true;
            div1.Visible = true;
            spreadPriority.ShowHeaderSelection = false;
            spreadPriority.Sheets[0].PageSize = spreadPriority.Sheets[0].RowCount;
            spreadPriority.SaveChanges();
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            spreadPriority.Height = height;
            divpriority.Visible = true;
            #endregion
        }
        catch { }
    }

    protected void Cell_Click(object sender, EventArgs e)
    {
        check = true;
    }
    protected void spreadPriority_render(object sender, EventArgs e)
    {
        if (flag_true == true)
        {
            spreadPriority.SaveChanges();
            string activrow = "";
            activrow = spreadPriority.Sheets[0].ActiveRow.ToString();
            string activecol = spreadPriority.Sheets[0].ActiveColumn.ToString();
            int actcol = Convert.ToInt16(activecol);
            int hy_order = 0;
            for (int i = 0; i <= Convert.ToInt16(spreadPriority.Sheets[0].RowCount) - 1; i++)
            {
                int isval = Convert.ToInt32(spreadPriority.Sheets[0].Cells[i, actcol].Value);
                if (isval == 1)
                {

                    hy_order++;
                    spreadPriority.Sheets[0].Cells[Convert.ToInt32(activrow), actcol].Locked = true;
                }
            }
            spreadPriority.Sheets[0].Cells[Convert.ToInt32(activrow), actcol + 1].Text = hy_order.ToString();
        }
    }
    protected void spreadPriority_ButtonCommand(object sender, EventArgs e)
    {
        spreadPriority.SaveChanges();
        string activerow = spreadPriority.ActiveSheetView.ActiveRow.ToString();
        string activecol = spreadPriority.ActiveSheetView.ActiveColumn.ToString();
        if (activecol == "3")
        {
            int act1 = Convert.ToInt32(activerow);
            int act2 = Convert.ToInt16(activecol);
            if (spreadPriority.Sheets[0].Cells[act1, act2].Value.ToString() == "1")
            {
                flag_true = true;
                spreadPriority.Sheets[0].Cells[act1, act2 + 1].Text = "";
            }
            else
            {
                flag_true = false;
            }
        }
        spreadPriority.SaveChanges();

    }

    protected void btnSetPriority_Click(object sender, EventArgs e)
    {
        try
        {
            bool check = false;
            int insQ2 = d2.update_method_wo_parameter("update fm_headermaster set hd_priority=null  where collegecode='" + ddlclgname.SelectedItem.Value + "'", "Text");
            if (spreadPriority.Sheets[0].Rows.Count > 0)
            {
                for (int i = 0; i < spreadPriority.Sheets[0].Rows.Count; i++)
                {
                    string priority = Convert.ToString(spreadPriority.Sheets[0].Cells[i, 4].Text.Trim());
                    string hdPK = Convert.ToString(spreadPriority.Sheets[0].Cells[i, 1].Tag);
                    string clgcode = Convert.ToString(spreadPriority.Sheets[0].Cells[i, 0].Tag);
                    if (priority.Trim() != "" && priority.Trim() != "0")
                    {
                        string insQ = "update fm_headermaster set hd_priority='" + priority + "' where headerpk='" + hdPK + "'  and collegecode='" + clgcode + "'";
                        int upd = d2.update_method_wo_parameter(insQ, "Text");
                        check = true;
                    }
                }
                if (check)
                {
                    lbl_alert.Text = "Priority Assigned";
                    imgdiv2.Visible = true;
                }
                else
                {
                    lbl_alert.Text = "Priority Not Assigned";
                    imgdiv2.Visible = true;
                }
            }
            else
            {
                lbl_alert.Text = "Priority Not Assigned";
                imgdiv2.Visible = true;
            }
        }
        catch { lbl_alert.Text = "Priority Not Assigned"; imgdiv2.Visible = true; }
    }
    protected void btnResetPriority_Click(object sender, EventArgs e)
    {
        try
        {
            bool check = false;
            if (spreadPriority.Sheets[0].Rows.Count > 0)
            {
                for (int i = 0; i < spreadPriority.Sheets[0].Rows.Count; i++)
                {
                    spreadPriority.Sheets[0].Cells[i, 3].Locked = false;
                    spreadPriority.Sheets[0].Cells[i, 3].Value = 0;
                    spreadPriority.Sheets[0].Cells[i, 4].Text = "";
                    check = true;
                }
            }
            spreadPriority.SaveChanges();
            if (check)
            {
                lbl_alert.Text = "Reset Successfully";
                imgdiv2.Visible = true;
            }
        }
        catch { }
    }
    protected void cbhdpriority_Changed(object sender, EventArgs e)
    {
        if (cbhdpriority.Checked)
        {
            Fpspreadnew.Visible = false;
            DataSet dsVal = loadDetails();
            if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
            {
                loadSpread(dsVal);
            }
            else
            {
                lblerror.Text = "Please select any header!";
                lblerror.Visible = true;
            }
        }
        else
        {
            spreadPriority.Visible = false;
            divpriority.Visible = false;
            loadMainDetails();
        }
    }

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }
    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = "";
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }
    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = "";
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch { }
    }

    #endregion


    // last modified 17.04.2017 sudhagar
}