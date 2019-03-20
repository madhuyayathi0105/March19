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


public partial class HM_RebateMaster : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DAccess2 d2 = new DAccess2();

    protected void Page_Load(object sender, EventArgs e)
    {
        //rdbdate.Checked = true;
        //****************************************************//
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("hostelindex"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/HostelMod/hostelindex.aspx");
                return;
            }
        }
        //****************************************************//
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        lbl_norec.Text = "";
        if (!IsPostBack)
        {
            bindhostelname();
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Visible = false;
            btnsave.Visible = false;
            btn_reset.Visible = false;
            rdbdate.Checked = true;
            // rdb_monthwise.Checked = true;
        }
    }
    public void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    public void bindhostelname()
    {
        try
        {
            ds.Clear();
            cbl_hostelname.Items.Clear();
            //string selecthostel = "select HostelMasterPK,HostelName from HM_HostelMaster order by HostelMasterPK";
            //ds = d2.select_method_wo_parameter(selecthostel, "Text");

            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "HostelName";
                cbl_hostelname.DataValueField = "HostelMasterPK";
                cbl_hostelname.DataBind();

                if (cbl_hostelname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                    {
                        cbl_hostelname.Items[i].Selected = true;
                    }

                    txt_hostelname.Text = "Hostel Name(" + cbl_hostelname.Items.Count + ")";
                }
            }
            else
            {
                txt_hostelname.Text = "--Select--";

            }
        }
        catch
        {
        }
    }
    public void cb_hostelname_checkedchange(object sender, EventArgs e)
    {
        try
        {

            if (cb_hostelname.Checked == true)
            {
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = true;
                }
                txt_hostelname.Text = "Hostel Name(" + (cbl_hostelname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = false;
                }
                txt_hostelname.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            txt_hostelname.Text = "--Select--";
            cb_hostelname.Checked = false;

            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_hostelname.Text = "Hostel Name(" + commcount.ToString() + ")";
                if (commcount == cbl_hostelname.Items.Count)
                {
                    cb_hostelname.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }


    }

    public void cb_month_checkedchange(object sender, EventArgs e)
    {
        try
        {

            if (cb_month.Checked == true)
            {
                for (int i = 0; i < cbl_month.Items.Count; i++)
                {
                    cbl_month.Items[i].Selected = true;
                }
                txt_month.Text = " Month(" + (cbl_month.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_month.Items.Count; i++)
                {
                    cbl_month.Items[i].Selected = false;
                }
                txt_month.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {

        }

    }
    public void cbl_month_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            txt_month.Text = "--Select--";
            cb_month.Checked = false;

            for (int i = 0; i < cbl_month.Items.Count; i++)
            {
                if (cbl_month.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_month.Text = " Month(" + commcount.ToString() + ")";
                if (commcount == cbl_month.Items.Count)
                {
                    cb_month.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            div_report.Visible = false;
            Printcontrol.Visible = false;
            FpSpread1.Visible = false;
            FpSpread1.SaveChanges();
            string itemheadercode = "";
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                }
            }

            string month = "";
            for (int i = 0; i < cbl_month.Items.Count; i++)
            {
                if (cbl_month.Items[i].Selected == true)
                {
                    if (month == "")
                    {
                        month = "" + cbl_month.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        month = month + "'" + "," + "'" + cbl_month.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (txt_hostelname.Text == "--Select--")
            {
                lblerror.Visible = true;
                lblerror.Text = "Kindly Select The Hostel";
                btnsave.Visible = false;
                btn_reset.Visible = false;
            }
            else
            {
                if (txt_month.Text == "--Select--")
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select The Month";
                    btnsave.Visible = false;
                    btn_reset.Visible = false;
                    //FpSpread1.Visible = false;
                }
                else
                {
                    //FpSpread1.Visible = true;
                    if (itemheadercode.Trim() != "")
                    {
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = false;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 1;


                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Columns[0].Width = 50;
                        FpSpread1.Columns[0].Locked = true;

                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Days";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[1].Width = 150;
                        FpSpread1.Columns[1].Locked = true;

                        for (int i = 0; i < cbl_month.Items.Count; i++)
                        {
                            if (cbl_month.Items[i].Selected == true)
                            {
                                FpSpread1.Sheets[0].ColumnCount++;
                                FarPoint.Web.Spread.DoubleCellType db = new FarPoint.Web.Spread.DoubleCellType();
                                db.ErrorMessage = "Enter only Numbers";
                                FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].CellType = db;

                                // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, i].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(cbl_month.Items[i].Text);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(cbl_month.Items[i].Value);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Columns[1].Width = 150;
                            }
                        }
                        for (int row = 0; row < 31; row++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(row + 1);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        }
                        DataView dv = new DataView();
                        string query = "";
                        string rebatetype = "";
                        if (rdbdate.Checked == true)
                        {
                            rebatetype = "1";
                        }
                        else
                        {
                            rebatetype = "2";
                        }
                        query = "select RebateActDays, RebateDays,RebateAmount  ,RebateMonth  from HM_RebateMaster where HostelFK in ('" + itemheadercode + "') and RebateMonth in ('" + month + "') and RebateType ='" + rebatetype + "'";
                        //}
                        //else
                        //{
                        //    rebatetype = "2";
                        //    query = "select Actual_Day, Grant_Day,Grant_Amount  ,Rebate_Month  from Rebate_Master where Hostel_Code in ('" + itemheadercode + "') and Rebate_Month in ('" + month + "') and Rebate_Type ='" + rebatetype + "'";
                        //}
                        ds1 = d2.select_method_wo_parameter(query, "Text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            if (FpSpread1.Sheets[0].ColumnCount > 2)
                            {
                                for (int ik = 2; ik < FpSpread1.Sheets[0].ColumnCount; ik++)
                                {
                                    string getmonthvalue = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, ik].Tag);
                                    if (FpSpread1.Sheets[0].RowCount > 0)
                                    {
                                        for (int row = 0; row < FpSpread1.Sheets[0].RowCount; row++)
                                        {
                                            string currentday = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Text);

                                            ds1.Tables[0].DefaultView.RowFilter = "RebateActDays='" + currentday + "' and RebateMonth='" + getmonthvalue + "'";
                                            dv = ds1.Tables[0].DefaultView;
                                            if (dv.Count > 0)
                                            {

                                                if (rdbdate.Checked == true)
                                                {

                                                    FpSpread1.Sheets[0].Cells[row, ik].Text = Convert.ToString(dv[0]["RebateDays"]);
                                                    FpSpread1.Sheets[0].Cells[row, ik].HorizontalAlign = HorizontalAlign.Right;
                                                }
                                                else
                                                {
                                                    FpSpread1.Sheets[0].Cells[row, ik].Text = Convert.ToString(dv[0]["RebateAmount"]);
                                                    FpSpread1.Sheets[0].Cells[row, ik].HorizontalAlign = HorizontalAlign.Right;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //25.04.16
                        //if (txt_allmonth.Text.Trim() != "")
                        //{
                        //    if (FpSpread1.Sheets[0].ColumnCount > 2)
                        //    {
                        //        for (int i = 0; i < FpSpread1.Sheets[0].ColumnCount - 2; i++)
                        //        {
                        //            int k = 0;
                        //            for (int row = 0; row < 31; row++)
                        //            {
                        //                k++;
                        //                FpSpread1.Sheets[0].Cells[k - 1, i + 2].Text = Convert.ToString(txt_allmonth.Text);
                        //                FpSpread1.Sheets[0].Cells[k - 1, i + 2].HorizontalAlign = HorizontalAlign.Center;
                        //                FpSpread1.Sheets[0].Cells[k - 1, i + 2].Font.Size = FontUnit.Medium;
                        //                FpSpread1.Sheets[0].Cells[k - 1, i + 2].Font.Name = "Book Antiqua";
                        //            }
                        //        }
                        //    }
                        //}
                    }
                    FpSpread1.Visible = true;
                    div_report.Visible = true;
                    //div1.Visible = true;
                    lblerror.Visible = false;
                    btnsave.Visible = true;
                    btn_reset.Visible = true;
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                }
            }
        }
        catch
        {
        }
    }

    public void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = new DateTime();
            string dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            FpSpread1.SaveChanges();

            if (cb_allowallmonth.Checked == true)
            {
                if (cbl_hostelname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                    {
                        if (cbl_hostelname.Items[i].Selected == true)
                        {
                            string hostelcode = Convert.ToString(cbl_hostelname.Items[i].Value);
                            if (FpSpread1.Sheets[0].ColumnCount > 2)
                            {
                                for (int ik = 2; ik < FpSpread1.Sheets[0].ColumnCount; ik++)
                                {
                                    for (int k = 0; k < cbl_month.Items.Count; k++)
                                    {
                                        if (cbl_month.Items[k].Selected == true)
                                        {
                                            string getmonthvalue = Convert.ToString(cbl_month.Items[k].Value);
                                            if (FpSpread1.Sheets[0].RowCount > 0)
                                            {
                                                for (int row = 0; row < FpSpread1.Sheets[0].RowCount; row++)
                                                {
                                                    string currentday = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Text);
                                                    string dayamount = Convert.ToString(FpSpread1.Sheets[0].Cells[row, ik].Text);
                                                    if (dayamount.Trim() != "" && dayamount.Trim() != null)
                                                    {
                                                        string query = "";
                                                        string rebatetype = "";
                                                        if (rdbdate.Checked == true)
                                                        {
                                                            if (cb_allowallmonth.Checked == true)
                                                            {

                                                                rebatetype = "1";
                                                                query = "if exists (select * from HM_RebateMaster where RebateActDays ='" + currentday + "' and RebateMonth='" + getmonthvalue + "' and  HostelFK='" + hostelcode + "' and RebateType='" + rebatetype + "') update HM_RebateMaster set RebateDays ='" + dayamount + "'  where  RebateActDays ='" + currentday + "' and RebateMonth='" + getmonthvalue + "' and  HostelFK='" + hostelcode + "' and RebateType='" + rebatetype + "'else insert into HM_RebateMaster (RebateType,RebateMonth,RebateDays,RebateActDays,HostelFK) values ('" + rebatetype + "','" + getmonthvalue + "','" + dayamount + "','" + currentday + "','" + hostelcode + "' )";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (cb_allowallmonth.Checked == true)
                                                            {
                                                                rebatetype = "2";
                                                                query = "if exists (select * from HM_RebateMaster where RebateActDays ='" + currentday + "' and RebateMonth='" + getmonthvalue + "' and  HostelFK='" + hostelcode + "' and RebateType='" + rebatetype + "') update HM_RebateMaster set RebateAmount ='" + dayamount + "'  where  RebateActDays ='" + currentday + "' and RebateMonth='" + getmonthvalue + "' and  HostelFK='" + hostelcode + "' and RebateType='" + rebatetype + "'else insert into HM_RebateMaster (RebateType,RebateMonth,RebateAmount,RebateActDays,HostelFK) values ('" + rebatetype + "','" + getmonthvalue + "','" + dayamount + "','" + currentday + "','" + hostelcode + "' )";
                                                            }
                                                        }
                                                        int ivalue = d2.update_method_wo_parameter(query, "Text");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                imgdiv2.Visible = true;
                                lblalerterr.Text = "Saved Successfully";
                            }
                        }
                    }
                }
            }
            else
            {
                if (cbl_hostelname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                    {
                        if (cbl_hostelname.Items[i].Selected == true)
                        {
                            string hostelcode = Convert.ToString(cbl_hostelname.Items[i].Value);
                            if (FpSpread1.Sheets[0].ColumnCount > 2)
                            {
                                for (int ik = 2; ik < FpSpread1.Sheets[0].ColumnCount; ik++)
                                {
                                    string getmonthvalue = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, ik].Tag);
                                    if (FpSpread1.Sheets[0].RowCount > 0)
                                    {
                                        for (int row = 0; row < FpSpread1.Sheets[0].RowCount; row++)
                                        {
                                            string currentday = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Text);
                                            string dayamount = Convert.ToString(FpSpread1.Sheets[0].Cells[row, ik].Text);
                                            if (dayamount.Trim() != "" && dayamount.Trim() != null)
                                            {
                                                string query = "";
                                                string rebatetype = "";
                                                if (rdbdate.Checked == true)
                                                {
                                                    rebatetype = "1";
                                                    query = "if exists (select * from HM_RebateMaster where RebateActDays ='" + currentday + "' and RebateMonth='" + getmonthvalue + "' and  HostelFK='" + hostelcode + "' and RebateType='" + rebatetype + "') update HM_RebateMaster set RebateDays ='" + dayamount + "'  where  RebateActDays ='" + currentday + "' and RebateMonth='" + getmonthvalue + "' and  HostelFK='" + hostelcode + "' and RebateType='" + rebatetype + "'else insert into HM_RebateMaster (RebateType,RebateMonth,RebateDays,RebateActDays,HostelFK) values ('" + rebatetype + "','" + getmonthvalue + "','" + dayamount + "','" + currentday + "','" + hostelcode + "' )";
                                                }
                                                else
                                                {
                                                    rebatetype = "2";
                                                    query = " if exists (select * from HM_RebateMaster where RebateActDays ='" + currentday + "' and RebateMonth='" + getmonthvalue + "' and  HostelFK='" + hostelcode + "' and RebateType='" + rebatetype + "') update HM_RebateMaster set RebateAmount ='" + dayamount + "'  where  RebateActDays ='" + currentday + "' and RebateMonth='" + getmonthvalue + "' and  HostelFK='" + hostelcode + "' and RebateType='" + rebatetype + "'else insert into HM_RebateMaster (RebateType,RebateMonth,RebateAmount,RebateActDays,HostelFK) values ('" + rebatetype + "','" + getmonthvalue + "','" + dayamount + "','" + currentday + "','" + hostelcode + "' )";
                                                }

                                                int ivalue = d2.update_method_wo_parameter(query, "Text");
                                            }
                                        }
                                    }
                                }
                                imgdiv2.Visible = true;
                                lblalerterr.Text = "Saved Successfully";
                            }
                        }
                    }
                }
            }
            btn_go_Click(sender, e);
        }
        catch
        {
        }
    }
    public void btn_reset_Click(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges();
        if (FpSpread1.Sheets[0].RowCount > 0)
        {
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                for (int j = 2; j < FpSpread1.Sheets[0].ColumnCount; j++)
                {
                    FpSpread1.Sheets[0].Cells[i, j].Text = "";
                }
            }
        }
        FpSpread1.SaveChanges();
    }

    protected void txtexcelname_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txt_excelname.Visible = true;
            btn_Excel.Visible = true;
            btn_printmaster.Visible = true;
            lbl_reportname.Visible = true;
            btn_Excel.Focus();
        }
        catch (Exception ex)
        {

        }
    }
    public void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {
                //  FpSpread1.Sheets[0].Columns[1].Visible = false;
                d2.printexcelreport(FpSpread1, report);
                lbl_norec.Visible = false;
            }
            else
            {
                lbl_norec.Text = "Please Enter Your Report Name";
                lbl_norec.Visible = true;
            }
            btn_Excel.Focus();
        }
        catch (Exception ex)
        {
            lbl_norec.Text = ex.ToString();
        }
    }
    public void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string date = "@" + "Date :" + System.DateTime.Now.ToString("dd/MM/yyy");
            string Month = "";
            if (cb_hostelname.Checked == true)
            {
                Month = "@" + "Month : " + cbl_month.SelectedItem.ToString();
            }
            string pagename = "HM_RebateMaster.aspx";
            string rebatedetails = "Rebate Report" + Month + date;
            Printcontrol.loadspreaddetails(FpSpread1, pagename, rebatedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
}

