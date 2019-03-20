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
using InsproDataAccess;


public partial class LibraryMod_individualstudent : System.Web.UI.Page
{
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    FarPoint.Web.Spread.DoubleCellType doubl = new FarPoint.Web.Spread.DoubleCellType();
    DataTable dtCommon = new DataTable();
    DataSet dsload = new DataSet();
    DataSet dsCommon = new DataSet();
    DataSet dsStaff = new DataSet();
    DataSet dsStaffdel = new DataSet();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string collegecode = string.Empty;
    bool check = false;
    bool DeleteRow = false;
    bool GenCard = false;
    bool DelCard = false;

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
                bindddlCatogery();
                getLibPrivil();
                bindBookType();
                BindBatchYear();
                binddeg();
                bindStaffDepartment();
                BindStafftype();
                bindStaffcategory();
                //ddldegree_SelectedIndexChanged(sender, e);
                binddept();
                rblstaffstudent_Selected(sender, e);
            }
        }
        catch (Exception ex)
        { }
    }

    #region bind method

    public void Bindcollege()
    {
        try
        {
            //ddl_library.Items.Clear();
            dtCommon.Clear();
            ddl_collegename.Enabled = false;

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
                ddl_collegename.DataSource = dtCommon;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
                ddl_collegename.SelectedIndex = 0;
                ddl_collegename.Enabled = true;
            }
        }
        catch (Exception ex)
        { //d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport");
        }
    }

    public void getLibPrivil()
    {
        try
        {
            string libcodecollection = "";
            string coll_Code = Convert.ToString(ddl_collegename.SelectedValue);
            string sql = "";
            string GrpUserVal = "";
            string GrpCode = "";
            string LibCollection = "";
            Hashtable hsLibcode = new Hashtable();
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
        }
        catch (Exception ex)
        {
        }
    }

    protected void bindLibrary(string libcode)
    {
        try
        {
            ddlLibrary.Items.Clear();
            ds.Clear();
            string College = ddl_collegename.SelectedValue.ToString();
            string SelectQ = string.Empty;
            if (!string.IsNullOrEmpty(College))
            {
                string lib_name = "select lib_code,lib_name,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + libcode + " AND college_code=" + College + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = d2.select_method_wo_parameter(lib_name, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlLibrary.DataSource = ds;
                    ddlLibrary.DataTextField = "lib_name";
                    ddlLibrary.DataValueField = "lib_code";
                    ddlLibrary.DataBind();
                    ddlLibrary.Items.Insert(0, "All");
                }
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, College, "Library_Card_Master");
        }
    }

    protected void bindBookType()
    {
        ddlbooktype.Items.Clear();
        ddlbooktype.Items.Add("All");
        ddlbooktype.Items.Add("Book");
        ddlbooktype.Items.Add("Periodicals");
        ddlbooktype.Items.Add("Project Book");
        ddlbooktype.Items.Add("Non-Book Material");
        ddlbooktype.Items.Add("Question Bank");
        ddlbooktype.Items.Add("Back Volume");
        ddlbooktype.Items.Add("Reference Volume");
    }

    protected void BindBatchYear()
    {
        string qry = " select distinct Batch_Year from Registration order by batch_year desc";
        DataTable dtbatchyr = dirAcc.selectDataTable(qry);
        ddlBatch.Items.Clear();
        if (dtbatchyr.Rows.Count > 0)
        {
            ddlBatch.DataSource = dtbatchyr;
            ddlBatch.DataTextField = "Batch_Year";
            ddlBatch.DataValueField = "Batch_Year";
            ddlBatch.DataBind();
        }
    }

    public void binddeg()
    {
        try
        {
            ddldegree.Items.Clear();
            collegecode = ddl_collegename.SelectedItem.Value.ToString();
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + collegecode + "'";
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch { }
    }

    public void binddept()
    {
        try
        {
            ddlbranch.Items.Clear();
            string batch2 = "";
            string degree = "";
            string course_id = ddldegree.SelectedItem.Value;
            string collcode = ddl_collegename.SelectedValue;
            string strquery = "select distinct degree.degree_code,department.dept_name,department.dept_code,degree.dept_priority,len(isnull(degree.dept_priority,1000)) from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + course_id + ") and degree.college_code='" + collcode + "'  and deptprivilages.Degree_code=degree.Degree_code and user_code='" + userCode + "' order by len(isnull(degree.dept_priority,1000)),degree.dept_priority asc ";
            // string strquery = " SELECT Course_Name+'-'+Dept_Name Degree,Degree_Code FROM Degree G,Course C,Department D WHERE G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code AND G.College_Code ='" + collcode + "' and c.course_id in(" + course_id + ")  ORDER BY Course_Name,Dept_Name";
            ds.Clear();
            ds = d2.select_method_wo_parameter(strquery, "Text");

            //batch2 = "";
            //for (int i = 0; i < ddlBatch.Items.Count; i++)
            //{
            //    if (ddlBatch.Items[i].Selected == true)
            //    {
            //        if (batch2 == "")
            //        {
            //            batch2 = Convert.ToString(ddlBatch.Items[i].Text);
            //        }
            //        else
            //        {
            //            batch2 += "','" + Convert.ToString(ddlBatch.Items[i].Text);
            //        }
            //    }
            //}

            //degree = "";
            //for (int i = 0; i < ddldegree.Items.Count; i++)
            //{
            //    if (ddldegree.Items[i].Selected == true)
            //    {
            //        if (degree == "")
            //        {
            //            degree = Convert.ToString(ddldegree.Items[i].Value);
            //        }
            //        else
            //        {
            //            degree += "," + Convert.ToString(ddldegree.Items[i].Value);
            //        }
            //    }
            //}
            //if (batch2 != "" && degree != "")
            //{
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
            //}
        }
        catch { }
    }

    protected void bindddlCatogery()
    {
        DataTable dtexistingcardcatogery = dirAcc.selectDataTable("select TextVal,TextCode from TextValTable where TextCriteria='CDCAT'");
        if (dtexistingcardcatogery.Rows.Count > 0)
        {
            ddl_CardCatogery.DataSource = dtexistingcardcatogery;
            ddl_CardCatogery.DataTextField = "TextVal";
            ddl_CardCatogery.DataValueField = "TextCode";
            ddl_CardCatogery.DataBind();
            ddl_CardCatogery.Items.Insert(0, "All");
        }
        else
        {
            ddl_CardCatogery.Items.Clear();
            ddl_CardCatogery.Items.Insert(0, "All");

        }
    }

    protected void BindStafftype()
    {
        string CollegeCode = ddl_collegename.SelectedValue.ToString();
        ds.Clear();
        string qry = " SELECT DISTINCT StfType FROM StaffTrans T,StaffMaster M WHERE T.Staff_Code = M.Staff_Code AND M.College_Code ='" + CollegeCode + "' AND T.Latestrec = 1 ORDER BY StfType ";
        ds = d2.select_method_wo_parameter(qry, "Text");
        ddlStaffType.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlStaffType.DataSource = ds;
            ddlStaffType.DataTextField = "stftype";
            ddlStaffType.DataValueField = "stftype";
            ddlStaffType.DataBind();
            ddlStaffType.Items.Insert(0, "All");
        }
    }

    protected void bindStaffcategory()
    {
        try
        {
            ddlStaffCat.Items.Clear();
            string CollegeCode = ddl_collegename.SelectedValue.ToString();
            ds.Clear();
            string Query = " select category_code,category_name,CategoryID,college_code from staffcategorizer where college_code='" + CollegeCode + "' order by category_code";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlStaffCat.DataSource = ds;
                ddlStaffCat.DataTextField = "category_name";
                ddlStaffCat.DataValueField = "category_code";
                ddlStaffCat.DataBind();
                ddlStaffCat.Items.Insert(0, "All");

            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void bindStaffDepartment()
    {
        string CollegeCode = ddl_collegename.SelectedValue.ToString();
        string Query = " select distinct dept_code,dept_name from hrdept_master where 1=1  AND college_code = '" + CollegeCode + "' order by dept_name";
        ds = d2.select_method_wo_parameter(Query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlDepartment.DataSource = ds;
            ddlDepartment.DataTextField = "dept_name";
            ddlDepartment.DataValueField = "dept_code";
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, "All");
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

    protected void ddl_collegename_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        getLibPrivil();
        binddeg();
        binddept();
        bindStaffDepartment();
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddept();
    }

    protected void ddlcardtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        string cardType = Convert.ToString(ddlcard.SelectedItem.Text);
        if (cardType == "Book Bank")
        {
            ddlcategory.Enabled = true;
            ddlcategory.Items.Clear();
            ddlcategory.Items.Add("All");
            ddlcategory.Items.Add("SC/ST Category");
        }
        else
        {
            ddlcategory.Enabled = false;
        }

    }

    protected void rblstaffstudent_Selected(object sender, EventArgs e)
    {
        try
        {
            if (rblstaff.SelectedIndex == 0)
            {
                Student.Visible = true;
                staff.Visible = false;
                grdindividual.Visible = false;
            }
            if (rblstaff.SelectedIndex == 1)
            {
                Student.Visible = false;
                staff.Visible = true;
                grdindividual.Visible = false;
            }
        }
        catch
        {

        }



    }

    #region Card Category Popup

    protected void btnadd_Click(object sender, EventArgs e)
    {
        txt_CardCatogery.Text = string.Empty;
        NewCardCatogery.Visible = true;
        DivCard.Visible = true;
    }

    protected void btndel_Click(object sender, EventArgs e)
    {
        if (ddl_CardCatogery.Items.Count > 0 && ddl_CardCatogery.SelectedValue != "0")
        {
            string categtodel = ddl_CardCatogery.SelectedValue;
            string delqry = "delete from TextValTable where  TextCode='" + categtodel + "'";
            dirAcc.deleteData(delqry);
            bindddlCatogery();
        }
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsgNEW.Text = string.Empty;
            divPopAlertNEW.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "individualstudent");
        }
    }

    protected void btn_NewCardCatogerySave_Click(object sender, EventArgs e)
    {
        string catogery = txt_CardCatogery.Text.Trim();
        if (!string.IsNullOrEmpty(catogery))
        {
            DataTable dtexistingcardcatogery = dirAcc.selectDataTable("select TextVal from TextValTable where TextCriteria='CDCAT'");
            if (dtexistingcardcatogery.Rows.Count > 0)
            {
                List<string> lstexistCat = dtexistingcardcatogery.AsEnumerable().Select(r => r.Field<string>("TextVal")).ToList();
                if (lstexistCat.Contains(catogery))
                {
                    lblErrNewCardCatoger.Visible = true;
                    lblErrNewCardCatoger.Text = "Catogery Already Exists";
                    return;
                }
            }
            string insertqry = "insert into TextValTable (TextVal,TextCriteria,college_code) values('" + catogery + "','CDCAT','" + Convert.ToString(Session["collegecode"]) + "')";
            if (dirAcc.insertData(insertqry) > 0)
            {
                bindddlCatogery();
                NewCardCatogery.Visible = false;
                DivCard.Visible = false;
                txt_CardCatogery.Text = string.Empty;
            }
        }
    }

    protected void btn_NewCardcatogeryExit_Click(object sender, EventArgs e)
    {
        NewCardCatogery.Visible = false;
        DivCard.Visible = false;
        txt_CardCatogery.Text = string.Empty;
    }

    #endregion

    public void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            string selectQry = string.Empty;
            string college = Convert.ToString(ddl_collegename.SelectedItem.Value);
            string library = Convert.ToString(ddlLibrary.SelectedValue);
            string booktype = Convert.ToString(ddlbooktype.SelectedValue);
            string cardType = Convert.ToString(ddlcard.SelectedItem.Text);
            string CardCategory = Convert.ToString(ddl_CardCatogery.SelectedValue);
            string StrBookType = string.Empty;
            string StrTransLibCode = string.Empty;
            string code = string.Empty;
            string code_Desc = string.Empty;
            string StrRenewDays = string.Empty;
            string StrCardCount = string.Empty;
            string GetCourseName = "";
            string GetDeptName = "";
            if (booktype == "Book")
                StrBookType = "BOK";
            if (booktype == "Periodicals")
                StrBookType = "PER";
            if (booktype == "Project Book")
                StrBookType = "PRO";
            if (booktype == "Non-Book Material")
                StrBookType = "NBM";
            if (booktype == "Question Bank")
                StrBookType = "QBA";
            if (booktype == "Back Volume")
                StrBookType = "BVO";
            if (booktype == "Reference Volume")
                StrBookType = "REF";
            if (booktype == "All")
                StrBookType = "All";

            #region Student

            if (rblstaff.SelectedIndex == 0)
            {
                string batchYr = Convert.ToString(ddlBatch.SelectedValue);
                string degCode = Convert.ToString(ddlbranch.SelectedItem.Value);
                string category = Convert.ToString(ddlcategory.SelectedValue);
                selectQry = " Select distinct degree.course_id,degree.dept_code,degree.degree_code from degree,registration where degree.college_code='" + college + "'and degree.degree_code=registration.degree_code and registration.batch_year='" + batchYr + "'and registration.degree_code='" + degCode + "' ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectQry, "Text");
                int sno = 0;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dtIndividualCardGen = new DataTable();
                    dtIndividualCardGen.Columns.Add("roll_no");
                    dtIndividualCardGen.Columns.Add("name");
                    dtIndividualCardGen.Columns.Add("General");
                    dtIndividualCardGen.Columns.Add("individual");
                    dtIndividualCardGen.Columns.Add("merit");
                    dtIndividualCardGen.Columns.Add("book");
                    dtIndividualCardGen.Columns.Add("total");
                    dtIndividualCardGen.Columns.Add("AddOrDel");
                    dtIndividualCardGen.Columns.Add("RenewDays");
                    dtIndividualCardGen.Columns.Add("DueDays");
                    dtIndividualCardGen.Columns.Add("FineType");
                    dtIndividualCardGen.Columns.Add("Fine");
                    dtIndividualCardGen.Columns.Add("FineOver");

                    #region query

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string C_id = Convert.ToString(ds.Tables[0].Rows[i]["course_id"]);
                        string dept_Code = Convert.ToString(ds.Tables[0].Rows[i]["dept_code"]);
                        selectQry = "select Course_Name from course where Course_Id='" + C_id + "'";
                        GetCourseName = d2.GetFunction(selectQry);
                        selectQry = "select Dept_Name from department where Dept_code='" + dept_Code + "'";
                        GetDeptName = d2.GetFunction(selectQry);
                        code = Convert.ToString(ds.Tables[0].Rows[i]["course_id"]) + "~" + Convert.ToString(ds.Tables[0].Rows[i]["dept_code"]);
                        code_Desc = GetCourseName + "-" + GetDeptName;
                    }
                    if (cardType != "Book Bank")
                    {
                        selectQry = "select * from lib_master where code='" + code + "' and batch_year='" + batchYr + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND Is_Staff = 0 ";
                        if (library != "All")
                        {
                            selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                        }
                        else if (library == "All")
                        {
                            selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='All'";
                        }
                        if (booktype != "All")
                        {
                            selectQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                        }
                        else if (booktype == "All")
                        {
                            selectQry += " AND ISNULL(Book_Type,'All') ='All'";
                        }
                        if (CardCategory != "All")
                        {
                            selectQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                        }
                        else
                        {
                            selectQry += " AND ISNULL(CardCat,'All') ='All'";
                        }
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectQry, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            selectQry = "SELECT R.Roll_No,R.Lib_ID,R.Stud_Name FROM Registration R WHERE DelFlag = 0 AND Exam_Flag = 'OK' AND Batch_Year='" + batchYr + "' AND Degree_Code='" + degCode + "'  ORDER BY LEN(Roll_No),Roll_No ";
                            dsload.Clear();
                            dsload = d2.select_method_wo_parameter(selectQry, "Text");
                            if (dsload.Tables[0].Rows.Count > 0)
                            {
                                grdindividual.Visible = true;
                                for (int i = 0; i < dsload.Tables[0].Rows.Count; i++)
                                {
                                    DataRow drow = dtIndividualCardGen.NewRow();
                                    double intTotCard = 0;
                                    double CardCount = 0;
                                    string rollNo = Convert.ToString(dsload.Tables[0].Rows[i]["Roll_No"]);
                                    string Lib_ID = Convert.ToString(dsload.Tables[0].Rows[i]["Lib_ID"]);
                                    selectQry = "SELECT ISNULL(Renew_Days,'') FROM TokenDetails WHERE (Roll_No ='" + rollNo + "' OR Roll_No ='" + Lib_ID + "' ) AND Is_Staff = 0 ";
                                    if (library != "All")
                                    {
                                        selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                                    }
                                    else if (library == "All")
                                    {
                                        selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='All'";
                                    }
                                    if (booktype != "All")
                                    {
                                        selectQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                    }
                                    else if (booktype == "All")
                                    {
                                        selectQry += " AND ISNULL(Book_Type,'All') ='All'";
                                    }
                                    if (CardCategory != "All")
                                    {
                                        selectQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                    }
                                    else
                                    {
                                        selectQry += " AND ISNULL(CardCat,'All') ='All'";
                                    }
                                    StrRenewDays = d2.GetFunction(selectQry);
                                    drow["roll_no"] = Convert.ToString(dsload.Tables[0].Rows[i]["roll_no"]);
                                    drow["name"] = Convert.ToString(dsload.Tables[0].Rows[i]["stud_name"]);
                                    drow["RenewDays"] = Convert.ToString(StrRenewDays);
                                    drow["DueDays"] = Convert.ToString(ds.Tables[0].Rows[0]["no_of_days"]);
                                    drow["FineType"] = Convert.ToString(ds.Tables[0].Rows[0]["FineType"]);
                                    drow["Fine"] = Convert.ToString(ds.Tables[0].Rows[0]["fine"]);
                                    drow["FineOver"] = Convert.ToString(ds.Tables[0].Rows[0]["overnightfine"]);

                                    //Available Card(general)
                                    selectQry = "SELECT ISNULL(COUNT(*),0) TotTok FROM TokenDetails T WHERE (T.Roll_No ='" + rollNo + "' OR T.Roll_No ='" + Lib_ID + "' ) AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND ISNULL(IndCategory,0) = 0 AND Is_Staff = 0 ";
                                    if (library != "All")
                                    {
                                        selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                                    }
                                    else if (library == "All")
                                    {
                                        selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='All'";
                                    }
                                    if (booktype != "All")
                                    {
                                        selectQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                    }
                                    else if (booktype == "All")
                                    {
                                        selectQry += " AND ISNULL(Book_Type,'All') ='All'";
                                    }
                                    if (CardCategory != "All")
                                    {
                                        selectQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                    }
                                    else
                                    {
                                        selectQry += " AND ISNULL(CardCat,'All') ='All'";
                                    }
                                    StrCardCount = d2.GetFunction(selectQry);
                                    double.TryParse(StrCardCount, out CardCount);
                                    intTotCard = intTotCard + CardCount;

                                    drow["General"] = Convert.ToString(StrCardCount);

                                    //Available Card(individual)

                                    selectQry = "SELECT ISNULL(COUNT(*),0) TotTok FROM TokenDetails T WHERE (T.Roll_No ='" + rollNo + "' OR T.Roll_No ='" + Lib_ID + "' ) AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND ISNULL(IndCategory,0) = 1 AND Is_Staff = 0 ";
                                    if (library != "All")
                                    {
                                        selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                                    }
                                    else if (library == "All")
                                    {
                                        selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='All'";
                                    }
                                    if (booktype != "All")
                                    {
                                        selectQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                    }
                                    else if (booktype == "All")
                                    {
                                        selectQry += " AND ISNULL(Book_Type,'All') ='All'";
                                    }
                                    if (CardCategory != "All")
                                    {
                                        selectQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                    }
                                    else
                                    {
                                        selectQry += " AND ISNULL(CardCat,'All') ='All'";
                                    }
                                    StrCardCount = d2.GetFunction(selectQry);
                                    double.TryParse(StrCardCount, out CardCount);
                                    intTotCard = intTotCard + CardCount;

                                    drow["individual"] = Convert.ToString(StrCardCount);

                                    //Available Card(Merit)
                                    selectQry = "SELECT ISNULL(COUNT(*),0) TotTok FROM TokenDetails T WHERE (T.Roll_No ='" + rollNo + "' OR T.Roll_No ='" + Lib_ID + "' ) AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND ISNULL(IndCategory,0) = 2 AND Is_Staff = 0 ";
                                    if (library != "All")
                                    {
                                        selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                                    }
                                    else if (library == "All")
                                    {
                                        selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='All'";
                                    }
                                    if (booktype != "All")
                                    {
                                        selectQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                    }
                                    else if (booktype == "All")
                                    {
                                        selectQry += " AND ISNULL(Book_Type,'All') ='All'";
                                    }
                                    if (CardCategory != "All")
                                    {
                                        selectQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                    }
                                    else
                                    {
                                        selectQry += " AND ISNULL(CardCat,'All') ='All'";
                                    }
                                    StrCardCount = d2.GetFunction(selectQry);
                                    double.TryParse(StrCardCount, out CardCount);
                                    intTotCard = intTotCard + CardCount;
                                    drow["merit"] = Convert.ToString(StrCardCount);

                                    //Available Card(BookBank)
                                    selectQry = "SELECT ISNULL(COUNT(*),0) TotTok FROM TokenDetails T WHERE (T.Roll_No ='" + rollNo + "' OR T.Roll_No ='" + Lib_ID + "' ) AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = '" + category + "' AND Is_Staff = 0 ";
                                    if (library != "All")
                                    {
                                        selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                                    }
                                    else if (library == "All")
                                    {
                                        selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='All'";
                                    }
                                    if (booktype != "All")
                                    {
                                        selectQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                    }
                                    else if (booktype == "All")
                                    {
                                        selectQry += " AND ISNULL(Book_Type,'All') ='All'";
                                    }
                                    if (CardCategory != "All")
                                    {
                                        selectQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                    }
                                    else
                                    {
                                        selectQry += " AND ISNULL(CardCat,'All') ='All'";
                                    }
                                    StrCardCount = d2.GetFunction(selectQry);
                                    double.TryParse(StrCardCount, out CardCount);
                                    intTotCard = intTotCard + CardCount;

                                    drow["book"] = Convert.ToString(StrCardCount);
                                    drow["total"] = Convert.ToString(intTotCard);
                                    dtIndividualCardGen.Rows.Add(drow);
                                }
                                divspread.Visible = true;
                                grdindividual.DataSource = dtIndividualCardGen;
                                grdindividual.DataBind();
                                grdindividual.Visible = true;
                            }
                            else
                            {
                                imgdiv2.Visible = true;
                                lbl_alert.Text = "Cards are not generated";
                            }
                        }
                        else
                        {
                            grdindividual.Visible = false;
                            imgdiv2.Visible = true;
                            lbl_alert.Text = "No record found";
                            return;
                        }
                    }
                    else
                    {
                        selectQry = "SELECT R.Roll_No,R.Lib_ID,R.Stud_Name FROM Registration R WHERE DelFlag = 0 AND Exam_Flag = 'OK' AND Batch_Year='" + batchYr + "' AND Degree_Code='" + degCode + "'  ORDER BY LEN(Roll_No),Roll_No ";
                        ds.Clear();
                        dsload = d2.select_method_wo_parameter(selectQry, "Text");

                        if (dsload.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsload.Tables[0].Rows.Count; i++)
                            {
                                DataRow drow = dtIndividualCardGen.NewRow();
                                double intTotCard = 0;
                                double CardCount = 0;
                                string rollNo = Convert.ToString(dsload.Tables[0].Rows[i]["Roll_No"]);
                                string Lib_ID = Convert.ToString(dsload.Tables[0].Rows[i]["Lib_ID"]);

                                selectQry = "SELECT ISNULL(Renew_Days,'') FROM TokenDetails WHERE (Roll_No ='" + rollNo + "' OR Roll_No ='" + Lib_ID + "' ) AND Is_Staff = 0 ";
                                if (library != "All")
                                {
                                    selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                                }
                                else if (library == "All")
                                {
                                    selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='All'";
                                }
                                if (booktype != "All")
                                {
                                    selectQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                }
                                else if (booktype == "All")
                                {
                                    selectQry += " AND ISNULL(Book_Type,'All') ='All'";
                                }
                                if (CardCategory != "All")
                                {
                                    selectQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                }
                                else
                                {
                                    selectQry += " AND ISNULL(CardCat,'All') ='All'";
                                }
                                StrRenewDays = d2.GetFunction(selectQry);

                                drow["roll_no"] = Convert.ToString(dsload.Tables[0].Rows[i]["roll_no"]);
                                drow["name"] = Convert.ToString(dsload.Tables[0].Rows[i]["stud_name"]);
                                drow["RenewDays"] = Convert.ToString(StrRenewDays);

                                selectQry = "select * from lib_master where code='" + rollNo + "' and batch_year='" + batchYr + "' AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = '" + category + "' AND Is_Staff = 0 ";

                                if (library != "All")
                                {
                                    selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                                }
                                else if (library == "All")
                                {
                                    selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='All'";
                                }
                                if (booktype != "All")
                                {
                                    selectQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                }
                                else if (booktype == "All")
                                {
                                    selectQry += " AND ISNULL(Book_Type,'All') ='All'";
                                }
                                if (CardCategory != "All")
                                {
                                    selectQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                }
                                else
                                {
                                    selectQry += " AND ISNULL(CardCat,'All') ='All'";
                                }
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(selectQry, "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    drow["DueDays"] = Convert.ToString(ds.Tables[0].Rows[0]["no_of_days"]);
                                    drow["Fine"] = Convert.ToString(ds.Tables[0].Rows[0]["fine"]);
                                    drow["FineOver"] = Convert.ToString(ds.Tables[0].Rows[0]["overnightfine"]);
                                }
                                else
                                {
                                    selectQry = "select * from lib_master where code='" + code + "' and batch_year='" + batchYr + "' AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = '" + category + "' AND Is_Staff = 0 ";

                                    if (library != "All")
                                    {
                                        selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                                    }
                                    else if (library == "All")
                                    {
                                        selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='All'";
                                    }
                                    if (booktype != "All")
                                    {
                                        selectQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                    }
                                    else if (booktype == "All")
                                    {
                                        selectQry += " AND ISNULL(Book_Type,'All') ='All'";
                                    }
                                    if (CardCategory != "All")
                                    {
                                        selectQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                    }
                                    else
                                    {
                                        selectQry += " AND ISNULL(CardCat,'All') ='All'";
                                    }
                                    ds.Clear();
                                    ds = d2.select_method_wo_parameter(selectQry, "Text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        drow["DueDays"] = Convert.ToString(ds.Tables[0].Rows[0]["no_of_days"]);
                                        drow["Fine"] = Convert.ToString(ds.Tables[0].Rows[0]["fine"]);
                                        drow["FineOver"] = Convert.ToString(ds.Tables[0].Rows[0]["overnightfine"]);
                                    }
                                }
                                //Available cards(General)
                                selectQry = "SELECT ISNULL(COUNT(*),0) TotTok FROM TokenDetails T WHERE (T.Roll_No ='" + rollNo + "' OR T.Roll_No ='" + Lib_ID + "' ) AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND ISNULL(IndCategory,0) = 0 AND Is_Staff = 0 ";
                                if (library != "All")
                                {
                                    selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                                }
                                else if (library == "All")
                                {
                                    selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='All'";
                                }
                                if (booktype != "All")
                                {
                                    selectQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                }
                                else if (booktype == "All")
                                {
                                    selectQry += " AND ISNULL(Book_Type,'All') ='All'";
                                }
                                if (CardCategory != "All")
                                {
                                    selectQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                }
                                else
                                {
                                    selectQry += " AND ISNULL(CardCat,'All') ='All'";
                                }
                                StrCardCount = d2.GetFunction(selectQry);
                                double.TryParse(StrCardCount, out CardCount);

                                drow["General"] = Convert.ToString(StrCardCount);
                                intTotCard = intTotCard + CardCount;

                                //Available cards(Individual)
                                selectQry = "SELECT ISNULL(COUNT(*),0) TotTok FROM TokenDetails T WHERE (T.Roll_No ='" + rollNo + "' OR T.Roll_No ='" + Lib_ID + "' ) AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND ISNULL(IndCategory,0) = 1 AND Is_Staff = 0 ";
                                if (library != "All")
                                {
                                    selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                                }
                                else if (library == "All")
                                {
                                    selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='All'";
                                }
                                if (booktype != "All")
                                {
                                    selectQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                }
                                else if (booktype == "All")
                                {
                                    selectQry += " AND ISNULL(Book_Type,'All') ='All'";
                                }
                                if (CardCategory != "All")
                                {
                                    selectQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                }
                                else
                                {
                                    selectQry += " AND ISNULL(CardCat,'All') ='All'";
                                }
                                StrCardCount = d2.GetFunction(selectQry);
                                double.TryParse(StrCardCount, out CardCount);

                                drow["individual"] = Convert.ToString(StrCardCount);
                                intTotCard = intTotCard + CardCount;

                                //Available cards(Merit)
                                selectQry = "SELECT ISNULL(COUNT(*),0) TotTok FROM TokenDetails T WHERE (T.Roll_No ='" + rollNo + "' OR T.Roll_No ='" + Lib_ID + "' ) AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND ISNULL(IndCategory,0) = 2 AND Is_Staff = 0 ";
                                if (library != "All")
                                {
                                    selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                                }
                                else if (library == "All")
                                {
                                    selectQry = selectQry + "AND ISNULL(TransLibCode,'All') ='All'";
                                }
                                if (booktype != "All")
                                {
                                    selectQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                }
                                else if (booktype == "All")
                                {
                                    selectQry += " AND ISNULL(Book_Type,'All') ='All'";
                                }
                                if (CardCategory != "All")
                                {
                                    selectQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                }
                                else
                                {
                                    selectQry += " AND ISNULL(CardCat,'All') ='All'";
                                }
                                StrCardCount = d2.GetFunction(selectQry);
                                double.TryParse(StrCardCount, out CardCount);

                                drow["merit"] = Convert.ToString(StrCardCount);
                                intTotCard = intTotCard + CardCount;

                                //Available cards(BookBank)
                                selectQry = "SELECT ISNULL(COUNT(*),0) TotTok FROM TokenDetails T WHERE (T.Roll_No ='" + rollNo + "' OR T.Roll_No ='" + Lib_ID + "' ) AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = '" + category + "' AND Is_Staff = 0 ";
                                if (library != "All")
                                {
                                    selectQry += "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                                }
                                else if (library == "All")
                                {
                                    selectQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                }
                                if (booktype != "All")
                                {
                                    selectQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                }
                                else if (booktype == "All")
                                {
                                    selectQry += " AND ISNULL(Book_Type,'All') ='All'";
                                }
                                if (CardCategory != "All")
                                {
                                    selectQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                }
                                else
                                {
                                    selectQry += " AND ISNULL(CardCat,'All') ='All'";
                                }
                                StrCardCount = d2.GetFunction(selectQry);
                                double.TryParse(StrCardCount, out CardCount);
                                drow["book"] = Convert.ToString(StrCardCount);
                                intTotCard = intTotCard + CardCount;
                                drow["total"] = Convert.ToString(intTotCard);
                                dtIndividualCardGen.Rows.Add(drow);
                            }
                            grdindividual.DataSource = dtIndividualCardGen;
                            grdindividual.DataBind();
                            grdindividual.Visible = true;
                            divspread.Visible = true;
                        }
                    }
                    #endregion
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "No Record found";
                    divspread.Visible = false;
                }
            }
            #endregion

            #region Staff

            if (rblstaff.SelectedIndex == 1)
            {
                string department = Convert.ToString(ddlDepartment.SelectedItem.Value);
                string staffType = Convert.ToString(ddlStaffType.SelectedValue);
                string staffCat = Convert.ToString(ddlStaffCat.SelectedValue);
                string cardcat = Convert.ToString(ddlcategory.SelectedValue);
                selectQry = " SELECT DISTINCT M.Staff_Code,M.Lib_ID,M.Staff_Name FROM StaffMaster M,StaffTrans T,StaffCategorizer C WHERE M.Staff_Code = T.Staff_Code AND T.Category_Code = C.Category_Code AND M.College_Code = C.College_Code AND T.Latestrec = 1 AND Resign = 0 AND Settled = 0 AND M.College_Code =" + college + " ";

                if (department != "All")
                {
                    selectQry += "AND T.Dept_Code ='" + department + "'";
                }

                if (staffCat != "All")
                {
                    selectQry += "AND C.category_code ='" + staffCat + "' ";
                }
                if (staffType != "All")
                {
                    selectQry += "AND T.StfType ='" + staffType + "' ";
                }

                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selectQry, "Text");

                if (dsload.Tables[0].Rows.Count > 0)
                {
                    DataTable dtIndividualCardGen = new DataTable();
                    dtIndividualCardGen.Columns.Add("roll_no");
                    dtIndividualCardGen.Columns.Add("name");
                    dtIndividualCardGen.Columns.Add("General");
                    dtIndividualCardGen.Columns.Add("individual");
                    dtIndividualCardGen.Columns.Add("merit");
                    dtIndividualCardGen.Columns.Add("book");
                    dtIndividualCardGen.Columns.Add("total");
                    dtIndividualCardGen.Columns.Add("AddOrDel");
                    dtIndividualCardGen.Columns.Add("RenewDays");
                    dtIndividualCardGen.Columns.Add("DueDays");
                    dtIndividualCardGen.Columns.Add("FineType");
                    dtIndividualCardGen.Columns.Add("Fine");
                    dtIndividualCardGen.Columns.Add("FineOver");

                    #region query
                    int sno = 0;
                    double cardCnt = 0;

                    for (int i = 0; i < dsload.Tables[0].Rows.Count; i++)
                    {
                        DataRow drow = dtIndividualCardGen.NewRow();
                        double intTotCard = 0;
                        string staffCode = Convert.ToString(dsload.Tables[0].Rows[i]["Staff_Code"]);
                        string libId = Convert.ToString(dsload.Tables[0].Rows[i]["Lib_ID"]);
                        selectQry = "SELECT ISNULL(Renew_Days,'') FROM TokenDetails WHERE Is_Staff = 1 AND (Roll_No ='" + staffCode + "' OR Roll_No ='" + libId + "' ) AND Is_Staff = 1 ";
                        if (library != "All")
                        {
                            selectQry += "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                        }
                        else if (library == "All")
                        {
                            selectQry += "AND ISNULL(TransLibCode,'All') ='All'";
                        }
                        if (booktype != "All")
                        {
                            selectQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                        }
                        else if (booktype == "All")
                        {
                            selectQry += " AND ISNULL(Book_Type,'All') ='All'";
                        }
                        if (CardCategory != "All")
                        {
                            selectQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                        }
                        else
                        {
                            selectQry += " AND ISNULL(CardCat,'All') ='All'";
                        }
                        StrRenewDays = d2.GetFunction(selectQry);

                        drow["roll_no"] = Convert.ToString(dsload.Tables[0].Rows[i]["Staff_Code"]);
                        drow["name"] = Convert.ToString(dsload.Tables[0].Rows[i]["Staff_Name"]);
                        drow["RenewDays"] = Convert.ToString(StrRenewDays);

                        selectQry = "select * from lib_master where is_staff = 1 AND code='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' ";
                        if (library != "All")
                        {
                            selectQry += "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                        }
                        else if (library == "All")
                        {
                            selectQry += "AND ISNULL(TransLibCode,'All') ='All'";
                        }
                        if (booktype != "All")
                        {
                            selectQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                        }
                        else if (booktype == "All")
                        {
                            selectQry += " AND ISNULL(Book_Type,'All') ='All'";
                        }
                        if (CardCategory != "All")
                        {
                            selectQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                        }
                        else
                        {
                            selectQry += " AND ISNULL(CardCat,'All') ='All'";
                        }
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectQry, "text");

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            drow["DueDays"] = Convert.ToString(ds.Tables[0].Rows[0]["no_of_days"]);
                            drow["Fine"] = Convert.ToString(ds.Tables[0].Rows[0]["fine"]);
                            drow["FineType"] = Convert.ToString(ds.Tables[0].Rows[0]["FineType"]);
                            drow["FineOver"] = Convert.ToString(ds.Tables[0].Rows[0]["overnightfine"]);
                        }

                        //Available card(General)
                        selectQry = "SELECT ISNULL(COUNT(*),0) TotTok FROM TokenDetails T WHERE Is_Staff = 1 AND (T.Roll_No ='" + staffCode + "' OR T.Roll_No ='" + libId + "' ) AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND ISNULL(IndCategory,0) = 0  ";
                        if (library != "All")
                        {
                            selectQry += "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                        }
                        else if (library == "All")
                        {
                            selectQry += "AND ISNULL(TransLibCode,'All') ='All'";
                        }
                        if (booktype != "All")
                        {
                            selectQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                        }
                        else if (booktype == "All")
                        {
                            selectQry += " AND ISNULL(Book_Type,'All') ='All'";
                        }
                        if (CardCategory != "All")
                        {
                            selectQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                        }
                        else
                        {
                            selectQry += " AND ISNULL(CardCat,'All') ='All'";
                        }
                        StrCardCount = d2.GetFunction(selectQry);
                        double.TryParse(StrCardCount, out cardCnt);

                        drow["General"] = Convert.ToString(StrCardCount);
                        intTotCard = intTotCard + cardCnt;

                        //Available card(Individual)
                        selectQry = "SELECT ISNULL(COUNT(*),0) TotTok FROM TokenDetails T WHERE Is_Staff = 1 AND (T.Roll_No ='" + staffCode + "' OR T.Roll_No ='" + libId + "' ) AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND ISNULL(IndCategory,0) = 1 ";
                        if (library != "All")
                        {
                            selectQry += "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                        }
                        else if (library == "All")
                        {
                            selectQry += "AND ISNULL(TransLibCode,'All') ='All'";
                        }
                        if (booktype != "All")
                        {
                            selectQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                        }
                        else if (booktype == "All")
                        {
                            selectQry += " AND ISNULL(Book_Type,'All') ='All' ";
                        }
                        if (CardCategory != "All")
                        {
                            selectQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "' ";
                        }
                        else
                        {
                            selectQry += " AND ISNULL(CardCat,'All') ='All'";
                        }
                        StrCardCount = d2.GetFunction(selectQry);
                        double.TryParse(StrCardCount, out cardCnt);

                        drow["individual"] = Convert.ToString(StrCardCount);
                        intTotCard = intTotCard + cardCnt;

                        //Available card(Merit)
                        selectQry = "SELECT ISNULL(COUNT(*),0) TotTok FROM TokenDetails T WHERE Is_Staff = 1 AND (T.Roll_No ='" + staffCode + "' OR T.Roll_No ='" + libId + "' ) AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND ISNULL(IndCategory,0) = 2 ";
                        if (library != "All")
                        {
                            selectQry += "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                        }
                        else if (library == "All")
                        {
                            selectQry += "AND ISNULL(TransLibCode,'All') ='All'";
                        }
                        if (booktype != "All")
                        {
                            selectQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                        }
                        else if (booktype == "All")
                        {
                            selectQry += " AND ISNULL(Book_Type,'All') ='All' ";
                        }
                        if (CardCategory != "All")
                        {
                            selectQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "' ";
                        }
                        else
                        {
                            selectQry += " AND ISNULL(CardCat,'All') ='All'";
                        }
                        StrCardCount = d2.GetFunction(selectQry);
                        double.TryParse(StrCardCount, out cardCnt);

                        drow["merit"] = Convert.ToString(StrCardCount);
                        intTotCard = intTotCard + cardCnt;

                        //Available card(BookBank)
                        selectQry = "SELECT ISNULL(COUNT(*),0) TotTok FROM TokenDetails T WHERE Is_Staff = 1 AND (T.Roll_No ='" + staffCode + "' OR T.Roll_No ='" + libId + "' ) AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = '" + cardcat + "' ";
                        if (library != "All")
                        {
                            selectQry += "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                        }
                        else if (library == "All")
                        {
                            selectQry += "AND ISNULL(TransLibCode,'All') ='All'";
                        }
                        if (booktype != "All")
                        {
                            selectQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "' ";
                        }
                        else if (booktype == "All")
                        {
                            selectQry += " AND ISNULL(Book_Type,'All') ='All' ";
                        }
                        if (CardCategory != "All")
                        {
                            selectQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "' ";
                        }
                        else
                        {
                            selectQry += " AND ISNULL(CardCat,'All') ='All'";
                        }
                        StrCardCount = d2.GetFunction(selectQry);
                        double.TryParse(StrCardCount, out cardCnt);

                        drow["book"] = Convert.ToString(StrCardCount);
                        intTotCard = intTotCard + cardCnt;
                        drow["total"] = Convert.ToString(intTotCard);
                        dtIndividualCardGen.Rows.Add(drow);
                    }
                    #endregion

                    grdindividual.DataSource = dtIndividualCardGen;
                    grdindividual.DataBind();
                    grdindividual.Visible = true;
                    divspread.Visible = true;
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "No Record found";
                }
            }
            #endregion
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, userCollegeCode, "individualstudent");
        }
    }

    //protected void spreadindividual_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{
    //    try
    //    {
    //        string actrow = spreadindividual.Sheets[0].ActiveRow.ToString();
    //        string actcol = spreadindividual.Sheets[0].ActiveColumn.ToString();
    //        if (actrow.Trim() == "0" && actcol.Trim() == "1")
    //        {
    //            if (spreadindividual.Sheets[0].RowCount > 0)
    //            {
    //                int checkval = Convert.ToInt32(spreadindividual.Sheets[0].Cells[0, 1].Value);
    //                if (checkval == 0)
    //                {
    //                    for (int i = 1; i < spreadindividual.Sheets[0].RowCount; i++)
    //                    {
    //                        spreadindividual.Sheets[0].Cells[i, 1].Value = 1;
    //                    }
    //                }
    //                if (checkval == 1)
    //                {
    //                    for (int i = 1; i < spreadindividual.Sheets[0].RowCount; i++)
    //                    {
    //                        spreadindividual.Sheets[0].Cells[i, 1].Value = 0;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        //d2.sendErrorMail(ex, collegecode, "Individual_StudentFeeStatus"); 
    //    }
    //}

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    protected void btngenerate_Click(object sender, EventArgs e)
    {
        try
        {
            string NoOfcards = "";
            if (rblstaff.SelectedIndex == 0)
            {
                int selectedcount = 0;
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    if (chk.Checked == true)
                    {
                        selectedcount++;
                        GenCard = true;
                        int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                        TextBox txtAddOrDel = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_AddOrDel");
                        if (txtAddOrDel.Text.Trim() == "")
                        {
                            GenCard = false;
                            imgdiv2.Visible = true;
                            lbl_alert.Text = "Fill the Number of Cards";
                            break;
                        }
                    }
                }
                if (GenCard == true)
                {
                    Surediv.Visible = true;
                    Div3.Visible = true;
                    LblGen.Visible = true;
                }
                if (selectedcount == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Select any Student to Generate Card";
                }
            }
            if (rblstaff.SelectedIndex == 1)
            {
                int selectedcount = 0;
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    if (chk.Checked == true)
                    {
                        selectedcount++;
                        GenCard = true;
                        int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                        TextBox txtAddOrDel = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_AddOrDel");
                        //NoOfcards = Convert.ToString(grdindividual.Rows[RowCnt].Cells[9].Text);
                        if (txtAddOrDel.Text.Trim() == "")
                        {
                            GenCard = false;
                            imgdiv2.Visible = true;
                            lbl_alert.Text = "Fill the Number of Cards";
                            break;
                        }
                    }
                }
                if (GenCard == true)
                {
                    Surediv.Visible = true;
                    Div3.Visible = true;
                    LblGen.Visible = true;
                }
                if (selectedcount == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Select any Staff to Generate Card";
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "individualstudent");
        }
    }

    protected void btnSure_yes_Click(object sender, EventArgs e)
    {
        if (rblstaff.SelectedIndex == 0)
        {
            StudentCardGen(sender, e);
        }
        if (rblstaff.SelectedIndex == 1)
        {
            StaffCardGen(sender, e);
        }
    }

    protected void StaffCardGen(object sender, EventArgs e)
    {
        try
        {
            string booktype = Convert.ToString(ddlbooktype.SelectedValue);
            string library = Convert.ToString(ddlLibrary.SelectedValue);
            string CardCategory = Convert.ToString(ddl_CardCatogery.SelectedValue);
            string college_Code = Convert.ToString(ddl_collegename.SelectedItem.Value);
            string StaffCat = Convert.ToString(ddlcard.SelectedValue);
            string StrBookType = string.Empty;
            string StrTransLibCode = string.Empty;
            string StrCardCat = string.Empty;
            string FineType = string.Empty;
            string StrDepDesc = string.Empty;
            string selQry = string.Empty;
            string MaxCard = string.Empty;
            string StrTokNo = string.Empty;
            string insertQry = string.Empty;
            string UpdateQry = string.Empty;
            int insert = 0;
            int update = 0;
            int intGenCout = 0;
            int StrMaxCard = 0;
            double NoCards = 0;
            double NoOfCards = 0;
            double Fine = 0;
            double OvernightFine = 0;
            double DueDays = 0;
            string staffCode = "";
            string staffName = "";

            if (booktype == "Book")
                StrBookType = "BOK";
            if (booktype == "Periodicals")
                StrBookType = "PER";
            if (booktype == "Project Book")
                StrBookType = "PRO";
            if (booktype == "Non-Book Material")
                StrBookType = "NBM";
            if (booktype == "Question Bank")
                StrBookType = "QBA";
            if (booktype == "Back Volume")
                StrBookType = "BVO";
            if (booktype == "Reference Volume")
                StrBookType = "REF";
            if (booktype == "All")
                StrBookType = "All";
            if (library != "All")
                StrTransLibCode = library;
            else
                StrTransLibCode = "All";

            if (CardCategory != "All")
                StrCardCat = CardCategory;
            else
                StrCardCat = "All";

            foreach (GridViewRow gvrow in grdindividual.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                if (chk.Checked == true)
                {
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    Label staff_Code = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_roll_no");
                    staffCode = staff_Code.Text.Trim();
                    Label staff_Name = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_Name");
                    staffName = staff_Name.Text.Trim();
                    TextBox NoOf_Cards = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_AddOrDel");
                    if (NoOf_Cards.Text.Trim() != "")
                    {
                        NoOfCards = Convert.ToDouble(NoOf_Cards.Text.Trim());
                    }
                    TextBox Due_Days = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_DueDays");
                    if (Due_Days.Text.Trim() != "")
                    {
                        DueDays = Convert.ToDouble(Due_Days.Text.Trim());
                    }
                    System.Web.UI.WebControls.CheckBox chkFine = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("cb_weekfine");
                    if (chkFine.Checked == true)
                    {
                        FineType = "1";
                    }
                    else
                    {
                        FineType = "0";
                    }
                    TextBox FineAmount = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_Fine");
                    if (FineAmount.Text.Trim() != "")
                    {
                        Fine = Convert.ToDouble(FineAmount.Text.Trim());
                    }
                    TextBox OvernightFineAmt = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_FineOver");
                    if (OvernightFineAmt.Text.Trim() != "")
                    {
                        OvernightFine = Convert.ToDouble(OvernightFineAmt.Text.Trim());
                    }
                    StrDepDesc = Convert.ToString(ddlDepartment.SelectedItem.Text);
                    //StrDepDesc = d2.GetFunction("SELECT Dept_Name FROM HrDept_Master D,StaffTrans T WHERE D.Dept_Code = T.Dept_Code AND T.Latestrec = 1 AND D.College_Code ='" + college_Code + "'");
                    NoCards = Convert.ToInt32(NoOfCards);

                    if (NoCards > 0)
                    {
                        #region insert for CardType(General)

                        if (StaffCat == "General")
                        {
                            selQry = "SELECT *  FROM TokenDetails WHERE Roll_No ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 0 AND Is_Staff = 1 ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selQry, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                int LastRow = ds.Tables[0].Rows.Count;
                                string TokenNo = Convert.ToString(ds.Tables[0].Rows[LastRow - 1]["Token_No"]);
                                string[] StrMaxToken = TokenNo.Split('.');
                                MaxCard = StrMaxToken[1];
                                StrMaxCard = Convert.ToInt32(MaxCard);
                            }
                            for (int k = StrMaxCard + 1; k <= (StrMaxCard + NoCards); k++)
                            {
                                StrTokNo = staffCode + "A." + k;
                                selQry = "Select token_no from tokendetails where Roll_No ='" + staffCode + "' AND token_no='" + StrTokNo + "' AND Is_Staff = 1  ";
                                dsCommon.Clear();
                                dsCommon = d2.select_method_wo_parameter(selQry, "Text");
                                if (dsCommon.Tables[0].Rows.Count == 0)
                                {
                                    string Time = DateTime.Now.ToString("HH:MM:ss tt");
                                    string Date = DateTime.Now.ToString("MM/dd/yyy");
                                    insertQry = "insert into tokendetails(token_no,roll_no,stud_name,is_staff,dept_name,access_date,access_time,is_locked,category,studcategory,indcategory,Renew_Days,TransLibCode,Book_Type,CardCat) values('" + StrTokNo + "','" + staffCode + "','" + staffName + "','1','" + StrDepDesc + "','" + Date + "', '" + Time + "','0','All','All',0,'','" + StrTransLibCode + "','" + StrBookType + "','" + StrCardCat + "')";
                                    insert = d2.update_method_wo_parameter(insertQry, "Text");
                                    intGenCout = intGenCout + 1;
                                }
                            }

                            selQry = "SELECT * FROM Lib_Master WHERE Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 0 AND Is_Staff = 1 ";

                            if (library != "All")
                            {
                                selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                            }
                            else if (library == "All")
                            {
                                selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                            }
                            if (booktype != "All")
                            {
                                selQry += " AND ISNULL(Book_Type,'All') ='" + booktype + "'";
                            }
                            else if (booktype == "All")
                            {
                                selQry += " AND ISNULL(Book_Type,'All') ='All'";
                            }
                            if (CardCategory != "All")
                            {
                                selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                            }
                            else
                            {
                                selQry += " AND ISNULL(CardCat,'All') ='All'";
                            }
                            dsStaff.Clear();
                            dsStaff = d2.select_method_wo_parameter(selQry, "Text");
                            if (dsStaff.Tables[0].Rows.Count == 0)
                            {
                                insertQry = "INSERT INTO Lib_Master(code,code_descp,batch_year,no_of_token,no_of_days,fine,is_staff,OverNightFine,category,studcategory,Book_Type,IndCategory,Ref_NoofDays,TransLibCode,FineType,CardCat) VALUES ('" + staffCode + "','" + StrDepDesc + "',0," + NoCards + "," + DueDays + "," + Fine + ",1," + OvernightFine + ",'All','All','" + StrBookType + "',0,0,'" + StrTransLibCode + "'," + FineType + ",'" + StrCardCat + "')";
                                insert = d2.update_method_wo_parameter(insertQry, "Text");
                            }
                            else
                            {
                                UpdateQry = "UPDATE Lib_Master SET no_of_token=no_of_token+" + NoCards + ",no_of_days=" + DueDays + ",fine=" + Fine + ",OverNightFine=" + OvernightFine + ",FineType =" + FineType + " WHERE Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 0 AND Is_Staff = 1 ";
                                if (library != "All")
                                {
                                    UpdateQry += "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                                }
                                else if (library == "All")
                                {
                                    UpdateQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                }
                                if (booktype != "All")
                                {
                                    UpdateQry += " AND ISNULL(Book_Type,'All') ='" + booktype + "'";
                                }
                                else if (booktype == "All")
                                {
                                    UpdateQry += " AND ISNULL(Book_Type,'All') ='All'";
                                }
                                if (CardCategory != "All")
                                {
                                    UpdateQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                }
                                else
                                {
                                    UpdateQry += " AND ISNULL(CardCat,'All') ='All'";
                                }
                                update = d2.update_method_wo_parameter(UpdateQry, "Text");
                            }
                        }
                        #endregion

                        #region insert for cardType(Individual)

                        if (StaffCat == "Individual")
                        {
                            selQry = "SELECT * FROM TokenDetails WHERE Roll_No ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 1 AND Is_Staff = 1 ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selQry, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                int LastRow = ds.Tables[0].Rows.Count;
                                string TokenNo = Convert.ToString(ds.Tables[0].Rows[LastRow - 1]["Token_No"]);
                                string[] StrMaxToken = TokenNo.Split('.');
                                MaxCard = StrMaxToken[1];
                                StrMaxCard = Convert.ToInt32(MaxCard);
                            }
                            NoCards = Convert.ToInt32(NoOfCards);
                            for (int k = StrMaxCard + 1; k <= (StrMaxCard + NoCards); k++)
                            {
                                StrTokNo = staffCode + "I." + k;
                                selQry = "Select token_no from tokendetails where Roll_No ='" + staffCode + "' AND token_no='" + StrTokNo + "' AND Is_Staff = 1 ";
                                dsCommon.Clear();
                                dsCommon = d2.select_method_wo_parameter(selQry, "Text");
                                if (dsCommon.Tables[0].Rows.Count == 0)
                                {
                                    string Time = DateTime.Now.ToString("HH:MM:ss tt");
                                    string Date = DateTime.Now.ToString("MM/dd/yyy");
                                    insertQry = "insert into tokendetails(token_no,roll_no,stud_name,is_staff,dept_name,access_date,access_time,is_locked,category,studcategory,indcategory,Renew_Days,TransLibCode,Book_Type,CardCat) values('" + StrTokNo + "','" + staffCode + "','" + staffName + "','1','" + StrDepDesc + "','" + Date + "', '" + Time + "','0','All','All',1,'','" + StrTransLibCode + "','" + StrBookType + "','" + StrCardCat + "')";

                                    insert = d2.update_method_wo_parameter(insertQry, "Text");
                                    intGenCout = intGenCout + 1;

                                }
                            }
                            selQry = "SELECT * FROM Lib_Master WHERE Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 1 AND Is_Staff = 1 ";

                            if (library != "All")
                            {
                                selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                            }
                            else if (library == "All")
                            {
                                selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                            }
                            if (booktype != "All")
                            {
                                selQry += " AND ISNULL(Book_Type,'All') ='" + booktype + "'";
                            }
                            else if (booktype == "All")
                            {
                                selQry += " AND ISNULL(Book_Type,'All') ='All'";
                            }
                            if (CardCategory != "All")
                            {
                                selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                            }
                            else
                            {
                                selQry += " AND ISNULL(CardCat,'All') ='All'";
                            }
                            dsStaff.Clear();
                            dsStaff = d2.select_method_wo_parameter(selQry, "Text");
                            if (dsStaff.Tables[0].Rows.Count == 0)
                            {
                                insertQry = "INSERT INTO Lib_Master(code,code_descp,batch_year,no_of_token,no_of_days,fine,is_staff,OverNightFine,category,studcategory,Book_Type,IndCategory,Ref_NoofDays,TransLibCode,FineType,CardCat) VALUES ('" + staffCode + "','" + StrDepDesc + "',0," + NoCards + "," + DueDays + "," + Fine + ",1," + OvernightFine + ",'All','All','" + StrBookType + "',1,0,'" + StrTransLibCode + "'," + FineType + ",'" + StrCardCat + "')";
                                insert = d2.update_method_wo_parameter(insertQry, "Text");
                            }
                            else
                            {
                                UpdateQry = "UPDATE Lib_Master SET no_of_token=no_of_token+" + NoCards + ",no_of_days=" + DueDays + ",fine=" + Fine + ",OverNightFine=" + OvernightFine + ",FineType =" + FineType + " WHERE Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 1 AND Is_Staff = 1 ";
                                if (library != "All")
                                {
                                    UpdateQry += "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                                }
                                else if (library == "All")
                                {
                                    UpdateQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                }
                                if (booktype != "All")
                                {
                                    UpdateQry += " AND ISNULL(Book_Type,'All') ='" + booktype + "'";
                                }
                                else if (booktype == "All")
                                {
                                    UpdateQry += " AND ISNULL(Book_Type,'All') ='All'";
                                }
                                if (CardCategory != "All")
                                {
                                    UpdateQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                }
                                else
                                {
                                    UpdateQry += " AND ISNULL(CardCat,'All') ='All'";
                                }
                                update = d2.update_method_wo_parameter(UpdateQry, "Text");
                            }
                        }
                        #endregion

                        #region insert for cardType(Merit)

                        if (StaffCat == "Merit")
                        {
                            selQry = "SELECT * FROM TokenDetails WHERE Roll_No ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 2 AND Is_Staff = 1 ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selQry, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                int LastRow = ds.Tables[0].Rows.Count;
                                string TokenNo = Convert.ToString(ds.Tables[0].Rows[LastRow - 1]["Token_No"]);
                                string[] StrMaxToken = TokenNo.Split('.');
                                MaxCard = StrMaxToken[1];
                                StrMaxCard = Convert.ToInt32(MaxCard);
                            }
                            NoCards = Convert.ToInt32(NoOfCards);
                            for (int k = StrMaxCard + 1; k <= (StrMaxCard + NoCards); k++)
                            {
                                StrTokNo = staffCode + "M." + k;
                                selQry = "Select token_no from tokendetails where Roll_No ='" + staffCode + "' AND token_no='" + StrTokNo + "' AND Is_Staff = 1 ";
                                dsCommon.Clear();
                                dsCommon = d2.select_method_wo_parameter(selQry, "Text");
                                if (dsCommon.Tables[0].Rows.Count == 0)
                                {
                                    string Time = DateTime.Now.ToString("HH:MM:ss tt");
                                    string Date = DateTime.Now.ToString("MM/dd/yyy");
                                    insertQry = "insert into tokendetails(token_no,roll_no,stud_name,is_staff,dept_name,access_date,access_time,is_locked,category,studcategory,indcategory,Renew_Days,TransLibCode,Book_Type,CardCat) values('" + StrTokNo + "','" + staffCode + "','" + staffName + "','1','" + StrDepDesc + "','" + Date + "', '" + Time + "','0','All','All',2,'','" + StrTransLibCode + "','" + StrBookType + "','" + StrCardCat + "')";
                                    insert = d2.update_method_wo_parameter(insertQry, "Text");
                                    intGenCout = intGenCout + 1;
                                }
                            }
                            selQry = "SELECT * FROM Lib_Master WHERE Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 2 AND Is_Staff = 1 ";
                            if (library != "All")
                            {
                                selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                            }
                            else if (library == "All")
                            {
                                selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                            }
                            if (booktype != "All")
                            {
                                selQry += " AND ISNULL(Book_Type,'All') ='" + booktype + "'";
                            }
                            else if (booktype == "All")
                            {
                                selQry += " AND ISNULL(Book_Type,'All') ='All'";
                            }
                            if (CardCategory != "All")
                            {
                                selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                            }
                            else
                            {
                                selQry += " AND ISNULL(CardCat,'All') ='All'";
                            }
                            dsStaff.Clear();
                            dsStaff = d2.select_method_wo_parameter(selQry, "Text");
                            if (dsStaff.Tables[0].Rows.Count == 0)
                            {
                                insertQry = "INSERT INTO Lib_Master(code,code_descp,batch_year,no_of_token,no_of_days,fine,is_staff,OverNightFine,category,studcategory,Book_Type,IndCategory,Ref_NoofDays,TransLibCode,FineType,CardCat) VALUES ('" + staffCode + "','" + StrDepDesc + "',0," + NoCards + "," + DueDays + "," + Fine + ",1," + OvernightFine + ",'All','All','" + StrBookType + "',2,0,'" + StrTransLibCode + "'," + FineType + ",'" + StrCardCat + "')";
                                insert = d2.update_method_wo_parameter(insertQry, "Text");
                            }
                            else
                            {
                                UpdateQry = "UPDATE Lib_Master SET no_of_token=no_of_token+" + NoCards + ",no_of_days=" + DueDays + ",fine=" + Fine + ",OverNightFine=" + OvernightFine + ",FineType =" + FineType + " WHERE Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 2 AND Is_Staff = 1 ";
                                if (library != "All")
                                {
                                    UpdateQry += "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                                }
                                else if (library == "All")
                                {
                                    UpdateQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                }
                                if (booktype != "All")
                                {
                                    UpdateQry += " AND ISNULL(Book_Type,'All') ='" + booktype + "'";
                                }
                                else if (booktype == "All")
                                {
                                    UpdateQry += " AND ISNULL(Book_Type,'All') ='All'";
                                }
                                if (CardCategory != "All")
                                {
                                    UpdateQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                }
                                else
                                {
                                    UpdateQry += " AND ISNULL(CardCat,'All') ='All'";
                                }
                                update = d2.update_method_wo_parameter(UpdateQry, "Text");
                            }
                        }

                        #endregion

                        #region insert for cardType(BookBank)

                        if (StaffCat == "Book Bank")
                        {
                            string staffCardcat = Convert.ToString(ddlcategory.SelectedValue);
                            selQry = "SELECT COUNT(*) FROM TokenDetails WHERE Roll_No ='" + staffCode + "' AND ISNULL(Category,'All') = 'Book Bank' AND Is_Staff = 1 AND ISNULL(StudCategory,'All') = '" + staffCardcat + "' ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selQry, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                int LastRow = ds.Tables[0].Rows.Count;
                                string TokenNo = Convert.ToString(ds.Tables[0].Rows[LastRow - 1]["Token_No"]);
                                string[] StrMaxToken = TokenNo.Split('.');
                                MaxCard = StrMaxToken[1];
                                StrMaxCard = Convert.ToInt32(MaxCard);
                            }
                            NoCards = Convert.ToInt32(NoOfCards);
                            for (int k = StrMaxCard + 1; k <= (StrMaxCard + NoCards); k++)
                            {
                                StrTokNo = staffCode + "S." + k;
                                selQry = "Select token_no from tokendetails where Roll_No ='" + staffCode + "' AND token_no='" + StrTokNo + "' AND Is_Staff = 1 ";
                                dsCommon.Clear();
                                dsCommon = d2.select_method_wo_parameter(selQry, "Text");
                                if (dsCommon.Tables[0].Rows.Count == 0)
                                {
                                    string Time = DateTime.Now.ToString("HH:MM:ss tt");
                                    string Date = DateTime.Now.ToString("MM/dd/yyy");
                                    insertQry = "insert into tokendetails(token_no,roll_no,stud_name,is_staff,dept_name,access_date,access_time,is_locked,category,studcategory,indcategory,Renew_Days,TransLibCode,Book_Type,CardCat) values('" + StrTokNo + "','" + staffCode + "','" + staffName + "','1','" + StrDepDesc + "','" + Date + "', '" + Time + "','0','Book Bank','" + staffCardcat + "',0,'','" + StrTransLibCode + "','" + StrBookType + "','" + StrCardCat + "')";

                                    insert = d2.update_method_wo_parameter(insertQry, "Text");
                                    intGenCout = intGenCout + 1;

                                }
                            }
                            selQry = "SELECT * FROM Lib_Master WHERE Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = '" + staffCardcat + "' AND Is_Staff = 1 ";

                            if (library != "All")
                            {
                                selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                            }
                            else if (library == "All")
                            {
                                selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                            }
                            if (booktype != "All")
                            {
                                selQry += " AND ISNULL(Book_Type,'All') ='" + booktype + "'";
                            }
                            else if (booktype == "All")
                            {
                                selQry += " AND ISNULL(Book_Type,'All') ='All'";
                            }
                            if (CardCategory != "All")
                            {
                                selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                            }
                            else
                            {
                                selQry += " AND ISNULL(CardCat,'All') ='All'";
                            }
                            dsStaff.Clear();
                            dsStaff = d2.select_method_wo_parameter(selQry, "Text");
                            if (dsStaff.Tables[0].Rows.Count == 0)
                            {
                                insertQry = "INSERT INTO Lib_Master(code,code_descp,batch_year,no_of_token,no_of_days,fine,is_staff,OverNightFine,category,studcategory,Book_Type,IndCategory,Ref_NoofDays,TransLibCode,FineType,CardCat) VALUES ('" + staffCode + "','" + StrDepDesc + "',0," + NoCards + "," + DueDays + "," + Fine + ",1," + OvernightFine + ",'Book Bank','" + staffCardcat + "','" + StrBookType + "',0,0,'" + StrTransLibCode + "'," + FineType + ",'" + StrCardCat + "')";
                                insert = d2.update_method_wo_parameter(insertQry, "Text");
                            }
                            else
                            {
                                UpdateQry = "UPDATE Lib_Master SET no_of_token=no_of_token+" + NoCards + ",no_of_days=" + DueDays + ",fine=" + Fine + ",OverNightFine=" + OvernightFine + ",FineType =" + FineType + " WHERE Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = '" + staffCardcat + "' AND Is_Staff = 1 ";
                                if (library != "All")
                                {
                                    UpdateQry += "AND ISNULL(TransLibCode,'All') ='" + library + "' ";
                                }
                                else if (library == "All")
                                {
                                    UpdateQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                }
                                if (booktype != "All")
                                {
                                    UpdateQry += " AND ISNULL(Book_Type,'All') ='" + booktype + "'";
                                }
                                else if (booktype == "All")
                                {
                                    UpdateQry += " AND ISNULL(Book_Type,'All') ='All'";
                                }
                                if (CardCategory != "All")
                                {
                                    UpdateQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                }
                                else
                                {
                                    UpdateQry += " AND ISNULL(CardCat,'All') ='All'";
                                }
                                update = d2.update_method_wo_parameter(UpdateQry, "Text");
                            }
                        }
                        #endregion
                    }
                }
            }
            if (intGenCout > 0)
            {
                Surediv.Visible = false;
                Div3.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Text = "Card(s) Generated Successfully";
                btngo_Click(sender, e);

            }
            else
            {
                Surediv.Visible = false;
                Div3.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Text = "No cards have been Generated";

            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "individualstudent");
        }
    }

    protected void StudentCardGen(object sender, EventArgs e)
    {
        try
        {
            string booktype = Convert.ToString(ddlbooktype.SelectedValue);
            string library = Convert.ToString(ddlLibrary.SelectedValue);
            string CardCategory = Convert.ToString(ddl_CardCatogery.SelectedValue);
            string college_Code = Convert.ToString(ddl_collegename.SelectedItem.Value);
            string batchYr = Convert.ToString(ddlBatch.SelectedValue);
            string degCode = Convert.ToString(ddlbranch.SelectedItem.Value);
            string category = Convert.ToString(ddlcategory.SelectedValue);
            string Studcard = Convert.ToString(ddlcard.SelectedValue);
            string StrBookType = string.Empty;
            string StrTransLibCode = string.Empty;
            string StrCardCat = string.Empty;
            string selectQry = string.Empty;
            string code = string.Empty;
            string code_Desc = string.Empty;
            string StrRenewDays = string.Empty;
            string StrCardCount = string.Empty;
            string GetCourseName = "";
            string GetDeptName = "";
            string FineType = string.Empty;
            string StrDepDesc = string.Empty;
            int StrMaxCard = 0;
            string MaxCard = string.Empty;
            string StrTokNo = string.Empty;
            string insertQry = string.Empty;
            string UpdateQry = string.Empty;
            int insert = 0;
            int update = 0;
            int intGenCout = 0;
            double NoCards = 0;
            double NoOfCards = 0;
            double Fine = 0;
            double OvernightFine = 0;
            double DueDays = 0;
            string Name = "";
            string roll_no = "";

            if (booktype == "Book")
                StrBookType = "BOK";
            if (booktype == "Periodicals")
                StrBookType = "PER";
            if (booktype == "Project Book")
                StrBookType = "PRO";
            if (booktype == "Non-Book Material")
                StrBookType = "NBM";
            if (booktype == "Question Bank")
                StrBookType = "QBA";
            if (booktype == "Back Volume")
                StrBookType = "BVO";
            if (booktype == "Reference Volume")
                StrBookType = "REF";
            if (booktype == "All")
                StrBookType = "All";

            if (library != "All")
                StrTransLibCode = library;
            else
                StrTransLibCode = "All";
            if (CardCategory != "All")
                StrCardCat = CardCategory;
            else
                StrCardCat = "All";

            selectQry = " Select distinct degree.course_id,degree.dept_code,degree.degree_code from degree,registration where degree.college_code='" + college_Code + "'and degree.degree_code=registration.degree_code and registration.batch_year='" + batchYr + "'and registration.degree_code='" + degCode + "' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectQry, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                string C_id = Convert.ToString(ds.Tables[0].Rows[0]["course_id"]);
                string dept_Code = Convert.ToString(ds.Tables[0].Rows[0]["dept_code"]);
                if (!string.IsNullOrEmpty(C_id) && !string.IsNullOrEmpty(dept_Code))
                {
                    selectQry = "select Course_Name from course where Course_Id='" + C_id + "'";
                    GetCourseName = d2.GetFunction(selectQry);
                    selectQry = "select Dept_Name from department where Dept_code='" + dept_Code + "'";
                    GetDeptName = d2.GetFunction(selectQry);
                    code = Convert.ToString(ds.Tables[0].Rows[0]["course_id"]) + "~" + Convert.ToString(ds.Tables[0].Rows[0]["dept_code"]);
                    code_Desc = GetCourseName + "-" + GetDeptName;
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Degree not Set";
                }
            }
            selectQry = "SELECT * FROM Lib_Master WHERE Batch_Year =" + batchYr + " AND Code ='" + code + "' AND Is_Staff = 0 ";

            if (Studcard == "Book Bank")
                selectQry += " AND ISNULL(Category,'All') = 'Book Bank' ";
            else
                selectQry += " AND ISNULL(Category,'All') = 'All' ";
            if (category == "All" || category == "")
                selectQry += " AND ISNULL(StudCategory,'All') = 'All'";
            else
                selectQry += " AND ISNULL(StudCategory,'All') = 'SC/ST Category'";
            if (library != "All")
                selectQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
            else
                selectQry += "AND ISNULL(TransLibCode,'All') ='All'";
            if (booktype != "All")
                selectQry += " AND ISNULL(Book_Type,'All') ='" + booktype + "'";
            else if (booktype == "All")
                selectQry += " AND ISNULL(Book_Type,'All') ='All'";
            if (CardCategory != "All")
                selectQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
            else
                selectQry += " AND ISNULL(CardCat,'All') ='All'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectQry, "Text");

            if (ds.Tables[0].Rows.Count == 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Cards are not generated";
            }
            foreach (GridViewRow gvrow in grdindividual.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                if (chk.Checked == true)
                {
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    Label rollno = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_roll_no");
                    if (rollno.Text.Trim() != "")
                    {
                        roll_no = rollno.Text.Trim();
                    }
                    Label stu_Name = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_Name");
                    if (stu_Name.Text.Trim() != "")
                    {
                        Name = stu_Name.Text.Trim();
                    }
                    TextBox NoOf_Cards = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_AddOrDel");
                    if (NoOf_Cards.Text.Trim() != "")
                    {
                        NoOfCards = Convert.ToDouble(NoOf_Cards.Text.Trim());
                    }
                    TextBox Due_Days = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_DueDays");
                    if (Due_Days.Text.Trim() != "")
                    {
                        DueDays = Convert.ToDouble(Due_Days.Text.Trim());
                    }
                    System.Web.UI.WebControls.CheckBox chkFine = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("cb_weekfine");
                    if (chkFine.Checked == true)
                    {
                        FineType = "1";
                    }
                    else
                    {
                        FineType = "0";
                    }
                    TextBox FineAmt = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_Fine");
                    if (FineAmt.Text.Trim() != "")
                    {
                        Fine = Convert.ToDouble(FineAmt.Text.Trim());
                    }
                    TextBox OvernightFineAmt = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_FineOver");
                    if (OvernightFineAmt.Text.Trim() != "")
                    {
                        OvernightFine = Convert.ToDouble(OvernightFineAmt.Text.Trim());
                    }
                    StrDepDesc = Convert.ToString(ddlbranch.SelectedItem.Text);
                    NoCards = Convert.ToDouble(NoOfCards);
                    if (NoCards > 0)
                    {
                        #region insert for CardType(General)

                        if (Studcard == "General")
                        {
                            selectQry = "SELECT * FROM TokenDetails WHERE Roll_No ='" + roll_no + "' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 0 AND Is_Staff = 0 ORDER BY LEN(Token_No),Token_No ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                int LastRow = ds.Tables[0].Rows.Count;
                                string TokenNo = Convert.ToString(ds.Tables[0].Rows[LastRow - 1]["Token_No"]);
                                string[] StrMaxToken = TokenNo.Split('.');
                                MaxCard = StrMaxToken[1];
                                StrMaxCard = Convert.ToInt32(MaxCard);
                            }

                            for (int k = StrMaxCard + 1; k <= (StrMaxCard + NoCards); k++)
                            {
                                StrTokNo = roll_no + "A." + k;
                                selectQry = "Select token_no from tokendetails where Roll_No ='" + roll_no + "' AND token_no='" + StrTokNo + "' AND Is_Staff = 0 ";
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(selectQry, "Text");
                                if (ds.Tables[0].Rows.Count == 0)
                                {
                                    string Time = DateTime.Now.ToString("HH:MM:ss tt");
                                    string Date = DateTime.Now.ToString("MM/dd/yyy");
                                    insertQry = "insert into tokendetails(token_no,roll_no,stud_name,is_staff,dept_name,access_date,access_time,is_locked,category,studcategory,indcategory,Renew_Days,TransLibCode,Book_Type,CardCat) values('" + StrTokNo + "','" + roll_no + "','" + Name + "','0','" + StrDepDesc + "','" + Date + "', '" + Time + "','0','All','All',0,'','" + StrTransLibCode + "','" + StrBookType + "','" + StrCardCat + "')";
                                    insert = d2.update_method_wo_parameter(insertQry, "Text");
                                    intGenCout = intGenCout + 1;
                                }
                            }
                        }
                        #endregion

                        #region insert for CardType(Individual)

                        if (Studcard == "Individual")
                        {
                            selectQry = "SELECT * FROM TokenDetails WHERE Roll_No ='" + roll_no + "' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 1 AND Is_Staff = 0 ORDER BY LEN(Token_No),Token_No  ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                int LastRow = ds.Tables[0].Rows.Count;
                                string TokenNo = Convert.ToString(ds.Tables[0].Rows[LastRow - 1]["Token_No"]);
                                string[] StrMaxToken = TokenNo.Split('.');
                                MaxCard = StrMaxToken[1];
                                StrMaxCard = Convert.ToInt32(MaxCard);
                            }

                            for (int k = StrMaxCard + 1; k <= (StrMaxCard + NoCards); k++)
                            {
                                StrTokNo = roll_no + "I." + k;
                                selectQry = "Select token_no from tokendetails where Roll_No ='" + roll_no + "' AND token_no='" + StrTokNo + "' AND Is_Staff = 0 ";
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(selectQry, "Text");
                                if (ds.Tables[0].Rows.Count == 0)
                                {
                                    string Time = DateTime.Now.ToString("HH:MM:ss tt");
                                    string Date = DateTime.Now.ToString("MM/dd/yyy");
                                    insertQry = "insert into tokendetails(token_no,roll_no,stud_name,is_staff,dept_name,access_date,access_time,is_locked,category,studcategory,indcategory,Renew_Days,TransLibCode,Book_Type,CardCat) values('" + StrTokNo + "','" + roll_no + "','" + Name + "','0','" + StrDepDesc + "','" + Date + "', '" + Time + "','0','All','All',1,'','" + StrTransLibCode + "','" + StrBookType + "','" + StrCardCat + "')";
                                    insert = d2.update_method_wo_parameter(insertQry, "Text");
                                    intGenCout = intGenCout + 1;
                                }
                            }
                        }
                        #endregion

                        #region insert for CardType(Merit)

                        if (Studcard == "Merit")
                        {
                            selectQry = "SELECT COUNT(*) FROM TokenDetails WHERE Roll_No ='" + roll_no + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 2 AND Is_Staff = 0 ORDER BY LEN(Token_No),Token_No";
                            MaxCard = d2.GetFunction(selectQry);
                            StrMaxCard = Convert.ToInt32(MaxCard);
                            for (int k = StrMaxCard + 1; k <= (StrMaxCard + NoCards); k++)
                            {
                                StrTokNo = roll_no + "M." + k;
                                selectQry = "Select token_no from tokendetails where Roll_No ='" + roll_no + "' AND token_no='" + StrTokNo + "' AND Is_Staff = 1 ";
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(selectQry, "Text");
                                if (ds.Tables[0].Rows.Count == 0)
                                {
                                    string Time = DateTime.Now.ToString("HH:MM:ss tt");
                                    string Date = DateTime.Now.ToString("MM/dd/yyy");
                                    insertQry = "insert into tokendetails(token_no,roll_no,stud_name,is_staff,dept_name,access_date,access_time,is_locked,category,studcategory,indcategory,Renew_Days,TransLibCode,Book_Type,CardCat) values('" + StrTokNo + "','" + roll_no + "','" + Name + "','0','" + StrDepDesc + "','" + Date + "', '" + Time + "','0','All','All',2,'','" + StrTransLibCode + "','" + StrBookType + "','" + StrCardCat + "')";
                                    insert = d2.update_method_wo_parameter(insertQry, "Text");
                                    intGenCout = intGenCout + 1;

                                }
                            }
                        }

                        #endregion

                        #region insert for cardType(BookBank)

                        if (Studcard == "Book Bank")
                        {
                            selectQry = "SELECT COUNT(*) FROM TokenDetails WHERE Roll_No ='" + roll_no + "' AND ISNULL(Category,'All') = 'Book Bank' AND Is_Staff = 0 AND ISNULL(StudCategory,'All') = '" + category + "' ";
                            MaxCard = d2.GetFunction(selectQry);
                            StrMaxCard = Convert.ToInt32(MaxCard);
                            for (int k = StrMaxCard + 1; k <= (StrMaxCard + NoCards); k++)
                            {
                                StrTokNo = roll_no + "S." + k;
                                selectQry = "Select token_no from tokendetails where Roll_No ='" + roll_no + "' AND token_no='" + StrTokNo + "' AND Is_Staff = 1 ";
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(selectQry, "Text");
                                if (ds.Tables[0].Rows.Count == 0)
                                {
                                    string Time = DateTime.Now.ToString("HH:MM:ss tt");
                                    string Date = DateTime.Now.ToString("MM/dd/yyy");
                                    insertQry = "insert into tokendetails(token_no,roll_no,stud_name,is_staff,dept_name,access_date,access_time,is_locked,category,studcategory,indcategory,Renew_Days,TransLibCode,Book_Type,CardCat) values('" + StrTokNo + "','" + roll_no + "','" + Name + "','0','" + StrDepDesc + "','" + Date + "', '" + Time + "','0','Book Bank','" + category + "',0,'','" + StrTransLibCode + "','" + StrBookType + "','" + StrCardCat + "')";
                                    insert = d2.update_method_wo_parameter(insertQry, "Text");
                                    intGenCout = intGenCout + 1;
                                }
                            }

                            selectQry = "SELECT * FROM Lib_Master WHERE Batch_Year ='" + batchYr + "' AND Code ='" + code + "' AND ISNULL(Category,'All') = 'Book Bank' AND Is_Staff = 0  AND ISNULL(StudCategory,'All') = '" + category + "' ";

                            if (library != "All")
                                selectQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                            else
                                selectQry += "AND ISNULL(TransLibCode,'All') ='All'";
                            if (booktype != "All")
                                selectQry += " AND ISNULL(Book_Type,'All') ='" + booktype + "'";
                            else if (booktype == "All")
                                selectQry += " AND ISNULL(Book_Type,'All') ='All'";
                            if (CardCategory != "All")
                                selectQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                            else
                                selectQry += " AND ISNULL(CardCat,'All') ='All'";

                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQry, "Text");

                            if (ds.Tables[0].Rows.Count == 0)
                            {
                                insertQry = "INSERT INTO Lib_Master(code,code_descp,batch_year,no_of_token,no_of_days,fine,is_staff,OverNightFine,category,studcategory,Book_Type,IndCategory,Ref_NoofDays,TransLibCode,FineType,CardCat) VALUES ('" + code + "','" + code_Desc + "','" + batchYr + "'," + NoOfCards + "," + DueDays + "," + Fine + ",0," + OvernightFine + ",'Book Bank','" + category + "','" + StrBookType + "',0,0,'" + StrTransLibCode + "'," + FineType + ",'" + StrCardCat + "')";
                                insert = d2.update_method_wo_parameter(insertQry, "Text");
                            }
                            else
                            {
                                UpdateQry = "UPDATE Lib_Master SET no_of_days=" + DueDays + ",fine=" + Fine + ",OverNightFine=" + OvernightFine + ",FineType =" + FineType + " WHERE Batch_Year =" + batchYr + " AND Code ='" + code_Desc + "' AND ISNULL(Category,'All') = 'Book Bank' AND Is_Staff = 0 AND ISNULL(StudCategory,'All') = '" + category + "' ";
                                if (library != "All")
                                    UpdateQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                else
                                    UpdateQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                if (booktype != "All")
                                    UpdateQry += " AND ISNULL(Book_Type,'All') ='" + booktype + "'";
                                else if (booktype == "All")
                                    UpdateQry += " AND ISNULL(Book_Type,'All') ='All'";
                                if (CardCategory != "All")
                                    UpdateQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                else
                                    UpdateQry += " AND ISNULL(CardCat,'All') ='All'";
                                update = d2.update_method_wo_parameter(UpdateQry, "Text");
                            }
                        }
                        #endregion
                    }
                }
            }
            if (intGenCout > 0)
            {
                Surediv.Visible = false;
                Div3.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Text = "Card(s) Generated Successfully";
                btngo_Click(sender, e);

            }
            else
            {
                Surediv.Visible = false;
                Div3.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Text = "No cards have been Generated";
            }
        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, userCollegeCode, "individualstudent");
        }
    }

    protected void btnSure_no_Click(object sender, EventArgs e)
    {
        Surediv.Visible = false;
        imgdiv2.Visible = true;
        lbl_alert.Text = "Cards are not generated";
    }

    #region Delete Card

    protected void btnDeleteCard_Click(object sender, EventArgs e)
    {
        try
        {
            double NoOfcards = 0;
            double Available = 0;
            int selectedcount = 0;
            if (rblstaff.SelectedIndex == 0)
            {
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    if (chk.Checked == true)
                    {
                        selectedcount++;
                        int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                        if (ddlcard.SelectedItem.Text == "General")
                        {
                            Label general = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_General");
                            if (general.Text.Trim() != "")
                            {
                                Available = Convert.ToDouble(general.Text.Trim());
                            }
                        }
                        if (ddlcard.SelectedItem.Text == "Individual")
                        {
                            Label Individual = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_Individual");
                            if (Individual.Text.Trim() != "")
                            {
                                Available = Convert.ToDouble(Individual.Text.Trim());
                            }
                        }
                        if (ddlcard.SelectedItem.Text == "Merit")
                        {
                            Label Merit = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_Merit");
                            if (Merit.Text.Trim() != "")
                            {
                                Available = Convert.ToDouble(Merit.Text.Trim());
                            }
                        }
                        if (ddlcard.SelectedItem.Text == "Book Bank")
                        {
                            Label BkBank = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_Book");
                            if (BkBank.Text.Trim() != "")
                            {
                                Available = Convert.ToDouble(BkBank.Text.Trim());
                            }
                        }
                        DelCard = true;
                        TextBox NoCard = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_AddOrDel");
                        if (NoCard.Text.Trim() != "")
                        {
                            NoOfcards = Convert.ToDouble(NoCard.Text.Trim());
                        }
                        if (NoOfcards == 0)
                        {
                            DelCard = false;
                            imgdiv2.Visible = true;
                            lbl_alert.Text = "Enter number of cards to delete";
                            return;
                        }
                        if (Available < NoOfcards)
                        {
                            DelCard = false;
                            imgdiv2.Visible = true;
                            lbl_alert.Text = "No. of card should be less than Available Card";
                            return;
                        }
                    }
                }
                if (DelCard == true)
                {
                    sureDivDel.Visible = true;
                    divDel.Visible = true;
                    lbldel.Visible = true;
                }
                if (selectedcount == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Select any Student to Delete the Card";
                }
            }
            if (rblstaff.SelectedIndex == 1)
            {
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    if (chk.Checked == true)
                    {
                        selectedcount++;
                        int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                        if (ddlcard.SelectedItem.Text == "General")
                        {
                            Label general = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_General");
                            if (general.Text.Trim() != "")
                            {
                                Available = Convert.ToDouble(general.Text.Trim());
                            }
                        }
                        if (ddlcard.SelectedItem.Text == "Individual")
                        {
                            Label Individual = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_Individual");
                            if (Individual.Text.Trim() != "")
                            {
                                Available = Convert.ToDouble(Individual.Text.Trim());
                            }
                        }
                        if (ddlcard.SelectedItem.Text == "Merit")
                        {
                            Label Merit = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_Merit");
                            if (Merit.Text.Trim() != "")
                            {
                                Available = Convert.ToDouble(Merit.Text.Trim());
                            }
                        }
                        if (ddlcard.SelectedItem.Text == "Book Bank")
                        {
                            Label BkBank = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_Book");
                            if (BkBank.Text.Trim() != "")
                            {
                                Available = Convert.ToDouble(BkBank.Text.Trim());
                            }
                        }
                        DelCard = true;
                        TextBox NoCard = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_AddOrDel");
                        if (NoCard.Text.Trim() != "")
                        {
                            NoOfcards = Convert.ToDouble(NoCard.Text.Trim());
                        }
                        if (NoOfcards == 0)
                        {
                            DelCard = false;
                            imgdiv2.Visible = true;
                            lbl_alert.Text = "Enter number of cards to delete";
                            return;
                        }
                        if (Available < NoOfcards)
                        {
                            DelCard = false;
                            imgdiv2.Visible = true;
                            lbl_alert.Text = "No. of card should be less than Available Card";
                            return;
                        }
                    }
                }
                if (DelCard == true)
                {
                    sureDivDel.Visible = true;
                    divDel.Visible = true;
                    lbldel.Visible = true;
                }
                if (selectedcount == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Select any Staff to Delete the Card";
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnDelYes_Click(object sender, EventArgs e)
    {
        if (rblstaff.SelectedIndex == 0)
        {
            StudentCardDel(sender, e);
        }
        if (rblstaff.SelectedIndex == 1)
        {
            StaffCardDel(sender, e);
            sureDivDel.Visible = false;
        }
    }

    protected void StudentCardDel(object sender, EventArgs e)
    {
        try
        {
            string studCard = Convert.ToString(ddlcard.SelectedValue);
            string batch = Convert.ToString(ddlBatch.SelectedValue);
            string library = Convert.ToString(ddlLibrary.SelectedValue);
            string CardCategory = Convert.ToString(ddl_CardCatogery.SelectedValue);
            string category = Convert.ToString(ddlcategory.SelectedValue);
            string booktype = Convert.ToString(ddlbooktype.SelectedValue);
            string roll_no = string.Empty;
            string StrDegCode = string.Empty;
            string selQry = string.Empty;
            string selectQry = string.Empty;
            string DelQry = string.Empty;
            string updateQry = string.Empty;
            int update = 0;
            int delete = 0;
            string StrBookType = string.Empty;

            double NoOfCards = 0;
            int intDeleteCount = 0;
            int intTotDelCard = 0;

            if (booktype == "Book")
                StrBookType = "BOK";
            if (booktype == "Periodicals")
                StrBookType = "PER";
            if (booktype == "Project Book")
                StrBookType = "PRO";
            if (booktype == "Non-Book Material")
                StrBookType = "NBM";
            if (booktype == "Question Bank")
                StrBookType = "QBA";
            if (booktype == "Back Volume")
                StrBookType = "BVO";
            if (booktype == "Reference Volume")
                StrBookType = "REF";
            if (booktype == "All")
                StrBookType = "All";

            #region Delete for General
            if (studCard == "General")
            {
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    if (chk.Checked == true)
                    {
                        int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    startGen:
                        Label rollno = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_roll_no");
                        if (rollno.Text.Trim() != "")
                        {
                            roll_no = rollno.Text.Trim();
                        }
                        TextBox NoOf_Cards = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_AddOrDel");
                        if (NoOf_Cards.Text.Trim() != "")
                        {
                            NoOfCards = Convert.ToDouble(NoOf_Cards.Text.Trim());
                        }

                        selQry = "SELECT ltrim(str(Course_ID))+'~'+ ltrim(str(Dept_Code)) Degree FROM Registration R,Degree G WHERE R.Degree_Code = G.Degree_Code AND R.Roll_No ='" + roll_no + "' ";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selQry, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            StrDegCode = Convert.ToString(ds.Tables[0].Rows[0]["Degree"]);
                        }
                        if (chk.Checked == true && (NoOfCards) > 0)
                        {
                            selQry = "SELECT * FROM TokenDetails WHERE Roll_No ='" + roll_no + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 0 AND Is_Staff = 0 ";

                            if (library != "All")
                                selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                            else
                                selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                            if (booktype != "All")
                                selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                            else
                                selQry += " AND ISNULL(Book_Type,'All') ='All'";
                            if (CardCategory != "All")
                                selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                            else
                                selQry += " AND ISNULL(CardCat,'All') ='All'";
                            selQry += " ORDER BY LEN(Token_No) DESC,Token_No DESC ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selQry, "Text");

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                {
                                    string tokenNo = Convert.ToString(ds.Tables[0].Rows[j]["Token_No"]);

                                    selQry = "SELECT * FROM Borrow WHERE Roll_No ='" + roll_no + "' AND Token_No ='" + tokenNo + "' AND Return_Flag = 0 AND Is_Staff = 0 ";
                                    dsload.Clear();
                                    dsload = d2.select_method_wo_parameter(selQry, "Text");
                                    if (dsload.Tables[0].Rows.Count == 0)
                                    {
                                        selQry = "SELECT * FROM Borrow WHERE Roll_No ='" + roll_no + "' AND Token_No ='" + tokenNo + "' AND Is_Staff = 0 ";
                                        dsload.Clear();
                                        dsload = d2.select_method_wo_parameter(selQry, "Text");
                                        if (dsload.Tables[0].Rows.Count == 0)
                                        {
                                            DelQry = "DELETE FROM TokenDetails WHERE Roll_No ='" + roll_no + "' AND Token_No ='" + tokenNo + "' AND IndCategory = 0 AND Is_Staff = 0 ";
                                            if (library != "All")
                                                DelQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                            else
                                                DelQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                            if (booktype != "All")
                                                DelQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                            else
                                                DelQry += " AND ISNULL(Book_Type,'All') ='All'";
                                            if (CardCategory != "All")
                                                DelQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                            else
                                                DelQry += " AND ISNULL(CardCat,'All') ='All'";

                                            delete = d2.update_method_wo_parameter(DelQry, "Text");
                                            if (delete == 1)
                                            {
                                                intDeleteCount = intDeleteCount + 1;
                                                intTotDelCard = intTotDelCard + 1;
                                            }
                                        }
                                        else
                                        {
                                            selQry = "SELECT Token_No FROM TokenDetails WHERE Is_Locked = 0 AND Roll_No ='" + roll_no + "' AND Token_No <> '" + tokenNo + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 0 AND Is_Staff = 0 ";
                                            if (library != "All")
                                                selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                            else
                                                selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                            if (booktype != "All")
                                                selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                            else
                                                selQry += " AND ISNULL(Book_Type,'All') ='All'";
                                            if (CardCategory != "All")
                                                selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                            else
                                                selQry += " AND ISNULL(CardCat,'All') ='All'";
                                            dsload.Clear();
                                            dsload = d2.select_method_wo_parameter(selQry, "Text");
                                            if (dsload.Tables[0].Rows.Count > 0)
                                            {

                                                updateQry = "UPDATE Borrow SET Token_No ='" + tokenNo + "' WHERE Roll_No ='" + roll_no + "' AND Token_No ='" + tokenNo + "' AND Is_Staff = 0 ";
                                                update = d2.update_method_wo_parameter(updateQry, "Text");
                                                DelQry = "DELETE FROM TokenDetails WHERE Roll_No ='" + roll_no + "' AND Token_No ='" + tokenNo + "' AND IndCategory = 0 AND Is_Staff = 0 ";
                                                if (library != "All")
                                                    DelQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                else
                                                    DelQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                if (booktype != "All")
                                                    DelQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                else
                                                    DelQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                if (CardCategory != "All")
                                                    DelQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                else
                                                    DelQry += " AND ISNULL(CardCat,'All') ='All'";

                                                delete = d2.update_method_wo_parameter(DelQry, "Text");
                                                if (delete == 1)
                                                {
                                                    intDeleteCount = intDeleteCount + 1;
                                                    intTotDelCard = intTotDelCard + 1;
                                                }
                                            }
                                        }
                                    }
                                    if (NoOfCards == intDeleteCount && RowCnt < grdindividual.Rows.Count)
                                    {
                                        RowCnt = RowCnt + 1;
                                        goto startGen;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region Delete for CardType(individual)
            if (studCard == "Individual")
            {
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    if (chk.Checked == true)
                    {
                        int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    startGen:
                        Label rollno = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_roll_no");
                        if (rollno.Text.Trim() != "")
                        {
                            roll_no = rollno.Text.Trim();
                        }
                        TextBox NoOf_Cards = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_AddOrDel");
                        if (NoOf_Cards.Text.Trim() != "")
                        {
                            NoOfCards = Convert.ToDouble(NoOf_Cards.Text.Trim());
                        }
                        selQry = "SELECT ltrim(str(Course_ID))+'~'+ ltrim(str(Dept_Code)) Degree FROM Registration R,Degree G WHERE R.Degree_Code = G.Degree_Code AND R.Roll_No ='" + roll_no + "' ";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selQry, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            StrDegCode = Convert.ToString(ds.Tables[0].Rows[0]["Degree"]);
                        }
                        if (chk.Checked == true && (NoOfCards) > 0)
                        {
                            selQry = "SELECT * FROM TokenDetails WHERE Roll_No ='" + roll_no + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 1 AND Is_Staff = 0 ";

                            if (library != "All")
                                selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                            else
                                selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                            if (booktype != "All")
                                selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                            else
                                selQry += " AND ISNULL(Book_Type,'All') ='All'";
                            if (CardCategory != "All")
                                selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                            else
                                selQry += " AND ISNULL(CardCat,'All') ='All'";
                            selQry += " ORDER BY LEN(Token_No) DESC,Token_No DESC ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selQry, "Text");

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                {
                                    string tokenNo = Convert.ToString(ds.Tables[0].Rows[j]["Token_No"]);

                                    selQry = "SELECT * FROM Borrow WHERE Roll_No ='" + roll_no + "' AND Token_No ='" + tokenNo + "' AND Return_Flag = 0 AND Is_Staff = 0 ";
                                    dsload.Clear();
                                    dsload = d2.select_method_wo_parameter(selQry, "Text");
                                    if (dsload.Tables[0].Rows.Count == 0)
                                    {
                                        selQry = "SELECT * FROM Borrow WHERE Roll_No ='" + roll_no + "' AND Token_No ='" + tokenNo + "' AND Is_Staff = 0 ";
                                        dsload.Clear();
                                        dsload = d2.select_method_wo_parameter(selQry, "Text");
                                        if (dsload.Tables[0].Rows.Count == 0)
                                        {
                                            DelQry = "DELETE FROM TokenDetails WHERE Roll_No ='" + roll_no + "' AND Token_No ='" + tokenNo + "' AND IndCategory = 1 AND Is_Staff = 0 ";

                                            if (library != "All")
                                                DelQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                            else
                                                DelQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                            if (booktype != "All")
                                                DelQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                            else
                                                DelQry += " AND ISNULL(Book_Type,'All') ='All'";
                                            if (CardCategory != "All")
                                                DelQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                            else
                                                DelQry += " AND ISNULL(CardCat,'All') ='All'";

                                            delete = d2.update_method_wo_parameter(DelQry, "Text");
                                            if (delete == 1)
                                            {
                                                intDeleteCount = intDeleteCount + 1;
                                                intTotDelCard = intTotDelCard + 1;

                                                selQry = "SELECT No_Of_Token FROM Lib_Master WHERE Batch_Year =" + batch + " AND Code ='" + roll_no + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 1 AND Is_Staff = 0 ";
                                                if (library != "All")
                                                    selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                else
                                                    selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                if (booktype != "All")
                                                    selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                else
                                                    selQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                if (CardCategory != "All")
                                                    selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                else
                                                    selQry += " AND ISNULL(CardCat,'All') ='All'";
                                                ds.Clear();
                                                ds = d2.select_method_wo_parameter(selQry, "Text");
                                                if (ds.Tables[0].Rows.Count > 0)
                                                {
                                                    int NoOfToken = Convert.ToInt32(ds.Tables[0].Rows[0]["No_Of_Token"]);
                                                    if (NoOfToken > 0)
                                                    {

                                                        updateQry = "UPDATE Lib_Master SET No_Of_Token = No_Of_Token - 1 WHERE  Batch_Year =" + batch + " AND Code ='" + roll_no + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 1 AND Is_Staff = 0 ";

                                                        if (library != "All")
                                                            updateQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                        else
                                                            updateQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                        if (booktype != "All")
                                                            updateQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                        else
                                                            updateQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                        if (CardCategory != "All")
                                                            updateQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                        else
                                                            updateQry += " AND ISNULL(CardCat,'All') ='All'";
                                                        update = d2.update_method_wo_parameter(updateQry, "Text");

                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            selQry = "SELECT Token_No FROM TokenDetails WHERE Is_Locked = 0 AND Roll_No ='" + roll_no + "' AND Token_No <> '" + tokenNo + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 1 AND Is_Staff = 0 ";
                                            if (library != "All")
                                                selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                            else
                                                selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                            if (booktype != "All")
                                                selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                            else
                                                selQry += " AND ISNULL(Book_Type,'All') ='All'";
                                            if (CardCategory != "All")
                                                selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                            else
                                                selQry += " AND ISNULL(CardCat,'All') ='All'";
                                            dsload.Clear();
                                            dsload = d2.select_method_wo_parameter(selQry, "Text");
                                            if (dsload.Tables[0].Rows.Count > 0)
                                            {

                                                updateQry = "UPDATE Borrow SET Token_No ='" + tokenNo + "' WHERE Roll_No ='" + roll_no + "' AND Token_No ='" + tokenNo + "' AND Is_Staff = 0 ";
                                                update = d2.update_method_wo_parameter(updateQry, "Text");
                                                DelQry = "DELETE FROM TokenDetails WHERE Roll_No ='" + roll_no + "' AND Token_No ='" + tokenNo + "' AND IndCategory = 1 AND Is_Staff = 0 ";
                                                if (library != "All")
                                                    DelQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                else
                                                    DelQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                if (booktype != "All")
                                                    DelQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                else
                                                    DelQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                if (CardCategory != "All")
                                                    DelQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                else
                                                    DelQry += " AND ISNULL(CardCat,'All') ='All'";

                                                delete = d2.update_method_wo_parameter(DelQry, "Text");
                                                if (delete == 1)
                                                {
                                                    intDeleteCount = intDeleteCount + 1;
                                                    intTotDelCard = intTotDelCard + 1;
                                                    selQry = "SELECT No_Of_Token FROM Lib_Master WHERE Batch_Year =" + batch + " AND Code ='" + roll_no + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 1 AND Is_Staff = 0 ";
                                                    if (library != "All")
                                                        selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                    else
                                                        selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                    if (booktype != "All")
                                                        selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                    else
                                                        selQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                    if (CardCategory != "All")
                                                        selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                    else
                                                        selQry += " AND ISNULL(CardCat,'All') ='All'";
                                                    ds.Clear();
                                                    ds = d2.select_method_wo_parameter(selQry, "Text");
                                                    if (ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        int NoOfToken = Convert.ToInt32(ds.Tables[0].Rows[0]["No_Of_Token"]);
                                                        if (NoOfToken > 0)
                                                        {

                                                            updateQry = "UPDATE Lib_Master SET No_Of_Token = No_Of_Token - 1 WHERE  Batch_Year =" + batch + " AND Code ='" + roll_no + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 1 AND Is_Staff = 0 ";

                                                            if (library != "All")
                                                                updateQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                            else
                                                                updateQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                            if (booktype != "All")
                                                                updateQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                            else
                                                                updateQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                            if (CardCategory != "All")
                                                                updateQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                            else
                                                                updateQry += " AND ISNULL(CardCat,'All') ='All'";
                                                            update = d2.update_method_wo_parameter(updateQry, "Text");

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (NoOfCards == intDeleteCount && RowCnt < grdindividual.Rows.Count)
                                    {
                                        RowCnt = RowCnt + 1;
                                        goto startGen;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region Delete for CardType(Merit)
            if (studCard == "Merit")
            {
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    if (chk.Checked == true)
                    {
                        int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    startGen:
                        Label rollno = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_roll_no");
                        if (rollno.Text.Trim() != "")
                        {
                            roll_no = rollno.Text.Trim();
                        }
                        TextBox NoOf_Cards = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_AddOrDel");
                        if (NoOf_Cards.Text.Trim() != "")
                        {
                            NoOfCards = Convert.ToDouble(NoOf_Cards.Text.Trim());
                        }
                        selQry = "SELECT ltrim(str(Course_ID))+'~'+ ltrim(str(Dept_Code)) Degree FROM Registration R,Degree G WHERE R.Degree_Code = G.Degree_Code AND R.Roll_No ='" + roll_no + "' ";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selQry, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            StrDegCode = Convert.ToString(ds.Tables[0].Rows[0]["Degree"]);
                        }
                        if (chk.Checked == true && (NoOfCards) > 0)
                        {
                            selQry = "SELECT * FROM TokenDetails WHERE Roll_No ='" + roll_no + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 2 AND Is_Staff = 0 ";

                            if (library != "All")
                                selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                            else
                                selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                            if (booktype != "All")
                                selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                            else
                                selQry += " AND ISNULL(Book_Type,'All') ='All'";
                            if (CardCategory != "All")
                                selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                            else
                                selQry += " AND ISNULL(CardCat,'All') ='All'";
                            selQry += " ORDER BY LEN(Token_No) DESC,Token_No DESC ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selQry, "Text");

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                {
                                    string tokenNo = Convert.ToString(ds.Tables[0].Rows[j]["Token_No"]);

                                    selQry = "SELECT * FROM Borrow WHERE Roll_No ='" + roll_no + "' AND Token_No ='" + tokenNo + "' AND Return_Flag = 0 AND Is_Staff = 0 ";
                                    dsload.Clear();
                                    dsload = d2.select_method_wo_parameter(selQry, "Text");
                                    if (dsload.Tables[0].Rows.Count == 0)
                                    {
                                        selQry = "SELECT * FROM Borrow WHERE Roll_No ='" + roll_no + "' AND Token_No ='" + tokenNo + "' AND Is_Staff = 0 ";
                                        dsload.Clear();
                                        dsload = d2.select_method_wo_parameter(selQry, "Text");
                                        if (dsload.Tables[0].Rows.Count == 0)
                                        {
                                            DelQry = "DELETE FROM TokenDetails WHERE Roll_No ='" + roll_no + "' AND Token_No ='" + tokenNo + "' AND IndCategory = 2 AND Is_Staff = 0 ";

                                            if (library != "All")
                                                DelQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                            else
                                                DelQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                            if (booktype != "All")
                                                DelQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                            else
                                                DelQry += " AND ISNULL(Book_Type,'All') ='All'";
                                            if (CardCategory != "All")
                                                DelQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                            else
                                                DelQry += " AND ISNULL(CardCat,'All') ='All'";

                                            delete = d2.update_method_wo_parameter(DelQry, "Text");
                                            if (delete == 1)
                                            {
                                                intDeleteCount = intDeleteCount + 1;
                                                intTotDelCard = intTotDelCard + 1;

                                                selQry = "SELECT No_Of_Token FROM Lib_Master WHERE Batch_Year =" + batch + " AND Code ='" + roll_no + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 2 AND Is_Staff = 0 ";
                                                if (library != "All")
                                                    selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                else
                                                    selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                if (booktype != "All")
                                                    selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                else
                                                    selQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                if (CardCategory != "All")
                                                    selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                else
                                                    selQry += " AND ISNULL(CardCat,'All') ='All'";
                                                ds.Clear();
                                                ds = d2.select_method_wo_parameter(selQry, "Text");
                                                if (ds.Tables[0].Rows.Count > 0)
                                                {
                                                    int NoOfToken = Convert.ToInt32(ds.Tables[0].Rows[0]["No_Of_Token"]);
                                                    if (NoOfToken > 0)
                                                    {

                                                        updateQry = "UPDATE Lib_Master SET No_Of_Token = No_Of_Token - 1 WHERE  Batch_Year =" + batch + " AND Code ='" + roll_no + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 2 AND Is_Staff = 0 ";

                                                        if (library != "All")
                                                            updateQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                        else
                                                            updateQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                        if (booktype != "All")
                                                            updateQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                        else
                                                            updateQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                        if (CardCategory != "All")
                                                            updateQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                        else
                                                            updateQry += " AND ISNULL(CardCat,'All') ='All'";
                                                        update = d2.update_method_wo_parameter(updateQry, "Text");

                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            selQry = "SELECT Token_No FROM TokenDetails WHERE Is_Locked = 0 AND Roll_No ='" + roll_no + "' AND Token_No <> '" + tokenNo + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 2 AND Is_Staff = 0 ";
                                            if (library != "All")
                                                selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                            else
                                                selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                            if (booktype != "All")
                                                selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                            else
                                                selQry += " AND ISNULL(Book_Type,'All') ='All'";
                                            if (CardCategory != "All")
                                                selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                            else
                                                selQry += " AND ISNULL(CardCat,'All') ='All'";
                                            dsload.Clear();
                                            dsload = d2.select_method_wo_parameter(selQry, "Text");
                                            if (dsload.Tables[0].Rows.Count > 0)
                                            {

                                                updateQry = "UPDATE Borrow SET Token_No ='" + tokenNo + "' WHERE Roll_No ='" + roll_no + "' AND Token_No ='" + tokenNo + "' AND Is_Staff = 0 ";
                                                update = d2.update_method_wo_parameter(updateQry, "Text");
                                                DelQry = "DELETE FROM TokenDetails WHERE Roll_No ='" + roll_no + "' AND Token_No ='" + tokenNo + "' AND IndCategory = 2 AND Is_Staff = 0 ";
                                                if (library != "All")
                                                    DelQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                else
                                                    DelQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                if (booktype != "All")
                                                    DelQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                else
                                                    DelQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                if (CardCategory != "All")
                                                    DelQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                else
                                                    DelQry += " AND ISNULL(CardCat,'All') ='All'";

                                                delete = d2.update_method_wo_parameter(DelQry, "Text");
                                                if (delete == 1)
                                                {
                                                    intDeleteCount = intDeleteCount + 1;
                                                    intTotDelCard = intTotDelCard + 1;

                                                    selQry = "SELECT No_Of_Token FROM Lib_Master WHERE Batch_Year =" + batch + " AND Code ='" + roll_no + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 2 AND Is_Staff = 0 ";
                                                    if (library != "All")
                                                        selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                    else
                                                        selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                    if (booktype != "All")
                                                        selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                    else
                                                        selQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                    if (CardCategory != "All")
                                                        selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                    else
                                                        selQry += " AND ISNULL(CardCat,'All') ='All'";
                                                    ds.Clear();
                                                    ds = d2.select_method_wo_parameter(selQry, "Text");
                                                    if (ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        int NoOfToken = Convert.ToInt32(ds.Tables[0].Rows[0]["No_Of_Token"]);
                                                        if (NoOfToken > 0)
                                                        {

                                                            updateQry = "UPDATE Lib_Master SET No_Of_Token = No_Of_Token - 1 WHERE  Batch_Year =" + batch + " AND Code ='" + roll_no + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 2 AND Is_Staff = 0 ";

                                                            if (library != "All")
                                                                updateQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                            else
                                                                updateQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                            if (booktype != "All")
                                                                updateQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                            else
                                                                updateQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                            if (CardCategory != "All")
                                                                updateQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                            else
                                                                updateQry += " AND ISNULL(CardCat,'All') ='All'";
                                                            update = d2.update_method_wo_parameter(updateQry, "Text");

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (NoOfCards == intDeleteCount && RowCnt < grdindividual.Rows.Count)
                                    {
                                        RowCnt = RowCnt + 1;
                                        goto startGen;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region Delete for CardType(Book Bank)
            if (studCard == "Book Bank")
            {
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    if (chk.Checked == true)
                    {
                        int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    startGen:
                        Label rollno = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_roll_no");
                        if (rollno.Text.Trim() != "")
                        {
                            roll_no = rollno.Text.Trim();
                        }
                        TextBox NoOf_Cards = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_AddOrDel");
                        if (NoOf_Cards.Text.Trim() != "")
                        {
                            NoOfCards = Convert.ToDouble(NoOf_Cards.Text.Trim());
                        }
                        selQry = "SELECT ltrim(str(Course_ID))+'~'+ ltrim(str(Dept_Code)) Degree FROM Registration R,Degree G WHERE R.Degree_Code = G.Degree_Code AND R.Roll_No ='" + roll_no + "' ";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selQry, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            StrDegCode = Convert.ToString(ds.Tables[0].Rows[0]["Degree"]);
                        }
                        if (chk.Checked == true && (NoOfCards) > 0)
                        {
                            selQry = "SELECT * FROM TokenDetails WHERE Roll_No ='" + roll_no + "' AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = '" + category + "'  AND Is_Staff = 0 ";

                            if (library != "All")
                                selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                            else
                                selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                            if (booktype != "All")
                                selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                            else
                                selQry += " AND ISNULL(Book_Type,'All') ='All'";
                            if (CardCategory != "All")
                                selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                            else
                                selQry += " AND ISNULL(CardCat,'All') ='All'";
                            selQry += " ORDER BY LEN(Token_No) DESC,Token_No DESC ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selQry, "Text");

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                {
                                    string tokenNo = Convert.ToString(ds.Tables[0].Rows[j]["Token_No"]);

                                    selQry = "SELECT * FROM Borrow WHERE Roll_No ='" + roll_no + "' AND Token_No ='" + tokenNo + "' AND Return_Flag = 0 AND Is_Staff = 0 ";
                                    dsload.Clear();
                                    dsload = d2.select_method_wo_parameter(selQry, "Text");
                                    if (dsload.Tables[0].Rows.Count == 0)
                                    {
                                        selQry = "SELECT * FROM Borrow WHERE Roll_No ='" + roll_no + "' AND Token_No ='" + tokenNo + "' AND Is_Staff = 0 ";
                                        dsload.Clear();
                                        dsload = d2.select_method_wo_parameter(selQry, "Text");
                                        if (dsload.Tables[0].Rows.Count == 0)
                                        {
                                            DelQry = "DELETE FROM TokenDetails WHERE Roll_No ='" + roll_no + "' AND Token_No ='" + tokenNo + "' AND ISNULL(Category,'All') = 'Book Bank' AND Is_Staff = 0 AND ISNULL(StudCategory,'All') = '" + category + "' ";

                                            if (library != "All")
                                                DelQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                            else
                                                DelQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                            if (booktype != "All")
                                                DelQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                            else
                                                DelQry += " AND ISNULL(Book_Type,'All') ='All'";
                                            if (CardCategory != "All")
                                                DelQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                            else
                                                DelQry += " AND ISNULL(CardCat,'All') ='All'";

                                            delete = d2.update_method_wo_parameter(DelQry, "Text");
                                            if (delete == 1)
                                            {
                                                intDeleteCount = intDeleteCount + 1;
                                                intTotDelCard = intTotDelCard + 1;

                                                selQry = "SELECT No_Of_Token FROM Lib_Master WHERE Batch_Year =" + batch + " AND Code ='" + roll_no + "' AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = '" + category + "'  AND Is_Staff = 0 ";
                                                if (library != "All")
                                                    selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                else
                                                    selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                if (booktype != "All")
                                                    selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                else
                                                    selQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                if (CardCategory != "All")
                                                    selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                else
                                                    selQry += " AND ISNULL(CardCat,'All') ='All'";
                                                ds.Clear();
                                                ds = d2.select_method_wo_parameter(selQry, "Text");
                                                if (ds.Tables[0].Rows.Count > 0)
                                                {
                                                    int NoOfToken = Convert.ToInt32(ds.Tables[0].Rows[0]["No_Of_Token"]);
                                                    if (NoOfToken > 0)
                                                    {

                                                        updateQry = "UPDATE Lib_Master SET No_Of_Token = No_Of_Token - 1 WHERE  Batch_Year =" + batch + " AND Code ='" + roll_no + "' AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = '" + category + "' AND Is_Staff = 0 ";

                                                        if (library != "All")
                                                            updateQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                        else
                                                            updateQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                        if (booktype != "All")
                                                            updateQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                        else
                                                            updateQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                        if (CardCategory != "All")
                                                            updateQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                        else
                                                            updateQry += " AND ISNULL(CardCat,'All') ='All'";
                                                        update = d2.update_method_wo_parameter(updateQry, "Text");

                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            selQry = "SELECT Token_No FROM TokenDetails WHERE Is_Locked = 0 AND Roll_No ='" + roll_no + "' AND Token_No <> '" + tokenNo + "' AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = '" + category + "' AND Is_Staff = 0 ";
                                            if (library != "All")
                                                selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                            else
                                                selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                            if (booktype != "All")
                                                selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                            else
                                                selQry += " AND ISNULL(Book_Type,'All') ='All'";
                                            if (CardCategory != "All")
                                                selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                            else
                                                selQry += " AND ISNULL(CardCat,'All') ='All'";
                                            dsload.Clear();
                                            dsload = d2.select_method_wo_parameter(selQry, "Text");
                                            if (dsload.Tables[0].Rows.Count > 0)
                                            {

                                                updateQry = "UPDATE Borrow SET Token_No ='" + tokenNo + "' WHERE Roll_No ='" + roll_no + "' AND Token_No ='" + tokenNo + "' AND Is_Staff = 0 ";
                                                update = d2.update_method_wo_parameter(updateQry, "Text");
                                                DelQry = "DELETE FROM TokenDetails WHERE Roll_No ='" + roll_no + "' AND Token_No ='" + tokenNo + "'  AND ISNULL(Category,'All') = 'Book Bank' AND Is_Staff = 0 AND ISNULL(StudCategory,'All') = '" + category + "'";
                                                if (library != "All")
                                                    DelQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                else
                                                    DelQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                if (booktype != "All")
                                                    DelQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                else
                                                    DelQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                if (CardCategory != "All")
                                                    DelQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                else
                                                    DelQry += " AND ISNULL(CardCat,'All') ='All'";

                                                delete = d2.update_method_wo_parameter(DelQry, "Text");
                                                if (delete == 1)
                                                {
                                                    intDeleteCount = intDeleteCount + 1;
                                                    intTotDelCard = intTotDelCard + 1;

                                                    selQry = "SELECT No_Of_Token FROM Lib_Master WHERE Batch_Year =" + batch + " AND Code ='" + roll_no + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 2 AND Is_Staff = 0 ";
                                                    if (library != "All")
                                                        selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                    else
                                                        selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                    if (booktype != "All")
                                                        selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                    else
                                                        selQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                    if (CardCategory != "All")
                                                        selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                    else
                                                        selQry += " AND ISNULL(CardCat,'All') ='All'";
                                                    ds.Clear();
                                                    ds = d2.select_method_wo_parameter(selQry, "Text");
                                                    if (ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        int NoOfToken = Convert.ToInt32(ds.Tables[0].Rows[0]["No_Of_Token"]);
                                                        if (NoOfToken > 0)
                                                        {
                                                            updateQry = "UPDATE Lib_Master SET No_Of_Token = No_Of_Token - 1 WHERE  Batch_Year =" + batch + " AND Code ='" + roll_no + "' AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = '" + category + "' AND Is_Staff = 0 ";

                                                            if (library != "All")
                                                                updateQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                            else
                                                                updateQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                            if (booktype != "All")
                                                                updateQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                            else
                                                                updateQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                            if (CardCategory != "All")
                                                                updateQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                            else
                                                                updateQry += " AND ISNULL(CardCat,'All') ='All'";
                                                            update = d2.update_method_wo_parameter(updateQry, "Text");

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (NoOfCards == intDeleteCount && RowCnt < grdindividual.Rows.Count)
                                    {
                                        RowCnt = RowCnt + 1;
                                        goto startGen;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            if (intTotDelCard > 0)
            {
                sureDivDel.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Text = "Cards are deleted for the selected students";
                btngo_Click(sender, e);
            }
            else
            {
                sureDivDel.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Text = "Cards are not deleted";
            }


        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "individualstudent");
        }
    }

    protected void StaffCardDel(object sender, EventArgs e)
    {
        try
        {
            string booktype = Convert.ToString(ddlbooktype.SelectedValue);
            string library = Convert.ToString(ddlLibrary.SelectedValue);
            string CardCategory = Convert.ToString(ddl_CardCatogery.SelectedValue);
            string college_Code = Convert.ToString(ddl_collegename.SelectedItem.Value);
            string StaffCat = Convert.ToString(ddlcard.SelectedValue);
            string StaffCardCat = Convert.ToString(ddlcategory.SelectedValue);
            string StrBookType = string.Empty;
            string StrTransLibCode = string.Empty;
            string StrCardCat = string.Empty;
            double NoOfCards = 0;
            string staffCode = string.Empty;
            string staffName = string.Empty;
            string FineType = string.Empty;
            string StrDepDesc = string.Empty;
            string selQry = string.Empty;
            string MaxCard = string.Empty;
            string StrTokNo = string.Empty;
            string insertQry = string.Empty;
            string UpdateQry = string.Empty;
            string DelQry = string.Empty;
            int delete = 0;
            int update = 0;

            int intDeleteCount = 0;
            int intTotDelCard = 0;

            if (booktype == "Book")
                StrBookType = "BOK";
            if (booktype == "Periodicals")
                StrBookType = "PER";
            if (booktype == "Project Book")
                StrBookType = "PRO";
            if (booktype == "Non-Book Material")
                StrBookType = "NBM";
            if (booktype == "Question Bank")
                StrBookType = "QBA";
            if (booktype == "Back Volume")
                StrBookType = "BVO";
            if (booktype == "Reference Volume")
                StrBookType = "REF";
            if (booktype == "All")
                StrBookType = "All";

            if (library != "All")
                StrTransLibCode = library;
            else
                StrTransLibCode = "All";

            if (CardCategory != "All")
                StrCardCat = CardCategory;
            else
                StrCardCat = "All";

            #region delete for CardType(general)

            if (StaffCat == "General")
            {
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    if (chk.Checked == true)
                    {
                        int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    startGen:
                        Label rollno = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_roll_no");
                        if (rollno.Text.Trim() != "")
                        {
                            staffCode = rollno.Text.Trim();
                        }
                        TextBox NoOf_Cards = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_AddOrDel");
                        if (NoOf_Cards.Text.Trim() != "")
                        {
                            NoOfCards = Convert.ToDouble(NoOf_Cards.Text.Trim());
                        }
                        if (chk.Checked == true && (NoOfCards) > 0)
                        {
                            selQry = "SELECT * FROM TokenDetails WHERE Roll_No ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 0 AND Is_Staff = 1 ";

                            if (library != "All")
                                selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                            else
                                selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                            if (booktype != "All")
                                selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                            else
                                selQry += " AND ISNULL(Book_Type,'All') ='All'";
                            if (CardCategory != "All")
                                selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                            else
                                selQry += " AND ISNULL(CardCat,'All') ='All'";
                            selQry += " ORDER BY LEN(Token_No) DESC,Token_No DESC ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selQry, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                {
                                    string tokenNo = Convert.ToString(ds.Tables[0].Rows[j]["Token_No"]);

                                    selQry = "SELECT * FROM Borrow WHERE Roll_No ='" + staffCode + "' AND Token_No ='" + tokenNo + "' AND Return_Flag = 0 AND Is_Staff = 1 ";
                                    dsload.Clear();
                                    dsload = d2.select_method_wo_parameter(selQry, "Text");
                                    if (dsload.Tables[0].Rows.Count == 0)
                                    {
                                        selQry = "SELECT * FROM Borrow WHERE Roll_No ='" + staffCode + "' AND Token_No ='" + tokenNo + "' AND Is_Staff = 1 ";
                                        dsload.Clear();
                                        dsload = d2.select_method_wo_parameter(selQry, "Text");
                                        if (dsload.Tables[0].Rows.Count == 0)
                                        {
                                            DelQry = "DELETE FROM TokenDetails WHERE Roll_No ='" + staffCode + "' AND Token_No ='" + tokenNo + "' AND IndCategory = 0 AND Is_Staff = 1 ";

                                            if (library != "All")
                                                DelQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                            else
                                                DelQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                            if (booktype != "All")
                                                DelQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                            else
                                                DelQry += " AND ISNULL(Book_Type,'All') ='All'";
                                            if (CardCategory != "All")
                                                DelQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                            else
                                                DelQry += " AND ISNULL(CardCat,'All') ='All'";

                                            delete = d2.update_method_wo_parameter(DelQry, "Text");
                                            if (delete == 1)
                                            {
                                                intDeleteCount = intDeleteCount + 1;
                                                intTotDelCard = intTotDelCard + 1;

                                                selQry = "SELECT No_Of_Token FROM Lib_Master WHERE Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 0 AND Is_Staff = 1 ";
                                                if (library != "All")
                                                    selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                else
                                                    selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                if (booktype != "All")
                                                    selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                else
                                                    selQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                if (CardCategory != "All")
                                                    selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                else
                                                    selQry += " AND ISNULL(CardCat,'All') ='All'";
                                                dsCommon.Clear();
                                                dsCommon = d2.select_method_wo_parameter(selQry, "Text");
                                                if (dsCommon.Tables[0].Rows.Count > 0)
                                                {
                                                    int NoOfToken = Convert.ToInt32(dsCommon.Tables[0].Rows[0]["No_Of_Token"]);
                                                    if (NoOfToken > 0)
                                                    {
                                                        UpdateQry = "UPDATE Lib_Master SET No_Of_Token = No_Of_Token - 1 WHERE  Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 0 AND Is_Staff = 1 ";

                                                        if (library != "All")
                                                            UpdateQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                        else
                                                            UpdateQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                        if (booktype != "All")
                                                            UpdateQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                        else
                                                            UpdateQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                        if (CardCategory != "All")
                                                            UpdateQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                        else
                                                            UpdateQry += " AND ISNULL(CardCat,'All') ='All'";
                                                        update = d2.update_method_wo_parameter(UpdateQry, "Text");

                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            selQry = "SELECT Token_No FROM TokenDetails WHERE Is_Locked = 0 AND Roll_No ='" + staffCode + "' AND Token_No <> '" + tokenNo + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 0 AND Is_Staff = 1 ";
                                            if (library != "All")
                                                selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                            else
                                                selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                            if (booktype != "All")
                                                selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                            else
                                                selQry += " AND ISNULL(Book_Type,'All') ='All'";
                                            if (CardCategory != "All")
                                                selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                            else
                                                selQry += " AND ISNULL(CardCat,'All') ='All'";
                                            dsCommon.Clear();
                                            dsCommon = d2.select_method_wo_parameter(selQry, "Text");
                                            if (dsCommon.Tables[0].Rows.Count > 0)
                                            {
                                                UpdateQry = "UPDATE Borrow SET Token_No ='" + tokenNo + "' WHERE Roll_No ='" + staffCode + "' AND Token_No ='" + tokenNo + "' AND Is_Staff = 1 ";
                                                update = d2.update_method_wo_parameter(UpdateQry, "Text");
                                                DelQry = "DELETE FROM TokenDetails WHERE Roll_No ='" + staffCode + "' AND Token_No ='" + tokenNo + "' AND IndCategory = 0 AND Is_Staff = 1 ";
                                                if (library != "All")
                                                    DelQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                else
                                                    DelQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                if (booktype != "All")
                                                    DelQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                else
                                                    DelQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                if (CardCategory != "All")
                                                    DelQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                else
                                                    DelQry += " AND ISNULL(CardCat,'All') ='All'";

                                                delete = d2.update_method_wo_parameter(DelQry, "Text");
                                                if (delete == 1)
                                                {
                                                    intDeleteCount = intDeleteCount + 1;
                                                    intTotDelCard = intTotDelCard + 1;

                                                    selQry = "SELECT No_Of_Token FROM Lib_Master WHERE Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 0 AND Is_Staff = 1 ";
                                                    if (library != "All")
                                                        selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                    else
                                                        selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                    if (booktype != "All")
                                                        selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                    else
                                                        selQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                    if (CardCategory != "All")
                                                        selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                    else
                                                        selQry += " AND ISNULL(CardCat,'All') ='All'";
                                                    dsStaff.Clear();
                                                    dsStaff = d2.select_method_wo_parameter(selQry, "Text");
                                                    if (dsStaff.Tables[0].Rows.Count > 0)
                                                    {
                                                        int NoOfToken = Convert.ToInt32(dsStaff.Tables[0].Rows[0]["No_Of_Token"]);
                                                        if (NoOfToken > 0)
                                                        {

                                                            UpdateQry = "UPDATE Lib_Master SET No_Of_Token = No_Of_Token - 1 WHERE  Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 0 AND Is_Staff = 1 ";
                                                            if (library != "All")
                                                                UpdateQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                            else
                                                                UpdateQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                            if (booktype != "All")
                                                                UpdateQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                            else
                                                                UpdateQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                            if (CardCategory != "All")
                                                                UpdateQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                            else
                                                                UpdateQry += " AND ISNULL(CardCat,'All') ='All'";
                                                            update = d2.update_method_wo_parameter(UpdateQry, "Text");

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (NoOfCards == intDeleteCount && RowCnt < grdindividual.Rows.Count)
                                    {
                                        RowCnt = RowCnt + 1;
                                        goto startGen;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region delete for CardType(Individual)

            if (StaffCat == "Individual")
            {
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    if (chk.Checked == true)
                    {
                        int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    startGen:
                        Label rollno = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_roll_no");
                        if (rollno.Text.Trim() != "")
                        {
                            staffCode = rollno.Text.Trim();
                        }
                        TextBox NoOf_Cards = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_AddOrDel");
                        if (NoOf_Cards.Text.Trim() != "")
                        {
                            NoOfCards = Convert.ToDouble(NoOf_Cards.Text.Trim());
                        }
                        if (chk.Checked == true && (NoOfCards) > 0)
                        {
                            selQry = "SELECT * FROM TokenDetails WHERE Roll_No ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 1 AND Is_Staff = 1 ";

                            if (library != "All")
                                selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                            else
                                selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                            if (booktype != "All")
                                selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                            else
                                selQry += " AND ISNULL(Book_Type,'All') ='All'";
                            if (CardCategory != "All")
                                selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                            else
                                selQry += " AND ISNULL(CardCat,'All') ='All'";
                            selQry += " ORDER BY LEN(Token_No) DESC,Token_No DESC ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selQry, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                {
                                    string tokenNo = Convert.ToString(ds.Tables[0].Rows[j]["Token_No"]);

                                    selQry = "SELECT * FROM Borrow WHERE Roll_No ='" + staffCode + "' AND Token_No ='" + tokenNo + "' AND Return_Flag = 0 AND Is_Staff = 1 ";
                                    dsload.Clear();
                                    dsload = d2.select_method_wo_parameter(selQry, "Text");
                                    if (dsload.Tables[0].Rows.Count == 0)
                                    {
                                        selQry = "SELECT * FROM Borrow WHERE Roll_No ='" + staffCode + "' AND Token_No ='" + tokenNo + "' AND Is_Staff = 1 ";
                                        dsCommon.Clear();
                                        dsCommon = d2.select_method_wo_parameter(selQry, "Text");
                                        if (dsCommon.Tables[0].Rows.Count == 0)
                                        {
                                            DelQry = "DELETE FROM TokenDetails WHERE Roll_No ='" + staffCode + "' AND Token_No ='" + tokenNo + "' AND IndCategory = 1 AND Is_Staff = 1 ";
                                            if (library != "All")
                                                DelQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                            else
                                                DelQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                            if (booktype != "All")
                                                DelQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                            else
                                                DelQry += " AND ISNULL(Book_Type,'All') ='All'";
                                            if (CardCategory != "All")
                                                DelQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                            else
                                                DelQry += " AND ISNULL(CardCat,'All') ='All'";

                                            delete = d2.update_method_wo_parameter(DelQry, "Text");
                                            if (delete == 1)
                                            {
                                                intDeleteCount = intDeleteCount + 1;
                                                intTotDelCard = intTotDelCard + 1;

                                                selQry = "SELECT No_Of_Token FROM Lib_Master WHERE Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 1 AND Is_Staff = 1 ";
                                                if (library != "All")
                                                    selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                else
                                                    selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                if (booktype != "All")
                                                    selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                else
                                                    selQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                if (CardCategory != "All")
                                                    selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                else
                                                    selQry += " AND ISNULL(CardCat,'All') ='All'";
                                                dsStaff.Clear();
                                                dsStaff = d2.select_method_wo_parameter(selQry, "Text");
                                                if (dsStaff.Tables[0].Rows.Count > 0)
                                                {
                                                    int NoOfToken = Convert.ToInt32(dsStaff.Tables[0].Rows[0]["No_Of_Token"]);
                                                    if (NoOfToken > 0)
                                                    {
                                                        UpdateQry = "UPDATE Lib_Master SET No_Of_Token = No_Of_Token - 1 WHERE  Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 1 AND Is_Staff = 1 ";

                                                        if (library != "All")
                                                            UpdateQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                        else
                                                            UpdateQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                        if (booktype != "All")
                                                            UpdateQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                        else
                                                            UpdateQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                        if (CardCategory != "All")
                                                            UpdateQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                        else
                                                            UpdateQry += " AND ISNULL(CardCat,'All') ='All'";
                                                        update = d2.update_method_wo_parameter(UpdateQry, "Text");
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            selQry = "SELECT Token_No FROM TokenDetails WHERE Is_Locked = 0 AND Roll_No ='" + staffCode + "' AND Token_No <> '" + tokenNo + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 1 AND Is_Staff = 1 ";
                                            if (library != "All")
                                                selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                            else
                                                selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                            if (booktype != "All")
                                                selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                            else
                                                selQry += " AND ISNULL(Book_Type,'All') ='All'";
                                            if (CardCategory != "All")
                                                selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                            else
                                                selQry += " AND ISNULL(CardCat,'All') ='All'";
                                            dsStaff.Clear();
                                            dsStaff = d2.select_method_wo_parameter(selQry, "Text");
                                            if (dsStaff.Tables[0].Rows.Count > 0)
                                            {
                                                UpdateQry = "UPDATE Borrow SET Token_No ='" + tokenNo + "' WHERE Roll_No ='" + staffCode + "' AND Token_No ='" + tokenNo + "' AND Is_Staff = 1 ";
                                                update = d2.update_method_wo_parameter(UpdateQry, "Text");
                                                DelQry = "DELETE FROM TokenDetails WHERE Roll_No ='" + staffCode + "' AND Token_No ='" + tokenNo + "' AND IndCategory = 1 AND Is_Staff = 1 ";
                                                if (library != "All")
                                                    DelQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                else
                                                    DelQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                if (booktype != "All")
                                                    DelQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                else
                                                    DelQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                if (CardCategory != "All")
                                                    DelQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                else
                                                    DelQry += " AND ISNULL(CardCat,'All') ='All'";

                                                delete = d2.update_method_wo_parameter(DelQry, "Text");
                                                if (delete == 1)
                                                {
                                                    intDeleteCount = intDeleteCount + 1;
                                                    intTotDelCard = intTotDelCard + 1;

                                                    selQry = "SELECT No_Of_Token FROM Lib_Master WHERE Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 1 AND Is_Staff = 1 ";
                                                    if (library != "All")
                                                        selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                    else
                                                        selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                    if (booktype != "All")
                                                        selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                    else
                                                        selQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                    if (CardCategory != "All")
                                                        selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                    else
                                                        selQry += " AND ISNULL(CardCat,'All') ='All'";
                                                    dsStaffdel.Clear();
                                                    dsStaffdel = d2.select_method_wo_parameter(selQry, "Text");
                                                    if (dsStaffdel.Tables[0].Rows.Count > 0)
                                                    {
                                                        int NoOfToken = Convert.ToInt32(dsStaffdel.Tables[0].Rows[0]["No_Of_Token"]);
                                                        if (NoOfToken > 0)
                                                        {
                                                            UpdateQry = "UPDATE Lib_Master SET No_Of_Token = No_Of_Token - 1 WHERE  Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 1 AND Is_Staff = 1 ";
                                                            if (library != "All")
                                                                UpdateQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                            else
                                                                UpdateQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                            if (booktype != "All")
                                                                UpdateQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                            else
                                                                UpdateQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                            if (CardCategory != "All")
                                                                UpdateQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                            else
                                                                UpdateQry += " AND ISNULL(CardCat,'All') ='All'";
                                                            update = d2.update_method_wo_parameter(UpdateQry, "Text");

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (NoOfCards == intDeleteCount && RowCnt < grdindividual.Rows.Count)
                                    {
                                        RowCnt = RowCnt + 1;
                                        goto startGen;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region delete for CardType(Merit)

            if (StaffCat == "Merit")
            {
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    if (chk.Checked == true)
                    {
                        int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    startGen:
                        Label rollno = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_roll_no");
                        if (rollno.Text.Trim() != "")
                        {
                            staffCode = rollno.Text.Trim();
                        }
                        TextBox NoOf_Cards = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_AddOrDel");
                        if (NoOf_Cards.Text.Trim() != "")
                        {
                            NoOfCards = Convert.ToDouble(NoOf_Cards.Text.Trim());
                        }
                        if (chk.Checked == true && (NoOfCards) > 0)
                        {
                            selQry = "SELECT * FROM TokenDetails WHERE Roll_No ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 2 AND Is_Staff = 1 ";

                            if (library != "All")
                                selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                            else
                                selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                            if (booktype != "All")
                                selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                            else
                                selQry += " AND ISNULL(Book_Type,'All') ='All'";
                            if (CardCategory != "All")
                                selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                            else
                                selQry += " AND ISNULL(CardCat,'All') ='All'";
                            selQry += " ORDER BY LEN(Token_No) DESC,Token_No DESC ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selQry, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                {
                                    string tokenNo = Convert.ToString(ds.Tables[0].Rows[j]["Token_No"]);

                                    selQry = "SELECT * FROM Borrow WHERE Roll_No ='" + staffCode + "' AND Token_No ='" + tokenNo + "' AND Return_Flag = 0 AND Is_Staff = 1 ";
                                    dsload.Clear();
                                    dsload = d2.select_method_wo_parameter(selQry, "Text");
                                    if (dsload.Tables[0].Rows.Count == 0)
                                    {
                                        selQry = "SELECT * FROM Borrow WHERE Roll_No ='" + staffCode + "' AND Token_No ='" + tokenNo + "' AND Is_Staff = 1 ";
                                        dsCommon.Clear();
                                        dsCommon = d2.select_method_wo_parameter(selQry, "Text");
                                        if (dsCommon.Tables[0].Rows.Count == 0)
                                        {
                                            DelQry = "DELETE FROM TokenDetails WHERE Roll_No ='" + staffCode + "' AND Token_No ='" + tokenNo + "' AND IndCategory = 2 AND Is_Staff = 1 ";
                                            if (library != "All")
                                                DelQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                            else
                                                DelQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                            if (booktype != "All")
                                                DelQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                            else
                                                DelQry += " AND ISNULL(Book_Type,'All') ='All'";
                                            if (CardCategory != "All")
                                                DelQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                            else
                                                DelQry += " AND ISNULL(CardCat,'All') ='All'";
                                            delete = d2.update_method_wo_parameter(DelQry, "Text");
                                            if (delete == 1)
                                            {
                                                intDeleteCount = intDeleteCount + 1;
                                                intTotDelCard = intTotDelCard + 1;

                                                selQry = "SELECT No_Of_Token FROM Lib_Master WHERE Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 2 AND Is_Staff = 1 ";
                                                if (library != "All")
                                                    selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                else
                                                    selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                if (booktype != "All")
                                                    selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                else
                                                    selQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                if (CardCategory != "All")
                                                    selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                else
                                                    selQry += " AND ISNULL(CardCat,'All') ='All'";
                                                dsStaff.Clear();
                                                dsStaff = d2.select_method_wo_parameter(selQry, "Text");
                                                if (dsStaff.Tables[0].Rows.Count > 0)
                                                {
                                                    int NoOfToken = Convert.ToInt32(dsStaff.Tables[0].Rows[0]["No_Of_Token"]);
                                                    if (NoOfToken > 0)
                                                    {
                                                        UpdateQry = "UPDATE Lib_Master SET No_Of_Token = No_Of_Token - 1 WHERE  Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 2 AND Is_Staff = 1 ";

                                                        if (library != "All")
                                                            UpdateQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                        else
                                                            UpdateQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                        if (booktype != "All")
                                                            UpdateQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                        else
                                                            UpdateQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                        if (CardCategory != "All")
                                                            UpdateQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                        else
                                                            UpdateQry += " AND ISNULL(CardCat,'All') ='All'";
                                                        update = d2.update_method_wo_parameter(UpdateQry, "Text");
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            selQry = "SELECT Token_No FROM TokenDetails WHERE Is_Locked = 0 AND Roll_No ='" + staffCode + "' AND Token_No <> '" + tokenNo + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 2 AND Is_Staff = 1 ";
                                            if (library != "All")
                                                selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                            else
                                                selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                            if (booktype != "All")
                                                selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                            else
                                                selQry += " AND ISNULL(Book_Type,'All') ='All'";
                                            if (CardCategory != "All")
                                                selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                            else
                                                selQry += " AND ISNULL(CardCat,'All') ='All'";
                                            dsStaff.Clear();
                                            dsStaff = d2.select_method_wo_parameter(selQry, "Text");
                                            if (dsStaff.Tables[0].Rows.Count > 0)
                                            {
                                                UpdateQry = "UPDATE Borrow SET Token_No ='" + tokenNo + "' WHERE Roll_No ='" + staffCode + "' AND Token_No ='" + tokenNo + "' AND Is_Staff = 1 ";
                                                update = d2.update_method_wo_parameter(UpdateQry, "Text");
                                                DelQry = "DELETE FROM TokenDetails WHERE Roll_No ='" + staffCode + "' AND Token_No ='" + tokenNo + "' AND IndCategory = 2 AND Is_Staff = 1 ";
                                                if (library != "All")
                                                    DelQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                else
                                                    DelQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                if (booktype != "All")
                                                    DelQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                else
                                                    DelQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                if (CardCategory != "All")
                                                    DelQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                else
                                                    DelQry += " AND ISNULL(CardCat,'All') ='All'";

                                                delete = d2.update_method_wo_parameter(DelQry, "Text");
                                                if (delete == 1)
                                                {
                                                    intDeleteCount = intDeleteCount + 1;
                                                    intTotDelCard = intTotDelCard + 1;

                                                    selQry = "SELECT No_Of_Token FROM Lib_Master WHERE Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 2 AND Is_Staff = 1 ";
                                                    if (library != "All")
                                                        selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                    else
                                                        selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                    if (booktype != "All")
                                                        selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                    else
                                                        selQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                    if (CardCategory != "All")
                                                        selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                    else
                                                        selQry += " AND ISNULL(CardCat,'All') ='All'";
                                                    dsStaffdel.Clear();
                                                    dsStaffdel = d2.select_method_wo_parameter(selQry, "Text");
                                                    if (dsStaffdel.Tables[0].Rows.Count > 0)
                                                    {
                                                        int NoOfToken = Convert.ToInt32(dsStaffdel.Tables[0].Rows[0]["No_Of_Token"]);
                                                        if (NoOfToken > 0)
                                                        {

                                                            UpdateQry = "UPDATE Lib_Master SET No_Of_Token = No_Of_Token - 1 WHERE  Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 2 AND Is_Staff = 1 ";
                                                            if (library != "All")
                                                                UpdateQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                            else
                                                                UpdateQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                            if (booktype != "All")
                                                                UpdateQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                            else
                                                                UpdateQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                            if (CardCategory != "All")
                                                                UpdateQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                            else
                                                                UpdateQry += " AND ISNULL(CardCat,'All') ='All'";
                                                            update = d2.update_method_wo_parameter(UpdateQry, "Text");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (NoOfCards == intDeleteCount && RowCnt < grdindividual.Rows.Count)
                                    {
                                        RowCnt = RowCnt + 1;
                                        goto startGen;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region delete for CardType(Merit)

            if (StaffCat == "Merit")
            {
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    if (chk.Checked == true)
                    {
                        int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    startGen:
                        Label rollno = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_roll_no");
                        if (rollno.Text.Trim() != "")
                        {
                            staffCode = rollno.Text.Trim();
                        }
                        TextBox NoOf_Cards = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_AddOrDel");
                        if (NoOf_Cards.Text.Trim() != "")
                        {
                            NoOfCards = Convert.ToDouble(NoOf_Cards.Text.Trim());
                        }
                        if (chk.Checked == true && (NoOfCards) > 0)
                        {
                            selQry = "SELECT * FROM TokenDetails WHERE Roll_No ='" + staffCode + "' AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = '" + StaffCardCat + "' AND Is_Staff = 1 ";

                            if (library != "All")
                                selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                            else
                                selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                            if (booktype != "All")
                                selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                            else
                                selQry += " AND ISNULL(Book_Type,'All') ='All'";
                            if (CardCategory != "All")
                                selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                            else
                                selQry += " AND ISNULL(CardCat,'All') ='All'";
                            selQry += " ORDER BY LEN(Token_No) DESC,Token_No DESC ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selQry, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                {
                                    string tokenNo = Convert.ToString(ds.Tables[0].Rows[j]["Token_No"]);

                                    selQry = "SELECT * FROM Borrow WHERE Roll_No ='" + staffCode + "' AND Token_No ='" + tokenNo + "' AND Return_Flag = 0 AND Is_Staff = 1 ";
                                    dsload.Clear();
                                    dsload = d2.select_method_wo_parameter(selQry, "Text");
                                    if (dsload.Tables[0].Rows.Count == 0)
                                    {
                                        selQry = "SELECT * FROM Borrow WHERE Roll_No ='" + staffCode + "' AND Token_No ='" + tokenNo + "' AND Is_Staff = 1 ";
                                        dsCommon.Clear();
                                        dsCommon = d2.select_method_wo_parameter(selQry, "Text");
                                        if (dsCommon.Tables[0].Rows.Count == 0)
                                        {
                                            DelQry = "DELETE FROM TokenDetails WHERE Roll_No ='" + staffCode + "' AND Token_No ='" + tokenNo + "' AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = '" + StaffCardCat + "' AND Is_Staff = 1 ";

                                            if (library != "All")
                                                DelQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                            else
                                                DelQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                            if (booktype != "All")
                                                DelQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                            else
                                                DelQry += " AND ISNULL(Book_Type,'All') ='All'";
                                            if (CardCategory != "All")
                                                DelQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                            else
                                                DelQry += " AND ISNULL(CardCat,'All') ='All'";

                                            delete = d2.update_method_wo_parameter(DelQry, "Text");
                                            if (delete == 1)
                                            {
                                                intDeleteCount = intDeleteCount + 1;
                                                intTotDelCard = intTotDelCard + 1;

                                                selQry = "SELECT No_Of_Token FROM Lib_Master WHERE Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = '" + StaffCardCat + "' AND Is_Staff = 1 ";
                                                if (library != "All")
                                                    selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                else
                                                    selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                if (booktype != "All")
                                                    selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                else
                                                    selQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                if (CardCategory != "All")
                                                    selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                else
                                                    selQry += " AND ISNULL(CardCat,'All') ='All'";
                                                dsStaff.Clear();
                                                dsStaff = d2.select_method_wo_parameter(selQry, "Text");
                                                if (dsStaff.Tables[0].Rows.Count > 0)
                                                {
                                                    int NoOfToken = Convert.ToInt32(dsStaff.Tables[0].Rows[0]["No_Of_Token"]);
                                                    if (NoOfToken > 0)
                                                    {
                                                        UpdateQry = "UPDATE Lib_Master SET No_Of_Token = No_Of_Token - 1 WHERE  Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = '" + StaffCardCat + "' AND Is_Staff = 1 ";

                                                        if (library != "All")
                                                            UpdateQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                        else
                                                            UpdateQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                        if (booktype != "All")
                                                            UpdateQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                        else
                                                            UpdateQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                        if (CardCategory != "All")
                                                            UpdateQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                        else
                                                            UpdateQry += " AND ISNULL(CardCat,'All') ='All'";
                                                        update = d2.update_method_wo_parameter(UpdateQry, "Text");

                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            selQry = "SELECT Token_No FROM TokenDetails WHERE Is_Locked = 0 AND Roll_No ='" + staffCode + "' AND Token_No <> '" + tokenNo + "' AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = '" + StaffCardCat + "' AND Is_Staff = 1 ";
                                            if (library != "All")
                                                selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                            else
                                                selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                            if (booktype != "All")
                                                selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                            else
                                                selQry += " AND ISNULL(Book_Type,'All') ='All'";
                                            if (CardCategory != "All")
                                                selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                            else
                                                selQry += " AND ISNULL(CardCat,'All') ='All'";
                                            dsStaff.Clear();
                                            dsStaff = d2.select_method_wo_parameter(selQry, "Text");
                                            if (dsStaff.Tables[0].Rows.Count > 0)
                                            {

                                                UpdateQry = "UPDATE Borrow SET Token_No ='" + tokenNo + "' WHERE Roll_No ='" + staffCode + "' AND Token_No ='" + tokenNo + "' AND Is_Staff = 1 ";
                                                update = d2.update_method_wo_parameter(UpdateQry, "Text");
                                                DelQry = "DELETE FROM TokenDetails WHERE Roll_No ='" + staffCode + "' AND Token_No ='" + tokenNo + "' AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = '" + StaffCardCat + "' AND Is_Staff = 1 ";
                                                if (library != "All")
                                                    DelQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                else
                                                    DelQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                if (booktype != "All")
                                                    DelQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                else
                                                    DelQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                if (CardCategory != "All")
                                                    DelQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                else
                                                    DelQry += " AND ISNULL(CardCat,'All') ='All'";

                                                delete = d2.update_method_wo_parameter(DelQry, "Text");
                                                if (delete == 1)
                                                {
                                                    intDeleteCount = intDeleteCount + 1;
                                                    intTotDelCard = intTotDelCard + 1;

                                                    selQry = "SELECT No_Of_Token FROM Lib_Master WHERE Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = '" + StaffCardCat + "' AND Is_Staff = 1 ";
                                                    if (library != "All")
                                                        selQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                    else
                                                        selQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                    if (booktype != "All")
                                                        selQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                    else
                                                        selQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                    if (CardCategory != "All")
                                                        selQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                    else
                                                        selQry += " AND ISNULL(CardCat,'All') ='All'";
                                                    dsStaffdel.Clear();
                                                    dsStaffdel = d2.select_method_wo_parameter(selQry, "Text");
                                                    if (dsStaffdel.Tables[0].Rows.Count > 0)
                                                    {
                                                        int NoOfToken = Convert.ToInt32(dsStaffdel.Tables[0].Rows[0]["No_Of_Token"]);
                                                        if (NoOfToken > 0)
                                                        {

                                                            UpdateQry = "UPDATE Lib_Master SET No_Of_Token = No_Of_Token - 1 WHERE  Code ='" + staffCode + "' AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = '" + StaffCardCat + "' AND Is_Staff = 1 ";
                                                            if (library != "All")
                                                                UpdateQry += "AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                                            else
                                                                UpdateQry += "AND ISNULL(TransLibCode,'All') ='All'";
                                                            if (booktype != "All")
                                                                UpdateQry += " AND ISNULL(Book_Type,'All') ='" + StrBookType + "'";
                                                            else
                                                                UpdateQry += " AND ISNULL(Book_Type,'All') ='All'";
                                                            if (CardCategory != "All")
                                                                UpdateQry += "AND ISNULL(CardCat,'All') ='" + CardCategory + "'";
                                                            else
                                                                UpdateQry += " AND ISNULL(CardCat,'All') ='All'";
                                                            update = d2.update_method_wo_parameter(UpdateQry, "Text");

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (NoOfCards == intDeleteCount && RowCnt < grdindividual.Rows.Count)
                                    {
                                        RowCnt = RowCnt + 1;
                                        goto startGen;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            if (intTotDelCard > 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Cards are deleted for the selected Staff";
                btngo_Click(sender, e);
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Cards are not deleted";
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "individualstudent");
        }
    }

    protected void btnDelNo_Click(object sender, EventArgs e)
    {
        sureDivDel.Visible = false;
        imgdiv2.Visible = true;
        lbl_alert.Text = "Cards are not Deleted";
    }

    #endregion

    protected void btnUpdateRenew_Click(object sender, EventArgs e)
    {
        try
        {
            if (rblstaff.SelectedIndex == 0)
            {
                double RenewDays = 0;
                double intcount = 0;
                int IntSel = 0;
                string updateQry = string.Empty;
                int update = 0;
                string roll_No = string.Empty;
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    TextBox Renew_Days = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_renewdays");
                    if (Renew_Days.Text.Trim() != "")
                    {
                        RenewDays = Convert.ToDouble(Renew_Days.Text.Trim());
                    }
                    if (chk.Checked == true)
                    {
                        intcount = intcount + RenewDays;
                        IntSel = IntSel + 1;
                    }
                }
                if (IntSel == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Select the student";

                }
                if (intcount == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Enter number of renewal times";
                }
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    TextBox Renew_Days = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_renewdays");
                    if (Renew_Days.Text.Trim() != "")
                    {
                        RenewDays = Convert.ToDouble(Renew_Days.Text.Trim());
                    }
                    Label rollno = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_roll_no");
                    if (rollno.Text.Trim() != "")
                    {
                        roll_No = rollno.Text.Trim();
                    }
                    if (chk.Checked == true)
                    {
                        updateQry = "UPDATE TokenDetails SET Renew_Days =" + RenewDays + " WHERE Roll_No ='" + roll_No + "' ";
                        update = d2.update_method_wo_parameter(updateQry, "Text");
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Maximum renewal count are updated for the selected students";
                        btngo_Click(sender, e);
                    }
                }
            }
            if (rblstaff.SelectedIndex == 1)
            {
                double RenewDays = 0;
                double intcount = 0;
                int IntSel = 0;
                string updateQry = string.Empty;
                int update = 0;
                string staffCode = string.Empty;
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    TextBox Renew_Days = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_renewdays");
                    if (Renew_Days.Text.Trim() != "")
                    {
                        RenewDays = Convert.ToDouble(Renew_Days.Text.Trim());
                    }
                    if (chk.Checked == true)
                    {
                        intcount = intcount + RenewDays;
                        IntSel = IntSel + 1;
                    }
                }
                if (IntSel == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Select the Staff";

                }
                if (intcount == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Enter number of renewal times";
                }
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    TextBox Renew_Days = (TextBox)grdindividual.Rows[RowCnt].FindControl("txt_renewdays");
                    if (Renew_Days.Text.Trim() != "")
                    {
                        RenewDays = Convert.ToDouble(Renew_Days.Text.Trim());
                    }
                    Label rollno = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_roll_no");
                    if (rollno.Text.Trim() != "")
                    {
                        staffCode = rollno.Text.Trim();
                    }
                    if (chk.Checked == true)
                    {
                        updateQry = "UPDATE TokenDetails SET Renew_Days =" + RenewDays + " WHERE Roll_No ='" + staffCode + "' ";
                        update = d2.update_method_wo_parameter(updateQry, "Text");
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Maximum renewal count are updated for the selected Staffs";
                        btngo_Click(sender, e);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "individualstudent");
        }
    }

    protected void BtnAllowBkBnk_Click(object sender, EventArgs e)
    {
        try
        {
            string updateQry = string.Empty;
            int update = 0;
            string rollNo = string.Empty;
            int IntSel = 0;
            if (rblstaff.SelectedIndex == 0)
            {
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    if (chk.Checked == true)
                    {
                        IntSel = IntSel + 1;
                    }
                }
                if (IntSel == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Select the student";
                }
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    Label rollno = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_roll_no");
                    if (rollno.Text.Trim() != "")
                    {
                        rollNo = rollno.Text.Trim();
                    }
                    if (chk.Checked == true)
                    {
                        updateQry = "update tokendetails set AllowAllBook =1 where roll_no = '" + rollNo + "' ";
                        update = d2.update_method_wo_parameter(updateQry, "Text");
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "updated sucessfully";
                        btngo_Click(sender, e);
                    }
                }
            }
            if (rblstaff.SelectedIndex == 1)
            {
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    if (chk.Checked == true)
                    {
                        IntSel = IntSel + 1;
                    }
                }
                if (IntSel == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Select the Staff";
                }
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    Label rollno = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_roll_no");
                    if (rollno.Text.Trim() != "")
                    {
                        rollNo = rollno.Text.Trim();
                    }
                    if (chk.Checked == true)
                    {
                        updateQry = "update tokendetails set AllowAllBook =1 where roll_no = '" + rollNo + "' ";
                        update = d2.update_method_wo_parameter(updateQry, "Text");
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "updated sucessfully";
                        btngo_Click(sender, e);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "individualstudent");
        }
    }

    protected void BtnRemoveBkBnk_Click(object sender, EventArgs e)
    {
        try
        {
            string updateQry = string.Empty;
            int update = 0;
            string rollNo = string.Empty;
            int IntSel = 0;
            if (rblstaff.SelectedIndex == 0)
            {
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    if (chk.Checked == true)
                    {
                        IntSel = IntSel + 1;
                    }
                }
                if (IntSel == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Select the student";
                }
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    Label rollno = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_roll_no");
                    if (rollno.Text.Trim() != "")
                    {
                        rollNo = rollno.Text.Trim();
                    }
                    if (chk.Checked == true)
                    {
                        updateQry = "update tokendetails set AllowAllBook =0 where roll_no = '" + rollNo + "' ";
                        update = d2.update_method_wo_parameter(updateQry, "Text");
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "updated sucessfully";
                        btngo_Click(sender, e);
                    }
                }
            }
            if (rblstaff.SelectedIndex == 1)
            {
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    if (chk.Checked == true)
                    {
                        IntSel = IntSel + 1;
                    }
                }
                if (IntSel == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Select the Staff";
                }
                foreach (GridViewRow gvrow in grdindividual.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    Label rollno = (Label)grdindividual.Rows[RowCnt].FindControl("lbl_roll_no");
                    if (rollno.Text.Trim() != "")
                    {
                        rollNo = rollno.Text.Trim();
                    }
                    if (chk.Checked == true)
                    {
                        updateQry = "update tokendetails set AllowAllBook =0 where roll_no = '" + rollNo + "' ";
                        update = d2.update_method_wo_parameter(updateQry, "Text");
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "updated sucessfully";
                        btngo_Click(sender, e);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "individualstudent");
        }
    }

    protected void grdindividual_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            ((CheckBox)e.Row.FindControl("allchk")).Attributes.Add("onclick",
                "javascript:SelectAll('" +
                ((CheckBox)e.Row.FindControl("allchk")).ClientID + "')");

            //GridView HeaderGrid = (GridView)sender;
            //GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            //TableCell HeaderCell = new TableCell();

            //HeaderCell.Text = "";
            //HeaderCell.ColumnSpan = 4;
            //HeaderGridRow.Cells.Add(HeaderCell);

            //TableCell HeaderCellAvail = new TableCell();
            //HeaderCellAvail = new TableCell();
            //HeaderCellAvail.Text = "Available Cards";
            //HeaderCellAvail.ColumnSpan = 5;
            //HeaderCellAvail.HorizontalAlign = HorizontalAlign.Center;

            //HeaderGridRow.Cells.Add(HeaderCellAvail);
            //TableCell HeaderCell1 = new TableCell();

            //HeaderCell1.Text = "";
            //HeaderCell1.ColumnSpan = 6;
            //HeaderGridRow.Cells.Add(HeaderCell1);
            //grdindividual.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ((rblstaff.SelectedIndex == 0 && ddlcard.SelectedItem.Text == "Book Bank") || rblstaff.SelectedIndex == 1)
            {
                e.Row.Cells[12].Enabled = true;
                e.Row.Cells[13].Enabled = true;
                e.Row.Cells[14].Enabled = true;
            }
            else
            {
                e.Row.Cells[12].Enabled = false;
                e.Row.Cells[13].Enabled = false;
                e.Row.Cells[14].Enabled = false;
            }
        }
    }

    //protected void grdindividual_OnRowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        for (int i = 1; i < e.Row.Cells.Count; i++)
    //        {
    //            TableCell cell = e.Row.Cells[i];
    //            cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
    //            cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
    //            cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
    //               , SelectedGridCellIndex.ClientID, i
    //               , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
    //        }
    //    }
    //}

    //protected void grdindividual_SelectedIndexChanged(Object sender, EventArgs e)
    //{
    //    var grid = (GridView)sender;
    //    GridViewRow selectedRow = grid.SelectedRow;
    //    int rowIndex = grid.SelectedIndex;
    //    int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
    //}

}

