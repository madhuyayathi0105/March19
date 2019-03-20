using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using InsproDataAccess;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Drawing;

public partial class LibraryMod_JournalLetterReport : System.Web.UI.Page
{
    #region Field Declaration
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 dacces2 = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds1 = new DataSet();
    #endregion

    #region page load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("LibraryHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/LibraryMod/LibraryHome.aspx");
                    return;
                }
            }
           
                userCollegeCode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
                userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
         
            if (!IsPostBack)
            {
                string Date = d2.ServerDate();
                string[] dat = Date.Split('/');
                if (dat.Length == 3)
                    Date = dat[1] + '/' + dat[0] + '/' + dat[2];
                txt_SubscriptionYear.Text = Date.Split(' ')[0];
                Bindreporttype();
                BindSupplierType();
                BindSupplierName();
                Bindlanguage();
                BindJournalName();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Journal_Letter_Report");
        }

    }
    #endregion page load

    #region reporttype

    public void Bindreporttype()
    {
        try
        {
            ddlreporttype.Items.Add("Proforma");
            ddlreporttype.Items.Add("proforma with Pending List");
            ddlreporttype.Items.Add("subscription List");
            ddlreporttype.Items.Add("Covering Letter");
            ddlreporttype.Items.Add("Local Supplier Price List");
            ddlreporttype.Items.Add("Journal Pending List");
            ddlreporttype.Text = "Journal Pending List";




        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Journal_Letter_Report");
        }
    }

    #endregion

    #region SupplierType

    public void BindSupplierType()
    {
        try
        {
            string selectQuery = "SELECT DISTINCT ISNULL(SupplierType,'') SupplierType FROM CO_VendorMaster WHERE 1=1  AND LibraryFlag='1' and SupplierType<>'NULL' ORDER BY SupplierType  ";
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklSupplierType.DataSource = ds;
                chklSupplierType.DataTextField = "SupplierType";
                chklSupplierType.DataValueField = "SupplierType";
                chklSupplierType.DataBind();
            }
            for (int i = 0; i < chklSupplierType.Items.Count; i++)
            {
                chklSupplierType.Items[i].Selected = true;

            }
            txtSupplierType.Text = lblSupplierType.Text + "(" + chklSupplierType.Items.Count + ")";
            chkSupplierType.Checked = true;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Journal_Letter_Report");
        }
    }

    #endregion

    #region SupplierName

    public void BindSupplierName()
    {
        try
        {
            string StrSupplierType = "";
            chklSupplierName.Items.Clear();
            for (int i = 0; chklSupplierType.Items.Count > i; i++)
            {
                if (chklSupplierType.Items[i].Selected == true)
                {
                    if (StrSupplierType == "")
                        StrSupplierType = "'" + chklSupplierType.Items[i].Text + "'";
                    else
                        StrSupplierType = StrSupplierType + ",'" + chklSupplierType.Items[i].Text + "'";
                }
            }
            string selectQuery = "SELECT VendorCompName FROM CO_VendorMaster WHERE 1=1  AND LibraryFlag='1' ";
            if (StrSupplierType.Trim() != "")
                selectQuery = selectQuery + " AND SupplierType IN (" + StrSupplierType + ") ";

            selectQuery = selectQuery + " ORDER BY VendorCompName ";
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklSupplierName.DataSource = ds;
                chklSupplierName.DataTextField = "VendorCompName";
                chklSupplierName.DataValueField = "VendorCompName";
                chklSupplierName.DataBind();
            }
            for (int i = 0; i < chklSupplierName.Items.Count; i++)
            {
                chklSupplierName.Items[i].Selected = true;

            }
            txtSupplierName.Text = lblSupplierName.Text + "(" + chklSupplierName.Items.Count + ")";
            chkSupplierName.Checked = true;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Journal_Letter_Report");
        }
    }

    #endregion

    #region language

    public void Bindlanguage()
    {
        try
        {
            chklchkLanguage.Items.Clear();
            chklchkLanguage.Items.Insert(0, "English");
            chklchkLanguage.Items.Insert(1, "Tamil");
            if (chklchkLanguage.Items.Count > 0)
            {
                for (int i = 0; i < chklchkLanguage.Items.Count; i++)
                {
                    chklchkLanguage.Items[i].Selected = true;

                }
                txtLanguage.Text = lblLanguage.Text + "(" + chklchkLanguage.Items.Count + ")";
                chkLanguage.Checked = true;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Journal_Letter_Report");
        }
    }

    #endregion

    #region JournalName

    public void BindJournalName()
    {
        try
        {
            string StrSupplierType = "";
            string StrSupplierName = "";
            string StrLanguage = "";
            Boolean BlnIsEnglish;
            Boolean BlnIsTamil;
            string lang = "";
            string langvalue = "";

            chklJournalName.Items.Clear();
            for (int i = 0; chklSupplierType.Items.Count > i; i++)
            {
                if (chklSupplierType.Items[i].Selected == true)
                {
                    if (StrSupplierType == "")
                        StrSupplierType = "'" + chklSupplierType.Items[i].Text + "'";
                    else
                        StrSupplierType = StrSupplierType + ",'" + chklSupplierType.Items[i].Text + "'";
                }
            }
            for (int i = 0; chklSupplierName.Items.Count > i; i++)
            {
                if (chklSupplierName.Items[i].Selected == true)
                {
                    if (StrSupplierName == "")
                        StrSupplierName = "'" + chklSupplierName.Items[i].Text + "'";
                    else
                        StrSupplierName = StrSupplierName + ",'" + chklSupplierName.Items[i].Text + "'";
                }
            }
            if (chklchkLanguage.Items.Count > 0)
                lang = Convert.ToString(d2.getCblSelectedValue(chklchkLanguage));
            if (lang == "English")
                langvalue = "0";
            else if (lang == "Tamil")
                langvalue = "1";
            else
                langvalue = "0','1";
            string selectQuery = "SELECT distinct Journal_Name FROM Journal_Master J,Library L,CO_VendorMaster S   WHERE J.Lib_Code = L.Lib_Code AND J.Supplier = S.VendorCompName ";
            selectQuery = selectQuery + " AND L.College_Code =" + userCollegeCode;
            if (StrSupplierType.Trim() != "")
                selectQuery = selectQuery + " AND SupplierType IN (" + StrSupplierType + ")";

            if (StrSupplierName.Trim() != "")
                selectQuery = selectQuery + " AND VendorCompName IN (" + StrSupplierName + ")";

            if (StrLanguage.Trim() != "")
                selectQuery = selectQuery + " AND ISNULL(TitleLanguage,0) IN (" + StrLanguage + ")";
            selectQuery = selectQuery + " and LibraryFlag='1' ORDER BY Journal_Name ";
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklJournalName.DataSource = ds;
                chklJournalName.DataTextField = "Journal_Name";
                chklJournalName.DataValueField = "Journal_Name";
                chklJournalName.DataBind();
            }
            for (int i = 0; i < chklJournalName.Items.Count; i++)
            {
                chklJournalName.Items[i].Selected = true;
            }
            txtJournalName.Text = lblJournalName.Text + "(" + chklJournalName.Items.Count + ")";
            chkJournalName.Checked = true;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Journal_Letter_Report");
        }
    }

    #endregion

    #region Index Changed Events

    protected void ddlreporttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (ddlreporttype.SelectedIndex == 0)
            {
                lbl_Proformafromdate1.Visible = true;
                txt_Proformafromdate1.Visible = true;
                lbl_SubscriptionYear.Visible = false;
                txt_SubscriptionYear.Visible = false;
                lbl_Proformatodate1.Visible = true;
                txt_Proformatodate1.Visible = true;
                lbl_DDAmount.Visible = false;
                txt_DDAmount.Visible = false;
                txt_localsupplierpricelist.Visible = false;

            }
            else if (ddlreporttype.SelectedIndex == 1)
            {
                lbl_Proformafromdate1.Visible = true;
                txt_Proformafromdate1.Visible = true;
                lbl_SubscriptionYear.Visible = false;
                txt_SubscriptionYear.Visible = false;
                lbl_Proformatodate1.Visible = true;
                txt_Proformatodate1.Visible = true;
                lbl_DDAmount.Visible = false;
                txt_DDAmount.Visible = false;
                txt_localsupplierpricelist.Visible = false;
            }
            else if (ddlreporttype.SelectedIndex == 2)
            {
                lbl_Proformafromdate1.Visible = false;
                txt_Proformafromdate1.Visible = false;
                lbl_SubscriptionYear.Visible = true;
                txt_SubscriptionYear.Visible = true;
                lbl_Proformatodate1.Visible = false;
                txt_Proformatodate1.Visible = false;
                lbl_DDAmount.Visible = true;
                txt_DDAmount.Visible = true;
                txt_localsupplierpricelist.Visible = false;
            }
            else if (ddlreporttype.SelectedIndex == 3)
            {
                lbl_Proformafromdate1.Visible = false;
                txt_Proformafromdate1.Visible = false;
                lbl_SubscriptionYear.Visible = false;
                txt_SubscriptionYear.Visible = false;
                lbl_Proformatodate1.Visible = false;
                txt_Proformatodate1.Visible = false;
                lbl_DDAmount.Visible = false;
                txt_DDAmount.Visible = false;
                txt_localsupplierpricelist.Visible = false;
            }
            else if (ddlreporttype.SelectedIndex == 4)
            {
                lbl_Proformafromdate1.Visible = false;
                txt_Proformafromdate1.Visible = false;
                lbl_SubscriptionYear.Visible = false;
                txt_SubscriptionYear.Visible = false;
                lbl_Proformatodate1.Visible = false;
                txt_Proformatodate1.Visible = false;
                lbl_DDAmount.Visible = false;
                txt_DDAmount.Visible = false;
                txt_localsupplierpricelist.Visible = true;
            }
            else if (ddlreporttype.SelectedIndex == 5)
            {
                lbl_Proformafromdate1.Visible = false;
                txt_Proformafromdate1.Visible = false;
                lbl_SubscriptionYear.Visible = false;
                txt_SubscriptionYear.Visible = false;
                lbl_Proformatodate1.Visible = false;
                txt_Proformatodate1.Visible = false;
                lbl_DDAmount.Visible = false;
                txt_DDAmount.Visible = false;
                txt_localsupplierpricelist.Visible = false;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Journal_Letter_Report");
        }
    }

    protected void chkSupplierType_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chkSupplierType, chklSupplierType, txtSupplierType, lblSupplierType.Text, "--Select--");
            BindSupplierName();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Journal_Letter_Report");
        }

    }

    protected void chklSupplierType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chkSupplierType, chklSupplierType, txtSupplierType, lblSupplierType.Text, "--Select--");

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Journal_Letter_Report");
        }
    }

    protected void chkSupplierName_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chkSupplierName, chklSupplierName, txtSupplierName, lblSupplierName.Text, "--Select--");
            //  BindJournalName();

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Journal_Letter_Report");
        }
    }

    protected void chklSupplierName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chkSupplierName, chklSupplierName, txtSupplierName, lblSupplierName.Text, "--Select--");


        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Journal_Letter_Report");
        }
    }

    protected void chkLanguage_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chkLanguage, chklchkLanguage, txtLanguage, lblLanguage.Text, "--Select--");
            BindJournalName();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Journal_Letter_Report");
        }
    }

    protected void chklchkLanguage_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxListChange(chkLanguage, chklchkLanguage, txtLanguage, lblLanguage.Text, "--Select--");

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Journal_Letter_Report");
        }
    }

    protected void chkJournalName_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxChange(chkJournalName, chklJournalName, txtJournalName, lblJournalName.Text, "--Select--");
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Journal_Letter_Report");
        }

    }

    protected void chklJournalName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


            CallCheckboxListChange(chkJournalName, chklJournalName, txtJournalName, lblJournalName.Text, "--Select--");
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Journal_Letter_Report");
        }
    }

    #endregion

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

    #region go

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string Sql = string.Empty;
            DataTable dtJournal = new DataTable();
            DataRow drow;
            int Sno = 0;
            if (ddlreporttype.Text == "Local Supplier Price List")
            {
                string StrSupType = "";
                string StrSupName = "";
                string StrJrnlName = "";
                int intSNo = 1;
                for (int i = 0; chklSupplierType.Items.Count > i; i++)
                {
                    if (chklSupplierType.Items[i].Selected == true)
                    {
                        if (StrSupType.Trim() == "")
                            StrSupType = "'" + chklSupplierType.Items[i].Text + "'";
                        else
                            StrSupType = StrSupType + ",'" + chklSupplierType.Items[i].Text + "'";
                    }
                }
                if (StrSupType.Trim() != "")
                    StrSupType = " AND SupplierType IN (" + StrSupType + ")";
                for (int i = 0; chklSupplierName.Items.Count > i; i++)
                {
                    if (chklSupplierName.Items[i].Selected == true)
                    {
                        if (StrSupName.Trim() == "")
                            StrSupName = "'" + chklSupplierName.Items[i].Text + "'";
                        else
                            StrSupName = StrSupName + ",'" + chklSupplierName.Items[i].Text + "'";
                    }
                }
                if (StrSupName.Trim() != "")
                    StrSupName = " AND VendorCompName IN (" + StrSupName + ")";

                for (int i = 0; chklJournalName.Items.Count > i; i++)
                {
                    if (chklJournalName.Items[i].Selected == true)
                    {
                        if (StrJrnlName.Trim() == "")
                            StrJrnlName = "'" + chklJournalName.Items[i].Text + "'";
                        else
                            StrJrnlName = StrJrnlName + ",'" + chklJournalName.Items[i].Text + "'";
                    }
                }
                if (StrJrnlName.Trim() != "")
                    StrJrnlName = " AND J.Title IN(" + StrJrnlName + ")";
                Sql = "SELECT U.VendorCode,VendorCompName,J.Journal_Code,J.Title,COUNT(*) Quantity,ISNULL(Price,0) Price,TitleLanguage ";
                Sql = Sql + " FROM Journal J,subscription S,CO_VendorMaster U,Journal_Master M ";
                Sql = Sql + " Where j.Journal_Code = S.Journal_Code and LibraryFlag='1' And j.Subs_Year = S.Subscription_Year";
                Sql = Sql + " AND S.Supplier_Code = U.VendorCode AND J.Journal_Code = M.Journal_Code ";
                //Sql = Sql + " and Month(Issue_Month) =" + txt_localsupplierpricelist.Text + " AND Year(Issue_Month) =" + txt_localsupplierpricelist.Text;
                Sql = Sql + StrSupType + StrSupName + StrJrnlName;
                Sql = Sql + " Group By U.VendorCode,VendorCompName,J.journal_code,J.title,Price,TitleLanguage";
                Sql = Sql + " ORDER BY VendorCompName,J.Journal_Code,J.title ";
                ds1 = d2.select_method_wo_parameter(Sql, "text");

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    dtJournal.Columns.Add("SNo", typeof(string));
                    dtJournal.Columns.Add("Supplier Code", typeof(string));
                    dtJournal.Columns.Add("Supplier Name", typeof(string));
                    dtJournal.Columns.Add("Journal Code", typeof(string));
                    dtJournal.Columns.Add("Title", typeof(string));
                    dtJournal.Columns.Add("Quantity", typeof(string));
                    dtJournal.Columns.Add("Price", typeof(string));
                    dtJournal.Columns.Add("Title Language", typeof(string));


                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        Sno++;
                        drow = dtJournal.NewRow();
                        drow["SNo"] = Sno;
                        drow["Supplier Code"] = Convert.ToString(ds1.Tables[0].Rows[i]["VendorCode"]);
                        drow["Supplier Name"] = Convert.ToString(ds1.Tables[0].Rows[i]["VendorCompName"]);
                        drow["Journal Code"] = Convert.ToString(ds1.Tables[0].Rows[i]["Journal_Code"]);
                        drow["Title"] = Convert.ToString(ds1.Tables[0].Rows[i]["Title"]);
                        drow["Quantity"] = Convert.ToString(ds1.Tables[0].Rows[i]["Quantity"]);
                        drow["Price"] = Convert.ToString(ds1.Tables[0].Rows[i]["Price"]);
                        string titleLang = Convert.ToString(ds1.Tables[0].Rows[i]["TitleLanguage"]);
                        string Lang = "";
                        if (!string.IsNullOrEmpty(titleLang))
                        {
                            if (titleLang == "1")
                                Lang = "Tamil";
                            if (titleLang == "0")
                                Lang = "English";
                        }
                        else
                        {
                            Lang = "";
                        }

                        drow["Title Language"] = Lang;
                        dtJournal.Rows.Add(drow);
                    }

                    grdJournal.DataSource = dtJournal;
                    grdJournal.DataBind();
                    grdJournal.Visible = true;
                    rptprint1.Visible = true;
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Found";
                    grdJournal.Visible = false;
                    rptprint1.Visible = false;
                }
            }
            else if (ddlreporttype.Text == "Journal Pending List")
            {
                string StrSupType = "";
                string StrSupName = "";
                string StrJrnlName = "";

                for (int i = 0; chklSupplierType.Items.Count > i; i++)
                {
                    if (chklSupplierType.Items[i].Selected == true)
                    {
                        if (StrSupType.Trim() == "")
                            StrSupType = "'" + chklSupplierType.Items[i].Text + "'";
                        else
                            StrSupType = StrSupType + ",'" + chklSupplierType.Items[i].Text + "'";
                    }
                }
                if (StrSupType.Trim() != "")
                    StrSupType = " AND SupplierType IN (" + StrSupType + ")";
                for (int i = 0; chklSupplierName.Items.Count > i; i++)
                {
                    if (chklSupplierName.Items[i].Selected == true)
                    {
                        if (StrSupName.Trim() == "")
                            StrSupName = "'" + chklSupplierName.Items[i].Text + "'";
                        else
                            StrSupName = StrSupName + ",'" + chklSupplierName.Items[i].Text + "'";
                    }
                }
                if (StrSupName.Trim() != "")
                    StrSupName = " AND VendorCompName IN (" + StrSupName + ")";

                for (int i = 0; chklJournalName.Items.Count > i; i++)
                {
                    if (chklJournalName.Items[i].Selected == true)
                    {
                        if (StrJrnlName.Trim() == "")
                            StrJrnlName = "'" + chklJournalName.Items[i].Text + "'";
                        else
                            StrJrnlName = StrJrnlName + ",'" + chklJournalName.Items[i].Text + "'";
                    }
                }
                if (StrJrnlName.Trim() != "")
                    StrJrnlName = " AND Journal_Name IN(" + StrJrnlName + ")";

                Sql = "SELECT VendorCompName,Journal_Type,M.journal_name ,I.Subs_Year,IssueMonth,IssueNo,ISNULL(s.Journal_Price ,0) Journal_Price,TitleLanguage ";
                Sql = Sql + "FROM Journal_Issues I,Journal_Master M,Subscription S,CO_VendorMaster U,library L ";
                Sql = Sql + "Where 1 = 1 and LibraryFlag='1' ";
                Sql = Sql + "and i.Journal_Code = M.journal_code and I.Journal_Code = S.Journal_Code  and I.Subs_Year = S.Subscription_Year ";
                Sql = Sql + "and S.Supplier_Code = U.VendorCode AND I.Lib_Code = L.lib_code and M.lib_code = L.lib_code AND S.lib_code = L.lib_code ";
                Sql = Sql + "AND Issue_Status = 0 ";
                string subDate = txt_SubscriptionYear.Text;
                string[] dat = subDate.Split('/');
                string year = dat[2];
                DateTime curdate = System.DateTime.Now;
                string dateval = Convert.ToString(curdate);
                string[] datmonth = subDate.Split('/');
                if (datmonth.Length == 3)
                    dateval = datmonth[2].Split(' ')[0];
                int yearcheck = Convert.ToInt32(dateval);

                if (Convert.ToInt32(year) >= yearcheck)
                    Sql = Sql + "AND IssueMonthNum <= " + datmonth[1];

                if (txt_SubscriptionYear.Text != "")
                {

                    Sql = Sql + " AND I.Subs_Year ='" + dat[2] + "'";
                }
                Sql = Sql + StrSupType + StrSupName + StrJrnlName;
                Sql = Sql + "Order By Subscription_Year,Journal_Name,IssueNo ";

                ds1 = d2.select_method_wo_parameter(Sql, "text");

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    dtJournal.Columns.Add("SNo", typeof(string));
                    dtJournal.Columns.Add("Supplier Name", typeof(string));
                    dtJournal.Columns.Add("Journal Type", typeof(string));
                    dtJournal.Columns.Add("Journal Name", typeof(string));
                    dtJournal.Columns.Add("Subscription Year", typeof(string));
                    dtJournal.Columns.Add("Issue Month", typeof(string));
                    dtJournal.Columns.Add("Issue No", typeof(string));
                    dtJournal.Columns.Add("Journal Price", typeof(string));
                    dtJournal.Columns.Add("Title Language", typeof(string));

                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        Sno++;
                        drow = dtJournal.NewRow();
                        drow["SNo"] = Sno;
                        drow["Supplier Name"] = Convert.ToString(ds1.Tables[0].Rows[i]["VendorCompName"]);
                        drow["Journal Type"] = Convert.ToString(ds1.Tables[0].Rows[i]["Journal_Type"]);
                        drow["Journal Name"] = Convert.ToString(ds1.Tables[0].Rows[i]["journal_name"]);
                        drow["Subscription Year"] = Convert.ToString(ds1.Tables[0].Rows[i]["Subs_Year"]);
                        drow["Issue Month"] = Convert.ToString(ds1.Tables[0].Rows[i]["IssueMonth"]);
                        drow["Issue No"] = Convert.ToString(ds1.Tables[0].Rows[i]["IssueNo"]);
                        drow["Journal Price"] = Convert.ToString(ds1.Tables[0].Rows[i]["Journal_Price"]);

                        string titleLang = Convert.ToString(ds1.Tables[0].Rows[i]["TitleLanguage"]);
                        string Lang = "";
                        if (!string.IsNullOrEmpty(titleLang))
                        {
                            if (titleLang == "1")
                                Lang = "Tamil";
                            if (titleLang == "0")
                                Lang = "English";
                        }
                        else
                        {
                            Lang = "";
                        }
                        drow["Title Language"] = Lang;
                        dtJournal.Rows.Add(drow);
                    }
                    grdJournal.DataSource = dtJournal;
                    grdJournal.DataBind();
                    grdJournal.Visible = true;
                    rptprint1.Visible = true;
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Found";
                    grdJournal.Visible = false;
                    rptprint1.Visible = false;
                }
                MergeRows(grdJournal);
            }
            else
            {
                string StrSupType = "";
                string StrSupName = "";
                string StrJrnlName = "";
                for (int i = 0; chklSupplierType.Items.Count > i; i++)
                {
                    if (chklSupplierType.Items[i].Selected == true)
                    {
                        if (StrSupType.Trim() == "")
                            StrSupType = "'" + chklSupplierType.Items[i].Text + "'";
                        else
                            StrSupType = StrSupType + ",'" + chklSupplierType.Items[i].Text + "'";
                    }
                }
                if (StrSupType.Trim() != "")
                    StrSupType = " AND SupplierType IN (" + StrSupType + ")";

                for (int i = 0; chklSupplierName.Items.Count > i; i++)
                {
                    if (chklSupplierName.Items[i].Selected == true)
                    {
                        if (StrSupName.Trim() == "")
                            StrSupName = "'" + chklSupplierName.Items[i].Text + "'";
                        else
                            StrSupName = StrSupName + ",'" + chklSupplierName.Items[i].Text + "'";
                    }
                }
                if (StrSupName.Trim() != "")
                    StrSupName = " AND VendorCompName IN (" + StrSupName + ")";

                for (int i = 0; chklJournalName.Items.Count > i; i++)
                {
                    if (chklJournalName.Items[i].Selected == true)
                    {
                        if (StrJrnlName.Trim() == "")
                            StrJrnlName = "'" + chklJournalName.Items[i].Text + "'";
                        else
                            StrJrnlName = StrJrnlName + ",'" + chklJournalName.Items[i].Text + "'";
                    }
                }
                if (StrJrnlName.Trim() != "")
                    StrJrnlName = " AND Journal_Name IN(" + StrJrnlName + ")";

                Sql = "SELECT VendorCode,VendorCompName,Journal_Code,Journal_Name,SubsAmount,TitleLanguage ";
                Sql = Sql + "FROM Journal_Master M ,CO_VendorMaster S ";
                Sql = Sql + "WHERE M.Supplier = S.VendorCompName and LibraryFlag='1' ";
                Sql = Sql + StrSupType + StrSupName + StrJrnlName;
                Sql = Sql + " ORDER BY VendorCompName,Journal_Name ";
                ds1 = d2.select_method_wo_parameter(Sql, "text");

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    dtJournal.Columns.Add("SNo", typeof(string));
                    dtJournal.Columns.Add("Supplier Code", typeof(string));
                    dtJournal.Columns.Add("Supplier Name", typeof(string));
                    dtJournal.Columns.Add("Journal Code", typeof(string));
                    dtJournal.Columns.Add("Journal Name", typeof(string));
                    dtJournal.Columns.Add("Subscription Amount", typeof(string));
                    dtJournal.Columns.Add("Title Language", typeof(string));

                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        Sno++;
                        drow = dtJournal.NewRow();
                        drow["SNo"] = Sno;
                        drow["Supplier Code"] = Convert.ToString(ds1.Tables[0].Rows[i]["VendorCode"]);
                        drow["Supplier Name"] = Convert.ToString(ds1.Tables[0].Rows[i]["VendorCompName"]);
                        drow["Journal Code"] = Convert.ToString(ds1.Tables[0].Rows[i]["Journal_Code"]);
                        drow["Journal Name"] = Convert.ToString(ds1.Tables[0].Rows[i]["Journal_Name"]);
                        drow["Subscription Amount"] = Convert.ToString(ds1.Tables[0].Rows[i]["SubsAmount"]);
                        string titleLang = Convert.ToString(ds1.Tables[0].Rows[i]["TitleLanguage"]);
                        string Lang = "";
                        if (!string.IsNullOrEmpty(titleLang))
                        {
                            if (titleLang == "1")
                                Lang = "Tamil";
                            if (titleLang == "0")
                                Lang = "English";
                        }
                        else
                        {
                            Lang = "";
                        }

                        drow["Title Language"] = Lang;
                        dtJournal.Rows.Add(drow);
                    }
                    grdJournal.DataSource = dtJournal;
                    rptprint1.Visible = true;
                    grdJournal.DataBind();
                    grdJournal.Visible = true;
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Found";
                    grdJournal.Visible = false;
                    rptprint1.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Journal_Letter_Report");
        }
    }

    public static void MergeRows(GridView grdjournalBook)
    {
        string suppName = grdjournalBook.HeaderRow.Cells[1].Text;
        string JourTyp = grdjournalBook.HeaderRow.Cells[2].Text;
        string JourName = grdjournalBook.HeaderRow.Cells[3].Text;

        for (int rowIndex = grdjournalBook.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridViewRow row = grdjournalBook.Rows[rowIndex];
            GridViewRow previousRow = grdjournalBook.Rows[rowIndex + 1];

            for (int i = 0; i < row.Cells.Count; i++)
            {
                if (grdjournalBook.HeaderRow.Cells[i].Text.ToLower() == suppName.ToLower() || grdjournalBook.HeaderRow.Cells[i].Text.ToLower() == JourTyp.ToLower() || grdjournalBook.HeaderRow.Cells[i].Text.ToLower() == JourName.ToLower())
                {
                    if (row.Cells[i].Text == previousRow.Cells[i].Text)
                    {
                        row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                               previousRow.Cells[i].RowSpan + 1;
                        previousRow.Cells[i].Visible = false;
                    }
                }
            }
        }
    }

    protected void grdJournal_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
    }

    #endregion

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        lbl_norec1.Visible = false;
        try
        {
            string reportname = "Journal Letter Report";
            if (reportname.ToString().Trim() != "")
            {

                d2.printexcelreportgrid(grdJournal, reportname);

                lbl_norec1.Visible = false;
            }
            else
            {
                lbl_norec1.Text = "Please Enter Your Report Name";
                lbl_norec1.Visible = true;
                txtexcelname1.Focus();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Journal_Letter_Report");
        }
    }

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {

            string duebooks = "Journal Letter Report";
            string pagename = "JournalLetterReport.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrol.loadspreaddetails(grdJournal, pagename, duebooks, 0, ss, "1");
            Printcontrol.Visible = true;
            lbl_norec1.Visible = false;
        }

        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Journal_Letter_Report");
        }
    }

    #region alertclose
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblalerterr.Text = string.Empty;
            alertpopwindow.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Journal_Letter_Report");
        }
    }


    #endregion


}