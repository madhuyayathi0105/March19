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


public partial class LibraryMod_library_Entry_Correction : System.Web.UI.Page
{
    Hashtable has = new Hashtable();
    DAccess2 da = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string collegeCode = string.Empty;
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    DataTable dtCommon = new DataTable();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    ReuasableMethods rs = new ReuasableMethods();
    Hashtable columnhash = new Hashtable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
    bool flag_true = false;
    string group_user = string.Empty;
    string singleuser = string.Empty;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    Boolean Cellclick = false;
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

           
            //txt_from.Attributes.Add("readonly", "readonly");
            //txt_from.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //Txtto.Attributes.Add("readonly", "readonly");
            //Txtto.Text = DateTime.Now.ToString("dd/MM/yyyy");
            if (!IsPostBack)
            {
                Bindcollege();
                lan1();
                type();
                loaddata();
                txtfromdate.Attributes.Add("readonly", "readonly");
                txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txttodate.Attributes.Add("readonly", "readonly");
                txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_from.Text = "";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "library Entry Correction");
        }
    }

    public void type()
    {
        try
        {
            //ddltype.Items.Add("");
            ddltype.Items.Add("Department");
            ddltype.Items.Add("Title");
            ddltype.Items.Add("Author");
            ddltype.Items.Add("Subject");
            ddltype.Items.Add("Attachments");
            ddltype.Items.Add("Publisher");
            ddltype.Items.Add("Supplier");
            ddltype.Items.Add("ISBN");
            ddltype.Items.Add("Book Type");
            ddltype.Items.Add("Invoice No");
            ddltype.Items.Add("Call No");
            ddltype.Items.Add("Edition");
            ddltype.Items.Add("Year");
            ddltype.Items.Add("Book Status");
            ddltype.Items.Add("Book Price");
            ddltype.Items.Add("Book Pages");
            ddltype.Items.Add("Volume");
            ddltype.Items.Add("Collation");
            ddltype.Items.Add("Discount");
            ddltype.Items.Add("Language");
            ddltype.Items.Add("Reference Book");
            ddltype.Items.Add("Remarks");
            ddltype.Items.Add("Date of Accession");
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "library Entry Correction");
        }
    }

    public void Bindcollege()
    {
        try
        {
            ddlCollege.Items.Clear();
            dtCommon.Clear();
            ddlCollege.Enabled = false;
            DataSet dsprint = new DataSet();
            dsprint.Clear();
            string qryUserCodeOrGroupCode = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["group_code"] != null && Session["single_user"] != null && Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true")
            {
                qryUserCodeOrGroupCode = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["usercode"]).Trim()))
            {
                qryUserCodeOrGroupCode = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (!string.IsNullOrEmpty(qryUserCodeOrGroupCode))
            {
                dicQueryParameter.Clear();
                dicQueryParameter.Add("column_field", Convert.ToString(qryUserCodeOrGroupCode));
                dtCommon = storeAcc.selectDataTable("bind_college", dicQueryParameter);
            }
            if (dtCommon.Rows.Count > 0)
            {
                ddlCollege.DataSource = dtCommon;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.SelectedIndex = 0;
                ddlCollege.Enabled = true;




            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "library Entry Correction");
        }

    }

    public void loaddata()
    {
        try
        {
            string Sql = string.Empty;
            DataSet bookallo = new DataSet();
            Lblselectentry.Visible = true;
            ddlentry.Visible = true;
            cblCollege.Items.Clear();
            if (Convert.ToString(ddltype.SelectedItem) == "Title")
            {

                Sql = "select distinct title from bookdetails where 1 = 1 ";
                if (txt_from.Text != "")
                    Sql = Sql + " AND title LIKE '%" + txt_from.Text + "%'";
                if (Convert.ToString(ddllang.SelectedItem) == "English")
                    Sql = Sql + " AND ISNULL(TitleLanguage,0) = 0";
                if (Convert.ToString(ddllang.SelectedItem) == "Tamil")
                    Sql = Sql + " AND ISNULL(TitleLanguage,0) = 1";
                Sql = Sql + " order by title";
                bookallo.Clear();
                bookallo = d2.select_method_wo_parameter(Sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    cblCollege.DataSource = bookallo;
                    cblCollege.DataTextField = "title";
                    cblCollege.DataValueField = "title";
                    cblCollege.DataBind();
                    ddlentry.DataSource = bookallo;
                    ddlentry.DataTextField = "title";
                    ddlentry.DataValueField = "title";
                    ddlentry.DataBind();

                    //if (cblCollege.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cblCollege.Items.Count; row++)
                    //    {
                    //        //cblCollege.Items[row].Selected = true;
                    //    }
                    //    //txtCollege.Text = "title (" + cblCollege.Items.Count + ")";
                    //    //chkCollege.Checked = true;
                    //}


                }
            }
            if (Convert.ToString(ddltype.SelectedItem) == "Author")
            {
                Sql = "select distinct author from bookdetails  where 1 = 1 ";
                if (txt_from.Text != "")
                    Sql = Sql + " AND Author LIKE '%" + txt_from.Text + "%'";
                if (Convert.ToString(ddllang.SelectedItem) == "English")
                    Sql = Sql + " AND ISNULL(AuthorLanguage,0) = 0";
                if (Convert.ToString(ddllang.SelectedItem) == "Tamil")
                    Sql = Sql + " AND ISNULL(AuthorLanguage,0) = 1";
                Sql = Sql + " order by author";
                bookallo.Clear();
                bookallo = d2.select_method_wo_parameter(Sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    cblCollege.DataSource = bookallo;
                    cblCollege.DataTextField = "author";
                    cblCollege.DataValueField = "author";
                    cblCollege.DataBind();
                    ddlentry.DataSource = bookallo;
                    ddlentry.DataTextField = "author";
                    ddlentry.DataValueField = "author";
                    ddlentry.DataBind();


                    //if (cblCollege.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cblCollege.Items.Count; row++)
                    //    {
                    //        //cblCollege.Items[row].Selected = true;
                    //    }
                    //    //txtCollege.Text = "author (" + cblCollege.Items.Count + ")";
                    //    //chkCollege.Checked = true;
                    //}


                }
            }
            if (Convert.ToString(ddltype.SelectedItem) == "Subject")
            {
                Sql = "select distinct subject from bookdetails  where 1 = 1 ";
                if (txt_from.Text != "")
                    Sql = Sql + " AND subject LIKE '%" + txt_from.Text + "%'";

                Sql = Sql + " order by subject ";
                bookallo.Clear();
                bookallo = d2.select_method_wo_parameter(Sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    cblCollege.DataSource = bookallo;
                    cblCollege.DataTextField = "subject";
                    cblCollege.DataValueField = "subject";
                    cblCollege.DataBind();
                    ddlentry.DataSource = bookallo;
                    ddlentry.DataTextField = "subject";
                    ddlentry.DataValueField = "subject";
                    ddlentry.DataBind();

                    //if (cblCollege.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cblCollege.Items.Count; row++)
                    //    {
                    //        //cblCollege.Items[row].Selected = true;
                    //    }
                    //    //txtCollege.Text = "subject (" + cblCollege.Items.Count + ")";
                    //    //chkCollege.Checked = true;
                    //}


                }
            }
            if (Convert.ToString(ddltype.SelectedItem) == "Attachments")
            {
                Sql = "select distinct attachment_name from attachment  where 1 = 1 ";
                if (txt_from.Text != "")
                    Sql = Sql + " AND attachment_name LIKE '%" + txt_from.Text + "%'";

                Sql = Sql + " order by attachment_name ";
                bookallo.Clear();
                bookallo = d2.select_method_wo_parameter(Sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    cblCollege.DataSource = bookallo;
                    cblCollege.DataTextField = "attachment_name";
                    cblCollege.DataValueField = "attachment_name";
                    cblCollege.DataBind();
                    ddlentry.DataSource = bookallo;
                    ddlentry.DataTextField = "attachment_name";
                    ddlentry.DataValueField = "attachment_name";
                    ddlentry.DataBind();

                    //if (cblCollege.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cblCollege.Items.Count; row++)
                    //    {
                    //        //cblCollege.Items[row].Selected = true;
                    //    }
                    //    //txtCollege.Text = "attachment_name (" + cblCollege.Items.Count + ")";
                    //    //chkCollege.Checked = true;
                    //}


                }
            }
            if (Convert.ToString(ddltype.SelectedItem) == "Publisher")
            {
                Sql = "select distinct publisher from bookdetails  where 1 = 1 ";
                if (txt_from.Text != "")
                    Sql = Sql + " AND publisher LIKE '%" + txt_from.Text + "%'";

                Sql = Sql + " order by attachment_name ";
                bookallo.Clear();
                bookallo = d2.select_method_wo_parameter(Sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    cblCollege.DataSource = bookallo;
                    cblCollege.DataTextField = "publisher";
                    cblCollege.DataValueField = "publisher";
                    cblCollege.DataBind();
                    ddlentry.DataSource = bookallo;
                    ddlentry.DataTextField = "publisher";
                    ddlentry.DataValueField = "publisher";
                    ddlentry.DataBind();

                    //if (cblCollege.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cblCollege.Items.Count; row++)
                    //    {
                    //        //cblCollege.Items[row].Selected = true;
                    //    }
                    //    //txtCollege.Text = "publisher (" + cblCollege.Items.Count + ")";
                    //    //chkCollege.Checked = true;
                    //}


                }
            }
            if (Convert.ToString(ddltype.SelectedItem) == "Supplier")
            {
                Sql = "select distinct Supplier from bookdetails  where 1 = 1 ";
                if (txt_from.Text != "")
                    Sql = Sql + " AND supplier LIKE '%" + txt_from.Text + "%'";

                Sql = Sql + " order by supplier  ";
                bookallo.Clear();
                bookallo = d2.select_method_wo_parameter(Sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    cblCollege.DataSource = bookallo;
                    cblCollege.DataTextField = "Supplier";
                    cblCollege.DataValueField = "Supplier";
                    cblCollege.DataBind();
                    ddlentry.DataSource = bookallo;
                    ddlentry.DataTextField = "Supplier";
                    ddlentry.DataValueField = "Supplier";
                    ddlentry.DataBind();

                    //if (cblCollege.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cblCollege.Items.Count; row++)
                    //    {
                    //        //cblCollege.Items[row].Selected = true;
                    //    }
                    //    //txtCollege.Text = "Supplier (" + cblCollege.Items.Count + ")";
                    //    //chkCollege.Checked = true;
                    //}


                }
            }
            if (Convert.ToString(ddltype.SelectedItem) == "Department")
            {
                Sql = "select distinct dept_code from bookdetails b,journal_dept j where b.lib_code = j.lib_code and j.college_code ='" + Convert.ToString(ddlCollege.SelectedValue) + "'";
                if (txt_from.Text != "")
                    Sql = Sql + " AND dept_code LIKE '%" + txt_from.Text + "%'";

                Sql = Sql + " order by dept_code ";
                bookallo.Clear();
                bookallo = d2.select_method_wo_parameter(Sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    cblCollege.DataSource = bookallo;
                    cblCollege.DataTextField = "dept_code";
                    cblCollege.DataValueField = "dept_code";
                    cblCollege.DataBind();
                    ddlentry.DataSource = bookallo;
                    ddlentry.DataTextField = "dept_code";
                    ddlentry.DataValueField = "dept_code";
                    ddlentry.DataBind();

                    //if (cblCollege.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cblCollege.Items.Count; row++)
                    //    {
                    //        //cblCollege.Items[row].Selected = true;
                    //    }
                    //    //txtCollege.Text = "dept_code (" + cblCollege.Items.Count + ")";
                    //    //chkCollege.Checked = true;
                    //}


                }
            }
            if (Convert.ToString(ddltype.SelectedItem) == "ISBN")
            {
                Sql = "select distinct ISBN from bookdetails  where 1 = 1 ";
                if (txt_from.Text != "")
                    Sql = Sql + " AND ISBN LIKE '%" + txt_from.Text + "%'";

                Sql = Sql + " order by ISBN ";
                bookallo.Clear();
                bookallo = d2.select_method_wo_parameter(Sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    cblCollege.DataSource = bookallo;
                    cblCollege.DataTextField = "ISBN";
                    cblCollege.DataValueField = "ISBN";
                    cblCollege.DataBind();
                    ddlentry.DataSource = bookallo;
                    ddlentry.DataTextField = "ISBN";
                    ddlentry.DataValueField = "ISBN";
                    ddlentry.DataBind();


                    //if (cblCollege.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cblCollege.Items.Count; row++)
                    //    {
                    //        //cblCollege.Items[row].Selected = true;
                    //    }
                    //    //txtCollege.Text = "ISBN (" + cblCollege.Items.Count + ")";
                    //    //chkCollege.Checked = true;
                    //}


                }
            }
            if (Convert.ToString(ddltype.SelectedItem) == "Book Type")
            {
                Sql = "select distinct TypeofBook from bookdetails  where 1 = 1 ";
                if (txt_from.Text != "")
                    Sql = Sql + " AND TypeofBook LIKE '%" + txt_from.Text + "%'";

                Sql = Sql + " order by TypeofBook ";
                bookallo.Clear();
                bookallo = d2.select_method_wo_parameter(Sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    cblCollege.DataSource = bookallo;
                    cblCollege.DataTextField = "TypeofBook";
                    cblCollege.DataValueField = "TypeofBook";
                    cblCollege.DataBind();
                    ddlentry.DataSource = bookallo;
                    ddlentry.DataTextField = "TypeofBook";
                    ddlentry.DataValueField = "TypeofBook";
                    ddlentry.DataBind();
                    //if (cblCollege.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cblCollege.Items.Count; row++)
                    //    {
                    //        //cblCollege.Items[row].Selected = true;
                    //    }
                    //    //txtCollege.Text = "TypeofBook (" + cblCollege.Items.Count + ")";
                    //    //chkCollege.Checked = true;
                    //}


                }
            }
            if (Convert.ToString(ddltype.SelectedItem) == "Book Status")
            {
                Sql = "select distinct Book_Status from bookdetails  where 1 = 1 ";
                if (txt_from.Text != "")
                    Sql = Sql + " AND Book_Status LIKE '%" + txt_from.Text + "%'";

                Sql = Sql + " order by Book_Status ";
                bookallo.Clear();
                bookallo = d2.select_method_wo_parameter(Sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    cblCollege.DataSource = bookallo;
                    cblCollege.DataTextField = "Book_Status";
                    cblCollege.DataValueField = "Book_Status";
                    cblCollege.DataBind();
                    ddlentry.DataSource = bookallo;
                    ddlentry.DataTextField = "Book_Status";
                    ddlentry.DataValueField = "Book_Status";
                    ddlentry.DataBind();

                    //if (cblCollege.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cblCollege.Items.Count; row++)
                    //    {
                    //        //cblCollege.Items[row].Selected = true;
                    //    }
                    //    //txtCollege.Text = "Book_Status (" + cblCollege.Items.Count + ")";
                    //    //chkCollege.Checked = true;
                    //}


                }
            }
            if (Convert.ToString(ddltype.SelectedItem) == "Book Pages")
            {
                Sql = "select distinct Book_Size from bookdetails  where 1 = 1 ";
                if (txt_from.Text != "")
                    Sql = Sql + " AND Book_Size LIKE '%" + txt_from.Text + "%'";

                Sql = Sql + " order by Book_Size ";
                bookallo.Clear();
                bookallo = d2.select_method_wo_parameter(Sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    cblCollege.DataSource = bookallo;
                    cblCollege.DataTextField = "Book_Size";
                    cblCollege.DataValueField = "Book_Size";
                    cblCollege.DataBind();
                    ddlentry.DataSource = bookallo;
                    ddlentry.DataTextField = "Book_Size";
                    ddlentry.DataValueField = "Book_Size";
                    ddlentry.DataBind();
                    //if (cblCollege.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cblCollege.Items.Count; row++)
                    //    {
                    //        //cblCollege.Items[row].Selected = true;
                    //    }
                    //    //txtCollege.Text = "Book_Size (" + cblCollege.Items.Count + ")";
                    //    //chkCollege.Checked = true;
                    //}


                }
            }
            if (Convert.ToString(ddltype.SelectedItem) == "Volume")
            {
                Sql = "select distinct Volume from bookdetails  where 1 = 1 ";
                if (txt_from.Text != "")
                    Sql = Sql + " AND Volume LIKE '%" + txt_from.Text + "%'";

                Sql = Sql + " order by Volume ";
                bookallo.Clear();
                bookallo = d2.select_method_wo_parameter(Sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    cblCollege.DataSource = bookallo;
                    cblCollege.DataTextField = "Volume";
                    cblCollege.DataValueField = "Volume";
                    cblCollege.DataBind();
                    ddlentry.DataSource = bookallo;
                    ddlentry.DataTextField = "Volume";
                    ddlentry.DataValueField = "Volume";
                    ddlentry.DataBind();

                    //if (cblCollege.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cblCollege.Items.Count; row++)
                    //    {
                    //       // cblCollege.Items[row].Selected = true;
                    //    }
                    //    //txtCollege.Text = "Volume (" + cblCollege.Items.Count + ")";
                    //    //chkCollege.Checked = true;
                    //}


                }
            }

            if (Convert.ToString(ddltype.SelectedItem) == "Collation")
            {
                Sql = "select distinct Collabrator from bookdetails  where 1 = 1 ";
                if (txt_from.Text != "")
                    Sql = Sql + " AND Collabrator LIKE '%" + txt_from.Text + "%'";

                Sql = Sql + " order by Collabrator ";
                bookallo.Clear();
                bookallo = d2.select_method_wo_parameter(Sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    cblCollege.DataSource = bookallo;
                    cblCollege.DataTextField = "Collabrator";
                    cblCollege.DataValueField = "Collabrator";
                    cblCollege.DataBind();
                    ddlentry.DataSource = bookallo;
                    ddlentry.DataTextField = "Collabrator";
                    ddlentry.DataValueField = "Collabrator";
                    ddlentry.DataBind();

                    //if (cblCollege.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cblCollege.Items.Count; row++)
                    //    {
                    //        //cblCollege.Items[row].Selected = true;
                    //    }
                    //    //txtCollege.Text = "Collabrator (" + cblCollege.Items.Count + ")";
                    //    //chkCollege.Checked = true;
                    //}


                }
            }

            if (Convert.ToString(ddltype.SelectedItem) == "Language")
            {
                Sql = "select distinct Language from bookdetails  where 1 = 1 ";
                if (txt_from.Text != "")
                    Sql = Sql + " AND Language LIKE '%" + txt_from.Text + "%'";

                Sql = Sql + " order by Language ";
                bookallo.Clear();
                bookallo = d2.select_method_wo_parameter(Sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    cblCollege.DataSource = bookallo;
                    cblCollege.DataTextField = "Language";
                    cblCollege.DataValueField = "Language";
                    cblCollege.DataBind();
                    ddlentry.DataSource = bookallo;
                    ddlentry.DataTextField = "Language";
                    ddlentry.DataValueField = "Language";
                    ddlentry.DataBind();
                    //if (cblCollege.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cblCollege.Items.Count; row++)
                    //    {
                    //        //cblCollege.Items[row].Selected = true;
                    //    }
                    //    //txtCollege.Text = "Language (" + cblCollege.Items.Count + ")";
                    //    //chkCollege.Checked = true;
                    //}


                }
            }
            if (Convert.ToString(ddltype.SelectedItem) == "Invoice No")
            {
                Sql = "select distinct Bill_No from bookdetails where 1 = 1 ";
                if (txt_from.Text != "")
                    Sql = Sql + " AND Bill_No LIKE '%" + txt_from.Text + "%'";

                Sql = Sql + " order by Bill_No";
                bookallo.Clear();
                bookallo = d2.select_method_wo_parameter(Sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    cblCollege.DataSource = bookallo;
                    cblCollege.DataTextField = "Bill_No";
                    cblCollege.DataValueField = "Bill_No";
                    cblCollege.DataBind();
                    ddlentry.DataSource = bookallo;
                    ddlentry.DataTextField = "Bill_No";
                    ddlentry.DataValueField = "Bill_No";
                    ddlentry.DataBind();
                    //if (cblCollege.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cblCollege.Items.Count; row++)
                    //    {
                    //        //cblCollege.Items[row].Selected = true;
                    //    }
                    //    //txtCollege.Text = "Bill_No (" + cblCollege.Items.Count + ")";
                    //    //chkCollege.Checked = true;
                    //}


                }
            }
            if (Convert.ToString(ddltype.SelectedItem) == "Call No")
            {
                Sql = "select distinct Call_No from bookdetails  where 1 = 1 ";
                if (txt_from.Text != "")
                    Sql = Sql + " AND Call_No LIKE '%" + txt_from.Text + "%'";

                Sql = Sql + " order by Call_No";
                bookallo.Clear();
                bookallo = d2.select_method_wo_parameter(Sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    cblCollege.DataSource = bookallo;
                    cblCollege.DataTextField = "Call_No";
                    cblCollege.DataValueField = "Call_No";
                    cblCollege.DataBind();
                    ddlentry.DataSource = bookallo;
                    ddlentry.DataTextField = "Call_No";
                    ddlentry.DataValueField = "Call_No";
                    ddlentry.DataBind();
                    //if (cblCollege.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cblCollege.Items.Count; row++)
                    //    {
                    //        //cblCollege.Items[row].Selected = true;
                    //    }
                    //    //txtCollege.Text = "Call_No (" + cblCollege.Items.Count + ")";
                    //    //chkCollege.Checked = true;
                    //}


                }
            }
            if (Convert.ToString(ddltype.SelectedItem) == "Edition")
            {
                Sql = "select distinct Edition from bookdetails   where 1 = 1 ";
                if (txt_from.Text != "")
                    Sql = Sql + " AND Edition LIKE '%" + txt_from.Text + "%'";

                Sql = Sql + " order by Edition ";
                bookallo.Clear();
                bookallo = d2.select_method_wo_parameter(Sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    cblCollege.DataSource = bookallo;
                    cblCollege.DataTextField = "Edition";
                    cblCollege.DataValueField = "Edition";
                    cblCollege.DataBind();
                    ddlentry.DataSource = bookallo;
                    ddlentry.DataTextField = "Edition";
                    ddlentry.DataValueField = "Edition";
                    ddlentry.DataBind();
                    //if (cblCollege.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cblCollege.Items.Count; row++)
                    //    {
                    //        //cblCollege.Items[row].Selected = true;
                    //    }
                    //    //txtCollege.Text = "Edition (" + cblCollege.Items.Count + ")";
                    //    //chkCollege.Checked = true;
                    //}


                }
            }
            if (Convert.ToString(ddltype.SelectedItem) == "Year")
            {
                Sql = "select distinct Pur_Year from bookdetails  where 1 = 1 ";
                if (txt_from.Text != "")
                    Sql = Sql + " AND Pur_Year LIKE '%" + txt_from.Text + "%'";

                Sql = Sql + " order by Pur_Year ";
                bookallo.Clear();
                bookallo = d2.select_method_wo_parameter(Sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    cblCollege.DataSource = bookallo;
                    cblCollege.DataTextField = "Pur_Year";
                    cblCollege.DataValueField = "Pur_Year";
                    cblCollege.DataBind();
                    ddlentry.DataSource = bookallo;
                    ddlentry.DataTextField = "Pur_Year";
                    ddlentry.DataValueField = "Pur_Year";
                    ddlentry.DataBind();
                    //if (cblCollege.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cblCollege.Items.Count; row++)
                    //    {
                    //       // cblCollege.Items[row].Selected = true;
                    //    }
                    //    //txtCollege.Text = "Year (" + cblCollege.Items.Count + ")";
                    //    //chkCollege.Checked = true;
                    //}


                }
            }
            if (Convert.ToString(ddltype.SelectedItem) == "Book Price")
            {
                Sql = "select distinct Price from bookdetails  where 1 = 1 ";
                if (txt_from.Text != "")
                    Sql = Sql + " AND Price LIKE '%" + txt_from.Text + "%'";

                Sql = Sql + " order by Price ";
                bookallo.Clear();
                bookallo = d2.select_method_wo_parameter(Sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    cblCollege.DataSource = bookallo;
                    cblCollege.DataTextField = "Price";
                    cblCollege.DataValueField = "Price";
                    cblCollege.DataBind();
                    ddlentry.DataSource = bookallo;
                    ddlentry.DataTextField = "Price";
                    ddlentry.DataValueField = "Price";
                    ddlentry.DataBind();
                    //if (cblCollege.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cblCollege.Items.Count; row++)
                    //    {
                    //        //cblCollege.Items[row].Selected = true;
                    //    }
                    //    //txtCollege.Text = "Book Price (" + cblCollege.Items.Count + ")";
                    //    //chkCollege.Checked = true;
                    //}


                }
            }
            if (Convert.ToString(ddltype.SelectedItem) == "Discount")
            {
                Sql = "select distinct B_Discount from bookdetails  where 1 = 1 ";
                if (txt_from.Text != "")
                    Sql = Sql + " AND B_Discount LIKE '%" + txt_from.Text + "%'";

                Sql = Sql + " order by B_Discount ";
                bookallo.Clear();
                bookallo = d2.select_method_wo_parameter(Sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    cblCollege.DataSource = bookallo;
                    cblCollege.DataTextField = "B_Discount";
                    cblCollege.DataValueField = "B_Discount";
                    cblCollege.DataBind();
                    ddlentry.DataSource = bookallo;
                    ddlentry.DataTextField = "B_Discount";
                    ddlentry.DataValueField = "B_Discount";
                    ddlentry.DataBind();
                    //if (cblCollege.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cblCollege.Items.Count; row++)
                    //    {
                    //        //cblCollege.Items[row].Selected = true;
                    //    }
                    //    //txtCollege.Text = "Discount (" + cblCollege.Items.Count + ")";
                    //    //chkCollege.Checked = true;
                    //}


                }
            }
            if (Convert.ToString(ddltype.SelectedItem) == "Remarks")
            {
                Sql = "select distinct Remark from bookdetails  where 1 = 1 ";
                if (txt_from.Text != "")
                    Sql = Sql + " AND Remark LIKE '%" + txt_from.Text + "%'";

                Sql = Sql + " order by Remark ";
                bookallo.Clear();
                bookallo = d2.select_method_wo_parameter(Sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    cblCollege.DataSource = bookallo;
                    cblCollege.DataTextField = "Remark";
                    cblCollege.DataValueField = "Remark";
                    cblCollege.DataBind();
                    ddlentry.DataSource = bookallo;
                    ddlentry.DataTextField = "Remark";
                    ddlentry.DataValueField = "Remark";
                    ddlentry.DataBind();
                    //if (cblCollege.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cblCollege.Items.Count; row++)
                    //    {
                    //        //cblCollege.Items[row].Selected = true;
                    //    }
                    //    //txtCollege.Text = "Remarks (" + cblCollege.Items.Count + ")";
                    //    //chkCollege.Checked = true;
                    //}


                }
            }
            if (Convert.ToString(ddltype.SelectedItem) == "Date of Accession")
            {
                Sql = "select distinct CONVERT(varchar(20),date_accession,103) date_accession  from bookdetails  where 1 = 1 ";
                if (chkredate.Checked == true)
                {
                    string from = Convert.ToString(txtfromdate.Text);
                    string[] frdate = from.Split('/');
                    string fdate = frdate[1] + '/' + frdate[0] + '/' + frdate[2];
                    string to = Convert.ToString(txttodate.Text);
                    string[] todate = from.Split('/');
                    string tdate = todate[1] + '/' + todate[0] + '/' + todate[2];
                    Sql = Sql + " AND date_accession between '" + fdate + "' AND '" + tdate + "' ";
                }
                Sql = Sql + " order by date_accession ";
                bookallo.Clear();
                bookallo = d2.select_method_wo_parameter(Sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    cblCollege.DataSource = bookallo;
                    cblCollege.DataTextField = "date_accession";
                    cblCollege.DataValueField = "date_accession";
                    cblCollege.DataBind();
                    ddlentry.DataSource = bookallo;
                    ddlentry.DataTextField = "date_accession";
                    ddlentry.DataValueField = "date_accession";
                    ddlentry.DataBind();
                    //if (cblCollege.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cblCollege.Items.Count; row++)
                    //    {
                    //        //cblCollege.Items[row].Selected = true;
                    //    }
                    //    //txtCollege.Text = "Date of Accession (" + cblCollege.Items.Count + ")";
                    //    //chkCollege.Checked = true;
                    //}


                }
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, userCollegeCode, "library Entry Correction");
        }
    }

    public void lan()
    {
        try
        {
            ddllng.Items.Add("English");
            ddllng.Items.Add("Tamil");
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "library Entry Correction");
        }
    }

    public void lan1()
    {
        try
        {

            ddllang.Items.Add("English");
            ddllang.Items.Add("Tamil");
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "library Entry Correction");
        }
    }

    protected void delete_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = string.Empty;
            DataSet bookallo = new DataSet();
            string typ = string.Empty;
            if (cblCollege.Items.Count > 0)
            {
                for (int i = 0; i < cblCollege.Items.Count; i++)
                {
                    if (cblCollege.Items[i].Selected == true)
                    {
                        if (typ == "")
                        {
                            typ = "" + cblCollege.Items[i].Text.ToString() + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + cblCollege.Items[i].Text.ToString() + "";
                        }
                    }
                }
            }
            if (Convert.ToString(ddltype.SelectedItem) == "Title")
            {
                sql = "delete from bookdetails where title in '" + typ + "'";
            }
            if (Convert.ToString(ddltype.SelectedItem) == "Department")
            {
                sql = "select * from bookdetails where dept_code in '" + typ + "'";
                bookallo = d2.select_method_wo_parameter(sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    sql = "delete from journal_dept where dept_name in '" + typ + "' where college_code ='" + Convert.ToString(ddlCollege.SelectedValue) + "'";
                    int ins1 = d2.update_method_wo_parameter(sql, "Text");
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Some books are there in this department. So  it can't be deleted.";
                }
            }

        }
        catch
        {
        }
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    protected void Chkaccno_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (Chkaccno.Checked == true)
            {
                TextBox1.Enabled = true;
                TextBox2.Enabled = true;
            }
            else
            {
                TextBox1.Enabled = false;
                TextBox2.Enabled = false;
            }
        }
        catch
        {
        }
    }

    protected void rdbprice_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (rdbprice.Checked == true)
                txtprice.Visible = true;
            else
                txtprice.Visible = false;
        }
        catch
        {
        }
    }

    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_from.Text = "";
            Lblselectentry.Visible = false;
            ddlentry.Visible = false;
            cblCollege.Items.Clear();
            if (ddltype.SelectedIndex == 0)
            {
                txt_from.Visible = true;
                datefield.Visible = false;
                Chklan.Visible = false;
                ddllng.Visible = false;
                Label_title.Visible = false;
                txtNew.Visible = false;
                rdbdept.Text = "Replace Department";
                Btndelete.Enabled = true;
            }
            else if (ddltype.SelectedIndex == 1)
            {
                txt_from.Visible = true;
                datefield.Visible = false;
                Chklan.Visible = true;
                ddllng.Visible = true;
                Label_title.Visible = true;
                Label_title.Text = "Title";
                txtNew.Visible = true;
                rdbdept.Text = "Replace Title";
                Btndelete.Enabled = true;
            }
            else if (ddltype.SelectedIndex == 2)
            {
                txt_from.Visible = true;
                datefield.Visible = false;
                Chklan.Visible = true;
                ddllng.Visible = true;
                Label_title.Visible = true;
                Label_title.Text = "Author";
                txtNew.Visible = true;
                rdbdept.Text = "Replace Author";
                Btndelete.Enabled = false;
            }
            else if (ddltype.SelectedIndex == 3)
            {
                txt_from.Visible = true;
                datefield.Visible = false;
                Chklan.Visible = false;
                ddllng.Visible = false;
                Label_title.Visible = false;
                txtNew.Visible = false;
                rdbdept.Text = "Replace Subject";
                Btndelete.Enabled = false;
            }
            else if (ddltype.SelectedIndex == 4)
            {
                txt_from.Visible = true;
                datefield.Visible = false;
                Chklan.Visible = false;
                ddllng.Visible = false;
                Label_title.Visible = false;
                txtNew.Visible = false;
                rdbdept.Text = "Replace Attachement";
                Btndelete.Enabled = false;
            }
            else if (ddltype.SelectedIndex == 5)
            {
                txt_from.Visible = true;
                datefield.Visible = false;
                Chklan.Visible = false;
                ddllng.Visible = false;
                Label_title.Visible = false;
                txtNew.Visible = false;
                rdbdept.Text = "Replace Publisher";
                Btndelete.Enabled = false;
            }
            else if (ddltype.SelectedIndex == 6)
            {
                txt_from.Visible = true;
                datefield.Visible = false;
                Chklan.Visible = false;
                ddllng.Visible = false;
                Label_title.Visible = false;
                txtNew.Visible = false;
                rdbdept.Text = "Replace Supplier";
                Btndelete.Enabled = false;
            }
            else if (ddltype.SelectedIndex == 7)
            {
                txt_from.Visible = true;
                datefield.Visible = false;
                Chklan.Visible = false;
                ddllng.Visible = false;
                Label_title.Visible = true;
                Label_title.Text = "ISBN";
                txtNew.Visible = true;
                rdbdept.Text = "Replace ISBN";
                Btndelete.Enabled = false;
            }

            else if (ddltype.SelectedIndex == 8)
            {
                txt_from.Visible = true;
                datefield.Visible = false;
                Chklan.Visible = false;
                ddllng.Visible = false;
                Label_title.Visible = false;
                txtNew.Visible = false;
                rdbdept.Text = "Replace BookType";
                Btndelete.Enabled = false;
            }
            else if (ddltype.SelectedIndex == 9)
            {
                txt_from.Visible = true;
                datefield.Visible = false;
                Chklan.Visible = false;
                ddllng.Visible = false;
                Label_title.Visible = true;
                Label_title.Text = "InvoiceNo";
                txtNew.Visible = true;
                rdbdept.Text = "Replace InvoiceNo";
                Btndelete.Enabled = true;
            }
            else if (ddltype.SelectedIndex == 10)
            {
                txt_from.Visible = true;
                datefield.Visible = false;
                Chklan.Visible = false;
                ddllng.Visible = false;
                Label_title.Visible = true;
                Label_title.Text = "CallNo";
                txtNew.Visible = true;
                rdbdept.Text = "Replace CallNo";
                Btndelete.Enabled = true;
            }
            else if (ddltype.SelectedIndex == 11)
            {
                txt_from.Visible = true;
                datefield.Visible = false;
                Chklan.Visible = false;
                ddllng.Visible = false;
                Label_title.Visible = true;
                Label_title.Text = "Edition";
                txtNew.Visible = true;
                rdbdept.Text = "Replace Edition";
                Btndelete.Enabled = true;
            }
            else if (ddltype.SelectedIndex == 12)
            {
                txt_from.Visible = true;
                datefield.Visible = false;
                Chklan.Visible = false;
                ddllng.Visible = false;
                Label_title.Visible = true;
                Label_title.Text = "Year";
                txtNew.Visible = true;
                rdbdept.Text = "Replace Year";
                Btndelete.Enabled = true;
            }
            else if (ddltype.SelectedIndex == 13)
            {
                txt_from.Visible = true;
                datefield.Visible = false;
                Chklan.Visible = false;
                ddllng.Visible = false;
                Label_title.Visible = false;
                txtNew.Visible = false;
                rdbdept.Text = "Replace BookStatus";
                Btndelete.Enabled = false;
            }
            else if (ddltype.SelectedIndex == 14)
            {
                txt_from.Visible = true;
                datefield.Visible = false;
                Chklan.Visible = false;
                ddllng.Visible = false;
                Label_title.Visible = true;
                Label_title.Text = "BookPrice";
                txtNew.Visible = true;
                rdbdept.Text = "Replace BookPrice";
                Btndelete.Enabled = true;
            }
            else if (ddltype.SelectedIndex == 15)
            {
                txt_from.Visible = true;
                datefield.Visible = false;
                Chklan.Visible = false;
                ddllng.Visible = false;
                Label_title.Visible = true;
                Label_title.Text = "BookPages";
                txtNew.Visible = true;
                rdbdept.Text = "Replace BookPages";
                Btndelete.Enabled = false;
            }
            else if (ddltype.SelectedIndex == 16)
            {
                txt_from.Visible = true;
                datefield.Visible = false;
                Chklan.Visible = false;
                ddllng.Visible = false;
                Label_title.Visible = true;
                Label_title.Text = "Volume";
                txtNew.Visible = true;
                rdbdept.Text = "Replace Volume";
                Btndelete.Enabled = false;
            }
            else if (ddltype.SelectedIndex == 17)
            {
                txt_from.Visible = true;
                datefield.Visible = false;
                Chklan.Visible = false;
                ddllng.Visible = false;
                Label_title.Visible = true;
                Label_title.Text = "Collation";
                txtNew.Visible = true;
                rdbdept.Text = "Replace Collation";
                Btndelete.Enabled = false;
            }
            else if (ddltype.SelectedIndex == 18)
            {
                txt_from.Visible = true;
                datefield.Visible = false;
                Chklan.Visible = false;
                ddllng.Visible = false;
                Label_title.Visible = true;
                Label_title.Text = "Discount";
                txtNew.Visible = true;
                rdbdept.Text = "Replace Discount";
                Btndelete.Enabled = true;
            }
            else if (ddltype.SelectedIndex == 19)
            {
                txt_from.Visible = true;
                datefield.Visible = false;
                Chklan.Visible = false;
                ddllng.Visible = false;
                Label_title.Visible = true;
                Label_title.Text = "Language";
                txtNew.Visible = true;
                rdbdept.Text = "Replace Language";
                Btndelete.Enabled = false;
            }
            else if (ddltype.SelectedIndex == 20)
            {
                txt_from.Visible = true;
                datefield.Visible = false;
                Chklan.Visible = false;
                ddllng.Visible = false;
                Label_title.Visible = false;
                txtNew.Visible = false;
                rdbdept.Text = "Replace ReferenceBook";
                Btndelete.Enabled = false;
            }
            else if (ddltype.SelectedIndex == 21)
            {
                txt_from.Visible = true;
                datefield.Visible = false;
                Chklan.Visible = false;
                ddllng.Visible = false;
                Label_title.Visible = true;
                Label_title.Text = "Remarks";
                txtNew.Visible = true;
                rdbdept.Text = "Replace Remarks";
                Btndelete.Enabled = true;
            }
            else
            {
                txt_from.Visible = false;
                Chklan.Visible = false;
                ddllng.Visible = false;
                Label_title.Visible = false;
                txtNew.Visible = false;
                datefield.Visible = true;
                rdbdept.Text = "Replace DateOfAcc";
                Btndelete.Enabled = false;
            }
            //if (Convert.ToString(ddltype.SelectedItem) == "Title")
            //    ddllang.Enabled = true;
            //else if (Convert.ToString(ddltype.SelectedItem) == "Author")
            //    ddllang.Enabled = true;
            //else
            //    ddllang.Enabled = false;
            //loaddata();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "library Entry Correction");
        }

    }

    protected void ddllang_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_from.Text = "";
            if (Convert.ToString(ddllang.SelectedItem) == "Tamil")
                txt_from.Font.Name = "Amudham";

            else
                txt_from.Font.Name = "Arial";



        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "library Entry Correction");
        }
    }

    protected void ddllng_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtNew.Text = "";
            if (Convert.ToString(ddllng.SelectedItem) == "Tamil")
                txtNew.Font.Name = "Amudham";

            else
                txtNew.Font.Name = "Arial";



        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "library Entry Correction");
        }
    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            type();
            loaddata();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "library Entry Correction");
        }
    }

    protected void chkredate_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkredate.Checked == true)
            {
                txtfromdate.Enabled = true;
                txttodate.Enabled = true;
            }
            else
            {
                txtfromdate.Enabled = false;
                txttodate.Enabled = false;

            }

        }
        catch
        {

        }

    }
    
    protected void Go_Click(object sender, EventArgs e)
    {
        try
        {
            loaddata();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "library Entry Correction");
        }
    }

     protected void ddlentry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            string txt = Convert.ToString(ddltype.SelectedItem);
            if (ddlentry.Items.Count > 0)
                txtNew.Text = Convert.ToString(ddlentry.SelectedValue);
            //for (int i = 0; i < ddlentry.Items.Count; i++)
            //{
            //    if (cblCollege.Items[i].Selected == true)
            //    {
            //        txtNew.Text = cblCollege.Items[i].Text;
            //    }
            //}

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "library Entry Correction");
        }

    }

    protected void replace_Click(object sender, EventArgs e)
    {
        try
        {
            string Sql = string.Empty;
            if (rdbprice.Checked == true && txtprice.Text != "")
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Enter Price to replace";
                txtprice.Focus();
            }
            if (rdbdept.Checked == true && txtNew.Text != "")
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Fill the replacing " + Convert.ToString(ddltype.SelectedItem) + "";
                txtNew.Focus();
            }
            if (rdbdept.Checked == true)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Fill the replacing " + Convert.ToString(ddltype.SelectedItem) + "";
                txt_from.Focus();
            }

            if (Chkbytype.Checked == true && Chkaccno.Checked == true)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Select Type of Access No to replace";

            }
            if (Chkaccno.Checked == true)
            {
                if (TextBox2.Text != "" && TextBox1.Text != "")
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Enter From and To Access No.";
                }
            }
            string typ = string.Empty;
            if (cblCollege.Items.Count > 0)
            {
                for (int i = 0; i < cblCollege.Items.Count; i++)
                {
                    if (cblCollege.Items[i].Selected == true)
                    {
                        if (typ == "")
                        {
                            typ = "" + cblCollege.Items[i].Text.ToString() + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + cblCollege.Items[i].Text.ToString() + "";
                        }
                    }
                }
            }
            if (Convert.ToString(ddltype.SelectedItem) == "Title" && Chklan.Checked == true)
            {
                if (typ != "")
                {
                    Sql = "update bookdetails set TitleLanguage =" + Convert.ToString(ddllng.SelectedIndex) + "";
                    Sql = Sql + " where Title in('" + typ + "')";
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Title Language updated sucessfully";
                    }
                }

            }
            else if (Convert.ToString(ddltype.SelectedItem) == "Author" && Chklan.Checked == true)
            {
                if (typ != "")
                {
                    Sql = "update bookdetails set AuthorLanguage =" + Convert.ToString(ddllng.SelectedIndex) + "";
                    Sql = Sql + " where Author in('" + typ + "')";
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Author Language updated sucessfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }

            }
            if (Convert.ToString(ddltype.SelectedItem) == "Title")
            {
                if (rdbdept.Checked == true && txtNew.Text != "")
                {
                    Sql = "update bookdetails set title='" + txtNew.Text + "' WHERE 1 = 1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Title in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";

                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (rdbprice.Checked == true && txtprice.Text != "")
                {
                    Sql = "update bookdetails set price ='" + txtprice.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Title in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (Rdbref.Checked == true)
                {
                    string reff = "0";

                    if (rdbyes.Checked)
                        reff = "1";
                    else
                        reff = "0";
                    Sql = "update bookdetails set Ref='" + reff + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND title in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }

            }
            else if (Convert.ToString(ddltype.SelectedItem) == "Author")
            {
                if (rdbdept.Checked == true && txtNew.Text != "")
                {
                    Sql = "update bookdetails set author='" + txtNew.Text + "' WHERE 1 = 1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + "AND author in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";

                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please select Correct Data from the list";
                    }
                }
                if (rdbprice.Checked == true && txtprice.Text != "")
                {
                    Sql = "update bookdetails set price ='" + txtprice.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND author in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (Rdbref.Checked == true)
                {
                    string reff = "0";

                    if (rdbyes.Checked)
                        reff = "1";
                    else
                        reff = "0";
                    Sql = "update bookdetails set Ref='" + reff + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND author in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
            }
            else if (Convert.ToString(ddltype.SelectedItem) == "Subject")
            {
                if (rdbdept.Checked == true && txtNew.Text != "")
                {
                    Sql = "update bookdetails set subject='" + txtNew.Text + "' WHERE 1 = 1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + "AND subject in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";

                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please select Correct Data from the list";
                    }
                }
                if (rdbprice.Checked == true && txtprice.Text != "")
                {
                    Sql = "update bookdetails set price ='" + txtprice.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND subject in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (Rdbref.Checked == true)
                {
                    string reff = "0";

                    if (rdbyes.Checked)
                        reff = "1";
                    else
                        reff = "0";
                    Sql = "update bookdetails set Ref='" + reff + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND subject in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }

            }
            else if (Convert.ToString(ddltype.SelectedItem) == "Publisher")
            {
                if (rdbdept.Checked == true && txtNew.Text != "")
                {
                    if (Convert.ToString(ddlentry.SelectedItem) != "")
                    {
                        Sql = "update bookdetails set publisher='" + txtNew.Text + "' WHERE 1 = 1 ";
                        if (Chkbytype.Checked == true)
                        {
                            if (typ != "")
                            {
                                Sql = Sql + " AND publisher in('" + typ + "')";
                            }
                        }
                        if (Chkaccno.Checked == true)
                        {
                            Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                        }
                        int up = d2.update_method_wo_parameter(Sql, "Text");
                        int m = up;
                        if (m != 0)
                        {
                            txt_from.Text = "";
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Records Updated Successfully";

                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Please select Correct Data from the list";
                        }
                    }
                    else
                    {
                        Sql = "update bookdetails set publisher='" + txtNew.Text + "' WHERE 1 = 1 ";
                        if (Chkbytype.Checked == true)
                        {
                            if (typ != "")
                            {
                                Sql = Sql + " AND publisher in('" + typ + "')";
                            }
                        }
                        if (Chkaccno.Checked == true)
                        {
                            Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                        }
                        int up = d2.update_method_wo_parameter(Sql, "Text");
                        int m = up;
                        if (m != 0)
                        {
                            txt_from.Text = "";
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Records Updated Successfully";

                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Please select Correct Data from the list";
                        }
                    }
                }
                if (rdbprice.Checked == true && txtprice.Text != "")
                {
                    Sql = "update bookdetails set price ='" + txtprice.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND publisher in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (Rdbref.Checked == true)
                {
                    string reff = "0";

                    if (rdbyes.Checked)
                        reff = "1";
                    else
                        reff = "0";
                    Sql = "update bookdetails set Ref='" + reff + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND publisher in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }

            }
            else if (Convert.ToString(ddltype.SelectedItem) == "Supplier")
            {
                if (rdbdept.Checked == true && txtNew.Text != "")
                {
                    if (Convert.ToString(ddlentry.SelectedItem) != "")
                    {
                        Sql = "update bookdetails set Supplier='" + txtNew.Text + "' WHERE 1 = 1 ";
                        if (Chkbytype.Checked == true)
                        {
                            if (typ != "")
                            {
                                Sql = Sql + " AND Supplier in('" + typ + "')";
                            }
                        }
                        if (Chkaccno.Checked == true)
                        {
                            Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                        }
                        int up = d2.update_method_wo_parameter(Sql, "Text");
                        int m = up;
                        if (m != 0)
                        {
                            txt_from.Text = "";
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Records Updated Successfully";
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Please select Correct Data from the list";
                        }
                    }

                }
                if (rdbprice.Checked == true && txtprice.Text != "")
                {
                    Sql = "update bookdetails set price ='" + txtprice.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Supplier in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (Rdbref.Checked == true)
                {
                    string reff = "0";

                    if (rdbyes.Checked)
                        reff = "1";
                    else
                        reff = "0";
                    Sql = "update bookdetails set Ref='" + reff + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Supplier in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }

            }
            else if (Convert.ToString(ddltype.SelectedItem) == "Attachments")
            {
                if (rdbdept.Checked == true && txtNew.Text != "")
                {
                    if (Convert.ToString(ddlentry.SelectedItem) != "")
                    {
                        Sql = "update attachment set attachment_name='" + txtNew.Text + "' where attachment_name in('" + typ + "')";
                        Sql = "update bookdetails set attachment='" + txtNew.Text + "' WHERE 1 = 1 ";
                        if (Chkbytype.Checked == true)
                        {
                            if (typ != "")
                            {
                                Sql = Sql + " AND attachment in('" + typ + "')";
                            }
                        }
                        if (Chkaccno.Checked == true)
                        {
                            Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                        }
                        int up = d2.update_method_wo_parameter(Sql, "Text");
                        int m = up;
                        if (m != 0)
                        {
                            txt_from.Text = "";
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Records Updated Successfully";

                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Please select Correct Data from the list";
                        }
                    }

                }
                if (rdbprice.Checked == true && txtprice.Text != "")
                {
                    Sql = "update bookdetails set price ='" + txtprice.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND attachment in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (Rdbref.Checked == true)
                {
                    string reff = "0";

                    if (rdbyes.Checked)
                        reff = "1";
                    else
                        reff = "0";
                    Sql = "update bookdetails set Ref='" + reff + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND attachment in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }

            }
            else if (Convert.ToString(ddltype.SelectedItem) == "Department")
            {
                if (rdbdept.Checked == true && txt_from.Text != "")
                {
                    if (Convert.ToString(ddlentry.SelectedItem) != "")
                    {

                        Sql = "update bookdetails set dept_code='" + txt_from.Text + "' WHERE 1 = 1 ";
                        if (Chkbytype.Checked == true)
                        {
                            if (typ != "")
                            {
                                Sql = Sql + " AND dept_code in('" + typ + "')";
                            }
                        }
                        if (Chkaccno.Checked == true)
                        {
                            Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                        }
                        int up = d2.update_method_wo_parameter(Sql, "Text");
                        int m = up;
                        if (m != 0)
                        {
                            txt_from.Text = "";
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Records Updated Successfully";

                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Please select Correct Data from the list";
                        }
                    }

                }
                if (rdbprice.Checked == true && txtprice.Text != "")
                {
                    Sql = "update bookdetails set price ='" + txtprice.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND dept_code in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (Rdbref.Checked == true)
                {
                    string reff = "0";

                    if (rdbyes.Checked)
                        reff = "1";
                    else
                        reff = "0";
                    Sql = "update bookdetails set Ref='" + reff + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND dept_code in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }

            }
            else if (Convert.ToString(ddltype.SelectedItem) == "ISBN")
            {
                if (rdbdept.Checked == true && txtNew.Text != "")
                {


                    Sql = "update bookdetails set ISBN='" + txtNew.Text + "' WHERE 1 = 1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND ISBN in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";

                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please select Correct Data from the list";
                    }


                }
                if (rdbprice.Checked == true && txtprice.Text != "")
                {
                    Sql = "update bookdetails set price ='" + txtprice.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND ISBN in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (Rdbref.Checked == true)
                {
                    string reff = "0";

                    if (rdbyes.Checked)
                        reff = "1";
                    else
                        reff = "0";
                    Sql = "update bookdetails set Ref='" + reff + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND ISBN in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }

            }
            else if (Convert.ToString(ddltype.SelectedItem) == "Book Type")
            {
                if (rdbdept.Checked == true)
                {


                    Sql = "update bookdetails set TypeofBook='" + txt_from.Text + "' WHERE 1 = 1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND TypeofBook in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";

                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please select Correct Data from the list";
                    }


                }
                if (rdbprice.Checked == true && txtprice.Text != "")
                {
                    Sql = "update bookdetails set price ='" + txtprice.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND TypeofBook in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (Rdbref.Checked == true)
                {
                    string reff = "0";

                    if (rdbyes.Checked)
                        reff = "1";
                    else
                        reff = "0";
                    Sql = "update bookdetails set Ref='" + reff + "',TypeofBook='" + txtNew.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND TypeofBook in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }

            }

            else if (Convert.ToString(ddltype.SelectedItem) == "Book Status")
            {
                if (rdbdept.Checked == true && txtNew.Text != "")
                {


                    Sql = "update bookdetails set Book_Status='" + txtNew.Text + "' WHERE 1 = 1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Book_Status in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";

                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please select Correct Data from the list";
                    }


                }
                if (rdbprice.Checked == true && txtprice.Text != "")
                {
                    Sql = "update bookdetails set price ='" + txtprice.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Book_Status in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (Rdbref.Checked == true)
                {
                    string reff = "0";

                    if (rdbyes.Checked)
                        reff = "1";
                    else
                        reff = "0";
                    Sql = "update bookdetails set Ref='" + reff + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Book_Status in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }

            }
            else if (Convert.ToString(ddltype.SelectedItem) == "Invoice No")
            {
                if (rdbdept.Checked == true && txtNew.Text != "")
                {


                    Sql = "update bookdetails set Bill_No='" + txtNew.Text + "' WHERE 1 = 1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Bill_No in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";

                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please select Correct Data from the list";
                    }


                }
                if (rdbprice.Checked == true && txtprice.Text != "")
                {
                    Sql = "update bookdetails set price ='" + txtprice.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Bill_No in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {

                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (Rdbref.Checked == true)
                {
                    string reff = "0";

                    if (rdbyes.Checked)
                        reff = "1";
                    else
                        reff = "0";
                    Sql = "update bookdetails set Ref='" + reff + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Bill_No in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }

            }
            else if (Convert.ToString(ddltype.SelectedItem) == "Call No")
            {
                if (rdbdept.Checked == true && txtNew.Text != "")
                {


                    Sql = "update bookdetails set Call_No='" + txtNew.Text + "' WHERE 1 = 1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Call_No in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";

                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please select Correct Data from the list";
                    }


                }
                if (rdbprice.Checked == true && txtprice.Text != "")
                {
                    Sql = "update bookdetails set price ='" + txtprice.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Call_No in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (Rdbref.Checked == true)
                {
                    string reff = "0";

                    if (rdbyes.Checked)
                        reff = "1";
                    else
                        reff = "0";
                    Sql = "update bookdetails set Ref='" + reff + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Call_No in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }

            }
            else if (Convert.ToString(ddltype.SelectedItem) == "Edition")
            {
                if (rdbdept.Checked == true && txtNew.Text != "")
                {


                    Sql = "update bookdetails set Edition='" + txtNew.Text + "' WHERE 1 = 1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Edition in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";

                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please select Correct Data from the list";
                    }


                }
                if (rdbprice.Checked == true && txtprice.Text != "")
                {
                    Sql = "update bookdetails set price ='" + txtprice.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Edition in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (Rdbref.Checked == true)
                {
                    string reff = "0";

                    if (rdbyes.Checked)
                        reff = "1";
                    else
                        reff = "0";
                    Sql = "update bookdetails set Ref='" + reff + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Edition in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }

            }
            else if (Convert.ToString(ddltype.SelectedItem) == "Year")
            {
                if (rdbdept.Checked == true && txtNew.Text != "")
                {


                    Sql = "update bookdetails set Pur_Year='" + txtNew.Text + "' WHERE 1 = 1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Pur_Year in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";

                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please select Correct Data from the list";
                    }


                }
                if (rdbprice.Checked == true && txtprice.Text != "")
                {
                    Sql = "update bookdetails set price ='" + txtprice.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Pur_Year in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (Rdbref.Checked == true)
                {
                    string reff = "0";

                    if (rdbyes.Checked)
                        reff = "1";
                    else
                        reff = "0";
                    Sql = "update bookdetails set Ref='" + reff + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Pur_Year in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }

            }
            else if (Convert.ToString(ddltype.SelectedItem) == "Book Price")
            {
                if (rdbdept.Checked == true && txtNew.Text != "")
                {


                    Sql = "update bookdetails set Price='" + txtNew.Text + "' WHERE 1 = 1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Price in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";

                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please select Correct Data from the list";
                    }


                }
                if (rdbprice.Checked == true && txtprice.Text != "")
                {
                    Sql = "update bookdetails set price ='" + txtprice.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Price in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (Rdbref.Checked == true)
                {
                    string reff = "0";

                    if (rdbyes.Checked)
                        reff = "1";
                    else
                        reff = "0";
                    Sql = "update bookdetails set Ref='" + reff + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Price in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }

            }
            else if (Convert.ToString(ddltype.SelectedItem) == "Discount")
            {
                if (rdbdept.Checked == true && txtNew.Text != "")
                {


                    Sql = "update bookdetails set B_Discount='" + txtNew.Text + "' WHERE 1 = 1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND B_Discount in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";

                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please select Correct Data from the list";
                    }


                }
                if (rdbprice.Checked == true && txtprice.Text != "")
                {
                    Sql = "update bookdetails set price ='" + txtprice.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND B_Discount in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (Rdbref.Checked == true)
                {
                    string reff = "0";

                    if (rdbyes.Checked)
                        reff = "1";
                    else
                        reff = "0";
                    Sql = "update bookdetails set Ref='" + reff + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND B_Discount in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }

            }
            else if (Convert.ToString(ddltype.SelectedItem) == "Book Pages")
            {
                if (rdbdept.Checked == true && txtNew.Text != "")
                {
                    Sql = "update bookdetails set Book_Size='" + txtNew.Text + "' WHERE 1 = 1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Book_Size in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";

                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please select Correct Data from the list";
                    }


                }
                if (rdbprice.Checked == true && txtprice.Text != "")
                {
                    Sql = "update bookdetails set price ='" + txtprice.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Book_Size in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (Rdbref.Checked == true)
                {
                    string reff = "0";

                    if (rdbyes.Checked)
                        reff = "1";
                    else
                        reff = "0";
                    Sql = "update bookdetails set Ref='" + reff + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Book_Size in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }

            }
            else if (Convert.ToString(ddltype.SelectedItem) == "Volume")
            {
                if (rdbdept.Checked == true && txtNew.Text != "")
                {
                    Sql = "update bookdetails set Volume='" + txtNew.Text + "' WHERE 1 = 1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Volume in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";

                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please select Correct Data from the list";
                    }


                }
                if (rdbprice.Checked == true && txtprice.Text != "")
                {
                    Sql = "update bookdetails set price ='" + txtprice.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Volume in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (Rdbref.Checked == true)
                {
                    string reff = "0";

                    if (rdbyes.Checked)
                        reff = "1";
                    else
                        reff = "0";
                    Sql = "update bookdetails set Ref='" + reff + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Volume in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }

            }
            else if (Convert.ToString(ddltype.SelectedItem) == "Collation")
            {
                if (rdbdept.Checked == true && txtNew.Text != "")
                {
                    Sql = "update bookdetails set Collabrator='" + txtNew.Text + "' WHERE 1 = 1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Collabrator in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";

                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please select Correct Data from the list";
                    }


                }
                if (rdbprice.Checked == true && txtprice.Text != "")
                {
                    Sql = "update bookdetails set price ='" + txtprice.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Collabrator in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (Rdbref.Checked == true)
                {
                    string reff = "0";

                    if (rdbyes.Checked)
                        reff = "1";
                    else
                        reff = "0";
                    Sql = "update bookdetails set Ref='" + reff + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Collabrator in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }

            }
            else if (Convert.ToString(ddltype.SelectedItem) == "Language")
            {
                if (rdbdept.Checked == true && txtNew.Text != "")
                {
                    Sql = "update bookdetails set Language='" + txtNew.Text + "' WHERE 1 = 1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Language in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";

                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please select Correct Data from the list";
                    }


                }
                if (rdbprice.Checked == true && txtprice.Text != "")
                {
                    Sql = "update bookdetails set price ='" + txtprice.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Language in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (Rdbref.Checked == true)
                {
                    string reff = "0";

                    if (rdbyes.Checked)
                        reff = "1";
                    else
                        reff = "0";
                    Sql = "update bookdetails set Ref='" + reff + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Language in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }

            }
            else if (Convert.ToString(ddltype.SelectedItem) == "Remarks")
            {
                if (rdbdept.Checked == true && txtNew.Text != "")
                {
                    Sql = "update bookdetails set Remark='" + txtNew.Text + "' WHERE 1 = 1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Remark in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";

                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please select Correct Data from the list";
                    }


                }
                if (rdbprice.Checked == true && txtprice.Text != "")
                {
                    Sql = "update bookdetails set price ='" + txtprice.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Remark in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (Rdbref.Checked == true)
                {
                    string reff = "0";

                    if (rdbyes.Checked)
                        reff = "1";
                    else
                        reff = "0";
                    Sql = "update bookdetails set Ref='" + reff + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND Remark in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }

            }
            else if (Convert.ToString(ddltype.SelectedItem) == "Date of Accession")
            {
                if (rdbdept.Checked == true && txtNew.Text != "")
                {
                    Sql = "update bookdetails set date_accession='" + txtNew.Text + "' WHERE 1 = 1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND date_accession in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";

                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please select Correct Data from the list";
                    }


                }
                if (rdbprice.Checked == true && txtprice.Text != "")
                {
                    Sql = "update bookdetails set price ='" + txtprice.Text + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND date_accession in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }
                if (Rdbref.Checked == true)
                {
                    string reff = "0";

                    if (rdbyes.Checked)
                        reff = "1";
                    else
                        reff = "0";
                    Sql = "update bookdetails set Ref='" + reff + "' WHERE 1=1 ";
                    if (Chkbytype.Checked == true)
                    {
                        if (typ != "")
                        {
                            Sql = Sql + " AND date_accession in('" + typ + "')";
                        }
                    }
                    if (Chkaccno.Checked == true)
                    {
                        Sql = Sql + " AND Acc_No BETWEEN '" + TextBox2.Text + "' AND '" + TextBox1.Text + "'";
                    }
                    int up = d2.update_method_wo_parameter(Sql, "Text");
                    int m = up;
                    if (m != 0)
                    {
                        txt_from.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Records Updated Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                    }
                }

            }
            Go_Click(sender, e);

        }


        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "library Entry Correction");
        }

    }

    protected void Chklan_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (Chklan.Checked == true)
            {
                ddllng.Enabled = true;
                lan();
            }
            else
            {
                ddllng.Enabled = false;
                ddllng.Items.Clear();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "library Entry Correction");
        }

    }


}