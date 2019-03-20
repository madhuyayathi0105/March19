using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using InsproDataAccess;
using System.Drawing;
using System.Collections;
using System.Globalization;
using System.Windows.Forms;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Configuration;


public partial class LibraryMod_bookissue : System.Web.UI.Page
{
    #region Variables

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet dsload = new DataSet();
    DataSet dsCommon = new DataSet();
    DataSet rsreserve = new DataSet();
    DataSet rsBookInHand = new DataSet();
    DataSet rsCalFine = new DataSet();
    DataSet rsCode = new DataSet();
    DataSet dsDispCard = new DataSet();
    DataSet rsLib = new DataSet();
    DataSet rsRenew = new DataSet();
    DataSet dsHoliday = new DataSet();
    DataSet securset = new DataSet();
    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    FarPoint.Web.Spread.TextCellType cellText = new FarPoint.Web.Spread.TextCellType();
    DataTable dtCommon = new DataTable();
    DataSet dsprint = new DataSet();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string collcode = string.Empty;
    string libcode = string.Empty;
    string libname = string.Empty;
    string activerow = "";
    string activecol = "";
    static string degree_codeVar = "";
    static string intStudCollCode = "";
    static string StrSaveRollNo = "";
    static string StrSaveLibID = "";
    static string batch_year = "";
    static int selectedcount = 0;
    static int intIsHoliday = 0;
    static int IntDueDatExcHol = 0;
    static int IntDispMess = 0;
    static int IntDispIssueMes = 0;
    static int IntDispFineMess = 0;
    static int intRenCount = 0;
    static int intRenDays = 0;
    static int nocal = 0;
    static bool SureYes = false;
    static bool SureYestToIssueBook = false;
    static bool SureYesIssue = false;
    int update = 0;
    string a = "";
    int intdegcode = 0;
    int intDegree = 0;
    string msg = "";
    static string issue_type = "";
    static bool BlnExcHoliday = false;
    static bool BlnLibHol = false;
    static bool BlnAllowMulColStud = false;
    static bool BlnAllowTrans = false;
    static bool Is_RollValid = false;
    static bool blncomm = false;
    static bool blnLock = false;
    static bool BlnBookBankLib = false;
    static bool BlnBookBankAll = false;
    static bool ISReffBook = false;
    static bool BlnRef = false;
    static bool BlnMulRenewDays = false;
    static bool IntTransType = false;
    static bool IntCancelFine = false;
    static bool Isspecial = false;
    static bool SpecialFine = false;
    static bool SpecialReturn = false;
    static bool BlnReserveDue = false;
    static bool check_bookInfo = false;
    static bool checkflag = false;
    static bool boolvar = false;
    static bool IsSelectedVal = false;
    static bool IsSamTitle = false;
    static string bodate = "";
    static string strtitle = "";
    static string strAuthor = "";
    static string deg = "";
    string sq = "";
    string VarF_IssueDate = "";
    string VarF_AccNo = "";
    string VarF_Days = "";
    string VarF_DueDate = "";
    string varAccno = "";
    string vartitle = "";
    string varauthor = "";
    string varCallNo = "";
    string varcTokenNo = "";
    string varRDueDay = "";
    string VarRDueDate = "";
    static string VarborrowNew = "";
    string StrDueDate = "";
    string Var_Fine = "";
    string StrMemberType = "";
    string varToken = "";
    string borrowdate = "";
    string varDueDate = "";
    double fine1 = 0;
    int IntReserveDueType = 0;
    int IntReserveDueVal = 0;
    int sno = 0;
    string varIssueDate = "";
    bool CellClick = false;
    Boolean pageno = false;
    int ivalue = 0;
    int curpage = 0;
    double pageSize1 = 0.0;
    int pagecnt = 0;
    int pgsize = 0;
    int s = 0;
    static bool firstRow = false;
    bool Remove = false;
    static string LibNameDefault = "";
    static int chosedmode = 0;
    static int personmode = 0;
    static string collegecode1 = "";
    static string usercodestat = string.Empty;
    static DAccess2 d22 = new DAccess2();
    static int AccBookType = 0;
    static string Acclibcode = string.Empty;
    static int SearchByBook = 0;
    static int SearchByBookSpecific = 0;
    bool CheckCount = true;
    static string AccNoCheck = "";
    DataTable dtIssuingBook = new DataTable();
    DataRow drow1;
    DataTable dtBooksInHand = new DataTable();
    static bool RefBook = false;
    static string RollNoChallanReceipt = "";
    static Hashtable hsAccNo = new Hashtable();
    static string StrAccNo = "";
    static Dictionary<int, string> dicRowColor = new Dictionary<int, string>();

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
            personmode = 0;
            chosedmode = 0;
            userCode = Session["usercode"].ToString();
            singleUser = Session["single_user"].ToString();
            groupUserCode = Session["group_code"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            usercodestat = userCode;
            img_stud1.ImageUrl = "";
            imgBook.ImageUrl = "";
            if (GrdIssuingBook.Rows.Count == 0)
            {
                dtIssuingBook.Columns.Add("Access No", typeof(string));
                dtIssuingBook.Columns.Add("Title", typeof(string));
                dtIssuingBook.Columns.Add("Author", typeof(string));
                dtIssuingBook.Columns.Add("Call No", typeof(string));
                dtIssuingBook.Columns.Add("Date Of Issue", typeof(string));
                dtIssuingBook.Columns.Add("Due Days", typeof(string));
                dtIssuingBook.Columns.Add("Due Date", typeof(string));
                dtIssuingBook.Columns.Add("Token No", typeof(string));
                dtIssuingBook.Columns.Add("Fine", typeof(string));
                GrdIssuingBook.DataSource = dtIssuingBook;
                GrdIssuingBook.DataBind();
                GrdIssuingBook.Visible = true;
            }
            if (!IsPostBack)
            {                
                Page.Form.DefaultFocus = txtRollNo.ClientID;
                if (ddlissue.SelectedItem.Text == "Book")
                {
                    AccBookType = 0;
                }
                StrSaveRollNo = "";
                hsAccNo.Clear();
                firstRow = false;
                Bindcollege();
                //bindLibrary();
                getLibPrivil();
                bindCategory();
                bindbatch();
                binddegree();
                bindbranch();
                bindsem();
                bindsec();
                SetLibSettings();
                reserv_details(StrSaveRollNo);
                LoadBooksHand(BlnBookBankLib, blncomm, BlnBookBankAll, StrSaveRollNo, StrSaveLibID);
                //filler(rsBookInHand, StrSaveRollNo);
                // rblissue_Selected(sender, e);
                ddllibrary_SelectedIndexChanged(sender, e);

                string issDate = d2.ServerDate();
                string[] dat = issDate.Split('/');
                if (dat.Length == 3)
                    issDate = dat[1] + '/' + dat[0] + '/' + dat[2];
                txtissuedate.Text = issDate.Split(' ')[0];
                Txtduedate.Text = issDate.Split(' ')[0];
            }
        }
        catch (Exception ex) { }
    }

    #region Binding Methods

    public void Bindcollege()
    {
        try
        {
            //ddl_library.Items.Clear();
            dtCommon.Clear();
            ddlcollege.Enabled = false;
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
                ddlcollege.DataSource = dtCommon;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
                ddlcollege.SelectedIndex = 0;
                ddlcollege.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, userCollegeCode, "LibraryRackAllocation");
        }
    }

    public void bindLibrary(string LibCode)
    {
        ds.Clear();
        string collegecode = Convert.ToString(ddlcollege.SelectedValue);
        string SelectQ = string.Empty;
        SelectQ = "select *,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + LibCode + " and college_code in('" + collegecode + "') ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
        ds = d2.select_method_wo_parameter(SelectQ, "text");
        int SelectVal = 0;
        int count = 0;
        if (ds.Tables[0].Rows.Count > 0)
        {
            Cbo_CardLibrary.DataSource = ds;
            Cbo_CardLibrary.DataTextField = "lib_name";
            Cbo_CardLibrary.DataValueField = "lib_code";
            Cbo_CardLibrary.DataBind();
            count = Cbo_CardLibrary.Items.Count;
            Cbo_CardLibrary.Items.Insert(count, "All");
            //SelectVal = count + 1;
            //Cbo_CardLibrary.SelectedValue = Convert.ToString(SelectVal);

            ddllibrary.DataSource = ds;
            ddllibrary.DataTextField = "lib_name";
            ddllibrary.DataValueField = "lib_code";
            ddllibrary.DataBind();

            ddlTraceLib.DataSource = ds;
            ddlTraceLib.DataTextField = "lib_name";
            ddlTraceLib.DataValueField = "lib_code";
            ddlTraceLib.DataBind();
        }
        SelectQ = "select setfor,l.lib_code,is_set from setdefault s,library l where s.college_code='" + collegecode + "' and s.setfor=l.lib_name";//descrip ='frmissue_return;" + Cbo_CardLibrary.SelectedItem.Text + "'
        dsload.Clear();
        dsload = d2.select_method_wo_parameter(SelectQ, "text");

        if (dsload.Tables[0].Rows.Count > 0)
        {
            string isSet = Convert.ToString(dsload.Tables[0].Rows[0]["is_set"]);
            string SetFor = Convert.ToString(dsload.Tables[0].Rows[0]["setfor"]);
            string libcode = Convert.ToString(dsload.Tables[0].Rows[0]["lib_code"]);
            if (isSet.ToLower() == "true" && SetFor != "")
            {
                Cbo_CardLibrary.SelectedValue = libcode;
                ddllibrary.SelectedValue = libcode;
            }
        }
        else
        {
            Cbo_CardLibrary.SelectedIndex = count;
        }
    }

    public void bindCategory()
    {
        ddlCardType.Items.Clear();
        ds.Clear();
        string collegecode = Convert.ToString(ddlcollege.SelectedValue);
        string SelectQ = string.Empty;

        SelectQ = "SELECT DISTINCT CardCat FROM Lib_Master M where ISNULL(CardCat,'') <> '' AND ISNULL(CardCat,'') <> 'All'";
        ds = d2.select_method_wo_parameter(SelectQ, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlCardType.DataSource = ds;
            ddlCardType.DataTextField = "CardCat";
            ddlCardType.DataValueField = "CardCat";
            ddlCardType.DataBind();
        }
        ddlCardType.Items.Insert(0, "All");
    }

    #region Master & Security Settings

    public void SetLibSettings()
    {
        try
        {
            string Sql = "";
            string StrRes = string.Empty;
            string LibCode = Convert.ToString(ddllibrary.SelectedValue);
            string CollegeCode = Convert.ToString(ddlcollege.SelectedValue);
            string LinkVal = string.Empty;

            Sql = "SELECT ISNULL(BarCodeTrans,0) BarCodeTrans,ISNULL(Cancel_Fine,0) Cancel_Fine, ISNULL(SP_Issue,0) SP_Issue,ISNULL(SP_Fine,0) SP_Fine,ISNULL(SP_Return,0) SP_Return FROM Lib_User_Perm WHERE User_Code =" + userCode + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(ds.Tables[0].Rows[0]["BarCodeTrans"]).ToLower() == "true")
                    IntTransType = true;
                else
                    IntTransType = false;
                if (Convert.ToString(ds.Tables[0].Rows[0]["Cancel_Fine"]).ToLower() == "true")
                    IntCancelFine = true;
                else
                    IntCancelFine = false;
                if (Convert.ToString(ds.Tables[0].Rows[0]["SP_Issue"]) == "1")
                    Isspecial = true;
                else
                    Isspecial = false;
                if (Convert.ToString(ds.Tables[0].Rows[0]["SP_Fine"]) == "1")
                    SpecialFine = true;
                else
                    SpecialFine = false;
                if (Convert.ToString(ds.Tables[0].Rows[0]["SP_Return"]) == "1")
                    SpecialReturn = true;
                else
                    SpecialReturn = false;
            }

            //**************Library Settings*************

            Sql = "SELECT ISNULL(ISBooks_DueDate,0) ISBooks_DueDate,ISNULL(Books_DueDate,'') Books_DueDate,ISNULL(AllowAllCollStud,0) AllowAllCollStud FROM Library WHERE Lib_Code ='" + LibCode + "' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(ds.Tables[0].Rows[0]["ISBooks_DueDate"]).ToLower() == "true")
                    Txtduedate.Text = Convert.ToString(ds.Tables[0].Rows[0]["Books_DueDate"]);
                else
                    Txtduedate.Text = txtissuedate.Text;

                if (Convert.ToString(ds.Tables[0].Rows[0]["AllowAllCollStud"]).ToLower() == "true")
                    BlnAllowMulColStud = true;
                else
                    BlnAllowMulColStud = false;
            }
            else
            {
                BlnAllowMulColStud = false;
            }

            //**************Security Settings*************

            Sql = " select * from inssettings where linkname='Due date exclude holidays' and college_code=" + CollegeCode + "";
            securset.Clear();
            securset = d2.select_method_wo_parameter(Sql, "text");
            if (securset.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(securset.Tables[0].Rows[0]["linkvalue"]) == "1")
                    IntDueDatExcHol = 1;
                else
                    IntDueDatExcHol = 0;
            }
            Sql = "select * from inssettings where linkname='Edit Issue Date' and college_code=" + CollegeCode + "";
            securset.Clear();
            securset = d2.select_method_wo_parameter(Sql, "text");
            if (securset.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(securset.Tables[0].Rows[0]["linkvalue"]) == "1")
                    txtissuedate.Enabled = true;
                else
                    txtissuedate.Enabled = false;
            }
            Sql = "select * from inssettings where linkname='Edit Due Date' and college_code=" + CollegeCode + "";
            securset.Clear();
            securset = d2.select_method_wo_parameter(Sql, "text");
            if (securset.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(securset.Tables[0].Rows[0]["linkvalue"]) == "1")
                    Txtduedate.Enabled = true;
                else
                    Txtduedate.Enabled = false;
            }
            Sql = "select * from inssettings where linkname='Display Return Message' and college_code=" + CollegeCode + "";
            securset.Clear();
            securset = d2.select_method_wo_parameter(Sql, "text");
            if (securset.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(securset.Tables[0].Rows[0]["linkvalue"]) == "1")
                    IntDispMess = 1;
                else
                    IntDispMess = 0;
            }
            Sql = "select * from inssettings where linkname='Display Issue Message' and college_code=" + CollegeCode + "";
            securset.Clear();
            securset = d2.select_method_wo_parameter(Sql, "text");
            if (securset.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(securset.Tables[0].Rows[0]["linkvalue"]) == "1")
                    IntDispIssueMes = 1;
                else
                    IntDispIssueMes = 0;
            }
            Sql = "select * from inssettings where linkname='Display Fine Message' and college_code=" + CollegeCode + "";
            securset.Clear();
            securset = d2.select_method_wo_parameter(Sql, "text");
            if (securset.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(securset.Tables[0].Rows[0]["linkvalue"]) == "1")
                    IntDispFineMess = 1;
                else
                    IntDispFineMess = 0;
            }
            Sql = "select * from inssettings where linkname='Display Book Status' and college_code=" + CollegeCode + "";
            securset.Clear();
            securset = d2.select_method_wo_parameter(Sql, "text");
            if (securset.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(securset.Tables[0].Rows[0]["linkvalue"]) == "1")
                {
                    lblmissing.Visible = true;
                    lbllost.Visible = true;
                    LblTotBooks.Visible = true;
                    TxtIssue.Visible = true;
                    Txtlost.Visible = true;
                    TxtTotBooks.Visible = true;
                }
                else
                {
                    lblmissing.Visible = false;
                    lbllost.Visible = false;
                    LblTotBooks.Visible = false;
                    TxtIssue.Visible = false;
                    Txtlost.Visible = false;
                    TxtTotBooks.Visible = false;
                }
            }
            //Sql = "SELECT SERVERPROPERTY('productversion') "
            //If securs.State Then securs.Close
            //securs.Open Sql, db
            //If Not securs.EOF Then
            //    StrSQLVersion = securs(0)
            //End If
            Sql = "SELECT * FROM InsSettings WHERE LinkName ='Fine Calculation Exclude Holidays' AND College_Code =" + CollegeCode + "";
            securset.Clear();
            securset = d2.select_method_wo_parameter(Sql, "text");
            if (securset.Tables[0].Rows.Count > 0)
            {
                string linkValHol = Convert.ToString(securset.Tables[0].Rows[0]["linkvalue"]);
                if (linkValHol == "1")
                    BlnExcHoliday = true;
                else
                    BlnExcHoliday = false;
            }
            else
            {
                BlnExcHoliday = false;
            }
            Sql = "select * from inssettings where linkname='Calculate Fine in Library Holidays' and college_code=" + CollegeCode + "";
            securset.Clear();
            securset = d2.select_method_wo_parameter(Sql, "text");
            if (securset.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(securset.Tables[0].Rows[0]["linkvalue"]) == "1")
                    BlnLibHol = true;
                else
                    BlnLibHol = false;
            }
            else
            {
                BlnLibHol = false;
            }

            Sql = "select * from inssettings where linkname='Display Book Status in Trans' and college_code=" + CollegeCode + "";
            securset.Clear();
            securset = d2.select_method_wo_parameter(Sql, "text");
            if (securset.Tables[0].Rows.Count > 0)
            {
                //if (Convert.ToString(securset.Tables[0].Rows[0]["linkvalue"]) == "1")
                // Fra_BookStatus.Visible = true;
                //else
                //  Fra_BookStatus.Visible = false;
            }
            else
                // Fra_BookStatus.Visible = true;
                Sql = "SELECT * FROM InsSettings WHERE LinkName ='Library Reservation Due' AND College_Code =" + CollegeCode + "";
            securset.Clear();
            securset = d2.select_method_wo_parameter(Sql, "text");
            if (securset.Tables[0].Rows.Count > 0)
            {
                LinkVal = Convert.ToString(securset.Tables[0].Rows[0]["linkvalue"]);
                string[] Linkarr = LinkVal.Split('/');
                int LinkValLen = Linkarr.Length;
                if (Linkarr.Length > 0)
                {
                    if (LinkValLen == 1)
                    {
                        BlnReserveDue = false;
                    }
                    if (LinkValLen == 2)
                    {
                        BlnReserveDue = Convert.ToBoolean(Linkarr[0]);
                        IntReserveDueType = Convert.ToInt32(Linkarr[1]);
                    }
                    if (LinkValLen == 3)
                    {
                        BlnReserveDue = Convert.ToBoolean(Linkarr[0]);
                        IntReserveDueType = Convert.ToInt32(Linkarr[1]);
                        IntReserveDueVal = Convert.ToInt32(Linkarr[2]);
                    }
                }
                else
                {
                    BlnReserveDue = false;
                }
            }
            else
            {
                BlnReserveDue = false;
            }

            Sql = "select * from inssettings where linkname='Display Toay Fine & Due Books in Issue Return' and college_code=" + CollegeCode + "";
            securset.Clear();
            securset = d2.select_method_wo_parameter(Sql, "text");
            if (securset.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(securset.Tables[0].Rows[0]["linkvalue"]) == "1")
                {
                    //Lbl_TodayDue.Visible = true;
                    //txt_circnum2.Visible = true;
                    //Lbl_TodayFine.Visible = true;
                    //Txt_CirFine.Visible = true;
                }
                else
                {
                    //Lbl_TodayDue.Visible = false;
                    //txt_circnum2.Visible = false;
                    //Lbl_TodayFine.Visible = false;
                    //Txt_CirFine.Visible = false;
                    //Shp_TodayDet.Height = Shp_TodayDet.Height - 500;
                }
            }
            else
            {
                //Lbl_TodayDue.Visible = true;
                //txt_circnum2.Visible = true;
                //Lbl_TodayFine.Visible = true;
                //Txt_CirFine.Visible = true;
            }

            Sql = "select * from inssettings where linkname='Allow Book Transaction Only if Geate In Entry' and college_code=" + CollegeCode + "";
            securset.Clear();
            securset = d2.select_method_wo_parameter(Sql, "text");
            if (securset.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(securset.Tables[0].Rows[0]["linkvalue"]) == "1")
                    BlnAllowTrans = true;
                else
                    BlnAllowTrans = false;
            }
            else
                BlnAllowTrans = false;


            Sql = "select * from inssettings where linkname='Multiple Renewal Days' and college_code=" + CollegeCode + "";
            securset.Clear();
            securset = d2.select_method_wo_parameter(Sql, "text");
            if (securset.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(securset.Tables[0].Rows[0]["linkvalue"]) == "1")
                    BlnMulRenewDays = true;
                else
                    BlnMulRenewDays = false;
            }
            else
                BlnMulRenewDays = false;
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    #endregion

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        List<string> name = new List<string>();
        //rollNoFlag = false;
        try
        {
            string query = "";
            WebService ws = new WebService();

            if (personmode == 0)
            {
                //student query
                if (chosedmode == 0)
                {
                    query = "select top 100 Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' and college_code=" + collegecode1 + " order by Roll_No asc";
                }
                else if (chosedmode == 1)
                {
                    query = "select  top 100 Reg_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by Reg_No asc";
                }
                else if (chosedmode == 2)
                {
                    query = "select  top 100 Roll_admit from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by Roll_admit asc";
                }
                else if (chosedmode == 3)
                {
                    query = "select  top 100 lib_id from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and lib_id like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by lib_id asc";
                }
                else if (chosedmode == 4)
                {
                    query = "select  top 100 smart_serial_no from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and smart_serial_no like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by smart_serial_no asc";
                }
            }
            else if (personmode == 1)
            {
                query = " select top 100 staff_code from staffmaster where resign<>1 and staff_code like '" + prefixText + "%' order by staff_code asc";
            }
            else if (personmode == 2)
            {
                //Vendor query
            }
            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        if (prefixText.Length > 0)
        {
            string[] nameval = prefixText.Split(' ');
            string query = string.Empty;
            string name_VAL = string.Empty;
            for (int i = 0; i < nameval.Length; i++)
            {
                name_VAL += "%" + nameval[i] + "%";
            }
            if (personmode == 0)
            {
                if (nameval.Length > 0)
                {
                    query = "select  top 100 a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+r.Roll_No,r.Roll_No from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + name_VAL + "'  and r.college_code='" + collegecode1 + "'";
                }
                else
                {
                    query = "select  top 100 a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+r.Roll_No,r.Roll_No from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '%" + prefixText + "%' and r.college_code='" + collegecode1 + "'";
                }
                Hashtable studhash = ws.GetNameSearch(query);
                if (studhash.Count > 0)
                {
                    foreach (DictionaryEntry p in studhash)
                    {
                        string studname = Convert.ToString(p.Key);
                        name.Add(studname);
                    }
                }
            }
            else if (personmode == 1)
            {
                if (nameval.Length > 0)
                {
                    query = " select top 100 staff_name+'-'+staff_code from staffmaster where resign<>1 and staff_name like '" + name_VAL + "' and college_code=" + collegecode1 + "  order by staff_name asc";
                }
                else
                {
                    query = " select top 100 staff_name+'-'+staff_code from staffmaster where resign<>1 and staff_name like '%" + prefixText + "%' and college_code=" + collegecode1 + "  order by staff_name asc";
                }
                name = ws.Getname(query);
            }

        }
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetAccNo(string prefixText)
    {
        WebService ws = new WebService();
        string query = "";
        if (AccBookType == 0)//Book
        {
            query = "select distinct top 100 acc_no from bookdetails where acc_no Like '" + prefixText + "%' AND lib_code='" + Acclibcode + "' order by acc_no asc";//and book_status='Available'
        }
        if (AccBookType == 1)//Periodicals
        {
            query = "select access_code from journal where access_code Like '" + prefixText + "%' AND lib_code='" + Acclibcode + "'  and issue_flag ='Available' order by access_code";
        }
        if (AccBookType == 2)//Project book
        {
            query = "SELECT distinct top 100 ProBook_Accno FROM Project_Book where ProBook_Accno Like '" + prefixText + "%' AND Lib_code ='" + Acclibcode + "' and issue_flag ='Available' order by ProBook_Accno";
        }
        if (AccBookType == 3)//Non-Book Material
        {
            query = "SELECT distinct top 100 nonbookmat_no FROM nonbookmat where nonbookmat_no Like '" + prefixText + "%' AND lib_code ='" + Acclibcode + "' and issue_flag ='Available'";
        }
        if (AccBookType == 4)//Question Bank
        {
            query = "select distinct top 100 access_code from university_question where access_code Like '" + prefixText + "%' AND lib_code='" + Acclibcode + "' and issue_flag ='Available' ";
        }
        if (AccBookType == 5)//Back Volume
        {
            query = "select access_code from back_volume where access_code Like '" + prefixText + "%' AND lib_code='" + Acclibcode + "' and issue_flag ='Available' order by access_code";
        }
        if (AccBookType == 6)//Reference Books
        {
            query = "select acc_no from bookdetails where acc_no like '%" + prefixText + "%' and book_status='Available' and lib_code='" + Acclibcode + "' and ref = 'Yes' order by acc_no";
        }

        DataSet dsN = new DataSet();
        List<string> name = new List<string>();
        try
        {
            name = ws.Getname(query);
        }
        catch { }
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Gettitle(string prefixText)
    {
        WebService ws = new WebService();

        string query = "";
        if (SearchByBook == 0)//Book
        {
            if (SearchByBookSpecific == 0)
            {
                query = "select acc_no from bookdetails where acc_no Like '" + prefixText + "%' AND lib_code='" + Acclibcode + "' and book_status='Available' order by acc_no asc";
            }
            if (SearchByBookSpecific == 1)
            {
                query = "select title from bookdetails where title Like '" + prefixText + "%' and lib_code='" + Acclibcode + "' and book_status='Available'";
            }
        }
        if (SearchByBook == 1)//Periodicals
        {
            if (SearchByBookSpecific == 0)
            {
                query = "select access_code from journal where access_code Like '" + prefixText + "%' and lib_code='" + Acclibcode + "'  and issue_flag ='Available'  order by access_code";
            }
            if (SearchByBookSpecific == 1)
            {
                query = "select title from journal where title Like '" + prefixText + "%' and lib_code='" + Acclibcode + "'  and issue_flag ='Available' order by title";
            }
            if (SearchByBookSpecific == 2)
            {
                query = "select journal_code from journal where journal_code Like '" + prefixText + "%' and lib_code='" + Acclibcode + "'  and issue_flag ='Available'  order by journal_code";
            }
        }
        if (SearchByBook == 2)//Project book
        {
            if (SearchByBookSpecific == 0)
            {
                query = "select probook_accno from project_book where probook_accno Like '" + prefixText + "%' and lib_code='" + Acclibcode + "'  and issue_flag ='Available' order by probook_accno";
            }
            if (SearchByBookSpecific == 1)
            {
                query = "select title from project_book where title Like '" + prefixText + "%' and lib_code='" + Acclibcode + "'  and issue_flag ='Available' order by title";
            }
            if (SearchByBookSpecific == 2)
            {
                query = "select name from project_book where name Like '" + prefixText + "%' and lib_code='" + Acclibcode + "'  and issue_flag ='Available' order by name";
            }
        }
        if (SearchByBook == 3)//Non-Book Material
        {
            if (SearchByBookSpecific == 0)
            {
                query = "select acc_no from nonbookmat where acc_no Like '" + prefixText + "%' and lib_code='" + Acclibcode + "' and  issue_flag='Available' order by acc_no";
            }
            if (SearchByBookSpecific == 1)
            {
                query = "select title from nonbookmat where title Like '" + prefixText + "%' and lib_code='" + Acclibcode + "' and  issue_flag='Available' order by title";
            }
            if (SearchByBookSpecific == 2)
            {
                query = "select nonbookmat_no from nonbookmat where nonbookmat_no Like '" + prefixText + "%' and lib_code='" + Acclibcode + "' and  issue_flag='Available' order by nonbookmat_no";
            }
        }
        if (SearchByBook == 4)//Question Bank
        {
            if (SearchByBookSpecific == 0)
            {
                query = "select access_code from university_question where access_code Like '" + prefixText + "%' and lib_code='" + Acclibcode + "' and issue_flag ='Available' order by access_code";
            }
            if (SearchByBookSpecific == 1)
            {
                query = "select title from university_question where title Like '" + prefixText + "%' and lib_code='" + Acclibcode + "' and issue_flag ='Available' order by title";
            }
        }
        if (SearchByBook == 5)//Back Volume
        {
            if (SearchByBookSpecific == 0)
            {
                query = "select access_code from back_volume where access_code Like '" + prefixText + "%' and lib_code='" + Acclibcode + "' and issue_flag='Available'  order by access_code";
            }
            if (SearchByBookSpecific == 1)
            {
                query = "select title from back_volume where title Like '" + prefixText + "%' and lib_code='" + Acclibcode + "' and issue_flag='Available'  order by title";
            }
        }
        if (SearchByBook == 6)//Reference Book
        {
            if (SearchByBookSpecific == 0)
            {
                query = "select acc_no from bookdetails where acc_no Like '" + prefixText + "%' and lib_code='" + Acclibcode + "' and ref = 'Yes' and book_status='Available' order by acc_no";
            }
            if (SearchByBookSpecific == 1)
            {
                query = "select title from bookdetails where title Like '" + prefixText + "%' and lib_code='" + Acclibcode + "' and ref = 'Yes' and book_status='Available' order by title";
            }
            if (SearchByBookSpecific == 2)
            {
                query = "select author from bookdetails where author Like '" + prefixText + "%' and lib_code='" + Acclibcode + "' and ref = 'Yes' and book_status='Available' order by author";
            }
        }
        DataSet dsN = new DataSet();
        List<string> name = new List<string>();
        try
        {
            name = ws.Getname(query);
        }
        catch { }
        return name;
    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Page.Form.DefaultFocus = txtRollNo.ClientID;
            string lib_code = Convert.ToString(ddllibrary.SelectedValue);
            Acclibcode = Convert.ToString(ddllibrary.SelectedValue);
            DispStatusList();
            string Sql = "";
            Sql = "SELECT ISNULL(Is_BookBank,0) BookBank FROM Library WHERE Lib_Code ='" + lib_code + "' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string bookBk = Convert.ToString(ds.Tables[0].Rows[0]["BookBank"]);
                if (bookBk.ToLower() == "true")
                    BlnBookBankLib = true;
                else
                    BlnBookBankLib = false;
            }
            else
                BlnBookBankLib = false;

            Sql = "SELECT ISNULL(BB_AllStud,0) BB_AllStud FROM Library WHERE Lib_Code ='" + lib_code + "' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string BB_AllStud = Convert.ToString(ds.Tables[0].Rows[0]["BB_AllStud"]);
                if (BB_AllStud.ToLower() == "true")
                    BlnBookBankAll = true;
                else
                    BlnBookBankAll = false;
            }
            else
                BlnBookBankAll = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void Cbo_CardLibrary_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Page.Form.DefaultFocus = txtRollNo.ClientID;
        if (Cbo_CardLibrary.Text != "All")
        {
            ddllibrary.SelectedValue = Cbo_CardLibrary.SelectedValue;
        }
        else
        {
            string collegecode = Convert.ToString(ddlcollege.SelectedValue);
            string Sql = "SELECT Lib_Name,lib_code FROM Library " + LibNameDefault + " AND college_code='" + collegecode + "' ";
            ds = d2.select_method_wo_parameter(Sql, "text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddllibrary.DataSource = ds;
                ddllibrary.DataTextField = "lib_name";
                ddllibrary.DataValueField = "lib_code";
                ddllibrary.DataBind();
            }
        }
    }

    protected void ddlissuetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlissue.SelectedItem.Text == "Book")
        {
            AccBookType = 0;
        }
        if (ddlissue.SelectedItem.Text == "Periodicals")
        {
            AccBookType = 1;
        }
        if (ddlissue.SelectedItem.Text == "Project book")
        {
            AccBookType = 2;
        }
        if (ddlissue.SelectedItem.Text == "Non-Book Material")
        {
            AccBookType = 3;
        }
        if (ddlissue.SelectedItem.Text == "Question Bank")
        {
            AccBookType = 4;
        }
        if (ddlissue.SelectedItem.Text == "Back Volume")
        {
            AccBookType = 5;
        }
        if (ddlissue.SelectedItem.Text == "Reference Books")
        {
            AccBookType = 6;
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        //bindLibrary();
        getLibPrivil();
        ddllibrary_SelectedIndexChanged(sender, e);
    }

    protected void BtnLib_click(object sender, EventArgs e)
    {
        if (Cbo_CardLibrary.SelectedItem.Text != "All")
        {
            SureDivSetDefault.Visible = true;
            Div1.Visible = true;
        }
    }

    protected void rblissue_Selected(object sender, EventArgs e)
    {
        try
        {
            lbl_issue.Text = "";
            if (rblissue.SelectedIndex == 0)
            {
                Page.Form.DefaultFocus = txtRollNo.ClientID;
                lbl_issue.Text = "Issue Date";
                lbl_due.Visible = true;
                Txtduedate.Visible = true;
                lblIssSpreadName.Text = "Issuing Books";
                lblIssSpreadName.ForeColor = Color.Green;
                lblIssSpreadName.Font.Bold = true;
                ClearFineDetails();
                Btnsave.Text = "Issue";
                //If cbo_UserEntry.Text = "Smart Card" Then
                //    Txt_SmartCardID.SetFocus
                //Else
                //    txt_rollno.SetFocus
                //End If

            }
            if (rblissue.SelectedIndex == 1)
            {
                Page.Form.DefaultFocus = Txtaccno.ClientID;
                lbl_issue.Text = "Return Date";
                lbl_due.Visible = false;
                Txtduedate.Visible = false;
                lblIssSpreadName.Text = "Returning Books";
                lblIssSpreadName.ForeColor = Color.Green;
                lblIssSpreadName.Font.Bold = true;
                //lbl_issue.Visible = false;
                // lbllast.Visible = false;
                // lblrenewal.Visible = false;
                ClearFineDetails();
                Btnsave.Text = "Return";
            }
            if (rblissue.SelectedIndex == 2)
            {

                lbl_issue.Text = "Renewal Date";
                lbl_due.Text = "Due Date";
                lbl_due.Visible = true;
                Txtduedate.Visible = true;
                lblIssSpreadName.Text = "Renewaling Books";
                lblIssSpreadName.ForeColor = Color.Green;
                lblIssSpreadName.Font.Bold = true;
                //lbl_issue.Visible = false;
                //  lbllast.Visible = false;
                //  lbl_return.Visible = false;
                ClearFineDetails();
                btnaccno.Enabled = false;
                Btnsave.Text = "Renewal";
            }
            if (rblissue.SelectedIndex == 3)
            {
                lbl_issue.Text = "Issue Date";
                lbl_due.Text = "Lost Date";
                lbl_due.Visible = true;
                Btnsave.Text = "Lost";
                lblIssSpreadName.Text = "Lost Books";
                lblIssSpreadName.ForeColor = Color.Green;
                lblIssSpreadName.Font.Bold = true;

                string serverDt = d2.ServerDate();
                string dt = serverDt.Split(' ')[0];
                Txtduedate.Text = dt;
                //lbl_issue.Visible = false;
                // lbl_return.Visible = false;
                // lblrenewal.Visible = false;
                ClearFineDetails();
                btnaccno.Enabled = false;

            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void ClearFineDetails()
    {
    }

    protected void RblMemType_Selected(object sender, EventArgs e)
    {
        try
        {
            if (RblMemType.SelectedIndex == 0)
            {
                Page.Form.DefaultFocus = txtRollNo.ClientID;
                lblUserEntryId.Text = "Roll No";
                lblUserEntryId.Font.Bold = true;
                LblPopName.Text = "Student Name";
                personmode = 0;
            }
            if (RblMemType.SelectedIndex == 1)
            {
                Page.Form.DefaultFocus = txtRollNo.ClientID;
                lblUserEntryId.Text = "Staff Code";
                lblUserEntryId.Font.Bold = true;
                LblPopName.Text = "Staff Name";
                personmode = 1;
            }
            if (RblMemType.SelectedIndex == 2)
            {
                Page.Form.DefaultFocus = txtRollNo.ClientID;
                lblUserEntryId.Text = "MemberId";
                lblUserEntryId.Font.Bold = true;
                ddluserentry.Enabled = false;
                personmode = 2;
            }
        }
        catch
        {
        }
    }

    protected void ddluserentry_SelectedIndexChanged(object sender, EventArgs e)
    {
        TxtSmartCard.Visible = false;
        if (RblMemType.SelectedIndex == 0)
        {
            if (ddluserentry.SelectedItem.Text == "Library ID")
            {
                lblUserEntryId.Text = "Lib ID";
                lblUserEntryId.Font.Bold = true;
                chosedmode = 3;
            }
            if (ddluserentry.SelectedItem.Text == "Roll Number")
            {
                lblUserEntryId.Text = "Roll No";
                lblUserEntryId.Font.Bold = true;
                chosedmode = 0;
            }
            if (ddluserentry.SelectedItem.Text == "Register Number")
            {
                lblUserEntryId.Text = "Reg No.";
                lblUserEntryId.Font.Bold = true;
                chosedmode = 1;
            }
            if (ddluserentry.SelectedItem.Text == "Admission Number")
            {
                lblUserEntryId.Text = "Admission Number";
                lblUserEntryId.Font.Bold = true;
                chosedmode = 2;
            }
            if (ddluserentry.SelectedItem.Text == "Smart Card")
            {
                lblUserEntryId.Text = "Roll No";
                TxtSmartCard.Visible = true;
                lblUserEntryId.Font.Bold = true;
                chosedmode = 4;
            }
        }
        if (RblMemType.SelectedIndex == 1)
        {
            if (ddluserentry.SelectedItem.Text == "Smart Card")
            {
                lblUserEntryId.Text = "Staff Code";
                TxtSmartCard.Visible = true;
                lblUserEntryId.Font.Bold = true;
            }
        }
    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        popupselectlibid.Visible = false;
    }

    protected void imagebtnpopclose4_Click(object sender, EventArgs e)
    {
        DivpopupStaff.Visible = false;
    }

    protected void btnissutype_Click(object sender, EventArgs e)
    {
        try
        {
            if (RblMemType.SelectedIndex == 0)
            {
                popupselectlibid.Visible = true;
                DivpopupStaff.Visible = false;
                grdStudent.Visible = false;
                btn_std_exit1.Visible = false;

                if (ddluserentry.Text == "Library ID")
                {
                    lbl_popupselectlibid.Text = "Select Library ID";
                    lbl_lib_id.Text = "Library ID";
                }
                if (ddluserentry.Text == "Register Number")
                {
                    lbl_popupselectlibid.Text = "Select Register Number";
                    lbl_lib_id.Text = "Reg No";
                }
                if (ddluserentry.Text == "Roll Number")
                {
                    lbl_popupselectlibid.Text = "Select Roll Number";
                    lbl_lib_id.Text = "Roll No";
                }
            }
            else
            {
                loadstaff_dept();
                btn_staff_Go_Click(sender, e);
                grdStaff.Visible = false;
                btn_staff_exit1.Visible = false;
                popupselectlibid.Visible = false;
                DivpopupStaff.Visible = true;
            }
        }
        catch
        {

        }
    }

    #region Staff LookUp

    public void loadstaff_dept()
    {
        try
        {
            ddl_staffdept.Items.Clear();
            ds.Clear();
            string College = ddlcollege.SelectedValue.ToString();
            string SelectQ = string.Empty;
            if (!string.IsNullOrEmpty(College))
            {
                SelectQ = "select dept_name,dept_code  from hrdept_master where college_code='" + College + "' order by dept_name";
                ds.Clear();
                ds = d2.select_method_wo_parameter(SelectQ, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_staffdept.DataSource = ds;
                    ddl_staffdept.DataTextField = "dept_name";
                    ddl_staffdept.DataValueField = "dept_code";
                    ddl_staffdept.DataBind();
                }
                ddl_staffdept.Items.Insert(0, "All");
            }

        }
        catch
        {


        }


    }

    protected void ddl_staffdept_SelectedIndexChanged(object sendre, EventArgs e)
    {

    }

    protected void btn_staff_Go_Click(object sender, EventArgs e)
    {
        try
        {

            DataSet dsgetsatff = new DataSet();
            dsgetsatff = getStaffdetails();
            if (dsgetsatff.Tables.Count > 0 && dsgetsatff.Tables[0].Rows.Count > 0)
            {
                loadspreadstaffdetails(dsgetsatff);
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alertMsg.Text = "No Records Found";
            }
        }
        catch
        {
        }
    }

    private DataSet getStaffdetails()
    {
        DataSet dsload1 = new DataSet();
        try
        {
            #region get Value

            string sqlgetstadetails = "";
            string strStaffID = "";
            string staffdeptcode = "";
            string stafftxt = "";
            string staffdept = "";

            string value = d2.GetFunction("select * from inssettings where linkname ='Library id'");
            if (value != "")
            {
                if (value == "0")
                    strStaffID = "staffmaster.staff_code";
                else
                    strStaffID = "staffmaster.lib_id";
            }
            else
                strStaffID = "staffmaster.staff_code";

            if (ddlcollege.Items.Count > 0)
                collcode = Convert.ToString(ddlcollege.SelectedValue);
            if (ddl_staffdept.Items.Count > 0)
                staffdeptcode = Convert.ToString(ddl_staffdept.SelectedValue);
            if (staffdeptcode != "All")
                staffdept = "and hm.dept_code='" + staffdeptcode + "'";
            if (txt_staffname.Text != "")
            {
                stafftxt = "AND  dept_name='" + txt_staffname.Text + "'";

            }

            if (!string.IsNullOrEmpty(collcode) && !string.IsNullOrEmpty(staffdeptcode))
            {
                sqlgetstadetails = "SELECT sm.staff_code,sm.staff_name,hm.dept_name  From staffmaster sm, stafftrans st, hrdept_master hm WHERE sm.staff_code = st.staff_code AND st.dept_code = hm.dept_code " + stafftxt + "  AND sm.resign = 0 and settled = 0 and latestrec = 1 and sm.college_code='" + collcode + "' " + staffdept + "";
            }
            dsload1.Clear();
            dsload1 = d2.select_method_wo_parameter(sqlgetstadetails, "Text");


            #endregion
        }
        catch (Exception ex)
        { }

        return dsload1;


    }

    public void loadspreadstaffdetails(DataSet ds)
    {
        try
        {
            DataTable bokstaff = new DataTable();
            DataRow drbokstaff;
            bokstaff.Columns.Add("Staff Code", typeof(string));
            bokstaff.Columns.Add("Staff Name", typeof(string));
            bokstaff.Columns.Add("Department", typeof(string));
            string id = "";
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    drbokstaff = bokstaff.NewRow();
                    id = Convert.ToString(ds.Tables[0].Rows[row]["staff_code"]).Trim();
                    string stname = Convert.ToString(ds.Tables[0].Rows[row]["staff_name"]).Trim();
                    string dept = Convert.ToString(ds.Tables[0].Rows[row]["dept_name"]).Trim();
                    drbokstaff["Staff Code"] = id;
                    drbokstaff["Staff Name"] = stname;
                    drbokstaff["Department"] = dept;
                    bokstaff.Rows.Add(drbokstaff);
                }
                grdStaff.DataSource = bokstaff;
                grdStaff.DataBind();
                grdStaff.Visible = true;
                btn_staff_exit1.Visible = true;
            }
        }
        catch
        {

        }
    }

    protected void grdStaff_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , HiddenFieldgrdStaff.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void grdStaff_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.HiddenFieldgrdStaff.Value);
            string staffids = grdStaff.Rows[rowIndex].Cells[1].Text;
            string staname = grdStaff.Rows[rowIndex].Cells[2].Text;
            txtRollNo.Text = staffids;
            TxtName.Text = staname;
            DivpopupStaff.Visible = false;
        }
        catch
        {
        }
    }

    protected void btn_staff_exit1_Click(object sender, EventArgs e)
    {
        try
        {
            DivpopupStaff.Visible = false;
        }
        catch
        {

        }

    }

    #endregion

    #region Student LookUp

    public void bindbatch()
    {
        try
        {

            ddlbatch.Items.Clear();
            ds = d2.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }
            int count1 = ds.Tables[1].Rows.Count;
            if (count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                ddlbatch.SelectedValue = max_bat.ToString();
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch();
            binddegree();
            bindsem();
            bindsec();
            grdStudent.Visible = false;

            btn_std_exit1.Visible = false;

        }
        catch (Exception ex)
        {

        }
    }

    public void binddegree()
    {
        try
        {

            ddldegree.Items.Clear();
            userCode = Session["usercode"].ToString();
            userCollegeCode = ddlcollege.SelectedItem.Value;
            singleUser = Session["single_user"].ToString();
            groupUserCode = Session["group_code"].ToString();
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = group_semi[0].ToString();
            }
            has.Clear();
            has.Add("single_user", singleUser);
            has.Add("group_code", groupUserCode);
            has.Add("college_code", userCollegeCode);
            has.Add("user_code", userCode);
            ds = d2.select_method("bind_degree", has, "sp");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch();
            bindsem();
            bindsec();
            grdStudent.Visible = false;

            btn_std_exit1.Visible = false;

        }
        catch (Exception ex)
        {

        }
    }

    public void bindbranch()
    {
        try
        {

            ddlsem.Items.Clear();
            has.Clear();
            userCode = Session["usercode"].ToString();
            userCollegeCode = ddlcollege.SelectedItem.Value;
            singleUser = Session["single_user"].ToString();
            groupUserCode = Session["group_code"].ToString();
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = group_semi[0].ToString();
            }
            has.Add("single_user", singleUser);
            has.Add("group_code", groupUserCode);
            has.Add("course_id", ddldegree.SelectedValue);
            has.Add("college_code", userCollegeCode);
            has.Add("user_code", userCode);
            ds = d2.select_method("bind_branch", has, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsem();
            bindsec();
            grdStudent.Visible = false;

            btn_std_exit1.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

    public void bindsem()
    {
        try
        {

            ddlsem.Items.Clear();
            string duration = string.Empty;
            Boolean first_year = false;
            has.Clear();
            userCollegeCode = ddlcollege.SelectedItem.Value;
            has.Add("degree_code", ddlbranch.SelectedValue.ToString());
            has.Add("batch_year", ddlbatch.SelectedValue.ToString());
            has.Add("college_code", userCollegeCode);
            ds = d2.select_method("bind_sem", has, "sp");
            int count3 = ds.Tables[0].Rows.Count;
            if (count3 > 0)
            {
                ddlsem.Enabled = true;
                duration = ds.Tables[0].Rows[0][0].ToString();
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                {
                    if (first_year == false)
                    {
                        ddlsem.Items.Add(loop_val.ToString());
                    }
                    else if (first_year == true && loop_val != 2)
                    {
                        ddlsem.Items.Add(loop_val.ToString());
                    }
                }
            }
            else
            {
                count3 = ds.Tables[1].Rows.Count;
                if (count3 > 0)
                {
                    ddlsem.Enabled = true;
                    duration = ds.Tables[1].Rows[0][0].ToString();
                    first_year = Convert.ToBoolean(ds.Tables[1].Rows[0][1].ToString());
                    for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                    {
                        if (first_year == false)
                        {
                            ddlsem.Items.Add(loop_val.ToString());
                        }
                        else if (first_year == true && loop_val != 2)
                        {
                            ddlsem.Items.Add(loop_val.ToString());
                        }
                    }
                }
                else
                {
                    ddlsem.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsec();
            grdStudent.Visible = false;

            btn_std_exit1.Visible = false;

        }
        catch (Exception ex)
        {

        }
    }

    public void bindsec()
    {
        try
        {

            ddlSec.Items.Clear();
            hat.Clear();
            hat.Add("batch_year", ddlbatch.SelectedValue.ToString());
            hat.Add("degree_code", ddlbranch.SelectedValue);
            ds = d2.select_method("bind_sec", hat, "sp");
            int count5 = ds.Tables[0].Rows.Count;
            if (count5 > 0)
            {
                ddlSec.DataSource = ds;
                ddlSec.DataTextField = "sections";
                ddlSec.DataValueField = "sections";
                ddlSec.DataBind();
                ddlSec.Enabled = true;
            }
            else
            {
                ddlSec.Enabled = false;
            }
            ddlSec.Items.Add("All");
        }
        catch (Exception ex)
        {
        }
    }

    protected void btn_go_libid_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsgetbook = new DataSet();
            dsgetbook = getStudentdetails();
            if (dsgetbook.Tables.Count > 0 && dsgetbook.Tables[0].Rows.Count > 0)
            {
                loadspreadstddetails(dsgetbook);
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alertMsg.Text = "No Records Found";
            }
        }
        catch
        {
        }
    }

    private DataSet getStudentdetails()
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get Value

            string sqlgetstddetails = "";
            string collcode = "";
            string batch = "";
            string courseid = "";
            string bran = "";
            string sem = "";
            string sec = "";
            string Section = "";
            string strID = "";
            string strStaffID = "";
            string stdID = "";
            string txtid = "";
            string txtname = "";


            string value = d2.GetFunction("select * from inssettings where linkname ='Library id'");
            if (value != "")
            {
                if (value == "0")
                {
                    strID = "R.roll_no";
                    strStaffID = "staffmaster.staff_code";
                }
                else
                {
                    strID = "R.lib_id";
                    strStaffID = "staffmaster.lib_id";
                }
            }
            else
            {
                strID = "R.roll_no";
                strStaffID = "staffmaster.staff_code";
            }


            if (ddlcollege.Items.Count > 0)
                collcode = Convert.ToString(ddlcollege.SelectedValue);
            if (ddlbatch.Items.Count > 0)
                batch = Convert.ToString(ddlbatch.SelectedValue);
            if (ddldegree.Items.Count > 0)
                courseid = Convert.ToString(ddldegree.SelectedValue);
            if (ddlbranch.Items.Count > 0)
                bran = Convert.ToString(ddlbranch.SelectedValue);
            if (ddlsem.Items.Count > 0)
                sem = Convert.ToString(ddlsem.SelectedValue);
            if (ddlSec.Items.Count > 0)
                sec = Convert.ToString(ddlSec.SelectedValue).Trim();

            if (sec == "" || sec == "All")
                Section = "";
            else
                Section = "and R.sections='" + sec + "'";
            if (RblMemType.SelectedIndex == 0)
            {
                if (lbl_lib_id.Text == "Library ID")
                {
                    stdID = "R.lib_id";
                    if (tx_libid.Text != "")
                        txtid = "and R.lib_id='" + tx_libid.Text + "'";
                    if (tx_libname.Text != "")
                        txtname = "and R.Stud_Name='" + tx_libname.Text + "'";
                }
                else if (lbl_lib_id.Text == "Reg No")
                {
                    stdID = "R.reg_no";
                    if (tx_libid.Text != "")
                        txtid = "and R.reg_no='" + tx_libid.Text + "'";
                    if (tx_libname.Text != "")
                        txtname = "and R.Stud_Name='" + tx_libname.Text + "'";
                }
                else if (lbl_lib_id.Text == "Roll No")
                {
                    stdID = "R.roll_no";
                    if (tx_libid.Text != "")
                        txtid = "and R.roll_no='" + tx_libid.Text + "'";
                    if (tx_libname.Text != "")
                        txtname = "and R.Stud_Name='" + tx_libname.Text + "'";
                }
            }
            if (!string.IsNullOrEmpty(collcode) && !string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(courseid) && !string.IsNullOrEmpty(bran) && !string.IsNullOrEmpty(sem))
            {
                sqlgetstddetails = "SELECT distinct " + stdID + ", R.Stud_Name, C.Course_Name + '-' + D.Dept_Name as Degree, R.Current_Semester FROM Registration R,Degree G,Course C,Department D WHERE R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK' and R.batch_year='" + batch + "' and  R.Degree_Code='" + bran + "' AND C.Course_Id='" + courseid + "'  and C.college_code='" + collcode + "' and R.Current_Semester='" + sem + "' " + Section + " " + txtid + txtname + " order by " + stdID + "";

            }
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(sqlgetstddetails, "Text");


            #endregion
        }
        catch (Exception ex)
        { }

        return dsload;


    }

    public void loadspreadstddetails(DataSet ds)
    {
        try
        {
            DataTable studdetails = new DataTable();
            DataRow drdet;
            if (lbl_lib_id.Text == "Library ID")
                studdetails.Columns.Add("Library ID", typeof(string));
            else if (lbl_lib_id.Text == "Reg No")
                studdetails.Columns.Add("Register No", typeof(string));

            else
                studdetails.Columns.Add("Roll No", typeof(string));

            studdetails.Columns.Add("Name", typeof(string));
            studdetails.Columns.Add("Degree", typeof(string));
            int sno = 0;
            string id = "";
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    drdet = studdetails.NewRow();
                    if (lbl_lib_id.Text == "Library ID")
                        id = Convert.ToString(ds.Tables[0].Rows[row]["lib_id"]).Trim();
                    else if (lbl_lib_id.Text == "Reg No")
                        id = Convert.ToString(ds.Tables[0].Rows[row]["reg_no"]).Trim();
                    else
                        id = Convert.ToString(ds.Tables[0].Rows[row]["roll_no"]).Trim();

                    string name = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]).Trim();
                    string degre = Convert.ToString(ds.Tables[0].Rows[row]["Degree"]).Trim();


                    if (lbl_lib_id.Text == "Library ID")
                        drdet["Library ID"] = id;

                    else if (lbl_lib_id.Text == "Reg No")
                        drdet["Register No"] = id;
                    else
                        drdet["Roll No"] = id;
                    drdet["Name"] = name;
                    drdet["Degree"] = degre;
                    studdetails.Rows.Add(drdet);

                }
                divRollNo.Visible = true;
                grdStudent.DataSource = studdetails;
                grdStudent.DataBind();
                grdStudent.Visible = true;
                for (int l = 0; l < grdStudent.Rows.Count; l++)
                {
                    foreach (GridViewRow row in grdStudent.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            grdStudent.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
                btn_std_exit1.Visible = true;
            }
        }
        catch
        {

        }

    }


    protected void grdStudent_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCell.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void grdStudent_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCell.Value);
            string idorno = grdStudent.Rows[rowIndex].Cells[1].Text;
            string stdname = grdStudent.Rows[rowIndex].Cells[1].Text;
            txtRollNo.Text = idorno;
            TxtName.Text = stdname;
            txtRollNo_Change(sender, e);
            popupselectlibid.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void btn_std_exit1_Click(object sender, EventArgs e)
    {
        popupselectBook.Visible = false;
    }

    #endregion

    #region Access No LookUp

    protected void btnaccno_Click(object sender, EventArgs e)
    {
        try
        {
            //SpreadIssuingBook.SaveChanges();
            string issuetype = Convert.ToString(ddlissue.SelectedItem.Text);
            string LibCode = Convert.ToString(ddllibrary.SelectedValue);
            //if (SpreadIssuingBook.Sheets[0].Rows.Count > 0)
            //{
            //    AccessNoLookup.Visible = true;
            //    lblAccessNoLookup.Text = "Save the details and select other type";
            //}
            if (issuetype == "Book")
            {
                dd_search.Items.Clear();
                popupselectBook.Visible = true;
                dd_search.Items.Add("All");
                dd_search.Items.Add("Access Number");
                dd_search.Items.Add("Title");
                SearchByBook = 0;
            }
            if (issuetype == "Periodicals")
            {
                dd_search.Items.Clear();
                popupselectBook.Visible = true;
                dd_search.Items.Add("All");
                dd_search.Items.Add("Access Number");
                dd_search.Items.Add("Title");
                dd_search.Items.Add("Journal Code");
                SearchByBook = 1;
            }
            if (issuetype == "Project book")
            {
                dd_search.Items.Clear();
                popupselectBook.Visible = true;
                dd_search.Items.Add("All");
                dd_search.Items.Add("Access Number");
                dd_search.Items.Add("Title");
                dd_search.Items.Add("Author");
                SearchByBook = 2;
            }
            if (issuetype == "Non-Book Material")
            {
                dd_search.Items.Clear();
                popupselectBook.Visible = true;
                dd_search.Items.Add("All");
                dd_search.Items.Add("Access Number");
                dd_search.Items.Add("Title");
                dd_search.Items.Add("NonBook No");
                SearchByBook = 3;
            }
            if (issuetype == "Question Bank")
            {
                dd_search.Items.Clear();
                popupselectBook.Visible = true;
                dd_search.Items.Add("All");
                dd_search.Items.Add("Access Number");
                dd_search.Items.Add("Title");
                SearchByBook = 4;
            }
            if (issuetype == "Back Volume")
            {
                dd_search.Items.Clear();
                popupselectBook.Visible = true;
                dd_search.Items.Add("All");
                dd_search.Items.Add("Access Number");
                dd_search.Items.Add("Title");
                SearchByBook = 5;
            }
            if (issuetype == "Reference Books")
            {
                dd_search.Items.Clear();
                popupselectBook.Visible = true;
                dd_search.Items.Add("All");
                dd_search.Items.Add("Access Number");
                dd_search.Items.Add("Title");
                dd_search.Items.Add("Author");
                SearchByBook = 6;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void dd_search_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            btn_Acc_exit1.Visible = false;
            if (dd_search.Text == "All")
                txt_search_book.Visible = false;
            else
                txt_search_book.Visible = true;
            if (ddlissue.SelectedItem.Text == "Book")
            {
                if (dd_search.Text == "Access Number")
                {
                    SearchByBookSpecific = 0;
                }
                if (dd_search.Text == "Title")
                {
                    SearchByBookSpecific = 1;
                }
            }
            if (ddlissue.SelectedItem.Text == "Periodicals")
            {
                if (dd_search.Text == "Access Number")
                {
                    SearchByBookSpecific = 0;
                }
                if (dd_search.Text == "Title")
                {
                    SearchByBookSpecific = 1;
                }
                if (dd_search.Text == "Journal Code")
                {
                    SearchByBookSpecific = 2;
                }
            }
            if (ddlissue.SelectedItem.Text == "Project book")
            {
                if (dd_search.Text == "Access Number")
                {
                    SearchByBookSpecific = 0;
                }
                if (dd_search.Text == "Title")
                {
                    SearchByBookSpecific = 1;
                }
                if (dd_search.Text == "Author")
                {
                    SearchByBookSpecific = 2;
                }
            }
            if (ddlissue.SelectedItem.Text == "Non-Book Material")
            {
                if (dd_search.Text == "Access Number")
                {
                    SearchByBookSpecific = 0;
                }
                if (dd_search.Text == "Title")
                {
                    SearchByBookSpecific = 1;
                }
                if (dd_search.Text == "NonBook No")
                {
                    SearchByBookSpecific = 2;
                }
            }
            if (ddlissue.SelectedItem.Text == "Question Bank")
            {
                if (dd_search.Text == "Access Number")
                {
                    SearchByBookSpecific = 0;
                }
                if (dd_search.Text == "Title")
                {
                    SearchByBookSpecific = 1;
                }
            }
            if (ddlissue.SelectedItem.Text == "Back Volume")
            {
                if (dd_search.Text == "Access Number")
                {
                    SearchByBookSpecific = 0;
                }
                if (dd_search.Text == "Title")
                {
                    SearchByBookSpecific = 1;
                }
            }
            if (ddlissue.SelectedItem.Text == "Reference Books")
            {
                if (dd_search.Text == "Access Number")
                {
                    SearchByBookSpecific = 0;
                }
                if (dd_search.Text == "Title")
                {
                    SearchByBookSpecific = 1;
                }
                if (dd_search.Text == "Author")
                {
                    SearchByBookSpecific = 2;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btn_go_book_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsgetaccno = new DataSet();
            string search1 = "";
            if (dd_search.Items.Count > 0)
                search1 = Convert.ToString(dd_search.SelectedValue);
            if (search1 != "" && search1 != "All")
            {
                if (txt_search_book.Text == "")
                {
                    imgdiv2.Visible = true;
                    lbl_alertMsg.Text = "Enter " + search1 + "";
                    return;
                }
            }
            dsgetaccno = getaccessnodetails();
            if (dsgetaccno.Tables.Count > 0 && dsgetaccno.Tables[0].Rows.Count > 0)
            {
                loadspreadaccnodetails(dsgetaccno);
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alertMsg.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
        }

    }

    private DataSet getaccessnodetails()
    {
        DataSet dsload2 = new DataSet();
        try
        {
            #region get Value

            string sqlgetaccno = "";
            string search = "";
            string libcode = "";
            string searchaccno = "";
            if (ddlcollege.Items.Count > 0)
                collcode = Convert.ToString(ddlcollege.SelectedValue);
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (dd_search.Items.Count > 0)
                search = Convert.ToString(dd_search.SelectedValue);
            string issuetype = Convert.ToString(ddlissue.SelectedItem.Text);
            if (search != "All")
            {
                if (txt_search_book.Text != "")
                {
                    if (issuetype == "Book" || issuetype == "Non-Book Material" || issuetype == "Reference Books")
                    {
                        if (search == "Access Number")
                            searchaccno = "and acc_no='" + txt_search_book.Text + "'";
                    }
                    if (issuetype == "Periodicals" || issuetype == "Back Volume")
                    {
                        if (search == "Access Number")
                            searchaccno = "and access_code='" + txt_search_book.Text + "'";
                    }
                    if (issuetype == "Project book")
                    {
                        if (search == "Access Number")
                            searchaccno = "and probook_accno='" + txt_search_book.Text + "'";
                    }

                    else if (search == "Title")
                        searchaccno = "and title='" + txt_search_book.Text + "'";
                    else if (search == "Author")
                        searchaccno = "and Author='" + txt_search_book.Text + "'";
                    else if (search == "Journal Code")
                        searchaccno = "and journal_code='" + txt_search_book.Text + "'";
                    else if (search == "NonBook No")
                        searchaccno = "and nonbookmat_no='" + txt_search_book.Text + "'";
                }
            }

            if (!string.IsNullOrEmpty(collcode) && !string.IsNullOrEmpty(libcode))
            {
                //sqlgetaccno = "select convert(numeric,bookdetails.acc_no)as acc_no,bookdetails.title,bookdetails.author,roll_no + '-' + stud_name as stud_name,bookdetails.publisher,bookdetails.edition from bookdetails,borrow where bookdetails.acc_no=borrow.acc_no and borrow.return_flag=0 and bookdetails.lib_code=borrow.lib_code and bookdetails.book_status not in(select book_status from bookdetails where book_status='Available' " + searchaccno + " and author = bookdetails.author AND lib_code='" + libcode + "' and bookdetails.lib_code='" + libcode + "') order by acc_no";
                if (issuetype == "Book")
                {
                    sqlgetaccno = "select acc_no,title,author from bookdetails where lib_code='" + libcode + "' and ref='No' and book_status = 'Available' " + searchaccno + " order by len(acc_no),acc_no";
                }
                if (issuetype == "Periodicals")
                {
                    sqlgetaccno = "select access_code as acc_no,journal_code,title from journal where lib_code='" + libcode + "'  and issue_flag ='Available' " + searchaccno + " order by len(journal_code),journal_code";
                }
                if (issuetype == "Project book")
                {
                    sqlgetaccno = "select probook_accno as acc_no,title,name from project_book where lib_code='" + libcode + "'  and issue_flag ='Available' " + searchaccno + " order by len(probook_accno),probook_accno";
                }
                if (issuetype == "Non-Book Material")
                {
                    sqlgetaccno = "select nonbookmat_no,acc_no,title,author from nonbookmat where lib_code='" + libcode + "' and  issue_flag='Available'  " + searchaccno + " order by len(acc_no),acc_no";
                }
                if (issuetype == "Question Bank")
                {
                    sqlgetaccno = "select access_code as acc_no,title from university_question where lib_code='" + libcode + "' and issue_flag ='Available' " + searchaccno + " order by len(access_code),access_code";
                }
                if (issuetype == "Back Volume")
                {
                    sqlgetaccno = "select access_code as acc_no,title from back_volume where lib_code='" + libcode + "' and issue_flag='Available' " + searchaccno + " order by len(access_code),access_code";
                }
                if (issuetype == "Reference Books")
                {
                    sqlgetaccno = "select acc_no,title,author from bookdetails where lib_code='" + libcode + "' and ref = 'Yes' and book_status='Available' " + searchaccno + " order by len(acc_no),acc_no";
                }
            }
            dsload2.Clear();
            dsload2 = d2.select_method_wo_parameter(sqlgetaccno, "Text");

            #endregion
        }
        catch (Exception ex)
        { }
        return dsload2;
    }

    public void loadspreadaccnodetails(DataSet ds)
    {
        try
        {
            string issuetype = Convert.ToString(ddlissue.SelectedItem.Text);
            DataTable bokaccess = new DataTable();
            DataRow drbokacc;

            bokaccess.Columns.Add("Access No", typeof(string));
            if (issuetype == "Periodicals")
            {
                bokaccess.Columns.Add("Journal Code", typeof(string));
            }
            if (issuetype == "Non-Book Material")
            {
                bokaccess.Columns.Add("NonBook No.", typeof(string));
            }
            bokaccess.Columns.Add("Title", typeof(string));

            if (issuetype == "Non-Book Material" || issuetype == "Reference Books" || issuetype == "Books")
            {
                bokaccess.Columns.Add("Author", typeof(string));
            }
            string accno = "";
            string title = "";
            string author = "";
            string BK_No = "";
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    drbokacc = bokaccess.NewRow();
                    accno = Convert.ToString(ds.Tables[0].Rows[row]["acc_no"]).Trim();
                    title = Convert.ToString(ds.Tables[0].Rows[row]["title"]).Trim();
                    drbokacc["Access No"] = accno;

                    if (issuetype == "Periodicals")
                    {
                        BK_No = Convert.ToString(ds.Tables[0].Rows[row]["journal_code"]).Trim();
                        drbokacc["Journal Code"] = BK_No;
                    }
                    if (issuetype == "Non-Book Material")
                    {
                        BK_No = Convert.ToString(ds.Tables[0].Rows[row]["nonbookmat_no"]).Trim();
                        drbokacc["NonBook No."] = BK_No;
                    }
                    drbokacc["Title"] = title;

                    if (issuetype == "Non-Book Material" || issuetype == "Reference Books" || issuetype == "Books")
                    {
                        author = Convert.ToString(ds.Tables[0].Rows[row]["author"]).Trim();
                        drbokacc["Author"] = author;
                    }
                    bokaccess.Rows.Add(drbokacc);
                }
                grdBook.DataSource = bokaccess;
                grdBook.DataBind();
                grdBook.Visible = true;
                btn_Acc_exit1.Visible = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void grdBook_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , HiddenFieldgrdBook.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void grdBook_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.HiddenFieldgrdBook.Value);
            string txtaccno = grdBook.Rows[rowIndex].Cells[1].Text;
            string txttitle = grdBook.Rows[rowIndex].Cells[2].Text;
            Txtaccno.Text = txtaccno;

            popupselectBook.Visible = false;
        }
        catch
        {
        }
    }

    protected void btn_Acc_exit1_Click(object sender, EventArgs e)
    {
        popupselectBook.Visible = false;
    }

    #endregion

    protected void lnkSetting_Click(object sender, EventArgs e)
    {
        popalertsetting.Visible = true;
    }

    #region Trace Book

    protected void lnktracebook_Click(object sender, EventArgs e)
    {
        DivTraceBkUp.Visible = true;
        DivTrace.Visible = true;
        txtTraceAccNo.Text = "";
    }

    protected void BtnTraceAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string Trace_Book_Type = "";
            string issueType = Convert.ToString(ddlissue.SelectedValue);
            if (issueType == "Book")
                Trace_Book_Type = "BOK";
            if (issueType == "Periodicals")
                Trace_Book_Type = "PER";
            if (issueType == "Project Book")
                Trace_Book_Type = "PRO";
            if (issueType == "Non-Book Material")
                Trace_Book_Type = "NBM";
            if (issueType == "Question Bank")
                Trace_Book_Type = "QBA";
            if (issueType == "Back Volume")
                Trace_Book_Type = "BVO";
            if (issueType == "Reference Books")
                Trace_Book_Type = "REF";
            string lib = Convert.ToString(ddlTraceLib.SelectedValue);
            int insert = 0;
            string qry = "insert into trace_bookdetails values(" + txtTraceAccNo.Text + ",'" + Trace_Book_Type + "'," + lib + ")";
            insert = d2.update_method_wo_parameter(qry, "Text");
            if (insert > 0)
            {
                imgdiv2.Visible = true;
                lbl_alertMsg.Text = "Added Successfully";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void BtnTraceDel_Click(object sender, EventArgs e)
    {
        try
        {
            string Trace_Book_Type = "";
            string issueType = Convert.ToString(ddlissue.SelectedValue);
            if (issueType == "Book")
                Trace_Book_Type = "BOK";
            if (issueType == "Periodicals")
                Trace_Book_Type = "PER";
            if (issueType == "Project Book")
                Trace_Book_Type = "PRO";
            if (issueType == "Non-Book Material")
                Trace_Book_Type = "NBM";
            if (issueType == "Question Bank")
                Trace_Book_Type = "QBA";
            if (issueType == "Back Volume")
                Trace_Book_Type = "BVO";
            if (issueType == "Reference Books")
                Trace_Book_Type = "REF";
            string lib = Convert.ToString(ddlTraceLib.SelectedValue);
            int delete = 0;
            string qry = "delete from trace_bookdetails where Acc_no=" + txtTraceAccNo.Text + " and booktype='" + Trace_Book_Type + "' and lib_code=" + lib + "";
            delete = d2.update_method_wo_parameter(qry, "Text");
            txtTraceAccNo.Text = "";
        }
        catch (Exception ex)
        {
        }
    }

    protected void BtnTraceExit_Click(object sender, EventArgs e)
    {
        DivTraceBkUp.Visible = false;
    }

    #endregion

    protected void Btnsettingclose_Click(object sender, EventArgs e)
    {
        popalertsetting.Visible = false;
        Div3.Visible = false;
    }

    protected void Btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            string Sql = "";
            string insert = "";
            int Updt = 0;
            string msg = "";
            string rackNo = "";
            string rowNo = "";
            int noOfCopy = 0;
            int noOfCopiesRacRow = 0;
            string book_type = "";
            string qry = "";
            string StrSql = "";
            int LinkA = 0;
            int LinkB = 0;
            int issuedVal = 0;
            string Library = Convert.ToString(ddllibrary.SelectedValue);
            string LibraryName = Convert.ToString(ddllibrary.SelectedItem.Text);
            string Libcode = Convert.ToString(Cbo_CardLibrary.SelectedValue);
            string college = Convert.ToString(ddlcollege.SelectedValue);

            string StrBookType = Convert.ToString(ddlBookType.SelectedValue);
            string cardCriteria = Convert.ToString(ddlCardType.SelectedValue);

            string serverDt = d2.ServerDate();
            string CurDate = serverDt.Split(' ')[0];
            string servertime = d2.ServerTime();
            string Time = servertime;

            string category = Convert.ToString(Session["category"]);
            //SpreadIssuingBook.SaveChanges();
            string issueType = Convert.ToString(ddlissue.SelectedValue);
            string IssueDate = txtissuedate.Text;
            string DueDate = Txtduedate.Text;
            string bookIss_dt = txtissuedate.Text;
            if (txtRollNo.Text == "")
            {
                imgdiv2.Visible = true;
                lbl_alertMsg.Text = "Enter Roll Number";
                return;
            }
            else if (StrSaveRollNo == "")
            {
                txtRollNo_Change(sender, e);
            }
            else if (ddlissue.Text == "")
            {
                imgdiv2.Visible = true;
                lbl_alertMsg.Text = "Select Issue Type";
                return;
            }
            else if (GrdIssuingBook.Rows.Count == 0)
            {
                imgdiv2.Visible = true;
                lbl_alertMsg.Text = "No books are entered";
                return;
            }
            ////if optIssue.value = true And Not ChkIssueDate Then Exit Sub
            if (rblissue.SelectedIndex == 0 || rblissue.SelectedIndex == 2)
            {
                //if txt_issuedate.value = txt_duedate.value Then
                //    InfoMsg "Check Due Date"
                //    Exit Sub
                //End if
            }
            if (issueType == "Book")
                book_type = "BOK";
            if (issueType == "Periodicals")
                book_type = "PER";
            if (issueType == "Project Book")
                book_type = "PRO";
            if (issueType == "Non-Book Material")
                book_type = "NBM";
            if (issueType == "Question Bank")
                book_type = "QBA";
            if (issueType == "Back Volume")
                book_type = "BVO";
            if (issueType == "Reference Books")
                book_type = "REF";
            if (BlnReserveDue == true && IntReserveDueVal > 0)
            {
                AutoReserveCancel();
            }
            string IssuedBy = d2.GetFunction("select Full_Name from usermaster where user_code='" + userCode + "'");

            if (rblissue.SelectedIndex == 0)
            {
                if (GrdIssuingBook.Rows.Count > 0)
                {
                    Sql = "select isnull(count(*),0) as count from borrow where roll_no = '" + StrSaveRollNo + "' and lib_code ='" + Library + "' and return_flag = 0 and due_date <'" + CurDate + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(Sql, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        double count = Convert.ToDouble(ds.Tables[0].Rows[0]["count"]);
                        if (count > 0)
                            msg = count + " Books are Due, Are you sure to issue the book";
                        else
                            msg = "Are you sure to issue the book";
                        string var = Convert.ToString(Btnsave.TabIndex);
                        if (rblissue.SelectedIndex == 0)
                        {
                            string[] dtIssue = IssueDate.Split('/');
                            if (dtIssue.Length == 3)
                                IssueDate = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                            for (int i = 0; i < GrdIssuingBook.Rows.Count; i++)
                            {
                                varAccno = Convert.ToString(GrdIssuingBook.Rows[i].Cells[2].Text);
                                vartitle = Convert.ToString(GrdIssuingBook.Rows[i].Cells[3].Text);
                                varauthor = Convert.ToString(GrdIssuingBook.Rows[i].Cells[4].Text);
                                varCallNo = Convert.ToString(GrdIssuingBook.Rows[i].Cells[5].Text);
                                varcTokenNo = Convert.ToString(GrdIssuingBook.Rows[i].Cells[9].Text);
                                varRDueDay = Convert.ToString(GrdIssuingBook.Rows[i].Cells[7].Text);
                                VarRDueDate = Convert.ToString(GrdIssuingBook.Rows[i].Cells[8].Text);

                                string[] Duedt = VarRDueDate.Split('/');
                                if (Duedt.Length == 3)
                                    VarRDueDate = Duedt[1].ToString() + "/" + Duedt[0].ToString() + "/" + Duedt[2].ToString();

                                Sql = "SELECT * FROM Borrow WHERE Acc_No ='" + varAccno + "' AND Return_Flag = 0 AND Return_Type ='" + book_type + "' AND Lib_Code ='" + Library + "' ";
                                dsload.Clear();
                                dsload = d2.select_method_wo_parameter(Sql, "text");
                                if (dsload.Tables[0].Rows.Count > 0)
                                {
                                    imgdiv2.Visible = true;
                                    lbl_alertMsg.Text = "Access No. " + varAccno + " already issued to Roll No. : " + Convert.ToString(dsload.Tables[0].Rows[0]["Roll_No"]) + " Name : " + Convert.ToString(dsload.Tables[0].Rows[0]["Stud_Name"]);
                                    return;
                                }
                                //=======Reservation Details============================'
                                if (category == "Staff")
                                {
                                    Sql = "update priority_studstaff set cancel_flag=2,PriorityNo=0 where staff_code='" + StrSaveRollNo + "' and access_number='" + varAccno + "'";
                                    update = d2.update_method_wo_parameter(Sql, "text");
                                    Sql = "update priority_studstaff set cancel_flag=2,PriorityNo=0 where staff_code='" + StrSaveRollNo + "' and OtherAcc_no ='" + varAccno + "'";
                                    update = d2.update_method_wo_parameter(Sql, "text");
                                }
                                else if (category == "Student")
                                {
                                    Sql = "update priority_studstaff set cancel_flag=2,PriorityNo=0 where roll_no='" + StrSaveRollNo + "' and access_number='" + varAccno + "'";
                                    update = d2.update_method_wo_parameter(Sql, "text");
                                    Sql = "update priority_studstaff set cancel_flag=2,PriorityNo=0 where roll_no='" + StrSaveRollNo + "' and OtherAcc_no='" + varAccno + "'";
                                    update = d2.update_method_wo_parameter(Sql, "text");
                                }
                                //=======================================================//
                                int issueCnt = Convert.ToInt32(TxtissuedCount.Text) + 1;
                                if (category == "Staff")
                                {
                                    insert = "insert into borrow(acc_no,title,author,call_no,token_no,roll_no,stud_name,is_staff,borrow_date,due_date,return_date,return_type,access_date,access_time,lib_code,return_flag,cirno_issue,book_issuedby,renewflag,renewaltimes,Issued_Time,Returned_Time) values ('" + varAccno + "','" + vartitle + "','" + varauthor + "','" + varCallNo + "','" + varcTokenNo + "','" + StrSaveRollNo + "','" + TxtName.Text + "',1,'" + IssueDate + "','" + VarRDueDate + "','" + CurDate + "','" + book_type + "','" + CurDate + "','" + Time + "','" + Library + "',0," + issueCnt + ",'" + IssuedBy + "',0,0,'" + Time + "','')";
                                }
                                else if (category == "Student")
                                {
                                    insert = "insert into borrow(acc_no,title,author,call_no,token_no,roll_no,stud_name,is_staff,borrow_date,due_date,return_date,return_type,access_date,access_time,lib_code,return_flag,cirno_issue,book_issuedby,renewflag,renewaltimes,Issued_Time,Returned_Time) values ('" + varAccno + "','" + vartitle + "','" + varauthor + "','" + varCallNo + "','" + varcTokenNo + "','" + StrSaveRollNo + "','" + TxtName.Text + "',0,'" + IssueDate + "','" + VarRDueDate + "','" + CurDate + "','" + book_type + "','" + CurDate + "','" + Time + "','" + Library + "',0," + issueCnt + ",'" + IssuedBy + "',0,0,'" + Time + "','')";
                                }
                                else if (category == "Nonmember")
                                {
                                    insert = "insert into borrow(acc_no,title,author,call_no,token_no,roll_no,stud_name,is_staff,borrow_date,due_date,return_date,return_type,access_date,access_time,lib_code,return_flag,cirno_issue,book_issuedby,renewflag,renewaltimes,Issued_Time,Returned_Time) values ('" + varAccno + "','" + vartitle + "','" + varauthor + "','" + varCallNo + "','" + varcTokenNo + "','" + StrSaveRollNo + "','" + TxtName.Text + "',2,'" + IssueDate + "','" + VarRDueDate + "','" + CurDate + "','" + book_type + "','" + CurDate + "','" + Time + "','" + Library + "',0," + issueCnt + ",'" + IssuedBy + "',0,0,'" + Time + "','')";
                                }

                                Updt = d2.update_method_wo_parameter(insert, "text");

                                if (issueType == "Book" || issueType == "Reference Books")
                                    Sql = "UPDATE bookdetails set book_status='Issued' where acc_no='" + varAccno + "' and lib_code='" + Library + "'";
                                else if (issueType == "Periodicals")
                                    Sql = "UPDATE journal set issue_flag ='Issued' where access_code='" + varAccno + "' and lib_code='" + Library + "'";
                                else if (issueType == "Project Book")
                                    Sql = "UPDATE project_book set issue_flag='Issued' where probook_accno='" + varAccno + "' and lib_code='" + Library + "'";
                                else if (issueType == "Non-Book Material")
                                    Sql = "UPDATE nonbookmat set issue_flag='Issued' where nonbookmat_no='" + varAccno + "' and lib_code='" + Library + "'";
                                else if (issueType == "Question Bank")
                                    Sql = "UPDATE university_question set issue_flag='Issued' where access_code='" + varAccno + "' and lib_code='" + Library + "'";
                                else if (issueType == "Back Volume")
                                    Sql = "UPDATE back_volume set issue_flag='Issued' where access_code='" + varAccno + "' and lib_code='" + Library + "'";

                                Updt = d2.update_method_wo_parameter(Sql, "text");

                                Sql = "UPDATE tokendetails set is_locked=1 where token_no='" + varcTokenNo + "' ";
                                Updt = d2.update_method_wo_parameter(Sql, "text");

                                //=====Updation of rack status=============================//

                                Sql = "select * from rack_allocation where lib_code='" + Library + "' and acc_no='" + varAccno + "' and rack_no <>'' and row_no <>'' and book_type='" + book_type + "'";
                                dsCommon.Clear();
                                dsCommon = d2.select_method_wo_parameter(Sql, "text");
                                if (dsCommon.Tables[0].Rows.Count > 0)
                                {
                                    rackNo = d2.GetFunction("select rack_no from rack_allocation where lib_code='" + Library + "'and acc_no='" + varAccno + "' and rack_no <> '' and book_type='" + book_type + "'");
                                    if (rackNo != "")
                                    {
                                        noOfCopy = Convert.ToInt32(d2.GetFunction("select no_of_copies from rack_master where lib_code='" + Library + "'and rack_no='" + rackNo + "'"));
                                        noOfCopy = noOfCopy - 1;
                                    }
                                    else
                                        noOfCopy = 0;

                                    rowNo = d2.GetFunction("select row_no from rack_allocation where lib_code='" + Library + "'and acc_no='" + varAccno + "' and row_no <> '' and book_type='" + book_type + "'");
                                    if (rowNo != "")
                                    {
                                        noOfCopiesRacRow = Convert.ToInt32(d2.GetFunction("select no_of_copies from rackrow_master where lib_code='" + Library + "'and rack_no='" + rackNo + "' and row_no='" + rowNo + "'"));
                                        noOfCopiesRacRow = noOfCopiesRacRow - 1;
                                    }
                                    else
                                        noOfCopiesRacRow = 0;

                                    Sql = "update rack_master set no_of_copies='" + noOfCopy + "' where lib_code='" + Library + "' and rack_no='" + rackNo + "'";
                                    update = d2.update_method_wo_parameter(Sql, "text");
                                    Sql = "update rackrow_master set no_of_copies='" + noOfCopiesRacRow + "' where lib_code='" + Library + "' and rack_no='" + rackNo + "' and row_no='" + rowNo + "'";
                                    update = d2.update_method_wo_parameter(Sql, "text");

                                    qry = "select * from rack_allocation_back where lib_code='" + Library + "' and acc_no='" + varAccno + "' and book_type='" + book_type + "'";
                                    dsload.Clear();
                                    dsload = d2.select_method_wo_parameter(qry, "text");
                                    if (dsload.Tables[0].Rows.Count > 0)
                                    {
                                        Sql = "update rack_allocation_back set rack_no=(select rack_no from rack_allocation where lib_code='" + Library + "' and acc_no='" + varAccno + "' and book_type='" + book_type + "'),row_no=(select row_no from rack_allocation where lib_code='" + Library + "' and acc_no='" + varAccno + "' and book_type='" + book_type + "') where lib_code='" + Library + "' and acc_no='" + varAccno + "' and book_type='" + book_type + "'";
                                        update = d2.update_method_wo_parameter(Sql, "text");
                                    }
                                    else
                                    {
                                        Sql = "insert into rack_allocation_back (lib_code,rack_no,row_no,acc_no,access_date,access_time,book_type,Pos_No,Pos_Place)  select lib_code,rack_no,row_no,acc_no,access_date,access_time,book_type,Pos_No,Pos_Place from rack_allocation where lib_code='" + Library + "' and acc_no='" + varAccno + "' and book_type='" + book_type + "'";
                                        update = d2.update_method_wo_parameter(Sql, "text");
                                    }
                                    Sql = "update rack_allocation set rack_no='',row_no='' where lib_code='" + Library + "' and acc_no='" + varAccno + "' and book_type='" + book_type + "'";
                                    update = d2.update_method_wo_parameter(Sql, "text");
                                }

                                issuedVal = Convert.ToInt32(txt_issued.Text) + 1;
                                int issueTotCnt = Convert.ToInt32(TxtissuedCount.Text) + 1;
                            }
                            if (issuedVal == Convert.ToInt32(txt_elgi.Text))
                            {
                                DispStatusList();
                                imgdiv2.Visible = true;
                                lbl_alertMsg.Text = "Issue details Saved";
                                ClearFunction();
                                img_stud1.ImageUrl = "";
                                imgBook.ImageUrl = "";
                                TotIssuedCount();
                                hsAccNo.Clear();
                            }
                            else
                            {
                                DispStatusList();
                                if (Convert.ToInt32(GrdIssuingBook.Rows.Count) >= 1 && rblissue.SelectedIndex != 3)
                                {
                                    if (IntDispIssueMes == 1)
                                    {
                                        var = Convert.ToString(BtnYes.TabIndex);
                                        if (var == "2")
                                        {
                                            BtnIssueNoAgain.Focus();
                                            BtnIssueNoAgain.BackColor = Color.LightGreen;
                                        }
                                        DivIssue.Visible = true;
                                        LblIssuesName.Text = "Issue details Saved.Do you want any other books?";
                                        hsAccNo.Clear();
                                        return;
                                    }
                                    else
                                    {
                                        if (rblissue.SelectedIndex == 0)
                                        {
                                        }
                                        else
                                        {
                                        }
                                        Txtaccno.Text = "";
                                        return;
                                    }
                                }
                            }
                            DivMess.Visible = false;
                        }
                    }
                }
            }


            else if (rblissue.SelectedIndex == 1 || rblissue.SelectedIndex == 3)
            {
                if (rblissue.SelectedIndex == 1)
                {
                    if (RblMemType.SelectedIndex == 0)
                        StrMemberType = "Student";
                    if (RblMemType.SelectedIndex == 1)
                        StrMemberType = "Staff";
                    else
                        StrMemberType = "Member";

                    if (IntDispMess == 1)
                    {
                        DivMess.Visible = true;
                        string var = Convert.ToString(Btnsave.TabIndex);
                        cmdReturn_Click();//kowshi
                    }
                    else
                    {
                        if (RblMemType.SelectedIndex == 0)
                            StrMemberType = "Student";
                        else if (RblMemType.SelectedIndex == 1)
                            StrMemberType = "Staff";
                        else
                            StrMemberType = "Member";
                        cmdReturn_Click();
                    }
                }
                if (rblissue.SelectedIndex == 3)
                {
                    if (RblMemType.SelectedIndex == 0)
                        StrMemberType = "Student";
                    else if (RblMemType.SelectedIndex == 1)
                        StrMemberType = "Staff";
                    else
                        StrMemberType = "Member";
                    msg = "Are you sure to Lost the Book ?";
                    string var = Convert.ToString(Btnsave.TabIndex);
                    cmdReturn_Click();
                    return;
                }
            }
            else if (rblissue.SelectedIndex == 2)
            {
                if (RblMemType.SelectedIndex == 0)
                    StrMemberType = "Student";
                else if (RblMemType.SelectedIndex == 1)
                    StrMemberType = "Staff";
                else
                    StrMemberType = "Member";
                for (int i = 0; i < GrdIssuingBook.Rows.Count; i++)
                {
                    varAccno = Convert.ToString(GrdIssuingBook.Rows[i].Cells[2].Text);
                    vartitle = Convert.ToString(GrdIssuingBook.Rows[i].Cells[3].Text);
                    varauthor = Convert.ToString(GrdIssuingBook.Rows[i].Cells[4].Text);
                    varCallNo = Convert.ToString(GrdIssuingBook.Rows[i].Cells[5].Text);
                    varcTokenNo = Convert.ToString(GrdIssuingBook.Rows[i].Cells[9].Text);
                    varRDueDay = Convert.ToString(GrdIssuingBook.Rows[i].Cells[7].Text);
                    VarRDueDate = Convert.ToString(GrdIssuingBook.Rows[i].Cells[8].Text);

                    qry = "select * from borrow where acc_no='" + varAccno + "' and return_type='" + book_type + "' AND ROLL_NO='" + txtRollNo.Text + "' and return_flag=0  ";
                    dsload.Clear();
                    dsload = d2.select_method_wo_parameter(qry, "text");
                    if (dsload.Tables[0].Rows.Count > 0)
                    {
                        qry = "select * from inssettings where LinkName='Renewal Permission' and College_Code=" + college + "";
                        dsCommon.Clear();
                        dsCommon = d2.select_method_wo_parameter(qry, "text");
                        string[] Linkarr = Convert.ToString(dsCommon.Tables[0].Rows[0]["LinkValue"]).Split('/');
                        if (Linkarr.Length > 0)
                            a = Linkarr[0];

                        Sql = "SELECT ISNULL(Renew_Days,0) as count FROM TokenDetails WHERE Roll_No='" + txtRollNo.Text + "' ";
                        if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                            Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                        else if (BlnBookBankLib == true && BlnBookBankAll == true)
                            Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                        else
                            Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                        if (Cbo_CardLibrary.SelectedItem.Text != "All")
                            Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                        else if (Cbo_CardLibrary.SelectedItem.Text == "All")
                            Sql += "AND ISNULL(TransLibCode,'All') ='All'";

                        if (ddlBookType.SelectedItem.Text != "All")
                            Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                        else
                            Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                        if (cardCriteria != "All")
                            Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                        else
                            Sql += "AND ISNULL(CardCat,'All') ='All' ";
                        string RenewCnt = d2.GetFunction(Sql);
                        if (!string.IsNullOrEmpty(RenewCnt))
                        {
                            LinkB = Convert.ToInt32(RenewCnt);
                        }
                        if (LinkB == 0)
                        {
                            qry = "select * from inssettings where LinkName='Renewal Permission' and College_Code=" + college + "";
                            dsDispCard.Clear();
                            dsDispCard = d2.select_method_wo_parameter(qry, "text");
                            string[] arr = Convert.ToString(dsDispCard.Tables[0].Rows[0]["LinkValue"]).Split('/');
                            if (arr.Length > 0)
                            {
                                LinkA = Convert.ToInt32(arr[0]);
                                LinkB = Convert.ToInt32(arr[1]);
                            }
                        }
                        if (LinkA == 1 && LinkB > 0)
                        {
                            int var = 0;
                            var = Convert.ToInt32(d2.GetFunction("select isnull(max(renewaltimes),0) renewaltimes from borrow where acc_no='" + varAccno + "' and return_type='" + book_type + "' AND ROLL_NO='" + txtRollNo.Text + "' "));
                            var = var + 1;
                            if (var <= Convert.ToInt32(LinkB))
                            { }
                            else
                            {
                                imgdiv2.Visible = true;
                                lbl_alertMsg.Text = "Your Renewal Count has been Expired";
                                //Command2.value = true
                                Txtaccno.Text = "";
                                rblissue.SelectedIndex = 0;
                                return;
                            }
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alertMsg.Text = "Can't renewal the book, give the renewal permission";
                            Txtaccno.Text = "";
                            rblissue.SelectedIndex = 0;
                            return;
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alertMsg.Text = "No book to renewal";
                        Txtaccno.Text = "";
                        rblissue.SelectedIndex = 0;
                        return;
                    }
                }
                if (IntDispMess == 1)
                {
                    cmdRenewal_Click(sender, e);
                }
                else
                {
                    cmdRenewal_Click(sender, e);
                }
            }
            DivMess.Visible = false;
            SureYes = false;
            SureYestToIssueBook = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "bookissue");
        }
    }

    protected void ChkCancel_OnCheckedChanged(object sender, EventArgs e)
    {
        if (ChkCancel.Checked == true)
        {
            lblReason.Visible = true;
            Btnaddd.Visible = true;
            ddl_Reason.Visible = true;
            Btndelete.Visible = true;
        }
        else
        {
            lblReason.Visible = false;
            Btnaddd.Visible = false;
            ddl_Reason.Visible = false;
            Btndelete.Visible = false;
        }
    }

    #region Auto Receipt Generation & Auto_Cardlock && AutoReserveCancel

    protected void Auto_ReceiptNo()
    {
        try
        {
            string codeno = "";
            string codeno1 = "";
            string Sql = "";
            string libCode = Convert.ToString(ddllibrary.SelectedValue);
            string college = Convert.ToString(ddlcollege.SelectedValue);
            DateTime curDate = DateTime.Now;
            Sql = "SELECT ISNULL(Rcpt_Acr,0) Rcpt_Acr,ISNULL(Rcpt_StNo,0) Rcpt_StNo,ISNULL(Rcpt_Size,0) Rcpt_Size,ISNULL(Rcpt_LastNo,1) Rcpt_LastNo FROM LibCode_Settings WHERE Lib_Code ='" + libCode + "' AND College_Code =" + college + " AND '" + curDate + "' >= FromDate AND Latestrec = 1 ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                codeno = Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_LastNo"]);
                if (Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Size"]) == "1")
                {
                    codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Acr"]) + codeno;
                }
                else if (Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Size"]) == "2")
                {
                    if (Convert.ToInt32(codeno) < 10)
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Acr"]) + "0" + codeno;
                    else
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Acr"]) + codeno;
                }
                else if (Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Size"]) == "3")
                {
                    if (Convert.ToInt32(codeno) < 99)
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Acr"]) + codeno;
                    else if (Convert.ToInt32(codeno) > 9 && Convert.ToInt32(codeno) < 100)
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Acr"]) + "0" + codeno;
                    else if (Convert.ToInt32(codeno) > 0 && Convert.ToInt32(codeno) < 10)
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Acr"]) + "00" + codeno;

                }
                else if (Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Size"]) == "4")
                {
                    if (Convert.ToInt32(codeno) > 999)
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Acr"]) + codeno;
                    else if (Convert.ToInt32(codeno) > 99 && Convert.ToInt32(codeno) < 1000)
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Acr"]) + "0" + codeno;
                    else if (Convert.ToInt32(codeno) > 9 && Convert.ToInt32(codeno) < 100)
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Acr"]) + "00" + codeno;
                    else if (Convert.ToInt32(codeno) > 0 && Convert.ToInt32(codeno) < 10)
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Acr"]) + "000" + codeno;
                }
                else if (Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Size"]) == "5")
                {
                    if (Convert.ToInt32(codeno) > 9999)
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Acr"]) + codeno;
                    else if (Convert.ToInt32(codeno) > 999 && Convert.ToInt32(codeno) < 10000)
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Acr"]) + "0" + codeno;
                    else if (Convert.ToInt32(codeno) > 99 && Convert.ToInt32(codeno) < 1000)
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Acr"]) + "00" + codeno;
                    else if (Convert.ToInt32(codeno) > 9 && Convert.ToInt32(codeno) < 100)
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Acr"]) + "000" + codeno;
                    else if (Convert.ToInt32(codeno) > 0 && Convert.ToInt32(codeno) < 10)
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Acr"]) + "0000" + codeno;
                }
                else if (Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Size"]) == "6")
                {
                    if (Convert.ToInt32(codeno) > 99999)
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Acr"]) + codeno;
                    else if (Convert.ToInt32(codeno) > 9999 && Convert.ToInt32(codeno) < 100000)
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Acr"]) + "0" + codeno;
                    else if (Convert.ToInt32(codeno) > 999 && Convert.ToInt32(codeno) < 10000)
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Acr"]) + "00" + codeno;
                    else if (Convert.ToInt32(codeno) > 99 && Convert.ToInt32(codeno) < 1000)
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Acr"]) + "000" + codeno;
                    else if (Convert.ToInt32(codeno) > 9 && Convert.ToInt32(codeno) < 100)
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Acr"]) + "0000" + codeno;
                    else if (Convert.ToInt32(codeno) > 0 && Convert.ToInt32(codeno) < 10)
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["Rcpt_Acr"]) + "00000" + codeno;

                }
                //txt_recptno.Text = codeno1;
                Txt_CurRcptNo.Text = codeno;
            }
            else
            {
                Sql = "select max(SUBSTRING(receipt_no, 4, LEN(receipt_no)-3)) as receiptNo from fine_details where lib_code ='" + libCode + "' and isnumeric(receipt_no)=0 and receipt_no<>'' and receipt_no is not null ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(Sql, "text");
                int RecptNo = Convert.ToInt32(ds.Tables[0].Rows[0]["receiptNo"]);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //if (RecptNo != 0) // If rs(0) <> 0 And IsNull(rs(0)) = False Then  
                    // txt_recptno.Text = "LIB" + RecptNo + 1;
                    // else
                    //txt_recptno.Text = "LIB1001";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void Auto_Cardlock()
    {
        try
        {
            string strlockno = "";
            string Sql = "";
            string bokcount = "";
            string colCode = Convert.ToString(ddlcollege.SelectedValue);
            Sql = "SELECT * FROM inssettings WHERE LinkName ='Automatic Card Lock' AND College_Code =" + colCode + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string[] strLock = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]).Split('/');
                if (strLock.Length > 0)
                    strlockno = strLock[1];
            }
            bokcount = d2.GetFunction("SELECT COUNT(*) as count FROM Borrow WHERE Return_Flag = 0 AND Roll_No ='" + StrSaveRollNo + "' and datediff(day,due_date,getdate()) > 0");
            if (bokcount == "0")
            {
                Sql = "update tokendetails set is_locked = '0',reas_loc = '',locked_by = '' where is_locked = 2 and reas_loc = 'Auto Lock' and locked_by = 'Auto'and (Roll_no='" + StrSaveRollNo + "' OR Roll_No ='" + StrSaveLibID + "') ";

                update = d2.update_method_wo_parameter(Sql, "text");
            }
            Sql = "update tokendetails set is_locked = '2',reas_loc = 'Auto Lock',locked_by = 'Auto' where is_locked = 0 and roll_no in (select roll_No from borrow where return_flag = 0 and datediff(day,due_date,getdate()) > 0 group by roll_no having count(roll_no) >=" + strlockno + ") ";
            update = d2.update_method_wo_parameter(Sql, "text");

            Sql = "update tokendetails set is_locked = '0',reas_loc = '',locked_by = '' where is_locked = 2 and reas_loc = 'Auto Lock' and locked_by = 'Auto' and roll_no in (select roll_No from borrow where return_flag = 0 and datediff(day,due_date,getdate()) > 0 group by roll_no having count(roll_no) <" + strlockno + ") ";
            update = d2.update_method_wo_parameter(Sql, "text");
        }
        catch (Exception ex)
        {
        }
    }

    protected void AutoReserveCancel()
    {
        string Sql = "";
        string time = "";
        time = System.DateTime.Now.ToString("hh:mm:ss");
        string date = System.DateTime.Now.ToString("mm/dd/yyyy");
        string colCode = Convert.ToString(ddlcollege.SelectedValue);
        Sql = "SELECT Access_Number,Returned_Time,Max(Return_Date) Return_Date FROM Priority_StudStaff P,Borrow B,Library L WHERE P.Access_Number = B.Acc_No AND P.Lib_Code = L.Lib_Code AND Cancel_Flag = 0 AND L.College_Code =" + colCode + " AND Access_Number NOT IN (SELECT Acc_No FROM Borrow R,Library B WHERE R.Lib_Code = B.Lib_Code AND Return_Flag = 0 AND L.College_Code =" + colCode + ") AND Return_Date = (SELECT MAX(Return_Date) FROM Borrow R,Library Y WHERE R.Lib_Code = Y.Lib_Code AND Y.College_Code =" + colCode + ") GROUP BY Access_Number,Returned_Time ";
        ds.Clear();
        ds = d2.select_method_wo_parameter(Sql, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                if (IntReserveDueType == 0)//hour
                {
                    Sql = "UPDATE Priority_StudStaff SET Cancel_Flag = 1,Can_Reason ='Auto Cancel' WHERE Access_Number ='" + Convert.ToString(ds.Tables[0].Rows[0]["Access_Number"]) + "' AND Cancel_Flag = 0  AND '" + time + "' > DateAdd(hh," + IntReserveDueVal + ",'" + Convert.ToString(ds.Tables[0].Rows[0]["Returned_Time"]) + "')";
                    update = d2.update_method_wo_parameter(Sql, "text");
                }
                else  //day
                {
                    Sql = "UPDATE Priority_StudStaff SET Cancel_Flag = 1,Can_Reason ='Auto Cancel' WHERE Access_Number ='" + Convert.ToString(ds.Tables[0].Rows[0]["Access_Number"]) + "' AND Cancel_Flag = 0  AND '" + date + "' >= DateAdd(day," + IntReserveDueVal + ",'" + Convert.ToString(ds.Tables[0].Rows[0]["Return_Date"]) + "')";
                    update = d2.update_method_wo_parameter(Sql, "text");
                }
            }
        }

    }

    #endregion

    protected void Btnclose_Click(object sender, EventArgs e)
    {
    }

    #region Default Library

    protected void btn_DeleteDefYes_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = "";
            string Set_descrip = "frmissue_return;Cbo_CardLibrary";
            string Def_Value = Convert.ToString(Cbo_CardLibrary.SelectedItem.Text);
            int insert = 0;
            string collCode = Convert.ToString(ddlcollege.SelectedValue);
            sql = "select * from setdefault where descrip='" + Set_descrip + "' and college_code =" + collCode + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(sql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                sql = "if exists(select setfor from setdefault where College_Code='" + collCode + "') update setdefault set setfor='" + Def_Value + "' else insert into setdefault(descrip,setfor,is_set,lib_code,College_Code) values('" + Set_descrip + "','" + Def_Value + "',1,'','" + collCode + "') ";
                insert = d2.update_method_wo_parameter(sql, "TEXT");
                imgdiv2.Visible = true;
                lbl_alertMsg.Text = "Default Set";
            }
            SureDivSetDefault.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void btn_DeleteDefNo_Click(object sender, EventArgs e)
    {
        SureDivSetDefault.Visible = false;
    }

    #endregion

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;

        if (GrdIssuingBook.Rows.Count > 0)
        {
            for (int issBkRowCnt = 0; issBkRowCnt < GrdIssuingBook.Rows.Count; issBkRowCnt++)
            {
                GrdIssuingBook.Rows[issBkRowCnt].Visible = false;
            }
        }
        if (GrdBookInHand.Rows.Count > 0)
        {
            for (int BkInHandRowCnt = 0; BkInHandRowCnt < GrdBookInHand.Rows.Count; BkInHandRowCnt++)
            {
                GrdBookInHand.Rows[BkInHandRowCnt].Visible = false;
            }
        }
        if (grdReservation.Rows.Count > 0)
        {
            for (int ReservationRowCnt = 0; ReservationRowCnt < grdReservation.Rows.Count; ReservationRowCnt++)
            {
                grdReservation.Rows[ReservationRowCnt].Visible = false;
            }
        }
        //SpreadIssuingBook.Sheets[0].RowCount = 0;
        //SpreadBookInHand.Sheets[0].RowCount = 0;
        txtRollNo.Text = "";
        TxtName.Text = "";
        txtDept.Text = "";
        Txtaccno.Text = "";
        ddlcodenumber.Items.Clear();
        txt_elgi.Text = "";
        txt_issued.Text = "";
        txt_Unlocked.Text = "";
        string serverDt = d2.ServerDate();
        string[] dat = serverDt.Split('/');
        if (dat.Length == 3)
            serverDt = dat[1] + '/' + dat[0] + '/' + dat[2];
        txtissuedate.Text = serverDt.Split(' ')[0];
        Txtduedate.Text = serverDt.Split(' ')[0];
        rblissue.SelectedIndex = 0;
        ddlissue.Enabled = true;
        txtlocked.Text = "";
        Page.Form.DefaultFocus = txtRollNo.ClientID;
        LostAndFineDiv.Visible = false;
    }

    //Confirmation for issuing another book after saving the details

    protected void btnIssueYes_Click(object sender, EventArgs e)
    {
        Txtaccno.Text = "";
        //Page.Form.DefaultFocus = Txtaccno.ClientID;
        LoadBooksHand(blncomm, BlnBookBankLib, BlnBookBankAll, StrSaveRollNo, StrSaveLibID);
        DispCardStatus(BlnBookBankLib, blncomm, BlnBookBankAll, StrSaveRollNo, StrSaveLibID);
        dtIssuingBook.Columns.Add("Title", typeof(string));
        dtIssuingBook.Columns.Add("Author", typeof(string));
        dtIssuingBook.Columns.Add("Call No", typeof(string));
        dtIssuingBook.Columns.Add("Date Of Issue", typeof(string));
        dtIssuingBook.Columns.Add("Due Days", typeof(string));
        dtIssuingBook.Columns.Add("Due Date", typeof(string));
        dtIssuingBook.Columns.Add("Token No", typeof(string));
        dtIssuingBook.Columns.Add("Fine", typeof(string));
        GrdIssuingBook.DataSource = dtIssuingBook;
        GrdIssuingBook.DataBind();
        GrdIssuingBook.Visible = true;
        ViewState["CurrentTable"] = null;
        DivIssue.Visible = false;
        firstRow = false;
        DispStatusList();
    }

    protected void btnIssueNo_Click(object sender, EventArgs e)
    {
        ClearFunction();
        ViewState["CurrentTable"] = null;
        DivMess.Visible = false;
        DivIssue.Visible = false;
        dtIssuingBook.Columns.Add("Access No", typeof(string));
        dtIssuingBook.Columns.Add("Title", typeof(string));
        dtIssuingBook.Columns.Add("Author", typeof(string));
        dtIssuingBook.Columns.Add("Call No", typeof(string));
        dtIssuingBook.Columns.Add("Date Of Issue", typeof(string));
        dtIssuingBook.Columns.Add("Due Days", typeof(string));
        dtIssuingBook.Columns.Add("Due Date", typeof(string));
        dtIssuingBook.Columns.Add("Token No", typeof(string));
        dtIssuingBook.Columns.Add("Fine", typeof(string));
        GrdIssuingBook.DataSource = dtIssuingBook;
        GrdIssuingBook.DataBind();
        GrdIssuingBook.Visible = true;
        dtBooksInHand.Columns.Add("SNo", typeof(string));
        dtBooksInHand.Columns.Add("Access No", typeof(string));
        dtBooksInHand.Columns.Add("Title", typeof(string));
        dtBooksInHand.Columns.Add("Author", typeof(string));
        dtBooksInHand.Columns.Add("Issue Date", typeof(string));
        dtBooksInHand.Columns.Add("Due Date", typeof(string));
        dtBooksInHand.Columns.Add("Department", typeof(string));
        dtBooksInHand.Columns.Add("Token No", typeof(string));
        dtBooksInHand.Columns.Add("Fine", typeof(string));
        dtBooksInHand.Columns.Add("Library", typeof(string));
        dtBooksInHand.Columns.Add("Book Type", typeof(string));
        GrdBookInHand.DataSource = dtBooksInHand;
        GrdBookInHand.DataBind();
        GrdBookInHand.Visible = true;
        DispStatusList();
    }

    //======================================================//

    protected void Cal_fine(int IntCurRow)
    {
        try
        {
            DataSet rsFine = new DataSet();
            int intStudDegCode = 0;
            string StrBatchYear = "";
            int intCurSem = 0;
            string StrTokCode = "";
            string StrSplitDate = "";
            DateTime StrDueDate = new DateTime();
            double DblFineAmt = 0;
            double FineAmt = 0;
            double DueDays = 0;
            double today = 0;
            double no = 0;
            double no1 = 0;
            string Sql = string.Empty;
            string Libcode = Convert.ToString(Cbo_CardLibrary.SelectedValue);
            string StrBookType = Convert.ToString(ddlBookType.SelectedValue);
            string cardCriteria = Convert.ToString(ddlCardType.SelectedValue);
            string library = Convert.ToString(ddllibrary.SelectedValue);
            string serverDt = d2.ServerDate();
            string issDate = txtissuedate.Text;
            string DuDate = Txtduedate.Text;

            string[] dtIssue = issDate.Split('/');
            if (dtIssue.Length == 3)
                issDate = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
            string BookDueDate = Txtduedate.Text;

            string[] dtdueDATE = DuDate.Split('/');
            if (dtdueDATE.Length == 3)
                DuDate = dtdueDATE[1].ToString() + "/" + dtdueDATE[0].ToString() + "/" + dtdueDATE[2].ToString();
            string displayissdate = "";
            VisibleFineDet();
            //if (chkAutoReceipt.Checked)
            //{
            //    Auto_ReceiptNo();
            //}
            if (RblMemType.SelectedIndex == 0)
            {
                Sql = "SELECT Course_ID,Dept_Code,R.Degree_Code,Batch_Year,Current_Semester FROM Registration R,Degree G WHERE R.Degree_Code = G.Degree_Code AND (Roll_No ='" + txtRollNo.Text + "' OR Lib_ID='" + txtRollNo.Text + "' )";
                rsFine.Clear();
                rsFine = d2.select_method_wo_parameter(Sql, "text");
                if (rsFine.Tables[0].Rows.Count > 0)
                {
                    intStudDegCode = Convert.ToInt32(rsFine.Tables[0].Rows[0]["Degree_Code"]);
                    StrBatchYear = Convert.ToString(rsFine.Tables[0].Rows[0]["Batch_Year"]);
                    StrTokCode = Convert.ToString(rsFine.Tables[0].Rows[0]["Course_ID"]) + "~" + Convert.ToString(rsFine.Tables[0].Rows[0]["Dept_Code"]);
                    intCurSem = Convert.ToInt32(rsFine.Tables[0].Rows[0]["Current_Semester"]);

                    Sql = "SELECT ISNULL(Fine,0) Fine,ISNULL(OverNightFine,0) OverNightFine from lib_master where code='" + StrTokCode + "' and batch_year='" + StrBatchYear + "' AND Is_Staff = 0 ";
                    if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                        Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                    else if (BlnBookBankLib == true && BlnBookBankAll == true)
                        Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                    else
                        Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                    if (Cbo_CardLibrary.SelectedItem.Text != "All")
                        Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                    else if (Cbo_CardLibrary.SelectedItem.Text == "All")
                        Sql += "AND ISNULL(TransLibCode,'All') ='All'";

                    if (ddlBookType.SelectedItem.Text != "All")
                        Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                    else
                        Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                    if (cardCriteria != "All")
                        Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                    else
                        Sql += "AND ISNULL(CardCat,'All') ='All' ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(Sql, "text");
                    displayissdate = Convert.ToString(GrdBookInHand.Rows[IntCurRow - 1].Cells[4].Text);
                    string[] displaydtIssue = displayissdate.Split('/');
                    if (displaydtIssue.Length == 3)
                        displayissdate = displaydtIssue[1].ToString() + "/" + displaydtIssue[0].ToString() + "/" + displaydtIssue[2].ToString();

                    string displayduedate = Convert.ToString(GrdBookInHand.Rows[IntCurRow - 1].Cells[5].Text); ; //Convert.ToString(SpreadBookInHand.Sheets[0].Cells[0, 5].Text);
                    string[] displaydueIssue = displayduedate.Split('/');
                    if (displaydueIssue.Length == 3)
                        displayduedate = displaydueIssue[1].ToString() + "/" + displaydueIssue[0].ToString() + "/" + displaydueIssue[2].ToString();

                    DateTime currendate = Convert.ToDateTime(serverDt.Split(' ')[0]);
                    DateTime DDate = Convert.ToDateTime(displayduedate);

                    TimeSpan diff = currendate.Subtract(DDate);
                    int Datedifference = diff.Days;

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToInt32(Datedifference) == 1)
                            DblFineAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["OverNightFine"]);
                        else
                            DblFineAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["Fine"]);
                    }
                }
            }
            else if (RblMemType.SelectedIndex == 1)
            {
                Sql = "SELECT ISNULL(Fine,0) Fine,ISNULL(OverNightFine,0) OverNightFine from lib_master where code='" + StrSaveRollNo + "' AND Is_Staff = 1 ";
                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else
                    Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else if (Cbo_CardLibrary.SelectedItem.Text == "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";

                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";

                ds.Clear();
                ds = d2.select_method_wo_parameter(Sql, "text");

                displayissdate = Convert.ToString(GrdBookInHand.Rows[IntCurRow - 1].Cells[4].Text);
                string[] displaydtIssue = displayissdate.Split('/');
                if (displaydtIssue.Length == 3)
                    displayissdate = displaydtIssue[1].ToString() + "/" + displaydtIssue[0].ToString() + "/" + displaydtIssue[2].ToString();

                string displayduedate = Convert.ToString(GrdBookInHand.Rows[IntCurRow - 1].Cells[5].Text);
                string[] displaydueIssue = displayduedate.Split('/');
                if (displaydueIssue.Length == 3)
                    displayduedate = displaydueIssue[1].ToString() + "/" + displaydueIssue[0].ToString() + "/" + displaydueIssue[2].ToString();

                DateTime currendate = Convert.ToDateTime(serverDt.Split(' ')[0]);
                DateTime DDate = Convert.ToDateTime(displayduedate);

                TimeSpan diff = currendate.Subtract(DDate);
                int Datedifference = diff.Days;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(Datedifference) == 1)
                        DblFineAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["OverNightFine"]);
                    else
                        DblFineAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["Fine"]);
                }
            }
            //For Day Calculation
            double Totalduedays = 0;
            for (int RowCnt = 0; RowCnt < GrdIssuingBook.Rows.Count; RowCnt++)
            {
                VarF_AccNo = Convert.ToString(GrdIssuingBook.Rows[RowCnt].Cells[2].Text);
                VarF_Days = Convert.ToString(GrdIssuingBook.Rows[RowCnt].Cells[7].Text);
                VarF_DueDate = Convert.ToString(GrdIssuingBook.Rows[RowCnt].Cells[8].Text);
                DueDays = Convert.ToDouble(VarF_Days);

                if (Convert.ToInt32(VarF_Days) > 0)
                {

                    string[] displaydtDue = VarF_DueDate.Split('/');
                    if (displaydtDue.Length == 3)
                        VarF_DueDate = displaydtDue[1].ToString() + "/" + displaydtDue[0].ToString() + "/" + displaydtDue[2].ToString();
                    if (BlnExcHoliday == true)
                    {
                        if (BlnLibHol == true)
                        {
                            Sql = "SELECT ISNULL(COUNT(*),0) TotHolDays FROM Holiday_Library WHERE Holiday_Date >='" + VarF_DueDate + "' AND Holiday_Date <='" + serverDt + "' AND Lib_Code ='" + library + "' ";
                        }
                        else
                        {
                            Sql = "SELECT ISNULL(COUNT(*),0) TotHolDays FROM HolidayStudents WHERE Degree_Code=" + intStudDegCode + " AND Semester =" + intCurSem + " AND HalfOrFull=0 AND Holiday_Date >='" + VarF_DueDate + "' AND Holiday_Date <='" + serverDt + "' ";
                        }
                        dsload.Clear();
                        dsload = d2.select_method_wo_parameter(Sql, "text");
                        if (dsload.Tables[0].Rows.Count > 0)
                        {
                            DueDays = DueDays - Convert.ToDouble(dsload.Tables[0].Rows[0]["TotHolDays"]);
                        }
                    }
                    if (nocal == 1)
                    {
                        if (DueDays > 0)
                        {
                            GrdIssuingBook.Rows[RowCnt].Cells[7].Text = Convert.ToString(DueDays);
                            FineAmt = DblFineAmt * DueDays;
                            GrdIssuingBook.Rows[RowCnt].Cells[10].Text = Convert.ToString(FineAmt);
                            txt_days.Text = Convert.ToString(DueDays);
                            txt_amount.Text = Convert.ToString(FineAmt);
                            Txt_ActAmount.Text = txt_amount.Text;
                        }
                    }
                }
            }
            //For Week Calculation
            if (DblFineAmt == 0)
            {
                DueDays = Convert.ToDouble(VarF_Days);
                if (Convert.ToInt32(VarF_Days) > 0)
                {
                    if (RblMemType.SelectedIndex == 0)
                    {
                        Sql = "SELECT * FROM ExamFinems WHERE Degree_Code=" + intStudDegCode + " AND Semester =" + StrBatchYear + " AND ExmFine = 2 ORDER BY FromDay ";
                    }
                    else if (RblMemType.SelectedIndex == 1)
                    {
                        Sql = "SELECT * FROM ExamFinems WHERE Category_Code ='" + txtRollNo.Text + "' AND ExmFine = 2 ORDER BY FromDay ";
                    }
                    rsFine.Clear();
                    rsFine = d2.select_method_wo_parameter(Sql, "text");
                    if (rsFine.Tables[0].Rows.Count > 0)
                    {
                        for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                        {
                            if (Convert.ToString(rsFine.Tables[0].Rows[m]["ToDay"]) != "0")
                            {
                                if (DueDays >= Convert.ToDouble(rsFine.Tables[0].Rows[m]["Fromday"]) && DueDays >= Convert.ToDouble(rsFine.Tables[0].Rows[m]["ToDay"]))
                                {
                                    today = Convert.ToDouble(rsFine.Tables[0].Rows[m]["ToDay"]);
                                    no = Convert.ToDouble(rsFine.Tables[0].Rows[m]["ToDay"]) - (Convert.ToDouble(rsFine.Tables[0].Rows[m]["Fromday"]) + 1);

                                    DblFineAmt = DblFineAmt + no * Convert.ToDouble(rsFine.Tables[0].Rows[m]["FineAmount"]);
                                    FineAmt = Convert.ToDouble(rsFine.Tables[0].Rows[m]["FineAmount"]);
                                }
                                else if (DueDays >= Convert.ToDouble(rsFine.Tables[0].Rows[m]["fromday"]) && DueDays <= Convert.ToDouble(rsFine.Tables[0].Rows[m]["Today"]))
                                {
                                    today = Convert.ToDouble(rsFine.Tables[0].Rows[m]["ToDay"]);
                                    no = Convert.ToDouble(rsFine.Tables[0].Rows[m]["ToDay"]) - DueDays + 1;
                                    DblFineAmt = DblFineAmt + no * Convert.ToDouble(rsFine.Tables[0].Rows[m]["FineAmount"]);
                                }
                            }
                            else
                            {
                                if (DueDays > Convert.ToDouble(rsFine.Tables[0].Rows[m]["fromday"]))
                                    DblFineAmt = DueDays * Convert.ToDouble(rsFine.Tables[0].Rows[m]["FineAmount"]);
                            }
                        }
                        if (DueDays > today)
                        {
                            no1 = today - DueDays;
                            DblFineAmt = DblFineAmt + no1 * FineAmt;
                        }
                        if (nocal == 1)
                        {
                            FineAmt = DblFineAmt * DueDays;
                            txt_days.Text = Convert.ToString(DueDays);
                            txt_amount.Text = Convert.ToString(DblFineAmt);
                            Txt_ActAmount.Text = txt_amount.Text;
                        }
                    }
                }
            }
            txt_TotalDue.Text = "0";
            int rowcount = GrdIssuingBook.Rows.Count;
            for (int i = 0; i < rowcount; i++)
            {
                Var_Fine = Convert.ToString(GrdIssuingBook.Rows[i].Cells[10].Text);
                double fine = 0;
                if (Var_Fine != "")
                    fine = Convert.ToDouble(Var_Fine);
                double Total = Convert.ToDouble(txt_TotalDue.Text);
                double FineTotalDue = Total + fine;
                txt_TotalDue.Text = Convert.ToString(FineTotalDue);
            }
            if (rblissue.SelectedIndex == 3)
            {
                LostAndFineDiv.Visible = true;
                rbfine.Visible = true;
                tdfine.Visible = true;
                Tdfinecnl.Visible = false;
            }
            if (IntCancelFine == true)
                ChkCancel.Visible = true;
            else
                ChkCancel.Visible = false;

            //chkpaid.Checked = true;

            if (rblissue.SelectedIndex == 2)
            {
                DateTime DDate = Convert.ToDateTime(DuDate);
                DateTime Issdate = Convert.ToDateTime(issDate);
                TimeSpan diff = DDate.Subtract(Issdate);

                int Datedifference = diff.Days + 1;
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collcode, "bookissue.aspx");
        }
    }

    protected void GrdIssuingBook_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    System.Web.UI.WebControls.CheckBox cb = new System.Web.UI.WebControls.CheckBox();
        //    e.Row.Cells[10].Controls.Add(cb);
        //}
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    string staffOrStudent = Convert.ToString(e.Row.Cells[2].Text);
        //    string course = Convert.ToString(e.Row.Cells[5].Text);
        //    string degree = Convert.ToString(e.Row.Cells[4].Text);
        //    string batch = Convert.ToString(e.Row.Cells[6].Text);
        //    string rollNo = Convert.ToString(e.Row.Cells[9].Text);
        //    double Duedays = Convert.ToDouble(e.Row.Cells[13].Text);
        //    string fine = "";
        //    if (staffOrStudent.ToLower() == "student")
        //    {
        //        fine = GetFunction("select fine from lib_master where code='" + course + "~" + degree + "' and batch_year='" + batch + "'");
        //    }
        //    else
        //    {
        //        fine = GetFunction("select fine from lib_master where code='" + rollNo + "'");
        //    }
        //    double fineamt = 0;
        //    if (fine != "")
        //    {
        //        fineamt = Convert.ToDouble(fine);
        //    }
        //    fineamt = Duedays * fineamt;
        //    e.Row.Cells[14].Text = Convert.ToString(fineamt);
        //    e.Row.Cells[2].Visible = false;
        //    e.Row.Cells[4].Visible = false;
        //    e.Row.Cells[5].Visible = false;
        //    e.Row.Cells[6].Visible = false;
        //}

    }

    #region NamePopUp

    protected void btnissuname_Click(object sender, EventArgs e)
    {
        DivPopName.Visible = true;
        if (RblMemType.SelectedIndex == 0)
            LblNamePop.Text = "Student Name";
        if (RblMemType.SelectedIndex == 1)
            LblNamePop.Text = "Staff Name";
    }

    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {
        DivPopName.Visible = false;
        popupselectBook.Visible = false;
    }

    protected void BtnNameGo_Click(object sender, EventArgs e)
    {
        DataTable dtName = new DataTable();
        DataRow drName = null;
        if (LblNamePop.Text == "Student Name")
            dtName.Columns.Add("Roll No", typeof(string));
        else
            dtName.Columns.Add("Staff Code", typeof(string));

        dtName.Columns.Add("Name", typeof(string));
        dtName.Columns.Add("Library Id", typeof(string));
        dtName.Columns.Add("Department", typeof(string));

        string Sql = "";
        string colCode = Convert.ToString(ddlcollege.Text);
        if (LblNamePop.Text == "Staff Name")
        {
            Sql = "SELECT M.Staff_Code,Staff_Name,Lib_ID,Dept_Name FROM StaffMaster M,StaffTrans T,HrDept_Master D WHERE M.Staff_Code = T.Staff_Code AND M.College_Code =" + colCode + "  AND T.Dept_Code = D.Dept_Code AND D.College_Code = M.College_Code AND Resign = 0 AND Settled = 0 AND T.Latestrec = 1 ";
            if (txtStudentName.Text != "")
            {
                Sql += "AND Staff_Name Like '" + txtStudentName.Text + "%' order by M.Staff_Code,Dept_Name";
            }
        }
        else
        {
            Sql = "SELECT Roll_No,Stud_Name,Lib_ID,Course_Name +'-'+Dept_Name Dept_Name FROM Registration R,Degree G,Course C,Department D WHERE R.Degree_Code = G.Degree_Code AND R.College_Code = G.College_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND D.Dept_Code = G.Dept_Code AND D.College_Code = G.College_Code AND CC=0 AND DelFlag = 0 AND Exam_Flag <> 'Denar' AND G.College_Code =" + colCode + " ";
            if (txtStudentName.Text != "")
            {
                Sql += "AND Stud_Name Like '" + txtStudentName.Text + "%' order by Roll_No,Dept_Name";
            }
        }
        ds.Clear();
        ds = d2.select_method_wo_parameter(Sql, "text");
        FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
        int sno = 0;
        string id = "";
        string name = "";
        string degree = "";
        string LibId = "";

        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                sno++;
                if (LblNamePop.Text == "Staff Name")
                {
                    id = Convert.ToString(ds.Tables[0].Rows[row]["Staff_Code"]).Trim();
                    name = Convert.ToString(ds.Tables[0].Rows[row]["Staff_Name"]).Trim();
                    degree = Convert.ToString(ds.Tables[0].Rows[row]["Dept_Name"]).Trim();
                    LibId = Convert.ToString(ds.Tables[0].Rows[row]["Lib_ID"]).Trim();
                }

                else
                {
                    id = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]).Trim();
                    name = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]).Trim();
                    degree = Convert.ToString(ds.Tables[0].Rows[row]["Dept_Name"]).Trim();
                    LibId = Convert.ToString(ds.Tables[0].Rows[row]["Lib_ID"]).Trim();
                }
                drName = dtName.NewRow();
                if (LblNamePop.Text == "Student Name")
                    drName["Roll No"] = id;
                else
                    drName["Staff Code"] = id;
                drName["Name"] = name;
                drName["Library Id"] = LibId;
                drName["Department"] = degree;
                dtName.Rows.Add(drName);
            }
            divNameStu.Visible = true;
            GrdName.DataSource = dtName;
            GrdName.DataBind();
            GrdName.Visible = true;
            btn_std_exit1.Visible = true;
        }
    }

    protected void GrdName_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , HiddenFieldName.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void GrdName_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.HiddenFieldName.Value);
            string idorno = GrdName.Rows[rowIndex].Cells[1].Text;
            string stdname = GrdName.Rows[rowIndex].Cells[2].Text;
            txtRollNo.Text = idorno;
            TxtName.Text = stdname;
            DivPopName.Visible = false;
            txtRollNo_Change(sender, e);
        }
        catch
        {
        }
    }

    protected void BtnStuNameExit_Click(object sender, EventArgs e)
    {
        DivPopName.Visible = false;
    }

    protected void txtRollNo_Change(object sender, EventArgs e)
    {
        try
        {
            //if (rollNoFlag == false)
            //{
            hsAccNo.Clear();
            SetLibSettings();
            foreach (GridViewRow gvrow in GrdIssuingBook.Rows)
            {
                int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                if (ViewState["CurrentTable"] != null)
                {
                    DataTable dt = (DataTable)ViewState["CurrentTable"];
                    dt.Rows.RemoveAt(RowCnt);
                    ViewState["CurrentTable"] = dt;
                    GrdIssuingBook.DataSource = dt;
                    GrdIssuingBook.DataBind();
                    GrdIssuingBook.Visible = true;
                }
            }
            bool checkflag = false;
            string StrSaveRollAdmit = "";
            string StrSaveRegNo = "";
            string strAppno = "";
            string Sql = "";
            string StudDegree = "";
            string intCourseCode = "";
            string intDeptCode = "";
            string AllowStud = "";
            string BlnAllowAllBooks = "";
            string intdegcode = "";
            string intDegree = "";
            string Libcode = Convert.ToString(Cbo_CardLibrary.SelectedValue);
            string ColCode = Convert.ToString(ddlcollege.SelectedValue);
            string StrBookType = Convert.ToString(ddlBookType.SelectedValue);
            string strtokenBookType = "";
            string booktype = Convert.ToString(ddlBookType.SelectedItem.Text);
            if (booktype == "Book")
                strtokenBookType = "BOK";
            if (booktype == "Periodicals")
                strtokenBookType = "PER";
            if (booktype == "Project Book")
                strtokenBookType = "PRO";
            if (booktype == "Non-Book Material")
                strtokenBookType = "NBM";
            if (booktype == "Question Bank")
                strtokenBookType = "QBA";
            if (booktype == "Back Volume")
                strtokenBookType = "BVO";
            if (booktype == "Reference Volume")
                strtokenBookType = "REF";
            if (booktype == "All")
                strtokenBookType = "All";
            string cardCriteria = Convert.ToString(ddlCardType.SelectedValue);
            ddlcodenumber.Items.Clear();

            Sql = "SELECT ISNULL(AllowAllCollStud,0) AllowAllCollStud FROM Library WHERE Lib_Code ='" + Libcode + "' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                AllowStud = Convert.ToString(ds.Tables[0].Rows[0]["AllowAllCollStud"]);
                if (AllowStud == "true")
                    BlnAllowMulColStud = true;
                else
                    BlnAllowMulColStud = false;
            }
            else
                BlnAllowMulColStud = false;
            if (ddluserentry.SelectedItem.Text == "Library ID")
            {
                Sql = "SELECT App_No,Roll_No,ISNULL(Lib_ID,'') Lib_ID,ISNULL(Roll_Admit,'') Roll_Admit,ISNULL(Reg_No,'') Reg_No,Stud_Name,Course_Name+'-'+Dept_Name Dept_Name,R.Batch_Year,R.Degree_Code,G.Course_ID,G.Dept_Code,G.Degree_Code,G.College_Code FROM Registration R,Degree G,Course C,Department D WHERE R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code AND Lib_ID ='" + txtRollNo.Text + "' ";
                if (Chkdis.Checked)
                    Sql += "AND DelFlag = 0 ";
                if (BlnAllowMulColStud == false)
                    Sql += " AND G.College_Code =" + ColCode + " ";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(Sql, "text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    strAppno = Convert.ToString(dsload.Tables[0].Rows[0]["App_No"]);
                    StrSaveRollNo = Convert.ToString(dsload.Tables[0].Rows[0]["Roll_No"]);
                    StrSaveLibID = Convert.ToString(dsload.Tables[0].Rows[0]["Lib_ID"]);
                    StrSaveRollAdmit = Convert.ToString(dsload.Tables[0].Rows[0]["Roll_Admit"]);
                    StrSaveRegNo = Convert.ToString(dsload.Tables[0].Rows[0]["Reg_No"]);
                    TxtName.Text = Convert.ToString(dsload.Tables[0].Rows[0]["Stud_Name"]);
                    txtDept.Text = Convert.ToString(dsload.Tables[0].Rows[0]["Dept_Name"]);
                    batch_year = Convert.ToString(dsload.Tables[0].Rows[0]["Batch_Year"]);
                    StudDegree = Convert.ToString(dsload.Tables[0].Rows[0]["Degree_Code"]);
                    intCourseCode = Convert.ToString(dsload.Tables[0].Rows[0]["Course_ID"]);
                    intDeptCode = Convert.ToString(dsload.Tables[0].Rows[0]["Dept_Code"]);
                    Session["category"] = "Student";
                    RblMemType.SelectedIndex = 0;
                    intStudCollCode = Convert.ToString(dsload.Tables[0].Rows[0]["College_Code"]);
                    img_stud1.Visible = true;
                    img_stud1.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + StrSaveRollNo + " ";
                }
                else
                {
                    Sql = "SELECT M.Staff_Code,ISNULL(Lib_ID,'') Lib_ID,Staff_Name,Dept_Name,M.College_Code FROM StaffMaster M,StaffTrans T,HrDept_Master D WHERE M.Staff_Code = T.Staff_Code AND T.Latestrec = 1 AND T.Dept_Code = D.Dept_Code AND M.College_Code = D.College_Code AND M.Lib_ID ='" + txtRollNo.Text + "' AND resign = 0 AND settled = 0 ";
                    if (BlnAllowMulColStud == false)
                        Sql += " AND M.College_Code =" + ColCode + "";
                    dsload.Clear();
                    dsload = d2.select_method_wo_parameter(Sql, "text");
                    if (dsload.Tables[0].Rows.Count > 0)
                    {
                        StrSaveRollNo = Convert.ToString(dsload.Tables[0].Rows[0]["Staff_Code"]);
                        StrSaveLibID = Convert.ToString(dsload.Tables[0].Rows[0]["Lib_ID"]);
                        TxtName.Text = Convert.ToString(dsload.Tables[0].Rows[0]["Staff_Name"]);
                        txtDept.Text = Convert.ToString(dsload.Tables[0].Rows[0]["Dept_Name"]);
                        img_stud1.Visible = true;
                        img_stud1.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + StrSaveRollNo + " ";
                        Session["category"] = "Staff";
                        RblMemType.SelectedIndex = 1;
                        intStudCollCode = Convert.ToString(dsload.Tables[0].Rows[0]["College_Code"]);
                    }
                }
            }
            if (ddluserentry.SelectedItem.Text == "Roll Number" || ddluserentry.SelectedItem.Text == "Smart Card")
            {
                Sql = "SELECT App_No,Roll_No,ISNULL(Lib_ID,'') Lib_ID,ISNULL(Roll_Admit,'') Roll_Admit,ISNULL(Reg_No,'') Reg_No,Stud_Name,Course_Name+'-'+Dept_Name Dept_Name,R.Batch_Year,R.Degree_Code,G.Course_ID,G.Dept_Code,G.College_Code FROM Registration R,Degree G,Course C,Department D WHERE R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code AND Roll_No ='" + txtRollNo.Text + "' ";
                if (Chkdis.Checked)
                    Sql += "AND DelFlag = 0 ";
                if (BlnAllowMulColStud == false)
                    Sql += " AND G.College_Code =" + ColCode + " ";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(Sql, "text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    strAppno = Convert.ToString(dsload.Tables[0].Rows[0]["App_No"]);
                    StrSaveRollNo = Convert.ToString(dsload.Tables[0].Rows[0]["Roll_No"]);
                    StrSaveLibID = Convert.ToString(dsload.Tables[0].Rows[0]["Lib_ID"]);
                    StrSaveRollAdmit = Convert.ToString(dsload.Tables[0].Rows[0]["Roll_Admit"]);
                    StrSaveRegNo = Convert.ToString(dsload.Tables[0].Rows[0]["Reg_No"]);
                    TxtName.Text = Convert.ToString(dsload.Tables[0].Rows[0]["Stud_Name"]);
                    txtDept.Text = Convert.ToString(dsload.Tables[0].Rows[0]["Dept_Name"]);
                    batch_year = Convert.ToString(dsload.Tables[0].Rows[0]["Batch_Year"]);
                    StudDegree = Convert.ToString(dsload.Tables[0].Rows[0]["Degree_Code"]);
                    intCourseCode = Convert.ToString(dsload.Tables[0].Rows[0]["Course_ID"]);
                    intDeptCode = Convert.ToString(dsload.Tables[0].Rows[0]["Dept_Code"]);

                    Session["category"] = "Student";
                    RblMemType.SelectedIndex = 0;
                    intStudCollCode = Convert.ToString(dsload.Tables[0].Rows[0]["College_Code"]);
                    img_stud1.Visible = true;
                    img_stud1.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + StrSaveRollNo + " ";
                }
                else
                {
                    Sql = "SELECT M.Staff_Code,ISNULL(Lib_ID,'') Lib_ID,Staff_Name,Dept_Name,M.College_Code FROM StaffMaster M,StaffTrans T,HrDept_Master D WHERE M.Staff_Code = T.Staff_Code AND T.Latestrec = 1 AND T.Dept_Code = D.Dept_Code AND M.College_Code = D.College_Code AND M.Staff_Code ='" + txtRollNo.Text + "' AND resign = 0 AND settled = 0 ";
                    if (BlnAllowMulColStud == false)
                        Sql += " AND M.College_Code =" + ColCode + "";
                    dsload.Clear();
                    dsload = d2.select_method_wo_parameter(Sql, "text");
                    if (dsload.Tables[0].Rows.Count > 0)
                    {
                        StrSaveRollNo = Convert.ToString(dsload.Tables[0].Rows[0]["Staff_Code"]);
                        StrSaveLibID = Convert.ToString(dsload.Tables[0].Rows[0]["Lib_ID"]);
                        TxtName.Text = Convert.ToString(dsload.Tables[0].Rows[0]["Staff_Name"]);
                        txtDept.Text = Convert.ToString(dsload.Tables[0].Rows[0]["Dept_Name"]);
                        Session["category"] = "Staff";
                        img_stud1.Visible = true;
                        RblMemType.SelectedIndex = 1;
                        img_stud1.ImageUrl = "~/Handler/staffphoto.ashx?Staff_code=" + StrSaveRollNo + " ";
                    }
                }
            }
            if (ddluserentry.SelectedItem.Text == "Register Number")
            {
                Sql = "SELECT App_No,Roll_No,ISNULL(Lib_ID,'') Lib_ID,ISNULL(Roll_Admit,'') Roll_Admit,ISNULL(Reg_No,'') Reg_No,Stud_Name,Course_Name+'-'+Dept_Name Dept_Name,R.Batch_Year,R.Degree_Code,G.Course_ID,G.Dept_Code,G.College_Code FROM Registration R,Degree G,Course C,Department D WHERE R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code AND Reg_No ='" + txtRollNo.Text + "' ";

                if (Chkdis.Checked)
                    Sql += "AND DelFlag = 0 ";
                if (BlnAllowMulColStud == false)
                    Sql += " AND G.College_Code =" + ColCode + " ";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(Sql, "text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    strAppno = Convert.ToString(dsload.Tables[0].Rows[0]["App_No"]);
                    StrSaveRollNo = Convert.ToString(dsload.Tables[0].Rows[0]["Roll_No"]);
                    StrSaveLibID = Convert.ToString(dsload.Tables[0].Rows[0]["Lib_ID"]);
                    StrSaveRollAdmit = Convert.ToString(dsload.Tables[0].Rows[0]["Roll_Admit"]);
                    StrSaveRegNo = Convert.ToString(dsload.Tables[0].Rows[0]["Reg_No"]);
                    TxtName.Text = Convert.ToString(dsload.Tables[0].Rows[0]["Stud_Name"]);
                    txtDept.Text = Convert.ToString(dsload.Tables[0].Rows[0]["Dept_Name"]);
                    batch_year = Convert.ToString(dsload.Tables[0].Rows[0]["Batch_Year"]);
                    StudDegree = Convert.ToString(dsload.Tables[0].Rows[0]["Degree_Code"]);
                    intCourseCode = Convert.ToString(dsload.Tables[0].Rows[0]["Course_ID"]);
                    intDeptCode = Convert.ToString(dsload.Tables[0].Rows[0]["Dept_Code"]);
                    Session["category"] = "Student";
                    RblMemType.SelectedIndex = 0;
                    intStudCollCode = Convert.ToString(dsload.Tables[0].Rows[0]["College_Code"]);
                    img_stud1.Visible = true;
                    img_stud1.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + StrSaveRollNo + " ";
                }
            }
            if (ddluserentry.SelectedItem.Text == "Admission Number")
            {
                Sql = "SELECT App_No,Roll_No,ISNULL(Lib_ID,'') Lib_ID,ISNULL(Roll_Admit,'') Roll_Admit,ISNULL(Reg_No,'') Reg_No,Stud_Name,Course_Name+'-'+Dept_Name Dept_Name,R.Batch_Year,R.Degree_Code,G.Course_ID,G.Dept_Code,G.College_Code FROM Registration R,Degree G,Course C,Department D WHERE R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code  AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code  AND Roll_Admit ='" + txtRollNo.Text + "'";
                if (Chkdis.Checked)
                    Sql += "AND DelFlag = 0 ";
                if (BlnAllowMulColStud == false)
                    Sql += " AND G.College_Code =" + ColCode + " ";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(Sql, "text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    strAppno = Convert.ToString(dsload.Tables[0].Rows[0]["App_No"]);
                    StrSaveRollNo = Convert.ToString(dsload.Tables[0].Rows[0]["Roll_No"]);
                    StrSaveLibID = Convert.ToString(dsload.Tables[0].Rows[0]["Lib_ID"]);
                    StrSaveRollAdmit = Convert.ToString(dsload.Tables[0].Rows[0]["Roll_Admit"]);
                    StrSaveRegNo = Convert.ToString(dsload.Tables[0].Rows[0]["Reg_No"]);
                    TxtName.Text = Convert.ToString(dsload.Tables[0].Rows[0]["Stud_Name"]);
                    txtDept.Text = Convert.ToString(dsload.Tables[0].Rows[0]["Dept_Name"]);
                    batch_year = Convert.ToString(dsload.Tables[0].Rows[0]["Batch_Year"]);
                    StudDegree = Convert.ToString(dsload.Tables[0].Rows[0]["Degree_Code"]);
                    intCourseCode = Convert.ToString(dsload.Tables[0].Rows[0]["Course_ID"]);
                    intDeptCode = Convert.ToString(dsload.Tables[0].Rows[0]["Dept_Code"]);
                    Session["category"] = "Student";
                    intStudCollCode = Convert.ToString(dsload.Tables[0].Rows[0]["College_Code"]);
                    img_stud1.Visible = true;
                    img_stud1.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + StrSaveRollNo + " ";
                }
            }
            if (StrSaveRollNo == "" && RblMemType.SelectedIndex != 2)
            {
                imgdiv2.Visible = true;
                lbl_alertMsg.Text = "No Students";
                return;
            }
            if (RblMemType.SelectedIndex == 0 && StudDegree == "0")
            {
                imgdiv2.Visible = true;
                lbl_alertMsg.Text = "No Students";
                return;
            }
            string LinkVal = d2.GetFunction("select linkvalue from inssettings where linkname='Allow Book Transaction Only if Geate In Entry' and college_code=" + ColCode + "");

            if (LinkVal == "1")
                BlnAllowTrans = true;
            else
                BlnAllowTrans = false;
            string serverDt = d2.ServerDate();
            string date = serverDt.Split(' ')[0];
            string libraryCode = Convert.ToString(ddllibrary.SelectedValue);
            if (BlnAllowTrans == true)
            {
                string currentdate = "";
                bool BlnUseClientSysDate = false;
                string SqlQry = "SELECT LinkValue FROM InsSettings WHERE LinkName = 'Use Client System Date for User In Out' AND College_Code =" + ddlcollege.SelectedValue + "";
                ds = d2.select_method_wo_parameter(SqlQry, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]) == "1")
                        BlnUseClientSysDate = true;
                    else
                        BlnUseClientSysDate = false;
                }
                else
                    BlnUseClientSysDate = false;
                if (BlnUseClientSysDate == true)
                {
                    currentdate = System.DateTime.Now.ToString("dd/MM/yyyy");
                }
                else
                {
                    string dtDate = d2.ServerDate();
                    currentdate = dtDate.Split(' ')[0];
                }
                Sql = "SELECT * FROM LibUsers WHERE Roll_No ='" + StrSaveRollNo + "' AND Entry_Date ='" + currentdate + "' AND Exit_Time = '' AND Lib_Code ='" + libraryCode + "' ";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(Sql, "text");
                if (dsload.Tables[0].Rows.Count == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alertMsg.Text = "Student not entered in E-Gate entry";
                    return;
                }
            }
            BlnAllowAllBooks = d2.GetFunction("SELECT ISNULL(AllowAllBook,0) FROM TokenDetails WHERE Roll_No='" + StrSaveRollNo + "' ");

            if (RblMemType.SelectedIndex == 2)
            {

                if (BlnAllowMulColStud == true)
                {
                    Sql = "SELECT USER_ID,Name,Department,Status,ISNULL(CloseDate,'') CloseDate,Is_Staff,College_Code FROM User_Master WHERE User_ID ='" + txtRollNo.Text + "' AND College_Code =" + intStudCollCode + "";
                }
                else
                {
                    Sql = "SELECT USER_ID,Name,Department,Status,ISNULL(CloseDate,'') CloseDate,Is_Staff,College_Code FROM User_Master WHERE User_ID ='" + txtRollNo.Text + "' AND College_Code =" + ColCode + "";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(Sql, "Text");
                if (ds.Tables[0].Rows.Count == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alertMsg.Text = "No Member ";

                    Is_RollValid = false;
                    return;
                }
                else
                {
                    string status = Convert.ToString(ds.Tables[0].Rows[0]["Status"]);
                    DateTime closedate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CloseDate"]);
                    if (status.ToLower() == "false")
                    {
                        imgdiv2.Visible = true;
                        lbl_alertMsg.Text = "Member is not Active";
                        Is_RollValid = false;
                        return;
                    }
                    DateTime curDate = Convert.ToDateTime(date);
                    if (closedate < curDate)
                    {
                        imgdiv2.Visible = true;
                        lbl_alertMsg.Text = "Date is Closed";
                        Is_RollValid = false;
                        return;
                    }
                    StrSaveRollNo = Convert.ToString(ds.Tables[0].Rows[0]["USER_ID"]);
                    StrSaveLibID = Convert.ToString(ds.Tables[0].Rows[0]["USER_ID"]);
                    TxtName.Text = Convert.ToString(ds.Tables[0].Rows[0]["Name"]);
                    txtDept.Text = Convert.ToString(ds.Tables[0].Rows[0]["Department"]);
                    string isStaff = Convert.ToString(ds.Tables[0].Rows[0]["Is_Staff"]);
                    if (isStaff == "0")
                    {
                        Session["category"] = "Nonmember";
                    }
                    else
                        Session["category"] = "Nonmember";
                    intStudCollCode = Convert.ToString(ds.Tables[0].Rows[0]["College_Code"]);
                }
            }
            if (RblMemType.SelectedIndex == 0)
            {
                if (!string.IsNullOrEmpty(StudDegree))
                {
                    Sql = "SELECT Course_ID,Dept_Code,degree_code FROM Degree where Degree_Code='" + StudDegree + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(Sql, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        deg = Convert.ToString(ds.Tables[0].Rows[0]["Course_ID"]) + "~" + Convert.ToString(ds.Tables[0].Rows[0]["Dept_Code"]);
                        intdegcode = Convert.ToString(ds.Tables[0].Rows[0]["degree_code"]);
                        intDegree = Convert.ToString(ds.Tables[0].Rows[0]["degree_code"]);
                    }
                }
            }
            if (RblMemType.SelectedIndex == 0)
            {
                Sql = "SELECT * FROM TokenDetails WHERE (Roll_No ='" + StrSaveRollNo + "' ) AND Is_Staff = 0 ";
                if (BlnAllowAllBooks.ToLower() == "false")
                {
                    if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                        Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                    else if (BlnBookBankLib == true && BlnBookBankAll == true)
                        Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                    else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                        Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                    else
                        Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";
                    if (Cbo_CardLibrary.SelectedItem.Text != "All")
                        Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                    else if (Cbo_CardLibrary.SelectedItem.Text == "All")
                        Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                }

                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + strtokenBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";
            }

            else if (RblMemType.SelectedIndex == 1)
            {
                Sql = "SELECT * FROM TokenDetails WHERE (Roll_No ='" + StrSaveRollNo + "' ) AND Is_Staff = 1 ";
                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else
                    Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + strtokenBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";
            }
            else
            {
                Sql = "SELECT * FROM TokenDetails WHERE Roll_No ='" + txtRollNo.Text + "' ";
            }
            dsprint.Clear();
            dsprint = d2.select_method_wo_parameter(Sql, "Text");
            if (dsprint.Tables[0].Rows.Count == 0)
            {
                imgdiv2.Visible = true;
                lbl_alertMsg.Text = "Card not generated for this member";
                Page.Form.DefaultFocus = txtRollNo.ClientID;
                txtRollNo.Text = "";
                TxtName.Text = "";
                txtDept.Text = "";
                img_stud1.ImageUrl = "";
                return;
            }

            if (RblMemType.SelectedIndex == 0)
            {

                GetComm(StrSaveRollNo);
                Sql = "SELECT DISTINCT ISNULL(Reas_Loc,'') Reas_Loc FROM TokenDetails WHERE (Roll_No ='" + StrSaveRollNo + "' ) AND Is_Staff = 0 AND Is_Locked = 2 ";
                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";
                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + strtokenBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(Sql, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string cardLock = Convert.ToString(ds.Tables[0].Rows[0]["Reas_Loc"]);
                    if (cardLock != "")
                    {
                        imgdiv2.Visible = true;
                        lbl_alertMsg.Text = "Cards Were Locked, for " + cardLock + "";
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alertMsg.Text = "Cards Were Locked";
                    }
                    blnLock = false;
                }
            }
            else
            {
                Sql = "SELECT DISTINCT ISNULL(Reas_Loc,'') Reas_Loc FROM TokenDetails WHERE (Roll_No ='" + StrSaveRollNo + "' ) AND Is_Staff = 1 AND Is_Locked = 2 ";
                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else
                    Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";
                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + strtokenBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";

                ds.Clear();
                ds = d2.select_method_wo_parameter(Sql, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string cardLock = Convert.ToString(ds.Tables[0].Rows[0]["Reas_Loc"]);
                    if (cardLock != "")
                    {
                        imgdiv2.Visible = true;
                        lbl_alertMsg.Text = "Cards Were Locked, for " + cardLock + "";
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alertMsg.Text = "Cards Were Locked";
                    }
                    blnLock = false;
                }
            }
            reserv_details(StrSaveRollNo);
            LoadBooksHand(BlnBookBankLib, blncomm, BlnBookBankAll, StrSaveRollNo, StrSaveLibID);
            DispCardStatus(BlnBookBankLib, blncomm, BlnBookBankAll, StrSaveRollNo, StrSaveLibID);

            //Load Token
            if (RblMemType.SelectedIndex == 0)
            {
                Sql = "SELECT Token_No,Is_Locked,Dept_Name FROM TokenDetails WHERE Is_Locked = 0 AND Is_Staff = 0  AND Roll_No='" + StrSaveRollNo + "'";
                if (BlnAllowAllBooks.ToLower() == "false")
                {
                    if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                        Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                    else if (BlnBookBankLib == true && BlnBookBankAll == true)
                        Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                    else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                        Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                    else
                        Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                    if (Cbo_CardLibrary.SelectedItem.Text != "All")
                        Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                    else
                        Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                }
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + strtokenBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";
                Sql += "ORDER BY LEN(Token_No),Token_No ";
                dsprint.Clear();
                dsprint = d2.select_method_wo_parameter(Sql, "Text");
            }
            else if (RblMemType.SelectedIndex == 1)
            {
                Sql = "SELECT Token_No,Is_Locked,Dept_Name FROM TokenDetails WHERE Is_Locked = 0 AND Is_Staff = 1 AND Roll_No='" + txtRollNo.Text + "'";

                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else
                    Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + strtokenBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";
                Sql += " ORDER BY LEN(Token_No),Token_No ";
                dsprint.Clear();
                dsprint = d2.select_method_wo_parameter(Sql, "Text");
            }
            else if (RblMemType.SelectedIndex == 2)
            {
                Sql = "SELECT Token_No,Is_Locked,Dept_Name FROM TokenDetails WHERE Is_Locked = 0 AND Roll_No='" + txtRollNo.Text + "'";
                Sql += " ORDER BY LEN(Token_No),Token_No ";
                dsprint.Clear();
                dsprint = d2.select_method_wo_parameter(Sql, "Text");
            }
            if (dsprint.Tables[0].Rows.Count > 0)
            {
                if (ddlcodenumber.Text == "")
                {
                    ddlcodenumber.Items.Clear();
                    for (int i = 0; i < dsprint.Tables[0].Rows.Count; i++)
                    {
                        string tokenNo = Convert.ToString(dsprint.Tables[0].Rows[i]["Token_No"]);
                        if (!string.IsNullOrEmpty(tokenNo))
                            ddlcodenumber.Items.Add(tokenNo);
                    }
                }
            }
            else
            {
                if (rblissue.SelectedIndex == 0 || rblissue.SelectedIndex == 1)
                {
                    DivNocard.Visible = true;
                    LblNocard.Text = "No Card to Issue";
                    //return;
                }
            }
            string DueDate = string.Empty;
            string duesundate = string.Empty;
            double dudate = 0;
            if (RblMemType.SelectedIndex == 0)
            {
                Sql = "SELECT no_of_days,no_of_token,fine,is_staff,isnull(Ref_NoofDays,0)Ref_NoofDays from lib_master where code='" + deg + "' and batch_year='" + batch_year + "' AND Is_Staff = 0 ";
                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else
                    Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";
                dsprint.Clear();
                dsprint = d2.select_method_wo_parameter(Sql, "Text");

                if (dsprint.Tables[0].Rows.Count > 0)
                {
                    string Library_name = Convert.ToString(ddllibrary.SelectedItem.Text);
                    string NoOfDays = Convert.ToString(dsprint.Tables[0].Rows[0]["no_of_days"]);
                    string Ref_NoofDays = Convert.ToString(dsprint.Tables[0].Rows[0]["Ref_NoofDays"]);
                    string LibraryCode = d2.GetFunction("Select lib_code from library where lib_name='" + Library_name + "' and college_code='" + ColCode + "'");

                    Sql = "select ISBooks_DueDate,books_duedate from library where lib_code ='" + LibraryCode + "' ";
                    rsLib.Clear();
                    rsLib = d2.select_method_wo_parameter(Sql, "Text");
                    if (rsLib.Tables[0].Rows.Count > 0)
                    {
                        string isBookDue = Convert.ToString(rsLib.Tables[0].Rows[0]["ISBooks_DueDate"]);
                        if (isBookDue.ToLower() == "true")
                        {
                            DueDate = Convert.ToString(rsLib.Tables[0].Rows[0]["books_duedate"]);
                            DateTime dt = Convert.ToDateTime(DueDate);
                            Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            string issueDt = "";
                            if (!Chk_SelectedDate.Checked)
                            {
                                ISRefBook(LibraryCode, Txtaccno.Text);
                                issueDt = txtissuedate.Text;
                                if (BlnRef == false)
                                {
                                    if (BlnMulRenewDays == false)
                                    {
                                        string[] dtIssue = issueDt.Split('/');
                                        if (dtIssue.Length == 3)
                                            issueDt = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                        DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                        duesundate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                        DateTime dt = Convert.ToDateTime(DueDate);
                                        Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                        dudate = Convert.ToDouble(NoOfDays) - 1;
                                    }
                                    else
                                    {
                                        GetRenewalDays(intRenCount, intRenDays, Txtaccno.Text);
                                        string[] dtIssue = issueDt.Split('/');
                                        if (dtIssue.Length == 3)
                                            issueDt = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                        DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                        duesundate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                        DateTime dt = Convert.ToDateTime(DueDate);
                                        Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                        dudate = Convert.ToDouble(NoOfDays) - 1;
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(Ref_NoofDays) > 0)
                                    {
                                        string[] dtIssue = issueDt.Split('/');
                                        if (dtIssue.Length == 3)
                                            issueDt = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                        DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(Ref_NoofDays) - 1).ToString();
                                        duesundate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(Ref_NoofDays) - 1).ToString();
                                        DateTime dt = Convert.ToDateTime(DueDate);
                                        Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                        dudate = Convert.ToDouble(Ref_NoofDays) - 1;
                                    }
                                    else
                                    {
                                        string[] dtIssue = issueDt.Split('/');
                                        if (dtIssue.Length == 3)
                                            issueDt = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                        DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                        duesundate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                        DateTime dt = Convert.ToDateTime(DueDate);
                                        Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                        dudate = Convert.ToDouble(NoOfDays) - 1;
                                    }
                                }
                            }
                            intIsHoliday = 1;
                            if (!Chk_SelectedDate.Checked)
                            {
                                if (IntDueDatExcHol == 1)
                                {
                                    if (intIsHoliday == 1)
                                    {
                                        if (BlnLibHol == true)
                                        {
                                            Sql = "SELECT DISTINCT Holiday_Date FROM Holiday_Library WHERE Holiday_Date ='" + DueDate + "' AND Lib_Code ='" + Libcode + "' ";
                                        }
                                        else
                                        {
                                            Sql = "select distinct holiday_date from holidayStudents where holiday_date ='" + DueDate + "' ";
                                        }
                                        dsHoliday.Clear();
                                        dsHoliday = d2.select_method_wo_parameter(Sql, "text");
                                        if (dsHoliday.Tables[0].Rows.Count > 0)
                                        {
                                            string[] dtIssue = duesundate.Split('/');
                                            if (dtIssue.Length == 3)
                                                duesundate = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                            DueDate = Convert.ToDateTime(duesundate).AddDays(1).ToString();
                                            DateTime dt = Convert.ToDateTime(DueDate);
                                            Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                            duesundate = dt.ToString("dd/MM/yyyy");
                                            intIsHoliday = 1;
                                        }
                                        else
                                        {
                                            string DateVal = Convert.ToString(Txtduedate.Text);
                                            string[] dtIssueVal = DateVal.Split('/');
                                            if (dtIssueVal.Length == 3)
                                                DateVal = dtIssueVal[1].ToString() + "/" + dtIssueVal[0].ToString() + "/" + dtIssueVal[2].ToString();
                                            DateTime day = Convert.ToDateTime(DateVal);

                                            if (day.DayOfWeek.ToString() == "Sunday")
                                            {
                                                string[] dtIssue = DueDate.Split('/');
                                                //if (dtIssue.Length == 3)
                                                // DueDate = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                                DueDate = Convert.ToDateTime(DueDate).AddDays(1).ToString();
                                                DateTime dt = Convert.ToDateTime(DueDate);
                                                Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                                duesundate = dt.ToString("dd/MM/yyyy");
                                                intIsHoliday = 1;
                                            }
                                            else
                                            {
                                                intIsHoliday = 0;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (RblMemType.SelectedIndex == 1)
            {
                Sql = "SELECT no_of_days,no_of_token,fine,is_staff,isnull(Ref_NoofDays,0)Ref_NoofDays from lib_master where (code='" + StrSaveRollNo + "' or code ='" + StrSaveLibID + "') AND Is_Staff = 1 ";
                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else
                    Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";
                dsprint.Clear();
                dsprint = d2.select_method_wo_parameter(Sql, "Text");

                if (dsprint.Tables[0].Rows.Count > 0)
                {
                    string Library_name = Convert.ToString(ddllibrary.SelectedValue);
                    string LibraryCode = d2.GetFunction("Select lib_code from library where lib_name='" + Library_name + "' and college_code='" + ColCode + "'");
                    Sql = "select ISBooks_DueDate,books_duedate from library where lib_code ='" + Library_name + "'";
                    rsLib.Clear();
                    rsLib = d2.select_method_wo_parameter(Sql, "Text");
                    if (rsLib.Tables[0].Rows.Count > 0)
                    {
                        string isBookDue = Convert.ToString(rsLib.Tables[0].Rows[0]["ISBooks_DueDate"]);
                        string NoOfDays = Convert.ToString(dsprint.Tables[0].Rows[0]["no_of_days"]);
                        string Ref_NoofDays = Convert.ToString(dsprint.Tables[0].Rows[0]["Ref_NoofDays"]);
                        string issueDt = txtissuedate.Text;
                        if (isBookDue.ToLower() == "true")
                        {
                            DueDate = Convert.ToString(rsLib.Tables[0].Rows[0]["books_duedate"]);
                            DateTime dt = Convert.ToDateTime(DueDate);
                            Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            ISRefBook(LibraryCode, Txtaccno.Text);
                            if (!ISReffBook)
                            {
                                string[] dtIssue = issueDt.Split('/');
                                if (dtIssue.Length == 3)
                                    issueDt = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                duesundate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                DateTime dt = Convert.ToDateTime(DueDate);
                                Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                //a.Caption = rs3(1) & "~" & rs3(2) & "~" & rs3(3) - 1
                                dudate = Convert.ToDouble(NoOfDays) - 1;
                            }
                            else
                            {
                                if (Convert.ToInt32(Ref_NoofDays) > 0)
                                {
                                    string[] dtIssue = issueDt.Split('/');
                                    if (dtIssue.Length == 3)
                                        issueDt = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                    DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(Ref_NoofDays) - 1).ToString();
                                    duesundate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(Ref_NoofDays) - 1).ToString();
                                    DateTime dt = Convert.ToDateTime(DueDate);
                                    Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                    //a.Caption = rs3(1) & "~" & rs3(2) & "~" & rs3(3) - 1
                                    dudate = Convert.ToDouble(Ref_NoofDays) - 1;
                                }
                                else
                                {
                                    string[] dtIssue = issueDt.Split('/');
                                    if (dtIssue.Length == 3)
                                        issueDt = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                    DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                    duesundate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                    DateTime dt = Convert.ToDateTime(DueDate);
                                    Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                    //a.Caption = rs3(1) & "~" & rs3(2) & "~" & rs3(3) - 1
                                    dudate = Convert.ToDouble(NoOfDays) - 1;
                                }
                            }
                            if (IntDueDatExcHol == 1)
                            {
                                intIsHoliday = 1;
                                if (intIsHoliday == 1)
                                {
                                    if (BlnLibHol == true)
                                    {
                                        Sql = "SELECT DISTINCT Holiday_Date FROM Holiday_Library WHERE Holiday_Date ='" + DueDate + "' AND Lib_Code ='" + Library_name + "' ";
                                    }
                                    else
                                    {
                                        Sql = "select distinct holiday_date from holidaystaff where holiday_date ='" + DueDate + "'";

                                    }
                                    dsHoliday.Clear();
                                    dsHoliday = d2.select_method_wo_parameter(Sql, "text");
                                    if (dsHoliday.Tables[0].Rows.Count > 0)
                                    {
                                        string[] dtIssue = duesundate.Split('/');
                                        if (dtIssue.Length == 3)
                                            duesundate = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                        DueDate = Convert.ToDateTime(duesundate).AddDays(1).ToString();
                                        DateTime dt = Convert.ToDateTime(DueDate);
                                        Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                        duesundate = dt.ToString("dd/MM/yyyy");

                                        intIsHoliday = 1;
                                    }
                                    else
                                    {
                                        // Txtduedate.Text = DueDate;
                                        string[] dt_DueDate = DueDate.Split('/');
                                        //if (dt_DueDate.Length == 3)
                                        //    DueDate = dt_DueDate[1].ToString() + "/" + dt_DueDate[0].ToString() + "/" + dt_DueDate[2].ToString();
                                        DateTime day = Convert.ToDateTime(DueDate);
                                        if (day.DayOfWeek.ToString() == "Sunday")
                                        {
                                            string[] dtIssue = duesundate.Split('/');
                                            //if (dtIssue.Length == 3)
                                            //  duesundate = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                            DueDate = Convert.ToDateTime(DueDate).AddDays(1).ToString();
                                            DateTime dt = Convert.ToDateTime(DueDate);
                                            Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                            duesundate = dt.ToString("dd/MM/yyyy");
                                            intIsHoliday = 1;
                                        }
                                        else
                                        {
                                            intIsHoliday = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (RblMemType.SelectedIndex == 2)
            {
                Sql = "SELECT no_of_days,no_of_token,fine,is_staff,isnull(Ref_NoofDays,0)Ref_NoofDays from lib_master where (code='" + StrSaveRollNo + "' or code ='" + StrSaveLibID + "') ";
                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else
                    Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";
                dsprint.Clear();
                dsprint = d2.select_method_wo_parameter(Sql, "Text");

                if (dsprint.Tables[0].Rows.Count > 0)
                {
                    string Library_name = Convert.ToString(ddllibrary.SelectedValue);
                    string LibraryCode = d2.GetFunction("Select lib_code from library where lib_name='" + Library_name + "' and college_code='" + ColCode + "'");
                    Sql = "select ISBooks_DueDate,books_duedate from library where lib_code ='" + Library_name + "'";
                    rsLib.Clear();
                    rsLib = d2.select_method_wo_parameter(Sql, "Text");
                    if (rsLib.Tables[0].Rows.Count > 0)
                    {
                        string isBookDue = Convert.ToString(rsLib.Tables[0].Rows[0]["ISBooks_DueDate"]);
                        string NoOfDays = Convert.ToString(dsprint.Tables[0].Rows[0]["no_of_days"]);
                        string Ref_NoofDays = Convert.ToString(dsprint.Tables[0].Rows[0]["Ref_NoofDays"]);
                        string issueDt = txtissuedate.Text;
                        if (isBookDue == "true")
                        {
                            DueDate = Convert.ToString(rsLib.Tables[0].Rows[0]["books_duedate"]);
                            DateTime dt = Convert.ToDateTime(DueDate);
                            Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            ISRefBook(LibraryCode, Txtaccno.Text);
                            if (!ISReffBook)
                            {
                                string[] dtIssue = issueDt.Split('/');
                                if (dtIssue.Length == 3)
                                    issueDt = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                duesundate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                DateTime dt = Convert.ToDateTime(DueDate);
                                Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                //a.Caption = rs3(1) & "~" & rs3(2) & "~" & rs3(3) - 1
                                dudate = Convert.ToDouble(NoOfDays) - 1;
                            }
                            else
                            {
                                if (Convert.ToInt32(Ref_NoofDays) > 0)
                                {
                                    string[] dtIssue = issueDt.Split('/');
                                    if (dtIssue.Length == 3)
                                        issueDt = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                    DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(Ref_NoofDays) - 1).ToString();
                                    duesundate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(Ref_NoofDays) - 1).ToString();
                                    DateTime dt = Convert.ToDateTime(DueDate);
                                    Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                    //a.Caption = rs3(1) & "~" & rs3(2) & "~" & rs3(3) - 1
                                    dudate = Convert.ToDouble(Ref_NoofDays) - 1;
                                }
                                else
                                {
                                    string[] dtIssue = issueDt.Split('/');
                                    if (dtIssue.Length == 3)
                                        issueDt = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                    DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                    duesundate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                    DateTime dt = Convert.ToDateTime(DueDate);
                                    Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                    //a.Caption = rs3(1) & "~" & rs3(2) & "~" & rs3(3) - 1
                                    dudate = Convert.ToDouble(NoOfDays) - 1;
                                }
                            }
                            if (IntDueDatExcHol == 1)
                            {
                                intIsHoliday = 1;
                                if (intIsHoliday == 1)
                                {
                                    if (BlnLibHol == true)
                                    {
                                        Sql = "SELECT DISTINCT Holiday_Date FROM Holiday_Library WHERE Holiday_Date ='" + DueDate + "' AND Lib_Code ='" + Library_name + "' ";
                                    }
                                    else
                                    {
                                        Sql = "select distinct holiday_date from holidaystaff where holiday_date ='" + DueDate + "'";

                                    }
                                    dsHoliday.Clear();
                                    dsHoliday = d2.select_method_wo_parameter(Sql, "text");
                                    if (dsHoliday.Tables[0].Rows.Count > 0)
                                    {
                                        string[] dtIssue = duesundate.Split('/');
                                        if (dtIssue.Length == 3)
                                            duesundate = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                        DueDate = Convert.ToDateTime(duesundate).AddDays(1).ToString();
                                        DateTime dt = Convert.ToDateTime(DueDate);
                                        Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                        duesundate = dt.ToString("dd/MM/yyyy");

                                        intIsHoliday = 1;
                                    }
                                    else
                                    {
                                        // Txtduedate.Text = DueDate;
                                        //string[] dt_DueDate = DueDate.Split('/');
                                        //if (dt_DueDate.Length == 3)
                                        //    DueDate = dt_DueDate[1].ToString() + "/" + dt_DueDate[0].ToString() + "/" + dt_DueDate[2].ToString();
                                        DateTime day = Convert.ToDateTime(DueDate);
                                        if (day.DayOfWeek.ToString() == "Sunday")
                                        {
                                            string[] dtIssue = duesundate.Split('/');
                                            //if (dtIssue.Length == 3)
                                            //   duesundate = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                            DueDate = Convert.ToDateTime(DueDate).AddDays(1).ToString();
                                            DateTime dt = Convert.ToDateTime(DueDate);
                                            Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                            duesundate = dt.ToString("dd/MM/yyyy");
                                            intIsHoliday = 1;
                                        }
                                        else
                                        {
                                            intIsHoliday = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (RblMemType.SelectedIndex == 0)
            {
                check_booksInfo(sender, e);
                boolvar = check_bookInfo;
                if (boolvar == true)
                {
                    Txtaccno.Text = "";
                    return;
                }
            }
            if (Txtaccno.Text == "")
            {
            }
            else
            {
                Valid_Accno(sender, e);
            }
            CheckCount = false;
            // rollNoFlag = true;
            //}
            Page.SetFocus(Txtaccno);
            //string pass = txtRollNo.Text;
            //txtRollNo.Attributes.Add("value", pass);
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collcode, "bookissue.aspx");
        }
    }

    protected void Valid_Accno(object sender, EventArgs e)
    {
        try
        {
            GetComm(txtRollNo.Text);
            string book_type = "";
            string issueType = Convert.ToString(ddlissue.SelectedValue);
            string libraryCode = Convert.ToString(ddllibrary.SelectedValue);
            string collegecode = Convert.ToString(ddlcollege.SelectedValue);
            string StrBatchYear = "";
            string DueDate = string.Empty;
            string issueDt = txtissuedate.Text;
            string token = "";
            string StudDegree = "";
            string acc_no_var = "";
            string Sql = "";
            string qry = "";
            int a = 0;
            int b = 0;
            int renewal = 0;
            string DegreeCode = "";
            string Libcode = Convert.ToString(Cbo_CardLibrary.SelectedValue);
            string ColCode = Convert.ToString(ddlcollege.SelectedValue);
            string StrBookType = Convert.ToString(ddlBookType.SelectedValue);
            string cardCriteria = Convert.ToString(ddlCardType.SelectedValue);

            if (issueType == "Book")
                book_type = "BOK";
            if (issueType == "Periodicals")
                book_type = "PER";
            if (issueType == "Project Book")
                book_type = "PRO";
            if (issueType == "Non-Book Material")
                book_type = "NBM";
            if (issueType == "Question Bank")
                book_type = "QBA";
            if (issueType == "Back Volume")
                book_type = "BVO";
            if (issueType == "Reference Books")
                book_type = "REF";

            StrBatchYear = d2.GetFunction("SELECT Batch_Year FROM Registration WHERE Roll_No ='" + txtRollNo.Text + "' ");
            if (rblissue.SelectedIndex == 0)
            {
                cmdadd_Click(sender, e);
            }
            else if (rblissue.SelectedIndex == 1 || rblissue.SelectedIndex == 2 || rblissue.SelectedIndex == 3)
            {
                acc_no_var = Txtaccno.Text;
                Sql = "SELECT Roll_No,Stud_Name,Token_No,IS_staff,CONVERT(varchar(10), borrow_date,103) as borrow_date from borrow where acc_no='" + Txtaccno.Text + "' and return_flag=0 and lib_code ='" + libraryCode + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(Sql, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string rollNo = Convert.ToString(ds.Tables[0].Rows[0]["Roll_No"]);
                    string isStaff = Convert.ToString(ds.Tables[0].Rows[0]["IS_staff"]);
                    int GrdIssuingBook_rowCount = GrdIssuingBook.Rows.Count;
                    if (GrdIssuingBook_rowCount > 0 && txtRollNo.Text != rollNo)
                    {
                        imgdiv2.Visible = true;
                        lbl_alertMsg.Text = "This book was not issued to this student";
                        Txtaccno.Text = "";
                        return;
                    }
                    if (rblissue.SelectedIndex == 3)
                    {
                        txtissuedate.Text = Convert.ToString(ds.Tables[0].Rows[0]["borrow_date"]);
                        Btnsave.Text = "Lost";
                        //txt_duedate.value = Now
                    }
                    if (isStaff.ToLower() == "false")
                        RblMemType.SelectedIndex = 0;
                    else
                        RblMemType.SelectedIndex = 1;

                    txtRollNo.Text = rollNo;
                    StrSaveRollNo = rollNo;
                    TxtName.Text = Convert.ToString(ds.Tables[0].Rows[0]["Stud_Name"]);
                    token = Convert.ToString(ds.Tables[0].Rows[0]["Token_No"]);
                    acc_no_var = Txtaccno.Text;
                    StrBatchYear = d2.GetFunction("SELECT Batch_Year FROM Registration WHERE Roll_No ='" + StrSaveRollNo + "' ");
                    StudDegree = d2.GetFunction("SELECT Degree_Code FROM Registration WHERE Roll_No ='" + StrSaveRollNo + "' ");
                    if (rblissue.SelectedIndex == 2)
                    {
                        Sql = "select * from borrow where acc_no='" + acc_no_var + "' and return_type='" + book_type + "' AND ROLL_NO='" + txtRollNo.Text + "' and return_flag=0  ";
                        dsprint.Clear();
                        dsprint = d2.select_method_wo_parameter(Sql, "Text");
                        if (dsprint.Tables[0].Rows.Count > 0)
                        {
                            string LinkVal = d2.GetFunction("select LinkValue from inssettings where LinkName='Renewal Permission' and College_Code=" + collegecode + "");
                            string[] arr = LinkVal.Split('/');
                            if (arr.Length > 0)
                            {
                                a = Convert.ToInt32(arr[0]);
                            }
                            Sql = "SELECT ISNULL(Renew_Days,0) FROM TokenDetails WHERE Roll_No='" + txtRollNo.Text + "' ";
                            if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                                Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                            else if (BlnBookBankLib == true && BlnBookBankAll == true)
                                Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                            else
                                Sql = "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                            if (Cbo_CardLibrary.SelectedItem.Text != "All")
                                Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                            else if (Cbo_CardLibrary.SelectedItem.Text == "All")
                                Sql += "AND ISNULL(TransLibCode,'All') ='All'";

                            if (ddlBookType.SelectedItem.Text != "All")
                                Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                            else
                                Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                            if (cardCriteria != "All")
                                Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                            else
                                Sql += "AND ISNULL(CardCat,'All') ='All' ";
                            if (b == 0)
                            {
                                string linkValue = d2.GetFunction("select LinkValue from inssettings where LinkName='Renewal Permission' and College_Code=" + collegecode + "");
                                string[] Linkarr = linkValue.Split('/');
                                if (Linkarr.Length > 0)
                                {
                                    a = Convert.ToInt32(Linkarr[0]);
                                    b = Convert.ToInt32(Linkarr[1]);
                                }
                            }
                            if (a == 1 && b > 0)
                            {
                                string renewtime = d2.GetFunction("select isnull(max(renewaltimes),0) renewaltimes from borrow where acc_no='" + acc_no_var + "' and return_type='" + book_type + "' AND ROLL_NO='" + StrSaveRollNo + "' ");
                                renewal = Convert.ToInt32(renewtime);
                                renewal = renewal + 1;
                                if (renewal > b)
                                {
                                    imgdiv2.Visible = true;
                                    lbl_alertMsg.Text = "Your Renewal Count has been Expired";
                                    //Command2.value = true clearbutton
                                    Txtaccno.Text = "";
                                    rblissue.SelectedIndex = 0;
                                    return;
                                }
                            }
                            else
                            {
                                imgdiv2.Visible = true;
                                lbl_alertMsg.Text = "Can't renewal the book, give the renewal permission";
                                //Command2.value = true
                                Txtaccno.Text = "";
                                rblissue.SelectedIndex = 0;
                                return;
                            }
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alertMsg.Text = "No book to renewal";
                            //Command2.value = true
                            Txtaccno.Text = "";
                            rblissue.SelectedIndex = 0;
                            return;
                        }
                    }
                    if (BlnAllowTrans == true)
                    {
                        string currentdate = "";
                        bool BlnUseClientSysDate = false;
                        string SqlQry = "SELECT LinkValue FROM InsSettings WHERE LinkName = 'Use Client System Date for User In Out' AND College_Code =" + ddlcollege.SelectedValue + "";
                        ds = d2.select_method_wo_parameter(SqlQry, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]) == "1")
                                BlnUseClientSysDate = true;
                            else
                                BlnUseClientSysDate = false;
                        }
                        else
                            BlnUseClientSysDate = false;
                        if (BlnUseClientSysDate == true)
                        {
                            currentdate = System.DateTime.Now.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            string dtDate = d2.ServerDate();
                            currentdate = dtDate.Split(' ')[0];
                        }
                        Sql = "SELECT * FROM LibUsers WHERE Roll_No ='" + StrSaveRollNo + "' AND Entry_Date ='" + currentdate + "' AND Exit_Time = '' AND Lib_Code ='" + libraryCode + "' ";
                        dsload.Clear();
                        dsload = d2.select_method_wo_parameter(Sql, "text");
                        if (dsload.Tables[0].Rows.Count == 0)
                        {
                            imgdiv2.Visible = true;
                            lbl_alertMsg.Text = "Student not entered in E-Gate entry";
                            return;
                        }
                    }
                    if (RblMemType.SelectedIndex == 0)
                    {
                        Sql = "SELECT Token_No,Is_Locked,Dept_Name FROM TokenDetails WHERE (Roll_No='" + StrSaveRollNo + "' OR Roll_No ='" + StrSaveLibID + "') AND Is_Locked = 0 AND Is_Staff = 0 ";
                        if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                            Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                        else if (BlnBookBankLib == true && BlnBookBankAll == true)
                            Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                        else
                            Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                        if (Cbo_CardLibrary.SelectedItem.Text != "All")
                            Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                        else if (Cbo_CardLibrary.SelectedItem.Text == "All")
                            Sql += "AND ISNULL(TransLibCode,'All') ='All'";

                        if (ddlBookType.SelectedItem.Text != "All")
                            Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                        else
                            Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                        if (cardCriteria != "All")
                            Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                        else
                            Sql += "AND ISNULL(CardCat,'All') ='All' ";

                        Sql += "ORDER BY LEN(Token_No),Token_No ";
                    }
                    else
                    {
                        Sql = "SELECT Token_No,Is_Locked,Dept_Name FROM TokenDetails WHERE (Roll_No='" + StrSaveRollNo + "' OR Roll_No ='" + StrSaveLibID + "') AND Is_Locked = 0 AND Is_Staff = 1 ";
                        if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                            Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                        if (BlnBookBankLib == true && BlnBookBankAll == true)
                            Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                        else
                            Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                        if (Cbo_CardLibrary.SelectedItem.Text != "All")
                            Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                        else if (Cbo_CardLibrary.SelectedItem.Text == "All")
                            Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                        if (ddlBookType.SelectedItem.Text != "All")
                            Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                        else
                            Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                        if (cardCriteria != "All")
                            Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                        else
                            Sql += "AND ISNULL(CardCat,'All') ='All' ";

                        Sql += "ORDER BY LEN(Token_No),Token_No ";
                    }
                    dsload.Clear();
                    dsload = d2.select_method_wo_parameter(Sql, "text");
                    if (dsload.Tables[0].Rows.Count > 0)
                    {
                        ddlcodenumber.Items.Clear();
                        for (int i = 0; i < dsload.Tables[0].Rows.Count; i++)
                        {
                            string Token_No = Convert.ToString(dsload.Tables[0].Rows[0]["Token_No"]);
                            if (!string.IsNullOrEmpty(Token_No))
                                ddlcodenumber.Items.Add(Token_No);
                        }
                    }
                    if (RblMemType.SelectedIndex == 0)
                    {
                        if (BlnAllowMulColStud == true)
                        {
                            qry = "SELECT distinct registration.reg_no, Registration.Stud_Name, Course.Course_Name + '-' + Department.Dept_Name as Deg, Registration.Current_Semester,app_no,Registration.degree_code,Registration.Roll_No FROM Registration , Department ,degree,course  where  Degree.Dept_Code = Department.Dept_Code and Degree.Course_Id = Course.Course_Id and  (course.college_code=" + intStudCollCode + ") AND  ((Registration.RollNo_Flag)<>0) AND ((Registration.CC)=0) and  (registration.roll_no ='" + txtRollNo.Text + "' or registration.reg_no ='" + txtRollNo.Text + "' or registration.lib_id ='" + txtRollNo.Text + "' or registration.roll_admit ='" + txtRollNo.Text + "') and registration.degree_code=degree.degree_code";
                        }
                        else
                        {
                            qry = "SELECT distinct registration.reg_no, Registration.Stud_Name, Course.Course_Name + '-' + Department.Dept_Name as Deg, Registration.Current_Semester,app_no,Registration.degree_code,Registration.Roll_No FROM Registration , Department ,degree,course  where  Degree.Dept_Code = Department.Dept_Code and Degree.Course_Id = Course.Course_Id and  (course.college_code=" + collegecode + ") AND  ((Registration.RollNo_Flag)<>0) AND ((Registration.CC)=0) and  (registration.roll_no ='" + txtRollNo.Text + "' or registration.reg_no ='" + txtRollNo.Text + "' or registration.lib_id ='" + txtRollNo.Text + "' or registration.roll_admit ='" + txtRollNo.Text + "') and registration.degree_code=degree.degree_code";
                        }
                    }
                    if (RblMemType.SelectedIndex == 1 || RblMemType.SelectedIndex == 2)
                    {
                        qry = "select * from tokendetails where roll_no='" + txtRollNo.Text + "'";
                        dsload.Clear();
                        dsload = d2.select_method_wo_parameter(qry, "text");
                        if (dsload.Tables[0].Rows.Count == 0)
                        {
                            string LibId = d2.GetFunction("select lib_id from staffmaster where staff_code ='" + txtRollNo.Text + "'");
                            if (Convert.ToInt32(LibId) > 0)
                                qry = "select * from tokendetails where roll_no='" + LibId + "'";
                        }
                    }
                    dsCommon.Clear();
                    dsCommon = d2.select_method_wo_parameter(qry, "text");
                    if (dsCommon.Tables[0].Rows.Count > 0)
                    {
                        string roll_no = Convert.ToString(dsCommon.Tables[0].Rows[0]["roll_no"]);
                        if (RblMemType.SelectedIndex == 1 || RblMemType.SelectedIndex == 2)
                        {
                            txtDept.Text = Convert.ToString(dsCommon.Tables[0].Rows[0]["dept_name"]);
                            img_stud1.Visible = true;
                            img_stud1.ImageUrl = "~/Handler/staffphoto.ashx?Staff_code=" + txtRollNo.Text + " ";
                            //SPicError:
                            //                Resume Next
                            //                StrStudAppNo = 0
                        }
                        else
                        {
                            txtDept.Text = Convert.ToString(dsCommon.Tables[0].Rows[0]["Deg"]);
                            degree_codeVar = Convert.ToString(dsCommon.Tables[0].Rows[0]["degree_code"]);
                            //On Error GoTo PicError
                            img_stud1.Visible = true;
                            img_stud1.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + roll_no + " ";
                            //PicError:
                            //                Resume Next
                            //                StrStudAppNo = rs("app_no")
                        }
                        imgBook.Visible = true;
                        imgBook.ImageUrl = "~/Handler/BookPhoto.ashx?acc_no=" + Txtaccno.Text + " ";

                    }
                    LoadBooksHand(BlnBookBankLib, blncomm, BlnBookBankAll, StrSaveRollNo, StrSaveLibID);
                    DispCardStatus(BlnBookBankLib, blncomm, BlnBookBankAll, StrSaveRollNo, StrSaveLibID);

                    qry = "select count(*) as count from tokendetails where roll_no='" + txtRollNo.Text + "' and is_locked=2";
                    dsprint.Clear();
                    dsprint = d2.select_method_wo_parameter(qry, "text");
                    string count = Convert.ToString(dsprint.Tables[0].Rows[0]["count"]);
                    if (dsprint.Tables[0].Rows.Count > 0)
                        txtlocked.Text = count;
                    else
                        txtlocked.Text = "0";
                }
                if (rblissue.SelectedIndex == 2)
                {
                    if (RblMemType.SelectedIndex == 0)
                    {
                        Sql = "SELECT course_id,dept_code from degree where degree_code=" + degree_codeVar + "";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(Sql, "text");
                        {
                            DegreeCode = Convert.ToString(ds.Tables[0].Rows[0]["course_id"]) + "~" + Convert.ToString(ds.Tables[0].Rows[0]["dept_code"]);
                        }
                    }
                    if (RblMemType.SelectedIndex == 0)
                    {
                        if (!Chk_SelectedDate.Checked)
                        {
                            Sql = "SELECT no_of_days,no_of_token,fine,is_staff,isnull(Ref_NoofDays,0) Ref_NoofDays from lib_master where code='" + DegreeCode + "' AND Is_Staff = 0  and batch_year='" + StrBatchYear + "' ";
                            if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                                Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                            if (BlnBookBankLib == true && BlnBookBankAll == true)
                                Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                            else
                                Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                            if (Cbo_CardLibrary.SelectedItem.Text != "All")
                                Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                            else if (Cbo_CardLibrary.SelectedItem.Text == "All")
                                Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                            if (ddlBookType.SelectedItem.Text != "All")
                                Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                            else
                                Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                            if (cardCriteria != "All")
                                Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                            else
                                Sql += "AND ISNULL(CardCat,'All') ='All' ";

                            dsload.Clear();
                            dsload = d2.select_method_wo_parameter(Sql, "text");
                            if (dsload.Tables[0].Rows.Count > 0)
                            {
                                string NoofDays = Convert.ToString(dsload.Tables[0].Rows[0]["no_of_days"]);
                                if (BlnMulRenewDays == false)
                                {
                                    string[] dtIssue = issueDt.Split('/');
                                    if (dtIssue.Length == 3)
                                        issueDt = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                    DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoofDays) - 1).ToString();
                                    string[] dtdue = DueDate.Split('/');
                                    if (dtdue.Length == 3)
                                        DueDate = dtdue[1].ToString() + "/" + dtdue[0].ToString() + "/" + dtdue[2].ToString();
                                    Txtduedate.Text = DueDate.Split(' ')[0];
                                }
                                else
                                {
                                    GetRenewalDays(intRenCount, intRenDays, acc_no_var);
                                    Sql = "SELECT ISNULL(FineAmount,0) FineAmount FROM ExamFineMs WHERE Degree_Code =" + degree_codeVar + " AND Semester =" + StrBatchYear + " AND " + renewal + " BETWEEN FromDay AND ToDay ";
                                    dsprint.Clear();
                                    dsprint = d2.select_method_wo_parameter(Sql, "text");
                                    if (dsprint.Tables[0].Rows.Count > 0)
                                    {
                                        string amt = Convert.ToString(dsprint.Tables[0].Rows[0]["FineAmount"]);
                                        string[] dtIssue = issueDt.Split('/');
                                        if (dtIssue.Length == 3)
                                            issueDt = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                        DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(amt) - 1).ToString();
                                        string[] dtdue = DueDate.Split('/');
                                        if (dtdue.Length == 3)
                                            DueDate = dtdue[1].ToString() + "/" + dtdue[0].ToString() + "/" + dtdue[2].ToString();
                                        Txtduedate.Text = DueDate.Split(' ')[0];
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Sql = "SELECT no_of_days,no_of_token,fine,is_staff,isnull(Ref_NoofDays,0) Ref_NoofDays from lib_master where code='" + StrSaveRollNo + "' AND Is_Staff = 1 ";
                        if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                            Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                        if (BlnBookBankLib == true && BlnBookBankAll == true)
                            Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                        else
                            Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                        if (Cbo_CardLibrary.SelectedItem.Text != "All")
                            Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                        else if (Cbo_CardLibrary.SelectedItem.Text == "All")
                            Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                        if (ddlBookType.SelectedItem.Text != "All")
                            Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                        else
                            Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                        if (cardCriteria != "All")
                            Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                        else
                            Sql += "AND ISNULL(CardCat,'All') ='All' ";

                        dsprint.Clear();
                        dsprint = d2.select_method_wo_parameter(Sql, "text");
                        if (dsprint.Tables[0].Rows.Count > 0)
                        {
                            string no_of_days = Convert.ToString(dsprint.Tables[0].Rows[0]["no_of_days"]);
                            if (BlnMulRenewDays == false)
                            {
                                string[] dtIssue = issueDt.Split('/');
                                if (dtIssue.Length == 3)
                                    issueDt = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(no_of_days) - 1).ToString();

                                string[] dtdue = DueDate.Split('/');
                                if (dtdue.Length == 3)
                                    DueDate = dtdue[1].ToString() + "/" + dtdue[0].ToString() + "/" + dtdue[2].ToString();
                                Txtduedate.Text = DueDate.Split(' ')[0];
                            }
                        }
                    }
                    //End if 'Renewal

                    //txt_accno.SetFocus
                }
                cmdadd_Click(sender, e);
                //else
                //{
                //    //if checkflag = true Then
                //    //txt_accno.SetFocus
                //}
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collcode, "bookissue.aspx");
        }
    }

    protected void cmdadd_Click(object sender, EventArgs e)
    {
        try
        {
            getInfo(sender, e);
            int RowCount_IssuingBook = GrdIssuingBook.Rows.Count;
            if (RowCount_IssuingBook > 0)
            {
                if (txtRollNo.Text == "")
                    return;
                else
                {
                    Txtaccno.Text = "";
                    ddlissue.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void getInfo(object sender, EventArgs e)
    {
        try
        {
            string sql = "";
            string sql1 = "";
            DataSet rssearch = new DataSet();
            int i = 0;
            string DueDate = string.Empty;
            string duesundate = string.Empty;
            double dudate = 0;
            string issueDt = txtissuedate.Text;
            string[] dtIssue = issueDt.Split('/');
            if (dtIssue.Length == 3)
                issueDt = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();

            string BookDueDate = Txtduedate.Text;
            string dueDATE = Txtduedate.Text;
            string[] dtdueDATE = dueDATE.Split('/');
            if (dtdueDATE.Length == 3)
                dueDATE = dtdueDATE[1].ToString() + "/" + dtdueDATE[0].ToString() + "/" + dtdueDATE[2].ToString();

            string book_type = string.Empty;
            string issueType = Convert.ToString(ddlissue.SelectedValue);
            string libraryCode = Convert.ToString(ddllibrary.SelectedValue);
            string collegecode = Convert.ToString(ddlcollege.SelectedValue);
            string Libcode = Convert.ToString(Cbo_CardLibrary.SelectedValue);
            string StrBookType = Convert.ToString(ddlBookType.SelectedValue);
            string cardCriteria = Convert.ToString(ddlCardType.SelectedValue);
            FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
            check.AutoPostBack = false;
            TimeSpan CDate = new TimeSpan();
            string serverDt = d2.ServerDate();
            DateTime CurDate = Convert.ToDateTime(serverDt.Split(' ')[0]);
            string acc_NO = Txtaccno.Text;
            if (Txtaccno.Text == "")
            {
                return;
            }
            checkflag = false;

            if (issueType == "Book")
                book_type = "BOK";
            if (issueType == "Periodicals")
                book_type = "PER";
            if (issueType == "Project Book")
                book_type = "PRO";
            if (issueType == "Non-Book Material")
                book_type = "NBM";
            if (issueType == "Question Bank")
                book_type = "QBA";
            if (issueType == "Back Volume")
                book_type = "BVO";
            if (issueType == "Reference Books")
                book_type = "REF";

            string FinalAccNo = "";
            StringBuilder sbAccNo = new StringBuilder();
            string AccNoValue = Convert.ToString(Txtaccno.Text);

            if (rblissue.SelectedIndex == 1 || rblissue.SelectedIndex == 2 || rblissue.SelectedIndex == 3)
            {
                DataRow drCurrentRow = null;
                if (GrdIssuingBook.Rows.Count > 0)
                {
                    dtIssuingBook.Columns.Add("Access No", typeof(string));
                    dtIssuingBook.Columns.Add("Title", typeof(string));
                    dtIssuingBook.Columns.Add("Author", typeof(string));
                    dtIssuingBook.Columns.Add("Call No", typeof(string));
                    dtIssuingBook.Columns.Add("Date Of Issue", typeof(string));
                    dtIssuingBook.Columns.Add("Due Days", typeof(string));
                    dtIssuingBook.Columns.Add("Due Date", typeof(string));
                    dtIssuingBook.Columns.Add("Token No", typeof(string));
                    dtIssuingBook.Columns.Add("Fine", typeof(string));
                    SetPreviousData();

                }
                sql = "select * from borrow where lib_code='" + libraryCode + "'  and ltrim(rtrim(acc_no)) in('" + AccNoValue + "') and roll_no='" + StrSaveRollNo + "' and return_flag=0 ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int dsCount = 0; dsCount < ds.Tables[0].Rows.Count; dsCount++)
                    {
                        DateTime bordt = Convert.ToDateTime(ds.Tables[0].Rows[dsCount]["borrow_date"]);
                        string borrow_date = bordt.ToString("dd/MM/yyyy");
                        DateTime duDt = Convert.ToDateTime(ds.Tables[0].Rows[dsCount]["due_date"]);
                        string due_date = duDt.ToString("dd/MM/yyyy");
                        string finalDueDate = duDt.ToString("dd/MM/yyyy");

                        if (!IsSelectedVal)// Not IsSelected(txt_accno.Text) Then
                        {
                            sno++;
                            if (ViewState["CurrentTable"] != null)
                            {
                                dtIssuingBook = (DataTable)ViewState["CurrentTable"];
                                drow1 = null;
                                if (dtIssuingBook.Rows.Count > 0)
                                {
                                    drow1 = dtIssuingBook.NewRow();
                                    drow1["Access No"] = Convert.ToString(ds.Tables[0].Rows[dsCount]["acc_no"]);
                                    drow1["Title"] = Convert.ToString(ds.Tables[0].Rows[dsCount]["title"]);
                                    drow1["Author"] = Convert.ToString(ds.Tables[0].Rows[dsCount]["author"]);
                                    drow1["Call No"] = Convert.ToString(ds.Tables[0].Rows[dsCount]["Call_No"]);
                                    drow1["Date Of Issue"] = Convert.ToString(borrow_date);

                                    if (rblissue.SelectedIndex == 1 || rblissue.SelectedIndex == 2)
                                    {
                                        string[] dttdudate = due_date.Split('/');
                                        if (dttdudate.Length == 3)
                                            due_date = dttdudate[1].ToString() + "/" + dttdudate[0].ToString() + "/" + dttdudate[2].ToString();
                                        DateTime DDate = Convert.ToDateTime(due_date);
                                        string serverDt1 = d2.ServerDate();
                                        DateTime curdate = Convert.ToDateTime(serverDt1.Split(' ')[0]);
                                        TimeSpan diff = curdate.Subtract(DDate);
                                        int Datedifference = diff.Days;

                                        if (Convert.ToInt32(Datedifference) > 0)
                                        {
                                            drow1["Due Days"] = Convert.ToString(Datedifference);
                                        }
                                        else
                                        {
                                            drow1["Due Days"] = Convert.ToString("0");
                                        }
                                    }
                                    else if (rblissue.SelectedIndex == 3)
                                    {
                                        string[] dttIssue = issueDt.Split('/');
                                        string serverDt2 = d2.ServerDate();
                                        DateTime Currentdate = Convert.ToDateTime(serverDt2.Split(' ')[0]);
                                        DateTime DDate = Convert.ToDateTime(duDt);
                                        TimeSpan diff = Currentdate.Subtract(DDate);
                                        int Datedifference = diff.Days;

                                        if (Convert.ToInt32(Datedifference) > 0)
                                        {
                                            drow1["Due Days"] = Convert.ToString(Datedifference);
                                        }
                                        else
                                        {
                                            drow1["Due Days"] = Convert.ToString("0");
                                        }
                                    }
                                    drow1["Due Date"] = Convert.ToString(finalDueDate);
                                    drow1["Token No"] = Convert.ToString(ds.Tables[0].Rows[dsCount]["token_no"]);
                                    drow1["Fine"] = Convert.ToString("0");

                                }
                            }
                            else
                            {
                                drow1 = dtIssuingBook.NewRow();
                                //drow1["SNo"] = Convert.ToString(sno);
                                drow1["Access No"] = Convert.ToString(ds.Tables[0].Rows[dsCount]["acc_no"]);
                                drow1["Title"] = Convert.ToString(ds.Tables[0].Rows[dsCount]["title"]);
                                drow1["Author"] = Convert.ToString(ds.Tables[0].Rows[dsCount]["author"]);
                                drow1["Call No"] = Convert.ToString(ds.Tables[0].Rows[dsCount]["Call_No"]);
                                drow1["Date Of Issue"] = Convert.ToString(borrow_date);

                                if (rblissue.SelectedIndex == 1 || rblissue.SelectedIndex == 2)
                                {
                                    string[] dttdudate = due_date.Split('/');
                                    if (dttdudate.Length == 3)
                                        due_date = dttdudate[1].ToString() + "/" + dttdudate[0].ToString() + "/" + dttdudate[2].ToString();
                                    DateTime DDate = Convert.ToDateTime(due_date);
                                    string serverDt1 = d2.ServerDate();
                                    DateTime curdate = Convert.ToDateTime(serverDt1.Split(' ')[0]);
                                    TimeSpan diff = curdate.Subtract(DDate);
                                    int Datedifference = diff.Days;

                                    if (Convert.ToInt32(Datedifference) > 0)
                                    {
                                        drow1["Due Days"] = Convert.ToString(Datedifference);
                                    }
                                    else
                                    {
                                        drow1["Due Days"] = Convert.ToString("0");
                                    }
                                }
                                else if (rblissue.SelectedIndex == 3)
                                {
                                    string[] dttIssue = issueDt.Split('/');
                                    string serverDt2 = d2.ServerDate();
                                    DateTime Currentdate = Convert.ToDateTime(serverDt2.Split(' ')[0]);
                                    DateTime DDate = Convert.ToDateTime(duDt);
                                    TimeSpan diff = Currentdate.Subtract(DDate);
                                    int Datedifference = diff.Days;

                                    if (Convert.ToInt32(Datedifference) > 0)
                                    {
                                        drow1["Due Days"] = Convert.ToString(Datedifference);
                                    }
                                    else
                                    {
                                        drow1["Due Days"] = Convert.ToString("0");
                                    }
                                }
                                drow1["Due Date"] = Convert.ToString(finalDueDate);
                                drow1["Token No"] = Convert.ToString(ds.Tables[0].Rows[dsCount]["token_no"]);
                                drow1["Fine"] = Convert.ToString("0");
                            }
                            dtIssuingBook.Rows.Add(drow1);
                        }
                        else
                        {
                            sno++;
                            if (ViewState["CurrentTable"] != null)
                            {
                                dtIssuingBook = (DataTable)ViewState["CurrentTable"];
                                drow1 = null;
                                if (dtIssuingBook.Rows.Count > 0)
                                {
                                    drow1 = dtIssuingBook.NewRow();
                                    //drow1["SNo"] = Convert.ToString(sno);
                                    drow1["Access No"] = Convert.ToString(ds.Tables[0].Rows[dsCount]["acc_no"]);
                                    drow1["Title"] = Convert.ToString(ds.Tables[0].Rows[dsCount]["title"]);
                                    drow1["Author"] = Convert.ToString(ds.Tables[0].Rows[dsCount]["author"]);
                                    drow1["Call No"] = Convert.ToString(ds.Tables[0].Rows[dsCount]["Call_No"]);
                                    drow1["Date Of Issue"] = Convert.ToString(borrow_date);

                                    VarborrowNew = Convert.ToString(borrow_date);
                                    DateTime DDate = Convert.ToDateTime(dueDATE);
                                    DateTime ISSDate = Convert.ToDateTime(issueDt);
                                    TimeSpan diff = ISSDate.Subtract(DDate);
                                    int Datedifference = diff.Days;
                                    if (Convert.ToInt32(Datedifference) > 0)
                                    {
                                        drow1["Due Days"] = Convert.ToString(Datedifference);
                                    }
                                    else
                                    {
                                        drow1["Due Days"] = Convert.ToString("0");
                                    }
                                    drow1["Due Date"] = Convert.ToString(finalDueDate);
                                    drow1["Token No"] = Convert.ToString(ds.Tables[0].Rows[dsCount]["token_no"]);
                                    drow1["Fine"] = Convert.ToString("0");
                                }
                            }
                            else
                            {
                                drow1 = dtIssuingBook.NewRow();
                                //drow1["SNo"] = Convert.ToString(sno);
                                drow1["Access No"] = Convert.ToString(ds.Tables[0].Rows[dsCount]["acc_no"]);
                                drow1["Title"] = Convert.ToString(ds.Tables[0].Rows[dsCount]["title"]);
                                drow1["Author"] = Convert.ToString(ds.Tables[0].Rows[dsCount]["author"]);
                                drow1["Call No"] = Convert.ToString(ds.Tables[0].Rows[dsCount]["Call_No"]);
                                drow1["Date Of Issue"] = Convert.ToString(borrow_date);

                                VarborrowNew = Convert.ToString(borrow_date);
                                DateTime DDate = Convert.ToDateTime(dueDATE);
                                DateTime ISSDate = Convert.ToDateTime(issueDt);
                                TimeSpan diff = ISSDate.Subtract(DDate);
                                int Datedifference = diff.Days;
                                if (Convert.ToInt32(Datedifference) > 0)
                                {
                                    drow1["Due Days"] = Convert.ToString(Datedifference);
                                }
                                else
                                {
                                    drow1["Due Days"] = Convert.ToString("0");
                                }
                                drow1["Due Date"] = Convert.ToString(finalDueDate);
                                drow1["Token No"] = Convert.ToString(ds.Tables[0].Rows[dsCount]["token_no"]);
                                drow1["Fine"] = Convert.ToString("0");
                            }
                            dtIssuingBook.Rows.Add(drow1);
                        }

                    }
                }
                ViewState["CurrentTable"] = dtIssuingBook;
                GrdIssuingBook.DataSource = dtIssuingBook;
                GrdIssuingBook.DataBind();
                GrdIssuingBook.Visible = true;
                for (int l = 0; l < GrdIssuingBook.Rows.Count; l++)
                {
                    foreach (GridViewRow row in GrdIssuingBook.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            GrdIssuingBook.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            GrdIssuingBook.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                            GrdIssuingBook.Rows[l].Cells[2].HorizontalAlign = HorizontalAlign.Center;
                            GrdIssuingBook.Rows[l].Cells[7].HorizontalAlign = HorizontalAlign.Right;
                            GrdIssuingBook.Rows[l].Cells[10].HorizontalAlign = HorizontalAlign.Right;
                        }
                    }
                }
                sql = "select * from library where lib_code =" + libraryCode + "";
                rsCalFine.Clear();
                rsCalFine = d2.select_method_wo_parameter(sql, "text");
                string ISFine_Calculate = Convert.ToString(rsCalFine.Tables[0].Rows[0]["ISFine_Calculate"]);
                DateTime FineFrom = Convert.ToDateTime(rsCalFine.Tables[0].Rows[0]["FineFrom"]);
                DateTime FineTo = Convert.ToDateTime(rsCalFine.Tables[0].Rows[0]["FineTo"]);

                if (rsCalFine.Tables[0].Rows.Count > 0)
                {
                    if (ISFine_Calculate == "true" && (Convert.ToDateTime(Txtduedate.Text) >= FineFrom) && (Convert.ToDateTime(Txtduedate.Text) <= FineTo))
                        nocal = 0;
                    else
                        nocal = 1;
                    int issueBkRowCnt = GrdIssuingBook.Rows.Count;
                    Cal_fine(issueBkRowCnt);
                    if (rblissue.SelectedIndex == 3)
                    {
                        ddlFine_OnSelectedIndexChanged(sender, e);
                    }
                    if (SpecialFine == true)
                    {
                        txt_amount.Enabled = false;
                    }
                    else
                    {
                        txt_amount.Enabled = true;
                    }
                }
            }
            #region
            else
            {
                DataRow drCurrentRow = null;
                if (GrdIssuingBook.Rows.Count > 0)
                {
                    dtIssuingBook.Columns.Add("Access No", typeof(string));
                    dtIssuingBook.Columns.Add("Title", typeof(string));
                    dtIssuingBook.Columns.Add("Author", typeof(string));
                    dtIssuingBook.Columns.Add("Call No", typeof(string));
                    dtIssuingBook.Columns.Add("Date Of Issue", typeof(string));
                    dtIssuingBook.Columns.Add("Due Days", typeof(string));
                    dtIssuingBook.Columns.Add("Due Date", typeof(string));
                    dtIssuingBook.Columns.Add("Token No", typeof(string));
                    dtIssuingBook.Columns.Add("Fine", typeof(string));
                    SetPreviousData();
                }

                if (ddllibrary.SelectedItem.Text == "")
                {
                    imgdiv2.Visible = true;
                    lbl_alertMsg.Text = "Select Library";
                    bodate = "No";
                    return;
                }
                if (txtRollNo.Text != "")
                {
                    if (Convert.ToString(ddlcodenumber.Text) == "")
                    {
                        AccessNoLookup.Visible = true;
                        lblAccessNoLookup.Text = "Card Not found";
                        Page.Form.DefaultFocus = txtRollNo.ClientID;
                        return;
                    }
                }
                if (txtRollNo.Text == "")
                {
                    goto start;
                }
                if (!Isspecial)
                {
                    //<<<<<<<<<<<<<<<<<chekcing for same book returned should not be issued>>>>>>>>>>>>

                    sql = "SELECT CONVERT(varchar(10), return_date,103) as return_date,return_flag From borrow WHERE acc_no = '" + Txtaccno.Text + "' and roll_no='" + txtRollNo.Text + "' and is_staff=0 ";
                    dsCommon.Clear();
                    dsCommon = d2.select_method_wo_parameter(sql, "text");
                    if (dsCommon.Tables[0].Rows.Count > 0)
                    {
                        string returnDt = "";
                        string return_flag = "";
                        for (int j = 0; j < dsCommon.Tables[0].Rows.Count; j++)
                        {
                            returnDt = Convert.ToString(dsCommon.Tables[0].Rows[0]["return_date"]);
                            //if (returnDt != "")
                            //{
                            //    string[] dasp = returnDt.Split(' ');
                            //    string[] ret = dasp[0].Split('/');
                            //    if (ret.Length == 3)
                            //        returnDt = ret[1] + "/" + ret[0] + "/" + ret[2];

                            //}
                            return_flag = Convert.ToString(dsCommon.Tables[0].Rows[0]["return_flag"]);
                            if (returnDt == txtissuedate.Text && return_flag == "1")
                            {
                                DivErrorMsg.Visible = true;
                                LblErrorMsg.Text = "Sorry! Today's Returned book cannot be issued to the same person";
                                checkflag = true;
                                Txtaccno.Text = "";
                                return;
                            }
                        }
                    }
                }

                //<<<<<<<<<<<<<<<Checking for Transfered Book should not Issue<<<<<<<<<<
                if (issueType == "Book")
                {
                    sql = "select ISNULL(Transfered,0) Transfered,ISNULL(To_Lib_Code,'') Dept_Code from bookdetails b,book_transfer t where b.acc_no = t.acc_no and transfer_type = 2 and b.acc_no='" + Txtaccno.Text + "' and b.lib_code='" + libraryCode + "'";
                    dsCommon.Clear();
                    dsCommon = d2.select_method_wo_parameter(sql, "text");
                    if (dsCommon.Tables[0].Rows.Count > 0)
                    {
                        string dept = Convert.ToString(dsCommon.Tables[0].Rows[0]["Dept_Code"]);
                        if (Convert.ToString(dsCommon.Tables[0].Rows[0]["Transfered"]) == "true")
                        {
                            DivErrorMsg.Visible = true;
                            LblErrorMsg.Text = "The Book was Transfered to " + dept;
                            Txtaccno.Text = "";
                            return;
                        }
                    }
                }

                //<<<<<<<<<<<<<<<Checking for Issue already<<<<<<<<<<
                if (issueType == "Book")
                {
                    sql = "select book_status from bookdetails where acc_no='" + Txtaccno.Text + "'and lib_code='" + libraryCode + "'";
                    dsCommon.Clear();
                    dsCommon = d2.select_method_wo_parameter(sql, "text");
                    if (dsCommon.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToString(dsCommon.Tables[0].Rows[0]["book_status"]) == "Issued")
                        {
                            sql = "select roll_no,stud_name,borrow_date,due_date,title,token_no from borrow where acc_no='" + Txtaccno.Text + "' and return_flag = 0 and roll_no<>'" + txtRollNo.Text + "'";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(sql, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DivErrorMsg.Visible = true;
                                LblErrorMsg.Text = "Book has been taken by Roll No : " + Convert.ToString(dsCommon.Tables[0].Rows[0]["roll_no"]) + "Name : " + Convert.ToString(dsCommon.Tables[0].Rows[0]["stud_name"]) + "Due Date : " + Convert.ToString(dsCommon.Tables[0].Rows[0]["due_date"]) + "Title :" + Convert.ToString(dsCommon.Tables[0].Rows[0]["title"]) + "Token No :" + Convert.ToString(dsCommon.Tables[0].Rows[0]["token_no"]);

                                checkflag = true;
                                return;
                            }
                        }
                    }
                }

                //------------------Checking for Book Access Permision in library-----------------'
                sql = "Select isnull(Access_Edu_Level,'') Access_Edu_Level from Library  Where Lib_Code ='" + libraryCode + "'";
                dsCommon.Clear();
                dsCommon = d2.select_method_wo_parameter(sql, "text");
                if (dsCommon.Tables[0].Rows.Count > 0 && Convert.ToString(dsCommon.Tables[0].Rows[0]["Access_Edu_Level"]) != "")
                {
                    sql = "Select Edu_Level from Registration R,Degree D,Course C Where R.Degree_Code = D.Degree_Code And D.Course_Id = C.Course_Id And Roll_No ='" + txtRollNo.Text + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sql, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToString(dsCommon.Tables[0].Rows[0]["Access_Edu_Level"]) != Convert.ToString(ds.Tables[0].Rows[0]["Edu_Level"]))
                        {
                            imgdiv2.Visible = true;
                            lbl_alertMsg.Text = "Can't Issue for '" + Convert.ToString(ds.Tables[0].Rows[0]["Edu_Level"]) + "' Students";
                            img_stud1.ImageUrl = "";
                            return;
                        }
                    }
                }
            //<<<<<<<<<<<<<<<<< SEARCHING FOR BOOK >>>>>>>>>>>>>>>>>>>>>>>>>
            start:
                if (issueType == "Book")
                {
                    sql = "select title,call_no,author,book_status,ref,dept_code,call_no,lib_code,acc_no from bookdetails where lib_code='" + libraryCode + "'  and ltrim(rtrim(acc_no)) ='" + Txtaccno.Text + "' ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sql, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToString(ds.Tables[0].Rows[0]["book_status"]).ToUpper() == "AVAILABLE")
                        {
                            IsSelected(Txtaccno.Text);
                            if (!IsSelectedVal)
                            {
                                string Title = Convert.ToString(ds.Tables[0].Rows[0]["Title"]);
                                IsSamtTitle(Title);
                                if (IsSamTitle)
                                {
                                    DivMess1.Visible = true;
                                    LblMessage1.Text = "Same title has taken,Do you still want to issue the book";
                                    if (SureYes == false)
                                    {
                                        Txtaccno.Text = "";
                                        return;
                                    }
                                }
                                ISRefBook(libraryCode, Txtaccno.Text);
                                if (ISReffBook)
                                {
                                    if (RblMemType.SelectedIndex == 0)
                                    {
                                        if (!Chkdis.Checked)
                                        {
                                            sql = "SELECT degree_code,stud_name,app_no,batch_year from registration where (roll_no ='" + txtRollNo.Text + "' or lib_id ='" + txtRollNo.Text + "' or roll_admit ='" + txtRollNo.Text + "') and delflag=0";
                                            dsprint.Clear();
                                            dsprint = d2.select_method_wo_parameter(sql, "text");
                                        }
                                        else
                                        {
                                            sql = "SELECT degree_code,stud_name,app_no,batch_year from registration where (roll_no ='" + txtRollNo.Text + "' or lib_id ='" + txtRollNo.Text + "' or roll_admit ='" + txtRollNo.Text + "')";
                                            dsprint.Clear();
                                            dsprint = d2.select_method_wo_parameter(sql, "text");
                                        }
                                    }
                                    else if (RblMemType.SelectedIndex == 1) //opt Staff
                                    {
                                        sql = "SELECT staff_code,staff_name from staffmaster where staff_code ='" + txtRollNo.Text + "'";
                                        dsprint.Clear();
                                        dsprint = d2.select_method_wo_parameter(sql, "text");
                                    }
                                    else if (RblMemType.SelectedIndex == 2) //opt Nonmember
                                    {
                                        sql = "SELECT USER_ID,Department FROM User_Master WHERE User_ID ='" + txtRollNo.Text + "' AND College_Code ='" + collegecode + "'";
                                        dsprint.Clear();
                                        dsprint = d2.select_method_wo_parameter(sql, "text");
                                    }
                                    if (RblMemType.SelectedIndex == 0)
                                    {
                                        sql = "SELECT course_id,dept_code from degree where degree_code=" + Convert.ToString(dsprint.Tables[0].Rows[0]["degree_code"]) + "";
                                        dsload.Clear();
                                        dsload = d2.select_method_wo_parameter(sql, "text");
                                        if (dsload.Tables[0].Rows.Count > 0)
                                        {
                                            a = Convert.ToString(dsload.Tables[0].Rows[0]["course_id"]) + "~" + Convert.ToString(dsload.Tables[0].Rows[0]["dept_code"]);
                                            intdegcode = Convert.ToInt32(dsload.Tables[0].Rows[0]["dept_code"]);
                                            intDegree = Convert.ToInt32(dsprint.Tables[0].Rows[0]["degree_code"]);
                                        }
                                    }
                                    else if (RblMemType.SelectedIndex == 1)
                                    {
                                        a = Convert.ToString(dsprint.Tables[0].Rows[0]["staff_code"]);
                                    }
                                    else if (RblMemType.SelectedIndex == 2)
                                    {
                                        a = Convert.ToString(dsprint.Tables[0].Rows[0]["USER_ID"]);
                                    }
                                    sql = "select ISBooks_DueDate,books_duedate from library where lib_code ='" + libraryCode + "'";
                                    dsCommon.Clear();
                                    dsCommon = d2.select_method_wo_parameter(sql, "text");
                                    if (dsCommon.Tables[0].Rows.Count > 0)
                                    {

                                        if (Convert.ToString(dsCommon.Tables[0].Rows[0]["ISBooks_DueDate"]) == "true")
                                        {
                                            Txtduedate.Text = Convert.ToString(dsCommon.Tables[0].Rows[0]["books_duedate"]);
                                            DueDate = Convert.ToString(dsCommon.Tables[0].Rows[0]["books_duedate"]);
                                        }
                                        else
                                        {
                                            if (!Chk_SelectedDate.Checked)
                                            {
                                                if (RblMemType.SelectedIndex == 0)
                                                {
                                                    sql = "SELECT no_of_days,no_of_token,fine,is_staff,isnull(Ref_NoofDays,0) Ref_NoofDays from lib_master where code='" + a + "' and batch_year='" + batch_year + "'";
                                                }
                                                else
                                                {
                                                    sql = "SELECT no_of_days,no_of_token,fine,is_staff,isnull(Ref_NoofDays,0) Ref_NoofDays from lib_master where code='" + a + "'";
                                                }
                                                rssearch.Clear();
                                                rssearch = d2.select_method_wo_parameter(sql, "text");
                                                if (rssearch.Tables[0].Rows.Count > 0)
                                                {
                                                    string Ref_NoofDays = Convert.ToString(rssearch.Tables[0].Rows[0]["Ref_NoofDays"]);
                                                    string no_of_days = Convert.ToString(rssearch.Tables[0].Rows[0]["no_of_days"]);
                                                    if (Convert.ToInt32(Ref_NoofDays) > 0)
                                                    {
                                                        DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(Ref_NoofDays) - 1).ToString();
                                                        duesundate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(Ref_NoofDays) - 1).ToString();
                                                        DateTime dt = Convert.ToDateTime(DueDate);
                                                        Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                                        dudate = Convert.ToInt32(Ref_NoofDays) - 1;
                                                    }
                                                    else
                                                    {
                                                        DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(no_of_days) - 1).ToString();
                                                        duesundate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(no_of_days) - 1).ToString();
                                                        DateTime dt = Convert.ToDateTime(DueDate);
                                                        Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                                        dudate = Convert.ToInt32(no_of_days) - 1;
                                                    }
                                                    intIsHoliday = 1;
                                                    if (IntDueDatExcHol == 1)
                                                    {
                                                        if (RblMemType.SelectedIndex == 0)
                                                        {
                                                            if (intIsHoliday == 1)
                                                            {
                                                                if (BlnLibHol == true)
                                                                {
                                                                    sql = "SELECT DISTINCT Holiday_Date FROM Holiday_Library WHERE Holiday_Date ='" + DueDate + "' AND Lib_Code ='" + libraryCode + "' ";
                                                                }
                                                                else
                                                                {
                                                                    sql = "select distinct holiday_date from holidayStudents where holiday_date ='" + DueDate + "' and degree_code =" + intDegree + "";
                                                                }
                                                                dsHoliday.Clear();
                                                                dsHoliday = d2.select_method_wo_parameter(sql, "text");
                                                                if (dsHoliday.Tables[0].Rows.Count > 0)
                                                                {
                                                                    string[] dt_DATE = DueDate.Split('/');
                                                                    if (dt_DATE.Length == 3)
                                                                        DueDate = dt_DATE[1].ToString() + "/" + dt_DATE[0].ToString() + "/" + dt_DATE[2].ToString();
                                                                    DueDate = Convert.ToDateTime(DueDate).AddDays(1).ToString();
                                                                    DateTime dt = Convert.ToDateTime(DueDate);
                                                                    Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                                                    duesundate = dt.ToString("dd/MM/yyyy");
                                                                    intIsHoliday = 1;
                                                                }
                                                                else
                                                                {
                                                                    Txtduedate.Text = DueDate;
                                                                    string[] dt_DueDate = DueDate.Split('/');
                                                                    if (dt_DueDate.Length == 3)
                                                                        DueDate = dt_DueDate[1].ToString() + "/" + dt_DueDate[0].ToString() + "/" + dt_DueDate[2].ToString();
                                                                    DateTime day = Convert.ToDateTime(DueDate);

                                                                    if (day.DayOfWeek.ToString() == "Sunday")
                                                                    {
                                                                        string[] dt_DATE = DueDate.Split('/');
                                                                        if (dt_DATE.Length == 3)
                                                                            DueDate = dt_DATE[1].ToString() + "/" + dt_DATE[0].ToString() + "/" + dt_DATE[2].ToString();
                                                                        DueDate = Convert.ToDateTime(DueDate).AddDays(1).ToString();
                                                                        DateTime dt = Convert.ToDateTime(DueDate);
                                                                        Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                                                        duesundate = dt.ToString("dd/MM/yyyy");
                                                                        intIsHoliday = 1;
                                                                    }
                                                                    else
                                                                    {
                                                                        intIsHoliday = 0;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (intIsHoliday == 1)
                                                            {
                                                                if (BlnLibHol == true)
                                                                    sql = "SELECT DISTINCT Holiday_Date FROM Holiday_Library WHERE Holiday_Date ='" + DueDate + "' AND Lib_Code ='" + libraryCode + "' ";
                                                                else
                                                                    sql = "select distinct holiday_date from holidaystaff where holiday_date ='" + DueDate + "'";
                                                                dsHoliday.Clear();
                                                                dsHoliday = d2.select_method_wo_parameter(sql, "text");
                                                                if (dsHoliday.Tables[0].Rows.Count > 0)
                                                                {
                                                                    string[] dt_DATE = DueDate.Split('/');
                                                                    if (dt_DATE.Length == 3)
                                                                        DueDate = dt_DATE[1].ToString() + "/" + dt_DATE[0].ToString() + "/" + dt_DATE[2].ToString();
                                                                    DueDate = Convert.ToDateTime(DueDate).AddDays(1).ToString();
                                                                    DateTime dt = Convert.ToDateTime(DueDate);
                                                                    Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                                                    duesundate = dt.ToString("dd/MM/yyyy");
                                                                    intIsHoliday = 1;
                                                                }
                                                                else
                                                                {
                                                                    Txtduedate.Text = DueDate;
                                                                    string[] dt_DueDate = DueDate.Split('/');
                                                                    if (dt_DueDate.Length == 3)
                                                                        DueDate = dt_DueDate[1].ToString() + "/" + dt_DueDate[0].ToString() + "/" + dt_DueDate[2].ToString();
                                                                    DateTime day = Convert.ToDateTime(DueDate);
                                                                    if (day.DayOfWeek.ToString() == "Sunday")
                                                                    {
                                                                        string[] dt_DATE = DueDate.Split('/');
                                                                        if (dt_DATE.Length == 3)
                                                                            DueDate = dt_DATE[1].ToString() + "/" + dt_DATE[0].ToString() + "/" + dt_DATE[2].ToString();
                                                                        DueDate = Convert.ToDateTime(DueDate).AddDays(1).ToString();
                                                                        DateTime dt = Convert.ToDateTime(DueDate);
                                                                        Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                                                        duesundate = dt.ToString("dd/MM/yyyy");
                                                                        intIsHoliday = 1;
                                                                    }
                                                                    else
                                                                    {
                                                                        intIsHoliday = 0;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else  //for not reference
                                {
                                    if (RblMemType.SelectedIndex == 0)
                                    {
                                        if (!Chk_SelectedDate.Checked)
                                        {
                                            sql = "SELECT degree_code,stud_name,app_no,batch_year from registration where (roll_no ='" + txtRollNo.Text + "' or lib_id ='" + txtRollNo.Text + "' or roll_admit ='" + txtRollNo.Text + "' or Reg_No ='" + txtRollNo.Text + "' ) and delflag=0";
                                            dsprint.Clear();
                                            dsprint = d2.select_method_wo_parameter(sql, "text");
                                        }
                                        else
                                        {
                                            sql = "SELECT degree_code,stud_name,app_no,batch_year from registration where (roll_no ='" + txtRollNo.Text + "' or lib_id ='" + txtRollNo.Text + "' or roll_admit ='" + txtRollNo.Text + "' or reg_no ='" + txtRollNo.Text + "')";
                                            dsprint.Clear();
                                            dsprint = d2.select_method_wo_parameter(sql, "text");
                                        }
                                    }
                                    if (RblMemType.SelectedIndex == 1) //opt staff
                                    {
                                        sql = "SELECT staff_code,staff_name from staffmaster where (staff_code ='" + txtRollNo.Text + "' or lib_id ='" + txtRollNo.Text + "')";
                                        dsprint.Clear();
                                        dsprint = d2.select_method_wo_parameter(sql, "text");
                                    }
                                    if (RblMemType.SelectedIndex == 2) //opt Nonmember
                                    {
                                        sql = "SELECT USER_ID,Department FROM User_Master WHERE User_ID ='" + txtRollNo.Text + "' AND College_Code ='" + collegecode + "'";
                                        dsprint.Clear();
                                        dsprint = d2.select_method_wo_parameter(sql, "text");
                                    }
                                    if (RblMemType.SelectedIndex == 0)
                                    {
                                        sql = "SELECT course_id,dept_code from degree where degree_code=" + Convert.ToString(dsprint.Tables[0].Rows[0]["degree_code"]) + "";
                                        dsload.Clear();
                                        dsload = d2.select_method_wo_parameter(sql, "text");
                                        if (dsload.Tables[0].Rows.Count > 0)
                                        {
                                            a = Convert.ToString(dsload.Tables[0].Rows[0]["course_id"]) + "~" + Convert.ToString(dsload.Tables[0].Rows[0]["dept_code"]);
                                            intdegcode = Convert.ToInt32(dsload.Tables[0].Rows[0]["dept_code"]);
                                            intDegree = Convert.ToInt32(dsprint.Tables[0].Rows[0]["degree_code"]);
                                        }
                                    }
                                    if (RblMemType.SelectedIndex == 1)
                                    {
                                        a = Convert.ToString(dsprint.Tables[0].Rows[0]["staff_code"]);
                                    }
                                    if (RblMemType.SelectedIndex == 2)
                                    {
                                        a = Convert.ToString(dsprint.Tables[0].Rows[0]["USER_ID"]);
                                    }
                                    if (RblMemType.SelectedIndex == 0)
                                    {
                                        if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                                            sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                                        else if (BlnBookBankLib == true && BlnBookBankAll == true)
                                            sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                                        else
                                            sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                                        if (Cbo_CardLibrary.SelectedItem.Text != "All")
                                            sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                                        else
                                            sql += "AND ISNULL(TransLibCode,'All') ='All'";
                                        if (ddlBookType.SelectedItem.Text != "All")
                                            sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                                        else
                                            sql += "AND ISNULL(Book_Type,'All') ='All' ";
                                        if (cardCriteria != "All")
                                            sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                                        else
                                            sql += "AND ISNULL(CardCat,'All') ='All' ";
                                    }
                                    else if (RblMemType.SelectedIndex == 1)
                                    {
                                        if (!Chk_SelectedDate.Checked)
                                        {
                                            sql = "SELECT no_of_days,no_of_token,fine,is_staff,isnull(Ref_NoofDays,0)Ref_NoofDays from lib_master where code='" + a + "' and batch_year='" + batch_year + "' AND Is_Staff = 1 ";
                                            if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                                                sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                                            else if (BlnBookBankLib == true && BlnBookBankAll == true)
                                                sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                                            else
                                                sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";
                                            if (Cbo_CardLibrary.SelectedItem.Text != "All")
                                                sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                                            else
                                                sql += "AND ISNULL(TransLibCode,'All') ='All'";
                                            if (ddlBookType.SelectedItem.Text != "All")
                                                sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                                            else
                                                sql += "AND ISNULL(Book_Type,'All') ='All' ";
                                            if (cardCriteria != "All")
                                                sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                                            else
                                                sql += "AND ISNULL(CardCat,'All') ='All' ";
                                        }
                                        sql1 = "select ISBooks_DueDate,books_duedate from library where lib_code ='" + libraryCode + "'";
                                        rsLib.Clear();
                                        rsLib = d2.select_method_wo_parameter(sql1, "text");
                                        if (rsLib.Tables[0].Rows.Count > 0)
                                        {
                                            if (Convert.ToString(rsLib.Tables[0].Rows[0]["ISBooks_DueDate"]) == "true")
                                            {
                                                Txtduedate.Text = Convert.ToString(rsLib.Tables[0].Rows[0]["books_duedate"]);
                                                DueDate = Convert.ToString(rsLib.Tables[0].Rows[0]["books_duedate"]);
                                            }
                                            else
                                            {
                                                intIsHoliday = 1;
                                                if (IntDueDatExcHol == 1)
                                                {
                                                    if (RblMemType.SelectedIndex == 0)
                                                    {
                                                        if (intIsHoliday == 1)
                                                        {
                                                            if (BlnLibHol == true)
                                                            {
                                                                sql = "SELECT DISTINCT Holiday_Date FROM Holiday_Library WHERE Holiday_Date ='" + DueDate + "' AND Lib_Code ='" + libraryCode + "' ";
                                                            }
                                                            else
                                                            {
                                                                sql = "select distinct holiday_date from holidayStudents where holiday_date ='" + DueDate + "' and degree_code =" + intDegree + "";
                                                            }
                                                            dsHoliday.Clear();
                                                            dsHoliday = d2.select_method_wo_parameter(sql, "text");
                                                            if (dsHoliday.Tables[0].Rows.Count > 0)
                                                            {
                                                                string[] dt_DATE = DueDate.Split('/');
                                                                if (dt_DATE.Length == 3)
                                                                    DueDate = dt_DATE[1].ToString() + "/" + dt_DATE[0].ToString() + "/" + dt_DATE[2].ToString();
                                                                DueDate = Convert.ToDateTime(DueDate).AddDays(1).ToString();
                                                                DateTime dt = Convert.ToDateTime(DueDate);
                                                                Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                                                duesundate = dt.ToString("dd/MM/yyyy");
                                                                intIsHoliday = 1;
                                                            }
                                                            else
                                                            {
                                                                Txtduedate.Text = DueDate;
                                                                string[] dt_DueDate = DueDate.Split('/');
                                                                if (dt_DueDate.Length == 3)
                                                                    DueDate = dt_DueDate[1].ToString() + "/" + dt_DueDate[0].ToString() + "/" + dt_DueDate[2].ToString();
                                                                DateTime day = Convert.ToDateTime(DueDate);
                                                                if (day.DayOfWeek.ToString() == "Sunday")
                                                                {
                                                                    string[] dt_DATE = DueDate.Split('/');
                                                                    if (dt_DATE.Length == 3)
                                                                        DueDate = dt_DATE[1].ToString() + "/" + dt_DATE[0].ToString() + "/" + dt_DATE[2].ToString();
                                                                    DueDate = Convert.ToDateTime(DueDate).AddDays(1).ToString();
                                                                    DateTime dt = Convert.ToDateTime(DueDate);
                                                                    Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                                                    duesundate = dt.ToString("dd/MM/yyyy");
                                                                    intIsHoliday = 1;
                                                                }
                                                                else
                                                                {
                                                                    intIsHoliday = 0;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (intIsHoliday == 1)
                                                        {
                                                            if (BlnLibHol == true)
                                                                sql = "SELECT DISTINCT Holiday_Date FROM Holiday_Library WHERE Holiday_Date ='" + DueDate + "' AND Lib_Code ='" + libraryCode + "' ";
                                                            else
                                                                sql = "select distinct holiday_date from holidaystaff where holiday_date ='" + DueDate + "'";
                                                            dsHoliday.Clear();
                                                            dsHoliday = d2.select_method_wo_parameter(sql, "text");
                                                            if (dsHoliday.Tables[0].Rows.Count > 0)
                                                            {
                                                                string[] dt_DATE = DueDate.Split('/');
                                                                if (dt_DATE.Length == 3)
                                                                    DueDate = dt_DATE[1].ToString() + "/" + dt_DATE[0].ToString() + "/" + dt_DATE[2].ToString();
                                                                DueDate = Convert.ToDateTime(DueDate).AddDays(1).ToString();
                                                                duesundate = DueDate;
                                                                Txtduedate.Text = DueDate;
                                                                intIsHoliday = 1;
                                                            }
                                                            else
                                                            {
                                                                DueDate = Txtduedate.Text;
                                                                string[] dt_DueDate = DueDate.Split('/');
                                                                if (dt_DueDate.Length == 3)
                                                                    DueDate = dt_DueDate[1].ToString() + "/" + dt_DueDate[0].ToString() + "/" + dt_DueDate[2].ToString();
                                                                DateTime day = Convert.ToDateTime(DueDate);
                                                                if (day.DayOfWeek.ToString() == "Sunday")
                                                                {
                                                                    string[] dt_DATE = DueDate.Split('/');
                                                                    if (dt_DATE.Length == 3)
                                                                        DueDate = dt_DATE[1].ToString() + "/" + dt_DATE[0].ToString() + "/" + dt_DATE[2].ToString();
                                                                    DueDate = Convert.ToDateTime(DueDate).AddDays(1).ToString();
                                                                    DateTime dt = Convert.ToDateTime(DueDate);
                                                                    Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                                                    duesundate = dt.ToString("dd/MM/yyyy");
                                                                    intIsHoliday = 1;
                                                                }
                                                                else
                                                                {
                                                                    intIsHoliday = 0;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    rssearch.Clear();
                                                    rssearch = d2.select_method_wo_parameter(sql, "text");
                                                    if (rssearch.Tables[0].Rows.Count > 0)
                                                    {
                                                        string no_of_days = Convert.ToString(rssearch.Tables[0].Rows[0]["no_of_days"]);

                                                        DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(no_of_days) - 1).ToString();
                                                        duesundate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(no_of_days) - 1).ToString();
                                                        DateTime dt = Convert.ToDateTime(DueDate);
                                                        Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                                        dudate = Convert.ToInt32(no_of_days) - 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (RblMemType.SelectedIndex == 2)
                                    {
                                        if (!Chk_SelectedDate.Checked)
                                        {
                                            sql = "SELECT no_of_days,no_of_token,fine,is_staff,isnull(Ref_NoofDays,0)Ref_NoofDays from lib_master where code='" + a + "' ";
                                            if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                                                sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                                            else if (BlnBookBankLib == true && BlnBookBankAll == true)
                                                sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                                            else
                                                sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";
                                            if (Cbo_CardLibrary.SelectedItem.Text != "All")
                                                sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                                            else
                                                sql += "AND ISNULL(TransLibCode,'All') ='All'";
                                            if (ddlBookType.SelectedItem.Text != "All")
                                                sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                                            else
                                                sql += "AND ISNULL(Book_Type,'All') ='All' ";
                                            if (cardCriteria != "All")
                                                sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                                            else
                                                sql += "AND ISNULL(CardCat,'All') ='All' ";
                                        }
                                        sql1 = "select ISBooks_DueDate,books_duedate from library where lib_code ='" + libraryCode + "'";
                                        rsLib.Clear();
                                        rsLib = d2.select_method_wo_parameter(sql1, "text");
                                        if (rsLib.Tables[0].Rows.Count > 0)
                                        {
                                            if (Convert.ToString(rsLib.Tables[0].Rows[0]["ISBooks_DueDate"]) == "true")
                                            {
                                                Txtduedate.Text = Convert.ToString(rsLib.Tables[0].Rows[0]["books_duedate"]);
                                                DueDate = Convert.ToString(rsLib.Tables[0].Rows[0]["books_duedate"]);
                                            }
                                            else
                                            {
                                                intIsHoliday = 1;
                                                if (IntDueDatExcHol == 1)
                                                {
                                                    if (intIsHoliday == 1)
                                                    {
                                                        if (BlnLibHol == true)
                                                            sql = "SELECT DISTINCT Holiday_Date FROM Holiday_Library WHERE Holiday_Date ='" + DueDate + "' AND Lib_Code ='" + libraryCode + "' ";
                                                        else
                                                            sql = "select distinct holiday_date from holidaystaff where holiday_date ='" + DueDate + "'";
                                                        dsHoliday.Clear();
                                                        dsHoliday = d2.select_method_wo_parameter(sql, "text");
                                                        if (dsHoliday.Tables[0].Rows.Count > 0)
                                                        {
                                                            string[] dt_DATE = DueDate.Split('/');
                                                            if (dt_DATE.Length == 3)
                                                                DueDate = dt_DATE[1].ToString() + "/" + dt_DATE[0].ToString() + "/" + dt_DATE[2].ToString();
                                                            DueDate = Convert.ToDateTime(DueDate).AddDays(1).ToString();
                                                            duesundate = DueDate;
                                                            Txtduedate.Text = DueDate;
                                                            intIsHoliday = 1;
                                                        }
                                                        else
                                                        {
                                                            DueDate = Txtduedate.Text;
                                                            string[] dt_DueDate = DueDate.Split('/');
                                                            if (dt_DueDate.Length == 3)
                                                                DueDate = dt_DueDate[1].ToString() + "/" + dt_DueDate[0].ToString() + "/" + dt_DueDate[2].ToString();
                                                            DateTime day = Convert.ToDateTime(DueDate);
                                                            if (day.DayOfWeek.ToString() == "Sunday")
                                                            {
                                                                string[] dt_DATE = DueDate.Split('/');
                                                                if (dt_DATE.Length == 3)
                                                                    DueDate = dt_DATE[1].ToString() + "/" + dt_DATE[0].ToString() + "/" + dt_DATE[2].ToString();
                                                                DueDate = Convert.ToDateTime(DueDate).AddDays(1).ToString();
                                                                DateTime dt = Convert.ToDateTime(DueDate);
                                                                Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                                                duesundate = dt.ToString("dd/MM/yyyy");
                                                                intIsHoliday = 1;
                                                            }
                                                            else
                                                            {
                                                                intIsHoliday = 0;
                                                            }
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    rssearch.Clear();
                                                    rssearch = d2.select_method_wo_parameter(sql, "text");
                                                    if (rssearch.Tables[0].Rows.Count > 0)
                                                    {
                                                        string no_of_days = Convert.ToString(rssearch.Tables[0].Rows[0]["no_of_days"]);
                                                        DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(no_of_days) - 1).ToString();
                                                        duesundate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(no_of_days) - 1).ToString();
                                                        DateTime dt = Convert.ToDateTime(DueDate);
                                                        Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                                        dudate = Convert.ToInt32(no_of_days) - 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                string borrow_date = txtissuedate.Text;
                                if (ViewState["CurrentTable"] != null)
                                {
                                    dtIssuingBook = (DataTable)ViewState["CurrentTable"];
                                    drCurrentRow = null;
                                    if (dtIssuingBook.Rows.Count > 0)
                                    {
                                        drCurrentRow = dtIssuingBook.NewRow();
                                        drCurrentRow["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                        drCurrentRow["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                        drCurrentRow["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["author"]);
                                        drCurrentRow["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["Call_No"]);
                                        drCurrentRow["Date Of Issue"] = Convert.ToString(borrow_date);
                                        DateTime DDate = Convert.ToDateTime(dueDATE);
                                        DateTime ISSDate = Convert.ToDateTime(issueDt);
                                        TimeSpan diff = DDate.Subtract(ISSDate);
                                        int Datedifference = diff.Days + 1;
                                        drCurrentRow["Due Days"] = Convert.ToString(Datedifference);
                                        drCurrentRow["Due Date"] = Convert.ToString(BookDueDate);
                                        drCurrentRow["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                        drCurrentRow["Fine"] = Convert.ToString("0");
                                        dtIssuingBook.Rows.Add(drCurrentRow);
                                    }
                                }
                                else
                                {
                                    drow1 = dtIssuingBook.NewRow();
                                    drow1["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                    drow1["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                    drow1["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["author"]);
                                    drow1["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["Call_No"]);
                                    drow1["Date Of Issue"] = Convert.ToString(borrow_date);

                                    DateTime DDate = Convert.ToDateTime(dueDATE);
                                    DateTime ISSDate = Convert.ToDateTime(issueDt);
                                    TimeSpan diff = DDate.Subtract(ISSDate);
                                    int Datedifference = diff.Days + 1;
                                    drow1["Due Days"] = Convert.ToString(Datedifference);

                                    drow1["Due Date"] = Convert.ToString(BookDueDate);
                                    drow1["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                    drow1["Fine"] = Convert.ToString("0");
                                    dtIssuingBook.Rows.Add(drow1);
                                }
                                if (ddlcodenumber.Items.Count > 0)
                                {
                                    string token = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                    ddlcodenumber.Items.Remove(token);
                                }
                            }
                            if (Convert.ToString(ds.Tables[0].Rows[0]["ref"]) == "Yes")
                            {
                                string var = Convert.ToString(Btnsave.TabIndex);
                                if (var == "1")
                                {
                                    BtnNo.Focus();
                                    BtnNo.BackColor = Color.LightGreen;
                                }
                                DivMess.Visible = true;
                                LblMessage.Text = "Do You want to Issue Reference book ? ";
                                RefBook = true;
                            }
                        }
                        else if (Convert.ToString(ds.Tables[0].Rows[0]["book_status"]).ToUpper() == "ISSUED")
                        {
                            sql = "select roll_no,stud_name,borrow_date,due_date,title,token_no from borrow where acc_no='" + Txtaccno.Text + "' and return_flag = 0 and roll_no<>'" + txtRollNo.Text + "'";
                            dsCommon.Clear();
                            dsCommon = d2.select_method_wo_parameter(sql, "text");
                            if (dsCommon.Tables[0].Rows.Count > 0)
                            {
                                DivErrorMsg.Visible = true;
                                LblErrorMsg.Text = "Book has been taken by Roll No : " + Convert.ToString(dsCommon.Tables[0].Rows[0]["roll_no"]) + "Name : " + Convert.ToString(dsCommon.Tables[0].Rows[0]["stud_name"]) + "Due Date : " + Convert.ToString(dsCommon.Tables[0].Rows[0]["due_date"]) + "Title :" + Convert.ToString(dsCommon.Tables[0].Rows[0]["title"]) + "Token No :" + Convert.ToString(dsCommon.Tables[0].Rows[0]["token_no"]);
                                Txtaccno.Text = "";
                                checkflag = true;
                            }
                            else
                            {
                                IsSelected(Txtaccno.Text);
                                if (!IsSelectedVal) //Not IsSelected(txt_accno.Text) Then
                                {
                                    if (ViewState["CurrentTable"] != null)
                                    {
                                        dtIssuingBook = (DataTable)ViewState["CurrentTable"];
                                        drCurrentRow = null;
                                        if (dtIssuingBook.Rows.Count > 0)
                                        {
                                            drCurrentRow = dtIssuingBook.NewRow();
                                            drCurrentRow["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                            drCurrentRow["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                            drCurrentRow["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["author"]);
                                            drCurrentRow["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["Call_No"]);
                                            drCurrentRow["Date Of Issue"] = Convert.ToString(CurDate);
                                            DateTime DDate = Convert.ToDateTime(dueDATE);
                                            DateTime ISSDate = Convert.ToDateTime(issueDt);
                                            TimeSpan diff = ISSDate.Subtract(DDate);
                                            int Datedifference = diff.Days;
                                            drCurrentRow["Due Days"] = Convert.ToString(Datedifference);
                                            drCurrentRow["Due Date"] = Convert.ToString(BookDueDate);
                                            drCurrentRow["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                            drCurrentRow["Fine"] = Convert.ToString("0");
                                            dtIssuingBook.Rows.Add(drCurrentRow);
                                        }
                                    }
                                    else
                                    {
                                        drow1 = dtIssuingBook.NewRow();
                                        drow1["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                        drow1["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                        drow1["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["author"]);
                                        drow1["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["Call_No"]);
                                        drow1["Date Of Issue"] = Convert.ToString(CurDate);
                                        DateTime DDate = Convert.ToDateTime(dueDATE);
                                        DateTime ISSDate = Convert.ToDateTime(issueDt);
                                        TimeSpan diff = ISSDate.Subtract(DDate);
                                        int Datedifference = diff.Days;
                                        drow1["Due Days"] = Convert.ToString(Datedifference);
                                        drow1["Due Date"] = Convert.ToString(BookDueDate);
                                        drow1["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                        drow1["Fine"] = Convert.ToString("0");
                                        dtIssuingBook.Rows.Add(drow1);
                                    }
                                }
                            }
                        }
                        else if (Convert.ToString(ds.Tables[0].Rows[0]["book_status"]).ToUpper() == "LOST")
                        {
                            DivMess1.Visible = true;
                            string lib = Convert.ToString(ds.Tables[0].Rows[0]["lib_code"]);
                            LblMessage1.Text = "This book is Trashed out. Do you want to change the status and issue the book?";
                            if (SureYes == true)
                            {
                                sql1 = "update bookdetails set book_status='Available' where acc_no='" + Txtaccno.Text + "' and lib_code='" + lib + "'";
                                update = d2.update_method_wo_parameter(sql1, "text");
                            }
                            IsSelected(Txtaccno.Text);

                            if (!IsSelectedVal) //Not IsSelected(txt_accno.Text) Then
                            {
                                if (ViewState["CurrentTable"] != null)
                                {
                                    dtIssuingBook = (DataTable)ViewState["CurrentTable"];
                                    drCurrentRow = null;
                                    if (dtIssuingBook.Rows.Count > 0)
                                    {
                                        drCurrentRow = dtIssuingBook.NewRow();
                                        drCurrentRow["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                        drCurrentRow["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                        drCurrentRow["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["author"]);
                                        drCurrentRow["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["Call_No"]);
                                        drCurrentRow["Date Of Issue"] = Convert.ToString(CurDate);
                                        DateTime DDate = Convert.ToDateTime(dueDATE);
                                        DateTime ISSDate = Convert.ToDateTime(issueDt);
                                        TimeSpan diff = ISSDate.Subtract(DDate);
                                        int Datedifference = diff.Days;
                                        drCurrentRow["Due Days"] = Convert.ToString(Datedifference);
                                        drCurrentRow["Due Date"] = Convert.ToString(BookDueDate);
                                        drCurrentRow["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                        drCurrentRow["Fine"] = Convert.ToString("0");
                                        dtIssuingBook.Rows.Add(drCurrentRow);
                                    }
                                }
                                else
                                {
                                    drow1 = dtIssuingBook.NewRow();
                                    drow1["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                    drow1["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                    drow1["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["author"]);
                                    drow1["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["Call_No"]);
                                    drow1["Date Of Issue"] = Convert.ToString(CurDate);

                                    DateTime DDate = Convert.ToDateTime(dueDATE);
                                    DateTime ISSDate = Convert.ToDateTime(issueDt);
                                    TimeSpan diff = ISSDate.Subtract(DDate);
                                    int Datedifference = diff.Days;
                                    drow1["Due Days"] = Convert.ToString(Datedifference);

                                    drow1["Due Date"] = Convert.ToString(BookDueDate);
                                    drow1["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                    drow1["Fine"] = Convert.ToString("0");

                                    dtIssuingBook.Rows.Add(drow1);
                                }
                            }
                            if (Convert.ToString(ds.Tables[0].Rows[0]["ref"]) == "Yes")
                            {
                                string var = Convert.ToString(Btnsave.TabIndex);
                                if (var == "1")
                                {
                                    BtnNo.Focus();
                                    BtnNo.BackColor = Color.LightGreen;
                                }
                                DivMess1.Visible = true;
                                LblMessage1.Text = "Do You want to Issue Reference book ? ";
                                RefBook = true;
                            }
                            else
                                Txtaccno.Text = "";
                        }
                        else
                        {
                            DivErrorMsg.Visible = true;
                            LblErrorMsg.Text = Txtaccno.Text + " ( " + Convert.ToString(ds.Tables[0].Rows[0]["title"]) + " ) is under " + Convert.ToString(ds.Tables[0].Rows[0]["book_status"]);
                            Txtaccno.Text = "";
                            bodate = "No";
                            checkflag = true;
                            return;
                        }
                        if (Txtaccno.Text != "")
                        {
                            sq = "Select * from priority_studstaff where Access_number='" + Txtaccno.Text + "' and Lib_code='" + libraryCode + "'";

                            strtitle = d2.GetFunction("select title from bookdetails where acc_no='" + Txtaccno.Text + "'");
                            strAuthor = d2.GetFunction("select author from bookdetails where acc_no='" + Txtaccno.Text + "'");
                            sq = "select roll_no,staff_code,access_number,cur_date,cur_time from priority_studstaff where cancel_flag=0 and roll_no<>'" + txtRollNo.Text + "' and staff_code <>'" + txtRollNo.Text + "' and  access_number='" + Txtaccno.Text + "' and cancel_flag=0 ";

                            rssearch.Clear();
                            rssearch = d2.select_method_wo_parameter(sq, "text");
                            if (rssearch.Tables[0].Rows.Count > 0)
                            {

                                msg = "This book has been requested by: ";
                                for (int k = 0; k < rssearch.Tables[0].Rows.Count; k++)
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(rssearch.Tables[0].Rows[k]["roll_no"])))
                                    {
                                        ReservedPopup.Visible = true;
                                        msg += "Roll No:" + Convert.ToString(rssearch.Tables[0].Rows[k]["roll_no"]);
                                        msg += ",Req. Time:" + Convert.ToString(rssearch.Tables[0].Rows[k]["cur_time"]) + " Req. Date:" + Convert.ToString(rssearch.Tables[0].Rows[k]["cur_date"]);
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(rssearch.Tables[0].Rows[k]["staff_code"])))
                                    {
                                        ReservedPopup.Visible = true;
                                        msg += "Staff Code:" + Convert.ToString(rssearch.Tables[0].Rows[k]["staff_code"]);
                                        msg += ",Req. Time:" + Convert.ToString(rssearch.Tables[0].Rows[k]["cur_time"]) + " Req. Date:" + Convert.ToString(rssearch.Tables[0].Rows[k]["cur_date"]);
                                    }
                                }
                                DivErrorMsg.Visible = true;
                                LblErrorMsg.Text = msg;
                                DivReservedbk.Visible = true;
                                LblReservedbkPop.Text = "Do you still want to issue the book";
                            }

                            //'************************ checking for same title as taken **************************
                            if (Txtaccno.Text != "")
                            {
                                strtitle = d2.GetFunction("select title from bookdetails where acc_no='" + Txtaccno.Text + "'");
                                strAuthor = d2.GetFunction("select author from bookdetails where acc_no='" + Txtaccno.Text + "'");
                                sql = "select * from borrow where title ='" + strtitle + "' and roll_no ='" + StrSaveRollNo + "' and return_flag = 0 and lib_code ='" + libraryCode + "'";
                                dsCommon.Clear();
                                dsCommon = d2.select_method_wo_parameter(sql, "text");
                                if (dsCommon.Tables[0].Rows.Count > 0)
                                {
                                    DivReservedbk.Visible = true;
                                    LblReservedbkPop.Text = "Same title has taken,Do you still want to issue the book";

                                }
                            }

                            //'for check same title in reservation-----------------------
                            DataSet rsResTit = new DataSet();

                            sql = "select Count(*) as count from bookdetails where book_status = 'Available' and title = (select title from bookdetails where acc_no='" + acc_NO + "' and lib_code ='" + libraryCode + "') and lib_code ='" + libraryCode + "'";
                            rsResTit.Clear();
                            rsResTit = d2.select_method_wo_parameter(sql, "text");
                            if (rsResTit.Tables[0].Rows.Count > 0)
                            {
                                string count = Convert.ToString(rsResTit.Tables[0].Rows[0]["count"]);
                                if (count == "1")
                                {
                                    strAuthor = d2.GetFunction("select author from bookdetails where acc_no='" + acc_NO + "'");
                                    sql1 = "select code,p.roll_no,roll_admit,stud_name,p.staff_code,staff_name,access_number,cur_date,cur_time from priority_studstaff p left join registration r on p.roll_no = r.roll_no left join staffmaster s on s.staff_code = p.staff_code where cancel_flag=0 and p.roll_no<>'" + txtRollNo.Text + "' and title in (select title from bookdetails where acc_no='" + acc_NO + "' and lib_code ='" + libraryCode + "') and lib_code ='" + libraryCode + "'";
                                    dsCommon.Clear();
                                    dsCommon = d2.select_method_wo_parameter(sql1, "text");
                                    if (dsCommon.Tables[0].Rows.Count > 0)
                                    {
                                        sql = "Update priority_studstaff set OtherAcc_No ='" + acc_NO + "' where code =" + Convert.ToString(dsCommon.Tables[0].Rows[0]["code"]);
                                        update = d2.update_method_wo_parameter(sql, "text");
                                        msg = "The Same Book Title has been requested by: ";
                                        for (int l = 0; l < dsCommon.Tables[0].Rows.Count; l++)
                                        {
                                            if (!string.IsNullOrEmpty(Convert.ToString(dsCommon.Tables[0].Rows[l]["roll_no"])) && Convert.ToString(dsCommon.Tables[0].Rows[i]["roll_no"]) != "Nil")
                                            {
                                                msg += "Roll No / Roll Admit:" + Convert.ToString(dsCommon.Tables[0].Rows[l]["roll_no"]) + "/" + Convert.ToString(dsCommon.Tables[0].Rows[l]["Roll_Admit"]) + "-" + Convert.ToString(dsCommon.Tables[0].Rows[l]["Stud_Name"]);
                                                msg += ",Req. Time:" + Convert.ToString(dsCommon.Tables[0].Rows[l]["cur_time"]) + " Req. Date:" + Convert.ToString(dsCommon.Tables[0].Rows[l]["cur_date"]);
                                            }
                                            if (!string.IsNullOrEmpty(Convert.ToString(dsCommon.Tables[0].Rows[i]["staff_code"])) && Convert.ToString(dsCommon.Tables[0].Rows[i]["staff_code"]) != "Nil")
                                            {
                                                msg += "Staff Code:" + Convert.ToString(dsCommon.Tables[0].Rows[l]["staff_code"]) + "-" + Convert.ToString(dsCommon.Tables[0].Rows[l]["Staff_Name"]);
                                                msg += ",Req. Time:" + Convert.ToString(dsCommon.Tables[0].Rows[l]["cur_time"]) + " Req. Date:" + Convert.ToString(dsCommon.Tables[0].Rows[l]["cur_date"]);
                                            }
                                        }
                                        DivErrorMsg.Visible = true;
                                        LblErrorMsg.Text = msg;
                                        DivReservedbk.Visible = true;
                                        LblReservedbkPop.Text = "Do you still want to issue the book";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        DivErrorMsg.Visible = true;
                        LblErrorMsg.Text = "Check Access No.";
                    }
                }
                //<<<<<<<<<<<<<<<<< SEARCHING FOR REFERENCE BOOK >>>>>>>>>>>>>>>>>>>>>>>>>
                else if (issueType == "Reference Books")
                {
                    sql = "select title,call_no,author,book_status,ref,dept_code,lib_code from bookdetails where lib_code='" + libraryCode + "'  and ltrim(rtrim(acc_no))='" + acc_NO + "' and ref='Yes'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sql, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToString(ds.Tables[0].Rows[0]["book_status"]).ToUpper() == "AVAILABLE")
                        {
                            //Image1.Picture = LoadPicture(photoAccess(photoGet, book, txt_accno.Text, GetLibraryCode(cbo_library.Text), "BOK"))
                            IsSelected(Txtaccno.Text);
                            if (!IsSelectedVal)
                            {
                                if (ViewState["CurrentTable"] != null)
                                {
                                    dtIssuingBook = (DataTable)ViewState["CurrentTable"];
                                    drCurrentRow = null;
                                    if (dtIssuingBook.Rows.Count > 0)
                                    {
                                        drCurrentRow = dtIssuingBook.NewRow();
                                        drCurrentRow["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                        drCurrentRow["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                        drCurrentRow["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["author"]);
                                        drCurrentRow["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["Call_No"]);
                                        drCurrentRow["Date Of Issue"] = Convert.ToString(CurDate);
                                        DateTime DDate = Convert.ToDateTime(dueDATE);
                                        DateTime ISSDate = Convert.ToDateTime(issueDt);
                                        TimeSpan diff = ISSDate.Subtract(DDate);
                                        int Datedifference = diff.Days;
                                        drCurrentRow["Due Days"] = Convert.ToString(Datedifference);
                                        drCurrentRow["Due Date"] = Convert.ToString(BookDueDate);
                                        drCurrentRow["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                        drCurrentRow["Fine"] = Convert.ToString("0");
                                        dtIssuingBook.Rows.Add(drCurrentRow);
                                    }
                                }
                                else
                                {
                                    drow1 = dtIssuingBook.NewRow();
                                    drow1["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                    drow1["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                    drow1["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["author"]);
                                    drow1["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["Call_No"]);
                                    drow1["Date Of Issue"] = Convert.ToString(CurDate);
                                    DateTime DDate = Convert.ToDateTime(dueDATE);
                                    DateTime ISSDate = Convert.ToDateTime(issueDt);
                                    TimeSpan diff = ISSDate.Subtract(DDate);
                                    int Datedifference = diff.Days;
                                    drow1["Due Days"] = Convert.ToString(Datedifference);
                                    drow1["Due Date"] = Convert.ToString(BookDueDate);
                                    drow1["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                    drow1["Fine"] = Convert.ToString("0");
                                    dtIssuingBook.Rows.Add(drow1);
                                }
                                if (ddlcodenumber.Items.Count > 0)
                                {
                                    string token = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                    ddlcodenumber.Items.Remove(token);
                                }
                                issue_type = "REF";
                            }
                            else if (Convert.ToString(ds.Tables[0].Rows[0]["book_status"]).ToUpper() == "ISSUED")
                            {
                                sql1 = "select roll_no,stud_name,borrow_date,due_date from borrow where acc_no='" + acc_NO + "' and return_flag = 0 ";
                                dsprint.Clear();
                                dsprint = d2.select_method_wo_parameter(sql1, "text");
                                if (dsprint.Tables[0].Rows.Count > 0)
                                {
                                    DivErrorMsg.Visible = true;
                                    LblErrorMsg.Text = "Book has been taken by Roll No : " + Convert.ToString(dsprint.Tables[0].Rows[0]["roll_no"]) + "Name : " + Convert.ToString(dsprint.Tables[0].Rows[0]["stud_name"]) + "Due Date : " + Convert.ToString(dsprint.Tables[0].Rows[0]["due_date"]);
                                    Txtaccno.Text = "";
                                    //Page.Form.DefaultFocus = Txtaccno.ClientID;
                                    checkflag = true;
                                }
                                else
                                {
                                    imgdiv2.Visible = false;
                                    lbl_alertMsg.Text = "Book Is Under Issue";
                                    bodate = "No";
                                    checkflag = true;
                                    return;
                                }
                            }
                            else if (Convert.ToString(ds.Tables[0].Rows[0]["book_status"]).ToUpper() == "LOST")
                            {
                                DivMess.Visible = true;
                                LblMessage.Text = "This book is Trashed out. Do you want to change the status and issue the book?";
                                if (SureYes == true)
                                {
                                    sql = "update bookdetails set book_status='Available' where acc_no='" + acc_NO + "' and lib_code='" + Convert.ToString(ds.Tables[0].Rows[0]["lib_code"]) + "'";
                                    ds = d2.select_method_wo_parameter(sql, "text");

                                    IsSelected(Txtaccno.Text);
                                    if (!IsSelectedVal)
                                    {
                                        if (ViewState["CurrentTable"] != null)
                                        {
                                            dtIssuingBook = (DataTable)ViewState["CurrentTable"];
                                            drCurrentRow = null;
                                            if (dtIssuingBook.Rows.Count > 0)
                                            {
                                                drCurrentRow = dtIssuingBook.NewRow();
                                                drCurrentRow["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                                drCurrentRow["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                                drCurrentRow["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["author"]);
                                                drCurrentRow["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["Call_No"]);
                                                drCurrentRow["Date Of Issue"] = Convert.ToString(CurDate);
                                                DateTime DDate = Convert.ToDateTime(dueDATE);
                                                DateTime ISSDate = Convert.ToDateTime(issueDt);
                                                TimeSpan diff = ISSDate.Subtract(DDate);
                                                int Datedifference = diff.Days;
                                                drCurrentRow["Due Days"] = Convert.ToString(Datedifference);
                                                drCurrentRow["Due Date"] = Convert.ToString(BookDueDate);
                                                drCurrentRow["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                                drCurrentRow["Fine"] = Convert.ToString("0");
                                                dtIssuingBook.Rows.Add(drCurrentRow);
                                            }
                                        }
                                        else
                                        {
                                            drow1 = dtIssuingBook.NewRow();
                                            drow1["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                            drow1["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                            drow1["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["author"]);
                                            drow1["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["Call_No"]);
                                            drow1["Date Of Issue"] = Convert.ToString(CurDate);
                                            DateTime DDate = Convert.ToDateTime(dueDATE);
                                            DateTime ISSDate = Convert.ToDateTime(issueDt);
                                            TimeSpan diff = ISSDate.Subtract(DDate);
                                            int Datedifference = diff.Days;
                                            drow1["Due Days"] = Convert.ToString(Datedifference);
                                            drow1["Due Date"] = Convert.ToString(BookDueDate);
                                            drow1["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                            drow1["Fine"] = Convert.ToString("0");
                                            dtIssuingBook.Rows.Add(drow1);
                                        }
                                        if (ddlcodenumber.Items.Count > 0)
                                        {
                                            string token = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                            ddlcodenumber.Items.Remove(token);
                                        }
                                    }
                                    issue_type = "REF";
                                }
                                else
                                    Txtaccno.Text = "";
                            }
                            else
                            {
                                DivErrorMsg.Visible = true;
                                LblErrorMsg.Text = acc_NO + "(" + Convert.ToString(ds.Tables[0].Rows[0]["title"]) + ") is " + Convert.ToString(ds.Tables[0].Rows[0]["book_status"]);
                                Txtaccno.Text = "";
                            }
                        }
                        else
                        {
                            DivErrorMsg.Visible = true;
                            LblErrorMsg.Text = "Check Access No.";
                            bodate = "No";
                        }
                    }
                }
                //<<<<<<<<<<<<<<<<< SEARCHING FOR PERIODICALS BOOK >>>>>>>>>>>>>>>>>>>>>>>>>
                else if (issueType == "Periodicals")
                {
                    sql1 = "SELECT title,journal_code,issue_flag from journal where ((access_code))='" + acc_NO + "' and lib_code='" + libraryCode + "' and back_flag='No' and bind_flag='No'";
                    //rs.Open "SELECT title,journal_code,issue_flag from journal where ((access_code))='" + Txtaccno.Text + "' and lib_code='" +libraryCode+ "' and back_flag='No' and bind_flag='No'", db, adOpenStatic
                    ds.Clear();
                    rssearch = d2.select_method_wo_parameter(sql1, "text");
                    if (rssearch.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToString(rssearch.Tables[0].Rows[0]["issue_flag"]).ToUpper() == "AVAILABLE")
                        {
                            //Image1.Picture = LoadPicture(photoAccess(photoGet, book, txt_accno.Text, GetLibraryCode(cbo_library.Text), "PER"))
                            sql = "SELECT periodicity from journal_master where journal_code='" + Convert.ToString(rssearch.Tables[0].Rows[0]["journal_code"]) + "' and lib_code='" + libraryCode + "'";
                            dsCommon.Clear();
                            dsCommon = d2.select_method_wo_parameter(sql, "text");
                            string periodicity = "";
                            periodicity = Convert.ToString(dsCommon.Tables[0].Rows[0]["periodicity"]);
                            IsSelected(Txtaccno.Text);
                            if (!IsSelectedVal)
                            {
                                if (ViewState["CurrentTable"] != null)
                                {
                                    dtIssuingBook = (DataTable)ViewState["CurrentTable"];
                                    drCurrentRow = null;
                                    if (dtIssuingBook.Rows.Count > 0)
                                    {
                                        drCurrentRow = dtIssuingBook.NewRow();
                                        drCurrentRow["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                        drCurrentRow["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                        drCurrentRow["Author"] = Convert.ToString(periodicity);
                                        drCurrentRow["Call No"] = Convert.ToString("");
                                        drCurrentRow["Date Of Issue"] = Convert.ToString(CurDate);
                                        DateTime DDate = Convert.ToDateTime(dueDATE);
                                        DateTime ISSDate = Convert.ToDateTime(issueDt);
                                        TimeSpan diff = ISSDate.Subtract(DDate);
                                        int Datedifference = diff.Days;
                                        drCurrentRow["Due Days"] = Convert.ToString(Datedifference);
                                        drCurrentRow["Due Date"] = Convert.ToString(BookDueDate);
                                        drCurrentRow["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                        drCurrentRow["Fine"] = Convert.ToString("0");
                                        dtIssuingBook.Rows.Add(drCurrentRow);
                                    }
                                }
                                else
                                {
                                    drow1 = dtIssuingBook.NewRow();
                                    drow1["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                    drow1["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                    drow1["Author"] = Convert.ToString(periodicity);
                                    drow1["Call No"] = Convert.ToString("");
                                    drow1["Date Of Issue"] = Convert.ToString(CurDate);
                                    DateTime DDate = Convert.ToDateTime(dueDATE);
                                    DateTime ISSDate = Convert.ToDateTime(issueDt);
                                    TimeSpan diff = ISSDate.Subtract(DDate);
                                    int Datedifference = diff.Days;
                                    drow1["Due Days"] = Convert.ToString(Datedifference);
                                    drow1["Due Date"] = Convert.ToString(BookDueDate);
                                    drow1["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                    drow1["Fine"] = Convert.ToString("0");
                                    dtIssuingBook.Rows.Add(drow1);
                                }
                                if (ddlcodenumber.Items.Count > 0)
                                {
                                    string token = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                    ddlcodenumber.Items.Remove(token);
                                }
                            }
                            issue_type = "PER";
                        }
                        else if (Convert.ToString(ds.Tables[0].Rows[0]["issue_flag"]).ToUpper() == "LOST")
                        {
                            DivMess.Visible = true;
                            LblMessage.Text = "This Journal is Trashed out. Do you want to change the status and issue the journal?";
                            if (SureYes == true)
                            {
                                sql = "update journal set issue_flag='Available' where access_code='" + acc_NO + "' and lib_code='" + libraryCode + "' and back_flag='No' and bind_flag='No'";
                                rssearch = d2.select_method_wo_parameter(sql, "text");


                                sql = "SELECT periodicity from journal_master where journal_code='" + Convert.ToString(ds.Tables[0].Rows[0]["journal_code"]) + "' and lib_code='" + libraryCode + "'";
                                issue_type = "PER";
                            }
                            else
                                Txtaccno.Text = "";
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alertMsg.Text = "Check Access No.";
                            bodate = "No";
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alertMsg.Text = "Check Access No.";
                        bodate = "No";
                    }
                }

                //'<<<<<<<<<<<<<<<<<< SEARCHING FOR PROJECTBOOK >>>>>>>>>>>>>>>>>>>>>>>>>
                else if (issueType == "Project book")
                {
                    sql = "SELECT title,name,issue_flag,area_of_project from project_book where lib_code='" + libraryCode + "'  and ltrim(rtrim(probook_accno))='" + acc_NO + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sql, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToString(ds.Tables[0].Rows[0]["issue_flag"]).ToUpper() == "AVAILABLE")
                        {
                            //Image1.Picture = LoadPicture(photoAccess(photoGet, book, txt_accno.Text, GetLibraryCode(cbo_library.Text), "PRO"))
                            IsSelected(Txtaccno.Text);
                            if (!IsSelectedVal)
                            {
                                if (ViewState["CurrentTable"] != null)
                                {
                                    dtIssuingBook = (DataTable)ViewState["CurrentTable"];
                                    drCurrentRow = null;
                                    if (dtIssuingBook.Rows.Count > 0)
                                    {
                                        drCurrentRow = dtIssuingBook.NewRow();
                                        drCurrentRow["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                        drCurrentRow["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                        drCurrentRow["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["name"]);
                                        drCurrentRow["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["area_of_project"]);
                                        drCurrentRow["Date Of Issue"] = Convert.ToString(CurDate);
                                        DateTime DDate = Convert.ToDateTime(dueDATE);
                                        DateTime ISSDate = Convert.ToDateTime(issueDt);
                                        TimeSpan diff = ISSDate.Subtract(DDate);
                                        int Datedifference = diff.Days;
                                        drCurrentRow["Due Days"] = Convert.ToString(Datedifference);
                                        drCurrentRow["Due Date"] = Convert.ToString(BookDueDate);
                                        drCurrentRow["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                        drCurrentRow["Fine"] = Convert.ToString("0");
                                        dtIssuingBook.Rows.Add(drCurrentRow);
                                    }
                                }
                                else
                                {
                                    drow1 = dtIssuingBook.NewRow();
                                    drow1["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                    drow1["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                    drow1["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["name"]);
                                    drow1["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["area_of_project"]);
                                    drow1["Date Of Issue"] = Convert.ToString(CurDate);
                                    DateTime DDate = Convert.ToDateTime(dueDATE);
                                    DateTime ISSDate = Convert.ToDateTime(issueDt);
                                    TimeSpan diff = ISSDate.Subtract(DDate);
                                    int Datedifference = diff.Days;
                                    drow1["Due Days"] = Convert.ToString(Datedifference);
                                    drow1["Due Date"] = Convert.ToString(BookDueDate);
                                    drow1["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                    drow1["Fine"] = Convert.ToString("0");
                                    dtIssuingBook.Rows.Add(drow1);
                                }
                                if (ddlcodenumber.Items.Count > 0)
                                {
                                    string token = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                    ddlcodenumber.Items.Remove(token);
                                }
                            }
                            issue_type = "PRO";
                        }
                        else if (Convert.ToString(ds.Tables[0].Rows[0]["issue_flag"]).ToUpper() == "LOST")
                        {
                            DivMess.Visible = true;
                            LblMessage.Text = "This Project Book is Trashed out. Do you want to change status and want to Issue?";
                            if (SureYes == true)
                            {
                                sql = "update project_book set issue_flag='Available' where lib_code='" + libraryCode + "'  and ltrim(rtrim(probook_accno))='" + acc_NO + "'";

                                IsSelected(Txtaccno.Text);
                                if (!IsSelectedVal)
                                {
                                    if (ViewState["CurrentTable"] != null)
                                    {
                                        dtIssuingBook = (DataTable)ViewState["CurrentTable"];
                                        drCurrentRow = null;
                                        if (dtIssuingBook.Rows.Count > 0)
                                        {
                                            drCurrentRow = dtIssuingBook.NewRow();
                                            drCurrentRow["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                            drCurrentRow["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                            drCurrentRow["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["name"]);
                                            drCurrentRow["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["area_of_project"]);
                                            drCurrentRow["Date Of Issue"] = Convert.ToString(CurDate);
                                            DateTime DDate = Convert.ToDateTime(dueDATE);
                                            DateTime ISSDate = Convert.ToDateTime(issueDt);
                                            TimeSpan diff = ISSDate.Subtract(DDate);
                                            int Datedifference = diff.Days;
                                            drCurrentRow["Due Days"] = Convert.ToString(Datedifference);
                                            drCurrentRow["Due Date"] = Convert.ToString(BookDueDate);
                                            drCurrentRow["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                            drCurrentRow["Fine"] = Convert.ToString("0");
                                            dtIssuingBook.Rows.Add(drCurrentRow);
                                        }
                                    }
                                    else
                                    {
                                        drow1 = dtIssuingBook.NewRow();
                                        drow1["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                        drow1["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                        drow1["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["name"]);
                                        drow1["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["area_of_project"]);
                                        drow1["Date Of Issue"] = Convert.ToString(CurDate);

                                        DateTime DDate = Convert.ToDateTime(dueDATE);
                                        DateTime ISSDate = Convert.ToDateTime(issueDt);
                                        TimeSpan diff = ISSDate.Subtract(DDate);
                                        int Datedifference = diff.Days;
                                        drow1["Due Days"] = Convert.ToString(Datedifference);

                                        drow1["Due Date"] = Convert.ToString(BookDueDate);
                                        drow1["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                        drow1["Fine"] = Convert.ToString("0");

                                        dtIssuingBook.Rows.Add(drow1);
                                    }
                                    if (ddlcodenumber.Items.Count > 0)
                                    {
                                        string token = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                        ddlcodenumber.Items.Remove(token);
                                    }
                                }
                                issue_type = "PRO";
                                sql = "SELECT title,name from project_book where lib_code='" + libraryCode + "'  and ltrim(rtrim(probook_accno))='" + acc_NO + "'";
                            }
                            else
                                Txtaccno.Text = "";

                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alertMsg.Text = "Check Access No.";
                            bodate = "No";
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alertMsg.Text = "Check Access No.";
                        bodate = "No";
                    }
                }
                //<<<<<<<<<<<<<<<<< SEARCHING FOR NONBOOKMATERIAL >>>>>>>>>>>>>>>>>>>>>>>>>
                else if (issueType == "Non-Book Material")
                {
                    sql = "SELECT title,author,issue_flag,publisher from nonbookmat where lib_code='" + libraryCode + "' and ltrim(rtrim(nonbookmat_no)) ='" + acc_NO + "' ";
                    ds = d2.select_method_wo_parameter(sql, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToString(ds.Tables[0].Rows[0]["issue_flag"]).ToUpper() == "AVAILABLE")
                        {
                            //Image1.Picture = LoadPicture(photoAccess(photoGet, book, txt_accno.Text, GetLibraryCode(cbo_library.Text), "PRO"))
                            IsSelected(Txtaccno.Text);
                            if (!IsSelectedVal)
                            {
                                if (ViewState["CurrentTable"] != null)
                                {
                                    dtIssuingBook = (DataTable)ViewState["CurrentTable"];
                                    drCurrentRow = null;
                                    if (dtIssuingBook.Rows.Count > 0)
                                    {
                                        drCurrentRow = dtIssuingBook.NewRow();
                                        drCurrentRow["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                        drCurrentRow["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                        drCurrentRow["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["author"]);
                                        drCurrentRow["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["Publisher"]);
                                        drCurrentRow["Date Of Issue"] = Convert.ToString(CurDate);
                                        DateTime DDate = Convert.ToDateTime(dueDATE);
                                        DateTime ISSDate = Convert.ToDateTime(issueDt);
                                        TimeSpan diff = ISSDate.Subtract(DDate);
                                        int Datedifference = diff.Days;
                                        drCurrentRow["Due Days"] = Convert.ToString(Datedifference);
                                        drCurrentRow["Due Date"] = Convert.ToString(BookDueDate);
                                        drCurrentRow["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                        drCurrentRow["Fine"] = Convert.ToString("0");
                                        dtIssuingBook.Rows.Add(drCurrentRow);
                                    }
                                }
                                else
                                {
                                    drow1 = dtIssuingBook.NewRow();
                                    drow1["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                    drow1["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                    drow1["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["author"]);
                                    drow1["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["Publisher"]);
                                    drow1["Date Of Issue"] = Convert.ToString(CurDate);

                                    DateTime DDate = Convert.ToDateTime(dueDATE);
                                    DateTime ISSDate = Convert.ToDateTime(issueDt);
                                    TimeSpan diff = ISSDate.Subtract(DDate);
                                    int Datedifference = diff.Days;
                                    drow1["Due Days"] = Convert.ToString(Datedifference);
                                    drow1["Due Date"] = Convert.ToString(BookDueDate);
                                    drow1["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                    drow1["Fine"] = Convert.ToString("0");

                                    dtIssuingBook.Rows.Add(drow1);
                                }

                                if (ddlcodenumber.Items.Count > 0)
                                {
                                    string token = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                    ddlcodenumber.Items.Remove(token);
                                }
                            }
                            //issue_type = "NBM";
                        }
                        else if (Convert.ToString(ds.Tables[0].Rows[0]["issue_flag"]).ToUpper() == "LOST")
                        {
                            DivMess.Visible = true;
                            LblMessage.Text = "This NonBookMaterial is Trashed out. Do you want to change the status and want to issue?";
                            if (SureYes == true)
                            {
                                sql = "update nonbookmat set issue_flag='Available' where  lib_code='" + libraryCode + "' and ltrim(rtrim(nonbookmat_no)) ='" + acc_NO + "' ";
                                rssearch = d2.select_method_wo_parameter(sql, "text");
                                IsSelected(Txtaccno.Text);
                                if (!IsSelectedVal)
                                {
                                    if (ViewState["CurrentTable"] != null)
                                    {
                                        dtIssuingBook = (DataTable)ViewState["CurrentTable"];
                                        drCurrentRow = null;
                                        if (dtIssuingBook.Rows.Count > 0)
                                        {
                                            drCurrentRow = dtIssuingBook.NewRow();
                                            drCurrentRow["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                            drCurrentRow["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                            drCurrentRow["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["author"]);
                                            drCurrentRow["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["Publisher"]);
                                            drCurrentRow["Date Of Issue"] = Convert.ToString(CurDate);
                                            DateTime DDate = Convert.ToDateTime(dueDATE);
                                            DateTime ISSDate = Convert.ToDateTime(issueDt);
                                            TimeSpan diff = ISSDate.Subtract(DDate);
                                            int Datedifference = diff.Days;
                                            drCurrentRow["Due Days"] = Convert.ToString(Datedifference);
                                            drCurrentRow["Due Date"] = Convert.ToString(BookDueDate);
                                            drCurrentRow["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                            drCurrentRow["Fine"] = Convert.ToString("0");
                                            dtIssuingBook.Rows.Add(drCurrentRow);
                                        }
                                    }
                                    else
                                    {
                                        drow1 = dtIssuingBook.NewRow();
                                        drow1["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                        drow1["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                        drow1["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["author"]);
                                        drow1["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["Publisher"]);
                                        drow1["Date Of Issue"] = Convert.ToString(CurDate);
                                        DateTime DDate = Convert.ToDateTime(dueDATE);
                                        DateTime ISSDate = Convert.ToDateTime(issueDt);
                                        TimeSpan diff = ISSDate.Subtract(DDate);
                                        int Datedifference = diff.Days;
                                        drow1["Due Days"] = Convert.ToString(Datedifference);
                                        drow1["Due Date"] = Convert.ToString(BookDueDate);
                                        drow1["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                        drow1["Fine"] = Convert.ToString("0");
                                        dtIssuingBook.Rows.Add(drow1);
                                    }
                                    if (ddlcodenumber.Items.Count > 0)
                                    {
                                        string token = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                        ddlcodenumber.Items.Remove(token);
                                    }
                                }
                                issue_type = "NBM";
                            }
                            else
                                Txtaccno.Text = "";
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alertMsg.Text = "Check Access No.";
                            bodate = "No";
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alertMsg.Text = "Check Access No.";
                        bodate = "No";
                    }
                }
                //<<<<<<<<<<<<<<<<< SEARCHING FOR QUESTION BANK>>>>>>>>>>>>>>>>>>>>>>>>>
                else if (issueType == "Question Bank")
                {
                    sql = "SELECT title,paper_name,sem_year,issue_flag from university_question where ltrim(rtrim(access_code))='" + acc_NO + "' and lib_code='" + libraryCode + "' ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sql, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToString(ds.Tables[0].Rows[0]["issue_flag"]).ToUpper() == "AVAILABLE")
                        {

                            IsSelected(Txtaccno.Text);
                            if (!IsSelectedVal)
                            {
                                if (ViewState["CurrentTable"] != null)
                                {
                                    dtIssuingBook = (DataTable)ViewState["CurrentTable"];
                                    drCurrentRow = null;
                                    if (dtIssuingBook.Rows.Count > 0)
                                    {
                                        drCurrentRow = dtIssuingBook.NewRow();
                                        drCurrentRow["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                        drCurrentRow["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                        drCurrentRow["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["paper_name"]);
                                        drCurrentRow["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["sem_year"]);
                                        drCurrentRow["Date Of Issue"] = Convert.ToString(CurDate);
                                        DateTime DDate = Convert.ToDateTime(dueDATE);
                                        DateTime ISSDate = Convert.ToDateTime(issueDt);
                                        TimeSpan diff = ISSDate.Subtract(DDate);
                                        int Datedifference = diff.Days;
                                        drCurrentRow["Due Days"] = Convert.ToString(Datedifference);
                                        drCurrentRow["Due Date"] = Convert.ToString(BookDueDate);
                                        drCurrentRow["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                        drCurrentRow["Fine"] = Convert.ToString("0");
                                        dtIssuingBook.Rows.Add(drCurrentRow);
                                    }
                                }
                                else
                                {
                                    drow1 = dtIssuingBook.NewRow();
                                    drow1["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                    drow1["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                    drow1["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["paper_name"]);
                                    drow1["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["sem_year"]);
                                    drow1["Date Of Issue"] = Convert.ToString(CurDate);
                                    DateTime DDate = Convert.ToDateTime(dueDATE);
                                    DateTime ISSDate = Convert.ToDateTime(issueDt);
                                    TimeSpan diff = ISSDate.Subtract(DDate);
                                    int Datedifference = diff.Days;
                                    drow1["Due Days"] = Convert.ToString(Datedifference);
                                    drow1["Due Date"] = Convert.ToString(BookDueDate);
                                    drow1["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                    drow1["Fine"] = Convert.ToString("0");
                                    dtIssuingBook.Rows.Add(drow1);
                                }
                                if (ddlcodenumber.Items.Count > 0)
                                {
                                    string token = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                    ddlcodenumber.Items.Remove(token);
                                }
                            }
                            issue_type = "QBA";
                        }
                        else if (Convert.ToString(ds.Tables[0].Rows[0]["issue_flag"]).ToUpper() == "LOST")
                        {
                            DivMess.Visible = true;
                            LblMessage.Text = "This Question Paper is Trashed out. Do you want to chage the status and want to issue?";
                            if (SureYes == true)
                            {
                                sql = "update univerysity_question set issue_flag='Available' where ltrim(rtrim(access_code))='" + acc_NO + "' and lib_code='" + libraryCode + "' ";
                                update = d2.update_method_wo_parameter(sql, "text");
                                IsSelected(Txtaccno.Text);
                                if (!IsSelectedVal)
                                {
                                    if (ViewState["CurrentTable"] != null)
                                    {
                                        dtIssuingBook = (DataTable)ViewState["CurrentTable"];
                                        drCurrentRow = null;
                                        if (dtIssuingBook.Rows.Count > 0)
                                        {
                                            drCurrentRow = dtIssuingBook.NewRow();
                                            drCurrentRow["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                            drCurrentRow["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                            drCurrentRow["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["paper_name"]);
                                            drCurrentRow["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["sem_year"]);
                                            drCurrentRow["Date Of Issue"] = Convert.ToString(CurDate);
                                            DateTime DDate = Convert.ToDateTime(dueDATE);
                                            DateTime ISSDate = Convert.ToDateTime(issueDt);
                                            TimeSpan diff = ISSDate.Subtract(DDate);
                                            int Datedifference = diff.Days;
                                            drCurrentRow["Due Days"] = Convert.ToString(Datedifference);
                                            drCurrentRow["Due Date"] = Convert.ToString(BookDueDate);
                                            drCurrentRow["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                            drCurrentRow["Fine"] = Convert.ToString("0");
                                            dtIssuingBook.Rows.Add(drCurrentRow);
                                        }
                                    }
                                    else
                                    {
                                        drow1 = dtIssuingBook.NewRow();
                                        drow1["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                        drow1["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                        drow1["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["paper_name"]);
                                        drow1["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["sem_year"]);
                                        drow1["Date Of Issue"] = Convert.ToString(CurDate);
                                        DateTime DDate = Convert.ToDateTime(dueDATE);
                                        DateTime ISSDate = Convert.ToDateTime(issueDt);
                                        TimeSpan diff = ISSDate.Subtract(DDate);
                                        int Datedifference = diff.Days;
                                        drow1["Due Days"] = Convert.ToString(Datedifference);
                                        drow1["Due Date"] = Convert.ToString(BookDueDate);
                                        drow1["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                        drow1["Fine"] = Convert.ToString("0");
                                        dtIssuingBook.Rows.Add(drow1);
                                    }
                                    if (ddlcodenumber.Items.Count > 0)
                                    {
                                        string token = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                        ddlcodenumber.Items.Remove(token);
                                    }
                                }
                                issue_type = "QBA";
                            }
                            else
                                Txtaccno.Text = "";
                        }
                        else if (Convert.ToString(ds.Tables[0].Rows[0]["issue_flag"]).ToUpper() == "ISSUED")
                        {
                            sql = "SELECT * FROM Borrow WHERE Acc_No ='" + acc_NO + "' AND Return_Flag = 0 AND Lib_Code ='" + libraryCode + "' AND Return_Type = 'QBA' ";
                            dsCommon.Clear();
                            dsCommon = d2.select_method_wo_parameter(sql, "text");
                            if (dsCommon.Tables[0].Rows.Count > 0)
                            {
                                imgdiv2.Visible = true;
                                lbl_alertMsg.Text = "Book Issued to Roll No. " + Convert.ToString(dsCommon.Tables[0].Rows[0]["Roll_No"]);
                                bodate = "No";
                            }
                            else
                            {
                                imgdiv2.Visible = true;
                                lbl_alertMsg.Text = "Book already issued";
                            }
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alertMsg.Text = "Check Access No.";
                            bodate = "No";
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alertMsg.Text = "Check Access No.";
                        bodate = "No";
                    }
                }
                //<<<<<<<<<<<<<<<<< SEARCHING FOR BACK VOLUME >>>>>>>>>>>>>>>>>>>>>>>>>
                else if (issueType == "Back Volume")
                {
                    sql = "SELECT * from back_volume where ltrim(rtrim(access_code))='" + acc_NO + "' and lib_code='" + libraryCode + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sql, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToString(ds.Tables[0].Rows[0]["issue_flag"]).ToUpper() == "AVAILABLE")
                        {
                            //Image1.Picture = LoadPicture(photoAccess(photoGet, book, txt_accno.Text, GetLibraryCode(cbo_library.Text), "BVO"))   
                            IsSelected(Txtaccno.Text);
                            if (!IsSelectedVal)
                            {
                                if (ViewState["CurrentTable"] != null)
                                {
                                    dtIssuingBook = (DataTable)ViewState["CurrentTable"];
                                    drCurrentRow = null;
                                    if (dtIssuingBook.Rows.Count > 0)
                                    {
                                        drCurrentRow = dtIssuingBook.NewRow();
                                        drCurrentRow["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                        drCurrentRow["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                        drCurrentRow["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["Publisher"]);
                                        drCurrentRow["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["journal_year"]);
                                        drCurrentRow["Date Of Issue"] = Convert.ToString(CurDate);
                                        DateTime DDate = Convert.ToDateTime(dueDATE);
                                        DateTime ISSDate = Convert.ToDateTime(issueDt);
                                        TimeSpan diff = ISSDate.Subtract(DDate);
                                        int Datedifference = diff.Days;
                                        drCurrentRow["Due Days"] = Convert.ToString(Datedifference);
                                        drCurrentRow["Due Date"] = Convert.ToString(BookDueDate);
                                        drCurrentRow["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                        drCurrentRow["Fine"] = Convert.ToString("0");
                                        dtIssuingBook.Rows.Add(drCurrentRow);
                                    }
                                }
                                else
                                {
                                    drow1 = dtIssuingBook.NewRow();
                                    drow1["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                    drow1["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                    drow1["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["Publisher"]);
                                    drow1["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["journal_year"]);
                                    drow1["Date Of Issue"] = Convert.ToString(CurDate);
                                    DateTime DDate = Convert.ToDateTime(dueDATE);
                                    DateTime ISSDate = Convert.ToDateTime(issueDt);
                                    TimeSpan diff = ISSDate.Subtract(DDate);
                                    int Datedifference = diff.Days;
                                    drow1["Due Days"] = Convert.ToString(Datedifference);
                                    drow1["Due Date"] = Convert.ToString(BookDueDate);
                                    drow1["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                    drow1["Fine"] = Convert.ToString("0");
                                    dtIssuingBook.Rows.Add(drow1);
                                }
                                if (ddlcodenumber.Items.Count > 0)
                                {
                                    string token = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                    ddlcodenumber.Items.Remove(token);
                                }
                            }
                            issue_type = "BVO";
                        }
                        else if (Convert.ToString(ds.Tables[0].Rows[0]["issue_flag"]).ToUpper() == "Lost")
                        {
                            DivMess.Visible = true;
                            LblMessage.Text = "This Back Volume is Trashed out. Do you want to change the status and want to issue?";
                            if (SureYes == true)
                            {
                                sql = "update back_volume set issue_flag='Available' where ltrim(rtrim(access_code))='" + acc_NO + "' and lib_code='" + libraryCode + "'";
                                update = d2.update_method_wo_parameter(sql, "text");
                                IsSelected(Txtaccno.Text);
                                if (!IsSelectedVal)
                                {
                                    if (ViewState["CurrentTable"] != null)
                                    {
                                        dtIssuingBook = (DataTable)ViewState["CurrentTable"];
                                        drCurrentRow = null;
                                        if (dtIssuingBook.Rows.Count > 0)
                                        {
                                            drCurrentRow = dtIssuingBook.NewRow();
                                            drCurrentRow["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                            drCurrentRow["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                            drCurrentRow["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["Publisher"]);
                                            drCurrentRow["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["journal_year"]);
                                            drCurrentRow["Date Of Issue"] = Convert.ToString(CurDate);
                                            DateTime DDate = Convert.ToDateTime(dueDATE);
                                            DateTime ISSDate = Convert.ToDateTime(issueDt);
                                            TimeSpan diff = ISSDate.Subtract(DDate);
                                            int Datedifference = diff.Days;
                                            drCurrentRow["Due Days"] = Convert.ToString(Datedifference);
                                            drCurrentRow["Due Date"] = Convert.ToString(BookDueDate);
                                            drCurrentRow["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                            drCurrentRow["Fine"] = Convert.ToString("0");
                                            dtIssuingBook.Rows.Add(drCurrentRow);
                                        }
                                    }
                                    else
                                    {
                                        drow1 = dtIssuingBook.NewRow();
                                        drow1["Access No"] = Convert.ToString(ds.Tables[0].Rows[0]["acc_no"]);
                                        drow1["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                                        drow1["Author"] = Convert.ToString(ds.Tables[0].Rows[0]["Publisher"]);
                                        drow1["Call No"] = Convert.ToString(ds.Tables[0].Rows[0]["journal_year"]);
                                        drow1["Date Of Issue"] = Convert.ToString(CurDate);
                                        DateTime DDate = Convert.ToDateTime(dueDATE);
                                        DateTime ISSDate = Convert.ToDateTime(issueDt);
                                        TimeSpan diff = ISSDate.Subtract(DDate);
                                        int Datedifference = diff.Days;
                                        drow1["Due Days"] = Convert.ToString(Datedifference);
                                        drow1["Due Date"] = Convert.ToString(BookDueDate);
                                        drow1["Token No"] = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                        drow1["Fine"] = Convert.ToString("0");
                                        dtIssuingBook.Rows.Add(drow1);
                                    }
                                    if (ddlcodenumber.Items.Count > 0)
                                    {
                                        string token = Convert.ToString(ddlcodenumber.SelectedItem.Text);
                                        ddlcodenumber.Items.Remove(token);
                                    }
                                }
                                issue_type = "BVO";
                            }
                            else
                                Txtaccno.Text = "";
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alertMsg.Text = "Check Access No.";
                            bodate = "No";
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alertMsg.Text = "Check Access No.";
                        bodate = "No";
                    }
                }
                ViewState["CurrentTable"] = dtIssuingBook;
                GrdIssuingBook.DataSource = dtIssuingBook;
                GrdIssuingBook.DataBind();
                GrdIssuingBook.Visible = true;
                for (int l = 0; l < GrdIssuingBook.Rows.Count; l++)
                {
                    foreach (GridViewRow row in GrdIssuingBook.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            GrdIssuingBook.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            GrdIssuingBook.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                            GrdIssuingBook.Rows[l].Cells[2].HorizontalAlign = HorizontalAlign.Center;
                            GrdIssuingBook.Rows[l].Cells[7].HorizontalAlign = HorizontalAlign.Right;
                            GrdIssuingBook.Rows[l].Cells[10].HorizontalAlign = HorizontalAlign.Right;
                        }
                    }
                }
            }
            #endregion
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collcode, "bookissue.aspx");
        }
    }

    protected void BtnRemove_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvrow in GrdIssuingBook.Rows)
        {
            System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                string token = Convert.ToString(GrdIssuingBook.Rows[RowCnt].Cells[9].Text);
                ddlcodenumber.Items.Add(token);
                if (ViewState["CurrentTable"] != null)
                {
                    DataTable dt = (DataTable)ViewState["CurrentTable"];
                    dt.Rows.RemoveAt(RowCnt);
                    ViewState["CurrentTable"] = dt;
                    GrdIssuingBook.DataSource = dt;
                    GrdIssuingBook.DataBind();
                    GrdIssuingBook.Visible = true;
                }
            }
        }
        if (Remove == true)
        {
            foreach (GridViewRow gvrow in GrdIssuingBook.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("CheckBox1");
                if (chk.Checked == true)
                {
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    string token = Convert.ToString(GrdIssuingBook.Rows[RowCnt].Cells[9].Text);
                    ddlcodenumber.Items.Add(token);
                    GrdIssuingBook.Rows[RowCnt].Visible = false;
                }
            }
        }
    }

    public void SetPreviousData()
    {
        try
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                Hashtable hashlist = new Hashtable();
                if (dt.Rows.Count > 0)
                {
                    hashlist.Add(0, "Access No");
                    hashlist.Add(1, "Title");
                    hashlist.Add(2, "Author");
                    hashlist.Add(3, "Call No");
                    hashlist.Add(4, "Date Of Issue");
                    hashlist.Add(5, "Due Days");
                    hashlist.Add(6, "Due Date");
                    hashlist.Add(7, "Token No");
                    hashlist.Add(8, "Fine");
                    DataRow dr;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string accNo = dt.Rows[i][0].ToString();
                        string title = dt.Rows[i][1].ToString();
                        string Author = dt.Rows[i][2].ToString();
                        string CllNo = dt.Rows[i][3].ToString();
                        string DtOfIss = dt.Rows[i][4].ToString();
                        string DueDays = dt.Rows[i][5].ToString();
                        string Duedt = dt.Rows[i][6].ToString();
                        string TokenNo = dt.Rows[i][7].ToString();
                        string Fine = dt.Rows[i][8].ToString();
                        string val_file = Convert.ToString(hashlist[i]);
                        rowIndex++;
                    }
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    //Book Save Details

    protected void btn_Yes_Click(object sender, EventArgs e)
    {
        try
        {
            if (RefBook == false)
            {
                DivMess.Visible = false;
                string Sql = "";
                string insert = "";
                int Updt = 0;
                string msg = "";
                string rackNo = "";
                string rowNo = "";
                int noOfCopy = 0;
                int noOfCopiesRacRow = 0;
                string book_type = "";
                string qry = "";
                string StrSql = "";
                int LinkA = 0;
                int LinkB = 0;
                int issuedVal = 0;
                string Library = Convert.ToString(ddllibrary.SelectedValue);
                string LibraryName = Convert.ToString(ddllibrary.SelectedItem.Text);
                string Libcode = Convert.ToString(Cbo_CardLibrary.SelectedValue);
                string college = Convert.ToString(ddlcollege.SelectedValue);
                string IssuedBy = d2.GetFunction("select Full_Name from usermaster where user_code='" + userCode + "'");
                string StrBookType = Convert.ToString(ddlBookType.SelectedValue);
                string cardCriteria = Convert.ToString(ddlCardType.SelectedValue);

                string serverDt = d2.ServerDate();
                string servertime = d2.ServerTime();
                string CurDate = serverDt.Split(' ')[0];
                string Time = servertime;
                string category = Convert.ToString(Session["category"]);
                //SpreadIssuingBook.SaveChanges();
                string issueType = Convert.ToString(ddlissue.SelectedValue);
                string IssueDate = txtissuedate.Text;
                string DueDate = Txtduedate.Text;
                string bookIss_dt = txtissuedate.Text;
                if (issueType == "Book")
                    book_type = "BOK";
                if (issueType == "Periodicals")
                    book_type = "PER";
                if (issueType == "Project Book")
                    book_type = "PRO";
                if (issueType == "Non-Book Material")
                    book_type = "NBM";
                if (issueType == "Question Bank")
                    book_type = "QBA";
                if (issueType == "Back Volume")
                    book_type = "BVO";
                if (issueType == "Reference Books")
                    book_type = "REF";
                if (rblissue.SelectedIndex == 0)
                {
                    string[] dtIssue = IssueDate.Split('/');
                    if (dtIssue.Length == 3)
                        IssueDate = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                    for (int i = 0; i < GrdIssuingBook.Rows.Count; i++)
                    {
                        varAccno = Convert.ToString(GrdIssuingBook.Rows[i].Cells[2].Text);
                        vartitle = Convert.ToString(GrdIssuingBook.Rows[i].Cells[3].Text);
                        varauthor = Convert.ToString(GrdIssuingBook.Rows[i].Cells[4].Text);
                        varCallNo = Convert.ToString(GrdIssuingBook.Rows[i].Cells[5].Text);
                        varcTokenNo = Convert.ToString(GrdIssuingBook.Rows[i].Cells[9].Text);
                        varRDueDay = Convert.ToString(GrdIssuingBook.Rows[i].Cells[7].Text);
                        VarRDueDate = Convert.ToString(GrdIssuingBook.Rows[i].Cells[8].Text);

                        string[] Duedt = VarRDueDate.Split('/');
                        if (Duedt.Length == 3)
                            VarRDueDate = Duedt[1].ToString() + "/" + Duedt[0].ToString() + "/" + Duedt[2].ToString();

                        Sql = "SELECT * FROM Borrow WHERE Acc_No ='" + varAccno + "' AND Return_Flag = 0 AND Return_Type ='" + book_type + "' AND Lib_Code ='" + Library + "' ";
                        dsload.Clear();
                        dsload = d2.select_method_wo_parameter(Sql, "text");
                        if (dsload.Tables[0].Rows.Count > 0)
                        {
                            imgdiv2.Visible = true;
                            lbl_alertMsg.Text = "Access No. " + varAccno + " already issued to Roll No. : " + Convert.ToString(dsload.Tables[0].Rows[0]["Roll_No"]) + " Name : " + Convert.ToString(dsload.Tables[0].Rows[0]["Stud_Name"]);
                            return;
                        }
                        //=======Reservation Details============================'
                        if (category == "Staff")
                        {
                            Sql = "update priority_studstaff set cancel_flag=2,PriorityNo=0 where staff_code='" + StrSaveRollNo + "' and access_number='" + varAccno + "'";
                            update = d2.update_method_wo_parameter(Sql, "text");
                            Sql = "update priority_studstaff set cancel_flag=2,PriorityNo=0 where staff_code='" + StrSaveRollNo + "' and OtherAcc_no ='" + varAccno + "'";
                            update = d2.update_method_wo_parameter(Sql, "text");
                        }
                        else if (category == "Student")
                        {
                            Sql = "update priority_studstaff set cancel_flag=2,PriorityNo=0 where roll_no='" + StrSaveRollNo + "' and access_number='" + varAccno + "'";
                            update = d2.update_method_wo_parameter(Sql, "text");
                            Sql = "update priority_studstaff set cancel_flag=2,PriorityNo=0 where roll_no='" + StrSaveRollNo + "' and OtherAcc_no='" + varAccno + "'";
                            update = d2.update_method_wo_parameter(Sql, "text");
                        }
                        //=======================================================//
                        int issueCnt = Convert.ToInt32(TxtissuedCount.Text) + 1;
                        if (category == "Staff")
                        {
                            insert = "insert into borrow(acc_no,title,author,call_no,token_no,roll_no,stud_name,is_staff,borrow_date,due_date,return_date,return_type,access_date,access_time,lib_code,return_flag,cirno_issue,book_issuedby,renewflag,renewaltimes,Issued_Time,Returned_Time) values ('" + varAccno + "','" + vartitle + "','" + varauthor + "','" + varCallNo + "','" + varcTokenNo + "','" + StrSaveRollNo + "','" + TxtName.Text + "',1,'" + IssueDate + "','" + VarRDueDate + "','" + CurDate + "','" + book_type + "','" + CurDate + "','" + Time + "','" + Library + "',0," + issueCnt + ",'" + IssuedBy + "',0,0,'" + Time + "','')";
                        }
                        else if (category == "Student")
                        {
                            insert = "insert into borrow(acc_no,title,author,call_no,token_no,roll_no,stud_name,is_staff,borrow_date,due_date,return_date,return_type,access_date,access_time,lib_code,return_flag,cirno_issue,book_issuedby,renewflag,renewaltimes,Issued_Time,Returned_Time) values ('" + varAccno + "','" + vartitle + "','" + varauthor + "','" + varCallNo + "','" + varcTokenNo + "','" + StrSaveRollNo + "','" + TxtName.Text + "',0,'" + IssueDate + "','" + VarRDueDate + "','" + CurDate + "','" + book_type + "','" + CurDate + "','" + Time + "','" + Library + "',0," + issueCnt + ",'" + IssuedBy + "',0,0,'" + Time + "','')";
                        }
                        else if (category == "Nonmember")
                        {
                            insert = "insert into borrow(acc_no,title,author,call_no,token_no,roll_no,stud_name,is_staff,borrow_date,due_date,return_date,return_type,access_date,access_time,lib_code,return_flag,cirno_issue,book_issuedby,renewflag,renewaltimes,Issued_Time,Returned_Time) values ('" + varAccno + "','" + vartitle + "','" + varauthor + "','" + varCallNo + "','" + varcTokenNo + "','" + StrSaveRollNo + "','" + TxtName.Text + "',2,'" + IssueDate + "','" + VarRDueDate + "','" + CurDate + "','" + book_type + "','" + CurDate + "','" + Time + "','" + Library + "',0," + issueCnt + ",'" + IssuedBy + "',0,0,'" + Time + "','')";
                        }

                        Updt = d2.update_method_wo_parameter(insert, "text");

                        if (issueType == "Book" || issueType == "Reference Books")
                            Sql = "UPDATE bookdetails set book_status='Issued' where acc_no='" + varAccno + "' and lib_code='" + Library + "'";
                        else if (issueType == "Periodicals")
                            Sql = "UPDATE journal set issue_flag ='Issued' where access_code='" + varAccno + "' and lib_code='" + Library + "'";
                        else if (issueType == "Project Book")
                            Sql = "UPDATE project_book set issue_flag='Issued' where probook_accno='" + varAccno + "' and lib_code='" + Library + "'";
                        else if (issueType == "Non-Book Material")
                            Sql = "UPDATE nonbookmat set issue_flag='Issued' where nonbookmat_no='" + varAccno + "' and lib_code='" + Library + "'";
                        else if (issueType == "Question Bank")
                            Sql = "UPDATE university_question set issue_flag='Issued' where access_code='" + varAccno + "' and lib_code='" + Library + "'";
                        else if (issueType == "Back Volume")
                            Sql = "UPDATE back_volume set issue_flag='Issued' where access_code='" + varAccno + "' and lib_code='" + Library + "'";

                        Updt = d2.update_method_wo_parameter(Sql, "text");

                        Sql = "UPDATE tokendetails set is_locked=1 where token_no='" + varcTokenNo + "' ";
                        Updt = d2.update_method_wo_parameter(Sql, "text");

                        //=====Updation of rack status=============================//

                        Sql = "select * from rack_allocation where lib_code='" + Library + "' and acc_no='" + varAccno + "' and rack_no <>'' and row_no <>'' and book_type='" + book_type + "'";
                        dsCommon.Clear();
                        dsCommon = d2.select_method_wo_parameter(Sql, "text");
                        if (dsCommon.Tables[0].Rows.Count > 0)
                        {
                            rackNo = d2.GetFunction("select rack_no from rack_allocation where lib_code='" + Library + "'and acc_no='" + varAccno + "' and rack_no <> '' and book_type='" + book_type + "'");
                            if (rackNo != "")
                            {
                                noOfCopy = Convert.ToInt32(d2.GetFunction("select no_of_copies from rack_master where lib_code='" + Library + "'and rack_no='" + rackNo + "'"));
                                noOfCopy = noOfCopy - 1;
                            }
                            else
                                noOfCopy = 0;

                            rowNo = d2.GetFunction("select row_no from rack_allocation where lib_code='" + Library + "'and acc_no='" + varAccno + "' and row_no <> '' and book_type='" + book_type + "'");
                            if (rowNo != "")
                            {
                                noOfCopiesRacRow = Convert.ToInt32(d2.GetFunction("select no_of_copies from rackrow_master where lib_code='" + Library + "'and rack_no='" + rackNo + "' and row_no='" + rowNo + "'"));
                                noOfCopiesRacRow = noOfCopiesRacRow - 1;
                            }
                            else
                                noOfCopiesRacRow = 0;

                            Sql = "update rack_master set no_of_copies='" + noOfCopy + "' where lib_code='" + Library + "' and rack_no='" + rackNo + "'";
                            update = d2.update_method_wo_parameter(Sql, "text");
                            Sql = "update rackrow_master set no_of_copies='" + noOfCopiesRacRow + "' where lib_code='" + Library + "' and rack_no='" + rackNo + "' and row_no='" + rowNo + "'";
                            update = d2.update_method_wo_parameter(Sql, "text");

                            qry = "select * from rack_allocation_back where lib_code='" + Library + "' and acc_no='" + varAccno + "' and book_type='" + book_type + "'";
                            dsload.Clear();
                            dsload = d2.select_method_wo_parameter(qry, "text");
                            if (dsload.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rack_allocation_back set rack_no=(select rack_no from rack_allocation where lib_code='" + Library + "' and acc_no='" + varAccno + "' and book_type='" + book_type + "'),row_no=(select row_no from rack_allocation where lib_code='" + Library + "' and acc_no='" + varAccno + "' and book_type='" + book_type + "') where lib_code='" + Library + "' and acc_no='" + varAccno + "' and book_type='" + book_type + "'";
                                update = d2.update_method_wo_parameter(Sql, "text");
                            }
                            else
                            {
                                Sql = "insert into rack_allocation_back (lib_code,rack_no,row_no,acc_no,access_date,access_time,book_type,Pos_No,Pos_Place)  select lib_code,rack_no,row_no,acc_no,access_date,access_time,book_type,Pos_No,Pos_Place from rack_allocation where lib_code='" + Library + "' and acc_no='" + varAccno + "' and book_type='" + book_type + "'";
                                update = d2.update_method_wo_parameter(Sql, "text");
                            }
                            Sql = "update rack_allocation set rack_no='',row_no='' where lib_code='" + Library + "' and acc_no='" + varAccno + "' and book_type='" + book_type + "'";
                            update = d2.update_method_wo_parameter(Sql, "text");
                        }

                        issuedVal = Convert.ToInt32(txt_issued.Text) + 1;
                        int issueTotCnt = Convert.ToInt32(TxtissuedCount.Text) + 1;
                    }
                    if (issuedVal == Convert.ToInt32(txt_elgi.Text))
                    {
                        DispStatusList();
                        imgdiv2.Visible = true;
                        lbl_alertMsg.Text = "Issue details Saved";
                        ClearFunction();
                        img_stud1.ImageUrl = "";
                        imgBook.ImageUrl = "";
                        TotIssuedCount();
                        hsAccNo.Clear();
                    }
                    else
                    {
                        DispStatusList();
                        if (Convert.ToInt32(GrdIssuingBook.Rows.Count) >= 1 && rblissue.SelectedIndex != 3)
                        {
                            if (IntDispIssueMes == 1)
                            {
                                string var = Convert.ToString(BtnYes.TabIndex);
                                if (var == "2")
                                {
                                    BtnIssueNoAgain.Focus();
                                    BtnIssueNoAgain.BackColor = Color.LightGreen;
                                }
                                DivIssue.Visible = true;
                                LblIssuesName.Text = "Issue details Saved.Do you want any other books?";
                                hsAccNo.Clear();
                                return;
                            }
                            else
                            {
                                if (rblissue.SelectedIndex == 0)
                                {
                                }
                                else
                                {
                                }
                                Txtaccno.Text = "";
                                return;
                            }
                        }
                    }
                    DivMess.Visible = false;
                }
                if (rblissue.SelectedIndex == 1)
                {
                    cmdReturn_Click();
                }
                if (rblissue.SelectedIndex == 2)
                {
                    cmdRenewal_Click(sender, e);
                }
                if (rblissue.SelectedIndex == 3)
                {
                    cmdReturn_Click();
                }
                int count = Cbo_CardLibrary.Items.Count;
                Cbo_CardLibrary.SelectedIndex = count - 1;
            }
            else
            {
                Page.Form.DefaultFocus = Txtaccno.ClientID;
                DivMess.Visible = false;
                RefBook = false;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collcode, "bookissue.aspx");
        }
    }

    protected void btn_No_Click(object sender, EventArgs e)
    {
        Txtaccno.Text = "";
        DivMess.Visible = false;
        // SureYes = false;
        // SureYestToIssueBook = false;
        int count = Cbo_CardLibrary.Items.Count;
        Cbo_CardLibrary.SelectedIndex = count - 1;
    }

    protected void btnMessYes_Click(object sender, EventArgs e)
    {
        SureYes = true;
        DivMess1.Visible = false;
        int count = Cbo_CardLibrary.Items.Count;
        Cbo_CardLibrary.SelectedIndex = count - 1;
        DispStatusList();
        //Btnsave_Click(sender, e);
    }

    protected void btnMessNo_Click(object sender, EventArgs e)
    {
        DivMess1.Visible = false;
        SureYes = false;
        //SpreadIssuingBook.Sheets[0].RowCount = 0;
        //SpreadBookInHand.Sheets[0].RowCount = 0;
        txtRollNo.Text = "";
        TxtName.Text = "";
        txtDept.Text = "";
        Txtaccno.Text = "";
        ddlcodenumber.Items.Clear();
        txt_elgi.Text = "";
        txt_issued.Text = "";
        txtlocked.Text = "";
        txt_Unlocked.Text = "";
        int count = Cbo_CardLibrary.Items.Count;
        Cbo_CardLibrary.SelectedIndex = count - 1;
        DispStatusList();
    }

    protected void IsSelected(string accNo)
    {
        for (int i = 0; i < GrdIssuingBook.Rows.Count; i++)
        {
            string BookAccNo = Convert.ToString(GrdIssuingBook.Rows[i].Cells[1].Text);
            if (BookAccNo == accNo)
            {
                IsSelectedVal = true;
                return;
            }
        }
        IsSelectedVal = false;
    }

    protected void IsSamtTitle(string title)
    {
        for (int i = 0; i < GrdIssuingBook.Rows.Count; i++)
        {
            string BookName = Convert.ToString(GrdIssuingBook.Rows[i].Cells[2].Text);
            if (BookName == title)
            {
                IsSamTitle = true;
                return;
            }
        }
        IsSamTitle = false;
    }

    protected void check_booksInfo(object sender, EventArgs e)
    {
        DataSet rsnew = new DataSet();
        DataSet rsaccno = new DataSet();
        try
        {
            string studcat = "";

            string sql = "";
            string tokenno = Convert.ToString(ddlcodenumber.SelectedItem.Text);
            string lib = Convert.ToString(ddllibrary.SelectedValue);
            if (tokenno != "")
            {

                sql = "select * from tokendetails where (roll_no=(select roll_no from registration where roll_no ='" + txtRollNo.Text + "') or roll_no = (select lib_id from registration where roll_no ='" + txtRollNo.Text + "') or roll_no =(select reg_no from registration where roll_no ='" + txtRollNo.Text + "')) and token_no='" + tokenno + "'";

                rsnew.Clear();
                rsnew = d2.select_method_wo_parameter(sql, "text");
                if (rsnew.Tables[0].Rows.Count == 0)
                {

                    sql = "select * from tokendetails where (roll_no=(select roll_no from registration where lib_id ='" + txtRollNo.Text + "') or roll_no = (select lib_id from registration where lib_id ='" + txtRollNo.Text + "') or roll_no =(select reg_no from registration where lib_id ='" + txtRollNo.Text + "')) and token_no='" + tokenno + "'";
                    rsnew.Clear();
                    rsnew = d2.select_method_wo_parameter(sql, "text");
                }
                if (rsnew.Tables[0].Rows.Count > 0)
                {
                    studcat = Convert.ToString(rsnew.Tables[0].Rows[0]["studcategory"]);
                }
                if (studcat != "")
                {
                    if (Txtaccno.Text != "")
                    {
                        sql = "select * from bookdetails where acc_no='" + Txtaccno.Text + "' and lib_code ='" + lib + "'";
                        rsaccno.Clear();
                        rsaccno = d2.select_method_wo_parameter(sql, "text");
                        if (rsaccno.Tables[0].Rows.Count > 0)
                        {
                            string category = Convert.ToString(rsaccno.Tables[0].Rows[0]["category"]);
                            if (studcat == "SC/ST Category" && category.ToUpper() == "BOOK BANK")
                            {
                            }
                            else if ((studcat == "others" || studcat == "All") && category.ToUpper() == "BOOK BANK")
                            {
                                imgdiv2.Visible = true;
                                lbl_alertMsg.Text = "This Book in Book Bank,Can't Issue This Book";
                                Txtaccno.Text = "";
                                check_bookInfo = true;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void GetRenewalDays(int pintRenCount, int pintRenDays, string pstrAccNo)
    {
        try
        {
            string Sql = "";
            int a = 0;
            int b = 0;
            string StrBookType = Convert.ToString(ddlissue.SelectedValue);
            string cardCriteria = Convert.ToString(ddlCardType.SelectedValue);
            string Libcode = Convert.ToString(Cbo_CardLibrary.SelectedValue);
            string college = Convert.ToString(ddlcollege.SelectedValue);
            string linkVal = d2.GetFunction("select LinkValue from inssettings where LinkName='Renewal Permission' and College_Code=" + college + "");
            string[] LinkValue = linkVal.Split('/');
            if (LinkValue.Length > 0)
            {
                a = Convert.ToInt32(LinkValue[0]);
            }
            Sql = "SELECT ISNULL(Renew_Days,0) as renewDays FROM TokenDetails WHERE Roll_No='" + txtRollNo.Text + "' ";

            if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
            else if (BlnBookBankLib == true && BlnBookBankAll == true)
                Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
            else
                Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";
            if (Cbo_CardLibrary.SelectedItem.Text != "All")
                Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
            else Sql += "AND ISNULL(TransLibCode,'All') ='All'";
            if (ddlBookType.SelectedItem.Text != "All")
                Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
            else
                Sql += "AND ISNULL(Book_Type,'All') ='All' ";
            if (cardCriteria != "All")
                Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
            else
                Sql += "AND ISNULL(CardCat,'All') ='All' ";
            string renewval = d2.GetFunction(Sql);
            if (!string.IsNullOrEmpty(renewval))
                b = Convert.ToInt32(renewval);
            if (b == 0)
            {
                string Link = d2.GetFunction("select LinkValue from inssettings where LinkName='Renewal Permission' and College_Code=" + college + "");
                string[] arr = Link.Split('/');
                if (arr.Length > 0)
                {
                    a = Convert.ToInt32(arr[0]);
                    b = Convert.ToInt32(arr[1]);
                }
            }
            if (a == 1 && b > 0)
            {
                int var = 0;
                var = Convert.ToInt32(d2.GetFunction("select isnull(max(renewaltimes),0) renewaltimes from borrow where acc_no='" + pstrAccNo + "' and return_type='" + StrBookType + "' AND ROLL_NO='" + StrSaveRollNo + "' "));
                var = var + 1;
                if (var <= b)
                {
                    pintRenCount = b;
                    pintRenDays = var;
                }
                else
                {
                    pintRenCount = b;
                    pintRenDays = 0;
                }
            }
            else
            {
                pintRenCount = b;
                pintRenDays = 0;
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collcode, "bookissue.aspx");
        }
    }

    protected void ISRefBook(string StrRLibCode, string StrRAccNo)
    {
        string rsRefBook = d2.GetFunction("select ref from bookdetails where lib_code ='" + StrRLibCode + "' and acc_no ='" + StrRAccNo + "'");
        if (rsRefBook != "")
        {
            if (rsRefBook == "Yes")
            {
                ISReffBook = true;
                BlnRef = true;
            }
            else
            {
                ISReffBook = false;
                BlnRef = false;
            }
        }
    }

    protected void GetComm(string StrRoll)
    {
        string sql = "SELECT TextVal FROM Applyn A,Registration R,TextValTable T WHERE A.App_No = R.App_No AND A.Community = T.TextCode AND Roll_No ='" + StrRoll + "' AND (TextVal Like 'SC%' or TextVal Like 'ST%') ";
        dsprint.Clear();
        dsprint = d2.select_method_wo_parameter(sql, "text");
        if (dsprint.Tables[0].Rows.Count > 0)
            blncomm = true;
        else
            blncomm = false;
    }

    public void reserv_details(string StrSaveRollNo)
    {
        string Sql = "";
        string Libcode = Convert.ToString(ddllibrary.SelectedValue);
        int sno = 0;
        string AccDt = "";
        if (StrSaveRollNo != "")
        {
            Sql = "select access_number,title,Convert(varchar(10),access_date,103) as access_date,access_time from priority_studstaff where (roll_no='" + StrSaveRollNo + "' or staff_code='" + txtRollNo.Text + "') and cancel_flag=0 and lib_code='" + Libcode + "'";
        }
        else
        {
            Sql = "select access_number,title,Convert(varchar(10),access_date,103) as access_date,access_time from priority_studstaff where (roll_no='" + txtRollNo.Text + "' or staff_code='" + txtRollNo.Text + "') and cancel_flag=0 and lib_code='" + Libcode + "'";
        }
        rsreserve.Clear();
        rsreserve = d2.select_method_wo_parameter(Sql, "Text");
        if (rsreserve.Tables[0].Rows.Count > 0)
        {
            grdReservation.DataSource = dsload;
            grdReservation.DataBind();
            grdReservation.Visible = true;
        }
        else
        {
            //Empty DataTable to execute the “else-condition” 
            rsreserve.Tables[0].Rows.Add(rsreserve.Tables[0].NewRow());
            grdReservation.DataSource = rsreserve;
            grdReservation.DataBind();
            int columncount = grdReservation.Rows[0].Cells.Count;
            grdReservation.Rows[0].Cells.Clear();
        }
    }

    //after adding card type

    protected void LoadBooksHand(bool BlnBBank, bool blncomm, bool BlnBookBankAll, string StrSaveRollNo, string StrSaveLibID)
    {
        try
        {

            string Sql = string.Empty;
            string Libcode = Convert.ToString(Cbo_CardLibrary.SelectedValue);
            string lib = Convert.ToString(ddllibrary.SelectedValue);
            string StrBookType = Convert.ToString(ddlBookType.SelectedValue);
            string cardCriteria = Convert.ToString(ddlCardType.Text);
            if (RblMemType.SelectedIndex == 0)
            {
                Sql = "SELECT distinct r.token_no,r.acc_no,r.title,r.author,CONVERT(varchar(10), borrow_date,103) as borrow_date,CONVERT(varchar(10), due_date,103) as due_date,r.lib_code,dept_code,0 as Fine,'BOK' as Book_Type from borrow r,bookdetails b,tokendetails t,registration g where r.acc_no = b.acc_no and r.lib_code = b.lib_code and (r.roll_no = g.roll_no ) and (g.roll_no = t.roll_no ) and r.token_no = t.token_no and (r.roll_no='" + StrSaveRollNo + "' ) AND return_flag=0 AND Return_Type = 'BOK' AND r.Is_Staff = 0 ";

                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and ISNULL(t.category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";
                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";

                Sql += " UNION ALL SELECT distinct r.token_no,r.acc_no,r.title,r.author,CONVERT(varchar(10), borrow_date,103) as borrow_date,CONVERT(varchar(10), due_date,103) as due_date,r.lib_code,ISNULL(department,'') as dept_code,0 as Fine,'NBM' as Book_Type from borrow r,nonbookmat b,tokendetails t where r.acc_no = b.nonbookmat_no and r.lib_code = b.lib_code and r.roll_no = t.roll_no and r.token_no = t.token_no and (r.roll_no='" + StrSaveRollNo + "' ) and return_flag=0 AND Return_Type = 'NBM' AND r.Is_Staff = 0 ";

                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and ISNULL(t.category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";
                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";

                Sql += " UNION ALL SELECT distinct r.token_no,r.acc_no,r.title,r.author,CONVERT(varchar(10), borrow_date,103) as borrow_date,CONVERT(varchar(10), due_date,103) as due_date,r.lib_code,ISNULL(Dept,'') as dept_code,0 as Fine,'QBA' as Book_Type from borrow r,University_Question Q,tokendetails t where r.acc_no = Q.Access_Code and r.lib_code = Q.lib_code and r.roll_no = t.roll_no and r.token_no = t.token_no and (r.roll_no='" + StrSaveRollNo + "' )  AND return_flag=0 AND Return_Type = 'QBA' AND r.Is_Staff = 0 ";// if (ddluserentry.SelectedItem.Text == "Library ID")


                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and ISNULL(t.category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";

                Sql += " UNION ALL SELECT distinct r.token_no,r.acc_no,r.title,r.author,CONVERT(varchar(10), borrow_date,103) as borrow_date,CONVERT(varchar(10), due_date,103) as due_date,r.lib_code,'' as dept_code,0 as Fine,'PRO' as Book_Type from borrow r,Project_Book P,tokendetails t where r.acc_no = P.ProBook_AccNo and r.lib_code = P.lib_code and r.roll_no = t.roll_no and r.token_no = t.token_no and (r.roll_no='" + StrSaveRollNo + "' )  and return_flag=0 AND Return_Type = 'PRO' AND r.Is_Staff = 0 ";
                //or r.roll_no ='" + StrSaveLibID + "'
                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and ISNULL(t.category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";

                Sql += " UNION ALL SELECT distinct r.token_no,r.acc_no,r.title,r.author,CONVERT(varchar(10), borrow_date,103) as borrow_date,CONVERT(varchar(10), due_date,103) as due_date,r.lib_code,ISNULL(J.Dept_Name,'') as dept_code,0 as Fine,'PER' as Book_Type from borrow r,Journal J,tokendetails t where r.acc_no = J.Access_Code and r.lib_code = J.lib_code and r.roll_no = t.roll_no and r.token_no = t.token_no and (r.roll_no='" + StrSaveRollNo + "' )  and return_flag=0 AND Return_Type = 'PER' AND r.Is_Staff = 0 ";

                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and ISNULL(t.category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";

                Sql += " order by r.token_no";
                rsBookInHand.Clear();
                rsBookInHand = d2.select_method_wo_parameter(Sql, "Text");
                filler(rsBookInHand, StrSaveRollNo);
            }
            else if (RblMemType.SelectedIndex == 1)
            {

                Sql += "SELECT distinct r.token_no,r.acc_no,r.title,r.author,CONVERT(varchar(10), borrow_date,103) as borrow_date,CONVERT(varchar(10), due_date,103) as due_date,r.lib_code,dept_code,0 as Fine,'BOK' as Book_Type from borrow r,bookdetails b,tokendetails t where r.acc_no = b.acc_no and r.lib_code = b.lib_code and (r.roll_no='" + StrSaveRollNo + "')  AND return_flag=0 AND Return_Type = 'BOK' AND r.Is_Staff = 1 and t.roll_no = r.roll_no and t.token_no = r.token_no ";

                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and ISNULL(t.category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";
                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";

                Sql += " UNION ALL SELECT distinct r.token_no,r.acc_no,r.title,r.author,CONVERT(varchar(10), borrow_date,103) as borrow_date,CONVERT(varchar(10), due_date,103) as due_date,r.lib_code,ISNULL(department,'') as dept_code,0 as Fine,'NBM' as Book_Type from borrow r,nonbookmat b,tokendetails t where r.acc_no = b.nonbookmat_no and r.lib_code = b.lib_code and (r.roll_no='" + StrSaveRollNo + "' )  and return_flag=0 AND Return_Type = 'NBM' AND r.Is_Staff = 1 and t.roll_no = r.roll_no and t.token_no = r.token_no ";

                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and ISNULL(t.category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";

                Sql += " UNION ALL SELECT distinct r.token_no,r.acc_no,r.title,r.author,CONVERT(varchar(10), borrow_date,103) as borrow_date,CONVERT(varchar(10), due_date,103) as due_date,r.lib_code,ISNULL(Dept,'') as dept_code,0 as Fine,'QBA' as Book_Type from borrow r,University_Question Q,tokendetails t where r.acc_no = Q.Access_Code and r.lib_code = Q.lib_code and (r.roll_no='" + StrSaveRollNo + "')  AND return_flag=0 AND Return_Type = 'QBA' AND r.Is_Staff = 1 and t.roll_no = r.roll_no and t.token_no = r.token_no ";

                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and ISNULL(t.category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";
                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";

                Sql += " UNION ALL SELECT distinct r.token_no,r.acc_no,r.title,r.author,CONVERT(varchar(10), borrow_date,103) as borrow_date,CONVERT(varchar(10), due_date,103) as due_date,r.lib_code,'' as dept_code,0 as Fine,'PRO' as Book_Type from borrow r,Project_Book P,tokendetails t where r.acc_no = P.ProBook_AccNo and r.lib_code = P.lib_code and (r.roll_no='" + StrSaveRollNo + "' )  and return_flag=0 AND Return_Type = 'PRO' AND r.Is_Staff = 1 and t.roll_no = r.roll_no and t.token_no = r.token_no ";

                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and ISNULL(t.category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";

                Sql += " UNION ALL SELECT distinct r.token_no,r.acc_no,r.title,r.author,CONVERT(varchar(10), borrow_date,103) as borrow_date,CONVERT(varchar(10), due_date,103) as due_date,r.lib_code,ISNULL(j.Dept_Name,'') as dept_code,0 as Fine,'PER' as Book_Type from borrow r,Journal J,tokendetails t where r.acc_no = J.Access_Code and r.lib_code = J.lib_code and (r.roll_no='" + StrSaveRollNo + "')  and return_flag=0 AND Return_Type = 'PER' AND r.Is_Staff = 1 and t.roll_no = r.roll_no and t.token_no = r.token_no ";

                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and ISNULL(t.category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";
                Sql += "and r.lib_code='" + lib + "' order by r.token_no";

                rsBookInHand.Clear();
                rsBookInHand = d2.select_method_wo_parameter(Sql, "Text");
                filler(rsBookInHand, StrSaveRollNo);
            }
            else
            {
                Sql += "SELECT distinct r.token_no,r.acc_no,r.title,r.author,CONVERT(varchar(10), borrow_date,103) as borrow_date,CONVERT(varchar(10), due_date,103) as due_date,r.lib_code,dept_code,0 as Fine,'BOK' as Book_Type from borrow r,bookdetails b,tokendetails t where r.acc_no = b.acc_no and r.lib_code = b.lib_code and (r.roll_no='" + StrSaveRollNo + "' )  AND return_flag=0 AND Return_Type = 'BOK' AND (r.Is_Staff = 1 or r.is_staff = 0) and t.roll_no = r.roll_no and t.token_no = r.token_no ";

                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and ISNULL(t.category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";

                Sql += " UNION ALL SELECT distinct r.token_no,r.acc_no,r.title,r.author,CONVERT(varchar(10), borrow_date,103) as borrow_date,CONVERT(varchar(10), due_date,103) as due_date,r.lib_code,ISNULL(department,'') as dept_code,0 as Fine,'NBM' as Book_Type from borrow r,nonbookmat b,tokendetails t where r.acc_no = b.nonbookmat_no and r.lib_code = b.lib_code and (r.roll_no='" + StrSaveRollNo + "' )  and return_flag=0 AND Return_Type = 'NBM' AND (r.Is_Staff = 1 or r.is_staff = 0) and t.roll_no = r.roll_no and t.token_no = r.token_no ";

                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and ISNULL(t.category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";

                Sql += " UNION ALL SELECT distinct r.token_no,r.acc_no,r.title,r.author,CONVERT(varchar(10), borrow_date,103) as borrow_date,CONVERT(varchar(10), due_date,103) as due_date,r.lib_code,ISNULL(Dept,'') as dept_code,0 as Fine,'QBA' as Book_Type from borrow r,University_Question Q,tokendetails t where r.acc_no = Q.Access_Code and r.lib_code = Q.lib_code and (r.roll_no='" + StrSaveRollNo + "' )  AND return_flag=0 AND Return_Type = 'QBA' AND (r.Is_Staff = 1 or r.is_staff = 0) and t.roll_no = r.roll_no and t.token_no = r.token_no ";

                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and ISNULL(t.category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";

                Sql += " UNION ALL SELECT distinct r.token_no,r.acc_no,r.title,r.author,CONVERT(varchar(10), borrow_date,103) as borrow_date,CONVERT(varchar(10), due_date,103) as due_date,r.lib_code,'' as dept_code,0 as Fine,'PRO' as Book_Type from borrow r,Project_Book P,tokendetails t where r.acc_no = P.ProBook_AccNo and r.lib_code = P.lib_code and (r.roll_no='" + StrSaveRollNo + "' )  and return_flag=0 AND Return_Type = 'PRO' AND (r.Is_Staff = 1 or r.is_staff = 0) and t.roll_no = r.roll_no and t.token_no = r.token_no ";

                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and ISNULL(t.category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";

                Sql += " UNION ALL SELECT distinct r.token_no,r.acc_no,r.title,r.author,CONVERT(varchar(10), borrow_date,103) as borrow_date,CONVERT(varchar(10), due_date,103) as due_date,r.lib_code,ISNULL(j.Dept_Name,'') as dept_code,0 as Fine,'PER' as Book_Type from borrow r,Journal J,tokendetails t where r.acc_no = J.Access_Code and r.lib_code = J.lib_code and (r.roll_no='" + StrSaveRollNo + "' )  and return_flag=0 AND Return_Type = 'PER' AND (r.Is_Staff = 1 or r.is_staff = 0) and t.roll_no = r.roll_no and t.token_no = r.token_no ";

                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and ISNULL(t.category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and ISNULL(t.category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";
                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";

                Sql += "and r.lib_code='" + lib + "' order by r.token_no";
                rsBookInHand.Clear();
                rsBookInHand = d2.select_method_wo_parameter(Sql, "Text");
                filler(rsBookInHand, StrSaveRollNo);
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collcode, "bookissue.aspx");
        }

    }

    protected void filler(DataSet rsBookInHand, string StrSaveRollNo)
    {
        try
        {

            DataRow drow;
            DataSet fillerStu = new DataSet();
            DataSet fillerStaff = new DataSet();
            string serverDt = d2.ServerDate();
            DateTime CurDate = Convert.ToDateTime(serverDt.Split(' ')[0]);
            int i = 0;
            int sno = 0;
            double fine = 0;
            int maxR = 0;
            string IssueDate = "";
            string DueDate = "";
            string Sql = "";
            int intdegcode = 0;
            int staffDegCode = 0;
            string code = "";
            string DaysTokenFine = "";
            string[] myarr = new string[3];
            string Libcode = Convert.ToString(Cbo_CardLibrary.SelectedValue);
            string lib = Convert.ToString(ddllibrary.SelectedValue);
            string StrBookType = Convert.ToString(ddlBookType.SelectedValue);
            string cardCriteria = Convert.ToString(ddlCardType.Text);
            int row = 0;
            //if (rsBookInHand.Tables[0].Rows.Count < maxR)
            //{
            //    //SpreadBookInHand.Sheets[0].RowCount = maxR;
            //}
            //else
            //{
            //    if (rsBookInHand.Tables[0].Rows.Count > 0)
            //    {
            //        //SpreadBookInHand.Sheets[0].RowCount = 0;
            //        //SpreadBookInHand.Sheets[0].RowCount++;
            //    }
            //    else
            //    {
            //        //SpreadBookInHand.Sheets[0].RowCount = rsBookInHand.Tables[0].Rows.Count;
            //        return;
            //    }
            //}
            //SpreadBookInHand.Sheets[0].ColumnCount = 1;

            if (RblMemType.SelectedIndex == 0)
            {
                dtBooksInHand.Columns.Add("SNo", typeof(string));
                dtBooksInHand.Columns.Add("Access No", typeof(string));
                dtBooksInHand.Columns.Add("Title", typeof(string));
                dtBooksInHand.Columns.Add("Author", typeof(string));
                dtBooksInHand.Columns.Add("Issue Date", typeof(string));
                dtBooksInHand.Columns.Add("Due Date", typeof(string));
                dtBooksInHand.Columns.Add("Department", typeof(string));
                dtBooksInHand.Columns.Add("Token No", typeof(string));
                dtBooksInHand.Columns.Add("Fine", typeof(string));
                dtBooksInHand.Columns.Add("Library", typeof(string));
                dtBooksInHand.Columns.Add("Book Type", typeof(string));
                if (rsBookInHand.Tables[0].Rows.Count > 0)
                {
                    intdegcode = Convert.ToInt32(d2.GetFunction("select Degree_Code from Registration WHERE Roll_No ='" + StrSaveRollNo + "' "));
                    Sql = "SELECT course_id,dept_code from degree where degree_code=" + intdegcode + "";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(Sql, "Text");

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        code = Convert.ToString(ds.Tables[0].Rows[0]["course_id"]) + "~" + Convert.ToString(ds.Tables[0].Rows[0]["dept_code"]);
                    }
                    Sql = "SELECT no_of_days,no_of_token,fine,is_staff,isnull(Ref_NoofDays,0)Ref_NoofDays from lib_master where code='" + code + "' AND Is_Staff = 0 and batch_year='" + batch_year + "' ";

                    if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                        Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                    else if (BlnBookBankLib == true && BlnBookBankAll == true)
                        Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                    else
                        Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";
                    if (Cbo_CardLibrary.SelectedItem.Text != "All")
                        Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                    else
                        Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                    if (ddlBookType.SelectedItem.Text != "All")
                        Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                    else
                        Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                    if (cardCriteria != "All")
                        Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                    else
                        Sql += "AND ISNULL(CardCat,'All') ='All' ";
                    fillerStu.Clear();
                    fillerStu = d2.select_method_wo_parameter(Sql, "Text");
                    if (fillerStu.Tables[0].Rows.Count > 0)
                    {
                        DaysTokenFine = Convert.ToString(fillerStu.Tables[0].Rows[0]["no_of_days"]) + "~" + Convert.ToString(fillerStu.Tables[0].Rows[0]["no_of_token"]) + "~" + (Convert.ToInt32(fillerStu.Tables[0].Rows[0]["fine"]) - 1);
                    }
                    if (DaysTokenFine != "")
                    {
                        myarr[0] = Convert.ToString(fillerStu.Tables[0].Rows[0]["no_of_days"]);
                        myarr[1] = Convert.ToString(fillerStu.Tables[0].Rows[0]["no_of_token"]);
                        myarr[2] = Convert.ToString(fillerStu.Tables[0].Rows[0]["fine"]);
                    }

                    for (int J = 0; J < rsBookInHand.Tables[0].Rows.Count; J++)
                    {
                        string dueDateForCkFine = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["due_date"]);
                        string libCode = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["lib_code"]);
                        string libName = d2.GetFunction("select lib_name from library where lib_code='" + libCode + "'");
                        row++;
                        drow = dtBooksInHand.NewRow();
                        drow["SNo"] = Convert.ToString(row);
                        drow["Access No"] = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["acc_no"]);
                        drow["Title"] = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["title"]);
                        drow["Author"] = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["author"]);
                        drow["Issue Date"] = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["borrow_date"]);
                        drow["Due Date"] = dueDateForCkFine;
                        drow["Department"] = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["dept_code"]);
                        drow["Token No"] = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["token_no"]);
                        string[] splitdate = dueDateForCkFine.Split('/');
                        if (splitdate.Length == 3)
                            dueDateForCkFine = splitdate[1].ToString() + "/" + splitdate[0].ToString() + "/" + splitdate[2].ToString();
                        DateTime DDate = Convert.ToDateTime(dueDateForCkFine);
                        TimeSpan diff = CurDate.Subtract(DDate);
                        int Datedifference = diff.Days;
                        if (Datedifference > 0)
                        {
                            Sql = "select * from examfinems where degree_code=" + intdegcode + " and semester in (select batch_year from registration where roll_no = '" + txtRollNo.Text + "') and exmfine=2";

                            rsCalFine.Clear();
                            rsCalFine = d2.select_method_wo_parameter(Sql, "Text");

                            if (rsCalFine.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < rsCalFine.Tables[0].Rows.Count; j++)
                                {
                                    string today = Convert.ToString(rsCalFine.Tables[0].Rows[0]["today"]);
                                    if (today != "0")
                                    {
                                        if (Datedifference > Convert.ToInt32(rsCalFine.Tables[0].Rows[0]["fromday"]) && Datedifference < Convert.ToInt32(rsCalFine.Tables[0].Rows[0]["today"]))
                                            fine = Datedifference * Convert.ToInt32(rsCalFine.Tables[0].Rows[0]["fineamount"]);
                                    }
                                    else
                                    {
                                        if (Datedifference > Convert.ToInt32(rsCalFine.Tables[0].Rows[0]["fromday"]))
                                            fine = Datedifference * Convert.ToInt32(rsCalFine.Tables[0].Rows[0]["fineamount"]);
                                    }
                                }
                            }
                            else
                            {
                                fine = Convert.ToDouble(Datedifference) * Convert.ToDouble(myarr[2]);
                            }
                        }
                        else
                        {
                            fine = 0;
                        }
                        drow["Fine"] = Convert.ToString(fine);
                        drow["Library"] = Convert.ToString(libName);
                        drow["Book Type"] = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["Book_Type"]);
                        dtBooksInHand.Rows.Add(drow);
                    }
                    GrdBookInHand.DataSource = dtBooksInHand;
                    GrdBookInHand.DataBind();
                    GrdBookInHand.Visible = true;
                    for (int l = 0; l < GrdBookInHand.Rows.Count; l++)
                    {
                        foreach (GridViewRow gvrow in GrdBookInHand.Rows)
                        {
                            foreach (TableCell cell in gvrow.Cells)
                            {
                                GrdBookInHand.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                GrdBookInHand.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                                GrdBookInHand.Rows[l].Cells[8].HorizontalAlign = HorizontalAlign.Right;
                            }
                        }
                    }
                }
                else
                {
                    GrdBookInHand.DataSource = dtBooksInHand;
                    GrdBookInHand.DataBind();
                    GrdBookInHand.Visible = true;
                    for (int l = 0; l < GrdBookInHand.Rows.Count; l++)
                    {
                        foreach (GridViewRow gvrow in GrdBookInHand.Rows)
                        {
                            foreach (TableCell cell in gvrow.Cells)
                            {
                                GrdBookInHand.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                GrdBookInHand.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                                GrdBookInHand.Rows[l].Cells[8].HorizontalAlign = HorizontalAlign.Right;
                            }
                        }
                    }
                }
            }
            if (RblMemType.SelectedIndex == 1)
            {
                dtBooksInHand.Columns.Add("SNo", typeof(string));
                dtBooksInHand.Columns.Add("Access No", typeof(string));
                dtBooksInHand.Columns.Add("Title", typeof(string));
                dtBooksInHand.Columns.Add("Author", typeof(string));
                dtBooksInHand.Columns.Add("Issue Date", typeof(string));
                dtBooksInHand.Columns.Add("Due Date", typeof(string));
                dtBooksInHand.Columns.Add("Department", typeof(string));
                dtBooksInHand.Columns.Add("Token No", typeof(string));
                dtBooksInHand.Columns.Add("Fine", typeof(string));
                dtBooksInHand.Columns.Add("Library", typeof(string));
                dtBooksInHand.Columns.Add("Book Type", typeof(string));

                if (rsBookInHand.Tables[0].Rows.Count > 0)
                {
                    Sql = "select staff_code,lib_id from staffmaster where staff_code ='" + txtRollNo.Text + "' or lib_id ='" + txtRollNo.Text + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(Sql, "Text");
                    string staffCode = "";
                    string libId = "";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        staffCode = Convert.ToString(ds.Tables[0].Rows[0]["staff_code"]);
                        libId = Convert.ToString(ds.Tables[0].Rows[0]["staff_code"]);
                        Sql = "SELECT no_of_days,no_of_token,fine,is_staff,isnull(Ref_NoofDays,0)Ref_NoofDays from lib_master where (code='" + staffCode + "' or code ='" + libId + "') AND Is_Staff = 1 ";
                        if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                            Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                        else if (BlnBookBankLib == true && BlnBookBankAll == true)
                            Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                        else
                            Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                        if (Cbo_CardLibrary.SelectedItem.Text != "All")
                            Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                        else
                            Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                        if (ddlBookType.SelectedItem.Text != "All")
                            Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                        else
                            Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                        if (cardCriteria != "All")
                            Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                        else
                            Sql += "AND ISNULL(CardCat,'All') ='All' ";
                        fillerStaff.Clear();
                        fillerStaff = d2.select_method_wo_parameter(Sql, "Text");
                        if (fillerStaff.Tables[0].Rows.Count > 0)
                        {
                            DaysTokenFine = Convert.ToString(fillerStaff.Tables[0].Rows[0]["no_of_days"]) + "~" + Convert.ToString(fillerStaff.Tables[0].Rows[0]["no_of_token"]) + "~" + Convert.ToString(fillerStaff.Tables[0].Rows[0]["fine"]);
                        }
                        if (DaysTokenFine != "")
                        {
                            myarr[0] = Convert.ToString(fillerStaff.Tables[0].Rows[0]["no_of_days"]);
                            myarr[1] = Convert.ToString(fillerStaff.Tables[0].Rows[0]["no_of_token"]);
                            myarr[2] = Convert.ToString(fillerStaff.Tables[0].Rows[0]["fine"]);
                        }
                        for (int J = 0; J < rsBookInHand.Tables[0].Rows.Count; J++)
                        {
                            string dueDateForCkFine = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["due_date"]);
                            string libCode = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["lib_code"]);
                            string libName = d2.GetFunction("select lib_name from library where lib_code='" + libCode + "'");
                            row++;
                            drow = dtBooksInHand.NewRow();
                            drow["SNo"] = Convert.ToString(row);
                            drow["Access No"] = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["acc_no"]);
                            drow["Title"] = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["title"]);
                            drow["Author"] = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["author"]);
                            drow["Issue Date"] = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["borrow_date"]);
                            drow["Due Date"] = dueDateForCkFine;
                            drow["Department"] = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["dept_code"]);
                            drow["Token No"] = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["token_no"]);
                            string[] splitdate = dueDateForCkFine.Split('/');
                            if (splitdate.Length == 3)
                                dueDateForCkFine = splitdate[1].ToString() + "/" + splitdate[0].ToString() + "/" + splitdate[2].ToString();
                            DateTime DDate = Convert.ToDateTime(dueDateForCkFine);
                            TimeSpan diff = CurDate.Subtract(DDate);
                            int Datedifference = diff.Days;
                            if (Datedifference > 0)
                            {
                                staffDegCode = Convert.ToInt32(d2.GetFunction("select dept_code from stafftrans t,staffmaster m where t.staff_code = m.staff_code and m.staff_code='" + txtRollNo.Text + "' "));
                                Sql = "select * from examfinems where degree_code='" + staffDegCode + "' and exmfine=2";

                                rsCalFine.Clear();
                                rsCalFine = d2.select_method_wo_parameter(Sql, "Text");

                                if (rsCalFine.Tables[0].Rows.Count > 0)
                                {
                                    for (int j = 0; j < rsCalFine.Tables[0].Rows.Count; j++)
                                    {
                                        string today = Convert.ToString(rsCalFine.Tables[0].Rows[0]["today"]);
                                        if (today != "0")
                                        {
                                            if (Datedifference > Convert.ToInt32(rsCalFine.Tables[0].Rows[0]["fromday"]) && Datedifference < Convert.ToInt32(rsCalFine.Tables[0].Rows[0]["today"]))
                                                fine = Datedifference * Convert.ToInt32(rsCalFine.Tables[0].Rows[0]["fineamount"]);
                                        }
                                        else
                                        {
                                            if (Datedifference > Convert.ToInt32(rsCalFine.Tables[0].Rows[0]["fromday"]))
                                                fine = Datedifference * Convert.ToInt32(rsCalFine.Tables[0].Rows[0]["fineamount"]);
                                        }
                                    }
                                }
                                else
                                {
                                    fine = Convert.ToDouble(Datedifference) * Convert.ToDouble(myarr[2]);
                                }
                            }
                            else
                            {
                                fine = 0;
                            }
                            drow["Fine"] = Convert.ToString(fine);
                            drow["Library"] = Convert.ToString(libName);
                            drow["Book Type"] = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["Book_Type"]);
                            dtBooksInHand.Rows.Add(drow);
                        }
                        GrdBookInHand.DataSource = dtBooksInHand;
                        GrdBookInHand.DataBind();
                        GrdBookInHand.Visible = true;
                        for (int l = 0; l < GrdBookInHand.Rows.Count; l++)
                        {
                            foreach (GridViewRow gvrow in GrdBookInHand.Rows)
                            {
                                foreach (TableCell cell in gvrow.Cells)
                                {
                                    GrdBookInHand.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                    GrdBookInHand.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                                    GrdBookInHand.Rows[l].Cells[8].HorizontalAlign = HorizontalAlign.Right;
                                }
                            }
                        }
                    }
                }
                else
                {
                    GrdBookInHand.DataSource = dtBooksInHand;
                    GrdBookInHand.DataBind();
                    GrdBookInHand.Visible = true;
                    for (int l = 0; l < GrdBookInHand.Rows.Count; l++)
                    {
                        foreach (GridViewRow gvrow in GrdBookInHand.Rows)
                        {
                            foreach (TableCell cell in gvrow.Cells)
                            {
                                GrdBookInHand.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                GrdBookInHand.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                                GrdBookInHand.Rows[l].Cells[8].HorizontalAlign = HorizontalAlign.Right;
                            }
                        }
                    }
                }
            }
            if (RblMemType.SelectedIndex == 2)
            {
                dtBooksInHand.Columns.Add("SNo", typeof(string));
                dtBooksInHand.Columns.Add("Access No", typeof(string));
                dtBooksInHand.Columns.Add("Title", typeof(string));
                dtBooksInHand.Columns.Add("Author", typeof(string));
                dtBooksInHand.Columns.Add("Issue Date", typeof(string));
                dtBooksInHand.Columns.Add("Due Date", typeof(string));
                dtBooksInHand.Columns.Add("Department", typeof(string));
                dtBooksInHand.Columns.Add("Token No", typeof(string));
                dtBooksInHand.Columns.Add("Fine", typeof(string));
                dtBooksInHand.Columns.Add("Library", typeof(string));
                dtBooksInHand.Columns.Add("Book Type", typeof(string));
                if (rsBookInHand.Tables[0].Rows.Count > 0)
                {
                    Sql = "select user_id from user_master where staff_code ='" + txtRollNo.Text + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(Sql, "Text");
                    string staffCode = "";

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        staffCode = Convert.ToString(ds.Tables[0].Rows[0]["user_id"]);
                        //libId = Convert.ToString(ds.Tables[0].Rows[0]["staff_code"]);
                        Sql = "SELECT no_of_days,no_of_token,fine,is_staff,isnull(Ref_NoofDays,0)Ref_NoofDays from lib_master where (code='" + staffCode + "') ";
                        if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                            Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                        else if (BlnBookBankLib == true && BlnBookBankAll == true)
                            Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                        else
                            Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                        if (Cbo_CardLibrary.SelectedItem.Text != "All")
                            Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                        else
                            Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                        if (ddlBookType.SelectedItem.Text != "All")
                            Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                        else
                            Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                        if (cardCriteria != "All")
                            Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                        else
                            Sql += "AND ISNULL(CardCat,'All') ='All' ";
                        fillerStaff.Clear();
                        fillerStaff = d2.select_method_wo_parameter(Sql, "Text");
                        if (fillerStaff.Tables[0].Rows.Count > 0)
                        {
                            DaysTokenFine = Convert.ToString(fillerStaff.Tables[0].Rows[0]["no_of_days"]) + "~" + Convert.ToString(fillerStaff.Tables[0].Rows[0]["no_of_token"]) + "~" + Convert.ToString(fillerStaff.Tables[0].Rows[0]["fine"]);
                        }
                        if (DaysTokenFine != "")
                        {
                            myarr[0] = Convert.ToString(fillerStaff.Tables[0].Rows[0]["no_of_days"]);
                            myarr[1] = Convert.ToString(fillerStaff.Tables[0].Rows[0]["no_of_token"]);
                            myarr[2] = Convert.ToString(fillerStaff.Tables[0].Rows[0]["fine"]);
                        }
                        for (int J = 0; J < rsBookInHand.Tables[0].Rows.Count; J++)
                        {
                            string dueDateForCkFine = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["due_date"]);
                            string libCode = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["lib_code"]);
                            string libName = d2.GetFunction("select lib_name from library where lib_code='" + libCode + "'");
                            row++;
                            drow = dtBooksInHand.NewRow();
                            drow["SNo"] = Convert.ToString(row);
                            drow["Access No"] = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["acc_no"]);
                            drow["Title"] = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["title"]);
                            drow["Author"] = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["author"]);
                            drow["Issue Date"] = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["borrow_date"]);
                            drow["Due Date"] = dueDateForCkFine;
                            drow["Department"] = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["dept_code"]);
                            drow["Token No"] = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["token_no"]);
                            string[] splitdate = dueDateForCkFine.Split('/');
                            if (splitdate.Length == 3)
                                dueDateForCkFine = splitdate[1].ToString() + "/" + splitdate[0].ToString() + "/" + splitdate[2].ToString();
                            DateTime DDate = Convert.ToDateTime(dueDateForCkFine);
                            TimeSpan diff = CurDate.Subtract(DDate);
                            int Datedifference = diff.Days;
                            if (Datedifference > 0)
                            {
                            }
                            else
                            {
                                fine = 0;
                            }
                            drow["Fine"] = Convert.ToString(fine);
                            drow["Library"] = Convert.ToString(libName);
                            drow["Book Type"] = Convert.ToString(rsBookInHand.Tables[0].Rows[J]["Book_Type"]);
                            dtBooksInHand.Rows.Add(drow);
                        }
                        GrdBookInHand.DataSource = dtBooksInHand;
                        GrdBookInHand.DataBind();
                        GrdBookInHand.Visible = true;
                        for (int l = 0; l < GrdBookInHand.Rows.Count; l++)
                        {
                            foreach (GridViewRow gvrow in GrdBookInHand.Rows)
                            {
                                foreach (TableCell cell in gvrow.Cells)
                                {
                                    GrdBookInHand.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                    GrdBookInHand.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                                    GrdBookInHand.Rows[l].Cells[8].HorizontalAlign = HorizontalAlign.Right;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                GrdBookInHand.DataSource = dtBooksInHand;
                GrdBookInHand.DataBind();
                GrdBookInHand.Visible = true;
                for (int l = 0; l < GrdBookInHand.Rows.Count; l++)
                {
                    foreach (GridViewRow gvrow in GrdBookInHand.Rows)
                    {
                        foreach (TableCell cell in gvrow.Cells)
                        {
                            GrdBookInHand.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            GrdBookInHand.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                            GrdBookInHand.Rows[l].Cells[8].HorizontalAlign = HorizontalAlign.Right;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collcode, "bookissue.aspx");
        }
    }

    protected void GrdBookInHand_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string serverDt = d2.ServerDate();
                string currentdate = serverDt.Split(' ')[0];
                string Duedate = Convert.ToString(e.Row.Cells[5].Text);
                string Accno = Convert.ToString(e.Row.Cells[1].Text);
                string Due_date = Convert.ToString(e.Row.Cells[5].Text);
                string BorrowDate = Convert.ToString(e.Row.Cells[4].Text);
                string[] D_date = Duedate.Split('/');
                if (D_date.Length == 3)
                    Duedate = D_date[1].ToString() + "/" + D_date[0].ToString() + "/" + D_date[2].ToString();
                string BookType = Convert.ToString(e.Row.Cells[10].Text);
                string refBookOrNo = d2.GetFunction("select ref from bookdetails where Acc_No='" + Accno + "'");
                if (!string.IsNullOrEmpty(refBookOrNo))
                {
                    if (refBookOrNo.ToLower() == "yes")
                        e.Row.ForeColor = ColorTranslator.FromHtml("#FF1493");
                    else//BOK
                        e.Row.ForeColor = ColorTranslator.FromHtml("#000000");
                }
                if (Convert.ToDateTime(Duedate) < Convert.ToDateTime(currentdate))
                {
                    e.Row.ForeColor = ColorTranslator.FromHtml("#ee3c34");//ee3c34,FF0000                   
                }
                else if (BookType == "NBM")
                {
                    e.Row.ForeColor = ColorTranslator.FromHtml("#228B22");
                }
                else if (BookType == "PER")
                {
                    e.Row.ForeColor = ColorTranslator.FromHtml("#8B4513");
                }
                else if (BookType == "QBA")
                {
                    e.Row.ForeColor = ColorTranslator.FromHtml("#00BFFF");
                }
                else if (BookType == "PRO")
                {
                    e.Row.ForeColor = ColorTranslator.FromHtml("#9370DB");
                }
                if (rblissue.SelectedIndex == 0)
                {
                }
                else
                {
                    //txtissuedate.Text = BorrowDate;
                    //Txtduedate.Text = Due_date;
                }
            }
        }
        catch (Exception ex)
        {
        }

    }

    protected void GetStfDeptCode(string StaffCode)
    {
        //string sql = "select stftype from stafftrans t,staffmaster m where t.staff_code = m.staff_code and (m.staff_code='" + StaffCode + "' or m.lib_id ='" + StaffCode + "') and latestrec=1";
        //rsCode.Clear();
        //rsCode = d2.select_method_wo_parameter(sql, "Text");
        //if (rsCode.Tables[0].Rows.Count > 0)
        //{
        //    string StaffType = Convert.ToString(rsCode.Tables[0].Rows[0]["stftype"]);
        //    if (StaffType == "Teaching")
        //        GetStfDeptCode = 1000;
        //    else if (StaffType == "Non-Teaching" || StaffType == "Non Teaching")
        //        GetStfDeptCode = 1001;
        //    else if (StaffType == "Office Staff")
        //        GetStfDeptCode = 1002;
        //    else if (StaffType == "Others")
        //        GetStfDeptCode = 1003;
        //    else
        //        GetStfDeptCode = 1004;
        //}
        //else
        //{
        //    GetStfDeptCode = 1004;
        //}
    }

    protected void DispCardStatus(bool BlnBookBankLib, bool blncomm, bool BlnBookBankAll, string StrSaveRollNo, string StrSaveLibID)
    {
        try
        {
            string Sql = "";
            string Libcode = Convert.ToString(Cbo_CardLibrary.SelectedValue);
            string lib = Convert.ToString(ddllibrary.SelectedValue);
            string StrBookType = Convert.ToString(ddlBookType.SelectedValue);
            string cardCriteria = Convert.ToString(ddlCardType.Text);
            //Cards Count
            if (RblMemType.SelectedIndex == 0)
            {
                //Eligible Cards Count
                Sql = "SELECT COUNT(Token_No) as token FROM TokenDetails WHERE Is_Staff = 0 and Roll_No ='" + StrSaveRollNo + "'";

                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and category ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";
                dsDispCard.Clear();
                dsDispCard = d2.select_method_wo_parameter(Sql, "Text");
                if (dsDispCard.Tables[0].Rows.Count > 0)
                {
                    txt_elgi.Text = Convert.ToString(dsDispCard.Tables[0].Rows[0]["token"]);
                    txt_elgi.Enabled = false;
                }

                //Issued Cards Count
                Sql = "SELECT COUNT(Token_No) as token FROM TokenDetails WHERE  Is_Staff = 0 AND Is_Locked = 1 and Roll_No ='" + StrSaveRollNo + "'";
                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and category ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";
                dsDispCard.Clear();
                dsDispCard = d2.select_method_wo_parameter(Sql, "Text");
                if (dsDispCard.Tables[0].Rows.Count > 0)
                {
                    txt_issued.Text = Convert.ToString(dsDispCard.Tables[0].Rows[0]["token"]);
                    txt_issued.Enabled = false;
                }

                //Balance Cards Count
                Sql = "SELECT COUNT(Token_No) as token FROM TokenDetails WHERE Is_Staff = 0 AND Is_Locked = 0 and Roll_No ='" + StrSaveRollNo + "'";
                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and category ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";
                dsDispCard.Clear();
                dsDispCard = d2.select_method_wo_parameter(Sql, "Text");
                if (dsDispCard.Tables[0].Rows.Count > 0)
                {
                    txt_Unlocked.Text = Convert.ToString(dsDispCard.Tables[0].Rows[0]["token"]);
                    txt_Unlocked.Enabled = false;
                }

                //Locked Cards Count
                Sql = "SELECT COUNT(Token_No) as token FROM TokenDetails WHERE Is_Staff = 0 AND Is_Locked = 2 and Roll_No ='" + StrSaveRollNo + "'";

                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and category ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";
                dsDispCard.Clear();
                dsDispCard = d2.select_method_wo_parameter(Sql, "Text");
                if (dsDispCard.Tables[0].Rows.Count > 0)
                {
                    txtlocked.Text = Convert.ToString(dsDispCard.Tables[0].Rows[0]["token"]);
                    txtlocked.Enabled = false;
                }
            }
            else if (RblMemType.SelectedIndex == 1)
            {
                //Eligible Cards Count
                Sql = "SELECT COUNT(Token_No) as token FROM TokenDetails WHERE Is_Staff = 1 and Roll_No ='" + StrSaveRollNo + "'";
                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and category ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";
                dsDispCard.Clear();
                dsDispCard = d2.select_method_wo_parameter(Sql, "Text");
                if (dsDispCard.Tables[0].Rows.Count > 0)
                {
                    txt_elgi.Text = Convert.ToString(dsDispCard.Tables[0].Rows[0]["token"]);
                    txt_elgi.Enabled = false;
                }

                //Issued Cards Count
                Sql = "SELECT COUNT(Token_No) as token FROM TokenDetails WHERE Is_Staff = 1 AND Is_Locked = 1 and Roll_No ='" + StrSaveRollNo + "'";
                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and category ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";
                dsDispCard.Clear();
                dsDispCard = d2.select_method_wo_parameter(Sql, "Text");
                if (dsDispCard.Tables[0].Rows.Count > 0)
                {
                    txt_issued.Text = Convert.ToString(dsDispCard.Tables[0].Rows[0]["token"]);
                    txt_issued.Enabled = false;
                }

                //Balance Cards Count
                Sql = "SELECT COUNT(Token_No) as token FROM TokenDetails WHERE Is_Staff = 1 AND Is_Locked = 0 and Roll_No ='" + StrSaveRollNo + "'";

                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and category ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";
                dsDispCard.Clear();
                dsDispCard = d2.select_method_wo_parameter(Sql, "Text");
                if (dsDispCard.Tables[0].Rows.Count > 0)
                {
                    txt_Unlocked.Text = Convert.ToString(dsDispCard.Tables[0].Rows[0]["token"]);
                    txt_Unlocked.Enabled = false;
                }

                //Locked Cards Count
                Sql = "SELECT COUNT(Token_No) as token FROM TokenDetails WHERE  Is_Staff = 1 AND Is_Locked = 2 and Roll_No ='" + StrSaveRollNo + "'";
                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else if (BlnBookBankLib == true && blncomm == false && BlnBookBankAll == false)
                    Sql += "and category ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else
                    Sql += "and category ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";
                dsDispCard.Clear();
                dsDispCard = d2.select_method_wo_parameter(Sql, "Text");
                if (dsDispCard.Tables[0].Rows.Count > 0)
                {
                    txtlocked.Text = Convert.ToString(dsDispCard.Tables[0].Rows[0]["token"]);
                    txtlocked.Enabled = false;
                }
            }
            else if (RblMemType.SelectedIndex == 2)
            {

                Sql = "SELECT COUNT(Token_No) as token FROM TokenDetails WHERE Roll_No ='" + txtRollNo.Text + "' ";
                dsDispCard.Clear();
                dsDispCard = d2.select_method_wo_parameter(Sql, "Text");
                if (dsDispCard.Tables[0].Rows.Count > 0)
                {
                    txt_elgi.Text = Convert.ToString(dsDispCard.Tables[0].Rows[0]["token"]);
                    txt_elgi.Enabled = false;
                }
                Sql = "SELECT COUNT(Token_No) as token FROM TokenDetails WHERE Roll_No ='" + txtRollNo.Text + "' AND Is_Locked = 1 ";
                dsDispCard.Clear();
                dsDispCard = d2.select_method_wo_parameter(Sql, "Text");
                if (dsDispCard.Tables[0].Rows.Count > 0)
                {
                    txt_issued.Text = Convert.ToString(dsDispCard.Tables[0].Rows[0]["token"]);
                    txt_issued.Enabled = false;
                }

                Sql = "SELECT COUNT(Token_No) as token FROM TokenDetails WHERE Roll_No ='" + txtRollNo.Text + "' AND Is_Locked = 0 ";
                dsDispCard.Clear();
                dsDispCard = d2.select_method_wo_parameter(Sql, "Text");
                if (dsDispCard.Tables[0].Rows.Count > 0)
                {
                    txt_Unlocked.Text = Convert.ToString(dsDispCard.Tables[0].Rows[0]["token"]);
                    txt_Unlocked.Enabled = false;
                }

                Sql = "SELECT COUNT(Token_No) as token From TokenDetails WHERE Roll_No ='" + txtRollNo.Text + "'  AND Is_Locked = 2 ";
                dsDispCard.Clear();
                dsDispCard = d2.select_method_wo_parameter(Sql, "Text");
                if (dsDispCard.Tables[0].Rows.Count > 0)
                {
                    txtlocked.Text = Convert.ToString(dsDispCard.Tables[0].Rows[0]["token"]);
                    txtlocked.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collcode, "bookissue.aspx");
        }
    }

    #endregion

    //*****************************Visible & Invisible Fine Details*************************

    protected void VisibleFineDet()
    {
        if (rblissue.SelectedIndex == 3)
        {
            rbfine.Visible = true;
            tdfine.Visible = true;
            lblBookPrice.Visible = true;
            txt_lostprice.Visible = true;
            LostAndFineDiv.Visible = true;

            LostAndFineDiv.Visible = true;
            TdAmt.Visible = true;
            Tdfinecnl.Visible = true;
            lblDueDays.Visible = true;
            txt_days.Visible = true;
            lblDueAmount.Visible = true;
            txt_amount.Visible = true;
            lbl_TotalDue.Visible = true;
            txt_TotalDue.Visible = true;
        }
        else
        {
            LostAndFineDiv.Visible = true;
            LostAndFineDiv.Visible = true;
            TdAmt.Visible = true;
            Tdfinecnl.Visible = true;
            lblDueDays.Visible = true;
            txt_days.Visible = true;
            lblDueAmount.Visible = true;
            txt_amount.Visible = true;
            lbl_TotalDue.Visible = true;
            txt_TotalDue.Visible = true;
            rbfine.Visible = false;
            tdfine.Visible = false;
            lblBookPrice.Visible = false;
            txt_lostprice.Visible = false;
        }


    }

    #region Count

    protected void TotIssuedCount()
    {
        string lib = Convert.ToString(ddllibrary.SelectedValue);
        string serverDt = d2.ServerDate();
        string Date = serverDt.Split(' ')[0];
        string Sql = "select ISNULL(count(*),0) as count from borrow where borrow_date='" + Date + "' and lib_code='" + lib + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(Sql, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtissuedCount.Text = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
        }

    }

    protected void TotReturnedCount()
    {
        string lib = Convert.ToString(ddllibrary.SelectedValue);
        string serverDt = d2.ServerDate();
        string Date = serverDt.Split(' ')[0];
        string Sql = "select ISNULL(count(*),0) as count from borrow where return_date='" + Date + "' and lib_code='" + lib + "' and return_flag=1";
        ds = d2.select_method_wo_parameter(Sql, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtReturnedCount.Text = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
        }
    }

    protected void TotDueBooksCount()
    {
        int duedays = 0;
        int noofdays = 0;
        string lib = Convert.ToString(ddllibrary.SelectedValue);
        string serverDt = d2.ServerDate();
        string Date = serverDt.Split(' ')[0];
        string Sql = "select ISNULL(count(*),0) as count from borrow where due_date ='" + Date + "' and lib_code='" + lib + "' and return_flag=0";
        ds = d2.select_method_wo_parameter(Sql, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            Txt_DueBookCount.Text = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
        }
    }

    protected void FineCollection()
    {
        double DblFinAmt = 0;
        string lib = Convert.ToString(ddllibrary.SelectedValue);
        string serverDt = d2.ServerDate();
        string Date = serverDt.Split(' ')[0];
        string Sql = "select isnull(sum(paidamt),0) Paidamt from fine_details where cal_date ='" + Date + "' and lib_code='" + lib + "' Group By Cal_Date";
        ds = d2.select_method_wo_parameter(Sql, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtFineAmount.Text = Convert.ToString(ds.Tables[0].Rows[0]["paidamt"]);
            //Txt_CirFine.Text = Format(Txt_CirFine.Text, "#0.00")
        }
        else
        {
            TxtFineAmount.Text = "0";
        }
    }

    protected void DispStatusList()
    {
        try
        {
            TotIssuedCount();
            TotReturnedCount();
            TotDueBooksCount();
            FineCollection();
            string Library = Convert.ToString(ddllibrary.SelectedValue);

            int avacnt = 0;
            int isscnt = 0;
            int miscnt = 0;
            int loscnt = 0;
            int TotBookCnt = 0;

            avacnt = Convert.ToInt32(d2.GetFunction("select count(*) from bookdetails where (upper(book_status) ='AVAILABLE' and ISNULL(Transfered,0) = 0) and lib_code='" + Library + "'"));
            isscnt = Convert.ToInt32(d2.GetFunction("select count(*) from borrow where return_flag=0 and lib_code='" + Library + "'"));
            miscnt = Convert.ToInt32(d2.GetFunction("select count(*) from bookdetails where upper(book_status) ='MISSING' and lib_code='" + Library + "'"));
            loscnt = Convert.ToInt32(d2.GetFunction("select count(*) from bookdetails where upper(book_status) ='LOST' and lib_code='" + Library + "'"));
            TotBookCnt = Convert.ToInt32(d2.GetFunction("select count(*) from bookdetails where lib_code='" + Library + "'"));
            Txtavailable.Text = Convert.ToString(avacnt);
            TxtIssue.Text = Convert.ToString(isscnt);
            TxtMissing.Text = Convert.ToString(miscnt);
            Txtlost.Text = Convert.ToString(loscnt);
            TxtTotBooks.Text = Convert.ToString(TotBookCnt);
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    protected void cmdRenewal_Click(object sender, EventArgs e)
    {
        try
        {
            RollNoChallanReceipt = txtRollNo.Text;
            string qry = "";
            bool blnSave = false;
            string book_type = string.Empty;
            string issueType = Convert.ToString(ddlissue.SelectedValue);
            string ColCode = Convert.ToString(ddlcollege.SelectedValue);
            string lib = Convert.ToString(ddllibrary.SelectedValue);
            string Libcode = Convert.ToString(Cbo_CardLibrary.SelectedValue);
            string servertime = d2.ServerTime();
            string time = servertime;
            string StrBookType = Convert.ToString(ddlBookType.SelectedValue);
            string cardCriteria = Convert.ToString(ddlCardType.SelectedValue);
            string finYeaid = d2.getCurrentFinanceYear(userCode, ColCode);
            string Sql = "";
            string count = "";
            string LinkValueA = "";
            string LinkValueB = "";
            string ReturnedBy = d2.GetFunction("select Full_Name from usermaster where user_code='" + userCode + "'");
            string returnBy = ReturnedBy + "/" + StrMemberType;
            if (issueType == "Book")
                book_type = "BOK";
            if (issueType == "Periodicals")
                book_type = "PER";
            if (issueType == "Project Book")
                book_type = "PRO";
            if (issueType == "Non-Book Material")
                book_type = "NBM";
            if (issueType == "Question Bank")
                book_type = "QBA";
            if (issueType == "Back Volume")
                book_type = "BVO";
            if (issueType == "Reference Books")
                book_type = "REF";

            string issuedDate = Convert.ToString(txtissuedate.Text);
            string[] dtIssued = issuedDate.Split('/');
            if (dtIssued.Length == 3)
                issuedDate = dtIssued[1].ToString() + "/" + dtIssued[0].ToString() + "/" + dtIssued[2].ToString();
            string Duedate = Convert.ToString(Txtduedate.Text);
            string[] dtDueDate = Duedate.Split('/');
            if (dtDueDate.Length == 3)
                Duedate = dtDueDate[1].ToString() + "/" + dtDueDate[0].ToString() + "/" + dtDueDate[2].ToString();

            string serverDt = d2.ServerDate();
            string CurrentDate = serverDt;
            int issuedCnt = Convert.ToInt32(TxtissuedCount.Text) + 1;
            int circnumgeniss = 0;
            string borrowdt = "";
            string dueDt = "";
            int totCount = 0;

            for (int i = 0; i < GrdIssuingBook.Rows.Count; i++)
            {
                varAccno = Convert.ToString(GrdIssuingBook.Rows[i].Cells[2].Text);
                vartitle = Convert.ToString(GrdIssuingBook.Rows[i].Cells[3].Text);
                varToken = Convert.ToString(GrdIssuingBook.Rows[i].Cells[9].Text);
                borrowdate = Convert.ToString(GrdIssuingBook.Rows[i].Cells[6].Text);
                string[] dtIssue = borrowdate.Split('/');
                if (dtIssue.Length == 3)
                    borrowdt = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                //varIssueDate = Format(varIssueDate, "dd/mm/yyyy");
                varauthor = Convert.ToString(GrdIssuingBook.Rows[i].Cells[4].Text);
                varCallNo = Convert.ToString(GrdIssuingBook.Rows[i].Cells[5].Text);
                varDueDate = Convert.ToString(GrdIssuingBook.Rows[i].Cells[8].Text);
                string[] dtDue = varDueDate.Split('/');
                if (dtDue.Length == 3)
                    dueDt = dtDue[1].ToString() + "/" + dtDue[0].ToString() + "/" + dtDue[2].ToString();

                //varDueDate = Format(varDueDate, "dd/mm/yyyy");
                if (!string.IsNullOrEmpty(GrdIssuingBook.Rows[i].Cells[10].Text))
                {
                    fine1 = Convert.ToInt32(GrdIssuingBook.Rows[i].Cells[10].Text);
                }
                Sql = "select max(cirno_issue) as count from borrow where return_date='" + issuedDate + "' and lib_code='" + lib + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(Sql, "text");

                if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["count"])))
                    circnumgeniss = Convert.ToInt32(ds.Tables[0].Rows[0]["count"]) + 1;
                else
                    circnumgeniss = 1;

                qry = "select * from borrow where acc_no='" + varAccno + "' and return_type='" + book_type + "' AND ROLL_NO='" + txtRollNo.Text + "' and return_flag=0  ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(qry, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    qry = "select * from inssettings where LinkName='Renewal Permission' and College_Code=" + ColCode + "";
                    dsload.Clear();
                    dsload = d2.select_method_wo_parameter(qry, "text");
                    string[] Linkarr = Convert.ToString(dsload.Tables[0].Rows[0]["LinkValue"]).Split('/');

                    if (Linkarr.Length > 0)
                        LinkValueA = Linkarr[0];

                    Sql = "SELECT ISNULL(Renew_Days,0) as count FROM TokenDetails WHERE Roll_No='" + txtRollNo.Text + "' ";
                    if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                        Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                    else if (BlnBookBankLib == true && BlnBookBankAll == true)
                        Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                    else
                        Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                    if (Cbo_CardLibrary.SelectedItem.Text != "All")
                        Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                    else if (Cbo_CardLibrary.SelectedItem.Text == "All")
                        Sql += "AND ISNULL(TransLibCode,'All') ='All'";

                    if (ddlBookType.SelectedItem.Text != "All")
                        Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                    else
                        Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                    if (cardCriteria != "All")
                        Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                    else
                        Sql += "AND ISNULL(CardCat,'All') ='All' ";
                    count = d2.GetFunction(Sql);
                    if (!string.IsNullOrEmpty(count))
                        totCount = Convert.ToInt32(count);
                    if (totCount == 0)
                    {
                        Sql = "select * from inssettings where LinkName='Renewal Permission' and College_Code=" + ColCode + "";
                        dsload.Clear();
                        dsload = d2.select_method_wo_parameter(qry, "text");
                        string[] Linkarr1 = Convert.ToString(dsload.Tables[0].Rows[0]["LinkValue"]).Split('/');

                        if (Linkarr.Length > 0)
                        {
                            LinkValueA = Linkarr1[0];
                            LinkValueB = Linkarr1[1];
                        }
                    }
                    if (LinkValueA == "1" && Convert.ToInt32(LinkValueB) > 0)
                    {
                        int renewTimes = 0;
                        renewTimes = Convert.ToInt32(d2.GetFunction("select isnull(max(renewaltimes),0) renewaltimes from borrow where acc_no='" + varAccno + "' and return_type='" + book_type + "' AND ROLL_NO='" + txtRollNo.Text + "' "));
                        renewTimes = renewTimes + 1;
                        if (renewTimes <= Convert.ToInt32(LinkValueB))
                        {
                            Sql = "update borrow set renewaltimes=" + renewTimes + ",Return_Flag=1,mode=0,return_date='" + issuedDate + "',cirno_return=" + issuedCnt + ",book_returnby='" + ReturnedBy + "',Returned_Time ='" + time + "' where acc_no='" + varAccno + "' and return_flag=0 and lib_code='" + lib + "' and return_type='" + book_type + "' and roll_no ='" + txtRollNo.Text + "' ";
                            update = d2.update_method_wo_parameter(Sql, "Text");
                            if (RblMemType.SelectedIndex == 0)
                            {
                                Sql = "insert into borrow(acc_no,title,author,call_no,token_no,roll_no,stud_name,is_staff,borrow_date,due_date,return_date,return_type,access_date,access_time,lib_code,return_flag,cirno_issue,book_returnby,renewflag,Issued_Time,Returned_Time) values ('" + varAccno + "','" + vartitle + "','" + varauthor + "','" + varCallNo + "','" + varToken + "','" + txtRollNo.Text + "','" + TxtName.Text + "',0,'" + issuedDate + "','" + Duedate + "','" + CurrentDate + "','" + book_type + "','" + CurrentDate + "','" + time + "','" + lib + "',0," + circnumgeniss + ",'Member',1,'" + time + "','')";
                                update = d2.update_method_wo_parameter(Sql, "Text");
                            }
                            else if (RblMemType.SelectedIndex == 1)
                            {
                                Sql = "insert into borrow(acc_no,title,author,call_no,token_no,roll_no,stud_name,is_staff,borrow_date,due_date,return_date,return_type,access_date,access_time,lib_code,return_flag,cirno_issue,book_returnby,renewflag,Issued_Time,Returned_Time) values ('" + varAccno + "','" + vartitle + "','" + varauthor + "','" + varCallNo + "','" + varToken + "','" + txtRollNo.Text + "','" + TxtName.Text + "',1,'" + issuedDate + "','" + Duedate + "','" + CurrentDate + "','" + book_type + "','" + CurrentDate + "','" + time + "','" + lib + "',0," + circnumgeniss + ",'Member',1,'" + time + "','')";
                                update = d2.update_method_wo_parameter(Sql, "Text");
                            }
                            //Link setting for library
                            string lid = string.Empty;
                            string hid = string.Empty;

                            if (Convert.ToInt32(txt_TotalDue.Text) > 0)
                            {
                                if (rblfine.SelectedIndex == 1)
                                {
                                    Sql = "select * from New_InsSettings where LinkName='LibraryFine' and college_code='" + ColCode + "' ";
                                    dsprint.Clear();
                                    dsprint = d2.select_method_wo_parameter(Sql, "text");
                                    if (dsprint.Tables[0].Rows.Count > 0)
                                    {
                                        string[] linkval = Convert.ToString(dsprint.Tables[0].Rows[0]["LinkValue"]).Split(',');
                                        hid = linkval[0];
                                        lid = linkval[1];
                                    }
                                    else
                                    {
                                        imgdiv2.Visible = true;
                                        lbl_alertMsg.Text = "Please Set the Header and Ledger to Save the Fine amount";
                                        return;
                                    }
                                }
                                //finYeaid = d2.getCurrentFinanceYear(userCode, college);
                                if (finYeaid == "")
                                {
                                    imgdiv2.Visible = true;
                                    lbl_alertMsg.Text = "Please Set the Financial Year to Save the Fine amount";
                                    return;
                                }

                                string strSemester = "";
                                int isStaff = 0;
                                int memtype = 0;
                                string appno = string.Empty;
                                string feeCat = string.Empty;
                                if (RblMemType.SelectedIndex == 0)
                                {
                                    strSemester = d2.GetFunction("SELECT Current_Semester FROM Registration WHERE Roll_No = '" + txtRollNo.Text + "' or lib_id ='" + txtRollNo.Text + "'");
                                    appno = d2.GetFunction("SELECT app_no FROM Registration WHERE Roll_No = '" + txtRollNo.Text + "' or lib_id ='" + txtRollNo.Text + "'");
                                    feeCat = d2.GetFunction("select textcode from textvaltable where TextCriteria = 'FEECA'and textval = '" + strSemester.Trim() + " Semester' and textval not like '-1%' and college_code='" + ColCode + "'");
                                    isStaff = 0;
                                    memtype = 1;
                                }
                                else if (RblMemType.SelectedIndex == 1)
                                {
                                    appno = d2.GetFunction("select appl_id from staff_appl_master sa,staffmaster s where staff_code='" + txtRollNo.Text + "' and sa.appl_no=s.appl_no");
                                    strSemester = "0";
                                    isStaff = 1;
                                    memtype = 2;
                                    feeCat = "0";
                                }
                                else
                                {
                                    strSemester = "0";
                                    isStaff = 1;
                                    memtype = 3;
                                    feeCat = "0";
                                }
                                Sql = "if exists (select * from fine_details where receipt_no='' and roll_no='" + txtRollNo.Text + "' and token_no='" + varToken + "' and acc_no='" + varAccno + "' and fineamt=" + fine1 + " and paidamt='0' and description='Lost and Overdue Fine' and booktype='" + book_type + "' and semester='" + strSemester + "' and iss_date='" + borrowdt + "' and due_date='" + dueDt + "' and cal_date='" + issuedDate + "' and is_staff='" + isStaff + "' and lib_code='" + lib + "' and title='" + vartitle + "' ) update fine_details set receipt_no='', roll_no='" + txtRollNo.Text + "' , token_no='" + varToken + "' , acc_no='" + varAccno + "' , fineamt=" + fine1 + " , paidamt='0' , description='Lost , Overdue Fine' , booktype='" + book_type + "' , semester='" + strSemester + "' , iss_date='" + borrowdt + "' , due_date='" + dueDt + "' , cal_date='" + issuedDate + "' , is_staff='" + isStaff + "' , lib_code='" + lib + "' , title='" + vartitle + "' where receipt_no='' and roll_no='" + txtRollNo.Text + "' and token_no='" + varToken + "' and acc_no='" + varAccno + "' and fineamt=" + fine1 + " else INSERT into fine_details(receipt_no,roll_no ,token_no,acc_no,fineamt,paidamt,description,booktype,semester,iss_date,due_date, cal_date,is_staff,lib_code,title)  VALUES ('','" + txtRollNo.Text + "','" + varToken + "','" + varAccno + "', " + fine1 + ",'0','Lost and Overdue Fine','" + book_type + "','" + strSemester + "','" + borrowdt + "','" + dueDt + "','" + issuedDate + "','" + isStaff + "','" + lib + "','" + vartitle + "')";
                                update = d2.update_method_wo_parameter(Sql, "Text");
                                double FeeAllot = 0;
                                double PaidAmt = 0;
                                double BalanceAmt = 0;
                                string updateQuery = string.Empty;
                                string serverDate = d2.ServerDate();
                                string insertQuery = " INSERT INTO FT_FeeAllot (AllotDate,MemType,PayMode,App_No,LedgerFK,HeaderFK,FeeAmount,TotalAmount,FeeCategory,BalAmount,FinYearFK) VALUES('" + serverDate + "'," + memtype + ",1," + appno + "," + lid + "," + hid + "," + fine1 + "," + fine1 + "," + feeCat + "," + fine1 + ",'" + finYeaid + "') ";

                                string selectQuery = " select * from FT_FeeAllot where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";

                                ds.Clear();
                                ds = d2.select_method_wo_parameter(selectQuery, "text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    FeeAllot = Convert.ToDouble(ds.Tables[0].Rows[0]["TotalAmount"]);
                                    PaidAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["PaidAmount"]);
                                    BalanceAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["BalAmount"]);
                                    double FinalFeeAllot = Convert.ToDouble(FeeAllot) + Convert.ToDouble(fine1);
                                    double FinalBalAmt = FinalFeeAllot - PaidAmt;
                                    updateQuery = " update FT_FeeAllot set AllotDate='" + serverDate + "', MemType=" + memtype + ",FeeAmount=" + FinalFeeAllot + ",BalAmount=" + FinalBalAmt + ",TotalAmount=" + FinalFeeAllot + " where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and FeeCategory in('" + feeCat + "')  and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";//isnull(TotalAmount,0)+
                                }
                                else
                                {
                                    updateQuery = " update FT_FeeAllot set AllotDate='" + serverDate + "', MemType=" + memtype + ",FeeAmount=" + fine1 + ",BalAmount=" + fine1 + ",TotalAmount=" + fine1 + " where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and FeeCategory in('" + feeCat + "')  and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";
                                }
                                string finalQuery = " if exists ( " + selectQuery + " ) " + updateQuery + " else " + insertQuery + " ";
                                update = d2.update_method_wo_parameter(finalQuery, "Text");
                            }

                            blnSave = true;
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alertMsg.Text = "Your Renewal Count has been Expired";
                            ClearFunction();
                            hsAccNo.Clear();
                            Txtaccno.Text = "";
                            rblissue.SelectedIndex = 0;
                            return;
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alertMsg.Text = "Can't renewal the book, give the renewal permission";
                        ClearFunction();
                        Txtaccno.Text = "";
                        hsAccNo.Clear();
                        rblissue.SelectedIndex = 0;
                        return;
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alertMsg.Text = "No book to renewal";
                    hsAccNo.Clear();
                    ClearFunction();
                    Txtaccno.Text = "";
                    rblissue.SelectedIndex = 0;
                    return;
                }
            }
            if (blnSave == true)
            {
                DivMess.Visible = false;
                imgdiv2.Visible = true;
                string var = Convert.ToString(BtnYes.TabIndex);
                if (var == "2")
                {
                    btn_errorclose.Focus();
                    btn_errorclose.BackColor = Color.LightGreen;
                }
                DispStatusList();
                DivReturnLost.Visible = true;
                LblReturnLost.Text = "Renewal entry saved successfully";
                //lbl_alertMsg.Text = "Renewal entry saved successfully";
                hsAccNo.Clear();
                ClearFunction();
                ViewState["CurrentTable"] = null;
                GrdIssuingBook.DataSource = null;
                GrdIssuingBook.DataBind();
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, collcode, "bookissue.aspx"); }
    }

    protected void cmdReturn_Click()
    {
        string coll = Convert.ToString(ddlcollege.SelectedValue);
        multiplereturn();
        string link = d2.GetFunction("select linkvalue from inssettings where linkname='Automatic Card Lock' and college_code='" + coll + "'");
        if (link == "1")
            Auto_Cardlock();

        //Call cleardata
        //InVisibleFineDet();
    }

    protected void multiplereturn()
    {
        try
        {
            RollNoChallanReceipt = txtRollNo.Text;
            int IntTokenNo = 0;
            string StrTokenNo = "";
            string book_type = string.Empty;
            string issueType = Convert.ToString(ddlissue.SelectedValue);
            string college = Convert.ToString(ddlcollege.SelectedValue);
            string lib = Convert.ToString(ddllibrary.SelectedValue);
            double FineAmount = 0;
            double CostPrice = 0;
            string Sql = "";
            if (issueType == "Book")
                book_type = "BOK";
            if (issueType == "Periodicals")
                book_type = "PER";
            if (issueType == "Project Book")
                book_type = "PRO";
            if (issueType == "Non-Book Material")
                book_type = "NBM";
            if (issueType == "Question Bank")
                book_type = "QBA";
            if (issueType == "Back Volume")
                book_type = "BVO";
            if (issueType == "Reference Books")
                book_type = "REF";
            if (RblMemType.SelectedIndex == 1)
                Session["category"] = "Staff";
            else if (RblMemType.SelectedIndex == 0)
                Session["category"] = "Student";
            else if (RblMemType.SelectedIndex == 2)
                Session["category"] = "Nonmember";
            StrMemberType = Convert.ToString(Session["category"]).Trim();
            string ReturnedBy = d2.GetFunction("select Full_Name from usermaster where user_code='" + userCode + "'");
            string returnBy = ReturnedBy + "/" + StrMemberType;
            string borrowdt = "";
            string dueDt = "";
            //IntTokenNo = InStr(1, txt_tokenno.Text, ".")
            //StrTokenNo = Mid(txt_tokenno, (IntTokenNo + 1), 1)
            //If (Chk_FineMeritCard.value = 1 And StrTokenNo = "M") Or StrTokenNo <> "M" Then
            //    If feeconrs.State Then feeconrs.Close
            //    feeconrs.Open ("select isfine_off,ISNULL(Lib_FeeCode,0) Lib_FeeCode from library where lib_code='" + lib + "' "), db, adOpenDynamic, adLockOptimistic
            //    If Not feeconrs.EOF Then
            //        If feeconrs("isfine_off") = True And Lib_FeeCode <> 0 Then
            //            Int_FeeCode = feeconrs("Lib_FeeCode")
            //            fee_value = Txt_TotalDue.Text
            //            If rollnors.State Then rollnors.Close
            //            If cbo_UserEntry.Text = "Roll Number" Then
            //                Set rollnors =Sql("select roll_admit from registration where roll_no='" + txt_rollno.Text + "'")
            //            ElseIf cbo_UserEntry.Text = "Library ID" Then
            //                Set rollnors =Sql("select roll_admit from registration where Lib_ID='" + txt_rollno.Text + "'")
            //            ElseIf cbo_UserEntry.Text = "Admission Number" Then
            //                Set rollnors =Sql("select roll_admit from registration where Lib_ID='" + txt_rollno.Text + "'")
            //            ElseIf cbo_UserEntry.Text = "Registration Number" Then
            //                Set rollnors =Sql("select roll_admit from registration where Lib_ID='" + txt_rollno.Text + "'")
            //            End If
            //            If (rollnors.RecordCount > 0) Then
            //                rolladmit = rollnors(0)
            //            End If
            //            rollnors.Close
            //        End If
            //    End If
            //End If ' for merit card
            string issuedDate = txtissuedate.Text;
            string[] dtIssue = issuedDate.Split('/');
            if (dtIssue.Length == 3)
                issuedDate = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
            string FinalDueDt = Txtduedate.Text;
            //string[] dtDue = FinalDueDt.Split('/');
            //if (dtDue.Length == 3)
            //    dueDt = dtDue[1].ToString() + "/" + dtDue[0].ToString() + "/" + dtDue[2].ToString();
            string finYeaid = d2.getCurrentFinanceYear(userCode, college);
            string serverDt = d2.ServerDate();
            string servertime = d2.ServerTime();
            string CurrentDate = servertime;
            string[] yearsplit = serverDt.Split('/');
            string Currentyear = yearsplit[2];
            string accNumber = string.Empty;
            for (int i = 0; i < GrdIssuingBook.Rows.Count; i++)
            {
                varAccno = Convert.ToString(GrdIssuingBook.Rows[i].Cells[2].Text);
                vartitle = Convert.ToString(GrdIssuingBook.Rows[i].Cells[3].Text);
                varToken = Convert.ToString(GrdIssuingBook.Rows[i].Cells[9].Text);
                varIssueDate = Convert.ToString(GrdIssuingBook.Rows[i].Cells[6].Text);
                if (accNumber == "")
                {
                    accNumber = varAccno;
                }
                else
                {
                    accNumber = accNumber + "','" + varAccno;
                }
                string[] dt_Issue = varIssueDate.Split('/');
                if (dt_Issue.Length == 3)
                    borrowdt = dt_Issue[1].ToString() + "/" + dt_Issue[0].ToString() + "/" + dt_Issue[2].ToString();

                varauthor = Convert.ToString(GrdIssuingBook.Rows[i].Cells[4].Text);
                varCallNo = Convert.ToString(GrdIssuingBook.Rows[i].Cells[5].Text);
                varDueDate = Convert.ToString(GrdIssuingBook.Rows[i].Cells[8].Text);
                string[] dtDue = varDueDate.Split('/');
                if (dtDue.Length == 3)
                    dueDt = dtDue[1].ToString() + "/" + dtDue[0].ToString() + "/" + dtDue[2].ToString();

                string fineAmt = Convert.ToString(GrdIssuingBook.Rows[i].Cells[10].Text);
                if (fineAmt != "")
                {
                    fine1 = Convert.ToDouble(GrdIssuingBook.Rows[i].Cells[10].Text);
                }
                int issuedCnt = Convert.ToInt32(TxtissuedCount.Text) + 1;
                string lid = string.Empty;
                string hid = string.Empty;
                string LostBklid = string.Empty;
                string LostBkhid = string.Empty;
                if (!string.IsNullOrEmpty(txt_TotalDue.Text))
                    FineAmount = Convert.ToDouble(txt_TotalDue.Text);
                if (!string.IsNullOrEmpty(txt_lostprice.Text))
                    CostPrice = Convert.ToDouble(txt_lostprice.Text);
                if (FineAmount > 0 || CostPrice > 0)
                {
                    if (FineAmount > 0)
                    {
                        Sql = "select * from New_InsSettings where LinkName='LibraryFine' and college_code='" + college + "' ";
                        dsprint.Clear();
                        dsprint = d2.select_method_wo_parameter(Sql, "text");
                        if (dsprint.Tables[0].Rows.Count > 0)
                        {
                            string[] linkval = Convert.ToString(dsprint.Tables[0].Rows[0]["LinkValue"]).Split(',');
                            hid = linkval[0];
                            lid = linkval[1];
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alertMsg.Text = "Please Set the Header and Ledger for OverDue Fine";
                            return;
                        }
                    }
                    if (CostPrice > 0)
                    {
                        Sql = "select * from New_InsSettings where LinkName='LostBookFine' and college_code='" + college + "' ";
                        dsprint.Clear();
                        dsprint = d2.select_method_wo_parameter(Sql, "text");
                        if (dsprint.Tables[0].Rows.Count > 0)
                        {
                            string[] linkval = Convert.ToString(dsprint.Tables[0].Rows[0]["LinkValue"]).Split(',');
                            LostBkhid = linkval[0];
                            LostBklid = linkval[1];
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alertMsg.Text = "Please Set the Header and Ledger for Lost Books";
                            return;
                        }
                    }
                    if (finYeaid == "")
                    {
                        imgdiv2.Visible = true;
                        lbl_alertMsg.Text = "Please Set the Financial Year";
                        return;
                    }
                }
                //For checking the finacial year
                string finSql = "select * from FM_FinYearMaster where FinYearPK='" + finYeaid + "'";
                DataSet dsFinYear = new DataSet();
                dsFinYear = d2.select_method_wo_parameter(finSql, "text");
                if (dsFinYear.Tables[0].Rows.Count > 0)
                {
                    //Update Return Flag in Transaction               
                    if (rblfine.SelectedIndex == 0)
                    {
                        Sql = "update borrow set Return_Flag=1,mode=1,return_date='" + issuedDate + "',cirno_return=" + issuedCnt + ",book_returnby='" + returnBy + "',Returned_Time='" + CurrentDate + "' where acc_no='" + varAccno + "' and return_flag=0 and lib_code='" + lib + "' and return_type='" + book_type + "'";
                    }
                    else
                    {
                        Sql = "update borrow set Return_Flag=1,mode=0,return_date='" + issuedDate + "',cirno_return=" + issuedCnt + ",book_returnby='" + returnBy + "',Returned_Time='" + CurrentDate + "' where acc_no='" + varAccno + "' and return_flag=0 and lib_code='" + lib + "' and return_type='" + book_type + "'";
                    }
                    update = d2.update_method_wo_parameter(Sql, "Text");
                    Sql = "UPDATE tokendetails set is_locked=0 where token_no='" + varToken + "'";
                    update = d2.update_method_wo_parameter(Sql, "Text");

                    //Update Book Status in Books
                    if (book_type == "BOK" || book_type == "REF")
                    {
                        if (rblissue.SelectedIndex == 1)
                        {
                            Sql = "UPDATE bookdetails set book_status='Available' where acc_no='" + varAccno + "'  and lib_code='" + lib + "'";
                            update = d2.update_method_wo_parameter(Sql, "text");
                        }
                        else if (rblissue.SelectedIndex == 3)
                        {
                            Sql = "UPDATE bookdetails set book_status='Lost' where acc_no='" + varAccno + "' and lib_code='" + lib + "'";
                            update = d2.update_method_wo_parameter(Sql, "text");
                            Sql = "insert into bookstatus(acc_no,book_type,y_lost,lib_code) values ('" + varAccno + "','" + book_type + "','" + Currentyear + "','" + lib + "')";
                            update = d2.update_method_wo_parameter(Sql, "text");
                        }
                    }
                    else if (book_type == "PER")
                    {
                        if (rblissue.SelectedIndex == 1)
                        {
                            Sql = "UPDATE journal set issue_flag='Available' where access_code='" + varAccno + "' and lib_code='" + lib + "' ";
                            update = d2.update_method_wo_parameter(Sql, "text");
                        }
                        else if (rblissue.SelectedIndex == 3)
                        {
                            Sql = "UPDATE journal set issue_flag='Lost' where access_code='" + varAccno + "'and lib_code='" + lib + "'";
                            update = d2.update_method_wo_parameter(Sql, "text");
                        }
                    }
                    else if (book_type == "PRO")
                    {
                        if (rblissue.SelectedIndex == 1)
                        {
                            Sql = "UPDATE project_book set issue_flag='Available' where probook_accno='" + varAccno + "'and lib_code='" + lib + "'";
                            update = d2.update_method_wo_parameter(Sql, "text");
                        }
                        else if (rblissue.SelectedIndex == 3)
                        {
                            Sql = "UPDATE project_book set issue_flag='Lost' where probook_accno='" + varAccno + "'and lib_code='" + lib + "'";
                            update = d2.update_method_wo_parameter(Sql, "text");
                            Sql = "insert into bookstatus(acc_no,book_type,y_lost,lib_code) values ('" + varAccno + "','" + book_type + "','" + Currentyear + "','" + lib + "')";
                        }
                    }
                    else if (book_type == "NBM")
                    {
                        if (rblissue.SelectedIndex == 1)
                        {
                            Sql = "UPDATE nonbookmat set issue_flag='Available' where acc_no='" + varAccno + "'and lib_code='" + lib + "'";
                            update = d2.update_method_wo_parameter(Sql, "text");
                        }
                        else if (rblissue.SelectedIndex == 3)
                        {
                            Sql = "UPDATE nonbookmat set issue_flag='Lost' where acc_no='" + varAccno + "' and lib_code='" + lib + "'";
                            update = d2.update_method_wo_parameter(Sql, "text");
                            Sql = "insert into bookstatus(acc_no,book_type,y_lost,lib_code) values ('" + varAccno + "','" + book_type + "','" + Currentyear + "','" + lib + "')";
                            update = d2.update_method_wo_parameter(Sql, "text");
                        }
                    }
                    else if (book_type == "QBA")
                    {
                        if (rblissue.SelectedIndex == 1)
                        {
                            Sql = "UPDATE university_question set issue_flag='Available' where access_code='" + varAccno + "' and lib_code='" + lib + "'";
                            update = d2.update_method_wo_parameter(Sql, "text");
                        }
                        else if (rblissue.SelectedIndex == 3)
                        {
                            Sql = "UPDATE university_question set issue_flag='Lost' where access_code='" + varAccno + "' and lib_code='" + lib + "'";
                            update = d2.update_method_wo_parameter(Sql, "text");
                        }
                    }
                    else if (book_type == "BVO")
                    {
                        if (rblissue.SelectedIndex == 1)
                        {
                            Sql = "UPDATE back_volume set issue_flag='Available' where access_code='" + varAccno + "' and lib_code='" + lib + "'";
                        }
                        else if (rblissue.SelectedIndex == 3)
                        {
                            Sql = "UPDATE back_volume set issue_flag='Lost' where access_code='" + varAccno + "'and lib_code='" + lib + "'";
                            update = d2.update_method_wo_parameter(Sql, "text");
                            Sql = "insert into bookstatus(acc_no,book_type,y_lost,lib_code) values ('" + varAccno + "','" + book_type + "','" + Currentyear + "','" + lib + "')";
                            update = d2.update_method_wo_parameter(Sql, "text");
                        }
                    }
                    //Update Fine Details
                    if (rblissue.SelectedIndex == 3)
                    {
                        if (rblfine.SelectedIndex == 1)
                        {

                        }
                    }
                    if (FineAmount > 0 || CostPrice > 0)
                    {
                        string strSemester = "";
                        int isStaff = 0;
                        int memtype = 0;
                        string appno = string.Empty;
                        string feeCat = string.Empty;
                        if (RblMemType.SelectedIndex == 0)
                        {
                            strSemester = d2.GetFunction("SELECT Current_Semester FROM Registration WHERE Roll_No = '" + txtRollNo.Text + "' or lib_id ='" + txtRollNo.Text + "'");

                            appno = d2.GetFunction("SELECT app_no FROM Registration WHERE Roll_No = '" + txtRollNo.Text + "' or lib_id ='" + txtRollNo.Text + "'");
                            feeCat = d2.GetFunction("select textcode from textvaltable where TextCriteria = 'FEECA'and textval = '" + strSemester.Trim() + " Semester' and textval not like '-1%' and college_code='" + college + "'");
                            isStaff = 0;
                            memtype = 1;
                        }
                        else if (RblMemType.SelectedIndex == 1)
                        {
                            appno = d2.GetFunction("select appl_id from staff_appl_master sa,staffmaster s where staff_code='" + txtRollNo.Text + "' and sa.appl_no=s.appl_no");
                            strSemester = "0";
                            isStaff = 1;
                            memtype = 2;
                            feeCat = "0";
                        }
                        else
                        {
                            strSemester = "0";
                            isStaff = 1;
                            memtype = 3;
                            feeCat = "0";
                        }
                        string serverDate = d2.ServerDate();

                        if (FineAmount > 0)
                        {
                            Sql = "if exists (select * from fine_details where receipt_no='' and roll_no='" + txtRollNo.Text + "' and token_no='" + varToken + "' and acc_no='" + varAccno + "' and fineamt=" + fine1 + " and paidamt='0' and description='Overdue Fine' and booktype='" + book_type + "' and semester='" + strSemester + "' and iss_date='" + borrowdt + "' and due_date='" + dueDt + "' and cal_date='" + issuedDate + "' and is_staff='" + isStaff + "' and lib_code='" + lib + "' and title='" + vartitle + "' ) update fine_details set receipt_no='', roll_no='" + txtRollNo.Text + "' , token_no='" + varToken + "' , acc_no='" + varAccno + "' , fineamt=" + fine1 + " , paidamt='0' , description='Overdue Fine' , booktype='" + book_type + "' , semester='" + strSemester + "' , iss_date='" + borrowdt + "' , due_date='" + dueDt + "' , cal_date='" + issuedDate + "' , is_staff='" + isStaff + "' , lib_code='" + lib + "' , title='" + vartitle + "' where receipt_no='' and roll_no='" + txtRollNo.Text + "' and token_no='" + varToken + "' and acc_no='" + varAccno + "' and fineamt=" + fine1 + " else INSERT into fine_details(receipt_no,roll_no ,token_no,acc_no,fineamt,paidamt,description,booktype,semester,iss_date,due_date, cal_date,is_staff,lib_code,title)  VALUES ('','" + txtRollNo.Text + "','" + varToken + "','" + varAccno + "', " + fine1 + ",'0','Overdue Fine','" + book_type + "','" + strSemester + "','" + borrowdt + "','" + dueDt + "','" + issuedDate + "','" + isStaff + "','" + lib + "','" + vartitle + "')";//," + Txt_ActAmount.Text + "
                            update = d2.update_method_wo_parameter(Sql, "Text");

                            double FeeAllot = 0;
                            double PaidAmt = 0;
                            double BalanceAmt = 0;
                            string updateQuery = string.Empty;
                            string insertQuery = " INSERT INTO FT_FeeAllot (AllotDate,MemType,PayMode,App_No,LedgerFK,HeaderFK,FeeAmount,TotalAmount,FeeCategory,BalAmount,FinYearFK) VALUES('" + issuedDate + "'," + memtype + ",1," + appno + "," + lid + "," + hid + "," + fine1 + "," + fine1 + "," + feeCat + "," + fine1 + ",'" + finYeaid + "') ";
                            string selectQuery = " select * from FT_FeeAllot where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQuery, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                FeeAllot = Convert.ToDouble(ds.Tables[0].Rows[0]["TotalAmount"]);
                                PaidAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["PaidAmount"]);
                                BalanceAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["BalAmount"]);
                                double FinalFeeAllot = Convert.ToDouble(FeeAllot) + Convert.ToDouble(fine1);
                                double FinalBalAmt = FinalFeeAllot - PaidAmt;
                                updateQuery = " update FT_FeeAllot set AllotDate='" + issuedDate + "', MemType=" + memtype + ",FeeAmount=" + FinalFeeAllot + ",BalAmount=" + FinalBalAmt + ",TotalAmount=" + FinalFeeAllot + " where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and FeeCategory in('" + feeCat + "')  and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";//isnull(TotalAmount,0)+
                            }
                            else
                            {
                                updateQuery = " update FT_FeeAllot set AllotDate='" + issuedDate + "', MemType=" + memtype + ",FeeAmount=" + fine1 + ",BalAmount=" + fine1 + ",TotalAmount=" + fine1 + " where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and FeeCategory in('" + feeCat + "')  and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";
                            }
                            string finalQuery = " if exists ( " + selectQuery + " ) " + updateQuery + " else " + insertQuery + " ";
                            update = d2.update_method_wo_parameter(finalQuery, "Text");
                        }
                        if (rblissue.SelectedIndex == 3)
                        {
                            if (rblfine.SelectedIndex == 1)
                            {
                                fine1 = CostPrice;
                                if (CostPrice > 0)
                                {
                                    Sql = "if exists (select * from fine_details where receipt_no='' and roll_no='" + txtRollNo.Text + "' and token_no='" + varToken + "' and acc_no='" + varAccno + "' and fineamt=" + fine1 + " and paidamt='0' and description='Lost Book Fine' and booktype='" + book_type + "' and semester='" + strSemester + "' and iss_date='" + borrowdt + "' and due_date='" + dueDt + "' and cal_date='" + issuedDate + "' and is_staff='" + isStaff + "' and lib_code='" + lib + "' and title='" + vartitle + "' ) update fine_details set receipt_no='', roll_no='" + txtRollNo.Text + "' , token_no='" + varToken + "' , acc_no='" + varAccno + "' , fineamt=" + fine1 + " , paidamt='0' , description='Lost Book Fine' , booktype='" + book_type + "' , semester='" + strSemester + "' , iss_date='" + borrowdt + "' , due_date='" + dueDt + "' , cal_date='" + issuedDate + "' , is_staff='" + isStaff + "' , lib_code='" + lib + "' , title='" + vartitle + "' where receipt_no='' and roll_no='" + txtRollNo.Text + "' and token_no='" + varToken + "' and acc_no='" + varAccno + "' and fineamt=" + fine1 + " else INSERT into fine_details(receipt_no,roll_no ,token_no,acc_no,fineamt,paidamt,description,booktype,semester,iss_date,due_date, cal_date,is_staff,lib_code,title)  VALUES ('','" + txtRollNo.Text + "','" + varToken + "','" + varAccno + "', " + fine1 + ",'0','Lost Book Fine','" + book_type + "','" + strSemester + "','" + borrowdt + "','" + dueDt + "','" + issuedDate + "','" + isStaff + "','" + lib + "','" + vartitle + "')";//," + Txt_ActAmount.Text + "
                                    update = d2.update_method_wo_parameter(Sql, "Text");

                                    double FeeAllot = 0;
                                    double PaidAmt = 0;
                                    double BalanceAmt = 0;
                                    string updateQuery = string.Empty;
                                    string insertQuery = " INSERT INTO FT_FeeAllot (AllotDate,MemType,PayMode,App_No,LedgerFK,HeaderFK,FeeAmount,TotalAmount,FeeCategory,BalAmount,FinYearFK) VALUES('" + issuedDate + "'," + memtype + ",1," + appno + "," + LostBklid + "," + LostBkhid + "," + fine1 + "," + fine1 + "," + feeCat + "," + fine1 + ",'" + finYeaid + "') ";
                                    string selectQuery = " select * from FT_FeeAllot where LedgerFK in('" + LostBklid + "') and HeaderFK in('" + LostBkhid + "') and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";
                                    ds.Clear();
                                    ds = d2.select_method_wo_parameter(selectQuery, "text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        FeeAllot = Convert.ToDouble(ds.Tables[0].Rows[0]["TotalAmount"]);
                                        PaidAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["PaidAmount"]);
                                        BalanceAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["BalAmount"]);
                                        double FinalFeeAllot = Convert.ToDouble(FeeAllot) + Convert.ToDouble(fine1);
                                        double FinalBalAmt = FinalFeeAllot - PaidAmt;
                                        updateQuery = " update FT_FeeAllot set AllotDate='" + issuedDate + "', MemType=" + memtype + ",FeeAmount=" + FinalFeeAllot + ",BalAmount=" + FinalBalAmt + ",TotalAmount=" + FinalFeeAllot + " where LedgerFK in('" + LostBklid + "') and HeaderFK in('" + LostBkhid + "') and FeeCategory in('" + feeCat + "')  and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";//isnull(TotalAmount,0)+
                                    }
                                    else
                                    {
                                        updateQuery = " update FT_FeeAllot set AllotDate='" + issuedDate + "', MemType=" + memtype + ",FeeAmount=" + fine1 + ",BalAmount=" + fine1 + ",TotalAmount=" + fine1 + " where LedgerFK in('" + LostBklid + "') and HeaderFK in('" + LostBkhid + "') and FeeCategory in('" + feeCat + "')  and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";
                                    }
                                    string finalQuery = " if exists ( " + selectQuery + " ) " + updateQuery + " else " + insertQuery + " ";
                                    update = d2.update_method_wo_parameter(finalQuery, "Text");
                                }
                            }
                        }
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alertMsg.Text = "Please Set the Financial Year";
                    return;
                }
            }
            if (rblissue.SelectedIndex == 1)
            {
                string str = string.Empty;
                if (IntDispMess == 1)
                {
                    DivMess.Visible = false;
                    string var = Convert.ToString(BtnYes.TabIndex);
                    if (var == "2")
                    {
                        btn_ReturnLostclose.Focus();
                        btn_ReturnLostclose.BackColor = Color.LightGreen;
                    }
                    DispStatusList();
                    ViewState["CurrentTable"] = null;
                    GrdIssuingBook.DataSource = null;
                    GrdIssuingBook.DataBind();
                    DivReturnLost.Visible = true;
                    LblReturnLost.Text = "Return details saved successfully";
                    hsAccNo.Clear();
                    img_stud1.ImageUrl = "";
                    //int libval = Cbo_CardLibrary.Items.Count;
                    //Cbo_CardLibrary.SelectedValue = Convert.ToString(libval);
                }
                string Libcode = Convert.ToString(ddllibrary.SelectedValue);
                if (StrSaveRollNo != "")
                {
                    Sql = "select * from priority_studstaff where cancel_flag=0 and lib_code='" + Libcode + "' and access_number in('" + accNumber + "') order by access_date";
                }
                else
                {
                    Sql = "select * from priority_studstaff where cancel_flag=0 and lib_code='" + Libcode + "' and access_number in('" + accNumber + "') order by access_date";
                }
                rsreserve.Clear();
                rsreserve = d2.select_method_wo_parameter(Sql, "Text");
                string reserveName = string.Empty;
                string reserveRollNo = string.Empty;
                string reserveAccNo = string.Empty;
                string reserveStaffCode = string.Empty;
                string reserveTitle = "";
                string reserveReqDt = "";
                string reserveReqTime = "";
                string msg = string.Empty;
                int Serno = 0;

                DataTable dtBkReserve = new DataTable();
                DataRow drowInst;
                ArrayList arrColHdrNames = new ArrayList();
                if (rsreserve.Tables[0].Rows.Count > 0)
                {
                    int rowHeight = 0;
                    arrColHdrNames.Add("S.No");
                    dtBkReserve.Columns.Add("S.No");
                    arrColHdrNames.Add("Roll No");
                    dtBkReserve.Columns.Add("Roll No");
                    arrColHdrNames.Add("Name");
                    dtBkReserve.Columns.Add("Name");
                    arrColHdrNames.Add("Access No");
                    dtBkReserve.Columns.Add("Access No");
                    arrColHdrNames.Add("Title");
                    dtBkReserve.Columns.Add("Title");
                    arrColHdrNames.Add("Req Date");
                    dtBkReserve.Columns.Add("Req Date");
                    arrColHdrNames.Add("Req Time");
                    dtBkReserve.Columns.Add("Req Time");

                    DataRow drHdr1 = dtBkReserve.NewRow();
                    for (int grCol = 0; grCol < dtBkReserve.Columns.Count; grCol++)
                        drHdr1[grCol] = arrColHdrNames[grCol];
                    dtBkReserve.Rows.Add(drHdr1);

                    for (int res = 0; res < rsreserve.Tables[0].Rows.Count; res++)
                    {
                        rowHeight += 30;
                        reserveRollNo = Convert.ToString(rsreserve.Tables[0].Rows[res]["roll_no"]);
                        reserveStaffCode = Convert.ToString(rsreserve.Tables[0].Rows[res]["staff_code"]);
                        reserveAccNo = Convert.ToString(rsreserve.Tables[0].Rows[res]["access_number"]);
                        reserveTitle = Convert.ToString(rsreserve.Tables[0].Rows[res]["title"]);
                        reserveReqDt = Convert.ToString(rsreserve.Tables[0].Rows[res]["cur_date"]).Trim();
                        reserveReqTime = Convert.ToString(rsreserve.Tables[0].Rows[res]["cur_time"]);

                        if (!string.IsNullOrEmpty(reserveRollNo) || reserveRollNo != "Nil")
                        {
                            Serno++;
                            drowInst = dtBkReserve.NewRow();
                            drowInst[0] = Convert.ToString(Serno);
                            drowInst[1] = Convert.ToString(reserveRollNo);
                            reserveName = d2.GetFunction("select Stud_Name from Registration where Roll_No='" + reserveRollNo + "' ");
                            drowInst[2] = Convert.ToString(reserveName);
                            drowInst[3] = Convert.ToString(reserveAccNo);
                            drowInst[4] = Convert.ToString(reserveTitle);
                            string[] dt_ReqDt = reserveReqDt.Split('/');
                            if (dt_ReqDt.Length == 3)
                                reserveReqDt = dt_ReqDt[1].ToString() + "/" + dt_ReqDt[0].ToString() + "/" + dt_ReqDt[2].ToString();
                            drowInst[5] = Convert.ToString(reserveReqDt);
                            drowInst[6] = Convert.ToString(reserveReqTime);
                        }
                        else
                        {
                            Serno++;
                            drowInst = dtBkReserve.NewRow();
                            drowInst[0] = Convert.ToString(Serno);
                            drowInst[1] = Convert.ToString(reserveStaffCode);
                            reserveName = d2.GetFunction("select staff_name from staffmaster where staff_code='" + reserveStaffCode + "'");
                            drowInst[2] = Convert.ToString(reserveName);
                            drowInst[3] = Convert.ToString(reserveAccNo);
                            drowInst[4] = Convert.ToString(reserveTitle);
                            string[] dt_ReqDt = reserveReqDt.Split('/');
                            if (dt_ReqDt.Length == 3)
                                reserveReqDt = dt_ReqDt[1].ToString() + "/" + dt_ReqDt[0].ToString() + "/" + dt_ReqDt[2].ToString();
                            drowInst[5] = Convert.ToString(reserveReqDt);
                            drowInst[6] = Convert.ToString(reserveReqTime);
                        }
                        dtBkReserve.Rows.Add(drowInst);
                    }
                    ReservedPopup.Visible = true;
                    GrdReservedBkList.DataSource = dtBkReserve;
                    GrdReservedBkList.DataBind();
                    GrdReservedBkList.Visible = true;

                    GrdReservedBkList.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    GrdReservedBkList.Rows[0].Font.Bold = true;
                    GrdReservedBkList.Rows[0].HorizontalAlign = HorizontalAlign.Center;

                    #region For Sending Sms and EMail

                    string MobileNo = "";
                    string Email = "";
                    string Qry = "";
                    DataSet dsmailSms = new DataSet();
                    for (int res = 0; res < rsreserve.Tables[0].Rows.Count; res++)
                    {
                        reserveRollNo = Convert.ToString(rsreserve.Tables[0].Rows[res]["roll_no"]);
                        reserveAccNo = Convert.ToString(rsreserve.Tables[0].Rows[res]["access_number"]);
                        reserveTitle = Convert.ToString(rsreserve.Tables[0].Rows[res]["title"]);
                        if (reserveRollNo != "Nil")
                        {
                            Qry = "select Student_Mobile,stuper_id from applyn a,Registration r where Roll_No='" + reserveRollNo + "' and a.app_no=r.App_No";
                            dsmailSms = d2.select_method_wo_parameter(Qry, "text");
                            if (dsmailSms.Tables[0].Rows.Count > 0)
                            {
                                MobileNo = Convert.ToString(dsmailSms.Tables[0].Rows[0]["Student_Mobile"]);
                                Email = Convert.ToString(dsmailSms.Tables[0].Rows[0]["stuper_id"]);
                                sendSms(MobileNo, reserveAccNo, reserveTitle);
                                sendEmail(Email, reserveAccNo, reserveTitle);
                            }
                        }
                        reserveStaffCode = Convert.ToString(rsreserve.Tables[0].Rows[res]["staff_code"]);
                        if (reserveStaffCode != "Nil")
                        {
                            Qry = "select per_mobileno,email from staff_appl_master sam,staffmaster sm where sm.staff_code='" + reserveStaffCode + "' and sm.appl_no=sam.appl_no";
                            dsmailSms = d2.select_method_wo_parameter(Qry, "text");
                            if (dsmailSms.Tables[0].Rows.Count > 0)
                            {
                                MobileNo = Convert.ToString(dsmailSms.Tables[0].Rows[0]["per_mobileno"]);
                                Email = Convert.ToString(dsmailSms.Tables[0].Rows[0]["email"]);
                                sendSms(MobileNo, reserveAccNo, reserveTitle);
                                sendEmail(Email, reserveAccNo, reserveTitle);
                            }
                        }
                    }

                    #endregion

                }
                ClearFunction();
                rblissue.SelectedIndex = 0;
                DivMess.Visible = false;
                img_stud1.ImageUrl = "";
                imgBook.ImageUrl = "";
            }
            else if (rblissue.SelectedIndex == 3)
            {
                if (IntDispMess == 1)
                {
                    string var = Convert.ToString(BtnYes.TabIndex);
                    if (var == "2")
                    {
                        btn_ReturnLostclose.Focus();
                        btn_ReturnLostclose.BackColor = Color.LightGreen;
                    }
                    DispStatusList();
                    DivReturnLost.Visible = true;
                    LblReturnLost.Text = "Lost details saved successfully";
                    hsAccNo.Clear();
                    ViewState["CurrentTable"] = null;
                }
                ClearFunction();
                rblissue.SelectedIndex = 0;
                img_stud1.ImageUrl = "";
                imgBook.ImageUrl = "";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collcode, "bookissue.aspx");
        }
    }

    public void sendSms(string MobNo, string accno, string title)
    {
        try
        {
            string Msg = "The Book (AccNo : '" + accno + "'  \n Title : '" + title + "') you have resevered is now available in the library, you can collect it within 2 working days or else the advance booking will be cancelled";
            string user_id = "";
            string ssr = "select * from Track_Value where college_code='" + ddlcollege.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(ssr, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                user_id = Convert.ToString(ds.Tables[0].Rows[0]["SMS_User_ID"]);
            }
            string isst = "0";
            if (!string.IsNullOrEmpty(user_id) && !string.IsNullOrEmpty(MobNo) && MobNo != "0")
            {
                int sms = d2.send_sms(user_id, ddlcollege.SelectedItem.Value, userCode, MobNo, Msg, isst);
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SendSMSToStudnets");
        }
    }

    public void sendEmail(string mail, string accno, string title)
    {
        try
        {
            string send_mail = string.Empty;
            string send_pw = string.Empty;
            string mailId = string.Empty;
            DataTable dtEmailInfo = new DataTable();
            mailId = mail;
            string Msg = "The Book (AccNo : " + accno + "  \n Title : " + title + ") you have resevered is now available in the library, you can collect it within 2 working days or else the booking will be cancelled.";
            if (!string.IsNullOrEmpty(mailId) && mailId != "0")
            {
                string strquery = "select massemail,masspwd from collinfo where college_code ='" + ddlcollege.SelectedItem.Value + "' ";
                dtEmailInfo.Dispose();
                dtEmailInfo.Reset();
                dtEmailInfo = dirAcc.selectDataTable(strquery);
                {
                    send_mail = Convert.ToString(dtEmailInfo.Rows[0]["massemail"]);
                    send_pw = Convert.ToString(dtEmailInfo.Rows[0]["masspwd"]);
                }
                SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                Mail.EnableSsl = true;
                MailMessage mailmsg = new MailMessage();
                MailAddress mfrom = new MailAddress(send_mail);
                mailmsg.From = mfrom;
                mailmsg.To.Add(mailId);

                mailmsg.Subject = "Book Reservation";
                mailmsg.IsBodyHtml = true;
                mailmsg.Body = Msg;
                Mail.EnableSsl = true;
                Mail.UseDefaultCredentials = false;
                NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                Mail.Credentials = credentials;
                Mail.Send(mailmsg);

            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SendEmailToStudnets");
        }
    }

    protected void GrdReservedBkList_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0)
            {
                e.Row.Visible = false;
            }
        }
    }

    protected void btn_ReturnLostclose_Click(object sender, EventArgs e)
    {
        DivReturnLost.Visible = false;
        LostAndFineDiv.Visible = false;
        DataSet dsReturnLost = new DataSet();
        string Libcode = Convert.ToString(ddllibrary.SelectedValue);
        string fine_Collection = d2.GetFunction("select isfine_off from library where lib_code='" + Libcode + "'");
        double FineAmount = 0;
        if (!string.IsNullOrEmpty(txt_TotalDue.Text))
            FineAmount = Convert.ToDouble(txt_TotalDue.Text);
        if (FineAmount > 0)
        {
            //if (fine_Collection.ToLower() == "false")
            //{
            //    Session["Rollno"] = RollNoChallanReceipt;
            //    string redrURI = ConfigurationManager.AppSettings["Finance"].Trim();
            //    string Enq = "C$" + Session["collegecode"] + "&U$" + Session["UserCode"] + "&G$" + Session["group_code"] + "&S$" + Session["Single_User"] + "&P$" + Session["column_header_row_count"] + "&V$" + Session["Version"] + "&SC$" + Session["Staff_Code"] + "&UN$" + Session["UserName"] + "&E$" + Session["Entry_Code"] + "&L$" + Session["IsLogin"] + "&Cu$" + Session["current_college_code"] + "&IC$" + Session["InternalCollegeCode"] + "&Pa$" + Session["password"] + "&PV$" + Session["prntvissble"] + "&D$" + Session["dashprod"] + "&M$" + Session["MID"] + "&SB$" + Session["backbutton_value"] + "&RO$" + Session["Rollno"];
            //    string encQstring = AesEncrypt(Enq);
            //    Response.Redirect(redrURI + "?t*" + encQstring);
            //}
            if (fine_Collection.ToLower() == "false")
            {
                Response.Redirect("~/FinanceMod/ChallanReceipt.aspx?Rollno=" + RollNoChallanReceipt + "");
            }
        }
        if (GrdIssuingBook.Rows.Count > 0)
        {
            for (int issBkRowCnt = 0; issBkRowCnt < GrdIssuingBook.Rows.Count; issBkRowCnt++)
            {
                GrdIssuingBook.Rows[issBkRowCnt].Visible = false;
            }
        }
        if (GrdBookInHand.Rows.Count > 0)
        {
            for (int BkInHandRowCnt = 0; BkInHandRowCnt < GrdBookInHand.Rows.Count; BkInHandRowCnt++)
            {
                GrdBookInHand.Rows[BkInHandRowCnt].Visible = false;
            }
        }
        if (grdReservation.Rows.Count > 0)
        {
            for (int ReservationRowCnt = 0; ReservationRowCnt < grdReservation.Rows.Count; ReservationRowCnt++)
            {
                grdReservation.Rows[ReservationRowCnt].Visible = false;
            }
        }
        imgdiv2.Visible = false;
        txtRollNo.Text = "";
        TxtName.Text = "";
        txtDept.Text = "";
        Txtaccno.Text = "";
        ddlcodenumber.Items.Clear();
        txt_elgi.Text = "";
        txt_issued.Text = "";
        txt_Unlocked.Text = "";
        string serverDt = d2.ServerDate();
        string[] dat = serverDt.Split('/');
        if (dat.Length == 3)
            serverDt = dat[1] + '/' + dat[0] + '/' + dat[2];
        txtissuedate.Text = serverDt.Split(' ')[0];
        Txtduedate.Text = serverDt.Split(' ')[0];
        rblissue.SelectedIndex = 0;
        ddlissue.Enabled = true;
        txtlocked.Text = "";
        Page.Form.DefaultFocus = txtRollNo.ClientID;
        LostAndFineDiv.Visible = false;
    }

    private string AesEncrypt(string password)
    {
        string strmsg = string.Empty;
        byte[] encode = new byte[password.Length];
        encode = Encoding.UTF8.GetBytes(password);
        strmsg = Convert.ToBase64String(encode);
        return strmsg;
    }

    #region Reports

    protected void ChkDueDet_OnCheckedChanged(object sender, EventArgs e)
    {
        divReports.Visible = true;
        divSpreadReport.Visible = true;
        //SpreadReport.Visible = true;
        LblRptName.Text = "Current Due Details";

        #region Value

        string Sql = "";
        string Lib = Convert.ToString(ddllibrary.SelectedValue);
        string serverDt = d2.ServerDate();
        string CurDate = serverDt;
        string colCode = Convert.ToString(ddlcollege.SelectedValue);
        int sno = 0;

        if (BlnAllowMulColStud == true)
        {
            Sql = "SELECT Acc_No as 'Access No',Title,B.Roll_No as 'Roll No',R.Stud_Name as 'Name',Course_Name+'-'+Dept_Name 'Department',Convert(varchar(10),Borrow_Date,103) as 'Issue Date' FROM Borrow B,Registration R,Degree G,Course C,Department D WHERE (B.Roll_No = R.Roll_No OR B.Roll_No = R.Lib_ID) AND R.Degree_Code = G.Degree_Code AND R.College_Code = G.College_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code AND Return_Flag = 0 AND Due_Date ='" + CurDate + "' AND Lib_Code ='" + Lib + "' AND G.College_Code =" + intStudCollCode + "";
            Sql += " UNION ALL SELECT Acc_No as 'Access N'o,Title,B.Roll_No as 'Roll No',M.Staff_Name as 'Name',Dept_Name as 'Department',Convert(varchar(10),Borrow_Date,103) as 'Issue Date' FROM Borrow B,StaffMaster M,StaffTrans T,HrDept_Master D WHERE (B.Roll_No = M.Staff_Code OR B.Roll_No = M.Lib_ID) AND M.Staff_Code = T.Staff_Code AND T.Dept_Code = D.Dept_Code AND M.College_Code = D.College_Code AND T.Latestrec = 1 AND Return_Flag = 0 AND Due_Date ='" + CurDate + "' AND Lib_Code ='" + Lib + "' AND M.College_Code =" + intStudCollCode + "";
            Sql += "ORDER BY Convert(varchar(10),Borrow_Date,103) ";
        }
        else
        {
            Sql = "SELECT Acc_No as 'Access No',Title,B.Roll_No as 'Roll No',R.Stud_Name as 'Name',Course_Name+'-'+Dept_Name 'Department',Convert(varchar(10),Borrow_Date,103) as 'Issue Date' FROM Borrow B,Registration R,Degree G,Course C,Department D WHERE (B.Roll_No = R.Roll_No OR B.Roll_No = R.Lib_ID) AND R.Degree_Code = G.Degree_Code AND R.College_Code = G.College_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code AND Return_Flag = 0 AND Due_Date ='" + CurDate + "' AND Lib_Code ='" + Lib + "' AND G.College_Code =" + colCode + "";
            Sql += " UNION ALL SELECT Acc_No as 'Access No',Title,B.Roll_No as 'Roll No',M.Staff_Name as 'Name',Dept_Name as 'Department', Convert(varchar(10),Borrow_Date,103) as 'Issue Date' FROM Borrow B,StaffMaster M,StaffTrans T,HrDept_Master D WHERE (B.Roll_No = M.Staff_Code OR B.Roll_No = M.Lib_ID) AND M.Staff_Code = T.Staff_Code AND T.Dept_Code = D.Dept_Code AND M.College_Code = D.College_Code AND T.Latestrec = 1 AND Return_Flag = 0 AND Due_Date ='" + CurDate + "' AND Lib_Code ='" + Lib + "' AND M.College_Code =" + colCode + "";
            Sql += "ORDER BY Convert(varchar(10),Borrow_Date,103) ";
        }
        ds.Clear();
        ds = d2.select_method_wo_parameter(Sql, "text");
        DataTable dtDueReport = new DataTable();
        DataRow drowInst;
        ArrayList arrColHdrNames = new ArrayList();
        if (ds.Tables[0].Rows.Count > 0)
        {
            arrColHdrNames.Add("S.No");
            dtDueReport.Columns.Add("S.No");
            arrColHdrNames.Add("Access No");
            dtDueReport.Columns.Add("Access No");
            arrColHdrNames.Add("Title");
            dtDueReport.Columns.Add("Title");
            arrColHdrNames.Add("Roll No");
            dtDueReport.Columns.Add("Roll No");
            arrColHdrNames.Add("Name");
            dtDueReport.Columns.Add("Name");
            arrColHdrNames.Add("Department");
            dtDueReport.Columns.Add("Department");
            arrColHdrNames.Add("Issue Date");
            dtDueReport.Columns.Add("Issue Date");

            DataRow drHdr1 = dtDueReport.NewRow();
            for (int grCol = 0; grCol < dtDueReport.Columns.Count; grCol++)
                drHdr1[grCol] = arrColHdrNames[grCol];
            dtDueReport.Rows.Add(drHdr1);
            int SNo = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                drowInst = dtDueReport.NewRow();
                SNo++;
                drowInst[0] = SNo;
                drowInst[1] = Convert.ToString(ds.Tables[0].Rows[i]["Access No"]);
                drowInst[2] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                drowInst[3] = Convert.ToString(ds.Tables[0].Rows[i]["Roll No"]);
                drowInst[4] = Convert.ToString(ds.Tables[0].Rows[i]["Name"]);
                drowInst[5] = Convert.ToString(ds.Tables[0].Rows[i]["Department"]);
                drowInst[6] = Convert.ToString(ds.Tables[0].Rows[i]["Issue Date"]);
                dtDueReport.Rows.Add(drowInst);
            }
            grdReport.DataSource = dtDueReport;
            grdReport.DataBind();
            divSpreadReport.Visible = true;
            grdReport.Visible = true;
           // print.Visible = true;
            grdReport.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            grdReport.Rows[0].Font.Bold = true;
            grdReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;
        }
        else
        {
            grdReport.DataSource = null;
            grdReport.DataBind();
            divReports.Visible = false;
            divSpreadReport.Visible = false;
            grdReport.Visible = false;
        }
        #endregion

        //print.Visible = true;
    }

    protected void chkissueDet_OnCheckedChanged(object sender, EventArgs e)
    {
        divReports.Visible = true;
        divSpreadReport.Visible = true;
        LblRptName.Text = "Current Issue Details";

        #region Value

        string Sql = "";
        string Lib = Convert.ToString(ddllibrary.SelectedValue);
        string serverDt = d2.ServerDate();
        string CurDate = serverDt;
        string colCode = Convert.ToString(ddlcollege.SelectedValue);
        int sno = 0;
        Sql = "Select distinct(borrow.acc_no)as 'Access No',token_no as 'Card No',cirno_issue as 'Issue Circulation No',borrow.stud_name as 'Name', Convert(varchar(10),Borrow_Date,103) as 'Borrow Date',Convert(varchar(10),due_date,103) as 'Due Date',title as 'Title',author as 'Author',borrow.book_issuedby as 'Book Issued By',borrow.return_type as 'Return Type',library.lib_name as 'Library Name',len(borrow.acc_no) from borrow,library,department,registration,degree where (registration.roll_no=borrow.roll_no or registration.lib_id=borrow.roll_no) and registration.degree_code=degree.degree_code and degree.dept_code=department.dept_code and department.dept_name like '%' AND borrow.lib_code=library.lib_code  and borrow.lib_code like '" + Lib + "'  and borrow_date = '" + CurDate + "' union all  Select distinct(borrow.acc_no)as 'Access No',token_no as 'Card No',cirno_issue as 'Issue Circulation No',borrow.stud_name as 'Name', Convert(varchar(10),Borrow_Date,103) as 'Borrow Date',Convert(varchar(10),due_date,103) as 'Due Date',title as 'Title',author as 'Author',borrow.book_issuedby as 'Book Issued By',borrow.return_type as 'Return Type',library.lib_name as 'Library Name',len(borrow.acc_no)  from borrow,library,staffmaster,stafftrans,hrdept_master where (staffmaster.staff_code=borrow.roll_no or staffmaster.lib_id = borrow.roll_no) and staffmaster.staff_code=stafftrans.staff_code and stafftrans.latestrec=1 and stafftrans.dept_code=hrdept_master.dept_code and hrdept_master.dept_name like '%' AND borrow.lib_code=library.lib_code and is_staff = 1 and borrow.lib_code like '" + Lib + "'  and borrow_date = '" + CurDate + "' union Select distinct(borrow.acc_no)as 'Access No',token_no as 'Card No',cirno_issue as 'Issue Circulation No',borrow.stud_name as 'Name', Convert(varchar(10),Borrow_Date,103) as 'Borrow Date',Convert(varchar(10),due_date,103) as 'Due Date',title as 'Title',author as 'Author',borrow.book_issuedby as 'Book Issued By',borrow.return_type as 'Return Type',library.lib_name as 'Library Name',len(borrow.acc_no) from borrow,library,user_master where user_master.user_id =borrow.roll_no and user_master.department like '%' AND  borrow.lib_code=library.lib_code   and borrow.lib_code like '" + Lib + "' and borrow_date = '" + CurDate + "' order by len(borrow.acc_no),borrow.acc_no,cirno_issue";

        ds.Clear();
        ds = d2.select_method_wo_parameter(Sql, "text");
        DataTable dtIssueReport = new DataTable();
        DataRow drowInst;
        ArrayList arrColHdrNames = new ArrayList();

        if (ds.Tables[0].Rows.Count > 0)
        {
            arrColHdrNames.Add("S.No");
            dtIssueReport.Columns.Add("S.No");
            arrColHdrNames.Add("Access No");
            dtIssueReport.Columns.Add("Access No");
            arrColHdrNames.Add("Card No");
            dtIssueReport.Columns.Add("Card No");
            arrColHdrNames.Add("Issue Circulation No");
            dtIssueReport.Columns.Add("Issue Circulation No");
            arrColHdrNames.Add("Name");
            dtIssueReport.Columns.Add("Name");
            arrColHdrNames.Add("Borrow Date");
            dtIssueReport.Columns.Add("Borrow Date");
            arrColHdrNames.Add("Due Date");
            dtIssueReport.Columns.Add("Due Date");
            arrColHdrNames.Add("Title");
            dtIssueReport.Columns.Add("Title");
            arrColHdrNames.Add("Author");
            dtIssueReport.Columns.Add("Author");
            arrColHdrNames.Add("Book Issued By");
            dtIssueReport.Columns.Add("Book Issued By");
            arrColHdrNames.Add("Return Type");
            dtIssueReport.Columns.Add("Return Type");
            arrColHdrNames.Add("Library Name");
            dtIssueReport.Columns.Add("Library Name");

            DataRow drHdr1 = dtIssueReport.NewRow();
            for (int grCol = 0; grCol < dtIssueReport.Columns.Count; grCol++)
                drHdr1[grCol] = arrColHdrNames[grCol];
            dtIssueReport.Rows.Add(drHdr1);
            int SNo = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                drowInst = dtIssueReport.NewRow();
                SNo++;
                drowInst[0] = SNo;
                drowInst[1] = Convert.ToString(ds.Tables[0].Rows[i]["Access No"]);
                drowInst[2] = Convert.ToString(ds.Tables[0].Rows[i]["Card No"]);
                drowInst[3] = Convert.ToString(ds.Tables[0].Rows[i]["Issue Circulation No"]);
                drowInst[4] = Convert.ToString(ds.Tables[0].Rows[i]["Name"]);
                drowInst[5] = Convert.ToString(ds.Tables[0].Rows[i]["Borrow Date"]);
                drowInst[6] = Convert.ToString(ds.Tables[0].Rows[i]["Due Date"]);
                drowInst[7] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                drowInst[8] = Convert.ToString(ds.Tables[0].Rows[i]["Author"]);
                drowInst[9] = Convert.ToString(ds.Tables[0].Rows[i]["Book Issued By"]);
                drowInst[10] = Convert.ToString(ds.Tables[0].Rows[i]["Return Type"]);
                drowInst[11] = Convert.ToString(ds.Tables[0].Rows[i]["Library Name"]);
                dtIssueReport.Rows.Add(drowInst);
            }

            grdReport.DataSource = dtIssueReport;
            grdReport.DataBind();
            grdReport.Visible = true;
            //print.Visible = true;
            grdReport.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            grdReport.Rows[0].Font.Bold = true;
            grdReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;
        }
        else
        {
            grdReport.DataSource = null;
            grdReport.DataBind();
            divReports.Visible = false;
            divSpreadReport.Visible = false;
            grdReport.Visible = false;
            DivErrorMsg.Visible = true;
            LblErrorMsg.Text = "There is no Books issued Currently";
            ChkissueDet.Checked = false;
        }
        #endregion
    }

    protected void ChkreturnDet_OnCheckedChanged(object sender, EventArgs e)
    {
        divReports.Visible = true;
        divSpreadReport.Visible = true;
        LblRptName.Text = "Current Return Details";

        #region Value

        string Sql = "";
        string Lib = Convert.ToString(ddllibrary.SelectedValue);
        string serverDt = d2.ServerDate();
        string CurDate = serverDt;
        string colCode = Convert.ToString(ddlcollege.SelectedValue);

        Sql = "Select distinct (borrow.acc_no)as 'Access No',token_no as 'Card No',cirno_return as 'Circulation Return',borrow.stud_name as 'Name',Convert(varchar(10),due_date,103) as 'Due Date',Convert(varchar(10),return_date,103) as 'Return Date',title as 'Title',author as 'Author',borrow.book_returnby as 'Book Return By',borrow.return_type as 'Return Type',library.lib_name as 'Library name',len(borrow.acc_no),borrow.acc_no from borrow,library,hrdept_master,staffmaster,stafftrans where (borrow.roll_no=staffmaster.staff_code or borrow.roll_no = staffmaster.lib_id) and staffmaster.staff_code=stafftrans.staff_code and stafftrans.dept_code=hrdept_master.dept_code and hrdept_master.dept_name like '%' AND borrow.lib_code=library.lib_code and  is_staff = 1 and return_flag= 1   and return_date between '" + CurDate + "' and '" + CurDate + "' and return_type like '%' and stafftrans.latestrec=1 and borrow.lib_code= '" + Lib + "'  union all Select distinct referenceissue.acc_no as 'Access No','','',staff_name as 'Name',Convert(varchar(10),duedate,103) as 'Due Date',Convert(varchar(10),returndate,103) as 'Return Date',title as 'Title',author as 'Author','',return_type as 'Return Type',library.lib_name as 'Library Name',len(referenceissue.acc_no),referenceissue.acc_no  from referenceissue,library,hrdept_master,staffmaster,stafftrans where referenceissue.rollno= staffmaster.staff_code  and staffmaster.staff_code=stafftrans.staff_code and stafftrans.dept_code=hrdept_master.dept_code and hrdept_master.dept_name like '%' AND referenceissue.lib_code=library.lib_code and  is_staff = 1 and returnflag= 1 and returndate between '" + CurDate + "' and '" + CurDate + "'  and return_type like '%' and stafftrans.latestrec=1  and referenceissue.lib_code = '" + Lib + "' union all Select distinct (borrow.acc_no)as 'Access No',token_no as 'Card No',cirno_return as 'Circulation Return',borrow.stud_name as 'Name',Convert(varchar(10),due_date,103) as 'Due Date',Convert(varchar(10),return_date,103) as 'Return Date',title as 'Title',author as 'Author',borrow.book_returnby as 'Book Return By',borrow.return_type as 'Return Type',library.lib_name as 'Library name',len(borrow.acc_no),borrow.acc_no  from borrow,library,user_master where borrow.roll_no=user_master.user_id and USER_master.department like '%' AND borrow.lib_code=library.lib_code and user_master.is_staff = 1 and borrow.is_staff=1 and return_flag= 1 and return_date between '" + CurDate + "' and '" + CurDate + "' union all Select distinct referenceissue.acc_no as 'Access No','','',name as 'Name',Convert(varchar(10),duedate,103) as 'Due Date',Convert(varchar(10),returndate,103) as 'Return Date',title as 'Title',author as 'Author','',return_type as 'Return Type',library.lib_name as 'Library Name',len(referenceissue.acc_no),referenceissue.acc_no  from referenceissue,library,user_master where referenceissue.rollno=user_master.user_id and user_master.department like '%' AND referenceissue.lib_code=library.lib_code and user_master.is_staff = 1 and referenceissue.is_staff=1 and returnflag= 1   and returndate between '" + CurDate + "' and '" + CurDate + "'  and referenceissue.lib_code = '" + Lib + "' union all Select distinct (borrow.acc_no)as 'Access No',token_no as 'Card No',cirno_return as 'Circulation Return',borrow.stud_name as 'Name',Convert(varchar(10),due_date,103) as 'Due Date',Convert(varchar(10),return_date,103) as 'Return Date',title as 'Title',author as 'Author',borrow.book_returnby as 'Book Return By',borrow.return_type as 'Return Type',library.lib_name as 'Library name',len(borrow.acc_no),borrow.acc_no from borrow,library,department,registration,degree where (registration.lib_id=borrow.roll_no or registration.roll_no=borrow.roll_no)and registration.degree_code=degree.degree_code and degree.dept_code=department.dept_code and department.dept_name like '%' AND borrow.lib_code=library.lib_code and return_flag= 1  and return_date between '" + CurDate + "' and '" + CurDate + "' and return_type like '%' and borrow.lib_code = '" + Lib + "' union all Select distinct referenceissue.acc_no as 'Access No','','',stud_name as 'Name',Convert(varchar(10),duedate,103) as 'Due Date',Convert(varchar(10),returndate,103) as 'Return Date',title as 'Title',author as 'Author','',return_type as 'Return Type',library.lib_name as 'Library Name',len(referenceissue.acc_no),referenceissue.acc_no  from referenceissue,library,department,registration,degree where registration.roll_no=referenceissue.rollno and registration.degree_code=degree.degree_code and degree.dept_code=department.dept_code and department.dept_name like '%' AND referenceissue.lib_code=library.lib_code and returnflag= 1  and returndate between '" + CurDate + "' and '" + CurDate + "'  and return_type like '%' and referenceissue.lib_code = '" + Lib + "' union all Select distinct referenceissue.acc_no as 'Access No','','',name as 'Name',Convert(varchar(10),duedate,103) as 'DueDate',Convert(varchar(10),returndate,103) as 'ReturnDate',title as 'Title',author as 'Author','',return_type as 'Return Type',library.lib_name as 'Library Name',len(referenceissue.acc_no),referenceissue.acc_no  from referenceissue,library,user_master where user_master.user_id=referenceissue.rollno and user_master.department like '%' AND referenceissue.lib_code=library.lib_code and returnflag= 1  and referenceissue.lib_code = '" + Lib + "'  and returndate between '" + CurDate + "' and '" + CurDate + "' order by len(acc_no),acc_no";

        ds.Clear();
        ds = d2.select_method_wo_parameter(Sql, "text");
        DataTable dtReturnReport = new DataTable();
        DataRow drowInst;
        ArrayList arrColHdrNames = new ArrayList();
        if (ds.Tables[0].Rows.Count > 0)
        {
            arrColHdrNames.Add("S.No");
            dtReturnReport.Columns.Add("S.No");
            arrColHdrNames.Add("Access No");
            dtReturnReport.Columns.Add("Access No");
            arrColHdrNames.Add("Card No");
            dtReturnReport.Columns.Add("Card No");
            arrColHdrNames.Add("Circulation Return");
            dtReturnReport.Columns.Add("Circulation Return");
            arrColHdrNames.Add("Name");
            dtReturnReport.Columns.Add("Name");
            arrColHdrNames.Add("Due Date");
            dtReturnReport.Columns.Add("Due Date");
            arrColHdrNames.Add("Return Date");
            dtReturnReport.Columns.Add("Return Date");
            arrColHdrNames.Add("Title");
            dtReturnReport.Columns.Add("Title");
            arrColHdrNames.Add("Author");
            dtReturnReport.Columns.Add("Author");
            arrColHdrNames.Add("Book Return By");
            dtReturnReport.Columns.Add("Book Return By");
            arrColHdrNames.Add("Return Type");
            dtReturnReport.Columns.Add("Return Type");
            arrColHdrNames.Add("Library Name");
            dtReturnReport.Columns.Add("Library Name");

            DataRow drHdr1 = dtReturnReport.NewRow();
            for (int grCol = 0; grCol < dtReturnReport.Columns.Count; grCol++)
                drHdr1[grCol] = arrColHdrNames[grCol];
            dtReturnReport.Rows.Add(drHdr1);
            int SNo = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                drowInst = dtReturnReport.NewRow();
                SNo++;
                drowInst[0] = SNo;
                drowInst[1] = Convert.ToString(ds.Tables[0].Rows[i]["acc_no"]);
                drowInst[2] = Convert.ToString(ds.Tables[0].Rows[i]["Card No"]);
                drowInst[3] = Convert.ToString(ds.Tables[0].Rows[i]["Circulation Return"]);
                drowInst[4] = Convert.ToString(ds.Tables[0].Rows[i]["Name"]);
                drowInst[5] = Convert.ToString(ds.Tables[0].Rows[i]["Due Date"]);
                drowInst[6] = Convert.ToString(ds.Tables[0].Rows[i]["Return Date"]);
                drowInst[7] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                drowInst[8] = Convert.ToString(ds.Tables[0].Rows[i]["Author"]);
                drowInst[9] = Convert.ToString(ds.Tables[0].Rows[i]["Book Return By"]);
                drowInst[10] = Convert.ToString(ds.Tables[0].Rows[i]["Return Type"]);
                drowInst[11] = Convert.ToString(ds.Tables[0].Rows[i]["Library Name"]);
                dtReturnReport.Rows.Add(drowInst);
            }
            grdReport.DataSource = dtReturnReport;
            grdReport.DataBind();
            grdReport.Visible = true;

            grdReport.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            grdReport.Rows[0].Font.Bold = true;
            grdReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;
        }
        else
        {
            grdReport.DataSource = null;
            grdReport.DataBind();
            grdReport.Visible = false;
        }

        #endregion

        //print.Visible = true;
    }

    protected void imagebtnpopclose5_Click(object sender, EventArgs e)
    {
        divReports.Visible = false;
        ChkDueDet.Checked = false;
        ChkissueDet.Checked = false;
        ChkreturnDet.Checked = false;
    }

    #endregion

    #region Print

    //protected void btnExcel_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string reportname = txtexcelname.Text;
    //        if (reportname.ToString().Trim() != "")
    //        {
    //            d2.printexcelreportgrid(grdReport, reportname);
    //            lblvalidation1.Visible = false;
    //        }
    //        else
    //        {
    //            lblvalidation1.Text = "Please Enter Your  Report Name";
    //            lblvalidation1.Visible = true;
    //            txtexcelname.Focus();
    //        }
    //    }
    //    catch
    //    { }
    //}

    ////public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    ////{ }

    //public void btnprintmaster_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblvalidation1.Text = "";
    //        txtexcelname.Text = "";
    //        string degreedetails = "";
    //        string pagename;
    //        string ss = null;
    //        if (ChkDueDet.Checked)
    //            degreedetails = "Due Details " + '@';
    //        if (ChkreturnDet.Checked)
    //            degreedetails = "Return Details " + '@';
    //        if (ChkissueDet.Checked)
    //            degreedetails = "Issue Details " + '@';
    //        pagename = "bookissue.aspx";
    //        Printcontrol.loadspreaddetails(grdReport, pagename, degreedetails, 0, ss);
    //        Printcontrol.Visible = true;
    //    }
    //    catch { }
    //}

    #endregion

    protected void TxtSmartCard_OnTextChanged(object sender, EventArgs e)
    {
        string UserId = TxtSmartCard.Text;
        if (UserId != "")
        {
            txtRollNo.Text = d2.GetFunction("SELECT Roll_No from registration where smart_serial_no ='" + UserId + "' ");
            if (txtRollNo.Text == "" || txtRollNo.Text == "0")
            {
                txtRollNo.Text = d2.GetFunction("SELECT Staff_Code from StaffMaster where Smartcard_serial_no ='" + UserId + "' ");
                if (txtRollNo.Text == "" || txtRollNo.Text == "0")
                {
                    txtRollNo.Text = d2.GetFunction("SELECT Roll_No from registration where roll_no ='" + UserId + "' ");
                    if (txtRollNo.Text == "" || txtRollNo.Text == "0")
                    {
                        txtRollNo.Text = d2.GetFunction("SELECT Roll_No from registration where lib_id ='" + UserId + "' ");
                        if (txtRollNo.Text == "" || txtRollNo.Text == "0")
                        {
                            txtRollNo.Text = d2.GetFunction("SELECT Staff_Code from StaffMaster where staff_code ='" + UserId + "' ");
                            if (txtRollNo.Text == "" || txtRollNo.Text == "0")
                            {
                                txtRollNo.Text = d2.GetFunction("SELECT Staff_Code from StaffMaster where lib_id ='" + UserId + "' ");
                                if (txtRollNo.Text == "" || txtRollNo.Text == "0")
                                {
                                    txtRollNo.Text = d2.GetFunction("SELECT User_ID from User_Master where User_ID ='" + UserId + "' ");
                                }
                            }
                        }
                    }
                }
            }
        }
        if (txtRollNo.Text != "")
        {
            txtRollNo_Change(sender, e);
        }
    }

    protected void Txtaccno_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string AccNoVal = Convert.ToString(Txtaccno.Text);
            StrAccNo = AccNoVal;
            if (GrdIssuingBook.Rows.Count > 0)
            {
                for (int i = 0; i < GrdIssuingBook.Rows.Count; i++)
                {
                    varAccno = Convert.ToString(GrdIssuingBook.Rows[i].Cells[2].Text);
                    string title = Convert.ToString(GrdIssuingBook.Rows[i].Cells[3].Text);
                    if (!hsAccNo.ContainsKey(varAccno))
                        hsAccNo.Add(varAccno, Convert.ToString(title));
                    else
                    {
                        hsAccNo.Remove(varAccno);
                        hsAccNo.Add(varAccno, Convert.ToString(title));
                    }
                }
            }
            if (!string.IsNullOrEmpty(AccNoVal))
            {
                if (!hsAccNo.Contains(AccNoVal))
                {
                    string AccessCheck = Convert.ToString(Txtaccno.Text);
                    // if (AccNoCheck != AccessCheck)
                    // {
                    string qry1 = "";
                    string Sql = "";
                    string book_type = string.Empty;
                    string issueType = Convert.ToString(ddlissue.SelectedValue);
                    string ColCode = Convert.ToString(ddlcollege.SelectedValue);
                    string lib_code = Convert.ToString(ddllibrary.SelectedValue);
                    string libname = Convert.ToString(ddllibrary.SelectedItem.Text);
                    string CardLibcode = Convert.ToString(Cbo_CardLibrary.SelectedValue);
                    if (issueType == "Book")
                        book_type = "BOK";
                    if (issueType == "Periodicals")
                        book_type = "PER";
                    if (issueType == "Project Book")
                        book_type = "PRO";
                    if (issueType == "Non-Book Material")
                        book_type = "NBM";
                    if (issueType == "Question Bank")
                        book_type = "QBA";
                    if (issueType == "Back Volume")
                        book_type = "BVO";
                    if (issueType == "Reference Books")
                        book_type = "REF";

                    string Library = d2.GetFunction("select lib_code from library where lib_name='" + libname + "'");

                    qry1 = "select * from trace_bookdetails where Acc_no='" + Txtaccno.Text + "' and BookType='" + book_type + "' and Lib_Code =" + Library + "";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(qry1, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DivErrorMsg.Visible = true;
                        LblErrorMsg.Text = "Traze book";
                    }
                    if (!string.IsNullOrEmpty(Txtaccno.Text))
                    {
                        string bkPhoto = d2.GetFunction("select photo from BookPhoto where acc_no='" + Txtaccno.Text + "' and lib_code ='" + lib_code + "' and Book_Type ='" + book_type + "'");
                        imgBook.Visible = true;
                        imgBook.ImageUrl = "~/Handler/BookPhoto.ashx?acc_no=" + Txtaccno.Text + " ";

                        //Image1.Picture = LoadPicture(photoAccess(photoGet, book, txt_accno.Text, GetLibraryCode(cbo_library.Text), book_type))
                        Sql = "SELECT ISNULL(Rack_No,'') Rack_No,ISNULL(Row_No,'') Row_No,ISNULL(Pos_No,'')+'-'+ISNULL(Pos_Place,'') Pos FROM Rack_Allocation WHERE Acc_No = '" + Txtaccno.Text + "' AND Book_Type ='" + book_type + "' AND Lib_Code ='" + lib_code + "' ";
                        dsload.Clear();
                        dsload = d2.select_method_wo_parameter(Sql, "text");
                        if (dsload.Tables[0].Rows.Count > 0)
                        {
                            //Lbl_RackVal.Text = Convert.ToString(dsload.Tables[0].Rows[0]["Rack_No"]);
                            //Lbl_ShelfVal.Text = Convert.ToString(dsload.Tables[0].Rows[0]["Row_No"]);
                            //Lbl_PosVal.Text = Convert.ToString(dsload.Tables[0].Rows[0]["Pos"]);
                        }
                        else
                        {
                            //Lbl_RackVal.Text = "";
                            //Lbl_ShelfVal.Text = "";
                            //Lbl_PosVal.Text = "";
                        }
                        if (book_type == "BOK")
                        {
                            if (AccNoVal.ToUpper() == "ISSUE")
                            {
                                rblissue.SelectedIndex = 0;
                                Txtaccno.Text = "";
                                return;
                            }
                            else if (AccNoVal.ToUpper() == "RETURN")
                            {
                                rblissue.SelectedIndex = 1;
                                Txtaccno.Text = "";
                                return;
                            }
                            else if (AccNoVal.ToUpper() == "RENEWAL")
                            {
                                rblissue.SelectedIndex = 2;
                                Txtaccno.Text = "";
                                return;
                            }
                            else if (AccNoVal.ToUpper() == "LOST")
                            {
                                rblissue.SelectedIndex = 3;
                                Txtaccno.Text = "";
                                return;
                            }
                            // ElseIf Trim(UCase(txt_accno.Text)) = "CLEAR" Or InStr(1, UCase(txt_accno.Text), "CLEAR") > 0 Then
                            //{
                            //     Command2.value = True
                            //     txt_accno.Text = ""
                            //     txt_rollno.SetFocus
                            //     Exit Sub
                            // }
                            if (GrdIssuingBook.Rows.Count == 0 && rblissue.SelectedIndex != 3)
                            {
                                Sql = "SELECT * FROM BookDetails WHERE Acc_No ='" + Txtaccno.Text + "' AND Lib_Code ='" + lib_code + "' ";
                                dsCommon.Clear();
                                dsCommon = d2.select_method_wo_parameter(Sql, "text");
                                if (dsCommon.Tables[0].Rows.Count > 0)
                                {
                                    if (Convert.ToString(dsCommon.Tables[0].Rows[0]["Book_Status"]) == "Issued")
                                    {
                                        if (rblissue.SelectedIndex == 0)
                                        {
                                            rblissue.SelectedIndex = 1;
                                            lblIssSpreadName.Text = "Returning Books";
                                            lblIssSpreadName.Font.Bold = true;
                                            LblSpreadBookName.ForeColor = Color.Green;
                                            lbl_issue.Text = "Return Date";
                                            lbl_issue.Font.Bold = true;
                                            lbl_due.Visible = false;
                                            Txtduedate.Visible = false;
                                            Btnsave.Text = "Return";
                                        }
                                    }
                                    else if (Convert.ToString(dsCommon.Tables[0].Rows[0]["Book_Status"]) == "Available")
                                    {
                                        rblissue.SelectedIndex = 0;
                                        Btnsave.Text = "Issue";
                                        //if(txtRollNo.Text=="")
                                        //{                                 
                                        //    If cbo_UserEntry.Text = "Smart Card" Then
                                        //        Txt_SmartCardID.SetFocus
                                        //    Else
                                        //        txt_rollno.SetFocus
                                        //    End If
                                        //    Exit Sub
                                        //}
                                    }
                                    else if (Convert.ToString(dsCommon.Tables[0].Rows[0]["Book_Status"]) == "Transfered")
                                    {

                                        Sql = "select ISNULL(Transfered,0) Transfered,ISNULL(To_Lib_Code,'') Dept_Code from bookdetails b,book_transfer t where b.acc_no = t.acc_no and transfer_type = 2 and b.acc_no='" + Txtaccno.Text + "' and b.lib_code='" + lib_code + "'";
                                        ds.Clear();
                                        ds = d2.select_method_wo_parameter(Sql, "text");
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {

                                            if (Convert.ToString(dsCommon.Tables[0].Rows[0]["Transfered"]) == "true")
                                            {
                                                DivErrorMsg.Visible = true;
                                                LblErrorMsg.Text = "The Book was Transfered to " + Convert.ToString(dsCommon.Tables[0].Rows[0]["Dept_Code"]) + " Department ";
                                                Txtaccno.Text = "";
                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToString(dsCommon.Tables[0].Rows[0]["Remark"]) != "")
                                        {
                                            DivErrorMsg.Visible = true;
                                            LblErrorMsg.Text = "Access No. : " + Convert.ToString(dsCommon.Tables[0].Rows[0]["Acc_No"]) + "Title : " + Convert.ToString(dsCommon.Tables[0].Rows[0]["Title"]) + " Status : " + Convert.ToString(dsCommon.Tables[0].Rows[0]["Book_Status"]) + " for " + Convert.ToString(dsCommon.Tables[0].Rows[0]["Remark"]);
                                        }
                                        else
                                        {
                                            DivErrorMsg.Visible = true;
                                            LblErrorMsg.Text = "Access No. : " + Convert.ToString(dsCommon.Tables[0].Rows[0]["Acc_No"]) + "Title : " + Convert.ToString(dsCommon.Tables[0].Rows[0]["Title"]) + " Status : " + Convert.ToString(dsCommon.Tables[0].Rows[0]["Book_Status"]);
                                        }
                                        Txtaccno.Text = "";
                                        return;
                                    }
                                }
                                else
                                {
                                    DivErrorMsg.Visible = true;
                                    LblErrorMsg.Text = "Book not found in this library";
                                    Txtaccno.Text = "";
                                    //Page.Form.DefaultFocus = Txtaccno.ClientID;
                                    return;
                                }
                            }
                        }
                        if (AccNoVal.ToUpper() == "ISSUE")
                        {
                            rblissue.SelectedIndex = 0;
                            Txtaccno.Text = "";
                            return;
                        }
                        else if (AccNoVal.ToUpper() == "RETURN")
                        {
                            rblissue.SelectedIndex = 1;
                            Txtaccno.Text = "";
                            return;
                        }
                        else if (AccNoVal.ToUpper() == "RENEWAL")
                        {
                            rblissue.SelectedIndex = 2;
                            Txtaccno.Text = "";
                            return;
                        }
                        else if (AccNoVal.ToUpper() == "LOST")
                        {
                            rblissue.SelectedIndex = 3;
                            Txtaccno.Text = "";
                            return;
                        }
                        else
                        {
                            Valid_Accno(sender, e);
                        }
                    }
                    else
                    {
                    }
                    Page.SetFocus(Txtaccno);
                }
                else
                {
                    Txtaccno.Text = "";
                    Page.SetFocus(Txtaccno);
                }
            }
            else
            {
                this.Txtaccno.Attributes.Add("onkeypress", "button_click(this,'" + this.Btnsave.ClientID + "')");
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collcode, "bookissue.aspx");
        }
    }

    protected void rblfine_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblfine.SelectedIndex == 0)
        {
            //lbl_fine.Visible = false;
            ddlFine.Visible = false;
        }
        if (rblfine.SelectedIndex == 1)
        {
            //lbl_fine.Visible = true;
            ddlFine.Visible = true;

        }
    }

    protected void ddlFine_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string book_type = string.Empty;
        string issueType = Convert.ToString(ddlissue.SelectedValue);
        string college = Convert.ToString(ddlcollege.SelectedValue);
        string lib = Convert.ToString(ddllibrary.SelectedValue);
        string Sql = "";
        if (issueType == "Book")
            book_type = "BOK";
        if (issueType == "Periodicals")
            book_type = "PER";
        if (issueType == "Project Book")
            book_type = "PRO";
        if (issueType == "Non-Book Material")
            book_type = "NBM";
        if (issueType == "Question Bank")
            book_type = "QBA";
        if (issueType == "Back Volume")
            book_type = "BVO";
        if (issueType == "Reference Books")
            book_type = "REF";
        double price = 0;
        rblissue.SelectedIndex = 3;
        string FineVal = Convert.ToString(ddlFine.SelectedValue);
        double fine = 0;
        if (FineVal == "Single")
            fine = 1;
        if (FineVal == "Double")
            fine = 2;
        if (FineVal == "Triple")
            fine = 3;
        if (FineVal == "Four")
            fine = 4;
        if (FineVal == "Five")
            fine = 5;
        if (FineVal == "Six")
            fine = 6;
        if (FineVal == "Seven")
            fine = 7;
        if (FineVal == "Eight")
            fine = 8;
        if (FineVal == "Nine")
            fine = 9;
        if (FineVal == "Ten")
            fine = 10;
        double FineAmt = 0;
        if (book_type == "BOK" || book_type == "REF")
        {
            Sql = "select price from bookdetails where acc_no= '" + StrAccNo + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                price = Convert.ToDouble(ds.Tables[0].Rows[0]["price"]);
                if (!string.IsNullOrEmpty(Convert.ToString(price)))
                {
                    FineAmt = fine * price;
                    txt_lostprice.Text = Convert.ToString(FineAmt);
                }
            }
        }
        if (book_type == "PER")
        {
            Sql = "select journal_price from journal_master where journal_name= '" + StrAccNo + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                price = Convert.ToDouble(ds.Tables[0].Rows[0]["price"]);
                if (!string.IsNullOrEmpty(Convert.ToString(price)))
                {
                    FineAmt = fine * price;
                    txt_lostprice.Text = Convert.ToString(FineAmt);
                }
            }
        }
        if (book_type == "NBM")
        {
            Sql = "select price from nonbookmat where nonbookmat_no= '" + Txtaccno.Text + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                price = Convert.ToDouble(ds.Tables[0].Rows[0]["price"]);
                if (!string.IsNullOrEmpty(Convert.ToString(price)))
                {
                    FineAmt = fine * price;
                    txt_lostprice.Text = Convert.ToString(FineAmt);
                }
            }
        }
    }

    protected void GrdBookInHand_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void GrdBookInHand_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            if (Convert.ToString(rowIndex) != "" || Convert.ToString(rowIndex) != "-1")
            {
                if (rblissue.SelectedIndex != 3)
                {
                    SetDueDate(sender, e);
                }
                else
                {
                    string serverDt = d2.ServerDate();
                    Txtduedate.Text = serverDt;
                }
                string accno = "";
                if (rblissue.SelectedIndex == 1 || rblissue.SelectedIndex == 2 || rblissue.SelectedIndex == 3)
                {
                    accno = GrdBookInHand.Rows[rowIndex].Cells[1].Text;
                    Txtaccno.Text = accno;
                    cmdadd_Click(sender, e);
                }
            }
            popupselectBook.Visible = false;
        }
        catch
        {
        }
    }

    protected void SetDueDate(Object sender, EventArgs e)
    {
        try
        {
            string Libcode = Convert.ToString(Cbo_CardLibrary.SelectedValue);
            string ColCode = Convert.ToString(ddlcollege.SelectedValue);
            string StrBookType = Convert.ToString(ddlBookType.SelectedValue);
            string cardCriteria = Convert.ToString(ddlCardType.SelectedValue);
            string DueDate = string.Empty;
            string duesundate = string.Empty;
            double dudate = 0;
            string Sql = "";
            if (RblMemType.SelectedIndex == 0)
            {
                Sql = "SELECT no_of_days,no_of_token,fine,is_staff,isnull(Ref_NoofDays,0)Ref_NoofDays from lib_master where code='" + deg + "' and batch_year='" + batch_year + "' AND Is_Staff = 0 ";
                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else
                    Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";
                dsprint.Clear();
                dsprint = d2.select_method_wo_parameter(Sql, "Text");

                if (dsprint.Tables[0].Rows.Count > 0)
                {
                    string Library_name = Convert.ToString(ddllibrary.SelectedItem.Text);
                    string NoOfDays = Convert.ToString(dsprint.Tables[0].Rows[0]["no_of_days"]);
                    string Ref_NoofDays = Convert.ToString(dsprint.Tables[0].Rows[0]["Ref_NoofDays"]);
                    string LibraryCode = d2.GetFunction("Select lib_code from library where lib_name='" + Library_name + "' and college_code='" + ColCode + "'");

                    Sql = "select ISBooks_DueDate,books_duedate from library where lib_code ='" + LibraryCode + "' ";
                    rsLib.Clear();
                    rsLib = d2.select_method_wo_parameter(Sql, "Text");
                    if (rsLib.Tables[0].Rows.Count > 0)
                    {
                        string isBookDue = Convert.ToString(rsLib.Tables[0].Rows[0]["ISBooks_DueDate"]);
                        if (isBookDue.ToLower() == "true")
                        {
                            DueDate = Convert.ToString(rsLib.Tables[0].Rows[0]["books_duedate"]);
                            DateTime dt = Convert.ToDateTime(DueDate);
                            Txtduedate.Text = dt.ToString("dd/MM/yyyy");

                        }
                        else
                        {
                            string issueDt = "";

                            ISRefBook(LibraryCode, Txtaccno.Text);
                            issueDt = txtissuedate.Text;
                            if (BlnRef == false)
                            {
                                if (BlnMulRenewDays == false)
                                {
                                    string[] dtIssue = issueDt.Split('/');
                                    if (dtIssue.Length == 3)
                                        issueDt = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                    DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                    duesundate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                    DateTime dt = Convert.ToDateTime(DueDate);
                                    Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                    //a.Caption = rs3(1) & "~" & rs3(2) & "~" & rs3(3) - 1
                                    dudate = Convert.ToDouble(NoOfDays) - 1;
                                }
                                else
                                {
                                    GetRenewalDays(intRenCount, intRenDays, Txtaccno.Text);
                                    string[] dtIssue = issueDt.Split('/');
                                    if (dtIssue.Length == 3)
                                        issueDt = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                    DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                    duesundate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                    DateTime dt = Convert.ToDateTime(DueDate);
                                    Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                    // a.Caption = rs3(1) & "~" & rs3(2) & "~" & rs3(3) - 1
                                    dudate = Convert.ToDouble(NoOfDays) - 1;
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(Ref_NoofDays) > 0)
                                {

                                    string[] dtIssue = issueDt.Split('/');
                                    if (dtIssue.Length == 3)
                                        issueDt = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();

                                    DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(Ref_NoofDays) - 1).ToString();
                                    duesundate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(Ref_NoofDays) - 1).ToString();
                                    DateTime dt = Convert.ToDateTime(DueDate);
                                    Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                    //a.Caption = rs3(1) & "~" & rs3(2) & "~" & rs3(3) - 1
                                    dudate = Convert.ToDouble(Ref_NoofDays) - 1;
                                }
                                else
                                {
                                    string[] dtIssue = issueDt.Split('/');
                                    if (dtIssue.Length == 3)
                                        issueDt = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                    DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                    duesundate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                    DateTime dt = Convert.ToDateTime(DueDate);
                                    Txtduedate.Text = dt.ToString("dd/MM/yyyy");

                                    //a.Caption = rs3(1) & "~" & rs3(2) & "~" & rs3(3) - 1
                                    dudate = Convert.ToDouble(NoOfDays) - 1;
                                }
                            }

                            intIsHoliday = 1;
                            if (IntDueDatExcHol == 1)
                            {
                                if (intIsHoliday == 1)
                                {
                                    if (BlnLibHol == true)
                                    {
                                        Sql = "SELECT DISTINCT Holiday_Date FROM Holiday_Library WHERE Holiday_Date ='" + DueDate + "' AND Lib_Code ='" + Libcode + "' ";
                                    }
                                    else
                                    {
                                        Sql = "select distinct holiday_date from holidayStudents where holiday_date ='" + DueDate + "' ";
                                    }
                                    dsHoliday.Clear();
                                    dsHoliday = d2.select_method_wo_parameter(Sql, "text");
                                    if (dsHoliday.Tables[0].Rows.Count > 0)
                                    {
                                        string[] dtIssue = duesundate.Split('/');
                                        if (dtIssue.Length == 3)
                                            duesundate = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                        DueDate = Convert.ToDateTime(duesundate).AddDays(1).ToString();
                                        DateTime dt = Convert.ToDateTime(DueDate);
                                        Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                        duesundate = dt.ToString("dd/MM/yyyy");
                                        intIsHoliday = 1;
                                    }
                                    else
                                    {
                                        string DueDtCon = Txtduedate.Text;
                                        string[] dtdue = DueDtCon.Split('/');
                                        if (dtdue.Length == 3)
                                            DueDtCon = dtdue[1].ToString() + "/" + dtdue[0].ToString() + "/" + dtdue[2].ToString();
                                        DateTime day = Convert.ToDateTime(DueDtCon);
                                        if (day.DayOfWeek.ToString() == "Sunday")
                                        {
                                            string[] dtIssue = DueDate.Split('/');
                                            if (dtIssue.Length == 3)
                                                DueDate = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                            DueDate = Convert.ToDateTime(DueDate).AddDays(1).ToString();
                                            DateTime dt = Convert.ToDateTime(DueDate);
                                            Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                            duesundate = dt.ToString("dd/MM/yyyy");
                                            intIsHoliday = 1;
                                        }
                                        else
                                        {
                                            intIsHoliday = 0;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
            else
            {
                Sql = "SELECT no_of_days,no_of_token,fine,is_staff,isnull(Ref_NoofDays,0)Ref_NoofDays from lib_master where (code='" + StrSaveRollNo + "' or code ='" + StrSaveLibID + "') AND Is_Staff = 1 ";
                if (BlnBookBankLib == true && blncomm == true && BlnBookBankAll == false)
                    Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category' ";
                else if (BlnBookBankLib == true && BlnBookBankAll == true)
                    Sql += "and ISNULL(category,'All') ='Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                else
                    Sql += "and ISNULL(category,'All') ='All' AND ISNULL(StudCategory,'All') = 'All' ";

                if (Cbo_CardLibrary.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(TransLibCode,'All') ='" + Libcode + "' ";
                else
                    Sql += "AND ISNULL(TransLibCode,'All') ='All'";
                if (ddlBookType.SelectedItem.Text != "All")
                    Sql += "AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                else
                    Sql += "AND ISNULL(Book_Type,'All') ='All' ";
                if (cardCriteria != "All")
                    Sql += "AND ISNULL(CardCat,'All') ='" + cardCriteria + "' ";
                else
                    Sql += "AND ISNULL(CardCat,'All') ='All' ";
                dsprint.Clear();
                dsprint = d2.select_method_wo_parameter(Sql, "Text");

                if (dsprint.Tables[0].Rows.Count > 0)
                {
                    string Library_name = Convert.ToString(ddllibrary.SelectedValue);
                    string LibraryCode = d2.GetFunction("Select lib_code from library where lib_name='" + Library_name + "' and college_code='" + ColCode + "'");
                    Sql = "select ISBooks_DueDate,books_duedate from library where lib_code ='" + Library_name + "'";
                    rsLib.Clear();
                    rsLib = d2.select_method_wo_parameter(Sql, "Text");
                    if (rsLib.Tables[0].Rows.Count > 0)
                    {
                        string isBookDue = Convert.ToString(rsLib.Tables[0].Rows[0]["ISBooks_DueDate"]);
                        string NoOfDays = Convert.ToString(dsprint.Tables[0].Rows[0]["no_of_days"]);
                        string Ref_NoofDays = Convert.ToString(dsprint.Tables[0].Rows[0]["Ref_NoofDays"]);
                        string issueDt = txtissuedate.Text;
                        if (isBookDue == "true")
                        {
                            DueDate = Convert.ToString(rsLib.Tables[0].Rows[0]["books_duedate"]);
                            DateTime dt = Convert.ToDateTime(DueDate);
                            Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            ISRefBook(LibraryCode, Txtaccno.Text);
                            if (!ISReffBook)
                            {
                                string[] dtIssue = issueDt.Split('/');
                                if (dtIssue.Length == 3)
                                    issueDt = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                duesundate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                DateTime dt = Convert.ToDateTime(DueDate);
                                Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                //a.Caption = rs3(1) & "~" & rs3(2) & "~" & rs3(3) - 1
                                dudate = Convert.ToDouble(NoOfDays) - 1;
                            }
                            else
                            {
                                if (Convert.ToInt32(Ref_NoofDays) > 0)
                                {
                                    string[] dtIssue = issueDt.Split('/');
                                    if (dtIssue.Length == 3)
                                        issueDt = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                    DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(Ref_NoofDays) - 1).ToString();
                                    duesundate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(Ref_NoofDays) - 1).ToString();
                                    DateTime dt = Convert.ToDateTime(DueDate);
                                    Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                    //a.Caption = rs3(1) & "~" & rs3(2) & "~" & rs3(3) - 1
                                    dudate = Convert.ToDouble(Ref_NoofDays) - 1;
                                }
                                else
                                {
                                    string[] dtIssue = issueDt.Split('/');
                                    if (dtIssue.Length == 3)
                                        issueDt = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                    DueDate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                    duesundate = Convert.ToDateTime(issueDt).AddDays(Convert.ToInt32(NoOfDays) - 1).ToString();
                                    DateTime dt = Convert.ToDateTime(DueDate);
                                    Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                    //a.Caption = rs3(1) & "~" & rs3(2) & "~" & rs3(3) - 1
                                    dudate = Convert.ToDouble(NoOfDays) - 1;
                                }
                            }
                            if (IntDueDatExcHol == 1)
                            {
                                intIsHoliday = 1;
                                if (intIsHoliday == 1)
                                {
                                    if (BlnLibHol == true)
                                    {
                                        Sql = "SELECT DISTINCT Holiday_Date FROM Holiday_Library WHERE Holiday_Date ='" + DueDate + "' AND Lib_Code ='" + Library_name + "' ";
                                    }
                                    else
                                    {
                                        Sql = "select distinct holiday_date from holidaystaff where holiday_date ='" + DueDate + "'";
                                    }
                                    dsHoliday.Clear();
                                    dsHoliday = d2.select_method_wo_parameter(Sql, "text");
                                    if (dsHoliday.Tables[0].Rows.Count > 0)
                                    {
                                        string[] dtIssue = duesundate.Split('/');
                                        if (dtIssue.Length == 3)
                                            duesundate = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                        DueDate = Convert.ToDateTime(duesundate).AddDays(1).ToString();
                                        DateTime dt = Convert.ToDateTime(DueDate);
                                        Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                        duesundate = dt.ToString("dd/MM/yyyy");

                                        intIsHoliday = 1;
                                    }
                                    else
                                    {
                                        Txtduedate.Text = DueDate;
                                        string DueDtCon = Txtduedate.Text;
                                        string[] dtdue = DueDtCon.Split('/');
                                        if (dtdue.Length == 3)
                                            DueDtCon = dtdue[1].ToString() + "/" + dtdue[0].ToString() + "/" + dtdue[2].ToString();
                                        DateTime day = Convert.ToDateTime(DueDtCon);
                                        if (day.DayOfWeek.ToString() == "Sunday")
                                        {
                                            string[] dtIssue = duesundate.Split('/');
                                            if (dtIssue.Length == 3)
                                                duesundate = dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString();
                                            DueDate = Convert.ToDateTime(DueDate).AddDays(1).ToString();
                                            DateTime dt = Convert.ToDateTime(DueDate);
                                            Txtduedate.Text = dt.ToString("dd/MM/yyyy");
                                            duesundate = dt.ToString("dd/MM/yyyy");
                                            intIsHoliday = 1;
                                        }
                                        else
                                        {
                                            intIsHoliday = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collcode, "bookissue.aspx");
        }
    }

    #region Redirecting to another page

    protected void Btngo_Click(Object sender, EventArgs e)
    {
        string view = Convert.ToString(ddlview.SelectedItem.Text);
        if (view == "View Stack Status")
        {
            Response.Redirect("~/LibraryMod/Rack_Status_Monitor.aspx");
        }
        if (view == "Reservation")
        {
            Response.Redirect("~/LibraryMod/Book_Reservation.aspx");
        }
        if (view == "Transaction Report")
        {
            Response.Redirect("~/LibraryMod/TransactionReport.aspx");
        }
    }

    #endregion

    #region Fine Reason Popup

    protected void bindddlReason()
    {
        string collegeCode = Convert.ToString(ddlcollege.SelectedValue);
        DataTable dtexistingcardcatogery = dirAcc.selectDataTable("select TextVal,TextCode from TextValTable where TextCriteria='LFRes' and college_code='" + collegeCode + "'");
        if (dtexistingcardcatogery.Rows.Count > 0)
        {
            ddl_Reason.DataSource = dtexistingcardcatogery;
            ddl_Reason.DataTextField = "TextVal";
            ddl_Reason.DataValueField = "TextCode";
            ddl_Reason.DataBind();
            ddl_Reason.Items.Insert(0, "All");
        }
        //else
        //{
        //    ddl_CardCatogery.Items.Clear();
        //    ddl_CardCatogery.Items.Insert(0, "All");

        //}
    }

    protected void btnadd_Click(object sender, EventArgs e)
    {
        txt_FineCancelRea.Text = string.Empty;
        DivFineCnlRea.Visible = true;
        DivFineReason.Visible = true;
    }

    protected void btndel_Click(object sender, EventArgs e)
    {
        if (ddl_Reason.Items.Count > 0 && ddl_Reason.SelectedValue != "0")
        {
            string collegeCode = Convert.ToString(ddlcollege.SelectedValue);
            string categtodel = ddl_Reason.SelectedValue;
            string delqry = "delete from TextValTable where TextCode='" + categtodel + "' and college_code='" + collegeCode + "'";
            dirAcc.deleteData(delqry);
            bindddlReason();
        }
    }

    //protected void btnPopAlertClose_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblAlertMsgNEW.Text = string.Empty;
    //        imgdiv2.Visible = false;
    //    }
    //    catch (Exception ex)
    //    {
    //        d2.sendErrorMail(ex, userCollegeCode, "individualstudent");
    //    }
    //}

    protected void btn_FineReasonSave_Click(object sender, EventArgs e)
    {
        string Reason = txt_FineCancelRea.Text.Trim();
        string collegeCode = Convert.ToString(ddlcollege.SelectedValue);
        if (!string.IsNullOrEmpty(Reason))
        {
            DataTable dtexistingcardcatogery = dirAcc.selectDataTable("select TextVal from TextValTable where TextCriteria='LFRes' and college_code='" + collegeCode + "'");
            if (dtexistingcardcatogery.Rows.Count > 0)
            {
                List<string> lstexistCat = dtexistingcardcatogery.AsEnumerable().Select(r => r.Field<string>("TextVal")).ToList();
                if (lstexistCat.Contains(Reason))
                {
                    lblErrNewCardCatoger.Visible = true;
                    lblErrNewCardCatoger.Text = "Reason Already Exists";
                    return;
                }
            }
            string insertqry = "insert into TextValTable (TextVal,TextCriteria,college_code) values('" + Reason + "','LFRes','" + collegeCode + "')";
            if (dirAcc.insertData(insertqry) > 0)
            {
                bindddlReason();
                DivFineCnlRea.Visible = false;
                DivFineReason.Visible = false;
                txt_FineCancelRea.Text = string.Empty;
            }
        }
    }

    protected void btn_FineReasonExit_Click(object sender, EventArgs e)
    {
        DivFineCnlRea.Visible = false;
        DivFineReason.Visible = false;
        txt_FineCancelRea.Text = string.Empty;
    }

    #endregion

    protected void BtnLblNocard_Click(Object sender, EventArgs e)
    {
        DivNocard.Visible = false;
    }

    protected void ClearFunction()
    {
        degree_codeVar = "";
        StrSaveRollNo = "";
        StrSaveLibID = "";
        batch_year = "";
        StrAccNo = "";
        hsAccNo.Clear();
        lbl_issue.Text = "Issue Date";
        lbl_due.Visible = true;
        Txtduedate.Visible = true;
        lblIssSpreadName.Text = "Issuing Books";
        lblIssSpreadName.ForeColor = Color.Green;
        lblIssSpreadName.Font.Bold = true;

        ClearFineDetails();
        RblMemType.SelectedIndex = 0;
        rblissue.SelectedIndex = 0;
        Txtaccno.Text = "";
        txtRollNo.Text = "";
        TxtName.Text = "";
        ddlcodenumber.Items.Clear();
        txtDept.Text = "";
        txt_elgi.Text = "";
        txt_issued.Text = "";
        txt_Unlocked.Text = "";
        txtlocked.Text = "";
        img_stud1.ImageUrl = "";
        imgBook.ImageUrl = "";
        ddlcollege.SelectedIndex = 0;
        Cbo_CardLibrary.SelectedIndex = 0;
        ddllibrary.SelectedIndex = 0;
        ddlBookType.SelectedIndex = 0;
        ddlCardType.SelectedIndex = 0;
        string serverDt = d2.ServerDate();
        string[] dat = serverDt.Split('/');
        if (dat.Length == 3)
            serverDt = dat[1] + '/' + dat[0] + '/' + dat[2];
        txtissuedate.Text = serverDt;
        Txtduedate.Text = serverDt;
        int count = Cbo_CardLibrary.Items.Count;
        Cbo_CardLibrary.SelectedIndex = count - 1;
        img_stud1.ImageUrl = "";
        Txtaccno.Text = "";
    }

    protected void txtaccnumber_OnTextChanged(Object sender, EventArgs e)
    {
        try
        {
            string StrTransDept = string.Empty;
            string accNo = Convert.ToString(txtaccnumber.Text.Trim());
            string libcode = Convert.ToString(ddllibrary.SelectedValue);
            string roll_no = "";
            string stud_name = "";
            string Sql = "SELECT B.Acc_No,Title,Author,Edition,Price,Dept_Code,Book_Status,Publisher,Bill_No,ISNULL(TypeofBook,'') TypeofBook,ISNULL(Rack_No,'') Rack_No,ISNULL(Row_No,'') Row_No,Isnull(Pos_No,'')+' - '+isnull(pos_place,'') Position FROM Bookdetails B LEFT JOIN Rack_Allocation R ON R.Acc_No = B.Acc_No AND R.Lib_Code = B.Lib_Code WHERE B.acc_no  ='" + accNo + "' and B.lib_code ='" + libcode + "'";
            DataSet dsAccNo = new DataSet();
            dsAccNo = d2.select_method_wo_parameter(Sql, "text");
            if (dsAccNo.Tables[0].Rows.Count > 0)
            {
                string AccessNo = Convert.ToString(dsAccNo.Tables[0].Rows[0]["acc_no"]);
                LblAccDet.Text = AccessNo;
                string Title = Convert.ToString(dsAccNo.Tables[0].Rows[0]["title"]);
                LblAccTitle.Text = Title;
                string Author = Convert.ToString(dsAccNo.Tables[0].Rows[0]["author"]);
                LblAccAuthor.Text = Author;
                string Edition = Convert.ToString(dsAccNo.Tables[0].Rows[0]["edition"]);
                LblAccEdition.Text = Edition;
                string Price = Convert.ToString(dsAccNo.Tables[0].Rows[0]["price"]);
                LblAccPrice.Text = Price;
                string DeptCode = Convert.ToString(dsAccNo.Tables[0].Rows[0]["dept_code"]);
                LblAccDept.Text = DeptCode;
                string BookStatus = Convert.ToString(dsAccNo.Tables[0].Rows[0]["Book_Status"]);
                if (BookStatus == "Transfered")
                {
                    string StrAccTransDept = d2.GetFunction("SELECT To_Lib_Code FROM Book_Transfer WHERE Acc_No ='" + accNo + "' AND from_lib_code = '" + libcode + "' and transfer_type = 2 ");
                    if (StrAccTransDept == "")
                        LblAccStatus.Text = BookStatus;
                    else
                        LblAccStatus.Text = BookStatus + "-" + StrAccTransDept;
                }
                else
                    LblAccStatus.Text = BookStatus;
                string publisher = Convert.ToString(dsAccNo.Tables[0].Rows[0]["publisher"]);
                LblAccPub.Text = publisher;
                string bill_no = Convert.ToString(dsAccNo.Tables[0].Rows[0]["bill_no"]);
                LblAccBill.Text = bill_no;
                string typeofbook = Convert.ToString(dsAccNo.Tables[0].Rows[0]["typeofbook"]);
                LblAccBkType.Text = typeofbook;
                string Rack_No = Convert.ToString(dsAccNo.Tables[0].Rows[0]["Rack_No"]);
                string Row_No = Convert.ToString(dsAccNo.Tables[0].Rows[0]["Row_No"]);
                string Position = Convert.ToString(dsAccNo.Tables[0].Rows[0]["Position"]);
                LblAccShelf.Text = Rack_No + "-" + Row_No + "-" + Position;
                if (BookStatus == "Issued")
                {
                    Sql = "select stud_name,roll_no from borrow,bookdetails where borrow.acc_no=bookdetails.acc_no and borrow.acc_no='" + accNo + "' and return_flag=0";
                    DataSet dsRollStu = new DataSet();
                    dsRollStu = d2.select_method_wo_parameter(Sql, "text");
                    if (dsRollStu.Tables[0].Rows.Count > 0)
                    {
                        roll_no = Convert.ToString(dsRollStu.Tables[0].Rows[0]["roll_no"]);
                        LblAccRollNo.Text = roll_no;
                        stud_name = Convert.ToString(dsRollStu.Tables[0].Rows[0]["stud_name"]);
                        LblAccStuName.Text = stud_name;
                    }
                }
                else
                {
                }
                DivAccessBookDet.Visible = true;
                DivAcessBkDet.Visible = true;
            }
            else
            {
                DivAccessBookDet.Visible = false;
                DivAcessBkDet.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void ImgBtnAccessBookDet_Click(Object sender, EventArgs e)
    {
        DivAccessBookDet.Visible = false;
        DivAcessBkDet.Visible = false;
    }

    protected void btn_errorMsgclose_Click(object sender, EventArgs e)
    {
        DivErrorMsg.Visible = false;
    }

    #region ReserVed Book Issuing PopUp

    protected void btnReservedbkYes_Click(object sender, EventArgs e)
    {
        DivReservedbk.Visible = false;
    }

    protected void btnReservedbkNo_Click(object sender, EventArgs e)
    {
        DivReservedbk.Visible = false;
        Remove = true;
        BtnRemove_Click(sender, e);
        Txtaccno.Text = "";
    }
    #endregion

    public void getLibPrivil()
    {
        try
        {
            Hashtable hsLibcode = new Hashtable();
            string libcodecollection = "";
            string coll_Code = Convert.ToString(ddlcollege.SelectedValue);
            string sql = "";
            string GrpUserVal = "";
            string GrpCode = "";
            string LibCollection = "";
            if (singleUser.ToLower() == "true")
            {
                sql = "SELECT DISTINCT lib_code from lib_privileges where user_code=" + userCode + " and lib_code in (select lib_code from library where college_code=" + coll_Code + ")";
                ds.Clear();
                ds = d2.select_method_wo_parameter(sql, "text");
            }
            else
            {
                string[] groupUser = groupUserCode.Split(';');
                if (groupUser.Length > 0)
                {
                    if (groupUser.Length == 1)
                    {
                        sql = "SELECT DISTINCT lib_code from lib_privileges where group_code=" + groupUser[0] + "";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(sql, "text");
                    }
                    if (groupUser.Length > 1)
                    {
                        for (int i = 0; i < groupUser.Length; i++)
                        {
                            GrpUserVal = groupUser[i];
                            if (!GrpCode.Contains(GrpUserVal))
                            {
                                if (GrpCode == "")
                                    GrpCode = GrpUserVal;
                                else
                                    GrpCode = GrpCode + "','" + GrpUserVal;
                            }
                        }
                        sql = "SELECT DISTINCT lib_code from lib_privileges where group_code in ('" + GrpCode + "')";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(sql, "text");
                    }
                }

            }
            if (ds.Tables[0].Rows.Count == 0)
            {
                libcodecollection = "WHERE lib_code IN (-1)";
                goto aa;
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string codeCollection = Convert.ToString(ds.Tables[0].Rows[i]["lib_code"]);
                    if (!hsLibcode.Contains(codeCollection))
                    {
                        hsLibcode.Add(codeCollection, "LibCode");
                        if (libcodecollection == "")
                            libcodecollection = codeCollection;
                        else
                            libcodecollection = libcodecollection + "','" + codeCollection;
                    }
                }
            }
            //libcodecollection = Left(libcodecollection, Len(libcodecollection) - 1);
            libcodecollection = "WHERE lib_code IN ('" + libcodecollection + "')";
        aa:
            LibCollection = libcodecollection;
            bindLibrary(LibCollection);
            LibNameDefault = LibCollection;
        }
        catch (Exception ex)
        {
        }
    }

    protected void BtnError_AccessNoLookup_Click(object sender, EventArgs e)
    {
        AccessNoLookup.Visible = false;

    }

    protected void BtnClear_Click(object sender, EventArgs e)
    {
        degree_codeVar = "";
        StrSaveRollNo = "";
        StrSaveLibID = "";
        batch_year = "";
        StrAccNo = "";
        hsAccNo.Clear();
        if (ddlissue.SelectedItem.Text == "Book")
        {
            AccBookType = 0;
        }
        Page.Form.DefaultFocus = txtRollNo.ClientID;
        firstRow = false;
        Bindcollege();
        getLibPrivil();
        bindCategory();
        bindbatch();
        binddegree();
        bindbranch();
        bindsem();
        bindsec();
        SetLibSettings();
        ddllibrary_SelectedIndexChanged(sender, e);
        string serverDt = d2.ServerDate();
        string[] dat = serverDt.Split('/');
        if (dat.Length == 3)
            serverDt = dat[1] + '/' + dat[0] + '/' + dat[2];
        txtissuedate.Text = serverDt;
        Txtduedate.Text = serverDt;
        //string pass = string.Empty;
        //txtRollNo.Attributes.Add("value", pass);
        txtRollNo.Text = "";
        TxtName.Text = "";
        txtDept.Text = "";
        Txtaccno.Text = "";
        txtaccnumber.Text = "";
        ddlcodenumber.Items.Clear();
        txt_elgi.Text = "0";
        txt_issued.Text = "0";
        txt_Unlocked.Text = "0";
        txtlocked.Text = "0";
        ddlissue.SelectedIndex = 0;
        ddlissue.Enabled = true;
        RblMemType.SelectedIndex = 0;
        img_stud1.ImageUrl = "";
        LostAndFineDiv.Visible = false;
        rblissue.SelectedIndex = 0;
        if (GrdIssuingBook.Rows.Count > 0)
        {
            dtIssuingBook.Clear();
            dtIssuingBook.Columns.Add("Access No", typeof(string));
            dtIssuingBook.Columns.Add("Title", typeof(string));
            dtIssuingBook.Columns.Add("Author", typeof(string));
            dtIssuingBook.Columns.Add("Call No", typeof(string));
            dtIssuingBook.Columns.Add("Date Of Issue", typeof(string));
            dtIssuingBook.Columns.Add("Due Days", typeof(string));
            dtIssuingBook.Columns.Add("Due Date", typeof(string));
            dtIssuingBook.Columns.Add("Token No", typeof(string));
            dtIssuingBook.Columns.Add("Fine", typeof(string));
            GrdIssuingBook.DataSource = dtIssuingBook;
            GrdIssuingBook.DataBind();
            GrdIssuingBook.Visible = true;
        }
        ViewState["CurrentTable"] = null;
        if (GrdBookInHand.Rows.Count > 0)
        {
            for (int BkInHandRowCnt = 0; BkInHandRowCnt < GrdBookInHand.Rows.Count; BkInHandRowCnt++)
            {
                GrdBookInHand.Rows[BkInHandRowCnt].Visible = false;
            }
        }
        if (grdReservation.Rows.Count > 0)
        {
            for (int ReservationRowCnt = 0; ReservationRowCnt < grdReservation.Rows.Count; ReservationRowCnt++)
            {
                grdReservation.Rows[ReservationRowCnt].Visible = false;
            }
        }
    }

    protected void imagebtnReservedPopup_Click(object sender, EventArgs e)
    {
        ReservedPopup.Visible = false;
    }

    protected void imagebtnReservepopclose1_Click(object sender, EventArgs e)
    {
        DivBookReservation.Visible = false;
    }

    protected void BtnReser_Click(object sender, EventArgs e)
    {
        DivBookReservation.Visible = true;
    }

    protected void ddlCardType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txtRollNo_Change(sender, e);
    }

    protected void txtissuedate_OnTextChanged(object sender, EventArgs e)
    {
        string issueddate = txtissuedate.Text;
        string[] dtIssue = issueddate.Split('/');
        DateTime DtIssuedate = Convert.ToDateTime(dtIssue[1].ToString() + "/" + dtIssue[0].ToString() + "/" + dtIssue[2].ToString());
        string CurserverDate = d2.ServerDate();
        DateTime DtDuedate = Convert.ToDateTime(CurserverDate);
        if (DtIssuedate > DtDuedate)
        {
            string[] dat = CurserverDate.Split('/');
            if (dat.Length == 3)
                CurserverDate = dat[1] + '/' + dat[0] + '/' + dat[2];
            txtissuedate.Text = CurserverDate;
        }
        if (rblissue.SelectedIndex != 3)
        {
            SetDueDate(sender, e);
        }
    }

    protected void BtnHistory_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable bookdet = new DataTable();
            int sno = 0;
            DataRow dr1;
            colour.Visible = true;
            string datebok = string.Empty;
            string typebok = string.Empty;
            string bokname = string.Empty;
            string bokaccno = string.Empty;
            string modebok = string.Empty;
            popwindowdetails.Visible = true;

            string editQ = "SELECT distinct bk.Acc_No,bk.Title,return_flag,isnull(br.mode,'0') as mode,borrow_date,CONVERT(varchar(20),return_date,103)return_date FROM bookdetails bk,borrow br,Registration r WHERE  br.roll_no ='" + Convert.ToString(txtRollNo.Text) + "' and br.acc_no=bk.Acc_No and r.Roll_No=br.roll_no order by borrow_date desc";
            DataSet edit = new DataSet();
            edit = d2.select_method_wo_parameter(editQ, "Text");
            colour.Visible = true;
            bookdet.Columns.Add("S.No", typeof(string));
            bookdet.Columns.Add("Acc No", typeof(string));
            bookdet.Columns.Add("Book Name", typeof(string));
            bookdet.Columns.Add("Issue Date", typeof(string));
            bookdet.Columns.Add("Return Date", typeof(string));
            bookdet.Columns.Add("Type", typeof(string));
            bookdet.Columns.Add("Fine Amount", typeof(string));
            bookdet.Columns.Add("Status", typeof(string));
            int rowCnt = 0;
            dicRowColor.Clear();

            if (edit.Tables[0].Rows.Count > 0)
            {
                for (int k = 0; k < edit.Tables[0].Rows.Count; k++)
                {
                    double TotFinAmt = 0;
                    sno++;
                    dr1 = bookdet.NewRow();
                    dr1["S.No"] = Convert.ToString(sno);
                    bokaccno = Convert.ToString(edit.Tables[0].Rows[k]["Acc_No"]).Trim();
                    dr1["Acc No"] = bokaccno;
                    bokname = Convert.ToString(edit.Tables[0].Rows[k]["Title"]).Trim();
                    dr1["Book Name"] = bokname;
                    editQ = "select fineamt,case when ((fineamt - paidamt) =0 )then 'Paid' else 'UnPaid/Returned' end as 'Status' from Fine_Details where roll_no='" + Convert.ToString(txtRollNo.Text) + "' and acc_no='" + bokaccno + "' and iss_date='" + Convert.ToString(edit.Tables[0].Rows[k]["borrow_date"]) + "' group by fineamt,paidamt";
                    DataSet dsFine = new DataSet();
                    dsFine = d2.select_method_wo_parameter(editQ, "text");
                    if (dsFine.Tables[0].Rows.Count > 0)
                    {
                        for (int fin = 0; fin < dsFine.Tables[0].Rows.Count; fin++)
                        {
                            double fineamt = Convert.ToDouble(dsFine.Tables[0].Rows[fin]["fineamt"]);
                            TotFinAmt = TotFinAmt + fineamt;
                        }
                        dr1["Fine Amount"] = TotFinAmt;
                        dr1["Status"] = Convert.ToString(dsFine.Tables[0].Rows[0]["Status"]);
                    }
                    else
                    {
                        dr1["Fine Amount"] = "0";
                        dr1["Status"] = "";
                    }
                    datebok = Convert.ToString(edit.Tables[0].Rows[k]["borrow_date"]).Trim();
                    string[] dat = datebok.Split('/');
                    if (dat.Length == 3)
                        datebok = dat[1] + '/' + dat[0] + '/' + dat[2];
                    dr1["Issue Date"] = datebok.Split(' ')[0];

                    typebok = Convert.ToString(edit.Tables[0].Rows[k]["return_flag"]).Trim();
                    modebok = Convert.ToString(edit.Tables[0].Rows[k]["mode"]).Trim();
                    if (typebok == "1" && modebok.ToLower() == "false")
                    {
                        dr1["Type"] = "Returned";
                        datebok = Convert.ToString(edit.Tables[0].Rows[k]["return_date"]).Trim();
                        dr1["Return Date"] = datebok;
                        dicRowColor.Add(rowCnt, "Returned");
                    }
                    else
                    {
                        dr1["Return Date"] = "";
                    }
                    if (typebok == "0" && modebok.ToLower() == "false")
                    {
                        dr1["Type"] = "Issued";
                        dicRowColor.Add(rowCnt, "Issued");
                    }
                    if (typebok == "1" && modebok.ToLower() == "true")
                    {
                        dr1["Type"] = "Lost";
                        datebok = Convert.ToString(edit.Tables[0].Rows[k]["return_date"]).Trim();
                        dr1["Return Date"] = datebok;
                        dicRowColor.Add(rowCnt, "Lost");
                    }
                    bookdet.Rows.Add(dr1);
                    rowCnt++;
                }
            }
            popwindowdetails.Visible = true;
            grdHistory.DataSource = bookdet;
            grdHistory.DataBind();
            grdHistory.Visible = true;

            foreach (KeyValuePair<int, string> dr in dicRowColor)
            {
                int RowVal = dr.Key;
                string DicValue = dr.Value;
                if (DicValue == "Returned")
                {
                    grdHistory.Rows[RowVal].BackColor = Color.LightYellow;
                }
                if (DicValue == "Issued")
                {
                    grdHistory.Rows[RowVal].BackColor = Color.LightGreen;
                }
                if (DicValue == "Lost")
                {
                    grdHistory.Rows[RowVal].BackColor = Color.Red;
                }
            }
        }
        catch (Exception ex) { }
    }

    protected void grdHistory_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
        }

    }

    protected void imagebtnpop2close_Click(object sender, EventArgs e)
    {
        popwindowdetails.Visible = false;
    }
}
