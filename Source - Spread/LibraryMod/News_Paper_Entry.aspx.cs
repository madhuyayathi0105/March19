using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using InsproDataAccess;
using System.Drawing;
using System.Globalization;
using System.Configuration;

public partial class LibraryMod_News_Paper_Entry : System.Web.UI.Page
{

    #region Field_Declaration
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    Dictionary<string, string> dicStaffList = new Dictionary<string, string>();
    Dictionary<string, string> dicSQLParameter = new Dictionary<string, string>();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    Dictionary<string, string> dictitle = new Dictionary<string, string>();
    DAccess2 d2 = new DAccess2();
    ReuasableMethods rs = new ReuasableMethods();
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    DataSet ds = new DataSet();
    string lib_code = "";
    string SuYear = "";
    string suptype = "";
    string suppname = "";
    string lang = "";
    string Journal = "";
    DateTime fdate = new DateTime();
    DateTime tdate = new DateTime();
    string fromdate = "";
    string todate = "";
    string actrow = "";
    string actcol = "";
    int arow = 0;
    int acol = 0;
    string AutoAccessNo = "";
    string langvalue = "";
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
           
            if (!IsPostBack)
            {
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                Library();
                subyear();
                loadSuppliertype();
                loadSuppliername();
                loadlanguage();
                loadJournalname();
                FpSpread1.Visible = false;
                btn_Save.Visible = false;
                lbl_total_journal.Visible = false;
                lbl_total_journal.Text = "";

                //ddl_year.Items.Clear();

                //for (int year = 0; year < 10; year++)
                //{
                //    string targetYear = String.Format("{0}", DateTime.Now.Year - year);
                //    ddl_year.Items.Add(new ListItem(targetYear, year.ToString()));
                //}
            }
        }
        catch (Exception ex) { }
    }

    #region Library
    public void Library()
    {
        try
        {
            ddllibrary.Items.Clear();
            ds.Clear();

            string SelectQ = string.Empty;
            if (!string.IsNullOrEmpty(userCollegeCode))
            {
                dicQueryParameter.Clear();
                dicQueryParameter.Add("CollegeCode", Convert.ToString(userCollegeCode));
                ds = storeAcc.selectDataSet("[GetLibrary]", dicQueryParameter);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddllibrary.DataSource = ds;
                    ddllibrary.DataTextField = "lib_name";
                    ddllibrary.DataValueField = "lib_code";
                    ddllibrary.DataBind();
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "News_Paper_Entry"); }

    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            subyear();
            FpSpread1.Visible = false;
            btn_Save.Visible = false;
            lbl_total_journal.Visible = false;
            lbl_total_journal.Text = "";
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "News_Paper_Entry"); }

    }
    #endregion

    #region Sub.Year
    public void subyear()
    {
        try
        {
            ddl_year.Items.Clear();
            ds = d2.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddl_year.DataSource = ds;
                ddl_year.DataTextField = "batch_year";
                ddl_year.DataValueField = "batch_year";
                ddl_year.DataBind();
            }
            int count1 = ds.Tables[1].Rows.Count;
            if (count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                ddl_year.SelectedValue = max_bat.ToString();
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "News_Paper_Entry"); }

    }

    protected void ddl_year_SelectedIndexChanged(object sender, EventArgs e)
    {

        FpSpread1.Visible = false;
        btn_Save.Visible = false;
        lbl_total_journal.Visible = false;
        lbl_total_journal.Text = "";
    }
    #endregion

    #region FromAndToDate
    protected void txt_fromdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_fromdate.Text != "" && txt_todate.Text != "")
            {

                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_todate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;

                if (dt > dt1)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Enter FromDate less than or equal to the ToDate";
                    txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                }
                else
                {

                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "News_Paper_Entry"); }

        // PopupMessage("Enter FromDate less than or equal to the ToDate", cv_fromtodt1);
    }
    protected void txt_todate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_fromdate.Text != "" && txt_todate.Text != "")
            {

                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_todate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;
                if (dt > dt1)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Enter ToDate greater than or equal to the FromDate ";
                    txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                }
                else
                {

                }

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "News_Paper_Entry"); }


        // PopupMessage("Enter ToDate greater than or equal to the FromDate", cv_fromtodt2);
    }
    #endregion

    #region SupplierType

    public void loadSuppliertype()
    {
        try
        {
            chklsuptype.Items.Clear();
            string sup = "SELECT DISTINCT ISNULL(SupplierType,'') SupplierType FROM CO_VendorMaster WHERE 1=1  AND LibraryFlag='1' ORDER BY SupplierType ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(sup, "Text");
            if (ds.Tables.Count > 0)
            {
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    chklsuptype.DataSource = ds;
                    chklsuptype.DataTextField = "SupplierType";
                    chklsuptype.DataValueField = "SupplierType";
                    chklsuptype.DataBind();
                }
                for (int i = 0; i < chklsuptype.Items.Count; i++)
                {
                    chklsuptype.Items[i].Selected = true;

                }
                txtsuptype.Text = lbl_suptype.Text + "(" + chklsuptype.Items.Count + ")";
                chksuptype.Checked = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "News_Paper_Entry"); }

    }

    protected void chksuptype_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckboxChange(chksuptype, chklsuptype, txtsuptype, "SupplierType", "--Select--");
        loadSuppliername();
        loadlanguage();
        loadJournalname();
    }

    protected void chklsuptype_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckboxListChange(chksuptype, chklsuptype, txtsuptype, "SupplierType", "--Select--");
        loadSuppliername();
        loadlanguage();
        loadJournalname();
    }

    #endregion

    #region Suppliername

    public void loadSuppliername()
    {
        try
        {            
            string StrSupplierType = "";           
            for (int i = 0; chklsuptype.Items.Count > i; i++)
            {
                if (chklsuptype.Items[i].Selected == true)
                {
                    if (StrSupplierType == "")
                        StrSupplierType = "'" + chklsuptype.Items[i].Text + "'";
                    else
                        StrSupplierType = StrSupplierType + ",'" + chklsuptype.Items[i].Text + "'";
                }
            }
            string supname = "";
            supname = "SELECT VendorCompName FROM CO_VendorMaster WHERE 1=1  AND LibraryFlag='1' ";
            if (StrSupplierType != "")
            {
                supname = supname + " AND SupplierType IN (" + StrSupplierType + ")  ORDER BY VendorCompName";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(supname, "Text");

            if (ds.Tables.Count > 0)
            {
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    chklsupname.DataSource = ds;
                    chklsupname.DataTextField = "VendorCompName";
                    chklsupname.DataValueField = "VendorCompName";
                    chklsupname.DataBind();
                }
                for (int i = 0; i < chklsupname.Items.Count; i++)
                {
                    chklsupname.Items[i].Selected = true;

                }
                txtsupname.Text = lblsupname.Text + "(" + chklsupname.Items.Count + ")";
                chksupname.Checked = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "News_Paper_Entry"); }

    }

    protected void chksupname_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckboxChange(chksupname, chklsupname, txtsupname, "Suppliername", "--Select--");
        loadlanguage();
        loadJournalname();
    }

    protected void chklsupname_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckboxListChange(chksupname, chklsupname, txtsupname, "Suppliername", "--Select--");
        loadlanguage();
        loadJournalname();
    }

    #endregion

    #region Language

    public void loadlanguage()
    {
        try
        {
            chkllang.Items.Clear();
            chkllang.Items.Insert(0, "English");
            chkllang.Items.Insert(1, "Tamil");
            if (chkllang.Items.Count > 0)
            {
                for (int i = 0; i < chkllang.Items.Count; i++)
                {
                    chkllang.Items[i].Selected = true;

                }
                txtlang.Text = lblang.Text + "(" + chkllang.Items.Count + ")";
                chklang.Checked = true;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "News_Paper_Entry"); }

    }

    protected void chklang_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckboxChange(chklang, chkllang, txtlang, "Language", "--Select--");
        loadJournalname();
    }

    protected void chkllang_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckboxListChange(chklang, chkllang, txtlang, "Language", "--Select--");
        loadJournalname();
    }
    #endregion

    #region JouName

    public void loadJournalname()
    {
        try
        {
            chkljname.Items.Clear();
            for (int i = 0; chklsuptype.Items.Count > i; i++)
            {
                if (chklsuptype.Items[i].Selected == true)
                {
                    if (suptype == "")
                        suptype = "'" + chklsuptype.Items[i].Text + "'";
                    else
                        suptype = suptype + ",'" + chklsuptype.Items[i].Text + "'";
                }
            }
            for (int i = 0; chklsupname.Items.Count > i; i++)
            {
                if (chklsupname.Items[i].Selected == true)
                {
                    if (suppname == "")
                        suppname = "'" + chklsupname.Items[i].Text + "'";
                    else
                        suppname = suppname + ",'" + chklsupname.Items[i].Text + "'";
                }
            }
            
            if (chkllang.Items.Count > 0)
                lang = Convert.ToString(d2.getCblSelectedValue(chkllang));
            if (lang == "English")
                langvalue = "0";
            else if (lang == "Tamil")
                langvalue = "1";
            else
                langvalue = "0','1";
           
            string journame = "";
            if (suptype != "" && suppname != "" && lang != "")
            {
                string selectQuery = "SELECT distinct Journal_Name FROM Journal_Master J,Library L,CO_VendorMaster S   WHERE J.Lib_Code = L.Lib_Code AND J.Supplier = S.VendorCompName ";
                selectQuery = selectQuery + " AND L.College_Code =" + userCollegeCode;
                if (suptype.Trim() != "")
                    selectQuery = selectQuery + " AND SupplierType IN (" + suptype + ")";

                if (suppname.Trim() != "")
                    selectQuery = selectQuery + " AND VendorCompName IN (" + suppname + ")";
                if (langvalue.Trim() != "")
                    selectQuery = selectQuery + " AND ISNULL(TitleLanguage,0) IN ('" + langvalue + "')";
                selectQuery = selectQuery + " and LibraryFlag='1' ORDER BY Journal_Name ";               
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectQuery, "Text");
            }
            if (ds.Tables.Count > 0)
            {
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    chkljname.DataSource = ds;
                    chkljname.DataTextField = "Journal_Name";
                    chkljname.DataValueField = "Journal_Name";
                    chkljname.DataBind();
                }
                for (int i = 0; i < chkljname.Items.Count; i++)
                {
                    chkljname.Items[i].Selected = true;

                }
                Txt_jname.Text = lbl_jname.Text + "(" + chkljname.Items.Count + ")";
                chkjname.Checked = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "News_Paper_Entry"); }

    }

    protected void chkjname_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckboxChange(chkjname, chkljname, Txt_jname, "JournalName", "--Select--");
    }

    protected void chkljname_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckboxListChange(chkjname, chkljname, Txt_jname, "JournalName", "--Select--");
    }

    #endregion

    #region Go
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {

            ds = getnewsDetails();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                newsloadspread(ds);
            }
            else
            {

                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Records Found";

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "News_Paper_Entry"); }


    }
    #endregion

    #region Fspread
    private DataSet getnewsDetails()
    {

        DataSet dsload1 = new DataSet();
        try
        {
            #region get Value

            string selQ = string.Empty;
            string StrSupType = string.Empty;
            string StrSupName = string.Empty;
            string StrJrnlName = string.Empty;
            if (ddl_year.Items.Count > 0)
                SuYear = Convert.ToString(ddl_year.SelectedValue);
            if (chklsuptype.Items.Count > 0)
                suptype = Convert.ToString(d2.getCblSelectedValue(chklsuptype));
            if (suptype != "")
                StrSupType = " AND SupplierType IN ('" + suptype + "')";
            if (chklsupname.Items.Count > 0)
                suppname = Convert.ToString(d2.getCblSelectedValue(chklsupname));
            if (suppname != "")
                StrSupName = " AND Supplier IN ('" + suppname + "')";
            if (chkllang.Items.Count > 0)
                lang = Convert.ToString(d2.getCblSelectedValue(chkllang));
            if (chkljname.Items.Count > 0)
                Journal = Convert.ToString(d2.getCblSelectedValue(chkljname));
            if (Journal != "")
                StrJrnlName = "  AND Journal_Name IN ('" + Journal + "')";

            string fromdate = Convert.ToString(txt_fromdate.Text);
            if (fromdate.Trim() != "")
            {
                string[] split = fromdate.Split('/');
                fdate = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            }

            string todate = Convert.ToString(txt_todate.Text);
            if (todate.Trim() != "")
            {
                string[] split = todate.Split('/');
                tdate = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            }

            if (!string.IsNullOrEmpty(userCollegeCode) && !string.IsNullOrEmpty(SuYear))
            {
                selQ = "SELECT DISTINCT S.Journal_Code,Journal_Name,ISNULL(TitleLanguage,0) TitleLanguage,Subs_Year FROM Journal_Master M  INNER JOIN Journal_Issues S ON M.Journal_Code = S.Journal_Code  LEFT JOIN Supplier_Details U ON M.Supplier = U.Supplier_Name   WHERE ISNULL(PeriodicalType,1) = 2  AND Subs_Year ='" + SuYear + "' AND IssueDate BETWEEN '" + fdate + "' AND '" + tdate + "' " + StrSupType + StrSupName + StrJrnlName + " ORDER BY Journal_Name";
            }
            dsload1.Clear();
            dsload1 = d2.select_method_wo_parameter(selQ, "Text");
            #endregion
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "News_Paper_Entry"); }


        return dsload1;
    }

    public void newsloadspread(DataSet dsnews)
    {
        try
        {
            FpSpread1.SaveChanges();
            if (dsnews.Tables.Count > 0 && dsnews.Tables[0].Rows.Count > 0)
            {
                FpSpread1.Sheets[0].RowCount = 1;
                FpSpread1.CommandBar.Visible = false;
                FpSpread1.Sheets[0].AutoPostBack = false;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread1.Sheets[0].ColumnHeader.Columns.Count = 3;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Columns[0].Width = 50;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Journal Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Columns[1].Width = 150;
                FpSpread1.Columns[1].Visible = true;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Journal Code";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Columns[2].Width = 150;
                FpSpread1.Columns[2].Visible = false;

                FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
                chkcell1.AutoPostBack = true;
                FarPoint.Web.Spread.CheckBoxCellType chkcell2 = new FarPoint.Web.Spread.CheckBoxCellType();
                chkcell2.AutoPostBack = false;

                int sno = 0;
                DateTime dateFromDate = new DateTime();
                DateTime dateToDate = new DateTime();
                dateFromDate = getdate(txt_fromdate.Text.ToString());
                dateToDate = getdate(txt_todate.Text.ToString());
                string coldate = "";
                int col = 0;
                while (dateFromDate <= dateToDate)
                {
                    coldate = dateFromDate.ToString("dd/MM/yyyy");
                    col = FpSpread1.Sheets[0].ColumnHeader.Columns.Count++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Text = coldate;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Columns[col].Width = 90;
                    FpSpread1.Columns[col].Visible = true;
                    FpSpread1.Sheets[0].Cells[0, col].CellType = chkcell1;
                    FpSpread1.Sheets[0].Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                    dateFromDate = dateFromDate.AddDays(1);
                    col++;
                }
                int nojoural = dsnews.Tables[0].Rows.Count;
                for (int row = 0; row < dsnews.Tables[0].Rows.Count; row++)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    sno++;
                    dateFromDate = getdate(txt_fromdate.Text.ToString());
                    dateToDate = getdate(txt_todate.Text.ToString());
                    string lan = Convert.ToString(dsnews.Tables[0].Rows[row]["TitleLanguage"]).Trim();
                    string jname = Convert.ToString(dsnews.Tables[0].Rows[row]["Journal_Name"]).Trim();
                    string jcode = Convert.ToString(dsnews.Tables[0].Rows[row]["Journal_Code"]).Trim();
                    string yr = Convert.ToString(dsnews.Tables[0].Rows[row]["Subs_Year"]);

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txtCell;


                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = jcode;

                    if (lan == "1")
                    {

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = jname;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Amudham";

                    }
                    else
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = jname;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";

                    }
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = jname;

                    col = 3;
                    while (dateFromDate <= dateToDate)
                    {
                        coldate = dateFromDate.ToString("yyyy/MM/dd");
                        string Sqlqry = d2.GetFunction("SELECT Issue_Status FROM Journal_Issues WHERE Journal_Code ='" + jcode + "' AND Subs_Year='" + yr + "' AND IssueDate ='" + coldate + "'");
                        if (Sqlqry == "1")
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Tag = Sqlqry;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = coldate;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = chkcell2;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Value = 1;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = Color.Green;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;

                        }
                        else
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Tag = Sqlqry;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = coldate;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = chkcell2;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Value = 0;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = Color.Red;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;

                        }
                        dateFromDate = dateFromDate.AddDays(1);
                        col++;
                    }

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Locked = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Locked = true;



                }
                FpSpread1.Sheets[0].PageSize = 100;
                FpSpread1.Sheets[0].CurrentPageIndex = 0;
                FpSpread1.SaveChanges();
                //FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.Width = 1000;
                FpSpread1.Height = 400;
                FpSpread1.Visible = true;
                btn_Save.Visible = true;
                lbl_total_journal.Visible = true;
                lbl_total_journal.Text = "No. Of Journals:" + nojoural;



            }

        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "News_Paper_Entry"); }


    }

    protected void FpSpread1_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        //Fpspread2.Visible = true;
        try
        {
            actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            arow = Convert.ToInt32(actrow);
            acol = Convert.ToInt32(actcol);
            if (actrow.Trim() != "" && actrow.Trim() == "0")
            {
                if (FpSpread1.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[arow, acol].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, acol].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, acol].Value = 0;
                        }
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "News_Paper_Entry"); }

    }

    #endregion

    #region Save
    protected void btn_Save_Click(object sender, EventArgs e)
    {
        try
        {

            if (ddllibrary.Items.Count > 0)
                lib_code = Convert.ToString(ddllibrary.SelectedValue);
            if (ddl_year.Items.Count > 0)
                SuYear = Convert.ToString(ddl_year.SelectedValue);
            actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            arow = Convert.ToInt32(actrow);
            acol = Convert.ToInt32(actcol);
            string varStatus = "";
            string strStatus = "";
            string VarJrnCode = "";
            string StrActDate = "";
            string StrActDate1 = "";
            string strAccno1 = "";
            string sqlinsert = "";
            string VarJrnName = "";
            string StrIssueNo = "";
            string StrMonthIssueNo = "";
            int insert = 0;
            int update = 0;
            bool chk = false;
            string Currentdate = DateTime.Now.ToString("MM/dd/yyyy");
            string Acctime = DateTime.Now.ToString("hh:mm tt");
            if (actrow.Trim() != "")
            {
                if (FpSpread1.Rows.Count > 0)
                {
                    FpSpread1.SaveChanges();

                    for (int col = 3; col < FpSpread1.Sheets[0].ColumnCount; col++)
                    {

                        for (int row = 1; row < FpSpread1.Sheets[0].RowCount; row++)
                        {
                            VarJrnCode = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 0].Tag);
                            StrActDate1 = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Tag);
                            if (StrActDate1 != "")
                            {
                                string[] actdate = StrActDate1.Split('/');
                                StrActDate = actdate[1] + "/" + actdate[2] + "/" + actdate[0];
                            }
                            //varStatus = Convert.ToString(FpSpread1.Sheets[0].Cells[row, acol].Tag);
                            VarJrnName = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 2].Tag);
                            int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, col].Value);
                            if (checkval == 1)
                                strStatus = "1";

                            else
                                strStatus = "0";
                            StrIssueNo = d2.GetFunction("SELECT IssueNo FROM Journal_Issues WHERE Journal_Code ='" + VarJrnCode + "' AND Subs_Year ='" + SuYear + "' AND IssueDate ='" + StrActDate + "'");
                            StrMonthIssueNo = d2.GetFunction("SELECT MonthIssue_No FROM Journal_Issues WHERE Journal_Code ='" + VarJrnCode + "' AND Subs_Year ='" + SuYear + "' AND IssueDate ='" + StrActDate + "'");
                            int IssueNo = Convert.ToInt32(StrIssueNo);
                            int MonthIssueNo = Convert.ToInt32(StrMonthIssueNo);
                            if (strStatus == "1")
                            {
                                strAccno1 = AutoAccessNo1();
                                if (strAccno1 != "")
                                {
                                    sqlinsert = "INSERT INTO Journal(access_date,access_time,access_code,journal_code,title,dept_name,volume_no,issue_no,received_date,issue_date,noofcopies,remarks,bind_flag,attachement,back_flag,lib_code,issue_flag,receive_date,issn,contents,newaccno,supplier,invoice_no,address,pay_type,expiry_date,Pages,Price,Volume,S_Term,Budget_Head,Subs_Year,Issue_Year,Issue_Month,MonthIssue_No,ActIssueNo)VALUES('" + Currentdate + "','" + Acctime + "','" + strAccno1 + "','" + VarJrnCode + "','" + VarJrnName + "','','','" + StrIssueNo + "','" + Currentdate + "','" + Acctime + "','1','','No','Nil','No','" + lib_code + "','Available','" + StrActDate + "','','','','','','','','" + StrActDate + "','','','','','','" + SuYear + "','" + SuYear + "','" + StrActDate + "','" + MonthIssueNo + "','" + IssueNo + "')";
                                    sqlinsert += "UPDATE Journal_Issues SET Issue_Status ='" + strStatus + "' where journal_code ='" + VarJrnCode + "' and subs_year ='" + SuYear + "' AND IssueDate ='" + StrActDate + "'";
                                    insert = d2.update_method_wo_parameter(sqlinsert, "Text");
                                }
                            }
                            else
                            {
                                //strStatus = "1";
                                sqlinsert = "DELETE FROM Journal WHERE Journal_Code ='" + VarJrnCode + "' AND Subs_Year ='" + SuYear + "' AND Issue_No =" + IssueNo + " AND Issue_Month ='" + StrActDate + "'";
                                sqlinsert = "UPDATE Journal_Issues SET Issue_Status ='" + strStatus + "' where journal_code ='" + VarJrnCode + "' and subs_year ='" + SuYear + "' AND IssueDate ='" + StrActDate + "' ";
                                insert = d2.update_method_wo_parameter(sqlinsert, "Text");

                            }
                        }



                        //else
                        //{
                        //    for (int row = 0; row < FpSpread1.Sheets[0].RowCount; row++)
                        //    {
                        //        int checkval1 = Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, acol].Value);
                        //        if (checkval1 == 1)
                        //        {
                        //            chk = true;
                        //            VarJrnCode = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 0].Tag);
                        //            StrActDate1 = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Tag);
                        //            if (StrActDate1 != "")
                        //            {
                        //                string[] actdate = StrActDate1.Split('/');
                        //                StrActDate = actdate[2] + "-" + actdate[1] + "-" + actdate[0];
                        //            }
                        //            varStatus = Convert.ToString(FpSpread1.Sheets[0].Cells[row, acol].Tag);
                        //            VarJrnName = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 2].Tag);
                        //            if (varStatus == "1")
                        //                strStatus = "1";
                        //            else
                        //                strStatus = "0";
                        //            StrIssueNo = d2.GetFunction("SELECT IssueNo FROM Journal_Issues WHERE Journal_Code ='" + VarJrnCode + "' AND Subs_Year ='" + SuYear + "' AND IssueDate ='" + StrActDate + "'");
                        //            StrMonthIssueNo = d2.GetFunction("SELECT MonthIssue_No FROM Journal_Issues WHERE Journal_Code ='" + VarJrnCode + "' AND Subs_Year ='" + SuYear + "' AND IssueDate ='" + StrActDate + "'");
                        //            int IssueNo = Convert.ToInt32(StrIssueNo);
                        //            int MonthIssueNo = Convert.ToInt32(StrMonthIssueNo);
                        //            if (strStatus == "1")
                        //            {
                        //                strAccno1 = AutoAccessNo1();
                        //                if (strAccno1 != "")
                        //                {
                        //                    sqlinsert = "INSERT INTO Journal(access_date,access_time,access_code,journal_code,title,dept_name,volume_no,issue_no,received_date,issue_date,noofcopies,remarks,bind_flag,attachement,back_flag,lib_code,issue_flag,receive_date,issn,contents,newaccno,supplier,invoice_no,address,pay_type,expiry_date,Pages,Price,Volume,S_Term,Budget_Head,Subs_Year,Issue_Year,Issue_Month,MonthIssue_No,ActIssueNo)                     VALUES('" + Currentdate + "','" + Acctime + "','" + strAccno1 + "','" + VarJrnCode + "','" + VarJrnName + "','','','" + StrIssueNo + "','" + Currentdate + "','" + Acctime + "','1','','No','Nil','No','" + lib_code + "','Available','" + StrActDate + "','','','','','','','','" + StrActDate + "','','','','','','" + SuYear + "','" + SuYear + "','" + StrActDate + "','" + MonthIssueNo + "','" + IssueNo + "')";
                        //                    sqlinsert += "UPDATE Journal_Issues SET Issue_Status ='" + strStatus + "' where journal_code ='" + VarJrnCode + "' and subs_year ='" + SuYear + "' AND IssueDate ='" + StrActDate + "'";
                        //                    insert = d2.update_method_wo_parameter(sqlinsert, "Text");
                        //                }

                        //            }
                        //            else
                        //            {

                        //                sqlinsert = "DELETE FROM Journal WHERE Journal_Code ='" + VarJrnCode + "' AND Subs_Year ='" + SuYear + "' AND Issue_No =" + IssueNo + " AND Issue_Month ='" + StrActDate + "'";
                        //                sqlinsert = "UPDATE Journal_Issues SET Issue_Status ='" + strStatus + "' where journal_code ='" + VarJrnCode + "' and subs_year ='" + SuYear + "' AND IssueDate ='" + StrActDate + "' ";
                        //                insert = d2.update_method_wo_parameter(sqlinsert, "Text");

                        //            }
                        //        }
                        //    }

                        //}
                    }
                }
            }
            //if (!chk)
            //{
            //    alertpopwindow.Visible = true;
            //    lblalerterr.Text = "Please Select Atleast one Record";

            //}
            if (insert > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Saved Sucessfully";
                btngo_Click(sender, e);
            }
        }
        //catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "News_Paper_Entry"); }
        catch (Exception ex) { }

    }
    #endregion

    #region commonFunction

    private DateTime getdate(string getspl)
    {
        DateTime date = new DateTime();
        try
        {
            date = DateTime.ParseExact(getspl, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return date;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "News_Paper_Entry"); }

        return new DateTime();
    }

    private string AutoAccessNo1()
    {

        try
        {
            string strAccno = "";
            int IntAccNo = 0;

            DataSet dsaccno = new DataSet();
            if (ddllibrary.Items.Count > 0)
                lib_code = Convert.ToString(ddllibrary.SelectedValue);
            if (lib_code != "")
            {
                string SqlAcc = "SELECT * FROM Journal WHERE Lib_Code='" + lib_code + "' ORDER BY access_code Desc,LEN(access_code)";
                dsaccno.Clear();
                dsaccno = d2.select_method_wo_parameter(SqlAcc, "Text");
                if (dsaccno.Tables[0].Rows.Count > 0)
                {
                    strAccno = Convert.ToString(dsaccno.Tables[0].Rows[0]["access_code"]);
                    if (strAccno != "")
                    {
                        string staccno = strAccno.Remove(0, 3);
                        IntAccNo = Convert.ToInt32(staccno) + 1;
                        AutoAccessNo = "PER" + Convert.ToString(IntAccNo);

                    }
                }
                else
                {
                    AutoAccessNo = "PER1";
                }

            }
        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "News_Paper_Entry"); }


        return AutoAccessNo;

    }
    #endregion

    #region Close

    protected void btnerrclose_Click(object sender, EventArgs e)
    {

        alertpopwindow.Visible = false;
    }

    #endregion
}