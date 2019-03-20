using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class FinanceMod_AllotUpdateBatch : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    ReuasableMethods reuse = new ReuasableMethods();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
            Response.Redirect("Default.aspx");
        usercode = Session["usercode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (!IsPostBack)
        {
            loadcollege();
            if (ddlcollegename.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
            bindBtch();
            loadheaderandledger();
            ledgerload();
        }
    }
    public void loadcollege()
    {
        ddlcollegename.Items.Clear();
        reuse.bindCollegeToDropDown(usercode, ddlcollegename);
    }
    public void bindBtch()
    {
        try
        {
            ddlyear.Items.Clear();
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlyear.DataSource = ds;
                ddlyear.DataTextField = "batch_year";
                ddlyear.DataValueField = "batch_year";
                ddlyear.DataBind();
            }
        }
        catch { }
    }
    #region headerandledger
    public void loadheaderandledger()
    {
        try
        {
            string clgvalue = ddlcollegename.SelectedItem.Value.ToString();
            chkl_studhed.Items.Clear();
            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + clgvalue + "  ";

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
                txt_studhed.Text = lblheader.Text + "(" + chkl_studhed.Items.Count + ")";
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
            string clgvalue = ddlcollegename.SelectedItem.Value.ToString();
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


            string query1 = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + clgvalue + "  and L.HeaderFK in('" + hed + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
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
        CallCheckboxChange(chk_studhed, chkl_studhed, txt_studhed, lblheader.Text, "--Select--");
        ledgerload();
    }

    public void chkl_studhed_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chk_studhed, chkl_studhed, txt_studhed, lblheader.Text, "--Select--");
        ledgerload();
    }
    public void chk_studled_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chk_studled, chkl_studled, txt_studled, "Ledger", "--Select--");
    }
    public void chkl_studled_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chk_studled, chkl_studled, txt_studled, "Ledger", "--Select--");
    }
    #endregion
    protected void ddlcollegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcollegename.Items.Count > 0)
            collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
        bindBtch();
        loadheaderandledger();
        ledgerload();
    }
    protected void btnDel_Click(object sender, EventArgs e)
    {
        try
        {
            System.Text.StringBuilder SBroll = new System.Text.StringBuilder();
            string batch = Convert.ToString(ddlyear.SelectedItem.Value);
            string collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
            string hdFK = Convert.ToString(getCblSelectedValue(chkl_studhed));
            string ldFK = Convert.ToString(getCblSelectedValue(chkl_studled));
            bool boolroll = false;
            string roll = Convert.ToString(txtroll.Text);
            if (!string.IsNullOrEmpty(roll) && roll != "0")
            {
                string[] splroll = roll.Split(',');
                if (splroll.Length > 0)
                {
                    for (int i = 0; i < splroll.Length; i++)
                    {
                        SBroll.Append(splroll[i] + ",");
                    }
                }
                if (SBroll.Length > 0)
                {
                    SBroll.Remove(SBroll.Length - 1, 1);
                    boolroll = true;
                }
            }
            bool check = false;
            if (!string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(hdFK) && !string.IsNullOrEmpty(ldFK))
            {
                string selQ = "     select sum(totalamount) as tot,sum(paidamount) as paid,sum(balamount) as bal,feecategory,ledgerfk,f.app_no,degree_code,batch_year from ft_feeallot f ,registration r where r.app_no=f.app_no  and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0 and r.batch_year in('" + batch + "')  and r.college_code='" + collegecode + "' and f.headerfk in ('" + hdFK + "') and f.ledgerFK in('" + ldFK + "') ";
                if (boolroll)
                    selQ += " and f.app_no in ('" + SBroll.ToString() + "')";
                selQ += " group by feecategory,ledgerfk,f.app_no,degree_code,batch_year having sum(isnull(totalamount,'0')) =sum(isnull(paidamount,'0')) and sum(isnull(paidamount,'0')) =sum(isnull(balamount,'0')) and sum(isnull(balamount,'0'))<>'0'";//and f.app_no='13875'
                selQ += "   select sum(debit) as tot,feecategory,ledgerfk,f.app_no,degree_code,batch_year from ft_findailytransaction f,registration r where r.app_no=f.app_no  and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0 and r.batch_year in('" + batch + "')  and r.college_code='" + collegecode + "' and f.headerfk in ('" + hdFK + "') and f.ledgerFK in('" + ldFK + "') and isnull(transcode,'')<>'' and isnull(iscanceled,'0')='0' and memtype='1' ";
                if (boolroll)
                    selQ += " and f.app_no in ('" + SBroll.ToString() + "')";
                selQ += " group by feecategory,ledgerfk,f.app_no,degree_code,batch_year having sum(isnull(debit,'0'))<>'0' ";//and f.app_no='13875'
                ds.Clear();
                ds = d2.select_method_wo_parameter(selQ, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            string str = "app_no='" + ds.Tables[0].Rows[row]["app_no"] + "' and feecategory='" + ds.Tables[0].Rows[row]["feecategory"] + "' and ledgerfk='" + ds.Tables[0].Rows[row]["ledgerfk"] + "' and degree_code='" + ds.Tables[0].Rows[row]["degree_code"] + "' and batch_year='" + ds.Tables[0].Rows[row]["batch_year"] + "'";
                            ds.Tables[1].DefaultView.RowFilter = str;
                            DataView dv = ds.Tables[1].DefaultView;
                            if (dv.Count > 0)
                            {
                                string updQ = " update ft_feeallot set paidamount=totalamount, balamount='0' where app_no='" + ds.Tables[0].Rows[row]["app_no"] + "' and feecategory='" + ds.Tables[0].Rows[row]["feecategory"] + "' and ledgerfk='" + ds.Tables[0].Rows[row]["ledgerfk"] + "'";
                                int upd = d2.update_method_wo_parameter(updQ, "Text");
                                check = true;
                            }
                        }
                    }
                }
                if (check)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('updated Successfully')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not updated')", true);
                }
            }
        }
        catch { }
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
}