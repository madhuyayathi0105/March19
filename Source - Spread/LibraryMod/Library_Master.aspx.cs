using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using wc = System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BalAccess;
using DalConnection;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using FarPoint.Web.Spread;
using Gios.Pdf;
using System.IO;
using InsproDataAccess;
using System.Text;

public partial class LibraryMod_Library_Master : System.Web.UI.Page
{
    #region initialization

    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    DataTable dtCommon = new DataTable();
    SqlCommand cmd = new SqlCommand();
    static Hashtable Has_Stage = new Hashtable();
    ReuasableMethods ru = new ReuasableMethods();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    static string LookUpBtnId = "";

    int yesorno = 0;
    public void Connection()
    {
        con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
        con.Open();
    }
    DAccess2 d2 = new DAccess2();
    DAccess2 dacces2 = new DAccess2();
    DAccess2 da = new DAccess2();
    DAccess2 obi_access = new DAccess2();
    DataSet ds;
    string usercode = "", singleuser = "", group_user = "";
    string collegecode = "";
    string sql = string.Empty;
    string InsertQ, UpdateQ = string.Empty;
    static string lib_Name = "", librarian_name = "", librarian_location = "";

    DataTable dtLibrary = new DataTable();
    DataRow drow;
    #endregion

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
          
            if (!Page.IsPostBack)
            {
                Bindcollege();
                loadlibrary();
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btn_excel.Visible = false;
                cb_books_CheckedChanged(sender, e);
                Bindinward();
                bindddlcategoryofbook();
                btnMainGo_Click(sender, e);
            }
        }
        catch (Exception ex) { }
    }

    public void Bindcollege()
    {
        try
        {
            ddlCollege1.Items.Clear();
            dtCommon.Clear();
            ddlCollege1.Enabled = false;
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
                ddlCollege1.DataSource = dtCommon;
                ddlCollege1.DataTextField = "collname";
                ddlCollege1.DataValueField = "college_code";
                ddlCollege1.DataBind();
                ddlCollege1.SelectedIndex = 0;
                ddlCollege1.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    public void bindddlcategoryofbook()
    {
        try
        {
            string selectQuery = "Select Distinct Edu_Level from Course";
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            ddlcategoryofbook.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcategoryofbook.DataSource = ds;
                ddlcategoryofbook.DataTextField = "Edu_Level";
                ddlcategoryofbook.DataValueField = "Edu_Level";
                ddlcategoryofbook.DataBind();



            }

            ddlcategoryofbook.Items.Insert(0, "All");
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    public void Bindinward()
    {
        try
        {
            ddlcategoryofinward.Items.Clear();
            ddlcategoryofinward.Items.Add("All");
            ddlcategoryofinward.Items.Add("Books");
            ddlcategoryofinward.Items.Add("Periodicals");
            ddlcategoryofinward.Items.Add("Non-Book");
            ddlcategoryofinward.Items.Add("Back Volume");
            ddlcategoryofinward.Items.Add("Project Book");
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void ddlCollege1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadlibrary();
            cblibrary_CheckedChanged(sender, e);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    public void loadlibrary()
    {
        try
        {
            string selectQuery = "select lib_code,lib_name from library where college_code=" + ddlCollege1.SelectedValue;
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            cbllibrary.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbllibrary.DataSource = ds;
                cbllibrary.DataTextField = "lib_name";
                cbllibrary.DataValueField = "lib_code";
                cbllibrary.DataBind();
                if (cbllibrary.Items.Count > 0)
                {
                    for (int i = 0; i < cbllibrary.Items.Count; i++)
                    {
                        cbllibrary.Items[i].Selected = true;
                    }
                    Txtlibrary.Text = Label1.Text + "(" + cbllibrary.Items.Count + ")";
                    cblibrary.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void cblibrary_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cblibrary, cbllibrary, Txtlibrary, Label1.Text, "--Select--");


        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void cbllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxListChange(cblibrary, cbllibrary, Txtlibrary, Label1.Text, "--Select--");


        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)//rajasekar
    {
        try
        {
            btnnew_Click(sender, e);
            divTarvellerEntryDetails.Visible = true;
            btndelete.Enabled = false;
            Btnsave.Enabled = true;
            Btnupdate.Enabled = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    #region button Go

    protected void btnMainGo_Click(object sender, EventArgs e)
    {
        try
        {
            string vallabrary = string.Empty;
            int sno = 0;
            if (cbllibrary.Items.Count > 0)
                vallabrary = ru.GetSelectedItemsValueAsString(cbllibrary);

            if (!string.IsNullOrEmpty(ddlCollege1.Text) && !string.IsNullOrEmpty(vallabrary))
            {
                string sellib = "select lib_name,librarian,location from library where college_code in('" + ddlCollege1.SelectedValue + "') and lib_code in('" + vallabrary + "')";
                ds = da.select_method_wo_parameter(sellib, "Text");
                dtLibrary.Columns.Add("SNo", typeof(string));
                dtLibrary.Columns.Add("Library Name", typeof(string));
                dtLibrary.Columns.Add("Librarian", typeof(string));
                dtLibrary.Columns.Add("Location", typeof(string));
                drow = dtLibrary.NewRow();
                drow["SNo"] = "SNo";
                drow["Library Name"] = "Library Name";
                drow["Librarian"] = "Librarian";
                drow["Location"] = "Location";
                dtLibrary.Rows.Add(drow);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                    {
                        sno++;
                        drow = dtLibrary.NewRow();
                        drow["SNo"] = Convert.ToString(sno);
                        drow["Library Name"] = Convert.ToString(ds.Tables[0].Rows[r]["lib_name"]);
                        drow["Librarian"] = Convert.ToString(ds.Tables[0].Rows[r]["librarian"]);
                        drow["Location"] = Convert.ToString(ds.Tables[0].Rows[r]["location"]);
                        dtLibrary.Rows.Add(drow);
                    }
                    GrdLibMaster.DataSource = dtLibrary;
                    GrdLibMaster.DataBind();
                    GrdLibMaster.Visible = true;
                    btnprintmaster.Visible = true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btn_excel.Visible = true;
                    lbprint.Visible = false;
                    lblerrmainapp.Visible = false;
                }
                else
                {
                    lblerrmainapp.Visible = true;
                    btnprintmaster.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btn_excel.Visible = false;
                    lbprint.Visible = false;
                    lblerrmainapp.Text = "No Record(s) Found";
                }
            }
            else
            {
                lblerrmainapp.Visible = true;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btn_excel.Visible = false;
                lbprint.Visible = false;
                lblerrmainapp.Text = "Select All Field";
            }

            RowHead(GrdLibMaster);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void RowHead(GridView GrdLibMaster)
    {
        for (int head = 0; head < 1; head++)
        {
            GrdLibMaster.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            GrdLibMaster.Rows[head].Font.Bold = true;
            GrdLibMaster.Rows[head].HorizontalAlign = HorizontalAlign.Center;

        }
    }

    protected void GrdLibMaster_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void GrdLibMaster_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void GrdLibMaster_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);

            btnnew_Click(sender, e);
            Btnsave.Enabled = false;
            Btnupdate.Enabled = true;
            btndelete.Enabled = true;

            lib_Name = Convert.ToString(GrdLibMaster.Rows[rowIndex].Cells[1].Text);
            if (lib_Name.Contains("&"))
            {
                string[] splitlibName = lib_Name.Split('&');
                string len = splitlibName[1];
                len = len.Remove(0, 4);
                lib_Name = splitlibName[0] + "&" + len;
            }
            librarian_name = Convert.ToString(GrdLibMaster.Rows[rowIndex].Cells[2].Text);
            librarian_location = Convert.ToString(GrdLibMaster.Rows[rowIndex].Cells[3].Text);
            if (librarian_location.Contains("&"))
            {
                string[] splitlibLocation = librarian_location.Split('&');
                string len = splitlibLocation[1];
                len = len.Remove(0, 4);
                librarian_location = splitlibLocation[0] + "&" + len;
            }

            string editQ = "select lib_code,lib_name,librarian,location,isfine_off,AutoAccessNo,BackvolumeAutono,JournalAutono,nonbookmaterial,college_code,gen_acr, gen_stno,categ_acr,categ_stno,bv_acr,bv_stno,pm_acr,pm_stno,nm_acr,nm_stno,Access_Edu_Level,Books_DueDate,IsFine_Calculate,FineFrom,FineTo,ISBooks_DueDate,IsStudCloseDate,IsStaffCloseDate,StaffCloseDate,StudCloseDate,Is_BookBank,ISNULL(Lib_Head,'') as Lib_Head,Lib_FeeCode,StdAutoNo,Std_Acr,Std_StNo,ProcAutoNo,Proc_Acr,Proc_StNo,ISNULL(BB_AllStud,'') as BB_AllStud,ISNULL(AllowAllCollStud,'') as AllowAllCollStud,ISNULL(ProjAutoNo,'') as ProjAutoNo,ISNULL(Proj_Acr,'') as Proj_Acr,ISNULL(Proj_StNo,'') as Proj_StNo,ISNULL(LibGroupName,'') as LibGroupName from library where lib_name='" + lib_Name + "' and librarian='" + librarian_name + "' and location='" + librarian_location + "' and college_code=" + ddlCollege1.SelectedValue + "";
            DataSet edit = new DataSet();
            edit = da.select_method_wo_parameter(editQ, "Text");

            if (edit.Tables[0].Rows.Count > 0)
            {
                txtname.Text = edit.Tables[0].Rows[0]["lib_name"].ToString();
                Txtlibrarian.Text = edit.Tables[0].Rows[0]["librarian"].ToString();
                Txtlocation.Text = edit.Tables[0].Rows[0]["location"].ToString();
                txtacronym1.Text = edit.Tables[0].Rows[0]["gen_acr"].ToString();
                txtstartnum1.Text = edit.Tables[0].Rows[0]["gen_stno"].ToString();
                txtacronym2.Text = edit.Tables[0].Rows[0]["categ_acr"].ToString();
                txtstartnum2.Text = edit.Tables[0].Rows[0]["categ_stno"].ToString();
                txtacronym3.Text = edit.Tables[0].Rows[0]["bv_acr"].ToString();
                txtstartnum3.Text = edit.Tables[0].Rows[0]["bv_stno"].ToString();
                txtacronym4.Text = edit.Tables[0].Rows[0]["pm_acr"].ToString();
                txtstartnum4.Text = edit.Tables[0].Rows[0]["pm_stno"].ToString();
                txtacronym5.Text = edit.Tables[0].Rows[0]["nm_acr"].ToString();
                txtstartnum5.Text = edit.Tables[0].Rows[0]["nm_stno"].ToString();
                txtacronym6.Text = edit.Tables[0].Rows[0]["Std_Acr"].ToString();
                txtstartnum6.Text = edit.Tables[0].Rows[0]["Std_StNo"].ToString();
                txtacronym7.Text = edit.Tables[0].Rows[0]["Proc_Acr"].ToString();
                txtstartnum7.Text = edit.Tables[0].Rows[0]["Proc_StNo"].ToString();
                txtacronym8.Text = edit.Tables[0].Rows[0]["Proj_Acr"].ToString();
                txtstartnum8.Text = edit.Tables[0].Rows[0]["Proj_StNo"].ToString();
                string cate = edit.Tables[0].Rows[0]["Access_Edu_level"].ToString();
                if (cate == "")
                    ddlcategoryofbook.Text = "All";
                else
                    ddlcategoryofbook.Text = cate;

                string gen_acr = edit.Tables[0].Rows[0]["gen_acr"].ToString();
                string gen_stno = edit.Tables[0].Rows[0]["gen_stno"].ToString();

                string categ_acr = edit.Tables[0].Rows[0]["categ_acr"].ToString();
                string categ_stno = edit.Tables[0].Rows[0]["categ_stno"].ToString();

                if (gen_acr != "" || gen_stno != "")
                {
                    rbgeneral.Checked = true;
                    rbgeneral_CheckedChanged(sender, e);
                }
                else if (categ_acr != "" || categ_stno != "")
                {

                    rbcategorywist.Checked = true;
                    rbcategorywist_CheckedChanged(sender, e);
                }
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["isfine_off"]) == 1)
                {
                    rboffice.Checked = true;
                    rblibrary.Checked = false;
                }
                else
                {
                    rblibrary.Checked = true;
                    rboffice.Checked = false;
                }
                int autoAccNo = Convert.ToInt32(edit.Tables[0].Rows[0]["AutoAccessNo"]);
                if (autoAccNo == 1)
                    cb_books.Checked = true;
                else
                    cb_books.Checked = false;
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["backvolumeautono"]) == 1)
                    cbbackvolume.Checked = true;
                else
                    cbbackvolume.Checked = false;
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["journalautono"]) == 1)
                    cbperiodicalsmaster.Checked = true;
                else
                    cbperiodicalsmaster.Checked = false;
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["nonbookmaterial"]) == 1)
                    cbnonbook.Checked = true;
                else
                    cbnonbook.Checked = false;
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["ISBooks_DueDate"]) == 1)
                {
                    cbfixedduedate.Checked = true;
                    txtfixedduedate.Text = Convert.ToDateTime(edit.Tables[0].Rows[0]["Books_DueDate"]).ToString("dd/MM/yyyy");
                }
                else
                {
                    cbfixedduedate.Checked = false;
                    txtfixedduedate.Text = "";
                }
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["IsFine_Calculate"]) == 1)
                {
                    cbfinecalculation.Checked = true;
                    txtfinecalculationfrom.Text = Convert.ToDateTime(edit.Tables[0].Rows[0]["FineFrom"]).ToString("dd/MM/yyyy");
                    txtfinecalculationto.Text = Convert.ToDateTime(edit.Tables[0].Rows[0]["FineTo"]).ToString("dd/MM/yyyy");
                }
                else
                {
                    cbfinecalculation.Checked = false;
                    txtfinecalculationfrom.Text = "";
                    txtfinecalculationto.Text = "";
                }
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["IsStudCloseDate"]) == 1)
                {
                    cbbookissueclosedatestudents.Checked = true;
                    txtbookissueclosedatestudents.Text = Convert.ToDateTime(edit.Tables[0].Rows[0]["StudCloseDate"]).ToString("dd/MM/yyyy");
                }
                else
                {
                    cbbookissueclosedatestudents.Checked = false;
                    txtbookissueclosedatestudents.Text = "";
                }
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["IsStaffCloseDate"]) == 1)
                {
                    cbbookissueclosedatestaff.Checked = true;
                    txtbookissueclosedatestaff.Text = Convert.ToDateTime(edit.Tables[0].Rows[0]["StaffCloseDate"]).ToString("dd/MM/yyyy");
                }
                else
                {
                    cbbookissueclosedatestaff.Checked = false;
                    txtbookissueclosedatestaff.Text = "";
                }
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["Is_BookBank"]) == 1)
                    cbbookbank.Checked = true;
                else
                    cbbookbank.Checked = false;
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["StdAutoNo"]) == 1)
                    cbstandardmaster.Checked = true;
                else
                    cbstandardmaster.Checked = false;
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["ProcAutoNo"]) == 1)
                    cbproceedings.Checked = true;
                else
                    cbproceedings.Checked = false;
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["BB_AllStud"]) == 1)
                    cbbookbank.Checked = true;
                else
                    cbbookbank.Checked = false;
                int AllowAllCollStud = Convert.ToInt32(edit.Tables[0].Rows[0]["AllowAllCollStud"]);
                if (AllowAllCollStud == 1)
                    cballowallcollegestudandstaff.Checked = true;
                else
                    cballowallcollegestudandstaff.Checked = false;
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["ProjAutoNo"]) == 1)
                    cbprojectmaster.Checked = true;
                else
                    cbprojectmaster.Checked = false;
                cb_books_CheckedChanged(sender, e);
                rbgeneral_CheckedChanged(sender, e);
                rbcategorywist_CheckedChanged(sender, e);
                cbbackvolume_CheckedChanged(sender, e);
                cbperiodicalsmaster_CheckedChanged(sender, e);
                cbnonbook_CheckedChanged(sender, e);
                cbstandardmaster_CheckedChanged(sender, e);
                cbproceedings_CheckedChanged(sender, e);
                cbprojectmaster_CheckedChanged(sender, e);
                cbfixedduedate_CheckedChanged(sender, e);
                cbfinecalculation_CheckedChanged(sender, e);
                cbbookissueclosedatestudents_CheckedChanged(sender, e);
                cbbookissueclosedatestaff_CheckedChanged(sender, e);
                //string qry = "select cat from libcat where college_code='" + ddlCollege1.SelectedValue + "' and lib_name='" + lib_Name + "'";
                //DataSet dslibcat = new DataSet();
                //dslibcat = da.select_method_wo_parameter(qry, "text");
                //if (dslibcat.Tables[0].Rows.Count > 0)
                //{
                //    string categ = Convert.ToString(dslibcat.Tables[0].Rows[0]["cat"]);
                //    ddlcategoryofinward.Items.FindByText(categ).Selected = true;
                //}
            }
            divTarvellerEntryDetails.Visible = true;

        }
        catch
        {
        }
    }

    #endregion

    #region Location Look Up

    protected void Btnlocation_Click(object sender, EventArgs e)
    {
        try
        {
            LookUpBtnId = btnlocation.ID;
            panel3.Visible = true;
            BindCollege();
            Lbltittle.Text = "Select Location";
            loadbuilding();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void loadbuilding()
    {
        try
        {
            lblDepartment.Visible = false;
            ddldepratstaff.Visible = false;
            lblsearchby.Visible = false;
            ddlstaff.Visible = false;
            txt_search.Visible = false;
            sql = "select Code,Building_Name,Building_Type,building_description,BuildType from building_master where College_Code='" + ddlcollege.SelectedValue + "'";

            DataSet dsbindspread = new DataSet();
            dsbindspread = obi_access.select_method_wo_parameter(sql, "Text");
            DataTable dtLocation = new DataTable();
            DataRow drow;
            dtLocation.Columns.Add("Building Code", typeof(string));
            dtLocation.Columns.Add("Building Name", typeof(string));
            if (dsbindspread.Tables[0].Rows.Count > 0)
            {
                int sno = 0;
                for (int rolcount = 0; rolcount < dsbindspread.Tables[0].Rows.Count; rolcount++)
                {
                    sno++;
                    string name = dsbindspread.Tables[0].Rows[rolcount]["Building_Name"].ToString();
                    string code = dsbindspread.Tables[0].Rows[rolcount]["Code"].ToString();
                    drow = dtLocation.NewRow();
                    drow["Building Code"] = Convert.ToString(code);
                    drow["Building Name"] = Convert.ToString(name);
                    dtLocation.Rows.Add(drow);
                }
                GrdLocation.DataSource = dtLocation;
                GrdLocation.DataBind();
                GrdLocation.Visible = true;
                GrdStaff.Visible = false;

                for (int l = 0; l < GrdLocation.Rows.Count; l++)
                {
                    foreach (GridViewRow row in GrdLocation.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            GrdLocation.Rows[l].Cells[2].HorizontalAlign = HorizontalAlign.Left;
                            GrdLocation.Rows[l].Cells[3].HorizontalAlign = HorizontalAlign.Left;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }

    }

    protected void GrdLocation_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        GrdLocation.PageIndex = e.NewPageIndex;
        loadbuilding();
    }

    #endregion

    #region Staff Look Up

    protected void Btnlibrarian_Click(object sender, EventArgs e)
    {
        try
        {
            LookUpBtnId = Btnlibrarian.ID;
            panel3.Visible = true;
            //fsstaff.Visible = true;
            //fsstaff.Sheets[0].RowCount = 0;
            BindCollege();
            loadstaffdep(collegecode);
            loadfsstaff();
            Lbltittle.Text = "Select Staff Incharge";
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    public void loadstaffdep(string collegecode)
    {
        try
        {
            string cmd = "select distinct dept_name,dept_code from hrdept_master where college_code=" + Session["collegecode"] + "";

            DataSet ds = new DataSet();
            ds = obi_access.select_method_wo_parameter(cmd, "Text");

            ddldepratstaff.DataSource = ds;
            ddldepratstaff.DataTextField = "dept_name";
            ddldepratstaff.DataValueField = "dept_code";
            ddldepratstaff.DataBind();
            ddldepratstaff.Items.Insert(0, "All");
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }

    }

    protected void loadfsstaff()
    {
        try
        {
            lblDepartment.Visible = true;
            ddldepratstaff.Visible = true;
            lblsearchby.Visible = true;
            ddlstaff.Visible = true;
            txt_search.Visible = true;
            if (ddldepratstaff.SelectedIndex != 0)
            {
                if (txt_search.Text != "")
                {
                    if (ddlstaff.SelectedIndex == 0)
                    {
                        sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0)and  (staffmaster.settled = 0)  and (staff_name like '" + txt_search.Text + "%') and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code) and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";
                    }
                    else if (ddlstaff.SelectedIndex == 1)
                    {
                        sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '" + txt_search.Text + "%') and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code) and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";
                    }
                }
                else
                {

                    sql = "SELECT staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";
                }
            }
            else if (txt_search.Text != "")
            {
                if (ddlstaff.SelectedIndex == 0)
                {
                    sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_name like '%" + txt_search.Text + "%') and (staffmaster.college_code =hrdept_master.college_code) and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";
                }
                else if (ddlstaff.SelectedIndex == 1)
                {
                    sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '%" + txt_search.Text + "%') and (staffmaster.college_code =hrdept_master.college_code) and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";
                }
                else if (ddlcollege.SelectedIndex != -1)
                {
                    sql = "select distinct staffmaster.staff_code, staff_name  from stafftrans,staffmaster where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and settled=0 and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";
                }
                else
                {
                    sql = "select distinct staffmaster.staff_code, staff_name from stafftrans,staffmaster,hrdept_master.dept_name where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";
                }
            }
            else
                if (ddldepratstaff.SelectedValue.ToString() == "All")
                {
                    sql = "select distinct staffmaster.staff_code, staff_name  from stafftrans,staffmaster where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and settled=0 and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";
                }

            DataSet dsbindspread = new DataSet();
            dsbindspread = obi_access.select_method_wo_parameter(sql, "Text");

            DataTable dtStaff = new DataTable();
            DataRow drow;
            dtStaff.Columns.Add("Staff Code", typeof(string));
            dtStaff.Columns.Add("Staff Name", typeof(string));
            if (dsbindspread.Tables[0].Rows.Count > 0)
            {
                int sno = 0;
                for (int rolcount = 0; rolcount < dsbindspread.Tables[0].Rows.Count; rolcount++)
                {
                    sno++;
                    string name = dsbindspread.Tables[0].Rows[rolcount]["staff_name"].ToString();
                    string code = dsbindspread.Tables[0].Rows[rolcount]["staff_code"].ToString();

                    drow = dtStaff.NewRow();
                    drow["Staff Code"] = Convert.ToString(code);
                    drow["Staff Name"] = Convert.ToString(name);
                    dtStaff.Rows.Add(drow);
                }
                GrdStaff.DataSource = dtStaff;
                GrdStaff.DataBind();
                GrdStaff.Visible = true;
                GrdLocation.Visible = false;
                for (int l = 0; l < GrdStaff.Rows.Count; l++)
                {
                    foreach (GridViewRow row in GrdStaff.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            GrdStaff.Rows[l].Cells[3].HorizontalAlign = HorizontalAlign.Left;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void btnstaffadd_Click(object sender, EventArgs e)
    {
        try
        {
            int okflag = 0;
            //string btnStaffOrLocation=btnlocation.ID;
            if (LookUpBtnId == "Btnlibrarian")
            {
                foreach (GridViewRow gvrow in GrdStaff.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("CheckBox1");
                    if (chk.Checked == true)
                    {
                        okflag = 1;
                        int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                        string StaffCode = Convert.ToString(GrdStaff.Rows[RowCnt].Cells[2].Text);
                        string StaffName = Convert.ToString(GrdStaff.Rows[RowCnt].Cells[3].Text);
                        Txtlibrarian.Text = StaffName;
                    }
                }
                if (okflag == 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Any Staff')", true);
                }
            }
            if (LookUpBtnId == "btnlocation")
            {
                foreach (GridViewRow gvrow in GrdLocation.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("CheckBox1");
                    if (chk.Checked == true)
                    {
                        okflag = 1;
                        int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                        string buildCode = Convert.ToString(GrdLocation.Rows[RowCnt].Cells[2].Text);
                        string buildName = Convert.ToString(GrdLocation.Rows[RowCnt].Cells[3].Text);
                        Txtlocation.Text = buildName;
                    }
                }
                if (okflag == 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Any Building')", true);
                }
            }
            panel3.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        try
        {
            panel3.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void GrdStaff_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void GrdStaff_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        GrdStaff.PageIndex = e.NewPageIndex;
        loadfsstaff();
    }

    #endregion

    public void BindCollege()
    {
        try
        {
            string cmd = "select collname,college_code from collinfo";
            DataSet ds = new DataSet();
            ds = obi_access.select_method_wo_parameter(cmd, "Text");
            ddlcollege.DataSource = ds;
            ddlcollege.DataTextField = "collname";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }

    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //fsstaff.Sheets[0].RowCount = 0;
            if (Lbltittle.Text == "Select Staff Incharge")
                loadfsstaff();
            else
                loadbuilding();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void ddldepratstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            //fsstaff.Sheets[0].RowCount = 0;
            loadfsstaff();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void ddlstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //fsstaff.Sheets[0].RowCount = 0;
            loadfsstaff();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void txt_search_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //fsstaff.Sheets[0].RowCount = 0;
            loadfsstaff();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void cb_books_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_books.Checked == false)
            {
                rbgeneral.Checked = false;
                lblacronym1.Visible = false;
                txtacronym1.Visible = false;
                lblstartnum1.Visible = false;
                txtstartnum1.Visible = false;
                rbcategorywist.Checked = false;
                lblacronym2.Visible = false;
                txtacronym2.Visible = false;
                lblstartnum2.Visible = false;
                txtstartnum2.Visible = false;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void rbgeneral_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_books.Checked == true)
            {
                if (rbgeneral.Checked == true)
                {

                    lblacronym1.Visible = true;
                    txtacronym1.Visible = true;
                    lblstartnum1.Visible = true;
                    txtstartnum1.Visible = true;
                    lblacronym2.Visible = false;
                    txtacronym2.Visible = false;
                    lblstartnum2.Visible = false;
                    txtstartnum2.Visible = false;

                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void rbcategorywist_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_books.Checked == true)
            {
                if (rbcategorywist.Checked == true)
                {

                    lblacronym1.Visible = false;
                    txtacronym1.Visible = false;
                    lblstartnum1.Visible = false;
                    txtstartnum1.Visible = false;
                    lblacronym2.Visible = true;
                    txtacronym2.Visible = true;
                    lblstartnum2.Visible = true;
                    txtstartnum2.Visible = true;

                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void cbbackvolume_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbbackvolume.Checked == true)
            {



                lblacronym3.Visible = true;
                txtacronym3.Visible = true;
                lblstartnum3.Visible = true;
                txtstartnum3.Visible = true;


            }
            if (cbbackvolume.Checked == false)
            {



                lblacronym3.Visible = false;
                txtacronym3.Visible = false;
                lblstartnum3.Visible = false;
                txtstartnum3.Visible = false;


            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void cbperiodicalsmaster_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbperiodicalsmaster.Checked == true)
            {



                lblacronym4.Visible = true;
                txtacronym4.Visible = true;
                lblstartnum4.Visible = true;
                txtstartnum4.Visible = true;


            }
            if (cbperiodicalsmaster.Checked == false)
            {



                lblacronym4.Visible = false;
                txtacronym4.Visible = false;
                lblstartnum4.Visible = false;
                txtstartnum4.Visible = false;


            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void cbnonbook_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbnonbook.Checked == true)
            {



                lblacronym5.Visible = true;
                txtacronym5.Visible = true;
                lblstartnum5.Visible = true;
                txtstartnum5.Visible = true;


            }
            if (cbnonbook.Checked == false)
            {



                lblacronym5.Visible = false;
                txtacronym5.Visible = false;
                lblstartnum5.Visible = false;
                txtstartnum5.Visible = false;


            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void cbstandardmaster_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbstandardmaster.Checked == true)
            {



                lblacronym6.Visible = true;
                txtacronym6.Visible = true;
                lblstartnum6.Visible = true;
                txtstartnum6.Visible = true;


            }
            if (cbstandardmaster.Checked == false)
            {



                lblacronym6.Visible = false;
                txtacronym6.Visible = false;
                lblstartnum6.Visible = false;
                txtstartnum6.Visible = false;


            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void cbproceedings_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbproceedings.Checked == true)
            {



                lblacronym7.Visible = true;
                txtacronym7.Visible = true;
                lblstartnum7.Visible = true;
                txtstartnum7.Visible = true;


            }
            if (cbproceedings.Checked == false)
            {



                lblacronym7.Visible = false;
                txtacronym7.Visible = false;
                lblstartnum7.Visible = false;
                txtstartnum7.Visible = false;


            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void cbprojectmaster_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (cbprojectmaster.Checked == true)
            {



                lblacronym8.Visible = true;
                txtacronym8.Visible = true;
                lblstartnum8.Visible = true;
                txtstartnum8.Visible = true;


            }
            if (cbprojectmaster.Checked == false)
            {



                lblacronym8.Visible = false;
                txtacronym8.Visible = false;
                lblstartnum8.Visible = false;
                txtstartnum8.Visible = false;


            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void cbfixedduedate_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbfixedduedate.Checked == true)
            {
                txtfixedduedate.Enabled = true;

            }
            if (cbfixedduedate.Checked == false)
            {
                txtfixedduedate.Enabled = false;

            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void cbfinecalculation_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbfinecalculation.Checked == true)
            {
                txtfinecalculationfrom.Enabled = true;
                txtfinecalculationto.Enabled = true;

            }
            if (cbfinecalculation.Checked == false)
            {
                txtfinecalculationfrom.Enabled = false;
                txtfinecalculationto.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void cbbookissueclosedatestudents_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbbookissueclosedatestudents.Checked == true)
            {
                txtbookissueclosedatestudents.Enabled = true;

            }
            if (cbbookissueclosedatestudents.Checked == false)
            {
                txtbookissueclosedatestudents.Enabled = false;

            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void cbbookissueclosedatestaff_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbbookissueclosedatestaff.Checked == true)
            {
                txtbookissueclosedatestaff.Enabled = true;

            }
            if (cbbookissueclosedatestaff.Checked == false)
            {
                txtbookissueclosedatestaff.Enabled = false;
                Btnattplus.Visible = true;
                Btnacch.Visible = true;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void ddlcategoryofinward_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


            Btnattplus.Visible = true;
            Btnacch.Visible = true;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }



    }

    protected void Btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtname.Text == "")
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Enter Name.";
                return;
            }
            if (Txtlibrarian.Text == "")
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Librarian.";
                return;
            }
            if (Txtlocation.Text == "")
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Location.";
                return;
            }
            if (rblibrary.Checked == false && rboffice.Checked == false)
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Fine Collection.";
                return;
            }
            if (cb_books.Checked == true && rbgeneral.Checked == false && rbgeneral.Checked == false)
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Either General Or Category Option must Select";
                return;
            }
            if (rbgeneral.Checked == true)
            {
                if (txtacronym1.Text == "" || txtstartnum1.Text == "")
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Either Acronym or Start Number is Must ";
                    return;
                }
            }
            if (rbcategorywist.Checked == true)
            {
                if (txtacronym2.Text == "" || txtstartnum2.Text == "")
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Either Acronym or Start Number is Must ";
                    return;
                }
            }
            if (cbbackvolume.Checked == true)
            {
                if (txtacronym3.Text == "" || txtstartnum3.Text == "")
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Either Acronym or Start Number is Must ";
                    return;
                }
            }
            if (cbperiodicalsmaster.Checked == true)
            {
                if (txtacronym4.Text == "" || txtstartnum4.Text == "")
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Either Acronym or Start Number is Must ";
                    return;
                }
            }
            if (cbnonbook.Checked == true)
            {
                if (txtacronym5.Text == "" || txtstartnum5.Text == "")
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Either Acronym or Start Number is Must ";
                    return;
                }
            }
            if (cbstandardmaster.Checked == true)
            {
                if (txtacronym6.Text == "" || txtstartnum6.Text == "")
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Either Acronym or Start Number is Must ";
                    return;
                }
            }
            if (cbproceedings.Checked == true)
            {
                if (txtacronym7.Text == "" || txtstartnum7.Text == "")
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Either Acronym or Start Number is Must ";
                    return;
                }
            }
            if (cbprojectmaster.Checked == true)
            {
                if (txtacronym8.Text == "" || txtstartnum8.Text == "")
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Either Acronym or Start Number is Must ";
                    return;
                }
            }
            if (yesorno == 0)
            {
                if (cbnonbook.Checked == false)
                {
                    imgAlert.Visible = true;
                    btn_alertclose.Visible = false;
                    btn_yes.Visible = true;
                    btn_No.Visible = true;
                    lbl_alert.Text = "Do you want to save Without Generating Automatic Access No";
                    return;
                }
            }
            int libcode1, finelib, typeno, backvolume, periodicalsmaster, nonbook, fixedduedate, cbfine, isclosedatestudents, isclosedatestaff, bookbank, Int_FeeCode, standardmaster, proceedings, projectmaster, allowallcollegestu, save = 0;
            Int_FeeCode = -1;
            string categoryofbook, textduedate, finefrom, fineto, closedatestudents, closedatestaff = "";
            string sqll2 = "SELECT MAX(cast(lib_code AS numeric)) As value FROM library";
            DateTime duedate = new DateTime();
            DateTime finedatefrome = new DateTime();
            DateTime finedateto = new DateTime();
            DateTime closedatestud = new DateTime();
            DateTime closedatestaff1 = new DateTime();

            DataSet dss2 = new DataSet();
            dss2 = obi_access.select_method_wo_parameter(sqll2, "Text");
            string LibValue = "";
            if (dss2.Tables[0].Rows.Count > 0)
            {
                LibValue = Convert.ToString(dss2.Tables[0].Rows[0][0]);
                if (!string.IsNullOrEmpty(LibValue))
                    libcode1 = Convert.ToInt32(LibValue) + 1;
                else
                    libcode1 = 1;
            }
            else
                libcode1 = 1;

            if (rblibrary.Checked == true)
                finelib = 0;
            else//rrrr
                finelib = 1;
            if (cb_books.Checked == true)
                typeno = 1;
            else
                typeno = 0;
            if (cbbackvolume.Checked == true)
                backvolume = 1;
            else
                backvolume = 0;

            if (cbperiodicalsmaster.Checked == true)
                periodicalsmaster = 1;
            else
                periodicalsmaster = 0;

            if (cbnonbook.Checked == true)
                nonbook = 1;
            else
                nonbook = 0;

            if (ddlcategoryofbook.Text == "All")
                categoryofbook = "";
            else
                categoryofbook = ddlcategoryofbook.SelectedValue;

            if (cbfixedduedate.Checked == true)
                fixedduedate = 1;
            else
                fixedduedate = 0;

            if (cbfixedduedate.Checked == true)
            {
                duedate = TextToDate(txtfixedduedate);
            }
            else
                duedate = Convert.ToDateTime("01/01/1900");

            if (cbfinecalculation.Checked == true)
            {
                cbfine = 1;
                finedatefrome = TextToDate(txtfinecalculationfrom);
                finedateto = TextToDate(txtfinecalculationto);
            }
            else
            {
                cbfine = 0;
                finedatefrome = Convert.ToDateTime("01/01/1900");
                finedateto = Convert.ToDateTime("01/01/1900");
            }

            if (cbbookissueclosedatestudents.Checked == true)
            {
                isclosedatestudents = 1;
                closedatestud = TextToDate(txtbookissueclosedatestudents);
            }
            else
            {
                isclosedatestudents = 0;
                closedatestud = Convert.ToDateTime("01/01/1900");
            }

            if (cbbookissueclosedatestaff.Checked == true)
            {
                isclosedatestaff = 1;
                closedatestaff1 = TextToDate(txtbookissueclosedatestaff);
            }
            else
            {
                isclosedatestaff = 0;
                closedatestaff1 = Convert.ToDateTime("01/01/1900");
            }

            if (cbbookbank.Checked == true)
                bookbank = 1;
            else
                bookbank = 0;

            if (cbstandardmaster.Checked == true)
                standardmaster = 1;
            else
                standardmaster = 0;

            if (cbproceedings.Checked == true)
                proceedings = 1;
            else
                proceedings = 0;

            if (cbprojectmaster.Checked == true)
                projectmaster = 1;
            else
                projectmaster = 0;

            if (cballowallcollegestudandstaff.Checked == true)
                allowallcollegestu = 1;
            else
                allowallcollegestu = 0;
            if (cbfixedduedate.Checked == true)
            {
                InsertQ = "insert into library(lib_code,lib_name,librarian,location,isfine_off,AutoAccessNo,backvolumeautono,journalautono,college_code,gen_acr,gen_stno,categ_acr,categ_stno,bv_acr,bv_stno,pm_acr,pm_stno,nm_acr,nm_stno,nonbookmaterial,Access_Edu_Level,ISBooks_DueDate,Books_DueDate,IsFine_Calculate,FineFrom,FineTo,IsStudCloseDate,StudCloseDate,IsStaffCloseDate,StaffCloseDate,Is_BookBank,Lib_FeeCode,StdAutoNo,Std_Acr,Std_StNo,ProcAutoNo,Proc_Acr,Proc_StNo,BB_AllStud,AllowAllCollStud,ProjAutoNo,Proj_Acr,Proj_StNo)";
                InsertQ = InsertQ + " values('" + libcode1 + "','" + txtname.Text.Trim() + "','" + Txtlibrarian.Text.Trim() + "','" + Txtlocation.Text.Trim() + "'," + finelib + "," + typeno + " ," + backvolume + "," + periodicalsmaster + "," + ddlCollege1.SelectedValue + ",'" + txtacronym1.Text + "','" + txtstartnum1.Text + "','" + txtacronym2.Text + "','" + txtstartnum2.Text + "','" + txtacronym3.Text + "','" + txtstartnum3.Text + "','" + txtacronym4.Text + "','" + txtstartnum4.Text + "','" + txtacronym5.Text + "','" + txtstartnum5.Text + "'," + nonbook + ",'" + categoryofbook + "'," + fixedduedate + ",";

                InsertQ = InsertQ + "'" + duedate + "'," + cbfine + ",'" + finedatefrome + "','" + finedateto + "'," + isclosedatestudents + ",'" + closedatestud + "'," + isclosedatestaff + ",'" + closedatestaff1 + "'," + bookbank + "," + Int_FeeCode + ",'" + standardmaster + "','" + txtacronym6.Text + "','" + txtstartnum6.Text + "','" + proceedings + "','" + txtacronym7.Text + "','" + txtstartnum7.Text + "'," + bookbank + ",'" + allowallcollegestu + "','" + projectmaster + "','" + txtacronym8.Text + "','" + txtstartnum8.Text + "')";

            }
            else
            {
                InsertQ = "insert into library(lib_code,lib_name,librarian,location,isfine_off,AutoAccessNo,backvolumeautono,journalautono,college_code,gen_acr,gen_stno,categ_acr,categ_stno,bv_acr,bv_stno,pm_acr,pm_stno,nm_acr,nm_stno,nonbookmaterial,Access_Edu_Level,ISBooks_DueDate,Books_DueDate,IsFine_Calculate,FineFrom,FineTo,IsStudCloseDate,StudCloseDate,IsStaffCloseDate,StaffCloseDate,Is_BookBank,Lib_FeeCode,StdAutoNo,Std_Acr,Std_StNo,ProcAutoNo,Proc_Acr,Proc_StNo,BB_AllStud,AllowAllCollStud,ProjAutoNo,Proj_Acr,Proj_StNo) ";
                InsertQ = InsertQ + "values('" + libcode1 + "','" + txtname.Text.Trim() + "','" + Txtlibrarian.Text.Trim() + "','" + Txtlocation.Text.Trim() + "'," + finelib + "," + typeno + " ," + backvolume + "," + periodicalsmaster + "," + ddlCollege1.SelectedValue + ",'" + txtacronym1.Text + "','" + txtstartnum1.Text + "','" + txtacronym2.Text + "','" + txtstartnum2.Text + "','" + txtacronym3.Text + "','" + txtstartnum3.Text + "','" + txtacronym4.Text + "','" + txtstartnum4.Text + "','" + txtacronym5.Text + "','" + txtstartnum5.Text + "'," + nonbook + ",'" + categoryofbook + "'," + fixedduedate + ",";
                InsertQ = InsertQ + "'" + duedate + "'," + cbfine + ",'" + finedatefrome + "','" + finedateto + "'," + isclosedatestudents + ",'" + closedatestud + "'," + isclosedatestaff + ",'" + closedatestaff1 + "'," + bookbank + "," + Int_FeeCode + ",'" + standardmaster + "','" + txtacronym6.Text + "','" + txtstartnum6.Text + "','" + proceedings + "','" + txtacronym7.Text + "','" + txtstartnum7.Text + "'," + bookbank + ",'" + allowallcollegestu + "','" + projectmaster + "','" + txtacronym8.Text + "','" + txtstartnum8.Text + "')";

            }
            save = dacces2.update_method_wo_parameter(InsertQ, "Text");
            string libcatInsert = " insert into libcat(lib_code,lib_name,cat,college_code) values ('" + libcode1 + "','" + txtname.Text.Trim() + "','" + ddlcategoryofinward.SelectedItem.Text + "'," + ddlCollege1.SelectedValue + ")";
            int libcatSave = dacces2.update_method_wo_parameter(libcatInsert, "Text");
            if (save == 1)
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Saved Sucessfully";
                btnnew_Click(sender, e);
                loadlibrary();
                btnMainGo_Click(sender, e);
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void Btnupdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtname.Text == "")
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Enter Name.";
                return;
            }
            if (Txtlibrarian.Text == "")
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Librarian.";
                return;
            }
            if (Txtlocation.Text == "")
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Location.";
                return;
            }
            if (rblibrary.Checked == false && rboffice.Checked == false)
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Fine Collection.";
                return;
            }
            int libcode1, finelib, typeno, backvolume, periodicalsmaster, nonbook, fixedduedate, cbfine, isclosedatestudents, isclosedatestaff, bookbank, Int_FeeCode, standardmaster, proceedings, projectmaster, allowallcollegestu, save = 0;
            Int_FeeCode = -1;
            string librarycode = "";
            string categoryofbook, textduedate, finefrom, fineto, closedatestudents, closedatestaff = "";
            string sqll2 = "SELECT lib_code As value FROM library where lib_name='" + txtname.Text.Trim() + "'";
            DateTime duedate = new DateTime();
            DateTime finedatefrome = new DateTime();
            DateTime finedateto = new DateTime();
            DateTime closedatestud = new DateTime();
            DateTime closedatestaff1 = new DateTime();
            DataSet dss2 = new DataSet();
            dss2 = obi_access.select_method_wo_parameter(sqll2, "Text");
            if (dss2.Tables[0].Rows.Count > 0)
                librarycode = Convert.ToString(dss2.Tables[0].Rows[0][0]);

            if (rblibrary.Checked == true)
                finelib = 0;
            else//rrrr
                finelib = 1;
            if (cb_books.Checked == true)
                typeno = 1;
            else
                typeno = 0;
            if (cbbackvolume.Checked == true)
                backvolume = 1;
            else
                backvolume = 0;

            if (cbperiodicalsmaster.Checked == true)
                periodicalsmaster = 1;
            else
                periodicalsmaster = 0;

            if (cbnonbook.Checked == true)
                nonbook = 1;
            else
                nonbook = 0;

            if (ddlcategoryofbook.Text == "All")
                categoryofbook = "";
            else
                categoryofbook = ddlcategoryofbook.SelectedValue;

            if (cbfixedduedate.Checked == true)
                fixedduedate = 1;
            else
                fixedduedate = 0;

            if (cbfixedduedate.Checked == true)
            {
                duedate = TextToDate(txtfixedduedate);
            }
            else
                duedate = Convert.ToDateTime("01/01/1900");

            if (cbfinecalculation.Checked == true)
            {
                cbfine = 1;
                finedatefrome = TextToDate(txtfinecalculationfrom);
                finedateto = TextToDate(txtfinecalculationto);
            }
            else
            {
                cbfine = 0;
                finedatefrome = Convert.ToDateTime("01/01/1900");
                finedateto = Convert.ToDateTime("01/01/1900");
            }

            if (cbbookissueclosedatestudents.Checked == true)
            {
                isclosedatestudents = 1;
                closedatestud = TextToDate(txtbookissueclosedatestudents);
            }
            else
            {
                isclosedatestudents = 0;
                closedatestud = Convert.ToDateTime("01/01/1900");
            }

            if (cbbookissueclosedatestaff.Checked == true)
            {
                isclosedatestaff = 1;
                closedatestaff1 = TextToDate(txtbookissueclosedatestaff);
            }
            else
            {
                isclosedatestaff = 0;
                closedatestaff1 = Convert.ToDateTime("01/01/1900");
            }

            if (cbbookbank.Checked == true)
                bookbank = 1;
            else
                bookbank = 0;

            if (cbstandardmaster.Checked == true)
                standardmaster = 1;
            else
                standardmaster = 0;

            if (cbproceedings.Checked == true)
                proceedings = 1;
            else
                proceedings = 0;

            if (cbprojectmaster.Checked == true)
                projectmaster = 1;
            else
                projectmaster = 0;

            if (cballowallcollegestudandstaff.Checked == true)
                allowallcollegestu = 1;
            else
                allowallcollegestu = 0;

            UpdateQ = "update library set lib_name='" + txtname.Text.Trim() + "',librarian='" + Txtlibrarian.Text.Trim() + "',location='" + Txtlocation.Text.Trim() + "',isfine_off=" + finelib + ",AutoAccessNo=" + typeno + " ,backvolumeautono=" + backvolume + ",journalautono=" + periodicalsmaster + ",college_code=" + ddlCollege1.SelectedValue + ",gen_acr='" + txtacronym1.Text + "',gen_stno='" + txtstartnum1.Text + "',categ_acr='" + txtacronym2.Text + "',categ_stno='" + txtstartnum2.Text + "',bv_acr='" + txtacronym3.Text + "',bv_stno='" + txtstartnum3.Text + "',pm_acr='" + txtacronym4.Text + "',pm_stno='" + txtstartnum4.Text + "',nm_acr='" + txtacronym5.Text + "',nm_stno='" + txtstartnum5.Text + "',nonbookmaterial=" + nonbook + ",Access_Edu_Level='" + categoryofbook + "',ISBooks_DueDate=" + fixedduedate + ",Books_DueDate='" + duedate + "',IsFine_Calculate=" + cbfine + ",FineFrom='" + finedatefrome + "',FineTo='" + finedateto + "',IsStudCloseDate=" + isclosedatestudents + ",StudCloseDate='" + closedatestud + "',IsStaffCloseDate=" + isclosedatestaff + ",StaffCloseDate='" + closedatestaff1 + "',Is_BookBank=" + bookbank + ",Lib_FeeCode=" + Int_FeeCode + ",StdAutoNo='" + standardmaster + "',Std_Acr='" + txtacronym6.Text + "',Std_StNo='" + txtstartnum6.Text + "',ProcAutoNo='" + proceedings + "',Proc_Acr='" + txtacronym7.Text + "',Proc_StNo='" + txtstartnum7.Text + "',BB_AllStud=" + bookbank + ",AllowAllCollStud='" + allowallcollegestu + "',ProjAutoNo='" + projectmaster + "',Proj_Acr='" + txtacronym8.Text + "',Proj_StNo='" + txtstartnum8.Text + "' where lib_name='" + lib_Name + "' and librarian='" + librarian_name + "' and location='" + librarian_location + "' and college_code=" + ddlCollege1.SelectedValue + "";

            save = dacces2.update_method_wo_parameter(UpdateQ, "Text");
            string libcatInsert = "if exists(select * from libcat where lib_name='" + lib_Name + "' and college_code=" + ddlCollege1.SelectedValue + ") update libcat set cat='" + ddlcategoryofinward.SelectedItem.Text + "',lib_code='" + librarycode + "' where lib_name='" + lib_Name + "' and college_code=" + ddlCollege1.SelectedValue + " else insert into libcat(lib_code,lib_name,cat,college_code) values ('" + librarycode + "','" + txtname.Text.Trim() + "','" + ddlcategoryofinward.SelectedItem.Text + "'," + ddlCollege1.SelectedValue + ")";
            int libcatSave = dacces2.update_method_wo_parameter(libcatInsert, "Text");
            if (save == 1)
            {

                imgAlert.Visible = true;
                lbl_alert.Text = "Updated Sucessfully";
                btnnew_Click(sender, e);
                loadlibrary();
                btnMainGo_Click(sender, e);
            }



        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    public DateTime TextToDate(TextBox txt)
    {
        DateTime dt = new DateTime();
        string firstdate = Convert.ToString(txt.Text);
        string[] split = firstdate.Split('/');
        dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
        return dt;
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            string deleteQ;
            deleteQ = " delete from library where lib_name='" + txtname.Text + "' and librarian='" + Txtlibrarian.Text + "' and location='" + Txtlocation.Text + "' and college_code=" + ddlCollege1.SelectedValue + "";
            int delete = dacces2.update_method_wo_parameter(deleteQ, "Text");
            if (delete == 1)
            {
                btnnew_Click(sender, e);
                imgAlert.Visible = true;
                lbl_alert.Text = "delete Sucessfully";
                loadlibrary();
                btnMainGo_Click(sender, e);

            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void btnnew_Click(object sender, EventArgs e)
    {
        try
        {
            btndelete.Enabled = false;
            Btnsave.Enabled = true;
            Btnupdate.Enabled = false;
            txtname.Text = Txtlibrarian.Text = Txtlocation.Text = txtacronym1.Text = txtstartnum1.Text = txtacronym2.Text = txtstartnum2.Text = txtacronym3.Text = txtstartnum3.Text = txtacronym4.Text = txtstartnum4.Text = txtacronym5.Text = txtstartnum5.Text = txtacronym6.Text = txtstartnum6.Text = txtacronym7.Text = txtstartnum7.Text = txtacronym8.Text = txtstartnum8.Text = txtfixedduedate.Text = txtfinecalculationfrom.Text = txtfinecalculationto.Text = txtbookissueclosedatestudents.Text = txtbookissueclosedatestaff.Text = "";
            rblibrary.Checked = true;
            rboffice.Checked = cb_books.Checked = rbgeneral.Checked = lblacronym1.Visible = txtacronym1.Visible = lblstartnum1.Visible = txtstartnum1.Visible = rbcategorywist.Checked = lblacronym2.Visible = txtacronym2.Visible = lblstartnum2.Visible = txtstartnum2.Visible = cbbackvolume.Checked = lblacronym3.Visible = txtacronym3.Visible = lblstartnum3.Visible = txtstartnum3.Visible = cbperiodicalsmaster.Checked = lblacronym4.Visible = txtacronym4.Visible = lblstartnum4.Visible = txtstartnum4.Visible = cbnonbook.Checked = lblacronym5.Visible = txtacronym5.Visible = lblstartnum5.Visible = txtstartnum5.Visible = cbstandardmaster.Checked = lblacronym6.Visible = txtacronym6.Visible = lblstartnum6.Visible = txtstartnum6.Visible = cbproceedings.Checked = lblacronym7.Visible = txtacronym7.Visible = lblstartnum7.Visible = txtstartnum7.Visible = lblacronym8.Visible = txtacronym8.Visible = lblstartnum8.Visible = txtstartnum8.Visible = cbbookbank.Checked = cbfixedduedate.Checked = txtfixedduedate.Enabled = cbfinecalculation.Checked = txtfinecalculationfrom.Enabled = txtfinecalculationto.Enabled = cbbookissueclosedatestudents.Checked = txtbookissueclosedatestudents.Enabled = cbbookissueclosedatestaff.Checked = txtbookissueclosedatestaff.Enabled = cballstudents.Checked = cballowallcollegestudandstaff.Checked = cbprojectmaster.Checked = false;
            ddlcategoryofbook.Text = "All";
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void btn_alertclose_Click(object sender, EventArgs e)
    {
        try
        {
            imgAlert.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void btn_yes_Click(object sender, EventArgs e)
    {
        try
        {
            yesorno = 1;
            btn_alertclose.Visible = true;
            btn_yes.Visible = false;
            btn_No.Visible = false;
            imgAlert.Visible = false;
            Btnsave_Click(sender, e);

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void btn_No_Click(object sender, EventArgs e)
    {
        try
        {
            yesorno = 0;
            btn_alertclose.Visible = true;
            btn_yes.Visible = false;
            btn_No.Visible = false;
            imgAlert.Visible = false;
            return;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void btn_exit_Click(object sender, EventArgs e)
    {
        try
        {
            divTarvellerEntryDetails.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void btn_excel_Click(object sender, EventArgs e)
    {

        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                lbprint.Visible = false;
                dacces2.printexcelreportgrid(GrdLibMaster, reportname);
            }
            else
            {
                txtexcelname.Focus();
                lbprint.Text = "Please Enter Your Report Name";
                lbprint.Visible = true;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            lbprint.Visible = false;
            string degreedetails = "Library Master";
            string pagename = "Library_Master.aspx";
            // Session["column_header_row_count"] = Fpload.ColumnHeader.RowCount;
            string ss = Session["usercode"].ToString();
            Printcontrolhed2.loadspreaddetails(GrdLibMaster, pagename, degreedetails, 0, ss);
            Printcontrolhed2.Visible = true;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }

    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        StringBuilder selectedvalue = new StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append("'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                    else
                    {
                        selectedvalue.Append(",'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
        return selectedvalue.ToString();
    }

    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        StringBuilder selectedText = new StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append("'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                    else
                    {
                        selectedText.Append(",'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
        return selectedText.ToString();
    }

    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = string.Empty;
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
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = string.Empty;
            cb.Checked = false;
            txt.Text = deft;
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
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    private void checkBoxListselectOrDeselect(CheckBoxList cbl, bool selected = true)
    {
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                li.Selected = selected;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    private bool getSelectedCheckBoxListCount(CheckBoxList cbl, out int selectedCount)
    {
        selectedCount = 0;
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                if (li.Selected)
                {
                    selectedCount++;
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }

    }

    /// <summary>
    /// Developed By Malang Raja T
    /// </summary>
    /// <param name="c">Only Data Bound Controls eg.DropDownList,RadioButtonList,CheckBoxList </param>
    /// <param name="selectedValue"></param>
    /// <param name="selectedText"></param>
    /// <param name="type">0 - Index; 1 - Text; 2 - Value;</param>
    private void SelectDataBound(Control c, string selectedValue, string selectedText)
    {
        try
        {
            bool isDataBoundControl = false;
            if (c is DataBoundControl)
            {
                if (c is CheckBoxList || c is DropDownList || c is RadioButtonList)
                {
                    isDataBoundControl = true;
                }
                if (isDataBoundControl)
                {
                    ListControl lstControls = (ListControl)c;
                    if (lstControls.Items.Count > 0)
                    {
                        System.Web.UI.WebControls.ListItem[] listItem = new System.Web.UI.WebControls.ListItem[lstControls.Items.Count];
                        lstControls.Items.CopyTo(listItem, 0);
                        if (listItem.Contains(new System.Web.UI.WebControls.ListItem(selectedText, selectedValue)))
                        {
                            lstControls.SelectedValue = selectedValue;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {

            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    #endregion
}