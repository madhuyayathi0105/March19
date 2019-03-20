using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Drawing;

public partial class ContraWithdrawReport : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet dsload = new DataSet();
    DataView dv = new DataView();
    DataView dv1 = new DataView();
    int count = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
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
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();

            if (!IsPostBack)
            {
                loadheaderandledger();
                ledgerload();
                loadbank();
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                Txt_Todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                Txt_Todate.Attributes.Add("readonly", "readonly");
                rbpety_OnCheckedChanged(sender, e);
            }
        }
        catch (Exception ex) { }
    }
    protected void lb3_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("default.aspx", false);
        }
        catch
        {
        }
    }

    #region headerandledger
    public void loadheaderandledger()
    {
        try
        {
            chkl_studhed.Items.Clear();
            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + "  ";

            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chkl_studhed.DataSource = ds;
                chkl_studhed.DataTextField = "HeaderName";
                chkl_studhed.DataValueField = "HeaderPK";
                chkl_studhed.DataBind();
                for (int i = 0; i < chkl_studhed.Items.Count; i++)
                {
                    chkl_studhed.Items[i].Selected = true;
                }
                txt_studhed.Text = "Header(" + chkl_studhed.Items.Count + ")";
                chk_studhed.Checked = true;
            }
            ledgerload();
        }
        catch
        {
        }
    }
    public void ledgerload()
    {
        try
        {

            chkl_studled.Items.Clear();
            string hed = "";
            for (int i = 0; i < chkl_studhed.Items.Count; i++)
            {
                if (chkl_studhed.Items[i].Selected == true)
                {
                    if (hed == "")
                    {
                        hed = chkl_studhed.Items[i].Value.ToString();
                    }
                    else
                    {
                        hed = hed + "','" + "" + chkl_studhed.Items[i].Value.ToString() + "";
                    }
                }
            }


            string query1 = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='1' and L.CollegeCode = " + collegecode1 + "  and L.HeaderFK in('" + hed + "')";
            //  string query1 = "select LedgerPK,LedgerName from FM_LedgerMaster  where CollegeCode='" + collegecode1 + "' and HeaderFK in ('" + hed + "') order by isnull(priority,1000), ledgerName asc ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chkl_studled.DataSource = ds;
                chkl_studled.DataTextField = "LedgerName";
                chkl_studled.DataValueField = "LedgerPK";
                chkl_studled.DataBind();
                for (int i = 0; i < chkl_studled.Items.Count; i++)
                {
                    chkl_studled.Items[i].Selected = true;
                }
                txt_studled.Text = "Ledger(" + chkl_studled.Items.Count + ")";
                chk_studled.Checked = true; ;

            }
            else
            {
                for (int i = 0; i < chkl_studled.Items.Count; i++)
                {
                    chkl_studled.Items[i].Selected = false;
                }
                txt_studled.Text = "--Select--";
                chk_studled.Checked = false; ;
            }

        }
        catch
        {
        }
    }
    public void chk_studhed_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string header = "";
            if (chk_studhed.Checked == true)
            {
                for (int i = 0; i < chkl_studhed.Items.Count; i++)
                {
                    chkl_studhed.Items[i].Selected = true;

                }
                if (chkl_studhed.Items.Count == 1)
                {
                    txt_studhed.Text = "" + header + "";

                }
                else
                {
                    txt_studhed.Text = "Header(" + (chkl_studhed.Items.Count) + ")";
                }

            }
            else
            {
                for (int i = 0; i < chkl_studhed.Items.Count; i++)
                {
                    chkl_studhed.Items[i].Selected = false;
                }
                txt_studhed.Text = "---Select---";
            }

            ledgerload();
        }
        catch (Exception ex)
        {

        }
    }

    public void chkl_studhed_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string header = "";
            chkl_studled.Items.Clear();
            int commcount = 0;
            txt_studhed.Text = "--Select--";
            chk_studhed.Checked = false;
            for (int i = 0; i < chkl_studhed.Items.Count; i++)
            {
                if (chkl_studhed.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    header = Convert.ToString(chkl_studhed.Items[i].Text);
                }
            }
            if (commcount > 0)
            {

                if (commcount == chkl_studhed.Items.Count)
                {
                    chk_studhed.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_studhed.Text = "" + header + "";
                }
                else
                {
                    txt_studhed.Text = "Header(" + commcount.ToString() + ")";
                }
            }
            ledgerload();
        }
        catch (Exception ex)
        {

        }
    }
    public void chk_studled_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string ledger = "";
            if (chk_studled.Checked == true)
            {
                for (int i = 0; i < chkl_studled.Items.Count; i++)
                {
                    chkl_studled.Items[i].Selected = true;
                    ledger = Convert.ToString(chkl_studled.Items[i].Text);
                }
                if (chkl_studled.Items.Count == 1)
                {
                    txt_studhed.Text = "" + ledger + "";

                }
                else
                {
                    txt_studled.Text = "Ledger(" + (chkl_studled.Items.Count) + ")";
                }

            }
            else
            {
                for (int i = 0; i < chkl_studled.Items.Count; i++)
                {
                    chkl_studled.Items[i].Selected = false;
                }
                txt_studled.Text = "---Select---";
            }

        }
        catch (Exception ex)
        {

        }
    }
    public void chkl_studled_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string ledger = "";
            int commcount = 0;
            txt_studled.Text = "--Select--";
            chk_studled.Checked = false;
            for (int i = 0; i < chkl_studled.Items.Count; i++)
            {
                if (chkl_studled.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    ledger = Convert.ToString(chkl_studled.Items[i].Text);
                }
            }
            if (commcount > 0)
            {

                if (commcount == chkl_studled.Items.Count)
                {
                    chk_studled.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_studled.Text = "" + ledger + "";
                }
                else
                {
                    txt_studled.Text = "Ledger(" + commcount.ToString() + ")";
                }
            }

        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    #region bank
    public void loadbank()
    {
        try
        {
            chklbank.Items.Clear();
            string query = "select BankName,BankCode,BankPK from FM_FinBankMaster where CollegeCode='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                chklbank.DataSource = ds;
                chklbank.DataTextField = "BankName";
                chklbank.DataValueField = "BankPK";
                chklbank.DataBind();
                for (int i = 0; i < chklbank.Items.Count; i++)
                {
                    chklbank.Items[i].Selected = true;
                }
                txtbank.Text = "Bank(" + chklbank.Items.Count + ")";
                chkbank.Checked = true; ;
            }

        }
        catch
        { }
    }
    public void chkbank_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string bank = "";
            if (chkbank.Checked == true)
            {
                for (int i = 0; i < chklbank.Items.Count; i++)
                {
                    chklbank.Items[i].Selected = true;
                    bank = Convert.ToString(chklbank.Items[i].Text);
                }
                if (chklbank.Items.Count == 1)
                {
                    txtbank.Text = "" + bank + "";

                }
                else
                {
                    txtbank.Text = "Bank(" + (chklbank.Items.Count) + ")";
                }

            }
            else
            {
                for (int i = 0; i < chklbank.Items.Count; i++)
                {
                    chklbank.Items[i].Selected = false;
                }
                txtbank.Text = "---Select---";
            }

        }
        catch (Exception ex)
        {

        }
    }
    public void chklbank_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string bank = "";
            int commcount = 0;
            txtbank.Text = "--Select--";
            chkbank.Checked = false;
            for (int i = 0; i < chklbank.Items.Count; i++)
            {
                if (chklbank.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    bank = Convert.ToString(chklbank.Items[i].Text);
                }
            }
            if (commcount > 0)
            {

                if (commcount == chklbank.Items.Count)
                {
                    chkbank.Checked = true;
                }
                if (commcount == 1)
                {
                    txtbank.Text = "" + bank + "";
                }
                else
                {
                    txtbank.Text = "Bank(" + commcount.ToString() + ")";
                }
            }

        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    #region rb events
    protected void rbpety_OnCheckedChanged(object sender, EventArgs e)
    {
        divheader.Visible = true;
        divbank.Visible = false;
        rbpety.Checked = true;

        //print
        divspread.Visible = false;
        output.Text = "";
        print.Visible = false;
        lblvalidation1.Text = "";
        txtexcelname.Text = "";
    }
    protected void rbbanks_OnCheckedChanged(object sender, EventArgs e)
    {
        divheader.Visible = false;
        divbank.Visible = true;
        divspread.Visible = false;
        //print
        output.Text = "";
        print.Visible = false;
        lblvalidation1.Text = "";
        txtexcelname.Text = "";
    }
    protected void rbboth_OnCheckedChanged(object sender, EventArgs e)
    {
        divheader.Visible = true;
        divbank.Visible = true;
        //print
        divspread.Visible = false;
        output.Text = "";
        print.Visible = false;
        lblvalidation1.Text = "";
        txtexcelname.Text = "";
    }
    #endregion

    #region button search

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            ds.Clear();
            ds = loaddataset();
            if (rbboth.Checked == true)
            {
                bothvalues();
            }
            else if (rbpety.Checked == true)
            {
                petyvalues();
            }
            else if (rbbanks.Checked == true)
            {
                bankvalues();
            }
        }
        catch { }

    }

    #region load Dataset

    public DataSet loaddataset()
    {
        try
        {
            #region get value
            string headerid = "";
            string ledgerid = "";
            string bankid = "";
            string fromdate = "";
            string todate = "";

            for (int i = 0; i < chkl_studhed.Items.Count; i++)
            {
                if (chkl_studhed.Items[i].Selected == true)
                {
                    if (headerid == "")
                    {
                        headerid = Convert.ToString(chkl_studhed.Items[i].Value);
                    }
                    else
                    {
                        headerid = headerid + "'" + "," + "'" + Convert.ToString(chkl_studhed.Items[i].Value);
                    }
                }
            }
            for (int i = 0; i < chkl_studled.Items.Count; i++)
            {
                if (chkl_studled.Items[i].Selected == true)
                {
                    if (ledgerid == "")
                    {
                        ledgerid = Convert.ToString(chkl_studled.Items[i].Value);
                    }
                    else
                    {
                        ledgerid = ledgerid + "'" + "," + "'" + Convert.ToString(chkl_studled.Items[i].Value);
                    }
                }
            }

            for (int i = 0; i < chklbank.Items.Count; i++)
            {
                if (chklbank.Items[i].Selected == true)
                {
                    if (bankid == "")
                    {
                        bankid = Convert.ToString(chklbank.Items[i].Value);
                    }
                    else
                    {
                        bankid = bankid + "'" + "," + "'" + Convert.ToString(chklbank.Items[i].Value);
                    }
                }
            }

            fromdate = txt_fromdate.Text;
            todate = Txt_Todate.Text;
            if (fromdate != "" && todate != "")
            {
                string[] frdate = fromdate.Split('/');
                if (frdate.Length == 3)
                {
                    fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                }
                string[] tdate = todate.Split('/');
                if (tdate.Length == 3)
                {
                    todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
                }
            }

            #endregion
            string SelectQ = "";
            if (rbboth.Checked == true)
            {
                SelectQ = " select CONVERT(varchar(10),TransDate,103) as TransDate,Debit,ChequeNo,CONVERT(varchar(10),ChequeDate,103)as ChequeDate,D.Remarks,HeaderfK,LedgerFK,BankFK,sm.staff_name   from FT_FinContraWithDrawDet D,staff_appl_master SA,staffmaster sm where  D.StaffApplNo =sa.appl_id and sa.appl_no=sm.appl_no";
                //if (headerid != "")
                //{
                //    SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                //}
                //if (ledgerid != "")
                //{
                //    SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                //}
                //if (bankid != "")
                //{
                //    SelectQ = SelectQ + " and BankFK in('" + bankid + "')";
                //}
                if (fromdate != "" && todate != "")
                {
                    SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                }

                SelectQ = SelectQ + " select * from FM_HeaderMaster H,FM_LedgerMaster L where H.HeaderPK =L.HeaderFK and HeaderFK in ('" + headerid + "') and LedgerPK in ('" + ledgerid + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
                SelectQ = SelectQ + " select * from FM_FinBankMaster where BankPK in ('" + bankid + "')";
            }
            else if (rbpety.Checked == true)
            {
                SelectQ = " select CONVERT(varchar(10),TransDate,103) as TransDate,Debit,D.Remarks,h.HeaderName ,l.LedgerName,sm.staff_name from FT_FinContraWithDrawDet D,staff_appl_master SA,staffmaster sm, FM_LedgerMaster l,FM_HeaderMaster H where ISNULL(bankFk,0)=0 and D.StaffApplNo =sa.appl_id and sa.appl_no=sm.appl_no and l.LedgerPK =d.LedgerFK and h.HeaderPK =l.HeaderFK and H.HeaderPK =d.HeaderfK ";
                if (headerid != "")
                {
                    SelectQ = SelectQ + " and d.HeaderFK in ('" + headerid + "')";
                }
                if (ledgerid != "")
                {
                    SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                }
                if (fromdate != "" && todate != "")
                {
                    SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                }
                SelectQ += "  order by isnull(l.priority,1000), l.ledgerName asc ";
            }
            else if (rbbanks.Checked == true)
            {
                SelectQ = " select CONVERT(varchar(10),TransDate,103) as TransDate,Debit,B.BankName,ChequeNo,CONVERT(varchar(10),ChequeDate,103)as ChequeDate,D.Remarks,sm.staff_name,b.AccNo from FT_FinContraWithDrawDet D,staff_appl_master SA,staffmaster sm, FM_FinBankMaster B where ISNULL(bankFk,0)<>0 and D.StaffApplNo =sa.appl_id and sa.appl_no=sm.appl_no and b.BankPK =d.BankFK ";
                if (bankid != "")
                {
                    SelectQ = SelectQ + " and BankFK in('" + bankid + "')";
                }
                if (fromdate != "" && todate != "")
                {
                    SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                }
                //  SelectQ += "  order by isnull(l.priority,1000), l.ledgerName asc ";
            }
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelectQ, "Text");
        }
        catch { }
        return dsload;
    }

    #endregion

    #region both method

    public void bothvalues()
    {
        try
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string cheqno = "";
                    string cheqdate = "";
                    string bankfk = "";
                    string headerfk = "";

                    #region design
                    FpSpread1.Sheets[0].RowCount = 0;
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    FpSpread1.CommandBar.Visible = false;
                    FpSpread1.Sheets[0].AutoPostBack = true;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread1.Sheets[0].RowHeader.Visible = false;
                    FpSpread1.Sheets[0].ColumnCount = 11;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    FarPoint.Web.Spread.TextCellType regno = new FarPoint.Web.Spread.TextCellType();
                    FarPoint.Web.Spread.TextCellType rollno = new FarPoint.Web.Spread.TextCellType();

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
                    // FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[1].Visible = true;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Response Person";
                    // FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Narration";
                    //   FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Header";
                    //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Ledger";
                    // FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Cheque No";
                    // FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Cheque Date";
                    // FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Bank Name";
                    // FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Left;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Acc No";
                    //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Amount";
                    //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Right;
                    #endregion

                    #region values

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FpSpread1.Sheets[0].RowCount++;
                        count += 40;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].RowCount);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["TransDate"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Remarks"]);
                        headerfk = Convert.ToString(ds.Tables[0].Rows[i]["HeaderFK"]);
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            if (headerfk != "")
                            {
                                ds.Tables[1].DefaultView.RowFilter = "HeaderPK='" + Convert.ToString(ds.Tables[0].Rows[i]["HeaderFK"]) + "' and LedgerPK='" + Convert.ToString(ds.Tables[0].Rows[i]["LedgerFK"]) + "'";
                                dv = ds.Tables[1].DefaultView;
                                if (dv.Count != 0 || dv.Count != null)
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[0]["HeaderName"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dv[0]["LedgerName"]);


                                }
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "-";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "-";
                            }

                        }
                        cheqno = Convert.ToString(ds.Tables[0].Rows[i]["ChequeNo"]);
                        if (cheqno != "")
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(cheqno);
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "-";
                        }
                        cheqdate = Convert.ToString(ds.Tables[0].Rows[i]["ChequeDate"]);
                        if (cheqdate != "")
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(cheqdate);
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = "-";
                        }

                        bankfk = Convert.ToString(ds.Tables[0].Rows[i]["BankFK"]);
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            if (bankfk != "")
                            {
                                ds.Tables[2].DefaultView.RowFilter = "BankPK='" + Convert.ToString(ds.Tables[0].Rows[i]["BankFK"]) + "'";
                                dv1 = ds.Tables[2].DefaultView;
                                if (dv1.Count != 0 || dv1.Count != null)
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(dv1[0]["BankName"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dv1[0]["AccNo"]);


                                }
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "-";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = "-";
                            }

                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(ds.Tables[0].Rows[i]["Debit"]);


                    }

                    #endregion

                    #region grandtot
                    FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                    FpSpread1.Sheets[0].Rows.Count++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 10);
                    double hedval = 0;
                    for (int j = 10; j < FpSpread1.Sheets[0].Columns.Count; j++)
                    {
                        for (int i = 0; i < FpSpread1.Rows.Count - 1; i++)
                        {
                            string values = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), j].Value);
                            if (values != "0" && values != "-" && values != "")
                            {
                                if (hedval == 0)
                                {
                                    hedval = Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), j].Text);
                                }
                                else
                                {
                                    hedval = hedval + Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), j].Text);
                                }
                            }
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(hedval);
                        hedval = 0;
                    }
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#4870BE");
                    #endregion

                    divspread.Visible = true;
                    FpSpread1.Visible = true;
                    output.Text = "Both--> Withdraw";
                    FpSpread1.Height = Convert.ToInt32(count);
                    FpSpread1.Width = 1200;
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.SaveChanges();
                    print.Visible = true;
                }
                else
                {
                    FpSpread1.Visible = false;
                    print.Visible = false;
                    pupdiv.Visible = true;
                    pupdiv1.Visible = true;
                    lbl_alert.Visible = true;
                    output.Text = "";
                    lbl_alert.Text = "No Record Found";
                }

            }
        }
        catch { }
    }

    #endregion



    #region pety method

    public void petyvalues()
    {
        try
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    #region design

                    FpSpread1.Sheets[0].RowCount = 0;
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    FpSpread1.CommandBar.Visible = false;
                    FpSpread1.Sheets[0].AutoPostBack = true;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread1.Sheets[0].RowHeader.Visible = false;
                    FpSpread1.Sheets[0].ColumnCount = 7;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    FarPoint.Web.Spread.TextCellType regno = new FarPoint.Web.Spread.TextCellType();
                    FarPoint.Web.Spread.TextCellType rollno = new FarPoint.Web.Spread.TextCellType();

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    // FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
                    //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[1].Visible = true;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Response Person";
                    // FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    // FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Right;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Narration";
                    //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Header";
                    //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Ledger";
                    //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Amount";
                    //   FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Right;
                    #endregion

                    #region values

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FpSpread1.Sheets[0].RowCount++;
                        count += 40;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].RowCount);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["TransDate"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Remarks"]);


                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["HeaderName"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["LedgerName"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["Debit"]);

                    }

                    #endregion

                    #region grandtot
                    FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                    FpSpread1.Sheets[0].Rows.Count++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                    double hedval = 0;
                    for (int j = 6; j < FpSpread1.Sheets[0].Columns.Count; j++)
                    {
                        for (int i = 0; i < FpSpread1.Rows.Count - 1; i++)
                        {
                            string values = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), j].Value);
                            if (values != "0" && values != "-" && values != "")
                            {
                                if (hedval == 0)
                                {
                                    hedval = Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), j].Text);
                                }
                                else
                                {
                                    hedval = hedval + Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), j].Text);
                                }
                            }
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(hedval);
                        hedval = 0;
                    }
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#4870BE");
                    #endregion

                    divspread.Visible = true;
                    FpSpread1.Visible = true;
                    output.Text = "Pety--> Withdraw";
                    FpSpread1.Height = Convert.ToInt32(count);
                    FpSpread1.Width = 930;
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.SaveChanges();
                    print.Visible = true;
                }
                else
                {
                    FpSpread1.Visible = false;
                    print.Visible = false;
                    pupdiv.Visible = true;
                    pupdiv1.Visible = true;
                    lbl_alert.Visible = true;
                    output.Text = "";
                    lbl_alert.Text = "No Record Found";
                }
            }

        }
        catch { }
    }

    #endregion


    #region bank method
    public void bankvalues()
    {
        try
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    #region design
                    FpSpread1.Sheets[0].RowCount = 0;
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    FpSpread1.CommandBar.Visible = false;
                    FpSpread1.Sheets[0].AutoPostBack = true;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread1.Sheets[0].RowHeader.Visible = false;
                    FpSpread1.Sheets[0].ColumnCount = 9;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    FarPoint.Web.Spread.TextCellType regno = new FarPoint.Web.Spread.TextCellType();
                    FarPoint.Web.Spread.TextCellType rollno = new FarPoint.Web.Spread.TextCellType();

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
                    // FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[1].Visible = true;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Response Person";
                    //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Columns[2].Width = 150;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Narration";
                    //   FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Cheque No";
                    //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Cheque Date";
                    // FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Bank Name";
                    // FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Account No";
                    //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Amount ";
                    //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Right;
                    #endregion

                    #region values

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FpSpread1.Sheets[0].RowCount++;
                        count += 40;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].RowCount);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["TransDate"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Remarks"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["ChequeNo"]);

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["ChequeDate"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["BankName"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["AccNo"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["Debit"]);




                    }

                    #endregion

                    #region grandtot
                    FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                    FpSpread1.Sheets[0].Rows.Count++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 8);
                    double hedval = 0;
                    for (int j = 8; j < FpSpread1.Sheets[0].Columns.Count; j++)
                    {
                        for (int i = 0; i < FpSpread1.Rows.Count - 1; i++)
                        {
                            string values = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), j].Value);
                            if (values != "0" && values != "-" && values != "")
                            {
                                if (hedval == 0)
                                {
                                    hedval = Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), j].Text);
                                }
                                else
                                {
                                    hedval = hedval + Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), j].Text);
                                }
                            }
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(hedval);
                        hedval = 0;
                    }
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#4870BE");
                    #endregion

                    divspread.Visible = true;
                    FpSpread1.Visible = true;
                    output.Text = "Bank--> Withdraw";
                    FpSpread1.Height = Convert.ToInt32(count);
                    FpSpread1.Width = 940;
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.SaveChanges();
                    print.Visible = true;
                }
                else
                {
                    FpSpread1.Visible = false;
                    print.Visible = false;
                    pupdiv.Visible = true;
                    pupdiv1.Visible = true;
                    lbl_alert.Visible = true;
                    output.Text = "";
                    lbl_alert.Text = "No Record Found";
                }
            }

        }
        catch { }
    }

    #endregion


    #endregion

    public void btn_errorclose_Click(object sender, EventArgs e)
    {
        pupdiv.Visible = false;
        pupdiv.Visible = false;
    }

    #region print control

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
                if (rbboth.Checked == true)
                {
                    lblvalidation1.Text = "Please Enter Your Both Withdraw Report Name";
                    lblvalidation1.Visible = true;
                    txtexcelname.Focus();
                }
                if (rbpety.Checked == true)
                {
                    lblvalidation1.Text = "Please Enter Your Pety Withdraw Report Name";
                    lblvalidation1.Visible = true;
                    txtexcelname.Focus();
                }
                if (rbbanks.Checked == true)
                {
                    lblvalidation1.Text = "Please Enter Your Bank Withdraw Report Name";
                    lblvalidation1.Visible = true;
                    txtexcelname.Focus();
                }

            }


        }
        catch
        { }

    }

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        { printmethod(); }
        catch { }
    }
    public void printmethod()
    {
        try
        {
            string degreedetails = "";
            string pagename = "";
            if (rbboth.Checked == true)
            {
                degreedetails = "Both Withdraw Report";
                pagename = "ContraWithdrawReport.aspx";
                Printcontrolhed.loadspreaddetails(FpSpread1, pagename, degreedetails);
                Printcontrolhed.Visible = true;
            }
            if (rbpety.Checked == true)
            {
                degreedetails = "Pety Withdraw Report";
                pagename = "ContraWithdrawReport.aspx";
                Printcontrolhed.loadspreaddetails(FpSpread1, pagename, degreedetails);
                Printcontrolhed.Visible = true;
            }
            if (rbbanks.Checked == true)
            {
                degreedetails = "Bank Withdraw Report";
                pagename = "ContraWithdrawReport.aspx";
                Printcontrolhed.loadspreaddetails(FpSpread1, pagename, degreedetails);
                Printcontrolhed.Visible = true;
            }

        }
        catch { }
    }

    #endregion
}