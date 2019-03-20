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

public partial class Investorsposetting : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    DataView dv = new DataView();
    string sql = "";

    FarPoint.Web.Spread.CheckBoxCellType chkboxcol = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkboxsel_all = new FarPoint.Web.Spread.CheckBoxCellType();
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
            if (!IsPostBack)
            {

                rbplainsheet.Checked = true;
                chkfootersign.Checked = true;
                chk_terms.Checked = true;
                loadprintdetails();
                loadtermssp();
                loadtermssp2();
                showterms();
                showstaffdesc();
                txtstaff.Attributes.Add("Readonly", "Readonly");
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = System.Drawing.Color.White;
                // darkstyle.Font.Name = "Book Antiqua";
                darkstyle.Font.Size = FontUnit.Medium;
                darkstyle.Border.BorderSize = 1;
                darkstyle.Border.BorderColor = System.Drawing.Color.White;
                fsstaff.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                retrive();

            }
        }
        catch (Exception ex) { }
    }

    public void retrive()
    {
        sql = "select * from IM_POSettings ";
        ds.Clear();
        ds = da.select_method_wo_parameter(sql, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {

            if (ds.Tables[0].Rows[0]["IsContinueRefNo"].ToString() == "True")
            {
                chkcontinuref.Checked = true;
                txt_refno.Text = "";
                txt_refno.Enabled = false;
            }
            else
            {
                chkcontinuref.Checked = false;
                txt_refno.Text = ds.Tables[0].Rows[0]["ReferenceNo"].ToString();
                txt_refno.Enabled = true;
            }
            txt_Refheader.Text = ds.Tables[0].Rows[0]["ReportHeader"].ToString();

            if (ds.Tables[0].Rows[0]["IsLetterPad"].ToString() == "True")
            {
                rbletterpad.Checked = true;

            }
            else
            {
                rbplainsheet.Checked = false;

            }
            if (ds.Tables[0].Rows[0]["IsTerms"].ToString() == "True")
            {
                chk_terms.Checked = true;

            }
            else
            {
                chk_terms.Checked = false;

            }
            txtdesign.Text = ds.Tables[0].Rows[0]["AddressDesc"].ToString();
            if (ds.Tables[0].Rows[0]["IsFooterDesc"].ToString() == "True")
            {
                chkfootersign.Checked = true;

            }
            else if (ds.Tables[0].Rows[0]["IsFooterDesc"].ToString().Trim() == "" || ds.Tables[0].Rows[0]["IsFooterDesc"].ToString().Trim() == null)
            {
                chkfootersign.Checked = true;

            }
            else
            {
                chkfootersign.Checked = false;

            }

            if (ds.Tables[0].Rows[0]["IsSignwithSeal"].ToString() == "True")
            {
                chksignaturewithseal.Checked = true;

            }
            else
            {
                chksignaturewithseal.Checked = false;

            }
        }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch
        {

        }
    }
    protected void chkcontinuref_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkcontinuref.Checked == true)
            {
                txt_refno.Text = "";
                txt_refno.Enabled = false;

            }
            else
            {
                txt_refno.Enabled = true;
            }
        }
        catch
        {

        }
    }

    protected void rbsheet_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (rbplainsheet.Checked == true)
            {
                collinfo.Visible = true;
            }
            else
            {
                collinfo.Visible = false;
                //for (int parent = 0; parent < chkcollege.Items.Count; parent++)
                //{
                //    chkcollege.Items[parent].Selected = false;

                //}
            }

        }
        catch
        {

        }
    }
    protected void chk_terms_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chk_terms.Checked == true)
            {
                terms.Visible = true;
            }
            else
            {
                terms.Visible = false;
            }

        }
        catch
        {

        }
    }
    protected void btnadd_Click(object sender, EventArgs e)
    {
        try
        {
            string istrm = "";
            string trmdesc = "";
            string[] spilttrmdesc;
            string againtrmdesc = "";
            string finaldesc = "";
            string addfinaldesc = "";
            if (txttermdesc.Text.ToString() == "")
            {
                lbl_alert.Text = "Please Enter Terms Description";
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;
                return;

            }
            if (txttermvalue.Text.ToString() == "")
            {
                lbl_alert.Text = "Please Enter Reference Header";
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;
                return;

            }
            addfinaldesc = txttermdesc.Text.ToString() + "-" + txttermvalue.Text.ToString();
            addfinaldesc = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(addfinaldesc);

            sql = "select * from IM_POSettings where collegecode='" + Session["collegecode"].ToString() + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(sql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    istrm = ds.Tables[0].Rows[0]["IsTerms"].ToString();
                    if (istrm.Trim() == "True")
                    {
                        trmdesc = ds.Tables[0].Rows[0]["TermsDesc"].ToString();
                        if (trmdesc != "")
                        {
                            spilttrmdesc = trmdesc.Split(';');
                            for (int j = 0; j < spilttrmdesc.Length; j++)
                            {
                                againtrmdesc = spilttrmdesc[j].ToString();

                                if (finaldesc == "")
                                {
                                    finaldesc = againtrmdesc;
                                }
                                else
                                {
                                    finaldesc = finaldesc + ";" + againtrmdesc;
                                }
                            }
                        }
                    }
                }
            }
            if (finaldesc != "")
            {
                finaldesc = finaldesc + ";" + addfinaldesc;
            }
            else
            {
                finaldesc = addfinaldesc;
            }

            if (chk_terms.Checked == true)
            {
                istrm = "1";
            }
            else
            {
                istrm = "0";
            }
            sql = "if exists (select * from IM_POSettings where collegecode='" + Session["collegecode"].ToString() + "' ) update IM_POSettings set TermsDesc='" + finaldesc + "' , IsTerms='" + istrm + "' where collegecode='" + Session["collegecode"].ToString() + "' else insert into  IM_POSettings (IsTerms,TermsDesc,CollegeCode) values ('" + istrm + "','" + finaldesc + "','" + Session["collegecode"].ToString() + "')";
            int a = da.update_method_wo_parameter(sql, "text");


            showterms();
            txttermdesc.Text = "";
            txttermvalue.Text = "";
        }



        catch
        {

        }
    }
    public void showterms()
    {


        FpSpread1.Sheets[0].RowCount = 0;


        string istrm = "";
        string trmdesc = "";
        string[] spilttrmdesc;
        string againtrmdesc = "";

        sql = "select * from IM_POSettings where collegecode='" + Session["collegecode"].ToString() + "'";
        ds.Clear();
        ds = da.select_method_wo_parameter(sql, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            FpSpread1.Sheets[0].Rows.Count++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = chkboxsel_all;
            chkboxsel_all.AutoPostBack = true;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                istrm = ds.Tables[0].Rows[0]["IsTerms"].ToString();
                if (istrm.Trim() == "True")
                {
                    trmdesc = ds.Tables[0].Rows[0]["TermsDesc"].ToString();
                    if (trmdesc != "")
                    {
                        spilttrmdesc = trmdesc.Split(';');
                        for (int j = 0; j < spilttrmdesc.Length; j++)
                        {
                            againtrmdesc = spilttrmdesc[j].ToString();

                            //dt.Rows.Add(againtrmdesc);
                            FpSpread1.Sheets[0].Rows.Count++;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].Rows.Count - 1);
                            // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].CellType = txtceltype;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(againtrmdesc);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = chkboxcol;

                        }

                    }
                }
            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();

            FpSpread1.Visible = true;
            if (FpSpread1.Sheets[0].Rows.Count == 1)
            {
                FpSpread1.Sheets[0].RowCount = 0;
            }
        }


    }

    public void showstaffdesc()
    {

        DataSet staffmaster = new DataSet();
        staffmaster.Clear();
        staffmaster = da.select_method_wo_parameter("select staff_code,staff_name from staffmaster", "Text");
        FpSpread2.Sheets[0].RowCount = 0;


        string istrm = "";
        string trmdesc = "";
        string[] spilttrmdesc;
        string againtrmdesc = "";
        string[] spiltagaintrmdesc;

        sql = "select * from IM_POSettings where collegecode='" + Session["collegecode"].ToString() + "'";
        ds.Clear();
        ds = da.select_method_wo_parameter(sql, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            FpSpread2.Sheets[0].Rows.Count++;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].CellType = chkboxsel_all;
            chkboxsel_all.AutoPostBack = true;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                istrm = ds.Tables[0].Rows[0]["IsFooterDesc"].ToString();
                if (istrm.Trim() == "True")
                {
                    trmdesc = ds.Tables[0].Rows[0]["FooterDescStaff"].ToString();
                    if (trmdesc != "")
                    {
                        spilttrmdesc = trmdesc.Split(';');
                        for (int j = 0; j < spilttrmdesc.Length; j++)
                        {
                            againtrmdesc = spilttrmdesc[j].ToString();
                            spiltagaintrmdesc = againtrmdesc.Split('-');
                            //for (int ii = 0; ii < spiltagaintrmdesc.Length; ii++)
                            //{
                            FpSpread2.Sheets[0].Rows.Count++;

                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(FpSpread2.Sheets[0].Rows.Count - 1);
                            // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].CellType = txtceltype;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].Text = Convert.ToString(spiltagaintrmdesc[0]);
                            if (staffmaster.Tables[0].Rows.Count > 0)
                            {
                                staffmaster.Tables[0].DefaultView.RowFilter = "staff_code='" + spiltagaintrmdesc[1] + "'";
                                dv = staffmaster.Tables[0].DefaultView;
                                if (dv.Count > 0)
                                {
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(dv[0][1].ToString());
                                }
                            }

                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Tag = Convert.ToString(spiltagaintrmdesc[1]);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].CellType = chkboxcol;

                            //}
                            //dt.Rows.Add(againtrmdesc);


                        }

                    }
                }
            }
        }
        if (FpSpread2.Sheets[0].Rows.Count == 1)
        {
            FpSpread2.Sheets[0].RowCount = 0;
        }


    }

    protected void chkselall_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkselall.Checked == true)
            {
                for (int parent = 0; parent < chkcollege.Items.Count; parent++)
                {
                    chkcollege.Items[parent].Selected = true;

                }
            }
            else
            {
                for (int parent = 0; parent < chkcollege.Items.Count; parent++)
                {
                    chkcollege.Items[parent].Selected = false;

                }
            }

        }
        catch
        {

        }
    }

    protected void chkfromaddress_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkfromaddress.Checked == true)
            {
                txtdesign.Enabled = false;
            }
            else
            {
                txtdesign.Enabled = true;
            }

        }
        catch
        {

        }
    }

    protected void chkfootersign_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkfootersign.Checked == true)
            {
                Table1.Visible = true;
            }
            else
            {
                Table1.Visible = false;
            }

        }
        catch
        {

        }
    }
    protected void btnstaffadd1_Click(object sender, EventArgs e)
    {
        try
        {
            int checkedcount = 0;
            string staffname = "";
            string staffcode = "";
            for (int iy = 0; iy < fsstaff.Sheets[0].RowCount; iy++)
            {
                if (Convert.ToInt32(fsstaff.Sheets[0].Cells[iy, 0].Value) == 1)
                {
                    checkedcount++;
                    staffcode = fsstaff.Sheets[0].Cells[iy, 1].Text.ToString();
                    staffname = fsstaff.Sheets[0].Cells[iy, 2].Text.ToString();
                }


            }
            if (checkedcount > 1 || checkedcount == 0)
            {
                lbl_alert.Text = "Please Select Only One Staff";
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;
                return;
            }
            else
            {
                lblstaffcode.Text = staffcode;
                txtstaff.Text = staffname;
                imgdiv2.Visible = false;
                imgshowdiv2.Visible = false;
            }

        }
        catch
        {
        }
    }

    protected void btnstaffadd_Click(object sender, EventArgs e)
    {
        try
        {
            string istrm = "";
            string trmdesc = "";
            string[] spilttrmdesc;
            string againtrmdesc = "";
            string finaldesc = "";
            string addfinaldesc = "";

            if (txtdescrip.Text.ToString() == "")
            {
                lbl_alert.Text = "Please Enter Footer Description";
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;
                return;

            }
            if (txtstaff.Text.ToString() == "")
            {
                lbl_alert.Text = "Please Select Any One Staff";
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;
                return;

            }
            addfinaldesc = txtdescrip.Text.ToString() + "-" + lblstaffcode.Text.ToString();
            addfinaldesc = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(addfinaldesc);
            sql = "select * from IM_POSettings where collegecode='" + Session["collegecode"].ToString() + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(sql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    // istrm = ds.Tables[0].Rows[0]["IsFooterDesc"].ToString();
                    if (chkfootersign.Checked == true)
                    {
                        istrm = "1";
                    }
                    else
                    {
                        istrm = "0";
                    }
                    if (istrm.Trim() == "1")
                    {
                        trmdesc = ds.Tables[0].Rows[0]["FooterDescStaff"].ToString();
                        if (trmdesc != "")
                        {
                            spilttrmdesc = trmdesc.Split(';');
                            for (int j = 0; j < spilttrmdesc.Length; j++)
                            {
                                againtrmdesc = spilttrmdesc[j].ToString();

                                if (finaldesc == "")
                                {
                                    finaldesc = againtrmdesc;
                                }
                                else
                                {
                                    finaldesc = finaldesc + ";" + againtrmdesc;
                                }
                            }
                        }
                    }
                }
            }
            if (finaldesc != "")
            {
                finaldesc = finaldesc + ";" + addfinaldesc;
            }
            else
            {
                finaldesc = addfinaldesc;
            }

            if (chkfootersign.Checked == true)
            {
                istrm = "1";
            }
            else
            {
                istrm = "0";
            }
            sql = "if exists (select * from IM_POSettings where collegecode='" + Session["collegecode"].ToString() + "' ) update IM_POSettings set FooterDescStaff='" + finaldesc + "' , IsFooterDesc='" + istrm + "' where collegecode='" + Session["collegecode"].ToString() + "' else insert into  IM_POSettings (IsFooterDesc,FooterDescStaff,CollegeCode) values ('" + istrm + "','" + finaldesc + "','" + Session["collegecode"].ToString() + "')";
            int a = da.update_method_wo_parameter(sql, "text");


            showstaffdesc();
            txtdescrip.Text = "";
            txtstaff.Text = "";
        }
        catch
        {

        }
    }

    protected void chksignaturewithseal_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {

        }
    }
    protected void btnfinalsave_Click(object sender, EventArgs e)
    {
        try
        {
            string Collvalue = "";
            string pagen = "Investorsposetting.aspx";
            if (chkcontinuref.Checked == false)
            {
                if (txt_refno.Text.ToString() == "")
                {
                    lbl_alert.Text = "Please Enter Reference No";
                    lbl_alert.Visible = true;
                    imgdiv2.Visible = true;
                    // txt_refno.BorderColor = Color.Red;
                    return;

                }
            }
            for (int parent = 0; parent < chkcollege.Items.Count; parent++)
            {
                if (chkcollege.Items[parent].Selected == true)
                {

                    string collinfo = chkcollege.Items[parent].Value;
                    if (Collvalue == "")
                    {
                        Collvalue = collinfo;
                    }
                    else
                    {
                        Collvalue = Collvalue + '#' + collinfo;
                    }
                }
            }

            sql = "";
            sql = "if exists(Select * from tbl_print_master_settings where  page_name='" + pagen + "')";
            sql = sql + " update tbl_print_master_settings set college_details='" + Collvalue + "' where page_name='" + pagen + "'";
            sql = sql + " else insert into tbl_print_master_settings (Page_Name,college_details) values ('" + pagen + "','" + Collvalue + "')";
            int a = da.update_method_wo_parameter(sql, "text");

            string refno = "";
            string refhead = "";
            string isref = "";
            string isletterpad = "";
            string addressdesi = "";
            string isseal = "";
            if (chkcontinuref.Checked == true)
            {
                refno = "";
                isref = "1";

            }
            else
            {
                refno = txt_refno.Text.ToString().ToUpper();
                // refno = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(refno);
                isref = "0";
            }

            if (rbletterpad.Checked == true)
            {
                isletterpad = "1";

            }
            else
            {
                isletterpad = "0";
            }
            if (chksignaturewithseal.Checked == true)
            {
                isseal = "1";

            }
            else
            {
                isseal = "0";
            }
            refhead = txt_Refheader.Text.ToString();
            refhead = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(refhead);
            addressdesi = txtdesign.Text.ToString();
            addressdesi = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(addressdesi);

            sql = "if exists (select * from IM_POSettings where collegecode='" + Session["collegecode"].ToString() + "' ) update IM_POSettings set IsContinueRefNo='" + isref + "' , ReferenceNo='" + refno + "',ReportHeader='" + refhead + "',IsLetterPad='" + isletterpad + "' ,AddressDesc='" + addressdesi + "',IsSignwithSeal='" + isseal + "'  where collegecode='" + Session["collegecode"].ToString() + "' else insert into  IM_POSettings (IsContinueRefNo,ReferenceNo,ReportHeader,IsLetterPad,AddressDesc,IsSignwithSeal,collegecode) values ('" + isref + "','" + refno + "','" + refhead + "','" + isletterpad + "','" + addressdesi + "','" + isseal + "','" + Session["collegecode"].ToString() + "')";
            a = da.update_method_wo_parameter(sql, "text");

            lbl_alert.Text = "Saved Successfully";
            lbl_alert.Visible = true;
            imgdiv2.Visible = true;
            return;


        }
        catch
        {

        }
    }
    protected void btndeletefp1_Click(object sender, EventArgs e)
    {
        try
        {
            string data = "";
            FpSpread1.SaveChanges();
            for (int iy = 1; iy < FpSpread1.Sheets[0].RowCount; iy++)
            {
                if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[iy, 2].Value) == 0)
                {


                    if (data == "")
                    {
                        data = FpSpread1.Sheets[0].Cells[iy, 1].Text.ToString();
                    }
                    else
                    {
                        data = data + ";" + FpSpread1.Sheets[0].Cells[iy, 1].Text.ToString();
                    }


                }


            }
            if (data != "")
            {
                sql = "update IM_POSettings set TermsDesc='" + data + "'  where collegecode='" + Session["collegecode"].ToString() + "' ";
                int a = da.update_method_wo_parameter(sql, "text");
            }
            else
            {
                sql = "select * from IM_POSettings ";
                ds.Clear();
                ds = da.select_method_wo_parameter(sql, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    sql = "update IM_POSettings set TermsDesc=''  where collegecode='" + Session["collegecode"].ToString() + "' ";
                    int a = da.update_method_wo_parameter(sql, "text");
                }
            }
            showterms();
        }
        catch
        {

        }
    }
    protected void btndeletefp2_Click(object sender, EventArgs e)
    {
        try
        {
            string data = "";
            FpSpread2.SaveChanges();
            for (int iy = 1; iy < FpSpread2.Sheets[0].RowCount; iy++)
            {
                if (Convert.ToInt32(FpSpread2.Sheets[0].Cells[iy, 3].Value) == 0)
                {


                    if (data == "")
                    {
                        data = FpSpread2.Sheets[0].Cells[iy, 2].Text.ToString() + "-" + FpSpread2.Sheets[0].Cells[iy, 1].Tag.ToString();
                    }
                    else
                    {
                        data = data + ";" + FpSpread2.Sheets[0].Cells[iy, 2].Text.ToString() + "-" + FpSpread2.Sheets[0].Cells[iy, 1].Tag.ToString();
                    }


                }


            }
            if (data != "")
            {
                sql = "update IM_POSettings set FooterDescStaff='" + data + "'  where collegecode='" + Session["collegecode"].ToString() + "' ";
                int a = da.update_method_wo_parameter(sql, "text");
            }
            else
            {
                sql = "select * from IM_POSettings ";
                ds.Clear();
                ds = da.select_method_wo_parameter(sql, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    sql = "update IM_POSettings set FooterDescStaff=''  where collegecode='" + Session["collegecode"].ToString() + "' ";
                    int a = da.update_method_wo_parameter(sql, "text");
                }
            }
            showstaffdesc();

        }
        catch
        {

        }
    }
    public void loadprintdetails()
    {
        string collegedetails = da.GetFunction("select college_details from tbl_print_master_settings where page_Name='Investorsposetting.aspx'");
        string[] spiltcollegedetails = collegedetails.Split('#');
        for (int i = 0; i <= spiltcollegedetails.GetUpperBound(0); i++)
        {
            string collinfo = spiltcollegedetails[i].ToString();
            if (collinfo == "College Name")
            {
                chkcollege.Items[0].Selected = true;

            }
            else if (collinfo == "University")
            {
                chkcollege.Items[1].Selected = true;
            }
            else if (collinfo == "Affliated By")
            {
                chkcollege.Items[2].Selected = true;
            }
            else if (collinfo == "Address")
            {
                chkcollege.Items[3].Selected = true;
            }
            else if (collinfo == "City")
            {
                chkcollege.Items[4].Selected = true;
            }
            else if (collinfo == "District & State & Pincode")
            {
                chkcollege.Items[5].Selected = true;
            }
            else if (collinfo == "Phone No & Fax")
            {
                chkcollege.Items[6].Selected = true;
            }
            else if (collinfo == "Email & Web Site")
            {
                chkcollege.Items[7].Selected = true;
            }
            else if (collinfo == "Right Logo")
            {
                chkcollege.Items[8].Selected = true;
            }
            else if (collinfo == "Left Logo")
            {
                chkcollege.Items[9].Selected = true;
            }
            else if (collinfo == "Signature")
            {
                chkcollege.Items[10].Selected = true;
            }
        }
    }

    public void loadtermssp()
    {
        FpSpread1.Sheets[0].RowHeader.Visible = false;

        FpSpread1.CommandBar.Visible = false;
        FpSpread1.Width = 380;
        FpSpread1.Height = 150;
        FpSpread1.Sheets[0].ColumnCount = 3;
        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
        FpSpread1.Sheets[0].AutoPostBack = false;
        FpSpread1.Sheets[0].Columns[0].Locked = true;
        FpSpread1.Sheets[0].Columns[1].Locked = true;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Terms";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Select";
        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
        FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;

        FpSpread1.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].ColumnHeader.Columns[0].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].ColumnHeader.Columns[1].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].ColumnHeader.Columns[1].VerticalAlign = VerticalAlign.Middle;

        FpSpread1.Sheets[0].ColumnHeader.Columns[2].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].ColumnHeader.Columns[2].VerticalAlign = VerticalAlign.Middle;

        FpSpread1.Sheets[0].Columns[0].Width = 40;
        FpSpread1.Sheets[0].Columns[1].Width = 280;
        FpSpread1.Sheets[0].Columns[2].Width = 40;

        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle.ForeColor = System.Drawing.Color.White;
        // darkstyle.Font.Name = "Book Antiqua";
        darkstyle.Font.Size = FontUnit.Medium;
        darkstyle.Border.BorderSize = 1;
        darkstyle.Border.BorderColor = System.Drawing.Color.White;
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
        FpSpread1.Sheets[0].RowCount = 0;
    }
    public void loadtermssp2()
    {
        FpSpread2.Sheets[0].RowHeader.Visible = false;

        FpSpread2.CommandBar.Visible = false;
        FpSpread2.Width = 380;
        FpSpread2.Height = 150;
        FpSpread2.Sheets[0].ColumnCount = 4;
        FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
        FpSpread2.Sheets[0].AutoPostBack = false;
        FpSpread2.Sheets[0].Columns[0].Locked = true;
        FpSpread2.Sheets[0].Columns[1].Locked = true;
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Description";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Select";
        FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
        FpSpread2.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
        FpSpread2.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;

        FpSpread2.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].ColumnHeader.Columns[0].VerticalAlign = VerticalAlign.Middle;
        FpSpread2.Sheets[0].ColumnHeader.Columns[1].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].ColumnHeader.Columns[1].VerticalAlign = VerticalAlign.Middle;
        FpSpread2.Sheets[0].ColumnHeader.Columns[2].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].ColumnHeader.Columns[2].VerticalAlign = VerticalAlign.Middle;
        FpSpread2.Sheets[0].ColumnHeader.Columns[3].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].ColumnHeader.Columns[3].VerticalAlign = VerticalAlign.Middle;

        FpSpread2.Sheets[0].Columns[0].Width = 40;
        FpSpread2.Sheets[0].Columns[1].Width = 100;
        FpSpread2.Sheets[0].Columns[2].Width = 180;
        FpSpread2.Sheets[0].Columns[3].Width = 40;

        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle.ForeColor = System.Drawing.Color.White;
        // darkstyle.Font.Name = "Book Antiqua";
        darkstyle.Font.Size = FontUnit.Medium;
        darkstyle.Border.BorderSize = 1;
        darkstyle.Border.BorderColor = System.Drawing.Color.White;
        FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
        FpSpread2.Sheets[0].RowCount = 0;
    }

    protected void FindBtn_Click(object sender, EventArgs e)
    {
        imgshowdiv2.Visible = true;
        panel3.Visible = true;
        // panelrollnopop.Visible = false;
        fsstaff.Visible = true;
        fsstaff.Sheets[0].RowCount = 0;
        BindCollege();
        loadstaffdep(Session["collegecode"].ToString());

        loadfsstaff();
        // loadallstaff();//Hidden By Srinath 9/5/2013
    }
    protected void loadfsstaff()
    {
        string sql = "";
        if (ddldepratstaff.SelectedIndex != 0)
        {
            if (txt_search.Text != "")
            {
                if (ddlstaff.SelectedIndex == 0)
                {
                    sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0)and  (staffmaster.settled = 0)  and (staff_name like '" + txt_search.Text + "%') and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code) and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";//Modifed By Srinath 9/5/2013
                }
                else if (ddlstaff.SelectedIndex == 1)
                {
                    sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '" + txt_search.Text + "%') and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code) and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";//Modifed By Srinath 9/5/2013
                }
            }
            else
            {
                //sql = "SELECT staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (hrdept_master.dept_name = '" + ddldepratstaff.Text + "') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "' and (staffmaster.college_code =hrdept_master.college_code)";
                sql = "SELECT staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";

            }
        }
        else if (txt_search.Text != "")
        {
            if (ddlstaff.SelectedIndex == 0)
            {
                sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_name like '" + txt_search.Text + "%') and (staffmaster.college_code =hrdept_master.college_code) and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";//Modifed By Srinath 9/5/2013
            }
            else if (ddlstaff.SelectedIndex == 1)
            {
                sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '" + txt_search.Text + "%') and (staffmaster.college_code =hrdept_master.college_code) and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";//Modifed By Srinath 9/5/2013
            }
            else if (ddlcollege.SelectedIndex != -1)
            {
                sql = "select distinct staffmaster.staff_code, staff_name  from stafftrans,staffmaster where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and settled=0 and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";//Modifed By Srinath 9/5/2013
            }

            else
            {
                sql = "select distinct staffmaster.staff_code, staff_name from stafftrans,staffmaster,hrdept_master.dept_name where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";//Modifed By Srinath 9/5/2013

            }
        }
        else
            if (ddldepratstaff.SelectedValue.ToString() == "All")
            {
                sql = "select distinct staffmaster.staff_code, staff_name  from stafftrans,staffmaster where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and settled=0 and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";//Modifed By Srinath 9/5/2013

            }
        fsstaff.Sheets[0].RowCount = 0;
        fsstaff.SaveChanges();

        FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
        FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();


        //fsstaff.Sheets[0].RowCount = fsstaff.Sheets[0].RowCount + 1;
        //fsstaff.Sheets[0].SpanModel.Add(fsstaff.Sheets[0].RowCount - 1, 0, 1, 3);
        fsstaff.Sheets[0].AutoPostBack = false;
        // string bindspread = sql;

        DataSet dsbindspread = new DataSet();
        dsbindspread = da.select_method_wo_parameter(sql, "Text");
        //con.Close();
        // con.Open();

        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

        fsstaff.Sheets[0].RowCount = 0;
        if (dsbindspread.Tables[0].Rows.Count > 0)
        {
            int sno = 0;
            for (int rolcount = 0; rolcount < dsbindspread.Tables[0].Rows.Count; rolcount++)
            {
                sno++;
                string name = dsbindspread.Tables[0].Rows[rolcount]["staff_name"].ToString();
                string code = dsbindspread.Tables[0].Rows[rolcount]["staff_code"].ToString();

                //  fsstaff.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
                fsstaff.Sheets[0].ColumnHeader.Columns[0].Label = "Select";
                fsstaff.Sheets[0].ColumnHeader.Columns[2].Label = "Staff Name";
                fsstaff.Sheets[0].ColumnHeader.Columns[1].Label = "Staff Code";
                fsstaff.Sheets[0].Columns[1].CellType = txt;
                fsstaff.SheetCorner.Cells[0, 0].Text = "S.No";
                fsstaff.SheetCorner.Cells[0, 0].Font.Bold = true;
                fsstaff.Sheets[0].Columns[1].Locked = true;//Added By Srinath 16/8/2013
                fsstaff.Sheets[0].Columns[2].Locked = true;
                fsstaff.Sheets[0].Columns[2].Width = 200;
                fsstaff.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

                fsstaff.Sheets[0].ColumnCount = 3;
                fsstaff.Sheets[0].Columns[2].Width = 250;
                fsstaff.Sheets[0].Columns[1].Width = 120;
                fsstaff.Width = 515;

                //fsstaff.Width = 398;
                fsstaff.Sheets[0].RowCount = fsstaff.Sheets[0].RowCount + 1;
                fsstaff.Sheets[0].Rows[fsstaff.Sheets[0].RowCount - 1].Font.Bold = false;
                //fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                //fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].Text = name;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].CellType = txt;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].Text = dsbindspread.Tables[0].Rows[rolcount]["staff_code"].ToString();
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;

                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].CellType = chkcell1;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                fsstaff.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Left;
                chkcell1.AutoPostBack = true;

            }
            int rowcount = fsstaff.Sheets[0].RowCount;
            fsstaff.Height = 278;
            fsstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
            fsstaff.SaveChanges();
            // con.Close();

        }
    }
    protected void ddldepratstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        fsstaff.Sheets[0].RowCount = 0;
        loadfsstaff();
    }
    protected void txt_search_TextChanged(object sender, EventArgs e)
    {
        fsstaff.Sheets[0].RowCount = 0;

        loadfsstaff();
    }
    protected void ddlstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        fsstaff.Sheets[0].RowCount = 0;

        loadfsstaff();
    }
    public void BindCollege()
    {
        // con.Open();
        string cmd = "select collname,college_code from collinfo";
        // SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        ds = da.select_method_wo_parameter(cmd, "Text");
        //da.Fill(ds);
        ddlcollege.DataSource = ds;
        ddlcollege.DataTextField = "collname";
        ddlcollege.DataValueField = "college_code";
        ddlcollege.DataBind();
        //ddlcollege.SelectedIndex = ddlcollege.Items.Count - 1;
        //con.Close();
    }
    public void loadstaffdep(string collegecode)
    {
        //con.Open();
        string cmd = "select distinct dept_name,dept_code from hrdept_master where college_code=" + Session["collegecode"] + "";
        // SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        ds = da.select_method_wo_parameter(cmd, "Text");
        // da.Fill(ds);
        ddldepratstaff.DataSource = ds;
        ddldepratstaff.DataTextField = "dept_name";
        ddldepratstaff.DataValueField = "dept_code";
        ddldepratstaff.DataBind();
        ddldepratstaff.Items.Insert(0, "All");
        //con.Close();
    }


    protected void btnexit_Click(object sender, EventArgs e)
    {
        panel3.Visible = false;
        imgshowdiv2.Visible = false;
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void Fpspread1_Command(object sender, EventArgs e)
    {

        if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 2].Value) == 1)
        {
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                FpSpread1.Sheets[0].Cells[i, 2].Value = 1;
            }
        }
        else if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 2].Value) == 0)
        {
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                FpSpread1.Sheets[0].Cells[i, 2].Value = 0;
            }

        }

        FpSpread1.Visible = true;
    }
    protected void Fpspread2_Command(object sender, EventArgs e)
    {

        if (Convert.ToInt32(FpSpread2.Sheets[0].Cells[0, 3].Value) == 1)
        {
            for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
            {
                FpSpread2.Sheets[0].Cells[i, 3].Value = 1;
            }
        }
        else if (Convert.ToInt32(FpSpread2.Sheets[0].Cells[0, 3].Value) == 0)
        {
            for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
            {
                FpSpread2.Sheets[0].Cells[i, 3].Value = 0;
            }

        }

        FpSpread2.Visible = true;
    }
}