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

public partial class LibraryMod_TransactionReport : System.Web.UI.Page
{
    #region Field Declaration

    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet dsload = new DataSet();
    DAccess2 dacces2 = new DAccess2();
    DataTable dtCommon = new DataTable();
    Hashtable ht = new Hashtable();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    ReuasableMethods rs = new ReuasableMethods();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    string collegeCode = string.Empty;
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string usercode = "", singleuser = "", group_user = "";
    string collegecode = "";
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    string qryCollege = string.Empty;
    string qry = string.Empty;
    string dept = string.Empty;
    string library = string.Empty;
    string selQ = string.Empty;
    string batch = string.Empty;
    string sqlcmd = "";
    string collegeName = string.Empty;
    string select = string.Empty;
    string reporttype = string.Empty;
    string qrybatchfilter = string.Empty;
    string qrylibraryFilter = string.Empty;
    string qryreporttypeFilter = string.Empty;
    string qryselectforfilter = string.Empty;
    string qrytxtrefbooksFilter = string.Empty;
    string qrydeptfilter = string.Empty;
    string libcode = string.Empty;
    string strID = string.Empty;
    string strStaffID = string.Empty;
    int loop = 0;
    DataTable trre = new DataTable();
    Boolean fpcellclick = false;
    FarPoint.Web.Spread.TextCellType cellText = new FarPoint.Web.Spread.TextCellType();
    DataTable transrepo = new DataTable();
    //Pageno Added by rajasekar 20/06/2018
    Boolean pageno = false;
    int ivalue = 0;
    int curpage = 0;
    double pageSize1 = 0.0;
    int pagecnt = 0;
    int pgsize = 0;
    int selectedpage = 0;
    static int first = 0;


    //***********//

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
                Bindcollege();
                getLibPrivil();
                Binddept();
                BindBatch();
                Bindreporttype();
                selectfor();
                Bindtype();
                Bindaccno();
                lostbooks();
                Bindsearchby();
                loadsupplier();
                loadinward();
                loadstatus();
                loadremarks();
                loadsubject();
                loaddepttype();
                // Studentinfo();
                bindBranch();
                bindBatch1();
                bindcourse();
                isLibraryID();
                loadstatus1();
                ddlheader.Items.Clear();
                ddlheader.Items.Add("---Select---");
                ddlheader.Items.Add("Roll No");
                ddlheader.Items.Add("Reg No");
                ddlheader.Items.Add("Name");
                ddloperator.Items.Clear();
                ddloperator.Items.Add("---Select---");
                ddloperator.Items.Add("Like");
                ddloperator.Items.Add("Starts With");
                ddloperator.Items.Add("Ends With");
                string strID = string.Empty;
                string strStaffID = string.Empty;
                txt_fromdate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }

    }
    #endregion page load

    #region college

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


                ddlcollegenew.DataSource = dtCommon;
                ddlcollegenew.DataTextField = "collname";
                ddlcollegenew.DataValueField = "college_code";
                ddlcollegenew.DataBind();
                //ddlcollegenew_SelectedIndexChanged(sender, e);
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    #endregion

    #region reporttype

    public void Bindreporttype()
    {
        try
        {
            ddlreporttype.Items.Add("Issued Books");
            ddlreporttype.Items.Add("Returned Books");
            ddlreporttype.Items.Add("Renewal Books");
            ddlreporttype.Items.Add("Lost Books");
            ddlreporttype.Items.Add("Due Books");
            ddlreporttype.Items.Add("Back Volume Details");
            ddlreporttype.Items.Add("Binding Details");
            ddlreporttype.Items.Add("Book Borrow Utility");
            ddlreporttype.Items.Add("Access Number Report");
            ddlreporttype.Items.Add("Card Locked Report");
            ddlreporttype.Items.Add("Card Holders Information");
            ddlreporttype.Items.Add("Issued Books(Non Member)");
            ddlreporttype.Items.Add("Missing Books");
            ddlreporttype.Items.Add("User Entry Status");
            ddlreporttype.Items.Add("News Paper List");
            ddlreporttype.Items.Add("Consolidated Book List Year Wise");
            ddlreporttype.Items.Add("Books in Issues");
            ddlreporttype.Items.Add("OPAC Hit Status Report");
            ddlreporttype.Items.Add("Book Purchase Report");
            ddlreporttype.Items.Add("Book Details");
            ddlreporttype.Items.Add("Departmentwise Abstract");
            ddlreporttype.Items.Add("Library contents");
            ddlreporttype.Items.Add("Individual  Library Usage");
            ddlreporttype.Items.Add("Returned Books Cum Reserved");
            ddlreporttype.Items.Add("Reservation Report");
            ddlreporttype.Items.Add("OverDue Books");
            ddlreporttype.Items.Add("Rack Information");
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    #endregion

    #region Library

    public void BindLibrary(string Libcodecol)
    {
        try
        {
            ddllibrary.Items.Clear();
            ds.Clear();
            string College = ddlCollege.SelectedValue.ToString();
            string SelectQ = string.Empty;
            if (!string.IsNullOrEmpty(College))
            {
                dicQueryParameter.Clear();
                SelectQ = "select *,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + Libcodecol + " and college_code in('" + College + "') ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = d2.select_method_wo_parameter(SelectQ, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddllibrary.DataSource = ds;
                    ddllibrary.DataTextField = "lib_name";
                    ddllibrary.DataValueField = "lib_code";
                    ddllibrary.DataBind();
                    ddllibrary.Items.Insert(0, "All");
                    ddlrackno.Items.Insert(0, "All");


                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    #endregion

    public void loadsupplier()
    {
        try
        {
            string selectQuery = "select distinct supplier from bookdetails order by supplier";
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsupplier.DataSource = ds;
                ddlsupplier.DataTextField = "supplier";
                ddlsupplier.DataValueField = "supplier";
                ddlsupplier.DataBind();
                ddlsupplier.Items.Insert(0, "All");


            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    public void loadinward()
    {
        try
        {
            string selectQuery = "SELECT DISTINCT ISNULL(Pur_Don,'') Pur_Don FROM BookDetails B WHERE ISNULL(Pur_Don,'') <> ''";
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlinwardtype.DataSource = ds;
                ddlinwardtype.DataTextField = "Pur_Don";
                ddlinwardtype.DataValueField = "Pur_Don";
                ddlinwardtype.DataBind();
                ddlinwardtype.Items.Insert(0, "All");


            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    public void isLibraryID()
    {
        try
        {
            string selectQuery = "select * from inssettings where linkname ='Library id'";
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["linkvalue"].ToString() == "0")
                {
                    Session["strID"] = "registration.roll_no";
                    Session["strStaffID"] = "staffmaster.staff_code";
                }
                else
                {
                    Session["strID"] = "registration.lib_id";
                    Session["strStaffID"] = "staffmaster.lib_id";
                }
            }
            else
            {
                Session["strID"] = "registration.roll_no";
                Session["strStaffID"] = "staffmaster.staff_code";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    public void loadstatus()
    {
        try
        {
            string selectQuery = "select distinct book_status from bookdetails where book_status<>'' order by book_status";
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlstatus.DataSource = ds;
                ddlstatus.DataTextField = "book_status";
                ddlstatus.DataValueField = "book_status";
                ddlstatus.DataBind();
                ddlstatus.Items.Insert(0, "All");


            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    public void loadremarks()
    {
        try
        {
            string selectQuery = "SELECT DISTINCT ISNULL(Remark,'') Remarks FROM BookDetails";
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlremarks.DataSource = ds;
                ddlremarks.DataTextField = "Remarks";
                ddlremarks.DataValueField = "Remarks";
                ddlremarks.DataBind();
                ddlremarks.Items.Insert(0, "All");


            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    public void loadsubject()
    {
        try
        {
            string selectQuery = "select distinct(subject) from bookdetails";
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsubject.DataSource = ds;
                ddlsubject.DataTextField = "subject";
                ddlsubject.DataValueField = "subject";
                ddlsubject.DataBind();
                ddlsubject.Items.Insert(0, "All");


            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    public void loadstatus1()
    {
        try
        {
            ddlstatus1.Items.Add("All");
            ddlstatus1.Items.Add("Reserved");
            ddlstatus1.Items.Add("Cancelled");
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    public void loaddepttype()
    {
        try
        {
            ddldepttype.Items.Add("All");
            ddldepttype.Items.Add("Reference Books");
            ddldepttype.Items.Add("Text Books");
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    protected void enqbtn_Click(object sender, EventArgs e)
    {


        Panellookup1.Visible = true;


    }

    protected void btncloselook1_Click(object sender, EventArgs e)
    {
        Panellookup1.Visible = false;
    }

    protected void ddlcollegenew_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loaddetails();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    public void loaddetails()
    {
        try
        {
            bindBatch1();
            bindcourse();

            if (ddlDegree.Items.Count > 0)
            {
                bindBranch();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    public void bindBatch1()
    {
        try
        {
            string qry = " select distinct Batch_Year from Registration order by batch_year desc";
            DataTable dtbatchyr = dirAcc.selectDataTable(qry);
            ddlbatch11.Items.Clear();
            if (dtbatchyr.Rows.Count > 0)
            {
                ddlbatch11.DataSource = dtbatchyr;
                ddlbatch11.DataTextField = "Batch_Year";
                ddlbatch11.DataValueField = "Batch_Year";
                ddlbatch11.DataBind();


            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    public void bindcourse()
    {
        try
        {
            string usercode = Session["usercode"].ToString();
            DAccess2 da1 = new DAccess2();
            DataSet ds1 = new DataSet();
            ht.Clear();
            string strisstaff = Session["Staff_Code"].ToString();
            ddlDegree.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ht.Clear();
            ht.Add("single_user", singleuser);
            ht.Add("group_code", group_user);
            if (strisstaff.ToLower().Trim() == "")
            {
                ht.Add("college_code", ddlcollegenew.SelectedValue.ToString());
            }
            else
            {
                ht.Add("college_code", ddlcollegenew.SelectedValue.ToString());
            }
            ht.Add("user_code", usercode);
            ds1 = da1.select_method("bind_degree", ht, "sp");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ddlDegree.Enabled = true;
                ddlDegree.Items.Clear();
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    ddlDegree.Items.Insert(i, new System.Web.UI.WebControls.ListItem(Convert.ToString(ds1.Tables[0].Rows[i]["course_name"]), Convert.ToString(ds1.Tables[0].Rows[i]["course_id"])));
                }
            }
            else
            {
                ddlDegree.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    public void bindBranch()
    {
        try
        {
            DAccess2 da1 = new DAccess2();
            DataSet ds1 = new DataSet();
            string strisstaff = Session["Staff_Code"].ToString();
            ddlBranch1.Items.Clear();
            ht.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ht.Add("single_user", singleuser);
            ht.Add("group_code", group_user);

            ht.Add("course_id", ddlDegree.SelectedValue);
            if (strisstaff.ToLower().Trim() == "")
            {
                ht.Add("college_code", ddlcollegenew.SelectedValue.ToString());
            }
            else
            {
                ht.Add("college_code", ddlcollegenew.SelectedValue.ToString());
            }
            ht.Add("user_code", usercode);
            ds1 = da1.select_method("bind_branch", ht, "sp");
            if (ds1.Tables.Count > 0)
            {
                ddlBranch1.DataSource = ds1;
                ddlBranch1.DataTextField = "Acronym";
                ddlBranch1.DataValueField = "degree_code";
                ddlBranch1.DataBind();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    protected void ddlbatch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //bindtype();
            bindcourse();
            bindBranch();
            // bindSem();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindBranch();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    protected void ddlBranch1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnlookupgo1_Click(object sender, EventArgs e)
    {
        try
        {

            StudentLookup1();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }

    }

    public void StudentLookup1()
    {
        try
        {
            DataRow dr1;
            string serach_Crita = "";
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            // ddlstatus.Enabled = true;
            gridview1.Visible = false;
            //FpSpread1.Sheets[0].RowCount = 0;
            if (ddlbatch11.Items.Count > 0)
            {
                if (ddlDegree.Items.Count > 0)
                {
                    if (ddlBranch1.Items.Count > 0)
                    {
                        if (ddlheader.SelectedIndex == 1)
                        {
                            if (ddloperator.SelectedIndex == 1)
                            {
                                serach_Crita = " and Roll_No like '%" + tbvalue.Text.Trim() + "%' ";
                            }
                            else if (ddloperator.SelectedIndex == 2)
                            {
                                serach_Crita = " and Roll_No like '" + tbvalue.Text.Trim() + "%' ";

                            }
                            else if (ddloperator.SelectedIndex == 3)
                            {
                                serach_Crita = " and Roll_No like '%" + tbvalue.Text.Trim() + "' ";
                            }
                        }
                        if (ddlheader.SelectedIndex == 2)
                        {
                            if (ddloperator.SelectedIndex == 1)
                            {
                                serach_Crita = " and Reg_No like '%" + tbvalue.Text.Trim() + "%' ";

                            }
                            else if (ddloperator.SelectedIndex == 2)
                            {
                                serach_Crita = " and Reg_No like '" + tbvalue.Text.Trim() + "%' ";

                            }
                            else if (ddloperator.SelectedIndex == 3)
                            {
                                serach_Crita = " and Reg_No like '%" + tbvalue.Text.Trim() + "' ";
                            }

                        }

                        if (ddlheader.SelectedIndex == 3)
                        {
                            if (ddloperator.SelectedIndex == 1)
                            {
                                serach_Crita = " and Stud_Name like '%" + tbvalue.Text.Trim() + "%' ";

                            }
                            else if (ddloperator.SelectedIndex == 2)
                            {
                                serach_Crita = " and Stud_Name like '" + tbvalue.Text.Trim() + "%' ";
                            }
                            else if (ddloperator.SelectedIndex == 3)
                            {
                                serach_Crita = " and Stud_Name like '%" + tbvalue.Text.Trim() + "' ";
                            }
                        }


                        string Branch_Code;
                        //string Degree;
                        Branch_Code = ddlBranch1.SelectedValue.ToString(); //GetFunction("select degree_Code from degree where acronym = '" + ddlBranch1.SelectedItem.Text.ToString() + "'");


                        sqlcmd = "select distinct Roll_No,Stud_Name,degree_code,Reg_No ,app_no,college_code from  registration where degree_code='" + ddlBranch1.SelectedValue.ToString() + "' and college_code = '" + ddlcollegenew.SelectedValue.ToString() + "' and batch_year='" + ddlbatch11.SelectedValue.ToString() + "' " + serach_Crita + " and cc=0 and exam_flag<>'debar' and delflag=0 and ((isnull(Bus_RouteID,'')='' and isnull(Boarding,'')='' and isnull(VehID,'')='') or iscanceledstage='1')";
                        //and (Bus_RouteID is null Or Boarding is null Or VehID is null or Bus_RouteID='' or Boarding='' or VehID='')";

                        dsload = dacces2.select_method_wo_parameter(sqlcmd, "Text");
                        if (dsload.Tables[0].Rows.Count > 0)
                        {
                            trre.Columns.Add("Dept", typeof(string));
                            trre.Columns.Add("Roll No", typeof(string));
                            trre.Columns.Add("App no", typeof(string));
                            trre.Columns.Add("Reg No", typeof(string));
                            trre.Columns.Add("Student Name", typeof(string));
                            trre.Columns.Add("college code", typeof(string));

                            for (loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                            {
                                dr1 = trre.NewRow();
                                dr1["Dept"] = ddlDegree.SelectedItem.Text.ToString() + "-" + ddlBranch1.SelectedItem.Text.ToString();
                                dr1["Roll No"] = dsload.Tables[0].Rows[loop]["Roll_No"].ToString();
                                dr1["App no"] = dsload.Tables[0].Rows[loop]["app_no"].ToString();
                                dr1["Reg No"] = dsload.Tables[0].Rows[loop]["Reg_No"].ToString();
                                dr1["Student Name"] = dsload.Tables[0].Rows[loop]["Stud_Name"].ToString();
                                dr1["college code"] = dsload.Tables[0].Rows[loop]["college_code"].ToString();
                                trre.Rows.Add(dr1);
                            }
                            StudGridLookup.Visible = true;
                            gridview1.DataSource = trre;
                            gridview1.DataBind();
                            gridview1.Visible = true;


                            lblerrefp1.Visible = false;
                            tbvalue.Text = "";
                            tbvalue.Enabled = true;
                            ddloperator.Enabled = true;
                        }
                        else
                        {
                            StudGridLookup.Visible = false;
                            lblerrefp1.Visible = true;
                            lblerrefp1.Text = "No Record(s) Found";
                            tbvalue.Text = "";
                            tbvalue.Enabled = false;
                            ddloperator.Enabled = false;
                            ddlheader.ClearSelection();
                            ddloperator.ClearSelection();
                            btnlookupgo1.Enabled = true;
                        }
                    }
                }
            }
        }

        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    protected void gridview1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[2].Visible = false;
        e.Row.Cells[5].Visible = false;
    }

    protected void ddlheader_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlheader.SelectedItem.Text != "---Select---")
            {
                ddloperator.Enabled = true;
                btnlookupgo1.Enabled = true;

            }
            else
            {
                ddloperator.Enabled = false;
                tbvalue.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    protected void ddloperator_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (ddloperator.SelectedItem.Text != "---Select---")
            {
                tbvalue.Enabled = true;
                btnlookupgo1.Enabled = true;
            }
            else
            {
                tbvalue.Enabled = false;
                btnlookupgo1.Enabled = false;
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }

    }

    protected void tbvalue_TextChanged(object sender, EventArgs e)
    {
        try
        {
            StudentLookup1();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    protected void gridview2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //((CheckBox)e.Row.FindControl("allchk")).Attributes.Add("onclick",
            //    "javascript:SelectAll('" +
            //    ((CheckBox)e.Row.FindControl("allchk")).ClientID + "')");
            for (int grCol = 0; grCol < gridview2.Columns.Count; grCol++)
                e.Row.Cells[grCol].Visible = false;
            //e.Row.Cells[5].Visible = false;
            if (ddlreporttype.SelectedIndex == 0)
            {
                if (cbcumlative.Checked)
                {
                    e.Row.Cells[1].Visible = false;
                }
            }
            if (ddlreporttype.SelectedIndex == 1)
            {
                if (cbcumlative.Checked)
                {
                    e.Row.Cells[1].Visible = false;
                }
            }
            //if (ddlreporttype.SelectedIndex != 7 && ddlreporttype.SelectedIndex != 9 && ddlreporttype.SelectedIndex != 10 && ddlreporttype.SelectedIndex != 13)
            //{
            //    e.Row.Cells[2].Visible = false;
            //}
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //if (ddlreporttype.SelectedIndex != 7 && ddlreporttype.SelectedIndex != 9 && ddlreporttype.SelectedIndex != 10 && ddlreporttype.SelectedIndex != 13)
            //{
            //    e.Row.Cells[2].Visible = false;
            //}
            if (e.Row.RowIndex == 0)
            {
                //CheckBox cbsel = (CheckBox)e.Row.Cells[5].FindControl("selectchk");
                //cbsel.Visible = false;
                //cbsel.Text = "Select";

                // e.Row.Cells[1].Text = "Select";
            }
            if (ddlreporttype.SelectedIndex == 0)
            {
                if (cbcumlative.Checked)
                {
                    e.Row.Cells[1].Visible = false;
                    
                }
            }
            if (ddlreporttype.SelectedIndex == 1)
            {
                if (cbcumlative.Checked)
                {
                    e.Row.Cells[1].Visible = false;
                }
            }
        }

    }

    protected void gridview1_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void gridview1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);

            if (rowIndex != -1)
            {
                string RollNo = "";
                string studname = "";
                string Dept = "";
                string appno = "";
                string clgcode = "";
                RollNo = gridview1.Rows[rowIndex].Cells[1].Text;
                appno = gridview1.Rows[rowIndex].Cells[2].Text;
                studname = gridview1.Rows[rowIndex].Cells[3].Text;
                Dept = gridview1.Rows[rowIndex].Cells[0].Text;
                clgcode = gridview1.Rows[rowIndex].Cells[5].Text;
                txt_rolllno.Text = RollNo.ToString();
                Session["studstaffcollegecode"] = Convert.ToString(ddlcollegenew.SelectedValue);
                ViewState["Clgcode"] = clgcode;
            }

            Panellookup1.Visible = false;

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    //public void Studentinfo()
    //{
    //    try
    //    {
    //        FpSpread1.Sheets[0].PageSize = 5;
    //        FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
    //        FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.NextPrev;
    //        FpSpread1.Pager.Align = HorizontalAlign.Right;
    //        FpSpread1.Pager.Font.Bold = true;
    //        FpSpread1.Pager.ForeColor = Color.DarkGreen;
    //        FpSpread1.Pager.BackColor = Color.Beige;
    //        FpSpread1.Pager.BackColor = Color.AliceBlue;
    //        FpSpread1.Pager.PageCount = 5;
    //        FpSpread1.ActiveSheetView.SheetCorner.Cells[0, 0].Text = "S.No";
    //        FpSpread1.ActiveSheetView.SheetCorner.DefaultStyle.Font.Bold = true;
    //        FpSpread1.ActiveSheetView.DefaultRowHeight = 25;
    //        FpSpread1.ActiveSheetView.Rows.Default.Font.Name = "MS Sans Serif";
    //        FpSpread1.ActiveSheetView.Rows.Default.Font.Size = FontUnit.Small;
    //        FpSpread1.ActiveSheetView.Rows.Default.Font.Bold = false;
    //        FpSpread1.ActiveSheetView.Columns.Default.Font.Bold = false;
    //        FpSpread1.ActiveSheetView.Columns.Default.Font.Size = FontUnit.Small;
    //        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
    //        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "MS Sans Serif";
    //        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Small;
    //        FpSpread1.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
    //        FpSpread1.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
    //        FpSpread1.Sheets[0].ColumnCount = 4;
    //        FarPoint.Web.Spread.TextCellType tt = new FarPoint.Web.Spread.TextCellType();
    //        FpSpread1.Sheets[0].RowCount = 0;
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Degree";
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
    //        FpSpread1.Sheets[0].Columns[2].CellType = tt;
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "StudentName";
    //        FpSpread1.Sheets[0].Columns[0].Width = 500;
    //        FpSpread1.Sheets[0].Columns[0].Locked = true;
    //        FpSpread1.Sheets[0].Columns[1].Locked = true;
    //        FpSpread1.Sheets[0].Columns[1].Width = 100;
    //        FpSpread1.Sheets[0].Columns[2].Width = 100;
    //        FpSpread1.Sheets[0].Columns[3].Width = 200;
    //        FpSpread1.Width = 650;
    //        FpSpread1.Sheets[0].AutoPostBack = true;
    //        FpSpread1.CommandBar.Visible = false;
    //    }
    //    catch (Exception ex)
    //    {
    //        d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
    //    }
    //}

    #region selectfor

    public void selectfor()
    {
        try
        {
            ddlselectfor.Items.Add("All");
            ddlselectfor.Items.Add("Staff");
            ddlselectfor.Items.Add("Student");
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    #endregion

    #region Bindsearchby

    public void Bindsearchby()
    {
        try
        {
            ddlsearchby.Items.Add("All");
            ddlsearchby.Items.Add("Yearwise");
            ddlsearchby.Items.Add("Departmentwise");
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    #endregion

    #region type

    public void Bindtype()
    {
        try
        {
            ddltype.Items.Add("Books");
            ddltype.Items.Add("Periodicals");

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    #endregion

    #region accno

    public void Bindaccno()
    {
        try
        {
            ddlaccno.Items.Add("Greater Than");
            ddlaccno.Items.Add("Less Than");
            ddlaccno.Items.Add("Equal to");
            ddlaccno.Items.Add("Between");
            txt_accno.Text = "0";
            txt_accno2.Text = "0";

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    #endregion

    #region dept

    public void Binddept()
    {
        try
        {
            Hashtable has = new Hashtable();
            ddldept.Items.Clear();
            ds.Clear();
            string collegecode = ddlCollege.SelectedValue;
            string qry = " SELECT Course_Name+'-'+Dept_Name Degree,Degree_Code FROM Degree G,Course C,Department D WHERE G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code AND G.College_Code ='" + collegecode + "' ORDER BY Course_Name,Dept_Name";
            DataTable dtbatchyr = dirAcc.selectDataTable(qry);
            ddlbatch11.Items.Clear();
            if (dtbatchyr.Rows.Count > 0)
            {
                ddldept.DataSource = dtbatchyr;
                ddldept.DataTextField = "Degree";
                ddldept.DataValueField = "Degree_Code";
                ddldept.DataBind();
                ddldept.Items.Insert(0, "All");

                chklstdept.DataSource = dtbatchyr;
                chklstdept.DataTextField = "Degree";
                chklstdept.DataValueField = "Degree_Code";
                chklstdept.DataBind();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }

    }

    #endregion

    #region Batch

    public void BindBatch()
    {
        try
        {




            string qry = " select distinct Batch_Year from Registration order by batch_year desc";
            DataTable dtbatchyr = dirAcc.selectDataTable(qry);
            ddlbatch.Items.Clear();
            if (dtbatchyr.Rows.Count > 0)
            {
                ddlbatch.DataSource = dtbatchyr;
                ddlbatch.DataTextField = "Batch_Year";
                ddlbatch.DataValueField = "Batch_Year";
                ddlbatch.DataBind();


            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    #endregion

    #region lostbooks
    public void lostbooks()
    {
        try
        {


        }
        catch
        {
        }
    }
    #endregion

    #region Index Changed Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            isLibraryID();
            getLibPrivil();
            Binddept();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }


    }

    protected void ddlsearchby_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlsearchby.SelectedIndex == 0)
            {
                ddldept.Enabled = false;
                ddlbatch.Visible = false;
            }
            else if (ddlsearchby.SelectedIndex == 1)
            {
                ddldept.Enabled = false;
                ddlbatch.Visible = true;
            }
            else if (ddlsearchby.SelectedIndex == 2)
            {
                ddldept.Enabled = true;
                ddlbatch.Visible = false;
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }


    }

    protected void chkdept_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkdept.Checked == true)
            {
                for (int i = 0; i < chklstdept.Items.Count; i++)
                {
                    chklstdept.Items[i].Selected = true;
                    txtdept.Text = lbldept.Text + "(" + (chklstdept.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstdept.Items.Count; i++)
                {
                    chklstdept.Items[i].Selected = false;
                    txtdept.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }

    }

    protected void chklstdept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int degreecount = 0;
            string value = "";
            string code = "";
            for (int i = 0; i < chklstdept.Items.Count; i++)
            {
                if (chklstdept.Items[i].Selected == true)
                {
                    value = chklstdept.Items[i].Text;
                    code = chklstdept.Items[i].Value.ToString();
                    degreecount = degreecount + 1;
                    txtdept.Text = lbldept.Text + "(" + degreecount.ToString() + ")";
                }
            }
            if (degreecount == 0)
                txtdept.Text = "---Select---";
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    protected void ddlreporttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lbl_pagecnt.Visible = false;
            lbl_totrecord.Visible = false;
            //ddl_Txt_PageNo.Items.Clear();
            //ddl_Txt_PageNo.Items.Insert(0, "   ");
            if (ddlreporttype.SelectedIndex == 0)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                lblrollno.Visible = true;
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                txt_rolllno.Visible = true;
                cbcumlative.Visible = true;
                cbnotreturn.Visible = true;
                rbllostbooks.Visible = false;
                ddldept.Enabled = true;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                txt_accno2.Visible = false;
                cbduplicateaccno.Visible = false;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                ddlbatch.Visible = false;
                lblbatch.Visible = false;
                cbbatch.Visible = false;
                chk_ovrngtiss.Visible = false;
                cbfrom.Visible = true;
                lbl_fromdate1.Visible = true;
                txt_fromdate1.Visible = true;
                lbl_todate1.Visible = true;
                txt_todate1.Visible = true;
                ddlselectfor.Visible = true;
                lblselectfor.Visible = true;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
                batchyearvis();
                gridview2.Visible = false;

            }
            else if (ddlreporttype.SelectedIndex == 1)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                cbnotreturn.Visible = false;
                rbllostbooks.Visible = false;
                ddldept.Text = "All";
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                txt_accno2.Visible = false;
                cbduplicateaccno.Visible = false;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                chk_ovrngtiss.Visible = false;
                cbfrom.Visible = true;
                lbl_fromdate1.Visible = true;
                txt_fromdate1.Visible = true;
                lbl_todate1.Visible = true;
                txt_todate1.Visible = true;
                ddlselectfor.Visible = true;
                lblselectfor.Visible = true;
                lblrollno.Visible = true;
                txt_rolllno.Visible = true;
                cbcumlative.Visible = true;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
                batchyearvis();

            }
            else if (ddlreporttype.SelectedIndex == 2)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                ddlselectfor.Visible = true;
                lblselectfor.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                cbcumlative.Visible = true;
                cbnotreturn.Visible = false;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = false;
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                txt_accno2.Visible = false;
                cbduplicateaccno.Visible = false;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
            }
            else if (ddlreporttype.SelectedIndex == 3)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                cbcumlative.Visible = false;
                cbnotreturn.Visible = false;
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = true;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                txt_accno2.Visible = false;
                cbduplicateaccno.Visible = false;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
            }
            else if (ddlreporttype.SelectedIndex == 4)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                cbcumlative.Visible = false;
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                cbnotreturn.Visible = false;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = false;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                ddldept.Enabled = true;
                ddlselectfor.Visible = true;
                lblselectfor.Visible = true;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                txt_accno2.Visible = false;
                cbduplicateaccno.Visible = false;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                cbfrom.Visible = true;
                lbl_fromdate1.Visible = true;
                txt_fromdate1.Visible = true;
                lbl_todate1.Visible = true;
                txt_todate1.Visible = true;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
            }
            else if (ddlreporttype.SelectedIndex == 5)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                cbcumlative.Visible = false;
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                cbnotreturn.Visible = false;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = false;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                ddlselectfor.Visible = false;
                lblselectfor.Visible = false;
                ddldept.Enabled = true;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                txt_accno2.Visible = false;
                cbduplicateaccno.Visible = false;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                cbfrom.Visible = true;
                lbl_fromdate1.Visible = true;
                txt_fromdate1.Visible = true;
                lbl_todate1.Visible = true;
                txt_todate1.Visible = true;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
            }
            else if (ddlreporttype.SelectedIndex == 6)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                cbcumlative.Visible = false;
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                cbnotreturn.Visible = false;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = false;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                ddlselectfor.Visible = false;
                lblselectfor.Visible = false;
                ddldept.Enabled = true;
                lbltype.Visible = true;
                ddltype.Visible = true;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                txt_accno2.Visible = false;
                cbduplicateaccno.Visible = false;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                cbfrom.Visible = true;
                lbl_fromdate1.Visible = true;
                txt_fromdate1.Visible = true;
                lbl_todate1.Visible = true;
                txt_todate1.Visible = true;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;

            }
            else if (ddlreporttype.SelectedIndex == 7)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                cbcumlative.Visible = false;
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                cbnotreturn.Visible = false;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = false;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                ddlselectfor.Visible = false;
                lblselectfor.Visible = false;
                ddldept.Enabled = true;
                lbltype.Visible = false;
                ddltype.Visible = false;
                ddlselectfor.Visible = true;
                lblselectfor.Visible = true;
                lblaccno.Visible = true;
                ddlaccno.Visible = true;
                txt_accno.Visible = true;
                cbduplicateaccno.Visible = false;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                cbfrom.Visible = true;
                lbl_fromdate1.Visible = true;
                txt_fromdate1.Visible = true;
                lbl_todate1.Visible = true;
                txt_todate1.Visible = true;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
                txt_accno2.Visible = false;
            }


            else if (ddlreporttype.SelectedIndex == 8)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                cbcumlative.Visible = false;
                cbnotreturn.Visible = false;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = false;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                ddlselectfor.Visible = false;
                lblselectfor.Visible = false;
                ddldept.Enabled = true;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                cbduplicateaccno.Visible = true;
                cbfrom.Visible = false;
                lbl_fromdate1.Visible = false;
                txt_fromdate1.Visible = false;
                lbl_todate1.Visible = false;
                txt_todate1.Visible = false;
                cbaccessno.Visible = true;
                lblaccnofrom.Visible = true;
                tex_accnofrom.Visible = true;
                lblaccnoto.Visible = true;
                txt_accnoto.Visible = true;
                lbl_acr.Visible = true;
                txt_acr.Visible = true;
                cbmissingaccno.Visible = true;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
                txt_accno2.Visible = false;
            }
            else if (ddlreporttype.SelectedIndex == 9)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                cbcumlative.Visible = false;
                cbnotreturn.Visible = false;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = false;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                ddlselectfor.Visible = true;
                lblselectfor.Visible = true;
                ddldept.Enabled = true;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                cbduplicateaccno.Visible = false;
                cbfrom.Visible = true;
                lbl_fromdate1.Visible = true;
                txt_fromdate1.Visible = true;
                lbl_todate1.Visible = true;
                txt_todate1.Visible = true;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
                txt_accno2.Visible = false;
            }
            else if (ddlreporttype.SelectedIndex == 10)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                cbcumlative.Visible = false;
                cbnotreturn.Visible = false;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = false;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                ddlselectfor.Visible = true;
                lblselectfor.Visible = true;
                ddldept.Enabled = true;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                cbduplicateaccno.Visible = false;
                cbfrom.Visible = true;
                lbl_fromdate1.Visible = true;
                txt_fromdate1.Visible = true;
                lbl_todate1.Visible = true;
                txt_todate1.Visible = true;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
                txt_accno2.Visible = false;
            }
            else if (ddlreporttype.SelectedIndex == 11)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                cbcumlative.Visible = true;
                cbnotreturn.Visible = false;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = false;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                ddlselectfor.Visible = false;
                lblselectfor.Visible = false;
                ddldept.Enabled = true;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                cbduplicateaccno.Visible = false;
                cbfrom.Visible = true;
                lbl_fromdate1.Visible = true;
                txt_fromdate1.Visible = true;
                lbl_todate1.Visible = true;
                txt_todate1.Visible = true;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
                txt_accno2.Visible = false;
            }
            else if (ddlreporttype.SelectedIndex == 12)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                cbcumlative.Visible = false;
                cbnotreturn.Visible = false;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = false;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                ddlselectfor.Visible = false;
                lblselectfor.Visible = false;
                ddldept.Enabled = true;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                cbduplicateaccno.Visible = false;
                cbfrom.Visible = true;
                lbl_fromdate1.Visible = true;
                txt_fromdate1.Visible = true;
                lbl_todate1.Visible = true;
                txt_todate1.Visible = true;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
                txt_accno2.Visible = false;
            }
            else if (ddlreporttype.SelectedIndex == 13)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                cbcumlative.Visible = false;
                cbnotreturn.Visible = false;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = false;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                ddlselectfor.Visible = false;
                lblselectfor.Visible = false;
                ddldept.Enabled = true;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                cbduplicateaccno.Visible = false;
                cbfrom.Visible = true;
                lbl_fromdate1.Visible = true;
                txt_fromdate1.Visible = true;
                lbl_todate1.Visible = true;
                txt_todate1.Visible = true;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = true;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
                txt_accno2.Visible = false;
            }
            else if (ddlreporttype.SelectedIndex == 14)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                cbcumlative.Visible = true;
                cbnotreturn.Visible = false;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = false;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                ddlselectfor.Visible = false;
                lblselectfor.Visible = false;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                cbduplicateaccno.Visible = false;
                cbfrom.Visible = true;
                lbl_fromdate1.Visible = true;
                txt_fromdate1.Visible = true;
                lbl_todate1.Visible = true;
                txt_todate1.Visible = true;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
                txt_accno2.Visible = false;
            }
            else if (ddlreporttype.SelectedIndex == 15)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                cbcumlative.Visible = true;
                cbnotreturn.Visible = false;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = false;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                ddlselectfor.Visible = false;
                lblselectfor.Visible = false;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                cbduplicateaccno.Visible = false;
                cbfrom.Visible = true;
                lbl_fromdate1.Visible = true;
                txt_fromdate1.Visible = true;
                lbl_todate1.Visible = true;
                txt_todate1.Visible = true;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
                txt_accno2.Visible = false;
            }



            else if (ddlreporttype.SelectedIndex == 16)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                ddldept.Enabled = false;
                ddllibrary.Enabled = true;
                cbcumlative.Visible = false;
                cbnotreturn.Visible = false;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = false;
                lblbatch.Visible = false;
                ddlbatch.Visible = true;
                cbbatch.Visible = false;
                ddlselectfor.Visible = true;
                lblselectfor.Visible = true;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                cbduplicateaccno.Visible = false;
                cbfrom.Visible = true;
                lbl_fromdate1.Visible = true;
                txt_fromdate1.Visible = true;
                lbl_todate1.Visible = true;
                txt_todate1.Visible = true;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = false;
                lblsearchby.Visible = true;
                ddlsearchby.Visible = true;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
                txt_accno2.Visible = false;


            }
            else if (ddlreporttype.SelectedIndex == 17)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                ddldept.Enabled = true;
                cbcumlative.Visible = false;
                cbnotreturn.Visible = false;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = false;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                ddlselectfor.Visible = false;
                lblselectfor.Visible = false;
                ddldept.Enabled = true;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                cbduplicateaccno.Visible = false;
                cbfrom.Visible = true;
                lbl_fromdate1.Visible = true;
                txt_fromdate1.Visible = true;
                lbl_todate1.Visible = true;
                txt_todate1.Visible = true;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                chk_ovrngtiss.Visible = false;
                rbhitstatus.Visible = true;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                ddllibrary.Enabled = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
                txt_accno2.Visible = false;
            }



            else if (ddlreporttype.SelectedIndex == 18)
            {
                lbldept.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                ddldept.Visible = false;
                ddllibrary.Enabled = true;
                cbcumlative.Visible = false;
                cbnotreturn.Visible = false;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = false;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                ddlselectfor.Visible = false;
                lblselectfor.Visible = false;
                ddldept.Enabled = true;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                cbduplicateaccno.Visible = false;
                cbfrom.Visible = true;
                lbl_fromdate1.Visible = true;
                txt_fromdate1.Visible = true;
                lbl_todate1.Visible = true;
                txt_todate1.Visible = true;
                cbaccessno.Visible = true;
                lblaccnofrom.Visible = true;
                tex_accnofrom.Visible = true;
                lblaccnoto.Visible = true;
                txt_accnoto.Visible = true;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = true;
                ddlinwardtype.Visible = true;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = true;
                lblbillnofrom.Visible = true;
                txtbillnofrom.Visible = true;
                lblbillnoto.Visible = true;
                txtbillnoto.Visible = true;
                lblsupplier.Visible = true;
                ddlsupplier.Visible = true;
                txtdept.Visible = true;
                Paneldept.Visible = true;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
                txt_accno2.Visible = false;
            }
            else if (ddlreporttype.SelectedIndex == 19)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                cbcumlative.Visible = false;
                cbnotreturn.Visible = false;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = false;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                ddlselectfor.Visible = false;
                lblselectfor.Visible = false;
                ddldept.Enabled = true;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                cbduplicateaccno.Visible = false;
                cbfrom.Visible = true;
                lbl_fromdate1.Visible = true;
                txt_fromdate1.Visible = true;
                lbl_todate1.Visible = true;
                txt_todate1.Visible = true;
                cbaccessno.Visible = true;
                lblaccnofrom.Visible = true;
                tex_accnofrom.Visible = true;
                lblaccnoto.Visible = true;
                txt_accnoto.Visible = true;
                lbl_acr.Visible = true;
                txt_acr.Visible = true;
                cbmissingaccno.Visible = false;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = true;
                ddlsubject.Visible = true;
                rbbookdetails.Visible = true;
                lblinwardtype.Visible = true;
                ddlinwardtype.Visible = true;
                lblstatus.Visible = true;
                ddlstatus.Visible = true;
                lblremarks.Visible = true;
                ddlremarks.Visible = true;
                txtremarks.Visible = true;
                lblpubyear.Visible = true;
                txtpubyear.Visible = true;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
                txt_accno2.Visible = false;
            }
            else if (ddlreporttype.SelectedIndex == 20)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                ddldept.Enabled = true;
                cbcumlative.Visible = false;
                cbnotreturn.Visible = false;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = false;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                ddlselectfor.Visible = false;
                lblselectfor.Visible = false;
                ddldept.Enabled = true;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                cbduplicateaccno.Visible = false;
                cbfrom.Visible = true;
                lbl_fromdate1.Visible = true;
                txt_fromdate1.Visible = true;
                lbl_todate1.Visible = true;
                txt_todate1.Visible = true;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                chk_ovrngtiss.Visible = false;
                rbhitstatus.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                ddllibrary.Enabled = true;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = true;
                ddldepttype.Visible = true;
                rbdeptwise.Visible = true;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
                txt_accno2.Visible = false;

            }
            else if (ddlreporttype.SelectedIndex == 21)
            {
                lbldept.Visible = false;
                ddldept.Visible = false;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                cbcumlative.Visible = false;
                cbnotreturn.Visible = false;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = false;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                ddlselectfor.Visible = false;
                lblselectfor.Visible = false;
                ddldept.Enabled = true;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                cbduplicateaccno.Visible = false;
                cbfrom.Visible = false;
                lbl_fromdate1.Visible = false;
                txt_fromdate1.Visible = false;
                lbl_todate1.Visible = false;
                txt_todate1.Visible = false;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
                txt_accno2.Visible = false;
            }
            else if (ddlreporttype.SelectedIndex == 22)
            {
                lbldept.Visible = false;
                ddldept.Visible = false;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                cbcumlative.Visible = false;
                cbnotreturn.Visible = false;
                lblrollno.Visible = true;
                txt_rolllno.Visible = true;
                rbllostbooks.Visible = false;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                ddlselectfor.Visible = true;
                lblselectfor.Visible = true;
                ddldept.Enabled = true;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                cbduplicateaccno.Visible = false;
                cbfrom.Visible = true;
                lbl_fromdate1.Visible = true;
                txt_fromdate1.Visible = true;
                lbl_todate1.Visible = true;
                txt_todate1.Visible = true;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = true;
                lblname.Visible = true;
                rblist.Visible = true;
                enqbtn.Visible = true;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
                txt_accno2.Visible = false;
            }
            else if (ddlreporttype.SelectedIndex == 23)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                ddlselectfor.Visible = true;
                lblselectfor.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                cbcumlative.Visible = false;
                cbnotreturn.Visible = false;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = false;
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                txt_accno2.Visible = false;
                cbduplicateaccno.Visible = false;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
            }
            else if (ddlreporttype.SelectedIndex == 24)
            {
                lbldept.Visible = false;
                ddldept.Visible = false;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                cbcumlative.Visible = false;
                cbnotreturn.Visible = false;
                lblrollno.Visible = true;
                txt_rolllno.Visible = true;
                rbllostbooks.Visible = false;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                ddlselectfor.Visible = true;
                lblselectfor.Visible = true;
                ddldept.Enabled = true;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                cbduplicateaccno.Visible = false;
                cbfrom.Visible = true;
                lbl_fromdate1.Visible = true;
                txt_fromdate1.Visible = true;
                lbl_todate1.Visible = true;
                txt_todate1.Visible = true;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = true;
                ddlstatus1.Visible = true;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
                txt_accno2.Visible = false;
            }
            else if (ddlreporttype.SelectedIndex == 25)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                cbcumlative.Visible = false;
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                cbnotreturn.Visible = false;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = false;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                ddlselectfor.Visible = false;
                lblselectfor.Visible = false;
                ddldept.Enabled = true;
                lbltype.Visible = false;
                ddltype.Visible = false;
                ddlselectfor.Visible = true;
                lblselectfor.Visible = true;
                lblaccno.Visible = false;
                ddlaccno.Visible = true;
                txt_accno.Visible = true;
                cbduplicateaccno.Visible = false;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                cbfrom.Visible = true;
                lbl_fromdate1.Visible = true;
                txt_fromdate1.Visible = true;
                lbl_todate1.Visible = true;
                txt_todate1.Visible = true;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = true;
                cbresignedstaff.Visible = true;
                lblrackno.Visible = false;
                ddlrackno.Visible = false;
                lblshelfno.Visible = false;
                ddlshelfno.Visible = false;
                txt_accno2.Visible = false;
            }
            else if (ddlreporttype.SelectedIndex == 26)
            {
                lbldept.Visible = true;
                ddldept.Visible = true;
                gridview2.Visible = false;
                // rptprint1.Visible = false;
                cbcumlative.Visible = false;
                ddldept.Enabled = true;
                ddllibrary.Enabled = true;
                cbnotreturn.Visible = false;
                lblrollno.Visible = false;
                txt_rolllno.Visible = false;
                rbllostbooks.Visible = false;
                lblbatch.Visible = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                ddlselectfor.Visible = false;
                lblselectfor.Visible = false;
                ddldept.Enabled = true;
                lbltype.Visible = false;
                ddltype.Visible = false;
                lblaccno.Visible = false;
                ddlaccno.Visible = false;
                txt_accno.Visible = false;
                txt_accno2.Visible = false;
                cbduplicateaccno.Visible = false;
                cbaccessno.Visible = false;
                lblaccnofrom.Visible = false;
                tex_accnofrom.Visible = false;
                lblaccnoto.Visible = false;
                txt_accnoto.Visible = false;
                lbl_acr.Visible = false;
                txt_acr.Visible = false;
                cbmissingaccno.Visible = false;
                cbfrom.Visible = true;
                lbl_fromdate1.Visible = true;
                txt_fromdate1.Visible = true;
                lbl_todate1.Visible = true;
                txt_todate1.Visible = true;
                chk_ovrngtiss.Visible = false;
                rbtype.Visible = false;
                lblsearchby.Visible = false;
                ddlsearchby.Visible = false;
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                rbbookdetails.Visible = false;
                lblinwardtype.Visible = false;
                ddlinwardtype.Visible = false;
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                lblremarks.Visible = false;
                ddlremarks.Visible = false;
                txtremarks.Visible = false;
                lblpubyear.Visible = false;
                txtpubyear.Visible = false;
                rbhitstatus.Visible = false;
                cbbillno.Visible = false;
                lblbillnofrom.Visible = false;
                txtbillnofrom.Visible = false;
                lblbillnoto.Visible = false;
                txtbillnoto.Visible = false;
                lblsupplier.Visible = false;
                ddlsupplier.Visible = false;
                txtdept.Visible = false;
                Paneldept.Visible = false;
                lbldepttype.Visible = false;
                ddldepttype.Visible = false;
                rbdeptwise.Visible = false;
                txtname.Visible = false;
                lblname.Visible = false;
                rblist.Visible = false;
                enqbtn.Visible = false;
                lblstatus1.Visible = false;
                ddlstatus1.Visible = false;
                lbldays.Visible = false;
                cbresignedstaff.Visible = false;
                lblrackno.Visible = true;
                ddlrackno.Visible = true;
                lblshelfno.Visible = true;
                ddlshelfno.Visible = true;

            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }

    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            string selectQuery = "Select DISTINCT rack_no from rack_allocation where lib_code='" + ddllibrary.SelectedValue + "' order by rack_no";
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            ddlrackno.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlrackno.DataSource = ds;
                ddlrackno.DataTextField = "rack_no";
                ddlrackno.DataValueField = "rack_no";
                ddlrackno.DataBind();



            }
            ddlrackno.Items.Insert(0, "All");
            ddlshelfno.Items.Clear();
            ddlshelfno.Items.Insert(0, "All");


        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }

    }

    protected void ddlrackno_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string selectQuery = "";
            if (ddlrackno.Text == "All")
                selectQuery = "Select DISTINCT row_no from rack_allocation where lib_code='" + ddllibrary.SelectedValue + "' order by row_no";
            else
                selectQuery = "Select DISTINCT row_no from rack_allocation where lib_code='" + ddllibrary.SelectedValue + "' and rack_no='" + ddlrackno.Text + "' order by row_no";


            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            ddlshelfno.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlshelfno.DataSource = ds;
                ddlshelfno.DataTextField = "row_no";
                ddlshelfno.DataValueField = "row_no";
                ddlshelfno.DataBind();
                ddlshelfno.Items.Insert(0, "All");


            }



        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }

    }

    protected void ddldept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {
        }

    }

    protected void cbaccessno_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbaccessno.Checked == true)
            {
                tex_accnofrom.Enabled = true;
                txt_accnoto.Enabled = true;
            }
            else
            {
                tex_accnofrom.Enabled = false;
                txt_accnoto.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }

    }

    protected void cbbillno_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbbillno.Checked == true)
            {
                txtbillnofrom.Enabled = true;
                txtbillnoto.Enabled = true;
            }
            else
            {
                txtbillnofrom.Enabled = false;
                txtbillnoto.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }

    }

    protected void ddlselectfor_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlselectfor.SelectedIndex == 0)
            {
                ddldept.Enabled = false;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                lblbatch.Visible = false;
            }
            if (ddlselectfor.SelectedIndex == 1)
            {
                ddldept.Enabled = true;
                ddlbatch.Visible = false;
                cbbatch.Visible = false;
                lblbatch.Visible = false;
            }
            if (ddlselectfor.SelectedIndex == 2)
            {
                ddldept.Enabled = true;
                batchyearvis();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }

    }

    protected void ddlaccno_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlaccno.SelectedIndex == 3)
            {
                txt_accno2.Visible = true;
            }
            else
            {
                txt_accno2.Visible = false;
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }

    }

    protected void cbbatch_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbbatch.Checked)
            {
                lblbatch.Enabled = true;
                ddlbatch.Enabled = true;

            }
            else
            {
                lblbatch.Enabled = false;
                ddlbatch.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }

    }

    protected void cbfrom_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbfrom.Checked)
            {
                txt_fromdate1.Enabled = true;
                txt_todate1.Enabled = true;

            }
            else
            {
                txt_fromdate1.Enabled = false;
                txt_todate1.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }

    }

    //protected void cbselect_OnCheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (cbselect.Checked)
    //        {
    //            txt_from.Enabled = true;
    //            txt_to.Enabled = true;

    //        }
    //        else
    //        {
    //            txt_from.Enabled = false;
    //            txt_to.Enabled = false;
    //        }
    //    }
    //    catch
    //    {
    //    }

    //}

    protected void rbllostbooks_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            cbcumlative.Visible = false;
            cbnotreturn.Visible = false;
            lblrollno.Visible = false;
            txt_rolllno.Visible = false;

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }

    }

    public void batchyearvis()
    {
        try
        {
            if (ddlreporttype.SelectedIndex == 0 && ddlselectfor.SelectedIndex == 2)
            {
                cbbatch.Visible = true;
                lblbatch.Visible = true;
                ddlbatch.Visible = true;
            }
            else if (ddlreporttype.SelectedIndex == 1 && ddlselectfor.SelectedIndex == 2)
            {
                cbbatch.Visible = true;
                lblbatch.Visible = true;
                ddlbatch.Visible = true;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    #endregion

    protected void gridview2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void gridview2_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        gridview2.PageIndex = e.NewPageIndex;
        btn_go_Click(sender, e);
    }

    #region go

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {

            DataSet dsdetails = new DataSet();
            dsdetails = Transaction();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }

    }

    public DataSet Transaction()
    {
        DataSet dsdetails = new DataSet();
        string fromdate1 = string.Empty;
        string todate1 = string.Empty;
        string cbfromdate = string.Empty;
        string cbtodate = string.Empty;
        string qrycbfromfilter = string.Empty;
        string selectfor = string.Empty;
        string qryselectfilter = string.Empty;
        string accessfromdate = string.Empty;
        string accesstodate = string.Empty;
        DataRow dr;
        string strDate = string.Empty;
        string strdept = string.Empty;
        string strDegreecode = string.Empty;
        string Sql1 = string.Empty;
        string Sql11 = string.Empty;
        DataSet ds1 = new DataSet();
        String Batch1 = string.Empty;
        string strDeptname = string.Empty;
        string strovernight = string.Empty;
        string qrystr = string.Empty;
        string strboktype = string.Empty;
        try
        {
            if (ddlCollege.Items.Count > 0)
                collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            if (ddllibrary.Items.Count > 0)
                library = Convert.ToString(ddllibrary.SelectedValue);
            if (ddldept.Items.Count > 0)
                dept = Convert.ToString(ddldept.SelectedValue);
            if (ddlselectfor.Items.Count > 0)
                selectfor = Convert.ToString(ddlselectfor.SelectedValue);
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(library))
            {
                //if (library != "All" && library != "")
                //{
                //    qrylibraryFilter = "and l.lib_code in('" + library + "')";
                //}
                //string typ1 = string.Empty;
                if (ddllibrary.Items.Count > 0)
                {
                    for (int i = 0; i < ddllibrary.Items.Count - 1; i++)
                    {
                        if (Convert.ToString(ddllibrary.SelectedItem) == "All")
                        {
                            if (qrylibraryFilter == "")
                            {
                                qrylibraryFilter = "" + ddllibrary.Items[i + 1].Value + "";
                            }
                            else
                            {
                                qrylibraryFilter = qrylibraryFilter + "'" + "," + "'" + ddllibrary.Items[i + 1].Value + "";
                            }
                        }
                        else
                            qrylibraryFilter = ddllibrary.SelectedValue;
                    }
                }
                if (dept != "All" && dept != "")
                {
                    qrydeptfilter = " and Dept_Code='" + dept + "'";
                }
                if (cbfrom.Checked)
                {
                    string fromDate = txt_fromdate1.Text;
                    string toDate = txt_todate1.Text;
                    string[] fromdate = fromDate.Split('/');
                    string[] todate = toDate.Split('/');
                    if (fromdate.Length == 3)
                        fromdate1 = fromdate[2].ToString() + "-" + fromdate[1].ToString() + "-" + fromdate[0].ToString();

                    if (todate.Length == 3)
                        todate1 = todate[2].ToString() + "-" + todate[1].ToString() + "-" + todate[0].ToString();
                    qrycbfromfilter = "and b.bill_date between'" + cbfromdate + "'and '" + cbtodate + "'";
                }
                if (cbbatch.Checked == true)
                {
                    if (ddlbatch.Items.Count > 0)
                        batch = Convert.ToString(ddlbatch.SelectedValue);
                    qrybatchfilter = " and registration.batch_year ='" + batch + "'";
                }
                //if (cbselect.Checked == true)
                //{
                //    accessfromdate = txt_from.Text;
                //    accesstodate = txt_to.Text;

                //    qryselectfilter = "and b.acc_no between '" + accessfromdate + "' and '" + accesstodate + "'";
                //}
                if (cbcumlative.Checked == false)
                {
                    if (ddlselectfor.SelectedIndex == 2)
                    {
                        qryselectforfilter = " and registration.batch_year ='" + batch + "'";
                    }
                }

                #region Issued Books

                if (ddlreporttype.SelectedIndex == 0)
                {
                    //strID = Session["strID"].ToString();
                    //strStaffID = Session["strStaffID"].ToString();
                    string deptqur = "";
                    string sqlCount = "";
                    string Batch = "";
                    string rollno = "";
                    if (txt_rolllno.Text != "")
                        rollno = " and borrow.roll_no='" + txt_rolllno.Text + "'";
                    else
                        rollno = "";
                    //if (ddllibrary.Text != "All")
                    //    libcode = ddllibrary.SelectedValue;
                    //else
                    //    libcode = "%";
                    if (ddldept.Text != "All")
                        deptqur = ddldept.Text;
                    else
                        deptqur = "%";
                    if (cbfrom.Checked == true)
                        strDate = "  and  borrow_date between '" + fromdate1 + "' and '" + todate1 + "'";
                    else
                        strDate = "";
                    if (chk_ovrngtiss.Checked == true)
                        strovernight = "  and due_date-borrow_date = 1 ";
                    else
                        strovernight = "";
                    if (ddldept.Text == "All")
                        strDegreecode = "";
                    else
                        strDegreecode = " and registration.degree_code='" + ddldept.SelectedValue + "'";

                    if (cbcumlative.Checked == false)
                    {
                        Sql11 = "Select distinct(borrow.acc_no)as 'Access No',token_no as 'Card No',cirno_issue as 'Issue Circulation No',borrow.roll_no as 'Roll No',borrow.stud_name as 'Name', convert(varchar,borrow_date,103) as 'Borrow Date',isnull(borrow.Issued_Time,'') as 'Issued_Time',convert(varchar,due_date,103) as 'Due Date',case when borrow.return_flag=0 then 'NR' else convert(varchar,return_date,103) end as 'Return Date',isnull(borrow.returned_time,'') as 'Returned Time',title as 'Title',author as 'Author',borrow.book_issuedby as 'Book Issued By',borrow.return_type as 'Return Type',library.lib_name as 'Library Name'";

                        if (ddlselectfor.Text == "Student")
                        {
                            if (cbbatch.Checked == true)
                                Batch1 = "  and registration.batch_year = '" + ddlbatch.SelectedItem + "'";
                            else
                                Batch1 = "";
                            if (cbnotreturn.Checked == false)
                                Sql1 = Sql11 + " from borrow,library,department,registration,degree where (registration.roll_no =borrow.roll_no or registration.lib_id = borrow.roll_no) and registration.roll_no =borrow.roll_no and registration.degree_code=degree.degree_code " + Batch1 + strDegreecode + "  and degree.dept_code=department.dept_code and borrow.lib_code=library.lib_code and is_staff = 0 " + strDate + strovernight + " and borrow.lib_code in ('" + qrylibraryFilter + "') " + rollno;
                            else
                                Sql1 = Sql11 + " from borrow,library,department,registration,degree where (registration.roll_no =borrow.roll_no or registration.lib_id = borrow.roll_no) and registration.roll_no =borrow.roll_no " + Batch1 + strDegreecode + "  and registration.degree_code=degree.degree_code and degree.dept_code=department.dept_code and borrow.lib_code=library.lib_code and is_staff = 0 " + strDate + strovernight + " and borrow.lib_code in ('" + qrylibraryFilter + "')  and borrow.return_flag=0" + rollno;

                            if (cbnotreturn.Checked == false)
                                Sql1 = Sql1 + " Union " + Sql11 + " from borrow,library,user_master where user_master.user_id=borrow.roll_no and user_master.is_staff=0 and user_master.department like '" + deptqur + "' AND borrow.lib_code=library.lib_code and borrow.is_staff = 0 " + strDate + strovernight + " and borrow.lib_code in ('" + qrylibraryFilter + "')" + rollno;
                            else
                                Sql1 = Sql1 + " Union " + Sql11 + " from borrow,library,user_master where user_master.user_id=borrow.roll_no and user_master.is_staff=0 and user_master.department like '" + deptqur + "' AND borrow.lib_code=library.lib_code and borrow.is_staff = 0 " + strDate + strovernight + " and borrow.lib_code in ('" + qrylibraryFilter + "')' and borrow.return_flag=0" + rollno;
                        }
                        else if (ddlselectfor.Text == "Staff")
                        {
                            if (cbnotreturn.Checked == false)
                                Sql1 = Sql11 + " from borrow,library,staffmaster,stafftrans,hrdept_master where (staffmaster.staff_code = borrow.roll_no or staffmaster.lib_id = borrow.roll_no) and staffmaster.staff_code=stafftrans.staff_code and stafftrans.latestrec=1 and stafftrans.dept_code=hrdept_master.dept_code and hrdept_master.dept_name like '" + deptqur + "' AND borrow.lib_code=library.lib_code and is_staff = 1" + strDate + strovernight + "  and borrow.lib_code in ('" + qrylibraryFilter + "')" + rollno;
                            else
                                Sql1 = Sql11 + " from borrow,library,staffmaster,stafftrans,hrdept_master where (staffmaster.staff_code = borrow.roll_no or staffmaster.lib_id = borrow.roll_no) and staffmaster.staff_code=stafftrans.staff_code and stafftrans.latestrec=1 and stafftrans.dept_code=hrdept_master.dept_code and hrdept_master.dept_name like '" + deptqur + "' AND borrow.lib_code=library.lib_code and is_staff = 1" + strDate + strovernight + "  and borrow.lib_code in ('" + qrylibraryFilter + "')' and borrow.return_flag=0" + rollno;
                            if (cbnotreturn.Checked == false)
                                Sql1 = Sql1 + " union " + Sql11 + " from borrow,library,user_master where user_master.user_id=borrow.roll_no and user_master.department like '" + deptqur + "' AND borrow.lib_code=library.lib_code and borrow.is_staff = 1 and user_master.is_staff=1 " + strDate + strovernight + "  and borrow.lib_code in ('" + qrylibraryFilter + "')" + rollno;
                            else
                                Sql1 = Sql1 + " union " + Sql11 + " from borrow,library,user_master where user_master.user_id=borrow.roll_no and user_master.department like '" + deptqur + "' AND borrow.lib_code=library.lib_code and borrow.is_staff = 1 and user_master.is_staff=1 " + strDate + strovernight + "  and borrow.lib_code in ('" + qrylibraryFilter + "') and borrow.return_flag=0" + rollno;
                        }
                        else if (ddlselectfor.Text == "All")
                        {
                            if (cbnotreturn.Checked == false)
                                sqlCount = Sql11 + " from borrow,library,department,registration,degree where (registration.roll_no = borrow.roll_no or registration.lib_id = borrow.roll_no) and registration.roll_no = borrow.roll_no and registration.degree_code=degree.degree_code " + strDegreecode + "  and degree.dept_code=department.dept_code and department.dept_name like '%' AND borrow.lib_code=library.lib_code " + strDate + strovernight + " and borrow.lib_code in ('" + qrylibraryFilter + "') and borrow.is_staff = 0" + rollno;
                            else
                                sqlCount = Sql11 + " from borrow,library,department,registration,degree where (registration.roll_no = borrow.roll_no or registration.lib_id = borrow.roll_no) and registration.roll_no = borrow.roll_no " + strDegreecode + " and registration.degree_code=degree.degree_code and degree.dept_code=department.dept_code and department.dept_name like '%' AND borrow.lib_code=library.lib_code " + strDate + strovernight + " and borrow.lib_code in ('" + qrylibraryFilter + "') and borrow.return_flag=0 and borrow.is_staff = 0" + rollno;

                            if (cbnotreturn.Checked == false)
                                sqlCount = Sql11 + " from borrow,library,staffmaster,stafftrans,hrdept_master where (staffmaster.staff_code = borrow.roll_no or staffmaster.lib_id = borrow.roll_no) and staffmaster.staff_code = borrow.roll_no and staffmaster.staff_code=stafftrans.staff_code and stafftrans.latestrec=1 and stafftrans.dept_code=hrdept_master.dept_code and hrdept_master.dept_name like '" + deptqur + "' AND borrow.lib_code=library.lib_code and is_staff = 1" + strDate + strovernight + " and borrow.lib_code in ('" + qrylibraryFilter + "') and borrow.is_staff = 1" + rollno;
                            else
                                sqlCount = Sql11 + " from borrow,library,staffmaster,stafftrans,hrdept_master where (staffmaster.staff_code = borrow.roll_no or staffmaster.lib_id = borrow.roll_no) and staffmaster.staff_code = borrow.roll_no and staffmaster.staff_code=stafftrans.staff_code and stafftrans.latestrec=1 and stafftrans.dept_code=hrdept_master.dept_code and hrdept_master.dept_name like '" + deptqur + "' AND borrow.lib_code=library.lib_code and is_staff = 1" + strDate + strovernight + " and borrow.lib_code in ('" + qrylibraryFilter + "') and borrow.return_flag=0 and borrow.is_staff = 1" + rollno;

                            if (cbnotreturn.Checked == false)
                                Sql1 = Sql1 + " union " + Sql1 + " from borrow,library,user_master where user_master.user_id =borrow.roll_no and user_master.department like '" + deptqur + "' AND borrow.lib_code=library.lib_code " + strDate + strovernight + "  and borrow.lib_code in ('" + qrylibraryFilter + "')" + rollno;
                            else
                                Sql1 = Sql1 + " union " + Sql11 + " from borrow,library,user_master where user_master.user_id =borrow.roll_no and user_master.department like '" + deptqur + "' AND borrow.lib_code=library.lib_code " + strDate + strovernight + "  and borrow.lib_code in ('" + qrylibraryFilter + "') and borrow.return_flag=0" + rollno;

                            if (cbnotreturn.Checked == false)
                                Sql1 = Sql11 + " from borrow,library,department,registration,degree where (registration.roll_no = borrow.roll_no or registration.lib_id = borrow.roll_no) and registration.roll_no = borrow.roll_no and registration.degree_code=degree.degree_code " + strDegreecode + "  and degree.dept_code=department.dept_code and department.dept_name like '%' AND borrow.lib_code=library.lib_code " + strDate + strovernight + " and borrow.lib_code in ('" + qrylibraryFilter + "')" + rollno + " union all ";
                            else
                                Sql1 = Sql11 + " from borrow,library,department,registration,degree where (registration.roll_no = borrow.roll_no or registration.lib_id = borrow.roll_no) and registration.roll_no = borrow.roll_no " + strDegreecode + " and registration.degree_code=degree.degree_code and degree.dept_code=department.dept_code and department.dept_name like '%' AND borrow.lib_code=library.lib_code " + strDate + strovernight + " and borrow.lib_code in ('" + qrylibraryFilter + "') and borrow.return_flag=0 " + rollno + " union all ";

                            if (cbnotreturn.Checked == false)
                                Sql1 = Sql1 + " " + Sql11 + " from borrow,library,staffmaster,stafftrans,hrdept_master where (staffmaster.staff_code = borrow.roll_no or staffmaster.lib_id = borrow.roll_no) and staffmaster.staff_code = borrow.roll_no and staffmaster.staff_code=stafftrans.staff_code and stafftrans.latestrec=1 and stafftrans.dept_code=hrdept_master.dept_code and hrdept_master.dept_name like '" + deptqur + "' AND borrow.lib_code=library.lib_code and is_staff = 1" + strDate + strovernight + " and borrow.lib_code in ('" + qrylibraryFilter + "')" + rollno;
                            else
                                Sql1 = Sql1 + " " + Sql11 + " from borrow,library,staffmaster,stafftrans,hrdept_master where (staffmaster.staff_code = borrow.roll_no or staffmaster.lib_id = borrow.roll_no) and staffmaster.staff_code = borrow.roll_no and staffmaster.staff_code=stafftrans.staff_code and stafftrans.latestrec=1 and stafftrans.dept_code=hrdept_master.dept_code and hrdept_master.dept_name like '" + deptqur + "' AND borrow.lib_code=library.lib_code and is_staff = 1" + strDate + strovernight + " and borrow.lib_code in ('" + qrylibraryFilter + "')' and borrow.return_flag=0" + rollno;
                            if (cbnotreturn.Checked == false)
                                Sql1 = Sql1 + " union " + Sql11 + " from borrow,library,user_master where user_master.user_id =borrow.roll_no and user_master.department like '" + deptqur + "' AND borrow.lib_code=library.lib_code " + strDate + strovernight + "  and borrow.lib_code in ('" + qrylibraryFilter + "')" + rollno;
                            else
                                Sql1 = Sql1 + " union " + Sql11 + " from borrow,library,user_master where user_master.user_id =borrow.roll_no and user_master.department like '" + deptqur + "' AND borrow.lib_code=library.lib_code " + strDate + strovernight + "  and borrow.lib_code in ('" + qrylibraryFilter + "') and borrow.return_flag=0" + rollno;
                        }
                        ds1.Clear();
                        ds1 = d2.select_method_wo_parameter(Sql1, "text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            transrepo.Columns.Add("SNo", typeof(string));
                            transrepo.Columns.Add("Access No", typeof(string));
                            transrepo.Columns.Add("Card No", typeof(string));
                            transrepo.Columns.Add("Issue Circulation No", typeof(string));
                            transrepo.Columns.Add("Roll No", typeof(string));
                            transrepo.Columns.Add("Name", typeof(string));
                            transrepo.Columns.Add("Borrow Date", typeof(string));
                            transrepo.Columns.Add("Issued_Time", typeof(string));
                            transrepo.Columns.Add("Due Date", typeof(string));
                            transrepo.Columns.Add("Return Date", typeof(string));
                            transrepo.Columns.Add("Returned Time", typeof(string));
                            transrepo.Columns.Add("Title", typeof(string));
                            transrepo.Columns.Add("Author", typeof(string));
                            transrepo.Columns.Add("Book Issued By", typeof(string));
                            transrepo.Columns.Add("Return Type", typeof(string));
                            transrepo.Columns.Add("Library Name", typeof(string));

                            int row = 0;
                            int i = 0;
                            int SNo = 0;
                            for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                            {
                                SNo++;
                                dr = transrepo.NewRow();
                                dr["SNo"] = Convert.ToString(SNo);
                                dr["Access No"] = ds1.Tables[0].Rows[r]["Access No"].ToString();
                                dr["Card No"] = ds1.Tables[0].Rows[r]["Card No"].ToString();
                                dr["Issue Circulation No"] = ds1.Tables[0].Rows[r]["Issue Circulation No"].ToString();
                                dr["Roll No"] = ds1.Tables[0].Rows[r]["Roll No"].ToString();
                                dr["Name"] = ds1.Tables[0].Rows[r]["Name"].ToString();
                                dr["Borrow Date"] = ds1.Tables[0].Rows[r]["Borrow Date"].ToString();
                                dr["Issued_Time"] = ds1.Tables[0].Rows[r]["Issued_Time"].ToString();
                                dr["Due Date"] = ds1.Tables[0].Rows[r]["Due Date"].ToString();
                                dr["Return Date"] = ds1.Tables[0].Rows[r]["Return Date"].ToString();
                                dr["Returned Time"] = ds1.Tables[0].Rows[r]["Returned Time"].ToString();
                                dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                                dr["Author"] = ds1.Tables[0].Rows[r]["Author"].ToString();
                                dr["Book Issued By"] = ds1.Tables[0].Rows[r]["Book Issued By"].ToString();
                                dr["Return Type"] = ds1.Tables[0].Rows[r]["Return Type"].ToString();
                                dr["Library Name"] = ds1.Tables[0].Rows[r]["Library Name"].ToString();
                                transrepo.Rows.Add(dr);
                            }
                            gridview2.DataSource = transrepo;
                            gridview2.DataBind();

                            gridview2.Visible = true;
                        }
                    }
                    else
                    {
                        if (ddlselectfor.Text == "Student")
                        {
                            if (cbbatch.Checked == true)
                                Batch = "  and registration.batch_year = '" + ddlbatch.Text + "'";
                            else
                                Batch = "";
                            Sql1 = "select 'Select' as sel,'Student' as Type,count(*) as 'Books' from borrow,library,department,registration,degree where registration.roll_no=borrow.roll_no " + Batch + strDegreecode + "  and registration.degree_code=degree.degree_code and degree.dept_code=department.dept_code and borrow.lib_code=library.lib_code and is_staff = 0 " + strDate + strovernight + " and borrow.lib_code like '" + qrylibraryFilter + "' and return_type = 'BOK'";
                            if (cbnotreturn.Checked == true)
                                Sql1 = Sql1 + " and borrow.return_flag = 0";
                            Sql1 = Sql1 + "select count(*) as 'Non Books' from borrow,library,department,registration,degree where registration.roll_no=borrow.roll_no " + Batch + strDegreecode + "  and registration.degree_code=degree.degree_code and degree.dept_code=department.dept_code and borrow.lib_code=library.lib_code and is_staff = 0 " + strDate + strovernight + " and borrow.lib_code like '" + libcode + "'  and borrow.return_flag=0 and return_type = 'NBM'";
                        }
                        else if (ddlselectfor.Text == "Staff")
                        {
                            Sql1 = "Select 'Staff' as Type,Count(*) as 'Books' from borrow,library,staffmaster,stafftrans,hrdept_master where staffmaster.staff_code=borrow.roll_no and staffmaster.staff_code=stafftrans.staff_code and stafftrans.latestrec=1 and stafftrans.dept_code=hrdept_master.dept_code and hrdept_master.dept_name like '" + deptqur + "' AND borrow.lib_code=library.lib_code and is_staff = 1" + strDate + strovernight + "  and borrow.lib_code like '" + qrylibraryFilter + "' and return_type = 'BOK'";
                            if (cbnotreturn.Checked == true)
                                Sql1 = Sql1 + " and borrow.return_flag = 0";
                            Sql1 = Sql1 + "Select Count(*) as 'Non Books' from borrow,library,staffmaster,stafftrans,hrdept_master where  staffmaster.staff_code=borrow.roll_no and staffmaster.staff_code=stafftrans.staff_code and stafftrans.latestrec=1 and stafftrans.dept_code=hrdept_master.dept_code and hrdept_master.dept_name like '" + deptqur + "' AND borrow.lib_code=library.lib_code and is_staff = 1" + strDate + strovernight + "  and borrow.lib_code like '" + qrylibraryFilter + "' and borrow.return_flag=0  and return_type = 'NBM'";
                        }
                        else if (ddlselectfor.Text == "All")
                        {
                            Sql1 = "select 'Student' as Type,count(*) as 'Books' from borrow,library,department,registration,degree where registration.roll_no=borrow.roll_no " + Batch + strDegreecode + "  and registration.degree_code=degree.degree_code and degree.dept_code=department.dept_code and borrow.lib_code=library.lib_code and is_staff = 0 " + strDate + strovernight + " and borrow.lib_code like '" + qrylibraryFilter + "' and return_type = 'BOK'";
                            if (cbnotreturn.Checked == true)
                                Sql1 = Sql1 + " and borrow.return_flag = 0";
                            Sql1 = Sql1 + "union all ";
                            Sql1 = Sql1 + "Select 'Staff' as type,Count(*) as 'Books' from borrow,library,staffmaster,stafftrans,hrdept_master where staffmaster.staff_code=borrow.roll_no and staffmaster.staff_code=stafftrans.staff_code and stafftrans.latestrec=1 and stafftrans.dept_code=hrdept_master.dept_code and hrdept_master.dept_name like '" + deptqur + "' AND borrow.lib_code=library.lib_code and is_staff = 1" + strDate + strovernight + "  and borrow.lib_code like '" + qrylibraryFilter + "' and return_type = 'BOK'";
                            if (cbnotreturn.Checked == true)
                                Sql1 = Sql1 + " and borrow.return_flag = 0";
                            Sql1 = Sql1 + "select count(*) as 'Non Books' from borrow,library where borrow.lib_code=library.lib_code " + strDate + strovernight + " and borrow.lib_code like '" + qrylibraryFilter + "' and borrow.return_flag=0  and return_type = 'NBM'";
                        }
                        ds1 = d2.select_method_wo_parameter(Sql1, "text");
                        int grandprev = 0;
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            int colcount = 0;
                            transrepo.Columns.Add("SNo", typeof(string));
                            transrepo.Columns.Add("Type", typeof(string));
                            transrepo.Columns.Add("Books", typeof(string));
                            transrepo.Columns.Add("Non Books", typeof(string));
                            transrepo.Columns.Add("Total", typeof(string));

                            int row = 0;
                            int i = 0;
                            int SNo = 0;
                            for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                            {
                                SNo++;
                                dr = transrepo.NewRow();
                                dr["SNo"] = Convert.ToString(SNo);
                                dr["Type"] = ds1.Tables[0].Rows[r]["Type"].ToString();
                                dr["Books"] = ds1.Tables[0].Rows[r]["Books"].ToString();
                                dr["Non Books"] = ds1.Tables[0].Rows[r]["Non Books"].ToString();
                                transrepo.Rows.Add(dr);
                                int m = Convert.ToInt32(ds1.Tables[0].Rows[r]["Books"]);
                                int n = Convert.ToInt32(ds1.Tables[0].Rows[r]["Non Books"]);
                                grandprev = m + n;
                                if (ds1.Tables[1].Rows.Count > r)
                                {
                                    grandprev = m + n;
                                }
                                else
                                {
                                    grandprev = m + 0;
                                }
                                dr = transrepo.NewRow();
                                dr["Total"] = Convert.ToString(grandprev);
                                transrepo.Rows.Add(dr);
                                gridview2.DataSource = transrepo;
                                gridview2.DataBind();

                                gridview2.Visible = true;
                            }
                        }
                    }
                }
                #endregion

                #region Returned Books added by rajasekar 17/5/2018

                else if (ddlreporttype.SelectedIndex == 1)
                {
                    if (cbcumlative.Checked == false)
                    {
                        if (ddlselectfor.Text == "Student")
                        {
                            Sql1 = "SELECT ISNULL(COUNT(*),0) ";
                            Sql1 = Sql1 + "FROM Borrow B,Registration R,Degree G,Library L ";
                            Sql1 = Sql1 + "WHERE (B.Roll_No = R.Roll_No OR B.Roll_No = R.Lib_ID) and B.Roll_No = R.Roll_No AND B.Lib_Code = L.Lib_Code ";
                            Sql1 = Sql1 + "AND R.Degree_Code = G.Degree_Code ";
                            Sql1 = Sql1 + "AND B.Is_Staff = 0 AND Return_Flag = 1 AND L.College_Code =" + collegeCode;
                            if (ddllibrary.Text != "All")
                                Sql1 = Sql1 + " AND B.Lib_Code ='" + ddllibrary.SelectedValue + "' ";
                            if (ddldept.Text != "All")
                                Sql1 = Sql1 + " AND R.Degree_Code =" + ddldept.SelectedValue;
                            if (cbfrom.Checked == true)
                                Sql1 = Sql1 + " AND Return_Date BETWEEN '" + fromdate1 + "' and '" + todate1 + "'";
                            if (chk_ovrngtiss.Checked == true)
                                Sql1 = Sql1 + " AND Due_Date-Borrow_Date = 1 ";
                            if (cbbatch.Checked == true)
                                Sql1 = Sql1 + " AND R.Batch_Year =" + ddlbatch.SelectedItem;
                            if (txt_rolllno.Text.Trim() != "")
                                Sql1 = Sql1 + " AND (R.Roll_NO ='" + txt_rolllno.Text + "' OR R.Lib_ID ='" + txt_rolllno.Text + "')";

                            ds1 = d2.select_method_wo_parameter(Sql1, "text");
                            string TotStud = ds1.Tables[0].Rows[0][0].ToString();

                            Sql1 = "SELECT DISTINCT B.Acc_No as 'Access No',B.Roll_No,R.Stud_Name as 'Name',Token_No as 'Card No',CONVERT(Varchar,Borrow_Date,103) as 'Borrow Date',CONVERT(Varchar,Due_Date,103) as 'Due Date',CONVERT(Varchar,Return_Date,103) as 'Return Date',ISNULL(Issued_Time,'') as Issued_Time,ISNULL(Returned_Time,'') as 'Returned Time',Title as 'Title',Author as 'Author',B.Book_Returnby as 'Book Return By',B.Return_Type as 'Return Type',L.Lib_Name as 'Library name' ";
                            Sql1 = Sql1 + "FROM Borrow B,Registration R,Degree G,Library L ";
                            Sql1 = Sql1 + "WHERE (B.Roll_No = R.Roll_No OR B.Roll_No = R.Lib_ID) and B.Roll_No = R.Roll_No AND B.Lib_Code = L.Lib_Code ";
                            Sql1 = Sql1 + "AND R.Degree_Code = G.Degree_Code ";
                            Sql1 = Sql1 + "AND B.Is_Staff = 0 AND Return_Flag = 1 AND L.College_Code =" + collegeCode;
                            if (ddllibrary.Text != "All")
                                Sql1 = Sql1 + " AND B.Lib_Code ='" + ddllibrary.SelectedValue + "' ";
                            if (ddldept.Text != "All")
                                Sql1 = Sql1 + " AND R.Degree_Code =" + ddldept.SelectedValue;
                            if (cbfrom.Checked == true)
                                Sql1 = Sql1 + " AND Return_Date BETWEEN '" + fromdate1 + "' and '" + todate1 + "'";
                            if (chk_ovrngtiss.Checked == true)
                                Sql1 = Sql1 + " AND Due_Date-Borrow_Date = 1 ";
                            if (cbbatch.Checked == true)
                                Sql1 = Sql1 + " AND R.Batch_Year =" + ddlbatch.SelectedItem;
                            if (txt_rolllno.Text.Trim() != "")
                                Sql1 = Sql1 + " AND (R.Roll_NO ='" + txt_rolllno.Text + "' OR R.Lib_ID ='" + txt_rolllno.Text + "')";
                            Sql1 = Sql1 + "ORDER BY 'Return Date','Name' ";
                        }
                        else if (ddlselectfor.Text == "Staff")
                        {
                            Sql1 = "SELECT COUNT(*) ";
                            Sql1 = Sql1 + "FROM Borrow B,StaffMaster M,StaffTrans T,HrDept_Master D,Library L ";
                            Sql1 = Sql1 + "WHERE (B.Roll_No = M.Staff_Code OR B.Roll_No = M.Lib_ID) and B.Roll_No = M.Staff_Code AND B.Lib_Code = L.Lib_Code ";
                            Sql1 = Sql1 + "AND M.Staff_Code = T.Staff_Code AND T.Dept_Code = D.Dept_Code AND M.College_Code = D.College_Code AND M.College_Code = L.College_Code ";
                            Sql1 = Sql1 + "AND T.Latestrec = 1 AND B.Is_Staff = 1 AND Return_Flag = 1 AND L.College_Code =" + collegeCode;
                            if (ddllibrary.Text != "All")
                                Sql1 = Sql1 + " AND B.Lib_Code ='" + ddllibrary.SelectedValue + "' ";
                            if (ddldept.Text != "All")
                                Sql1 = Sql1 + " AND R.Degree_Code =" + ddldept.SelectedValue;
                            if (cbfrom.Checked == true)
                                Sql1 = Sql1 + " AND Return_Date BETWEEN '" + fromdate1 + "' and '" + todate1 + "'";
                            if (chk_ovrngtiss.Checked == true)
                                Sql1 = Sql1 + " AND Due_Date-Borrow_Date = 1 ";
                            if (txt_rolllno.Text.Trim() != "")
                                Sql1 = Sql1 + " AND (M.Staff_Code ='" + txt_rolllno.Text + "' OR M.Lib_ID ='" + txt_rolllno.Text + "')";


                            ds1 = d2.select_method_wo_parameter(Sql1, "text");
                            string TotStaff = ds1.Tables[0].Rows[0][0].ToString();
                            Sql1 = "SELECT DISTINCT B.Acc_No as 'Access No',B.Roll_No,M.Staff_Name as 'Name',Token_No as 'Card No',CONVERT(Varchar,Borrow_Date,103) as 'Borrow Date',CONVERT(Varchar,Due_Date,103) as 'Due Date',CONVERT(Varchar,Return_Date,103) as 'Return Date',ISNULL(Issued_Time,'') as Issued_Time,ISNULL(Returned_Time,'') as 'Returned Time',Title as 'Title',Author as 'Author',B.Book_Returnby as 'Book Return By',B.Return_Type as 'Return Type',L.Lib_Name as 'Library name' ";
                            Sql1 = Sql1 + "FROM Borrow B,StaffMaster M,StaffTrans T,HrDept_Master D,Library L ";
                            Sql1 = Sql1 + "WHERE (B.Roll_No = M.Staff_Code OR B.Roll_No = M.Lib_ID) and B.Roll_No = M.Staff_Code AND B.Lib_Code = L.Lib_Code ";
                            Sql1 = Sql1 + "AND M.Staff_Code = T.Staff_Code AND T.Dept_Code = D.Dept_Code AND M.College_Code = D.College_Code AND M.College_Code = L.College_Code ";
                            Sql1 = Sql1 + "AND B.Is_Staff = 1 AND Return_Flag = 1 AND L.College_Code =" + collegeCode;
                            if (ddllibrary.Text != "All")
                                Sql1 = Sql1 + " AND B.Lib_Code ='" + ddllibrary.SelectedValue + "' ";
                            if (ddldept.Text != "All")
                                Sql1 = Sql1 + " AND R.Degree_Code =" + ddldept.SelectedValue;
                            if (cbfrom.Checked == true)
                                Sql1 = Sql1 + " AND Return_Date BETWEEN '" + fromdate1 + "' and '" + todate1 + "'";
                            if (chk_ovrngtiss.Checked == true)
                                Sql1 = Sql1 + " AND Due_Date-Borrow_Date = 1 ";
                            if (txt_rolllno.Text.Trim() != "")
                                Sql1 = Sql1 + " AND (M.Staff_Code ='" + txt_rolllno.Text + "' OR M.Lib_ID ='" + txt_rolllno.Text + "')";
                            Sql1 = Sql1 + "ORDER BY 'Return Date','Name' ";
                        }

                        else if (ddlselectfor.Text == "All")
                        {
                            Sql1 = "SELECT ISNULL(COUNT(*),0) ";
                            Sql1 = Sql1 + "FROM Borrow B,Registration R,Degree G,Library L ";
                            Sql1 = Sql1 + "WHERE (B.Roll_No = R.Roll_No OR B.Roll_No = R.Lib_ID) and B.Roll_No = R.Roll_No AND B.Lib_Code = L.Lib_Code ";
                            Sql1 = Sql1 + "AND R.Degree_Code = G.Degree_Code ";
                            Sql1 = Sql1 + "AND B.Is_Staff = 0 AND Return_Flag = 1 AND L.College_Code =" + collegeCode;
                            if (ddllibrary.Text != "All")
                                Sql1 = Sql1 + " AND B.Lib_Code ='" + ddllibrary.SelectedValue + "' ";
                            if (ddldept.Text != "All")
                                Sql1 = Sql1 + " AND R.Degree_Code =" + ddldept.SelectedValue;
                            if (cbfrom.Checked == true)
                                Sql1 = Sql1 + " AND Return_Date BETWEEN '" + fromdate1 + "' and '" + todate1 + "'";
                            if (chk_ovrngtiss.Checked == true)
                                Sql1 = Sql1 + " AND Due_Date-Borrow_Date = 1 ";
                            if (cbbatch.Checked == true)
                                Sql1 = Sql1 + " AND R.Batch_Year =" + ddlbatch.SelectedItem;
                            if (txt_rolllno.Text.Trim() != "")
                                Sql1 = Sql1 + " AND (R.Roll_NO ='" + txt_rolllno.Text + "' OR R.Lib_ID ='" + txt_rolllno.Text + "')";
                            ds1 = d2.select_method_wo_parameter(Sql1, "text");
                            string TotStud = ds1.Tables[0].Rows[0][0].ToString();

                            Sql1 = "SELECT COUNT(*) ";
                            Sql1 = Sql1 + "FROM Borrow B,StaffMaster M,StaffTrans T,HrDept_Master D,Library L ";
                            Sql1 = Sql1 + "WHERE (B.Roll_No = M.Staff_Code OR B.Roll_No = M.Lib_ID) and B.Roll_No = M.Staff_Code AND B.Lib_Code = L.Lib_Code ";
                            Sql1 = Sql1 + "AND M.Staff_Code = T.Staff_Code AND T.Dept_Code = D.Dept_Code AND M.College_Code = D.College_Code AND M.College_Code = L.College_Code ";
                            Sql1 = Sql1 + " AND T.Latestrec = 1 AND B.Is_Staff = 1 AND Return_Flag = 1 AND L.College_Code =" + collegeCode;
                            if (ddllibrary.Text != "All")
                                Sql1 = Sql1 + " AND B.Lib_Code ='" + ddllibrary.SelectedValue + "' ";
                            if (ddldept.Text != "All")
                                Sql1 = Sql1 + " AND R.Degree_Code =" + ddldept.SelectedValue;
                            if (cbfrom.Checked == true)
                                Sql1 = Sql1 + " AND Return_Date BETWEEN '" + fromdate1 + "' and '" + todate1 + "'";

                            if (chk_ovrngtiss.Checked == true)
                                Sql1 = Sql1 + " AND Due_Date-Borrow_Date = 1 ";

                            if (txt_rolllno.Text.Trim() != "")
                                Sql1 = Sql1 + " AND (M.Staff_Code ='" + txt_rolllno.Text + "' OR M.Lib_ID ='" + txt_rolllno.Text + "')";
                            ds1 = d2.select_method_wo_parameter(Sql1, "text");
                            string TotStaff = ds1.Tables[0].Rows[0][0].ToString();
                            Sql1 = "SELECT DISTINCT B.Acc_No as 'Access No',B.Roll_No,R.Stud_Name as 'Name',Token_No as 'Card No',CONVERT(Varchar,Borrow_Date,103) as 'Borrow Date',CONVERT(Varchar,Due_Date,103) as 'Due Date',CONVERT(Varchar,Return_Date,103) as 'Return Date',ISNULL(Issued_Time,'') as Issued_Time,ISNULL(Returned_Time,'') as 'Returned Time',Title as 'Title',Author as 'Author',B.Book_Returnby as 'Book Return By',B.Return_Type as 'Return Type',L.Lib_Name as 'Library name' ";
                            Sql1 = Sql1 + "FROM Borrow B,Registration R,Degree G,Library L ";
                            Sql1 = Sql1 + "WHERE (B.Roll_No = R.Roll_No OR B.Roll_No = R.Lib_ID) and B.Roll_No = R.Roll_No AND B.Lib_Code = L.Lib_Code ";
                            Sql1 = Sql1 + "AND R.Degree_Code = G.Degree_Code ";
                            Sql1 = Sql1 + "AND B.Is_Staff = 0 AND Return_Flag = 1 AND L.College_Code =" + collegeCode;
                            if (ddllibrary.Text != "All")
                                Sql1 = Sql1 + " AND B.Lib_Code ='" + ddllibrary.SelectedValue + "' ";
                            if (ddldept.Text != "All")
                                Sql1 = Sql1 + " AND R.Degree_Code =" + ddldept.SelectedValue;
                            if (cbfrom.Checked == true)
                                Sql1 = Sql1 + " AND Return_Date BETWEEN '" + fromdate1 + "' and '" + todate1 + "'";
                            if (chk_ovrngtiss.Checked == true)
                                Sql1 = Sql1 + " AND Due_Date-Borrow_Date = 1 ";
                            if (cbbatch.Checked == true)
                                Sql1 = Sql1 + " AND R.Batch_Year =" + ddlbatch.SelectedItem;
                            if (txt_rolllno.Text.Trim() != "")
                                Sql1 = Sql1 + " AND (R.Roll_NO ='" + txt_rolllno.Text + "' OR R.Lib_ID ='" + txt_rolllno.Text + "')";
                            Sql1 = Sql1 + " UNION ALL ";
                            Sql1 = Sql1 + "SELECT DISTINCT B.Acc_No as 'Access No',B.Roll_No,M.Staff_Name as 'Name',Token_No as 'Card No',CONVERT(Varchar,Borrow_Date,103) as 'Borrow Date',CONVERT(Varchar,Due_Date,103) as 'Due Date',CONVERT(Varchar,Return_Date,103) as 'Return Date',ISNULL(Issued_Time,'') as Issued_Time,ISNULL(Returned_Time,'') as 'Returned Time',Title as 'Title',Author as 'Author',B.Book_Returnby as 'Book Return By',B.Return_Type as 'Return Type',L.Lib_Name as 'Library name' ";
                            Sql1 = Sql1 + "FROM Borrow B,StaffMaster M,StaffTrans T,HrDept_Master D,Library L ";
                            Sql1 = Sql1 + "WHERE (B.Roll_No = M.Staff_Code OR B.Roll_No = M.Lib_ID) and B.Roll_No = M.Staff_Code AND B.Lib_Code = L.Lib_Code ";
                            Sql1 = Sql1 + "AND M.Staff_Code = T.Staff_Code AND T.Dept_Code = D.Dept_Code AND M.College_Code = D.College_Code AND M.College_Code = L.College_Code ";
                            Sql1 = Sql1 + "AND B.Is_Staff = 1 AND Return_Flag = 1 AND L.College_Code =" + collegeCode;
                            if (ddllibrary.Text != "All")
                                Sql1 = Sql1 + " AND B.Lib_Code ='" + ddllibrary.SelectedValue + "' ";

                            if (ddldept.Text != "All")
                                Sql1 = Sql1 + " AND R.Degree_Code =" + ddldept.SelectedValue;

                            if (cbfrom.Checked == true)
                                Sql1 = Sql1 + " AND Return_Date BETWEEN '" + fromdate1 + "' and '" + todate1 + "'";

                            if (chk_ovrngtiss.Checked == true)
                                Sql1 = Sql1 + " AND Due_Date-Borrow_Date = 1 ";

                            if (txt_rolllno.Text.Trim() != "")
                                Sql1 = Sql1 + " AND (M.Staff_Code ='" + txt_rolllno.Text + "' OR M.Lib_ID ='" + txt_rolllno.Text + "')";
                            Sql1 = Sql1 + "ORDER BY 'Return Date','Name' ";
                        }
                        ds1 = d2.select_method_wo_parameter(Sql1, "text");

                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            int colcount = 0;
                            transrepo.Columns.Add("SNo", typeof(string));
                            transrepo.Columns.Add("Access No", typeof(string));
                            transrepo.Columns.Add("Roll No", typeof(string));
                            transrepo.Columns.Add("Name", typeof(string));
                            transrepo.Columns.Add("Card No", typeof(string));
                            transrepo.Columns.Add("Borrow Date", typeof(string));
                            transrepo.Columns.Add("Due Date", typeof(string));
                            transrepo.Columns.Add("Return Date", typeof(string));
                            transrepo.Columns.Add("Issued_Time", typeof(string));
                            transrepo.Columns.Add("Returned Time", typeof(string));
                            transrepo.Columns.Add("Title", typeof(string));
                            transrepo.Columns.Add("Author", typeof(string));
                            transrepo.Columns.Add("Book Return By", typeof(string));
                            transrepo.Columns.Add("Return Type", typeof(string));
                            transrepo.Columns.Add("Library name", typeof(string));


                            int i = 0;
                            int SNo = 0;
                            for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                            {
                                SNo++;
                                dr = transrepo.NewRow();
                                dr["SNo"] = Convert.ToString(SNo);
                                dr["Access No"] = ds1.Tables[0].Rows[r]["Access No"].ToString();
                                dr["Roll No"] = ds1.Tables[0].Rows[r]["Roll_No"].ToString();
                                dr["Name"] = ds1.Tables[0].Rows[r]["Name"].ToString();
                                dr["Card No"] = ds1.Tables[0].Rows[r]["Card No"].ToString();
                                dr["Borrow Date"] = ds1.Tables[0].Rows[r]["Borrow Date"].ToString();
                                dr["Due Date"] = ds1.Tables[0].Rows[r]["Due Date"].ToString();
                                dr["Return Date"] = ds1.Tables[0].Rows[r]["Return Date"].ToString();
                                dr["Issued_Time"] = ds1.Tables[0].Rows[r]["Issued_Time"].ToString();
                                dr["Returned Time"] = ds1.Tables[0].Rows[r]["Returned Time"].ToString();
                                dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                                dr["Author"] = ds1.Tables[0].Rows[r]["Author"].ToString();
                                dr["Book Return By"] = ds1.Tables[0].Rows[r]["Book Return By"].ToString();
                                dr["Return Type"] = ds1.Tables[0].Rows[r]["Return Type"].ToString();
                                dr["Library Name"] = ds1.Tables[0].Rows[r]["Library Name"].ToString();
                                transrepo.Rows.Add(dr);
                            }
                            gridview2.DataSource = transrepo;
                            gridview2.DataBind();

                            gridview2.Visible = true;
                        }
                    }
                    else
                    {
                        if (ddlselectfor.Text == "Student")
                        {
                            Sql1 = "SELECT COUNT(*) as 'Books'";
                            Sql1 = Sql1 + "FROM Borrow B,Registration R,Degree G,Library L ";
                            Sql1 = Sql1 + "WHERE (B.Roll_No = R.Roll_No OR B.Roll_No = R.Lib_ID) and B.Roll_No = R.Roll_No AND B.Lib_Code = L.Lib_Code ";
                            Sql1 = Sql1 + "AND R.Degree_Code = G.Degree_Code ";
                            Sql1 = Sql1 + "AND B.Is_Staff = 0 AND Return_Flag = 1 AND Return_Type = 'BOK' AND L.College_Code =" + collegeCode;
                            if (ddllibrary.Text != "All")
                                Sql1 = Sql1 + " AND B.Lib_Code ='" + ddllibrary.SelectedValue + "' ";
                            if (ddldept.Text != "All")
                                Sql1 = Sql1 + " AND R.Degree_Code =" + ddldept.SelectedValue;
                            if (cbfrom.Checked == true)
                                Sql1 = Sql1 + " AND Return_Date BETWEEN '" + fromdate1 + "' and '" + todate1 + "'";
                            if (chk_ovrngtiss.Checked == true)
                                Sql1 = Sql1 + " AND Due_Date-Borrow_Date = 1 ";
                            if (cbbatch.Checked == true)
                                Sql1 = Sql1 + " AND R.Batch_Year =" + ddlbatch.SelectedItem;
                        }
                        else if (ddlselectfor.Text == "Staff")
                        {
                            Sql1 = "SELECT COUNT(*) as 'Books' ";
                            Sql1 = Sql1 + "FROM Borrow B,StaffMaster M,StaffTrans T,HrDept_Master D,Library L ";
                            Sql1 = Sql1 + "WHERE (B.Roll_No = M.Staff_Code OR B.Roll_No = M.Lib_ID) and B.Roll_No = M.Staff_Code AND B.Lib_Code = L.Lib_Code ";
                            Sql1 = Sql1 + "AND M.Staff_Code = T.Staff_Code AND T.Dept_Code = D.Dept_Code AND M.College_Code = D.College_Code AND M.College_Code = L.College_Code ";
                            Sql1 = Sql1 + "AND T.Latestrec = 1 AND B.Is_Staff = 1 AND Return_Flag = 1 AND Return_Type = 'BOK' AND L.College_Code =" + collegeCode;
                            if (ddllibrary.Text != "All")
                                Sql1 = Sql1 + " AND B.Lib_Code ='" + ddllibrary.SelectedValue + "' ";
                            if (ddldept.Text != "All")
                                Sql1 = Sql1 + " AND R.Degree_Code =" + ddldept.SelectedValue;
                            if (cbfrom.Checked == true)
                                Sql1 = Sql1 + " AND Return_Date BETWEEN '" + fromdate1 + "' and '" + todate1 + "'";
                            if (chk_ovrngtiss.Checked == true)
                                Sql1 = Sql1 + " AND Due_Date-Borrow_Date = 1 ";
                        }
                        else if (ddlselectfor.Text == "All")
                        {
                            Sql1 = "select COUNT(*) as 'Books' from borrow b,Library l where  Return_Flag = 1 AND Return_Type = 'BOK' and l.lib_code=b.lib_code and l.college_code='" + collegeCode + "'";
                            if (ddllibrary.Text != "All")
                                Sql1 = Sql1 + " AND B.Lib_Code ='" + ddllibrary.SelectedValue + "' ";
                        }
                        ds1 = d2.select_method_wo_parameter(Sql1, "text");

                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            transrepo.Columns.Add("SNo", typeof(string));
                            transrepo.Columns.Add("Books", typeof(string));
                            transrepo.Columns.Add("Non Book", typeof(string));
                            transrepo.Columns.Add("Total", typeof(string));


                            int SNo = 0;
                            for (int r = 0; r < ds1.Tables[0].Rows.Count; r++)
                            {
                                SNo++;
                                dr = transrepo.NewRow();
                                dr["SNo"] = Convert.ToString(SNo);
                                dr["Books"] = ds1.Tables[0].Rows[r]["Books"].ToString();
                                dr["Non Book"] = 0;
                                dr["Total"] = ds1.Tables[0].Rows[r]["Books"].ToString();
                                transrepo.Rows.Add(dr);
                            }
                            gridview2.DataSource = transrepo;
                            gridview2.DataBind();
                            gridview2.Visible = true;
                        }
                    }
                }
                #endregion

                #region Renewal Books added by rajasekar 18/5/2018
                else if (ddlreporttype.SelectedIndex == 2)
                {
                    if (ddllibrary.Text != "All")
                        libcode = ddllibrary.SelectedValue;
                    else
                        libcode = "%";
                    if (cbfrom.Checked == true)
                        strDate = "  and  borrow_date between '" + fromdate1 + "' and '" + todate1 + "'";
                    else
                        strDate = "";
                    if (chk_ovrngtiss.Checked == true)
                        strovernight = "  and due_date-borrow_date = 1 ";
                    else
                        strovernight = "";
                    if (ddldept.Text == "All")
                        strDegreecode = "";
                    else
                        strDegreecode = "  and r.degree_code=" + ddldept.SelectedValue + "";
                    strboktype = "";
                    if (ddlselectfor.Text == "Student")
                    {
                        Sql1 = "Select DISTINCT acc_no as 'Access No',token_no as 'Card No',B.Roll_No as 'Roll No',R.Stud_Name as 'Student Name',convert(varchar,borrow_date,103) as 'Issue Date',convert(varchar,due_date,103) as 'Due Date',case when B.return_flag=0 then 'NR' else convert(varchar,return_date,103) end  as 'Return Date',title as 'Title',author as 'Author',B.book_returnby as 'Book Return By',B.return_type as 'Return Type',L.lib_name as 'Library Name'";
                        Sql1 = Sql1 + "FROM Borrow B,Registration R,Library L,Degree G,Department D ";
                        Sql1 = Sql1 + "WHERE (B.Roll_No = R.Roll_No OR B.Roll_No = R.Lib_ID) and B.Roll_No = R.Roll_No AND B.Lib_Code = L.Lib_Code ";
                        Sql1 = Sql1 + "AND R.Degree_Code = G.Degree_Code AND G.Dept_Code = D.Dept_Code ";
                        Sql1 = Sql1 + "AND B.Is_Staff = 0 AND RenewFlag = 1 ";
                        if (ddllibrary.Text != "All")
                            Sql1 = Sql1 + " AND B.Lib_Code ='" + libcode + "' ";
                        Sql1 = Sql1 + strDate + strovernight + strDegreecode + strboktype;
                    }
                    else if (ddlselectfor.Text == "Staff")
                    {
                        Sql1 = "Select DISTINCT acc_no as 'Access No',token_no as 'Card No',B.Roll_No as 'Staff Code',Staff_Name as 'Staff Name',convert(varchar,borrow_date,103) as 'Issue Date',convert(varchar,due_date,103) as 'Due Date',case when B.return_flag=0 then 'NR' else convert(varchar,return_date,103) end  as 'Return Date',title as 'Title',author as 'Author',B.book_returnby as 'Book Return By',B.return_type as 'Return Type',L.lib_name as 'Library Name'";
                        Sql1 = Sql1 + "FROM Borrow B,StaffMaster M,StaffTrans T,HrDept_Master D,Library L ";
                        Sql1 = Sql1 + "WHERE (B.Roll_No = M.Staff_Code OR B.Roll_No = M.Lib_ID) and B.Roll_No = M.Staff_Code AND M.Staff_Code = T.Staff_Code ";
                        Sql1 = Sql1 + "AND T.Dept_Code = D.Dept_Code AND B.Lib_Code = L.Lib_Code ";
                        Sql1 = Sql1 + "AND B.Is_Staff = 1 AND RenewFlag = 1 AND T.Latestrec = 1 ";
                        if (ddldept.Text != "All")
                            Sql1 = Sql1 + "AND D.Dept_Name ='" + ddldept.SelectedValue + "";
                        Sql1 = Sql1 + strDate + strovernight + strboktype;
                    }
                    else if (ddlselectfor.Text == "All")
                    {
                        Sql1 = "Select DISTINCT acc_no as 'Access No',token_no as 'Card No',B.Roll_No as 'Roll No/Staff Code',R.Stud_Name as 'Name',convert(varchar,borrow_date,103) as 'Issue Date',convert(varchar,due_date,103) as 'Due Date',case when B.return_flag=0 then 'NR' else convert(varchar,return_date,103) end  as 'Return Date',title as 'Title',author as 'Author',B.book_returnby as 'Book Return By',B.return_type as 'Return Type',L.lib_name as 'Library Name'";
                        Sql1 = Sql1 + "FROM Borrow B,Registration R,Library L,Degree G,Department D ";
                        Sql1 = Sql1 + "WHERE (B.Roll_No = R.Roll_No OR B.Roll_No = R.Lib_ID) and B.Roll_No = R.Roll_No AND B.Lib_Code = L.Lib_Code ";
                        Sql1 = Sql1 + "AND R.Degree_Code = G.Degree_Code AND G.Dept_Code = D.Dept_Code ";
                        Sql1 = Sql1 + "AND B.Is_Staff = 0 AND RenewFlag = 1 ";
                        if (ddllibrary.Text != "All")
                            Sql1 = Sql1 + " AND B.Lib_Code ='" + libcode + "' ";
                        Sql1 = Sql1 + strDate + strovernight + strDegreecode + strboktype;
                        Sql1 = Sql1 + " UNION ALL ";
                        Sql1 = Sql1 + "Select DISTINCT acc_no as 'Access No',token_no as 'Card No',B.Roll_No as 'Roll No/Staff Code',Staff_Name as 'Name',convert(varchar,borrow_date,103) as 'Issue Date',convert(varchar,due_date,103) as 'Due Date',case when B.return_flag=0 then 'NR' else convert(varchar,return_date,103) end  as 'Return Date',title as 'Title',author as 'Author',B.book_returnby as 'Book Return By',B.return_type as 'Return Type',L.lib_name as 'Library Name'";
                        Sql1 = Sql1 + "FROM Borrow B,StaffMaster M,StaffTrans T,HrDept_Master D,Library L ";
                        Sql1 = Sql1 + "WHERE (B.Roll_No = M.Staff_Code OR B.Roll_No = M.Lib_ID) and B.Roll_No = M.Staff_Code AND M.Staff_Code = T.Staff_Code ";
                        Sql1 = Sql1 + "AND T.Dept_Code = D.Dept_Code AND B.Lib_Code = L.Lib_Code ";
                        Sql1 = Sql1 + "AND B.Is_Staff = 1 AND RenewFlag = 1 AND T.Latestrec = 1 ";
                        if (ddldept.Text != "All")
                            Sql1 = Sql1 + "AND D.Dept_Name ='" + ddldept.SelectedValue + "";
                        Sql1 = Sql1 + strDate + strovernight + strboktype;
                    }
                    ds1 = d2.select_method_wo_parameter(Sql1, "text");

                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        transrepo.Columns.Add("SNo", typeof(string));
                        transrepo.Columns.Add("Access No", typeof(string));
                        transrepo.Columns.Add("Card No", typeof(string));
                        if (ddlselectfor.Text == "Student")
                        {
                            transrepo.Columns.Add("Roll No", typeof(string));
                            transrepo.Columns.Add("Student Name", typeof(string));
                        }
                        else if (ddlselectfor.Text == "Staff")
                        {
                            transrepo.Columns.Add("Staff Code", typeof(string));
                            transrepo.Columns.Add("Staff Name", typeof(string));
                        }
                        else if (ddlselectfor.Text == "All")
                        {
                            transrepo.Columns.Add("Roll No/Staff Code", typeof(string));
                            transrepo.Columns.Add("Name", typeof(string));
                        }
                        transrepo.Columns.Add("Issue Date", typeof(string));
                        transrepo.Columns.Add("Due Date", typeof(string));
                        transrepo.Columns.Add("Return Date", typeof(string));
                        transrepo.Columns.Add("Title", typeof(string));
                        transrepo.Columns.Add("Author", typeof(string));
                        transrepo.Columns.Add("Book Return By", typeof(string));
                        transrepo.Columns.Add("Return Type", typeof(string));
                        transrepo.Columns.Add("Library Name", typeof(string));





                        int i = 0;
                        int SNo = 0;
                        for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                        {
                            SNo++;
                            dr = transrepo.NewRow();
                            dr["SNo"] = Convert.ToString(SNo);
                            dr["Access No"] = ds1.Tables[0].Rows[r]["Access No"].ToString();
                            dr["Card No"] = ds1.Tables[0].Rows[r]["Card No"].ToString();

                            if (ddlselectfor.Text == "Student")
                            {
                                dr["Roll No"] = ds1.Tables[0].Rows[r]["Roll No"].ToString();
                                dr["Student Name"] = ds1.Tables[0].Rows[r]["Student Name"].ToString();
                            }
                            else if (ddlselectfor.Text == "Staff")
                            {
                                dr["Staff Code"] = ds1.Tables[0].Rows[r]["Staff Code"].ToString();
                                dr["Staff Name"] = ds1.Tables[0].Rows[r]["Staff Name"].ToString();
                            }
                            else if (ddlselectfor.Text == "All")
                            {
                                dr["Roll No/Staff Code"] = ds1.Tables[0].Rows[r]["Roll No/Staff Code"].ToString();
                                dr["Name"] = ds1.Tables[0].Rows[r]["Name"].ToString();
                            }
                            dr["Issue Date"] = ds1.Tables[0].Rows[r]["Issue Date"].ToString();
                            dr["Due Date"] = ds1.Tables[0].Rows[r]["Due Date"].ToString();
                            dr["Return Date"] = ds1.Tables[0].Rows[r]["Return Date"].ToString();
                            dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                            dr["Author"] = ds1.Tables[0].Rows[r]["Author"].ToString();
                            dr["Book Return By"] = ds1.Tables[0].Rows[r]["Book Return By"].ToString();
                            dr["Return Type"] = ds1.Tables[0].Rows[r]["Return Type"].ToString();
                            dr["Library Name"] = ds1.Tables[0].Rows[r]["Library Name"].ToString();
                            transrepo.Rows.Add(dr);
                        }
                        gridview2.DataSource = transrepo;
                        gridview2.DataBind();

                        gridview2.Visible = true;
                    }
                }
                #endregion

                #region Lost Books added by rajasekar 16/5/2018

                else if (ddlreporttype.SelectedIndex == 3)
                {
                    if (rbllostbooks.SelectedValue == "WithFine")
                    {
                        if (cbfrom.Checked == true)
                            strDate = " and cal_date between '" + fromdate1 + "' and '" + todate1 + "'";
                        else
                            strDate = "";
                        if (ddllibrary.Text != "All")
                            libcode = " and fine_details.lib_code=" + ddllibrary.SelectedValue + "";
                        if (ddlselectfor.Text == "Student")
                        {
                            if (ddldept.Text == "All")
                                strDegreecode = "";
                            else
                                strDegreecode = " and department.dept_name=" + ddldept.SelectedValue + "";
                            Sql1 = "Select DISTINCT fine_details.acc_no as 'Access No',borrow.title as 'Book Title' ,borrow.author as 'Author Name',borrow.stud_name as 'Lost By' ,borrow.roll_no as 'Roll No',isnull(borrow.mode,0) as 'Mode',library.lib_name as 'Library Name' FROM borrow, fine_details, library, registration, department,degree WHERE library.lib_code = fine_details.lib_code AND fine_details.description IN ('Lost Fine','Lost and Overdue Fine') and Registration.Roll_No=borrow.roll_no and Registration.Roll_No=Fine_Details.roll_no";
                            Sql1 = Sql1 + " AND fine_details.acc_no = borrow.acc_no AND fine_details.roll_no = borrow.roll_no " + libcode + " AND borrow.lib_code = fine_details.lib_code AND department.dept_code=degree.dept_code and degree.degree_code= registration.degree_code" + strDate + " " + strDegreecode + " ";// AND fine_details.booktype = borrow.return_type
                        }
                        else if (ddlselectfor.Text == "Staff")
                        {
                            if (ddldept.Text == "All")
                                strDegreecode = "";
                            else
                                strDegreecode = " and hrdept_master.dept_name=" + ddldept.SelectedValue + "";
                            Sql1 = "Select DISTINCT fine_details.acc_no as 'Access No',borrow.title as 'Book Title',borrow.author as 'Author Name',borrow.stud_name as 'Lost By',borrow.roll_no as 'Roll No',isnull(borrow.mode,0) as 'Mode',library.lib_name as 'Library Name' FROM borrow, fine_details, library,staffmaster,stafftrans,hrdept_master  WHERE library.lib_code = fine_details.lib_code AND fine_details.description IN ('Lost Fine','Lost and Overdue Fine')";
                            Sql1 = Sql1 + " AND fine_details.acc_no = borrow.acc_no AND fine_details.roll_no = borrow.roll_no " + libcode + " AND borrow.lib_code = fine_details.lib_code  AND staffmaster.staff_code=stafftrans.staff_code and stafftrans.dept_code=hrdept_master.dept_code and stafftrans.latestrec=1 " + strDate + " " + strDegreecode + "";//AND fine_details.booktype = borrow.return_type
                        }
                        else if (ddlselectfor.Text == "All")
                        {
                            if (ddldept.Text == "All")
                                strDegreecode = "";
                            else
                                strDegreecode = " and department.dept_name=" + ddldept.SelectedValue + "";

                            Sql1 = "Select DISTINCT fine_details.acc_no as 'Access No',borrow.title as 'Book Title' ,borrow.author as 'Author Name',borrow.stud_name as 'Lost By' ,borrow.roll_no as 'Roll No',isnull(borrow.mode,0) as 'Mode',library.lib_name as 'Library Name' FROM borrow, fine_details, library, registration, department,degree WHERE library.lib_code = fine_details.lib_code AND fine_details.description IN ('Lost Fine','Lost and Overdue Fine') and Registration.Roll_No=borrow.roll_no and Registration.Roll_No=Fine_Details.roll_no";
                            Sql1 = Sql1 + " AND fine_details.acc_no = borrow.acc_no AND fine_details.roll_no = borrow.roll_no " + libcode + " AND borrow.lib_code = fine_details.lib_code  AND department.dept_code=degree.dept_code and degree.degree_code= registration.degree_code" + strDate + " " + strDegreecode + "";//AND fine_details.booktype = borrow.return_type 

                            if (ddldept.Text == "All")
                                strDeptname = "";
                            else
                                strDeptname = " and hrdept_master.dept_name=" + ddldept.SelectedValue + "";
                            Sql1 = Sql1 + " UNION ALL ";

                            Sql1 = Sql1 + "Select DISTINCT fine_details.acc_no as 'Access No',borrow.title as 'Book Title',borrow.author as 'Author Name',borrow.stud_name as 'Lost By',borrow.roll_no as 'Roll No',isnull(borrow.mode,0) as 'Mode',library.lib_name as 'Library Name' FROM borrow, fine_details, library,staffmaster,stafftrans,hrdept_master  WHERE library.lib_code = fine_details.lib_code AND fine_details.description IN ('Lost Fine','Lost and Overdue Fine')";
                            Sql1 = Sql1 + " AND fine_details.acc_no = borrow.acc_no AND fine_details.roll_no = borrow.roll_no " + libcode + " AND borrow.lib_code = fine_details.lib_code AND fine_details.booktype = borrow.return_type AND staffmaster.staff_code=stafftrans.staff_code and stafftrans.dept_code=hrdept_master.dept_code and stafftrans.latestrec=1 " + strDate + " " + strDeptname + "";
                        }
                        ds1 = d2.select_method_wo_parameter(Sql1, "text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            transrepo.Columns.Add("SNo", typeof(string));
                            transrepo.Columns.Add("Access No", typeof(string));
                            transrepo.Columns.Add("Book Title", typeof(string));
                            transrepo.Columns.Add("Author Name", typeof(string));
                            transrepo.Columns.Add("Lost By", typeof(string));
                            transrepo.Columns.Add("Roll No", typeof(string));
                            transrepo.Columns.Add("Mode", typeof(string));
                            transrepo.Columns.Add("Library Name", typeof(string));



                            int i = 0;
                            int SNo = 0;
                            for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                            {
                                SNo++;
                                dr = transrepo.NewRow();
                                dr["SNo"] = Convert.ToString(SNo);
                                dr["Access No"] = ds1.Tables[0].Rows[r]["Access No"].ToString();
                                dr["Book Title"] = ds1.Tables[0].Rows[r]["Book Title"].ToString();
                                dr["Author Name"] = ds1.Tables[0].Rows[r]["Author Name"].ToString();
                                dr["Lost By"] = ds1.Tables[0].Rows[r]["Lost By"].ToString();
                                dr["Roll No"] = ds1.Tables[0].Rows[r]["Roll No"].ToString();
                                dr["Mode"] = ds1.Tables[0].Rows[r]["Mode"].ToString();
                                dr["Library Name"] = ds1.Tables[0].Rows[r]["Library Name"].ToString();
                                transrepo.Rows.Add(dr);
                            }
                            gridview2.DataSource = transrepo;
                            gridview2.DataBind();

                            gridview2.Visible = true;
                        }

                    }
                    else if (rbllostbooks.SelectedValue == "ReplacebyNewBook")
                    {
                        Sql1 = "";
                        Sql1 = "Select distinct borrow.acc_no as 'Access No',borrow.title as 'Title',borrow.author as 'Author',borrow.stud_name as 'Name',borrow.roll_no as 'Roll No',borrow.mode 'Mode',library.lib_name as 'Library Name'";

                        if (cbfrom.Checked == true)
                            strDate = " and cal_date between '" + fromdate1 + "' and '" + todate1 + "'";
                        else
                            strDate = "";

                        if (ddlselectfor.Text == "Student")
                        {
                            if (ddllibrary.Text != "All" && ddldept.Text != "All")
                            {
                                Sql1 = Sql1 + " FROM borrow, library, registration, department,degree,fine_details WHERE library.lib_code = borrow.lib_code ";
                                Sql1 = Sql1 + " and borrow.lib_code='" + ddllibrary.SelectedValue + " AND department.dept_code=degree.dept_code and degree.degree_code= registration.degree_code" + strDate + " AND department.dept_name = '" + ddldept.Text + "' and borrow.mode=1 and Registration.Roll_No=borrow.roll_no and Registration.Roll_No=Fine_Details.roll_no";
                            }
                            else if (ddllibrary.Text == "All" && ddldept.Text != "All")
                            {
                                Sql1 = Sql1 + " FROM borrow,library, registration, department,degree,fine_details WHERE library.lib_code = borrow.lib_code ";
                                Sql1 = Sql1 + "  AND department.dept_code=degree.dept_code   and degree.degree_code= registration.degree_code AND department.dept_name = '" + ddldept.Text + "' and borrow.mode=1 and Registration.Roll_No=borrow.roll_no and Registration.Roll_No=Fine_Details.roll_no";
                            }
                            else if (ddllibrary.Text != "All" && ddldept.Text == "All")
                            {
                                Sql1 = Sql1 + " FROM borrow,library, registration, department,degree,fine_details WHERE library.lib_code = borrow.lib_code";
                                Sql1 = Sql1 + " and borrow.lib_code='" + ddllibrary.SelectedValue + " AND department.dept_code=degree.dept_code" + strDate + " and degree.degree_code= registration.degree_code and borrow.mode=1 and Registration.Roll_No=borrow.roll_no and Registration.Roll_No=Fine_Details.roll_no";
                            }
                            else if (ddllibrary.Text == "All" && ddldept.Text == "All")
                            {
                                Sql1 = Sql1 + " FROM borrow,library, registration, department,degree,fine_details WHERE library.lib_code = borrow.lib_code";
                                Sql1 = Sql1 + " and degree.degree_code = registration.degree_code and borrow.mode=1 and Registration.Roll_No=borrow.roll_no and Registration.Roll_No=Fine_Details.roll_no";
                            }
                        }
                        else if (ddlselectfor.Text == "Staff")
                        {
                            if (ddllibrary.Text != "All" && ddldept.Text != "All")
                            {
                                Sql1 = Sql1 + " FROM borrow,library,staffmaster,stafftrans,hrdept_master,fine_details WHERE library.lib_code = borrow.lib_code ";
                                Sql1 = Sql1 + " AND borrow.lib_code='" + ddllibrary.SelectedValue + "' and staffmaster.staff_code=stafftrans.staff_code and stafftrans.dept_code=hrdept_master.dept_code and stafftrans.latestrec=1 " + strDate + " AND hrdept_master.dept_name = '" + ddldept.Text + "' and borrow.mode=1";
                            }
                            else if (ddllibrary.Text == "All" && ddldept.Text != "All")
                            {
                                Sql1 = Sql1 + " FROM borrow,library,staffmaster,stafftrans,hrdept_master,fine_details WHERE library.lib_code = borrow.lib_code";
                                Sql1 = Sql1 + " AND staffmaster.staff_code=stafftrans.staff_code and stafftrans.dept_code=hrdept_master.dept_code and stafftrans.latestrec=1 " + strDate + "  and borrow.mode=1 AND hrdept_master.dept_name = '" + ddldept.Text + "' ";
                            }
                            else if (ddllibrary.Text != "All" && ddldept.Text == "All")
                            {
                                Sql1 = Sql1 + " FROM borrow, library, staffmaster,stafftrans,hrdept_master,fine_details WHERE library.lib_code = borrow.lib_code";
                                Sql1 = Sql1 + " and borrow.lib_code='" + ddllibrary.SelectedValue + "' and staffmaster.staff_code=stafftrans.staff_code and stafftrans.dept_code=hrdept_master.dept_code  and borrow.mode=1 and stafftrans.latestrec=1 " + strDate + "";
                            }
                            else if (ddllibrary.Text == "All" && ddldept.Text == "All")
                            {
                                Sql1 = Sql1 + " FROM borrow, library, staffmaster,stafftrans,hrdept_master,fine_details WHERE library.lib_code = borrow.lib_code ";
                                Sql1 = Sql1 + " and staffmaster.staff_code=stafftrans.staff_code and stafftrans.dept_code=hrdept_master.dept_code  and borrow.mode=1 and stafftrans.latestrec=1 " + strDate + "";
                            }
                        }
                        if (ddlselectfor.Text != "All")
                        {
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                transrepo.Columns.Add("SNo", typeof(string));
                                transrepo.Columns.Add("Access No", typeof(string));
                                transrepo.Columns.Add("Title", typeof(string));
                                transrepo.Columns.Add("Author", typeof(string));
                                transrepo.Columns.Add("Name", typeof(string));
                                transrepo.Columns.Add("Roll No", typeof(string));
                                transrepo.Columns.Add("Mode", typeof(string));
                                transrepo.Columns.Add("Library Name", typeof(string));


                                int i = 0;
                                int SNo = 0;
                                for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                                {
                                    SNo++;
                                    dr = transrepo.NewRow();
                                    dr["SNo"] = Convert.ToString(SNo);
                                    dr["Access No"] = ds1.Tables[0].Rows[r]["Access No"].ToString();
                                    dr["Title"] = ds1.Tables[0].Rows[r]["Book Title"].ToString();
                                    dr["Author"] = ds1.Tables[0].Rows[r]["Author Name"].ToString();
                                    dr["Name"] = ds1.Tables[0].Rows[r]["Lost By"].ToString();
                                    dr["Roll No"] = ds1.Tables[0].Rows[r]["Roll No"].ToString();
                                    dr["Mode"] = ds1.Tables[0].Rows[r]["Mode"].ToString();
                                    dr["Library Name"] = ds1.Tables[0].Rows[r]["Library Name"].ToString();
                                    transrepo.Rows.Add(dr);
                                }
                                gridview2.DataSource = transrepo;
                                gridview2.DataBind();

                                gridview2.Visible = true;
                            }

                        }
                    }
                    else if (rbllostbooks.SelectedValue == "All")
                    {
                        if (cbfrom.Checked == true)
                            strDate = " and cal_date between '" + fromdate1 + "' and '" + todate1 + "'";
                        else
                            strDate = "";

                        if (ddllibrary.Text != "All")
                            libcode = " and fine_details.lib_code=" + ddllibrary.SelectedValue + "";

                        Sql11 = "Select DISTINCT fine_details.acc_no,borrow.title,borrow.author,borrow.stud_name,borrow.roll_no,isnull(borrow.mode,0) as Mode,library.lib_name";
                        if (ddlselectfor.Text == "Student")
                        {
                            if (ddldept.Text != "All")
                                strDeptname = " and department.dept_name='" + ddldept.SelectedValue + "'";

                            Sql1 = Sql11 + " FROM borrow, fine_details, library, registration, department,degree WHERE library.lib_code = fine_details.lib_code AND fine_details.description IN ('Lost Fine','Lost and Overdue Fine')";
                            Sql1 = Sql1 + " AND fine_details.acc_no = borrow.acc_no AND fine_details.roll_no = borrow.roll_no " + libcode + " AND borrow.lib_code = fine_details.lib_code   AND department.dept_code=degree.dept_code and Registration.Roll_No=borrow.roll_no and Registration.Roll_No=Fine_Details.roll_no and degree.degree_code= registration.degree_code" + strDate + " " + strDeptname + "";//AND fine_details.booktype = borrow.return_type
                            if (ddldept.Text != "All")
                                strDeptname = " and user_master.department='" + ddldept.SelectedValue + "'";
                            Sql1 = Sql1 + " union all " + Sql11 + " FROM borrow, fine_details, library, user_master WHERE library.lib_code = fine_details.lib_code AND fine_details.description IN ('Lost Fine','Lost and Overdue Fine') and user_master.is_staff=0";
                            Sql1 = Sql1 + " AND fine_details.acc_no = borrow.acc_no AND fine_details.roll_no = borrow.roll_no and fine_details.lib_code like '" + libcode + "' AND borrow.lib_code = fine_details.lib_code  AND user_master.user_id = borrow.roll_no " + strDate + " " + strDeptname + "";//AND fine_details.booktype = borrow.return_type

                        }
                        else if (ddlselectfor.Text == "Staff")
                        {
                            if (ddldept.Text != "All")
                                strDeptname = " and hrdept_master.dept_name='" + ddldept.SelectedValue + "'";
                            Sql1 = Sql11 + " FROM borrow, fine_details, library,staffmaster,stafftrans,hrdept_master  WHERE library.lib_code = fine_details.lib_code AND fine_details.description IN ('Lost Fine','Lost and Overdue Fine')";
                            Sql1 = Sql1 + " AND fine_details.acc_no = borrow.acc_no AND fine_details.roll_no = borrow.roll_no " + libcode + " AND borrow.lib_code = fine_details.lib_code  AND staffmaster.staff_code=stafftrans.staff_code and stafftrans.dept_code=hrdept_master.dept_code and stafftrans.latestrec=1 and borrow.is_staff=1 " + strDate + " " + strDeptname + "";
                            if (ddldept.Text != "All")
                                strDeptname = " and user_master.department='" + ddldept.SelectedValue + "'";
                            Sql1 = Sql1 + " Union all " + Sql11 + " FROM borrow, fine_details, library,user_master WHERE library.lib_code = fine_details.lib_code AND fine_details.description IN ('Lost Fine','Lost and Overdue Fine')";
                            Sql1 = Sql1 + " AND fine_details.acc_no = borrow.acc_no AND fine_details.roll_no = borrow.roll_no and fine_details.lib_code like '" + libcode + "' AND borrow.lib_code = fine_details.lib_code AND  user_master.user_id = borrow.roll_no " + strDate + " " + strDeptname + "";
                        }
                        else if (ddlselectfor.Text == "All")
                        {
                            if (ddldept.Text != "All")
                                strDeptname = " and department.dept_name='" + ddldept.SelectedValue + "'";
                            Sql1 = Sql11 + " FROM borrow, fine_details, library, registration, department,degree WHERE library.lib_code = fine_details.lib_code AND fine_details.description IN ('Lost Fine','Lost and Overdue Fine')";
                            Sql1 = Sql1 + " AND fine_details.acc_no = borrow.acc_no AND fine_details.roll_no = borrow.roll_no " + libcode + " AND borrow.lib_code = fine_details.lib_code AND department.dept_code=degree.dept_code and degree.degree_code= registration.degree_code and Registration.Roll_No=borrow.roll_no and Registration.Roll_No=Fine_Details.roll_no " + strDate + " " + strDeptname + "";
                            if (ddldept.Text != "All")
                                strDeptname = " and user_master.department='" + ddldept.SelectedValue + "'";

                            Sql1 = Sql1 + " union all " + Sql11 + " FROM borrow, fine_details, library, user_master WHERE library.lib_code = fine_details.lib_code AND fine_details.description IN ('Lost Fine','Lost and Overdue Fine') and user_master.is_staff=0";
                            Sql1 = Sql1 + " AND fine_details.acc_no = borrow.acc_no AND fine_details.roll_no = borrow.roll_no " + libcode + " AND borrow.lib_code = fine_details.lib_code AND user_master.user_id = borrow.roll_no " + strDate + " " + strDeptname + "";
                            if (ddldept.Text != "All")
                                strDeptname = " and hrdept_master.dept_name='" + ddldept.SelectedValue + "'";
                            Sql1 = Sql1 + " UNION ALL ";
                            Sql1 = Sql1 + Sql11 + " FROM borrow, fine_details, library,staffmaster,stafftrans,hrdept_master  WHERE library.lib_code = fine_details.lib_code AND fine_details.description IN ('Lost Fine','Lost and Overdue Fine')";
                            Sql1 = Sql1 + " AND fine_details.acc_no = borrow.acc_no AND fine_details.roll_no = borrow.roll_no " + libcode + " AND borrow.lib_code = fine_details.lib_code AND staffmaster.staff_code=stafftrans.staff_code and stafftrans.dept_code=hrdept_master.dept_code and stafftrans.latestrec=1 and borrow.is_staff=1 " + strDate + " " + strDeptname + "";
                            if (ddldept.Text != "All")
                                strDeptname = " and user_master.department='" + ddldept.SelectedValue + "'";
                            Sql1 = Sql1 + " Union all " + Sql11 + " FROM borrow, fine_details, library,user_master WHERE library.lib_code = fine_details.lib_code AND fine_details.description IN ('Lost Fine','Lost and Overdue Fine')";
                            Sql1 = Sql1 + " AND fine_details.acc_no = borrow.acc_no AND fine_details.roll_no = borrow.roll_no " + libcode + " AND borrow.lib_code = fine_details.lib_code AND  user_master.user_id = borrow.roll_no " + strDate + " " + strDeptname + "";
                        }
                        ds1 = d2.select_method_wo_parameter(Sql1, "text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                           
                            transrepo.Columns.Add("SNo", typeof(string));
                            transrepo.Columns.Add("Access No", typeof(string));
                            transrepo.Columns.Add("Title", typeof(string));
                            transrepo.Columns.Add("Author", typeof(string));
                            transrepo.Columns.Add("Name", typeof(string));
                            transrepo.Columns.Add("Roll No", typeof(string));
                            transrepo.Columns.Add("Mode", typeof(string));
                            transrepo.Columns.Add("Library Name", typeof(string));

                            int i = 0;
                            int SNo = 0;
                            for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                            {
                                SNo++;
                                dr = transrepo.NewRow();
                                dr["SNo"] = Convert.ToString(SNo);
                                dr["Access No"] = ds1.Tables[0].Rows[r]["acc_no"].ToString();
                                dr["Title"] = ds1.Tables[0].Rows[r]["title"].ToString();
                                dr["Author"] = ds1.Tables[0].Rows[r]["author"].ToString();
                                dr["Name"] = ds1.Tables[0].Rows[r]["stud_name"].ToString();
                                dr["Roll No"] = ds1.Tables[0].Rows[r]["roll_no"].ToString();
                                dr["Mode"] = ds1.Tables[0].Rows[r]["Mode"].ToString();
                                dr["Library Name"] = ds1.Tables[0].Rows[r]["lib_name"].ToString();
                                transrepo.Rows.Add(dr);
                            }
                            gridview2.DataSource = transrepo;
                            gridview2.DataBind();

                            gridview2.Visible = true;
                        }
                    }
                }
                #endregion

                #region Due Books

                else if (ddlreporttype.SelectedIndex == 4)
                {
                    if (cbfrom.Checked == true)
                    {
                        strDate = " AND Due_Date BETWEEN '" + fromdate1 + "' AND '" + todate1 + "'";
                    }
                    else
                    {
                        strDate = "";
                    }
                    if (ddldept.SelectedValue != "All")
                    {
                        if (ddlselectfor.SelectedValue == "Student")

                            strdept = " AND R.Degree_Code =" + ddldept.SelectedIndex;
                        else
                            strdept = " AND D.Dept_Name ='" + ddldept.SelectedValue + "'";
                    }
                    else
                    {
                        strdept = "";
                    }
                    if (ddlselectfor.SelectedValue == "Student")
                    {
                        Sql1 = "SELECT Acc_No AS 'Access No',Title AS 'Title',B.Token_No AS 'Card No',B.Author AS 'Author',B.Roll_No,B.Stud_Name AS 'Student Name',CONVERT(varchar(10),Borrow_Date,103) AS 'Issued Date',CONVERT(varchar(10),Due_Date,103) As 'Due_Date',L.Lib_Name AS 'Library Name' ";
                        Sql1 = Sql1 + " FROM Borrow B,Registration R,Library L ";
                        Sql1 = Sql1 + " WHERE B.Lib_Code = L.Lib_Code ANd (B.Roll_No = R.Roll_No or B.Roll_No = R.Lib_Id) and B.Roll_No = R.Roll_No";
                        Sql1 = Sql1 + " AND Is_Staff = 0 AND Return_Flag <> 1";
                        Sql1 = Sql1 + strDate + strdept;
                        if (ddllibrary.SelectedValue != "All")
                            Sql1 = Sql1 + " AND B.Lib_Code ='" + ddllibrary.SelectedIndex + "'";
                        //            if lblBatch.value = 1 
                        //Sql = Sql & vbCrLf & " AND Batch_Year ='" & dtpBatch.Year & "'"
                    }
                    else if (ddlselectfor.SelectedValue == "Staff")
                    {
                        Sql1 = "SELECT Acc_No AS 'Access No',Title AS 'Title',B.Token_No AS 'Card No',B.Author AS 'Author',B.Roll_No,B.Stud_Name AS 'Student Name',CONVERT(varchar(10),Borrow_Date,103) AS 'Issued Date',CONVERT(varchar(10),Due_Date,103) As 'Due_Date',L.Lib_Name AS 'Library Name' ";
                        Sql1 = Sql1 + " FROM Borrow B,StaffMaster M,StaffTrans T,HrDept_Master D,Library L ";
                        Sql1 = Sql1 + " WHERE B.Lib_Code = L.Lib_Code ANd (B.Roll_No = M.Staff_Code or B.Roll_No = M.Lib_Id) and B.Roll_No = M.Staff_Code";
                        Sql1 = Sql1 + " AND M.Staff_Code = T.Staff_Code AND T.Dept_Code = D.Dept_Code AND T.Latestrec = 1";
                        Sql1 = Sql1 + " AND Is_Staff = 1 AND Return_Flag <> 1";
                        Sql1 = Sql1 + strDate + strdept;
                        if (ddllibrary.SelectedValue != "All")
                            Sql1 = Sql1 + " AND B.Lib_Code ='" + ddllibrary.SelectedIndex + "'";
                    }
                    else if (ddlselectfor.SelectedValue == "All")
                    {
                        Sql1 = "SELECT Acc_No AS 'Access No',Title AS 'Title',B.Token_No AS 'Card No',B.Author AS 'Author',B.Roll_No,B.Stud_Name AS 'Student Name',CONVERT(varchar(10),Borrow_Date,103) AS 'Issued Date',CONVERT(varchar(10),Due_Date,103) As 'Due_Date',L.Lib_Name AS 'Library Name' ";
                        Sql1 = Sql1 + " FROM Borrow B,Registration R,Library L";
                        Sql1 = Sql1 + " WHERE B.Lib_Code = L.Lib_Code ANd (B.Roll_No = R.Roll_No or B.Roll_No = R.Lib_Id) and B.Roll_No = R.Roll_No";
                        Sql1 = Sql1 + " AND Is_Staff = 0 AND Return_Flag <> 1";
                        Sql1 = Sql1 + strDate + strdept;
                        if (ddllibrary.SelectedValue != "All")
                            Sql1 = Sql1 + " AND B.Lib_Code ='" + ddllibrary.SelectedIndex + "'";

                        Sql1 = Sql1 + " UNION ALL";
                        Sql1 = Sql1 + " SELECT Acc_No AS 'Access No',Title AS 'Title',B.Token_No AS 'Card No',B.Author AS 'Author',B.Roll_No,B.Stud_Name AS 'Student Name',CONVERT(varchar(10),Borrow_Date,103) AS 'Issued Date',CONVERT(varchar(10),Due_Date,103) As 'Due_Date',L.Lib_Name AS 'Library Name' ";
                        Sql1 = Sql1 + " FROM Borrow B,StaffMaster M,StaffTrans T,HrDept_Master D,Library L ";
                        Sql1 = Sql1 + " WHERE B.Lib_Code = L.Lib_Code ANd (B.Roll_No = M.Staff_Code or B.Roll_No = M.Lib_Id) and B.Roll_No = M.Staff_Code ";
                        Sql1 = Sql1 + " AND M.Staff_Code = T.Staff_Code AND T.Dept_Code = D.Dept_Code AND T.Latestrec = 1";
                        Sql1 = Sql1 + " AND Is_Staff = 1 AND Return_Flag <> 1";
                        Sql1 = Sql1 + strDate + strdept;
                        if (ddllibrary.SelectedValue != "All")
                            Sql1 = Sql1 + " AND B.Lib_Code ='" + ddllibrary.SelectedIndex + "'";
                    }
                    ds1 = d2.select_method_wo_parameter(Sql1, "text");
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        transrepo.Columns.Add("SNo", typeof(string));
                        transrepo.Columns.Add("Access No", typeof(string));
                        transrepo.Columns.Add("Title", typeof(string));
                        transrepo.Columns.Add("Card No", typeof(string));
                        transrepo.Columns.Add("Author", typeof(string));
                        transrepo.Columns.Add("Roll No", typeof(string));
                        transrepo.Columns.Add("Name", typeof(string));
                        transrepo.Columns.Add("Issued Date", typeof(string));
                        transrepo.Columns.Add("Due Date", typeof(string));
                        transrepo.Columns.Add("Library Name", typeof(string));


                    }
                    int i = 0;
                    int SNo = 0;
                    for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                    {
                        SNo++;
                        dr = transrepo.NewRow();
                        dr["SNo"] = Convert.ToString(SNo);
                        dr["Access No"] = ds1.Tables[0].Rows[r]["Access No"].ToString();
                        dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                        dr["Card No"] = ds1.Tables[0].Rows[r]["Card No"].ToString();
                        dr["Author"] = ds1.Tables[0].Rows[r]["Author"].ToString();
                        dr["Roll No"] = ds1.Tables[0].Rows[r]["Roll_No"].ToString();
                        dr["Name"] = ds1.Tables[0].Rows[r]["Student Name"].ToString();
                        dr["Issued Date"] = ds1.Tables[0].Rows[r]["Issued Date"].ToString();
                        dr["Due Date"] = ds1.Tables[0].Rows[r]["Due_Date"].ToString();
                        dr["Library Name"] = ds1.Tables[0].Rows[r]["Library Name"].ToString();
                        transrepo.Rows.Add(dr);
                    }
                    gridview2.DataSource = transrepo;
                    gridview2.DataBind();

                    gridview2.Visible = true;
                }
                #endregion

                #region Back Volume Details

                else if (ddlreporttype.SelectedIndex == 5)
                {
                    Sql1 = "";
                    if (cbfrom.Checked == true)
                    {
                        strDate = " and back_volume.access_date between '" + fromdate1 + "' AND '" + todate1 + "'";
                    }
                    else
                    {
                        strDate = "";
                    }
                    if (ddldept.SelectedValue == "All")
                    {
                        Sql1 = "Select access_code as 'Access Code',title as 'Title',convert(varchar,access_date,103) as 'Access Date',journal_year as 'Journal Year',library.lib_name as 'Library Name' from back_volume,library where back_volume.lib_code=library.lib_code " + strDate;
                    }
                    else if (ddldept.SelectedValue != "All")
                    {
                        Sql1 = "";
                        Sql1 = "Select access_code as 'Access Code',title as 'Title',convert(Varchar,back_volume.access_date,103) as 'Access Date',journal_year as 'Journal Year',library.lib_name as 'library Name' from back_volume,library,journal_master where  back_volume.lib_code=library.lib_code " + strDate + " and ";
                        Sql1 = Sql1 + "journal_master.journal_name = back_volume.periodicalname AND journal_master.lib_code = back_volume.lib_code AND back_volume.lib_code =library.lib_code  and journal_master.department='" + ddldept.Text + "'";
                    }
                    ds1 = d2.select_method_wo_parameter(Sql1, "text");
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        transrepo.Columns.Add("SNo", typeof(string));
                        transrepo.Columns.Add("Access Code", typeof(string));
                        transrepo.Columns.Add("Title", typeof(string));
                        transrepo.Columns.Add("Access Date", typeof(string));
                        transrepo.Columns.Add("Journal Year", typeof(string));
                        transrepo.Columns.Add("Library Name", typeof(string));



                        int i = 0;
                        int SNo = 0;
                        for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                        {
                            SNo++;
                            dr = transrepo.NewRow();
                            dr["SNo"] = Convert.ToString(SNo);
                            dr["Access Code"] = ds1.Tables[0].Rows[r]["Access Code"].ToString();
                            dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                            dr["Access Date"] = ds1.Tables[0].Rows[r]["Access Date"].ToString();
                            dr["Journal Year"] = ds1.Tables[0].Rows[r]["Journal Year"].ToString();
                            dr["Library Name"] = ds1.Tables[0].Rows[r]["Library Name"].ToString();
                            transrepo.Rows.Add(dr);
                        }
                        gridview2.DataSource = transrepo;
                        gridview2.DataBind();

                        gridview2.Visible = true;
                    }
                }
                #endregion

                #region Binding Details added by rajasekar 09/05/2018

                else if (ddlreporttype.SelectedIndex == 6)
                {
                    Sql1 = "";
                    if (cbfrom.Checked == true)
                    {
                        strDate = " and binding.binding_date between '" + fromdate1 + "' AND '" + todate1 + "'";
                    }
                    else
                    {
                        strDate = "";
                    }
                    if (ddldept.SelectedValue == "All")
                    {
                        strdept = "";
                    }
                    else if (ddldept.SelectedValue != "All")
                    {
                        strdept = " and bookdetails.dept_code='" + ddldept.Text + "' ";
                    }
                    if (ddltype.Text == "Books")
                    {
                        Sql1 = "Select bookdetails.acc_no as 'Access No',bookdetails.title as 'Title',bookdetails.author as 'Author',bookdetails.edition as 'Edition',binding.serial_no as 'Serial No',convert(varchar,binding.binding_date,103) as 'Binding Date',library.lib_name as 'Library Name',binding.cname as 'Company Name' from bookdetails,binding,library where library.lib_code=binding.lib_code " + strDate + " and bookdetails.lib_code=binding.lib_code and bookdetails.acc_no =binding.access_code" + strdept;
                    }
                    else if (ddltype.Text == "Periodicals")
                    {
                        Sql1 = "Select journal.journal_code as 'Journal Code',journal.title as 'Title',journal.access_code as 'Access Code',journal.dept_name as 'Department Name',journal.volume_no as 'Volume No',journal.issue_no as 'Issue No',binding.serial_no as 'Serial No',convert(varchar,binding.binding_date,103) as 'Binding Date',library.lib_name as 'Library Name',binding.cname as 'Company Name' from journal,binding,library where library.lib_code=binding.lib_code" + strDate + " and journal.lib_code=binding.lib_code and journal.access_code=binding.access_code" + strdept;
                    }
                    ds1 = d2.select_method_wo_parameter(Sql1, "text");
                    if (ddltype.Text == "Books")
                    {
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            transrepo.Columns.Add("SNo", typeof(string));
                            transrepo.Columns.Add("Access No", typeof(string));
                            transrepo.Columns.Add("Title", typeof(string));
                            transrepo.Columns.Add("Author", typeof(string));
                            transrepo.Columns.Add("Edition", typeof(string));
                            transrepo.Columns.Add("Serial No", typeof(string));
                            transrepo.Columns.Add("Binding Date", typeof(string));
                            transrepo.Columns.Add("Library Name", typeof(string));
                            transrepo.Columns.Add("Company Name", typeof(string));


                            int i = 0;
                            int SNo = 0;
                            for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                            {
                                SNo++;
                                dr = transrepo.NewRow();
                                dr["SNo"] = Convert.ToString(SNo);
                                dr["Access No"] = ds1.Tables[0].Rows[r]["Access No"].ToString();
                                dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                                dr["Author"] = ds1.Tables[0].Rows[r]["Author"].ToString();
                                dr["Edition"] = ds1.Tables[0].Rows[r]["Edition"].ToString();
                                dr["Serial No"] = ds1.Tables[0].Rows[r]["Serial No"].ToString();
                                dr["Binding Date"] = ds1.Tables[0].Rows[r]["Binding Date"].ToString();
                                dr["Library Name"] = ds1.Tables[0].Rows[r]["Library Name"].ToString();
                                dr["Company Name"] = ds1.Tables[0].Rows[r]["Company Name"].ToString();
                                transrepo.Rows.Add(dr);
                            }
                            gridview2.DataSource = transrepo;
                            gridview2.DataBind();

                            gridview2.Visible = true;
                        }
                    }
                    if (ddltype.Text == "Periodicals")
                    {
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            transrepo.Columns.Add("SNo", typeof(string));
                            transrepo.Columns.Add("Journal Code", typeof(string));
                            transrepo.Columns.Add("Title", typeof(string));
                            transrepo.Columns.Add("Access Code", typeof(string));
                            transrepo.Columns.Add("Department Name", typeof(string));
                            transrepo.Columns.Add("Volume No", typeof(string));
                            transrepo.Columns.Add("Serial No", typeof(string));
                            transrepo.Columns.Add("Binding Date", typeof(string));
                            transrepo.Columns.Add("Library Name", typeof(string));
                            transrepo.Columns.Add("Company Name", typeof(string));

                            int i = 0;
                            int SNo = 0;
                            for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                            {
                                SNo++;
                                dr = transrepo.NewRow();
                                dr["SNo"] = Convert.ToString(SNo);
                                dr["Journal Code"] = ds1.Tables[0].Rows[r]["Journal Code"].ToString();
                                dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                                dr["Access Code"] = ds1.Tables[0].Rows[r]["Access Code"].ToString();
                                dr["Department Name"] = ds1.Tables[0].Rows[r]["Department Name"].ToString();
                                dr["Volume No"] = ds1.Tables[0].Rows[r]["Volume No"].ToString();
                                dr["Issue No"] = ds1.Tables[0].Rows[r]["Issue No"].ToString();
                                dr["Serial No"] = ds1.Tables[0].Rows[r]["Serial No"].ToString();
                                dr["Binding Date"] = ds1.Tables[0].Rows[r]["Binding Date"].ToString();
                                dr["Library Name"] = ds1.Tables[0].Rows[r]["Library Name"].ToString();
                                dr["Company Name"] = ds1.Tables[0].Rows[r]["Company Name"].ToString();
                                transrepo.Rows.Add(dr);
                            }
                            gridview2.DataSource = transrepo;
                            gridview2.DataBind();

                            gridview2.Visible = true;
                        }
                    }
                    if (cbduplicateaccno.Checked == true)
                    {
                        transrepo.Columns.Add("SNo", typeof(string));
                        transrepo.Columns.Add("Access No", typeof(string));
                        transrepo.Columns.Add("Title", typeof(string));
                        transrepo.Columns.Add("Author", typeof(string));
                        transrepo.Columns.Add("Publisher", typeof(string));
                        transrepo.Columns.Add("Department", typeof(string));
                        transrepo.Columns.Add("Library Name", typeof(string));

                        Sql1 = "select '' as 'Select',Acc_No,Title,Author,Publisher,Dept_Code,Lib_Name,BookID ";
                        Sql1 = Sql1 + " from bookdetails,library ";
                        Sql1 = Sql1 + " where bookdetails.lib_code = library.lib_code and acc_no in ";
                        Sql1 = Sql1 + " (Select acc_no from bookdetails group by acc_no having count(acc_no) > 1) ";
                        if (tex_accnofrom.Text.Trim() != "" || txt_accnoto.Text.Trim() != "")
                        {
                            if (collegeName == "THE NEW COLLEGE (AUTONOMOUS) SHIFT I" || collegeName == "THE NEW COLLEGE (AUTONOMOUS) SHIFT II")
                                Sql1 = Sql1 + " and cast(substring(bookdetails.acc_no,4,len(acc_no)-3) as int) between '" + tex_accnofrom.Text + "' AND '" + txt_accnoto.Text + "' ";
                            else
                            {
                                Sql1 = Sql1 + "and case when isnumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                Sql1 = Sql1 + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + tex_accnofrom.Text + "' and '" + txt_accnoto.Text + "'";
                                Sql1 = Sql1 + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                Sql1 = Sql1 + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + tex_accnofrom.Text + "' and '" + txt_accnoto.Text + "'";
                            }
                        }
                        if (txt_acr.Text.Trim() != "")
                            Sql1 = Sql1 + " AND Left(Acc_No,3) ='" + txt_acr.Text + "' ";
                        if (ddllibrary.Text != "All")
                            Sql1 = Sql1 + " and lib_name ='" + ddllibrary.Text + "'";
                        if (ddldept.Text != "All")
                            Sql1 = Sql1 + " and Dept_Code ='" + ddldept.Text + "'";
                        if (collegeName == "THE NEW COLLEGE (AUTONOMOUS) SHIFT I" || collegeName == "THE NEW COLLEGE (AUTONOMOUS) SHIFT II")
                            Sql1 = Sql1 + "ORDER BY CONVERT(nvarchar(30),SUBSTRING(Acc_No,1,3)), CONVERT(int,SUBSTRING(Acc_No,4,len(acc_no)-3)) ";
                        else
                            Sql1 = Sql1 + " order by case when isnumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',bookdetails.acc_no)),len(bookdetails.acc_no))as int) end";
                        ds1 = d2.select_method_wo_parameter(Sql1, "text");
                        int row = 0;
                        int i = 0;
                        int SNo = 0;
                        for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                        {
                            SNo++;
                            dr = transrepo.NewRow();
                            dr["SNo"] = SNo;
                            dr["Access No"] = ds1.Tables[0].Rows[r]["Acc_No"].ToString();
                            dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                            dr["Author"] = ds1.Tables[0].Rows[r]["Author"].ToString();
                            dr["Publisher"] = ds1.Tables[0].Rows[r]["Publisher"].ToString();
                            dr["Department"] = ds1.Tables[0].Rows[r]["Dept_Code"].ToString();
                            dr["Library Name"] = ds1.Tables[0].Rows[r]["Lib_name"].ToString();
                            transrepo.Rows.Add(dr);
                        }
                        gridview2.DataSource = transrepo;
                        gridview2.DataBind();

                        gridview2.Visible = true;
                    }
                }
                #endregion

                #region Book Borrow Utility

                else if (ddlreporttype.SelectedIndex == 7)
                {
                    Sql1 = "";
                    if (cbfrom.Checked == true)
                    {
                        strDate = " AND Borrow_Date BETWEEN '" + fromdate1 + "' AND '" + todate1 + "'";
                    }
                    else
                    {
                        strDate = "";
                    }
                    if (ddldept.SelectedValue != "All")
                    {
                        if (ddlselectfor.SelectedValue == "Student")

                            strdept = " AND R.Degree_Code =" + ddldept.SelectedIndex;
                        else
                            strdept = " AND D.Dept_Name ='" + ddldept.SelectedValue + "'";
                    }
                    else
                    {
                        strdept = "";
                    }
                    if (ddlselectfor.Text == "Student")
                    {
                        Sql1 = "SELECT B.Roll_No as 'Roll No',B.Stud_Name,COUNT(B.Roll_No) AS 'No. of Books'";
                        Sql1 = Sql1 + " FROM Borrow B,Registration R,Library L ";
                        Sql1 = Sql1 + " WHERE B.Lib_Code = L.Lib_Code and B.Roll_No = R.Roll_No ANd (B.Roll_No = R.Roll_No or B.Roll_No = R.Lib_Id)";
                        Sql1 = Sql1 + strDate + strdept;
                        if (ddllibrary.Text != "All")
                        {
                            Sql1 = Sql1 + " AND B.Lib_Code ='" + ddllibrary.SelectedIndex + "'";
                            Sql1 = Sql1 + " GROUP BY B.Lib_Code,B.Roll_No,B.Stud_Name";
                        }
                        else
                            Sql1 = Sql1 + " GROUP BY B.Roll_No,B.Stud_Name";

                        if (ddlaccno.SelectedIndex == 0)
                            Sql1 = Sql1 + " HAVING COUNT(B.Roll_No) > " + txt_accno.Text;
                        else if (ddlaccno.SelectedIndex == 1)
                            Sql1 = Sql1 + " HAVING COUNT(B.Roll_No) < " + txt_accno.Text;
                        else if (ddlaccno.SelectedIndex == 2)
                            Sql1 = Sql1 + " HAVING COUNT(B.Roll_No) = " + txt_accno.Text;
                        else if (ddlaccno.SelectedIndex == 3)
                            Sql1 = Sql1 + " HAVING COUNT(B.Roll_No) >= " + txt_accno.Text + " AND COUNT(B.Roll_No) <= " + txt_accno2.Text;
                        Sql1 = Sql1 + " ORDER BY Count(B.Roll_No) Desc";
                    }
                    else if (ddlselectfor.Text == "Staff")
                    {
                        Sql1 = "SELECT B.Roll_No as 'Roll No',B.Stud_Name,COUNT(B.Roll_No) AS 'No. of Books'";
                        Sql1 = Sql1 + " FROM Borrow B,StaffMaster M,StaffTrans T,HrDept_Master D,Library L ";
                        Sql1 = Sql1 + " WHERE (B.Roll_No = M.Staff_Code OR B.Roll_No = M.Lib_Id) and B.Roll_No = M.Staff_Code AND M.Staff_Code = T.Staff_Code ";
                        Sql1 = Sql1 + " AND T.Dept_Code = D.Dept_Code AND B.Lib_Code = L.Lib_Code AND T.Latestrec = 1 ";
                        Sql1 = Sql1 + strDate + strdept;
                        if (ddllibrary.Text != "All")
                        {
                            Sql1 = Sql1 + " AND B.Lib_Code ='" + ddllibrary.SelectedIndex + "'";
                            Sql1 = Sql1 + " GROUP BY B.Lib_Code,B.Roll_No,B.Stud_Name";
                        }
                        else
                            Sql1 = Sql1 + " GROUP BY B.Roll_No,B.Stud_Name";

                        if (ddlaccno.SelectedIndex == 0)
                            Sql1 = Sql1 + " HAVING COUNT(B.Roll_No) > " + txt_accno.Text;
                        else if (ddlaccno.SelectedIndex == 1)
                            Sql1 = Sql1 + " HAVING COUNT(B.Roll_No) < " + txt_accno.Text;
                        else if (ddlaccno.SelectedIndex == 2)
                            Sql1 = Sql1 + " HAVING COUNT(B.Roll_No) = " + txt_accno.Text;
                        else if (ddlaccno.SelectedIndex == 3)
                            Sql1 = Sql1 + " HAVING COUNT(B.Roll_No) >= " + txt_accno.Text + " AND COUNT(B.Roll_No) <= " + txt_accno2.Text;
                        Sql1 = Sql1 + " ORDER BY Count(B.Roll_No) Desc";
                    }
                    else if (ddlselectfor.Text == "All")
                    {
                        Sql1 = "SELECT B.Roll_No as 'Roll No',B.Stud_Name,COUNT(B.Roll_No) AS 'No. of Books'";
                        Sql1 = Sql1 + " FROM Borrow B,Registration R,Library L ";
                        Sql1 = Sql1 + " WHERE B.Lib_Code = L.Lib_Code and B.Roll_No = R.Roll_No ANd (B.Roll_No = R.Roll_No or B.Roll_No = R.Lib_Id)";
                        Sql1 = Sql1 + strDate + strdept;
                        if (ddllibrary.Text != "All")
                        {
                            Sql1 = Sql1 + " AND B.Lib_Code ='" + ddllibrary.SelectedIndex + "'";
                            Sql1 = Sql1 + " GROUP BY B.Lib_Code,B.Roll_No,B.Stud_Name";
                        }
                        else
                            Sql1 = Sql1 + " GROUP BY B.Roll_No,B.Stud_Name";

                        if (ddlaccno.SelectedIndex == 0)
                            Sql1 = Sql1 + " HAVING COUNT(B.Roll_No) > " + txt_accno.Text;
                        else if (ddlaccno.SelectedIndex == 1)
                            Sql1 = Sql1 + " HAVING COUNT(B.Roll_No) < " + txt_accno.Text;
                        else if (ddlaccno.SelectedIndex == 2)
                            Sql1 = Sql1 + " HAVING COUNT(B.Roll_No) = " + txt_accno.Text;
                        else if (ddlaccno.SelectedIndex == 3)
                            Sql1 = Sql1 + " HAVING COUNT(B.Roll_No) >= " + txt_accno.Text + " AND COUNT(B.Roll_No) <= " + txt_accno2.Text;
                        Sql1 = Sql1 + " UNION ALL";
                        Sql1 = Sql1 + " SELECT B.Roll_No as 'Roll No',B.Stud_Name,COUNT(B.Roll_No) AS 'No. of Books'";
                        Sql1 = Sql1 + " FROM Borrow B,StaffMaster M,StaffTrans T,HrDept_Master D,Library L ";
                        Sql1 = Sql1 + " WHERE (B.Roll_No = M.Staff_Code OR B.Roll_No = M.Lib_Id) and B.Roll_No = M.Staff_Code AND M.Staff_Code = T.Staff_Code ";
                        Sql1 = Sql1 + " AND T.Dept_Code = D.Dept_Code AND B.Lib_Code = L.Lib_Code AND T.Latestrec = 1 ";
                        Sql1 = Sql1 + strDate + strdept;
                        if (ddllibrary.Text != "All")
                        {
                            Sql1 = Sql1 + " AND B.Lib_Code ='" + ddllibrary.SelectedIndex + "'";
                            Sql1 = Sql1 + " GROUP BY B.Lib_Code,B.Roll_No,B.Stud_Name";
                        }
                        else
                            Sql1 = Sql1 + " GROUP BY B.Roll_No,B.Stud_Name";

                        if (ddlaccno.SelectedIndex == 0)
                            Sql1 = Sql1 + " HAVING COUNT(B.Roll_No) > " + txt_accno.Text;
                        else if (ddlaccno.SelectedIndex == 1)
                            Sql1 = Sql1 + " HAVING COUNT(B.Roll_No) < " + txt_accno.Text;
                        else if (ddlaccno.SelectedIndex == 2)
                            Sql1 = Sql1 + " HAVING COUNT(B.Roll_No) = " + txt_accno.Text;
                        else if (ddlaccno.SelectedIndex == 3)
                            Sql1 = Sql1 + " HAVING COUNT(B.Roll_No) >= " + txt_accno.Text + " AND COUNT(B.Roll_No) <= " + txt_accno2.Text;
                        Sql1 = Sql1 + " ORDER BY Count(B.Roll_No) Desc";
                    }
                    ds1 = d2.select_method_wo_parameter(Sql1, "text");
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        transrepo.Columns.Add("SNo", typeof(string));
                        transrepo.Columns.Add("Roll No", typeof(string));
                        transrepo.Columns.Add("Stud_Name", typeof(string));
                        transrepo.Columns.Add("No. of Books", typeof(string));

                        int i = 0;
                        int SNo = 0;
                        for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                        {
                            SNo++;
                            dr = transrepo.NewRow();
                            dr["SNo"] = Convert.ToString(SNo);
                            dr["Roll No"] = ds1.Tables[0].Rows[r]["Roll No"].ToString();
                            dr["Stud_Name"] = ds1.Tables[0].Rows[r]["Stud_Name"].ToString();
                            dr["No. of Books"] = ds1.Tables[0].Rows[r]["No. of Books"].ToString();
                            transrepo.Rows.Add(dr);
                        }
                        gridview2.DataSource = transrepo;
                        gridview2.DataBind();

                        gridview2.Visible = true;
                    }
                }
                #endregion

                #region Access Number Report added by rajasekar 09/05/2018
                else if (ddlreporttype.SelectedIndex == 8)
                {
                    string sqlCount = string.Empty;
                    string StrGrp = string.Empty;
                    double DblPrice = 0;
                    string priceVal = "";
                    collegeName = da.GetFunction("select collname from collinfo where  college_code='" + collegeCode + "'");
                    string lib_code = string.Empty;
                    if (ddllibrary.Items.Count > 0)
                    {
                        for (int i = 0; i < ddllibrary.Items.Count - 1; i++)
                        {
                            if (Convert.ToString(ddllibrary.SelectedItem) == "All")
                            {
                                if (lib_code == "")
                                {
                                    lib_code = "" + ddllibrary.Items[i + 1].Value + "";
                                }
                                else
                                {
                                    lib_code = lib_code + "'" + "," + "'" + ddllibrary.Items[i + 1].Value + "";
                                }
                            }
                            else
                                lib_code = ddllibrary.SelectedValue;
                        }
                    }
                    if (cbduplicateaccno.Checked == false)
                    {
                        if (cbmissingaccno.Checked == false)
                        {
                            Sql1 = "select Acc_No,Title,Author,Edition,ISBN,price,dept_code,book_status,attachment,lib_Name,CAST(RIGHT(Acc_No, LEN(Acc_No) - PATINDEX('%[0-9]%', Acc_No)+1) AS numeric), LEFT(Acc_No, PATINDEX('%[0-9]%', Acc_No)-1) from bookdetails,library ";
                            Sql1 = Sql1 + "where bookdetails.lib_code = library.lib_code ";

                            if (tex_accnofrom.Text.Trim() != "" || txt_accnoto.Text.Trim() != "")
                            {
                                if (collegeName == "THE NEW COLLEGE (AUTONOMOUS) SHIFT I" || collegeName == "THE NEW COLLEGE (AUTONOMOUS) SHIFT II")
                                    Sql1 = Sql1 + " and cast(substring(bookdetails.acc_no,4,len(acc_no)-3) as int) between '" + tex_accnofrom.Text + "' AND '" + txt_accnoto.Text + "' ";
                                else
                                {
                                    Sql1 = Sql1 + "and case when isnumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql1 = Sql1 + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + tex_accnofrom.Text + "' and '" + txt_accnoto.Text + "'";
                                    Sql1 = Sql1 + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql1 = Sql1 + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + tex_accnofrom.Text + "' and '" + txt_accnoto.Text + "'";
                                }
                            }
                            if (ddllibrary.Text != "All")
                                Sql1 = Sql1 + "and library.lib_code in('" + lib_code + "' )";

                            if (ddldept.Text != "All")
                                Sql1 = Sql1 + " and Dept_Code ='" + ddldept.Text + "'";

                            if (collegeName == "THE NEW COLLEGE (AUTONOMOUS) SHIFT I" || collegeName == "THE NEW COLLEGE (AUTONOMOUS) SHIFT II")
                                Sql1 = Sql1 + "ORDER BY CONVERT(nvarchar(30),SUBSTRING(Acc_No,1,3)), CONVERT(int,SUBSTRING(Acc_No,4,len(acc_no)-3)) ";
                            else
                                Sql1 = Sql1 + " ORDER BY LEFT(Acc_No, PATINDEX('%[0-9]%', Acc_No)-1)  ,  CAST(RIGHT(Acc_No, LEN(Acc_No) - PATINDEX('%[0-9]%', Acc_No)+1) AS numeric)";
                            sqlCount = "select SUM(CAST(isnull(Price,0) as Float)) from bookdetails,library ";
                            sqlCount = sqlCount + "where bookdetails.lib_code = library.lib_code and library.lib_code in('" + lib_code + "' )";
                            if (tex_accnofrom.Text.Trim() != "" || (txt_accnoto.Text.Trim()) != "")
                            {
                                if (collegeName == "THE NEW COLLEGE (AUTONOMOUS) SHIFT I" || collegeName == "THE NEW COLLEGE (AUTONOMOUS) SHIFT II")
                                    sqlCount = sqlCount + " and cast(substring(bookdetails.acc_no,4,len(acc_no)-3) as int) between '" + tex_accnofrom.Text + "' AND '" + txt_accnoto.Text + "' ";
                                else
                                {
                                    sqlCount = sqlCount + "and case when isnumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    sqlCount = sqlCount + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + tex_accnofrom.Text + "' and '" + txt_accnoto.Text + "'";
                                    sqlCount = sqlCount + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    sqlCount = sqlCount + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + tex_accnofrom.Text + "' and '" + txt_accnoto.Text + "'";
                                }
                            }
                            if (ddllibrary.Text != "All")
                                sqlCount = sqlCount + " and lib_name in('" + lib_code + "' )";
                            if (ddldept.Text != "All")
                                sqlCount = sqlCount + " and Dept_Code ='" + ddldept.Text + "'";
                            if (ddllibrary.Text != "All")
                                StrGrp = "GROUP BY Lib_Name";
                            if (ddldept.Text != "All")
                            {
                                if (StrGrp != "")
                                    StrGrp = ",Dept_Code";
                                else
                                    StrGrp = "GROUP BY Dept_Code";
                            }

                            priceVal = Convert.ToString(da.GetFunction(sqlCount));
                            if (!string.IsNullOrEmpty(priceVal))
                                DblPrice = Convert.ToDouble(priceVal);
                            if (DblPrice > 0)
                                DblPrice = Math.Round(DblPrice, 2);
                            else
                                DblPrice = 0;
                            ds1 = d2.select_method_wo_parameter(Sql1, "text");
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                transrepo.Columns.Add("SNo", typeof(string));
                                transrepo.Columns.Add("Access No", typeof(string));
                                transrepo.Columns.Add("Title", typeof(string));
                                transrepo.Columns.Add("Author", typeof(string));
                                transrepo.Columns.Add("Edition", typeof(string));
                                transrepo.Columns.Add("ISBN", typeof(string));
                                transrepo.Columns.Add("Price", typeof(string));
                                transrepo.Columns.Add("Department", typeof(string));
                                transrepo.Columns.Add("Book Status", typeof(string));
                                transrepo.Columns.Add("Attachment", typeof(string));
                                transrepo.Columns.Add("Library", typeof(string));

                                int i = 0;
                                int SNo = 0;
                                for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                                {
                                    SNo++;
                                    dr = transrepo.NewRow();
                                    dr["SNo"] = SNo;
                                    dr["Access No"] = ds1.Tables[0].Rows[r]["Acc_No"].ToString();
                                    dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                                    dr["Author"] = ds1.Tables[0].Rows[r]["Author"].ToString();
                                    dr["Edition"] = ds1.Tables[0].Rows[r]["Edition"].ToString();
                                    dr["ISBN"] = ds1.Tables[0].Rows[r]["ISBN"].ToString();
                                    dr["price"] = ds1.Tables[0].Rows[r]["price"].ToString();
                                    dr["Department"] = ds1.Tables[0].Rows[r]["dept_code"].ToString();
                                    dr["Book Status"] = ds1.Tables[0].Rows[r]["book_status"].ToString();
                                    dr["Attachment"] = ds1.Tables[0].Rows[r]["attachment"].ToString();
                                    dr["Library"] = ds1.Tables[0].Rows[r]["lib_Name"].ToString();
                                    dr["ISBN"] = "Total";
                                    dr["price"] = DblPrice.ToString();
                                    transrepo.Rows.Add(dr);
                                }
                            }
                            gridview2.DataSource = transrepo;
                            gridview2.DataBind();

                            gridview2.Visible = true;
                        }
                    }
                    else
                    {
                        int rowHeight = 0;
                        int colcount = 0;
                        transrepo.Columns.Add("SNo", typeof(string));
                        transrepo.Columns.Add("Access No", typeof(string));
                        transrepo.Columns.Add("Missing No", typeof(string));

                        int sn = 0;
                        int accnofrom = 0, accnoto = 0;
                        if (tex_accnofrom.Text != "")
                            accnofrom = Convert.ToInt32(tex_accnofrom.Text);
                        if (txt_accnoto.Text != "")
                            accnoto = Convert.ToInt32(txt_accnoto.Text);
                        for (int i = accnofrom; i <= accnoto; i++)
                        {
                            rowHeight += 40;
                            Sql1 = "select '' as 'Select',Acc_No from bookdetails,library ";
                            if (txt_acr.Text == "")
                                Sql1 = Sql1 + " where bookdetails.lib_code = library.lib_code and acc_no ='" + i + "'";
                            else
                                Sql1 = Sql1 + " where bookdetails.lib_code = library.lib_code and substring(acc_no," + txt_acr.Text.Length + 1 + ",len(acc_no)-3) ='" + i + "' AND Left(Acc_No,3) ='" + txt_acr.Text + "' ";
                            if (ddllibrary.Text != "All")
                                Sql1 = Sql1 + " and lib_name ='" + ddllibrary.Text + "'";
                            if (ddldept.Text.Trim() != "All")
                                Sql1 = Sql1 + " and Dept_Code ='" + ddldept.Text + "'";
                            ds1 = d2.select_method_wo_parameter(Sql1, "text");
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                sn++;
                                dr = transrepo.NewRow();
                                dr["SNo"] = sn;
                                dr["Access No"] = i.ToString();
                                dr["Title"] = (txt_acr.Text + i).ToString();
                                transrepo.Rows.Add(dr);
                            }
                            gridview2.DataSource = transrepo;
                            gridview2.DataBind();

                            gridview2.Visible = true;
                        }
                    }
                }
                #endregion

                #region Card Locked Report added by rajasekar 14/05/2018
                else if (ddlreporttype.SelectedIndex == 9)
                {
                    if (cbfrom.Checked == true)
                        strDate = "  and access_date between '" + fromdate1 + "' and '" + todate1 + "' ";
                    else
                        strDate = "";
                    if (ddldept.Text != "All")
                        strdept = " and dept_name='" + ddldept.Text + "'";
                    else
                        strdept = "";

                    if (ddlreporttype.Text == "Card Locked Report" && ddlselectfor.Text == "Student")
                        Sql1 = "Select token_no as 'Token No',stud_name as 'Student Name',reas_loc as 'Reason' from tokendetails where is_staff = 0 " + strDate + " and is_locked = 2" + strdept;
                    else if (ddlreporttype.Text == "Card Locked Report" && ddlselectfor.Text == "Staff")
                        Sql1 = "Select token_no as 'Token No',stud_name as 'Staff Name',reas_loc as 'Reason' from tokendetails where is_staff = 1" + strDate + " and is_locked = 2" + strdept;
                    else if (ddlreporttype.Text == "Card Locked Report" && ddlselectfor.Text == "All")
                        Sql1 = "Select token_no as 'Token No',stud_name as 'Name',reas_loc as 'Reason' from tokendetails where is_locked = 2 " + strDate + strdept;
                    ds1 = d2.select_method_wo_parameter(Sql1, "text");

                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        transrepo.Columns.Add("SNo", typeof(string));
                        transrepo.Columns.Add("Token No", typeof(string));
                        if (ddlselectfor.Text == "Student")
                        {
                            transrepo.Columns.Add("Student Name", typeof(string));
                        }
                        else if (ddlselectfor.Text == "Staff")
                        {
                            transrepo.Columns.Add("Staff Name", typeof(string));
                        }
                        else if (ddlselectfor.Text == "All")
                        {
                            transrepo.Columns.Add("Name", typeof(string));
                        }
                        transrepo.Columns.Add("Reason", typeof(string));

                        int row = 0;
                        int i = 0;

                        int SNo = 0;
                        for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                        {
                            SNo++;
                            dr = transrepo.NewRow();
                            dr["SNo"] = Convert.ToString(SNo);
                            dr["Token No"] = ds1.Tables[0].Rows[r]["Token No"].ToString();
                            if (ddlselectfor.Text == "Student")
                            {
                                dr["Student Name"] = ds1.Tables[0].Rows[r]["Student Name"].ToString();
                            }
                            else if (ddlselectfor.Text == "Staff")
                            {
                                dr["Staff Name"] = ds1.Tables[0].Rows[r]["Staff Name"].ToString();
                            }
                            else if (ddlselectfor.Text == "All")
                            {
                                dr["Name"] = ds1.Tables[0].Rows[r]["Name"].ToString();
                            }
                            dr["Reason"] = ds1.Tables[0].Rows[r]["Reason"].ToString();
                            transrepo.Rows.Add(dr);
                        }
                        gridview2.DataSource = transrepo;
                        gridview2.DataBind();
                        gridview2.Visible = true;
                    }
                }
                #endregion

                #region Card Holders Information

                else if (ddlreporttype.SelectedIndex == 10)
                {
                    if (cbfrom.Checked == true)
                        strDate = "  and access_date between '" + fromdate1 + "' and '" + todate1 + "' ";
                    else
                        strDate = "";
                    if (ddldept.Text != "All")
                        strdept = " and dept_name='" + ddldept.Text + "'";
                    else
                        strdept = "";
                    if (ddlselectfor.Text == "Student")
                        Sql1 = "Select token_no as 'Token No',tokendetails.roll_no as 'Roll No',stud_name as 'Student Name',dept_name as 'Department Name' from tokendetails where is_staff = 0 " + strDate + "" + strdept + " order by roll_no";
                    else if (ddlselectfor.Text == "Staff")
                        Sql1 = "Select token_no as 'Token No',tokendetails.roll_no as 'Roll No',stud_name as 'Staff Name',dept_name as 'Department Name' from tokendetails where is_staff = 1 " + strDate + "" + strdept + " order by roll_no";
                    else if (ddlselectfor.Text == "All")
                        Sql1 = "Select token_no as 'Token No',tokendetails.roll_no as 'Roll No',stud_name as 'Name',dept_name  as 'Department Name' from tokendetails where  is_locked <> 2 " + strDate + "" + strdept + " order by roll_no";
                    ds1 = d2.select_method_wo_parameter(Sql1, "text");
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        transrepo.Columns.Add("SNo", typeof(string));
                        transrepo.Columns.Add("Token No", typeof(string));
                        transrepo.Columns.Add("Roll No", typeof(string));
                        if (ddlselectfor.Text == "Student")
                        {
                            transrepo.Columns.Add("Student Name", typeof(string));
                        }
                        else if (ddlselectfor.Text == "Staff")
                        {
                            transrepo.Columns.Add("Staff Name", typeof(string));
                        }
                        else if (ddlselectfor.Text == "All")
                        {
                            transrepo.Columns.Add("Name", typeof(string));
                        }
                        transrepo.Columns.Add("Department Name", typeof(string));


                        int row = 0;
                        int i = 0;

                        int SNo = 0;
                        for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                        {
                            SNo++;
                            dr = transrepo.NewRow();
                            dr["SNo"] = Convert.ToString(SNo);
                            dr["Token No"] = ds1.Tables[0].Rows[r]["Token No"].ToString();
                            dr["Roll No"] = ds1.Tables[0].Rows[r]["Roll No"].ToString();
                            if (ddlselectfor.Text == "Student")
                            {
                                dr["Student Name"] = ds1.Tables[0].Rows[r]["Student Name"].ToString();
                            }
                            else if (ddlselectfor.Text == "Staff")
                            {
                                dr["Staff Name"] = ds1.Tables[0].Rows[r]["Staff Name"].ToString();
                            }
                            else if (ddlselectfor.Text == "All")
                            {
                                dr["Name"] = ds1.Tables[0].Rows[r]["Name"].ToString();
                            }
                            dr["Department Name"] = ds1.Tables[0].Rows[r]["Department Name"].ToString();
                            transrepo.Rows.Add(dr);
                        }
                        gridview2.DataSource = transrepo;
                        gridview2.DataBind();

                        gridview2.Visible = true;
                    }
                }
                #endregion

                #region Issued Books(Non Member)

                else if (ddlreporttype.SelectedIndex == 11)
                {
                    string coll = Convert.ToString(ddlCollege.SelectedValue);
                    DataSet dSNonmem = new DataSet();
                    StringBuilder sbNonmem = new StringBuilder();
                    string SqlQry = "Select * from user_master where college_code='" + coll + "'";
                    if (ddldept.SelectedItem.Text != "All")
                    {
                        SqlQry += "and department='" + ddldept.SelectedItem.Text + "'";
                    }
                    dSNonmem.Clear();
                    dSNonmem = d2.select_method_wo_parameter(SqlQry, "text");
                    string rollNo = "";
                    if (dSNonmem.Tables[0].Rows.Count > 0)
                    {
                        for (int Non = 0; Non < dSNonmem.Tables[0].Rows.Count; Non++)
                        {
                            string User_id = Convert.ToString(dSNonmem.Tables[0].Rows[Non]["user_id"]);
                            sbNonmem.Append(User_id).Append("','");
                        }
                        rollNo = Convert.ToString(sbNonmem);
                        rollNo = rollNo.TrimEnd(',');
                    }
                    if (cbfrom.Checked == true)
                        strDate = " and borrow_date between '" + fromdate1 + "' and '" + todate1 + "' ";
                    else
                        strDate = "";

                    // if (ddlreporttype.Text == "Issued Books(Non Member)" && ddllibrary.Text != "All")
                    //    Sql1 = "Select nonmem_SNo as 'Serail No',Acc_No as 'Access No',Nonmem_name as 'Name',staff_code as 'Code',booktype as 'Book Type',convert(varchar,Borrow_date,103) as 'Issue Date',library.lib_name as 'Library Name' from nonmembers,library where " + strDate + " library.lib_code=nonmembers.lib_code and nonmembers.lib_code like '" + ddllibrary.Text + "'";
                    //else if (ddlreporttype.Text == "Issued Books(Non Member)" && ddllibrary.Text == "All")
                    //    Sql1 = "Select nonmem_SNo as 'Serial No',Acc_No as 'Access No',Nonmem_name as 'Name',staff_code as 'Code',booktype as 'Book Type',convert(varchar,Borrow_date,103)as 'Issue Date',library.lib_name as 'Library Name' from nonmembers,library where " + strDate + " library.lib_code=nonmembers.lib_code ";

                    if (ddlreporttype.Text == "Issued Books(Non Member)")
                        Sql1 = "Select distinct borrow.roll_no as 'Roll No',(borrow.acc_no)as 'Access No',token_no as 'Card No',cirno_issue as 'Issue Circulation No',borrow.stud_name as 'Name', convert(varchar,borrow_date,103) as 'Borrow Date',isnull(borrow.Issued_Time,'') as 'Issued_Time',convert(varchar,due_date,103) as 'Due Date',case when borrow.return_flag=0 then 'NR' else convert(varchar,return_date,103) end as 'Return Date',isnull(borrow.returned_time,'') as 'Returned Time',title as 'Title',author as 'Author',borrow.book_issuedby as 'Book Issued By',borrow.return_type as 'Book Type',library.lib_name as 'Library Name' from library,borrow,user_master where user_id=roll_no and library.lib_code=borrow.lib_code and roll_no in('" + rollNo + "') " + strDate + " ";

                    ds1 = d2.select_method_wo_parameter(Sql1, "text");
                    int rowHeight = 0;
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        transrepo.Columns.Add("SNo", typeof(string));
                        transrepo.Columns.Add("Access No", typeof(string));
                        transrepo.Columns.Add("Card No", typeof(string));
                        transrepo.Columns.Add("Name", typeof(string));
                        transrepo.Columns.Add("Roll No", typeof(string));
                        transrepo.Columns.Add("Book Type", typeof(string));
                        transrepo.Columns.Add("Borrow Date", typeof(string));
                        transrepo.Columns.Add("Library Name", typeof(string));


                        int row = 0;
                        int i = 0;
                        int SNo = 0;
                        for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                        {
                            SNo++;
                            dr = transrepo.NewRow();
                            dr["SNo"] = Convert.ToString(SNo);
                            dr["Access No"] = ds1.Tables[0].Rows[r]["Access No"].ToString();
                            dr["Card No"] = ds1.Tables[0].Rows[r]["Card No"].ToString();
                            dr["Name"] = ds1.Tables[0].Rows[r]["Name"].ToString();
                            dr["Roll No"] = ds1.Tables[0].Rows[r]["Roll No"].ToString();
                            dr["Book Type"] = ds1.Tables[0].Rows[r]["Book Type"].ToString();
                            dr["Borrow Date"] = ds1.Tables[0].Rows[r]["Borrow Date"].ToString();
                            dr["Library Name"] = ds1.Tables[0].Rows[r]["Library Name"].ToString();
                            transrepo.Rows.Add(dr);
                        }
                        gridview2.DataSource = transrepo;
                        gridview2.DataBind();

                        gridview2.Visible = true;
                    }
                }
                #endregion

                #region Missing Books added by rajasekar 16/05/2018
                else if (ddlreporttype.SelectedIndex == 12)
                {
                    if (ddldept.Text != "All")
                        strdept = "" + ddldept.Text + "";
                    else
                        strdept = "";
                    if (ddldept.Text != "All" && ddllibrary.Text != "All" && cbfrom.Checked == true)
                    {
                        Sql1 = "Select distinct bookdetails.acc_no as 'Access No',bookdetails.Title as 'Title',bookdetails.author as 'Author',library.lib_name as 'Library Name' From bookdetails,library WHERE library.lib_code= bookdetails.lib_code and bookdetails.book_status like 'missing%' and  bill_date between '" + fromdate1 + "' and '" + todate1 + "' and bookdetails.lib_code='" + ddllibrary.Text + "' and dept_code='" + strdept + "'";
                    }
                    else if (ddldept.Text != "All" && ddllibrary.Text != "All" && cbfrom.Checked == false)
                        Sql1 = "Select distinct bookdetails.acc_no as 'Access No',bookdetails.Title as 'Title',bookdetails.author as 'Author',library.lib_name as 'Library Name' From bookdetails,library WHERE library.lib_code= bookdetails.lib_code and bookdetails.book_status like 'missing%'  and bookdetails.lib_code='" + ddllibrary.Text + "' and dept_code='" + strdept + "'";
                    else if (ddldept.Text == "All" && ddllibrary.Text == "All" && cbfrom.Checked == true)
                        Sql1 = "Select distinct bookdetails.acc_no as 'Access No',bookdetails.Title as 'Title',bookdetails.author as 'Author',library.lib_name as 'Library Name' From bookdetails ,library WHERE library.lib_code= bookdetails.lib_code and bookdetails.book_status like 'missing%' and  bill_date between '" + fromdate1 + "' and '" + todate1 + "'  ";
                    else if (ddldept.Text == "All" && ddllibrary.Text == "All" && cbfrom.Checked == false)
                        Sql1 = "Select distinct bookdetails.acc_no as 'Access No',bookdetails.Title as 'Title',bookdetails.author as 'Author',library.lib_name as 'Library Name' From bookdetails,library WHERE library.lib_code= bookdetails.lib_code and bookdetails.book_status like 'missing%' ";
                    else if (ddldept.Text == "All" && ddllibrary.Text != "All" && cbfrom.Checked == false)
                        Sql1 = "Select distinct bookdetails.acc_no as 'Access No',bookdetails.Title as 'Title',bookdetails.author as 'Author',library.lib_name as 'Library Name' From bookdetails ,library WHERE library.lib_code= bookdetails.lib_code and bookdetails.book_status like 'missing%' and  bill_date between '" + fromdate1 + "' and '" + todate1 + "' and bookdetails.lib_code='" + ddllibrary.Text + "'";
                    else if (ddldept.Text == "All" && ddllibrary.Text != "All" && cbfrom.Checked == false)
                        Sql1 = "Select distinct  bookdetails.acc_no as 'Access No',bookdetails.Title as 'Title',bookdetails.author as 'Author',library.lib_name as 'Library Name' From bookdetails,library WHERE library.lib_code= bookdetails.lib_code and bookdetails.book_status like 'missing%' and bookdetails.lib_code='" + ddllibrary.Text + "'";
                    else if (ddldept.Text != "All" && ddllibrary.Text == "All" && cbfrom.Checked == true)
                        Sql1 = "Select distinct bookdetails.acc_no as 'Access No',bookdetails.Title as 'Title',bookdetails.author as 'Author',library.lib_name as 'Library Name' From bookdetails,library WHERE library.lib_code= bookdetails.lib_code and bookdetails.book_status like 'missing%' and  bill_date between '" + fromdate1 + "' and '" + todate1 + "' and dept_code='" + strdept + "'";
                    else if (ddldept.Text != "All" && ddllibrary.Text == "All" && cbfrom.Checked == true)
                        Sql1 = "Select distinct bookdetails.acc_no as 'Access No',bookdetails.Title as 'Title',bookdetails.author as 'Author',library.lib_name as 'Library Name' From bookdetails,library WHERE library.lib_code= bookdetails.lib_code and bookdetails.book_status like 'missing%' and  dept_code='" + strdept + "'";
                    ds1 = d2.select_method_wo_parameter(Sql1, "text");

                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        transrepo.Columns.Add("SNo", typeof(string));
                        transrepo.Columns.Add("Access No", typeof(string));
                        transrepo.Columns.Add("Title", typeof(string));
                        transrepo.Columns.Add("Author", typeof(string));
                        transrepo.Columns.Add("Library Name", typeof(string));



                        int row = 0;
                        int i = 0;
                        int SNo = 0;
                        for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                        {
                            SNo++;
                            dr = transrepo.NewRow();
                            dr["SNo"] = Convert.ToString(SNo);
                            dr["Access No"] = ds1.Tables[0].Rows[r]["Access No"].ToString();
                            dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                            dr["Author"] = ds1.Tables[0].Rows[r]["Author"].ToString();
                            dr["Library Name"] = ds1.Tables[0].Rows[r]["Library Name"].ToString();
                            transrepo.Rows.Add(dr);
                        }
                        gridview2.DataSource = transrepo;
                        gridview2.DataBind();

                        gridview2.Visible = true;
                    }
                }
                #endregion

                #region User Entry Status
                else if (ddlreporttype.SelectedIndex == 13)
                {
                    if (cbfrom.Checked == true)
                        strDate = " and entry_date between '" + fromdate1 + "' and '" + todate1 + "' ";
                    else
                        strDate = "";
                    if (ddllibrary.Text != "All" && ddldept.Text == "All")
                        qrystr = "and lib_code=" + ddllibrary.SelectedValue + "";
                    else if (ddllibrary.Text == "All" && ddldept.Text != "All")
                        qrystr = "and libusers.dept_name='" + ddldept.Text + "'";
                    else if (ddllibrary.Text != "All" && ddldept.Text != "All")
                        qrystr = "and lib_code=" + ddllibrary.SelectedValue + "  and  libusers.dept_name='" + ddldept.Text + "'";
                    else if (ddllibrary.Text == "All" && ddldept.Text == "All")
                        qrystr = "";

                    Sql1 = "";
                    if (rbtype.SelectedValue == "Hit by Staff")
                        Sql1 = "select convert(varchar,entry_date,103) as 'Entry Date',count(*) as 'Hit Status'from libusers where usercat='Staff'" + strDate + "" + qrystr + " group by entry_date";
                    else if (rbtype.SelectedValue == "Hit by Student")
                        Sql1 = "select convert(varchar,entry_date,103) as 'Entry Date',count(*) as 'Hit Status'from libusers where usercat='Student'" + strDate + "" + qrystr + " group by entry_date";
                    else if (rbtype.SelectedValue == "Visitor")
                        Sql1 = "select convert(varchar,entry_date,103) as 'Entry Date',count(*) as 'Hit Status' from libusers where usercat='Visitor'" + strDate + "" + qrystr + " group by entry_date";

                    ds1 = d2.select_method_wo_parameter(Sql1, "text");

                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        transrepo.Columns.Add("SNo", typeof(string));
                        transrepo.Columns.Add("Entry Date", typeof(string));
                        transrepo.Columns.Add("Hit Status", typeof(string));


                        int row = 0;
                        int i = 0;
                        int SNo = 0;
                        for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                        {
                            SNo++;
                            dr = transrepo.NewRow();
                            dr["SNo"] = Convert.ToString(SNo);
                            dr["Entry Date"] = ds1.Tables[0].Rows[r]["Entry Date"].ToString();
                            dr["Hit Status"] = ds1.Tables[0].Rows[r]["Hit Status"].ToString();
                            transrepo.Rows.Add(dr);
                        }
                        gridview2.DataSource = transrepo;
                        gridview2.DataBind();

                        gridview2.Visible = true;
                    }
                }
                #endregion

                #region News Paper List

                else if (ddlreporttype.SelectedIndex == 14)
                {
                    if (ddllibrary.Text == "All")
                    {
                        Sql1 = "select serial_no as 'Serial No',cur_date as 'Current Date',title as 'Paper Name',price as 'Price',Lib_Code as 'Library Code',attachment as 'Attachment',noofcopies as 'No Of Copies' from news_paper where cur_date >= '" + fromdate1 + "' and cur_date <= '" + todate1 + "'";
                    }
                    else if (ddllibrary.Text != "All")
                    {
                        Sql1 = "select serial_no as 'Serial No',cur_date as 'Current Date',title as 'Paper Name',price as 'Price',Lib_Code as 'Library Code',attachment as 'Attachment',noofcopies as 'No Of Copies' from news_paper where lib_code='" + ddllibrary.SelectedValue + "' and cur_date >= '" + fromdate1 + "' and cur_date <= '" + todate1 + "'";
                    }
                    ds1 = d2.select_method_wo_parameter(Sql1, "text");

                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        transrepo.Columns.Add("SNo", typeof(string));
                        transrepo.Columns.Add("Serial No", typeof(string));
                        transrepo.Columns.Add("Current Date", typeof(string));
                        transrepo.Columns.Add("Paper Name", typeof(string));
                        transrepo.Columns.Add("Price", typeof(string));
                        transrepo.Columns.Add("Library Code", typeof(string));
                        transrepo.Columns.Add("Attachment", typeof(string));
                        transrepo.Columns.Add("No Of Copies", typeof(string));

                        int row = 0;
                        int i = 0;
                        int SNo = 0;
                        for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                        {
                            SNo++;
                            dr = transrepo.NewRow();
                            dr["SNo"] = SNo;
                            dr["Serial No"] = ds1.Tables[0].Rows[r]["Serial No"].ToString();
                            dr["Current Date"] = ds1.Tables[0].Rows[r]["Current Date"].ToString();
                            dr["Paper Name"] = ds1.Tables[0].Rows[r]["Paper Name"].ToString();
                            dr["Price"] = ds1.Tables[0].Rows[r]["Price"].ToString();
                            dr["Library Code"] = ds1.Tables[0].Rows[r]["Library Code"].ToString();
                            dr["Attachment"] = ds1.Tables[0].Rows[r]["Attachment"].ToString();
                            dr["No Of Copies"] = ds1.Tables[0].Rows[r]["No Of Copies"].ToString();
                            transrepo.Rows.Add(dr);
                        }
                        gridview2.DataSource = transrepo;
                        gridview2.DataBind();

                        gridview2.Visible = true;
                    }
                }
                #endregion

                #region Consolidated Book List Year Wise by rajasekar 21/05/2018
                else if (ddlreporttype.SelectedIndex == 15)
                {
                    if (ddldept.Text == "All")
                        strDegreecode = "%";
                    else
                        strDegreecode = "  and r.degree_code=" + ddldept.SelectedValue + "";
                    if (ddllibrary.Text != "All")
                        libcode = ddllibrary.SelectedValue;
                    else
                        libcode = "%";
                    if (cbcumlative.Checked == false)
                    {
                        if (ddllibrary.Text != "All" && cbfrom.Checked == false)
                            Sql1 = "Select year(bill_date) as 'Bill Date',count(acc_no) as 'Access No',count(distinct title) as 'Title',price as 'Price',library.lib_name as 'Library Name'from library,bookdetails where dept_code like '" + strDegreecode + "' and bookdetails.lib_code=library.lib_code and bookdetails.lib_code='" + libcode + "' group by year(bill_date),lib_name,price order by year(bill_date)";
                        else if (ddllibrary.Text == "All" && cbfrom.Checked == false)
                            Sql1 = "Select year(bill_date)as 'Bill Date',count(acc_no)as 'Access No',count(distinct title)as 'Title',price as 'Price',library.lib_name as 'Library Name' from library,bookdetails where dept_code like '" + strDegreecode + "' and  bookdetails.lib_code=library.lib_code group by year(bill_date),lib_name,price order by year(bill_date)";
                        else if (ddllibrary.Text != "All" && cbfrom.Checked == true)
                            Sql1 = "Select year(bill_date)as 'Bill Date',count(acc_no)as 'Access No',count(distinct title)as 'Title',price as 'Price',library.lib_name as 'Library Name' from library,bookdetails where  dept_code like '" + strDegreecode + "' and bookdetails.lib_code=library.lib_code and bookdetails.lib_code='" + libcode + "' and year(bill_date) between '" + fromdate1 + "' and '" + todate1 + "'  group by year(bill_date),lib_name,price order by year(bill_date)";
                        else if (ddllibrary.Text == "All" && cbfrom.Checked == true)
                            Sql1 = "Select year(bill_date)as 'Bill Date',count(acc_no)as 'Access No',count(distinct title)as 'Title',price as 'Price',library.lib_name as 'Library Name' from library,bookdetails where  dept_code like '" + libcode + "' and bookdetails.lib_code=library.lib_code and year(bill_date) between '" + fromdate1 + "' and '" + todate1 + "'group by year(bill_date),lib_name,price order by year(bill_date) ";
                        ds1 = d2.select_method_wo_parameter(Sql1, "text");
                        int rowHeight = 0;
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            transrepo.Columns.Add("SNo", typeof(string));
                            transrepo.Columns.Add("Bill Date", typeof(string));
                            transrepo.Columns.Add("Access No", typeof(string));
                            transrepo.Columns.Add("Title", typeof(string));
                            transrepo.Columns.Add("Price", typeof(string));
                            transrepo.Columns.Add("Library Name", typeof(string));


                            int row = 0;
                            int i = 0;
                            int SNo = 0;
                            for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                            {
                                SNo++;
                                dr = transrepo.NewRow();
                                dr["SNo"] = Convert.ToString(SNo);
                                dr["Bill Date"] = ds1.Tables[0].Rows[r]["Bill Date"].ToString();
                                dr["Access No"] = ds1.Tables[0].Rows[r]["Access No"].ToString();
                                dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                                dr["Price"] = ds1.Tables[0].Rows[r]["Price"].ToString();
                                dr["Library Name"] = ds1.Tables[0].Rows[r]["Library Name"].ToString();
                                transrepo.Rows.Add(dr);
                            }
                            gridview2.DataSource = transrepo;
                            gridview2.DataBind();

                            gridview2.Visible = true;
                        }
                    }
                    else if (cbcumlative.Checked == true)
                    {
                        if (ddllibrary.Text != "All" && cbfrom.Checked == false)
                            Sql1 = "Select year(bill_date) as 'Year',count(distinct title) as 'Title',count(title) as 'Volume',sum(cast(price as float)) as 'Price',library.lib_name as 'Library Name'from library,bookdetails where dept_code like '" + strDegreecode + "' and bookdetails.lib_code=library.lib_code and bookdetails.lib_code='" + libcode + "' group by year(bill_date),lib_name order by year(bill_date)";
                        else if (ddllibrary.Text == "All" && cbfrom.Checked == false)
                            Sql1 = "Select year(bill_date)as 'Year',count(distinct title) as 'Title',count(title) as 'Volume',sum(cast(price as float)) as 'Price',library.lib_name as 'Library Name' from library,bookdetails where dept_code like '" + strDegreecode + "' and  bookdetails.lib_code=library.lib_code group by year(bill_date),lib_name order by year(bill_date)";
                        else if (ddllibrary.Text != "All" && cbfrom.Checked == true)
                            Sql1 = "Select year(bill_date)as 'Year',count(distinct title)as 'Title',count(title) as 'Volume',sum(cast(price as float)) as 'Price',library.lib_name as 'Library Name' from library,bookdetails where  dept_code like '" + strDegreecode + "' and bookdetails.lib_code=library.lib_code and bookdetails.lib_code='" + libcode + "' and year(bill_date) between '" + fromdate1 + "' and '" + todate1 + "'  group by year(bill_date),lib_name order by year(bill_date)";
                        else if (ddllibrary.Text == "All" && cbfrom.Checked == true)
                            Sql1 = "Select year(bill_date)as 'Year',count(distinct title)as 'Title',count(title) as 'Volume',sum(cast(price as float)) as 'Price',library.lib_name as 'Library Name' from library,bookdetails where  dept_code like '" + strDegreecode + "' and bookdetails.lib_code=library.lib_code and year(bill_date) between '" + fromdate1 + "' and '" + todate1 + "'group by year(bill_date),lib_name order by year(bill_date) ";

                        ds1 = d2.select_method_wo_parameter(Sql1, "text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            transrepo.Columns.Add("SNo", typeof(string));
                            transrepo.Columns.Add("Year", typeof(string));
                            transrepo.Columns.Add("Title", typeof(string));
                            transrepo.Columns.Add("Volume", typeof(string));
                            transrepo.Columns.Add("Price", typeof(string));
                            transrepo.Columns.Add("Library Name", typeof(string));


                            int i = 0;
                            int SNo = 0;
                            for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                            {
                                SNo++;
                                dr = transrepo.NewRow();
                                dr["SNo"] = Convert.ToString(SNo);
                                dr["Year"] = ds1.Tables[0].Rows[r]["Year"].ToString();
                                dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                                dr["Volume"] = ds1.Tables[0].Rows[r]["Volume"].ToString();
                                dr["Price"] = ds1.Tables[0].Rows[r]["Price"].ToString();
                                dr["Library Name"] = ds1.Tables[0].Rows[r]["Library Name"].ToString();
                                transrepo.Rows.Add(dr);
                            }
                            gridview2.DataSource = transrepo;
                            gridview2.DataBind();

                            gridview2.Visible = true;
                        }
                    }
                }
                #endregion

                #region Books in Issues added by rajasekar 21/05/2018
                else if (ddlreporttype.SelectedIndex == 16)
                {
                    if (ddllibrary.Text != "All")
                        libcode = ddllibrary.SelectedValue;
                    else
                        libcode = "";
                    string StrIssueDate = "";
                    if (cbfrom.Checked == true)
                        StrIssueDate = " and Borrow_Date Between '" + fromdate1 + "' and '" + todate1 + "'";
                    else
                        StrIssueDate = "";
                    if (ddlsearchby.Text == "Yearwise")
                    {
                        if (ddlselectfor.Text == "Student")
                        {
                            Sql1 = "select borrow.acc_no as 'Access No',borrow.title as 'Title',borrow.author as 'Author',borrow.Roll_No as 'Roll No',borrow.token_no as 'Token No',borrow.stud_name as 'Student Name',convert(nvarchar,borrow.borrow_date,103) as 'Issue Date',convert(nvarchar,borrow.due_date,103) as 'Due Date',library.lib_name as 'Library Name' from borrow, library, registration where registration.batch_year = '" + ddlbatch.SelectedItem + "' and borrow.return_flag=0 and borrow.lib_code = library.lib_code and (registration.roll_no = borrow.roll_no or registration.lib_id = borrow.roll_no) and borrow.is_staff = 0 " + StrIssueDate + " and borrow.lib_code like '%" + libcode + "%' ";
                            Sql1 = Sql1 + " UNION ALL ";
                            Sql1 = Sql1 + "select borrow.acc_no as 'Access No',borrow.title as 'Title',borrow.author as 'Author',borrow.Roll_No as 'Roll No',borrow.token_no as 'Token No',borrow.stud_name as 'Student Name',convert(nvarchar,borrow.borrow_date,103) as 'Issue Date',convert(nvarchar,borrow.due_date,103) as 'Due Date',library.lib_name as 'Library Name' from borrow, library, user_master where borrow.return_flag=0 and borrow.lib_code = library.lib_code and user_master.user_id = borrow.roll_no and borrow.is_staff = 0 " + StrIssueDate + " and borrow.lib_code like '%" + libcode + "%' ";
                        }
                        else if (ddlselectfor.Text == "Staff")
                        {
                            Sql1 = "select borrow.acc_no as 'Access No',borrow.title as 'Title',borrow.author as 'Author',borrow.Roll_No as 'Roll No',borrow.token_no as 'Token No',borrow.stud_name as 'Staff Name',convert(nvarchar,borrow.borrow_date,103) as 'Issue Date',convert(nvarchar,borrow.due_date,103) as 'Due Date',library.lib_name as 'Library Name' from borrow, library, staffmaster where borrow.return_flag=0 and borrow.lib_code = library.lib_code and (staffmaster.staff_code = borrow.roll_no or staffmaster.lib_id = borrow.roll_no) and borrow.is_staff = 1" + StrIssueDate + " and borrow.lib_code like '%" + libcode + "%' ";
                            Sql1 = Sql1 + " UNION ALL ";
                            Sql1 = Sql1 + "select borrow.acc_no as 'Access No',borrow.title as 'Title',borrow.author as 'Author',borrow.Roll_No as 'Roll No',borrow.token_no as 'Token No',borrow.stud_name as 'Staff Name',convert(nvarchar,borrow.borrow_date,103) as 'Issue Date',convert(nvarchar,borrow.due_date,103) as 'Due Date',library.lib_name as 'Library Name' from borrow, library, user_master where borrow.return_flag=0 and borrow.lib_code = library.lib_code and user_master.user_id = borrow.roll_no and borrow.is_staff = 1" + StrIssueDate + " and borrow.lib_code like '%" + libcode + "%' ";
                        }
                        else if (ddlselectfor.Text == "All")
                        {
                            Sql1 = "select borrow.acc_no as 'Access No',borrow.title as 'Title',borrow.author as 'Author',borrow.Roll_No as 'Roll No',borrow.token_no as 'Token No',borrow.stud_name as 'Student Name',convert(nvarchar,borrow.borrow_date,103) as 'Issue Date',convert(nvarchar,borrow.due_date,103) as 'Due Date',library.lib_name as 'Library Name' from borrow, library, registration where registration.batch_year = '" + ddlbatch.SelectedItem + "' and borrow.return_flag=0 and borrow.lib_code = library.lib_code and (registration.roll_no = borrow.roll_no or registration.lib_id = borrow.roll_no) and borrow.is_staff = 0 " + StrIssueDate + " and borrow.lib_code like '%" + libcode + "%' ";
                            Sql1 = Sql1 + "UNION ALL ";
                            Sql1 = Sql1 + "select borrow.acc_no as 'Access No',borrow.title as 'Title',borrow.author as 'Author',borrow.Roll_No as 'Roll No',borrow.token_no as 'Token No',borrow.stud_name as 'Staff Name',convert(nvarchar,borrow.borrow_date,103) as 'Issue Date',convert(nvarchar,borrow.due_date,103) as 'Due Date',library.lib_name as 'Library Name' from borrow, library, staffmaster where borrow.return_flag=0 and borrow.lib_code = library.lib_code and (staffmaster.staff_code = borrow.roll_no or staffmaster.lib_id = borrow.roll_no) and borrow.is_staff = 1" + StrIssueDate + " and borrow.lib_code like '%" + libcode + "%' ";
                            Sql1 = Sql1 + "UNION ALL ";
                            Sql1 = Sql1 + "select borrow.acc_no as 'Access No',borrow.title as 'Title',borrow.author as 'Author',borrow.Roll_No as 'Roll No',borrow.token_no as 'Token No',borrow.stud_name as 'Staff Name',convert(nvarchar,borrow.borrow_date,103) as 'Issue Date',convert(nvarchar,borrow.due_date,103) as 'Due Date',library.lib_name as 'Library Name' from borrow, library, user_master where borrow.return_flag=0 and borrow.lib_code = library.lib_code and user_master.user_id = borrow.roll_no " + StrIssueDate + " and borrow.lib_code like '%" + libcode + "%' ";
                        }
                    }
                    else if (ddlsearchby.Text == "Departmentwise")
                    {
                        if (ddlselectfor.Text == "Student")
                            qrystr = " and borrow.is_staff = 0 ";
                        else if (ddlselectfor.Text == "Staff")
                            qrystr = " and borrow.is_staff = 1 ";
                        else if (ddlselectfor.Text == "All")
                            qrystr = "";

                        if (ddldept.Text != "All")
                            strdept = " and bookdetails.dept_code='" + ddldept.SelectedValue + "'";
                        else if (ddldept.Text == "All")
                            strdept = "";

                        if (ddlselectfor.Text == "Student")
                        {
                            Sql1 = "SELECT borrow.acc_no as 'Access No', borrow.title as 'Title',borrow.author as 'Author',borrow.Roll_No as 'Roll No', borrow.token_no as 'Token No',borrow.stud_name as 'Student Name' ,convert(nvarchar,borrow.borrow_date,103) as 'Issue Date', convert(nvarchar,borrow.due_date,103) as 'Due Date', library.lib_name as 'Library Name' from borrow, library, registration,bookdetails WHERE borrow.return_flag = 0 AND borrow.lib_code = library.lib_code and (registration.roll_no = borrow.roll_no or registration.lib_id = borrow.roll_no) and bookdetails.Acc_No = borrow.acc_no and bookdetails.lib_code = library.lib_code " + qrystr + strdept + StrIssueDate + " and borrow.lib_code like '%" + libcode + "%' ";
                            Sql1 = Sql1 + "UNION ALL ";
                            Sql1 = Sql1 + "SELECT borrow.acc_no as 'Access No', borrow.title as 'Title',borrow.author as 'Author',borrow.Roll_No as 'Roll No', borrow.token_no as 'Token No',borrow.stud_name as 'Student Name' ,convert(nvarchar,borrow.borrow_date,103) as 'Issue Date', convert(nvarchar,borrow.due_date,103) as 'Due Date', library.lib_name as 'Library Name' from borrow, library, user_master,bookdetails WHERE borrow.return_flag = 0 AND borrow.lib_code = library.lib_code and user_master.user_id = borrow.roll_no and bookdetails.Acc_No = borrow.acc_no and bookdetails.lib_code = library.lib_code " + qrystr + strdept + StrIssueDate + " and borrow.lib_code like '%" + libcode + "%' ";
                        }
                        else if (ddlselectfor.Text == "Staff")
                        {
                            Sql1 = "SELECT borrow.acc_no as 'Access No', borrow.title as 'Title',borrow.author as 'Author',borrow.Roll_No as 'Roll No', borrow.token_no as 'Token No',borrow.stud_name as 'Staff Name' ,convert(nvarchar,borrow.borrow_date,103) as 'Issue Date', convert(nvarchar,borrow.due_date,103) as 'Due Date', library.lib_name as 'Library Name' from borrow, library, staffmaster,bookdetails WHERE borrow.return_flag = 0 AND borrow.lib_code = library.lib_code and (staffmaster.staff_code = borrow.roll_no or staffmaster.lib_id = borrow.roll_no) and bookdetails.Acc_No = borrow.acc_no and bookdetails.lib_code = library.lib_code " + qrystr + strdept + StrIssueDate + " and borrow.lib_code like '%" + libcode + "%' ";
                            Sql1 = Sql1 + "UNION ALL ";
                            Sql1 = Sql1 + "SELECT borrow.acc_no as 'Access No', borrow.title as 'Title',borrow.author as 'Author',borrow.Roll_No as 'Roll No', borrow.token_no as 'Token No',borrow.stud_name as 'Staff Name' ,convert(nvarchar,borrow.borrow_date,103) as 'Issue Date', convert(nvarchar,borrow.due_date,103) as 'Due Date', library.lib_name as 'Library Name' from borrow, library, user_master,bookdetails WHERE borrow.return_flag = 0 AND borrow.lib_code = library.lib_code and user_master.user_id = borrow.roll_no and bookdetails.Acc_No = borrow.acc_no and bookdetails.lib_code = library.lib_code " + qrystr + strdept + StrIssueDate + " and borrow.lib_code like '%" + libcode + "%' ";
                        }
                        else if (ddlselectfor.Text == "All")
                        {
                            Sql1 = "SELECT borrow.acc_no as 'Access No', borrow.title as 'Title',borrow.author as 'Author',borrow.Roll_No as 'Roll No', borrow.token_no as 'Token No',borrow.stud_name as 'Student Name' ,convert(nvarchar,borrow.borrow_date,103) as 'Issue Date', convert(nvarchar,borrow.due_date,103) as 'Due Date', library.lib_name as 'Library Name' from borrow, library, registration,bookdetails WHERE borrow.return_flag = 0 AND borrow.lib_code = library.lib_code  and (registration.roll_no = borrow.roll_no or registration.lib_id = borrow.roll_no)  and bookdetails.Acc_No = borrow.acc_no and bookdetails.lib_code = library.lib_code " + qrystr + strdept + StrIssueDate + " and borrow.lib_code like '%" + libcode + "%' ";
                            Sql1 = Sql1 + "UNION ALL ";
                            Sql1 = Sql1 + "SELECT borrow.acc_no as 'Access No', borrow.title as 'Title',borrow.author as 'Author',borrow.Roll_No as 'Roll No', borrow.token_no as 'Token No',borrow.stud_name as 'Staff Name' ,convert(nvarchar,borrow.borrow_date,103) as 'Issue Date', convert(nvarchar,borrow.due_date,103) as 'Due Date', library.lib_name as 'Library Name' from borrow, library, staffmaster,bookdetails WHERE borrow.return_flag = 0 AND borrow.lib_code = library.lib_code and (staffmaster.staff_code = borrow.roll_no or staffmaster.lib_id = borrow.roll_no) and bookdetails.Acc_No = borrow.acc_no and bookdetails.lib_code = library.lib_code " + qrystr + strdept + StrIssueDate + " and borrow.lib_code like '%" + libcode + "%' ";
                            Sql1 = Sql1 + "UNION ALL ";
                            Sql1 = Sql1 + "SELECT borrow.acc_no as 'Access No', borrow.title as 'Title',borrow.author as 'Author',borrow.Roll_No as 'Roll No', borrow.token_no as 'Token No',borrow.stud_name as 'Staff Name' ,convert(nvarchar,borrow.borrow_date,103) as 'Issue Date', convert(nvarchar,borrow.due_date,103) as 'Due Date', library.lib_name as 'Library Name' from borrow, library, user_master,bookdetails WHERE borrow.return_flag = 0 AND borrow.lib_code = library.lib_code and user_master.user_id = borrow.roll_no and bookdetails.Acc_No = borrow.acc_no and bookdetails.lib_code = library.lib_code " + qrystr + strdept + StrIssueDate + " and borrow.lib_code like '%" + libcode + "%' ";
                        }
                    }
                    else
                    {
                        if (ddlselectfor.Text == "Student")
                        {
                            Sql1 = "select borrow.acc_no as 'Access No',borrow.title as 'Title',borrow.author as 'Author',borrow.Roll_No as 'Roll No',borrow.token_no as 'Token No',borrow.stud_name as 'Student Name',convert(nvarchar,borrow.borrow_date,103) as 'Issue Date',convert(nvarchar,borrow.due_date,103) as 'Due Date',library.lib_name as 'Library Name' from borrow, library, registration where borrow.return_flag=0 and borrow.lib_code = library.lib_code  and (registration.roll_no = borrow.roll_no or registration.lib_id = borrow.roll_no)  and  borrow.is_staff = 0 and borrow.lib_code like '%" + libcode + "%' " + StrIssueDate;
                            Sql1 = Sql1 + "UNION ALL ";
                            Sql1 = Sql1 + "select borrow.acc_no as 'Access No',borrow.title as 'Title',borrow.author as 'Author',borrow.Roll_No as 'Roll No',borrow.token_no as 'Token No',borrow.stud_name as 'Student Name',convert(nvarchar,borrow.borrow_date,103) as 'Issue Date',convert(nvarchar,borrow.due_date,103) as 'Due Date',library.lib_name as 'Library Name' from borrow, library, user_master where borrow.return_flag=0 and borrow.lib_code = library.lib_code  and user_master.user_id = borrow.roll_no and  borrow.is_staff = 0 and borrow.lib_code like '%" + libcode + "%' " + StrIssueDate;
                        }
                        else if (ddlselectfor.Text == "Staff")
                        {
                            Sql1 = "select borrow.acc_no as 'Access No',borrow.title as 'Title',borrow.author as 'Author',borrow.Roll_No as 'Roll No',borrow.token_no as 'Token No',borrow.stud_name as 'Staff Name',convert(nvarchar,borrow.borrow_date,103) as 'Issue Date',convert(nvarchar,borrow.due_date,103) as 'Due Date',library.lib_name  as 'Library Name' from borrow, library, staffmaster where borrow.return_flag=0 and borrow.lib_code = library.lib_code and (staffmaster.staff_code = borrow.roll_no or staffmaster.lib_id = borrow.roll_no) and borrow.is_staff = 1 and borrow.lib_code like '%" + libcode + "%' " + StrIssueDate;
                            Sql1 = Sql1 + "UNION ALL ";
                            Sql1 = Sql1 + "select borrow.acc_no as 'Access No',borrow.title as 'Title',borrow.author as 'Author',borrow.Roll_No as 'Roll No',borrow.token_no as 'Token No',borrow.stud_name as 'Staff Name',convert(nvarchar,borrow.borrow_date,103) as 'Issue Date',convert(nvarchar,borrow.due_date,103) as 'Due Date',library.lib_name  as 'Library Name' from borrow, library, user_master where borrow.return_flag=0 and borrow.lib_code = library.lib_code and user_master.user_id = borrow.roll_no and borrow.is_staff = 1 and borrow.lib_code like '%" + libcode + "%' " + StrIssueDate;
                        }
                        else if (ddlselectfor.Text == "All")
                        {
                            Sql1 = "select borrow.acc_no as 'Access No',borrow.title as 'Title',borrow.author as 'Author',borrow.Roll_No as 'Roll No',borrow.token_no as 'Token No',borrow.stud_name as 'Student Name',convert(nvarchar,borrow.borrow_date,103) as 'Issue Date',convert(nvarchar,borrow.due_date,103) as 'Due Date',library.lib_name as 'Library Name' from borrow, library, registration where borrow.return_flag=0 and borrow.lib_code = library.lib_code and (registration.roll_no = borrow.roll_no or registration.lib_id = borrow.roll_no) and borrow.is_staff = 0 and borrow.lib_code like '%" + libcode + "%' " + StrIssueDate;
                            Sql1 = Sql1 + "UNION ALL ";
                            Sql1 = Sql1 + "select borrow.acc_no as 'Access No',borrow.title as 'Title',borrow.author as 'Author',borrow.Roll_No as 'Roll No',borrow.token_no as 'Token No',borrow.stud_name as 'Staff Name',convert(nvarchar,borrow.borrow_date,103) as 'Issue Date',convert(nvarchar,borrow.due_date,103) as 'Due Date',library.lib_name  as 'Library Name' from borrow, library, staffmaster where borrow.return_flag=0 and borrow.lib_code = library.lib_code and (staffmaster.staff_code= borrow.roll_no or staffmaster.lib_id = borrow.roll_no) and borrow.is_staff = 1 and borrow.lib_code like '%" + libcode + "%' " + StrIssueDate;
                            Sql1 = Sql1 + "UNION ALL ";
                            Sql1 = Sql1 + "select borrow.acc_no as 'Access No',borrow.title as 'Title',borrow.author as 'Author',borrow.Roll_No as 'Roll No',borrow.token_no as 'Token No',borrow.stud_name as 'Staff Name',convert(nvarchar,borrow.borrow_date,103) as 'Issue Date',convert(nvarchar,borrow.due_date,103) as 'Due Date',library.lib_name  as 'Library Name' from borrow, library, user_master where borrow.return_flag=0 and borrow.lib_code = library.lib_code and user_master.user_id= borrow.roll_no and borrow.lib_code like '%" + libcode + "%' " + StrIssueDate;
                        }
                    }
                    ds1 = d2.select_method_wo_parameter(Sql1, "text");
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        transrepo.Columns.Add("SNo", typeof(string));
                        transrepo.Columns.Add("Access No", typeof(string));
                        transrepo.Columns.Add("Title", typeof(string));
                        transrepo.Columns.Add("Author", typeof(string));
                        transrepo.Columns.Add("Roll No", typeof(string));
                        transrepo.Columns.Add("Token No", typeof(string));
                        if (ddlselectfor.Text == "Student" || ddlselectfor.Text == "All")
                        {
                            transrepo.Columns.Add("Student Name", typeof(string));
                        }
                        else if (ddlselectfor.Text == "Staff")
                        {
                            transrepo.Columns.Add("Staff Name", typeof(string));
                        }
                        transrepo.Columns.Add("Issue Date", typeof(string));
                        transrepo.Columns.Add("Due Date", typeof(string));
                        transrepo.Columns.Add("Library Name", typeof(string));


                        int row = 0;
                        int i = 0;
                        int SNo = 0;
                        for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                        {
                            SNo++;
                            dr = transrepo.NewRow();
                            dr["SNo"] = Convert.ToString(SNo);
                            dr["Access No"] = ds1.Tables[0].Rows[r]["Access No"].ToString();
                            dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                            dr["Author"] = ds1.Tables[0].Rows[r]["Author"].ToString();
                            dr["Roll No"] = ds1.Tables[0].Rows[r]["Roll No"].ToString();
                            dr["Token No"] = ds1.Tables[0].Rows[r]["Token No"].ToString();
                            if (ddlselectfor.Text == "Student" || ddlselectfor.Text == "All")
                            {
                                dr["Student Name"] = ds1.Tables[0].Rows[r]["Student Name"].ToString();
                            }
                            else if (ddlselectfor.Text == "Staff")
                            {
                                dr["Staff Name"] = ds1.Tables[0].Rows[r]["Staff Name"].ToString();
                            }
                            dr["Issue Date"] = ds1.Tables[0].Rows[r]["Issue Date"].ToString();
                            dr["Due Date"] = ds1.Tables[0].Rows[r]["Due Date"].ToString();
                            dr["Library Name"] = ds1.Tables[0].Rows[r]["Library Name"].ToString();
                            transrepo.Rows.Add(dr);
                        }
                        gridview2.DataSource = transrepo;
                        gridview2.DataBind();

                        gridview2.Visible = true;
                    }
                }
                #endregion

                #region OPAC Hit Status Report by rajasekar 22/05/2018
                else if (ddlreporttype.SelectedIndex == 17)
                {
                    if (rbhitstatus.SelectedValue == "Hit by Student")
                    {
                        if (cbfrom.Checked == true && ddldept.Text != "All")
                            Sql1 = "select CONVERT(varchar(10),lib_date,103) as 'Entry Date',lib_count as 'No of Hits' from lib_queryhit where is_staff=0 and lib_date between '" + fromdate1 + "' and '" + todate1 + "' and department='" + ddldept.Text + "'";
                        else if (cbfrom.Checked == false && ddldept.Text != "All")
                            Sql1 = "select CONVERT(varchar(10),lib_date,103) as 'Entry Date',lib_count as 'No of Hits' from lib_queryhit where is_staff=0 and department='" + ddldept.Text + "'";
                        else if (cbfrom.Checked == true && ddldept.Text == "All")
                            Sql1 = "select CONVERT(varchar(10),lib_date,103) as 'Entry Date',sum(lib_count) as 'No of Hits' from lib_queryhit where lib_date between '" + fromdate1 + "' and '" + todate1 + "' and is_staff=0 group by lib_date";
                        else if (cbfrom.Checked == false && ddldept.Text == "All")
                            Sql1 = "select CONVERT(varchar(10),lib_date,103) as 'Entry Date',sum(lib_count) as 'No of Hits' from lib_queryhit where is_staff=0 group by lib_date";
                    }
                    else if (rbhitstatus.SelectedValue == "Hit by Staff")
                    {
                        if (cbfrom.Checked == true && ddldept.Text != "All")
                            Sql1 = "select CONVERT(varchar(10),lib_date,103) as 'Entry Date',lib_count as 'No of Hits' from lib_queryhit where lib_date between '" + fromdate1 + "' and '" + todate1 + "' and is_staff=1 and department='" + ddldept.Text + "'";
                        else if (cbfrom.Checked == false && ddldept.Text != "All")
                            Sql1 = "select CONVERT(varchar(10),lib_date,103) as 'Entry Date',lib_count as 'No of Hits' from lib_queryhit where is_staff=1 and department='" + ddldept.Text + "'";
                        else if (cbfrom.Checked == true && ddldept.Text == "All")
                            Sql1 = "select CONVERT(varchar(10),lib_date,103) as 'Entry Date',sum(lib_count) as 'No of Hits' from lib_queryhit where lib_date between '" + fromdate1 + "' and '" + todate1 + "' and is_staff=1 group by lib_date";
                        else if (cbfrom.Checked == false && ddldept.Text == "All")
                            Sql1 = "select CONVERT(varchar(10),lib_date,103) as 'Entry Date',sum(lib_count) as 'No of Hits' from lib_queryhit where is_staff=1 group by lib_date";
                    }
                    else if (rbhitstatus.SelectedValue == "All")
                    {
                        if (cbfrom.Checked == true)
                            Sql1 = "select CONVERT(varchar(10),lib_date,103) as 'Entry Date',sum(lib_count) as 'No of Hits' from lib_queryhit where lib_date between '" + fromdate1 + "' and '" + todate1 + "' group by lib_date";
                        else if (cbfrom.Checked == false)
                            Sql1 = "select CONVERT(varchar(10),lib_date,103) as 'Entry Date',sum(lib_count) as 'No of Hits' from lib_queryhit group by lib_date";
                    }
                    ds1 = d2.select_method_wo_parameter(Sql1, "text");
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        transrepo.Columns.Add("SNo", typeof(string));
                        transrepo.Columns.Add("Entry Date", typeof(string));
                        transrepo.Columns.Add("No of Hits", typeof(string));


                        int i = 0;
                        int SNo = 0;
                        for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                        {
                            SNo++;
                            dr = transrepo.NewRow();
                            dr["SNo"] = Convert.ToString(SNo);
                            dr["Entry Date"] = ds1.Tables[0].Rows[r]["Entry Date"].ToString();
                            dr["No of Hits"] = ds1.Tables[0].Rows[r]["No of Hits"].ToString();
                            transrepo.Rows.Add(dr);
                        }
                        gridview2.DataSource = transrepo;
                        gridview2.DataBind();

                        gridview2.Visible = true;
                    }
                }
                #endregion

                #region Book Purchase Report

                else if (ddlreporttype.SelectedIndex == 18)
                {
                    string collegeName = da.GetFunction("select collname from collinfo where  college_code='" + collegeCode + "'");
                    String StrBillNo = "";
                    String StrPurDept = "";
                    if (cbfrom.Checked == true)
                        strDate = " AND Bill_Date >='" + fromdate1 + "' AND Bill_Date <='" + todate1 + "'";
                    else
                        strDate = "";
                    if (cbbillno.Checked == true)
                    {
                        StrBillNo = " AND CASE WHEN isnumeric(right(B.bill_no,1)) = 1 then cast(substring(B.bill_no,(PATINDEX('%[0-9]%',";
                        StrBillNo = StrBillNo + " " + "B.bill_no)),len(B.bill_no))as int) end between '" + txtbillnofrom.Text + "' and '" + txtbillnoto.Text + "'";
                        StrBillNo = StrBillNo + " " + "and case when isnumeric(left(B.bill_no,1)) = 1 then cast(substring(B.bill_no,(PATINDEX('%[0-9]%',";
                        StrBillNo = StrBillNo + " " + "B.bill_no)),len(B.bill_no))as int) end between '" + txtbillnofrom.Text + "' and '" + txtbillnoto.Text + "'";
                    }
                    else
                        StrBillNo = "";
                    for (int i = 0; i < chklstdept.Items.Count; i++)
                    {
                        if (chklstdept.Items[i].Selected == true)
                        {
                            if (StrPurDept == "")
                            {
                                StrPurDept = "'" + chklstdept.Items[i].Value.ToString() + "'";
                            }
                            else
                            {
                                StrPurDept = StrPurDept + ",'" + chklstdept.Items[i].Value.ToString() + "'";
                            }
                        }
                    }

                    Sql1 = "SELECT Acc_No as 'Access No',Title as 'Title',Author as 'Author',Publisher as 'Publisher',Edition as 'Edition',Price as 'Price',Pur_Year as 'Purchase Year',Bill_No as 'Bill No',CONVERT(varchar,Bill_Date,103) as 'Bill Date',Book_Size as 'Pages',Supplier as 'Supplier',Lib_Name as 'Library Name' ";
                    Sql1 = Sql1 + "FROM Bookdetails B,Library L ";
                    Sql1 = Sql1 + "WHERE B.Lib_Code=L.Lib_Code " + strDate + StrBillNo;
                    if (ddldept.Text != "All")
                        Sql1 = Sql1 + "AND Dept_Code IN (" + StrPurDept + ")";
                    if (ddllibrary.Text != "All")
                        Sql1 = Sql1 + " AND B.Lib_Code='" + ddllibrary.SelectedValue + "' ";
                    if (ddlsupplier.Text != "All")
                        Sql1 = Sql1 + " AND Supplier='" + ddlsupplier.Text + "' ";

                    if (ddlinwardtype.Text != "All")
                        Sql1 = Sql1 + " AND pur_don ='" + ddlinwardtype.Text + "' ";
                    if (cbaccessno.Checked == true)
                    {
                        if (collegeName == "THE NEW COLLEGE (AUTONOMOUS) SHIFT I" || collegeName == "THE NEW COLLEGE (AUTONOMOUS) SHIFT II")
                        {
                            Sql1 = Sql1 + " and cast(substring(bookdetails.acc_no,4,len(acc_no)-3) as int) between '" + tex_accnofrom.Text + "' AND '" + txt_accnoto.Text + "' ";
                        }
                        else
                        {
                            int number1, number2;
                            if (int.TryParse(tex_accnofrom.Text.Trim(), out number1) == true && int.TryParse(txt_accnoto.Text.Trim(), out number2) == true)
                            {
                                Sql1 = Sql1 + "and case when isnumeric(right(B.acc_no,1)) = 1 then cast(substring(B.acc_no,(PATINDEX('%[0-9]%',";
                                Sql1 = Sql1 + "B.acc_no)),len(B.acc_no))as int) end between '" + tex_accnofrom.Text + "' and '" + txt_accnoto.Text + "'";
                                Sql1 = Sql1 + "and case when isnumeric(left(B.acc_no,1)) = 1 then cast(substring(B.acc_no,(PATINDEX('%[0-9]%',";
                                Sql1 = Sql1 + "B.acc_no)),len(B.acc_no))as int) end between '" + tex_accnofrom.Text + "' and '" + txt_accnoto.Text + "'";
                            }
                            else
                                Sql1 = Sql1 + " and B.acc_no between '" + tex_accnofrom.Text + "' and '" + txt_accnoto.Text + "'";
                        }
                    }
                    Sql1 = Sql1 + " ORDER BY CASE WHEN isnumeric(right(B.acc_no,1)) = 1 then cast(substring(B.acc_no,(PATINDEX('%[0-9]%',B.acc_no)),len(B.acc_no))as int) end ";

                    ds1 = d2.select_method_wo_parameter(Sql1, "text");
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        transrepo.Columns.Add("SNo", typeof(string));
                        transrepo.Columns.Add("Access No", typeof(string));
                        transrepo.Columns.Add("Title", typeof(string));
                        transrepo.Columns.Add("Author", typeof(string));
                        transrepo.Columns.Add("Publisher", typeof(string));
                        transrepo.Columns.Add("Edition", typeof(string));
                        transrepo.Columns.Add("Price", typeof(string));
                        transrepo.Columns.Add("Purchase Year", typeof(string));
                        transrepo.Columns.Add("Bill No", typeof(string));
                        transrepo.Columns.Add("Bill Date", typeof(string));
                        transrepo.Columns.Add("Pages", typeof(string));
                        transrepo.Columns.Add("Supplier", typeof(string));
                        transrepo.Columns.Add("Library Name", typeof(string));


                        int row = 0;
                        int i = 0;
                        int SNo = 0;
                        for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                        {
                            SNo++;
                            dr = transrepo.NewRow();
                            dr["SNo"] = Convert.ToString(SNo);
                            dr["Access No"] = ds1.Tables[0].Rows[r]["Access No"].ToString();
                            dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                            dr["Author"] = ds1.Tables[0].Rows[r]["Author"].ToString();
                            dr["Publisher"] = ds1.Tables[0].Rows[r]["Publisher"].ToString();
                            dr["Edition"] = ds1.Tables[0].Rows[r]["Edition"].ToString();
                            dr["Price"] = ds1.Tables[0].Rows[r]["Price"].ToString();
                            dr["Bill No"] = ds1.Tables[0].Rows[r]["Bill No"].ToString();
                            dr["Bill Date"] = ds1.Tables[0].Rows[r]["Bill Date"].ToString();
                            dr["Pages"] = ds1.Tables[0].Rows[r]["Pages"].ToString();
                            dr["Supplier"] = ds1.Tables[0].Rows[r]["Supplier"].ToString();
                            dr["Library Name"] = ds1.Tables[0].Rows[r]["Library Name"].ToString();
                            transrepo.Rows.Add(dr);
                        }
                        gridview2.DataSource = transrepo;
                        gridview2.DataBind();

                        gridview2.Visible = true;
                    }
                }
                #endregion

                #region Book Details

                else if (ddlreporttype.SelectedIndex == 19)
                {
                    string sqlprice = "";
                    string strprice = "";
                    string StrBookType = "";
                    string StrPurDon = "";
                    string collegeName = da.GetFunction("select collname from collinfo where  college_code='" + collegeCode + "'");
                    if (cbfrom.Checked == true)
                        strDate = " AND Bill_Date >='" + fromdate1 + "' AND Bill_Date <='" + todate1 + "'";
                    else
                        strDate = "";
                    if (ddlinwardtype.Text != "All")
                        StrPurDon = " and pur_don ='" + ddlinwardtype.Text + "' ";
                    else
                        StrPurDon = "";
                    Sql1 = "Select acc_no as 'Access No',title as 'Title',bookdetails.edition as 'Edition',author as 'Author',bookdetails.dept_code as 'Department',subject as 'Subject',call_no as 'Call No',CONVERT(nvarchar(10),date_accession,103) 'Accession Date',price as 'Price',ISNULL(ISBN,'') 'ISBN',bookdetails.book_size as 'Pages',bookdetails.publisher as 'Publisher',bookdetails.supplier as 'Supplier',bookdetails.bill_no as 'Bill Number',bookdetails.bill_date as 'Bill Date',bookdetails.pur_year as 'Year of Publication',ISNULL(Remark,'') as 'Remarks',library.lib_name as 'Library Name' ";
                    Sql11 = "SELECT SUM(A.Tit) FROM (SELECT COUNT(DISTINCT Title) Tit FROM BookDetails ";
                    if (rbbookdetails.SelectedValue == "Reference Books Only")
                    {
                        Sql1 = Sql1 + "from bookdetails,library where bookdetails.lib_code=library.lib_code and ref='Yes' " + strDate + StrBookType + StrPurDon;
                        Sql11 = Sql11 + "where ref='Yes' " + strDate + StrBookType + StrPurDon;
                    }
                    else if (rbbookdetails.SelectedValue == "Text Books Only")
                    {
                        Sql1 = Sql1 + "from bookdetails,library where bookdetails.lib_code=library.lib_code and ref='No' " + strDate + StrBookType + StrPurDon;
                        Sql11 = Sql11 + "where ref='No' " + strDate + StrBookType + StrPurDon;
                    }
                    else if (rbbookdetails.SelectedValue == "All")
                    {
                        Sql1 = Sql1 + "from bookdetails,library where bookdetails.lib_code=library.lib_code " + strDate + StrBookType + StrPurDon;
                        Sql11 = Sql11 + " where 1=1 " + strDate + StrBookType + StrPurDon;
                    }
                    if (cbaccessno.Checked == true)
                    {
                        if (collegeName == "THE NEW COLLEGE (AUTONOMOUS) SHIFT I" || collegeName == "THE NEW COLLEGE (AUTONOMOUS) SHIFT II")
                        {
                            Sql1 = Sql1 + " and cast(substring(bookdetails.acc_no,4,len(acc_no)-3) as int) between '" + tex_accnofrom.Text + "' AND '" + txt_accnoto.Text + "' ";
                        }
                        else
                        {
                            int number1, number2;
                            if (int.TryParse(tex_accnofrom.Text.Trim(), out number1) == true && int.TryParse(txt_accnoto.Text.Trim(), out number2) == true)
                            {

                                Sql1 = Sql1 + "and case when isnumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                Sql1 = Sql1 + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + tex_accnofrom.Text + "' and '" + txt_accnoto.Text + "'";
                                Sql1 = Sql1 + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                Sql1 = Sql1 + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + tex_accnofrom.Text + "' and '" + txt_accnoto.Text + "'";
                            }
                            else
                                Sql1 = Sql1 + " and bookdetails.acc_no between '" + tex_accnofrom.Text + "' and '" + txt_accnoto.Text + "'";
                        }
                    }
                    if (ddllibrary.Text != "All")
                    {
                        Sql1 = Sql1 + " and bookdetails.Lib_Code ='" + ddllibrary.SelectedValue + "'";
                        Sql11 = Sql11 + " and bookdetails.Lib_Code ='" + ddllibrary.SelectedValue + "'";
                    }
                    if (ddldept.Text != "All")
                    {
                        Sql1 = Sql1 + " and bookdetails.Dept_Code ='" + ddldept.Text + "'";
                        Sql11 = Sql11 + " and bookdetails.Dept_Code ='" + ddldept.Text + "'";
                    }
                    if (ddlsubject.Text != "All")
                    {
                        Sql1 = Sql1 + " and bookdetails.Subject Like '" + ddlsubject.Text + "%'";
                        Sql11 = Sql11 + " and bookdetails.Subject Like '" + ddlsubject.Text + "%'";
                    }
                    if (ddlstatus.Text != "All")
                    {
                        Sql1 = Sql1 + " and bookdetails.book_status='" + ddlstatus.Text + "'";
                        Sql11 = Sql11 + " and bookdetails.book_status='" + ddlstatus.Text + "'";
                    }
                    if (txtpubyear.Text.Trim() != "")
                    {
                        Sql1 = Sql1 + " and bookdetails.pur_year Like '" + txtpubyear.Text + "%'";
                        Sql11 = Sql11 + " and bookdetails.pur_year Like '" + txtpubyear.Text + "%'";
                    }
                    if (txt_acr.Text.Trim() != "")
                    {
                        Sql1 = Sql1 + " and bookdetails.acc_no Like '" + txt_acr.Text + "%'";
                        Sql11 = Sql11 + " and bookdetails.acc_no Like '" + txt_acr.Text + "%'";
                    }
                    if (ddlremarks.Text.Trim() != "All")
                    {
                        Sql1 = Sql1 + " and bookdetails.remark ='" + ddlremarks.Text + "' ";
                        Sql11 = Sql11 + " and bookdetails.remark ='" + ddlremarks.Text + "' ";
                    }
                    else
                    {
                        if (txtremarks.Text.Trim() != "")
                        {
                            Sql1 = Sql1 + " and bookdetails.remark like '%" + txtremarks.Text + "%' ";
                            Sql11 = Sql11 + " and bookdetails.remark like '%" + txtremarks.Text + "%' ";
                        }
                    }
                    Sql11 = Sql11 + " GROUP BY Dept_Code ) A ";
                    if (collegeName == "THE NEW COLLEGE (AUTONOMOUS) SHIFT I" || collegeName == "THE NEW COLLEGE (AUTONOMOUS) SHIFT II")
                        Sql1 = Sql1 + "ORDER BY CONVERT(nvarchar(30),SUBSTRING(Acc_No,1,3)), CONVERT(int,SUBSTRING(Acc_No,4,len(acc_no)-3)) ";
                    else
                        Sql1 = Sql1 + " ORDER BY LEN(Acc_No),Acc_No ";
                    ds1 = d2.select_method_wo_parameter(Sql1, "text");
                    int rowHeight = 0;
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        transrepo.Columns.Add("SNo", typeof(string));
                        transrepo.Columns.Add("Access No", typeof(string));
                        transrepo.Columns.Add("Title", typeof(string));
                        transrepo.Columns.Add("Edition", typeof(string));
                        transrepo.Columns.Add("Author", typeof(string));
                        transrepo.Columns.Add("Department", typeof(string));
                        transrepo.Columns.Add("Subject", typeof(string));
                        transrepo.Columns.Add("Call No", typeof(string));
                        transrepo.Columns.Add("Accession Date", typeof(string));
                        transrepo.Columns.Add("Price", typeof(string));
                        transrepo.Columns.Add("ISBN", typeof(string));
                        transrepo.Columns.Add("Pages", typeof(string));
                        transrepo.Columns.Add("Suppliers", typeof(string));
                        transrepo.Columns.Add("Bill Number", typeof(string));
                        transrepo.Columns.Add("Bill Date", typeof(string));
                        transrepo.Columns.Add("Year of Publication", typeof(string));
                        transrepo.Columns.Add("Remarks", typeof(string));
                        transrepo.Columns.Add("Library Name", typeof(string));


                        int row = 0;
                        int i = 0;
                        int SNo = 0;
                        for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                        {
                            SNo++;
                            dr = transrepo.NewRow();
                            dr["SNo"] = Convert.ToString(SNo);
                            dr["Access No"] = ds1.Tables[0].Rows[r]["Access No"].ToString();
                            dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                            dr["Edition"] = ds1.Tables[0].Rows[r]["Edition"].ToString();
                            dr["Author"] = ds1.Tables[0].Rows[r]["Author"].ToString();
                            dr["Department"] = ds1.Tables[0].Rows[r]["Department"].ToString();
                            dr["Subject"] = ds1.Tables[0].Rows[r]["Subject"].ToString();
                            dr["Call No"] = ds1.Tables[0].Rows[r]["Call No"].ToString();
                            dr["Accession Date"] = ds1.Tables[0].Rows[r]["Accession Date"].ToString();
                            dr["Price"] = ds1.Tables[0].Rows[r]["Price"].ToString();
                            dr["ISBN"] = ds1.Tables[0].Rows[r]["ISBN"].ToString();
                            dr["Pages"] = ds1.Tables[0].Rows[r]["Pages"].ToString();
                            dr["Suppliers"] = ds1.Tables[0].Rows[r]["Supplier"].ToString();
                            dr["Bill Number"] = ds1.Tables[0].Rows[r]["Bill Number"].ToString();
                            dr["Bill Date"] = ds1.Tables[0].Rows[r]["Bill Date"].ToString();
                            dr["Year of Publication"] = ds1.Tables[0].Rows[r]["Year of Publication"].ToString();
                            dr["Remarks"] = ds1.Tables[0].Rows[r]["Remarks"].ToString();
                            dr["Library Name"] = ds1.Tables[0].Rows[r]["Library Name"].ToString();
                            transrepo.Rows.Add(dr);
                        }
                        gridview2.DataSource = transrepo;
                        gridview2.DataBind();

                        gridview2.Visible = true;
                    }
                }
                #endregion

                #region Departmentwise Abstract

                else if (ddlreporttype.SelectedIndex == 20)
                {
                    if (ddldept.Text == "All")
                        strDegreecode = "%";
                    else
                        strDegreecode = ddldept.SelectedValue;
                    if (cbfrom.Checked == true)
                        strDate = "  bookdetails.bill_date >= '" + fromdate1 + "' and  bookdetails.bill_date <= '" + todate1 + "' and  ";
                    else
                        strDate = "";
                    if (ddllibrary.Text == "All")
                    {
                        if (rbdeptwise.SelectedValue == "Subjectwise Report")
                        {
                            if (ddldept.Text == "All")
                                Sql1 = "Select DISTINCT bookdetails.subject as 'Subject',call_no as 'Call No',bookdetails.call_des as 'Call Description',library.lib_name as 'Library Name' FROM bookdetails,library where " + strDate + " library.lib_code=bookdetails.lib_code";
                            else if (ddldept.Text != "All")
                                Sql1 = "Select DISTINCT bookdetails.subject as 'Subject',call_no as 'Call No',bookdetails.call_des as 'Call Description',library.lib_name as 'Library Name' FROM bookdetails,library where " + strDate + " library.lib_code=bookdetails.lib_code and (bookdetails.Dept_Code = '" + ddldept.Text + "') and bookdetails.lib_code ='" + ddllibrary.SelectedValue + "'";
                        }
                        else if (rbdeptwise.SelectedValue == "Total No.of Books")
                        {
                            //Sql1 = "SELECT distinct dept_code as 'Department Code',count(distinct(author))as 'Single',COUNT(title) as 'Volumes' From bookdetails WHERE " + strDate + " dept_code like '" + strDegreecode + "' group by dept_code";
                            Sql1 = "select a.dept_code as 'Department Code',sum(a.Single) as 'Single',0 as 'Volumes' from (select dept_code, count(distinct(title))as 'Single' from bookdetails WHERE dept_code like '" + strDegreecode + "'";
                            if (ddldepttype.Text == "Reference Books")
                                Sql1 = Sql1 + " and ref ='Yes'";
                            else if (ddldepttype.Text == "Text Books")
                                Sql1 = Sql1 + " and ref ='No'";
                            Sql1 = Sql1 + " group by dept_code,title,author) a";
                            Sql1 = Sql1 + " group by dept_code order by dept_code";
                        }
                    }
                    else if (ddllibrary.Text != "All")
                    {
                        if (rbdeptwise.SelectedValue == "Subjectwise Report")
                        {
                            if (ddldept.Text == "All")
                                Sql1 = "Select DISTINCT bookdetails.subject as 'Subject',call_no as 'Call No',bookdetails.call_des as 'Call Description',library.lib_name as 'Library Name' FROM bookdetails,library where " + strDate + " library.lib_code=bookdetails.lib_code and bookdetails.lib_code='" + ddllibrary.SelectedValue + "'";
                            else if (ddldept.Text != "All")
                                Sql1 = "Select DISTINCT bookdetails.subject as 'Subject',call_no as 'Call No',bookdetails.call_des as 'Call Description',library.lib_name as 'Library Name' FROM bookdetails,library where " + strDate + " library.lib_code=bookdetails.lib_code and bookdetails.Dept_Code like '" + strDegreecode + "' and bookdetails.lib_code='" + ddllibrary.SelectedValue + "'";
                        }
                        else if (rbdeptwise.SelectedValue == "Total No.of Books")
                        {
                            Sql1 = "select a.dept_code as 'Department Code',sum(a.Single) as 'Single',0 as 'Volumes' from (select dept_code, count(distinct(title))as 'Single' from bookdetails WHERE dept_code like '" + strDegreecode + "'";
                            Sql1 = Sql1 + "and bookdetails.lib_code='" + ddllibrary.SelectedValue + "' group by dept_code,title,author) a";
                            Sql1 = Sql1 + " group by dept_code order by dept_code";
                        }
                    }
                    ds1 = d2.select_method_wo_parameter(Sql1, "text");
                    if (rbdeptwise.SelectedValue == "Subjectwise Report")
                    {
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            transrepo.Columns.Add("SNo", typeof(string));
                            transrepo.Columns.Add("Subject", typeof(string));
                            transrepo.Columns.Add("Call No", typeof(string));
                            transrepo.Columns.Add("Call Description", typeof(string));
                            transrepo.Columns.Add("Library Name", typeof(string));


                            int row = 0;
                            int i = 0;
                            int SNo = 0;
                            for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                            {
                                SNo++;
                                dr = transrepo.NewRow();
                                dr["SNo"] = Convert.ToString(SNo);
                                dr["Subject"] = ds1.Tables[0].Rows[r]["Subject"].ToString();
                                dr["Call No"] = ds1.Tables[0].Rows[r]["Call No"].ToString();
                                dr["Call Description"] = ds1.Tables[0].Rows[r]["Call Description"].ToString();
                                dr["Library Name"] = ds1.Tables[0].Rows[r]["Library Name"].ToString();
                                transrepo.Rows.Add(dr);
                            }
                            gridview2.DataSource = transrepo;
                            gridview2.DataBind();

                            gridview2.Visible = true;
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "No Record Found";
                        }
                    }
                    else if (rbdeptwise.SelectedValue == "Total No.of Books")
                    {
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            transrepo.Columns.Add("SNo", typeof(string));
                            transrepo.Columns.Add("Department Code", typeof(string));
                            transrepo.Columns.Add("Single", typeof(string));
                            transrepo.Columns.Add("Volumes", typeof(string));

                            int i = 0;
                            int SNo = 0;
                            for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                            {
                                SNo++;
                                dr = transrepo.NewRow();
                                dr["SNo"] = Convert.ToString(SNo);
                                dr["Department Code"] = ds1.Tables[0].Rows[r]["Department Code"].ToString();
                                dr["Single"] = ds1.Tables[0].Rows[r]["Single"].ToString();
                                dr["Volumes"] = ds1.Tables[0].Rows[r]["Volumes"].ToString();
                                transrepo.Rows.Add(dr);
                            }
                            gridview2.DataSource = transrepo;
                            gridview2.DataBind();

                            gridview2.Visible = true;
                        }
                    }
                }
                #endregion

                #region Library Contents

                else if (ddlreporttype.SelectedIndex == 21)
                {
                    if (ddllibrary.Text == "All")
                    {
                        Sql11 = "select lib_code from library";
                        dsdetails = d2.select_method_wo_parameter(Sql11, "text");

                        if (dsdetails.Tables[0].Rows.Count > 0)
                        {
                            transrepo.Columns.Add("SNo", typeof(string));
                            transrepo.Columns.Add("No of Books", typeof(string));
                            transrepo.Columns.Add("No of Titles", typeof(string));
                            transrepo.Columns.Add("No of Journals", typeof(string));
                            transrepo.Columns.Add("No of Back Volumes", typeof(string));
                            transrepo.Columns.Add("No of Project Books", typeof(string));
                            transrepo.Columns.Add("No of NonBookMaterials", typeof(string));
                            transrepo.Columns.Add("Library Name", typeof(string));


                            int SNo = 0;
                            for (int r = 0; r < dsdetails.Tables[0].Rows.Count; r++)
                            {
                                Sql1 = "select (select count(*) from bookdetails where lib_code='" + dsdetails.Tables[0].Rows[r]["lib_code"] + "')as 'No of Books', (select count(distinct title) from bookdetails where lib_code='" + dsdetails.Tables[0].Rows[r]["lib_code"] + "')as 'No of Titles', (select count(*) from journal where lib_code='" + dsdetails.Tables[0].Rows[r]["lib_code"] + "')as 'No of Journals', (select count(*) from back_volume where lib_code='" + dsdetails.Tables[0].Rows[r]["lib_code"] + "')as 'No of Back Volumes', (select count(*) from project_book where lib_code='" + dsdetails.Tables[0].Rows[r]["lib_code"] + "')as 'No of Project Books', (select count(*) from nonbookmat where lib_code='" + dsdetails.Tables[0].Rows[r]["lib_code"] + "')as 'No of NonBookMaterials',(select lib_name from library where lib_code='" + dsdetails.Tables[0].Rows[r]["lib_code"] + "')as 'Library Name'";
                                ds1 = d2.select_method_wo_parameter(Sql1, "text");
                                if (ds1.Tables[0].Rows.Count > 0)
                                {
                                    SNo++;
                                    dr = transrepo.NewRow();
                                    dr["SNo"] = Convert.ToString(SNo);
                                    dr["No of Books"] = ds1.Tables[0].Rows[0]["No of Books"].ToString();
                                    dr["No of Titles"] = ds1.Tables[0].Rows[0]["No of Titles"].ToString();
                                    dr["No of Journals"] = ds1.Tables[0].Rows[0]["No of Journals"].ToString();
                                    dr["No of Back Volumes"] = ds1.Tables[0].Rows[0]["No of Back Volumes"].ToString();
                                    dr["No of Project Books"] = ds1.Tables[0].Rows[0]["No of Project Books"].ToString();
                                    dr["No of NonBookMaterials"] = ds1.Tables[0].Rows[0]["No of NonBookMaterials"].ToString();
                                    dr["Library Name"] = ds1.Tables[0].Rows[0]["Library Name"].ToString();
                                    transrepo.Rows.Add(dr);
                                }
                            }
                            gridview2.DataSource = transrepo;
                            gridview2.DataBind();

                            gridview2.Visible = true;
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "No Record Found";
                        }

                    }
                    else if (ddllibrary.Text != "All")
                    {
                        Sql1 = "Select (select count(*) from bookdetails where lib_code='" + ddllibrary.SelectedValue + "')as 'No of Books','No Of Title' =(select sum(a.tit) from (select count(distinct title) tit from bookdetails where lib_code =" + ddllibrary.SelectedValue + " group by lib_code,dept_code) a),(select count(title) from journal where lib_code='" + ddllibrary.SelectedValue + "')as 'No of Journals',(select count(*) from back_volume where lib_code='" + ddllibrary.SelectedValue + "')as 'No of BackVolumes',(select count(*) from project_book where lib_code='" + ddllibrary.SelectedValue + "')as 'No of ProjectBooks',(select count(*) from nonbookmat where lib_code='" + ddllibrary.SelectedValue + "')as 'No of NonBookMaterials',(select lib_name from library where lib_code='" + ddllibrary.SelectedValue + "')as 'Library Name'";
                        ds1 = d2.select_method_wo_parameter(Sql1, "text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            transrepo.Columns.Add("SNo", typeof(string));
                            transrepo.Columns.Add("No of Books", typeof(string));
                            transrepo.Columns.Add("No of Titles", typeof(string));
                            transrepo.Columns.Add("No of Journals", typeof(string));
                            transrepo.Columns.Add("No of Back Volumes", typeof(string));
                            transrepo.Columns.Add("No of Project Books", typeof(string));
                            transrepo.Columns.Add("No of NonBookMaterials", typeof(string));
                            transrepo.Columns.Add("Library Name", typeof(string));
                            int SNo = 0;
                            for (int r = 0; r < ds1.Tables[0].Rows.Count; r++)
                            {
                                SNo++;
                                dr = transrepo.NewRow();
                                dr["SNo"] = Convert.ToString(SNo);
                                dr["No of Books"] = ds1.Tables[0].Rows[0]["No of Books"].ToString();
                                dr["No of Titles"] = ds1.Tables[0].Rows[0]["No of Titles"].ToString();
                                dr["No of Journals"] = ds1.Tables[0].Rows[0]["No of Journals"].ToString();
                                dr["No of Back Volumes"] = ds1.Tables[0].Rows[0]["No of Back Volumes"].ToString();
                                dr["No of Project Books"] = ds1.Tables[0].Rows[0]["No of Project Books"].ToString();
                                dr["No of NonBookMaterials"] = ds1.Tables[0].Rows[0]["No of NonBookMaterials"].ToString();
                                dr["Library Name"] = ds1.Tables[0].Rows[0]["Library Name"].ToString();
                                transrepo.Rows.Add(dr);
                            }
                            gridview2.DataSource = transrepo;
                            gridview2.DataBind();

                            gridview2.Visible = true;
                        }
                    }
                }
                #endregion

                #region Induvudual Library Usage

                else if (ddlreporttype.SelectedIndex == 22)
                {
                    string strlibcode = "";
                    string strStatus = "";
                    if (cbfrom.Checked == true)
                    {
                        if (rblist.SelectedValue == "Issue List")
                            strDate = " AND Borrow_Date BETWEEN '" + fromdate1 + "' AND '" + todate1 + "'";
                        else if (rblist.SelectedValue == "Return List")
                            strDate = " AND Return_Date BETWEEN '" + fromdate1 + "' AND '" + todate1 + "'";
                        else if (rblist.SelectedValue == "Due List")
                            strDate = " AND Due_Date BETWEEN '" + fromdate1 + "' AND '" + todate1 + "'";
                        else if (rblist.SelectedValue == "All")
                            strDate = " AND ((Borrow_Date BETWEEN '" + fromdate1 + "' AND '" + todate1 + "') OR (Return_Date BETWEEN '" + fromdate1 + "' AND '" + todate1 + "') OR (Due_Date BETWEEN '" + fromdate1 + "' AND '" + todate1 + "'))";
                        else
                            strDate = "";
                    }
                    else
                        strDate = "";
                    if (ddllibrary.Text != "All")
                        strlibcode = " AND B.Lib_Code ='" + ddllibrary.SelectedValue + "' ";
                    else
                        strlibcode = "";

                    if (rblist.SelectedValue == "Issue List")
                        strStatus = " AND Return_Flag = 0 ";
                    else if (rblist.SelectedValue == "Return List")
                        strStatus = " AND Return_Flag = 1 ";
                    else if (rblist.SelectedValue == "Due List")
                        strStatus = " AND Return_Flag = 0 ";
                    else if (rblist.SelectedValue == "All")
                        strStatus = "";
                    else
                        strStatus = "";
                    if (ddlselectfor.Text == "Student")
                    {
                        Sql1 = "SELECT B.Roll_No as 'Roll No',B.Stud_Name as 'Student Name',Course_Name+'-'+Dept_Name as 'Course',Acc_No as 'Access No',Title as 'Title',Author as 'Author',CONVERT(varchar,borrow_date,103) as 'Borrow Date',CONVERT(varchar,Return_Date,103) as 'Return Date',CONVERT(varchar,due_date,103) as 'Due Date',Lib_Name as 'Library Name' ";
                        Sql1 = Sql1 + "FROM Borrow B,Library L,Registration R,Degree G,Course C,Department D ";
                        Sql1 = Sql1 + "WHERE B.Lib_Code=L.Lib_Code AND (B.Roll_No = R.Roll_No OR B.Roll_No = R.Lib_ID) and B.Roll_No = R.Roll_No";
                        Sql1 = Sql1 + " AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code ";
                        Sql1 = Sql1 + " AND B.Is_Staff = 0 " + strDate + strlibcode + strStatus;
                        if (txt_rolllno.Text != "")
                            Sql1 = Sql1 + " AND B.Roll_No ='" + txt_rolllno.Text + "' ";
                        if (txtname.Text != "")
                            Sql1 = Sql1 + " AND B.Stud_Name Like '" + txtname.Text + "%' ";
                        Sql1 = Sql1 + " ORDER BY Borrow_Date ";
                        ds1 = d2.select_method_wo_parameter(Sql1, "text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            transrepo.Columns.Add("SNo", typeof(string));
                            transrepo.Columns.Add("Roll No", typeof(string));
                            transrepo.Columns.Add("Student Names", typeof(string));
                            transrepo.Columns.Add("Course", typeof(string));
                            transrepo.Columns.Add("Access No", typeof(string));
                            transrepo.Columns.Add("Title", typeof(string));
                            transrepo.Columns.Add("Author", typeof(string));
                            transrepo.Columns.Add("Borrow Date", typeof(string));
                            transrepo.Columns.Add("Return Date", typeof(string));
                            transrepo.Columns.Add("Due Date", typeof(string));
                            transrepo.Columns.Add("Library Name", typeof(string));


                            int row = 0;
                            int i = 0;
                            int SNo = 0;
                            for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                            {
                                SNo++;
                                dr = transrepo.NewRow();
                                dr["SNo"] = Convert.ToString(SNo);
                                dr["Roll No"] = ds1.Tables[0].Rows[r]["Roll No"].ToString();
                                dr["Student Name"] = ds1.Tables[0].Rows[r]["Student Name"].ToString();
                                dr["Course"] = ds1.Tables[0].Rows[r]["Course"].ToString();
                                dr["Access No"] = ds1.Tables[0].Rows[r]["Access No"].ToString();
                                dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                                dr["Author"] = ds1.Tables[0].Rows[r]["Author"].ToString();
                                dr["Borrow Date"] = ds1.Tables[0].Rows[r]["Borrow Date"].ToString();
                                dr["Return Date"] = ds1.Tables[0].Rows[r]["Return Date"].ToString();
                                dr["Due Date"] = ds1.Tables[0].Rows[r]["Due Date"].ToString();
                                dr["Library Name"] = ds1.Tables[0].Rows[r]["Library Name"].ToString();
                                transrepo.Rows.Add(dr);
                            }
                            gridview2.DataSource = transrepo;
                            gridview2.DataBind();

                            gridview2.Visible = true;
                        }
                        else
                        {
                            gridview2.Visible = false;
                        }
                    }
                    else if (ddlselectfor.Text == "Staff")
                    {
                        Sql1 = "SELECT B.Roll_No as 'Staff Code',B.Stud_Name as 'Staff Name',Dept_Name as 'Department',Acc_No as 'Access No',Title as 'Title',Author as 'Author',CONVERT(varchar,borrow_date,103) as 'Borrow Date',CONVERT(varchar,due_date,103) as 'Due Date',Lib_Name as 'Library Name' ";
                        Sql1 = Sql1 + " FROM Borrow B,Library L,StaffMaster M,StaffTrans T,HrDept_Master D ";
                        Sql1 = Sql1 + " WHERE B.Lib_code=L.Lib_Code AND (B.Roll_No = M.Staff_Code OR B.Roll_No = M.Lib_ID) and B.Roll_No = M.Staff_Code";
                        Sql1 = Sql1 + " AND M.Staff_Code = T.Staff_Code AND T.Dept_Code = D.Dept_Code AND T.Latestrec = 1 ";
                        Sql1 = Sql1 + " AND B.Is_Staff = 1 " + strDate + strlibcode + strStatus;
                        if (txt_rolllno.Text != "")
                            Sql1 = Sql1 + " AND B.Roll_No ='" + txt_rolllno.Text + "' ";
                        if (txtname.Text != "")
                            Sql1 = Sql1 + " AND B.Stud_Name ='" + txtname.Text + "' ";
                        Sql1 = Sql1 + " ORDER BY Borrow_Date ";
                        ds1 = d2.select_method_wo_parameter(Sql1, "text");

                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            transrepo.Columns.Add("SNo", typeof(string));
                            transrepo.Columns.Add("Staff Code", typeof(string));
                            transrepo.Columns.Add("Staff Names", typeof(string));
                            transrepo.Columns.Add("Department", typeof(string));
                            transrepo.Columns.Add("Access No", typeof(string));
                            transrepo.Columns.Add("Title", typeof(string));
                            transrepo.Columns.Add("Author", typeof(string));
                            transrepo.Columns.Add("Borrow Date", typeof(string));
                            transrepo.Columns.Add("Due Date", typeof(string));
                            transrepo.Columns.Add("Library Name", typeof(string));


                            int row = 0;
                            int i = 0;
                            int SNo = 0;
                            for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                            {
                                SNo++;
                                dr = transrepo.NewRow();
                                dr["SNo"] = Convert.ToString(SNo);
                                dr["Staff Code"] = ds1.Tables[0].Rows[r]["Staff Code"].ToString();
                                dr["Staff Name"] = ds1.Tables[0].Rows[r]["Staff Name"].ToString();
                                dr["Department"] = ds1.Tables[0].Rows[r]["Department"].ToString();
                                dr["Access No"] = ds1.Tables[0].Rows[r]["Access No"].ToString();
                                dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                                dr["Author"] = ds1.Tables[0].Rows[r]["Author"].ToString();
                                dr["Borrow Date"] = ds1.Tables[0].Rows[r]["Borrow Date"].ToString();
                                dr["Due Date"] = ds1.Tables[0].Rows[r]["Due Date"].ToString();
                                dr["Library Name"] = ds1.Tables[0].Rows[r]["Library Name"].ToString();
                                transrepo.Rows.Add(dr);
                            }
                            gridview2.DataSource = transrepo;
                            gridview2.DataBind();

                            gridview2.Visible = true;
                        }
                        else
                        {
                            gridview2.Visible = false;
                        }
                    }
                    else if (ddlselectfor.Text == "All")
                    {
                        Sql1 = "SELECT B.Roll_No as 'Roll No',B.Stud_Name as 'Name',Course_Name+'-'+Dept_Name as 'Course',Acc_No as 'Access No',Title as 'Title',Author as 'Author',CONVERT(varchar,borrow_date,103) as 'Borrow Date',CONVERT(varchar,Return_Date,103) as 'Return Date',CONVERT(varchar,due_date,103) as 'Due Date',Lib_Name as 'Library Name' ";
                        Sql1 = Sql1 + " FROM Borrow B,Library L,Registration R,Degree G,Course C,Department D ";
                        Sql1 = Sql1 + " WHERE B.Lib_Code=L.Lib_Code AND (B.Roll_No = R.Roll_No OR B.Roll_No = R.Lib_ID) and B.Roll_No = R.Roll_No";
                        Sql1 = Sql1 + " AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code ";
                        Sql1 = Sql1 + " AND B.Is_Staff = 0 " + strDate + strlibcode + strStatus;
                        if (txt_rolllno.Text != "")
                            Sql1 = Sql1 + " AND B.Roll_No ='" + txt_rolllno.Text + "' ";
                        if (txtname.Text != "")
                            Sql1 = Sql1 + " AND B.Stud_Name Like '" + txtname.Text + "%' ";
                        Sql1 = Sql1 + " UNION ALL ";
                        Sql1 = Sql1 + "SELECT B.Roll_No as 'Staff Code',B.Stud_Name as 'Name',Dept_Name as 'Department',Acc_No as 'Access No',Title as 'Title',Author as 'Author',CONVERT(varchar,borrow_date,103) as 'Borrow Date',CONVERT(varchar,Return_Date,103) as 'Return Date',CONVERT(varchar,due_date,103) as 'Due Date',Lib_Name as 'Library Name' ";
                        Sql1 = Sql1 + " FROM Borrow B,Library L,StaffMaster M,StaffTrans T,HrDept_Master D ";
                        Sql1 = Sql1 + " WHERE B.Lib_code=L.Lib_Code AND (B.Roll_No = M.Staff_Code OR B.Roll_No = M.Lib_ID) and B.Roll_No = M.Staff_Code";
                        Sql1 = Sql1 + " AND M.Staff_Code = T.Staff_Code AND T.Dept_Code = D.Dept_Code AND T.Latestrec = 1 ";
                        Sql1 = Sql1 + " AND B.Is_Staff = 1 " + strDate + strlibcode + strStatus;
                        if (txt_rolllno.Text != "")
                            Sql1 = Sql1 + " AND B.Roll_No ='" + txt_rolllno.Text + "' ";
                        if (txtname.Text != "")
                            Sql1 = Sql1 + " AND B.Stud_Name ='" + txtname.Text + "' ";
                        ds1 = d2.select_method_wo_parameter(Sql1, "text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            transrepo.Columns.Add("SNo", typeof(string));
                            transrepo.Columns.Add("Roll No", typeof(string));
                            transrepo.Columns.Add("Name", typeof(string));
                            transrepo.Columns.Add("Course", typeof(string));
                            transrepo.Columns.Add("Access No", typeof(string));
                            transrepo.Columns.Add("Title", typeof(string));
                            transrepo.Columns.Add("Author", typeof(string));
                            transrepo.Columns.Add("Borrow Date", typeof(string));
                            transrepo.Columns.Add("Return Date", typeof(string));
                            transrepo.Columns.Add("Due Date", typeof(string));
                            transrepo.Columns.Add("Library Name", typeof(string));

                            int i = 0;
                            int SNo = 0;
                            for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                            {
                                SNo++;
                                dr = transrepo.NewRow();
                                dr["SNo"] = Convert.ToString(SNo);
                                dr["Roll No"] = ds1.Tables[0].Rows[r]["Roll No"].ToString();
                                dr["Name"] = ds1.Tables[0].Rows[r]["Name"].ToString();
                                dr["Course"] = ds1.Tables[0].Rows[r]["Course"].ToString();
                                dr["Access No"] = ds1.Tables[0].Rows[r]["Access No"].ToString();
                                dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                                dr["Author"] = ds1.Tables[0].Rows[r]["Author"].ToString();
                                dr["Borrow Date"] = ds1.Tables[0].Rows[r]["Borrow Date"].ToString();
                                dr["Return Date"] = ds1.Tables[0].Rows[r]["Return Date"].ToString();
                                dr["Due Date"] = ds1.Tables[0].Rows[r]["Due Date"].ToString();
                                dr["Library Name"] = ds1.Tables[0].Rows[r]["Library Name"].ToString();
                                transrepo.Rows.Add(dr);
                            }
                            gridview2.DataSource = transrepo;
                            gridview2.DataBind();

                            gridview2.Visible = true;
                        }
                        else
                        {
                            gridview2.Visible = false;
                        }
                    }
                }
                #endregion

                #region Returned Books Cum Reserved

                else if (ddlreporttype.SelectedIndex == 23)
                {
                    if (ddllibrary.Text != "All" && ddlselectfor.Text == "Student")
                        Sql1 = "select * from borrow,priority_studstaff as pri where pri.cancel_flag=0 and borrow.acc_no=pri.access_number and borrow.is_staff=0 and return_flag=1 and borrow.lib_code='" + ddllibrary.SelectedValue + "'";
                    else if (ddllibrary.Text != "All" && ddlselectfor.Text == "Staff")
                        Sql1 = "select * from borrow,priority_studstaff as pri where pri.cancel_flag=0 and borrow.acc_no=pri.access_number and borrow.is_staff=1 and return_flag=1 and borrow.lib_code='" + ddllibrary.SelectedValue + "'";
                    else if (ddllibrary.Text == "All" && ddlselectfor.Text == "All")
                        Sql1 = "select * from borrow,priority_studstaff as pri where pri.cancel_flag=0 and borrow.acc_no=pri.access_number and return_flag=1 ";
                    else if (ddllibrary.Text == "All" && ddlselectfor.Text == "Student")
                        Sql1 = "select * from borrow,priority_studstaff as pri where pri.cancel_flag=0 and borrow.acc_no=pri.access_number and borrow.is_staff=0 and return_flag=1 ";
                    else if (ddllibrary.Text == "All" && ddlselectfor.Text == "Staff")
                        Sql1 = "select * from borrow,priority_studstaff as pri where pri.cancel_flag=0 and borrow.acc_no=pri.access_number and borrow.is_staff=1 and return_flag=1";
                    else if (ddllibrary.Text != "All" && ddlselectfor.Text == "All")
                        Sql1 = "select * from borrow,priority_studstaff as pri where pri.cancel_flag=0 and borrow.acc_no=pri.access_number and borrow.lib_code='" + ddllibrary.SelectedValue + "' and return_flag=1 ";
                    ds1 = d2.select_method_wo_parameter(Sql1, "text");
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        transrepo.Columns.Add("SNo", typeof(string));
                        transrepo.Columns.Add("acc_no", typeof(string));
                        transrepo.Columns.Add("title", typeof(string));
                        transrepo.Columns.Add("author", typeof(string));
                        transrepo.Columns.Add("call_no", typeof(string));
                        transrepo.Columns.Add("token_no", typeof(string));
                        transrepo.Columns.Add("stud_name", typeof(string));
                        transrepo.Columns.Add("is_staff", typeof(string));
                        transrepo.Columns.Add("borrow_date", typeof(string));
                        transrepo.Columns.Add("due_date", typeof(string));
                        transrepo.Columns.Add("return_date", typeof(string));
                        transrepo.Columns.Add("return_type", typeof(string));
                        transrepo.Columns.Add("access_date", typeof(string));
                        transrepo.Columns.Add("access_time", typeof(string));
                        transrepo.Columns.Add("lib_code", typeof(string));
                        transrepo.Columns.Add("return_flag", typeof(string));
                        transrepo.Columns.Add("cirno_issue", typeof(string));
                        transrepo.Columns.Add("cirno_return", typeof(string));
                        transrepo.Columns.Add("renewflag", typeof(string));
                        transrepo.Columns.Add("book_issuedby", typeof(string));
                        transrepo.Columns.Add("book_returnby", typeof(string));
                        transrepo.Columns.Add("mode", typeof(string));
                        transrepo.Columns.Add("renewstatus", typeof(string));
                        transrepo.Columns.Add("renewaltimes", typeof(string));
                        transrepo.Columns.Add("Issued_Time", typeof(string));
                        transrepo.Columns.Add("Returned_Time", typeof(string));
                        transrepo.Columns.Add("cur_Date", typeof(string));
                        transrepo.Columns.Add("cur_time", typeof(string));
                        transrepo.Columns.Add("roll_no", typeof(string));
                        transrepo.Columns.Add("staff_code", typeof(string));
                        transrepo.Columns.Add("access_number", typeof(string));
                        transrepo.Columns.Add("cancel_flag", typeof(string));
                        transrepo.Columns.Add("OtherAcc_No", typeof(string));
                        transrepo.Columns.Add("Code", typeof(string));
                        transrepo.Columns.Add("Can_Reason", typeof(string));
                        transrepo.Columns.Add("Is_Staff", typeof(string));


                        int row = 0;
                        int i = 0;
                        int SNo = 0;
                        for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                        {
                            SNo++;
                            dr = transrepo.NewRow();
                            dr["SNo"] = Convert.ToString(SNo);
                            dr["acc_no"] = ds1.Tables[0].Rows[r]["acc_no"].ToString();
                            dr["title"] = ds1.Tables[0].Rows[r]["title"].ToString();
                            dr["author"] = ds1.Tables[0].Rows[r]["author"].ToString();
                            dr["call_no"] = ds1.Tables[0].Rows[r]["call_no"].ToString();
                            dr["token_no"] = ds1.Tables[0].Rows[r]["token_no"].ToString();
                            dr["roll_no"] = ds1.Tables[0].Rows[r]["roll_no"].ToString();
                            dr["stud_name"] = ds1.Tables[0].Rows[r]["stud_name"].ToString();
                            dr["is_staff"] = ds1.Tables[0].Rows[r]["is_staff"].ToString();
                            dr["borrow_date"] = ds1.Tables[0].Rows[r]["borrow_date"].ToString();
                            dr["due_date"] = ds1.Tables[0].Rows[r]["due_date"].ToString();
                            dr["return_date"] = ds1.Tables[0].Rows[r]["return_date"].ToString();
                            dr["return_type"] = ds1.Tables[0].Rows[r]["return_type"].ToString();
                            dr["access_date"] = ds1.Tables[0].Rows[r]["access_date"].ToString();
                            dr["access_time"] = ds1.Tables[0].Rows[r]["access_time"].ToString();
                            dr["lib_code"] = ds1.Tables[0].Rows[r]["lib_code"].ToString();
                            dr["return_flag"] = ds1.Tables[0].Rows[r]["return_flag"].ToString();
                            dr["cirno_issue"] = ds1.Tables[0].Rows[r]["cirno_issue"].ToString();
                            dr["cirno_return"] = ds1.Tables[0].Rows[r]["cirno_return"].ToString();
                            dr["renewflag"] = ds1.Tables[0].Rows[r]["renewflag"].ToString();
                            dr["book_issuedby"] = ds1.Tables[0].Rows[r]["book_issuedby"].ToString();
                            dr["book_returnby"] = ds1.Tables[0].Rows[r]["book_returnby"].ToString();
                            dr["mode"] = ds1.Tables[0].Rows[r]["mode"].ToString();
                            dr["renewstatus"] = ds1.Tables[0].Rows[r]["renewstatus"].ToString();
                            dr["renewaltimes"] = ds1.Tables[0].Rows[r]["renewaltimes"].ToString();
                            dr["Issued_Time"] = ds1.Tables[0].Rows[r]["Issued_Time"].ToString();
                            dr["Returned_Time"] = ds1.Tables[0].Rows[r]["Returned_Time"].ToString();
                            dr["cur_Date"] = ds1.Tables[0].Rows[r]["cur_Date"].ToString();
                            dr["cur_time"] = ds1.Tables[0].Rows[r]["cur_time"].ToString();
                            dr["roll_no"] = ds1.Tables[0].Rows[r]["roll_no"].ToString();
                            dr["staff_code"] = ds1.Tables[0].Rows[r]["staff_code"].ToString();
                            dr["access_number"] = ds1.Tables[0].Rows[r]["access_number"].ToString();
                            dr["cancel_flag"] = ds1.Tables[0].Rows[r]["cancel_flag"].ToString();
                            dr["OtherAcc_No"] = ds1.Tables[0].Rows[r]["OtherAcc_No"].ToString();
                            dr["Code"] = ds1.Tables[0].Rows[r]["Code"].ToString();
                            dr["Can_Reason"] = ds1.Tables[0].Rows[r]["Can_Reason"].ToString();
                            dr["Is_Staff"] = ds1.Tables[0].Rows[r]["Is_Staff"].ToString();
                            transrepo.Rows.Add(dr);
                        }
                        gridview2.DataSource = transrepo;
                        gridview2.DataBind();

                        gridview2.Visible = true;
                    }
                }
                #endregion

                #region Reservation Report

                else if (ddlreporttype.SelectedIndex == 24)
                {
                    string lcode = "";
                    string strStatus = "";
                    if (ddllibrary.Text != "All")
                        lcode = ddllibrary.SelectedValue;
                    else
                        lcode = "%";

                    if (ddlstatus1.Text == "All")
                        strStatus = "";
                    else if (ddlstatus1.Text == "Cancelled")
                        strStatus = " and cancel_flag=1";
                    else if (ddlstatus1.Text == "Reserved")
                        strStatus = " and cancel_flag=0 ";

                    if (cbfrom.Checked == true)
                        strDate = " and p.access_date between '" + fromdate1 + "' and '" + todate1 + "'";
                    else
                        strDate = "";

                    if (ddlselectfor.Text == "All")
                    {
                        Sql1 = " select distinct p.roll_no + ' - ' + r.stud_name as 'Reserved By',p.access_number as 'Access No',p.title as 'Title',p.access_date as 'Date',p.access_time as 'Time',case when cancel_flag=1 then 'Cancelled' else 'Reserved' end as 'Status' from priority_studstaff as p, registration as r Where p.roll_no = r.roll_no And p.lib_code like '" + lcode + "' " + strStatus + strDate + " union all";
                        Sql1 = Sql1 + " select distinct p.staff_code + ' - ' + s.staff_name,p.access_number,p.title,p.access_date,p.access_time,case when cancel_flag=1 then 'Cancelled' else 'Reserved' end as 'Status' from priority_studstaff as p, staffmaster as s Where p.staff_code = S.staff_code And p.staff_code like '" + lcode + "'" + strStatus + strDate;
                        Sql1 = Sql1 + " union all select distinct u.user_id + ' - ' + u.name,p.access_number,p.title,p.access_date,p.access_time,case when cancel_flag=1 then 'Cancelled' else 'Reserved' end as 'Status' from priority_studstaff as p, user_master as u,borrow as b Where p.staff_code =u.user_id And p.lib_code like '" + lcode + "'" + strStatus + strDate;
                    }
                    else if (ddlselectfor.Text == "Student")
                    {
                        Sql1 = "select distinct p.roll_no + ' - ' + r.stud_name as 'Reserved By',p.access_number as 'Access No',p.title as 'Title',p.access_date as 'Date',p.access_time as 'Time',case when cancel_flag=1 then 'Cancelled' else 'Reserved' end as 'Status' from priority_studstaff as p, registration as r Where p.roll_no = r.roll_no And p.lib_code like '" + lcode + "' " + strStatus + strDate;
                        Sql1 = Sql1 + " union all select distinct p.roll_no + ' - ' + u.name,p.access_number,p.title,p.access_date,p.access_time,case when cancel_flag=1 then 'Cancelled' else 'Reserved' end as 'Status' from priority_studstaff as p, user_master as u,borrow as b  Where p.roll_no = u.user_id And u.is_staff=0 and p.lib_code like '" + lcode + "'" + strStatus + strDate;
                    }
                    else if (ddlselectfor.Text == "Staff")
                    {
                        Sql1 = "select distinct p.staff_code + ' - ' + s.staff_name as 'Reserved By',p.access_number as 'Access No',p.title as 'Title',p.access_date as 'Date',p.access_time as 'Time',case when cancel_flag=1 then 'Cancelled' else 'Reserved' end as 'Status' from priority_studstaff as p, staffmaster as s,borrow as b Where p.staff_code = S.staff_code And p.lib_code like '" + lcode + "' " + strStatus + strDate;
                        Sql1 = Sql1 + " union all select distinct p.staff_code + ' - ' + u.name,p.access_number,p.title,p.access_date,p.access_time,case when cancel_flag=1 then 'Cancelled' else 'Reserved' end as 'Status' from priority_studstaff as p, user_master as u,borrow as b Where p.staff_code =u.user_id And u.is_staff=1 and p.lib_code like '" + lcode + "'" + strStatus + strDate;
                    }
                    ds1 = d2.select_method_wo_parameter(Sql1, "text");
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        transrepo.Columns.Add("SNo", typeof(string));
                        transrepo.Columns.Add("Reserved By", typeof(string));
                        transrepo.Columns.Add("Access No", typeof(string));
                        transrepo.Columns.Add("Title", typeof(string));
                        transrepo.Columns.Add("Date", typeof(string));
                        transrepo.Columns.Add("Time", typeof(string));
                        transrepo.Columns.Add("Status", typeof(string));


                        int i = 0;
                        int SNo = 0;
                        for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                        {
                            SNo++;
                            dr = transrepo.NewRow();
                            dr["SNo"] = Convert.ToString(SNo);
                            dr["Reserved By"] = ds1.Tables[0].Rows[r]["Reserved By"].ToString();
                            dr["Access No"] = ds1.Tables[0].Rows[r]["Access No"].ToString();
                            dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                            dr["Date"] = ds1.Tables[0].Rows[r]["Date"].ToString();
                            dr["Time"] = ds1.Tables[0].Rows[r]["Time"].ToString();
                            dr["Status"] = ds1.Tables[0].Rows[r]["Status"].ToString();
                            transrepo.Rows.Add(dr);
                        }
                        gridview2.DataSource = transrepo;
                        gridview2.DataBind();

                        gridview2.Visible = true;
                    }
                }
                #endregion

                #region OverDue Books

                else if (ddlreporttype.SelectedIndex == 25)
                {
                    if (cbfrom.Checked == true)
                        strDate = " AND Due_Date BETWEEN '" + fromdate1 + "' and '" + todate1 + "'";
                    else
                        strDate = "";
                    if (ddlselectfor.Text == "Student")
                    {
                        Sql1 = "SELECT CASE WHEN B.Is_Staff = 0 THEN 'Student' ELSE 'Staff' END 'Type',Acc_No as 'Access No',Title as 'Title',Author as 'Author',B.Roll_No,B.Stud_Name as 'Student Name',Token_No as 'Card No', CONVERT(varchar,borrow_date,103) as 'Borrow Date',CONVERT(varchar,due_date,103) as 'Due Date', (abs(datediff(d,getdate(),due_date))) as 'Due Days',0 as Fine,L.lib_name  as 'Library Name' ";
                        Sql1 = Sql1 + "FROM Borrow B,Library L,Registration R,Degree G,Course C,Department D ";
                        Sql1 = Sql1 + "WHERE B.Lib_Code=L.Lib_Code AND (B.Roll_No = R.Roll_No OR B.Roll_No = R.Lib_ID) and B.Roll_No = R.Roll_No ";
                        Sql1 = Sql1 + "AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code ";
                        Sql1 = Sql1 + "AND Return_Flag=0 AND Is_Staff = 0 AND Due_Date < GETDATE() " + strDate;
                        if (ddllibrary.Text != "All")
                            Sql1 = Sql1 + " AND L.Lib_Code='" + ddllibrary.SelectedValue + "'";
                        if (ddldept.Text != "All")
                            Sql1 = Sql1 + " AND R.Degree_Code =" + ddldept.SelectedValue;
                        if (cbresignedstaff.Checked == true)
                            Sql1 = Sql1 + "AND (R.CC = 1 OR R.DelFlag <> 0 OR Exam_Flag <> 'OK') ";
                        if (ddlaccno.Text.Trim() == "Less Than")
                        {
                            if (Convert.ToInt32(txt_accno.Text) >= 0)
                                Sql1 = Sql1 + "AND ((abs(datediff(d,getdate(),due_date))) <" + Convert.ToInt32(txt_accno.Text) + ")";
                        }
                        else if (ddlaccno.Text.Trim() == "Greater Than")
                        {
                            if (Convert.ToInt32(txt_accno.Text) >= 0)
                                Sql1 = Sql1 + "AND ((abs(datediff(d,getdate(),due_date))) >" + Convert.ToInt32(txt_accno.Text) + ")";
                        }
                        else if (ddlaccno.Text.Trim() == "Equal to")
                        {
                            if (Convert.ToInt32(txt_accno.Text) >= 0)
                                Sql1 = Sql1 + "AND ((abs(datediff(d,getdate(),due_date))) =" + Convert.ToInt32(txt_accno.Text) + ")";
                        }
                        else if (ddlaccno.Text.Trim() == "Between")
                        {
                            if (Convert.ToInt32(txt_accno.Text) >= 0 && Convert.ToInt32(txt_accno2.Text) >= 0)
                                Sql1 = Sql1 + "AND ((abs(datediff(d,getdate(),due_date))) >=" + Convert.ToInt32(txt_accno.Text) + " AND ((abs(datediff(d,getdate(),due_date))) <=" + Convert.ToInt32(txt_accno2.Text) + "))";
                        }
                        Sql1 = Sql1 + " ORDER BY (abs(datediff(d,getdate(),due_date))) DESC";
                    }
                    else if (ddlselectfor.Text == "Staff")
                    {
                        Sql1 = "SELECT CASE WHEN B.Is_Staff = 0 THEN 'Student' ELSE 'Staff' END 'Type',Acc_No as 'Access No',Title as 'Title',Author as 'Author',B.Roll_No,B.Stud_Name as 'Staff Name',Token_No as 'Card No', CONVERT(varchar,borrow_date,103) as 'Borrow Date',CONVERT(varchar,due_date,103) as 'Due Date', (abs(datediff(d,getdate(),due_date))) as 'Due Days',0 as Fine,L.Lib_Name as 'Library Name' ";
                        Sql1 = Sql1 + "FROM Borrow B,Library L,StaffMaster M,StaffTrans T,HrDept_Master D ";
                        Sql1 = Sql1 + "WHERE B.Lib_code=L.Lib_Code AND (B.Roll_No = M.Staff_Code OR B.Roll_No = M.Lib_ID) and B.Roll_No = M.Staff_Code ";
                        Sql1 = Sql1 + "AND M.Staff_Code = T.Staff_Code AND T.Dept_Code = D.Dept_Code AND T.Latestrec = 1 ";
                        Sql1 = Sql1 + "AND Return_Flag=0 AND Is_Staff = 1 AND Due_Date < GETDATE() " + strDate;
                        if (ddllibrary.Text != "All")
                            Sql1 = Sql1 + "AND L.Lib_Code='" + ddllibrary.SelectedValue + "'";
                        if (ddldept.Text != "All")
                            Sql1 = Sql1 + "AND D.Dept_Name ='" + ddldept.Text + "'";
                        if (cbresignedstaff.Checked == true)
                            Sql1 = Sql1 + "AND (M.Resign = 1 AND M.Settled = 1) ";
                        if (ddlaccno.Text.Trim() == "Less Than")
                        {
                            if (Convert.ToInt32(txt_accno.Text) >= 0)
                                Sql1 = Sql1 + "AND ((abs(datediff(d,getdate(),due_date))) <" + Convert.ToInt32(txt_accno.Text) + ")";
                        }
                        else if (ddlaccno.Text.Trim() == "Greater Than")
                        {
                            if (Convert.ToInt32(txt_accno.Text) >= 0)
                                Sql1 = Sql1 + "AND ((abs(datediff(d,getdate(),due_date))) >" + Convert.ToInt32(txt_accno.Text) + ")";
                        }
                        else if (ddlaccno.Text.Trim() == "Equal to")
                        {
                            if (Convert.ToInt32(txt_accno.Text) >= 0)
                                Sql1 = Sql1 + "AND ((abs(datediff(d,getdate(),due_date))) =" + Convert.ToInt32(txt_accno.Text) + ")";
                        }
                        else if (ddlaccno.Text.Trim() == "Between")
                        {
                            if (Convert.ToInt32(txt_accno.Text) >= 0 && Convert.ToInt32(txt_accno2.Text) >= 0)
                                Sql1 = Sql1 + "AND ((abs(datediff(d,getdate(),due_date))) >=" + Convert.ToInt32(txt_accno.Text) + " AND ((abs(datediff(d,getdate(),due_date))) <=" + Convert.ToInt32(txt_accno2.Text) + "))";
                        }
                        Sql1 = Sql1 + " ORDER BY (abs(datediff(d,getdate(),due_date))) DESC";
                    }
                    else if (ddlselectfor.Text == "All")
                    {
                        Sql1 = "SELECT CASE WHEN B.Is_Staff = 0 THEN 'Student' ELSE 'Staff' END 'Type',Acc_No as 'Access No',Title as 'Title',Author as 'Author',B.Roll_No,B.Stud_Name as 'Student Name',Token_No as 'Card No', CONVERT(varchar,borrow_date,103) as 'Borrow Date',CONVERT(varchar,due_date,103) as 'Due Date', (abs(datediff(d,getdate(),due_date))) as 'Due Days',0 as Fine,L.lib_name  as 'Library Name' ";
                        Sql1 = Sql1 + "FROM Borrow B,Library L,Registration R,Degree G,Course C,Department D ";
                        Sql1 = Sql1 + "WHERE B.Lib_Code=L.Lib_Code AND (B.Roll_No = R.Roll_No OR B.Roll_No = R.Lib_ID) and B.Roll_No = R.Roll_No ";
                        Sql1 = Sql1 + "AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code ";
                        Sql1 = Sql1 + "AND Return_Flag=0 AND Is_Staff = 0 AND Due_Date < GETDATE() " + strDate;
                        if (ddllibrary.Text != "All")
                            Sql1 = Sql1 + " AND L.Lib_Code='" + ddllibrary.SelectedValue + "'";
                        if (ddldept.Text != "All")
                            Sql1 = Sql1 + " AND R.Degree_Code =" + ddldept.SelectedValue;
                        if (cbresignedstaff.Checked == true)
                            Sql1 = Sql1 + "AND (R.CC = 1 OR R.DelFlag <> 0 OR Exam_Flag <> 'OK') ";
                        if (ddlaccno.Text.Trim() == "Less Than")
                        {
                            if (Convert.ToInt32(txt_accno.Text) >= 0)
                                Sql1 = Sql1 + "AND ((abs(datediff(d,getdate(),due_date))) <" + Convert.ToInt32(txt_accno.Text) + ")";
                        }
                        else if (ddlaccno.Text.Trim() == "Greater Than")
                        {
                            if (Convert.ToInt32(txt_accno.Text) >= 0)
                                Sql1 = Sql1 + "AND ((abs(datediff(d,getdate(),due_date))) >" + Convert.ToInt32(txt_accno.Text) + ")";
                        }
                        else if (ddlaccno.Text.Trim() == "Equal to")
                        {
                            if (Convert.ToInt32(txt_accno.Text) >= 0)
                                Sql1 = Sql1 + "AND ((abs(datediff(d,getdate(),due_date))) =" + Convert.ToInt32(txt_accno.Text) + ")";
                        }
                        else if (ddlaccno.Text.Trim() == "Between")
                        {
                            if (Convert.ToInt32(txt_accno.Text) >= 0 && Convert.ToInt32(txt_accno2.Text) >= 0)
                                Sql1 = Sql1 + "AND ((abs(datediff(d,getdate(),due_date))) >=" + Convert.ToInt32(txt_accno.Text) + " AND ((abs(datediff(d,getdate(),due_date))) <=" + Convert.ToInt32(txt_accno2.Text) + "))";
                        }
                        Sql1 = Sql1 + " UNION ALL ";
                        Sql1 = Sql1 + "SELECT CASE WHEN B.Is_Staff = 0 THEN 'Student' ELSE 'Staff' END 'Type',Acc_No as 'Access No',Title as 'Title',Author as 'Author',B.Roll_No,B.Stud_Name as 'Staff Name',Token_No as 'Card No', CONVERT(varchar,borrow_date,103) as 'Borrow Date',CONVERT(varchar,due_date,103) as 'Due Date', (abs(datediff(d,getdate(),due_date))) as 'Due Days',0 as Fine,L.Lib_Name as 'Library Name' ";
                        Sql1 = Sql1 + "FROM Borrow B,Library L,StaffMaster M,StaffTrans T,HrDept_Master D ";
                        Sql1 = Sql1 + "WHERE B.Lib_code=L.Lib_Code AND (B.Roll_No = M.Staff_Code OR B.Roll_No = M.Lib_ID) and B.Roll_No = M.Staff_Code ";
                        Sql1 = Sql1 + "AND M.Staff_Code = T.Staff_Code AND T.Dept_Code = D.Dept_Code AND T.Latestrec = 1 ";
                        Sql1 = Sql1 + "AND Return_Flag=0 AND Is_Staff = 1 AND Due_Date < GETDATE() " + strDate;
                        if (ddllibrary.Text != "All")
                            Sql1 = Sql1 + "AND L.Lib_Code='" + ddllibrary.SelectedValue + "'";
                        if (ddldept.Text != "All")
                            Sql1 = Sql1 + "AND D.Dept_Name ='" + ddldept.Text + "'";
                        if (cbresignedstaff.Checked == true)
                            Sql1 = Sql1 + "AND (M.Resign = 1 AND M.Settled = 1) ";
                        if (ddlaccno.Text.Trim() == "Less Than")
                        {
                            if (Convert.ToInt32(txt_accno.Text) >= 0)
                                Sql1 = Sql1 + "AND ((abs(datediff(d,getdate(),due_date))) <" + Convert.ToInt32(txt_accno.Text) + ")";
                        }
                        else if (ddlaccno.Text.Trim() == "Greater Than")
                        {
                            if (Convert.ToInt32(txt_accno.Text) >= 0)
                                Sql1 = Sql1 + "AND ((abs(datediff(d,getdate(),due_date))) >" + Convert.ToInt32(txt_accno.Text) + ")";
                        }
                        else if (ddlaccno.Text.Trim() == "Equal to")
                        {
                            if (Convert.ToInt32(txt_accno.Text) >= 0)
                                Sql1 = Sql1 + "AND ((abs(datediff(d,getdate(),due_date))) =" + Convert.ToInt32(txt_accno.Text) + ")";
                        }
                        else if (ddlaccno.Text.Trim() == "Between")
                        {
                            if (Convert.ToInt32(txt_accno.Text) >= 0 && Convert.ToInt32(txt_accno2.Text) >= 0)
                                Sql1 = Sql1 + "AND ((abs(datediff(d,getdate(),due_date))) >=" + Convert.ToInt32(txt_accno.Text) + " AND ((abs(datediff(d,getdate(),due_date))) <=" + Convert.ToInt32(txt_accno2.Text) + "))";
                        }
                        Sql1 = Sql1 + " ORDER BY (abs(datediff(d,getdate(),due_date))) DESC";
                    }
                    ds1 = d2.select_method_wo_parameter(Sql1, "text");
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        transrepo.Columns.Add("SNo", typeof(string));
                        transrepo.Columns.Add("Type", typeof(string));
                        transrepo.Columns.Add("Access No", typeof(string));
                        transrepo.Columns.Add("Title", typeof(string));
                        transrepo.Columns.Add("Author", typeof(string));
                        transrepo.Columns.Add("Roll No", typeof(string));
                        if (ddlselectfor.Text == "Student" || ddlselectfor.Text == "All")
                        {
                            transrepo.Columns.Add("Student Name", typeof(string));
                        }
                        else
                        {
                            transrepo.Columns.Add("Staff Name", typeof(string));
                        }
                        transrepo.Columns.Add("Card No", typeof(string));
                        transrepo.Columns.Add("Borrow Date", typeof(string));
                        transrepo.Columns.Add("Due Date", typeof(string));
                        transrepo.Columns.Add("Due Days", typeof(string));
                        transrepo.Columns.Add("Fine", typeof(string));
                        transrepo.Columns.Add("Library Name", typeof(string));


                        int row = 0;
                        int i = 0;
                        int SNo = 0;
                        for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                        {
                            SNo++;
                            dr = transrepo.NewRow();
                            dr["SNo"] = Convert.ToString(SNo);
                            dr["Type"] = ds1.Tables[0].Rows[r]["Type"].ToString();
                            dr["Access No"] = ds1.Tables[0].Rows[r]["Access No"].ToString();
                            dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                            dr["Author"] = ds1.Tables[0].Rows[r]["Author"].ToString();
                            dr["Roll No"] = ds1.Tables[0].Rows[r]["Roll_No"].ToString();
                            if (ddlselectfor.Text == "Student" || ddlselectfor.Text == "All")
                            {
                                dr["Student Name"] = ds1.Tables[0].Rows[r]["Student Name"].ToString();
                            }
                            else
                            {
                                dr["Staff Name"] = ds1.Tables[0].Rows[r]["Staff Name"].ToString();
                            }
                            dr["Card No"] = ds1.Tables[0].Rows[r]["Card No"].ToString();
                            dr["Borrow Date"] = ds1.Tables[0].Rows[r]["Borrow Date"].ToString();
                            dr["Due Date"] = ds1.Tables[0].Rows[r]["Due Date"].ToString();
                            dr["Due Days"] = ds1.Tables[0].Rows[r]["Due Days"].ToString();
                            dr["Fine"] = ds1.Tables[0].Rows[r]["Fine"].ToString();
                            dr["Library Name"] = ds1.Tables[0].Rows[r]["Library Name"].ToString();
                            transrepo.Rows.Add(dr);
                        }
                        gridview2.DataSource = transrepo;
                        gridview2.DataBind();

                        gridview2.Visible = true;
                    }
                }
                #endregion

                #region RackInformation

                else if (ddlreporttype.SelectedIndex == 26)
                {
                    if (cbfrom.Checked == true)
                        strDate = " and rack_allocation.access_date between '" + fromdate1 + "' and '" + todate1 + "'";
                    else
                        strDate = "";
                    if (ddldept.Text != "All")
                    {
                        if (ddlrackno.Text == "All" && ddlshelfno.Text != "All")
                            Sql1 = "Select rack_allocation.acc_no as 'Access No',title as 'Title',author as 'Author',call_no as 'Call No',rack_allocation.access_date as 'Access Date' ,rack_no as 'Rack No' from rack_allocation,bookdetails,library where library.lib_code=rack_allocation.lib_code and rack_allocation.acc_no=bookdetails.acc_no " + strDate + " and  row_no='" + ddlshelfno.Text + "' and bookdetails.dept_code='" + ddldept.Text + "'";
                        else if (ddlrackno.Text != "All" && ddlshelfno.Text == "All")
                            Sql1 = "Select rack_allocation.acc_no as 'Access No',title as 'Title',author as 'Author',call_no as 'Call No',rack_allocation.access_date as 'Access Date',rack_no as 'Rack No' from rack_allocation,bookdetails,library where library.lib_code=rack_allocation.lib_code and rack_allocation.acc_no=bookdetails.acc_no  and rack_no='" + ddlrackno.Text + "'" + strDate + " and bookdetails.dept_code='" + ddldept.Text + "'";
                        else if (ddlrackno.Text == "All" && ddlshelfno.Text == "All")

                            Sql1 = "Select rack_allocation.acc_no as 'Access No',title as 'Title',author as 'Author',call_no as 'Call No',rack_allocation.access_date as 'Access Date',rack_no as 'Rack No'from rack_allocation,bookdetails,library where library.lib_code=rack_allocation.lib_code and rack_allocation.acc_no=bookdetails.acc_no " + strDate + " and bookdetails.dept_code='" + ddldept.Text + "'";
                        else if (ddlrackno.Text != "All" && ddlshelfno.Text != "All")
                            Sql1 = "Select rack_allocation.acc_no as 'Access No',title as 'Title',author as 'Author' ,call_no as  'Call No',rack_allocation.access_date as 'Access Date',rack_no  as 'Rack No'from rack_allocation,bookdetails,library where library.lib_code=rack_allocation.lib_code and rack_allocation.acc_no=bookdetails.acc_no" + strDate + " and  rack_no='" + ddlrackno.Text + "' and row_no='" + ddlshelfno.Text + "' and bookdetails.dept_code='" + ddldept.Text + "'and bookdetails.rack_flag=1";
                    }
                    else if (ddldept.Text == "All")
                    {
                        if (ddlrackno.Text == "All" && ddlshelfno.Text != "All")
                            Sql1 = "Select rack_allocation.acc_no as 'Access No',title as 'Title',author as 'Author',call_no as 'Call No',rack_allocation.access_date as 'Access Date',rack_no as 'Rack No' from rack_allocation,bookdetails,library where library.lib_code=rack_allocation.lib_code and rack_allocation.acc_no=bookdetails.acc_no " + strDate + "  and  row_no='" + ddlshelfno.Text + "'";
                        else if (ddlrackno.Text != "All" && ddlshelfno.Text == "All")
                            Sql1 = "Select rack_allocation.acc_no as 'Access No',title as 'Title',author as 'Author',call_no as 'Call No',rack_allocation.access_date as 'Access Date',rack_no as 'Rack No' from rack_allocation,bookdetails,library where library.lib_code=rack_allocation.lib_code and rack_no='" + ddlrackno.Text + "' and rack_allocation.acc_no=bookdetails.acc_no " + strDate;
                        else if (ddlrackno.Text == "All" && ddlshelfno.Text == "All")
                            Sql1 = "Select rack_allocation.acc_no as 'Access No',title as 'Title',author as 'Author',call_no as 'Call No',rack_allocation.access_date as 'Access Date',rack_no as 'Rack No' from rack_allocation,bookdetails,library where library.lib_code=rack_allocation.lib_code and rack_allocation.acc_no=bookdetails.acc_no" + strDate;
                        else if (ddlrackno.Text != "All" && ddlshelfno.Text != "All")
                            Sql1 = "Select rack_allocation.acc_no as 'Access No',title as 'Title',author as 'Author',call_no as 'Call No',rack_allocation.access_date as 'Access Date',rack_no as 'Rack No' from rack_allocation,bookdetails,library where library.lib_code=rack_allocation.lib_code and rack_allocation.acc_no=bookdetails.acc_no  and  rack_no='" + ddlrackno.Text + "'" + strDate + " and row_no='" + ddlshelfno.Text + "'";
                    }
                    ds1 = d2.select_method_wo_parameter(Sql1, "text");
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        transrepo.Columns.Add("SNo", typeof(string));
                        transrepo.Columns.Add("Access No", typeof(string));
                        transrepo.Columns.Add("Title", typeof(string));
                        transrepo.Columns.Add("Author", typeof(string));
                        transrepo.Columns.Add("Call No", typeof(string));
                        transrepo.Columns.Add("Access Date", typeof(string));
                        transrepo.Columns.Add("Rack No", typeof(string));


                        int i = 0;
                        int SNo = 0;
                        for (int r = i; r < ds1.Tables[0].Rows.Count; r++)
                        {
                            SNo++;
                            dr = transrepo.NewRow();
                            dr["SNo"] = Convert.ToString(SNo);
                            dr["Access No"] = ds1.Tables[0].Rows[r]["Access No"].ToString();
                            dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                            dr["Author"] = ds1.Tables[0].Rows[r]["Author"].ToString();
                            dr["Call No"] = ds1.Tables[0].Rows[r]["Call No"].ToString();
                            dr["Access Date"] = ds1.Tables[0].Rows[r]["Access Date"].ToString();
                            dr["Rack No"] = ds1.Tables[0].Rows[r]["Rack No"].ToString();
                            transrepo.Rows.Add(dr);
                        }
                        gridview2.DataSource = transrepo;
                        gridview2.DataBind();

                        gridview2.Visible = true;
                    }
                }
                #endregion

                if (gridview2.Rows.Count > 0)
                {
                    rptprint1.Visible = true;
                }
                else
                {
                    gridview2.Visible = false;
                    rptprint1.Visible = false;
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Found";
                }
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
        return dsdetails;
    }

    # endregion  

    #region Print
    public override void VerifyRenderingInServerForm(Control control)
    { }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(gridview2, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your  Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "TransactionReports";
            pagename = "TransactionReport";
            string ss = Session["usercode"].ToString();
            Printcontrol.loadspreaddetails(gridview2, pagename, degreedetails, 0, ss,"1");
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    protected void getPrintSettings()
    {
        try
        {

            #region Excel print settings
            string usertype = "";
            if (userCode.Trim() != "")
                usertype = " and usercode='" + userCode + "'";
            else if (groupUserCode.Trim() != "")
                usertype = " and group_code='" + groupUserCode + "'";
            string printset = d2.GetFunction("select value from Master_Settings where settings='Excel and Pdf Print Settings' " + usertype + " ");
            if (printset != "")
            {
                if (printset.Contains("E"))
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                }
                if (printset.Contains("P"))
                {
                    btnprintmasterhed.Visible = true;
                }
                if (printset == "0")
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    btnprintmasterhed.Visible = true;
                }
            }
            #endregion
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }

    #endregion

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
            d2.sendErrorMail(ex, userCollegeCode, "TransactionReport");
        }
    }


    #endregion

    public void getLibPrivil()
    {
        try
        {
            Hashtable hsLibcode = new Hashtable();
            string libcodecollection = "";
            string coll_Code = Convert.ToString(ddlCollege.SelectedValue);
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
            BindLibrary(LibCollection);

        }
        catch (Exception ex)
        {
        }
    }
}