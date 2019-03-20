using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

public partial class DeductionUpdate : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string usercode = string.Empty;
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
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                loadheaderandledger();
                ledgerload();
                bindsem();
                loadReason();
            }
        }
        catch (Exception ex) { }
    }
    #region headerandledger
    public void loadheaderandledger()
    {
        try
        {

            chkl_studhed.Items.Clear();
            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode + "  ";

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


            string query1 = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + collegecode + "  and L.HeaderFK in('" + hed + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
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
            CallCheckboxChange(chk_studhed, chkl_studhed, txt_studhed, "Header", "--Select--");
            ledgerload();
        }
        catch (Exception ex)
        { }
    }

    public void chkl_studhed_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chk_studhed, chkl_studhed, txt_studhed, "Header", "--Select--");
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
            CallCheckboxChange(chk_studled, chkl_studled, txt_studled, "Ledger", "--Select--");

        }
        catch (Exception ex)
        { }
    }
    public void chkl_studled_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chk_studled, chkl_studled, txt_studled, "Ledger", "--Select--");
        }
        catch (Exception ex)
        { }
    }
    #endregion
    protected void bindsem()
    {
        try
        {
            string sem = "";
            ddlsem.Items.Clear();
            string semyear = "select * from New_InsSettings where linkname = 'SemesterandYear' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'";
            DataSet dsset = new DataSet();
            dsset.Clear();
            dsset = d2.select_method_wo_parameter(semyear, "Text");
            if (dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
            {
                string value = Convert.ToString(dsset.Tables[0].Rows[0]["LinkValue"]);
                if (value == "1")
                {

                    string SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and (textval like '%Semester' or textval like '%Year') and textval not like '-1%' and college_code ='" + collegecode + "' order by len(textval),textval asc";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(SelectQ, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
                        ddlsem.DataSource = ds;
                        ddlsem.DataTextField = "TextVal";
                        ddlsem.DataValueField = "TextCode";
                        ddlsem.DataBind();
                    }
                }
                else
                {
                    string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(settingquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
                        if (linkvalue == "0")
                        {
                            string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester' and textval not like '-1%' and college_code ='" + collegecode + "' order by len(textval),textval asc";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(semesterquery, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                //text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
                                ddlsem.DataSource = ds;
                                ddlsem.DataTextField = "TextVal";
                                ddlsem.DataValueField = "TextCode";
                                ddlsem.DataBind();
                            }
                        }
                        else
                        {
                            string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Year' and textval not like '-1%' and college_code ='" + collegecode + "' order by len(textval),textval asc";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(semesterquery, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                // text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
                                ddlsem.DataSource = ds;
                                ddlsem.DataTextField = "TextVal";
                                ddlsem.DataValueField = "TextCode";
                                ddlsem.DataBind();
                            }
                        }
                    }
                }
            }
        }
        catch { }
    }

    protected void loadReason()
    {
        try
        {
            ddldeduct.Items.Clear();
            ds.Clear();
            string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='DedRe' and college_code ='" + collegecode + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldeduct.DataSource = ds;
                ddldeduct.DataTextField = "TextVal";
                ddldeduct.DataValueField = "TextCode";
                ddldeduct.DataBind();
            }

        }
        catch
        { }
    }

    protected void btnupdate_Click(object sender, EventArgs e)
    {
        try
        {
            string roll = Convert.ToString(txtroll.Text);
            if (!string.IsNullOrEmpty(roll))
            {
                string feecat = Convert.ToString(ddlsem.SelectedItem.Value);
                string deductcode = Convert.ToString(ddldeduct.SelectedItem.Value);
                string hedg = getCblSelectedValue(chkl_studhed);
                string ledg = getCblSelectedValue(chkl_studled);
                string appno = d2.GetFunction(" select App_No from Registration where reg_no='" + roll + "'");
                if (!string.IsNullOrEmpty(appno) && appno != "0" && !string.IsNullOrEmpty(feecat) && !string.IsNullOrEmpty(hedg) && !string.IsNullOrEmpty(ledg))
                {
                    loadDetailsFFC(appno, deductcode, feecat, hedg, ledg);
                    //if (rbmode.SelectedIndex == 0)
                    //{                   
                    //    loadDetailsFFC(appno, deductcode, feecat, hedg, ledg);
                    //}
                    //else
                    //{                   
                    //    loadDetailsSFC(appno, deductcode, feecat, hedg, ledg);
                    //}
                }
                else
                {
                    lbl_alert.Text = "Please Enter Correct Values";
                    imgdiv2.Visible = true;
                }
            }
            else
            {
                lbl_alert.Text = "Please Enter The Reg No";
                imgdiv2.Visible = true;
            }
        }
        catch { }
    }


    protected void loadDetailsFFC(string rollno, string deductcode, string feecat, string hedg, string ledg)
    {
        try
        {
            string update = "  update ft_feeallot set deductreason='" + deductcode + "' where app_no='" + rollno + "' and headerfk in('" + hedg + "') and ledgerfk in('" + ledg + "')  and feecategory='" + feecat + "'";
            //and feeAmount<>TotalAmount 
            int upd = d2.update_method_wo_parameter(update, "Text");
            if (upd > 0)
            {
                txtroll.Text = "";
                lbl_alert.Text = "Updated Successfully";
                imgdiv2.Visible = true;
            }
            else
            {
                lbl_alert.Text = " Not Updated";
                imgdiv2.Visible = true;
            }
        }
        catch { }
    }

    //protected void loadDetailsSFC(string rollno, string deductcode, string feecat, string hedg, string ledg)
    //{
    //    try
    //    {
    //        string update = "update ft_feeallot set deductreason='" + deductcode + "' where app_no='" + rollno + "' and headerfk in('" + hedg + "') and ledgerfk in('" + ledg + "') and feecategory='" + feecat + "' ";
    //        //and feeAmount<>TotalAmount 
    //        int upd = d2.update_method_wo_parameter(update, "Text");
    //        if (upd > 0)
    //        {
    //            txtroll.Text = "";
    //            Response.Write("<script>alert('saved successfully')</script>");
    //        }
    //    }
    //    catch { }
    //}




    //protected void rbmode_Selected(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (rbmode.SelectedIndex == 0)
    //        {
    //            txtroll.Text = "";
    //        }
    //        else
    //        {

    //            txtroll.Text = "";
    //        }
    //    }
    //    catch { }
    //}

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

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
}